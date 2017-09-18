// IndicatorPriceVolumeTrend.cpp: implementation of the CIndicatorPriceVolumeTrend class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorPriceVolumeTrend.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorPriceVolumeTrend::CIndicatorPriceVolumeTrend(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indPriceVolumeTrend;

}

CIndicatorPriceVolumeTrend::~CIndicatorPriceVolumeTrend()
{
	CIndicator::OnDestroy();
}



void CIndicatorPriceVolumeTrend::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramStr[1] = Volume (eg "msft.volume")

	*/
	
	SetParam(0, ptSource, "");
	SetParam(1, ptVolume, "");

}


BOOL CIndicatorPriceVolumeTrend::Calculate()
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

 	if(paramStr[0] == paramStr[1]){
	//	ProcessError("Source 1 cannot be the same as Source 2\nSource 2 must be a volume series");
		return FALSE;
	}

	// Get the data
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	if(!EnsureField(pSource, paramStr[0])) return FALSE;
	CField* pVolume = SeriesToField("Volume", paramStr[1], size);
	if(!EnsureField(pVolume, paramStr[1])) return FALSE;
	 
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);
	pRS->addField(pVolume);

	pNav->setRecordset(pRS);


	// Calculate the indicator
	CIndex ta;
	pInd = ta.PriceVolumeTrend(pNav, pSource, pVolume, szName);

	if(pCtrl->m_Language==1){
		szTitle = "TVP";
	}
	else{
		szTitle = "PVT";
	}	

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

