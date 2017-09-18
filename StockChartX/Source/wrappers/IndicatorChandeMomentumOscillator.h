// IndicatorChandeMomentumOscillator.h: interface for the CIndicatorChandeMomentumOscillator class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORChandeMomentumOscillator_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORChandeMomentumOscillator_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorChandeMomentumOscillator : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorChandeMomentumOscillator(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorChandeMomentumOscillator();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORChandeMomentumOscillator_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)

