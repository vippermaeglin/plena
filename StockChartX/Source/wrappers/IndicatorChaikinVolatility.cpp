// IndicatorChaikinVolatility.cpp: implementation of the CIndicatorChaikinVolatility class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorChaikinVolatility.h"
#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorChaikinVolatility::CIndicatorChaikinVolatility(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indChaikinVolatility;

}

CIndicatorChaikinVolatility::~CIndicatorChaikinVolatility()
{
	CIndicator::OnDestroy();
}


void CIndicatorChaikinVolatility::SetParamInfo()
{
	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Symbol (eg "msft")
	  2. paramInt[1] = Periods (eg 14)
	  3. paramInt[2] = Rate of Change (eg 2)
	  4. paramInt[3] = Moving Average Type (eg indSimpleMovingAverage)

	*/

	SetParam(0, ptSymbol, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptRateOfChange, "2");
	SetParam(3, ptMAType, "Simple");

}
	



BOOL CIndicatorChaikinVolatility::Calculate()
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
//	Addition of type cast (unsigned int)
	if(paramStr.size() < (unsigned int)paramCount) return FALSE;
//	End Of Revision

	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] <= 0){
		//ProcessError("Invalid Rate of Change for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] < MA_START || paramInt[3] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
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
	pInd = ta.ChaikinVolatility(pNav, pRS, paramInt[1], paramInt[2], paramInt[3], szName);
	if(pCtrl->m_Language==1){
		szTitle = "VC"+ CString ("("+paramInt[1]+","+paramInt[2]+")");
	}
	else{
		szTitle = "CV" +CString("("+paramInt[1]+","+paramInt[2]+")");
	}	

	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0] + ".close");
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

