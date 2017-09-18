using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace ModulusFE.Sockets
{
    public class ServerRabbit : IDisposable
    {
        public class ServerException : Exception
        {
            public ServerException(string message)
                : base(message)
            {
            }
        }

        public int Id;
        public delegate void LogHandler(string format, params object[] args);
        public LogHandler Log;
        public EndPoint _localEndPoint;
        private bool _disposed;
        private bool _stoped;
        private object _syncHandle = new object();
        public bool depure = true; //Enable extra logs
        public int _BUFFER_SIZE = 40 * 1024;

        //Rabbit members
        public ConnectionFactory _conFactory;
        public IConnection _connection;
        public IModel _channel;
        public List<ClientRabbit> _clients = new List<ClientRabbit>();

        //Thread used to listen to incoming connection
        private Thread _acceptThread;

        #region Events
        public event ClientRabbit.DataReceivedHandler DataReceived;
        public delegate void ClientNotAuthenticatedHandler(ServerRabbit sender, string clientId);
        public event ClientNotAuthenticatedHandler ClientNotAuthenticated;

        #endregion

        private void DoLog(string format, params object[] args)
        {
            if (Log != null)
                Log(format, args);
        }
         
        private readonly Dictionary<int, Type> _structures = new Dictionary<int, Type>();

        public ServerRabbit(int id)
        {
            _stoped = true;
            Id = Id;
        }

        ~ServerRabbit()
        {
            Dispose(false);
        }

        public bool IsStarted
        {
            get { return !_stoped; }
        }
                
        public void Start(IPEndPoint localEndPoint)
        {
            string address = "localhost";//TODO: localEndPoint.Address.ToString();
            Id = localEndPoint.Port;
            try
            {
                _localEndPoint = localEndPoint;
                _stoped = false;

                //TODO: build Host name as "IP : Port"??
                _conFactory = new ConnectionFactory() { HostName = address };
                _connection = _conFactory.CreateConnection();
                _channel = _connection.CreateModel();


                DoLog("TCP/Server Main starting");
                _acceptThread = new Thread(DoListen);
                _acceptThread.Name = "_acceptThread";
                _acceptThread.IsBackground = true;
                _acceptThread.Start(localEndPoint);
            }
            catch (Exception ex)
            {
                Log("EXCEPTION Start() " + address + "\n" + ex);
            }
            
        }

        public void DoListen(object state)
        { 
            try
            {
                Starting(this);
                _channel.QueueDeclare("channelServer", false, false, false, null);

                var consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume("channelServer", true, consumer);
                Started(this);
                while (true)
                {
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    List<IParserStruct> structures = Parse(ea.Body);
                    foreach (IParserStruct s in structures)
                    {
                        DataReceived(new ClientRabbit(this.Id), s);
                        Log("[x] Recieved on port "+Id);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                Log("EXCEPTION DoListen() "+ex.Message);
            }
        }

        public List<IParserStruct> Parse(byte[] message)
        {
            List<IParserStruct> result = new List<IParserStruct>();
            try
            {
                MemoryStream _stream = new MemoryStream(message.Length);
                _stream.Write(message, 0, message.Length);

                byte[] _buffer = new byte[40 * 1024];
                int step = 0;
                int structLength = -1;

                IParserStruct currentStruct = null;

                //reset stream Position, After writing it points to the end of stream
                _stream.Position = 0;

                while (_stream.Position + 8 <= _stream.Length) //8 cause we need at least 8 bytes to start
                {
                    if (step == 0)
                    {
                        //read Id - it must be always at first place
                        _stream.Read(_buffer, 0, 8); //8 bytes for StructId and its Length
                        int structId = BitConverter.ToInt32(_buffer, 0);

                        Type currentStructType;
                        if (!_structures.TryGetValue(structId, out currentStructType))
                        {
                            Log(string.Format("ERROR Getting value for structID {2} - Stream Length: {0}.", structId, _stream.Length));
                            _stream.SetLength(0);
                            _stream.Seek(0, SeekOrigin.Begin);
                            return result;
                            //throw new BufferParserException(string.Format("Structure with ID = {0} is not supported.", structId));
                        }

                        currentStruct = (IParserStruct)Activator.CreateInstance(currentStructType);

                        //read struct Length
                        structLength = BitConverter.ToInt32(_buffer, 4);

                        step = 1;
                        continue;
                    }
                    if (step == 1)
                    {
                        if (currentStruct == null || structLength == -1)
                            Log("Got to read the structure, but not created.");
                        if (structLength + _stream.Position <= _stream.Length)
                        {
                            _stream.Read(_buffer, 0, structLength);
                            currentStruct.ReadBytes(_buffer, 0);
                            //make an additional check to ensure struct was read OK
                            //if (!currentStruct.CrcOk())
                            //  throw new BufferParserException("Structure was read, but its CRC is wrong.");

                            //if is ok, raise the event that structure was read succesfully
                            result.Add(currentStruct);

                            //go to read another structure
                            step = 0;
                        }
                        else
                        {
                            break; //nothing to read
                        }
                    }
                }
                if (step == 1)
                    _stream.Position -= 8;

                int shiftBufferLength = (int)(_stream.Length - _stream.Position);
                if (shiftBufferLength > 0)
                {
                    _stream.Read(_buffer, 0, shiftBufferLength);
                    _stream.SetLength(0);
                    _stream.Seek(0, SeekOrigin.Begin);
                    _stream.Write(_buffer, 0, shiftBufferLength);
                }
                else
                {
                    _stream.SetLength(0);
                    _stream.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (Exception ex) { /*Log("EXCEPTION Parse() "+ex.Message);*/ }
            return result;
        }

        public void AddClient(ClientRabbit messageId)
        {
            lock (_syncHandle)
            {                
                int index = -1;
                //for (int i = 0; i < _clients.Count;i++ ) Log("[AddClient] socket.REP=" + ((IPEndPoint)socket.RemoteEndPoint).Address.ToString() + " _client[" + i + "].IP=" + _clients[i].IPadress.ToString());
                try
                {
                    index = _clients.IndexOf(_clients.Find(s=>s.Id==messageId.Id));
                }
                catch (Exception) { }
                //Already exist channel!
                if(index != null && index!=-1)
                {
                    DoLog("Client is already registered!");
                    return;
                }

                _clients.Add(messageId);
                
                //start receiving on this new socket
                DoReceiveAsync(_clients.Last());

                

            }
        }

        private void ClientOnDataReceived(ClientRabbit client, IParserStruct structure)
        {
            if (DataReceived != null)
                DataReceived(client, structure);
        }
        public void DeleteClient(ClientRabbit client)
        {
            lock (_syncHandle)
            {
                _clients.Remove(client);
                //CloseClientSocket(client);
            }
        }
        private void DoReceiveAsync(ClientRabbit client)
        {
            MethodInvoker async = delegate
            {
                try
                {
                    client.StartReceiving(client.Id);

                }
                catch (Exception ex)
                {
                    Log("EXCEPTION: DoReceiveAsync : " + ex.Message);
                }

            };
            async.BeginInvoke(CallBack,null);
        }

        private void CallBack(IAsyncResult result)
        {

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

        public void DoSendAsync(string clientId, IEnumerable<IParserStruct> structures)
        {
            try
            {
                //TODO: Parse structures to Byte array!
                foreach (IParserStruct struc in structures)
                {
                    switch (struc.Id)
                    {
                        case 7:
                        case 8:
                        case 12:
                        case 13:
                        case 14:
                            _BUFFER_SIZE = 40 * 1024;
                            break;
                        default:
                            _BUFFER_SIZE = 1024;
                            break;
                    }
                    byte[] body = new byte[_BUFFER_SIZE];
                    struc.WriteBytes(body, 0);
                    _channel.BasicPublish("", clientId, null, body);
                    if (depure) Log("DoSendAsync struct for client "+clientId);
                }
            }
            catch (Exception ex)
            {
                Log("EXCEPTION: DoSendAsync : " + ex.Message);
            }
            return;
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
                //TODO: Close Connections!
            }
        }

        #endregion

    }
}
