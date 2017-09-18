// IndicatorUltimateOscillator.cpp: implementation of the CIndicatorUltimateOscillator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorUltimateOscillator.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorUltimateOscillator::CIndicatorUltimateOscillator(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 4;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indUltimateOscillator;

}

CIndicatorUltimateOscillator::~CIndicatorUltimateOscillator()
{
	CIndicator::OnDestroy();
}


void CIndicatorUltimateOscillator::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Cycle 1 (eg 7)
	  3. paramInt[2] = Cycle 2 (eg 14)
	  4. paramInt[3] = Cycle 3 (eg 28)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptCycle1, "7");
	SetParam(2, ptCycle2, "14");
	SetParam(3, ptCycle3, "28");

	

}

BOOL CIndicatorUltimateOscillator::Calculate()
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


	if(paramInt[1] < 1){
		//ProcessError("Invalid Cycle 1 for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < 1){
		//ProcessError("Invalid Cycle 2 for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] < 1){
		//ProcessError("Invalid Cycle 3 for indicator " + szName);
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
	COscillator ta;
	pInd = ta.UltimateOscillator(pNav, pRS, paramInt[1], paramInt[2], paramInt[3], szName);
	
	szTitle = "UO" + CString("("+paramInt[1]+","+paramInt[2]+","+paramInt[3]+")");

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	double value = 0, jdate = 0;
	int max = paramInt[1];
	if(paramInt[2] > max) max = paramInt[2];
	if(paramInt[3] > max) max = paramInt[3];
	for(int n = 0; n < size; ++n){
		if(n < max){
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

