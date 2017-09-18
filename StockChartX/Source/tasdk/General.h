// General.h: interface for the General class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_GENERAL_H__0813D5A7_5088_4876_8828_9428B06860C0__INCLUDED_)
#define AFX_GENERAL_H__0813D5A7_5088_4876_8828_9428B06860C0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CNavigator;
class CRecordset;
class CNote;
class CField;

class CGeneral  
{
public:
	CGeneral();
	virtual ~CGeneral();
	CRecordset* HighMinusLow(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "High Minus Low");
	CRecordset* MedianPrice(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "Median Price");
	CRecordset* TypicalPrice(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "Typical Price");
	CRecordset* WeightedClose(CNavigator* pNav, CRecordset* pOHLCV, LPCTSTR Alias = "Weighted Close");
	CRecordset* VolumeROC(CNavigator* pNav, CField* Volume, int Periods, LPCTSTR Alias = "Volume ROC");
	CRecordset* PriceROC(CNavigator* pNav, CField* pSource, int Periods, LPCTSTR Alias = "Price ROC");
	CRecordset* Volume(CNavigator* pNav, CField* Volume, LPCTSTR Alias = "Volume");
	double CorrelationAnalysis(CField* pSource1, CField* pSource2);
	CRecordset* StandardDeviation(CNavigator* pNav, CField* pSource, int Periods, 
		double StandardDeviations, int MAType, LPCTSTR Alias = "Standard Deviation");
	CNote* MaxValue(CField* pSource, int StartPeriod, int EndPeriod);
	CNote* MinValue(CField* pSource, int StartPeriod, int EndPeriod);
	CRecordset* HHV(CNavigator* pNav, CField* High, int Periods, LPCTSTR Alias = "HHV");
	CRecordset* LLV(CNavigator* pNav, CField* Low, int Periods, LPCTSTR Alias = "LLV");
	bool IsPrime(long number);

};

#endif // !defined(AFX_GENERAL_H__0813D5A7_5088_4876_8828_9428B06860C0__INCLUDED_)
