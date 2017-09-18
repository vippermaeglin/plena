// IndicatorPrimeNumberBands.cpp: implementation of the CIndicatorPrimeNumberBands class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorPrimeNumberBands.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorPrimeNumberBands::CIndicatorPrimeNumberBands(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 1;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indPrimeNumberBands;

}

CIndicatorPrimeNumberBands::~CIndicatorPrimeNumberBands()
{
	CIndicator::OnDestroy();
}

void CIndicatorPrimeNumberBands::SetParamInfo()
{
	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")	  

	*/

	SetParam(0, ptSymbol, "");	

}

BOOL CIndicatorPrimeNumberBands::Calculate()
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


	// Get user input
	if(!GetUserInput()) return FALSE;


	// Validate
	long size = pCtrl->RecordCount();
	if(size == 0) return FALSE;
	if(paramStr.size() < (unsigned int)paramCount)
		return FALSE;

 
	// Get the data
	CField* pHigh = SeriesToField("High", paramStr[0] + ".high", size);
	if(!EnsureField(pHigh, paramStr[0] + ".high")) return FALSE;
	CField* pLow = SeriesToField("Low", paramStr[0] + ".low", size);
	if(!EnsureField(pLow, paramStr[0] + ".low")) return FALSE;

 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pHigh);
	pRS->addField(pLow);	

	pNav->setRecordset(pRS);


	// Calculate the indicator
	CBands ta;
	pInd = ta.PrimeNumberBands(pNav, pHigh, pLow);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".high");
	if(!series) return FALSE;

	
	CSeries* pBottom = EnsureSeries(szName + " Bottom");
	//SetSeriesColor(szName + " Bottom", lineColor);
	SetSeriesSignal(szName + " Bottom",lineColor,lineStyle,lineWeight);
	
	szTitle = szName + " Top";


	double top = 0, bottom = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		top = pInd->getValue("Prime Bands Top", n + 1);
		bottom = pInd->getValue("Prime Bands Bottom", n + 1);
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, top);
		pCtrl->AppendNewValue(szName + " Bottom", jdate, bottom);		
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

