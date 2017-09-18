// IndicatorPrimeNumberOscillator.cpp: implementation of the CIndicatorPrimeNumberOscillator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorPrimeNumberOscillator.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorPrimeNumberOscillator::CIndicatorPrimeNumberOscillator(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indPrimeNumberOscillator;

}

CIndicatorPrimeNumberOscillator::~CIndicatorPrimeNumberOscillator()
{
	CIndicator::OnDestroy();
}

void CIndicatorPrimeNumberOscillator::SetParamInfo()
{
	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")	  

	*/

	SetParam(0, ptSource, "");	

}

BOOL CIndicatorPrimeNumberOscillator::Calculate()
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
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	if(!EnsureField(pSource, paramStr[0])) return FALSE;
	
 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.PrimeNumberOscillator(pNav, pSource, szName);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		value = pInd->getValue(szName, n + 1);
		jdate = series->data_master[n].jdate;
		AppendValue(jdate, value);
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

