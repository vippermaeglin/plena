using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DSPlena;
using M4.DataServer.Interface;
using M4.M4v2.Chart;
using M4.M4v2.Portfolio;
using M4.AsyncOperations;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;
using M4Data.MessageService;
using Telerik.Charting.Styles;
using Telerik.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using Telerik.Charting;
using Telerik.WinControls.UI.Docking;
using Periodicity = M4.DataServer.Interface.Periodicity;
using M4.M4v2.Chart.IndicatorElements;
using M4.M4v2.Settings;
using System.Diagnostics;
using System.Data.SqlClient;

namespace M4.M4v2.GridviewRowDetailsExtended.GridViewRT
{
    public partial class GridDataAssetsRT : UserControl
    {
        RadChart chart = new RadChart();
        public PortfolioDataSet dataSource = new PortfolioDataSet();
        private static bool suspended = false;
        private static bool contextmenuopened = false;
        private Point _point;
        // private static bool ChildExpanded = false;
        public AutoResetEvent eventsSync = new AutoResetEvent(true);
        public AutoResetEvent SetSenderSync = new AutoResetEvent(true);
        public List<string> assetList = new List<string>();
        private bool eventSyncEnable = true;
        private string _searchAsset = "";
        private readonly SelectView1 _parentControl;
        private RadDropDownMenu _menu;
        private Dictionary<string, CellFormattingEventArgs> _cellsVolumeChart = new Dictionary<string, CellFormattingEventArgs>();
        public int _selectedViewIndex;
        private Point lastClick;



        public GridDataAssetsRT(SelectView1 parentcontrol, List<string> Symbols)
        {
            InitializeComponent();
            Symbols.Remove("");
            assetList = Symbols;
            _parentControl = parentcontrol;
            OnLoad();
            //this.SelectedControl = this.radGridView1;
            radGridView1.EnableAlternatingRowColor = true;
            //radGridView1.TableElement.AlternatingRowColor = Color.FromArgb(218, 222, 234, 242);
            ((GridTableElement)this.radGridView1.TableElement).AlternatingRowColor = Color.FromArgb(226, 234, 241, 247);

            // ((GridTableElement)this.radGridView1.TableElement).AlternatingRowColor = Color.FromArgb(226, 234, 241, 247);
        }

        #region Event handlers

        /*protected override*/
        public void OnLoad(/*EventArgs e*/)
        {
            //base.OnLoad(e);

            this.radGridView1.BeginUpdate();

            ManageGrid();

            radGridView1.GridBehavior = new RadGridBehavior();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Fill Plena Data Source:
            /*dataSource.Add(new RTAssetsInfo()
            {
                Close = 31.18,
                High = 42.9,
                Last = 31.18,
                Low = 28.5,
                Open = 29.2,
                Position = 0,
                Symbol = "AALC11B",
                Time = DateTime.Now.ToString(),
                Trades = 4,
                Variation = 2.3,
                Volume = 42536,
                Photo = Properties.Resources.aalc11b
            }); */
            lock (dataSource)
            {
                dataSource = frmMain2.GetPortfolioView(assetList);
                /*for (int i = 0; i < assetList.Count; i++)
                {
                    //Application.DoEvents();

                    BarData bar = frmMain2.GetLastBarData(assetList[i]);
                    dataSource.Add(new RTAssetsInfo()
                                       {
                                           Close = bar.ClosePrice,
                                           High = bar.HighPrice,
                                           Last = bar.ClosePrice,
                                           Low = bar.LowPrice,
                                           Open = bar.OpenPrice,
                                           Position = i,
                                           Symbol = assetList[i],
                                           Time = frmMain2.GetLastDates(assetList[i]),
                                           Trades = bar.VolumeT,
                                           Variation =
                                               bar.OpenPrice != 0
                                                   ? (bar.ClosePrice - bar.OpenPrice) * 100 / bar.OpenPrice
                                                   : 0,
                                           Volume = frmMain2.GetLastVolumes(assetList[i]) //bar.Volume,
                                           //Photo = Properties.Resources.aalc11b
                                       });
                }*/
            }
            employeesBindingSource.DataMember = "Assets";
            employeesBindingSource.DataSource = dataSource;

            PrepareChartControl();
            LoadDetailsTable();
            LoadPerformanceTable();
            this.radGridView1.UseScrollbarsInHierarchy = true;
            this.radGridView1.ReadOnly = true;
            radGridView1.MasterTemplate.ChildViewTabsPosition = TabPositions.Bottom;
            this.radGridView1.EndUpdate();
            radGridView1.RowsChanged += radGridView1_RowsChanged;
            // Hook Up DS-Server events:
            //DataFeeder.Instance().DSForm.TickEvent += new DS_MainForm.TickEventHandler(OnTickEvent);
            //DataFeeder.Instance().DSForm.UpdateEvent += new DS_MainForm.BarDataEventHandler(OnUpdateEvent);
            DBlocalSQL.SnapshotEvent += DBDailyShared_SnapshotEvent;
            DBlocalSQL.UpdateBaseEvent += OnUpdateEvent;
            _parentControl.DecimalFormating();
        }

        void radGridView1_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                UpdateDBOrder(_parentControl.TabSelected);
        }

        public void UpdateDBOrder(string name)
        {
            int index = _parentControl.TabSelectedIndex;
            if (index == -1) return;
            List<string> updatePortfolios = new List<string>();

            bool replaced = false;

            if (name == "")
                return;
            //Create a new portfolio
            SymbolGroup portChanged = new SymbolGroup()
            {
                Name = name,
                Type = (int)DataServer.Interface.GroupType.Portfolio,
                Index = index
            };
            try
            {
                List<string> Symbols = new List<string>();
                foreach (var r in radGridView1.Rows)
                {
                    string codeParse = r.Cells["Symbol"].Value.ToString();
                    string newAsset = codeParse;
                    Symbols.Add(newAsset);
                }
                portChanged.Symbols = string.Join(",", Symbols.ToArray());
                //Replace it on User's Portfolios
                foreach (var p in _parentControl.UserPortfolios)
                {
                    if (p.Name != portChanged.Name)
                        continue;

                    updatePortfolios.Add(p.Name);

                    p.Symbols = portChanged.Symbols;
                }

                if (updatePortfolios.Count <= 0)
                    return;

                frmMain2.SaveUserPortfolios(_parentControl.UserPortfolios);
            }
            finally
            {
                eventsSync.Set();
            }
        }

        public void ReloadAssets()
        {
            lock (dataSource)
            {
                dataSource = frmMain2.GetPortfolioView(assetList);
                /*dataSource = new PortfolioDataSet();
                for (int i = 0; i < assetList.Count; i++)
                {

                    BarData bar = frmMain2.GetLastBarData(assetList[i]);
                    dataSource.Add(new RTAssetsInfo()
                                       {
                                           Close = bar.ClosePrice,
                                           High = bar.HighPrice,
                                           Last = bar.ClosePrice,
                                           Low = bar.LowPrice,
                                           Open = bar.OpenPrice,
                                           Position = i,
                                           Symbol = assetList[i],
                                           Time = bar.TradeDate.ToString(),
                                           Trades = bar.VolumeF,
                                           Variation =
                                               bar.OpenPrice != 0
                                                   ? (bar.ClosePrice - bar.OpenPrice) * 100 / bar.OpenPrice
                                                   : 0,
                                           Volume = frmMain2.GetLastVolumes(assetList[i]) //bar.Volume,
                                           // Photo = Properties.Resources.aalc11b
                                       });
                }*/
                employeesBindingSource.DataMember = "Assets";
                employeesBindingSource.DataSource = dataSource;
            }
        }

        void radGridView1_ChildViewExpanded(object sender, ChildViewExpandedEventArgs e)
        {
            try
            {
                if (e.ChildRow.ChildViewInfos.Count > 0 && e.ChildRow.ChildViewInfos[0].ChildRows.Count > 0) e.ChildRow.ChildViewInfos[0].ChildRows[0].Height = 145;
                if (e.ChildRow.ChildViewInfos.Count > 1 && e.ChildRow.ChildViewInfos[1].ChildRows.Count > 0) e.ChildRow.ChildViewInfos[1].ChildRows[0].Height = 145;
                e.ChildRow.Height = 224;
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }


        void radGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            try
            {
                //Change row color by Variation:                

                double variation = 0.000000000000000000;

                object variationValue = null;
                if (e.CellElement.ColumnInfo.OwnerTemplate.Caption != Program.LanguageDefault.DictionarySelectTools["tabDetails"]) variationValue = e.CellElement.RowInfo.Cells[2].Value;
                else
                {
                    variationValue = double.Parse(e.CellElement.RowInfo.Cells[5].Value.ToString()) - double.Parse(e.CellElement.RowInfo.Cells[2].Value.ToString());
                }
                if ((string)e.CellElement.RowInfo.Cells[0].Value == "ABEV3")
                {
                    Console.Write("ABEV3 ");
                }
                if (variationValue != null)
                {

                    if (!Convert.IsDBNull(variationValue))
                    {
                        variation = double.Parse(variationValue.ToString());
                    }
                    if (variation == 0.000000000000000000)
                    {
                        e.CellElement.ForeColor = Color.Black;
                        e.CellElement.BackColor = Color.Empty;
                    }
                    if (variation > 0.000000000000000000)
                    {
                        e.CellElement.ForeColor = Color.Green;
                        e.CellElement.BackColor = Color.Empty;
                    }
                    else if (variation < 0.000000000000000000)
                    {
                        e.CellElement.ForeColor = Color.Red;
                        e.CellElement.BackColor = Color.Empty;
                    }
                }

                GridViewDataColumn column = e.CellElement.ColumnInfo as GridViewDataColumn;
                if (column == null) return;
                if (column.OwnerTemplate.Caption != Program.LanguageDefault.DictionarySelectTools["tabDetails"] && column.OwnerTemplate.Caption != Program.LanguageDefault.DictionarySelectTools["tabPerformance"])
                {
                    if (e.CellElement.IsCurrentRow)//(e.CellElement.IsCurrent)
                    {
                        //Font newFont = new Font("Segoe UI", 8.50f, FontStyle.Bold);
                        //e.CellElement.RowInfo.Cells["Variation"].Value 
                        if (double.Parse(e.CellElement.RowInfo.Cells["Variation"].Value.ToString()) < 0)
                        {
                            e.CellElement.BackColor = Color.Red;
                            e.CellElement.ForeColor = Color.White;
                        }
                        else if (/*e.CellElement.BackColor == Color.ForestGreen || e.CellElement.ForeColor == Color.Green*/double.Parse(e.CellElement.RowInfo.Cells["Variation"].Value.ToString()) > 0)
                        {
                            e.CellElement.BackColor = Color.ForestGreen;
                            e.CellElement.ForeColor = Color.White;
                        }
                        else if (double.Parse(e.CellElement.RowInfo.Cells["Variation"].Value.ToString()) == 0 /*e.CellElement.BackColor == Color.FromArgb(136, 136, 136) || e.CellElement.ForeColor == Color.Black*/)
                        {
                            e.CellElement.BackColor = Color.FromArgb(136, 136, 136);
                            e.CellElement.ForeColor = Color.White;
                        }
                        // e.CellElement.Font = newFont;
                    }
                    /*else
                    {
                        //Font newFont = new Font("Segoe UI", 8.25f, FontStyle.Regular);
                        //e.CellElement.Font = newFont;
                        if (e.CellElement.BackColor == Color.Red)
                        {
                            e.CellElement.ForeColor = Color.Red;
                            e.CellElement.BackColor = Color.Empty;
                        }
                        else if (e.CellElement.BackColor == Color.ForestGreen)
                        {
                            e.CellElement.ForeColor = Color.Green;
                            e.CellElement.BackColor = Color.Empty;
                        }
                        else if (e.CellElement.BackColor == Color.FromArgb(136, 136, 136))
                        {
                            e.CellElement.ForeColor = Color.Black;
                            e.CellElement.BackColor = Color.Empty;
                        }
                    }*/
                }
                //if(suspended) return;
                if (column.FieldName == "Volume")
                {
                    string format = _parentControl.DecimalsFormating.Remove(_parentControl.DecimalsFormating.Length - 1);
                    if (Convert.ToDouble(e.CellElement.Value) > 1000000000)
                        e.CellElement.Text = String.Format(format.Insert(format.Length, "B}"),
                                                           (Convert.ToDouble(e.CellElement.Value) / 1000000000));
                    else if (Convert.ToDouble(e.CellElement.Value) > 1000000)
                        e.CellElement.Text = String.Format(format.Insert(format.Length, "M}"),
                                                           (Convert.ToDouble(e.CellElement.Value) / 1000000));
                    else if (Convert.ToDouble(e.CellElement.Value) > 1000)
                        e.CellElement.Text = String.Format(format.Insert(format.Length, "K}"),
                                                           (Convert.ToDouble(e.CellElement.Value) / 1000));
                }
                if (column.FieldName == "Variation" || column.FieldName == "High" || column.FieldName == "Last"
                    || column.FieldName == "Low" || column.FieldName == "Close" || column.FieldName == "Open")
                {
                    e.CellElement.Text = String.Format(_parentControl.DecimalsFormating, (Convert.ToDouble(e.CellElement.Value)));
                }
                if (column != null &&
                    column.OwnerTemplate.Caption == Program.LanguageDefault.DictionarySelectTools["tabDetails"])
                {
                    e.CellElement.ColumnInfo.DisableHTMLRendering = false;
                    if (column.FieldName == "Label Time")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        string[] date = ((string)e.CellElement.RowInfo.Cells["Time"].Value).Split(new char[] { ' ' });
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnTime"] +
                            ":</b> ";
                    }
                    if (column.FieldName == "Time")
                    {
                        string[] date = ((string)e.CellElement.Value).Split(new char[] { ' ' });
                        e.CellElement.Text = date[0] + " " + date[1];
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                    if (column.FieldName == "Label Open")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnOpen"] +
                                             ":</b> ";
                    }
                    if (column.FieldName == "Label Close")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnClose"] +
                                             ":</b> " + "";
                    }
                    if (column.FieldName == "Label High")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnHigh"] +
                                             ":</b> " + "";
                    }
                    if (column.FieldName == "Label Low")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnLow"] +
                                             ":</b> " + "";
                    }
                    if (column.FieldName == "Label Volume")
                    {
                        e.CellElement.ColumnInfo.Width = 40;
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                        e.CellElement.Text = "<html><b>" + Program.LanguageDefault.DictionarySelectTools["columnVolume"] +
                                             ":</b> " + "";
                    }
                    if (column.FieldName == "Open")
                    {
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                    if (column.FieldName == "Close")
                    {
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                    if (column.FieldName == "High")
                    {
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                    if (column.FieldName == "Low")
                    {
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                    if (column.FieldName == "Volume")
                    {
                        e.CellElement.ColumnInfo.TextAlignment = ContentAlignment.MiddleLeft;
                    }
                }
                if (column != null &&
                    column.OwnerTemplate.Caption == Program.LanguageDefault.DictionarySelectTools["tabPerformance"])
                {
                    //Add cell info to dictionary:
                    if (!_cellsVolumeChart.ContainsKey((string)e.CellElement.Value))
                        _cellsVolumeChart.Add((string)e.CellElement.Value, new CellFormattingEventArgs(e.CellElement));

                    //lock(dataSource.Assets)//if (true/*e.CellElement.RowInfo.Tag == null*/)
                    //{
                    chart.Series.Clear();
                    chart.Series.Add(
                        GetRowData(dataSource.Assets.First(s => s.Symbol == (string)e.CellElement.Value).Symbol));
                    e.CellElement.RowInfo.Tag = chart.GetBitmap();
                    //}
                    e.CellElement.Image = e.CellElement.RowInfo.Tag as Image;
                    e.CellElement.DrawBorder = false;
                    e.CellElement.DrawFill = false;
                    e.CellElement.Text = "";
                    e.CellElement.Padding = new Padding(10, 0, 0, 0);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                eventsSync.Set();
            }
        }

        void radGridView1_CreateCell(object sender, GridViewCreateCellEventArgs e)
        {
            if (suspended) return;
            if (e.CellType == typeof(GridDetailViewCellElement))
            {
                e.CellElement = new TabbedGridViewSemple.CustomDetailViewCellElement(e.Column, e.Row);
            }
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            //SetSenderSync.Reset();
            suspended = true;
            contextmenuopened = true;
            RadDropDownMenu menu = e.ContextMenu;
            int count = 0;
            foreach (RadItem VARIABLE in menu.Items)
            {

                if (!VARIABLE.Text.Contains("Clear") || VARIABLE.GetType().ToString().Contains("RadMenuSeparatorItem") /*&& count < 2*/)
                {
                    count++;
                    VARIABLE.Visibility = ElementVisibility.Collapsed;
                    continue;
                }
                if (Properties.Settings.Default.DictionaryLanguage == "PortugueseBrazil")
                    VARIABLE.Text = Program.LanguageDefault.DictionaryMenuPlena["mnuClearSorting"];
                VARIABLE.Click += new EventHandler(menu_click);
                if (VARIABLE.GetType().ToString().Contains("RadMenuItem"))
                    if (((RadMenuItem)VARIABLE).Items.Count > 0)
                        foreach (var internmenu in ((RadMenuItem)VARIABLE).Items)
                        {
                            internmenu.Click += menu_click;
                        }

            }
        }

        void menu_click(object sender, EventArgs e)
        {
            //SetSenderSync.Set();
            contextmenuopened = false;
            suspended = false;
            radGridView1.MasterTemplate.Refresh();
            radGridView1.MasterTemplate.Templates[0].Refresh();
        }

        private void radGridView1_RightClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right) return;
        }

        private void radGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                bool ignoreDragDrop = false;
                if (Math.Abs(lastClick.X - e.X) < 3 && Math.Abs(lastClick.Y - e.Y) < 3)
                {
                    ignoreDragDrop = true;
                }
                lastClick = new Point(e.X, e.Y);
                suspended = true;
                RadElement radElement = radGridView1.ElementTree.GetElementAtPoint(_point);
                if (radElement is GridHeaderCellElement)
                    return;

                if (e.Button != MouseButtons.Right)
                {
                    if (radElement is GridDataCellElement && !ignoreDragDrop)
                    {
                        int index = 0;
                        foreach (GridViewCellInfo VARIABLE in ((GridDataCellElement)radElement).RowInfo.Cells)
                        {
                            if (VARIABLE.ColumnInfo.Name.Equals("Symbol"))
                            {
                                index = VARIABLE.ColumnInfo.Index;
                            }
                        }
                        radGridView1.DoDragDrop(((GridDataCellElement)radElement).RowInfo.Cells[index].Value.ToString(), DragDropEffects.Copy);
                    }
                    return;
                }
                _menu.Show(radGridView1, e.X, e.Y);
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }

        }

        private void radGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            //SetSenderSync.WaitOne();
            if (!contextmenuopened)
            {
                suspended = false;
                if (e.Button != MouseButtons.Left)
                {
                    radGridView1.MasterTemplate.Refresh();
                    radGridView1.MasterTemplate.Templates[0].Refresh();
                }
            }
            //SetSenderSync.Set();
        }

        private void radGridView1_ChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            try
            {
                if (!e.IsExpanded)
                {
                    e.ParentRow.ActiveView = e.ParentRow.Views[_selectedViewIndex];
                }
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void radGridView1_ColumnIndexChanged(object sender, ColumnIndexChangedEventArgs e)
        {
            if (eventSyncEnable) eventsSync.Set();
        }
        #endregion

        void PrepareChartControl()
        {
            chart.Size = new Size(200, 150);
            chart.Chart.ChartTitle.Visible = false;
            chart.Legend.Visible = false;
            chart.Appearance.Border.Visible = false;
            chart.Appearance.FillStyle.MainColor = Color.White;
            chart.AutoLayout = true;
            chart.ChartTitle.Visible = false;
            chart.ChartTitle.TextBlock.Text = "";
            chart.PlotArea.Appearance.Border.Visible = false;
            chart.PlotArea.Appearance.FillStyle.FillType = FillType.Solid;
            chart.PlotArea.Appearance.FillStyle.MainColor = Color.Transparent;
            chart.PlotArea.Appearance.FillStyle.SecondColor = Color.Transparent;
            chart.PlotArea.YAxis.Appearance.CustomFormat = "C0";
            chart.PlotArea.YAxis.LabelStep = 2;
            //chart.PlotArea.YAxis.Appearance.Visible = ChartAxisVisibility.False;
            //chart.PlotArea.XAxis.Appearance.Visible = ChartAxisVisibility.False;
            chart.PlotArea.XAxis.Appearance.ValueFormat = ChartValueFormat.ShortDate;
            chart.PlotArea.XAxis.Appearance.CustomFormat = "dd";
        }

        public void LoadDetailsTable()
        {
            if (suspended) return;
            //setup the master template
            radGridView1.ThemeName = frmMain2.GInstance.m_Style;
            GridViewTextBoxColumn symbolColumn = new GridViewTextBoxColumn("Symbol");
            radGridView1.MasterTemplate.Columns.Add(symbolColumn);
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Last"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Variation"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Volume"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("Time"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("High"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Low"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Close"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Open"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Trades"));
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn("Position"));

            //Columns properties
            radGridView1.Columns["Symbol"].MinWidth = 45;
            radGridView1.Columns["Last"].MinWidth = 50;
            radGridView1.Columns["Variation"].MinWidth = 54; //62
            radGridView1.Columns["Volume"].MinWidth = 56; //58
            radGridView1.Columns["Symbol"].Width = 55; //45
            radGridView1.Columns["Last"].Width = 50; //50
            radGridView1.Columns["Variation"].Width = 54; //62
            radGridView1.Columns["Volume"].Width = 56; //58
            //Console.WriteLine("\n\nWidth columns chanded!");

            //Hide details' columns:
            radGridView1.Columns["Close"].IsVisible = false;
            radGridView1.Columns["Close"].Width = 10;
            radGridView1.Columns["Open"].IsVisible = false;
            radGridView1.Columns["Open"].Width = 10;
            radGridView1.Columns["High"].IsVisible = false;
            radGridView1.Columns["High"].Width = 10;
            radGridView1.Columns["Low"].IsVisible = false;
            radGridView1.Columns["Low"].Width = 10;
            radGridView1.Columns["Time"].IsVisible = false;
            radGridView1.Columns["Time"].Width = 10;
            radGridView1.Columns["Trades"].IsVisible = false;
            radGridView1.Columns["Trades"].Width = 10;
            radGridView1.Columns["Position"].IsVisible = false;
            radGridView1.Columns["Position"].Width = 10;

            //Translate to the current language
            radGridView1.Columns["Symbol"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnSymbol"];
            radGridView1.Columns["Last"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnLast"];
            radGridView1.Columns["Variation"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnVariation"];
            radGridView1.Columns["Volume"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnVolume"];
            radGridView1.Columns["Close"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnClose"];
            radGridView1.Columns["Open"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnOpen"];
            radGridView1.Columns["High"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnHigh"];
            radGridView1.Columns["Low"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnLow"];
            radGridView1.Columns["Time"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnTime"];
            radGridView1.Columns["Trades"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnTrades"];

            //setup the child template
            GridViewTemplate template = new GridViewTemplate();
            template.AllowAddNewRow = true;
            template.Columns.Add(new GridViewTextBoxColumn("Symbol"));
            template.Columns.Add(new GridViewTextBoxColumn("Time"));
            template.Columns.Add(new GridViewDecimalColumn("Open"));
            template.Columns.Add(new GridViewDecimalColumn("High"));
            template.Columns.Add(new GridViewDecimalColumn("Low"));
            template.Columns.Add(new GridViewDecimalColumn("Close"));
            template.Columns.Add(new GridViewDecimalColumn("Volume"));
            template.Columns.Add(new GridViewTextBoxColumn("Label Time"));
            template.Columns.Add(new GridViewTextBoxColumn("Label Open"));
            template.Columns.Add(new GridViewTextBoxColumn("Label High"));
            template.Columns.Add(new GridViewTextBoxColumn("Label Low"));
            template.Columns.Add(new GridViewTextBoxColumn("Label Close"));
            template.Columns.Add(new GridViewTextBoxColumn("Label Volume"));
            template.Caption = Program.LanguageDefault.DictionarySelectTools["tabDetails"];

            //Caracteristics child template
            template.AllowRowResize = false;
            template.AllowRowReorder = true;
            template.ShowColumnHeaders = false;
            template.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            template.Columns["Label Volume"].Width = 10;

            //template.Columns["Label Volume"].BestFit();
            template.Columns["Time"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Open"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["High"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Low"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Close"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Volume"].TextAlignment = ContentAlignment.TopLeft;

            //Translate to the current language
            template.Columns["Volume"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnVolume"];
            template.Columns["Time"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnTime"];
            template.Columns["Open"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnOpen"];
            template.Columns["High"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnHigh"];
            template.Columns["Low"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnLow"];
            template.Columns["Close"].HeaderText = Program.LanguageDefault.DictionarySelectTools["columnClose"];
            radGridView1.MasterTemplate.Templates.Add(template);

            //create the relation
            GridViewRelation relation = new GridViewRelation(radGridView1.MasterTemplate);
            relation.ChildTemplate = template;
            relation.RelationName = "SymbolsDetails";
            relation.ParentColumnNames.Add("Symbol");
            relation.ChildColumnNames.Add("Symbol");
            radGridView1.Relations.Add(relation);

            LoadUnboundData();

            //Create Details cell:
            HtmlViewDefinition viewDef = new HtmlViewDefinition();
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Label Time", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Label Open", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("Label High", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Label Low", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Label Close", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Label Volume", 0, 1, 1));
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Time", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Open", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("High", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Low", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Close", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Volume", 0, 1, 1));
            viewDef.RowTemplate.Rows[0].Height = 30;
            viewDef.RowTemplate.Rows[1].Height = 24;
            viewDef.RowTemplate.Rows[2].Height = 24;
            viewDef.RowTemplate.Rows[3].Height = 24;
            viewDef.RowTemplate.Rows[4].Height = 24;
            viewDef.RowTemplate.Rows[5].Height = 19;
            template.ViewDefinition = viewDef;
            #region OLD_Code
            /*   //Create main grid:
            DataTable table = new DataTable("Details");
            table.Columns.Add("Symbol", typeof(string));
           //table.Columns.Add("Photo", typeof(Bitmap));
            table.Columns.Add("Last", typeof(double));
            table.Columns.Add("Variation", typeof(double));
            table.Columns.Add("Volume", typeof(double));
            table.Columns.Add("Time", typeof(string));
            table.Columns.Add("High", typeof(double));
            table.Columns.Add("Low", typeof(double));
            table.Columns.Add("Close", typeof(double));
            table.Columns.Add("Open", typeof(double));
            table.Columns.Add("Trades", typeof(double));
            table.Columns.Add("Position", typeof(int));
            table.Columns.Add("Label Volume", typeof(double));
            table.Columns.Add("Label Time", typeof(string));
            table.Columns.Add("Label High", typeof(double));
            table.Columns.Add("Label Low", typeof(double));
            table.Columns.Add("Label Close", typeof(double));
            table.Columns.Add("Label Open", typeof(double));
            foreach (RTAssetsInfo row in dataSource.Assets)
            {
                table.Rows.Add(row.Symbol, row.Photo,row.Last,
                    row.Variation, row.Volume, row.Time, row.High, row.Low, row.Close, row.Open, row.Trades, row.Position);
            }

            //Create design for details' cell:
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Program.LanguageDefault.DictionarySelectTools["tabDetails"];
            template.DataSource = table;
            template.AllowRowResize = false;
            template.ShowColumnHeaders = false;
            //template.Columns["Photo"].Width = 65;
            template.Columns["Volume"].Width = 65;
            template.Columns["Time"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Open"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["High"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Low"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Close"].TextAlignment = ContentAlignment.TopLeft;
            template.Columns["Volume"].TextAlignment = ContentAlignment.TopLeft;
            this.radGridView1.Templates.Insert(0, template);


            GridViewRelation relation = new GridViewRelation(this.radGridView1.MasterTemplate);
            relation.ChildTemplate = template;
            relation.ParentColumnNames.Add("Symbol");
            relation.ChildColumnNames.Add("Symbol");
            this.radGridView1.Relations.Add(relation);

            //Create Details cell:
            HtmlViewDefinition viewDef = new HtmlViewDefinition();
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            //viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Photo", 0, 1, 6));
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Label Time", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Label Open", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("Label High", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Label Low", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Label Close", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Label Volume", 0, 1, 1));
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Time", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Open", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("High", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Low", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Close", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Volume", 0, 1, 1));
            viewDef.RowTemplate.Rows[0].Height = 30;
            template.ViewDefinition = viewDef; */
            #endregion
        }

        private void LoadUnboundData()
        {
            using (radGridView1.DeferRefresh())
            {
                foreach (RTAssetsInfo row in dataSource.Assets)
                {
                    //Info Row:
                    radGridView1.MasterTemplate.Rows.Add(row.Symbol, row.Last, row.Variation, row.Volume.Last(), row.Time.Last(),
                                                         row.High, row.Low, row.Close, row.Open, row.Trades,
                                                         row.Position);
                    //Details cell:
                    GridViewTemplate template = radGridView1.MasterTemplate.Templates[0];
                    template.Rows.Add(row.Symbol, row.Time.Last(), row.Open, row.High, row.Low, row.Close, row.Volume.Last());
                    //Volume chart:
                    /*if (radGridView1.MasterTemplate.Templates.Count > 1)
                    {
                        GridViewTemplate template2 = radGridView1.MasterTemplate.Templates[1];
                        template2.Rows.Add(row.Volume[0], row.Volume[1], row.Volume[2], row.Volume[3], row.Volume[4], row.Volume[5], row.Volume[6], row.Volume[7], row.Volume[8], row.Volume[9]);
                    }*/
                }
            }
        }

        public void ReloadDetailsTable()
        {
            try
            {
                if (eventSyncEnable)
                {
                    eventsSync.WaitOne();
                }
                this.radGridView1.MasterTemplate.Reset();
                LoadDetailsTable();
                if (suspended) return;
                radGridView1.MasterTemplate.Refresh();
                radGridView1.MasterTemplate.Templates[0].Refresh();
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }

            #region OLD_Code

            /*   GridViewTemplate template = this.radGridView1.Templates.ElementAt(0);
               this.radGridView1.Templates.RemoveAt(0);
               //Create main grid:
               DataTable table = new DataTable("Details");
               table.Columns.Add("Symbol", typeof(string));
               //table.Columns.Add("Photo", typeof(Bitmap));
               table.Columns.Add("Last", typeof(double));
               table.Columns.Add("Variation", typeof(double));
               table.Columns.Add("Volume", typeof(double));
               table.Columns.Add("Time", typeof(string));
               table.Columns.Add("High", typeof(double));
               table.Columns.Add("Low", typeof(double));
               table.Columns.Add("Close", typeof(double));
               table.Columns.Add("Open", typeof(double));
               table.Columns.Add("Trades", typeof(double));
               table.Columns.Add("Position", typeof(int));
               table.Columns.Add("Label Volume", typeof(double));
               table.Columns.Add("Label Time", typeof(string));
               table.Columns.Add("Label High", typeof(double));
               table.Columns.Add("Label Low", typeof(double));
               table.Columns.Add("Label Close", typeof(double));
               table.Columns.Add("Label Open", typeof(double));
               foreach (RTAssetsInfo row in dataSource.Assets)
               {
                   table.Rows.Add(row.Symbol, row.Photo,  row.Last,
                       row.Variation, row.Volume, row.Time, row.High, row.Low, row.Close, row.Open, row.Trades, row.Position);
               }

               //Create design for details' cell:
               template.Caption = Program.LanguageDefault.DictionarySelectTools["tabDetails"];
               template.DataSource = table;
               template.AllowRowResize = false;
               template.ShowColumnHeaders = false;
               //template.Columns["Photo"].Width = 80;
               template.Columns["Volume"].Width = 80;
               template.Columns["Time"].TextAlignment = ContentAlignment.TopLeft;
               template.Columns["Open"].TextAlignment = ContentAlignment.TopLeft;
               template.Columns["High"].TextAlignment = ContentAlignment.TopLeft;
               template.Columns["Low"].TextAlignment = ContentAlignment.TopLeft;
               template.Columns["Close"].TextAlignment = ContentAlignment.TopLeft;
               template.Columns["Volume"].TextAlignment = ContentAlignment.TopLeft;
               this.radGridView1.Templates.Insert(0, template);


               GridViewRelation relation = new GridViewRelation(this.radGridView1.MasterTemplate);
               relation.ChildTemplate = template;
               relation.ParentColumnNames.Add("Symbol");
               relation.ChildColumnNames.Add("Symbol");
               this.radGridView1.Relations[0] = relation;

               //Create Details cell:
               HtmlViewDefinition viewDef = new HtmlViewDefinition();
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               viewDef.RowTemplate.Rows.Add(new RowDefinition());
               //viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Photo", 0, 1, 6));
               viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Label Time", 0, 1, 1));
               viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Label Open", 0, 1, 1));
               viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("Label High", 0, 1, 1));
               viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Label Low", 0, 1, 1));
               viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Label Close", 0, 1, 1));
               viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Label Volume", 0, 1, 1));
               viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Time", 0, 1, 1));
               viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Open", 0, 1, 1));
               viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("High", 0, 1, 1));
               viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Low", 0, 1, 1));
               viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Close", 0, 1, 1));
               viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Volume", 0, 1, 1));
               viewDef.RowTemplate.Rows[0].Height = 30;
               template.ViewDefinition = viewDef;*/

            #endregion
        }

        public void OrderRefresh()
        {
            try
            {
                if (eventSyncEnable)
                {
                    eventsSync.WaitOne();
                }
                radGridView1.Rows.Clear();
                radGridView1.Templates[0].Rows.Clear();
                LoadUnboundData();
                if (suspended) return;
                radGridView1.MasterTemplate.Refresh();
                radGridView1.MasterTemplate.Templates[0].Refresh();
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        public void UpdateRefresh()
        {
            try
            {
                if (radGridView1.MasterTemplate.Templates.Count() == 0) return;
                if (eventSyncEnable)
                {
                    eventsSync.WaitOne();
                }
                if (suspended) return;
                radGridView1.MasterTemplate.Refresh();
                radGridView1.MasterTemplate.Templates[0].Refresh();
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        public void LoadPerformanceTable()
        {
            if (suspended) return;
            Random r = new Random();
            DataTable chartTable = new DataTable();
            chartTable.Columns.Add("Symbol", typeof(string));
            for (int i = 0; i < 10; i++)
            {
                chartTable.Columns.Add("Day" + (i + 1), typeof(int));
            }
            foreach (RTAssetsInfo row in dataSource.Assets)
            {
                DataRow dataRow = chartTable.NewRow();
                dataRow["Symbol"] = row.Symbol;
                for (int i = 0; i < 10; i++)
                {
                    //TODO: Find why volume cant be bigger than Int32
                    if (row.Volume[i] < 2000000000) dataRow[i + 1] = row.Volume[i];
                    else dataRow[i + 1] = 0;
                }
                chartTable.Rows.Add(dataRow);
            }
            GridViewTemplate template2 = new GridViewTemplate();
            template2.Caption = Program.LanguageDefault.DictionarySelectTools["tabPerformance"];
            template2.DataSource = chartTable;
            template2.AllowRowResize = false;
            template2.ShowColumnHeaders = false;
            template2.ShowRowHeaderColumn = false;
            template2.Columns[0].Width = 200;
            for (int i = 1; i < template2.Columns.Count; i++)
            {
                template2.Columns[i].IsVisible = false;
            }
            if (radGridView1.Templates.Count > 1) radGridView1.Templates.Remove(radGridView1.Templates.Last());
            this.radGridView1.Templates.Add(template2);

            GridViewRelation relation2 = new GridViewRelation(this.radGridView1.MasterTemplate);
            relation2.ChildTemplate = template2;
            relation2.ParentColumnNames.Add("Symbol");
            relation2.ChildColumnNames.Add("Symbol");
            if (radGridView1.Relations.Count > 2) radGridView1.Relations.Remove(radGridView1.Relations.Last());
            this.radGridView1.Relations.Add(relation2);
        }

        Telerik.Charting.ChartSeries GetRowData(string Symbol)
        {
            double[] Volumes = dataSource.Assets.First(a => a.Symbol == Symbol).Volume;
            string[] Dates = dataSource.Assets.First(a => a.Symbol == Symbol).Time;
            Telerik.Charting.ChartSeries series = new Telerik.Charting.ChartSeries();
            series.Type = ChartSeriesType.Bar;
            series.Name = "Volume";
            series.Appearance.LabelAppearance.Visible = false;
            double maxValue, Step;
            maxValue = 0;
            Step = 0;
            string[] volumeText = new string[10];
            for (int i = 0; i < 10; ++i)
            {
                char pos = ' ';
                maxValue = maxValue < Volumes[i] ? Volumes[i] : maxValue;
                //Normalize to a number with 2 digits:
                //Billion:
                if (Volumes[i] > 1000000000)
                {
                    pos = 'B';
                    volumeText[i] = ((int)(Volumes[i] / 1000000000)).ToString() + pos;
                    //Volumes[i] /= 1000000000;
                }
                //Million:
                else if (Volumes[i] > 1000000)
                {
                    pos = 'M';
                    volumeText[i] = ((int)(Volumes[i] / 1000000)).ToString() + pos;
                    //Volumes[i] /= 1000000;
                }
                //Thousand:
                else if (Volumes[i] > 1000)
                {
                    pos = 'K';
                    volumeText[i] = ((int)(Volumes[i] / 1000)).ToString() + pos;
                    //Volumes[i] /= 1000;
                }
                //TODO: Insert unit = K, M, B after number... 
                series.Items.Add(new ChartSeriesItem(Volumes[i]));
            }
            Step = maxValue != 0 ? maxValue / 10 : 1;
            chart.PlotArea.YAxis.AutoScale = false;
            chart.PlotArea.YAxis.AddRange(0, maxValue, Step);
            chart.PlotArea.YAxis[0].TextBlock.Text = "0";
            for (int i = 1; i <= 10; i++)
            {
                if (i * Step > 1000000000)
                {
                    chart.PlotArea.YAxis[i].TextBlock.Text = ((int)(i * Step / 1000000000)) + "B";
                }
                else if (i * Step > 1000000)
                {
                    chart.PlotArea.YAxis[i].TextBlock.Text = ((int)(i * Step / 1000000)) + "M";
                }
                else if (i * Step > 1000)
                {
                    chart.PlotArea.YAxis[i].TextBlock.Text = ((int)(i * Step / 1000)) + "k";
                }
            }
            chart.PlotArea.XAxis.AutoScale = false;
            chart.PlotArea.XAxis.AddRange(0, 9, 1);
            for (int i = 0; i < 10; i++)
            {
                if (Dates[i] != " ") chart.PlotArea.XAxis[i].TextBlock.Text = Dates[i].Substring(0, 2);
                else chart.PlotArea.XAxis[i].TextBlock.Text = " ";
            }
            return series;
        }

        protected string GetExampleDefaultTheme()
        {
            return "ControlDefault";
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {/*
            try
            {
                if (eventSyncEnable) eventsSync.WaitOne();
                //if (radGridView1.ContextMenu!=null) radGridView1.colu ContextMenu.Popup += new EventHandler(radGridView1_PopUp);
                dataSource.Employees[0].FirstName = "teste " + count;
                count++;
                if (!suspended) radGridView1.MasterTemplate.Refresh();
                if (eventSyncEnable) eventsSync.Set();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/
        }

        void DBDailyShared_SnapshotEvent(object sender, SnapshotEventArgs args)
        {
            if (args.Base != (int)BaseType.Days) return;
            try
            {
                //_eventWait.WaitOne();
                Action action = () =>
                {
                    UpdateSnapshot(args.Snapshot);
                    //_eventWait.Set();
                };
                BeginInvoke(action);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GridDataAssetsRT:OnSnapshotEvent(): " + ex.Message);
            }
        }

        public void UpdateSnapshot(SymbolSnapshot snapshot)
        {
            if (assetList.Exists(s => s == snapshot.Symbol) &&
                dataSource.Assets.Exists(s => s.Symbol == snapshot.Symbol))
            {
                RTAssetsInfo asset = dataSource.Assets.Where(a => a.Symbol == snapshot.Symbol).First();
                asset = frmMain2.GetPortfolioView(new List<string> { snapshot.Symbol }).Assets.First();
                /*asset = new RTAssetsInfo()
                {
                    Close = snapshot.Close,
                    High = snapshot.High,
                    Last = snapshot.Close,
                    Low = snapshot.Low,
                    Open = snapshot.Open,
                    Position = asset.Position,
                    Symbol = snapshot.Symbol,
                    Time = snapshot.Timestamp.ToString(),
                    Trades = snapshot.Quantity,
                    Variation = snapshot.Open != 0 ? (snapshot.Close - snapshot.Open) * 100 / snapshot.Open : 0,
                    //*TODO: Volume deriva em 3 tipos:
                      //* Ações = Quantidade de Ações
                      //* Financeiro = Quantidade de Ações x Preço
                      //* Negócios = Quantidade de Transações (1 por tick)
                      //*
                    Volume = new double[]{asset.Volume[0],asset.Volume[1],asset.Volume[2],asset.Volume[3],
																asset.Volume[4],asset.Volume[5],asset.Volume[6],asset.Volume[7],
																asset.Volume[8],snapshot.VolumeFinancial}


                };*/
                //Update grid:
                int index;
                index =
                    dataSource.Assets.IndexOf(
                        dataSource.Assets.Where(a => a.Symbol == asset.Symbol).First());
                if (index >= 0 && index < dataSource.Assets.Count)
                {
                    lock (dataSource)
                    {
                        dataSource.Update(asset);
                    }
                    UpdateGrid(index);
                }

                UpdateVolumeChart(snapshot.Symbol);


            }
        }

        public void UpdateVolumeChart(string Symbol)
        {
            //Update Volume's Chart:
            chart.Series.Clear();
            chart.Series.Add(GetRowData(dataSource.Assets.First(s => s.Symbol == Symbol).Symbol));
            if (_cellsVolumeChart.ContainsKey(Symbol))
                radGridView1_CellFormatting(new object(), _cellsVolumeChart[Symbol]);

        }
        /// <summary>
        /// Updates the radGridView.    
        /// </summary>
        /// <param name="index">Index of Symbol on datasource</param>

        public void UpdateGrid(int index)
        {
            try
            {
                //if(suspended) return;
                //update grid
                GridViewTemplate template = radGridView1.MasterTemplate.Templates[0];
                var mastertemplate =
                    (from GridViewRowInfo r in radGridView1.Rows
                     where (string)r.Cells["Symbol"].Value == dataSource.Assets[index].Symbol
                     select r).FirstOrDefault();
                var rowtemplate =
                    (from GridViewRowInfo r in template.Rows
                     where (string)r.Cells["Symbol"].Value == dataSource.Assets[index].Symbol
                     select r).FirstOrDefault();
                if (mastertemplate != null && rowtemplate != null)
                {
                    mastertemplate.Cells["Variation"].Value = dataSource.Assets[index].Variation;
                    mastertemplate.Cells["Last"].Value = dataSource.Assets[index].Last;
                    mastertemplate.Cells["Close"].Value = dataSource.Assets[index].Close;
                    mastertemplate.Cells["High"].Value = dataSource.Assets[index].High;
                    mastertemplate.Cells["Low"].Value = dataSource.Assets[index].Low;
                    mastertemplate.Cells["Volume"].Value = dataSource.Assets[index].Volume.Last();
                    mastertemplate.Cells["Time"].Value = dataSource.Assets[index].Time.Last();
                    mastertemplate.Cells["Open"].Value = dataSource.Assets[index].Open;
                    mastertemplate.Cells["Trades"].Value = dataSource.Assets[index].Trades;
                    mastertemplate.Cells["Position"].Value = dataSource.Assets[index].Position;
                    rowtemplate.Cells["Open"].Value = dataSource.Assets[index].Open;
                    rowtemplate.Cells["Close"].Value = dataSource.Assets[index].Close;
                    rowtemplate.Cells["High"].Value = dataSource.Assets[index].High;
                    rowtemplate.Cells["Low"].Value = dataSource.Assets[index].Low;
                    rowtemplate.Cells["Volume"].Value = dataSource.Assets[index].Volume.Last();
                    rowtemplate.Cells["Time"].Value = dataSource.Assets[index].Time.Last();
                }

                if (eventSyncEnable) eventsSync.WaitOne();
                if (suspended) return;
                radGridView1.MasterTemplate.Refresh();
                radGridView1.MasterTemplate.Templates[0].Refresh();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Telerik.WinControls.RadMessageBox.Show(ex.Message);
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }

        }

        public void OnUpdateEvent(object sender, BarDataEventArgs e)
        {
            //Console.WriteLine("*****");                                                
            //return;

            if (assetList.Exists(s => s == e.Bar.Symbol) &&
                dataSource.Assets.Exists(s => s.Symbol == e.Bar.Symbol))
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Action action = () =>
                                            {
                                                Console.WriteLine("INVOKE:GridDataAssetsRT:OnUpdateEvent(" + e.Bar.Symbol + ")");
                                                RTAssetsInfo asset = dataSource.Assets.Where(a => a.Symbol == e.Bar.Symbol).First();
                                                asset = frmMain2.GetPortfolioView(new List<string>() { e.Bar.Symbol }).Assets.First();
                                                                                                
                                                asset.Close = e.Bar.ClosePrice;
                                                asset.High = e.Bar.HighPrice;
                                                asset.Last = e.Bar.ClosePrice;
                                                asset.Low = e.Bar.LowPrice;
                                                asset.Open = e.Bar.OpenPrice;
                                                asset.Position = asset.Position;
                                                asset.Symbol = e.Bar.Symbol;
                                                //asset.Time = frmMain2.GetLastDates(e.Bar.Symbol);
                                                asset.Variation = e.Bar.OpenPrice != 0 ? (e.Bar.ClosePrice - e.Bar.OpenPrice) * 100 / e.Bar.OpenPrice : 0;
                                                //asset.Volume = frmMain2.GetLastVolumes(e.Bar.Symbol);
                                                for (int i = 0; i < 9; i++)
                                                {
                                                    asset.Time[i] = asset.Time[i+1];
                                                    asset.Volume[i] = asset.Volume[i+1];
                                                }
                                                asset.Time[9] = e.Bar.TradeDate.ToString();
                                                asset.Volume[9] = e.Bar.VolumeF;

                                                //Application.DoEvents();
                                                //Update grid:
                                                int index;
                                                index =
                                                    dataSource.Assets.IndexOf(
                                                        dataSource.Assets.Where(a => a.Symbol == asset.Symbol).First());
                                                if (index >= 0 && index < dataSource.Assets.Count)
                                                {
                                                    lock (dataSource)
                                                    {
                                                        dataSource.Update(asset);
                                                    }
                                                    UpdateGrid(index);
                                                }
                                                //Application.DoEvents();

                                                //Update Volume's Chart:
                                                chart.Series.Clear();
                                                chart.Series.Add(GetRowData(dataSource.Assets.First(s => s.Symbol == e.Bar.Symbol).Symbol));
                                                if (_cellsVolumeChart.ContainsKey(e.Bar.Symbol))
                                                    radGridView1_CellFormatting(new object(), _cellsVolumeChart[e.Bar.Symbol]);

                                                //Application.DoEvents();
                                            };
                        BeginInvoke(action);
                    }
                    else
                    {
                        Console.WriteLine("GridDataAssetsRT:OnUpdateEvent("+e.Bar.Symbol+")");
                        RTAssetsInfo asset = dataSource.Assets.Where(a => a.Symbol == e.Bar.Symbol).First();
                        asset = frmMain2.GetPortfolioView(new List<string>() { e.Bar.Symbol }).Assets.First();

                        asset.Close = e.Bar.ClosePrice;
                        asset.High = e.Bar.HighPrice;
                        asset.Last = e.Bar.ClosePrice;
                        asset.Low = e.Bar.LowPrice;
                        asset.Open = e.Bar.OpenPrice;
                        asset.Position = asset.Position;
                        asset.Symbol = e.Bar.Symbol;
                        //asset.Time = frmMain2.GetLastDates(e.Bar.Symbol);
                        asset.Variation = e.Bar.OpenPrice != 0 ? (e.Bar.ClosePrice - e.Bar.OpenPrice) * 100 / e.Bar.OpenPrice : 0;
                        //asset.Volume = frmMain2.GetLastVolumes(e.Bar.Symbol);
                        for (int i = 0; i < 9; i++)
                        {
                            asset.Time[i] = asset.Time[i + 1];
                            asset.Volume[i] = asset.Volume[i + 1];
                        }
                        asset.Time[9] = e.Bar.TradeDate.ToString();
                        asset.Volume[9] = e.Bar.VolumeF;

                        //Application.DoEvents();
                        //Update grid:
                        int index;
                        index =
                            dataSource.Assets.IndexOf(
                                dataSource.Assets.Where(a => a.Symbol == asset.Symbol).First());
                        if (index >= 0 && index < dataSource.Assets.Count)
                        {
                            lock (dataSource)
                            {
                                dataSource.Update(asset);
                            }
                            UpdateGrid(index);
                        }

                        //Application.DoEvents();
                        //Update Volume's Chart:
                        chart.Series.Clear();
                        chart.Series.Add(GetRowData(dataSource.Assets.First(s => s.Symbol == e.Bar.Symbol).Symbol));
                        if (_cellsVolumeChart.ContainsKey(e.Bar.Symbol))
                            radGridView1_CellFormatting(new object(), _cellsVolumeChart[e.Bar.Symbol]);
                        //Application.DoEvents();
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GridDataAssetsRT:OnUpdateEvent(" + e.Bar.Symbol + "): " + ex.Message);
                }
            }
        }
        private void OpenCloseDetailsRow(bool open)
        {
            try
            {
                if (suspended) return;
                radGridView1.CurrentRow.IsExpanded = open;
                radGridView1.CurrentRow.InvalidateRow();
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void RadGridView1PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool ignoreChart = false;
            if (!(_parentControl.MFrmMain2.GetCtlActiveWindowFloat() != null && _parentControl.MFrmMain2.GetCtlActiveWindowFloatName() == "CtlPainelChart"))
            {
                ignoreChart = true;
            }

            int index = 0;
            switch (e.KeyValue)
            {
                case 32: //Space
                case 13: //Enter   
                    foreach (GridViewCellInfo VARIABLE in radGridView1.CurrentCell.RowInfo.Cells)
                    {
                        if (VARIABLE.ColumnInfo.Name.Equals("Symbol"))
                        {
                            index = VARIABLE.ColumnInfo.Index;
                        }
                    }
                    ViewChart(radGridView1.CurrentCell.RowInfo.Cells[index].Value.ToString());
                    break;
                case 37: //Left
                    OpenCloseDetailsRow(false);
                    break;
                case 45: //Insert   
                    foreach (GridViewCellInfo VARIABLE in radGridView1.CurrentCell.RowInfo.Cells)
                    {
                        if (VARIABLE.ColumnInfo.Name.Equals("Symbol"))
                        {
                            index = VARIABLE.ColumnInfo.Index;
                        }
                    }
                    ViewChart(radGridView1.CurrentCell.RowInfo.Cells[index].Value.ToString(), true);
                    break;
                    break;
                case 39: //Right
                    OpenCloseDetailsRow(true);
                    break;
                // CHART ACTIONS:
                case 109: //Zoom Out
                case 189:
                    if (!ignoreChart)
                    {
                        int records = ((((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.VisibleRecordCount * ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0])._percent) / 100) / 2;
                        ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.ZoomOut(records > 0 ? records : ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.VisibleRecordCount);
                        ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).SaveViewportJDate();
                    }
                    break;
                case 107: //Zoom In
                case 187:
                    if (!ignoreChart)
                    {
                        int records = ((((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.VisibleRecordCount * ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0])._percent) / 100) / 2;
                        ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.ZoomIn(records);
                        ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).SaveViewportJDate();
                    }
                    break;
                /*case 33: //Scroll R
                    if(!ignoreChart)
                    {
                        chart.StockChartX1.ScrollRight(10);
                        chart._mRecord += 10;
                        chart.LoadScroll();
                    }
                    break;
                case 34: //Scroll L
                    if (!ignoreChart)
                    {
                        chart.StockChartX1.ScrollLeft(10);
                        chart._mRecord -= 10;
                        chart.LoadScroll();
                    }
                    break;*/
                case 8: //Backspace
                    if (!ignoreChart)
                    {
                        ((CtlPainelChart)_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]).StockChartX1.ResetZoom();
                    }
                    break;
                default:

                    if (char.IsLetter((char)e.KeyValue) || char.IsNumber((char)e.KeyValue))
                    {
                        _searchAsset += ((char)e.KeyValue).ToString().ToUpper();

                        if (!SearchTextGrid())
                        {
                            _searchAsset = ((char)e.KeyValue).ToString().ToUpper();

                            if (!SearchTextGrid())
                                _searchAsset = "";
                        }
                    }

                    break;
            }
        }

        private bool SearchTextGrid()
        {
            //if (suspended) return false;
            foreach (GridViewRowInfo t in radGridView1.Rows.Where(t => t.Cells[0].Value.ToString().StartsWith(_searchAsset)))
            {
                t.IsCurrent = true;
                t.IsSelected = true;
                return true;
            }

            return false;
        }
        private void NewChart(object sender, EventArgs e)
        {
            try
            {
                if (radGridView1.CurrentCell == null)
                    return;

                ChartSelection selection = null;
                switch (Properties.Settings.Default.DefaultPeriodicity)
                {
                    case "Daily":
                        selection = new ChartSelection
                        {
                            Symbol = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString(),
                            Interval = 1,
                            Bars = 500,
                            Periodicity = M4Core.Entities.Periodicity.Daily,
                            Source = "PLENA"
                        };
                        break;
                    case "Weekly":
                        selection = new ChartSelection
                        {
                            Symbol = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString(),
                            Interval = 1,
                            Bars = 500,
                            Periodicity = M4Core.Entities.Periodicity.Weekly,
                            Source = "PLENA"
                        };
                        break;
                    case "Month":
                        selection = new ChartSelection
                        {
                            Symbol = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString(),
                            Interval = 1,
                            Bars = 500,
                            Periodicity = M4Core.Entities.Periodicity.Month,
                            Source = "PLENA"
                        };
                        break;
                    case "Yearly":
                        selection = new ChartSelection
                        {
                            Symbol = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString(),
                            Interval = 1,
                            Bars = 500,
                            Periodicity = M4Core.Entities.Periodicity.Year,
                            Source = "PLENA"
                        };
                        break;
                    default:
                        selection = (new FrmSelectChart { CodeStock = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString() }).GetChartSelection(radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
                        break;
                }
                if (selection == null)
                    return;

                // _parentControl.MFrmMain.CreateNewCtlPainel(selection);
                _parentControl.MFrmMain2.CreateNewCtlPainel(selection, chart =>
                {
                    Scheme.Instance().UpdateChartColors(chart.StockChartX1, Properties.Settings.Default.SchemeColor);
                    chart.m_SchemeColor = Properties.Settings.Default.SchemeColor;
                    chart.StockChartX1.Visible = true;
                    this.Focus();
                });
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void ViewChart(string symbol, bool insert = false)
        {
            bool returnFocus = true;
            SqlConnection _con = DBlocalSQL.Connect();
            if (DBlocalSQL.GetLastBarDataDisk(symbol, BaseType.Days, _con).TradeDate == DateTime.MinValue)
            {
                RadMessageBox.Show("O ativo escolhido ainda não possui dados sincronizados.", " ");
                DBlocalSQL.Disconnect(_con);
                return;
            }
            DBlocalSQL.Disconnect(_con);

            //SAME WINDOW WITH SAME SETTINGS! (ENTER OR DBL-CLICK)
            if (_parentControl.MFrmMain2.GetCtlActiveWindowFloat() != null && !insert)
            {
                if (_parentControl.MFrmMain2.GetCtlActiveWindowFloatName() == "CtlPainelChart")
                //if (_parentControl.MFrmMain2.GetCtlActiveWindowFloatName() != "ctlWeb")
                {
                    CtlPainelChart chart = (CtlPainelChart)(_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]);
                    if (symbol.Equals(chart.MSymbol))
                        return;
                    chart.m_StopLoadScroll = true;


                    //Console.WriteLine("\nViewChart() MSymbol=" + chart.MSymbol + " NewWSymbol=" + symbol);

                    chart.StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                    chart.SaveViewportJDate();

                    chart.MSymbol = symbol;
                    chart.InitRTChartAsync(b => chart._asyncOp.Post(() =>
                    {
                        if (b)
                        {
                            //chart.BindContextMenuEvents();
                            chart.StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                           "TempTemplate" + ".sct");
                            chart.UpdateMenus();
                            chart.StockChartX1.Visible = true;
                            if (returnFocus) this.Focus();
                            return;
                        }
                        return;
                    }));

                    chart.Parent.Text = chart.GetChartTitle();

                    if (((DockWindow)Parent).DockState == DockState.Floating)
                    {

                        foreach (DockWindow document in _parentControl.MFrmMain2.radDock2.DockWindows)
                        {
                            if (document.AccessibleName == ((DockWindow)Parent).AccessibleDescription)
                            {
                                document.Text = chart.GetChartTitle();
                            }
                        }
                    }
                    Console.WriteLine("\n\t1- " + DateTime.Now.TimeOfDay + " Grid:ViewChart() ");
                    return;
                }
            }

            // NEW WINDOW WITH SAME SETTINGS (INSERT)!
            ChartSelection selection;
            if (insert)
            {
                CtlPainelChart chart = (CtlPainelChart)(_parentControl.MFrmMain2.GetCtlActiveWindowFloat().Controls[0]);
                chart.StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");
                chart.SaveViewportJDate();
                
                selection = new ChartSelection
                        {
                            Symbol = symbol,
                            Interval = chart.m_BarSize,
                            Bars = chart.m_Bars,
                            Periodicity = chart.m_Periodicity,
                            //PriceStyle = chart.m_PriceStyle,
                            Source = "PLENA"
                        };

                _parentControl.MFrmMain2.UseLastChartVisual = true;
                _parentControl.MFrmMain2.CreateNewCtlPainel(selection, chart2 =>
                {
                    Scheme.Instance().UpdateChartColors(chart2.StockChartX1, chart.m_SchemeColor);
                    chart2.m_SchemeColor = chart.m_SchemeColor;
                    chart2.StockChartX1.LoadGeneralTemplate(ListTemplates._path +
                                                   "TempTemplate" + ".sct");
                    chart2.UpdateMenus();
                    chart2.StockChartX1.Visible = true;
                    this.Focus();
                });
                return;
            }

            // NEW WINDOW WITH NEW SETTINGS (ENTER OR DBL-CLICK)!
            selection = null;
            switch (Properties.Settings.Default.DefaultPeriodicity)
            {
                case "Daily":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Daily,
                        Source = "PLENA"
                    };
                    break;
                case "Weekly":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Weekly,
                        Source = "PLENA"
                    };
                    break;
                case "Month":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Month,
                        Source = "PLENA"
                    };
                    break;
                case "Yearly":
                    selection = new ChartSelection
                    {
                        Symbol = symbol,
                        Interval = 1,
                        Bars = 500,
                        Periodicity = M4Core.Entities.Periodicity.Year,
                        Source = "PLENA"
                    };
                    break;
                default:
                    selection = (new FrmSelectChart { CodeStock = symbol }).GetChartSelection(symbol);
                    break;
            }

            if (selection == null)
                return;
            _parentControl.MFrmMain2.CreateNewCtlPainel(selection, chart =>
            {
                Scheme.Instance().UpdateChartColors(chart.StockChartX1, Properties.Settings.Default.SchemeColor);
                chart.m_SchemeColor = Properties.Settings.Default.SchemeColor;
                chart.StockChartX1.Visible = true;
                this.Focus();
            });
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                radGridView1.CellDoubleClick -= radGridView1_CellDoubleClick;

                RadElement radElement = radGridView1.ElementTree.GetElementAtPoint(_point);

                if ((!(radElement is GridDataCellElement)) || (radGridView1.CurrentCell == null))
                    return;
                int index = 0;
                foreach (GridViewCellInfo VARIABLE in radGridView1.CurrentCell.RowInfo.Cells)
                {
                    if (VARIABLE.ColumnInfo.Name.Equals("Symbol"))
                    {
                        index = VARIABLE.ColumnInfo.Index;
                    }
                }
                /* ViewChart(!radGridView1.CurrentCell.RowInfo.Cells[0].ColumnInfo.Name.Equals("Symbol")
                               ? radGridView1.CurrentCell.RowInfo.Cells[6].Value.ToString()
                               : radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString()); */
                ViewChart(radGridView1.CurrentCell.RowInfo.Cells[index].Value.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                radGridView1.CellDoubleClick += radGridView1_CellDoubleClick;
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void radGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            _point = e.Location;
        }

        private void radGridView1_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            try
            {
                //if(suspended) return;
                //radGridView1.EnableAlternatingRowColor = true;
                //radGridView1.TableElement.AlternatingRowColor = Color.FromArgb(218, 222, 234, 242);
                for (int i = 0; i < e.RowElement.RowInfo.Cells.Count; i++)
                {

                    //        //e.RowElement.DrawFill = true;
                    if (e.RowElement.IsCurrent)
                    {
                        //if (e.RowElement.ViewTemplate.Caption != Program.LanguageDefault.DictionarySelectTools["tabDetails"])
                        //            {
                        //                e.RowElement.BackColor = Color.White;
                        //            }
                        //            else
                        if (!e.RowElement.IsOdd)
                        {
                            e.RowElement.BackColor = Color.Empty;
                        }
                        else
                        {
                            e.RowElement.BackColor = Color.FromArgb(226, 234, 241, 247);
                        }

                        //        }
                        //        else
                        //        {
                        //            if (e.RowElement.IsOdd)
                        //            {
                        //                e.RowElement.BackColor = Color.White;
                        //            }
                        //            else
                        //            {
                        //                e.RowElement.BackColor = Color.FromArgb(218, 222, 234, 242);
                        //                e.RowElement.BackColor2 = Color.FromArgb(218, 222, 234, 242);
                        //                e.RowElement.BorderRightColor = Color.FromArgb(218, 222, 234, 242);
                        //                e.RowElement.BorderLeftColor = Color.FromArgb(218, 222, 234, 242);
                        //            }
                    }
                }

                //Change color by Variation:                

                /*double variation = 0.000000000000000000;

                object variationValue = e.RowElement.RowInfo.Cells[2].Value;
                if ((string)e.RowElement.RowInfo.Cells[0].Value == "CYRE3")
                {
                    Console.Write("CYRE3 ");
                }
                if (variationValue != null)
                {

                    if (!Convert.IsDBNull(variationValue))
                    {
                        variation = double.Parse(variationValue.ToString());
                    }
                    if (variation == 0.000000000000000000)
                    {
                        e.RowElement.ForeColor = Color.Black;
                    }
                    if (variation > 0.000000000000000000)
                    {
                        e.RowElement.ForeColor = Color.Green;
                    }
                    else if (variation < 0.000000000000000000)
                    {
                        e.RowElement.ForeColor = Color.Red;
                    }
                }*/


            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }

        }

        private void ManageGrid()
        {

            _menu = new RadDropDownMenu();

            RadMenuItem menuPortifolio = new RadMenuItem(Program.LanguageDefault.DictionarySelectTools["editPortfolios"]);
            menuPortifolio.Click += EditPortfolio;

            RadMenuItem menuChart = new RadMenuItem(Program.LanguageDefault.DictionarySelectTools["viewChart"]);
            menuChart.Click += ContextViewGraphic;

            RadMenuItem menuNewChart = new RadMenuItem(Program.LanguageDefault.DictionarySelectTools["newChart"]);
            menuNewChart.Click += NewChart;

            _menu.Items.Add(menuChart);
            _menu.Items.Add(menuNewChart);
            _menu.Items.Add(menuPortifolio);
        }

        private void EditPortfolio(object sender, EventArgs e)
        {
            EditPortfolio portEdit = new EditPortfolio(_parentControl, _parentControl.TabSelected);
            portEdit.ShowDialog();
        }

        private void ContextViewGraphic(object sender, EventArgs e)
        {
            try
            {

                if (radGridView1.CurrentCell == null)
                    return;

                if (radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString().Length > 2)
                {
                    ViewChart(radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
                }
            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void radGridView1_ColumnIndexChanging(object sender, ColumnIndexChangingEventArgs e)
        {
            try
            {

            }
            finally
            {
                if (eventSyncEnable) eventsSync.Set();
            }
        }

        private void radGridView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {

        }

        private void radGridView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            suspended = true;

        }
        private void radGridView1_CurrentViewChanged(object sender, GridViewCurrentViewChangedEventArgs e)
        {
            if (radGridView1.Templates.IndexOf(e.NewView.ViewTemplate) == -1)
            {
                return;
            }

            _selectedViewIndex = radGridView1.Templates.IndexOf(e.NewView.ViewTemplate);
            _parentControl.SetViewDefault(_selectedViewIndex);

            // radGridView1.Rows.Where(r => r.IsExpanded).Cast<GridViewHierarchyRowInfo>().ToList().ForEach(r => r.ActiveView = r.Views[_selectedViewIndex]);

        }
        public void ResizeGrid()
        {
            radGridView1.Dock = DockStyle.Fill;
        }


    }
}
