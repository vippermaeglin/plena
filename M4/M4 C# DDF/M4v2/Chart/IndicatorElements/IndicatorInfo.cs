using System.ComponentModel;
using System.Drawing;

namespace M4.M4v2.Chart.IndicatorElements
{

    public class IndicatorInfo
    {
        #region GENERAL
           
            #region Average

            [Category("Average")]
            public int Periods { get; set; }

            [Category("Average")]
            public Color ColorAverage { get; set; }

            [Category("Average")]
            public Enums.LineStyle LineStyleAverage { get; set; }

            [Category("Average")]
            public int LineThicknessAverage { get; set; }

            [Category("Average")]
            public Enums.Source SourceAverage { get; set; }

            [Category("Average")]
            public double ScaleAverage { get; set; }
        
            #endregion

            #region Histogram

            [Category("Histogram")]
            public Color ColorHistogram { get; set; }

            #endregion

            #region Value

            [Category("Value")]
            public int ShortCycle { get; set; }

            [Category("Value")]
            public int LongCycle { get; set; }

            [Category("Value")]
            public Color ColorValue { get; set; }

            [Category("Value")]
            public Enums.LineStyle LineStyleValue { get; set; }

            [Category("Value")]
            public int LineThicknessValue { get; set; }

            #endregion

            #region Window

            [Category("Window")]
            public Enums.Window Window { get; set; }

            #endregion

            #region Parameters

            [Category("Parameters")]
            public int PeriodsParameters { get; set; }

            [Category("Parameters")]
            public Color ColorParameters { get; set; }

            [Category("Parameters")]
            public Enums.LineStyle LineStyleParameters { get; set; }

            [Category("Parameters")]
            public int LineThicknessParameters { get; set; }

            [Category("Parameters")]
            public Enums.Source SourceParameters { get; set; }

            [Category("Parameters")]
            public Enums.Source VolumeParameters { get; set; }

            [Category("Parameters")]
            public int ShortTermParameters { get; set; }

            [Category("Parameters")]
            public int LongTermParameters { get; set; }

            [Category("Parameters")]
            public Enums.PointsPercent PointsPercentsParameters { get; set; }

            [Category("Parameters")]
            public int Cycle1 { get; set; }

            [Category("Parameters")]
            public int Cycle2 { get; set; }

            [Category("Parameters")]
            public int Cycle3 { get; set; }

            [Category("Parameters")]
            public double StandardDev { get; set; }

            [Category("Parameters")]
            public Enums.Type Type { get; set; }

            [Category("Parameters")]
            public Enums.Source SourceComparativeParameters { get; set; }

            [Category("Parameters")]
            public int Levels { get; set; }

            [Category("Parameters")]
            public double MinAf { get; set; }

            [Category("Parameters")]
            public double MaxAf { get; set; }

            [Category("Parameters")]
            public int RatOfChg { get; set; }

            [Category("Parameters")]
            public double ShiftParameters { get; set; }

            [Category("Parameters")]
            public int BarHistoryParameters { get; set; }

            [Category("Parameters")]
            public int KPeriodsParameters { get; set; }

            [Category("Parameters")]
            public int KSlowingParameters { get; set; }

            [Category("Parameters")]
            public int DPeriodsParameters { get; set; }

            [Category("Parameters")]
            public Enums.Type DType { get; set; }

            [Category("Parameters")]
            public int KDblSmooth { get; set; }
        
            [Category("Parameters")]
            public Color Color2Parameters { get; set; }

            [Category("Parameters")]
            public Enums.LineStyle LineStyle2Parameters { get; set; }

            [Category("Parameters")]
            public int LineThickness2Parameters { get; set; } 

            #endregion
        
        #endregion

    }

    public class MACDInfo
    {
        #region Average

        [Category("Average")]
        public int Periods { get; set; }

        [Category("Average")]
        public Color ColorAverage { get; set; }

        [Category("Average")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("Average")]
        public int LineThicknessAverage { get; set; }
        
        #endregion

        #region Value

        [Category("Value")]
        public int ShortCycle { get; set; }

        [Category("Value")]
        public int LongCycle { get; set; }

        [Category("Value")]
        public Color ColorValue { get; set; }

        [Category("Value")]
        public Enums.LineStyle LineStyleValue { get; set; }

        [Category("Value")]
        public int LineThicknessValue { get; set; }

        #endregion
 
    }

    public class MACDHInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public int ShortCycle { get; set; }

        [Category("Parameters")]
        public int LongCycle { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorHistogram { get; set; }

        #endregion

    }

    public class MAInfo
    {
       
        #region Parameters

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        [Category("Parameters")]
        public int Periods { get; set; }
        
        [Category("Parameters")]
        public Enums.Source SourceAverage { get; set; }

        [Category("Parameters")]
        public int ShiftParameters { get; set; }

        [Category("Parameters")]
        public double ScaleAverage { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorAverage { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("View")]
        public int LineThicknessAverage { get; set; }

        #endregion
    }

    public class MAEInfo
    {

        #region Parameters

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        [Category("Parameters")]
        public double Percentage { get; set; }
        
        #endregion

        #region View

        [Category("View")]
        public Color ColorAverage { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("View")]
        public int LineThicknessAverage { get; set; }

        #endregion
    }

    public class BBInfo
    {

        #region Parameters

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        [Category("Parameters")]
        public double StandardDev { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorAverage { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("View")]
        public int LineThicknessAverage { get; set; }

        #endregion
    }

    public class HVInfo
    {

        #region Parameters

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        [Category("Parameters")]
        public double StandardDev { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorAverage { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("View")]
        public int LineThicknessAverage { get; set; }

        #endregion
    }

    public class RSIInfo
    {

        #region Parameters
        
        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceAverage { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorAverage { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("View")]
        public int LineThicknessAverage { get; set; }

        #endregion

        #region Thresholds

        [Category("ZThresholds")]
        public double Threshold1 { get; set; }

        [Category("ZThresholds")]
        public double Threshold2 { get; set; }

        [Category("ZThresholds")]
        public Color ColorT { get; set; }

        [Category("ZThresholds")]
        public Enums.LineStyle LineStyleT { get; set; }

        [Category("ZThresholds")]
        public int LineThicknessT { get; set; }

        #endregion

    }

    public class VolumeInfo
    {
        
        #region Parameters

        [Category("Parameters")]
        public Enums.SourceVolume  SourceParameters { get; set; }
        
        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        #endregion
    }

    public class HILOInfo
    {

        #region Parameters
        
        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public int PeriodsParameters { get; set; }

        [Category("Parameters")]
        public int ShiftParameters { get; set; }

        [Category("Parameters")]
        public double ScaleAverage { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Color Color2Parameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }

        #endregion
    }

	public class PSARInfo
    {

        #region Parameters

        [Category("Parameters")]
        public double MinAF { get; set; }

        [Category("Parameters")]
        public double MaxAF { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }

        #endregion
    }

    public class TPInfo
    {
        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }
        
        #endregion
    }

    public class SMIInfo
    {

        #region %K Parameters

        [Category("%K Parameters")]
        public int KPeriods { get; set; }

        [Category("%K Parameters")]
        public int Smooth { get; set; }

        [Category("%K Parameters")]
        public int KDblSmooth { get; set; }

        [Category("%K Parameters")]
        public Enums.Type Type { get; set; }

        [Category("%K Parameters")]
        public Color ColorAverage { get; set; }

        [Category("%K Parameters")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("%K Parameters")]
        public int LineThicknessAverage { get; set; }
        
        #endregion

        #region %D Parameters

        [Category("%D Parameters")]
        public int DPeriods { get; set; }

        [Category("%D Parameters")]
        public Enums.Type DType { get; set; }

        [Category("%D Parameters")]
        public Color ColorValue { get; set; }

        [Category("%D Parameters")]
        public Enums.LineStyle LineStyleValue { get; set; }

        [Category("%D Parameters")]
        public int LineThicknessValue { get; set; }

        #endregion
    }
    
    public class SOInfo
    {
        #region %K Parameters

        [Category("%K Parameters")]
        public int KPeriods { get; set; }

        [Category("%K Parameters")]
        public int KSlowingParameters { get; set; }

        [Category("%K Parameters")]
        public Color ColorValue { get; set; }

        [Category("%K Parameters")]
        public Enums.LineStyle LineStyleValue{ get; set; }

        [Category("%K Parameters")]
        public int LineThicknessValue { get; set; }

        #endregion

        #region %D Parameters

        [Category("%D Parameters")]
        public int DPeriods { get; set; }

        [Category("%D Parameters")]
        public Enums.Type Type { get; set; }

        [Category("%D Parameters")]
        public Color ColorAverage { get; set; }

        [Category("%D Parameters")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("%D Parameters")]
        public int LineThicknessAverage { get; set; }

        #endregion
        
    }

    public class AroonInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }
        

        #endregion

        #region Aroon Down

        [Category("Aroon Down")]
        public Color Color2Parameters { get; set; }

        [Category("Aroon Down")]
        public Enums.LineStyle LineStyle2Parameters { get; set; }

        [Category("Aroon Down")]
        public int LineThickness2Parameters { get; set; }

        #endregion
    }

    public class AOInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        #endregion
    }

    public class CMFInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }
        
        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }
        #endregion
    }

    public class EOMInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class MFIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class ASIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public double LimitMoveValue { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class CVInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public int RatOfChg { get; set; }

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class CMOInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class WWSInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class VHFInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class TRIXInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class LRRSInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class LRFInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class LRSInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class MOInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Source SourceParameters { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class ADXInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion

    }

    public class DIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region DI+

        [Category("DI+")]
        public Color ColorParameters { get; set; }

        [Category("DI+")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("DI+")]
        public int LineThicknessParameters { get; set; }


        #endregion

        #region DI-

        [Category("DI-")]
        public Color Color2Parameters { get; set; }

        [Category("DI-")]
        public Enums.LineStyle LineStyle2Parameters { get; set; }

        [Category("DI-")]
        public int LineThickness2Parameters { get; set; }


        #endregion

    }

    public class ADXDIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region DI+

        [Category("DI+")]
        public Color ColorParameters { get; set; }
            
        [Category("DI+")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("DI+")]
        public int LineThicknessParameters { get; set; }

#endregion

        #region DI-

        [Category("DI-")]
        public Color Color2Parameters { get; set; }

        [Category("DI-")]
        public Enums.LineStyle LineStyle2Parameters { get; set; }

        [Category("DI-")]
        public int LineThickness2Parameters { get; set; }
        #endregion

        #region ADX

        [Category("ADX")]
        public Color ColorAverage { get; set; }

        [Category("ADX")]
        public Enums.LineStyle LineStyleAverage { get; set; }

        [Category("ADX")]
        public int LineThicknessAverage { get; set; }

        #endregion
    }

    public class CCIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class DPOInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class MIInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }
        
        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class OBVInfo
    {
        #region Parameters
        
        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class PROCInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class PVTInfo
    {
        #region Parameters

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class WilliamsRInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class WADInfo
    {
        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class WCInfo
    {
        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class VROCInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }
    
    public class FCOInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class FCBInfo
    {
        #region Parameters

        [Category("Parameters")]
        public int Periods { get; set; }

        #endregion

        #region view

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion

    }

    public class  UOInfo
    {        
        
        #region Parameters

        [Category("Parameters")]
        public int Cycle1 { get; set; }

        [Category("Parameters")]
        public int Cycle2 { get; set; }

        [Category("Parameters")]
        public int Cycle3 { get; set; }

        #endregion

        #region view

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }

        #endregion


    }

    public class TRInfo
    {
        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class ROInfo
    {

        #region Parameters


        [Category("Parameters")]
        public int Levels { get; set; }

        [Category("Parameters")]
        public Enums.SourceOHLC SourceAverageOHLC { get; set; }

        [Category("Parameters")]
        public Enums.Type Type { get; set; }

        #endregion

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }

    public class ADInfo
    {

        #region View

        [Category("View")]
        public Color ColorParameters { get; set; }

        [Category("View")]
        public Enums.LineStyle LineStyleParameters { get; set; }

        [Category("View")]
        public int LineThicknessParameters { get; set; }


        #endregion
    }


}
