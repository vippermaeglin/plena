/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System.Drawing;
using System.Windows.Forms;

namespace M4
{
  public class DataGridViewImageButtonCell : DataGridViewButtonCell
  {
    public bool Checked { get; set; }
    public Image ImageOn { get; set; }
    public Image ImageOff { get; set; }
    public int OffsetY { get; set; }
    public int OffsetX { get; set; }

    protected override void OnClick(DataGridViewCellEventArgs e)
    {
      Checked = !Checked;
      base.OnClick(e);
    }

    protected override void OnMouseLeave(int rowIndex)
    {
      DataGridView.InvalidateCell(this);
      base.OnMouseLeave(rowIndex);
    }

    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, 
      DataGridViewElementStates elementState, object value, object formattedValue, string errorText, 
      DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, 
      DataGridViewPaintParts paintParts)
    {
      if (ImageOn == null || ImageOff == null) return;

      Image img = Checked ? ImageOn : ImageOff;
      int left = cellBounds.X + cellBounds.Width / 2 - img.Width / 2;

      graphics.FillRectangle(Brushes.Black, cellBounds);

      //Draw cell border
      Color color = DataGridView.DefaultCellStyle.ForeColor;
      if (DataGridView.SelectedRows[0].Index == rowIndex)
        color = DataGridView.RowsDefaultCellStyle.SelectionForeColor;
      Pen p = new Pen(color);
      Pen p2 = new Pen(DataGridView.GridColor);
      graphics.DrawRectangle(p2, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
      graphics.DrawRectangle(p, cellBounds.X, cellBounds.Y, cellBounds.Width - 2, cellBounds.Height - 2);
      p.Dispose();
      p2.Dispose();

      graphics.DrawImage(img, left + OffsetX, cellBounds.Y + OffsetY, img.Width, img.Height);
    }
  }
}
