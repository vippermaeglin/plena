/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace M4
{
  public partial class ctlForexPair : UserControl
  {
    private double m_LastAsk;
    private double m_LastBid; 

    public ctlForexPair(string PairName)
    {
      InitializeComponent();

      lblPairName.Text = PairName;
        
        lblBidSize.Parent = picSell;
        lblBidA.Parent = picSell;
        lblBidB.Parent = picSell;
        picLeft.Parent = picSell;
        lblBidA.BackColor = Color.Transparent;
        lblBidB.BackColor = Color.Transparent;
        picLeft.BackColor = Color.Transparent;
        lblBidSize.BackColor = Color.Transparent;

        lblAskSize.Parent = picBuy;
        lblAskA.Parent = picBuy;
        lblAskB.Parent = picBuy;
        picRight.Parent = picBuy;
        lblAskA.BackColor = Color.Transparent;
        lblAskB.BackColor = Color.Transparent;
        picRight.BackColor = Color.Transparent;
        lblAskSize.BackColor = Color.Transparent;

        picBuy.Image = ImageList2.Images["gray"];
        picSell.Image = ImageList2.Images["gray"];

    }

    private void ctlForexPair_Resize(object sender, EventArgs e)
    {
      Width = 281;
      Height = 112;
    }

    public double GetLastBidValue()
    {
      return m_LastBid;
    }

    public double GetLastAskValue()
    {
      return m_LastAsk;
    }

    public int GetLastBidSize()
    {
      return Convert.ToInt32(lblBidSize.Text);
    }

    public int GetLastAskSize()
    {
      return Convert.ToInt32(lblAskSize.Text);
    }

    //Updates the bid side. Automatically calculates change and low.
    public void UpdateBid(double Value, long BidSize)
    {
      //Calculate the change
      Value = Math.Round(Value, 4);
      double change = 0.0;
      if (m_LastBid > 0.0)
      {
        change = Value - m_LastBid;
      }
      m_LastBid = Value;

      //Update color and arrow
      if (change == 0.0)
      {
        lblBidB.ForeColor = Color.RoyalBlue;
        picLeft.Image = null;
        picBuy.Image = ImageList2.Images["gray"];
      }
      else if (change > 0.0)
      {
        lblBidB.ForeColor = Color.Lime;
        picLeft.Image = ImageList1.Images[0];
        picBuy.Image = ImageList2.Images["blue"];
      }
      else if (change < 0.0)
      {
        lblBidB.ForeColor = Color.White;
        picLeft.Image = ImageList1.Images[1];
        picBuy.Image = ImageList2.Images["red"];
      }

      //Get last two chars of value        
      string strVal = Convert.ToString(Value);
      if (strVal.Length < 6)
      {
        strVal = strVal + "0";
      }
      string lastTwo = strVal.Substring(strVal.Length - 2);
      string first = strVal.Substring(0, strVal.Length - 2);
      lblBidA.Text = first;
      lblBidB.Text = lastTwo;
      lblBidSize.Text = BidSize.ToString();
    }

    //Updates the Ask side. Automatically calculates change and low.
    public void UpdateAsk(double Value, long AskSize)
    {
      //Calculate the change
      Value = Math.Round(Value, 4);
      double change = 0.0;
      if (m_LastAsk > 0.0)
      {
        change = Value - m_LastAsk;
      }
      m_LastAsk = Value;

      //Update color and arrow
      if (change == 0.0)
      {
        picSell.Image = ImageList2.Images["gray"];
        lblAskB.ForeColor = Color.RoyalBlue;
        picRight.Image = null;
      }
      else if (change > 0.0)
      {
        picSell.Image = ImageList2.Images["blue"];
        lblAskB.ForeColor = Color.Lime;
        picRight.Image = ImageList1.Images[0];
      }
      else if (change < 0.0)
      {
        picSell.Image = ImageList2.Images["blue"];
        lblAskB.ForeColor = Color.White;
        picRight.Image = ImageList1.Images[1];
      }

      //Get last two chars of value
      string strVal = Convert.ToString(Value);
      if (strVal.Length < 6)
      {
        strVal = strVal + "0";
      }
      string lastTwo = strVal.Substring(strVal.Length - 2);
      string first = strVal.Substring(0, strVal.Length - 2);
      lblAskA.Text = first;
      lblAskB.Text = lastTwo;
      lblAskSize.Text = AskSize.ToString();
    }
  }
}
