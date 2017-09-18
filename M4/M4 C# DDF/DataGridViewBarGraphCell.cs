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
  public class DataGridViewBarGraphCell : DataGridViewTextBoxCell
  {
    protected override void Paint(Graphics graphics, Rectangle clipBounds, 
      Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, 
      object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, 
      DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
      base.Paint(graphics, clipBounds, cellBounds,
                 rowIndex, cellState,
                 value, "", errorText, cellStyle,
                 advancedBorderStyle, paintParts);

      //Get the value of the cell
      int cellValue = Utils.IsDBNull(value) ? 0 : Format.FromLocalInteger(value);

      //If cell value is 0, you still
      //want to show something, so set the value to 1
      if (cellValue == 0)
      {
        cellValue = 1;
      }

      DataGridViewBarGraphColumn parent = (DataGridViewBarGraphColumn)OwningColumn;
      parent.CalcMaxValue();
      long maxValue = parent.MaxValue;
      Font fnt = parent.InheritedStyle.Font;

      int availableWidth = cellBounds.Width;

      if (maxValue != 0)
        cellValue = (int) ((cellValue / maxValue) * availableWidth);

      //Draw the bar, truncating to fit in the space 
      //you've got in the cell:
      const int HORIZONTALOFFSET = 1;

    //  Image img = ctlScanner.GradientBar((int)cellValue, cellBounds.Height - 3);
     // graphics.DrawImage(img, cellBounds.X + HORIZONTALOFFSET, cellBounds.Y + 1, img.Width, img.Height);

      //Get the text to draw and calculate its width:
      string cellText = formattedValue.ToString();
      SizeF textSize = graphics.MeasureString(cellText, fnt);

      PointF textStart = new PointF(1, (cellBounds.Height - textSize.Height) / 2);

      //Calculate the correct color:
      Color textColor = parent.InheritedStyle.ForeColor;
      if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
      {
        textColor = Color.Yellow; // textColor = parent.InheritedStyle.SelectionForeColor; // Optional
      }

      //Draw the text:
      using (SolidBrush brush = new SolidBrush(textColor))
      {
        graphics.DrawString(cellText, fnt, brush, 
                            cellBounds.X + cellBounds.Width - textSize.Width,
                            cellBounds.Y + textStart.Y);
      }
    }
  }
}
