using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace ModulusFE.Sockets
{
    public class ClientRabbit : IDisposable
    {
        public bool ignoreParse = false;

        private bool _disposed;
        private bool _closed;
        public string Id;
        private readonly BufferParser _parser;
        protected byte[] _bufferForReceive;
        protected int _offsetForReceive;
        protected byte[] _bufferForSend;
        protected int _offsetForSend;
        public int _BUFFER_SIZE = 40 * 1024;
        public int Port = -1;
        public bool ConnectedMain = false;

        //Rabbit members
        public ConnectionFactory _conFactory;
        public IConnection _connection;
        public IModel _channel;

        public delegate void DataReceivedHandler(ClientRabbit sender, IParserStruct structure);
        public event DataReceivedHandler DataReceived;

        public delegate void OnExceptionHandler(ClientRabbit sender, Exception exception, bool? okToContinue);
        public event OnExceptionHandler OnException;

        public event Action<ClientRabbit> OnConnecting;

        public delegate void LogHandler(string message);
        public LogHandler OnLog;

        private void InvokeOnConnecting(ClientRabbit sender)
        {
            Action<ClientRabbit> connecting = OnConnecting;
            if (connecting != null) connecting(sender);
        }

        public event Action<ClientRabbit> OnConnected;

        private void InvokeOnConnected(ClientRabbit sender)
        {
            Action<ClientRabbit> connected = OnConnected;
            if (connected != null) connected(sender);
        }

        private void InvokeOnException(Exception exception, bool? okToContinue)
        {
            //MessageBox.Show(exception.Message);
            OnExceptionHandler exceptionHandler = OnException;
            if (exceptionHandler != null)
            {
                //Debug.WriteLine(string.Format("[SocketClient]11C CallingExceptionHandler"));
                exceptionHandler.BeginInvoke(this, exception, okToContinue, EndInvokeEvent, null);
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

        private void EndInvokeEvent(IAsyncResult result)
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
                //Debug.WriteLine("An event listener went kaboom!");
            }
        }
        private void InvokeLog(string message)
        {
            //MessageBox.Show(exception.Message);
            LogHandler logHandler = OnLog;
            if (logHandler != null)
            {
                //Debug.WriteLine(string.Format("[SocketClient]InvokeLog "+message));
                logHandler.BeginInvoke(message, EndInvokeLog, null);
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
        public void SetBuffers(byte[] bufferForReceive, int offsetForReceive,
          byte[] bufferForSend, int offsetForSend)
        {
            _offsetForReceive = offsetForReceive;
            _bufferForReceive = bufferForReceive;

            _offsetForSend = offsetForSend;
            _bufferForSend = bufferForSend;
        }

        /// <summary>
        /// Gets or sets whether the client is used as a standalone object, not 
        /// from Server. In this case it will raise exceptions, in case when it is used
        /// from under Server exceptions must be propagated to the Server
        /// </summary>
        public bool StandaloneUsage;
        
        private readonly Dictionary<int, Type> _structures = new Dictionary<int, Type>();

        public ClientRabbit(int port)
        {
            Init();
            Id = GetMacAddress();
            Port = port;

            _parser = new BufferParser();
            _parser.StructRead += (parser, structure) =>
            {
                if (DataReceived != null)
                    foreach(var struc in structure)DataReceived(this, struc);
            };
            HasSubscriptionToData = false;
        }
        public string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
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

        public void Connect(IPAddress ipAddress, int port)
        {
            _closed = true;
            Port = port;
            UriBuilder urib = new UriBuilder(ipAddress.ToString());
            string address = urib.Uri.ToString();
            //while (_closed && !cancelConnecting.IsCancellationRequested)
            //{
                try
                {

                    //TODO: build Host name as "IP : Port"??
                    _conFactory = new ConnectionFactory();
                    _conFactory.UserName = "plena";
                    _conFactory.Password = "plena";
                    _conFactory.VirtualHost = "/";
                    _conFactory.Protocol = Protocols.FromEnvironment();
                    _conFactory.HostName = ipAddress.ToString();
                    _conFactory.Port = AmqpTcpEndpoint.UseDefaultPort;
                    _connection = _conFactory.CreateConnection();
                    _channel = _connection.CreateModel();

                    InvokeOnConnecting(this);


                    InvokeOnConnected(this);

                    _closed = false;

                }
                catch (Exception exception)
                {
                    InvokeOnException(exception, false);
                    return;
                }
        }

        public void StartReceiving(string messageId)
        {
            MethodInvoker async = delegate
            {
                try
                {
                    _channel.QueueDeclare(messageId, true, false, true, null);

                    var consumer = new QueueingBasicConsumer(_channel);
                    _channel.BasicConsume(messageId, true, consumer);

                    while (true)
                    {                          

                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        if (!ignoreParse)
                        {
                            List<IParserStruct> structures = Parse(ea.Body);
                            foreach (IParserStruct s in structures)
                            {
                                DataReceived(this, s);
                                //InvokeLog("[x] Recieved on channel " + messageId);
                            }
                        }
                        //else InvokeLog("[x] Recieved no parse on port " + Id);

                    }

                }
                catch (Exception ex)
                {
                    //InvokeLog("EXCEPTION: DoReceiveAsync : " + ex.Message);
                }

            };
            async.BeginInvoke(Receive_CB, null);
        }

        private void Receive_CB(IAsyncResult result)
        {
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
                            //InvokeLog(string.Format("ERROR Getting value for structID {0} - Stream Length: {1}.", structId, _stream.Length));
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
                            InvokeLog("Got to read the structure, but not created.");
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
            catch (Exception ex) { /*InvokeLog("EXCEPTION Parse() " + ex.Message);*/ }
            return result;
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

        public void StartSending(IParserStruct structure, string messageId)
        {
            try
            {
                switch (structure.Id)
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
                structure.WriteBytes(body, 0);
                _channel.BasicPublish("", messageId, null, body);
            }
            catch (Exception ex)
            {
                //InvokeLog("EXCEPTION: StartSending : " + ex.Message);
            }

            return;
        }

        public void SendStruct(IParserStruct @struct, string messageId, Action onCompleted)
        {
            switch (@struct.Id)
            {
                default:
                    StartSending(@struct, messageId);
                    break;
            }
            onCompleted();
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
                //Close Rabbit connections
                //_channel.QueueDelete(Id);
                _channel.Close();
                _connection.Close();
            }
            catch (Exception exception)
            {
            }
            _closed = true;
        }

        #endregion


    }
}
