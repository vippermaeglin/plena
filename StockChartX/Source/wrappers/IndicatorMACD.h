// IndicatorMACD.h: interface for the CIndicatorMACD class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORMACD_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)
#define AFX_INDICATORMACD_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorMACD : public CIndicator  
{
public:
	//TWIN
	/*OLE_COLOR lineColorSignal;
	int lineStyleSignal;
	int lineWeightSignal;*/
	void SetParamInfo();
	CIndicatorMACD(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorMACD();
	BOOL Calculate();

};

#endif // !defined(AFX_INDICATORMACD_H__52A2F0F5_8BE9_4204_974B_1C3D63A4D013__INCLUDED_)

