using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using M4.M4v2.Settings;
using STOCKCHARTXLib;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using M4Data.List;
using M4Core.Entities;

namespace M4.M4v2.Chart
{
    public partial class ndtChartTools : UserControl
    {
        public CtlPainelChart MActiveChart;
        private frmMain2 frmMain;
        public bool _mCrossHairToggle = false;
        public bool _mSelectToggle = false;
        public bool _mDeltaToggle = false;
        public bool _mMagneticToggle = false;

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
            LINECOLOR,
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

        public ndtChartTools(CtlPainelChart ctlPainelChartParent, frmMain2 frmMain2)
        {
            frmMain = frmMain2;
            MActiveChart = ctlPainelChartParent;
            InitializeComponent();
            cmdDeltaCursor.ToggleStateChanging -= cmdDeltaCursor_ToggleStateChanging;
            cmdSelect.ToggleStateChanging -= cmdSelect_ToggleStateChanging;
            cmdCrosshair.ToggleStateChanging -= cmdCrosshair_ToggleStateChanging;
            cmdMagnetic.ToggleStateChanging -= cmdMagnetic_ToggleStateChanging;
            cmdDeltaCursor.ToggleState = frmMain2.cmdDeltaCursor.ToggleState;
            cmdSelect.ToggleState = frmMain2.cmdSelect.ToggleState;
            cmdCrosshair.ToggleState = frmMain2.cmdCrosshair.ToggleState;
            cmdMagnetic.ToggleState = frmMain2.cmdMagnetic.ToggleState;
            cmdDeltaCursor.ToggleStateChanging += cmdDeltaCursor_ToggleStateChanging;
            cmdSelect.ToggleStateChanging += cmdSelect_ToggleStateChanging;
            cmdCrosshair.ToggleStateChanging += cmdCrosshair_ToggleStateChanging;
            cmdMagnetic.ToggleStateChanging += cmdMagnetic_ToggleStateChanging;
            MActiveChart.popout = true;
            MActiveChart.ChartToolsPopOut = this;
            SetupButtons();
        }

        private  void SetupButtons()
        {
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
            frmMain._actionChart = frmMain2.ActionChart.TEXTOBJECT;
                        //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.TEXTOBJECT;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserDefinedText("My Text " + DateTime.Now);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.Focus();
                }
            }
        }

        public void cmdCrosshair_Click(object sender, EventArgs args)
        {
            if (cmdCrosshair.ToggleState == ToggleState.On) return;
            LoadViewCrosshair();
        }

        public void cmdSelect_Click(object sender, EventArgs e)
        {
            if (cmdSelect.ToggleState == ToggleState.On) return;
            LoadViewCrosshair();
        }

        public void cmdDeltaCursor_Click(object sender, EventArgs args)
        {
            LoadDeltaCursor();
        }

        public void cmdMagnetic_Click(object sender, EventArgs args)
        {

            bool @checked = cmdMagnetic.ToggleState == ToggleState.On ? false : true;
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.Magnetic = @checked;
                }
            }
            _mMagneticToggle = true;
            frmMain._mMagneticToggle = true;
            frmMain.mnuViewMagnetic.IsChecked = @checked;

            cmdMagnetic.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
            frmMain.cmdMagnetic.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
        }

        public void cmdBuySymbol_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.BUYSYMBOL;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.BUYSYMBOL;
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
            frmMain._actionChart = frmMain2.ActionChart.SELLSYMBOL;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.SELLSYMBOL;
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
            frmMain._actionChart = frmMain2.ActionChart.EXITSYMBOL;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.EXITSYMBOL;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserSymbolObject(SymbolType.soExitSymbolObject,
                                                                   "Exit " +
                                                                   Convert.ToString(((CtlPainelChart)window.Controls[0]).StockChartX1.GetObjectCount(
                                                                                      ObjectType.otExitSymbolObject)), "");
                }
            }
        }

        private void cmdLineColor_Click(object sender, EventArgs e)
        {
            RadColorDialogForm colorBox = new RadColorDialogForm();
            frmMain.configStudies = ListConfigStudies.Instance().LoadListConfigStudies();
            colorBox.SelectedColor = frmMain.configStudies.Color;
            if(colorBox.ShowDialog() == DialogResult.OK)
            {
                frmMain.configStudies.Color = colorBox.SelectedColor;
            }
            ListConfigStudies.Instance().Update(frmMain.configStudies);
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    CtlPainelChart ctlPanel = (CtlPainelChart)window.Controls[0];
                    ctlPanel.StockChartX1.LineColor = frmMain.configStudies.Color;
                }
            }
        }

        public void cmdTrendLine_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.TRENDLINE;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.TRENDLINE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserTrendLine("TL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdRay_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.RAY;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.RAY;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsRay, "RL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdChannel_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.CHANNEL;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.CHANNEL;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    string dateTimeNow = Convert.ToString(DateTime.Now);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH1 " + dateTimeNow);
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsChannel, "CH2 " + dateTimeNow);
                }
            }
        }

        public void cmdVerticalLine_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.YLINE;
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.YLINE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserYLine("VL " + DateTime.Now.Ticks);
                }
            }
        }

        public void cmdHorizontalLine_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.XLINE;            
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.XLINE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.AddUserXLine("HL " + DateTime.Now.Ticks);
                }
            }
        }

        public void cmdRectangle_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.RECTANGLE;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.RECTANGLE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsRectangle, "RE " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdEllipse_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.ELIPSE;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.ELIPSE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsEllipse, "EL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdArrow_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.ARROW;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.ARROW;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsTriangle, "AR " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdPolyline_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.POLYLINE;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.POLYLINE;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsPolyline, "PL " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciRetracements_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.FIBONACCIRETRACEMENTS;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.FIBONACCIRETRACEMENTS;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciRetracements, "FR " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciProjections_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.FIBONACCIPROJECTIONS;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.FIBONACCIPROJECTIONS;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciProgression, "FP" + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciArcs_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.FIBONACCIARCS;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.FIBONACCIARCS;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciArcs, "FA " + Convert.ToString(DateTime.Now));
                }
            }
        }

        public void cmdFibonacciFan_Click(object sender, EventArgs e)
        {
            frmMain._actionChart = frmMain2.ActionChart.FIBONACCIFAN;
                                    //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0])._actionChart = frmMain2.ActionChart.FIBONACCIFAN;
                    ((CtlPainelChart)window.Controls[0]).MUserEditing = true;
                    ((CtlPainelChart)window.Controls[0]).DrawingLineStudy = true;
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DrawLineStudy(StudyType.lsFibonacciFan, "FF " + Convert.ToString(DateTime.Now));
                }
            }
        }
        
        public void LoadViewScaleType()
        {
            bool @checked = MActiveChart.StockChartX1.ScaleType == ScaleType.stLinearScale;
            MActiveChart.StockChartX1.ScaleType = !@checked ? ScaleType.stLinearScale : ScaleType.stSemiLogScale;
            MActiveChart.StockChartX1.ResetYScale(0);

            //MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowXGrid()
        {
            bool @checked = !MActiveChart.StockChartX1.XGrid;
            MActiveChart.StockChartX1.XGrid = @checked;

            //MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewShowYGrid()
        {
            bool @checked = !MActiveChart.StockChartX1.YGrid;
            MActiveChart.StockChartX1.YGrid = @checked;

            //MActiveChart.LoadCheckMenuRight();
        }

        public void LoadViewSeparators()
        {
            bool @checked = !MActiveChart.StockChartX1.HorizontalSeparators;
            MActiveChart.StockChartX1.HorizontalSeparators = @checked;

            //MActiveChart.LoadCheckMenuRight();
        }

        public void LoadDarvasBoxes()
        {
            bool _checked = MActiveChart.StockChartX1.DarvasBoxes;
            MActiveChart.StockChartX1.DarvasBoxes = !_checked;

            //MActiveChart.LoadCheckMenuRight();
        }


        public void LoadViewCrosshair()
        {
            bool @checked = cmdCrosshair.ToggleState == ToggleState.On ? false : true;
            frmMain.mnuViewCrossHair.IsChecked = @checked;
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
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
            frmMain._mCrossHairToggle = true;
            frmMain._mSelectToggle = true;
            frmMain.cmdCrosshair.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
            frmMain.cmdSelect.ToggleState = @checked ? ToggleState.Off : ToggleState.On;

        }

        public void LoadDeltaCursor()
        {

            bool @checked = cmdDeltaCursor.ToggleState == ToggleState.On ? false : true;
            //Change all charts status:
            foreach (DockWindow window in frmMain.radDock2.DockWindows)
            {
                if (window.AccessibleName.Equals("CtlPainelChart"))
                {
                    ((CtlPainelChart)window.Controls[0]).StockChartX1.DeltaCursor = @checked;
                }
            }
            _mDeltaToggle = true;
            frmMain._mDeltaToggle = true;

            cmdDeltaCursor.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
            frmMain.cmdDeltaCursor.ToggleState = @checked ? ToggleState.On : ToggleState.Off;
        }

        public void LoadSchemeClick(string value)
        {
            foreach (KeyValuePair<string, string> scheme in
                Scheme.Instance().Schemes.Where(scheme => scheme.Value.Equals(value)))
            {
                MActiveChart.m_SchemeColor = scheme.Key;
            }

            Scheme.Instance().UpdateChartColors(MActiveChart.StockChartX1, MActiveChart.m_SchemeColor);

            //foreach (RadMenuItem nCommand in
            //        frmMain2.GInstance.mnuChartColors.Items.Where(nCommand => nCommand.Text.ToUpper().Equals(s.Text.ToUpper())))
            //{
            //    ((RadMenuItem)s).IsChecked = nCommand.IsChecked;
            //}

            foreach (RadMenuItem nCommand in frmMain2.GInstance.mnuChartColors.Items)
                nCommand.IsChecked = nCommand.Text.ToUpper().Equals(value.ToUpper());

            MActiveChart.LoadCheckMenuRight();
        }

    }
}
