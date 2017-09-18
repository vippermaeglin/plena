// PriceStyle.h: interface for the CPriceStyle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PRICESTYLEHEIKINASHI_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
#define AFX_PRICESTYLEHEIKINASHI_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "PriceStyle.h"

class CChartPanel;
class CStockChartXCtrl;


class CPriceStyleHeikinAshi : public CPriceStyle
{
public:
	CPriceStyleHeikinAshi::CPriceStyleHeikinAshi(bool Smoothed = false);
	virtual ~CPriceStyleHeikinAshi();		
	void OnPaint(CDC* pDC);
	void Connect(CStockChartXCtrl *Ctrl, CSeries* Series);
	bool connected;
	bool smoothed;

private:
	void DrawGradientCandle(CDC *pDC, CRect boxRect, OLE_COLOR color);
	double GetX(int period);
	double MaxOf(double a, double b, double c);
	double MinOf(double a, double b, double c);
};



#endif // !defined(AFX_PRICESTYLEEQUIVOL_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
