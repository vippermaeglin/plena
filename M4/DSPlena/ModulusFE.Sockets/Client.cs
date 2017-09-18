using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace ModulusFE.Sockets
{
    public enum SocketsType
    {
        Main,
        History,
        RealTime
    }
    public class Client : IDisposable
    {
        //DEBUG
        private Stopwatch timeWatch = new Stopwatch();
        private long lastTimeWatch = 0;
        private long countRead = 0;

        private System.Timers.Timer FlushStructsHistory = new Timer(100);

        private bool _disposed;
        private bool _closed;
        private readonly object _sendSync = new object();
        private readonly object _recvSync = new object();

        public delegate void DataReceivedHandler(Client sender, List<IParserStruct> structure);
        public event DataReceivedHandler DataReceived;

        public delegate void OnExceptionHandler(Client sender, Exception exception, bool? okToContinue);
        public event OnExceptionHandler OnException;

        public delegate void LogHandler(Client sender, string message);
        public event LogHandler OnLog;

        public event Action<Client> OnConnecting;

        private void InvokeOnConnecting(Client sender)
        {
            Action<Client> connecting = OnConnecting;
            if (connecting != null) connecting(sender);
        }

        public event Action<Client> OnConnected;

        private void InvokeOnConnected(Client sender)
        {
            Action<Client> connected = OnConnected;
            if (connected != null) connected.BeginInvoke(sender,EndInvokeConnected,null);
        }

        private void EndInvokeConnected(IAsyncResult result)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            var invokedMethod = (Action<Client>)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(result);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }

        public event Action<Client> OnConnectedAux;

        private void InvokeOnConnectedAux(Client sender)
        {
            Action<Client> connected = OnConnectedAux;
            if (connected != null) connected.BeginInvoke(sender, EndInvokeConnectedAux, null);
        }

        private void EndInvokeConnectedAux(IAsyncResult result)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            var invokedMethod = (Action<Client>)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(result);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }
        private void InvokeOnException(Exception exception, bool? okToContinue)
        {
            //MessageBox.Show(exception.Message);
            OnExceptionHandler exceptionHandler = OnException;
            if (exceptionHandler != null)
            {
                //Debug.WriteLine(string.Format("[SocketClient]11C CallingExceptionHandler"));
                exceptionHandler.BeginInvoke(this, exception, okToContinue,EndInvokeException,null);
            }
            //if (exception.Message == "Unable to open the database file") okToContinue = true;

            //Debug.WriteLine(string.Format("[SocketClient]11D ok to continue"));
            if (okToContinue == false)
            {
                //Debug.WriteLine(string.Format("[SocketClient]11E Dispose()"));
                Dispose();
            }
            //Debug.WriteLine(string.Format("[SocketClient]11F Out Invoke"));
        }

        private void EndInvokeException(IAsyncResult result)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            var invokedMethod = (OnExceptionHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(result);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }

        private void InvokeLog(string message)
        {
            //MessageBox.Show(exception.Message);
            LogHandler logHandler = OnLog;
            if (logHandler != null)
            {
                //Debug.WriteLine(string.Format("[SocketClient]InvokeLog "+message));
                logHandler.BeginInvoke(this, message, EndInvokeLog,null);
            }
        }

        private void EndInvokeLog(IAsyncResult result)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            var invokedMethod = (LogHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(result);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }

        private readonly BufferParser _parser;
        private readonly BufferParser _parserHistory;
        private readonly BufferParser _parserRT;

        private Socket _socket;
        private Socket _socketHistory;
        private Socket _socketRT;

        protected byte[] _bufferForReceive;
        protected int _offsetForReceive;
        protected byte[] _bufferForSend;
        protected int _offsetForSend;

        public string Id;

        private Thread threadConnecting;
        private Thread threadConnectingAux;

        public bool TryingToConnect = false;
        private CancellationTokenSource cancelConnecting = new CancellationTokenSource();

        /// <summary>
        /// Gets or sets whether the client is used as a standalone object, not 
        /// from Server. In this case it will raise exceptions, in case when it is used
        /// from under Server exceptions must be propagated to the Server
        /// </summary>
        public bool StandaloneUsage;

        /// <summary>
        /// For multuthreading synchronization
        /// </summary>
        public object SyncRoot = new object();

        private readonly Dictionary<int, Type> _structures = new Dictionary<int, Type>();

        public Client()
        {
            Init();

            _parser = new BufferParser();
            _parser.StructRead += (parser, structure) =>
                                    {
                                        if (DataReceived != null)
                                            DataReceived(this, structure);
                                    };
            _parserHistory = new BufferParser(){IsHistorical = true};
            _parserHistory.StructRead += (parser, structure) =>
            {
                if (DataReceived != null)
                    DataReceived(this, structure);
            };
            _parserRT = new BufferParser();
            _parserRT.StructRead += (parser, structure) =>
            {
                if (DataReceived != null)
                    DataReceived(this, structure);
            };

            FlushStructsHistory.Elapsed += FlushStructsHistoryEvent;
            FlushStructsHistory.AutoReset = true;
            FlushStructsHistory.Start();

            HasSubscriptionToData = false;
        }

        private void FlushStructsHistoryEvent(object source, ElapsedEventArgs e)
        {
            List<IParserStruct> hist = _parserHistory.GetStructures();
            if (hist.Count > 0)
            {
                DataReceived(this, hist);
            }
        }

        /// <summary>
        /// Register a structure that is going to be sent to client and back
        /// </summary>
        public void RegisterStructure(int structId, Type structureType)
        {
            if (_structures.ContainsKey(structId))
                throw new Server.ServerException(
                  string.Format("Structure with Id = {0} already registered.", Id));

            if (!typeof(IParserStruct).IsAssignableFrom(structureType))
                throw new Server.ServerException(
                  string.Format("Can't register type. Given type is not derived from IParserStruct."));

            _structures.Add(structId, structureType);
        }

        private const int StandaloneBufferSize = 1024;

        /*
        public void TryToConnect(IPAddress ipAddress, int port, bool forceReconnection)
        {
            if (threadConnecting == null)
            {
                threadConnecting = new Thread(() => Connect(ipAddress, port));
                threadConnecting.IsBackground = true;
                threadConnecting.Name = "ThreadConnecting";
                threadConnecting.Start();
            }
            else
            {
                //lock (threadConnecting)
                //{
                    if (!forceReconnection && TryingToConnect)
                    {
                        // Let another thread trying to connect:
                        if (threadConnecting.IsAlive)
                        {
                            //Debug.WriteLine(
                                string.Format(
                                    "[SocketClient]   Aborting new ThreadConnecting, another thread is trying!"));
                            return;
                        }
                    }
                    else //Finish another thread and retry:
                    {
                        TryingToConnect = true;
                        if (threadConnecting != null && threadConnecting.IsAlive)
                        {
                            try
                            {
                                //if(Thread.CurrentThread.Name!=null && !Thread.CurrentThread.Name.Contains("Worker"))threadConnecting.Abort();
                            }
                            catch (Exception ex)
                            {
                            }
                            finally
                            {
                                //cancelConnecting.Cancel();

                                //Debug.WriteLine(string.Format("[SocketClient]   Waiting ThreadConnecting finishing!"));
                                threadConnecting.Join();
                                TryingToConnect = false;
                                if (threadConnecting.IsAlive)
                                    //Debug.WriteLine(string.Format("[SocketClient]   ThreadConnecting NOT finished!"));
                                else //Debug.WriteLine(string.Format("[SocketClient]   ThreadConnecting finished!"));

                            }
                        }
                    }
                    threadConnecting = new Thread(() => Connect(ipAddress, port));
                    threadConnecting.IsBackground = true;
                    threadConnecting.Name = "ThreadConnecting";
                    threadConnecting.Start();
                //}
            }
        }
        */


        public void TryToConnect(IPAddress ipAddress, int port, bool forceReconnection)
        {

            if (!forceReconnection)
            {
                // Let another thread trying to connect:
                if (threadConnecting != null && threadConnecting.IsAlive) return;
            }
            else //Finish another thread and retry:
            {
                if (threadConnecting != null && threadConnecting.IsAlive)
                {
                    try
                    {
                        //threadConnecting.Abort();
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        //cancelConnecting.Cancel();

                        //Debug.WriteLine(string.Format("[SocketClient]   Waiting threadConnectingAux finishing!"));
                        threadConnecting.Join();
                        //if (threadConnecting.IsAlive) //Debug.WriteLine(string.Format("[SocketClient]   threadConnectingAux finished!"));
                        //else //Debug.WriteLine(string.Format("[SocketClient]   threadConnectingAux NOT finished!"));


                    }
                }
            }
            cancelConnecting = new CancellationTokenSource();
            threadConnecting = new Thread(() => Connect(ipAddress, port));
            threadConnecting.IsBackground = true;
            threadConnecting.Name = "threadConnecting";
            threadConnecting.Start();
        }

        public void Connect(IPAddress ipAddress, int port)
        {
            //Debug.WriteLine(string.Format("[ThreadConnect]1 Starting..."));

            _parser.Structs = _structures;
            _closed = true;
            //while (_closed && !cancelConnecting.IsCancellationRequested)
            //{
                try
                {
                    InvokeOnConnecting(this);

                    //Debug.WriteLine(string.Format("[ThreadConnect]2 Connecting MainSocket"));
                    _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _socket.Connect(ipAddress, port);
                    _socket.NoDelay = true;

                    //Debug.WriteLine(string.Format("[ThreadConnect]3 Connecting HistorySocket"));
                    /*_socketHistory = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _socketHistory.Connect(ipAddress, port + 1);
                    _socketHistory.NoDelay = true;

                    //Debug.WriteLine(string.Format("[ThreadConnect]4 Connecting RTSocket"));
                    _socketRT = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _socketRT.Connect(ipAddress, port + 2);
                    _socketRT.NoDelay = true;*/


                    if (StandaloneUsage)
                    {
                        //Debug.WriteLine(string.Format("[ThreadConnect]5 Setting bufers sizes"));
                        _bufferForReceive = new byte[StandaloneBufferSize];
                        _offsetForReceive = 0;
                        _bufferForSend = new byte[StandaloneBufferSize];
                        _offsetForSend = 0;
                        //Debug.WriteLine(string.Format("[ThreadConnect]6 Setting MainsSocket buffers"));
                        _socket.ReceiveBufferSize = StandaloneBufferSize;
                        _socket.SendBufferSize = StandaloneBufferSize;
                        //Debug.WriteLine(string.Format("[ThreadConnect]7 Setting HistorySocket buffers"));
                        //_socketHistory.ReceiveBufferSize = StandaloneBufferSize;
                        //_socketHistory.SendBufferSize = StandaloneBufferSize;
                        //Debug.WriteLine(string.Format("[ThreadConnect]8 Setting RTSocket buffers"));
                        //_socketRT.ReceiveBufferSize = StandaloneBufferSize;
                        //_socketRT.SendBufferSize = StandaloneBufferSize;
                    }

                    //Debug.WriteLine(string.Format("[ThreadConnect]9 InvokeOnConnected()"));
                    InvokeOnConnected(this);

                    //Debug.WriteLine(string.Format("[ThreadConnect]10 Close=False"));
                    _closed = false;

                    _parser.StandaloneUsage = StandaloneUsage;

                }
                catch (Exception exception)
                {
                    //Debug.WriteLine(string.Format("[SocketClient]11A Connect() {0}", exception.Message));

                    if (!StandaloneUsage)
                        throw; //goes to server
                    //remove exception just for tests:
                    InvokeOnException(exception, false);
                    //Debug.WriteLine(string.Format("[SocketClient]11B Bye!", exception.Message));
                    return;
                }
                finally
                {

                    //Debug.WriteLine(string.Format("[ThreadConnect]12 Finally Sleep()"));
                    Thread.Sleep(10000);

                    //Debug.WriteLine(string.Format("[ThreadConnect]13 Good Bye!"));
                }
            //}
        }

        public void TryToConnectAux(IPAddress ipAddress, int port, bool forceReconnection)
        {
            if (!forceReconnection)
            {
                // Let another thread trying to connect:
                if (threadConnectingAux != null && threadConnectingAux.IsAlive)
                {
                    InvokeLog("[SocketClient]threadConnectingAux is Alive, returned!");
                    return;
                }
            }
            else //Finish another thread and retry:
            {
                if (threadConnectingAux != null && threadConnectingAux.IsAlive)
                {
                    try
                    {
                        //threadConnectingAux.Abort();
                    }
                    catch (Exception ex)
                    {
                        InvokeLog("[SocketClient] " + ex.Message);
                    }
                    finally
                    {
                        //cancelConnecting.Cancel();

                        //Debug.WriteLine(string.Format("[SocketClient]   Waiting threadConnectingAux finishing!"));
                        InvokeLog("[SocketClient]threadConnectingAux is Alive, aborting...");
                        threadConnectingAux.Join();
                        if (threadConnectingAux.IsAlive) InvokeLog(string.Format("[SocketClient]   threadConnectingAux finished!"));
                        else InvokeLog(string.Format("[SocketClient]   threadConnectingAux NOT finished!"));

                    }
                }
            }
            cancelConnecting = new CancellationTokenSource();
            threadConnectingAux = new Thread(() => ConnectAux(ipAddress, port));
            threadConnectingAux.IsBackground = true;
            threadConnectingAux.Name = "threadConnectingAux";
            threadConnectingAux.Start();
        }

        public void ConnectAux(IPAddress ipAddress, int port)
        {
            //Debug.WriteLine(string.Format("[ThreadConnectAux]1 Starting..."));
            _parserHistory.Structs = _structures;
            _parserRT.Structs = _structures;
            //_closed = true;
            //while (_closed && !cancelConnecting.IsCancellationRequested)
            //{
            try
            {

                //Debug.WriteLine(string.Format("[ThreadConnectAux]3 Connecting HistorySocket"));
                InvokeLog("[SocketClient]Trying to connect SoketHistory on IP=" + ipAddress + " PORT=" + (port + 1));
                _socketHistory = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socketHistory.Connect(ipAddress, port + 1);
                _socketHistory.NoDelay = true;

                //Debug.WriteLine(string.Format("[ThreadConnectAux]4 Connecting RTSocket"));
                InvokeLog("[SocketClient]Trying to connect SoketRT on IP=" + ipAddress + " PORT=" + (port + 2));
                _socketRT = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socketRT.Connect(ipAddress, port + 2);
                _socketRT.NoDelay = true;


                if (StandaloneUsage)
                {

                    _socketHistory.ReceiveBufferSize = StandaloneBufferSize;
                    _socketHistory.SendBufferSize = StandaloneBufferSize;
                    _socketRT.ReceiveBufferSize = StandaloneBufferSize;
                    _socketRT.SendBufferSize = StandaloneBufferSize;
                }
                
                _parserHistory.StandaloneUsage = StandaloneUsage;
                _parserRT.StandaloneUsage = StandaloneUsage;
                InvokeOnConnectedAux(this);

            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient]11A ConnectAux() {0}", exception.Message));

                if (!StandaloneUsage)
                    throw; //goes to server
                //remove exception just for tests:
                InvokeOnException(exception, false);
                InvokeLog("[SocketClient] "+exception.Message);
                //Debug.WriteLine(string.Format("[SocketClient]11B Bye!", exception.Message));
                return;
            }
            finally
            {

                //Debug.WriteLine(string.Format("[ThreadConnectAux]12 Finally Sleep()"));
                Thread.Sleep(10000);
                //Debug.WriteLine(string.Format("[ThreadConnectAux]13 Good Bye!"));
            }
            //}
        }

        public void StartReceiving(SocketsType socketType)
        {
            if(socketType == SocketsType.RealTime)BeginReceive(StandaloneBufferSize, Receive_CB_RT, socketType);
            else if (socketType == SocketsType.History)
            {
                BeginReceive(StandaloneBufferSize, Receive_CB_History, socketType);
            }

            else { BeginReceive(StandaloneBufferSize, Receive_CB, socketType); }
        }

        private void Receive_CB(IAsyncResult result)
        {
            if (!EndReceive(result))
            {
                //Debug.WriteLine("[Client Socket] EndReceive said NO. Can't Receive more.");
                //InvokeOnConnected(this);
                return;
            }
            StartReceiving(SocketsType.Main);
        }
        
        private void Receive_CB_History(IAsyncResult result)
        {
            if (!EndReceiveHistory(result))
            {
                //Debug.WriteLine("[Client Socket] EndReceiveHistory said NO. Can't Receive more.");
                //InvokeOnConnected(this);
                return;
            }
            StartReceiving(SocketsType.History);
        }


        private void Receive_CB_RT(IAsyncResult result)
        {
            if (!EndReceiveRT(result))
            {
                //Debug.WriteLine("[Client Socket] EndReceiveHistory said NO. Can't Receive more.");
                //InvokeOnConnected(this);
                return;
            }
            StartReceiving(SocketsType.RealTime);
        }

        public void Write(byte[] buffer, int offset, int count, SocketsType type)
        {
            switch(type)

            {
                case SocketsType.Main:
                    lock (_parser)
                    {
                        _parser.Write(buffer, offset, count);
                    }
                    break;
                case SocketsType.History:
                    lock (_parserHistory)
                    {
                        _parserHistory.Write(buffer, offset, count);
                    }
                    break;
                case SocketsType.RealTime:
                    lock (_parserRT)
                    {
                        _parserRT.Write(buffer, offset, count);
                    }
                    break;
            }
        }

        public void SetBuffers(byte[] bufferForReceive, int offsetForReceive,
          byte[] bufferForSend, int offsetForSend)
        {
            _offsetForReceive = offsetForReceive;
            _bufferForReceive = bufferForReceive;

            _offsetForSend = offsetForSend;
            _bufferForSend = bufferForSend;
        }

        public void Init()
        {
            _disposed = false;
        }

        public bool Disposed
        {
            get { return _disposed; }
        }

        public bool Closed
        {
            get { return _closed; }
        }

        public bool ConnectedMain
        {
            get { return _socket != null && _socket.Connected  && !_closed && !_disposed; }
        }

        public bool ConnectedHistory
        {
            get { return _socketHistory != null && _socketHistory.Connected && !_closed && !_disposed; }
        }

        public bool ConnectedRT

        {
            get { return _socketRT != null && _socketRT.Connected && !_closed && !_disposed; }
        }
        
        // Used only as standalon for Server:
        public void Attach(Socket socket)
        {
            _disposed = false;
            _closed = false;
            _socket = socket;

            _parser.StandaloneUsage = false;
        }

        // Used only as standalon for Server:
        public void AttachHistory(Socket socket)
        {
            _socketHistory = socket;

            _parserHistory.StandaloneUsage = false;
        }

        // Used only as standalon for Server:
        public void AttachRT(Socket socket)
        {
            _socketRT = socket;

            _parserRT.StandaloneUsage = false;
        }

        public void BeginReceive(int bufferSize, AsyncCallback callback, SocketsType socketType)
        {
            if (Disposed || _closed)
            {
                //Debug.WriteLine(string.Format("[SocketClient] Can't BeginReceive struct cause Disposed='{0}' or Close='{1}'", Disposed, _closed));
                return;
            }
            ////Debug.WriteLine("Client: BeginReceive");
            try
            {
                SocketError error;
                if(socketType==SocketsType.History)
                {
                    //InvokeLog("[SocketClient] BeginReceive History");
                    _socketHistory.BeginReceive(_bufferForReceive, _offsetForReceive, bufferSize, SocketFlags.None, out error, callback, this);
                }
                else if (socketType == SocketsType.RealTime)
                {
                    //InvokeLog("[SocketClient] BeginReceive RT");
                    _socketRT.BeginReceive(_bufferForReceive, _offsetForReceive, bufferSize, SocketFlags.None, out error, callback, this);
                }
                else
                {
                    //InvokeLog("[SocketClient] BeginReceive Main");
                    _socket.BeginReceive(_bufferForReceive, _offsetForReceive, bufferSize, SocketFlags.None, out error, callback, this);
                }

                if (error != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: "+error.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] BeginReceive finished with error '{0}'", error));
                    if (error == SocketError.ConnectionReset || error == SocketError.ConnectionAborted || error == SocketError.NotConnected || error == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+error), true);
                }
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!StandaloneUsage)
                    throw;
                InvokeOnException(exception, true);

            }
        }

        public bool EndReceive(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            if (client.Disposed || client._closed)
            {
                //Debug.WriteLine(string.Format("[SocketClient] Can't EndReceive struct cause Disposed='{0}' or Close='{1}'", Disposed, _closed));
                return true;
            }

            //      //Debug.WriteLine("Client: EndReceive");
            try
            {
                SocketError socketError;
                int read = client._socket.EndReceive(result, out socketError);
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not receive. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), true);
                    return false;
                }
                if (read <= 0)
                {
                    //Debug.WriteLine(string.Format("[SocketClient] Received a negative quantity '{0}'", read));
                    return false;
                }

                client._parser.Write(client._bufferForReceive, client._offsetForReceive, read);
                client._parser.Parse();
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }

            return true;
        }
        
        public bool EndReceiveHistory(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            if (client.Disposed || client._closed)
            {
                //Debug.WriteLine(string.Format("[SocketClient] Can't EndReceiveHistory struct cause Disposed='{0}' or Close='{1}'", Disposed, _closed));
                return true;
            }

            //      //Debug.WriteLine("Client: EndReceive");
            try
            {
                countRead++;
                /*InvokeLog("socket.EndRecieve starting read(" + countRead + ") after " + (timeWatch.ElapsedMilliseconds-lastTimeWatch));
                lastTimeWatch = timeWatch.ElapsedMilliseconds;*/
                SocketError socketError;
                int read = client._socketHistory.EndReceive(result, out socketError);
                FlushStructsHistory.Stop();
                FlushStructsHistory.Start();
                /*InvokeLog("socket.EndRecieve finish read(" + countRead + ")=" + read + " after " + (timeWatch.ElapsedMilliseconds - lastTimeWatch));
                lastTimeWatch = timeWatch.ElapsedMilliseconds;*/
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not receive. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), false);
                    return false;
                }
                if (read <= 0)
                {
                    //Debug.WriteLine(string.Format("[SocketClient] Received a negative quantity '{0}'", read));
                    return false;
                }
                FlushStructsHistory.Stop();
                FlushStructsHistory.Start();
                /*InvokeLog("socket.EndRecieve starting Write() after " + (timeWatch.ElapsedMilliseconds - lastTimeWatch));
                lastTimeWatch = timeWatch.ElapsedMilliseconds;*/

                client._parserHistory.Write(client._bufferForReceive, client._offsetForReceive, read);
                /*InvokeLog("socket.EndRecieve starting Parse() with stream=" + client._parserHistory.Size() + " after " + (timeWatch.ElapsedMilliseconds - lastTimeWatch));
                lastTimeWatch = timeWatch.ElapsedMilliseconds;*/
                client._parserHistory.Parse();
                /*InvokeLog("socket.EndRecieve finished Parse() with stream=" + client._parserHistory.Size() + " after " + (timeWatch.ElapsedMilliseconds - lastTimeWatch));
                lastTimeWatch = timeWatch.ElapsedMilliseconds;*/

            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }

            return true;
        }

        public bool EndReceiveRT(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            if (client.Disposed || client._closed)
            {
                //Debug.WriteLine(string.Format("[SocketClient] Can't EndReceive struct cause Disposed='{0}' or Close='{1}'", Disposed, _closed));
                return true;
            }

            //      //Debug.WriteLine("Client: EndReceive");
            try
            {
                SocketError socketError;
                int read = client._socketRT.EndReceive(result, out socketError);
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not receive. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), true);
                    return false;
                }
                if (read <= 0)
                {
                    //Debug.WriteLine(string.Format("[SocketClient] Received a negative quantity '{0}'", read));
                    return false;
                }

                client._parserRT.Write(client._bufferForReceive, client._offsetForReceive, read);
                client._parserRT.Parse();
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }

            return true;
        }

        public void BeginSend(IParserStruct structure, AsyncCallback callback, SocketsType socketType)
        {
            lock (_sendSync)
            {
                if (Disposed || _closed)
                {
                    //Debug.WriteLine(string.Format("[SocketClient] Can't BeginSend cause Disposed='{0}' or Close='{1}'", Disposed,_closed));
					InvokeOnException(new Exception(string.Format("[SocketClient] Can't BeginSend cause Disposed='{0}' or Close='{1}'", Disposed,
                                                  _closed)), true);

                    return;
                }

                structure.WriteBytes(_bufferForSend, _offsetForSend);
                //we add 8 bytes to strcuture length, cause every structure has a header created 
                //from 2 Int32 fields
                //Structue.Length - reports the length of structure itself, without header
                try
                {
                    SocketError error;
                    if (socketType == SocketsType.History) _socketHistory.BeginSend(_bufferForSend, _offsetForSend, structure.Length + 8, SocketFlags.None, out error,callback, this);
                    else if (socketType == SocketsType.RealTime) _socketRT.BeginSend(_bufferForSend, _offsetForSend, structure.Length + 8, SocketFlags.None, out error, callback, this);
                    else _socket.BeginSend(_bufferForSend, _offsetForSend, structure.Length + 8, SocketFlags.None, out error,callback, this);
                    if (error != SocketError.Success)
                    {
                        InvokeLog("[SocketClient] ERROR: " + error.ToString());
                        //Debug.WriteLine(string.Format("[SocketClient] Can't begin cause of error '{0}'", error));
                        if (error == SocketError.ConnectionReset || error == SocketError.ConnectionAborted || error == SocketError.NotConnected || error == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+error), true);
                    }
                }
                catch (Exception exception)
                {
                    //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                    if (!StandaloneUsage)
                        throw;
                    InvokeOnException(exception, true);
                }
            }
        }

        public int EndSend(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                Socket socket = client._socket;
                if (socket == null)
                {
                    //Debug.WriteLine("[SocketClient] Could not EndSend cause socket is Null");
                    return -1;
                }
                SocketError socketError;
                int res = socket.EndSend(result, out socketError);
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not send. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), true);
                    return -1;
                }
                ////Debug.WriteLine(string.Format("Data has been sent to client '{0}'", client.Id));
                return res;
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }
            return -1;
        }
        
        public int EndSendHistory(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                Socket socket = client._socketHistory;
                if (socket == null)
                {
                    //Debug.WriteLine("[SocketClient] Could not EndSend cause socket is Null");
                    return -1;
                }
                SocketError socketError;
                int res = socket.EndSend(result, out socketError);
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not send. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), true);
                    return -1;
                }
                ////Debug.WriteLine(string.Format("Data has been sent to client '{0}'", client.Id));
                return res;
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }
            return -1;
        }

        public int EndSendRT(IAsyncResult result)
        {
            Client client = (Client)result.AsyncState;
            try
            {
                Socket socket = client._socketRT;
                if (socket == null)
                {
                    //Debug.WriteLine("[SocketClient] Could not EndSend cause socket is Null");
                    return -1;
                }
                SocketError socketError;
                int res = socket.EndSend(result, out socketError);
                if (socketError != SocketError.Success)
                {
                    InvokeLog("[SocketClient] ERROR: " + socketError.ToString());
                    //Debug.WriteLine(string.Format("[SocketClient] Could not send. Error = '{0}'", socketError));
                    if (socketError == SocketError.ConnectionReset || socketError == SocketError.ConnectionAborted || socketError == SocketError.NotConnected || socketError == SocketError.ConnectionRefused) InvokeOnException(new Exception("Connection Closed - "+socketError), true);
                    return -1;
                }
                ////Debug.WriteLine(string.Format("Data has been sent to client '{0}'", client.Id));
                return res;
            }
            catch (Exception exception)
            {
                //Debug.WriteLine(string.Format("[SocketClient] {0}", exception.Message));
                if (!client.StandaloneUsage)
                    throw;
                client.InvokeOnException(exception, true);
            }
            return -1;
        }

        public void SendStruct(IParserStruct @struct, Action onCompleted)
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
                    BeginSend(@struct, result =>
                                         {
                                             EndSend(result);
                                             onCompleted();
                                         },SocketsType.Main);
                    break;
                case 8: //BarData
                    BeginSend(@struct, result =>
                                         {
                                             EndSendHistory(result);
                                             onCompleted();
                                         },SocketsType.History);
                    break;
                case 6: //TickData
                    BeginSend(@struct, result =>
                                         {
                                             EndSendRT(result);
                                             onCompleted();
                                         },SocketsType.RealTime);
                    break;
            }
        }

        public string IP
        {
            get
            {
                return _socket != null ? _socket.RemoteEndPoint.ToString() : "<No IP>";
            }
        }

        public IPAddress IPadress
        {
            get
            {
                return _socket != null ? ((IPEndPoint)_socket.RemoteEndPoint).Address : null;
            }
        }

        public int Port
        {
            get
            {
                return _socket != null ? ((IPEndPoint)_socket.LocalEndPoint).Port : -1;
            }
        }
        public void SetParserStructs(Dictionary<int, Type> structs)
        {
            _parser.Structs = structs;
            _parserHistory.Structs = structs;
            _parserRT.Structs = structs;
        }

        public bool HasSubscriptionToData { get; set; }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, 
        /// or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Close();
            }

            _disposed = true;
        }

        public void Close()
        {
            try
            {
                if (_socket != null && _socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                if (_socketHistory != null && _socket.Connected)
                    _socketHistory.Shutdown(SocketShutdown.Both);
                if (_socketRT != null && _socket.Connected)
                    _socketRT.Shutdown(SocketShutdown.Both);
            }
            catch (Exception exception)
            {
                if (StandaloneUsage)
                    InvokeOnException(exception, true);
            }
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
            if (_socketHistory != null)
            {
                _socketHistory.Close();
                _socketHistory = null;
            }
            if (_socketRT != null)
            {
                _socketRT.Close();
                _socketRT = null;
            }

            //Id = string.Empty;
            _closed = true;
        }

        #endregion


    }
}

