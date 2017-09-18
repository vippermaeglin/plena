// IndicatorAroon.cpp: implementation of the CIndicatorAroon class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorAroon.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorAroon::CIndicatorAroon(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 2;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indAroon;	

}

CIndicatorAroon::~CIndicatorAroon()
{
	CIndicator::OnDestroy();
}


void CIndicatorAroon::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Periods (eg 14)

	*/
	
	SetParam(0, ptSymbol, "");
	SetParam(1, ptPeriods, "14");

}


BOOL CIndicatorAroon::Calculate()
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
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	if(paramStr.size() < (unsigned int)paramCount)
//	End Of Revision
		return FALSE;


	if(paramInt[1] < 1 || paramInt[1] > size / 2){
	//	ProcessError("Invalid Periods for indicator " + szName);
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
	pInd = ta.Aroon(pNav, pRS, paramInt[1]);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
	
	CSeries* pAroonDown = GetSeries(szName + " Down");
	if(!pAroonDown) pCtrl->AddNewSeries(szName + " Down", OBJECT_SERIES_LINE, ownerPanel->index);		
	if(!pAroonDown) pAroonDown = GetSeries(szName + " Down");
	if(pAroonDown){
		pAroonDown->Clear();
		pAroonDown->linkTo = szName;
		pAroonDown->isTwin = true;
	}
	
	SetSeriesSignal(szName + " Down", lineColorSignal, lineStyleSignal, lineWeightSignal);
	pAroonDown->szTitle = "Aroon Down";
	szTitle = "Aroon Up" + CString ("(" + paramInt[1] +")");

	double top = 0, bottom = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
			top = NULL_VALUE;			
			bottom = NULL_VALUE;
		}
		else{
			top = pInd->getValue("Aroon Up", n + 1);			
			bottom = pInd->getValue("Aroon Down", n + 1);
		}
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, top);
		pCtrl->AppendNewValue(szName + " Down", jdate, bottom);
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;

	return CIndicator::Calculate();


}

