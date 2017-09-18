// IndicatorParabolicSAR.cpp: implementation of the CIndicatorParabolicSAR class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorParabolicSAR.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorParabolicSAR::CIndicatorParabolicSAR(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 3;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indParabolicSAR;

}

CIndicatorParabolicSAR::~CIndicatorParabolicSAR()
{
	CIndicator::OnDestroy();
}

void CIndicatorParabolicSAR::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")	  
	  2. paramDbl[1] = MinAF (eg 0.02)
	  3. paramDbl[2] = MaxAF (eg 0.2)

	*/
	
	SetParam(0, ptSymbol, "");
	SetParam(1, ptMinAF, "0.02");
	SetParam(2, ptMaxAF, "0.2");

}

BOOL CIndicatorParabolicSAR::Calculate()
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
//	End Of Revision
		return FALSE;


	if(paramDbl[1] <= 0){
		ProcessError("Invalid Max AF for indicator " + szName);
		return FALSE;
	}
	else if(paramDbl[2] <= 0){
		ProcessError("Invalid Min AF for indicator " + szName);
		return FALSE;
	}


	// Get the data
	CField* pHigh = SeriesToField("High", paramStr[0] + ".high", size);
	if(!EnsureField(pHigh, paramStr[0] + ".high")) return FALSE;
	CField* pLow = SeriesToField("Low", paramStr[0] + ".low", size);
	if(!EnsureField(pLow, paramStr[0] + ".low")) return FALSE;
	
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pHigh);
	pRS->addField(pLow);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.ParabolicSAR(pNav, pHigh, pLow, paramDbl[1], paramDbl[2], szName); // Changed 2/24/08
	//lineStyle = 2;

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".high");
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < 2){
			value = NULL_VALUE;
		}
		else{
			value = pInd->getValue(szName, n + 1);
		}
		jdate = series->data_master[n].jdate;
		AppendValue(jdate, value);
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

