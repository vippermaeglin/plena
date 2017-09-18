/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

namespace M4
{

  using System;
  using System.IO;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class DataGridViewTickChartColumn : DataGridViewImageColumn
  {

    public DataGridViewTickChartColumn()
    {
      this.CellTemplate = new DataGridViewTickChartCell(DataGridViewTickChartCell.DisplayStyles.XP, "");
      this.ReadOnly = true;
    }

    public long MaxValue;

    public void CalcMaxValue()
    {
      int colIndex = this.DisplayIndex;
      for (int rowIndex = 0; rowIndex <= this.DataGridView.Rows.Count - 1; rowIndex++)
      {

        DataGridViewRow row = this.DataGridView.Rows[rowIndex];
        MaxValue = System.Math.Max(MaxValue, (long)row.Cells[colIndex].Value);
      }
    }
  }


  public class DataGridViewTickChartCell : DataGridViewImageCell
  {


    public enum DisplayStyles
    {
      XP = 0,
      Vista = 1,
      Silver = 2,
      Office = 3
    }

    private Bitmap m_Chart = null;
    private int m_Width = 0;
    private int m_Height = 0;
    private List<double> m_Values = new List<double>();
    private int m_MaxRecords = 50;
    private bool m_Drawing;
    private Rectangle m_CellBounds;
    private string m_Symbol;

    private DisplayStyles m_Style = DisplayStyles.XP;
    private Pen m_PenForeColor = null;
    private Brush m_BrushForeColor = null;
    private Pen m_PenBorder = null;
    private Font m_Font = null;
    private Image m_BackGroundImage = null;

    public DataGridViewTickChartCell(DisplayStyles Style, string Symbol)
    {
      m_Style = Style;
      m_Symbol = Symbol;
      UpdateStyle();
      Value = m_BackGroundImage;
    }

    public DataGridViewTickChartCell()
    {
      m_Style = DisplayStyles.XP;
      UpdateStyle();
      Value = m_BackGroundImage;
    }


    //The maximum number of records to display on the chart 
    public int MaxRecordsOnChart
    {
      get { return m_MaxRecords; }
      set
      {
        if (value >= 3 & value <= 1000)
        {
          m_MaxRecords = value;
        }
        else
        {
          throw new Exception("Invalid value");
        }
      }
    }


    //Built in styles (colors, backgrounds, etc.) 
    public DisplayStyles DispalyStyle
    {
      get { return m_Style; }
      set
      {
        m_Style = value;
        UpdateStyle();
      }
    }


    //Paints the cell 
    protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, System.Windows.Forms.DataGridViewElementStates elementState, object value, object formattedValue, string errorText, System.Windows.Forms.DataGridViewCellStyle cellStyle, System.Windows.Forms.DataGridViewAdvancedBorderStyle advancedBorderStyle,
    System.Windows.Forms.DataGridViewPaintParts paintParts)
    {

      base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
      paintParts);

      if (m_Chart == null | cellBounds.Width != m_Width | cellBounds.Height != m_Height) this.Value = GetTickChart(cellBounds);

      m_CellBounds.Width = cellBounds.Width;
      m_CellBounds.Height = cellBounds.Height;
      m_CellBounds.X  = cellBounds.X;
      m_CellBounds.Y = cellBounds.Y;
      m_Width = cellBounds.Width;
      m_Height = cellBounds.Height;

    }


    //Updates the tick chart with the last price 
    public void UpdateTickChart(double Last)
    {

      bool @add = m_Values.Count == 0;
      if (!@add) @add = m_Values[m_Values.Count - 1] != Last;

      if (@add)
      {

        //Add the new values 
        m_Values.Add(Last);

        //Remove values FILO 
        int range = m_Values.Count - m_MaxRecords;
        if (range > 0) m_Values.RemoveRange(0, range);

        if (!m_Drawing) Value = GetTickChart(m_CellBounds);
      }


    }


    //Creates a tick chart based on the available tick data 
    private Image GetTickChart(Rectangle CellBounds)
    {

      if (m_Drawing)
      {
        m_Drawing = false;
        return m_Chart;
      }

      m_Drawing = true;

      //Create a new bitmap if it doesn't exist 
      if (CellBounds.Width == 0) return m_Chart;
      if ((m_Chart != null)) m_Chart.Dispose();
      m_Chart = new Bitmap(CellBounds.Width, CellBounds.Height);

      //Create a graphics to draw on 
      Graphics gr = Graphics.FromImage(m_Chart);

      //Draw the gradient background image       
      Bitmap backImg = new Bitmap(ResizeImage(m_BackGroundImage, CellBounds.Width, CellBounds.Height));

      gr.DrawImage(backImg, 0, 0, backImg.Width, backImg.Height);

      //Set drawing to antialiased mode 
      gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

      //Draw the chart border 
      gr.DrawRectangle(m_PenBorder, 0, 0, CellBounds.Width - 1, CellBounds.Height - 1);

      //Draw some text while collecting data to build the chart 
      if (m_Values.Count < 3)
      {
        int top = (CellBounds.Height / 2) - (m_Font.Height / 2);
        gr.DrawString("Creating Chart...", m_Font, m_BrushForeColor, 5, top);
        m_Drawing = false;
        return m_Chart;
      }

      //Draw the chart 
      double max = m_Values[0];
      double min = max;
      for (int n = 0; n <= m_Values.Count - 1; n++)
      {
        max = Math.Max(m_Values[n], max);
        min = Math.Min(m_Values[n], min);
      }

      int x = 0;
      int y = 0;
      int prevX = 0;
      int prevY = -1;
      for (int n = 0; n <= m_Values.Count - 1; n++)
      {
        x = n * ((CellBounds.Width - 20) / m_Values.Count);
        y = (int)((((double)CellBounds.Height - 10) * Normalize(m_Values[n], max, min)) + 5);
        if (prevY == -1) prevY = y;
        gr.DrawLine(m_PenForeColor, x, y, prevX, prevY);
        prevX = x;
        prevY = y;
      }

      m_Drawing = false;
      return m_Chart;

    }


    //Changes the visual style 
    private void UpdateStyle()
    {

      if ((m_PenForeColor != null)) m_PenForeColor.Dispose();
      if ((m_PenBorder != null)) m_PenBorder.Dispose();
      if ((m_Font != null)) m_Font.Dispose();
      if ((m_BackGroundImage != null)) m_BackGroundImage.Dispose();

      System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      Stream myStream = null;
 
      Color foreColor = System.Drawing.Color.White;
      switch (m_Style)
      {
        case DisplayStyles.XP:
          foreColor = System.Drawing.Color.Lime;
          m_BrushForeColor = new SolidBrush(Color.Lime);
          myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_xp.bmp");          
          break;
        case DisplayStyles.Vista:
          foreColor = System.Drawing.Color.White;
          m_BrushForeColor = new SolidBrush(Color.White);
          myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_vista.bmp");
          break;
        case DisplayStyles.Silver:
          foreColor = System.Drawing.Color.White;
          m_BrushForeColor = new SolidBrush(Color.Black);
          myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_silver.bmp");
          break;
        case DisplayStyles.Office:
          foreColor = System.Drawing.Color.SteelBlue;
          m_BrushForeColor = new SolidBrush(Color.Black);
          myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_office.bmp");
          break;
        default:
          foreColor = System.Drawing.Color.Lime;
          m_BrushForeColor = new SolidBrush(Color.Black);
          myStream = myAssembly.GetManifestResourceStream("M4.Images.gradient_xp.bmp");
          break;
      }

      m_BackGroundImage = new Bitmap(myStream);
      m_PenForeColor = new Pen(foreColor);
      m_PenBorder = new Pen(foreColor, 2);

      m_Font = new Font("Arial", 8, GraphicsUnit.Point);

      if ((DataGridView != null)) this.Value = GetTickChart(m_CellBounds);

    }


    //Resizes an image 
    private Image ResizeImage(Image originalImage, int width, int height)
    {

      if (width < 1) width = 1;
      if (height < 1) height = 1;

      Image finalImage = new Bitmap(width, height);
      Graphics graphic = Graphics.FromImage(finalImage);

      graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
      graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

      Rectangle rectangle = new Rectangle(0, 0, width, height);

      graphic.DrawImage(originalImage, rectangle);

      return finalImage;

    }


    //Normalizes values for the chart 
    private double Normalize(double Value, double Max, double Min)
    {
      if ((Max - Min) > 0)
      {
        return (Value - Min) / (Max - Min);
      }
      else
      {
        return Value;
      }
    }


    //Overrides dispose to clean up the drawing objects 
    protected override void Dispose(bool disposing)
    {
      if ((m_PenForeColor != null)) m_PenForeColor.Dispose();
      if ((m_PenBorder != null)) m_PenBorder.Dispose();
      if ((m_Font != null)) m_Font.Dispose();
      if ((m_BackGroundImage != null)) m_BackGroundImage.Dispose();
      base.Dispose(disposing);
    }
  }
}