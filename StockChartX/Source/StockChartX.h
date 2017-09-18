#if !defined(AFX_STOCKCHARTX_H__FF01AF83_F2C7_4B96_A60F_F76ACF5EF3CB__INCLUDED_)
#define AFX_STOCKCHARTX_H__FF01AF83_F2C7_4B96_A60F_F76ACF5EF3CB__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// StockChartX.h : main header file for STOCKCHARTX.DLL

#if !defined( __AFXCTL_H__ )
	#error include 'afxctl.h' before including this file
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CStockChartXApp : See StockChartX.cpp for implementation.

#include "StockChartXCtl.h"
class CStockChartXCtl;

#include "ChartPanel.h"
class CChartPanel;





class CStockChartXApp : public COleControlModule
{
public:

	BOOL InitInstance();
	int ExitInstance();

};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STOCKCHARTX_H__FF01AF83_F2C7_4B96_A60F_F76ACF5EF3CB__INCLUDED)
