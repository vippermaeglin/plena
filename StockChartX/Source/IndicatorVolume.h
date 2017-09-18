// IndicatorVolume.h: interface for the CIndicatorVolume class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORVolume_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORVolume_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorVolume : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorVolume(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorVolume();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORVolumeROC_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
