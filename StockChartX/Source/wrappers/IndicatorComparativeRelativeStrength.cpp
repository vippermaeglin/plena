// IndicatorComparativeRelativeStrength.cpp: implementation of the CIndicatorComparativeRelativeStrength class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorComparativeRelativeStrength.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorComparativeRelativeStrength::CIndicatorComparativeRelativeStrength(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 2;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indComparativeRelativeStrength;

}

CIndicatorComparativeRelativeStrength::~CIndicatorComparativeRelativeStrength()
{
	CIndicator::OnDestroy();
}

void CIndicatorComparativeRelativeStrength::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source1 (eg "msft.close")
	  2. paramStr[1] = Source2 (eg "aapl.volume")

	*/


	SetParam(0, ptSource1, "");
	SetParam(1, ptSource2, "");
	
}

BOOL CIndicatorComparativeRelativeStrength::Calculate()
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

//	Revision added 6/10/2004 By Katchei, for .net, use:
	// if(paramStr.size() < (unsigned int)paramCount)
	if((int)paramStr.size() < paramCount)
		return FALSE;


	if(paramStr[0] == paramStr[1]){
		ProcessError("Source 1 cannot be the same as Source 2");
		return FALSE;
	}


	// Get the data
	CField* pSource1 = SeriesToField("Source1", paramStr[0], size);	
	if(!EnsureField(pSource1, paramStr[0])) return FALSE;
	
	CField* pSource2 = SeriesToField("Source2", paramStr[1], size);	
	if(!EnsureField(pSource2, paramStr[1])) return FALSE;
	
 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource1);
	pRS->addField(pSource2);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	CIndex ta;
	pInd = ta.ComparativeRelativeStrength(pNav, pSource1, pSource2, szName);
	

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
