// Date.h: interface for the CDate class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DATE_H__CDE3B576_532F_4175_92B0_08C05515CE45__INCLUDED_)
#define AFX_DATE_H__CDE3B576_532F_4175_92B0_08C05515CE45__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CDate  
{
public:
	CString GetMonth(int nMonth, int nLength, CStockChartXCtrl* pCtrl);
	CDate();
	CDate(int nMonth, int nDay, int nYear, int nHour, int nMinute, int nSecond);
	bool IsValid();
	virtual ~CDate();
	int Day;
	int Month;
	int Year;
	int Hour;
	int Minute;
	int Second;
};

#endif // !defined(AFX_DATE_H__CDE3B576_532F_4175_92B0_08C05515CE45__INCLUDED_)
