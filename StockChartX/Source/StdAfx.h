#if !defined(AFX_STDAFX_H__D44A9FBD_3153_4119_B1D3_422A3503595A__INCLUDED_)
#define AFX_STDAFX_H__D44A9FBD_3153_4119_B1D3_422A3503595A__INCLUDED_

// StockChartX PRO Version

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#if !defined(WINVER)
#define WINVER 0x0501
#endif

#pragma warning (disable:4222) // .NET ordinal error
#pragma warning (disable:4089)
#pragma warning (disable:4244)
#pragma warning (disable:4018)
#pragma warning (disable:4996)

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#define _AFXDLL

#include <afxctl.h>         // MFC support for ActiveX Controls

#include "memdc.h"
#include <vector>
#include <deque>
#include <math.h>

#include "XML.h"
#include "ChartPanel.h"
#include "Series.h"
#include "SeriesStock.h"
#include "SeriesStandard.h"
#include "LineStandard.h"
#include "LineStudyEllipse.h"
#include "LineStudyRectangle.h"
#include "LineStudyTriangle.h"
#include "LineStudyPolyline.h"
#include "LineStudySpeedLines.h"
#include "LineStudyGannFan.h"
#include "LineStudyFibonacciArcs.h"
#include "LineStudyFibonacciFan.h"
#include "LineStudyFibonacciRetracements.h"
#include "LineStudyFibonacciProgression.h"
#include "LineStudyFibonacciTimeZones.h"
#include "LineStudyTironeLevels.h"
#include "LineStudyQuadrantLines.h"
#include "LineStudyRaffRegression.h"
#include "LineStudyErrorChannel.h"
#include "LineStudyFreehand.h"
#include "TextArea.h"
#include "SymbolObject.h"
#include "PriceStyle.h"
#include "PriceStylePointAndFigure.h"
#include "PriceStyleEquiVolume.h"
#include "PriceStyleThreeLineBreak.h"
#include "PriceStyleRenko.h"
#include "PriceStyleKagi.h"
#include "PriceStyleHeikinAshi.h"

//New Class
#include "LineStudyChannel.h"

// TA-SDK Indicator Wrappers

// CBands
#include "IndicatorBollingerBands.h"
#include "IndicatorHighLowBands.h"
#include "IndicatorMovingAverageEnvelope.h"
#include "IndicatorFractalChaosBands.h"
#include "IndicatorPrimeNumberBands.h"
#include "IndicatorHILOActivator.h"

// CGeneral
#include "IndicatorHighMinusLow.h"
#include "IndicatorMedian.h"
#include "IndicatorPriceROC.h"
#include "IndicatorStandardDeviation.h"
#include "IndicatorTypicalPrice.h"
#include "IndicatorVolumeROC.h"
#include "IndicatorWeightedClose.h"
#include "IndicatorVolume.h"

// CIndex
#include "IndicatorAccumulationDistribution.h"
#include "IndicatorAccumulativeSwingIndex.h"
#include "IndicatorChaikinMoneyFlow.h"
#include "IndicatorCommodityChannelIndex.h"
#include "IndicatorComparativeRelativeStrength.h"
#include "IndicatorMassIndex.h"
#include "IndicatorMoneyFlowIndex.h"
#include "IndicatorNegativeVolumeIndex.h"
#include "IndicatorOnBalanceVolume.h"
#include "IndicatorPerformanceIndex.h"
#include "IndicatorPositiveVolumeIndex.h"
#include "IndicatorPriceVolumeTrend.h"
#include "IndicatorRelativeStrengthIndex.h"
#include "IndicatorSwingIndex.h"
#include "IndicatorTradeVolumeIndex.h"
#include "IndicatorStochasticMomentumIndex.h"

// CLinearRegression
#include "IndicatorLinearRegressionRSquared.h"
#include "IndicatorLinearRegressionForecast.h"
#include "IndicatorLinearRegressionSlope.h"
#include "IndicatorLinearRegressionIntercept.h"

// COscillator
#include "IndicatorADX.h"
#include "IndicatorAroon.h"
#include "IndicatorAroonOscillator.h"
#include "IndicatorChaikinVolatility.h"
#include "IndicatorChandeMomentumOscillator.h"
#include "IndicatorDetrendedPriceOscillator.h"
#include "IndicatorDirectionalMovementSystem.h"
#include "IndicatorDI.h"
#include "IndicatorEaseOfMovement.h"
#include "IndicatorMACD.h"
#include "IndicatorMACDHistogram.h"
#include "IndicatorMomentum.h"
#include "IndicatorParabolicSAR.h"
#include "IndicatorPriceOscillator.h"
#include "IndicatorRainbowOscillator.h"
#include "IndicatorStochasticOscillator.h"
#include "IndicatorTRIX.h"
#include "IndicatorTrueRange.h"
#include "IndicatorUltimateOscillator.h"
#include "IndicatorVerticalHorizontalFilter.h"
#include "IndicatorVolumeOscillator.h"
#include "IndicatorWilliamsAccumulationDistribution.h"
#include "IndicatorWilliamsPctR.h"
#include "IndicatorFractalChaosOscillator.h"
#include "IndicatorPrimeNumberOscillator.h"
#include "IndicatorHistoricalVolatility.h"

// CMovingAverage
#include "IndicatorExponentialMovingAverage.h"
#include "IndicatorSimpleMovingAverage.h"
#include "IndicatorTimeSeriesMovingAverage.h"
#include "IndicatorTriangularMovingAverage.h"
#include "IndicatorVariableMovingAverage.h"
#include "IndicatorVIDYA.h"
#include "IndicatorWeightedMovingAverage.h"
#include "IndicatorWellesWilderSmoothing.h"
#include "IndicatorGenericMovingAverage.h"

#include "PointF.h"

//Custom Indicators
#include "IndicatorCustom.h"


#define MAX_VISIBLE			5000 // Pixels
#define MAX_PARAMS			101 // Maximum number of parameters
#define MAX_PANELS			25 // Maximum number of chart panels
#define CALENDAR_HEIGHT		20 // Bottom calendar panel height

/* see eLineStudy
#define DRAWING_TRENDLINE	1
#define DRAWING_SPEEDLINES	2
#define DRAWING_GANNFAN		3
#define DRAWING_GANGRID		4
#define DRAWING_PITCHFORK	5
#define DRAWING_FIBFAN		6
#define DRAWING_FIBRETRACE	7
#define DRAWING_FIBZONES	8
#define DRAWING_QUADLINES	9
#define DRAWING_TIRONELEVEL	10
#define DRAWING_RECTANGLE	11
#define DRAWING_ARC			12
#define DRAWING_ELLIPSE		13
#define DRAWING_TRIANGLE	14
*/

#define MSG_FIRSTMSG		600
#define MSG_TOOLTIP			601
#define MSG_POINTOUT        602
#define MSG_LASTMSG			699

#define MOUSE_NORMAL		1
#define MOUSE_OPEN_HAND		2
#define MOUSE_CLOSED_HAND	3
#define MOUSE_DRAWING		4
#define MOUSE_TEXT			5
#define MOUSE_ZOOM			6
#define MOUSE_SYMBOL		7

#define TIMER_CARET		1
#define	TIMER_PAINT		2
#define	TIMER_RECALC	3
#define	TIMER_IND_DLG_CANCEL 4

#define MIN_VISIBLE 5 // Minimum number of visible periods
#define NULL_VALUE -987654321
#define	VALUE_FONT_SIZE	80
#define DEFAULT_FONT_SIZE 88

#define MOUSE_DOWN	-1
#define	MOUSE_UP	1

#define	SEL_SPACE	45 // Space between selection boxes

// Scaling types
#define	SEMILOG		1
#define LINEAR		2

// Alignment
#define	LEFT	1
#define	RIGHT	2

// Chart symbol object types
#define BUY_SYMBOL		1
#define SELL_SYMBOL		2
#define EXIT_SYMBOL		3
#define CUSTOM_SYMBOL	4
#define EXIT_LONG_SYMBOL 5
#define EXIT_SHORT_SYMBOL 6
#define SIGNAL_SYMBOL 7

// Object types
#define	OBJECT_SERIES_INDICATOR	 497
#define OBJECT_SERIES_PF		 498
#define OBJECT_SERIES_RENKO		 499
#define OBJECT_SERIES_STOCK		 500
#define OBJECT_SERIES_CANDLE	 501
#define OBJECT_SERIES_LINE		 502
#define OBJECT_SERIES_BAR		 503
#define OBJECT_TEXT				 504
#define OBJECT_TEXT_LOCKED		 510
#define OBJECT_SYMBOL			 505
#define OBJECT_SERIES_STOCK_HLC	 506
#define OBJECT_SERIES_STOCK_LINE 507



#define OBJECT_LINE_STUDY		604


// Series types
#define LINE	  0 // Line series
#define BAR		  1 // Bar / volume
#define STOCK	  2 // OHLC
#define CANDLE	  3 // Candlestick
#define PF		  4 // Point & Figure
#define RENKO	  5 // Renko
#define STOCKLINE 6 // StockLine


// Compression intervals for Julian dates
#define JSECOND	1.1574011296034e-005f
#define	JMINUTE	0.00069444440305233f
#define	JHOUR	0.041666666511446f
#define	JDAY	1.0000000000000f
#define	JWEEK	7.0000000000000f
#define	JMONTH	30.000000002328f
#define	JYEAR	365.00000022864f

// Compression levels for Julian dates
#define LEVEL1	1	// 1.00069f	1 day
#define LEVEL2	2	// 5.00345f	1 week
#define LEVEL3	3	// 30.3600f	1 month
#define LEVEL4	4	// 365.270f	1 year

#define SCX_FILE_VERSION	10
/*
class CPointF{
public:
	double x,y;
	CPointF(double _x, double _y){
		x=_x;
		y=_y;
	};
};

class CRectF{
public:
	double left,top,right,bottom;
	CRectF(double _left,double _top, double _right, double _bottom){
		left=_left;
		top=_top;
		right=_right;
		bottom=_bottom;
	};
};
*/
class CLocation{
public:
	double y1;
	double y2;
	long x1;
	long x2;
};


class CRGB{
public:
	int red;
	int green;
	int blue;
};

typedef /* [helpstring][uuid] */ 
enum eZOrder{
	zOrderFront = 1,
	zOrderBack = -1
}ZOrder;

typedef /* [helpstring][uuid] */ 
enum eFillStyle{
	fsTransparent = 0,
	fsOpaque = 1
}FillStyle;


typedef /* [helpstring][uuid] */ 
enum eStudyType{
		lsEllipse,		
		lsRectangle,
		lsTrendLine,
		lsSpeedLines,
		lsGannFan,		
		lsFibonacciArcs,
		lsFibonacciFan,
		lsFibonacciRetracements,
		lsFibonacciProgression,
		lsFibonacciTimeZones,		
		lsTironeLevels,
		lsQuadrantLines,
		lsRaffRegression,
		lsErrorChannel,
		lsFreehand,
		lsTriangle,
		lsChannel,
		lsRay,
		lsXLine,
		lsYLine,
		lsPolyline
}STUDYTYPE;

typedef /* [helpstring][uuid] */ 
enum ePriceStyle{
	psStandard,
	psPointAndFigure,
	psRenko,
	psKagi,
	psThreeLineBreak,
	psEquiVolume,
	psEquiVolumeShadow,
	psCandleVolume,
	psHeikinAshi,
	psHeikinAshiSmooth,
	psStockLine
}PRICESTYLE;

typedef /* [helpstring][uuid] */ 
enum eSeriesType{
	stLineChart = OBJECT_SERIES_LINE,
	stVolumeChart = OBJECT_SERIES_BAR,
	stStockBarChart = OBJECT_SERIES_STOCK,
	stStockBarChartHLC = OBJECT_SERIES_STOCK_HLC,
	stCandleChart = OBJECT_SERIES_CANDLE,
	stStockLineChart = OBJECT_SERIES_STOCK_LINE
}SERIESTYPE;

 
const enum eObjectType{
	otTextObject = OBJECT_TEXT,
	otStaticTextObject = OBJECT_TEXT_LOCKED,
	otSymbolObject = OBJECT_SYMBOL,
	otBuySymbolObject = BUY_SYMBOL,
	otSellSymbolObject = SELL_SYMBOL,
	otLineStudyObject = OBJECT_LINE_STUDY,
	otExitSymbolObject = EXIT_SYMBOL,
	otExitLongSymbolObject = EXIT_LONG_SYMBOL,
	otExitShortSymbolObject = EXIT_SHORT_SYMBOL,
	otSignalSymbolObject = SIGNAL_SYMBOL,
	otTrendLineObject	= OBJECT_LINE_STUDY,
	otLineChart			= OBJECT_SERIES_LINE,
	otVolumeChart		= OBJECT_SERIES_BAR,
	otStockBarChart		= OBJECT_SERIES_STOCK,
	otStockBarChartHLC	= OBJECT_SERIES_STOCK_HLC,
	otCandleChart		= OBJECT_SERIES_CANDLE,
	otIndicator			= OBJECT_SERIES_INDICATOR,
	otStockLineChart	= OBJECT_SERIES_STOCK_LINE,
}OBJECTTYPE;


typedef /* [helpstring][uuid] */ 
enum eScaleType{
	stLinearScale = LINEAR,
	stSemiLogScale = SEMILOG
}SCALETYPE;



typedef /* [helpstring][uuid] */ 
enum eIndicator
    {indSimpleMovingAverage	= 0,
	indExponentialMovingAverage	= indSimpleMovingAverage + 1,
	indTimeSeriesMovingAverage	= indExponentialMovingAverage + 1,
	indTriangularMovingAverage	= indTimeSeriesMovingAverage + 1,
	indVariableMovingAverage	= indTriangularMovingAverage + 1,
	indVIDYA = indVariableMovingAverage + 1,
	indWeightedMovingAverage = indVIDYA + 1,
	indWellesWilderSmoothing = indWeightedMovingAverage + 1,
	indWilliamsPctR = indWellesWilderSmoothing + 1,
	indWilliamsAccumulationDistribution = indWilliamsPctR + 1,
	indVolumeOscillator = indWilliamsAccumulationDistribution + 1,
	indVerticalHorizontalFilter = indVolumeOscillator + 1,
	indUltimateOscillator = indVerticalHorizontalFilter + 1,
	indTrueRange = indUltimateOscillator + 1,
	indTRIX = indTrueRange + 1,
	indRainbowOscillator = indTRIX + 1,
	indPriceOscillator = indRainbowOscillator + 1,
	indParabolicSAR = indPriceOscillator + 1,
	indMomentumOscillator = indParabolicSAR + 1,
	indMACD = indMomentumOscillator + 1,
	indEaseOfMovement = indMACD + 1,
	indDirectionalMovementSystem = indEaseOfMovement + 1,
	indDetrendedPriceOscillator = indDirectionalMovementSystem + 1,
	indChandeMomentumOscillator = indDetrendedPriceOscillator + 1,
	indChaikinVolatility = indChandeMomentumOscillator + 1,
	indAroon = indChaikinVolatility + 1,
	indAroonOscillator = indAroon + 1,
	indLinearRegressionRSquared = indAroonOscillator + 1,
	indLinearRegressionForecast = indLinearRegressionRSquared + 1,
	indLinearRegressionSlope = indLinearRegressionForecast + 1,
	indLinearRegressionIntercept = indLinearRegressionSlope + 1,
	indPriceVolumeTrend = indLinearRegressionIntercept + 1,
	indPerformanceIndex = indPriceVolumeTrend + 1,
	indCommodityChannelIndex = indPerformanceIndex + 1,
	indChaikinMoneyFlow = indCommodityChannelIndex + 1,
	indWeightedClose = indChaikinMoneyFlow + 1,
	indVolumeROC = indWeightedClose + 1,
	indTypicalPrice = indVolumeROC + 1,
	indStandardDeviation = indTypicalPrice + 1,
	indPriceROC = indStandardDeviation + 1,
	indMedian = indPriceROC + 1,
	indHighMinusLow = indMedian + 1,
	indBollingerBands	= indHighMinusLow + 1,
	indFractalChaosBands	= indBollingerBands + 1,
	indHighLowBands	= indFractalChaosBands + 1,
	indMovingAverageEnvelope	= indHighLowBands + 1,
	indSwingIndex	= indMovingAverageEnvelope + 1,
	indAccumulativeSwingIndex	= indSwingIndex + 1,
	indComparativeRelativeStrength	= indAccumulativeSwingIndex + 1,
	indMassIndex	= indComparativeRelativeStrength + 1,
	indMoneyFlowIndex	= indMassIndex + 1,
	indNegativeVolumeIndex	= indMoneyFlowIndex + 1,
	indOnBalanceVolume	= indNegativeVolumeIndex + 1,
	indPositiveVolumeIndex	= indOnBalanceVolume + 1,
	indRelativeStrengthIndex	= indPositiveVolumeIndex + 1,
	indTradeVolumeIndex	= indRelativeStrengthIndex + 1,	
	indStochasticOscillator	= indTradeVolumeIndex + 1,
	indStochasticMomentumIndex = indStochasticOscillator + 1,
	indFractalChaosOscillator = indStochasticMomentumIndex + 1,
	indPrimeNumberOscillator = indFractalChaosOscillator + 1,
	indPrimeNumberBands = indPrimeNumberOscillator + 1,
	indHistoricalVolatility = indPrimeNumberBands + 1,
	indMACDHistogram = indHistoricalVolatility + 1,
	indCustomIndicator = indMACDHistogram + 1,
	indVolume	= indCustomIndicator + 1,
	indGenericMovingAverage = indVolume + 1,
	indHILOActivator = indGenericMovingAverage + 1,
	indAccumulationDistribution = indHILOActivator + 1,
	indADX	= indAccumulationDistribution + 1,

	indDI   = indADX + 1,
	LastIndicator	= indDI + 1
}INDICATORS;

#define		MA_START		indSimpleMovingAverage
#define		MA_END			indWeightedMovingAverage



typedef /* [helpstring][uuid] */ 
enum eParamType{
	ptMAType = 0, //changed by Eugen to 0 index. cause of ODL maintenance, it was = LastIndicator
	ptPctDMAType = ptMAType + 1,
	ptSymbol = ptPctDMAType + 1,
	ptSource = ptSymbol + 1,
	ptSource1 = ptSource + 1,
	ptSource2 = ptSource1 + 1,
	ptSource3 = ptSource2 + 1,
	ptVolume = ptSource3 + 1,
	ptPointsOrPercent = ptVolume + 1,
	ptPeriods = ptPointsOrPercent + 1,
	ptCycle1 = ptPeriods + 1,
	ptCycle2 = ptCycle1 + 1,
	ptCycle3 = ptCycle2 + 1,
	ptShortTerm = ptCycle3 + 1,
	ptLongTerm = ptShortTerm + 1,
	ptRateOfChange = ptLongTerm + 1,
	ptPctKPeriods = ptRateOfChange + 1,
	ptPctKSlowing = ptPctKPeriods + 1,
	ptPctDSmooth = ptPctKSlowing + 1,	
	ptPctKSmooth = ptPctDSmooth + 1,
	ptPctDDblSmooth = ptPctKSmooth + 1,
	ptPctDPeriods = ptPctDDblSmooth + 1,
	ptPctKDblSmooth = ptPctDPeriods + 1,
	ptShortCycle = ptPctKDblSmooth + 1,
	ptLongCycle = ptShortCycle + 1,
	ptStandardDeviations = ptLongCycle + 1,
	ptR2Scale = ptStandardDeviations + 1,
	ptMinAF = ptR2Scale + 1,
	ptMaxAF = ptMinAF + 1,
	ptShift = ptMaxAF + 1,
	ptFactor = ptShift + 1,
	ptSignalPeriods = ptFactor + 1,
	ptLimitMoveValue = ptSignalPeriods + 1,
	ptMinTickVal = ptLimitMoveValue + 1,
	ptLevels = ptMinTickVal + 1,
	ptBarHistory = ptLevels + 1,
	ptColorR3 = ptBarHistory + 1,
	ptColorG3 = ptColorR3 + 1,
	ptColorB3 = ptColorG3 + 1,
	ptStyle3 = ptColorG3 + 1,
	ptThickness3 = ptStyle3 + 1,
	ptThreshold1 = ptThickness3 + 1,
	ptThreshold2 = ptThreshold1 + 1,
	LastParamType = ptThreshold2 + 1,
}PARAMTYPE;


 

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__D44A9FBD_3153_4119_B1D3_422A3503595A__INCLUDED_)
