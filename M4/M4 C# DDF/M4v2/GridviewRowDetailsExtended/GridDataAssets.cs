using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DSPlena;
using M4.AsyncOperations;
using M4.DataServer.Interface;
using M4.M4v2.Base;
using M4.M4v2.Chart;
using M4.M4v2.Portfolio;
using M4.M4v2.UpdateService;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;
using M4Data.MessageService;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;
using System;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public partial class GridDataAssets : UserControl
    {
        private readonly SelectView1 _parentControl;
        public List<string> AssetList = new List<string>();
        public List<Assets> AllAssets;
        private Point _point;
        private RadDropDownMenu _menu;
        public bool DisableOrderAssets { get; set; }
        public string NamePort { get; set; }
        private string _searchAsset = "";
        private const double NULL_VALUE = -987654321.0;
        public List<DataAssetsInfo> dataAssetsInfos = new List<DataAssetsInfo>();
        public List<Assets> visibleAssetList = new List<Assets>();
        public List<bool> rowsExpanded = new List<bool>();

        public GridDataAssets(SelectView1 parentcontrol, List<string> allassets)
        {
            InitializeComponent();

            _parentControl = parentcontrol;

            /*if (File.Exists(ListAssets.Instance().Path))
                AllAssets = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);*/
            AllAssets = new List<Assets>();
            int position = 0;
            foreach (string asset in allassets)
            {
                BarData bar = frmMain.GetLastBarData(asset);
                AllAssets.Add(new Assets()
                                  {
                                      Close = (decimal)bar.ClosePrice,
                                      High = (decimal)bar.HighPrice,
                                      Last = (decimal)bar.ClosePrice,
                                      Low = (decimal)bar.LowPrice,
                                      Open = (decimal)bar.OpenPrice,
                                      Position = position,
                                      Symbol = bar.Symbol,
                                      Time = bar.TradeDate.Date.ToString(),
                                      Trades = 0,
                                      Variation = bar.OpenPrice!=0?(decimal)((bar.ClosePrice - bar.OpenPrice) * 100 / bar.OpenPrice):0
                                  });
                position++;
            }

            AssetList = allassets;

            radGridView1.Rows.CollectionChanged += RowsCollectionChanged;

            //Tested by example, should works!
            //radGridView1.VirtualMode = true;
        }

        private void ManageGrid()
        {
            radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            radGridView1.TableElement.RowHeight = 20;
            radGridView1.AllowAddNewRow = false;
            radGridView1.AllowRowReorder = DisableOrderAssets;
            radGridView1.ReadOnly = true;
            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

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
            EditPortfolio portEdit = new EditPortfolio(_parentControl.MFrmMain._select, _parentControl.TabSelected);
            portEdit.ShowDialog();
        }

        private void ContextViewGraphic(object sender, EventArgs e)
        {
            if (radGridView1.CurrentCell == null)
                return;

            if (radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString().Length > 2)
            {
                ViewChart(radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
            }
        }

        private void NewChart(object sender, EventArgs e)
        {
            if (radGridView1.CurrentCell == null)
                return;

            ChartSelection selection = (new FrmSelectChart { CodeStock = radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString() }).GetChartSelection();

            if (selection == null)
                return;

            _parentControl.MFrmMain.CreateNewCtlPainel(selection);
            _parentControl.MFrmMain2.CreateNewCtlPainel(selection);
        }

        private void ViewChart(string symbol)
        {
            if (_parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument != null)
            {
                if (_parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument.Client.Name == "CtlPainelChart")
                {

                    MessageService.SubmitRequest(new MSRequest("m" + _parentControl.MFrmMain._messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { symbol, BaseType.Days.GetHashCode() }));
                    _parentControl.MFrmMain.AddRequestedOperation(
                            new Operations("m" + _parentControl.MFrmMain._messageRequestID,
                                           TypeOperations.LoadCtlPainelChart,
                                           new object[]
                                               {
                                                   _parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument.ID,
                                                   symbol, _parentControl.MFrmMain.MActiveChart.m_Periodicity,
                                                   _parentControl.MFrmMain.MActiveChart.m_BarSize,
                                                   _parentControl.MFrmMain.MActiveChart.m_Bars,
                                                   _parentControl.MFrmMain.MActiveChart._source
                                               }));
                    //Check if there's file with data first:
                    /*string path = Directory.GetCurrentDirectory() + "\\Base\\";
                    if ((_parentControl.MFrmMain.MActiveChart.m_Periodicity == Periodicity.Minutely) || (_parentControl.MFrmMain.MActiveChart.m_Periodicity == Periodicity.Hourly))
                        path += "MINUTE\\" + _parentControl.MFrmMain.MActiveChart.MSymbol + ".csv";
                    else
                        path += "DAILY\\" + _parentControl.MFrmMain.MActiveChart.MSymbol + ".csv";
                    if (!File.Exists(path))
                    {
                        MessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgInvalidPeriodicity"] + _parentControl.MFrmMain.MActiveChart.MSymbol);
                        return;
                    }
                    _parentControl.MFrmMain.MActiveChart.m_StopLoadScroll = true;
                    
                    _parentControl.MFrmMain.MActiveChart.StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");

                    _parentControl.MFrmMain.MActiveChart._source = _parentControl.MFrmMain.MActiveChart._mListSymbols.Where(r => r.Code.Equals(symbol)).Single().Source;
                    _parentControl.MFrmMain.MActiveChart.LoadChart(_parentControl.MFrmMain.MActiveChart.MSymbol);

                    _parentControl.MFrmMain.MActiveChart.InitRTChartAsync(b => _parentControl.MFrmMain.MActiveChart._asyncOp.Post(() =>
                    {
                        if (b)
                        {
                            _parentControl.MFrmMain.MActiveChart.BindContextMenuEvents();
                            return;
                        }

                        return;
                    }));

                    _parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument.Text = _parentControl.MFrmMain.MActiveChart.GetChartTitle();
                    _parentControl.MFrmMain.MActiveChart.StockChartX1.Update();

                    if (_parentControl.MFrmMain.MActiveChart.m_EndJDate != NULL_VALUE)
                    {
                        _parentControl.MFrmMain.MActiveChart.StockChartX1.FirstVisibleRecord = _parentControl.MFrmMain.MActiveChart.StockChartX1.GetRecordByJDate(_parentControl.MFrmMain.MActiveChart.m_EndJDate) - _parentControl.MFrmMain.MActiveChart.m_QtyJDate;
                        _parentControl.MFrmMain.MActiveChart.StockChartX1.LastVisibleRecord = _parentControl.MFrmMain.MActiveChart.StockChartX1.GetRecordByJDate(_parentControl.MFrmMain.MActiveChart.m_EndJDate);
                    }
                    _parentControl.MFrmMain.MActiveChart.LoadScroll();*/

                    return;
                }
            }

            ChartSelection selection = (new FrmSelectChart { CodeStock = symbol }).GetChartSelection(symbol);

            if (selection == null)
                return;
            MessageService.SubmitRequest(new MSRequest("m" + _parentControl.MFrmMain._messageRequestID, MSRequestStatus.Pending, MSRequestType.GetHistoricalData, MSRequestOwner.M4, new object[] { selection.Symbol, BaseType.Days.GetHashCode() }));
            _parentControl.MFrmMain.AddRequestedOperation(
                    new Operations("m" + _parentControl.MFrmMain._messageRequestID,
                                   TypeOperations.CreateNewCtlPainelChart, new object[] { selection }));

            // _parentControl.MFrmMain.CreateNewCtlPainel(selection);
        }

        private void RadGridView1CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name.Equals("Variation"))
            {
                e.CellElement.ColumnInfo.HeaderText = "%";
                e.CellElement.ColumnInfo.Width = 40;
            }

            GridViewDataColumn column = e.CellElement.ColumnInfo as GridViewDataColumn;

            if (column == null)
                return;

            switch (column.FieldName)
            {
                case "Last":
                    if (!e.CellElement.RowInfo.Cells["Last"].Value.ToString().Equals("-"))
                        e.CellElement.Text = String.Format("{0:F2}", decimal.Parse(e.CellElement.RowInfo.Cells["Last"].Value.ToString())).Replace(',', '.');
                    break;
                case "Variation":

                    if (!e.CellElement.RowInfo.Cells["Variation"].Value.ToString().Equals("-"))
                        e.CellElement.Text = String.Format("{0:F2}", decimal.Parse(e.CellElement.RowInfo.Cells["Variation"].Value.ToString())).Replace(',', '.').Replace("-", "").Replace("+", "");
                    break;
                case "Symbol":
                    column.IsVisible = !column.OwnerTemplate.Caption.Equals("DetailsAssets");
                    e.CellElement.Text = e.CellElement.RowInfo.Cells["Symbol"].Value.ToString().ToUpper();
                    break;
                case "Time":
                case "Low":
                case "Open":
                case "High":
                case "Close":
                case "Trades":
                    column.Width = radGridView1.Width - 125;
                    break;
                case "Time1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["Time1"].Value + ":</b> ";
                    break;
                case "Low1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["Low1"].Value + ":</b> ";
                    break;
                case "Open1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["Open1"].Value + ":</b> ";
                    break;
                case "High1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["High1"].Value + ":</b> ";
                    break;
                case "Close1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["Close1"].Value + ":</b> ";
                    break;
                case "Trades1":
                    column.Width = 65;
                    column.MaxWidth = 65;
                    column.MinWidth = 65;
                    e.CellElement.Text = "<html><b>" + e.CellElement.RowInfo.Cells["Trades1"].Value + ":</b> ";
                    break;
            }
        }

        private void RadGridView1CreateCell(object sender, GridViewCreateCellEventArgs e)
        {
            if (e.CellType == typeof(GridDetailViewCellElement))
            {
                e.CellElement = new CustomDetailViewCellElement(e.Column, e.Row);
            }
        }

        public void LoadTable()
        {
            LoadAssetsTable();

            //LoadDetailsTable();

            //LoadGrid();
        }

        public void LoadAssetsTable()
        {
            if (radGridView1.Columns.Count <= 0)
            {
                radGridView1.Columns.Add("Symbol", Program.LanguageDefault.DictionarySelectTools["columnSymbol"],
                                         "Symbol");
                radGridView1.Columns.Add("Last", Program.LanguageDefault.DictionarySelectTools["columnLast"], "Last");
                radGridView1.Columns.Add("Variation", Program.LanguageDefault.DictionarySelectTools["columnVariation"],
                                         "Variation");
                radGridView1.Columns.Add("Volume", Program.LanguageDefault.DictionarySelectTools["columnVolume"],
                                         "Volume");
            }
            rowsExpanded = new List<bool>();
            if (radGridView1.Rows.Count > 0)
            {
                foreach (GridViewRowInfo row in radGridView1.Rows)
                {
                  rowsExpanded.Add(row.IsExpanded);  
                }
                //radGridView1.Rows.Clear();
            }

            dataAssetsInfos = new List<DataAssetsInfo>();

            lock (visibleAssetList)
            {
                foreach (var assets in visibleAssetList)
                {
                    string volume = assets.Volume.ToString();
                    double volumeRound;

                    if (assets.Volume > 1000000000)
                    {
                        volumeRound = Math.Round(assets.Volume / 1000000000, 2);
                        volume = volumeRound + "B";
                    }
                    else if (assets.Volume > 1000000)
                    {
                        volumeRound = Math.Round(assets.Volume / 1000000, 2);
                        volume = volumeRound + "M";
                    }
                    else if (assets.Volume > 1000)
                    {
                        volumeRound = Math.Round(assets.Volume / 1000, 2);
                        volume = volumeRound + "K";
                    }

                    switch (_parentControl.TabSelected.ToUpper())
                    {
                        case "ALL":
                        case "TODOS":
                            if (assets.Position.Equals(-1))
                            {
                                dataAssetsInfos.Add(new DataAssetsInfo
                                                        {
                                                            Symbol = assets.Symbol,
                                                            Last = "-",
                                                            Variation = "-",
                                                            Volume = "-",
                                                            Low = "-",
                                                            Open = "-",
                                                            Close = "-",
                                                            High = "-",
                                                            Time = "-",
                                                            Trades = "-"
                                                        });
                            }
                            else
                            {
                                dataAssetsInfos.Add(new DataAssetsInfo
                                                        {
                                                            Symbol = assets.Symbol,
                                                            Last = Math.Round(assets.Last, 2).ToString(),
                                                            Variation = assets.Variation.ToString(),
                                                            Volume = volume,
                                                            Low = "-",
                                                            Open = "-",
                                                            Close = "-",
                                                            High = "-",
                                                            Time = "-",
                                                            Trades = "-"
                                                        });
                            }
                            break;
                        default:
                            if (assets.Position.Equals(-1))
                            {
                                radGridView1.Rows.Add(new object[]
                                                          {
                                                              assets.Symbol, "-", "-", "-"
                                                          });
                            }
                            else
                            {
                                radGridView1.Rows.Add(new object[]
                                                          {
                                                              assets.Symbol, Math.Round(assets.Last, 2),
                                                              assets.Variation, volume
                                                          });
                            }
                            break;
                    }
                }
            }

            //if ((_parentControl.TabSelected.ToUpper().Equals("ALL")) || (_parentControl.TabSelected.ToUpper().Equals("TODOS")))
            try
            {
                radGridView1.DataSource = dataAssetsInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadDetailsTable()
        {
            //radGridView1.DataSource = dataAssetsInfos;
            if (radGridView1.MasterTemplate.Templates.Count > 0)
                radGridView1.MasterTemplate.Templates.Clear();


            GridViewTemplate template = new GridViewTemplate
                                            {
                                                AllowAddNewRow = false,
                                                ShowColumnHeaders = false,
                                                AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill
                                            };
            radGridView1.MasterTemplate.ReadOnly = false;
            template.Rows.Clear();
            template.Columns.Clear();
            template.Columns.Add("Low1", Program.LanguageDefault.DictionarySelectTools["columnLow"], "Low1");
            template.Columns.Add("Time1", Program.LanguageDefault.DictionarySelectTools["columnTime"], "Time1");
            template.Columns.Add("Open1", Program.LanguageDefault.DictionarySelectTools["columnOpen"], "Open1");
            template.Columns.Add("High1", Program.LanguageDefault.DictionarySelectTools["columnHigh"], "High1");
            template.Columns.Add("Close1", Program.LanguageDefault.DictionarySelectTools["columnClose"], "Close1");
            template.Columns.Add("Trades1", Program.LanguageDefault.DictionarySelectTools["columnTrades"], "Trades1");
            template.Columns.Add("Symbol", Program.LanguageDefault.DictionarySelectTools["columnSymbol"], "Symbol");
            template.Columns.Add("Low", Program.LanguageDefault.DictionarySelectTools["columnLow"], "Low");
            template.Columns.Add("Time", Program.LanguageDefault.DictionarySelectTools["columnTime"], "Time");
            template.Columns.Add("Open", Program.LanguageDefault.DictionarySelectTools["columnOpen"], "Open");
            template.Columns.Add("High", Program.LanguageDefault.DictionarySelectTools["columnHigh"], "High");
            template.Columns.Add("Close", Program.LanguageDefault.DictionarySelectTools["columnClose"], "Close");
            template.Columns.Add("Trades", Program.LanguageDefault.DictionarySelectTools["columnTrades"], "Trades");
            radGridView1.MasterTemplate.Templates.Add(template);

            radGridView1.MasterTemplate.Templates[0].Columns["Time1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Low1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Open1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["High1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Close1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Trades1"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Time"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Low"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Open"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["High"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Close"].DisableHTMLRendering = false;
            radGridView1.MasterTemplate.Templates[0].Columns["Trades"].DisableHTMLRendering = false;

            lock (visibleAssetList)
            {
                foreach (var assets in visibleAssetList)
                {
                    radGridView1.MasterTemplate.Templates[0].Rows.Add(new object[]
                                                                          {
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnLow"],
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnTime"],
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnOpen"],
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnHigh"],
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnClose"],
                                                                              Program.LanguageDefault.
                                                                                  DictionarySelectTools[
                                                                                      "columnTrades"], assets.Symbol,
                                                                              assets.Low
                                                                              , assets.Time, assets.Open, assets.High,
                                                                              assets.Close, assets.Trades
                                                                          });
                }
            }
        }

        public void LoadGrid()
        {
            if (radGridView1.Relations.Count > 0)
                radGridView1.Relations.Clear();

            GridViewRelation relation1 = new GridViewRelation(radGridView1.MasterTemplate)
                                             {
                                                 ChildTemplate = radGridView1.MasterTemplate.Templates[0],
                                                 RelationName = "FKSymbol"
                                             };

            relation1.ParentColumnNames.Add("Symbol");
            relation1.ChildColumnNames.Add("Symbol");
            radGridView1.Relations.Add(relation1);

            HtmlViewDefinition viewDef = new HtmlViewDefinition();
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows.Add(new RowDefinition());
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Time1", 0, 1, 1));
            viewDef.RowTemplate.Rows[0].Cells.Add(new CellDefinition("Time", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Open1", 0, 1, 1));
            viewDef.RowTemplate.Rows[1].Cells.Add(new CellDefinition("Open", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("High1", 0, 1, 1));
            viewDef.RowTemplate.Rows[2].Cells.Add(new CellDefinition("High", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Low1", 0, 1, 1));
            viewDef.RowTemplate.Rows[3].Cells.Add(new CellDefinition("Low", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Close1", 0, 1, 1));
            viewDef.RowTemplate.Rows[4].Cells.Add(new CellDefinition("Close", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Trades1", 0, 1, 1));
            viewDef.RowTemplate.Rows[5].Cells.Add(new CellDefinition("Trades", 0, 1, 1));
            radGridView1.MasterTemplate.Templates[0].ViewDefinition = viewDef;

            radGridView1.Columns["Symbol"].MinWidth = 60;
            radGridView1.Columns["Last"].MinWidth = 50;
            radGridView1.Columns["Variation"].MinWidth = 50;
            radGridView1.Columns["Volume"].MinWidth = 60;
            radGridView1.Columns["Symbol"].TextAlignment = ContentAlignment.TopLeft;
            radGridView1.Columns["Last"].TextAlignment = ContentAlignment.TopRight;
            radGridView1.Columns["Variation"].TextAlignment = ContentAlignment.TopRight;
            radGridView1.Columns["Volume"].TextAlignment = ContentAlignment.TopRight;

            radGridView1.GridBehavior = new RadGridBehavior();

            //Expand rows:
            if (rowsExpanded.Count > 0 && rowsExpanded.Count == radGridView1.Rows.Count)
            {
                for (int i = 0; i < radGridView1.Rows.Count; i++)
                {
                    radGridView1.Rows[i].IsExpanded = rowsExpanded[i];
                }
            }
        }

        private void RadGridView1RowFormatting(object sender, RowFormattingEventArgs e)
        {
            for (int i = 0; i < e.RowElement.RowInfo.Cells.Count; i++)
            {
                if (!e.RowElement.RowInfo.Cells[i].ColumnInfo.FieldName.Equals("Variation") || e.RowElement.RowInfo.Cells[i].Value == null)
                    continue;

                decimal variation = -1;

                object variationValue = e.RowElement.RowInfo.Cells[i].Value;

                if (variationValue.Equals("-"))
                    continue;

                if (variationValue != null && !Convert.IsDBNull(variationValue))
                {
                    variation = decimal.Parse(variationValue.ToString());
                }
                if (variation == 0)
                {
                    e.RowElement.ForeColor = Color.Gray;
                }
                if (variation > 0)
                {
                    e.RowElement.ForeColor = Color.Green;
                }
                else if (variation < 0)
                {
                    e.RowElement.ForeColor = Color.Red;
                }
            }
        }

        private void RadGridView1Resize(object sender, EventArgs e)
        {
            radGridView1.MasterTemplate.Refresh();
            radGridView1.Refresh();

            radGridView1.GridViewElement.TableElement.VScrollBar.MinSize = new Size(16, Height);
        }

        private void RadGridView1MouseMove(object sender, MouseEventArgs e)
        {
            _point = e.Location;
        }

        private void RadGridView1MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            RadElement radElement = radGridView1.ElementTree.GetElementAtPoint(_point);

            if (radElement is GridHeaderCellElement)
                return;

            _menu.Show(radGridView1, e.X, e.Y);
        }

        private void GridDataAssetsLoad(object sender, EventArgs e)
        {
            _eventWait.WaitOne();
            ManageGrid();
            lock (visibleAssetList)
            {
                visibleAssetList = AssetList.Select(assetse1 =>
                                                    AllAssets.Where(assetse => assetse.Symbol.Equals(assetse1))
                                                        .FirstOrDefault() ?? new Assets
                                                                                 {
                                                                                     Symbol = assetse1,
                                                                                     Position = -1
                                                                                 }).ToList();
            }
            LoadTable();
            _eventWait.Set();
            DataFeeder.Instance().DSForm.TickEvent += OnTickEvent;
        }

        private void OpenCloseDetailsRow(bool open)
        {
            radGridView1.CurrentRow.IsExpanded = open;
            radGridView1.CurrentRow.InvalidateRow();
        }

        private void RadGridView1PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 13: //Enter
                    ViewChart(!radGridView1.CurrentCell.RowInfo.Cells[0].ColumnInfo.Name.Equals("Symbol")
                           ? radGridView1.CurrentCell.RowInfo.Cells[6].Value.ToString()
                           : radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
                    break;
                case 37: //Left
                    OpenCloseDetailsRow(false);
                    break;
                case 45: //Up
                    NewChart(null, null);
                    break;
                case 39: //Right
                    OpenCloseDetailsRow(true);
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
            foreach (GridViewRowInfo t in radGridView1.Rows.Where(t => t.Cells[0].Value.ToString().StartsWith(_searchAsset)))
            {
                t.IsCurrent = true;
                t.IsSelected = true;
                return true;
            }

            return false;
        }

        private void MoveUp()
        {
            MoveRow(true);
        }

        private void MoveDown()
        {
            MoveRow(false);
        }

        private void MoveRow(bool moveUp)
        {
            GridViewRowInfo currentRow = radGridView1.CurrentRow;

            if (currentRow == null)
                return;

            if ((moveUp) && (currentRow.Index - 1 > -1))
            {
                radGridView1.Rows[currentRow.Index - 1].IsCurrent = true;
                radGridView1.Rows[currentRow.Index - 1].IsSelected = true;
            }
            else if ((!moveUp) && (currentRow.Index + 1 < radGridView1.RowCount))
            {
                radGridView1.Rows[currentRow.Index + 1].IsCurrent = true;
                radGridView1.Rows[currentRow.Index + 1].IsSelected = true;
            }
        }

        private void RowsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((e.Action != NotifyCollectionChangedAction.Move) || (e.NewStartingIndex == e.OldStartingIndex))
                return;

            string asset = AssetList[e.OldStartingIndex];

            AssetList.Remove(AssetList[e.OldStartingIndex]);
            AssetList.Insert(e.NewStartingIndex, asset);

            //TODO: save new portfolio order on extremeDB!
            //ListPortfolios.Instance().ReorderPosition(_parentControl.TabSelected, asset, e.NewStartingIndex, e.NewStartingIndex > e.OldStartingIndex);
        }

        private void RadGridView1ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                if (e.ContextMenu.Items.Count == 0)
                    e.Cancel = true;

                RadItemOwnerCollection collection = new RadItemOwnerCollection { e.ContextMenu.Items[2] };
                e.ContextMenu.Items.Clear();

                collection[0].Text = Program.LanguageDefault.DictionaryMenuPlena["clearSorting"];

                e.ContextMenu.Items.Add(collection[0]);
            }
            catch (Exception)
            {

            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                radGridView1.CellDoubleClick -= radGridView1_CellDoubleClick;

                RadElement radElement = radGridView1.ElementTree.GetElementAtPoint(_point);

                if ((!(radElement is GridDataCellElement)) || (radGridView1.CurrentCell == null))
                    return;

                ViewChart(!radGridView1.CurrentCell.RowInfo.Cells[0].ColumnInfo.Name.Equals("Symbol")
                              ? radGridView1.CurrentCell.RowInfo.Cells[6].Value.ToString()
                              : radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                radGridView1.CellDoubleClick += radGridView1_CellDoubleClick;
            }
        }

        public void UpdateTick(TickData Tick)
        {
            lock (visibleAssetList)
            {
                if (AssetList.Exists(s => s == Tick.Symbol) &&
                    visibleAssetList.Exists(s => s.Symbol == Tick.Symbol))
                {
                    //Console.WriteLine("TickEvent on Portfolios for symbol " + e.Tick.Symbol);
                    Assets asset = visibleAssetList.Where(a => a.Symbol == Tick.Symbol).First();
                    asset = new Assets()
                    {
                        Close = (decimal)Tick.Price,
                        High =
                            Tick.Price > (double)asset.High
                                ? (decimal)Tick.Price
                                : asset.High,
                        Last = (decimal)Tick.Price,
                        Low =
                            Tick.Price < (double)asset.Low
                                ? (decimal)Tick.Price
                                : asset.Low,
                        Open = asset.Open,
                        Position = asset.Position,
                        Symbol = asset.Symbol,
                        Time = Tick.TradeDate.ToString(),
                        Trades = asset.Trades,
                        Variation = asset.Open!=0?((decimal)Tick.Price - asset.Open) * 100 / asset.Open:0,
                        /*TODO: Volume deriva em 3 tipos:
      * Ações = Quantidade de Ações
      * Financeiro = Quantidade de Ações x Preço
      * Negócios = Quantidade de Transações (1 por tick)
      */
                        Volume = asset.Volume

                    };
                    DataAssetsInfo assetInfo = new DataAssetsInfo(asset);
                    //Update grid:
                    int index;
                    index =
                        visibleAssetList.IndexOf(
                            visibleAssetList.Where(a => a.Symbol == asset.Symbol).First());
                    if (index >= 0 && index < visibleAssetList.Count)
                        visibleAssetList[index] = asset;





                    ((List<DataAssetsInfo>)radGridView1.DataSource)[
                        ((List<DataAssetsInfo>)radGridView1.DataSource).IndexOf(
                            ((List<DataAssetsInfo>)radGridView1.DataSource).Where(s => s.Symbol == Tick.Symbol).First())
                        ] = assetInfo;

                    radGridView1.MasterTemplate.Refresh();
                    //LoadTable();
                }
            }
            
        }

        private readonly AutoResetEvent _eventWait = new AutoResetEvent(true);
        public void OnTickEvent(object sender, TickEventArgs e)
        {
            //return;
            _eventWait.WaitOne();
            Action action = () =>
                           {
                               UpdateTick(e.Tick);
                               _eventWait.Set();
                           };
            BeginInvoke(action);
        }

        private void radGridView1_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Drag Enter!");
        }

        private void radGridView1_DragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Drag Over!");
        }

        private void radGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Drag Drop!");
        }

        private void radGridView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Give Feedback!");

        }

        private void radGridView1_DragLeave(object sender, EventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Drag Leave!");

        }

        private void radGridView1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Query Drag!");

        }

        private void radGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] Begin Edit!");

        }

        private void radGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            Console.WriteLine("[GRIDVIEW] End Edit!");

        }


    }
}