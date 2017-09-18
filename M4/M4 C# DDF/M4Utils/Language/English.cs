
using System;

namespace M4Utils.Language
{
    public class English : LanguageDefault
    {
        public English()
        {
            LoadPlena();
            LoadDictionaryAssets();
            LoadTabAssets();
            LoadMenuAssets();
            LoadChartCtl();
            LoadMenuBar();
            LoadMessage();
            LoadTitleMessage();
            LoadSelectChart();
            LoadSelectIndicator();
            LoadSelectStudy();
            LoadBrowser();
            LoadSelectTools();
            LoadSettings();
            LoadPortfolio();
            LoadMenuPlena();
            LoadLogin();
            LoadPermission();
            LoadTemplate();
            LoadWorkspace();
            LoadOutput();
        }

        public override sealed void LoadPlena()
        {
            DictionaryPlena.Add("webBrowser", "Web");
            DictionaryPlena.Add("tradeDiary", "Trade Diary");
            DictionaryPlena.Add("statusManager", "Status Manager");
            DictionaryPlena.Add("about", "Copyright by Seamus Financial Software\n\n\n\nWarning: This computer program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the maximum extend possible under the law.");
        }

        public override sealed void LoadMenuPlena()
        {
            DictionaryMenuPlena.Add("clearSorting", "Clear Sorting");
            DictionaryMenuPlena.Add("mnuMenuFile", "File");
            DictionaryMenuPlena.Add("mnuLoadChartDisk", "Load Chart From Disk");
            DictionaryMenuPlena.Add("mnuSaveChart", "Save Chart");
            DictionaryMenuPlena.Add("mnuSaveChartImage", "Save Chart As Image");
            DictionaryMenuPlena.Add("mnuPrintChart", "Print Chart");
            DictionaryMenuPlena.Add("mnuExit", "Exit");
            DictionaryMenuPlena.Add("mnuMenuHelp", "Help");
            DictionaryMenuPlena.Add("mnuHelpContent", "Help Content");
            DictionaryMenuPlena.Add("mnuAboutPLENA", "About PLENA");
            DictionaryMenuPlena.Add("mnuMenuWorkspace", "Workspace");
            DictionaryMenuPlena.Add("mnuManager", "Manager");
            DictionaryMenuPlena.Add("mnuMenuChart", "Chart");
            DictionaryMenuPlena.Add("mnuPopoutActiveChart", "Popout Active Chart");
            DictionaryMenuPlena.Add("mnuTradesTweets", "Trades Tweets");
            DictionaryMenuPlena.Add("mnuMenuView", "View");
            DictionaryMenuPlena.Add("mnuToolbar", "Componentes");
            DictionaryMenuPlena.Add("mnuCalculator", "Calculator");
            DictionaryMenuPlena.Add("mnuChartTools", "Chart Tools");
            //DictionaryMenuPlena.Add("mnuApplicationStyle", "Application Style");
            DictionaryMenuPlena.Add("mnuChartColors", "Color Scheme");
            DictionaryMenuPlena.Add("mnuApplicationStyle", "Application Style");
            DictionaryMenuPlena.Add("mnuUseSemiLogScale", "Use Semi-Log Scale");
            DictionaryMenuPlena.Add("mnuShowXGrid", "Show X Grid");
            DictionaryMenuPlena.Add("mnuShowYGrid", "Show Y Grid");
            DictionaryMenuPlena.Add("mnuShowPanelSeparators", "Show Panel Separators");
            DictionaryMenuPlena.Add("mnuCrosshair", "Crosshair");
            DictionaryMenuPlena.Add("mnuDarvasBoxes", "Darvas Boxes");
            DictionaryMenuPlena.Add("mnuViewStartPag", "View Start Page");
            DictionaryMenuPlena.Add("mnuSettings", "Settings");
            DictionaryMenuPlena.Add("mnuCloseWindow", "Close Tab");
            DictionaryMenuPlena.Add("mnuCloseAllButThis", "Close Other Tabs");
            DictionaryMenuPlena.Add("mnuCloseAll", "Close All Tabs");
            DictionaryMenuPlena.Add("mnuFloat", "Float");
            DictionaryMenuPlena.Add("mnuFloating", "Floating");
            DictionaryMenuPlena.Add("mnuDocked", "Docked");
            DictionaryMenuPlena.Add("mnuSortWindow", "Sort Windows");
            DictionaryMenuPlena.Add("mnuAutoHide", "Auto Hide");
            DictionaryMenuPlena.Add("mnuClearSorting", "Clear Sorting");
            DictionaryMenuPlena.Add("mnuAddRemoveButtonsMenuItem", "Add or Remove Buttons");
            DictionaryMenuPlena.Add("mnuPanelPosition", "Panel Position");
            DictionaryMenuPlena.Add("mnuPanelDown", "Down");
            DictionaryMenuPlena.Add("mnuPanelUp", "Up");
        }

        /// <summary>
        /// Carrega os valores do dicionário de ativos
        /// </summary>
        public override sealed void LoadDictionaryAssets()
        {
            #region GridAssets

            DictionaryGridAssets.Add("columnActive", "Active");
            DictionaryGridAssets.Add("columnAmount", "Amount");
            DictionaryGridAssets.Add("columnBusiness", "Business");
            DictionaryGridAssets.Add("columnClosing", "Closing");
            DictionaryGridAssets.Add("columnHour", "Hour");
            DictionaryGridAssets.Add("columnLast", "Last");
            DictionaryGridAssets.Add("columnMaximum", "Maximum");
            DictionaryGridAssets.Add("columnMinimum", "Minimum");
            DictionaryGridAssets.Add("columnOpening", "Opening");
            DictionaryGridAssets.Add("columnVariation", "Variation");
            DictionaryGridAssets.Add("columnVolume", "Volume");
            DictionaryGridAssets.Add("messageDeleteRecord", "Want to delete this record?");
            DictionaryGridAssets.Add("titleDeleteRecord", "Delete Record");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário de tabs dos ativos
        /// </summary>
        public override sealed void LoadTabAssets()
        {
            #region TabAssets

            DictionaryTabAssets.Add("tabAll", "All");
            DictionaryTabAssets.Add("newTab", "New Tab");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário do menu dos ativos
        /// </summary>
        public override sealed void LoadMenuAssets()
        {
            #region MenuAssets

            DictionaryMenuAssets.Add("btnNewWallet", "New Wallet");
            DictionaryMenuAssets.Add("btnDeleteWallet", "Delete Wallet");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário da tela do gráfico
        /// </summary>
        public override sealed void LoadChartCtl()
        {
            #region Chart

            DictionaryChartCtl.Add("twSumary", "Sumary");
            DictionaryChartCtl.Add("twAdvisors", "Advisors");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário do menu principal
        /// </summary>
        public override sealed void LoadMenuBar()
        {
            #region MenuBar

            DictionaryMenuBar.Add("cmdPositionFlaps", "Position Flaps");
            
            DictionaryMenuBar.Add("cbxApplicationStyle", "Application Style");
            DictionaryMenuBar.Add("optOffice2007Blue", "Office 2007 Blue");
            DictionaryMenuBar.Add("optOffice2007Silver", "Office 2007 Silver");
            DictionaryMenuBar.Add("optWindowsVista", "Windows Vista");
            DictionaryMenuBar.Add("optWindowsDefault", "Windows Default");

            DictionaryMenuBar.Add("cmdUseSemiLogScale", "Use Semi-Log Scale");
            DictionaryMenuBar.Add("cmdShowXGrid", "Show X Grid");
            DictionaryMenuBar.Add("cmdShowYGrid", "Show Y Grid");
            DictionaryMenuBar.Add("cmdShowPanelSeparators", "Show Panel Separators");
            DictionaryMenuBar.Add("cmdCrosshair", "Crosshair");
            DictionaryMenuBar.Add("cmdThreeDStyle", "Three D Style");
            DictionaryMenuBar.Add("cmdDarvasBoxes", "Darvas Boxes");
            DictionaryMenuBar.Add("cmdViewStarPage", "View StarPage");
            DictionaryMenuBar.Add("cmdViewForexScreen", "View Forex Screen");
            DictionaryMenuBar.Add("cmdTextObject", "Text Object");
            DictionaryMenuBar.Add("cmdBuySymbol", "Buy Symbol");
            DictionaryMenuBar.Add("cmdSellSymbol", "Sell Symbol");
            DictionaryMenuBar.Add("cmdExitSymbol", "Exit Symbol");
            DictionaryMenuBar.Add("cmdLineColor", "Line Color");
            DictionaryMenuBar.Add("cmdTrendLine", "Trend Line");
            DictionaryMenuBar.Add("cmdEllipse", "Ellipse");
            DictionaryMenuBar.Add("cmdSpeedLines", "Speed Lines");
            DictionaryMenuBar.Add("cmdGannFan", "Gann Fan");
            DictionaryMenuBar.Add("cmdChart", "New Chart");
            DictionaryMenuBar.Add("cmdFibonacciArcs", "Fibonacci Arcs");
            DictionaryMenuBar.Add("cmdFibonacciRetracements", "Fibonacci Retracements");
            DictionaryMenuBar.Add("cmdFibonacciFan", "Fibonacci Fan");
            DictionaryMenuBar.Add("cmdFibonacciTimeZones", "Fibonacci Time Zones");
            DictionaryMenuBar.Add("cmdTironeLevels", "Tirone Levels");
            DictionaryMenuBar.Add("cmdQuadrantLines", "Quadrant Lines");
            DictionaryMenuBar.Add("cmdRaffRegression", "Raff Regression");
            DictionaryMenuBar.Add("cmdErrorChannels", "Error Channels");
            DictionaryMenuBar.Add("cmdRectangle", "Rectangle");
            DictionaryMenuBar.Add("cmdArrow", "Arrow");
            DictionaryMenuBar.Add("cmdFreeHandDrawing", "Freehand Drawing");
            DictionaryMenuBar.Add("cmdZoomArea", "Zoom Area");
            DictionaryMenuBar.Add("cmdZoomZero", "Overview");
            DictionaryMenuBar.Add("cmdZoomIn", "Zoom In");
            DictionaryMenuBar.Add("cmdZoomOut", "Zoom Out");
            DictionaryMenuBar.Add("cmdCandleChart", "Candle Chart");
            DictionaryMenuBar.Add("cmdStockLine", "StockLine");
            DictionaryMenuBar.Add("cmdBarChart", "Bar Chart");
            DictionaryMenuBar.Add("cmdHeikinAshi", "Heikin Ashi");
            DictionaryMenuBar.Add("cmdHeikinAshiSmooth", "Heikin Ashi Smoothed");
            DictionaryMenuBar.Add("cmdDeleteStock", "Delete Drawings");
            DictionaryMenuBar.Add("cmdTechnicalAnalysis", "Technical Analysis");
            DictionaryMenuBar.Add("cmdPriceSeries", "Price Options");

            DictionaryMenuBar.Add("mnuToolbar", "Toolbar");
            DictionaryMenuBar.Add("mnuPriceIndicatorsToolbar", "Price and Indicators");
            DictionaryMenuBar.Add("mnuZoomTemplatesToolbar", "Zoom and Templates");
            DictionaryMenuBar.Add("mnuCalculatorToolbar", "Calculator");
            DictionaryMenuBar.Add("mnuViewToolbar", "View");
            DictionaryMenuBar.Add("mnuChartToolsToolbar", "Chart Tools");
            DictionaryMenuBar.Add("mnuFileToolbar", "File");

            DictionaryMenuBar.Add("cmdSelect", "Select");
            DictionaryMenuBar.Add("cmdDeltaCursor", "Measure");
            DictionaryMenuBar.Add("cmdMagnetic", "Magnetic");
            DictionaryMenuBar.Add("cmdRay", "Ray");
            DictionaryMenuBar.Add("cmdChannel", "Channel");
            DictionaryMenuBar.Add("cmdHorizontalLine", "Horizontal Line");
            DictionaryMenuBar.Add("cmdVerticalLine", "Vertical Line");
            DictionaryMenuBar.Add("cmdElipse", "Elipse");
            DictionaryMenuBar.Add("cmdPolyline", "Polyline");
            DictionaryMenuBar.Add("cmdFibonacciProjections", "Fibonacci Projections");

            DictionaryMenuBar.Add("cmdPeriodicityDaily", "Daily");
            DictionaryMenuBar.Add("cmdPeriodicityMonth", "Weekly");
            DictionaryMenuBar.Add("cmdPeriodicityWeekly", "Month");
            DictionaryMenuBar.Add("cmdPeriodicityYearly", "Year");
            DictionaryMenuBar.Add("cmdPeriodicityCustom", "Periodicity Custom");
            DictionaryMenuBar.Add("cmdPeriodicityAsk", "(Ask)");

            DictionaryMenuBar.Add("titleTabDaily", "Daily");
            DictionaryMenuBar.Add("titleTabWeekly", "Weekly");
            DictionaryMenuBar.Add("titleTabMonthly", "Monthly");
            DictionaryMenuBar.Add("titleTabYearly", "Yearly");

            DictionaryMenuBar.Add("titleShortTabDaily", "Day");
            DictionaryMenuBar.Add("titleShortTabWeekly", "Wk");
            DictionaryMenuBar.Add("titleShortTabMonthly", "Month");
            DictionaryMenuBar.Add("titleShortTabYearly", "Year");

            DictionaryMenuBar.Add("ndtChartTools", "Chart Tools");

            #endregion
        }

        public override sealed void LoadMessage()
        {
            DictionaryMessage.Add("msgWarningTitle", "Warning");
            DictionaryMessage.Add("msgSelectStock", "Select an stock to continue");
            DictionaryMessage.Add("msgSelectPeriodicity", "Select a periodicity to continue");
            DictionaryMessage.Add("msgErrLoadStockLocalData", "Active not found in our database");
            DictionaryMessage.Add("msgGenerateDataWeekly", "There was an error in data conversion.");
            DictionaryMessage.Add("msgCandlesMinimum", "Recurrence has not generated the required minimum number of candles for display.");
            DictionaryMessage.Add("msgIntervalMaximum", "The range for intra-day periodicity can not be less than 1 or exceeds 420 minutes.");
            DictionaryMessage.Add("msgHistoryMaximum", "The range for intra-day history can not be less than 2 or exceeds 15000.");
            DictionaryMessage.Add("msgSelectIndicator", "Select a indicator to continue");
            DictionaryMessage.Add("msgPeriodsMinimum", "The reporting period must be greater than 0.");
            DictionaryMessage.Add("msgCycleMinimum", "The cycle is reported to be greater than 0.");
            DictionaryMessage.Add("msgPeriodsIsEmpty", "The reporting period can not be empty.");
            DictionaryMessage.Add("msgThresholdIsEmpty", "The reporting threshold can not be empty.");
            DictionaryMessage.Add("msgCycleIsEmpty", "The cycle entered can not be empty");
            DictionaryMessage.Add("msgRemoveIndicatorMock", "Really remove the indicator selected?");
            DictionaryMessage.Add("msgIndicatorChanged", "Indicator successfully changed!");
            DictionaryMessage.Add("msgIndicatorInserted", "Indicator inserted successfully!");
            DictionaryMessage.Add("msgExistingBookmark", "Existing Bookmark!");
            DictionaryMessage.Add("msgNotRootRemove", "Can not remove records that contain items.");
            DictionaryMessage.Add("msgTemplateAdded", "Template successfully added!");
            DictionaryMessage.Add("msgTemplateDeleted", "Template successfully deleted!");
            DictionaryMessage.Add("msgTemplateExists", "There is information in this description.");
            DictionaryMessage.Add("msgWorkspaceExists", "There is information in this description.");
            DictionaryMessage.Add("msgTemplateChanged", "Template changed successfully!");
            DictionaryMessage.Add("msgTemplateEmpty", "Enter a value in the template!");
            DictionaryMessage.Add("msgWorkspaceEmpty", "Enter a value in the workspace!");
            DictionaryMessage.Add("msgTemplateMainNotRemoved", "Template main not can removed.");
            DictionaryMessage.Add("msgWorkspaceMainNotRemoved", "Workspace main not can removed.");
            DictionaryMessage.Add("msgRemoveStudy", "Really remove the study selected?");
            DictionaryMessage.Add("msgRemoveWorkspace", "Really remove the selected workspace?");
            DictionaryMessage.Add("msgRemoveNodeTemplate", "Really remove the selected template?");
            DictionaryMessage.Add("msgRemoveAllDrawings", "Remove all studies/drawings?");
            DictionaryMessage.Add("msgInvalidPeriodicity", "Invalid periodicity for ");
        }

        public override sealed void LoadTitleMessage()
        {
            DictionaryTitleMessage.Add("titleAnswerError", "Answer Error");
        }

        public override sealed void LoadSelectChart()
        {
            DictionarySelectChart.Add("lblDescriptionSymbol", "Symbol");
            DictionarySelectChart.Add("lblDescriptionPeriodicity", "Periodicity");
            DictionarySelectChart.Add("lblAdvanced", "Advanced");
            DictionarySelectChart.Add("lblAdvancedDescriptionPeriodicity", "Periodicity");
            DictionarySelectChart.Add("lblAdvancedDescriptionHistory", "History");
            DictionarySelectChart.Add("lblAdvancedDescriptionInterval", "Interval");
            DictionarySelectChart.Add("btnOk", "Ok");
            DictionarySelectChart.Add("btnCancel", "Cancel");
            DictionarySelectChart.Add("btnDismiss", "Dismiss");
            DictionarySelectChart.Add("btnWeek", "W");
            DictionarySelectChart.Add("btnYear", "Y");
            DictionarySelectChart.Add("btnAdvancedWeek", "W");
            DictionarySelectChart.Add("btnAdvancedYear", "Y");
            DictionarySelectChart.Add("FrmSelectChartTitle", "New Chart");
            DictionarySelectChart.Add("msgTemplateExists", "Template already exists!");
            DictionarySelectChart.Add("msgWorkspaceExists", "Workspace already exists!");
            DictionarySelectChart.Add("mnuEdit", "Edit");
            DictionarySelectChart.Add("mnuDelete", "Delete");
        }

        public override sealed void LoadSelectIndicator()
        {
            DictionarySelectIndicator.Add("ErrorPanel", "There's no place to add this indicator, remove one panel!");
            DictionarySelectIndicator.Add("btnNew", "New Indicator");
            DictionarySelectIndicator.Add("btnRemove", "Remove Indicator");
            DictionarySelectIndicator.Add("btnApply", "Apply Changes");
            //Indicators Parameters
            DictionarySelectIndicator.Add("FrmSelectIndicatorTitle", "Indicators");
            DictionarySelectIndicator.Add("lblShortCycle", "Short Cycle");
            DictionarySelectIndicator.Add("lblLongCycle", "Long Cycle");
            DictionarySelectIndicator.Add("lblPeriods", "Periods");
            DictionarySelectIndicator.Add("lblColor", "Color");
            DictionarySelectIndicator.Add("lblLineStyle", "Line Style");
            DictionarySelectIndicator.Add("lblLineThickness", "Line Thickness");
            DictionarySelectIndicator.Add("lblWindow", "Window");
            DictionarySelectIndicator.Add("lblSourceAverage", "Source");
            DictionarySelectIndicator.Add("lblIndicators", "Indicators");
            DictionarySelectIndicator.Add("lblScaleAverage", "Scale");
            DictionarySelectIndicator.Add("lblLimitMoveValue", "Limit Move Value");
            DictionarySelectIndicator.Add("lblMinimumTickValue", "Minimum Tick value");
            DictionarySelectIndicator.Add("lblSourceParameters", "Source");
            DictionarySelectIndicator.Add("lblVolumeSource", "Volume");
            DictionarySelectIndicator.Add("lblLongTermParameters", "Long Term");
            DictionarySelectIndicator.Add("lblShortTermParameters", "Short Term");
            DictionarySelectIndicator.Add("lblPointsPercentsParameters", "Points or Percents");
            DictionarySelectIndicator.Add("lblCycle1", "Cycle 1");
            DictionarySelectIndicator.Add("lblCycle2", "Cycle 2");
            DictionarySelectIndicator.Add("lblCycle3", "Cycle 3");
            DictionarySelectIndicator.Add("lblStandardDev", "Standard Dev");
            DictionarySelectIndicator.Add("lblDeviation", "Deviation");
            DictionarySelectIndicator.Add("lblPercentage", "Percentage");
            DictionarySelectIndicator.Add("lblType", "MA Type");
            DictionarySelectIndicator.Add("lblSourceComparativeParameters", "Source 2");
            DictionarySelectIndicator.Add("lblLevels", "Levels");
            DictionarySelectIndicator.Add("lblMinAf", "Min AF");
            DictionarySelectIndicator.Add("lblMaxAf", "Max AF");
            DictionarySelectIndicator.Add("lblRatOfChg", "Rat of Chg");
            DictionarySelectIndicator.Add("lblShiftParameters", "Shift");
            DictionarySelectIndicator.Add("lblBarHistoryParameters", "Bar History");
            DictionarySelectIndicator.Add("lblKPeriodsParameters", "%K Periods");
            DictionarySelectIndicator.Add("lblKSlowingParameters", "%K Slowing");
            DictionarySelectIndicator.Add("lblDPeriodsParameters", "%D Periods");
            DictionarySelectIndicator.Add("lblDType", "%D MA Type");
            DictionarySelectIndicator.Add("lblKSmooth", "%KSmooth");
            DictionarySelectIndicator.Add("lblKDblSmooth", "%K Dbl Smooth");
            DictionarySelectIndicator.Add("lblTemplates", "Templates");
            DictionarySelectIndicator.Add("lblDefault", "Default");
            DictionarySelectIndicator.Add("lblRename", "Rename");
            DictionarySelectIndicator.Add("lblVariation", "Variation(%)");
            DictionarySelectIndicator.Add("lblThreshold1", "Lower Threshold");
            DictionarySelectIndicator.Add("lblThreshold2", "Upper Threshold");
            DictionarySelectIndicator.Add("pgrdIndicators", "Window");
            DictionarySelectIndicator.Add("gprAverage", "Average");
            DictionarySelectIndicator.Add("gprValue", "Value");
            DictionarySelectIndicator.Add("gprWindow", "Window");
            DictionarySelectIndicator.Add("gprHistogram", "Histogram");
            DictionarySelectIndicator.Add("gprParameters", "Parameters");
            DictionarySelectIndicator.Add("gprKParameters", "%K Parameters");
            DictionarySelectIndicator.Add("gprDParameters", "%D Parameters");
            DictionarySelectIndicator.Add("gprView", "View");
            DictionarySelectIndicator.Add("gprThresholds", "Thresholds");
            // Indicator's names
            DictionarySelectIndicator.Add("MA", "MA");
            DictionarySelectIndicator.Add("Moving Average", "MA Moving Average");
            DictionarySelectIndicator.Add("MACD", "MACD");
            DictionarySelectIndicator.Add("MACD Histogram", "MACD Histogram");
            DictionarySelectIndicator.Add("Volume", "Volume");
            DictionarySelectIndicator.Add("Moving Average Envelope", "MAE Moving A. Envelope");
            DictionarySelectIndicator.Add("Bollinger Bands", "BB Bollinger Bands");
            DictionarySelectIndicator.Add("Relative Strength Index", "RSI Relative Strength Index");
            DictionarySelectIndicator.Add("HILO Activator", "HILO Activator");
            DictionarySelectIndicator.Add("Typical Price", "TP Typical Price");
            DictionarySelectIndicator.Add("Parabolic SAR", "PSAR Parabolic SAR");
            DictionarySelectIndicator.Add("Stochastic Momentum Index", "SMI Stochastic Momentum Index");
            DictionarySelectIndicator.Add("Stochastic Oscillator", "SO Stochastic Oscillator");
            DictionarySelectIndicator.Add("Aroon Oscillator", "AO Aroon Oscillator");
            DictionarySelectIndicator.Add("Money Flow Index", "MFI Money Flow Index");
            DictionarySelectIndicator.Add("Simple Moving Average", "SMA Simple Moving Average");
            DictionarySelectIndicator.Add("Accumulative Swing Index", "ASI Accumulative Swing Index");
            DictionarySelectIndicator.Add("Exponential Moving Average", "EMA Exponential Moving Average");
            DictionarySelectIndicator.Add("Chaikin Volatility", "CV Chaikin Volatility");
            DictionarySelectIndicator.Add("Historical Volatility", "HV Historical Volatility");
            DictionarySelectIndicator.Add("Chande Momentum Oscillator", "CMO Chande Momentum Oscillator");
            DictionarySelectIndicator.Add("Welles Wilder Smoothing", "WWS Welles Wilder Smoothing");
            DictionarySelectIndicator.Add("Vertical Horizontal Filter", "VHF Vertical Horizontal Filter");
            DictionarySelectIndicator.Add("Linear Regression R-Squared", "LRRS Linear Regression R-Squared");
            DictionarySelectIndicator.Add("Linear Regression Forecast", "LRF Linear Regression Forecast");
            DictionarySelectIndicator.Add("Linear Regression Slope", "LRS Linear Regression Slope");
            DictionarySelectIndicator.Add("Linear Regression Intercept", "LRI Linear Regression Intercept");
            DictionarySelectIndicator.Add("Momentum Oscillator", "MO Momentum Oscillator");
            DictionarySelectIndicator.Add("ADX", "ADX");
            DictionarySelectIndicator.Add("DI+/DI-", "DI+/DI-");
            DictionarySelectIndicator.Add("DI+/DI-/ADX", "DI+/DI-/ADX");
            DictionarySelectIndicator.Add("Commodity Channel Index", "CCI Commodity Channel Index");
            DictionarySelectIndicator.Add("Detrended Price Oscillator", "DPO Detrended Price Oscillator");
            DictionarySelectIndicator.Add("Mass Index", "MI Mass Index");
            DictionarySelectIndicator.Add("On Balance Volume", "OBV On Balance Volume");
            DictionarySelectIndicator.Add("Price ROC", "PROC Price ROC");
            DictionarySelectIndicator.Add("Price Volume Trend", "PVT Price Volume Trend");
            DictionarySelectIndicator.Add("Williams %R", "Williams %R");
            DictionarySelectIndicator.Add("Williams Accumulation Distribution", "WAD Williams Accumulation Distribution");
            DictionarySelectIndicator.Add("Wheighted Close", "WC Wheighted Close");
            DictionarySelectIndicator.Add("Volume ROC", "VROC Volume ROC");
            DictionarySelectIndicator.Add("Fractal Chaos Oscillator", "FCO Fractal Chaos Oscillator");
            DictionarySelectIndicator.Add("Fractal Chaos Bands", "FCB Fractal Chaos Bands");
            DictionarySelectIndicator.Add("Ultimate Oscillator", "UO Ultimate Oscillator");
            DictionarySelectIndicator.Add("True Range", "TR True Range");
            DictionarySelectIndicator.Add("Rainbow Oscillator", "RO Rainbow Oscillator");
            DictionarySelectIndicator.Add("Accumulation/Distribution", "A/D Accumulation Distribution");

        }

        public override sealed void LoadSelectStudy()
        {
            DictionarySelectIndicator.Add("FrmSelectStudyTitle", "Study Lines");
            DictionarySelectIndicator.Add("lblToolsStudy", "Tools");
            DictionarySelectIndicator.Add("gprStudyColor", "Color");
            DictionarySelectIndicator.Add("gprStudyParameters", "Parameters");
            DictionarySelectIndicator.Add("gprStudyLineStyle", "Line Style");
            DictionarySelectIndicator.Add("gprStudyLineThickness", "Line Thickness");
            DictionarySelectIndicator.Add("gprStudyRightExtension", "Right Extension");
            DictionarySelectIndicator.Add("gprStudyLeftExtension", "Left Extension");
            DictionarySelectIndicator.Add("gprStudyExtension", "Extension");
            DictionarySelectIndicator.Add("gprStudyValue", "Value");
            DictionarySelectIndicator.Add("StudyLine", "Study Line");
            DictionarySelectIndicator.Add("TrendLine", "Trend Line");
            DictionarySelectIndicator.Add("RayLine", "Ray Line");
            DictionarySelectIndicator.Add("VerticalLine", "Vertical Line");
            DictionarySelectIndicator.Add("HorizontalLine", "Horizontal Line");
            DictionarySelectIndicator.Add("Rectangle", "Rectangle");
            DictionarySelectIndicator.Add("Ellipse", "Ellipse");
            DictionarySelectIndicator.Add("Arrow", "Arrow");
            DictionarySelectIndicator.Add("FreeHand", "Free Hand");
            DictionarySelectIndicator.Add("Polyline", "Polyline");
            DictionarySelectIndicator.Add("Channel", "Canal");
            DictionarySelectIndicator.Add("Fibonacci Arcs", "Fibonacci Arcs");
            DictionarySelectIndicator.Add("Fibonacci Fan", "Fibonacci Fan");
            DictionarySelectIndicator.Add("Fibonacci Projections", "Fibonacci Projections");
            DictionarySelectIndicator.Add("Fibonacci Retracements", "Fibonacci Retracements");
            DictionarySelectIndicator.Add("Fibonacci Timezones", "Fibonacci Time-Zones");
            DictionarySelectIndicator.Add("Speed Line", "Linha de Velocidade");
            DictionarySelectIndicator.Add("Gann Fan", "Gann Fan");
        }

        public override sealed void LoadBrowser()
        {
            DictionaryBrowser.Add("tsbUndo", "Back");
            DictionaryBrowser.Add("tsbRedo", "Forward");
            DictionaryBrowser.Add("tsbRefresh", "Refresh");
            DictionaryBrowser.Add("tsbPageHome", "Home");
            DictionaryBrowser.Add("tsbNavigate", "Go to");
        }

        public override sealed void LoadSelectTools()
        {
            DictionarySelectTools.Add("newChart", "New Open Graph");
            DictionarySelectTools.Add("editPortfolios", "Edit Portfolios");
            DictionarySelectTools.Add("viewChart", "View Chart");
            DictionarySelectTools.Add("columnSymbol", "Symbol");
            DictionarySelectTools.Add("columnLast", "Last");
            DictionarySelectTools.Add("columnVariation", "Variation");
            DictionarySelectTools.Add("columnVolume", "Volume");
            DictionarySelectTools.Add("columnTime", "Time");
            DictionarySelectTools.Add("columnOpen", "Open");
            DictionarySelectTools.Add("columnHigh", "High");
            DictionarySelectTools.Add("columnLow", "Low");
            DictionarySelectTools.Add("columnClose", "Close");
            DictionarySelectTools.Add("columnTrades", "Trades");
            DictionarySelectTools.Add("selectTools", "Select Tools");
            DictionarySelectTools.Add("tabDetails", "Details");
            DictionarySelectTools.Add("tabPerformance", "Volume");
            DictionarySelectTools.Add("tabAll", "All");
        }

        public override sealed void LoadSettings()
        {
            DictionarySettings.Add("frmOptions", "Options");
            DictionarySettings.Add("msgSaved", "Settings succesfully saved!");

            //Button's
            DictionarySettings.Add("btnApply", "Apply");
            DictionarySettings.Add("btnOk", "Ok");
            DictionarySettings.Add("btnCancel", "Cancel");
            DictionarySettings.Add("cbApplyAll", "Change all opened charts");

            //Chart
            DictionarySettings.Add("grpNumberCandles", "Number Candles");
            DictionarySettings.Add("grpAppearance", "Appearance");
            DictionarySettings.Add("grpBehavior", "Behavior");
            DictionarySettings.Add("grpTabData", "Data Flap");

            DictionarySettings.Add("tabChart", "Chart");
            DictionarySettings.Add("cbxSemiLogScale", "Semi-Log Scale");
            DictionarySettings.Add("cbxPanelSeparator", "Panel Separator");
            DictionarySettings.Add("cbxGridVertical", "Vertical Grid");
            DictionarySettings.Add("cbxGridHorizontal", "Horizontal Grid");
            DictionarySettings.Add("lblPaddingTop", "Padding-Top");
            DictionarySettings.Add("lblPaddingBottom", "Padding-Bottom");
            DictionarySettings.Add("lblPaddingRight", "Padding-Right");
            DictionarySettings.Add("lblChartViewport", "Chart Viewport");
            DictionarySettings.Add("lblDecimals", "Decimals");
            DictionarySettings.Add("lblChartHistory", "Chart History");
            DictionarySettings.Add("lblPositionChart", "Position");
            DictionarySettings.Add("grpChart", "Settings");
            DictionarySettings.Add("lblColorScheme", "Color Scheme");
            DictionarySettings.Add("cbxVisiblePortfolio", "Portfolio");

            //TabData
            DictionarySettings.Add("LessChart", "Less");
            DictionarySettings.Add("BottomChart", "Bottom");

            //Colors Scheme 
            DictionarySettings.Add("SchemeSky", "Light");
            DictionarySettings.Add("SchemeWhite", "White");
            DictionarySettings.Add("SchemeBlue", "Blue");
            DictionarySettings.Add("SchemeBeige", "Beige");
            DictionarySettings.Add("SchemeDark", "Dark");
            DictionarySettings.Add("SchemeGreen", "Green");
            DictionarySettings.Add("SchemeMono", "Mono");

            //Studies
            DictionarySettings.Add("tabStudies", "Studies");
            DictionarySettings.Add("grpStudies", "Settings");
            DictionarySettings.Add("lblLineThickness", "Line Thickness");
            DictionarySettings.Add("lblColor", "Color");
            DictionarySettings.Add("columnRetracements", "Retracements");
            DictionarySettings.Add("columnProjections", "Projections");
            DictionarySettings.Add("lblFibonacci", "Fibonacci");

            //Color
            DictionarySettings.Add("btnAssociate", "Locked/Unlocked Back Gradient");
            DictionarySettings.Add("tabColor", "Colors");
            DictionarySettings.Add("grpColors", "Settings");
            DictionarySettings.Add("lblBackGradientBottom", "Back Gradient Bottom");
            DictionarySettings.Add("lblBackGradientTop", "Back Gradient Top");
            DictionarySettings.Add("lblGridColor", "Grid");
            DictionarySettings.Add("lblDownColor", "Candle Down");
            DictionarySettings.Add("lblUpColor", "Candle Up");
            DictionarySettings.Add("lblScale", "Scale");
            DictionarySettings.Add("lblChartBackColor", "Chart Back");
            DictionarySettings.Add("lblPainelSeparatorColor", "Painel Separator");
            DictionarySettings.Add("lblCandleBorderColor", "Candle Border");
            DictionarySettings.Add("cbxOverwriteAll", "Overwrite All");
            DictionarySettings.Add("grpOverwrite", "Overwrite Theme");

            //Server
            DictionarySettings.Add("tabServer", "Server");
            DictionarySettings.Add("lblPort1", "Port 1");
            DictionarySettings.Add("lblPort2", "Port 2");
            DictionarySettings.Add("lblPort3", "Port 3");
            DictionarySettings.Add("lblServer1", "Server 1");
            DictionarySettings.Add("lblServer2", "Server 2");
            DictionarySettings.Add("lblServer3", "Server 3");
            DictionarySettings.Add("grpSettingsServer", "Settings");

            //Proxy
            DictionarySettings.Add("tabProxy", "Connection");
            DictionarySettings.Add("grpSettingsProxy", "Settings");
            DictionarySettings.Add("optNotProxy", "Do not use proxy server");
            DictionarySettings.Add("optConfigProxyNavigator", "Use proxy settings of your web browser");
            DictionarySettings.Add("optProxyServer", "Use the following proxy server");
            DictionarySettings.Add("Address", "Address");
            DictionarySettings.Add("Port", "Port");
            DictionarySettings.Add("optProxySocks", "Use following SOCKS proxy:");
            DictionarySettings.Add("cbxAuthenticationProxy", "Requires authentication");
            DictionarySettings.Add("lblUser", "User");
            DictionarySettings.Add("lblPassword", "Password");

            //Price
            DictionarySettings.Add("tabPrice", "Price's");
            DictionarySettings.Add("grpPrice", "Settings");
            DictionarySettings.Add("grpLine", "Line");
            DictionarySettings.Add("grpBar", "Bar");
            DictionarySettings.Add("grpSmoothed", "HA Smoothed");
            DictionarySettings.Add("lblMonoColor", "Mono/Color");
            DictionarySettings.Add("lblLineThicknessLine", "Line Thickness");
            DictionarySettings.Add("lblLineThicknessBar", "Line Thickness");
            DictionarySettings.Add("lblPeriodsSmoothed", "Periods");
            DictionarySettings.Add("lblTypeAverageSmoothed", "Type Average");

            //User
            DictionarySettings.Add("tabUser", "User");
            DictionarySettings.Add("grpBasic", "Register");
            DictionarySettings.Add("grpOptional", "Informations");
            DictionarySettings.Add("lblFirstName", "Name");
            DictionarySettings.Add("lblLastName", "Surname");
            DictionarySettings.Add("lblCPF", "CPF");
            DictionarySettings.Add("lblEmail", "Email");
            DictionarySettings.Add("lblUserName", "Login");
            DictionarySettings.Add("lblOldPassword", "Old Password");
            DictionarySettings.Add("lblNewPassword", "New Password");
            DictionarySettings.Add("lblNewPassword2", "Confirm New Password");
            DictionarySettings.Add("lblBirthday", "Birthday");
            DictionarySettings.Add("lblCEP", "CEP");
            DictionarySettings.Add("lblState", "State");
            DictionarySettings.Add("lblCity", "City");
            DictionarySettings.Add("lblDistrict", "District");
            DictionarySettings.Add("lblStreet", "Street");
            DictionarySettings.Add("lblNumber", "Number");
            DictionarySettings.Add("lblComplement", "Complement");
        }

        public override sealed void LoadPortfolio()
        {
            DictionaryPortfolio.Add("lblPortfolio", "Portfolio");
            DictionaryPortfolio.Add("PortfolioExists", "doesn't exist, please type a valid portfolio!");
            DictionaryPortfolio.Add("fieldEmptyPortfolio", "The portfolio entered can not be empty");
            DictionaryPortfolio.Add("frmEditPortfolio", "Edit Portfolios");
            DictionaryPortfolio.Add("toolTipAddPortfolio", "New Portfolio");
            DictionaryPortfolio.Add("toolTipDeletePortfolio", "Delete Portfolio");
            DictionaryPortfolio.Add("lblSearchPort", "Search");
            DictionaryPortfolio.Add("toolTipAddPortList", "Add");
            DictionaryPortfolio.Add("toolTipRemovePortList", "Remove");
            DictionaryPortfolio.Add("btnOk", "Ok");
            DictionaryPortfolio.Add("btnApply", "Apply");
            DictionaryPortfolio.Add("btnCancel", "Cancel");
            DictionaryPortfolio.Add("ConfirmSavePortfolio", "Save changes in");
            DictionaryPortfolio.Add("ConfirmDeletePortfolio", "Confirm portfolio deletion?");
            DictionaryPortfolio.Add("portfolioExisted", "The name {0} has been used already on another portfolio!");
            DictionaryPortfolio.Add("portfolioCreated", "New portfolio {0} created!");
            DictionaryPortfolio.Add("portfolioRenameExist", "This name has been used already on another portfolio!");
            DictionaryPortfolio.Add("portfolioRenameInvalid", "This name isn't valid!");
            DictionaryPortfolio.Add("FormPortfolio", "New Portfolio");
            DictionaryPortfolio.Add("FormRanameTab", "Rename");
        }

        public override sealed void LoadLogin()
        {
            DictionaryLogin.Add("frmLogin", "Plena Login");
            DictionaryLogin.Add("lblLogin", "Login");
            DictionaryLogin.Add("lblPassword", "Password");
            DictionaryLogin.Add("btnGuest", "Login as guest");
            DictionaryLogin.Add("lblForgotPassword", "Forgot Password");
            DictionaryLogin.Add("lblRegister", "Register");
            DictionaryLogin.Add("btnOk", "Ok");
            DictionaryLogin.Add("invalidLogin", "Login and password invalid");
            DictionaryLogin.Add("blockedLogin", "Locked system to access.");
            DictionaryLogin.Add("txtLoginEmpty", "Enter the login field");
            DictionaryLogin.Add("txtPasswordEmpty", "Enter the password field");
            DictionaryLogin.Add("notConnectServer", "Could not connect to server. Try again later.");
            DictionaryLogin.Add("lblVersion", "Version Beta 2014");
            DictionaryLogin.Add("lblStatus", "Enter your login and password...");
            DictionaryLogin.Add("cbxLembrar", "Remember");
            DictionaryLogin.Add("cbxOffline", "Login Offline");
            DictionaryLogin.Add("messageLogin", "Usuário {0} iniciando sessão {1}.");
            DictionaryLogin.Add("ServerValidate", "Validating login and password on the Server.");
            DictionaryLogin.Add("ValidationSuccessful", "Validation was successful.");
            DictionaryLogin.Add("InsertLogin", "Enter your username and password...");
            DictionaryLogin.Add("OpeningDB", "Opening data base...");
            DictionaryLogin.Add("ImpossibleAtMoment", "It was impossible at the moment...");
            DictionaryLogin.Add("ImpossibleAccessDB", "It was impossible to connect to the data base and the application will be closed. If this error persists, contact suporte@plena-tp.com.br.");
            DictionaryLogin.Add("LoadingDB", "Loading Data Base...");
            DictionaryLogin.Add("LoadingChartCommands", "Loading chart of the command line...");
            DictionaryLogin.Add("LoadingDM", "Loading Data Manager...");
            DictionaryLogin.Add("CheckingWorkspace", "Checking workspace...");
            DictionaryLogin.Add("LoadWebBrowser", "Loading Web Browser...");
            DictionaryLogin.Add("LoadLastWorkspace", "Loading last workspace saved...");
            DictionaryLogin.Add("msgGuestInfo", "Starting as a guest...");
            DictionaryLogin.Add("msgGuestInvalid", "Not Starting as a guest...");
            DictionaryLogin.Add("msgGuestError", "Invalid value for guests!");
            DictionaryLogin.Add("msgSymbolGuest", "Guest can't access ");
        }

        public override sealed void LoadPermission()
        {
            DictionaryPermission.Add("unauthorizedAccess", "Unauthorized Access");
        }

        public override sealed void LoadTemplate()
        {
            DictionaryTemplate.Add("frmTemplate", "Templates");
            DictionaryTemplate.Add("lblDescription", "Description");
            DictionaryTemplate.Add("cbxDefault", "Default");
        }

        public override sealed void LoadWorkspace()
        {
            DictionaryWorkspace.Add("workspace", "Workspace");
            DictionaryWorkspace.Add("frmWorkspace", "Manager Workspace");
            DictionaryWorkspace.Add("btnDismiss", "Dismiss");
            DictionaryWorkspace.Add("mnuLoadWorkspace", "Load");
            DictionaryWorkspace.Add("mnuDefaultWorkspace", "Default");
            DictionaryWorkspace.Add("mnuRenameWorkspace", "Rename");
        }

        public override sealed void LoadOutput()
        {
            DictionaryOutput.Add("tabConnection", "Connection Status");
            DictionaryOutput.Add("tabAlerts", "Alerts & Messages");
        }
    }
}