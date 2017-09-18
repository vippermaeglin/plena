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

    public class DBlocalSQL
    {
        //TODO: change path reference!
        private static string _stringConnection = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=\"" + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena\\PlenaData.mdf\"; Integrated Security=true;Connection Timeout=60"; //;User ID=plena;Password=plena";
        private static List<SqlConnection> _connections = new List<SqlConnection>();
        private static bool _stockListOk = false;
        private static bool _portfoliosOk = false;
        private static PortfolioDataSet PortfolioView = new PortfolioDataSet();
        private static List<string> _symbolsWithData = new List<string>();


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
                LogDB("Connect() " + ex.Message);
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

        public static bool Shrink(SqlConnection _connection)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("DBCC SHRINKDATABASE(0)", _connection);
                cmd.CommandTimeout = 1800;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogDB("\nShrink() " + ex.Message);
                return false;
            }
            finally
            {
                _connection.Close();
            }
            return true;
        }

        public static string GetSqlVersion(SqlConnection _connection)
        {
            string version = "";
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Value FROM SqlInfo WHERE Description = 'Version'", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    version = rdr.IsDBNull(0) ? null : rdr.GetString(0);
                }
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB("GetSqlVersion() " + ex.Message);
                _connection.Close();
            }
            return version;
        }

        public static bool SaveSqlVersion(SqlConnection _connection, string version)
        {
            bool result = true;
            try
            {
                _connection.Open();
                //Clean old versions:
                SqlCommand cmd = new SqlCommand("DELETE FROM SqlInfo WHERE Description = 'Version'", _connection);
                cmd.ExecuteNonQuery();
                //Save new version:
                cmd = new SqlCommand("INSERT INTO SqlInfo (Id, Value, Description) VALUES (0,'" + version + "', 'Version' )", _connection);
                cmd.ExecuteNonQuery();
                _connection.Close();

                // save database image to file
                LogDB("\tUpdated SQl Version to " + version);
            }
            catch (Exception ex)
            {
                LogDB("SaveSqlVersion() " + ex.Message);
                _connection.Close();
                return false;
            }
            return result;
        }

        public static CommandRequest GetLastServerCommand(SqlConnection _connection, int _serverCommand)
        {
            CommandRequest result = new CommandRequest() { CommandID = _serverCommand, Date = DateTime.MinValue, Parameters = "" };
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT CommandId, Date, Parameters FROM ServerIntegrity WHERE Date = (SELECT MAX(Date) FROM ServerIntegrity)", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    result = new CommandRequest()
                    {
                        CommandID = rdr.IsDBNull(0) ? -1 : rdr.GetInt32(0),
                        Date = rdr.IsDBNull(1) ? DateTime.MinValue : rdr.GetDateTime(1),
                        Parameters = rdr.IsDBNull(2) ? null : rdr.GetString(2)
                    };
                }
                _connection.Close();
            }
            catch (Exception ex)
            {
                LogDB("GetLastServerCommand() " + ex.Message);
                _connection.Close();
            }
            return result;
        }

        public static bool SaveServerCommand(CommandRequest command, SqlConnection _connection)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ServerIntegrity (CommandId,Date,Parameters) VALUES (" + command.CommandID + ",'" + command.Date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + command.Parameters + "')", _connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                LogDB("SaveServerCommand() " + ex.Message);
                _connection.Close();

            }
            _connection.Close();
            return true;
        }

        /// <summary>
        /// Save symbols to database.
        /// </summary>
        /// <param name="StockList"></param>
        /// <returns>Returns true if succeeded</returns>
        public static bool SaveSymbols(List<Symbol> StockList, SqlConnection _connection, bool Sync = false)
        {
            try
            {
                _connection.Open();
                //Clean old symbols:
                string table = Sync ? "SymbolsSync" : "Symbols";
                SqlCommand cmd = new SqlCommand("DELETE FROM " + table, _connection);
                cmd.ExecuteNonQuery();
                //Save symbols:
                foreach (Symbol symbol in StockList)
                {
                    cmd = new SqlCommand("INSERT INTO " + table + " (Code) VALUES ('" + symbol.Code + "')", _connection);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();
                _stockListOk = true;
                LogDB("\tSaved {0} {1} database...", StockList.Count, table);
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
                    if (group.Name == "All") continue;
                    string symbolsList = group.Symbols;
                    cmd = new SqlCommand("INSERT INTO Portfolios (pName, pIndex, pType, pSymbols) VALUES ('" + group.Name + "'," + group.Index + "," + group.Type + ",'" + symbolsList + "')", _connection);
                    cmd.ExecuteNonQuery();
                }
                _connection.Close();

                // save database image to file
                LogDB("\tSaved portfolios");
                //foreach(SymbolGroup group in Groups) LogDB(" "+group.Name);
            }
            catch (Exception ex)
            {
                LogDB(ex.Message);
                _connection.Close();
                return false;
            }
            return true;
        }

        public static bool UpdateGroup(SymbolGroup Group, SqlConnection _connection)
        {
            try
            {
                _connection.Open();
                //Clean old portfolios:
                SqlCommand cmd = new SqlCommand("DELETE FROM Portfolios WHERE pName = '" + Group.Name + "'", _connection);
                cmd.ExecuteNonQuery();
                //Update portfolio:
                string symbolsList = Group.Symbols;
                cmd = new SqlCommand("INSERT INTO Portfolios (pName, pIndex, pType, pSymbols) VALUES ('" + Group.Name + "'," + Group.Index + "," + Group.Type + ",'" + symbolsList + "')", _connection);
                cmd.ExecuteNonQuery();
                _connection.Close();

                // save database image to file
                LogDB("\tUpdated portfolio " + Group.Name);
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
        public static List<Symbol> LoadSymbols(SqlConnection _connection, bool Sync = false)
        {
            List<Symbol> StockList = new List<Symbol>();
            try
            {
                // open read-only transaction
                _connection.Open();
                // Open cursor by "id" index
                int StockID = 0;
                string table = Sync ? "SymbolsSync" : "Symbols";
                SqlCommand cmd = new SqlCommand("SELECT Code, Name, Sector, SubSector, Segment, Source, Type, Activity, Site, Status, Priority FROM " + table, _connection);
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
                    StockList.Add(new Symbol()
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
        /// Get Symbol's List from database.
        /// </summary>
        /// <returns></returns>
        public static List<SymbolsPS> LoadSymbolsWithData(SqlConnection _connection)
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

                //Remove symbols without data:
                List<SymbolsPS> StockList2 = new List<SymbolsPS>();
                foreach (SymbolsPS symbol in StockList)
                {
                    if (GetBarDataAll(symbol.StockInfo.Code, 1, (int)Periodicity.Daily, _connection).Count() > 1) StockList2.Add(symbol);
                }
                StockList = StockList2;
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
                SqlCommand cmd = new SqlCommand("SELECT pName, pIndex, pType, pSymbols FROM Portfolios WHERE Name = " + Name, _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string name = rdr.GetString(0);
                    int index = rdr.IsDBNull(1) ? -1 : rdr.GetInt32(1);
                    int type = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    string symbols = rdr.IsDBNull(3) ? null : rdr.GetString(3);
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
                SqlCommand cmd = new SqlCommand("SELECT pName, pIndex, pType, pSymbols FROM Portfolios WHERE pType = " + ((int)Type), _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string name = rdr.GetString(0);
                    int index = rdr.IsDBNull(1) ? -1 : rdr.GetInt32(1);
                    int type = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    string symbols = rdr.IsDBNull(3) ? null : rdr.GetString(3);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM Portfolios WHERE pName = '" + Name + "'", _connection);
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
                    cmd = new SqlCommand("SELECT DateTicks FROM " + table + " WHERE Symbol = '" + bar.Data.Symbol + "' AND DateTicks = " + bar.Data.TradeDateTicks, _connection);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    bool ignore = false;
                    while (rdr.Read())
                    {
                        ignore = true;
                    }
                    rdr.Close();
                    if (ignore || bar.Data.TradeDate == DateTime.MinValue) continue;
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
        public static bool SaveBarDatas(List<BarData> Bars, SqlConnection _connection)
        {
            try
            {
                bool inMemory = false;
                string table = "BaseDay";
                switch ((BaseType)Bars[0].BaseType)
                {
                    case BaseType.Days:
                        inMemory = StockBarsOnMemoryDay.Exists(s => s.Symbol == Bars[0].Symbol);
                        break;
                    case BaseType.Minutes:
                        inMemory = StockBarsOnMemoryMin.Exists(s => s.Symbol == Bars[0].Symbol);
                        table = "BaseMin";
                        break;
                    case BaseType.Minutes15:
                        inMemory = StockBarsOnMemoryMin15.Exists(s => s.Symbol == Bars[0].Symbol);
                        table = "BaseMin15";
                        break;
                }
                _connection.Open();
                SqlCommand cmd;
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                int countRows = 0;
                string strInsert = "INSERT INTO " + table + " (DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD) VALUES";
                foreach (BarData bar in Bars)
                {
                    //Normalize data:
                    switch ((BaseType)bar.BaseType)
                    {
                        case BaseType.Days:
                            bar.TradeDate = bar.TradeDate.Date;
                            bar.TradeDateTicks = bar.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes:
                            bar.TradeDate = bar.TradeDate.AddSeconds(-(bar.TradeDate.Second));
                            bar.TradeDateTicks = bar.TradeDate.Ticks;
                            break;
                        case BaseType.Minutes15:
                            bar.TradeDate = bar.TradeDate.AddSeconds(-(bar.TradeDate.Second));
                            bar.TradeDate = bar.TradeDate.AddMinutes(-(bar.TradeDate.Minute % 15));
                            bar.TradeDateTicks = bar.TradeDate.Ticks;
                            break;
                    }
                    //Data already exists?
                    cmd = new SqlCommand("SELECT DateTicks FROM " + table + " WHERE Symbol = '" + bar.Symbol + "' AND DateTicks = " + bar.TradeDateTicks, _connection);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    bool ignore = false;
                    while (rdr.Read())
                    {
                        ignore = true;
                    }
                    rdr.Close();
                    if (ignore || bar.TradeDate == DateTime.MinValue) continue;

                    //Insert less than 1000 rows:
                    if (countRows >= 950)
                    {
                        //Save data:
                        try
                        {
                            strInsert = strInsert.Remove(strInsert.Length - 1);
                            cmd = new SqlCommand(strInsert, _connection);
                            cmd.ExecuteNonQuery();
                            UpdateBaseEvent(new object(), new BarDataEventArgs(Bars.Last()));
                        }
                        catch (Exception) { }
                        strInsert = "INSERT INTO " + table + " (DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD) VALUES";
                        countRows = 0;
                    }
                    else
                    {
                        countRows++;
                        strInsert += " (" + bar.TradeDateTicks + ",'" + bar.TradeDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + bar.Symbol + "'," + bar.OpenPrice.ToString(nfi) + "," + bar.HighPrice.ToString(nfi) + "," + bar.LowPrice.ToString(nfi) + "," + bar.ClosePrice.ToString(nfi) + "," + bar.VolumeF.ToString(nfi) + "," + bar.VolumeS + "," + bar.VolumeT + "," + bar.AdjustS.ToString(nfi) + "," + bar.AdjustD.ToString(nfi) + "),";
                    }

                }
                if (countRows > 0)
                {
                    //Save data:
                    try
                    {
                        strInsert = strInsert.Remove(strInsert.Length - 1);
                        cmd = new SqlCommand(strInsert, _connection);
                        cmd.ExecuteNonQuery();
                        UpdateBaseEvent(new object(), new BarDataEventArgs(Bars.Last()));
                    }
                    catch (Exception) { }
                }

                _connection.Close();


            }
            catch (Exception ex)
            {
                // Ignore duplicate's error
                //if (ex.errorCode == 13) return true;
                MessageBox.Show("SaveBarData(bar) " + ex.Message);
                LogDB("SaveBarData(bat) " + ex.Message);
                _connection.Close();
                return false;
            }
            return true;
        }
        public static bool RemoveAllBarDatas(BaseType Base, SqlConnection _connection)
        {
            bool removed = false;
            string table = "BaseDay";
            if (Base == BaseType.Minutes) table = "BaseMin";
            else if (Base == BaseType.Minutes15) table = "BaseMin15";
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM " + table, _connection);
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
        public static bool RemoveBarDatas(string Symbol, BaseType Base, SqlConnection _connection)
        {
            bool removed = false;
            string table = "BaseDay";
            if (Base == BaseType.Minutes) table = "BaseMin";
            else if (Base == BaseType.Minutes15) table = "BaseMin15";
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE Symbol = '" + Symbol + "'", _connection);
                cmd.ExecuteNonQuery();
                LogDB("Removed all data for symbol " + Symbol);
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
                //List<BarData> All = GetBarDataAll(Symbol, 1, (int)Periodicity.Daily, _connection);
                /*
                 SELECT rh.user_name, rh.report_name, rh.report_run_date
                    FROM report_history rh,
                      (SELECT max(report_run_date) AS maxdate, report_name
                       FROM report_history
                       GROUP BY report_name) maxresults
                    WHERE rh.report_name = maxresults.report_name
                    AND rh.report_run_date= maxresults.maxdate;
                 */
                //SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM " + table + " WHERE Symbol = '" + Symbol + "' AND ", _connection);


                string table = "BaseDayView";
                if (Base == BaseType.Minutes15)
                {
                    table = "BaseMin15View";
                }
                else if (Base == BaseType.Minutes)
                {
                    table = "BaseMinView";
                }
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + table + " WHERE Symbol = '" + Symbol + "'", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    float openPrice = rdr.GetFloat(3);
                    float highPrice = rdr.GetFloat(4);
                    float lowPrice = rdr.GetFloat(5);
                    float closePrice = rdr.GetFloat(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    float adjustS = rdr.GetFloat(10);
                    float adjustD = rdr.GetFloat(11);

                    LastBar = new BarData()
                    {
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
                    };

                }
                _connection.Close();

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
                _connection.Close();
            }
            return LastBar;
        }
        public static List<BarData> GetLastBarDataAll(BaseType Base, SqlConnection _connection)
        {
            List<BarData> LastBar = new List<BarData>();//Get last data from extremeDB:

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
                //List<BarData> All = GetBarDataAll(Symbol, 1, (int)Periodicity.Daily, _connection);
                /*
                 SELECT rh.user_name, rh.report_name, rh.report_run_date
                    FROM report_history rh,
                      (SELECT max(report_run_date) AS maxdate, report_name
                       FROM report_history
                       GROUP BY report_name) maxresults
                    WHERE rh.report_name = maxresults.report_name
                    AND rh.report_run_date= maxresults.maxdate;
                 */
                //SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM " + table + " WHERE Symbol = '" + Symbol + "' AND ", _connection);


                string table = "BaseDayView";
                if (Base == BaseType.Minutes15)
                {
                    table = "BaseMin15View";
                }
                else if (Base == BaseType.Minutes)
                {
                    table = "BaseMinView";
                }
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + table, _connection);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    float openPrice = rdr.GetFloat(3);
                    float highPrice = rdr.GetFloat(4);
                    float lowPrice = rdr.GetFloat(5);
                    float closePrice = rdr.GetFloat(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    float adjustS = rdr.GetFloat(10);
                    float adjustD = rdr.GetFloat(11);

                    LastBar.Add(new BarData()
                    {
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
                _connection.Close();
            }
            return LastBar;
        }
        public static PortfolioDataSet GetPortfolioView(SqlConnection _connection, List<string> assetList, bool Memory = true)
        {
            PortfolioDataSet result = new PortfolioDataSet();
            Stopwatch watch = new Stopwatch();
            long timer2 = 0;
            int position = 0;
            watch.Start();

            if (Memory)
            {
                foreach (string Symbol in assetList)
                {
                    lock (PortfolioView)
                    {
                        if (PortfolioView.Assets.Exists(p => p.Symbol == Symbol))
                        {
                            result.Add(PortfolioView.Assets.First(p => p.Symbol == Symbol));
                            continue;
                        }
                    }

                }
            }
            try
            {
                //Using 6 months bars:
                _connection.Open();
                long ticks = DateTime.Now.AddMonths(-6).Ticks;
                SqlCommand cmd = new SqlCommand("SELECT * FROM BaseDay WHERE DateTicks>" + ticks + " ORDER BY Symbol , DateTicks", _connection);

                //Using All bars:
                //SqlCommand cmd = new SqlCommand("SELECT * FROM BaseDay", _connection);

                SqlDataReader rdr = cmd.ExecuteReader();
                List<BarData> bars = new List<BarData>();
                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    float openPrice = rdr.GetFloat(3);
                    float highPrice = rdr.GetFloat(4);
                    float lowPrice = rdr.GetFloat(5);
                    float closePrice = rdr.GetFloat(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    float adjustS = rdr.GetFloat(10);
                    float adjustD = rdr.GetFloat(11);

                    if (!result.Assets.Exists(a => a.Symbol == symbol))
                    {
                        bars.Add(new BarData()
                        {
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
                }
                timer2 = watch.ElapsedMilliseconds;
                foreach (string Symbol in assetList)
                {
                    if (result.Assets.Exists(a => a.Symbol == Symbol)) continue;
                    double[] volumes = new double[10];
                    string[] dates = new string[10];

                    List<BarData> barAll = bars.Where(b => b.Symbol == Symbol).ToList();


                    if (barAll.Count > 10)
                    {
                        for (int i = barAll.Count - 10; i < barAll.Count; i++)
                        {
                            volumes[i - (barAll.Count - 10)] = barAll[i].VolumeF;
                            dates[i - (barAll.Count - 10)] = barAll[i].TradeDate.ToString();
                        }
                    }
                    else if (barAll.Count > 0)
                    {
                        int j = barAll.Count();
                        for (int i = 0; i < 10 - j; i++)
                        {
                            volumes[i] = 0;
                            dates[i] = " ";
                        }
                        for (int i = 10 - j; i < 10; i++)
                        {
                            volumes[i] = barAll[i - 10 + j].VolumeF;
                            dates[i] = barAll[i - 10 + j].TradeDate.ToString();
                        }
                    }
                    SymbolSnapshot lastSnapshot = GetSnapshot(Symbol, BaseType.Days);
                    if (lastSnapshot != null)
                    {
                        volumes[9] = lastSnapshot.VolumeFinancial;
                        dates[9] = lastSnapshot.Timestamp.ToString();
                    }
                    if (barAll.Count() > 0)
                    {
                        result.Add(new RTAssetsInfo()
                        {
                            Close = barAll.Last().ClosePrice,
                            High = barAll.Last().HighPrice,
                            Last = barAll.Last().ClosePrice,
                            Low = barAll.Last().LowPrice,
                            Open = barAll.Last().OpenPrice,
                            Position = position,
                            Symbol = barAll.Last().Symbol,
                            Time = dates,
                            Trades = barAll.Last().VolumeT,
                            Variation =
                                barAll.Last().OpenPrice != 0
                                    ? (barAll.Last().ClosePrice - barAll.Last().OpenPrice) * 100 / barAll.Last().OpenPrice
                                    : 0,
                            Volume = volumes
                        });
                        lock (PortfolioView)
                        {
                            PortfolioView.Add(new RTAssetsInfo()
                            {
                                Close = barAll.Last().ClosePrice,
                                High = barAll.Last().HighPrice,
                                Last = barAll.Last().ClosePrice,
                                Low = barAll.Last().LowPrice,
                                Open = barAll.Last().OpenPrice,
                                Position = position,
                                Symbol = barAll.Last().Symbol,
                                Time = dates,
                                Trades = barAll.Last().VolumeT,
                                Variation =
                                    barAll.Last().OpenPrice != 0
                                        ? (barAll.Last().ClosePrice - barAll.Last().OpenPrice) * 100 / barAll.Last().OpenPrice
                                        : 0,
                                Volume = volumes
                            });
                        }
                    }
                    else //Select all bars for empty (old) symbols:
                    {                     

                        //Using All bars for symbol:
                        cmd = new SqlCommand("SELECT * FROM BaseDay WHERE Symbol='" + Symbol + "'", _connection);

                        rdr.Close();   
                        rdr = cmd.ExecuteReader();
                        barAll = new List<BarData>();
                        while (rdr.Read())
                        {
                            long dateTick = rdr.GetInt64(0);
                            DateTime date = rdr.GetDateTime(1);
                            string symbol = rdr.GetString(2);
                            float openPrice = rdr.GetFloat(3);
                            float highPrice = rdr.GetFloat(4);
                            float lowPrice = rdr.GetFloat(5);
                            float closePrice = rdr.GetFloat(6);
                            double volumeF = rdr.GetDouble(7);
                            long volumeS = rdr.GetInt64(8);
                            long volumeT = rdr.GetInt64(9);
                            float adjustS = rdr.GetFloat(10);
                            float adjustD = rdr.GetFloat(11);

                            barAll.Add(new BarData()
                            {
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

                        volumes = new double[10];
                        dates = new string[10];



                        if (barAll.Count > 10)
                        {
                            for (int i = barAll.Count - 10; i < barAll.Count; i++)
                            {
                                volumes[i - (barAll.Count - 10)] = barAll[i].VolumeF;
                                dates[i - (barAll.Count - 10)] = barAll[i].TradeDate.ToString();
                            }
                        }
                        else if (barAll.Count > 0)
                        {
                            int j = barAll.Count();
                            for (int i = 0; i < 10 - j; i++)
                            {
                                volumes[i] = 0;
                                dates[i] = " ";
                            }
                            for (int i = 10 - j; i < 10; i++)
                            {
                                volumes[i] = barAll[i - 10 + j].VolumeF;
                                dates[i] = barAll[i - 10 + j].TradeDate.ToString();
                            }
                        }
                        lastSnapshot = GetSnapshot(Symbol, BaseType.Days);
                        if (lastSnapshot != null)
                        {
                            volumes[9] = lastSnapshot.VolumeFinancial;
                            dates[9] = lastSnapshot.Timestamp.ToString();
                        }
                        if (barAll.Count() > 0)
                        {
                            result.Add(new RTAssetsInfo()
                            {
                                Close = barAll.Last().ClosePrice,
                                High = barAll.Last().HighPrice,
                                Last = barAll.Last().ClosePrice,
                                Low = barAll.Last().LowPrice,
                                Open = barAll.Last().OpenPrice,
                                Position = position,
                                Symbol = barAll.Last().Symbol,
                                Time = dates,
                                Trades = barAll.Last().VolumeT,
                                Variation =
                                    barAll.Last().OpenPrice != 0
                                        ? (barAll.Last().ClosePrice - barAll.Last().OpenPrice) * 100 / barAll.Last().OpenPrice
                                        : 0,
                                Volume = volumes
                            });
                            lock (PortfolioView)
                            {
                                PortfolioView.Add(new RTAssetsInfo()
                                {
                                    Close = barAll.Last().ClosePrice,
                                    High = barAll.Last().HighPrice,
                                    Last = barAll.Last().ClosePrice,
                                    Low = barAll.Last().LowPrice,
                                    Open = barAll.Last().OpenPrice,
                                    Position = position,
                                    Symbol = barAll.Last().Symbol,
                                    Time = dates,
                                    Trades = barAll.Last().VolumeT,
                                    Variation =
                                        barAll.Last().OpenPrice != 0
                                            ? (barAll.Last().ClosePrice - barAll.Last().OpenPrice) * 100 / barAll.Last().OpenPrice
                                            : 0,
                                    Volume = volumes
                                });
                            }
                        }
                        else
                        {
                            //Null values:
                            result.Add(new RTAssetsInfo()
                            {
                                Close = 0,
                                High = 0,
                                Last = 0,
                                Low = 0,
                                Open = 0,
                                Position = position,
                                Symbol = Symbol,
                                Time = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                                Trades = 0,
                                Variation = 0,
                                Volume = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                            });
                            PortfolioView.Add(new RTAssetsInfo()
                            {
                                Close = 0,
                                High = 0,
                                Last = 0,
                                Low = 0,
                                Open = 0,
                                Position = position,
                                Symbol = Symbol,
                                Time = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " },
                                Trades = 0,
                                Variation = 0,
                                Volume = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                            });
                        }
                    }
                    position++;
                }

                _connection.Close();



                //Using View: ERROR
                /*_connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM PortfolioView", _connection);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                timer1 = watch.ElapsedMilliseconds;
                List<BarData> bar10 = new List<BarData>();
                int position = 0;
                while (rdr.Read())
                {
                    if (bar10.Count() == 0 || rdr.GetString(2) == bar10.Last().Symbol)
                    {
                        bar10.Add(new BarData()
                        {
                            TradeDateTicks = rdr.GetInt64(0),
                            TradeDate = rdr.GetDateTime(1),
                            Symbol = rdr.GetString(2),
                            OpenPrice = rdr.GetFloat(3),
                            HighPrice = rdr.GetFloat(4),
                            LowPrice = rdr.GetFloat(5),
                            ClosePrice = rdr.GetFloat(6),
                            VolumeF = rdr.GetDouble(7),
                            VolumeS = rdr.GetInt64(8),
                            VolumeT = rdr.GetInt64(9),
                            AdjustS = rdr.GetFloat(10),
                            AdjustD = rdr.GetFloat(11)
                        });
                    }
                    else
                    {
                        if (bar10.Last().Symbol == "AMBV4")
                        {
                            bool Break = true;
                            Break = false;
                        }
                        double[] volumes = new double[]{0,0,0,0,0,0,0,0,0,0};
                        for(int i=0;i<bar10.Count();i++)
                        {
                            volumes[9-i] = bar10[i].VolumeF; 
                        }
                        result.Add(new RTAssetsInfo()
                        {
                            Close = bar10.First().ClosePrice,
                            High = bar10.First().HighPrice,
                            Last = bar10.First().ClosePrice,
                            Low = bar10.First().LowPrice,
                            Open = bar10.First().OpenPrice,
                            Position = position,
                            Symbol = bar10.First().Symbol,
                            Time = bar10.First().TradeDate.ToString(),
                            Trades = bar10.First().VolumeT,
                            Variation =
                                bar10.First().OpenPrice != 0
                                    ? (bar10.First().ClosePrice - bar10.First().OpenPrice) * 100 / bar10.First().OpenPrice
                                    : 0,
                            Volume = volumes,
                            //Photo = Properties.Resources.aalc11b
                        });
                        position++;
                        bar10 = new List<BarData>();
                    }
                }
                _connection.Close();*/
                //timer2 = watch.ElapsedMilliseconds - timer1;
                Console.WriteLine("\nTime Portfolio = " + watch.ElapsedMilliseconds + "ms (SQL=" + timer2 + "ms)");
            }
            catch (Exception ex)
            {
                _connection.Close();
                LogDB("GetPortfolioView() " + ex.Message);
            }
            return result;

        }

        public static bool TestDatabase(SqlConnection _connection)
        {
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM BaseDay WHERE DateTicks>635436576000000000 ORDER BY Symbol , DateTicks", _connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                List<BarData> bars = new List<BarData>();
                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    float openPrice = rdr.GetFloat(3);
                    float highPrice = rdr.GetFloat(4);
                    float lowPrice = rdr.GetFloat(5);
                    float closePrice = rdr.GetFloat(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    float adjustS = rdr.GetFloat(10);
                    float adjustD = rdr.GetFloat(11);

                    bars.Add(new BarData()
                    {
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
                Console.WriteLine("TEST-DB Time = " + timer.ElapsedMilliseconds + " ms.");
            }
            catch (Exception ex) { }
            return true;
        }
        public static List<BarData> GetBarDataAll(string Symbol, int Interval, int Periodicity, SqlConnection _connection)
        {
            List<BarData> All = new List<BarData>();
            bool addInMemory = false;
            BaseType Base = BaseType.Days;
            string table = "BaseDay";
            if (Periodicity == (int)Interface.Periodicity.Minutely)
            {
                if (Interval % 15 == 0)
                {
                    Base = BaseType.Minutes15;
                    table = "BaseMin15";
                }
                else
                {
                    Base = BaseType.Minutes;
                    table = "BaseMin";
                }
            }
            if (Periodicity == (int)Interface.Periodicity.Hourly)
            {
                Base = BaseType.Minutes15;
                table = "BaseMin15";
            }

            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM " + table + " WHERE Symbol = '" + Symbol + "' ORDER BY DateTicks ASC", _connection);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    long dateTick = rdr.GetInt64(0);
                    DateTime date = rdr.GetDateTime(1);
                    string symbol = rdr.GetString(2);
                    float openPrice = rdr.GetFloat(3);
                    float highPrice = rdr.GetFloat(4);
                    float lowPrice = rdr.GetFloat(5);
                    float closePrice = rdr.GetFloat(6);
                    double volumeF = rdr.GetDouble(7);
                    long volumeS = rdr.GetInt64(8);
                    long volumeT = rdr.GetInt64(9);
                    float adjustS = rdr.GetFloat(10);
                    float adjustD = rdr.GetFloat(11);

                    All.Add(new BarData()
                    {
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

                float high = BasicBars[i].HighPrice;
                float low = BasicBars[i].LowPrice;
                float open = BasicBars[i].OpenPrice;
                float close = BasicBars[i].ClosePrice;
                double volumeF = BasicBars[i].VolumeF;
                long volumeS = BasicBars[i].VolumeS;
                long volumeT = BasicBars[i].VolumeT;


                int intervalTemp = Interval;

                i++;

                if (i > BasicBars.Count - 1)
                {
                    /*NewBars.Add(new BarData()
                    {
                        Symbol = symbol,
                        TradeDate = date,
                        OpenPrice = open,
                        HighPrice = high,
                        LowPrice = low,
                        ClosePrice = close,
                        VolumeF = volumeF,
                        VolumeS = volumeS,
                        VolumeT = volumeT
                    });*/
                    break;
                }

                if (Period < Interface.Periodicity.Weekly)
                {
                    while ((i <= BasicBars.Count - 1) && (BasicBars[i].TradeDate < nextTimeReference))
                    {
                        close = BasicBars[i].ClosePrice;
                        low = (BasicBars[i].LowPrice < low) ? BasicBars[i].LowPrice : low;
                        high = (BasicBars[i].HighPrice > high) ? BasicBars[i].HighPrice : high;
                        volumeF += BasicBars[i].VolumeF;
                        volumeS += BasicBars[i].VolumeS;
                        volumeT += BasicBars[i].VolumeT;
                        i++;
                    }
                }
                else
                {
                    while ((intervalTemp > 0) && (i <= BasicBars.Count - 1))
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
                        volumeF += BasicBars[i].VolumeF;
                        volumeS += BasicBars[i].VolumeS;
                        volumeT += BasicBars[i].VolumeT;
                        i++;
                    }
                    if (i < BasicBars.Count()) timeReference = BasicBars[i].TradeDate;
                }
                NewBars.Add(new BarData()
                {
                    Symbol = symbol,
                    TradeDate = date,
                    OpenPrice = open,
                    HighPrice = high,
                    LowPrice = low,
                    ClosePrice = close,
                    VolumeF = volumeF,
                    VolumeS = volumeS,
                    VolumeT = volumeT
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
        public static string[] GetLastDates(string Symbol, int Interval, int Periodicity, SqlConnection _connection)
        {
            string[] result = new string[10];
            List<BarData> barAll = GetBarDataAll(Symbol, Interval, Periodicity, _connection);
            if (barAll.Count > 10)
            {
                for (int i = barAll.Count - 10; i < barAll.Count; i++)
                {
                    result[i - (barAll.Count - 10)] = barAll[i].TradeDate.ToString();
                }
            }
            else
            {
                int j = barAll.Count();
                for (int i = 0; i < 10 - j; i++)
                {
                    result[i] = " ";
                }
                for (int i = 10 - j; i < 10; i++)
                {
                    result[i] = barAll[i - 10 + j].TradeDate.ToString();
                }
            }
            SymbolSnapshot lastSnapshot = GetSnapshot(Symbol, BaseType.Days);
            if (lastSnapshot != null) result[10] = lastSnapshot.Timestamp.ToString();
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

        public static void WakeDatabase(SqlConnection _connection)
        {

            _connection.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM SymbolsSync", _connection);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            _connection.Close();
        }

    }

}
