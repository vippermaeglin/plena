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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ExtremeDB;
using M4.AsyncOperations;
using M4.DataServer.Interface.ProtocolStructs;
using M4.M4v2.Authentication.Login;
using M4.M4v2.Chart;
using M4.M4v2.GridviewRowDetailsExtended;
using M4.M4v2.Portfolio;
using M4.M4v2.Settings;
using M4.M4v2.UpdateService;
using M4.M4v2.Workspace;
using M4.modulusfe.platform;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;
using M4Data.MessageService;
using Nevron.Globalization;
using Nevron.UI;
using Nevron.UI.WinForm.Controls;
using Nevron.UI.WinForm.Docking;
using STOCKCHARTXLib;
using oAuth;
using Telerik.WinControls;
using M4.DataServer.Interface;
using Periodicity = M4Core.Entities.Periodicity;
using System.Threading;

namespace M4
{
    public partial class frmMain : NForm
    {
        #region Members

        public IList<frmPopoutChart> ListPopout { get; set; }
        private FrmLogin _frmLogin;

        private string _archiveNameWorkspace;
        private bool _closeStartPage;
        private bool _closePortifolioPage;
        public ctlWeb _web;

        public ActionChart _actionChart;
        public int AmountWallet { get; set; }

        //A global intance of main form
        public static frmMain GInstance;

        public static string ClientId = "FROEDE"; //TODO: enter your Modulus client id here
        public static string ClientPassword = "847263854"; //TODO: enter your Modulus client password here
        public static string ClientTitle = "PLENA"; //TODO: enter the title of your application here
        public static string LicenseKey;

        public static NPalette NevronPalette;
        public ColorScheme CurColorScheme;
        public frmMain FrmMain;
        public string m_Style; //Nevron UI skin style

        public List<ExpertAdvisor> MExpertAdvisors;

        public CtlPainelChart MActiveChart;
        //public CtlPainelChart MActivePanelChart;
        private StatusManager _statusManager;

        private readonly NDocumentManager _mDocMan;
        private string _mActiveDocumentName; //The active document name (ctlChart, ctlOrder, etc.)
        private NUIDocument _mActiveDocument;
        private readonly string _mCmdArg; //The command line string
        private bool _mClosed; //For Nevron events
        private ctlData _mCtlData;

        public PortfolioView1 _data;
        public SelectView1 _select;

        public string path = Directory.GetCurrentDirectory() + "\\Base\\PORTFOLIO\\Summary.xml";

        public ConfigStudies configStudies;
        private static List<SymbolsPS> StockList = new List<SymbolsPS>();
		private static List<SymbolGroup> Portfolios = new List<SymbolGroup>();
        private Queue<MSRequest> _messageRequests = new Queue<MSRequest>();
        public int _messageRequestID = 0;
        private List<Operations> requestedOperations = new List<Operations>();
        private static readonly object _locker = new object();
        
        public System.Diagnostics.Stopwatch measureTime = new Stopwatch();
        public long timeEllapsedRequest = 0;
        public long timeEllapsedDatabase = 0;
        public long timeEllapsedDatabaseAccess = 0;
        public long timeEllapsedLoading = 0;
        public long timeEllapsed = 0;

        
#pragma warning disable 649
        private ctlPortfolio _mCtlPortfolio;
#pragma warning restore 649

        private readonly AsyncOperation _asyncOperation;

        public bool TweetTrades;

        public bool GetHorizontalLine { get; set; }
        public bool GetVerticaoLine { get; set; }

        #endregion

        #region Initizalization

        private void TranslateText()
        {
            cbxApplicationStyle.Items.Clear();
            cbxApplicationStyle.Items.Add(new NListBoxItem("Office 2007 Blue"));
            cbxApplicationStyle.Items.Add(new NListBoxItem("Office 2007 Silver"));
            cbxApplicationStyle.Items.Add(new NListBoxItem("Windows Vista"));
            cbxApplicationStyle.Items.Add(new NListBoxItem("Windows Default"));
            mnuHelp.Properties.Text = NLocalizationManager.Instance.Translate("&Help", "PtBr");
            mnuFile.Properties.Text = NLocalizationManager.Instance.Translate("&File", "PtBr");

            cbxApplicationStyle.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cbxApplicationStyle"];
            cbxApplicationStyle.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cbxApplicationStyle"];
            cbxApplicationStyle.Items[0].Text = Program.LanguageDefault.DictionaryMenuBar["optOffice2007Blue"];
            cbxApplicationStyle.Items[1].Text = Program.LanguageDefault.DictionaryMenuBar["optOffice2007Silver"];
            cbxApplicationStyle.Items[2].Text = Program.LanguageDefault.DictionaryMenuBar["optWindowsVista"];
            cbxApplicationStyle.Items[3].Text = Program.LanguageDefault.DictionaryMenuBar["optWindowsDefault"];

            cmdChart.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdChart"];
            cmdChart.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdChart"];
            cmdUseSemiLogScale.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            cmdUseSemiLogScale.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            cmdShowXGrid.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowXGrid"];
            cmdShowXGrid.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowXGrid"];
            cmdShowYGrid.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowYGrid"];
            cmdShowYGrid.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowYGrid"];
            cmdShowPanelSeparators.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            cmdShowPanelSeparators.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            cmdThreeDStyle.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdThreeDStyle"];
            cmdThreeDStyle.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdThreeDStyle"];
            cmdDarvasBoxes.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            cmdDarvasBoxes.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            cmdViewStarPage.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdViewStarPage"];
            cmdViewStarPage.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdViewStarPage"];
            cmdViewForexScreen.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdViewForexScreen"];
            cmdViewForexScreen.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdViewForexScreen"];

            cmdTextObject.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdTextObject"];
            cmdTextObject.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTextObject"];
            cmdBuySymbol.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdBuySymbol"];
            cmdBuySymbol.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdBuySymbol"];
            cmdExitSymbol.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdExitSymbol"];
            cmdExitSymbol.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdExitSymbol"];
            cmdTrendLine.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdTrendLine"];
            cmdTrendLine.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTrendLine"];
            cmdGannFan.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdGannFan"];
            cmdGannFan.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdGannFan"];
            cmdFibonacciArcs.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciArcs"];
            cmdFibonacciArcs.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciArcs"];
            cmdFibonacciRetracements.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciRetracements"];
            cmdFibonacciRetracements.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciRetracements"];
            cmdFibonacciFan.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciFan"];
            cmdFibonacciFan.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciFan"];
            cmdFibonacciTimeZones.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciTimeZones"];
            cmdFibonacciTimeZones.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciTimeZones"];
            cmdRectangle.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdRectangle"];
            cmdRectangle.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdRectangle"];
            cmdArrow.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdArrow"];
            cmdArrow.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdArrow"];

            cmdSelect.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdSelect"];
            cmdSelect.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSelect"];
            cmdDeltaCursor.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            cmdDeltaCursor.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            cmdMagnetic.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdMagnetic"];
            cmdMagnetic.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdMagnetic"];
            cmdSellSymbol.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdSellSymbol"];
            cmdSellSymbol.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSellSymbol"];
            cmdRay.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdRay"];
            cmdRay.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdRay"];
            cmdChannel.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdChannel"];
            cmdChannel.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdChannel"];
            cmdHorizontalLine.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdHorizontalLine"];
            cmdHorizontalLine.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdHorizontalLine"];
            cmdVerticalLine.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdVerticalLine"];
            cmdVerticalLine.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdVerticalLine"];
            cmdElipse.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdElipse"];
            cmdElipse.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdElipse"];
            cmdPolyline.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdPolyline"];
            cmdPolyline.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdPolyline"];
            cmdFibonacciProjections.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciProjections"];
            cmdFibonacciProjections.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciProjections"];
            cmdSpeedLines.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdSpeedLines"];
            cmdSpeedLines.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSpeedLines"];

            cmdFreeHandDrawing.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdFreeHandDrawing"];
            cmdFreeHandDrawing.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFreeHandDrawing"];
            cmdZoomArea.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomArea.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomArea"];
            cmdZoomZero.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomZero.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomZero"];
            cmdZoomIn.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomIn.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomIn"];
            cmdZoomOut.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];
            cmdZoomOut.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdZoomOut"];

            cmdCandleChart.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdCandleChart"];
            cmdCandleChart.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdCandleChart"];
            cmdStockLine.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdStockLine"];
            cmdStockLine.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdStockLine"];
            cmdBarChart.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdBarChart"];
            cmdBarChart.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdBarChart"];
            cmdHeikinAshi.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshi"];
            cmdHeikinAshi.Properties.TooltipText = Program.LanguageDefault.DictionaryMenuBar["cmdHeikinAshi"];

            mnuToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuToolbar"];
            mnuPriceIndicatorsToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuPriceIndicatorsToolbar"];
            mnuZoomTemplatesToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuZoomTemplatesToolbar"];

            mnuViewToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuViewToolbar"];
            mnuChartToolsToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuChartToolsToolbar"];
            mnuFileToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuBar["mnuFileToolbar"];

            mnuFile.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuFile"];
            mnuFileSaveImage.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSaveChartImage"];
            mnuFilePrint.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPrintChart"];
            mnuFileExit.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuExit"];

            mnuWorkspace.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuWorkspace"];
            mnuManagerWorkspace.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuManager"];

            mnuHelp.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuHelp"];

            mnuAbout.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuAboutPLENA"];

            mnuChart.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuChart"];

            mnuPopoutChart.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPopoutActiveChart"];

            mnuView.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuView"];
            mnuToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuToolbar"];

            mnuChartToolsToolbar.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuChartTools"];
            mnuAppStyle.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuApplicationStyle"];
            mnuViewScaleType.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuUseSemiLogScale"];
            mnuViewShowXGrid.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowXGrid"];
            mnuViewYGrid.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowYGrid"];
            mnuViewSeparators.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowPanelSeparators"];
            mnuViewCrosshair.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuCrosshair"];
            mnuDarvasBoxes.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuDarvasBoxes"];
            mnuStartPage.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuViewStartPag"];

            mnuSettings.Properties.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSettings"];
        }

        public frmMain()
        {

            
            FrmMain = this;
            _asyncOperation = AsyncHelper.CreateOperation();

            InitializeComponent();

            MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending,
                                                           MSRequestType.GetSymbolsList,
                                                           MSRequestOwner.M4,
                                                           new object[] { }));
            AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.InitializeSymbolDatabase,
                                                       new object[] { }));
            TranslateText();

            _mDocMan = m_DockManager.DocumentManager;

            GInstance = this;

            //Set visual settings here
            m_DockManager.DocumentStyle.TabAlign = TabAlign.Top;
            m_DockManager.DocumentStyle.StripButtons = DocumentStripButtons.VS2005;
            //m_DockManager.DocumentStyle.DocumentViewStyle = DocumentViewStyle.MdiStandard;

            _mDocMan.ActiveDocumentChanged += M_DocMan_OnActiveDocumentChanged;
            _mDocMan.DocumentActivated += M_DocMan_OnDocumentActivated;
            _mDocMan.DocumentClosing += M_DocMan_OnDocumentClosing;
            _mDocMan.DocumentInserted += M_DocMan_OnDocumentInserted;

            LoadWorkspaces();
            LoadColorScheme();

            m_DockManager.DocumentTabDoubleClick += m_DockManager_DocumentTabDoubleClick;
        }

        public frmMain(string LoadChart)
        {
            FrmMain = this;
            _asyncOperation = AsyncHelper.CreateOperation();

            InitializeComponent();

            GInstance = this;

            MessageService.SubmitRequest(new MSRequest("INITIALIZE", MSRequestStatus.Pending,
                                                           MSRequestType.GetSymbolsList,
                                                           MSRequestOwner.M4,
                                                           new object[] { }));
           AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.InitializeSymbolDatabase,
                                                       new object[] { }));
            _mCmdArg = LoadChart;
            _mDocMan.ActiveDocumentChanged += M_DocMan_OnActiveDocumentChanged;
            _mDocMan.DocumentActivated += M_DocMan_OnDocumentActivated;
            _mDocMan.DocumentClosing += M_DocMan_OnDocumentClosing;
            _mDocMan.DocumentInserted += M_DocMan_OnDocumentInserted;

            LoadWorkspaces();
            LoadColorScheme();

            m_DockManager.DocumentTabDoubleClick += m_DockManager_DocumentTabDoubleClick;
        }

        private void m_DockManager_DocumentTabDoubleClick(object sender, DocumentEventArgs e)
        {
           // frmMain2 teste = new frmMain2(this);
           // teste.Show();
            try
            {
                if (ListPopout == null)
                    ListPopout = new List<frmPopoutChart>();

                switch (e.Document.Client.Name)
                {
                    case "ctlChart":
                    case "CtlPainelChart":
                        {
                            CtlPainelChart chart = ((CtlPainelChart)e.Document.Client);

                            if (chart != null)
                            {

                                var chartExist = ListPopout.Any(cada => cada.MActiveChart.Parent == chart.Parent);

                                if (chartExist)
                                    return;

                                frmPopoutChart popout = new frmPopoutChart
                                {
                                    _myParent = MActiveChart.Parent,
                                    Text = MActiveChart.Title,
                                    Width = (Width * 80) / 100,
                                    Height = (Height * 80) / 100,
                                    MActiveChart = chart,
                                    IdTab = e.Document.ID
                                };

                                ListPopout.Add(popout);

                                MActiveChart.Parent = popout;

                                
                                popout.Show();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// Create an async operation bound to main UI thread
        /// </summary>
        /// <returns></returns>
        public AsyncOperation CreateUIAsyncOperation()
        {
            AsyncOperation asyncOp = null;
            _asyncOperation.Send(() => asyncOp = AsyncHelper.CreateOperation());
            if (asyncOp == null)
                throw new ApplicationException("Error creating async operation.");
            return asyncOp;
        }

        //Checks the license using the Modulus Platform server.
        public static bool CheckLicense()
        {
            LicenseKey = Properties.Settings.Default.LSTR;
            if (LicenseKey.Length != 47)
            {
                using (frmActivate activate = new frmActivate())
                {
                    activate.ShowDialog();
                    LicenseKey = Properties.Settings.Default.LSTR;
                    if (LicenseKey.Length != 47)
                    {
                        Environment.Exit(0);
                        return false;
                    }
                }
            }

            //The license has been set on this machine, now use the Modulus Platform web service 
            //to verify that the license key is still on the server (not revoked)
            Service auth = new Service();
            try
            {
                string message = auth.ValidateLicenseKey(ClientId, ClientPassword, LicenseKey);
                if (string.IsNullOrEmpty(message)) //Failed if null string
                {
                    MessageBox.Show("Invalid license key.", "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Properties.Settings.Default.LSTR = "";
                    Properties.Settings.Default.Save();
                    Environment.Exit(0);
                    return false;
                }
                //Optionally, show when this software expires
            }
            catch (Exception se)
            {
                if (se.Message == "License key expired")
                {
                    Properties.Settings.Default.LSTR = "";
                    Properties.Settings.Default.Save();
                    MessageBox.Show("License key expired.", "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (se.Message == "License key not activated")
                {
                    MessageBox.Show("Invalid license key.", "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Properties.Settings.Default.LSTR = "";
                    Properties.Settings.Default.Save();
                    Environment.Exit(0);
                    return false;
                }
                else
                {
                    MessageBox.Show("Failed to connect to web service.", "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Environment.Exit(0);
            }
            return true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {


            //Initialize DSServer Thread:
            //if (!DataFeeder.Instance().ProcessActive) DataFeeder.Instance().StartProcess();
            ////create a custom palette
            //NPalette palette = new NUIPalette
            //                       {
            //                           ControlDark = Color.FromArgb(170, 170, 170),
            //                           ControlLight = Color.FromArgb(170, 170, 170),
            //                           ControlBorder = Color.FromArgb(170, 170, 170),
            //                           Control = Color.FromArgb(170, 170, 170),
            //                       };

            ////apply the palette
            //NUIManager.ApplyPalette((NForm)sender, palette);
        }

        private readonly AutoResetEvent _evArrived = new AutoResetEvent(false);

        private void ReInitializeSymbols(object sender, EventArgs args)
        {
            InitializeSymbols();
        }

        private void InitializeSymbols()
        {
            //Get Symbols from database:
            DBSymbolShared.OpenDatabase("Plena", Directory.GetCurrentDirectory() + "\\Base\\Symbol\\");
            Connection _connection = DBSymbolShared.Connect();
            lock (StockList)
            {
                StockList = DBSymbolShared.LoadSymbols(_connection);
            }
            lock(Portfolios)
            {
                Portfolios = DBSymbolShared.LoadGroups(GroupType.Index, _connection);
                Portfolios.AddRange(DBSymbolShared.LoadGroups(GroupType.Portfolio, _connection));
            }
            DBSymbolShared.Disconnect(_connection);

            //Load portolios and stock selector:
            //if (_select != null) _select.LoadData();

        }


        public void PreLoad(FrmLogin frmLogin)
        {
            try
            {
                _frmLogin = frmLogin;

                Visible = false;

                _frmLogin.LoadStatus("Configurando Plena...");

                System.Threading.Thread.Sleep(1000);

                // Get local Symbol's List and prepare events:
                InitializeSymbols();
                // SYMBOLS LIST UPDATED: 
                DBSymbolShared.UpdateSymbolsEvent += new DBSymbolShared.SymbolsEventHandler(ReInitializeSymbols);

                cboPriceStyles.HostedControl.SelectedIndex = 0;
                mnuViewYGrid.Checked = true;
                mnuViewSeparators.Checked = true;
                mnuView3D.Checked = true;
                mnuViewCrosshair.Checked = false;
                EnableControls(false);
                mnuPriceStyle.Enabled = false;
                cmdSaveWorkspace.Enabled = false;

                cmdLoadWorkspace.Enabled = true;
                mnuFileImportExcel.Enabled = true;
                mnuFileImportCSV.Enabled = true;
                cmdImportExcel.Enabled = true;

                cboIndicators.HostedControl.SelectedIndex = 0;

                _frmLogin.LoadStatus("Update Style");

                UpdateStyle();

                //Load a chart file from a command line? 
                if (!string.IsNullOrEmpty(_mCmdArg))
                {
                    if (File.Exists(_mCmdArg))
                    {
                        _frmLogin.LoadStatus("Load chart from command line...");

                        ctlChart ctl = new ctlChart(this, null, _mCmdArg) { Dock = DockStyle.Fill };
                        NUIDocument document = new NUIDocument("", -1, ctl);
                        m_DockManager.DocumentManager.AddDocument(document);
                        ctl.StockChartX1.LoadFile(_mCmdArg);
                        // FROEDE_MARK ctl.StockChartX1.LoadFile(m_CmdArg);
                        document.Text = ctl.GetChartTitle();  // FROEDE_MARK ctl.StockChartX1.Symbol;
                        ctl.EnableControls(true);
                    }
                }

                //Recall window size and position
                if (!Properties.Settings.Default.LastPos.IsEmpty)
                {
                    Location = Properties.Settings.Default.LastPos;
                    Size = Properties.Settings.Default.LastSize;
                    if (Width < 800) Width = 800;
                    if (Height < 550) Height = 550;
                }

                //Flicker work-around for Nevron UI when maximizing
                if (Properties.Settings.Default.WindowState == FormWindowState.Maximized)
                {
                    Application.DoEvents();
                    Height = Screen.PrimaryScreen.Bounds.Height;
                    Width = Screen.PrimaryScreen.Bounds.Width;
                    WindowState = FormWindowState.Maximized;
                    Width = Properties.Settings.Default.LastSize.Width;
                    Height = Properties.Settings.Default.LastSize.Height;
                    Application.DoEvents();
                }

                _statusManager = new StatusManager { Dock = DockStyle.Fill };

                //Data manager window
                _frmLogin.LoadStatus("Carregando DataManager...");

                //_data = new PortfolioView1(this) { Dock = DockStyle.Fill };

                _mCtlData = new ctlData(this) { Dock = DockStyle.Fill, StatusManager = _statusManager };
                //NUIDocument docData = new NUIDocument("Data Manager", -1, _mCtlData);
                //m_DockManager.DocumentManager.AddDocument(docData);
                //m_DockManager.DocumentManager.RemoveDocument(docData);

                _frmLogin.LoadStatus("Verificando workspace...");

                XmlDocument xmlDocumentWorkspace = new XmlDocument();
                xmlDocumentWorkspace.Load(ListWorkspace._path + "Workspace.xml");
                XmlNodeList nodeList = xmlDocumentWorkspace.GetElementsByTagName("WORKSPACE");

                XmlNode node = nodeList.Cast<XmlNode>().Where(xmlNode => xmlNode["DEFAULT"].InnerText == "1" && !xmlNode["TEXT"].Equals("Plena")).
                    FirstOrDefault();

                bool defaultLoadWorkspace;

                if (node != null)
                {
                    _frmLogin.LoadStatus("Carregando último workspace salvo...");

                    LoadPanelsDockManager();

                    defaultLoadWorkspace = ManagerWorkspace.Instance().Load(this);

                    if (!defaultLoadWorkspace)
                    {
                        RestoreWorkspace(node["TEXT"].InnerText);
                        WorkspaceLoaded(node["TEXT"].InnerText);
                    }
                }
                else
                {
                    _frmLogin.LoadStatus("Loading Web Browser...");
                    OpenURL(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"]);

                    //Data manager window V2
                    AmountWallet = 1;

                    _frmLogin.LoadStatus("Loading Expert Advisors...");
                    LoadExpertAdvisors();

                    _frmLogin.LoadStatus("Loading Web Browser...");

                    _frmLogin.LoadStatus("Installing Hotkeys...");
                    InstallHotKeys(); //Only after everything is loaded

                    TweetTrades = Properties.Settings.Default.TweetTrades == "1";

                    LoadPanelsDockManager();

                    defaultLoadWorkspace = ManagerWorkspace.Instance().Load(this);
                }

                if (!defaultLoadWorkspace)
                    UpdateStyle();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message, "Initialization Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            nStatusBar1.Visible = false;
            Visible = true;

            string messageLogin = String.Format(Program.LanguageDefault.DictionaryLogin["messageLogin"], Program.LoginAuthentication.Login, Program.LoginAuthentication.Offline ? "OFFLINE" : "ONLINE");
            _statusManager.SetMessage(messageLogin, OutputWindowV2.OutputIcon.Chart);
        }

        private void LoadPanelsDockManager()
        {

            _select = new SelectView1 { Dock = DockStyle.Fill, MFrmMain = this };

            INDockZone root = m_DockManager.RootContainer.RootZone;

            NDockingPanel panel = new NDockingPanel
            {
                Text = Program.LanguageDefault.DictionarySelectTools["selectTools"],
                Key = "SelectTools",
                TabInfo = { Text = Program.LanguageDefault.DictionarySelectTools["selectTools"] }
            };

            panel.Permissions.AllowHide = false;
            panel.Permissions.AllowMaximize = false;
            panel.Permissions.Editable = false;
            panel.Permissions.ExposedDockAreas = DockAreaMask.None;

            panel.DockPadding.All = 1;

            panel.Caption.Buttons.Remove(1);
            panel.Caption.Buttons.Remove(3);
            panel.Controls.Add(_select);
            panel.PerformDock(root, DockStyle.Left);
            panel.ParentZone.SizeInfo.SizeLogic = SizeLogic.AutoSize;
            panel.AutoSize = false;
            panel.Width = 300;
            panel.MinimumSize = new Size(300, 0);
            panel.FloatingSize = new Size(300, 500);
            panel.TabInfo.AutoHideSize = new Size(300, 0);

            panel = new NDockingPanel
            {
                Text = Program.LanguageDefault.DictionaryPlena["statusManager"],
                Key = "StatusManager",
                TabInfo = { Text = Program.LanguageDefault.DictionaryPlena["statusManager"] },
            };
            panel.DockPadding.All = 2;

            panel.UseEmbeddedContextMenuCommands = false;

            panel.Caption.Buttons.Remove(1);
            panel.Caption.Buttons.Remove(3);
            panel.Controls.Add(_statusManager);
            panel.PerformDock(root, DockStyle.Bottom);
            panel.AutoHide();

            panel.Permissions.AllowHide = false;
            panel.Permissions.AllowMaximize = false;
        }

        private void LoadSelectTools(NDockingPanel panel)
        {
            INDockZone docHost = m_DockManager.DocumentManager.DocumentViewHost;

            _select = new SelectView1 { Dock = DockStyle.Fill, MFrmMain = this };

            panel.Controls.Add(_select);
            Size minsize = new Size(300, 0);
            panel.PerformDock(docHost, DockStyle.Left);
            INDockZone target = panel.ParentZone;
            panel.ParentZone.SizeInfo.SizeLogic = SizeLogic.AutoSize;
            panel.PerformDock(target, DockStyle.Left);
            panel.ParentZone.SizeInfo.SizeLogic = SizeLogic.AutoSize;
            panel.AutoSize = false;
            panel.Width = 300;
            panel.MinimumSize = minsize;

            panel.ContextMenuCommands[1].Enabled = false;
            panel.ContextMenuCommands[2].Enabled = false;

        }

        public void ReloadSelectTools()
        {
            _select.Clear();
            //_select.LoadGridDataAssetsAll();
            _select.LoadPortfolios();
        }

        public void ReloadSelectTools(List<string> updatePortfolios)
        {
            _select.LoadPortfolios(updatePortfolios);
        }

        public void CreatePortfolioTab(string portfolio)
        {
            _select.CreatePortfolioTab(/*portfolio*/);
        }

        public void RemovePortfolio(string name)
        {
            _select.RemovePortfolio(name);
        }

        private void LoadStatusManager()
        {
            INDockZone docHost = m_DockManager.DocumentManager.DocumentViewHost;

            _statusManager = new StatusManager { Dock = DockStyle.Fill };

            //output window
            NDockingPanel panel = new NDockingPanel
            {
                Text = Program.LanguageDefault.DictionaryPlena["statusManager"]
            };

            panel.Controls.Add(_statusManager);

            panel.PerformDock(docHost, DockStyle.Bottom);
            INDockZone target = panel.ParentZone;
            panel.PerformDock(target, DockStyle.Bottom);

            panel.AutoHide();
        }

        private void InstallHotKeys()
        {
            //HotKey htk = new HotKey("Save Workspace", Keys.W, HotKey.HotKeyModifiers.MOD_SHIFT | HotKey.HotKeyModifiers.MOD_CONTROL);
            //HotKeys.Add(htk);
        }

        #endregion

        #region Form Closing
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.WindowState = WindowState;

            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.LastPos = Location;
                Properties.Settings.Default.LastSize = Size;
            }

            if (_mCtlPortfolio != null)
                _mCtlPortfolio.SavePortfolio();

            //Close the data window's connection
            if (_mCtlData != null)
            {
                _mCtlData.DataFeed.Symbols = "";
                _mCtlData.DataFeed.EventForm = null;
                _mCtlData.DataFeed = null;
                _mCtlData = null;
            }

            Properties.Settings.Default.Save();

          // ManagerWorkspace.Instance().SaveTemplate(m_Style, this);

            if ((!String.IsNullOrEmpty(Program.LoginAuthentication.CodeSession)) && (!Program.LoginAuthentication.Login.ToUpper().Equals("GUEST")))
                Server.Instance(Program.LanguageDefault).EndSession(Program.LoginAuthentication.CodeSession);

            // Abort DSServer Thread:
            //DataFeeder.Instance().StopProcess();
        }

        #endregion

        #region Helper Functions
        //Enables/disables Nevron menus and command buttons
        private void EnableControls(bool enable)
        {
            mnuNN.Enabled = enable;
            mnuConsensusReport.Enabled = enable;
            mnuChart.Enabled = enable;
            mnuApplyExpertAdvisor.Enabled = enable;
            cboPriceStyles.Enabled = enable;
            cboIndicators.Enabled = enable;
            cmdZoomIn.Enabled = enable;
            cmdZoomOut.Enabled = enable;
            cmdScrollLeft.Enabled = enable;
            cmdTemplate.Enabled = enable;
            cmdScrollRight.Enabled = enable;
            cmdDelete.Enabled = enable;
            mnuExcel.Enabled = enable;
            cmdPrintChart.Enabled = enable;
            mnuFileSaveImage.Enabled = enable;
            mnuFileSaveTemplate.Enabled = enable;
            mnuFileExport.Enabled = enable;
            mnuFileExportCSV.Enabled = enable;
            mnuFileImportCSV.Enabled = true; // FROEDE_MARK Enable;
            mnuFilePrint.Enabled = enable;
            mnuView3D.Enabled = enable;
            mnuAppStyle.Enabled = enable;
            mnuViewScaleType.Enabled = enable;
            mnuViewSeparators.Enabled = enable;
            mnuViewShowXGrid.Enabled = enable;
            mnuViewYGrid.Enabled = enable;
            mnuViewCrosshair.Enabled = enable;
            mnuDarvasBoxes.Enabled = enable;
            mnuColors.Enabled = enable;
            mnuTools.Enabled = enable;
            mnuZoomIn.Enabled = enable;
            mnuZoomOut.Enabled = enable;
            mnuApplyTemplate.Enabled = enable;
            mnuScrollLeft.Enabled = enable;
            mnuScrollRight.Enabled = enable;
            mnuPriceStyle.Enabled = enable;
            mnuPatternRecognition.Enabled = enable;
            Application.DoEvents();
        }

        //Prompts to open a file
        private static string OpenDialog(string filter = "", string directory = "")
        {
            OpenFileDialog flOpenDialog = new OpenFileDialog
                                            {
                                                Filter = (string.IsNullOrEmpty(filter) ? "Stock Chart Files|*.icx" : filter),
                                                InitialDirectory = (string.IsNullOrEmpty(directory)
                                                                      ? Environment.GetFolderPath(
                                                                          Environment.SpecialFolder.DesktopDirectory)
                                                                      : directory),
                                                Title = "Open",
                                                CheckFileExists = true
                                            };
            flOpenDialog.ShowDialog();
            return flOpenDialog.FileName;
        }

        //Prompts to save a file
        private static string SaveDialog(string Filter)
        {
            SaveFileDialog flSaveDialog = new SaveFileDialog
                                            {
                                                Filter = (string.IsNullOrEmpty(Filter) ? "Stock Chart Files|*.icx" : Filter),
                                                Title = "Save",
                                                CheckFileExists = false,
                                                InitialDirectory =
                                                  Environment.GetFolderPath(
                                                  Environment.SpecialFolder.DesktopDirectory)
                                            };
            flSaveDialog.ShowDialog();
            return flSaveDialog.FileName;
        }
        #endregion

        #region Nevron UI Operations
        //The active document has changed
        private void M_DocMan_OnActiveDocumentChanged(object sender, DocumentEventArgs e)
        {
            bool enable = true;

            if (e.Document.Client == null)
                return;

            //Disable the controls if not the same type of form
            if (_mActiveDocumentName != e.Document.Client.Name)
            {
                enable = false;
            }
            if (!enable)
            {
                EnableControls(false);
            }

            if (e.Document.Client.Name == "ctlChart")
            {
                MActiveChart = (CtlPainelChart)e.Document.Client;
                MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);
                MActiveChart.LoadStockPortfolioActive();
            }
            else if (e.Document.Client.Name == "CtlPainelChart")
            {
                MActiveChart = (CtlPainelChart)e.Document.Client;
                //MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);

                if (_actionChart != ActionChart.NONE)
                    MActiveChart.ActiveChartChanged = true;

                MActiveChart.LoadStockPortfolioActive();
            }
            else
            {
                if (MActiveChart != null)
                {
                    MActiveChart = null;
                    _actionChart = ActionChart.NONE;
                }
            }
            ShowStatus("");

            _mActiveDocumentName = e.Document.Client.Name;
        }

        private void M_DocMan_OnDocumentActivated(object sender, DocumentEventArgs e)
        {
            if (_mClosed)
                _mClosed = false;

            if (e.Document.Client == null)
                return;

            switch (e.Document.Client.Name)
            {
                case "ctlChart":
                    if (MActiveChart != e.Document.Client)
                    {
                        MActiveChart = (CtlPainelChart)e.Document.Client;
                        MActiveChart.EnableControls(true);
                        MActiveChart.LoadStockPortfolioActive();
                    }
                    break;
                case "CtlPainelChart":
                    if (MActiveChart != e.Document.Client)
                    {
                        MActiveChart = (CtlPainelChart)e.Document.Client;
                        MActiveChart.EnableControls(true);
                        MActiveChart.LoadStockPortfolioActive();
                    }
                    break;
            }

            _mActiveDocument = e.Document;
        }

        //Document is closing - query save changes
        private void M_DocMan_OnDocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            EnableControls(false);

            if (e.Document.Client == null)
                return;

            switch (e.Document.Client.Name)
            {
                //case "ctlData":
                case "ctlWeb":
                    //This window may not be allowed to close
                    if (!_closeStartPage)
                        e.Cancel = true;
                    break;
                case "ctlPortfolio":
                case "PortfolioView1":
                    //This window may not be allowed to close
                    if (!_closePortifolioPage)
                        e.Cancel = true;
                    break;
                case "ctlAlert":
                    ctlAlert alert = (ctlAlert)e.Document.Client;
                    alert.Disconnect();
                    alert.SaveAlert(true);
                    break;
            }

            _mClosed = true;
        }

        public void RemoveActiveDocument()
        {
            if (_mActiveDocument == null) return;
            m_DockManager.DocumentManager.RemoveDocument(_mActiveDocument);
        }

        private void M_DocMan_OnDocumentInserted(object sender, DocumentEventArgs e)
        {
            _mActiveDocument = e.Document;
        }

        #endregion

        #region Save and Load Workspaces

        private void mnuFileLoadWorkspace_Click(object sender, CommandEventArgs e)
        {
            LoadWorkspace();
        }

        //Saves all charts and level 1 symbols
        public void SaveWorkspace()
        {
            if (_mCtlData == null) return;

            List<string> l1symbols = _mCtlData.GetSymbols();
            string output = l1symbols.Aggregate("Symbols\r\n", (current, t) => current + t + "\r\n");
            output = output + "Charts\r\n";

            //Ensure that the chart directory exists
            string chartPath = Application.StartupPath + @"\Charts\";
            Directory.CreateDirectory(chartPath);

            foreach (NUIDocument t in _mDocMan.Documents)
            {
                switch (t.Client.Name)
                {
                    case "ctlChart":
                        {
                            ctlChart chart = (ctlChart)t.Client;
                            ChartSelection selection = chart.GetChartSelection();

                            //Save the binary chart file
                            string chartFileName = selection.Symbol + "_" + selection.Periodicity + "_" +
                                                   Convert.ToString(selection.Interval) +
                                                   "_" + Convert.ToString(selection.Bars) + ".chart";
                            chart.SaveChart(chartPath + chartFileName);

                            //Add the binary chart filename to the workspace file
                            output = output + chartPath + chartFileName + "\r\n";
                        }
                        break;
                    case "CtlPainelChart":
                        {
                            CtlPainelChart chart = (CtlPainelChart)t.Client;
                            ChartSelection selection = chart.GetChartSelection();

                            //Save the binary chart file
                            string chartFileName = selection.Symbol + "_" + selection.Periodicity + "_" +
                                                   Convert.ToString(selection.Interval) +
                                                   "_" + Convert.ToString(selection.Bars) + ".chart";
                            chart.SaveChart(chartPath + chartFileName);

                            //Add the binary chart filename to the workspace file
                            output = output + chartPath + chartFileName + "\r\n";
                        }
                        break;
                }
            }

            string fileName = SaveDialog("Workspaces|*.workspace");
            if (fileName == "") return;

            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(output);
            sw.Close();
        }

        private void mnuFileSaveWorkspace_Click(object sender, CommandEventArgs e)
        {
            SaveWorkspace();
        }

        public void LoadWorkspace()
        {
            if (_mCtlData == null)
                return;

            string fileName = OpenDialog("Workspaces|*.workspace", "");
            if (!File.Exists(fileName)) return;

            List<string> symbols = new List<string>();
            StreamReader sr = new StreamReader(fileName);
            int loadType = -1;
            while (!sr.EndOfStream)
            {
                string record = sr.ReadLine();
                switch (record)
                {
                    case "Symbols":
                        loadType = 1;
                        break;
                    case "Charts":
                        loadType = 2;
                        break;
                    case "BinCharts":
                        loadType = 3;
                        break;
                    default:
                        switch (loadType)
                        {
                            case 1:
                                record = record.Trim();
                                if ((record.Length > 0) & (symbols.IndexOf(record) == -1))
                                {
                                    symbols.Add(record);
                                }
                                break;

                            case 2:
                                /*** FROEDE_MARK Do Nothing: WorkSpace just take care of Symbols and components position/sizing
                     
                            {
                    
                              string[] temp = record.Split('_');
                              if (temp.Length == 4)
                              {
                                ChartSelection selection = new ChartSelection();
                                string[] fileN = temp[0].Split('\\');
                                selection.Symbol = fileN[fileN.Length - 1];
                                selection.Interval = Convert.ToInt32(temp[2]);
                                temp[3] = temp[3].Replace(".chart", "");
                                selection.Bars = Convert.ToInt32(temp[3]);
                                switch (temp[1])
                                {
                                  case "Minutely":
                                    selection.Periodicity = Periodicity.Minutely;
                                    break;
                                  case "Hourly":
                                    selection.Periodicity = Periodicity.Hourly;
                                    break;
                                  case "Daily":
                                    selection.Periodicity = Periodicity.Daily;
                                    break;
                                  case "Weekly":
                                    selection.Periodicity = Periodicity.Weekly;
                                    break;
                                }

                                CreateNewChart(selection);
                            //    /* Uncomment this if you want to load objects on the charts
                            //     * when you load a workspace. This may cause issues with
                            //     * your data depending on the type of data provider.
                            //  ctlChart chart = m_ctlData.LoadRealTimeChart(selection);
                            //  if ((chart != null) & File.Exists(record))
                            //  {
                            //    chart.LoadAndUpdateChartFile(record);
                            //  }
                            //   
                              }
                                */
                                break;
                            //}
                        }
                        break;
                }
            }
            sr.Close();

            NUIDocument doc = _mDocMan.GetDocumentByText("Data Manager");
            if (doc != null)
            {
                _mDocMan.ActiveDocument = doc;
            }
            _mCtlData.LoadSymbols(symbols);
        }

        public void LoadColorScheme()
        {
            if (MActiveChart != null)
                MActiveChart._scheme = Properties.Settings.Default.SchemeColor;

            mnuAppStyle.Commands.Clear();

            foreach (KeyValuePair<string, string> scheme in Scheme.Instance().Schemes)
            {
                NCommand nCommand = new NCommand
                                        {
                                            Properties = { Text = scheme.Value }
                                        };

                if (MActiveChart != null) nCommand.Checked =
                    MActiveChart.m_SchemeColor.Equals(scheme.Key);

                nCommand.Click += NCommandSchemeClick;
                mnuAppStyle.Commands.Add(nCommand);
            }
        }

        private void NCommandSchemeClick(object sender, CommandEventArgs e)
        {
            foreach (KeyValuePair<string, string> scheme in
               Scheme.Instance().Schemes.Where(scheme => scheme.Value.Equals(e.Command.Properties.Text)))
            {
                MActiveChart.m_SchemeColor = scheme.Key;
            }

            if (e.Command.Checked)
                return;

            Scheme.Instance().UpdateChartColors(MActiveChart.StockChartX1, MActiveChart.m_SchemeColor);

            foreach (NCommand nCommand in mnuAppStyle.Commands)
                nCommand.Checked = nCommand.Properties.Text.ToUpper().Equals(e.Command.Properties.Text.ToUpper());

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadWorkspaces()
        {
            mnuWorkspace.Commands.Clear();

            NCommand cmdManager = new NCommand { Properties = { Text = Program.LanguageDefault.DictionaryMenuPlena["mnuManager"] } };
            cmdManager.Click += MnuManagerWorkspaceClick;

            mnuWorkspace.Commands.Add(cmdManager);

            XmlDocument xmlDocumentWorkspace = new XmlDocument();
            xmlDocumentWorkspace.Load(ListWorkspace._path + "Workspace.xml");
            XmlNodeList nodeList = xmlDocumentWorkspace.GetElementsByTagName("WORKSPACE");

            foreach (NCommand nCommand in from XmlNode node in nodeList select new NCommand { Properties = { Text = node["TEXT"].InnerText } })
            {
                nCommand.Click += NCommandClick;

                if (nCommand.Properties.Text == "Plena")
                    nCommand.Properties.BeginGroup = true;

                mnuWorkspace.Commands.Add(nCommand);
            }
        }

        public void WorkspaceLoaded(string description)
        {
            foreach (NCommand nCommand in mnuWorkspace.Commands)
                nCommand.Checked = nCommand.Properties.Text.ToUpper().Equals(description.ToUpper());

            nStatusBar1.Visible = false;
        }

        private void NCommandClick(object sender, CommandEventArgs e)
        {
            RestoreWorkspace(e.Command.Properties.Text.Trim());
            WorkspaceLoaded(e.Command.Properties.Text.Trim());
            UpdateStyle();
        }

        public void RestoreWorkspace(string archiveNameWorkspace)
        {
            _closeStartPage = true;
            _closePortifolioPage = true;
            _archiveNameWorkspace = archiveNameWorkspace;

            if (!File.Exists(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + _archiveNameWorkspace + ".xml"))
                return;

            NDockingFrameworkState state = new NDockingFrameworkState(m_DockManager);
            state.StateRestored += StateStateRestored;
            state.Load(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + _archiveNameWorkspace + ".xml");
            _closeStartPage = false;
            _closePortifolioPage = false;
            nStatusBar1.Visible = false;
        }

        private void StateStateRestored(object sender, EventArgs e)
        {
            NDockingFrameworkState nDockingFrameworkState = (NDockingFrameworkState)sender;

            RestoreDocumentsDefault(nDockingFrameworkState.Manager.DocumentManager.Documents);

            if (!File.Exists(ListWorkspace._path + "ATIVOS_" + _archiveNameWorkspace + ".xml"))
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ListWorkspace._path + "ATIVOS_" + _archiveNameWorkspace + ".xml");
            XmlNodeList nodeListAtivos = xmlDocument.GetElementsByTagName("ATIVO");

            bool permission = ListFeatures.Instance().PermissionFeature(EFeatures.HISTORICDATA, EPermission.Master, Program.LoginAuthentication.Features);
            var mBars = 0;


            switch (permission)
            {
                case true:
                    mBars = Properties.Settings.Default.History;
                    break;
                case false:
                    mBars = Properties.Settings.Default.MaxHistoryGuest;
                    break;
            }

            foreach (NUIDocument document in nDockingFrameworkState.Manager.DocumentManager.Documents.Where(document =>
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"]))))
            {
                XmlNode node = nodeListAtivos.Cast<XmlNode>().Where(nodeListAtivo => document.Text.Equals(nodeListAtivo["NOME"].InnerText)).
                    FirstOrDefault();

                if (node == null)
                    continue;

                string periodicidade = node["PERIODICIDADE"].InnerText;
                Periodicity periodicity;
                Periodicity.TryParse(periodicidade, false, out periodicity);

                string simbolo = node["SIMBOLO"].InnerText;

                CtlPainelChart chart = new CtlPainelChart(this)
                                           {
                                               Dock = DockStyle.Fill,
                                               MCtlData = _mCtlData,
                                               MSymbol = simbolo,
                                               m_Periodicity = periodicity,
                                               m_BarSize = 1,
                                               m_Bars = mBars,
                                               _source = "Plena",
                                               _async = true,
                                               StateDummy = true
                                           };

                document.Client = chart;


                if ((node["VISIBLE"] != null) && (node["VISIBLE"].InnerText.Equals("0")))
                    continue;


                MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { simbolo, periodicity }));
                AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.LoadCtlPainelChart,
                                                           new object[]
                                                               {document.ID, simbolo, periodicity, 1, mBars, "Plena"}));


               /*MActiveChart = new CtlPainelChart(this, _mCtlData, simbolo, periodicity, 1, mBars, "Plena") { Dock = DockStyle.Fill };
                LoadChartSettings(MActiveChart);

                document.Client = MActiveChart;

                document.Manager.ActiveDocument = document;*/
            }

            nDockingFrameworkState.Manager.DocumentManager.ActiveDocumentChanged += DocumentManagerActiveDocumentChanged;
        }

        private void DocumentManagerActiveDocumentChanged(object sender, DocumentEventArgs e)
        {
            if ((e.Document.Client == null) || (!e.Document.Client.Name.Equals("CtlPainelChart")))
                return;
            if ((MActiveChart == null) || (!MActiveChart.StateDummy))
                return;

            CtlPainelChart oldChart = (CtlPainelChart)e.Document.Client;

            MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { oldChart.MSymbol, oldChart.m_Periodicity }));
            AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.LoadCtlPainelChart,
                                                       new object[]
                                                           {
                                                               e.Document.ID, oldChart.MSymbol, oldChart.m_Periodicity,
                                                               oldChart.m_BarSize, oldChart.m_Bars, oldChart._source
                                                           }));



            /*
            MActiveChart = new CtlPainelChart(this, _mCtlData, oldChart.MSymbol, oldChart.m_Periodicity, oldChart.m_BarSize, oldChart.m_Bars, oldChart._source) { Dock = DockStyle.Fill };
            LoadChartSettings(MActiveChart);

            e.Document.Client = MActiveChart;
             */
        }

        /// <summary>
        /// Restaura os documentos padrões na tela
        /// </summary>
        /// <param name="documents"></param>
        private void RestoreDocumentsDefault(NUIDocument[] documents)
        {
            foreach (NUIDocument document in documents)
            {
                if (document.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"]))
                {
                    if (document.Client != null)
                        m_DockManager.DocumentManager.RemoveDocument(document);
                    else
                    {
                        if (_web == null)
                            _web = new ctlWeb(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"], this) { Dock = DockStyle.Fill };

                        document.Client = _web;
                    }
                }
                //else if (document.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"]))
                //{
                //    if (document.Client != null)
                //        m_DockManager.DocumentManager.RemoveDocument(document);
                //    else
                //    {
                //        if (_data == null)
                //            _data = new PortfolioView1(this) { Dock = DockStyle.Fill };

                //        document.Client = _data;
                //    }
                //}
            }
        }

        private void MnuManagerWorkspaceClick(object sender, CommandEventArgs e)
        {
            LoadViewManagerWorkspace();
        }

        #endregion

        #region Web Service Calls
        //Delete's all alerts that are more than one day old
        private static void ClearAlerts()
        {
            using (Service service = new Service())
            {
                try
                {
                    service.ClearAlerts(ClientId, ClientPassword, LicenseKey);
                }
                catch (Exception)
                {

                }
            }
        }

        //Check once per minute to see if its a new day, then clear alerts
        private static DateTime lastDate = DateTime.Now.Date;

        private void tmrClearAlerts_Tick(object sender, EventArgs e)
        {
            if (lastDate != DateTime.Now.Date)
            {
                lastDate = DateTime.Now.Date;
                //ClearAlerts();
            }
        }
        #endregion

        #region Voice Help
        //Speaks a command using the wav files in the Res directory
        public void Speak(string text)
        {
            try
            {
                while (BackgroundWorker1.IsBusy) // Wait until we can play the recording
                {
                    Application.DoEvents(); // Don't lock up the window
                }
                BackgroundWorker1.RunWorkerAsync(text); // Play the wave file now
            }
            catch (Exception)
            {
                // Do nothing if the wav file isn't available
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] values = e.Argument.ToString().Split(',');
            string lastWord = "";
            foreach (string v in values)
            {
                string value = v;
                if (value.IndexOf("[") != -1)
                {
                    value = value.Substring(1, value.Length - 2);
                    if (value.Length > 1 && lastWord != "symbol" && lastWord != "news alert" && lastWord != "trade alert")
                    {
                        UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\multiple.wav", 0, UnmanagedMethods.SND_FILENAME);
                    }
                    else
                    {
                        foreach (char t in value)
                        {
                            UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\" + t + ".wav", 0, UnmanagedMethods.SND_FILENAME);
                        }
                    }
                }
                else
                {
                    UnmanagedMethods.PlaySound(Application.StartupPath + @"\Res\" + value + ".wav", 0, UnmanagedMethods.SND_FILENAME);
                }
                lastWord = value;
            }
        }
        #endregion

        #region Expert Advisors


        /// <summary>
        /// Loads the expert advisors.
        /// This method is called at start up and each time
        /// the apply button is clicked on frmExpertAdvisors
        /// </summary>
        public void LoadExpertAdvisors()
        {

            ExpertAdvisorIO io = new ExpertAdvisorIO();
            try
            {
                MExpertAdvisors = io.LoadExpertAdvisors();
            }
            catch (ExpertAdvisorIOException e)
            {
                MessageBox.Show(e.Message, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        // Allow the user to select an expert advisor
        private void mnuApplyExpertAdvisor_Click(object sender, CommandEventArgs e)
        {
            frmExpertAdvisors eas = new frmExpertAdvisors(MExpertAdvisors, MActiveChart);
            eas.ShowDialog(this);
        }

        private void mnuManageExpertAdvisors_Click(object sender, CommandEventArgs e)
        {
            frmEditExpertAdvisor editEA = new frmEditExpertAdvisor(MExpertAdvisors);
            editEA.ShowDialog(this);
            LoadExpertAdvisors();
        }

        private void mnuCreateExpertAdvisor_Click(object sender, CommandEventArgs e)
        {
            frmExpertAdvisors eas = new frmExpertAdvisors(MExpertAdvisors, MActiveChart);
            eas.SetEditMode(true);
            eas.ShowDialog(this);

            LoadExpertAdvisors();
        }

        #endregion

        #region Artificial Intelligence

        private void mnuConsensusReport_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.RunConsensusReport();
        }

        private void mnuNN_Click(object sender, CommandEventArgs e)
        {
            frmNN nn = new frmNN();
            nn.ShowDialog();
        }
        #endregion

        #region Twitter
        public bool SendTweet(string message)
        {

            oAuthTwitter oAuth = new oAuthTwitter
                                     {
                                         Token = Properties.Settings.Default.oauth_token,
                                         TokenSecret = Properties.Settings.Default.oauth_secret
                                     };
            const string url = "http://twitter.com/statuses/update.xml";
            try
            {
                oAuth.oAuthWebRequest(oAuthTwitter.Method.POST, url, "status=" + oAuth.UrlEncode(message));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Popout Chart

        private void MnuPopoutChartClick(object sender, CommandEventArgs e)
        {
            LoadPopoutChart();
        }

        public void TabActivate()
        {
            if ((m_DockManager.DocumentManager.ActiveDocument == null) || (m_DockManager.DocumentManager.ActiveDocument.Client == null))
                return;

            try
            {
                if (m_DockManager.DocumentManager.ActiveDocument.Client.Name == "CtlPainelChart")
                {
                    CtlPainelChart chart = ((CtlPainelChart)m_DockManager.DocumentManager.ActiveDocument.Client);
                    MActiveChart = chart;
                }
            }
            catch (Exception ex)
            {
                MActiveChart = null;
            }
        }

        #endregion

        #region Menu Operations and Misc
        private void mnuFileExit_Click(object sender, CommandEventArgs e)
        {
            Close();
        }

        private void mnuFilePrint_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.PrintChart();
        }

        private void mnuFileSaveImage_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.SaveChartImage();
        }

        private void mnuFileLoadChart_Click(object sender, CommandEventArgs e)
        {
            LoadChartFileFromDisk();
        }

        private void LoadChartFileFromDisk()
        {
            string fName = OpenDialog();
            if (!File.Exists(fName)) return;
            CtlPainelChart ctl = new CtlPainelChart(this) { Dock = DockStyle.Fill };
            NUIDocument document = new NUIDocument("Chart", -1, ctl);
            m_DockManager.DocumentManager.AddDocument(document);
            ctl.LoadChartFile(fName);
            document.Text = ctl.GetChartTitle(); // FROEDE_MARK ctl.StockChartX1.Symbol;
        }

        private void mnuFileSaveChart_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.SaveChart("");
        }

        private void cmdPrintChart_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.StockChartX1.PrintChart();
        }

        private void mnuDarvasBoxes_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadDarvasBoxes();
        }

        private void mnuViewScaleType_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewScaleType();
        }

        private void mnuViewShowXGrid_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewShowXGrid();
        }

        private void mnuViewYGrid_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewShowYGrid();
        }

        private void mnuViewCrosshair_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewCrosshair();
        }

        private void cmdDeltaCursor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            LoadDeltaCursor();
        }
        public void LoadDeltaCursor()
        {

            bool @checked = !cmdDeltaCursor.Checked;


            MActiveChart.StockChartX1.DeltaCursor = @checked;
            cmdDeltaCursor.Checked = @checked;
        }
        private void mnuViewMagnetic_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            /*
            bool @checked = !MActiveChart.StockChartX1.Magnetic;
            mnuViewMagnetic.Checked = @checked;
            MActiveChart.StockChartX1.Magnetic = @checked;
            cmdMagnetic.Checked = @checked;*/

            bool @checked = !cmdMagnetic.Checked;
            mnuViewMagnetic.Checked = @checked;
            MActiveChart.StockChartX1.Magnetic = @checked;
            cmdMagnetic.Checked = @checked;

        }

        private void mnuViewSeparators_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewSeparators();
        }

        private void mnuText_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.StockChartX1.AddUserDefinedText("My Text " + DateTime.Now);
            MActiveChart.StockChartX1.Focus();
        }

        private void mnuBuySymbol_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.StockChartX1.AddUserSymbolObject(SymbolType.soBuySymbolObject,
                                                           "Buy " +
                                                           Convert.ToString(MActiveChart.StockChartX1.GetObjectCount(
                                                                              ObjectType.otBuySymbolObject)), "");
        }

        private void mnuSellSymbol_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.StockChartX1.AddUserSymbolObject(SymbolType.soSellSymbolObject,
                                                           "Sell " +
                                                           Convert.ToString(MActiveChart.StockChartX1.GetObjectCount(
                                                                              ObjectType.otSellSymbolObject)), "");
        }

        private void mnuExitSymbol_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.StockChartX1.AddUserSymbolObject(SymbolType.soExitSymbolObject,
                                                           "Exit " +
                                                           Convert.ToString(MActiveChart.StockChartX1.GetObjectCount(
                                                                              ObjectType.otExitSymbolObject)), "");
        }

        private void mnuTrendLine_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.AddUserTrendLine("TL " + Convert.ToString(DateTime.Now));
        }

        private void mnuEllipse_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsEllipse, "EL " + Convert.ToString(DateTime.Now));
        }

        private void mnuSpeedLines_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsSpeedLines, "SL " + Convert.ToString(DateTime.Now));
        }

        private void mnuGannFan_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsGannFan, "GF" + Convert.ToString(DateTime.Now));
        }

        private void mnuFibonacciRetracements_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFibonacciRetracements, "FR " + Convert.ToString(DateTime.Now));
        }

        private void mnuFibonacciArcs_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFibonacciArcs, "FA " + Convert.ToString(DateTime.Now));
        }

        private void mnuFibonacciFan_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFibonacciFan, "FF " + Convert.ToString(DateTime.Now));
        }

        private void mnuFibonacciTimeZones_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFibonacciTimeZones, "FT " + Convert.ToString(DateTime.Now));
        }

        private void mnuTironeLevels_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsTironeLevels, Convert.ToString(DateTime.Now));
        }

        private void mnuQuadrantLines_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsQuadrantLines, Convert.ToString(DateTime.Now));
        }

        private void mnuRaffRegression_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsRaffRegression, Convert.ToString(DateTime.Now));
        }

        private void mnuErrorChannels_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsErrorChannel, Convert.ToString(DateTime.Now));
        }

        private void mnuRectangle_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsRectangle, "RE " + Convert.ToString(DateTime.Now));
        }

        private void mnuArrow_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsTriangle, "AR " + Convert.ToString(DateTime.Now));
        }

        private void mnuFreehand_Click(object sender, CommandEventArgs e)
        {
            MActiveChart.StockChartX1.Update();
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFreehand, "FH " + Convert.ToString(DateTime.Now));
        }

        private void mnuFileExport_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.ExportToExcel();
        }

        private void mnuFileExportCSV_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.ExportToCSV();
        }

        private void mnuExcel_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.ExportToExcel();
        }

        private void mnuFileImportExcel_Click(object sender, CommandEventArgs e)
        {
            if (_mCtlData == null) return;
            ctlChart chart = new ctlChart(this, _mCtlData, "Excel Chart", true) { Dock = DockStyle.Fill };
            NUIDocument doc = new NUIDocument("Excel Chart", -1, chart);
            m_DockManager.DocumentManager.AddDocument(doc);
        }

        private void mnuFileImportCSV_Click(object sender, CommandEventArgs e)
        {
            if (_mCtlData == null)
                return;

            CtlPainelChart chart = new CtlPainelChart(this, _mCtlData, "CSV Chart", true) { Dock = DockStyle.Fill };
            NUIDocument doc = new NUIDocument("CSV Chart " + chart.GetChartTitle(), -1, chart);
            m_DockManager.DocumentManager.AddDocument(doc);

            //ctlChart chart = new ctlChart(this, _mCtlData, "CSV Chart", true) { Dock = DockStyle.Fill };
            //NUIDocument doc = new NUIDocument("CSV Chart " + chart.GetChartTitle(), -1, chart);
            //m_DockManager.DocumentManager.AddDocument(doc);
        }

        private void cmdImportExcel_Click(object sender, CommandEventArgs e)
        {
            if (_mCtlData == null) return;
            ctlChart chart = new ctlChart(this, _mCtlData, "Excel Chart", true) { Dock = DockStyle.Fill };
            NUIDocument doc = new NUIDocument("Excel Chart " + chart.GetChartTitle(), -1, chart);
            m_DockManager.DocumentManager.AddDocument(doc);
        }

        private void mnuForeColor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ChartForeColor = (new frmColor()).GetColor(MActiveChart.StockChartX1.ChartForeColor);
        }

        private void mnuBackColor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ChartBackColor = (new frmColor()).GetColor(MActiveChart.StockChartX1.ChartBackColor);
        }

        private void mnuGridColor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.Gridcolor = (new frmColor()).GetColor(MActiveChart.StockChartX1.Gridcolor);
        }

        private void mnuUpColor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.UpColor = (new frmColor()).GetColor(MActiveChart.StockChartX1.UpColor);
        }

        private void mnuDownColor_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.DownColor = (new frmColor()).GetColor(MActiveChart.StockChartX1.DownColor);
        }

        private void mnuGradientTop_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.BackGradientTop = (new frmColor()).GetColor(MActiveChart.StockChartX1.BackGradientTop);
            MActiveChart.StockChartX1.Update();
        }

        private void mnuGradientBottom_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.BackGradientBottom = (new frmColor()).GetColor(MActiveChart.StockChartX1.BackGradientBottom);
            MActiveChart.StockChartX1.Update();
        }

        private void cboPriceStyles_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.ChangePriceStyle(cboPriceStyles.HostedControl.Text);
        }

        private void mnuView3D_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            bool @checked = !MActiveChart.StockChartX1.ThreeDStyle;
            mnuView3D.Checked = @checked;
            MActiveChart.StockChartX1.ThreeDStyle = @checked;
        }

        private void cboIndicators_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.AddIndicator();
        }

        private void mnuFileSaveTemplate_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.SaveGeneralTemplate(SaveDialog("Chart Template|*.sct"));

        }

        private void cmdTemplate_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            string templateName = OpenDialog("Chart Template|*.sct", @"C:\");
            if (string.IsNullOrEmpty(templateName)) return;
            MActiveChart.StockChartX1.LoadGeneralTemplate(templateName);
        }

        private void mnuApplyTemplate_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            string templateName = OpenDialog("Chart Template|*.sct", @"C:\");
            if (string.IsNullOrEmpty(templateName)) return;
            MActiveChart.StockChartX1.LoadGeneralTemplate(templateName);
        }

        private void cmdZoomIn_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.StockChartX1.ZoomUserDefined();
        }

        private void cmdZoomOut_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ResetZoom();
        }

        private void mnuZoomIn_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ZoomUserDefined();
        }

        private void mnuZoomOut_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ResetZoom();
        }

        private void mnuScrollLeft_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ScrollLeft(1);
        }

        private void mnuScrollRight_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ScrollRight(1);
        }

        private void cmdScrollLeft_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ScrollLeft(1);
        }

        private void cmdScrollRight_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.ScrollRight(1);
        }

        private void mnuPatternRecognition_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.RunPatternRecognition();
        }

        private void cmdDelete_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.DeleteDrawings();
        }

        private void MnuFileSymbolClick(object sender, CommandEventArgs e)
        {
#pragma warning disable 168
            string symbol = (new frmFindSymbol()).GetSymbol(); //'TODO: Do something with the symbol
#pragma warning restore 168
        }

        private void mnuPriceStyle_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            (new frmPriceStyle()).GetInput(MActiveChart.StockChartX1, cboPriceStyles.HostedControl.Text);
        }

        private void cmdHelp_Click(object sender, CommandEventArgs e)
        {
            DisplayHelp();
        }

        private void DisplayHelp()
        {
            OpenURL(Application.StartupPath + @"\Res\Help.pdf", "System Help");
        }

        private void mnuAbout_Click(object sender, CommandEventArgs e)
        {
            (new frmHelp2()).ShowDialog();
        }

        private void mnuAppOffice2007_Click(object sender, CommandEventArgs e)
        {
            Properties.Settings.Default.Style = "Office2007Blue";
            UpdateStyle();
        }

        private void mnuAppOfficeSilver_Click(object sender, CommandEventArgs e)
        {
            Properties.Settings.Default.Style = "Office2007Silver";
            UpdateStyle();
        }

        private void mnuAppStandard_Click(object sender, CommandEventArgs e)
        {
            Properties.Settings.Default.Style = "WindowsDefault";
            UpdateStyle();

            ThemeResolutionService.ApplyThemeToControlTree(this, "Windows7");
        }

        private void cmdNewChart_Click(object sender, CommandEventArgs e)
        {
            CreateNewChart();
        }

        private void mnuCreateChart_Click(object sender, CommandEventArgs e)
        {
            CreateNewChart();
        }

        private void cmdAlert_Click(object sender, CommandEventArgs e)
        {
            CreateAlert();
        }

        private void mnuAlert_Click(object sender, CommandEventArgs e)
        {
            CreateAlert();
        }

        //Creates a new alert using TradeScript
        private void CreateAlert()
        {
            ctlAlert alert = new ctlAlert(this, _mCtlData) { Dock = DockStyle.Fill };
            NUIDocument docAlert = new NUIDocument("New Alert", -1, alert);
            m_DockManager.DocumentManager.AddDocument(docAlert);
        }

        private void mnuBackTest_Click(object sender, CommandEventArgs e)
        {
            RunBacktest();
        }

        public void RunBacktest()
        {
            ctlBacktest test = new ctlBacktest(this, _mCtlData) { Dock = DockStyle.Fill };
            NUIDocument docTest = new NUIDocument("Back Test", -1, test);
            m_DockManager.DocumentManager.AddDocument(docTest);
        }

        private void mnuScanner_Click(object sender, CommandEventArgs e)
        {
            RunScanner();
        }

        public void RunScanner()
        {
            ctlScanner scanner = new ctlScanner(this, _mCtlData) { Dock = DockStyle.Fill };
            NUIDocument docScanner = new NUIDocument("New Scanner", -1, scanner);
            m_DockManager.DocumentManager.AddDocument(docScanner);
        }

        public void LoadAllChartSettings()
        {
            //Update all controls with new color theme
            foreach (NUIDocument doc in _mDocMan.Documents)
            {
                if (doc.Client == null)
                    continue;

                switch (doc.Client.Name)
                {
                    case "CtlPainelChart":

                        LoadChartSettings((CtlPainelChart)doc.Client);
                        break;
                }
            }
        }

        public void LoadChartSettings(CtlPainelChart MActiveChart)
        {
            MActiveChart.StockChartX1.VolumePostfixLetter = "M";
            MActiveChart.StockChartX1.DisplayTitleBorder = true;
            MActiveChart.StockChartX1.ThreeDStyle = false;



            //Right, Top and Bottom Space
            /*MActiveChart.paddingTop = Properties.Settings.Default.PaddingTop;
            MActiveChart.paddingBottom = Properties.Settings.Default.PaddingBottom;
            MActiveChart.paddingRight = Properties.Settings.Default.PaddingRight;*/
            MActiveChart.SetChartPadding(Properties.Settings.Default.PaddingTop, Properties.Settings.Default.PaddingBottom, Properties.Settings.Default.PaddingRight);

            //Scale Type
            MActiveChart.StockChartX1.ScaleType = Properties.Settings.Default.SemiLogScale ?
                                                        ScaleType.stLinearScale
                                                      : ScaleType.stSemiLogScale;
            mnuViewScaleType.Checked = MActiveChart.StockChartX1.ScaleType == ScaleType.stLinearScale;

            //Visible Bars
            MActiveChart.StockChartX1.VisibleRecordCount = Properties.Settings.Default.ChartViewport;
            //Scale Precision
            MActiveChart.StockChartX1.ScalePrecision = Properties.Settings.Default.Decimals;
            //Show Horizontal Grid
            MActiveChart.StockChartX1.YGrid = Properties.Settings.Default.GridHorizontal;
            mnuViewYGrid.Checked = Properties.Settings.Default.GridHorizontal;
            //Show Vertical Grid
            MActiveChart.StockChartX1.XGrid = Properties.Settings.Default.GridVertical;
            mnuViewShowXGrid.Checked = Properties.Settings.Default.GridVertical;
            //Panel Separators
            MActiveChart.StockChartX1.HorizontalSeparators = Properties.Settings.Default.PanelSeparator;
            mnuViewSeparators.Checked = Properties.Settings.Default.PanelSeparator;
            //Heikin Ashi Smooth params
            MActiveChart.StockChartX1.SmoothHeikinPeriods = Properties.Settings.Default.SettingsHeikinSmoothPeriod;
            MActiveChart.StockChartX1.SmoothHeikinType = Properties.Settings.Default.SettingsHeikinSmoothType;


        }

        //Creates a new chart
        public void CreateNewChart()
        {

            if (_mCtlData == null) return;
            ChartSelection selection = (new frmSelectChart()).GetChartSelection();
            //      Application.DoEvents();
            //      if (string.IsNullOrEmpty(selection.Symbol)) 
            //        return;
            CreateNewCtlPainel(selection);
            //      m_ctlData.LoadRealTimeChart(selection);
        }


        // Creates a new chart from a selection
        public void CreateNewChart(ChartSelection selection)
        {
            if (_mCtlData == null)
                return;

            if (string.IsNullOrEmpty(selection.Symbol))
                return;

            if (selection.Bars > 10000)
            {
                //_statusManager.SetMessage("Loading extremely large selection for " + selection.Symbol + " - this may take awhile...", OutputWindowV2.OutputIcon.Chart);

                foreach (NUIDocument doc in _mDocMan.Documents.Where(doc => doc.Client.Name == "ctlData"))
                {
                    m_DockManager.DocumentManager.ActiveDocument = doc;
                    break;
                }
            }

            _mCtlData.LoadRealTimeChartAsync(selection, chart => { });
            //_mCtlData.LoadRealTimeChart(selection);        
        }

        // Creates a new chart from a selection
        public void CreateNewCtlPainel(ChartSelection selection, bool schemeActive=false)
        {
            try
            {
                if (_mCtlData == null)
                    return;

                if (string.IsNullOrEmpty(selection.Symbol))
                    return;

                if (selection.Bars > 10000)
                {
                    //_statusManager.SetMessage(
                    //    "Loading extremely large selection for " + selection.Symbol + " - this may take awhile...",
                    //    OutputWindowV2.OutputIcon.Chart);

                    foreach (NUIDocument doc in _mDocMan.Documents.Where(doc => doc.Client.Name == "ctlData"))
                    {
                        m_DockManager.DocumentManager.ActiveDocument = doc;
                        break;
                    }
                }
                else
                {
                    //_statusManager.SetMessage("Loading " + selection.Symbol + "...", OutputWindowV2.OutputIcon.Chart);
                }

                _mCtlData.LoadRealTimeCtlPainelChartAsync(selection, chart => { }, schemeActive);

                //_mCtlData.LoadRealTimeChart(selection);        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Shows a symbol on the order entry screen
        public void EnterOrder(string symbol)
        {
            if (_mCtlPortfolio == null) return;
            Application.DoEvents();
            ctlPortfolio order;
            foreach (NUIDocument doc in _mDocMan.Documents)
            {
                if (doc.Client.Name != "ctlPortfolio") continue;
                order = (ctlPortfolio)doc.Client;
                order.txtSymbol.Text = symbol.Trim().ToUpper();
                _mDocMan.ActiveDocument = doc;
                return;
            }
        }

        //Loads a URL in a web browser if it is not already displayed    
        public void OpenURL(string URL, string Title)
        {
            foreach (NUIDocument doc in _mDocMan.Documents)
            {
                if (doc.Client.Name != "ctlWeb")
                    continue;

                _web = (ctlWeb)doc.Client;
                if (_web.Title != Title)
                    continue;

                _web.WebBrowser1.Navigate(_web.Url); //Refresh
                _mDocMan.ActiveDocument = doc;
                return;
            }
            _web = new ctlWeb(URL, Title, this) { Dock = DockStyle.Fill };
            NUIDocument docWeb = new NUIDocument(Title, -1, _web);

            NMdiChild child = docWeb.Host as NMdiChild;

            if (child != null)
                child.CloseButton = false;

            m_DockManager.DocumentManager.AddDocument(docWeb);
        }

        private void cmdLoadWorkspace_Click(object sender, CommandEventArgs e)
        {
            LoadWorkspace();
        }

        private void cmdSaveWorkspace_Click(object sender, CommandEventArgs e)
        {
            SaveWorkspace();
        }

        public void UpdateStyle()
        {
            m_Style = Properties.Settings.Default.Style;

            if (m_Style == "")
            {
                m_Style = "Office2007Silver";
            }

            CurColorScheme = ColorScheme.GrayScale;
            NUIManager.SetPredefinedFrame(PredefinedFrame.None);

            //mnuAppOfficeSilver.Checked = false;
            //mnuAppStandard.Checked = false;
            //mnuAppOffice2007.Checked = false;

            //if (m_Style == "Office2007Blue")
            //{
            //    CurColorScheme = ColorScheme.Office2007Blue;
            //    NUIManager.SetPredefinedFrame(PredefinedFrame.Office2007Blue);
            //    mnuAppOffice2007.Checked = true;
            //}
            //else if (m_Style == "Office2007Silver")
            //{
            //    CurColorScheme = ColorScheme.LunaSilver;
            //    NUIManager.SetPredefinedFrame(PredefinedFrame.VistaSlate);
            //    mnuAppOfficeSilver.Checked = true;
            //}
            //else if (m_Style == "WindowsDefault")
            //{
            //    CurColorScheme = ColorScheme.WindowsDefault;
            //    NUIManager.SetPredefinedFrame(PredefinedFrame.None);
            //    mnuAppStandard.Checked = true;
            //}

            m_DockManager.Palette.Scheme = CurColorScheme;
            ndtPriceIndicators.Palette.Scheme = CurColorScheme;
            ndtZoom.Palette.Scheme = CurColorScheme;
            ndtFile.Palette.Scheme = CurColorScheme;
            ndtView.Palette.Scheme = CurColorScheme;
            //m_MenuBar.Palette.Scheme = CurColorScheme;
            //ndtChartTools.Palette.Scheme = CurColorScheme;
            //Palette.Scheme = CurColorScheme;

            //Palette.PressedLight = Color.Black;
            //Palette.PressedDark = Color.Black;

            //NUIManager.Palette.PressedLight = Color.Black;
            //NUIManager.Palette.PressedDark = Color.Black;
            ////NUIManager.Palette.Scheme = CurColorScheme; 
            //NUIManager.ApplyPalette();

            UpdateChartColors();

            M4v2.Themes.ChangeTheme.ChangeThemeName(m_Style);

            //NevronPalette = NUIManager.Palette;

            //create a custom palette
            NPalette palette = new NUIPalette
            {
                ControlDark = Color.FromArgb(170, 170, 170),
                ControlLight = Color.FromArgb(170, 170, 170),
                ControlBorder = Color.FromArgb(170, 170, 170),
                Control = Color.FromArgb(170, 170, 170),
            };

            //apply the palette
            NUIManager.ApplyPalette(this, palette);
        }

        public void UpdateChartColors()
        {
            try
            {
                //Update all controls with new color theme
                foreach (NUIDocument doc in _mDocMan.Documents)
                {
                    if (doc.Client == null)
                        continue;

                    switch (doc.Client.Name)
                    {
                        case "CtlPainelChart":
                            Scheme.Instance().UpdateChartColors(((CtlPainelChart)doc.Client).StockChartX1, MActiveChart.m_SchemeColor);
                            break;
                        case "ctlChart":
                            Scheme.Instance().UpdateChartColors(((CtlPainelChart)doc.Client).StockChartX1, MActiveChart.m_SchemeColor);
                            break;
                        case "ctlPortfolio":
                            ((ctlPortfolio)doc.Client).UpdateStyle(m_Style);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void mnuDataLogin_Click(object sender, CommandEventArgs e)
        {
            //_mCtlData.Login(false);
        }

        public void ShowStatus(string Status)
        {
            nStatusBar1.Panels[0].Text = Status;
        }

        private void mnuStartPage_Click(object sender, CommandEventArgs e)
        {
            OpenURL(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"]);
        }

        public ctlPortfolio GetPortfolio()
        {
            return _mCtlPortfolio;
        }

        //Displays the forex pairs screen
        //TODO: You must hookup this screen with your own live forex data.
        //This screen is populated with random Forex data as an example only!
        private void MnuViewForexClick(object sender, CommandEventArgs e)
        {
            if (_mDocMan.Documents.Any(doc => doc.Client.Name == "ctlForex"))
            {
                return;
            }

            //TODO: setup your pairs here via a selection box or pre-defined list
            //See comments in timer in ctlForex.vb
            List<string> pairs = new List<string>
                             {
                               "EUR/USD",
                               "EUR/GBP",
                               "USD/CHF",
                               "JPY/USD",
                               "EUR/CHF",
                               "GBP/USD",
                               "EUR/USD",
                               "AUD/USD",
                               "NZD/USD",
                               "USD/CAD"
                             };
            ctlForex forex = new ctlForex();
            forex.InitializePairs(pairs);

            forex.Dock = DockStyle.Fill;
            NUIDocument docForex = new NUIDocument("Forex", -1, forex);
            m_DockManager.DocumentManager.AddDocument(docForex);
        }


        private void mnuScriptHelp_Click(object sender, CommandEventArgs e)
        {
            OpenURL("http://www.modulusfe.com/tradescript/TradeScript.pdf", "TradeScript Help");
        }

        private void mnuTwitter_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.ShowTwitter();
        }

        private void CmdUseSemiLogScaleClick(object sender, CommandEventArgs e)
        {
            mnuViewScaleType_Click(sender, e);
        }

        private void CmdShowXGridClick(object sender, CommandEventArgs e)
        {
            mnuViewShowXGrid_Click(sender, e);
        }

        private void CmdShowYGridClick(object sender, CommandEventArgs e)
        {
            mnuViewYGrid_Click(sender, e);
        }

        private void CmdShowPanelSeparatorsClick(object sender, CommandEventArgs e)
        {
            mnuViewSeparators_Click(sender, e);
        }

        private void CmdCrosshairClick(object sender, CommandEventArgs e)
        {
            if ((MActiveChart == null) || (MActiveChart.StockChartX1.CrossHairs))
            {
                _actionChart = ActionChart.CROSSHAIR;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuViewCrosshair_Click(sender, e);
        }

        private void CmdMagneticClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.MAGNETIC;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuViewMagnetic_Click(sender, e);
        }

        public void CmdCrosshairClick()
        {
            CmdCrosshairClick(null, null);
        }
        private void CmdSelectClick(object sender, CommandEventArgs e)
        {
            if ((MActiveChart == null) || (!MActiveChart.StockChartX1.CrossHairs))
            {
                _actionChart = ActionChart.SELECT;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuViewCrosshair_Click(sender, e);
        }

        public void CmdSelectClick()
        {
            CmdSelectClick(null, null);
        }
        private void CmdThreeDStyleClick(object sender, CommandEventArgs e)
        {
            mnuView3D_Click(sender, e);
        }

        private void CmdDarvasBoxesClick(object sender, CommandEventArgs e)
        {
            mnuDarvasBoxes_Click(sender, e);
        }

        private void CmdViewStarPageClick(object sender, CommandEventArgs e)
        {
            mnuStartPage_Click(sender, e);
        }

        private void CmdViewForexScreenClick(object sender, CommandEventArgs e)
        {
            MnuViewForexClick(sender, e);
        }

        private void CmdSellSymbolClick(object sender, CommandEventArgs e)
        {
            mnuSellSymbol_Click(sender, e);
        }

        private void CmdEllipseClick(object sender, CommandEventArgs e)
        {
            mnuEllipse_Click(sender, e);
        }

        private void CmdSpeedLinesClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.SPEEDLINES;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuSpeedLines_Click(sender, e);
        }

        public void CmdSpeedLinesClick()
        {
            CmdSpeedLinesClick(null, null);
        }
        private void CmdTironeLevelsClick(object sender, CommandEventArgs e)
        {
            mnuTironeLevels_Click(sender, e);
        }

        private void CmdQuadrantLinesClick(object sender, CommandEventArgs e)
        {
            mnuQuadrantLines_Click(sender, e);
        }

        private void CmdRaffRegressionClick(object sender, CommandEventArgs e)
        {
            mnuRaffRegression_Click(sender, e);
        }

        private void CmdErrorChannelsClick(object sender, CommandEventArgs e)
        {
            mnuErrorChannels_Click(sender, e);
        }

        private void CmdFreehandDrawingClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FREEHANDDRAWING;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFreeHandDrawing_Click(sender, e);
        }

        public void CmdFreeHandDrawingClick()
        {
            CmdFreeHandDrawingClick(null, null);
        }

        private void CbxApplicationStyleClick(object sender, CommandEventArgs e)
        {
            switch (cbxApplicationStyle.HostedControl.SelectedIndex)
            {
                case 0:
                    mnuAppOffice2007_Click(sender, e);
                    break;
                case 1:
                    mnuAppOfficeSilver_Click(sender, e);
                    break;
                case 2:
                    break;
                case 3:
                    mnuAppStandard_Click(sender, e);
                    break;
            }
        }

        private void MnuPriceIndicatorsToolbarClick(object sender, CommandEventArgs e)
        {
            ndtPriceIndicators.Visible = !ndtPriceIndicators.Visible.Equals(true);
            mnuPriceIndicatorsToolbar.Checked = ndtPriceIndicators.Visible.Equals(true);
        }

        private void MnuZoomTemplatesToolbarClick(object sender, CommandEventArgs e)
        {
            ndtZoom.Visible = !ndtZoom.Visible.Equals(true);
            mnuZoomTemplatesToolbar.Checked = ndtZoom.Visible.Equals(true);
        }

        private void MnuViewToolbarClick(object sender, CommandEventArgs e)
        {
            ndtView.Visible = !ndtView.Visible.Equals(true);
            mnuViewToolbar.Checked = ndtView.Visible.Equals(true);
        }

        private void MnuChartToolsToolbarClick(object sender, CommandEventArgs e)
        {
            ndtChartTools.Visible = !ndtChartTools.Visible.Equals(true);
            mnuChartToolsToolbar.Checked = ndtChartTools.Visible.Equals(true);
        }

        private void MnuFileToolbarClick(object sender, CommandEventArgs e)
        {
            ndtFile.Visible = !ndtFile.Visible.Equals(true);
            mnuFileToolbar.Checked = ndtFile.Visible.Equals(true);
        }

        #endregion

        #region Methods

        private void CmdCandleChartClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.ChangePriceStyle("Candle Chart");
        }

        private void CmdStockLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.ChangePriceStyle("StockLine");
        }

        private void CmdBarChartClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.ChangePriceStyle("Bar Chart");
        }

        private void CmdHeikinAshiClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.ChangePriceStyle("Heikin Ashi");
        }

        private void CmdHeikinAshiSmoothClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.ChangePriceStyle("Heikin Ashi Smooth");
        }

        public void CmdMagneticClick()
        {
            CmdMagneticClick(null, null);
        }

        private void CmdTextObjectClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.TEXTOBJECT;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuText_Click(sender, e);
        }

        public void CmdTextObjectClick()
        {
            CmdTextObjectClick(null, null);
        }

        private void CmdBuySymbolClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.BUYSYMBOL;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuBuySymbol_Click(sender, e);
        }

        public void CmdBuySymbolClick()
        {
            CmdBuySymbolClick(null, null);
        }

        private void SellSymbolClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.SELLSYMBOL;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuSellSymbol_Click(sender, e);
        }

        public void SellSymbolClick()
        {
            SellSymbolClick(null, null);
        }

        private void CmdExitSymbolClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.EXITSYMBOL;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuExitSymbol_Click(sender, e);
        }

        public void CmdExitSymbolClick()
        {
            CmdExitSymbolClick(null, null);
        }

        private void CmdTrendLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.TRENDLINE;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuTrendLine_Click(sender, e);
        }

        public void CmdTrendLineClick()
        {
            CmdTrendLineClick(null, null);
        }

        private void CmdRayClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.RAY;
                return;
            }
            _actionChart = ActionChart.NONE;
            MnuRayClick(sender, e);
        }

        public void CmdRayClick()
        {
            CmdRectangleClick(null, null);
        }

        private void CmdChannelClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
            {
                _actionChart = ActionChart.CHANNEL;
                return;
            }
            _actionChart = ActionChart.NONE;
            MnuChannelClick(sender, e);
        }

        public void CmdChannelClick()
        {
            CmdChannelClick(null, null);
        }

        private void MnuChannelClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            string dateTimeNow = Convert.ToString(DateTime.Now);
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH1 " + dateTimeNow);
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH2 " + dateTimeNow);
        }

        public void CmdHorizontalLineClick()
        {
            CmdHorizontalLineClick(null, null);
        }

        private void CmdHorizontalLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.XLINE;
                return;
            }
            _actionChart = ActionChart.NONE;

            //VINICIUS
            MnuHorizontalLineClick(sender, e);

            /*
            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = false;
            MActiveChart.DrawingLineStudy = false;
            GetHorizontalLine = true;*/



            //double m_Value = MActiveChart.StockChartX1.GetYValueByPixel(50);
            //string key = "hline" + DateTime.Now.Ticks;

            //MActiveChart.StockChartX1.DrawTrendLine(MActiveChart.StockChartX1.CurrentPanel,
            //  m_Value, (int)DataType.dtNullValue, m_Value, (int)DataType.dtNullValue, key);
            //MActiveChart.StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(MActiveChart.StockChartX1.ChartForeColor));
        }

        private void MnuHorizontalLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.AddUserXLine("HL " + DateTime.Now.Ticks);
        }

        private void MnuVerticalLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.AddUserYLine("VL " + DateTime.Now.Ticks);
        }

        public void CmdVerticalLineClick()
        {
            CmdVerticalLineClick(null, null);
        }

        private void CmdVerticalLineClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.YLINE;
                return;
            }
            _actionChart = ActionChart.NONE;

            //VINICIUS
            MnuVerticalLineClick(sender, e);
            //GetVerticaoLine = true;


            //var m_Record = 300;
            //string key = "vline" + DateTime.Now.Ticks;

            //MActiveChart.StockChartX1.DrawTrendLine(MActiveChart.StockChartX1.CurrentPanel,
            //  (double)DataType.dtNullValue, m_Record, (double)DataType.dtNullValue, m_Record, key);
            //MActiveChart.StockChartX1.set_ObjectColor(ObjectType.otTrendLineObject, key, (uint)ColorTranslator.ToOle(MActiveChart.StockChartX1.ChartForeColor));
        }

        private void CmdRectangleClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.RECTANGLE;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuRectangle_Click(sender, e);
        }

        public void CmdRectangleClick()
        {
            CmdRectangleClick(null, null);
        }

        private void CmdElipseClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
            {
                _actionChart = ActionChart.ELIPSE;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuElipse_Click(sender, e);
        }

        public void CmdElipseClick()
        {
            CmdElipseClick(null, null);
        }

        private void mnuElipse_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsEllipse, "EL " + Convert.ToString(DateTime.Now));
        }

        private void CmdArrowClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.ARROW;
                return;
            }

            _actionChart = ActionChart.NONE;
            mnuArrow_Click(sender, e);
        }

        public void CmdArrowClick()
        {
            CmdArrowClick(null, null);
        }

        private void CmdPolylineClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
            {
                _actionChart = ActionChart.POLYLINE;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuPolyline_Click(null, null);
        }

        public void CmdPolylineClick()
        {
            CmdPolylineClick(null, null);
        }

        private void mnuPolyline_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsPolyline, "PL " + Convert.ToString(DateTime.Now));
        }

        private void CmdFreeHandDrawingClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FREEHANDDRAWING;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFreeHandDrawing_Click(sender, e);
        }

        private void mnuFreeHandDrawing_Click(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFreehand, "FH " + Convert.ToString(DateTime.Now));
        }

        private void CmdFibonacciRetracementsClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FIBONACCIRETRACEMENTS;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFibonacciRetracements_Click(sender, e);
        }

        public void CmdFibonacciRetracementsClick()
        {
            CmdFibonacciRetracementsClick(null, null);
        }

        private void CmdFibonacciProjectionsClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FIBONACCIPROJECTIONS;
                return;
            }
            _actionChart = ActionChart.NONE;
            MnuFibonacciProjectionsClick(sender, e);
        }

        public void CmdFibonacciProjectionsClick()
        {
            CmdFibonacciProjectionsClick(null, null);
        }

        private void MnuFibonacciProjectionsClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
                return;

            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsFibonacciProgression, "FP" + Convert.ToString(DateTime.Now));
        }

        private void CmdFibonacciArcsClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FIBONACCIARCS;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFibonacciArcs_Click(sender, e);
        }

        public void CmdFibonacciArcsClick()
        {
            CmdFibonacciArcsClick(null, null);
        }

        private void CmdFibonacciFanClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FIBONACCIFAN;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFibonacciFan_Click(sender, e);
        }

        public void CmdFibonacciFanClick()
        {
            CmdFibonacciFanClick(null, null);
        }

        private void CmdFibonacciTimeZonesClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.FIBONACCITIMEZONES;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuFibonacciTimeZones_Click(sender, e);
        }

        public void CmdFibonacciTimeZonesClick()
        {
            CmdFibonacciTimeZonesClick(null, null);
        }

        private void CmdGannFanClick(object sender, CommandEventArgs e)
        {
            if (MActiveChart == null)
            {
                _actionChart = ActionChart.GANNFAN;
                return;
            }
            _actionChart = ActionChart.NONE;
            mnuGannFan_Click(sender, e);
        }

        public void CmdGannFanClick()
        {

            CmdGannFanClick(null, null);
        }

        private void MnuRayClick(object sender, CommandEventArgs e)
        {

            if (MActiveChart == null)
                return;

            _actionChart = ActionChart.NONE;
            MActiveChart.MUserEditing = true;
            MActiveChart.DrawingLineStudy = true;

            //trocar o nome do parametro lsTriangle para lsRay
            MActiveChart.StockChartX1.DrawLineStudy(StudyType.lsRay, "RL " + Convert.ToString(DateTime.Now));
        }

        private void nCmdHorizontalLine_Click(object sender, CommandEventArgs e)
        {
            CmdHorizontalLineClick(sender, e);
        }

        private void nCmdVerticalLine_Click(object sender, CommandEventArgs e)
        {
            CmdVerticalLineClick(sender, e);
        }

        private void nCmdRectangle_Click(object sender, CommandEventArgs e)
        {
            CmdRectangleClick(sender, e);
        }

        private void nCmdElipse_Click(object sender, CommandEventArgs e)
        {
            CmdElipseClick(sender, e);
        }

        private void nCmdArrow_Click(object sender, CommandEventArgs e)
        {
            CmdArrowClick(sender, e);
        }

        private void nCmdPolyline_Click(object sender, CommandEventArgs e)
        {
            CmdPolylineClick(sender, e);
        }

        private void nCmdFibonacciArcs_Click(object sender, CommandEventArgs e)
        {
            CmdFibonacciArcsClick(sender, e);
        }

        private void nCmdFibonacciFan_Click(object sender, CommandEventArgs e)
        {
            CmdFibonacciFanClick(sender, e);
        }

        private void nCmdFibonacciTimeZones_Click(object sender, CommandEventArgs e)
        {
            CmdFibonacciTimeZonesClick(sender, e);
        }

        private void nCmdGannFan_Click(object sender, CommandEventArgs e)
        {
            CmdGannFanClick(sender, e);
        }

        private void nCmdSpeedLines_Click(object sender, CommandEventArgs e)
        {
            CmdSpeedLinesClick(sender, e);
        }

        private void CmdChartClick(object sender, CommandEventArgs e)
        {
            ChartSelection selection = (new FrmSelectChart()).GetChartSelection();

            if (selection == null)
                return;
            measureTime.Start();
            MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { selection.Symbol, Periodicity.Daily.GetHashCode() }));
            AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.CreateNewCtlPainelChart,
                                                       new object[] {selection}));

            //CreateNewCtlPainel(selection);
        }

        #endregion

        public enum ActionChart
        {
            NONE,
            SELECT,
            CROSSHAIR,
            MAGNETIC,
            TEXTOBJECT,
            BUYSYMBOL,
            SELLSYMBOL,
            EXITSYMBOL,
            TRENDLINE,
            RAY,
            CHANNEL,
            HORIZONTALLINE,
            VERTICALLINE,
            RECTANGLE,
            ELIPSE,
            ARROW,
            POLYLINE,
            FREEHANDDRAWING,
            FIBONACCIRETRACEMENTS,
            FIBONACCIPROJECTIONS,
            FIBONACCIARCS,
            FIBONACCIFAN,
            FIBONACCITIMEZONES,
            GANNFAN,
            SPEEDLINES,
            XLINE,
            YLINE
        }

        private void MnuSettingsClick(object sender, CommandEventArgs e)
        {
            LoadFrmSettings();
        }

        public void LoadFrmSettings()
        {
            new FrmSettings(this).ShowDialog();
        }

        public void LoadDarvasBoxes()
        {
            bool _checked = MActiveChart.StockChartX1.DarvasBoxes;
            mnuDarvasBoxes.Checked = !_checked;
            MActiveChart.StockChartX1.DarvasBoxes = mnuDarvasBoxes.Checked;

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowXGrid()
        {
            bool @checked = !MActiveChart.StockChartX1.XGrid;
            mnuViewShowXGrid.Checked = @checked;
            MActiveChart.StockChartX1.XGrid = @checked;

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowYGrid()
        {
            bool @checked = !MActiveChart.StockChartX1.YGrid;
            mnuViewYGrid.Checked = @checked;
            MActiveChart.StockChartX1.YGrid = @checked;

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewSeparators()
        {
            bool @checked = !MActiveChart.StockChartX1.HorizontalSeparators;
            mnuViewSeparators.Checked = @checked;
            MActiveChart.StockChartX1.HorizontalSeparators = @checked;

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewScaleType()
        {
            bool @checked = MActiveChart.StockChartX1.ScaleType == ScaleType.stLinearScale;
            mnuViewScaleType.Checked = !@checked;
            MActiveChart.StockChartX1.ScaleType = !@checked ? ScaleType.stLinearScale : ScaleType.stSemiLogScale;
            MActiveChart.StockChartX1.ResetYScale(0);

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewCrosshair()
        {
            bool @checked = !cmdCrosshair.Checked;

            cmdCrosshair.Checked = mnuViewCrosshair.Checked = @checked;
            MActiveChart.StockChartX1.CrossHairs = @checked;
            cmdSelect.Checked = !@checked;

            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadPopoutChart()
        {
            if (ListPopout == null)
                ListPopout = new List<frmPopoutChart>();

            var chartExist = ListPopout.Any(cada => cada.MActiveChart.Parent == MActiveChart.Parent);

            if (chartExist)
                return;

            frmPopoutChart popout = new frmPopoutChart
            {
                _myParent = MActiveChart.Parent,
                Text = MActiveChart.Title,
                Width = (Width * 80) / 100,
                Height = (Height * 80) / 100,
                IdTab = m_DockManager.DocumentManager.ActiveDocument.ID
            };

            ListPopout.Add(popout);

            MActiveChart.Parent = popout;
            popout.Show();
        }

        public void LoadViewManagerWorkspace()
        {
            FrmWorkspace frmWorkspace = new FrmWorkspace(this);

            foreach (NCommand nCommand in mnuWorkspace.Commands.Cast<NCommand>().Where(nCommand => nCommand.Checked))
            {
                frmWorkspace.WorkspaceLoaded = nCommand.Properties.Text;
            }

            if (String.IsNullOrEmpty(frmWorkspace.WorkspaceLoaded))
                frmWorkspace.WorkspaceLoaded = "Plena";

            frmWorkspace.ShowDialog();

            MActiveChart.LoadWorkspaceRight();
        }

        public void LoadSchemeClick(string value)
        {
            foreach (KeyValuePair<string, string> scheme in
                Scheme.Instance().Schemes.Where(scheme => scheme.Value.Equals(value)))
            {
                MActiveChart.m_SchemeColor = scheme.Key;
            }

            Scheme.Instance().UpdateChartColors(MActiveChart.StockChartX1, MActiveChart.m_SchemeColor);

            foreach (NCommand nCommand in mnuAppStyle.Commands)
                nCommand.Checked = nCommand.Properties.Text.ToUpper().Equals(value.ToUpper());

            MActiveChart.LoadCheckMenuRight();
        }
        
        private void CheckAlerts()


        {
            int delIndex = -1;
            _messageRequests = MessageService.GetRequest(MSRequestOwner.M4);
            MSRequest request;
            // Process messages:
            while( _messageRequests.Count>0)

            {
                delIndex = -1;
                request = _messageRequests.Dequeue();
                switch(request.MSType)

                {
                    case MSRequestType.SummaryCreated:
                        break;
                    case MSRequestType.GetSymbolsList:
                        
                        lock (_locker)
                        {
                            foreach (Operations reqOp in requestedOperations)
                            {
                                if (reqOp.OID == request.ID)
                                {
                                    InitializeSymbols();
                                    delIndex = requestedOperations.IndexOf(reqOp);
                                }
                            }
                            if (delIndex != -1) requestedOperations.RemoveAt(delIndex);
                        }
                        break;
                    case MSRequestType.GetHistoricalData:
                        if(request.MSStatus==MSRequestStatus.Failed)
                        {
                            //TODO: tell user there's no data for symbol!
                            MessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgInvalidPeriodicity"] +
                                            request.MSParams[0]);
                        }
                        else if(request.MSStatus == MSRequestStatus.Processing)
                        {
                            //TODO: tell user that server is getting data!
                        }
                        else if(request.MSStatus == MSRequestStatus.Done)

                        {
                            lock (_locker)
                            {
                                foreach (Operations reqOp in requestedOperations)
                                {
                                    if (reqOp.OID == request.ID)
                                    {
                                        delIndex = requestedOperations.IndexOf(reqOp);
                                        switch (reqOp.OType)
                                        {
                                            case TypeOperations.CreateNewCtlPainelChart:
                                                timeEllapsedRequest = measureTime.ElapsedMilliseconds;
                                                CreateNewCtlPainel((ChartSelection) reqOp.OParams[0]);
                                                break;
                                            case TypeOperations.LoadCtlPainelChart:
                                                MActiveChart = new CtlPainelChart(this, _mCtlData,
                                                                                  (string) reqOp.OParams[1],
                                                                                  (Periodicity) reqOp.OParams[2],
                                                                                  (int) reqOp.OParams[3],
                                                                                  (int) reqOp.OParams[4],
                                                                                  (string) reqOp.OParams[5])
                                                                   {Dock = DockStyle.Fill};
                                                LoadChartSettings(MActiveChart);
                                                foreach (NUIDocument document in m_DockManager.DocumentManager.Documents
                                                    )
                                                {
                                                    if (document.ID == (System.Guid) reqOp.OParams[0])
                                                    {
                                                        document.Client = MActiveChart;
                                                        document.Manager.ActiveDocument = document;
                                                        document.Text = MActiveChart.GetChartTitle();
                                                    }
                                                }
                                                break;
                                        }
                                        _evArrived.Set();
                                    }
                                }
                                if (delIndex != -1 && requestedOperations.Count>0) requestedOperations.RemoveAt(delIndex);
                            }




                        }
                        break;
                }
            }
        }

        private void tmrMessageService_Tick(object sender, EventArgs e)
        {
            tmrMessageService.Enabled = false;
            CheckAlerts();
            tmrMessageService.Enabled = true;
        }
        
        public void AddRequestedOperation(Operations operation)
        {
            Thread threadReq = new Thread(()=>ThreadRequestedOp(operation));
            threadReq.IsBackground = true;
            threadReq.Start();
        }

        private void ThreadRequestedOp(Operations operation)
        {
            lock (_locker)
            {
                requestedOperations.Add(operation);
                _messageRequestID++;
            }
        } 

        public static List<SymbolsPS>GetStockList()

        {
            List<SymbolsPS> Result = new List<SymbolsPS>();
            lock(StockList)
            {
                Result = StockList;
            }
            return Result;
        }

        public static List<SymbolGroup> GetUserPortfolios()
        {
            List<SymbolGroup> result = new List<SymbolGroup>();
            lock (Portfolios)
            {
                if(Portfolios.Count<=0)
                {
                    Connection _connection = DBSymbolShared.Connect();
                    Portfolios = DBSymbolShared.LoadGroups(GroupType.Index, _connection);
                    Portfolios.AddRange(DBSymbolShared.LoadGroups(GroupType.Portfolio,_connection));
                }
                result = Portfolios;
            }
            return result;
        }

        public static void SaveUserPortfolios(List<SymbolGroup> Portfolios)
        {
            Connection _connection = DBSymbolShared.Connect();
            DBSymbolShared.Clear(typeof(SymbolGroup), _connection);
            DBSymbolShared.SaveGroups(Portfolios, _connection);
            DBSymbolShared.Disconnect(_connection);
        }

        public static void RemoveUserPortfolio(string name)
        {
            Connection _connection = DBSymbolShared.Connect();
            DBSymbolShared.RemoveGroup(name, _connection);
            DBSymbolShared.Disconnect(_connection);
        }

        public static BarData GetLastBarData(string Symbol)
        {
            BarData result;
            Connection _connection = DBDailyShared.Connect();
            result = DBDailyShared.GetLastBarDataDiskOrMemory(Symbol,BaseType.Days,_connection);
            DBDailyShared.Disconnect(_connection);
            return result;
            return result;


        }

        public void ReconfigureTabsChart()
        {
            foreach (NUIDocument t in _mDocMan.Documents)
            {
                switch (t.Client.Name)
                {
                    case "CtlPainelChart":
                        {
                            CtlPainelChart chart = (CtlPainelChart)t.Client;

                            if (chart != null)
                                chart.ReconfigureTabs();
                        }
                        break;
                }
            }
        }

        
    }
}