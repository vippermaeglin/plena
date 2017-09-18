// IndicatorRelativeStrengthIndex.cpp: implementation of the CIndicatorRelativeStrengthIndex class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorRelativeStrengthIndex.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorRelativeStrengthIndex::CIndicatorRelativeStrengthIndex(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indRelativeStrengthIndex;

}

CIndicatorRelativeStrengthIndex::~CIndicatorRelativeStrengthIndex()
{
	CIndicator::OnDestroy();
}


void CIndicatorRelativeStrengthIndex::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)
	  2. paramDbl[2] = Threshold Min (eg 30)
	  2. paramDbl[3] = Threshold Max (eg 70)

	*/
	
	SetParam(0, ptSource, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptThreshold1, "30.0");
	SetParam(3, ptThreshold2, "70.0");

}


BOOL CIndicatorRelativeStrengthIndex::Calculate()
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


	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)
	  2. paramDbl[2] = Threshold Min (eg 30)
	  2. paramDbl[3] = Threshold Max (eg 70)

	*/
	
 
	// Get the data
	CField* pSource = SeriesToField("Source", paramStr[0], size);	
	CSeries* p30 = EnsureSeries(szName + " 30");
	SetSeriesSignal(szName + " 30", lineColorSignal, lineStyleSignal, lineWeightSignal);
	CSeries* p70 = EnsureSeries(szName + " 70");
	SetSeriesSignal(szName + " 70", lineColorSignal, lineStyleSignal, lineWeightSignal);
	if(!EnsureField(pSource, paramStr[0])) return FALSE;
	
	if(pCtrl->m_Language==1){
		szTitle = "IFR" +CString("("+paramInt[1]+")");
		p30->szTitle="IFR "+CString(paramDbl[2]+"%");
		p70->szTitle="IFR "+CString(paramDbl[3]+"%");
	}
	else{
		szTitle = "RSI" +CString("("+paramInt[1]+")");
		p30->szTitle="RSI "+CString(paramDbl[2]+"%");
		p70->szTitle="RSI "+CString(paramDbl[3]+"%");
	}

 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pSource);
	pNav->setRecordset(pRS);


	// Calculate the indicator
	CIndex ta;
	pInd = ta.RelativeStrengthIndex(pNav, pSource, paramInt[1], szName);
	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	double value = 0, jdate = 0;
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1] + 1 || value == 0){
			value = NULL_VALUE;
		}
		else{
			value = pInd->getValue(szName, n + 1);
		}
		jdate = series->data_master[n].jdate;
		AppendValue(jdate, value);
		//if(n != 0)
		//{
			pCtrl->AppendNewValue(szName + " 30", jdate, paramDbl[2]);
			pCtrl->AppendNewValue(szName + " 70", jdate, paramDbl[3]);
		//}
		//else
		//{
		//	pCtrl->AppendNewValue(szName + " 30", jdate, 0);
		//	pCtrl->AppendNewValue(szName + " 70", jdate, 100);
		//}
	}
 

	// Clean up
	delete pRS;
	delete pInd;	
	delete pNav;


	return CIndicator::Calculate();
}

