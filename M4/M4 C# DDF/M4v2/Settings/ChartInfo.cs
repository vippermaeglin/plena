using System.ComponentModel;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Settings
{
    /// <summary>
    /// Para ordenação dos grupos é necessário ordenar as categorias. 
    /// Ex.: Código (CA = Categoria A) + Nome do Grupo
    /// </summary>
    public class ChartInfo
    {
        #region Behavior

        [Category("CA-Behavior")]
        public bool SemiLogScale { get; set; }

        [Category("CA-Behavior")]
        public bool PanelSeparator { get; set; }

        [Category("CA-Behavior")]
        public bool GridVertical { get; set; }

        [Category("CA-Behavior")]
        public bool GridHorizontal { get; set; }

        [Category("CA-Behavior")]
        public int Decimals { get; set; }

        [Category("CA-Behavior")]
        public bool VisiblePortfolio { get; set; }

        #endregion

        #region Appearance

        [Category("CB-Appearance")]
        public int PaddingTop { get; set; }

        [Category("CB-Appearance")]
        public int PaddingBottom { get; set; }

        [Category("CB-Appearance")]
        public int PaddingRight { get; set; }

        [Category("CB-Appearance")]
        public string ColorSchemes { get; set; }

        #endregion

        #region NumbeCandles

        [Category("CC-NumberCandles")]
        public int ChartViewport { get; set; }

        [Category("CC-NumberCandles")]
        public int History { get; set; }

        [Category("CC-NumberCandles")]
        public string Periodicity { get; set; }
		
        #endregion

        #region TabData

        [Category("CD-TabData")]
        public string Position { get; set; }

        #endregion
    }

    public class ItemChart
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public ItemChart(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
