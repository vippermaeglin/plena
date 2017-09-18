// IndicatorHighLowBands.cpp: implementation of the CIndicatorHighLowBands class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "IndicatorHighLowBands.h"

#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorHighLowBands::CIndicatorHighLowBands(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	szName = name;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	seriesType = type;
	memberCount = members;
	Initialize();
	nSpace = 0;

	// Resize param arrays for this indicator.
	// NOTE! Set all array sizes to max number of parameters.
	// ALL three arrays must be resized.
	paramCount = 2;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indHighLowBands;

}

CIndicatorHighLowBands::~CIndicatorHighLowBands()
{
	CIndicator::OnDestroy();
}

void CIndicatorHighLowBands::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Periods (eg 14)

	*/
	
	SetParam(0, ptSymbol, "");
	SetParam(1, ptPeriods, "14");

}

BOOL CIndicatorHighLowBands::Calculate()
{
	
	/*
		1. Validate the indicator parameters (if any)
		2. Validate available inputs
		3. Gather the inputs into a TA-SDK recordset
		4. Calculate the indicator
		5. If there is only one output, store the data
		   in the data_master array of this series. 
		   If there are two or more outputs, create new 
		   CSeriesStandard for each additional ouput
	*/


	// Get input from user
	if(!GetUserInput()) return FALSE;


	// Validate
	long size = pCtrl->RecordCount();
	if(size == 0) return FALSE;
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	if(paramStr.size() < (unsigned int)paramCount)
		return FALSE;
	if(paramInt.size() < (unsigned int)paramCount)
		return FALSE;
//	End Of Revision
	if(paramInt[1] < 6 || paramInt[1] > size / 2){
		ProcessError("Invalid Periods (min 6) for indicator " + szName);
		return FALSE;
	}
	
 
	// Get the data
	CField* pHigh = SeriesToField("High", paramStr[0] + ".high", size);
	if(!EnsureField(pHigh, paramStr[0] + ".high")) return FALSE;
	CField* pLow = SeriesToField("Low", paramStr[0] + ".low", size);
	if(!EnsureField(pLow, paramStr[0] + ".low")) return FALSE;
	CField* pClose = SeriesToField("Close", paramStr[0] + ".close", size);	

	if(!EnsureField(pClose, paramStr[0] + ".close")) return FALSE;
 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pHigh);
	pRS->addField(pLow);
	pRS->addField(pClose);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	CBands ta;
	pInd = ta.HighLowBands(pNav, pHigh, pLow, pClose, paramInt[1]);


	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	EnsureSeries(szName + " Top");
	EnsureSeries(szName + " Bottom");
	//SetSeriesColor(szName + " Top", lineColor);
	//SetSeriesColor(szName + " Bottom", lineColor);
	SetSeriesSignal(szName + " Top",lineColor,lineStyle,lineWeight);
	SetSeriesSignal(szName + " Bottom",lineColor,lineStyle,lineWeight);
	double top = 0, median = 0, bottom = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
			top = NULL_VALUE;
			median = NULL_VALUE;
			bottom = NULL_VALUE;
		}
		else{
			top = pInd->getValue("High Low Bands Top", n + 1);
			//median = pInd->getValue("High Low Bands Median", n + 1);
			bottom = pInd->getValue("High Low Bands Bottom", n + 1);
		}
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, median);
		pCtrl->AppendNewValue(szName + " Top", jdate, top);
		pCtrl->AppendNewValue(szName + " Bottom", jdate, bottom);
	}
 

	// Clean up	
	delete pRS;
	delete pInd;
	delete pNav;


	return CIndicator::Calculate();
}
