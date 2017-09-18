// IndicatorRainbowOscillator.cpp: implementation of the CIndicatorRainbowOscillator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorRainbowOscillator.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorRainbowOscillator::CIndicatorRainbowOscillator(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indRainbowOscillator;

}

CIndicatorRainbowOscillator::~CIndicatorRainbowOscillator()
{
	CIndicator::OnDestroy();
}


void CIndicatorRainbowOscillator::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Levels (eg 9)
	  3. paramInt[2] = Moving Average Type (eg indSimpleMovingAverage)

	*/
	
	SetParam(0, ptSource, "");
	SetParam(1, ptLevels, "9");
	SetParam(2, ptMAType, "Simple");

}


BOOL CIndicatorRainbowOscillator::Calculate()
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


	if(paramInt[1] < 2 || paramInt[1] > size / 2){
		//ProcessError("Invalid Level (min 2) for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < MA_START || paramInt[2] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
		return FALSE;
	}

 
	// Get the data
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	if(!EnsureField(pSource, paramStr[0])) return FALSE;
	
 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);
	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.RainbowOscillator(pNav, pSource, paramInt[1], paramInt[2], szName);
	
	szTitle = "RO" + CString("("+paramInt[1] +")");

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
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

