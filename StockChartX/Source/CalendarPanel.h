// CalendarPanel.h: interface for the CCalendarPanel class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CALENDARPANEL_H__5CA24B84_1477_4A06_B93E_26D5C8314BD9__INCLUDED_)
#define AFX_CALENDARPANEL_H__5CA24B84_1477_4A06_B93E_26D5C8314BD9__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
class CDate;
class CStockChartXCtrl;
class CCalendarPanel
{
public:
	void OnPaint(CDC* pDC, CStockChartXCtrl* pCtrl);
	CDate FromJulianDateToCDate(double jdate);
	//LPCTSTR FormatDate(int month, int day, int year, int hour, int minute, int second);
	CCalendarPanel();
	virtual ~CCalendarPanel();
private:	
	double GetDate(int period, int x, CStockChartXCtrl* pCtrl);
	double GetX(int period,CStockChartXCtrl* pCtrl);
};

#endif // !defined(AFX_CALENDARPANEL_H__5CA24B84_1477_4A06_B93E_26D5C8314BD9__INCLUDED_)
