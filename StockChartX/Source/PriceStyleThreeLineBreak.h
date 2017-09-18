// PriceStyle.h: interface for the CPriceStyle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PRICESTYLETLB_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
#define AFX_PRICESTYLETLB_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "PriceStyle.h"

class CChartPanel;
class CStockChartXCtrl;


class CPriceStyleThreeLineBreak : public CPriceStyle
{
public:
	CPriceStyleThreeLineBreak::CPriceStyleThreeLineBreak();
	virtual ~CPriceStyleThreeLineBreak();		
	void OnPaint(CDC* pDC);
	void Connect(CStockChartXCtrl *Ctrl, CSeries* Series);
	bool connected;

private:	
	long prevIndex;
	void PaintBox(CDC* pDC, int x, double space,
						double top, double bottom, 
						int direction, int index,
						CSeries* pClose);
	bool IsNewBlock(int direction, double close);
	void AddBlock(double high, double low);
	std::vector<double> highs;
	std::vector<double> lows;

};




#endif // !defined(AFX_PRICESTYLETLB_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
