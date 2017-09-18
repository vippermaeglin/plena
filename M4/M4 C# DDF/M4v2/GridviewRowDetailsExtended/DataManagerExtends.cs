using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M4.AsyncOperations;
using M4.M4v2.Chart;
using M4.M4v2.Portfolio;
using M4Core.Entities;
using M4Core.Enums;
using M4Data.List;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System;

namespace M4.M4v2.GridviewRowDetailsExtended
{
    public partial class DataManagerExtends
    {
        #region Members

        private readonly SelectView1 _parentControl;
        public List<string> AssetList = new List<string>();
        public List<Assets> AllAssets;

        #endregion

        #region Initialization

        public DataManagerExtends(SelectView1 parentcontrol, List<string> allassets)
        {
            InitializeComponent();

            if (File.Exists(ListAssets.Instance().Path))
                AllAssets = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);

            _parentControl = parentcontrol;

            AssetList = allassets;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadAssets(AssetList);

            ManageGrid();
        }

        public void LoadAssets(List<string> assetList)
        {
            if (AllAssets == null)
                return;

            List<Assets> visibleAssetList =
                (from assetse in AllAssets from s in assetList where assetse.Symbol.Equals(s) select assetse).ToList();

            radGridView1.DataSource = GetDataAssets(visibleAssetList);
        }

        private static DataTable GetDataAssets(List<Assets> visibleAssetList)
        {
            DataTable dataAssets = new DataTable("Assets");
            dataAssets.Columns.Add("Symbol");
            dataAssets.Columns.Add("Last");
            dataAssets.Columns.Add("Time");
            dataAssets.Columns.Add("Variation");
            dataAssets.Columns.Add("High");
            dataAssets.Columns.Add("Low");
            dataAssets.Columns.Add("Close");
            dataAssets.Columns.Add("Open");
            dataAssets.Columns.Add("Details");
            dataAssets.Columns.Add("Trades");
            dataAssets.Columns.Add("Volume");

            foreach (var assets in visibleAssetList)
            {
                string details = "    Time:  \t " + assets.Time + "\n    Open: \t " + assets.Open + "\n    High: \t  " +
                                 assets.High + "\n    Low:  \t  " + assets.Low + "\n    Close: \t " + assets.Close +
                                 "\n    Trades:\t " + assets.Trades;
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

                dataAssets.Rows.Add(new object[]
                             {
                                 assets.Symbol, Math.Round(assets.Last, 2), assets.Time, assets.Variation, assets.High,
                                 assets.Low, assets.Close, assets.Open, details, assets.Trades, volume
                             });
            }

            return dataAssets;
        }

        private void ManageGrid()
        {
            radGridView1.TableElement.RowHeight = 20;
            radGridView1.EnableSorting = true;
            radGridView1.EnableGrouping = false;
            radGridView1.AllowAddNewRow = false;
            radGridView1.MasterTemplate.AllowRowReorder = true;
            radGridView1.ReadOnly = true;
            radGridView1.Columns["Time"].IsVisible = false;
            radGridView1.Columns["High"].IsVisible = false;
            radGridView1.Columns["Low"].IsVisible = false;
            radGridView1.Columns["Close"].IsVisible = false;
            radGridView1.Columns["Open"].IsVisible = false;
            radGridView1.Columns["Trades"].IsVisible = false;
            radGridView1.Columns["Details"].IsVisible = false;
            radGridView1.Columns["Details"].WrapText = true;
            radGridView1.Columns["Symbol"].MinWidth = 50;
            radGridView1.Columns["Last"].MinWidth = 50;
            radGridView1.Columns["Variation"].MinWidth = 50;
            radGridView1.Columns["Volume"].MinWidth = 50;
            radGridView1.Columns["Symbol"].TextAlignment = ContentAlignment.TopLeft;
            radGridView1.Columns["Last"].TextAlignment = ContentAlignment.TopRight;
            radGridView1.Columns["Variation"].TextAlignment = ContentAlignment.TopRight;
            radGridView1.Columns["Volume"].TextAlignment = ContentAlignment.TopRight;
            radGridView1.DetailsColumn = radGridView1.Columns["Details"];
            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView1.MasterTemplate.BestFitColumns();
            radGridView1.ShowRowDetails = true;
            radGridView1.EnableAllExpand = false;
            radGridView1.ContextMenuStrip = new ContextMenuStrip();
            radGridView1.ContextMenuStrip.Items.Add("Edit Portfolios", null, EditPortfolio);
            radGridView1.ContextMenuStrip.Items.Add("View Chart", null, ContextViewGraphic);
        }

        #endregion

        private void RadGridView1CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name.Equals("Variation"))
            {
                e.CellElement.ColumnInfo.HeaderText = "%";
                e.CellElement.ColumnInfo.Width = 40;
            }

            if (!e.CellElement.RowInfo.IsCurrent || !ReferenceEquals(e.CellElement.ColumnInfo, radGridView1.DetailsColumn))
                return;

            e.CellElement.DrawFill = true;
            e.CellElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            e.CellElement.BackColor = Color.White;
            e.CellElement.Padding = new Padding(0);
            e.CellElement.ForeColor = Color.Black;
        }

        private void RadGridView1RowFormatting(object sender, RowFormattingEventArgs e)
        {
            for (int i = 0; i < e.RowElement.RowInfo.Cells.Count; i++)
            {
                if (!e.RowElement.RowInfo.Cells[i].ColumnInfo.FieldName.Equals("Variation")) continue;

                decimal variation = -1;

                object variationValue = e.RowElement.RowInfo.Cells[i].Value;

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

        private void EditPortfolio(object sender, EventArgs e)
        {
            EditPortfolio portEdit = new EditPortfolio(_parentControl.MFrmMain._data, _parentControl.TabSelected);
            portEdit.ShowDialog();
        }

        private void RadGridView1CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.Cells[0].Value.ToString().Length > 2)
            {
                ViewChart(e.Row.Cells[0].Value.ToString());
            }
        }

        private void ContextViewGraphic(object sender, EventArgs e)
        {
            if (radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString().Length > 2)
            {
                ViewChart(radGridView1.CurrentCell.RowInfo.Cells[0].Value.ToString());
            }
        }

        private void ViewChart(string symbol)
        {
            if(_parentControl.MFrmMain.MActiveChart!=null)_parentControl.MFrmMain.MActiveChart.StockChartX1.LoadUserStudyLine(-1);
            if (_parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument != null)
            {
                if (_parentControl.MFrmMain.m_DockManager.DocumentManager.ActiveDocument.Client.Name == "CtlPainelChart")
                {
                    _parentControl.MFrmMain.MActiveChart.StockChartX1.SaveGeneralTemplate(ListTemplates._path + "TempTemplate" + ".sct");

                    _parentControl.MFrmMain.MActiveChart.MSymbol = symbol;
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
                    _parentControl.MFrmMain.MActiveChart.StockChartX1.Focus();
                    _parentControl.MFrmMain.MActiveChart.StockChartX1.Update();

                    return;
                }
            }

            ChartSelection selection = (new FrmSelectChart { CodeStock = symbol }).GetChartSelection(symbol);

            if (selection == null)
                return;

            _parentControl.MFrmMain.CreateNewCtlPainel(selection);
        }
    }
}
