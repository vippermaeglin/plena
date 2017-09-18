// IndicatorDirectionalMovementSystem.cpp: implementation of the CIndicatorDirectionalMovementSystem class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorDirectionalMovementSystem.h"
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

CIndicatorDirectionalMovementSystem::CIndicatorDirectionalMovementSystem(LPCTSTR name, int type, int members, CChartPanel* owner)
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
#ifdef _CONSOLE_DEBUG
	printf (" \n\n\n\n Visible = %d ", seriesVisible );
#endif
	nSpace = 0;

	// Resize param arrays for this indicator.
	// NOTE! Set all array sizes to max number of parameters.
	// ALL three arrays must be resized.
	paramCount = 7;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indDirectionalMovementSystem;

}

CIndicatorDirectionalMovementSystem::~CIndicatorDirectionalMovementSystem()
{
	CIndicator::OnDestroy();
}

void CIndicatorDirectionalMovementSystem::SetParamInfo(){
	
	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Periods (eg 14)
	  3. paramInt[2] = ColorR3 (eg 255)
	  4. paramInt[3] = ColorG3 (eg 255)
	  5. paramInt[4] = ColorB3 (eg 255)
	  6. paramInt[5] = Style3 (eg 0)
	  7. paramInt[6] = Thickness3 (eg 1)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptColorR3, "255");
	SetParam(3, ptColorG3, "255");
	SetParam(4, ptColorB3, "255");
	SetParam(3, ptStyle3, "0");
	SetParam(4, ptThickness3, "1");
}


BOOL CIndicatorDirectionalMovementSystem::Calculate()
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
	pInd = ta.DirectionalMovementSystem(pNav, pRS, paramInt[1]);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	

	CSeries* pDIPLUS = EnsureSeries(szName + " DI-");
	CSeries* pADX = EnsureSeries(szName + " ADX");
	pDIPLUS->szTitle = "DI-";
	pADX->szTitle = "ADX" + CString("("+paramInt[1]+")");
	//SetSeriesColor(szName + " DI+", RGB(255,0,0));
	SetSeriesSignal(szName + " DI-", lineColorSignal, lineStyleSignal, lineWeightSignal);
	//SetSeriesColor(szName + " DI-", RGB(0,0,255));
	SetSeriesSignal(szName + " ADX", RGB(paramInt[2],paramInt[3],paramInt[4]), paramInt[5], paramInt[6]);
	szTitle = "DI+";

	double adx = 0, adxr = 0, dip = 0, din = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
			dip = NULL_VALUE;
			din = NULL_VALUE;
		}
		else{
				dip = pInd->getValue("DI+", n + 1);
				din = pInd->getValue("DI-", n + 1);
		}
		if (n < paramInt[1] * 2 - 1){
			adx = NULL_VALUE;
		}
		else{
			adx = pInd->getValue("ADX", n + 1);
		}
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, dip);
		pCtrl->AppendNewValue(szName + " DI-", jdate, din);
		pCtrl->AppendNewValue(szName + " ADX", jdate, adx);
	}
 
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

