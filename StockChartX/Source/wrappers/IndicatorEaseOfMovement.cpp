// IndicatorEaseOfMovement.cpp: implementation of the CIndicatorEaseOfMovement class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorEaseOfMovement.h"
#include "tasdk.h"


//#define _CONSOLE_DEBUG
#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorEaseOfMovement::CIndicatorEaseOfMovement(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	szName = name;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	seriesType = type;
	memberCount = members;
	Initialize();
	nSpace = 0;
#ifdef _CONSOLE_DEBUG
		printf("\n\n\n\t paramStr[1] = %s", name);
#endif
	// Resize param arrays for this indicator.
	// NOTE! Set all array sizes to max number of parameters.
	// ALL three arrays must be resized.
	paramCount = 4;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indEaseOfMovement;


}

CIndicatorEaseOfMovement::~CIndicatorEaseOfMovement()
{
	CIndicator::OnDestroy();
}


void CIndicatorEaseOfMovement::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramStr[1] = Volume (eg "msft.volume")
	  3. paramInt[2] = Periods (eg 14)
	  4. paramInt[3] = Moving Average Type (eg indSimpleMovingAverage)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptVolume, "");
	SetParam(2, ptPeriods, "8");
	SetParam(3, ptMAType, "Simple");

}

BOOL CIndicatorEaseOfMovement::Calculate()
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

	if(paramInt[2] < 1 || paramInt[2] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] < MA_START || paramInt[3] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
		return FALSE;
	}

	
	// Get the data
	CField* pOpen = SeriesToField("Open", paramStr[0] + ".open", size);
	if(!EnsureField(pOpen, paramStr[0] + ".open")) return FALSE;
	CField* pHigh = SeriesToField("High", paramStr[0] + ".high", size);
	if(!EnsureField(pHigh, paramStr[0] + ".high")) return FALSE;
	CField* pLow = SeriesToField("Low", paramStr[0] + ".low", size);
	if(!EnsureField(pLow, paramStr[0] + ".low")) return FALSE;
	CField* pClose = SeriesToField("Close", paramStr[0] + ".close", size);	
	if(!EnsureField(pClose, paramStr[0] + ".close")) return FALSE;
	CField* pVolume = SeriesToField("Volume", paramStr[1], size);	
	if(!EnsureField(pVolume, paramStr[1])) return FALSE;
	
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pHigh);
	pRS->addField(pLow);
	pRS->addField(pClose);
	pRS->addField(pVolume);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.EaseOfMovement(pNav, pRS, paramInt[2], paramInt[3], szName);
	szTitle = "EOM"+CString("("+paramInt[2]+")");
	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".high");
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < paramInt[2]){
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

