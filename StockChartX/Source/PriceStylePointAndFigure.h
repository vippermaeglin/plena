// PriceStyle.h: interface for the CPriceStyle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PRICESTYLEPF_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
#define AFX_PRICESTYLEPF_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "PriceStyle.h"

class CChartPanel;
class CStockChartXCtrl;


class CPriceStylePointAndFigure : public CPriceStyle
{
public:
	CPriceStylePointAndFigure::CPriceStylePointAndFigure();
	virtual ~CPriceStylePointAndFigure();		
	void OnPaint(CDC* pDC);
	void Connect(CStockChartXCtrl *Ctrl, CSeries* Series);
	bool connected;

};




#endif // !defined(AFX_PRICESTYLEPF_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
