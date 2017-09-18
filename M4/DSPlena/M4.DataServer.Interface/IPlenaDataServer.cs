using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Xml;
using ExtremeDB;
using M4.DataServer.Interface;
using M4.DataServer.Interface.ProtocolStructs;
using ModulusFE.Sockets;
using Client = ModulusFE.Sockets.Client;
using Timer = System.Threading.Timer;


namespace M4.DataServer.Interface
{
    public class IPlenaDataServer
    {
        /// <summary>
        /// EndPoint on which server will operate
        /// </summary>

        /// <summary>
        /// history request -> client Id
        /// </summary>
        private readonly Dictionary<string, Client> _dMuxes
          = new Dictionary<string, Client>();

        public IPEndPoint EndPoint { get; set; }

        public delegate void LogHandler(IPlenaDataServer server, string format, params object[] args);
        public event LogHandler Log = delegate { };

        public delegate void SubscribeSymbolHandler(object server, string clienId, string symbol, bool subscribe);
        public event SubscribeSymbolHandler SubscribeSymbol = delegate { };

        public string PathSymbols = Directory.GetCurrentDirectory() + "\\Base\\SYMBOL\\";
        public string PathHistoricalDaily = Directory.GetCurrentDirectory() + "\\Base\\HISTORICAL\\";
        public List<SymbolsPS> StockList = new List<SymbolsPS>();
        //public List<BarDataPS> HistoricalBase = new List<BarDataPS>();
        private Server _server;
        private SQLiteConnection _connectionSQLite;

        public bool useRealDatabase = false;

        private DataBaseManager _symbolDataBase;
        private DataBaseManager _dailyDataBase;
        private DataBaseManager _minutelyDataBase;

        public bool StockListOk = false;

        const int PAGE_SIZE = 2048; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
        const int DATABASE_SIZE = 16 * 1024 * 1024; //16MB

        #region RANDOM

        private class SymbolValue
        {
            public string Symbol;
            public double LastPrice;
            public double LastVolume;
        }



        public class BookQuote
        {
            public string Symbol { get; set; }
            public DateTime Timestamp { get; set; }
            public double[] BidPrices { get; set; }
            public long[] BidSizes { get; set; }
            public double[] AskPrices { get; set; }
            public long[] AskSizes { get; set; }
            public string[] Buyers { get; set; }
            public string[] Sellers { get; set; }
        }

        private readonly List<SymbolValue> _lastPrices = new List<SymbolValue>();
        private readonly System.Random _rnd = new System.Random();
        private System.Timers.Timer _timerRT;

        private List<SymbolSnapshot> _snapshotTableDay = new List<SymbolSnapshot>();
        private List<SymbolSnapshot> _snapshotTableMin = new List<SymbolSnapshot>();
        private List<BookQuote> _ookQuoteTable = new List<BookQuote>();


        String DBIMAGE_FILENAME = Directory.GetCurrentDirectory() + "\\Base\\SYMBOL" + "\\Plena.img";
        #endregion

        /// <summary>
        /// For unique symbol have a list of clients that are subscribed to RT data for it
        /// </summary>
        private readonly Dictionary<string, List<Client>> _symbolClients
          = new Dictionary<string, List<Client>>(StringComparer.InvariantCultureIgnoreCase);

        public bool IsStarted
        {
            get
            {
                return _server != null && _server.IsStarted;
            }
        }

        private void ServerOnDataReceived(Client client, IParserStruct structure)
        {
            Debug.WriteLine(string.Format("[IDataServer] Received new structure with id '{0}'", structure.Id));
            switch (structure.Id)
            {
                case StructsIds.SubscribeSymbol_Id:
                    DoSubscribeRealTime(client, (SubscribeSymbolPS)structure);
                    break;
                case StructsIds.SymbolsRequest_Id:
                    SymbolRequestPS request = (SymbolRequestPS)structure;
                    //Dont comment this if list exists only on XML (create SQLite by XML on first time):
                    //StockList = LoadSymbolsFromXML(PathSymbols);
                    List<SymbolsPS> symbolResponse = new List<SymbolsPS>();
                    foreach (SymbolsPS stock in StockList)
                    {
                        symbolResponse.Add(new SymbolsPS() { ClientId = request.ClientId, RequestId = ((SymbolRequestPS)structure).RequestId, MuxId = request.MuxId, StockId = stock.StockId, StockInfo = stock.StockInfo });
                    }
                    //Last data contains "NULL" information and Stockid = list size: 
                    symbolResponse.Add(new SymbolsPS
                    {
                        ClientId = request.ClientId,
                        RequestId = ((SymbolRequestPS)structure).RequestId,
                        MuxId = request.MuxId,
                        StockId = StockList.Count,
                        StockInfo = new Symbol
                        {
                            Name = "NULL",
                            Code = "NULL",
                            Sector = "NULL",
                            SubSector = "NULL",
                            Segment = "NULL",
                            Source = "NULL",
                            Type = "NULL",
                            Activity = "NULL",
                            Site = "NULL",
                            Status = 0
                        }
                    });
                    if (symbolResponse.Count > 0)
                    {
                        _server.DoSendAsync(client, symbolResponse);
                        Log(this, "List sent with {0} symbols!", symbolResponse.Count);
                    }
                    else Log(this, "Error loading Symbols!");
                    break;
                case StructsIds.HistoricalRequest_Id:
                    HistoryRequestPS historyRequestPs = (HistoryRequestPS)structure;
                    Log(this, "Recieved historical request for {0} {2} from {1}", historyRequestPs.Request.Symbol, historyRequestPs.ClientId, Enum.GetName(typeof(Periodicity), historyRequestPs.Request.Periodicity));
                    if (string.IsNullOrEmpty(client.Id))
                        client.Id = historyRequestPs.ClientId;
                    Thread threadHistory = new Thread(() => RequestHistory(client, historyRequestPs));
                    threadHistory.IsBackground = true;
                    threadHistory.Start();
                    break;
                case StructsIds.Authentication_Id:
                    AuthenticationPS authenticationPs = (AuthenticationPS)structure;
                    Log(this, "Client authenticates. ClientID = {0}", authenticationPs.Values[0]);

                    AuthenticationAnswerPS authenticationAnswerPs = new AuthenticationAnswerPS()
                                                                      {
                                                                          ClientId = Guid.NewGuid().ToString(),
                                                                          DataServers = new[] { new DataServerInfo(), },
                                                                      };
                    _dMuxes.Add(authenticationPs.Values[0], client);
                    if (AuthenticateClient != null)
                    {
                        bool authenticated = true;
                        string password = string.Empty;
                        string username = string.Empty;
                        if (authenticationPs.Values.Length > 0)
                            username = authenticationPs.Values[0];
                        if (authenticationPs.Values.Length > 1)
                            password = authenticationPs.Values[1];
                        AuthenticateClient(this, username, password, ref authenticated);
                        if (!authenticated)
                            authenticationAnswerPs.ClientId = string.Empty;
                    }

                    SendStruct(client, authenticationAnswerPs);
                    break;
                default:
                    Debug.WriteLine(string.Format("[IDataServer] Received wrong data structure with id '{0}'", structure.Id));
                    break;
            }
        }

        private void DoSubscribeRealTime(Client client, SubscribeSymbolPS symbolPs)
        {
            if (string.IsNullOrEmpty(client.Id))
                client.Id = symbolPs.ClientId;
            lock (_symbolClients)
            {
                if (symbolPs.Subscribe)
                {
                    List<Client> clients;
                    if (!_symbolClients.TryGetValue(symbolPs.LastBar.Symbol, out clients))
                    {
                        _symbolClients.Add(symbolPs.LastBar.Symbol, clients = new List<Client>());
                    }

                    //make sure client is not subscribed more than once to this symbol
                    if (clients.Find(client1 => client1.Id == client.Id) != null)
                    {
                        Log(this, "Client {0} already subscribed to symbol {1}", client.Id, symbolPs.LastBar.Symbol);

                        // Synchronize ticks with last updated snapshot:
                        DoSendSnapshot(client, symbolPs);
                        return;
                    }

                    Log(this, "Subscribe client {0} to symbol {1} {2}", client.Id, symbolPs.LastBar.Symbol, Enum.GetName(typeof(BaseType), (BaseType)symbolPs.LastBar.BaseType));

                    clients.Add(client);

                    //raise the event
                    ThreadPool.QueueUserWorkItem(
                      state =>
                      {
                          if (SubscribeSymbol != null)
                              SubscribeSymbol(this, client.Id, symbolPs.LastBar.Symbol, symbolPs.Subscribe);
                      });
                }
                else
                {
                    Log(this, "Un-Subscribe client {0} from symbol {1}", client.Id, symbolPs.LastBar.Symbol);

                    List<Client> clients;
                    if (_symbolClients.TryGetValue(symbolPs.LastBar.Symbol, out clients))
                    {
                        clients.Remove(client);

                        if (clients.Count > 0) //there are more clients watching this symbol
                            return;
                    }

                    Log(this, "Remove watchable symbol {0} from DDF Server", symbolPs.LastBar.Symbol);
                }

                SubscribeToSymbol(client, symbolPs.LastBar.Symbol, symbolPs.Subscribe);

                // Synchronize ticks with last updated snapshot:
                DoSendSnapshot(client, symbolPs);
            }
        }

        private bool DoSendSnapshot(Client client, SubscribeSymbolPS symbolPs)
        {
            //Verify wich base must be updated:
            BaseType Base = (BaseType)symbolPs.LastBar.BaseType;
            int Interval = Base == BaseType.Minutes15 ? 15 : 1;
            Periodicity Periodicity = Base == BaseType.Days ? Periodicity.Daily : Periodicity.Minutely;
            List<BarData> updateBars;
            //Send missed bar-datas:
            if (useRealDatabase)
            {
                Connection _connection = DBDailyShared.Connect();
                updateBars = DBDailyShared.GetBarDataSince(symbolPs.LastBar.Symbol, symbolPs.LastBar,
                                                                         Interval, (int)Periodicity, _connection);
            }
            else
            {
                if (Base == BaseType.Days) updateBars = _dailyDataBase.GetBarDataSince(symbolPs.LastBar.Symbol, symbolPs.LastBar);
                else updateBars = _minutelyDataBase.GetBarDataSince(symbolPs.LastBar.Symbol, symbolPs.LastBar);
            }
            if (updateBars.Count > 0)
            {
                int barIndex = 0;
                foreach (BarData bar in updateBars)
                {
                    BarDataPS barData = new BarDataPS
                    {
                        Data = bar,
                        BarIndex = barIndex++,
                        RequestId = symbolPs.RequestId,
                        ClientId = symbolPs.ClientId,
                        MuxId = symbolPs.MuxId

                    };
                    if (!SendBarData(client, barData))
                    {
                        return false;
                    }
                }
                //Send a NULL bardata to finish:
                BarDataPS barDataNull = new BarDataPS
                {
                    Data = new BarData
                    {
                        Symbol = symbolPs.LastBar.Symbol,
                        BaseType = updateBars[0].BaseType
                    },
                    BarIndex = -1,
                    TotalBarsCount = barIndex,
                    RequestId = symbolPs.RequestId,
                    ClientId = symbolPs.ClientId,
                    MuxId = symbolPs.MuxId
                };
                if (!SendBarData(client, barDataNull))
                {
                    return false;
                }
                Log(this, "Sync-History sent for symbol {0} with {1} {2}-bars!", symbolPs.LastBar.Symbol, barIndex, Enum.GetName(typeof(BaseType), Base));
            }
            else Log(this, "Sync-History not necessary for symbol {0} {1} !", symbolPs.LastBar.Symbol, Enum.GetName(typeof(BaseType), Base));

            //Send last snapshot:
            SymbolSnapshotPS snapshotPS = new SymbolSnapshotPS()
                                               {
                                                   ClientId = symbolPs.ClientId,
                                                   MuxId = symbolPs.MuxId,
                                                   RequestId = symbolPs.RequestId,
                                                   BaseType = (int)Base,
                                                   Snapshot = Base == BaseType.Days ?
                                                       _snapshotTableDay.Find(s => s.Symbol == symbolPs.LastBar.Symbol) :
                                                       _snapshotTableMin.Find(s => s.Symbol == symbolPs.LastBar.Symbol)
                                               };

            SendStruct(client, snapshotPS);

            return true;
        }

        /// <summary>
        /// Initializes the DataServer
        /// </summary>
        public void Init(string username, string password, params object[] args)
        {
            _lastPrices.Clear();
            _snapshotTableDay.Clear();
            _snapshotTableMin.Clear();
            if (useRealDatabase)
            {
                DBSymbolShared.OpenDatabase("Plena", PathSymbols);
                Connection _connection = DBSymbolShared.Connect();
                lock (StockList)
                {
                    StockList = DBSymbolShared.LoadSymbols(_connection);
                    if (StockList.Count > 0) StockListOk = true;
                }
                DBSymbolShared.Disconnect(_connection);
            }
            else
            {
                _symbolDataBase = new DataBaseManager("Plena", DataBaseType.AllInMemory, typeof(Symbol), PathSymbols);
                _symbolDataBase.Connect();
                lock (StockList)
                {
                    StockList = _symbolDataBase.LoadSymbols();
                    if (StockList.Count > 0) StockListOk = true;
                }
            }
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            Stop();

            _server = new Server();
            _server.DataReceived += ServerOnDataReceived;
            _server.Starting += sender => Starting(this);
            _server.Started += sender => Started(this);
            _server.Exception += (sender, exception, name) => Exception(this, exception, name);
            _server.IncommingConnection += (sender, point) => IncommingConnection(this, point);
            _server.Stoping += sender => Stoping(this);
            _server.Stoped += sender => Stoped(this);
            _server.ClientDisconnected += ServerOnClientDisconnected;
            _server.Log += (format, objects) => Log(this, format, objects);

            _server.RegisterStructure(StructsIds.SubscribeSymbol_Id, typeof(SubscribeSymbolPS));
            _server.RegisterStructure(StructsIds.HistoricalRequest_Id, typeof(HistoryRequestPS));
            _server.RegisterStructure(StructsIds.SymbolsRequest_Id, typeof(SymbolRequestPS));
            _server.RegisterStructure(StructsIds.Authentication_Id, typeof(AuthenticationPS));
            _server.RegisterStructure(StructsIds.SymbolSnapshot_Id, typeof(SymbolSnapshotPS));

            _server.Start(EndPoint);

            StartServer();

            //Initialize Database Connections:

            /* Convert SQLite Symbols database to ExtremeDB if it doesn't exists: */
            bool resetDatabase = false;
            if (!StockListOk || resetDatabase)
            {
                StockList = LoadSymbolsFromSQLite();
                if (useRealDatabase)
                {
                    Connection _connection = DBSymbolShared.Connect();
                    DBSymbolShared.SaveSymbols(StockList, _connection);
                    DBSymbolShared.Disconnect(_connection);
                }
                else _symbolDataBase.SaveSymbols(StockList);
                StockListOk = true;
            }

            //Connect to historical daily database:
            if (useRealDatabase)
            {
                DBDailyShared.OpenDatabase("Daily", typeof(BarData), typeof(BarDataDisk), PathHistoricalDaily);
                /*Connection _connection = DBDailyShared.Connect();
                if (DBDailyShared.CreateDatabaseMem(_connection))
                    Console.WriteLine("Created Memory Bardata, records = " + DBDailyShared.Count(_connection));
                else Console.WriteLine("Failed creating Memory BarData!");
                DBDailyShared.Disconnect(_connection);*/
            }
            else
            {
                _dailyDataBase = new DataBaseManager("Daily", DataBaseType.AllInMemory, typeof(BarData), PathHistoricalDaily);
                _dailyDataBase.Connect();
                _minutelyDataBase = new DataBaseManager("Minutely", DataBaseType.AllInMemory, typeof(BarData), PathHistoricalDaily);
                _minutelyDataBase.Connect();
            }

        }

        private void ServerOnClientDisconnected(object sender, string id)
        {
            foreach (var client in _symbolClients)
            {
                client.Value.RemoveAll(client1 => client1.Id == id);
            }
            //raise event
            ClientDisconnected(this, id);
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            StopServer();

            if (_server == null)
                return;

            _server.Stop();
        }

        /// <summary>
        /// User friendly dataserver name
        /// </summary>
        public string Name
        {
            get { return "RND"; }
        }

        private void TimerRtOnElapsed(object sender, ElapsedEventArgs args)
        {
            foreach (SymbolSnapshot snapshot in _snapshotTableDay)
            {
                TickData tickData = new TickData
                {
                    Id = snapshot.Id + 1,
                    Symbol = snapshot.Symbol,
                    TradeDate = DateTime.Now,
                    Price = Random(snapshot.Close * 0.99, snapshot.Close * 1.01),
                    Quantity = snapshot.Quantity = (long)Random(100, 500),
                    Buyer = snapshot.Buyer = "Buyer" + ((long)Random(0, 10)),
                    Seller = snapshot.Seller = "Seller" + ((long)Random(0, 10)),
                };
                //Update snaphots:
                snapshot.AskPrice = tickData.Price - 0.01;
                snapshot.AskSize = tickData.Quantity;
                snapshot.BidPrice = tickData.Price + 0.01;
                snapshot.BidSize = tickData.Quantity;
                snapshot.Buyer = tickData.Buyer;
                snapshot.Close = tickData.Price;
                snapshot.High = snapshot.High > tickData.Price ? snapshot.High : tickData.Price;
                snapshot.Low = snapshot.Low < tickData.Price ? snapshot.Low : tickData.Price;
                snapshot.Id = tickData.Id;
                snapshot.Quantity = tickData.Quantity;
                snapshot.Timestamp = tickData.TradeDate;

                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).AskPrice = tickData.Price - 0.01;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).AskSize = tickData.Quantity;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).BidPrice = tickData.Price + 0.01;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).BidSize = tickData.Quantity;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Buyer = tickData.Buyer;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Close = tickData.Price;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).High = _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).High > tickData.Price ? _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).High : tickData.Price;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Low = _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Low < tickData.Price ? _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Low : tickData.Price;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Id = tickData.Id;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Quantity = tickData.Quantity;
                _snapshotTableMin.Find(s => s.Symbol == tickData.Symbol).Timestamp = tickData.TradeDate;



                SendTickData(tickData);

                //save to local cache
                //TickDataFilesManager.I.WriteData(tickData.Symbol, new TickDataFile.Data(tickData.TradeDate, (decimal)tickData.Price, tickData.Quantity));
                //ERROR: Generating exception on Client::EndRecieve() because .NET version!!!
                //{"Mixed mode assembly is built against version 'v2.0.50727' of the runtime and cannot be loaded in the 4.0 runtime without additional configuration information.":null}
                //FIXED: Added "<startup useLegacyV2RuntimeActivationPolicy="true"><supportedRuntime version="v2.0"/>" on DAWServer::App.config
            }

            /*
            for (int i = 0; i < _lastPrices.Count; i++)
            {
                double lastPrice = _lastPrices[i].LastPrice;
                _lastPrices[i].LastPrice = lastPrice = Random(lastPrice * 0.99, lastPrice * 1.01);

                SendTickData(new TickData
                               {
                                   Symbol = _lastPrices[i].Symbol,
                                   TradeDate = DateTime.Now,
                                   Price = lastPrice,
                                   Volume = _lastPrices[i].LastVolume = NextVolume(_lastPrices[i].LastVolume),
                                   Bid = _rnd.NextDouble() * lastPrice,
                                   BidSize = (long)(_rnd.NextDouble() * _lastPrices[i].LastVolume),
                                   Ask = _rnd.NextDouble() * lastPrice,
                                   AskSize = (long)(_rnd.NextDouble() * _lastPrices[i].LastVolume),
                                   Buyer = "Buyer" + ((long) Random(0, 10)),
                                   Seller = "Seller" + ((long) Random(0, 10)),
                               });
            }
            */
        }

        /// <summary>
        /// Subscribe to a real-time symbol
        /// </summary>
        /// <param name="client"></param>
        /// <param name="symbol"></param>
        /// <param name="subscribe">subscribe or remove subscription</param>
        protected void SubscribeToSymbol(ModulusFE.Sockets.Client client, string symbol, bool subscribe)
        {
            if (!subscribe || SymbolExists(symbol)) return;

            InitSymbolLastValues(symbol);
        }

        private bool SymbolExists(string symbol)
        {
            //return _lastPrices.FindIndex(value => value.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase)) != -1;
            return _snapshotTableDay.FindIndex(value => value.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase)) != -1;


        }

        private SymbolValue ValueBySymbol(string symbol)
        {
            return _lastPrices.Find(value => value.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase));
        }

        private SymbolSnapshot SnapshotBySymbol(string symbol)
        {
            return _snapshotTableDay.Find(value => value.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase));
        }

        private void InitSymbolLastValues(string symbol)
        {
            BarData lastBar = new BarData();
            BarData lastBarM = new BarData();
            if (useRealDatabase)
            {
                Connection _connection = DBDailyShared.Connect();
                lastBar = DBDailyShared.GetLastBarDataDiskOrMemory(symbol, BaseType.Days, _connection);
                lastBarM = DBDailyShared.GetLastBarDataDiskOrMemory(symbol, BaseType.Minutes, _connection);
                DBDailyShared.Disconnect(_connection);
            }
            else
            {
                lastBar = _dailyDataBase.GetLastBarData(symbol);
                lastBarM = _minutelyDataBase.GetLastBarData(symbol);
            }
            double lastPrice = lastBar.ClosePrice;
            double lastVolume = lastBar.Volume;
            long Quantity = (long) Random(100, 500);
            string Buyer = "Buyer" + ((long) Random(0, 10));
            string Seller = "Seller" + ((long) Random(0, 10));
            _lastPrices.Add(new SymbolValue
            {
                Symbol = symbol,
                LastPrice = lastPrice,
                LastVolume = lastVolume
            });
             _snapshotTableDay.Add(new SymbolSnapshot
            {
                Id = 0,
                Symbol = symbol,
                Timestamp = lastBar.TradeDate,
                Close = lastBar.ClosePrice,
                High = lastBar.HighPrice,
                Low = lastBar.LowPrice,
                Open = lastBar.OpenPrice,
                Quantity = Quantity,
                Buyer = Buyer,
                Seller = Seller,
                VolumeFinancial = lastBar.Volume,
                VolumeStocks = (long)lastBar.Volume,
                VolumeTrades = (long)lastBar.Volume
            });
             _snapshotTableMin.Add(new SymbolSnapshot
             {
                 Id = 0,
                 Symbol = symbol,
                 Timestamp = lastBarM.TradeDate,
                 Close = lastBarM.ClosePrice,
                 High = lastBarM.HighPrice,
                 Low = lastBarM.LowPrice,
                 Open = lastBarM.OpenPrice,
                 Quantity = Quantity,
                 Buyer = Buyer,
                 Seller = Seller,
                 VolumeFinancial = lastBarM.Volume,
                 VolumeStocks = (long)lastBarM.Volume,
                 VolumeTrades = (long)lastBarM.Volume
             });
            
        }

        private double NextVolume(double lastVolume)
        {
            int sign = 1;
            if (_rnd.NextDouble() > 0.5)
                sign = -1;

            return lastVolume + sign * Random(0, lastVolume * 0.01); //1%
        }

        private bool GetLocalHistory(ModulusFE.Sockets.Client client, HistoryRequestPS historyRequest)
        {
            System.Diagnostics.Stopwatch stop = new Stopwatch();
            int barIndex = 0;
            stop.Start();
            BaseType Base = historyRequest.Request.Periodicity == Periodicity.Daily ? BaseType.Days : BaseType.Minutes;
            List<BarData> Bars;
            Connection _connection = DBDailyShared.Connect();
            // Request all data from base:
            if (historyRequest.Request.LastRecordTime == DateTime.MinValue || historyRequest.Request.LastRecordTime.Date > DateTime.Now.Date)
            {
                if (useRealDatabase) Bars = DBDailyShared.GetBarDataAll(historyRequest.Request.Symbol, historyRequest.Request.BarSize, (int)historyRequest.Request.Periodicity, _connection);
                else
                {
                    if (Base == BaseType.Days) Bars = _dailyDataBase.GetBarDataAll(historyRequest.Request.Symbol);
                    else Bars = _minutelyDataBase.GetBarDataAll(historyRequest.Request.Symbol);
                }
                if (Bars.Count() > 0)
                {
                    foreach (BarData bar in Bars)
                    {
                        BarDataPS barData = new BarDataPS
                        {
                            Data = bar,
                            BarIndex = barIndex++,
                            RequestId = historyRequest.RequestId,
                            ClientId = historyRequest.ClientId,
                            MuxId = historyRequest.MuxId

                        };
                        if (!SendBarData(client, barData))
                        {
                            return false;
                        }
                    }
                    //Send a NULL bardata to finish:
                    BarDataPS barDataNull = new BarDataPS
                    {
                        Data = new BarData
                        {
                            Symbol = historyRequest.Request.Symbol,
                            BaseType = Bars[0].BaseType
                        },
                        BarIndex = -1,
                        TotalBarsCount = barIndex,
                        RequestId = historyRequest.RequestId,
                        ClientId = historyRequest.ClientId,
                        MuxId = historyRequest.MuxId,

                    };
                    if (!SendBarData(client, barDataNull))
                    {
                        return false;
                    }
                    Log(this, "History sent for symbol {0} with {1} {2} bars!", historyRequest.Request.Symbol, barIndex, Enum.GetName(typeof(Periodicity), historyRequest.Request.Periodicity));
                }
                else
                {
                    return false;
                }
            }
            else // Request partial data by Datetime referenced:
            {
                if (useRealDatabase)
                {
                    Bars = DBDailyShared.GetBarDataSince(historyRequest.Request.Symbol,
                                                         new BarData
                                                             {
                                                                 Symbol = historyRequest.Request.Symbol,
                                                                 TradeDate = historyRequest.Request.LastRecordTime,
                                                                 ClosePrice = historyRequest.Request.LastRecordValue
                                                             }, 1,
                                                         Periodicity.Daily.GetHashCode(), _connection);
                }
                else
                {
                    if (Base == BaseType.Days) Bars = _dailyDataBase.GetBarDataSince(historyRequest.Request.Symbol,
                                                          new BarData
                                                            {
                                                                Symbol = historyRequest.Request.Symbol,
                                                                TradeDate = historyRequest.Request.LastRecordTime,
                                                                ClosePrice = historyRequest.Request.LastRecordValue
                                                            });
                    else Bars = _minutelyDataBase.GetBarDataSince(historyRequest.Request.Symbol,
                                                          new BarData
                                                          {
                                                              Symbol = historyRequest.Request.Symbol,
                                                              TradeDate = historyRequest.Request.LastRecordTime,
                                                              ClosePrice = historyRequest.Request.LastRecordValue
                                                          });
                }
                if (Bars.Count() > 0)
                {
                    foreach (BarData bar in Bars)
                    {
                        BarDataPS barData = new BarDataPS
                        {
                            Data = bar,
                            BarIndex = barIndex++,
                            RequestId = historyRequest.RequestId,
                            ClientId = historyRequest.ClientId,
                            MuxId = historyRequest.MuxId
                        };
                        if (!SendBarData(client, barData))
                        {
                            return false;
                        }
                    }
                    //Send a NULL bardata to finish:
                    BarDataPS barDataNull = new BarDataPS
                    {
                        Data = new BarData
                        {
                            Symbol = historyRequest.Request.Symbol,
                            BaseType = Bars[0].BaseType
                        },
                        BarIndex = -1,
                        TotalBarsCount = barIndex,
                        RequestId = historyRequest.RequestId,
                        ClientId = historyRequest.ClientId,
                        MuxId = historyRequest.MuxId
                    };
                    if (!SendBarData(client, barDataNull))
                    {
                        return false;
                    }
                    Log(this, "History sent for symbol {0} with {1} bars!", historyRequest.Request.Symbol, barIndex);
                }
                else //if (_dailyDataBase.GetLastBarData(historyRequest.Request.Symbol).TradeDate <= historyRequest.Request.LastRecordTime)
                {
                    //Send last bar to validate:
                    BarDataPS barDataLast = new BarDataPS
                                                {
                                                    Data =
                                                        (useRealDatabase ?
                                                        DBDailyShared.GetLastBarDataDisk(
                                                                 historyRequest.Request.Symbol, Base, _connection)
                                                             : (Base == BaseType.Days ? _dailyDataBase.GetLastBarData(
                                                                 historyRequest.Request.Symbol) : _minutelyDataBase.GetLastBarData(
                                                                 historyRequest.Request.Symbol))),
                                                    BarIndex = barIndex,
                                                    RequestId = historyRequest.RequestId,
                                                    ClientId = historyRequest.ClientId,
                                                    MuxId = historyRequest.MuxId
                                                };
                    if (!SendBarData(client, barDataLast))
                    {
                        return false;
                    }
                    Log(this, "History sent for symbol {0} with {1} bars!", historyRequest.Request.Symbol, barIndex);
                    //Send a NULL bardata to finish:
                    BarDataPS barDataNull = new BarDataPS
                    {
                        Data = new BarData
                        {
                            Symbol = historyRequest.Request.Symbol,
                            BaseType = (int)Base
                        },
                        BarIndex = -1,
                        TotalBarsCount = barIndex,
                        RequestId = historyRequest.RequestId,
                        ClientId = historyRequest.ClientId,
                        MuxId = historyRequest.MuxId
                    };
                    if (!SendBarData(client, barDataNull))
                    {
                        return false;
                    }
                }

            }
            DBDailyShared.Disconnect(_connection);
            long time = stop.ElapsedMilliseconds;
            //MessageBox.Show("Time: " + time+"\nQty: "+barIndex);
            return true;
        }

        /// <summary>
        /// Request historical data. 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="historyRequest"></param>
        protected void RequestHistory(ModulusFE.Sockets.Client client, HistoryRequestPS historyRequest)
        {
            try
            {
                List<BarDataPS> HistoricalBase = new List<BarDataPS>();
                //Try to get records on local database:
                if (GetLocalHistory(client, historyRequest))
                {
                    return;
                }

                //Request data from server if isnt local: (emulated radomly)
                //HistoricalBase.Clear();
                DateTime endDate = DateTime.Now;
                DateTime startDate;
                if (historyRequest.Request.Periodicity == Periodicity.Daily) startDate = new DateTime(1995, 01, 01, 12, 00, 00);
                else startDate = new DateTime(endDate.Year, endDate.Month-1, 01, 00, 00, 00);
                BaseType Base = new BaseType();
                if (historyRequest.Request.Periodicity == Periodicity.Daily) Base = BaseType.Days;
                else Base = BaseType.Minutes;
                //else if (historyRequest.Request.BarSize == 15) Base = BaseType.Minutes15;
                if (useRealDatabase)
                {
                    Connection _connection = DBDailyShared.Connect();
                    DateTime lastTime = DBDailyShared.GetLastBarDataDisk(historyRequest.Request.Symbol, Base, _connection).TradeDate;
                    DBDailyShared.Disconnect(_connection);
                    if (lastTime != DateTime.MinValue)
                    {
                        startDate = lastTime;
                    }
                }
                else
                {
                    DateTime lastTime = new DateTime();
                    if (Base == BaseType.Days) lastTime = _dailyDataBase.GetLastBarData(historyRequest.Request.Symbol).TradeDate;
                    else lastTime = _minutelyDataBase.GetLastBarData(historyRequest.Request.Symbol).TradeDate;
                    if (lastTime != DateTime.MinValue)
                    {
                        startDate = lastTime;
                    }
                }
                //Check if client request already have last data:
                /*if (historyRequest.Request.LastRecordTime != DateTime.MinValue)
                {
                    startDate = historyRequest.Request.LastRecordTime;
                }
                else */

                double datetimeStep = 0; //seconds
                /*if (historyRequest.Request.BarSize > 500 || historyRequest.Request.BarSize < 1)
                {
                    SendBarData(client, bdEndOfSequence);
                    return;
                }

                if (!SymbolExists(historyRequest.Request.Symbol))
                {
                    InitSymbolLastValues(historyRequest.Request.Symbol);
                }*/

                switch (historyRequest.Request.Periodicity)
                {
                    case Periodicity.Secondly:
                        datetimeStep = 1 * historyRequest.Request.BarSize;
                        startDate = endDate.AddSeconds(-historyRequest.Request.BarCount * historyRequest.Request.BarSize);
                        break;
                    case Periodicity.Minutely:
                        datetimeStep = 60; // * historyRequest.Request.BarSize;
                        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, 0, 0);
                        break;
                    case Periodicity.Hourly:
                        datetimeStep = 60 * 60 * historyRequest.Request.BarSize;
                        startDate = endDate.AddHours(-historyRequest.Request.BarCount * historyRequest.Request.BarSize);
                        break;
                    case Periodicity.Daily:
                        datetimeStep = 60 * 60 * 24;
                        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 12, 0, 0, 0);
                        break;
                    case Periodicity.Weekly:
                        datetimeStep = 60 * 60 * 24 * 7 * historyRequest.Request.BarSize;
                        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 12, 0, 0, 0);
                        startDate = endDate.AddDays(-7 * historyRequest.Request.BarCount * historyRequest.Request.BarSize);
                        break;
                }

                int barIndex = 0;
                SymbolValue symbolValue = new SymbolValue()
                {
                    LastPrice = Random(1, 100),
                    LastVolume = Random(1000, 10000),
                    Symbol = historyRequest.Request.Symbol
                };


                for (; startDate <= endDate; startDate = startDate.AddSeconds(datetimeStep))
                {
                    if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday || !(startDate.Hour > 10 && startDate.Hour < 17)) continue;
                    BarDataPS barData = new BarDataPS
                    {
                        Data = new BarData
                        {
                            Symbol = historyRequest.Request.Symbol
                        },
                        BarIndex = barIndex++,
                        //TotalBarsCount = historyRequest.Request.BarCount,
                        RequestId = historyRequest.RequestId,
                    };
                    barData.Data.TradeDate = startDate;
                    barData.Data.TradeDateTicks = startDate.Ticks;
                    barData.Data.BaseType = (int)Base;
                    barData.Data.OpenPrice = symbolValue.LastPrice;
                    double maxHigh = symbolValue.LastPrice * 1.1;
                    double minLow = symbolValue.LastPrice;
                    barData.Data.HighPrice = Random(minLow, maxHigh);
                    minLow = symbolValue.LastPrice * 0.9;
                    maxHigh = symbolValue.LastPrice;
                    barData.Data.LowPrice = Random(minLow, maxHigh);
                    barData.Data.ClosePrice = Random(barData.Data.LowPrice, barData.Data.HighPrice);
                    symbolValue.LastPrice = barData.Data.ClosePrice;

                    barData.Data.Volume = symbolValue.LastVolume;

                    //if (!SendBarData(client, barData))
                    //    break;

                    symbolValue.LastVolume = Random(1E6, 1E9);
                    HistoricalBase.Add(barData);
                }
                Console.WriteLine("Created " + barIndex + " bars for symbol " + historyRequest.Request.Symbol);
                // Create Historical Database:
                bool resetDatabase = true;
                if ((useRealDatabase ? !File.Exists(PathHistoricalDaily + "Daily.dbs") : !File.Exists(PathHistoricalDaily + "Daily.img")) || resetDatabase)
                {
                    if (useRealDatabase)
                    {
                        Connection _connection = DBDailyShared.Connect();
                        if (DBDailyShared.SaveBarDatas(HistoricalBase, _connection))
                        {
                            DBDailyShared.Disconnect(_connection);
                            //Try to get records on local database AGAIN:)
                            if (GetLocalHistory(client, historyRequest)) return;
                            else return;
                        }
                        else
                        {
                            DBDailyShared.Disconnect(_connection);
                            return;
                        }
                    }
                    else
                    {
                        if (Base == BaseType.Days)
                        {
                            if (_dailyDataBase.SaveBarDatas(HistoricalBase))
                            {
                                //Try to get records on local database AGAIN:
                                if (GetLocalHistory(client, historyRequest)) return;
                                else return;
                            }
                            else return;
                        }
                        else
                        {
                            if (_minutelyDataBase.SaveBarDatas(HistoricalBase))
                            {
                                //Try to get records on local database AGAIN:
                                if (GetLocalHistory(client, historyRequest)) return;
                                else return;
                            }
                            else return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private double Random(double min, double max)
        {
            return min + (_rnd.NextDouble() * (max - min));
        }

        /// <summary>
        /// Stops the internal DataServer
        /// </summary>
        protected void StopServer()
        {
            if (_timerRT != null)
                _timerRT.Enabled = false;
        }

        /// <summary>
        /// Starts the internal DataServer
        /// </summary>
        protected void StartServer()
        {
            _timerRT = new System.Timers.Timer { AutoReset = true };
            _timerRT.Elapsed += TimerRtOnElapsed;
            _timerRT.Interval = 500;
            _timerRT.Enabled = true;
        }

        public List<SymbolsPS> LoadSymbolsFromSQLite()
        {
            List<SymbolsPS> summary = new List<SymbolsPS>();
            try
            {
                using (_connectionSQLite = new SQLiteConnection((new SQLiteConnectionStringBuilder
                                                                    {
                                                                        DataSource =
                                                                            string.Format("{0}\\Plena.db", PathSymbols)
                                                                    }).ToString()))
                {
                    _connectionSQLite.Open();
                    //DataTable tables = connectionSQLite.GetSchema("ATIVOS");//Create the SQL Command  
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = _connectionSQLite;
                    cmd.CommandText = "SELECT * FROM Ativos";

                    //Retrieve the records using SQLiteDataReader  
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    int StockID = 0;
                    summary.Clear();
                    while (dr.Read())
                    {
                        summary.Add(new SymbolsPS
                        {
                            StockId = StockID,
                            StockInfo = new Symbol
                            {
                                Code = dr["codigo"].ToString(),
                                Name = dr["nome"].ToString(),
                                Type = dr["id_tipo"].ToString(),
                                Sector = ((Setor)dr["id_setor"]).ToString(),
                                SubSector = ((SubSetor)dr["id_sub_setor"]).ToString(),
                                Segment = ((Segmento)dr["id_segmento"]).ToString(),
                                Source = "PLENA",
                                Activity = dr["atividade_principal"].ToString(),
                                Site = dr["web_site"].ToString(),
                                Status = int.Parse(dr["status"].ToString()),
                                Priority = ((dr["codigo"].ToString() == "AALC11B" ||
                                             dr["codigo"].ToString() == "AAPL11B" ||
                                             dr["codigo"].ToString() == "ABCB4" ||
                                             dr["codigo"].ToString() == "ABCP11" ||
                                             dr["codigo"].ToString() == "ABRE11") ? 1 : 0)
                            }
                        });
                        StockID++;
                    }
                    //The last register is StockInfo="NULL", and StockID = Registers Count
                    /*summary.Add(new SymbolsPS
                    {
                        StockId = StockID,
                        StockInfo = new Symbol
                        {
                            Name = "NULL",
                            Code = "NULL",
                            Sector = "NULL",
                            SubSector = "NULL",
                            Segment = "NULL",
                            Source = "NULL",
                            Type = "NULL",
                            Activity = "NULL",
                            Site = "NULL",
                            Status = 0
                        }
                    });*/
                }
            }
            catch (Exception ex) { }

            return summary;
        }

        private List<SymbolsPS> LoadSymbolsFromXML(string caminho)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(PathSymbols + "\\Symbols.xml");

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("ATIVO");
            List<SymbolsPS> summary = new List<SymbolsPS>();
            foreach (XmlNode node in nodeList)
            {
                if (summary.Count == 0)
                {
                    summary.Add(new SymbolsPS
                    {
                        StockId = 0,
                        StockInfo = new Symbol
                            {
                                Name = node["NOME"].InnerText,
                                Code = node["COD"].InnerText.ToUpper(),
                                Sector = node["SETOR"].InnerText,
                                SubSector = node["SUBSETOR"].InnerText,
                                Segment = node["SEGMENTO"].InnerText,
                                Source = node["FONTE"].InnerText,
                                Type = node["TIPO"].InnerText,
                                Activity = node["ATIVIDADE"].InnerText,
                                Site = node["SITE"].InnerText,
                                Status = int.Parse(node["STATUS"].InnerText)
                            }
                    });
                }
                else
                {
                    summary.Add(new SymbolsPS
                    {
                        StockId = summary[summary.Count - 1].StockId + 1,
                        StockInfo = new Symbol
                        {
                            Name = node["NOME"].InnerText,
                            Code = node["COD"].InnerText.ToUpper(),
                            Sector = node["SETOR"].InnerText,
                            SubSector = node["SUBSETOR"].InnerText,
                            Segment = node["SEGMENTO"].InnerText,
                            Source = node["FONTE"].InnerText,
                            Type = node["TIPO"].InnerText,
                            Activity = node["ATIVIDADE"].InnerText,
                            Site = node["SITE"].InnerText,
                            Status = int.Parse(node["STATUS"].InnerText)
                        }
                    });
                }
            }
            //The last register is StockInfo="NULL" and StockID = Registers Count
            summary.Add(new SymbolsPS
            {
                StockId = summary[summary.Count - 1].StockId + 1,
                StockInfo = new Symbol
                {
                    Name = "NULL",
                    Code = "NULL",
                    Sector = "NULL",
                    SubSector = "NULL",
                    Segment = "NULL",
                    Source = "NULL",
                    Type = "NULL",
                    Activity = "NULL",
                    Site = "NULL",
                    Status = 0
                }
            });

            GenerateSymbolTable(summary);

            return summary;
            /*
            return (from XmlNode node in nodeList
                    select new SummaryPS{StockId=0,StockInfo = new Stock
                    {
                        Name = node["NOME"].InnerText,
                        Code = node["COD"].InnerText.ToUpper(),
                        Sector = node["SETOR"].InnerText,
                        Source = node["FONTE"].InnerText,
                        CodeName = node["COD"].InnerText.ToUpper() + " - " + node["NOME"].InnerText
                    }}).ToList();*/
        }

        private void GenerateSymbolTable(List<SymbolsPS> summary)
        {
            using (_connectionSQLite = new SQLiteConnection((new SQLiteConnectionStringBuilder
                                                                          {
                                                                              DataSource = string.Format("{0}\\Plena.db", PathSymbols)
                                                                          }).ToString()))
            {
                _connectionSQLite.Open();
                SQLiteCommand command = _connectionSQLite.CreateCommand();
                //CREATE TABLE
                /*command.CommandText = "CREATE TABLE IF NOT EXISTS [" + "Ativos" + @"]
                                        (
                                          [NAME] TEXT NOT NULL,
                                          [CODE] TEXT NOT NULL,
                                          [SECTOR] TEXT NOT NULL,
                                          [SUBSECTOR] TEXT NOT NULL,
                                          [SEGMENT] TEXT NOT NULL,
                                          [SOURCE] TEXT NOT NULL,
                                          [TYPE] TEXT NOT NULL,
                                          [ACTIVITY] TEXT NOT NULL,
                                          [SITE] TEXT NOT NULL,
                                          [STATUS] INTEGER NOT NULL,
                                          PRIMARY KEY ([CODE]) ON CONFLICT REPLACE
                                        )";

                command.ExecuteNonQuery();*/
                using (SQLiteTransaction transaction = _connectionSQLite.BeginTransaction())
                {
                    //POPULATE TABLE
                    command.CommandText =
                        "INSERT INTO [Ativos] VALUES(@codigo, @nome, @id_tipo, @id_setor,@id_sub_setor, @id_segmento, @id_fonte_dados, @atividade_principal, @web_site, @status)";
                    SQLiteParameter codigo = command.Parameters.Add("@codigo", DbType.String);
                    SQLiteParameter nome = command.Parameters.Add("@nome", DbType.String);
                    SQLiteParameter id_tipo = command.Parameters.Add("@id_tipo", DbType.Int32);
                    SQLiteParameter id_setor = command.Parameters.Add("@id_setor", DbType.Int32);
                    SQLiteParameter id_sub_setor = command.Parameters.Add("@id_sub_setor", DbType.Int32);
                    SQLiteParameter id_segmento = command.Parameters.Add("@id_segmento", DbType.Int32);
                    SQLiteParameter id_fonte_dados = command.Parameters.Add("@id_fonte_dados", DbType.Int32);
                    SQLiteParameter atividade_principal = command.Parameters.Add("@atividade_principal", DbType.String);
                    SQLiteParameter web_site = command.Parameters.Add("@web_site", DbType.String);
                    SQLiteParameter status = command.Parameters.Add("@status", DbType.Int32);

                    foreach (var stock in summary)
                    {
                        //TODO: Generate ID's by Enums
                        if (stock.StockInfo.Code == "NULL") break;
                        codigo.Value = stock.StockInfo.Code;
                        nome.Value = stock.StockInfo.Name;
                        id_tipo.Value = 0;
                        id_setor.Value = 0;
                        id_sub_setor.Value = 0;
                        id_segmento.Value = 0;
                        id_fonte_dados.Value = 0;
                        id_tipo.Value = 0;
                        atividade_principal.Value = stock.StockInfo.Activity;
                        web_site.Value = stock.StockInfo.Site;
                        status.Value = stock.StockInfo.Status;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

            }

        }

        private void GenerateSymbolTableExtremeDB()
        {
            try
            {
                Database db = new Database(new ExtremedbWrapper());
                Database.Parameters parameters = new Database.Parameters();

                parameters.MemPageSize = PAGE_SIZE;
                parameters.Classes = new Type[] { typeof(Symbol) };

                // open & connect database
                db.Open("saveload-db", parameters, DATABASE_SIZE);

                Connection con = new Connection(db);
                foreach (SymbolsPS stock in StockList)
                {
                    con.StartTransaction(Database.TransactionType.ReadWrite);
                    //Symbol symbol = stock.StockInfo);
                    con.Insert(stock.StockInfo);
                    con.CommitTransaction();
                }

                // save database image to file
                Console.WriteLine("\tSave database...");
                con.SaveSnapshot(PathSymbols + "\\Plena.img");
                con.Disconnect();
                Console.WriteLine("\tClose database\n");
                db.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GenerateDiaryTable(List<SymbolsPS> summary)
        {
            using (_connectionSQLite = new SQLiteConnection((new SQLiteConnectionStringBuilder
            {
                DataSource = string.Format("{0}\\Plena.db", PathSymbols)
            }).ToString()))
            {
                _connectionSQLite.Open();
                SQLiteCommand command = _connectionSQLite.CreateCommand();
                //CREATE TABLE
                /*command.CommandText = "CREATE TABLE IF NOT EXISTS [" + "Ativos" + @"]
                                        (
                                          [NAME] TEXT NOT NULL,
                                          [CODE] TEXT NOT NULL,
                                          [SECTOR] TEXT NOT NULL,
                                          [SUBSECTOR] TEXT NOT NULL,
                                          [SEGMENT] TEXT NOT NULL,
                                          [SOURCE] TEXT NOT NULL,
                                          [TYPE] TEXT NOT NULL,
                                          [ACTIVITY] TEXT NOT NULL,
                                          [SITE] TEXT NOT NULL,
                                          [STATUS] INTEGER NOT NULL,
                                          PRIMARY KEY ([CODE]) ON CONFLICT REPLACE
                                        )";

                command.ExecuteNonQuery();*/
                using (SQLiteTransaction transaction = _connectionSQLite.BeginTransaction())
                {
                    //POPULATE TABLE
                    command.CommandText =
                        "INSERT INTO [Ativos] VALUES(@codigo, @nome, @id_tipo, @id_setor,@id_sub_setor, @id_segmento, @id_fonte_dados, @atividade_principal, @web_site, @status)";
                    SQLiteParameter codigo = command.Parameters.Add("@codigo", DbType.String);
                    SQLiteParameter nome = command.Parameters.Add("@nome", DbType.String);
                    SQLiteParameter id_tipo = command.Parameters.Add("@id_tipo", DbType.Int32);
                    SQLiteParameter id_setor = command.Parameters.Add("@id_setor", DbType.Int32);
                    SQLiteParameter id_sub_setor = command.Parameters.Add("@id_sub_setor", DbType.Int32);
                    SQLiteParameter id_segmento = command.Parameters.Add("@id_segmento", DbType.Int32);
                    SQLiteParameter id_fonte_dados = command.Parameters.Add("@id_fonte_dados", DbType.Int32);
                    SQLiteParameter atividade_principal = command.Parameters.Add("@atividade_principal", DbType.String);
                    SQLiteParameter web_site = command.Parameters.Add("@web_site", DbType.String);
                    SQLiteParameter status = command.Parameters.Add("@status", DbType.Int32);

                    foreach (var stock in summary)
                    {
                        //TODO: Generate ID's by Enums
                        if (stock.StockInfo.Code == "NULL") break;
                        codigo.Value = stock.StockInfo.Code;
                        nome.Value = stock.StockInfo.Name;
                        id_tipo.Value = 0;
                        id_setor.Value = 0;
                        id_sub_setor.Value = 0;
                        id_segmento.Value = 0;
                        id_fonte_dados.Value = 0;
                        id_tipo.Value = 0;
                        atividade_principal.Value = stock.StockInfo.Activity;
                        web_site.Value = stock.StockInfo.Site;
                        status.Value = stock.StockInfo.Status;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

            }

        }

        /// <summary>
        /// Finds a client by its ID and sends a binary encoded data packet.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="struct"></param>
        protected void SendStruct(Client client, IParserStruct @struct)
        {
            if (_server == null)
                throw new ArgumentNullException();
            _server.DoSendAsync(client, new[] { @struct });
        }

        protected bool SendTickData(DataServer.Interface.TickData tickData)
        {
            TickDataPS dataPs = new TickDataPS
                              {
                                  TickData = tickData
                              };

            lock (_symbolClients)
            {
                List<Client> clients;
                if (!_symbolClients.TryGetValue(tickData.Symbol, out clients))
                    return false;

                lock (clients)
                {
                    //foreach (Client client in clients)
                    for (int i = 0; i < clients.Count; i++)
                    {
                        SendStruct(clients[i], dataPs);
                    }
                }
            }

            DeleteDeadReferences();

            return true;
        }

        protected bool SendBarData(Client client, BarDataPS barDataPs)
        {
            SendStruct(client, barDataPs);
            return true;
        }

        protected void DoLog(string format, params object[] args)
        {
            Log(this, format, args);
        }

        private void DeleteDeadReferences()
        {
            lock (_symbolClients)
            {
                foreach (var pair in _symbolClients)
                {
                    List<Client> clients = pair.Value;
                    clients.RemoveAll(client1 => client1.Closed);
                }
            }
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
