/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using M4.modulusfe.platform;
using TradeScriptLib;
using M4.DataServer.Interface;
using M4.M4v2.Chart;
using M4.M4v2.Scripts;

namespace M4.M4v2.Alerts
{
  public partial class ctlAlert : UserControl, IDataSubscriber
  {
    #region Initialization and Members
    private bool m_changed;

    private readonly frmMain2 m_frmMain;
    private readonly ctlData m_ctlData;
    private CtlPainelChart m_chart;

    private static readonly CultureInfo usCulture = new CultureInfo("en-US");

    private readonly Alert oBuyAlert;
    private readonly Alert oSellAlert;
    private readonly Alert oExitLongAlert;
    private readonly Alert oExitShortAlert;

    private readonly ScriptManager svc = new ScriptManager();

    private bool m_BuyAlertProcessed;
    private bool m_SellAlertProcessed;
    private bool m_ExitBuyAlertProcessed;
    private bool m_ExitSellAlertProcessed;

    public ctlAlert(frmMain2 oMain, ctlData oData)
    {
      InitializeComponent();

      m_frmMain = oMain;
      m_ctlData = oData;

      oBuyAlert = new Alert { License = "XRT93NQR79ABTW788XR48" };
      oSellAlert = new Alert { License = "XRT93NQR79ABTW788XR48" };
      oExitLongAlert = new Alert { License = "XRT93NQR79ABTW788XR48" };
      oExitShortAlert = new Alert { License = "XRT93NQR79ABTW788XR48" };

      txtAlertName.GotFocus += (sender,e) => Text_Focus((TextBoxBase)sender);
      txtBars.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtInterval.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtStopLimit.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtSymbol.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtSymbolOrder.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);

      oBuyAlert.Alert += oBuyAlert_Alert;
      oSellAlert.Alert += oSellAlert_Alert;
      oExitLongAlert.Alert += oExitLongAlert_Alert;
      oExitShortAlert.Alert += oExitShortAlert_Alert;
    }

    #endregion

    #region Controls
    private void ctlAlert_Load(object sender, EventArgs e)
    {
      txtAlertName.Focus();
      LoadPortfolios();
      LoadAlerts();
      cboPeriodicity.SelectedIndex = 0;

      //TODO: add/remove your exchanges here:
      cboExchanges.Items.Add("NASDAQ");
      cboExchanges.Items.Add("NYSE");
      cboExchanges.Items.Add("AMEX");
      cboExchanges.SelectedIndex = 0;

      m_changed = false;
    }

    //Enables/Disables controls for when the script is running
    private void EnableControls(bool Enable)
    {
      grpSaveLoad.Enabled = Enable;
      grpData.Enabled = Enable;
      tabScripts.Enabled = Enable;
      pnlTrade.Enabled = Enable;
      chkEnableOrder.Enabled = Enable;
    }

    //Turns the alerts on and off
    private void cmdEnable_Click(object sender, EventArgs e)
    {
      if (!VerifyForm()) return;
      if (cmdEnable.Text == "&Enable Alerts")
      {
        if (!StartAlerts())
        {
          EnableControls(true);
          EnableAlerts();
        }
      }
      else
      {
        cmdEnable.Text = "&Enable Alerts";
        EnableControls(true);
        StopAlerts();
      }
      m_changed = true;
    }

    private void EnableAlerts()
    {
      cmdEnable.Text = "&Enable Alerts";
      cmdEnable.Enabled = true;
    }

    private void DisableAlerts()
    {
      if (cmdEnable.Text != "&Disable Alerts") return;
      chkEnableOrder.Checked = false;
      cmdEnable.Text = "&Enable Alerts";
      StopAlerts();
    }

    //Display the TradeScript documentation
    private void cmdDocumentation_Click(object sender, EventArgs e)
    {
      m_frmMain.OpenURL("http://www.modulusfe.com/tradescript/TradeScript.pdf", "TradeScript Help");
    }

    private void ctlAlert_Resize(object sender, EventArgs e)
    {
      tabScripts.Height = ((Height - tabScripts.Top) - pnlTrade.Height) - 20;
      cmdEnable.Top = (tabScripts.Top + tabScripts.Height) + 10;
      cmdDocumentation.Top = (tabScripts.Top + tabScripts.Height) + 10;
      pnlTrade.Top = (tabScripts.Top + tabScripts.Height) + 20;
      grpAlerts.Width = (Width - grpAlerts.Left) - 8;
      grpAlerts.Height = Height - 0x10;
      chkEnableOrder.Top = (tabScripts.Top + tabScripts.Height) + 10;
      m_ListAlerts.Columns[1].Width = (grpAlerts.Width - m_ListAlerts.Columns[0].Width) - 8;
    }

    //Handles GotGocus for several text boxes
    private static void Text_Focus(TextBoxBase textBox)
    {
      textBox.SelectAll();
    }

    private void txtAlertName_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void txtSymbol_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void cboPeriodicity_SelectedIndexChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void txtInterval_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void txtBars_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void txtScript_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
      ResetAlertFlags();
    }

    private void ResetAlertFlags()
    {
      m_BuyAlertProcessed = false;
      m_SellAlertProcessed = false;
      m_ExitBuyAlertProcessed = false;
      m_ExitSellAlertProcessed = false;
    }

    private void chkEnableOrder_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
      pnlTrade.Enabled = chkEnableOrder.Checked;
      if (chkEnableOrder.Checked)
      {
        m_frmMain.Speak("automated trading enabled");
        m_frmMain.Speak("review automated license");
      }
      else
      {
        m_frmMain.Speak("automated trading disabled");
      }
    }

    private void cboExchanges_SelectedIndexChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void udQuantity_ValueChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoMarket_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoStopMarket_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoLimit_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoStopLimit_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void txtStopLimit_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoDay_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoGTC_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoDayHours_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void rdoGTCHours_CheckedChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    private void txtSymbolOrder_TextChanged(object sender, EventArgs e)
    {
      m_changed = true;
    }

    #endregion

    #region Save/Load
    //Loads the list of available portfolios
    private void LoadPortfolios()
    {

      //List all portfolios in the user's web service entry list
      string[] portfolios = null;
      try
      {
          object[] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
        if (_ != null)
        {
          portfolios = new string[_.Length];
          for (int i = 0; i < _.Length; i++)
            portfolios[i] = _[i].ToString();
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
        }
      }
  
    }

    //Loads the list of previously saved alerts
    private void LoadAlerts()
    {
      string[ ] alerts = null;
      try
      {
        object[ ] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
        if (_ != null)
        {
          alerts = new string[_.Length];
          for (int i = 0; i < _.Length; i++)
            alerts[i] = _[i].ToString();
        }
      }
      catch (Exception)
      {
        return;
      }
      if (alerts == null) return;

      //Add them to combobox
      cboAlerts.Items.Clear();
      for (int n = 0; n <= alerts.Length - 1; n++)
      {
        if (alerts[n].StartsWith("Trade Alert Settings: "))
        {
          cboAlerts.Items.Add(alerts[n].Replace("Trade Alert Settings: ", ""));
        }
      }
    }

    //Validates the form for saving
    private bool VerifyForm()
    {
      uint tmpVal;
      if (txtAlertName.Text == "")
      {
        txtAlertName.Focus();
        MessageBox.Show("Please enter a name for this alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if (txtSymbol.Text == "")
      {
        txtSymbol.Focus();
        MessageBox.Show("Please enter a symbol", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if (cboPeriodicity.Text == "")
      {
        cboPeriodicity.Focus();
        MessageBox.Show("Please select a periodicity", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if ((txtBars.Text == "") | !Utils.IsNumeric(txtInterval.Text))
      {
        MessageBox.Show("Please enter the number of bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if ((txtInterval.Text == "") | !Utils.IsNumeric(txtInterval.Text))
      {
        MessageBox.Show("Please enter the interval", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if (!uint.TryParse(txtBars.Text, out tmpVal))
      {
        txtBars.SelectAll();
        txtBars.Focus();
        MessageBox.Show("Please enter the number of bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if (!uint.TryParse(txtInterval.Text, out tmpVal))
      {
        txtInterval.SelectAll();
        txtInterval.Focus();
        MessageBox.Show("Please enter the interval", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      if (chkEnableOrder.Checked)
      {
        txtSymbolOrder.Text = txtSymbolOrder.Text.Trim();
        if (txtSymbolOrder.TextLength == 0)
        {
          txtSymbolOrder.Focus();
          MessageBox.Show("Please enter a symbol", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        else
        {
          return true;
        }
      }
      return true;
    }

    private void cmdSave_Click(object sender, EventArgs e)
    {
      SaveAlert(false);
    }

    public void SaveAlert()
    {
      SaveAlert(false);
    }
    public void SaveAlert(bool Prompt)
    {
      string alertName = txtAlertName.Text;
      if (Prompt)
      {
        if (alertName == "")
        {
          alertName = "Untitled";
        }
        if (
          MessageBox.Show("Save alert '" + alertName + "'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
          return;
        }
      }

      if (!VerifyForm()) return;

      cmdSave.Enabled = false;
      cmdDelete.Enabled = false;
      cboAlerts.Enabled = false;

      string d = Utils.Chr(134).ToString();
      
      string data = txtSymbol.Text + d + cboPeriodicity.Text + d + txtInterval.Text + d +
                    txtBars.Text +
                    d + txtBuyScript.Text + d + txtSellScript.Text + d + txtExitLongScript.Text + d +
                    txtExitShortScript.Text + d + chkEnableOrder.Checked + d +
                    cmbPortfolio.Text +
                    d + txtSymbolOrder.Text + d + cboExchanges.Text + d + udQuantity.Text + d +
                    rdoMarket.Checked + d + rdoStopMarket.Checked + d +
                    rdoLimit.Checked + d + rdoStopLimit.Checked + d +
                    txtStopLimit.Text + d + rdoDay.Checked + d + rdoGTC.Checked +
                    d + rdoDayHours.Checked + d + rdoGTCHours.Checked + d + cmdEnable.Text;
      try
      {
        svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                        "Trade Alert Settings: " + alertName, data);
        m_changed = false;
      }
      catch (Exception)
      {
        MessageBox.Show("Failed to save alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      bool found = false;
      for (short n = 0; n <= cboAlerts.Items.Count - 1; n++)
      {
        if (cboAlerts.Items[n] == txtAlertName.Text)
        {
          found = true;
          break;
        }
      }
      if (!found)
      {
        LoadAlerts();
        for (short n = 0; n <= cboAlerts.Items.Count - 1; n++)
        {
          if (cboAlerts.Items[n] == alertName)
          {
            cboAlerts.SelectedIndex = n;
            break;
          }
        }
      }
      UpdateName(txtAlertName.Text);
      cmdSave.Enabled = true;
      if (cboAlerts.SelectedIndex > -1)
      {
        cmdDelete.Enabled = true;
      }
      cboAlerts.Enabled = true;
    }

    //Updates the Nevron tab text
    private void UpdateName(string name)
    {/*
      foreach (NUIDocument doc in m_frmMain.m_DockManager.DocumentManager.Documents)
      {
        if (doc.Client.Name == "ctlAlert")
        {
          ctlAlert alert = (ctlAlert)doc.Client;
          if (alert.Handle == Handle)
          {
            doc.Text = "Alert: " + name;
          }
        }
      }*/
    }

    //Load the selected alert
    private void cboAlerts_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (m_changed && cboAlerts.Enabled)
      {
        SaveAlert(true);
      }
      LoadAlert();
    }

    //Loads an alert
    private void LoadAlert()
    {
      string alertName = cboAlerts.Text;
      if (alertName == "") return;
      cmdSave.Enabled = false;
      cmdDelete.Enabled = false;
      cboAlerts.Enabled = false;
      string data;
      try
      {
        data = svc.GetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                               "Trade Alert Settings: " + alertName);
      }
      catch (Exception)
      {
        MessageBox.Show("Failed to load alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        cmdSave.Enabled = true;
        cmdDelete.Enabled = true;
        cboAlerts.Enabled = true;
        return;
      }
      string[ ] text = data.Split(new[ ] { Utils.Chr(134) });
      if (text.Length < 23) return;

      txtSymbol.Text = text[0];
      for (short n = 0; n <= cboPeriodicity.Items.Count - 1; n = (short)(n + 1))
      {
        if (cboPeriodicity.Items[n] == text[1])
        {
          cboPeriodicity.SelectedIndex = n;
          break;
        }
      }
      txtInterval.Text = text[2];
      txtBars.Text = text[3];
      txtBuyScript.Text = text[4];
      txtSellScript.Text = text[5];
      txtExitLongScript.Text = text[6];
      txtExitShortScript.Text = text[7];
      chkEnableOrder.Checked = Convert.ToBoolean(text[8]);
      for (short n = 0; n <= cmbPortfolio.Items.Count - 1; n = (short)(n + 1))
      {
        if (cmbPortfolio.Items[n] == text[9])
        {
          cmbPortfolio.SelectedIndex = n;
          break;
        }
      }
      txtSymbolOrder.Text = text[10];
      for (short n = 0; n <= cboExchanges.Items.Count - 1; n = (short)(n + 1))
      {
        if (cboExchanges.Items[n] == text[11])
        {
          cboExchanges.SelectedIndex = n;
          break;
        }
      }
      udQuantity.Text = text[12];
      rdoMarket.Checked = Convert.ToBoolean(text[13]);
      rdoStopMarket.Checked = Convert.ToBoolean(text[14]);
      rdoLimit.Checked = Convert.ToBoolean(text[15]);
      rdoStopLimit.Checked = Convert.ToBoolean(text[0x10]);
      txtStopLimit.Text = text[0x11];
      rdoDay.Checked = Convert.ToBoolean(text[0x12]);
      rdoGTC.Checked = Convert.ToBoolean(text[0x13]);
      rdoDayHours.Checked = Convert.ToBoolean(text[20]);
      rdoGTCHours.Checked = Convert.ToBoolean(text[0x15]);
      txtAlertName.Text = cboAlerts.Text;
      cmdEnable.Text = "&Enable Alerts";
      EnableControls(true);
      StopAlerts();
      m_changed = false;
      UpdateName(txtAlertName.Text);
      cmdSave.Enabled = true;
      cmdDelete.Enabled = true;
      cboAlerts.Enabled = true;
      if (cboAlerts.SelectedIndex == -1)
      {
        cmdDelete.Enabled = false;
      }
    }

    private void cmdDelete_Click(object sender, EventArgs e)
    {
      if ((cboAlerts.Text != "") &&
          (MessageBox.Show("Delete alert " + cboAlerts.Text + "?", "Confirm", MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) == DialogResult.OK))
      {
        string alertName = cboAlerts.Text;
        try
        {
          svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                               "Trade Alert Settings: " + alertName, "");
          LoadAlerts();
          cboAlerts.SelectedIndex = cboAlerts.Items.Count - 1;
        }
        catch (Exception)
        {
          MessageBox.Show("Failed to delete alert", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    #endregion

    #region Alert Processing

    private static DateTime lastSpokenBuyAlert;
    private void oBuyAlert_Alert(string symbol, string name)
    {
      //Can this alert be processed?
      //TODO: add a static variable and text box if you want the script to fire n-times

      if (m_BuyAlertProcessed) return;

      m_SellAlertProcessed = false;
      m_ExitBuyAlertProcessed = false;
      m_BuyAlertProcessed = true;

      if (File.Exists(Application.StartupPath + @"\Res\ScriptAlert.wav"))
      {
        try
        {
          BroadcastAlert("Buy Alert", "LONG");
          if (chkEnableOrder.Checked)
          {
            SubmitOrder(ctlPortfolio.Orders.Side.LongSide);
          }
          else if ((DateTime.Now - lastSpokenBuyAlert).TotalSeconds > 15L) //limit
          {
            m_frmMain.Speak("trade alert,[" + m_chart.StockChartX1.Symbol + "]");
            lastSpokenBuyAlert = DateTime.Now;
          }
        }
        catch (Exception)
        {
        }
      }
    }

    private static DateTime oSellAlertAlertlastSpoken;
    private void oSellAlert_Alert(string symbol, string name)
    {
      //Can this alert be processed?
      //TODO: add a static variable and text box if you want the script to fire n-times
      if (m_SellAlertProcessed) return; //Can't process until another script evaluates to true

      m_BuyAlertProcessed = false;
      m_ExitSellAlertProcessed = false;
      m_SellAlertProcessed = true;

      //Play the alert sound
      if (File.Exists(Application.StartupPath + @"\Res\ScriptAlert.wav"))
      {
        try
        {
          BroadcastAlert("Sell Alert", "SHORT");
          if (chkEnableOrder.Checked)
          {
            SubmitOrder(ctlPortfolio.Orders.Side.ShortSide);
          }
          else if ((DateTime.Now - oSellAlertAlertlastSpoken).TotalSeconds > 15) //limit
          {
            m_frmMain.Speak("trade alert,[" + symbol + "]");
            oSellAlertAlertlastSpoken = DateTime.Now;
          }
        }
        catch (Exception)
        {
        }
      }
    }

    private void oExitLongAlert_Alert(string symbol, string name)
    {
      //Can this alert be processed?
      //TODO: add a static variable and text box if you want the script to fire n-times
      if (m_ExitBuyAlertProcessed) return; //Can't process until another script evaluates to true

      m_SellAlertProcessed = false;
      m_BuyAlertProcessed = false;
      m_ExitBuyAlertProcessed = true;

      //Play the alert sound
      if (File.Exists(Application.StartupPath + @"\Res\ScriptAlert.wav"))
      {
        try
        {
          BroadcastAlert("Exit-Long Alert", "EXIT");
          if (chkEnableOrder.Checked)
          {
            //TODO: impliment long exits based on your preference or leave as-is
            SubmitOrder(ctlPortfolio.Orders.Side.ShortSide);
          }
          else
          {
            m_frmMain.Speak("trade alert,[" + symbol + "]");
          }
        }
        catch (Exception)
        {
        }
      }
    }

    private void oExitShortAlert_Alert(string symbol, string name)
    {
      //Can this alert be processed?
      if (m_ExitSellAlertProcessed) return; //Can't process until another script evaluates to true

      m_SellAlertProcessed = false;
      m_BuyAlertProcessed = false;
      m_ExitSellAlertProcessed = true;

      //Play the alert sound
      if (File.Exists(Application.StartupPath + @"\Res\ScriptAlert.wav"))
      {
        try
        {
          BroadcastAlert("Exit-Short Alert", "EXIT");
          if (chkEnableOrder.Checked)
          {
            //TODO: impliment short exits based on your preference or leave as-is
            SubmitOrder(ctlPortfolio.Orders.Side.LongSide);
          }
          else
          {
            m_frmMain.Speak("trade alert,[" + symbol + "]");
          }
        }
        catch (Exception)
        {
        }
      }
    }

    //Stops  alerts
    public void StopAlerts()
    {
      m_ctlData.RemoveSymbolWatch(txtSymbol.Text, this);
      oBuyAlert.ClearRecords();
      oSellAlert.ClearRecords();
      oExitLongAlert.ClearRecords();
      oExitShortAlert.ClearRecords();
      oBuyAlert.AlertScript = "";
      oSellAlert.AlertScript = "";
      oExitLongAlert.AlertScript = "";
      oExitShortAlert.AlertScript = "";
      tmrUpdate.Enabled = false;
    }

    //Starts alerts
    public bool StartAlerts()
    {
      Periodicity periodicity;
      M4Core.Entities.ChartSelection selection = new M4Core.Entities.ChartSelection();
      if (!VerifyForm()) return false;
      if (!TestScripts()) return false;
      if (m_ctlData == null) return false;

      cmdEnable.Text = "&Disable Alerts";
      EnableControls(false);

      switch (cboPeriodicity.Text)
      {
        case "Minute":
          periodicity = Periodicity.Minutely;
          break;
        case "Hour":
          periodicity = Periodicity.Hourly;
          break;
        case "Day":
          periodicity = Periodicity.Daily;
          break;
        case "Week":
          periodicity = Periodicity.Weekly;
          break;
        default:
          periodicity = Periodicity.Minutely;
          break;
      }
      cmdEnable.Enabled = false;
      selection.Periodicity = (M4Core.Entities.Periodicity)periodicity;
      selection.Symbol = txtSymbol.Text;
      try
      {
        selection.Interval = (int)Math.Round(Convert.ToDouble(txtInterval.Text));
      }
      catch { }
      try{
        selection.Bars = (int)Math.Round(Convert.ToDouble(txtBars.Text));
      }
      catch { }

      m_ctlData.LoadRealTimeCtlPainelChartAsync2(selection, new Action<CtlPainelChart>(chart => { m_chart = chart; }));
      if (m_chart == null) return false; //Failed to get the chart

      DataManager.BarData[ ] bars = m_chart.GetDataFromChart();
      if (bars.Length < 1) return false;

      m_chart.Subscribers++; //Increment (deincremented in frmMain UI unload)

      cmdEnable.Enabled = true;

      //Get historic data and subscribe to realtime updates
      if (bars.Length < 3) return false; //Bad request 

      //Insert the data into all four instances of TradeScript
      oBuyAlert.AlertScript = "";
      oSellAlert.AlertScript = "";
      oExitLongAlert.AlertScript = "";
      oExitShortAlert.AlertScript = "";
      oBuyAlert.ClearRecords();
      oSellAlert.ClearRecords();
      oExitLongAlert.ClearRecords();
      oExitShortAlert.ClearRecords();
      for (int n = 0; n <= bars.Length - 1; n++)
      {
        DateTime td = bars[n].TradeDate;
        double jdate = oBuyAlert.ToJulianDate(td.Year, td.Month, td.Day, td.Hour, td.Minute, td.Second, 0);
        oBuyAlert.AppendHistoryRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice, bars[n].LowPrice, bars[n].ClosePrice,
                                    (int)Math.Round(bars[n].Volume));
        oSellAlert.AppendHistoryRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice, bars[n].LowPrice, bars[n].ClosePrice,
                                     (int)Math.Round(bars[n].Volume));
        oExitLongAlert.AppendHistoryRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice, bars[n].LowPrice,
                                         bars[n].ClosePrice, (int)Math.Round(bars[n].Volume));
        oExitShortAlert.AppendHistoryRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice, bars[n].LowPrice,
                                          bars[n].ClosePrice, (int)Math.Round(bars[n].Volume));
      }

      //Now that the history has been added, turn the scripts on
      oBuyAlert.AlertName = "Buy Script";
      oBuyAlert.AlertScript = txtBuyScript.Text;
      oSellAlert.AlertName = "Sell Script";
      oSellAlert.AlertScript = txtSellScript.Text;
      oExitLongAlert.AlertName = "Exit Long Script";
      oExitLongAlert.AlertScript = txtExitLongScript.Text;
      oExitShortAlert.AlertName = "Exit Short Script";
      oExitShortAlert.AlertScript = txtExitShortScript.Text;

      //Start the data poll timer
      tmrUpdate.Enabled = true;

      return true;
    }

    //Polls the chart for data every 500ms
    //Note that we could request bars and updates in a push directly from DDFDataManager, however
    //since the bars start at a different time, they would not match with the chart that the user
    //us seeing on the screen.

    private static int tmrUpdate_TicklastRecCnt;
    private void tmrUpdate_Tick(object sender, EventArgs e)
    {
      long recCnt = m_chart.StockChartX1.RecordCount;
      string Symbol = m_chart.StockChartX1.Symbol;

      //Get the last bar values
      double j = m_chart.StockChartX1.GetJDate(Symbol + ".Close", (int)recCnt);
      double o = m_chart.StockChartX1.GetValue(Symbol + ".Open", (int)recCnt);
      double h = m_chart.StockChartX1.GetValue(Symbol + ".High", (int)recCnt);
      double l = m_chart.StockChartX1.GetValue(Symbol + ".Low", (int)recCnt);
      double c = m_chart.StockChartX1.GetValue(Symbol + ".Close", (int)recCnt);
      long v = (long)Math.Round(m_chart.StockChartX1.GetValue(Symbol + ".Volume", (int)recCnt));


      if (recCnt != tmrUpdate_TicklastRecCnt) //New Record
      {
        oBuyAlert.AppendRecord(j, o, h, l, c, (int)v);
        oSellAlert.AppendRecord(j, o, h, l, c, (int)v);
        oExitLongAlert.AppendRecord(j, o, h, l, c, (int)v);
        oExitShortAlert.AppendRecord(j, o, h, l, c, (int)v);
      }
      else
      { //Edit last record
        j = oBuyAlert.GetJDate(oBuyAlert.RecordCount);
        oBuyAlert.EditRecord(j, o, h, l, c, (int)v);
        oSellAlert.EditRecord(j, o, h, l, c, (int)v);
        oExitLongAlert.EditRecord(j, o, h, l, c, (int)v);
        oExitShortAlert.EditRecord(j, o, h, l, c, (int)v);
      }

      tmrUpdate_TicklastRecCnt = (int)recCnt;
    }

    public void Disconnect()
    {
      StopAlerts();
      if (m_chart == null) return;
      m_chart.Subscribers--;
    }

    private bool TestScripts()
    {
      Validate script = new ValidateClass { License = "XRT93NQR79ABTW788XR48" };

      string err = script.Validate(txtBuyScript.Text);
      if (!string.IsNullOrEmpty(err))
      {
        tabScripts.SelectedIndex = 0;
        if (script.ScriptHelp != "")
        {
          if (MessageBox.Show("Your buy script generated an error:\r\n" + err.Replace("Error: ", "") +
                              "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                              MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            MessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
        else
        {
          MessageBox.Show("Your buy script generated an error:\r\n" + err.Replace("Error: ", ""), "Error:",
                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      err = script.Validate(txtSellScript.Text);
      if (!string.IsNullOrEmpty(err))
      {
        tabScripts.SelectedIndex = 1;
        if (script.ScriptHelp != "")
        {
          if (MessageBox.Show("Your sell script generated an error:\r\n" + err.Replace("Error: ", "") +
                              "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                              MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            MessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
        else
        {
          MessageBox.Show("Your sell script generated an error:\r\n" + err.Replace("Error: ", ""), "Error:",
                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      err = script.Validate(txtExitLongScript.Text);
      if (!string.IsNullOrEmpty(err))
      {
        tabScripts.SelectedIndex = 2;
        if (script.ScriptHelp != "")
        {
          if (MessageBox.Show("Your exit-long script generated an error:\r\n" + err.Replace("Error: ", "") +
                              "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                              MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            MessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
        else
        {
          MessageBox.Show("Your exit-long script generated an error:\r\n" + err.Replace("Error: ", ""), "Error:",
                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      err = script.Validate(txtExitShortScript.Text);
      if (string.IsNullOrEmpty(err))
      {
        return true;
      }
      tabScripts.SelectedIndex = 3;
      if (script.ScriptHelp != "")
      {
        if (MessageBox.Show("Your exit-short script generated an error:\r\n" + err.Replace("Error: ", "") +
                            "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          MessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      else
      {
        MessageBox.Show("Your exit-short script generated an error:\r\n" + err.Replace("Error: ", ""), "Error:",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return false;
    }

    #endregion

    #region Broadcast Alerts
    //'Uploads the alert to the web service so that it can be 
    //viewed by another application (desktop or mobile)
    //Note that alerts must be limited in a FIFO manner so that
    //the server is not overloaded with alerts.
    //NOTE: It is a violation of your license agreement to change
    //the number from 100 unless you are licensed to run the web
    //service on your own server!
    private static DateTime BroadcastAlertlastTime = DateTime.Parse("1/1/1900", usCulture.DateTimeFormat);
    private void BroadcastAlert(string Description, string Side)
    {
      //Don't send too many alets at once:
      if ((DateTime.Now - BroadcastAlertlastTime).TotalSeconds < 5) return;

      BroadcastAlertlastTime = DateTime.Now;

      //Count the number of alerts already sent to the server in the past 24 hours
      DataSet ds;
      try
      {
        ds = svc.GetAlerts(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
        if (ds == null)
        {
          return;
        }
      }
      catch
      {
        return;
      }
      int count = 0;
      DateTime earliest = DateTime.Now;
      string firstKey = "";
      for (int n = 0; n < ds.Tables["UserData"].Rows.Count; n++)
      {
        string[ ] items = Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]).Split('|');
        if ((items.Length > 0) && (items[2] == "SCRIPT"))
        {
          DateTime d;
          count++;
          try
          {
            d = DateTime.Parse(items[1], usCulture.DateTimeFormat);
          }
          catch (Exception)
          {
            d = Convert.ToDateTime(items[1]);
          }
          if (DateTime.Compare(d, earliest) < 0)
          {
            earliest = d;
            firstKey = Convert.ToString(ds.Tables["UserData"].Rows[n]["DataKey"]);
          }
        }
      }


      if (count > 99)
      {
        //NOTE: It is a violation of your license agreement to change
        //the number of alerts unless you are licensed to run the web
        //service on your own server!
        //We must remove the earliest alert (FIFO):
        svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey, firstKey, "");
      }

      //Send the alert to the web service. Format:
      //ALERT|DATE|TYPE|SYMBOL|TAG (ALERT|DATE|SCRIPT|SYMBOL|SIDE)
      string key = "ALERT|" + Convert.ToString(DateTime.Now) + "|SCRIPT|" + txtSymbol.Text + "|" + Side;
      string data = Convert.ToString(DateTime.Now) + "|" + txtAlertName.Text + ": " + txtSymbol.Text + ": " +
                    Description;
      try
      {
        svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey, key, data);
      }
      catch (Exception)
      {
        return;
      }

      //Add the aler to the list
      Color clr = Color.WhiteSmoke;
      ListViewItem item = new ListViewItem { Text = Convert.ToString(DateTime.Now) };
      item.SubItems.Add(Description);
      item.ForeColor = m_ctlData.ForeColor;
      item.BackColor = (m_ListAlerts.Items.Count % 2) > 0 ? clr : Color.White;
      m_ListAlerts.Items.Add(item);

      Application.DoEvents();
    }

    #endregion

    #region IDataSubscriber
    public IntPtr GetHandle()
    {
      return IntPtr.Zero;
    }

    public void BarUpdate(string Symbol, Periodicity BarType, int BarSize, BarData Bar, bool IsNewBar)
    {
    }

    public void PriceUpdate(string Symbol, DateTime TradeDate, double LastPrice, long Volume)
    {
    }
    #endregion

    #region Order Execution

    private static int SubmitOrderlastRecCnt;
    private void SubmitOrder(ctlPortfolio.Orders.Side BuySell)
    {
      if (m_chart == null) return;

      // Saftey measure allows only one trade per bar (TODO: consider removal if so desired)
      if (m_chart.StockChartX1.RecordCount == SubmitOrderlastRecCnt) return;

      SubmitOrderlastRecCnt = m_chart.StockChartX1.RecordCount;
      ctlPortfolio.Order MyOrder = new ctlPortfolio.Order();
      ctlPortfolio portfolio = new ctlPortfolio(m_frmMain, m_ctlData);//m_frmMain.GetPortfolio();

      if (portfolio == null) return;

      //Gather the order details    
      MyOrder.OrderID = portfolio.CreateOrderID();
      MyOrder.Side = BuySell;
      MyOrder.Quantity = Convert.ToInt32(udQuantity.Text);
      MyOrder.Exchange = cboExchanges.Text;
      MyOrder.Symbol = txtSymbol.Text;

      try
      {
        MyOrder.LimitPrice = Convert.ToDouble(txtStopLimit.Text);
      }
      catch { }

      if (rdoLimit.Checked)
      {
        MyOrder._Order = ctlPortfolio.Order.OrderType.Limit;
      }
      else if (rdoMarket.Checked)
      {
        MyOrder._Order = ctlPortfolio.Order.OrderType.Market;
      }
      else if (rdoStopLimit.Checked)
      {
        MyOrder._Order = ctlPortfolio.Order.OrderType.StopLimit;
      }
      else if (rdoStopMarket.Checked)
      {
        MyOrder._Order = ctlPortfolio.Order.OrderType.StopMarket;
      }
      if (rdoGTC.Checked)
      {
        MyOrder.Expires = ctlPortfolio.Order.Expiration.GoodTillCanceled;
      }
      else if (rdoGTCHours.Checked)
      {
        MyOrder.Expires = ctlPortfolio.Order.Expiration.GoodTillCanceledPlusAfterHours;
      }
      else if (rdoDay.Checked)
      {
        MyOrder.Expires = ctlPortfolio.Order.Expiration.DayOrder;
      }
      else if (rdoDayHours.Checked)
      {
        MyOrder.Expires = ctlPortfolio.Order.Expiration.DayOrderPlusAfterHours;
      }

      //Ensure the portfolio is selected
      if (portfolio.cmbPortfolio.Text != cmbPortfolio.Text)
      {
        for (short n = 0; n < portfolio.cmbPortfolio.Items.Count; n++)
        {
          if (portfolio.cmbPortfolio.Items[n] == cmbPortfolio.Text)
          {
            portfolio.cmbPortfolio.SelectedIndex = n;
            break;
          }
        }
      }

      //#### TODO: WARNING! Example code only! Your order entry API is responsible
      //for sending/receiving orders to update this control. This example just
      //sends the order straight to the DataViewGrid control! Also the exec time 
      //and status should be set by the server.
      MyOrder.ExecTime = DateTime.Now;
      MyOrder.Status = ctlPortfolio.Orders.Status.Sending;
      portfolio.ExecuteOrder(MyOrder.OrderID, MyOrder.Status, MyOrder.Symbol, MyOrder.ExecTime, MyOrder.Side,
                             MyOrder.Quantity, portfolio.GetLastPrice(MyOrder.Symbol), MyOrder._Order,
                             MyOrder.Expires, MyOrder.LimitPrice);
    }
    #endregion
  }
}
