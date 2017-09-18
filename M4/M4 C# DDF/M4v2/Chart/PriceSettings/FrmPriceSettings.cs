using System;
using System.Threading;
using System.Windows.Forms;
using M4.M4v2.Chart.IndicatorElements;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using M4;
using Telerik.WinControls;

namespace M4.M4v2.Chart.PriceSettings
{
    public partial class FrmPriceSettings : RadForm
    {
        private CtlPainelChart _mCtlParent;
        private Thread _thread;

        public FrmPriceSettings(CtlPainelChart parent)
        {
            _mCtlParent = parent;

            InitializeComponent();
            //LoadProxy();

            //LoadConfigProxy();

            TranslateForm();

            //tabChart.BackColor = Utils.GetDefaultBackColor();
            //tabProxy.BackColor = Utils.GetDefaultBackColor();
            //tabServer.BackColor = Utils.GetDefaultBackColor();
            //tabStudies.BackColor = Utils.GetDefaultBackColor();
            tabPrice.BackColor = Utils.GetDefaultBackColor();
        }

        private void TranslateForm()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            Text = Program.LanguageDefault.DictionarySettings["frmOptions"];

            btnOk.Text = Program.LanguageDefault.DictionarySettings["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionarySettings["btnCancel"];
            cbApplyAll.Text = Program.LanguageDefault.DictionarySettings["cbApplyAll"];
            //tabServer.Text = Program.LanguageDefault.DictionarySettings["tabServer"];
            //tabProxy.Text = Program.LanguageDefault.DictionarySettings["tabProxy"];
            //tabChart.Text = Program.LanguageDefault.DictionarySettings["tabChart"];
            //tabStudies.Text = Program.LanguageDefault.DictionarySettings["tabStudies"];
            tabPrice.Text = Program.LanguageDefault.DictionarySettings["tabPrice"];
        }

        private void LoadConfigPrice()
        {
            ConfigurePropertyGridPrice();
            
            PriceInfo priceInfo = new PriceInfo
            {
                //LineMono = _mCtlParent.StockChartX1.PriceLineMono,
                LineThickness = _mCtlParent.StockChartX1.PriceLineThickness,//Properties.Settings.Default.SettingsPriceLineThickness,
                BarLineThickness = _mCtlParent.StockChartX1.PriceLineThicknessBar,
                Period = _mCtlParent.StockChartX1.SmoothHeikinPeriods,
                TipoMedia = (Enums.TypeHeikin)_mCtlParent.StockChartX1.SmoothHeikinType
            };

            pgrdPrice.SelectedObject = priceInfo;
        }

        private void BtnCancelarClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnConfirmClick(object sender, EventArgs e)
        {
            pgrdPrice.EndEdit();
            SavePrice();
            Close();
        }

        private void SavePrice()
        {
            PriceInfo priceInfo = (PriceInfo)pgrdPrice.SelectedObject;

            if (cbApplyAll.Checked)
            {
                //Apply in All charts
                foreach (DockWindow window in frmMain2.GInstance.radDock2.DockWindows)
                {
                    if (window.AccessibleName.Equals("CtlPainelChart"))
                    {
                        CtlPainelChart ctlPanel = (CtlPainelChart)window.Controls[0];

                        //Heikin Ashi Smooth params
                        ctlPanel.StockChartX1.SmoothHeikinType = priceInfo.TipoMedia.GetHashCode();
                        ctlPanel.StockChartX1.SmoothHeikinPeriods = priceInfo.Period;
                        if (ctlPanel.m_PriceStyle == "Heikin Ashi Smooth") ctlPanel.ChangePriceStyle("Heikin Ashi Smooth",true);

                        //_mCtlParent.StockChartX1.PriceLineMono = priceInfo.LineMono;
                        ctlPanel.StockChartX1.PriceLineThickness = priceInfo.LineThickness;
                        ctlPanel.StockChartX1.PriceLineThicknessBar = priceInfo.BarLineThickness;

                        ctlPanel.StockChartX1.ForcePaint();
                    }
                }

            }

            //Apply only in current chart!!!

            //Heikin Ashi Smooth params
            _mCtlParent.StockChartX1.SmoothHeikinType = priceInfo.TipoMedia.GetHashCode();
            _mCtlParent.StockChartX1.SmoothHeikinPeriods = priceInfo.Period;

            //_mCtlParent.StockChartX1.PriceLineMono = priceInfo.LineMono;
            _mCtlParent.StockChartX1.PriceLineThickness = priceInfo.LineThickness;
            _mCtlParent.StockChartX1.PriceLineThicknessBar = priceInfo.BarLineThickness;


            if (_mCtlParent.m_PriceStyle == "Heikin Ashi Smooth") _mCtlParent.ChangePriceStyle("Heikin Ashi Smooth", true);
            else _mCtlParent.StockChartX1.ForcePaint();

        }

        private void FrmSettingsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;

            Close();
        }

        private void LoadTabs()
        {
            //LoadChart();
            LoadPrice();
            //LoadServer();
            //NewLoadStudies();

            //LoadConfigChart();
            LoadConfigPrice();
            //LoadConfigServer();
            //LoadConfigStudies();
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

    }
}