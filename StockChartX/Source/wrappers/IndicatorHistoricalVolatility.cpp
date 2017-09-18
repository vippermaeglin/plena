// IndicatorHistoricalVolatility.cpp: implementation of the CIndicatorHistoricalVolatility class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorHistoricalVolatility.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define _CONSOLE_DEBUG

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorHistoricalVolatility::CIndicatorHistoricalVolatility(LPCTSTR name, int type, int members, CChartPanel* owner)
{
#ifdef _CONSOLE_DEBUG
	printf("\nCIndicatorHistoricalVolatility()");
#endif
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
	paramCount = 4;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indHistoricalVolatility;

}

CIndicatorHistoricalVolatility::~CIndicatorHistoricalVolatility()
{
	CIndicator::OnDestroy();
}


void CIndicatorHistoricalVolatility::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)
	  3. paramInt[2] = Bar History (eg 365)
	  4. paramDbl[3] = Standard Deviations (eg 2)

	*/
	
	SetParam(0, ptSource, "");
	SetParam(1, ptPeriods, "30");
	SetParam(2, ptBarHistory, "365");
	SetParam(3, ptStandardDeviations, "2");

}


BOOL CIndicatorHistoricalVolatility::Calculate()
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


	if(paramInt[1] < 1 || paramInt[1] > size ){
#ifdef _CONSOLE_DEBUG
		printf("\nInvalid Periods for indicator %s paramInt[1]=%d",szName,paramInt[1]);
#endif
		ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}

	
	/*if(paramInt[2] < 1){
		ProcessError("Invalid Bar History for indicator " + szName);
		return FALSE;
	}*/

	if(paramInt[3] < 0){
		ProcessError("Invalid Standard Deviations for indicator " + szName);
		return FALSE;
	}

	if (pCtrl->m_Language == 1){
		szTitle = CString("VH(" + paramInt[1] + "," + paramDbl[3] + ")");
	}
	else{
		szTitle = CString("HV(" + paramInt[1] + "," + paramDbl[3] + ")");
	}

	// Get the data
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	if(!EnsureField(pSource, paramStr[0])) return FALSE;

 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);
	pNav->setRecordset(pRS);


	/*Fixed BarHistory:
	Day = 252/Bars
	Week = 52/Bars
	Month = 12/Bars
	Year = 1/Bars
	*/

	int BarHistory = 252;
	switch (ownerPanel->pCtrl->GetPeriodicity()){
	case Minutely:
		BarHistory = 252 * ownerPanel->pCtrl->GetBarSize();
		break;
	case Hourly:
		BarHistory = 252/60 * ownerPanel->pCtrl->GetBarSize();
		break;
	case Daily:
		BarHistory = 252 / ownerPanel->pCtrl->GetBarSize();
		break;
	case Weekly:
		BarHistory = 52 / ownerPanel->pCtrl->GetBarSize();
		break;
	case Month:
		BarHistory = 12 / ownerPanel->pCtrl->GetBarSize();
		break;
	case Year:
		BarHistory = 1 / ownerPanel->pCtrl->GetBarSize();
		break;
	}

	// Calculate the indicator
	CIndex ta;
	pInd = ta.HistoricalVolatility(pNav, pSource, paramInt[1], BarHistory, paramInt[2], paramDbl[3], szName);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
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

