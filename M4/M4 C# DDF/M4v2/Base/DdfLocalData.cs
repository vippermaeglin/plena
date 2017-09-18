using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using M4.DataServer.Interface;
using Periodicity = M4Core.Entities.Periodicity;
using System.Data.SqlClient;

namespace M4.M4v2.Base
{
    public class DdfLocalData
    {
        public frmMain2 mFrmMain2;
        public string Source { get; set; }
        public string Symbol { get; set; }
        public Periodicity Periodicity { get; set; }
        public int Interval { get; set; }
        public int History { get; set; }

        public List<RegCandle> Candles { get; private set; }
        public List<RegCandle> CandlesBasic { get; private set; }
        public List<RegCandle> CandlesHistory { get; private set; }
        public List<BarData> CandlesDaily { get; private set; }

        public bool Error { get; set; }
        public string MessageErro { get; set; }

        public int NumReg { get; set; }

        public DdfLocalData()
        {
            Candles = new List<RegCandle>();
            CandlesBasic = new List<RegCandle>();
            CandlesHistory = new List<RegCandle>();
        }

        public void GetHistoryData()
        {
            LoadStockDailyOrMinute();
        }

        private string LoadPathFileCsv()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\";

            if ((Periodicity == Periodicity.Minutely) || (Periodicity == Periodicity.Hourly))
                path += "MINUTE\\" + Symbol + ".csv";
            else
                path += "DAILY\\" + Symbol + ".csv";

            if (!File.Exists(path))
                throw new Exception(Program.LanguageDefault.DictionaryMessage["msgErrLoadStockLocalData"]);

            return path;
        }

        public void LoadStockDailyOrMinute()
        {
            Candles.Clear();
            CandlesBasic.Clear();
            CandlesHistory.Clear();

            try
            {
                string fName = LoadPathFileCsv();

                bool bDate1 = false;
                bool bDate2 = false;
                bool fileOrderDescending;

                DateTime date1 = DateTime.Today;
                DateTime date2 = DateTime.Today;
                string[] time1 = null;
                string[] time2 = null;
                int candleInfoIndex;

                StreamReader stream = new StreamReader(fName);
                string row;
                bool goodSoFar = false;

                while ((row = stream.ReadLine()) != null)
                {
                    string[] splitRow = row.Split(';');

                    if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                        continue;

                    if (!bDate1)
                    {
                        DateTime.TryParse(splitRow[1], out date1);
                        bDate1 = true;
                        time1 = splitRow[2].Split(':');
                        continue;
                    }

                    if (!bDate2)
                    {
                        DateTime.TryParse(splitRow[1], out date2);
                        bDate2 = true;
                        time2 = splitRow[2].Split(':');
                        goodSoFar = true;
                        stream.Close();
                        break;
                    }
                }

                if (!goodSoFar)
                    throw new Exception("Invalid CSV File - 1 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).");

                if (time1.Length == 3 && time2.Length == 3)
                {
                    candleInfoIndex = 3;

                    DateTime date1Tmp = new DateTime(date1.Year, date1.Month, date1.Day,
                                               Convert.ToInt32(time1[0]), Convert.ToInt32(time1[1]), Convert.ToInt32(time1[2]));
                    DateTime date2Tmp = new DateTime(date2.Year, date2.Month, date2.Day,
                                               Convert.ToInt32(time2[0]), Convert.ToInt32(time2[1]), Convert.ToInt32(time2[2]));

                    fileOrderDescending = (date1Tmp > date2Tmp) ? false : true;
                }
                else if (time1.Length == 1 && time2.Length == 1)
                {
                    candleInfoIndex = 2;
                    fileOrderDescending = (date1 > date2) ? false : true;
                }
                else
                    throw new Exception("Invalid CSV File" + " 2 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).");

                int hr;
                int mn;
                int sc;

                stream = new StreamReader(fName);

                List<string> listRowCollection = new List<string>();

                while ((row = stream.ReadLine()) != null)
                    listRowCollection.Add(row);

                stream.Close();

                if (fileOrderDescending)
                {
                    foreach (string t in listRowCollection)
                    {
                        string[] splitRow = t.Split(';');

                        if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                            continue;

                        DateTime dt;
                        if (!DateTime.TryParseExact(splitRow[1], "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
                            continue;

                        hr = dt.Hour;
                        mn = dt.Minute;
                        sc = dt.Second;
                        if (dt.Hour == 0)
                        {
                            hr = 0;
                            mn = 0;
                            sc = 0;
                        }

                        if (candleInfoIndex == 3)
                        {
                            string[] strTime = splitRow[2].Split(':');
                            if (strTime.Length == 3)
                            {
                                hr = Convert.ToInt16(strTime[0]);
                                mn = Convert.ToInt16(strTime[1]);
                                sc = Convert.ToInt16(strTime[2]);
                            }
                        }

                        CandlesBasic.Add(new RegCandle
                        {
                            Date = new DateTime(dt.Year, dt.Month, dt.Day, hr, mn, sc),
                            Open = Convert.ToDouble(splitRow[candleInfoIndex]),
                            High = Convert.ToDouble(splitRow[candleInfoIndex + 1]),
                            Low = Convert.ToDouble(splitRow[candleInfoIndex + 2]),
                            Close = Convert.ToDouble(splitRow[candleInfoIndex + 3]),
                            Volume = Convert.ToDouble(splitRow[candleInfoIndex + 4])
                        });
                    }
                }
                else
                {
                    for (int i = listRowCollection.Count - 1; i >= 0; i--)
                    {
                        string[] splitRow = listRowCollection[i].Split(';');

                        if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                            continue;

                        DateTime dt;
                        if (!DateTime.TryParseExact(splitRow[1], "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
                            continue;

                        hr = dt.Hour;
                        mn = dt.Minute;
                        sc = dt.Second;

                        if (dt.Hour == 0)
                        {
                            hr = 0;
                            mn = 0;
                            sc = 0;
                        }

                        if (candleInfoIndex == 3)
                        {
                            string[] strTime = splitRow[2].Split(':');
                            if (strTime.Length == 3)
                            {
                                hr = Convert.ToInt16(strTime[0]);
                                mn = Convert.ToInt16(strTime[1]);
                                sc = Convert.ToInt16(strTime[2]);
                            }
                        }

                        CandlesBasic.Add(new RegCandle
                        {
                            Date = new DateTime(dt.Year, dt.Month, dt.Day, hr, mn, sc),
                            Open = Convert.ToDouble(splitRow[candleInfoIndex]),
                            High = Convert.ToDouble(splitRow[candleInfoIndex + 1]),
                            Low = Convert.ToDouble(splitRow[candleInfoIndex + 2]),
                            Close = Convert.ToDouble(splitRow[candleInfoIndex + 3]),
                            Volume = Convert.ToDouble(splitRow[candleInfoIndex + 4])
                        });
                    }
                }

                if (((Periodicity == Periodicity.Daily) || ((Periodicity == Periodicity.Minutely))) &&
                   (Interval == 1))
                    Candles.AddRange(CandlesBasic);
                else
                    Transform();

                CandlesHistory.AddRange((Candles.Count - History) > 0
                                            ? Candles.GetRange(Candles.Count - History, History)
                                            : Candles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LoadStockDaily()
        {
            if(!mFrmMain2.measureTime.IsRunning) mFrmMain2.measureTime.Start();
            long timeEllapsed = mFrmMain2.measureTime.ElapsedMilliseconds;
            Candles.Clear();
            CandlesBasic.Clear();
            CandlesHistory.Clear();
            mFrmMain2.timeEllapsedDatabaseAccess = mFrmMain2.measureTime.ElapsedMilliseconds;

            try
            {
                SqlConnection _connection = DBlocalSQL.Connect();
                CandlesDaily = DBlocalSQL.GetBarDataAll(Symbol,Interval,(int)Periodicity,_connection);
                DBlocalSQL.Disconnect(_connection);
                int size = Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") ? Properties.Settings.Default.History > 300 ? 300 : Properties.Settings.Default.History : Properties.Settings.Default.History;
                if (CandlesDaily.Count > size)
                {
                    CandlesDaily = CandlesDaily.GetRange(CandlesDaily.Count-size,size);
                }


                //Get all data from localDB
                /*string conString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=\"C:\\Users\\Admin\\Desktop\\PLENA\\M4\\trunk\\M4 C# DDF\\PlenaData.mdf\";Integrated Security=True";
                SqlConnection localdbCon = new SqlConnection(conString);
                localdbCon.Open();
                SqlCommand cmd = new SqlCommand("SELECT DateTicks, Date, Symbol, OpenPrice, HighPrice, LowPrice, ClosePrice, VolumeF, VolumeS, VolumeT, AdjustS, AdjustD FROM BaseDay WHERE Symbol = N'" + Symbol + "'", localdbCon);
                SqlDataReader rdr = cmd.ExecuteReader();
                CandlesDaily = new List<BarData>();
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

                    CandlesDaily.Add(new BarData() {
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

                    // Do somthing with this rows string, for example to put them in to a list
                }*/

                mFrmMain2.timeEllapsedDatabaseAccess = mFrmMain2.measureTime.ElapsedMilliseconds - mFrmMain2.timeEllapsedDatabaseAccess;
                Console.WriteLine("DBDAILYSHARED() -> " + mFrmMain2.timeEllapsedDatabaseAccess);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message+CandlesDaily.Count);
            }
        }

        public int DayOfWeek(DayOfWeek dayOfWeek)
        {
            //Indica Domingo
            if (dayOfWeek == System.DayOfWeek.Sunday)
                return 0;
            //Indica Segunda
            else if (dayOfWeek == System.DayOfWeek.Monday)
                return 1;
            //Indica Terça
            else if (dayOfWeek == System.DayOfWeek.Tuesday)
                return 2;
            //Indica Quarta
            else if (dayOfWeek == System.DayOfWeek.Wednesday)
                return 3;
            //Indica Quinta
            else if (dayOfWeek == System.DayOfWeek.Thursday)
                return 4;
            //Indica Sexta
            else if (dayOfWeek == System.DayOfWeek.Friday)
                return 5;

            //Indica Sábado
            return 6;
        }

        public void Transform()
        {
            try
            {
                switch (Periodicity)
                {
                    case Periodicity.Minutely:
                        ProcessingMinutely();
                        break;
                    case Periodicity.Hourly:
                        Interval = Interval * 60;
                        ProcessingMinutely();
                        break;
                    case Periodicity.Daily:
                        break;
                    case Periodicity.Weekly:
                    case Periodicity.Month:
                    case Periodicity.Year:
                        Processing();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Program.LanguageDefault.DictionaryMessage["msgGenerateDataWeekly"] + ex);
            }
        }

        /// <summary>
        /// Processo de transformação dos dados para periodicidade e intervalo solicitado
        /// </summary>
        private void Processing()
        {
            int i = 0;

            while (i < CandlesBasic.Count)
            {
                DateTime date = CandlesBasic[i].Date;
                double high = CandlesBasic[i].High;
                double low = CandlesBasic[i].Low;
                double open = CandlesBasic[i].Open;
                double volume = CandlesBasic[i].Volume;

                int intervalTemp = Interval;

                while ((intervalTemp > 0) && (i < CandlesBasic.Count - 1))
                {
                    switch (Periodicity)
                    {
                        case Periodicity.Weekly:
                            if (DayOfWeek(CandlesBasic[i + 1].Date.DayOfWeek) < DayOfWeek(CandlesBasic[i].Date.DayOfWeek))
                                intervalTemp--;
                            break;
                        case Periodicity.Month:
                            if (CandlesBasic[i + 1].Date.Month > CandlesBasic[i].Date.Month)
                                intervalTemp--;
                            break;
                        case Periodicity.Year:
                            if (CandlesBasic[i + 1].Date.Year > CandlesBasic[i].Date.Year)
                                intervalTemp--;
                            break;
                    }

                    if (intervalTemp <= 0)
                        continue;

                    low = (CandlesBasic[i].Low < low) ? CandlesBasic[i].Low : low;
                    high = (CandlesBasic[i].High > high) ? CandlesBasic[i].High : high;
                    volume += CandlesBasic[i].Volume;
                    i++;
                }

                double close = CandlesBasic[i].Close;
                volume += CandlesBasic[i].Volume;
                low = (CandlesBasic[i].Low < low) ? CandlesBasic[i].Low : low;
                high = (CandlesBasic[i].High > high) ? CandlesBasic[i].High : high;
                i++;

                Candles.Add(new RegCandle
                {
                    Date = date,
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = volume
                });
            }
        }

        private void ProcessingMinutely()
        {
            int i = 0;
            DateTime testDate =DateTime.Parse("01/01/1980");



            while (i < CandlesBasic.Count)
            {
                DateTime checkPoint = new DateTime(CandlesBasic[i].Date.Year, CandlesBasic[i].Date.Month, CandlesBasic[i].Date.Day, CandlesBasic[i].Date.Hour, 0, 0).AddMinutes(Interval);
                //DateTime checkPoint = new DateTime(testDate.Date.Year, testDate.Date.Month, testDate.Date.Day, testDate.Date.Hour, 0, 0).AddMinutes(Interval);
                while ((new DateTime(CandlesBasic[i].Date.Year, CandlesBasic[i].Date.Month, CandlesBasic[i].Date.Day)) ==
                    (new DateTime(checkPoint.Year, checkPoint.Month, checkPoint.Day)))
                {
                    DateTime date = CandlesBasic[i].Date;
                    double high = CandlesBasic[i].High;
                    double low = CandlesBasic[i].Low;
                    double open = CandlesBasic[i].Open;
                    double volume = CandlesBasic[i].Volume;
                    double close = CandlesBasic[i].Close;

                    int numElements = 0;

                    while (CandlesBasic[i].Date < checkPoint)
                    {
                        low = (CandlesBasic[i].Low < low) ? CandlesBasic[i].Low : low;
                        high = (CandlesBasic[i].High > high) ? CandlesBasic[i].High : high;
                        volume += CandlesBasic[i].Volume;
                        close = CandlesBasic[i].Close;
                        i++;
                        numElements++;

                        if (i < CandlesBasic.Count)
                            continue;

                        break;
                    }

                    if (numElements > 0)
                    {
                        Candles.Add(new RegCandle
                        {
                            Date = date,
                            Open = open,
                            High = high,
                            Low = low,
                            Close = close,
                            Volume = volume
                        });
                    }

                    checkPoint = checkPoint.AddMinutes(Interval);

                    if (i >= CandlesBasic.Count)
                        break;
                }
            }
        }
    }
}
