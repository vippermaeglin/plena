// IndicatorMovingAverageEnvelope.cpp: implementation of the CIndicatorMovingAverageEnvelope class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "IndicatorMovingAverageEnvelope.h"

#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorMovingAverageEnvelope::CIndicatorMovingAverageEnvelope(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indMovingAverageEnvelope;

}

CIndicatorMovingAverageEnvelope::~CIndicatorMovingAverageEnvelope()
{
	CIndicator::OnDestroy();
}

void CIndicatorMovingAverageEnvelope::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)	  
	  3. paramInt[2] = Moving Average Type (eg indSimpleMovingAverage)
	  4. paramDbl[3] = Shift (eg 5%)

	*/

	SetParam(0, ptSource, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptMAType, "Simple");
	SetParam(3, ptShift, "5");

}


BOOL CIndicatorMovingAverageEnvelope::Calculate()
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
	if(paramInt.size() < (unsigned int)paramCount) return FALSE;

	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < MA_START || paramInt[2] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
		return FALSE;
	}
	else if(paramDbl[3] < 0 || paramDbl[3] > 100){
		//ProcessError("Invalid Band Shift Percentage for indicator " + szName);
		return FALSE;
	}
	
 
	// Get the data
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	if(!EnsureField(pSource, paramStr[0])) return FALSE;
	 
	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);	

	pNav->setRecordset(pRS);


	// Calculate the indicator
	CBands ta;
	pInd = ta.MovingAverageEnvelope(pNav, pSource, paramInt[1], paramInt[2], paramDbl[3]);


	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	
	CSeries* sTop = EnsureSeries(szName + " Top");
	sTop->szTitle = "MAE"+"("+paramInt[1]+","+paramDbl[3]+")"+(pCtrl->m_Language==1?" Topo":" Top");
	//SetSeriesColor(szName + " Top", lineColor);
	SetSeriesSignal(szName + " Top",lineColor,lineStyle,lineWeight);
	//szTitle = szName + " Bottom";

	double top = 0, bottom = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < (paramInt[1] - 1) /* * 2*/){
			top = NULL_VALUE;
			bottom = NULL_VALUE;
		}
		else{
			top = pInd->getValue("Envelope Top", n + 1);			
			bottom = pInd->getValue("Envelope Bottom", n + 1);
		}
		jdate = series->data_master[n].jdate;		
		pCtrl->AppendNewValue(szName + " Top", jdate, top);
		this->AppendValue(jdate, bottom);
	}
 

	// Clean up	
	delete pRS;
	delete pInd;
	delete pNav;


	return CIndicator::Calculate();
}
