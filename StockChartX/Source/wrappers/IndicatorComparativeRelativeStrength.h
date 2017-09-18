// IndicatorComparativeRelativeStrength.h: interface for the CIndicatorComparativeRelativeStrength class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORComparativeRelativeStrength_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORComparativeRelativeStrength_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorComparativeRelativeStrength : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorComparativeRelativeStrength(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorComparativeRelativeStrength();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORComparativeRelativeStrength_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
