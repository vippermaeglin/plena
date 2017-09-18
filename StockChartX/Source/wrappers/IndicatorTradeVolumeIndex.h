// IndicatorTradeVolumeIndex.h: interface for the CIndicatorTradeVolumeIndex class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORTradeVolumeIndex_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORTradeVolumeIndex_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorTradeVolumeIndex : public CIndicator  
{
public:
	void SetParamInfo();
	CIndicatorTradeVolumeIndex(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorTradeVolumeIndex();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORTradeVolumeIndex_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)

