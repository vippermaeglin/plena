// IndicatorMACD.cpp: implementation of the CIndicatorMACD class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorMACD.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorMACD::CIndicatorMACD(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	szName = name;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	seriesType = type;
	memberCount = members;
	lineColorSignal = RGB(255,0,0);
	lineStyleSignal = 0;
	lineWeightSignal = 1;
	Initialize();
	nSpace = 0;

	// Resize param arrays for this indicator.
	// NOTE! Set all array sizes to max number of parameters.
	// ALL three arrays must be resized.
	paramCount = 4;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indMACD;

}

CIndicatorMACD::~CIndicatorMACD()
{
	CIndicator::OnDestroy();
}


void CIndicatorMACD::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Long Cycle (eg 26)
	  3. paramInt[2] = Short Cycle (eg 13)
	  4. paramInt[3] = Signal Periods (eg 9)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptLongCycle, "26");
	SetParam(2, ptShortCycle, "13");
	SetParam(3, ptSignalPeriods, "9");

}

BOOL CIndicatorMACD::Calculate()
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

	// 9/20/07
	if(paramInt[1] > 500) paramInt[1] = 500;
	if(paramInt[2] > 500) paramInt[2] = 500;
	if(paramInt[1] < 0) paramInt[1] = 0;
	if(paramInt[2] < 0) paramInt[2] = 0;
	if(paramInt[3] > 500) paramInt[3] = 500;
	if(paramInt[3] < 0) paramInt[3] = 0;

	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid Long Cycle for indicator " + szName); // 3/1/08
		return FALSE;
	}

	if(paramInt[2] == paramInt[3]){
		//ProcessError("Signal Periods cannot be the same as Short Cycle");
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
	
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pOpen);
	pRS->addField(pHigh);
	pRS->addField(pLow);
	pRS->addField(pClose);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.MACD(pNav, pRS, paramInt[3], paramInt[1], paramInt[2], szName);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	CSeries* pSignal=EnsureSeries(szName + " Signal");	
	SetSeriesSignal(szName + " Signal", lineColorSignal, lineStyleSignal, lineWeightSignal);
	pSignal->szTitle="MACD"+CString("("+paramInt[3]+")");
	//szTitle = "MACD";
	double signal = 0, macd = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
			macd = NULL_VALUE;
		}
		else{
			macd = pInd->getValue(szName, n + 1);
		}

		//if(n < paramInt[1] + paramInt[2]){ // Updated 2/21/08
		if(n < paramInt[1] * 2.25){
			signal = NULL_VALUE;
		}
		else{
			signal = pInd->getValue(szName + "Signal", n + 1);
		}
		
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, macd);		
		pCtrl->AppendNewValue(szName + " Signal", jdate, signal);
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

