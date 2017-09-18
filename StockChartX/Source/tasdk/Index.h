// Index.h: interface for the Index class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDEX_H__FFEEC053_2408_4046_A889_9980CECA4B86__INCLUDED_)
#define AFX_INDEX_H__FFEEC053_2408_4046_A889_9980CECA4B86__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CNavigator;
class CRecordset;
class CField;
class TASDK;

class CIndex  
{
public:
	CIndex();
	virtual ~CIndex();
	CRecordset* MoneyFlowIndex(CNavigator* pNav, CRecordset* pOHLCV, 
	    	    int Periods, LPCTSTR Alias = "Money Flow Index");
	CRecordset* TradeVolumeIndex(CNavigator* pNav, CField* pSource,
				CField* Volume, double MinTickValue, LPCTSTR Alias = "Trade Volume Index");
	CRecordset* SwingIndex(CNavigator* pNav, CRecordset* pOHLCV,
				double LimitMoveValue, LPCTSTR Alias = "Swing Index");
	CRecordset* AccumulativeSwingIndex(CNavigator* pNav, CRecordset* pOHLCV,
				double LimitMoveValue, LPCTSTR Alias = "Accumulative Swing Index");
	CRecordset* RelativeStrengthIndex(CNavigator* pNav, CField* pSource,
				int Periods, LPCTSTR Alias = "Relative Strength Index");
	CRecordset* ComparativeRelativeStrength(CNavigator* pNav, CField* pSource1,
				CField* pSource2, LPCTSTR Alias = "Comparative Relative Strength");
	CRecordset* PriceVolumeTrend(CNavigator* pNav, CField* pSource,
				CField* Volume, LPCTSTR Alias = "Price Volume Trend");
	CRecordset* PositiveVolumeIndex(CNavigator* pNav,
				CField* pSource, CField* Volume, LPCTSTR Alias = "Positive Volume Index");
	CRecordset* NegativeVolumeIndex(CNavigator* pNav,
				CField* pSource, CField* Volume, LPCTSTR Alias = "Negative Volume Index");
	CRecordset* Performance(CNavigator* pNav, CField* pSource, LPCTSTR Alias = "Performance");
	CRecordset* MassIndex(CNavigator* pNav, CRecordset* pOHLCV, 
			int Periods, LPCTSTR Alias = "Mass Index");
	CRecordset* OnBalanceVolume(CNavigator* pNav, CField* pSource,
				CField* Volume, LPCTSTR Alias = "On Balance Volume");
	CRecordset* ChaikinMoneyFlow(CNavigator* pNav, CRecordset* pOHLCV, 
		int Periods, LPCTSTR Alias = "Chaikin Money Flow");
	CRecordset* AccumulationDistribution(CNavigator* pNav, CRecordset* pOHLCV, 
		LPCTSTR Alias = "Accumulation / Distribution");
	CRecordset* CommodityChannelIndex(CNavigator* pNav, CRecordset* pOHLCV, 
	  int Periods, LPCTSTR Alias = "CCI");
	CRecordset* StochasticMomentumIndex(CNavigator* pNav, CRecordset* pOHLCV,
            int KPeriods, int KSmooth, int KDoubleSmooth, 
			int DPeriods, int MAType, int PctD_MAType);
	CRecordset* HistoricalVolatility(CNavigator* pNav, CField* pSource,
              int Periods = 30, int BarHistory = 365, int MAType = 0, double StandardDeviations = 2.0,
			  LPCTSTR Alias = "Historical Volatility");

};

#endif // !defined(AFX_INDEX_H__FFEEC053_2408_4046_A889_9980CECA4B86__INCLUDED_)
