// IndicatorMovingAverage.cpp: implementation of the CIndicatorMovingAverage class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorGenericMovingAverage.h"
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

CIndicatorGenericMovingAverage::CIndicatorGenericMovingAverage(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indGenericMovingAverage;

}

CIndicatorGenericMovingAverage::~CIndicatorGenericMovingAverage()
{
	CIndicator::OnDestroy();
}


void CIndicatorGenericMovingAverage::SetParamInfo(){
	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)
	  3. paramInt[2] = Shift (eg 14)
	  4. paramInt[3] = Moving Average Type (eg Simple)
	  5. paramDbl[4] = R2 Scale (0.1 to 0.95)

	*/
	
	SetParam(0, ptSource, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptShift, "14");
	SetParam(3, ptMAType, "Simple");
	SetParam(4, ptR2Scale,"0.65");

}


BOOL CIndicatorGenericMovingAverage::Calculate()
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


	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
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

#ifdef _CONSOLE_DEBUG
	printf("\nCalculate %s Type = %d",szName,paramInt[3]);
#endif

	// Calculate the indicator
	CMovingAverage ta;
	switch(paramInt[3])
	{
		case (int)indExponentialMovingAverage:
			pInd = ta.ExponentialMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			break;
		case (int)indTimeSeriesMovingAverage:
			pInd = ta.TimeSeriesMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			break;
		case (int)indVariableMovingAverage:
			pInd = ta.VariableMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			break;
		case (int)indTriangularMovingAverage:
			pInd = ta.TriangularMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			break;
		case (int)indWeightedMovingAverage:
			pInd = ta.WeightedMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			break;
		case (int)indVIDYA:
			pInd = ta.VIDYA(pNav, pSource, paramInt[1], paramDbl[4], szName, paramInt[2]);
			break;
		case (int)indWellesWilderSmoothing:
			pInd = ta.WellesWilderSmoothing(pSource, paramInt[1], szName, paramInt[2]);
			break;
		default:
			pInd = ta.SimpleMovingAverage(pNav, pSource, paramInt[1], szName, paramInt[2]);
			//pInd = ta.WellesWilderSmoothing(pSource, paramInt[1], szName, paramInt[2]);
			break;
	}
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size  ; ++n){
		if(n < paramInt[1] + paramInt[2] - 1){
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

