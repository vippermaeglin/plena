using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M4.M4v2.GridviewRowDetailsExtended;
using M4Core.Entities;
using M4Data.List;
using Telerik.WinControls.UI;

namespace M4.M4v2.Portfolio
{
    public partial class DataManager : UserControl
    {
        //For minute bars, hour bars, etc.
        public enum Periodicity
        {
            Secondly = 1,
            Minutely = 2,
            Hourly = 3,
            Daily = 4,
            Weekly = 5
        }

        public List<string> AssetList;
        public string NamePort { get; set; }

        public DataManager(List<string> AList)
        {
            InitializeComponent();

            AssetList = AList;

            LoadAssets(AssetList);

            ManagerGrid();

            //LoadDictionaryGrid();
        }

        private void ManagerGrid()
        {
            grdAtivos.TableElement.RowHeight = 25;
            grdAtivos.EnableSorting = true;
            grdAtivos.EnableGrouping = false;
            grdAtivos.AllowAddNewRow = false;
            grdAtivos.MasterTemplate.AllowRowReorder = true;
            grdAtivos.ReadOnly = true;
            grdAtivos.Dock = DockStyle.Fill;
            grdAtivos.Columns["Time"].MinWidth = 50;
            grdAtivos.Columns["High"].MinWidth = 50;
            grdAtivos.Columns["Low"].MinWidth = 50;
            grdAtivos.Columns["Close"].MinWidth = 50;
            grdAtivos.Columns["Open"].MinWidth = 50;
            grdAtivos.Columns["Trades"].MinWidth = 50;
            grdAtivos.Columns["Symbol"].MinWidth = 50;
            grdAtivos.Columns["Last"].MinWidth = 50;
            grdAtivos.Columns["Variation"].MinWidth = 50;
            grdAtivos.Columns["Volume"].MinWidth = 50;
            grdAtivos.Columns["Time"].Width = 50;
            grdAtivos.Columns["High"].Width = 50;
            grdAtivos.Columns["Low"].Width = 50;
            grdAtivos.Columns["Close"].Width = 50;
            grdAtivos.Columns["Open"].Width = 50;
            grdAtivos.Columns["Trades"].Width = 50;
            grdAtivos.Columns["Symbol"].Width = 50;
            grdAtivos.Columns["Last"].Width = 50;
            grdAtivos.Columns["Variation"].Width = 50;
            grdAtivos.Columns["Volume"].Width = 50;
            //grdAtivos.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            grdAtivos.MasterTemplate.AllowAutoSizeColumns = true;
            grdAtivos.MasterTemplate.HorizontalScrollState = ScrollState.AlwaysShow;
            //grdAtivos.MasterTemplate.BestFitColumns();
        }

        private void LoadDictionaryGrid()
        {
            //grdAtivos.Columns[0].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnActive"];
            grdAtivos.Columns[0].Width = 60;

            //grdAtivos.Columns[1].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnLast"];
            grdAtivos.Columns[1].Width = 40;

            //grdAtivos.Columns[2].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnHour"];
            grdAtivos.Columns[2].Width = 55;

            //grdAtivos.Columns[3].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnVariation"];
            grdAtivos.Columns[3].Width = 40;

            //grdAtivos.Columns[4].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnMaximum"];
            grdAtivos.Columns[4].Width = 50;

            //grdAtivos.Columns[5].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnMinimum"];
            grdAtivos.Columns[5].Width = 50;

            //grdAtivos.Columns[6].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnClosing"];
            grdAtivos.Columns[6].Width = 50;

            //grdAtivos.Columns[7].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnOpening"];
            grdAtivos.Columns[7].Width = 50;

            //grdAtivos.Columns[8].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnBusiness"];
            grdAtivos.Columns[8].Width = 50;

            //grdAtivos.Columns[9].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnVolume"];
            grdAtivos.Columns[9].Width = 60;
        }

        public void LoadAssets(List<string> assetList)
        {
            grdAtivos.Rows.Clear();

            List<Assets> all = File.Exists(ListAssets.Instance().Path) ? ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path) : new List<Assets>();

            List<DataAssetsInfo> dataAssetsInfos = new List<DataAssetsInfo>();

            foreach (var assets in all)
            {
                foreach (string s in assetList)
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

                    if (s == assets.Symbol)
                    {
                        dataAssetsInfos.Add(new DataAssetsInfo
                                                {
                                                    Symbol = assets.Symbol,
                                                    Last = Math.Round(assets.Last, 2).ToString(),
                                                    Variation = assets.Variation.ToString(),
                                                    Volume = volume,
                                                    Close = assets.Close.ToString(),
                                                    High = assets.High.ToString(),
                                                    Low = assets.Low.ToString(),
                                                    Open = assets.Open.ToString(),
                                                    Time = assets.Time,
                                                    Trades = assets.Trades.ToString()
                                                });
                    }
                }
            }

            grdAtivos.DataSource = dataAssetsInfos;
        }

        private void GrdAtivosRowFormatting(object sender, RowFormattingEventArgs e)
        {
            for (int i = 0; i < e.RowElement.RowInfo.Cells.Count; i++)
            {
                if (!e.RowElement.RowInfo.Cells[i].ColumnInfo.FieldName.Equals("Variation"))
                    continue;

                decimal variation = -1;

                object variationValue = e.RowElement.RowInfo.Cells[i].Value;

                if (variationValue != null && !Convert.IsDBNull(variationValue))
                {
                    variation = decimal.Parse(variationValue.ToString());
                }

                if (variation == 0)
                    e.RowElement.ForeColor = Color.Gray;
                if (variation > 0)
                    e.RowElement.ForeColor = Color.Green;
                else if (variation < 0)
                    e.RowElement.ForeColor = Color.Red;
            }
        }

        private void grdAtivos_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnIndex != 0)
                return;
        }

        private void MasterTemplateKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            foreach (var row in grdAtivos.Rows.Where(row => row.IsSelected))
            {
                AssetList.RemoveAt(AssetList.IndexOf(row.Cells[0].Value.ToString()));
                grdAtivos.Rows.Remove(row);
                break;
            }
        }

        public void GrdAtivosAddRow(string s)
        {
            List<Assets> all = ListAssets.Instance().LoadListAssets(ListAssets.Instance().Path);

            foreach (var assets in all.Where(assets => s == assets.Symbol))
            {
                grdAtivos.Rows.Add(assets.Symbol, Math.Round(assets.Last, 2), assets.Time, assets.Variation, assets.High,
                                   assets.Low, assets.Close, assets.Open, assets.Trades, assets.Volume);
            }
        }

        private void GrdAtivosSizeChanged(object sender, EventArgs e)
        {
            Size minsize = new Size(500, 0);

            if (grdAtivos.Size.Width < minsize.Width)
            {
                grdAtivos.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                grdAtivos.MasterTemplate.HorizontalScrollState = ScrollState.AlwaysShow;
                grdAtivos.Columns["Time"].Width = 65;
                grdAtivos.Columns["High"].Width = 50;
                grdAtivos.Columns["Low"].Width = 50;
                grdAtivos.Columns["Close"].Width = 50;
                grdAtivos.Columns["Open"].Width = 50;
                grdAtivos.Columns["Trades"].Width = 50;
                grdAtivos.Columns["Symbol"].Width = 50;
                grdAtivos.Columns["Last"].Width = 50;
                grdAtivos.Columns["Variation"].Width = 50;
                grdAtivos.Columns["Volume"].Width = 60;
            }
            else
            {
                grdAtivos.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                grdAtivos.MasterTemplate.HorizontalScrollState = ScrollState.AutoHide;
            }
        }
    }
}
