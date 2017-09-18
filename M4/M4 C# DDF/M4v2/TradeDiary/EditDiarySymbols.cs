using System;
using System.Collections.Generic;
using System.Linq;
using M4.M4v2.Portfolio;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace M4.M4v2.TradeDiary
{
    public partial class EditDiarySymbols : RadForm
    {
        private readonly PortfolioView1 _parentControl;
        public string TemporaryPortName;
        public List<string> TemporaryPortAssets;
        public bool Changes;
        public string PreviewPortfolio;

        public EditDiarySymbols(PortfolioView1 PARENTCONTROL)
        {
            InitializeComponent();
            _parentControl = PARENTCONTROL;
            LoadDiaries();
        }

        private void LoadDiaries()
        {
            foreach (string s in _parentControl.SymbolsDiary)
            {
                radListControl1.Items.Add(new RadListDataItem(s, s));
                ddlFilter.Items.Add(s);
            }
        }

        private void RadButton1Click(object sender, EventArgs e)
        {
            try
            {
                radListControl2.Items.Add(new RadListDataItem(radListControl1.SelectedItem.Text));
            }
            catch (Exception)
            {

            }
        }

        private void RadButton2Click(object sender, EventArgs e)
        {
            try
            {
                radListControl2.Items.Remove(radListControl2.SelectedItem);
            }
            catch (Exception)
            {


            }
        }
        
        private void DdlFilterSelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (ddlFilter.SelectedItem == null) 
                return;

            foreach (RadListDataItem r in radListControl1.Items.Where(r => r.Text == ddlFilter.SelectedItem.Text))
            {
                radListControl1.SelectedItem = r;
            }

            foreach (RadListDataItem r in radListControl2.Items.Where(r => r.Text == ddlFilter.SelectedItem.Text))
            {
                radListControl2.SelectedItem = r;
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (radListControl2.Items.Count > 0)
            {
                _parentControl.DiaryCurrentSymbols = new List<string>();

                foreach (RadListDataItem item in radListControl2.Items)
                {
                        _parentControl.DiaryCurrentSymbols.Add(item.Text);
                }

               _parentControl.Diary.FilterDiaries(_parentControl.DiaryCurrentSymbols, _parentControl.DiaryStart, _parentControl.DiaryEnd);
            }
            else
            {
                RadMessageBox.Show("Select one or more symbols!");
                return;
            }

            Close();
        }
        
    }
}
