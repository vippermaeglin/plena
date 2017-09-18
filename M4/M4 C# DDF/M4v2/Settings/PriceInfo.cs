using System.ComponentModel;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Settings
{
    /// <summary>
    /// Para ordenação dos grupos é necessário ordenar as categorias. 
    /// Ex.: Código (CA = Categoria A) + Nome do Grupo
    /// </summary>
    public class PriceInfo
    {
        #region Line

        //[Category("CA-Line")]
        //public bool LineMono { get; set; }

        [Category("CA-Line")]
        public int LineThickness { get; set; }
        
        #endregion

        #region Bar

        [Category("CB-Bar")]
        public int BarLineThickness { get; set; }

        #endregion

        #region Smoothed

        [Category("CC-Smoothed")]
        public int Period { get; set; }

        [Category("CC-Smoothed")]
        public Enums.TypeHeikin TipoMedia { get; set; }

        #endregion
    }
}
