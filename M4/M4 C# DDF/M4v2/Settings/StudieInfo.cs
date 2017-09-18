using System.ComponentModel;
using System.Drawing;

namespace M4.M4v2.Settings
{
    /// <summary>
    /// Para ordenação dos grupos é necessário ordenar as categorias. 
    /// Ex.: Código (CA = Categoria A) + Nome do Grupo
    /// </summary>
    public class StudieInfo
    {
        #region Appearance

        [Category("CA-Appearance")]
        public decimal LineThickness { get; set; }

        [Category("CA-Appearance")]
        public Color Color { get; set; }

        #endregion

        #region Fibonacci Retraction

        [Category("CB-Retraction")]
        public bool Retraction0 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction38 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction50 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction61 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction100 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction161 { get; set; }

        [Category("CB-Retraction")]
        public bool Retraction261 { get; set; }

        #endregion

        #region Fibonacci Projection

        [Category("CC-Projection")]
        public bool Projection0 { get; set; }

        [Category("CC-Projection")]
        public bool Projection38 { get; set; }

        [Category("CC-Projection")]
        public bool Projection50 { get; set; }

        [Category("CC-Projection")]
        public bool Projection61 { get; set; }

        [Category("CC-Projection")]
        public bool Projection100 { get; set; }

        [Category("CC-Projection")]
        public bool Projection161 { get; set; }

        [Category("CC-Projection")]
        public bool Projection261 { get; set; }

        #endregion
    }
}
