// TASDK.h: interface for the CTASDK class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_TASDK_H__0E41906B_8167_43C6_AE5A_03E6D4AC3756__INCLUDED_)
#define AFX_TASDK_H__0E41906B_8167_43C6_AE5A_03E6D4AC3756__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <vector>
using namespace std;

#pragma warning(disable: 4267) 

#define NULL_VALUE	-987654321

#define	MA_START	0
#define SIMPLE		1
#define EXPONENTIAL	2
#define TIME_SERIES	3
#define VARIABLE	4
#define TRIANGULAR	5
#define WEIGHTED	6
#define VOLATILITY	7
#define WILDER		8
#define MA_END		9
		
class field
{
public:
	field::field()
	{
		name = "";		
	}
	string name;
	vector<double> data;
};

typedef vector<field> recordset;


class CTASDK  
{
public:
	CTASDK();
	virtual ~CTASDK();




	void Test();

	
	field* AddField( const string& name, int size, recordset& recSet );
	__inline field* GetField( const string& name, recordset& recSet );
	__inline field CopyField( const string& name, recordset& recSet );
	__inline bool IsEqual( recordset& recSet );
	__inline int GetSize( recordset& ohlcv );



	// General functions
	field HighMinusLow( recordset& ohlcv, const string& name = "" );
	field MedianPrice( recordset& ohlcv, const string& name = "" );
	field TypicalPrice( recordset& ohlcv, const string& name = "" );
	field WeightedClose( recordset& ohlcv, const string& name = "" );
	field PriceROC( field* source, int periods, const string& name = "" );
	field VolumeROC( field* volume, int periods, const string& name = "" );	
	double CorrelationAnalysis( field* source1, field* source2 );
	void MaxMinValue( field* source, const int start_period, const int end_period, double& max_value, double& min_value );
	bool IsPrime( const long number );
	field HHV( field* high, int periods, const string& name = "" );
	field LLV( field* low, int periods, const string& name = "" );
	field StandardDeviation( field* source, int periods, int standardDeviations, int maType, const string& name = "" );
	field Trend( field* source, int periods, const string& name = "" );
	void LogItem(string logitem, string file);

	// Linear regression functions
	recordset Regression( field* source, int periods );
	field TimeSeriesForecast( field* source, int periods, const string& name = "" );
	
	// Moving average functions
	field CTASDK::GetMAByType(field* source, int periods, int maType, const string& name = "");
	field GenericMovingAverage(field* source, int periods, int shift, int maType, double r2scale, const string& name);
	field SimpleMovingAverage( field* source, int periods, const string& name = "", int shift=0);
	field ExponentialMovingAverage( field* source, int periods, const string& name = "" );
	field TimeSeriesMovingAverage( field* source, int periods, const string& name = "" );
	field VariableMovingAverage( field* source, int periods, const string& name = "" );
	field TriangularMovingAverage( field* source, int periods, const string& name = "" );
	field WeightedMovingAverage( field* source, int periods, const string& name = "" );
	field VIDYA( field* source, int periods, double r2scale, const string& name = "" );
	field WellesWilderSmoothing( field* source, int periods, const string& name = "" );
	
	// Oscillator functions
	field ChandeMomentumOscillator( field* source, int periods, const string& name = "" );
	field MomentumOscillator( field* source, int periods, const string& name = "" );
	field TRIX( field* source, int periods, const string& name = "" );
	field UltimateOscillator( recordset& ohlcv, int cycle1, int cycle2, int cycle3, const string& name = "" );
	field VerticalHorizontalFilter( field* source, int periods, const string& name = "" );
	field WilliamsPctR( recordset& ohlcv, int periods, const string& name = "" );
	field WilliamsAccumulationDistribution( recordset& ohlcv, const string& name = "" );
	field VolumeOscillator( field* volume, int shortTerm, int longTerm, int maType, int pointsOrPercent, const string& name = "" );
	field ChaikinVolatility( recordset& ohlcv, int periods, int roc, int maType, const string& name = "" );
	recordset StochasticOscillator( recordset& ohlcv, int kPeriods, int kSlowingPeriods, int dPeriods, int maType );
	field PriceOscillator( field* source, int shortTerm, int longTerm, int maType, const string& name = "" );
	recordset MACD( recordset& ohlcv, int shortCycle, int longCycle, int signalPeriods, int maType, const string& name = "" );
	field EaseOfMovement( recordset& ohlcv, int periods, int maType, const string& name = "" );
	field DetrendedPriceOscillator( field* source, int periods, int maType, const string& name = "" );
	field ParabolicSAR( field* highPrice, field* lowPrice, double minAF = 0.02, double maxAF = 0.2, const string& name = "" );
	field TrueRange( recordset& ohlcv, const string& name = "" );
	field AverageTrueRange( recordset& ohlcv, int periods, int maType, const string& name = "" );
	recordset DirectionalMovementSystem( recordset& ohlcv, int periods );
	recordset Aroon( recordset& ohlcv, int periods );
	field RainbowOscillator( field* source, int levels, int maType, const string& name = "" );
	field FractalChaosOscillator( recordset& ohlcv, int periods, const string& name = "" );
	field PrimeNumberOscillator( field* source, const string& name = "" );

	// Index functions
	field MoneyFlowIndex( recordset& ohlcv, int periods, const string& name = "" );
	field TradeVolumeIndex( field* source, field* volume, double minTickValue, const string& name = "" );
	field SwingIndex( recordset& ohlcv, double limitMoveValue, const string& name = "" );
	field AccumulativeSwingIndex( recordset& ohlcv, double limitMoveValue, const string& name = "" );
	field RelativeStrengthIndex( field* source, int periods, const string& name = "" );
	field ComparativeRelativeStrengthIndex( field* source1, field* source2, const string& name = "" );
	field PriceVolumeTrend( field* source, field* volume, const string& name = "" );
	field PositiveVolumeIndex( field* source, field* volume, const string& name = "" );
	field NegativeVolumeIndex( field* source, field* volume, const string& name = "" );
	field PerformanceIndex( field* source, const string& name = "" );
	field OnBalanceVolume( field* source, field* volume, const string& name = "" );
	field CTASDK::HistoricalVolatility( field* source, int periods = 30, int barHistory = 365, 
										  int standardDeviations = 2, const string& name = "" );
	field MassIndex( recordset& ohlcv, int periods, const string& name = "" );
	field ChaikinMoneyFlow( recordset& ohlcv, int periods, const string& name = "" );
	field CommodityChannelIndex( recordset& ohlcv, int periods, int maType, const string& name = "" );
	recordset StochasticMomentumIndex( recordset& ohlcv, int kPeriods, int kSmooth, 
										  int kDoubleSmooth, int dPeriods, int maType, int pctD_maType );

	// Band functions
	recordset KeltnerChannel( recordset& ohlcv, int periods, int maType, double multiplier );
	recordset BollingerBands( field* source, int periods, int standardDeviations, int maType );
	recordset MovingAverageEnvelope( field* source, int periods, int maType, double shift );
	recordset PrimeNumberBands( recordset& ohlcv );

	// Candlestick recognition

private:
	
	double m_MinPrice;
	long m_MinVolume;
	
	class Candlestick
	{
	public:
		Candlestick::Candlestick()
		{
			jdateTime = 0;
			pattern = 0;
			direction = 0;			
			range = bullBear  = 
			openPrice = highPrice = 
			lowPrice = closePrice = 0;
			volume = 0;
		}
		double jdateTime;
		int pattern;
		double range;
		double bullBear;
		int direction;
		double openPrice;
		double highPrice;
		double lowPrice;
		double closePrice;
		double volume;
	};
	std::vector<Candlestick> m_Candlesticks;
public:
	
	// Candlestick pattern definitions 
	#define	LONG_BODY	1
	#define	DOJI	2
	#define	HAMMER	3
	#define	HARAMI	4
	#define	STAR	5
	#define DOJI_STAR	6
	#define	MORNING_STAR	7
	#define	EVENING_STAR	8
	#define	PIERCING_LINE	9
	#define	BULLISH_ENGULFING_LINE	10
	#define	HANGING_MAN	11
	#define DARK_CLOUD_COVER	12
	#define	BEARISH_ENGULFING_LINE	13
	#define	BEARISH_DOJI_STAR	14
	#define	BEARISH_SHOOTING_STAR	15
	#define	SPINNING_TOPS	16
	#define	HARAMI_CROSS	17
	#define	BULLISH_TRISTAR	18
	#define	THREE_WHITE_SOLDIERS	19
	#define	THREE_BLACK_CROWS	20
	#define	ABANDONED_BABY	21
	#define	BULLISH_UPSIDE_GAP	22
	#define	BULLISH_HAMMER	23
	#define	BULLISH_KICKING	24
	#define	BEARISH_KICKING	25
	#define	BEARISH_BELT_HOLD	26
	#define	BULLISH_BELT_HOLD	27
	#define	BEARISH_TWO_CROWS	28
	#define	BULLISH_MATCHING_LOW	29

	int CandleStickPattern( recordset& ohlcv );


};

#endif // !defined(AFX_TASDK_H__0E41906B_8167_43C6_AE5A_03E6D4AC3756__INCLUDED_)
