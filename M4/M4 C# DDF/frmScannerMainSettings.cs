/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using M4.Properties;
using M4Core.Entities;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmScannerMainSettings : NForm
  {
    private readonly ctlScanner m_scanner;

    public frmScannerMainSettings(ctlScanner owner)
    {
      InitializeComponent();

      m_scanner = owner;
      Palette = frmMain.NevronPalette;
    }

    private void OK_Button_Click(object sender, EventArgs e)
    {
      if (!VerifyForm()) return;
      
      DialogResult = DialogResult.OK;
      Close();
    }



    //Validates the form for saving
    private bool VerifyForm()
    {
      uint uintVal;
      if (string.IsNullOrEmpty(cboPeriodicity.Text))
      {
        cboPeriodicity.Focus();
        MessageBox.Show("Please select a periodicity", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!uint.TryParse(txtBars.Text, out uintVal))
      {
        txtBars.SelectAll();
        txtBars.Focus();
        MessageBox.Show("Please enter the number of bars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!uint.TryParse(txtInterval.Text, out uintVal))
      {
        txtInterval.SelectAll();
        txtBars.Focus();
        MessageBox.Show("Please enter the interval", "Error: ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      GetSymbols();

      if (m_scanner.Symbols.Count  < 2)
      {
        MessageBox.Show("Please enter a list of symbols", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (m_scanner.Symbols.Count > 5000)
      {
        //TODO: Change your symbol count restriction here
        MessageBox.Show("Please enter fewer symbols (max 5000)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      //Update owner
      switch (cboPeriodicity.Text)
      {
        case "Minute":
          m_scanner.Periodicity = Periodicity.Minutely;
          break;
        case "Hour":
          m_scanner.Periodicity = Periodicity.Hourly;
          break;
        case "Day":
          m_scanner.Periodicity = Periodicity.Daily;
          break;
        case "Week":
          m_scanner.Periodicity = Periodicity.Weekly;
          break;
      }

      m_scanner.Bars = Convert.ToInt32(txtBars.Text);
      m_scanner.Interval = Convert.ToInt32(txtInterval.Text);
      m_scanner.Script = txtScript.Text;

      Settings.Default.Interval = m_scanner.Interval;
      Settings.Default.Bars = m_scanner.Bars;
      Settings.Default.Script = m_scanner.Script;
      Settings.Default.Periodicity = cboPeriodicity.Text;

      return true;
    }


    //Loads a CSV or text file with symbols (one symbol per crlf)
    private void GetSymbols()
    {
      string fileName = Settings.Default.ScanSymbols;
      if (string.IsNullOrEmpty(fileName)) return;

      FileInfo fi = new FileInfo(fileName);
      if (!fi.Exists) return;
      using (StreamReader sr = new StreamReader(fileName))
      {
        List<string> symbols = new List<string>();
        symbols.AddRange(sr.ReadToEnd().Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries));
        Dictionary<string, bool> d = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
        
        for (int i = 0; i < symbols.Count; i++)
        {
          if (symbols[i].Length < 1 || symbols[i].Length > 10 || d.ContainsKey(symbols[i]))
          {
            symbols.RemoveAt(i);
            i--;
            continue;
          }
        }
        m_scanner.Symbols = symbols;

        if (symbols.Count > 0)
        {
          int found = fileName.LastIndexOf('\\');
          if (found > 0)
            fileName = fileName.Substring(found + 1);
          lblSymbolFile.Text = fileName + Environment.NewLine + symbols.Count + " symbols loaded";
          return;
        }
        return;
      }

    }

    private void Cancel_button_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void cmdDocumentation_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Abort;
      Close();
    }

    //Estimates the maximum number of periods used in the script
    private void txtScript_TextChanged(object sender, EventArgs e)
    {
      int max = 0;
      int left = 0, right = 0;
      StringBuilder sNum = new StringBuilder();

      foreach (char c in txtScript.Text)
      {
        if (c == '(') left++;
        else if (c == ')') right++;

        bool inFunc = left != right;
        if (inFunc && (byte)c > 47 && (byte)c < 58)
        {
          sNum.Append(c);
        }
        else
        {
          int value;
          if (int.TryParse(sNum.ToString(), out value))
          {
            if (value > max) max = value;
          }
          sNum.Length = 0;
        }
      }

      txtBars.Text = max.ToString();
    }

    private void cmdBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog1.CheckFileExists = true;
      OpenFileDialog1.FileName = string.Empty;
      if (OpenFileDialog1.ShowDialog() != DialogResult.OK)
        return;

      Settings.Default.ScanSymbols = OpenFileDialog1.FileName;

      GetSymbols();
    }

    private void frmScannerMainSettings_Load(object sender, EventArgs e)
    {
      GetSymbols();

      txtScript.Text = Settings.Default.Script;
      txtInterval.Text = Utils.ValueOrDef(Settings.Default.Interval, 0, 5).ToString();
      txtBars.Text = Utils.ValueOrDef(Settings.Default.Bars, 0, 250).ToString();
      
      cboPeriodicity.SelectedIndex = 0;
      string periods = Utils.ValueOrDef(Settings.Default.Periodicity, "", "Minute");
      int index = cboPeriodicity.FindStringExact(periods);
      if (index != -1)
        cboPeriodicity.SelectedIndex = index;
    }
  }
}
