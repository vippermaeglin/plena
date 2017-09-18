/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;
using M4Core.Entities;
using Nevron.UI.WinForm.Controls;

namespace M4
{
    public partial class frmSelectChart : NForm
    {
        private ChartSelection m_selection = new ChartSelection();

        public frmSelectChart()
        {
            InitializeComponent();

            if (frmMain.NevronPalette != null)
                Palette = frmMain.NevronPalette;

            txtSymbol.GotFocus += (sender, e) => txtSymbol.SelectAll();
            txtBars.GotFocus += (sender, e) => txtBars.SelectAll();
            txtInterval.GotFocus += (sender, e) => txtInterval.SelectAll();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            txtSymbol.Text = txtSymbol.Text.Trim().ToUpper();
            if (!Utils.IsNumeric(txtBars.Text)) txtBars.Text = "100";
            if (!Utils.IsNumeric(txtInterval.Text)) txtInterval.Text = "1";
            if (txtInterval.Text.Contains(".") || txtInterval.Text.Contains(",")) txtInterval.Text = "1";
            m_selection.Symbol = txtSymbol.Text.Trim().ToUpper();
            m_selection.Interval = int.Parse(txtInterval.Text);
            m_selection.Bars = int.Parse(txtBars.Text);
            switch (cboPeriodicity.Text)
            {
                //Secondly is not supported at this time
                case "Minute":
                    m_selection.Periodicity = Periodicity.Minutely;
                    break;

                case "Hour":
                    m_selection.Periodicity = Periodicity.Hourly;
                    break;

                case "Day":
                    m_selection.Periodicity = Periodicity.Daily;
                    break;

                case "Week":
                    m_selection.Periodicity = Periodicity.Weekly;
                    break;
            }

            // Todo: It is not advisable to load more than 50,000 bars
            if (m_selection.Bars > 50000) 
                m_selection.Bars = 50000;

            Properties.Settings.Default.Symbol = m_selection.Symbol;
            Properties.Settings.Default.Interval = m_selection.Interval;
            Properties.Settings.Default.Bars = m_selection.Bars;
            DialogResult = DialogResult.OK;
            Close();
        }

        //Gets a chart selection from the user
        public ChartSelection GetChartSelection()
        {
            txtSymbol.Text = Properties.Settings.Default.Symbol;
            txtInterval.Text = Properties.Settings.Default.Interval.ToString();
            txtBars.Text = Properties.Settings.Default.Bars.ToString();

            if (txtInterval.Text == "0")
                txtInterval.Text = "15";
            if (txtBars.Text == "0")
                txtBars.Text = "250";
            cboPeriodicity.SelectedIndex = 0;
            ShowDialog();
            return m_selection;
        }

        private void cboPeriodicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboPeriodicity.Text)
            {
                case "Day":
                case "Week":
                    txtInterval.Text = "1";
                    txtInterval.Enabled = false;
                    break;
                default:
                    txtInterval.Enabled = true;
                    break;
            }
        }

        private void frmSelectChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                m_selection = new ChartSelection();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            m_selection = new ChartSelection();
            DialogResult = DialogResult.Cancel;
        }
    }

}
