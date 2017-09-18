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
  public partial class frmNewPortfolio : NForm
  {
    public struct NewPortfolio
    {
      public string PortfolioName;
      public double StartingBalance;
    }

    public frmNewPortfolio()
    {
      InitializeComponent();

      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;

      txtName.GotFocus += (sender,e) => txtName.SelectAll();
      txtBalance.GotFocus += (sender,e) => txtBalance.SelectAll();
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (!Utils.IsNumeric(txtBalance.Text))
      {
        MessageBox.Show("Please enter a numeric value for the starting account balance.",
                        "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      DialogResult = DialogResult.OK;
      Close();
    }

    //Returns the portfolio name and starting account balance
    public NewPortfolio GetPortfolio()
    {
      NewPortfolio ret = new NewPortfolio();
      if (ShowDialog() == DialogResult.OK)
      {
        ret.PortfolioName = txtName.Text.Trim();
        try{
          ret.StartingBalance = Convert.ToDouble(txtBalance.Text.Trim());
        }
        catch { }
      }
      return ret;
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }
  
    #region Numeric Formatting
    private void txtBalance_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 8) return;
      string str = e.KeyChar.ToString();
      if (!Utils.IsNumeric(str) && str != "," && str != "." && str != Utils.GetCurrencySymbol())
        str = "";
      txtBalance.SelectedText += str;
      e.Handled = true;
    }

    private void frmNewPortfolio_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!Utils.IsNumeric(txtBalance.Text))
        txtBalance.Text = "0";
    }

    private void txtBalance_TextChanged(object sender, EventArgs e)
    {
      if (!Utils.IsNumeric(txtBalance.Text) && txtBalance.Text != Utils.GetCurrencySymbol())
        txtBalance.Text = "";
    }
    #endregion
  }
}
