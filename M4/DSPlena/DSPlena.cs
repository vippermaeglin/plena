using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using M4.DataServer.Interface;
using System.Diagnostics;
using M4.DataServer.Interface.ProtocolStructs;
using M4Data.MessageService;
using ModulusFE.Sockets;
using System.Threading;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace DSPlena
{
    public class DSPlena
    {
        private static DSPlena _dsPlena;
        public DateTime LastBarSyncDate = new DateTime();
        public bool Initialized = false;
        private int mainThread;
        private List<Symbol> StockList = new List<Symbol>();
        private List<string> StockListPortfolioAll = new List<string>();
        private List<SymbolGroup> Portfolios = new List<SymbolGroup>();
        private List<BarDataPS> BarDatas = new List<BarDataPS>();
        public bool ConnectedActive = false;
        private Queue<MSRequest> _messageRequests;
        private static bool Sincronizing = false;

        // TICK DATA EVENT
        public delegate void TickEventHandler(object sender, TickEventArgs args);
        public event TickEventHandler TickEvent = delegate { };

        // UPDATE DATA EVENT
        public delegate void BarDataEventHandler(object sender, BarDataEventArgs args);
        public event BarDataEventHandler UpdateEvent = delegate { };

        //INTERFACE
        static List<IPAddress> cmbDAWAddress = new List<IPAddress>();
        System.Timers.Timer tmrMessageService = new System.Timers.Timer(200);
        System.Timers.Timer tmrWakeDatabase = new System.Timers.Timer(60000);
        public string PathSymbol, PathHistorical;
        public static SeamusLog Log;

        //********************************************************************************************

        public List<string> StockNames = new List<string>();
        private Thread threadConnecting;
        private IPAddress IpHost;
        private int PortHost;
        private Thread threadAuthenticate;
        private Queue<int> PortHosts = new Queue<int>();
        private System.Timers.Timer _timerKeepAlive = new System.Timers.Timer(120000);
        private CancellationTokenSource _cancelAuthenticate = new CancellationTokenSource();

        // DATA RECIEVED FROM TCP DESTINED TO THREADS:
        public List<TickDataPS> IncomingTickData = new List<TickDataPS>();
        public List<BarsDatasPS> IncomingBarData = new List<BarsDatasPS>();
        public List<Symbol> IncomingSymbols = new List<Symbol>();
        public List<List<SymbolGroupPS>> IncomingSymbolGroups = new List<List<SymbolGroupPS>>();
        private List<SymbolSnapshotPS> IncomingSnapshots = new List<SymbolSnapshotPS>();

        
        public static DSPlena Instance()
        {
            return _dsPlena ?? (_dsPlena = new DSPlena());
        }

        /// <asa>
        /// A TCP client that will connect to DAW server
        /// </asa>
        private ClientRabbit _dawClient;

        private bool DawClientConnected
        {
            get { return _dawClient.ConnectedMain; }
        }

        private bool DawClientAuthenticated = false;
        /*{
            get { return _dawClient.Connected && _dawClient.Id != null; }
        }*/

        private List<string> _subscribedSymbols
            = new List<string>();


        //********************************************************************************************

        public DSPlena()
        {
            PathSymbol = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\";
            PathHistorical = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\HISTORICAL\\";
            Log = new SeamusLog(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + @"\Base\SYSTEM");
            ConnectedActive = false;

            tmrWakeDatabase.Enabled = true;


        }

        public void Start()
        {
            Log.Info("\n\nDSPlena succesfully started!");

            DS_MainForm_Load();
            DAW_InitClient();

            DAW_InitDatabase();
            
        }

        private void DS_MainForm_Load()
        {
            foreach (IPAddress address in Utils.LocalAddresses)
            {
                cmbDAWAddress.Add(address);
            }
            //cmbDAWAddress.Add(IPAddress.Parse("192.168.1.13"));
            //cmbDAWAddress.Add(IPAddress.Parse("54.187.159.71"));
            tmrMessageService.Elapsed += tmrMessageService_Tick;
            tmrMessageService.Start();
        }

        public static void SaveServerIpAdress(string ip)
        {
            cmbDAWAddress.Add(IPAddress.Parse(ip));       
        }
        public void Close()
        {
            DAW_ClientClose();
            Log.Flush();
            Log.Info("\n\nDSPlena succesfully ended!");
        }

        private void tmrWakeDatabase_Tick(object sender, EventArgs e)
        {
            SqlConnection _con = DBlocalSQL.Connect();
            DBlocalSQL.WakeDatabase(_con);
            DBlocalSQL.Disconnect(_con);
        }
        private void tmrMessageService_Tick(object sender, EventArgs e)
        {
            CheckAlerts();
        }

        private void CheckAlerts()
        {
            //Dont handle requests while sincronizing:
            tmrMessageService.Enabled = false;
            _messageRequests = MessageService.GetRequest(MSRequestOwner.DServer);
            MSRequest request;
            Queue<MSRequest> requestsWait = new Queue<MSRequest>();
            // Process messages:
            while (_messageRequests.Count > 0)
            {
                request = _messageRequests.Dequeue();
                switch (request.MSType)
                {
                    case MSRequestType.GetSymbolsList:
                        mainThread = Thread.CurrentThread.ManagedThreadId;
                        DAW_Initialize(true);
                        break;
                    case MSRequestType.GetHistoricalData:
                        
                        List<SymbolsPS> symbols = new List<SymbolsPS>();
                        foreach (string s in (List<string>)request.MSParams[0]) symbols.Add(new SymbolsPS() { StockInfo = new Symbol() { Code = s }, RequestId = request.ID });
                        Thread threadHistory = new Thread(() => ThreadRequestHistory(symbols, (int)request.MSParams[1]));
                        threadHistory.IsBackground = true;
                        threadHistory.Name = "Thread" + request.ID;
                        threadHistory.Start();

                        break;
                }
            }
            // Put back requests waiting:
            while (requestsWait.Count > 0)
            {
                MessageService.SubmitRequest(requestsWait.Dequeue());
            }
            tmrMessageService.Enabled = true;
        }
        private string GetMacAddress()
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

        private void ThreadAuthenticate()
        {
            try
            {
                Log.Info("Trying to Authenticate as " + _dawClient.Id);
                _dawClient.SendStruct(new AuthenticationPS
                {
                    ValueCount = 2,
                    Values = new[] { _dawClient.Id, "demo" }
                }, "channelServer"+PortHost, () => { });
                //Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("<ThreadAuth>" + ex.Message);
                return;
            }
        }

        private void ThreadRequestSymbols(string RequestID)
        {
            List<Symbol> resultSymbols = new List<Symbol>();
            Stopwatch timeout = new Stopwatch();
            Stopwatch timecount = new Stopwatch();

            //TODO: Try to reconnect X times before return error with STATUS=OFFLINE

            if (DawClientAuthenticated)
            {
                DAW_RequestSymbols(RequestID);
                timeout.Start();
                timecount.Start();
                while (timeout.ElapsedMilliseconds < 65000)
                {
                    if (timecount.ElapsedMilliseconds > 1000)
                    {
                        lock (IncomingSymbols)
                        {
                            try
                            {
                                //Disconnected:
                                if (timeout.ElapsedMilliseconds > 60000)
                                {
                                    ConnectedActive = false;
                                    Log.Info("ThreadRequestSymbol Time-Out!");
                                    DAW_Initialize(true);
                                    return;
                                }
                                if (IncomingSymbols.Count() <= 0)
                                    continue;
                                resultSymbols = new List<Symbol>(IncomingSymbols);
                                if (resultSymbols.Count > 0)
                                {
                                    Log.Info("Server sent " + resultSymbols.Count + " symbols");
                                    lock (Portfolios)
                                    {
                                        SqlConnection _connection2 = DBlocalSQL.Connect();
                                        SymbolGroup portfolioAll;
                                        lock (StockList)
                                        {
                                            //Save all symbols first:
                                            StockList = resultSymbols;
                                            DBlocalSQL.SaveSymbols(StockList,_connection2);

                                            //Use that to reset database:
                                            bool resetDB = false;

                                            if (resetDB)
                                            {
                                                List<Symbol> DeleteSymbols = new List<Symbol>(StockList);
                                                //Load Stocklist with sync:
                                                StockList = new List<Symbol>();
                                                StockList = DBlocalSQL.LoadSymbols(_connection2, true);
                                                foreach (Symbol s in DeleteSymbols)
                                                {
                                                    if (!StockList.Exists(d => d.Code == s.Code)) DBlocalSQL.RemoveBarDatas(s.Code, BaseType.Days, _connection2);
                                                }
                                            }
                                            else
                                            {
                                                //Load Stocklist with sync:
                                                StockList = new List<Symbol>();
                                                StockList = DBlocalSQL.LoadSymbols(_connection2, true);
                                            }

                                            //SymbolSync exists?
                                            if (StockList.Count == 0) //Create it if don't
                                            { 
                                                //Save Stocklist only with sync symbols:

                                                Portfolios = new List<SymbolGroup>();
                                                Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Index, _connection2));
                                                Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Portfolio, _connection2));
                                                bool generateAll = !Portfolios.Exists(p=>p.Name=="All");
                                                List<string> symbolsAll = new List<string>();
                                                //Generate All?
                                                if(generateAll)
                                                {
                                                    Portfolios.Insert(0, new SymbolGroup() { Name = "All",Symbols = "", Type = 1});
                                                }
                                                StockList = new List<Symbol>();
                                                foreach (Symbol symbol in resultSymbols)
                                                {
                                                    if (Portfolios.Exists(p => p.Symbols.Contains(symbol.Code)))
                                                    {
                                                        StockList.Add(symbol);
                                                        if (generateAll) symbolsAll.Add(symbol.Code);
                                                    }
                                                }
                                                DBlocalSQL.SaveSymbols(StockList, _connection2, true);
                                                if(generateAll)
                                                {
                                                    Portfolios[0].Symbols = string.Join(",", symbolsAll.ToArray()) ;
                                                    DBlocalSQL.SaveGroups(Portfolios,_connection2);
                                                }


                                            }
                                            Portfolios = new List<SymbolGroup>(){new SymbolGroup(){ Name = "All",Symbols = string.Join(",", StockListPortfolioAll.ToArray()), Type = 1}};
                                            Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Index, _connection2));
                                            Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Portfolio, _connection2));
                                            //StockListPortfolioAll = Portfolios.First(p=>p.Name=="All").Symbols.Split(new char[] { ',' }).ToList();
                                        }

                                        
                                        DBlocalSQL.Disconnect(_connection2);
                                    }
                                    break;
                                }
                            }
                            catch (ArgumentNullException NULL)
                            {
                                continue;
                            }
                            catch (InvalidOperationException OP)
                            {
                                continue;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine((
                                        "Exception not handled on ThreadRequestSymbols(): \n" +
                                        ex.Message));
                                //try again later:
                                Thread.Sleep(100000);
                                MessageService.SubmitRequest(new MSRequest(RequestID, MSRequestStatus.Pending,
                                                                                MSRequestType.GetSymbolsList,
                                                                                MSRequestOwner.M4,
                                                                                new object[] { }));
                            }
                            
                        }
                        timecount.Reset();
                        timecount.Start();
                    }
                }
               // Connection _connection = DBSymbolShared.Connect();
                if (true/*CreateSummaryDatabase()*/)
                {
                    MessageService.SubmitRequest(new MSRequest(RequestID, MSRequestStatus.Done,
                                                               MSRequestType.GetSymbolsList,
                                                               MSRequestOwner.DServer,
                                                               new object[] { }));

                    //Start requesting UPDATE for all symbols:
                    int i = 0;
                    ThreadPool
                        .QueueUserWorkItem(
                            o =>
                            {
                                lock (StockList)
                                {
                                    // Check\update DAY-data for all symbols:
                                    List<SymbolsPS> symbolsPS = new List<SymbolsPS>();
                                    foreach (string symbol in StockListPortfolioAll)
                                    {
                                        //i++;
                                        symbolsPS.Add(new SymbolsPS() { StockInfo = new Symbol() { Code = symbol }, RequestId = "INITIALIZE" + i });
                                    }
                                    BarDatas.Clear();
                                    Thread threadHistory =
                                        new Thread(
                                            () =>
                                            ThreadRequestHistory(symbolsPS, (int)BaseType.Days));
                                    threadHistory.IsBackground = true;
                                    threadHistory.Name = "INITIALIZE";
                                    threadHistory.Start();
                                    _evHistoryArived.WaitOne();


                                    Log.Info("Syncronization ALL succeeded!");
                                    MessageService.SubmitRequest(new MSRequest(null, MSRequestStatus.Done,
                                                                               MSRequestType.MessageStatus,
                                                                               MSRequestOwner.DServer,
                                                                               new object[] { MSMessageStatusType.UpdatedAll,LastBarSyncDate.ToShortDateString()}));

                                    Initialized = true;
                                    SqlConnection _con = DBlocalSQL.Connect();
                                    DBlocalSQL.SaveServerCommand(new CommandRequest()
                                    {
                                        CommandID = (int)ServerCommand.SyncUpdate,
                                        Date = DateTime.Now,
                                        Parameters = ""
                                    }, _con);
                                    DBlocalSQL.Disconnect(_con);

                                    /*
                                    //Still connected?
                                    if (!DawClientAuthenticated) return;
                                    // Check\update 15MIN-data for all symbols:
                                    foreach (string symbol in StockListPortfolioAll)
                                    {
                                        i++;
                                        BarDatas.Clear();
                                        Thread threadHistory =
                                            new Thread(
                                                () =>
                                                ThreadRequestHistory(
                                                    new SymbolsPS() { StockInfo = new Symbol() { Code = symbol }, RequestId = "INITIALIZE" + i }, (int)BaseType.Minutes15));
                                        threadHistory.IsBackground = true;
                                        threadHistory.Name = "INITIALIZE" + i;
                                        threadHistory.Start();
                                        _evHistoryArived.WaitOne();

                                    }
                                    //Still connected?
                                    if (!DawClientAuthenticated) return;

                                    // Check\update MIN-data for all symbols:
                                    foreach (string symbol in StockListPortfolioAll)
                                    {
                                        i++;
                                        BarDatas.Clear();
                                        Thread threadHistory =
                                            new Thread(
                                                () =>
                                                ThreadRequestHistory(
                                                    new SymbolsPS() { StockInfo = new Symbol() { Code = symbol }, RequestId = "INITIALIZE" + i }, (int)BaseType.Minutes));
                                        threadHistory.IsBackground = true;
                                        threadHistory.Name = "INITIALIZE" + i;
                                        threadHistory.Start();
                                        _evHistoryArived.WaitOne();

                                    }

                                    //Still connected?
                                    if (!DawClientAuthenticated) return;
                                    //Subscribe to all portfolios after updates:
                                    foreach (string symbol in StockListPortfolioAll)
                                    {
                                        Thread threadSubscribeDay =
                                            new Thread(() => ThreadSubscribeSymbol(symbol, BaseType.Days));
                                        threadSubscribeDay.IsBackground = true;
                                        threadSubscribeDay.Start();
                                        threadSubscribeDay.Name = "SUBSCRIBE DAY " + symbol;
                                        _evHistoryArived.WaitOne();
                                        /*Thread threadSubscribe15Min =
                                            new Thread(() => ThreadSubscribeSymbol(symbol, BaseType.Minutes15));
                                        threadSubscribe15Min.IsBackground = true;
                                        threadSubscribe15Min.Start();
                                        threadSubscribe15Min.Name = "SUBSCRIBE 15MIN " + symbol;
                                        _evHistoryArived.WaitOne();
                                        Thread threadSubscribeMin =
                                            new Thread(() => ThreadSubscribeSymbol(symbol, BaseType.Minutes));
                                        threadSubscribeMin.IsBackground = true;
                                        threadSubscribeMin.Start();
                                        threadSubscribeMin.Name = "SUBSCRIBE MIN " + symbol;
                                        _evHistoryArived.WaitOne();

                                    }*/
                                }
                            });
                    lock (StockList)
                    {
                        foreach (var stock in StockList)
                        {
                             StockNames.Add(stock.Code);
                        }
                        //cmbHstSymbol.DataSource = StockNames;
                        //cmbRTSymbol.DataSource = StockNames;
                        Log.Info("List created with " + StockList.Count + " symbols");
                    }
                    return;
                }
             //   DBSymbolShared.Disconnect(_connection);
            }
            else
            {
                //Shutdown socket and re-inicialize:
                Log.Info("ThreadRequestSymbol Not Authenticated!");
                ConnectedActive = false;
                DAW_Initialize(true);
            }
            //if (DBSymbolShared.IsStockListOk())
            //{
            //    MessageService.SubmitRequest(new MSRequest(RequestID, MSRequestStatus.Done,
            //                                                MSRequestType.GetSymbolsList,
            //                                                MSRequestOwner.DServer,
            //                                                new object[] { }));
            //}
            //else
            //{
            //    MessageService.SubmitRequest(new MSRequest(RequestID, MSRequestStatus.Failed,
            //                                                MSRequestType.GetSymbolsList,
            //                                                MSRequestOwner.DServer,
            //                                                new object[] { }));
            //}

        }

        private static Mutex mut = new Mutex();

        private void ThreadRequestHistory(List<SymbolsPS> SymbolRequests, int BaseType)
        {
            List<BarData> BarDatasLocal = new List<BarData>();
            List<List<BarData>> AllBars = new List<List<BarData>>();
            List<BarsDatasPS> resultAllBarDatas;
            string RequestId = SymbolRequests.First().RequestId;
            Stopwatch timeout = new Stopwatch();
            Stopwatch timecount = new Stopwatch();
            SqlConnection _connection;

            //_dawClient.ignoreParse = true;

            try
            {
                mut.WaitOne();


                //First time check data with server:
                if (/*RequestId.Contains("INITIALIZE") && */DawClientConnected)
                {
                    DAW_RequestHistory(SymbolRequests, (BaseType)BaseType);
                    timeout.Start();
                    timecount.Start();

                    while (timeout.ElapsedMilliseconds < 60000)
                    {
                        if (true/*timecount.ElapsedMilliseconds > 1000*/)
                        {
                            lock (IncomingBarData)
                            {
                                try
                                {
                                    //Ignore if data isnt ready:
                                    if ((IncomingBarData.Count(
                                        bar => bar.RequestId == RequestId) <= 0) || IncomingBarData.Count(bar=>bar.BarIndex == -1)<=0)
                                    {
                                        //Reset Time-Out if there's data incoming:
                                        timeout.Restart();
                                        timeout.Start();
                                        continue;
                                    }
                                    try
                                    {
                                        resultAllBarDatas =
                                            IncomingBarData.Where(bar => bar.RequestId == RequestId && bar.Data.Count() > 0).ToList();
                                    }
                                    catch (Exception) {
                                        IncomingBarData.Clear();
                                        resultAllBarDatas = null; }

                                    if (resultAllBarDatas !=null)
                                    {
                                        AllBars = GetDistinctBars(resultAllBarDatas);
                                        foreach (List<BarData> resultBarDatas in AllBars)
                                        { 
                                            BarDatasLocal = resultBarDatas.OrderBy(o=>o.TradeDate).ToList();
                                            while (IncomingBarData.Exists(bar => bar.RequestId == RequestId))
                                            {
                                                IncomingBarData.Remove(IncomingBarData.First(
                                                bar => bar.RequestId == RequestId));
                                            }
                                            try
                                            {
                                                _connection = DBlocalSQL.Connect();
                                                //Check if data need to be adjusted:
                                                BarData Last = DBlocalSQL.GetLastBarDataDisk(BarDatasLocal.Last().Symbol, (BaseType)BaseType, _connection);

                                                if (!Last.Equals(BarDatasLocal.Last()) && Last.TradeDate == BarDatasLocal.Last().TradeDate)
                                                {

                                                    DBlocalSQL.AdjustBarDatas(BarDatasLocal.First(), _connection);
                                                }
                                                Log.Info("Historical recieved " + (BarDatasLocal.Count) + " bars for symbol " + BarDatasLocal[0].Symbol + Enum.GetName(typeof(BaseType), (BaseType)BaseType) + ": " + RequestId);
                                                if (BarDatasLocal.Last().TradeDate > LastBarSyncDate) LastBarSyncDate = BarDatasLocal.Last().TradeDate;
                                                if (BarDatasLocal.Count > 1)
                                                {
                                                    if (DBlocalSQL.SaveBarDatas(BarDatasLocal, _connection))
                                                        Log.Info("Saved historical recieved " + (BarDatasLocal.Count) + " bars for symbol " + BarDatasLocal[0].Symbol + Enum.GetName(typeof(BaseType), (BaseType)BaseType) + ": " + RequestId);
                                                }
                                                DBlocalSQL.Disconnect(_connection);


                                                
                                                BarDatas.Clear();
                                                //if(IncomingBarData.Count()==0)return;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        if (RequestId.Contains("INITIALIZE"))
                                        {
                                            _evHistoryArived.Set();
                                        }
                                        else
                                        {

                                            //TODO: Refatorar comportamento antigo!
                                            Console.WriteLine("DS: Historical Ready - submited message");
                                            MessageService.SubmitRequest(new MSRequest(RequestId, MSRequestStatus.Done,
                                                                                        MSRequestType.GetHistoricalData,
                                                                                        MSRequestOwner.DServer,
                                                                                        new object[] { BarDatasLocal[0].Symbol, BaseType }));

                                            //Notify user:
                                            if (SymbolRequests.Count() > 1)
                                            {
                                                MessageService.SubmitRequest(new MSRequest(null, MSRequestStatus.Done,
                                                                                           MSRequestType.MessageStatus,
                                                                                           MSRequestOwner.DServer,
                                                                                           new object[] { MSMessageStatusType.InsertedSymbols , LastBarSyncDate.ToShortDateString() }));


                                            }
                                            else
                                            {
                                                MessageService.SubmitRequest(new MSRequest(null, MSRequestStatus.Done,
                                                                                           MSRequestType.MessageStatus,
                                                                                           MSRequestOwner.DServer,
                                                                                           new object[] { MSMessageStatusType.InsertedSymbol, LastBarSyncDate.ToShortDateString(), SymbolRequests.First().StockInfo.Code.ToUpper() }));


                                            }
                                        }
                                        return;
                                    }
                                }
                                catch (ArgumentNullException NULL)
                                {
                                    continue;
                                }
                                catch (InvalidOperationException OP)
                                {
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(
                                               "Exception not handled on ThreadRequestSymbols(): \n" +
                                               ex.Message);
                                }
                            }
                            timecount.Reset();
                            timecount.Start();
                        }
                    }

                    //Disconnected:
                    Log.Info("TIME-OUT: Historical not recieved for request INITIALIZE "  + Enum.GetName(typeof(BaseType), (BaseType)BaseType));
                    //Shutdown socket and re-inicialize:
                    ConnectedActive = false;
                    DAW_Initialize(true);
                    _evHistoryArived.Set();
                    //GetLocalHistory(SymbolRequest, M4.DataServer.Interface.BaseType.Days);
                }
                //else GetLocalHistory(SymbolRequest, M4.DataServer.Interface.BaseType.Days);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
               mut.ReleaseMutex();
            }

        }

        private List<List<BarData>> GetDistinctBars(List<BarsDatasPS> AllBars)
        {
            List<List<BarData>> result = new List<List<BarData>>();
            foreach (BarsDatasPS barPS in AllBars)
            {
                foreach (BarData bar in barPS.Data)
                {
                    int index = -1;
                    index = result.FindIndex(0, list =>
                                                      list.Count(
                                                          barData =>
                                                          barData.Symbol == bar.Symbol) > 0);
                    if (index != -1 && index != null) result[index].Add(bar);
                    else result.Add(new List<BarData>() { bar });
                }
            }
            return result;
        }

        private void ThreadSubscribeSymbol(string symbol, BaseType Base)
        {
            List<BarDataPS> BarDatasLocal = new List<BarDataPS>();
            List<BarDataPS> resultBarDatas;
            SymbolSnapshotPS resultSnapshot;
            Stopwatch timeout = new Stopwatch();
            Stopwatch timecount = new Stopwatch();
            bool subscribed = false;

            if (DawClientAuthenticated)
            {
                string requestId = "SUBSCRIBE " + Enum.GetName(typeof(BaseType), Base) + " " + symbol;
                DS_SubscribeToSymbol(symbol, Base, requestId);
                timeout.Start();
                timecount.Start();

                //First check bar's update with server:
                while (timeout.ElapsedMilliseconds < 90000)
                {
                    if (timecount.ElapsedMilliseconds > 1000)
                    {
                        lock (IncomingSnapshots)
                        {
                            lock (IncomingBarData)
                            {
                                try
                                {
                                    #region Wait for Sync-Update bars:
                                    /*if (IncomingBarData.Count(bar => bar.RequestId == requestId) > 0)
                                    {
                                        resultBarDatas =
                                            IncomingBarData.First(bar => bar.RequestId == requestId && bar.Data.Count()>0);
                                        if (resultBarDatas.Count > 0)
                                        {
                                            BarDatasLocal = resultBarDatas;
                                            IncomingBarData.RemoveAt(IncomingBarData.IndexOf(IncomingBarData.First(
                                                list => list.Find(bar => bar.RequestId == requestId) != null)));
                                            try
                                            {
                                                SqlConnection _connection = DBlocalSQL.Connect();

                                                //Check if data need to be adjusted:
                                                BarData Last =
                                                    DBlocalSQL.GetLastBarDataDisk(BarDatasLocal.Last().Data.Symbol,
                                                                                     Base,
                                                                                     _connection);

                                                if (!Last.Equals(BarDatasLocal.Last().Data) &&
                                                    Last.TradeDate == BarDatasLocal.Last().Data.TradeDate)
                                                {

                                                    DBlocalSQL.AdjustBarDatas(BarDatasLocal.First().Data, _connection);
                                                }
                                                if (BarDatasLocal.Count > 1)
                                                {
                                                    if (DBlocalSQL.SaveBarDatas(BarDatasLocal, _connection))
                                                        Log.Info("Updated bars recieved for subscribe symbol " +
                                                                      BarDatasLocal[0].Data.Symbol+" "+Enum.GetName(typeof(BaseType),BarDatasLocal[0].Data.BaseType));
                                                    timecount.Reset();
                                                    timecount.Start();
                                                }
                                                DBlocalSQL.Disconnect(_connection);

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            BarDatas.Clear();
                                            //break;
                                        }
                                    }
                                    #endregion

                                    #region Wait for snapshot:
                                    if (IncomingSnapshots.Where(snap => snap.RequestId == requestId).Count() > 0)
                                    {
                                        resultSnapshot =
                                            IncomingSnapshots.First(snap => snap.RequestId == requestId);
                                        if (resultSnapshot != null)
                                        {
                                            DAW_UpdateSnapshot(resultSnapshot);
                                            Log.Info("Recieved snapshot for " + requestId + Enum.GetName(typeof(BaseType), (BaseType)Base));
                                            IncomingSnapshots.Remove(
                                                IncomingSnapshots.First(snap => snap.RequestId == requestId));
                                            subscribed = true;
                                            _evHistoryArived.Set();
                                        }
                                    }
                                    */
                                    #endregion

                                }
                                catch (ArgumentNullException NULL)
                                {
                                    continue;
                                }
                                catch (InvalidOperationException OP)
                                {
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(
                                               "Exception not handled on ThreadRequestSymbols(): \n" +
                                               ex.Message);
                                }
                                finally
                                {
                                    timecount.Reset();
                                    timecount.Start();
                                }
                            }
                        }
                    }
                }
                if (!subscribed)
                {
                    #region Time-Out:
                    _evHistoryArived.Set();
                    Log.Info("TIME-OUT: Updated bars not recieved for subscribe symbol " + symbol + " " + Enum.GetName(typeof(BaseType), Base));
                    //Shutdown socket and re-inicialize:
                    ConnectedActive = false;
                    DAW_Initialize(true);
                    return;
                    #endregion
                }
            }

        }

        //********************************************************************************************

        private void DAW_InitClient()
        {
            _dawClient = new ClientRabbit(-1)
            {
                StandaloneUsage = true, //make sure we get exceptions
            };
            // Register structures to recieve on DAW Client:
            _dawClient.RegisterStructure(StructsIds.TickData_Id, typeof(TickDataPS));
            _dawClient.RegisterStructure(StructsIds.BarData_Id, typeof(BarDataPS));
            _dawClient.RegisterStructure(StructsIds.AuthenticationAnswer_Id, typeof(AuthenticationAnswerPS));
            _dawClient.RegisterStructure(StructsIds.Symbols_Id, typeof(SymbolsPS));
            _dawClient.RegisterStructure(StructsIds.SymbolSnapshot_Id, typeof(SymbolSnapshotPS));
            _dawClient.RegisterStructure(StructsIds.Ping_Id, typeof(PingPS));
            _dawClient.RegisterStructure(StructsIds.SymbolGroup_Id, typeof(SymbolGroupPS));
            _dawClient.RegisterStructure(StructsIds.BarsDatas_Id, typeof(BarsDatasPS));
            _dawClient.RegisterStructure(StructsIds.ListSymbols_Id, typeof(ListSymbolsPS));

            _dawClient.OnException
              += (client, exception, okToContinue) =>
              {
                  if (exception.Message.Contains("System.Threading.ThreadAbortException") || exception.Message.Contains("aborted")) return;
                  Log.Info("[DAWClient] Exception = " + exception + ". Disconnected = " + !okToContinue);
                  if (okToContinue != false) return;
                  _timerKeepAlive.Stop();
                  ConnectedActive = false;
                  _timerKeepAlive.Interval = 60000;
                  DAW_Initialize(true);

                  //see if we were in a middle of local historical data request
                  /*if (!string.IsNullOrEmpty(_historicalRequestId))
                  {
                      _historicalRequestId = string.Empty;
                      btnHstRequest.I(
                        () =>
                        {
                            btnHstRequest.Enabled = true;
                            btnHstRequest.Text = "Request";
                        });
                  }*/
              };

            //_dawClient.Log += (string format, params object[] args) => { Log.Info(string.Format(format,args); };
            
            _dawClient.OnConnected
              += action =>
              {
                  if (ConnectedActive) return;
                     ConnectedActive = true;
                     Log.Info("Connected on MUX!");
                     _dawClient.ConnectedMain = true;
                     //Enable Keep Alive:
                     _timerKeepAlive.Start();
                     _dawClient.Id = Guid.NewGuid().ToString();//GetMacAddress();
                     _dawClient.StartReceiving(_dawClient.Id);
                     Log.Info("Start recieving on channel " + _dawClient.Id);
                     DawClientAuthenticated = false;
                     if (threadAuthenticate != null && threadAuthenticate.IsAlive)
                     {
                         Debug.WriteLine(string.Format("Waiting threadAuthenticate finishing!"));
                         threadAuthenticate.Join();
                         if (threadAuthenticate.IsAlive) Debug.WriteLine(string.Format("threadAuthenticate finished!"));
                         else Debug.WriteLine(string.Format("threadAuthenticate NOT finished!"));
                     }
                     _cancelAuthenticate = new CancellationTokenSource();
                     threadAuthenticate = new Thread(() => ThreadAuthenticate());
                     threadAuthenticate.IsBackground = true;
                     threadAuthenticate.Name = "ThreadAuthenticate";
                     threadAuthenticate.Start();

                 };

            _dawClient.OnLog += (message) => Log.Info("[Client]"+message);
            _dawClient.DataReceived += DawClientOnDataReceived;
            _timerKeepAlive.Elapsed += timerKeepAlive_Tick;
            _timerKeepAlive.AutoReset = false;


        }

        private void DAW_InitDatabase()
        {
            //Initialize Database:
            DBlocalSQL.LogDB += (format, objects) => Log.Info("[DBSymbolShared] "+string.Format(format, objects));


            //Get symbols's list and portfolios:
            SqlConnection _connection = DBlocalSQL.Connect();

            Stopwatch timer = new Stopwatch();
            long t1, t2;
            timer.Start();

            //_connection.Open();
            //SqlCommand cmd = new SqlCommand("SELECT * FROM BaseDay", _connection);
            //SqlDataReader rdr = cmd.ExecuteReader();
            //_connection.Close();


            //Initialize Views:
            //DBlocalSQL.GetPortfolioView(_connection);
            //DBlocalSQL.GetBarDataAll("AMBV4", 1, (int)Periodicity.Daily, _connection);
            //t1 = timer.ElapsedMilliseconds;
            //_connection.Open();
            //SqlCommand cmd2 = new SqlCommand("SELECT * FROM BaseDay", _connection);
           // SqlDataReader rdr2 = cmd2.ExecuteReader();
            //_connection.Close();

            //DBlocalSQL.GetBarDataAll("AMBV4", 1, (int)Periodicity.Daily, _connection);
            //t2 = timer.ElapsedMilliseconds - t1;
            //Console.WriteLine("\n\tInitialize SQL "+t1+"ms and "+t2+"ms");

            lock (StockList)
            {
                StockList = DBlocalSQL.LoadSymbols(_connection);

                lock (Portfolios)
                {
                    Portfolios = DBlocalSQL.LoadGroups(GroupType.Index, _connection);
                    Portfolios.AddRange( DBlocalSQL.LoadGroups(GroupType.Portfolio, _connection));
                    StockListPortfolioAll = new List<string>();
                    foreach(Symbol asset in DBlocalSQL.LoadSymbols(_connection,true)) StockListPortfolioAll.Add(asset.Code);
                    
                }
                //Load all database to memory:

                DBlocalSQL.GetPortfolioView(_connection, StockListPortfolioAll, false);
                //DBlocalSQL.TestDatabase(_connection);
                Console.WriteLine("\n\tInitialize SQL "+timer.ElapsedMilliseconds+"ms");

                /*foreach (SymbolsPS symbol in StockList)
                {
                    DBlocalSQL.GetBarDataAll(symbol.StockInfo.Code, 1, (int)Periodicity.Daily, _connection);
                }*/
            }

            //Check last syncronization with HUB:
            CommandRequest lastCheck = DBlocalSQL.GetLastServerCommand(_connection, (int)ServerCommand.SyncUpdate);
            Initialized = lastCheck.Date.Date == DateTime.Now.Date;
            if (Initialized) Initialized = !(lastCheck.Date.Hour < 19 && DateTime.Now.Hour > 19);

            DBlocalSQL.Disconnect(_connection);
            tmrWakeDatabase.Elapsed += tmrWakeDatabase_Tick;
            tmrWakeDatabase.Start();

            //Connect to historical daily database:
            //DBDailyShared.LogDB += (format, objects) => Log.Info("[DBDailyShared] "+string.Format(format, objects));
            //DBDailyShared.OpenDatabase("Daily", typeof(BarData), typeof(BarDataDisk), PathHistorical);
            //Connection _connection2 = DBDailyShared.Connect();
            //int countDay = DBDailyShared.Count(BaseType.Days, _connection2);
            //int countMin = DBDailyShared.Count(BaseType.Minutes, _connection2);
            //DBDailyShared.Disconnect(_connection2);
           // Log.Info("Historical database opened with " + countDay + " Day's bars and " + countMin + " Min's bars");

            /*******************************/
            /*********  TESTE  *************/
            /*******************************/
            return;

      //      Stopwatch time1 = new Stopwatch();
            
      //      string conString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=\"C:\\Users\\Admin\\Desktop\\PLENA\\M4\\trunk\\M4 C# DDF\\PlenaData.mdf\";Integrated Security=True";
      //      SqlConnection localdbCon = new SqlConnection(conString);

      //      time1.Start();

      //      //Get all data from extremeDB and insert on localDB
      //      localdbCon.Open();
      //      SqlCommand cmd = new SqlCommand("DELETE FROM Symbols", localdbCon);
      //      cmd.ExecuteNonQuery();
      //      cmd = new SqlCommand("DELETE FROM BaseDay", localdbCon);
      //      cmd.ExecuteNonQuery();

      //      foreach(SymbolsPS symbol in StockList)
      //      {
      //          cmd = new SqlCommand("INSERT INTO Symbols (Code) VALUES ('"+symbol.StockInfo.Code+"')",localdbCon);
      //          //cmd.Parameters.Add(new SqlParameter( symbol.StockInfo.Code, SqlDbType.NVarChar));
      //          cmd.ExecuteNonQuery();
      //      }

      //      Console.WriteLine("\nLocalDB Symbols table created in {0} ms with {1} symbols!",time1.ElapsedMilliseconds,StockList.Count());

      //      time1.Stop();
      //      time1.Reset();
      //      time1.Start();

      //      long count = 0;
      ////      Connection _conDB = DBDailyShared.Connect();
      //      foreach (SymbolsPS symbol in StockList)
      //      {

      // //         foreach(BarData bar in DBDailyShared.GetBarDataAll(symbol.StockInfo.Code,1,(int)Periodicity.Daily,_conDB)){
      //              cmd = new SqlCommand("INSERT INTO BaseDay (DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD) VALUES (" + bar.TradeDateTicks + ", N'" + bar.TradeDate.ToString("yyyy-MM-dd HH:mm:ss") + "', N'" + bar.Symbol + "', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)", localdbCon);
      //              //cmd.Parameters.Add(new SqlParameter( symbol.StockInfo.Code, SqlDbType.NVarChar));
      //              cmd.ExecuteNonQuery();
      //              count++;
      //          }
           // }

           // Console.WriteLine("\nBase Daily table created in {0} ms with {1} candles!", time1.ElapsedMilliseconds, count);
        }

        public void DAW_Initialize(bool forceConnection)
        {
            if (ConnectedActive)
            {
                Log.Info("Trying to initialize but there's another connection active!");
                return;
            }
            //Shutdown socket
            if (_dawClient.ConnectedMain) DAW_ClientClose();

            _timerKeepAlive.Stop();

            //Clean data buffers:
            IncomingBarData.Clear();
            IncomingSymbols.Clear();
            IncomingTickData.Clear();

            //Re-connect:
            DAW_ClientConnect(forceConnection);


        }

        private void DAW_ClientConnect(bool forceConnection)
        {
            //if(Thread.CurrentThread.ManagedThreadId!=mainThread) return;
            try
            {
                IpHost = cmbDAWAddress[cmbDAWAddress.Count - 1];

                if (PortHosts.Count > 1)
                {
                    // Alternate for another port:
                    int aux = PortHosts.Dequeue();
                    PortHosts.Enqueue(aux);
                    PortHost = PortHosts.First();
                }
                else PortHost = 2000;
                Log.Info("Trying to connect on IP " + IpHost + " and PORT " + PortHost);
                _dawClient.Init();
                _dawClient.Connect(IpHost, PortHost);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        private void DAW_ClientDisconnect()
        {
            if (threadConnecting.IsAlive) threadConnecting.Abort();
            _dawClient.Close();
        }

        private bool DawCheckClientConnected()
        {
            if (DawClientConnected)
                return true;

            Log.Info("DAW Client not connected");
            return false;
        }

        private void DawClientOnDataReceived(ClientRabbit client, IParserStruct istruct)
        {
            //Reset Keep-Alive if enabled:
            if (_timerKeepAlive.Enabled)
            {
                _timerKeepAlive.Stop();
                _timerKeepAlive.Start();
            }
            /*if (structures[0].Id == StructsIds.BarData_Id)
            {
                List<BarDataPS> bar = new List<BarDataPS>();
                //received historical data, forward to DS_Server, it will redirect to correct client
                foreach (IParserStruct parserStruct in structures)
                {
                    bar.Add((BarDataPS)parserStruct);
                }
                bar[0].Data.TradeDateTicks = bar[0].Data.TradeDate.Ticks;
                lock (IncomingBarData)
                {
                    int index = -1;
                    index = IncomingBarData.FindIndex(0,
                                                      list =>
                                                      list.Count(
                                                          barData =>
                                                          barData.Data.Symbol == bar[0].Data.Symbol &&
                                                          barData.RequestId == bar[0].RequestId) > 0);
                    if (index != -1 && index != null) IncomingBarData[index].AddRange(bar);
                    else IncomingBarData.Add(new List<BarDataPS>(bar));
                }
            }
            else*/ 
            
            //foreach (IParserStruct istruct in structures)
                //{
                    switch (istruct.Id)
                    {
                        case StructsIds.TickData_Id:
                            /*lock (IncomingTickData)
                            {
                                IncomingTickData.Add((TickDataPS) structure);
                            }*/
                            TickDataPS tickDataPs = (TickDataPS)istruct;
                            DAW_BroadcastTickData(tickDataPs.TickData);
                            /*TODO: Volume deriva em 3 tipos:
                                * Ações = Quantidade de Ações
                                * Financeiro = Quantidade de Ações x Preço
                                * Negócios = Quantidade de Transações (1 por tick)
                            */
                            break;
                        case StructsIds.BarData_Id:
                            //received historical data, forward to DS_Server, it will redirect to correct client
                            /*BarDataPS bar = (BarDataPS)istruct;
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            lock (IncomingBarData)
                            {
                                int index = -1;
                                index = IncomingBarData.FindIndex(0,
                                                                  list =>
                                                                  list.Count(
                                                                      barData =>
                                                                      barData.Data.Symbol == bar.Data.Symbol &&
                                                                      barData.RequestId == bar.RequestId) > 0);
                                if (index != -1 && index != null) IncomingBarData[index].Add(bar);
                                else IncomingBarData.Add(new List<BarDataPS>() { bar });
                            }*/
                            break;
                        case StructsIds.BarsDatas_Id:
                            //received historical data, forward to DS_Server, it will redirect to correct client
                            BarsDatasPS bars = (BarsDatasPS)istruct;
                            lock (IncomingBarData)
                            {
                                /*int index = -1;
                                index = IncomingBarData.FindIndex(0,
                                                                  list =>
                                                                  list.RequestId == bars.RequestId);
                                if (index != -1 && index != null) IncomingBarData[index]=bars;
                                else*/ IncomingBarData.Add( bars );
                            }
                            break;
                        case StructsIds.AuthenticationAnswer_Id:
                            AuthenticationAnswerPS auth = (AuthenticationAnswerPS)istruct;
                            _dawClient.Id = auth.ClientId;
                            DawClientAuthenticated = true;
                            Log.Info("Authenticated!");


                            //First time just get servers on Mux Connectora and retry on real Muxes:
                            if (auth.ConnectionType == (int)ConnectionType.Connector)
                            {
                                PortHosts.Clear();
                                foreach (DataServerInfo server in auth.DataServers)
                                {
                                    PortHosts.Enqueue(server.Port);
                                }
                                //Rotate:
                                for (int i = 0; i < PortHosts.Count - 1; i++)
                                {
                                    int aux = PortHosts.Dequeue();
                                    PortHosts.Enqueue(aux);
                                }
                                Log.Info("Recieved server's list from Connector, re-trying connections...");
                                ConnectedActive = false;
                                DAW_Initialize(true);
                                return;
                            }

                            //Request inicialization if NOT INITIALIZED:
                            if (!Initialized)
                            {
                                //Thread.Sleep(60000);
                                Thread threadSymbol = new Thread(() => ThreadRequestSymbols("INITIALIZE"));
                                threadSymbol.IsBackground = true;
                                threadSymbol.Name = "ThreadInitialize";
                                threadSymbol.Start();
                            }
                            else Log.Info("Symbols are already updated for today!");
                            break;

                        /*case StructsIds.Symbols_Id:
                            Symbol summary = ((SymbolsPS)istruct).StockInfo;
                            lock (StockList)
                            {
                                if (StockList.Count > 0)
                                {
                                    if (StockList.Last().Code == "NULL")
                                    {
                                        if (summary.Code == "NULL") return;
                                        else StockList.Clear();
                                    }
                                }
                                if (summary.Code != "NULL")
                                {
                                    int index = -1;
                                    index = StockList.FindIndex(s => s.Code == summary.Code);
                                    if (index >= 0) StockList[index] = summary;
                                    else StockList.Add(summary);
                                }
                                else
                                {
                                    lock (IncomingSymbols)
                                    {
                                        IncomingSymbols.Add(StockList);
                                    }
                                }
                            }
                            //MessageBox.Show("Summary ADDED:" + summary.StockInfo.Code);
                            break;*/
                        case StructsIds.ListSymbols_Id:
                            ListSymbolsPS summary = (ListSymbolsPS)istruct;
                            lock (IncomingSymbols)
                            {
                                IncomingSymbols = new List<Symbol>(StockList);
                            }
                            break;
                        case StructsIds.SymbolGroup_Id:
                            SymbolGroup group = ((SymbolGroupPS)istruct).GroupInfo;
                            SqlConnection _con = DBlocalSQL.Connect();
                            Portfolios = DBlocalSQL.LoadGroups(GroupType.Index, _con);
                            if (Portfolios.Exists(p => p.Name == group.Name && p.Type == group.Type)) DBlocalSQL.UpdateGroup(group, _con);
                            else
                            {
                                //Order portfolios by type:
                                List<SymbolGroup> portIndex = Portfolios.FindAll(p => p.Type == 1);
                                List<SymbolGroup> portUser = DBlocalSQL.LoadGroups(GroupType.Portfolio, _con);
                                if (group.Type == 1) portIndex.Add(group);
                                else if (group.Type == 0) portUser.Add(group);
                                Portfolios = new List<SymbolGroup>(portIndex);
                                Portfolios.AddRange(portUser);
                                DBlocalSQL.SaveGroups(Portfolios, _con);
                            }
                            DBlocalSQL.Disconnect(_con);
                            break;
                        case StructsIds.SymbolSnapshot_Id:
                            SymbolSnapshotPS snapshotPs = (SymbolSnapshotPS)istruct;
                            lock (IncomingSnapshots)
                            {
                                int index = -1;
                                index = IncomingSnapshots.FindIndex(0, snap =>
                                                                       snap.Snapshot.Symbol == snapshotPs.Snapshot.Symbol &&
                                                                       snap.RequestId == snapshotPs.RequestId);
                                if (index != -1 && index != null) IncomingSnapshots.RemoveAt(index);
                                IncomingSnapshots.Add(snapshotPs);
                            }
                            break;
                        case StructsIds.Ping_Id:
                            PingPS pingPS = (PingPS)istruct;
                            //Log.Info("[DawClient] Recieved KEEP ALIVE!");
                            if (_timerKeepAlive.Interval != pingPS.Ping.IntervalKeepAliveMillis)
                            {
                                _timerKeepAlive.Interval = pingPS.Ping.IntervalKeepAliveMillis;
                                Log.Info("Keep Alive set to " + _timerKeepAlive.Interval + " milliseconds!");
                            }
                            break;
                        case StructsIds.CommandRequest_Id:
                            CommandRequestPS comPS = (CommandRequestPS)istruct;
                            switch((ServerCommand)comPS.Request.CommandID)
                            {
                                case ServerCommand.RemoveAllBars:
                                    ThreadPool.QueueUserWorkItem(state =>
                                    {
                                        Log.Info("Recieved server command to REMOVE for All!");
                                            DAW_RemoveAllBars();
                                        });
                                    break;
                                case ServerCommand.RemoveBarsForSymbol:
                                    ThreadPool.QueueUserWorkItem(state =>
                                    {
                                        Log.Info("Recieved server command to REMOVE for " + comPS.Request.Parameters + "!");
                                            DAW_RemoveBarsForSymbol(comPS.Request.Parameters);
                                        });
                                    break;
                                case ServerCommand.ReSyncronize:
                                    ThreadPool.QueueUserWorkItem(state =>
                                    {
                                        Log.Info("Recieved server command to RESYNCRONIZE!");
                                        Thread threadSymbol = new Thread(() => ThreadRequestSymbols("INITIALIZE"));
                                        threadSymbol.IsBackground = true;
                                        threadSymbol.Name = "ThreadInitialize";
                                        threadSymbol.Start();
                                    });
                                    break;
                            }
                            break;
                        default:
                            Debug.WriteLine("[DawClient] Received a wrong datastructure. Id = " + istruct.Id);
                            break;
                    }
                //}
        }

        private void DAW_RemoveAllBars()
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            DBlocalSQL.RemoveAllBarDatas(BaseType.Days,_connection);
            DBlocalSQL.Disconnect(_connection);
        }

        private void DAW_RemoveBarsForSymbol(string Symbol)
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            DBlocalSQL.RemoveBarDatas(Symbol, BaseType.Days, _connection);
            DBlocalSQL.Disconnect(_connection);
        }

        private void DAW_BroadcastTickData(TickData tickData)
        {
            //with no delay send data further to DS clients
            //DS_BroadcastTickData(tickData);

            Log.Info("Recieved tick data for symbol " + tickData.Symbol);
            //save to local cache
            SqlConnection _connection = DBlocalSQL.Connect();
            if (DBlocalSQL.SaveTick(tickData, _connection)) ;// TickEvent(this, new TickEventArgs(tickData));
            DBlocalSQL.Disconnect(_connection);
            //TickDataFilesManager.Instance.WriteData(tickData.Symbol, new TickDataFile.Data(tickData.TradeDate, (decimal)tickData.Price, tickData.Quantity));
            //ERROR: Generating exception on Client::EndRecieve() because .NET version!!!
            //{"Mixed mode assembly is built against version 'v2.0.50727' of the runtime and cannot be loaded in the 4.0 runtime without additional configuration information.":null}
            //FIXED: Added "<startup useLegacyV2RuntimeActivationPolicy="true"><supportedRuntime version="v2.0"/>" on DAWServer::App.config

            //update local tickdata cache
            //ThreadPool.QueueUserWorkItem(state => UpdateTickDataCache(tickData));
        }

        private void DAW_UpdateSnapshot(SymbolSnapshotPS snapshotPS)
        {

            DBlocalSQL.SaveSnapshot(snapshotPS.Snapshot, (BaseType)snapshotPS.BaseType);

        }

        private void DAW_ClientClose()
        {
            if (!DawClientConnected)
                return;
            _dawClient.Dispose();
        }

        private bool DS_SubscribeToSymbol(string symbol, BaseType Base, string requestId)
        {
            return DS_SubscribeToSymbol(symbol, Base, requestId, true);
        }

        private bool DS_SubscribeToSymbol(string symbol, BaseType Base, string requestId, bool subscribe)
        {
            //if (_subscribedSymbols.ContainsKey(symbol))
            //  return true;

            if (!DawCheckClientConnected())
                return false;
            SqlConnection _connection = DBlocalSQL.Connect();
            BarData lastbBarData = DBlocalSQL.GetLastBarDataDiskOrMemory(symbol, Base, _connection);
            DBlocalSQL.Disconnect(_connection);
            if (!_subscribedSymbols.Contains(symbol))
            {
                _subscribedSymbols.Add(symbol);
            }

            _dawClient.SendStruct(new SubscribeSymbolPS
            {
                Subscribe = subscribe,
                LastBar = lastbBarData,
                ClientId = _dawClient.Id,
                RequestId = requestId
            }, "channelServer" + PortHost,
                                  () => Log.Info("Subscribe request to symbol " + symbol + Enum.GetName(typeof(BaseType), Base) + " sent."));
            return true;
        }

        private readonly AutoResetEvent _evHistoryArived = new AutoResetEvent(false);

        public void GetLocalHistory(SymbolsPS SymbolsRequest, BaseType Base)
        {
            try
            {
                SqlConnection _connection = DBlocalSQL.Connect();
                List<BarData> Bars = DBlocalSQL.GetBarDataAll(SymbolsRequest.StockInfo.Code, 1, (int)Periodicity.Daily, _connection);


                DBlocalSQL.Disconnect(_connection);
                if (Bars.Count > 0)
                {
                    MessageService.SubmitRequest(new MSRequest(SymbolsRequest.RequestId, MSRequestStatus.Done,
                                                               MSRequestType.GetHistoricalData,
                                                               MSRequestOwner.DServer,
                                                               new object[] { SymbolsRequest.StockInfo.Code, Base }));
                }
                else MessageService.SubmitRequest(new MSRequest(SymbolsRequest.RequestId, MSRequestStatus.Failed,
                                                           MSRequestType.GetHistoricalData,
                                                           MSRequestOwner.DServer,
                                                           new object[] { SymbolsRequest.StockInfo.Code, Base }));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DAW_RequestHistory(List<SymbolsPS> SymbolsRequests, BaseType Base)
        {
            try
            {
                System.Diagnostics.Stopwatch timeEllapsed = new Stopwatch();
                //Get last data from extremeDB:
                SqlConnection _connection = DBlocalSQL.Connect();
                List<BarData> lastValue = DBlocalSQL.GetLastBarDataAll(Base, _connection);
                DBlocalSQL.Disconnect(_connection);

                /*if (!DawCheckClientConnected() && lastValue.TradeDate != DateTime.MinValue)
                {
                    if (!SymbolsRequest.RequestId.Contains("INITIALIZE")) GetLocalHistory(SymbolsRequest, Base);
                    return;
                }*/

                //MessageBox.Show("[DBManagerShared] GetLastBarData() Time = " + timeEllapsed.ElapsedMilliseconds);
                timeEllapsed.Stop();
                int Interval = 1;
                Periodicity Periodicity = M4.DataServer.Interface.Periodicity.Daily;
                if (Base == BaseType.Minutes)
                {
                    Periodicity = Periodicity.Minutely;
                }
                else if (Base == BaseType.Minutes15)
                {
                    Periodicity = Periodicity.Minutely;
                    Interval = 15;
                }
                //_historicalRequestId = Guid.NewGuid().ToString();
                HistoryRequestPS req = new HistoryRequestPS
                {
                    ClientId = _dawClient.Id,
                    MuxId = "NULL",
                    RequestCount = SymbolsRequests.Count(),
                    Request = new List<HistoryRequest>()
                };
                foreach(SymbolsPS symbol in SymbolsRequests)
                {
                    if (!lastValue.Exists(v => v.Symbol == symbol.StockInfo.Code)) lastValue.Add(new BarData() { Symbol = symbol.StockInfo.Code });
                    req.Request.Add(new HistoryRequest()
                    {
                            RequestId = symbol.RequestId,
                            Periodicity = Periodicity,
                            BarSize = Interval,
                            Symbol = symbol.StockInfo.Code,
                            LastRecordValue =
                                lastValue.First(v => v.Symbol == symbol.StockInfo.Code).ClosePrice,
                            LastRecordTime =
                                lastValue.First(v => v.Symbol == symbol.StockInfo.Code).TradeDate
                        });
                }
                _dawClient.SendStruct(req, "channelServer" + PortHost, () => Log.Info("HUB historical request sent for ALL " /*+ SymbolsRequest.StockInfo.Code*/ + Enum.GetName(typeof(BaseType), Base)));
                Log.Info("HUB historical request sent for ALL");
                //_evHistoryArived.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DAW_RequestSymbols(string RequestId)
        {
            if (!DawCheckClientConnected())
            {
                lock (StockList)
                {

                    //if (DBSymbolShared.IsStockListOk())
                    //{
                    //    MessageService.SubmitRequest(new MSRequest(RequestId, MSRequestStatus.Done,
                    //                                               MSRequestType.GetSymbolsList,
                    //                                               MSRequestOwner.DServer,
                    //                                               new object[] { }));
                    //}
                    //else
                    //{
                    //    MessageService.SubmitRequest(new MSRequest(RequestId, MSRequestStatus.Failed,
                    //                                               MSRequestType.GetSymbolsList,
                    //                                               MSRequestOwner.DServer,
                    //                                               new object[] { }));
                    //}
                }
                return;
            }

            SymbolRequestPS req = new SymbolRequestPS { RequestId = RequestId, ClientId = _dawClient.Id, MuxId = "NULL"/*, LastUpdate = lastCommand.Date*/ };
            
            _dawClient.SendStruct(req, "channelServer" + PortHost, () => Log.Info("Summary request sent"));
        }

        public bool CreateSummaryDatabase()
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            lock (StockList)
            {
                if (StockList.Count < 1)
                {
                    return false;
                }




                if (!DBlocalSQL.SaveSymbols(StockList, _connection)) return false;
            }
            DBlocalSQL.Disconnect(_connection);
            return true;
        }

        public void CreateSummaryXML()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode nodeRoot = xmlDocument.AppendChild(xmlDocument.CreateElement("ATIVOS"));
            lock (StockList)
            {
                foreach (var stock in StockList)
                {
                    if (stock.Code == "NULL") break;
                    XmlNode nodeChild = nodeRoot.AppendChild(xmlDocument.CreateElement("ATIVO"));
                    XmlNode NOME = nodeChild.AppendChild(xmlDocument.CreateElement("NOME"));
                    NOME.InnerText = stock.Name;
                    XmlNode COD = nodeChild.AppendChild(xmlDocument.CreateElement("COD"));
                    COD.InnerText = stock.Code;
                    XmlNode SETOR = nodeChild.AppendChild(xmlDocument.CreateElement("SETOR"));
                    SETOR.InnerText = stock.Sector;
                    XmlNode SUBSETOR = nodeChild.AppendChild(xmlDocument.CreateElement("SUBSETOR"));
                    SUBSETOR.InnerText = stock.SubSector;
                    XmlNode SEGMENTO = nodeChild.AppendChild(xmlDocument.CreateElement("SEGMENTO"));
                    SEGMENTO.InnerText = stock.Segment;
                    XmlNode FONTE = nodeChild.AppendChild(xmlDocument.CreateElement("FONTE"));
                    FONTE.InnerText = stock.Source;
                    XmlNode TIPO = nodeChild.AppendChild(xmlDocument.CreateElement("TIPO"));
                    TIPO.InnerText = stock.Type;
                    XmlNode ATIVIDADE = nodeChild.AppendChild(xmlDocument.CreateElement("ATIVIDADE"));
                    ATIVIDADE.InnerText = stock.Activity;
                    XmlNode SITE = nodeChild.AppendChild(xmlDocument.CreateElement("SITE"));
                    SITE.InnerText = stock.Site;
                    XmlNode STATUS = nodeChild.AppendChild(xmlDocument.CreateElement("STATUS"));
                    STATUS.InnerText = stock.Status.ToString();
                }
            }

            if (File.Exists(PathSymbol + "Symbols.xml"))
                File.Delete(PathSymbol + "Symbols.xml");

            xmlDocument.Save(PathSymbol + "Symbols.xml");
        }

        private void timerKeepAlive_Tick(object sender, EventArgs e)
        {
            //return;
            Log.Info("RE-CONNECTING... KEEP ALIVE FAILED!");
            ConnectedActive = false;
            _timerKeepAlive.Stop();
            _timerKeepAlive.Interval = 120000;
            DAW_Initialize(true);
        }

    }
}
