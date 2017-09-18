// IndicatorStochasticOscillator.cpp: implementation of the CIndicatorStochasticOscillator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorStochasticOscillator.h"
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

CIndicatorStochasticOscillator::CIndicatorStochasticOscillator(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 5;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indStochasticOscillator;

}

CIndicatorStochasticOscillator::~CIndicatorStochasticOscillator()
{
	CIndicator::OnDestroy();
}

void CIndicatorStochasticOscillator::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = %K Periods (eg 14)
	  3. paramInt[2] = %K Slowing (eg 3)
	  4. paramInt[3] = %D Periods (eg 5)
	  5. paramInt[4] = Moving Average Type (eg indSimpleMovingAverage)

	*/
	
	SetParam(0, ptSymbol, "");
	SetParam(1, ptPctKPeriods, "9");
	SetParam(2, ptPctKSlowing, "3");
	SetParam(3, ptPctDPeriods, "9");
	SetParam(4, ptMAType, "Simple");

}

BOOL CIndicatorStochasticOscillator::Calculate()
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
		//ProcessError("Invalid %K Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < 1 || paramInt[2] > size / 2){
		//ProcessError("Invalid %K Slowing Periods (min 1) for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] <= 0 || paramInt[3] > size / 2){
		//ProcessError("Invalid %D Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[4] < MA_START || paramInt[4] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
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

	pInd = ta.StochasticOscillator(pNav, pRS, paramInt[1], paramInt[2], paramInt[3], paramInt[4]);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	CSeries* pSignalK=EnsureSeries(szName + " %K");
	SetSeriesSignal(szName + " %K",lineColorSignal,lineStyleSignal,lineWeightSignal);

#ifdef _CONSOLE_DEBUG
	printf("\n\n\tszName = %s, szTitle = %s", pSignalK->szName, pSignalK->szTitle);
#endif

	
	if(pCtrl->m_Language==1){
		szTitle = "OE %D"+ CString ("("+paramInt[3]+")");
		pSignalK->szTitle="OE %K"+CString("("+paramInt[1]+")");
	}
	else{
		szTitle = "SO %D" +CString("("+paramInt[3]+")");
		pSignalK->szTitle="SO %K"+CString("("+paramInt[1]+")");
	}

	double pctd = 0, pctk = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		pctd = pInd->getValue("%D", n + 1);
		pctk = pInd->getValue("%K", n + 1);
		if(n < paramInt[1] || pctk <= 0 || pctk > 100){ // 9/5/08
			pctk = NULL_VALUE;
		}
		if(n < paramInt[3] || pctd <= 0 || pctd > 100){ // 9/5/08
			pctd = NULL_VALUE;		
		}
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, pctd);
		pCtrl->AppendNewValue(szName + " %K", jdate, pctk);		
	}
 
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

