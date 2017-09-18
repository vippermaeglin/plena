#pragma once

#include "Indicator.h"
class CIndicatorAccumulationDistribution : public CIndicator
{
public:
	void SetParamInfo();
	CIndicatorAccumulationDistribution(LPCTSTR name, int type, int members, CChartPanel* owner);
	virtual ~CIndicatorAccumulationDistribution();
	BOOL Calculate();

};

