using System.Drawing;
using System.Windows.Forms;
using M4.M4v2.GridviewRowDetailsExtended;
using Telerik.WinControls.UI.Docking;

namespace M4
{
    partial class frmMain2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (System.Exception) { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.Docking.AutoHideGroup autoHideGroup1 = new Telerik.WinControls.UI.Docking.AutoHideGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain2));
            this.dockWindowPlaceholder1 = new Telerik.WinControls.UI.Docking.DockWindowPlaceholder();
            this.mnuFile = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuFileSaveImage = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuFilePrint = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem2 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuExit = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuWorkspace = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuManagerWorkspace = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuOptions = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuView = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuToolBar = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuChartToolsToolbar = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuStatusManager = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuSelectTools = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuColors = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuForeColor = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuBackColor = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuGridColor = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuUpColor = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuDownColor = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuGradientTop = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuGradientBottom = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppStyle = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppAqua = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppBreeze = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppDesert = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppHighContrastBlack = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppOffice2007Black = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppOffice2007Silver = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppOffice2010Black = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppOffice2010Blue = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppOffice2010Silver = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppTelerikMetro = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppTelerikMetroBlue = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppVisualStudio2012Dark = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppVisualStudio2012Light = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppWindows7 = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAppWindows8 = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuChartColors = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem4 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuViewScaleType = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewShowXGrid = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewYGrid = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewSeparators = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewCrossHair = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewMagnetic = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuView3D = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuDarvasBoxes = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem5 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuStartPage = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuViewForex = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAlerts = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuScanner = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuBackTest = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuScript = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuChart = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuPopoutChart = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuHelp = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuScriptHelp = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuAbout = new Telerik.WinControls.UI.RadMenuItem();
            this.commandBarRowElement4 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.ndtFile = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButton1 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton2 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton3 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton4 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton5 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton6 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton7 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator3 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton8 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarRowElement2 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.ndtView = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarLabel1 = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbxApplicationStyle = new Telerik.WinControls.UI.CommandBarDropDownList();
            this.cmdUseSemiLogScale = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdShowXGrid = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdShowYGrid = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdShowPanelSeparators = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator4 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdThreeDStyle = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdDarvasBoxes = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator5 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdViewStarPage = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator6 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdViewForexScreen = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarStripElement2 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButton9 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton10 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton11 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton12 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator13 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton13 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator14 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton14 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator15 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton15 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton16 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton17 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator16 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton18 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton19 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton20 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator17 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton21 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton22 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton23 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton24 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton25 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator18 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton26 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton27 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton28 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton29 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarStripElement4 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButton30 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton31 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton32 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton33 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator19 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton34 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator20 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton35 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator21 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton36 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton37 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton38 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator22 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton39 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton40 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton41 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator23 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton42 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton43 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton44 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton45 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton46 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator24 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButton47 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton48 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton49 = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButton50 = new Telerik.WinControls.UI.CommandBarButton();
            this.radDock1 = new Telerik.WinControls.UI.Docking.RadDock();
            this.WorkspaceDefault = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.radDock2 = new Telerik.WinControls.UI.Docking.RadDock();
            this._selectview = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.toolTabStrip3 = new Telerik.WinControls.UI.Docking.ToolTabStrip();
            this.documentContainer2 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip2 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this._toolWeb = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.radCommandBar2 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement3 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.ndtChartTools = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cmdChart = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdCrosshair = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdSelect = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.cmdDeltaCursor = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator7 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdMagnetic = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator8 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdTextObject = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator9 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdBuySymbol = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdSellSymbol = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdExitSymbol = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator10 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdTrendLine = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdRay = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdChannel = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdHorizontalLine = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdVerticalLine = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdRectangle = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdEllipse = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdArrow = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdPolyline = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator12 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cmdFibonacciRetracements = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdFibonacciProjections = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdFibonacciArcs = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdFibonacciFan = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdGannFan = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdSpeedLines = new Telerik.WinControls.UI.CommandBarButton();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip1 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this._toolAlerts = new Telerik.WinControls.UI.Docking.ToolWindow();
            this._toolScanner = new Telerik.WinControls.UI.Docking.ToolWindow();
            this._toolBackTest = new Telerik.WinControls.UI.Docking.ToolWindow();
            this._toolScript = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.tmrMessageService = new System.Windows.Forms.Timer(this.components);
            this.tmrClearAlerts = new System.Windows.Forms.Timer(this.components);
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.m_MenuBar = new Telerik.WinControls.UI.RadMenu();
            this.commandBarRowElement5 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.radDesktopAlert1 = new Telerik.WinControls.UI.RadDesktopAlert(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).BeginInit();
            this.radDock1.SuspendLayout();
            this.WorkspaceDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDock2)).BeginInit();
            this.radDock2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip3)).BeginInit();
            this.toolTabStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer2)).BeginInit();
            this.documentContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip2)).BeginInit();
            this.documentTabStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            this.documentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).BeginInit();
            this.documentTabStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_MenuBar)).BeginInit();
            this.SuspendLayout();
            // 
            // dockWindowPlaceholder1
            // 
            this.dockWindowPlaceholder1.DockWindowName = "statusManager";
            this.dockWindowPlaceholder1.DockWindowText = "Status Manager";
            this.dockWindowPlaceholder1.Location = new System.Drawing.Point(0, 0);
            this.dockWindowPlaceholder1.Margin = new System.Windows.Forms.Padding(0);
            this.dockWindowPlaceholder1.Name = "dockWindowPlaceholder1";
            this.dockWindowPlaceholder1.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this.dockWindowPlaceholder1.Size = new System.Drawing.Size(200, 200);
            this.dockWindowPlaceholder1.Text = "dockWindowPlaceholder1";
            // 
            // mnuFile
            // 
            this.mnuFile.AccessibleDescription = "File";
            this.mnuFile.AccessibleName = "File";
            this.mnuFile.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuFileSaveImage,
            this.mnuFilePrint,
            this.radMenuSeparatorItem2,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Text = "File";
            this.mnuFile.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuFileSaveImage
            // 
            this.mnuFileSaveImage.AccessibleDescription = "Save Chart as Image";
            this.mnuFileSaveImage.AccessibleName = "Save Chart as Image";
            this.mnuFileSaveImage.Name = "mnuFileSaveImage";
            this.mnuFileSaveImage.Text = "Save Chart as Image";
            this.mnuFileSaveImage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuFileSaveImage.Click += new System.EventHandler(this.mnuFileSaveImage_Click);
            // 
            // mnuFilePrint
            // 
            this.mnuFilePrint.AccessibleDescription = "Print Chart";
            this.mnuFilePrint.AccessibleName = "Print Chart";
            this.mnuFilePrint.Image = null;
            this.mnuFilePrint.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.mnuFilePrint.Name = "mnuFilePrint";
            this.mnuFilePrint.Text = "Print Chart";
            this.mnuFilePrint.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuFilePrint.Click += new System.EventHandler(this.mnuFilePrint_Click);
            // 
            // radMenuSeparatorItem2
            // 
            this.radMenuSeparatorItem2.AccessibleDescription = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.AccessibleName = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.Name = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.Text = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuExit
            // 
            this.mnuExit.AccessibleDescription = "Exit";
            this.mnuExit.AccessibleName = "Exit";
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Text = "Exit";
            this.mnuExit.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuWorkspace
            // 
            this.mnuWorkspace.AccessibleDescription = "Workspace";
            this.mnuWorkspace.AccessibleName = "Workspace";
            this.mnuWorkspace.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuManagerWorkspace,
            this.radMenuSeparatorItem1});
            this.mnuWorkspace.Name = "mnuWorkspace";
            this.mnuWorkspace.Text = "Workspace";
            this.mnuWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuManagerWorkspace
            // 
            this.mnuManagerWorkspace.AccessibleDescription = "Manager";
            this.mnuManagerWorkspace.AccessibleName = "Manager";
            this.mnuManagerWorkspace.Name = "mnuManagerWorkspace";
            this.mnuManagerWorkspace.Text = "Manager";
            this.mnuManagerWorkspace.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuManagerWorkspace.Click += new System.EventHandler(this.mnuManagerWorkspace_Click);
            // 
            // radMenuSeparatorItem1
            // 
            this.radMenuSeparatorItem1.AccessibleDescription = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.AccessibleName = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuOptions
            // 
            this.mnuOptions.AccessibleDescription = "Options";
            this.mnuOptions.AccessibleName = "Options";
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Text = "Options";
            this.mnuOptions.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuView
            // 
            this.mnuView.AccessibleDescription = "View";
            this.mnuView.AccessibleName = "View";
            this.mnuView.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuToolBar,
            this.mnuColors,
            this.mnuAppStyle,
            this.mnuChartColors,
            this.radMenuSeparatorItem4,
            this.mnuViewScaleType,
            this.mnuViewShowXGrid,
            this.mnuViewYGrid,
            this.mnuViewSeparators,
            this.mnuViewCrossHair,
            this.mnuViewMagnetic,
            this.mnuView3D,
            this.mnuDarvasBoxes,
            this.radMenuSeparatorItem5,
            this.mnuStartPage,
            this.mnuViewForex,
            this.mnuAlerts,
            this.mnuScanner,
            this.mnuBackTest,
            this.mnuScript});
            this.mnuView.Name = "mnuView";
            this.mnuView.Text = "View";
            this.mnuView.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuToolBar
            // 
            this.mnuToolBar.AccessibleDescription = "Toolbar";
            this.mnuToolBar.AccessibleName = "Toolbar";
            this.mnuToolBar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuChartToolsToolbar,
            this.mnuStatusManager,
            this.mnuSelectTools});
            this.mnuToolBar.Name = "mnuToolBar";
            this.mnuToolBar.Text = "Toolbar";
            this.mnuToolBar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuChartToolsToolbar
            // 
            this.mnuChartToolsToolbar.AccessibleDescription = "Chart Tools";
            this.mnuChartToolsToolbar.AccessibleName = "Chart Tools";
            this.mnuChartToolsToolbar.Name = "mnuChartToolsToolbar";
            this.mnuChartToolsToolbar.Text = "Chart Tools";
            this.mnuChartToolsToolbar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuChartToolsToolbar.Click += new System.EventHandler(this.mnuChartToolsToolbar_Click);
            // 
            // mnuStatusManager
            // 
            this.mnuStatusManager.AccessibleDescription = "mnuStatusManager";
            this.mnuStatusManager.AccessibleName = "mnuStatusManager";
            this.mnuStatusManager.Name = "mnuStatusManager";
            this.mnuStatusManager.Text = "Status Manager";
            this.mnuStatusManager.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuStatusManager.Click += new System.EventHandler(this.mnuStatusManager_Click);
            // 
            // mnuSelectTools
            // 
            this.mnuSelectTools.AccessibleDescription = "radMenuItem2";
            this.mnuSelectTools.AccessibleName = "radMenuItem2";
            this.mnuSelectTools.Name = "mnuSelectTools";
            this.mnuSelectTools.Text = "Select Tools";
            this.mnuSelectTools.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuSelectTools.Click += new System.EventHandler(this.mnuSelectTools_Click);
            // 
            // mnuColors
            // 
            this.mnuColors.AccessibleDescription = "radMenuSeparatorItem3";
            this.mnuColors.AccessibleName = "radMenuSeparatorItem3";
            this.mnuColors.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuForeColor,
            this.mnuBackColor,
            this.mnuGridColor,
            this.mnuUpColor,
            this.mnuDownColor,
            this.mnuGradientTop,
            this.mnuGradientBottom});
            this.mnuColors.Name = "mnuColors";
            this.mnuColors.Text = "Chart Colors";
            this.mnuColors.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuForeColor
            // 
            this.mnuForeColor.AccessibleDescription = "Fore Color";
            this.mnuForeColor.AccessibleName = "Fore Color";
            this.mnuForeColor.Name = "mnuForeColor";
            this.mnuForeColor.Text = "Fore Color";
            this.mnuForeColor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuBackColor
            // 
            this.mnuBackColor.AccessibleDescription = "Back Color";
            this.mnuBackColor.AccessibleName = "Back Color";
            this.mnuBackColor.Name = "mnuBackColor";
            this.mnuBackColor.Text = "Back Color";
            this.mnuBackColor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuGridColor
            // 
            this.mnuGridColor.AccessibleDescription = "Grid Color";
            this.mnuGridColor.AccessibleName = "Grid Color";
            this.mnuGridColor.Name = "mnuGridColor";
            this.mnuGridColor.Text = "Grid Color";
            this.mnuGridColor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuUpColor
            // 
            this.mnuUpColor.AccessibleDescription = "Up Color";
            this.mnuUpColor.AccessibleName = "Up Color";
            this.mnuUpColor.Name = "mnuUpColor";
            this.mnuUpColor.Text = "Up Color";
            this.mnuUpColor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuDownColor
            // 
            this.mnuDownColor.AccessibleDescription = "Down Color";
            this.mnuDownColor.AccessibleName = "Down Color";
            this.mnuDownColor.Name = "mnuDownColor";
            this.mnuDownColor.Text = "Down Color";
            this.mnuDownColor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuGradientTop
            // 
            this.mnuGradientTop.AccessibleDescription = "Gradient Top Color";
            this.mnuGradientTop.AccessibleName = "Gradient Top Color";
            this.mnuGradientTop.Name = "mnuGradientTop";
            this.mnuGradientTop.Text = "Gradient Top Color";
            this.mnuGradientTop.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuGradientBottom
            // 
            this.mnuGradientBottom.AccessibleDescription = "Gradient Bottom Color";
            this.mnuGradientBottom.AccessibleName = "Gradient Bottom Color";
            this.mnuGradientBottom.Name = "mnuGradientBottom";
            this.mnuGradientBottom.Text = "Gradient Bottom Color";
            this.mnuGradientBottom.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuAppStyle
            // 
            this.mnuAppStyle.AccessibleDescription = "Application Style";
            this.mnuAppStyle.AccessibleName = "Application Style";
            this.mnuAppStyle.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuAppAqua,
            this.mnuAppBreeze,
            this.mnuAppDesert,
            this.mnuAppHighContrastBlack,
            this.mnuAppOffice2007Black,
            this.mnuAppOffice2007Silver,
            this.mnuAppOffice2010Black,
            this.mnuAppOffice2010Blue,
            this.mnuAppOffice2010Silver,
            this.mnuAppTelerikMetro,
            this.mnuAppTelerikMetroBlue,
            this.mnuAppVisualStudio2012Dark,
            this.mnuAppVisualStudio2012Light,
            this.mnuAppWindows7,
            this.mnuAppWindows8});
            this.mnuAppStyle.Name = "mnuAppStyle";
            this.mnuAppStyle.Text = "Application Style";
            this.mnuAppStyle.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuAppAqua
            // 
            this.mnuAppAqua.AccessibleDescription = "Aqua";
            this.mnuAppAqua.AccessibleName = "Aqua";
            this.mnuAppAqua.Name = "mnuAppAqua";
            this.mnuAppAqua.Text = "Aqua";
            this.mnuAppAqua.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppAqua.Click += new System.EventHandler(this.mnuAppAqua_Click);
            // 
            // mnuAppBreeze
            // 
            this.mnuAppBreeze.AccessibleDescription = "Breeze";
            this.mnuAppBreeze.AccessibleName = "Breeze";
            this.mnuAppBreeze.Name = "mnuAppBreeze";
            this.mnuAppBreeze.Text = "Breeze";
            this.mnuAppBreeze.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppBreeze.Click += new System.EventHandler(this.mnuAppBreeze_Click);
            // 
            // mnuAppDesert
            // 
            this.mnuAppDesert.AccessibleDescription = "Desert";
            this.mnuAppDesert.AccessibleName = "Desert";
            this.mnuAppDesert.Name = "mnuAppDesert";
            this.mnuAppDesert.Text = "Desert";
            this.mnuAppDesert.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppDesert.Click += new System.EventHandler(this.mnuAppDesert_Click);
            // 
            // mnuAppHighContrastBlack
            // 
            this.mnuAppHighContrastBlack.AccessibleDescription = "High Contrast Black";
            this.mnuAppHighContrastBlack.AccessibleName = "HighContrastBlack";
            this.mnuAppHighContrastBlack.Name = "mnuAppHighContrastBlack";
            this.mnuAppHighContrastBlack.Text = "High Contrast Black";
            this.mnuAppHighContrastBlack.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppHighContrastBlack.Click += new System.EventHandler(this.mnuAppHighContrastBlack_Click);
            // 
            // mnuAppOffice2007Black
            // 
            this.mnuAppOffice2007Black.AccessibleDescription = "Office2007Black";
            this.mnuAppOffice2007Black.AccessibleName = "Office2007Black";
            this.mnuAppOffice2007Black.Name = "mnuAppOffice2007Black";
            this.mnuAppOffice2007Black.Text = "Office 2007 Black";
            this.mnuAppOffice2007Black.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppOffice2007Black.Click += new System.EventHandler(this.mnuAppStandard_Click);
            // 
            // mnuAppOffice2007Silver
            // 
            this.mnuAppOffice2007Silver.AccessibleDescription = "Office 2007 Silver";
            this.mnuAppOffice2007Silver.AccessibleName = "Office2007Silver";
            this.mnuAppOffice2007Silver.Name = "mnuAppOffice2007Silver";
            this.mnuAppOffice2007Silver.Text = "Office 2007 Silver";
            this.mnuAppOffice2007Silver.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppOffice2007Silver.Click += new System.EventHandler(this.mnuAppOfficeSilver_Click);
            // 
            // mnuAppOffice2010Black
            // 
            this.mnuAppOffice2010Black.AccessibleDescription = "Office 2010 Black";
            this.mnuAppOffice2010Black.AccessibleName = "Office2010Black";
            this.mnuAppOffice2010Black.Name = "mnuAppOffice2010Black";
            this.mnuAppOffice2010Black.Text = "Office 2010 Black";
            this.mnuAppOffice2010Black.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppOffice2010Black.Click += new System.EventHandler(this.mnuAppOffice2010Black_Click);
            // 
            // mnuAppOffice2010Blue
            // 
            this.mnuAppOffice2010Blue.AccessibleDescription = "Office 2010 Blue";
            this.mnuAppOffice2010Blue.AccessibleName = "Office2010Blue";
            this.mnuAppOffice2010Blue.Name = "mnuAppOffice2010Blue";
            this.mnuAppOffice2010Blue.Text = "Office 2010 Blue";
            this.mnuAppOffice2010Blue.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppOffice2010Blue.Click += new System.EventHandler(this.mnuAppOffice2010Blue_Click);
            // 
            // mnuAppOffice2010Silver
            // 
            this.mnuAppOffice2010Silver.AccessibleDescription = "Office 2010 Silver";
            this.mnuAppOffice2010Silver.AccessibleName = "Office2010Silver";
            this.mnuAppOffice2010Silver.Name = "mnuAppOffice2010Silver";
            this.mnuAppOffice2010Silver.Text = "Office 2010 Silver";
            this.mnuAppOffice2010Silver.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppOffice2010Silver.Click += new System.EventHandler(this.mnuAppOffice2010Silver_Click);
            // 
            // mnuAppTelerikMetro
            // 
            this.mnuAppTelerikMetro.AccessibleDescription = "Telerik Metro";
            this.mnuAppTelerikMetro.AccessibleName = "TelerikMetro";
            this.mnuAppTelerikMetro.Name = "mnuAppTelerikMetro";
            this.mnuAppTelerikMetro.Text = "Telerik Metro";
            this.mnuAppTelerikMetro.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppTelerikMetro.Click += new System.EventHandler(this.mnuAppTelerikMetro_Click);
            // 
            // mnuAppTelerikMetroBlue
            // 
            this.mnuAppTelerikMetroBlue.AccessibleDescription = "Telerik Metro Blue";
            this.mnuAppTelerikMetroBlue.AccessibleName = "TelerikMetroBlue";
            this.mnuAppTelerikMetroBlue.Name = "mnuAppTelerikMetroBlue";
            this.mnuAppTelerikMetroBlue.Text = "Telerik Metro Blue";
            this.mnuAppTelerikMetroBlue.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppTelerikMetroBlue.Click += new System.EventHandler(this.mnuAppTelerikMetroBlue_Click);
            // 
            // mnuAppVisualStudio2012Dark
            // 
            this.mnuAppVisualStudio2012Dark.AccessibleDescription = "Visual Studio 2012 Dark";
            this.mnuAppVisualStudio2012Dark.AccessibleName = "Visual Studio 2012 Dark";
            this.mnuAppVisualStudio2012Dark.Name = "mnuAppVisualStudio2012Dark";
            this.mnuAppVisualStudio2012Dark.Text = "Visual Studio 2012 Dark";
            this.mnuAppVisualStudio2012Dark.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppVisualStudio2012Dark.Click += new System.EventHandler(this.mnuAppVisualStudio2012Dark_Click);
            // 
            // mnuAppVisualStudio2012Light
            // 
            this.mnuAppVisualStudio2012Light.AccessibleDescription = "Visual Studio 2012 Light";
            this.mnuAppVisualStudio2012Light.AccessibleName = "Visual Studio 2012 Light";
            this.mnuAppVisualStudio2012Light.Name = "mnuAppVisualStudio2012Light";
            this.mnuAppVisualStudio2012Light.Text = "Visual Studio 2012 Light";
            this.mnuAppVisualStudio2012Light.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppVisualStudio2012Light.Click += new System.EventHandler(this.mnuAppVisualStudio2012Light_Click);
            // 
            // mnuAppWindows7
            // 
            this.mnuAppWindows7.AccessibleDescription = "Windows7";
            this.mnuAppWindows7.AccessibleName = "Windows7";
            this.mnuAppWindows7.Name = "mnuAppWindows7";
            this.mnuAppWindows7.Text = "Windows7";
            this.mnuAppWindows7.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppWindows7.Click += new System.EventHandler(this.mnuAppWindows7_Click);
            // 
            // mnuAppWindows8
            // 
            this.mnuAppWindows8.AccessibleDescription = "Windows8 Theme";
            this.mnuAppWindows8.AccessibleName = "Windows8";
            this.mnuAppWindows8.Name = "mnuAppWindows8";
            this.mnuAppWindows8.Text = "Windows8";
            this.mnuAppWindows8.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAppWindows8.Click += new System.EventHandler(this.mnuWindows8Theme_Click);
            // 
            // mnuChartColors
            // 
            this.mnuChartColors.AccessibleDescription = "Color Scheme";
            this.mnuChartColors.AccessibleName = "Color Scheme";
            this.mnuChartColors.Name = "mnuChartColors";
            this.mnuChartColors.Text = "Color Scheme";
            this.mnuChartColors.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radMenuSeparatorItem4
            // 
            this.radMenuSeparatorItem4.AccessibleDescription = "radMenuSeparatorItem4";
            this.radMenuSeparatorItem4.AccessibleName = "radMenuSeparatorItem4";
            this.radMenuSeparatorItem4.Name = "radMenuSeparatorItem4";
            this.radMenuSeparatorItem4.Text = "radMenuSeparatorItem4";
            this.radMenuSeparatorItem4.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuViewScaleType
            // 
            this.mnuViewScaleType.AccessibleDescription = "Use Semi-log Scale";
            this.mnuViewScaleType.AccessibleName = "Use Semi-log Scale";
            this.mnuViewScaleType.CheckOnClick = true;
            this.mnuViewScaleType.Name = "mnuViewScaleType";
            this.mnuViewScaleType.Text = "Use Semi-log Scale";
            this.mnuViewScaleType.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuViewScaleType.Click += new System.EventHandler(this.mnuViewScaleType_Click);
            // 
            // mnuViewShowXGrid
            // 
            this.mnuViewShowXGrid.AccessibleDescription = "radMenuItem2";
            this.mnuViewShowXGrid.AccessibleName = "radMenuItem2";
            this.mnuViewShowXGrid.Name = "mnuViewShowXGrid";
            this.mnuViewShowXGrid.Text = "Show X Grid";
            this.mnuViewShowXGrid.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuViewShowXGrid.Click += new System.EventHandler(this.mnuViewShowXGrid_Click);
            // 
            // mnuViewYGrid
            // 
            this.mnuViewYGrid.AccessibleDescription = "Show Y Grid";
            this.mnuViewYGrid.AccessibleName = "Show Y Grid";
            this.mnuViewYGrid.Name = "mnuViewYGrid";
            this.mnuViewYGrid.Text = "Show Y Grid";
            this.mnuViewYGrid.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuViewYGrid.Click += new System.EventHandler(this.mnuViewYGrid_Click);
            // 
            // mnuViewSeparators
            // 
            this.mnuViewSeparators.AccessibleDescription = "Panel Separator";
            this.mnuViewSeparators.AccessibleName = "Panel Separator";
            this.mnuViewSeparators.Name = "mnuViewSeparators";
            this.mnuViewSeparators.Text = "Panel Separator";
            this.mnuViewSeparators.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuViewSeparators.Click += new System.EventHandler(this.mnuViewSeparators_Click);
            // 
            // mnuViewCrossHair
            // 
            this.mnuViewCrossHair.AccessibleDescription = "Crosshair";
            this.mnuViewCrossHair.AccessibleName = "Crosshair";
            this.mnuViewCrossHair.Name = "mnuViewCrossHair";
            this.mnuViewCrossHair.Text = "Crosshair";
            this.mnuViewCrossHair.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuViewCrossHair.Click += new System.EventHandler(this.mnuViewCrossHair_Click);
            // 
            // mnuViewMagnetic
            // 
            this.mnuViewMagnetic.AccessibleDescription = "Magnetic";
            this.mnuViewMagnetic.AccessibleName = "Magnetic";
            this.mnuViewMagnetic.Name = "mnuViewMagnetic";
            this.mnuViewMagnetic.Text = "Magnetic";
            this.mnuViewMagnetic.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.mnuViewMagnetic.Click += new System.EventHandler(this.mnuViewMagnetic_Click);
            // 
            // mnuView3D
            // 
            this.mnuView3D.AccessibleDescription = "Three D Style";
            this.mnuView3D.AccessibleName = "Three D Style";
            this.mnuView3D.Name = "mnuView3D";
            this.mnuView3D.Text = "Three D Style";
            this.mnuView3D.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // mnuDarvasBoxes
            // 
            this.mnuDarvasBoxes.AccessibleDescription = "Darvas Boxes";
            this.mnuDarvasBoxes.AccessibleName = "Darvas Boxes";
            this.mnuDarvasBoxes.Name = "mnuDarvasBoxes";
            this.mnuDarvasBoxes.Text = "Darvas Boxes";
            this.mnuDarvasBoxes.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuDarvasBoxes.Click += new System.EventHandler(this.mnuDarvasBoxes_Click);
            // 
            // radMenuSeparatorItem5
            // 
            this.radMenuSeparatorItem5.AccessibleDescription = "radMenuSeparatorItem5";
            this.radMenuSeparatorItem5.AccessibleName = "radMenuSeparatorItem5";
            this.radMenuSeparatorItem5.Name = "radMenuSeparatorItem5";
            this.radMenuSeparatorItem5.Text = "radMenuSeparatorItem5";
            this.radMenuSeparatorItem5.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuStartPage
            // 
            this.mnuStartPage.AccessibleDescription = "View Start Page";
            this.mnuStartPage.AccessibleName = "View Start Page";
            this.mnuStartPage.Name = "mnuStartPage";
            this.mnuStartPage.Text = "View Start Page";
            this.mnuStartPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuStartPage.Click += new System.EventHandler(this.mnuStartPage_Click);
            // 
            // mnuViewForex
            // 
            this.mnuViewForex.AccessibleDescription = "Forex Screen";
            this.mnuViewForex.AccessibleName = "Forex Screen";
            this.mnuViewForex.Name = "mnuViewForex";
            this.mnuViewForex.Text = "Forex Screen";
            this.mnuViewForex.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // mnuAlerts
            // 
            this.mnuAlerts.AccessibleDescription = "mnuAlerts";
            this.mnuAlerts.AccessibleName = "mnuAlerts";
            this.mnuAlerts.Name = "mnuAlerts";
            this.mnuAlerts.Text = "Alarmes";
            this.mnuAlerts.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAlerts.Click += new System.EventHandler(this.mnuAlerts_Click);
            // 
            // mnuScanner
            // 
            this.mnuScanner.AccessibleDescription = "Scanner";
            this.mnuScanner.AccessibleName = "Scanner";
            this.mnuScanner.Name = "mnuScanner";
            this.mnuScanner.Text = "Scanner";
            this.mnuScanner.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.mnuScanner.Click += new System.EventHandler(this.mnuScanner_Click);
            // 
            // mnuBackTest
            // 
            this.mnuBackTest.AccessibleDescription = "Back Test";
            this.mnuBackTest.AccessibleName = "Back Test";
            this.mnuBackTest.Name = "mnuBackTest";
            this.mnuBackTest.Text = "Back Test";
            this.mnuBackTest.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuBackTest.Click += new System.EventHandler(this.mnuBackTest_Click);
            // 
            // mnuScript
            // 
            this.mnuScript.AccessibleDescription = "Script Editor";
            this.mnuScript.AccessibleName = "Script Editor";
            this.mnuScript.Name = "mnuScript";
            this.mnuScript.Text = "Script Editor";
            this.mnuScript.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuScript.Click += new System.EventHandler(this.mnuScript_Click);
            // 
            // mnuChart
            // 
            this.mnuChart.AccessibleDescription = "Chart";
            this.mnuChart.AccessibleName = "Chart";
            this.mnuChart.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuPopoutChart});
            this.mnuChart.Name = "mnuChart";
            this.mnuChart.Text = "Chart";
            this.mnuChart.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuPopoutChart
            // 
            this.mnuPopoutChart.AccessibleDescription = "Popout Active Chart";
            this.mnuPopoutChart.AccessibleName = "Popout Active Chart";
            this.mnuPopoutChart.Name = "mnuPopoutChart";
            this.mnuPopoutChart.Text = "Popout Active Chart";
            this.mnuPopoutChart.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuPopoutChart.Click += new System.EventHandler(this.mnuPopoutChart_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.AccessibleDescription = "Help";
            this.mnuHelp.AccessibleName = "Help";
            this.mnuHelp.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuScriptHelp,
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Text = "Help";
            this.mnuHelp.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mnuScriptHelp
            // 
            this.mnuScriptHelp.AccessibleDescription = "TradeScript Help";
            this.mnuScriptHelp.AccessibleName = "TradeScript Help";
            this.mnuScriptHelp.Name = "mnuScriptHelp";
            this.mnuScriptHelp.Text = "TradeScript Help";
            this.mnuScriptHelp.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // mnuAbout
            // 
            this.mnuAbout.AccessibleDescription = "&About PLENA";
            this.mnuAbout.AccessibleName = "&About PLENA";
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Text = "&About PLENA";
            this.mnuAbout.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // commandBarRowElement4
            // 
            this.commandBarRowElement4.DisplayName = null;
            this.commandBarRowElement4.MinSize = new System.Drawing.Size(25, 25);
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Margin = new System.Windows.Forms.Padding(0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1,
            this.commandBarRowElement2});
            this.radCommandBar1.Size = new System.Drawing.Size(1068, 85);
            this.radCommandBar1.TabIndex = 1;
            this.radCommandBar1.Text = "radCommandBar1";
            this.radCommandBar1.Visible = false;
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.DisplayName = null;
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.ndtFile});
            this.commandBarRowElement1.Text = "";
            // 
            // ndtFile
            // 
            this.ndtFile.DisplayName = "commandBarStripElement1";
            this.ndtFile.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButton1,
            this.commandBarButton2,
            this.commandBarButton3,
            this.commandBarButton4,
            this.commandBarSeparator1,
            this.commandBarButton5,
            this.commandBarButton6,
            this.commandBarSeparator2,
            this.commandBarButton7,
            this.commandBarSeparator3,
            this.commandBarButton8});
            this.ndtFile.Name = "commandBarStripElement1";
            this.ndtFile.ShouldPaint = true;
            this.ndtFile.StretchHorizontally = false;
            this.ndtFile.Text = "";
            this.ndtFile.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.ndtFile.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton1
            // 
            this.commandBarButton1.AccessibleDescription = "commandBarButton1";
            this.commandBarButton1.AccessibleName = "commandBarButton1";
            this.commandBarButton1.DisplayName = "commandBarButton1";
            this.commandBarButton1.Image = null;
            this.commandBarButton1.Name = "commandBarButton1";
            this.commandBarButton1.Text = "";
            this.commandBarButton1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton2
            // 
            this.commandBarButton2.AccessibleDescription = "commandBarButton2";
            this.commandBarButton2.AccessibleName = "commandBarButton2";
            this.commandBarButton2.DisplayName = "commandBarButton2";
            this.commandBarButton2.EnableImageTransparency = true;
            this.commandBarButton2.Image = null;
            this.commandBarButton2.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.commandBarButton2.Name = "commandBarButton2";
            this.commandBarButton2.Text = "";
            this.commandBarButton2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton3
            // 
            this.commandBarButton3.AccessibleDescription = "commandBarButton3";
            this.commandBarButton3.AccessibleName = "commandBarButton3";
            this.commandBarButton3.DisplayName = "commandBarButton3";
            this.commandBarButton3.EnableImageTransparency = true;
            this.commandBarButton3.Image = null;
            this.commandBarButton3.Name = "commandBarButton3";
            this.commandBarButton3.Text = "";
            this.commandBarButton3.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton4
            // 
            this.commandBarButton4.AccessibleDescription = "commandBarButton4";
            this.commandBarButton4.AccessibleName = "commandBarButton4";
            this.commandBarButton4.DisplayName = "commandBarButton4";
            this.commandBarButton4.EnableImageTransparency = true;
            this.commandBarButton4.Image = null;
            this.commandBarButton4.Name = "commandBarButton4";
            this.commandBarButton4.ScaleTransform = new System.Drawing.SizeF(0.5F, 0.5F);
            this.commandBarButton4.Text = "";
            this.commandBarButton4.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.AccessibleDescription = "commandBarSeparator1";
            this.commandBarSeparator1.AccessibleName = "commandBarSeparator1";
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // commandBarButton5
            // 
            this.commandBarButton5.AccessibleDescription = "commandBarButton5";
            this.commandBarButton5.AccessibleName = "commandBarButton5";
            this.commandBarButton5.DisplayName = "commandBarButton5";
            this.commandBarButton5.Image = global::M4.Properties.Resources.open;
            this.commandBarButton5.Name = "commandBarButton5";
            this.commandBarButton5.Text = "";
            this.commandBarButton5.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton6
            // 
            this.commandBarButton6.AccessibleDescription = "commandBarButton6";
            this.commandBarButton6.AccessibleName = "commandBarButton6";
            this.commandBarButton6.DisplayName = "commandBarButton6";
            this.commandBarButton6.Image = global::M4.Properties.Resources.save;
            this.commandBarButton6.Name = "commandBarButton6";
            this.commandBarButton6.Text = "";
            this.commandBarButton6.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.AccessibleDescription = "commandBarSeparator2";
            this.commandBarSeparator2.AccessibleName = "commandBarSeparator2";
            this.commandBarSeparator2.DisplayName = "commandBarSeparator2";
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.commandBarSeparator2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // commandBarButton7
            // 
            this.commandBarButton7.AccessibleDescription = "commandBarButton7";
            this.commandBarButton7.AccessibleName = "commandBarButton7";
            this.commandBarButton7.DisplayName = "commandBarButton7";
            this.commandBarButton7.EnableImageTransparency = true;
            this.commandBarButton7.Image = null;
            this.commandBarButton7.Name = "commandBarButton7";
            this.commandBarButton7.Text = "";
            this.commandBarButton7.UseDefaultDisabledPaint = true;
            this.commandBarButton7.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator3
            // 
            this.commandBarSeparator3.AccessibleDescription = "commandBarSeparator3";
            this.commandBarSeparator3.AccessibleName = "commandBarSeparator3";
            this.commandBarSeparator3.DisplayName = "commandBarSeparator3";
            this.commandBarSeparator3.Name = "commandBarSeparator3";
            this.commandBarSeparator3.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator3.VisibleInOverflowMenu = false;
            // 
            // commandBarButton8
            // 
            this.commandBarButton8.AccessibleDescription = "commandBarButton8";
            this.commandBarButton8.AccessibleName = "commandBarButton8";
            this.commandBarButton8.DisplayName = "commandBarButton8";
            this.commandBarButton8.EnableImageTransparency = true;
            this.commandBarButton8.Image = global::M4.Properties.Resources.Help;
            this.commandBarButton8.Name = "commandBarButton8";
            this.commandBarButton8.Text = "";
            this.commandBarButton8.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarRowElement2
            // 
            this.commandBarRowElement2.DisplayName = null;
            this.commandBarRowElement2.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement2.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.ndtView});
            this.commandBarRowElement2.Text = "";
            // 
            // ndtView
            // 
            this.ndtView.DisplayName = "commandBarStripElement3";
            this.ndtView.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarLabel1,
            this.cbxApplicationStyle,
            this.cmdUseSemiLogScale,
            this.cmdShowXGrid,
            this.cmdShowYGrid,
            this.cmdShowPanelSeparators,
            this.commandBarSeparator4,
            this.cmdThreeDStyle,
            this.cmdDarvasBoxes,
            this.commandBarSeparator5,
            this.cmdViewStarPage,
            this.commandBarSeparator6,
            this.cmdViewForexScreen});
            this.ndtView.Name = "commandBarStripElement3";
            this.ndtView.Text = "";
            // 
            // commandBarLabel1
            // 
            this.commandBarLabel1.AccessibleDescription = "commandBarLabel1";
            this.commandBarLabel1.AccessibleName = "commandBarLabel1";
            this.commandBarLabel1.DisplayName = "commandBarLabel1";
            this.commandBarLabel1.Name = "commandBarLabel1";
            this.commandBarLabel1.Text = "Application Style";
            this.commandBarLabel1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cbxApplicationStyle
            // 
            this.cbxApplicationStyle.AccessibleDescription = "commandBarDropDownList1";
            this.cbxApplicationStyle.AccessibleName = "commandBarDropDownList1";
            this.cbxApplicationStyle.DisplayName = "commandBarDropDownList1";
            this.cbxApplicationStyle.DropDownAnimationEnabled = true;
            this.cbxApplicationStyle.MaxDropDownItems = 0;
            this.cbxApplicationStyle.Name = "cbxApplicationStyle";
            this.cbxApplicationStyle.Text = "";
            this.cbxApplicationStyle.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cmdUseSemiLogScale
            // 
            this.cmdUseSemiLogScale.AccessibleDescription = "commandBarToggleButton1";
            this.cmdUseSemiLogScale.AccessibleName = "commandBarToggleButton1";
            this.cmdUseSemiLogScale.DisplayName = "commandBarToggleButton1";
            this.cmdUseSemiLogScale.EnableImageTransparency = true;
            this.cmdUseSemiLogScale.Image = null;
            this.cmdUseSemiLogScale.Name = "cmdUseSemiLogScale";
            this.cmdUseSemiLogScale.Text = "";
            this.cmdUseSemiLogScale.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cmdShowXGrid
            // 
            this.cmdShowXGrid.AccessibleDescription = "commandBarToggleButton2";
            this.cmdShowXGrid.AccessibleName = "commandBarToggleButton2";
            this.cmdShowXGrid.DisplayName = "commandBarToggleButton2";
            this.cmdShowXGrid.EnableImageTransparency = true;
            this.cmdShowXGrid.Image = null;
            this.cmdShowXGrid.Name = "cmdShowXGrid";
            this.cmdShowXGrid.Text = "";
            this.cmdShowXGrid.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cmdShowYGrid
            // 
            this.cmdShowYGrid.AccessibleDescription = "commandBarToggleButton3";
            this.cmdShowYGrid.AccessibleName = "commandBarToggleButton3";
            this.cmdShowYGrid.DisplayName = "commandBarToggleButton3";
            this.cmdShowYGrid.EnableImageTransparency = true;
            this.cmdShowYGrid.Image = null;
            this.cmdShowYGrid.Name = "cmdShowYGrid";
            this.cmdShowYGrid.Text = "";
            this.cmdShowYGrid.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cmdShowPanelSeparators
            // 
            this.cmdShowPanelSeparators.AccessibleDescription = "commandBarToggleButton4";
            this.cmdShowPanelSeparators.AccessibleName = "commandBarToggleButton4";
            this.cmdShowPanelSeparators.DisplayName = "commandBarToggleButton4";
            this.cmdShowPanelSeparators.EnableImageTransparency = true;
            this.cmdShowPanelSeparators.Image = null;
            this.cmdShowPanelSeparators.Name = "cmdShowPanelSeparators";
            this.cmdShowPanelSeparators.Text = "";
            this.cmdShowPanelSeparators.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator4
            // 
            this.commandBarSeparator4.AccessibleDescription = "commandBarSeparator4";
            this.commandBarSeparator4.AccessibleName = "commandBarSeparator4";
            this.commandBarSeparator4.DisplayName = "commandBarSeparator4";
            this.commandBarSeparator4.Name = "commandBarSeparator4";
            this.commandBarSeparator4.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator4.VisibleInOverflowMenu = false;
            // 
            // cmdThreeDStyle
            // 
            this.cmdThreeDStyle.AccessibleDescription = "commandBarToggleButton5";
            this.cmdThreeDStyle.AccessibleName = "commandBarToggleButton5";
            this.cmdThreeDStyle.DisplayName = "commandBarToggleButton5";
            this.cmdThreeDStyle.EnableImageTransparency = true;
            this.cmdThreeDStyle.Image = null;
            this.cmdThreeDStyle.Name = "cmdThreeDStyle";
            this.cmdThreeDStyle.Text = "";
            this.cmdThreeDStyle.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cmdDarvasBoxes
            // 
            this.cmdDarvasBoxes.AccessibleDescription = "commandBarToggleButton6";
            this.cmdDarvasBoxes.AccessibleName = "commandBarToggleButton6";
            this.cmdDarvasBoxes.DisplayName = "commandBarToggleButton6";
            this.cmdDarvasBoxes.EnableImageTransparency = true;
            this.cmdDarvasBoxes.Image = null;
            this.cmdDarvasBoxes.Name = "cmdDarvasBoxes";
            this.cmdDarvasBoxes.Text = "";
            this.cmdDarvasBoxes.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator5
            // 
            this.commandBarSeparator5.AccessibleDescription = "commandBarSeparator5";
            this.commandBarSeparator5.AccessibleName = "commandBarSeparator5";
            this.commandBarSeparator5.DisplayName = "commandBarSeparator5";
            this.commandBarSeparator5.Name = "commandBarSeparator5";
            this.commandBarSeparator5.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator5.VisibleInOverflowMenu = false;
            // 
            // cmdViewStarPage
            // 
            this.cmdViewStarPage.AccessibleDescription = "commandBarToggleButton7";
            this.cmdViewStarPage.AccessibleName = "commandBarToggleButton7";
            this.cmdViewStarPage.DisplayName = "commandBarToggleButton7";
            this.cmdViewStarPage.EnableImageTransparency = true;
            this.cmdViewStarPage.Image = null;
            this.cmdViewStarPage.Name = "cmdViewStarPage";
            this.cmdViewStarPage.Text = "";
            this.cmdViewStarPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator6
            // 
            this.commandBarSeparator6.AccessibleDescription = "commandBarSeparator6";
            this.commandBarSeparator6.AccessibleName = "commandBarSeparator6";
            this.commandBarSeparator6.DisplayName = "commandBarSeparator6";
            this.commandBarSeparator6.Name = "commandBarSeparator6";
            this.commandBarSeparator6.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator6.VisibleInOverflowMenu = false;
            // 
            // cmdViewForexScreen
            // 
            this.cmdViewForexScreen.AccessibleDescription = "commandBarToggleButton1";
            this.cmdViewForexScreen.AccessibleName = "commandBarToggleButton1";
            this.cmdViewForexScreen.DisplayName = "commandBarToggleButton1";
            this.cmdViewForexScreen.EnableImageTransparency = true;
            this.cmdViewForexScreen.Image = null;
            this.cmdViewForexScreen.Name = "cmdViewForexScreen";
            this.cmdViewForexScreen.Text = "";
            this.cmdViewForexScreen.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarStripElement2
            // 
            this.commandBarStripElement2.DisplayName = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement2.Grip.AngleTransform = 90F;
            this.commandBarStripElement2.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButton9,
            this.commandBarButton10,
            this.commandBarButton11,
            this.commandBarButton12,
            this.commandBarSeparator13,
            this.commandBarButton13,
            this.commandBarSeparator14,
            this.commandBarButton14,
            this.commandBarSeparator15,
            this.commandBarButton15,
            this.commandBarButton16,
            this.commandBarButton17,
            this.commandBarSeparator16,
            this.commandBarButton18,
            this.commandBarButton19,
            this.commandBarButton20,
            this.commandBarSeparator17,
            this.commandBarButton21,
            this.commandBarButton22,
            this.commandBarButton23,
            this.commandBarButton24,
            this.commandBarButton25,
            this.commandBarSeparator18,
            this.commandBarButton26,
            this.commandBarButton27,
            this.commandBarButton28,
            this.commandBarButton29});
            this.commandBarStripElement2.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement2.OverflowButton.AngleTransform = 90F;
            this.commandBarStripElement2.StretchHorizontally = false;
            this.commandBarStripElement2.Text = "";
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.commandBarStripElement2.GetChildAt(0))).AngleTransform = 90F;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElement2.GetChildAt(2))).AngleTransform = 90F;
            // 
            // commandBarButton9
            // 
            this.commandBarButton9.DisplayName = "commandBarButton9";
            this.commandBarButton9.Image = null;
            this.commandBarButton9.Name = "commandBarButton9";
            this.commandBarButton9.StretchVertically = false;
            this.commandBarButton9.Text = "";
            this.commandBarButton9.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton10
            // 
            this.commandBarButton10.DisplayName = "commandBarButton9";
            this.commandBarButton10.Image = null;
            this.commandBarButton10.Name = "commandBarButton10";
            this.commandBarButton10.StretchVertically = false;
            this.commandBarButton10.Text = "";
            this.commandBarButton10.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton11
            // 
            this.commandBarButton11.DisplayName = "commandBarButton9";
            this.commandBarButton11.Image = null;
            this.commandBarButton11.Name = "commandBarButton11";
            this.commandBarButton11.StretchVertically = false;
            this.commandBarButton11.Text = "";
            this.commandBarButton11.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton12
            // 
            this.commandBarButton12.DisplayName = "commandBarButton9";
            this.commandBarButton12.Image = null;
            this.commandBarButton12.Name = "commandBarButton12";
            this.commandBarButton12.StretchVertically = false;
            this.commandBarButton12.Text = "";
            this.commandBarButton12.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator13
            // 
            this.commandBarSeparator13.AccessibleDescription = "commandBarSeparator7";
            this.commandBarSeparator13.AccessibleName = "commandBarSeparator7";
            this.commandBarSeparator13.AngleTransform = 90F;
            this.commandBarSeparator13.DisplayName = "commandBarSeparator7";
            this.commandBarSeparator13.Name = "commandBarSeparator13";
            this.commandBarSeparator13.StretchVertically = false;
            this.commandBarSeparator13.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator13.VisibleInOverflowMenu = false;
            // 
            // commandBarButton13
            // 
            this.commandBarButton13.DisplayName = "commandBarButton9";
            this.commandBarButton13.Image = null;
            this.commandBarButton13.Name = "commandBarButton13";
            this.commandBarButton13.StretchVertically = false;
            this.commandBarButton13.Text = "";
            this.commandBarButton13.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator14
            // 
            this.commandBarSeparator14.AccessibleDescription = "commandBarSeparator8";
            this.commandBarSeparator14.AccessibleName = "commandBarSeparator8";
            this.commandBarSeparator14.AngleTransform = 90F;
            this.commandBarSeparator14.DisplayName = "commandBarSeparator8";
            this.commandBarSeparator14.Name = "commandBarSeparator14";
            this.commandBarSeparator14.StretchVertically = false;
            this.commandBarSeparator14.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator14.VisibleInOverflowMenu = false;
            // 
            // commandBarButton14
            // 
            this.commandBarButton14.DisplayName = "commandBarButton9";
            this.commandBarButton14.Image = null;
            this.commandBarButton14.Name = "commandBarButton14";
            this.commandBarButton14.StretchVertically = false;
            this.commandBarButton14.Text = "";
            this.commandBarButton14.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator15
            // 
            this.commandBarSeparator15.AccessibleDescription = "commandBarSeparator9";
            this.commandBarSeparator15.AccessibleName = "commandBarSeparator9";
            this.commandBarSeparator15.AngleTransform = 90F;
            this.commandBarSeparator15.DisplayName = "commandBarSeparator9";
            this.commandBarSeparator15.Name = "commandBarSeparator15";
            this.commandBarSeparator15.StretchVertically = false;
            this.commandBarSeparator15.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator15.VisibleInOverflowMenu = false;
            // 
            // commandBarButton15
            // 
            this.commandBarButton15.DisplayName = "commandBarButton9";
            this.commandBarButton15.Image = null;
            this.commandBarButton15.Name = "commandBarButton15";
            this.commandBarButton15.StretchVertically = false;
            this.commandBarButton15.Text = "";
            this.commandBarButton15.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton16
            // 
            this.commandBarButton16.DisplayName = "commandBarButton9";
            this.commandBarButton16.Image = null;
            this.commandBarButton16.Name = "commandBarButton16";
            this.commandBarButton16.StretchVertically = false;
            this.commandBarButton16.Text = "";
            this.commandBarButton16.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton17
            // 
            this.commandBarButton17.DisplayName = "commandBarButton9";
            this.commandBarButton17.Image = null;
            this.commandBarButton17.Name = "commandBarButton17";
            this.commandBarButton17.StretchVertically = false;
            this.commandBarButton17.Text = "";
            this.commandBarButton17.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator16
            // 
            this.commandBarSeparator16.AccessibleDescription = "commandBarSeparator10";
            this.commandBarSeparator16.AccessibleName = "commandBarSeparator10";
            this.commandBarSeparator16.AngleTransform = 90F;
            this.commandBarSeparator16.DisplayName = "commandBarSeparator10";
            this.commandBarSeparator16.Name = "commandBarSeparator16";
            this.commandBarSeparator16.StretchVertically = false;
            this.commandBarSeparator16.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator16.VisibleInOverflowMenu = false;
            // 
            // commandBarButton18
            // 
            this.commandBarButton18.DisplayName = "commandBarButton9";
            this.commandBarButton18.Image = null;
            this.commandBarButton18.Name = "commandBarButton18";
            this.commandBarButton18.StretchVertically = false;
            this.commandBarButton18.Text = "";
            this.commandBarButton18.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton19
            // 
            this.commandBarButton19.DisplayName = "commandBarButton9";
            this.commandBarButton19.Image = null;
            this.commandBarButton19.Name = "commandBarButton19";
            this.commandBarButton19.StretchVertically = false;
            this.commandBarButton19.Text = "";
            this.commandBarButton19.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton20
            // 
            this.commandBarButton20.DisplayName = "commandBarButton9";
            this.commandBarButton20.Image = null;
            this.commandBarButton20.Name = "commandBarButton20";
            this.commandBarButton20.StretchVertically = false;
            this.commandBarButton20.Text = "";
            this.commandBarButton20.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator17
            // 
            this.commandBarSeparator17.AccessibleDescription = "commandBarSeparator11";
            this.commandBarSeparator17.AccessibleName = "commandBarSeparator11";
            this.commandBarSeparator17.AngleTransform = 90F;
            this.commandBarSeparator17.DisplayName = "commandBarSeparator11";
            this.commandBarSeparator17.Name = "commandBarSeparator17";
            this.commandBarSeparator17.StretchVertically = false;
            this.commandBarSeparator17.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator17.VisibleInOverflowMenu = false;
            // 
            // commandBarButton21
            // 
            this.commandBarButton21.DisplayName = "commandBarButton9";
            this.commandBarButton21.Image = null;
            this.commandBarButton21.Name = "commandBarButton21";
            this.commandBarButton21.StretchVertically = false;
            this.commandBarButton21.Text = "";
            this.commandBarButton21.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton22
            // 
            this.commandBarButton22.DisplayName = "commandBarButton9";
            this.commandBarButton22.Image = null;
            this.commandBarButton22.Name = "commandBarButton22";
            this.commandBarButton22.StretchVertically = false;
            this.commandBarButton22.Text = "";
            this.commandBarButton22.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton23
            // 
            this.commandBarButton23.DisplayName = "commandBarButton9";
            this.commandBarButton23.Image = null;
            this.commandBarButton23.Name = "commandBarButton23";
            this.commandBarButton23.StretchVertically = false;
            this.commandBarButton23.Text = "";
            this.commandBarButton23.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton24
            // 
            this.commandBarButton24.DisplayName = "commandBarButton9";
            this.commandBarButton24.Image = null;
            this.commandBarButton24.Name = "commandBarButton24";
            this.commandBarButton24.StretchVertically = false;
            this.commandBarButton24.Text = "";
            this.commandBarButton24.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton25
            // 
            this.commandBarButton25.DisplayName = "commandBarButton9";
            this.commandBarButton25.Image = null;
            this.commandBarButton25.Name = "commandBarButton25";
            this.commandBarButton25.StretchVertically = false;
            this.commandBarButton25.Text = "";
            this.commandBarButton25.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator18
            // 
            this.commandBarSeparator18.AccessibleDescription = "commandBarSeparator12";
            this.commandBarSeparator18.AccessibleName = "commandBarSeparator12";
            this.commandBarSeparator18.AngleTransform = 90F;
            this.commandBarSeparator18.DisplayName = "commandBarSeparator12";
            this.commandBarSeparator18.Name = "commandBarSeparator18";
            this.commandBarSeparator18.StretchVertically = false;
            this.commandBarSeparator18.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator18.VisibleInOverflowMenu = false;
            // 
            // commandBarButton26
            // 
            this.commandBarButton26.DisplayName = "commandBarButton9";
            this.commandBarButton26.Image = null;
            this.commandBarButton26.Name = "commandBarButton26";
            this.commandBarButton26.StretchVertically = false;
            this.commandBarButton26.Text = "";
            this.commandBarButton26.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton27
            // 
            this.commandBarButton27.DisplayName = "commandBarButton9";
            this.commandBarButton27.Image = null;
            this.commandBarButton27.Name = "commandBarButton27";
            this.commandBarButton27.StretchVertically = false;
            this.commandBarButton27.Text = "";
            this.commandBarButton27.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton28
            // 
            this.commandBarButton28.DisplayName = "commandBarButton9";
            this.commandBarButton28.Image = null;
            this.commandBarButton28.Name = "commandBarButton28";
            this.commandBarButton28.StretchVertically = false;
            this.commandBarButton28.Text = "";
            this.commandBarButton28.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton29
            // 
            this.commandBarButton29.DisplayName = "commandBarButton9";
            this.commandBarButton29.Image = null;
            this.commandBarButton29.Name = "commandBarButton29";
            this.commandBarButton29.StretchVertically = false;
            this.commandBarButton29.Text = "";
            this.commandBarButton29.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarStripElement4
            // 
            this.commandBarStripElement4.DisplayName = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement4.Grip.AngleTransform = 90F;
            this.commandBarStripElement4.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButton30,
            this.commandBarButton31,
            this.commandBarButton32,
            this.commandBarButton33,
            this.commandBarSeparator19,
            this.commandBarButton34,
            this.commandBarSeparator20,
            this.commandBarButton35,
            this.commandBarSeparator21,
            this.commandBarButton36,
            this.commandBarButton37,
            this.commandBarButton38,
            this.commandBarSeparator22,
            this.commandBarButton39,
            this.commandBarButton40,
            this.commandBarButton41,
            this.commandBarSeparator23,
            this.commandBarButton42,
            this.commandBarButton43,
            this.commandBarButton44,
            this.commandBarButton45,
            this.commandBarButton46,
            this.commandBarSeparator24,
            this.commandBarButton47,
            this.commandBarButton48,
            this.commandBarButton49,
            this.commandBarButton50});
            this.commandBarStripElement4.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement4.OverflowButton.AngleTransform = 90F;
            this.commandBarStripElement4.StretchHorizontally = false;
            this.commandBarStripElement4.Text = "";
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.commandBarStripElement4.GetChildAt(0))).AngleTransform = 90F;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElement4.GetChildAt(2))).AngleTransform = 90F;
            // 
            // commandBarButton30
            // 
            this.commandBarButton30.DisplayName = "commandBarButton9";
            this.commandBarButton30.Image = null;
            this.commandBarButton30.Name = "commandBarButton30";
            this.commandBarButton30.StretchVertically = false;
            this.commandBarButton30.Text = "";
            this.commandBarButton30.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton31
            // 
            this.commandBarButton31.DisplayName = "commandBarButton9";
            this.commandBarButton31.Image = null;
            this.commandBarButton31.Name = "commandBarButton31";
            this.commandBarButton31.StretchVertically = false;
            this.commandBarButton31.Text = "";
            this.commandBarButton31.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton32
            // 
            this.commandBarButton32.DisplayName = "commandBarButton9";
            this.commandBarButton32.Image = null;
            this.commandBarButton32.Name = "commandBarButton32";
            this.commandBarButton32.StretchVertically = false;
            this.commandBarButton32.Text = "";
            this.commandBarButton32.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton33
            // 
            this.commandBarButton33.DisplayName = "commandBarButton9";
            this.commandBarButton33.Image = null;
            this.commandBarButton33.Name = "commandBarButton33";
            this.commandBarButton33.StretchVertically = false;
            this.commandBarButton33.Text = "";
            this.commandBarButton33.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator19
            // 
            this.commandBarSeparator19.AccessibleDescription = "commandBarSeparator7";
            this.commandBarSeparator19.AccessibleName = "commandBarSeparator7";
            this.commandBarSeparator19.AngleTransform = 90F;
            this.commandBarSeparator19.DisplayName = "commandBarSeparator7";
            this.commandBarSeparator19.Name = "commandBarSeparator19";
            this.commandBarSeparator19.StretchVertically = false;
            this.commandBarSeparator19.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator19.VisibleInOverflowMenu = false;
            // 
            // commandBarButton34
            // 
            this.commandBarButton34.DisplayName = "commandBarButton9";
            this.commandBarButton34.Image = null;
            this.commandBarButton34.Name = "commandBarButton34";
            this.commandBarButton34.StretchVertically = false;
            this.commandBarButton34.Text = "";
            this.commandBarButton34.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator20
            // 
            this.commandBarSeparator20.AccessibleDescription = "commandBarSeparator8";
            this.commandBarSeparator20.AccessibleName = "commandBarSeparator8";
            this.commandBarSeparator20.AngleTransform = 90F;
            this.commandBarSeparator20.DisplayName = "commandBarSeparator8";
            this.commandBarSeparator20.Name = "commandBarSeparator20";
            this.commandBarSeparator20.StretchVertically = false;
            this.commandBarSeparator20.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator20.VisibleInOverflowMenu = false;
            // 
            // commandBarButton35
            // 
            this.commandBarButton35.DisplayName = "commandBarButton9";
            this.commandBarButton35.Image = null;
            this.commandBarButton35.Name = "commandBarButton35";
            this.commandBarButton35.StretchVertically = false;
            this.commandBarButton35.Text = "";
            this.commandBarButton35.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator21
            // 
            this.commandBarSeparator21.AccessibleDescription = "commandBarSeparator9";
            this.commandBarSeparator21.AccessibleName = "commandBarSeparator9";
            this.commandBarSeparator21.AngleTransform = 90F;
            this.commandBarSeparator21.DisplayName = "commandBarSeparator9";
            this.commandBarSeparator21.Name = "commandBarSeparator21";
            this.commandBarSeparator21.StretchVertically = false;
            this.commandBarSeparator21.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator21.VisibleInOverflowMenu = false;
            // 
            // commandBarButton36
            // 
            this.commandBarButton36.DisplayName = "commandBarButton9";
            this.commandBarButton36.Image = null;
            this.commandBarButton36.Name = "commandBarButton36";
            this.commandBarButton36.StretchVertically = false;
            this.commandBarButton36.Text = "";
            this.commandBarButton36.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton37
            // 
            this.commandBarButton37.DisplayName = "commandBarButton9";
            this.commandBarButton37.Image = null;
            this.commandBarButton37.Name = "commandBarButton37";
            this.commandBarButton37.StretchVertically = false;
            this.commandBarButton37.Text = "";
            this.commandBarButton37.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton38
            // 
            this.commandBarButton38.DisplayName = "commandBarButton9";
            this.commandBarButton38.Image = null;
            this.commandBarButton38.Name = "commandBarButton38";
            this.commandBarButton38.StretchVertically = false;
            this.commandBarButton38.Text = "";
            this.commandBarButton38.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator22
            // 
            this.commandBarSeparator22.AccessibleDescription = "commandBarSeparator10";
            this.commandBarSeparator22.AccessibleName = "commandBarSeparator10";
            this.commandBarSeparator22.AngleTransform = 90F;
            this.commandBarSeparator22.DisplayName = "commandBarSeparator10";
            this.commandBarSeparator22.Name = "commandBarSeparator22";
            this.commandBarSeparator22.StretchVertically = false;
            this.commandBarSeparator22.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator22.VisibleInOverflowMenu = false;
            // 
            // commandBarButton39
            // 
            this.commandBarButton39.DisplayName = "commandBarButton9";
            this.commandBarButton39.Image = null;
            this.commandBarButton39.Name = "commandBarButton39";
            this.commandBarButton39.StretchVertically = false;
            this.commandBarButton39.Text = "";
            this.commandBarButton39.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton40
            // 
            this.commandBarButton40.DisplayName = "commandBarButton9";
            this.commandBarButton40.Image = null;
            this.commandBarButton40.Name = "commandBarButton40";
            this.commandBarButton40.StretchVertically = false;
            this.commandBarButton40.Text = "";
            this.commandBarButton40.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton41
            // 
            this.commandBarButton41.DisplayName = "commandBarButton9";
            this.commandBarButton41.Image = null;
            this.commandBarButton41.Name = "commandBarButton41";
            this.commandBarButton41.StretchVertically = false;
            this.commandBarButton41.Text = "";
            this.commandBarButton41.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator23
            // 
            this.commandBarSeparator23.AccessibleDescription = "commandBarSeparator11";
            this.commandBarSeparator23.AccessibleName = "commandBarSeparator11";
            this.commandBarSeparator23.AngleTransform = 90F;
            this.commandBarSeparator23.DisplayName = "commandBarSeparator11";
            this.commandBarSeparator23.Name = "commandBarSeparator23";
            this.commandBarSeparator23.StretchVertically = false;
            this.commandBarSeparator23.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator23.VisibleInOverflowMenu = false;
            // 
            // commandBarButton42
            // 
            this.commandBarButton42.DisplayName = "commandBarButton9";
            this.commandBarButton42.Image = null;
            this.commandBarButton42.Name = "commandBarButton42";
            this.commandBarButton42.StretchVertically = false;
            this.commandBarButton42.Text = "";
            this.commandBarButton42.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton43
            // 
            this.commandBarButton43.DisplayName = "commandBarButton9";
            this.commandBarButton43.Image = null;
            this.commandBarButton43.Name = "commandBarButton43";
            this.commandBarButton43.StretchVertically = false;
            this.commandBarButton43.Text = "";
            this.commandBarButton43.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton44
            // 
            this.commandBarButton44.DisplayName = "commandBarButton9";
            this.commandBarButton44.Image = null;
            this.commandBarButton44.Name = "commandBarButton44";
            this.commandBarButton44.StretchVertically = false;
            this.commandBarButton44.Text = "";
            this.commandBarButton44.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton45
            // 
            this.commandBarButton45.DisplayName = "commandBarButton9";
            this.commandBarButton45.Image = null;
            this.commandBarButton45.Name = "commandBarButton45";
            this.commandBarButton45.StretchVertically = false;
            this.commandBarButton45.Text = "";
            this.commandBarButton45.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton46
            // 
            this.commandBarButton46.DisplayName = "commandBarButton9";
            this.commandBarButton46.Image = null;
            this.commandBarButton46.Name = "commandBarButton46";
            this.commandBarButton46.StretchVertically = false;
            this.commandBarButton46.Text = "";
            this.commandBarButton46.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarSeparator24
            // 
            this.commandBarSeparator24.AccessibleDescription = "commandBarSeparator12";
            this.commandBarSeparator24.AccessibleName = "commandBarSeparator12";
            this.commandBarSeparator24.AngleTransform = 90F;
            this.commandBarSeparator24.DisplayName = "commandBarSeparator12";
            this.commandBarSeparator24.Name = "commandBarSeparator24";
            this.commandBarSeparator24.StretchVertically = false;
            this.commandBarSeparator24.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator24.VisibleInOverflowMenu = false;
            // 
            // commandBarButton47
            // 
            this.commandBarButton47.DisplayName = "commandBarButton9";
            this.commandBarButton47.Image = null;
            this.commandBarButton47.Name = "commandBarButton47";
            this.commandBarButton47.StretchVertically = false;
            this.commandBarButton47.Text = "";
            this.commandBarButton47.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton48
            // 
            this.commandBarButton48.DisplayName = "commandBarButton9";
            this.commandBarButton48.Image = null;
            this.commandBarButton48.Name = "commandBarButton48";
            this.commandBarButton48.StretchVertically = false;
            this.commandBarButton48.Text = "";
            this.commandBarButton48.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton49
            // 
            this.commandBarButton49.DisplayName = "commandBarButton9";
            this.commandBarButton49.Image = null;
            this.commandBarButton49.Name = "commandBarButton49";
            this.commandBarButton49.StretchVertically = false;
            this.commandBarButton49.Text = "";
            this.commandBarButton49.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // commandBarButton50
            // 
            this.commandBarButton50.DisplayName = "commandBarButton9";
            this.commandBarButton50.Image = null;
            this.commandBarButton50.Name = "commandBarButton50";
            this.commandBarButton50.StretchVertically = false;
            this.commandBarButton50.Text = "";
            this.commandBarButton50.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radDock1
            // 
            this.radDock1.ActiveWindow = this.WorkspaceDefault;
            this.radDock1.CausesValidation = false;
            this.radDock1.Controls.Add(this.documentContainer1);
            this.radDock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock1.IsCleanUpTarget = true;
            this.radDock1.Location = new System.Drawing.Point(0, 20);
            this.radDock1.MainDocumentContainer = this.documentContainer1;
            this.radDock1.Name = "radDock1";
            this.radDock1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.radDock1.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.radDock1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock1.RootElement.Padding = new System.Windows.Forms.Padding(0);
            autoHideGroup1.Windows.Add(this.dockWindowPlaceholder1);
            this.radDock1.SerializableAutoHideContainer.BottomAutoHideGroups.Add(autoHideGroup1);
            this.radDock1.Size = new System.Drawing.Size(1080, 722);
            this.radDock1.TabIndex = 4;
            this.radDock1.TabStop = false;
            this.radDock1.Text = "radDock1";
            // 
            // WorkspaceDefault
            // 
            this.WorkspaceDefault.Caption = null;
            this.WorkspaceDefault.Controls.Add(this.radDock2);
            this.WorkspaceDefault.Controls.Add(this.radCommandBar2);
            this.WorkspaceDefault.Location = new System.Drawing.Point(6, 6);
            this.WorkspaceDefault.Name = "WorkspaceDefault";
            this.WorkspaceDefault.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this.WorkspaceDefault.Size = new System.Drawing.Size(1068, 710);
            this.WorkspaceDefault.Text = "Workspace Default";
            // 
            // radDock2
            // 
            this.radDock2.ActiveWindow = this._selectview;
            this.radDock2.CausesValidation = false;
            this.radDock2.Controls.Add(this.toolTabStrip3);
            this.radDock2.Controls.Add(this.documentContainer2);
            this.radDock2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock2.IsCleanUpTarget = true;
            this.radDock2.Location = new System.Drawing.Point(0, 0);
            this.radDock2.MainDocumentContainer = this.documentContainer2;
            this.radDock2.Name = "radDock2";
            this.radDock2.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.radDock2.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock2.RootElement.Padding = new System.Windows.Forms.Padding(0);
            this.radDock2.Size = new System.Drawing.Size(1037, 710);
            this.radDock2.TabIndex = 4;
            this.radDock2.TabStop = false;
            this.radDock2.Text = "radDock2";
            this.radDock2.AutoHideWindowDisplaying += new Telerik.WinControls.UI.Docking.AutoHideWindowDisplayingEventHandler(this.radDock2_AutoHideWindowDisplaying);
            this.radDock2.TransactionCommitted += new Telerik.WinControls.UI.Docking.RadDockTransactionEventHandler(this.radDock2_TransactionCommitted);
            this.radDock2.TransactionCommitting += new Telerik.WinControls.UI.Docking.RadDockTransactionCancelEventHandler(this.radDock2_TransactionCommitting);
            this.radDock2.DockWindowClosing += new Telerik.WinControls.UI.Docking.DockWindowCancelEventHandler(this.radDock2_DockWindowClosing);
            this.radDock2.DockWindowClosed += new Telerik.WinControls.UI.Docking.DockWindowEventHandler(this.radDock2_DockWindowClosed);
            this.radDock2.ActiveWindowChanged += new Telerik.WinControls.UI.Docking.DockWindowEventHandler(this.radDock2_ActiveWindowChanged);
            this.radDock2.FloatingWindowCreated += new Telerik.WinControls.UI.Docking.FloatingWindowEventHandler(this.radDock2_FloatingWindowCreated);
            this.radDock2.LoadedFromXml += new System.EventHandler(this.StateStateRestored);
            // 
            // _selectview
            // 
            this._selectview.AccessibleName = "selectView";
            this._selectview.AutoScroll = true;
            this._selectview.Caption = null;
            this._selectview.DefaultFloatingSize = new System.Drawing.Size(300, 500);
            this._selectview.Location = new System.Drawing.Point(1, 24);
            this._selectview.Margin = new System.Windows.Forms.Padding(0);
            this._selectview.Name = "_selectview";
            this._selectview.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._selectview.Size = new System.Drawing.Size(218, 684);
            this._selectview.Text = "_selectview";
            this._selectview.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.AutoHide;
            // 
            // toolTabStrip3
            // 
            this.toolTabStrip3.CanUpdateChildIndex = true;
            this.toolTabStrip3.CausesValidation = false;
            this.toolTabStrip3.Controls.Add(this._selectview);
            this.toolTabStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolTabStrip3.Name = "toolTabStrip3";
            // 
            // 
            // 
            this.toolTabStrip3.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.toolTabStrip3.SelectedIndex = 0;
            this.toolTabStrip3.Size = new System.Drawing.Size(220, 710);
            this.toolTabStrip3.SizeInfo.AbsoluteSize = new System.Drawing.Size(220, 200);
            this.toolTabStrip3.SizeInfo.SplitterCorrection = new System.Drawing.Size(20, 0);
            this.toolTabStrip3.TabIndex = 1;
            this.toolTabStrip3.TabStop = false;
            // 
            // documentContainer2
            // 
            this.documentContainer2.CausesValidation = false;
            this.documentContainer2.Controls.Add(this.documentTabStrip2);
            this.documentContainer2.Name = "documentContainer2";
            this.documentContainer2.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.documentContainer2.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer2.RootElement.Padding = new System.Windows.Forms.Padding(0);
            this.documentContainer2.SizeInfo.AbsoluteSize = new System.Drawing.Size(868, 200);
            this.documentContainer2.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer2.SizeInfo.SplitterCorrection = new System.Drawing.Size(-20, 0);
            this.documentContainer2.TabIndex = 2;
            // 
            // documentTabStrip2
            // 
            this.documentTabStrip2.AccessibleName = "Document Manager";
            this.documentTabStrip2.AllowDrop = true;
            this.documentTabStrip2.CanUpdateChildIndex = true;
            this.documentTabStrip2.CausesValidation = false;
            this.documentTabStrip2.Controls.Add(this._toolWeb);
            this.documentTabStrip2.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip2.Name = "documentTabStrip2";
            // 
            // 
            // 
            this.documentTabStrip2.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip2.SelectedIndex = 0;
            this.documentTabStrip2.Size = new System.Drawing.Size(813, 710);
            this.documentTabStrip2.TabIndex = 0;
            this.documentTabStrip2.TabStop = false;
            // 
            // _toolWeb
            // 
            this._toolWeb.AccessibleName = "ctlWeb";
            this._toolWeb.Caption = null;
            this._toolWeb.Location = new System.Drawing.Point(6, 29);
            this._toolWeb.Margin = new System.Windows.Forms.Padding(0);
            this._toolWeb.Name = "_toolWeb";
            this._toolWeb.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._toolWeb.Size = new System.Drawing.Size(801, 675);
            this._toolWeb.Text = "Web";
            // 
            // radCommandBar2
            // 
            this.radCommandBar2.Dock = System.Windows.Forms.DockStyle.Right;
            this.radCommandBar2.Location = new System.Drawing.Point(1037, 0);
            this.radCommandBar2.Margin = new System.Windows.Forms.Padding(0);
            this.radCommandBar2.Name = "radCommandBar2";
            this.radCommandBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.radCommandBar2.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement3});
            this.radCommandBar2.Size = new System.Drawing.Size(31, 710);
            this.radCommandBar2.TabIndex = 3;
            this.radCommandBar2.Text = "radCommandBar2";
            // 
            // commandBarRowElement3
            // 
            this.commandBarRowElement3.DisplayName = null;
            this.commandBarRowElement3.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement3.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.ndtChartTools});
            this.commandBarRowElement3.Text = "";
            // 
            // ndtChartTools
            // 
            this.ndtChartTools.DisplayName = "commandBarStripElement1";
            this.ndtChartTools.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
            this.ndtChartTools.GradientAngle = 15390F;
            // 
            // 
            // 
            this.ndtChartTools.Grip.AngleTransform = 90F;
            this.ndtChartTools.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cmdChart,
            this.cmdCrosshair,
            this.cmdSelect,
            this.cmdDeltaCursor,
            this.commandBarSeparator7,
            this.cmdMagnetic,
            this.commandBarSeparator8,
            this.cmdTextObject,
            this.commandBarSeparator9,
            this.cmdBuySymbol,
            this.cmdSellSymbol,
            this.cmdExitSymbol,
            this.commandBarSeparator10,
            this.cmdTrendLine,
            this.cmdRay,
            this.cmdChannel,
            this.cmdHorizontalLine,
            this.cmdVerticalLine,
            this.cmdRectangle,
            this.cmdEllipse,
            this.cmdArrow,
            this.cmdPolyline,
            this.commandBarSeparator12,
            this.cmdFibonacciRetracements,
            this.cmdFibonacciProjections,
            this.cmdFibonacciArcs,
            this.cmdFibonacciFan,
            this.cmdGannFan,
            this.cmdSpeedLines});
            this.ndtChartTools.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.ndtChartTools.OverflowButton.AngleTransform = 90F;
            this.ndtChartTools.StretchHorizontally = true;
            this.ndtChartTools.StretchVertically = true;
            this.ndtChartTools.Text = "";
            this.ndtChartTools.VisibleInCommandBarChanged += new System.EventHandler(this.ndtChartTools_VisibleInCommandBarChanged);
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.ndtChartTools.GetChildAt(0))).AngleTransform = 90F;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.ndtChartTools.GetChildAt(2))).AngleTransform = 90F;
            // 
            // cmdChart
            // 
            this.cmdChart.DisplayName = "commandBarButton9";
            this.cmdChart.Image = global::M4.Properties.Resources._16_NewChart;
            this.cmdChart.Name = "cmdChart";
            this.cmdChart.StretchHorizontally = false;
            this.cmdChart.StretchVertically = false;
            this.cmdChart.Text = "";
            this.cmdChart.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdChart.Click += new System.EventHandler(this.cmdChart_Click);
            // 
            // cmdCrosshair
            // 
            this.cmdCrosshair.AccessibleDescription = "commandBarToggleButton1";
            this.cmdCrosshair.AccessibleName = "commandBarToggleButton1";
            this.cmdCrosshair.DisplayName = "commandBarToggleButton1";
            this.cmdCrosshair.Image = ((System.Drawing.Image)(resources.GetObject("cmdCrosshair.Image")));
            this.cmdCrosshair.Name = "cmdCrosshair";
            this.cmdCrosshair.StretchHorizontally = false;
            this.cmdCrosshair.StretchVertically = false;
            this.cmdCrosshair.Text = "commandBarToggleButton1";
            this.cmdCrosshair.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdCrosshair.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.cmdCrosshair_ToggleStateChanging);
            this.cmdCrosshair.Click += new System.EventHandler(this.cmdCrosshair_Click);
            // 
            // cmdSelect
            // 
            this.cmdSelect.AccessibleDescription = "commandBarToggleButton1";
            this.cmdSelect.AccessibleName = "commandBarToggleButton1";
            this.cmdSelect.DisplayName = "commandBarToggleButton1";
            this.cmdSelect.Image = ((System.Drawing.Image)(resources.GetObject("cmdSelect.Image")));
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.StretchHorizontally = false;
            this.cmdSelect.StretchVertically = false;
            this.cmdSelect.Text = "commandBarToggleButton1";
            this.cmdSelect.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.cmdSelect.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdSelect.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.cmdSelect_ToggleStateChanging);
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // cmdDeltaCursor
            // 
            this.cmdDeltaCursor.AccessibleDescription = "commandBarToggleButton1";
            this.cmdDeltaCursor.AccessibleName = "commandBarToggleButton1";
            this.cmdDeltaCursor.DisplayName = "commandBarToggleButton1";
            this.cmdDeltaCursor.Image = ((System.Drawing.Image)(resources.GetObject("cmdDeltaCursor.Image")));
            this.cmdDeltaCursor.Name = "cmdDeltaCursor";
            this.cmdDeltaCursor.StretchHorizontally = false;
            this.cmdDeltaCursor.StretchVertically = false;
            this.cmdDeltaCursor.Text = "commandBarToggleButton1";
            this.cmdDeltaCursor.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdDeltaCursor.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.cmdDeltaCursor_ToggleStateChanging);
            this.cmdDeltaCursor.Click += new System.EventHandler(this.cmdDeltaCursor_Click);
            // 
            // commandBarSeparator7
            // 
            this.commandBarSeparator7.AccessibleDescription = "commandBarSeparator7";
            this.commandBarSeparator7.AccessibleName = "commandBarSeparator7";
            this.commandBarSeparator7.AngleTransform = 90F;
            this.commandBarSeparator7.DisplayName = "commandBarSeparator7";
            this.commandBarSeparator7.Name = "commandBarSeparator7";
            this.commandBarSeparator7.StretchHorizontally = true;
            this.commandBarSeparator7.StretchVertically = false;
            this.commandBarSeparator7.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator7.VisibleInOverflowMenu = false;
            // 
            // cmdMagnetic
            // 
            this.cmdMagnetic.AccessibleDescription = "commandBarToggleButton1";
            this.cmdMagnetic.AccessibleName = "commandBarToggleButton1";
            this.cmdMagnetic.DisplayName = "commandBarToggleButton1";
            this.cmdMagnetic.Image = global::M4.Properties.Resources._19_Magnetic;
            this.cmdMagnetic.Name = "cmdMagnetic";
            this.cmdMagnetic.StretchHorizontally = false;
            this.cmdMagnetic.StretchVertically = false;
            this.cmdMagnetic.Text = "commandBarToggleButton1";
            this.cmdMagnetic.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdMagnetic.ToggleStateChanging += new Telerik.WinControls.UI.StateChangingEventHandler(this.cmdMagnetic_ToggleStateChanging);
            this.cmdMagnetic.Click += new System.EventHandler(this.cmdMagnetic_Click);
            // 
            // commandBarSeparator8
            // 
            this.commandBarSeparator8.AccessibleDescription = "commandBarSeparator8";
            this.commandBarSeparator8.AccessibleName = "commandBarSeparator8";
            this.commandBarSeparator8.AngleTransform = 90F;
            this.commandBarSeparator8.DisplayName = "commandBarSeparator8";
            this.commandBarSeparator8.Name = "commandBarSeparator8";
            this.commandBarSeparator8.StretchHorizontally = true;
            this.commandBarSeparator8.StretchVertically = false;
            this.commandBarSeparator8.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator8.VisibleInOverflowMenu = false;
            // 
            // cmdTextObject
            // 
            this.cmdTextObject.DisplayName = "commandBarButton9";
            this.cmdTextObject.Image = global::M4.Properties.Resources._20_Text;
            this.cmdTextObject.Name = "cmdTextObject";
            this.cmdTextObject.StretchHorizontally = false;
            this.cmdTextObject.StretchVertically = false;
            this.cmdTextObject.Text = "";
            this.cmdTextObject.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdTextObject.Click += new System.EventHandler(this.cmdTextObject_Click);
            // 
            // commandBarSeparator9
            // 
            this.commandBarSeparator9.AccessibleDescription = "commandBarSeparator9";
            this.commandBarSeparator9.AccessibleName = "commandBarSeparator9";
            this.commandBarSeparator9.AngleTransform = 90F;
            this.commandBarSeparator9.DisplayName = "commandBarSeparator9";
            this.commandBarSeparator9.Name = "commandBarSeparator9";
            this.commandBarSeparator9.StretchHorizontally = true;
            this.commandBarSeparator9.StretchVertically = false;
            this.commandBarSeparator9.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator9.VisibleInOverflowMenu = false;
            // 
            // cmdBuySymbol
            // 
            this.cmdBuySymbol.DisplayName = "commandBarButton9";
            this.cmdBuySymbol.Image = global::M4.Properties.Resources._21_BuySymbol;
            this.cmdBuySymbol.Name = "cmdBuySymbol";
            this.cmdBuySymbol.StretchHorizontally = false;
            this.cmdBuySymbol.StretchVertically = false;
            this.cmdBuySymbol.Text = "";
            this.cmdBuySymbol.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdBuySymbol.Click += new System.EventHandler(this.cmdBuySymbol_Click);
            // 
            // cmdSellSymbol
            // 
            this.cmdSellSymbol.DisplayName = "commandBarButton9";
            this.cmdSellSymbol.Image = global::M4.Properties.Resources._22_SellSymbol;
            this.cmdSellSymbol.Name = "cmdSellSymbol";
            this.cmdSellSymbol.StretchHorizontally = false;
            this.cmdSellSymbol.StretchVertically = false;
            this.cmdSellSymbol.Text = "";
            this.cmdSellSymbol.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdSellSymbol.Click += new System.EventHandler(this.cmdSellSymbol_Click);
            // 
            // cmdExitSymbol
            // 
            this.cmdExitSymbol.DisplayName = "commandBarButton9";
            this.cmdExitSymbol.EnableImageTransparency = true;
            this.cmdExitSymbol.Image = ((System.Drawing.Image)(resources.GetObject("cmdExitSymbol.Image")));
            this.cmdExitSymbol.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmdExitSymbol.Name = "cmdExitSymbol";
            this.cmdExitSymbol.ScaleTransform = new System.Drawing.SizeF(1F, 1F);
            this.cmdExitSymbol.StretchHorizontally = false;
            this.cmdExitSymbol.StretchVertically = false;
            this.cmdExitSymbol.Text = "";
            this.cmdExitSymbol.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdExitSymbol.Click += new System.EventHandler(this.cmdExitSymbol_Click);
            // 
            // commandBarSeparator10
            // 
            this.commandBarSeparator10.AccessibleDescription = "commandBarSeparator10";
            this.commandBarSeparator10.AccessibleName = "commandBarSeparator10";
            this.commandBarSeparator10.AngleTransform = 90F;
            this.commandBarSeparator10.DisplayName = "commandBarSeparator10";
            this.commandBarSeparator10.Name = "commandBarSeparator10";
            this.commandBarSeparator10.StretchHorizontally = true;
            this.commandBarSeparator10.StretchVertically = false;
            this.commandBarSeparator10.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator10.VisibleInOverflowMenu = false;
            // 
            // cmdTrendLine
            // 
            this.cmdTrendLine.DisplayName = "commandBarButton9";
            this.cmdTrendLine.Image = global::M4.Properties.Resources._24_TrendLine;
            this.cmdTrendLine.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.cmdTrendLine.Name = "cmdTrendLine";
            this.cmdTrendLine.Padding = new System.Windows.Forms.Padding(0);
            this.cmdTrendLine.StretchHorizontally = false;
            this.cmdTrendLine.StretchVertically = false;
            this.cmdTrendLine.Text = "";
            this.cmdTrendLine.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdTrendLine.Click += new System.EventHandler(this.cmdTrendLine_Click);
            // 
            // cmdRay
            // 
            this.cmdRay.DisplayName = "commandBarButton9";
            this.cmdRay.Image = global::M4.Properties.Resources._25_Ray;
            this.cmdRay.Name = "cmdRay";
            this.cmdRay.StretchHorizontally = false;
            this.cmdRay.StretchVertically = false;
            this.cmdRay.Text = "";
            this.cmdRay.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdRay.Click += new System.EventHandler(this.cmdRay_Click);
            // 
            // cmdChannel
            // 
            this.cmdChannel.DisplayName = "commandBarButton9";
            this.cmdChannel.Image = global::M4.Properties.Resources._26_Channel;
            this.cmdChannel.Name = "cmdChannel";
            this.cmdChannel.StretchHorizontally = false;
            this.cmdChannel.StretchVertically = false;
            this.cmdChannel.Text = "";
            this.cmdChannel.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdChannel.Click += new System.EventHandler(this.cmdChannel_Click);
            // 
            // cmdHorizontalLine
            // 
            this.cmdHorizontalLine.AccessibleDescription = "commandBarButton51";
            this.cmdHorizontalLine.AccessibleName = "commandBarButton51";
            this.cmdHorizontalLine.DisplayName = "commandBarButton51";
            this.cmdHorizontalLine.Image = global::M4.Properties.Resources._27_HorizontalLine;
            this.cmdHorizontalLine.Name = "cmdHorizontalLine";
            this.cmdHorizontalLine.StretchHorizontally = false;
            this.cmdHorizontalLine.StretchVertically = false;
            this.cmdHorizontalLine.Text = "commandBarButton51";
            this.cmdHorizontalLine.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdHorizontalLine.Click += new System.EventHandler(this.cmdHorizontalLine_Click);
            // 
            // cmdVerticalLine
            // 
            this.cmdVerticalLine.DisplayName = "commandBarButton9";
            this.cmdVerticalLine.Image = global::M4.Properties.Resources._28_VerticalLine;
            this.cmdVerticalLine.Name = "cmdVerticalLine";
            this.cmdVerticalLine.StretchHorizontally = false;
            this.cmdVerticalLine.StretchVertically = false;
            this.cmdVerticalLine.Text = "";
            this.cmdVerticalLine.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdVerticalLine.Click += new System.EventHandler(this.cmdVerticalLine_Click);
            // 
            // cmdRectangle
            // 
            this.cmdRectangle.DisplayName = "commandBarButton9";
            this.cmdRectangle.Image = global::M4.Properties.Resources._29_Rectangle;
            this.cmdRectangle.Name = "cmdRectangle";
            this.cmdRectangle.StretchHorizontally = false;
            this.cmdRectangle.StretchVertically = false;
            this.cmdRectangle.Text = "";
            this.cmdRectangle.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdRectangle.Click += new System.EventHandler(this.cmdRectangle_Click);
            // 
            // cmdEllipse
            // 
            this.cmdEllipse.DisplayName = "commandBarButton9";
            this.cmdEllipse.Image = global::M4.Properties.Resources._30_Ellipse;
            this.cmdEllipse.Name = "cmdEllipse";
            this.cmdEllipse.StretchHorizontally = false;
            this.cmdEllipse.StretchVertically = false;
            this.cmdEllipse.Text = "";
            this.cmdEllipse.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdEllipse.Click += new System.EventHandler(this.cmdEllipse_Click);
            // 
            // cmdArrow
            // 
            this.cmdArrow.DisplayName = "commandBarButton9";
            this.cmdArrow.Image = global::M4.Properties.Resources._31_Arrow;
            this.cmdArrow.Name = "cmdArrow";
            this.cmdArrow.StretchHorizontally = false;
            this.cmdArrow.StretchVertically = false;
            this.cmdArrow.Text = "";
            this.cmdArrow.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdArrow.Click += new System.EventHandler(this.cmdArrow_Click);
            // 
            // cmdPolyline
            // 
            this.cmdPolyline.DisplayName = "commandBarButton9";
            this.cmdPolyline.Image = global::M4.Properties.Resources._32_Polyline;
            this.cmdPolyline.Name = "cmdPolyline";
            this.cmdPolyline.StretchHorizontally = false;
            this.cmdPolyline.StretchVertically = false;
            this.cmdPolyline.Text = "";
            this.cmdPolyline.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdPolyline.Click += new System.EventHandler(this.cmdPolyline_Click);
            // 
            // commandBarSeparator12
            // 
            this.commandBarSeparator12.AccessibleDescription = "commandBarSeparator12";
            this.commandBarSeparator12.AccessibleName = "commandBarSeparator12";
            this.commandBarSeparator12.AngleTransform = 90F;
            this.commandBarSeparator12.DisplayName = "commandBarSeparator12";
            this.commandBarSeparator12.Name = "commandBarSeparator12";
            this.commandBarSeparator12.StretchHorizontally = true;
            this.commandBarSeparator12.StretchVertically = false;
            this.commandBarSeparator12.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator12.VisibleInOverflowMenu = false;
            // 
            // cmdFibonacciRetracements
            // 
            this.cmdFibonacciRetracements.DisplayName = "commandBarButton9";
            this.cmdFibonacciRetracements.Image = global::M4.Properties.Resources._34_FibonacciRetracements;
            this.cmdFibonacciRetracements.Name = "cmdFibonacciRetracements";
            this.cmdFibonacciRetracements.StretchHorizontally = false;
            this.cmdFibonacciRetracements.StretchVertically = false;
            this.cmdFibonacciRetracements.Text = "";
            this.cmdFibonacciRetracements.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdFibonacciRetracements.Click += new System.EventHandler(this.cmdFibonacciRetracements_Click);
            // 
            // cmdFibonacciProjections
            // 
            this.cmdFibonacciProjections.DisplayName = "commandBarButton9";
            this.cmdFibonacciProjections.Image = global::M4.Properties.Resources._35_FibonacciProjecttions;
            this.cmdFibonacciProjections.Name = "cmdFibonacciProjections";
            this.cmdFibonacciProjections.StretchHorizontally = false;
            this.cmdFibonacciProjections.StretchVertically = false;
            this.cmdFibonacciProjections.Text = "";
            this.cmdFibonacciProjections.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdFibonacciProjections.Click += new System.EventHandler(this.cmdFibonacciProjections_Click);
            // 
            // cmdFibonacciArcs
            // 
            this.cmdFibonacciArcs.DisplayName = "commandBarButton9";
            this.cmdFibonacciArcs.Image = global::M4.Properties.Resources._36_FibonacciArcs;
            this.cmdFibonacciArcs.Name = "cmdFibonacciArcs";
            this.cmdFibonacciArcs.StretchHorizontally = false;
            this.cmdFibonacciArcs.StretchVertically = false;
            this.cmdFibonacciArcs.Text = "";
            this.cmdFibonacciArcs.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdFibonacciArcs.Click += new System.EventHandler(this.cmdFibonacciArcs_Click);
            // 
            // cmdFibonacciFan
            // 
            this.cmdFibonacciFan.DisplayName = "commandBarButton9";
            this.cmdFibonacciFan.Image = global::M4.Properties.Resources._37_FibonacciFan;
            this.cmdFibonacciFan.Name = "cmdFibonacciFan";
            this.cmdFibonacciFan.StretchHorizontally = false;
            this.cmdFibonacciFan.StretchVertically = false;
            this.cmdFibonacciFan.Text = "";
            this.cmdFibonacciFan.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdFibonacciFan.Click += new System.EventHandler(this.cmdFibonacciFan_Click);
            // 
            // cmdGannFan
            // 
            this.cmdGannFan.DisplayName = "commandBarButton51";
            this.cmdGannFan.Image = global::M4.Properties.Resources._39_GannFan;
            this.cmdGannFan.Name = "cmdGannFan";
            this.cmdGannFan.StretchHorizontally = false;
            this.cmdGannFan.StretchVertically = false;
            this.cmdGannFan.Text = "";
            this.cmdGannFan.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdGannFan.Click += new System.EventHandler(this.cmdGannFan_Click);
            // 
            // cmdSpeedLines
            // 
            this.cmdSpeedLines.DisplayName = "commandBarButton52";
            this.cmdSpeedLines.Image = global::M4.Properties.Resources._40_SpeedLines;
            this.cmdSpeedLines.Name = "cmdSpeedLines";
            this.cmdSpeedLines.StretchHorizontally = false;
            this.cmdSpeedLines.StretchVertically = false;
            this.cmdSpeedLines.Text = "";
            this.cmdSpeedLines.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.cmdSpeedLines.Click += new System.EventHandler(this.cmdSpeedLine_Click);
            // 
            // documentContainer1
            // 
            this.documentContainer1.CausesValidation = false;
            this.documentContainer1.Controls.Add(this.documentTabStrip1);
            this.documentContainer1.Name = "documentContainer1";
            this.documentContainer1.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer1.RootElement.Padding = new System.Windows.Forms.Padding(0);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.TabIndex = 3;
            // 
            // documentTabStrip1
            // 
            this.documentTabStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.documentTabStrip1.CanUpdateChildIndex = true;
            this.documentTabStrip1.CausesValidation = false;
            this.documentTabStrip1.Controls.Add(this.WorkspaceDefault);
            this.documentTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip1.Name = "documentTabStrip1";
            // 
            // 
            // 
            this.documentTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip1.SelectedIndex = 0;
            this.documentTabStrip1.Size = new System.Drawing.Size(1080, 722);
            this.documentTabStrip1.TabIndex = 0;
            this.documentTabStrip1.TabStop = false;
            this.documentTabStrip1.TabStripVisible = false;
            // 
            // _toolAlerts
            // 
            this._toolAlerts.AccessibleName = "ctlAlerts";
            this._toolAlerts.Caption = null;
            this._toolAlerts.Location = new System.Drawing.Point(6, 29);
            this._toolAlerts.Margin = new System.Windows.Forms.Padding(0);
            this._toolAlerts.Name = "_toolAlerts";
            this._toolAlerts.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._toolAlerts.Size = new System.Drawing.Size(801, 675);
            this._toolAlerts.Text = "Alarmes";
            // 
            // _toolScanner
            // 
            this._toolScanner.AccessibleName = "_toolScanner";
            this._toolScanner.Caption = null;
            this._toolScanner.Location = new System.Drawing.Point(6, 29);
            this._toolScanner.Margin = new System.Windows.Forms.Padding(0);
            this._toolScanner.Name = "_toolScanner";
            this._toolScanner.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._toolScanner.Size = new System.Drawing.Size(801, 675);
            this._toolScanner.Text = "Scanner";
            // 
            // _toolBackTest
            // 
            this._toolBackTest.AccessibleName = "_toolBackTest";
            this._toolBackTest.Caption = null;
            this._toolBackTest.Location = new System.Drawing.Point(6, 29);
            this._toolBackTest.Margin = new System.Windows.Forms.Padding(0);
            this._toolBackTest.Name = "_toolBackTest";
            this._toolBackTest.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._toolBackTest.Size = new System.Drawing.Size(801, 675);
            this._toolBackTest.Text = "Back Test";
            // 
            // _toolScript
            // 
            this._toolScript.AccessibleName = "_toolScript";
            this._toolScript.Caption = null;
            this._toolScript.Location = new System.Drawing.Point(6, 29);
            this._toolScript.Margin = new System.Windows.Forms.Padding(0);
            this._toolScript.Name = "_toolScript";
            this._toolScript.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this._toolScript.Size = new System.Drawing.Size(801, 675);
            this._toolScript.Text = "Script Editor";
            // 
            // tmrMessageService
            // 
            this.tmrMessageService.Enabled = true;
            this.tmrMessageService.Tick += new System.EventHandler(this.tmrMessageService_Tick);
            // 
            // tmrClearAlerts
            // 
            this.tmrClearAlerts.Enabled = true;
            this.tmrClearAlerts.Interval = 120000;
            this.tmrClearAlerts.Tick += new System.EventHandler(this.tmrClearAlerts_Tick);
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // m_MenuBar
            // 
            this.m_MenuBar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuFile,
            this.mnuWorkspace,
            this.mnuOptions,
            this.mnuView,
            this.mnuChart,
            this.mnuHelp});
            this.m_MenuBar.Location = new System.Drawing.Point(0, 0);
            this.m_MenuBar.Margin = new System.Windows.Forms.Padding(0);
            this.m_MenuBar.Name = "m_MenuBar";
            this.m_MenuBar.Size = new System.Drawing.Size(1080, 20);
            this.m_MenuBar.TabIndex = 0;
            this.m_MenuBar.Text = "Menu";
            // 
            // commandBarRowElement5
            // 
            this.commandBarRowElement5.MinSize = new System.Drawing.Size(25, 25);
            // 
            // radDesktopAlert1
            // 
            this.radDesktopAlert1.CanMove = false;
            this.radDesktopAlert1.CaptionText = "ALERTA M4";
            this.radDesktopAlert1.ContentText = "Exemplo de alerta do M4.";
            this.radDesktopAlert1.Opacity = 0.5F;
            this.radDesktopAlert1.ShowOptionsButton = false;
            // 
            // frmMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 742);
            this.Controls.Add(this.radDock1);
            this.Controls.Add(this.m_MenuBar);
            this.Name = "frmMain2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLENA Trading Platform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain2_FormClosing);
            this.Load += new System.EventHandler(this.frmMain2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).EndInit();
            this.radDock1.ResumeLayout(false);
            this.WorkspaceDefault.ResumeLayout(false);
            this.WorkspaceDefault.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDock2)).EndInit();
            this.radDock2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip3)).EndInit();
            this.toolTabStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer2)).EndInit();
            this.documentContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip2)).EndInit();
            this.documentTabStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            this.documentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).EndInit();
            this.documentTabStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_MenuBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private Telerik.WinControls.UI.RadMenuItem mnuFile;
        private Telerik.WinControls.UI.RadMenuItem mnuOptions;
        private Telerik.WinControls.UI.RadMenuItem mnuView;
        private Telerik.WinControls.UI.RadMenuItem mnuHelp;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem2;
        private Telerik.WinControls.UI.RadMenuItem mnuExit;
        private Telerik.WinControls.UI.RadMenuItem mnuToolBar;
        private Telerik.WinControls.UI.RadMenuItem mnuChartToolsToolbar;
        private Telerik.WinControls.UI.RadMenuSeparatorItem mnuColors;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem4;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem5;
        private Telerik.WinControls.UI.RadMenuItem mnuStartPage;
        public Telerik.WinControls.UI.RadMenuItem mnuViewScaleType;
        private Telerik.WinControls.UI.RadMenuItem mnuViewForex;
        private Telerik.WinControls.UI.RadMenuItem mnuForeColor;
        private Telerik.WinControls.UI.RadMenuItem mnuBackColor;
        private Telerik.WinControls.UI.RadMenuItem mnuGridColor;
        private Telerik.WinControls.UI.RadMenuItem mnuUpColor;
        private Telerik.WinControls.UI.RadMenuItem mnuDownColor;
        private Telerik.WinControls.UI.RadMenuItem mnuGradientTop;
        private Telerik.WinControls.UI.RadMenuItem mnuGradientBottom;
        private Telerik.WinControls.UI.RadMenuItem mnuAppOffice2007Silver;
        private Telerik.WinControls.UI.RadMenuItem mnuAppOffice2007Black;
        private Telerik.WinControls.UI.RadMenuItem mnuPopoutChart;
        private Telerik.WinControls.UI.RadMenuItem mnuAbout;
        private Telerik.WinControls.UI.RadMenuItem mnuScriptHelp;
        private Telerik.WinControls.UI.RadMenuItem mnuManagerWorkspace;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement4;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement ndtFile;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton1;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton2;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton3;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton4;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton5;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton6;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton7;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator3;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton8;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement2;
        private Telerik.WinControls.UI.CommandBarStripElement ndtView;
        private Telerik.WinControls.UI.CommandBarLabel commandBarLabel1;
        private Telerik.WinControls.UI.CommandBarDropDownList cbxApplicationStyle;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdUseSemiLogScale;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdShowXGrid;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdShowYGrid;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdShowPanelSeparators;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator4;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdThreeDStyle;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdDarvasBoxes;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator5;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdViewStarPage;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator6;
        private Telerik.WinControls.UI.CommandBarToggleButton cmdViewForexScreen;
        private Telerik.WinControls.UI.RadSplitContainer radSplitContainer1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement2;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton9;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton10;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton11;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton12;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator13;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton13;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator14;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton14;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator15;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton15;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton16;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton17;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator16;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton18;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton19;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton20;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator17;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton21;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton22;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton23;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton24;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton25;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator18;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton26;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton27;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton28;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton29;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement4;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton30;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton31;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton32;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton33;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator19;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton34;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator20;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton35;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator21;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton36;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton37;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton38;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator22;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton39;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton40;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton41;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator23;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton42;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton43;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton44;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton45;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton46;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator24;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton47;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton48;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton49;
        private Telerik.WinControls.UI.CommandBarButton commandBarButton50;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer2;
        private Telerik.WinControls.UI.Docking.ToolWindow _toolWeb;
        private Telerik.WinControls.UI.Docking.ToolWindow _toolAlerts;
        private Telerik.WinControls.UI.Docking.ToolWindow _toolScanner;
        private Telerik.WinControls.UI.Docking.ToolWindow _toolBackTest;
        private Telerik.WinControls.UI.Docking.ToolWindow _toolScript;
        private Telerik.WinControls.UI.Docking.ToolWindow _selectview;
        public SelectView1 _select;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar2;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement3;
        private Telerik.WinControls.UI.CommandBarStripElement ndtChartTools;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator7;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator8;
        private Telerik.WinControls.UI.CommandBarButton cmdTextObject;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator9;
        private Telerik.WinControls.UI.CommandBarButton cmdBuySymbol;
        private Telerik.WinControls.UI.CommandBarButton cmdSellSymbol;
        private Telerik.WinControls.UI.CommandBarButton cmdExitSymbol;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator10;
        private Telerik.WinControls.UI.CommandBarButton cmdTrendLine;
        private Telerik.WinControls.UI.CommandBarButton cmdRay;
        private Telerik.WinControls.UI.CommandBarButton cmdChannel;
        private Telerik.WinControls.UI.CommandBarButton cmdVerticalLine;
        private Telerik.WinControls.UI.CommandBarButton cmdRectangle;
        private Telerik.WinControls.UI.CommandBarButton cmdEllipse;
        private Telerik.WinControls.UI.CommandBarButton cmdArrow;
        private Telerik.WinControls.UI.CommandBarButton cmdPolyline;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator12;
        private Telerik.WinControls.UI.CommandBarButton cmdFibonacciRetracements;
        private Telerik.WinControls.UI.CommandBarButton cmdFibonacciProjections;
        private Telerik.WinControls.UI.CommandBarButton cmdFibonacciArcs;
        private Telerik.WinControls.UI.CommandBarButton cmdFibonacciFan;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem1;
        private ToolWindow WorkspaceDefault;
        private DockWindowPlaceholder dockWindowPlaceholder1;
        private Timer tmrMessageService;
        private DocumentTabStrip documentTabStrip1;
        private DocumentTabStrip documentTabStrip2;
        public Telerik.WinControls.UI.CommandBarButton cmdChart;
        internal Timer tmrClearAlerts;
        internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
        public Telerik.WinControls.UI.RadMenuItem mnuViewCrossHair;
        public Telerik.WinControls.UI.RadMenuItem mnuViewMagnetic;
        public Telerik.WinControls.UI.CommandBarToggleButton cmdCrosshair;
        public Telerik.WinControls.UI.CommandBarToggleButton cmdDeltaCursor;
        public Telerik.WinControls.UI.CommandBarToggleButton cmdMagnetic;
        public Telerik.WinControls.UI.RadMenuItem mnuViewShowXGrid;
        public Telerik.WinControls.UI.RadMenuItem mnuViewYGrid;
        public Telerik.WinControls.UI.RadMenuItem mnuViewSeparators;
        public Telerik.WinControls.UI.RadMenuItem mnuView3D;
        public Telerik.WinControls.UI.RadMenuItem mnuDarvasBoxes;
        public Telerik.WinControls.UI.RadMenuItem mnuAppStyle;
        public Telerik.WinControls.UI.RadMenuItem mnuWorkspace;
        private Telerik.WinControls.UI.CommandBarButton cmdHorizontalLine;
        public Telerik.WinControls.UI.RadMenuItem mnuChart;
        public Telerik.WinControls.UI.RadMenuItem mnuFileSaveImage;
        public Telerik.WinControls.UI.RadMenuItem mnuFilePrint;
        public Telerik.WinControls.UI.RadMenuItem mnuChartColors;
        private Telerik.WinControls.Themes.Office2007BlackTheme office2007BlackTheme1;
        private Telerik.WinControls.Themes.Office2007SilverTheme office2007SilverTheme1;
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        public RadDock radDock1;
        public RadDock radDock2;
        private Telerik.WinControls.UI.RadMenuItem mnuAppWindows8;
        private Telerik.WinControls.UI.RadMenuItem mnuAppAqua;
        private Telerik.WinControls.UI.RadMenuItem mnuAppBreeze;
        private Telerik.WinControls.UI.RadMenuItem mnuAppDesert;
        private Telerik.WinControls.UI.RadMenuItem mnuAppHighContrastBlack;
        private Telerik.WinControls.UI.RadMenuItem mnuAppOffice2010Black;
        private Telerik.WinControls.UI.RadMenuItem mnuAppOffice2010Blue;
        private Telerik.WinControls.UI.RadMenuItem mnuAppOffice2010Silver;
        private Telerik.WinControls.UI.RadMenuItem mnuAppTelerikMetro;
        private Telerik.WinControls.UI.RadMenuItem mnuAppTelerikMetroBlue;
        private Telerik.WinControls.UI.RadMenuItem mnuAppWindows7;
        private Telerik.WinControls.UI.RadMenu m_MenuBar;
        private Telerik.WinControls.UI.RadMenuItem mnuAppVisualStudio2012Dark;
        private Telerik.WinControls.UI.RadMenuItem mnuAppVisualStudio2012Light;
        public Telerik.WinControls.UI.CommandBarToggleButton cmdSelect;
        private Telerik.WinControls.UI.RadSplitContainer radSplitContainer2;
        private ToolTabStrip toolTabStrip3;
        private Telerik.WinControls.UI.RadMenuItem mnuStatusManager;
        private Telerik.WinControls.UI.RadMenuItem mnuSelectTools;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement5;
        private Telerik.WinControls.UI.CommandBarButton cmdGannFan;
        private Telerik.WinControls.UI.CommandBarButton cmdSpeedLines;
        private Telerik.WinControls.UI.RadDesktopAlert radDesktopAlert1;
        private Telerik.WinControls.UI.RadMenuItem mnuAlerts;
        private Telerik.WinControls.UI.RadMenuItem mnuScanner;
        private Telerik.WinControls.UI.RadMenuItem mnuBackTest;
        private Telerik.WinControls.UI.RadMenuItem mnuScript;

    }
}