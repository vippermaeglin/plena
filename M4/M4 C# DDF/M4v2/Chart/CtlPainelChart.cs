using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using AxSTOCKCHARTXLib;
using M4.AsyncOperations;
using M4.DataServer.Interface.ProtocolStructs;
using M4.M4v2.Chart.LineStudy;
using M4.M4v2.Chart.PriceSettings;
using M4.M4v2.Chart.TechnicalAnalysis;
using M4.M4v2.Chart.Templates;
using M4.M4v2.Settings;
using M4Core.Entities;
using M4Data.List;
using STOCKCHARTXLib;
using Telerik.WinControls;
using Telerik.WinControls.Commands;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI.Docking;
//using TradeScriptLib;
using Telerik.WinControls.UI;
using Indicator = STOCKCHARTXLib.Indicator;
using M4.DataServer.Interface;
using Periodicity = M4Core.Entities.Periodicity;
using System.Data.SqlClient;
using M4.M4v2.Chart.IndicatorElements;
using M4.M4v2.Chart.Averages;

namespace M4.M4v2.Chart
{
    public partial class CtlPainelChart : UserControl, IDataSubscriber, IPoolable
    {

        public bool BlockUpdateStock { get; set; }

        //public CtlPainelChart()
        //{
        //    InitializeComponent();
        //    SetTextTab();
        //}

        public void SetTextTab()
        {
            twSumary.Text = Program.LanguageDefault.DictionaryChartCtl["twSumary"];
        }

        #region Members and Structures

        public bool StateUsed { get; set; }
        public bool StateDummy { get; set; }
        public bool isLoading = false;

        public bool popout = false;
        public ndtChartTools ChartToolsPopOut;

        private const double NULL_VALUE = -987654321.0;
        public int _percent = 20;
        public bool ActiveChartChanged { get; set; }
        private FrmSelectIndicator _frmSelectIndicator = new FrmSelectIndicator();
        private FrmSelectStudy _frmSelectStudy = new FrmSelectStudy();
        private bool _mouseWheel;

        private string _templateText;
        private string _templateParent;
        private string _templateDefault;

        public string _nameTemplateDefault;
        public frmMain2.ActionChart _actionChart;

        public string _currentTemplate = null;
        private readonly List<string> _mPatternSeries = new List<string>();
        private ctlData _mCtlData;
        private string _hostedControlText;

        public ctlData MCtlData
        {
            get { return _mCtlData; }
            set { _mCtlData = value; }
        }

        public HistoryRequestAnswer Answer { get; set; }

        private string _token = "";

        public int Subscribers; //If subscribers <> 0 then do not unload!
        public bool MClosing;
        public static frmMain2 m_frmMain2;
        public Color MSelectionBorderColor;
        public string MSymbol;
        public bool MUserEditing;
        public bool RealTimeUpdates = true;
        private DateTime _mTimeStamp;
        private readonly List<string> m_SeriesNames = new List<string>();
        private bool m_missingVolume;
        private bool _mDialogShown;
        private KeyEventHandler ddlStock_KeyUp_Handler;
        private KeyEventHandler ddlStock_KeyDown_Handler;
        private Telerik.WinControls.UI.Data.PositionChangedEventHandler DdlStockSelectedIndexChanged_Handler;
        private AxSTOCKCHARTXLib._DStockChartXEvents_PaintEventHandler PaintEvent_Handler;
        public string PathSymbols = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\Symbols.xml";
        public string PathStudy = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\STUDY\\";
        public List<Stock> _mListSymbols;
        public AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        public Keys currentKeyPressed;
        public bool CrossHairs, DeltaCursor;
        public GraphicsState CrossHairState;
        Pen crossHairPen;
        Graphics crossHairGraphics;
        public Point lastMousePositionStart, lastMousePositionEnd;
        public int paddingTop, paddingBottom, paddingRight;
        public bool errorCharFast = true;
        public bool waitPeriodicity = false;

        private abstract class HorzLine
        {
            public const int Panel = 0;
            public const double Value = 0;
        }

        private bool _mMenu;
        private readonly List<HorzLine> _mHorzLines = new List<HorzLine>();
        public double _mValue;
        public int _mRecord;
        public int _lastPanelRClicked = 0;

        public class BarData
        {
            public double jDate;
            public double OpenPrice;
            public double HighPrice;
            public double LowPrice;
            public double ClosePrice;
            public double Volume;
        }

        private abstract class RTCache
        {
            public double LastPrice = 0;
            public string Symbol = "";
            public DateTime TradeDate = DateTime.MinValue;
            public long Volume = 0;
        }
        private readonly List<RTCache> _mRtCache = new List<RTCache>(); //Caches data while user is editing the chart

        private class BarCache
        {
            public M4.DataServer.Interface.BarData Bar;
            public Periodicity BarType;
            public string Symbol;
            public bool IsNewBar;
        }
        private readonly List<BarCache> _mBarCache = new List<BarCache>(); //Caches bars while user is editing the chart

        private readonly List<BarData> Data = new List<BarData>();

        private string m_name;
        private int m_objectType;
        private readonly string m_CmdArg;

        private string m_SelectedObject;
        public AsyncOperation _asyncOp;

        private class ChartOrder
        {
            //public ctlPortfolio.Orders.Side OrderSide;
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
        //private ctlPortfolio.Orders.Side m_Side = ctlPortfolio.Orders.Side.Unknown;
        //private readonly modulusfe.platform.Service svc = new modulusfe.platform.Service();
        private static readonly CultureInfo usCulture = new CultureInfo("en-US");


        public string m_PriceStyle;                     //Price styles: line, candle, Heikin Ashi etc
        public Periodicity m_Periodicity;   			//Periodicity type: minute, day, week, mounth or year
        public int m_BarSize;                           // Interval of periodicity like "X" Minutes (it's usually 1 when not-intraday)
        public int m_Bars;                              // Number of bars (ex: 5000)
        public string m_SchemeColor;                    // Colors for background, candles, backchart, backgradient, scale etc
        public double m_StartJDate = NULL_VALUE;        // First visible record
        public double m_EndJDate = NULL_VALUE;          // Last visible record
        public int m_QtyJDate = Properties.Settings.Default.ChartViewport;                   // Number of records on viewport
        public bool m_StopLoadScroll = false;           // Dont fire CtlPainelChart::LoadScroll() while chart initializes
        public bool m_StopSaveViewport = false;           // Dont save viewport sometimes
        public bool _async;
        public string _source;

        private bool m_DrawingLineStudy;
        private int m_lastObjectCount;

        public bool DrawingLineStudy
        {
            get
            {
                return m_DrawingLineStudy;
            }
            set
            {
                m_DrawingLineStudy = value;
                if (value)
                {
                    EnableControls(false);
                    m_lastObjectCount = StockChartX1.GetObjectCount((ObjectType)(-1));
                }
                else
                {
                    EnableControls(true);
                }
            }
        }

        #endregion

        #region Initialization

        private void TranslateText()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            cmdLineColor.ButtonElement.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdLineColor"];
            cmdFacebook.ButtonElement.ToolTipText = "Facebook";
            cmdZoomAreaStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomAreaStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomAreaStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomZeroStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomZeroStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomZeroStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomInStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomInStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomInStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomOutStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];
            cmdZoomOutStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];
            cmdZoomOutStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];

            cmdCandleChartStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdCandleChart"];
            cmdCandleChartStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdCandleChart"];
            cmdCandleChartStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdCandleChart"];
            cmdStockLineStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdStockLine"];
            cmdStockLineStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdStockLine"];
            cmdStockLineStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdStockLine"];
            cmdBarChartStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdBarChart"];
            cmdBarChartStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdBarChart"];
            cmdBarChartStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdBarChart"];
            cmdHeikinAshiStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshi"];
            cmdHeikinAshiStock.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshi"];
            cmdHeikinAshiStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshi"];
            cmdHeikinAshiSmooth.Text = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshiSmooth"];
            cmdHeikinAshiSmooth.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshiSmooth"];
            cmdHeikinAshiSmooth.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshiSmooth"];
            cmdDeleteDrawings.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDeleteStock"];
            cmdDeleteDrawings.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdDeleteStock"];
            cmdDeleteDrawings.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdDeleteStock"];

            cmdTechnicalAnalysisProfessional.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTechnicalAnalysis"];
            cmdTechnicalAnalysisProfessional.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdTechnicalAnalysis"];
            cmdTechnicalAnalysisProfessional.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdTechnicalAnalysis"];

            cmdPeriodicityDaily.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"];
            cmdPeriodicityMonth.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"];
            cmdPeriodicityWeekly.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"];
            cmdPeriodicityCustom.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityCustom"];
            cmdPeriodicityDaily.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"];
            cmdPeriodicityMonth.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"];
            cmdPeriodicityWeekly.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"];
            cmdPeriodicityCustom.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityCustom"];
            ddlStock.DisplayName = "Seletor de Ativos";
            cmdCheckPortfolio.DisplayName = "Associar Portfólios";
            ddlStock.ToolTipText = "Seletor de Ativos";
            cmdCheckPortfolio.ToolTipText = "Associar Portfólios";

            twTemplates.Text = Program.LanguageDefault.DictionarySelectIndicator["lblTemplates"];
            cmnuTemplates.Items[0].Text = Program.LanguageDefault.DictionarySelectIndicator["lblDefault"];
            cmnuTemplates.Items[1].Text = Program.LanguageDefault.DictionarySelectIndicator["lblRename"];
        }

        private void BindGenericEvents()
        {
            //Events Handler
            DdlStockSelectedIndexChanged_Handler =

                new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DdlStockSelectedIndexChanged);
            PaintEvent_Handler =
                new AxSTOCKCHARTXLib._DStockChartXEvents_PaintEventHandler(this.StockChartX1_PaintEvent);
            DBlocalSQL.SnapshotEvent += DBDailyShared_SnapshotEvent;
            DBlocalSQL.NewBarEvent += DBDailyShared_NewBarEvent;
            DBlocalSQL.UpdateBaseEvent += DBDailyShared_UpdateBaseEvent;

            ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyDown +=
                new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyDown);
            ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyUp +=
                new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyUp);
            ddlStock.SelectedIndexChanged += DdlStockSelectedIndexChanged_Handler;
            StockChartX1.PaintEvent += PaintEvent_Handler;

            var check = new RadCheckBoxElement { ShowBorder = false, Checked = Properties.Settings.Default.VisiblePortfolio };
            check.ToggleStateChanged += CheckToggleStateChanged;
            cmdCheckPortfolio.HostedItem = check;

            foreach (
                    var item in
                        from radItem in ctmEditChart.Items
                        where radItem.Name.Equals("mnuEsquemaCores")
                        select ((RadMenuItem)radItem))
            {
                item.Items.Clear();
                foreach (KeyValuePair<string, string> scheme in Scheme.Instance().Schemes)
                {
                    RadMenuItem esquemaCores = new RadMenuItem
                    {
                        AccessibleDescription = scheme.Value,
                        AccessibleName = scheme.Value,
                        Name = scheme.Key,
                        Text = scheme.Value,
                        Visibility = ElementVisibility.Visible
                    };

                    esquemaCores.Click += RadItemClick;

                    item.Items.AddRange(new RadItem[] { esquemaCores });
                }
            }

            foreach (var item in
                from radItem in ctmEditChart.Items
                where radItem.GetType().Name.Equals("RadMenuItem")
                select ((RadMenuItem)radItem))
            {
                if (item.Items.Count == 0)
                    item.Click += RadItemClick;
                else
                {
                    foreach (var subItem in from subRadItem in item.Items
                                            where subRadItem.GetType().Name.Equals("RadMenuItem")
                                            select ((RadMenuItem)subRadItem)
                                                into subItem
                                                where subItem != null
                                                select subItem)
                    {
                        subItem.Click += RadItemClick;
                    }
                }
            }

            //Drag Drop
            radPanel1.AllowDrop = true;

            /*pnlCtlPainelChart.DragEnter -= radPanel1_DragEnter;
            pnlCtlPainelChart.DragEnter += radPanel1_DragEnter;

            radPanel2.DragEnter -= radPanel1_DragEnter;
            radPanel2.DragEnter += radPanel1_DragEnter;


            radPanel1.DragOver -= radPanel1_DragEnter;
            radPanel1.DragOver += radPanel1_DragEnter;*/


            radPanel2.MouseMove += radPanel1_MouseEnter;
            radPanel2.MouseEnter += radPanel1_MouseEnter;


            radPanel1.DragEnter -= radPanel1_DragEnter;
            radPanel1.DragEnter += radPanel1_DragEnter;

            radPanel1.DragDrop -= radPanel1_DragDrop;
            radPanel1.DragDrop += radPanel1_DragDrop;


        }

        void radPanel1_MouseEnter(object sender, EventArgs e)
        {
            radPanel1.Focus();
        }

        void radPanel1_DragDrop(object sender, DragEventArgs e)
        {
            string symbol = e.Data.GetData(typeof(string)).ToString();
            if (symbol == "") return;

            // NEW WINDOW WITH SAME SETTINGS (INSERT)!

            ChartSelection selection;

            
            StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            SaveViewportJDate();

            selection = new ChartSelection
            {
                Symbol = symbol,
                Interval = m_BarSize,
                Bars = m_Bars,
                Periodicity = m_Periodicity,
                //PriceStyle = m_PriceStyle,
                Source = "PLENA"
            };

            m_frmMain2.UseLastChartVisual = true;
            m_frmMain2.CreateNewCtlPainel(selection, chart2 =>
            {
                chart2.m_SchemeColor = m_SchemeColor;
                Scheme.Instance().UpdateChartColors(chart2.StockChartX1, m_SchemeColor);
                chart2.StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                               "TempTemplate" + ".sct");
                chart2.UpdateMenus();
                chart2.StockChartX1.Visible = true;
                this.Focus();
            });
            return;

            //Insert new tab behavior (error loading template)
            /*ChartSelection selection = new ChartSelection();
            selection.Interval = m_BarSize;
            selection.Bars = m_Bars;
            selection.Periodicity = m_Periodicity;
            selection.Symbol = symbol;
            selection.Source = _source;
            selection.PriceStyle = m_PriceStyle;

            StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            try
            {
                m_frmMain2.UseLastChartVisual = true;
                m_frmMain2.QtyLastViewport = m_QtyJDate;
                m_frmMain2.SchemeLastChart = m_SchemeColor;
                //Send stockchart settings:
                m_frmMain2.CreateNewCtlPainel(selection, chart =>
                {
                    //Chart collor
                    chart.m_SchemeColor = m_SchemeColor;
                    Scheme.Instance().UpdateChartColors(chart.StockChartX1, m_SchemeColor);

                    //Right, Top and Bottom Space
                    chart.SetChartPadding(paddingTop, paddingBottom, paddingRight);

                    //Scale Type
                    chart.StockChartX1.ScaleType = StockChartX1.ScaleType;

                    //Visible Bars
                    chart.StockChartX1.VisibleRecordCount = StockChartX1.VisibleRecordCount;
                    //Scale Precision
                    chart.StockChartX1.ScalePrecision = StockChartX1.ScalePrecision;
                    //Show Horizontal Grid
                    chart.StockChartX1.YGrid = StockChartX1.YGrid;
                    //Show Vertical Grid
                    chart.StockChartX1.XGrid = StockChartX1.XGrid;
                    //Panel Separators
                    chart.StockChartX1.HorizontalSeparators = StockChartX1.HorizontalSeparators;

                    //Price params
                    chart.StockChartX1.SmoothHeikinPeriods = StockChartX1.SmoothHeikinPeriods;
                    chart.StockChartX1.SmoothHeikinType = StockChartX1.SmoothHeikinType;
                    chart.StockChartX1.PriceLineMono = StockChartX1.PriceLineMono;
                    chart.StockChartX1.PriceLineThickness = StockChartX1.PriceLineThickness;
                    chart.StockChartX1.PriceLineThicknessBar = StockChartX1.PriceLineThicknessBar;

                    //Studies params
                    chart.StockChartX1.LineThickness = StockChartX1.LineThickness;
                    chart.ddlLineWidth.SelectedIndex = StockChartX1.LineThickness - 1;
                    chart.StockChartX1.LineColor = StockChartX1.LineColor;
                    cmdLineColor.ButtonElement.ButtonFillElement.BackColor = StockChartX1.LineColor;

                    UpdateMenus();

                    chart.LoadDataTemplate(_currentTemplate);
                    chart.StockChartX1.LoadGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");

                    chart.StockChartX1.Visible = true;



                    chart.StockChartX1.Focus();
                });

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("Ativo inválido!", " ");
                StockChartX1.Focus();
                LoadScroll();
                return;

            }*/

            //GRID double click behavior:
            /*
            if (m_frmMain2.GetCtlActiveWindowFloat() != null)
            {
                if (m_frmMain2.GetCtlActiveWindowFloatName() == "CtlPainelChart")
                //if (_parentControl.MFrmMain2.GetCtlActiveWindowFloatName() != "ctlWeb")
                {
                    CtlPainelChart chart = (CtlPainelChart)(m_frmMain2.GetCtlActiveWindowFloat().Controls[0]);
                    if (symbol.Equals(chart.MSymbol))
                        return;
                    chart.m_StopLoadScroll = true;


                    //Console.WriteLine("\nViewChart() MSymbol=" + chart.MSymbol + " NewWSymbol=" + symbol);

                    chart.StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                    chart.SaveViewportJDate();

                    chart.MSymbol = symbol;
                    chart.InitRTChartAsync(b => chart._asyncOp.Post(() =>
                    {
                        if (b)
                        {
                            //chart.BindContextMenuEvents();
                            chart.StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                            chart.UpdateMenus();
                            chart.StockChartX1.Visible = true;
                            return;
                        }
                        return;
                    }));

                    chart.Parent.Text = chart.GetChartTitle();

                    if (((DockWindow)Parent).DockState == DockState.Floating)
                    {

                        foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                        {
                            if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                            {
                                document.Text = chart.GetChartTitle();
                            }
                        }
                    }
                    Console.WriteLine("\n\t1- " + DateTime.Now.TimeOfDay + " Grid:ViewChart() ");
                    return;
                }
            }*/
        }

        void radPanel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        public void SetControls()
        {
            try
            {
                if (!String.IsNullOrEmpty(MSymbol))
                    ddlStock.SelectedValue = MSymbol;

                StockChartX1.Language = Program.LanguageStockChartX;
                StockChartX1.ApplicationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena";
                //m_SchemeColor = Properties.Settings.Default.SchemeColor;
                //SaveViewportJDate();
                //Console.WriteLine("\n\nSetControls(): CrossHairs value changed.");
                CrossHairs =
                    StockChartX1.CrossHairs =
                    m_frmMain2.cmdCrosshair.ToggleState == ToggleState.On;
                StockChartX1.Magnetic = m_frmMain2.cmdMagnetic.ToggleState == ToggleState.On;
                DeltaCursor = StockChartX1.DeltaCursor = m_frmMain2.cmdDeltaCursor.ToggleState == ToggleState.On;
                //Default values for Line Study
                //StockChartX1.LoadUserStudyLine(-1);
                twSumary.Text = Program.LanguageDefault.DictionaryChartCtl["twSumary"];
                ctmDeleteButton.Text = Program.LanguageDefault.DictionarySelectChart["mnuDelete"];
                ctmEditButton.Text = Program.LanguageDefault.DictionarySelectChart["mnuEdit"];
                m_frmMain2.configStudies = ListConfigStudies.Instance().LoadListConfigStudies();
                StockChartX1.LineColor = m_frmMain2.configStudies.Color;
                StockChartX1.LineThickness = (int)m_frmMain2.configStudies.LineThickness;
                StockChartX1.SetFibonacciRetParams(
                    bool.Parse(m_frmMain2.configStudies.Retracements[4, 1]) ? 0.0 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[3, 1]) ? 0.382 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[2, 1]) ? 0.5 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[1, 1]) ? 0.618 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[0, 1]) ? 1.0 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[5, 1]) ? 1.618 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Retracements[6, 1]) ? 2.618 : NULL_VALUE, NULL_VALUE, NULL_VALUE,
                    NULL_VALUE);
                StockChartX1.SetFibonacciProParams(
                    bool.Parse(m_frmMain2.configStudies.Projections[4, 1]) ? 0.0 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[3, 1]) ? 0.382 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[2, 1]) ? 0.5 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[1, 1]) ? 0.618 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[0, 1]) ? 1.0 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[5, 1]) ? 1.618 : NULL_VALUE,
                    bool.Parse(m_frmMain2.configStudies.Projections[6, 1]) ? 2.168 : NULL_VALUE, NULL_VALUE, NULL_VALUE,
                    NULL_VALUE);
                StockChartX1.LineThickness = (int)m_frmMain2.configStudies.LineThickness;
                paddingTop = Properties.Settings.Default.PaddingTop;
                paddingBottom = Properties.Settings.Default.PaddingBottom;
                paddingRight = Properties.Settings.Default.PaddingRight;
                lastMousePositionStart = new Point(-1, -1);
                lastMousePositionEnd = new Point(-1, -1);
                //Initialize CrossHairs
                crossHairPen = new Pen(Color.Black, 1);
                crossHairGraphics = StockChartX1.CreateGraphics();
                CrossHairState = crossHairGraphics.Save();

                cmdLineColor.ButtonElement.ButtonFillElement.BackColor = ListConfigStudies.Instance().LoadListConfigStudies().Color;


                ddlLineWidth.SelectedIndex = (int)ListConfigStudies.Instance().LoadListConfigStudies().LineThickness - 1;



                LabelMouseX.BringToFront();
                LabelMouseY.BringToFront();
                LabelMouseX.BackColor = Utils.GetDefaultLabelGraphColor();
                LabelMouseX.ForeColor = Color.White;

                LabelMouseY.BackColor = Utils.GetDefaultLabelGraphColor();
                LabelMouseY.ForeColor = Color.White;


                StockChartX1.ValuePanelColor = Color.FromArgb(191, 219, 255);

                radDock1.SplitPanelElement.Fill.BackColor = Utils.GetDefaultBackColorChart();


                //Translate and remove overflowbuttons
                if (Properties.Settings.Default.DictionaryLanguage == "PortugueseBrazil")
                    commandBarRowElement1.Strips[0].OverflowButton.AddRemoveButtonsMenuItem.Text =
                        Program.LanguageDefault.DictionaryMenuPlena["mnuAddRemoveButtonsMenuItem"];
                commandBarRowElement1.Strips[0].OverflowButton.CustomizeButtonMenuItem.Visibility =
                    Telerik.WinControls.ElementVisibility.Collapsed;



                //TODO: Causa erros na primeira vez q abre o gráfico pelo Seletor
                ReconfigureTabs();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void ddlLineWidth_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            AssignTooltips(args.VisualItem);
        }


        void ddlLineWidth_ToolTipTextNeeded(object sender, Telerik.WinControls.ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = "Espessura da Linha";
        }
        private void AssignTooltips(RadListVisualItem item)
        {
            item.ToolTipText = "Espessura da Linha";
        }
        public void LoadWorkspaceRight()
        {
            foreach (var item in from radItem in ctmEditChart.Items where radItem.Name.Equals("mnuWorkspaceChart") select ((RadMenuItem)radItem))
            {
                item.Items.Clear();

                item.Items.AddRange(new RadItem[]
                                        {
                                            mnuManagerChart,
                                            radMenuSeparatorItem6
                                        });

                XmlDocument xmlDocumentWorkspace = new XmlDocument();
                xmlDocumentWorkspace.Load(ListWorkspace._path + "Workspace.xml");
                XmlNodeList nodeList = xmlDocumentWorkspace.GetElementsByTagName("WORKSPACE");

                foreach (RadMenuItem nCommand in from XmlNode node in nodeList select new RadMenuItem { Text = node["TEXT"].InnerText, Name = node["TEXT"].InnerText })
                {
                    nCommand.Visibility = ElementVisibility.Visible;
                    nCommand.Click += RadItemClick;

                    item.Items.AddRange(new RadItem[] { nCommand });
                }
            }
        }

        public void LoadCheckMenuRight()
        {
            LoadWorkspaceRight();

            mnuDeltaCursor.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            mnuEscalaLogaritmica.Text = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            mnuGridEixoX.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowXGrid"];
            mnuGridEixoY.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowYGrid"];
            mnuSeparadorPaineis.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            mnuMouseCruz.Text = Program.LanguageDefault.DictionaryMenuBar["cmdCrosshair"];
            mnuDarvasBoxes.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            mnuIndicatorChart.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTechnicalAnalysis"];
            mnuPriceSeries.Text = Program.LanguageDefault.DictionaryMenuBar["cmdPriceSeries"];

            mnuPanelPosition.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPanelPosition"];
            mnuPanelUp.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPanelUp"];
            mnuPanelDown.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPanelDown"];

            mnuTabChartPosition.Text = Program.LanguageDefault.DictionaryMenuBar["cmdPositionFlaps"];
            mnuPositionBottom.Text = Program.LanguageDefault.DictionarySettings["BottomChart"];
            mnuPositionLess.Text = Program.LanguageDefault.DictionarySettings["LessChart"];

            mnuManagerChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuManager"];
            mnuSettingsChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSettings"];
            mnuWorkspaceChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuWorkspace"];
            mnuEsquemaCores.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuChartColors"];
            mnuSaveImageChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSaveChartImage"];
            mnuPopoutChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPopoutActiveChart"];

            if (popout)
            {
                mnuPopoutChart.Enabled = false;
            }
            else
            {
                mnuPopoutChart.Enabled = true;
            }

            mnuPanelPosition.Enabled = mnuPanelUp.Enabled = mnuPanelDown.Enabled = true;
            if (_lastPanelRClicked == 0) mnuPanelPosition.Enabled = false;
            //else if (StockChartX1.get_PanelY1(_lastPanelRClicked) == StockChartX1.get_PanelY2(0)) mnuPanelUp.Enabled = false;
            //if (StockChartX1.get_PanelY2(_lastPanelRClicked) == StockChartX1.) mnuPanelDown.Enabled = false;

            foreach (var item in (from radItem in ctmEditChart.Items
                                  where radItem.GetType().Name.Equals("RadMenuItem")
                                  select ((RadMenuItem)radItem)).Where(item => item.Items.Count == 0))
            {

                switch (item.Name)
                {
                    case "mnuEscalaLogaritmica":
                        item.IsChecked = frmMain2.GInstance.mnuViewScaleType.ToggleState == ToggleState.On;
                        break;
                    case "mnuGridEixoX":
                        item.IsChecked = frmMain2.GInstance.mnuViewShowXGrid.ToggleState == ToggleState.On;
                        break;
                    case "mnuGridEixoY":
                        item.IsChecked = frmMain2.GInstance.mnuViewYGrid.ToggleState == ToggleState.On;
                        break;
                    case "mnuSeparadorPaineis":
                        item.IsChecked = frmMain2.GInstance.mnuViewSeparators.ToggleState == ToggleState.On;
                        break;
                    case "mnuMouseCruz":
                        item.IsChecked = frmMain2.GInstance.mnuViewCrossHair.ToggleState == ToggleState.On;
                        break;
                    case "mnuDarvasBoxes":
                        item.IsChecked = frmMain2.GInstance.mnuDarvasBoxes.ToggleState == ToggleState.On;
                        break;
                    case "mnuPositionBottom":
                        item.IsChecked = Properties.Settings.Default.TabDataPosition.Equals("Bottom");
                        break;
                    case "mnuPositionLess":
                        item.IsChecked = Properties.Settings.Default.TabDataPosition.Equals("Less");
                        break;
                    case "mnuDeltaCursor":
                        if (!popout)
                        {
                            item.IsChecked = frmMain2.GInstance.cmdDeltaCursor.ToggleState == ToggleState.On;
                        }
                        else
                        {
                            item.IsChecked = ChartToolsPopOut.cmdDeltaCursor.ToggleState == ToggleState.On;
                        }
                        break;
                }
            }

            foreach (var item in from radItem in ctmEditChart.Items where radItem.Name.Equals("mnuEsquemaCores") select ((RadMenuItem)radItem))
            {
                foreach (var s in item.Items)
                {
                    foreach (RadMenuItem nCommand in
                    frmMain2.GInstance.mnuChartColors.Items.Where(nCommand => nCommand.Text.ToUpper().Equals(s.Text.ToUpper())))
                    {
                        ((RadMenuItem)s).IsChecked = nCommand.IsChecked;
                    }
                }
            }

            foreach (var item in from radItem in ctmEditChart.Items where radItem.Name.Equals("mnuWorkspaceChart") select ((RadMenuItem)radItem))
            {
                foreach (var s in item.Items)
                {
                    foreach (RadMenuItem nCommand in
                        frmMain2.GInstance.mnuWorkspace.Items.Where(nCommand => nCommand.Text.ToUpper().Equals(s.Text.ToUpper())))
                    {
                        ((RadMenuItem)s).IsChecked = nCommand.IsChecked;
                    }
                }
            }

            foreach (var item in from radItem in ctmEditChart.Items where radItem.Name.Equals("mnuTabChartPosition") select ((RadMenuItem)radItem))
            {
                foreach (RadMenuItem s in item.Items)
                {
                    switch (s.Name)
                    {
                        case "mnuPositionBottom":
                            s.IsChecked = Properties.Settings.Default.TabDataPosition.Equals("Bottom");
                            break;
                        case "mnuPositionLess":
                            s.IsChecked = Properties.Settings.Default.TabDataPosition.Equals("Less");
                            break;
                    }
                }
            }
        }

        void frmPopoutChart_ParentChanged(object sender, EventArgs eventArgs)
        {
        }

        private void RadItemClick(object sender, EventArgs e)
        {
            try
            {
                if (sender == null)
                    return;

                var mnuItem = (RadMenuItem)sender;

                switch (mnuItem.Name)
                {
                    case "mnuIndicatorChart":

                        tmrSelectIndicator.Enabled = false;
                        _frmSelectIndicator.ParentForm = this;
                        _frmSelectIndicator.StockChartX1 = StockChartX1;
                        _frmSelectIndicator.SelectIndicator = m_name;
                        _frmSelectIndicator.ShowDialog();

                        return;

                    case "mnuPriceSeries":

                        new FrmPriceSettings(this).ShowDialog();

                        return;
                    case "mnuEscalaLogaritmica":
                        if (!popout)
                        {
                            frmMain2.GInstance.mnuViewScaleType.IsChecked = !mnuItem.IsChecked;
                            frmMain2.GInstance.LoadViewScaleType();
                        }
                        else
                        {
                            frmMain2.GInstance.mnuViewScaleType.IsChecked = !mnuItem.IsChecked;
                            ChartToolsPopOut.LoadViewScaleType();
                        }
                        return;
                    case "mnuGridEixoX":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadViewShowXGrid();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadViewShowXGrid();
                            frmMain2.GInstance.mnuViewShowXGrid.IsChecked = !mnuItem.IsChecked;
                        }
                        return;
                    case "mnuGridEixoY":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadViewShowYGrid();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadViewShowYGrid();
                            frmMain2.GInstance.mnuViewYGrid.IsChecked = !mnuItem.IsChecked;
                        }
                        return;
                    case "mnuSeparadorPaineis":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadViewSeparators();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadViewSeparators();
                            frmMain2.GInstance.mnuViewSeparators.IsChecked = !mnuItem.IsChecked;
                        }
                        return;
                    case "mnuMouseCruz":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadViewCrosshair();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadViewCrosshair();
                            frmMain2.GInstance.mnuViewCrossHair.IsChecked = !mnuItem.IsChecked;
                        }
                        return;
                    case "mnuDarvasBoxes":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadDarvasBoxes();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadDarvasBoxes();
                            frmMain2.GInstance.mnuDarvasBoxes.IsChecked = !mnuItem.IsChecked;
                        }
                        return;
                    case "mnuManagerChart":
                        frmMain2.GInstance.LoadViewManagerWorkspace();
                        return;
                    case "mnuSettingsChart":
                        frmMain2.GInstance.LoadFrmSettings();
                        return;
                    case "mnuPopoutChart":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadPopoutChart();
                        }
                        return;
                    case "mnuSaveImageChart":
                        if (!popout)
                        {
                            frmMain2.GInstance.MActiveChart.SaveChartImage();
                        }
                        else
                        {
                            ChartToolsPopOut.MActiveChart.SaveChartImage();
                        }
                        return;
                    case "mnuPositionLess":
                        Properties.Settings.Default.TabDataPosition = "Less";
                        frmMain2.GInstance.ReconfigureTabsChart();
                        break;
                    case "mnuPositionBottom":
                        Properties.Settings.Default.TabDataPosition = "Bottom";
                        frmMain2.GInstance.ReconfigureTabsChart();
                        break;
                    case "mnuDeltaCursor":
                        if (!popout)
                        {
                            frmMain2.GInstance.LoadDeltaCursor();
                        }
                        else
                        {
                            ChartToolsPopOut.LoadDeltaCursor();
                        }
                        break;
                    case "mnuPanelUp":
                        StockChartX1.MovePanelIndex(_lastPanelRClicked, 1, true);
                        break;
                    case "mnuPanelDown":
                        StockChartX1.MovePanelIndex(_lastPanelRClicked, 1, false);
                        break;
                }

                var scheme = Scheme.Instance().Schemes.Where(r => r.Key.Equals(mnuItem.Name)).SingleOrDefault();

                if (mnuItem.Name == scheme.Key)
                {
                    if (!popout)
                    {
                        frmMain2.GInstance.LoadSchemeClick(scheme.Value);
                        return;
                    }
                    else
                    {
                        ChartToolsPopOut.LoadSchemeClick(scheme.Value);
                    }
                }

                foreach (RadMenuItem command in
                    frmMain2.GInstance.mnuWorkspace.Items.Where(command => mnuItem.Text.ToUpper().Equals(command.Text.ToUpper())))
                {
                    frmMain2.GInstance.RestoreWorkspace(command.Text.Trim());
                    frmMain2.GInstance.WorkspaceLoaded(command.Text.Trim());
                    frmMain2.GInstance.UpdateStyle();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CheckToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            LoadStockPortfolioActive();
        }

        public void LoadStockPortfolioActive()
        {
            BlockUpdateStock = true;

            var check = (RadCheckBoxElement)cmdCheckPortfolio.HostedItem;

            if (check == null) return;

            if (check.Checked)
            {
                var userPortfolios = frmMain2.GetUserPortfolios().Where(r => r.Name.Equals(frmMain2.GInstance._select.TabSelected)).FirstOrDefault();

                if (userPortfolios == null)
                {
                    /*if ((_mListSymbols == null) || (_mListSymbols.Count == 0))
                    {
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\Symbols.xml";

                        List<Stock> list = ListStocks.Instance().LoadStocks(path);
                        _mListSymbols = list != null ? list.OrderBy(r => r.Name).ToList() : new List<Stock>();
                    }*/
                    _mListSymbols = new List<Stock>();
                    foreach (Symbol symbol in frmMain2.GetStockList())
                    {
                        _mListSymbols.Add(new Stock() { Code = symbol.Code });
                    }

                    ddlStock.DisplayMember = "Code";
                    ddlStock.ValueMember = "Code";
                    ddlStock.DataSource = _mListSymbols;

                    if (!string.IsNullOrEmpty(MSymbol))
                        ddlStock.SelectedValue = MSymbol;

                    return;
                }

                List<Stock> mListSymbols = new List<Stock>();

                foreach (var s in _mListSymbols.Where(s => userPortfolios.Symbols.Contains(s.Code)))
                {
                    mListSymbols.Add(s);
                }

                ddlStock.DisplayMember = "Code";
                ddlStock.ValueMember = "Code";
                ddlStock.DataSource = mListSymbols;

                if (mListSymbols.Where(r => r.Code == MSymbol).FirstOrDefault() != null)
                    ddlStock.SelectedValue = MSymbol;
                else
                    ddlStock.DropDownListElement.TextBox.Text = "";
            }
            else
            {
                if ((_mListSymbols == null) || (_mListSymbols.Count == 0))
                {
                    /*string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\Symbols.xml";

                    List<Stock> list = ListStocks.Instance().LoadStocks(path);
                    _mListSymbols = list != null ? list.OrderBy(r => r.Name).ToList() : new List<Stock>();*/
                    _mListSymbols = new List<Stock>();
                    foreach (Symbol symbol in frmMain2.GetStockList())
                    {
                        _mListSymbols.Add(new Stock() { Code = symbol.Code });
                    }
                }

                ddlStock.DisplayMember = "Code";
                ddlStock.ValueMember = "Code";
                ddlStock.DataSource = _mListSymbols;
            }

            BlockUpdateStock = false;
        }



        public CtlPainelChart()
        {
            try
            {
                InitializeComponent();
                StockChartX1.StudyDirectory = PathStudy;
                CreateAsyncOp();

                //BindContextMenuEvents();

                LoadStock();

                //SetControls();

                //LoadDataTemplate();

                TranslateText();

                HideTags();


                Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("Error ao criar PainelChart " + ex.Message, " ");
            }

        }

        //Usado no seletor de ativos:
        public void LoadCtlPainelChart(frmMain2 oMain2, ctlData oData, string Symbol, Periodicity BarPeriodicity, int BarSize, int Bars, string source,
          bool async)
        {
            try
            {
                m_frmMain2 = oMain2;
                _mCtlData = oData;
                //InitializeComponent();
                //CreateAsyncOp();
                MSymbol = Symbol;
                StockChartX1.Symbol = Symbol;
                m_Periodicity = BarPeriodicity;
                StockChartX1.Periodicity = (int)BarPeriodicity;
                m_BarSize = BarSize;
                StockChartX1.BarSize = m_BarSize;
                m_Bars = Bars;
                _async = async;
                _source = source;

                InitChartForm();

                /*if (!_async)
                {
                    InitRTChart();
                    BindContextMenuEvents();
                }*/

                SetTextTab();

                //LoadStock();

                //SetControls();

                BindGenericEvents();

                LoadToolTipStock();

                StateDummy = false;

                //LoadDataTemplate();

                //TranslateText();

                //HideTags();


            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("Error ao carregar PainelChart " + ex.Message, " ");
            }

        }

        public CtlPainelChart(frmMain2 oMain2, ctlData oData, string LoadChart)
        {
            m_CmdArg = LoadChart;
            m_frmMain2 = oMain2;
            //m_frmMain = oMain;
            _mCtlData = oData;
            InitializeComponent();
            CreateAsyncOp();

            InitChartForm();

            BindContextMenuEvents();

            LoadStock();

            SetControls();

            LoadToolTipStock();

            LoadDataTemplate();

            TranslateText();

            HideTags();
        }


        private void HideTags()
        {
            twSumary.AutoHide();

            ContextMenuService menuService = this.radDock1.GetService<ContextMenuService>();
            menuService.ContextMenuDisplaying += MenuServiceContextMenuDisplaying;

            radDock1.FloatingWindowCreated += radDock1_FloatingWindowCreated;
        }

        private void radDock1_FloatingWindowCreated(object sender, FloatingWindowEventArgs e)
        {
            if ((radDock1.ActiveWindow == twTemplates) || (radDock1.ActiveWindow == twSumary))
            {
                e.Window.FormElement.TitleBar.CloseButton.Visibility = ElementVisibility.Collapsed;
            }
        }

        private static void MenuServiceContextMenuDisplaying(object sender, ContextMenuDisplayingEventArgs e)
        {
            e.Cancel = true;
        }

        private void CreateAsyncOp()
        {
            _asyncOp = AsyncHelper.CreateOperation();
        }

        public void BindContextMenuEvents()
        {
            //mnucHorzLine.Click += mnucHorzLine_Click;
            //mnuVertLine.Click += mnucVertLine_Click;
            //mnuEditSeries.Click += mnuEditSeries_Click;
            //mnuDeleteObject.Click += mnuDeleteObject_Click;
            //mnuDeleteSeries.Click += mnuDeleteSeries_Click;
            btnSubmit.Click += btnSubmit_Click;
            //mnuBuyHere.Click += mnuBuyHere_Click;
            //mnuSellHere.Click += mnuSellHere_Click;
            //mnuClearOrders.Click += mnuClearOrders_Click;
            cmdCancel.Click += cmdCancel_Click;
        }

        //Initializes the chart form and loads a file depending
        //on which constructor was used.
        private void InitChartForm()
        {
            //StockChartX1.Visible = false;
            EnableControls(false);
            // StockChartX1.EnumIndicators();
            //Application.DoEvents();
            if (m_CmdArg != "")
            {
                //StockChartX1.Visible = true;
                if (File.Exists(m_CmdArg))
                {
                    StockChartX1.LoadFile(m_CmdArg);
                    EnableControls(true);
                }
            }
        }

        private void LoadToolTipStock()
        {
            cmdZoomAreaStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomAreaStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomZeroStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomZeroStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomInStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomInStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomOutStock.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];
            cmdZoomOutStock.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];
            cmdDeleteDrawings.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdDeleteStock"];
            cmdDeleteDrawings.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDeleteStock"];
        }

        #endregion

        #region Destruction

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
            SuspendEvents();
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


        public void DisposeToPool()
        {

            SuspendEvents();
            ObjectPool.Delete(this);
        }

        public void SuspendEvents()
        {
            DBlocalSQL.SnapshotEvent -= DBDailyShared_SnapshotEvent;
            DBlocalSQL.NewBarEvent -= DBDailyShared_NewBarEvent;
            DBlocalSQL.UpdateBaseEvent -= DBDailyShared_UpdateBaseEvent;

            ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyDown -=
                new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyDown);
            ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyUp -=
                new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyUp);
            ddlStock.SelectedIndexChanged -= DdlStockSelectedIndexChanged_Handler;
            StockChartX1.PaintEvent -= PaintEvent_Handler;


            //Events Handler
            //DdlStockSelectedIndexChanged_Handler =

            // new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DdlStockSelectedIndexChanged);
            //PaintEvent_Handler =
            //new AxSTOCKCHARTXLib._DStockChartXEvents_PaintEventHandler(this.StockChartX1_PaintEvent);
            //DBlocalSQL.SnapshotEvent += DBDailyShared_SnapshotEvent;
            //DBlocalSQL.NewBarEvent += DBDailyShared_NewBarEvent;
            //DBlocalSQL.UpdateBaseEvent += DBDailyShared_UpdateBaseEvent;

            //ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyDown +=
            //    new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyDown);
            //ddlStock.DropDownListElement.TextBox.TextBoxItem.KeyUp +=
            //     new System.Windows.Forms.KeyEventHandler(this.ddlStock_KeyUp);
            // ddlStock.SelectedIndexChanged += DdlStockSelectedIndexChanged_Handler;
            // StockChartX1.PaintEvent += PaintEvent_Handler;

            //var check = new RadCheckBoxElement { ShowBorder = false, Checked = Properties.Settings.Default.VisiblePortfolio };
            ((RadCheckBoxElement)cmdCheckPortfolio.HostedItem).ToggleStateChanged -= CheckToggleStateChanged;
            //cmdCheckPortfolio.HostedItem = check;

            foreach (
                    var item in
                        from radItem in ctmEditChart.Items
                        where radItem.Name.Equals("mnuEsquemaCores")
                        select ((RadMenuItem)radItem))
            {
                //item.Items.Clear();
                foreach (RadMenuItem subItem in item.Items)
                {
                    /*RadMenuItem esquemaCores = new RadMenuItem
                    {
                        AccessibleDescription = scheme.Value,
                        AccessibleName = scheme.Value,
                        Name = scheme.Key,
                        Text = scheme.Value,
                        Visibility = ElementVisibility.Visible
                    };*/

                    subItem.Click -= RadItemClick;

                    //item.Items.AddRange(new RadItem[] { esquemaCores });
                }
            }

            foreach (var item in
                from radItem in ctmEditChart.Items
                where radItem.GetType().Name.Equals("RadMenuItem")
                select ((RadMenuItem)radItem))
            {
                if (item.Items.Count == 0)
                    item.Click -= RadItemClick;
                else
                {
                    foreach (var subItem in from subRadItem in item.Items
                                            where subRadItem.GetType().Name.Equals("RadMenuItem")
                                            select ((RadMenuItem)subRadItem)
                                                into subItem
                                                where subItem != null
                                                select subItem)
                    {
                        subItem.Click -= RadItemClick;
                    }
                }
            }

        }
        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~CtlPainelChart()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region IPoolable

        public void Create()
        {
            Console.WriteLine("\nCreate()");
        }

        public void New()
        {
            Console.WriteLine("\nNew()");
        }

        public void Delete()
        {
            Console.WriteLine("\nDelete()");
        }

        #endregion

        #region Chart Loading
        //Loads a chart into StockChartX
        //NOTE: This is not the real-time chart loading function. See InitRTChart below.
        public void LoadChart(string Symbol)
        {
            StockChartX1.RemoveAllSeries();

            //m_frmMain.cboPriceStyles.HostedControl.SelectedIndex = 0;

            StockChartX1.Symbol = Symbol.Replace(".", "");
            StockChartX1.Visible = true;

            //First add a panel (chart area) for the OHLC data:
            short panel = (short)StockChartX1.AddChartPanel();

            //Now add the open, high, low and close series to that panel:
            StockChartX1.AddSeries(Symbol + ".open", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(Symbol + ".high", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(Symbol + ".low", SeriesType.stCandleChart, panel);
            StockChartX1.AddSeries(Symbol + ".close", SeriesType.stCandleChart, panel);

            //Change the color:
            StockChartX1.set_SeriesColor(Symbol + ".close", ColorTranslator.ToOle(Color.Black));

            //Add the volume chart panel
            if (!m_missingVolume)
            {
                //panel = (short)StockChartX1.AddChartPanel();
                panel = 0;
                StockChartX1.AddSeries(Symbol + ".volume", SeriesType.stVolumeChart, panel);

                //Change volume color and weight of the volume panel:
                StockChartX1.set_SeriesColor(Symbol + ".volume", ColorTranslator.ToOle(Color.Blue));
                StockChartX1.set_SeriesWeight(Symbol + ".volume", 3);

                //Resize the volume panel to make it smaller
                //StockChartX1.set_PanelY1(1, (int)Math.Round(StockChartX1.Height * 0.8));

                //Add another Volume as indicator just for tests:

                //panel = (short)StockChartX1.AddChartPanel();
                //StockChartX1.AddSeries("Indicator-Voolume 1", SeriesType.stVolumeChart, panel);

                //Change volume color and weight of the volume panel:
                //StockChartX1.set_SeriesColor("Indicator-Voolume 1", ColorTranslator.ToOle(Color.Lime));
                //StockChartX1.set_SeriesWeight("Indicator-Voolume 1", 3);
            }
            for (short row = 2; row <= Data.Count - 1; row++)
            {
                StockChartX1.AppendValue(Symbol + ".open", Data[row].jDate, Data[row].OpenPrice);
                StockChartX1.AppendValue(Symbol + ".high", Data[row].jDate, Data[row].HighPrice);
                StockChartX1.AppendValue(Symbol + ".low", Data[row].jDate, Data[row].LowPrice);
                StockChartX1.AppendValue(Symbol + ".close", Data[row].jDate, Data[row].ClosePrice);
                if (!m_missingVolume)
                {
                    StockChartX1.AppendValue(Symbol + ".volume", Data[row].jDate, Data[row].Volume);
                    //StockChartX1.AppendValue("Indicator-Voolume 1", Data[row].jDate, Data[row].Volume);
                }
            }
            /*StockChartX1.ThreeDStyle = false;
            StockChartX1.HorizontalSeparators = true;
            StockChartX1.DisplayTitles = true;
            StockChartX1.VolumePostfixLetter = "M";
            UpdateYScale();*/
            //m_frmMain.MActiveChart = this;
            m_frmMain2.MActiveChart = this;
            // m_frmMain.LoadChartSettings(this);
            m_frmMain2.LoadChartSettings(this);

            Scheme.Instance().UpdateChartColors(StockChartX1, m_SchemeColor);
            //UpdateChartColors(m_frmMain.m_Style);
            StockChartX1.Update();
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
            {
                m_frmMain2.mnuViewScaleType.IsChecked = false;
                m_frmMain2.mnuViewScaleType.Enabled = false;
            }
            else
            {
                m_frmMain2.mnuViewScaleType.Enabled = true;
            }

            ddlStock.SelectedValue = Symbol;
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

            m_frmMain2.mnuDarvasBoxes.IsChecked = StockChartX1.DarvasBoxes;
            m_frmMain2.mnuViewSeparators.IsChecked = StockChartX1.HorizontalSeparators;
            m_frmMain2.mnuView3D.IsChecked = StockChartX1.ThreeDStyle;
            m_frmMain2.mnuViewScaleType.IsChecked = StockChartX1.ScaleType == ScaleType.stLinearScale;
            m_frmMain2.mnuViewShowXGrid.IsChecked = StockChartX1.XGrid;
            m_frmMain2.mnuViewYGrid.IsChecked = StockChartX1.YGrid;
            m_frmMain2.mnuViewCrossHair.IsChecked = false;
            m_frmMain2.mnuViewCrossHair.IsChecked = false;
            m_frmMain2.mnuViewMagnetic.IsChecked = false;
            StockChartX1.Visible = true;
            EnableControls(true);

            //Can't show semi-log if chart is below 1
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1)
            {
                m_frmMain2.mnuViewScaleType.IsChecked = false;
                m_frmMain2.mnuViewScaleType.Enabled = false;
            }
            else
            {
                m_frmMain2.mnuViewScaleType.Enabled = true;
            }

            //Update information about the chart definition
            MSymbol = StockChartX1.Symbol;
            // FROEDE_MARK TODO m_BarSize = StockChartX1.GetBarSize();
            // FROEDE_MARK TODO m_Periodicity = StockChartX1.GetPeriodicity(); 
        }

        //Updates a previously saved chart file with new price data and loads it
        public bool LoadAndUpdateChartFile(string FileName)
        {
            if (!File.Exists(FileName)) return false;

            try
            {
                StockChartX2.LoadFile(FileName);
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
                StockChartX2.SaveFile(FileName);
                if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
                {
                    MCtlData.MFrmMain2.mnuViewScaleType.IsChecked = false;
                    MCtlData.MFrmMain2.mnuViewScaleType.Enabled = false;
                }
                else
                {
                    MCtlData.MFrmMain2.mnuViewScaleType.Enabled = true;
                }
                StockChartX1.LoadFile(FileName);

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
        public void SaveChart(string FileName)
        {
            if (m_Orders.Count > 0) ClearOrders();
            if (FileName.Length == 0)
                FileName = SaveDialog();
            if (FileName == "") return;
            StockChartX1.SaveFile(FileName);
        }
        #endregion

        #region Microsoft Excel Import/Export
        //NOTE: the following functions work at the time of this writing but are unsupported

        //Exports a chart to Excel
        public void ExportToExcel()
        {
            if (StockChartX1.RecordCount < 3)
            {
                Telerik.WinControls.RadMessageBox.Show("A chart must be loaded before using this feature.", " ", MessageBoxButtons.OK,
                                Telerik.WinControls.RadMessageIcon.Error);
                return;
            }

            //m_frmMain.ShowStatus("Exporting " + StockChartX1.Symbol + "...");
            Cursor = Cursors.WaitCursor;

            //List all series
            m_SeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (m_SeriesNames.Count < StockChartX1.SeriesCount)
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
            //m_frmMain.ShowStatus("");
        }

        //Exports a chart to a CSV File
        public void ExportToCSV()
        {
            if (StockChartX1.RecordCount < 3)
            {
                Telerik.WinControls.RadMessageBox.Show("A chart must be loaded before using this feature.", " ", MessageBoxButtons.OK,
                                Telerik.WinControls.RadMessageIcon.Error);
                return;
            }

            //m_frmMain.ShowStatus("Exporting " + StockChartX1.Symbol + "...");
            Cursor = Cursors.WaitCursor;

            //List all series
            m_SeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (m_SeriesNames.Count < StockChartX1.SeriesCount)
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
            //m_frmMain.ShowStatus("");
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
            switch (m_Periodicity)
            {
                case Periodicity.Secondly:
                    title += " " + m_BarSize + " Sec";
                    break;
                case Periodicity.Minutely:
                    title += " " + m_BarSize + " Min";
                    break;
                case Periodicity.Hourly:
                    title += " " + m_BarSize + " Hour";
                    break;
                case Periodicity.Daily:
                    if (m_BarSize > 1)
                        title += " " + m_BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabDaily"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabDaily"];
                    break;
                case Periodicity.Weekly:
                    if (m_BarSize > 1)
                        title += " " + m_BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabWeekly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabWeekly"];
                    break;
                case Periodicity.Month:
                    if (m_BarSize > 1)
                        title += " " + m_BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabMonthly"];
                    else
                        title += " " + Program.LanguageDefault.DictionaryMenuBar["titleTabMonthly"];
                    break;
                case Periodicity.Year:
                    if (m_BarSize > 1)
                        title += " " + m_BarSize + " " + Program.LanguageDefault.DictionaryMenuBar["titleShortTabYearly"];
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
                Telerik.WinControls.RadMessageBox.Show("Excel is not open", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }

            //excel.Application 
            if (objExcel == null)
            {
                Telerik.WinControls.RadMessageBox.Show("Please ensure that Excel is open before" + "\r\n" + "attempting to using this feature", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                return;
            }
            objWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)objExcel.ActiveSheet;
            if (objWorksheet == null)
            {
                Telerik.WinControls.RadMessageBox.Show("Please ensure that Excel is open and a Sheet is" + "\r\n" + "loaded before attempting to using this feature.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                Telerik.WinControls.RadMessageBox.Show("The second column must contain dates (dd/MM/yyyy)." + strDate, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                return;
            }

            if (startRange.Columns.Count < 6)
            {
                StockChartX1.Visible = false;
                Telerik.WinControls.RadMessageBox.Show("Selection must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                return;
            }

            StockChartX1.Visible = false;
            return;

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
            StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".close", ColorTranslator.ToOle(Color.Green));

            //Add volume 
            if (startRange.Columns.Count == 6)
            {
                //Add the volume chart panel 
                Panel = StockChartX1.AddChartPanel();
                StockChartX1.AddSeries(StockChartX1.Symbol + ".volume", SeriesType.stVolumeChart, Panel);
                //Change volume color and weight of the volume panel: 
                StockChartX1.set_SeriesColor(StockChartX1.Symbol + ".volume", ColorTranslator.ToOle(Color.Blue));
                StockChartX1.set_SeriesWeight(StockChartX1.Symbol + ".volume", 3);
                //Resize the volume panel to make it smaller 
                StockChartX1.set_PanelY1(1, (int)(StockChartX1.Height * 0.8));
            }


            //All custom series (added to right of volume) are inserted as line charts overlaying the main OHLC chart 
            int customSeries = startRange.Columns.Count - 6;
            int n;
            int p;
            for (n = 1; n <= customSeries; n++)
            {
                p = StockChartX1.AddChartPanel();
                StockChartX1.AddSeries("Series " + n + 6, SeriesType.stLineChart, p);
            }


            //Insert values into StockChartX 
            double jdate = 0;
            double Value;
            int hr;
            int mn;
            int sc;

            Cursor = Cursors.WaitCursor;
            //m_frmMain.ShowStatus("Importing " + StockChartX1.Symbol + "...");
            Application.DoEvents();

            //Copy entire selection into memory 
            //object[,] ExcelData = new object[1, 1]; 
            //ExcelData = (object[,])objWorksheet.get_Range(startRange.Rows, startRange.Columns);

            //object[,] ExcelData; 
            //ExcelData = (object[,])objWorksheet.get_Range(startRange.Rows, startRange.Columns);

            DateTime date1;
            DateTime.TryParse(getCellValue(objWorksheet, offRow, offCol, 1, 1), out date1);
            DateTime date2;
            DateTime.TryParse(getCellValue(objWorksheet, offRow, offCol, 2, 1), out date2);

            int rowCount = startRange.get_Offset(Type.Missing, Type.Missing).Rows.Count;
            int colCount = startRange.get_Offset(Type.Missing, Type.Missing).Columns.Count;

            //Descending 
            if (date2 > date1)
            {

                for (row = 1; row <= rowCount; row++)
                {
                    for (col = 1; col <= colCount; col++)
                    {

                        if (col == 1)
                        {
                            strDate = getCellValue(objWorksheet, offRow, offCol, row, 1);
                            DateTime dt;
                            DateTime.TryParse(strDate, out dt);
                            hr = dt.Hour;
                            mn = dt.Minute;
                            sc = dt.Second;
                            if (dt.Hour == 0)
                            {
                                hr = 12;
                                mn = 0;
                                sc = 0;
                            }
                            jdate = StockChartX1.ToJulianDate(dt.Year, dt.Month, dt.Day, hr, mn, sc);
                        }

                        else
                        {

                            Double.TryParse(getCellValue(objWorksheet, offRow, offCol, row, col), out Value);

                            switch (col)
                            {
                                case 2:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".open", jdate, Value);
                                    break;
                                case 3:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".high", jdate, Value);
                                    break;
                                case 4:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".low", jdate, Value);
                                    break;
                                case 5:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".close", jdate, Value);
                                    break;
                                case 6:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".volume", jdate, Value);
                                    break;
                                default:
                                    //Custom series
                                    string strValue = getCellValue(objWorksheet, offRow, offCol, row, col);
                                    if (string.IsNullOrEmpty(strValue)) Value = -987654321;
                                    StockChartX1.AppendValue("Series " + col, jdate, Value);
                                    break;
                            }
                        }

                    }

                }
            }

            else
            {

                for (row = rowCount; row >= 1; row += -1)
                {

                    for (col = 1; col <= colCount; col++)
                    {

                        if (col == 1)
                        {
                            strDate = getCellValue(objWorksheet, offRow, offCol, row, 1);
                            DateTime dt;
                            DateTime.TryParse(strDate, out dt);
                            hr = dt.Hour;
                            mn = dt.Minute;
                            sc = dt.Second;
                            if (dt.Hour == 0)
                            {
                                hr = 12;
                                mn = 0;
                                sc = 0;
                            }

                            jdate = StockChartX1.ToJulianDate(dt.Year, dt.Month, dt.Day, hr, mn, sc);
                        }

                        else
                        {

                            Double.TryParse(getCellValue(objWorksheet, offRow, offCol, row, col), out Value);

                            switch (col)
                            {
                                case 2:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".open", jdate, Value);
                                    break;
                                case 3:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".high", jdate, Value);
                                    break;
                                case 4:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".low", jdate, Value);
                                    break;
                                case 5:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".close", jdate, Value);
                                    break;
                                case 6:
                                    StockChartX1.AppendValue(StockChartX1.Symbol + ".volume", jdate, Value);
                                    break;
                                default:
                                    //Custom series 
                                    if (string.IsNullOrEmpty(getCellValue(objWorksheet, offRow, offCol, row, col))) Value = -987654321;
                                    StockChartX1.AppendValue("Series " + col, jdate, Value);
                                    break;
                            }
                        }

                    }

                }
            }

            //m_frmMain.ShowStatus("");
            Cursor = Cursors.Arrow;
            EnableControls(true);

            //Change some display properties: 
            StockChartX1.ThreeDStyle = false;
            StockChartX1.UpColor = Color.Green;
            StockChartX1.DownColor = Color.Red;
            StockChartX1.DisplayTitles = true;

            //Update the chart 
            StockChartX1.Update();

            //Can't show semi-log if chart is below 1 
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1)
            {
                m_frmMain2.mnuViewScaleType.IsChecked = false;
                m_frmMain2.mnuViewScaleType.Enabled = false;
            }
            else
            {
                m_frmMain2.mnuViewScaleType.Enabled = true;
            }

            StockChartX1.Visible = true;

            MSymbol = StockChartX1.Symbol;

            //Restore and activate 
            if (!(UnmanagedMethods.IsWindowVisible(this.Handle)))
            {
                UnmanagedMethods.ShowWindow(this.Handle, UnmanagedMethods.SW_SHOW);
            }
            if ((UnmanagedMethods.IsIconic(this.Handle)))
            {
                UnmanagedMethods.SendMessage(this.Handle, UnmanagedMethods.WM_SYSCOMMAND, UnmanagedMethods.SC_RESTORE, IntPtr.Zero);
            }
            UnmanagedMethods.SetForegroundWindow(this.Handle);

            /*
          catch{

              EnableControls(false); 
              StockChartX1.Visible = false;    
              Telerik.WinControls.RadMessageBox.Show("Invalid data selection. Please try again!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);

          }
        */


        }

        //Imports data from a CSV data file - if available 
        public void ImportFromCSV()
        {
            //Source source = new Source(StockChartX1, m_frmMain);
            //source.ImportFromCsv();

            //MSymbol = source.MSymbol;

            ////Restore and activate 
            //if (!(UnmanagedMethods.IsWindowVisible(this.Handle)))
            //{
            //    UnmanagedMethods.ShowWindow(this.Handle, UnmanagedMethods.SW_SHOW);
            //}
            //if ((UnmanagedMethods.IsIconic(this.Handle)))
            //{
            //    UnmanagedMethods.SendMessage(this.Handle, UnmanagedMethods.WM_SYSCOMMAND, UnmanagedMethods.SC_RESTORE, IntPtr.Zero);
            //}
            //UnmanagedMethods.SetForegroundWindow(this.Handle);

            //return;

            bool goodSoFar;

            //Looking for the input file
            string fName = OpenDialog("CSV Stock Chart Files|*.csv");
            if (!File.Exists(fName))
            {
                Telerik.WinControls.RadMessageBox.Show("Unable to locate the file " + fName, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                Telerik.WinControls.RadMessageBox.Show(" 0 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                Telerik.WinControls.RadMessageBox.Show(" 1 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                Telerik.WinControls.RadMessageBox.Show(" 2 Each row must be Symbol, Date, Time (optional), Open, High, Low, Close, and Volume (optional).", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                StockChartX1.Height = Height - 80;

            StockChartX1.set_PanelY1(1, (int)(StockChartX1.Height * 0.8));

            //Insert values into StockChartX 
            double jdate = 0;
            int hr;
            int mn;
            int sc;

            Cursor = Cursors.WaitCursor;
            //m_frmMain.ShowStatus("Importing " + StockChartX1.Symbol + "...");
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

            //m_frmMain.ShowStatus("");
            Cursor = Cursors.Arrow;
            EnableControls(true);

            //Change some display properties: 
            StockChartX1.ThreeDStyle = false;
            StockChartX1.UpColor = Color.FromArgb(51, 204, 51); // FROEDE_MARK Green;
            StockChartX1.DownColor = Color.FromArgb(255, 80, 80); // FROEDE_MARK Red;
            StockChartX1.DisplayTitles = true;

            //Update the chart 
            StockChartX1.Update();

            //Can't show semi-log if chart is below 1 
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1)
            {
                m_frmMain2.mnuViewScaleType.IsChecked = false;
                m_frmMain2.mnuViewScaleType.Enabled = false;
            }
            else
            {
                m_frmMain2.mnuViewScaleType.Enabled = true;
            }

            StockChartX1.Visible = true;

            MSymbol = StockChartX1.Symbol;

            //Restore and activate 
            if (!(UnmanagedMethods.IsWindowVisible(this.Handle)))
            {
                UnmanagedMethods.ShowWindow(this.Handle, UnmanagedMethods.SW_SHOW);
            }
            if ((UnmanagedMethods.IsIconic(this.Handle)))
            {
                UnmanagedMethods.SendMessage(this.Handle, UnmanagedMethods.WM_SYSCOMMAND, UnmanagedMethods.SC_RESTORE, IntPtr.Zero);
            }
            UnmanagedMethods.SetForegroundWindow(this.Handle);

            /*
          catch{

              EnableControls(false); 
              StockChartX1.Visible = false;    
              Telerik.WinControls.RadMessageBox.Show("Invalid data selection. Please try again!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);

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
        public void ChangePriceStyle(string hostedControlText, bool force = false)
        {
            if (hostedControlText == m_PriceStyle && !force) return;
            _hostedControlText = hostedControlText;
            StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            m_PriceStyle = hostedControlText;

            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                    LoadScroll();
                    UpdateMenus();
                    StockChartX1.Visible = true;
                    return;
                }
                return;
            }));

        }

        //Draws a horizontal line on the chart under the mouse pointer
        private void mnucHorzLine_Click(object sender, CommandEventArgs commandEventArgs)
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


            //StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));

        }

        // Draws a vertical line on the chart. NOTE: StockChartX does NOT support click events for vertical lines, due to the 
        // individual "panel" design. This behavior is different from horizontal lines (which were originally intended for 
        // "80/20" indicator reference lines. You may want to remove the vertical lines if the inconsistent usage is a concern.
        private void mnucVertLine_Click(object sender, CommandEventArgs e)
        {
            // NOTE: by design, vertical lines that extend through more than one panel cannot be selected in StockChartX.     
            //string key = "vline" + DateTime.Now.Ticks;

            //StockChartX1.DrawTrendLine(StockChartX1.GetPanelBySeriesName(StockChartX1.Symbol + ".volume"),
            //  (double)DataType.dtNullValue, m_Record, (double)DataType.dtNullValue, m_Record, key);
            //StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));      

            string key = "vline" + DateTime.Now.Ticks;

            StockChartX1.DrawTrendLine(StockChartX1.CurrentPanel,
              (double)DataType.dtNullValue, _mRecord, (double)DataType.dtNullValue, _mRecord, key);
            //StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(StockChartX1.ChartForeColor));

        }

        //Adds a new indicator to the chart and requests the user to enter the parameters
        public void AddIndicator()
        {
            if (StockChartX1.RecordCount < 3) return;
            string cnt = "";
            // int n = StockChartX1.GetIndicatorCountByType((Indicator)m_frmMain.cboIndicators.HostedControl.SelectedIndex);
            //if (n > 0)
            //{
            //    cnt = " " + (n + 1);
            //}
            //  int panel = IsOverlay(m_frmMain2.cboIndicators.HostedControl.Text) ? 0 : StockChartX1.AddChartPanel();
            //  int indicator = m_frmMain2.cboIndicators.HostedControl.SelectedIndex;
            //  StockChartX1.AddIndicatorSeries((Indicator)indicator, m_frmMain.cboIndicators.HostedControl.Text + cnt, panel, true);
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
                //m_frmMain.cboPriceStyles.Enabled = Enable;
                //m_frmMain.cboIndicators.Enabled = Enable;
                //m_frmMain.cmdZoomIn.Enabled = Enable;
                //m_frmMain.cmdZoomOut.Enabled = Enable;
                //m_frmMain.cmdScrollLeft.Enabled = Enable;
                //m_frmMain.cmdTemplate.Enabled = Enable;
                //m_frmMain.cmdScrollRight.Enabled = Enable;
                //m_frmMain.cmdDelete.Enabled = Enable;
                //m_frmMain.mnuExcel.Enabled = Enable;
                //m_frmMain.mnuPatternRecognition.Enabled = Enable;
                //m_frmMain.cmdPrintChart.Enabled = Enable;
                m_frmMain2.mnuFileSaveImage.Enabled = Enable;
                //m_frmMain.mnuFileSaveTemplate.Enabled = Enable;
                //m_frmMain.mnuFileExport.Enabled = Enable;
                //m_frmMain.mnuFileExportCSV.Enabled = Enable;
                //m_frmMain2.mnuFileImportCSV.Enabled = true; // FROEDE_MARK Enable;
                m_frmMain2.mnuFilePrint.Enabled = Enable;
                m_frmMain2.mnuView3D.Enabled = Enable;
                m_frmMain2.mnuAppStyle.Enabled = Enable;

                //if (Enable)
                //{
                //    foreach (KeyValuePair<string, string> scheme in Scheme.Instance().Schemes)
                //    {
                //        if (!m_SchemeColor.Equals(scheme.Key))
                //            continue;

                //        foreach (NCommand nCommand in m_frmMain.mnuAppStyle.Commands)
                //            nCommand.Checked = nCommand.Properties.Text.ToUpper().Equals(scheme.Value.ToUpper());

                //        break;
                //    }
                //}

                m_frmMain2.mnuViewScaleType.Enabled = Enable;
                m_frmMain2.mnuViewSeparators.Enabled = Enable;
                m_frmMain2.mnuViewShowXGrid.Enabled = Enable;
                m_frmMain2.mnuViewYGrid.Enabled = Enable;
                m_frmMain2.mnuViewCrossHair.Enabled = Enable;
                m_frmMain2.mnuDarvasBoxes.Enabled = Enable;
                m_frmMain2.mnuChartColors.Enabled = Enable;
                //m_frmMain.mnuTools.Enabled = Enable;
                //m_frmMain.mnuZoomIn.Enabled = Enable;
                //m_frmMain.mnuZoomOut.Enabled = Enable;
                //m_frmMain.mnuApplyTemplate.Enabled = Enable;
                //m_frmMain.mnuScrollLeft.Enabled = Enable;
                //m_frmMain.mnuScrollRight.Enabled = Enable;
                //if (StockChartX1.PriceStyle == PriceStyle.psStandard)
                //    m_frmMain.mnuPriceStyle.Enabled = false;
                //else
                //    m_frmMain.mnuPriceStyle.Enabled = Enable;
                //m_frmMain.mnuApplyExpertAdvisor.Enabled = Enable;
                //m_frmMain.mnuConsensusReport.Enabled = Enable;
                //m_frmMain.mnuNN.Enabled = Enable;
                m_frmMain2.mnuChart.Enabled = Enable;
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
            StockChartX1.RemoveObject((ObjectType)m_objectType, m_name);
        }

        //Deletes a series    
        private void mnuDeleteSeries_Click(object sender, CommandEventArgs e)
        {
            DialogResult result = Telerik.WinControls.RadMessageBox.Show("Remover series?", " ", MessageBoxButtons.YesNo,
                                                  Telerik.WinControls.RadMessageIcon.Question);
            if (result == DialogResult.No) return;
            StockChartX1.RemoveSeries(m_name);
        }

        //Removes all drawings from the chart
        public void DeleteDrawings()
        {
            DialogResult result = RadMessageBox.Show(this, Program.LanguageDefault.DictionaryMessage["msgRemoveAllDrawings"], "Pergunta", MessageBoxButtons.YesNo, RadMessageIcon.Question);
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
        private void CtlPainelChart_Resize(object sender, EventArgs e)
        {
            /*radCommandBar1.Visible = false;
            radCommandBar1.Location = new Point((this.Width / 2) - (520 / 2), commandBarStripElement1.Location.Y);
            if (radCommandBar1.Location.X + radCommandBar1.Size.Width > this.Width) radCommandBar1.Location = new Point(0, radCommandBar1.Location.Y);
            if (Width < 520) radCommandBar1.Location = new Point(0, radCommandBar1.Location.Y);
            if (radCommandBar1.Size.Width > 520) radCommandBar1.Width = 520;
            radCommandBar1.AutoSize = true;*/

            //WARNING: StockChartX1 dock must be NONE
            StockChartX1.Top = 2;
            StockChartX1.Left = 2;
            StockChartX1.Height = Height - 80;

            //if (radDock1.Visible)
            //{
            //    radDock1.Dock = DockStyle.Left;
            //    radDock1.Left = 0;
            //    radDock1.Height = Height;
            //    StockChartX1.Left = radDock1.Width + 2;
            //    StockChartX1.Width = Width - radDock1.Width - 4;
            //}
            //else 

            if (pnlConsensus.Visible)
            {
                pnlTwitter.Visible = false;
                pnlConsensus.Height = Height;
                pnlConsensus.Left = radPanel1.Width - pnlConsensus.Width;
                rtbConsensus.Height = Height - rtbConsensus.Top;
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

            RepositionEAButton();

            LoadScroll();
        }

        public void LoadScroll()
        {
            if (m_StopLoadScroll || StockChartX1.RecordCount == 0 || (StockChartX1.LastVisibleRecord - StockChartX1.FirstVisibleRecord) == 0)
                return;

            scrollChart.Width = radPanel1.Width;
            //scrollChart.Location = new Point(Location.X, StockChartX1.Height - (scrollChart.Height));

            scrollChart.Maximum = StockChartX1.RecordCount;
            scrollChart.Minimum = 1;
            scrollChart.Value = StockChartX1.FirstVisibleRecord;

            decimal d = Decimal.Round(StockChartX1.RecordCount / (decimal)(StockChartX1.LastVisibleRecord - StockChartX1.FirstVisibleRecord), 1);
            if (d < 1) d = 1;
            decimal e = Decimal.Round(scrollChart.Maximum / d, 0);
            scrollChart.LargeChange = (int)e;
            //Right, Top and Bottom Space            
            SetChartPadding(paddingTop, paddingBottom, paddingRight);
            if (!m_StopSaveViewport) SaveViewportJDate();

        }

        //New Scroll Bar
        private void ScrollChartScroll(object sender, ScrollEventArgs e)
        {
            scrollChart.Update();
            //Scroll Left
            if ((e.NewValue - e.OldValue) < 0)
            {
                StockChartX1.ScrollLeft(e.OldValue - e.NewValue);
                _mRecord = (int)StockChartX1.GetXRecordByPixel(StockChartX1.PointToClient(MousePosition).X);
                SaveViewportJDate();
                //StockChartX1_MouseMoveEvent(new object());
                return;
            }

            //Scroll Right
            StockChartX1.ScrollRight(e.NewValue - e.OldValue);
            _mRecord = (int)StockChartX1.GetXRecordByPixel(StockChartX1.PointToClient(MousePosition).X);
            SaveViewportJDate();
            //StockChartX1_MouseMoveEvent(new object());
        }

        #endregion

        #region StockChartX Events

        private void StockChartX1Resize(object sender, EventArgs e)
        {
            //LoadScroll();
            /*if (m_frmMain.WindowState == FormWindowState.Maximized) LabelMouseY.Location = m_frmMain.PointToClient(new Point(StockChartX1.Width - 86, MousePosition.Y));
            else LabelMouseY.Location = m_frmMain.PointToClient(new Point(StockChartX1.Width - 78, MousePosition.Y));
            LabelMouseY.Text = StockChartX1.GetYValueByPixel(MousePosition.Y).ToString("N3").PadRight(20, ' ').Replace(',', '.');*/
            //commandBarStripElement1.Location = new Point((this.Width / 2) - (commandBarStripElement1.Size.Width / 2), commandBarStripElement1.Location.Y);
            //if (commandBarStripElement1.Location.X + commandBarStripElement1.Size.Width > StockChartX1.Width) commandBarStripElement1.Location = new Point(0, commandBarStripElement1.Location.Y);

        }

        //Fired on Zoom finished
        private void StockChartX1_Zoom(object sender, EventArgs e)
        {
            LoadScroll();
            StockChartX1.ResetYScale(0);
            double max = Convert.ToDouble(((double)Properties.Settings.Default.PaddingTop / 100) * (StockChartX1.GetVisibleMaxValue(StockChartX1.Symbol + ".high") - StockChartX1.GetVisibleMinValue(StockChartX1.Symbol + ".low")) + StockChartX1.GetVisibleMaxValue(StockChartX1.Symbol + ".high"));
            double min = Convert.ToDouble(StockChartX1.GetVisibleMinValue(StockChartX1.Symbol + ".low") - ((double)Properties.Settings.Default.PaddingBottom / 100) * (StockChartX1.GetVisibleMaxValue(StockChartX1.Symbol + ".high") - StockChartX1.GetVisibleMinValue(StockChartX1.Symbol + ".low")));
            StockChartX1.SetYScale(0, max, min);
            StockChartX1.Update();
        }

        //Lists all available indicators. Fires after StockChartX1.EnumIndicators is called.
        private void StockChartX1_EnumIndicator(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEvent e)
        {
            //m_frmMain.cboIndicators.Items.Add(e.indicatorName);
            string indicatorName = e.indicatorName.ToLower();
            //if (indicatorName.IndexOf("bands") != -1 || indicatorName.IndexOf("macd") != -1 || indicatorName.IndexOf("stochastic oscillator") != -1)
            // m_frmMain.cboIndicators.Items[m_frmMain.cboIndicators.Items.Count - 1].ImageIndex = 10;
            //else
            // m_frmMain.cboIndicators.Items[m_frmMain.cboIndicators.Items.Count - 1].ImageIndex = 9;
        }

        // Lists all series added to the chart
        public List<string> GetSeries()
        {
            m_SeriesNames.Clear();
            StockChartX1.EnumSeries();
            while (m_SeriesNames.Count < StockChartX1.SeriesCount)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }
            return m_SeriesNames;
        }

        //Fires after StockChartX1.EnumSeries is called.
        private void StockChartX1_EnumSeriesEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_EnumSeriesEvent e)
        {
            m_SeriesNames.Add(e.name);
        }

        //Has an item been clicked on?
        //This fires for Text, Symbol, or line objects as well as series.
        private void StockChartX1_ItemRightClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemRightClickEvent EventArgs)
        {
            _mMenu = true;
            frmMain2.GInstance.MActiveChart = this;
            Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };
            m_name = EventArgs.name;
            m_objectType = (int)EventArgs.objectType;
            //Telerik.WinControls.RadMessageBox.Show("name:" + m_name + "\ntype=" + EventArgs.objectType);

            switch (EventArgs.objectType)
            {
                case ObjectType.otVolumeChart:
                case ObjectType.otCandleChart:
                case ObjectType.otStockBarChart:
                    if (m_name.IndexOf(StockChartX1.Symbol + ".") > -1) return;
                    ctmDeleteSeries.Show(this, p);
                    StockChartX1.Update();
                    break;
                /*case ObjectType.otIndicator:        //Indicator
                case ObjectType.otLineChart:        //Indicator
                case ObjectType.otLineObject:       //Study
                case ObjectType.otTrendLineObject:  //Study
                case ObjectType.otTextObject:
                case ObjectType.otSymbolObject:*/
                default:
                    ctmDeleteOrEdit.Show(m_frmMain2, Cursor.Position.X + Left, Cursor.Position.Y + Top);
                    break;
            }
        }
        private void StockChartX1_MouseMoveEvent(object sender)
        {
            Point MouseP = StockChartX1.PointToClient(MousePosition);
            StockChartX1_MouseMoveEvent(sender,
                                        new AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEvent(MouseP.X,
                                                                                                MouseP.Y,
                                                                                                _mRecord));
        }
        private void StockChartX1_MouseMoveEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEvent e)
        {
            if (DrawingLineStudy && _actionChart != m_frmMain2._actionChart)
            {
                DrawingLineStudy = false;
                //Console.WriteLine("\n\nStockChartX1_MouseMoveEvent(): MUserEditing = false");
                MUserEditing = false;
                StockChartX1.AbortDrawing();
                //Telerik.WinControls.RadMessageBox.Show("Abort Draw!");
            }
            //tmrLabels.Enabled = false;
            LabelMouseX.Visible = true;
            LabelMouseY.Visible = true;
            // Telerik.WinControls.RadMessageBox.Show("MousePosition=(" + MousePosition.X + "," + MousePosition.Y + ")\ne=(" + StockChartX1.PointToScreen(radCommandBar1.Location).X + "," + StockChartX1.PointToScreen(radCommandBar1.Location).Y +
            //                ")");
            if (grpOrder.Visible)
                return;

            _mValue = StockChartX1.GetYValueByPixel(StockChartX1.MousePositionY);
            //if ((_mValue * 100) % 2 == 0) StockChartX1.GetYValueByPixel(e.y-1);
            _mRecord = e.record;


            // Show ScrollBar on bottom?
            /*if (e.y >= scrollChart.Location.Y - 10)
            {
                scrollChart.Visible = true;
            }
            else
            {
                scrollChart.Visible = false;
            }*/
            // Show MenuBar on top?
            /*if (!DrawingLineStudy)
            {
                timerMnuAutoHideIn.Enabled = true;
            }*/
            //Telerik.WinControls.RadMessageBox.Show("MouseX=" + MouseP.X + " e.X=" + e.x + "\nMouseY=" + MouseP.Y + " e.Y=" + e.y);
            int pad;
            string labelText = "";
            string sDateTime = "";
            string sDate;
            string sTime;
            try
            {
                if (MousePosition.Y > StockChartX1.PointToScreen(StockChartX1.Location).Y + 5 && MousePosition.Y < StockChartX1.PointToScreen(StockChartX1.Location).Y + StockChartX1.Height - 25)
                {
                    LabelMouseY.Location = new Point(StockChartX1.Location.X + StockChartX1.Width - 59, StockChartX1.PointToClient(MousePosition).Y + (LabelMouseY.Height) / 2 + 3);
                    string scale = "";
                    if (_mValue >= 1000000000)
                    {
                        _mValue = _mValue / 1000000000;
                        scale = "B";
                    }
                    else if (_mValue >= 1000000)
                    {
                        _mValue = _mValue / 1000000;
                        scale = "M";
                    }
                    else if (_mValue >= 1000)
                    {
                        _mValue = _mValue / 1000;
                        scale = "K";
                    }
                    switch (Properties.Settings.Default.Decimals)
                    {
                        case 0:
                            labelText = _mValue.ToString("N1").Replace(',', '.');
                            labelText += ".000";
                            break;
                        case 1:
                            labelText = _mValue.ToString("N1").Replace(',', '.');
                            labelText += "00";
                            break;
                        case 2:
                            labelText = _mValue.ToString("N2").Replace(',', '.');
                            labelText += "0";
                            break;
                        case 3:
                            labelText = _mValue.ToString("N3").Replace(',', '.');
                            break;
                    }
                    int trim = 3 - Properties.Settings.Default.Decimals;
                    if (trim < 0) trim = 0;
                    else if (trim == 3) trim = 4;
                    labelText = labelText.Remove(labelText.Length - trim, trim);
                    labelText += scale;
                    if (labelText.Contains("-987.654.321")) labelText = "NA";
                    if (labelText.Length < 14) pad = (14 - labelText.Length) / 2;
                    else pad = 0;
                    labelText = labelText.PadLeft(labelText.Length + pad, ' ');
                    labelText = labelText.PadRight(labelText.Length + pad, ' ');
                    LabelMouseY.Text = labelText;
                    //LabelMouseY.Size = new System.Drawing.Size(370, 18);
                }

                if (MousePosition.X > StockChartX1.PointToScreen(StockChartX1.Location).X && MousePosition.X < StockChartX1.PointToScreen(StockChartX1.Location).X + StockChartX1.Width)
                {
                    sDateTime =
                        StockChartX1.FromJulianDate(StockChartX1.GetJDate(StockChartX1.get_SeriesName(0), e.record));

                    if (sDateTime != "" && sDateTime != "error")
                    {
                        Point location;
                        sDate = sDateTime.Substring(0, sDateTime.IndexOf(" ")).Trim();
                        sTime = sDateTime.Substring(11, 5).Trim();
                        LabelMouseX.Text = (sTime != "00:00" ? sDate + " " + sTime : sDate);
                        if (StockChartX1.PointToClient(MousePosition).X > StockChartX1.Location.X + LabelMouseX.Width / 2 + 1)
                        {
                            //LabelMouseX.Location = new Point(StockChartX1.PointToClient(MousePosition).X - LabelMouseX.Width / 2, StockChartX1.Location.Y + Height - 40);
                            LabelMouseX.Location = new Point(StockChartX1.PointToClient(MousePosition).X - LabelMouseX.Width / 2, StockChartX1.Height + 3);
                        }

                    }
                }
                //if (scrollChart.Visible) LabelMouseX.Location = new Point(LabelMouseX.Location.X, StockChartX1.Height - 20);
            }
            catch (Exception ex)
            {
                //Telerik.WinControls.RadMessageBox.Show(ex.Message+"\nsDateTime="+sDateTime);
            }

            //Draw CrossHairs
            //if (CrossHairs) DrawCrossHairs();
            tmrLabels.Enabled = true;
        }

        private void StockChartX1_OnRButtonDown(object sender, EventArgs e)
        {
            frmMain2.GInstance.MActiveChart = this;
            _mMenu = false; //No context menu is shown
            MUserEditing = true;
        }

        private void StockChartX1_OnRButtonUp(object sender, EventArgs e)
        {
            if (_mMenu)
            {
                _mMenu = false;
                return; //Another context menu is shown 
            }
            //if (StockChartX1.SelectedKey != "") return;
            frmMain2.GInstance.MActiveChart = this;
            //ContextMenu (BUY | SELL | HORIZONTAL | VERTICAL):
            /*Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };
            ctmLines.Show(this, p);
            tmrEdit.Enabled = true;*/

            //Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };
            //ctmEditChart.Show(this, p);

            //Save the panel's index:
            _lastPanelRClicked = 0;
            for (int i = 0; i < StockChartX1.PanelCount; i++)
            {

                /*if(i==0)
                {
                    MessageBox.Show("Y1=" + StockChartX1.get_PanelY1(i)+ 
                        "\nPointToScreen(Y1)=" + StockChartX1.PointToScreen(new Point(0, StockChartX1.get_PanelY1(i))).Y+
                        "\nPointToClient(Y1)=" + StockChartX1.PointToClient(new Point(0, StockChartX1.get_PanelY1(i))).Y +
                        "\nY2=" + StockChartX1.get_PanelY2(i) +
                        "\nPointToScreen(Y2)=" + StockChartX1.PointToScreen(new Point(0, StockChartX1.get_PanelY2(i))).Y +
                        "\nPointToClient(Y2)=" + StockChartX1.PointToClient(new Point(0, StockChartX1.get_PanelY2(i))).Y +
                        "\nY=" + MousePosition.Y + Top +
                        "\nPointToScreen(Y)=" + StockChartX1.PointToScreen(new Point(0, MousePosition.Y + Top)).Y +
                        "\nPointToClient(Y)=" + StockChartX1.PointToClient(new Point(0, MousePosition.Y + Top)).Y +
                        "\n_lastP=" + _lastPanelRClicked);
                }*/

                if (StockChartX1.get_PanelY1(i) < StockChartX1.PointToClient(new Point(0, MousePosition.Y + Top)).Y + Top && StockChartX1.get_PanelY2(i) > StockChartX1.PointToClient(new Point(0, MousePosition.Y + Top)).Y + Top) _lastPanelRClicked = i;

            }
            ctmEditChart.Show(m_frmMain2, Cursor.Position.X + Left, Cursor.Position.Y + Top);

            LoadCheckMenuRight();
        }



        private void StockChartX1_ClickEvent(object sender, EventArgs e)
        {
            if (m_frmMain2.GetHorizontalLine)
            {
                mnucHorzLine_Click(null, null);
                m_frmMain2.GetHorizontalLine = false;
                return;
            }

            if (m_frmMain2.GetVerticaoLine)
            {
                mnucVertLine_Click(null, null);
                m_frmMain2.GetVerticaoLine = false;
                return;
            }
            DrawSelection();
        }

        private void StockChartX1_ItemDoubleClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemDoubleClickEvent EventArgs)
        {
            Point p = new Point { X = (Cursor.Position.X + Left), Y = (Cursor.Position.Y + Top) };

            m_name = EventArgs.name;
            m_objectType = (int)EventArgs.objectType;
            if ((m_objectType == (int)ObjectType.otIndicator) || (m_objectType == (int)ObjectType.otLineChart)) EditIndicator();
            else if ((m_objectType == (int)ObjectType.otLineObject) || (m_objectType == (int)ObjectType.otTrendLineObject)) EditStudy();
        }

        private void EditIndicator()
        {

            tmrSelectIndicator.Enabled = true;

        }

        private void EditStudy()
        {
            _frmSelectStudy.StockChartX1 = StockChartX1;
            //Telerik.WinControls.RadMessageBox.Show("Object="+ m_objectType);
            switch (m_objectType)
            {
                case (int)ObjectType.otTrendLineObject:
                    if ((m_name[0] == 'T') && (m_name[1] == 'L')) //TREND LINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "TrendLine";
                    }
                    else if ((m_name[0] == 'R') && (m_name[1] == 'L')) //RAY LINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "RayLine";
                    }
                    else if ((m_name[0] == 'V') && (m_name[1] == 'L')) //VERTICAL LINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "VerticalLine";
                    }
                    else if ((m_name[0] == 'H') && (m_name[1] == 'L')) //HORIZONTAL LINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "HorizontalLine";
                    }
                    else if ((m_name[0] == 'R') && (m_name[1] == 'E')) //RECTANGLE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Rectangle";
                    }
                    else if ((m_name[0] == 'E') && (m_name[1] == 'L')) //ELIPSE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Ellipse";
                    }
                    else if ((m_name[0] == 'A') && (m_name[1] == 'R')) //ARROW
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Arrow";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'H')) //FREE HAND
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "FreeHand";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'H')) //FREE HAND
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "FreeHand";
                    }
                    else if ((m_name[0] == 'P') && (m_name[1] == 'L')) //POLYLINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Polyline";
                    }
                    else if ((m_name[0] == 'C') && (m_name[1] == 'H')) //CHANNEL
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Channel";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'A')) //FIBONACCI ARCS
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Fibonacci Arcs";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'F')) //FIBONACCI FAN
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Fibonacci Fan";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'P')) //FIBONACCI PROJECTIONS
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Fibonacci Projections";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'R')) //FIBONACCI RETRACEMENTS
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Fibonacci Retracements";
                    }
                    else if ((m_name[0] == 'F') && (m_name[1] == 'T')) //FIBONACCI TIMEZONES
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Fibonacci Timezones";
                    }
                    else if ((m_name[0] == 'S') && (m_name[1] == 'L')) //SPEED LINE
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Speed Line";
                    }
                    else if ((m_name[0] == 'G') && (m_name[1] == 'F')) //GANN FAN
                    {
                        _frmSelectStudy.LineStudyKey = m_name;
                        _frmSelectStudy.LineStudyType = "Gann Fan";
                    }
                    break;
                default:
                    _frmSelectStudy.LineStudyKey = m_name;
                    _frmSelectStudy.LineStudyType = "StudyLine";
                    break;
            }
            _frmSelectStudy.ShowDialog();
        }

        /*private void RemoveIndicator()
        {
            if (Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgRemoveIndicatorMock"], "", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                return;

            StockChartX1.RemoveSeries(m_name);
            StockChartX1.Update();

            _frmSelectIndicator._mock.RemoveIndicatorMock(_frmSelectIndicator._mock.GetIndicatorMockByCodeMock(Regex.Replace(m_name, @"\d", ""), m_name));
            _frmSelectIndicator.LoadIndicators(null);
        }*/

        private void RemoveStudy()
        {
            // No confirmation
            //if (Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgRemoveStudy"], "", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            //    return;

            StockChartX1.RemoveObject((STOCKCHARTXLib.ObjectType)m_objectType, m_name);
            StockChartX1.Update();
        }

        private void ctmEditButtonClick(object sender, EventArgs e)
        {
            //Indicator?
            if ((m_objectType == (int)ObjectType.otIndicator) || (m_objectType == (int)ObjectType.otLineChart)) EditIndicator();
            //Line Study?
            else if ((m_objectType == (int)ObjectType.otLineObject) || (m_objectType == (int)ObjectType.otTrendLineObject))
            {
                EditStudy();
            }

        }

        private void ctmDeleteButtonClick(object sender, EventArgs e)
        {
            //Indicator?
            if ((m_objectType == (int)ObjectType.otIndicator) || (m_objectType == (int)ObjectType.otLineChart)) StockChartX1_RemoveSeries(m_name);
            //Study Line?
            else if ((m_objectType == (int)ObjectType.otLineObject) || (m_objectType == (int)ObjectType.otTrendLineObject)) RemoveStudy();
            else StockChartX1.RemoveObject((ObjectType)m_objectType, m_name);

        }

        private void StockChartX1_OnKeyUp(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_OnKeyUpEvent e)
        {
            ChartFastTextBox.Text = "";
            if (StockChartX1.UserEditing)
            {
                //Telerik.WinControls.RadMessageBox.Show("UserEditing!");
                return;
            }
            if (MUserEditing || m_DrawingLineStudy || DrawingLineStudy)
            {
                //if(MUserEditing) Telerik.WinControls.RadMessageBox.Show("MUserEditing!");
                //if (m_DrawingLineStudy) Telerik.WinControls.RadMessageBox.Show("m_DrawingLineStudy!");
                //if (DrawingLineStudy) Telerik.WinControls.RadMessageBox.Show("DrawingLineStudy!");
                return;
            }

            string removed = m_SelectedObject;
            byte[] charbytes = new byte[1];
            //Declare x as a new Point
            Point center = new Point();
            center = new Point(MousePosition.X - Location.X, MousePosition.Y - Location.Y);

            //Telerik.WinControls.RadMessageBox.Show("KeyUp="+e.nChar);



            //Check for alphabetics
            if ((e.nChar >= 65) && (e.nChar <= 90))
            {
                charbytes[0] = Convert.ToByte(e.nChar);
                char c = Encoding.UTF8.GetString(charbytes)[0];

                /*



                ChartFastTextBox.Focus();
                ChartFastTextBox.Text = "";
                ChartFastTextBox.Visible = true;
                System.Windows.Forms.SendKeys.Send(""+c);
                 * */

                /*
                Keys key = (Keys)(byte)c;
                KeyEventArgs eKey = new KeyEventArgs(key);
                //Telerik.WinControls.RadMessageBox.Show("char c=" + c + " key.value="+eKey.KeyValue+" key.data="+eKey.KeyData+" key.code="+eKey.KeyCode);

                ChartFastTextBox.Visible = true;
                ChartFastTextBox.BringToFront();
                ChartFastTextBox.Location = PointToClient(center);
                ChartFastTextBox.Text = Encoding.UTF8.GetString(charbytes);
                ChartFastTextBox.Select(ChartFastTextBox.Text.Length, 0);
                ChartFastTextBox.Focus();
                ChartFastTextBox_KeyUp(sender, eKey);*/


                //Telerik.WinControls.RadMessageBox.Show("Nome do Ativo!");
                /*if (!ChartFastTextBox.Visible)
                {
                    ChartFastTextBox.Focus();*/
                ChartFastTextBox.Visible = true;
                ChartFastTextBox.BringToFront();
                ChartFastTextBox.Location = PointToClient(center);

                ChartFastTextBox.Text += Encoding.UTF8.GetString(charbytes);
                ChartFastTextBox.Select(ChartFastTextBox.Text.Length, 0);
                ChartFastTextBox.Focus();
                //SendKeys.Send(Encoding.UTF8.GetString(charbytes));
                /*}
                else
                {
                    ChartFastTextBox.Text += Encoding.UTF8.GetString(charbytes);
                    ChartFastTextBox.Select(ChartFastTextBox.Text.Length, 0);
                    ChartFastTextBox.Focus();
                }*/
            }
            //Check for numbers
            else if (((e.nChar >= 96) && (e.nChar <= 105)) || ((e.nChar >= 48) && (e.nChar <= 57)))
            {
                //Telerik.WinControls.RadMessageBox.Show("Nome do Ativo!");
                ChartFastTextBox.Visible = true;
                ChartFastTextBox.BringToFront();
                ChartFastTextBox.Location = PointToClient(center);
                ChartFastTextBox.Focus();
                if (e.nChar > 95) ChartFastTextBox.Text = MSymbol + " " + (e.nChar - 96);
                else ChartFastTextBox.Text = MSymbol + " " + (e.nChar - 48);
                ChartFastTextBox.Select(ChartFastTextBox.Text.Length, 0);
            }
            //Check for Delete
            else if ((e.nChar == 46) && (!removed.Contains(".close")))
            {
                //Telerik.WinControls.RadMessageBox.Show("ObectType=" + m_objectType);
                //Indicator?
                if ((m_objectType == (int)ObjectType.otIndicator) || (m_objectType == (int)ObjectType.otLineChart)) StockChartX1_RemoveSeries(removed);
                //Study Line?
                else if ((m_objectType == (int)ObjectType.otLineObject) || (m_objectType == (int)ObjectType.otTrendLineObject)) RemoveStudy();
                else StockChartX1.RemoveObject((ObjectType)m_objectType, m_name);
            }
            //Check for BackSpace
            else if (e.nChar == 8)
            {
                //Telerik.WinControls.RadMessageBox.Show("BackSpace!");
                StockChartX1.ResetZoom();
            }
            //Check for Zoom-In
            else if ((e.nChar == 107) || (e.nChar == 187))
            {
                timerKeyPressed.Enabled = false;
                int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
                //Telerik.WinControls.RadMessageBox.Show("Zoom-In:" + records + "\nVisible:" + StockChartX1.VisibleRecordCount+"\n");
                StockChartX1.ZoomIn(records);
                SaveViewportJDate();
            }
            //Check for Zoom-Out
            else if ((e.nChar == 109) || (e.nChar == 189))
            {
                timerKeyPressed.Enabled = false;
                int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
                //Telerik.WinControls.RadMessageBox.Show("Zoom-Out:"+records+"\nVisible:"+StockChartX1.VisibleRecordCount);
                StockChartX1.ZoomOut(records > 0 ? records : StockChartX1.VisibleRecordCount);
                SaveViewportJDate();
            }
            //Check for Stretch In
            else if (e.nChar == 38)
            {
                timerKeyPressed.Enabled = false;
                paddingTop -= 10;
                paddingBottom -= 10;
                if (paddingTop < -20) paddingTop = -20;
                if (paddingBottom < -20) paddingBottom = -20;
                SetChartPadding(paddingTop, paddingBottom, paddingRight);
            }
            //Check for Stretch Out
            else if (e.nChar == 40)
            {
                timerKeyPressed.Enabled = false;
                paddingTop += 10;
                paddingBottom += 10;
                if (paddingTop > 100) paddingTop = 100;
                if (paddingBottom > 100) paddingBottom = 100;
                SetChartPadding(paddingTop, paddingBottom, paddingRight);
            }
            //Check for Scroll-Right++
            else if (e.nChar == 33)
            {
                timerKeyPressed.Enabled = false;
                //Telerik.WinControls.RadMessageBox.Show("ScrollRight++!");
                StockChartX1.ScrollRight(10);
                _mRecord += 10;
                LoadScroll();
            }
            //Check for Scroll-Left++
            else if (e.nChar == 34)
            {
                timerKeyPressed.Enabled = false;
                //Telerik.WinControls.RadMessageBox.Show("ScrollLeft++!");
                StockChartX1.ScrollLeft(10);
                _mRecord -= 10;
                LoadScroll();
            }
            //Check for Stretch Right
            else if (e.nChar == 39)
            {
                timerKeyPressed.Enabled = false;
                if (paddingRight - 5 > 0)
                {
                    paddingRight -= 5;
                }
                SetChartPadding(paddingTop, paddingBottom, paddingRight);
                //Telerik.WinControls.RadMessageBox.Show("ScrollRight!");
                //StockChartX1.ScrollRight(1);
                //_mRecord += 1;
                //LoadScroll();
            }
            //Check for Scroll-Left
            else if (e.nChar == 37)
            {
                timerKeyPressed.Enabled = false;
                if (paddingRight + 5 < StockChartX1.Width - 10)
                {
                    paddingRight += 5;
                }
                SetChartPadding(paddingTop, paddingBottom, paddingRight);
                //Telerik.WinControls.RadMessageBox.Show("ScrollLeft!");
                //StockChartX1.ScrollLeft(1);
                //_mRecord -= 1;
                //LoadScroll();
            }

        }


        private void StockChartX1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (StockChartX1.UserEditing) return;
            //Telerik.WinControls.RadMessageBox.Show("KeyDown=" + e.KeyCode);
            currentKeyPressed = e.KeyCode;
            //Check for Scroll
            if ((e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.PageUp) || (e.KeyCode == Keys.PageDown) || (e.KeyCode == Keys.Subtract) || (e.KeyCode == Keys.Add) || (e.KeyCode == Keys.Oemplus) || (e.KeyCode == Keys.OemMinus))
            {
                timerKeyPressed.Enabled = true;
                //Telerik.WinControls.RadMessageBox.Show("KeyDownD=" + e.nChar);
            }
            StockChartX1.Focus();

        }

        public void StockChartX1_RemoveSeries(string removed)
        {

            try
            {
                StockChartX1.RemoveSeries(removed);
                if (removed.Contains("MACD"))
                {
                    if (removed.Contains(" Signal")) StockChartX1.RemoveSeries(removed.Replace(" Signal", ""));
                    else StockChartX1.RemoveSeries(removed + " Signal");
                }
                else if (removed.Contains("RSI"))
                {
                    if (removed.Contains(" 30"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" 30", " 70"));
                        StockChartX1.RemoveSeries(removed.Replace(" 30", ""));
                    }
                    else if (removed.Contains(" 70"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" 70", " 30"));
                        StockChartX1.RemoveSeries(removed.Replace(" 70", ""));
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " 30");
                        StockChartX1.RemoveSeries(removed + " 70");
                    }
                }
                else if (removed.Contains("Aroon"))
                {
                    if (removed.Contains(" Down")) StockChartX1.RemoveSeries(removed.Replace(" Down", ""));
                    else StockChartX1.RemoveSeries(removed + " Down");
                }
                else if (removed.Contains("BB"))
                {
                    if (removed.Contains(" Top"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Top", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Top", "") + " Bottom");
                    }
                    else if (removed.Contains(" Bottom"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", "") + " Top");
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " Top");
                        StockChartX1.RemoveSeries(removed + " Bottom");
                    }
                }
                else if (removed.Contains("HILO"))
                {
                    if (removed.Contains(" Top"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Top", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Top", "") + " Bottom");
                    }
                    else if (removed.Contains(" Bottom"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", "") + " Top");
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " Top");
                        StockChartX1.RemoveSeries(removed + " Bottom");
                    }
                }
                else if (removed.Contains("DI+/DI-"))
                {
                    if (removed.Contains(" DI+"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" DI+", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" DI+", "") + " DI-");
                        StockChartX1.RemoveSeries(removed.Replace(" DI+", "") + " ADX");
                    }
                    else if (removed.Contains(" DI-"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" DI-", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" DI-", "") + " DI+");
                        StockChartX1.RemoveSeries(removed.Replace(" DI-", "") + " ADX");
                    }
                    else if (removed.Contains(" ADX"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" ADX", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" ADX", "") + " DI-");
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " DI+");
                        StockChartX1.RemoveSeries(removed + " DI-");
                        StockChartX1.RemoveSeries(removed + " ADX");
                    }
                }
                else if (removed.Contains("FCB"))
                {
                    if (removed.Contains(" High"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" High", ""));
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " High");
                    }
                }
                else if (removed.Contains("HLB"))
                {
                    if (removed.Contains(" Top"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Top", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Top", "") + " Bottom");
                    }
                    else if (removed.Contains(" Bottom"))
                    {
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", ""));
                        StockChartX1.RemoveSeries(removed.Replace(" Bottom", "") + " Top");
                    }
                    else
                    {
                        StockChartX1.RemoveSeries(removed + " Top");
                        StockChartX1.RemoveSeries(removed + " Bottom");
                    }
                }
                else if (removed.Contains("MAE"))
                {
                    if (removed.Contains(" Top")) StockChartX1.RemoveSeries(removed.Replace(" Top", ""));
                    else StockChartX1.RemoveSeries(removed + " Top");
                }
                else if (removed.Contains("SMI"))
                {
                    if (removed.Contains(" %K")) StockChartX1.RemoveSeries(removed.Replace(" %K", ""));
                    else StockChartX1.RemoveSeries(removed + " %K");
                }
                else if (removed.Contains("SO"))
                {
                    if (removed.Contains(" %K")) StockChartX1.RemoveSeries(removed.Replace(" %K", ""));
                    else StockChartX1.RemoveSeries(removed + " %K");
                }
                StockChartX1.Update();
                //_frmSelectIndicator._mock.RemoveIndicatorMock(_frmSelectIndicator._mock.GetIndicatorMockByCodeMock(Regex.Replace(removed, @"\d", ""), removed));

            }
            catch (Exception ex)
            {

            }
        }

        private void StockChartX1_SelectSeries(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_SelectSeriesEvent e)
        {
            if (e.name != "")
            {
                m_SelectedObject = e.name;
                m_name = e.name;
                m_objectType = (int)ObjectType.otIndicator;
            }
        }

        #endregion

        #region Misc
        public string Title
        {
            get
            {
                return MCtlData.GetChartTitle(MSymbol, m_Periodicity, m_BarSize);
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
                    if (MCtlData != null)
                        title = "Save " + Title + " chart?";
                    else
                        title = "Save changes to chart?";

                    DialogResult answer = Telerik.WinControls.RadMessageBox.Show(title, " ", MessageBoxButtons.YesNoCancel, Telerik.WinControls.RadMessageIcon.Question);
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
            string sufix = "";
            switch (m_PriceStyle)
            {
                case "Heikin Ashi":
                    sufix = " HA";
                    break;
                case "Heikin Ashi Smooth":
                    sufix = " HAS(MM" + ((Enums.TypeHeikin)StockChartX1.SmoothHeikinType).ToString()[0] + "," + StockChartX1.SmoothHeikinPeriods + ")";
                    break;
                default:
                    sufix = "";
                    break;
            }
            //StockChartX1.SaveImageTitle = sufix;
            string bmpName = SaveDialog("PNG Images|*.png");
            if (bmpName == "") return;
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
                //Add a border frame:
                int borderSize = 3;

                Bitmap bm = new Bitmap(BMPName);
                Bitmap bm2 = new Bitmap(bm.Width + 2 * borderSize, bm.Height + 2 * borderSize + 20);


                using (Brush border = new SolidBrush(SystemColors.Highlight))
                {
                    Graphics g = Graphics.FromImage(bm2);
                    Point pos = new Point(borderSize, borderSize);
                    g.FillRectangle(border, 0, 0,
                        bm2.Width, bm2.Height);
                    g.DrawImage(bm, pos);
                    g.DrawString("http://www.plena-tp.com.br", new Font("Tahoma", 8), Brushes.White, new PointF(borderSize, bm2.Height - 20));
                    g.DrawString("Copyright © 2012, 2014 - Seamus FS", new Font("Tahoma", 8), Brushes.White, new PointF(bm2.Width - 200, bm2.Height - 20));
                    g.DrawString("PLENA Trading Platform", new Font("Tahoma", 8), Brushes.White, new PointF(bm2.Width / 2 - 50, bm2.Height - 20));
                    g.Dispose();
                }

                bm2.Save(PNGName, ImageFormat.Png);
                bm.Dispose();
                bm2.Dispose();
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

        private bool InitRTChartHeader(bool reset)
        {
            if (MCtlData == null) return false;

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
            panel = 0;
            StockChartX1.AddSeries(MSymbol + ".volume", SeriesType.stVolumeChart, (int)panel);

            //Change volume color and weight of the volume panel:
            StockChartX1.set_SeriesColor(MSymbol + ".volume", ColorTranslator.ToOle(Color.Blue));
            StockChartX1.set_SeriesWeight(MSymbol + ".volume", 3);

            //Resize the volume panel to make it smaller


            //StockChartX1.UpColor = Color.Green; //
            //StockChartX1.DownColor = Color.Red; //


            return true;
        }

        private void InitRtChartFooter(List<M4.DataServer.Interface.BarData> bars, bool isHeikin, int periodsH, Enums.TypeHeikin typeH)
        {
            Stopwatch sw = new Stopwatch();
            double prevJDate = 0;
            sw.Start();

            //Apply Heikin Ashi
            if (isHeikin)
            {
                bars = new List<DataServer.Interface.BarData>(ConvertHeikinAshi(bars, periodsH, typeH));
            }

            // Limit on 3000 candles for tests:
            /*int size = bars.Count > 3000 ? 3000 : bars.Count;
            double[] JDates, ValuesO, ValuesH, ValuesL, ValuesC, ValuesV;
            JDates = new double[size];
            ValuesO = new double[size];
            ValuesH = new double[size];
            ValuesL = new double[size];
            ValuesC = new double[size];
            ValuesV = new double[size];
            int iBar = bars.Count > 3000 ? bars.Count - 3000 : 0;
            for (int i = 0; i < size; i++)
            {
                double jdate = StockChartX1.ToJulianDate(bars[iBar].TradeDate.Year, bars[iBar].TradeDate.Month,
                                                         bars[iBar].TradeDate.Day, bars[iBar].TradeDate.Hour,
                                                         bars[iBar].TradeDate.Minute, bars[iBar].TradeDate.Second);
                JDates[i] = jdate;
                ValuesO[i] = bars[iBar].OpenPrice;
                ValuesH[i] = bars[iBar].HighPrice;
                ValuesL[i] = bars[iBar].LowPrice;
                ValuesC[i] = bars[iBar].ClosePrice;
                ValuesV[i] = bars[iBar].Volume;

                iBar++;

            }*/

            int size = bars.Count;
            //int size = 3;
            double[] JDates, ValuesO, ValuesH, ValuesL, ValuesC, ValuesV;
            JDates = new double[size];
            ValuesO = new double[size];
            ValuesH = new double[size];
            ValuesL = new double[size];
            ValuesC = new double[size];
            ValuesV = new double[size];
            for (int i = 0; i < size; i++)
            {
                double jdate = StockChartX1.ToJulianDate(bars[i].TradeDate.Year, bars[i].TradeDate.Month,
                                                         bars[i].TradeDate.Day, bars[i].TradeDate.Hour,
                                                         bars[i].TradeDate.Minute, bars[i].TradeDate.Second);
                JDates[i] = jdate;
                ValuesO[i] = bars[i].OpenPrice;
                ValuesH[i] = bars[i].HighPrice;
                ValuesL[i] = bars[i].LowPrice;
                ValuesC[i] = bars[i].ClosePrice;
                ValuesV[i] = bars[i].VolumeF;

            }
            string seriesName = MSymbol;
            //if(isHeikin)seriesName+="-HA";

            StockChartX1.AppendRangeValues(seriesName + ".open", JDates, ValuesO, size);
            StockChartX1.AppendRangeValues(seriesName + ".high", JDates, ValuesH, size);
            StockChartX1.AppendRangeValues(seriesName + ".low", JDates, ValuesL, size);
            StockChartX1.AppendRangeValues(seriesName + ".close", JDates, ValuesC, size);
            StockChartX1.AppendRangeValues(seriesName + ".volume", JDates, ValuesV, size);
            /*if (bars[0].VolumeF == 0.0)
                StockChartX1.RemoveSeries(MSymbol + ".volume");*/

            JDates = null;
            ValuesO = null;
            ValuesH = null;
            ValuesL = null;
            ValuesC = null;
            ValuesV = null;
            GC.Collect();
            StockChartX1.RealTimeXLabels = true;
            StockChartX1.ThreeDStyle = false;

            StockChartX1.DisplayTitles = true;

            /*if (reset)
            {
                StockChartX1.RealTimeXLabels = true;
                StockChartX1.ThreeDStyle = false;

                StockChartX1.DisplayTitles = true;

            }
            else
            {
                StockChartX1.RealTimeXLabels = m_frmMain2.MActiveChart.StockChartX1.RealTimeXLabels;
                StockChartX1.ThreeDStyle = m_frmMain2.MActiveChart.StockChartX1.ThreeDStyle;

                StockChartX1.DisplayTitles = m_frmMain2.MActiveChart.StockChartX1.DisplayTitles;
                ChangePriceStyle(_hostedControlText);
            }*/

            /*if (m_SchemeColor != null) Scheme.Instance().UpdateChartColors(StockChartX1, m_SchemeColor);
            else
            {
                Scheme.Instance().UpdateChartColors(StockChartX1, Properties.Settings.Default.SchemeColor);
                m_SchemeColor = Properties.Settings.Default.SchemeColor;
            }*/
            /*
            if (StockChartX1.GetMinValue(StockChartX1.Symbol + ".low") < 1.0)
            {
                m_frmMain2.mnuViewScaleType.IsChecked = false;
                m_frmMain2.mnuViewScaleType.Enabled = false;
            }
            else
                m_frmMain2.mnuViewScaleType.Enabled = true;*/


            if ((m_Periodicity != Periodicity.Daily) && (m_Periodicity != Periodicity.Weekly))
            {
                //ShowNewSessions(bars);
            }

            //StockChartX1.Visible = true;

            //EnableControls(true);

            if (m_EndJDate != NULL_VALUE)
            {
                StockChartX1.VisibleRecordCount = m_QtyJDate + 1;
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate > 0 ? StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate : 1;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);


            }
            else
            {
                StockChartX1.VisibleRecordCount = Properties.Settings.Default.ChartViewport;


            }
            /*else if (frmMain2.GInstance.EndJDateRecord != NULL_VALUE)
            {
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(frmMain2.GInstance.EndJDateRecord) - frmMain2.GInstance.QtyChartRecord;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(frmMain2.GInstance.EndJDateRecord);
                StockChartX1.VisibleRecordCount = frmMain2.GInstance.QtyChartRecord;
            }*/


            Console.WriteLine("InitRtChartFooter() -> " + sw.ElapsedMilliseconds + " " + DateTime.Now.ToString());

        }

        private List<M4.DataServer.Interface.BarData> ConvertHeikinAshi(List<M4.DataServer.Interface.BarData> bars, int periods, Enums.TypeHeikin type)
        {
            //return bars;
            List<M4.DataServer.Interface.BarData> barsHeikin = new List<M4.DataServer.Interface.BarData>();

            for (int i = 0; i < bars.Count; i++)
            {
                float xClose = (bars[i].OpenPrice + bars[i].HighPrice + bars[i].LowPrice + bars[i].ClosePrice) / 4;
                float xOpen = ((i > 0 ? barsHeikin[i - 1].OpenPrice : bars[i].OpenPrice) + (i > 0 ? barsHeikin[i - 1].ClosePrice : bars[i].ClosePrice)) / 2;
                float xHigh = Math.Max(Math.Max(bars[i].HighPrice, xOpen), xClose);
                float xLow = Math.Min(Math.Min(bars[i].LowPrice, xOpen), xClose);
                barsHeikin.Add(new M4.DataServer.Interface.BarData()
                {
                    AdjustD = bars[i].AdjustD,
                    AdjustS = bars[i].AdjustS,
                    BaseType = bars[i].BaseType,
                    ClosePrice = xClose,
                    HighPrice = xHigh,
                    LowPrice = xLow,
                    OpenPrice = xOpen,
                    Symbol = bars[i].Symbol,
                    TradeDate = bars[i].TradeDate,
                    TradeDateTicks = bars[i].TradeDateTicks,
                    VolumeF = bars[i].VolumeF,
                    VolumeS = bars[i].VolumeS,
                    VolumeT = bars[i].VolumeT

                });
            }
            //Smooth?
            if (!(periods == 1 && type == 0))
            {
                switch (type)
                {
                    case Enums.TypeHeikin.Simples:
                        barsHeikin = new List<DataServer.Interface.BarData>(MovingAverages.I().SimpleAverage(barsHeikin, periods));
                        break;
                    case Enums.TypeHeikin.Exponencial:
                        barsHeikin = new List<DataServer.Interface.BarData>(MovingAverages.I().ExponentialAverage(barsHeikin, periods));
                        break;
                    case Enums.TypeHeikin.WellesWilder:
                        barsHeikin = new List<DataServer.Interface.BarData>(MovingAverages.I().WellesWilderSmooth(barsHeikin, periods));
                        break;
                    case Enums.TypeHeikin.Ponderada:
                        barsHeikin = new List<DataServer.Interface.BarData>(MovingAverages.I().WeightedAverage(barsHeikin, periods));
                        break;
                }
            }

            return barsHeikin;

        }

        public bool InitRTChartAsync(Action<bool> afterChartLoaded)
        {
            Console.WriteLine("\n\nStarting InitRTChartAsync()");
            isLoading = true;
            StockChartX1.Freeze(true);
            if (!InitRTChartHeader(false))
                return false;

            Utils.Trace("Start historical request");

            BlockUpdateStock = true;

            switch (_source.ToUpper())
            {
                case "BARCHART":
                    /*MCtlData.GetHistoryAsync(MSymbol, this, m_Periodicity, m_BarSize, m_Bars,
                                              answer =>
                                              {
                                                  Utils.Trace("History received. HasError = " + answer.HasError);
                                                  if (answer.HasError || answer.Data.Count < 3)
                                                  {
                                                      _asyncOp.Post(() => afterChartLoaded(false));
                                                      Answer = answer;
                                                      return;
                                                  }

                                                  Utils.Trace("Add bardata to chart");
                                                  //_asyncOp.Post(() => InitRTChartFooter(answer.Data));
                                                  Action a = () =>
                                                  {
                                                      InitRtChartFooter(answer.Data);
                                                      Utils.Trace("After chart loaded");
                                                      afterChartLoaded(true);
                                                  };
                                                  BeginInvoke(a);
                                              });*/
                    break;
                case "PLENA":
                    MCtlData.GetHistoryLocal(MSymbol, this, m_Periodicity, m_BarSize, m_Bars, _source,
                                          answer =>
                                          {
                                              Utils.Trace("History received. HasError = " + answer.HasError);
                                              if (answer.HasError || answer.Data.Count < 1)
                                              {
                                                  _asyncOp.Post(() => afterChartLoaded(false));
                                                  Answer = answer;
                                                  return;
                                              }

                                              Utils.Trace("Add bardata to chart");
                                              //_asyncOp.Post(() => InitRTChartFooter(answer.Data));
                                              Action a = () =>
                                              {
                                                  if (!m_frmMain2.measureTime.IsRunning) m_frmMain2.measureTime.Start();
                                                  m_frmMain2.timeEllapsed = m_frmMain2.measureTime.ElapsedMilliseconds;
                                                  switch (m_PriceStyle)
                                                  {
                                                      case "Heikin Ashi":
                                                          InitRtChartFooter(answer.Data, true, 1, Enums.TypeHeikin.Simples);
                                                          ChangeStyle(SeriesType.stCandleChart);
                                                          cmdCandleChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdStockLineStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdBarChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiStock.BackColor = Utils.GetDefaultLabelGraphColor();
                                                          cmdHeikinAshiSmooth.BackColor = Utils.GetDefaultBackColor();

                                                          //OLD BEHAVIOR
                                                          //InitRtChartFooter(answer.Data, false, 1, Enums.Type.Simples);
                                                          //StockChartX1.PriceStyle = PriceStyle.psHeikinAshiSmooth;
                                                          break;
                                                      case "Heikin Ashi Smooth":
                                                          InitRtChartFooter(answer.Data, true, StockChartX1.SmoothHeikinPeriods /*Properties.Settings.Default.SettingsHeikinSmoothPeriod*/, (Enums.TypeHeikin)StockChartX1.SmoothHeikinType/*Properties.Settings.Default.SettingsHeikinSmoothType*/);
                                                          ChangeStyle(SeriesType.stCandleChart);
                                                          cmdCandleChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdStockLineStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdBarChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiSmooth.BackColor = Utils.GetDefaultLabelGraphColor();
                                                          break;
                                                      case "Bar Chart":
                                                          InitRtChartFooter(answer.Data, false, 1, Enums.TypeHeikin.Simples);
                                                          ChangeStyle(SeriesType.stStockBarChart);
                                                          if (m_SchemeColor == "SchemeMono") StockChartX1.UpColor = Color.FromArgb(160, 160, 160);
                                                          cmdCandleChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdStockLineStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdBarChartStock.BackColor = Utils.GetDefaultLabelGraphColor();
                                                          cmdHeikinAshiStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiSmooth.BackColor = Utils.GetDefaultBackColor();
                                                          break;
                                                      case "StockLine":
                                                          InitRtChartFooter(answer.Data, false, 1, Enums.TypeHeikin.Simples);
                                                          ChangeStyle(SeriesType.stStockLineChart);
                                                          if (m_SchemeColor == "SchemeMono") StockChartX1.UpColor = Color.FromArgb(160, 160, 160);
                                                          cmdCandleChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdStockLineStock.BackColor = Utils.GetDefaultLabelGraphColor();
                                                          cmdBarChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiSmooth.BackColor = Utils.GetDefaultBackColor();
                                                          break;
                                                      default:
                                                          InitRtChartFooter(answer.Data, false, 1, Enums.TypeHeikin.Simples);
                                                          ChangeStyle(SeriesType.stCandleChart);
                                                          cmdCandleChartStock.BackColor = Utils.GetDefaultLabelGraphColor();
                                                          cmdStockLineStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdBarChartStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiStock.BackColor = Utils.GetDefaultBackColor();
                                                          cmdHeikinAshiSmooth.BackColor = Utils.GetDefaultBackColor();
                                                          break;
                                                  }
                                                  //StockChartX1.Visible = false;
                                                  StockChartX1.Width = 200;
                                                  try
                                                  {
                                                      StockChartX1.Periodicity = (int)m_Periodicity;
                                                      StockChartX1.BarSize = m_BarSize;
                                                      //StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                      //                                 "TempTemplate" + ".sct");
                                                      //Telerik.WinControls.RadMessageBox.Show("Load: StartJDate=" + m_StartJDate + " EndIndex=" + m_EndJDate + " Qty=" + m_QtyJDate);
                                                      SetControls();

                                                      //UpdateMenus();
                                                      afterChartLoaded(true);
                                                      LoadScroll();
                                                      m_StopLoadScroll = false;
                                                      BlockUpdateStock = false;
                                                      //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = false");
                                                      StockChartX1.LoadUserStudyLine(-1);
                                                      //ReconfigureTabs();
                                                      StockChartX1.Freeze(false);
                                                      isLoading = false;
                                                  }
                                                  catch (Exception ex1)
                                                  {
                                                      //Telerik.WinControls.RadMessageBox.Show(ex1.Message);
                                                      if (!File.Exists(PathStudy + StockChartX1.Symbol + ".sty")) File.Create(PathStudy + StockChartX1.Symbol + ".sty");
                                                  }
                                                  Utils.Trace("After chart loaded");
                                                  Console.WriteLine("STOCKCHART() -> " + (m_frmMain2.measureTime.ElapsedMilliseconds - m_frmMain2.timeEllapsed) + " " + DateTime.Now.ToString());
                                              };
                                              BeginInvoke(a);
                                          });
                    break;
            }
            Console.WriteLine("Ending InitRTChartAsync()");
            return true;
        }

        //Realtime updates 		
        public void PriceUpdate(string Symbol, DateTime TradeDate, double LastPrice, long Volume)
        {
            _mTimeStamp = TradeDate;

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
        public void BarUpdate(string Symbol, M4.DataServer.Interface.Periodicity BarType, int BarSize, M4.DataServer.Interface.BarData Bar, bool IsNewBar)
        {
            if (!RealTimeUpdates) return;

            if (MClosing) return; // The chart is being closed

            if (BarSize != m_BarSize && BarType != (M4.DataServer.Interface.Periodicity)m_Periodicity) return;

            if (StockChartX1.UserEditing || MUserEditing)
            {
                BarCache cache = new BarCache { Symbol = Symbol, BarType = (M4Core.Entities.Periodicity)BarType, Bar = Bar, IsNewBar = IsNewBar };
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
                            BarUpdate(cache.Symbol, (M4.DataServer.Interface.Periodicity)cache.BarType, BarSize, cache.Bar, cache.IsNewBar);
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

            //for (int n = 0; n < _expertAdvisors.Count; ++n)
            //{

            //    double j = 0, o = 0, h = 0, l = 0, c = 0;
            //    int v = 0;

            //    // Buy script
            //    Alert oAlert = _expertAdvisors[n].buyAlert;
            //    if (oAlert.GetRecordByIndex(oAlert.RecordCount, ref j, ref o, ref h, ref l, ref c, ref v))
            //    {

            //        if (IsNewBar)
            //        {
            //            // Append new bar
            //            double jDate = StockChartX1.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
            //                                                   Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second);
            //            oAlert.AppendRecord(jDate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.Volume);
            //        }
            //        else
            //        {   // Edit existing bar
            //            oAlert.EditRecord(j, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.Volume);
            //        }
            //    }

            //    // Sell script
            //    oAlert = _expertAdvisors[n].sellAlert;
            //if (oAlert.GetRecordByIndex(oAlert.RecordCount, ref j, ref o, ref h, ref l, ref c, ref v))
            //{

            //    if (IsNewBar)
            //    {
            //        // Append new bar     
            //        double jDate = StockChartX1.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
            //                                               Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second);
            //        oAlert.AppendRecord(jDate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.Volume);
            //    }
            //    else
            //    {   // Edit existing bar
            //        oAlert.EditRecord(j, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.Volume);
            //    }
            //}

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

        public IntPtr GetHandle()
        {
            return IsHandleCreated ? Handle : IntPtr.Zero;
        }

        #endregion

        #region Real Time Charting UI Support
        private void StockChartX1_OnLButtonDown(object sender, EventArgs e)
        {
            frmMain2.GInstance.MActiveChart = this;
            MUserEditing = true;
        }

        private void StockChartX1_OnLButtonUp(object sender, EventArgs e)
        {
            frmMain2.GInstance.MActiveChart = this;
            tmrEdit.Enabled = true;
        }

        private void StockChartX1_OnScroll(object sender, EventArgs e)
        {
            LoadScroll();
        }

        private void StockChartX1_HideDialog(object sender, EventArgs e)
        {
            MUserEditing = false;
            _mDialogShown = false;
        }

        private void StockChartX1_ShowDialog(object sender, EventArgs e)
        {
            MUserEditing = true;
            _mDialogShown = true;
        }

        private void StockChartX1_PaintEvent(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_PaintEvent e)
        {
            StockChartX1.ShowLastTick(StockChartX1.Symbol + ".close", StockChartX1.GetValue(StockChartX1.Symbol + ".close", StockChartX1.LastVisibleRecord));



        }

        #endregion

        #region Advanced Pattern Recogntiion - Patent Pending

        // NOTICE: APR IS PATENT-PENDING.


        private readonly List<string> m_patterns = new List<string>();

        private bool _patternScannerRunning;



        //Modulus Advanced Pattern Recognition library
        //NOTE: A pattern designer utility is included in the Examples\APR\ directory.
        //This library is licensed separately. Refer to www.modulusfe.com/apr/

        //Removes old patterns from the chart
        private void RemovePatterns(string PatternName)
        {
            int n = 0;
            for (n = 0; n <= m_patterns.Count - 1; n++)
            {
                if (m_patterns[n] == PatternName)
                {
                    m_patterns.RemoveAt(n);
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

            m_patterns.Clear();
            _mPatternSeries.Clear();
            _mPatternSeries.Add("Top Pattern");
            _mPatternSeries.Add("Bottom Pattern");
            m_patterns.Add(PatternName);

            StockChartX1.AddSeries("Top Pattern", SeriesType.stLineChart, 0);
            StockChartX1.AddSeries("Bottom Pattern", SeriesType.stLineChart, 0);
            StockChartX1.set_SeriesColor("Top Pattern", ColorTranslator.ToOle(PatternColor));
            StockChartX1.set_SeriesColor("Bottom Pattern", ColorTranslator.ToOle(PatternColor));

            int i;
            double jDate;
            for (i = 1; i <= StockChartX1.RecordCount; i++)
            {
                jDate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", i);
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
            //if (_patternScannerRunning)
            //{
            //    Telerik.WinControls.RadMessageBox.Show(
            //      "Scanning in progress. Wait until done.",
            //      "Already scanning",
            //      MessageBoxButtons.OK,
            //      Telerik.WinControls.RadMessageIcon.Exclamation);
            //    return;
            //}

            //if (StockChartX1.RecordCount > 5000)
            //{
            //    Telerik.WinControls.RadMessageBox.Show("Too many bars loaded, please retry on a chart with fewer than 5,000 bars", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
            //    return;
            //}

            //frmPatternRecognition apr = new frmPatternRecognition();
            //string file = apr.GetPatternDefinitionFile();
            //if (file != "")
            //{
            //    string pattern = file;
            //    short found = (short)file.LastIndexOf(@"\");
            //    pattern = pattern.Substring(found + 1).Replace(".apr.xml", "");
            //    if (!LoadPatternData())
            //    {
            //        Telerik.WinControls.RadMessageBox.Show("The Pattern Recognition plug-in is not installed!", " ", MessageBoxButtons.OK,
            //                        Telerik.WinControls.RadMessageIcon.Error);
            //    }
            //    else
            //    {
            //        ScanForPatterns(file, pattern, Color.Blue);
            //    }
            //}
        }





        #endregion

        #region Chart Trading Support

        private void mnuBuyHere_Click(object sender, CommandEventArgs e)
        {
            if (StockChartX1.PriceStyle != PriceStyle.psStandard)
            {
                Telerik.WinControls.RadMessageBox.Show("Chart trading can be used only with standard HLC or candle charts!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "") txtQuantity.Text = "1";

            if (StockChartX1.CurrentPanel != 0) return;
            LoadPortfolios();
            grpOrder.Left = StockChartX1.GetXPixel(_mRecord - StockChartX1.FirstVisibleRecord) - 50;
            grpOrder.Top = StockChartX1.GetYPixel(0, _mValue) - 20;

            // Don't go offscreen
            if (grpOrder.Left + grpOrder.Width > StockChartX1.Left + StockChartX1.Width)
                grpOrder.Left = StockChartX1.Left + StockChartX1.Width - grpOrder.Width;
            if (grpOrder.Top + grpOrder.Height > StockChartX1.Top + StockChartX1.Height)
                grpOrder.Top = StockChartX1.Top + StockChartX1.Height - grpOrder.Height;

            if (cmbPortfolio.Items.Count > -1) cmbPortfolio.SelectedIndex = 0;
            if (grpOrder.Text == "") grpOrder.Text = "1";
            // m_Side = ctlPortfolio.Orders.Side.LongSide;
            grpOrder.Visible = true;
            txtQuantity.SelectAll();
            txtQuantity.Focus();

        }

        private void mnuSellHere_Click(object sender, CommandEventArgs e)
        {
            if (StockChartX1.PriceStyle != PriceStyle.psStandard)
            {
                Telerik.WinControls.RadMessageBox.Show("Chart trading can be used only with standard HLC or candle charts!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "") txtQuantity.Text = "1";

            if (StockChartX1.CurrentPanel != 0) return;
            LoadPortfolios();
            grpOrder.Left = StockChartX1.GetXPixel(_mRecord - StockChartX1.FirstVisibleRecord) - 50;
            grpOrder.Top = StockChartX1.GetYPixel(0, _mValue) - 20;

            // Don't go offscreen
            if (grpOrder.Left + grpOrder.Width > StockChartX1.Left + StockChartX1.Width)
                grpOrder.Left = StockChartX1.Left + StockChartX1.Width - grpOrder.Width;
            if (grpOrder.Top + grpOrder.Height > StockChartX1.Top + StockChartX1.Height)
                grpOrder.Top = StockChartX1.Top + StockChartX1.Height - grpOrder.Height;

            if (cmbPortfolio.Items.Count > -1) cmbPortfolio.SelectedIndex = 0;
            if (grpOrder.Text == "") grpOrder.Text = "1";
            // m_Side = ctlPortfolio.Orders.Side.ShortSide;
            grpOrder.Visible = true;
            txtQuantity.SelectAll();
            txtQuantity.Focus();

        }

        private void mnuClearOrders_Click(object sender, CommandEventArgs e)
        {
            ClearOrders();
        }

        //Removes chart orders 
        public void ClearOrders()
        {
            //Prompt the user 
            DialogResult result = Telerik.WinControls.RadMessageBox.Show("This action WILL clear all pending chart orders but will NOT close any existing open positions.", " ", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question);

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

        ////Manage chart orders 
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
            int x;
            int y;
            x = StockChartX1.GetXPixel(m_Orders[index].ChartRecord - StockChartX1.FirstVisibleRecord);
            y = StockChartX1.GetYPixel(0, m_Orders[index].EntryPrice);

            //Optional: Only trade if the trend line was crossed from the reverse side using  & e.direction > 0
            //if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.LongSide & e.direction > 0) { 
            //Buy 
            //if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.LongSide)
            //{

            //    m_Orders[index].Executed = true;

            //    //Update the chart and order status 
            //    StockChartX1.AddSymbolObject(0, m_Orders[index].EntryPrice, StockChartX1.RecordCount, SymbolType.soBuySymbolObject, "Buy Order " + (index + 1), "Submitted Buy Order @ " + m_Orders[index].EntryPrice);
            //    StockChartX1.set_ObjectSelectable(ObjectType.otBuySymbolObject, "Buy Order " + (index + 1), false);

            //    m_Orders[index].ChartObjectSymbolName = "Buy Object " + (index + 1);

            //    StockChartX1.RemoveObject(ObjectType.otTextObject, m_Orders[index].ChartObjectTextName);

            //    StockChartX1.AddStaticText(0, "Buy Order (Qty " + m_Orders[index].Quantity + ") Submitted @ " + Math.Round(m_Orders[index].EntryPrice, 4), "Buy Order " + (index + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Green), false, x, y);

            //    //SubmitMarketOrder(StockChartX1.Symbol, ctlPortfolio.Orders.Side.LongSide, m_Orders[index].Quantity);
            //}

            //Sell 
            //// & e.direction < 0 optional 
            //else if (m_Orders[index].OrderSide == ctlPortfolio.Orders.Side.ShortSide)
            //{

            //    m_Orders[index].Executed = true;

            //    //Update the chart and order status 
            //    StockChartX1.AddSymbolObject(0, m_Orders[index].EntryPrice, StockChartX1.RecordCount, SymbolType.soSellSymbolObject, "Sell Order " + (index + 1), "Submitted Sell Order @ " + m_Orders[index].EntryPrice);

            //    StockChartX1.set_ObjectSelectable(ObjectType.otSellSymbolObject, "Sell Order " + (index + 1), false);

            //    m_Orders[index].ChartObjectSymbolName = "Sell Object " + (index + 1);

            //    StockChartX1.AddStaticText(0, "Sell Order (Qty " + m_Orders[index].Quantity + ") Submitted @ " + Math.Round(m_Orders[index].EntryPrice, 4), "Sell Order " + (index + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Red), false, x, y);

            //    StockChartX1.RemoveObject(ObjectType.otTextObject, m_Orders[index].ChartObjectTextName);


            //    //SubmitMarketOrder(StockChartX1.Symbol, ctlPortfolio.Orders.Side.ShortSide, m_Orders[index].Quantity);
            //}

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            //Ensure a portfolio is selected 
            if (string.IsNullOrEmpty(cmbPortfolio.Text))
            {
                Telerik.WinControls.RadMessageBox.Show("Please select a portfolio!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info);
                return;
            }

            grpOrder.Visible = false;

            int quantity;
            int.TryParse(txtQuantity.Text, out quantity);
            if (quantity == 0) return;


            //if (m_Side == ctlPortfolio.Orders.Side.LongSide)
            //{

            //    //Create the chart order 

            //    double price = _mValue;
            //    StockChartX1.DrawTrendLine(0, price, _mRecord, price, StockChartX1.RecordCount, "Buy Order " + (m_Orders.Count + 1));
            //    StockChartX1.AddTrendLineWatch("Buy Order " + (m_Orders.Count + 1), StockChartX1.Symbol + ".close");
            //    //Auto-extend 
            //    StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, "Buy Order " + (m_Orders.Count + 1), (uint)ColorTranslator.ToOle(Color.Green));

            //    int x;
            //    int y;
            //    x = StockChartX1.GetXPixel(_mRecord - StockChartX1.FirstVisibleRecord + 1);
            //    y = StockChartX1.GetYPixel(0, price);
            //    StockChartX1.AddStaticText(0, "Buy Order (Qty " + txtQuantity.Text + ") Waiting @ " + Math.Round(_mValue, 4), "Buy Order " + (m_Orders.Count + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Green), false, x, y);
            //    StockChartX1.set_ObjectWeight(ObjectType.otTrendLineObject, "Buy Order " + (m_Orders.Count + 1), 3);
            //    StockChartX1.set_ObjectSelectable(ObjectType.otTrendLineObject, "Buy Order " + (m_Orders.Count + 1), false);

            //    ChartOrder order = new ChartOrder();
            //    order.EntryPrice = price;
            //    order.OrderSide = ctlPortfolio.Orders.Side.LongSide;
            //    order.ChartObjectLineName = "Buy Order " + (m_Orders.Count + 1);
            //    order.ChartObjectTextName = "Buy Order " + (m_Orders.Count + 1) + " Text";
            //    order.ChartRecord = _mRecord;
            //    order.Quantity = quantity;
            //    m_Orders.Add(order);
            //}

            //else if (m_Side == ctlPortfolio.Orders.Side.ShortSide)
            //{

            //    //Create the chart order 

            //    double price = _mValue;
            //    StockChartX1.DrawTrendLine(0, price, _mRecord, price, StockChartX1.RecordCount, "Sell Order " + (m_Orders.Count + 1));
            //    StockChartX1.AddTrendLineWatch("Sell Order " + (m_Orders.Count + 1), StockChartX1.Symbol + ".close");
            //    //Auto-extend 
            //    StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, "Sell Order " + (m_Orders.Count + 1), (uint)ColorTranslator.ToOle(Color.Red));

            //    int x;
            //    int y;
            //    x = StockChartX1.GetXPixel(_mRecord - StockChartX1.FirstVisibleRecord + 1);
            //    y = StockChartX1.GetYPixel(0, price);
            //    StockChartX1.AddStaticText(0, "Sell Order (Qty " + txtQuantity.Text + ") Waiting @ " + Math.Round(_mValue, 4), "Sell Order " + (m_Orders.Count + 1) + " Text", (uint)ColorTranslator.ToOle(Color.Red), false, x, y);
            //    StockChartX1.set_ObjectWeight(ObjectType.otTrendLineObject, "Sell Order " + (m_Orders.Count + 1), 3);
            //    StockChartX1.set_ObjectSelectable(ObjectType.otTrendLineObject, "Sell Order " + (m_Orders.Count + 1), false);

            //    ChartOrder order = new ChartOrder();
            //    order.EntryPrice = price;
            //    order.OrderSide = ctlPortfolio.Orders.Side.ShortSide;
            //    order.ChartObjectLineName = "Sell Order " + (m_Orders.Count + 1);
            //    order.ChartObjectTextName = "Sell Order " + (m_Orders.Count + 1) + " Text";
            //    order.Quantity = quantity;
            //    order.ChartRecord = _mRecord;

            //    m_Orders.Add(order);
            //}

        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            grpOrder.Visible = false;
        }

        #endregion

        #region Order Execution

        //private void SubmitMarketOrder(string Symbol, ctlPortfolio.Orders.Side BuySell, int Quantity)
        //{

        //    ctlPortfolio.Order MyOrder = new ctlPortfolio.Order();
        //    ctlPortfolio portfolio = m_frmMain.GetPortfolio();
        //    if (portfolio == null) return;

        //    //Gather the order details 
        //    MyOrder.OrderID = portfolio.CreateOrderID();
        //    MyOrder.Side = BuySell;
        //    MyOrder.Quantity = Quantity;
        //    MyOrder.Exchange = "NASDAQ";
        //    //TODO: Update for your order entry API 
        //    MyOrder.Symbol = Symbol;
        //    MyOrder.LimitPrice = 0;
        //    MyOrder._Order = ctlPortfolio.Order.OrderType.Market;

        //    //Ensure a portfolio is selected 
        //    if (portfolio.cmbPortfolio.Text != cmbPortfolio.Text)
        //    {
        //        for (Int16 n = 0; n <= portfolio.cmbPortfolio.Items.Count - 1; n++)
        //        {
        //            if (portfolio.cmbPortfolio.Items[n].Text == cmbPortfolio.Text)
        //            {
        //                portfolio.cmbPortfolio.SelectedIndex = n;
        //                break; // TODO: might not be correct. Was : Exit For 
        //            }
        //        }
        //    }

        //    //#### TODO: WARNING! Example code only! Your order entry API is responsible 
        //    //for sending/receiving orders to update this control. This example just 
        //    //sends the order straight to the DataViewGrid control! Also the exec time 
        //    //and status should be set by the server. 
        //    MyOrder.ExecTime = DateTime.Now;
        //    MyOrder.Status = ctlPortfolio.Orders.Status.Sending;
        //    portfolio.ExecuteOrder(MyOrder.OrderID, MyOrder.Status, MyOrder.Symbol, MyOrder.ExecTime, MyOrder.Side, MyOrder.Quantity,
        //      portfolio.GetLastPrice(MyOrder.Symbol), MyOrder._Order, MyOrder.Expires, MyOrder.LimitPrice
        //    );

        //}


        //Loads the list of available portfolios
        private void LoadPortfolios()
        {

            //List all portfolios in the user's web service entry list
            /*string[] portfolios = null;
            try
            {
                object[] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
                if (_ != null)
                {
                    portfolios = new string[_.Length];
                    for (int i = 0; i < _.Length; i++)
                        portfolios[i] = _[i].ToString();
                }
            }
            catch (Exception)
            {
                //No need to tell the user there are no portfolios
                return;
            }
            if (portfolios == null) return;

            //Add them to combobox
            for (int n = 0; n <= portfolios.Length - 1; n++)
            {
                if (portfolios[n].StartsWith("Portfolio: "))
                {
                    cmbPortfolio.Items.Add(portfolios[n].Replace("Portfolio: ", ""));
                    cmbPortfolio.Items[cmbPortfolio.Items.Count - 1] = "";
                }
            }*/

        }

        #endregion

        #region Expert Advisors

        //public class ExpertAdvisorAlert
        //{
        //    public ExpertAdvisor ea = null;
        //    public Alert buyAlert = null;
        //    public Alert sellAlert = null;
        //    public int prevAlert = 0;
        //    public DateTime lastBuyAlert = DateTime.MinValue;
        //    public DateTime lastSellAlert = DateTime.MinValue;
        //}

        //public List<ExpertAdvisorAlert> _expertAdvisors = new List<ExpertAdvisorAlert>();


        ///// <summary>
        ///// Adds an expert advisor.
        ///// </summary>
        ///// <param name="ea">The expert advisor.</param>
        //public void AddExpertAdvisor(ExpertAdvisor ea)
        //{
        //    RemoveExpertAdvisor(ea);

        //    ExpertAdvisorAlert eaa = new ExpertAdvisorAlert();

        //    eaa.lastBuyAlert = DateTime.MinValue;
        //    eaa.lastSellAlert = DateTime.MinValue;

        //    eaa.buyAlert = new Alert
        //    {
        //        AlertName = ea.Name,
        //        AlertScript = ea.BuyScript,
        //        Symbol = MSymbol,
        //        License = "XRT93NQR79ABTW788XR48"
        //    };
        //    eaa.buyAlert.Alert += OnBuyAlert;
        //    eaa.buyAlert.ScriptError += new _IAlertEvents_ScriptErrorEventHandler(ScriptError);

        //    eaa.sellAlert = new Alert
        //    {
        //        AlertName = ea.Name,
        //        AlertScript = ea.SellScript,
        //        Symbol = MSymbol,
        //        License = "XRT93NQR79ABTW788XR48"
        //    };
        //    eaa.sellAlert.Alert += OnSellAlert;
        //    eaa.sellAlert.ScriptError += new _IAlertEvents_ScriptErrorEventHandler(ScriptError);

        //    // How much data needs to be loaded from StockChartX into TradeScript?      
        //    double max = 0;
        //    try
        //    {
        //        max = Math.Max(Utils.ExtractNumbers(ea.BuyScript).Max(), Utils.ExtractNumbers(ea.SellScript).Max());
        //    }
        //    catch (Exception)
        //    {
        //        max = 20;
        //    }

        //    int startRecord = Math.Max((StockChartX1.RecordCount - (int)(max * 3)) - 50, 0);
        //    //startRecord = 0;

        //    // Prime history
        //    double j = 0, o = 0, h = 0, l = 0, c = 0, v = 0;
        //    //string test = "";
        //    for (int n = startRecord; n < StockChartX1.RecordCount; ++n)
        //    {
        //        j = StockChartX1.GetJDate(MSymbol + ".close", n);
        //        o = StockChartX1.GetValue(MSymbol + ".open", n);
        //        h = StockChartX1.GetValue(MSymbol + ".high", n);
        //        l = StockChartX1.GetValue(MSymbol + ".low", n);
        //        c = StockChartX1.GetValue(MSymbol + ".close", n);
        //        v = StockChartX1.GetValue(MSymbol + ".volume", n);
        //        if (v == -987654321) v = 0;
        //        if (j != -987654321 && o != -987654321)
        //        {
        //            //test += StockChartX1.FromJulianDate(j) + "\t" + o + "\t" + h + "\t" + l + "\t" + c + "\t" + v + "\r\n";
        //            eaa.buyAlert.AppendHistoryRecord(j, o, h, l, c, (int)v);
        //            eaa.sellAlert.AppendHistoryRecord(j, o, h, l, c, (int)v);
        //        }
        //    }
        //    //Clipboard.Clear();
        //    //Clipboard.SetText(test);
        //    // DEBUG: StockChartX data copied to clipboard

        //    /*
        //    string data = "";
        //    int vl = 0;
        //    for (int k = 0; k < eaa.sellAlert.RecordCount; ++k)
        //    {
        //      eaa.sellAlert.GetRecordByIndex(k + 1, ref j, ref o, ref h, ref l, ref c, ref vl);
        //      string date = StockChartX1.FromJulianDate(j);
        //      data += date + "\t" + o + "\t" + h + "\t" + l + "\t" + c + "\t" + v + "\r\n";
        //    }
        //    Clipboard.Clear();
        //    Clipboard.SetText(data);
        //     */
        //    // DEBUG: TradeScript data copied to clipboard


        //    eaa.ea = ea;

        //    _expertAdvisors.Add(eaa);

        //    cmdEAs.Visible = true;
        //    RepositionEAButton();

        //}

        private void RepositionEAButton()
        {
            cmdEAs.Top = 4;
            cmdEAs.Left = Width - StockChartX1.RightDrawingSpacePixels - cmdEAs.Width - 5;

        }

        ///// <summary>
        ///// Removes an expert advisor.
        ///// </summary>
        ///// <param name="ea">The expert advisor.</param>
        //public void RemoveExpertAdvisor(ExpertAdvisor ea)
        //{
        //    ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(ea.Name);
        //    if (eaa == null) return;
        //    _expertAdvisors.Remove(eaa);

        //    cmdEAs.Visible = _expertAdvisors.Count == 0;
        //}


        /// <summary>
        /// This event fires when an expert advisor generates a TradeScript buy alert.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor.</param>
        //private void OnBuyAlert(string symbol, string alertName)
        //{
        //    if (MClosing) return; // The chart is being closed

        //    ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(alertName);
        //    if (eaa == null) return;

        //    /* uncomment if desired
        //    if (eaa.prevAlert != 1)
        //      eaa.prevAlert = 1;
        //    else
        //      return;
        //    */

        //    // Prevent alerts from displaying too frequently
        //    if (DateTime.Now.Subtract(eaa.lastBuyAlert).Seconds < 5 && eaa.lastBuyAlert != DateTime.MinValue) return;
        //    eaa.lastBuyAlert = DateTime.Now;

        //    ShowPopup(eaa.ea.Name, eaa.ea.ParseMessage(ExpertAdvisor.BuyOrSell.Buy));

        //    try
        //    {
        //        UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\ExpertAdvisorBuyAlert.wav", 0, UnmanagedMethods.SND_FILENAME | UnmanagedMethods.SND_ASYNC);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //}


        /// <summary>
        /// This event fires when an expert advisor generates a TradeScript sell alert.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor.</param>
        //private void OnSellAlert(string symbol, string alertName)
        //{
        //    if (MClosing) return; // The chart is being closed

        //    ExpertAdvisorAlert eaa = FindExpertAdvisorAlert(alertName);
        //    if (eaa == null) return;

        //    /* uncomment if desired
        //    if (eaa.prevAlert != 2)
        //      eaa.prevAlert = 2;
        //    else
        //      return;
        //    */

        //    // Prevent alerts from displaying too frequently
        //    if (DateTime.Now.Subtract(eaa.lastSellAlert).Seconds < 5 && eaa.lastSellAlert != DateTime.MinValue) return;
        //    eaa.lastSellAlert = DateTime.Now;

        //    ShowPopup(eaa.ea.Name, eaa.ea.ParseMessage(ExpertAdvisor.BuyOrSell.Sell));

        //    try
        //    {
        //        UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\ExpertAdvisorSellAlert.wav", 0, UnmanagedMethods.SND_FILENAME | UnmanagedMethods.SND_ASYNC);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //}


        /// <summary>
        /// This event fires when an expert advisor generates an error.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="alertName">Name of the expert advisor alert.</param>
        /// <param name="description">The description of the error.</param>
        private void ScriptError(string symbol, string alertName, string description)
        {
            // Virtually guaranteed not to get here since scripts are evaluated before saving
            //MCtlData.OutputWindow1.DisplayAlertOrMessage("'" + alertName +
            //  "' expert advisor generated an error: " + description, OutputWindow.OutputIcon.Warning);
        }


        ///// <summary>
        ///// Finds the expert advisor alert based on an expert advisor.
        ///// </summary>    
        ///// <param name="name">The expert advisor name.</param>
        ///// <returns></returns>
        //public ExpertAdvisorAlert FindExpertAdvisorAlert(string name)
        //{
        //    ExpertAdvisorAlert eaa = _expertAdvisors.Find(
        //      delegate(ExpertAdvisorAlert temp)
        //      {
        //          return temp.ea.Name == name;
        //      }
        //       );
        //    return eaa;
        //}

        //public bool FindExpertAdvisor(string name)
        //{
        //    return FindExpertAdvisorAlert(name) != null;
        //}


        //public void ClearExpertAdvisors()
        //{
        //    _expertAdvisors.Clear();
        //    cmdEAs.Visible = false;
        //}

        //private void cmdEAs_Click(object sender, EventArgs e)
        //{
        //    frmExpertAdvisors eas = new frmExpertAdvisors(frmMain2.GInstance.MExpertAdvisors, this);
        //    eas.ShowDialog(frmMain2.GInstance);
        //}

        ///// <summary>
        ///// Shows the expert advisor popup.
        ///// </summary>
        ///// <param name="popup">The popup.</param>
        //internal void ShowPopup(string name, string message)
        //{

        //    if (MClosing) return; // The chart is being closed

        //    NPopupNotify eaPopup;
        //    eaPopup = new NPopupNotify();

        //    System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        //    string[] names = myAssembly.GetManifestResourceNames();

        //    System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream("M4.Images.ExpertAdvisorAlert.bmp");
        //    Bitmap bmp = new Bitmap(s);
        //    s.Close();

        //    eaPopup.Shape = bmp;
        //    eaPopup.PredefinedStyle = PredefinedPopupStyle.Shaped;
        //    eaPopup.ShapeTransparentColor = Color.Magenta;
        //    eaPopup.Caption.ButtonSize = new NSize(17, 17);
        //    eaPopup.Caption.ButtonsMargins = new NPadding(347, 2, 18, 363);
        //    eaPopup.CaptionBounds = new Rectangle(0, 0, 366, 20);
        //    eaPopup.MoveableBounds = new Rectangle(0, 0, 366, 20);
        //    eaPopup.ContentBounds = new Rectangle(5, 24, 361, 150);

        //    NImageAndTextItem item = eaPopup.Content;
        //    item.Text = "<font face='Arial' size='9' color='Navy'><b>" + name + "</b>    " + MSymbol + "    " + _mTimeStamp + "</font><br/><br/>" + message;

        //    NUIItemImageSet imageSet = eaPopup.CloseButtonImageSet;

        //    imageSet.NormalImage = imageList1.Images[0];
        //    imageSet.HotImage = imageList1.Images[1];
        //    imageSet.PressedImage = imageList1.Images[2];

        //    PopupAnimation animation = PopupAnimation.None;
        //    animation |= PopupAnimation.Fade;
        //    animation |= PopupAnimation.Slide;
        //    eaPopup.AutoHide = false;
        //    eaPopup.VisibleSpan = 120000;
        //    eaPopup.Opacity = 255;
        //    eaPopup.Animation = animation;
        //    eaPopup.AnimationDirection = PopupAnimationDirection.Automatic;
        //    eaPopup.VisibleOnMouseOver = true;
        //    eaPopup.FullOpacityOnMouseOver = true;
        //    eaPopup.AnimationInterval = 10;
        //    eaPopup.AnimationSteps = 20;
        //    eaPopup.Palette.Copy(NUIManager.Palette);
        //    eaPopup.Show();

        //}

        #endregion

        #region Consensus Reports

        //frmExpertAdvisors eas = null;
        bool _consensusReportRunning = false;
        bool _consensusReportRan = false;
        //List<ExpertAdvisor> _consensusEAs = null;
        //ConsensusReport cr = null;

        //public void RunConsensusReport()
        //{
        //    RunConsensusReport(true);
        //}

        ///// <summary>
        ///// Runs the consensus report.
        ///// </summary>
        //public void RunConsensusReport(bool select)
        //{

        //    if (_consensusReportRunning)
        //    {
        //        Telerik.WinControls.RadMessageBox.Show(
        //          "Consensus report in progress. Wait until done.",
        //          "Already running",
        //          MessageBoxButtons.OK,
        //          Telerik.WinControls.RadMessageIcon.Exclamation);
        //        return;
        //    }

        //    cr = new ConsensusReport();

        //    if (select)
        //    {
        //        // Select the expert advisors       
        //        eas = new frmExpertAdvisors(frmMain2.GInstance.MExpertAdvisors);
        //        eas.Client = cr;
        //        _asyncOp.Send(() => eas.ShowDialog(frmMain2.GInstance));
        //    }
        //    else
        //    {
        //        cr._expertAdvisors = _consensusEAs;
        //    }

        //    ProgressWorkerParams workerParams = new ProgressWorkerParams { Alignment = System.Drawing.ContentAlignment.BottomLeft, AllowCancel = true, ControlAnchor = this, Modal = true, };
        //    ProgressWorker.RunSTA(
        //      workerParams,
        //      visualizer =>
        //      {
        //          try
        //          {

        //              bool first = true;
        //              cr.ProgressCallback += (object o, int records, int record, ref bool stop) =>
        //              {
        //                  if (first)
        //                  {
        //                      first = false;
        //                      visualizer.InitProgress(0, records);
        //                  }
        //                  visualizer.ReportProgress(record);
        //                  stop = visualizer.CancelReqested;
        //              };

        //              visualizer.SetProgressTitle("Consensus Report");
        //              visualizer.SetProgressAction("Generating report...");
        //              visualizer.InitProgress(0, cr.ResultCount);

        //              // Run the report. You may optionally change the number of periods for the window size
        //              ConsensusReport.ConsensusReportResults results = cr.RunConsensusReport(this, 10);

        //              // Exit if no report, optional
        //              //if (cr.ResultCount < 1 || visualizer.CancelReqested)

        //              // Output results
        //              _asyncOp.Send(() => rtbConsensus.Rtf = results.Report);
        //              _asyncOp.Send(() => pnlConsensus.Height = Height);
        //              _asyncOp.Send(() => pnlConsensus.Left = radPanel1.Width - pnlConsensus.Width);
        //              _asyncOp.Send(() => StockChartX1.Width = Width - pnlConsensus.Width - 4);
        //              _asyncOp.Send(() => rtbConsensus.Height = Height - rtbConsensus.Top);
        //              _asyncOp.Send(() => pnlConsensus.BackColor = frmMain2.GInstance.BackColor);
        //              _asyncOp.Send(() => rtbConsensus.ReadOnly = true);
        //              _asyncOp.Send(() => pnlConsensus.Visible = true);
        //              _asyncOp.Send(() => pnlTwitter.Visible = false);
        //              _asyncOp.Send(() => StockChartX1.Left = 0);
        //              _asyncOp.Send(() => cmdHide.Visible = true);
        //              _asyncOp.Send(() => cmdRefresh.Visible = true);
        //              _asyncOp.Send(() => guage1.Visible = true);
        //              _asyncOp.Send(() => guage1.Value = (int)(double)(results.Ranking * 100));

        //          }
        //          catch (Exception ex)
        //          {
        //              _asyncOp.Post(
        //                () =>
        //                Telerik.WinControls.RadMessageBox.Show(
        //                  "An error occured while running the consensus report. Error :" + Environment.NewLine + ex.Message,
        //                  "Consensus Report Error",
        //                  MessageBoxButtons.OK,
        //                  Telerik.WinControls.RadMessageIcon.Error));
        //          }
        //          finally
        //          {
        //              _consensusReportRunning = false;
        //          }
        //      },
        //      () =>
        //      { });

        //    _consensusEAs = cr._expertAdvisors;
        //    _consensusReportRan = true;

        //}

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            //if (!_consensusReportRan) return; // Haven't selected expert advisors yet
            //rtbConsensus.Text = "Running...";
            //guage1.Value = 0;
            //RunConsensusReport(false);
        }

        private void cmdHide_Click(object sender, EventArgs e)
        {
            pnlConsensus.Visible = false;
            cmdHide.Visible = false;
            cmdRefresh.Visible = false;
            guage1.Visible = false;
            StockChartX1.Width = Width - 4;
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

            /*else
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

                cmdRefreshTwitter.Enabled = false;
                cmdTweet.Enabled = false;
                webBrowser1.Top = StockChartX1.Top;
                webBrowser1.Left = StockChartX1.Left;
                webBrowser1.Width = StockChartX1.Width;
                webBrowser1.Height = StockChartX1.Height;
                webBrowser1.Visible = true;
                StockChartX1.Visible = false;

            }*/

        }

        private void GetDisplayName()
        {
            // Get the display name
            /*oAuthTwitter oAuth = new oAuthTwitter();
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
            }*/
        }

        private XmlDocument GetUserTimelineXML(string displayName, int count)
        {

            if (string.IsNullOrEmpty(Properties.Settings.Default.oauth_token)) return null; // Not authenticated
            return null;
            /*if (count < 1) count = 1;

            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = Properties.Settings.Default.oauth_token;
            oAuth.TokenSecret = Properties.Settings.Default.oauth_secret;

            string url = "http://twitter.com/statuses/user_timeline.xml?count=" + count.ToString() + "&screen_name=" + displayName;

            string xml = "";
            xml = oAuth.oAuthWebRequest(oAuthTwitter.Method.GET, url, String.Empty);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;*/
        }

        private void cmdHideTwitter_Click(object sender, EventArgs e)
        {
            /*pnlTwitter.Visible = false;
            StockChartX1.Width = Width - 4;

            if (webBrowser1.Visible)
            {
                webBrowser1.Top = StockChartX1.Top;
                webBrowser1.Left = StockChartX1.Left;
                webBrowser1.Width = StockChartX1.Width;
                webBrowser1.Height = StockChartX1.Height;
                pnlTwitterAuthorize.Left = webBrowser1.Width / 2 - (pnlTwitterAuthorize.Width / 2);
            }*/

        }

        private void cmdRefreshTwitter_Click(object sender, EventArgs e)
        {
            //RefreshTwitter();
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

            /*if (txtPin.Text.Trim().Length < 1) return;
            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = _token;

            try
            {
                oAuth.AccessTokenGet(_token, txtPin.Text.Trim());
            }
            catch (Exception)
            {
                Telerik.WinControls.RadMessageBox.Show("Authentication failed", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
            cmdRefreshTwitter.Enabled = true;
            cmdTweet.Enabled = true;*/
        }

        // Refresh the timeline control
        private void RefreshTwitter()
        {

            /*GetDisplayName();

            Application.DoEvents();

            oAuthTwitter oAuth = new oAuthTwitter();
            oAuth.Token = Properties.Settings.Default.oauth_token;
            oAuth.TokenSecret = Properties.Settings.Default.oauth_secret;

            cmdTweet.Enabled = true;
            cmdRefreshTwitter.Visible = true;

            System.Xml.XmlDocument doc;
            try
            {
                doc = GetUserTimelineXML(Properties.Settings.Default.display_name, 20);
            }
            catch (Exception)
            {
                Telerik.WinControls.RadMessageBox.Show("Twitter login failed", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                cmdTweet.Enabled = false;
                cmdRefreshTwitter.Visible = false;
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

            cmdTweet.Enabled = true;*/

        }

        private void txtTweet_TextChanged(object sender, EventArgs e)
        {
            //lblTweetSize.Text = (140 - txtTweet.Text.Length).ToString();
        }

        private void chkTwitter_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.TweetTrades = chkTwitter.Checked ? "1" : "0";
            //frmMain2.GInstance.TweetTrades = chkTwitter.Checked;
        }

        private void cmdTweet_Click(object sender, EventArgs e)
        {
            //if (!frmMain.GInstance.SendTweet(txtTweet.Text))
            //{
            //    Telerik.WinControls.RadMessageBox.Show("Failed to send tweet!", "Twitter Error:", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
            //}
            //RefreshTwitter();
        }

        private void tmrAttention_Tick(object sender, EventArgs e)
        {
            picHighlight.Visible = !picHighlight.Visible;
        }

        #endregion

        private void tmrEdit_Tick(object sender, EventArgs e)
        {
            if (DrawingLineStudy && ((StockChartX1.GetObjectCount((ObjectType)(-1)) != m_lastObjectCount) && !StockChartX1.UserEditing))
            {
                DrawingLineStudy = false;
                if (m_frmMain2._actionChart == frmMain2.ActionChart.TRENDLINE)
                    m_frmMain2._actionChart = frmMain2.ActionChart.NONE;
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
            int cnt = 0;
            foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
            {
                if (doc.ActiveControl == null) continue;
                if (doc.AccessibleName != "CtlPainelChart")
                    continue;

                ((CtlPainelChart)doc.Controls[0]).BackColor = m_frmMain2.BackColor;
                if (((CtlPainelChart)doc.Controls[0]).Visible)
                {
                    cnt++;
                }
            }
            if (cnt > 1)
            {
                BackColor = MSelectionBorderColor;
            }
        }

        //Updates m_frmMain's menus based on this chart
        public void UpdateMenus()
        {
            try
            {

                m_frmMain2.mnuViewShowXGrid.IsChecked = StockChartX1.XGrid;
                m_frmMain2.mnuViewYGrid.IsChecked = StockChartX1.YGrid;
                m_frmMain2.mnuViewSeparators.IsChecked = StockChartX1.HorizontalSeparators;
                m_frmMain2.mnuView3D.IsChecked = StockChartX1.ThreeDStyle;
                m_frmMain2.mnuDarvasBoxes.IsChecked = StockChartX1.DarvasBoxes;
                m_frmMain2.mnuViewScaleType.IsChecked = StockChartX1.ScaleType == ScaleType.stLinearScale;
                ddlLineWidth.SelectedIndex = StockChartX1.LineThickness - 1;
                cmdLineColor.ButtonElement.ButtonFillElement.BackColor = StockChartX1.LineColor;
                if (!popout)
                {
                    //Console.WriteLine("\n\nUpdateMenus():  if !popout: CrossHairs value changed.");
                    StockChartX1.CrossHairs =
                        m_frmMain2.mnuViewCrossHair.IsChecked =
                        m_frmMain2.cmdCrosshair.ToggleState == ToggleState.On;
                    ;
                    StockChartX1.Magnetic =
                        m_frmMain2.mnuViewMagnetic.IsChecked =
                        m_frmMain2.cmdMagnetic.ToggleState == ToggleState.On;
                    ;
                    StockChartX1.DeltaCursor = m_frmMain2.cmdDeltaCursor.ToggleState ==
                                               ToggleState.On;
                }
                else
                {
                    if (ChartToolsPopOut != null)
                    {
                        // Console.WriteLine("\n\nUpdateMenus():  if popout: CrossHairs value changed.");
                        StockChartX1.CrossHairs =
                            m_frmMain2.mnuViewCrossHair.IsChecked =
                            ChartToolsPopOut.cmdCrosshair.ToggleState == ToggleState.On;
                        StockChartX1.Magnetic =
                            m_frmMain2.mnuViewMagnetic.IsChecked =
                            ChartToolsPopOut.cmdMagnetic.ToggleState == ToggleState.On;
                        StockChartX1.DeltaCursor = ChartToolsPopOut.cmdDeltaCursor.ToggleState ==
                                                   ToggleState.On;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CtlPainelChart::UpdateMenus():\n" + ex.Message);
            }
        }

        private void DocumentTabStrip1Resize(object sender, EventArgs e)
        {
            try
            {
                pnlConsensus.Left = radPanel1.Width - pnlConsensus.Width;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void LoadStock()
        {
            ddlStock.Items.Clear();
            _mListSymbols = new List<Stock>();
            foreach (Symbol symbol in frmMain2.GetStockList())
            {
                _mListSymbols.Add(new Stock() { Code = symbol.Code });
            }

            ddlStock.DisplayMember = "Code";
            ddlStock.ValueMember = "Code";
            ddlStock.DataSource = _mListSymbols;


            if (!String.IsNullOrEmpty(MSymbol))
                ddlStock.SelectedValue = MSymbol;

            ddlStock.DropDownListElement.TextBox.TextBoxItem.HideSelection = false;
            ddlStock.DropDownListElement.TextBox.TextBoxItem.SelectAll();
            ddlStock.DropDownListElement.Focus();
            ddlStock.DropDownListElement.EnableMouseWheel = false;
            ddlStock.DropDownListElement.DropDownWidth = 100;

            foreach (var symbol in _mListSymbols)
            {
                source.Add(symbol.Code);
            }
            ChartFastTextBox.AutoCompleteCustomSource = source;
            ChartFastTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void ChangeStyle()
        {
            //StockChartX1.get

            //case "Bar Chart":
            //        ChangeStyle(SeriesType.stStockBarChart);
            //        break;
            //    case "Candle Chart":
            //        ChangeStyle(SeriesType.stCandleChart);
            //        break;
            //    case "StockLine":
            //        ChangeStyle(SeriesType.stStockLineChart);
            //        break;
            //    case "Point && Figure":
            //        StockChartX1.PriceStyle = PriceStyle.psPointAndFigure;
            //        m_frmMain.mnuPriceStyle.Enabled = true;
            //        (new frmPriceStyle()).GetInput(StockChartX1, m_frmMain.cboPriceStyles.HostedControl.Text);
            //        break;
            //    case "Renko":
            //        StockChartX1.PriceStyle = PriceStyle.psRenko;
            //        m_frmMain.mnuPriceStyle.Enabled = true;
            //        (new frmPriceStyle()).GetInput(StockChartX1, m_frmMain.cboPriceStyles.HostedControl.Text);
            //        break;
            //    case "Kagi":
            //        StockChartX1.PriceStyle = PriceStyle.psKagi;
            //        m_frmMain.mnuPriceStyle.Enabled = true;
            //        (new frmPriceStyle()).GetInput(StockChartX1, m_frmMain.cboPriceStyles.HostedControl.Text);
            //        break;
            //    case "Three Line Break":
            //        StockChartX1.PriceStyle = PriceStyle.psThreeLineBreak;
            //        m_frmMain.mnuPriceStyle.Enabled = true;
            //        (new frmPriceStyle()).GetInput(StockChartX1, m_frmMain.cboPriceStyles.HostedControl.Text);
            //        break;
            //    case "EquiVolume":
            //        StockChartX1.PriceStyle = PriceStyle.psEquiVolume;
            //        break;
            //    case "EquiVolume Shadow":
            //        StockChartX1.PriceStyle = PriceStyle.psEquiVolumeShadow;
            //        break;
            //    case "Candle Volume":
            //        StockChartX1.PriceStyle = PriceStyle.psCandleVolume;
            //        break;
            //    case "Heikin Ashi":
            //        StockChartX1.PriceStyle = PriceStyle.psHeikinAshi;
            //        break;
        }

        private void DdlStockSelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (BlockUpdateStock)
                return;


            if (_mouseWheel)
            {
                _mouseWheel = false;
                return;
            }
            if (ddlStock.SelectedItem == null)
                return;

            if (ddlStock.SelectedItem.Value.Equals(MSymbol))
                return;


            SqlConnection _con = DBlocalSQL.Connect();
            if (DBlocalSQL.GetLastBarDataDisk((string)ddlStock.SelectedItem.Value, BaseType.Days, _con).TradeDate == DateTime.MinValue)
            {
                RadMessageBox.Show("O ativo escolhido ainda não possui dados sincronizados.", " ");
                DBlocalSQL.Disconnect(_con);
                //ddlStock.SelectedValue = MSymbol;
                return;
            }
            DBlocalSQL.Disconnect(_con);

            m_StopLoadScroll = true;


            //Console.WriteLine("\nDDLStockIndexChange() MSymbol=" + MSymbol + " NewWSymbol=" + ddlStock.SelectedItem.Value);


            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");

            MSymbol = (string)ddlStock.SelectedItem.Value;

            /*string newSymbol = ddlStock.SelectedItem.Value.ToString();

            var reqOp = new Operations("m" + m_frmMain2._messageRequestID, TypeOperations.LoadCtlPainelChart,
                                       new object[]
                                           {
                                               m_frmMain2.GetCtlActiveWindowFloat(), newSymbol, m_Periodicity,
                                               m_BarSize, m_Bars, "Plena"
                                           });
            CtlPainelChart chart = (CtlPainelChart)(m_frmMain2.GetCtlActiveWindowFloat().Controls[0]);

            var check = (RadCheckBoxElement)chart.cmdCheckPortfolio.HostedItem;

            chart.LoadCtlPainelChart(m_frmMain2._mCtlData,
                                      (string)reqOp.OParams[1],
                                      (M4Core.Entities.Periodicity)reqOp.OParams[2],
                                      (int)reqOp.OParams[3],
                                      (int)reqOp.OParams[4],
                                      (string)reqOp.OParams[5]);
            m_frmMain2.LoadChartSettings(chart);

            if (check != null) chart.cmdCheckPortfolio.HostedItem = check;
            chart.Text = m_frmMain2.MActiveChart.GetChartTitle();

            foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
            {
                if (document == reqOp.OParams[0])
                {
                    document.Text = m_frmMain2.MActiveChart.GetChartTitle();
                }
                if (document.AccessibleName == ((DockWindow)reqOp.OParams[0]).AccessibleDescription)
                {
                    document.Text = m_frmMain2.MActiveChart.GetChartTitle();





                }
            }             
            */

            //LoadChart(MSymbol);

            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    //Telerik.WinControls.RadMessageBox.Show("Template Path = " + ListTemplates._path);
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    ddlStock.DropDownListElement.Focus();

                    return;
                }
                return;
            }));

            if (Parent != null) { Parent.Text = GetChartTitle(); }
            else return;

            if (((DockWindow)Parent).DockState == DockState.Floating)
            {

                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();

                    }
                }
            }/*
            else
            {
                m_frmMain.m_DockManager.DocumentManager.ActiveDocument.Text = GetChartTitle();
            }*/

        }

        private void CmdCandleChartStockClick(object sender, EventArgs e)
        {
            ChangePriceStyle("Candle Chart");
        }

        private void CmdStockLineStockClick(object sender, EventArgs e)
        {
            ChangePriceStyle("StockLine");
        }

        private void CmdBarChartStockClick(object sender, EventArgs e)
        {
            ChangePriceStyle("Bar Chart");
        }

        private void CmdHeikinAshiStockClick(object sender, EventArgs e)
        {
            ChangePriceStyle("Heikin Ashi");
        }

        private void CmdHeikinAshiSmoothStockClick(object sender, EventArgs e)
        {
            ChangePriceStyle("Heikin Ashi Smooth");
        }

        private void cmdZoomOutStock_MouseDown(object sender, EventArgs e)
        {
            int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
            StockChartX1.ZoomOut(records > 0 ? records : StockChartX1.VisibleRecordCount);
            SaveViewportJDate();
        }

        private void cmdZoomInStock_MouseDown(object sender, EventArgs e)
        {
            int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
            StockChartX1.ZoomIn(records);
            SaveViewportJDate();
        }

        private void CmdZoomAreaStockClick(object sender, EventArgs e)
        {
            StockChartX1.ZoomUserDefined();
            SaveViewportJDate();
        }

        private void CmdZoomZeroStockClick(object sender, EventArgs e)
        {
            StockChartX1.Update();
            StockChartX1.ResetZoom();
            SaveViewportJDate();
        }

        private void cmdZoomReset_Click(object sender, EventArgs e)
        {
            StockChartX1.Update();
            StockChartX1.ResetZoom();
            StockChartX1.VisibleRecordCount = 10;
            SaveViewportJDate();
        }

        private void CmdDeleteStockClick(object sender, EventArgs e)
        {
            DeleteDrawings();
        }
        private void CmdPeriodicityDailyClick(object sender, EventArgs e)
        {
            if (waitPeriodicity) return;
            waitPeriodicity = true;
            m_StopLoadScroll = true;
            //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = true");
            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            if (m_Periodicity == Periodicity.Daily)
            {
                m_StopLoadScroll = false;
                //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = false");
                waitPeriodicity = false;
                return;
            }

            StockChartX1.Periodicity = (int)Periodicity.Daily;
            m_Periodicity = Periodicity.Daily;
            m_BarSize = 1;
            StockChartX1.BarSize = m_BarSize;

            SaveViewportJDate();

            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    //BindContextMenuEvents();
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    waitPeriodicity = false;
                    UpdateMenus();
                    return;
                }
                return;
            }));



            Parent.Text = GetChartTitle();
            if (((DockWindow)Parent).DockState == DockState.Floating)
            {
                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();
                    }
                }
            }

            StockChartX1.Focus();
            if (m_EndJDate != NULL_VALUE)
            {
                StockChartX1.LastVisibleRecord = StockChartX1.RecordCount;
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);
            }
            LoadScroll();
        }

        private void CmdPeriodicityWeeklyClick(object sender, EventArgs e)
        {
            if (waitPeriodicity) return;
            waitPeriodicity = true;
            m_frmMain2.measureTime.Reset();
            m_frmMain2.measureTime.Start();
            m_StopLoadScroll = true;
            //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = true");
            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            if (m_Periodicity == Periodicity.Weekly)
            {
                m_StopLoadScroll = false;
                //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = false");
                waitPeriodicity = false;
                return;
            }
            m_Periodicity = Periodicity.Weekly;
            StockChartX1.Periodicity = (int)Periodicity.Weekly;
            m_BarSize = 1;
            StockChartX1.BarSize = m_BarSize;

            SaveViewportJDate();

            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    //BindContextMenuEvents();
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    waitPeriodicity = false;
                    UpdateMenus();
                    StockChartX1.Update();
                    return;
                }
                return;
            }));



            Parent.Text = GetChartTitle();
            if (((DockWindow)Parent).DockState == DockState.Floating)
            {
                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();
                    }
                }
            }

            StockChartX1.Focus();
            if (m_EndJDate != NULL_VALUE)
            {
                StockChartX1.LastVisibleRecord = StockChartX1.RecordCount;
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);
            }
            LoadScroll();
        }

        private void CmdPeriodicityMonthClick(object sender, EventArgs e)
        {
            if (waitPeriodicity) return;
            waitPeriodicity = true;
            m_StopLoadScroll = true;
            //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = true");
            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            if (m_Periodicity == Periodicity.Month)
            {
                m_StopLoadScroll = false;
                //Telerik.WinControls.RadMessageBox.Show("StopLoadScroll = false");
                waitPeriodicity = false;
                return;
            }
            StockChartX1.Periodicity = (int)Periodicity.Month;
            m_Periodicity = Periodicity.Month;
            m_BarSize = 1;
            StockChartX1.BarSize = m_BarSize;

            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    //BindContextMenuEvents();
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    waitPeriodicity = false;
                    UpdateMenus();
                    StockChartX1.Update();
                    return;
                }
                return;
            }));




            Parent.Text = GetChartTitle();
            if (((DockWindow)Parent).DockState == DockState.Floating)
            {
                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();
                    }
                }
            }

            StockChartX1.Focus();
            if (m_EndJDate != NULL_VALUE)
            {
                StockChartX1.LastVisibleRecord = StockChartX1.RecordCount;
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);
            }
            LoadScroll();
        }

        private void cmdPeriodicityCustom_Click(object sender, EventArgs e)
        {
            if (waitPeriodicity) return;
            waitPeriodicity = true;
            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            ChartSelection selection = (new FrmSelectChart { CodeStock = MSymbol }).GetChartSelection();

            if (selection == null)
            {
                waitPeriodicity = false;
                return;
            }

            //Save old parameters:
            Periodicity old_Periodicity = m_Periodicity;
            int old_BarSize = m_BarSize;
            int old_Bars = m_Bars;
            string old_Symbol = MSymbol;
            string old_Source = _source;

            //Set new parameters:
            m_Periodicity = selection.Periodicity;
            StockChartX1.Periodicity = (int)selection.Periodicity;
            m_BarSize = selection.Interval;
            StockChartX1.BarSize = m_BarSize;
            m_Bars = selection.Bars;
            MSymbol = selection.Symbol;
            _source = selection.Source;


            //Try new periodicity:


            if (!InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    //BindContextMenuEvents();
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    waitPeriodicity = false;
                    UpdateMenus();
                    StockChartX1.Update();
                    return;
                }
                return;
            })))
            {
                m_Periodicity = old_Periodicity;
                StockChartX1.Periodicity = (int)old_Periodicity;

                m_BarSize = old_BarSize;

                StockChartX1.BarSize = old_BarSize;

                m_Bars = old_Bars;
                MSymbol = old_Symbol;
                _source = old_Source;
            }


            Parent.Text = GetChartTitle();
            if (((DockWindow)Parent).DockState == DockState.Floating)
            {
                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();
                    }
                }
            }
            StockChartX1.Focus();
            if (m_EndJDate != NULL_VALUE)
            {
                StockChartX1.LastVisibleRecord = StockChartX1.RecordCount;
                StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate;
                StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);
            }
            LoadScroll();

        }

        /*
        private void CmdTechnicalAnalysisClick(object sender, EventArgs e)
        {
            _frmTechnicalAnalysis.StockChartX1 = StockChartX1;

            if (!_frmTechnicalAnalysis.ShowDialog().Equals(DialogResult.OK))
                return;

            StockChartX1 = _frmTechnicalAnalysis.StockChartX1;
            StockChartX1.Update();
        }
        */

        private void CmdTechnicalAnalysisProfessionalClick(object sender, EventArgs e)
        {

            tmrSelectIndicator.Enabled = true;

        }

        #region Templates

        private void AddNodeTemplate(XmlNode inXmlNode, RadTreeNode inTreeNode)
        {
            XmlNode xNode;
            XmlNodeList nodeList;

            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                int i;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];

                    if (_templateText == null)
                        _templateText = (xNode.Name.Equals("TEXT")) ? xNode.InnerText : null;

                    if (_templateParent == null)
                        _templateParent = (xNode.Name.Equals("PARENT")) ? xNode.InnerText : null;

                    if (_templateDefault == null)
                        _templateDefault = ((xNode.NextSibling != null) && (xNode.NextSibling.Name.Equals("DEFAULT"))) ? xNode.NextSibling.InnerText : null;

                    if (_templateDefault == "1") _nameTemplateDefault = xNode.InnerText;

                    if (String.IsNullOrEmpty(_templateText))
                    {
                        if ((xNode.Name.Equals("TEMPLATE")) || (xNode.Name.Equals("SUPORTE")))
                            AddNodeTemplate(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvTemplates.Nodes[0]);

                        continue;
                    }

                    if (_templateText.ToUpper().Equals("TEMPLATES"))
                        _templateText = Program.LanguageDefault.DictionaryTemplate["frmTemplate"];

                    RadTreeNode radTreeNode = new RadTreeNode
                    {
                        Name = _templateText,
                        Text = _templateText,
                        Value = _templateParent,
                        Tag = _templateDefault
                    };

                    if ((_templateDefault != null) && (_templateDefault.Equals("1")))
                        radTreeNode.ImageIndex = 0;
                    else
                        radTreeNode.ImageIndex = -1;

                    if (inTreeNode == null)
                        trvTemplates.Nodes.Add(radTreeNode);
                    else
                        inTreeNode.Nodes.Add(radTreeNode);

                    _templateText = null;
                    _templateParent = null;
                    _templateDefault = null;
                    AddNodeTemplate(xNode, inTreeNode != null ? inTreeNode.Nodes[inTreeNode.Nodes.Count - 1] : trvTemplates.Nodes[0]);
                }
            }
            else
            {
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }

        }

        public void LoadDataTemplate(string nameSelected = null)
        {
            _nameTemplateDefault = "";
            trvTemplates.Nodes.Clear();

            if (!File.Exists(ListTemplates._path + "Plena.sct")) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "Plena.sct");

            if (ListTemplates.Instance().LoadTemplates() == null) return;
            try
            {
                XmlNodeList nodeList = ListTemplates.Instance().LoadTemplates()[0].SelectNodes("SUPORTE");

                int i = 0;
                foreach (XmlNode nivelSuperior in nodeList)
                {
                    AddNodeTemplate(nivelSuperior, trvTemplates.Nodes.Count > 0 ? trvTemplates.Nodes[i] : null);
                    i++;
                }
                if (nameSelected == "-987654321.123456789") ;
                else if (nameSelected != null)
                    trvTemplates.SelectedNode = trvTemplates.Nodes[0].Nodes[trvTemplates.Nodes[0].Nodes.IndexOf(nameSelected)];
                else if (_nameTemplateDefault != "")
                    trvTemplates.Nodes[0].Nodes[_nameTemplateDefault].Selected = true;
                /*else trvTemplates.Nodes[0].Nodes[0].Selected = true;*/

                trvTemplates.ExpandAll();
            }
            catch (Exception) { }

        }

        public void UpdateAllTemplates()
        {
            foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
            {
                if (doc.AccessibleName == "CtlPainelChart")
                {
                    if (doc.Controls.Count == 0) return;
                    ((CtlPainelChart)doc.Controls[0]).LoadDataTemplate(((CtlPainelChart)doc.Controls[0])._currentTemplate);
                    StockChartX1.LoadUserStudyLine(-1);
                }

            }
        }

        private void BtnNewTemplateClick(object sender, EventArgs e)
        {
            //StockChartX1.Update();
            InsertTemplate(null);
        }

        public void InsertTemplate(string textTemplate)
        {
            //StockChartX1.Update();
            AlterTemplate alterTemplate = new AlterTemplate { Insert = true };

            if (!String.IsNullOrEmpty(textTemplate))
            {
                alterTemplate.TextTemplate = textTemplate;
                alterTemplate.SetTextTemplate();
            }

            DialogResult result = alterTemplate.ShowDialog();

            if (!result.Equals(DialogResult.OK))
                return;

            try
            {
                //if (trvTemplates.SelectedNode == null) trvTemplates.SelectedNode = trvTemplates.Nodes[0];
                //trvTemplates.SelectedNode = trvTemplates.Nodes[0];

                Template template = new Template
                {
                    Parent = "Templates",
                    Text = alterTemplate.TextTemplate,
                    Default = alterTemplate.DefaultWorkspace
                };

                ListTemplates.Instance().Insert(template);

                if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + template.Text + ".sct");
                _currentTemplate = alterTemplate.TextTemplate;
                UpdateAllTemplates();
                //LoadDataTemplate(alterTemplate.TextTemplate); 
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionarySelectChart["msgTemplateExists"], " ");
                InsertTemplate(alterTemplate.TextTemplate);
            }
        }

        private void BtnApplyTemplateClick(object sender, EventArgs e)
        {
            //StockChartX1.Update();
            AlterTemplate alterTemplate = new AlterTemplate
            {
                ParentTemplate = trvTemplates.Nodes[0].Text,
                TextTemplate = trvTemplates.SelectedNode.Text,
                Insert = false
            };

            if (trvTemplates.SelectedNode.Tag == null)
                alterTemplate.DefaultWorkspace = null;
            else
                alterTemplate.DefaultWorkspace = (trvTemplates.SelectedNode.Tag.Equals("1")) ? true : false;

            alterTemplate.SetTextTemplate();

            if (alterTemplate.ShowDialog() != DialogResult.OK)
                return;

            foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
            {
                if (doc.AccessibleName == "CtlPainelChart")
                {
                    if (((CtlPainelChart)doc.Controls[0])._currentTemplate == trvTemplates.SelectedNode.Text)
                        ((CtlPainelChart)doc.Controls[0])._currentTemplate = alterTemplate.TextTemplate;
                }
            }
            DeleteTemplate(ListTemplates._path + trvTemplates.SelectedNode.Text + ".sct");
            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + alterTemplate.TextTemplate + ".sct");
            UpdateAllTemplates();

        }

        private void BtnRemoveTemplateClick(object sender, EventArgs e)
        {
            //StockChartX1.Update();
            try
            {
                if (!Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgRemoveNodeTemplate"], " ", MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
                    return;
                if (_nameTemplateDefault == trvTemplates.SelectedNode.Text)
                {
                    foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
                    {
                        if (doc.AccessibleName == "CtlPainelChart")
                        {
                            ((CtlPainelChart)doc.Controls[0])._nameTemplateDefault = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text;
                            Template template = new Template
                                                    {
                                                        Parent = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Parent.Text,
                                                        Text = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text,
                                                        Default = true
                                                    };

                            ListTemplates.Instance().Update(((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text, template, false);
                        }
                    }
                }
                ListTemplates.Instance().Delete(trvTemplates.SelectedNode.Text);
                DeleteTemplate(ListTemplates._path + trvTemplates.SelectedNode.Text + ".sct");
                foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
                {
                    if (doc.AccessibleName == "CtlPainelChart")
                    {
                        /*if (((CtlPainelChart)doc.Client)._currentTemplate == trvTemplates.SelectedNode.Text)*/
                        ((CtlPainelChart)doc.Controls[0])._currentTemplate = null;
                    }

                }
                //trvTemplates.SelectedNode.Remove();
                UpdateAllTemplates();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
            }
        }

        private void TrvTemplatesSelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            /*if(e.Node.Text.ToUpper().Equals("TEMPLATES"))return;
            btnApplyTemplate.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && (!e.Node.Text.ToUpper().Equals("TEMPLATES"));
            btnRemoveTemplate.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && (!e.Node.Text.ToUpper().Equals("TEMPLATES"));

            try
            {
                string templateName = ListTemplates._path + e.Node.Text + ".sct";

                if (File.Exists(templateName))
                {
                    StockChartX1.LoadGeneralTemplate(templateName);
                    _currentTemplate = e.Node.Text;
                    StockChartX1.LoadUserStudyLine(-1);
                }
                else
                {
                    //TODO: Mudar ícone do template para negativo (erro) 
                    //DeleteSeries();
                    Telerik.WinControls.RadMessageBox.Show("Template File not found: "+e.Node.Text); 
                    ListTemplates.Instance().Delete(e.Node.Text);
                    _currentTemplate = null;
                    foreach (var doc in m_frmMain.m_DockManager.DocumentManager.Documents)
                    {
                        if (doc.Client.Name == "CtlPainelChart")
                        {
                            if (((CtlPainelChart)doc.Client)._currentTemplate == e.Node.Text) ((CtlPainelChart)doc.Client)._currentTemplate = null;
                        }

                    }
                    trvTemplates.Nodes[0].Nodes.Remove(e.Node.Text);
                    trvTemplates.Refresh();
                    UpdateAllTemplates();
                    trvTemplates.Refresh();
                    //e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }*/
        }

        private void trvTemplates_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node == null) return;
            if (e.Node.Text.ToUpper().Equals("TEMPLATES") || e.Node.Text.ToUpper().Equals("MODELOS")) return;
            btnApplyTemplate.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && !(e.Node.Text.ToUpper().Equals("TEMPLATES") || e.Node.Text.ToUpper().Equals("MODELOS"));
            btnRemoveTemplate.Enabled = (!e.Node.Text.ToUpper().Equals("PLENA")) && !(e.Node.Text.ToUpper().Equals("TEMPLATES") || e.Node.Text.ToUpper().Equals("MODELOS"));

            try
            {
                string templateName = ListTemplates._path + e.Node.Text + ".sct";

                if (File.Exists(templateName))
                {
                    StockChartX1.LoadGeneralTemplate(templateName);
                    _currentTemplate = e.Node.Text;
                    StockChartX1.LoadUserStudyLine(-1);
                }
                else
                {
                    //TODO: Mudar ícone do template para negativo (erro) 
                    //DeleteSeries();
                    Telerik.WinControls.RadMessageBox.Show("Template File not found: " + e.Node.Text, " ");
                    if (_nameTemplateDefault == e.Node.Text)
                    {
                        foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
                        {
                            if (doc.AccessibleName == "CtlPainelChart")
                            {
                                ((CtlPainelChart)doc.Controls[0])._nameTemplateDefault = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text;
                                Template template = new Template
                                {
                                    Parent = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Parent.Text,
                                    Text = ((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text,
                                    Default = true
                                };

                                ListTemplates.Instance().Update(((CtlPainelChart)doc.Controls[0]).trvTemplates.Nodes[0].Nodes[0].Text, template, false);
                            }
                        }
                    }
                    ListTemplates.Instance().Delete(e.Node.Text);
                    _currentTemplate = null;
                    foreach (DockWindow doc in m_frmMain2.documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
                    {
                        if (doc.AccessibleName == "CtlPainelChart")
                        {
                            /*if (((CtlPainelChart)doc.Client)._currentTemplate == e.Node.Text)*/
                            ((CtlPainelChart)doc.Controls[0])._currentTemplate = null;
                        }

                    }
                    //trvTemplates.Nodes[0].Nodes.Remove(e.Node.Text);
                    //trvTemplates.Refresh();
                    UpdateAllTemplates();
                    //trvTemplates.Refresh();
                    //e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                //Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
        }
        private void CreateIndicators()
        {
            for (int i = 0; i < StockChartX1.SeriesCount; i++)
            {
                if (StockChartX1.GetIndicatorType(StockChartX1.get_SeriesName(i)).ToString().Equals("-1"))
                    continue;

                string name = StockChartX1.get_SeriesName(i);
            }
        }

        private void DeleteTemplate(string templateName)
        {
            if (File.Exists(templateName))
                File.Delete(templateName);
        }

        private void CmnuTemplatesDropDownClosed(object sender, EventArgs e)
        {
            if (((RadContextMenu)sender).DropDown.ClickedItem == null)
                return;

            if (((RadContextMenu)sender).DropDown.ClickedItem.AccessibleName.ToUpper().Equals("RENAME"))
            {
                BtnApplyTemplateClick(sender, e);
                UpdateAllTemplates();
                return;
            }

            if (!((RadContextMenu)sender).DropDown.ClickedItem.AccessibleName.ToUpper().Equals("DEFAULT"))
                return;

            Template template = new Template
            {
                Parent = trvTemplates.SelectedNode.Parent.Text,
                Text = trvTemplates.SelectedNode.Text,
                Default = true
            };
            _nameTemplateDefault = trvTemplates.SelectedNode.Text;
            ListTemplates.Instance().Update(trvTemplates.SelectedNode.Text, template, false);

            LoadDataTemplate();
            UpdateAllTemplates();
        }

        private void CmnuTemplatesDropDownOpening(object sender, CancelEventArgs e)
        {
            if (trvTemplates.SelectedNode.Parent == null)
                e.Cancel = true;

            cmnuTemplates.Items[1].Enabled = (!trvTemplates.SelectedNode.Text.ToUpper().Equals("PLENA")) &&
                                             (!trvTemplates.SelectedNode.Text.ToUpper().Equals("TEMPLATES"));

            cmnuTemplates.Items[0].Enabled = trvTemplates.SelectedNode.ImageIndex != 0;
        }

        #endregion

        private void cmdDeleteStock_Click(object sender, EventArgs e)
        {
            if (!popout)
            {
                if (m_frmMain2.MActiveChart == null) return;
                m_frmMain2.MActiveChart.DeleteDrawings();
            }
            else
            {
                if (ChartToolsPopOut.MActiveChart == null) return;
                ChartToolsPopOut.MActiveChart.DeleteDrawings();
            }
        }

        private void ddlStock_KeyDown(object sender, KeyEventArgs e)
        {
            //Telerik.WinControls.RadMessageBox.Show("key_down");
            //Disable SelectedIndexChanged:
            ddlStock.SelectedIndexChanged -= DdlStockSelectedIndexChanged_Handler;
        }

        private void ddlStock_KeyUp(object sender, KeyEventArgs e)
        {
            //Enable SelectedIndexChanged:
            ddlStock.SelectedIndexChanged += DdlStockSelectedIndexChanged_Handler;
            //Telerik.WinControls.RadMessageBox.Show("key_up");
            if (ddlStock.SelectedItem == null)
                return;

            if (ddlStock.SelectedItem.Value.Equals(MSymbol))
                return;

            SqlConnection _con = DBlocalSQL.Connect();
            if (DBlocalSQL.GetLastBarDataDisk((string)ddlStock.SelectedItem.Value, BaseType.Days, _con).TradeDate == DateTime.MinValue)
            {
                RadMessageBox.Show("O ativo escolhido ainda não possui dados sincronizados.", " ");
                DBlocalSQL.Disconnect(_con);
                //ddlStock.SelectedValue = MSymbol;
                ddlStock.SelectedIndexChanged += DdlStockSelectedIndexChanged_Handler;
                return;
            }
            DBlocalSQL.Disconnect(_con);

            //Console.WriteLine("\nDDLStockKeyUp() MSymbol=" + MSymbol + " NewWSymbol=" + ddlStock.SelectedItem.Value);

            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
            MSymbol = (string)ddlStock.SelectedItem.Value;



            //LoadChart(MSymbol);
            InitRTChartAsync(b => _asyncOp.Post(() =>
            {
                if (b)
                {
                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                    LoadScroll();

                    ddlStock.DropDownListElement.Focus();

                    return;
                }
                return;
            }));
            Parent.Text = GetChartTitle();
            if (((DockWindow)Parent).DockState == DockState.Floating)
            {
                foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                {
                    if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                    {
                        document.Text = GetChartTitle();
                    }
                }
            }/*
            else
            {
                m_frmMain.m_DockManager.DocumentManager.ActiveDocument.Text = GetChartTitle();
            }*/

            //Enable SelectedIndexChanged:
            ddlStock.SelectedIndexChanged += DdlStockSelectedIndexChanged_Handler;
        }

        private void ChartFastTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (errorCharFast && ChartFastTextBox.Text.Length < 2)
            {
                byte[] charbytes = new byte[1];
                charbytes[0] = Convert.ToByte(e.KeyValue);

                char c = Encoding.UTF8.GetString(charbytes)[0];
                SendKeys.Send("" + c);
            }
            errorCharFast = false;
            string[] arg = ChartFastTextBox.Text.Trim().Split(new char[] { ' ' });
            string period, interval_string;
            string symbol = MSymbol;
            int interval = m_BarSize;
            int periodicity = m_Periodicity.GetHashCode();

            int space_count = arg.Length - 1;

            #region Space-Bar
            if (e.KeyCode == Keys.Space)
            {
                //Telerik.WinControls.RadMessageBox.Show("Space!");
                if (space_count > 0) //User can't type more than 1 space-bar (maximum = 2 parameters)
                {
                    ChartFastTextBox.Text = ChartFastTextBox.Text.Remove(ChartFastTextBox.SelectionStart - 1, 1);
                    ChartFastTextBox.Select(ChartFastTextBox.Text.Length, 0);
                }
            }
            #endregion

            #region Esc
            else if (e.KeyCode == Keys.Escape)
            {
                //Telerik.WinControls.RadMessageBox.Show("Esc!");
                if (ChartFastTextBox.Visible) StockChartX1.Focus();
                LoadScroll();
            }
            #endregion

            #region Enter
            else if (e.KeyCode == Keys.Enter)
            {
                bool ignore = true;
                //Telerik.WinControls.RadMessageBox.Show("Load: StartJDate=" + m_StartJDate + " EndIndex=" + m_EndJDate + " Qty=" + m_QtyJDate);
                SaveViewportJDate();
                if (m_EndJDate != NULL_VALUE)
                {
                    StockChartX1.LastVisibleRecord = StockChartX1.RecordCount;
                    StockChartX1.FirstVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate) - m_QtyJDate;
                    StockChartX1.LastVisibleRecord = StockChartX1.GetRecordByJDate(m_EndJDate);
                }
                if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                //Telerik.WinControls.RadMessageBox.Show("Enter!");
                if (ChartFastTextBox.Visible)
                {
                    if (arg.Length > 1)
                    {
                        try
                        {
                            period = arg[1].Trim(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                            period = period.Trim();
                            if (period != "") interval_string = arg[1].Replace(period, "");
                            else interval_string = arg[1];
                            interval_string = interval_string.Trim();
                            symbol = arg[0];
                            if (interval_string != "")
                            {
                                try
                                {
                                    interval = Convert.ToInt32(interval_string);
                                }
                                catch (Exception ex)
                                {
                                    Telerik.WinControls.RadMessageBox.Show("Intervalo inválido!", " ");
                                    StockChartX1.Focus();
                                    LoadScroll();
                                    return;
                                }
                            }
                            else interval = 1;
                            if (period != "")
                            {
                                if (period.Length > 1)
                                {
                                    if ((period[0] == 'M') && (period[1] == 'I')) periodicity = Periodicity.Minutely.GetHashCode();
                                    else if ((period[0] == 'H'))
                                    {
                                        periodicity = Periodicity.Hourly.GetHashCode();
                                    }
                                    else if (period[0] == 'D')
                                    {
                                        periodicity = Periodicity.Daily.GetHashCode();
                                    }
                                    else if ((period[0] == 'W') || (period[0] == 'S'))
                                    {
                                        periodicity = Periodicity.Weekly.GetHashCode();
                                        interval = 1;
                                    }
                                    else if (period[0] == 'M')
                                    {
                                        periodicity = Periodicity.Month.GetHashCode();
                                    }
                                    else if ((period[0] == 'Y') || (period[0] == 'A'))
                                    {
                                        periodicity = Periodicity.Year.GetHashCode();
                                        interval = 1;
                                    }
                                }
                                else if ((period[0] == 'H'))
                                {
                                    periodicity = Periodicity.Hourly.GetHashCode();
                                }
                                else if (period[0] == 'D')
                                {
                                    periodicity = Periodicity.Daily.GetHashCode();
                                }
                                else if ((period[0] == 'W') || (period[0] == 'S'))
                                {
                                    periodicity = Periodicity.Weekly.GetHashCode();
                                    interval = 1;
                                }
                                else if (period[0] == 'M')
                                {
                                    periodicity = Periodicity.Month.GetHashCode();
                                }
                                else if ((period[0] == 'Y') || (period[0] == 'A'))
                                {
                                    periodicity = Periodicity.Year.GetHashCode();
                                    interval = 1;
                                }
                            }
                            //Telerik.WinControls.RadMessageBox.Show("Period = " + period + "\nInterval = " + interval + "\nSource = " + _source);
                            if ((MSymbol == symbol) && (m_Periodicity == (Periodicity)periodicity) && (m_BarSize == interval))
                            {
                                StockChartX1.Focus();
                                LoadScroll();
                                return;
                            }

                            MSymbol = symbol;


                            _source = "Plena";


                            if (periodicity != m_Periodicity.GetHashCode())
                            {
                                m_Periodicity = (Periodicity)periodicity;
                                StockChartX1.Periodicity = periodicity;
                            }

                            if (interval != m_BarSize)
                            {
                                m_BarSize = interval;
                                StockChartX1.BarSize = m_BarSize;
                            }

                        }
                        catch (Exception ex)
                        {
                            Telerik.WinControls.RadMessageBox.Show("Ativo inválido!", " ");
                            StockChartX1.Focus();
                            LoadScroll();
                            return;

                        }
                    }
                    else //Just one argument
                    {
                        // No changes?
                        if (MSymbol == arg[0])
                        {

                            StockChartX1.Focus();


                            LoadScroll();
                            return;

                        }

                        // That's a symbol?
                        if (frmMain2.GetStockList().Count(stock => stock.Code == arg[0]) > 0)
                            symbol = arg[0];
                        // That's a periodicity?
                        else
                        {
                            period = arg[0].Trim(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                            period = period.Trim();
                            if (period != "") interval_string = arg[0].Replace(period, "");
                            else interval_string = arg[0];
                            interval_string = interval_string.Trim();
                            if (interval_string != "")
                            {
                                try
                                {
                                    interval = Convert.ToInt32(interval_string);
                                }
                                catch (Exception ex)
                                {
                                    Telerik.WinControls.RadMessageBox.Show("Intervalo inválido!", " ");
                                    StockChartX1.Focus();
                                    LoadScroll();
                                    return;
                                }
                            }
                            else interval = 1;
                            if (period != "")
                            {
                                if (period.Length > 1)
                                {
                                    if ((period[0] == 'M') && (period[1] == 'I'))
                                        periodicity = Periodicity.Minutely.GetHashCode();

                                    else if ((period[0] == 'H'))
                                    {
                                        periodicity = Periodicity.Hourly.GetHashCode();
                                    }
                                    else if (period[0] == 'D')
                                    {
                                        periodicity = Periodicity.Daily.GetHashCode();
                                    }
                                    else if ((period[0] == 'W') || (period[0] == 'S'))
                                    {
                                        periodicity = Periodicity.Weekly.GetHashCode();
                                        interval = 1;
                                    }
                                    else if (period[0] == 'M')
                                    {
                                        periodicity = Periodicity.Month.GetHashCode();
                                    }
                                    else if ((period[0] == 'Y') || (period[0] == 'A'))
                                    {
                                        periodicity = Periodicity.Year.GetHashCode();
                                        interval = 1;
                                    }
                                }
                                else if ((period[0] == 'H'))
                                {
                                    periodicity = Periodicity.Hourly.GetHashCode();
                                }
                                else if (period[0] == 'D')
                                {
                                    periodicity = Periodicity.Daily.GetHashCode();
                                }
                                else if ((period[0] == 'W') || (period[0] == 'S'))
                                {
                                    periodicity = Periodicity.Weekly.GetHashCode();
                                    interval = 1;
                                }
                                else if (period[0] == 'M')
                                {
                                    periodicity = Periodicity.Month.GetHashCode();
                                }
                                else if ((period[0] == 'Y') || (period[0] == 'A'))
                                {
                                    periodicity = Periodicity.Year.GetHashCode();
                                    interval = 1;
                                }
                            }


                            if (periodicity != (int)m_Periodicity || interval != m_BarSize)
                            {
                                m_Periodicity = (Periodicity)periodicity;
                                StockChartX1.Periodicity = periodicity;
                                m_BarSize = interval;
                                StockChartX1.BarSize = m_BarSize;
                            }
                            else
                            {
                                StockChartX1.Focus();
                                LoadScroll();
                                return;


                            }
                            //Telerik.WinControls.RadMessageBox.Show("Period = " + period + "\nInterval = " + interval + "\nSource = " + _source);
                        }

                    }

                    MSymbol = symbol;
                    if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");




                    m_StopLoadScroll = true;
                    InitRTChartAsync(b => _asyncOp.Post(() =>
                    {
                        if (b)
                        {

                            StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                                   "TempTemplate" + ".sct");

                            UpdateMenus();
                            //chart.BindContextMenuEvents();
                            StockChartX1.Visible = true;
                            StockChartX1.Focus();


                            //chart.StockChartX1.Update();
                            return;
                        }
                        return;
                    }));


                    Parent.Text = GetChartTitle();


                    if (((DockWindow)Parent).DockState == DockState.Floating)
                    {

                        foreach (DockWindow document in m_frmMain2.radDock2.DockWindows)
                        {
                            if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                            {
                                document.Text = GetChartTitle();

                            }
                        }

                    }

                }
            }
            #endregion

            #region New Tab
            else if (e.KeyCode == Keys.Insert)
            {
                ChartSelection selection = new ChartSelection();
                selection.Interval = m_BarSize;
                selection.Bars = m_Bars;
                selection.Periodicity = m_Periodicity;
                selection.Symbol = MSymbol;
                selection.Source = _source;
                selection.PriceStyle = m_PriceStyle;

                if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                //Telerik.WinControls.RadMessageBox.Show("New Tab!");
                if (/*ChartFastTextBox.Visible*/true)
                {
                    if (arg.Length > 1)
                    {
                        try
                        {
                            period = arg[1].Trim(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                            period = period.Trim();
                            if (period != "") interval_string = arg[1].Replace(period, "");
                            else interval_string = arg[1];
                            interval_string = interval_string.Trim();
                            symbol = arg[0];
                            if (interval_string != "")
                            {
                                try
                                {
                                    interval = Convert.ToInt32(interval_string);
                                }
                                catch (Exception ex)
                                {
                                    Telerik.WinControls.RadMessageBox.Show("Intervalo inválido!", " ");
                                    StockChartX1.Focus();
                                    LoadScroll();
                                    return;
                                }
                            }
                            else interval = 1;
                            if (period != "")
                            {
                                if (period.Length > 1)
                                {
                                    if ((period[0] == 'M') && (period[1] == 'I')) periodicity = Periodicity.Minutely.GetHashCode();
                                    else if ((period[0] == 'H'))
                                    {
                                        periodicity = Periodicity.Hourly.GetHashCode();
                                    }
                                    else if (period[0] == 'D')
                                    {
                                        periodicity = Periodicity.Daily.GetHashCode();
                                    }
                                    else if ((period[0] == 'W') || (period[0] == 'S'))
                                    {
                                        periodicity = Periodicity.Weekly.GetHashCode();
                                        interval = 1;
                                    }
                                    else if (period[0] == 'M')
                                    {
                                        periodicity = Periodicity.Month.GetHashCode();
                                    }
                                    else if ((period[0] == 'Y') || (period[0] == 'A'))
                                    {
                                        periodicity = Periodicity.Year.GetHashCode();
                                    }
                                }
                                else if ((period[0] == 'H'))
                                {
                                    periodicity = Periodicity.Hourly.GetHashCode();
                                }
                                else if (period[0] == 'D')
                                {
                                    periodicity = Periodicity.Daily.GetHashCode();
                                }
                                else if ((period[0] == 'W') || (period[0] == 'S'))
                                {
                                    periodicity = Periodicity.Weekly.GetHashCode();
                                    interval = 1;
                                }
                                else if (period[0] == 'M')
                                {
                                    periodicity = Periodicity.Month.GetHashCode();
                                }
                                else if ((period[0] == 'Y') || (period[0] == 'A'))
                                {
                                    periodicity = Periodicity.Year.GetHashCode();
                                    interval = 1;
                                }
                            }
                            selection.Symbol = symbol;
                            if (periodicity != -1) selection.Periodicity = (Periodicity)periodicity;
                            if (interval != -1) selection.Interval = interval;

                            selection.Source = "Plena";

                            selection.PriceStyle = m_PriceStyle;
                            //Telerik.WinControls.RadMessageBox.Show("Period = " + (Periodicity)periodicity + "\nInterval = " + interval + "\nSource = " + _source);
                            /*MessageService.SubmitRequest(new MSRequest("m" + m_frmMain2._messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { selection.Symbol, BaseType.Days.GetHashCode() }));
                            m_frmMain2.AddRequestedOperation(new Operations("m" + m_frmMain2._messageRequestID, TypeOperations.CreateNewCtlPainelChart,
                                                                       new object[] { selection }));*/

                            m_frmMain2.UseLastChartVisual = true;
                            m_frmMain2.QtyLastViewport = m_QtyJDate;
                            m_frmMain2.SchemeLastChart = m_SchemeColor;
                            //Send stockchart settings:
                            m_frmMain2.CreateNewCtlPainel(selection, chart =>
                            {
                                //Chart collor
                                chart.m_SchemeColor = m_SchemeColor;
                                Scheme.Instance().UpdateChartColors(chart.StockChartX1, m_SchemeColor);

                                //Right, Top and Bottom Space
                                chart.SetChartPadding(paddingTop, paddingBottom, paddingRight);

                                //Scale Type
                                chart.StockChartX1.ScaleType = StockChartX1.ScaleType;

                                //Visible Bars
                                chart.StockChartX1.VisibleRecordCount = StockChartX1.VisibleRecordCount;
                                //Scale Precision
                                chart.StockChartX1.ScalePrecision = StockChartX1.ScalePrecision;
                                //Show Horizontal Grid
                                chart.StockChartX1.YGrid = StockChartX1.YGrid;
                                //Show Vertical Grid
                                chart.StockChartX1.XGrid = StockChartX1.XGrid;
                                //Panel Separators
                                chart.StockChartX1.HorizontalSeparators = StockChartX1.HorizontalSeparators;

                                //Price params
                                chart.StockChartX1.SmoothHeikinPeriods = StockChartX1.SmoothHeikinPeriods;
                                chart.StockChartX1.SmoothHeikinType = StockChartX1.SmoothHeikinType;
                                chart.StockChartX1.PriceLineMono = StockChartX1.PriceLineMono;
                                chart.StockChartX1.PriceLineThickness = StockChartX1.PriceLineThickness;
                                chart.StockChartX1.PriceLineThicknessBar = StockChartX1.PriceLineThicknessBar;

                                //Studies params
                                chart.StockChartX1.LineThickness = StockChartX1.LineThickness;
                                chart.ddlLineWidth.SelectedIndex = StockChartX1.LineThickness - 1;
                                chart.StockChartX1.LineColor = StockChartX1.LineColor;
                                cmdLineColor.ButtonElement.ButtonFillElement.BackColor = StockChartX1.LineColor;

                                UpdateMenus();

                                chart.LoadDataTemplate(_currentTemplate);
                                chart.StockChartX1.LoadGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");

                                chart.StockChartX1.Visible = true;



                                chart.StockChartX1.Focus();
                            });

                        }
                        catch (Exception ex)
                        {
                            Telerik.WinControls.RadMessageBox.Show("Ativo inválido!", " ");
                            StockChartX1.Focus();
                            LoadScroll();
                            return;

                        }
                    }
                    else //Just one argument
                    {
                        try
                        {

                            selection.Source = "Plena";

                            selection.Symbol = arg[0];
                        }
                        catch (Exception ex2)
                        {
                            period = arg[0].Trim(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                            period = period.Trim();
                            if (period != "") interval_string = arg[0].Replace(period, "");
                            else interval_string = arg[0];
                            interval_string = interval_string.Trim();
                            if (interval_string != "")
                            {
                                try
                                {
                                    interval = Convert.ToInt32(interval_string);
                                }
                                catch (Exception ex)
                                {
                                    Telerik.WinControls.RadMessageBox.Show("Intervalo inválido!", " ");
                                    StockChartX1.Focus();
                                    LoadScroll();
                                    return;
                                }
                            }
                            else interval = 1;
                            if (period != "")
                            {
                                if (period.Length > 1)
                                {
                                    if ((period[0] == 'M') && (period[1] == 'I'))
                                        periodicity = Periodicity.Minutely.GetHashCode();

                                    else if ((period[0] == 'H'))
                                    {
                                        periodicity = Periodicity.Hourly.GetHashCode();
                                    }
                                    else if (period[0] == 'D')
                                    {
                                        periodicity = Periodicity.Daily.GetHashCode();
                                        interval = 1;
                                    }
                                    else if ((period[0] == 'W') || (period[0] == 'S'))
                                    {
                                        periodicity = Periodicity.Weekly.GetHashCode();
                                        interval = 1;
                                    }
                                    else if (period[0] == 'M')
                                    {
                                        periodicity = Periodicity.Month.GetHashCode();
                                        interval = 1;
                                    }
                                    else if ((period[0] == 'Y') || (period[0] == 'A'))
                                    {
                                        periodicity = Periodicity.Year.GetHashCode();
                                        interval = 1;
                                    }
                                }
                                else if ((period[0] == 'H'))
                                {
                                    periodicity = Periodicity.Hourly.GetHashCode();
                                }
                                else if (period[0] == 'D')
                                {
                                    periodicity = Periodicity.Daily.GetHashCode();
                                    interval = 1;
                                }
                                else if ((period[0] == 'W') || (period[0] == 'S'))
                                {
                                    periodicity = Periodicity.Weekly.GetHashCode();
                                    interval = 1;
                                }
                                else if (period[0] == 'M')
                                {
                                    periodicity = Periodicity.Month.GetHashCode();
                                    interval = 1;
                                }
                                else if ((period[0] == 'Y') || (period[0] == 'A'))
                                {
                                    periodicity = Periodicity.Year.GetHashCode();
                                    interval = 1;
                                }
                            }
                            //Telerik.WinControls.RadMessageBox.Show("Period = " + (Periodicity)periodicity + "\nInterval = " + interval + "\nSource = " + _source);
                        }


                        if (periodicity != -1) selection.Periodicity = (Periodicity)periodicity;
                        if (interval != -1) selection.Interval = interval;

                        /*MessageService.SubmitRequest(new MSRequest("m" + m_frmMain2._messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { selection.Symbol, BaseType.Days.GetHashCode() }));
                         m_frmMain2.AddRequestedOperation(new Operations("m" + m_frmMain2._messageRequestID, TypeOperations.CreateNewCtlPainelChart,
                                                                   new object[] { selection }));*/

                        m_frmMain2.UseLastChartVisual = true;
                        m_frmMain2.QtyLastViewport = m_QtyJDate;
                        m_frmMain2.SchemeLastChart = m_SchemeColor;
                        m_frmMain2.CreateNewCtlPainel(selection, chart =>
                                                {
                                                    //Chart collor
                                                    chart.m_SchemeColor = m_SchemeColor;
                                                    Scheme.Instance().UpdateChartColors(chart.StockChartX1, m_SchemeColor);

                                                    //Right, Top and Bottom Space
                                                    chart.SetChartPadding(paddingTop, paddingBottom, paddingRight);

                                                    //Scale Type
                                                    chart.StockChartX1.ScaleType = StockChartX1.ScaleType;

                                                    //Visible Bars
                                                    chart.StockChartX1.VisibleRecordCount = StockChartX1.VisibleRecordCount;
                                                    //Scale Precision
                                                    chart.StockChartX1.ScalePrecision = StockChartX1.ScalePrecision;
                                                    //Show Horizontal Grid
                                                    chart.StockChartX1.YGrid = StockChartX1.YGrid;
                                                    //Show Vertical Grid
                                                    chart.StockChartX1.XGrid = StockChartX1.XGrid;
                                                    //Panel Separators
                                                    chart.StockChartX1.HorizontalSeparators = StockChartX1.HorizontalSeparators;

                                                    //Price params
                                                    chart.StockChartX1.SmoothHeikinPeriods = StockChartX1.SmoothHeikinPeriods;
                                                    chart.StockChartX1.SmoothHeikinType = StockChartX1.SmoothHeikinType;
                                                    chart.StockChartX1.PriceLineMono = StockChartX1.PriceLineMono;
                                                    chart.StockChartX1.PriceLineThickness = StockChartX1.PriceLineThickness;
                                                    chart.StockChartX1.PriceLineThicknessBar = StockChartX1.PriceLineThicknessBar;

                                                    //Studies params
                                                    chart.StockChartX1.LineThickness = StockChartX1.LineThickness;
                                                    chart.ddlLineWidth.SelectedIndex = StockChartX1.LineThickness - 1;
                                                    chart.StockChartX1.LineColor = StockChartX1.LineColor;
                                                    chart.cmdLineColor.ButtonElement.ButtonFillElement.BackColor = StockChartX1.LineColor;

                                                    UpdateMenus();

                                                    chart.LoadDataTemplate(_currentTemplate);
                                                    chart.StockChartX1.LoadGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                                                    chart.StockChartX1.Visible = true;


                                                    chart.StockChartX1.Focus();
                                                });



                    }

                }
            }
            #endregion

        }

        private void ChartFastTextBox_LostFocus(object sender, EventArgs e)
        {
            ChartFastTextBox.Visible = false;
            errorCharFast = true;
        }

        private void timerKeyScrollPressed_Tick(object sender, EventArgs e)
        {
            switch (currentKeyPressed)
            {
                //Check for Scroll-Right++
                case Keys.PageUp:
                    {
                        //Telerik.WinControls.RadMessageBox.Show("ScrollRight++!");
                        StockChartX1.ScrollRight(10);
                        _mRecord += 10;
                        LoadScroll();
                        break;
                    }
                //Check for Scroll-Left++
                case Keys.PageDown:
                    {
                        //Telerik.WinControls.RadMessageBox.Show("ScrollLeft++!");
                        StockChartX1.ScrollLeft(10);
                        _mRecord -= 10;
                        LoadScroll();
                        break;
                    }
                //Check for Scroll-Right
                case Keys.Right:
                    {
                        //Telerik.WinControls.RadMessageBox.Show("ScrollRight!");
                        //StockChartX1.ScrollRight(1);
                        if (paddingRight - 5 > 0) paddingRight -= 5;
                        SetChartPadding(paddingTop, paddingBottom, paddingRight);
                        //_mRecord += 1;
                        //LoadScroll();


                        break;
                    }
                //Check for Scroll-Left
                case Keys.Left:
                    {
                        //Telerik.WinControls.RadMessageBox.Show("ScrollLeft!");
                        //StockChartX1.ScrollLeft(1);
                        if (paddingRight + 5 < StockChartX1.Width - 10) paddingRight += 5;
                        SetChartPadding(paddingTop, paddingBottom, paddingRight);
                        //_mRecord -= 1;
                        //LoadScroll();


                        break;
                    }
                //Check for Zoom-In

                case Keys.Add:
                case Keys.Oemplus:
                    {
                        int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
                        //Telerik.WinControls.RadMessageBox.Show("Zoom-In:" + records + "\nVisible:" + StockChartX1.VisibleRecordCount+"\n");
                        StockChartX1.ZoomIn(records);
                        SaveViewportJDate();
                        break;
                    }
                //Check for Zoom-Out

                case Keys.Subtract:
                case Keys.OemMinus:
                    {
                        int records = ((StockChartX1.VisibleRecordCount * _percent) / 100) / 2;
                        //Telerik.WinControls.RadMessageBox.Show("Zoom-Out:"+records+"\nVisible:"+StockChartX1.VisibleRecordCount);
                        StockChartX1.ZoomOut(records > 0 ? records : StockChartX1.VisibleRecordCount);
                        SaveViewportJDate();
                        break;
                    }
                //Check for Stretch-In:
                case Keys.Up:
                    {
                        paddingTop -= 10;
                        paddingBottom -= 10;
                        if (paddingTop < -20) paddingTop = -20;
                        if (paddingBottom < -20) paddingBottom = -20;
                        SetChartPadding(paddingTop, paddingBottom, paddingRight);
                        break;
                    }
                //Check for Stretch-Out:
                case Keys.Down:
                    {
                        timerKeyPressed.Enabled = false;
                        paddingTop += 10;
                        paddingBottom += 10;
                        if (paddingTop > 100) paddingTop = 100;
                        if (paddingBottom > 100) paddingBottom = 100;
                        SetChartPadding(paddingTop, paddingBottom, paddingRight);
                        break;
                    }
                default:
                    timerKeyPressed.Enabled = false;
                    break;
            }
        }
        public void DrawCrossHairs()
        {
            StockChartX1.Invalidate();
            Point e = StockChartX1.PointToClient(MousePosition);
            /*if ((lastMousePositionStart != new Point(-1, -1)) && (lastMousePositionEnd != new Point(-1, -1)))
            {
                ControlPaint.DrawReversibleLine(lastMousePositionStart, lastMousePositionEnd, Color.Black);
            } */
            //Draw X-Line
            crossHairGraphics.DrawLine(crossHairPen, m_frmMain2.PointToClient(new Point(0, e.Y + (LabelMouseY.Height) + 2)), m_frmMain2.PointToClient(new Point(StockChartX1.Width, e.Y + (LabelMouseY.Height) + 2)));
            //ControlPaint.DrawReversibleLine(m_frmMain.PointToClient(new Point(0, e.Y + (LabelMouseY.Height) + 2)), m_frmMain.PointToClient(new Point(StockChartX1.Width, e.Y + (LabelMouseY.Height) + 2)), Color.Black);
            //Draw Y-Line
            crossHairGraphics.DrawLine(crossHairPen, m_frmMain2.PointToClient(new Point(e.X, 0)), m_frmMain2.PointToClient(new Point(e.X, StockChartX1.Height)));
            CrossHairState = crossHairGraphics.Save();
            //crossHairPen.Dispose();
            //crossHairGraphics.Dispose();

            //lastMousePositionStart = m_frmMain.PointToClient(new Point(0, e.Y + (LabelMouseY.Height) + 2));
            //lastMousePositionEnd = m_frmMain.PointToClient(new Point(StockChartX1.Width, e.Y + (LabelMouseY.Height) + 2));
        }

        private void radCommandBar1_MouseLeave(object sender, EventArgs e)
        {/*
            Telerik.WinControls.RadMessageBox.Show("mouse leave");
            radCommandBar1.Visible = false;*/
        }

        private void StockChartX1_ItemLeftClick(object sender, AxSTOCKCHARTXLib._DStockChartXEvents_ItemLeftClickEvent e)
        {
            m_SelectedObject = e.name;
            m_name = e.name;
            m_objectType = (int)e.objectType;
        }

        private void StockChartX1_SizeChanged(object sender, EventArgs e)
        {
            LabelMouseX.Visible = false;
            LabelMouseY.Visible = false;
        }

        private void timerMenuAutoHideIn_Tick(object sender, EventArgs e)
        {
            /*Point stockMnuP = StockChartX1.PointToScreen(radCommandBar1.Location);
            if (MousePosition.X > stockMnuP.X && MousePosition.X < (stockMnuP.X + radCommandBar1.Width) && MousePosition.Y > stockMnuP.Y && MousePosition.Y < (stockMnuP.Y + radCommandBar1.Height))
            {
                radCommandBar1.Visible = true;
            }
            else radCommandBar1.Visible = false;
            timerMnuAutoHideIn.Enabled = false;*/

        }

        private void StockChartX1_UserDrawingComplete(object sender, _DStockChartXEvents_UserDrawingCompleteEvent e)
        {
            //Console.WriteLine("\n\n\nStockChartX1_UserDrawingComplete()\n\n");
            DrawingLineStudy = false;
            m_frmMain2._actionChart = frmMain2.ActionChart.NONE;
        }

        private void cmdZoomReset_Click_1(object sender, EventArgs e)
        {
            StockChartX1.VisibleRecordCount = Properties.Settings.Default.ChartViewport;
        }

        public void SetChartPadding(int paddingTop, int paddingBottom, int paddingRight)
        {
            //Telerik.WinControls.RadMessageBox.Show("SetChartPad!");
            try
            {
                StockChartX1.RightDrawingSpacePixels = paddingRight;
                StockChartX1.ResetYScale(0);


                double sMaxValue, sMinValue, max, min = StockChartX1.GetVisibleMaxValue(StockChartX1.Symbol + ".high");

                max = sMaxValue = StockChartX1.GetVisibleMaxValue(StockChartX1.Symbol + ".high");
                min = sMinValue = StockChartX1.GetVisibleMinValue(StockChartX1.Symbol + ".low");


                //if (StockChartX1.ScaleType != ScaleType.stSemiLogScale)
                //{

                max =
                    Convert.ToDouble(((double)paddingTop / 100) * (sMaxValue - sMinValue) + sMaxValue);

                min =
                    Convert.ToDouble(sMinValue - ((double)paddingBottom / 100) * (sMaxValue - sMinValue));
                //}

                StockChartX1.SetYScale(0, max, min);
            }
            catch (Exception ex)
            {
                //Telerik.WinControls.RadMessageBox.Show("SetChartPadding() " + ex.Message, " "); //This error on stockchart invoke error on messagebox!
            }
            finally
            {
                if (!StockChartX1.Visible) StockChartX1.Visible = true;
            }

        }

        private void trvTemplates_NodeMouseHover(object sender, RadTreeViewEventArgs e)
        {
            StockChartX1.Update();

        }

        public void SaveViewportJDate()
        {
            //Telerik.WinControls.RadMessageBox.Show("Saved: StartJDate=" + m_StartJDate + " EndJDate=" + m_EndJDate);
            m_StartJDate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", StockChartX1.FirstVisibleRecord);
            m_EndJDate = StockChartX1.GetJDate(StockChartX1.Symbol + ".close", StockChartX1.LastVisibleRecord);
            m_QtyJDate = StockChartX1.LastVisibleRecord - StockChartX1.FirstVisibleRecord;

        }

        private void tmrSelectIndicator_Tick(object sender, EventArgs e)
        {
            tmrSelectIndicator.Enabled = false;
            _frmSelectIndicator.ParentForm = this;
            _frmSelectIndicator.StockChartX1 = StockChartX1;
            _frmSelectIndicator.SelectIndicator = m_name;
            _frmSelectIndicator.ShowDialog();

        }

        private void CtlPainelChart_Load(object sender, EventArgs e)
        {
            twSumary.BackColor = Utils.GetDefaultBackColor();
            twTemplates.BackColor = Utils.GetDefaultBackColor();
            trvTemplates.TreeViewElement.BackColor = Utils.GetDefaultBackColor();
        }

        private void tmrLabels_Tick(object sender, EventArgs e)
        {
            if (!(MousePosition.Y > StockChartX1.PointToScreen(StockChartX1.Location).Y && MousePosition.Y < StockChartX1.PointToScreen(StockChartX1.Location).Y + StockChartX1.Height) ||
            !(MousePosition.X > StockChartX1.PointToScreen(StockChartX1.Location).X && MousePosition.X < StockChartX1.PointToScreen(StockChartX1.Location).X + StockChartX1.Width))
            {
                LabelMouseY.Visible = false;
                LabelMouseX.Visible = false;
                //if (scrollChart.Visible) scrollChart.Visible = false;
                tmrLabels.Enabled = false;
            }
            else
            {
                //tmrLabels.Enabled = false;
            }
        }

        private void CtlPainelChart_Enter(object sender, EventArgs e)
        {
            //Console.WriteLine("\n\nCtlPainelChart_Enter(): UpdateMenus()\n");
            UpdateMenus();
        }

        void DBDailyShared_SnapshotEvent(object sender, SnapshotEventArgs args)
        {
            BaseType m_BaseType = BaseType.Days;
            if (m_Periodicity == Periodicity.Minutely)
            {
                if (m_BarSize % 15 != 0) m_BaseType = BaseType.Minutes;
                else m_BaseType = BaseType.Minutes15;
            }
            else if (m_Periodicity == Periodicity.Hourly) m_BaseType = BaseType.Minutes15;
            if (args.Snapshot.Symbol == MSymbol && args.Base == (int)m_BaseType)
            {
                //StockChartX1.AppendValueAsTick(MSymbol + ".close", StockChartX1.ToJulianDate(args.Snapshot.Timestamp.Year, args.Snapshot.Timestamp.Month, args.Snapshot.Timestamp.Day, args.Snapshot.Timestamp.Hour, args.Snapshot.Timestamp.Minute, args.Snapshot.Timestamp.Second), args.Snapshot.Close);
                //Update last bar:
                StockChartX1.EditValueByRecord(MSymbol + ".close", StockChartX1.RecordCount, args.Snapshot.Close);
                StockChartX1.EditValueByRecord(MSymbol + ".high", StockChartX1.RecordCount, args.Snapshot.High);
                StockChartX1.EditValueByRecord(MSymbol + ".open", StockChartX1.RecordCount, args.Snapshot.Open);
                StockChartX1.EditValueByRecord(MSymbol + ".low", StockChartX1.RecordCount, args.Snapshot.Low);
                StockChartX1.EditValueByRecord(MSymbol + ".volume", StockChartX1.RecordCount, args.Snapshot.VolumeFinancial);
                if (InvokeRequired) Invoke(new MethodInvoker(() => { StockChartX1.Update(); }));


            }
            else
            {
                //Console.WriteLine("Snapshot ignored on CtlPanel " + MSymbol + " " + m_Periodicity);
                return;


            }
        }

        void DBDailyShared_NewBarEvent(object sender, DataServer.Interface.BarEventArgs args)
        {
            BaseType m_BaseType = BaseType.Days;
            if (m_Periodicity == Periodicity.Minutely)
            {
                if (m_BarSize % 15 != 0) m_BaseType = BaseType.Minutes;
                else m_BaseType = BaseType.Minutes15;
            }
            else if (m_Periodicity == Periodicity.Hourly) m_BaseType = BaseType.Minutes15;
            if (args.Bar.Symbol == MSymbol && args.Base == (int)m_BaseType)
            {
                //Update last bar:
                StockChartX1.AppendValueAsTick(MSymbol + ".close", StockChartX1.ToJulianDate(args.Bar.TradeDate.Year, args.Bar.TradeDate.Month, args.Bar.TradeDate.Day, args.Bar.TradeDate.Hour, args.Bar.TradeDate.Minute, args.Bar.TradeDate.Second), args.Bar.ClosePrice);
                StockChartX1.AppendValueAsTick(MSymbol + ".volume", StockChartX1.ToJulianDate(args.Bar.TradeDate.Year, args.Bar.TradeDate.Month, args.Bar.TradeDate.Day, args.Bar.TradeDate.Hour, args.Bar.TradeDate.Minute, args.Bar.TradeDate.Second), args.Bar.VolumeF);
                if (InvokeRequired) Invoke(new MethodInvoker(() => { StockChartX1.Update(); }));


            }
            else
            {
                Console.WriteLine("New bar ignored on CtlPanel " + MSymbol + " " + m_Periodicity);
                return;


            }
        }

        public void DBDailyShared_UpdateBaseEvent(object sender, BarDataEventArgs e)
        {
            //Re-load chart!
            BaseType baseType = m_Periodicity >= Periodicity.Daily
                                    ? BaseType.Days
                                    : m_Periodicity == Periodicity.Minutely && m_BarSize % 15 == 0
                                          ? BaseType.Minutes15
                                          : BaseType.Minutes;
            if (e.Bar.Symbol == MSymbol && e.Bar.BaseType == (int)baseType)
            {
                try
                {
                    Action action = () =>
                                        {
                                            Console.WriteLine("CtlPainelChart:OnUpdateEvent()");
                                            if (!isLoading) StockChartX1.SaveGeneralTemplate(ListTemplates._path +
                                                                             "TempTemplate" + ".sct");
                                            InitRTChartAsync(b => _asyncOp.Post(() =>
                                            {
                                                if (b)
                                                {
                                                    //chart.BindContextMenuEvents();
                                                    StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                                                   "TempTemplate" + ".sct");

                                                    return;
                                                }
                                                return;
                                            }));

                                        };
                    BeginInvoke(action);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CtlPainelChart:OnUpdateEvent(): " + ex.Message);
                }
            }
        }

        public void ReconfigureTabs()
        {
            var parent = twSumary.Parent;

            if (parent.Controls.Count < 2)
                return;

            ToolWindow tab0 = (ToolWindow)parent.Controls[0];
            ToolWindow tab1 = (ToolWindow)parent.Controls[1];

            if (Properties.Settings.Default.TabDataPosition.Equals("Less"))
            {
                radDock1.DockWindow(tab0, DockPosition.Left);
                radDock1.DockWindow(tab1, DockPosition.Left);
            }
            else if (Properties.Settings.Default.TabDataPosition.Equals("Bottom"))
            {
                radDock1.DockWindow(tab0, DockPosition.Bottom);
                radDock1.DockWindow(tab1, DockPosition.Bottom);
            }

            tab0.DockTo(tab1, DockPosition.Fill);

            tab0.AutoHide();


            //if (Properties.Settings.Default.TabDataPosition.Equals("Less"))
            //{
            //    radDock1.DockWindow(twSumary, DockPosition.Left);
            //    radDock1.DockWindow(twTemplates, DockPosition.Left);
            //}
            //else if (Properties.Settings.Default.TabDataPosition.Equals("Bottom"))
            //{
            //    radDock1.DockWindow(twSumary, DockPosition.Bottom);
            //    radDock1.DockWindow(twTemplates, DockPosition.Bottom);
            //}

            //twTemplates.DockTo(twSumary, DockPosition.Fill);

            //twTemplates.AutoHide();
        }

        private void cmdLineColor_Click(object sender, EventArgs e)
        {
            RadColorDialogForm colorBox = new RadColorDialogForm();
            m_frmMain2.configStudies = ListConfigStudies.Instance().LoadListConfigStudies();
            colorBox.SelectedColor = m_frmMain2.configStudies.Color;
            colorBox.ActiveMode = ColorPickerActiveMode.Basic;
            if (colorBox.ShowDialog() == DialogResult.OK)
            {
                //m_frmMain2.configStudies.Color = colorBox.SelectedColor;
                //ListConfigStudies.Instance().Update(m_frmMain2.configStudies);
                StockChartX1.LineColor = colorBox.SelectedColor;
                cmdLineColor.ButtonElement.ButtonFillElement.BackColor = colorBox.SelectedColor;
            }

        }

        public void cmdFacebook_Click(object sender, EventArgs e)
        {
            //RadMessageBox.Show("FACEBOOK!");


            if (!frmMain2.facebookLogged)
            {
                if (!frmMain2.ConnectFacebook()) return;
            }

            string bmpName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TEMP\\" + GenerateRandomName() + ".png";//SaveDialog("PNG Images|*.png");
            if (bmpName == "") return;
            bmpName = bmpName.Substring(0, bmpName.Length - 4) + ".bmp";
            try
            {
                StockChartX1.SaveChartBitmap(bmpName);
                string pngName = bmpName.Substring(0, bmpName.Length - 4) + ".png";
                ConvertBMP(bmpName, pngName);

                MemoryStream ms = new MemoryStream();

                // Save the screenshot to the specified path that the user has chosen
                Bitmap imageBmp = new Bitmap(pngName);
                imageBmp.Save(ms, ImageFormat.Png);
                frmMain2.PublishFacebook(pngName);
            }
            catch (Exception ex) { }
        }
        private void ddlLineWidth_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            /*m_frmMain2.configStudies = ListConfigStudies.Instance().LoadListConfigStudies();
            m_frmMain2.configStudies.LineThickness = (decimal)ddlLineWidth.SelectedIndex+1;
            ListConfigStudies.Instance().Update(m_frmMain2.configStudies); */
            StockChartX1.LineThickness = ddlLineWidth.SelectedIndex + 1;

        }
        public string GenerateRandomName()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            char[] arr = GuidString.ToCharArray();

            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c))));
            GuidString = new string(arr);
            return GuidString;
        }

    }
}
