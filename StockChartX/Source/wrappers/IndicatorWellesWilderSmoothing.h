// IndicatorWellesWilderSmoothing.h: interface for the CIndicatorWellesWilderSmoothing class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_IndicatorWellesWilderSmoothing_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_IndicatorWellesWilderSmoothing_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorWellesWilderSmoothing : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorWellesWilderSmoothing(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorWellesWilderSmoothing();
	BOOL Calculate();

};

#endif // !defined(AFX_IndicatorWellesWilderSmoothing_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)

