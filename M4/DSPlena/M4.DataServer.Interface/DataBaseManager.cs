using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExtremeDB;
using M4.DataServer.Interface.ProtocolStructs;

namespace M4.DataServer.Interface
{
    public enum DataBaseType
    {
        AllInMemory,
        Persistent,
        SharedInMemory,
        SharedPersistent
    }
    public class DataBaseManager
    {
        const int PAGE_SIZE = 128;
        const int DISK_PAGE_SIZE = 4096;
        const int DISK_CACHE_SIZE = 64 * 1024 * 1024;
        const int DATABASE_SIZE = 256 * 1024 * 1024;

        private string _path;
        private Database _database;
        private Connection _connection;
        public static DataBaseManager _database_daily_shared;
        public static DataBaseManager _database_symbol_shared;
        private Database.Parameters _parameters;
        private string _name;
        private DataBaseType _type;
        private Database.Mode _mode;
        private Type _class;
        private Database.Device[] _devices;
        private const int _database_size = 256 * 1024 * 1024; //256MB
        private const int _page_size = 128; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
        private const int _cache_size = 64 * 1024 * 1024;
        private const int _disk_page_size = 4096; //PAGESIZE MAX = 130.072, FILE MAX = 14.873KB
        private const int _disk_cache_size = 64 * 1024 * 1024;

        //private static DataBaseManager _instance;


        public DataBaseManager(string Name, DataBaseType Type, Type ClassType, string Path)
        {
            _name = Name;
            _path = Path;
            _type = Type;
            _class = ClassType;
            _parameters = new Database.Parameters();
            _parameters.MemPageSize = PAGE_SIZE;
            _parameters.Classes = new Type[] { _class };
            _parameters.MaxConnections = 10;
            _mode = 0;
            switch (Type)
            {
                case DataBaseType.AllInMemory:
                    _parameters.DatabaseSnapshotFilePath = _path + _name + ".img"; // set database image filename
                    _devices = new Database.Device[]
                                   {
                                       new Database.PrivateMemoryDevice(Database.Device.Kind.Data, _database_size)
                                   };
                    break;
                case DataBaseType.SharedInMemory:
                    _parameters.DatabaseSnapshotFilePath = _path + _name + ".img"; // set database image filename
                    _mode |= Database.Mode.SharedMemorySupport;
                    _devices = new Database.Device[]
                                   {
                                       new Database.SharedMemoryDevice(Database.Device.Kind.Data, _name, new IntPtr(0),
                                                                       _database_size)
                                   };
                    break;
                case DataBaseType.Persistent:
                    _parameters.DiskPageSize = DISK_PAGE_SIZE;
                    _parameters.DiskClassesByDefault = true; // mark @Persistent classes as on-disk classes by default
                    _mode |= Database.Mode.DiskSupport;
                    _devices = new Database.Device[]
                                   {
                                       new Database.PrivateMemoryDevice(
                                           Database.Device.Kind.Data, DATABASE_SIZE),
                                       new Database.PrivateMemoryDevice(
                                           Database.Device.Kind.DiskCache, DISK_CACHE_SIZE),
                                       new Database.FileDevice(
                                           Database.Device.Kind.Data, _path + _name + ".dbs"),
                                       new Database.FileDevice(
                                           Database.Device.Kind.TransactionLog,
                                           _path + _name + ".log")
                                   };
                    break;
                case DataBaseType.SharedPersistent:
                    _parameters.DiskPageSize = _page_size;
                    _parameters.DiskClassesByDefault = true; // mark @Persistent classes as on-disk classes by default
                    _mode |= Database.Mode.DiskSupport;
                    _mode |= Database.Mode.SharedMemorySupport;
                    _devices = new Database.Device[]
                                   {
                                       new Database.SharedMemoryDevice(
                                           Database.Device.Kind.Data, _name, new IntPtr(0), _database_size),
                                       new Database.SharedMemoryDevice(
                                           Database.Device.Kind.DiskCache, _name + "-cache", new IntPtr(0), _cache_size),
                                       new Database.FileDevice(
                                           Database.Device.Kind.Data, _path + _name + ".dbs"),
                                       new Database.FileDevice(
                                           Database.Device.Kind.TransactionLog,
                                           _path + _name + ".log")
                                   };
                    break;
            }
            try
            {
                _database = new Database(_mode, "../../../libs/extremeDB");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_instance = this;
        }

        public DataBaseManager DatabaseSymbolShared()
        {
            return _database_symbol_shared;
        }

        public DataBaseManager DatabaseDailyShared()
        {
            return _database_daily_shared;
        }

       /* public Database SymbolSharedInstance(string Name, DataBaseType Type, Type ClassType, string Path)
        {
            // First time create Database and Connection:
            if(_database_symbol_shared == null)
            {
                _name = Name;
                _path = Path;
                _type = Type;
                _class = ClassType;
                _parameters = new Database.Parameters();
                _parameters.MemPageSize = PAGE_SIZE;
                _parameters.Classes = new Type[] { _class };
                _parameters.MaxConnections = 10;
                _mode = 0;
                switch (Type)
                {
                    case DataBaseType.SharedInMemory:
                        _parameters.DatabaseSnapshotFilePath = _path + _name + ".img"; // set database image filename
                        _mode |= Database.Mode.SharedMemorySupport;
                        _devices = new Database.Device[]
                                   {
                                       new Database.SharedMemoryDevice(Database.Device.Kind.Data, _name, new IntPtr(0),
                                                                       _database_size)
                                   };
                        break;
                    case DataBaseType.SharedPersistent:
                        _parameters.DiskPageSize = _page_size;
                        _parameters.DiskClassesByDefault = true; // mark @Persistent classes as on-disk classes by default
                        _mode |= Database.Mode.DiskSupport;
                        _mode |= Database.Mode.SharedMemorySupport;
                        _devices = new Database.Device[]
                                   {
                                       new Database.SharedMemoryDevice(
                                           Database.Device.Kind.Data, _name, new IntPtr(0), _database_size),
                                       new Database.SharedMemoryDevice(
                                           Database.Device.Kind.DiskCache, _name + "-cache", new IntPtr(0), _cache_size),
                                       new Database.FileDevice(
                                           Database.Device.Kind.Data, _path + _name + ".dbs"),
                                       new Database.FileDevice(
                                           Database.Device.Kind.TransactionLog,
                                           _path + _name + ".log")
                                   };
                        break;
                }
                try
                {
                    _database_symbol_shared = new Database(_mode, "../../../libs/extremeDB");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return _database_symbol_shared;
        }*/

        public bool Connect()
        {
            try
            {
                _database.Open(_name, _parameters, _devices);
                _connection = new Connection(_database);
            }
            catch (DatabaseError dbe)
            {
                if (dbe.errorCode != 66)
                {
                    // code 66 means duplicate instance. Valid case for SHM configuration
                    if (dbe.errorCode == 62)
                        Console.WriteLine(
                            "eXtremeDB assembly is not compatible with option 'sharedmemory'. Please replace reference to assembly with shared memory functionality");
                    else if (dbe.errorCode == 620000)
                        Console.WriteLine(
                            "eXtremeDB assembly is not compatible with option 'disk'. Please replace reference to assembly with disk manager functionality");
                    throw dbe;
                }
            }
            return true;
        }

        public bool Disconnect()
        {
            try
            {
                _connection.Disconnect();
                _database.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DataBaseManager] " + ex.Message);
            }
            return true;
        }

        public List<SymbolsPS> LoadSymbols()
        {
            List<SymbolsPS> StockList = new List<SymbolsPS>();
            try
            {
                // open read-only transaction
                _connection.StartTransaction(Database.TransactionType.ReadOnly);
                // Open cursor by "id" index
                int StockID = 0;
                foreach (Symbol symbol in _connection.GetTable<Symbol>())
                {
                    // get all objects
                    StockList.Add(new SymbolsPS
                    {
                        StockId = StockID,
                        StockInfo = symbol
                    });
                    StockID++;
                }
                //The last register is StockInfo="NULL" and StockID = Registers Count
                /*StockList.Add(new SymbolsPS
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
                _connection.RollbackTransaction(); // end transaction
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return StockList;
        }

        public Boolean SaveSymbols(List<SymbolsPS> StockList)
        {
            try
            {
                _connection.StartTransaction(Database.TransactionType.ReadWrite);
                foreach (SymbolsPS stock in StockList)
                {
                    _connection.Insert(stock.StockInfo);
                }
                _connection.CommitTransaction();

                // save database image to file
                Console.WriteLine("\tSave database...");
                if (_type == DataBaseType.AllInMemory || _type == DataBaseType.SharedInMemory) _connection.SaveSnapshot(_path + _name + ".img");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public List<BarData> GetBarDataAll(string Symbol)
        {
            List<BarData> All = new List<BarData>();
            try
            {
                //Get data from extremeDB:
                _connection.StartTransaction(Database.TransactionType.ReadOnly);
                // Request all data from base:
                var query = from bar in _connection.GetTable<BarData>()
                            where bar.Symbol == Symbol
                            orderby bar.TradeDate
                            select bar;

                if (query.Count() > 0)
                {
                    foreach (BarData bar in query)
                    {
                        All.Add(bar);
                    }
                }
                _connection.CommitTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DataBaseManager] " + ex.Message);
            }
            return All;
        }

        public List<BarData> GetBarDataSince(string Symbol, BarData Last)
        {
            List<BarData> All = new List<BarData>();
            try
            {
                //Get data from extremeDB:
                _connection.StartTransaction(Database.TransactionType.ReadOnly);
                // Request data from base:
                var query = from bar in _connection.GetTable<BarData>()
                            where bar.Symbol == Symbol &&
                                bar.TradeDate >= Last.TradeDate
                            orderby bar.TradeDate
                            select bar;
                if (query.Count() > 0)
                {
                    foreach (BarData bar in query)
                    {
                        All.Add(bar);
                    }
                }
                _connection.CommitTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DataBaseManager] " + ex.Message);
            }
            return All;
        }

        public BarData GetLastBarData(string Symbol)
        {
            BarData LastBar = new BarData() { Symbol = Symbol, TradeDate = new DateTime(), ClosePrice = 0 };//Get last data from extremeDB:
            _connection.StartTransaction(Database.TransactionType.ReadOnly);
            /*foreach (BarData barData in _connection.GetTable<BarData>())
            {
                if(barData.Symbol==Symbol)
                {
                    if (barData.TradeDate > LastBar.TradeDate) LastBar = barData;
                }
            }*/
            var query = from bar in _connection.GetTable<BarData>()
                        where bar.Symbol == Symbol
                        orderby bar.TradeDate
                        select bar;
            if (query.Count() > 0)
            {
                LastBar = query.Last();
            }
            _connection.RollbackTransaction(); // end transaction
            return LastBar;
        }

        public bool SaveBarDatas(List<BarDataPS> Bars)
        {
            try
            {
                //var query = from bar in Bars orderby bar.Symbol select bar;
                foreach (BarDataPS bar in Bars)
                {
                    _connection.StartTransaction(Database.TransactionType.ReadWrite);
                    _connection.Insert(bar.Data);
                    _connection.CommitTransaction();
                }

                // save database image to file
                Console.WriteLine("\tSaved BarData on database...");
                if (_type == DataBaseType.AllInMemory || _type == DataBaseType.SharedInMemory) _connection.SaveSnapshot(_path + _name + ".img");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DataBaseManager] " + ex.Message);
                return false;
            }
            return true;
        }

        public bool Clear()
        {
            try
            {
                if (Disconnect())
                {
                    if (_type == DataBaseType.AllInMemory || _type == DataBaseType.SharedInMemory)
                    {
                        if (File.Exists(_path + _name + ".img"))
                        {
                            File.Delete(_path + _name + ".img");
                        }
                    }
                    else
                    {
                        if (File.Exists(_path + _name + ".dbs"))
                        {
                            File.Delete(_path + _name + ".dbs");
                        }
                        if (File.Exists(_path + _name + ".log"))
                        {
                            File.Delete(_path + _name + ".log");
                        }
                    }
                    if (!Connect()) return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.StartTransaction(Database.TransactionType.ReadOnly);
                if (_class == typeof(Symbol)) count = _connection.GetTable<Symbol>().Count();
                if (_class == typeof(BarData)) count = _connection.GetTable<BarData>().Count();
                _connection.RollbackTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }


    }
}
