// IndicatorHILOActivator.cpp: implementation of the CIndicatorHILOActivator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "IndicatorHILOActivator.h"

#include "tasdk.h"



#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif
//#define _CONSOLE_DEBUG

#ifdef _CONSOLE_DEBUG
#include "julian.h"
#endif
//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorHILOActivator::CIndicatorHILOActivator(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indHILOActivator;

}

CIndicatorHILOActivator::~CIndicatorHILOActivator()
{
	CIndicator::OnDestroy();
}

void CIndicatorHILOActivator::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = PeriodsHigh (eg 3)
	  3. paramInt[2] = PeriodsLow (eg 3)
	  4. paramInt[3] = Shift (eg 1)
	  5. paramDbl[4] = %Scale (eg 0.0)

	*/
	
	SetParam(0, ptSymbol, "");
	SetParam(1, ptPeriods, "3");
	SetParam(2, ptPeriods, "3");
	SetParam(1, ptPeriods, "3");
	SetParam(3, ptShift, "1");
	SetParam(4, ptR2Scale,"0.00");

}

BOOL CIndicatorHILOActivator::Calculate()
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
	if( paramInt[1] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}
	if (paramDbl[4] <0 || paramDbl[4] > 100){
		//ProcessError("Invalid Periods for indicator " + szName);
		paramDbl[4] = 0;
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
	pInd = ta.HILOBands(pNav, pHigh, pLow, pClose, paramInt[1], paramInt[2], paramInt[3], paramDbl[4]);


	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	CSeries* pTop=EnsureSeries(szName + " Top");
	CSeries* pBottom=EnsureSeries(szName + " Bottom");
	if(pCtrl->m_Language==1){
		pTop->szTitle = CString("HILO("+paramInt[1]+","+paramInt[2]+","+paramInt[3]+")")+" Superior";
		pBottom->szTitle = CString("HILO("+paramInt[1]+","+paramInt[2]+","+paramInt[3]+")")+" Inferior";
	}
	else{
		pTop->szTitle = CString("HILO("+paramInt[1]+","+paramInt[2]+","+paramInt[3]+")")+" Top";
		pBottom->szTitle = CString("HILO("+paramInt[1]+","+paramInt[2]+","+paramInt[3]+")")+" Bottom";
	}
	SetSeriesSignal(szName + " Top",lineColor,lineStyle,lineWeight);
	SetSeriesSignal(szName + " Bottom",lineColorSignal,lineStyle,lineWeight);
	double top = 0, median = 0, bottom = 0, jdate = 0;	
	int state = 0; // 0 = NULL | 1 = Show Top | 2 = Show Bottom
	for(int n = 0; n < size; ++n){

		//High Band:
		if(n > paramInt[1]){
			top = pInd->getValue("HILO Bands Top", n + 1);
			if(state!=2){
				if(series->data_master[n].value>top) state=2; 
			}
		}
		
		//Low Band:
		if(n > paramInt[2]){
			bottom = pInd->getValue("HILO Bands Bottom", n + 1);
			if(state!=1){
				if(series->data_master[n].value<bottom) state=1; 
			}
		}
		if (state != 1) { 
			top = NULL_VALUE; 
			bottom = bottom + paramDbl[4] * bottom / 100;
		}
		if (state != 2) { 
			bottom = NULL_VALUE;
			top = top + paramDbl[4] * top / 100;
		}

		jdate = pCtrl->panels[0]->series[0]->data_master[n-1].jdate;
		//this->AppendValue(jdate, median);
		pCtrl->AppendNewValue(szName + " Top", jdate, top);
		pCtrl->AppendNewValue(szName + " Bottom", jdate, bottom);



	}
 

	// Clean up	
	delete pRS;
	delete pInd;
	delete pNav;


	return CIndicator::Calculate();
}
