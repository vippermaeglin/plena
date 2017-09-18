/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using M4.Properties;
using M4Core.Entities;
using TradeScriptLib;
using M4.DataServer.Interface;
using M4.M4v2.Chart;
using M4.M4v2.Scripts;

//M4 Trading Platform - copyright Modulus Financial Engineering, Inc. - all rights reserved. 
//http://www.modulusfe.com 

//NOTICE: TradeScript(tm) is provided under a separate license. Email sales@modulusfe.com for details. 

namespace M4.M4v2.Scanner
{
  public partial class ctlScanner : UserControl, IDataSubscriber
  {
    private readonly Alert oScript;

    private DateTime m_TimeStamp;
    public bool m_scanning;
    public bool m_changed;
    private bool m_stop;
    private bool m_loading;
    private bool m_dialogShown;

    private readonly frmMain2 m_frmMain;
    private readonly ctlData m_ctlData;

    private readonly ScriptManager svc = new ScriptManager();

    private List<DataManager.BarData> DataBars;

    public List<string> Symbols { get; set; }

    public int Periodicity { get; set; }

    public int Interval { get; set; }

    public int Bars { get; set; }

    public string Script { get; set; }

    public string TempScript { get; set; }

    public ctlScanner(frmMain2 oMain, ctlData oData)
    {
      InitializeComponent();
      m_frmMain = oMain;
      m_ctlData = oData;

      oScript = new Alert { License = "XRT93NQR79ABTW788XR48" };
      Symbols = new List<string>();
      DataBars = new List<DataManager.BarData>();

      //Setup the results grid 
      grdResults.RowTemplate.Height = 28;

      grdResults.ShowCellToolTips = false;
      grdResults.GridColor = Color.FromArgb(50, 50, 50);
      grdResults.RowsDefaultCellStyle.SelectionBackColor = Color.Black;
      grdResults.RowsDefaultCellStyle.SelectionForeColor = Color.Yellow;

      grdResults.BackgroundColor = Color.Black;
      grdResults.ForeColor = Color.White;
      grdResults.DefaultCellStyle.BackColor = Color.Black;
      grdResults.DefaultCellStyle.ForeColor = Color.White;

      grdResults.RowTemplate.Height = 30;

      DataGridViewTextBoxColumn tradeTime = new DataGridViewTextBoxColumn();
      {
        tradeTime.HeaderText = "Trade Time";
        tradeTime.Name = "Trade Time";
        tradeTime.ReadOnly = true;
        grdResults.Columns.Add(tradeTime);
      }

      DataGridViewTextBoxColumn symbolCol = new DataGridViewTextBoxColumn();
      {
        symbolCol.HeaderText = "Symbol";
        symbolCol.Name = "Symbol";
        symbolCol.ReadOnly = true;
      }
      grdResults.Columns.Add(symbolCol);

      DataGridViewTextBoxColumn lastCol = new DataGridViewTextBoxColumn();
      {
        lastCol.HeaderText = "Last";
        lastCol.Name = "Last";
        lastCol.ReadOnly = true;
        lastCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      }
      grdResults.Columns.Add(lastCol);

      DataGridViewBarGraphColumn volumeCol = new DataGridViewBarGraphColumn();
      {
        volumeCol.HeaderText = "Volume";
        volumeCol.Name = "Volume";
        volumeCol.ReadOnly = true;
        volumeCol.SortMode = DataGridViewColumnSortMode.Automatic;
        volumeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      }
      grdResults.Columns.Add(volumeCol);

      DataGridViewTextBoxScannerColorColumn alertTime = new DataGridViewTextBoxScannerColorColumn();
      {
        alertTime.HeaderText = "Alert Time";
        alertTime.Name = "Alert Time";
        alertTime.ReadOnly = true;
        grdResults.Columns.Add(alertTime);
      }

      DataGridViewButtonColumn tradeCol = new DataGridViewButtonColumn();
      {
        tradeCol.HeaderText = "Trade";
        tradeCol.Name = "Trade";
        tradeCol.Text = "Trade";
        tradeCol.ToolTipText = "Enter Order";
        tradeCol.DefaultCellStyle.NullValue = "Trade";
        tradeCol.UseColumnTextForButtonValue = true;
      }
      grdResults.Columns.Add(tradeCol);

      DataGridViewButtonColumn chartCol = new DataGridViewButtonColumn();
      {
        chartCol.HeaderText = "Chart";
        chartCol.Name = "Chart";
        chartCol.Text = "Chart";
        chartCol.UseColumnTextForButtonValue = true;
      }
      grdResults.Columns.Add(chartCol);

      DataGridViewButtonColumn settingsCol = new DataGridViewButtonColumn();
      {
        settingsCol.HeaderText = "Settings";
        settingsCol.Name = "Settings";
        settingsCol.Text = "Settings";
        settingsCol.ToolTipText = "Edit Settings";
        settingsCol.DefaultCellStyle.NullValue = "Settings";
        settingsCol.UseColumnTextForButtonValue = true;
        grdResults.Columns.Add(settingsCol);
      }

      DataGridViewImageButtonColumn lockCol = new DataGridViewImageButtonColumn();
      {
        lockCol.HeaderText = "Lock Script";
        lockCol.Name = "Locked";
        grdResults.Columns.Add(lockCol);
      }

      DataGridViewImageButtonColumn startCol = new DataGridViewImageButtonColumn();
      {
        startCol.HeaderText = "Pause";
        startCol.Name = "Start";
        startCol.ToolTipText = "Start Scan";
        grdResults.Columns.Add(startCol);
      }

      UpdateStyle(m_frmMain.m_Style);
    }

    //Updates the colors and styles of this form 
    public void UpdateStyle(string Style)
    {

      // Optional: If you want to change the colors of the grid, modify this code

      if (Style == "Office2007Blue")
      {
        grdResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 233, 251);
        grdResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 249, 254);
        grdResults.DefaultCellStyle.SelectionForeColor = Color.Black;
        grdResults.DefaultCellStyle.ForeColor = Label1.ForeColor;
      }
      else if (Style == "Office2007Silver")
      {
        grdResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(223, 223, 233);
        grdResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(233, 233, 243);
        grdResults.DefaultCellStyle.SelectionForeColor = Color.Black;
      }
      else if (Style == "WindowsVista")
      {
        grdResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
        grdResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(158, 158, 158);
        grdResults.DefaultCellStyle.SelectionForeColor = Color.Black;
        grdResults.DefaultCellStyle.ForeColor = Color.Black;
      }
      else
      {
        grdResults.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ButtonFace;
        grdResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(107, 104, 97);
        grdResults.DefaultCellStyle.SelectionForeColor = Color.White;
      }

    }

    /// <summary>
    /// Contains symbols and historic data in TS Alert objects
    /// </summary>
    private readonly Dictionary<string, Alert> m_TSAlertDictionary = 
      new Dictionary<string, Alert>(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Contains references to DataGridView rows
    /// </summary>
    private readonly Dictionary<string, DataGridViewRow> m_DGVRowDictionary = 
      new Dictionary<string, DataGridViewRow>(StringComparer.InvariantCultureIgnoreCase);

    //###############
    // International Patents Pending. Use of this scanning code outside of the M4 application is a violation of the license agreement.
    //###############

    //Returns a TradeScript Alert object for a specific symbol
    public Alert GetTSAlertObject(string symbol)
    {
      return m_TSAlertDictionary.ContainsKey(symbol) ? m_TSAlertDictionary[symbol] : null;
    }

    //Loads all required data into memory. This function must be called first.
    public bool LoadAllSymbolsIntoMemory()
    {
      //Verify the input and check script for errors
      if (m_ctlData == null || !TestScripts())
      {
        return false;
      }

      EnableControls(false);

      //Clear everything
      m_stop = false;
      m_TSAlertDictionary.Clear();
      m_DGVRowDictionary.Clear();
      grdResults.Rows.Clear();

      DataGridViewBarGraphColumn volumeCol = (DataGridViewBarGraphColumn)grdResults.Columns["Volume"];
      if (volumeCol != null)
      {
        volumeCol.MaxValue = 0;
      }

      //Load history for all symbols into memory
      pnlProgress.Cursor = Cursors.WaitCursor;
      pnlProgress.Visible = true;

      if (Symbols.Count > 1)
      {
        ProgressBar1.Maximum = Symbols.Count - 1;
      }

      ChartSelection selection = new ChartSelection
                                   {
                                     Periodicity = (M4Core.Entities.Periodicity)Periodicity,
                                     Interval = Interval,
                                     Bars = Bars * 3
                                   };
      if (selection.Bars < 50)
      {
        selection.Bars = 50;
      }

      lblInfo.Text = "Priming scanner, please wait...";

      for (int n = 0; n < Symbols.Count; n++)
      {
        string symbol = Symbols[n];

        if (m_stop)
        {
          break;
        }

        if (m_TSAlertDictionary.ContainsKey(symbol))
        {
          continue;
        }

        selection.Symbol = symbol;

        ProgressBar1.Value = n;
        lblSymbol.Text = symbol;
        Application.DoEvents();

        //Get historic data
        var bars = m_ctlData.GetHistory(symbol, this, selection.Periodicity, selection.Interval, selection.Bars, "Plena", answer => { });
        if (bars == null || bars.Count < 3)
        {
          continue;
        }

        //Insert the data into TradeScript
        Alert oAlert = new Alert { License = "XRT93NQR79ABTW788XR48" };

        foreach (M4.DataServer.Interface.BarData t in bars)
        {
          double jDate = oAlert.ToJulianDate(t.TradeDate.Year, t.TradeDate.Month, t.TradeDate.Day,
                                             t.TradeDate.Hour, t.TradeDate.Minute, t.TradeDate.Second,
                                             t.TradeDate.Millisecond);
          oAlert.AppendHistoryRecord(jDate, t.OpenPrice, t.HighPrice, t.LowPrice, t.ClosePrice, (int)t.VolumeF);
        }

        //Start the alert object by adding the script            
        oAlert.AlertScript = Script;
        oAlert.Symbol = symbol;

        oAlert.ScriptError += oScript_ScriptError;
        oAlert.Alert += OnAlert;

        m_TSAlertDictionary.Add(symbol, oAlert);

        //Add symbol to datagridview
        int row;
        try
        {
          row = grdResults.Rows.Add(new DataGridViewRow());
        }
        catch (Exception)
        {
          return false;
        }
        m_DGVRowDictionary.Add(symbol, grdResults.Rows[row]);
        grdResults.Rows[row].Height = 25;

        M4.DataServer.Interface.BarData lastBar = bars.Last();

        DataGridViewTextBoxScannerColorCell alertDateTime = (DataGridViewTextBoxScannerColorCell)grdResults.Rows[row].Cells["Alert Time"];
        alertDateTime.HighlightOnly = true;
        alertDateTime.Value = lastBar.TradeDate;
        alertDateTime.Interval = 5000;

        grdResults.Rows[row].Cells["Trade Time"].Value = lastBar.TradeDate;
        grdResults.Rows[row].Cells["Symbol"].Value = symbol;
        grdResults.Rows[row].Cells["Last"].Value = Format.ToUsCurrency(lastBar.ClosePrice);
        grdResults.Rows[row].Cells["Volume"].Value = Format.ToLocalInteger((int)lastBar.VolumeF);

        DataGridViewButtonCell button = (DataGridViewButtonCell)grdResults.Rows[row].Cells["Trade"];
        button.Value = "Trade";
        button.FlatStyle = FlatStyle.Flat;

        button = (DataGridViewButtonCell)grdResults.Rows[row].Cells["Chart"];
        button.Value = "Chart";
        button.FlatStyle = FlatStyle.Flat;

        button = (DataGridViewButtonCell)grdResults.Rows[row].Cells["Settings"];
        button.Value = "Settings";
        button.FlatStyle = FlatStyle.Flat;

        button = (DataGridViewButtonCell)grdResults.Rows[row].Cells["Start"];
        button.Value = "Start";
        button.FlatStyle = FlatStyle.Flat;

        DataGridViewImageButtonCell start = (DataGridViewImageButtonCell)grdResults.Rows[row].Cells["Start"];
        start.ImageOn = Resources.Play;
        start.ImageOff = Resources.Pause;
        start.OffsetY = 4;

        DataGridViewImageButtonCell @lock = (DataGridViewImageButtonCell)grdResults.Rows[row].Cells["Locked"];
        @lock.ImageOn = Resources.Lock;
        @lock.ImageOff = Resources.Unlock;
        @lock.OffsetY = 2;
      }

      lblInfo.Text = string.Empty;
      pnlProgress.Visible = false;
      pnlProgress.Cursor = Cursors.Arrow;

      EnableControls(true);

      return true;
    }

    private void OnAlert(string symbol, string alertName)
    {
      if (m_loading || m_stop)
      {
        return;
      }

      DataGridViewRow row = m_DGVRowDictionary[symbol];
      if (row == null)
      {
        return;
      }

      row.Cells["Alert Time"].Value = row.Cells["Trade Time"].Value;
    }

    private void oScript_ScriptError(string symbol, string alertName, string description)
    {
      if (m_loading || m_dialogShown)
      {
        return;
      }

      m_dialogShown = true;

      //Prevent the error from occuring over and over
      Alert oAlert = m_TSAlertDictionary[symbol];
      if (oAlert == null)
      {
        return;
      }

      //Find and highlight the record
      int row = -1;
      for (int n = 0; n < grdResults.Rows.Count; n++)
      {
        if (string.Compare(grdResults.Rows[n].Cells["Symbol"].Value.ToString(), symbol, true) == 0)
        {
          row = n;
          break;
        }
      }

      DataGridViewImageButtonCell cell = (DataGridViewImageButtonCell)grdResults.Rows[row].Cells["Start"];
      if (cell == null)
      {
        return;
      }

      TempScript = oAlert.AlertName;
      if (string.IsNullOrEmpty(oAlert.AlertName)) //if not already paused
      {
        TempScript = oAlert.AlertScript;
        oAlert.AlertName = oAlert.AlertScript; //Using AlertName as a Tag
        oAlert.AlertScript = string.Empty; //Pause
        cell.Checked = true;
      }

      // Display the error message
      if (!string.IsNullOrEmpty(oAlert.ScriptHelp))
      {
        if (MessageBox.Show("Your script generated an error: " + Environment.NewLine +
            description + Environment.NewLine +
            "Would you like to view help regarding this error?", "Error:",
            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          MessageBox.Show(oScript.ScriptHelp, symbol, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      else
      {
        MessageBox.Show("Your script generated an error:" + Environment.NewLine +
                        description, symbol, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      //Show edit dialog
      TempScript = string.IsNullOrEmpty(oAlert.AlertScript) ? oAlert.AlertName : oAlert.AlertScript;

      frmScannerScript settings = new frmScannerScript(this) { HeaderText = symbol + " Script" };
      DialogResult result = settings.ShowDialog();
      m_dialogShown = false;
      if (result == DialogResult.Abort)
      {
        m_frmMain.OpenURL("http://www.modulusfe.com/TradeScript/TradeScript.pdf", "TradeScript Help");
        return;
      }

      if (result == DialogResult.OK)
      {
        oAlert.AlertScript = TempScript;
        oAlert.AlertName = string.Empty;
        cell.Checked = false; //Script is runing now
      }
    }

    //Validates the form for saving
    private bool VerifyForm()
    {
      frmScannerMainSettings settings = new frmScannerMainSettings(this);

      var result = settings.ShowDialog();
      if (result == DialogResult.Abort)
      {
        m_frmMain.OpenURL("http://www.modulusfe.com/TradeScript/TradeScript.pdf", "TradeScript Help");
        return false;
      }

      if (Symbols.Count < 2)
      {
        MessageBox.Show("Please enter a list of symbols", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      return true;
    }

    //Enables/Disables controls for when the script is running
    private void EnableControls(bool Enable, bool UpdateUI)
    {
      grpSaveLoadScanner.Enabled = Enable;
      if ((Enable))
      {
        cmdScanner.Text = "&Run Scanner";
      }
      else
      {
        cmdScanner.Text = "&Stop Scanner";
      }

    }

    private void EnableControls(bool Enable)
    {
      EnableControls(Enable, true);
    }

    //Checks the scripts for errors 
    private bool TestScripts()
    {

      Validate script = new Validate { License = "XRT93NQR79ABTW788XR48" };

      string err = "";

      if (!string.IsNullOrEmpty(Script))
      {
        err = script.Validate(Script);
      }

      if (!string.IsNullOrEmpty(err))
      {
        if (!string.IsNullOrEmpty(script.ScriptHelp))
        {
          if (MessageBox.Show("Your script generated an error. Would you like to view help regarding this error?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
            MessageBox.Show(oScript.ScriptHelp, "Help:", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
        else
        {
          MessageBox.Show("Your script generated an error.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
      }

      return true;
    }

    //Get the user inputed selection information 
    private ChartSelection GetSelection(string Symbol)
    {
      ChartSelection selection = new ChartSelection
                                   {
                                       Periodicity = (M4Core.Entities.Periodicity)Periodicity,
                                     Interval = Interval,
                                     Bars = Bars,
                                     Symbol = Symbol
                                   };
      return selection;
    }


    #region Save/Load
    // Loads the list of previously saved scanners     
    private void LoadScanners()
    {
      string[] alerts = null;
      try
      {
        object[] _ = svc.ListUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey);
        if (_ != null)
        {
          alerts = new string[_.Length];
          for (int i = 0; i < _.Length; i++)
            alerts[i] = _[i].ToString();
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return;
      }
      if (alerts == null)
      {
        return;
      }

      //Add them to combobox
      cboScanners.Items.Clear();
      for (int n = 0; n <= alerts.Length - 1; n++)
      {
        if (alerts[n].StartsWith("Scanner Settings: "))
        {
          cboScanners.Items.Add(alerts[n].Replace("Scanner Settings: ", ""));
        }
      }
    }

    private void cmdSave_Click(object sender, EventArgs e)
    {
      SaveScanner(false);
    }

    //Saves the scanner
    public void SaveScanner()
    {
      SaveScanner(false);
    }

    public void SaveScanner(bool prompt)
    {
      string scanName = txtScannerName.Text;

      if (prompt) //Ask the user first (may be called by frmMain)
      {
        if (string.IsNullOrEmpty(scanName))
        {
          scanName = "Untitled";
        }

        if (MessageBox.Show("Save scanner '" + scanName + "'?",
                            "Confirm",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.No)
        {
          return;
        }
      }

      cmdSave.Enabled = false;
      cmdDelete.Enabled = false;
      cboScanners.Enabled = false;

      char d = Utils.Chr(134);
      char d2 = Utils.Chr(182);
      StringBuilder strRows = new StringBuilder();
      Alert oAlert;
      string symbol, script = string.Empty;
      bool unlocked, paused;

      for (int n = 0; n < grdResults.Rows.Count; n++)
      {
        //Symbol, Script, Locked, Paused
        symbol = grdResults.Rows[n].Cells["Symbol"].Value.ToString();
        oAlert = m_TSAlertDictionary[symbol];
        if (oAlert == null)
        {
          continue;
        }

        script = oAlert.AlertScript;
        if (string.IsNullOrEmpty(script))
        {
          script = oAlert.AlertName;
        }

        DataGridViewImageButtonCell cell = (DataGridViewImageButtonCell)grdResults.Rows[n].Cells["Locked"];
        unlocked = !cell.Checked;
        cell = (DataGridViewImageButtonCell)grdResults.Rows[n].Cells["Start"];
        paused = false;
        if (cell != null)
        {
          paused = cell.Checked;
        }

        strRows.Append(symbol)
          .Append(d2).Append(script)
          .Append(d2).Append(unlocked)
          .Append(d2).Append(paused)
          .Append(Utils.Chr(145));
      }

      StringBuilder data = new StringBuilder();
      data.Append(strRows.ToString())
        .Append(d).Append(Periodicity)
        .Append(d).Append(Interval)
        .Append(d).Append(Bars)
        .Append(d).Append(Script);

      try
      {
        svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                        "Scanner Settings: " + scanName, data.ToString());
        m_changed = false;
      }
      catch (Exception)
      {
        MessageBox.Show("Failed to save scanner.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      bool found = cboScanners.FindStringExact(txtScannerName.Text) != -1;
      if (!found)
      {
        LoadScanners();
        int idx = cboScanners.FindStringExact(scanName);
        if (idx != -1)
        {
          cboScanners.SelectedIndex = idx;
        }
      }

      UpdateName(txtScannerName.Text);

      cmdSave.Enabled = true;
      if (cboScanners.SelectedIndex != -1)
      {
        cmdDelete.Enabled = true;
      }

      cboScanners.Enabled = true;
    }

    private void LoadScanner()
    {
      string scanName = cboScanners.Text.Trim();
      if (string.IsNullOrEmpty(scanName))
      {
        return;
      }

      m_loading = true;

      cmdSave.Enabled = false;
      cmdDelete.Enabled = false;
      cboScanners.Enabled = false;
      txtScannerName.Text = scanName;

      string data;
      try
      {
        data = svc.GetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey, "Scanner Settings: " + scanName);
      }
      catch (Exception)
      {
        MessageBox.Show("Failed to load scanner.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        cmdSave.Enabled = true;
        cmdDelete.Enabled = true;
        cboScanners.Enabled = true;
        return;
      }

      string[] text = data.Split(Utils.Chr(134));
      if (text.Length < 4)
      {
        return;
      }

      List<string> strRows = new List<string>(text[0].Split(Utils.Chr(145)));
      char d = Utils.Chr(182);

      //Clear everything
      m_stop = false;
      m_TSAlertDictionary.Clear();
      m_DGVRowDictionary.Clear();
      grdResults.Rows.Clear();
      DataGridViewBarGraphColumn volumeCol = (DataGridViewBarGraphColumn)grdResults.Columns["Volume"];
      if (volumeCol != null)
      {
        volumeCol.MaxValue = 0;
      }

      //Load all symbols
      Symbols.Clear();
      string[] strRow;

      for (int n = 0; n < strRows.Count - 1; n++)
      {
        strRow = strRows[n].Split(d);
        Symbols.Add(strRow[0]);
      }

      //Global settings
      switch (text[1])
      {
        case "Minutely":
              Periodicity = (int)M4Core.Entities.Periodicity.Minutely;
          break;
        case "Hourly":
          Periodicity = (int)M4Core.Entities.Periodicity.Hourly;
          break;
        case "Daily":
          Periodicity = (int)M4Core.Entities.Periodicity.Daily;
          break;
        case "Weekly":
          Periodicity = (int)M4Core.Entities.Periodicity.Weekly;
          break;
      }
      Interval = Convert.ToInt32(text[2]);
      Bars = Convert.ToInt32(text[3]);
      Script = text[4];

      //Prime data
      LoadAllSymbolsIntoMemory();

      //Go back through for settings
      for (int n = 0; n < strRows.Count - 2; n++)
      {
        strRow = strRows[n].Split(new[] { d });
        if (strRow.Length < 3) continue;

        for (int j = 0; j < grdResults.Rows.Count; j++)
        {
          if (string.Compare(grdResults.Rows[j].Cells["Symbol"].Value.ToString(), strRow[0], true) == 0)
          {
            DataGridViewImageButtonCell start = (DataGridViewImageButtonCell)grdResults.Rows[j].Cells["Start"];
            start.Checked = string.Compare(strRow[3], "true", true) == 0;

            Alert oAlert = m_TSAlertDictionary[strRow[0]];
            if (oAlert != null)
            {
              if (!start.Checked)
              {
                oAlert.AlertScript = strRow[1];
              }
              else
              {
                oAlert.AlertScript = strRow[1];
              }
            }//oAlert != null


            DataGridViewImageButtonCell @lock = (DataGridViewImageButtonCell)grdResults.Rows[j].Cells["Locked"];
            @lock.Checked = string.Compare(strRow[2], "true", true) == 0;

            break;
          }//if grdResults
        }//for j
      }//for n

      m_loading = false;
      m_changed = false;

      UpdateName(txtScannerName.Text);

      cmdSave.Enabled = true;
      cmdDelete.Enabled = true;
      cboScanners.Enabled = true;

      cmdDelete.Enabled = cboScanners.SelectedIndex != -1;
    }


    //Updates the Nevron tab text 
    public void UpdateName(string name)
    {
      ctlScanner scan;
      foreach (Telerik.WinControls.UI.Docking.DockWindow doc in m_frmMain.radDock2.DockWindows)
      {
        if (doc.Name != "ctlScanner")
        {
          continue;
        }

        scan = (ctlScanner)doc.Controls[0];
        if (scan.Handle == Handle)
        {
          doc.Text = "Scanner: " + name;
        }
      }
    }

    //Load the selected scanner
    private void cboScanners_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (m_changed && cboScanners.Enabled)
      {
        SaveScanner(true);
      }

      LoadScanner();
    }

    //Stops the scanner
    public void StopScanner()
    {
      m_stop = true;
    }

    private void cmdScanner_Click(object sender, EventArgs e)
    {
      if (cmdScanner.Text == "&Run Scanner")
      {
        if (!VerifyForm())
        {
          return;
        }

        LoadAllSymbolsIntoMemory();
        m_changed = true;
        cmdEditScript.Enabled = true;

      }
      else
      {
        if (MessageBox.Show(
          "Stopping the scanner will require a complete re-priming of data if you wish to restart the scanner",
          "Continue?",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          EnableControls(true);
          m_stop = true;
          cmdEditScript.Enabled = false;
        }
      }
    }
    #endregion

    #region Implementation of IDataSubscriber

    public void PriceUpdate(string Symbol, DateTime TradeDate, double LastPrice, long Volume)
    {
      m_TimeStamp = TradeDate;
    }

    //Bar udpates
    public void BarUpdate(string Symbol, M4.DataServer.Interface.Periodicity BarType, 
      int BarSize, M4.DataServer.Interface.BarData Bar, bool IsNewBar)
    {
      if (m_stop)
      {
        return;
      }

      Alert oAlert = GetTSAlertObject(Symbol);
      if (oAlert == null)
      {
        return; // Not watching this symbol
      }

      double jdate = 0, o = 0, h = 0, l = 0, c = 0;
      int v = 0;
      if (!oAlert.GetRecordByIndex(oAlert.RecordCount, ref jdate, ref o, ref h, ref l, ref c, ref v))
      {
        return;
      }

      if (IsNewBar)
      {
        // Append new bar                
        double jDate = oAlert.ToJulianDate(Bar.TradeDate.Year, Bar.TradeDate.Month, Bar.TradeDate.Day,
                                           Bar.TradeDate.Hour, Bar.TradeDate.Minute, Bar.TradeDate.Second, Bar.TradeDate.Millisecond);
        oAlert.AppendRecord(jDate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
      }
      else
      {   // Edit existing bar
        oAlert.EditRecord(jdate, Bar.OpenPrice, Bar.HighPrice, Bar.LowPrice, Bar.ClosePrice, (int)Bar.VolumeF);
      }

      //Update the DataGridView
      DataGridViewRow row = m_DGVRowDictionary[Symbol];
      if (row != null)
      {
        row.Cells["Trade Time"].Value = m_TimeStamp;
        row.Cells["Last"].Value = Format.ToUsCurrency(Bar.ClosePrice);
        row.Cells["Volume"].Value = Format.ToLocalInteger((int)Bar.VolumeF);
      }
    }

    public IntPtr GetHandle()
    {
      return IsHandleCreated ? Handle : IntPtr.Zero;
    }

    #endregion

    //Creates a variable width gradient bar with text 
    public static Image GradientBar(int Width, int Height)
    {

      System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      Stream myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_green.bmp");
      Bitmap bmp = new Bitmap(myStream);

      Image img = bmp;

      img = ResizeImage(img, Width, Height);

      Bitmap OrgImg = (Bitmap)img;
      Bitmap TargImg = new Bitmap(OrgImg.Width, OrgImg.Height);
      Graphics gr = Graphics.FromImage(TargImg);

      gr.DrawImage(OrgImg, 0, 0, OrgImg.Width, OrgImg.Height);
      //gr.DrawString(Text, Font, FontColor, 1, 1) 

      return TargImg;

    }

    //Resizes an image 
    private static Image ResizeImage(Image originalImage, int width, int height)
    {

      if (width < 1) width = 1;
      if (height < 1) height = 1;

      Image finalImage = new Bitmap(width, height);
      Graphics graphic = Graphics.FromImage(finalImage);

      graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
      graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

      Rectangle rectangle = new Rectangle(0, 0, width, height);

      graphic.DrawImage(originalImage, rectangle);

      return finalImage;

    }

    //Handles grid button clicks
    private void grdResults_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      // this method handles only cell click
      // if e.RowIndex == -1 it means user clicked on header, so ignore this
      if (e.RowIndex == -1)
      {
        return;
      }

      try
      {
        string symbol = grdResults.SelectedRows[0].Cells["Symbol"].Value.ToString();
        Alert oAlert = m_TSAlertDictionary[symbol];
        if (oAlert == null)
        {
          return;
        }

        if (e.ColumnIndex == 5) //Trade
        {
          grdResults.Rows[e.RowIndex].Selected = true;
          //m_frmMain.EnterOrder(symbol);
        }
        else if (e.ColumnIndex == 6) //Chart
        {
          EnableControls(true, false);
          grdResults.Cursor = Cursors.WaitCursor;
          grdResults.Rows[e.RowIndex].Selected = true;
          ChartSelection selection = GetSelection(symbol);
          m_frmMain.CreateNewCtlPainel(selection, new Action<CtlPainelChart>(chart => { }));
          grdResults.Cursor = Cursors.Arrow;
          EnableControls(true, true);
        }
        else if (e.ColumnIndex == 7) //Change the script for this individual alert
        {
          TempScript = string.IsNullOrEmpty(oAlert.AlertScript)
                         ? oAlert.AlertName
                         : oAlert.AlertScript;
          frmScannerScript settings = new frmScannerScript(this) { HeaderText = symbol + " Script" };
          var result = settings.ShowDialog();
          if (result == DialogResult.Abort)
          {
            m_frmMain.OpenURL("http://www.modulusfe.com/TradeScript/TradeScript.pdf", "TradeScript Help");
            return;
          }
          if (result == DialogResult.OK)
          {
            oAlert.AlertScript = TempScript;
            oAlert.AlertName = string.Empty;
            DataGridViewImageButtonCell cell = (DataGridViewImageButtonCell)grdResults.Rows[e.RowIndex].Cells["Start"];
            if (cell == null) return;
            cell.Checked = false; //Script is running now
          }
        }//else if Column Index 7
        else if (e.ColumnIndex == 8) //Lock/unlock script to control effects of the "Edit Script" button
        {

        }
        else if (e.ColumnIndex == 9) //Play/pause
        {
          DataGridViewImageButtonCell cell = (DataGridViewImageButtonCell)grdResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
          if (cell == null) return;
          bool pause = cell.Checked;
          if (pause)
          {
            oAlert.AlertName = oAlert.AlertScript; //Using AlertName as a Tag
            oAlert.AlertScript = string.Empty;
          }
          else
          {
            if (!string.IsNullOrEmpty(oAlert.AlertName)) //Play
            {
              oAlert.AlertScript = oAlert.AlertName;
              oAlert.AlertName = string.Empty;
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    //Allows the user to enter a new script that will be applied to all unlocked symbols
    private void cmdEditScript_Click(object sender, EventArgs e)
    {
      frmScannerScript settings = new frmScannerScript(this);
      TempScript = Script;
      settings.HeaderText = "Script (will be applied to all unlocked symbols)";
      var result = settings.ShowDialog();
      if (result == DialogResult.Abort)
      {
        m_frmMain.OpenURL("http://www.modulusfe.com/TradeScript/TradeScript.pdf", "TradeScript Help");
        return;
      }
      if (result == DialogResult.OK)
      {
        Script = TempScript;
        //Apply script to each unlocked symbol
        for (int n = 0; n < grdResults.Rows.Count; n++)
        {
          string symbol = grdResults.Rows[n].Cells["Symbol"].Value.ToString();
          bool locked = ((DataGridViewImageButtonCell)grdResults.Rows[n].Cells["Locked"]).Checked;
          if (m_TSAlertDictionary.ContainsKey(symbol))
          {
            if (!locked) //This symbol's script isn't locked so change it
            {
              m_TSAlertDictionary[symbol].AlertScript = Script;
            }
          }
        }
      }
    }

    //Deletes a scanner from the web service
    private void cmdDelete_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(cboScanners.Text)) return;
      if (MessageBox.Show("Delete scanner", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2) != DialogResult.OK)
        return;

      string alertName = cboScanners.Text;
      try
      {
        svc.SetUserData(frmMain2.ClientId, frmMain2.ClientPassword, frmMain2.LicenseKey,
                "Trade Alert Settings: " + alertName, "");
        LoadScanners();
        cboScanners.SelectedIndex = cboScanners.Items.Count - 1;
      }
      catch (Exception)
      {
        MessageBox.Show("Failed to delete alert", "Error", MessageBoxButtons.OK);
      }
    }

    private void ctlScanner_Load(object sender, EventArgs e)
    {
      LoadScanners();
      m_changed = false;
    }
  }
}