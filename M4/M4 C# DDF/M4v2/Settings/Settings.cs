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
using Nevron.UI.WinForm.Controls;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace M4.M4v2.Settings
{
    public partial class FrmSettings : RadForm
    {
        private readonly frmMain _mFrMain;
        private readonly frmMain2 _mFrMain2;
        private Thread _thread;

        public FrmSettings(frmMain2 mainForm)
        {
            _mFrMain2 = mainForm;

            InitializeComponent();
            LoadProxy();

            LoadConfigProxy();

            TranslateForm();

            tabChart.BackColor = Utils.GetDefaultBackColor();
            tabProxy.BackColor = Utils.GetDefaultBackColor();
            tabServer.BackColor = Utils.GetDefaultBackColor();
            tabStudies.BackColor = Utils.GetDefaultBackColor();
            tabPrice.BackColor = Utils.GetDefaultBackColor();
        }

        public FrmSettings(frmMain mainForm)
        {
            _mFrMain = mainForm;

            InitializeComponent();
            LoadProxy();

            LoadConfigProxy();

            TranslateForm();

            tabChart.BackColor = Utils.GetDefaultBackColor();
            tabProxy.BackColor = Utils.GetDefaultBackColor();
            tabServer.BackColor = Utils.GetDefaultBackColor();
            tabStudies.BackColor = Utils.GetDefaultBackColor();
            tabPrice.BackColor = Utils.GetDefaultBackColor();
        }
        private void TranslateForm()
        {
            Text = Program.LanguageDefault.DictionarySettings["frmOptions"];

            btnOk.Text = Program.LanguageDefault.DictionarySettings["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionarySettings["btnCancel"];

            tabServer.Text = Program.LanguageDefault.DictionarySettings["tabServer"];
            tabProxy.Text = Program.LanguageDefault.DictionarySettings["tabProxy"];
            tabChart.Text = Program.LanguageDefault.DictionarySettings["tabChart"];
            tabStudies.Text = Program.LanguageDefault.DictionarySettings["tabStudies"];
            tabPrice.Text = Program.LanguageDefault.DictionarySettings["tabPrice"];
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
            txtPortProxySocks.Text = Properties.Settings.Default.PortProxySocks;
            txtSuperscriptionProxy.Text = Properties.Settings.Default.SuperscriptionProxy;
            txtSuperscriptionProxySocks.Text = Properties.Settings.Default.SuperscriptionProxySocks;
            txtUserAuthenticationProxy.Text = Properties.Settings.Default.UserAuthenticationProxy;
            txtPasswordAuthenticationProxy.Text = Properties.Settings.Default.PasswordAuthenticationProxy;

            optNotProxy.Text = Program.LanguageDefault.DictionarySettings["optNotProxy"];
            optProxySocks.Text = Program.LanguageDefault.DictionarySettings["optProxySocks"];
            optProxyServer.Text = Program.LanguageDefault.DictionarySettings["optProxyServer"];
            grpSettingsProxy.Text = Program.LanguageDefault.DictionarySettings["grpSettingsProxy"];
            cbxAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["cbxAuthenticationProxy"];
            optConfigProxyNavigator.Text = Program.LanguageDefault.DictionarySettings["optConfigProxyNavigator"];
            lblSuperscriptionProxy.Text = Program.LanguageDefault.DictionarySettings["Address"];
            lblSuperscriptionProxySocks.Text = Program.LanguageDefault.DictionarySettings["Address"];
            lblPortProxy.Text = Program.LanguageDefault.DictionarySettings["Port"];
            lblPortProxySocks.Text = Program.LanguageDefault.DictionarySettings["Port"];
            lblUserAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["lblUser"];
            lblPasswordAuthenticationProxy.Text = Program.LanguageDefault.DictionarySettings["lblPassword"];
            grpSettingsProxy.Text = Program.LanguageDefault.DictionarySettings["grpSettingsProxy"];
            btnApplyProxy.Text = Program.LanguageDefault.DictionarySettings["btnApply"];

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
            txtServer1.Text = Properties.Settings.Default.Server1_Ip;
            txtServer2.Text = Properties.Settings.Default.Server2_Ip;
            txtServer3.Text = Properties.Settings.Default.Server3_Ip;
            txtServerPort1.Text = Properties.Settings.Default.Server1_Port;
            txtServerPort2.Text = Properties.Settings.Default.Server2_Port;
            txtServerPort3.Text = Properties.Settings.Default.Server3_Port;

            grpSettingsServer.Text = Program.LanguageDefault.DictionarySettings["grpSettingsServer"];
            lblServer1.Text = Program.LanguageDefault.DictionarySettings["lblServer1"];
            lblServer2.Text = Program.LanguageDefault.DictionarySettings["lblServer2"];
            lblServer3.Text = Program.LanguageDefault.DictionarySettings["lblServer3"];
            lblPort1.Text = Program.LanguageDefault.DictionarySettings["lblPort1"];
            lblPort2.Text = Program.LanguageDefault.DictionarySettings["lblPort2"];
            lblPort3.Text = Program.LanguageDefault.DictionarySettings["lblPort3"];
            btnApplyServer.Text = Program.LanguageDefault.DictionarySettings["btnApply"];
        }

        private void LoadConfigChart()
        {
            ConfigurePropertyGridChart();

            btnApplyChart.Text = Program.LanguageDefault.DictionarySettings["btnApply"];

            ChartInfo chartInfo = new ChartInfo
            {
                GridHorizontal = Properties.Settings.Default.GridHorizontal,
                GridVertical = Properties.Settings.Default.GridVertical,
                SemiLogScale = Properties.Settings.Default.SemiLogScale,
                PanelSeparator = Properties.Settings.Default.PanelSeparator,
                Decimals = int.Parse(Properties.Settings.Default.Decimals.ToString()),
                History = int.Parse(Properties.Settings.Default.History.ToString()),
                ChartViewport = int.Parse(Properties.Settings.Default.ChartViewport.ToString()),
                PaddingTop = int.Parse(Properties.Settings.Default.PaddingTop.ToString()),
                PaddingBottom = int.Parse(Properties.Settings.Default.PaddingBottom.ToString()),
                PaddingRight = int.Parse(Properties.Settings.Default.PaddingRight.ToString()),
                VisiblePortfolio = Properties.Settings.Default.VisiblePortfolio,
            };

            switch (Properties.Settings.Default.SchemeColor)
            {
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

            pgrdChart.SelectedObject = chartInfo;
        }

        private void LoadConfigStudies()
        {
            ConfigurePropertyGridStudies();

            btnApplyStudies.Text = Program.LanguageDefault.DictionarySettings["btnApply"];

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

            btnApplyPrice.Text = Program.LanguageDefault.DictionarySettings["btnApply"];

            PriceInfo priceInfo = new PriceInfo
            {
                LineMono = Properties.Settings.Default.SettingsPriceLineMono,
                LineThickness = Properties.Settings.Default.SettingsPriceLineThickness,
                BarLineThickness = Properties.Settings.Default.SettingsPriceBarLineThickness,
                Period = Properties.Settings.Default.SettingsHeikinSmoothPeriod,
                TipoMedia = (Enums.Type)Properties.Settings.Default.SettingsHeikinSmoothType
            };

            pgrdPrice.SelectedObject = priceInfo;
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
            SaveProxy();
            SaveStudies();
            SaveServer();
            SaveChart();
            SavePrice();
            Close();
        }

        private void SaveServer()
        {
            Properties.Settings.Default.Server1_Ip = txtServer1.Text;
            Properties.Settings.Default.Server2_Ip = txtServer2.Text;
            Properties.Settings.Default.Server3_Ip = txtServer3.Text;
            Properties.Settings.Default.Server1_Port = txtServerPort1.Text;
            Properties.Settings.Default.Server2_Port = txtServerPort2.Text;
            Properties.Settings.Default.Server3_Port = txtServerPort3.Text;
            Properties.Settings.Default.Save();
        }

        private void SaveProxy()
        {
            Properties.Settings.Default.NotProxy = optNotProxy.IsChecked;
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
                                                                       { studieInfo.Retraction100.ToString(), studieInfo.Retraction100.ToString() }
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
                    bool.Parse(configStudies.Retracements[0, 1]) ? 1.0 : nullValue, nullValue, nullValue,
                    nullValue, nullValue, nullValue);
                ((CtlPainelChart)document.Controls[0]).StockChartX1.SetFibonacciProParams(
                    bool.Parse(configStudies.Projections[4, 1]) ? 0.0 : nullValue,
                    bool.Parse(configStudies.Projections[3, 1]) ? 0.382 : nullValue,
                    bool.Parse(configStudies.Projections[2, 1]) ? 0.5 : nullValue,
                    bool.Parse(configStudies.Projections[1, 1]) ? 0.618 : nullValue,
                    bool.Parse(configStudies.Projections[0, 1]) ? 1.0 : nullValue,
                    bool.Parse(configStudies.Projections[5, 1]) ? 1.0 : nullValue,
                    bool.Parse(configStudies.Projections[6, 1]) ? 1.0 : nullValue, nullValue, nullValue,
                    nullValue);
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
            Properties.Settings.Default.ChartViewport = int.Parse(chartInfo.ChartViewport.ToString());
            Properties.Settings.Default.History = int.Parse(chartInfo.History.ToString());
            Properties.Settings.Default.Decimals = int.Parse(chartInfo.Decimals.ToString());
            Properties.Settings.Default.VisiblePortfolio = chartInfo.VisiblePortfolio;

            if (chartInfo.Position.Equals(Program.LanguageDefault.DictionarySettings["LessChart"]))
            {
                if (!Properties.Settings.Default.TabDataPosition.Equals("Less"))
                {
                    Properties.Settings.Default.TabDataPosition = "Less";
                    frmMain.GInstance.ReconfigureTabsChart();
                }
            }
            else if (chartInfo.Position.Equals(Program.LanguageDefault.DictionarySettings["BottomChart"]))
            {
                if (!Properties.Settings.Default.TabDataPosition.Equals("Bottom"))
                {
                    Properties.Settings.Default.TabDataPosition = "Bottom";
                    frmMain.GInstance.ReconfigureTabsChart();
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


            Properties.Settings.Default.SettingsHeikinSmoothType = priceInfo.TipoMedia.GetHashCode();
            Properties.Settings.Default.SettingsHeikinSmoothPeriod = priceInfo.Period;
            Properties.Settings.Default.SettingsPriceLineMono = priceInfo.LineMono;
            Properties.Settings.Default.SettingsPriceLineThickness = priceInfo.LineThickness;
            Properties.Settings.Default.SettingsPriceBarLineThickness = priceInfo.BarLineThickness;
            Properties.Settings.Default.Save();
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

        private void BtnApplyChartClick(object sender, EventArgs e)
        {
            SaveChart();
        }

        private void BtnApplyStudiesClick(object sender, EventArgs e)
        {
            SaveStudies();
        }

        private void BtnApplyProxyClick(object sender, EventArgs e)
        {
            SaveProxy();
        }

        private void BtnApplyServerClick(object sender, EventArgs e)
        {
            SaveServer();
        }

        private void BtnApplyPriceClick(object sender, EventArgs e)
        {
            btnApplyPrice.Focus();

            SavePrice();
        }

        private void LoadTabs()
        {
            LoadChart();
            LoadPrice();
            LoadServer();
            NewLoadStudies();

            LoadConfigChart();
            LoadConfigPrice();
            LoadConfigServer();
            LoadConfigStudies();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            _thread = new Thread(LoadTabs);
            _thread.Start();
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
            btnApplyPrice.Text = Program.LanguageDefault.DictionarySettings["btnApply"];

            if ((pgrdPrice == null) || (pgrdPrice.Groups.Count <= 0))
                return;

            grpPrice.Text = Program.LanguageDefault.DictionarySettings["grpPrice"];



            pgrdPrice.Groups[0].Label = Program.LanguageDefault.DictionarySettings["grpLine"];
            pgrdPrice.Groups[1].Label = Program.LanguageDefault.DictionarySettings["grpBar"];
            pgrdPrice.Groups[2].Label = Program.LanguageDefault.DictionarySettings["grpSmoothed"];

            pgrdPrice.Items[0].Label = Program.LanguageDefault.DictionarySettings["lblMonoColor"];
            pgrdPrice.Items[1].Label = Program.LanguageDefault.DictionarySettings["lblLineThicknessLine"];
            pgrdPrice.Items[2].Label = Program.LanguageDefault.DictionarySettings["lblLineThicknessBar"];
            pgrdPrice.Items[3].Label = Program.LanguageDefault.DictionarySettings["lblPeriodsSmoothed"];
            pgrdPrice.Items[4].Label = Program.LanguageDefault.DictionarySettings["lblTypeAverageSmoothed"];
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
            if ((e.Item.Name == "ColorSchemes") || (e.Item.Name == "Position"))
            {
                e.EditorType = typeof(PropertyGridDropDownListEditor);
            }
        }

        private static void PgrdChartEditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "ColorSchemes":
                    {
                        PropertyGridDropDownListEditor editor = e.Editor as PropertyGridDropDownListEditor;
                        BaseDropDownListEditorElement editorElement = editor.EditorElement as BaseDropDownListEditorElement;

                        if (editorElement != null)
                        {
                            editorElement.ValueMember = "Text";
                            editorElement.DisplayMember = "Text";
                            editorElement.DataSource = Scheme.Instance().Schemes.Select(s => new ItemChart(s.Key, s.Value));

                            editorElement.SelectedValue = ((PropertyGridItem)e.Item).Value.ToString();
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
                        }
                    }
                    break;
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

        private static void PgrdChartEdited(object sender, PropertyGridItemEditedEventArgs e)
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
        private void TabChartPaint(object sender, PaintEventArgs e)
        {
            if ((pgrdChart == null) || (pgrdChart.Groups.Count <= 0))
                return;

            grpChart.Text = Program.LanguageDefault.DictionarySettings["grpChart"];

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
            pgrdChart.Items[12].Label = Program.LanguageDefault.DictionarySettings["lblPositionChart"];
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
                                colorBoxElement.ColorDialog.ColorDialogForm.ShowWebColors = false;
                                colorBoxElement.ColorDialog.ColorDialogForm.ShowSystemColors = false;
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

            grpStudies.Text = Program.LanguageDefault.DictionarySettings["grpStudies"];

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
            pgrdStudies.Items[7].Label = "0%";
            pgrdStudies.Items[8].Label = "38,2%";
            pgrdStudies.Items[9].Label = "50%";
            pgrdStudies.Items[10].Label = "61,8%";
            pgrdStudies.Items[11].Label = "100%";
            pgrdStudies.Items[12].Label = "161,8%";
            pgrdStudies.Items[13].Label = "261,8%";
        }

        #endregion
    }
}