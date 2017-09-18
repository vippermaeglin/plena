using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using M4.M4v2.Chart.IndicatorElements;
using M4.M4v2.Chart.Mock;
using Telerik.WinControls.UI;
using AxSTOCKCHARTXLib;

namespace M4.M4v2.Chart.LineStudy
{
    public partial class FrmSelectStudy : RadForm
    {
        #region Members and Properties

        public string Symbol { get; set; }
        public readonly Mock.Mock _mock = new Mock.Mock();
        public AxStockChartX StockChartX1 { get; set; }
        public string LineStudyType;
        public string LineStudyKey;
        public CtlPainelChart ParentForm;
        public StudyInfo infoGrid;
        public Point cursorPosition;
        public double NULL_VALUE = -987654321.0;

        #endregion

        #region Initialize

        public FrmSelectStudy()
        {
            InitializeComponent();

            ConfigurePropertyGrid();
        }

        private void InitializeStudy()
        {
            if (LineStudyType == "TrendLine")
            {
                infoGrid = new StudyInfo
                               {
                                   ColorParameters =
                                       ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                                   LineStyleParameters = (Enums.LineStyle) StockChartX1.GetTrendLineStyle(LineStudyKey),
                                   LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey),
                                   RightExtensionParameters = StockChartX1.GetTrendLineRightExtension(LineStudyKey),
                                   LeftExtensionParameters = StockChartX1.GetTrendLineLeftExtension(LineStudyKey)

                               };
            }
            else if (LineStudyType == "RayLine") //Ray only have one Radius Extension or both Left\Right Extension
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey),
                    ExtensionParameters = StockChartX1.GetTrendLineRightExtension(LineStudyKey)

                };
            }
            else if (LineStudyType == "VerticalLine") 
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "HorizontalLine")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey),
                    ExtensionParameters = StockChartX1.GetTrendLineLeftExtension(LineStudyKey),
                    ValuePosition = Math.Round(StockChartX1.GetTrendLineValue(LineStudyKey),3)
                };
            }
            else if (LineStudyType == "Rectangle")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Ellipse")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Arrow")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "FreeHand")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Polyline")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Channel")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Fibonacci Arcs")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Fibonacci Fan")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Fibonacci Projections")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey),
                    Percent1Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 0) != NULL_VALUE,
                    Percent2Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 1) != NULL_VALUE,
                    Percent3Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 2) != NULL_VALUE,
                    Percent4Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 3) != NULL_VALUE,
                    Percent5Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 4) != NULL_VALUE,
                    Percent6Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 5) != NULL_VALUE,
                    Percent7Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 6) != NULL_VALUE
                };

            }
            else if (LineStudyType == "Fibonacci Retracements")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey),
                    Percent1Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 0) != NULL_VALUE,
                    Percent2Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 1) != NULL_VALUE,
                    Percent3Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 2) != NULL_VALUE,
                    Percent4Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 3) != NULL_VALUE,
                    Percent5Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 4) != NULL_VALUE,
                    Percent6Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 5) != NULL_VALUE,
                    Percent7Parameters = StockChartX1.GetFibonacciParameter(LineStudyKey, 6) != NULL_VALUE
                };
                
            }
            else if (LineStudyType == "Fibonacci Timezones")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Speed Line")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            else if (LineStudyType == "Gann Fan")
            {
                infoGrid = new StudyInfo
                {
                    ColorParameters =
                        ColorTranslator.FromOle(StockChartX1.GetTrendLineColor(LineStudyKey)),
                    LineStyleParameters = (Enums.LineStyle)StockChartX1.GetTrendLineStyle(LineStudyKey),
                    LineThicknessParameters = StockChartX1.GetTrendLineThickness(LineStudyKey)
                };
            }
            pgrdStudy.SelectedObject = infoGrid;
            LoadDictionaryParameters();
            //pgrdStudy.AutoSize = true;
        }

        private void LoadDictionaryParameters()
        {
            pgrdStudy.Groups[0].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyParameters"];
            pgrdStudy.Items[0].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyColor"];
            pgrdStudy.Items[1].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyLineStyle"];
            pgrdStudy.Items[2].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyLineThickness"];
            pgrdStudy.Items[3].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyRightExtension"];
            pgrdStudy.Items[4].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyLeftExtension"];
            pgrdStudy.Items[5].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyExtension"];
            pgrdStudy.Items[6].Label = "0%";
            pgrdStudy.Items[7].Label = "38,2%";
            pgrdStudy.Items[8].Label = "50%";
            pgrdStudy.Items[9].Label = "61,8%";
            pgrdStudy.Items[10].Label = "100%";
            pgrdStudy.Items[11].Label = "161,8%";
            pgrdStudy.Items[12].Label = "261,8%";
            pgrdStudy.Items[13].Label = Program.LanguageDefault.DictionarySelectIndicator["gprStudyValue"];

        }

        private void ConfigurePropertyGrid()
        {
            pgrdStudy.EnableSorting = false;
            pgrdStudy.HelpVisible = true;
            pgrdStudy.SortOrder = SortOrder.None;
            pgrdStudy.PropertyValidating += pgrdStudy_PropertyValidating;
            pgrdStudy.Edited += pgrdStudy_Edited;
            pgrdStudy.EditorInitialized += pgrdStudy_EditorInitialized;
            pgrdStudy.PropertySort = PropertySort.Categorized;
            pgrdStudy.PropertyGridElement.SplitElement.PropertyTableElement.ItemHeight = 20;
            pgrdStudy.PropertyGridElement.SplitElement.PropertyTableElement.ItemIndent = 15;
            if (LineStudyType == "Fibonacci Retracements" || LineStudyType == "Fibonacci Projections")
                this.Size = new Size(Size.Width,220);
        }

        #endregion

        #region Methods
        
        private void CustomizeGrid()
        {
            EnableAll();
            if(LineStudyType=="TrendLine")
            {
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if(LineStudyType == "RayLine")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "VerticalLine")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "HorizontalLine")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
            }
            else if (LineStudyType == "Rectangle")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Ellipse")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Arrow")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "FreeHand")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Polyline")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Channel")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                //pgrdStudy.Items[5].Visible = false;
                //pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Fibonacci Arcs")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Fibonacci Fan")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Fibonacci Projections")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Fibonacci Retracements")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Fibonacci Timezones")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Speed Line")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
            else if (LineStudyType == "Gann Fan")
            {
                pgrdStudy.Items[3].Visible = false;
                pgrdStudy.Items[3].Expanded = false;
                pgrdStudy.Items[4].Visible = false;
                pgrdStudy.Items[4].Expanded = false;
                pgrdStudy.Items[5].Visible = false;
                pgrdStudy.Items[5].Expanded = false;
                pgrdStudy.Items[6].Visible = false;
                pgrdStudy.Items[6].Expanded = false;
                pgrdStudy.Items[7].Visible = false;
                pgrdStudy.Items[7].Expanded = false;
                pgrdStudy.Items[8].Visible = false;
                pgrdStudy.Items[8].Expanded = false;
                pgrdStudy.Items[9].Visible = false;
                pgrdStudy.Items[9].Expanded = false;
                pgrdStudy.Items[10].Visible = false;
                pgrdStudy.Items[10].Expanded = false;
                pgrdStudy.Items[11].Visible = false;
                pgrdStudy.Items[11].Expanded = false;
                pgrdStudy.Items[12].Visible = false;
                pgrdStudy.Items[12].Expanded = false;
                pgrdStudy.Items[13].Visible = false;
                pgrdStudy.Items[13].Expanded = false;
            }
        }

        private void EnableAll()
        {
            foreach (var item in pgrdStudy.Items)
            {
                item.Visible = true;
                item.Expanded = true;
            }
        }

        #endregion

        #region PropertyGrid Events

        private void pgrdStudy_EditorInitialized(object sender, PropertyGridItemEditorInitializedEventArgs e)
        {
            PropertyGridColorEditor gridColorEditor = e.Editor as PropertyGridColorEditor;

            if (gridColorEditor != null)
            {
                RadColorBoxElement colorBoxElement = ((PropertyGridColorEditor)e.Editor).EditorElement as RadColorBoxElement;

                if (colorBoxElement != null)
                {
                    //colorBoxElement.ColorDialog.ColorDialogForm.ShowSystemColors = false;
                    //colorBoxElement.ColorDialog.ColorDialogForm.ShowWebColors = false;
                    colorBoxElement.ColorDialog.ColorDialogForm.ActiveMode = Telerik.WinControls.ColorPickerActiveMode.Basic;
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
                case "PeriodsParameters":
                    if (element != null)
                    {
                        element.MinValue = 1;
                        element.MaxValue = 999;
                    }
                    break;
            }
        }

        private void pgrdStudy_Edited(object sender, PropertyGridItemEditedEventArgs e)
        {
            PropertyGridItem item = e.Item as PropertyGridItem;

            if (item != null)
                item.ErrorMessage = "";
        }

        private void pgrdStudy_PropertyValidating(object sender, PropertyValidatingEventArgs e)
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
        }

        private void pgrdStudy_ItemFormatting(object sender, PropertyGridItemFormattingEventArgs e)
        {
            //CustomizeGrid("study",e.Item);
            /*
            if (trvIndicators.SelectedNode == null) return;
            try
            {
                CustomizeGrid(trvIndicators.SelectedNode.Parent == null ? trvIndicators.SelectedNode.Value.ToString() :
                   trvIndicators.SelectedNode.Parent.Value.ToString(), e.Item);
            }
            catch (Exception ex)
            {

                Telerik.WinControls.RadMessageBox.Show(ex.Message + "\n" + ex);
            }*/
        }

        #endregion

        
        private void FrmSelectStudy_Shown(object sender, EventArgs e)
        {
            Text = Program.LanguageDefault.DictionarySelectIndicator[LineStudyType];

            InitializeStudy();

            CustomizeGrid();

            if (LineStudyType == "Fibonacci Retracements" || LineStudyType == "Fibonacci Projections")
                Height = 300;
            else Height = 220;
        }
        
        private void pgrdStudy_PropertyValueChanged(object sender, PropertyGridItemValueChangedEventArgs e)
        {
            Color StudyColor = (Color) pgrdStudy.Items[0].Value;
            Enums.LineStyle StudyLineStyle = (Enums.LineStyle) pgrdStudy.Items[1].Value;
            int StudyLineThickness = (int)pgrdStudy.Items[2].Value;
            bool StudyRightExtension = (bool) pgrdStudy.Items[3].Value;
            bool StudyLeftExtension = (bool) pgrdStudy.Items[4].Value;
            bool StudyExtension = (bool) pgrdStudy.Items[5].Value;
            double StudyParam00 = (bool)pgrdStudy.Items[10].Value ? 0.0 : NULL_VALUE;
            double StudyParam38 = (bool)pgrdStudy.Items[9].Value ? 0.382 : NULL_VALUE;
            double StudyParam50 = (bool)pgrdStudy.Items[8].Value ? 0.5 : NULL_VALUE;
            double StudyParam61 = (bool)pgrdStudy.Items[7].Value ? 0.618 : NULL_VALUE;
            double StudyParam100 = (bool)pgrdStudy.Items[6].Value ? 1.0 : NULL_VALUE;
            double StudyParam161 = (bool)pgrdStudy.Items[11].Value ? 1.618 : NULL_VALUE;
            double StudyParam261 = (bool)pgrdStudy.Items[12].Value ? 2.618 : NULL_VALUE;
            double StudyValue = (double)pgrdStudy.Items[13].Value;
            infoGrid = new StudyInfo
                                         {
                                             ColorParameters = StudyColor,
                                             LineStyleParameters = StudyLineStyle,
                                             LineThicknessParameters = StudyLineThickness,
                                             RightExtensionParameters = StudyRightExtension,
                                             LeftExtensionParameters = StudyLeftExtension,
                                             ExtensionParameters = StudyExtension
                                         };
            if (LineStudyType == "TrendLine")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, StudyRightExtension, StudyLeftExtension,0);
            }
            else if (LineStudyType == "RayLine")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, true, StudyExtension, StudyExtension,0);
            }
            else if (LineStudyType == "VerticalLine")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false,0);
            }
            else if (LineStudyType == "HorizontalLine")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, StudyExtension, StudyValue);
            }
            else if (LineStudyType == "Rectangle")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false, 0);
            }
            else if (LineStudyType == "Ellipse")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false, 0);
            }
            else if (LineStudyType == "Arrow")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false, 0);
            }
            else if (LineStudyType == "FreeHand")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false, 0);
            }
            else if (LineStudyType == "Polyline")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, false, false, 0);
            }
            else if (LineStudyType == "Channel")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, StudyExtension, 0);
            }
            else if (LineStudyType == "Fibonacci Arcs")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
            }
            else if (LineStudyType == "Fibonacci Fan")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
            }
            else if (LineStudyType == "Fibonacci Projections")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
                StockChartX1.UpdateFibonacciParams(LineStudyKey, StudyParam100, StudyParam61, StudyParam50, StudyParam38, StudyParam00, StudyParam161, StudyParam261, NULL_VALUE, NULL_VALUE, NULL_VALUE);
            }
            else if (LineStudyType == "Fibonacci Retracements")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
                StockChartX1.UpdateFibonacciParams(LineStudyKey, StudyParam100, StudyParam61, StudyParam50, StudyParam38, StudyParam00, StudyParam161, StudyParam261, NULL_VALUE, NULL_VALUE, NULL_VALUE);
            }
            else if (LineStudyType == "Fibonacci Timezones")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);


            }
            else if (LineStudyType == "Speed Line")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
            }
            else if (LineStudyType == "Gann Fan")
            {
                StockChartX1.UpdateTrendLine(LineStudyKey, StudyLineStyle.GetHashCode(), StudyLineThickness,
                                             StudyColor.R, StudyColor.G,
                                             StudyColor.B, false, true, false, 0);
            }

        }

        private void FrmSelectStudy_Leave(object sender, EventArgs e)
        {
            pgrdStudy.EndEdit();
        }

        private void FrmSelectStudy_FormClosing(object sender, FormClosingEventArgs e)
        {
            pgrdStudy.EndEdit();
            StockChartX1.Update();
        }


    }
}