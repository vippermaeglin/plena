using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M4.Properties;
using M4Core.Entities;
using M4Data.List;
using M4Utils.Language;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace M4.M4v2.FormDock
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

        public DataManager()
        {
            InitializeComponent();

            ManagerGrid();
            LoadAssets();
            CreateCommandDelete();
            LoadDictionaryGrid();
        }

        private void ManagerGrid()
        {
            grdAtivos.TableElement.RowHeight = 35;
            grdAtivos.EnableSorting = false;
            grdAtivos.EnableGrouping = false;
            grdAtivos.AllowAddNewRow = false;
            grdAtivos.MasterTemplate.AllowRowReorder = true;
            grdAtivos.ReadOnly = true;
            grdAtivos.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void grdAtivos_CommandCellClick(object sender, EventArgs e)
        {
            foreach (var row in grdAtivos.Rows.Where(row => row.IsSelected))
            {
                grdAtivos.Rows.Remove(row);
                break;
            }
        }

        private void CreateCommandDelete()
        {
            GridViewCommandColumn gridViewCommandColumn = new GridViewCommandColumn
            {
                FieldName = "cmdDelete",
                ImageLayout = ImageLayout.Center,
                TextAlignment = ContentAlignment.MiddleCenter,
                HeaderText = "",
                Width = 25,
                TextImageRelation = TextImageRelation.ImageAboveText,
            };

            grdAtivos.MasterTemplate.Columns.Insert(0, gridViewCommandColumn);
            grdAtivos.CommandCellClick += grdAtivos_CommandCellClick;
        }

        private void LoadDictionaryGrid()
        {

            grdAtivos.Columns[1].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnActive"];
            grdAtivos.Columns[1].Width = 80;

            grdAtivos.Columns[2].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnAmount"];
            grdAtivos.Columns[2].Width = 80;

            grdAtivos.Columns[3].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnBusiness"];
            grdAtivos.Columns[3].Width = 80;

            grdAtivos.Columns[4].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnClosing"];
            grdAtivos.Columns[4].Width = 80;

            grdAtivos.Columns[5].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnHour"];
            grdAtivos.Columns[5].Width = 80;

            grdAtivos.Columns[6].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnLast"];
            grdAtivos.Columns[6].Width = 80;

            grdAtivos.Columns[7].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnMaximum"];
            grdAtivos.Columns[7].Width = 80;

            grdAtivos.Columns[8].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnMinimum"];
            grdAtivos.Columns[8].Width = 80;

            grdAtivos.Columns[9].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnOpening"];
            grdAtivos.Columns[9].Width = 80;

            grdAtivos.Columns[10].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnVariation"];
            grdAtivos.Columns[10].Width = 120;

            grdAtivos.Columns[11].HeaderText = Program.LanguageDefault.DictionaryGridAssets["columnVolume"];
            grdAtivos.Columns[11].Width = 180;
        }

        private void LoadAssets()
        {
            string path = Directory.GetCurrentDirectory() + "\\RegAtivos.xml";
            List<Assets> lista = ListAssets.Instance().LoadListAssets(path);

            foreach (var assets in lista)
            {
                grdAtivos.Rows.Add(assets.Active, assets.Last, assets.Hour, assets.Variation, assets.Maximum,
                    assets.Minimum, assets.Closing, assets.Opening, assets.Business, assets.Amount, assets.Volume);
            }
        }

        private void grdAtivos_RowFormatting(object sender, RowFormattingEventArgs e)
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

            RadButtonElement element = (RadButtonElement)e.CellElement.Children[0];
            element.DisplayStyle = DisplayStyle.Image;
            element.Image = Resources.delete;
            element.ImageAlignment = ContentAlignment.MiddleCenter;
        }
    }
}
