/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using Nevron.UI.WinForm.Controls;

namespace M4
{
  public partial class frmPriceStyle : NForm
  {
    private string m_PriceStyle;
    private AxSTOCKCHARTXLib.AxStockChartX m_StockChartX;

    public frmPriceStyle()
    {
      InitializeComponent();
      if (frmMain.NevronPalette != null)
        Palette = frmMain.NevronPalette;

      txtInputA.GotFocus += (sender,e) => txtInputA.SelectAll();
      txtInputB.GotFocus += (sender,e) => txtInputB.SelectAll();
      txtInputC.GotFocus += (sender,e) => txtInputC.SelectAll();
    }

    //Returns a StockChartX price style selection from the user
    private void cmdOK_Click(object sender, EventArgs e)
    {
      double A = 0;
      double B = 0;
      try
      {
        A = Convert.ToDouble(txtInputA.Text);
      }
      catch { }
      try
      {
        B = Convert.ToDouble(txtInputB.Text);
      }
      catch { }
      switch (m_PriceStyle)
      {
        case "Point && Figure":
          m_StockChartX.set_PriceStyleParam(1, A); //Box size
          m_StockChartX.set_PriceStyleParam(2, B); //Reversal size
          break;
        case "Three Line Break":
          m_StockChartX.set_PriceStyleParam(1, A); //Line break (1 to n)
          break;
        case "Renko":
          m_StockChartX.set_PriceStyleParam(1, A); //Box size
          break;
        case "Kagi":
          m_StockChartX.set_PriceStyleParam(1, A); //Reversal size
          m_StockChartX.set_PriceStyleParam(2, B); //Points or percent (eg 1 or 0.01)
          break;
      }

      m_StockChartX.Update();
      Close();
    }

    public void GetInput(AxSTOCKCHARTXLib.AxStockChartX Chart, string PriceStyle)
    {
      m_StockChartX = Chart;

      txtInputA.Visible = false;
      txtInputB.Visible = false;
      txtInputC.Visible = false;
      lblInputA.Text = "";
      lblInputB.Text = "";
      lblInputC.Text = "";

      m_PriceStyle = PriceStyle;

      switch (m_PriceStyle)
      {
        case "Point && Figure":
          txtInputA.Text = m_StockChartX.get_PriceStyleParam(1).ToString();
          lblInputA.Text = "Box Size";
          txtInputA.Visible = true;
          lblInputA.Visible = true;
          txtInputB.Text = m_StockChartX.get_PriceStyleParam(2).ToString();
          lblInputB.Text = "Reversal Size";
          txtInputB.Visible = true;
          lblInputB.Visible = true;
          break;
        case "Three Line Break":
          txtInputA.Text = m_StockChartX.get_PriceStyleParam(1).ToString();
          lblInputA.Text = "Line Break";
          txtInputA.Visible = true;
          lblInputA.Visible = true;
          break;
        case "Renko":
          txtInputA.Text = m_StockChartX.get_PriceStyleParam(1).ToString();
          lblInputA.Text = "Box Size";
          txtInputA.Visible = true;
          lblInputA.Visible = true;
          break;
        case "Kagi":
          txtInputA.Text = m_StockChartX.get_PriceStyleParam(1).ToString();
          lblInputA.Text = "Reversal Size";
          txtInputA.Visible = true;
          lblInputA.Visible = true;
          txtInputB.Text = m_StockChartX.get_PriceStyleParam(2).ToString();
          lblInputB.Text = "Points or Pct";
          txtInputB.Visible = true;
          lblInputB.Visible = true;
          break;
      }
      ShowDialog();
    }
  }
}
