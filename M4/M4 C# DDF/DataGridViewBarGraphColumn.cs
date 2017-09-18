/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Windows.Forms;

namespace M4
{
  public class DataGridViewBarGraphColumn : DataGridViewColumn
  {
    public DataGridViewBarGraphColumn()
    {
      CellTemplate = new DataGridViewBarGraphCell();
      ReadOnly = true;
    }

    public long MaxValue;

    public void CalcMaxValue()
    {
      int colIndex = DisplayIndex;
      foreach (DataGridViewRow row in DataGridView.Rows)
      {
        MaxValue = (long)Math.Max(MaxValue, Convert.ToDouble(row.Cells[colIndex].Value));
      }
    }
  }
}
