// MovingAverage.h: interface for the MovingAverage class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MOVINGAVERAGE_H__429672E4_EBBE_4F67_B56C_272B14F30D11__INCLUDED_)
#define AFX_MOVINGAVERAGE_H__429672E4_EBBE_4F67_B56C_272B14F30D11__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class TASDK;
class CField;
class CNavigator;
class CRecordset;

class CMovingAverage  
{
public:
	CMovingAverage();
	virtual ~CMovingAverage();
	CRecordset* SimpleMovingAverage(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Simple Moving Average", int Shift = 0);
	CRecordset* ExponentialMovingAverage(CNavigator* pNav,CField* pSource, int Periods, LPCTSTR Alias = "Exponential Moving Average", int Shift = 0);
	CRecordset* TimeSeriesMovingAverage(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Time Series Moving Average", int Shift = 0);
	CRecordset* VariableMovingAverage(CNavigator* pNav,CField* pSource, int Periods, LPCTSTR Alias = "Variable Moving Average", int Shift = 0);
	CRecordset* TriangularMovingAverage(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Triangular Moving Average", int Shift = 0);
	CRecordset* WeightedMovingAverage(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Weighted Moving Average", int Shift = 0);
	CRecordset* VIDYA(CNavigator* pNav, CField* pSource, int Periods, double R2Scale, LPCTSTR Alias = "VIDYA", int Shift = 0);
	CRecordset* WellesWilderSmoothing(CField* pSource, int Periods, LPCTSTR Alias = "Welles Wilder Smoothing", int Shift = 0, int ignoreOffset = 0);
	//CRecordset* GenericMovingAverage(CNavigator* pNav, CField* pSource, int Periods, int Shift, int Type, double R2Scale, LPCTSTR Alias = "Generic Moving Average");

};

#endif // !defined(AFX_MOVINGAVERAGE_H__429672E4_EBBE_4F67_B56C_272B14F30D11__INCLUDED_)
