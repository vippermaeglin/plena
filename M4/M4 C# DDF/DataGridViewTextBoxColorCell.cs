/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

//Colors last price grid red or green if up or down tick for n number of ms 

namespace M4
{
  using System;
  using System.IO;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class DataGridViewTextBoxColorColumn : DataGridViewTextBoxColumn
  {


    public DataGridViewTextBoxColorColumn()
    {
      this.CellTemplate = new DataGridViewTextBoxCell();
      this.ReadOnly = true;
    }

    public long MaxValue;

    public void CalcMaxValue()
    {
      int colIndex = this.DisplayIndex;
      for (int rowIndex = 0; rowIndex <= this.DataGridView.Rows.Count - 1; rowIndex++)
      {

        DataGridViewRow row = this.DataGridView.Rows[rowIndex];
        MaxValue = Math.Max(MaxValue, (long)row.Cells[colIndex].Value);
      }
    }
  }


  public class DataGridViewTextBoxColorCell : DataGridViewTextBoxCell
  {

    private Timer m_Timer = null;
    private double m_LastValue = 0;
    private Color m_Color;
    private int m_Interval = 2000;

    public DataGridViewTextBoxColorCell()
    {
      m_Timer = new Timer();
      m_Timer.Interval = 2000;
      m_Timer.Tick += m_Timer_Tick;
    }

    public int Interval
    {
      get { return m_Interval; }
      set { m_Interval = value; }
    }

    protected override bool SetValue(int rowIndex, object value)
    {

      m_Timer.Enabled = false;

      double price = 0;
      Double.TryParse(value.ToString(), 
        System.Globalization.NumberStyles.Any, 
        System.Threading.Thread.CurrentThread.CurrentCulture, out price);

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

      this.Style.BackColor = m_Color;
      this.Style.SelectionBackColor = m_Color;

      m_Timer.Enabled = true;

      m_LastValue = price;
      return base.SetValue(rowIndex, value);

    }

    private void m_Timer_Tick(object sender, System.EventArgs e)
    {
      if (DataGridView == null) return;
      this.Style.BackColor = DataGridView.DefaultCellStyle.BackColor;
      this.Style.SelectionBackColor = DataGridView.DefaultCellStyle.SelectionBackColor;
      m_Timer.Enabled = false;
    }
  }

}