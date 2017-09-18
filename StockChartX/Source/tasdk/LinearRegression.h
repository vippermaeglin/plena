// LinearRegression.h: interface for the LinearRegression class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINEARREGRESSION_H__BBE3A2D0_DE22_479B_BBCD_90DC75EFF9CE__INCLUDED_)
#define AFX_LINEARREGRESSION_H__BBE3A2D0_DE22_479B_BBCD_90DC75EFF9CE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CNavigator;
class CField;

class CLinearRegression  
{
public:
	CLinearRegression();
	virtual ~CLinearRegression();
	CRecordset* Regression(CNavigator* pNav, CField* pSource, int Periods);
	CRecordset* TimeSeriesForecast(CNavigator* pNav,CField* pSource, int Periods, LPCTSTR Alias = "Time Series Forecast");

};

#endif // !defined(AFX_LINEARREGRESSION_H__BBE3A2D0_DE22_479B_BBCD_90DC75EFF9CE__INCLUDED_)
