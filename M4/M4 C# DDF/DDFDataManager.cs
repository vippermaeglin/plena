/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using M4.AsyncOperations;
using M4.M4v2.Base;
using M4.M4v2.Chart;
using M4.M4v2.Portfolio;
using M4.modulusfe.platform.data;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;

namespace M4
{
    public partial class DdfDataManager : DataManager
    {
        #region Members and Structs

        private readonly DdfLocalData _ddfLocalData;

        public StatusManager StatusManager { get; set; }

        public class SymbolCache
        {
            public string Symbol { get; set; }
            public double Price { get; set; }
            public SymbolCache(string symbol, double price)
            {
                Symbol = symbol;
                Price = price;
            }
        }
        
        private readonly CultureInfo _usCulture = new CultureInfo("en-US");

        private readonly AsyncOperation _asyncOp;

        public AsyncOperation AsyncOp
        {
            get { return _asyncOp; }
        }

        public class PriceUpdate
        {
            public DateTime TradeDateTime;
            public string Symbol;
            public double Price;
            public long Volume;
            public double Bid;
            public long BidSize;
            public double Ask;
            public long AskSize;
            public bool SystemWatch;
        }

        public frmMain2 MFrmMain2;

        private readonly List<BarData> _mData = new List<BarData>();
        private readonly List<string> _mSymbols = new List<string>();
        private readonly List<SymbolCache> _cache = new List<SymbolCache>();
        private readonly List<PriceUpdate> _mPriceUpdates = new List<PriceUpdate>();
        private readonly string _mUsername;
        private readonly string _mPassword;

        public DdfDataManager(string mUsername, string mPassword)
        {
            InitializeComponent();

            _mUsername = mUsername;
            _mPassword = mPassword;

            _asyncOp = AsyncHelper.CreateOperation();

            _ddfLocalData = new DdfLocalData();

        }

        public DdfDataManager()
        {
            InitializeComponent();

            _asyncOp = AsyncHelper.CreateOperation();

            _ddfLocalData = new DdfLocalData();
        }

        #endregion

        #region Login and Connection

        private static string OpenUrl(string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream ios = client.OpenRead(url);
                StreamReader reader = new StreamReader(ios);
                System.Text.StringBuilder ret = new System.Text.StringBuilder();
                while (!reader.EndOfStream)
                {
                    Application.DoEvents();
                    ret.Append(reader.ReadLine() + "\n");
                }
                return ret.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static IEnumerable<string> OpenUrlEx(string url)
        {
            WebClient client = new WebClient();
            Stream ios = client.OpenRead(url);
            StreamReader reader = new StreamReader(ios);
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }


        public override List<M4.DataServer.Interface.BarData> GetHistory(string symbol, IDataSubscriber subscriber, M4Core.Entities.Periodicity barType, int barSize, int barCount, string source,
          Action<HistoryRequestAnswer> onCompleted)
        {
            long timeEllapsed = MFrmMain2.measureTime.ElapsedMilliseconds;
            HistoryRequestAnswer answer = new HistoryRequestAnswer();
            try
            {
                _ddfLocalData.Periodicity = barType;
                _ddfLocalData.Source = source;
                _ddfLocalData.Symbol = symbol;
                _ddfLocalData.Interval = barSize;

                EPermission permission = Program.LoginAuthentication.VerifyFeature(EFeatures.HISTORICDATA); //ListFeatures.Instance().PermissionFeature(EFeatures.HISTORICDATA, EPermission.Master, Program.LoginAuthentication.Features);

                switch (permission)
                {
                    case EPermission.Permitido:
                        _ddfLocalData.History = barCount;
                        break;
                    case EPermission.Restringido:
                        _ddfLocalData.History = Properties.Settings.Default.MaxHistoryGuest;
                        break;
                    case EPermission.Negado:
                        _ddfLocalData.History = 0;
                        break;
                }

                if (barSize > 500 || barSize < 1)
                {
                    answer.HasError = true;
                    answer.ErrorMessage = "BarSize value out of range. Must be between 1..500";
                    onCompleted(answer);
                    return answer.Data;
                }

                answer.Data = new List<M4.DataServer.Interface.BarData>();
                bool first = true;
                /*_ddfLocalData.LoadStockDailyOrMinute();
                if(_ddfLocalData.Periodicity != Periodicity.Daily)
                {
                    answer.HasError = true;
                    answer.ErrorMessage = "Periodicity is different from Daily";
                    onCompleted(answer);
                    return;
                }*/
                if (_ddfLocalData.mFrmMain2 == null) _ddfLocalData.mFrmMain2 = MFrmMain2;
                _ddfLocalData.LoadStockDaily();

                if (_ddfLocalData.CandlesDaily.Count < 1)
                {
                    answer.HasError = true;
                    answer.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgCandlesMinimum"];
                    onCompleted(answer);
                    return answer.Data;
                }

                if (_ddfLocalData.Error)
                {
                    answer.HasError = true;
                    answer.ErrorMessage = "Arquivo de dados inconsistente.";
                }
                else answer.Data.AddRange(_ddfLocalData.CandlesDaily);



                if (!answer.HasError)
                {
                    //AsyncHelper.RunAsync(operation => 
                    _asyncOp.Post(() =>
                    {
                        //Automatically add the selection to the watch list
                        //GetLastPriceAndVolume(symbol);
                        if (!_mPriceUpdates.Any(_ => _.Symbol == symbol))
                        {
                            lock (_mPriceUpdates)
                            {
                                PriceUpdate pu = new PriceUpdate { Symbol = symbol, SystemWatch = false };

                                if (_mSymbols.IndexOf(symbol) == -1)
                                {
                                    _mSymbols.Add(symbol);
                                }

                                var lastRec = answer.Data.LastOrDefault();
                                if (lastRec == null)
                                {
                                    answer.HasError = true;
                                    answer.ErrorMessage = "Response contains no data. Either wrong symbol or your account has no access to it.";
                                    onCompleted(answer);
                                    return;
                                }

                                // Get the quote pipeline going to make timestamp on bars in even increments...
                                // Updated April 2010
                                m_LastHistoryTimeStamp = lastRec.TradeDate;
                                PrimeBars(new PrimeBar
                                {
                                    Symbol = symbol,
                                    BarType = barType,
                                    BarSize = barSize,
                                    Bar = lastRec
                                });

                                DdfWatch(symbol);

                                //tmrUpdate.Interval = 20000;  // FOR DEBUG
                                //tmrUpdate.Enabled = true; // FOR DEBUG

                                pu.Price = lastRec.ClosePrice;
                                pu.Volume = (long)lastRec.VolumeF;
                                pu.TradeDateTime = lastRec.TradeDate;

                                _mPriceUpdates.Add(pu);
                            }
                        }

                        AddWatch(symbol, subscriber, barType, barSize);
                        //DDFWatch(symbol);
                    });
                    //);
                }
            }
            catch (Exception ex)
            {
                answer.HasError = true;
                answer.ErrorMessage = ex.Message;
                return answer.Data;
            }
            Debug.Assert(onCompleted != null);

            if (answer.Data.Count > 0)
            {
                var last = answer.Data.Last();
                // Updated April 2010
                PrimeBars(new PrimeBar
                {
                    Symbol = symbol,
                    BarType = barType,
                    BarSize = barSize,
                    Bar = last
                });
                m_LastHistoryTimeStamp = last.TradeDate;
            }
            MFrmMain2.timeEllapsedDatabase = MFrmMain2.measureTime.ElapsedMilliseconds -
                                            MFrmMain2.timeEllapsedRequest;
            onCompleted(answer);
            Console.WriteLine("GETHISTORY() -> " + (MFrmMain2.measureTime.ElapsedMilliseconds - timeEllapsed));
            return answer.Data;
        }

        // Returns a list of BarData based on the symbol requested.
        // <param name="Symbol">Stock symbol</param>
        // <param name="Periodicity">Periodicity type, e.g. minute, daily, weekly</param>
        // <param name="BarSize">The size of each historic bar, e.g. 5 min, 15 min, 1 day</param>
        // <param name="BarCount">The number of historic bars to return, e.g. 50, 100, 250</param>
        // <remarks>Requesting too much data at once (thousands of bars), or specifying too large of a bar size (more than 60) may result in a very long delay.</remarks>
        // <returns>A list of bar data in the form of a <r>BarData</r> List.</returns>
        /*public override List<M4.DataServer.Interface.BarData> GetHistory(string symbol, IDataSubscriber subscriber, M4Core.Entities.Periodicity barType, int barSize, int barCount)
        {

            List<M4.DataServer.Interface.BarData> ret = new List<M4.DataServer.Interface.BarData>();

            if (DataFeed == null) return ret;
            if (_mUsername.Length == 0 || _mPassword.Length == 0) return ret;
            if (barSize > 500 || barSize < 1) return ret;

            //Request the historic data 
            string url = "http://ds01.ddfplus.com/historical/{0}.ashx?username={1}&password={2}&symbol={3}&maxrecords={4}&interval={5}&data={6}&format=csv&order=desc";

            string urlName = "queryminutes";
            string dataName = "minute";
            switch (barType)
            {
                case Periodicity.Minutely:
                    urlName = "queryminutes";
                    dataName = "minute";
                    break;
                case Periodicity.Hourly:
                    urlName = "queryminutes";
                    dataName = "minute";
                    barSize = 60 * barSize;
                    break;
                case Periodicity.Daily:
                    urlName = "queryeod";
                    dataName = "daily";
                    break;
                case Periodicity.Weekly:
                    urlName = "queryeod";
                    dataName = "weekly";
                    break;
                case Periodicity.Month:
                    urlName = "queryeod";
                    dataName = "monthly";
                    break;
                case Periodicity.Year:
                    urlName = "queryeod";
                    dataName = "yearly";
                    break;
            }

            //Format the url 
            url = String.Format(url, new object[] { urlName, _mUsername, _mPassword, symbol, barCount, barSize, dataName });
            if (barType > Periodicity.Daily) url = url.Replace("interval=1", "");

            //Download the history then split it into rows and columns 
            List<string> rows = new List<string>();
            try
            {
                rows.AddRange(OpenUrl(url).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            }
            catch
            {
                return ret;
            }

            if (rows.Count == 0) return ret;
            if (rows[0].IndexOf("Error") > -1) return ret;

            if (barType >= Periodicity.Daily)
            {
                // for (int n = 0; n < rows.Count; n++) // changed by ddf 1/1/2010
                for (int n = rows.Count - 1; n != -1; n--)
                {
                    Application.DoEvents();
                    M4.DataServer.Interface.BarData bar = new M4.DataServer.Interface.BarData();
                    string[] cols = rows[n].Split(',');
                    if (cols.Length > 6)
                    {
                        bar.TradeDate = DateTime.Parse(cols[1].Replace("-", " "), _usCulture.DateTimeFormat);
                        bar.OpenPrice = float.Parse(cols[2], _usCulture.NumberFormat);
                        bar.HighPrice = float.Parse(cols[3], _usCulture.NumberFormat);
                        bar.LowPrice = float.Parse(cols[4], _usCulture.NumberFormat);
                        bar.ClosePrice = float.Parse(cols[5], _usCulture.NumberFormat);
                        bar.VolumeF = double.Parse(cols[6], _usCulture.NumberFormat);
                        bar.VolumeS = long.Parse(cols[7], _usCulture.NumberFormat);
                        bar.VolumeT = long.Parse(cols[8], _usCulture.NumberFormat);
                        bar.AdjustD = float.Parse(cols[9], _usCulture.NumberFormat);
                        bar.AdjustS = float.Parse(cols[10], _usCulture.NumberFormat);
                    }
                    ret.Add(bar);
                }
            }

            else
            {
                for (int n = rows.Count - 1; n != -1; n--)
                {
                    Application.DoEvents();
                    M4.DataServer.Interface.BarData bar = new M4.DataServer.Interface.BarData();
                    string[] cols = rows[n].Split(',');
                    if (cols.Length > 6)
                    {
                        bar.TradeDate = DateTime.Parse(cols[0].Replace("-", " "), _usCulture.DateTimeFormat);
                        bar.OpenPrice = float.Parse(cols[2], _usCulture.NumberFormat);
                        bar.HighPrice = float.Parse(cols[3], _usCulture.NumberFormat);
                        bar.LowPrice = float.Parse(cols[4], _usCulture.NumberFormat);
                        bar.ClosePrice = float.Parse(cols[5], _usCulture.NumberFormat);
                        bar.VolumeF = double.Parse(cols[6], _usCulture.NumberFormat);
                        bar.VolumeS = long.Parse(cols[6], _usCulture.NumberFormat);
                        bar.VolumeT = long.Parse(cols[6], _usCulture.NumberFormat);
                        bar.AdjustD = float.Parse(cols[9], _usCulture.NumberFormat);
                        bar.AdjustS = float.Parse(cols[10], _usCulture.NumberFormat);
                    }
                    ret.Add(bar);
                }
            }

            // Get the quote pipeline going to make timestamp on bars in even increments...
            RealTimeUpdate(symbol, ret[ret.Count - 1].TradeDate, ret[ret.Count - 1].ClosePrice, 1);

            //Automatically add the selection to the watch list
            GetLastPriceAndVolume(symbol);
            AddWatch(symbol, subscriber, barType, barSize);
            DdfWatch(symbol);

            return ret;
        }*/

        //Returns TRUE if the Client ID, Username, Password and 
        //machine ID are authenticated by the web service.        
        private bool Authenticate(string clientId, string username, string password)
        {
            //##########################################################################
            //WARNING! Changing the code in this function is a violation of your license 
            //agreement and may result in fines and/or license revocation of M4!
            //##########################################################################

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            return new Service().Authenticate(clientId, username, password, new MachineInfo().GetMachineID());
        }
        #endregion

        #region Symbol Management
        //Returns the list of symbols that are loaded in the Level 1 grid
        public List<string> GetSymbols()
        {
            return _mSymbols;
        }

        //Helper function removes a symbol from the connection symbols string
        private void RemoveSymbol(string symbol)
        {
        }

        //Helper function adds a symbol to the connection symbols string
        private void AddSymbol(string symbol)
        {
        }

        //Loads symbols into the Level 1 grid via a list
        public void LoadSymbols(List<string> Symbols)
        {
            //Remove loaded symbols (if any)
            int count = _mSymbols.Count - 1;
            for (int n = 0; n <= count; n++)
            {
                //No harm if the symbol wasn't never loaded
                RemoveSymbol(_mSymbols[n]);
            }
            _mSymbols.Clear(); //Clear the local symbols list
            count = Symbols.Count - 1;
            for (int n = 0; n <= count; n++)
            {
                if (_mSymbols.IndexOf(Symbols[n]) == -1)
                {
                    _mSymbols.Add(Symbols[n]);
                }
                AddSymbol(Symbols[n]);
                Application.DoEvents(); // Required for DDF
            }
            //Now that we have loaded symbols on the level 1 grid,
            //enable the option to save the workspace on frmMain
            UpdateSaveWorkspaceEnabledStatus();

            Thread.Sleep(1000);
            DdfWatch(Symbols[0]); // DDF workaround
        }

        //Removes a symbol from the data feed API watch list if no other subscribers are watching it
        public bool RemoveSymbolWatch(string symbol, IDataSubscriber Subscriber)
        {
            if (RemoveWatch(symbol, Subscriber))
            {
                if (IsWatchedBySystem(symbol)) //DO NOT remove symbols that are watched by the order entry screen
                {
                    return true;
                }
                //Otherwise, remove it
                RemoveSymbol(symbol);
                int count = _mSymbols.Count - 1;
                for (int n = 0; n <= count; n++)
                {
                    if (_mSymbols[n] == symbol)
                    {
                        _mSymbols.RemoveAt(n);
                        return false;
                    }
                }
            }
            return false;
        }

        //Ensures that all L1 symbols are added to m_symbols()
        //NEVER call DDF1.Watch() to watch a level 1 symbol
        //from anyplace other than here!
        private void DdfWatch(string symbol)
        {
            RemoveSymbol(symbol);
            AddSymbol(symbol);
            if (_mSymbols.IndexOf(symbol) == -1)
            {
                _mSymbols.Add(symbol);
            }

            //Now that we have loaded symbols on the level 1 grid,
            //enable the option to save the workspace on frmMain
            UpdateSaveWorkspaceEnabledStatus();
        }

        private void UpdateSaveWorkspaceEnabledStatus()
        {
            bool enable = _mSymbols.Count > 0;
            //MFrmMain2.cmdSaveWorkspace.Enabled = enable;
            //MFrmMain2.mnuFileSaveWorkspace.Enabled = enable;
        }

        //Server timestamp for DataManager to run  to compare
        private void DataFeed_OnNewTimestamp(DateTime Stamp)
        {
            //MFrmMain.ShowStatus("Server time: " + Stamp);
            //return; // DEBUG
            // Updated April 2010
            // Remove this code if you want better performance but irregular intraday bars
            //if (m_Quote == null) return; // No updates yet

            // Update bar subscribers with previously cached price but 
            // with new timestamp, giving them a chance to close intraday bars
            //try
            //{
            //    RealTimeUpdate(m_Quote.Symbol, Stamp, Math.Round(m_Quote.Session.Last, 4), m_Quote.Session.Volume);
            //}
            //catch (Exception ex)
            //{
            //    OutputWindow1.DisplayAlertOrMessage(string.Format("DDF DataManager Realtime update exception. {0}", ex.Message),
            //                                        OutputWindow.OutputIcon.Warning);
            //}
        }

       
        //Update the symbol in m_priceUpdates so controls can get the last price/volume
        private void UpdateLastPrice(string symbol, double Price, long volume)
        {
            int count = _mPriceUpdates.Count - 1;
            for (int n = 0; n <= count; n++)
            {
                if (_mPriceUpdates[n].Symbol == symbol)
                {
                    _mPriceUpdates[n].Price = Price;
                    _mPriceUpdates[n].Volume = volume;
                }
            }
        }

        #endregion

        #region Misc (form resize, colors, etc.)

        private void DdfDataManagerLoad(object sender, EventArgs e)
        {
            //Login();  //Login to the data feed server
        }

        #endregion

        #region Charting Features

        ////Finds a real-time chart or loads it if not already loaded.
        //public ctlChart GetChart(ChartSelection Selection)
        //{
        //    return GetChart(Selection, false);
        //}

        //Finds a real-time chart or loads it if not already loaded.
        //public CtlPainelChart GetChartCtlPainelChart(ChartSelection Selection)
        //{
        //    return GetCtlPainelChart(Selection, false);
        //}

        //public ctlChart GetChart(ChartSelection Selection, bool CreateNew)
        //{
        //    ctlChart chart;
        //    //Is the chart already loaded?
        //    if (!CreateNew)
        //    {
        //        foreach (NUIDocument doc in MFrmMain.m_DockManager.DocumentManager.Documents)
        //        {
        //            if (doc.Client.Name != "ctlChart")
        //                continue;

        //            chart = (ctlChart)doc.Client;

        //            if ((((chart.m_Symbol == Selection.Symbol &&
        //                    (chart.m_Bars >= Selection.Bars)) &&
        //                    (chart.m_Periodicity == Selection.Periodicity)) &&
        //                    (chart.m_BarSize == Selection.Interval)))
        //            {
        //                return chart;
        //            }
        //        }
        //    }

        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(Selection.Symbol, Selection.Periodicity, Selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    //CtlPainelChart ctlPainelChart = new CtlPainelChart { Dock = DockStyle.Fill };
        //    //ctlPainelChart.CtlChart = new ctlChart(m_frmMain, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //    //                     Selection.Bars, false);

        //    //Create a new chart based on the user's selection
        //    chart = new ctlChart(MFrmMain, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //                         Selection.Bars, false);

        //    //If enough data was loaded, then display the chart using the Nevron document manager on frmMain
        //    if (chart.StockChartX1.RecordCount > 3)
        //    {
        //        chart.Dock = DockStyle.Fill;
        //        NUIDocument doc = new NUIDocument(title, -1, chart);
        //        MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //        MFrmMain.ShowStatus("");
        //        return chart;
        //    }
        //    //We failed to load the chart, so notify the user
        //    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //    MFrmMain.ShowStatus("Failed to load " + title);
        //    return null;
        //}

        //public CtlPainelChart GetCtlPainelChart(ChartSelection Selection, bool CreateNew)
        //{
        //    CtlPainelChart ctlPainelChart;
        //    //Is the chart already loaded?
        //    if (!CreateNew)
        //    {
        //        foreach (NUIDocument doc in MFrmMain.m_DockManager.DocumentManager.Documents)
        //        {
        //            if (doc.Client.Name != "CtlPainelChart")
        //                continue;

        //            ctlPainelChart = (CtlPainelChart)doc.Client;

        //            if ((((ctlPainelChart.MSymbol == Selection.Symbol &&
        //                    (ctlPainelChart.m_Bars >= Selection.Bars)) &&
        //                    (ctlPainelChart.m_Periodicity == Selection.Periodicity)) &&
        //                    (ctlPainelChart.m_BarSize == Selection.Interval)))
        //            {
        //                return ctlPainelChart;
        //            }
        //        }
        //    }

        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(Selection.Symbol, Selection.Periodicity, Selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    ctlPainelChart = new CtlPainelChart(MFrmMain, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //                         Selection.Bars, Selection.Source);

        //    //If enough data was loaded, then display the chart using the Nevron document manager on frmMain
        //    if ((ctlPainelChart != null) && (ctlPainelChart.StockChartX1.RecordCount > 3))
        //    {
        //        ctlPainelChart.Dock = DockStyle.Fill;
        //        NUIDocument doc = new NUIDocument(title, -1, ctlPainelChart);
        //        MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //        MFrmMain.ShowStatus("");
        //        return ctlPainelChart;
        //    }

        //    //We failed to load the chart, so notify the user
        //    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //    MFrmMain.ShowStatus("Failed to load " + title);
        //    return null;
        //}

        //public void LoadRealTimeChartAsync(ChartSelection selection, Action<ctlChart> onCompleted)
        //{
        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(selection.Symbol, selection.Periodicity, selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    //Create a new chart based on the user's selection
        //    ctlChart chart = new ctlChart(MFrmMain, this, selection.Symbol, selection.Periodicity, selection.Interval,
        //                                  selection.Bars, true);

        //    chart.InitRTChartAsync(b => _asyncOp.Post(() =>
        //    {
        //        Utils.Trace("Create a new tab - Chart");
        //        if (b)
        //        {
        //            chart.Dock = DockStyle.Fill;
        //            NUIDocument doc = new NUIDocument(title, -1, chart);
        //            MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //            MFrmMain.ShowStatus("");
        //            onCompleted(chart);
        //            chart.BindContextMenuEvents();
        //            return;
        //        }
        //        //We failed to load the chart, so notify the user
        //        Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //        MFrmMain.ShowStatus("Failed to load " + title);
        //        //OutputWindow1.DisplayAlertOrMessage("Failed to load " + title, OutputWindow.OutputIcon.System);
        //        onCompleted(null);
        //        return;
        //    }));
        //}



        //public void LoadRealTimeCtlPainelChartAsync(ChartSelection selection, Action<CtlPainelChart> onCompleted, bool schemeActive = false)
        //{
        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(selection.Symbol, selection.Periodicity, selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    CtlPainelChart ctlPainelChart = new CtlPainelChart(MFrmMain, this, selection.Symbol, selection.Periodicity, selection.Interval,
        //                                 selection.Bars, selection.Source, true) { Dock = DockStyle.Fill };

        //    if (schemeActive) ctlPainelChart.m_SchemeColor = MFrmMain.MActiveChart.m_SchemeColor;
        //    //CtlPainelChart2 chart2 = new CtlPainelChart2(m_frmMain, this, selection.Symbol, selection.Periodicity, selection.Interval,
        //    //                             selection.Bars, true) { Dock = DockStyle.Fill };

        //    ctlPainelChart.InitRTChartAsync(b => _asyncOp.Post(() =>
        //    {
        //        Console.WriteLine("Starting Post()");
        //        Utils.Trace("Create a new tab - Chart");
        //        if (b)
        //        {
        //            int docGroup = -1;
        //            for (int i = 0; i < MFrmMain.m_DockManager.DocumentManager.DocumentGroups.Length; i++)
        //            {
        //                foreach (Nevron.UI.WinForm.Docking.l1lI11I11l children in MFrmMain.m_DockManager.DocumentManager.DocumentGroups[i].Children)
        //                {
        //                    if (children.Document == MFrmMain.m_DockManager.DocumentManager.ActiveDocument) docGroup = i;
        //                }
        //            }
        //            NUIDocument doc = new NUIDocument(title, -1, ctlPainelChart);
        //            MFrmMain.m_DockManager.DocumentManager.AddDocument(doc, (docGroup != -1 ? MFrmMain.m_DockManager.DocumentManager.DocumentGroups[docGroup] : MFrmMain.m_DockManager.DocumentManager.DocumentGroups[0]));
        //            MFrmMain.ShowStatus("");
        //            onCompleted(ctlPainelChart);
        //            ctlPainelChart.BindContextMenuEvents();

        //            if (!string.IsNullOrEmpty(ctlPainelChart._nameTemplateDefault))
        //            {
        //                ctlPainelChart.StockChartX1.LoadGeneralTemplate(ListTemplates._path + ctlPainelChart._nameTemplateDefault + ".sct");
        //                ctlPainelChart.StockChartX1.Update();
        //            }
        //            if (selection.PriceStyle != null) ctlPainelChart.ChangePriceStyle(selection.PriceStyle);
        //            MFrmMain.MActiveChart = ctlPainelChart;
        //            MFrmMain.LoadChartSettings(MFrmMain.MActiveChart);
        //            MFrmMain.timeEllapsedLoading = MFrmMain.measureTime.ElapsedMilliseconds - MFrmMain.timeEllapsedDatabase -
        //                                           MFrmMain.timeEllapsedRequest;
        //            //Telerik.WinControls.RadMessageBox.Show("Time spent on request: " + MFrmMain2.timeEllapsedRequest + "\nTime spent getting data from database:" + MFrmMain2.timeEllapsedDatabase + "\nTime spent just accessing database:" + MFrmMain2.timeEllapsedDatabaseAccess + "\nTime spent on StockChart: " + MFrmMain2.timeEllapsedLoading + "\nTOTAL: " + (MFrmMain2.timeEllapsedRequest + MFrmMain2.timeEllapsedDatabase + MFrmMain2.timeEllapsedLoading));
        //            MFrmMain.measureTime.Stop();
        //            MFrmMain.measureTime.Reset();
        //            return;
        //        }

        //        if (ctlPainelChart.Answer.HasError)
        //        {
        //            Telerik.WinControls.RadMessageBox.Show(ctlPainelChart.Answer.ErrorMessage, Program.LanguageDefault.DictionaryTitleMessage["titleAnswerError"], MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //            return;
        //        }
        //        else
        //        {
        //            //We failed to load the chart, so notify the user
        //            Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //            MFrmMain.ShowStatus("Failed to load " + title);
        //            //OutputWindow1.DisplayAlertOrMessage("Failed to load " + title, OutputWindow.OutputIcon.System);
        //        }
        //        Console.WriteLine("Ending Post()");
        //        onCompleted(null);
        //        return;
        //    }));
        //    return;
        //}

        public void LoadRealTimeCtlPainelChartAsync2(ChartSelection selection, Action<CtlPainelChart> onCompleted)
        {
            //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
            string title = GetChartTitle(selection.Symbol, selection.Periodicity, selection.Interval);
            CtlPainelChart ctlPainelChart;
            if (MFrmMain2.ChartsList.IsEmpty())
            {
                //Instance CtlPanelChart from pool:
                ctlPainelChart = ObjectPool.New<CtlPainelChart>();
            }
            else
            {
                ctlPainelChart = MFrmMain2.ChartsList.TakeFromList();
            }
            ctlPainelChart.BlockUpdateStock = true;
            ctlPainelChart.m_StopLoadScroll = true;
            ctlPainelChart.LoadCtlPainelChart(MFrmMain2, (ctlData)this, selection.Symbol, selection.Periodicity, selection.Interval,
                                         selection.Bars, selection.Source, true);
            ctlPainelChart.StateUsed = true;




            //CtlPainelChart ctlPainelChart = new CtlPainelChart(MFrmMain2, this, selection.Symbol, selection.Periodicity, selection.Interval,
            //                           selection.Bars, selection.Source, true) { Dock = DockStyle.Fill };);
            //if (schemeActive && MFrmMain2.MActiveChart!=null) ctlPainelChart.m_SchemeColor = MFrmMain2.MActiveChart.m_SchemeColor;
            //CtlPainelChart2 chart2 = new CtlPainelChart2(m_frmMain, this, selection.Symbol, selection.Periodicity, selection.Interval,
            //                             selection.Bars, true) { Dock = DockStyle.Fill };
            ctlPainelChart.InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                Console.WriteLine("Starting Post()");
                Utils.Trace("Create a new tab - Chart");
                if (b)
                {
                    ctlPainelChart.BindContextMenuEvents();

                    /*if (!string.IsNullOrEmpty(ctlPainelChart._nameTemplateDefault))
                    {
                        ctlPainelChart.StockChartX1.LoadGeneralTemplate(ListTemplates._path + ctlPainelChart._nameTemplateDefault + ".sct");
                        ctlPainelChart.StockChartX1.Update();
                    }*/

                    if (selection.PriceStyle != null) ctlPainelChart.ChangePriceStyle(selection.PriceStyle);
                    MFrmMain2.MActiveChart = ctlPainelChart;
                    //ctlPainelChart.StockChartX1.Freeze(true);
                    MFrmMain2.LoadChartSettings(MFrmMain2.MActiveChart);
                    onCompleted(MFrmMain2.MActiveChart);
                    //ctlPainelChart.StockChartX1.Freeze(false);
                    ctlPainelChart.StockChartX1.ForcePaint();
                    MFrmMain2.timeEllapsedLoading = MFrmMain2.measureTime.ElapsedMilliseconds - MFrmMain2.timeEllapsedDatabase -
                                                  MFrmMain2.timeEllapsedRequest;
                    //Telerik.WinControls.RadMessageBox.Show("Time spent on request: " + MFrmMain2.timeEllapsedRequest + "\nTime spent getting data from database:" + MFrmMain2.timeEllapsedDatabase + "\nTime spent just accessing database:" + MFrmMain2.timeEllapsedDatabaseAccess + "\nTime spent on StockChart: " + MFrmMain2.timeEllapsedLoading + "\nTOTAL: " + (MFrmMain2.timeEllapsedRequest + MFrmMain2.timeEllapsedDatabase + MFrmMain2.timeEllapsedLoading));
                    MFrmMain2.measureTime.Stop();
                    MFrmMain2.measureTime.Reset();
                    ctlPainelChart.BlockUpdateStock = false;
                    //ctlPainelChart.StockChartX1.ForcePaint();
                    return;
                }

                if (ctlPainelChart.Answer.HasError)
                {
                    Telerik.WinControls.RadMessageBox.Show(ctlPainelChart.Answer.ErrorMessage, Program.LanguageDefault.DictionaryTitleMessage["titleAnswerError"], MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                    return;
                }
                else
                {
                    //We failed to load the chart, so notify the user
                    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                    // MFrmMain.ShowStatus("Failed to load " + title);
                    //OutputWindow1.DisplayAlertOrMessage("Failed to load " + title, OutputWindow.OutputIcon.System);
                }
                Console.WriteLine("Ending Post()");
                onCompleted(null);
                return;
            }));

            ctlPainelChart.LoadDataTemplate();

            MFrmMain2.CreateCtlPanelChart(title, ctlPainelChart);
            return;
        }

        ////Loads a real time chart
        //public ctlChart LoadRealTimeChart(ChartSelection Selection)
        //{
        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(Selection.Symbol, Selection.Periodicity, Selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    //Create a new chart based on the user's selection
        //    ctlChart chart = new ctlChart(MFrmMain, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //                                  Selection.Bars, false);

        //    //If enough data was loaded, then display the chart using the Nevron document manager on frmMain
        //    if (chart.StockChartX1.RecordCount > 3)
        //    {
        //        chart.Dock = DockStyle.Fill;
        //        NUIDocument doc = new NUIDocument(title, -1, chart);
        //        MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //        MFrmMain.ShowStatus("");
        //        return chart;
        //    }
        //    //We failed to load the chart, so notify the user
        //    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //    MFrmMain.ShowStatus("Failed to load " + title);
        //    return null;
        //}

        //Loads a real time chart
        //public CtlPainelChart LoadRealTimeCtlPainelChart(ChartSelection Selection)
        //{
        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(Selection.Symbol, Selection.Periodicity, Selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    //Create a new chart based on the user's selection
        //    CtlPainelChart chart = new CtlPainelChart(MFrmMain, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //                                  Selection.Bars, Selection.Source, false);

        //    //If enough data was loaded, then display the chart using the Nevron document manager on frmMain
        //    if (chart.StockChartX1.RecordCount > 3)
        //    {
        //        chart.Dock = DockStyle.Fill;
        //        NUIDocument doc = new NUIDocument(title, -1, chart);
        //        MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //        MFrmMain.ShowStatus("");
        //        return chart;
        //    }
        //    //We failed to load the chart, so notify the user
        //    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //    MFrmMain.ShowStatus("Failed to load " + title);
        //    return null;
        //}

        //Loads a real time chart
        //public CtlPainelChart LoadRealTimeCtlPainelChart(ChartSelection Selection)
        //{
        //    //Get the chart title (e.g. "MSFT 5 Min", "AAPL Daily", etc.)
        //    string title = GetChartTitle(Selection.Symbol, Selection.Periodicity, Selection.Interval);

        //    //Notify the user that we are requesting historic data...
        //    MFrmMain.ShowStatus("Requesting " + title + "...");

        //    //Create a new chart based on the user's selection
        //    CtlPainelChart chart = new CtlPainelChart(MFrmMain2, this, Selection.Symbol, Selection.Periodicity, Selection.Interval,
        //                                  Selection.Bars, Selection.Source, false);

        //    //If enough data was loaded, then display the chart using the Nevron document manager on frmMain
        //    if (chart.StockChartX1.RecordCount > 3)
        //    {
        //        chart.Dock = DockStyle.Fill;
        //        MFrmMain2.CreateCtlPanelChart(title, chart);
        //        //NUIDocument doc = new NUIDocument(title, -1, chart);
        //        //MFrmMain.m_DockManager.DocumentManager.AddDocument(doc);
        //        //MFrmMain2.ShowStatus("");
        //        return chart;
        //    }
        //    //We failed to load the chart, so notify the user
        //    Telerik.WinControls.RadMessageBox.Show("Failed to load " + title, "Timeout", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //    //MFrmMain.ShowStatus("Failed to load " + title);
        //    return null;
        //}

        public string GetChartTitle(string symbol, M4Core.Entities.Periodicity BarPeriodicity, int BarSize)
        {
            string title = symbol;
            switch (BarPeriodicity)
            {
                case M4Core.Entities.Periodicity.Secondly:
                    title += " " + BarSize + " Sec";
                    break;
                case M4Core.Entities.Periodicity.Minutely:
                    title += " " + BarSize + " Min";
                    break;
                case M4Core.Entities.Periodicity.Hourly:
                    title += " " + BarSize + " Hour";
                    break;
                case M4Core.Entities.Periodicity.Daily:
                    if (BarSize > 1)
                        title += " " + BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabDaily"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabDaily"];
                    break;
                case M4Core.Entities.Periodicity.Weekly:
                    if (BarSize > 1)
                        title += " " + BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabWeekly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabWeekly"];
                    break;
                case M4Core.Entities.Periodicity.Month:
                    if (BarSize > 1)
                        title += " " + BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabMonthly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabMonthly"];
                    break;
                case M4Core.Entities.Periodicity.Year:
                    if (BarSize > 1)
                        title += " " + BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabYearly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabYearly"];
                    break;
                default:
                    break;
            }
            return title;
        }
        #endregion

        #region Real Time Price Updates
        //Returns the last price/volume for a symbol.
        public PriceUpdate GetLastPriceAndVolume(string symbol)
        {
            return GetLastPriceAndVolume(symbol, false);
        }

        public PriceUpdate GetLastPriceAndVolume(string symbol, bool SystemWatch)
        {
            //Try to find the symbol in m_priceUpdates        
            int count = _mPriceUpdates.Count - 1;
            for (int n = 0; n <= count; n++)
            {
                if (_mPriceUpdates[n].Symbol == symbol)
                {
                    return _mPriceUpdates[n];
                }
            }

            //Add it if its not there
            PriceUpdate price = new PriceUpdate
            {
                Symbol = symbol,
                SystemWatch = SystemWatch,
                TradeDateTime = DateTime.Now,
            };
            _mPriceUpdates.Add(price);

            //Add the watch
            DdfWatch(symbol);
            if (_mSymbols.IndexOf(symbol) == -1)
            {
                _mSymbols.Add(symbol);
            }

            //Now wait until the data feed api returns a price
            //or until the the request times out
            DateTime timeout = DateTime.Now;
            bool timedout = false;
            while (price.Price == 0.0) //2 second timeout
            {
                Thread.Sleep(10);
                Application.DoEvents();
                if ((DateTime.Now - timeout).TotalSeconds > 2)
                {
                    timedout = true;
                    break;
                }
                if (price.Price != 0.0)
                {
                    break;
                }
            }

            price.Price = Math.Round(price.Price, 4);

            //Must be after market hours so get the last price from history
            if (timedout)
            {
                List<M4.DataServer.Interface.BarData> bars = GetHistory(symbol, null, Periodicity.Minutely, 1, 20, "Plena", answer => { }); //Min=20
                if ((bars != null) && (bars.Count > 0))
                {
                    price.Price = bars[bars.Count - 1].ClosePrice;
                    price.Volume = (long)Math.Round(bars[bars.Count - 1].VolumeF);
                    price.TradeDateTime = DateTime.Now;
                }
            }

            return price;
        }

        //Returns true if the symbol is being watched for last price updates
        private bool IsWatchedBySystem(string symbol)
        {
            int count = _mPriceUpdates.Count - 1;
            for (int n = 0; n <= count; n++)
            {
                if (_mPriceUpdates[n].Symbol == symbol)
                {
                    return _mPriceUpdates[n].SystemWatch;
                }
            }
            return false;
        }
        #endregion

        #region Historic Data Requests
        //DDF data is sent in TCP/IP packets so it needs to be sorted after it is returned
        //Also removes bars where there is no volume
        public void SortBars()
        {
            int n;
            int bars = 0;
            for (n = 0; n <= _mData.Count; n++)
            {
                if (_mData[n].ClosePrice > 0.0)
                {
                    bars++;
                }
            }
            BarData[] Temp = new BarData[bars + 1];
            DateTime max = new DateTime(2500, 1, 1);
            for (n = 0; n <= bars - 1; n++)
            {
                if (_mData[n].ClosePrice > 0.0)
                {
                    Temp[n] = new BarData
                    {
                        TradeDate = _mData[n].TradeDate,
                        OpenPrice = _mData[n].OpenPrice,
                        HighPrice = _mData[n].HighPrice,
                        LowPrice = _mData[n].LowPrice,
                        ClosePrice = _mData[n].ClosePrice,
                        Volume = _mData[n].Volume
                    };
                }
            }
            int volumeCount = 0;
            for (n = 0; n < _mData.Count; n++)
            {
                int minn = 0;
                if (_mData[n].Volume > 0.0)
                {
                    volumeCount++;
                }
                DateTime min = max;
                for (int j = 0; j < _mData.Count; j++)
                {
                    if ((DateTime.Compare(Temp[j].TradeDate, min) < 0) &
                        (DateTime.Compare(Temp[j].TradeDate, Convert.ToDateTime("12:00:00 AM")) != 0))
                    {
                        min = Temp[j].TradeDate;
                        minn = j;
                    }
                }
                _mData[n] = Temp[minn];
                Temp[minn].TradeDate = new DateTime();
            }
            Temp = new BarData[(volumeCount - 1) + 1];
            int cnt = 0;
            for (n = 0; n <= bars - 1; n++)
            {
                if (_mData[n].Volume > 0.0)
                {
                    Temp[cnt] = new BarData
                    {
                        TradeDate = _mData[n].TradeDate,
                        OpenPrice = _mData[n].OpenPrice,
                        HighPrice = _mData[n].HighPrice,
                        LowPrice = _mData[n].LowPrice,
                        ClosePrice = _mData[n].ClosePrice,
                        Volume = _mData[n].Volume
                    };
                    cnt++;
                }
            }
            _mData.Clear();
            _mData.AddRange(Temp);
        }

        // Plena M4 use this:
        public void GetHistoryLocal(string symbol, IDataSubscriber subscriber, M4Core.Entities.Periodicity barType, int barSize, int barCount, string source,
          Action<HistoryRequestAnswer> onCompleted)
        {
            AsyncHelper.RunAsync(
              asyncOp =>
              {
                  long timeEllapsed = MFrmMain2.measureTime.ElapsedMilliseconds;
                  HistoryRequestAnswer answer = new HistoryRequestAnswer();
                  try
                  {
                      _ddfLocalData.Periodicity = barType;
                      _ddfLocalData.Source = source;
                      _ddfLocalData.Symbol = symbol;
                      _ddfLocalData.Interval = barSize;

                      EPermission permission = Program.LoginAuthentication.VerifyFeature(EFeatures.HISTORICDATA); //ListFeatures.Instance().PermissionFeature(EFeatures.HISTORICDATA, EPermission.Master, Program.LoginAuthentication.Features);

                      switch (permission)
                      {
                          case EPermission.Permitido:
                              _ddfLocalData.History = barCount;
                              break;
                          case EPermission.Restringido:
                              _ddfLocalData.History = Properties.Settings.Default.MaxHistoryGuest;
                              break;
                          case EPermission.Negado:
                              _ddfLocalData.History = 0;
                              break;
                      }

                      if (barSize > 500 || barSize < 1)
                      {
                          answer.HasError = true;
                          answer.ErrorMessage = "BarSize value out of range. Must be between 1..500";
                          onCompleted(answer);
                          return;
                      }

                      answer.Data = new List<M4.DataServer.Interface.BarData>();
                      bool first = true;
                      /*_ddfLocalData.LoadStockDailyOrMinute();
                      if(_ddfLocalData.Periodicity != Periodicity.Daily)
                      {
                          answer.HasError = true;
                          answer.ErrorMessage = "Periodicity is different from Daily";
                          onCompleted(answer);
                          return;
                      }*/
                      if (_ddfLocalData.mFrmMain2 == null) _ddfLocalData.mFrmMain2 = MFrmMain2;
                      _ddfLocalData.LoadStockDaily();

                      if (_ddfLocalData.CandlesDaily.Count < 1)
                      {
                          answer.HasError = true;
                          answer.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgCandlesMinimum"];
                          onCompleted(answer);
                          return;
                      }

                      if (_ddfLocalData.Error)
                      {
                          answer.HasError = true;
                          answer.ErrorMessage = "Arquivo de dados inconsistente.";
                      }
                      else answer.Data.AddRange(_ddfLocalData.CandlesDaily);

                      /*
                      foreach (DataServer.Interface.BarData candle in _ddfLocalData.CandlesDaily)
                      {
                          if (first)
                          {
                              first = false;
                              if (_ddfLocalData.Error)
                              {
                                  answer.HasError = true;
                                  answer.ErrorMessage = "Arquivo de dados inconsistente.";
                                  break;
                              }
                          }

                          BarData bar = new BarData
                                            {
                                                TradeDate = candle.TradeDate,
                                                OpenPrice = candle.OpenPrice,
                                                HighPrice = candle.HighPrice,
                                                LowPrice = candle.LowPrice,
                                                ClosePrice = candle.ClosePrice,
                                                Volume = candle.Volume/1000000
                                            };

                          answer.Data.Add(bar);
                      });*/

                      if (!answer.HasError)
                      {
                          //AsyncHelper.RunAsync(operation => 
                          _asyncOp.Post(() =>
                          {
                              //Automatically add the selection to the watch list
                              //GetLastPriceAndVolume(symbol);
                              if (!_mPriceUpdates.Any(_ => _.Symbol == symbol))
                              {
                                  lock (_mPriceUpdates)
                                  {
                                      PriceUpdate pu = new PriceUpdate { Symbol = symbol, SystemWatch = false };

                                      if (_mSymbols.IndexOf(symbol) == -1)
                                      {
                                          _mSymbols.Add(symbol);
                                      }

                                      var lastRec = answer.Data.LastOrDefault();
                                      if (lastRec == null)
                                      {
                                          answer.HasError = true;
                                          answer.ErrorMessage = "Response contains no data. Either wrong symbol or your account has no access to it.";
                                          onCompleted(answer);
                                          return;
                                      }

                                      // Get the quote pipeline going to make timestamp on bars in even increments...
                                      // Updated April 2010
                                      m_LastHistoryTimeStamp = lastRec.TradeDate;
                                      PrimeBars(new PrimeBar
                                      {
                                          Symbol = symbol,
                                          BarType = barType,
                                          BarSize = barSize,
                                          Bar = lastRec
                                      });

                                      DdfWatch(symbol);

                                      //tmrUpdate.Interval = 20000;  // FOR DEBUG
                                      //tmrUpdate.Enabled = true; // FOR DEBUG

                                      pu.Price = lastRec.ClosePrice;
                                      pu.Volume = (long)lastRec.VolumeF;
                                      pu.TradeDateTime = lastRec.TradeDate;

                                      _mPriceUpdates.Add(pu);
                                  }
                              }

                              AddWatch(symbol, subscriber, barType, barSize);
                              //DDFWatch(symbol);
                          });
                          //);
                      }
                  }
                  catch (Exception ex)
                  {
                      answer.HasError = true;
                      answer.ErrorMessage = ex.Message;
                  }
                  Debug.Assert(onCompleted != null);

                  if (answer.Data.Count > 0)
                  {
                      var last = answer.Data.Last();
                      // Updated April 2010
                      PrimeBars(new PrimeBar
                      {
                          Symbol = symbol,
                          BarType = barType,
                          BarSize = barSize,
                          Bar = last
                      });
                      m_LastHistoryTimeStamp = last.TradeDate;
                  }
                  MFrmMain2.timeEllapsedDatabase = MFrmMain2.measureTime.ElapsedMilliseconds -
                                                  MFrmMain2.timeEllapsedRequest;
                  onCompleted(answer);
                  Console.WriteLine("GETHISTORYLOCAL() -> " + (MFrmMain2.measureTime.ElapsedMilliseconds - timeEllapsed));
              });
        }

        //Returns TRUE if the previous price for a symbol is the same as the current price.
        //Also saves the current price.
        private bool Cached(string symbol, double price)
        {
            for (int n = 0; n <= _cache.Count - 1; n++)
            {
                if (_cache[n].Symbol == symbol)
                {
                    bool ret = _cache[n].Price == price;
                    _cache[n].Price = price;
                    return ret;
                }
            }
            _cache.Add(new SymbolCache(symbol, price));
            return false;
        }

        #endregion

        #region DDF Plus Events

        private void DataFeed_OnAPIError(long errorCode)
        {
            //StatusManager.SetMessage("Data Feed API Error Code " + errorCode, OutputWindowV2.OutputIcon.Warning);
        }

        private void DataFeed_OnConnectionStatus(string description)
        {
            //StatusManager.SetMessage(description, OutputWindowV2.OutputIcon.Info);
        }

        private void DataFeed_OnNewMessage(string message)
        {
            //StatusManager.SetMessage(message, OutputWindowV2.OutputIcon.Info);
        }
        #endregion
    }

}

class M4DataGridView : DataGridView
{
    public M4DataGridView()
    {
        DoubleBuffered = true;
    }
}
