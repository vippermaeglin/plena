/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmSymbol : NForm
  {
    private string m_Symbol;

    public frmSymbol()
    {
      InitializeComponent();

      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;

      txtSymbol.GotFocus += (sender,e) => txtSymbol.SelectAll();
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      m_Symbol = txtSymbol.Text.Trim().ToUpper();
      DialogResult = DialogResult.OK;
      Close();
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    private void frmSymbol_Load(object sender, EventArgs e)
    {
      txtSymbol.Focus();
    }

    //Returns a symbol. Returns OriginalSymbol if no symbol is selected.
    public string GetSymbol(string OriginalSymbol, string textCaption)
    {
      m_Symbol = OriginalSymbol;
      txtSymbol.Text = m_Symbol;
      Text = textCaption;
      return ShowDialog() == DialogResult.OK ? m_Symbol : string.Empty;
    }
  }
}
