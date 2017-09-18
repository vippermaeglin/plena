/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.IO;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmFindSymbol : NForm
  {
    private string m_Symbol;

    public frmFindSymbol()
    {
      InitializeComponent();

      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;

      txtSearch.GotFocus += (sender, e) => txtSearch.SelectAll();
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      FindSymbol();
    }

    public string GetSymbol()
    {
      m_Symbol = "";
      dgvSymbols.Rows.Clear();
      Height = 105;
      ShowDialog();
      return m_Symbol;
    }

    //Find the symbol(s)
    private void FindSymbol()
    {
      Cursor = Cursors.WaitCursor;
      dgvSymbols.Rows.Clear();
      Height = 105;
      Application.DoEvents();
      if (!File.Exists(Application.StartupPath + @"\Symbols.txt"))
      {
        MessageBox.Show("Missing symbols definition file!", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Cursor = Cursors.Arrow;
      }
      else
      {
        string[] symbols;
        try
        {
          StreamReader sr = new StreamReader(Application.StartupPath + @"\Symbols.txt");
          symbols = sr.ReadToEnd().Split(new[] { '\r', '\n' });
          sr.Close();
        }
        catch (Exception e)
        {
          MessageBox.Show(e.Message, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          Cursor = Cursors.Arrow;
          return;
        }
        string search = txtSearch.Text.ToLower();
        if (search != "")
        {
          for (int n = 0; n < symbols.Length; n++)
          {
            string[] symbol = symbols[n].Split(new[] { "          " }, StringSplitOptions.RemoveEmptyEntries);
            if ((symbol.Length > 1) && symbol[0].IndexOf(search) != -1)
            {
              dgvSymbols.Rows.Add();
              dgvSymbols.Rows[dgvSymbols.Rows.Count - 1].Cells[0].Value = symbol[1];
              dgvSymbols.Rows[dgvSymbols.Rows.Count - 1].Cells[1].Value = symbol[0];
            }
          }
          if (dgvSymbols.Rows.Count > 0)
          {
            Height = 248;
          }
          else
          {
            MessageBox.Show("No symbols found", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          Cursor = Cursors.Arrow;
        }
      }
    }
  }
}
