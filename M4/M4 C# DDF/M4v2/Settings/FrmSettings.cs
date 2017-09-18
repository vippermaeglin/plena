using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using M4.M4v2.Chart;
using M4.M4v2.Chart.IndicatorElements;
using M4Core.Entities;
using M4Data.List;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using M4Utils;
using M4Data.Settings;

namespace M4.M4v2.Settings
{
    public partial class FrmSettings : RadForm
    {
        private readonly frmMain2 _mFrMain2;
        public static bool _userChanged = false;
        public static bool _userSaving = false;

        public FrmSettings(frmMain2 mainForm)
        {
            _mFrMain2 = mainForm;

            InitializeComponent();
            LoadProxy();

            LoadConfigProxy();

            TranslateForm();

            tabChart.BackColor = Utils.GetDefaultBackColor();
            tabProxy.BackColor = Utils.GetDefaultBackColor();
            tabStudies.BackColor = Utils.GetDefaultBackColor();
            tabPrice.BackColor = Utils.GetDefaultBackColor();
            tabUser.BackColor = Utils.GetDefaultBackColor();
        }

        private void TranslateForm()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            Text = Program.LanguageDefault.DictionarySettings["frmOptions"];

            btnOk.Text = Program.LanguageDefault.DictionarySettings["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionarySettings["btnCancel"];

            tabProxy.Text = Program.LanguageDefault.DictionarySettings["tabProxy"];
            tabChart.Text = Program.LanguageDefault.DictionarySettings["tabChart"];
            tabStudies.Text = Program.LanguageDefault.DictionarySettings["tabStudies"];
            tabPrice.Text = Program.LanguageDefault.DictionarySettings["tabPrice"];
            tabUser.Text = Program.LanguageDefault.DictionarySettings["tabUser"];
        }

        private void GridStudies()
        {
            for (int i = 0; i < 3; i++)
                AppendNewColumn(i);

            ConfigStudies configStudies = ListConfigStudies.Instance().LoadListConfigStudies();

            rseLineThickness.Value = configStudies.LineThickness;

            boxColor.Value = Color.FromArgb(configStudies.Color.ToArgb());

            string[] valuesFibonacci = new[]
                                           {
                                               "0%",
                                               "38,2%",
                                               "50%",
                                               "61,8%",
                                               "100%",
                                               "161,8%",
                                               "261,8%"
                                           };

            int index = 0;
            foreach (string value in valuesFibonacci)
            {
                GridViewRowInfo rowInfo = grdFibonacci.Rows.AddNew();
                rowInfo.Cells[0].Value = value;
                rowInfo.Cells[0].ReadOnly = true;

                if ((value == "161,8%") || (value == "261,8%"))
                {
                    rowInfo.Cells[1].Value = false;
                    rowInfo.Cells[1].ReadOnly = true;
                }
                else
                {
                    rowInfo.Cells[1].Value = bool.Parse(configStudies.Retracements[index, 1]);
                    rowInfo.Cells[1].ReadOnly = false;
                }

                rowInfo.Cells[2].Value = bool.Parse(configStudies.Projections[index, 1]);
                rowInfo.Cells[2].ReadOnly = false;

                index++;
            }

            grdFibonacci.CurrentRow = grdFibonacci.Rows[0];
        }

        private void InitializeGrids()
        {
            GridStudies();
        }

        private void AppendNewColumn(int column)
        {
            GridViewDataColumn newColumn = null;
            switch (column)
            {
                case 0:
                    newColumn = new GridViewMaskBoxColumn { Name = "", EnableExpressionEditor = true, Width = 50, HeaderText = "" };
                    break;
                case 1:
                    newColumn = new GridViewCheckBoxColumn
                                    {
                                        Name = "Retracements",
                                        EnableExpressionEditor = true,
                                        Width = 100,
                                        HeaderText = Program.LanguageDefault.DictionarySettings["columnRetracements"]
                                    };
                    break;
                case 2:
                    newColumn = new GridViewCheckBoxColumn
                                    {
                                        Name = "Projections",
                                        EnableExpressionEditor = true,
                                        Width = 100,
                                        HeaderText = Program.LanguageDefault.DictionarySettings["columnProjections"]
                                    };
                    break;
            }

            grdFibonacci.Columns.Add(newColumn);
        }

        private void LoadConfigProxy()
        {
            txtPortProxy.Text = Properties.Settings.Default.PortProxy;
            txtServerURL.Text = Properties.Settings.Default.ServerURL;
            txtPortProxySocks.Text = Properties.Settings.Default.PortProxySocks;
            txtSuperscriptionProxy.Text = Properties.Settings.Default.SuperscriptionProxy;
            txtSuperscriptionProxySocks.Text = Properties.Settings.Default.SuperscriptionProxySocks;
            txtUserAuthenticationProxy.Text = Properties.Settings.Default.UserAuthenticationProxy;
            txtPasswordAuthenticationProxy.Text = Properties.Settings.Default.PasswordAuthenticationProxy;

            optNotProxy.Text = Program.LanguageDefault.DictionarySettings["optNotProxy"];
            optProxySocks.Text = Program.LanguageDefault.DictionarySettings["optProxySocks"];
            optProxyServer.Text = Program.LanguageDefault.DictionarySettings["optProxyServer"];
            cbxAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["cbxAuthenticationProxy"];
            optConfigProxyNavigator.Text = Program.LanguageDefault.DictionarySettings["optConfigProxyNavigator"];
            lblSuperscriptionProxy.Text = Program.LanguageDefault.DictionarySettings["Address"];
            lblSuperscriptionProxySocks.Text = Program.LanguageDefault.DictionarySettings["Address"];
            lblPortProxy.Text = Program.LanguageDefault.DictionarySettings["Port"];
            lblPortProxySocks.Text = Program.LanguageDefault.DictionarySettings["Port"];
            lblUserAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["lblUser"];
            lblPasswordAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["lblPassword"];

            if (Properties.Settings.Default.NotProxy)
                optNotProxy.IsChecked = true;
            else if (Properties.Settings.Default.ConfigProxyNavigator)
                optConfigProxyNavigator.IsChecked = true;
            else if (Properties.Settings.Default.ProxyServer)
            {
                txtPortProxy.Enabled = true;
                optProxyServer.IsChecked = true;
                txtSuperscriptionProxy.Enabled = true;
            }
            else if (Properties.Settings.Default.ProxySocks)
            {
                optProxySocks.IsChecked = true;
                txtPortProxySocks.Enabled = true;
                txtSuperscriptionProxySocks.Enabled = true;
            }

            if (!Properties.Settings.Default.AuthenticationProxy)
                return;

            if ((Properties.Settings.Default.ProxyServer) || (Properties.Settings.Default.ProxySocks))
                cbxAuthenticationProxy.IsChecked = true;
        }

        private void LoadConfigServer()
        {
           
        }

        private void LoadConfigChart()
        {
            ConfigurePropertyGridChart();

            ChartInfo chartInfo = new ChartInfo
            {
                GridHorizontal = Properties.Settings.Default.GridHorizontal,
                GridVertical = Properties.Settings.Default.GridVertical,
                SemiLogScale = Properties.Settings.Default.SemiLogScale,
                PanelSeparator = Properties.Settings.Default.PanelSeparator,
                Decimals = int.Parse(Properties.Settings.Default.Decimals.ToString()),
                History = Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") && int.Parse(Properties.Settings.Default.History.ToString()) > 300 ? 300 : int.Parse(Properties.Settings.Default.History.ToString()),
                ChartViewport = Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") && int.Parse(Properties.Settings.Default.ChartViewport.ToString())>300 ? 300 : int.Parse(Properties.Settings.Default.ChartViewport.ToString()),
                PaddingTop = int.Parse(Properties.Settings.Default.PaddingTop.ToString()),
                PaddingBottom = int.Parse(Properties.Settings.Default.PaddingBottom.ToString()),
                PaddingRight = int.Parse(Properties.Settings.Default.PaddingRight.ToString()),
                VisiblePortfolio = Properties.Settings.Default.VisiblePortfolio,
            };

            switch (Properties.Settings.Default.SchemeColor)
            {
                case "SchemeSky":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeSky"];
                    break;
                case "SchemeBeige":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeBeige"];
                    break;
                case "SchemeBlue":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeBlue"];
                    break;
                case "SchemeDark":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeDark"];
                    break;
                case "SchemeGreen":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeGreen"];
                    break;
                case "SchemeMono":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeMono"];
                    break;
                case "SchemeWhite":
                    chartInfo.ColorSchemes = Program.LanguageDefault.DictionarySettings["SchemeWhite"];
                    break;
            }

            switch (Properties.Settings.Default.TabDataPosition)
            {
                case "Less":
                    chartInfo.Position = Program.LanguageDefault.DictionarySettings["LessChart"];
                    break;
                case "Bottom":
                    chartInfo.Position = Program.LanguageDefault.DictionarySettings["BottomChart"];
                    break;
            }
            string defaultPeriodicity = Properties.Settings.Default.DefaultPeriodicity;
            switch (defaultPeriodicity)
            {
                case "Daily":
                    chartInfo.Periodicity = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"];
                    break;
                case "Weekly":
                    chartInfo.Periodicity = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"];
                    break;
                case "Month":
                    chartInfo.Periodicity = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"];
                    break;
                case "Yearly":
                    chartInfo.Periodicity = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityYearly"];
                    break;
                default:
                    chartInfo.Periodicity = Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityAsk"];
                    break;
            }
            

            pgrdChart.SelectedObject = chartInfo;
        }

        private void LoadConfigStudies()
        {
            ConfigurePropertyGridStudies();
            
            ConfigStudies configStudies = ListConfigStudies.Instance().LoadListConfigStudies();

            StudieInfo studieInfo = new StudieInfo
            {
                LineThickness = configStudies.LineThickness,
                Color = Color.FromArgb(configStudies.Color.ToArgb()),
                Retraction0 = bool.Parse(configStudies.Retracements[0, 1]),
                Retraction38 = bool.Parse(configStudies.Retracements[1, 1]),
                Retraction50 = bool.Parse(configStudies.Retracements[2, 1]),
                Retraction61 = bool.Parse(configStudies.Retracements[3, 1]),
                Retraction100 = bool.Parse(configStudies.Retracements[4, 1]),
                Retraction161 = bool.Parse(configStudies.Retracements[5, 1]),
                Retraction261 = bool.Parse(configStudies.Retracements[6, 1]),
                Projection0 = bool.Parse(configStudies.Projections[0, 1]),
                Projection38 = bool.Parse(configStudies.Projections[1, 1]),
                Projection50 = bool.Parse(configStudies.Projections[2, 1]),
                Projection61 = bool.Parse(configStudies.Projections[3, 1]),
                Projection100 = bool.Parse(configStudies.Projections[4, 1]),
                Projection161 = bool.Parse(configStudies.Projections[5, 1]),
                Projection261 = bool.Parse(configStudies.Projections[6, 1]),
            };

            pgrdStudies.SelectedObject = studieInfo;
        }

        private void LoadConfigPrice()
        {
            ConfigurePropertyGridPrice();

            PriceInfo priceInfo = new PriceInfo
            {
                //LineMono = Properties.Settings.Default.SettingsPriceLineMono,
                LineThickness = Properties.Settings.Default.SettingsPriceLineThickness,
                BarLineThickness = Properties.Settings.Default.SettingsPriceBarLineThickness,
                Period = Properties.Settings.Default.SettingsHeikinSmoothPeriod,
                TipoMedia = (Enums.TypeHeikin)Properties.Settings.Default.SettingsHeikinSmoothType
            };
            pgrdPrice.SelectedObject = priceInfo;
        }
        private void LoadConfigUser()
        {
            if (Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") || Program.LoginAuthentication.Offline || Program.LoginAuthentication.UserProfile.Birthday==null) return;
            ConfigurePropertyGridUser();
            
            UserInfo userInfo = new UserInfo
            {
                Birthday = Program.LoginAuthentication.UserProfile.Birthday.Value,
                CEP = Program.LoginAuthentication.UserProfile.CEP,
                City = Program.LoginAuthentication.UserProfile.City,
                Complement = Program.LoginAuthentication.UserProfile.Complement,
                CPF = Program.LoginAuthentication.UserProfile.CPF,
                District = Program.LoginAuthentication.UserProfile.District,
                Email = Program.LoginAuthentication.UserProfile.Email,
                FirstName = Program.LoginAuthentication.UserProfile.FirstName,
                LastName = Program.LoginAuthentication.UserProfile.LastName,
                Number = Program.LoginAuthentication.UserProfile.Number,
                Street = Program.LoginAuthentication.UserProfile.Street,
                UserName = Program.LoginAuthentication.UserProfile.UserName
            };
            switch (Program.LoginAuthentication.UserProfile.State)
            {
                case "Acre (AC)":
                    userInfo.State = EnumStates.Enum.AC;
                    break;
                case "Alagoas (AL)":
                    userInfo.State = EnumStates.Enum.AL;
                    break;
                case "Amapá (AP)":
                    userInfo.State = EnumStates.Enum.AP;
                    break;
                case "Amazonas (AM)":
                    userInfo.State = EnumStates.Enum.AM;
                    break;
                case "Bahia (BA)":
                    userInfo.State = EnumStates.Enum.BA;
                    break;
                case "Ceará (CE)":
                    userInfo.State = EnumStates.Enum.CE;
                    break;
                case "Distrito Federal (DF)":
                    userInfo.State = EnumStates.Enum.DF;
                    break;
                case "Espírito Santo (ES)":
                    userInfo.State = EnumStates.Enum.ES;
                    break;
                case "Goiás (GO)":
                    userInfo.State = EnumStates.Enum.GO;
                    break;
                case "Maranhão (MA)":
                    userInfo.State = EnumStates.Enum.MA;
                    break;
                case "Mato Grosso (MT)":
                    userInfo.State = EnumStates.Enum.MT;
                    break;
                case "Mato Grosso do Sul (MS)":
                    userInfo.State = EnumStates.Enum.MS;
                    break;
                case "Minas Gerais (MG)":
                    userInfo.State = EnumStates.Enum.MG;
                    break;
                case "Pará (PA) ":
                    userInfo.State = EnumStates.Enum.PA;
                    break;
                case "Paraíba (PB)":
                    userInfo.State = EnumStates.Enum.PB;
                    break;
                case "Paraná (PR)":
                    userInfo.State = EnumStates.Enum.PR;
                    break;
                case "Pernambuco (PE)":
                    userInfo.State = EnumStates.Enum.PE;
                    break;
                case "Piauí (PI)":
                    userInfo.State = EnumStates.Enum.PI;
                    break;
                case "Rio de Janeiro (RJ)":
                    userInfo.State = EnumStates.Enum.RJ;
                    break;
                case "Rio Grande do Norte (RN)":
                    userInfo.State = EnumStates.Enum.RN;
                    break;
                case "Rio Grande do Sul (RS)":
                    userInfo.State = EnumStates.Enum.RS;
                    break;
                case "Rondônia (RO)":
                    userInfo.State = EnumStates.Enum.RO;
                    break;
                case "Roraima (RR)":
                    userInfo.State = EnumStates.Enum.RR;
                    break;
                case "Santa Catarina (SC)":
                    userInfo.State = EnumStates.Enum.SC;
                    break;
                case "São Paulo (SP)":
                    userInfo.State = EnumStates.Enum.SP;
                    break;
                case "Sergipe (SE)":
                    userInfo.State = EnumStates.Enum.SE;
                    break;
                case "Tocantins (TO)":
                    userInfo.State = EnumStates.Enum.TO;
                    break;
            }

            pgrdUser.SelectedObject = userInfo;
        }

        private void NotConfigProxy()
        {
            txtUserAuthenticationProxy.Enabled = false;
            txtPasswordAuthenticationProxy.Enabled = false;
            txtPortProxy.Enabled = false;
            txtSuperscriptionProxy.Enabled = false;
            txtPortProxySocks.Enabled = false;
            txtSuperscriptionProxySocks.Enabled = false;
            cbxAuthenticationProxy.Enabled = false;
        }

        private void BtnCancelarClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnConfirmClick(object sender, EventArgs e)
        {
            pgrdChart.EndEdit();
            pgrdPrice.EndEdit();
            pgrdStudies.EndEdit();
            pgrdUser.EndEdit();
            SaveProxy();
            SaveStudies();
            SaveChart();
            SavePrice();
            SaveUser();
            //if (!_userSaving) RadMessageBox.Show(Program.LanguageDefault.DictionarySettings["msgSaved"], " ");
            //else _userSaving = false;
            _userSaving = false;
            Close();
        }

        private void SaveProxy()
        {
            Properties.Settings.Default.NotProxy = optNotProxy.IsChecked;
            Properties.Settings.Default.ServerURL = txtServerURL.Text;
            Properties.Settings.Default.Server1_Ip = txtServerURL.Text;
            Properties.Settings.Default.ProxySocks = optProxySocks.IsChecked;
            Properties.Settings.Default.ProxyServer = optProxyServer.IsChecked;
            Properties.Settings.Default.AuthenticationProxy = cbxAuthenticationProxy.IsChecked;
            Properties.Settings.Default.ConfigProxyNavigator = optConfigProxyNavigator.IsChecked;
            Properties.Settings.Default.PortProxy = txtPortProxy.Text;
            Properties.Settings.Default.SuperscriptionProxy = txtSuperscriptionProxy.Text;
            Properties.Settings.Default.PortProxySocks = txtPortProxySocks.Text;
            Properties.Settings.Default.SuperscriptionProxySocks = txtSuperscriptionProxySocks.Text;
            Properties.Settings.Default.UserAuthenticationProxy = txtUserAuthenticationProxy.Text;
            Properties.Settings.Default.PasswordAuthenticationProxy = txtPasswordAuthenticationProxy.Text;
            Properties.Settings.Default.Save();

            //Just for tests:
            DefiniServer.Instance(Program.LanguageDefault).SaveSettingsServer(Properties.Settings.Default.Server1_Ip, Properties.Settings.Default.Server2_Ip, Properties.Settings.Default.Server3_Ip, Properties.Settings.Default.Server1_Port, Properties.Settings.Default.Server2_Port, Properties.Settings.Default.Server3_Port);
        }

        private void SaveStudies()
        {
            StudieInfo studieInfo = (StudieInfo)pgrdStudies.SelectedObject;

            const double nullValue = -987654321.0;
            ConfigStudies configStudies = new ConfigStudies
            {
                Color = studieInfo.Color,
                LineThickness = studieInfo.LineThickness,
                Retracements = new[,]
                                                                   {
                                                                       { studieInfo.Retraction0.ToString(), studieInfo.Retraction0.ToString() },
                                                                       { studieInfo.Retraction38.ToString(), studieInfo.Retraction38.ToString() },
                                                                       { studieInfo.Retraction50.ToString(), studieInfo.Retraction50.ToString() },
                                                                       { studieInfo.Retraction61.ToString(), studieInfo.Retraction61.ToString() },
                                                                       { studieInfo.Retraction100.ToString(), studieInfo.Retraction100.ToString() },
                                                                       { studieInfo.Retraction161.ToString(), studieInfo.Retraction161.ToString() },
                                                                       { studieInfo.Retraction261.ToString(), studieInfo.Retraction261.ToString() }
                                                                   },
                Projections = new[,]
                                                                   {
                                                                       { studieInfo.Projection0.ToString(), studieInfo.Projection0.ToString() },
                                                                       { studieInfo.Projection38.ToString(), studieInfo.Projection38.ToString() },
                                                                       { studieInfo.Projection50.ToString(), studieInfo.Projection50.ToString() },
                                                                       { studieInfo.Projection61.ToString(), studieInfo.Projection61.ToString() },
                                                                       { studieInfo.Projection100.ToString(), studieInfo.Projection100.ToString() },
                                                                       { studieInfo.Projection161.ToString(), studieInfo.Projection161.ToString() },
                                                                       { studieInfo.Projection261.ToString(), studieInfo.Projection261.ToString() }
                                                                   },
            };


            ListConfigStudies.Instance().Update(configStudies);

            //PERCORRER TODOS OS DOCUMENTOS DO MAIN PARA ATUALIZAR GRÁFICOS ABERTOS:
            foreach (DockWindow document in _mFrMain2.documentManager.Where(document =>
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["webBrowser"])) &&
                (!document.Text.Equals(Program.LanguageDefault.DictionaryPlena["tradeDiary"])) && 
                (!document.Text.Equals(Program.LanguageDefault.DictionarySelectTools["selectTools"]))).Where(document =>
                    document.AccessibleName.Equals("CtlPainelChart")))
            {
                ((CtlPainelChart)document.Controls[0]).StockChartX1.LineColor = configStudies.Color;
                ((CtlPainelChart)document.Controls[0]).StockChartX1.LineThickness = (int)configStudies.LineThickness;
                ((CtlPainelChart)document.Controls[0]).StockChartX1.SetFibonacciRetParams(
                    bool.Parse(configStudies.Retracements[4, 1]) ? 0.0 : nullValue,
                    bool.Parse(configStudies.Retracements[3, 1]) ? 0.382 : nullValue,
                    bool.Parse(configStudies.Retracements[2, 1]) ? 0.5 : nullValue,
                    bool.Parse(configStudies.Retracements[1, 1]) ? 0.618 : nullValue,
                    bool.Parse(configStudies.Retracements[0, 1]) ? 1.0 : nullValue,
                    bool.Parse(configStudies.Retracements[5, 1]) ? 1.61 : nullValue,
                    bool.Parse(configStudies.Retracements[6, 1]) ? 2.61 : nullValue, nullValue, nullValue,
                    nullValue);
                ((CtlPainelChart)document.Controls[0]).StockChartX1.SetFibonacciProParams(
                    bool.Parse(configStudies.Projections[4, 1]) ? 0.0 : nullValue,
                    bool.Parse(configStudies.Projections[3, 1]) ? 0.382 : nullValue,
                    bool.Parse(configStudies.Projections[2, 1]) ? 0.5 : nullValue,
                    bool.Parse(configStudies.Projections[1, 1]) ? 0.618 : nullValue,
                    bool.Parse(configStudies.Projections[0, 1]) ? 1.0 : nullValue,
                    bool.Parse(configStudies.Projections[5, 1]) ? 1.61 : nullValue,
                    bool.Parse(configStudies.Projections[6, 1]) ? 2.61 : nullValue, nullValue, nullValue,
                    nullValue);
                ((CtlPainelChart)document.Controls[0]).UpdateMenus();
            }
        }

        private void SaveChart()
        {
            ChartInfo chartInfo = (ChartInfo)pgrdChart.SelectedObject;

            Properties.Settings.Default.GridVertical = chartInfo.GridVertical;
            Properties.Settings.Default.GridHorizontal = chartInfo.GridHorizontal;
            Properties.Settings.Default.PanelSeparator = chartInfo.PanelSeparator;
            Properties.Settings.Default.SemiLogScale = chartInfo.SemiLogScale;
            Properties.Settings.Default.PaddingTop = int.Parse(chartInfo.PaddingTop.ToString());
            Properties.Settings.Default.PaddingBottom = int.Parse(chartInfo.PaddingBottom.ToString());
            Properties.Settings.Default.PaddingRight = int.Parse(chartInfo.PaddingRight.ToString());
            if(! Program.LoginAuthentication.Login.ToUpper().Equals("GUEST"))Properties.Settings.Default.ChartViewport = int.Parse(chartInfo.ChartViewport.ToString());
            if(! Program.LoginAuthentication.Login.ToUpper().Equals("GUEST"))Properties.Settings.Default.History = int.Parse(chartInfo.History.ToString());
            Properties.Settings.Default.Decimals = int.Parse(chartInfo.Decimals.ToString());
            Properties.Settings.Default.VisiblePortfolio = chartInfo.VisiblePortfolio;
            frmMain2.GInstance._select.DecimalFormating();

            if (chartInfo.Position.Equals(Program.LanguageDefault.DictionarySettings["LessChart"]))
            {
                if (!Properties.Settings.Default.TabDataPosition.Equals("Less"))
                {
                    Properties.Settings.Default.TabDataPosition = "Less";
                    _mFrMain2.ReconfigureTabsChart();
                }
            }
            else if (chartInfo.Position.Equals(Program.LanguageDefault.DictionarySettings["BottomChart"]))
            {
                if (!Properties.Settings.Default.TabDataPosition.Equals("Bottom"))
                {
                    Properties.Settings.Default.TabDataPosition = "Bottom";
                    _mFrMain2.ReconfigureTabsChart();
                }
            }

            if (chartInfo.Periodicity.Equals(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityAsk"]))
            {
                if (!Properties.Settings.Default.DefaultPeriodicity.Equals("Ask"))
                {
                    Properties.Settings.Default.DefaultPeriodicity = "Ask";
                }
            }
            else if (chartInfo.Periodicity.Equals(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"]))
            {
                if (!Properties.Settings.Default.DefaultPeriodicity.Equals("Daily"))
                {
                    Properties.Settings.Default.DefaultPeriodicity = "Daily";
                }
            }
            else if (chartInfo.Periodicity.Equals(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"]))
            {
                if (!Properties.Settings.Default.DefaultPeriodicity.Equals("Weekly"))
                {
                    Properties.Settings.Default.DefaultPeriodicity = "Weekly";
                }
            }
            else if (chartInfo.Periodicity.Equals(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"]))
            {
                if (!Properties.Settings.Default.DefaultPeriodicity.Equals("Month"))
                {
                    Properties.Settings.Default.DefaultPeriodicity = "Month";
                }
            }
            else if (chartInfo.Periodicity.Equals(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityYearly"]))
            {
                if (!Properties.Settings.Default.DefaultPeriodicity.Equals("Yearly"))
                {
                    Properties.Settings.Default.DefaultPeriodicity = "Yearly";
                }
            }

            var schemes = Scheme.Instance().Schemes.Where(s => s.Value == chartInfo.ColorSchemes).FirstOrDefault();
            Properties.Settings.Default.SchemeColor = schemes.Key;

            Properties.Settings.Default.Save();

            //frmMain2.GInstance.LoadAllChartSettings();
            
            //frmMain2.GInstance.LoadColorScheme();

            //frmMain2.GInstance.UpdateChartColors();
        }

        private void SavePrice()
        {
            PriceInfo priceInfo = (PriceInfo)pgrdPrice.SelectedObject;


            Properties.Settings.Default.SettingsHeikinSmoothType = (int)priceInfo.TipoMedia;
            Properties.Settings.Default.SettingsHeikinSmoothPeriod = priceInfo.Period;
            //Properties.Settings.Default.SettingsPriceLineMono = priceInfo.LineMono;
            Properties.Settings.Default.SettingsPriceLineThickness = priceInfo.LineThickness;
            Properties.Settings.Default.SettingsPriceBarLineThickness = priceInfo.BarLineThickness;
            Properties.Settings.Default.Save();
        }
        
        private void SaveUser()
        {
            if (_userChanged)
            {
                UserInfo userInfo = (UserInfo)pgrdUser.SelectedObject;
                UserRegister userRegister = ValidateUserInfo(userInfo);


                try
                {
                    if (Server.Instance(Program.LanguageDefault).Update(userRegister))
                    {
                        RadMessageBox.Show("Usuário atualizado com sucesso.", " ");
                        Program.LoginAuthentication.UserProfile = userRegister;
                        Program.LoginAuthentication.Login = userRegister.UserName;
                        Program.LoginAuthentication.Password = userRegister.Password;
                        _userChanged = false;
                    }
                    else
                    {
                        RadMessageBox.Show("Usuário não atualizado.", " ");
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, " ");
                    _userSaving = true;
                    //Server.Instance(Program.LanguageDefault).ReloadLogin(userRegister.UserName, userRegister.Password, "");
                }
                _userSaving = true;
            }
        }

        private UserRegister ValidateUserInfo(UserInfo userInfo)
        {
            UserRegister userRegister = Program.LoginAuthentication.UserProfile;
            if (userInfo.FirstName != "" && userInfo.FirstName != userRegister.FirstName) 
                userRegister.FirstName = userInfo.FirstName;
            if (userInfo.LastName != "" && userInfo.LastName != userRegister.LastName)
                userRegister.LastName = userInfo.LastName;
            if (userInfo.CPF != "" && userInfo.CPF != userRegister.CPF)
                userRegister.CPF = userInfo.CPF.ToString();
            if (userInfo.Email != "" && userInfo.Email != userRegister.Email)
                userRegister.Email = userInfo.Email;
            if (userInfo.UserName != "" && userInfo.UserName != userRegister.UserName)
                userRegister.UserName = userInfo.UserName;
            if (userInfo.Birthday != null && userInfo.Birthday != DateTime.MinValue && userInfo.Birthday != userRegister.Birthday)
                userRegister.Birthday = userInfo.Birthday;
            if (userInfo.CEP != "" && userInfo.CEP != userRegister.CEP)
                userRegister.CEP = userInfo.CEP.ToString();
            if (!Program.LoginAuthentication.UserProfile.State.Contains("(" + userInfo.State.ToString() + ")"))
                userRegister.State = EnumStates.States[(int)userInfo.State];
            if (userInfo.City != "" && userInfo.City != userRegister.City)
                userRegister.City = userInfo.City;
            if (userInfo.District != "" && userInfo.District != userRegister.District)
                userRegister.District = userInfo.District;
            if (userInfo.Street != "" && userInfo.Street != userRegister.Street)
                userRegister.Street = userInfo.Street;
            if (userInfo.Number != "" && userInfo.Number != userRegister.Number)
                userRegister.Number = userInfo.Number;
            if (userInfo.Complement != "" && userInfo.Complement != userRegister.Complement)
                userRegister.Complement = userInfo.Complement;
            if (userInfo.OldPassword != "" && userInfo.NewPassword != "" && userInfo.NewPassword2 != ""
                && userInfo.OldPassword != null && userInfo.NewPassword != null && userInfo.NewPassword2 != null)
            {
                userRegister.Password = Utility.Cript(userInfo.NewPassword);
                userRegister.ConfirmPassword = Utility.Cript(userInfo.NewPassword2);
            }
            return userRegister;
        }

        private void grdFibonacci_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name.Equals("Retracements") && e.Row.Cells[1].ReadOnly)
                e.CellElement.Enabled = false;
        }

        private void OptProxyServerToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            NotConfigProxy();

            lblSuperscriptionProxy.Enabled = true;
            txtSuperscriptionProxy.Enabled = true;
            lblPortProxy.Enabled = true;
            txtPortProxy.Enabled = true;
            cbxAuthenticationProxy.Enabled = true;
            lblUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            lblPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
        }

        private void OptConfigProxyNavigatorToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            NotConfigProxy();
        }

        private void OptNotProxyToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            NotConfigProxy();
        }

        private void OptProxySocksToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            NotConfigProxy();

            lblSuperscriptionProxySocks.Enabled = true;
            txtSuperscriptionProxySocks.Enabled = true;
            lblPortProxySocks.Enabled = true;
            txtPortProxySocks.Enabled = true;
            cbxAuthenticationProxy.Enabled = true;
            lblUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            lblPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
        }

        private void CbxAuthenticationProxyToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            lblUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtUserAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            lblPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
            txtPasswordAuthenticationProxy.Enabled = cbxAuthenticationProxy.IsChecked;
        }

        private void FrmSettingsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;

            Close();
        }

        private void LoadTabs()
        {
            LoadChart();
            LoadPrice();
            LoadUser();
            NewLoadStudies();

            LoadConfigChart();
            LoadConfigPrice();
            LoadConfigServer();
            LoadConfigStudies();
            LoadConfigUser();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            //_thread = new Thread(LoadTabs);
            //_thread.Start();
            LoadTabs();
        }

        #region PropertyGrid Price Events

        private void ConfigurePropertyGridPrice()
        {
            pgrdPrice.EnableSorting = false;
            pgrdPrice.HelpVisible = true;
            pgrdPrice.SortOrder = SortOrder.None;
            pgrdPrice.PropertyValidating += PgrdPricePropertyValidating;
            pgrdPrice.Edited += PgrdPriceEdited;
            pgrdPrice.PropertySort = PropertySort.Categorized;
            pgrdPrice.EditorInitialized += PgrdPriceEditorInitialized;
            pgrdPrice.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdPrice.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;
            pgrdPrice.HelpBarHeight = 35;
        }

        private static void PgrdPriceEditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            PropertyGridSpinEditor editorGridSpinEditor = e.Editor as PropertyGridSpinEditor;

            if (editorGridSpinEditor == null)
                return;

            BaseSpinEditorElement element = editorGridSpinEditor.EditorElement as BaseSpinEditorElement;

            switch (e.Item.Name)
            {
                case "LineThickness":
                case "BarLineThickness":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 10;
                    }
                    break;
                case "Period":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
            }
        }

        private static void PgrdPriceEdited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;

            if (item != null)
                item.ErrorMessage = "";
        }

        private static void PgrdPricePropertyValidating(object sender, PropertyValidatingEventArgs e)
        {
            PropertyGridItem item = (PropertyGridItem)e.Item;

            if ((e.Item.Name == "BarraLineThickness") && (int.Parse(e.NewValue.ToString()) < 1))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgCycleMinimum"];
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Pela Thread os valores não são preenchidos e não é possível traduzir as palavras.
        /// Tradução feita ao pintar a aba ao abrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabPricePaint(object sender, PaintEventArgs e)
        {
            if ((pgrdPrice == null) || (pgrdPrice.Groups.Count <= 0))
                return;
            
            pgrdPrice.Groups[0].Label = Program.LanguageDefault.DictionarySettings["grpLine"];
            pgrdPrice.Groups[1].Label = Program.LanguageDefault.DictionarySettings["grpBar"];
            pgrdPrice.Groups[2].Label = Program.LanguageDefault.DictionarySettings["grpSmoothed"];

            //pgrdPrice.Items[0].Label = Program.LanguageDefault.DictionarySettings["lblMonoColor"];
            pgrdPrice.Items[0].Label = Program.LanguageDefault.DictionarySettings["lblLineThicknessLine"];
            pgrdPrice.Items[1].Label = Program.LanguageDefault.DictionarySettings["lblLineThicknessBar"];
            pgrdPrice.Items[2].Label = Program.LanguageDefault.DictionarySettings["lblPeriodsSmoothed"];
            pgrdPrice.Items[3].Label = Program.LanguageDefault.DictionarySettings["lblTypeAverageSmoothed"];
        }

        #endregion

        #region PropertyGrid Chart Events

        private void ConfigurePropertyGridChart()
        {
            pgrdChart.EnableSorting = false;
            pgrdChart.HelpVisible = true;
            pgrdChart.SortOrder = SortOrder.None;
            pgrdChart.Edited += PgrdChartEdited;
            pgrdChart.PropertySort = PropertySort.Categorized;
            pgrdChart.EditorInitialized += PgrdChartEditorInitialized;
            pgrdChart.EditorRequired += pgrdChart_EditorRequired;
            pgrdChart.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdChart.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;

            pgrdChart.HelpBarHeight = 35;
        }

        private void pgrdChart_EditorRequired(object sender, PropertyGridEditorRequiredEventArgs e)
        {
            if ((e.Item.Name == "ColorSchemes") || (e.Item.Name == "Position") || (e.Item.Name == "Periodicity"))
            {
                e.EditorType = typeof(PropertyGridDropDownListEditor);
            }
        }

        private void PgrdChartEditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "ColorSchemes":
                    {
                        PropertyGridDropDownListEditor editor = e.Editor as PropertyGridDropDownListEditor;
                        BaseDropDownListEditorElement editorElement = editor.EditorElement as BaseDropDownListEditorElement;

                        if (editorElement != null )
                        {
                            editorElement.ValueMember = "Text";
                            editorElement.DisplayMember = "Text";
                            editorElement.DataSource = Scheme.Instance().Schemes.Select(s => new ItemChart(s.Key, s.Value));

                            if (((PropertyGridItem)e.Item).Value!=null) editorElement.SelectedValue = ((PropertyGridItem)e.Item).Value.ToString();
                            editorElement.RadPropertyChanged += editorElement_RadPropertyChanged;
                        }

                        return;
                    }
                case "Position":
                    {
                        PropertyGridDropDownListEditor editor = e.Editor as PropertyGridDropDownListEditor;
                        BaseDropDownListEditorElement editorElement = editor.EditorElement as BaseDropDownListEditorElement;

                        if (editorElement != null)
                        {
                            IList<ItemChart> items = new List<ItemChart>
                                                         {
                                                             new ItemChart(Program.LanguageDefault.DictionarySettings["LessChart"], Program.LanguageDefault.DictionarySettings["LessChart"]),
                                                             new ItemChart(Program.LanguageDefault.DictionarySettings["BottomChart"], Program.LanguageDefault.DictionarySettings["BottomChart"])
                                                         };

                            editorElement.ValueMember = "Text";
                            editorElement.DisplayMember = "Text";
                            editorElement.DataSource = items;

                            editorElement.SelectedValue = ((PropertyGridItem)e.Item).Value.ToString();
                            editorElement.RadPropertyChanged += editorElement_RadPropertyChanged;
                        }
                        return;
                    }
                case "Periodicity":
                    {
                        PropertyGridDropDownListEditor editor = e.Editor as PropertyGridDropDownListEditor;
                        BaseDropDownListEditorElement editorElement = editor.EditorElement as BaseDropDownListEditorElement;

                        if (editorElement != null)
                        {
                            IList<ItemChart> items = new List<ItemChart>
                                                         {
                                                             new ItemChart(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityAsk"], Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityAsk"]),
                                                             new ItemChart(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"], Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityDaily"]),
                                                             new ItemChart(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"], Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityWeekly"]),
                                                             new ItemChart(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"], Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityMonth"]),
                                                             new ItemChart(Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityYearly"], Program.LanguageDefault.DictionaryMenuBar["cmdPeriodicityYearly"])
                                                         };

                            editorElement.ValueMember = "Text";
                            editorElement.DisplayMember = "Text";
                            editorElement.DataSource = items;

                            editorElement.SelectedValue = ((PropertyGridItem)e.Item).Value.ToString();
                            editorElement.RadPropertyChanged += editorElement_RadPropertyChanged;
                        }
                    }
                    return;

            }

            PropertyGridSpinEditor editorGridSpinEditor = e.Editor as PropertyGridSpinEditor;

            if (editorGridSpinEditor == null)
                return;

            BaseSpinEditorElement element = editorGridSpinEditor.EditorElement as BaseSpinEditorElement;

            switch (e.Item.Name)
            {
                case "Decimals":
                    if (element != null)
                    {
                        element.MinValue = 0;
                        element.MaxValue = 3;
                    }
                    break;
                case "PaddingTop":
                case "PaddingBottom":
                    if (element != null)
                    {
                        element.MinValue = 0;
                        element.MaxValue = 30;
                    }
                    break;
                case "PaddingRight":
                    if (element != null)
                    {
                        element.MinValue = 0;
                        element.MaxValue = 300;
                    }
                    break;
                case "ChartViewport":
                case "History":
                    if (element != null)
                    {
                        element.MinValue = 2;
                        element.MaxValue = 10000;
                    }
                    break;
            }
        }


        private void editorElement_RadPropertyChanged(object sender, RadPropertyChangedEventArgs e)
        {
            if (e.Property == RadElement.ContainsFocusProperty && !(bool)e.NewValue)
            {
                 pgrdChart.EndEdit();
            }
        }

        private static void PgrdChartEdited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;
            if (item != null)
                item.ErrorMessage = "";
            //Guests cant change History Bars or Viewport:
            if (Program.LoginAuthentication.Login.ToUpper().Equals("GUEST") && (item.Name == "History" || item.Name == "ChartViewport") && int.Parse(item.Value.ToString()) > 300)
            {
                RadMessageBox.Show(Program.LanguageDefault.DictionaryLogin["msgGuestError"], " ");
                item.Value = item.OriginalValue;
            }
        }

        /// <summary>
        /// Pela Thread os valores não são preenchidos e não é possível traduzir as palavras.
        /// Tradução feita ao pintar a aba ao abrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabChartPaint(object sender, PaintEventArgs e)
        {
            if ((pgrdChart == null) || (pgrdChart.Groups.Count <= 0))
                return;
            
            pgrdChart.Groups[0].Label = Program.LanguageDefault.DictionarySettings["grpBehavior"];
            pgrdChart.Groups[1].Label = Program.LanguageDefault.DictionarySettings["grpAppearance"];
            pgrdChart.Groups[2].Label = Program.LanguageDefault.DictionarySettings["grpNumberCandles"];
            pgrdChart.Groups[3].Label = Program.LanguageDefault.DictionarySettings["grpTabData"];

            pgrdChart.Items[0].Label = Program.LanguageDefault.DictionarySettings["cbxSemiLogScale"];
            pgrdChart.Items[1].Label = Program.LanguageDefault.DictionarySettings["cbxPanelSeparator"];
            pgrdChart.Items[2].Label = Program.LanguageDefault.DictionarySettings["cbxGridVertical"];
            pgrdChart.Items[3].Label = Program.LanguageDefault.DictionarySettings["cbxGridHorizontal"];
            pgrdChart.Items[4].Label = Program.LanguageDefault.DictionarySettings["lblDecimals"];
            pgrdChart.Items[5].Label = Program.LanguageDefault.DictionarySettings["cbxVisiblePortfolio"];
            pgrdChart.Items[6].Label = Program.LanguageDefault.DictionarySettings["lblPaddingTop"];
            pgrdChart.Items[7].Label = Program.LanguageDefault.DictionarySettings["lblPaddingBottom"];
            pgrdChart.Items[8].Label = Program.LanguageDefault.DictionarySettings["lblPaddingRight"];
            pgrdChart.Items[9].Label = Program.LanguageDefault.DictionarySettings["lblColorScheme"];
            pgrdChart.Items[10].Label = Program.LanguageDefault.DictionarySettings["lblChartViewport"];
            pgrdChart.Items[11].Label = Program.LanguageDefault.DictionarySettings["lblChartHistory"];
            pgrdChart.Items[12].Label = Program.LanguageDefault.DictionarySelectChart["lblDescriptionPeriodicity"];
            pgrdChart.Items[13].Label = Program.LanguageDefault.DictionarySettings["lblPositionChart"];
        }

        #endregion

        #region PropertyGrid Chart Studies

        private void ConfigurePropertyGridStudies()
        {
            pgrdStudies.EnableSorting = false;
            pgrdStudies.HelpVisible = true;
            pgrdStudies.SortOrder = SortOrder.None;
            pgrdStudies.Edited += PgrdStudiesEdited;
            pgrdStudies.PropertySort = PropertySort.Categorized;
            pgrdStudies.EditorInitialized += PgrdStudiesEditorInitialized;
            pgrdStudies.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdStudies.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;

            pgrdStudies.HelpBarHeight = 35;
        }

        private static void PgrdStudiesEditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "Color":
                    {
                        PropertyGridColorEditor gridColorEditor = e.Editor as PropertyGridColorEditor;

                        if (gridColorEditor != null)
                        {
                            RadColorBoxElement colorBoxElement = ((PropertyGridColorEditor)e.Editor).EditorElement as RadColorBoxElement;

                            if (colorBoxElement != null)
                            {
                                //colorBoxElement.ColorDialog.ColorDialogForm.ShowWebColors = false;
                                //colorBoxElement.ColorDialog.ColorDialogForm.ShowSystemColors = false;
                                colorBoxElement.ColorDialog.ColorDialogForm.ActiveMode = ColorPickerActiveMode.Basic;
                            }
                        }

                        return;
                    }
            }

            PropertyGridSpinEditor editorGridSpinEditor = e.Editor as PropertyGridSpinEditor;

            if (editorGridSpinEditor == null)
                return;

            BaseSpinEditorElement element = editorGridSpinEditor.EditorElement as BaseSpinEditorElement;

            switch (e.Item.Name)
            {
                case "LineThickness":
                    if (element != null)
                    {
                        element.MinValue = 0;
                        element.MaxValue = 10;
                    }
                    break;
            }
        }

        private static void PgrdStudiesEdited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;

            if (item != null)
                item.ErrorMessage = "";
        }

        /// <summary>
        /// Pela Thread os valores não são preenchidos e não é possível traduzir as palavras.
        /// Tradução feita ao pintar a aba ao abrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabStudiesPaint(object sender, PaintEventArgs e)
        {
            if ((pgrdStudies == null) || (pgrdStudies.Groups.Count <= 0))
                return;
            
            pgrdStudies.Groups[0].Label = Program.LanguageDefault.DictionarySettings["grpAppearance"];
            pgrdStudies.Groups[1].Label = Program.LanguageDefault.DictionarySelectIndicator["Fibonacci Retracements"];
            pgrdStudies.Groups[2].Label = Program.LanguageDefault.DictionarySelectIndicator["Fibonacci Projections"];

            pgrdStudies.Items[0].Label = Program.LanguageDefault.DictionarySettings["lblLineThickness"];
            pgrdStudies.Items[1].Label = Program.LanguageDefault.DictionarySettings["lblColor"];
            pgrdStudies.Items[2].Label = "0%";
            pgrdStudies.Items[3].Label = "38,2%";
            pgrdStudies.Items[4].Label = "50%";
            pgrdStudies.Items[5].Label = "61,8%";
            pgrdStudies.Items[6].Label = "100%";
            pgrdStudies.Items[7].Label = "161,8%";
            pgrdStudies.Items[8].Label = "261,8%";
            pgrdStudies.Items[9].Label = "0%";
            pgrdStudies.Items[10].Label = "38,2%";
            pgrdStudies.Items[11].Label = "50%";
            pgrdStudies.Items[12].Label = "61,8%";
            pgrdStudies.Items[13].Label = "100%";
            pgrdStudies.Items[14].Label = "161,8%";
            pgrdStudies.Items[15].Label = "261,8%";
        }

        #endregion

        #region PropertyGrid User

        private void ConfigurePropertyGridUser()
        {
            pgrdUser.EnableSorting = false;
            pgrdUser.HelpVisible = true;
            pgrdUser.SortOrder = SortOrder.None;
            pgrdUser.Edited += PgrdUserEdited;
            pgrdUser.PropertySort = PropertySort.Categorized;
            pgrdUser.EditorInitialized += PgrdUserEditorInitialized;
            pgrdUser.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdUser.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;

            pgrdUser.HelpBarHeight = 35;
        }

        private static void PgrdUserEditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            /*switch (e.Item.Name)
            {
                case "Color":
                    {
                        PropertyGridColorEditor gridColorEditor = e.Editor as PropertyGridColorEditor;

                        if (gridColorEditor != null)
                        {
                            RadColorBoxElement colorBoxElement = ((PropertyGridColorEditor)e.Editor).EditorElement as RadColorBoxElement;

                            if (colorBoxElement != null)
                            {
                                colorBoxElement.ColorDialog.ColorDialogForm.ShowWebColors = false;
                                colorBoxElement.ColorDialog.ColorDialogForm.ShowSystemColors = false;
                                colorBoxElement.ColorDialog.ColorDialogForm.ActiveMode = ColorPickerActiveMode.Basic;
                            }
                        }

                        return;
                    }
            }*/

            PropertyGridSpinEditor editorGridSpinEditor = e.Editor as PropertyGridSpinEditor;

            if (editorGridSpinEditor == null)
                return;

            BaseSpinEditorElement element = editorGridSpinEditor.EditorElement as BaseSpinEditorElement;

            switch (e.Item.Name)
            {
                case "CPF":
                    if (element != null)
                    {
                        element.MinValue = 10000000000;
                        element.MaxValue = 99999999999;
                    }
                    break;
                case "CEP":
                    if (element != null)
                    {
                        element.MinValue = 10000000;
                        element.MaxValue = 999999999;
                    }
                    break;
            }
        }

        private void PgrdUserEdited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;

            if (item != null)
            {
                switch (item.Name)
                {
                    case "FirstName":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.FirstName)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "LastName":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.LastName)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "CPF":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.CPF)
                        {
                            if (CheckCPF(item.Value.ToString()))
                            {
                                _userChanged = true;
                            }
                            else
                            {
                                RadMessageBox.Show("CPF inválido.", " ");
                                item.ResetValue();
                            }
                        }
                        else item.ResetValue();
                        break;
                    case "Email":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.Email)
                        {
                            if (CheckEmail(item.Value.ToString()))
                            {
                                _userChanged = true;
                            }
                            else
                            {
                                RadMessageBox.Show("Email inválido.", " ");
                                item.ResetValue();
                            }
                        }
                        else item.ResetValue();
                        break;
                    case "UserName":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.Email)
                        {
                            if (item.Value.ToString().Length>3)
                            {
                                _userChanged = true;
                            }
                            else
                            {
                                RadMessageBox.Show("Login inválido.", " "); 
                                item.ResetValue();
                            }
                        }
                        else item.ResetValue();
                        break;
                    case "OldPassword":
                    case "NewPassword":
                    case "NewPassword2":
                        if (pgrdUser.Items[5].Value != null && pgrdUser.Items[6].Value != null && pgrdUser.Items[7].Value != null &&
                            pgrdUser.Items[5].Value.ToString() != "" && pgrdUser.Items[6].Value.ToString() != "" && pgrdUser.Items[7].Value.ToString() != "")
                        {
                            if (pgrdUser.Items[5].Value.ToString() == Utility.Decript(Program.LoginAuthentication.Password))
                            {
                                if (pgrdUser.Items[6].Value.ToString() == pgrdUser.Items[7].Value.ToString())
                                {
                                    _userChanged = true;
                                }
                                else
                                {
                                    RadMessageBox.Show("Senha nova não confirmada.", " ");
                                    pgrdUser.Items[6].ResetValue();
                                    pgrdUser.Items[7].ResetValue();
                                }
                            }
                            else
                            {
                                RadMessageBox.Show("Senha antiga incorreta.", " ");
                                pgrdUser.Items[5].ResetValue();
                            }
                        }
                        break;
                    case "Birthday":
                        if ((DateTime)item.Value != Program.LoginAuthentication.UserProfile.Birthday && (DateTime)item.Value != DateTime.MinValue && (DateTime)item.Value != new DateTime(1900,1,1))
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "CEP":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.CEP)
                        {
                            if (CheckCEP(item.Value.ToString()))
                            {
                                _userChanged = true;
                            }
                            else
                            {
                                RadMessageBox.Show("CEP inválido.", " ");
                                item.ResetValue();
                            }
                        }
                        else item.ResetValue();
                        break;
                    case "State":
                        if (!Program.LoginAuthentication.UserProfile.State.Contains("("+item.Value.ToString()+")") && item.Value.ToString()!="_")
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "City":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.City)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "District":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.District)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "Street":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.Street)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "Number":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.Number)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;
                    case "Complement":
                        if (item.Value.ToString() != Program.LoginAuthentication.UserProfile.Complement)
                        {
                            _userChanged = true;
                        }
                        else item.ResetValue();
                        break;

                }
            }
        }

        
        public bool CheckCPF(string cpf)
        {
            cpf = new String(cpf.Where(Char.IsDigit).ToArray());
            if (cpf == "" || cpf.Length != 11) return false;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        public bool CheckCEP(string cep)
        {
            cep = new String(cep.Where(Char.IsDigit).ToArray());
            if (cep.Length > 7 && cep.Length < 10) return true;
            else return false;

        }
        public bool CheckEmail(string email)
        {
            email = email.Replace(" ", "");
            string[] arroba = email.Split(new char[] { '@' });
            if (arroba.Length != 2) return false;
            else
            {
                string[] dot = arroba[1].Split(new char[] { '.' });
                if (dot.Length < 2 || dot.Length > 3) return false;
                else if (dot[dot.Length - 1] == "") return false;
            }
            return true;
        }

        /// <summary>
        /// Pela Thread os valores não são preenchidos e não é possível traduzir as palavras.
        /// Tradução feita ao pintar a aba ao abrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabUserPaint(object sender, PaintEventArgs e)
        {
            if ((pgrdUser == null) || (pgrdUser.Groups.Count <= 0))
                return;
            _userChanged = false;
            pgrdUser.Groups[0].Label = Program.LanguageDefault.DictionarySettings["grpBasic"];
            pgrdUser.Groups[1].Label = Program.LanguageDefault.DictionarySettings["grpOptional"];

            pgrdUser.Items[0].Label = Program.LanguageDefault.DictionarySettings["lblFirstName"];
            pgrdUser.Items[1].Label = Program.LanguageDefault.DictionarySettings["lblLastName"];
            pgrdUser.Items[2].Label = Program.LanguageDefault.DictionarySettings["lblCPF"];
            pgrdUser.Items[3].Label = Program.LanguageDefault.DictionarySettings["lblEmail"];
            pgrdUser.Items[4].Label = Program.LanguageDefault.DictionarySettings["lblUserName"];
            pgrdUser.Items[5].Label = Program.LanguageDefault.DictionarySettings["lblOldPassword"];
            pgrdUser.Items[6].Label = Program.LanguageDefault.DictionarySettings["lblNewPassword"];
            pgrdUser.Items[7].Label = Program.LanguageDefault.DictionarySettings["lblNewPassword2"];
            pgrdUser.Items[8].Label = Program.LanguageDefault.DictionarySettings["lblBirthday"];
            pgrdUser.Items[9].Label = Program.LanguageDefault.DictionarySettings["lblCEP"];
            pgrdUser.Items[10].Label = Program.LanguageDefault.DictionarySettings["lblState"];
            pgrdUser.Items[11].Label = Program.LanguageDefault.DictionarySettings["lblCity"];
            pgrdUser.Items[12].Label = Program.LanguageDefault.DictionarySettings["lblDistrict"];
            pgrdUser.Items[13].Label = Program.LanguageDefault.DictionarySettings["lblStreet"];
            pgrdUser.Items[14].Label = Program.LanguageDefault.DictionarySettings["lblNumber"];
            pgrdUser.Items[15].Label = Program.LanguageDefault.DictionarySettings["lblComplement"];
        }

        #endregion
    }
}