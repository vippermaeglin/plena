// Oscillator.h: interface for the Oscillator class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_OSCILLATOR_H__E8E538A4_DFDC_403F_A2BD_03CB35B08014__INCLUDED_)
#define AFX_OSCILLATOR_H__E8E538A4_DFDC_403F_A2BD_03CB35B08014__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CNavigator;
class CRecordset;
class CField;
class MovingAverageType;

class COscillator  
{
public:
	COscillator();
	virtual ~COscillator();
	CRecordset* ChandeMomentumOscillator(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "CMO");
	CRecordset* Momentum(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Momentum");
	CRecordset* TRIX(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "TRIX");
	CRecordset* UltimateOscillator(CNavigator* pNav, CRecordset* pOHLCV, 
			int Cycle1, int Cycle2, int Cycle3, LPCTSTR Alias = "Ultimate Oscillator");
	CRecordset* VerticalHorizontalFilter(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "VHF");
	CRecordset* WilliamsPctR(CNavigator* pNav, CRecordset* pOHLCV, int Periods, LPCTSTR Alias = "Williams' %R");
	CRecordset* WilliamsAccumulationDistribution(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "Williams' Accumulation Distribution");
	CRecordset* VolumeOscillator(CNavigator* pNav, CField* Volume, int ShortTerm, 
			int LongTerm, int PointsOrPercent, LPCTSTR Alias = "Volume Oscillator");
	CRecordset* ChaikinVolatility(CNavigator* pNav, CRecordset* pOHLCV,
				int Periods, int ROC, int MAType, LPCTSTR Alias = "Chaikin Volatility");
	CRecordset* StochasticOscillator(CNavigator* pNav, CRecordset* pOHLCV,
				int KPeriods, int KSlowingPeriods, int DPeriods, int MAType);
	CRecordset* PriceOscillator(CNavigator* pNav, CField* pSource,
				int LongCycle, int ShortCycle, int MAType, LPCTSTR Alias = "Price Oscillator");
	CRecordset* MACD(CNavigator* pNav, CRecordset* pOHLCV,
				int SignalPeriods, int LongCycle, int ShortCycle, LPCTSTR Alias = "MACD");
	CRecordset* MACDHistogram(CNavigator* pNav, CRecordset* pOHLCV,
				int SignalPeriods, int LongCycle, int ShortCycle, LPCTSTR Alias = "MACD");
	CRecordset* EaseOfMovement(CNavigator* pNav, CRecordset* pOHLCV,
				int Periods, int MAType, LPCTSTR Alias = "Ease of Movement");
	CRecordset* DetrendedPriceOscillator(CNavigator* pNav, CField* pSource,
				int Periods, int MAType, LPCTSTR Alias = "DPO");
	CRecordset* ParabolicSAR(CNavigator* pNav, CField* HighPrice,
            CField* LowPrice, double MinAF = 0.02, double MaxAF = 0.2, 
			LPCTSTR Alias = "Parabolic SAR");
	CRecordset* DirectionalMovementSystem(CNavigator* pNav, CRecordset* pOHLCV, int Periods);
	CRecordset* TrueRange(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "TR");
	CRecordset* Aroon(CNavigator* pNav, CRecordset* pOHLCV, int Periods);
	CRecordset* RainbowOscillator(CNavigator* pNav, CField* pSource, int Levels, 
			int MAType, LPCTSTR Alias = "Rainbow Oscillator");
	CRecordset* FractalChaosOscillator(CNavigator* pNav, CRecordset* pOHLCV, 
			int Periods, LPCTSTR Alias = "Fractal Chaos Oscillator");	   
   CRecordset* PrimeNumberOscillator(CNavigator* pNav, 
			CField* pSource, LPCTSTR Alias ="Prime Number Oscillator");

};

#endif // !defined(AFX_OSCILLATOR_H__E8E538A4_DFDC_403F_A2BD_03CB35B08014__INCLUDED_)
