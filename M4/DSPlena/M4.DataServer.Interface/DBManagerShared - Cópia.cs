using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using M4.DataServer.Interface.ProtocolStructs;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace M4.DataServer.Interface
{
    public class BarEventArgs : EventArgs
    {
        public BarData Bar { get; set; }
        public int Base { get; set; }
        public bool Append { get; set; }
        public BarEventArgs(BarData bar, int baseType, bool append)
        {
            Bar = bar;
            Base = baseType;
            Append = append;
        }
    }

    public class SnapshotEventArgs : EventArgs
    {
        public SymbolSnapshot Snapshot { get; set; }
        public int Base { get; set; }
        public SnapshotEventArgs(SymbolSnapshot snapshot, int baseType)
        {
            Snapshot = snapshot;
            Base = baseType;
        }
    }

    //public class DBSymbolShared
    //{
    //    private static string _path;
    //    private static Database _database;
    //    //private static Connection _connection;
    //    private static List<Connection> _connections = new List<Connection>();
    //    private static Database.Parameters _parameters;
    //    private static string _name;
    //    private static Database.Mode _mode;
    //    private static Database.Device[] _devices;
    //    private const int _database_size = 16 * 1024 * 1024; //16MB
    //    private const int _page_size = 128; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
    //    private const int _cache_size = 8 * 1024 * 1024;
    //    private const int _disk_page_size = 4096; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
    //    private static bool _initialized = false;
    //    private static bool _stockListOk = false;
    //    private static bool _portfoliosOk = false;

    //    //private static DataBaseManager _instance;

    //    // SYMBOLS LIST UPDATED: 
    //    public delegate void SymbolsEventHandler(object sender, EventArgs args);
    //    public static event SymbolsEventHandler UpdateSymbolsEvent = delegate { };


    //    public delegate void LogHandler(string format, params object[] args);
    //    public static event LogHandler LogDB = delegate { };


    //    /// <summary>
    //    /// Create unique instance for database persistent on disk with shared memory.
    //    /// </summary>
    //    /// <param name="Name">Database file name.</param>
    //    /// <param name="ClassType">Class type.</param>
    //    /// <param name="Path">Database file path.</param>
    //    public static void OpenDatabase(string Name, string Path)
    //    {
    //        if (_initialized) return;
    //        _name = Name;
    //        _path = Path;
    //        _parameters = new Database.Parameters();
    //        _parameters.MemPageSize = _page_size;
    //        _parameters.Classes = new Type[] { typeof(Symbol), typeof(SymbolGroup) };
    //        _parameters.MaxConnections = 10;
    //        _mode = 0;
    //        // PersistentShared Mode:
    //        _parameters.DiskPageSize = _disk_page_size;
    //        _parameters.DiskClassesByDefault = true; // mark @Persistent classes as on-disk classes by default
    //        _mode |= Database.Mode.DiskSupport;
    //        _mode |= Database.Mode.SharedMemorySupport;
    //        _mode |= Database.Mode.MVCCTransactionManager;
    //        _mode |= Database.Mode.DebugSupport;
    //        _devices = new Database.Device[]
    //                        {
    //                            new Database.SharedMemoryDevice(
    //                                Database.Device.Kind.Data, _name, new IntPtr(0), _database_size),
    //                            new Database.SharedMemoryDevice(
    //                                Database.Device.Kind.DiskCache, _name + "-cache", new IntPtr(0), _cache_size),
    //                            new Database.FileDevice(
    //                                Database.Device.Kind.Data, _path + _name + ".dbs"),
    //                            new Database.FileDevice(
    //                                Database.Device.Kind.TransactionLog,
    //                                _path + _name + ".log")
    //                        };
    //        try
    //        {
    //            _database = new Database(_mode);
    //            _database.Open(_name, _parameters, _devices);
    //            _initialized = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //        }
    //    }
    //    /// <summary>
    //    /// Connect to database.
    //    /// </summary>
    //    /// <returns>Returns true if succeeded</returns>
    //    public static Connection Connect()
    //    {
    //        try
    //        {
    //            if (!_initialized) return null;
    //            _connections.Add(new Connection(_database));
    //            return _connections.Last();
    //        }
    //        catch (DatabaseError dbe)
    //        {
    //            if (dbe.errorCode != 66)
    //            {
    //                // code 66 means duplicate instance. Valid case for SHM configuration
    //                if (dbe.errorCode == 62)
    //                    LogDB(
    //                        "eXtremeDB assembly is not compatible with option 'sharedmemory'. Please replace reference to assembly with shared memory functionality");
    //                else if (dbe.errorCode == 620000)
    //                    LogDB(
    //                        "eXtremeDB assembly is not compatible with option 'disk'. Please replace reference to assembly with disk manager functionality");


    //            }
    //        }
    //        return null;
    //    }
    //    /// <summary>
    //    /// Disconnect from database.
    //    /// </summary>
    //    /// <returns>Returns true if succeded.</returns>
    //    public static bool Disconnect(Connection _connection)
    //    {
    //        try
    //        {
    //            _connection.Disconnect();
    //            _connections.Remove(_connection);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB("Disconnect() " + ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>
    //    /// Get Symbol's List from database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<SymbolsPS> LoadSymbols(Connection _connection)
    //    {
    //        List<SymbolsPS> StockList = new List<SymbolsPS>();
    //        try
    //        {
    //            // open read-only transaction
    //            _connection.StartTransaction(Database.TransactionType.ReadOnly);
    //            // Open cursor by "id" index
    //            int StockID = 0;
    //            foreach (Symbol symbol in _connection.GetTable<Symbol>())
    //            {
    //                // get all objects
    //                StockList.Add(new SymbolsPS
    //                {
    //                    StockId = StockID,
    //                    StockInfo = symbol
    //                });
    //                StockID++;
    //            }
    //            //The last register is StockInfo="NULL" and StockID = Registers Count
    //            /*
    //            if (StockList.Last().StockInfo.Code != "NULL")
    //            {
    //                StockList.Add(new SymbolsPS
    //                {
    //                    StockId = StockID,
    //                    StockInfo = new Symbol
    //                    {
    //                        Name = "NULL",
    //                        Code = "NULL",
    //                        Sector = "NULL",
    //                        SubSector = "NULL",
    //                        Segment = "NULL",
    //                        Source = "NULL",
    //                        Type = "NULL",
    //                        Activity = "NULL",
    //                        Site = "NULL",
    //                        Status = 0
    //                    }
    //                });
    //            }*/
    //            _connection.RollbackTransaction(); // end transaction
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB("LoadSymbol() " + ex.Message);
    //        }
    //        if (StockList.Count > 0) _stockListOk = true;
    //        return StockList;
    //    }
    //    /// <summary>
    //    /// Get a group of symbols (index or portfolio) from database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static SymbolGroup LoadGroup(string Name, GroupType Type, Connection _connection)
    //    {
    //        SymbolGroup group = new SymbolGroup();
    //        try
    //        {
    //            // open read-only transaction
    //            _connection.StartTransaction(Database.TransactionType.ReadOnly);
    //            // Open cursor by "id" index
    //            int StockID = 0;
    //            foreach (SymbolGroup sgroup in _connection.GetTable<SymbolGroup>())
    //            {
    //                if (sgroup.Name == Name && sgroup.Type == (int)Type) group = sgroup;
    //            }
    //            _connection.RollbackTransaction(); // end transaction
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //        }
    //        return group;
    //    }
    //    /// <summary>
    //    /// Get a group's List (indexes or portfolios) from database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<SymbolGroup> LoadGroups(GroupType Type, Connection _connection)
    //    {
    //        List<SymbolGroup> groups = new List<SymbolGroup>();
    //        try
    //        {
    //            // open read-only transaction
    //            _connection.StartTransaction(Database.TransactionType.ReadOnly);
    //            foreach (SymbolGroup sgroup in _connection.GetTable<SymbolGroup>())
    //            {
    //                if (sgroup.Type == (int)Type) groups.Add(sgroup);
    //            }
    //            _connection.RollbackTransaction(); // end transaction
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //        }
    //        if (groups.Count > 0) _portfoliosOk = true;
    //        return groups;
    //    }
    //    /// <summary>
    //    /// Save symbols to database.
    //    /// </summary>
    //    /// <param name="StockList"></param>
    //    /// <returns>Returns true if succeeded</returns>
    //    public static bool SaveSymbols(List<SymbolsPS> StockList, Connection _connection)
    //    {
    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            //Clean old symbols:
    //            _connection.RemoveAll(typeof(Symbol));
    //            //Save portfolios:
    //            foreach (SymbolsPS symbol in StockList)
    //            {
    //                _connection.Insert(symbol.StockInfo);
    //            }
    //            _connection.CommitTransaction();
    //            /*
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            Cursor<Symbol> cursor = new Cursor<Symbol>(_connection, "Code");
    //            foreach (SymbolsPS stock in StockList)
    //            {
    //                if (cursor.Search(Operation.Equals, stock.StockInfo.Code))
    //                {
    //                    cursor.MoveNext();
    //                    cursor.Remove();
    //                }
    //                _connection.Insert(stock.StockInfo);
    //            }
    //            _connection.CommitTransaction();*/
    //            _stockListOk = true;
    //            // save database image to file
    //            LogDB("\tSaved {0} symbols database...", StockList.Count);
    //            UpdateSymbolsEvent(new object(), new EventArgs());
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB("SaveSymbol() " + ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>
    //    /// Save\subscribe user's portfolios to database.
    //    /// </summary>
    //    /// <param name="Portfolios"></param>
    //    /// <returns></returns>
    //    public static bool SaveGroups(List<SymbolGroup> Groups, Connection _connection)
    //    {
    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            //Clean old portfolios:
    //            _connection.RemoveAll(typeof(SymbolGroup));
    //            //Save portfolios:
    //            foreach (SymbolGroup group in Groups)
    //            {
    //                _connection.Insert(group);
    //            }
    //            _connection.CommitTransaction();

    //            // save database image to file
    //            LogDB("\tSaved portfolio database...");
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }
    //    public static bool IsStockListOk()
    //    {
    //        return _stockListOk;
    //    }
    //    public static bool IsPortfolioOk()
    //    {
    //        return _portfoliosOk;
    //    }
    //    public static bool RemoveGroup(string Name, Connection _connection)
    //    {
    //        bool removed = false;
    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            Cursor<SymbolGroup> cursor = new Cursor<SymbolGroup>(_connection);
    //            foreach (var symbolGroup in cursor)
    //            {
    //                if (symbolGroup.Name == Name)
    //                {
    //                    cursor.Remove();
    //                    removed = true;
    //                    continue;
    //                }
    //                if (removed)
    //                {
    //                    symbolGroup.Index = symbolGroup.Index - 1;
    //                    cursor.Current.Index = symbolGroup.Index;
    //                    cursor.Update();
    //                }
    //            }
    //            _connection.CommitTransaction();
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //        }
    //        return removed;
    //    }
    //    /// <summary>
    //    /// Clear all type's data from database.
    //    /// </summary>
    //    /// <param name="Class">Class type "Symbol" or "SymbolGroup".</param>
    //    /// <returns>Returns true if succeeded.</returns>
    //    public static bool Clear(Type Class, Connection _connection)
    //    {
    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            if (Class == typeof(Symbol)) _connection.RemoveAll(typeof(Symbol));
    //            if (Class == typeof(SymbolGroup)) _connection.RemoveAll(typeof(SymbolGroup));
    //            _connection.CommitTransaction();
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }

    //    public static int Count(Type Class, Connection _connection)
    //    {
    //        int count = 0;
    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadOnly);
    //            if (Class == typeof(Symbol)) count = _connection.GetTable<Symbol>().Count();
    //            if (Class == typeof(SymbolGroup)) count = _connection.GetTable<SymbolGroup>().Count();
    //            _connection.RollbackTransaction();
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);
    //        }
    //        return count;
    //    }

    //}

    //public class DBDailyShared
    //{

    //    #region Synchronization events:

    //    // DATABASE UPDATED: 
    //    public delegate void BaseEventHandler(object sender, BarDataEventArgs args);
    //    public static event BaseEventHandler UpdateBaseEvent = delegate { };

    //    // NEW CANDLE CREATED
    //    public delegate void BarEventHandler(object sender, BarEventArgs args);
    //    public static event BarEventHandler NewBarEvent = delegate { };

    //    // SNAPSHOT UPDATED
    //    public delegate void SnapshotEventHandler(object sender, SnapshotEventArgs args);
    //    public static event SnapshotEventHandler SnapshotEvent = delegate { };


    //    public delegate void LogHandler(string format, params object[] args);


    //    public static event LogHandler LogDB = delegate { };
    //    #endregion

    //    #region Members
    //    private static System.Diagnostics.Stopwatch timer = new Stopwatch();
    //    private static string _path;
    //    private static Database _database;
    //    //private static Connection _connection;
    //    private static List<Connection> _connections = new List<Connection>();
    //    private static Connection _connection2;
    //    private static Database.Parameters _parameters;
    //    private static string _name;
    //    private static Database.Mode _mode;
    //    private static Type _class;
    //    private static Type _classM;
    //    private static Database.Device[] _devices;
    //    private const int _database_size = 256 * 1024 * 1024; //256MB
    //    private const int _page_size = 1024; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
    //    private const int _cache_size = 64 * 1024 * 1024;
    //    private const int _disk_page_size = 32768; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
    //    private static bool _initialized = false;
    //    private static Mutex _mutexCon = new Mutex();
    //    private static readonly AutoResetEvent _syncConnect = new AutoResetEvent(true);
    //    #endregion

    //    #region Snapshots
    //    //Bars on memory database:
    //    private static List<BarData> StockBarsOnMemoryDay = new List<BarData>();
    //    private static List<BarData> StockBarsOnMemoryMin = new List<BarData>();
    //    private static List<BarData> StockBarsOnMemoryMin15 = new List<BarData>();
    //    //Represents last time bars updated with server:
    //    private static List<SymbolSnapshot> LastSnapshotDay = new List<SymbolSnapshot>();
    //    private static List<SymbolSnapshot> LastSnapshotMin = new List<SymbolSnapshot>();
    //    private static List<SymbolSnapshot> LastSnapshotMin15 = new List<SymbolSnapshot>();
    //    //Represents all ticks (not stored on disk):
    //    private static List<List<TickData>> TicksOnMemory = new List<List<TickData>>();
    //    //Represents ticks not processed on base:
    //    private static List<List<TickData>> TicksOnBufferDay = new List<List<TickData>>();
    //    private static List<List<TickData>> TicksOnBufferMin = new List<List<TickData>>();
    //    private static List<List<TickData>> TicksOnBufferMin15 = new List<List<TickData>>();
    //    //Represents new candles built by ticks:
    //    private static List<BarData> NewCandlesDay = new List<BarData>();
    //    private static List<BarData> NewCandlesMin = new List<BarData>();
    //    private static List<BarData> NewCandlesMin15 = new List<BarData>();
    //    #endregion


    //    public static void OpenDatabase(string Name, Type ClassType, Type ClassMType, string Path)
    //    {
    //        if (_initialized) return;
    //        _name = Name;
    //        _path = Path;
    //        _class = ClassType;
    //        _classM = ClassMType;
    //        _parameters = new Database.Parameters();
    //        _parameters.MemPageSize = _page_size;
    //        _parameters.Classes = new Type[] { _class, _classM };
    //        _parameters.MaxConnections = 10000;
    //        _mode = 0;
    //        // PersistentShared Mode:
    //        _parameters.DiskPageSize = _disk_page_size;
    //        _parameters.DiskClassesByDefault = false; // mark @Persistent classes as on-disk classes by default
    //        _mode |= Database.Mode.DiskSupport;
    //        _mode |= Database.Mode.SharedMemorySupport;
    //        _mode |= Database.Mode.MVCCTransactionManager;
    //        _mode |= Database.Mode.DebugSupport;
    //        _devices = new Database.Device[]
    //                        {
    //                            new Database.SharedMemoryDevice(
    //                                Database.Device.Kind.Data, _name, new IntPtr(0), _database_size),
    //                            new Database.SharedMemoryDevice(
    //                                Database.Device.Kind.DiskCache, _name + "-cache", new IntPtr(0), _cache_size),
    //                            new Database.FileDevice(
    //                                Database.Device.Kind.Data, _path + _name + ".dbs"),
    //                            new Database.FileDevice(
    //                                Database.Device.Kind.TransactionLog,
    //                                _path + _name + ".log")
    //                        };
    //        try
    //        {
    //            _database = new Database(_mode);
    //            //_database = new Database(new ExtremeDB.ExtremedbWrapper());
    //            _database.Open(_name, _parameters, _devices);
    //            _database.GenerateMcoFile(_path + _name + ".mco"); // generate database schema file
    //            _initialized = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);

    //        }
    //    }


    //    public static Connection Connect()
    //    {
    //        try
    //        {
    //            if (!_initialized || _database == null || _connections == null /*|| ExtremeDB.ExtremedbWrapper == null*/)
    //            {
    //                return null;
    //            }
    //            //_mutexCon.WaitOne();
    //            //_syncConnect.WaitOne();
    //            //int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
    //            //byte[] context = BitConverter.GetBytes(pid);
    //            _connections.Add(new Connection(_database/*, context*/));
    //            //_mutexCon.ReleaseMutex();
    //            //_syncConnect.Set();
    //            return _connections.Last();
    //        }
    //        catch (DatabaseError dbe)
    //        {
    //            if (dbe.errorCode == 125)
    //            {
    //                File.Delete(_path + _name + ".mco");
    //            }
    //            if (dbe.errorCode != 66)
    //            {
    //                // code 66 means duplicate instance. Valid case for SHM configuration
    //                if (dbe.errorCode == 62)
    //                    LogDB(


    //                        "eXtremeDB assembly is not compatible with option 'sharedmemory'. Please replace reference to assembly with shared memory functionality");
    //                else if (dbe.errorCode == 620000)
    //                    LogDB(


    //                        "eXtremeDB assembly is not compatible with option 'disk'. Please replace reference to assembly with disk manager functionality");


    //            }
    //        }
    //        catch (Exception exception)
    //        {
    //            LogDB(exception.Message);
    //        }
    //        return null;
    //    }

    //    public static bool Disconnect(Connection _connection)
    //    {
    //        try
    //        {
    //            if (!_initialized) return false;
    //            _connection.Disconnect();
    //            _connections.Remove(_connection);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB("Disconnect() " + ex.Message);
    //        }
    //        return true;
    //    }

    //    public static List<BarData> GetBarDataAll(string Symbol, int Interval, int Periodicity, Connection _connection)
    //    {
    //        List<BarData> All = new List<BarData>();
    //        bool addInMemory = false;
    //        BaseType Base = BaseType.Days;
    //        if (Periodicity == (int)Interface.Periodicity.Minutely)
    //        {
    //            if (Interval % 15 == 0) Base = BaseType.Minutes15;
    //            else Base = BaseType.Minutes;
    //        }
    //        if (Periodicity == (int)Interface.Periodicity.Hourly) Base = BaseType.Minutes15;

    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            /*
    //            var query = from bar in _connection.GetTable<BarData>()
    //                        where bar.Symbol == Symbol
    //                        orderby bar.TradeDate
    //                        select bar;
    //            if (query.Count() > 0)
    //            {
    //                LastBar = query.Last();
    //            }*/
    //            Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbol");
    //            Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbol");
    //            //Search in memory:
    //            if (cursor.Search(Operation.Equals, new object[] { Base, Symbol }))
    //            {
    //                while (cursor.MoveNext())
    //                {
    //                    if (cursor.Current.Symbol == Symbol && cursor.Current.BaseType == (int)Base)
    //                    {
    //                        All.Add(cursor.Current);
    //                    }
    //                    else break;
    //                }
    //            }
    //            //Search on Disk:
    //            else if (cursorDisk.Search(Operation.Equals, new object[] { Base, Symbol }))
    //            {
    //                while (cursorDisk.MoveNext())
    //                {
    //                    if (cursorDisk.Current.Symbol == Symbol && cursorDisk.Current.BaseType == (int)Base)
    //                    {
    //                        All.Add(new BarData(cursorDisk.Current));
    //                        //Backup data on memory:
    //                        _connection.Insert(All.Last());
    //                        if (!addInMemory) addInMemory = true;
    //                    }
    //                    else break;
    //                }
    //            }
    //            _connection.CommitTransaction(); // end transaction
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message + "\n" + ex.StackTrace);
    //        }
    //        if (All.Count > 1)
    //        {
    //            //if (All[0].TradeDate > All[1].TradeDate) All.Reverse();
    //            All = All.OrderBy(b => b.TradeDateTicks).ToList();
    //            if (addInMemory)
    //            {
    //                switch (Base)
    //                {
    //                    case BaseType.Days:
    //                        lock (StockBarsOnMemoryDay)
    //                        {
    //                            StockBarsOnMemoryDay.Add(All.Last());
    //                        }
    //                        break;
    //                    case BaseType.Minutes:
    //                        lock (StockBarsOnMemoryMin)
    //                        {
    //                            StockBarsOnMemoryMin.Add(All.Last());
    //                        }
    //                        break;
    //                    case BaseType.Minutes15:
    //                        lock (StockBarsOnMemoryMin15)
    //                        {
    //                            StockBarsOnMemoryMin15.Add(All.Last());
    //                        }
    //                        break;
    //                }
    //            }
    //            if ((Periodicity == (int)Interface.Periodicity.Daily && Interval > 1) || (Periodicity == (int)Interface.Periodicity.Minutely && Interval > 1 && Interval != 15) || (Periodicity == (int)Interface.Periodicity.Hourly) || (Periodicity > (int)Interface.Periodicity.Daily)) All = ProcessPeriodicity(All, Interval, Periodicity, _connection);
    //        }
    //        return All;
    //    }

    //    public static double[] GetLastVolumes(string Symbol, int Interval, int Periodicity, Connection _connection)
    //    {
    //        double[] result = new double[10];
    //        List<BarData> barAll = GetBarDataAll(Symbol, Interval, Periodicity, _connection);
    //        if (barAll.Count > 10)
    //        {
    //            for (int i = barAll.Count - 10; i < barAll.Count; i++)
    //            {
    //                result[i - (barAll.Count - 10)] = barAll[i].VolumeF;
    //            }
    //        }
    //        else
    //        {
    //            int j = barAll.Count();
    //            for (int i = 0; i < 10 - j; i++)
    //            {
    //                result[i] = 0;
    //            }
    //            for (int i = 10 - j; i < 10; i++)
    //            {
    //                result[i] = barAll[i - 10 + j].VolumeF;
    //            }
    //        }
    //        SymbolSnapshot lastSnapshot = GetSnapshot(Symbol, BaseType.Days, _connection);
    //        if (lastSnapshot != null) result[10] = lastSnapshot.VolumeFinancial;
    //        return result;
    //    }

    //    public static List<BarData> GetBarDataSince(string Symbol, BarData Last, int Interval, int Periodicity, Connection _connection)
    //    {
    //        List<BarData> All = new List<BarData>();
    //        bool addInMemory = false;
    //        BaseType Base = BaseType.Days;
    //        if (Periodicity == (int)Interface.Periodicity.Minutely)
    //        {
    //            if (Interval % 15 == 0) Base = BaseType.Minutes15;
    //            else Base = BaseType.Minutes;
    //        }
    //        if (Periodicity == (int)Interface.Periodicity.Hourly) Base = BaseType.Minutes15;

    //        try
    //        {
    //            //_connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            /*
    //            var query = from bar in _connection.GetTable<BarData>()
    //                        where bar.Symbol == Symbol
    //                        orderby bar.TradeDate
    //                        select bar;
    //            if (query.Count() > 0)
    //            {
    //                LastBar = query.Last();
    //            }*/
    //            Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbol");
    //            Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbol");
    //            //Search in memory:
    //            if (cursor.Search(Operation.Equals, new object[] { Base, Symbol }))
    //            {
    //                while (cursor.MoveNext())
    //                {
    //                    if (cursor.Current.Symbol == Symbol && cursor.Current.BaseType == (int)Base && cursor.Current.TradeDate > Last.TradeDate)
    //                    {
    //                        All.Add(cursor.Current);
    //                        if (!addInMemory) addInMemory = true;
    //                    }
    //                    else break;
    //                }
    //            }
    //            //Search on Disk:
    //            else if (cursorDisk.Search(Operation.Equals, new object[] { Base, Symbol }))
    //            {
    //                _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //                while (cursorDisk.MoveNext())
    //                {
    //                    if (cursorDisk.Current.Symbol == Symbol && cursorDisk.Current.BaseType == (int)Base && cursorDisk.Current.TradeDate > Last.TradeDate)
    //                    {
    //                        All.Add(new BarData(cursorDisk.Current));
    //                        //Backup data on memory:
    //                        _connection.Insert(All.Last());
    //                    }
    //                    else break;
    //                }
    //                _connection.CommitTransaction(); // end transaction
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message + "\n" + ex.StackTrace);
    //        }
    //        if (All.Count > 1)
    //        {
    //            if (All[0].TradeDate > All[1].TradeDate) All.Reverse();
    //            if (addInMemory)
    //            {
    //                switch (Base)
    //                {
    //                    case BaseType.Days:
    //                        lock (StockBarsOnMemoryDay)
    //                        {
    //                            StockBarsOnMemoryDay.Add(All.Last());
    //                        }
    //                        break;
    //                    case BaseType.Minutes:
    //                        lock (StockBarsOnMemoryMin)
    //                        {
    //                            StockBarsOnMemoryMin.Add(All.Last());
    //                        }
    //                        break;
    //                    case BaseType.Minutes15:
    //                        lock (StockBarsOnMemoryMin15)
    //                        {
    //                            StockBarsOnMemoryMin15.Add(All.Last());
    //                        }
    //                        break;
    //                }
    //            }
    //            if (!(Interval == 1 && (Periodicity == (int)Interface.Periodicity.Daily || Periodicity == (int)Interface.Periodicity.Minutely) || (Interval % 15 == 0 && Periodicity == (int)Interface.Periodicity.Minutely))) All = ProcessPeriodicity(All, Interval, Periodicity.GetHashCode(), _connection);
    //        }
    //        return All;
    //    }
    //    public static BarData GetLastBarDataDiskOrMemory(string Symbol, BaseType Base, Connection _connection)
    //    {
    //        try
    //        {
    //            //Try in memory:
    //            switch (Base)
    //            {
    //                case BaseType.Days:
    //                    lock (StockBarsOnMemoryDay)
    //                    {
    //                        if (StockBarsOnMemoryDay.Count(s => s.Symbol == Symbol) > 0)
    //                            return StockBarsOnMemoryDay.Find(s => s.Symbol == Symbol);

    //                    }
    //                    break;
    //                case BaseType.Minutes:
    //                    lock (StockBarsOnMemoryMin)
    //                    {
    //                        if (StockBarsOnMemoryMin.Count(s => s.Symbol == Symbol) > 0)
    //                            return StockBarsOnMemoryMin.Find(s => s.Symbol == Symbol);

    //                    }
    //                    break;
    //                case BaseType.Minutes15:
    //                    lock (StockBarsOnMemoryMin15)
    //                    {
    //                        if (StockBarsOnMemoryMin15.Count(s => s.Symbol == Symbol) > 0)
    //                            return StockBarsOnMemoryMin15.Find(s => s.Symbol == Symbol);

    //                    }
    //                    break;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message + "\n" + ex.StackTrace);
    //        }
    //        //Finally try on disk:
    //        return GetLastBarDataDisk(Symbol, Base, _connection);
    //    }

    //    public static BarData GetLastBarDataDisk(string ISymbol, BaseType IBase, Connection I_connection)
    //    {
    //        string Symbol = ISymbol;
    //        BaseType Base = IBase;
    //        Connection _connection = I_connection;
    //        BarData LastBar = new BarData() { Symbol = Symbol, TradeDate = new DateTime(), ClosePrice = 0, BaseType = (int)Base };//Get last data from extremeDB:

    //        try
    //        {
    //            _connection.StartTransaction(Database.TransactionType.ReadOnly);

    //            Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbol");


    //            //Search on Disk:
    //            if (cursorDisk.Search(Operation.Equals, new object[] { Base, Symbol }))
    //            {
    //                while (cursorDisk.MoveNext())
    //                {
    //                    if (cursorDisk.Current.Symbol == Symbol && cursorDisk.Current.BaseType == (int)Base)
    //                    {
    //                        if (cursorDisk.Current.TradeDate > LastBar.TradeDate) LastBar = new BarData(cursorDisk.Current);
    //                        //Backup data on memory:
    //                        //_connection.Insert(cursorDisk.Current);
    //                    }
    //                    else break;
    //                }
    //            }
    //            _connection.CommitTransaction(); // end transaction
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message + "\n" + ex.StackTrace);
    //        }
    //        return LastBar;
    //    }

    //    public static TickData GetLastBarDataTick(string Symbol, Connection _connection)
    //    {
    //        List<TickData> ticks = TicksOnMemory.Find(list => list.Count(s => s.Symbol == Symbol) > 0);
    //        if (ticks == null) return new TickData() { Buyer = -1, Price = 0, Quantity = 0, Seller = -1, Symbol = "NULL", TradeDate = new DateTime() };
    //        return ticks.Last();
    //    }

    //    public static List<BarData> GetStockBarsOnMemory(BaseType Base, Connection _connection)
    //    {
    //        List<BarData> result = new List<BarData>();
    //        switch (Base)
    //        {
    //            case BaseType.Days:
    //                lock (StockBarsOnMemoryDay)
    //                {
    //                    result = StockBarsOnMemoryDay;
    //                }
    //                break;
    //            case BaseType.Minutes:
    //                lock (StockBarsOnMemoryMin)
    //                {
    //                    result = StockBarsOnMemoryMin;
    //                }
    //                break;
    //            case BaseType.Minutes15:
    //                lock (StockBarsOnMemoryMin15)
    //                {
    //                    result = StockBarsOnMemoryMin15;
    //                }
    //                break;
    //        }
    //        return result;
    //    }

    //    public static List<BarData> ProcessPeriodicity(List<BarData> BasicBars, int Interval, int Periodicity, Connection _connection)
    //    {
    //        //long timeEllapsed = timer.ElapsedMilliseconds;
    //        Periodicity Period = (Periodicity)Periodicity;
    //        List<BarData> NewBars = new List<BarData>();

    //        int i = 0;

    //        BaseType Base = (BaseType)BasicBars[0].BaseType;

    //        //User as reference 10:00 to 17:00:
    //        DateTime timeReference = BasicBars[0].TradeDate.Date;
    //        DateTime nextTimeReference = BasicBars[0].TradeDate.Date;
    //        //Minutes:
    //        if ((Base == BaseType.Minutes || Base == BaseType.Minutes15) && Period != Interface.Periodicity.Hourly)
    //        {
    //            timeReference = timeReference.AddHours(10);
    //            nextTimeReference = nextTimeReference.AddHours(10);
    //            nextTimeReference = nextTimeReference.AddMinutes(Interval);
    //        }
    //        //Hours:
    //        else if (Period == Interface.Periodicity.Hourly)
    //        {
    //            timeReference = timeReference.AddHours(10);
    //            nextTimeReference = nextTimeReference.AddHours(10);
    //            nextTimeReference = nextTimeReference.AddHours(Interval);
    //        }
    //        //Days:
    //        else if (Period == Interface.Periodicity.Daily)
    //        {
    //            nextTimeReference = nextTimeReference.AddDays(Interval);
    //        }
    //        /*//Weeks:
    //        else if(Period == Interface.Periodicity.Weekly)
    //        {
    //            nextTimeReference = nextTimeReference.AddHours(5 * Interval);
    //        }
    //        //Months:
    //        else if (Period == Interface.Periodicity.Month)
    //        {
    //            nextTimeReference = nextTimeReference.AddHours(22 * Interval);
    //        }
    //        //Years:
    //        else if (Period == Interface.Periodicity.Year)
    //        {
    //            nextTimeReference = nextTimeReference.AddHours(260 * Interval);
    //        }*/

    //        while (i < BasicBars.Count)
    //        {
    //            // Test if there was any negociation on interval:
    //            if (Period < Interface.Periodicity.Weekly)
    //            {
    //                if (BasicBars[i].TradeDate >= nextTimeReference)
    //                {
    //                    //Minutes:
    //                    if ((Base == BaseType.Minutes || Base == BaseType.Minutes15) &&
    //                        Period != Interface.Periodicity.Hourly)
    //                    {
    //                        timeReference = timeReference.AddMinutes(Interval);
    //                        nextTimeReference = nextTimeReference.AddMinutes(Interval);
    //                        if (timeReference.Hour > 17)
    //                        {
    //                            timeReference = nextTimeReference = nextTimeReference.AddDays(1).Date;
    //                            timeReference = nextTimeReference = nextTimeReference.AddHours(10);
    //                            nextTimeReference = nextTimeReference.AddMinutes(Interval);
    //                        }
    //                    }
    //                    //Hours:

    //                    else if (Period == Interface.Periodicity.Hourly)
    //                    {
    //                        timeReference = timeReference.AddHours(Interval);
    //                        nextTimeReference = nextTimeReference.AddHours(Interval);
    //                        if (timeReference.Hour > 17)
    //                        {
    //                            timeReference = nextTimeReference = nextTimeReference.AddDays(1).Date;
    //                            timeReference = nextTimeReference = nextTimeReference.AddHours(10);
    //                            nextTimeReference = nextTimeReference.AddHours(Interval);
    //                        }
    //                    }
    //                    //Days:

    //                    else
    //                    {
    //                        timeReference = timeReference.AddDays(Interval);
    //                        nextTimeReference = nextTimeReference.AddDays(Interval);
    //                    }
    //                    continue;
    //                }
    //            }
    //            string symbol = BasicBars[i].Symbol;
    //            DateTime date = timeReference;

    //            double high = BasicBars[i].HighPrice;
    //            double low = BasicBars[i].LowPrice;
    //            double open = BasicBars[i].OpenPrice;
    //            double close = BasicBars[i].ClosePrice;
    //            double volume = BasicBars[i].VolumeF;


    //            int intervalTemp = Interval;

    //            i++;

    //            if (i >= BasicBars.Count - 1)
    //            {
    //                NewBars.Add(new BarData()
    //                {
    //                    Symbol = symbol,
    //                    TradeDate = date,
    //                    OpenPrice = open,
    //                    HighPrice = high,
    //                    LowPrice = low,
    //                    ClosePrice = close,
    //                    VolumeF = volume
    //                });
    //                break;
    //            }

    //            if (Period < Interface.Periodicity.Weekly)
    //            {
    //                while ((BasicBars[i].TradeDate < nextTimeReference) && (i < BasicBars.Count - 1))
    //                {
    //                    close = BasicBars[i].ClosePrice;
    //                    low = (BasicBars[i].LowPrice < low) ? BasicBars[i].LowPrice : low;
    //                    high = (BasicBars[i].HighPrice > high) ? BasicBars[i].HighPrice : high;
    //                    volume += BasicBars[i].VolumeF;
    //                    i++;
    //                }
    //            }
    //            else

    //            {
    //                while ((intervalTemp > 0) && (i < BasicBars.Count - 1))

    //                {
    //                    switch (Period)
    //                    {

    //                        case Interface.Periodicity.Weekly:
    //                            if (BasicBars[i - 1].TradeDate.DayOfWeek > BasicBars[i].TradeDate.DayOfWeek)
    //                                intervalTemp--;
    //                            break;
    //                        case Interface.Periodicity.Month:
    //                            if (BasicBars[i - 1].TradeDate.Month != BasicBars[i].TradeDate.Month)
    //                                intervalTemp--;
    //                            break;
    //                        case Interface.Periodicity.Year:
    //                            if (BasicBars[i - 1].TradeDate.Year != BasicBars[i].TradeDate.Year)
    //                                intervalTemp--;
    //                            break;
    //                    }

    //                    if (intervalTemp <= 0) continue;

    //                    low = (BasicBars[i].LowPrice < low) ? BasicBars[i].LowPrice : low;
    //                    high = (BasicBars[i].HighPrice > high) ? BasicBars[i].HighPrice : high;
    //                    close = BasicBars[i].ClosePrice;
    //                    volume += BasicBars[i].VolumeF;
    //                    i++;
    //                }
    //                timeReference = BasicBars[i].TradeDate;
    //            }
    //            NewBars.Add(new BarData()
    //            {
    //                Symbol = symbol,
    //                TradeDate = date,
    //                OpenPrice = open,
    //                HighPrice = high,
    //                LowPrice = low,
    //                ClosePrice = close,
    //                VolumeF = volume
    //            });

    //        }
    //        //LogDB("DATA PROCESSED = "+(timer.ElapsedMilliseconds - timeEllapsed));
    //        return NewBars;
    //    }

    //    public static bool SaveBarDatas(List<BarDataPS> Bars, Connection _connection)
    //    {
    //        try
    //        {
    //            bool inMemory = false;
    //            switch ((BaseType)Bars[0].Data.BaseType)
    //            {
    //                case BaseType.Days:
    //                    inMemory = StockBarsOnMemoryDay.Exists(s => s.Symbol == Bars[0].Data.Symbol);
    //                    break;
    //                case BaseType.Minutes:
    //                    inMemory = StockBarsOnMemoryMin.Exists(s => s.Symbol == Bars[0].Data.Symbol);
    //                    break;
    //                case BaseType.Minutes15:
    //                    inMemory = StockBarsOnMemoryMin15.Exists(s => s.Symbol == Bars[0].Data.Symbol);
    //                    break;
    //            }
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbolDate");
    //            Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbolDate");
    //            foreach (BarDataPS bar in Bars)
    //            {
    //                if (bar.BarIndex == -1) continue;
    //                //Normalize data:
    //                switch ((BaseType)bar.Data.BaseType)
    //                {
    //                    case BaseType.Days:
    //                        bar.Data.TradeDate = bar.Data.TradeDate.Date;
    //                        bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
    //                        break;
    //                    case BaseType.Minutes:
    //                        bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
    //                        bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
    //                        break;
    //                    case BaseType.Minutes15:
    //                        bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
    //                        bar.Data.TradeDate = bar.Data.TradeDate.AddMinutes(-(bar.Data.TradeDate.Minute % 15));
    //                        bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
    //                        break;
    //                }
    //                //Save In-Memory if exists:
    //                if (inMemory)
    //                {
    //                    if (cursor.Search(Operation.Equals,
    //                                      new object[] { bar.Data.BaseType, bar.Data.Symbol, bar.Data.TradeDateTicks }))
    //                    {
    //                        cursor.MoveNext();
    //                        /*while (cursor.MoveNext())*/
    //                        cursor.Remove();
    //                    }
    //                    _connection.Insert(bar.Data);

    //                    switch ((BaseType)bar.Data.BaseType)
    //                    {
    //                        case BaseType.Days:
    //                            if (bar.Data.TradeDate >
    //                                StockBarsOnMemoryDay.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
    //                                StockBarsOnMemoryDay[
    //                                    StockBarsOnMemoryDay.IndexOf(
    //                                        StockBarsOnMemoryDay.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
    //                            break;
    //                        case BaseType.Minutes:
    //                            if (bar.Data.TradeDate >
    //                                StockBarsOnMemoryMin.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
    //                                StockBarsOnMemoryMin[
    //                                    StockBarsOnMemoryMin.IndexOf(
    //                                        StockBarsOnMemoryMin.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
    //                            break;
    //                        case BaseType.Minutes15:
    //                            if (bar.Data.TradeDate >
    //                                StockBarsOnMemoryMin15.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
    //                                StockBarsOnMemoryMin15[
    //                                    StockBarsOnMemoryMin15.IndexOf(
    //                                        StockBarsOnMemoryMin15.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
    //                            break;
    //                    }
    //                }

    //                if (cursorDisk.Search(Operation.Equals,
    //                                      new object[] { bar.Data.BaseType, bar.Data.Symbol, bar.Data.TradeDateTicks }))
    //                {
    //                    /*cursorDisk.MoveNext();
    //                    cursorDisk.Remove();*/

    //                    continue;
    //                }
    //                //Save In-Disk:
    //                _connection.Insert(new BarDataDisk()
    //                                       {
    //                                           BaseType = bar.Data.BaseType,
    //                                           ClosePrice = bar.Data.ClosePrice,
    //                                           HighPrice = bar.Data.HighPrice,
    //                                           LowPrice = bar.Data.LowPrice,
    //                                           OpenPrice = bar.Data.OpenPrice,
    //                                           Symbol = bar.Data.Symbol,
    //                                           TradeDate = bar.Data.TradeDate,
    //                                           TradeDateTicks = bar.Data.TradeDateTicks,
    //                                           VolumeF = bar.Data.VolumeF
    //                                       });

    //                if (bar == Bars[Bars.IndexOf(Bars.Last()) - 1])
    //                {
    //                    UpdateBaseEvent(new object(), new BarDataEventArgs(bar.Data));
    //                }
    //                //LogDB("\tSaved {0} {1} in database: \n\tPrice{2} Date={3}", bar.Data.Symbol, Enum.GetName(typeof(BaseType), Bars[0].Data.BaseType), bar.Data.ClosePrice, bar.Data.TradeDate.ToString());

    //            }
    //            _connection.CommitTransaction();
    //            //LogDB("\tDATABASE {0} with {1} bars!", Enum.GetName(typeof(BaseType), Bars[0].Data.BaseType), Count((BaseType)Bars[0].Data.BaseType, _connection));
    //        }
    //        catch (ExtremeDB.DatabaseError ex)
    //        {
    //            // Ignore duplicate's error
    //            //if (ex.errorCode == 13) return true;
    //            LogDB("SaveBarData() " + ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }

    //    public static bool SaveSnapshot(SymbolSnapshot snapshot, BaseType Base, Connection _connection)
    //    {
    //        try
    //        {
    //            int index = -1;
    //            switch (Base)
    //            {
    //                case BaseType.Days:
    //                    index = LastSnapshotDay.IndexOf(LastSnapshotDay.Find(s => s.Symbol == snapshot.Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        LastSnapshotDay[index] = snapshot;
    //                    }
    //                    else LastSnapshotDay.Add(snapshot);
    //                    break;
    //                case BaseType.Minutes:
    //                    index = LastSnapshotMin.IndexOf(LastSnapshotMin.Find(s => s.Symbol == snapshot.Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        LastSnapshotMin[index] = snapshot;
    //                    }
    //                    else LastSnapshotMin.Add(snapshot);
    //                    break;
    //                case BaseType.Minutes15:
    //                    index = LastSnapshotMin15.IndexOf(LastSnapshotMin15.Find(s => s.Symbol == snapshot.Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        LastSnapshotMin15[index] = snapshot;
    //                    }
    //                    else LastSnapshotMin15.Add(snapshot);
    //                    break;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);

    //            return false;

    //        }
    //        return true;
    //    }


    //    public static SymbolSnapshot GetSnapshot(string Symbol, BaseType Base, Connection _connection)
    //    {
    //        SymbolSnapshot snapshot = null;
    //        try
    //        {
    //            int index = -1;
    //            switch (Base)
    //            {
    //                case BaseType.Days:
    //                    index = LastSnapshotDay.IndexOf(LastSnapshotDay.Find(s => s.Symbol == Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        snapshot = LastSnapshotDay[index];
    //                    }
    //                    else return null;
    //                    break;
    //                case BaseType.Minutes:
    //                    index = LastSnapshotMin.IndexOf(LastSnapshotMin.Find(s => s.Symbol == Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        snapshot = LastSnapshotMin[index];
    //                    }
    //                    else return null;
    //                    break;
    //                case BaseType.Minutes15:
    //                    index = LastSnapshotMin15.IndexOf(LastSnapshotMin15.Find(s => s.Symbol == Symbol));
    //                    if (index != null && index != -1)
    //                    {
    //                        snapshot = LastSnapshotMin15[index];
    //                    }
    //                    else return null;
    //                    break;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);

    //            return null;

    //        }
    //        return snapshot;
    //    }

    //    public static bool SaveTick(TickData tickData, Connection _connection)
    //    {
    //        //Save ticks by symbol:
    //        int index = -1;
    //        index = TicksOnMemory.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
    //        if (index == null || index == -1) TicksOnMemory.Add(new List<TickData>() { tickData });
    //        else TicksOnMemory[index].Add(tickData);
    //        BarData lastBar;
    //        bool raiseEvent = false;
    //        // Subscribe for base = DAY:
    //        if (LastSnapshotDay.Exists(bar => bar.Symbol == tickData.Symbol))
    //        {
    //            //There's data waiting on buffer?
    //            if (TicksOnBufferDay.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
    //            {
    //                foreach (TickData t in TicksOnBufferDay.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
    //                {
    //                    if (t.Id > LastSnapshotDay.Find(s => s.Symbol == t.Symbol).Id) ProcessTickDay(t, _connection);
    //                }
    //                TicksOnBufferDay.Remove(
    //                    TicksOnBufferDay.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
    //            }
    //            ProcessTickDay(tickData, _connection);
    //            raiseEvent = true;
    //        }
    //        else
    //        {
    //            int indexDay = -1;
    //            indexDay = TicksOnBufferDay.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
    //            if (indexDay == null || indexDay == -1) TicksOnBufferDay.Add(new List<TickData>() { tickData });
    //            else TicksOnBufferDay[indexDay].Add(tickData);
    //        }


    //        // Subscribe for base = MIN:);
    //        if (LastSnapshotMin.Exists(bar => bar.Symbol == tickData.Symbol))
    //        {
    //            //There's data waiting on buffer?
    //            if (TicksOnBufferMin.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
    //            {
    //                foreach (TickData t in TicksOnBufferMin.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
    //                {
    //                    if (t.Id > LastSnapshotMin.Find(s => s.Symbol == t.Symbol).Id) ProcessTickMin(t, _connection);
    //                }
    //                TicksOnBufferMin.Remove(
    //                    TicksOnBufferMin.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
    //            }
    //            ProcessTickMin(tickData, _connection);
    //            raiseEvent = true;
    //        }
    //        else
    //        {
    //            int indexMin = -1;
    //            indexMin = TicksOnBufferMin.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
    //            if (indexMin == null || indexMin == -1) TicksOnBufferMin.Add(new List<TickData>() { tickData });
    //            else TicksOnBufferMin[indexMin].Add(tickData);
    //        }

    //        // Subscribe for base = MIN15:
    //        if (LastSnapshotMin15.Exists(bar => bar.Symbol == tickData.Symbol))
    //        {
    //            //There's data waiting on buffer?
    //            if (TicksOnBufferMin15.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
    //            {
    //                foreach (TickData t in TicksOnBufferMin15.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
    //                {
    //                    if (t.Id > LastSnapshotMin15.Find(s => s.Symbol == t.Symbol).Id) ProcessTickMin15(t, _connection);
    //                }
    //                TicksOnBufferMin15.Remove(
    //                    TicksOnBufferMin15.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
    //            }
    //            ProcessTickMin15(tickData, _connection);
    //            raiseEvent = true;
    //        }
    //        else
    //        {
    //            int indexMin15 = -1;
    //            indexMin15 = TicksOnBufferMin15.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
    //            if (indexMin15 == null || indexMin15 == -1) TicksOnBufferMin15.Add(new List<TickData>() { tickData });
    //            else TicksOnBufferMin15[indexMin15].Add(tickData);
    //        }
    //        if (raiseEvent) return true;
    //        return false;
    //    }
        





























    //    public static bool SaveTickTest(TickData tickData, Connection _connection)
    //    {
    //        //Save ticks by symbol:
    //        int index = -1;
    //        index = TicksOnMemory.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
    //        if (index == null || index == -1) TicksOnMemory.Add(new List<TickData>() { tickData });
    //        else TicksOnMemory[index].Add(tickData);
    //        BarData lastBar;
    //        bool raiseEvent = true;
    //        ProcessTickMin(tickData, _connection);
    //        ProcessTickMin15(tickData, _connection);
    //        ProcessTickDay(tickData, _connection);
    //        if (raiseEvent) return true;
    //        return false;
    //    }

    //    public static void ProcessTickDay(TickData tickData, Connection _connection)
    //    {
    //        BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Days, _connection);
    //        if (!NewCandlesDay.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesDay.Add(lastBar);
    //        else lastBar = NewCandlesDay.Find(s => s.Symbol == tickData.Symbol);
    //        //Create new bar data?
    //        if (lastBar.TradeDate.Date != tickData.TradeDate.Date)
    //        {
    //            //Save on disk last candle created:
    //            if (NewCandlesDay.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
    //            {
    //                SaveBarDatas(new List<BarDataPS>() { new BarDataPS(NewCandlesDay.Find(s => s.Symbol == tickData.Symbol)) },
    //                            _connection);
    //                lastBar = new BarData
    //                {
    //                    BaseType = lastBar.BaseType,
    //                    ClosePrice = tickData.Price,
    //                    HighPrice = tickData.Price,
    //                    LowPrice = tickData.Price,
    //                    OpenPrice = tickData.Price,
    //                    Symbol = tickData.Symbol,
    //                    TradeDate = tickData.TradeDate.Date,
    //                    TradeDateTicks = tickData.TradeDate.Date.Ticks,
    //                    VolumeF = tickData.Quantity
    //                };
    //            }
    //            else

    //            {
    //                lastBar = new BarData
    //                {
    //                    BaseType = lastBar.BaseType,
    //                    ClosePrice = tickData.Price,
    //                    HighPrice = tickData.Price,
    //                    LowPrice = tickData.Price,
    //                    OpenPrice = tickData.Price,
    //                    Symbol = tickData.Symbol,
    //                    TradeDate = tickData.TradeDate.Date,
    //                    TradeDateTicks = tickData.TradeDate.Date.Ticks,
    //                    VolumeF = tickData.Quantity
    //                };
    //                SaveBarDatas(new List<BarDataPS>() { new BarDataPS(lastBar) },
    //                            _connection);

    //            }



    //        }
    //        //Merge tick with last BarData?
    //        else
    //        {
    //            lastBar = new BarData
    //            {
    //                BaseType = lastBar.BaseType,
    //                ClosePrice = tickData.Price,
    //                HighPrice =
    //                    tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
    //                LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
    //                OpenPrice = lastBar.OpenPrice,
    //                Symbol = tickData.Symbol,
    //                TradeDate = lastBar.TradeDate,
    //                TradeDateTicks = lastBar.TradeDateTicks,
    //                VolumeF = lastBar.VolumeF + tickData.Quantity
    //            };

    //        }
    //        NewCandlesDay.Remove(NewCandlesDay.Find(s => s.Symbol == tickData.Symbol));
    //        NewCandlesDay.Add(lastBar);
    //        SymbolSnapshot snapshot = LastSnapshotDay.Find(s => s.Symbol == lastBar.Symbol);
    //        snapshot.Id = tickData.Id;
    //        snapshot.Close = tickData.Price;
    //        snapshot.Open = lastBar.OpenPrice;
    //        snapshot.High = lastBar.HighPrice;
    //        snapshot.Low = lastBar.LowPrice;
    //        snapshot.Quantity = tickData.Quantity;
    //        snapshot.Seller = tickData.Seller;
    //        snapshot.Timestamp = tickData.TradeDate;
    //        snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
    //        snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
    //        snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
    //        LastSnapshotDay.Remove(LastSnapshotDay.Find(s => s.Symbol == lastBar.Symbol));
    //        LastSnapshotDay.Add(snapshot);
    //        SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Days));
    //    }


    //    public static void ProcessTickMin(TickData tickData, Connection _connection)
    //    {
    //        BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Minutes, _connection);
    //        if (!NewCandlesMin.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesMin.Add(lastBar);
    //        else lastBar = NewCandlesMin.Find(s => s.Symbol == tickData.Symbol);
    //        //Merge tick with last BarData?
    //        if (lastBar.TradeDate.Date == tickData.TradeDate.Date && lastBar.TradeDate.Minute == tickData.TradeDate.Minute)
    //        {
    //            lastBar = new BarData
    //            {
    //                BaseType = lastBar.BaseType,
    //                ClosePrice = tickData.Price,
    //                HighPrice =
    //                    tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
    //                LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
    //                OpenPrice = lastBar.OpenPrice,
    //                Symbol = tickData.Symbol,
    //                TradeDate = lastBar.TradeDate,
    //                TradeDateTicks = lastBar.TradeDateTicks,
    //                VolumeF = lastBar.VolumeF + tickData.Quantity
    //            };
    //        }
    //        //Create new bar data?
    //        else
    //        {
    //            //Save on disk last candle created:
    //            if (NewCandlesMin.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
    //            {
    //                SaveBarDatas(
    //                    new List<BarDataPS>() { new BarDataPS(NewCandlesMin.Find(s => s.Symbol == tickData.Symbol)) },
    //                    _connection);
    //            }
    //            lastBar = new BarData
    //            {
    //                BaseType = lastBar.BaseType,
    //                ClosePrice = tickData.Price,
    //                HighPrice = tickData.Price,
    //                LowPrice = tickData.Price,
    //                OpenPrice = tickData.Price,
    //                Symbol = tickData.Symbol,
    //                TradeDate = tickData.TradeDate,
    //                TradeDateTicks = tickData.TradeDate.Date.Ticks,
    //                VolumeF = tickData.Quantity
    //            };
    //            NewBarEvent(new object(), new BarEventArgs(lastBar, (int)BaseType.Minutes, false));

    //        }
    //        NewCandlesMin.Remove(NewCandlesMin.Find(s => s.Symbol == tickData.Symbol));
    //        NewCandlesMin.Add(lastBar);
    //        SymbolSnapshot snapshot = LastSnapshotMin.Find(s => s.Symbol == lastBar.Symbol);
    //        snapshot.Id = tickData.Id;
    //        snapshot.Close = tickData.Price;
    //        snapshot.Open = lastBar.OpenPrice;
    //        snapshot.High = lastBar.HighPrice;
    //        snapshot.Low = lastBar.LowPrice;
    //        snapshot.Quantity = tickData.Quantity;
    //        snapshot.Seller = tickData.Seller;
    //        snapshot.Timestamp = tickData.TradeDate;
    //        snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
    //        snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
    //        snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
    //        LastSnapshotMin.Remove(LastSnapshotMin.Find(s => s.Symbol == lastBar.Symbol));
    //        LastSnapshotMin.Add(snapshot);
    //        SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Minutes));
    //    }

    //    public static void ProcessTickMin15(TickData tickData, Connection _connection)
    //    {
    //        BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Minutes15, _connection);
    //        if (!NewCandlesMin15.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesMin15.Add(lastBar);
    //        else lastBar = NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol);
    //        //Merge tick with last BarData?
    //        if (lastBar.TradeDate.Date == tickData.TradeDate.Date && tickData.TradeDate.Minute < lastBar.TradeDate.Minute + 15 && tickData.TradeDate.Hour == lastBar.TradeDate.Hour)
    //        {
    //            lastBar = new BarData
    //            {
    //                BaseType = lastBar.BaseType,
    //                ClosePrice = tickData.Price,
    //                HighPrice =
    //                    tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
    //                LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
    //                OpenPrice = lastBar.OpenPrice,
    //                Symbol = tickData.Symbol,
    //                TradeDate = lastBar.TradeDate,
    //                TradeDateTicks = lastBar.TradeDateTicks,
    //                VolumeF = lastBar.VolumeF + tickData.Quantity
    //            };
    //        }
    //        //Create new bar data?
    //        else
    //        {
    //            //Save on disk last candle created:
    //            if (NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
    //            {
    //                SaveBarDatas(
    //                    new List<BarDataPS>() { new BarDataPS(NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol)) },
    //                    _connection);
    //                lastBar = new BarData
    //                {
    //                    BaseType = lastBar.BaseType,
    //                    ClosePrice = tickData.Price,
    //                    HighPrice = tickData.Price,
    //                    LowPrice = tickData.Price,
    //                    OpenPrice = tickData.Price,
    //                    Symbol = tickData.Symbol,
    //                    TradeDate = tickData.TradeDate,
    //                    TradeDateTicks = tickData.TradeDate.Date.Ticks,
    //                    VolumeF = tickData.Quantity
    //                };
    //            }
    //            else

    //            {
    //                lastBar = new BarData
    //                {
    //                    BaseType = lastBar.BaseType,
    //                    ClosePrice = tickData.Price,
    //                    HighPrice = tickData.Price,
    //                    LowPrice = tickData.Price,
    //                    OpenPrice = tickData.Price,
    //                    Symbol = tickData.Symbol,
    //                    TradeDate = tickData.TradeDate,
    //                    TradeDateTicks = tickData.TradeDate.Date.Ticks,
    //                    VolumeF = tickData.Quantity
    //                };
    //                SaveBarDatas(
    //                    new List<BarDataPS>() { new BarDataPS(lastBar) },
    //                    _connection);
    //            }


    //            NewBarEvent(new object(), new BarEventArgs(lastBar, (int)BaseType.Minutes15, false));

    //        }
    //        NewCandlesMin15.Remove(NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol));
    //        NewCandlesMin15.Add(lastBar);
    //        SymbolSnapshot snapshot = LastSnapshotMin15.Find(s => s.Symbol == lastBar.Symbol);
    //        snapshot.Id = tickData.Id;
    //        snapshot.Close = tickData.Price;
    //        snapshot.Open = lastBar.OpenPrice;
    //        snapshot.High = lastBar.HighPrice;
    //        snapshot.Low = lastBar.LowPrice;
    //        snapshot.Quantity = tickData.Quantity;
    //        snapshot.Seller = tickData.Seller;
    //        snapshot.Timestamp = tickData.TradeDate;
    //        snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
    //        snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
    //        snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
    //        LastSnapshotMin15.Remove(LastSnapshotMin15.Find(s => s.Symbol == lastBar.Symbol));
    //        LastSnapshotMin15.Add(snapshot);
    //        SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Minutes15));
    //    }

    //    public static bool IsInMemory(string Symbol, BaseType Base, Connection _connection)
    //    {
    //        switch (Base)
    //        {
    //            case BaseType.Days:
    //                lock (StockBarsOnMemoryDay)
    //                {
    //                    return StockBarsOnMemoryDay.Count(stock => stock.Symbol == Symbol) > 0;
    //                }
    //                break;
    //            case BaseType.Minutes:
    //                lock (StockBarsOnMemoryMin)
    //                {
    //                    return StockBarsOnMemoryMin.Count(stock => stock.Symbol == Symbol) > 0;
    //                }
    //                break;
    //            case BaseType.Minutes15:
    //                lock (StockBarsOnMemoryMin15)
    //                {
    //                    return StockBarsOnMemoryMin15.Count(stock => stock.Symbol == Symbol) > 0;
    //                }
    //                break;
    //        }
    //        return false;
    //    }

    //    public static bool TickIsProcessed(string Symbol, BaseType Base)
    //    {
    //        switch (Base)
    //        {
    //            case BaseType.Days:
    //                return LastSnapshotDay.Exists(b => b.Symbol == Symbol);
    //            case BaseType.Minutes:
    //                return LastSnapshotMin.Exists(b => b.Symbol == Symbol);
    //            case BaseType.Minutes15:
    //                return LastSnapshotMin15.Exists(b => b.Symbol == Symbol);
    //        }
    //        return false;
    //    }

    //    public static bool AdjustBarDatas(BarData LastBar, Connection _connection)
    //    {
    //        double adjustment;
    //        DateTime PreviousDate = new DateTime();
    //        bool first = true;
    //        try
    //        {
    //            //Update data from extremeDB on disk:
    //            _connection.StartTransaction(Database.TransactionType.ReadWrite);
    //            // Request all data from base:
    //            Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbolDate");
    //            Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbol");
    //            adjustment = 0;
    //            BarDataDisk BarCursorDisk;
    //            BarDataDisk LastBarDisk = new BarDataDisk()
    //                                          {
    //                                              BaseType = LastBar.BaseType,
    //                                              ClosePrice = LastBar.ClosePrice,
    //                                              HighPrice = LastBar.HighPrice,
    //                                              LowPrice = LastBar.LowPrice,
    //                                              OpenPrice = LastBar.OpenPrice,
    //                                              Symbol = LastBar.Symbol,
    //                                              TradeDate = LastBar.TradeDate,
    //                                              TradeDateTicks = LastBar.TradeDateTicks,
    //                                              VolumeF = LastBar.VolumeF
    //                                          };
    //            PreviousDate = new DateTime();
    //            first = true;
    //            if (cursorDisk.Search(Operation.Equals, new object[] { LastBarDisk.BaseType, LastBarDisk.Symbol, LastBarDisk.TradeDate.Ticks }))
    //            {
    //                while (cursorDisk.MovePrev())
    //                {
    //                    if (first)
    //                    {
    //                        BarCursorDisk = cursorDisk.Current;
    //                        adjustment = (LastBarDisk.ClosePrice - BarCursorDisk.ClosePrice) / BarCursorDisk.ClosePrice;
    //                        BarCursorDisk.ClosePrice += adjustment * BarCursorDisk.ClosePrice;
    //                        BarCursorDisk.OpenPrice += adjustment * BarCursorDisk.OpenPrice;
    //                        BarCursorDisk.LowPrice += adjustment * BarCursorDisk.LowPrice;
    //                        BarCursorDisk.HighPrice += adjustment * BarCursorDisk.HighPrice;
    //                        PreviousDate = BarCursorDisk.TradeDate;
    //                        cursorDisk.Update();
    //                        first = false;
    //                        continue;
    //                    }
    //                    if (cursorDisk.Current.Symbol == LastBarDisk.Symbol && cursorDisk.Current.BaseType == LastBarDisk.BaseType)
    //                    {
    //                        BarCursorDisk = cursorDisk.Current;
    //                        if (BarCursorDisk.TradeDate <= PreviousDate)
    //                        {
    //                            BarCursorDisk.ClosePrice += adjustment * BarCursorDisk.ClosePrice;
    //                            BarCursorDisk.OpenPrice += adjustment * BarCursorDisk.OpenPrice;
    //                            BarCursorDisk.LowPrice += adjustment * BarCursorDisk.LowPrice;
    //                            BarCursorDisk.HighPrice += adjustment * BarCursorDisk.HighPrice;
    //                            PreviousDate = BarCursorDisk.TradeDate;
    //                            cursorDisk.Update();
    //                        }
    //                        else
    //                        {
    //                            _connection.RollbackTransaction();
    //                            return false;
    //                        }
    //                    }
    //                    else break;
    //                }
    //            }

    //            // Remove data from database in memory and reload:
    //            if (cursor.Search(Operation.Equals,
    //                                       new object[] { LastBar.BaseType, LastBar.Symbol }))
    //            {
    //                while (cursor.MoveNext()) cursor.Remove();
    //            }
    //            _connection.CommitTransaction();

    //            switch ((BaseType)LastBar.BaseType)
    //            {
    //                case BaseType.Days:
    //                    lock (StockBarsOnMemoryDay)
    //                    {
    //                        if (StockBarsOnMemoryDay.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
    //                        {
    //                            StockBarsOnMemoryDay.Remove(StockBarsOnMemoryDay.First(stock => stock.Symbol == LastBar.Symbol));
    //                            GetBarDataAll(LastBar.Symbol, 1, Periodicity.Daily.GetHashCode(), _connection);
    //                        }
    //                    }
    //                    break;
    //                case BaseType.Minutes:
    //                    if (StockBarsOnMemoryMin.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
    //                    {

    //                        StockBarsOnMemoryMin.Remove(StockBarsOnMemoryMin.First(stock => stock.Symbol == LastBar.Symbol));
    //                        GetBarDataAll(LastBar.Symbol, 1, Periodicity.Minutely.GetHashCode(), _connection);
    //                    }

    //                    break;
    //                case BaseType.Minutes15:
    //                    if (StockBarsOnMemoryMin15.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
    //                    {
    //                        StockBarsOnMemoryMin15.Remove(StockBarsOnMemoryMin15.First(stock => stock.Symbol == LastBar.Symbol));
    //                        GetBarDataAll(LastBar.Symbol, 15, Periodicity.Minutely.GetHashCode(), _connection);
    //                    }
    //                    break;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB("AdjustBarData() " + ex.Message);
    //            return false;
    //        }


    //        return true;
    //    }

    //    public static bool Clear(Connection _connection)
    //    {
    //        try
    //        {
    //            if (Disconnect(_connection))
    //            {
    //                if (File.Exists(_path + _name + ".dbs"))
    //                {
    //                    File.Delete(_path + _name + ".dbs");
    //                }
    //                if (File.Exists(_path + _name + ".log"))
    //                {
    //                    File.Delete(_path + _name + ".log");
    //                }
    //                if (Connect() == null) return false;
    //            }
    //            else return false;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);

    //            return false;
    //        }
    //        return true;
    //    }

    //    public static int Count(BaseType Base, Connection _connection)
    //    {
    //        int count = 0;
    //        try
    //        {
    //            if (_connection != null)
    //            {
    //                _connection.StartTransaction(Database.TransactionType.ReadOnly);
    //                count = _connection.GetTable<BarDataDisk>().Count(s => s.BaseType == (int)Base);
    //                _connection.RollbackTransaction();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogDB(ex.Message);

    //        }
    //        return count;
    //    }


    //}

    public class DBlocalSQL
    {
        //TODO: change path reference!
        private static string _stringConnection = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=\"" + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena\\PlenaData.mdf\";Integrated Security=True";
        private static List<SqlConnection> _connections = new List<SqlConnection>();
        private static bool _stockListOk = false;
        private static bool _portfoliosOk = false;


        #region Synchronization events:

        // SYMBOLS LIST UPDATED: 
        public delegate void SymbolsEventHandler(object sender, EventArgs args);
        public static event SymbolsEventHandler UpdateSymbolsEvent = delegate { };

        // DATABASE UPDATED: 
        public delegate void BaseEventHandler(object sender, BarDataEventArgs args);
        public static event BaseEventHandler UpdateBaseEvent = delegate { };

        // NEW CANDLE CREATED
        public delegate void BarEventHandler(object sender, BarEventArgs args);
        public static event BarEventHandler NewBarEvent = delegate { };

        // SNAPSHOT UPDATED
        public delegate void SnapshotEventHandler(object sender, SnapshotEventArgs args);
        public static event SnapshotEventHandler SnapshotEvent = delegate { };


        public delegate void LogHandler(string format, params object[] args);
        public static event LogHandler LogDB = delegate { };

        #endregion
        
        #region Snapshots
        //Bars on memory database:
        private static List<BarData> StockBarsOnMemoryDay = new List<BarData>();
        private static List<BarData> StockBarsOnMemoryMin = new List<BarData>();
        private static List<BarData> StockBarsOnMemoryMin15 = new List<BarData>();
        //Represents last time bars updated with server:
        private static List<SymbolSnapshot> LastSnapshotDay = new List<SymbolSnapshot>();
        private static List<SymbolSnapshot> LastSnapshotMin = new List<SymbolSnapshot>();
        private static List<SymbolSnapshot> LastSnapshotMin15 = new List<SymbolSnapshot>();
        //Represents all ticks (not stored on disk):
        private static List<List<TickData>> TicksOnMemory = new List<List<TickData>>();
        //Represents ticks not processed on base:
        private static List<List<TickData>> TicksOnBufferDay = new List<List<TickData>>();
        private static List<List<TickData>> TicksOnBufferMin = new List<List<TickData>>();
        private static List<List<TickData>> TicksOnBufferMin15 = new List<List<TickData>>();
        //Represents new candles built by ticks:
        private static List<BarData> NewCandlesDay = new List<BarData>();
        private static List<BarData> NewCandlesMin = new List<BarData>();
        private static List<BarData> NewCandlesMin15 = new List<BarData>();
        #endregion

        /// <summary>
        /// Connect to database.
        /// </summary>
        /// <returns>Returns true if succeeded</returns>
        public static SqlConnection Connect()
        {
            try
            {
                _connections.Add(new SqlConnection(_stringConnection));
                return _connections.Last();
            }
            catch (Exception ex)
            {
                LogDB("Connect() "+ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Disconnect from database.
        /// </summary>
        /// <returns>Returns true if succeded.</returns>
        public static bool Disconnect(SqlConnection _connection)
        {
            try
            {
                _connections.Remove(_connection);
            }
            catch (Exception ex)
            {
                LogDB("Disconnect() " + ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Save symbols to database.
        /// </summary>
        /// <param name="StockList"></param>
        /// <returns>Returns true if succeeded</returns>
        public static bool SaveSymbols(List<SymbolsPS> StockList, SqlConnection _connection)
        {
            try
            {
                _connection.Open();
                //Clean old symbols:
                SqlCommand cmd = new SqlCommand("DELETE FROM Symbols", _connection);
                cmd.ExecuteNonQuery();
                //Save symbols:
                foreach (SymbolsPS symbol in StockList)
                {
                    cmd = new SqlCommand("INSERT INTO Symbols (Code) VALUES ('" + symbol.StockInfo.Code + "')", _connection);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();
                _stockListOk = true;
                LogDB("\tSaved {0} symbols database...", StockList.Count);
                UpdateSymbolsEvent(new object(), new EventArgs());
            }
            catch (Exception ex)
            {
                LogDB("SaveSymbol() " + ex.Message);
                _connection.Close();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Save\subscribe user's portfolios to database.
        /// </summary>
        /// <param name="Portfolios"></param>
        /// <returns></returns>
        public static bool SaveGroups(List<SymbolGroup> Groups, SqlConnection _connection)
        {
            try
            {
                _connection.Open();
                //Clean old portfolios:
                SqlCommand cmd = new SqlCommand("DELETE FROM Portfolios", _connection);
                cmd.ExecuteNonQuery();
                //Save portfolios:
                foreach (SymbolGroup group in Groups)
                {
                    string symbolsList = string.Join(",", group.Symbols); 
                    cmd = new SqlCommand("INSERT INTO Portfolios (pName, pIndex, pType, pSymbols) VALUES ('" + group.Name + "',"+group.Index+","+group.Type+",'"+symbolsList+"')", _connection);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();

                // save database image to file
                LogDB("\tSaved portfolio database...");
            }
            catch (Exception ex)
            {
                LogDB(ex.Message);
                _connection.Close();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get Symbol's List from database.
        /// </summary>
        /// <returns></returns>
        public static List<SymbolsPS> LoadSymbols(SqlConnection _connection)
        {
            List<SymbolsPS> StockList = new List<SymbolsPS>();
            try
            {
                // open read-only transaction
                _connection.Open();
                // Open cursor by "id" index
                int StockID = 0;
                SqlCommand cmd = new SqlCommand("SELECT Code, Name, Sector, SubSector, Segment, Source, Type, Activity, Site, Status, Priority FROM Symbols", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string Code = rdr.GetString(0);
                    string Name = rdr.IsDBNull(1) ? null : rdr.GetString(1);  
                    string Sector = rdr.IsDBNull(2) ? null : rdr.GetString(2);
                    string SubSector = rdr.IsDBNull(3) ? null : rdr.GetString(3);
                    string Segment = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                    string Source = rdr.IsDBNull(5) ? null : rdr.GetString(5);
                    string Type = rdr.IsDBNull(6) ? null : rdr.GetString(6);
                    string Activity = rdr.IsDBNull(7) ? null : rdr.GetString(7);
                    string Site = rdr.IsDBNull(8) ? null : rdr.GetString(8);
                    int Status = rdr.IsDBNull(9) ? -1 : rdr.GetInt32(9);
                    int Priority = rdr.IsDBNull(10) ? -1 : rdr.GetInt32(10);
                    StockList.Add(new SymbolsPS()
                    {
                        StockId = StockID,
                        StockInfo = new Symbol()
                        {
                            Code = rdr.GetString(0),
                            Name = rdr.IsDBNull(1) ? null : rdr.GetString(1),
                            Sector = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                            SubSector = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                            Segment = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                            Source = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                            Type = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                            Activity = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                            Site = rdr.IsDBNull(8) ? null : rdr.GetString(8),
                            Status = rdr.IsDBNull(9) ? -1 : rdr.GetInt32(9),
                            Priority = rdr.IsDBNull(10) ? -1 : rdr.GetInt32(10)
                        }
                    });
                    StockID++;
                }
                rdr.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB("LoadSymbol() " + ex.Message);
                _connection.Close();
            }
            if (StockList.Count > 0) _stockListOk = true;
            return StockList;
        }
        /// <summary>
        /// Get a group of symbols (index or portfolio) from database.
        /// </summary>
        /// <returns></returns>
        public static SymbolGroup LoadGroup(string Name, GroupType Type, SqlConnection _connection)
        {
            SymbolGroup group = new SymbolGroup();
            try
            {
                // open read-only transaction
                _connection.Open();
                // Open cursor by "id" index       
                SqlCommand cmd = new SqlCommand("SELECT pName, pIndex, pType, pSymbols FROM Portfolios WHERE Name = "+Name, _connection);       
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string name = rdr.GetString(0);
                    int index = rdr.IsDBNull(1) ? -1 : rdr.GetInt32(1);
                    int type = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    string[] symbols = rdr.IsDBNull(3) ? null : rdr.GetString(3).Split(new char[] { ',' });
                    group = new SymbolGroup()
                    {
                        Name = name,
                        Index = index,
                        Type = type,
                        Symbols = symbols
                    };
                }
                rdr.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB(ex.Message);
                _connection.Close();
            }
            return group;
        }
        /// <summary>
        /// Get a group's List (indexes or portfolios) from database.
        /// </summary>
        /// <returns></returns>
        public static List<SymbolGroup> LoadGroups(GroupType Type, SqlConnection _connection)
        {
            List<SymbolGroup> groups = new List<SymbolGroup>();
            try
            {
                // open read-only transaction
                _connection.Open();
                // Open cursor by "id" index       
                SqlCommand cmd = new SqlCommand("SELECT pName, pIndex, pType, pSymbols FROM Portfolios WHERE pType = "+((int)Type), _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string name = rdr.GetString(0);
                    int index = rdr.IsDBNull(1) ? -1 : rdr.GetInt32(1);
                    int type = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    string[] symbols = rdr.IsDBNull(3) ? null : rdr.GetString(3).Split(new char[] { ',' });
                    groups.Add(new SymbolGroup()
                            {
                                Name = name,
                                Index = index,
                                Type = type,
                                Symbols = symbols
                            });
                }
                rdr.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB(ex.Message);
                _connection.Close();
            }
            if (groups.Count > 0) _portfoliosOk = true;
            return groups;
        }
        public static bool IsStockListOk()
        {
            return _stockListOk;
        }
        public static bool IsPortfolioOk()
        {
            return _portfoliosOk;
        }
        public static bool RemoveGroup(string Name, SqlConnection _connection)
        {
            bool removed = false;
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Portfolios WHERE pName = "+Name, _connection);
                cmd.ExecuteNonQuery();
                removed = true;
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB(ex.Message);
                _connection.Close();
            }
            return removed;
        }


        public static bool SaveBarDatas(List<BarDataPS> Bars, SqlConnection _connection)
        {
            try
            {
                bool inMemory = false;
                string table = "BaseDay";
                switch ((BaseType)Bars[0].Data.BaseType)
                {
                    case BaseType.Days:
                        inMemory = StockBarsOnMemoryDay.Exists(s => s.Symbol == Bars[0].Data.Symbol);
                        break;
                    case BaseType.Minutes:
                        inMemory = StockBarsOnMemoryMin.Exists(s => s.Symbol == Bars[0].Data.Symbol);
                        table = "BaseMin";
                        break;
                    case BaseType.Minutes15:
                        inMemory = StockBarsOnMemoryMin15.Exists(s => s.Symbol == Bars[0].Data.Symbol);
                        table = "BaseMin15";
                        break;
                }
                _connection.Open();
                SqlCommand cmd;
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                foreach (BarDataPS bar in Bars)
                {
                    if (bar.BarIndex == -1) continue;
                    //Normalize data:
                    switch ((BaseType)bar.Data.BaseType)
                    {
                        case BaseType.Days:
                            bar.Data.TradeDate = bar.Data.TradeDate.Date;
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes:
                            bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes15:
                            bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
                            bar.Data.TradeDate = bar.Data.TradeDate.AddMinutes(-(bar.Data.TradeDate.Minute % 15));
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                    }
                    //Data already exists?
                    cmd = new SqlCommand("SELECT DateTicks FROM " + table + " WHERE Symbol = '" + bar.Data.Symbol + "' AND DateTicks = "+bar.Data.TradeDateTicks, _connection);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    bool ignore = false;
                    while (rdr.Read())
                    {
                        ignore = true;
                    }
                    rdr.Close();
                    if (ignore) continue;
                    //Save data:
                    cmd = new SqlCommand("INSERT INTO " + table + " (DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD) VALUES (" + bar.Data.TradeDateTicks + ",'" + bar.Data.TradeDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + bar.Data.Symbol + "'," + bar.Data.OpenPrice.ToString(nfi) + "," + bar.Data.HighPrice.ToString(nfi) + "," + bar.Data.LowPrice.ToString(nfi) + "," + bar.Data.ClosePrice.ToString(nfi) + "," + bar.Data.VolumeF.ToString(nfi) + "," + bar.Data.VolumeS + "," + bar.Data.VolumeT + "," + bar.Data.AdjustS.ToString(nfi) + "," + bar.Data.AdjustD.ToString(nfi) + ")", _connection);
                    //cmd = new SqlCommand("INSERT INTO " + table + " (DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD) VALUES (0,'2013-10-10 21:18:00','Teste',2.00,4.00,1.00,3.00,1000.00,500,800,4.5,4.6)", _connection);
                    cmd.ExecuteNonQuery(); 
                    if (bar == Bars[Bars.IndexOf(Bars.Last()) - 1])
                    {
                        UpdateBaseEvent(new object(), new BarDataEventArgs(bar.Data));
                    }

                }
                _connection.Close();




                /*Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbolDate");
                Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbolDate");
                foreach (BarDataPS bar in Bars)
                {
                    if (bar.BarIndex == -1) continue;
                    //Normalize data:
                    switch ((BaseType)bar.Data.BaseType)
                    {
                        case BaseType.Days:
                            bar.Data.TradeDate = bar.Data.TradeDate.Date;
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes:
                            bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes15:
                            bar.Data.TradeDate = bar.Data.TradeDate.AddSeconds(-(bar.Data.TradeDate.Second));
                            bar.Data.TradeDate = bar.Data.TradeDate.AddMinutes(-(bar.Data.TradeDate.Minute % 15));
                            bar.Data.TradeDateTicks = bar.Data.TradeDate.Ticks;
                            break;
                    }
                    //Save In-Memory if exists:
                    if (inMemory)
                    {
                        if (cursor.Search(Operation.Equals,
                                          new object[] { bar.Data.BaseType, bar.Data.Symbol, bar.Data.TradeDateTicks }))
                        {
                            cursor.MoveNext();
                            cursor.Remove();
                        }
                        _connection.Insert(bar.Data);

                        switch ((BaseType)bar.Data.BaseType)
                        {
                            case BaseType.Days:
                                if (bar.Data.TradeDate >
                                    StockBarsOnMemoryDay.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
                                    StockBarsOnMemoryDay[
                                        StockBarsOnMemoryDay.IndexOf(
                                            StockBarsOnMemoryDay.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
                                break;
                            case BaseType.Minutes:
                                if (bar.Data.TradeDate >
                                    StockBarsOnMemoryMin.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
                                    StockBarsOnMemoryMin[
                                        StockBarsOnMemoryMin.IndexOf(
                                            StockBarsOnMemoryMin.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
                                break;
                            case BaseType.Minutes15:
                                if (bar.Data.TradeDate >
                                    StockBarsOnMemoryMin15.Find(stock => stock.Symbol == bar.Data.Symbol).TradeDate)
                                    StockBarsOnMemoryMin15[
                                        StockBarsOnMemoryMin15.IndexOf(
                                            StockBarsOnMemoryMin15.Find(stock => stock.Symbol == bar.Data.Symbol))] = bar.Data;
                                break;
                        }
                    }

                    if (cursorDisk.Search(Operation.Equals,
                                          new object[] { bar.Data.BaseType, bar.Data.Symbol, bar.Data.TradeDateTicks }))
                    {

                        continue;
                    }
                    //Save In-Disk:
                    _connection.Insert(new BarDataDisk()
                    {
                        BaseType = bar.Data.BaseType,
                        ClosePrice = bar.Data.ClosePrice,
                        HighPrice = bar.Data.HighPrice,
                        LowPrice = bar.Data.LowPrice,
                        OpenPrice = bar.Data.OpenPrice,
                        Symbol = bar.Data.Symbol,
                        TradeDate = bar.Data.TradeDate,
                        TradeDateTicks = bar.Data.TradeDateTicks,
                        VolumeF = bar.Data.VolumeF
                    });

                    if (bar == Bars[Bars.IndexOf(Bars.Last()) - 1])
                    {
                        UpdateBaseEvent(new object(), new BarDataEventArgs(bar.Data));
                    }
                    //LogDB("\tSaved {0} {1} in database: \n\tPrice{2} Date={3}", bar.Data.Symbol, Enum.GetName(typeof(BaseType), Bars[0].Data.BaseType), bar.Data.ClosePrice, bar.Data.TradeDate.ToString());

                }
                _connection.CommitTransaction();
                //LogDB("\tDATABASE {0} with {1} bars!", Enum.GetName(typeof(BaseType), Bars[0].Data.BaseType), Count((BaseType)Bars[0].Data.BaseType, _connection));
            */
            }
            catch (Exception ex)
            {
                // Ignore duplicate's error
                //if (ex.errorCode == 13) return true;
                MessageBox.Show("SaveBarData() " + ex.Message);
                LogDB("SaveBarData() " + ex.Message);
                _connection.Close();
                return false;
            }
            return true;
        }
        public static BarData GetLastBarDataDiskOrMemory(string Symbol, BaseType Base, SqlConnection _connection)
        {

            return GetLastBarDataDisk(Symbol, Base, _connection);

            //TODO:
            try
            {
                //Try in memory:
                switch (Base)
                {
                    case BaseType.Days:
                        lock (StockBarsOnMemoryDay)
                        {
                            if (StockBarsOnMemoryDay.Count(s => s.Symbol == Symbol) > 0)
                                return StockBarsOnMemoryDay.Find(s => s.Symbol == Symbol);

                        }
                        break;
                    case BaseType.Minutes:
                        lock (StockBarsOnMemoryMin)
                        {
                            if (StockBarsOnMemoryMin.Count(s => s.Symbol == Symbol) > 0)
                                return StockBarsOnMemoryMin.Find(s => s.Symbol == Symbol);

                        }
                        break;
                    case BaseType.Minutes15:
                        lock (StockBarsOnMemoryMin15)
                        {
                            if (StockBarsOnMemoryMin15.Count(s => s.Symbol == Symbol) > 0)
                                return StockBarsOnMemoryMin15.Find(s => s.Symbol == Symbol);

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogDB(ex.Message + "\n" + ex.StackTrace);
                _connection.Close();
            }
            //Finally try on disk:
            return GetLastBarDataDisk(Symbol, Base, _connection);
        }
        public static BarData GetLastBarDataDisk(string Symbol, BaseType Base, SqlConnection _connection)
        {
            BarData LastBar = new BarData() { Symbol = Symbol, TradeDate = new DateTime(), ClosePrice = 0, BaseType = (int)Base };//Get last data from extremeDB:

            try
            {
                /*http://stackoverflow.com/questions/7745609/sql-select-only-rows-with-max-value-on-a-column
                select yt.id, yt.rev, yt.contents
                from YourTable yt
                inner join(
                    select id, max(rev) rev
                    from YourTable
                    group by id
                ) ss on yt.id = ss.id and yt.rev = ss.rev
                 * */
                List<BarData> All = GetBarDataAll(Symbol, 1, (int)Periodicity.Daily, _connection);

                if (All.Count() > 0)
                {
                    LastBar = All.OrderBy(b => b.TradeDateTicks).Last();
                }


                /*Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbol");

                //Search on Disk:
                if (cursorDisk.Search(Operation.Equals, new object[] { Base, Symbol }))
                {
                    while (cursorDisk.MoveNext())
                    {
                        if (cursorDisk.Current.Symbol == Symbol && cursorDisk.Current.BaseType == (int)Base)
                        {
                            if (cursorDisk.Current.TradeDate > LastBar.TradeDate) LastBar = new BarData(cursorDisk.Current);
                            //Backup data on memory:
                            //_connection.Insert(cursorDisk.Current);
                        }
                        else break;
                    }
                }
                _connection.CommitTransaction(); // end transaction
                 * */
            }
            catch (Exception ex)
            {
                LogDB(ex.Message + "\n" + ex.StackTrace);
            }
            return LastBar;
        }
        public static List<BarData> GetBarDataAll(string Symbol, int Interval, int Periodicity, SqlConnection _connection)
        {
            List<BarData> All = new List<BarData>();
            bool addInMemory = false;
            BaseType Base = BaseType.Days;
            string table = "BaseDay";
            if (Periodicity == (int)Interface.Periodicity.Minutely)
            {
                if (Interval % 15 == 0) {
                    Base = BaseType.Minutes15;
                    table = "BaseMin15";
                }
                else {
                    Base = BaseType.Minutes;
                    table = "BaseMin";
                }
            }
            if (Periodicity == (int)Interface.Periodicity.Hourly) {
                Base = BaseType.Minutes15;
                table = "BaseMin15";
            }

            try
            {
                _connection.Open();                
                SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM "+table+" WHERE Symbol = '"+Symbol+"'", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    double openPrice = rdr.GetDouble(3);
                    double highPrice = rdr.GetDouble(4);
                    double lowPrice = rdr.GetDouble(5);
                    double closePrice = rdr.GetDouble(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    double adjustS = rdr.GetDouble(10);
                    double adjustD = rdr.GetDouble(10);

                    All.Add(new BarData() {
                        TradeDateTicks = dateTick,
                        TradeDate = date,
                        Symbol = symbol,
                        OpenPrice = openPrice,
                        HighPrice = highPrice,
                        LowPrice = lowPrice,
                        ClosePrice = closePrice,
                        VolumeF = volumeF,
                        VolumeS = volumeS,
                        VolumeT = volumeT,
                        AdjustS = adjustS,
                        AdjustD = adjustD
                    });

                }
                _connection.Close();


                /*Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbol");
                Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbol");
                //Search in memory:
                if (cursor.Search(Operation.Equals, new object[] { Base, Symbol }))
                {
                    while (cursor.MoveNext())
                    {
                        if (cursor.Current.Symbol == Symbol && cursor.Current.BaseType == (int)Base)
                        {
                            All.Add(cursor.Current);
                        }
                        else break;
                    }
                }
                //Search on Disk:
                else if (cursorDisk.Search(Operation.Equals, new object[] { Base, Symbol }))
                {
                    while (cursorDisk.MoveNext())
                    {
                        if (cursorDisk.Current.Symbol == Symbol && cursorDisk.Current.BaseType == (int)Base)
                        {
                            All.Add(new BarData(cursorDisk.Current));
                            //Backup data on memory:
                            _connection.Insert(All.Last());
                            if (!addInMemory) addInMemory = true;
                        }
                        else break;
                    }
                }
                _connection.CommitTransaction(); // end transaction*/
            }
            catch (Exception ex)
            {
                LogDB(ex.Message + "\n" + ex.StackTrace);
                _connection.Close();
            }
            if (All.Count > 1)
            {
                //if (All[0].TradeDate > All[1].TradeDate) All.Reverse();
                All = All.OrderBy(b => b.TradeDateTicks).ToList();
                if (addInMemory)
                {
                    switch (Base)
                    {
                        case BaseType.Days:
                            lock (StockBarsOnMemoryDay)
                            {
                                StockBarsOnMemoryDay.Add(All.Last());
                            }
                            break;
                        case BaseType.Minutes:
                            lock (StockBarsOnMemoryMin)
                            {
                                StockBarsOnMemoryMin.Add(All.Last());
                            }
                            break;
                        case BaseType.Minutes15:
                            lock (StockBarsOnMemoryMin15)
                            {
                                StockBarsOnMemoryMin15.Add(All.Last());
                            }
                            break;
                    }
                }
                if ((Periodicity == (int)Interface.Periodicity.Daily && Interval > 1) || (Periodicity == (int)Interface.Periodicity.Minutely && Interval > 1 && Interval != 15) || (Periodicity == (int)Interface.Periodicity.Hourly) || (Periodicity > (int)Interface.Periodicity.Daily)) All = ProcessPeriodicity(All, Interval, Periodicity, _connection);
            }
            return All;
        }
        public static List<BarData> ProcessPeriodicity(List<BarData> BasicBars, int Interval, int Periodicity, SqlConnection _connection)
        {
            //long timeEllapsed = timer.ElapsedMilliseconds;
            Periodicity Period = (Periodicity)Periodicity;
            List<BarData> NewBars = new List<BarData>();

            int i = 0;

            BaseType Base = (BaseType)BasicBars[0].BaseType;

            //User as reference 10:00 to 17:00:
            DateTime timeReference = BasicBars[0].TradeDate.Date;
            DateTime nextTimeReference = BasicBars[0].TradeDate.Date;
            //Minutes:
            if ((Base == BaseType.Minutes || Base == BaseType.Minutes15) && Period != Interface.Periodicity.Hourly)
            {
                timeReference = timeReference.AddHours(10);
                nextTimeReference = nextTimeReference.AddHours(10);
                nextTimeReference = nextTimeReference.AddMinutes(Interval);
            }
            //Hours:
            else if (Period == Interface.Periodicity.Hourly)
            {
                timeReference = timeReference.AddHours(10);
                nextTimeReference = nextTimeReference.AddHours(10);
                nextTimeReference = nextTimeReference.AddHours(Interval);
            }
            //Days:
            else if (Period == Interface.Periodicity.Daily)
            {
                nextTimeReference = nextTimeReference.AddDays(Interval);
            }
            /*//Weeks:
            else if(Period == Interface.Periodicity.Weekly)
            {
                nextTimeReference = nextTimeReference.AddHours(5 * Interval);
            }
            //Months:
            else if (Period == Interface.Periodicity.Month)
            {
                nextTimeReference = nextTimeReference.AddHours(22 * Interval);
            }
            //Years:
            else if (Period == Interface.Periodicity.Year)
            {
                nextTimeReference = nextTimeReference.AddHours(260 * Interval);
            }*/

            while (i < BasicBars.Count)
            {
                // Test if there was any negociation on interval:
                if (Period < Interface.Periodicity.Weekly)
                {
                    if (BasicBars[i].TradeDate >= nextTimeReference)
                    {
                        //Minutes:
                        if ((Base == BaseType.Minutes || Base == BaseType.Minutes15) &&
                            Period != Interface.Periodicity.Hourly)
                        {
                            timeReference = timeReference.AddMinutes(Interval);
                            nextTimeReference = nextTimeReference.AddMinutes(Interval);
                            if (timeReference.Hour > 17)
                            {
                                timeReference = nextTimeReference = nextTimeReference.AddDays(1).Date;
                                timeReference = nextTimeReference = nextTimeReference.AddHours(10);
                                nextTimeReference = nextTimeReference.AddMinutes(Interval);
                            }
                        }
                        //Hours:
                        else if (Period == Interface.Periodicity.Hourly)
                        {
                            timeReference = timeReference.AddHours(Interval);
                            nextTimeReference = nextTimeReference.AddHours(Interval);
                            if (timeReference.Hour > 17)
                            {
                                timeReference = nextTimeReference = nextTimeReference.AddDays(1).Date;
                                timeReference = nextTimeReference = nextTimeReference.AddHours(10);
                                nextTimeReference = nextTimeReference.AddHours(Interval);
                            }
                        }
                        //Days:
                        else
                        {
                            timeReference = timeReference.AddDays(Interval);
                            nextTimeReference = nextTimeReference.AddDays(Interval);
                        }
                        continue;
                    }
                }
                string symbol = BasicBars[i].Symbol;
                DateTime date = timeReference;

                double high = BasicBars[i].HighPrice;
                double low = BasicBars[i].LowPrice;
                double open = BasicBars[i].OpenPrice;
                double close = BasicBars[i].ClosePrice;
                double volume = BasicBars[i].VolumeF;


                int intervalTemp = Interval;

                i++;

                if (i >= BasicBars.Count - 1)
                {
                    NewBars.Add(new BarData()
                    {
                        Symbol = symbol,
                        TradeDate = date,
                        OpenPrice = open,
                        HighPrice = high,
                        LowPrice = low,
                        ClosePrice = close,
                        VolumeF = volume
                    });
                    break;
                }

                if (Period < Interface.Periodicity.Weekly)
                {
                    while ((BasicBars[i].TradeDate < nextTimeReference) && (i < BasicBars.Count - 1))
                    {
                        close = BasicBars[i].ClosePrice;
                        low = (BasicBars[i].LowPrice < low) ? BasicBars[i].LowPrice : low;
                        high = (BasicBars[i].HighPrice > high) ? BasicBars[i].HighPrice : high;
                        volume += BasicBars[i].VolumeF;
                        i++;
                    }
                }
                else
                {
                    while ((intervalTemp > 0) && (i < BasicBars.Count - 1))
                    {
                        switch (Period)
                        {

                            case Interface.Periodicity.Weekly:
                                if (BasicBars[i - 1].TradeDate.DayOfWeek > BasicBars[i].TradeDate.DayOfWeek)
                                    intervalTemp--;
                                break;
                            case Interface.Periodicity.Month:
                                if (BasicBars[i - 1].TradeDate.Month != BasicBars[i].TradeDate.Month)
                                    intervalTemp--;
                                break;
                            case Interface.Periodicity.Year:
                                if (BasicBars[i - 1].TradeDate.Year != BasicBars[i].TradeDate.Year)
                                    intervalTemp--;
                                break;
                        }

                        if (intervalTemp <= 0) continue;

                        low = (BasicBars[i].LowPrice < low) ? BasicBars[i].LowPrice : low;
                        high = (BasicBars[i].HighPrice > high) ? BasicBars[i].HighPrice : high;
                        close = BasicBars[i].ClosePrice;
                        volume += BasicBars[i].VolumeF;
                        i++;
                    }
                    timeReference = BasicBars[i].TradeDate;
                }
                NewBars.Add(new BarData()
                {
                    Symbol = symbol,
                    TradeDate = date,
                    OpenPrice = open,
                    HighPrice = high,
                    LowPrice = low,
                    ClosePrice = close,
                    VolumeF = volume
                });

            }
            //LogDB("DATA PROCESSED = "+(timer.ElapsedMilliseconds - timeEllapsed));
            return NewBars;
        }

        public static double[] GetLastVolumes(string Symbol, int Interval, int Periodicity, SqlConnection _connection)
        {
            double[] result = new double[10];
            List<BarData> barAll = GetBarDataAll(Symbol, Interval, Periodicity, _connection);
            if (barAll.Count > 10)
            {
                for (int i = barAll.Count - 10; i < barAll.Count; i++)
                {
                    result[i - (barAll.Count - 10)] = barAll[i].VolumeF;
                }
            }
            else
            {
                int j = barAll.Count();
                for (int i = 0; i < 10 - j; i++)
                {
                    result[i] = 0;
                }
                for (int i = 10 - j; i < 10; i++)
                {
                    result[i] = barAll[i - 10 + j].VolumeF;
                }
            }
            SymbolSnapshot lastSnapshot = GetSnapshot(Symbol, BaseType.Days);
            if (lastSnapshot != null) result[10] = lastSnapshot.VolumeFinancial;
            return result;
        }
        public static TickData GetLastBarDataTick(string Symbol)
        {
            List<TickData> ticks = TicksOnMemory.Find(list => list.Count(s => s.Symbol == Symbol) > 0);
            if (ticks == null) return new TickData() { Buyer = -1, Price = 0, Quantity = 0, Seller = -1, Symbol = "NULL", TradeDate = new DateTime() };
            return ticks.Last();
        }
        public static bool SaveSnapshot(SymbolSnapshot snapshot, BaseType Base)
        {
            try
            {
                int index = -1;
                switch (Base)
                {
                    case BaseType.Days:
                        index = LastSnapshotDay.IndexOf(LastSnapshotDay.Find(s => s.Symbol == snapshot.Symbol));
                        if (index != null && index != -1)
                        {
                            LastSnapshotDay[index] = snapshot;
                        }
                        else LastSnapshotDay.Add(snapshot);
                        break;
                    case BaseType.Minutes:
                        index = LastSnapshotMin.IndexOf(LastSnapshotMin.Find(s => s.Symbol == snapshot.Symbol));
                        if (index != null && index != -1)
                        {
                            LastSnapshotMin[index] = snapshot;
                        }
                        else LastSnapshotMin.Add(snapshot);
                        break;
                    case BaseType.Minutes15:
                        index = LastSnapshotMin15.IndexOf(LastSnapshotMin15.Find(s => s.Symbol == snapshot.Symbol));
                        if (index != null && index != -1)
                        {
                            LastSnapshotMin15[index] = snapshot;
                        }
                        else LastSnapshotMin15.Add(snapshot);
                        break;
                }

            }
            catch (Exception ex)
            {
                LogDB(ex.Message);

                return false;

            }
            return true;
        }
        public static SymbolSnapshot GetSnapshot(string Symbol, BaseType Base)
        {
            SymbolSnapshot snapshot = null;
            try
            {
                int index = -1;
                switch (Base)
                {
                    case BaseType.Days:
                        index = LastSnapshotDay.IndexOf(LastSnapshotDay.Find(s => s.Symbol == Symbol));
                        if (index != null && index != -1)
                        {
                            snapshot = LastSnapshotDay[index];
                        }
                        else return null;
                        break;
                    case BaseType.Minutes:
                        index = LastSnapshotMin.IndexOf(LastSnapshotMin.Find(s => s.Symbol == Symbol));
                        if (index != null && index != -1)
                        {
                            snapshot = LastSnapshotMin[index];
                        }
                        else return null;
                        break;
                    case BaseType.Minutes15:
                        index = LastSnapshotMin15.IndexOf(LastSnapshotMin15.Find(s => s.Symbol == Symbol));
                        if (index != null && index != -1)
                        {
                            snapshot = LastSnapshotMin15[index];
                        }
                        else return null;
                        break;
                }

            }
            catch (Exception ex)
            {
                LogDB(ex.Message);

                return null;

            }
            return snapshot;
        }
        public static bool SaveTick(TickData tickData, SqlConnection _connection)
        {
            //Save ticks by symbol:
            int index = -1;
            index = TicksOnMemory.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
            if (index == null || index == -1) TicksOnMemory.Add(new List<TickData>() { tickData });
            else TicksOnMemory[index].Add(tickData);
            BarData lastBar;
            bool raiseEvent = false;
            // Subscribe for base = DAY:
            if (LastSnapshotDay.Exists(bar => bar.Symbol == tickData.Symbol))
            {
                //There's data waiting on buffer?
                if (TicksOnBufferDay.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
                {
                    foreach (TickData t in TicksOnBufferDay.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
                    {
                        if (t.Id > LastSnapshotDay.Find(s => s.Symbol == t.Symbol).Id) ProcessTickDay(t, _connection);
                    }
                    TicksOnBufferDay.Remove(
                        TicksOnBufferDay.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
                }
                ProcessTickDay(tickData, _connection);
                raiseEvent = true;
            }
            else
            {
                int indexDay = -1;
                indexDay = TicksOnBufferDay.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
                if (indexDay == null || indexDay == -1) TicksOnBufferDay.Add(new List<TickData>() { tickData });
                else TicksOnBufferDay[indexDay].Add(tickData);
            }

            // Subscribe for base = MIN:);
            if (LastSnapshotMin.Exists(bar => bar.Symbol == tickData.Symbol))
            {
                //There's data waiting on buffer?
                if (TicksOnBufferMin.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
                {
                    foreach (TickData t in TicksOnBufferMin.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
                    {
                        if (t.Id > LastSnapshotMin.Find(s => s.Symbol == t.Symbol).Id) ProcessTickMin(t, _connection);
                    }
                    TicksOnBufferMin.Remove(
                        TicksOnBufferMin.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
                }
                ProcessTickMin(tickData, _connection);
                raiseEvent = true;
            }
            else
            {
                int indexMin = -1;
                indexMin = TicksOnBufferMin.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
                if (indexMin == null || indexMin == -1) TicksOnBufferMin.Add(new List<TickData>() { tickData });
                else TicksOnBufferMin[indexMin].Add(tickData);
            }

            // Subscribe for base = MIN15:
            if (LastSnapshotMin15.Exists(bar => bar.Symbol == tickData.Symbol))
            {
                //There's data waiting on buffer?
                if (TicksOnBufferMin15.Exists(list => list.Count(s => s.Symbol == tickData.Symbol) > 0))
                {
                    foreach (TickData t in TicksOnBufferMin15.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0))
                    {
                        if (t.Id > LastSnapshotMin15.Find(s => s.Symbol == t.Symbol).Id) ProcessTickMin15(t, _connection);
                    }
                    TicksOnBufferMin15.Remove(
                        TicksOnBufferMin15.Find(list => list.Count(tick => tick.Symbol == tickData.Symbol) > 0));
                }
                ProcessTickMin15(tickData, _connection);
                raiseEvent = true;
            }
            else
            {
                int indexMin15 = -1;
                indexMin15 = TicksOnBufferMin15.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
                if (indexMin15 == null || indexMin15 == -1) TicksOnBufferMin15.Add(new List<TickData>() { tickData });
                else TicksOnBufferMin15[indexMin15].Add(tickData);
            }
            if (raiseEvent) return true;
            return false;
        }
        public static bool SaveTickTest(TickData tickData, SqlConnection _connection)
        {
            //Save ticks by symbol:
            int index = -1;
            index = TicksOnMemory.FindIndex(list => list.Find(s => s.Symbol == tickData.Symbol) != null);
            if (index == null || index == -1) TicksOnMemory.Add(new List<TickData>() { tickData });
            else TicksOnMemory[index].Add(tickData);
            BarData lastBar;
            bool raiseEvent = true;
            ProcessTickMin(tickData, _connection);
            ProcessTickMin15(tickData, _connection);
            ProcessTickDay(tickData, _connection);
            if (raiseEvent) return true;
            return false;
        }
        public static void ProcessTickDay(TickData tickData, SqlConnection _connection)
        {
            BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Days, _connection);
            if (!NewCandlesDay.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesDay.Add(lastBar);
            else lastBar = NewCandlesDay.Find(s => s.Symbol == tickData.Symbol);
            //Create new bar data?
            if (lastBar.TradeDate.Date != tickData.TradeDate.Date)
            {
                //Save on disk last candle created:
                if (NewCandlesDay.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
                {
                    SaveBarDatas(new List<BarDataPS>() { new BarDataPS(NewCandlesDay.Find(s => s.Symbol == tickData.Symbol)) },
                                _connection);
                    lastBar = new BarData
                    {
                        BaseType = lastBar.BaseType,
                        ClosePrice = tickData.Price,
                        HighPrice = tickData.Price,
                        LowPrice = tickData.Price,
                        OpenPrice = tickData.Price,
                        Symbol = tickData.Symbol,
                        TradeDate = tickData.TradeDate.Date,
                        TradeDateTicks = tickData.TradeDate.Date.Ticks,
                        VolumeF = tickData.Quantity
                    };
                }
                else
                {
                    lastBar = new BarData
                    {
                        BaseType = lastBar.BaseType,
                        ClosePrice = tickData.Price,
                        HighPrice = tickData.Price,
                        LowPrice = tickData.Price,
                        OpenPrice = tickData.Price,
                        Symbol = tickData.Symbol,
                        TradeDate = tickData.TradeDate.Date,
                        TradeDateTicks = tickData.TradeDate.Date.Ticks,
                        VolumeF = tickData.Quantity
                    };
                    SaveBarDatas(new List<BarDataPS>() { new BarDataPS(lastBar) },
                                _connection);

                }



            }
            //Merge tick with last BarData?
            else
            {
                lastBar = new BarData
                {
                    BaseType = lastBar.BaseType,
                    ClosePrice = tickData.Price,
                    HighPrice =
                        tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
                    LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
                    OpenPrice = lastBar.OpenPrice,
                    Symbol = tickData.Symbol,
                    TradeDate = lastBar.TradeDate,
                    TradeDateTicks = lastBar.TradeDateTicks,
                    VolumeF = lastBar.VolumeF + tickData.Quantity
                };

            }
            NewCandlesDay.Remove(NewCandlesDay.Find(s => s.Symbol == tickData.Symbol));
            NewCandlesDay.Add(lastBar);
            SymbolSnapshot snapshot = LastSnapshotDay.Find(s => s.Symbol == lastBar.Symbol);
            snapshot.Id = tickData.Id;
            snapshot.Close = tickData.Price;
            snapshot.Open = lastBar.OpenPrice;
            snapshot.High = lastBar.HighPrice;
            snapshot.Low = lastBar.LowPrice;
            snapshot.Quantity = tickData.Quantity;
            snapshot.Seller = tickData.Seller;
            snapshot.Timestamp = tickData.TradeDate;
            snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
            snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
            snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
            LastSnapshotDay.Remove(LastSnapshotDay.Find(s => s.Symbol == lastBar.Symbol));
            LastSnapshotDay.Add(snapshot);
            SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Days));
        }
        public static void ProcessTickMin(TickData tickData, SqlConnection _connection)
        {
            BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Minutes, _connection);
            if (!NewCandlesMin.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesMin.Add(lastBar);
            else lastBar = NewCandlesMin.Find(s => s.Symbol == tickData.Symbol);
            //Merge tick with last BarData?
            if (lastBar.TradeDate.Date == tickData.TradeDate.Date && lastBar.TradeDate.Minute == tickData.TradeDate.Minute)
            {
                lastBar = new BarData
                {
                    BaseType = lastBar.BaseType,
                    ClosePrice = tickData.Price,
                    HighPrice =
                        tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
                    LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
                    OpenPrice = lastBar.OpenPrice,
                    Symbol = tickData.Symbol,
                    TradeDate = lastBar.TradeDate,
                    TradeDateTicks = lastBar.TradeDateTicks,
                    VolumeF = lastBar.VolumeF + tickData.Quantity
                };
            }
            //Create new bar data?
            else
            {
                //Save on disk last candle created:
                if (NewCandlesMin.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
                {
                    SaveBarDatas(
                        new List<BarDataPS>() { new BarDataPS(NewCandlesMin.Find(s => s.Symbol == tickData.Symbol)) },
                        _connection);
                }
                lastBar = new BarData
                {
                    BaseType = lastBar.BaseType,
                    ClosePrice = tickData.Price,
                    HighPrice = tickData.Price,
                    LowPrice = tickData.Price,
                    OpenPrice = tickData.Price,
                    Symbol = tickData.Symbol,
                    TradeDate = tickData.TradeDate,
                    TradeDateTicks = tickData.TradeDate.Date.Ticks,
                    VolumeF = tickData.Quantity
                };
                NewBarEvent(new object(), new BarEventArgs(lastBar, (int)BaseType.Minutes, false));

            }
            NewCandlesMin.Remove(NewCandlesMin.Find(s => s.Symbol == tickData.Symbol));
            NewCandlesMin.Add(lastBar);
            SymbolSnapshot snapshot = LastSnapshotMin.Find(s => s.Symbol == lastBar.Symbol);
            snapshot.Id = tickData.Id;
            snapshot.Close = tickData.Price;
            snapshot.Open = lastBar.OpenPrice;
            snapshot.High = lastBar.HighPrice;
            snapshot.Low = lastBar.LowPrice;
            snapshot.Quantity = tickData.Quantity;
            snapshot.Seller = tickData.Seller;
            snapshot.Timestamp = tickData.TradeDate;
            snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
            snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
            snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
            LastSnapshotMin.Remove(LastSnapshotMin.Find(s => s.Symbol == lastBar.Symbol));
            LastSnapshotMin.Add(snapshot);
            SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Minutes));
        }
        public static void ProcessTickMin15(TickData tickData, SqlConnection _connection)
        {
            BarData lastBar = GetLastBarDataDiskOrMemory(tickData.Symbol, BaseType.Minutes15, _connection);
            if (!NewCandlesMin15.Exists(bar => bar.Symbol == tickData.Symbol)) NewCandlesMin15.Add(lastBar);
            else lastBar = NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol);
            //Merge tick with last BarData?
            if (lastBar.TradeDate.Date == tickData.TradeDate.Date && tickData.TradeDate.Minute < lastBar.TradeDate.Minute + 15 && tickData.TradeDate.Hour == lastBar.TradeDate.Hour)
            {
                lastBar = new BarData
                {
                    BaseType = lastBar.BaseType,
                    ClosePrice = tickData.Price,
                    HighPrice =
                        tickData.Price > lastBar.HighPrice ? tickData.Price : lastBar.HighPrice,
                    LowPrice = tickData.Price < lastBar.LowPrice ? tickData.Price : lastBar.LowPrice,
                    OpenPrice = lastBar.OpenPrice,
                    Symbol = tickData.Symbol,
                    TradeDate = lastBar.TradeDate,
                    TradeDateTicks = lastBar.TradeDateTicks,
                    VolumeF = lastBar.VolumeF + tickData.Quantity
                };
            }
            //Create new bar data?
            else
            {
                //Save on disk last candle created:
                if (NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol).TradeDate != DateTime.MinValue)
                {
                    SaveBarDatas(
                        new List<BarDataPS>() { new BarDataPS(NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol)) },
                        _connection);
                    lastBar = new BarData
                    {
                        BaseType = lastBar.BaseType,
                        ClosePrice = tickData.Price,
                        HighPrice = tickData.Price,
                        LowPrice = tickData.Price,
                        OpenPrice = tickData.Price,
                        Symbol = tickData.Symbol,
                        TradeDate = tickData.TradeDate,
                        TradeDateTicks = tickData.TradeDate.Date.Ticks,
                        VolumeF = tickData.Quantity
                    };
                }
                else
                {
                    lastBar = new BarData
                    {
                        BaseType = lastBar.BaseType,
                        ClosePrice = tickData.Price,
                        HighPrice = tickData.Price,
                        LowPrice = tickData.Price,
                        OpenPrice = tickData.Price,
                        Symbol = tickData.Symbol,
                        TradeDate = tickData.TradeDate,
                        TradeDateTicks = tickData.TradeDate.Date.Ticks,
                        VolumeF = tickData.Quantity
                    };
                    SaveBarDatas(
                        new List<BarDataPS>() { new BarDataPS(lastBar) },
                        _connection);
                }


                NewBarEvent(new object(), new BarEventArgs(lastBar, (int)BaseType.Minutes15, false));

            }
            NewCandlesMin15.Remove(NewCandlesMin15.Find(s => s.Symbol == tickData.Symbol));
            NewCandlesMin15.Add(lastBar);
            SymbolSnapshot snapshot = LastSnapshotMin15.Find(s => s.Symbol == lastBar.Symbol);
            snapshot.Id = tickData.Id;
            snapshot.Close = tickData.Price;
            snapshot.Open = lastBar.OpenPrice;
            snapshot.High = lastBar.HighPrice;
            snapshot.Low = lastBar.LowPrice;
            snapshot.Quantity = tickData.Quantity;
            snapshot.Seller = tickData.Seller;
            snapshot.Timestamp = tickData.TradeDate;
            snapshot.VolumeFinancial = snapshot.VolumeFinancial + tickData.Quantity;
            snapshot.VolumeStocks = snapshot.VolumeStocks + tickData.Quantity;
            snapshot.VolumeTrades = snapshot.VolumeTrades + tickData.Quantity;
            LastSnapshotMin15.Remove(LastSnapshotMin15.Find(s => s.Symbol == lastBar.Symbol));
            LastSnapshotMin15.Add(snapshot);
            SnapshotEvent(new object(), new SnapshotEventArgs(snapshot, (int)BaseType.Minutes15));
        }
        public static bool TickIsProcessed(string Symbol, BaseType Base)
        {
            switch (Base)
            {
                case BaseType.Days:
                    return LastSnapshotDay.Exists(b => b.Symbol == Symbol);
                case BaseType.Minutes:
                    return LastSnapshotMin.Exists(b => b.Symbol == Symbol);
                case BaseType.Minutes15:
                    return LastSnapshotMin15.Exists(b => b.Symbol == Symbol);
            }
            return false;
        }
        public static bool AdjustBarDatas(BarData LastBar, SqlConnection _connection)
        {
            return false;

            /*double adjustment;
            DateTime PreviousDate = new DateTime();
            bool first = true;
            try
            {
                //Update data from extremeDB on disk:
                _connection.StartTransaction(Database.TransactionType.ReadWrite);
                // Request all data from base:
                Cursor<BarDataDisk> cursorDisk = new Cursor<BarDataDisk>(_connection, "byBaseSymbolDate");
                Cursor<BarData> cursor = new Cursor<BarData>(_connection, "byBaseSymbol");
                adjustment = 0;
                BarDataDisk BarCursorDisk;
                BarDataDisk LastBarDisk = new BarDataDisk()
                {
                    BaseType = LastBar.BaseType,
                    ClosePrice = LastBar.ClosePrice,
                    HighPrice = LastBar.HighPrice,
                    LowPrice = LastBar.LowPrice,
                    OpenPrice = LastBar.OpenPrice,
                    Symbol = LastBar.Symbol,
                    TradeDate = LastBar.TradeDate,
                    TradeDateTicks = LastBar.TradeDateTicks,
                    VolumeF = LastBar.VolumeF
                };
                PreviousDate = new DateTime();
                first = true;
                if (cursorDisk.Search(Operation.Equals, new object[] { LastBarDisk.BaseType, LastBarDisk.Symbol, LastBarDisk.TradeDate.Ticks }))
                {
                    while (cursorDisk.MovePrev())
                    {
                        if (first)
                        {
                            BarCursorDisk = cursorDisk.Current;
                            adjustment = (LastBarDisk.ClosePrice - BarCursorDisk.ClosePrice) / BarCursorDisk.ClosePrice;
                            BarCursorDisk.ClosePrice += adjustment * BarCursorDisk.ClosePrice;
                            BarCursorDisk.OpenPrice += adjustment * BarCursorDisk.OpenPrice;
                            BarCursorDisk.LowPrice += adjustment * BarCursorDisk.LowPrice;
                            BarCursorDisk.HighPrice += adjustment * BarCursorDisk.HighPrice;
                            PreviousDate = BarCursorDisk.TradeDate;
                            cursorDisk.Update();
                            first = false;
                            continue;
                        }
                        if (cursorDisk.Current.Symbol == LastBarDisk.Symbol && cursorDisk.Current.BaseType == LastBarDisk.BaseType)
                        {
                            BarCursorDisk = cursorDisk.Current;
                            if (BarCursorDisk.TradeDate <= PreviousDate)
                            {
                                BarCursorDisk.ClosePrice += adjustment * BarCursorDisk.ClosePrice;
                                BarCursorDisk.OpenPrice += adjustment * BarCursorDisk.OpenPrice;
                                BarCursorDisk.LowPrice += adjustment * BarCursorDisk.LowPrice;
                                BarCursorDisk.HighPrice += adjustment * BarCursorDisk.HighPrice;
                                PreviousDate = BarCursorDisk.TradeDate;
                                cursorDisk.Update();
                            }
                            else
                            {
                                _connection.RollbackTransaction();
                                return false;
                            }
                        }
                        else break;
                    }
                }

                // Remove data from database in memory and reload:
                if (cursor.Search(Operation.Equals,
                                           new object[] { LastBar.BaseType, LastBar.Symbol }))
                {
                    while (cursor.MoveNext()) cursor.Remove();
                }
                _connection.CommitTransaction();

                switch ((BaseType)LastBar.BaseType)
                {
                    case BaseType.Days:
                        lock (StockBarsOnMemoryDay)
                        {
                            if (StockBarsOnMemoryDay.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
                            {
                                StockBarsOnMemoryDay.Remove(StockBarsOnMemoryDay.First(stock => stock.Symbol == LastBar.Symbol));
                                GetBarDataAll(LastBar.Symbol, 1, Periodicity.Daily.GetHashCode(), _connection);
                            }
                        }
                        break;
                    case BaseType.Minutes:
                        if (StockBarsOnMemoryMin.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
                        {
                            StockBarsOnMemoryMin.Remove(StockBarsOnMemoryMin.First(stock => stock.Symbol == LastBar.Symbol));
                            GetBarDataAll(LastBar.Symbol, 1, Periodicity.Minutely.GetHashCode(), _connection);
                        }
                        break;
                    case BaseType.Minutes15:
                        if (StockBarsOnMemoryMin15.Count(stock => stock.Symbol == LastBar.Symbol) > 0)
                        {
                            StockBarsOnMemoryMin15.Remove(StockBarsOnMemoryMin15.First(stock => stock.Symbol == LastBar.Symbol));
                            GetBarDataAll(LastBar.Symbol, 15, Periodicity.Minutely.GetHashCode(), _connection);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogDB("AdjustBarData() " + ex.Message);
                return false;
            }
            return true;
            */
        }

    }

}
