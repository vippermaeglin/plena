// IndicatorParabolicSAR.h: interface for the CIndicatorParabolicSAR class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORParabolicSAR_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORParabolicSAR_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorParabolicSAR : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorParabolicSAR(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorParabolicSAR();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORParabolicSAR_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)

