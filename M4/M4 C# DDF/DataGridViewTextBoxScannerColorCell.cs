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
  public class DataGridViewTextBoxScannerColorCell : DataGridViewTextBoxCell
  {
    private readonly Timer m_Timer;
    private double m_LastValue;
    private Color m_Color;
    
    public int Interval { get; set; }
    public bool HighlightOnly { get; set; }

    public DataGridViewTextBoxScannerColorCell()
    {
      Interval = 2000;
      m_Timer = new Timer {Interval = Interval};
      m_Timer.Tick += MTimerOnTick;
    }

    protected override bool SetValue(int rowIndex, object value)
    {
      m_Timer.Enabled = false;

      if (HighlightOnly)
      {
        if (value != Value)
        {
          Style.BackColor = Color.Yellow;
          Style.SelectionBackColor = Color.Yellow;
          Style.SelectionForeColor = Color.Black;
          Style.ForeColor = Color.Black;
          m_Timer.Interval = Interval;
          m_Timer.Enabled = true;
        }
      }
      else
      {
        double price = Convert.ToDouble(value);

        if (price > m_LastValue)
        {
          m_Color = Color.Lime;
        }
        else if (price < m_LastValue)
        {
          m_Color = Color.Red;
        }
        else
        {
          m_Color = Color.Silver;
        }

        Style.BackColor = m_Color;
        Style.SelectionBackColor = m_Color;

        m_Timer.Interval = Interval;
        m_Timer.Enabled = true;

        m_LastValue = price;
      }

      return base.SetValue(rowIndex, value);
    }

    private void MTimerOnTick(object sender, EventArgs eventArgs)
    {
      if (DataGridView == null) return;
      Style.BackColor = DataGridView.DefaultCellStyle.BackColor;
      Style.SelectionBackColor = DataGridView.RowsDefaultCellStyle.SelectionBackColor;
      Style.SelectionForeColor = DataGridView.DefaultCellStyle.ForeColor;
      Style.ForeColor = DataGridView.DefaultCellStyle.ForeColor;
      m_Timer.Enabled = false;
    }
  }
}
