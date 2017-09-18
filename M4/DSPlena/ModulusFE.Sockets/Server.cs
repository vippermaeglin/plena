using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace ModulusFE.Sockets
{
    public class Server : IDisposable, IServerEvents
    {
        public class ServerException : Exception
        {
            public ServerException(string message)
                : base(message)
            {
            }
        }

        public delegate void LogHandler(string format, params object[] args);
        public LogHandler Log;

        private void DoLog(string format, params object[] args)
        {
            if (Log != null)
                Log(format, args);
        }

        private TcpListener _server;
        private TcpListener _serverHistory;
        private TcpListener _serverRT;
        public EndPoint _localEndPoint;
        private bool _stoped;
        private bool _disposed;
        //this thread is used to listen to incoming connection
        private Thread _acceptThread;
        //this thread is used to listen to incoming connection
        private Thread _acceptThreadHistory;
        //this thread is used to listen to incoming connection
        private Thread _acceptThreadRT;
        private object _syncHandle = new object();

        #region Events
        public event Client.DataReceivedHandler DataReceived;
        public delegate void ClientNotAuthenticatedHandler(Server sender, string clientId);
        public event ClientNotAuthenticatedHandler ClientNotAuthenticated;

        #endregion

        private readonly List<Client> _clients = new List<Client>(100);

        private const int MaximumClientCount = 1000;
        private const int BufferSizeForAClient = 1024;
        private readonly BufferManager _bufferManager;  //// represents a large reusable set of buffers for all socket operations
        private readonly Pool<Client> _subscribersPool;

        private readonly Dictionary<int, Type> _structures = new Dictionary<int, Type>();

        public Server()
        {
            //multiple by 2, one buffer for receive one for send
            _bufferManager = new BufferManager(2 * BufferSizeForAClient * MaximumClientCount, BufferSizeForAClient);
            _subscribersPool = new Pool<Client>(MaximumClientCount);

            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds 
            // against memory fragmentation
            _bufferManager.InitBuffer();

            // preallocate pool of Subscribers objects
            for (int i = 0; i < MaximumClientCount; i++)
            {
                Client client = new Client(){Id = Guid.NewGuid().ToString()};
                client.OnException += (mux, exception, okToContinue) =>
                                          {
                                              ClientDisconnected(this,mux.Id);
                                              //Exception(this,exception,"client.OnException");
                                          };


                int offsetForReceive;
                byte[] bufferForReceive = _bufferManager.GetBuffer(out offsetForReceive);

                int offsetForSend;
                byte[] bufferForSend = _bufferManager.GetBuffer(out offsetForSend);

                client.SetBuffers(bufferForReceive, offsetForReceive, bufferForSend, offsetForSend);
                client.SetParserStructs(_structures);

                _subscribersPool.Push(client);
            }

            _stoped = true;
        }

        ~Server()
        {
            Dispose(false);
        }

        public bool IsStarted
        {
            get { return !_stoped; }
        }

        public void Start(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            _stoped = false;

            DoLog("TCP/Server Main starting");
            _acceptThread = new Thread(DoListen);
            _acceptThread.Name = "_acceptThread";
            _acceptThread.IsBackground = true;
            _acceptThread.Start(localEndPoint);

            IPEndPoint localEndPointHistory = new IPEndPoint(localEndPoint.Address, localEndPoint.Port + 1);
            _acceptThreadHistory = new Thread(DoListenHistory);
            _acceptThreadHistory.Name = "_acceptThreadHistory";
            _acceptThreadHistory.IsBackground = true;
            _acceptThreadHistory.Start(localEndPointHistory);

            
            IPEndPoint localEndPointRT = new IPEndPoint(localEndPointHistory.Address, localEndPointHistory.Port + 1);
            _acceptThreadRT = new Thread(DoListenRT);
            _acceptThreadRT.Name = "_acceptThreadRT";
            _acceptThreadRT.IsBackground = true;
            _acceptThreadRT.Start(localEndPointRT);
        }


        /*
        public void Start(IPEndPoint localEndPoint)
        {
            _localEndPoint = localEndPoint;
            _stoped = false;

            DoLog("TCP/Server Main starting");
            _acceptThread = new Thread(DoListen);
            _acceptThread.Name = "_acceptThread";
            _acceptThread.IsBackground = true;
            _acceptThread.Start(localEndPoint);

            IPEndPoint localEndPointHistory = new IPEndPoint(localEndPoint.Address, localEndPoint.Port+1);
            _acceptThreadHistory = new Thread(DoListenHistory);
            _acceptThreadHistory.Name = "_acceptThreadHistory";
            _acceptThreadHistory.IsBackground = true;
            _acceptThreadHistory.Start(localEndPointHistory);

            IPEndPoint localEndPointRT = new IPEndPoint(localEndPointHistory.Address, localEndPointHistory.Port+1);
            _acceptThreadRT = new Thread(DoListenRT);
            _acceptThreadRT.Name = "_acceptThreadRT";
            _acceptThreadRT.IsBackground = true;
            _acceptThreadRT.Start(localEndPointRT);
        }
        */

        public void Start(string ipAddress, int port)
        {
            if (string.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException("ipAddress");
            IPAddress ip = IPAddress.Parse(ipAddress);
            if (ip == null)
                throw new ArgumentNullException("");
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);

            Start(ipEndPoint);
        }

        public void Stop()
        {
            Stoping(this);

            DoStop(true);

            Stoped(this);
        }

        /// <summary>
        /// Register a structure that is going to be sent to client and back
        /// </summary>
        public void RegisterStructure(int Id, Type structureType)
        {
            if (_structures.ContainsKey(Id))
                throw new ServerException(string.Format("Structure with Id = {0} already registered.", Id));

            if (!typeof(IParserStruct).IsAssignableFrom(structureType))
                throw new ServerException(string.Format("Can't register type. Given type is not derived from IParserStruct."));

            _structures.Add(Id, structureType);
        }

        private void DoListen(object state)
        {
            try
            {
                Starting(this);

                _server = new TcpListener((IPEndPoint)state);

                _server.Start();
                
                DoLog("TCP/Server started. Listening for clients");

                Started(this);

                while (!_stoped)
                {
                    DoLog("Start listening for a client.");
                    try
                    {
                        Socket clientSocket = _server.AcceptSocket();
                        Debug.WriteLine("[ThreadMain] Accepted socket with IP {0} on port {1}", ((IPEndPoint)clientSocket.RemoteEndPoint).Address, ((IPEndPoint)clientSocket.LocalEndPoint).Port);

                        DoLog("Incoming connection accepted.");
                        AddClient(clientSocket, SocketsType.Main);
                    }
                    catch (Exception ex)
                    {
                        DoLog("[SocketServer] AcceptSocket failed. Error: " + ex);
                    }
                }
                DoLog("[SocketServer] Stoped.");
            }
            catch (Exception ex)
            {
                if (!_stoped)
                    Exception(this, ex, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DoListenHistory(object state)
        {
            try
            {
                Starting(this);

                _serverHistory = new TcpListener((IPEndPoint)state);

                _serverHistory.Start();

                DoLog("TCP/Server started. Listening for clients");

                Started(this);

                while (!_stoped)
                {
                    DoLog("Start listening for a client.");
                    try
                    {
                        Socket clientSocket = _serverHistory.AcceptSocket();
                        Debug.WriteLine("[ThreadHistory] Accepted socket with IP: {0} on port {1}", ((IPEndPoint)clientSocket.RemoteEndPoint).Address, ((IPEndPoint)clientSocket.LocalEndPoint).Port);

                        DoLog("Incoming connection accepted.");
                        AddClient(clientSocket, SocketsType.History);
                    }
                    catch (Exception ex)
                    {
                        DoLog("[SocketServer] AcceptSocket failed. Error: " + ex);
                    }
                }
                DoLog("[SocketServer] Stoped.");
            }
            catch (Exception ex)
            {
                if (!_stoped)
                    Exception(this, ex, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void DoListenRT(object state)
        {
            try
            {
                Starting(this);

                _serverRT = new TcpListener((IPEndPoint)state);

                _serverRT.Start();

                DoLog("TCP/Server started. Listening for clients");

                Started(this);

                while (!_stoped)
                {
                    DoLog("Start listening for a client.");
                    try
                    {
                        Socket clientSocket = _serverRT.AcceptSocket();
                        Debug.WriteLine("[ThreadRT] Accepted socket with IP: {0} on port {1}", ((IPEndPoint)clientSocket.RemoteEndPoint).Address, ((IPEndPoint)clientSocket.LocalEndPoint).Port);

                        DoLog("Incoming connection accepted.");
                        AddClient(clientSocket, SocketsType.RealTime);
                    }
                    catch (Exception ex)
                    {
                        DoLog("[SocketServer] AcceptSocket failed. Error: " + ex);
                    }
                }
                DoLog("[SocketServer] Stoped.");
            }
            catch (Exception ex)
            {
                if (!_stoped)
                    Exception(this, ex, MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AddClient(Socket socket, SocketsType socketType)
        {
            lock (_syncHandle)
            {
                Client client;
                if (socketType != SocketsType.Main)
                {
                    int index = -1;
                    index = _clients.IndexOf(_clients.Find(c => ((IPEndPoint)socket.RemoteEndPoint).Address.ToString() == c.IPadress.ToString() && (((IPEndPoint)socket.LocalEndPoint).Port == c.Port + 1 || ((IPEndPoint)socket.LocalEndPoint).Port == c.Port + 2)));
                    //index = _clients.Count - 1;
                    if(index == null || index== -1)
                    {
                        DoLog("ERROR: Socket dont have main client or that's just a MUX Connector!");
                        Debug.WriteLine("[AddClient] ERROR: Socket dont have main client or that's just a MUX Connector!");
                        Debug.WriteLine("\tSizeof(_clients) = {0}\n\t_clients.IP = {1}\n\tsocket.IP = {2}\n\tsocket.RemotPort={3}\n\tsocket.LocalPort={4}\n\t_client.Port={5}", _clients.Count, _clients[0].IPadress, ((IPEndPoint)socket.RemoteEndPoint).Address, ((IPEndPoint)socket.RemoteEndPoint).Port, ((IPEndPoint)socket.LocalEndPoint).Port,_clients[0].Port);
                        return;
                    }


                    if (socketType == SocketsType.History)
                    {
                        if (!_clients[index].ConnectedHistory)
                        {
                            _clients[index].AttachHistory(socket);
                            Debug.WriteLine("[AddClient] socketHistory attached to client {0}", _clients[index].Id);
                        }
                        else Debug.WriteLine("[AddClient] socketHistory already exists, not attached to client {0}", _clients[index].Id);
                    }
                    else 
                    {
                        if (!_clients[index].ConnectedRT)
                        {
                            _clients[index].AttachRT(socket);
                            Debug.WriteLine("[AddClient] socketRT attached to client {0}", _clients[index].Id);
                        }
                        else Debug.WriteLine("[AddClient] socketRT already exists, not attached to client {0}", _clients[index].Id);
                    }


                    //start receiving on this new socket
                    DoReceiveAsync(_clients[index], socketType);


                }
                else
                {
                    client = _subscribersPool.Pop();



                    client.Attach(socket);
                    Debug.WriteLine("[AddClient] socket attached as client {0}",client.Id);
                    SetDesiredKeepAlive(socket);

                    if (!client.HasSubscriptionToData)
                    {
                        client.DataReceived += ClientOnDataReceived;
                        client.HasSubscriptionToData = true;
                    }

                    _clients.Add(client);

                    IncommingConnection(this, (IPEndPoint)socket.RemoteEndPoint);



                    //start receiving on this new socket
                    DoReceiveAsync(client,socketType);


                }

            }
        }

        private void ClientOnDataReceived(Client client, List<IParserStruct> structure)
        {
            if (DataReceived != null)
                DataReceived(client, structure);
        }

        private readonly Type[] _beginReceiveExceptions = new[]
                                                        {
                                                          typeof (ArgumentNullException), 
                                                          typeof (SocketException),
                                                          typeof (ObjectDisposedException),
                                                          typeof (ArgumentOutOfRangeException)
                                                        };
        private void DoReceiveAsync(Client client, SocketsType socketType)
        {
            try
            {
                //Debug.WriteLine("Server. Client::BeginReceive");
                client.BeginReceive(BufferSizeForAClient, result => { ClientReceive_CB(result, socketType); }, socketType);
            }
            catch (Exception ex)
            {

                Debug.WriteLine("EXCEPTION: DoReceiveAsync : " + ex.Message);

                if (CheckException(ex, _beginReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        private readonly Type[] _endReceiveExceptions = new[]
                                                      {
                                                        typeof (ArgumentNullException),
                                                        typeof (InvalidOperationException),
                                                        typeof (SocketException),
                                                        typeof (ObjectDisposedException)
                                                      };
        private void ClientReceive_CB(IAsyncResult result, SocketsType socketType)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                //Debug.WriteLine("Server. Client::EndReceive");
                if (socketType == SocketsType.History)
                {
                    if (!client.EndReceiveHistory(result))
                    {
                        CloseClientSocket(client);
                        return;
                    }
                }
                else if (socketType == SocketsType.RealTime)
                {
                    if (!client.EndReceiveRT(result))
                    {
                        CloseClientSocket(client);
                        return;
                    }
                }
                else
                {
                    if (!client.EndReceive(result))
                    {
                        CloseClientSocket(client);
                        return;
                    }

                }
                if (!_stoped)
                {
                    DoReceiveAsync(client, socketType);
                }
                else
                {
                    DoLog("[SocketServer] Can't Start DoReceiveAsync on client cause stoped.");
                }
            }
            catch (BufferParser.BufferParserException ex)
            {
                Debug.WriteLine("BufferParser had an exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: ClientReceive_CB : " + ex.Message);

                if (CheckException(ex, _endReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        public void DoSendAsync(string clientId, IEnumerable<IParserStruct> structures)
        {
            Client client = _clients.Find(client1 => client1.Id == clientId);
            if (client == null)
            {
                ClientNotAuthenticated(this, clientId);
                return;
            }
            DoSendAsync(client, structures);
        }

        public void DoSendAsync(Client client, IEnumerable<IParserStruct> structures)
        {
            try
            {
                if (client.Disposed || !client.ConnectedMain)
                {
                    Debug.WriteLine(
                      string.Format("[SocketServer] Can't DOSendAsync - Client '{0}' is Disposed '{1}' or Disconnected '{2}'",
                                    client.Id, client.Disposed, !client.ConnectedMain));
                    return;
                }

                foreach (var @struct in structures)
                {
                    switch (@struct.Id)
                    {
                        case 1: //Authentication
                        case 2: //AuthenticationAnswer
                        case 3: //Symbol
                        case 4: //SymbolRequest
                        case 5: //Subscribe
                        case 7: //HistoryRequest
                        case 9: //Ping
                        case 10: //Snapshot
                            client.BeginSend(@struct, ClientSend_CB, SocketsType.Main);
                            break;
                        case 8: //BarData
                            client.BeginSend(@struct, ClientSend_CB_History, SocketsType.History); //TESTES
                            break;
                        case 6: //TickData
                            client.BeginSend(@struct, ClientSend_CB_RT, SocketsType.RealTime);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: DoSendAsync : " + ex.Message);

                if (CheckException(ex, _beginReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        private void ClientSend_CB(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                if (client.EndSend(result) == 0)
                {
                    Debug.WriteLine(string.Format("[ServerSocket] Could not send data to client '{0}'. Closing it.", client.Id));
                    CloseClientSocket(client);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: ClientSend_CB : " + ex.Message);

                if (CheckException(ex, _endReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        private void ClientSend_CB_History(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                if (client.EndSendHistory(result) == 0)
                {
                    Debug.WriteLine(string.Format("[ServerSocket] Could not send data to client '{0}'. Closing it.", client.Id));
                    CloseClientSocket(client);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: ClientSend_CB : " + ex.Message);

                if (CheckException(ex, _endReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        private void ClientSend_CB_RT(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                if (client.EndSendRT(result) == 0)
                {
                    Debug.WriteLine(string.Format("[ServerSocket] Could not send data to client '{0}'. Closing it.", client.Id));
                    CloseClientSocket(client);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: ClientSend_CB : " + ex.Message);

                if (CheckException(ex, _endReceiveExceptions))
                {
                    CloseClientSocket(client);
                    return;
                }
                throw;
            }
        }

        public void Broadcast(IParserStruct @struct)
        {
            List<Client> _clientsToDelete = new List<Client>();
            foreach (Client client in _clients)
            {
                Client cl = client;
                try
                {
                    cl.BeginSend(@struct,
                                 result =>
                                 {
                                     try
                                     {
                                         int res = cl.EndSend(result);
                                         if (res == -1)
                                             _clientsToDelete.Add(cl);
                                     }
                                     catch (Exception ex)
                                     {
                                         if (!CheckException(ex, _endReceiveExceptions))
                                             throw;
                                         _clientsToDelete.Add(cl);
                                     }
                                 }, SocketsType.Main);
                }
                catch (Exception ex)
                {
                    if (!CheckException(ex, _endReceiveExceptions))
                        throw;
                    _clientsToDelete.Add(cl);
                }
            }

            lock (_clients)
            {
                foreach (var client in _clientsToDelete)
                {
                    _clients.Remove(client);
                    CloseClientSocket(client);
                }
            }
        }

        public void DeleteClient(Client client)
        {
            lock (_syncHandle)
            {
                _clients.Remove(client);
                CloseClientSocket(client);
            }
        }

        private void CloseClientSocket(Client client)
        {
            //return;
            lock (_syncHandle)
            {
                try
                {
                    //raise the event 
                    ClientDisconnected(this, client.Id);

                    //close the socket associated with this client
                    client.Close();

                    //remove from list of clients
                    _clients.Remove(client);

                    //push client back to client's pool to be reused later for another connections
                    _subscribersPool.Push(client);
                }
                catch (Exception ex)
                {
                    DoLog("[SocketServer] CloseClientSocket error: " + ex.Message);
                }
            }
        }

        private static bool CheckException(Exception toCheck, IEnumerable<Type> checkFor)
        {
            Type t = toCheck.GetType();

            foreach (Type exception in checkFor)
            {
                if (exception == t)
                    return true;
            }
            return false;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(false);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            DoStop(disposing);

            _disposed = true;
        }

        private void DoStop(bool disposing)
        {
            _stoped = true;

            if (disposing)
            {
                //stop server
                if (_server != null)
                    _server.Stop();
                if (_serverHistory != null)
                    _serverHistory.Stop();
                if (_serverRT != null)
                    _serverRT.Stop();
                /*try
                {
                    //stop thread, no more incomming connections
                    if (_acceptThread != null)
                    {
                        //_acceptThread.Abort();
                        _acceptThread.Join();
                        _acceptThread = null;
                    }

                    //stop thread, no more incomming connections
                    if (_acceptThreadHistory != null)
                    {
                        _acceptThreadHistory.Abort();



                            //I know is bad, but there is no way to break the AcceptSocket call
                        _acceptThread.Join();
                        _acceptThreadHistory = null;
                    }



                    //stop thread, no more incomming connections
                    if (_acceptThreadRT != null)
                    {



                        _acceptThreadRT.Abort(); //I know is bad, but there is no way to break the AcceptSocket call
                        _acceptThreadRT.Join();
                        _acceptThreadRT = null;
                    }

                }
                catch(Exception ex)



                {
                    
                }*/





                //close unclosed clients
                foreach (Client client in _clients)
                {
                    if (!client.Disposed)
                        client.Dispose();
                }
            }
        }

        #endregion

        private static void SetDesiredKeepAlive(Socket socket)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            const uint time = 10000;
            const uint interval = 20000;
            SetKeepAlive(socket, true, time, interval);
        }

        static void SetKeepAlive(Socket s, bool on, uint time, uint interval)
        {
            /* the native structure
            struct tcp_keepalive {
            ULONG onoff;
            ULONG keepalivetime;
            ULONG keepaliveinterval;
            };
            */

            // marshal the equivalent of the native structure into a byte array
            uint dummy = 0;
            var inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)time).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)interval).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            // of course there are other ways to marshal up this byte array, this is just one way

            // call WSAIoctl via IOControl
            int ignore = s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }

        #region Implementation of IServerEvents

        public event StartingHandler Starting = delegate { };
        public event StartingHandler Started = delegate { };
        public event IncommingConnectionHandler IncommingConnection = delegate { };
        public event ExceptionHandler Exception = delegate { };
        public event ClientDisconnectedHandler ClientDisconnected = delegate { };
        public event MessageHandler Message = delegate { };
        public event StopingHandler Stoping = delegate { };
        public event StopedHandler Stoped = delegate { };
        public event AuthenticateClientHandler AuthenticateClient;

        #endregion
    }
}
