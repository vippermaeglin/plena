// PriceStyle.h: interface for the CPriceStyle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PRICESTYLE_H__064B65A7_7A24_4CCA_A3D2_0E22CF673D5F__INCLUDED_)
#define AFX_PRICESTYLE_H__064B65A7_7A24_4CCA_A3D2_0E22CF673D5F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CStockChartXCtrl;

class CPriceStyle
{
public:
	int nSavedDC;
	UINT Style;
	CPriceStyle();
	virtual ~CPriceStyle();
	virtual void OnPaint(CDC *pDC);
	virtual void Connect(CStockChartXCtrl *Ctrl, CSeries* Series);
	CSeries* pSeries;
	void ExcludeRects(CDC* pDC);
	void IncludeRects(CDC* pDC);
	CStockChartXCtrl* pCtrl;
};

#endif // !defined(AFX_PRICESTYLE_H__064B65A7_7A24_4CCA_A3D2_0E22CF673D5F__INCLUDED_)
