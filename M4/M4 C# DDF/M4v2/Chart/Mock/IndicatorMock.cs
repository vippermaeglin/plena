using System.Drawing;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.Mock
{
    public class IndicatorMock : Indicator
    {
        public string CodeMock { get; set; }

        public int Periods { get; set; }

        public int PeriodsParameters { get; set; }

        public int KPeriodsParameters { get; set; }

        public int DPeriodsParameters { get; set; }

        public int ShortCycle { get; set; }

        public int LongCycle { get; set; }

        public int LineThicknessValue { get; set; }

        public int LineThicknessAverage { get; set; }

        public int LineThickness2Parameters { get; set; }

        public int LineThicknessParameters { get; set; }

        public int LineThicknessT { get; set; }

        public double LimitMoveValue { get; set; }

        public int ShortTermParameters { get; set; }

        public int LongTermParameters { get; set; }

        public int Cycle1 { get; set; }

        public int Cycle2 { get; set; }

        public int Cycle3 { get; set; }

        public int RatOfChg { get; set; }

        public int Levels { get; set; }

        public int StandardDev { get; set; }

        public int ShiftParameters { get; set; }

        public int BarHistoryParameters { get; set; }

        public int KSlowingParameters { get; set; }

        public int KDblSmooth { get; set; }

        public int Smooth { get; set; }

        public double ScaleAverage { get; set; }

        public double MinAf { get; set; }

        public double MaxAf { get; set; }

        public Color ColorHistogram { get; set; }

        public Color ColorValue { get; set; }

        public Color ColorAverage { get; set; }

        public Color ColorParameters { get; set; }

        public Color Color2Parameters { get; set; }

        public Color ColorT { get; set; }

        public Enums.LineStyle LineStyleAverage { get; set; }

        public Enums.LineStyle LineStyleValue { get; set; }

        public Enums.LineStyle LineStyleParameters { get; set; }

        public Enums.LineStyle LineStyle2Parameters { get; set; }

        public Enums.LineStyle LineStyleT { get; set; }

        public Enums.Source SourceAverage { get; set; }

        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        public Enums.Source SourceParameters { get; set; }

        public Enums.Source SourceComparativeParameters { get; set; }

        public Enums.Source VolumeParameters { get; set; }

        public Enums.SourceOHLC SourceOHLC { get; set; }

        public Enums.SourceVolume SourceVolume { get; set; }

        public Enums.PointsPercent PointsPercentsParameters { get; set; }

        public Enums.Type Type { get; set; }

        public Enums.Type DType { get; set; }

        public double Threshold1 { get; set; }

        public double Threshold2 { get; set; }
    }
}
