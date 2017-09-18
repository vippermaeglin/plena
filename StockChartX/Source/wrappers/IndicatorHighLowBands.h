// IndicatorHighLowBands.h: interface for the CIndicatorHighLowBands class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORHighLowBands_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORHighLowBands_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorHighLowBands : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorHighLowBands(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorHighLowBands();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORHighLowBands_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
