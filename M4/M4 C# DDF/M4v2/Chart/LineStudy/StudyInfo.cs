using System.ComponentModel;
using System.Drawing;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.LineStudy
{

    public class StudyInfo
    {
        #region Parameters

        [Category("Parameters")]
        public Color ColorParameters { get; set; }

        [Category("Parameters")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("Parameters")]
        public int LineThicknessParameters { get; set; }

        [Category("Parameters")]
        public bool RightExtensionParameters { get; set; }

        [Category("Parameters")]
        public bool LeftExtensionParameters { get; set; }

        [Category("Parameters")]
        public bool ExtensionParameters { get; set; }

        [Category("Parameters")]
        public bool Percent1Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent2Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent3Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent4Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent5Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent6Parameters { get; set; }

        [Category("Parameters")]
        public bool Percent7Parameters { get; set; }

        [Category("Parameters")]
        public double ValuePosition { get; set; }

        #endregion
    }
}
