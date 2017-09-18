// SeriesStock.h: interface for the CSeriesStock class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SERIESSTOCK_H__E0F4400D_E56E_41B9_AD7E_46CAEAAC9E52__INCLUDED_)
#define AFX_SERIESSTOCK_H__E0F4400D_E56E_41B9_AD7E_46CAEAAC9E52__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Series.h"
#include "PriceStyle.h"
#include "CO.h"



//#define _CONSOLE_DEBUG 1


class CStockChartXCtrl;
class CPriceStyle;

class CSeriesStock : public CSeries  
{
public:
	void PaintDarvasBoxes(CDC *pDC);
	void OnDoubleClick(CPoint point);
	void OnRButtonUp(CPoint point);
	void OnPaintXOR(CDC* pDC);
	void OnLButtonDown(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnMouseMove(CPoint point);
	void OnPaint(CDC* pDC);
	bool m_candleChart;
	CSeriesStock(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CSeriesStock();
	
	// + function to get candle
	Candle GetCandle(int recordCount);

private:	
	void DrawGradientCandle(CDC *pDC, CRect boxRect, OLE_COLOR color);
	double nSpace;	
	void PaintCandles(CDC *pDC);
	void PaintLines(CDC *pDC);
	void PaintStock(CDC* pDC);
	CStockChartXCtrl* pCtrl;

};

#endif // !defined(AFX_SERIESSTOCK_H__E0F4400D_E56E_41B9_AD7E_46CAEAAC9E52__INCLUDED_)
