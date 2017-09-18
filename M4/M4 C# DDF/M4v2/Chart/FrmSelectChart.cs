using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M4.DataServer.Interface.ProtocolStructs;
using M4Core.Entities;
using M4Data.List;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace M4.M4v2.Chart
{
    public partial class FrmSelectChart : RadForm
    {
        private ChartSelection _mSelection = new ChartSelection();
        private bool _checkAdvanced;
        private bool _periodicitySelected;
        private List<Stock> _lista;
        public string CodeStock { get; set; }
        private Timer _showDropDownTimeout;
        private bool _editHistory;
        private bool _editDdlStock;
        private bool _editInterval;

        public FrmSelectChart()
        {
            InitializeComponent();

            ddlStock.GotFocus += (sender, e) => ddlStock.Select();
            txtHistory.GotFocus += (sender, e) => txtHistory.Select();
            txtInterval.GotFocus += (sender, e) => txtInterval.Select();

            MinimizePanelAdvanced();
            SetBorderPanel();

            LoadDictionary();

            txtHistory.LostFocus += TxtHistoryLostFocus;
            txtInterval.LostFocus += TxtIntervalLostFocus;

            BtnDayClick(null, null);
        }

        private void LoadDictionary()
        {
            lblDescriptionSymbol.Text = Program.LanguageDefault.DictionarySelectChart["lblDescriptionSymbol"].ToUpper();
            lblDescriptionPeriodicity.Text = Program.LanguageDefault.DictionarySelectChart["lblDescriptionPeriodicity"].ToUpper();
            lblAdvanced.Text = Program.LanguageDefault.DictionarySelectChart["lblAdvanced"].ToUpper();
            lblAdvancedDescriptionPeriodicity.Text = Program.LanguageDefault.DictionarySelectChart["lblAdvancedDescriptionPeriodicity"];
            lblAdvancedDescriptionHistory.Text = Program.LanguageDefault.DictionarySelectChart["lblAdvancedDescriptionHistory"];
            lblAdvancedDescriptionInterval.Text = Program.LanguageDefault.DictionarySelectChart["lblAdvancedDescriptionInterval"];
            btnOk.Text = Program.LanguageDefault.DictionarySelectChart["btnOk"];
            btnCancel.Text = Program.LanguageDefault.DictionarySelectChart["btnCancel"];
            btnWeek.Text = Program.LanguageDefault.DictionarySelectChart["btnWeek"];
            btnYear.Text = Program.LanguageDefault.DictionarySelectChart["btnYear"];
            btnAdvancedWeek.Text = Program.LanguageDefault.DictionarySelectChart["btnAdvancedWeek"];
            btnAdvancedYear.Text = Program.LanguageDefault.DictionarySelectChart["btnAdvancedYear"];
            Text = Program.LanguageDefault.DictionarySelectChart["FrmSelectChartTitle"];
        }

        private void LoadStock()
        {
            /*string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Seamus-FS\\Plena" + "\\Base\\SYMBOL\\Symbols.xml";

            List<Stock> list = ListStocks.Instance().LoadStocks(path);
            _lista = list != null ? list.OrderBy(r => r.Name).ToList() : new List<Stock>();

            _lista.Insert(0, new Stock { Code = "", CodeName = "", Name = "", Sector = "", Source = "" });

            ddlStock.DisplayMember = "CodeName";
            ddlStock.ValueMember = "Code";
            ddlStock.DataSource = _lista;*/

            _lista = new List<Stock>();
            foreach (M4.DataServer.Interface.Symbol symbol in frmMain2.GetStockList())
            {
                _lista.Add(new Stock() { Code = symbol.Code,Source = symbol.Source });
            }
            ddlStock.DisplayMember = "Code";
            ddlStock.ValueMember = "Code";
            ddlStock.DataSource = _lista;


            ddlStock.SelectedIndex = 0;

            if (String.IsNullOrEmpty(CodeStock))
                return;

            ddlStock.SelectedValue = CodeStock;
        }

        private void FrmSelectChartLoad(object sender, EventArgs e)
        {
            _showDropDownTimeout = new Timer(components);
            _showDropDownTimeout.Tick += showDropDownTimeout_Tick;
            _showDropDownTimeout.Interval = 100;
            _showDropDownTimeout.Start();

            BackColor = Utils.GetDefaultBackColor();
        }

        private void showDropDownTimeout_Tick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CodeStock))
                ddlStock.DropDownListElement.TextBox.TextBoxItem.HostedControl.Focus();
            else
            {
                btnOk.Focus();
            }

            if (_editHistory)
            {
                txtHistory.Focus();
                _showDropDownTimeout.Tick -= showDropDownTimeout_Tick;
            }
            else if (_editInterval)
            {
                txtInterval.Focus();
                _showDropDownTimeout.Tick -= showDropDownTimeout_Tick;
            }
            else if (_editDdlStock)
            {
                ddlStock.Focus();
                _showDropDownTimeout.Tick -= showDropDownTimeout_Tick;
            }
        }

        private void SetBorderPanel()
        {
            BorderPrimitive border = (BorderPrimitive)pnlAdvanced.PanelElement.Children[1];
            border.Width = 0;
        }

        //Gets a chart selection from the user
        public ChartSelection GetChartSelection()
        {
            LoadStock();

            txtInterval.Value = decimal.Parse("1");
            txtHistory.Value = Properties.Settings.Default.History;

            DialogResult result = ShowDialog();

            return result.Equals(DialogResult.Cancel) ? null : _mSelection;
        }

        //Gets a chart selection from the user with choosed symbol
        public ChartSelection GetChartSelection(string symbol)
        {
            bool abort = true;

            LoadStock();

            foreach (var stock in _lista.Where(stock => stock.Code == symbol))
            {
                abort = false;
            }

            if (!abort)
            {
                ddlStock.SelectedValue = symbol;
                txtInterval.Value = decimal.Parse("1");
                txtHistory.Value = Properties.Settings.Default.History;
                BtnDayClick(null, null);
                btnOk.Focus();
                DialogResult result = ShowDialog();

                return result.Equals(DialogResult.Cancel) ? null : _mSelection;
            }

            return null;
        }
        private void BtnOkClick(object sender, EventArgs e)
        {
            if (ValidateFields())
                return;

            if (!Utils.IsNumeric(txtHistory.Value))
                txtHistory.Value = Properties.Settings.Default.History;

            if (!Utils.IsNumeric(txtInterval.Value))
                txtInterval.Value = decimal.Parse("1");

            if (txtInterval.Value.ToString().Contains(".") || txtInterval.Value.ToString().Contains(","))
                txtInterval.Value = decimal.Parse("1");

            _mSelection.Symbol = ddlStock.SelectedItem.Value.ToString().Trim().ToUpper();
            _mSelection.Interval = int.Parse(txtInterval.Value.ToString());
            _mSelection.Bars = int.Parse(txtHistory.Value.ToString());
            _mSelection.Source = "PLENA";//_lista.Where(r => r.Code.Equals(ddlStock.SelectedItem.Value)).Single().Source;

            PeriodicitySelect();

            // Software is prepared to deal with a history range from 2 to 50000 
            // Todo: It is not advisable to load more than 15,000 bars
            if (_mSelection.Bars > 15000)
                _mSelection.Bars = 15000;

            Properties.Settings.Default.Symbol = _mSelection.Symbol;
            Properties.Settings.Default.Interval = _mSelection.Interval;
            Properties.Settings.Default.Bars = _mSelection.Bars;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateFields()
        {
            if ((ddlStock.SelectedItem == null) || (ddlStock.SelectedItem.Value.Equals("")))
            {
                RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgSelectStock"],
                    Program.LanguageDefault.DictionaryMessage["msgWarningTitle"], MessageBoxButtons.OK, RadMessageIcon.Info);
                ddlStock.Select();
                return true;
            }

            if (!_periodicitySelected)
            {
                RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgSelectPeriodicity"],
                                   Program.LanguageDefault.DictionaryMessage["msgWarningTitle"], MessageBoxButtons.OK,
                                   RadMessageIcon.Info);
                return true;
            }

            return false;
        }

        private void PeriodicitySelect()
        {
            if (btnAdvancedMinute.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Minutely;

            else if (btnAdvancedHour.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Hourly;

            else if (btnAdvancedDay.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Daily;

            else if (btnAdvancedWeek.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Weekly;

            else if (btnAdvancedMonth.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Month;

            else if (btnAdvancedYear.Enabled.Equals(false))
                _mSelection.Periodicity = Periodicity.Year;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            _mSelection = new ChartSelection();
            DialogResult = DialogResult.Cancel;
        }

        private void FrmSelectChartFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
                _mSelection = new ChartSelection();
        }

        private void LblAdvancedClick(object sender, EventArgs e)
        {
            if (_checkAdvanced)
                MinimizePanelAdvanced();
            else
                MaximizePanelAdvanced();
        }

        private void MinimizePanelAdvanced()
        {
            _checkAdvanced = false;
            Height -= pnlAdvanced.Height;
            btnOk.Location = new Point(btnOk.Location.X, btnOk.Location.Y - pnlAdvanced.Height);
            btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y - pnlAdvanced.Height);
            pnlAdvanced.Visible = _checkAdvanced;
        }

        private void MaximizePanelAdvanced()
        {
            _checkAdvanced = true;
            Height += pnlAdvanced.Height;
            btnOk.Location = new Point(btnOk.Location.X, btnOk.Location.Y + pnlAdvanced.Height);
            btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y + pnlAdvanced.Height);
            pnlAdvanced.Visible = _checkAdvanced;
        }

        private void EnableInterval(bool enableInterval)
        {
            txtInterval.Value = decimal.Parse("1");
            txtInterval.Enabled = enableInterval;
        }

        #region Buttons Periodicity

        private void BtnOneMinuteClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnOneMinute.Enabled = false;
            BtnAdvancedMinuteClick(null, null);
            txtInterval.Value = decimal.Parse("1");
        }

        private void BtnFifteenMinuteClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnFifteenMinute.Enabled = false;
            BtnAdvancedMinuteClick(null, null);
            txtInterval.Value = decimal.Parse("15");
        }

        private void BtnThirtyMinuteClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnThirtyMinute.Enabled = false;
            BtnAdvancedMinuteClick(null, null);
            txtInterval.Value = decimal.Parse("30");
        }

        private void BtnFortyFiveMinuteClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnFortyFiveMinute.Enabled = false;
            BtnAdvancedMinuteClick(null, null);
            txtInterval.Value = decimal.Parse("45");
        }

        private void BtnSixtyMinuteClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnSixtyMinute.Enabled = false;
            BtnAdvancedHourClick(null, null);
        }

        private void BtnDayClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnDay.Enabled = false;
            BtnAdvancedDayClick(null, null);
        }

        private void BtnWeekClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnWeek.Enabled = false;
            BtnAdvancedWeekClick(null, null);
        }

        private void BtnMonthClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnMonth.Enabled = false;
            BtnAdvancedMonthClick(null, null);
        }

        private void BtnYearClick(object sender, EventArgs e)
        {
            EnableButtons();
            btnYear.Enabled = false;
            BtnAdvancedYearClick(null, null);
        }

        private void EnableButtons()
        {
            btnOneMinute.Enabled = true;
            btnFifteenMinute.Enabled = true;
            btnThirtyMinute.Enabled = true;
            btnFortyFiveMinute.Enabled = true;
            btnSixtyMinute.Enabled = true;
            btnDay.Enabled = true;
            btnWeek.Enabled = true;
            btnMonth.Enabled = true;
            btnYear.Enabled = true;
        }

        #endregion

        #region Advanced Periodicity

        private void BtnAdvancedMinuteClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedMinute.Enabled = false;
            EnableInterval(true);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void BtnAdvancedHourClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedHour.Enabled = false;
            EnableInterval(true);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void BtnAdvancedDayClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedDay.Enabled = false;
            EnableInterval(true);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void BtnAdvancedWeekClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedWeek.Enabled = false;
            EnableInterval(false);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void BtnAdvancedMonthClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedMonth.Enabled = false;
            EnableInterval(true);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void BtnAdvancedYearClick(object sender, EventArgs e)
        {
            EnableButtonsAdvanced();
            btnAdvancedYear.Enabled = false;
            EnableInterval(false);

            if (sender != null)
                EnableButtons();

            _periodicitySelected = true;
        }

        private void EnableButtonsAdvanced()
        {
            btnAdvancedMinute.Enabled = true;
            btnAdvancedHour.Enabled = true;
            btnAdvancedDay.Enabled = true;
            btnAdvancedWeek.Enabled = true;
            btnAdvancedMonth.Enabled = true;
            btnAdvancedYear.Enabled = true;
        }

        #endregion

        private void TxtIntervalKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!btnOneMinute.Enabled)
            {
                if (!txtInterval.Value.ToString().Equals("1"))
                    btnOneMinute.Enabled = true;
            }
            else if (!btnFifteenMinute.Enabled)
            {
                if (!txtInterval.Value.ToString().Equals("15"))
                    btnFifteenMinute.Enabled = true;
            }
            else if (!btnThirtyMinute.Enabled)
            {
                if (!txtInterval.Value.ToString().Equals("30"))
                    btnThirtyMinute.Enabled = true;
            }
            else if (!btnFortyFiveMinute.Enabled)
            {
                if (!txtInterval.Value.ToString().Equals("45"))
                    btnFortyFiveMinute.Enabled = true;
            }
        }

        private void TxtIntervalLostFocus(object sender, EventArgs e)
        {
            int interval = int.Parse(txtInterval.Value.ToString());

            if ((!btnAdvancedMinute.Enabled) && ((interval > 420) || (interval < 1)))
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgIntervalMaximum"]);
                txtInterval.Value = txtInterval.Value > 420 ? decimal.Parse("420") : decimal.Parse("1");
            }
            else if ((!btnAdvancedHour.Enabled) && ((interval > 7) || (interval < 1)))
            {
                Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgIntervalMaximum"]);
                txtInterval.Value = txtInterval.Value > 7 ? decimal.Parse("7") : decimal.Parse("1");
            }
        }

        private void TxtHistoryLostFocus(object sender, EventArgs e)
        {
            if ((txtHistory.Value <= 15000) && (txtHistory.Value >= 2))
                return;

            Telerik.WinControls.RadMessageBox.Show(Program.LanguageDefault.DictionaryMessage["msgHistoryMaximum"]);
            txtHistory.Value = txtHistory.Value > 15000 ? decimal.Parse("15000") : decimal.Parse("2");
        }

        private void EscapeForm()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FrmSelectChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;

            EscapeForm();
        }

        private void txtHistory_MouseMove(object sender, MouseEventArgs e)
        {
            _editHistory = true;
        }

        private void txtInterval_MouseMove(object sender, MouseEventArgs e)
        {
            _editInterval = true;
        }

        private void ddlStock_MouseMove(object sender, MouseEventArgs e)
        {
            _editDdlStock = true;
        }

        private void ddlStock_Leave(object sender, EventArgs e)
        {
            try
            {
                ddlStock.SelectedItem = ddlStock.Items.First(i => i.Text == ddlStock.Text);
            }
            catch (Exception ex){ }
        }
    }
}