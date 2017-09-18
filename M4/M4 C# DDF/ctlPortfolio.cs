/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using M4.modulusfe.platform;

//M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved.
//http://www.modulusfe.com

//#############################
//TODO: WARNING! Example code only! 
//Your order entry API is responsible for returning order status events to update the portfolio!
//NOTE! It is HIGHLY recommended that you request fresh portfolio entries from your server 
//each time a portfolio is selected, instead of relying on values loaded from the local file.
//Because this control requires modification to work with your order entry API, this control 
//comes without any warranty, neither expressed nor implied. Modify, test and use at your own risk.
//NOTE: You should add your own login screen to this form. You may reuse the data feed login form.
//#############################

namespace M4
{
    public partial class ctlPortfolio : UserControl
    {
        #region Members and Classes
        private readonly frmMain2 m_frmMain;
        private readonly ctlData m_ctlData;
        private int m_RowIndex;
        private string m_portfolioName;
        private double m_StartingBalance;
        private string m_prevPortfolio;
        private readonly Service svc = new Service();
        private readonly CultureInfo ciEnUs = new CultureInfo("en-us");

        private readonly Random objRandom = new Random();

        public class Orders
        {
            public enum Status
            {
                Sending = 1,        //Order is being transmitted
                Sent = 2,           //Order has been routed
                Received = 3,       //Order has been received
                CancelSending = 4,  //Cancel request is being sent 
                CancelSent = 5,     //Cancel request has been sent
                CancelReceived = 6, //Cancel request has been received 
                Out = 7,            //Your order is out of the market 
                Filled = 8,         //Order has been filled 
                PartialFill = 9,    //Your order has been partially filled 
                Expired = 10,       //Time limit of the order has expired 
                Rejected = 11,      //Order was rejected
                Unknown = 12        //Indicates a problem - call your broker!
            }

            public enum Side
            {
                LongSide = 1,
                ShortSide = 2,
                Unknown = 3
            }

            public static string SideToString(Side Value)
            {
                return Value == Side.LongSide ? "Long" : "Short";
            }

            public static Side SideFromString(string Value)
            {
                return Value == "Long" ? Side.LongSide : Side.ShortSide;
            }

            public static string StatusToString(Status Value)
            {
                switch (Value)
                {
                    case Status.Sending:
                        return "Sending";
                    case Status.Sent:
                        return "Sent";
                    case Status.Received:
                        return "Received";
                    case Status.CancelSending:
                        return "Cancel Sending";
                    case Status.CancelSent:
                        return "Cancel Sent";
                    case Status.CancelReceived:
                        return "Cancel Received";
                    case Status.Out:
                        return "Out";
                    case Status.Filled:
                        return "Filled";
                    case Status.PartialFill:
                        return "Partial Fill";
                    case Status.Expired:
                        return "Expired";
                    case Status.Rejected:
                        return "Rejected";
                    case Status.Unknown:
                        return "Unknown";
                }
                return "Unknown";
            }

            public static Status StatusFromString(string Value)
            {
                switch (Value)
                {
                    case "Sending":
                        return Status.Sending;
                    case "Sent":
                        return Status.Sent;
                    case "Received":
                        return Status.Received;
                    case "Cancel Sending":
                        return Status.CancelSending;
                    case "Cancel Sent":
                        return Status.CancelSent;
                    case "Cancel Received":
                        return Status.CancelReceived;
                    case "Out":
                        return Status.Out;
                    case "Filled":
                        return Status.Filled;
                    case "Partial Fill":
                        return Status.PartialFill;
                    case "Expired":
                        return Status.Expired;
                    case "Rejected":
                        return Status.Rejected;
                    case "Unknown":
                        return Status.Unknown;
                }
                return Status.Unknown;
            }
        }

        public class Order
        {
            public string Exchange;
            public Expiration Expires;
            public double LimitPrice;
            public OrderType _Order;
            public string OrderID;
            public string Password;
            public int Quantity;
            public Orders.Side Side;
            public string Symbol;
            public string UserID;

            //'######## TODO: These four must be set by your order entry API:
            public DateTime EntryTime;
            public DateTime ExecTime;
            public Orders.Status Status;

            public enum Expiration
            {
                GoodTillCanceled = 1,
                GoodTillCanceledPlusAfterHours = 2,
                DayOrder = 3,
                DayOrderPlusAfterHours = 4,
                Unknown = 5
            }

            public enum OrderType
            {
                Limit = 1,
                Market = 2,
                StopLimit = 3,
                StopMarket = 4,
                Unknown = 5
            }

            public static string OrderTypeToString(OrderType Value)
            {
                switch (Value)
                {
                    case OrderType.Limit:
                        return "Limit";
                    case OrderType.Market:
                        return "Market";
                    case OrderType.StopLimit:
                        return "Stop Limit";
                    case OrderType.StopMarket:
                        return "Stop Market";
                }
                return "Unknown";
            }

            public static OrderType OrderTypeFromString(string Value)
            {
                switch (Value)
                {
                    case "Limit":
                        return OrderType.Limit;
                    case "Market":
                        return OrderType.Market;
                    case "Stop Limit":
                        return OrderType.StopLimit;
                    case "Stop Market":
                        return OrderType.StopMarket;
                }
                return OrderType.Unknown;
            }

            public static string ExpirationToString(Expiration Value)
            {
                switch (Value)
                {
                    case Expiration.GoodTillCanceled:
                        return "GTC";
                    case Expiration.GoodTillCanceledPlusAfterHours:
                        return "GTC+EXT";
                    case Expiration.DayOrder:
                        return "Day";
                    case Expiration.DayOrderPlusAfterHours:
                        return "Day+EXT";
                }
                return "Unknown";
            }

            public static Expiration ExpirationFromString(string Value)
            {
                switch (Value)
                {
                    case "Day":
                        return Expiration.DayOrder;
                    case "Day+EXT":
                        return Expiration.DayOrderPlusAfterHours;
                    case "GTC":
                        return Expiration.GoodTillCanceled;
                    case "GTC+EXT":
                        return Expiration.GoodTillCanceledPlusAfterHours;
                }
                return Expiration.Unknown;
            }
        }
        #endregion

        #region Initialization and Controls
        public ctlPortfolio(frmMain2 oMain, ctlData oData)
        {
            InitializeComponent();

            m_frmMain = oMain;
            m_ctlData = oData;

            //Initialize portfolios list
            InitPortfolioCombo();

            //Disable UI controls untill a portfolio is selected
            EnableUIControls(false);

            //TODO: Add supported exchanges here via your order entry API
            m_PortfolioGrid.AllowUserToResizeColumns = true;
            cboExchanges.Items.Add("NASDAQ");
            cboExchanges.Items.Add("NYSE");
            cboExchanges.Items.Add("AMEX");
            cboExchanges.SelectedIndex = 0;

            //Disable sorting (important)
            foreach (DataGridViewColumn column in m_PortfolioGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //Enable/disable controls
        private void EnableUIControls(bool bEnable)
        {
            foreach (Control ctrl in pnlPortfolio.Controls)
            {
                if (ctrl.Name != "cmbPortfolio" && ctrl.Name != "lblPortfolio")
                {
                    ctrl.Enabled = bEnable;
                }
            }
            Application.DoEvents();
        }

        //Handling this event to select symbol for OrderEntry form
        private void m_PortfolioGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button != MouseButtons.Right) || (m_PortfolioGrid.RowCount <= 0)) return;

            //Using HitTest to retrieve row by position in grid
            string sSymbol;
            Orders.Status status;
            DataGridView.HitTestInfo htInfo = m_PortfolioGrid.HitTest(e.X, e.Y);
            if (htInfo.Type == DataGridViewHitTestType.Cell)
            {
                sSymbol = m_PortfolioGrid.Rows[htInfo.RowIndex].Cells["Symbol"].Value.ToString();
                status = Orders.StatusFromString(m_PortfolioGrid.Rows[htInfo.RowIndex].Cells["Status"].Value.ToString());
                m_RowIndex = htInfo.RowIndex;
            }
            else
            {
                sSymbol = m_PortfolioGrid.SelectedRows[0].Cells["Symbol"].Value.ToString();
                status = Orders.StatusFromString(m_PortfolioGrid.SelectedRows[0].Cells["Status"].Value.ToString());
            }

            //Can the order be cancelled?
            if (((((((status == Orders.Status.CancelReceived) | (status == Orders.Status.CancelSending)) |
                    (status == Orders.Status.CancelSent)) | (status == Orders.Status.Expired)) |
                  (status == Orders.Status.Filled)) | (status == Orders.Status.Out)) | (status == Orders.Status.Rejected))
            {
                miCancelOrder.Enabled = false;
            }
            else
            {
                miCancelOrder.Enabled = true;
            }
            miChartSymbol.Enabled = true;
            miChartSymbol.Text = "Chart " + sSymbol;
            miCancelOrder.Text = "Cancel " + sSymbol;
        }

        //Cancel an order
        private void miCancelOrder_Click(object sender, EventArgs e)
        {
            if (m_RowIndex >= m_PortfolioGrid.Rows.Count)
            {
                return;
            }

            m_PortfolioGrid.Rows[m_RowIndex].Selected = true;
            if (!miCancelOrder.Enabled)
            {
                return;
            }

            string orderID = m_PortfolioGrid.SelectedRows[0].Cells["OrderID"].Value.ToString();
            string symbol = Convert.ToString(m_PortfolioGrid.SelectedRows[0].Cells["Symbol"].Value);
            if (Telerik.WinControls.RadMessageBox.Show("Cancel order " + orderID + " for " + symbol + "?",
                                "Confirm", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question) != DialogResult.OK) return;

            //##############
            //TODO: Send a cancel message to your order entry API for this symbol
            //##############      
            ExecuteOrder(orderID, Orders.Status.CancelSending, symbol, DateTime.MinValue, Orders.Side.Unknown, 0, 0.0,
                         Order.OrderType.Unknown, Order.Expiration.Unknown, 0.0);
        }

        //Display a chart
        private void miChartSymbol_Click(object sender, EventArgs e)
        {
            m_PortfolioGrid.Rows[m_RowIndex].Selected = true;
            if (!miChartSymbol.Enabled) return;

            string symbol = Convert.ToString(m_PortfolioGrid.SelectedRows[0].Cells["Symbol"].Value);
            Properties.Settings.Default.Symbol = symbol;
           // m_frmMain.CreateNewChart();
        }

        //Initialize the combo
        private void InitPortfolioCombo()
        {
            cmbPortfolio.Items.Clear();
            cmbPortfolio.Items.Add("<new portfolio...>");
            cmbPortfolio.Items[0] = "New";
            LoadPortfolioNamesToCombo();
        }

        //Load a portfolio
        private void cmbPortfolio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPortfolio.SelectedIndex < 0) return;

            if (cmbPortfolio.Items[cmbPortfolio.SelectedIndex].ToString() == "New")
            {
                EnableUIControls(false);

                //Create a new portfolio
                cmdDeletePortfolio.Enabled = false;
                cmdDeletePortfolio.Visible = false;
                //frmNewPortfolio.NewPortfolio selection = (new frmNewPortfolio()).GetPortfolio();
                //if (string.IsNullOrEmpty(selection.PortfolioName))
                //{
                //    cmbPortfolio.SelectedIndex = -1;
                //    EnableUIControls(false);
                //    return;
                //}
                //m_portfolioName = selection.PortfolioName;
                //CreatePortfolio(selection);
            }
            else
            {

                //Automatically save the previous portfolio
                if (m_prevPortfolio != cmbPortfolio.Text && !string.IsNullOrEmpty(m_prevPortfolio))
                {
                    SavePortfolio(m_prevPortfolio); //Save the portfolio before the new one is loaded
                }
                m_PortfolioGrid.Rows.Clear();
                m_prevPortfolio = cmbPortfolio.Text;
                cmdDeletePortfolio.Visible = true;
                m_portfolioName = m_prevPortfolio;
                LoadPortfolio();
            }
        }

        //Creates a new encrypted portfolio file
        //private void CreatePortfolio(frmNewPortfolio.NewPortfolio selection)
        //{

        //    //Create and encrypt the new portfolio
        //    string portfolio = selection.PortfolioName + "\r\n" + Convert.ToString(selection.StartingBalance);
        //    portfolio = new Security().Encrypt(portfolio);
        //    m_StartingBalance = selection.StartingBalance;

        //    cmbPortfolio.Enabled = false;
        //    EnableUIControls(false);

        //    //Check the web service to see if this portfolio exists already        
        //    string data = string.Empty;
        //    try
        //    {
        //        data = svc.GetUserData(frmMain.ClientId, frmMain.ClientPassword, frmMain.LicenseKey,
        //                                    "Portfolio: " + selection.PortfolioName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }

        //    if (!string.IsNullOrEmpty(data) &&
        //        (Telerik.WinControls.RadMessageBox.Show("Overwrite existing portfolio?", "Warning!", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Question) ==
        //         DialogResult.No))
        //    {
        //        cmbPortfolio.Enabled = true;
        //        EnableUIControls(true);
        //        return;
        //    }

        //    //Create a new portfolio using the web service
        //    try
        //    {
        //        svc.SetUserData(frmMain.ClientId, frmMain.ClientPassword, frmMain.LicenseKey,
        //                             "Portfolio: " + selection.PortfolioName, portfolio);
        //    }
        //    catch (Exception ex)
        //    {
        //        Telerik.WinControls.RadMessageBox.Show("Failed to create portfolio." + Environment.NewLine +
        //                        "Error: " + ex.Message, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
        //        cmbPortfolio.Enabled = true;
        //        EnableUIControls(true);
        //        return;
        //    }

        //    //Refresh the combo box
        //    InitPortfolioCombo();

        //    //Select this portfolio (loop beacuse its auto sorted)
        //    for (int n = 0; n <= cmbPortfolio.Items.Count - 1; n++)
        //    {
        //        if (cmbPortfolio.Items[n].Text != selection.PortfolioName) continue;

        //        cmbPortfolio.SelectedIndex = n;
        //        cmbPortfolio.Enabled = true;
        //        EnableUIControls(true);
        //        break;
        //    }
        //}

        //Delete's a portfolio
        private void cmdDeletePortfolio_Click(object sender, EventArgs e)
        {
            //if (Telerik.WinControls.RadMessageBox.Show("Delete the " + cmbPortfolio.Text + " portfolio?",
            //                    "Warning!", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}

            //EnableUIControls(false);
            //cmbPortfolio.Enabled = false;

            ////Delete the portfolio from the web service
            //try
            //{
            //    svc.SetUserData(frmMain.ClientId, frmMain.ClientPassword, frmMain.LicenseKey,
            //                    "Portfolio: " + cmbPortfolio.Text, "");
            //}
            //catch (Exception)
            //{
            //    Telerik.WinControls.RadMessageBox.Show("Failed to delete portfolio", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
            //    EnableUIControls(true);
            //    return;
            //}

            ////Refresh the combo box
            //InitPortfolioCombo();

            ////Select this portfolio
            //if (cmbPortfolio.Items.Count > 0)
            //{
            //    cmbPortfolio.SelectedIndex = cmbPortfolio.Items.Count - 1;
            //}

            //if (cmbPortfolio.Items[cmbPortfolio.Items.Count - 1].Text == "<new portfolio...>")
            //{
            //    cmbPortfolio.SelectedIndex = -1;
            //    EnableUIControls(false);
            //}
            //else
            //{
            //    cmbPortfolio.SelectedIndex = cmbPortfolio.Items.Count - 1;
            //    EnableUIControls(true);
            //}

            //cmbPortfolio.Enabled = true;
            //EnableUIControls(true);
        }

        private void ctlPortfolio_Resize(object sender, EventArgs e)
        {
            try
            {
                grpAccountSummary.Width = (Width - 9) - grpAccountSummary.Left;
            }
            catch (Exception)
            {
                //Just in case
            }
            //int x1 = grpExpires.Left + grpExpires.Width;
            //int x2 = Width;
            //btnSubmit.Left = (((x2 - x1) / 2) + x1) - (btnSubmit.Width / 2)
        }
        #endregion

        #region Saving and Loading Portfolios

        //Saves the portfolio that is already loaded
        public void SavePortfolio()
        {
            SavePortfolio("");
        }
        public void SavePortfolio(string name)
        {
            //Encrypted file format:
            //PortfolioName
            //StartingBalance        
            //Orderid, status, details, time, symbol, type, qty, entry. Last three are calcualted: last, $ gain, % gain

            if (cmbPortfolio.Text == "<new portfolio...>" || !cmbPortfolio.Enabled) return;
            if (name == "") name = cmbPortfolio.Text;
            if (name == "") return;

            cmbPortfolio.Enabled = false;
            EnableUIControls(false);

            //Save the portfolio
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(m_portfolioName).Append(m_StartingBalance.ToString(ciEnUs));
            if (m_PortfolioGrid.Rows.Count > 0)
            {
                sb.AppendLine("");
            }
            for (int n = 0; n < m_PortfolioGrid.Rows.Count; n++)
            {
                sb.Append(Convert.ToString(m_PortfolioGrid.Rows[n].Cells["OrderID"].Value, ciEnUs)).Append("|");
                sb.Append(m_PortfolioGrid.Rows[n].Cells["Status"].Value).Append("|");
                sb.Append(m_PortfolioGrid.Rows[n].Cells["Details"].Value).Append("|");
                sb.Append(Convert.ToString(m_PortfolioGrid.Rows[n].Cells["Time"].Value, ciEnUs)).Append("|");
                sb.Append(m_PortfolioGrid.Rows[n].Cells["Symbol"].Value).Append("|");
                sb.Append(m_PortfolioGrid.Rows[n].Cells["Type"].Value).Append("|");
                sb.Append(Convert.ToString(m_PortfolioGrid.Rows[n].Cells["Qty"].Value, ciEnUs)).Append("|");
                sb.Append(Convert.ToString(m_PortfolioGrid.Rows[n].Cells["Entry"].Value, ciEnUs)).Append("|");

                if (n < (m_PortfolioGrid.Rows.Count - 1))
                {
                    sb.AppendLine();
                }
            }

            //Encrypt the portfolio
            string data = new Security().Encrypt(sb.ToString());

            //Save the portfolio via the web service
            try
            {
                svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey, "Portfolio: " + name, data);
            }
            catch (Exception)
            {
                Telerik.WinControls.RadMessageBox.Show("Failed to save portfolio", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                cmbPortfolio.Enabled = true;
                EnableUIControls(true);
                return;
            }

            cmbPortfolio.Enabled = true;
            EnableUIControls(true);
        }

        //Loads the list of available portfolios
        private void LoadPortfolioNamesToCombo()
        {
            //Retrieve available portfolios from the server

            //List all portfolios in the user's web service entry list
            string[] portfolios = null;
            try
            {
                object[] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
                if (_ != null)
                {
                    portfolios = new string[_.Length];
                    for (int i = 0; i < _.Length; i++)
                    {
                        portfolios[i] = _[i].ToString();
                    }
                }
            }
            catch (Exception)
            {
                //No need to tell the user there are no portfolios
                return;
            }
            if (portfolios == null) return;

            //Add them to combobox
            for (int n = 0; n <= portfolios.Length - 1; n++)
            {
                if (portfolios[n].StartsWith("Portfolio: "))
                {
                    cmbPortfolio.Items.Add(portfolios[n].Replace("Portfolio: ", ""));
                    cmbPortfolio.Items[cmbPortfolio.Items.Count - 1] = "";
                }
            }
        }

        //Loads an encrypted portfolio file
        private void LoadPortfolio()
        {
            //Load the portfolio from the web service
            string portfolio = cmbPortfolio.Text;
            string encryptedPortfolio;
            try
            {
                encryptedPortfolio = svc.GetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                                                     "Portfolio: " + portfolio);
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show("Failed to load portfolio. " + Environment.NewLine +
                                "Error: " + ex.Message, " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }

            //Descript the portfolio
            Security secure = new Security();
            string decryptedPortfolio;
            try
            {
                decryptedPortfolio = secure.Decrypt(encryptedPortfolio);
            }
            catch (Exception)
            {
                Telerik.WinControls.RadMessageBox.Show("Failed to load portfolio", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }

            //The first two lines are portfolio name and starting balance
            string[] rows = decryptedPortfolio.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            m_portfolioName = rows[0];
            try
            {
                m_StartingBalance = Convert.ToDouble(rows[1], ciEnUs);
            }
            catch { }

            //Orderid, status, details time, symbol, type, qty, entry, last, $ gain, % gain
            if (rows.Length > 2)
            {
                m_PortfolioGrid.Rows.Clear();
                for (int row = 2; row < rows.Length; row++)
                {
                    string[] cols = rows[row].Split('|');
                    string orderId = cols[0];
                    string status = cols[1];
                    string details = cols[2];
                    DateTime time = DateTime.Parse(cols[3], ciEnUs);
                    string symbol = cols[4];
                    string type = cols[5];
                    int qty = int.Parse(cols[6], ciEnUs);
                    double entry = double.Parse(cols[7], ciEnUs);

                    m_PortfolioGrid.Rows.Add(new object[]
                                     {
                                       orderId, status, details, time, symbol, type, qty, entry
                                     });
                }

                UpdatePortolioScreen();
            }

            EnableUIControls(true);
        }

        #endregion

        #region Calculations
        //Shows portfolio summary
        private void UpdatePortolioScreen()
        {
            GetLastPrices();
            CalculateGains();
            CalculatePortfolioValue();
        }

        private void CalculateGains()
        {
            foreach (DataGridViewRow row in m_PortfolioGrid.Rows)
            {
                Orders.Status status = Orders.StatusFromString(Convert.ToString(row.Cells["Status"].Value));

                if (!(((status != Orders.Status.CancelReceived) & (status != Orders.Status.Rejected)) &
                      (status != Orders.Status.Expired)))
                {
                    continue;
                }

                double pctGain = 0.0;
                double valGain = 0.0;
                double LastPrice = Format.FromUsCurrency(row.Cells["Last"].Value);
                double EntryPrice = Format.FromUsCurrency(row.Cells["Entry"].Value);
                int Quantity = (int)Math.Round((double)Format.FromLocalInteger(row.Cells["Qty"].Value));

                if (row.Cells["Type"].Value.ToString() == "Long") //Long position                    
                {
                    if ((LastPrice != 0.0) & (EntryPrice != 0.0))
                    {
                        valGain = LastPrice - EntryPrice;
                        pctGain = Math.Round((LastPrice / EntryPrice) - 1.0, 4) * 100.0;
                    }
                }
                else if (row.Cells["Type"].Value.ToString() == "Short") //Short position
                {
                    if ((LastPrice != 0.0) & (EntryPrice != 0.0))
                    {
                        valGain = EntryPrice - LastPrice;
                        pctGain = Math.Round((EntryPrice / LastPrice) - 1.0, 4) * 100.0;
                    }
                }
                else
                {
                    valGain = 0.0;
                    pctGain = 0.0;
                }

                if (valGain > 0.0)
                {
                    row.Cells["DollarGain"].Style.ForeColor = Color.Green;
                    row.Cells["PercentGain"].Style.ForeColor = Color.Green;
                }
                else if (valGain < 0.0)
                {
                    row.Cells["DollarGain"].Style.ForeColor = Color.Red;
                    row.Cells["PercentGain"].Style.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["DollarGain"].Style.ForeColor = Color.DarkBlue;
                    row.Cells["PercentGain"].Style.ForeColor = Color.DarkBlue;
                }
                row.Cells["DollarGain"].Value = Format.ToUsCurrency(valGain * Quantity);
                row.Cells["PercentGain"].Value = Convert.ToString(Math.Round(pctGain, 2)) + "%";
            }
        }

        //Calculates values for entire portfolio (top label)
        private void CalculatePortfolioValue()
        {
            double cashUsed = 0.0;
            double totalPnL = 0.0;

            foreach (DataGridViewRow row in m_PortfolioGrid.Rows)
            {
                Orders.Status status = Orders.StatusFromString(Convert.ToString(row.Cells["Status"].Value));
                if (!(((status != Orders.Status.CancelReceived) & (status != Orders.Status.Rejected)) &
                    (status != Orders.Status.Expired)))
                {
                    continue;
                }

                double entryPrice = Format.FromUsCurrency(row.Cells["Entry"].Value);
                double dollarGain = Format.FromUsCurrency(row.Cells["DollarGain"].Value);
                int qty = Format.FromLocalInteger(row.Cells["Qty"].Value);

                cashUsed += entryPrice * qty;
                totalPnL += dollarGain;

                //cashUsed += Convert.ToDouble(row.Cells["Entry"].Value) * Convert.ToDouble(row.Cells["Qty"].Value);
                //totalPnL += double.Parse(row.Cells["DollarGain"].Value.ToString(), NumberStyles.Currency);
            }

            string PortfolioVal = "Portfolio value: " + Format.ToUsCurrency(m_StartingBalance + totalPnL);
            string TotalPL = "Total P&L: " + Format.ToUsCurrency(totalPnL);
            string AccountBalance = "Account balance (cash): " + Format.ToUsCurrency(m_StartingBalance - cashUsed);
            string Trades = "Trades: " + m_PortfolioGrid.Rows.Count;
        }
        #endregion

        #region Real Time Data Aquisition
        private void GetLastPrices()
        {
            foreach (DataGridViewRow row in m_PortfolioGrid.Rows)
            {
                string symbol = Convert.ToString(row.Cells["Symbol"].Value);

                //Update the price only if we're not out of the position
                Orders.Status status = Orders.StatusFromString(Convert.ToString(row.Cells["Status"].Value));
                if ((status != Orders.Status.Out) & (status != Orders.Status.CancelReceived))
                {
                    row.Cells["Last"].Value = Format.ToUsCurrency(GetLastPrice(symbol));
                }
            }
        }

        public double GetLastPrice(string symbol)
        {
            return m_ctlData.GetLastPriceAndVolume(symbol).Price;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            //Poll for last prices once per second
            bool update = false;
            for (int n = 0; n <= m_PortfolioGrid.Rows.Count - 1; n++)
            {
                string symbol = Convert.ToString(m_PortfolioGrid.Rows[n].Cells["Symbol"].Value);
                double lastPrice = GetLastPrice(symbol);
                try
                {
                    double gridPrice = Format.FromUsCurrency(m_PortfolioGrid.Rows[n].Cells["Last"].Value);
                    if (lastPrice != gridPrice)
                    {
                        m_PortfolioGrid.Rows[n].Cells["Last"].Value = Format.ToUsCurrency(lastPrice);
                        update = true;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
            if (update)
            {
                UpdatePortolioScreen(); //Recalc the portfolio if a price has changed
            }
        }

        #endregion

        #region Numeric Formatting
        //Ensures that only numeric values are entered
        private void txtStopLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8) return;
            string str = e.KeyChar.ToString();
            if (!Utils.IsNumeric(str) && str != "," && str != "." && str != Utils.GetCurrencySymbol())
            {
                str = string.Empty;
            }
            txtStopLimit.SelectedText += str;
            e.Handled = true;
        }

        private void txtStopLimit_TextChanged(object sender, EventArgs e)
        {
            if (!Utils.IsNumeric(txtStopLimit.Text) && txtStopLimit.Text != Utils.GetCurrencySymbol())
            {
                txtStopLimit.Text = string.Empty;
            }
        }
        #endregion

        #region Order Entry
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtSymbol.Text == "")
            {
                Telerik.WinControls.RadMessageBox.Show("Invalid Symbol!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }
            if (udQuantity.Text == "0" || !Utils.IsNumeric(udQuantity.Text))
            {
                Telerik.WinControls.RadMessageBox.Show("Invalid quantity!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }
            if (txtStopLimit.Text == "0" || !Utils.IsNumeric(txtStopLimit.Text) && !rdoMarket.Checked)
            {
                Telerik.WinControls.RadMessageBox.Show("Invalid Stop / Limit Price!", " ", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Exclamation);
                return;
            }
            if (Telerik.WinControls.RadMessageBox.Show("Confirm trade for portfolio " + cmbPortfolio.Text,
                                "Confirm", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question) != DialogResult.OK) return;

            //Send the order
            SubmitOrder();
        }

        private void SubmitOrder()
        {
            //Gather the order details
            double stopLimit = 0;
            try
            {
                Convert.ToDouble(txtStopLimit.Text);
            }
            catch { }

            Order MyOrder = new Order
                              {
                                  OrderID = CreateOrderID(),
                                  Side = rdoBtnBuy.Checked ? Orders.Side.LongSide : Orders.Side.ShortSide,
                                  Quantity = Convert.ToInt32(udQuantity.Text),
                                  Exchange = cboExchanges.Text,
                                  Symbol = txtSymbol.Text.ToUpper().Trim(),
                                  LimitPrice = string.IsNullOrEmpty(txtStopLimit.Text) ? 0 : stopLimit,
                              };
            if (rdoLimit.Checked)
            {
                MyOrder._Order = Order.OrderType.Limit;
            }
            else if (rdoMarket.Checked)
            {
                MyOrder._Order = Order.OrderType.Market;
            }
            else if (rdoStopLimit.Checked)
            {
                MyOrder._Order = Order.OrderType.StopLimit;
            }
            else if (rdoStopMarket.Checked)
            {
                MyOrder._Order = Order.OrderType.StopMarket;
            }
            if (rdoGTC.Checked)
            {
                MyOrder.Expires = Order.Expiration.GoodTillCanceled;
            }
            else if (rdoGTCHours.Checked)
            {
                MyOrder.Expires = Order.Expiration.GoodTillCanceledPlusAfterHours;
            }
            else if (rdoDay.Checked)
            {
                MyOrder.Expires = Order.Expiration.DayOrder;
            }
            else if (rdoDayHours.Checked)
            {
                MyOrder.Expires = Order.Expiration.DayOrderPlusAfterHours;
            }

            //#### TODO: WARNING! Example code only! Your order entry API is responsible
            //for sending/receiving orders to update this control. This example just
            //sends the order straight to the DataViewGrid control! Also the exec time 
            //and status should be set by the server.
            MyOrder.ExecTime = DateTime.Now;
            MyOrder.Status = Orders.Status.Sending;
            ExecuteOrder(MyOrder.OrderID, MyOrder.Status, MyOrder.Symbol, MyOrder.ExecTime, MyOrder.Side,
                              MyOrder.Quantity, GetLastPrice(MyOrder.Symbol), MyOrder._Order, MyOrder.Expires,
                              MyOrder.LimitPrice);
            SavePortfolio(); //Optional - the portfolio will be saved when this control is closed
        }

        //#############
        //TODO: Your order entry API should return a unique order ID
        //This function is for example purposes only.
        //#############
        public string CreateOrderID()
        {
            string s = "";
            const string bad = "0O0Iil1B8";
            for (int n = 1; n <= 15; n++)
            {
                string sChr;
                if (objRandom.NextDouble() > 0.5)
                {
                    //Don't use characters that can be confused in a ticket: oO0, I1l
                    sChr = Convert.ToString((char)((int)Math.Round((objRandom.NextDouble() * 25.0) + 65.0)));
                    while (bad.IndexOf(sChr) > -1)
                    {
                        sChr = Convert.ToString((char)((int)Math.Round((objRandom.NextDouble() * 25.0) + 65.0)));
                    }
                    s = s + sChr;
                }
                else
                {
                    sChr = Convert.ToString((char)((int)Math.Round((objRandom.NextDouble() * 9.0) + 48.0)));
                    while (bad.IndexOf(sChr) > -1)
                    {
                        sChr = Convert.ToString((char)((int)Math.Round((objRandom.NextDouble() * 9.0) + 48.0)));
                    }
                    s = s + sChr;
                }
            }
            return s;
        }

        //#############################
        //WARNING! Example code only! 
        //Your order entry API is responsible for returning order status events to update the portfolio!
        //#############################

        //Call this routine when your order entry API returns a fill, cancel, reject, etc. message
        //If the orderID is not found, it is created
        //Orderid, status, time, symbol, type, qty, entry, last, $ gain, % gain
        public void ExecuteOrder(string orderID, Orders.Status OrderStatus, string symbol,
                                 DateTime ExecTime, Orders.Side Side, int Quantity, double EntryPrice,
                                 Order.OrderType OrderType, Order.Expiration Expires, double Limit)
        {
            //Details for the order including order type, expiration and limit price (if not market)
            string details;
            if (OrderType != Order.OrderType.Market)
            {
                details = Order.OrderTypeToString(OrderType) + " (" + Convert.ToString(Limit) + ") " +
                          Order.ExpirationToString(Expires);
            }
            else
            {
                details = Order.OrderTypeToString(OrderType) + " " + Order.ExpirationToString(Expires);
            }
            if ((DateTime.Compare(ExecTime, DateTime.MinValue) != 0) |
                (DateTime.Compare(ExecTime, Convert.ToDateTime("12:00:00 AM")) == 0))
            {
                ExecTime = DateTime.Now; //Time must be valid
            }

            //Try to find the order to update it
            for (int n = 0; n <= m_PortfolioGrid.Rows.Count - 1; n++)
            {
                if (m_PortfolioGrid.Rows[n].Cells["orderID"].Value.ToString().ToLower() != orderID.ToLower()) continue;

                if (OrderStatus != Orders.Status.Unknown) m_PortfolioGrid.Rows[n].Cells["Status"].Value = Orders.StatusToString(OrderStatus);
                if (OrderType != Order.OrderType.Unknown) m_PortfolioGrid.Rows[n].Cells["Details"].Value = details;
                if (symbol != "") m_PortfolioGrid.Rows[n].Cells["symbol"].Value = symbol;
                if (Side != Orders.Side.Unknown) m_PortfolioGrid.Rows[n].Cells["Type"].Value = Orders.SideToString(Side);
                if (Quantity != 0) m_PortfolioGrid.Rows[n].Cells["Qty"].Value = Quantity;
                if (EntryPrice != 0.0) m_PortfolioGrid.Rows[n].Cells["Entry"].Value = EntryPrice;

                //Send an alert
                SendExecutionAlertToServer(orderID, OrderStatus, symbol, ExecTime, Side, Quantity, EntryPrice, OrderType,
                                           Expires, Limit);

                //Speak the order change 'TODO: optional
                string text = Orders.StatusToString(OrderStatus);
                //m_frmMain.Speak("an order has been updated to," + text + ",for,symbol,[" + symbol + "]");

                return;
            }

            //Couldn't find the order so add it
            try
            {
                m_PortfolioGrid.Rows.Add(new object[]
                                        {
                                          orderID, Orders.StatusToString(OrderStatus), details, ExecTime, symbol,
                                          Orders.SideToString(Side), Quantity, EntryPrice
                                        });

                //Send an alert
                SendExecutionAlertToServer(orderID, OrderStatus, symbol, ExecTime, Side, Quantity, EntryPrice, OrderType,
                                                Expires, Limit);

                ////Speak the order 'TODO: optional
                //switch (Side)
                //{
                //    case Orders.Side.LongSide:
                //        m_frmMain.Speak("a buy order has been submitted,[" + Convert.ToString(Quantity) + "],shares of,symbol,[" +
                //                        symbol + "]");
                //        break;
                //    case Orders.Side.ShortSide:
                //        m_frmMain.Speak("a sell order has been submitted,[" + Convert.ToString(Quantity) +
                //                        "]symbol,shares of,symbol,[" + symbol + "]");
                //        break;
                //}
            }
            catch (Exception)
            {
                //Form was closing when adding an order to the grid
            }

            UpdatePortolioScreen(); //Refresh the data grid view

            // Send an update to twitter, if requested
            //if (frmMain2.GInstance.TweetTrades)
            //{
            //    string trade = Side.ToString().Replace("Side", "") + " " + symbol + " @ " + EntryPrice; // TODO: change message as desired
            //    frmMain2.GInstance.SendTweet(trade);
            //}

        }

        //Sends an alert to the web service for the mobile app
        private void SendExecutionAlertToServer(string orderID, Orders.Status OrderStatus, string symbol, DateTime ExecTime,
                                                Orders.Side Side, int Quantity, double EntryPrice, Order.OrderType OrderType,
                                                Order.Expiration Expires, double Limit)
        {
            string key = "ALERT|" + Convert.ToString(ExecTime) + "|ORDER|" + symbol + "|" + orderID;
            string data = orderID + "|" + Orders.StatusToString(OrderStatus) + "|" + symbol + "|" +
                          Convert.ToString(ExecTime) + "|" + Orders.SideToString(Side) + "|" +
                          Convert.ToString(Quantity) + "|" + Convert.ToString(EntryPrice) + "|" +
                          Order.OrderTypeToString(OrderType) + "|" + Order.ExpirationToString(Expires) + "|" +
                          Convert.ToString(Limit);
            try
            {
                svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey, key, data);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region Misc
        //Updates the colors and styles of this form
        public void UpdateStyle(string Style)
        {
            switch (Style)
            {
                case "Office2007Blue":
                    m_PortfolioGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 233, 251);
                    m_PortfolioGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 249, 254);
                    m_PortfolioGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    m_PortfolioGrid.DefaultCellStyle.ForeColor = lblPortfolio.ForeColor;
                    break;
                case "Office2007Silver":
                    m_PortfolioGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(223, 223, 233);
                    m_PortfolioGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(233, 233, 243);
                    m_PortfolioGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    break;
                case "WindowsVista":
                    m_PortfolioGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
                    m_PortfolioGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(158, 158, 158);
                    m_PortfolioGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    m_PortfolioGrid.DefaultCellStyle.ForeColor = Color.Black;
                    break;
                default:
                    m_PortfolioGrid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ButtonFace;
                    m_PortfolioGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(107, 104, 97);
                    m_PortfolioGrid.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;
            }
        }
        #endregion
    }
}
