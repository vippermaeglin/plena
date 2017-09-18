using Nevron.UI.WinForm.Controls;
using Nevron.UI.WinForm.Docking;

namespace M4
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                base.Dispose(disposing);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.m_ImgList = new System.Windows.Forms.ImageList(this.components);
            this.m_ImgList2 = new System.Windows.Forms.ImageList(this.components);
            this.m_ImgList3 = new System.Windows.Forms.ImageList(this.components);
            this.m_ImgList4 = new System.Windows.Forms.ImageList(this.components);
            this.tmrClearAlerts = new System.Windows.Forms.Timer(this.components);
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.m_CmdBarsManager = new Nevron.UI.WinForm.Controls.NCommandBarsManager(this.components);
            this.m_MenuBar = new Nevron.UI.WinForm.Controls.NMenuBar();
            this.mnuFile = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileLoadWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileImportExcel = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileImportCSV = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileSymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileSaveWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileSaveImage = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileSaveTemplate = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileExportCSV = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileExport = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFilePrint = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileExit = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuManagerWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuSettings = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuView = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFileToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuZoomTemplatesToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuPriceIndicatorsToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuChartToolsToolbar = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuColors = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuForeColor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuBackColor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuGridColor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuUpColor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuDownColor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuGradientTop = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuGradientBottom = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAppStyle = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAppOffice2007 = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAppOfficeSilver = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAppStandard = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewScaleType = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewShowXGrid = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewYGrid = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewSeparators = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewCrosshair = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewMagnetic = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuView3D = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuDarvasBoxes = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuStartPage = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuViewForex = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuData = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuDataLogin = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuCreateChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAlert = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuBackTest = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuScanner = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuZoomIn = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuZoomOut = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuScrollLeft = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuScrollRight = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuApplyTemplate = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuPriceStyle = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuPopoutChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuTools = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuText = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuBuySymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuSellSymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuExitSymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuTrendLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.m_ImageList5 = new System.Windows.Forms.ImageList(this.components);
            this.mnuRay = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuChannel = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuEllipse = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuSpeedLines = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuGannFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFibonacciArcs = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFibonacciRetracements = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFibonacciFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFibonacciTimeZones = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuTironeLevels = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuQuadrantLines = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuRaffRegression = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuErrorChannels = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuRectangle = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuArrow = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuFreehand = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdHorizontalLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdVerticalLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdRectangle = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdElipse = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdArrow = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuPolyline = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFreeHandDrawing = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFibonacciRetracements = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFibonacciProjections = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFibonacciArcs = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFibonacciFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdFibonacciTimeZones = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdGannFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCmdSpeedLines = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAI = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuApplyExpertAdvisor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuManageExpertAdvisors = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuCreateExpertAdvisor = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuConsensusReport = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuNN = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuPatternRecognition = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuHelp = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuScriptHelp = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuAbout = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCommand4 = new Nevron.UI.WinForm.Controls.NCommand();
            this.nCommand5 = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtFile = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.mnuExcel = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdImportExcel = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdNewChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdAlert = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdLoadWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdSaveWorkspace = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdPrintChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdHelp = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtView = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.nCommand32 = new Nevron.UI.WinForm.Controls.NCommand();
            this.cbxApplicationStyle = new Nevron.UI.WinForm.Controls.NComboBoxCommand();
            this.cmdUseSemiLogScale = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdShowXGrid = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdShowYGrid = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdShowPanelSeparators = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdThreeDStyle = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdDarvasBoxes = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdViewStarPage = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdViewForexScreen = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtZoom = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.cmdZoomIn = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdZoomOut = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdZoomArea = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdZoomZero = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdScrollLeft = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdScrollRight = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdTemplate = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdDelete = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtCalculator = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.cmdCalculator = new Nevron.UI.WinForm.Controls.NCalculatorDropDown();
            this.ndtPriceIndicators = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.nCommand1 = new Nevron.UI.WinForm.Controls.NCommand();
            this.cboPriceStyles = new Nevron.UI.WinForm.Controls.NComboBoxCommand();
            this.cmdCandleChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdStockLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdBarChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdHeikinAshi = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtChartTools = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.cmdChart = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdCrosshair = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdSelect = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdDeltaCursor = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdMagnetic = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdTextObject = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdBuySymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdSellSymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdExitSymbol = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdTrendLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdRay = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdChannel = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdHorizontalLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdVerticalLine = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdRectangle = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdElipse = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdArrow = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdPolyline = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFreeHandDrawing = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFibonacciRetracements = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFibonacciProjections = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFibonacciArcs = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFibonacciFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdFibonacciTimeZones = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdGannFan = new Nevron.UI.WinForm.Controls.NCommand();
            this.cmdSpeedLines = new Nevron.UI.WinForm.Controls.NCommand();
            this.ndtTechnicalAnalysis = new Nevron.UI.WinForm.Controls.NDockingToolbar();
            this.nCommand3 = new Nevron.UI.WinForm.Controls.NCommand();
            this.cboIndicators = new Nevron.UI.WinForm.Controls.NComboBoxCommand();
            this.nStatusBar1 = new Nevron.UI.WinForm.Controls.NStatusBar();
            this.nStatusBarPanel1 = new Nevron.UI.WinForm.Controls.NStatusBarPanel();
            this.nStatusBarPanel2 = new Nevron.UI.WinForm.Controls.NStatusBarPanel();
            this.nRichTextLabel1 = new Nevron.UI.WinForm.Controls.NRichTextLabel();
            this.m_DockManager = new Nevron.UI.WinForm.Docking.NDockManager(this.components);
            this.cmdCalcHost = new Nevron.UI.WinForm.Controls.NControlHostCommand();
            this.nCommand2 = new Nevron.UI.WinForm.Controls.NCommand();
            this.axStockChartX1 = new AxSTOCKCHARTXLib.AxStockChartX();
            this.tmrMessageService = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_CmdBarsManager)).BeginInit();
            this.ndtCalculator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nStatusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nStatusBarPanel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nRichTextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_DockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ImgList
            // 
            this.m_ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList.ImageStream")));
            this.m_ImgList.TransparentColor = System.Drawing.Color.White;
            this.m_ImgList.Images.SetKeyName(0, "Icon_Create_Layout.png");
            this.m_ImgList.Images.SetKeyName(1, "Icon_Open_Layout.png");
            this.m_ImgList.Images.SetKeyName(2, "Icon_Save.png");
            this.m_ImgList.Images.SetKeyName(3, "Print.bmp");
            // 
            // m_ImgList2
            // 
            this.m_ImgList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList2.ImageStream")));
            this.m_ImgList2.TransparentColor = System.Drawing.Color.Fuchsia;
            this.m_ImgList2.Images.SetKeyName(0, "Print.bmp");
            this.m_ImgList2.Images.SetKeyName(1, "Save.bmp");
            this.m_ImgList2.Images.SetKeyName(2, "Open.bmp");
            this.m_ImgList2.Images.SetKeyName(3, "OpenWeb.bmp");
            this.m_ImgList2.Images.SetKeyName(4, "Excel.PNG");
            this.m_ImgList2.Images.SetKeyName(5, "ImportExcel.bmp");
            this.m_ImgList2.Images.SetKeyName(6, "Globe.bmp");
            this.m_ImgList2.Images.SetKeyName(7, "Help.bmp");
            this.m_ImgList2.Images.SetKeyName(8, "Bars.bmp");
            this.m_ImgList2.Images.SetKeyName(9, "Alert.bmp");
            // 
            // m_ImgList3
            // 
            this.m_ImgList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList3.ImageStream")));
            this.m_ImgList3.TransparentColor = System.Drawing.Color.Fuchsia;
            this.m_ImgList3.Images.SetKeyName(0, "Bars.bmp");
            this.m_ImgList3.Images.SetKeyName(1, "PointAndFigure.bmp");
            this.m_ImgList3.Images.SetKeyName(2, "Renko.bmp");
            this.m_ImgList3.Images.SetKeyName(3, "ThreeLineBreak.bmp");
            this.m_ImgList3.Images.SetKeyName(4, "Candles.bmp");
            this.m_ImgList3.Images.SetKeyName(5, "CandleVolume.bmp");
            this.m_ImgList3.Images.SetKeyName(6, "EquiVolume.bmp");
            this.m_ImgList3.Images.SetKeyName(7, "EquiVolumeShadow.bmp");
            this.m_ImgList3.Images.SetKeyName(8, "Kagi.bmp");
            this.m_ImgList3.Images.SetKeyName(9, "Indicator.bmp");
            this.m_ImgList3.Images.SetKeyName(10, "Bands.bmp");
            // 
            // m_ImgList4
            // 
            this.m_ImgList4.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImgList4.ImageStream")));
            this.m_ImgList4.TransparentColor = System.Drawing.Color.Fuchsia;
            this.m_ImgList4.Images.SetKeyName(0, "Zoom.bmp");
            this.m_ImgList4.Images.SetKeyName(1, "ZoomOut.bmp");
            this.m_ImgList4.Images.SetKeyName(2, "Left.bmp");
            this.m_ImgList4.Images.SetKeyName(3, "Right.bmp");
            this.m_ImgList4.Images.SetKeyName(4, "Template.bmp");
            this.m_ImgList4.Images.SetKeyName(5, "Properties.bmp");
            this.m_ImgList4.Images.SetKeyName(6, "Help.bmp");
            this.m_ImgList4.Images.SetKeyName(7, "Delete.bmp");
            this.m_ImgList4.Images.SetKeyName(8, "Bars.bmp");
            this.m_ImgList4.Images.SetKeyName(9, "patternrecognition.jpg");
            this.m_ImgList4.Images.SetKeyName(10, "Twitter.bmp");
            this.m_ImgList4.Images.SetKeyName(11, "1-CandleChart.png");
            this.m_ImgList4.Images.SetKeyName(12, "2-StockLine.png");
            this.m_ImgList4.Images.SetKeyName(13, "3-BarChart.png");
            this.m_ImgList4.Images.SetKeyName(14, "4-HeinkinAshi.png");
            this.m_ImgList4.Images.SetKeyName(15, "5-HeinkinAshiSuavizado.png");
            this.m_ImgList4.Images.SetKeyName(16, "6-ZoomIn.png");
            this.m_ImgList4.Images.SetKeyName(17, "7-ZoomOut.png");
            this.m_ImgList4.Images.SetKeyName(18, "8-ZoomArea.png");
            this.m_ImgList4.Images.SetKeyName(19, "9-Overview.png");
            this.m_ImgList4.Images.SetKeyName(20, "10-ZoomReset.png");
            this.m_ImgList4.Images.SetKeyName(21, "11-Daily.png");
            this.m_ImgList4.Images.SetKeyName(22, "12-Weekly.png");
            this.m_ImgList4.Images.SetKeyName(23, "13-Monthly.png");
            this.m_ImgList4.Images.SetKeyName(24, "14-PeriodicityCustom.png");
            this.m_ImgList4.Images.SetKeyName(25, "15-Indicators.png");
            this.m_ImgList4.Images.SetKeyName(26, "16-NewChart.png");
            this.m_ImgList4.Images.SetKeyName(27, "17-Crosshair.png");
            this.m_ImgList4.Images.SetKeyName(28, "18-Select.png");
            this.m_ImgList4.Images.SetKeyName(29, "19-Magnetic.png");
            this.m_ImgList4.Images.SetKeyName(30, "20-Text.png");
            this.m_ImgList4.Images.SetKeyName(31, "21-BuySymbol.png");
            this.m_ImgList4.Images.SetKeyName(32, "22-SellSymbol.png");
            this.m_ImgList4.Images.SetKeyName(33, "23-ExitSymbol.png");
            this.m_ImgList4.Images.SetKeyName(34, "24-TrendLine.png");
            this.m_ImgList4.Images.SetKeyName(35, "25-Ray.png");
            this.m_ImgList4.Images.SetKeyName(36, "26-Channel.png");
            this.m_ImgList4.Images.SetKeyName(37, "27-HorizontalLine.png");
            this.m_ImgList4.Images.SetKeyName(38, "28-VerticalLine.png");
            this.m_ImgList4.Images.SetKeyName(39, "29-Rectangle.png");
            this.m_ImgList4.Images.SetKeyName(40, "30-Ellipse.png");
            this.m_ImgList4.Images.SetKeyName(41, "31-Arrow.png");
            this.m_ImgList4.Images.SetKeyName(42, "32-Polyline.png");
            this.m_ImgList4.Images.SetKeyName(43, "33-FreehandDrawing.png");
            this.m_ImgList4.Images.SetKeyName(44, "34-FibonacciRetracements.png");
            this.m_ImgList4.Images.SetKeyName(45, "35-FibonacciProjecttions.png");
            this.m_ImgList4.Images.SetKeyName(46, "36-FibonacciArcs.png");
            this.m_ImgList4.Images.SetKeyName(47, "37-FibonacciFan.png");
            this.m_ImgList4.Images.SetKeyName(48, "38-FibonacciTimeZones.png");
            this.m_ImgList4.Images.SetKeyName(49, "39-GannFan.png");
            this.m_ImgList4.Images.SetKeyName(50, "40-SpeedLines.png");
            // 
            // tmrClearAlerts
            // 
            this.tmrClearAlerts.Interval = 59000;
            this.tmrClearAlerts.Tick += new System.EventHandler(this.tmrClearAlerts_Tick);
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // m_CmdBarsManager
            // 
            this.m_CmdBarsManager.ImageList = this.m_ImgList;
            this.m_CmdBarsManager.ParentControl = this;
            this.m_CmdBarsManager.Toolbars.Add(this.m_MenuBar);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtFile);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtView);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtZoom);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtCalculator);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtPriceIndicators);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtChartTools);
            this.m_CmdBarsManager.Toolbars.Add(this.ndtTechnicalAnalysis);
            // 
            // m_MenuBar
            // 
            this.m_MenuBar.AutoDropDownDelay = false;
            this.m_MenuBar.BackgroundType = Nevron.UI.WinForm.Controls.BackgroundType.Transparent;
            this.m_MenuBar.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuFile,
            this.mnuWorkspace,
            this.mnuSettings,
            this.mnuView,
            this.mnuData,
            this.mnuChart,
            this.mnuTools,
            this.mnuAI,
            this.mnuHelp,
            this.nCommand4,
            this.nCommand5});
            this.m_MenuBar.DefaultCommandStyle = Nevron.UI.WinForm.Controls.CommandStyle.Text;
            this.m_MenuBar.DefaultLocation = new System.Drawing.Point(0, 0);
            this.m_MenuBar.HasPendantCommand = false;
            this.m_MenuBar.Name = "m_MenuBar";
            this.m_MenuBar.PrefferedRowIndex = 0;
            this.m_MenuBar.RowIndex = 0;
            this.m_MenuBar.ShowTooltips = false;
            resources.ApplyResources(this.m_MenuBar, "m_MenuBar");
            // 
            // mnuFile
            // 
            this.mnuFile.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuFileLoadWorkspace,
            this.mnuFileImportExcel,
            this.mnuFileImportCSV,
            this.mnuFileSymbol,
            this.mnuFileSaveWorkspace,
            this.mnuFileSaveImage,
            this.mnuFileSaveTemplate,
            this.mnuFileExportCSV,
            this.mnuFileExport,
            this.mnuFilePrint,
            this.mnuFileExit});
            this.mnuFile.Properties.ImageList = this.m_ImgList2;
            this.mnuFile.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuFile.Properties.Text = resources.GetString("resource.Text11");
            // 
            // mnuFileLoadWorkspace
            // 
            this.mnuFileLoadWorkspace.Properties.ImageIndex = 2;
            this.mnuFileLoadWorkspace.Properties.ImageList = this.m_ImgList2;
            this.mnuFileLoadWorkspace.Properties.Text = resources.GetString("resource.Text");
            this.mnuFileLoadWorkspace.Properties.Visible = false;
            this.mnuFileLoadWorkspace.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileLoadWorkspace_Click);
            // 
            // mnuFileImportExcel
            // 
            this.mnuFileImportExcel.Properties.ImageIndex = 5;
            this.mnuFileImportExcel.Properties.ImageList = this.m_ImgList2;
            this.mnuFileImportExcel.Properties.Text = resources.GetString("resource.Text1");
            this.mnuFileImportExcel.Properties.Visible = false;
            this.mnuFileImportExcel.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileImportExcel_Click);
            // 
            // mnuFileImportCSV
            // 
            this.mnuFileImportCSV.Properties.BeginGroup = true;
            this.mnuFileImportCSV.Properties.Text = resources.GetString("resource.Text2");
            this.mnuFileImportCSV.Properties.Visible = false;
            this.mnuFileImportCSV.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileImportCSV_Click);
            // 
            // mnuFileSymbol
            // 
            this.mnuFileSymbol.Properties.Text = resources.GetString("resource.Text3");
            this.mnuFileSymbol.Properties.Visible = false;
            this.mnuFileSymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuFileSymbolClick);
            // 
            // mnuFileSaveWorkspace
            // 
            this.mnuFileSaveWorkspace.Properties.BeginGroup = true;
            this.mnuFileSaveWorkspace.Properties.ImageIndex = 1;
            this.mnuFileSaveWorkspace.Properties.ImageList = this.m_ImgList2;
            this.mnuFileSaveWorkspace.Properties.Text = resources.GetString("resource.Text4");
            this.mnuFileSaveWorkspace.Properties.Visible = false;
            this.mnuFileSaveWorkspace.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileSaveWorkspace_Click);
            // 
            // mnuFileSaveImage
            // 
            this.mnuFileSaveImage.Properties.Text = resources.GetString("resource.Text5");
            this.mnuFileSaveImage.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileSaveImage_Click);
            // 
            // mnuFileSaveTemplate
            // 
            this.mnuFileSaveTemplate.Properties.Text = resources.GetString("resource.Text6");
            this.mnuFileSaveTemplate.Properties.Visible = false;
            this.mnuFileSaveTemplate.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileSaveTemplate_Click);
            // 
            // mnuFileExportCSV
            // 
            this.mnuFileExportCSV.Properties.BeginGroup = true;
            this.mnuFileExportCSV.Properties.Text = resources.GetString("resource.Text7");
            this.mnuFileExportCSV.Properties.Visible = false;
            this.mnuFileExportCSV.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileExportCSV_Click);
            // 
            // mnuFileExport
            // 
            this.mnuFileExport.Properties.ImageIndex = 4;
            this.mnuFileExport.Properties.ImageList = this.m_ImgList2;
            this.mnuFileExport.Properties.Text = resources.GetString("resource.Text8");
            this.mnuFileExport.Properties.Visible = false;
            this.mnuFileExport.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileExport_Click);
            // 
            // mnuFilePrint
            // 
            this.mnuFilePrint.Properties.ImageIndex = 0;
            this.mnuFilePrint.Properties.ImageList = this.m_ImgList2;
            this.mnuFilePrint.Properties.Text = resources.GetString("resource.Text9");
            this.mnuFilePrint.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFilePrint_Click);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Properties.BeginGroup = true;
            this.mnuFileExit.Properties.Text = resources.GetString("resource.Text10");
            this.mnuFileExit.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFileExit_Click);
            // 
            // mnuWorkspace
            // 
            this.mnuWorkspace.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuManagerWorkspace});
            this.mnuWorkspace.Properties.ImageShadow = true;
            this.mnuWorkspace.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuWorkspace.Properties.Text = resources.GetString("resource.Text13");
            // 
            // mnuManagerWorkspace
            // 
            this.mnuManagerWorkspace.Properties.Text = resources.GetString("resource.Text12");
            this.mnuManagerWorkspace.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuManagerWorkspaceClick);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuSettings.Properties.Text = resources.GetString("resource.Text14");
            this.mnuSettings.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuSettingsClick);
            // 
            // mnuView
            // 
            this.mnuView.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuToolbar,
            this.mnuColors,
            this.mnuAppStyle,
            this.mnuViewScaleType,
            this.mnuViewShowXGrid,
            this.mnuViewYGrid,
            this.mnuViewSeparators,
            this.mnuViewCrosshair,
            this.mnuViewMagnetic,
            this.mnuView3D,
            this.mnuDarvasBoxes,
            this.mnuStartPage,
            this.mnuViewForex});
            this.mnuView.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuView.Properties.Text = resources.GetString("resource.Text43");
            // 
            // mnuToolbar
            // 
            this.mnuToolbar.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuFileToolbar,
            this.mnuViewToolbar,
            this.mnuZoomTemplatesToolbar,
            this.mnuPriceIndicatorsToolbar,
            this.mnuChartToolsToolbar});
            this.mnuToolbar.Properties.Text = resources.GetString("resource.Text20");
            // 
            // mnuFileToolbar
            // 
            this.mnuFileToolbar.Properties.Text = resources.GetString("resource.Text15");
            this.mnuFileToolbar.Properties.Visible = false;
            this.mnuFileToolbar.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuFileToolbarClick);
            // 
            // mnuViewToolbar
            // 
            this.mnuViewToolbar.Properties.Text = resources.GetString("resource.Text16");
            this.mnuViewToolbar.Properties.Visible = false;
            this.mnuViewToolbar.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuViewToolbarClick);
            // 
            // mnuZoomTemplatesToolbar
            // 
            this.mnuZoomTemplatesToolbar.Properties.Text = resources.GetString("resource.Text17");
            this.mnuZoomTemplatesToolbar.Properties.Visible = false;
            this.mnuZoomTemplatesToolbar.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuZoomTemplatesToolbarClick);
            // 
            // mnuPriceIndicatorsToolbar
            // 
            this.mnuPriceIndicatorsToolbar.Properties.Text = resources.GetString("resource.Text18");
            this.mnuPriceIndicatorsToolbar.Properties.Visible = false;
            this.mnuPriceIndicatorsToolbar.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuPriceIndicatorsToolbarClick);
            // 
            // mnuChartToolsToolbar
            // 
            this.mnuChartToolsToolbar.Properties.Text = resources.GetString("resource.Text19");
            this.mnuChartToolsToolbar.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuChartToolsToolbarClick);
            // 
            // mnuColors
            // 
            this.mnuColors.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuForeColor,
            this.mnuBackColor,
            this.mnuGridColor,
            this.mnuUpColor,
            this.mnuDownColor,
            this.mnuGradientTop,
            this.mnuGradientBottom});
            this.mnuColors.Properties.BeginGroup = true;
            this.mnuColors.Properties.Text = resources.GetString("resource.Text28");
            this.mnuColors.Properties.Visible = false;
            // 
            // mnuForeColor
            // 
            this.mnuForeColor.Properties.Text = resources.GetString("resource.Text21");
            this.mnuForeColor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuForeColor_Click);
            // 
            // mnuBackColor
            // 
            this.mnuBackColor.Properties.Text = resources.GetString("resource.Text22");
            this.mnuBackColor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuBackColor_Click);
            // 
            // mnuGridColor
            // 
            this.mnuGridColor.Properties.Text = resources.GetString("resource.Text23");
            this.mnuGridColor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuGridColor_Click);
            // 
            // mnuUpColor
            // 
            this.mnuUpColor.Properties.Text = resources.GetString("resource.Text24");
            this.mnuUpColor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuUpColor_Click);
            // 
            // mnuDownColor
            // 
            this.mnuDownColor.Properties.Text = resources.GetString("resource.Text25");
            this.mnuDownColor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuDownColor_Click);
            // 
            // mnuGradientTop
            // 
            this.mnuGradientTop.Properties.Text = resources.GetString("resource.Text26");
            this.mnuGradientTop.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuGradientTop_Click);
            // 
            // mnuGradientBottom
            // 
            this.mnuGradientBottom.Properties.Text = resources.GetString("resource.Text27");
            this.mnuGradientBottom.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuGradientBottom_Click);
            // 
            // mnuAppStyle
            // 
            this.mnuAppStyle.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuAppOffice2007,
            this.mnuAppOfficeSilver,
            this.mnuAppStandard});
            this.mnuAppStyle.Properties.BeginGroup = true;
            this.mnuAppStyle.Properties.Text = resources.GetString("resource.Text32");
            // 
            // mnuAppOffice2007
            // 
            this.mnuAppOffice2007.Checked = true;
            this.mnuAppOffice2007.Properties.Text = resources.GetString("resource.Text29");
            this.mnuAppOffice2007.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuAppOffice2007_Click);
            // 
            // mnuAppOfficeSilver
            // 
            this.mnuAppOfficeSilver.Properties.Text = resources.GetString("resource.Text30");
            this.mnuAppOfficeSilver.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuAppOfficeSilver_Click);
            // 
            // mnuAppStandard
            // 
            this.mnuAppStandard.Properties.Text = resources.GetString("resource.Text31");
            this.mnuAppStandard.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuAppStandard_Click);
            // 
            // mnuViewScaleType
            // 
            this.mnuViewScaleType.Properties.BeginGroup = true;
            this.mnuViewScaleType.Properties.Text = resources.GetString("resource.Text33");
            this.mnuViewScaleType.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewScaleType_Click);
            // 
            // mnuViewShowXGrid
            // 
            this.mnuViewShowXGrid.Properties.Text = resources.GetString("resource.Text34");
            this.mnuViewShowXGrid.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewShowXGrid_Click);
            // 
            // mnuViewYGrid
            // 
            this.mnuViewYGrid.Properties.Text = resources.GetString("resource.Text35");
            this.mnuViewYGrid.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewYGrid_Click);
            // 
            // mnuViewSeparators
            // 
            this.mnuViewSeparators.Properties.Text = resources.GetString("resource.Text36");
            this.mnuViewSeparators.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewSeparators_Click);
            // 
            // mnuViewCrosshair
            // 
            this.mnuViewCrosshair.Properties.Text = resources.GetString("resource.Text37");
            this.mnuViewCrosshair.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewCrosshair_Click);
            // 
            // mnuViewMagnetic
            // 
            this.mnuViewMagnetic.Properties.Text = resources.GetString("resource.Text38");
            this.mnuViewMagnetic.Properties.Visible = false;
            this.mnuViewMagnetic.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuViewMagnetic_Click);
            // 
            // mnuView3D
            // 
            this.mnuView3D.Properties.BeginGroup = true;
            this.mnuView3D.Properties.Text = resources.GetString("resource.Text39");
            this.mnuView3D.Properties.Visible = false;
            this.mnuView3D.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuView3D_Click);
            // 
            // mnuDarvasBoxes
            // 
            this.mnuDarvasBoxes.Properties.Text = resources.GetString("resource.Text40");
            this.mnuDarvasBoxes.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuDarvasBoxes_Click);
            // 
            // mnuStartPage
            // 
            this.mnuStartPage.Properties.BeginGroup = true;
            this.mnuStartPage.Properties.Text = resources.GetString("resource.Text41");
            this.mnuStartPage.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuStartPage_Click);
            // 
            // mnuViewForex
            // 
            this.mnuViewForex.Properties.BeginGroup = true;
            this.mnuViewForex.Properties.Text = resources.GetString("resource.Text42");
            this.mnuViewForex.Properties.Visible = false;
            this.mnuViewForex.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuViewForexClick);
            // 
            // mnuData
            // 
            this.mnuData.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuDataLogin,
            this.mnuCreateChart,
            this.mnuAlert,
            this.mnuBackTest,
            this.mnuScanner});
            this.mnuData.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuData.Properties.Text = resources.GetString("resource.Text49");
            this.mnuData.Properties.Visible = false;
            // 
            // mnuDataLogin
            // 
            this.mnuDataLogin.Properties.Text = resources.GetString("resource.Text44");
            this.mnuDataLogin.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuDataLogin_Click);
            // 
            // mnuCreateChart
            // 
            this.mnuCreateChart.Properties.BeginGroup = true;
            this.mnuCreateChart.Properties.ImageIndex = 8;
            this.mnuCreateChart.Properties.ImageList = this.m_ImgList2;
            this.mnuCreateChart.Properties.Text = resources.GetString("resource.Text45");
            this.mnuCreateChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuCreateChart_Click);
            // 
            // mnuAlert
            // 
            this.mnuAlert.Properties.ImageIndex = 9;
            this.mnuAlert.Properties.ImageList = this.m_ImgList2;
            this.mnuAlert.Properties.Text = resources.GetString("resource.Text46");
            this.mnuAlert.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuAlert_Click);
            // 
            // mnuBackTest
            // 
            this.mnuBackTest.Properties.Text = resources.GetString("resource.Text47");
            this.mnuBackTest.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuBackTest_Click);
            // 
            // mnuScanner
            // 
            this.mnuScanner.Properties.Text = resources.GetString("resource.Text48");
            this.mnuScanner.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuScanner_Click);
            // 
            // mnuChart
            // 
            this.mnuChart.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuZoomIn,
            this.mnuZoomOut,
            this.mnuScrollLeft,
            this.mnuScrollRight,
            this.mnuApplyTemplate,
            this.mnuPriceStyle,
            this.mnuPopoutChart});
            this.mnuChart.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuChart.Properties.Text = resources.GetString("resource.Text57");
            // 
            // mnuZoomIn
            // 
            this.mnuZoomIn.Properties.ImageIndex = 0;
            this.mnuZoomIn.Properties.ImageList = this.m_ImgList4;
            this.mnuZoomIn.Properties.Text = resources.GetString("resource.Text50");
            this.mnuZoomIn.Properties.Visible = false;
            this.mnuZoomIn.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuZoomIn_Click);
            // 
            // mnuZoomOut
            // 
            this.mnuZoomOut.Properties.ImageIndex = 1;
            this.mnuZoomOut.Properties.ImageList = this.m_ImgList4;
            this.mnuZoomOut.Properties.Text = resources.GetString("resource.Text51");
            this.mnuZoomOut.Properties.Visible = false;
            this.mnuZoomOut.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuZoomOut_Click);
            // 
            // mnuScrollLeft
            // 
            this.mnuScrollLeft.Properties.ImageIndex = 2;
            this.mnuScrollLeft.Properties.ImageList = this.m_ImgList4;
            this.mnuScrollLeft.Properties.Text = resources.GetString("resource.Text52");
            this.mnuScrollLeft.Properties.Visible = false;
            this.mnuScrollLeft.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuScrollLeft_Click);
            // 
            // mnuScrollRight
            // 
            this.mnuScrollRight.Properties.ImageIndex = 3;
            this.mnuScrollRight.Properties.ImageList = this.m_ImgList4;
            this.mnuScrollRight.Properties.Text = resources.GetString("resource.Text53");
            this.mnuScrollRight.Properties.Visible = false;
            this.mnuScrollRight.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuScrollRight_Click);
            // 
            // mnuApplyTemplate
            // 
            this.mnuApplyTemplate.Properties.ImageIndex = 4;
            this.mnuApplyTemplate.Properties.ImageList = this.m_ImgList4;
            this.mnuApplyTemplate.Properties.Text = resources.GetString("resource.Text54");
            this.mnuApplyTemplate.Properties.Visible = false;
            this.mnuApplyTemplate.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuApplyTemplate_Click);
            // 
            // mnuPriceStyle
            // 
            this.mnuPriceStyle.Properties.Text = resources.GetString("resource.Text55");
            this.mnuPriceStyle.Properties.Visible = false;
            this.mnuPriceStyle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuPriceStyle_Click);
            // 
            // mnuPopoutChart
            // 
            this.mnuPopoutChart.Properties.Text = resources.GetString("resource.Text56");
            this.mnuPopoutChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuPopoutChartClick);
            // 
            // mnuTools
            // 
            this.mnuTools.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuText,
            this.mnuBuySymbol,
            this.mnuSellSymbol,
            this.mnuExitSymbol,
            this.mnuTrendLine,
            this.mnuRay,
            this.mnuChannel,
            this.mnuEllipse,
            this.mnuSpeedLines,
            this.mnuGannFan,
            this.mnuFibonacciArcs,
            this.mnuFibonacciRetracements,
            this.mnuFibonacciFan,
            this.mnuFibonacciTimeZones,
            this.mnuTironeLevels,
            this.mnuQuadrantLines,
            this.mnuRaffRegression,
            this.mnuErrorChannels,
            this.mnuRectangle,
            this.mnuArrow,
            this.mnuFreehand,
            this.nCmdHorizontalLine,
            this.nCmdVerticalLine,
            this.nCmdRectangle,
            this.nCmdElipse,
            this.nCmdArrow,
            this.mnuPolyline,
            this.nCmdFreeHandDrawing,
            this.nCmdFibonacciRetracements,
            this.nCmdFibonacciProjections,
            this.nCmdFibonacciArcs,
            this.nCmdFibonacciFan,
            this.nCmdFibonacciTimeZones,
            this.nCmdGannFan,
            this.nCmdSpeedLines});
            this.mnuTools.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuTools.Properties.Text = resources.GetString("resource.Text93");
            this.mnuTools.Properties.Visible = false;
            // 
            // mnuText
            // 
            this.mnuText.Properties.Text = resources.GetString("resource.Text58");
            this.mnuText.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuText_Click);
            // 
            // mnuBuySymbol
            // 
            this.mnuBuySymbol.Properties.BeginGroup = true;
            this.mnuBuySymbol.Properties.Text = resources.GetString("resource.Text59");
            this.mnuBuySymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuBuySymbol_Click);
            // 
            // mnuSellSymbol
            // 
            this.mnuSellSymbol.Properties.Text = resources.GetString("resource.Text60");
            this.mnuSellSymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuSellSymbol_Click);
            // 
            // mnuExitSymbol
            // 
            this.mnuExitSymbol.Properties.Text = resources.GetString("resource.Text61");
            this.mnuExitSymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuExitSymbol_Click);
            // 
            // mnuTrendLine
            // 
            this.mnuTrendLine.Properties.BeginGroup = true;
            this.mnuTrendLine.Properties.ImageIndex = 0;
            this.mnuTrendLine.Properties.ImageList = this.m_ImageList5;
            this.mnuTrendLine.Properties.Text = resources.GetString("resource.Text62");
            this.mnuTrendLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuTrendLine_Click);
            // 
            // m_ImageList5
            // 
            this.m_ImageList5.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImageList5.ImageStream")));
            this.m_ImageList5.TransparentColor = System.Drawing.Color.Transparent;
            this.m_ImageList5.Images.SetKeyName(0, "LineStudies_TrendLine.png");
            this.m_ImageList5.Images.SetKeyName(1, "LineStudies_Ellipse.png");
            this.m_ImageList5.Images.SetKeyName(2, "LineStudies_ErrorChannels.png");
            this.m_ImageList5.Images.SetKeyName(3, "LineStudies_FibArcs.png");
            this.m_ImageList5.Images.SetKeyName(4, "LineStudies_FibFan.png");
            this.m_ImageList5.Images.SetKeyName(5, "LineStudies_FibRetracements.png");
            this.m_ImageList5.Images.SetKeyName(6, "LineStudies_FibTimeZones.png");
            this.m_ImageList5.Images.SetKeyName(7, "LineStudies_GannFan.png");
            this.m_ImageList5.Images.SetKeyName(8, "LineStudies_QuadLines.png");
            this.m_ImageList5.Images.SetKeyName(9, "LineStudies_RaffRegression.png");
            this.m_ImageList5.Images.SetKeyName(10, "LineStudies_SpeedLines.png");
            this.m_ImageList5.Images.SetKeyName(11, "LineStudies_Tirone.png");
            // 
            // mnuRay
            // 
            this.mnuRay.Properties.Text = resources.GetString("resource.Text63");
            this.mnuRay.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuRayClick);
            // 
            // mnuChannel
            // 
            this.mnuChannel.Properties.Text = resources.GetString("resource.Text64");
            this.mnuChannel.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.MnuChannelClick);
            // 
            // mnuEllipse
            // 
            this.mnuEllipse.Properties.ImageIndex = 1;
            this.mnuEllipse.Properties.ImageList = this.m_ImageList5;
            this.mnuEllipse.Properties.Text = resources.GetString("resource.Text65");
            this.mnuEllipse.Properties.Visible = false;
            this.mnuEllipse.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuEllipse_Click);
            // 
            // mnuSpeedLines
            // 
            this.mnuSpeedLines.Properties.ImageIndex = 10;
            this.mnuSpeedLines.Properties.ImageList = this.m_ImageList5;
            this.mnuSpeedLines.Properties.Text = resources.GetString("resource.Text66");
            this.mnuSpeedLines.Properties.Visible = false;
            this.mnuSpeedLines.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuSpeedLines_Click);
            // 
            // mnuGannFan
            // 
            this.mnuGannFan.Properties.ImageIndex = 4;
            this.mnuGannFan.Properties.ImageList = this.m_ImageList5;
            this.mnuGannFan.Properties.Text = resources.GetString("resource.Text67");
            this.mnuGannFan.Properties.Visible = false;
            this.mnuGannFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuGannFan_Click);
            // 
            // mnuFibonacciArcs
            // 
            this.mnuFibonacciArcs.Properties.ImageIndex = 3;
            this.mnuFibonacciArcs.Properties.ImageList = this.m_ImageList5;
            this.mnuFibonacciArcs.Properties.Text = resources.GetString("resource.Text68");
            this.mnuFibonacciArcs.Properties.Visible = false;
            this.mnuFibonacciArcs.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFibonacciArcs_Click);
            // 
            // mnuFibonacciRetracements
            // 
            this.mnuFibonacciRetracements.Properties.ImageIndex = 5;
            this.mnuFibonacciRetracements.Properties.ImageList = this.m_ImageList5;
            this.mnuFibonacciRetracements.Properties.Text = resources.GetString("resource.Text69");
            this.mnuFibonacciRetracements.Properties.Visible = false;
            this.mnuFibonacciRetracements.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFibonacciRetracements_Click);
            // 
            // mnuFibonacciFan
            // 
            this.mnuFibonacciFan.Properties.ImageIndex = 10;
            this.mnuFibonacciFan.Properties.ImageList = this.m_ImageList5;
            this.mnuFibonacciFan.Properties.Text = resources.GetString("resource.Text70");
            this.mnuFibonacciFan.Properties.Visible = false;
            this.mnuFibonacciFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFibonacciFan_Click);
            // 
            // mnuFibonacciTimeZones
            // 
            this.mnuFibonacciTimeZones.Properties.ImageIndex = 6;
            this.mnuFibonacciTimeZones.Properties.ImageList = this.m_ImageList5;
            this.mnuFibonacciTimeZones.Properties.Text = resources.GetString("resource.Text71");
            this.mnuFibonacciTimeZones.Properties.Visible = false;
            this.mnuFibonacciTimeZones.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFibonacciTimeZones_Click);
            // 
            // mnuTironeLevels
            // 
            this.mnuTironeLevels.Properties.ImageIndex = 11;
            this.mnuTironeLevels.Properties.ImageList = this.m_ImageList5;
            this.mnuTironeLevels.Properties.Text = resources.GetString("resource.Text72");
            this.mnuTironeLevels.Properties.Visible = false;
            this.mnuTironeLevels.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuTironeLevels_Click);
            // 
            // mnuQuadrantLines
            // 
            this.mnuQuadrantLines.Properties.ImageIndex = 8;
            this.mnuQuadrantLines.Properties.ImageList = this.m_ImageList5;
            this.mnuQuadrantLines.Properties.Text = resources.GetString("resource.Text73");
            this.mnuQuadrantLines.Properties.Visible = false;
            this.mnuQuadrantLines.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuQuadrantLines_Click);
            // 
            // mnuRaffRegression
            // 
            this.mnuRaffRegression.Properties.ImageIndex = 9;
            this.mnuRaffRegression.Properties.ImageList = this.m_ImageList5;
            this.mnuRaffRegression.Properties.Text = resources.GetString("resource.Text74");
            this.mnuRaffRegression.Properties.Visible = false;
            this.mnuRaffRegression.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuRaffRegression_Click);
            // 
            // mnuErrorChannels
            // 
            this.mnuErrorChannels.Properties.ImageIndex = 11;
            this.mnuErrorChannels.Properties.ImageList = this.m_ImageList5;
            this.mnuErrorChannels.Properties.Text = resources.GetString("resource.Text75");
            this.mnuErrorChannels.Properties.Visible = false;
            this.mnuErrorChannels.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuErrorChannels_Click);
            // 
            // mnuRectangle
            // 
            this.mnuRectangle.Properties.Text = resources.GetString("resource.Text76");
            this.mnuRectangle.Properties.Visible = false;
            this.mnuRectangle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuRectangle_Click);
            // 
            // mnuArrow
            // 
            this.mnuArrow.Properties.Text = resources.GetString("resource.Text77");
            this.mnuArrow.Properties.Visible = false;
            this.mnuArrow.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuArrow_Click);
            // 
            // mnuFreehand
            // 
            this.mnuFreehand.Properties.Text = resources.GetString("resource.Text78");
            this.mnuFreehand.Properties.Visible = false;
            this.mnuFreehand.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFreehand_Click);
            // 
            // nCmdHorizontalLine
            // 
            this.nCmdHorizontalLine.Properties.Text = resources.GetString("resource.Text79");
            this.nCmdHorizontalLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdHorizontalLine_Click);
            // 
            // nCmdVerticalLine
            // 
            this.nCmdVerticalLine.Properties.Text = resources.GetString("resource.Text80");
            this.nCmdVerticalLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdVerticalLine_Click);
            // 
            // nCmdRectangle
            // 
            this.nCmdRectangle.Properties.Text = resources.GetString("resource.Text81");
            this.nCmdRectangle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdRectangle_Click);
            // 
            // nCmdElipse
            // 
            this.nCmdElipse.Properties.Text = resources.GetString("resource.Text82");
            this.nCmdElipse.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdElipse_Click);
            // 
            // nCmdArrow
            // 
            this.nCmdArrow.Properties.Text = resources.GetString("resource.Text83");
            this.nCmdArrow.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdArrow_Click);
            // 
            // mnuPolyline
            // 
            this.mnuPolyline.Properties.Text = resources.GetString("resource.Text84");
            this.mnuPolyline.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuPolyline_Click);
            // 
            // nCmdFreeHandDrawing
            // 
            this.nCmdFreeHandDrawing.Properties.Text = resources.GetString("resource.Text85");
            // 
            // nCmdFibonacciRetracements
            // 
            this.nCmdFibonacciRetracements.Properties.BeginGroup = true;
            this.nCmdFibonacciRetracements.Properties.Text = resources.GetString("resource.Text86");
            this.nCmdFibonacciRetracements.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuFibonacciRetracements_Click);
            // 
            // nCmdFibonacciProjections
            // 
            this.nCmdFibonacciProjections.Properties.Text = resources.GetString("resource.Text87");
            // 
            // nCmdFibonacciArcs
            // 
            this.nCmdFibonacciArcs.Properties.Text = resources.GetString("resource.Text88");
            this.nCmdFibonacciArcs.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdFibonacciArcs_Click);
            // 
            // nCmdFibonacciFan
            // 
            this.nCmdFibonacciFan.Properties.Text = resources.GetString("resource.Text89");
            this.nCmdFibonacciFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdFibonacciFan_Click);
            // 
            // nCmdFibonacciTimeZones
            // 
            this.nCmdFibonacciTimeZones.Properties.Text = resources.GetString("resource.Text90");
            this.nCmdFibonacciTimeZones.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdFibonacciTimeZones_Click);
            // 
            // nCmdGannFan
            // 
            this.nCmdGannFan.Properties.BeginGroup = true;
            this.nCmdGannFan.Properties.Text = resources.GetString("resource.Text91");
            this.nCmdGannFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdGannFan_Click);
            // 
            // nCmdSpeedLines
            // 
            this.nCmdSpeedLines.Properties.Text = resources.GetString("resource.Text92");
            this.nCmdSpeedLines.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.nCmdSpeedLines_Click);
            // 
            // mnuAI
            // 
            this.mnuAI.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuApplyExpertAdvisor,
            this.mnuManageExpertAdvisors,
            this.mnuCreateExpertAdvisor,
            this.mnuConsensusReport,
            this.mnuNN,
            this.mnuPatternRecognition});
            this.mnuAI.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuAI.Properties.Text = resources.GetString("resource.Text100");
            this.mnuAI.Properties.Visible = false;
            // 
            // mnuApplyExpertAdvisor
            // 
            this.mnuApplyExpertAdvisor.Properties.Text = resources.GetString("resource.Text94");
            this.mnuApplyExpertAdvisor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuApplyExpertAdvisor_Click);
            // 
            // mnuManageExpertAdvisors
            // 
            this.mnuManageExpertAdvisors.Properties.BeginGroup = true;
            this.mnuManageExpertAdvisors.Properties.Text = resources.GetString("resource.Text95");
            this.mnuManageExpertAdvisors.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuManageExpertAdvisors_Click);
            // 
            // mnuCreateExpertAdvisor
            // 
            this.mnuCreateExpertAdvisor.Properties.Text = resources.GetString("resource.Text96");
            this.mnuCreateExpertAdvisor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuCreateExpertAdvisor_Click);
            // 
            // mnuConsensusReport
            // 
            this.mnuConsensusReport.Properties.BeginGroup = true;
            this.mnuConsensusReport.Properties.Text = resources.GetString("resource.Text97");
            this.mnuConsensusReport.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuConsensusReport_Click);
            // 
            // mnuNN
            // 
            this.mnuNN.Properties.BeginGroup = true;
            this.mnuNN.Properties.Text = resources.GetString("resource.Text98");
            this.mnuNN.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuNN_Click);
            // 
            // mnuPatternRecognition
            // 
            this.mnuPatternRecognition.Properties.BeginGroup = true;
            this.mnuPatternRecognition.Properties.ImageIndex = 9;
            this.mnuPatternRecognition.Properties.Text = resources.GetString("resource.Text99");
            this.mnuPatternRecognition.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuPatternRecognition_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuScriptHelp,
            this.mnuAbout});
            this.mnuHelp.Properties.ImageList = this.m_ImgList2;
            this.mnuHelp.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.mnuHelp.Properties.Text = resources.GetString("resource.Text103");
            // 
            // mnuScriptHelp
            // 
            this.mnuScriptHelp.Properties.Text = resources.GetString("resource.Text101");
            this.mnuScriptHelp.Properties.Visible = false;
            this.mnuScriptHelp.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuScriptHelp_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Properties.Text = resources.GetString("resource.Text102");
            this.mnuAbout.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuAbout_Click);
            // 
            // nCommand4
            // 
            this.nCommand4.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            // 
            // nCommand5
            // 
            this.nCommand5.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            // 
            // ndtFile
            // 
            this.ndtFile.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuExcel,
            this.cmdImportExcel,
            this.cmdNewChart,
            this.cmdAlert,
            this.cmdLoadWorkspace,
            this.cmdSaveWorkspace,
            this.cmdPrintChart,
            this.cmdHelp});
            this.ndtFile.DefaultLocation = new System.Drawing.Point(2, 26);
            this.ndtFile.ImageList = this.m_ImgList2;
            this.ndtFile.Name = "ndtFile";
            this.ndtFile.PrefferedRowIndex = 1;
            this.ndtFile.RowIndex = 1;
            resources.ApplyResources(this.ndtFile, "ndtFile");
            // 
            // mnuExcel
            // 
            this.mnuExcel.Properties.ImageIndex = 4;
            this.mnuExcel.Properties.TooltipText = resources.GetString("resource.TooltipText");
            this.mnuExcel.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.mnuExcel_Click);
            // 
            // cmdImportExcel
            // 
            this.cmdImportExcel.Properties.ImageIndex = 5;
            this.cmdImportExcel.Properties.TooltipText = resources.GetString("resource.TooltipText1");
            this.cmdImportExcel.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdImportExcel_Click);
            // 
            // cmdNewChart
            // 
            this.cmdNewChart.Properties.ImageIndex = 8;
            this.cmdNewChart.Properties.TooltipText = resources.GetString("resource.TooltipText2");
            this.cmdNewChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdNewChart_Click);
            // 
            // cmdAlert
            // 
            this.cmdAlert.Properties.ImageIndex = 9;
            this.cmdAlert.Properties.TooltipText = resources.GetString("resource.TooltipText3");
            this.cmdAlert.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdAlert_Click);
            // 
            // cmdLoadWorkspace
            // 
            this.cmdLoadWorkspace.Properties.BeginGroup = true;
            this.cmdLoadWorkspace.Properties.ImageIndex = 2;
            this.cmdLoadWorkspace.Properties.TooltipText = resources.GetString("resource.TooltipText4");
            this.cmdLoadWorkspace.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdLoadWorkspace_Click);
            // 
            // cmdSaveWorkspace
            // 
            this.cmdSaveWorkspace.Properties.ImageIndex = 1;
            this.cmdSaveWorkspace.Properties.TooltipText = resources.GetString("resource.TooltipText5");
            this.cmdSaveWorkspace.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdSaveWorkspace_Click);
            // 
            // cmdPrintChart
            // 
            this.cmdPrintChart.Properties.BeginGroup = true;
            this.cmdPrintChart.Properties.ImageIndex = 0;
            this.cmdPrintChart.Properties.TooltipText = resources.GetString("resource.TooltipText6");
            this.cmdPrintChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdPrintChart_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Properties.BeginGroup = true;
            this.cmdHelp.Properties.ImageIndex = 7;
            this.cmdHelp.Properties.TooltipText = resources.GetString("resource.TooltipText7");
            this.cmdHelp.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdHelp_Click);
            // 
            // ndtView
            // 
            this.ndtView.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.nCommand32,
            this.cbxApplicationStyle,
            this.cmdUseSemiLogScale,
            this.cmdShowXGrid,
            this.cmdShowYGrid,
            this.cmdShowPanelSeparators,
            this.cmdThreeDStyle,
            this.cmdDarvasBoxes,
            this.cmdViewStarPage,
            this.cmdViewForexScreen});
            this.ndtView.DefaultLocation = new System.Drawing.Point(214, 53);
            this.ndtView.ImageList = this.m_ImgList4;
            this.ndtView.Name = "ndtView";
            this.ndtView.PrefferedRowIndex = 2;
            this.ndtView.RowIndex = 2;
            resources.ApplyResources(this.ndtView, "ndtView");
            // 
            // nCommand32
            // 
            this.nCommand32.Properties.Selectable = false;
            this.nCommand32.Properties.Style = Nevron.UI.WinForm.Controls.CommandStyle.Text;
            this.nCommand32.Properties.Text = resources.GetString("resource.Text104");
            this.nCommand32.Properties.TooltipHeading = false;
            this.nCommand32.Properties.TooltipImage = false;
            this.nCommand32.Properties.TooltipShortcut = false;
            // 
            // cbxApplicationStyle
            // 
            this.cbxApplicationStyle.ControlText = "";
            this.cbxApplicationStyle.PrefferedWidth = 140;
            this.cbxApplicationStyle.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.cbxApplicationStyle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CbxApplicationStyleClick);
            // 
            // cmdUseSemiLogScale
            // 
            this.cmdUseSemiLogScale.Properties.BeginGroup = true;
            this.cmdUseSemiLogScale.Properties.ImageIndex = 7;
            this.cmdUseSemiLogScale.Properties.TooltipText = resources.GetString("resource.TooltipText8");
            this.cmdUseSemiLogScale.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdUseSemiLogScaleClick);
            // 
            // cmdShowXGrid
            // 
            this.cmdShowXGrid.Properties.ImageIndex = 7;
            this.cmdShowXGrid.Properties.TooltipText = resources.GetString("resource.TooltipText9");
            this.cmdShowXGrid.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdShowXGridClick);
            // 
            // cmdShowYGrid
            // 
            this.cmdShowYGrid.Properties.ImageIndex = 7;
            this.cmdShowYGrid.Properties.TooltipText = resources.GetString("resource.TooltipText10");
            this.cmdShowYGrid.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdShowYGridClick);
            // 
            // cmdShowPanelSeparators
            // 
            this.cmdShowPanelSeparators.Properties.ImageIndex = 7;
            this.cmdShowPanelSeparators.Properties.TooltipText = resources.GetString("resource.TooltipText11");
            this.cmdShowPanelSeparators.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdShowPanelSeparatorsClick);
            // 
            // cmdThreeDStyle
            // 
            this.cmdThreeDStyle.Properties.BeginGroup = true;
            this.cmdThreeDStyle.Properties.ImageIndex = 7;
            this.cmdThreeDStyle.Properties.TooltipText = resources.GetString("resource.TooltipText12");
            this.cmdThreeDStyle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdThreeDStyleClick);
            // 
            // cmdDarvasBoxes
            // 
            this.cmdDarvasBoxes.Properties.ImageIndex = 7;
            this.cmdDarvasBoxes.Properties.TooltipText = resources.GetString("resource.TooltipText13");
            this.cmdDarvasBoxes.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdDarvasBoxesClick);
            // 
            // cmdViewStarPage
            // 
            this.cmdViewStarPage.Properties.BeginGroup = true;
            this.cmdViewStarPage.Properties.ImageIndex = 7;
            this.cmdViewStarPage.Properties.TooltipText = resources.GetString("resource.TooltipText14");
            this.cmdViewStarPage.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdViewStarPageClick);
            // 
            // cmdViewForexScreen
            // 
            this.cmdViewForexScreen.Properties.BeginGroup = true;
            this.cmdViewForexScreen.Properties.ImageIndex = 7;
            this.cmdViewForexScreen.Properties.TooltipText = resources.GetString("resource.TooltipText15");
            this.cmdViewForexScreen.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdViewForexScreenClick);
            // 
            // ndtZoom
            // 
            this.ndtZoom.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.cmdZoomIn,
            this.cmdZoomOut,
            this.cmdZoomArea,
            this.cmdZoomZero,
            this.cmdScrollLeft,
            this.cmdScrollRight,
            this.cmdTemplate,
            this.cmdDelete});
            this.ndtZoom.DefaultLocation = new System.Drawing.Point(706, 80);
            this.ndtZoom.ImageList = this.m_ImgList4;
            this.ndtZoom.Name = "ndtZoom";
            this.ndtZoom.PrefferedRowIndex = 3;
            this.ndtZoom.RowIndex = 3;
            resources.ApplyResources(this.ndtZoom, "ndtZoom");
            // 
            // cmdZoomIn
            // 
            this.cmdZoomIn.Properties.ImageIndex = 0;
            this.cmdZoomIn.Properties.TooltipText = resources.GetString("resource.TooltipText16");
            this.cmdZoomIn.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdZoomIn_Click);
            // 
            // cmdZoomOut
            // 
            this.cmdZoomOut.Properties.ImageIndex = 1;
            this.cmdZoomOut.Properties.TooltipText = resources.GetString("resource.TooltipText17");
            this.cmdZoomOut.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdZoomOut_Click);
            // 
            // cmdZoomArea
            // 
            this.cmdZoomArea.Properties.ImageIndex = 7;
            // 
            // cmdZoomZero
            // 
            this.cmdZoomZero.Properties.ImageIndex = 7;
            // 
            // cmdScrollLeft
            // 
            this.cmdScrollLeft.Properties.BeginGroup = true;
            this.cmdScrollLeft.Properties.ImageIndex = 2;
            this.cmdScrollLeft.Properties.TooltipText = resources.GetString("resource.TooltipText18");
            this.cmdScrollLeft.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdScrollLeft_Click);
            // 
            // cmdScrollRight
            // 
            this.cmdScrollRight.Properties.ImageIndex = 3;
            this.cmdScrollRight.Properties.TooltipText = resources.GetString("resource.TooltipText19");
            this.cmdScrollRight.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdScrollRight_Click);
            // 
            // cmdTemplate
            // 
            this.cmdTemplate.Properties.BeginGroup = true;
            this.cmdTemplate.Properties.ImageIndex = 4;
            this.cmdTemplate.Properties.TooltipText = resources.GetString("resource.TooltipText20");
            this.cmdTemplate.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdTemplate_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Properties.BeginGroup = true;
            this.cmdDelete.Properties.ImageIndex = 7;
            this.cmdDelete.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdDelete_Click);
            // 
            // ndtCalculator
            // 
            this.ndtCalculator.Controls.Add(this.cmdCalculator);
            this.ndtCalculator.DefaultLocation = new System.Drawing.Point(0, 107);
            this.ndtCalculator.MinimumSize = new System.Drawing.Size(132, 0);
            this.ndtCalculator.Name = "ndtCalculator";
            this.ndtCalculator.PrefferedRowIndex = 4;
            this.ndtCalculator.RowIndex = 4;
            resources.ApplyResources(this.ndtCalculator, "ndtCalculator");
            // 
            // cmdCalculator
            // 
            this.cmdCalculator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            // 
            // 
            // 
            this.cmdCalculator.Calculator.BackColor = System.Drawing.Color.Transparent;
            this.cmdCalculator.Calculator.Location = ((System.Drawing.Point)(resources.GetObject("cmdCalculator.Calculator.Location")));
            this.cmdCalculator.Calculator.Name = "";
            this.cmdCalculator.Calculator.ShowDisplay = false;
            this.cmdCalculator.Calculator.TabIndex = ((int)(resources.GetObject("cmdCalculator.Calculator.TabIndex")));
            this.cmdCalculator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdCalculator.Label.ContentAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCalculator.Label.Padding = new Nevron.UI.NPadding(2, 2, 2, 4);
            this.cmdCalculator.Label.Style.FontInfo = new Nevron.UI.NThemeFontInfo("Microsoft Sans Serif", 8.25F, Nevron.GraphicsCore.FontStyleEx.Regular);
            this.cmdCalculator.Label.Style.TextRenderMode = Nevron.UI.TextRenderMode.Gdi;
            this.cmdCalculator.Label.Text = resources.GetString("cmdCalculator.Label.Text");
            resources.ApplyResources(this.cmdCalculator, "cmdCalculator");
            this.cmdCalculator.Name = "cmdCalculator";
            // 
            // 
            // 
            this.cmdCalculator.Popup.ActivateOnDisplay = true;
            this.cmdCalculator.Popup.HostedControl = this.cmdCalculator.Calculator;
            this.cmdCalculator.Popup.Location = new System.Drawing.Point(0, 0);
            this.cmdCalculator.Popup.Padding = new Nevron.UI.NPadding(2);
            this.cmdCalculator.Popup.Size = new Nevron.GraphicsCore.NSize(240, 220);
            this.cmdCalculator.Popup.SizeStyle = Nevron.UI.PopupSizeStyle.BottomRight;
            // 
            // ndtPriceIndicators
            // 
            this.ndtPriceIndicators.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.nCommand1,
            this.cboPriceStyles,
            this.cmdCandleChart,
            this.cmdStockLine,
            this.cmdBarChart,
            this.cmdHeikinAshi});
            this.ndtPriceIndicators.DefaultLocation = new System.Drawing.Point(485, 132);
            this.ndtPriceIndicators.ImageList = this.m_ImgList4;
            this.ndtPriceIndicators.Name = "ndtPriceIndicators";
            this.ndtPriceIndicators.PrefferedRowIndex = 5;
            this.ndtPriceIndicators.RowIndex = 5;
            resources.ApplyResources(this.ndtPriceIndicators, "ndtPriceIndicators");
            // 
            // nCommand1
            // 
            this.nCommand1.Properties.Selectable = false;
            this.nCommand1.Properties.Style = Nevron.UI.WinForm.Controls.CommandStyle.Text;
            this.nCommand1.Properties.Text = resources.GetString("resource.Text105");
            this.nCommand1.Properties.TooltipHeading = false;
            this.nCommand1.Properties.TooltipImage = false;
            this.nCommand1.Properties.TooltipShortcut = false;
            // 
            // cboPriceStyles
            // 
            this.cboPriceStyles.ControlText = "";
            this.cboPriceStyles.ListProperties.ColumnOnLeft = false;
            this.cboPriceStyles.PrefferedWidth = 165;
            this.cboPriceStyles.Properties.ImageList = this.m_ImgList3;
            this.cboPriceStyles.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.cboPriceStyles.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cboPriceStyles_Click);
            // 
            // cmdCandleChart
            // 
            this.cmdCandleChart.Properties.BeginGroup = true;
            this.cmdCandleChart.Properties.ImageIndex = 7;
            this.cmdCandleChart.Properties.TooltipText = resources.GetString("resource.TooltipText21");
            this.cmdCandleChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdCandleChartClick);
            // 
            // cmdStockLine
            // 
            this.cmdStockLine.Properties.ImageIndex = 7;
            this.cmdStockLine.Properties.TooltipText = resources.GetString("resource.TooltipText22");
            this.cmdStockLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdStockLineClick);
            // 
            // cmdBarChart
            // 
            this.cmdBarChart.Properties.ImageIndex = 7;
            this.cmdBarChart.Properties.TooltipText = resources.GetString("resource.TooltipText23");
            this.cmdBarChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdBarChartClick);
            // 
            // cmdHeikinAshi
            // 
            this.cmdHeikinAshi.Properties.ImageIndex = 7;
            this.cmdHeikinAshi.Properties.TooltipText = resources.GetString("resource.TooltipText24");
            this.cmdHeikinAshi.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdHeikinAshiClick);
            // 
            // ndtChartTools
            // 
            this.ndtChartTools.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.cmdChart,
            this.cmdCrosshair,
            this.cmdSelect,
            this.cmdDeltaCursor,
            this.cmdMagnetic,
            this.cmdTextObject,
            this.cmdBuySymbol,
            this.cmdSellSymbol,
            this.cmdExitSymbol,
            this.cmdTrendLine,
            this.cmdRay,
            this.cmdChannel,
            this.cmdHorizontalLine,
            this.cmdVerticalLine,
            this.cmdRectangle,
            this.cmdElipse,
            this.cmdArrow,
            this.cmdPolyline,
            this.cmdFreeHandDrawing,
            this.cmdFibonacciRetracements,
            this.cmdFibonacciProjections,
            this.cmdFibonacciArcs,
            this.cmdFibonacciFan,
            this.cmdFibonacciTimeZones,
            this.cmdGannFan,
            this.cmdSpeedLines});
            this.ndtChartTools.DefaultLocation = new System.Drawing.Point(0, 1);
            resources.ApplyResources(this.ndtChartTools, "ndtChartTools");
            this.ndtChartTools.ImageList = this.m_ImgList4;
            this.ndtChartTools.Name = "ndtChartTools";
            this.ndtChartTools.PrefferedRowIndex = 0;
            this.ndtChartTools.RowIndex = 0;
            // 
            // cmdChart
            // 
            this.cmdChart.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdChart.Properties.ImageInfo.Image = global::M4.Properties.Resources._16_NewChart;
            this.cmdChart.Properties.TooltipText = resources.GetString("resource.TooltipText25");
            this.cmdChart.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdChartClick);
            // 
            // cmdCrosshair
            // 
            this.cmdCrosshair.Properties.BeginGroup = true;
            this.cmdCrosshair.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdCrosshair.Properties.ImageInfo.Image = global::M4.Properties.Resources._17_Crosshair;
            this.cmdCrosshair.Properties.TooltipText = resources.GetString("resource.TooltipText26");
            this.cmdCrosshair.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdCrosshairClick);
            // 
            // cmdSelect
            // 
            this.cmdSelect.Checked = true;
            this.cmdSelect.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdSelect.Properties.ImageInfo.Image = global::M4.Properties.Resources._18_Select;
            this.cmdSelect.Properties.TooltipText = resources.GetString("resource.TooltipText27");
            this.cmdSelect.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdSelectClick);
            // 
            // cmdDeltaCursor
            // 
            this.cmdDeltaCursor.Properties.ImageIndex = 4;
            this.cmdDeltaCursor.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cmdDeltaCursor_Click);
            // 
            // cmdMagnetic
            // 
            this.cmdMagnetic.Properties.BeginGroup = true;
            this.cmdMagnetic.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdMagnetic.Properties.ImageInfo.Image = global::M4.Properties.Resources._19_Magnetic;
            this.cmdMagnetic.Properties.TooltipText = resources.GetString("resource.TooltipText28");
            this.cmdMagnetic.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdMagneticClick);
            // 
            // cmdTextObject
            // 
            this.cmdTextObject.Properties.BeginGroup = true;
            this.cmdTextObject.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdTextObject.Properties.ImageInfo.Image = global::M4.Properties.Resources._20_Text;
            this.cmdTextObject.Properties.Text = resources.GetString("resource.Text106");
            this.cmdTextObject.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdTextObjectClick);
            // 
            // cmdBuySymbol
            // 
            this.cmdBuySymbol.Properties.BeginGroup = true;
            this.cmdBuySymbol.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdBuySymbol.Properties.ImageInfo.Image = global::M4.Properties.Resources._21_BuySymbol;
            this.cmdBuySymbol.Properties.Text = resources.GetString("resource.Text107");
            this.cmdBuySymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdBuySymbolClick);
            // 
            // cmdSellSymbol
            // 
            this.cmdSellSymbol.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdSellSymbol.Properties.ImageInfo.Image = global::M4.Properties.Resources._22_SellSymbol;
            this.cmdSellSymbol.Properties.Text = resources.GetString("resource.Text108");
            this.cmdSellSymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdSellSymbolClick);
            // 
            // cmdExitSymbol
            // 
            this.cmdExitSymbol.Properties.ImageInfo.Image = global::M4.Properties.Resources.exit;
            this.cmdExitSymbol.Properties.Text = resources.GetString("resource.Text109");
            this.cmdExitSymbol.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdExitSymbolClick);
            // 
            // cmdTrendLine
            // 
            this.cmdTrendLine.Properties.BeginGroup = true;
            this.cmdTrendLine.Properties.ImageInfo.Image = global::M4.Properties.Resources._24_TrendLine;
            this.cmdTrendLine.Properties.Text = resources.GetString("resource.Text110");
            this.cmdTrendLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdTrendLineClick);
            // 
            // cmdRay
            // 
            this.cmdRay.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdRay.Properties.ImageInfo.Image = global::M4.Properties.Resources._25_Ray;
            this.cmdRay.Properties.Text = resources.GetString("resource.Text111");
            this.cmdRay.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdRayClick);
            // 
            // cmdChannel
            // 
            this.cmdChannel.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdChannel.Properties.ImageInfo.Image = global::M4.Properties.Resources._26_Channel;
            this.cmdChannel.Properties.Text = resources.GetString("resource.Text112");
            this.cmdChannel.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdChannelClick);
            // 
            // cmdHorizontalLine
            // 
            this.cmdHorizontalLine.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdHorizontalLine.Properties.ImageInfo.Image = global::M4.Properties.Resources._27_HorizontalLine;
            this.cmdHorizontalLine.Properties.Text = resources.GetString("resource.Text113");
            this.cmdHorizontalLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdHorizontalLineClick);
            // 
            // cmdVerticalLine
            // 
            this.cmdVerticalLine.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdVerticalLine.Properties.ImageInfo.Image = global::M4.Properties.Resources._28_VerticalLine;
            this.cmdVerticalLine.Properties.Text = resources.GetString("resource.Text114");
            this.cmdVerticalLine.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdVerticalLineClick);
            // 
            // cmdRectangle
            // 
            this.cmdRectangle.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdRectangle.Properties.ImageInfo.Image = global::M4.Properties.Resources._29_Rectangle;
            this.cmdRectangle.Properties.Text = resources.GetString("resource.Text115");
            this.cmdRectangle.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdRectangleClick);
            // 
            // cmdElipse
            // 
            this.cmdElipse.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdElipse.Properties.ImageInfo.Image = global::M4.Properties.Resources._30_Ellipse;
            this.cmdElipse.Properties.Text = resources.GetString("resource.Text116");
            this.cmdElipse.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdElipseClick);
            // 
            // cmdArrow
            // 
            this.cmdArrow.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdArrow.Properties.ImageInfo.Image = global::M4.Properties.Resources._31_Arrow;
            this.cmdArrow.Properties.Text = resources.GetString("resource.Text117");
            this.cmdArrow.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdArrowClick);
            // 
            // cmdPolyline
            // 
            this.cmdPolyline.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdPolyline.Properties.ImageInfo.Image = global::M4.Properties.Resources._32_Polyline;
            this.cmdPolyline.Properties.Text = resources.GetString("resource.Text118");
            this.cmdPolyline.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdPolylineClick);
            // 
            // cmdFreeHandDrawing
            // 
            this.cmdFreeHandDrawing.Properties.ImageIndex = 7;
            this.cmdFreeHandDrawing.Properties.Text = resources.GetString("resource.Text119");
            this.cmdFreeHandDrawing.Properties.Visible = false;
            this.cmdFreeHandDrawing.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFreeHandDrawingClick);
            // 
            // cmdFibonacciRetracements
            // 
            this.cmdFibonacciRetracements.Properties.BeginGroup = true;
            this.cmdFibonacciRetracements.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdFibonacciRetracements.Properties.ImageInfo.Image = global::M4.Properties.Resources._34_FibonacciRetracements;
            this.cmdFibonacciRetracements.Properties.Text = resources.GetString("resource.Text120");
            this.cmdFibonacciRetracements.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFibonacciRetracementsClick);
            // 
            // cmdFibonacciProjections
            // 
            this.cmdFibonacciProjections.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdFibonacciProjections.Properties.ImageInfo.Image = global::M4.Properties.Resources._35_FibonacciProjecttions;
            this.cmdFibonacciProjections.Properties.Text = resources.GetString("resource.Text121");
            this.cmdFibonacciProjections.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFibonacciProjectionsClick);
            // 
            // cmdFibonacciArcs
            // 
            this.cmdFibonacciArcs.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdFibonacciArcs.Properties.ImageInfo.Image = global::M4.Properties.Resources._36_FibonacciArcs;
            this.cmdFibonacciArcs.Properties.Text = resources.GetString("resource.Text122");
            this.cmdFibonacciArcs.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFibonacciArcsClick);
            // 
            // cmdFibonacciFan
            // 
            this.cmdFibonacciFan.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdFibonacciFan.Properties.ImageInfo.Image = global::M4.Properties.Resources._37_FibonacciFan;
            this.cmdFibonacciFan.Properties.Text = resources.GetString("resource.Text123");
            this.cmdFibonacciFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFibonacciFanClick);
            // 
            // cmdFibonacciTimeZones
            // 
            this.cmdFibonacciTimeZones.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdFibonacciTimeZones.Properties.ImageInfo.Image = global::M4.Properties.Resources._38_FibonacciTimeZones;
            this.cmdFibonacciTimeZones.Properties.Text = resources.GetString("resource.Text124");
            this.cmdFibonacciTimeZones.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdFibonacciTimeZonesClick);
            // 
            // cmdGannFan
            // 
            this.cmdGannFan.Properties.BeginGroup = true;
            this.cmdGannFan.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdGannFan.Properties.ImageInfo.Image = global::M4.Properties.Resources._39_GannFan;
            this.cmdGannFan.Properties.Text = resources.GetString("resource.Text125");
            this.cmdGannFan.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdGannFanClick);
            // 
            // cmdSpeedLines
            // 
            this.cmdSpeedLines.Properties.ImageInfo.DrawMode = Nevron.UI.WinForm.Controls.ImageDrawMode.Center;
            this.cmdSpeedLines.Properties.ImageInfo.Image = global::M4.Properties.Resources._40_SpeedLines;
            this.cmdSpeedLines.Properties.Text = resources.GetString("resource.Text126");
            this.cmdSpeedLines.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.CmdSpeedLinesClick);
            // 
            // ndtTechnicalAnalysis
            // 
            this.ndtTechnicalAnalysis.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.nCommand3,
            this.cboIndicators});
            this.ndtTechnicalAnalysis.DefaultLocation = new System.Drawing.Point(132, 159);
            this.ndtTechnicalAnalysis.ImageList = this.m_ImgList4;
            this.ndtTechnicalAnalysis.Name = "ndtTechnicalAnalysis";
            this.ndtTechnicalAnalysis.PrefferedRowIndex = 6;
            this.ndtTechnicalAnalysis.RowIndex = 6;
            resources.ApplyResources(this.ndtTechnicalAnalysis, "ndtTechnicalAnalysis");
            // 
            // nCommand3
            // 
            this.nCommand3.Properties.Selectable = false;
            this.nCommand3.Properties.Style = Nevron.UI.WinForm.Controls.CommandStyle.Text;
            this.nCommand3.Properties.Text = resources.GetString("resource.Text127");
            this.nCommand3.Properties.TooltipHeading = false;
            this.nCommand3.Properties.TooltipImage = false;
            this.nCommand3.Properties.TooltipShortcut = false;
            // 
            // cboIndicators
            // 
            this.cboIndicators.ControlText = "";
            this.cboIndicators.PrefferedWidth = 220;
            this.cboIndicators.Properties.ImageList = this.m_ImgList3;
            this.cboIndicators.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            this.cboIndicators.Click += new Nevron.UI.WinForm.Controls.CommandEventHandler(this.cboIndicators_Click);
            // 
            // nStatusBar1
            // 
            resources.ApplyResources(this.nStatusBar1, "nStatusBar1");
            this.nStatusBar1.Name = "nStatusBar1";
            this.nStatusBar1.Panels.AddRange(new Nevron.UI.WinForm.Controls.NStatusBarPanel[] {
            this.nStatusBarPanel1,
            this.nStatusBarPanel2});
            this.nStatusBar1.ShowPanels = true;
            // 
            // nStatusBarPanel1
            // 
            this.nStatusBarPanel1.BorderStyle = Nevron.UI.BorderStyle3D.None;
            resources.ApplyResources(this.nStatusBarPanel1, "nStatusBarPanel1");
            // 
            // nStatusBarPanel2
            // 
            resources.ApplyResources(this.nStatusBarPanel2, "nStatusBarPanel2");
            this.nStatusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.nStatusBarPanel2.BorderStyle = Nevron.UI.BorderStyle3D.None;
            // 
            // nRichTextLabel1
            // 
            this.nRichTextLabel1.BackColor = System.Drawing.Color.Transparent;
            this.nRichTextLabel1.FillInfo.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nRichTextLabel1.FillInfo.FillStyle = Nevron.UI.WinForm.Controls.FillStyle.Solid;
            this.nRichTextLabel1.Item.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter;
            resources.ApplyResources(this.nRichTextLabel1, "nRichTextLabel1");
            this.nRichTextLabel1.Name = "nRichTextLabel1";
            this.nRichTextLabel1.ShadowInfo.Draw = false;
            this.nRichTextLabel1.SizeToContent = true;
            this.nRichTextLabel1.StrokeInfo.PenWidth = 3;
            this.nRichTextLabel1.StrokeInfo.Rounding = 5;
            // 
            // m_DockManager
            // 
            this.m_DockManager.Form = this;
            this.m_DockManager.GroupBorderStyle = Nevron.UI.BorderStyle3D.Flat;
            this.m_DockManager.Palette.Scheme = Nevron.UI.WinForm.Controls.ColorScheme.GrayScale;
            this.m_DockManager.RootContainerZIndex = 0;
            this.m_DockManager.UndockToleranceSize = 2;
            //  
            // Root Zone
            //  
            this.m_DockManager.RootContainer.RootZone.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // cmdCalcHost
            // 
            this.cmdCalcHost.PrefferedHeight = 20;
            this.cmdCalcHost.PrefferedWidth = 100;
            this.cmdCalcHost.Properties.ShowArrowStyle = Nevron.UI.WinForm.Controls.ShowArrowStyle.Never;
            // 
            // nCommand2
            // 
            this.nCommand2.Properties.Text = resources.GetString("resource.Text128");
            // 
            // axStockChartX1
            // 
            resources.ApplyResources(this.axStockChartX1, "axStockChartX1");
            this.axStockChartX1.Name = "axStockChartX1";
            this.axStockChartX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axStockChartX1.OcxState")));
            // 
            // tmrMessageService
            // 
            this.tmrMessageService.Tick += new System.EventHandler(this.tmrMessageService_Tick);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axStockChartX1);
            this.Controls.Add(this.nStatusBar1);
            this.Name = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_CmdBarsManager)).EndInit();
            this.ndtCalculator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nStatusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nStatusBarPanel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nRichTextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_DockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ImageList m_ImgList;
        internal System.Windows.Forms.ImageList m_ImgList2;
        internal System.Windows.Forms.ImageList m_ImgList3;
        internal System.Windows.Forms.ImageList m_ImgList4;
        internal System.Windows.Forms.Timer tmrClearAlerts;
        internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private Nevron.UI.WinForm.Controls.NCommandBarsManager m_CmdBarsManager;
        private Nevron.UI.WinForm.Controls.NMenuBar m_MenuBar;
        private Nevron.UI.WinForm.Controls.NCommand mnuFile;
        private Nevron.UI.WinForm.Controls.NCommand mnuView;
        private Nevron.UI.WinForm.Controls.NCommand mnuData;
        public Nevron.UI.WinForm.Controls.NCommand mnuChart;
        public Nevron.UI.WinForm.Controls.NCommand mnuTools;
        private Nevron.UI.WinForm.Controls.NCommand mnuHelp;
        private Nevron.UI.WinForm.Controls.NCommand mnuFileLoadWorkspace;
        public Nevron.UI.WinForm.Controls.NCommand mnuFileImportExcel;
        private Nevron.UI.WinForm.Controls.NCommand mnuFileSymbol;
        public Nevron.UI.WinForm.Controls.NCommand mnuFileExport;
        public Nevron.UI.WinForm.Controls.NCommand mnuFilePrint;
        private Nevron.UI.WinForm.Controls.NCommand mnuFileExit;
        public Nevron.UI.WinForm.Controls.NCommand mnuColors;
        private Nevron.UI.WinForm.Controls.NCommand mnuForeColor;
        private Nevron.UI.WinForm.Controls.NCommand mnuBackColor;
        private Nevron.UI.WinForm.Controls.NCommand mnuGridColor;
        private Nevron.UI.WinForm.Controls.NCommand mnuUpColor;
        public Nevron.UI.WinForm.Controls.NCommand mnuAppStyle;
        public Nevron.UI.WinForm.Controls.NCommand mnuViewScaleType;
        public Nevron.UI.WinForm.Controls.NCommand mnuViewShowXGrid;
        public Nevron.UI.WinForm.Controls.NCommand mnuViewYGrid;
        public Nevron.UI.WinForm.Controls.NCommand mnuViewSeparators;
        public Nevron.UI.WinForm.Controls.NCommand mnuView3D;
        public Nevron.UI.WinForm.Controls.NCommand mnuDarvasBoxes;
        private Nevron.UI.WinForm.Controls.NCommand mnuStartPage;
        private Nevron.UI.WinForm.Controls.NCommand mnuViewForex;
        private Nevron.UI.WinForm.Controls.NCommand mnuDownColor;
        private Nevron.UI.WinForm.Controls.NCommand mnuGradientTop;
        private Nevron.UI.WinForm.Controls.NCommand mnuGradientBottom;
        private Nevron.UI.WinForm.Controls.NCommand mnuAppOffice2007;
        private Nevron.UI.WinForm.Controls.NCommand mnuAppOfficeSilver;
        private Nevron.UI.WinForm.Controls.NCommand mnuAppStandard;
        private Nevron.UI.WinForm.Controls.NCommand mnuDataLogin;
        private Nevron.UI.WinForm.Controls.NCommand mnuCreateChart;
        private Nevron.UI.WinForm.Controls.NCommand mnuAlert;
        private Nevron.UI.WinForm.Controls.NCommand mnuBackTest;
        public Nevron.UI.WinForm.Controls.NCommand mnuZoomIn;
        public Nevron.UI.WinForm.Controls.NCommand mnuZoomOut;
        public Nevron.UI.WinForm.Controls.NCommand mnuScrollLeft;
        public Nevron.UI.WinForm.Controls.NCommand mnuScrollRight;
        public Nevron.UI.WinForm.Controls.NCommand mnuApplyTemplate;
        public Nevron.UI.WinForm.Controls.NCommand mnuPriceStyle;
        private Nevron.UI.WinForm.Controls.NCommand mnuText;
        private Nevron.UI.WinForm.Controls.NCommand mnuBuySymbol;
        private Nevron.UI.WinForm.Controls.NCommand mnuSellSymbol;
        private Nevron.UI.WinForm.Controls.NCommand mnuExitSymbol;
        private Nevron.UI.WinForm.Controls.NCommand mnuTrendLine;
        private Nevron.UI.WinForm.Controls.NCommand mnuEllipse;
        private Nevron.UI.WinForm.Controls.NCommand mnuSpeedLines;
        private Nevron.UI.WinForm.Controls.NCommand mnuGannFan;
        private Nevron.UI.WinForm.Controls.NCommand mnuFibonacciArcs;
        private Nevron.UI.WinForm.Controls.NCommand mnuFibonacciRetracements;
        private Nevron.UI.WinForm.Controls.NCommand mnuFibonacciFan;
        private Nevron.UI.WinForm.Controls.NCommand mnuFibonacciTimeZones;
        private Nevron.UI.WinForm.Controls.NCommand mnuTironeLevels;
        private Nevron.UI.WinForm.Controls.NCommand mnuQuadrantLines;
        private Nevron.UI.WinForm.Controls.NCommand mnuRaffRegression;
        private Nevron.UI.WinForm.Controls.NCommand mnuErrorChannels;
        private Nevron.UI.WinForm.Controls.NCommand mnuRectangle;
        private Nevron.UI.WinForm.Controls.NCommand mnuAbout;
        private Nevron.UI.WinForm.Controls.NStatusBar nStatusBar1;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtPriceIndicators;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtTechnicalAnalysis;
        private Nevron.UI.WinForm.Controls.NCommand nCommand1;
        public Nevron.UI.WinForm.Controls.NComboBoxCommand cboPriceStyles;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtZoom;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtCalculator;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtView;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtChartTools;
        public Nevron.UI.WinForm.Controls.NCommand cmdZoomIn;
        public Nevron.UI.WinForm.Controls.NCommand cmdZoomOut;
        public Nevron.UI.WinForm.Controls.NCommand cmdScrollLeft;
        public Nevron.UI.WinForm.Controls.NCommand cmdScrollRight;
        public Nevron.UI.WinForm.Controls.NCommand cmdTemplate;
        public Nevron.UI.WinForm.Controls.NCommand cmdDelete;
        private Nevron.UI.WinForm.Controls.NDockingToolbar ndtFile;
        public Nevron.UI.WinForm.Controls.NCommand mnuExcel;
        public Nevron.UI.WinForm.Controls.NCommand cmdImportExcel;
        private Nevron.UI.WinForm.Controls.NCommand cmdNewChart;
        private Nevron.UI.WinForm.Controls.NCommand cmdAlert;
        private Nevron.UI.WinForm.Controls.NCommand cmdLoadWorkspace;
        public Nevron.UI.WinForm.Controls.NCommand cmdPrintChart;
        private Nevron.UI.WinForm.Controls.NCommand cmdHelp;
        public Nevron.UI.WinForm.Controls.NCommand mnuFileSaveWorkspace;
        public Nevron.UI.WinForm.Controls.NCommand cmdSaveWorkspace;
        public Nevron.UI.WinForm.Controls.NCommand mnuFileSaveTemplate;
        public Nevron.UI.WinForm.Controls.NStatusBarPanel nStatusBarPanel1;
        private Nevron.UI.WinForm.Controls.NCommand mnuScanner;
        private Nevron.UI.WinForm.Controls.NCommand mnuFreehand;
        private Nevron.UI.WinForm.Controls.NStatusBarPanel nStatusBarPanel2;
        public NCommand mnuAI;
        private System.Windows.Forms.ImageList m_ImageList5;
        public NCommand mnuApplyExpertAdvisor;
        private NCommand mnuManageExpertAdvisors;
        private NCommand mnuCreateExpertAdvisor;
        private NRichTextLabel nRichTextLabel1;
        internal NDockManager m_DockManager;
        private NCommand mnuScriptHelp;
        public NCommand mnuConsensusReport;
        private NCalculatorDropDown cmdCalculator;
        public NCommand mnuNN;
        public NCommand mnuPatternRecognition;
        private NCommand mnuPopoutChart;
        public NCommand mnuFileImportCSV;
        public NCommand mnuFileExportCSV;
        public NCommand mnuViewCrosshair;
        private NCommand mnuArrow;
        private NCommand cmdCandleChart;
        private NCommand cmdStockLine;
        private NCommand cmdBarChart;
        private NCommand cmdHeikinAshi;
        private NControlHostCommand cmdCalcHost;
        private NComboBoxCommand cbxApplicationStyle;
        private NCommand cmdUseSemiLogScale;
        private NCommand cmdShowXGrid;
        private NCommand cmdShowYGrid;
        private NCommand cmdShowPanelSeparators;
        private NCommand cmdThreeDStyle;
        private NCommand cmdDarvasBoxes;
        private NCommand cmdViewStarPage;
        private NCommand cmdViewForexScreen;
        public NCommand cmdCrosshair;
        private NCommand nCommand32;
        private NCommand cmdZoomArea;
        private NCommand cmdZoomZero;
        private NCommand mnuToolbar;
        private NCommand mnuPriceIndicatorsToolbar;
        private NCommand mnuZoomTemplatesToolbar;
        private NCommand mnuViewToolbar;
        private NCommand mnuChartToolsToolbar;
        private NCommand mnuFileToolbar;
        private NCommand nCommand3;
        public NComboBoxCommand cboIndicators;
        private NCommand cmdSelect;
        public NCommand cmdMagnetic;
        private NCommand cmdTextObject;
        private NCommand cmdBuySymbol;
        private NCommand cmdSellSymbol;
        private NCommand cmdExitSymbol;
        private NCommand cmdTrendLine;
        private NCommand cmdRay;
        private NCommand cmdChannel;
        private NCommand cmdHorizontalLine;
        private NCommand cmdVerticalLine;
        private NCommand cmdRectangle;
        private NCommand cmdElipse;
        private NCommand cmdArrow;
        private NCommand cmdPolyline;
        private NCommand cmdFreeHandDrawing;
        private NCommand cmdFibonacciRetracements;
        private NCommand cmdFibonacciProjections;
        private NCommand cmdFibonacciArcs;
        private NCommand cmdFibonacciFan;
        private NCommand cmdFibonacciTimeZones;
        private NCommand cmdGannFan;
        private NCommand cmdSpeedLines;
        private NCommand mnuRay;
        private NCommand mnuChannel;
        private NCommand nCmdHorizontalLine;
        private NCommand nCmdVerticalLine;
        private NCommand nCmdRectangle;
        private NCommand nCmdElipse;
        private NCommand nCmdArrow;
        private NCommand mnuPolyline;
        private NCommand nCmdFreeHandDrawing;
        private NCommand nCmdFibonacciRetracements;
        private NCommand nCmdFibonacciProjections;
        private NCommand nCmdFibonacciArcs;
        private NCommand nCmdFibonacciFan;
        private NCommand nCmdFibonacciTimeZones;
        private NCommand nCmdGannFan;
        private NCommand nCmdSpeedLines;
        private NCommand cmdChart;
        public NCommand mnuWorkspace;
        private NCommand mnuManagerWorkspace;
        public NCommand mnuViewMagnetic;
        public NCommand nCommand2;
        private AxSTOCKCHARTXLib.AxStockChartX axStockChartX1;
        private NCommand mnuSettings;
        private NCommand nCommand4;
        private NCommand nCommand5;
        public NCommand mnuFileSaveImage;
        public NCommand cmdDeltaCursor;
        private System.Windows.Forms.Timer tmrMessageService;
    }
}

