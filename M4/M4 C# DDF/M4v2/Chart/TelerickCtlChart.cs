using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using M4.AsyncOperations;
using M4Core.Entities;
using ModulusFE.APR;
using oAuth;
using STOCKCHARTXLib;
using System.Web.UI.WebControls;
using TradeScriptLib;
using System.Runtime.InteropServices;

namespace M4.M4v2.Chart
{
    public partial class TelerickCtlChart : UserControl, IDataSubscriber, IExpertAdvisorClient
    {
    
        #region Members and Structures

        private readonly List<string> _mPatternSeries = new List<string>();
        private readonly ctlData _mCtlData;
        private string _token = "";

        public int Subscribers; //If subscribers <> 0 then do not unload!
        public bool MClosing;
        public static frmMain MMain;
        public Color MSelectionBorderColor;
        public string MSymbol;
        public bool MUserEditing;
        public bool RealTimeUpdates = true;
        private readonly List<string> _mSeriesNames = new List<string>();
        private const bool MMissingVolume = false;
        private bool _mDialogShown;

        private abstract class HorzLine
        {
            public const int Panel = 0;
            public const double Value = 0;
        }

        private bool _mMenu;
        private readonly List<HorzLine> _mHorzLines = new List<HorzLine>();
        private double _mValue;
        private int _mRecord;

        public class BarData
        {
            public double JDate;
            public double OpenPrice;
            public double HighPrice;
            public double LowPrice;
            public double ClosePrice;
            public double Volume;
        }

        private class BarCache
        {
            public M4.DataServer.Interface.BarData Bar;

            public Periodicity BarType;
            public string Symbol;
            public bool IsNewBar;
        }
        private readonly List<BarCache> _mBarCache = new List<BarCache>(); //Caches bars while user is editing the chart

        private readonly List<BarData> Data = new List<BarData>();

        private string _mName;
        private int _mObjectType;
        private readonly string _mCmdArg;

        private AsyncOperation _asyncOp;

        private abstract class ChartOrder
        {
            public ctlPortfolio.Orders.Side OrderSide;
            public double EntryPrice;
            public int ChartRecord;
            public string ChartObjectLineName;
            public string ChartObjectTextName;
            public string ChartObjectSymbolName;
            public bool Executed;
            public int Quantity;
        }

        private readonly List<ChartOrder> m_Orders = new List<ChartOrder>();
        //For trading off the chart 

        public Periodicity MPeriodicity;
        public int MBarSize;
        public int MBars;

        private bool _mDrawingLineStudy;
        private int _mLastObjectCount;

        public bool DrawingLineStudy
        {
            get
            {
                return _mDrawingLineStudy;
            }
            set
            {
                _mDrawingLineStudy = value;
                if (value)
                {
                    EnableControls(false);
                    _mLastObjectCount = StockChartX1.GetObjectCount((ObjectType)(-1));
                }
                else
                {
                    EnableControls(true);
                }
            }
        }

        #endregion

        #region Initialization

        public TelerickCtlChart()
        {
            InitializeComponent();
            CreateAsyncOp();

            BindContextMenuEvents();
        }

        public TelerickCtlChart(frmMain oMain, ctlData oData, string Symbol, bool ErrorOnExcelFailure)
        {
            InitializeComponent();
            CreateAsyncOp();

            MMain = oMain;
            _mCtlData = oData;
            MSymbol = Symbol;
            UpdateChartColors(MMain.m_Style);

            if (Symbol.Equals("Excel Chart"))
            {
                ImportFromExcel();
            }
            else if (Symbol.Equals("CSV Chart"))
            {
                ImportFromCSV();
            }

            BindContextMenuEvents();
        }

        public TelerickCtlChart(frmMain oMain, ctlData oData, string loadChart)
        {
            _mCmdArg = loadChart;
            MMain = oMain;
            _mCtlData = oData;
            InitializeComponent();
            CreateAsyncOp();

            InitChartForm();

            BindContextMenuEvents();
        }

        public TelerickCtlChart(frmMain oMain, DdfDataManager oData, string symbol, Periodicity barPeriodicity, int barSize, int bars,
          bool async)
        {
            MMain = oMain;
            _mCtlData = (ctlData)oData;
            InitializeComponent();

            CreateAsyncOp();

            InitChartForm();
            MSymbol = symbol;
            MPeriodicity = barPeriodicity;
            MBarSize = barSize;
            MBars = bars;
            if (!async)
            {
                InitRTChart();
                BindContextMenuEvents();
            }
        }

        public TelerickCtlChart(frmMain oMain)
        {
            MMain = oMain;
            InitializeComponent();
            CreateAsyncOp();

            InitChartForm();

            BindContextMenuEvents();
        }

        private void CreateAsyncOp()
        {
            _asyncOp = AsyncHelper.CreateOperation();
        }

        public void BindContextMenuEvents()
        {
            //mnucHorzLine.Click += mnucHorzLine_Click;
            //mnuVertLine.Click += MnucVertLineClick;
            //mnuEditSeries.Click += mnuEditSeries_Click;
            //mnuDeleteObject.Click += mnuDeleteObject_Click;
            //mnuDeleteSeries.Click += mnuDeleteSeries_Click;
            //btnSubmit.Click += btnSubmit_Click;
            //mnuBuyHere.Click += mnuBuyHere_Click;
            //mnuSellHere.Click += mnuSellHere_Click;
            //mnuClearOrders.Click += mnuClearOrders_Click;
            //cmdCancel.Click += cmdCancel_Click;
        }

        //Initializes the chart form and loads a file depending
        //on which constructor was used.
        private void InitChartForm()
        {
            StockChartX1.ScalePrecision = 3; // TODO: Change this if you are trading forex

            /*** FROEDE_MARK Keep going with the last choices...
            m_Main.cboPriceStyles.HostedControl.SelectedIndex = 0;
            m_Main.mnuViewYGrid.Checked = true;
            m_Main.mnuViewSeparators.Checked = true;
            m_Main.mnuView3D.Checked = true;
             ***/

            StockChartX1.Visible = false;
            EnableControls(false);
            //m_Main.mnuPriceStyle.Enabled = false;
            //m_Main.mnuFileLoadChart.Enabled = true;
            //m_Main.mnuFileImportExcel.Enabled = true;
            //m_Main.mnuFileImportCSV.Enabled = true;
            //m_Main.cmdImportExcel.Enabled = true;
            //m_Main.cboIndicators.Items.Clear();
            StockChartX1.EnumIndicators();
            //m_Main.cboIndicators.HostedControl.SelectedIndex = 0;
            Application.DoEvents();
            //m_Main.UpdateStyle();
            if (_mCmdArg != "")
            {
                StockChartX1.Visible = true;
                if (File.Exists(_mCmdArg))
                {
                    StockChartX1.LoadFile(_mCmdArg);
                    EnableControls(true);
                }
            }
        }
        #endregion

        #region Destruction

        private bool disposed;

        public void DisposeEx()
        {
            MClosing = true;
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected override void Dispose(bool disposing)
        {
            MClosing = true;
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    StockChartX1.Dispose();

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~TelerickCtlChart()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region Chart Loading
        //Loads a chart into StockChartX
        //NOTE: This is not the real-time chart loading function. See InitRTChart below.
        public void LoadChart(string symbol)
        {
            StockChartX1.RemoveAllSeries();

            //m_Main.cboPriceStyles.HostedControl.SelectedIndex = 0;

            StockChartX1.Symbol = symbol.Replace(".", "");
            StockChartX1.Visible = true;

            //First add a panel (chart area) for the OHLC data:
            short panel = (short)StockChartX1.AddChartPanel();

            //Now add the open, high, low and close series to that panel:
            StockChartX1.AddSeries(symbol + ".open", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(symbol + ".high", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(symbol + ".low", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(symbol + ".close", SeriesType.stCandleChart, panel);

            //Change the color:
            StockChartX1.set_SeriesColor(symbol + ".close", ColorTranslator.ToOle(Color.Black));

            //Add the volume chart panel
            if (!MMissingVolume)
            {
                panel = (short)StockChartX1.AddChartPanel();
                StockChartX1.AddSeries(symbol + ".volume", SeriesType.stVolumeChart, panel);

                //Change volume color and weight of the volume panel:
                StockChartX1.set_SeriesColor(symbol + ".volume", ColorTranslator.ToOle(Color.Blue));
                StockChartX1.set_SeriesWeight(symbol + ".volume", 3);

                //Resize the volume panel to make it smaller
                StockChartX1.set_PanelY1(1, (int)Math.Round(StockChartX1.Height * 0.8));
            }
            for (short row = 2; row <= Data.Count - 1; row++)
            {
                StockChartX1.AppendValue(symbol + ".open", Data[row].JDate, Data[row].OpenPrice);
                StockChartX1.AppendValue(symbol + ".high", Data[row].JDate, Data[row].HighPrice);
                StockChartX1.AppendValue(symbol + ".low", Data[row].JDate, Data[row].LowPrice);
                StockChartX1.AppendValue(symbol + ".close", Data[row].JDate, Data[row].ClosePrice);
                if (!MMissingVolume)
                {
                    StockChartX1.AppendValue(symbol + ".volume", Data[row].JDate, Data[row].Volume);
                }
            }
            StockChartX1.ThreeDStyle = true;
            StockChartX1.HorizontalSeparators = true;
            StockChartX1.DisplayTitles = true;
            UpdateChartColors(MMain.m_Style);
            UpdateYScale();
            StockChartX1.Update();
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
            {
                //m_Main.mnuViewScaleType.Checked = false;
                //m_Main.mnuViewScaleType.Enabled = false;
            }
            else
            {
                //m_Main.mnuViewScaleType.Enabled = true;
            }
        }

        #endregion

        #region Save/Load Charts
        //Loads a previously saved chart. NOTE this does not connect to real-time data
        //Saving and loading a chart simply allows the user to save drawings, objects, etc.
        //for future reference - not to reconnect to a data feed.
        public void LoadChartFile(string FileName)
        {
            StockChartX1.LoadFile(FileName);
            StockChartX1.ForcePaint();

            EnableControls(true);

            //m_Main.mnuDarvasBoxes.Checked = StockChartX1.DarvasBoxes;
            //m_Main.mnuViewSeparators.Checked = StockChartX1.HorizontalSeparators;
            //m_Main.mnuView3D.Checked = StockChartX1.ThreeDStyle;
            //m_Main.mnuViewScaleType.Checked = StockChartX1.ScaleType == ScaleType.stLinearScale;
            //m_Main.mnuViewShowXGrid.Checked = StockChartX1.XGrid;
            //m_Main.mnuViewYGrid.Checked = StockChartX1.YGrid;
            //m_Main.mnuViewCrosshair.Checked = false;
            StockChartX1.Visible = true;
            EnableControls(true);

            //Can't show semi-log if chart is below 1
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1)
            {
                //m_Main.mnuViewScaleType.Checked = false;
                //m_Main.mnuViewScaleType.Enabled = false;
            }
            else
            {
                //m_Main.mnuViewScaleType.Enabled = true;
            }

            //Update information about the chart definition
            MSymbol = StockChartX1.Symbol;
            // FROEDE_MARK TODO m_BarSize = StockChartX1.GetBarSize();
            // FROEDE_MARK TODO m_Periodicity = StockChartX1.GetPeriodicity(); 
        }

        //Updates a previously saved chart file with new price data and loads it
        public bool LoadAndUpdateChartFile(string fileName)
        {
            if (!File.Exists(fileName)) return false;

            try
            {
                StockChartX2.LoadFile(fileName);
                StockChartX2.Top = StockChartX1.Top;
                StockChartX2.Left = StockChartX1.Left;
                StockChartX2.Width = StockChartX1.Width;
                StockChartX2.Height = StockChartX1.Height;
                StockChartX2.ClearAllSeries();
                for (int row = 1; row <= StockChartX1.RecordCount; row++)
                {
                    double jDate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", row);
                    double o = StockChartX1.GetValue(StockChartX1.Symbol + ".open", row);
                    double h = StockChartX1.GetValue(StockChartX1.Symbol + ".high", row);
                    double l = StockChartX1.GetValue(StockChartX1.Symbol + ".low", row);
                    double c = StockChartX1.GetValue(StockChartX1.Symbol + ".close", row);
                    long v = (long)Math.Round(StockChartX1.GetValue(StockChartX1.Symbol + ".volume", row));
                    StockChartX2.AppendValue(StockChartX2.Symbol + ".open", jDate, o);
                    StockChartX2.AppendValue(StockChartX2.Symbol + ".high", jDate, h);
                    StockChartX2.AppendValue(StockChartX2.Symbol + ".low", jDate, l);
                    StockChartX2.AppendValue(StockChartX2.Symbol + ".close", jDate, c);
                    StockChartX2.AppendValue(StockChartX2.Symbol + ".volume", jDate, v);
                }
                StockChartX2.Update();
                StockChartX2.SaveFile(fileName);
                if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
                {
                    //m_ctlData.m_Main.mnuViewScaleType.Checked = false;
                    //m_ctlData.m_Main.mnuViewScaleType.Enabled = false;
                }
                else
                {
                    //m_ctlData.m_Main.mnuViewScaleType.Enabled = true;
                }
                StockChartX1.LoadFile(fileName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Saves a chart to disk
        public void SaveChart()
        {
            SaveChart("");
        }
        public void SaveChart(string fileName)
        {
            if (m_Orders.Count > 0) ClearOrders();
            if (fileName.Length == 0)
                fileName = SaveDialog();
            if (fileName == "") return;
            StockChartX1.SaveFile(fileName);
        }
        #endregion

        #region Microsoft Excel Import/Export
        //NOTE: the following functions work at the time of this writing but are unsupported

        //Exports a chart to Excel
        public void ExportToExcel()
        {
            if (StockChartX1.RecordCount < 3)
            {
                MessageBox.Show("A chart must be loaded before using this feature.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            //m_Main.ShowStatus("Exporting " + StockChartX1.Symbol + "...");
            Cursor = Cursors.WaitCursor;

            //List all series
            _mSeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (_mSeriesNames.Count < StockChartX1.SeriesCount)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }

            //Field names -- inibidos: não mais exibimos a linha de descrição das colunas
            //OHLCV
            /*
          string output = StockChartX1.Symbol + ";";
          output += "Date;";
          output += StockChartX1.Symbol + " Open;";
          output += StockChartX1.Symbol + " High;";
          output += StockChartX1.Symbol + " Low;";
          output += StockChartX1.Symbol + " Close;";
          output += StockChartX1.Symbol + " Volume";
             */

            //Indicators -- inibidos por enquanto: no futuro serão opcionais
            /*
          for (int n = 4; n <= m_SeriesNames.Count - 2; n++)
          {
            output += "," + m_SeriesNames[n];
          }
         
          output = output + "\r\n";
            */

            string output = "\r\n";

            for (int r = 1; r <= StockChartX1.RecordCount; r++)
            {
                output += StockChartX1.Symbol + ";";
                string sDate = StockChartX1.FromJulianDate(StockChartX1.GetJDate(MSymbol + ".close", r));
                sDate = sDate.Substring(0, sDate.IndexOf(" "));
                output += sDate + ";";
                //OHLCV
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".open", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".high", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".low", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".close", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".volume", r)) + "";

                //Indicators -- inibidos por enquanto: no futuro serão opcionais
                /*
              for (int n = 4; n <= m_SeriesNames.Count - 2; n++)
              {
                double value = StockChartX1.GetValue(m_SeriesNames[n], r);
                if (value == (double)DataType.dtNullValue)
                {
                  value = 0.0;
                }
                output = output + ";" + Convert.ToString(value);
              }
                 */
                output = output + "\r\n";
            }

            //Create the file
            string path = Application.StartupPath + @"\Exported\";
            string fileName = path + StockChartX1.Symbol + ".csv";
            Directory.CreateDirectory(path);
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception)
                {
                    //The file is locked
                    fileName = path + StockChartX1.Symbol + "~" + Convert.ToString(DateTime.Now.Ticks) + ".csv";
                }
            }
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(output);
            sw.Close();

            //Load the data into excel
            Process.Start(fileName);
            Cursor = Cursors.Arrow;
            //m_Main.ShowStatus("");
        }

        //Exports a chart to a CSV File
        public void ExportToCsv()
        {
            if (StockChartX1.RecordCount < 3)
            {
                MessageBox.Show("A chart must be loaded before using this feature.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            //m_Main.ShowStatus("Exporting " + StockChartX1.Symbol + "...");
            Cursor = Cursors.WaitCursor;

            //List all series
            _mSeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (_mSeriesNames.Count < StockChartX1.SeriesCount)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }

            //Field names 
            //OHLCV

            string output = "#;" + StockChartX1.Symbol + ";Date;Time;Open;High;Low;Close;Volume";

            //Indicators -- disabled for now: optional in the future...may be
            /*
          for (int n = 4; n <= m_SeriesNames.Count - 2; n++)
          {
            output += "," + m_SeriesNames[n];
          }
         
            output = output + "\r\n";
             */

            output += "\r\n";

            for (int r = 1; r <= StockChartX1.RecordCount; r++)
            {
                output += StockChartX1.Symbol + ";";
                string sDateTime = StockChartX1.FromJulianDate(StockChartX1.GetJDate(MSymbol + ".close", r));
                string sDate = sDateTime.Substring(0, sDateTime.IndexOf(" "));
                string sTime = sDateTime.Substring(11, 8);
                output += sDate + ";" + sTime + ";";

                //OHLCV
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".open", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".high", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".low", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".close", r)) + ";";
                output += Convert.ToString(StockChartX1.GetValue(MSymbol + ".volume", r)) + "";

                //Indicators -- disabled for now: optional in the future...may be
                /*
              for (int n = 4; n <= m_SeriesNames.Count - 2; n++)
              {
                double value = StockChartX1.GetValue(m_SeriesNames[n], r);
                if (value == (double)DataType.dtNullValue)
                {
                  value = 0.0;
                }
                output = output + ";" + Convert.ToString(value);
              }
                 */
                output = output + "\r\n";
            }

            //Create the file
            string fileName = SaveDialog("CSV Stock Chart Files|*.csv");

            if (fileName.Length < 5)
            {
                return;
            }

            fileName = fileName.Substring(0, fileName.Length - 4) + ".csv";

            /*** FROEDE_MARK
            string path = Application.StartupPath + @"\Exported\";
            string fileName = path + StockChartX1.Symbol + ".csv";
            Directory.CreateDirectory(path);
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception)
                {
                    //The file is locked
                    fileName = path + StockChartX1.Symbol + "~" + Convert.ToString(DateTime.Now.Ticks) + ".csv";
                }
            }
             ***/

            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(output);
            sw.Close();

            //Load the data into application
            Process.Start(fileName);
            Cursor = Cursors.Arrow;
            //m_Main.ShowStatus("");
        }

        public bool IsDate(object inValue)
        {
            DateTime dt;
            return DateTime.TryParse(inValue.ToString(), out dt);
        }

        public bool IsDateTimeExact(object inValue)
        {
            DateTime dt;
            return DateTime.TryParseExact(inValue.ToString(), "dd/MM/yyyy hh:mm:ss", null,
                                 DateTimeStyles.None, out dt);
        }

        public bool IsDateExact(object inValue)
        {
            DateTime dt;
            return DateTime.TryParseExact(inValue.ToString(), "dd/MM/yyyy", null,
                                 DateTimeStyles.None, out dt);
        }

        public bool IsTimeExact(object inValue)
        {
            DateTime dt;
            return DateTime.TryParseExact(inValue.ToString(), "hh:mm:ss", null,
                                 DateTimeStyles.None, out dt);
        }

        public string GetChartTitle()
        {
            string title = MSymbol;
            switch (MPeriodicity)
            {
                case Periodicity.Secondly:
                    title += " " + MBarSize + " Sec";
                    break;
                case Periodicity.Minutely:
                    title += " " + MBarSize + " Min";
                    break;
                case Periodicity.Hourly:
                    title += " " + MBarSize + " Hour";
                    break;
                case Periodicity.Daily:
                    if (MBarSize > 1)
                        title += " " + MBarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabDaily"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabDaily"];
                    break;
                case Periodicity.Weekly:
                    if (MBarSize > 1)
                        title += " " + MBarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabWeekly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabWeekly"];
                    break;
                case Periodicity.Month:
                    if (MBarSize > 1)
                        title += " " + MBarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabMonthly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabMonthly"];
                    break;
                case Periodicity.Year:
                    if (MBarSize > 1)
                        title += " " + MBarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabYearly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabYearly"];
                    break;
                default:
                    break;
            }

            return title;
        }

        private static string getCellValue(Microsoft.Office.Interop.Excel.Worksheet Worksheet, int offRow, int offCol, int Row, int Col)
        {
            string ret = "";
            try
            {
                Microsoft.Office.Interop.Excel.Range objRange = (Microsoft.Office.Interop.Excel.Range)Worksheet.Cells[offRow + Row - 1, offCol + Col - 1];
                ret = objRange.get_Value(Type.Missing).ToString();
            }
            catch { }
            return ret;
        }

        //Imports data from the active Excel sheet - if available 
        public void ImportFromExcel()
        {
            string strInstr;

            int row;
            int col;

            Microsoft.Office.Interop.Excel.Application objExcel;
            Microsoft.Office.Interop.Excel.Worksheet objWorksheet;

            try
            {
                objExcel = (Microsoft.Office.Interop.Excel.Application)Marshal.GetActiveObject("Excel.Application");
            }
            catch (Exception)
            {
                MessageBox.Show("Excel is not open", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //excel.Application 
            if (objExcel == null)
            {
                MessageBox.Show("Please ensure that Excel is open before" + "\r\n" + "attempting to using this feature", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            objWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)objExcel.ActiveSheet;
            if (objWorksheet == null)
            {
                MessageBox.Show("Please ensure that Excel is open and a Sheet is" + "\r\n" + "loaded before attempting to using this feature.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StockChartX1.Symbol = objWorksheet.Name;
            StockChartX1.Symbol = StockChartX1.Symbol.Replace(".", "");
            int found = StockChartX1.Symbol.IndexOf("~");
            if (found > -1)
            {
                StockChartX1.Symbol = StockChartX1.Symbol.Substring(0, found - 1);
            }

            objExcel.Visible = true;
            strInstr = "Select the range of data to include in the chart." + "\r\n" + "You can type the range in the box below or you can use the mouse to select the data on your worksheet.";

            Microsoft.Office.Interop.Excel.Range startRange;

            startRange = (Microsoft.Office.Interop.Excel.Range)objExcel.InputBox(strInstr, "Data Source", Type.Missing,
              Type.Missing, Type.Missing, Type.Missing, Type.Missing, 8);

            //Validate the selection 
            string strDate;
            int offRow = startRange.get_Offset(Type.Missing, Type.Missing).Row;
            int offCol = startRange.get_Offset(Type.Missing, Type.Missing).Column;
            Microsoft.Office.Interop.Excel.Range objRange = (Microsoft.Office.Interop.Excel.Range)objWorksheet.Cells[offRow, offCol];
            // FROEDE_MARK strDate = objRange.get_Value(Type.Missing).ToString();

            strDate = getCellValue(objWorksheet, offRow, offCol, 1, 2);
            if (!IsDateTimeExact(strDate))
            {
                StockChartX1.Visible = false;
                MessageBox.Show("The second column must contain dates (dd/MM/yyyy)." + strDate, "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (startRange.Columns.Count < 6)
            {
                StockChartX1.Visible = false;
                MessageBox.Show("Selection must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StockChartX1.Visible = false;
            return;

/*
          catch{

              EnableControls(false); 
              StockChartX1.Visible = false;    
              MessageBox.Show("Invalid data selection. Please try again!", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);

          }
        */


        }

        //Imports data from a CSV data file - if available 
        public void ImportFromCSV()
        {
            bool goodSoFar;

            //Looking for the input file
            string fName = OpenDialog("CSV Stock Chart Files|*.csv");
            if (!File.Exists(fName))
            {
                MessageBox.Show("Unable to locate the file " + fName, "Invalid CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Looking for the chart name  
            StreamReader stream = new StreamReader(fName);
            string row = null;

            goodSoFar = false;
            while ((row = stream.ReadLine()) != null)
            {
                string[] splitRow = row.Split(';');

                if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                {
                    continue;
                }

                StockChartX1.Symbol = splitRow[0];
                stream.Close();
                goodSoFar = true;
                break;
            }

            if (!goodSoFar)
            {
                MessageBox.Show(" 0 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", "Invalid CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Looking for the date order  
            bool bDate1 = false, bDate2 = false, fileOrderDescending = false;
            DateTime date1 = DateTime.Today, date2 = DateTime.Today;
            string[] time1 = null, time2 = null;
            int candleInfoIndex = 2;

            stream = new StreamReader(fName);
            row = null;
            goodSoFar = false;

            while ((row = stream.ReadLine()) != null)
            {
                string[] splitRow = row.Split(';');

                if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                {
                    continue;
                }

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
            {
                MessageBox.Show(" 1 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", "Invalid CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (time1.Length == 3 && time2.Length == 3)
            {
                candleInfoIndex = 3;

                DateTime date1Tmp = new DateTime(date1.Year, date1.Month, date1.Day,
                                           Convert.ToInt32(time1[0]), Convert.ToInt32(time1[1]), Convert.ToInt32(time1[2]));
                DateTime date2Tmp = new DateTime(date2.Year, date2.Month, date2.Day,
                                           Convert.ToInt32(time2[0]), Convert.ToInt32(time2[1]), Convert.ToInt32(time2[2]));

                fileOrderDescending = (date1Tmp > date2Tmp) ? true : false;
            }
            else if (time1.Length == 1 && time2.Length == 1)
            {
                candleInfoIndex = 2;
                fileOrderDescending = (date1 > date2) ? true : false;
            }
            else
            {
                MessageBox.Show(" 2 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", "Invalid CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int Panel;

            //Reset the entire chart 
            EnableControls(false);
            StockChartX1.Visible = false;
            StockChartX1.RemoveAllSeries();

            //First add a panel (chart area) for the OHLC data: 
            Panel = StockChartX1.AddChartPanel();

            //Now add the open, high, low and close series to that panel: 
            StockChartX1.AddSeries(StockChartX1.Symbol + ".open", SeriesType.stCandleChart, Panel);
            StockChartX1.AddSeries(StockChartX1.Symbol + ".high", SeriesType.stCandleChart, Panel);
            StockChartX1.AddSeries(StockChartX1.Symbol + ".low", SeriesType.stCandleChart, Panel);
            StockChartX1.AddSeries(StockChartX1.Symbol + ".close", SeriesType.stCandleChart, Panel);
            //Change the color: 
            StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".close", ColorTranslator.ToOle(Color.Black));

            //Now add the volume chart panel 
            Panel = StockChartX1.AddChartPanel();
            StockChartX1.AddSeries(StockChartX1.Symbol + ".volume", SeriesType.stVolumeChart, Panel);
            //Change volume color and weight of the volume panel: 
            StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".volume", ColorTranslator.ToOle(Color.Blue));
            StockChartX1.set_SeriesWeight(StockChartX1.Symbol + ".volume", 3);
            //Resize the volume panel to make it smaller 
            
            int chartH = StockChartX1.Height;

            if (chartH > Height)
                StockChartX1.Height = Height - 40;

            StockChartX1.set_PanelY1(1, (int)(StockChartX1.Height * 0.8));

            //Insert values into StockChartX 
            double jdate = 0;
            int hr;
            int mn;
            int sc;

            Cursor = Cursors.WaitCursor;
            //m_Main.ShowStatus("Importing " + StockChartX1.Symbol + "...");
            Application.DoEvents();

            //Find out how many rows is in the CSV file
            stream = new StreamReader(fName);
            row = null;
            int rowCount = 0;
            while ((row = stream.ReadLine()) != null) rowCount++;
            stream.Close();

            //Bring all of them to the memory
            stream = new StreamReader(fName);
            row = null;
            string[] rowCollection = new string[rowCount];
            rowCount = 0;
            while ((row = stream.ReadLine()) != null)
            {
                rowCollection[rowCount] = row;
                rowCount++;
            }
            stream.Close();

            //Add data to the chart
            if (!fileOrderDescending)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    string[] splitRow = rowCollection[i].Split(';');

                    if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                    {
                        continue;
                    }

                    //Getting date and time information
                    DateTime dt;
                    if (!DateTime.TryParseExact(splitRow[1], "dd/MM/yyyy", null,
                                       DateTimeStyles.None, out dt))
                    {
                        continue;
                    }

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

                    jdate = StockChartX1.ToJulianDate(dt.Year, dt.Month, dt.Day, hr, mn, sc);

                    //Getting candle and volume information
                    double open = Convert.ToDouble(splitRow[candleInfoIndex]);
                    double high = Convert.ToDouble(splitRow[candleInfoIndex + 1]);
                    double low = Convert.ToDouble(splitRow[candleInfoIndex + 2]);
                    double close = Convert.ToDouble(splitRow[candleInfoIndex + 3]);
                    double volume = Convert.ToDouble(splitRow[candleInfoIndex + 4]);

                    //Add data to the chart
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".open", jdate, open);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".high", jdate, high);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".low", jdate, low);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".close", jdate, close);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".volume", jdate, volume);
                }
            }
            else
            {
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    string[] splitRow = rowCollection[i].Split(';');

                    if (splitRow.Length < 6 || splitRow[0].Equals("#"))
                    {
                        continue;
                    }

                    //Getting date and time information
                    DateTime dt;
                    if (!DateTime.TryParseExact(splitRow[1], "dd/MM/yyyy", null,
                                       DateTimeStyles.None, out dt))
                    {
                        continue;
                    }

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

                    jdate = StockChartX1.ToJulianDate(dt.Year, dt.Month, dt.Day, hr, mn, sc);

                    //Getting candle and volume information
                    double open = Convert.ToDouble(splitRow[candleInfoIndex]);
                    double high = Convert.ToDouble(splitRow[candleInfoIndex + 1]);
                    double low = Convert.ToDouble(splitRow[candleInfoIndex + 2]);
                    double close = Convert.ToDouble(splitRow[candleInfoIndex + 3]);
                    double volume = Convert.ToDouble(splitRow[candleInfoIndex + 4]);

                    //Add data to the chart
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".open", jdate, open);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".high", jdate, high);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".low", jdate, low);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".close", jdate, close);
                    StockChartX1.AppendValue(StockChartX1.Symbol + ".volume", jdate, volume);
                }
            }

            //m_Main.ShowStatus("");
            Cursor = Cursors.Arrow;
            EnableControls(true);

            //Change some display properties: 
            StockChartX1.ThreeDStyle = true;
            StockChartX1.UpColor = Color.FromArgb(51, 204, 51); // FROEDE_MARK Green;
            StockChartX1.DownColor = Color.FromArgb(255, 80, 80); // FROEDE_MARK Red;
            StockChartX1.DisplayTitles = true;

            //Update the chart 
            StockChartX1.Update();

            //Can't show semi-log if chart is below 1 
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1)
            {
                //m_Main.mnuViewScaleType.Checked = false;
                //m_Main.mnuViewScaleType.Enabled = false;
            }
            else
            {
                //m_Main.mnuViewScaleType.Enabled = true;
            }

            StockChartX1.Visible = true;

            MSymbol = StockChartX1.Symbol;

            //Restore and activate 
            if (!(UnmanagedMethods.IsWindowVisible(Handle)))
            {
                //UnmanagedMethods.ShowWindow(Handle, UnmanagedMethods.SW_SHOW);
            }
            if ((UnmanagedMethods.IsIconic(Handle)))
            {
                UnmanagedMethods.SendMessage(Handle, UnmanagedMethods.WM_SYSCOMMAND, UnmanagedMethods.SC_RESTORE, IntPtr.Zero);
            }
            UnmanagedMethods.SetForegroundWindow(Handle);

            /*
          catch{

              EnableControls(false); 
              StockChartX1.Visible = false;    
              MessageBox.Show("Invalid data selection. Please try again!", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);

          }
           */

        }

        #endregion

        #region Menus and Controls
        //Updates the chart style
        private void ChangeStyle(SeriesType ChartStyle)
        {
            string Symbol = StockChartX1.Symbol;
            StockChartX1.PriceStyle = PriceStyle.psStandard;
            SeriesType stType = ChartStyle; // FROEDE_MARK BarStyle ? SeriesType.stStockBarChart : SeriesType.stCandleChart;
            StockChartX1.set_SeriesType(Symbol + ".open", stType);
            StockChartX1.set_SeriesType(Symbol + ".high", stType);
            StockChartX1.set_SeriesType(Symbol + ".low", stType);
            StockChartX1.set_SeriesType(Symbol + ".close", stType);
            StockChartX1.Update();
        }

        //Changes the chart's price style (candle, bar, renk, kagi, etc.)
        public void ChangePriceStyle(string HostedControlText)
        {
            //m_Main.mnuPriceStyle.Enabled = false;
            switch (HostedControlText)
            {
                case "Bar Chart":
                    ChangeStyle(SeriesType.stStockBarChart);
                    break;
                case "Candle Chart":
                    ChangeStyle(SeriesType.stCandleChart);
                    break;
                case "StockLine":
                    ChangeStyle(SeriesType.stStockLineChart);
                    break;
                case "Point && Figure":
                    StockChartX1.PriceStyle = PriceStyle.psPointAndFigure;
                    //m_Main.mnuPriceStyle.Enabled = true;
                    //(new frmPriceStyle()).GetInput(StockChartX1, m_Main.cboPriceStyles.HostedControl.Text);
                    break;
                case "Renko":
                    StockChartX1.PriceStyle = PriceStyle.psRenko;
                    //m_Main.mnuPriceStyle.Enabled = true;
                    //(new frmPriceStyle()).GetInput(StockChartX1, m_Main.cboPriceStyles.HostedControl.Text);
                    break;
                case "Kagi":
                    StockChartX1.PriceStyle = PriceStyle.psKagi;
                    //m_Main.mnuPriceStyle.Enabled = true;
                    //(new frmPriceStyle()).GetInput(StockChartX1, m_Main.cboPriceStyles.HostedControl.Text);
                    break;
                case "Three Line Break":
                    StockChartX1.PriceStyle = PriceStyle.psThreeLineBreak;
                    //m_Main.mnuPriceStyle.Enabled = true;
                    //(new frmPriceStyle()).GetInput(StockChartX1, m_Main.cboPriceStyles.HostedControl.Text);
                    break;
                case "EquiVolume":
                    StockChartX1.PriceStyle = PriceStyle.psEquiVolume;
                    break;
                case "EquiVolume Shadow":
                    StockChartX1.PriceStyle = PriceStyle.psEquiVolumeShadow;
                    break;
                case "Candle Volume":
                    StockChartX1.PriceStyle = PriceStyle.psCandleVolume;
                    break;
                case "Heikin Ashi":
                    StockChartX1.PriceStyle = PriceStyle.psHeikinAshi;
                    break;
            }
        }

        //Draws a horizontal line on the chart under the mouse pointer
        private void mnucHorzLine_Click(object sender, CommandEventArgs e)
        {

            //string key = "hline" + DateTime.Now.Ticks;
            //StockChartX1.DrawTrendLine(StockChartX1.CurrentPanel, m_Value, 0, m_Value, StockChartX1.RecordCount, key);
            //StockChartX1.AddTrendLineWatch(key, StockChartX1.Symbol + ".close");

            /*
            StockChartX1.AddHorizontalLine(StockChartX1.CurrentPanel, m_Value);
            m_horzLines.Add(new HorzLine
            {
              Panel = StockChartX1.CurrentPanel,
              Value = m_Value,
            });
            */

            //StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));
            //StockChartX1.Update();

            string key = "hline" + DateTime.Now.Ticks;

            StockChartX1.DrawTrendLine(StockChartX1.CurrentPanel,
              _mValue, (int)DataType.dtNullValue, _mValue, (int)DataType.dtNullValue, key);
            StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));

        }

        // Draws a vertical line on the chart. NOTE: StockChartX does NOT support click events for vertical lines, due to the 
        // individual "panel" design. This behavior is different from horizontal lines (which were originally intended for 
        // "80/20" indicator reference lines. You may want to remove the vertical lines if the inconsistent usage is a concern.
        private void MnucVertLineClick(object sender, CommandEventArgs e)
        {
            // NOTE: by design, vertical lines that extend through more than one panel cannot be selected in StockChartX.     
            //string key = "vline" + DateTime.Now.Ticks;

            //StockChartX1.DrawTrendLine(StockChartX1.GetPanelBySeriesName(StockChartX1.Symbol + ".volume"),
            //  (double)DataType.dtNullValue, m_Record, (double)DataType.dtNullValue, m_Record, key);
            //StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));      

            string key = "vline" + DateTime.Now.Ticks;

            StockChartX1.DrawTrendLine(StockChartX1.CurrentPanel,
              (double)DataType.dtNullValue, _mRecord, (double)DataType.dtNullValue, _mRecord, key);
            StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));

        }

        //Adds a new indicator to the chart and requests the user to enter the parameters
        public void AddIndicator()
        {
            if (StockChartX1.RecordCount < 3) return;
            string cnt = "";
            //int n = StockChartX1.GetIndicatorCountByType((Indicator)m_Main.cboIndicators.HostedControl.SelectedIndex);
            //if (n > 0)
            //{
            //    cnt = " " + (n + 1);
            //}
            //int panel = IsOverlay(m_Main.cboIndicators.HostedControl.Text) ? 0 : StockChartX1.AddChartPanel();
            //int indicator = m_Main.cboIndicators.HostedControl.SelectedIndex;
            //StockChartX1.AddIndicatorSeries((Indicator)indicator, m_Main.cboIndicators.HostedControl.Text + cnt, panel, true);
            StockChartX1.Update();
        }

        //Returns TRUE if a series is an OHLC overlay
        private static bool IsOverlay(string name)
        {
            string[] overlays = new[]
                             {
                               "PARABOLIC", "PSAR", "FORECAST", "INTERCEPT",
                               "WEIGHTED CLOSE", "TYPICAL PRICE", "WEIGHTED PRICE",
                               "MEDIAN PRICE", "SMOOTHING", "BOLLINGER",
                               "MOVING AVERAGE", "BANDS"
                             };
            return overlays.Any(overlay => name.IndexOf(overlay, StringComparison.CurrentCultureIgnoreCase) != -1);
        }

        //Enables/disables the controls on frmMain that pertain to charting
        public void EnableControls(bool Enable)
        {
            try
            {
                if (DrawingLineStudy) Enable = true;
                //m_Main.mnuFileSaveChart.Enabled = Enable;
                //m_Main.cboPriceStyles.Enabled = Enable;
                //m_Main.cboIndicators.Enabled = Enable;
                //m_Main.cmdZoomIn.Enabled = Enable;
                //m_Main.cmdZoomOut.Enabled = Enable;
                //m_Main.cmdScrollLeft.Enabled = Enable;
                //m_Main.cmdTemplate.Enabled = Enable;
                //m_Main.cmdScrollRight.Enabled = Enable;
                //m_Main.cmdDelete.Enabled = Enable;
                //m_Main.mnuExcel.Enabled = Enable;
                //m_Main.mnuPatternRecognition.Enabled = Enable;
                //m_Main.cmdPrintChart.Enabled = Enable;
                //m_Main.mnuFileLoadChart.Enabled = true; // FROEDE_MARK Enable;
                //m_Main.mnuFileSaveImage.Enabled = Enable;
                //m_Main.mnuFileSaveTemplate.Enabled = Enable;
                //m_Main.mnuFileExport.Enabled = Enable;
                //m_Main.mnuFileExportCSV.Enabled = Enable;
                //m_Main.mnuFileImportCSV.Enabled = true; // FROEDE_MARK Enable;
                //m_Main.mnuFilePrint.Enabled = Enable;
                //m_Main.mnuView3D.Enabled = Enable;
                //m_Main.mnuViewScaleType.Enabled = Enable;
                //m_Main.mnuViewSeparators.Enabled = Enable;
                //m_Main.mnuViewShowXGrid.Enabled = Enable;
                //m_Main.mnuViewYGrid.Enabled = Enable;
                //m_Main.mnuViewCrosshair.Enabled = Enable;
                //m_Main.mnuDarvasBoxes.Enabled = Enable;
                //m_Main.mnuColors.Enabled = Enable;
                //m_Main.mnuTools.Enabled = Enable;
                //m_Main.mnuZoomIn.Enabled = Enable;
                //m_Main.mnuZoomOut.Enabled = Enable;
                //m_Main.mnuApplyTemplate.Enabled = Enable;
                //m_Main.mnuScrollLeft.Enabled = Enable;
                //m_Main.mnuScrollRight.Enabled = Enable;
                //if (StockChartX1.PriceStyle == PriceStyle.psStandard)
                //    m_Main.mnuPriceStyle.Enabled = false;
                //else
                //    m_Main.mnuPriceStyle.Enabled = Enable;
                //m_Main.mnuApplyExpertAdvisor.Enabled = Enable;
                //m_Main.mnuConsensusReport.Enabled = Enable;
                //m_Main.mnuNN.Enabled = Enable;
                //m_Main.mnuChart.Enabled = Enable;
            }
            catch (Exception)
            {
                // Form already closing, possible COM object separation
            }
        }

        //Shows the StockChartX series property dialog
        private void mnuEditSeries_Click(object sender, CommandEventArgs e)
        {
            StockChartX1.ShowIndicatorDialog(StockChartX1.SelectedKey);
        }

        //Deletes an object
        private void mnuDeleteObject_Click(object sender, CommandEventArgs e)
        {
            StockChartX1.RemoveObject((ObjectType)_mObjectType, _mName);
        }

        //Deletes a series    
        private void mnuDeleteSeries_Click(object sender, CommandEventArgs e)
        {
            DialogResult result = MessageBox.Show("Remove series?", "Question", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            StockChartX1.RemoveSeries(_mName);
        }

        //Removes all drawings from the chart
        public void DeleteDrawings()
        {
            DialogResult result = MessageBox.Show("Remove all drawings?", "Question", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            StockChartX1.ClearDrawings();
            foreach (HorzLine horzLine in _mHorzLines)
            {
                StockChartX1.RemoveHorizontalLine(HorzLine.Panel, HorzLine.Value);
            }
            _mHorzLines.Clear();

            // Remove APR series      
            StockChartX1.RemoveSeries("Top Pattern");
            StockChartX1.RemoveSeries("Bottom Pattern");

        }

        //A 2-pixel border is left around the chart control so a selection border can be drawn
        private void ctlChart_Resize(object sender, EventArgs e)
        {
            //WARNING: StockChartX1 dock must be NONE
            StockChartX1.Top = 2;
            StockChartX1.Left = 2;
            StockChartX1.Height = Height - 40;

            if (pnlConsensus.Visible)
            {
                pnlTwitter.Visible = false;
                pnlConsensus.Height = Height;
                pnlConsensus.Left = Width - pnlConsensus.Width;
                StockChartX1.Width = Width - pnlConsensus.Width - 4;
            }
            else if (pnlTwitter.Visible)
            {
                pnlConsensus.Visible = false;
                pnlTwitter.Height = Height;
                pnlTwitter.Left = Width - pnlTwitter.Width;
                TwitterTimelineControl.Height = Height - TwitterTimelineControl.Top - pnlTwitterControls.Height - 4;
                StockChartX1.Width = Width - pnlTwitter.Width - 4;
            }
            else
            {
                StockChartX1.Width = Width - 4;
            }

            if (webBrowser1.Visible)
            {
                webBrowser1.Top = StockChartX1.Top;
                webBrowser1.Left = StockChartX1.Left;
                webBrowser1.Width = StockChartX1.Width;
                webBrowser1.Height = StockChartX1.Height;
                pnlTwitterAuthorize.Left = webBrowser1.Width / 2 - (pnlTwitterAuthorize.Width / 2);
            }

            RepositionEaButton();
        }
        #endregion

        #region StockChartX Events
        //Lists all available indicators. Fires after StockChartX1.EnumIndicators is called.
        private void StockChartX1_EnumIndicator(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEvent e)
        {
            //m_Main.cboIndicators.Items.Add(e.indicatorName);
            string indicatorName = e.indicatorName.ToLower();
            //if (indicatorName.IndexOf("bands") != -1 || indicatorName.IndexOf("macd") != -1 || indicatorName.IndexOf("stochastic oscillator") != -1)
            //    m_Main.cboIndicators.Items[m_Main.cboIndicators.Items.Count - 1].ImageIndex = 10;
            //else
            //    m_Main.cboIndicators.Items[m_Main.cboIndicators.Items.Count - 1].ImageIndex = 9;
        }

        // Lists all series added to the chart
        public List<string> GetSeries()
        {
            _mSeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (_mSeriesNames.Count < StockChartX1.SeriesCount)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }
            return _mSeriesNames;
        }

        //Fires after StockChartX1.EnumSeries is called.
        private void StockChartX1_EnumSeriesEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumSeriesEvent e)
        {
            _mSeriesNames.Add(e.name);
        }

        //Has an item been clicked on?
        //This fires for Text, Symbol, or line objects as well as series.
        private void StockChartX1_ItemRightClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemRightClickEvent EventArgs)
        {
            Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };

            _mName = EventArgs.name;
            _mObjectType = (int)EventArgs.objectType;

            switch (EventArgs.objectType)
            {
                case ObjectType.otIndicator:
                case ObjectType.otVolumeChart:
                case ObjectType.otLineChart:
                case ObjectType.otCandleChart:
                case ObjectType.otStockBarChart:
                    if (_mName.IndexOf(StockChartX1.Symbol + ".") > -1) return;
                    _mMenu = true;
                    StockChartX1.Update();
                    break;
                case ObjectType.otTextObject:
                case ObjectType.otLineStudyObject:
                case ObjectType.otSymbolObject:
                case ObjectType.otLineObject:
                    _mMenu = true;
                    break;
            }
        }

        private void StockChartX1_MouseMoveEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEvent e)
        {
            _mValue = StockChartX1.GetYValueByPixel(e.y);
            _mRecord = e.record;
        }

        private void StockChartX1_OnRButtonDown(object sender, EventArgs e)
        {
            _mMenu = false; //No context menu is shown
            MUserEditing = true;
        }

        private void StockChartX1_OnRButtonUp(object sender, EventArgs e)
        {
            if (_mMenu) return; //Another context menu is shown        
            if (StockChartX1.SelectedKey != "") return;
            Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };
            tmrEdit.Enabled = true;
        }

        private void StockChartX1_ClickEvent(object sender, EventArgs e)
        {
            DrawSelection();
        }

        #endregion

        #region Misc
        public string Title
        {
            get
            {
                return _mCtlData.GetChartTitle(MSymbol, MPeriodicity, MBarSize);
            }
        }

        //Query to save changes for StockChartX
        public bool QuerySaveChanges()
        {
            try
            {
                if (StockChartX1.Changed && StockChartX1.Visible)
                {
                    string title;
                    if (_mCtlData != null)
                        title = "Save " + Title + " chart?";
                    else
                        title = "Save changes to chart?";

                    DialogResult answer = MessageBox.Show(title, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (answer == DialogResult.Cancel)
                        return false; //Don't close the application
                    if (answer == DialogResult.No)
                        return true; //Don't save

                    SaveDialog("");
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
            return true; //Nothing to do
        }

        private void UpdateYScale()
        {
            double max = StockChartX1.GetMaxValue(StockChartX1.Symbol + ".high");
            double min = StockChartX1.GetMinValue(StockChartX1.Symbol + ".low");
            StockChartX1.YScaleMinTick = (max - min) < 1.0 ? 0.05 : 0.25;
        }

        //Updates the chart colors based on the Nevron style selected on frmMain
        public void UpdateChartColors(string style)
        {
            StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".close", ColorTranslator.ToOle(Color.Black));
            switch (style)
            {
                case "Office2007Blue":

                    if (Properties.Settings.Default.AssociateGradient)
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    }
                    else
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientTopOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[2]));
                    }

                    StockChartX1.BackGradientBottom = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                          ? Color.FromArgb(0xd5, 0xe7, 0xff)
                                                          : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    StockChartX1.ChartBackColor = !Properties.Settings.Default.ChartBackColorOverwrite
                                                      ? Color.FromArgb(0xd5, 0xe7, 0xff)
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[2]));
                    StockChartX1.Gridcolor = !Properties.Settings.Default.GridColorOverwrite
                                                 ? Color.SkyBlue
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[2]));
                    StockChartX1.ChartForeColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                      ? Color.Black
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));
                    StockChartX1.UpColor = !Properties.Settings.Default.UpColorOverwrite
                                               ? Color.Lime
                                               : Color.FromArgb(
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[0]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[1]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[2]));
                    StockChartX1.DownColor = !Properties.Settings.Default.DownColorOverwrite
                                                 ? Color.Red
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[2]));
                    StockChartX1.HorizontalSeparatorColor = !Properties.Settings.Default.PainelSeparatorColorOverwrite
                                                                ? Color.SkyBlue
                                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[0]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[1]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[2]));
                    MSelectionBorderColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                ? Color.Blue
                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));

                    if (Properties.Settings.Default.CandleBorderColorOverwrite)
                    {
                        Color colorCandle = Color.FromArgb(int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[0]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[1]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[2]));

                        StockChartX1.CandleUpOutlineColor = colorCandle;
                        StockChartX1.CandleDownOutlineColor = colorCandle;
                        StockChartX1.WickUpColor = colorCandle;
                        StockChartX1.WickDownColor = colorCandle;
                    }
                    else
                    {
                        StockChartX1.CandleUpOutlineColor = Color.Black;
                        StockChartX1.CandleDownOutlineColor = Color.Black;
                        StockChartX1.WickUpColor = Color.Black;
                        StockChartX1.WickDownColor = Color.Black;
                    }

                    break;
                case "Office2007Silver":

                    if (Properties.Settings.Default.AssociateGradient)
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    }
                    else
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientTopOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[2]));
                    }


                    StockChartX1.BackGradientBottom = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                          ? Color.Silver
                                                          : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    StockChartX1.ChartBackColor = !Properties.Settings.Default.ChartBackColorOverwrite
                                                      ? Color.DarkGray
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[2]));
                    StockChartX1.Gridcolor = !Properties.Settings.Default.GridColorOverwrite
                                                 ? Color.SkyBlue
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[2]));
                    StockChartX1.ChartForeColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                      ? Color.Black
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));
                    StockChartX1.UpColor = !Properties.Settings.Default.UpColorOverwrite
                                               ? Color.Lime
                                               : Color.FromArgb(
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[0]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[1]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[2]));
                    StockChartX1.DownColor = !Properties.Settings.Default.DownColorOverwrite
                                                 ? Color.Red
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[2]));
                    StockChartX1.HorizontalSeparatorColor = !Properties.Settings.Default.PainelSeparatorColorOverwrite
                                                                ? Color.White
                                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[0]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[1]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[2]));
                    MSelectionBorderColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                ? Color.Red
                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));

                    if (Properties.Settings.Default.CandleBorderColorOverwrite)
                    {
                        Color colorCandle = Color.FromArgb(int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[0]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[1]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[2]));

                        StockChartX1.CandleUpOutlineColor = colorCandle;
                        StockChartX1.CandleDownOutlineColor = colorCandle;
                        StockChartX1.WickUpColor = colorCandle;
                        StockChartX1.WickDownColor = colorCandle;
                    }
                    else
                    {
                        StockChartX1.CandleUpOutlineColor = Color.Black;
                        StockChartX1.CandleDownOutlineColor = Color.Black;
                        StockChartX1.WickUpColor = Color.Black;
                        StockChartX1.WickDownColor = Color.Black;
                    }

                    break;
                case "WindowsVista":

                    if (Properties.Settings.Default.AssociateGradient)
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                      ? Color.DarkGray
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    }
                    else
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientTopOverwrite
                                                       ? Color.DarkGray
                                                       : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[0]),
                                                                        int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[1]),
                                                                        int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[2]));
                    }

                    StockChartX1.BackGradientBottom = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                          ? Color.Black
                                                          : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    StockChartX1.ChartBackColor = !Properties.Settings.Default.ChartBackColorOverwrite
                                                      ? Color.Black
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[2]));
                    StockChartX1.Gridcolor = !Properties.Settings.Default.GridColorOverwrite
                                                 ? Color.Gray
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[2]));
                    StockChartX1.ChartForeColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));
                    StockChartX1.UpColor = !Properties.Settings.Default.UpColorOverwrite
                                               ? Color.Blue
                                               : Color.FromArgb(
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[0]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[1]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[2]));
                    StockChartX1.DownColor = !Properties.Settings.Default.DownColorOverwrite
                                                 ? Color.Red
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[2]));
                    StockChartX1.HorizontalSeparatorColor = !Properties.Settings.Default.PainelSeparatorColorOverwrite
                                                                ? Color.Gray
                                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[0]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[1]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[2]));
                    MSelectionBorderColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                ? Color.White
                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));

                    if (Properties.Settings.Default.CandleBorderColorOverwrite)
                    {
                        Color colorCandle = Color.FromArgb(int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[0]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[1]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[2]));

                        StockChartX1.CandleUpOutlineColor = colorCandle;
                        StockChartX1.CandleDownOutlineColor = colorCandle;
                        StockChartX1.WickUpColor = colorCandle;
                        StockChartX1.WickDownColor = colorCandle;
                    }
                    else
                    {
                        StockChartX1.CandleUpOutlineColor = Color.Black;
                        StockChartX1.CandleDownOutlineColor = Color.Black;
                        StockChartX1.WickUpColor = Color.Black;
                        StockChartX1.WickDownColor = Color.Black;
                    }

                    StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".close", Color.FromArgb(0xFF, 0xff, 0xff, 0xff).ToArgb());
                    break;
                default:

                    if (Properties.Settings.Default.AssociateGradient)
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                       int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    }
                    else
                    {
                        StockChartX1.BackGradientTop = !Properties.Settings.Default.BackGradientTopOverwrite
                                                       ? Color.White
                                                       : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[0]),
                                                                        int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[1]),
                                                                        int.Parse(Properties.Settings.Default.BackGradientTop.Split(',')[2]));
                    }

                    StockChartX1.BackGradientBottom = !Properties.Settings.Default.BackGradientBottomOverwrite
                                                          ? Color.White
                                                          : Color.FromArgb(int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[0]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[1]),
                                                                           int.Parse(Properties.Settings.Default.BackGradientBottom.Split(',')[2]));
                    StockChartX1.ChartBackColor = !Properties.Settings.Default.ChartBackColorOverwrite
                                                      ? Color.White
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ChartBackColor.Split(',')[2]));
                    StockChartX1.Gridcolor = !Properties.Settings.Default.GridColorOverwrite
                                                 ? Color.Silver
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.GridColor.Split(',')[2]));
                    StockChartX1.ChartForeColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                      ? Color.Black
                                                      : Color.FromArgb(
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                          int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));
                    StockChartX1.UpColor = !Properties.Settings.Default.UpColorOverwrite
                                               ? Color.Lime
                                               : Color.FromArgb(
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[0]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[1]),
                                                   int.Parse(Properties.Settings.Default.UpColor.Split(',')[2]));
                    StockChartX1.DownColor = !Properties.Settings.Default.DownColorOverwrite
                                                 ? Color.Red
                                                 : Color.FromArgb(
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[0]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[1]),
                                                     int.Parse(Properties.Settings.Default.DownColor.Split(',')[2]));
                    StockChartX1.HorizontalSeparatorColor = !Properties.Settings.Default.PainelSeparatorColorOverwrite
                                                                ? Color.Silver
                                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[0]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[1]),
                                                                                 int.Parse(Properties.Settings.Default.HorizontalSeparatorColor.Split(',')[2]));
                    MSelectionBorderColor = !Properties.Settings.Default.ScaleColorOverwrite
                                                ? Color.Blue
                                                : Color.FromArgb(int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[0]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[1]),
                                                                 int.Parse(Properties.Settings.Default.ScaleColor.Split(',')[2]));

                    if (Properties.Settings.Default.CandleBorderColorOverwrite)
                    {
                        Color colorCandle = Color.FromArgb(int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[0]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[1]),
                                                                  int.Parse(Properties.Settings.Default.CandleBorderColor.Split(',')[2]));

                        StockChartX1.CandleUpOutlineColor = colorCandle;
                        StockChartX1.CandleDownOutlineColor = colorCandle;
                        StockChartX1.WickUpColor = colorCandle;
                        StockChartX1.WickDownColor = colorCandle;
                    }
                    else
                    {
                        StockChartX1.CandleUpOutlineColor = Color.Black;
                        StockChartX1.CandleDownOutlineColor = Color.Black;
                        StockChartX1.WickUpColor = Color.Black;
                        StockChartX1.WickDownColor = Color.Black;
                    }

                    break;
            }
            StockChartX1.Update();
        }

        //Shows an open-file dialog
        public string OpenDialog()
        {
            return OpenDialog("");
        }
        //Shows an open-file dialog
        public string OpenDialog(string Filter)
        {
            OpenFileDialog flOpenDialog = new OpenFileDialog
            {
                Filter = string.IsNullOrEmpty(Filter) ? "Stock Chart Files|*.icx" : Filter,
                Title = "Open",
                CheckFileExists = true,
                InitialDirectory =
                  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            };
            flOpenDialog.ShowDialog();
            return flOpenDialog.FileName;
        }

        //Shows a save-file dialog
        public string SaveDialog()
        {
            return SaveDialog("");
        }
        //Shows a save-file dialog
        public string SaveDialog(string Filter)
        {
            SaveFileDialog flSaveDialog = new SaveFileDialog
            {
                Filter = string.IsNullOrEmpty(Filter) ? "Stock Chart Files|*.icx" : Filter,
                Title = "Save",
                CheckFileExists = false,
                InitialDirectory =
                  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            };
            flSaveDialog.ShowDialog();
            return flSaveDialog.FileName;
        }

        public Color GetColor()
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.ShowDialog();
            colorDlg.Dispose();
            return colorDlg.Color;
        }

        //Saves a chart as a PNG image
        public void SaveChartImage()
        {
            string bmpName = SaveDialog("PNG Images|*.png");
            bmpName = bmpName.Substring(0, bmpName.Length - 4) + ".bmp";
            StockChartX1.SaveChartBitmap(bmpName);
            string pngName = bmpName.Substring(0, bmpName.Length - 4) + ".png";
            ConvertBMP(bmpName, pngName);
        }

        //Convert the StockChartX bmp into a png (can be changed to gif)
        private static void ConvertBMP(string BMPName, string PNGName)
        {
            try
            {
                Bitmap bm = new Bitmap(BMPName);
                bm.Save(PNGName, ImageFormat.Png);
                bm.Dispose();
                File.Delete(BMPName);
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        //Returns historic data from StockChartX
        public DataManager.BarData[] GetDataFromChart()
        {
            List<DataManager.BarData> bars = new List<DataManager.BarData>();
            string Symbol = StockChartX1.Symbol;
            int count = StockChartX1.RecordCount;
            for (int n = 1; n <= count; n++)
            {
                try
                {
                    DataManager.BarData bar = new DataManager.BarData
                    {
                        TradeDate =
                          Convert.ToDateTime(
                          StockChartX1.FromJulianDate(StockChartX1.GetJDate(Symbol + ".Close", n))),
                        OpenPrice = StockChartX1.GetValue(Symbol + ".Open", n),
                        HighPrice = StockChartX1.GetValue(Symbol + ".High", n),
                        LowPrice = StockChartX1.GetValue(Symbol + ".Low", n),
                        ClosePrice = StockChartX1.GetValue(Symbol + ".Close", n),
                        Volume = StockChartX1.GetValue(Symbol + ".Volume", n)
                    };
                    bars.Add(bar);
                }
                catch (Exception) { }
            }
            return bars.ToArray();
        }
        #endregion

        #region Real Time Charting Data Support
        private bool InitRTChartHeader()
        {
            if (_mCtlData == null) return false;

            StockChartX1.Symbol = MSymbol;

            //First setup the chart for real time data
            StockChartX1.RemoveAllSeries();
            StockChartX1.RealTimeXLabels = true;

            //First add a panel (chart area) for the OHLC data:
            long panel = StockChartX1.AddChartPanel();

            //Now add the open, high, low and close series to that panel:
            StockChartX1.AddSeries(MSymbol + ".open", SeriesType.stCandleChart, (int)panel);
            StockChartX1.AddSeries(MSymbol + ".high", SeriesType.stCandleChart, (int)panel);
            StockChartX1.AddSeries(MSymbol + ".low", SeriesType.stCandleChart, (int)panel);
            StockChartX1.AddSeries(MSymbol + ".close", SeriesType.stCandleChart, (int)panel);

            //Change the color:
            StockChartX1.set_SeriesColor(MSymbol + ".close", ColorTranslator.ToOle(Color.White));

            //Add the volume chart panel
            panel = StockChartX1.AddChartPanel();
            StockChartX1.AddSeries(MSymbol + ".volume", SeriesType.stVolumeChart, (int)panel);

            //Change volume color and weight of the volume panel:
            StockChartX1.set_SeriesColor(MSymbol + ".volume", ColorTranslator.ToOle(Color.Blue));
            StockChartX1.set_SeriesWeight(MSymbol + ".volume", 3);

            //Resize the volume panel to make it smaller

            int chartH = StockChartX1.Height;

            if (chartH > Height)
                StockChartX1.Height = Height - 40;

            StockChartX1.set_PanelY1(1, (int)Math.Round(StockChartX1.Height * 0.8));

            StockChartX1.UpColor = Color.Green;
            StockChartX1.DownColor = Color.Red;

            return true;
        }

        private void InitRTChartFooter(IList<M4.DataServer.Interface.BarData> bars)
        {
            Stopwatch sw = new Stopwatch();
            double prevJDate = 0;
            sw.Start();
            foreach (M4.DataServer.Interface.BarData t in bars)
            {
                if (sw.ElapsedMilliseconds > 100)
                {
                    //loading 50.000 on main thread may freeze app. Can't append data into chart in a separate thread cause chart is created in main GUI thread
                    Application.DoEvents();
                    sw.Reset();
                }
                double jdate = StockChartX1.ToJulianDate(t.TradeDate.Year, t.TradeDate.Month,
                                                         t.TradeDate.Day, t.TradeDate.Hour,
                                                         t.TradeDate.Minute, t.TradeDate.Second);
                if (jdate != prevJDate)
                {
                    prevJDate = jdate;
                    StockChartX1.AppendValue(MSymbol + ".open", jdate, t.OpenPrice);
                    StockChartX1.AppendValue(MSymbol + ".high", jdate, t.HighPrice);
                    StockChartX1.AppendValue(MSymbol + ".low", jdate, t.LowPrice);
                    StockChartX1.AppendValue(MSymbol + ".close", jdate, t.ClosePrice);
                    StockChartX1.AppendValue(MSymbol + ".volume", jdate, t.VolumeF);
                    Application.DoEvents();
                }
            }
            Debug.WriteLine("Last Values " + bars.Last());

            if (bars[0].VolumeF == 0.0) StockChartX1.RemoveSeries(MSymbol + ".volume");
            StockChartX1.RealTimeXLabels = true;
            StockChartX1.ThreeDStyle = true;
            StockChartX1.HorizontalSeparators = true;
            StockChartX1.DisplayTitles = true;

            UpdateChartColors(MMain.m_Style);
            UpdateYScale();

            if (StockChartX1.RecordCount > 100)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.RecordCount - 100;
            }

            StockChartX1.Update();

            //if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
            //{
            //    m_Main.mnuViewScaleType.Checked = false;
            //    m_Main.mnuViewScaleType.Enabled = false;
            //}
            //else
            //{
            //    m_Main.mnuViewScaleType.Enabled = true;
            //}
            if ((MPeriodicity != Periodicity.Daily) && (MPeriodicity != Periodicity.Weekly))
            {
                ShowNewSessions(bars);
            }
            StockChartX1.Visible = true;

            EnableControls(true);
        }

        public void InitRTChartAsync(Action<bool> afterChartLoaded)
        {
            if (!InitRTChartHeader()) return;

            Utils.Trace("Start historical request");
            _mCtlData.GetHistoryAsync(MSymbol, this, MPeriodicity, MBarSize, MBars,
              answer =>
              {
                  Utils.Trace("History received. HasError = " + answer.HasError);
                  if (answer.HasError || answer.Data.Count < 3)
                  {
                      _asyncOp.Post(() => afterChartLoaded(false));
                      return;
                  }

                  Utils.Trace("Add bardata to chart");
                  //_asyncOp.Post(() => InitRTChartFooter(answer.Data));
                  Action a = () =>
                  {
                      InitRTChartFooter(answer.Data);
                      Utils.Trace("After chart loaded");
                      afterChartLoaded(true);
                  };
                  BeginInvoke(a);
              });
        }

        private void InitRTChart()
        {/*

            if (!InitRTChartHeader()) return;

            List<DataManager.BarData> bars = _mCtlData.GetHistory(MSymbol, this, MPeriodicity, MBarSize, MBars);
            if (bars.Count < 3) return;

            InitRTChartFooter(bars);*/
        }

        private void ShowNewSessions(IList<M4.DataServer.Interface.BarData> bars)
        {
            for (int n = 1; n < bars.Count; n++)
            {
                if (bars[n].TradeDate.Day == bars[n - 1].TradeDate.Day) continue;

                StockChartX1.AddSymbolObject(0, 0.0, n, SymbolType.soSignalSymbolObject, Convert.ToString(bars[n].TradeDate.Day), "New Session");
                StockChartX1.set_ObjectSelectable(ObjectType.otSignalSymbolObject, Convert.ToString(bars[n].TradeDate.Day), false);
            }
        }


        //Optional: draw a small symbol wherever a there is a new trading day
        private void ShowNewSessions()
        {
            try
            {
                int count = StockChartX1.RecordCount;
                string symbol = MSymbol + ".close";
                for (int n = 2; n <= count; n++)
                {
                    //          if (n <= 0) continue;
                    //          DateTime date1 = DateTime.Parse(StockChartX1.FromJulianDate(StockChartX1.GetJDate(symbol, n)),
                    //                                          usCulture.DateTimeFormat);
                    //          DateTime date2 =
                    //            DateTime.Parse(StockChartX1.FromJulianDate(StockChartX1.GetJDate(symbol, n - 1)),
                    //                           usCulture.DateTimeFormat);
                    DateTime date1 = DateTimeEx.FromJDate(StockChartX1.GetJDate(symbol, n));
                    DateTime date2 = DateTimeEx.FromJDate(StockChartX1.GetJDate(symbol, n - 1));
                    if (date1.Day == date2.Day) continue;

                    StockChartX1.AddSymbolObject(0, 0.0, n, SymbolType.soSignalSymbolObject, Convert.ToString(date1.Day), "New Session");
                    StockChartX1.set_ObjectSelectable(ObjectType.otSignalSymbolObject, Convert.ToString(date1.Day), false);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        //Returns the chart selection for this instance
        public ChartSelection GetChartSelection()
        {
            ChartSelection ret = new ChartSelection
            {
                Symbol = MSymbol,
                Periodicity = MPeriodicity,
                Interval = MBarSize,
                Bars = MBars
            };
            return ret;
        }


        private void LogFile(string item, string function)
        {
            StreamWriter SW = File.AppendText(Application.StartupPath + @"\" + function + ".txt");
            SW.WriteLine(item);
            SW.Close();
        }


        //Realtime updates 		
        public void PriceUpdate(string Symbol, DateTime TradeDate, double LastPrice, long Volume)
        {
            // No longer used. See BarUpdate below.

            /*
      
                  // DEBUG
      #if DEBUG
                  LogFile(TradeDate + "\t" + Symbol + "\t" + LastPrice, "PriceUpdate");
      #endif

                  //We cannot update the chart if the user is editing it
                  if (StockChartX1.UserEditing || m_UserEditing)
                  {
                      RTCache cache = new RTCache { Symbol = Symbol, TradeDate = TradeDate, LastPrice = LastPrice, Volume = Volume };
                      m_RTCache.Add(cache);
                      return;
                  }
                  if (m_RTCache.Count > 0) //Empty the cache
                  {
                      if (!emptyingCache)
                      {
                          emptyingCache = true;
                          int count = m_RTCache.Count - 1;
                          for (int n = 0; n <= count; n++)
                          {
                              PriceUpdate(m_RTCache[n].Symbol, m_RTCache[n].TradeDate, m_RTCache[n].LastPrice,
                                                      m_RTCache[n].Volume);
                          }
                          m_RTCache.Clear();
                          emptyingCache = false;
                      }
                      else
                      {
                          return;
                      }
                  }
            */
        }

        //Bar udpates (from what we requested in GetHistory)
        private static bool emptyingCacheBarUpdate;
        public void BarUpdate(string Symbol, Periodicity BarType, int BarSize, M4.DataServer.Interface.BarData Bar, bool IsNewBar)
        {
            if (!RealTimeUpdates) return;

            if (MClosing) return; // The chart is being closed

            if (BarSize != MBarSize && BarType != MPeriodicity) return;

            if (StockChartX1.UserEditing || MUserEditing)
            {
                BarCache cache = new BarCache { Symbol = Symbol, BarType = BarType, Bar = Bar, IsNewBar = IsNewBar };
                _mBarCache.Add(cache);
                return;
            }
            if (_mBarCache.Count > 10000) _mBarCache.Clear(); // Must be in debug mode
            if (_mBarCache.Count > 0)
            {
                lock (_mBarCache)
                {
                    if (!emptyingCacheBarUpdate)
                    {
                        emptyingCacheBarUpdate = true;
                        foreach (BarCache cache in _mBarCache)
                        {
                            BarUpdate(cache.Symbol, cache.BarType, BarSize, cache.Bar, cache.IsNewBar);
                            if (MClosing) return;
                        }
                        _mBarCache.Clear();
                        emptyingCacheBarUpdate = false;
                        StockChartX1.Update();
                    }
                }
            }



            if (IsNewBar)
            {

                // DEBUG
                /*
        #if DEBUG
                double o = StockChartX1.GetValue(m_Symbol + ".open", StockChartX1.RecordCount);
                double h = StockChartX1.GetValue(m_Symbol + ".high", StockChartX1.RecordCount);
                double l = StockChartX1.GetValue(m_Symbol + ".low", StockChartX1.RecordCount);
                double c = StockChartX1.GetValue(m_Symbol + ".close", StockChartX1.RecordCount);
                LogFile(StockChartX1.FromJulianDate(StockChartX1.GetJDate(m_Symbol + ".close", StockChartX1.RecordCount)) +
                    "\t" + Symbol + "\t" + o + "\t" + h + "\t" + l + "\t" + c, "BarUpdate");
        #endif
                 */

                double jdate = StockChartX1.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
                                Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second);
                StockChartX1.AppendValue(MSymbol + ".open", jdate, Bar.OpenPrice);
                StockChartX1.AppendValue(MSymbol + ".high", jdate, Bar.HighPrice);
                StockChartX1.AppendValue(MSymbol + ".low", jdate, Bar.LowPrice);
                StockChartX1.AppendValue(MSymbol + ".close", jdate, Bar.ClosePrice);
                StockChartX1.AppendValue(MSymbol + ".volume", jdate, Bar.VolumeF);

                // For pattern recognition
                foreach (string series in _mPatternSeries)
                    StockChartX1.AppendValue(series, jdate, -987654321);

                // For neural network (does not process in real time, too CPU intensive)        
                StockChartX1.AppendValue("Neural Network", jdate, -987654321);

            }
            else
            {
                double jdate = StockChartX1.GetJDate(MSymbol + ".close", StockChartX1.RecordCount);
                StockChartX1.EditValue(MSymbol + ".open", jdate, Bar.OpenPrice);
                StockChartX1.EditValue(MSymbol + ".high", jdate, Bar.HighPrice);
                StockChartX1.EditValue(MSymbol + ".low", jdate, Bar.LowPrice);
                StockChartX1.EditValue(MSymbol + ".close", jdate, Bar.ClosePrice);
                StockChartX1.EditValue(MSymbol + ".volume", jdate, Bar.VolumeF);
                StockChartX1.EditJDate(StockChartX1.RecordCount, jdate);
            }

            if (!emptyingCacheBarUpdate) StockChartX1.Update();

            // Chart updated, now update the expert advisors

            foreach (ExpertAdvisorAlert t in _expertAdvisors)
            {
                double j = 0, o = 0, h = 0, l = 0, c = 0;
                int v = 0;

                // Buy script
                Alert oAlert = t.buyAlert;
                if (oAlert.GetRecordByIndex(oAlert.RecordCount, ref j, ref o, ref h, ref l, ref c, ref v))
                {

                    if (IsNewBar)
                    {
                        // Append new bar
                        double jDate = StockChartX1.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
                                                                 Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second);
                        oAlert.AppendRecord(jDate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
                    }
                    else
                    {   // Edit existing bar
                        oAlert.EditRecord(j, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
                    }
                }

                // Sell script
                oAlert = t.sellAlert;
                if (oAlert.GetRecordByIndex(oAlert.RecordCount, ref j, ref o, ref h, ref l, ref c, ref v))
                {

                    if (IsNewBar)
                    {
                        // Append new bar     
                        double jDate = StockChartX1.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
                                                                 Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second);
                        oAlert.AppendRecord(jDate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
                    }
                    else
                    {   // Edit existing bar
                        oAlert.EditRecord(j, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
                    }
                }

                // DEBUG:
                /*
                // It is normal for the first bar after a historic request to be missing.
                // The data will automatically be brought up to date as new ticks come in.
                string data = "";
                for (int k = 0; k < oAlert.RecordCount; ++k)
                {
                  oAlert.GetRecordByIndex(k + 1, ref j, ref o, ref h, ref l, ref c, ref v);
                  string date = StockChartX1.FromJulianDate(j);
                  data += date + "\t" + o + "\t" + h + "\t" + l + "\t" + c + "\t" + v + "\r\n";
                }
                Clipboard.Clear();
                Clipboard.SetText(data);
                */
            }


        }


        public IntPtr GetHandle()
        {
            return IsHandleCreated ? Handle : IntPtr.Zero;
        }

        #endregion

        #region Real Time Charting UI Support
        private void StockChartX1_OnLButtonDown(object sender, EventArgs e)
        {
            MUserEditing = true;
        }

        private void StockChartX1_OnLButtonUp(object sender, EventArgs e)
        {
            tmrEdit.Enabled = true;
        }

        private void StockChartX1HideDialog(object sender, EventArgs e)
        {
            MUserEditing = false;
            _mDialogShown = false;
        }

        private void StockChartX1ShowDialog(object sender, EventArgs e)
        {
            MUserEditing = true;
            _mDialogShown = true;
        }

        private void TmrEditTick(object sender, EventArgs e)
        {
            if (DrawingLineStudy && ((StockChartX1.GetObjectCount((ObjectType)(-1)) != _mLastObjectCount) && !StockChartX1.UserEditing))
            {
                DrawingLineStudy = false;
            }
            if (_mDialogShown) return;
            MUserEditing = false;
            tmrEdit.Enabled = false;
        }

        //Draw a selection border around this chart only if other charts are visible
        //because the user must know which chart is ready for input from frmMain.
        public void DrawSelection()
        {
            //Remove selection border from all charts
            const int cnt = 0;
            //foreach (NUIDocument doc in m_Main.m_DockManager.DocumentManager.Documents)
            //{
            //    if (doc.Client.Name != "TelerickCtlChart") continue;
            //    TelerickCtlChart chart = (TelerickCtlChart)doc.Client;
            //    chart.BackColor = m_Main.BackColor;
            //    if (chart.Visible)
            //    {
            //        cnt++;
            //    }
            //}
            if (cnt > 1)
            {
                BackColor = MSelectionBorderColor;
            }
        }

        //Updates m_Main's menus based on this chart
        public void UpdateMenus()
        {
            try
            {
                //m_Main.mnuViewShowXGrid.Checked = StockChartX1.XGrid;
                //m_Main.mnuViewYGrid.Checked = StockChartX1.YGrid;
                //m_Main.mnuViewCrosshair.Checked = false;
                //m_Main.mnuViewSeparators.Checked = StockChartX1.HorizontalSeparators;
                //m_Main.mnuView3D.Checked = StockChartX1.ThreeDStyle;
                //m_Main.mnuDarvasBoxes.Checked = StockChartX1.DarvasBoxes;
                //m_Main.mnuViewScaleType.Checked = StockChartX1.ScaleType == ScaleType.stLinearScale;
            }
            catch (Exception)
            { }
        }

        private void StockChartX1PaintEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_PaintEvent e)
        {
            StockChartX1.ShowLastTick(StockChartX1.Symbol + ".close", StockChartX1.GetValue(StockChartX1.Symbol + ".close", StockChartX1.RecordCount));
        }

        #endregion

        #region Advanced Pattern Recogntiion - Patent Pending

        // NOTICE: APR IS PATENT-PENDING.


        private readonly List<string> _mPatterns = new List<string>();
        private readonly List<Ohlcv> _data = new List<Ohlcv>();

        private bool LoadPatternData()
        {
            _data.Clear();

            for (int n = 1; n < StockChartX1.RecordCount; ++n)
            {

                double jdate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", n);
                DateTime dt;
                DateTime.TryParse(StockChartX1.FromJulianDate(jdate), out dt);

                Ohlcv value = new Ohlcv
                {
                    Date = dt,
                    Open = StockChartX1.GetValue(StockChartX1.Symbol + ".open", n),
                    High = StockChartX1.GetValue(StockChartX1.Symbol + ".high", n),
                    Low = StockChartX1.GetValue(StockChartX1.Symbol + ".low", n),
                    Close = StockChartX1.GetValue(StockChartX1.Symbol + ".close", n),
                    Volume = (int)StockChartX1.GetValue(StockChartX1.Symbol + ".volume", n)
                };
                _data.Add(value);
            }
            return _data.Count > 0;
        }

        private bool _patternScannerRunning;


        private void ScanForPatterns(string xmlFileName, string displayName, Color PatternColor)
        {
            ProgressWorkerParams workerParams = new ProgressWorkerParams { Alignment = ContentAlignment.BottomLeft, AllowCancel = true, ControlAnchor = this, Modal = true, };
            ProgressWorker.Run(
              workerParams,
              visualizer =>
              {
                  try
                  {
                      visualizer.SetProgressTitle(Title);
                      visualizer.SetProgressAction("Initalizing...");

                      PatternRecognizer r = new PatternRecognizer();
                      bool first = true;
                      r.ProgressCallback += (object o, int records, int record, ref bool stop) =>
                        {
                            if (first)
                            {
                                first = false;
                                visualizer.InitProgress(0, records);
                            }
                            visualizer.ReportProgress(record);
                            stop = visualizer.CancelReqested;
                        };

                      //Initialize Pattern Recognizer
                      r.Init();
                      //Add data to Pattern Recognizer

                      visualizer.SetProgressAction("Append Data...");
                      r.AppendRecords(_data);

                      //scan for patterns
                      visualizer.SetProgressAction("Scanning...");
                      int count = r.Scan(xmlFileName, "{2E036561-F762-471d-93FD-869AFE438639}");
                      if (count == -1)
                      {
                          //_mCtlData.OutputWindow1.DisplayAlertOrMessage(
                          //  "XML pattern definition for " + displayName + " not found", OutputWindow.OutputIcon.Warning);
                          return;
                      }

                      if (count == -100)
                      {
                          //_mCtlData.OutputWindow1.DisplayAlertOrMessage("Wrong APR license key", OutputWindow.OutputIcon.Warning);
                          return;
                      }

                      //Exit if no patterns found            
                      //LogEvent(r.ResultCount + " patterns detected for " + displayName);
                      if (r.ResultCount < 1 || visualizer.CancelReqested) return; // No patterns found

                      //Patterns were found, so add them to the chart.
                      visualizer.SetProgressAction("Initializing pattern...");
                      //_asyncOp.Post(() => InitializePattern(r.Title, PatternColor));
                      InitializePattern(r.Title, PatternColor);

                      visualizer.SetProgressAction("Plotting found patterns...");
                      visualizer.InitProgress(0, r.ResultCount);

                      int i = 0;
                      StockChartX1.Freeze(true);
                      ModulusFE.APR.PatternValue prev = null;
                      foreach (var patternValue in r.Results)
                      {
                          if (prev != null)
                          { // Optional: filter extra patterns that are too close together
                              if (patternValue.Interval.x - prev.Interval.x < 5)
                                  continue;
                          }
                          prev = patternValue;

                          visualizer.ReportProgress(i);
                          if (visualizer.CancelReqested)
                          {
                              break;
                          }

                          i = i + 1;
                          int n = (int)patternValue.Interval.x;
                          foreach (PatternBound bound in patternValue.Bound)
                          {
                              n = n + 1;
                              double jDate = this.StockChartX1.GetJDate(this.StockChartX1.Symbol + ".close", n);
                              StockChartX1.EditValue("Top Pattern", jDate, bound.Upper);
                              StockChartX1.EditValue("Bottom Pattern", jDate, bound.Lower);
                          }

                          //Add an icon with the pattern name at the start of the pattern
                          StockChartX1.RemoveObject(ObjectType.otSignalSymbolObject, r.Title + i);
                          StockChartX1.AddSymbolObject(
                            0,
                            StockChartX1.GetValue(StockChartX1.Symbol + ".close", (int)patternValue.Interval.x + 1),
                            (int)patternValue.Interval.x + 1,
                            SymbolType.soSignalSymbolObject,
                            r.Title + i,
                            r.Title + " " + i + " (ranking: " + Math.Round(patternValue.Ranking * 100, 0) + "%)");

                          //Add a text object
                          StockChartX1.RemoveObject(ObjectType.otStaticTextObject, r.Title + i + " Text");

                          UInt32 color = Convert.ToUInt32(ColorTranslator.ToOle(PatternColor));

                          StockChartX1.AddStaticText(
                            0,
                            r.Title + " " + i + " ranking: " + Math.Round(patternValue.Ranking * 100, 0) + "%",
                            r.Title + i + " Text",
                            color,
                            true,
                            StockChartX1.GetXPixel((int)patternValue.Interval.x + 1),
                            StockChartX1.GetYPixel(
                              0, StockChartX1.GetValue(StockChartX1.Symbol + ".close", (int)patternValue.Interval.x + 1)));

                          if (i > 100)
                          {
                              MessageBox.Show(
                                "Too many patterns, please refine your search", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                              //_mCtlData.OutputWindow1.DisplayAlertOrMessage(
                              //  "Too many patterns found, please refine your search", OutputWindow.OutputIcon.Warning);
                              break;
                          }
                      }

                      //m_Main.ShowStatus(i + " " + r.Title + " patterns added to chart");
                  }
                  catch (Exception ex)
                  {
                      _asyncOp.Post(
                        () =>
                        MessageBox.Show(
                          "An error occured while scaning. Error :" + Environment.NewLine + ex.Message,
                          "APR Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error));
                  }
                  finally
                  {
                      _patternScannerRunning = false;
                      StockChartX1.Freeze(false);
                      StockChartX1.Update();
                  }
              },
              () =>
              { });
        }



        //Modulus Advanced Pattern Recognition library
        //NOTE: A pattern designer utility is included in the Examples\APR\ directory.
        //This library is licensed separately. Refer to www.modulusfe.com/apr/

        //Removes old patterns from the chart
        private void RemovePatterns(string PatternName)
        {
            int n = 0;
            for (n = 0; n <= _mPatterns.Count - 1; n++)
            {
                if (_mPatterns[n] == PatternName)
                {
                    _mPatterns.RemoveAt(n);
                    break;
                }
            }
        }


        //Modulus Advanced Pattern Recognition library
        //NOTE: A pattern designer utility is included in the Examples\APR\ directory.
        //This library is licensed separately. Refer to www.modulusfe.com/apr/

        //Clears the old results and adds two new series with null values
        private void InitializePattern(string PatternName, Color PatternColor)
        {

            foreach (string series in _mPatternSeries)
                StockChartX1.RemoveSeries(series);

            StockChartX1.ClearDrawings();
            StockChartX1.ResetZoom();
            StockChartX1.ForcePaint();

            _mPatterns.Clear();
            _mPatternSeries.Clear();
            _mPatternSeries.Add("Top Pattern");
            _mPatternSeries.Add("Bottom Pattern");
            _mPatterns.Add(PatternName);

            StockChartX1.AddSeries("Top Pattern", SeriesType.stLineChart, 0);
            StockChartX1.AddSeries("Bottom Pattern", SeriesType.stLineChart, 0);
            StockChartX1.set_SeriesColor("Top Pattern", ColorTranslator.ToOle(PatternColor));
            StockChartX1.set_SeriesColor("Bottom Pattern", ColorTranslator.ToOle(PatternColor));

            int i;
            for (i = 1; i <= StockChartX1.RecordCount; i++)
            {
                double jDate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", i);
                StockChartX1.AppendValue("Top Pattern", jDate, (double)DataType.dtNullValue);
                StockChartX1.AppendValue("Bottom Pattern", jDate, (double)DataType.dtNullValue);
            }

            StockChartX1.Update();
        }



        //TODO: The Modulus Advanced Pattern Recognition feature requires
        //the Modulus APR dll to be installed on the client.
        //In addition, the Pattern Designer must also be installed.
        //The Pattern Designer requires the MS .NET 3.5 runtime.        
        public void RunPatternRecognition()
        {
            if (_patternScannerRunning)
            {
                MessageBox.Show(
                  "Scanning in progress. Wait until done.",
                  "Already scanning",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
                return;
            }

            if (StockChartX1.RecordCount > 5000)
            {
                MessageBox.Show("Too many bars loaded, please retry on a chart with fewer than 5,000 bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmPatternRecognition apr = new frmPatternRecognition();
            string file = apr.GetPatternDefinitionFile();
            if (file != "")
            {
                string pattern = file;
                short found = (short)file.LastIndexOf(@"\");
                pattern = pattern.Substring(found + 1).Replace(".apr.xml", "");
                if (!LoadPatternData())
                {
                    MessageBox.Show("The Pattern Recognition plug-in is not installed!", "Error:", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    ScanForPatterns(file, pattern, Color.Blue);
                }
            }
        }





        #endregion

        #region Chart Trading Support

        //Removes chart orders 
        public void ClearOrders()
        {
            //Prompt the user 
            DialogResult result = MessageBox.Show("This action WILL clear all pending chart orders but will NOT close any existing open positions.", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            //Clear the objects from the chart 
            for (int n = 0; n <= m_Orders.Count - 1; n++)
            {
                StockChartX1.RemoveObject(ObjectType.otTrendLineObject, m_Orders[n].ChartObjectLineName);
                StockChartX1.RemoveObject(ObjectType.otTextObject, m_Orders[n].ChartObjectTextName);
                StockChartX1.RemoveObject(ObjectType.otBuySymbolObject, m_Orders[n].ChartObjectSymbolName);
                StockChartX1.RemoveObject(ObjectType.otSellSymbolObject, m_Orders[n].ChartObjectSymbolName);
            }

            //Clear the array 
            m_Orders.Clear();

        }


        //Manage chart orders 
        private void StockChartX1_TrendLinePenetration(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_TrendLinePenetrationEvent e)
        {

            //Find the corresponding order 
            int index = -1;
            for (int n = 0; n <= m_Orders.Count - 1; n++)
            {
                if (m_Orders[n].ChartObjectLineName == e.trendLineName)
                {
                    index = n;
                    break;
                }
            }

            if (e.seriesName != StockChartX1.Symbol + ".close") return;
            //Trade only on the last price 
            if (index == -1) return;
            //Must be a trend line watch unrelated to a chart order 
            if (m_Orders[index].Executed) return;
            //Order already executed 

            //Prepare to execute the order 
            int x = StockChartX1.GetXPixel(m_Orders[index].ChartRecord - StockChartX1.FirstVisibleRecord);
            int y = StockChartX1.GetYPixel(0, m_Orders[index].EntryPrice);

            //Optional: Only trade if the trend line was crossed from the reverse side using  & e.direction > 0
            //if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.LongSide & e.direction > 0) { 
            //Buy 
            if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.LongSide)
            {

                m_Orders[index].Executed = true;

                //Update the chart and order status 
                StockChartX1.AddSymbolObject(0, m_Orders[index].EntryPrice, StockChartX1.RecordCount, SymbolType.soBuySymbolObject, "Buy Order " + (index + 1), "Submitted Buy Order @ " + m_Orders[index].EntryPrice);
                StockChartX1.set_ObjectSelectable(ObjectType.otBuySymbolObject, "Buy Order " + (index + 1), false);

                m_Orders[index].ChartObjectSymbolName = "Buy Object " + (index + 1);

                StockChartX1.RemoveObject(ObjectType.otTextObject, m_Orders[index].ChartObjectTextName);

                StockChartX1.AddStaticText(0, "Buy Order (Qty " + m_Orders[index].Quantity + ") Submitted @ " + Math.Round(m_Orders[index].EntryPrice, 4), "Buy Order " + (index + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Green), false, x, y);

                SubmitMarketOrder(StockChartX1.Symbol, ctlPortfolio.Orders.Side.LongSide, m_Orders[index].Quantity);
            }

            //Sell 
            // & e.direction < 0 optional 
            else if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.ShortSide)
            {

                m_Orders[index].Executed = true;

                //Update the chart and order status 
                StockChartX1.AddSymbolObject(0, m_Orders[index].EntryPrice, StockChartX1.RecordCount, SymbolType.soSellSymbolObject, "Sell Order " + (index + 1), "Submitted Sell Order @ " + m_Orders[index].EntryPrice);

                StockChartX1.set_ObjectSelectable(ObjectType.otSellSymbolObject, "Sell Order " + (index + 1), false);

                m_Orders[index].ChartObjectSymbolName = "Sell Object " + (index + 1);

                StockChartX1.AddStaticText(0, "Sell Order (Qty " + m_Orders[index].Quantity + ") Submitted @ " + Math.Round(m_Orders[index].EntryPrice, 4), "Sell Order " + (index + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Red), false, x, y);

                StockChartX1.RemoveObject(ObjectType.otTextObject, m_Orders[index].ChartObjectTextName);


                SubmitMarketOrder(StockChartX1.Symbol, ctlPortfolio.Orders.Side.ShortSide, m_Orders[index].Quantity);
            }

        }

        #endregion

        #region Order Execution

        private static void SubmitMarketOrder(string symbol, ctlPortfolio.Orders.Side buySell, int quantity)
        {

            ctlPortfolio.Order myOrder = new ctlPortfolio.Order();
            //ctlPortfolio portfolio = m_Main.GetPortfolio();
            //if (portfolio == null) return;

            //Gather the order details 
            //MyOrder.OrderID = portfolio.CreateOrderID();
            //MyOrder.Side = BuySell;
            //MyOrder.Quantity = Quantity;
            //MyOrder.Exchange = "NASDAQ";
            ////TODO: Update for your order entry API 
            //MyOrder.Symbol = Symbol;
            //MyOrder.LimitPrice = 0;
            //MyOrder._Order = ctlPortfolio.Order.OrderType.Market;

            ////Ensure a portfolio is selected 
            //if (portfolio.cmbPortfolio.Text != cmbPortfolio.Text)
            //{
            //    for (Int16 n = 0; n <= portfolio.cmbPortfolio.Items.Count - 1; n++)
            //    {
            //        if (portfolio.cmbPortfolio.Items[n].Text == cmbPortfolio.Text)
            //        {
            //            portfolio.cmbPortfolio.SelectedIndex = n;
            //            break; // TODO: might not be correct. Was : Exit For 
            //        }
            //    }
            //}

            //#### TODO: WARNING! Example code only! Your order entry API is responsible 
            //for sending/receiving orders to update this control. This example just 
            //sends the order straight to the DataViewGrid control! Also the exec time 
            //and status should be set by the server. 
            //MyOrder.ExecTime = DateTime.Now;
            //MyOrder.Status = ctlPortfolio.Orders.Status.Sending;
            //portfolio.ExecuteOrder(MyOrder.OrderID, MyOrder.Status, MyOrder.Symbol, MyOrder.ExecTime, MyOrder.Side, MyOrder.Quantity,
            //  portfolio.GetLastPrice(MyOrder.Symbol), MyOrder._Order, MyOrder.Expires, MyOrder.LimitPrice
            //);

        }


        //Loads the list of available portfolios

        #endregion

        #region Expert Advisors

        public class ExpertAdvisorAlert
        {
            public ExpertAdvisor ea = null;
            public Alert buyAlert = null;
            public Alert sellAlert = null;
            public int prevAlert = 0;
            public DateTime lastBuyAlert = DateTime.MinValue;
            public DateTime lastSellAlert = DateTime.MinValue;
        }

        public List<ExpertAdvisorAlert> _expertAdvisors = new List<ExpertAdvisorAlert>();


        /// <summary>
        /// Adds an expert advisor.
        /// </summary>
        /// <param name="ea">The expert advisor.</param>
        public void AddExpertAdvisor(ExpertAdvisor ea)
        {
            RemoveExpertAdvisor(ea);

            ExpertAdvisorAlert eaa = new ExpertAdvisorAlert
                                         {
                                             lastBuyAlert = DateTime.MinValue,
                                             lastSellAlert = DateTime.MinValue,
                                             buyAlert = new Alert
                                                            {
                                                                AlertName = ea.Name,
                                                                AlertScript = ea.BuyScript,
                                                                Symbol = MSymbol,
                                                                License = "XRT93NQR79ABTW788XR48"
                                                            }
                                         };

            eaa.buyAlert.Alert += OnBuyAlert;
            eaa.buyAlert.ScriptError += ScriptError;

            eaa.sellAlert = new Alert
            {
                AlertName = ea.Name,
                AlertScript = ea.SellScript,
                Symbol = MSymbol,
                License = "XRT93NQR79ABTW788XR48"
            };
            eaa.sellAlert.Alert += OnSellAlert;
            eaa.sellAlert.ScriptError += ScriptError;

            // How much data needs to be loaded from StockChartX into TradeScript?      
            double max = 0;
            try
            {
                max = Math.Max(Utils.ExtractNumbers(ea.BuyScript).Max(), Utils.ExtractNumbers(ea.SellScript).Max());
            }
            catch (Exception)
            {
                max = 20;
            }

            int startRecord = Math.Max((StockChartX1.RecordCount - (int)(max * 3)) - 50, 0);
            //startRecord = 0;

            // Prime history
            //string test = "";
            for (int n = startRecord; n < StockChartX1.RecordCount; ++n)
            {
                double j = StockChartX1.GetJDate(MSymbol + ".close", n);
                double o = StockChartX1.GetValue(MSymbol + ".open", n);
                double h = StockChartX1.GetValue(MSymbol + ".high", n);
                double l = StockChartX1.GetValue(MSymbol + ".low", n);
                double c = StockChartX1.GetValue(MSymbol + ".close", n);
                double v = StockChartX1.GetValue(MSymbol + ".volume", n);
                if (v == -987654321) v = 0;
                if (j != -987654321 && o != -987654321)
                {
                    //test += StockChartX1.FromJulianDate(j) + "\t" + o + "\t" + h + "\t" + l + "\t" + c + "\t" + v + "\r\n";
                    eaa.buyAlert.AppendHistoryRecord(j, o, h, l, c, (int)v);
                    eaa.sellAlert.AppendHistoryRecord(j, o, h, l, c, (int)v);
                }
            }
            //Clipboard.Clear();
            //Clipboard.SetText(test);
            // DEBUG: StockChartX data copied to clipboard

            /*
            string data = "";
            int vl = 0;
            for (int k = 0; k < eaa.sellAlert.RecordCount; ++k)
            {
              eaa.sellAlert.GetRecordByIndex(k + 1, ref j, ref o, ref h, ref l, ref c, ref vl);
              string date = StockChartX1.FromJulianDate(j);
              data += date + "\t" + o + "\t" + h + "\t" + l + "\t" + c + "\t" + v + "\r\n";
            }
            Clipboard.Clear();
            Clipboard.SetText(data);
             */
            // DEBUG: TradeScript data copied to clipboard


            eaa.ea = ea;

            _expertAdvisors.Add(eaa);

            cmdEAs.Visible = true;
            RepositionEaButton();

        }

        private void RepositionEaButton()
        {
            cmdEAs.Top = 4;
            try
            {
                cmdEAs.Left = Width - StockChartX1.RightDrawingSpacePixels - cmdEAs.Width - 5;
            }
            catch (Exception)
            {
            }
            
        }

        /// <summary>
        /// Removes an expert advisor.
        /// </summary>
        /// <param name="ea">The expert advisor.</param>
        public void RemoveExpertAdvisor(ExpertAdvisor ea)
        {
            ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(ea.Name);
            if (eaa == null) return;
            _expertAdvisors.Remove(eaa);

            cmdEAs.Visible = _expertAdvisors.Count == 0;
        }


        /// <summary>
        /// This event fires when an expert advisor generates a TradeScript buy alert.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor.</param>
        private void OnBuyAlert(string symbol, string alertName)
        {
            if (MClosing) return; // The chart is being closed

            ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(alertName);
            if (eaa == null) return;

            /* uncomment if desired
            if (eaa.prevAlert != 1)
              eaa.prevAlert = 1;
            else
              return;
            */

            // Prevent alerts from displaying too frequently
            if (DateTime.Now.Subtract(eaa.lastBuyAlert).Seconds < 5 && eaa.lastBuyAlert != DateTime.MinValue) return;
            eaa.lastBuyAlert = DateTime.Now;

            ShowPopup(eaa.ea.Name, eaa.ea.ParseMessage(ExpertAdvisor.BuyOrSell.Buy));

            try
            {
                UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\ExpertAdvisorBuyAlert.wav", 0, UnmanagedMethods.SND_FILENAME | UnmanagedMethods.SND_ASYNC);
            }
            catch (Exception)
            {
            }

        }


        /// <summary>
        /// This event fires when an expert advisor generates a TradeScript sell alert.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor.</param>
        private void OnSellAlert(string symbol, string alertName)
        {
            if (MClosing) return; // The chart is being closed

            ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(alertName);
            if (eaa == null) return;

            /* uncomment if desired
            if (eaa.prevAlert != 2)
              eaa.prevAlert = 2;
            else
              return;
            */

            // Prevent alerts from displaying too frequently
            if (DateTime.Now.Subtract(eaa.lastSellAlert).Seconds < 5 && eaa.lastSellAlert != DateTime.MinValue) return;
            eaa.lastSellAlert = DateTime.Now;

            ShowPopup(eaa.ea.Name, eaa.ea.ParseMessage(ExpertAdvisor.BuyOrSell.Sell));

            try
            {
                UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\ExpertAdvisorSellAlert.wav", 0, UnmanagedMethods.SND_FILENAME | UnmanagedMethods.SND_ASYNC);
            }
            catch (Exception)
            {
            }

        }


        /// <summary>
        /// This event fires when an expert advisor generates an error.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor alert.</param>
        /// <param name="description">The description of the error.</param>
        private void ScriptError(string symbol, string alertName, string description)
        {
            // Virtually guaranteed not to get here since scripts are evaluated before saving
            //_mCtlData.OutputWindow1.DisplayAlertOrMessage("'" + alertName +
            //  "' expert advisor generated an error: " + description, OutputWindow.OutputIcon.Warning);
        }


        /// <summary>
        /// Finds the expert advisor alert based on an expert advisor.
        /// </summary>    
        /// <param name="name">The expert advisor name.</param>
        /// <returns></returns>
        public ExpertAdvisorAlert FindExpertAdvisorAlert(string name)
        {
            ExpertAdvisorAlert eaa = _expertAdvisors.Find(
                temp => temp.ea.Name == name
                );
            return eaa;
        }

        public bool FindExpertAdvisor(string name)
        {
            return FindExpertAdvisorAlert(name) != null;
        }


        public void ClearExpertAdvisors()
        {
            _expertAdvisors.Clear();
            cmdEAs.Visible = false;
        }

        private void CmdEAsClick(object sender, EventArgs e)
        {
            frmExpertAdvisors eas = new frmExpertAdvisors(frmMain.GInstance.MExpertAdvisors, this);
            eas.ShowDialog(frmMain.GInstance);
        }

        /// <summary>
        /// Shows the expert advisor popup.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        internal void ShowPopup(string name, string message)
        {

            if (MClosing) return; // The chart is being closed

            //NPopupNotify eaPopup;
            //eaPopup = new NPopupNotify();

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = myAssembly.GetManifestResourceNames();

            Stream s = GetType().Assembly.GetManifestResourceStream("M4.Images.ExpertAdvisorAlert.bmp");
            Bitmap bmp = new Bitmap(s);
            s.Close();

            //eaPopup.Shape = bmp;
            //eaPopup.PredefinedStyle = PredefinedPopupStyle.Shaped;
            //eaPopup.ShapeTransparentColor = Color.Magenta;
            //eaPopup.Caption.ButtonSize = new NSize(17, 17);
            //eaPopup.Caption.ButtonsMargins = new NPadding(347, 2, 18, 363);
            //eaPopup.CaptionBounds = new Rectangle(0, 0, 366, 20);
            //eaPopup.MoveableBounds = new Rectangle(0, 0, 366, 20);
            //eaPopup.ContentBounds = new Rectangle(5, 24, 361, 150);

            //NImageAndTextItem item = eaPopup.Content;
            //item.Text = "<font face='Arial' size='9' color='Navy'><b>" + name + "</b>    " + m_Symbol + "    " + m_timeStamp + "</font><br/><br/>" + message;

            //NUIItemImageSet imageSet = eaPopup.CloseButtonImageSet;

            //imageSet.NormalImage = imageList1.Images[0];
            //imageSet.HotImage = imageList1.Images[1];
            //imageSet.PressedImage = imageList1.Images[2];

            //PopupAnimation animation = PopupAnimation.None;
            //animation |= PopupAnimation.Fade;
            //animation |= PopupAnimation.Slide;
            //eaPopup.AutoHide = false;
            //eaPopup.VisibleSpan = 120000;
            //eaPopup.Opacity = 255;
            //eaPopup.Animation = animation;
            //eaPopup.AnimationDirection = PopupAnimationDirection.Automatic;
            //eaPopup.VisibleOnMouseOver = true;
            //eaPopup.FullOpacityOnMouseOver = true;
            //eaPopup.AnimationInterval = 10;
            //eaPopup.AnimationSteps = 20;
            //eaPopup.Palette.Copy(NUIManager.Palette);
            //eaPopup.Show();

        }

        #endregion

        #region Consensus Reports

        frmExpertAdvisors eas = null;
        bool _consensusReportRunning = false;
        bool _consensusReportRan = false;
        List<ExpertAdvisor> _consensusEAs = null;
        ConsensusReport cr = null;

        public void RunConsensusReport()
        {
            RunConsensusReport(true);
        }

        /// <summary>
        /// Runs the consensus report.
        /// </summary>
        public void RunConsensusReport(bool select)
        {

            if (_consensusReportRunning)
            {
                MessageBox.Show(
                  "Consensus report in progress. Wait until done.",
                  "Already running",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);
                return;
            }

            cr = new ConsensusReport();

            if (select)
            {
                // Select the expert advisors       
                eas = new frmExpertAdvisors(M4.frmMain.GInstance.MExpertAdvisors) {Client = cr};
                _asyncOp.Send(() => eas.ShowDialog(M4.frmMain.GInstance));
            }
            else
            {
                cr._expertAdvisors = _consensusEAs;
            }

            ProgressWorkerParams workerParams = new ProgressWorkerParams { Alignment = ContentAlignment.BottomLeft, AllowCancel = true, ControlAnchor = this, Modal = true, };
            ProgressWorker.RunSTA(
              workerParams,
              visualizer =>
              {
                  try
                  {

                      bool first = true;
                      cr.ProgressCallback += (object o, int records, int record, ref bool stop) =>
                      {
                          if (first)
                          {
                              first = false;
                              visualizer.InitProgress(0, records);
                          }
                          visualizer.ReportProgress(record);
                          stop = visualizer.CancelReqested;
                      };

                      visualizer.SetProgressTitle("Consensus Report");
                      visualizer.SetProgressAction("Generating report...");
                      visualizer.InitProgress(0, cr.ResultCount);

                      // Run the report. You may optionally change the number of periods for the window size
                      //ConsensusReport.ConsensusReportResults results = cr.RunConsensusReport(this, 10);

                      // Exit if no report, optional
                      //if (cr.ResultCount < 1 || visualizer.CancelReqested)

                      // Output results
                      //_asyncOp.Send(() => rtbConsensus.Rtf = results.Report);
                      _asyncOp.Send(() => pnlConsensus.Height = Height);
                      _asyncOp.Send(() => pnlConsensus.Left = Width - pnlConsensus.Width);
                      _asyncOp.Send(() => StockChartX1.Width = Width - pnlConsensus.Width - 4);
                      _asyncOp.Send(() => pnlConsensus.BackColor = M4.frmMain.GInstance.BackColor);
                      _asyncOp.Send(() => pnlConsensus.Visible = true);
                      _asyncOp.Send(() => pnlTwitter.Visible = false);
                      _asyncOp.Send(() => StockChartX1.Left = 0);
                      _asyncOp.Send(() => guage1.Visible = true);
                      //_asyncOp.Send(() => guage1.Value = (int)(double)(results.Ranking * 100));

                  }
                  catch (Exception ex)
                  {
                      _asyncOp.Post(
                        () =>
                        MessageBox.Show(
                          "An error occured while running the consensus report. Error :" + Environment.NewLine + ex.Message,
                          "Consensus Report Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error));
                  }
                  finally
                  {
                      _consensusReportRunning = false;
                  }
              },
              () =>
              { });

            _consensusEAs = cr._expertAdvisors;
            _consensusReportRan = true;

        }

        #endregion

        #region Twitter

        public void ShowTwitter()
        {

            Application.DoEvents();

            chkTwitter.Checked = Properties.Settings.Default.TweetTrades == "1";

            pnlTwitter.Height = Height;
            pnlTwitter.Left = Width - pnlTwitter.Width;
            StockChartX1.Width = Width - pnlTwitter.Width - 4;
            TwitterTimelineControl.Height = Height - TwitterTimelineControl.Top - pnlTwitterControls.Height - 4;
            pnlTwitterControls.Top = Height - pnlTwitterControls.Height - 4;
            pnlTwitter.Visible = true;

            // Load the timeline if previous oAuth success
            if (!string.IsNullOrEmpty(Properties.Settings.Default.oauth_token))
            {
                GetDisplayName();
                RefreshTwitter();
                webBrowser1.Visible = false;
                pnlTwitterAuthorize.Visible = false;
                StockChartX1.Visible = true;
            }

            else
            {

                // Perform oAuth
                oAuthTwitter oAuth = new oAuthTwitter();
                Uri url = new Uri(oAuth.AuthorizationLinkGet());
                _token = HttpUtility.ParseQueryString(url.Query)["oauth_token"];

                webBrowser1.DocumentText = "<html><head><title></title><style type='text/css'>body {background-color:#5599BB;color:#ffffff;font-family:arial;}img{border:none}</style></head><body>"
                  + "<h1>PLENA Twitter Authorization</h1><p>This is your first time using the Twitter Trade Tweeter.</p>"
                  + "<p>You must login to Twitter to grant access to PLENA in order to use this feature.</p>"
                  + "<p>Click the login button below and the Twitter login page will appear.</p>"
                  + "<p>You will be provided with a PIN after you login.</p>"
                  + "<p>Please enter the PIN in the box below then click Update.</p>"
                  + "<p><a href=\"" + url.ToString() + "\"><img src=\"http://apiwiki.twitter.com/f/1242697608/Sign-in-with-Twitter-lighter.png\" alt=\"Sign in with Twitter\" /></a></p>"
                  + "</body></html>";

                webBrowser1.Top = StockChartX1.Top;
                webBrowser1.Left = StockChartX1.Left;
                webBrowser1.Width = StockChartX1.Width;
                webBrowser1.Height = StockChartX1.Height;
                webBrowser1.Visible = true;
                StockChartX1.Visible = false;

            }

        }


        private void GetDisplayName()
        {
            // Get the display name
            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = Properties.Settings.Default.oauth_token;
            oAuth.TokenSecret = Properties.Settings.Default.oauth_secret;
            string urlStr = "http://twitter.com/account/verify_credentials.xml";
            string xml = "";
            try
            {
                xml = oAuth.oAuthWebRequest(oAuthTwitter.Method.GET, urlStr, String.Empty);
            }
            catch (Exception)
            {
                return;
            }
            int found = xml.IndexOf("<screen_name>");
            if (found > 0)
            {
                xml = xml.Substring(found + 13);
                found = xml.IndexOf("</screen_name>");
                if (found > 0)
                {
                    Properties.Settings.Default.display_name = xml.Substring(0, found).Trim();
                    Properties.Settings.Default.Save();
                }
            }
        }


        private XmlDocument GetUserTimelineXML(string displayName, int count)
        {

            if (string.IsNullOrEmpty(Properties.Settings.Default.oauth_token)) return null; // Not authenticated
            if (count < 1) count = 1;

            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = Properties.Settings.Default.oauth_token;
            oAuth.TokenSecret = Properties.Settings.Default.oauth_secret;

            string url = "http://twitter.com/statuses/user_timeline.xml?count=" + count.ToString() + "&screen_name=" + displayName;

            string xml = "";
            xml = oAuth.oAuthWebRequest(oAuthTwitter.Method.GET, url, String.Empty);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }

        // Try to populate txtPin.Text with the PIN when it is displayed
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string text = webBrowser1.Document.Body.OuterHtml.ToLower();
            int found = text.IndexOf("id=oauth_pin");
            if (found > 0)
            {
                text = text.Substring(found + 13);
                found = text.IndexOf("</div>");
                if (found > 0)
                {
                    txtPin.Text = text.Substring(0, found - 1).Trim();
                    picHighlight.Visible = true;
                    pnlTwitterAuthorize.Visible = true;
                }
            }
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            // Save the PIN/oAuth token

            if (txtPin.Text.Trim().Length < 1) return;
            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = _token;

            try
            {
                oAuth.AccessTokenGet(_token, txtPin.Text.Trim());
            }
            catch (Exception)
            {
                MessageBox.Show("Authentication failed", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (oAuth.TokenSecret.Length > 0)
            {

                Properties.Settings.Default.oauth_token = oAuth.Token;
                Properties.Settings.Default.oauth_secret = oAuth.TokenSecret;
                Properties.Settings.Default.Save();

                // Get the display name
                GetDisplayName();
                RefreshTwitter();
                pnlTwitterAuthorize.Visible = false;

            }

            webBrowser1.Visible = false;
            pnlTwitterAuthorize.Visible = false;
            StockChartX1.Visible = true;
        }

        // Refresh the timeline control
        private void RefreshTwitter()
        {

            GetDisplayName();

            Application.DoEvents();

            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = Properties.Settings.Default.oauth_token;
            oAuth.TokenSecret = Properties.Settings.Default.oauth_secret;


            XmlDocument doc;
            try
            {
                doc = GetUserTimelineXML(Properties.Settings.Default.display_name, 20);
            }
            catch (Exception)
            {
                MessageBox.Show("Twitter login failed", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sCreatedAt = "";
            string sText = "";

            string sScreenName = "";
            string sLocation = "";
            string sImage = "";

            bool first = true;
            TwitterTimelineControl.Controls.Clear();

            if (doc == null) return;
            if (doc.SelectNodes("statuses/status") == null) return;
            XmlNodeList status = doc.SelectNodes("statuses/status");
            // you have status.Count posts
            foreach (XmlNode node in status)
            {

                if (first) // Get user info
                {
                    XmlNodeList screen_name = node.SelectNodes("user/screen_name");
                    sScreenName = screen_name.Item(0).InnerText;
                    XmlNodeList location = node.SelectNodes("user/location");
                    sLocation = location.Item(0).InnerText;
                    XmlNodeList image_url = node.SelectNodes("user/profile_image_url");
                    sImage = image_url.Item(0).InnerText;
                    first = false;

                    // Display user
                    userPictureBox.LoadAsync(sImage);
                    UserNameLabel.Text = sScreenName + " Trade Tweets";
                }

                // Get message
                XmlNodeList created_at = node.SelectNodes("created_at");
                sCreatedAt = created_at.Item(0).InnerText;

                XmlNodeList text = node.SelectNodes("text");
                sText = text.Item(0).InnerText;

                // Display message
                TwitterTimelineControl.Controls.Add(new TwitterTimeline(sCreatedAt, sText));
            }
        }


        private void txtTweet_TextChanged(object sender, EventArgs e)
        {
            lblTweetSize.Text = (140 - txtTweet.Text.Length).ToString();
        }

        private void ChkTwitterCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TweetTrades = chkTwitter.Checked ? "1" : "0";
            //M4.frmMain.GInstance.tweetTrades = chkTwitter.Checked;
        }

        private void TmrAttentionTick(object sender, EventArgs e)
        {
            picHighlight.Visible = !picHighlight.Visible;
        }

        #endregion

    }
}
