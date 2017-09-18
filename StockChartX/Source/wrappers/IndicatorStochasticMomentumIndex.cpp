// IndicatorStochasticMomentumIndex.cpp: implementation of the CIndicatorStochasticMomentumIndex class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "IndicatorStochasticMomentumIndex.h"

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

CIndicatorStochasticMomentumIndex::CIndicatorStochasticMomentumIndex(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 7;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indStochasticMomentumIndex;

}

CIndicatorStochasticMomentumIndex::~CIndicatorStochasticMomentumIndex()
{
	CIndicator::OnDestroy();
}


void CIndicatorStochasticMomentumIndex::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = %KPeriods (eg 5)
	  3. paramInt[2] = %KSmooth (eg 3)
	  4. paramInt[3] = %ptPctKDblSmooth (eg 3)
	  5. paramInt[4] = %DPeriods (eg 3)
	  6. paramInt[5] = MAType (eg indSimpleMovingAverage)
	  7. paramInt[6] = PctD_MAType (eg indSimpleMovingAverage)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptPctKPeriods, "13");
	SetParam(2, ptPctKSmooth, "25");
	SetParam(3, ptPctKDblSmooth, "2");
	SetParam(4, ptPctDPeriods, "9");
	SetParam(5, ptMAType, "Exponential");
	SetParam(6, ptPctDMAType, "Exponential");

}


BOOL CIndicatorStochasticMomentumIndex::Calculate()
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
	if(paramStr.size() < (unsigned int)paramCount)
		return FALSE;
	if(paramInt.size() < (unsigned int)paramCount)
		return FALSE;

	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid %K Periods for indicator " + szName);
		return FALSE;
	}
 	else if(paramInt[2] < 1 || paramInt[2] > size / 2){
		//ProcessError("Invalid %K Smoothing Periods for indicator " + szName);
		return FALSE;
	}
 	else if(paramInt[3] < 1 || paramInt[3] > size / 2){
		//ProcessError("Invalid %K Double Smoothing Periods for indicator " + szName);
		return FALSE;
	}
 	else if(paramInt[4] < 1 || paramInt[4] > size / 2){
		//ProcessError("Invalid %D Periods for indicator " + szName);
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
	CIndex ta;
	pInd = ta.StochasticMomentumIndex(pNav, pRS, paramInt[1], paramInt[2], paramInt[3],
										paramInt[4], paramInt[5], paramInt[6]);


	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	CSeries* pSignalK=EnsureSeries(szName + " %K");
	SetSeriesColor(szName + " %K", RGB(0,0,255));
	SetSeriesSignal(szName + " %K",lineColorSignal,lineStyleSignal,lineWeightSignal);
	


	if(pCtrl->m_Language==1){
		szTitle = "ME %D" +CString("("+paramInt[4]+")");
		pSignalK->szTitle="ME %K"+CString("("+paramInt[1]+")");
	}
	else{
		szTitle = "SMI %D" +CString("("+paramInt[4]+")");
		pSignalK->szTitle="SMI %K"+CString("("+paramInt[1]+")");
	}

	double pctd = 0, pctk = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		pctd = pInd->getValue("%D", n + 1);
		pctk = pInd->getValue("%K", n + 1);
		if(n < paramInt[1] * 2){			
			pctd = pctk = NULL_VALUE;			
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
