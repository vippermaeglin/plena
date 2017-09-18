/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TradeScriptLib;
using M4.M4v2.Chart;
using M4.DataServer.Interface;


namespace M4.M4v2.BackTest.ORIGINAL
{
  public partial class ctlBacktest : UserControl, IDataSubscriber
  {
    #region Initialization and Members
    private readonly ScriptOutput oScript;
    private readonly Backtest oBacktest;
    private readonly CultureInfo ciEnUs = new CultureInfo("en-us");

    public class Bar
    {
      public double jDate = 0;
      public double O = 0;
      public double H = 0;
      public double L = 0;
      public double C = 0;
      public long V = 0;
    }

    public bool m_changed;

    private CtlPainelChart m_chart;
    private readonly ctlData m_ctlData;
    private readonly frmMain2 m_frmMain;


    public ctlBacktest(frmMain2 oMain, ctlData oData)
    {
      InitializeComponent();

      m_frmMain = oMain;
      m_ctlData = oData;
      oBacktest = new BacktestClass { License = "XRT93NQR79ABTW788XR48" };
      oScript = new ScriptOutputClass { License = "XRT93NQR79ABTW788XR48" };

      txtBars.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtInterval.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);
      txtSymbol.GotFocus += (sender, e) => Text_Focus((TextBoxBase)sender);

      oScript.ScriptError += oScript_ScriptError;
      oBacktest.ScriptError += oBacktest_ScriptError;

      //tabScripts.Visible = false;
    }

    #endregion

    #region Controls
    private void ctlBacktest_Load(object sender, EventArgs e)
    {
      cboPeriodicity.SelectedIndex = 0;
      m_changed = false;
    }

    //Enables/Disables controls for when the script is running
    private void EnableControls(bool Enable)
    {
      grpData.Enabled = Enable;
      tabScripts.Enabled = Enable;
      cmdBacktest.Enabled = Enable;
      if (Enable)
      {
        cmdBacktest.Text = "&Back Test";
      }
      else
      {
        cmdBacktest.Text = "&Stop Backtest";
      }
    }

    //Display the TradeScript documentation
    private void cmdDocumentation_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.modulusfe.com/tradescript/TradeScript.pdf",
                        "TradeScript Help");
    }

    private void ctlBacktest_Resize(object sender, EventArgs e)
    {
      tabScripts.Height = (Height - tabScripts.Top) - 75;
      cmdBacktest.Top = (tabScripts.Top + tabScripts.Height) + 10;
      cmdDocumentation.Top = (tabScripts.Top + tabScripts.Height) + 10;
      grpTrades.Width = (Width - grpTrades.Left) - 8;
      grpTrades.Height = (tabScripts.Top + tabScripts.Height) - 10;
      m_ListTrades.Columns[0].Width = grpTrades.Width - 10;
    }

    //Handles GotGocus for several text boxes
    private static void Text_Focus(TextBoxBase textBox)
    {
      textBox.SelectAll();
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

    #region Back Testing
    private void oScript_ScriptError(string description)
    {
      if (oScript.ScriptHelp != "")
      {
        if (MessageBox.Show("Your script generated an error:\r\n" + description +
                            "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          MessageBox.Show(oScript.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      else
      {
        MessageBox.Show("Your script generated an error:\r\n" + description, "Error:", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
      EnableControls(true);
    }

    private void oBacktest_ScriptError(string description)
    {
      if (oBacktest.ScriptHelp != "")
      {
        if (MessageBox.Show("Your script generated an error:\r\n" + description +
                            "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          MessageBox.Show(oBacktest.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
      else
      {
        MessageBox.Show("Your script generated an error:\r\n" + description, "Error:", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
      EnableControls(true);
    }

    private void cmdBacktest_Click(object sender, EventArgs e)
    {
      string Results = RunBacktest();
      if (Results != "")
      {
        DisplayResults(Results);
      }
    }

    //Validates the form for saving
    private bool VerifyForm()
    {
      uint uintVal;
      txtSymbol.Text = txtSymbol.Text.Trim();
      if (txtSymbol.Text == "")
      {
        txtSymbol.Focus();
        MessageBox.Show("Please enter a symbol", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (cboPeriodicity.Text == "")
      {
        cboPeriodicity.Focus();
        MessageBox.Show("Please select a periodicity", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!uint.TryParse(txtBars.Text, out uintVal))
      {
        txtBars.SelectAll();
        txtBars.Focus();
        MessageBox.Show("Please enter the number of bars", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!uint.TryParse(txtInterval.Text, out uintVal))
      {
        txtInterval.SelectAll();
        txtInterval.Focus();
        MessageBox.Show("Please enter the interval", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      return true;
    }

    //Checks the scripts for errors
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
          if (MessageBox.Show("Your exit long script generated an error:\r\n" + err.Replace("Error: ", "") +
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
      if (!string.IsNullOrEmpty(err))
      {
        tabScripts.SelectedIndex = 3;
        if (script.ScriptHelp != "")
        {
          if (MessageBox.Show("Your exit short script generated an error:\r\n" + err.Replace("Error: ", "") +
                              "\r\nWould you like to view help regarding this error?", "Error:", MessageBoxButtons.YesNo,
                              MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            MessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
        }
        else
        {
          MessageBox.Show("Your exit-short script generated an error:\r\n" + err.Replace("Error: ", ""), "Error:",
                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      return true;
    }

    //Runs the backtest and displays the chart with buy/sell/exit icons.
    private string RunBacktest()
    {
      string ret = string.Empty;

      oScript.ClearRecords();
      oBacktest.ClearRecords();

      if (!VerifyForm()) return string.Empty;
      if (!TestScripts()) return string.Empty;
      if (m_ctlData == null) return string.Empty;

      cmdBacktest.Text = "&Stop Backtest";
      EnableControls(false);

      Periodicity periodicity;
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

      cmdBacktest.Enabled = false;


      Telerik.WinControls.UI.Docking.DockWindow activeDoc = frmMain2.GInstance.radDock2.DocumentManager.ActiveDocument;


      //Get the data selection
      M4Core.Entities.ChartSelection selection = new M4Core.Entities.ChartSelection
      {
        Periodicity = (M4Core.Entities.Periodicity)periodicity,
        Symbol = txtSymbol.Text,
        Interval = Convert.ToInt32(txtInterval.Text),
        Bars = Convert.ToInt32(txtBars.Text)
      };

      m_ctlData.LoadRealTimeCtlPainelChartAsync2(selection, new Action<CtlPainelChart>(chart => {
          m_chart = chart; 
          if (m_chart == null) goto Quit;

          DataManager.BarData[] bars = m_chart.GetDataFromChart();
          if (bars.Length < 1) goto Quit;

          m_chart.Subscribers += 1;
          m_chart.RealTimeUpdates = false;

          //Get historic data
          cmdBacktest.Enabled = true;
          if (bars.Length < 3) goto Quit; //Bad request 

          //Insert the data into all four instances of TradeScript
          oScript.ClearRecords();
          oBacktest.ClearRecords();

          DateTime td;
          double jdate;
          for (int n = 1; n < bars.Length - 1; n++)
          {
              td = bars[n].TradeDate;
              jdate = oScript.ToJulianDate(td.Year, td.Month, td.Day, td.Hour, td.Minute, td.Second, 0);
              oScript.AppendRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice,
                                   bars[n].LowPrice, bars[n].ClosePrice, (int)bars[n].Volume);
              oBacktest.AppendRecord(jdate, bars[n].OpenPrice, bars[n].HighPrice,
                                     bars[n].LowPrice, bars[n].ClosePrice, (int)bars[n].Volume);
          }

          //TODO
          ret = oBacktest.Backtest(txtBuyScript.Text, txtSellScript.Text, txtExitLongScript.Text, txtExitShortScript.Text, 0.001);
          if (string.IsNullOrEmpty(ret)) goto Quit;

          string output =
            oScript.GetScriptOutput(txtBuyScript.Text + " AND \r\n" + txtSellScript.Text + " AND \r\n" + txtExitLongScript.Text +
                                    " AND \r\n" + txtExitShortScript.Text);
          if (string.IsNullOrEmpty(output)) goto Quit;

          int row;
          string[] cols;
          int col;
          string[] rows = output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
          string[] Header = SplitHeader(rows[0]);


          //StockChartX
          AxSTOCKCHARTXLib.AxStockChartX StockChartX1 = m_chart.StockChartX1;
          StockChartX1.RemoveAllSeries();

          string symbol = selection.Symbol;

          StockChartX1.Symbol = symbol;


          StockChartX1.AddChartPanel();
          StockChartX1.AddSeries(symbol + ".open", STOCKCHARTXLib.SeriesType.stCandleChart, 0);
          StockChartX1.AddSeries(symbol + ".high", STOCKCHARTXLib.SeriesType.stCandleChart, 0);
          StockChartX1.AddSeries(symbol + ".low", STOCKCHARTXLib.SeriesType.stCandleChart, 0);
          StockChartX1.AddSeries(symbol + ".close", STOCKCHARTXLib.SeriesType.stCandleChart, 0);
          StockChartX1.set_SeriesColor(symbol + ".close", ColorTranslator.ToOle(Color.Black));

          bool hasVolume = bars[0].Volume != -987654321;
          if (hasVolume)
          {
              StockChartX1.AddChartPanel();
              StockChartX1.AddSeries(symbol + ".volume", STOCKCHARTXLib.SeriesType.stVolumeChart, 1);
              StockChartX1.set_SeriesColor(symbol + ".volume", ColorTranslator.ToOle(Color.Blue));
              StockChartX1.set_SeriesWeight(symbol + ".volume", 3);
              StockChartX1.VolumePostfixLetter = "M"; //Google trades in millions
          }

          for (col = 6; col < Header.Length; col++)
          {
              int panel = StockChartX1.AddChartPanel();
              StockChartX1.AddSeries(Header[col], STOCKCHARTXLib.SeriesType.stLineChart, panel);

              System.Drawing.Color color;
              switch (col)
              {
                  case 6:
                      color = System.Drawing.Color.Blue;
                      break;
                  case 7:
                      color = System.Drawing.Color.Red;
                      break;
                  case 8:
                      color = System.Drawing.Color.Green;
                      break;
                  case 9:
                      color = System.Drawing.Color.Orange;
                      break;
                  case 10:
                      color = System.Drawing.Color.Purple;
                      break;
                  default:
                      color = System.Drawing.Color.Blue;
                      break;
              }
              StockChartX1.set_SeriesColor(Header[col], System.Drawing.ColorTranslator.ToOle(color));

          }

          for (row = 1; row < rows.Length; row++)
          {
              cols = rows[row].Split(',');
              jdate = GetJDate(cols[0]);
              StockChartX1.AppendValue(symbol + ".open", jdate, Convert.ToDouble(cols[1], ciEnUs));
              StockChartX1.AppendValue(symbol + ".high", jdate, Convert.ToDouble(cols[2], ciEnUs));
              StockChartX1.AppendValue(symbol + ".low", jdate, Convert.ToDouble(cols[3], ciEnUs));
              StockChartX1.AppendValue(symbol + ".close", jdate, Convert.ToDouble(cols[4], ciEnUs));
              if (hasVolume)
              {
                  StockChartX1.AppendValue(symbol + ".volume", jdate, Convert.ToDouble(cols[5], ciEnUs) / 1000000);
              }

              for (col = 6; col < cols.Length; col++)
              {
                  double value = Convert.ToDouble(cols[col], ciEnUs);
                  if (value == 0 && row < rows.Length * 0.2)
                  {
                      value = (double)STOCKCHARTXLib.DataType.dtNullValue;
                  }

                  StockChartX1.AppendValue(Header[col], jdate, value);
              }
          }

          StockChartX1.Update();

      Quit:

          if (m_chart != null)
          {
              m_chart.Show();
          }

          cmdBacktest.Text = "&Back Test";
          EnableControls(true);

          if (m_chart != null)
          {
              m_chart.Subscribers -= 1;
          }

      })); //Ensure the chart is new

      while (ret == string.Empty) Application.DoEvents();
      return ret;


    }

    private void DisplayResults(string Results)
    {
      if (string.IsNullOrEmpty(Results))
      {
        MessageBox.Show("No results. Make sure that at least Buy and Sell scripts are typed in.");
        return;
      }

      int found = Results.IndexOf("Trade Log:");
      AxSTOCKCHARTXLib.AxStockChartX StockChartX1 = m_chart.StockChartX1;
      if (found > 0)
      {
        int n;
        string[] tradeLog = Results.Substring(found + "trade log:".Length + 1).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        string[] report = Results.Substring(0, found - 1).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        //Loop through the trade log and the statistical report

        m_ListTrades.Items.Clear();
        m_ListTrades.Items.Add(report[0].Replace("Back tested from ", ""));
        for (n = 1; n < report.Length; n++)
        {
          m_ListTrades.Items.Add(report[n]);
        }

        //Get the trade
        for (n = 0; n < tradeLog.Length; n++)
        {
          long record;
          double value;

          m_ListTrades.Items.Add(tradeLog[n]);


          string[] trade = tradeLog[n].Split(',');
          double jDate = GetJDate(trade[0]);
          string signal = trade[1];
          double price = double.Parse(trade[2]);
          switch (signal)
          {
            case "LONG":
              record = StockChartX1.GetRecordByJDate(jDate);
              value = StockChartX1.GetValue(StockChartX1.Symbol + ".low", (int)record);
              StockChartX1.AddSymbolObject(0, value, (int)record, STOCKCHARTXLib.SymbolType.soBuySymbolObject,
                                           "BUY " + Convert.ToString(record), "Long at $" + Convert.ToString(price));
              break;

            case "SHORT":
              record = StockChartX1.GetRecordByJDate(jDate);
              value = StockChartX1.GetValue(StockChartX1.Symbol + ".high", (int)record);
              StockChartX1.AddSymbolObject(0, value, (int)record, STOCKCHARTXLib.SymbolType.soSellSymbolObject,
                                           "SELL " + Convert.ToString(record), "Short at $" + Convert.ToString(price));
              break;

            case "EXIT":
              record = StockChartX1.GetRecordByJDate(jDate);
              StockChartX1.AddSymbolObject(0, price, (int)record, STOCKCHARTXLib.SymbolType.soExitSymbolObject,
                                           "EXIT " + Convert.ToString(record), "Exit at $" + Convert.ToString(price));
              break;
          }
        }
        m_ListTrades.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
      }

      try
      {
        StockChartX1.ForcePaint();
      }
      catch (Exception) { }
    }

    private static string[] SplitHeader(string header)
    {
      List<string> Values = new List<string>();
      int p = 1;
      int n = 0;
      int PCnt = 0;
      while (n < header.Length)
      {
        if (header[n] == '\"') PCnt += 1;
        if (header[n] == ',' && (PCnt % 2) == 0)
        {
          string ret = header.Substring(p, n - p).Trim();
          p = n + 1;
          Values.Add(ret.Replace("\"", ""));
        }
        n += 1;
      }

      Values.Add(header.Substring(p, n - p).Trim().Replace("\"", ""));
      return Values.ToArray();
    }


    private double GetJDate(string szDate)
    {
      try
      {
        DateTime dtDate = DateTime.Parse(szDate);

        //Get a Julian date from the StockChartX control
        return m_chart.StockChartX1.ToJulianDate(dtDate.Year, dtDate.Month, dtDate.Day,
                                                 dtDate.Hour, dtDate.Minute, dtDate.Second);
      }
      catch (Exception ex)
      {
        Debug.WriteLine("GetJDate Error: " + ex.Message);
        return 0.0;
      }
    }

    #endregion
  }
}
