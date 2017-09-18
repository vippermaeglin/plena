// IndicatorVolumeOscillator.cpp: implementation of the CIndicatorVolumeOscillator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorVolumeOscillator.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorVolumeOscillator::CIndicatorVolumeOscillator(LPCTSTR name, int type, int members, CChartPanel* owner)
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
	paramCount = 4;
	paramStr.resize(paramCount);
	paramDbl.resize(paramCount);
	paramInt.resize(paramCount);

	indicatorType = indVolumeOscillator;

}

CIndicatorVolumeOscillator::~CIndicatorVolumeOscillator()
{
	CIndicator::OnDestroy();
}

void CIndicatorVolumeOscillator::SetParamInfo(){

	/*  Required inputs for this indicator:
		  
	  1. paramStr[0] = Volume (eg "msft.volume")
	  2. paramInt[1] = Short Term Period (eg 9)
	  3. paramInt[2] = Long Term Period (eg 21)	  
	  4. paramInt[3] = Points or Percent (1 for points or 2 for percent)

	*/

	SetParam(0, ptVolume, "");
	SetParam(1, ptShortTerm, "9");
	SetParam(2, ptLongTerm, "21");	
	SetParam(3, ptPointsOrPercent, "Points");

}

BOOL CIndicatorVolumeOscillator::Calculate()
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


	if(paramInt[1] > paramInt[2] || paramInt[1] < 1){
		ProcessError("Invalid Short Term Period for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < 8){
		ProcessError("Invalid Long Term Period (min 8) for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] != 1 && paramInt[3] != 2){
		ProcessError("Invalid Points or Percent for indicator " + szName);
		return FALSE;
	}	
	
 
	// Get the data
	CField* pVolume = SeriesToField("Volume", paramStr[0], size);
	if(!EnsureField(pVolume, paramStr[0])) return FALSE;
	
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pVolume);
	pNav->setRecordset(pRS);


	// Calculate the indicator
	COscillator ta;
	pInd = ta.VolumeOscillator(pNav, pVolume, paramInt[1], paramInt[2], paramInt[3], szName);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < paramInt[2]){
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

