using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using M4.M4v2.Chart.IndicatorElements;
using M4.M4v2.Chart.Mock;
using Telerik.WinControls.UI;
using AxSTOCKCHARTXLib;
using Telerik.WinControls;
using System.Threading;

namespace M4.M4v2.Chart.TechnicalAnalysis
{
    public partial class FrmSelectIndicator : RadForm
    {
        #region Members and Properties

        public string Symbol { get; set; }
        public readonly Mock.Mock _mock = new Mock.Mock();
        public AxStockChartX StockChartX1 { get; set; }
        private int indicatorKey;
        public string SelectIndicator;
        public CtlPainelChart ParentForm;
        public string _searchIndicator = "";
        private ToolTip buttonsToolTip = new ToolTip();

        #endregion


        #region Initialize

        public FrmSelectIndicator()
        {
            InitializeComponent();

            InitializeIndicators(); //LoadIndicators(null);

            LoadDictionary();

            ConfigurePropertyGrid();

            this.GotFocus += FrmSelectIndicator_GotFocus;
        }

        private void ConfigurePropertyGrid()
        {
            pgrdIndicators.EnableSorting = false;
            pgrdIndicators.HelpVisible = true;
            pgrdIndicators.SortOrder = SortOrder.None;
            pgrdIndicators.PropertyValidating += pgrdIndicators_PropertyValidating;
            pgrdIndicators.Edited += pgrdIndicators_Edited;
            pgrdIndicators.EditorInitialized += pgrdIndicators_EditorInitialized;
            pgrdIndicators.PropertySort = PropertySort.Categorized;
            pgrdIndicators.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdIndicators.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;
        }

        #endregion

        #region Methods

        private void LoadDictionary()
        {
            RadMessageLocalizationProvider.CurrentProvider = new LocalizationProvider();
            // Create the ToolTips and associate with the Form buttons.

            // Set up the delays for the ToolTips:
            buttonsToolTip.AutoPopDelay = 5000;
            buttonsToolTip.InitialDelay = 500;
            buttonsToolTip.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            buttonsToolTip.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            buttonsToolTip.SetToolTip(btnNew, Program.LanguageDefault.DictionarySelectIndicator["btnNew"]);
            buttonsToolTip.SetToolTip(btnRemove, Program.LanguageDefault.DictionarySelectIndicator["btnRemove"]);
            buttonsToolTip.SetToolTip(btnApply, Program.LanguageDefault.DictionarySelectIndicator["btnApply"]);

            Text = Program.LanguageDefault.DictionarySelectIndicator["FrmSelectIndicatorTitle"];
            lblIndicators.Text = Program.LanguageDefault.DictionarySelectIndicator["lblIndicators"];
            btnDismiss.Text = Program.LanguageDefault.DictionarySelectChart["btnDismiss"];
            pgrdIndicators.Text = Program.LanguageDefault.DictionarySelectIndicator["pgrdIndicators"];
        }

        private void LoadItemsDictionary()
        {
            //Load Dictionary for groups
            foreach (PropertyGridGroupItem group in pgrdIndicators.Groups)
            {
                switch (group.Label)
                {
                    case "Average":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprAverage"];
                        break;
                    case "Value":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprValue"];
                        break;
                    case "Parameters":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprParameters"];
                        break;
                    case "%K Parameters":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprKParameters"];
                        break;
                    case "%D Parameters":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprDParameters"];
                        break;
                    case "View":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprView"];
                        break;
                    case "ZThresholds":
                        group.Label = Program.LanguageDefault.DictionarySelectIndicator["gprThresholds"];
                        break;
                }

            }
            //Load Dictionary for properties
            foreach (PropertyGridItem item in pgrdIndicators.Items)
            {
                switch (item.Label)
                {

                    case "Periods":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblPeriods"];
                        if (pgrdIndicators.SelectedObject.GetType().ToString().Contains("HILO")) item.Label += " High";
                        break;
                    case "ColorAverage":
                    case "ColorValue":
                    case "ColorHistogram":
                    case "ColorParameters":
                    case "Color2Parameters":
                    case "ColorT":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblColor"];
                        if (pgrdIndicators.SelectedObject.GetType().ToString().Contains("HILO")) item.Label += " High";
                        break;
                    case "LineStyleAverage":
                    case "LineStyleValue":
                    case "LineStyleParameters":
                    case "LineStyle2Parameters":
                    case "LineStyleT":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblLineStyle"];
                        break;
                    case "LineThicknessAverage":
                    case "LineThicknessValue":
                    case "LineThicknessParameters":
                    case "LineThickness2Parameters":
                    case "LineThicknessT":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblLineThickness"];
                        break;
                    case "ShortCycle":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblShortCycle"];
                        break;
                    case "LongCycle":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblLongCycle"];
                        break;
                    case "Type":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblType"];
                        break;
                    case "SourceAverageOHLC":
                    case "SourceAverage":
                    case "SourceParameters":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblSourceAverage"];
                        break;
                    case "ShiftParameters":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblShiftParameters"];
                        break;
                    case "ScaleAverage":
                        if (pgrdIndicators.SelectedObject.GetType().ToString().Contains("HILO")) item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblVariation"];
                        else item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblScaleAverage"];
                        break;
                    case "Deviation":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblDeviation"];
                        break;
                    case "Percentage":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblPercentage"];
                        break;
                    case "StandardDev":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblStandardDev"];
                        break;
                    case "KPeriods":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblKPeriodsParameters"];
                        break;
                    case "DPeriods":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblDPeriodsParameters"];
                        break;
                    case "DType":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblDType"];
                        break;
                    case "KSmooth":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblKSmooth"];
                        break;
                    case "KDblSmooth":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblKDblSmooth"];
                        break;
                    case "KSlowingParameters":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblKSlowingParameters"];
                        break;
                    case "LimitMoveValue":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblLimitMoveValue"];
                        break;
                    case "RatOfChg":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblRatOfChg"];
                        break;
                    case "Threshold1":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblThreshold1"];
                        break;
                    case "Threshold2":
                        item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblThreshold2"];
                        break;

                }
            }

            
        }

        public void InitializeIndicators()
        {
            trvIndicators.Nodes.Clear();
            trvIndicators.Refresh();
            string indicatorCode, indicatorDescription;

            //Indicadores ordenados pelo nome
            var indicators = _mock.Indicators.OrderBy(r => r.Description).ToList();
            for (int i = 0; i < indicators.Count; i++)
            {
                trvIndicators.Nodes.Add(new RadTreeNode(indicators[i].Description) { Value = indicators[i].Code, Name = indicators[i].Description });
            }
            trvIndicators.SelectedNode = trvIndicators.Nodes.First();
            trvIndicators.ExpandAll();
            trvIndicators.Refresh();
            trvIndicators.BringIntoView(trvIndicators.SelectedNode);
            trvIndicators.Refresh();

        }

        public void GetStockChartIndicators()
        {
            #region Inicialization

            string indicatorDescription = "";

            //Clean All Nodes
            trvIndicators.ShowLines = false;
            trvIndicators.Nodes.Clear();
            trvIndicators.ShowLines = true;
            trvIndicators.Refresh();
            _mock.ClearIndicatorsMock();

            //Load Indicators Types (parent nodes)
            var indicators = _mock.Indicators.OrderBy(r => r.Description).ToList();
            for (int i = 0; i < indicators.Count; i++)
            {
                trvIndicators.Nodes.Add(new RadTreeNode(indicators[i].Description) { Value = indicators[i].Code, Name = indicators[i].Description });
            }
            //Load Indicators from Stockchart (child nodes)
            for (int i = 0; i < StockChartX1.SeriesCount; i++)
            {
                if (!((StockChartX1.get_SeriesName(i).Contains(".open")) ||
                    (StockChartX1.get_SeriesName(i).Contains(".high")) ||
                    (StockChartX1.get_SeriesName(i).Contains(".low")) ||
                    (StockChartX1.get_SeriesName(i).Contains(".close")) ||
                    (StockChartX1.get_SeriesName(i).Contains(".volume")) ||
                    (StockChartX1.get_SeriesName(i).Contains(" Down")) ||
                    (StockChartX1.get_SeriesName(i).Contains(" Top")) ||
                    (StockChartX1.get_SeriesName(i).Contains(" Bottom")) ||
                    //(StockChartX1.get_SeriesName(i).Contains(" DI+")) ||
                    (StockChartX1.get_SeriesName(i).Contains(" DI-")) ||
                    ((StockChartX1.get_SeriesName(i).Contains("DI+/DI-/ADX")&&
                    StockChartX1.get_SeriesName(i).Contains(" ADX"))) ||
                    (StockChartX1.get_SeriesName(i).Contains(" High")) ||
                    (StockChartX1.get_SeriesName(i).Contains(" %K")) ||
                    ((StockChartX1.get_SeriesName(i).Contains("RSI")) &&
                    ((StockChartX1.get_SeriesName(i).Contains(" 30")) ||
                     (StockChartX1.get_SeriesName(i).Contains(" 70"))))||
                    (StockChartX1.get_SeriesName(i).Contains(" Signal"))))
                {
                    Mock.Indicator indicatorSelected =
                        _mock.GetIndicatorByCode(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""));
                    if(indicatorSelected == null)
                    {

                        Telerik.WinControls.RadMessageBox.Show("Indicador não reconhecido:\n" + StockChartX1.get_SeriesName(i)); //Just comment this!
                        return;
                    }
                    IndicatorMock indicatorMock = new IndicatorMock();
                    string indicatorName = StockChartX1.get_SeriesName(i);
                    int OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName);
                    Color indicatorColor = ColorTranslator.FromOle(OLE_COLOR);
                    int OLE_COLOR2;
                    Color indicatorColor2;
                    Color indicatorSignalColor;
                    Enums.SourceOHLC indicatorSourceOHLC = new Enums.SourceOHLC();
                    Enums.Source indicatorSource = new Enums.Source();
                    Enums.Source indicatorSource2 = new Enums.Source();
                    String MAType = "";
                    string[] splitSource, splitSource2;
                    double Shift;
                    string PointPercent = "Point";
            #endregion

            #region Indicators

                    switch (indicatorSelected.Code)
                    {

					    #region BB

                        case "BB":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                StandardDev = (int)StockChartX1.get_SeriesStandarDev(indicatorName, indicatorSelected.Code)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Bollinger Bands"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region HV

                        case "HV":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("HV", "VH") : indicatorName) + " " + (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                StandardDev = (int)StockChartX1.get_SeriesStandarDev(indicatorName, indicatorSelected.Code)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Historical Volatility"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region HILO



                        case "HILO":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName+" Low");
                            indicatorColor2 = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code+" Low") +
                                    "," + StockChartX1.get_SeriesShift(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                PeriodsParameters = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code + " Low"),
                                ShiftParameters = StockChartX1.get_SeriesShiftInt(indicatorName, indicatorSelected.Code),
                                ScaleAverage = StockChartX1.get_SeriesR2Scale(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                Color2Parameters = indicatorColor2,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Moving Average"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;
#endregion

                        #region MA
      



                        case "MA":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("MA","MM") : indicatorName) +" "+ (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code)+"," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverage = indicatorSource,
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                ShiftParameters =StockChartX1.get_SeriesShiftInt(indicatorName, indicatorSelected.Code),
                                ScaleAverage = StockChartX1.get_SeriesR2Scale(indicatorName, indicatorSelected.Code),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Moving Average"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region MAE





                        case "MAE":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     (Program.LanguageStockChartX == 1 ? indicatorName.Replace("MAE", "MME") : indicatorName) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                ScaleAverage = StockChartX1.get_SeriesShift(indicatorName, indicatorSelected.Code)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Moving Average Envelope"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region MACD



                        case "MACD":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " Signal");
                            indicatorSignalColor = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + "," +
                                    StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesShortCycle(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesLongCycle(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorSignalColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                ShortCycle = StockChartX1.get_SeriesShortCycle(indicatorName, indicatorSelected.Code),
                                LongCycle = StockChartX1.get_SeriesLongCycle(indicatorName, indicatorSelected.Code),
                                ColorValue = indicatorColor,
                                LineStyleValue =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " Signal"),
                                LineThicknessValue = StockChartX1.get_SeriesWeight(indicatorName + " Signal"),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = 
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["MACD"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region MACD-H





                        case "MACD-H":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + "," +
                                    StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesShortCycle(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesLongCycle(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorHistogram = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                ShortCycle = StockChartX1.get_SeriesShortCycle(indicatorName, indicatorSelected.Code),
                                LongCycle = StockChartX1.get_SeriesLongCycle(indicatorName, indicatorSelected.Code),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["MACD Histogram"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region RSI


                        case "RSI":
                            double Threshold1, Threshold2;
                            int LineThicknessT;
                            Color ColorT;
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName+" 30");
                            ColorT = ColorTranslator.FromOle(OLE_COLOR);
                            IndicatorElements.Enums.LineStyle LineStyleT;
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     (Program.LanguageStockChartX == 1 ? indicatorName.Replace("RSI", "IFR") : indicatorName) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverage = indicatorSource,
                                Threshold1 = StockChartX1.get_SeriesThreshold(indicatorName, 2),
                                Threshold2 = StockChartX1.get_SeriesThreshold(indicatorName, 3),
                                ColorT = ColorT,
                                LineThicknessT = StockChartX1.get_SeriesWeight(indicatorName+" 30"),
                                LineStyleT = (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " 30"),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Relative Strength Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region VOL



                        case "VOL":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                                                {
                                                    Code = indicatorSelected.Code,
                                                    CodeMock = indicatorName,
                                                    Description =
                                                        indicatorName + "," + splitSource[1][0] + splitSource[1][1],
                                                    ColorParameters = indicatorColor,
                                                    SourceParameters = indicatorSource
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Volume"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region TP
                        case "TP":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                   (Program.LanguageStockChartX == 1 ? indicatorName.Replace("TP", "PT") : indicatorName),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Typical Price"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

#endregion

                        #region PSAR

                        case "PSAR":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesMinAF(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_MaxAF(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                MinAf = 
                                    StockChartX1.get_SeriesMinAF(indicatorName, indicatorSelected.Code),
                                MaxAf = 
                                    StockChartX1.get_MaxAF(indicatorName, indicatorSelected.Code)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Parabolic SAR"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region SMI



                        case "SMI":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " %K");
                            indicatorSignalColor = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("SMI", "ME") : indicatorName) + " " + StockChartX1.get_SeriesKPeriod(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesDPeriod(indicatorName, indicatorSelected.Code),
                                DPeriodsParameters = StockChartX1.get_SeriesDPeriod(indicatorName, indicatorSelected.Code),
                                KPeriodsParameters = StockChartX1.get_SeriesKPeriod(indicatorName, indicatorSelected.Code),
                                ColorAverage = indicatorSignalColor,
                                Smooth = StockChartX1.get_SeriesKSmooth(indicatorName, indicatorSelected.Code),
                                KDblSmooth = StockChartX1.get_SeriesPctKDblSmooth(indicatorName, indicatorSelected.Code),
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                DType = (Enums.Type)StockChartX1.get_SeriesDMAType(indicatorName, indicatorSelected.Code),
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " %K"),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName + " %K"),
                                ColorValue = indicatorColor,
                                LineStyleValue =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessValue = StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Stochastic Momentum Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region SO
                            
                        case "SO":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " %K");
                            indicatorSignalColor = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("SO", "OE") : indicatorName) + " " + StockChartX1.get_SeriesKPeriod(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesDPeriod(indicatorName, indicatorSelected.Code),
                                DPeriodsParameters = StockChartX1.get_SeriesDPeriod(indicatorName, indicatorSelected.Code),
                                KPeriodsParameters = StockChartX1.get_SeriesKPeriod(indicatorName, indicatorSelected.Code),
                                ColorValue = indicatorSignalColor,
                                KSlowingParameters = StockChartX1.get_SeriesKSlowing(indicatorName, indicatorSelected.Code),
                                LineStyleValue =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName+ " %K"),
                                LineThicknessValue =
                                    StockChartX1.get_SeriesWeight(indicatorName+ " %K"),
                                ColorAverage = indicatorColor,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessAverage = StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Stochastic Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region Aroon

                        case "Aroon":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " Down");
                            indicatorColor2 = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) ,
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters = 
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters = 
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                Color2Parameters = indicatorColor2,
                                LineStyle2Parameters = 
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " Down"),
                                LineThickness2Parameters = StockChartX1.get_SeriesWeight(indicatorName + " Down"),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = "Aroon";
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region Aroon Oscillator

                        case "AO":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Aroon Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region CMF

                        case "CMF":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters = 
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = "CMF Chaikin Money Flow";
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region EOM

                        case "EOM":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Type = 
                                     (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = "EOM Ease Of Movement";
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region MFI
                            
                        case "MFI":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("MFI", "IFD") : indicatorName) + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters = 
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Money Flow Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region ASI

                        case "ASI":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     (Program.LanguageStockChartX == 1 ? indicatorName.Replace("ASI", "IBA") : indicatorName) + " " + StockChartX1.get_SeriesLimitMove(indicatorName, indicatorSelected.Code),
                                LimitMoveValue = StockChartX1.get_SeriesLimitMove(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Accumulative Swing Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region CV

                        case "CV":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     (Program.LanguageStockChartX == 1 ? indicatorName.Replace("CV", "VC") : indicatorName) + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code)
                                     +","+ StockChartX1.get_SeriesRateChange(indicatorName,indicatorSelected.Code),
                                RatOfChg = StockChartX1.get_SeriesRateChange(indicatorName,indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName,indicatorSelected.Code),
                                Type = (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName,indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Chaikin Volatility"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region CMO

                        case "CMO":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code)+"," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Chande Momentum Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region WWS

                        case "WWS":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Welles Wilder Smoothing"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region VHF
                        case "VHF":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Vertical Horizontal Filter"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region TRIX
                        case "TRIX":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = "TRIX";
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region LRRS
                        case "LRRS":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression R-Squared"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region LRF
                        case "LRF":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Forecast"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region LRS
                        case "LRS":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Slope"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region LRS
                        case "LRI":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Intercept"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region MO
                        case "MO":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSource = Enums.Source.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSource = Enums.Source.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSource = Enums.Source.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSource = Enums.Source.Fechamento;
                                    break;
                                case "VOLUME":
                                    indicatorSource = Enums.Source.Volume;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) + "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                SourceParameters = indicatorSource,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Momentum Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region ADX
                        case "ADX":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["ADX"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region DI
                        case "DI+/DI-":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " DI-");
                            indicatorSignalColor = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                Color2Parameters = indicatorSignalColor,
                                LineStyle2Parameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " DI-"),
                                LineThickness2Parameters =
                                    StockChartX1.get_SeriesWeight(indicatorName + " DI-")
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["DI+/DI-"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region DI ADX
                        case "DI+/DI-/ADX":
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " DI-");
                            indicatorSignalColor = ColorTranslator.FromOle(OLE_COLOR);
                            OLE_COLOR = StockChartX1.get_SeriesColor(indicatorName + " ADX");
                            indicatorColor2 = ColorTranslator.FromOle(OLE_COLOR);
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                Color2Parameters = indicatorSignalColor,
                                LineStyle2Parameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " DI-"),
                                LineThickness2Parameters =
                                    StockChartX1.get_SeriesWeight(indicatorName + " DI-"),
                                ColorAverage = indicatorColor2,
                                LineStyleAverage =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName + " ADX"),
                                LineThicknessAverage =
                                    StockChartX1.get_SeriesWeight(indicatorName + " ADX"),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["DI+/DI-/ADX"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region CCI

                        case "CCI":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                      indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName)
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Commodity Channel Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region DPO

                        case "DPO": 
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code) + "," + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Type =
                                     (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Detrended Price Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region MI

                        case "MI":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (Program.LanguageStockChartX == 1 ? indicatorName.Replace("MI", "IM") : indicatorName) + " " 
                                    + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Mass Index"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region OBV

                        case "OBV":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    (indicatorName) + " "+ splitSource[1],
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["On Balance Volume"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region PROC

                        case "PROC":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Price ROC"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region PVT

                        case "PVT":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName + " " + splitSource[1][0] + splitSource[1][1],
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Price Volume Trend"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region WilliamsR

                        case "Williams":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Williams %R"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region WAD

                        case "WAD":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName ,
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Williams Accumulation Distribution"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region WC

                        case "WC":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName,
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Wheighted Close"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region VROC

                        case "VROC":
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code) +
                                    "," + splitSource[1][0] + splitSource[1][1],
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                                SourceAverageOHLC = indicatorSourceOHLC,
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Volume ROC"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region FCO

                        case "FCO":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                Periods = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Fractal Chaos Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region FCB

                        case "FCB":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                PeriodsParameters = StockChartX1.get_SeriesPeriods(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Fractal Chaos Bands"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region UO

                        case "UO":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                    indicatorName + " " + StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 0)
                                    + "," + StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 1)
                                    + "," + StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 2),
                                Cycle1 = StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 1),
                                Cycle2 = StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 2),
                                Cycle3 = StockChartX1.get_SeriesCycle(indicatorName, indicatorSelected.Code, 3),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Ultimate Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region TR

                        case "TR":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName,
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["True Range"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region RO

                        case "RO": 
                            splitSource = StockChartX1.get_SeriesSource(indicatorName, indicatorSelected.Code).Split(new char[] { '.' });
                            splitSource[1] = splitSource[1].ToUpper();
                            switch (splitSource[1])
                            {
                                case "OPEN":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Abertura;
                                    break;
                                case "HIGH":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Máximo;
                                    break;
                                case "LOW":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Mínimo;
                                    break;
                                case "CLOSE":
                                    indicatorSourceOHLC = Enums.SourceOHLC.Fechamento;
                                    break;
                            }
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description = 
                                    " " + (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code) 
                                    + "," + StockChartX1.get_SeriesLevel(indicatorName, indicatorSelected.Code) 
                                    + "," + splitSource[1][0] + splitSource[1][1],
                                Levels = StockChartX1.get_SeriesLevel(indicatorName, indicatorSelected.Code),
                                SourceAverageOHLC = indicatorSourceOHLC,
                                Type =
                                     (Enums.Type)StockChartX1.get_SeriesMAType(indicatorName, indicatorSelected.Code),
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Rainbow Oscillator"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        #region A/D

                        case "A/D":
                            indicatorMock = new IndicatorMock
                            {
                                Code = indicatorSelected.Code,
                                CodeMock = indicatorName,
                                Description =
                                     indicatorName,
                                ColorParameters = indicatorColor,
                                LineStyleParameters =
                                    (Enums.LineStyle)StockChartX1.get_SeriesStyle(indicatorName),
                                LineThicknessParameters =
                                    StockChartX1.get_SeriesWeight(indicatorName),
                            };
                            _mock.AddIndicatorMock(Regex.Replace(StockChartX1.get_SeriesName(i), @"\d", ""), indicatorMock);
                            indicatorDescription = Program.LanguageDefault.DictionarySelectIndicator["Accumulation/Distribution"];
                            trvIndicators.Nodes[indicatorDescription].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });
                            break;

                        #endregion

                        default:
                            return;

                    }
                    
                }
            }
            #endregion

            LoadIndicators(SelectIndicator);
        }

        public void LoadIndicators(string code)
        {
            if (!String.IsNullOrEmpty(code)) {
                if (code.Contains(" Bottom")) code=code.Replace(" Bottom", "");
                else if (code.Contains(" Top")) code = code.Replace(" Top", "");
                else if (code.Contains(" Up")) code = code.Replace(" Up", "");
                else if (code.Contains(" Down")) code = code.Replace(" Down", "");
                else if (code.Contains(" Signal")) code = code.Replace(" Signal", "");
                else if (code.Contains(" 30")) code = code.Replace(" 30", "");
                else if (code.Contains(" 70")) code = code.Replace(" 70", "");
                else if (code.Contains(" ADX")) code = code.Replace(" ADX", "");
                else if (code.Contains(" DI+")) code = code.Replace(" DI+", "");
                else if (code.Contains(" DI-")) code = code.Replace(" DI-", "");
            }
            /*try
            {
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR : " + ex.Message);
                //return;
            }*/

            trvIndicators.ShowLines = false;
            trvIndicators.Nodes.Clear();
            trvIndicators.ShowLines = true;
            trvIndicators.Refresh();
            string indicatorCode, indicatorDescription;

            //Indicadores ordenados pelo nome
            var indicators = _mock.Indicators.OrderBy(r => r.Description).ToList();
            trvIndicators.SelectedNode = null;
            for (int i = 0; i < indicators.Count; i++)
            {
                trvIndicators.Nodes.Add(new RadTreeNode(indicators[i].Description) { Value = indicators[i].Code, Name = indicators[i].Description });
                if (indicators[i].Code.Equals(code))
                {
                    trvIndicators.SelectedNode = trvIndicators.Nodes.Last();
                }

                foreach (var indicatorMock in indicators[i].IndicatorsMocks)
                {
                    trvIndicators.Nodes[i].Nodes.Add(new RadTreeNode(indicatorMock.Description) { Value = indicatorMock.CodeMock, Name = indicatorMock.Description });

                    if (!String.IsNullOrEmpty(code))
                    {
                        if (indicatorMock.CodeMock.Equals(code))
                        {
                            trvIndicators.SelectedNode = trvIndicators.Nodes[i].LastNode;
                            trvIndicators.ExpandAll();
                            //return;
                        }

                    }
                }
            }
            if (trvIndicators.SelectedNode == null) trvIndicators.SelectedNode = trvIndicators.Nodes.First();

            trvIndicators.ExpandAll();
            trvIndicators.Refresh(); 
            //trvIndicators.BringIntoView(trvIndicators.SelectedNode);
            //trvIndicators.Refresh(); 
            if (trvIndicators.SelectedNode.Parent == null)
            {
                if (trvIndicators.SelectedNode.Index+7 < trvIndicators.Nodes.Count()) trvIndicators.BringIntoView(trvIndicators.Nodes[trvIndicators.SelectedNode.Index+7]);
                else trvIndicators.BringIntoView(trvIndicators.Nodes.Last());
            }
            else
            {
                if (trvIndicators.SelectedNode.Parent.Index+7 < trvIndicators.Nodes.Count()) trvIndicators.BringIntoView(trvIndicators.Nodes[trvIndicators.SelectedNode.Parent.Index+7]);
                else trvIndicators.BringIntoView(trvIndicators.Nodes.Last());
            }
            trvIndicators.Refresh();
        }


        #endregion

        #region PropertyGrid Events

        private void pgrdIndicators_EditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            PropertyGridColorEditor gridColorEditor = e.Editor as PropertyGridColorEditor;

            if (gridColorEditor != null)
            {
                RadColorBoxElement colorBoxElement = ((PropertyGridColorEditor)e.Editor).EditorElement as RadColorBoxElement;

                if (colorBoxElement != null)
                {
                    //colorBoxElement.ColorDialog.ColorDialogForm.ShowSystemColors = false;
                    //colorBoxElement.ColorDialog.ColorDialogForm.ShowWebColors = false;
                    colorBoxElement.ColorDialog.ColorDialogForm.ActiveMode = ColorPickerActiveMode.Basic;
                }
            }

            PropertyGridSpinEditor editor = e.Editor as PropertyGridSpinEditor;

            if (editor == null)
                return;

            BaseSpinEditorElement element = editor.EditorElement as BaseSpinEditorElement;

            switch (e.Item.Name)
            {
                case "LineThicknessValue":
                case "LineThicknessAverage":
                case "LineThicknessParameters":
                case "LineThickness2Parameters":
                case "LineThicknessT":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 10;
                    }
                    break;
                case "ShortCycle":
                case "ShortTermParameters":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "LongCycle":
                case "LongTermParameters":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "Periods":
                case "DPeriods":
                case "KPeriods":
                case "PeriodsParameters":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "Smooth":
                case "KDblSmooth":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "Slowing":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "RatOfChg":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
                case "Threshold1":
                case "Threshold2":
                case "ScaleAverage":
                    if (element != null)
                    {
                        element.MinValue = 0;
                        element.MaxValue = 100;
                    }
                    break;
            }
        }

        private void pgrdIndicators_Edited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;

            if (item != null)
                item.ErrorMessage = "";
        }

        private void pgrdIndicators_PropertyValidating(object sender, PropertyValidatingEventArgs e)
        {
            PropertyGridItem item = (PropertyGridItem)e.Item;

            if (((e.Item.Name == "ShortCycle") || (e.Item.Name == "LongCycle") || (e.Item.Name == "ShortTermParameters") ||
                (e.Item.Name == "LongTermParameters")) && (int.Parse(e.NewValue.ToString()) < 1))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgCycleMinimum"];
                e.Cancel = true;
            }

            if (((e.Item.Name == "Periods") || (e.Item.Name == "PeriodsParameters")) && (int.Parse(e.NewValue.ToString()) < 1))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgPeriodsMinimum"];
                e.Cancel = true;
            }

            if (((e.Item.Name == "ShortCycle") || (e.Item.Name == "LongCycle") || (e.Item.Name == "ShortTermParameters") ||
                (e.Item.Name == "LongTermParameters")) && (String.IsNullOrEmpty(e.NewValue.ToString())))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgCycleIsEmpty"];
                e.Cancel = true;
            }

            if (((e.Item.Name == "Periods") || (e.Item.Name == "PeriodsParameters")) && (String.IsNullOrEmpty(e.NewValue.ToString())))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgPeriodsIsEmpty"];
                e.Cancel = true;
            }
            if (((e.Item.Name == "Threshold1") || (e.Item.Name == "Threshold2")) && (String.IsNullOrEmpty(e.NewValue.ToString())))
            {
                item.ErrorMessage = Program.LanguageDefault.DictionaryMessage["msgThresholdIsEmpty"];
                e.Cancel = true;
            }
        }

        private void pgrdIndicators_ItemFormatting(object sender, PropertyGridItemFormattingEventArgs e)
        {
            //Telerik.WinControls.RadMessageBox.Show(e.Item.Label);
            if (pgrdIndicators.SelectedObject.GetType().ToString().Contains("MAInfo") &&!(pgrdIndicators.SelectedObject.GetType().ToString().Contains("SMAInfo")||pgrdIndicators.SelectedObject.GetType().ToString().Contains("EMAInfo")))
            {
                try
                {
                    if (pgrdIndicators.Items[0].Value != null)
                        if ((Enums.Type)pgrdIndicators.Items[0].Value != Enums.Type.VIDYA)
                        {
                            if (pgrdIndicators.Items[4].Visible) pgrdIndicators.Items[4].Visible = false;
                        }
                        else if (!pgrdIndicators.Items[4].Visible) pgrdIndicators.Items[4].Visible = true;
                }
                catch(Exception ex)
                {
                }
            }
            else if (pgrdIndicators.SelectedObject.GetType().ToString().Contains("HILO"))
            {

                foreach (PropertyGridItem item in pgrdIndicators.Items)
                {
                    switch (item.Label)
                    {
                        case "Periods":
                            item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblPeriods"]+" High";
                            break;
                        case "PeriodsParameters":
                            item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblPeriods"] + " Low";
                            break;
                        case "ColorParameters":
                            item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblColor"] + " High";
                            break;
                        case "Color2Parameters":
                            item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblColor"] + " Low";
                            break;
                        case "ScaleAverage":
                            item.Label = Program.LanguageDefault.DictionarySelectIndicator["lblVariation"];
                            break;


                    }
                }
            }
        }

        #endregion

        #region TreeView Events

        private void RadTreeView1NodeFormatting(object sender, TreeNodeFormattingEventArgs args)
        {
            if (args.Node.Parent == null)
                return;
            args.NodeElement.ContentElement.Font = new Font("Arial", 8, FontStyle.Italic);
        }

        private void trvIndicators_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            //Before allow treeview node's change must finish any prperty grid operation!
            pgrdIndicators.EndEdit();
            pgrdIndicators.SelectedGridItem = null;
            pgrdIndicators.Update();
        }

        private void TrvIndicatorsSelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node == null)
                return;
            try
            {
                #region Bollinger Bands

                if (e.Node.Text.Contains("BB"))
                {
                    BBInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new BBInfo
                                            {
                                                LineStyleAverage = indicatorMock.LineStyleAverage,
                                                LineThicknessAverage = indicatorMock.LineThicknessAverage,
                                                ColorAverage = indicatorMock.ColorAverage,
                                                Type = indicatorMock.Type,
                                                Periods = indicatorMock.Periods,
                                                SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                                                StandardDev = indicatorMock.StandardDev

                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new BBInfo
                                            {
                                                LineStyleAverage = Enums.LineStyle.Normal,
                                                LineThicknessAverage = 1,
                                                ColorAverage = Color.Red,
                                                Type = Enums.Type.Simples,
                                                Periods = 20,
                                                SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                                                StandardDev = 2
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region Historical Volatility

                if (e.Node.Text.Contains("HV") || e.Node.Text.Contains("VH"))
                {
                    HVInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new HVInfo
                        {
                            LineStyleAverage = indicatorMock.LineStyleAverage,
                            LineThicknessAverage = indicatorMock.LineThicknessAverage,
                            ColorAverage = indicatorMock.ColorAverage,
                            Type = indicatorMock.Type,
                            Periods = indicatorMock.Periods,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            StandardDev = indicatorMock.StandardDev

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new HVInfo
                        {
                            LineStyleAverage = Enums.LineStyle.Normal,
                            LineThicknessAverage = 2,
                            ColorAverage = Color.Red,
                            Type = Enums.Type.Simples,
                            Periods = 14,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                            StandardDev = 2
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region HILO Activator

                else if (e.Node.Text.Contains("HILO"))
                {
                    HILOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new HILOInfo
                        {
                            Periods = indicatorMock.Periods,
                            PeriodsParameters = indicatorMock.PeriodsParameters,
                            ShiftParameters = indicatorMock.ShiftParameters,
                            ScaleAverage = indicatorMock.ScaleAverage,
                            ColorParameters = indicatorMock.ColorParameters,
                            Color2Parameters = indicatorMock.Color2Parameters,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new HILOInfo
                        {
                            Periods = 3,
                            PeriodsParameters = 3,
                            ShiftParameters = 1,
                            ScaleAverage = 0,
                            ColorParameters = Color.Red,
                            Color2Parameters = Color.Green,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 1
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region Relative Strength Index

                else if (e.Node.Text.Contains("RSI") || e.Node.Text.Contains("IFR"))
                {
                    RSIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new RSIInfo
                                            {
                                                LineStyleAverage = indicatorMock.LineStyleAverage,
                                                LineThicknessAverage = indicatorMock.LineThicknessAverage,
                                                ColorAverage = indicatorMock.ColorAverage,
                                                Periods = indicatorMock.Periods,
                                                SourceAverage = indicatorMock.SourceAverage,
                                                Threshold1 = indicatorMock.Threshold1,
                                                Threshold2 = indicatorMock.Threshold2,
                                                ColorT = indicatorMock.ColorT,
                                                LineStyleT = indicatorMock.LineStyleT,
                                                LineThicknessT = indicatorMock.LineThicknessT
                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new RSIInfo
                                            {
                                                LineStyleAverage = Enums.LineStyle.Normal,
                                                LineThicknessAverage = 2,
                                                ColorAverage = Color.Blue,
                                                Periods = 9,
                                                SourceAverage = Enums.Source.Fechamento,
                                                Threshold1 = 10.0,
                                                Threshold2 = 90.0,
                                                ColorT = Color.Red,
                                                LineStyleT = Enums.LineStyle.Pontilhados,
                                                LineThicknessT = 1
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region MACD Histogram

                if (e.Node.Text.Contains("MACD Histogram") || e.Node.Text.Contains("MACD-H"))
                {
                    MACDHInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new MACDHInfo
                                            {
                                                ShortCycle = indicatorMock.ShortCycle,
                                                LongCycle = indicatorMock.LongCycle,
                                                Periods = indicatorMock.Periods,
                                                ColorHistogram = indicatorMock.ColorHistogram
                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MACDHInfo
                                            {
                                                ShortCycle = 12,
                                                LongCycle = 26,
                                                Periods = 9,
                                                ColorHistogram = Color.DarkOrange
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }
                #endregion

                #region MACD

                else if (e.Node.Text.Contains("MACD"))
                {
                    MACDInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new MACDInfo
                                            {
                                                ShortCycle = indicatorMock.ShortCycle,
                                                LongCycle = indicatorMock.LongCycle,
                                                Periods = indicatorMock.Periods,
                                                ColorAverage = indicatorMock.ColorAverage,
                                                ColorValue = indicatorMock.ColorValue,
                                                LineStyleAverage = indicatorMock.LineStyleAverage,
                                                LineThicknessAverage = indicatorMock.LineThicknessAverage,
                                                LineStyleValue = indicatorMock.LineStyleValue,
                                                LineThicknessValue = indicatorMock.LineThicknessValue
                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MACDInfo
                                            {
                                                ShortCycle = 12,
                                                LongCycle = 26,
                                                Periods = 9,
                                                ColorAverage = Color.Red,
                                                ColorValue = Color.Green,
                                                LineStyleAverage = Enums.LineStyle.Normal,
                                                LineThicknessAverage = 2,
                                                LineStyleValue = Enums.LineStyle.Normal,
                                                LineThicknessValue = 2
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }
                #endregion

                #region Moving Average Envelope

                else if (e.Node.Text.Contains("MAE") || e.Node.Text.Contains("MME"))
                {
                    MAEInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new MAEInfo
                                            {
                                                LineStyleAverage = indicatorMock.LineStyleAverage,
                                                LineThicknessAverage = indicatorMock.LineThicknessAverage,
                                                ColorAverage = indicatorMock.ColorAverage,
                                                Type = indicatorMock.Type,
                                                Periods = indicatorMock.Periods,
                                                SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                                                Percentage = indicatorMock.ScaleAverage

                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MAEInfo
                                            {
                                                LineStyleAverage = Enums.LineStyle.Normal,
                                                LineThicknessAverage = 1,
                                                ColorAverage = Color.Red,
                                                Type = Enums.Type.Simples,
                                                Periods = 9,
                                                SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                                                Percentage = 1
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region Moving Average

                else if (e.Node.Text.Contains("MA") || e.Node.Text.Contains("MM"))
                {
                    MAInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new MAInfo
                                            {
                                                LineStyleAverage = indicatorMock.LineStyleAverage,
                                                LineThicknessAverage = indicatorMock.LineThicknessAverage,
                                                ColorAverage = indicatorMock.ColorAverage,
                                                Type = indicatorMock.Type,
                                                Periods = indicatorMock.Periods,
                                                SourceAverage = indicatorMock.SourceAverage,
                                                ShiftParameters = indicatorMock.ShiftParameters,
                                                ScaleAverage = indicatorMock.ScaleAverage

                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MAInfo
                                            {
                                                LineStyleAverage = Enums.LineStyle.Normal,
                                                LineThicknessAverage = 1,
                                                ColorAverage = Color.Red,
                                                Type = Enums.Type.Simples,
                                                Periods = 9,
                                                SourceAverage = Enums.Source.Fechamento,
                                                ShiftParameters = 0,
                                                ScaleAverage = 1.0
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region Volume

                else if (e.Node.Text.Contains("Volume") || e.Node.Text.Contains("VOL"))
                {
                    VolumeInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new VolumeInfo
                                            {
                                                ColorParameters = indicatorMock.ColorParameters,
                                                SourceParameters = indicatorMock.SourceVolume

                                            };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new VolumeInfo
                                            {
                                                ColorParameters = SystemColors.MenuHighlight,
                                                SourceParameters = Enums.SourceVolume.Volume
                                            };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);

                    pgrdIndicators.EndEdit();
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region TP

                if (e.Node.Text.Contains("TP") || e.Node.Text.Contains("Typical Price") || e.Node.Text.Contains("PM"))
                {
                    TPInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new TPInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            ColorParameters = indicatorMock.ColorParameters,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new TPInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 1,
                            ColorParameters = Color.DarkGray,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }


                #endregion

                #region PSAR

                if (e.Node.Text.Contains("PSAR"))
                {
                    PSARInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new PSARInfo
                        {
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            MaxAF = indicatorMock.MaxAf,
                            MinAF = indicatorMock.MinAf,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new PSARInfo
                        {
                            LineThicknessParameters = 1,
                            ColorParameters = Color.Red,
                            MaxAF = 0.2,
                            MinAF = 0.02,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }
                #endregion

                #region SMI

                if (e.Node.Text.Contains("SMI") || e.Node.Text.Contains("ME") && !(e.Node.Text.Contains("MME")))
                {
                    SMIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new SMIInfo
                        {
                            KPeriods = indicatorMock.KPeriodsParameters,
                            DPeriods = indicatorMock.DPeriodsParameters,
                            Type = indicatorMock.Type,
                            DType = indicatorMock.DType,
                            KDblSmooth = indicatorMock.KDblSmooth,
                            Smooth = indicatorMock.Smooth,
                            ColorAverage = indicatorMock.ColorAverage,
                            ColorValue = indicatorMock.ColorValue,
                            LineStyleAverage = indicatorMock.LineStyleAverage,
                            LineThicknessAverage = indicatorMock.LineThicknessAverage,
                            LineStyleValue = indicatorMock.LineStyleValue,
                            LineThicknessValue = indicatorMock.LineThicknessValue
                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new SMIInfo
                        {

                            KPeriods = 13,
                            DPeriods = 9,
                            Type = Enums.Type.Exponencial,
                            DType = Enums.Type.Exponencial,
                            Smooth = 25,
                            KDblSmooth = 2,
                            ColorAverage = Color.Red,
                            ColorValue = Color.Blue,
                            LineStyleAverage = Enums.LineStyle.Normal,
                            LineThicknessAverage = 2,
                            LineStyleValue = Enums.LineStyle.Normal,
                            LineThicknessValue = 2
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }
                #endregion

                #region SO

                if (e.Node.Text.Contains("SO") || e.Node.Text.Contains("OE"))
                {
                    SOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                        e.Node.Value.ToString());

                        indicatorInfo = new SOInfo
                        {
                            KPeriods = indicatorMock.KPeriodsParameters,
                            DPeriods = indicatorMock.DPeriodsParameters,
                            KSlowingParameters = indicatorMock.KSlowingParameters,
                            Type = indicatorMock.Type,
                            ColorAverage = indicatorMock.ColorAverage,
                            ColorValue = indicatorMock.ColorValue,
                            LineStyleAverage = indicatorMock.LineStyleAverage,
                            LineThicknessAverage = indicatorMock.LineThicknessAverage,
                            LineStyleValue = indicatorMock.LineStyleValue,
                            LineThicknessValue = indicatorMock.LineThicknessValue
                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new SOInfo
                        {

                            KPeriods = 14,
                            DPeriods = 9,
                            Type = Enums.Type.Exponencial,
                            KSlowingParameters = 3,
                            ColorAverage = Color.Green,
                            ColorValue = Color.Red,
                            LineStyleAverage = Enums.LineStyle.Normal,
                            LineThicknessAverage = 2,
                            LineStyleValue = Enums.LineStyle.Normal,
                            LineThicknessValue = 2
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }
                #endregion

                #region Aroon

                else if (e.Node.Text.Contains("Aroon") && !e.Node.Text.Contains("AO"))
                {
                    AroonInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new AroonInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineStyle2Parameters = indicatorMock.LineStyle2Parameters,
                            LineThickness2Parameters = indicatorMock.LineThickness2Parameters,
                            Color2Parameters = indicatorMock.Color2Parameters,
                            Periods = indicatorMock.Periods,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new AroonInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 2,
                            ColorParameters = Color.Red,
                            LineStyle2Parameters = Enums.LineStyle.Normal,
                            LineThickness2Parameters = 2,
                            Color2Parameters = Color.Blue,
                            Periods = 14,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region Aroon Oscillator

                else if (e.Node.Text.Contains("AO"))
                {
                    AOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new AOInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Periods = indicatorMock.Periods,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new AOInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            Periods = 14,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region CMF

                else if (e.Node.Text.Contains("CMF"))
                {
                    CMFInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new CMFInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Periods = indicatorMock.Periods,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new CMFInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            Periods = 14,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region EOM

                else if (e.Node.Text.Contains("EOM"))
                {
                    EOMInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new EOMInfo
                        {
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Periods = indicatorMock.Periods,
                            Type = indicatorMock.Type

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new EOMInfo
                        {
                            LineThicknessParameters = 1,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            Periods = 14,
                            Type = Enums.Type.Simples

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region MFI

                else if (e.Node.Text.Contains("MFI") || e.Node.Text.Contains("IFD"))
                {
                    MFIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new MFIInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Periods = indicatorMock.Periods,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MFIInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 1,
                            ColorParameters = Color.Red,
                            Periods = 14,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region ASI

                else if (e.Node.Text.Contains("ASI") || e.Node.Text.Contains("IBA"))
                {
                    ASIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new ASIInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LimitMoveValue = indicatorMock.LimitMoveValue,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new ASIInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LimitMoveValue = 14,
                            LineThicknessParameters = 1,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region CV

                else if (e.Node.Text.Contains("CV") || e.Node.Text.Contains("VC"))
                {
                    CVInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new CVInfo
                        {
                            Periods = indicatorMock.Periods,
                            RatOfChg = indicatorMock.RatOfChg,
                            Type = indicatorMock.Type,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new CVInfo
                        {
                            Periods = 14,
                            RatOfChg = 2,
                            Type = Enums.Type.Simples,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region CMO

                else if (e.Node.Text.Contains("CMO"))
                {
                    CMOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new CMOInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new CMOInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region WWS

                else if (e.Node.Text.Contains("WWS") || e.Node.Text.Contains("AWW"))
                {
                    WWSInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new WWSInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new WWSInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region VHF

                else if (e.Node.Text.Contains("VHF") || e.Node.Text.Contains("FVH"))
                {
                    VHFInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new VHFInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new VHFInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region TRIX

                else if (e.Node.Text.Contains("TRIX"))
                {
                    TRIXInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new TRIXInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new TRIXInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region LRRS

                else if (e.Node.Text.Contains("LRRS") || e.Node.Text.Contains("RLRR"))
                {
                    LRRSInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new LRRSInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new LRRSInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region LRF

                else if (e.Node.Text.Contains("LRF") || e.Node.Text.Contains("PRL"))
                {
                    LRFInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new LRFInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new LRFInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region LRS

                else if (e.Node.Text.Contains("LRS") || e.Node.Text.Contains("IRL"))
                {
                    LRSInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new LRSInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new LRSInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region LRI

                else if (e.Node.Text.Contains("LRI") || e.Node.Text.Contains("RLI"))
                {
                    LRSInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new LRSInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new LRSInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region     MO

                else if (e.Node.Text.Contains("MO"))
                {
                    LRSInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new LRSInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceParameters = indicatorMock.SourceParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new LRSInfo
                        {
                            Periods = 14,
                            SourceParameters = Enums.Source.Fechamento,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region ADX

                else if (e.Node.Text.Contains("ADX") && !e.Node.Text.Contains("DI+/DI-"))
                {
                    ADXInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new ADXInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new ADXInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region DI

                else if (e.Node.Text.Contains("DI+/DI-") && !e.Node.Text.Contains("ADX"))
                {
                    DIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new DIInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            Color2Parameters = indicatorMock.Color2Parameters,
                            LineStyle2Parameters = indicatorMock.LineStyle2Parameters,
                            LineThickness2Parameters = indicatorMock.LineThickness2Parameters
                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new DIInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                            Color2Parameters = Color.Blue,
                            LineStyle2Parameters = Enums.LineStyle.Normal,
                            LineThickness2Parameters = 2
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region DI ADX

                else if (e.Node.Text.Contains("DI+/DI-/ADX"))
                {
                    ADXDIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new ADXDIInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            Color2Parameters = indicatorMock.Color2Parameters,
                            LineThickness2Parameters = indicatorMock.LineThickness2Parameters,
                            LineStyle2Parameters = indicatorMock.LineStyle2Parameters,
                            ColorAverage = indicatorMock.ColorAverage,
                            LineThicknessAverage = indicatorMock.LineThicknessAverage,
                            LineStyleAverage = indicatorMock.LineStyleAverage,
                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new ADXDIInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                            Color2Parameters = Color.Blue,
                            LineThickness2Parameters = 2,
                            LineStyle2Parameters = Enums.LineStyle.Normal,
                            ColorAverage = Color.Green,
                            LineThicknessAverage = 1,
                            LineStyleAverage = Enums.LineStyle.Pontilhados,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region CCI

                else if (e.Node.Text.Contains("CCI"))
                {
                    CCIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new CCIInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new CCIInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region DPO

                else if (e.Node.Text.Contains("DPO"))
                {
                    DPOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new DPOInfo
                        {
                            Periods = indicatorMock.Periods,
                            Type = indicatorMock.Type,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new DPOInfo
                        {
                            Periods = 14,
                            Type = Enums.Type.Simples,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region MI

                else if ((e.Node.Text.Contains("MI") || e.Node.Text.Contains("IM")) && !e.Node.Text.Contains("SMI"))
                {
                    MIInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new MIInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new MIInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region OBV

                else if (e.Node.Text.Contains("OBV") || e.Node.Text.Contains("VES"))
                {
                    OBVInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new OBVInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new OBVInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region PROC

                else if (e.Node.Text.Contains("PROC"))
                {
                    PROCInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new PROCInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new PROCInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region PVT

                else if (e.Node.Text.Contains("PVT") || e.Node.Text.Contains("TVP"))
                {
                    PVTInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new PVTInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new PVTInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region WilliamsR

                else if (e.Node.Text.Contains("Williams") && !e.Node.Text.Contains("WAD"))
                {
                    WilliamsRInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new WilliamsRInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new WilliamsRInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region WAD

                else if (e.Node.Text.Contains("WAD"))
                {
                    WADInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new WADInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new WADInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region WC

                else if (e.Node.Text.Contains("WC"))
                {
                    WCInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new WCInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new WCInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region VROC

                else if (e.Node.Text.Contains("VROC"))
                {
                    VROCInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new VROCInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new VROCInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region FCO

                else if (e.Node.Text.Contains("FCO"))
                {
                    FCOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new FCOInfo
                        {
                            Periods = indicatorMock.Periods,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new FCOInfo
                        {
                            Periods = 14,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region FCB

                else if (e.Node.Text.Contains("FCB"))
                {
                    FCBInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new FCBInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Periods = indicatorMock.Periods,

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new FCBInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 1,
                            ColorParameters = Color.Red,
                            Periods = 14,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region UO

                else if (e.Node.Text.Contains("UO"))
                {
                    UOInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new UOInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            Cycle1 = indicatorMock.Cycle1,
                            Cycle2 = indicatorMock.Cycle2,
                            Cycle3 = indicatorMock.Cycle3,
                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new UOInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            LineThicknessParameters = 2,
                            ColorParameters = Color.Red,
                            Cycle1 = 7,
                            Cycle2 = 14,
                            Cycle3 = 28,

                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region TR

                else if (e.Node.Text.Contains("TR"))
                {
                    TRInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new TRInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new TRInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region RO

                else if (e.Node.Text.Contains("RO"))
                {
                    ROInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new ROInfo
                        {
                            Levels = indicatorMock.Levels,
                            Type = indicatorMock.Type,
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            SourceAverageOHLC = indicatorMock.SourceAverageOHLC,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new ROInfo
                        {
                            Levels = 9,
                            Type = Enums.Type.Simples,
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 1,
                            SourceAverageOHLC = Enums.SourceOHLC.Fechamento,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                #region A/D

                else if (e.Node.Text.Contains("A/D"))
                {
                    ADInfo indicatorInfo;

                    if (e.Node.Parent != null)
                    {
                        IndicatorMock indicatorMock = _mock.GetIndicatorMockByCodeMock(e.Node.Parent.Value.ToString(),
                                                                                       e.Node.Value.ToString());

                        indicatorInfo = new ADInfo
                        {
                            LineStyleParameters = indicatorMock.LineStyleParameters,
                            ColorParameters = indicatorMock.ColorParameters,
                            LineThicknessParameters = indicatorMock.LineThicknessParameters

                        };

                        //foreach (PropertyGridItem item in pgrdIndicators.Items)
                        //CustomizeGrid(e.Node.Parent.Value.ToString(), item);
                    }
                    else
                    {
                        indicatorInfo = new ADInfo
                        {
                            LineStyleParameters = Enums.LineStyle.Normal,
                            ColorParameters = Color.Red,
                            LineThicknessParameters = 2,
                        };

                    }
                    btnApply.Enabled = (e.Node.Parent != null);
                    btnRemove.Enabled = (e.Node.Parent != null);
                    pgrdIndicators.SelectedObject = indicatorInfo;
                }

                #endregion

                pgrdIndicators.PropertyGridElement.SplitElement.PropertyTableElement.ValueColumnWidth = 87;
                LoadItemsDictionary();
            }
            catch (Exception ex)
            { }
            trvIndicators.BringIntoView(e.Node);
            trvIndicators.Refresh();
            e.Node.EnsureVisible();
            trvIndicators.Refresh();
            


        }

        #endregion

        #region Button Events

        private void BtnDismissClick(object sender, EventArgs e)
        {
            StockChartX1.UnSelect();
            Close();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            string parentIndicator;

            if (Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgRemoveIndicatorMock"], "", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                return;

            //Telerik.WinControls.RadMessageBox.Show(trvIndicators.SelectedNode.Value.ToString() + " " + trvIndicators.SelectedNode.Parent.Value.ToString());
            ParentForm.StockChartX1_RemoveSeries(trvIndicators.SelectedNode.Value.ToString());
            StockChartX1.ForcePaint();
            parentIndicator = trvIndicators.SelectedNode.Parent.Value.ToString();
            _mock.RemoveIndicatorMock(_mock.GetIndicatorMockByCodeMock(trvIndicators.SelectedNode.Parent.Value.ToString(), trvIndicators.SelectedNode.Value.ToString()));
            LoadIndicators(parentIndicator);
        }

        private void BtnNewClick(object sender, EventArgs e)
        {
            if (trvIndicators.SelectedNode == null)
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgSelectIndicator"]);
                return;
            }

            if (InsertTechnicalAnalysis())
            {
                pgrdIndicators.EndEdit();
                AddIndicatorTreeView();
                StockChartX1.LoadUserStudyLine(-1);
            }
            else
            {
                pgrdIndicators.EndEdit();
                return;
            }

            if (trvIndicators.SelectedNode.Parent == null)
                trvIndicators.Nodes[trvIndicators.SelectedNode.Name].LastNode.Selected = true;
            else
                trvIndicators.Nodes[trvIndicators.SelectedNode.Parent.Name].LastNode.Selected = true;
        }

        private void AddIndicatorTreeView()
        {
            Mock.Indicator indicatorSelected = trvIndicators.SelectedNode.Parent == null ? _mock.GetIndicatorByCode(trvIndicators.SelectedNode.Value.ToString()) :
                  _mock.GetIndicatorByCode(trvIndicators.SelectedNode.Parent.Value.ToString());
            IndicatorMock indicatorMock;
            string sourceCode2;
            string sourceCode;
            switch (indicatorSelected.Code)
            {
                #region BB
                case "BB":
                    sourceCode = pgrdIndicators.Items[2].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[1].Value + "," + sourceCode[0] + sourceCode[1],
                        Type = (Enums.Type)pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[2].Value,
                        StandardDev = int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[4].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region HV
                case "HV":
                    sourceCode = pgrdIndicators.Items[2].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "VH" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[1].Value + "," + sourceCode[0] + sourceCode[1],
                        Type = (Enums.Type)pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[2].Value,
                        StandardDev = int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[4].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region HILO
                case "HILO":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[1].Value + "," + pgrdIndicators.Items[2].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        PeriodsParameters = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        ShiftParameters = int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                        ScaleAverage = double.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[4].Value,
                        Color2Parameters = (Color)pgrdIndicators.Items[5].Value,
                        LineStyleParameters = 
                            (Enums.LineStyle)pgrdIndicators.Items[6].Value,
                        LineThicknessParameters = 
                            int.Parse(pgrdIndicators.Items[7].Value.ToString())
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MA
                case "MA":
                    sourceCode = pgrdIndicators.Items[2].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "MM" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value+ "," +
                            pgrdIndicators.Items[1].Value + "," + sourceCode[0] + sourceCode[1],
                        Type = (Enums.Type)pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        SourceAverage = (Enums.Source)pgrdIndicators.Items[2].Value,
                        ShiftParameters = int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ScaleAverage = double.Parse(pgrdIndicators.Items[4].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[5].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[6].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[7].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MAE
                case "MAE":
                    sourceCode = pgrdIndicators.Items[2].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "MME" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[1].Value + "," + sourceCode[0] + sourceCode[1],
                        Type = (Enums.Type)pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[2].Value,
                        ScaleAverage = double.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[4].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MACD
                case "MACD":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " +
                            pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[4].Value + "," +
                            pgrdIndicators.Items[5].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleAverage = (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessAverage = int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        ShortCycle = int.Parse(pgrdIndicators.Items[4].Value.ToString()),
                        LongCycle = int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                        ColorValue = (Color)pgrdIndicators.Items[6].Value,
                        LineStyleValue = (Enums.LineStyle)pgrdIndicators.Items[7].Value,
                        LineThicknessValue = int.Parse(pgrdIndicators.Items[8].Value.ToString())
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MACD-H
                case "MACD-H":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " +
                            pgrdIndicators.Items[0].Value + "," +
                            pgrdIndicators.Items[1].Value + "," +
                            pgrdIndicators.Items[2].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ShortCycle = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        LongCycle = int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                        ColorHistogram = (Color)pgrdIndicators.Items[3].Value
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region RSI
                case "RSI":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "IFR" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceAverage = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorAverage = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString()),
                        Threshold1 = int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                        Threshold2 = int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                        ColorT = (Color)pgrdIndicators.Items[7].Value,
                        LineStyleT =
                            (Enums.LineStyle)pgrdIndicators.Items[8].Value,
                        LineThicknessT =
                            int.Parse(pgrdIndicators.Items[9].Value.ToString())
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region VOL
                case "VOL":
                    sourceCode = pgrdIndicators.Items[0].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey + "," + sourceCode[0] + sourceCode[1],
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        SourceParameters = (Enums.Source) pgrdIndicators.Items[0].Value
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region TP
                case "TP":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "PM" : indicatorSelected.Code) +
                            indicatorKey,
                        ColorParameters = (Color)pgrdIndicators.Items[0].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[1].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[2].Value.ToString()),

                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region PSAR
                case "PSAR":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + " ; " + pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        MinAf =
                            double.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        MaxAf =
                            double.Parse(pgrdIndicators.Items[1].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region SMI
                case "SMI":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "ME" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[3].Value + "," + pgrdIndicators.Items[0].Value + "," + pgrdIndicators.Items[8].Value +","+ pgrdIndicators.Items[7].Value,
                        Type = (Enums.Type)pgrdIndicators.Items[3].Value,
                        KPeriodsParameters = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        Smooth = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[4].Value,
                        LineStyleValue = 
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThicknessValue = 
                        int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                        DPeriodsParameters = int.Parse(pgrdIndicators.Items[7].Value.ToString()),
                        DType = (Enums.Type)pgrdIndicators.Items[8].Value,
                        KDblSmooth = int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                        ColorValue = (Color)pgrdIndicators.Items[9].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[10].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[11].Value.ToString()),
                    };
                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region SO

                case "SO":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "OE" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[5].Value + "," + pgrdIndicators.Items[6].Value + "," + pgrdIndicators.Items[0].Value + "," + pgrdIndicators.Items[1].Value,

                        //%K Series parameters
                        KPeriodsParameters = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        KSlowingParameters = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        ColorValue = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleValue =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessValue =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString()),

                        //%D Series parameters
                        DPeriodsParameters = int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                        Type = (Enums.Type)pgrdIndicators.Items[6].Value,
                        ColorAverage = (Color)pgrdIndicators.Items[7].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[8].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[9].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region Aroon

                case "Aroon":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,

                        //Up parameters
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters = 
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters = 
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        //Down parameters
                        Color2Parameters = (Color)pgrdIndicators.Items[4].Value,
                        LineStyle2Parameters =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThickness2Parameters =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region Aroon Oscillator

                case "AO":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region CMF

                case "CMF":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region EOM

                case "EOM":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[1].Value + "," + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters = 
                            int.Parse(pgrdIndicators.Items[4].Value.ToString()),
                        Type = 
                            (Enums.Type)pgrdIndicators.Items[1].Value
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MFI

                case "MFI":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "IFD" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters = 
                            int.Parse(pgrdIndicators.Items[3].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region ASI

                case "ASI":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "IBA" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        LimitMoveValue = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region CV

                case "CV":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "VC" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[2].Value.ToString() + ","+ pgrdIndicators.Items[0].Value + "," + pgrdIndicators.Items[1].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        RatOfChg = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        Type = (Enums.Type)pgrdIndicators.Items[2].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[3].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[4].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[5].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region CMO

                case "CMO":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region WWS

                case "WWS":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "AWW" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region VHF
                case "VHF":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "FVH" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region TRIX
                case "TRIX":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region LRRS
                case "LRRS":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "RLRR" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region LRF
                case "LRF":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "PRL" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region LRS
                case "LRS":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "IRL" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region LRI
                case "LRI":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "RLI" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MO
                case "MO":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceParameters = (Enums.Source)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region ADX
                case "ADX":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region DI
                case "DI+/DI-":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        Color2Parameters = (Color)pgrdIndicators.Items[4].Value,
                        LineStyle2Parameters =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThickness2Parameters =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region DI ADX

                case "DI+/DI-/ADX":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        Color2Parameters = (Color)pgrdIndicators.Items[4].Value,
                        LineStyle2Parameters =
                            (Enums.LineStyle)pgrdIndicators.Items[5].Value,
                        LineThickness2Parameters =
                            int.Parse(pgrdIndicators.Items[6].Value.ToString()),
                        ColorAverage = (Color)pgrdIndicators.Items[7].Value,
                        LineStyleAverage =
                            (Enums.LineStyle)pgrdIndicators.Items[8].Value,
                        LineThicknessAverage =
                            int.Parse(pgrdIndicators.Items[9].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region CCI

                case "CCI":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString())
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region DPO

                case "DPO":
                    sourceCode = pgrdIndicators.Items[2].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[1].Value + "," + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[3].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[4].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                        Type =
                            (Enums.Type)pgrdIndicators.Items[1].Value,
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[2].Value,
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region MI

                case "MI":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "IM" : indicatorSelected.Code) +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region OBV

                case "OBV":
                    sourceCode = pgrdIndicators.Items[0].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                             ( indicatorSelected.Code) +
                            indicatorKey + " " + sourceCode,
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[0].Value,
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region PROC

                case "PROC":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region PVT

                case "PVT":
                    sourceCode = pgrdIndicators.Items[0].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            (Program.LanguageStockChartX == 1 ? "TVP" : indicatorSelected.Code) + indicatorKey + " " + sourceCode[0] + sourceCode[1],
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[0].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region WilliamsR

                case "Williams":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region WAD

                case "WAD":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey,
                        ColorParameters = (Color)pgrdIndicators.Items[0].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[1].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region WC

                case "WC":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey,
                        ColorParameters = (Color)pgrdIndicators.Items[0].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[1].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region VROC

                case "VROC":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + sourceCode[0] + sourceCode[1],
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[1].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[2].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[3].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[4].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region FCO

                case "FCO":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region FCB

                case "FCB":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value,
                        Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[1].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[2].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[3].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region UO

                case "UO":

                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value
                            + "," + pgrdIndicators.Items[1].Value
                            + "," + pgrdIndicators.Items[2].Value,
                        Cycle1 = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        Cycle2 = int.Parse(pgrdIndicators.Items[1].Value.ToString()),
                        Cycle3 = int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                        ColorParameters = (Color)pgrdIndicators.Items[3].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[4].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region TR

                case "TR":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey,
                        ColorParameters = (Color)pgrdIndicators.Items[0].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[1].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region RO

                case "RO":
                    sourceCode = pgrdIndicators.Items[1].Value.ToString();
                    sourceCode = sourceCode.ToUpper();
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code +
                            indicatorKey + " " + pgrdIndicators.Items[0].Value + "," + pgrdIndicators.Items[2].Value + "," + sourceCode[0] + sourceCode[1],
                        Levels = int.Parse(pgrdIndicators.Items[0].Value.ToString()),
                        SourceAverageOHLC = (Enums.SourceOHLC)pgrdIndicators.Items[1].Value,
                        Type =
                            (Enums.Type)pgrdIndicators.Items[2].Value,
                        ColorParameters = (Color)pgrdIndicators.Items[3].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[4].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[5].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

                #region A/D

                case "A/D":
                    indicatorMock = new IndicatorMock
                    {
                        Code = indicatorSelected.Code,
                        CodeMock =
                            indicatorSelected.Code +
                            indicatorKey,
                        Description =
                            indicatorSelected.Code + indicatorKey,
                        ColorParameters = (Color)pgrdIndicators.Items[0].Value,
                        LineStyleParameters =
                            (Enums.LineStyle)pgrdIndicators.Items[1].Value,
                        LineThicknessParameters =
                            int.Parse(pgrdIndicators.Items[2].Value.ToString()),
                    };

                    _mock.AddIndicatorMock(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                    trvIndicators.SelectedNode.Parent.Value.ToString(), indicatorMock);
                    LoadIndicators(indicatorMock.CodeMock);
                    break;
                #endregion

            }

        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            string currentSelect;
            if ((trvIndicators.SelectedNode == null) || (trvIndicators.SelectedNode.Parent == null))
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgSelectIndicator"]);
                return;
            }

            currentSelect = trvIndicators.SelectedNode.Value.ToString();

            //Telerik.WinControls.RadMessageBox.Show("VM");
            pgrdIndicators.EndEdit();
            if (!UpdateTechnicalAnalysis(currentSelect)) return;
            GetStockChartIndicators();

            LoadIndicators(currentSelect);

            //Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgIndicatorChanged"]);


        }

        //Returns TRUE if a series is an OHLC overlay
        private static bool IsOverlay(string code)
        {
            string[] overlays = new[]
                             {
                               "BB","HILO","MA","MAE", "PSAR", "FCB", "WC"
                             };
            return overlays.Any(overlay => code == overlay);
        }

        private bool InsertTechnicalAnalysis()
        {
            Indicator item = trvIndicators.SelectedNode.Parent == null ? _mock.GetIndicatorByCode(trvIndicators.SelectedNode.Value.ToString()) :
                _mock.GetIndicatorByCode(trvIndicators.SelectedNode.Parent.Value.ToString());
            if (StockChartX1.RecordCount < 3)
                return false;
            //Params Default:
            int panel = IsOverlay(item.Code) ? 0 : StockChartX1.AddChartPanel();
            if(panel == -1)
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionarySelectIndicator["ErrorPanel"]);
                return false;
            }
            string Window = "";
            string Symbol = StockChartX1.Symbol;
            string Source1;
            Color lineColor = Color.Red;
            Color lineColor2 = Color.Red;
            Color lineColor3 = Color.Red;
            int lineThickness = 1;
            int lineThickness2 = 1;
            int lineThickness3 = 1;
            int lineStyleInt2 = 0;
            int lineStyleInt3 = 0;
            int lineStyleInt = 0;
            bool errorKey = true;
            indicatorKey = 1;
            //Others params:
            int Periods, PeriodsParameters;
            double LimitMove;
            double StandardDev;
            int Smooth;
            int MAType = 0;
            int RateChange;
            string Source2;
            int BarHistory;
            int ShortCycle;
            int LongCycle;
            double Scale;
            int Shift;
            double MinAF;
            double MaxAF;
            int Levels;
            int KPeriods;
            int KSlowing;
            int DPeriods;
            int DMAType = 0;
            int KDblSmooth;
            double MinimumTick;
            int Cycle1;
            int Cycle2;
            int Cycle3;
            int ShortTerm;
            int LongTerm;
            int PointPercent;
            switch (item.Code)
            {
                #region BB
                case "BB":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    StandardDev = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorBollinger(item.Code + indicatorKey, panel, Source1,
                                                  Periods, StandardDev, MAType, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region HV
                case "HV":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    StandardDev = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorHistoricalVolatility(item.Code+indicatorKey,panel,Source1,Periods,MAType,2.0,lineColor.R,lineColor.G,lineColor.B,lineStyleInt,lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region HILO
                case "HILO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    PeriodsParameters = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Shift = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    Scale = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineColor2 = (Color)pgrdIndicators.Items[5].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[7].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorHILO(item.Code + indicatorKey, panel, Symbol,
                                                                              Periods, PeriodsParameters, Shift, Scale, (uint)ColorTranslator.ToOle(lineColor), (uint)ColorTranslator.ToOle(lineColor2), lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MA
                case "MA":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    Shift = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    Scale = double.Parse(pgrdIndicators.Items[4].Value.ToString());
                    lineColor = (Color) pgrdIndicators.Items[5].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[7].Value.ToString());
                    
                    string serieN;
                    //If source=volume, add on Volume Panel
                    if (pgrdIndicators.Items[2].Value.ToString() == "Volume")
                    {
                        int selectedNode = trvIndicators.SelectedNode.Index;
                        if (lineThickness < 2) lineThickness = 2;
                        for (int i = 0; i < StockChartX1.SeriesCount; i++)
                        {
                            serieN = StockChartX1.get_SeriesName(i).ToLower();
                            if (!(serieN.Contains(".volume")) &&
                                serieN.Contains("vol"))
                                panel = StockChartX1.GetPanelBySeriesName(StockChartX1.get_SeriesName(i));
                        }
                        if(panel==0)
                        {
                            panel = StockChartX1.AddChartPanel();
                            StockChartX1.AddIndicatorVolume("VOL1", panel, Symbol+".Volume", 128, 128,
                                            128, 0, 0);
                            //Update MOCK:
                            //trvIndicators.SelectedNode = trvIndicators.Nodes[6];

                            //AddIndicatorTreeView();
                            StockChartX1.LoadUserStudyLine(-1);
                            IndicatorMock indicatorMock = new IndicatorMock
                            {
                                Code = "VOL",
                                CodeMock =
                                    "VOL1",
                                Description =
                                    "VOL1" + "," + "VO",
                                ColorParameters = Color.Gray,
                                SourceParameters = Enums.Source.Volume
                            };
                            _mock.AddIndicatorMock("VOL", indicatorMock);
                            LoadIndicators(indicatorMock.CodeMock);
                            trvIndicators.SelectedNode = trvIndicators.Nodes[selectedNode];
                        }
                    }
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                           
                        try
                        {
                            StockChartX1.AddIndicatorGenericMovingAverage(item.Code + indicatorKey, panel, Source1,
                                                                              Periods, Shift, MAType, Scale, lineColor.R,
                                                                              lineColor.G,
                                                                              lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MAE
                case "MAE":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    Scale = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorMAEnvelope(item.Code + indicatorKey, panel, Source1,
                                                  Periods, MAType, Scale, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MACD
                case "MACD":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color) pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    ShortCycle = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    LongCycle = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[6].Value;
                    lineStyleInt2 = 0;
                    switch (pgrdIndicators.Items[7].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[8].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorMACD(item.Code + indicatorKey, panel, Symbol,
                                                  Periods, 7, ShortCycle, LongCycle, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G,
                                                  lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MACD-H
                case "MACD-H":
                    /*foreach (PropertyGridItem pgItem in pgrdIndicators.Items)
                    {
                        Telerik.WinControls.RadMessageBox.Show("Index=" + pgrdIndicators.Items.IndexOf(pgItem) + "\nValue=" + pgItem.Value);
                    }*/
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    ShortCycle = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    LongCycle = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorMACDHistogram(item.Code + indicatorKey, panel, Symbol,
                                                  LongCycle, ShortCycle, Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, 0);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region RSI

                case "RSI":
                    //Params:
                    int  TStyle, TThickness;
                    double Threshold1, Threshold2;
                    Color TColor;
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    Threshold1 = double.Parse(pgrdIndicators.Items[5].Value.ToString());
                    Threshold2 = double.Parse(pgrdIndicators.Items[6].Value.ToString());
                    TColor = (Color)pgrdIndicators.Items[7].Value;
                    TStyle = 0;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            TStyle = 0;
                            break;
                        case "Tracejados":
                            TStyle = 1;
                            break;
                        case "Pontilhados":
                            TStyle = 2;
                            break;
                    }
                    TThickness = int.Parse(pgrdIndicators.Items[9].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorRelativeStrenght(item.Code + indicatorKey, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, Threshold1, Threshold2, TColor.R, TColor.G,TColor.B, TStyle, TThickness);
                            StockChartX1.SetYScale(panel, 100, 0);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region VOL
                case "VOL":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;


                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorVolume(item.Code + indicatorKey, panel, Source1, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, 0);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region TP
                case "TP":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorTypicalPrice(item.Code + indicatorKey, panel, Symbol, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region PSAR
                case "PSAR":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    MinAF = double.Parse(pgrdIndicators.Items[0].Value.ToString());
                    MaxAF = double.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.AddIndicatorParabolicSAR(item.Code + indicatorKey, panel, Symbol, MinAF, MaxAF, lineColor.R, lineColor.G,
                                                  lineColor.B, 0, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region SMI
                case "SMI":
                    //Params:
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    KPeriods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Smooth = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    DPeriods = int.Parse(pgrdIndicators.Items[7].Value.ToString());
                    KDblSmooth = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Simples":
                            DMAType = 0;
                            break;
                        case "Exponencial":
                            DMAType = 1;
                            break;
                        case "TimeSeries":
                            DMAType = 2;
                            break;
                        case "Triangular":
                            DMAType = 3;
                            break;
                        case "Variável":
                            DMAType = 4;
                            break;
                        case "VIDYA":
                            DMAType = 5;
                            break;
                        case "Weighted":
                            DMAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[9].Value;
                    lineStyleInt2 = 0;
                    switch (pgrdIndicators.Items[10].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[11].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorStocMomentum(item.Code + indicatorKey, panel, Symbol,
                                                                              KPeriods, Smooth, KDblSmooth, DPeriods, MAType, DMAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region SO
                case "SO":
                    //Params:
                    KPeriods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    KSlowing = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    
                    //%D series

                    DPeriods = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[7].Value;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[9].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorStocOscillator(item.Code + indicatorKey, panel, Symbol,
                                                                              KPeriods, KSlowing, DPeriods, MAType,  lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region Aroon
                case "Aroon":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Down
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorAroon(item.Code + indicatorKey, panel, Symbol,
                                                                              Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region Aroon Oscillator
                case "AO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorAroonOsc(item.Code + indicatorKey, panel, Symbol,
                                                                              Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            StockChartX1.SetYScale(panel, 100, -100);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CMF
                case "CMF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorChaikinMoney(item.Code + indicatorKey, panel, Symbol, Symbol+".volume",
                                                                              Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region EOM
                case "EOM":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorEasyMovement(item.Code + indicatorKey, panel, Symbol, Symbol+".volume", Periods, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region MFI
                case "MFI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorMoneyFlow(item.Code + indicatorKey, panel, Symbol, Symbol + ".volume",
                                                                              Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region ASI
                case "ASI":
                    //Params:
                    LimitMove = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorASI(item.Code + indicatorKey, panel, Symbol,LimitMove, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CV
                case "CV":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    RateChange = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorChaikinVolatility(item.Code + indicatorKey, panel, Symbol, Periods, RateChange, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CMO
                case "CMO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorChandeMomentum(item.Code + indicatorKey, panel, Source1 , Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region WWS
                case "WWS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorWellesWilder(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region VHF
                case "VHF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorVertiHoriFilter(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region TRIX
                case "TRIX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorTRIX(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region LRRS
                case "LRRS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorLinearRegRSquare(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region LRF
                case "LRF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorLinearRegForecast(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region LRS
                case "LRS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorLinearRegSlope(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region LRS
                case "LRI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorLinearRegIntercept(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region MO
                case "MO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorMomentum(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region ADX
                case "ADX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorADX(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt,lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region DI
                case "DI+/DI-":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorDI(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region DI ADX
                case "DI+/DI-/ADX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    lineColor3 = (Color)pgrdIndicators.Items[7].Value;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt3 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt3 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt3 = 2;
                            break;
                    }
                    lineThickness3 = int.Parse(pgrdIndicators.Items[9].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorDirectMoveSystem(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2, lineColor3.R, lineColor3.G, lineColor3.B, lineStyleInt3, lineThickness3);

                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CCI
                case "CCI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorCommodityChannel(item.Code + indicatorKey, panel,Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                //if (ex.Message.Contains("Invalid Period"))
                                //{
                                //    string parentIndicator;
                                //    ParentForm.StockChartX1_RemoveSeries(item.Code);
                                //    StockChartX1.ForcePaint();
                                //    parentIndicator = item.Code;
                                //    _mock.RemoveIndicatorMock(_mock.GetIndicatorMockByCodeMock(item.Code, item.Code));
                                //    LoadIndicators(parentIndicator);
                                //}
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region DPO
                case "DPO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorDetrendedPrice(item.Code + indicatorKey, panel, Source1, Periods, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region MI
                case "MI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorMassIndex(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region OBV
                case "OBV":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorBalanceVolume(item.Code + indicatorKey, panel, Source1, Symbol+".volume", lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region PROC
                case "PROC":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorPriceROC(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region PVT
                case "PVT":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorPriceVolume(item.Code + indicatorKey, panel, Source1, Symbol + ".volume", lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region WilliamsR
                case "Williams":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorWilliamPCTR(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region WAD
                case "WAD":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorWilliamAD(item.Code + indicatorKey, panel, Symbol,lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region WC
                case "WC":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorWeightedClose(item.Code + indicatorKey, panel, Symbol, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region VROC
                case "VROC":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorVolumeROC(item.Code + indicatorKey, panel, Source1, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region FCO
                case "FCO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorFractalChaosOsc(item.Code + indicatorKey, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            StockChartX1.SetYScale(panel, 1.2, -1.2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region FCB
                case "FCB":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorFractalChaos(item.Code + indicatorKey, panel, Symbol,
                                                                              Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region UO
                case "UO":
                    //Params:
                    Cycle1 = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Cycle2 = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Cycle3 = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorUltimateOsc(item.Code + indicatorKey, panel, Symbol,
                                                                              Cycle1,Cycle2,Cycle3, lineColor.R, 
                                                                              lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region TR
                case "TR":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorTrueRange(item.Code + indicatorKey, panel, Symbol, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region RO
                case "RO":
                    //Params:
                    Levels = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorRainbowOsc(item.Code + indicatorKey, panel, Source1, Levels, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region A/D
                case "A/D":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.AddIndicatorAccumulationDistribution(item.Code + indicatorKey, panel, Symbol, Symbol+".Volume", lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion



            }
            //StockChartX1.ShowHelp(item.Code + cnt, panel, sourceStr, int.Parse(pgrdIndicators.Items[0].Value.ToString()), lineColor.R, lineColor.G, lineColor.B, lineStyleInt, int.Parse(pgrdIndicators.Items[3].Value.ToString()));
            StockChartX1.Update(0);
            return true;
        }

        private bool UpdateTechnicalAnalysis(string item)
        {
            string Code = Regex.Replace(item, @"\d", "");
            int panel = StockChartX1.GetPanelBySeriesName(item);
            //Params Default:
            string Window = "";
            string Symbol = StockChartX1.Symbol;
            string Source1;
            Color lineColor;
            Color lineColor2;
            Color lineColor3;
            int lineThickness = 1;
            int lineThickness2 = 1;
            int lineThickness3 = 1;
            int lineStyleInt = 0;
            int lineStyleInt2 = 0;
            int lineStyleInt3 = 0;
            bool errorKey = true;
            indicatorKey = 1;
            //Others params:
            int Periods = 0;
            int PeriodsParameters;
            double LimitMove;
            int StandardDev;
            int MAType = 0;
            int Smooth;
            int RateChange;
            string Source2;
            int BarHistory;
            int ShortCycle;
            int LongCycle;
            double Scale;
            int Shift;
            double  MinAF;
            double MaxAF;
            int Levels;
            int KPeriods;
            int KSlowing;
            int DPeriods;
            int DMAType = 0;
            int KDblSmooth;
            double MinimumTick;
            int Cycle1;
            int Cycle2;
            int Cycle3;
            int ShortTerm;
            int LongTerm;
            int PointPercent;
            switch (Code)
            {
                #region BB
                case "BB":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    StandardDev = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorBollinger(item, panel, Source1,0,
                                                  Periods, StandardDev, MAType, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region HV
                case "HV":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    StandardDev = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorHistoricalVolatility(item,panel,Source1,0,Periods,MAType,StandardDev, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region HILO
                case "HILO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    PeriodsParameters = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Shift = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    Scale = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineColor2 = (Color)pgrdIndicators.Items[5].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[7].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorHILO(item, panel, Symbol,
                                                                              Periods, PeriodsParameters, Shift, Scale, (uint)ColorTranslator.ToOle(lineColor), (uint)ColorTranslator.ToOle(lineColor2), lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MA
                case "MA":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    Shift = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    Scale = double.Parse(pgrdIndicators.Items[4].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[5].Value;

                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[7].Value.ToString());
                    string serieN;
                    if ((Symbol + "." + pgrdIndicators.Items[2].Value) != StockChartX1.get_SeriesSource(item, "MA") && (pgrdIndicators.Items[2].Value.ToString() == "Volume" || StockChartX1.get_SeriesSource(item, "MA").Contains("Volume")))
                    {
                        //Remove old indicator
                        ParentForm.StockChartX1_RemoveSeries(trvIndicators.SelectedNode.Value.ToString());
                        //If source changed TO volume, add on Volume Panel
                        if (pgrdIndicators.Items[2].Value.ToString() == "Volume")
                        {
                            if (lineThickness < 2) lineThickness = 2;
                            for (int i = 0; i < StockChartX1.SeriesCount; i++)
                            {
                                serieN = StockChartX1.get_SeriesName(i).ToLower();
                                if (!(serieN.Contains(".volume")) &&
                                    serieN.Contains("vol"))
                                    panel = StockChartX1.GetPanelBySeriesName(StockChartX1.get_SeriesName(i));
                            }
                            if (panel == 0)
                            {
                                panel = StockChartX1.AddChartPanel();
                                StockChartX1.AddIndicatorVolume("VOL1", panel, Symbol + ".Volume", 128, 128,
                                                                128, 0, 0);
                                //Update MOCK:
                                //trvIndicators.SelectedNode = trvIndicators.Nodes[6];

                                //AddIndicatorTreeView();
                                StockChartX1.LoadUserStudyLine(-1);
                                trvIndicators.SelectedNode = trvIndicators.Nodes[4];
                                IndicatorMock indicatorMock = new IndicatorMock
                                                                  {
                                                                      Code = "VOL",
                                                                      CodeMock =
                                                                          "VOL1",
                                                                      Description =
                                                                          "VOL1" + "," + "VO",
                                                                      ColorParameters = Color.Gray,
                                                                      SourceParameters = Enums.Source.Volume
                                                                  };
                                _mock.AddIndicatorMock("VOL", indicatorMock);
                                LoadIndicators(indicatorMock.CodeMock);
                                trvIndicators.SelectedNode = trvIndicators.Nodes[4];
                            }
                        }
                        //If source changed FROM volume, add on Panel 0
                        else 
                        {
                            panel = 0;
                            lineThickness = 1;
                        }
                        //Try to add until available key isnt found:
                        while (errorKey)
                        {

                            try
                            {
                                StockChartX1.AddIndicatorGenericMovingAverage(item, panel, Source1,
                                                                                  Periods, Shift, MAType, Scale, lineColor.R,
                                                                                  lineColor.G,
                                                                                  lineColor.B, lineStyleInt, lineThickness);
                                errorKey = false;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message == "Key not unique")
                                {
                                    errorKey = true;
                                    indicatorKey++;
                                }
                                else
                                {
                                    Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Try to Update until available key isnt found:
                        while (errorKey)
                        {
                            try
                            {
                                StockChartX1.UpdateIndicatorGenericMovingAverage(item, panel, Source1,
                                                                                 Periods, (int) Shift, MAType, Scale,
                                                                                 lineColor.R, lineColor.G,
                                                                                 lineColor.B, lineStyleInt,
                                                                                 lineThickness);
                                errorKey = false;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message == "Key not unique")
                                {
                                    errorKey = true;
                                    indicatorKey++;
                                }
                                else
                                {
                                    Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region MAE
                case "MAE":
                    //Params:
                    switch (pgrdIndicators.Items[0].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Periods = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    Scale = double.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorMAEnvelope(item, panel, Source1,
                                                  Periods, MAType, Scale, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MACD
                case "MACD":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    ShortCycle = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    LongCycle = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[6].Value;
                    lineStyleInt2 = 0;
                    switch (pgrdIndicators.Items[7].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[8].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorMACD(item, panel, Symbol,
                                                  0, Periods, ShortCycle, LongCycle, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G,
                                                  lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MACD-H
                case "MACD-H":
                    /*foreach (PropertyGridItem pgItem in pgrdIndicators.Items)
                    {
                        Telerik.WinControls.RadMessageBox.Show("Index=" + pgrdIndicators.Items.IndexOf(pgItem) + "\nValue=" + pgItem.Value);
                    }*/
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    ShortCycle = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    LongCycle = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorMACDHistogram(item, panel, Symbol, 0,
                                                  LongCycle, ShortCycle, Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, 0);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region RSI
                case "RSI":
                    //Params:
                    int  TStyle, TThickness;
                    double Threshold1, Threshold2;
                    Color TColor;
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    Threshold1 = double.Parse(pgrdIndicators.Items[5].Value.ToString());
                    Threshold2 = double.Parse(pgrdIndicators.Items[6].Value.ToString());
                    TColor = (Color)pgrdIndicators.Items[7].Value;
                    TStyle = 0;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            TStyle = 0;
                            break;
                        case "Tracejados":
                            TStyle = 1;
                            break;
                        case "Pontilhados":
                            TStyle = 2;
                            break;
                    }
                    TThickness = int.Parse(pgrdIndicators.Items[9].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {

                            StockChartX1.UpdateIndicatorRelativeStrenght(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, Threshold1, Threshold2, TColor.R, TColor.G, TColor.B,TStyle,TThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region VOL
                case "VOL":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorVolume(item, panel, Source1, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, 0);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region TP
                case "TP":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorTypicalPrice(item, panel, Symbol, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region PSAR
                case "PSAR":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    MinAF = double.Parse(pgrdIndicators.Items[0].Value.ToString());
                    MaxAF = double.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorParabolicSAR(item, panel, Symbol, MinAF, MaxAF, lineColor.R, lineColor.G,
                                                  lineColor.B,0, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region SMI
                case "SMI":
                    //Params:
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    KPeriods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Smooth = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    DPeriods = int.Parse(pgrdIndicators.Items[7].Value.ToString());
                    KDblSmooth = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Simples":
                            DMAType = 0;
                            break;
                        case "Exponencial":
                            DMAType = 1;
                            break;
                        case "TimeSeries":
                            DMAType = 2;
                            break;
                        case "Triangular":
                            DMAType = 3;
                            break;
                        case "Variável":
                            DMAType = 4;
                            break;
                        case "VIDYA":
                            DMAType = 5;
                            break;
                        case "Weighted":
                            DMAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[9].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[10].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[11].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorStocMomentum(item, panel, Symbol,
                                                                              KPeriods, Smooth, KDblSmooth, DPeriods, MAType, DMAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt,lineThickness,lineColor2.R, lineColor2.G, lineColor2.B,lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
        #endregion

                #region SO
                case "SO":
                    //Params:
                    KPeriods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    KSlowing = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[4].Value.ToString());

                    DPeriods = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    switch (pgrdIndicators.Items[6].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[7].Value;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[9].Value.ToString());



                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorStocOscillator(item, panel, Symbol,
                                                                              KPeriods, KSlowing, DPeriods, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion
                
                #region Aroon
                case "Aroon":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());



                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorAroon(item, panel, Symbol, 0, Periods,lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region Aroon Oscillator
                case "AO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorAroonOsc(item, panel, Symbol, 0, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CMF
                case "CMF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorChaikinMoney(item, panel, Symbol, 0, Symbol + ".volume", Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region EOM
                case "EOM":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorEasyMovement(item, panel, Symbol, 0, Symbol + ".volume", Periods, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region MFI
                case "MFI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:)
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorMoneyFlow(item, panel, Symbol, Symbol + ".volume", Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region ASI
                case "ASI":
                    //Params:
                    LimitMove = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:)
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorASI(item, panel, Symbol,0, LimitMove, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CV
                case "CV":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    RateChange = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());

                    //Try to add until available key isnt found:)
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorChaikinVolatility(item, panel, Symbol, 0, Periods, RateChange, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region CMO
                case "CMO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorChandeMomentum(item, panel, Source1, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region WWS
                case "WWS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorWellesWilder(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region VHF
                case "VHF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorWellesWilder(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region TRIX
                case "TRIX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorTRIX(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region LRRS
                case "LRRS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorLinearRegRSquare(item, panel, Source1, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region LRF
                case "LRF":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorLinearRegForecast(item, panel, Source1, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region LRS
                case "LRS":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorLinearRegSlope(item, panel, Source1, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region LRI
                case "LRI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorLinearRegIntercept(item, panel, Source1, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region MO
                case "MO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorMomentum(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region ADX
                case "ADX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorADX(item, panel, Symbol,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region DI
                case "DI+/DI-":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    lineStyleInt2 = 0;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorDI(item, panel, Symbol,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region DI ADX
                case "DI+/DI-/ADX":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    lineColor2 = (Color)pgrdIndicators.Items[4].Value;
                    switch (pgrdIndicators.Items[5].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt2 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt2 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt2 = 2;
                            break;
                    }
                    lineThickness2 = int.Parse(pgrdIndicators.Items[6].Value.ToString());
                    lineColor3 = (Color)pgrdIndicators.Items[7].Value;
                    switch (pgrdIndicators.Items[8].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt3 = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt3 = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt3 = 2;
                            break;
                    }
                    lineThickness3 = int.Parse(pgrdIndicators.Items[9].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorDirectMoveSystem(item, panel, Symbol,0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness, lineColor2.R, lineColor2.G, lineColor2.B, lineStyleInt2, lineThickness2, lineColor3.R, lineColor3.G, lineColor3.B, lineStyleInt3, lineThickness3);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region CCI
                case "CCI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorCommodityChannel(item, panel, Symbol, 0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region DPO
                case "DPO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[2].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorDetrendedPrice(item, panel, Source1, 0, Periods, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region MI
                case "MI":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorMassIndex(item, panel, Symbol, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region OBV
                case "OBV":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorBalanceVolume(item, panel, Source1, Symbol+".volume", lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region PROC
                case "PROC":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorPriceROC(item, panel, Source1, 
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region PVT
                case "PVT":
                    //Params:
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[0].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorPriceVolume(item, panel, Source1,
                                                  Symbol + ".volume", lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region WilliamsR
                case "Williams":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorWilliamPCTR(item, panel, Symbol,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region WAD
                case "WAD":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorWilliamAD(item, panel, Symbol,
                                                  lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region WC
                case "WC":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorWeightedCLose(item, panel, Symbol,
                                                  lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region VROC
                case "VROC":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    lineColor = (Color)pgrdIndicators.Items[2].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[3].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[4].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorVolumeROC(item, panel, Source1,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region FCO
                case "FCO":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorFractalChaosOsc(item, panel, Symbol,0,
                                                  Periods, lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region FCB
                case "FCB":
                    //Params:
                    Periods = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[1].Value;
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[3].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorFractalChaos(item, panel, Symbol, 0, Periods, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region UO
                case "UO":
                    //Params:
                    Cycle1 = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Cycle2 = int.Parse(pgrdIndicators.Items[1].Value.ToString());
                    Cycle3 = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorUltimateOsc(item, panel, Symbol, Cycle1,Cycle2,Cycle3, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region TR
                case "TR":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorTrueRange(item, panel, Symbol,
                                                  lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion

                #region RO
                case "RO":
                    //Params:
                    Levels = int.Parse(pgrdIndicators.Items[0].Value.ToString());
                    Source1 = Symbol + "." + ((Enums.Source2)(((int)(Enums.Source)Enum.Parse(typeof(Enums.Source), pgrdIndicators.Items[1].Value.ToString())))).ToString();
                    switch (pgrdIndicators.Items[2].Value.ToString())
                    {
                        case "Simples":
                            MAType = 0;
                            break;
                        case "Exponencial":
                            MAType = 1;
                            break;
                        case "TimeSeries":
                            MAType = 2;
                            break;
                        case "Triangular":
                            MAType = 3;
                            break;
                        case "Variável":
                            MAType = 4;
                            break;
                        case "VIDYA":
                            MAType = 5;
                            break;
                        case "Weighted":
                            MAType = 6;
                            break;
                    }
                    lineColor = (Color)pgrdIndicators.Items[3].Value;
                    switch (pgrdIndicators.Items[4].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[5].Value.ToString());

                    //Try to add until available key isnt found:
                    while (errorKey)
                    {

                        try
                        {
                            StockChartX1.UpdateIndicatorRainbowOsc(item, panel, Source1, Levels, MAType, lineColor.R, lineColor.G, lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }

                    break;
                #endregion

                #region A/D
                case "A/D":
                    //Params:
                    lineColor = (Color)pgrdIndicators.Items[0].Value;
                    lineStyleInt = 0;
                    switch (pgrdIndicators.Items[1].Value.ToString())
                    {
                        case "Normal":
                            lineStyleInt = 0;
                            break;
                        case "Tracejados":
                            lineStyleInt = 1;
                            break;
                        case "Pontilhados":
                            lineStyleInt = 2;
                            break;
                    }
                    lineThickness = int.Parse(pgrdIndicators.Items[2].Value.ToString());
                    //Try to add until available key isnt found:
                    while (errorKey)
                    {
                        try
                        {
                            StockChartX1.UpdateIndicatorAccumulationDistribution(item, panel, Symbol, 0, Symbol+".Volume",0,
                                                  lineColor.R, lineColor.G,
                                                  lineColor.B, lineStyleInt, lineThickness);
                            errorKey = false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Key not unique")
                            {
                                errorKey = true;
                                indicatorKey++;
                            }
                            else
                            {
                                Telerik.WinControls.RadMessageBox.Show(ex.Message);
                                return false;
                            }
                        }
                    }
                    break;
                #endregion


            }
            StockChartX1.ForcePaint(); 
            StockChartX1.LoadUserStudyLine(-1);

            return true;
        }


        private void trvIndicators_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
            if (char.IsLetter((char)e.KeyValue) || char.IsNumber((char)e.KeyValue) || e.KeyValue == 32)
            {
                if (e.KeyValue == 32) _searchIndicator += " ";
                else _searchIndicator += ((char)e.KeyValue).ToString().ToUpper();
                if (!SearchTextNode())
                {
                    _searchIndicator = ((char)e.KeyValue).ToString().ToUpper();

                    if (!SearchTextNode())
                        _searchIndicator = "";
                }
            }

        }
        
        private bool SearchTextNode()
        {
            //trvIndicators.BringIntoView(trvIndicators.SelectedNode);
            trvIndicators.ExpandAll();
            trvIndicators.Refresh();
            foreach (var t in trvIndicators.Nodes.Where(t => t.Text.ToString().ToUpper().StartsWith(_searchIndicator)))
            {
                t.Selected = true;
                trvIndicators.SelectedNode = t;
                trvIndicators.ExpandAll();
                trvIndicators.Refresh();
                //trvIndicators.BringIntoView(t);
                if (trvIndicators.SelectedNode.Parent == null)
                {
                    if (trvIndicators.SelectedNode.Index + 7 < trvIndicators.Nodes.Count()) trvIndicators.BringIntoView(trvIndicators.Nodes[trvIndicators.SelectedNode.Index + 7]);
                    else trvIndicators.BringIntoView(trvIndicators.Nodes.Last());
                }
                else
                {
                    if (trvIndicators.SelectedNode.Parent.Index + 7 < trvIndicators.Nodes.Count()) trvIndicators.BringIntoView(trvIndicators.Nodes[trvIndicators.SelectedNode.Parent.Index + 7]);
                    else trvIndicators.BringIntoView(trvIndicators.Nodes.Last());
                }
                trvIndicators.Refresh();
                return true;
            }      
            return false;
        }

        private void FrmSelectIndicator_FormClosing(object sender, FormClosingEventArgs e)
        {
            StockChartX1.UnSelect();
            //e.Cancel = true;
            //Hide();
        }
        
        private void FrmSelectIndicator_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                //Telerik.WinControls.RadMessageBox.Show("FrmSelectIndicator_VisibleChanged");
                /*GetStockChartIndicators();
                TopMost = true;
                GotFocus += FrmSelectIndicator_GotFocus;
                //trvIndicators.LostFocus += trvIndicators_LostFocus;
                trvIndicators.GotFocus += trvIndicators_GotFocus;
                trvIndicators.Focus();*/
            }

        }
    
        private void trvIndicators_LostFocus(object sender, EventArgs e)
        {
            //Telerik.WinControls.RadMessageBox.Show("trvIndicators_LostFocus");
            
        }
       
        private void trvIndicators_GotFocus(object sender, EventArgs e)
        {
            //Telerik.WinControls.RadMessageBox.Show("trvIndicators_GotFocus");

        }
       
        private void FrmSelectIndicator_GotFocus(object sender, EventArgs e)
        {
            trvIndicators.Focus();
        }

        private void FrmSelectIndicator_Shown(object sender, EventArgs e)
        {
            //Telerik.WinControls.RadMessageBox.Show("showm");
            GetStockChartIndicators();

            ResizeTreeview();
            trvIndicators.Focus();

        }

        public void ResizeTreeview()
        {            
            trvIndicators.Height = 369;
            trvIndicators.Height = 370;
        }

    }
        #endregion
}