using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using M4.AsyncOperations;
using M4.DataServer.Interface;
using M4.M4v2.Authentication.Login;
using M4.M4v2.Chart;
using M4.M4v2.GridviewRowDetailsExtended;
using M4.M4v2.Portfolio;
using M4.M4v2.Settings;
using M4.M4v2.Workspace;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;
using M4Data.MessageService;
using STOCKCHARTXLib;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using M4.DataServer.Interface.ProtocolStructs;
using GroupType = M4.DataServer.Interface.GroupType;
using Periodicity = M4Core.Entities.Periodicity;
using M4.modulusfe.platform;
using System.Data.SqlClient;
using System.Collections;
using M4Data;
using System.Dynamic;
using VersionChecker;
using System.Net;
using M4.M4v2.Alerts;
using M4.M4v2.Scanner;
using M4.M4v2.BackTest;
using M4.M4v2.Scripts;
//using Facebook.Schema;
//using Facebook.Utility;
//using Facebook.Winforms.Components;

using Facebook;
using M4.M4v2.Facebook;
using System.Drawing.Imaging;

namespace M4
{
    public partial class frmMain2 : Form
    {
        #region Members
        //Themes Telerik
        private bool _preLoadEnd = false;
        private ToolWindow statusManager = new ToolWindow();
        private AquaTheme AquaTheme = new AquaTheme();
        private BreezeTheme BreezeTheme = new BreezeTheme();
        private DesertTheme DesertTheme = new DesertTheme();
        private HighContrastBlackTheme HighContrastBlackTheme = new HighContrastBlackTheme();
        private Office2010BlackTheme Office2010BlackTheme = new Office2010BlackTheme();
        private TelerikMetroBlueTheme TelerikMetroBlueTheme = new TelerikMetroBlueTheme();
        private TelerikMetroTheme TelerikMetroTheme = new TelerikMetroTheme();
        private Windows7Theme Windows7Theme = new Windows7Theme();
        private Windows8Theme Windows8Theme = new Windows8Theme();
        private VisualStudio2012DarkTheme VisualStudio2012DarkTheme = new VisualStudio2012DarkTheme();
        private VisualStudio2012LightTheme VisualStudio2012LightTheme = new VisualStudio2012LightTheme();
        private Point WebInitialPoint;
        private Size WebInitialSize;

        //Check once per minute to see if its a new day, then clear alerts
        private static int DockWindowID = 0;
        private static DateTime lastDate = DateTime.Now.Date;
        private readonly AutoResetEvent _evArrived = new AutoResetEvent(false);
        public DockWindowCollection documentManager;
        private string _mActiveDocumentName; //The active document name (ctlChart, ctlOrder, etc.)
        private string _archiveNameWorkspace;
        private StatusManager _statusManager;
        public ctlData _mCtlData;
        private FrmLogin _frmLogin;
        public bool _mCrossHairToggle = false;
        public bool _mSelectToggle = false;
        public bool _mDeltaToggle = false;
        public bool _mMagneticToggle = false;
        private static List<Symbol> StockList = new List<Symbol>();
        private static List<SymbolGroup> Portfolios = new List<SymbolGroup>();
        private List<Operations> requestedOperations = new List<Operations>();
        private static readonly object _locker = new object();
        private readonly AsyncOperation _asyncOperation;
        private readonly string _mCmdArg; //The command line string

        public int QtyLastViewport = 0;
        public string SchemeLastChart;
        public bool UseLastChartVisual = false;
        public double JDateChartViewport;
        public static frmMain2 GInstance;
        public CtlPainelChartList ChartsList;
        public static string ClientId = "FROEDE"; //TODO: enter your Modulus client id here
        public static string ClientPassword = "847263854"; //TODO: enter your Modulus client password here
        public static string ClientTitle = "PLENA"; //TODO: enter the title of your application here
        public static string LicenseKey;
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\BASE\\PORTFOLIO\\Summary.xml";
        public string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\BASE\\";
        public int AmountWallet { get; set; }
        public int _messageRequestID = 0;
        public ActionChart _actionChart;
        public CtlPainelChart MActiveChart;
        public ctlWeb _web;
        public ctlAlert _alerts;
        public ctlScanner _scanner;
        public ctlBackTest _backTest;
        public ctlScripts _scriptEditor;
        public ConfigStudies configStudies;
        public Stopwatch measureTime = new Stopwatch();
        public long timeEllapsedRequest = 0;
        public long timeEllapsedDatabase = 0;
        public long timeEllapsedDatabaseAccess = 0;
        public long timeEllapsedLoading = 0;
        public long timeEllapsed = 0;
        public bool GetHorizontalLine { get; set; }
        public bool GetVerticaoLine { get; set; }
        public bool TweetTrades;
        public string m_Style;
        private Queue<MSRequest> _messageRequests = new Queue<MSRequest>();

        //Facebook Service 6.0.0
        public static bool facebookLogged = false;
        public static readonly string facebookAppId = "1520253931529844";
        private static string[] facebookPermissions = new[] { "public_profile", "user_friends" };
        private static FacebookLoginDialog facebookLoginDialog;
        private static string facebookAccessToken = "";

        //Facebook Service 3.0.1
        /*public static readonly FacebookService fbService = new FacebookService();
        public static bool facebookLogged = false;
        private static List<long> facebookFriendsUids;
        private static List<Point> facebookFriendsPositions;*/

#pragma warning disable 649
        private ctlPortfolio _mCtlPortfolio;
#pragma warning restore 649
        #endregion

        #region Initialization

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
                    Telerik.WinControls.RadMessageBox.Show("Invalid license key.", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
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
                    Telerik.WinControls.RadMessageBox.Show("License key expired.", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                }
                else if (se.Message == "License key not activated")
                {
                    Telerik.WinControls.RadMessageBox.Show("Invalid license key.", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                    Properties.Settings.Default.LSTR = "";
                    Properties.Settings.Default.Save();
                    Environment.Exit(0);
                    return false;
                }
                else
                {
                    Telerik.WinControls.RadMessageBox.Show("Failed to connect to web service.", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
                }
                Environment.Exit(0);
            }
            return true;
        }

        private void TranslateText()
        {

            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            _selectview.Text = Program.LanguageDefault.DictionarySelectTools["selectTools"];
            statusManager.Text = Program.LanguageDefault.DictionaryPlena["statusManager"];
            statusManager.Name = Program.LanguageDefault.DictionaryPlena["statusManager"];
            statusManager.AccessibleName = Program.LanguageDefault.DictionaryPlena["statusManager"];
            mnuFile.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuFile"];

            cbxApplicationStyle.Text = Program.LanguageDefault.DictionaryMenuBar["cbxApplicationStyle"];

            cmdChart.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdChart"];
            cmdChart.Text = Program.LanguageDefault.DictionaryMenuBar["cmdChart"];
            cmdChart.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdChart"];
            cmdUseSemiLogScale.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            cmdUseSemiLogScale.Text = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            cmdUseSemiLogScale.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdUseSemiLogScale"];
            cmdShowXGrid.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdShowXGrid"];
            cmdShowXGrid.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowXGrid"];
            cmdShowXGrid.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowXGrid"];
            cmdShowYGrid.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdShowYGrid"];
            cmdShowYGrid.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowYGrid"];
            cmdShowYGrid.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowYGrid"];
            cmdShowPanelSeparators.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            cmdShowPanelSeparators.Text = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            cmdShowPanelSeparators.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdShowPanelSeparators"];
            cmdThreeDStyle.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdThreeDStyle"];
            cmdThreeDStyle.Text = Program.LanguageDefault.DictionaryMenuBar["cmdThreeDStyle"];
            cmdThreeDStyle.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdThreeDStyle"];
            cmdDarvasBoxes.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            cmdDarvasBoxes.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            cmdDarvasBoxes.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdDarvasBoxes"];
            cmdViewStarPage.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdViewStarPage"];
            cmdViewStarPage.Text = Program.LanguageDefault.DictionaryMenuBar["cmdViewStarPage"];
            cmdViewStarPage.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdViewStarPage"];
            cmdViewForexScreen.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdViewForexScreen"];
            cmdViewForexScreen.Text = Program.LanguageDefault.DictionaryMenuBar["cmdViewForexScreen"];
            cmdViewForexScreen.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdViewForexScreen"];

            cmdTextObject.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdTextObject"];
            cmdTextObject.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTextObject"];
            cmdTextObject.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdTextObject"];
            cmdBuySymbol.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdBuySymbol"];
            cmdBuySymbol.Text = Program.LanguageDefault.DictionaryMenuBar["cmdBuySymbol"];
            cmdBuySymbol.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdBuySymbol"];
            cmdExitSymbol.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdExitSymbol"];
            cmdExitSymbol.Text = Program.LanguageDefault.DictionaryMenuBar["cmdExitSymbol"];
            cmdExitSymbol.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdExitSymbol"];
            cmdTrendLine.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdTrendLine"];
            cmdTrendLine.Text = Program.LanguageDefault.DictionaryMenuBar["cmdTrendLine"];
            cmdTrendLine.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdTrendLine"];
            cmdFibonacciArcs.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciArcs"];
            cmdFibonacciArcs.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciArcs"];
            cmdFibonacciArcs.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciArcs"];
            cmdFibonacciRetracements.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciRetracements"];
            cmdFibonacciRetracements.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciRetracements"];
            cmdFibonacciRetracements.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciRetracements"];
            cmdFibonacciFan.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciFan"];
            cmdFibonacciFan.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciFan"];
            cmdFibonacciFan.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciFan"];
            cmdGannFan.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdGannFan"];
            cmdGannFan.Text = Program.LanguageDefault.DictionaryMenuBar["cmdGannFan"];
            cmdGannFan.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdGannFan"];
            cmdSpeedLines.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdSpeedLines"];
            cmdSpeedLines.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSpeedLines"];
            cmdSpeedLines.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdSpeedLines"];
            cmdRectangle.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdRectangle"];
            cmdRectangle.Text = Program.LanguageDefault.DictionaryMenuBar["cmdRectangle"];
            cmdRectangle.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdRectangle"];
            cmdArrow.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdArrow"];
            cmdArrow.Text = Program.LanguageDefault.DictionaryMenuBar["cmdArrow"];
            cmdArrow.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdArrow"];
            cmdEllipse.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdEllipse"];
            cmdEllipse.Text = Program.LanguageDefault.DictionaryMenuBar["cmdEllipse"];
            cmdEllipse.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdEllipse"];
            cmdCrosshair.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdCrosshair"];
            cmdCrosshair.Text = Program.LanguageDefault.DictionaryMenuBar["cmdCrosshair"];
            cmdCrosshair.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdCrosshair"];
            cmdHorizontalLine.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdHorizontalLine"];
            cmdHorizontalLine.Text = Program.LanguageDefault.DictionaryMenuBar["cmdHorizontalLine"];
            cmdHorizontalLine.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdHorizontalLine"];

            cmdSelect.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdSelect"];
            cmdSelect.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSelect"];
            cmdSelect.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdSelect"];
            cmdDeltaCursor.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            cmdDeltaCursor.Text = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            cmdDeltaCursor.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdDeltaCursor"];
            cmdMagnetic.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdMagnetic"];
            cmdMagnetic.Text = Program.LanguageDefault.DictionaryMenuBar["cmdMagnetic"];
            cmdMagnetic.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdMagnetic"];
            cmdSellSymbol.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdSellSymbol"];
            cmdSellSymbol.Text = Program.LanguageDefault.DictionaryMenuBar["cmdSellSymbol"];
            cmdSellSymbol.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdSellSymbol"];
            cmdRay.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdRay"];
            cmdRay.Text = Program.LanguageDefault.DictionaryMenuBar["cmdRay"];
            cmdRay.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdRay"];
            cmdChannel.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdChannel"];
            cmdChannel.Text = Program.LanguageDefault.DictionaryMenuBar["cmdChannel"];
            cmdChannel.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdChannel"];
            cmdVerticalLine.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdVerticalLine"];
            cmdVerticalLine.Text = Program.LanguageDefault.DictionaryMenuBar["cmdVerticalLine"];
            cmdVerticalLine.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdVerticalLine"];
            cmdPolyline.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdPolyline"];
            cmdPolyline.Text = Program.LanguageDefault.DictionaryMenuBar["cmdPolyline"];
            cmdPolyline.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdPolyline"];
            cmdFibonacciProjections.DisplayName = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciProjections"];
            cmdFibonacciProjections.Text = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciProjections"];
            cmdFibonacciProjections.ToolTipText = Program.LanguageDefault.DictionaryMenuBar["cmdFibonacciProjections"];

            mnuChartToolsToolbar.Text = Program.LanguageDefault.DictionaryMenuBar["mnuChartToolsToolbar"];

            mnuFile.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuFile"];
            mnuFileSaveImage.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSaveChartImage"];
            mnuFilePrint.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPrintChart"];
            mnuExit.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuExit"];

            mnuWorkspace.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuWorkspace"];
            mnuManagerWorkspace.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuManager"];

            mnuHelp.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuHelp"];
            mnuAbout.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuAboutPLENA"];

            mnuOptions.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSettings"];

            mnuAbout.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuAboutPLENA"];

            mnuChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuChart"];

            mnuPopoutChart.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuPopoutActiveChart"];

            mnuView.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuMenuView"];

            mnuChartToolsToolbar.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuChartTools"];
            mnuAppStyle.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuApplicationStyle"];
            mnuChartColors.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuChartColors"];
            mnuViewScaleType.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuUseSemiLogScale"];
            mnuViewShowXGrid.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowXGrid"];
            mnuViewYGrid.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowYGrid"];
            mnuViewSeparators.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuShowPanelSeparators"];
            mnuDarvasBoxes.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuDarvasBoxes"];
            mnuStartPage.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuViewStartPag"];
            mnuViewCrossHair.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuCrosshair"];
            mnuStatusManager.Text = Program.LanguageDefault.DictionaryPlena["statusManager"];
            mnuSelectTools.Text = Program.LanguageDefault.DictionarySelectTools["selectTools"];

            ndtChartTools.Text = Program.LanguageDefault.DictionaryMenuBar["ndtChartTools"];
            ndtChartTools.DisplayName = Program.LanguageDefault.DictionaryMenuBar["ndtChartTools"];

            mnuToolBar.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuToolbar"];
            

            //Translate and remove overflowbuttons
            if (Properties.Settings.Default.DictionaryLanguage == "PortugueseBrazil")
                commandBarRowElement3.Strips[0].OverflowButton.AddRemoveButtonsMenuItem.Text =
                    Program.LanguageDefault.DictionaryMenuPlena["mnuAddRemoveButtonsMenuItem"];
            commandBarRowElement3.Strips[0].OverflowButton.CustomizeButtonMenuItem.Visibility =
                Telerik.WinControls.ElementVisibility.Collapsed;
            //hide the separators
            foreach (var item in commandBarRowElement3.Strips[0].OverflowButton.DropDownMenu.Items)
            {
                RadMenuSeparatorItem separator = item as RadMenuSeparatorItem;
                if (separator != null)
                {
                    separator.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                }
            }

        }

        public frmMain2()
        {
            _asyncOperation = AsyncHelper.CreateOperation();

            InitializeComponent();

            TranslateText();

            documentTabStrip2.OverflowMenuButton.ActionButton.ToolTipText = "Gráficos";

            GInstance = this;
            CheckDirectoryFileWorkspace();

            //Old Behavior, now we have a stand-alone thread in Program:
            //VersionChecker.VersionChecker.UpdateFiles();

            LoadWorkspaces();
            LoadColorScheme(null);
        }

        public frmMain2(string LoadChart)
        {
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
            VersionChecker.VersionChecker.CheckAndCreateDirectories();
            //CheckDirectoryFileWorkspace();
            LoadWorkspaces();
            LoadColorScheme(null);
        }

        public void PreLoad(FrmLogin frmLogin)
        {
            try
            {
                _frmLogin = frmLogin;
                _frmLogin.LoadStatus("Procurando atualizações disponíveis...");

                //First check for login:
                if (!Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") && !Program.LoginAuthentication.Offline)
                {
                    try
                    {
                        bool optional = true;
                        List<string> newVersions = Server.Instance(Program.LanguageDefault).UpgradeVersion(Program.VERSION, out optional);
                        if (newVersions.Count > 0)
                        {

                            if (optional)
                            {
                                string message = "Está disponível uma nova versão do programa, deseja instalar agora?";
                                if (RadMessageBox.Show(message, "Software Update", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    //TODO: save patch's in Base and check for already downloaded?

                                    //Download new patch:
                                    using (WebClient Client = new WebClient())
                                    {
                                        FileInfo file = new FileInfo(newVersions.Last());
                                        Client.DownloadFile("http://" + Properties.Settings.Default.Server1_Ip + "/PlenaServer/Update/Update.ashx?file=" + newVersions.Last(), file.FullName);

                                        Process.Start(file.FullName);
                                        Environment.Exit(0);
                                    }
                                }
                            }
                            else
                            {
                                string message = "Está disponível uma nova versão do programa, a atualização será instalada agora.";
                                RadMessageBox.Show(message, "Software Update");
                                //TODO: save patch's in Base and check for already downloaded?

                                //Download new patch:
                                using (WebClient Client = new WebClient())
                                {
                                    FileInfo file = new FileInfo(newVersions.Last());
                                    Client.DownloadFile("http://" + Properties.Settings.Default.Server1_Ip + "/PlenaServer/Update/Update.ashx?file=" + newVersions.Last(), file.FullName);

                                    Process.Start(file.FullName);
                                    Environment.Exit(0);
                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, " ");
                        
                    }
                }


                Visible = false;
                VersionChecker.VersionChecker.ChangeVersionOfAllFiles(BasePath, Program.VERSION);
               // string[] paths = XmlChecker.GetDifferentVersion(BasePath, "1.0.0.1");

                //Thread.Sleep(1000);
                
                int tryDatabase = 0;
                bool databaseOk = false;
                // Get local Symbol's List, try to connect first time on database:
                while (tryDatabase < 10)
                {
                    _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["OpeningDB"]);
                    if (!InitializeSymbols())
                    {
                        _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["ImpossibleAtMoment"]);
                        tryDatabase++;
                        Thread.Sleep(500);

                    }
                    else
                    {
                        databaseOk = true;
                        tryDatabase = 10;
                    }
                }
                //ABORT IF DATABASE IS OFF:
                if (!databaseOk)
                {
                    Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["ImpossibleAccessDB"], " ");

                    Environment.Exit(0);
                }
                // SYMBOLS LIST UPDATED: 
                //DBSymbolShared.UpdateSymbolsEvent += ReInitializeSymbols;

                _statusManager = new StatusManager { Dock = DockStyle.Fill };

               

                _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadingDB"]);

                /*MethodInvoker mk = delegate
                {
                    LoadWindowsManager();
                };
                mk.BeginInvoke(callbackfunction, null);
                 */

                _frmLogin.LoadStatus("Carregando portfólios...");
                LoadWindowsManager();

                mnuViewYGrid.IsChecked = true;
                mnuViewSeparators.IsChecked = true;
                mnuView3D.IsChecked = true;
                mnuViewCrossHair.IsChecked = false;
                cmdSelect.IsChecked = true;
                EnableControls(false);
                documentManager = radDock2.DockWindows; //All Windows, including Floating windows.
                //_frmLogin.LoadStatus("Update Style");
                //ManagerWorkspace.Instance().LoadTheme();

                BeginInvoke(new Action(() =>
                {
                    _frmLogin.LoadStatus("Carregando gráficos...");

                    ChartsList = new CtlPainelChartList();
                    ChartsList.FillList();
                }));

                //Load a chart file from a command line? 
                if (!string.IsNullOrEmpty(_mCmdArg))
                {
                    if (File.Exists(_mCmdArg))
                    {
                        _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadingChartCommands"]);

                        CtlPainelChart ctl = new CtlPainelChart(this, null, _mCmdArg) { Dock = DockStyle.Fill };
                        CreateCtlPanelChart(ctl.GetChartTitle(), ctl);
                        ctl.StockChartX1.LoadFile(_mCmdArg);
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
                    //Application.DoEvents();
                    Height = Screen.PrimaryScreen.Bounds.Height;
                    Width = Screen.PrimaryScreen.Bounds.Width;
                    WindowState = FormWindowState.Maximized;
                    Width = Properties.Settings.Default.LastSize.Width;
                    Height = Properties.Settings.Default.LastSize.Height;
                    //Application.DoEvents();
                }

                //Data manager window
                _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadingDM"]);

                _mCtlData = new ctlData(this) { Dock = DockStyle.Fill, StatusManager = _statusManager };

                _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["CheckingWorkspace"]);
                XmlDocument xmlDocumentWorkspace = new XmlDocument();
                xmlDocumentWorkspace.Load(ListWorkspace._path + "Workspace.xml");               
                XmlNodeList nodeList = xmlDocumentWorkspace.GetElementsByTagName("WORKSPACE");

                XmlNode node = nodeList.Cast<XmlNode>().Where(xmlNode => xmlNode["DEFAULT"].InnerText == "1" && !xmlNode["TEXT"].Equals("Plena")).
                    FirstOrDefault();

                //bool defaultLoadWorkspace;
                //defaultLoadWorkspace = ManagerWorkspace.Instance().Load(this);
                if (node != null)
                {
                    _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadWebBrowser"]);
                    MethodInvoker wk = delegate
                    {
                        //OpenURL(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"]);
                    };
                    wk.BeginInvoke(callbackfunction, null);
                    //Thread.Sleep(20000);
                    _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadLastWorkspace"]);
                    if (!node["TEXT"].InnerText.Contains("Plena"))
                    {
                        RestoreWorkspace(node["TEXT"].InnerText);
                        WorkspaceLoaded(node["TEXT"].InnerText);
                    }
                    else
                    {
                        //Caso não exista o workspace padrão, salvar para que possa ser carregado pelo usuário
                        if (!Directory.Exists(ListWorkspace._path + "Plena" + "\\"))
                        {
                            Visible = true;
                            Directory.CreateDirectory(ListWorkspace._path + "Plena" + "\\");
                            radDock2.SaveToXml(ListWorkspace._path + "Plena" + "\\" + "Plena.xml");
                            VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + "Plena" + "\\" + "Plena.xml", VersionChecker.VersionChecker.Version);
                        }
                        else if (!File.Exists(ListWorkspace._path + "Plena" + "\\" + "Plena.xml"))
                        {
                            Visible = true;
                            radDock2.SaveToXml(ListWorkspace._path + "Plena" + "\\" + "Plena.xml");
                            VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + "Plena" + "\\" + "Plena.xml", VersionChecker.VersionChecker.Version);
                        }
                    }
                    UpdateStyle();
                    AtivateDoubleClickTabs();
                    InitializeWindows();

                }
                else
                {
                    _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadWebBrowser"]);
                    MethodInvoker wk = delegate
                    {
                        //OpenURL(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"]);
                    };
                    wk.BeginInvoke(callbackfunction, null);
                    //Data manager window V2
                    AmountWallet = 1;

                    //    _frmLogin.LoadStatus("Loading Expert Advisors...");
                    //   LoadExpertAdvisors();

                    _frmLogin.LoadStatus(Program.LanguageDefault.DictionaryLogin["LoadWebBrowser"]);

                    TweetTrades = Properties.Settings.Default.TweetTrades == "1";

                    AtivateDoubleClickTabs();
                    InitializeWindows();
                    RestoreWorkspace("Plena");
                    WorkspaceLoaded("Plena");
                    UpdateStyle();


                }



            }
            catch (Exception Err)
            {
                Telerik.WinControls.RadMessageBox.Show(Err.Message, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error);
            } 


            Visible = true;

            if (!Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") && !Program.LoginAuthentication.Offline)
            {
                DSPlena.DSPlena.SaveServerIpAdress(Program.LoginAuthentication.IpServer);
                MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending,
                                                              MSRequestType.GetSymbolsList,
                                                              MSRequestOwner.M4,
                                                              new object[] { }));
                AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.InitializeSymbolDatabase,
                                                           new object[] { }));
            }

            string messageLogin = String.Format(Program.LanguageDefault.DictionaryLogin["messageLogin"], Program.LoginAuthentication.Login, Program.LoginAuthentication.Offline ? "OFFLINE" : "ONLINE");
            _statusManager.SetMessage(messageLogin, Program.LoginAuthentication.Offline ? OutputWindowV2.OutputIcon.Warning : OutputWindowV2.OutputIcon.Info);
           

            _preLoadEnd = true;

            mnuScript_Click(new object(), new EventArgs() );

        }

        private void callbackfunction(IAsyncResult res)
        {
        }

        private void frmMain2_Load(object sender, EventArgs e)
        {
            TranslateText();
            ContextMenuService menuService = this.radDock2.GetService<ContextMenuService>();
            menuService.ContextMenuDisplaying += new ContextMenuDisplayingEventHandler(menuService_ContextMenuDisplaying);

            mnuAppStyle.Visibility = ElementVisibility.Collapsed;

            FormatWindows();

            cmdChart.MouseDown += cmdChart_MouseDown;

        }

        void cmdChart_MouseDown(object sender, MouseEventArgs e)
        {
            //DoDragDrop("BBAS3",DragDropEffects.Copy);
        }

        void menuService_ContextMenuDisplaying(object sender, ContextMenuDisplayingEventArgs e)
        {

            if (Properties.Settings.Default.DictionaryLanguage == "PortugueseBrazil")
            {
                bool floating = false;
                if(e.DockWindow!=null)floating = e.DockWindow.IsInFloatingMode;
                for (int i = 0; i < e.MenuItems.Count; i++)
                {
                    RadMenuItemBase menuItem = e.MenuItems[i];
                    switch (menuItem.Name)
                    {
                        case "CloseWindow":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuCloseWindow"];
                            break;
                        case "CloseAllButThis":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuCloseAllButThis"];
                            break;
                        case "CloseAll":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuCloseAll"];
                            break;
                        case "Floating":
                            if (floating)
                            {
                                menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuFloating"];
                            }
                            else
                            {
                                menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuFloat"];
                            }
                            break;
                        case "Docked":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuDocked"]; 
                            break;
                        case "TabbedDocument":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuDocked"];
                            break;
                        case "AutoHide":
                            menuItem.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuAutoHide"];
                            break;
                    }


                }
            }
            if (e.DockWindow == null) return;
            //the menu request is associated with a valid DockWindow instance, which resides within a DocumentTabStrip
            if (e.MenuType == ContextMenuType.DockWindow &&
                e.DockWindow.DockTabStrip is DocumentTabStrip)
            {
                RadMenuItemBase SortWindow = new RadMenuItem { Text = Program.LanguageDefault.DictionaryMenuPlena["mnuSortWindow"] };
                SortWindow.Click += SortWindow_Click;
                e.MenuItems.Add(SortWindow);
                for (int i = 0; i < e.MenuItems.Count; i++)
                {
                    RadMenuItemBase menuItem = e.MenuItems[i];
                    if (menuItem.Name == "NewHTabGroup" ||
                        menuItem.Name == "NewVTabGroup" ||
                        menuItem.Name == "Docked" ||
                        menuItem.Name == "AutoHide" ||
                        menuItem.Name == "Hidden" /*||
                        /*menuItem is RadMenuSeparatorItem*/)
                    {
                        // In case you just want to disable to option you can set Enabled false
                        //menuItem.Enabled = false;
                        menuItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                    }
                }
            }
            else if (e.DockWindow.DockState == DockState.Floating)
            {
                for (int i = 0; i < e.MenuItems.Count; i++)
                {
                    RadMenuItemBase menuItem = e.MenuItems[i];
                    if ((menuItem.Name == "Docked" && e.DockWindow.AccessibleName == "CtlPainelChart") ||
                        (menuItem.Name == "TabbedDocument" && e.DockWindow.AccessibleName != "CtlPainelChart") ||
                        menuItem.Name == "AutoHide" ||
                        menuItem.Name == "Hidden" /*||
                        menuItem is RadMenuSeparatorItem*/)
                    {
                        // In case you just want to disable to option you can set Enabled false
                        //menuItem.Enabled = false;
                        menuItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                    }
                }
            }
            else
            {
                for (int i = 0; i < e.MenuItems.Count; i++)
                {
                    RadMenuItemBase menuItem = e.MenuItems[i];
                    if (menuItem.Name == "Hidden" ||
                        menuItem.Name == "TabbedDocument" /*||
                        /*menuItem is RadMenuSeparatorItem*/)
                    {
                        // In case you just want to disable to option you can set Enabled false
                        //menuItem.Enabled = false;
                        menuItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                    }
                }
            }
        }

        private void SortTabs(DocumentTabStrip radTabStrip)
        {

            TabPanelCollection tabArr = radTabStrip.TabPanels;
            QuickSort(tabArr);
            //TabPanel old = tabArr[1];
            //tabArr.RemoveAt(1);
            //TabPanel next = tabArr[0];
            //tabArr.RemoveAt(0);
            //tabArr.Add(old);
            //tabArr.Add(next);
            //radTabStrip.TabPanels.Clear();
            //radTabStrip.TabPanels.AddRange(tabArr);
        }

        private void QuickSort(TabPanelCollection Collection)
        {
            TabPanel[] panelsArr = new TabPanel[Collection.Count];

            //List<RadPanel> listPanels = new List<RadPanel>();
            int i = 0;
            foreach (TabPanel panel in Collection)
            {
                panelsArr[i] = Collection[i];
                i++;
            }
            if (Collection.ContainsKey("ctlWeb"))
            {
                int index = Collection.IndexOfKey("ctlWeb");
                Collection.Clear();
                if (index == 0)
                {
                    quicksort(panelsArr, 1, panelsArr.Length - 1);
                }
                else
                {
                    swap(panelsArr, 0, index);
                    quicksort(panelsArr, 1, panelsArr.Length - 1);
                }
            }
            else
            {
                Collection.Clear();
                quicksort(panelsArr, 0, panelsArr.Length - 1);
            }
            foreach (TabPanel newpanel in panelsArr)
            {
                Collection.Add(newpanel);
            }
            //listPanels.Sort();
            //foreach (RadPanel panel in listPanels)
            //{
            //    Collection.Add(panel);
            //}
            //return Collection;

        }
        private void quicksort(TabPanel[] input, int low, int high)
        {
            int pivot_loc = 0;

            if (low < high)
            {
                pivot_loc = partition(input, low, high);
                quicksort(input, low, pivot_loc - 1);
                quicksort(input, pivot_loc + 1, high);
            }
        }

        private int partition(TabPanel[] input, int low, int high)
        {
            TabPanel pivot = input[low];
            int i = low + 1;
            int f = high;

            while (i <= f)
            {
                if (input[i].Text.CompareTo(pivot.Text) <= 0)
                {
                    i++;
                }
                else if (pivot.Text.CompareTo(input[f].Text) < 0)
                {
                    f--;
                }
                else
                {
                    TabPanel swap = input[i];
                    input[i] = input[f];
                    input[f] = swap;
                    i++;
                    f--;
                }
            }
            input[low] = input[f];
            input[f] = pivot;
            return f;

        }



        private void swap(TabPanel[] ar, int a, int b)
        {
            TabPanel temp = ar[a];
            ar[a] = ar[b];
            ar[b] = temp;
        }


        void SortWindow_Click(object sender, EventArgs e)
        {
            if (radDock2.ActiveWindow.Parent is DocumentTabStrip)
            {
                SortTabs((DocumentTabStrip)radDock2.ActiveWindow.Parent);
            }

        }
        private bool InitializeSymbols()
        {
            //Get Symbols from database:
            //SqlConnection _connection = DBlocalSQL.Connect();
            //if (_connection == null) return false;
            //DBlocalSQL.Shrink(_connection);
            ReloadStockListPortfolios();
            return true;
        }

        public static void ReloadStockListPortfolios()
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            lock (Portfolios)
            {

                List<string> portAll = new List<string>();
                StockList = DBlocalSQL.LoadSymbols(_connection,true);
                foreach (Symbol symbol in StockList)
                {
                    portAll.Add(symbol.Code);
                }
                Portfolios = new List<SymbolGroup>() { new SymbolGroup(){
                                Index = 0,
                                Name = "All",
                                Symbols= "",
                                Type = 0 
                            }};
                Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Index, _connection));
                Portfolios.AddRange(DBlocalSQL.LoadGroups(GroupType.Portfolio, _connection));
                
                //Update portfolio All:
                foreach (SymbolGroup port in Portfolios)
                {
                    foreach (string s in port.Symbols.Split(new char[] { ',' }))
                    {
                        if (s != "" && !StockList.Exists(sy => sy.Code == s))
                        {
                            portAll.Add(s);
                            StockList.Add(new Symbol() { Code = s } );
                        }
                    }
                }
                portAll = portAll.OrderBy(s => s).ToList();
                StockList = StockList.OrderBy(s => s.Code).ToList();
                Portfolios[0].Symbols = string.Join(",", portAll.ToArray());
                DBlocalSQL.SaveSymbols(StockList,_connection,true);
            }
            DBlocalSQL.Disconnect(_connection);

        }

        private void ReInitializeSymbols(object sender, EventArgs args)
        {
            InitializeSymbols();
            ReloadSelectTools();
        }

        public void UpdateStyle()
        {


            if (m_Style == "" || m_Style == null)
            {
                m_Style = "VisualStudio2012Light";
            }
            //UpdateChartColors();
            M4v2.Themes.ChangeTheme.ChangeThemeName(m_Style);
            foreach (RadMenuItem menuItem in mnuAppStyle.Items)
            {
                menuItem.IsChecked = menuItem.AccessibleName == m_Style;
            }
        }


        public void LoadAlerts()
        {
            _alerts = new ctlAlert(this, _mCtlData);
            _toolAlerts.Controls.Add(_alerts);
            //_toolAlerts.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
            //_toolAlerts.AllowedDockState = AllowedDockState.TabbedDocument;
            //_toolAlerts.ToolCaptionButtons = ToolStripCaptionButtons.All & ToolStripCaptionButtons.Close;
            _toolAlerts.Text = "Alarmes";
        }
        public void LoadScanner()
        {
            _scanner = new ctlScanner(this, _mCtlData);
            _toolScanner.Controls.Add(_scanner);
            //_toolScanner.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
            //_toolScanner.AllowedDockState = AllowedDockState.TabbedDocument;
            //_toolScanner.ToolCaptionButtons = ToolStripCaptionButtons.All & ToolStripCaptionButtons.Close;
            _toolScanner.Text = "Scanner";
        }

        public void LoadBackTest()
        {
            _backTest = new ctlBackTest(this, _mCtlData);
            _toolBackTest.Controls.Add(_backTest);
            //_toolBackTest.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
            //_toolBackTest.AllowedDockState = AllowedDockState.TabbedDocument;
            //_toolBackTest.ToolCaptionButtons = ToolStripCaptionButtons.All & ToolStripCaptionButtons.Close;
            _toolBackTest.Text = "Back Test";
        }

        public void LoadScriptEditor()
        {
            _scriptEditor = new ctlScripts(this, _mCtlData);
            _toolScript.Controls.Add(_scriptEditor);
            //_toolBackTest.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
            //_toolBackTest.AllowedDockState = AllowedDockState.TabbedDocument;
            //_toolBackTest.ToolCaptionButtons = ToolStripCaptionButtons.All & ToolStripCaptionButtons.Close;
            _toolScript.Text = "Script Editor";
        }

        //Loads a URL in a web browser if it is not already displayed    
        public void OpenURL(string URL, string Title, bool test = false)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        _web = new ctlWeb(URL, Title,this) { Dock = DockStyle.Fill };
                        _toolWeb.Controls.Clear();
                        _toolWeb.Controls.Add(_web);
                        _toolWeb.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
                        _toolWeb.AllowedDockState = AllowedDockState.TabbedDocument;
                        _toolWeb.ToolCaptionButtons = ToolStripCaptionButtons.All & ~ToolStripCaptionButtons.Close;
                        WebInitialSize = _web.Size;
                        WebInitialSize.Width += 8;
                        WebInitialPoint = _toolWeb.PointToScreen(Point.Empty);

                    }));
                }
                else
                {
                    _web = new ctlWeb(URL, Title, this) { Dock = DockStyle.Fill };
                    _toolWeb.Controls.Clear();
                    _toolWeb.Controls.Add(_web);
                    _toolWeb.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
                    _toolWeb.AllowedDockState = AllowedDockState.TabbedDocument;
                    _toolWeb.ToolCaptionButtons = ToolStripCaptionButtons.All & ~ToolStripCaptionButtons.Close;
                    WebInitialSize = _web.Size;
                    WebInitialSize.Width += 8;
                    WebInitialPoint = _toolWeb.PointToScreen(Point.Empty);

                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("OpenUrl failed! " + ex.Message, " ");
            }
            /*
                BeginInvoke(new Action(()=>{
                try
                {
                    _web = new ctlWeb(URL, Title) {Dock = DockStyle.Fill};
                    ctlWeb.Controls.Add(_web);
                    ctlWeb.DefaultFloatingSize = new Size(this.Width - 100, this.Height - 100);
                    ctlWeb.AllowedDockState = AllowedDockState.TabbedDocument;
                    ctlWeb.ToolCaptionButtons = ToolStripCaptionButtons.All & ~ToolStripCaptionButtons.Close;
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show("OpenUrl failed!" + ex.Message);
                }
                }));
             * */


        }

        #endregion

        #region Portfolio Functions

        public void ReloadSelectTools()
        {
            while (_select == null) ; //wait
            _select.Clear();
            _select.LoadPortfolios();
        }

        public void ReloadSelectTools(List<string> updatePortfolios)
        {
            _select.LoadPortfolios(updatePortfolios);
        }
        
        public static List<SymbolGroup> GetUserPortfolios()
        {
            List<SymbolGroup> result = new List<SymbolGroup>();
            if (Portfolios.Count <= 0)
            {
                ReloadStockListPortfolios();
            }
            result = Portfolios;
            return result;
        }

        public static void RemoveUserPortfolio(string name)
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            DBlocalSQL.RemoveGroup(name, _connection);
            DBlocalSQL.Disconnect(_connection);
            lock (Portfolios)
            {
                Portfolios.Remove(Portfolios.First(p=>p.Name == name));
            }
        }

        public static void SaveUserPortfolios(List<SymbolGroup> Portfolios)
        {
            SqlConnection _connection = DBlocalSQL.Connect();
            DBlocalSQL.SaveGroups(Portfolios, _connection);
            DBlocalSQL.Disconnect(_connection);
        }

        public void CreatePortfolioTab(/*string portfolio*/)
        {
            _select.CreatePortfolioTab(/*portfolio*/);
        }

        public void RemovePortfolio(string name)
        {
            _select.RemovePortfolio(name);
        }
        #endregion

        #region Chart Functions
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

        public void LoadChartSettings(CtlPainelChart MActiveChart)
        {
            MActiveChart.StockChartX1.VolumePostfixLetter = "M";
            MActiveChart.StockChartX1.DisplayTitleBorder = true;
            MActiveChart.StockChartX1.ThreeDStyle = false;
            MActiveChart.StockChartX1.DarvasBoxes = mnuDarvasBoxes.IsChecked = false;

            //Right, Top and Bottom Space
            MActiveChart.SetChartPadding(Properties.Settings.Default.PaddingTop, Properties.Settings.Default.PaddingBottom, Properties.Settings.Default.PaddingRight);

            //Scale Type
            mnuViewScaleType.IsChecked = Properties.Settings.Default.SemiLogScale ;
            LoadViewScaleType();

            
            //Scale Precision
            MActiveChart.StockChartX1.ScalePrecision = Properties.Settings.Default.Decimals;
            //Show Horizontal Grid
            MActiveChart.StockChartX1.YGrid = Properties.Settings.Default.GridHorizontal;
            mnuViewYGrid.IsChecked = Properties.Settings.Default.GridHorizontal;
            //Show Vertical Grid
            MActiveChart.StockChartX1.XGrid = Properties.Settings.Default.GridVertical;
            mnuViewShowXGrid.IsChecked = Properties.Settings.Default.GridVertical;
            //Panel Separators
            MActiveChart.StockChartX1.HorizontalSeparators = Properties.Settings.Default.PanelSeparator;
            mnuViewSeparators.IsChecked = Properties.Settings.Default.PanelSeparator;
            //Price params

            MActiveChart.StockChartX1.SmoothHeikinPeriods = Properties.Settings.Default.SettingsHeikinSmoothPeriod;
            MActiveChart.StockChartX1.SmoothHeikinType = Properties.Settings.Default.SettingsHeikinSmoothType;
            MActiveChart.StockChartX1.PriceLineMono = Properties.Settings.Default.SettingsPriceLineMono;
            MActiveChart.StockChartX1.PriceLineThickness = Properties.Settings.Default.SettingsPriceLineThickness;
            MActiveChart.StockChartX1.PriceLineThicknessBar = Properties.Settings.Default.SettingsPriceBarLineThickness;

            //MActiveChart.StockChartX1.Freeze(true);
            //Visible Bars
            if (UseLastChartVisual)
            {
                UseLastChartVisual = false;
                //MActiveChart.StockChartX1.VisibleRecordCount = QtyLastViewport;
                MActiveChart.StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
            }
            else MActiveChart.StockChartX1.VisibleRecordCount = Properties.Settings.Default.ChartViewport;
            MActiveChart.StateDummy = false;
        }

        public void CreateCtlPanelChart(string caption, CtlPainelChart _panelChart)
        {
            DockWindow CtlWindow = new ToolWindow(caption)
            {
                AccessibleName = "CtlPainelChart",
                DockState = DockState.TabbedDocument,
                CloseAction = DockWindowCloseAction.Close,
               // DefaultFloatingSize = WebInitialSize,
                //AllowedDockState = AllowedDockState.All & ~AllowedDockState.Docked,
                Text = caption,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };
            try
            {
                MActiveChart = _panelChart;
                _panelChart.Padding = new Padding(0);
                _panelChart.Margin = new Padding(0);
                LoadColorScheme(null);
                CtlWindow.Controls.Add(_panelChart);
                DocumentTabStrip tabStrip = (DocumentTabStrip)radDock2.DocumentManager.ActiveDocument.Parent;
                CtlWindow.Move += CtlWindow_DockChanged;
                tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                CtlWindow.DockTo(radDock2.DocumentManager.ActiveDocument, DockPosition.Fill);
                CtlWindow.AccessibleDescription = CtlWindow.Name.Substring(10);

                CtlWindow.DockState = DockState.TabbedDocument;
                radDock2.ActivateWindow(CtlWindow);

               

                //  CtlWindow.DockManager.SelectedTabChanged -= radDock2_SelectedTabChanged;
                // CtlWindow.TabStripItem.FontChanged -= new EventHandler(TabStripItem_FontChanged);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
            }
            finally
            {
                //  CtlWindow.DockManager.SelectedTabChanged += radDock2_SelectedTabChanged;
                //  CtlWindow.TabStripItem.FontChanged += new EventHandler(TabStripItem_FontChanged);
            }
        }

        void TabStripItem_FontChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(((TabStripItem)sender).Text + " changed.");
            DockWindow SelectedWindow = (DockWindow)((TabStripItem)sender).TabPanel.Controls.Owner;
            bool enable = true;

            if (SelectedWindow.Controls.Count == 0 || SelectedWindow != SelectedWindow.DockManager.DocumentManager.ActiveDocument)
                return;


            //Disable the controls if not the same type of form
            if (_mActiveDocumentName != SelectedWindow.AccessibleName || SelectedWindow.AccessibleName != "CtlPainelChart")
            {
                enable = false;
            }
            if (!enable)
            {
                EnableControls(false);
            }
            if (SelectedWindow.AccessibleName == "ctlChart")
            {
                MActiveChart = (CtlPainelChart)SelectedWindow.Controls[0];
                MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);
                MActiveChart.LoadStockPortfolioActive();
            }
            else if (SelectedWindow.AccessibleName == "CtlPainelChart" && SelectedWindow.DockState != DockState.Floating)
            {
                //Console.WriteLine("MActiveChart " + SelectedWindow.Text + " selected.");
                MActiveChart = (CtlPainelChart)SelectedWindow.Controls[0];
                MActiveChart.DrawSelection();
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
            _mActiveDocumentName = SelectedWindow.AccessibleName;
        }

        ////Creates a new chart
        //public void CreateNewChart()
        //{

        //    if (_mCtlData == null) return;
        //    ChartSelection selection = (new frmSelectChart()).GetChartSelection();
        //    CreateNewCtlPainel(selection);
        //}

        // Creates a new chart from a selection
        public void CreateNewCtlPainel(ChartSelection selection, Action<CtlPainelChart> onCompleted = null)
        {
            try
            {
                if (_mCtlData == null)
                    return;

                if (string.IsNullOrEmpty(selection.Symbol))
                    return;
                _mCtlData.LoadRealTimeCtlPainelChartAsync2(selection, onCompleted ?? (chart =>
                {

                }));
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
            }
        }

        #endregion

        #region Tabs Functions
        /// <summary>
        /// Ativa o double click para as novas tabs criadas pelo controle do telerik. 
        /// </summary>
        private void AtivateDoubleClickTabs()
        {
            try
            {
                foreach (DockWindow window in radDock2.DocumentManager.DocumentArray)
                {
                    DocumentTabStrip tabStrip = (DocumentTabStrip)window.Parent;
                    try
                    {
                        tabStrip.DoubleClick -= TabStrip_DoubleClick;
                        // window.DockManager.SelectedTabChanged -= radDock2_SelectedTabChanged;
                        //  window.TabStripItem.FontChanged -= TabStripItem_FontChanged;
                    }
                    finally
                    {
                        tabStrip.DoubleClick += TabStrip_DoubleClick;
                        //  window.DockManager.SelectedTabChanged += radDock2_SelectedTabChanged;
                        //   window.TabStripItem.FontChanged += TabStripItem_FontChanged;
                    }
                    tabStrip.AllowDrop = true;
                    tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                    if (window.AccessibleName == "CtlPainelChart")
                        if (Convert.ToInt32(window.AccessibleDescription) > DockWindowID) DockWindowID = Convert.ToInt32(window.AccessibleDescription) + 1;
                }
            }
            catch (Exception exception)
            {
                Telerik.WinControls.RadMessageBox.Show(exception.Message + " at AtivateDoubleClickTabs()", " ");
            }
        }

        private void TabStrip_DoubleClick(object sender, EventArgs e)
        {
            DocumentTabStrip tabStrip = (DocumentTabStrip)sender;

            if (tabStrip.DockManager.ActiveWindow.AccessibleName == "CtlPainelChart")
            {
                CreateGhostWindow(tabStrip.DockManager.ActiveWindow);
            }

        }

        #endregion

        #region Windows Functions
        private void InitializeWindows()
        {
            foreach (DockWindow window in radDock2.DocumentManager.DocumentArray)
            {
                try
                {
                    window.Move -= CtlWindow_DockChanged;
                }
                finally
                {
                    window.Move += CtlWindow_DockChanged;
                }

            }
        }

        public DockWindow GetCtlActiveWindow()
        {
            return radDock2.DocumentManager.ActiveDocument;
        }

        public DockWindow GetCtlActiveWindowFloat()
        {
            if (!(radDock2.ActiveWindow.DockType == DockType.ToolWindow && radDock2.ActiveWindow.Name == "_selectview")) return radDock2.ActiveWindow;
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleDescription!=null && window.AccessibleDescription.Equals(GetCtlActiveWindow().AccessibleName))
                {
                    return window;
                }
            }


            return GetCtlActiveWindow();
        }

        public string GetCtlActiveWindowName()
        {
            return radDock2.DocumentManager.ActiveDocument.AccessibleName;
        }

        public string GetCtlActiveWindowFloatName()
        {
            if (!(radDock2.ActiveWindow.DockType == DockType.ToolWindow && radDock2.ActiveWindow.Name == "_selectview")) return radDock2.ActiveWindow.AccessibleName; 
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleDescription != null && window.AccessibleDescription.Equals(GetCtlActiveWindow().AccessibleName))
                {
                    return window.AccessibleName;
                }
            }
            return GetCtlActiveWindowName();
        }
		
        private void LoadWindowsManager()
        {
            try
            {
                //Select View Window
                _select = new SelectView1 { Dock = DockStyle.Fill, MFrmMain2 = this };
                _select.Dock = DockStyle.Fill;
                _select.Name = "_select";
                _selectview.ToolCaptionButtons &= ~ToolStripCaptionButtons.Close;
                _selectview.AllowedDockState = AllowedDockState.All & ~AllowedDockState.TabbedDocument;
                _selectview.Controls.Add(_select);
                DockTabStrip strip = (DockTabStrip)_selectview.TabStrip;
                strip.SizeInfo.MinimumSize = new Size(275, 100);

                //Status Manager Window
                statusManager.Controls.Add(_statusManager);
                statusManager.ToolCaptionButtons &= ~ToolStripCaptionButtons.Close & ~ToolStripCaptionButtons.SystemMenu;
                statusManager.AllowedDockState = AllowedDockState.All & ~AllowedDockState.TabbedDocument /* &
                                                 ~AllowedDockState.Hidden*/;
                statusManager.DefaultFloatingSize = new Size(490, 200);
                statusManager.DockState = DockState.Docked;
                //Console.WriteLine("\n\nLoadWindowsManager();");
                radDock2.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                this.WireRadDockEvents();
                setMnuCheck();
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("\nErro:  LoadWindowsManager(): " + ex.Message, " ");
            }

        }
        private void setMnuCheck()
        {
            try
            {
                //Menus check

                mnuStatusManager.IsChecked = statusManager.DockState != DockState.Hidden;
                mnuSelectTools.IsChecked = _selectview.DockState != DockState.Hidden;
                mnuChartToolsToolbar.IsChecked = radCommandBar2.Visible = ndtChartTools.VisibleInCommandBar;
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("\n\nsetMnuCheck() " + ex.Message, " ");
            }
        }

        private void FormatWindows()
        {
            int error = 0;
            while (_select == null)
            {
                error++;
                if (error > 99999999999999999)
                {
                    RadMessageBox.Show("Erro ao iniciar portfólios!", " ");
                    return;
                }
            }
            _selectview.SizeChanged += _selectview_SizeChanged;
            DockTabStrip strip = (DockTabStrip)_selectview.TabStrip;
            strip.SizeInfo.MinimumSize = new Size(275, 100);
            _selectview.AutoHideSize = new Size(275, 100);
            _select.AutoSize = true;
            _select.Dock = DockStyle.Fill;
            _selectview.AllowedDockState = AllowedDockState.All & ~AllowedDockState.TabbedDocument;

            
            if (radDock2.DockWindows.Where(win => win.Text == Program.LanguageDefault.DictionaryPlena["statusManager"]).Any())
            {
                statusManager =
                    (ToolWindow)radDock2.DockWindows.ToolWindows.Where(
                        win => win.Text == Program.LanguageDefault.DictionaryPlena["statusManager"]).First();
                statusManager.Controls.Add(_statusManager);
                statusManager.ToolCaptionButtons &= ~ToolStripCaptionButtons.Close & ~ToolStripCaptionButtons.SystemMenu;
                statusManager.AllowedDockState = AllowedDockState.All & ~AllowedDockState.TabbedDocument /*&
                                                 ~AllowedDockState.Hidden*/;
                statusManager.DefaultFloatingSize = new Size(490, 200); 

            }
            else
            {
                radDock2.DockWindow(statusManager, DockPosition.Bottom);
            }
            statusManager.DockState = DockState.AutoHide;
            statusManager.AutoHideSize = new Size(400, 200);
            
            strip = (DockTabStrip)statusManager.TabStrip;
            strip.SizeInfo.MinimumSize = new Size(400, 100);

        }

        #endregion

        #region Helper Functions

        //Enables/disables Nevron menus and command buttons
        private void EnableControls(bool enable)
        {
            bool @checked = mnuChart.IsChecked;
            mnuChart.Enabled = enable;
            mnuChart.IsChecked = @checked;
            @checked = mnuFileSaveImage.IsChecked;
            mnuFileSaveImage.Enabled = enable;
            mnuFileSaveImage.IsChecked = @checked;
            @checked = mnuFilePrint.IsChecked;
            mnuFilePrint.Enabled = enable;
            mnuFilePrint.IsChecked = @checked;
            @checked = mnuView3D.IsChecked;
            mnuView3D.Enabled = enable;
            mnuView3D.IsChecked = @checked;
            @checked = mnuChartColors.IsChecked;
            mnuChartColors.Enabled = enable;
            mnuChartColors.IsChecked = @checked;
            @checked = mnuViewScaleType.IsChecked;
            mnuViewScaleType.Enabled = enable;
            mnuViewScaleType.IsChecked = @checked;
            @checked = mnuViewSeparators.IsChecked;
            mnuViewSeparators.Enabled = enable;
            mnuViewSeparators.IsChecked = @checked;
            @checked = mnuViewShowXGrid.IsChecked;
            mnuViewShowXGrid.Enabled = enable;
            mnuViewShowXGrid.IsChecked = @checked;
            @checked = mnuViewYGrid.IsChecked;
            mnuViewYGrid.Enabled = enable;
            mnuViewYGrid.IsChecked = @checked;
            @checked = mnuViewCrossHair.IsChecked;
            mnuViewCrossHair.Enabled = enable;
            mnuViewCrossHair.IsChecked = @checked;
            @checked = mnuDarvasBoxes.IsChecked;
            mnuDarvasBoxes.Enabled = enable;
            mnuDarvasBoxes.IsChecked = @checked;
            mnuColors.Enabled = enable;
            //Application.DoEvents();
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

        #region Expert Advisors

        /// <summary>
        /// Loads the expert advisors.
        /// This method is called at start up and each time
        /// the apply button is clicked on frmExpertAdvisors
        /// </summary>
        //public void LoadExpertAdvisors()
        //{
        //    return;
        //    ExpertAdvisorIO io = new ExpertAdvisorIO();
        //    try
        //    {
        //        MExpertAdvisors = io.LoadExpertAdvisors();
        //    }
        //    catch (ExpertAdvisorIOException e)
        //    {
        //        Telerik.WinControls.RadMessageBox.Show(e.Message, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //        return;
        //    }
        //}
        #endregion

        #region Timers

        private void tmrMessageService_Tick(object sender, EventArgs e)
        {
            tmrMessageService.Enabled = false;
            CheckAlerts();
            tmrMessageService.Enabled = true;
        }

        //Re-validate session conection:
        private void tmrClearAlerts_Tick(object sender, EventArgs e)
        {
            if (lastDate != DateTime.Now)
            {
                lastDate = DateTime.Now;
                if ((!String.IsNullOrEmpty(Program.LoginAuthentication.CodeSession)) && (!Program.LoginAuthentication.Login.ToUpper().Equals("GUEST")) && !Program.LoginAuthentication.Offline)
                {
                    try
                    {
                        Server.Instance(Program.LanguageDefault).ReloadLogin(Program.LoginAuthentication.Login, Program.LoginAuthentication.Password,  Program.LoginAuthentication.CodeSession);
                    }
                    catch(Exception ex ){
                        Console.Write("\n"+ex.Message);
                    }
                }
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

        #region Action button and menu

        public void LoadViewManagerWorkspace()
        {
            FrmWorkspace frmWorkspace = new FrmWorkspace(this);

            foreach (RadMenuItem Command in mnuWorkspace.Items.Where(item => item.GetType().ToString().Contains("RadMenuItem")))
            {
                if (Command.IsChecked)
                    frmWorkspace.WorkspaceLoaded = Command.Text;
            }

            if (String.IsNullOrEmpty(frmWorkspace.WorkspaceLoaded))
                frmWorkspace.WorkspaceLoaded = "Plena";

            frmWorkspace.ShowDialog();

            if (MActiveChart == null)
                return;
            MActiveChart.LoadWorkspaceRight();
        }

        public void LoadPopoutChart()
        {
            if (radDock2.DocumentManager.ActiveDocument.AccessibleName == "CtlPainelChart")
            {
                CreateGhostWindow(radDock2.DocumentManager.ActiveDocument);
            }
        }

        public void CreateGhostWindow(DockWindow CtlWindow)
        {
            try
            {
                if (CtlWindow.Parent.GetType().ToString().Contains("ToolTabStrip")) return;
                DockWindow GhostWindow = new ToolWindow(CtlWindow.Text)
                {
                    AccessibleDescription = "GhostWindow",
                    AccessibleName = CtlWindow.AccessibleDescription,
                    DockState = DockState.TabbedDocument,
                    CloseAction = DockWindowCloseAction.Close,
                    DefaultFloatingSize = new Size(190, 160),
                    AllowedDockState = AllowedDockState.All & ~AllowedDockState.Docked,
                    Text = CtlWindow.Text,
                };
                var strip = CtlWindow.DockTabStrip;
                var index = strip.Controls.GetChildIndex(CtlWindow);
                GhostWindow.DocumentButtons = DocumentStripButtons.ActiveWindowList;
                GhostWindow.Move -= CtlWindow_DockChanged;
                GhostWindow.Move += CtlWindow_DockChanged;
                GhostWindow.DockTo(CtlWindow, DockPosition.Fill);
                GhostWindow.DockState = DockState.TabbedDocument;
                if (GhostWindow.DockTabStrip == strip)
                    strip.Controls.SetChildIndex(GhostWindow, index);
                CtlWindow.DefaultFloatingSize = new Size(0,0);
                //CtlWindow.FloatingParent.Location = documentContainer2.PointToScreen(Point.Empty);
                //CtlWindow.FloatingParent.Size = new Size(documentContainer2.Width + chartTools.Width, documentContainer2.Height);
                radDock2.FloatWindow(CtlWindow);
                EnableControls(false);
                MActiveChart = null;
                //Console.WriteLine("\n\nCreateGhostWindow(): ChartTools adicionado em " + CtlWindow.Text + "\n\n");
                Window_Initialize(CtlWindow);
                CtlWindow.Focus();
                radDock2.ActivateWindow(CtlWindow);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message + " at CreateGhostWindow()", " ");
            }
        }

        public void ReconfigureTabsChart()
        {
            foreach (DockWindow t in documentManager)
            {
                switch (t.AccessibleName)
                {
                    case "CtlPainelChart":
                        {
                            if (((CtlPainelChart)t.Controls[0]) != null)
                                ((CtlPainelChart)t.Controls[0]).ReconfigureTabs();
                        }
                        break;
                }
            }
        }

        public void LoadFrmSettings()
        {
            new FrmSettings(this).ShowDialog();
        }

        public void LoadSchemeClick(string value)
        {
            foreach (KeyValuePair<string, string> scheme in
                Scheme.Instance().Schemes.Where(scheme => scheme.Value.Equals(value)))
            {
                MActiveChart.m_SchemeColor = scheme.Key;
            }

            Scheme.Instance().UpdateChartColors(MActiveChart.StockChartX1, MActiveChart.m_SchemeColor);

            foreach (RadMenuItem nCommand in mnuChartColors.Items)
                nCommand.IsChecked = nCommand.Text.ToUpper().Equals(value.ToUpper());

            //MActiveChart.LoadCheckMenuRight();
        }

        public void LoadColorScheme(CtlPainelChart chart)
        {
            if (chart == null) chart = MActiveChart;
            if (chart != null)
            {
                string scheme = UseLastChartVisual ? SchemeLastChart : Properties.Settings.Default.SchemeColor;

                chart.m_SchemeColor = scheme;
                Scheme.Instance().UpdateChartColors(chart.StockChartX1, scheme);
            }
            mnuChartColors.Items.Clear();

            foreach (KeyValuePair<string, string> scheme in Scheme.Instance().Schemes)
            {
                RadMenuItem menuItem = new RadMenuItem
                {
                    Text = scheme.Value
                };

                if (chart != null && chart.m_SchemeColor!=null) menuItem.IsChecked =
                    chart.m_SchemeColor.Equals(scheme.Key);

                menuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                menuItem.Click += menuItemColorClick;
                mnuChartColors.Items.Add(menuItem);
            }
        }

        public void LoadViewCrosshair()
        {
            bool @checked = cmdCrosshair.ToggleState == ToggleState.On ? false : true;
            mnuViewCrossHair.IsChecked = @checked;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.CrossHairs = @checked;
                }
                if (window.DockState == DockState.Floating)
                {
                    if (window.Controls.Count == 2)
                    {
                        if (window.Controls[1] is ndtChartTools)
                        {
                            ((ndtChartTools)window.Controls[1])._mCrossHairToggle = true;
                            ((ndtChartTools)window.Controls[1])._mSelectToggle = true;
                            ((ndtChartTools)window.Controls[1]).cmdCrosshair.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
                            ((ndtChartTools)window.Controls[1]).cmdSelect.ToggleState = @checked ? ToggleState.Off : ToggleState.On;
                        }
                    }
                }
            }

            _mCrossHairToggle = true;
            _mSelectToggle = true;
            cmdCrosshair.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
            cmdSelect.ToggleState = @checked ? ToggleState.Off : ToggleState.On;
        }

        public void LoadDeltaCursor()
        {

            bool @checked = cmdDeltaCursor.ToggleState == ToggleState.On ? false : true;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DeltaCursor = @checked;
                }
                if (window.DockState == DockState.Floating)
                {
                    if (window.Controls.Count == 2)
                    {
                        if (window.Controls[1] is ndtChartTools)
                        {
                            ((ndtChartTools)window.Controls[1])._mDeltaToggle = true;
                            ((ndtChartTools)window.Controls[1]).cmdDeltaCursor.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
                        }
                    }
                }
            }
            _mDeltaToggle = true;

            cmdDeltaCursor.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
        }

        public void LoadViewScaleType()
        {
            bool @checked = mnuViewScaleType.IsChecked;

            MActiveChart.StockChartX1.ScaleType = @checked ? ScaleType.stLinearScale : ScaleType.stSemiLogScale;
            MActiveChart.StockChartX1.ResetYScale(0);
            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowXGrid()
        {
            bool @checked = !mnuViewShowXGrid.IsChecked;
            mnuViewShowXGrid.IsChecked = @checked;
            MActiveChart.StockChartX1.XGrid = @checked;
            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowYGrid()
        {
            bool @checked = !mnuViewYGrid.IsChecked;
            mnuViewYGrid.IsChecked = @checked;
            MActiveChart.StockChartX1.YGrid = @checked;
            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewSeparators()
        {
            bool @checked = !MActiveChart.StockChartX1.HorizontalSeparators;

            mnuViewSeparators.IsChecked = @checked;
            MActiveChart.StockChartX1.HorizontalSeparators = @checked;
            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadDarvasBoxes()
        {
            bool _checked = MActiveChart.StockChartX1.DarvasBoxes;
            mnuDarvasBoxes.IsChecked = !_checked;
            MActiveChart.StockChartX1.DarvasBoxes = mnuDarvasBoxes.IsChecked;
            MActiveChart.LoadCheckMenuRight();
        }

        public void LoadAllChartSettings()
        {
            //Update all controls with new color theme
            foreach (DockWindow doc in documentManager.Where(doc =>
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
            {
                if (doc.Controls[0] == null)
                    continue;

                if (doc.AccessibleName == "CtlPainelChart")
                {
                    LoadChartSettings((CtlPainelChart)doc.Controls[0]);
                    break;
                }

            }
        }

        /*public void UpdateChartColors()
        {
            if (MActiveChart == null) return;
            try
            {
                //Update all controls with new color theme
                foreach (DockWindow doc in documentManager.Where(doc =>
                    (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                    (!doc.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                    (!doc.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))))
                {
                    if (doc.Controls.Count == 0)
                        continue;

                    switch (doc.AccessibleName)
                    {
                        case "CtlPainelChart":
                            Scheme.Instance().UpdateChartColors(((CtlPainelChart)doc.Controls[0]).StockChartX1, MActiveChart.m_SchemeColor);
                            break;
                        case "ctlPortfolio":
                            ((ctlPortfolio)doc.ActiveControl).UpdateStyle(m_Style);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }

        }*/

        #endregion

        #region Buttons Events

        private void ndtChartTools_VisibleInCommandBarChanged(object sender, EventArgs e)
        {
            if (ndtChartTools.VisibleInCommandBar == false)
            {
                radCommandBar2.Visible = false;
            }
            else
            {
                radCommandBar2.Visible = true;
            }
        }

        private void cmdCrosshair_ToggleStateChanging(object sender, StateChangingEventArgs args)
        {
            if (!_mCrossHairToggle)
            {
                args.Cancel = true;
                return;
            }
            _mCrossHairToggle = false;
        }

        private void cmdSelect_ToggleStateChanging(object sender, StateChangingEventArgs args)
        {
            if (!_mSelectToggle)
            {
                args.Cancel = true;
                return;
            }
            _mSelectToggle = false;
        }

        private void cmdDeltaCursor_ToggleStateChanging(object sender, StateChangingEventArgs args)
        {
            if (!_mDeltaToggle)
            {
                args.Cancel = true;
                return;
            }
            _mDeltaToggle = false;
        }

        private void cmdMagnetic_ToggleStateChanging(object sender, StateChangingEventArgs args)
        {
            if (!_mMagneticToggle)
            {
                args.Cancel = true;
                return;
            }
            _mMagneticToggle = false;
        }

        public void cmdTextObject_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.TEXTOBJECT;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.TEXTOBJECT;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserDefinedText("My Text " + DateTime.Now);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.Focus();
                }
            }
        }

        public void cmdCrosshair_Click(object sender, EventArgs args)
        {
            if (cmdCrosshair.ToggleState == ToggleState.On) return;
            mnuViewCrossHair_Click(sender, args);
        }

        public void cmdSelect_Click(object sender, EventArgs e)
        {
            if (cmdSelect.ToggleState == ToggleState.On) return;
            mnuViewCrossHair_Click(sender, e);
        }

        public void cmdDeltaCursor_Click(object sender, EventArgs args)
        {
            LoadDeltaCursor();
        }

        public void cmdMagnetic_Click(object sender, EventArgs args)
        {
            mnuViewMagnetic_Click(sender, args);
        }

        public void cmdChart_Click(object sender, EventArgs e)
        {
            ChartSelection selection = (new FrmSelectChart()).GetChartSelection();
            
            SqlConnection _con = DBlocalSQL.Connect();
            if (DBlocalSQL.GetLastBarDataDisk(selection.Symbol, BaseType.Days, _con).TradeDate == DateTime.MinValue)
            {
                RadMessageBox.Show("O ativo escolhido ainda não possui dados sincronizados.", " ");
                DBlocalSQL.Disconnect(_con);
                return;
            }

            DBlocalSQL.Disconnect(_con);
            if (selection == null)
                return;
            measureTime.Start();
            /*MessageService.SubmitRequest(new MSRequest("m" + _messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { selection.Symbol, Periodicity.Daily.GetHashCode() }));
            AddRequestedOperation(new Operations("m" + _messageRequestID, TypeOperations.CreateNewCtlPainelChart,
                                                       new object[] { selection }));*/
            CreateNewCtlPainel(selection, chart =>
            {
                Scheme.Instance().UpdateChartColors(chart.StockChartX1, Properties.Settings.Default.SchemeColor);
                chart.m_SchemeColor = Properties.Settings.Default.SchemeColor;
                chart.StockChartX1.Visible = true;
            });
        }

        public void cmdBuySymbol_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.BUYSYMBOL;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.BUYSYMBOL;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserSymbolObject(SymbolType.soBuySymbolObject,
                                                                   "Buy " +
                                                                   Convert.ToString(((CtlPainelChart)window.Controls[0]).StockChartX1.GetObjectCount(
                                                                                      ObjectType.otBuySymbolObject)), "");
                }
            }
        }

        public void cmdSellSymbol_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.SELLSYMBOL;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.SELLSYMBOL;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserSymbolObject(SymbolType.soSellSymbolObject,
                                                                   "Sell " +
                                                                   Convert.ToString(((CtlPainelChart)window.Controls[0]).StockChartX1.GetObjectCount(
                                                                                      ObjectType.otSellSymbolObject)), "");
                }
            }
        }

        public void cmdExitSymbol_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.EXITSYMBOL;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.EXITSYMBOL;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserSymbolObject(SymbolType.soExitSymbolObject,
                                                                   "Exit " +
                                                                   Convert.ToString(((CtlPainelChart)window.Controls[0]).StockChartX1.GetObjectCount(
                                                                                      ObjectType.otExitSymbolObject)), "");
                }
            }
        }


        public void cmdTrendLine_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.TRENDLINE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.TRENDLINE;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserTrendLine("TL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdRay_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.RAY;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.RAY;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsRay, "RL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdChannel_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.CHANNEL;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    string dateTimeNow = Convert.ToString(DateTime.Now);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH1 " + dateTimeNow);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH2 " + dateTimeNow);
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.CHANNEL;
                }
            }
        }

        public void cmdVerticalLine_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.YLINE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserYLine("VL " + DateTime.Now.Ticks);
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.YLINE;
                }
            }
        }

        public void cmdRectangle_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.RECTANGLE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.RECTANGLE;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsRectangle, "RE " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdEllipse_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.ELIPSE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.ELIPSE;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsEllipse, "EL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdArrow_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.ARROW;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.ARROW;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsTriangle, "AR " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdPolyline_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.POLYLINE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.POLYLINE;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsPolyline, "PL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciRetracements_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.FIBONACCIRETRACEMENTS;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.FIBONACCIRETRACEMENTS;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciRetracements, "FR " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciProjections_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.FIBONACCIPROJECTIONS;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.FIBONACCIPROJECTIONS;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciProgression, "FP" + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciArcs_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.FIBONACCIARCS;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.FIBONACCIARCS;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciArcs, "FA " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciFan_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.FIBONACCIFAN;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.FIBONACCIFAN;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciFan, "FF " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdGannFan_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.GANNFAN;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.GANNFAN;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsGannFan, "GF " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdSpeedLine_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.SPEEDLINES;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.SPEEDLINES;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsSpeedLines, "SL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdHorizontalLine_Click(object sender, EventArgs e)
        {
            _actionChart = ActionChart.XLINE;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0])._actionChart = ActionChart.XLINE;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserXLine("HL " + DateTime.Now.Ticks);
                }
            }
        }

        #endregion

        #region Menus Events

        private void mnuManagerWorkspace_Click(object sender, EventArgs e)
        {
            LoadViewManagerWorkspace();
        }

        private void MenuItemClick(object sender, EventArgs e)
        {
            RadMenuItem item = (RadMenuItem)sender;

            RestoreWorkspace(item.Text.Trim());
            WorkspaceLoaded(item.Text.Trim());
            UpdateStyle();
            AtivateDoubleClickTabs();
            InitializeWindows();
        }

        private void mnuFileSaveImage_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.SaveChartImage();
        }

        private void mnuViewMagnetic_Click(object sender, EventArgs e)
        {


            bool @checked = cmdMagnetic.ToggleState == ToggleState.On ? false : true;
            //Change all charts status:
            foreach (DockWindow window in radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.Magnetic = @checked;
                }
                if (window.DockState == DockState.Floating)
                {
                    if (window.Controls.Count == 2)
                    {
                        if (window.Controls[1] is ndtChartTools)
                        {
                            ((ndtChartTools)window.Controls[1])._mMagneticToggle = true;
                            ((ndtChartTools)window.Controls[1]).cmdMagnetic.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
                        }
                    }
                }
            }
            _mMagneticToggle = true;
            mnuViewMagnetic.IsChecked = @checked;

            cmdMagnetic.ToggleState = @checked ? ToggleState.On : ToggleState.Off;


        }

        private void mnuFilePrint_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null) return;
            MActiveChart.StockChartX1.PrintChart();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void mnuOptions_Click(object sender, EventArgs e)
        {
            LoadFrmSettings();
        }

        private void mnuViewCrossHair_Click(object sender, EventArgs e)
        {
            LoadViewCrosshair();
        }

        public void menuItemColorClick(object sender, EventArgs e)
        {
            RadMenuItem item = (RadMenuItem)sender;
            foreach (KeyValuePair<string, string> scheme in
               Scheme.Instance().Schemes.Where(scheme => scheme.Value.Equals(item.Text)))
            {
                MActiveChart.m_SchemeColor = scheme.Key;
            }
            if (item.IsChecked)
                return;
            Scheme.Instance().UpdateChartColors(MActiveChart.StockChartX1, MActiveChart.m_SchemeColor);
            foreach (RadMenuItem radMenuItem in mnuChartColors.Items)
                radMenuItem.IsChecked = radMenuItem.Text.ToUpper().Equals(item.Text.ToUpper());

            MActiveChart.LoadCheckMenuRight();
        }

        public void UpdateMnuColor()
        {
            if (MActiveChart == null) return;
            string color = MActiveChart.m_SchemeColor;
            switch (color)
            {
                case "SchemeGreen":
                    color = Program.LanguageDefault.DictionarySettings["SchemeGreen"];
                    break;
                case "SchemeBlue":
                    color = Program.LanguageDefault.DictionarySettings["SchemeBlue"];
                    break;
                case "SchemeWhite":
                    color = Program.LanguageDefault.DictionarySettings["SchemeWhite"];
                    break;
                case "SchemeDark":
                    color = Program.LanguageDefault.DictionarySettings["SchemeDark"];
                    break;
                case "SchemeBeige":
                    color = Program.LanguageDefault.DictionarySettings["SchemeBeige"];
                    break;
                case "SchemeMono":
                    color = Program.LanguageDefault.DictionarySettings["SchemeMono"];
                    break;
                default:
                    color = Program.LanguageDefault.DictionarySettings["SchemeSky"];
                    break;
            }

                
            foreach (RadMenuItem radMenuItem in mnuChartColors.Items)
                radMenuItem.IsChecked = radMenuItem.Text.ToUpper().Equals(color.ToUpper());
        }

        private void mnuViewScaleType_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewScaleType();
        }
        private void mnuViewScaleType_StateChanging(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            //LoadViewScaleType();
        }
        private void mnuViewShowXGrid_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewShowXGrid();
        }

        private void mnuViewYGrid_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewShowYGrid();
        }

        private void mnuViewSeparators_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadViewSeparators();
        }

        private void mnuDarvasBoxes_Click(object sender, EventArgs e)
        {
            if (MActiveChart == null)
                return;

            LoadDarvasBoxes();
        }

        private void mnuStartPage_Click(object sender, EventArgs e)
        {
            //OpenURL(Properties.Settings.Default.StartPage, Program.LanguageDefault.DictionaryPlena["webBrowser"]);
            ctlWeb web = (ctlWeb)_toolWeb.Controls[0];
            web.Navigate(Properties.Settings.Default.StartPage);
            DocumentTabStrip tabStrip = (DocumentTabStrip)_toolWeb.Parent;
            tabStrip.ActiveWindow = _toolWeb;
        }

        private void mnuChartToolsToolbar_Click(object sender, EventArgs e)
        {
            ndtChartTools.VisibleInCommandBar = !ndtChartTools.VisibleInCommandBar;
            mnuChartToolsToolbar.IsChecked = radCommandBar2.Visible = ndtChartTools.VisibleInCommandBar;
        }

        private void mnuStatusManager_Click(object sender, EventArgs e)
        {
            statusManager.DockState = statusManager.DockState == DockState.Hidden
                                        ? statusManager.PreviousDockState
                                        : DockState.Hidden;
            mnuStatusManager.IsChecked = statusManager.DockState != DockState.Hidden;
        }

        private void mnuSelectTools_Click(object sender, EventArgs e)
        {
            _selectview.DockState = _selectview.DockState == DockState.Hidden
                                        ? _selectview.PreviousDockState
                                        : DockState.Hidden;
            mnuSelectTools.IsChecked = _selectview.DockState != DockState.Hidden;

        }

        private void mnuPopoutChart_Click(object sender, EventArgs e)
        {
            LoadPopoutChart();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            (new frmHelp2() { button = "Fechar"}).ShowDialog();
        }

        private void CheckMnuStyle()
        {
            foreach (RadMenuItem menuItem in mnuAppStyle.Items)
            {
                menuItem.IsChecked = menuItem.AccessibleName == m_Style;
            }
        }

        private void mnuAppOfficeSilver_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = "Office2007SilverTheme";
            CheckMnuStyle();
        }

        private void mnuAppStandard_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = "Office2007BlackTheme";
            CheckMnuStyle();
        }

        private void mnuAppAqua_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = AquaTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppBreeze_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = BreezeTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppDesert_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = DesertTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppHighContrastBlack_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = HighContrastBlackTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppOffice2010Black_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = Office2010BlackTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppOffice2010Blue_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = "Office2010BlueTheme";
            CheckMnuStyle();
        }

        private void mnuAppOffice2010Silver_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = "Office2010SilverTheme";
            CheckMnuStyle();
        }

        private void mnuAppTelerikMetro_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = TelerikMetroTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppTelerikMetroBlue_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = TelerikMetroBlueTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppWindows7_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = Windows7Theme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuWindows8Theme_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = Windows8Theme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppVisualStudio2012Dark_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = VisualStudio2012DarkTheme.ThemeName;
            CheckMnuStyle();
        }

        private void mnuAppVisualStudio2012Light_Click(object sender, EventArgs e)
        {
            m_Style = ThemeResolutionService.ApplicationThemeName = VisualStudio2012LightTheme.ThemeName;
            CheckMnuStyle();
        }

        #endregion

        #region Windows Events

        /*//public class CustomFloatingWindow : FloatingWindow
        //{

        //    //private Form LastDockForm;
        //    private frmMain2 frmMain;
        //    private ndtChartTools chartTools;
        //    private DockWindow FloatWindow;
        //    public CustomFloatingWindow(RadDock dockManager, frmMain2 frmMain2)
        //        : base(dockManager)
        //    {

        //        frmMain = frmMain2;
        //        Size = new Size(800,600);
        //    }


        //    protected override void OnActivated(EventArgs e)
        //    {

        //        base.OnActivated(e);

        //        //Form dockForm = this.DockManager.FindForm();
        //        //LastDockForm = dockForm;
        //        //if (dockForm != null)
        //        //{

        //        //    dockForm.RemoveOwnedForm(this);
        //        //    Console.WriteLine("\n\nOnActivated() " + LastDockForm.Text);
        //        //}

        //        this.ShowInTaskbar = true;
        //        FloatWindow = (DockWindow)GetContainerControl().ActiveControl;
        //        if (FloatWindow.AccessibleName == "CtlPainelChart")
        //        {

        //            if (FloatWindow.Controls.Count != 0)
        //            {

        //                Console.WriteLine("MActiveChart " + Text + " selected.");
        //                if(FloatWindow.Controls.Count == 1)
        //                {

        //                    chartTools = new ndtChartTools((CtlPainelChart)FloatWindow.Controls[0], frmMain)
        //                    {

        //                        Dock = DockStyle.Right,
        //                        AccessibleName = "CtlPainelChart"
        //                    };

        //                    FloatWindow.Controls.Add(chartTools);
        //                }

        //                frmMain.MActiveChart = (CtlPainelChart)FloatWindow.Controls[0];
        //                frmMain.MActiveChart.DrawSelection();
        //                frmMain.MActiveChart.UpdateMenus();
        //                frmMain.MActiveChart.EnableControls(true);

        //                if (frmMain._actionChart != ActionChart.NONE)
        //                    frmMain.MActiveChart.ActiveChartChanged = true;

        //                frmMain.MActiveChart.VerifyAction();
        //                frmMain.MActiveChart.LoadStockPortfolioActive();
        //            }
        //            else
        //            {



        //                if (frmMain.MActiveChart != null)
        //                {

        //                    frmMain.MActiveChart = null;
        //                    frmMain._actionChart = ActionChart.NONE;
        //                }
        //            }
        //        }





        //        frmMain._mActiveDocumentName = AccessibleName;
        //        //Focus();
        //    }


        //    protected override void OnClosing(CancelEventArgs e)
        //    {

        //        base.OnClosing(e);
        //        //if(LastDockForm != null)
        //        //{

        //        //    LastDockForm.AddOwnedForm(this);
        //        //    Console.WriteLine("\n\nOnClosing() " + LastDockForm.Text);
        //        //} 

        //        DockWindow dockWindow = FloatWindow;
        //        dockWindow.Controls.Remove(chartTools);
        //        foreach (DockWindow window in frmMain.radDock2.DocumentManager.DocumentArray)
        //        {

        //            if (window.AccessibleName != dockWindow.AccessibleDescription)
        //                continue;

        //            dockWindow.DockChanged -= frmMain.CtlWindow_DockChanged;
        //            dockWindow.DockTo(window, DockPosition.Fill);
        //            dockWindow.DockChanged += frmMain.CtlWindow_DockChanged;
        //            frmMain.radDock2.CloseWindow(window);
        //            e.Cancel = true;
        //            return;
        //        }
        //    }




        //}*/

        void radDock2_FloatingWindowCreated(object sender, FloatingWindowEventArgs e)
        {
            //CustomFloatingWindow customWindow = new CustomFloatingWindow(this.radDock2, this);
            if (!radDock2.ActiveWindow.Equals(_selectview))
            {
                //e.Window = customWindow;
                e.Window.ShowInTaskbar = true;
                e.Window.MinimizeBox = true;
                e.Window.MaximizeBox = true;
                e.Window.FormBorderStyle = FormBorderStyle.Sizable;
                e.Window.Activated += new EventHandler(Window_Activated);
                e.Window.Closing += new CancelEventHandler(Window_Closing);
            }
        }


        void Window_Initialize(DockWindow floatingWindow)
        {
            ndtChartTools chartTools;
            chartTools = new ndtChartTools((CtlPainelChart)floatingWindow.Controls[0], this)
            {
                Dock = DockStyle.Right,
                AccessibleName = "CtlPainelChart",
            };
            floatingWindow.Controls.Add(chartTools);
            //floatingWindow.Location = floatingWindow.DockManager.Location;
            //Point p = new Point(documentContainer1.Location.X, documentContainer1.Location.Y);
            floatingWindow.FloatingParent.Location = documentContainer2.PointToScreen(Point.Empty);
            floatingWindow.FloatingParent.Size = new Size(documentContainer2.Width+chartTools.Width, documentContainer2.Height);
            //floatingWindow.;
            //floatingWindow.Height = documentContainer1.Height;
           // floatingWindow.Width = documentContainer1.Width;
        }

        public static void Window_Initialize(DockWindow floatingWindow, CtlPainelChart chart)
        {
            ndtChartTools chartTools;
            chartTools = new ndtChartTools(chart, frmMain2.GInstance)
            {
                Dock = DockStyle.Right,
                AccessibleName = "CtlPainelChart"
            };
            floatingWindow.FloatingParent.Location = new Point(202, 70);
            floatingWindow.FloatingParent.ClientSize = new Size(900, 600);
            floatingWindow.FloatingParent.ShowInTaskbar = true;
            if (!floatingWindow.Controls.ContainsKey("ndtChartTools"))
                floatingWindow.Controls.Add(chartTools);
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            FloatingWindow floatingParent = (FloatingWindow)sender;

            //DockWindow dockWindow = (DockWindow)floatingWindow.GetContainerControl().ActiveControl;
            DockWindow dockWindow = null;
            foreach (DockWindow floatingWindow in radDock2.DockWindows)
            {
                if (floatingWindow.FloatingParent == null) continue;
                if (floatingWindow.FloatingParent.Equals(floatingParent))
                    dockWindow = floatingWindow;
            }
            if (dockWindow == null) return;
            if (dockWindow.Controls.Count > 1)
            {

                ndtChartTools chartTools = (ndtChartTools)dockWindow.Controls[1];
                cmdDeltaCursor.ToggleState = chartTools.cmdDeltaCursor.ToggleState;
                cmdSelect.ToggleState = chartTools.cmdSelect.ToggleState;
                cmdCrosshair.ToggleState = chartTools.cmdCrosshair.ToggleState;
                cmdMagnetic.ToggleState = chartTools.cmdMagnetic.ToggleState;
                chartTools.MActiveChart.popout = false;
                chartTools.MActiveChart.ChartToolsPopOut = null;
                MActiveChart = chartTools.MActiveChart;
                MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);
                if (_actionChart != ActionChart.NONE)
                    MActiveChart.ActiveChartChanged = true;
                dockWindow.Controls.RemoveAt(1);
            }
            else
            {
                if (MActiveChart != null)
                {
                    MActiveChart = null;
                    _actionChart = ActionChart.NONE;
                }

            }
            foreach (DockWindow window in radDock2.DocumentManager.DocumentArray)
            {
                if (window.AccessibleName != dockWindow.AccessibleDescription)
                    continue;
                dockWindow.DockChanged -= CtlWindow_DockChanged;
                dockWindow.DockTo(window, DockPosition.Fill);
                dockWindow.DockChanged += CtlWindow_DockChanged;
                _mActiveDocumentName = dockWindow.AccessibleName;
                radDock2.CloseWindow(window);
                radDock2.ActivateWindow(dockWindow);
                e.Cancel = true;
                return;
            }
        }

        public void Window_Activated(object sender, EventArgs e)
        {
            EnableControls(false);
            if (MActiveChart != null)
            {
                MActiveChart = null;
                _actionChart = ActionChart.NONE;
            }
            //FloatingWindow floatingWindow = (FloatingWindow)sender;
            //DockWindow floatWindow = (DockWindow)floatingWindow.GetContainerControl().ActiveControl;
            //if (floatWindow == null)
            //{
            //    MActiveChart = null;
            //    return;
            //}
            //if (floatWindow.AccessibleName == "CtlPainelChart")
            //{
            //    if (floatWindow.Controls.Count != 0)
            //    {
            //        Console.WriteLine("MActiveChart " + floatWindow.Text + " selected.");
            //        MActiveChart = (CtlPainelChart)floatWindow.Controls[0];
            //MActiveChart.DrawSelection();
            //MActiveChart.UpdateMenus();
            //MActiveChart.EnableControls(true);
            //if (_actionChart != ActionChart.NONE)
            //    MActiveChart.ActiveChartChanged = true;
            //MActiveChart.LoadStockPortfolioActive();
            //    }
            //    else
            //    {
            //        if (MActiveChart != null)
            //        {
            //            MActiveChart = null;
            //            _actionChart = ActionChart.NONE;
            //        }
            //    }
            //}
            //_mActiveDocumentName = floatWindow.AccessibleName;
        }

        void CtlWindow_DockChanged(object sender, EventArgs e)
        {


            DockWindow window = (DockWindow)sender;
            if (window.Parent == null) return;





            if (window.AccessibleName == "CtlPainelChart" && window.DockState == DockState.Floating && window.DesiredDockState == DockState.Docked && window.Parent.GetType().ToString().Contains("ToolTabStrip"))
            {







                //if (window.Parent.GetType().ToString().Contains("ToolTabStrip"))
                //    return;
                //DocumentTabStrip tabStrip = (DocumentTabStrip)window.Parent; 
                if (window.DesiredDockState == DockState.Docked && window.DockState == DockState.Floating)
                {



                    foreach (DockWindow Ctlwindow in radDock2.DocumentManager.DocumentArray)
                    {
                        if (Ctlwindow.AccessibleName != window.AccessibleDescription)
                            continue;
                        if (window.Controls.Count > 1)
                        {
                            window.Controls.RemoveAt(1);
                        }
                        window.DockTo(Ctlwindow, DockPosition.Fill);
                        radDock2.CloseWindow(Ctlwindow);
                        radDock2.ActivateWindow(window);
                        return;
                    }
                }

                window.DockState = DockState.TabbedDocument;
            }
            else if (window.AccessibleName != "CtlPainelChart")
            {
                return;
            }

            else
            {


                if (window.Parent.GetType().ToString().Contains("ToolTabStrip"))
                    return;
                DocumentTabStrip tabStrip = (DocumentTabStrip)window.Parent;
                if (tabStrip.Controls.Count == 1)
                {



                    try
                    {

                        tabStrip.DoubleClick -= TabStrip_DoubleClick;
                        //  window.DockManager.SelectedTabChanged -= radDock2_SelectedTabChanged;
                        //   window.TabStripItem.FontChanged -= TabStripItem_FontChanged;
                    }



                    finally
                    {
                        tabStrip.DoubleClick += TabStrip_DoubleClick;
                        //  window.DockManager.SelectedTabChanged += radDock2_SelectedTabChanged;
                        //  window.TabStripItem.FontChanged += TabStripItem_FontChanged;
                    }

                    return;
                }
                if (window.DesiredDockState == DockState.Docked && window.DockState == DockState.Floating)
                {
                    foreach (DockWindow Ctlwindow in radDock2.DocumentManager.DocumentArray)
                    {
                        if (Ctlwindow.AccessibleName != window.AccessibleDescription)
                            continue;
                        if (window.Controls.Count > 1)
                        {
                            window.Controls.RemoveAt(1);
                        }
                        window.DockTo(Ctlwindow, DockPosition.Fill);
                        radDock2.CloseWindow(Ctlwindow);
                        radDock2.ActivateWindow(window);
                        return;
                    }
                }
                window.DockState = DockState.TabbedDocument;
            }
        }

        private void radDock2_DockWindowClosing(object sender, DockWindowCancelEventArgs e)
        {
            if (e.NewWindow.IsInFloatingMode)
            {
                foreach (DockWindow window in radDock2.DocumentManager.DocumentArray)
                {
                    if (window.AccessibleName != e.NewWindow.AccessibleDescription)
                        continue;
                    e.NewWindow.DockChanged -= CtlWindow_DockChanged;
                    e.NewWindow.DockTo(window, DockPosition.Fill);
                    e.NewWindow.DockChanged += CtlWindow_DockChanged;
                    radDock2.CloseWindow(window);
                    e.Cancel = true;
                    return;
                }
            }
            else if (e.NewWindow.AccessibleDescription == "GhostWindow")
            {
                var strip = e.NewWindow.DockTabStrip;
                if (strip == null) return;
                var index = strip.Controls.GetChildIndex(e.NewWindow);
                foreach (DockWindow window in radDock2.DocumentManager.DocumentArray)
                {
                    if (window.AccessibleDescription != e.NewWindow.AccessibleName)
                        continue;
                    if (strip == window.DockTabStrip)
                        strip.Controls.SetChildIndex(window, index);
                    break;
                }
            }
            else if (e.NewWindow.AccessibleName == "CtlPainelChart")
            {
                ChartsList.InsertCtlFromWindow(e.NewWindow);
            }
            //if(e.NewWindow.Controls.ContainsKey("CtlPainelChart"))
            //{       
            //    if (ChartsList.Count() <= 10)
            //    {
            //        ChartsList.InsertToList((CtlPainelChart)e.DockWindow.Controls[0]);
            //    }
            //}

        }

        private void frmMain2_FormClosing(object sender, FormClosingEventArgs e)
        {

            SqlConnection _con = DBlocalSQL.Connect();
            DBlocalSQL.Shrink(_con);
            DBlocalSQL.Disconnect(_con);
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
                _mCtlData = null;
            }

            Properties.Settings.Default.Save();

            ManagerWorkspace.Instance().SaveTemplate(m_Style, this);

            if ((!String.IsNullOrEmpty(Program.LoginAuthentication.CodeSession)) && (!Program.LoginAuthentication.Login.ToUpper().Equals("GUEST")) && !Program.LoginAuthentication.Offline)
                Server.Instance(Program.LanguageDefault).EndSession(Program.LoginAuthentication.CodeSession);
        }

        private void radDock2_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            bool enable = true;

            if (e.NewWindow.Controls.Count == 0 || e.NewWindow != e.NewWindow.DockManager.DocumentManager.ActiveDocument)
                return;

            //Disable the controls if not the same type of form
            if (_mActiveDocumentName != e.NewWindow.AccessibleName || e.NewWindow.AccessibleName != "CtlPainelChart")
            {
                enable = false;
            }
            if (!enable)
            {
                EnableControls(false);
            }
            if (e.NewWindow.AccessibleName == "ctlChart")
            {
                MActiveChart = (CtlPainelChart)e.NewWindow.Controls[0];
                MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);
                MActiveChart.LoadStockPortfolioActive();
            }
            else if (e.NewWindow.AccessibleName == "CtlPainelChart" && e.NewWindow.DockState != DockState.Floating)
            {
                //Console.WriteLine("MActiveChart " + e.NewWindow.Text + " selected.");
                MActiveChart = (CtlPainelChart)e.NewWindow.Controls[0];
                MActiveChart.DrawSelection();
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

            _mActiveDocumentName = e.DockWindow.AccessibleName;
        }

        private void radDock2_DockWindowClosed(object sender, DockWindowEventArgs e)
        {

            foreach (DockWindow dockWindow in documentManager)
            {
                if (dockWindow.AccessibleName == "CtlPainelChart")
                    return;
            }
            EnableControls(false);
        }

        /// <summary>
        /// Função para remoção dos botões close das janelas Select e Status Manager. Disparada quando ocorre uma transação de estado de alguma janela no RadDock2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radDock2_TransactionCommitted(object sender, RadDockTransactionEventArgs e)
        {
            if (e.Transaction.AssociatedWindows.Count == 0) return;
            try
            {
                if (e.Transaction.AssociatedWindows.Contains(_selectview))
                {
                    int index = e.Transaction.AssociatedWindows.FindIndex(
                        delegate(DockWindow dockWindow)
                        {
                            if (dockWindow == _selectview)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        });
                    if (e.Transaction.AssociatedWindows[index].FloatingParent != null)
                    {
                        e.Transaction.AssociatedWindows[index].FloatingParent.ControlBox = false;
                        e.Transaction.AssociatedWindows[index].FloatingParent.FormBorderStyle = FormBorderStyle.Sizable;
                        e.Transaction.AssociatedWindows[index].FloatingParent.MinimumSize = new Size(275, 120);
                    }
                    else
                    {
                        if (_selectview.TabStrip != null)
                        {
                            DockTabStrip strip = (DockTabStrip)_selectview.TabStrip;
                            strip.SizeInfo.MinimumSize = new Size(275, 100);
                        }
                    }
                }
                else if (e.Transaction.AssociatedWindows.Contains(statusManager))
                {
                    int index = e.Transaction.AssociatedWindows.FindIndex(
                        delegate(DockWindow dockWindow)
                        {
                            if (dockWindow == statusManager)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        });
                    if (e.Transaction.AssociatedWindows[index].FloatingParent != null)
                    {
                        e.Transaction.AssociatedWindows[index].FloatingParent.ControlBox = false;
                        e.Transaction.AssociatedWindows[index].FloatingParent.FormBorderStyle = FormBorderStyle.Sizable;
                        e.Transaction.AssociatedWindows[index].FloatingParent.MinimumSize = new Size(405, 130);
                    }
                    else
                    {
                        if (statusManager.TabStrip != null)
                        {
                            DockTabStrip strip = (DockTabStrip)statusManager.TabStrip;
                            strip.SizeInfo.MinimumSize = new Size(400, 100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
            }

        }



        private void radDock2_TransactionCommitting(object sender, RadDockTransactionCancelEventArgs e)
        {
            try
            {
                RedockTransaction transaction = e.Transaction as RedockTransaction;
                if (transaction != null)
                {
                    //if (transaction.AssociatedWindows.Contains(_selectview))
                    //{
                    //    DockTabStrip strip = (DockTabStrip)statusManager.TabStrip;
                    //    strip.SizeInfo.MinimumSize = new Size(400, 100);
                    //}
                    if (transaction.TargetState == DockState.Floating)
                    {
                        if (transaction.AssociatedWindows[0].AccessibleName == "CtlPainelChart")
                        {
                            e.Cancel = true;
                            CreateGhostWindow(transaction.AssociatedWindows[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Save and Load Workspaces

        public void LoadWorkspaces()
        {
            mnuWorkspace.Items.Clear();

            mnuWorkspace.Items.Add(mnuManagerWorkspace);
            mnuWorkspace.Items.Add(radMenuSeparatorItem1);

            XmlDocument xmlDocumentWorkspace = new XmlDocument();
            xmlDocumentWorkspace.Load(ListWorkspace._path + "Workspace.xml");
            XmlNodeList nodeList = xmlDocumentWorkspace.GetElementsByTagName("WORKSPACE");

            foreach (RadMenuItem MenuItem in from XmlNode node in nodeList select new RadMenuItem { Text = node["TEXT"].InnerText })
            {
                MenuItem.Click += MenuItemClick;
                mnuWorkspace.Items.Add(MenuItem);
            }
        }


        public void CheckDirectoryFileWorkspace()
        {
            //Also check for studies:
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\STUDY"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\STUDY");
            }

            string workspace_path = Path.Combine(ListWorkspace._path, @"Workspace.xml");
            if (!Directory.Exists(ListWorkspace._path))
            {
                Directory.CreateDirectory(ListWorkspace._path);
                if (m_Style == null)
                    m_Style = "VisualStudio2012Light";
                ManagerWorkspace.Instance().SaveTemplate(m_Style, this);
                XmlDocument workspace = new XmlDocument();
                XmlNode nodeConjunto = workspace.CreateNode(XmlNodeType.Element, "CONJUNTO", null);
                XmlNode nodeSuporte = workspace.CreateNode(XmlNodeType.Element, "SUPORTE", null);
                XmlNode nodeParent = workspace.CreateNode(XmlNodeType.Element, "PARENT", null);
                XmlNode nodeText = workspace.CreateNode(XmlNodeType.Element, "TEXT", null);
                XmlNode nodeWorkspace = workspace.CreateNode(XmlNodeType.Element, "WORKSPACE", null);
                XmlNode nodeSubParent = workspace.CreateNode(XmlNodeType.Element, "PARENT", null);
                XmlNode nodeSubText = workspace.CreateNode(XmlNodeType.Element, "TEXT", null);
                XmlNode nodeDefault = workspace.CreateNode(XmlNodeType.Element, "DEFAULT", null);


                nodeText.InnerText = "Workspaces";
                nodeSubParent.InnerText = "Workspaces";
                nodeSubText.InnerText = "Plena";
                nodeDefault.InnerText = "1";

                nodeWorkspace.AppendChild(nodeSubParent);
                nodeWorkspace.AppendChild(nodeSubText);
                nodeWorkspace.AppendChild(nodeDefault);

                nodeSuporte.AppendChild(nodeParent);
                nodeSuporte.AppendChild(nodeText);
                nodeSuporte.AppendChild(nodeWorkspace);

                nodeConjunto.AppendChild(nodeSuporte);

                workspace.AppendChild(nodeConjunto);
                workspace.Save(ListWorkspace._path + "\\Workspace.xml");
                VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + "\\Workspace.xml", VersionChecker.VersionChecker.Version);
            }
            else if (!File.Exists(workspace_path))
            {
                if (m_Style == null)
                    m_Style = "VisualStudio2012Light";
                ManagerWorkspace.Instance().SaveTemplate(m_Style, this);
                XmlDocument workspace = new XmlDocument();
                XmlNode nodeConjunto = workspace.CreateNode(XmlNodeType.Element, "CONJUNTO", null);
                XmlNode nodeSuporte = workspace.CreateNode(XmlNodeType.Element, "SUPORTE", null);
                XmlNode nodeParent = workspace.CreateNode(XmlNodeType.Element, "PARENT", null);
                XmlNode nodeText = workspace.CreateNode(XmlNodeType.Element, "TEXT", null);
                XmlNode nodeWorkspace = workspace.CreateNode(XmlNodeType.Element, "WORKSPACE", null);
                XmlNode nodeSubParent = workspace.CreateNode(XmlNodeType.Element, "PARENT", null);
                XmlNode nodeSubText = workspace.CreateNode(XmlNodeType.Element, "TEXT", null);
                XmlNode nodeDefault = workspace.CreateNode(XmlNodeType.Element, "DEFAULT", null);


                nodeText.InnerText = "Workspaces";
                nodeSubParent.InnerText = "Workspaces";
                nodeSubText.InnerText = "Plena";
                nodeDefault.InnerText = "1";

                nodeWorkspace.AppendChild(nodeSubParent);
                nodeWorkspace.AppendChild(nodeSubText);
                nodeWorkspace.AppendChild(nodeDefault);

                nodeSuporte.AppendChild(nodeParent);
                nodeSuporte.AppendChild(nodeText);
                nodeSuporte.AppendChild(nodeWorkspace);

                nodeConjunto.AppendChild(nodeSuporte);

                workspace.AppendChild(nodeConjunto);
                workspace.Save(ListWorkspace._path + "Workspace.xml");
                VersionChecker.VersionChecker.InsertVersion(ListWorkspace._path + "\\Workspace.xml", VersionChecker.VersionChecker.Version);
            }
        }

        public void WorkspaceLoaded(string description)
        {
            foreach (RadMenuItem nCommand in mnuWorkspace.Items.Where(item => item.GetType().ToString().Contains("RadMenuItem")))
                nCommand.IsChecked = nCommand.Text.ToUpper().Equals(description.ToUpper());

        }

        public void RestoreWorkspace(string archiveNameWorkspace)
        {
            try
            {
                _archiveNameWorkspace = archiveNameWorkspace;

                if (!File.Exists(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + _archiveNameWorkspace + ".xml"))
                {
                    Telerik.WinControls.RadMessageBox.Show("Arquivo " + _archiveNameWorkspace + ".xml não foi encontrado no diretório " + ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\", " ");
                    ListWorkspace.Instance().Delete(_archiveNameWorkspace);
                    foreach (var VARIABLE in mnuWorkspace.Items)
                    {
                        if (VARIABLE.Text == _archiveNameWorkspace)
                        {
                            mnuWorkspace.Items.Remove(VARIABLE);
                            break;
                        }

                    }
                    if (File.Exists(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml"))
                        File.Delete(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml");
                    return;
                }

                ChartsList.InsertFromDockControls(radDock2);
                IEnumerable<DockWindow> docManager = radDock2.DockWindows.Where(doc => doc == null || doc.IsDisposed);
                int docCount = docManager.ToList().Count;
                for (int i = 0; i < docCount; i++)
                {
                    docManager.ToList().First().Dispose();
                }
                //radDock2.CloseAllWindows();
                if (ListWorkspace._path != null) radDock2.LoadFromXml(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + _archiveNameWorkspace + ".xml");

                docManager = radDock2.DockWindows.Where(doc => doc == null || doc.IsDisposed);

                docCount = docManager.ToList().Count;
                for (int i = 0; i < docCount; i++)
                {
                    docManager.ToList().First().Dispose();
                }
                setMnuCheck();

            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("\n RestoreWorkspace() " + ex.Message, " ");
            }
        }

        private void StateStateRestored(object sender, EventArgs e)
        {

            RadDock dock = (RadDock)sender;
            DockWindowCollection docManager = dock.DockWindows;
            try
            {
                setMnuCheck();
                //RestoreDocumentsDefault(docManager);
                if (!File.Exists(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml"))
                    return;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml");
                XmlNodeList nodeListAtivos = xmlDocument.GetElementsByTagName("ATIVO");

                EPermission permission = Program.LoginAuthentication.VerifyFeature(EFeatures.HISTORICDATA); //ListFeatures.Instance().PermissionFeature(EFeatures.HISTORICDATA, EPermission.Master, Program.LoginAuthentication.Features);
                var mBars = 0;


                switch (permission)
                {
                    case EPermission.Permitido:
                        mBars = Properties.Settings.Default.History;
                        break;
                    case EPermission.Restringido:
                        mBars = Properties.Settings.Default.MaxHistoryGuest;
                        break;
                    case EPermission.Negado:
                        mBars = 0;
                        break;
                }

                LoadAlerts();
                LoadBackTest();
                LoadScanner();

                foreach (DockWindow document in radDock2.DockWindows.Where(document => document.AccessibleName == "CtlPainelChart"))
                {
                    if (document.AccessibleDescription == null) continue;
                    XmlNode node = nodeListAtivos.Cast<XmlNode>().Where(nodeListAtivo => document.Name.Equals(nodeListAtivo["NOME_TOOLWINDOW"].InnerText)).
                        FirstOrDefault();



                    if (node == null)
                    {
                        continue;
                    }
                    XmlNode propriedades = node["PROPRIEDADES"];
                    string periodicidade = node["PERIODICIDADE"].InnerText;
                    Periodicity periodicity;
                    Periodicity.TryParse(periodicidade, false, out periodicity);
                    int barSize = 1;
                    int.TryParse(node["INTERVALO"].InnerText, out barSize);
                    string simbolo = node["SIMBOLO"].InnerText;

                    if (!ChartsList.IsEmpty())
                    {
                        CtlPainelChart chart;
                        chart = ChartsList.TakeFromList();

                        string windowName = ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] +
                                                                       "\\" + document.Name + ".sct";
                        //Thread changeWindow = new Thread(() => ChartsList.AddChart(document, propriedades, node, simbolo, periodicity, barSize, mBars, windowName));
                        //changeWindow.Start();
                        chart.StateUsed = true;
                        Action action = () =>
                            {
                                ChartsList.AddChart(chart, document, propriedades, node, simbolo, periodicity, barSize, mBars, windowName);
                                chart.LoadDataTemplate("-987654321.123456789");
                            };

                        BeginInvoke(action);
                        try
                        {
                            document.Move -= CtlWindow_DockChanged;
                        }
                        finally
                        {
                            document.Move += CtlWindow_DockChanged;
                        }

                    }
                    else
                    {
                        CtlPainelChart chart;
                        chart = ObjectPool.New<CtlPainelChart>();
                        chart.StockChartX1.Visible = false;
                        chart.LoadCtlPainelChart(this, _mCtlData, simbolo, periodicity, barSize,
                                                 mBars, "Plena", true);
                        chart.StateDummy = true;
                        chart.BlockUpdateStock = true;
                        chart.m_StopLoadScroll = true;
                        document.Controls.Add(chart);
                        chart.BlockUpdateStock = false;
                        XmlChartPropriedades(chart, propriedades);
                        if (document.DockState == DockState.Floating)
                        {

                            document.FloatingParent.ShowInTaskbar = true;
                            document.FloatingParent.MinimizeBox = true;
                            document.FloatingParent.MaximizeBox = true;
                            document.FloatingParent.FormBorderStyle = FormBorderStyle.Sizable;
                            document.FloatingParent.Activated += new EventHandler(Window_Activated);
                            document.FloatingParent.Closing += new CancelEventHandler(Window_Closing);
                            Window_Initialize(document, chart);
                        }
                        try
                        {
                            document.Move -= CtlWindow_DockChanged;
                        }
                        finally
                        {
                            document.Move += CtlWindow_DockChanged;
                        }
                        if ((node["VISIBLE"] != null) && (node["VISIBLE"].InnerText.Equals("0")))
                            continue;
                        chart.StateDummy = false;
                        string windowName = ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] +
                                                                       "\\" + document.Name + ".sct";
                        chart.InitRTChartAsync(b => chart._asyncOp.Post(() =>
                        {
                            if (b)
                            {
                                chart.StockChartX1.FirstVisibleRecord = int.Parse(propriedades["FIRST_VISIBLE_RECORD"].InnerText);
                                chart.StockChartX1.LastVisibleRecord = int.Parse(propriedades["LAST_VISIBLE_RECORD"].InnerText);
                                chart.BindContextMenuEvents();
                                chart.m_SchemeColor = propriedades["COLOR"].InnerText;
                                Scheme.Instance().UpdateChartColors(chart.StockChartX1, chart.m_SchemeColor);
                                MActiveChart = chart;
                                chart.LoadDataTemplate();
                                chart.UpdateMenus();
                                //LoadColorScheme(chart);
                                chart.StockChartX1.Visible = true;
                                chart.StockChartX1.Width = chart.Width - 4;
                                if (File.Exists(windowName))
                                    chart.StockChartX1.LoadGeneralTemplate(windowName);
                                chart.LoadScroll();
                                return;
                            }
                            return;
                        }));
                    }
                }
                try
                {
                    dock.ActiveWindowChanged -= radDock2_ActiveWindowChanged;
                }
                finally
                {
                    dock.ActiveWindowChanged += radDock2_ActiveWindowChanged;
                }
                if (MActiveChart != null)
                {
                    // MActiveChart.LoadWorkspaceRight();
                    // MActiveChart.StockChartX1.ForcePaint();
                }

                //Resize Select Tools:
                _selectview.DefaultFloatingSize = new System.Drawing.Size(300, 500);
                _selectview.AutoHideSize = new System.Drawing.Size(300, 500);
                if (_selectview.Controls.Count > 0)
                {
                    _selectview.Controls[0].Dock = DockStyle.Fill;
                    ((SelectView1)_selectview.Controls[0]).ResizeControls();
                }
            }
            catch (Exception exception)
            {
                Telerik.WinControls.RadMessageBox.Show("\nStateStateRestored() " + exception.Message, " ");
                throw;
            }

        }

        public static void XmlChartPropriedades(CtlPainelChart chart, XmlNode propriedades)
        {
            if (chart == null) return;
            chart.StockChartX1.ScaleType = propriedades["VIEW_SCALE_SEMI_LOG"].InnerText.Equals("0")
                                               ? ScaleType.stLinearScale
                                               : ScaleType.stSemiLogScale;
            chart.StockChartX1.XGrid = propriedades["SHOW_XGRID"].InnerText == "0" ? false : true;
            chart.StockChartX1.YGrid = propriedades["SHOW_YGRID"].InnerText == "0" ? false : true;
            chart.StockChartX1.HorizontalSeparators = propriedades["SHOW_SEPARATORS"].InnerText == "0" ? false : true;
            chart.StockChartX1.DarvasBoxes = propriedades["SHOW_DARVAS_BOXES"].InnerText == "0" ? false : true;
            chart.m_SchemeColor = propriedades["COLOR"].InnerText;
            // Scheme.Instance().UpdateChartColors(chart.StockChartX1, chart.m_SchemeColor);
            // chart.UpdateMenus();

        }

        private void ActiveWindowChanged(DockWindow NewWindow)
        {
            bool enable = true;

            if (NewWindow.Controls.Count == 0 || NewWindow != NewWindow.DockManager.DocumentManager.ActiveDocument)
            {
                return;
            }

            //Console.WriteLine("\n\nActive Window changed to " + NewWindow.Text);
            //Disable the controls if not the same type of form
            if (_mActiveDocumentName != NewWindow.AccessibleName || NewWindow.AccessibleName != "CtlPainelChart")
            {
                enable = false;
            }
            if (!enable)
            {
                EnableControls(false);
            }
            if (NewWindow.AccessibleName == "ctlChart")
            {
                MActiveChart = (CtlPainelChart)NewWindow.Controls[0];
                MActiveChart.DrawSelection();
                MActiveChart.UpdateMenus();
                MActiveChart.EnableControls(true);
                MActiveChart.LoadStockPortfolioActive();
            }
            else if (NewWindow.AccessibleName == "CtlPainelChart" && NewWindow.DockState != DockState.Floating)
            {
                //Console.WriteLine("MActiveChart " + NewWindow.Text + " selected.\n");
                if (NewWindow.Controls.Count > 0)
                {
                    MActiveChart = (CtlPainelChart)NewWindow.Controls[0];
                    MActiveChart.DrawSelection();
                    MActiveChart.UpdateMenus();
                    UpdateMnuColor();
                    MActiveChart.EnableControls(true);

                    if (_actionChart != ActionChart.NONE)
                        MActiveChart.ActiveChartChanged = true;


                    MActiveChart.LoadStockPortfolioActive();
                }
            }
            else
            {
                if (MActiveChart != null)
                {
                    MActiveChart = null;
                    _actionChart = ActionChart.NONE;
                }
            }

            _mActiveDocumentName = NewWindow.AccessibleName;
        }

        void radDock2_ActiveWindowChanged(object sender, DockWindowEventArgs e)
        {

            ActiveWindowChanged(e.DockWindow);
            if ((e.DockWindow.ActiveControl == null) || (!e.DockWindow.AccessibleName.Equals("CtlPainelChart")))
                return;
            if ((MActiveChart == null) || (!MActiveChart.StateDummy))
                return;

            CtlPainelChart oldChart = (CtlPainelChart)e.DockWindow.ActiveControl;

            if (!File.Exists(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml"))
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] + "\\" + "ATIVOS_" + _archiveNameWorkspace + ".xml");
            XmlNodeList nodeListAtivos = xmlDocument.GetElementsByTagName("ATIVO");
            XmlNode node = nodeListAtivos.Cast<XmlNode>().Where(nodeListAtivo => e.DockWindow.Name.Equals(nodeListAtivo["NOME_TOOLWINDOW"].InnerText)).
                        FirstOrDefault();
            XmlNode propriedades = node["PROPRIEDADES"];
            XmlChartPropriedades(((CtlPainelChart)e.DockWindow.ActiveControl), propriedades);
            string windowName = ListWorkspace._path + _archiveNameWorkspace.Split('.')[0] +
                                                            "\\" + e.DockWindow.Name + ".sct";
            ((CtlPainelChart)e.DockWindow.ActiveControl).InitRTChartAsync(b => ((CtlPainelChart)e.DockWindow.ActiveControl)._asyncOp.Post(() =>
            {
                if (b)
                {
                    ((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1.FirstVisibleRecord = int.Parse(propriedades["FIRST_VISIBLE_RECORD"].InnerText);
                    ((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1.LastVisibleRecord = int.Parse(propriedades["LAST_VISIBLE_RECORD"].InnerText);
                    ((CtlPainelChart)e.DockWindow.ActiveControl).BindContextMenuEvents();
                    ((CtlPainelChart)e.DockWindow.ActiveControl).m_SchemeColor = propriedades["COLOR"].InnerText;
                    Scheme.Instance().UpdateChartColors(((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1, ((CtlPainelChart)e.DockWindow.ActiveControl).m_SchemeColor);
                    MActiveChart = ((CtlPainelChart)e.DockWindow.ActiveControl);
                    ((CtlPainelChart)e.DockWindow.ActiveControl).LoadDataTemplate();
                    ((CtlPainelChart)e.DockWindow.ActiveControl).UpdateMenus();
                    ((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1.Visible = true;
                    ((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1.Width = ((CtlPainelChart)e.DockWindow.ActiveControl).Width - 4;
                    //LoadColorScheme(((CtlPainelChart)e.DockWindow.ActiveControl));
                    ((CtlPainelChart)e.DockWindow.ActiveControl).StateDummy = false;
                    if (File.Exists(windowName))
                        ((CtlPainelChart)e.DockWindow.ActiveControl).StockChartX1.LoadGeneralTemplate(windowName);
                    ((CtlPainelChart)e.DockWindow.ActiveControl).LoadScroll();
                    return;
                }
                return;
            }));
            //if (MActiveChart != null)
            //{

            //    //Console.WriteLine("\n\t2- " + DateTime.Now.TimeOfDay + " Main:CheckAlerts() ");
            //    var check = (RadCheckBoxElement)MActiveChart.cmdCheckPortfolio.HostedItem;

            //    MActiveChart = new CtlPainelChart(this, _mCtlData,
            //                                      (string)reqOp.OParams[1],

            //                                      (Periodicity)reqOp.OParams[2],
            //                                      (int)reqOp.OParams[3],
            //                                      (int)reqOp.OParams[4],


            //                                      (string)reqOp.OParams[5]) { Dock = DockStyle.Fill };
            //    LoadChartSettings(MActiveChart);
            //    if (check != null)
            //        MActiveChart.cmdCheckPortfolio.HostedItem = check;
            //}

            /*else
            {
                //MActiveChart.Dispose();
                MActiveChart = new CtlPainelChart(this, _mCtlData,
                                                  (string)reqOp.OParams[1],
                                                  (Periodicity)reqOp.OParams[2],
                                                  (int)reqOp.OParams[3],
                                                  (int)reqOp.OParams[4],
                                                  (string)reqOp.OParams[5]) { Dock = DockStyle.Fill };
                LoadChartSettings(MActiveChart);
            }*/

            //foreach (DockWindow document in radDock2.DockWindows)
            //{


            //    if (document == reqOp.OParams[0])
            //    {

            //        document.Controls.Clear();
            //        document.Controls.Add(MActiveChart);
            //        document.Text = MActiveChart.GetChartTitle();
            //    }

            //    if (document.AccessibleName == ((DockWindow)reqOp.OParams[0]).AccessibleDescription)
            //    {

            //        document.Text = MActiveChart.GetChartTitle();
            //    }
            //}



        }
        #endregion

        #region Threads Functions

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

        public void AddRequestedOperation(Operations operation)
        {
            Thread threadReq = new Thread(() => ThreadRequestedOp(operation));
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

        private void CheckAlerts()
        {
            int delIndex = -1;
            _messageRequests = MessageService.GetRequest(MSRequestOwner.M4);
            MSRequest request;
            // Process messages:
            while (_messageRequests.Count > 0)
            {
                delIndex = -1;
                request = _messageRequests.Dequeue();
                switch (request.MSType)
                {
                    case MSRequestType.MessageStatus:
                        while (!_preLoadEnd) Thread.Sleep(500);
                        switch ((MSMessageStatusType)request.MSParams[0])
                        {
                            case MSMessageStatusType.UpdatedAll:
                                _statusManager.SetMessage("Todos os ativos foram atualizados até a data " + (string)request.MSParams[1], OutputWindowV2.OutputIcon.Chart);
                                //radDesktopAlert1.ThemeName = Windows7Theme.ThemeName;//VisualStudio2012LightTheme.ThemeName;
                                radDesktopAlert1.ContentImage = Properties.Resources.PageRedo;
                                radDesktopAlert1.CaptionText = "ATIVOS ATUALIZADOS";
                                radDesktopAlert1.ContentText = "Todos os ativos foram atualizados até a data " + (string)request.MSParams[1];
                                radDesktopAlert1.Show();
                                break;
                            case MSMessageStatusType.InsertedSymbol:
                                _statusManager.SetMessage("O ativo " + (string)request.MSParams[2] + " foi atualizado até a data " + (string)request.MSParams[1], OutputWindowV2.OutputIcon.Chart);
                                //radDesktopAlert1.ThemeName = Windows7Theme.ThemeName;//VisualStudio2012LightTheme.ThemeName;
                                radDesktopAlert1.ContentImage = Properties.Resources.PageRedo;
                                radDesktopAlert1.CaptionText = (string)request.MSParams[2]+" ATUALIZADO";
                                radDesktopAlert1.ContentText = "O ativo " + (string)request.MSParams[2] + " foi atualizado até a data " + (string)request.MSParams[1];
                                radDesktopAlert1.Show();
                                break;
                            case MSMessageStatusType.InsertedSymbols:
                                _statusManager.SetMessage("Os ativos inseridos foram atualizados até a data " + (string)request.MSParams[1], OutputWindowV2.OutputIcon.Chart);
                                //radDesktopAlert1.ThemeName = Windows7Theme.ThemeName;//VisualStudio2012LightTheme.ThemeName;
                                radDesktopAlert1.ContentImage = Properties.Resources.PageRedo;
                                radDesktopAlert1.CaptionText = "ATIVOS ATUALIZADOS";
                                radDesktopAlert1.ContentText = "Os ativos inseridos foram atualizados até a data " + (string)request.MSParams[1];
                                radDesktopAlert1.Show();
                                break;

                        }
                        break;
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
                                    tmrMessageService.Enabled = false;
                                }
                            }
                            if (delIndex != -1) requestedOperations.RemoveAt(delIndex);
                        }
                        break;
                    case MSRequestType.GetHistoricalData:
                        if (request.MSStatus == MSRequestStatus.Failed)
                        {
                            //TODO: tell user there's no data for symbol!
                            Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgInvalidPeriodicity"] +
                                            request.MSParams[0], " ");
                        }
                        else if (request.MSStatus == MSRequestStatus.Processing)
                        {
                            //TODO: tell user that server is getting data!
                        }
                        else if (request.MSStatus == MSRequestStatus.Done)
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
                                            case TypeOperations.LoadSelectView:
                                                // Re-load select view:
                                                ReloadSelectTools();
                                                break;
                                            case TypeOperations.CreateNewCtlPainelChart:
                                                //CreateNewCtlPainel((ChartSelection)reqOp.OParams[0]);
                                                break;
                                            case TypeOperations.LoadCtlPainelChart:
                                                //var check = (RadCheckBoxElement)MActiveChart.cmdCheckPortfolio.HostedItem;
                                                //MActiveChart = new CtlPainelChart(this, _mCtlData,
                                                //                                  (string)reqOp.OParams[1],
                                                //                                  (Periodicity)reqOp.OParams[2],
                                                //                                  (int)reqOp.OParams[3],
                                                //                                  (int)reqOp.OParams[4],
                                                //                                  (string)reqOp.OParams[5]) { Dock = DockStyle.Fill };
                                                //MActiveChart.LoadCtlPainelChart(_mCtlData,
                                                //                                  (string)reqOp.OParams[1],
                                                //                                  (Periodicity)reqOp.OParams[2],
                                                //                                  (int)reqOp.OParams[3],
                                                //                                  (int)reqOp.OParams[4],
                                                //                                  (string)reqOp.OParams[5]);
                                                //LoadChartSettings(MActiveChart);
                                                //MActiveChart.cmdCheckPortfolio.HostedItem = check;
                                                /*CtlPainelChart chart;
                                                foreach (DockWindow document in radDock2.DockWindows)
                                                {

                                                    if (document == reqOp.OParams[0])
                                                    {
                                                        
                                                        if (document.Controls.Count > 0)
                                                        {
                                                            chart = (CtlPainelChart) document.Controls[0];
                                                            //var check = (RadCheckBoxElement)MActiveChart.cmdCheckPortfolio.HostedItem;
                                                            chart.LoadCtlPainelChart(_mCtlData,
                                                                                      (string)reqOp.OParams[1],
                                                                                      (Periodicity)reqOp.OParams[2],
                                                                                      (int)reqOp.OParams[3],
                                                                                      (int)reqOp.OParams[4],
                                                                                      (string)reqOp.OParams[5]);
                                                            LoadChartSettings(chart);
                                                            //MActiveChart.cmdCheckPortfolio.HostedItem = check;
                                                            //document.Controls.Clear();
                                                            //document.Controls.Add(MActiveChart);
                                                            document.Text = chart.GetChartTitle();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("\n\nError ao atualizar o Chart");
                                                        }

                                                        break;
                                                    }
                                                }*/
                                                break;
                                        }
                                        _evArrived.Set();
                                    }
                                }
                                if (delIndex != -1 && requestedOperations.Count > 0) requestedOperations.RemoveAt(delIndex);
                            }




                        }
                        break;
                }
            }
        }
        #endregion

        #region DB Functions

        public static List<Symbol> GetStockList()
        {
            List<Symbol> Result = new List<Symbol>();
            lock (StockList)
            {
                Result = StockList;
            }
            return Result;
        }

        public static List<Symbol> GetStockListAll()
        {
            List<Symbol> Result = new List<Symbol>();
            SqlConnection _con = DBlocalSQL.Connect();
            Result = DBlocalSQL.LoadSymbols(_con);
            DBlocalSQL.Disconnect(_con);
            return Result;
        }

        public static BarData GetLastBarData(string Symbol)
        {
            BarData result;
            SqlConnection _connection = DBlocalSQL.Connect();
            result = DBlocalSQL.GetLastBarDataDiskOrMemory(Symbol, BaseType.Days, _connection);
            DBlocalSQL.Disconnect(_connection);
            return result;
        }

        public static double[] GetLastVolumes(string Symbol)
        {
            double[] result = new double[10];
            SqlConnection _connection = DBlocalSQL.Connect();
            result = DBlocalSQL.GetLastVolumes(Symbol, 1, (int)Periodicity.Daily, _connection);
            DBlocalSQL.Disconnect(_connection);
            return result;
        }

        public static string[] GetLastDates(string Symbol)
        {
            string[] result = new string[10];
            SqlConnection _connection = DBlocalSQL.Connect();
            result = DBlocalSQL.GetLastDates(Symbol, 1, (int)Periodicity.Daily, _connection);
            DBlocalSQL.Disconnect(_connection);
            return result;
        }
        #endregion

        #region ImplementationDragDropRadDock

        private Control OnDragWindow;
        private bool OnDragWindowIsSelectView;


        private void WireRadDockEvents()
        {
            DragDropService service2 = radDock2.GetService<DragDropService>();
            service2.Starting += new StateServiceStartingEventHandler(OnDragDropService_Starting);
            service2.PreviewDropTarget += new DragDropTargetEventHandler(OnDragDropService_PreviewDropTarget);
            service2.PreviewDockPosition += new DragDropDockPositionEventHandler(OnDragDropService_PreviewDockPosition);
            service2.Stopped += new EventHandler(OnDragDropService_Stopped);
        }

        void OnDragDropService_Stopped(object sender, EventArgs e)
        {
            OnDragWindow = null;
            DragDropService service2 = radDock2.GetService<DragDropService>();
            service2.AllowedDockManagerEdges = AllowedDockPosition.All;

            //PERCORRER TODOS OS DOCUMENTOS DO MAIN PARA ATUALIZAR GRÁFICOS ABERTOS:
            foreach (DockWindow document in documentManager.Where(document =>
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) &&
                (!document.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))).Where(document =>
                    document.AccessibleName.Equals("CtlPainelChart")))
            {
                ((CtlPainelChart)document.Controls[0]).StockChartX1.Update();
            }
        }

        private void OnDragDropService_PreviewDockPosition(object sender, DragDropDockPositionEventArgs e)
        {

            DragDropService service2 = radDock2.GetService<DragDropService>();
            if (OnDragWindow == null)
            {
                return;
            }
            if (OnDragWindow.GetType().ToString().Contains("FloatingWindow")
                && !(OnDragWindow.Text == Program.LanguageDefault.DictionarySelectTools["selectTools"]
                || OnDragWindow.Text == Program.LanguageDefault.DictionaryPlena["statusManager"]))
            {
                e.AllowedDockPosition = AllowedDockPosition.Fill;
                service2.AllowedDockManagerEdges = AllowedDockPosition.None;
            }
            else
            {
                if (e.DropTarget == documentContainer2)
                {
                    e.AllowedDockPosition = AllowedDockPosition.All & ~AllowedDockPosition.Fill;
                }
                else
                {
                    e.AllowedDockPosition = AllowedDockPosition.All;
                }
            }
        }

        private void OnDragDropService_PreviewDropTarget(object sender, DragDropTargetEventArgs e)
        {
            if (OnDragWindow == null || e.DropTarget == null) return;
            if (OnDragWindow.AccessibleName == "CtlPainelChart" && OnDragWindow.GetType().ToString().Contains("FloatingWindow"))
            {
                if (e.DropTarget != documentContainer2)
                {
                    //Console.WriteLine("\n\n  OnDragDropService_PreviewDropTarget() !doc2");
                    e.DropTarget = null;
                }
            }
        }

        private void OnDragDropService_Starting(object sender, StateServiceStartingEventArgs e)
        {
            Control context = e.Context as Control;
            if (context == null)
            {
                return;
            }

            OnDragWindow = (context);

        }

        #endregion


        private void radDock2_AutoHideWindowDisplaying(object sender, AutoHideWindowDisplayingEventArgs e)
        {
            if (e.NewWindow == _selectview)
            {
                e.NewWindow.AutoHideSize = new Size(278, 100);
                if (e.NewWindow.Parent.Parent is AutoHidePopup)
                {
                    e.NewWindow.Parent.Parent.MinimumSize = new Size(278, 0);
                    e.NewWindow.Parent.Parent.LocationChanged -= Parent_LocationChanged;
                    e.NewWindow.Parent.Parent.LocationChanged += Parent_LocationChanged;
                }
                //radDock2.GetAutoHideTabStrip(AutoHidePosition.Left).MinSize = new Size(278, 0);
                //stripitem.MinSize = new Size(280, 280);
                //stripitem.MaxSize = new Size(290, 290);
                ToolTabStrip strip = (ToolTabStrip)e.NewWindow.TabStrip;
                strip.SizeInfo.MinimumSize = new Size(278, 100);
                _select.Dock = DockStyle.Fill;
            }
        }
        
        void Parent_LocationChanged(object sender, EventArgs e)
        {
            AutoHidePopup hide = (AutoHidePopup)sender;
            Point f = hide.DesktopLocation;
            if (f.X > this.Width - 349)
            {
                hide.Location = new Point(this.Width - 349, hide.Location.Y);
            }
        }

        public static PortfolioDataSet GetPortfolioView(List<string>assetList, bool Memory = true)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            PortfolioDataSet result = new PortfolioDataSet();
            SqlConnection _con = DBlocalSQL.Connect();
            result = DBlocalSQL.GetPortfolioView(_con, assetList, Memory);
            DBlocalSQL.Disconnect(_con);
            Console.WriteLine("\tGetPortfolioView("+timer.ElapsedMilliseconds+"ms)");
            return result;
        }
        void _selectview_SizeChanged(object sender, EventArgs e)
        {
            //  _selectview.AutoHideSize = new Size(278, 100);          
        }

        //Facebook Service 6.0.0
        public static bool ConnectFacebook()
        {
            try
            {
                facebookAccessToken = "";
                facebookLoginDialog = new FacebookLoginDialog(facebookAppId, facebookPermissions);
                facebookLoginDialog.ShowDialog();
                facebookAccessToken = facebookLoginDialog.FacebookOAuthResult.AccessToken;
                DisplayFacebookMessage(facebookLoginDialog.FacebookOAuthResult);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool PublishFacebook(string FileName)
        {
            try
            {
                var picture = File.ReadAllBytes(FileName);
                var fb = new FacebookClient(facebookAccessToken);

                fb.PostCompleted +=
                    (o, args) =>
                    {
                        if (args.Error == null)
                        {
                            MessageBox.Show("Picture posted to wall successfully.");
                        }
                        else
                        {
                            MessageBox.Show(args.Error.Message);
                        }
                    };

                dynamic parameters = new ExpandoObject();
                parameters.caption = "TESTE: Análise técnica utilizando Plena Trading Platform";
                parameters.method = "facebook.photos.upload";

                var mediaObject = new FacebookMediaObject
                {
                    FileName = FileName,
                    ContentType = "image/png"
                };
                mediaObject.SetValue(picture);
                parameters.source = mediaObject;

                fb.PostAsync(parameters);
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        private static void DisplayFacebookMessage(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult == null)
            {
                // most likely user closed the FacebookLoginDialog, so do nothing
                return;
            }

            if (facebookOAuthResult.IsSuccess)
            {
                // we got the access token
                //var infoDialog = new Info(facebookOAuthResult.AccessToken);
                //infoDialog.ShowDialog();
                //RadMessageBox.Show("Login Success!");
            }
            else
            {
                // for some reason we failed to get the access token.
                // most likely the user clicked don't allow
                MessageBox.Show(facebookOAuthResult.ErrorDescription);
            }
        }

        private void mnuAlerts_Click(object sender, EventArgs e)
        {
            if (documentTabStrip2.Controls.Contains(_toolAlerts)) documentTabStrip2.ActiveWindow = _toolAlerts;
            else
            {
                _toolAlerts = new ToolWindow("Alerts")
                {
                    AccessibleName = "_toolAlerts",
                    DockState = DockState.TabbedDocument,
                    CloseAction = DockWindowCloseAction.Close,
                    Text = "Alarmes",
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                };
                try
                {
                    //_toolAlerts.Controls.Add(_alerts);
                    LoadAlerts();
                    DocumentTabStrip tabStrip = (DocumentTabStrip)radDock2.DocumentManager.ActiveDocument.Parent;
                    _toolAlerts.Move += CtlWindow_DockChanged;
                    tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                    _toolAlerts.DockTo(radDock2.DocumentManager.ActiveDocument, DockPosition.Fill);
                    _toolAlerts.AccessibleDescription = _toolAlerts.Name.Substring(10);

                    _toolAlerts.DockState = DockState.TabbedDocument;
                    radDock2.ActivateWindow(_toolAlerts);
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
                }
            }
        }

        private void mnuScanner_Click(object sender, EventArgs e)
        {
            if (documentTabStrip2.Controls.Contains(_toolScanner)) documentTabStrip2.ActiveWindow = _toolScanner;
            else
            {
                _toolScanner = new ToolWindow("Scanner")
                {
                    AccessibleName = "_toolScanner",
                    DockState = DockState.TabbedDocument,
                    CloseAction = DockWindowCloseAction.Close,
                    Text = "Scanner",
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                };
                try
                {
                    //_toolScanner.Controls.Add(_scanner);
                    LoadScanner();
                    DocumentTabStrip tabStrip = (DocumentTabStrip)radDock2.DocumentManager.ActiveDocument.Parent;
                    _toolScanner.Move += CtlWindow_DockChanged;
                    tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                    _toolScanner.DockTo(radDock2.DocumentManager.ActiveDocument, DockPosition.Fill);
                    _toolScanner.AccessibleDescription = _toolScanner.Name.Substring(10);

                    _toolScanner.DockState = DockState.TabbedDocument;
                    radDock2.ActivateWindow(_toolScanner);
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
                }
            }
        }

        private void mnuBackTest_Click(object sender, EventArgs e)
        {
            if (documentTabStrip2.Controls.Contains(_toolBackTest)) documentTabStrip2.ActiveWindow = _toolBackTest;
            else
            {
                _toolBackTest = new ToolWindow("Back Test")
                {
                    AccessibleName = "_toolBackTest",
                    DockState = DockState.TabbedDocument,
                    CloseAction = DockWindowCloseAction.Close,
                    Text = "Back Test",
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                };
                try
                {
                    //_toolBackTest.Controls.Add(_backTest);
                    LoadBackTest();
                    DocumentTabStrip tabStrip = (DocumentTabStrip)radDock2.DocumentManager.ActiveDocument.Parent;
                    _toolBackTest.Move += CtlWindow_DockChanged;
                    tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                    _toolBackTest.DockTo(radDock2.DocumentManager.ActiveDocument, DockPosition.Fill);
                    _toolBackTest.AccessibleDescription = _toolBackTest.Name.Substring(10);

                    _toolBackTest.DockState = DockState.TabbedDocument;
                    radDock2.ActivateWindow(_toolBackTest);
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
                }
            }
        }

        private void mnuScript_Click(object sender, EventArgs e)
        {
            if (documentTabStrip2.Controls.Contains(_toolScript)) documentTabStrip2.ActiveWindow = _toolScript;
            else
            {
                _toolScript = new ToolWindow("Script Editor")
                {
                    AccessibleName = "_toolScript",
                    DockState = DockState.TabbedDocument,
                    CloseAction = DockWindowCloseAction.Close,
                    Text = "Script Editor",
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                };
                try
                {
                    //_toolScript.Controls.Add(_script);
                    LoadScriptEditor();
                    DocumentTabStrip tabStrip = (DocumentTabStrip)radDock2.DocumentManager.ActiveDocument.Parent;
                    _toolScript.Move += CtlWindow_DockChanged;
                    tabStrip.DockManager.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
                    _toolScript.DockTo(radDock2.DocumentManager.ActiveDocument, DockPosition.Fill);
                    _toolScript.AccessibleDescription = _toolScript.Name.Substring(10);

                    _toolScript.DockState = DockState.TabbedDocument;
                    radDock2.ActivateWindow(_toolScript);
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show(ex.Message, " ");
                }
            }
        }


        /* Facebook Service 3.0.1
        public static bool ConnectFacebook()
        {
            try
            {
                //Start facebook service:

                // The application key of the Facebook application used
                fbService.ApplicationKey = "1520253931529844";

                // Add all needed permissions
                List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
                                                    {
                                                                        Enums.ExtendedPermissions.create_note,
                                                                        Enums.ExtendedPermissions.publish_stream,
                                                                        Enums.ExtendedPermissions.status_update,
                                                                        Enums.ExtendedPermissions.photo_upload
                                                                    };
                fbService.ConnectToFacebook(perms);
            }
            catch (Exception ex)
            {
                if(!ex.Message.Contains("dictionary")){
                RadMessageBox.Show("Erro ao conectar em sua conta!");
                facebookLogged = false;
                return false;}
            }
            facebookLogged = true;
            return true;
        }
        public static bool PublishFacebook(MemoryStream imagePng)
        {
            // Just for demonstration////////////////////////////////////////////////
           
            // The origin of the screen shot
            Point origin = new Point(0, 0);

            // The size of it
            Point size = new Point(800, 600);

            /////////////////////////////////////////////////////////////////////////

            // Used for the tagging feature//////////////////////////////////////////

            // Uids of friends to be tagged
            facebookFriendsUids = new List<long>();

            // Corresponding positions
            //Note: Positions are in terms of percentage relative to screen shot size 
            facebookFriendsPositions = new List<Point>();

            /////////////////////////////////////////////////////////////////////////

            try
            {
                //screenShotFormatter = new ScreenShotFormatter(origin.X, origin.Y, size.X, size.Y);

                IList<album> albums = fbService.Photos.GetAlbums();

                string albumAid = "";
                foreach (album album in albums)
                {
                    // Album name to create - if doesn't exist - is "Plena Album"
                    if (album.name == "Plena Album")
                    {
                        albumAid = album.aid;
                        break;
                    }
                }

                // If not found, create it
                if (albumAid == "")
                {
                    fbService.Photos.CreateAlbumAsync("Plena Album", null, "Screenshots by Plena Trading Platform", CreateAlbumFacebookCallback, imagePng);
                    return true;
                }
                fbService.Photos.UploadAsync(albumAid,
                    "Analise técnica utilizando a ferramenta Plena Trading Platform.",
                    imagePng.ToArray(),
                    Enums.FileType.png,
                    UploadFacebookCallback,
                    null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void CreateAlbumFacebookCallback(album album, object state, FacebookException e)
        {
            fbService.Photos.UploadAsync(album.aid,
                    "Analise técnica utilizando a ferramenta Plena Trading Platform.",
                    ((MemoryStream)state).ToArray(),
                Enums.FileType.png,
                UploadFacebookCallback,
                null);
        }

        private static void UploadFacebookCallback(photo p, object state, FacebookException e)
        {
            if (facebookFriendsUids != null && facebookFriendsPositions != null)
                PhotoTaggerFacebook(p.pid);
        }

        private static void PhotoTaggerFacebook(string photoPid)
        {
            for (int i = 0; i < facebookFriendsUids.Count; i++)
            {
                fbService.Photos.AddTag(photoPid, facebookFriendsUids[i], null, facebookFriendsPositions[i].X, facebookFriendsPositions[i].Y);
            }
        }

        */


     
        
     
    }
}
