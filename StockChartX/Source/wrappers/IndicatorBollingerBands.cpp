// IndicatorBollingerBands.cpp: implementation of the CIndicatorBollingerBands class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "IndicatorBollingerBands.h"

#include "tasdk.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorBollingerBands::CIndicatorBollingerBands(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indBollingerBands;

}

CIndicatorBollingerBands::~CIndicatorBollingerBands()
{
	CIndicator::OnDestroy();
}


void CIndicatorBollingerBands::SetParamInfo(){

	/*  Required inputs for this indicator:
	
	  1. paramStr[0] = Source (eg "msft.close")
	  2. paramInt[1] = Periods (eg 14)
	  3. paramDbl[2] = Standard Deviations (eg 2)
	  4. paramInt[3] = Moving Average Type (eg indSimpleMovingAverage)

	*/

	SetParam(0, ptSource, "");
	SetParam(1, ptPeriods, "14");
	SetParam(2, ptStandardDeviations, "2");
	SetParam(3, ptMAType, "Simple");

}


BOOL CIndicatorBollingerBands::Calculate()
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
	if(paramStr.size() < (unsigned int)paramCount)
		return FALSE;
	if(paramInt.size() < (unsigned int)paramCount)
		return FALSE;
//	End Of Revision

	if(paramInt[1] < 1 || paramInt[1] > size / 2){
		//ProcessError("Invalid Periods for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[2] < 0 || paramInt[2] > 10){
		//ProcessError("Invalid Standard Deviations for indicator " + szName);
		return FALSE;
	}
	else if(paramInt[3] < MA_START || paramInt[3] > MA_END){
		//ProcessError("Invalid Moving Average Type for indicator " + szName);
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
	pInd = ta.BollingerBands(pNav, pSource, paramInt[1], paramDbl[2], paramInt[3]);


	// Output the indicator values
	Clear();
	CSeries* series = GetSeries(paramStr[0]);
	
	CSeries* pTop = EnsureSeries(szName + " Top");
	SetSeriesSignal(szName + " Top",lineColor,lineStyle,lineWeight);
	CSeries* pBottom = EnsureSeries(szName + " Bottom");
	SetSeriesSignal(szName + " Bottom",lineColor,lineStyle,lineWeight);
	if(pCtrl->m_Language==1){
		pTop->szTitle = CString("BB("+paramInt[1]+","+paramDbl[2]+")")+" Superior";
		pBottom->szTitle = CString("BB("+paramInt[1]+","+paramDbl[2]+")")+" Inferior";
	}
	else{
		pTop->szTitle = CString("BB("+paramInt[1]+","+paramDbl[2]+")")+" Top";
		pBottom->szTitle = CString("BB("+paramInt[1]+","+paramDbl[2]+")")+" Bottom";
	}
	//szTitle = "";

	double top = 0, median = 0, bottom = 0, jdate = 0;	
	for(int n = 0; n < size; ++n){
		if(n < paramInt[1]){
			top = NULL_VALUE;
			median = NULL_VALUE;
			bottom = NULL_VALUE;
		}
		else{
			top = pInd->getValue("Bollinger Band Top", n + 1);
			//median = pInd->getValue("Bollinger Band Median", n + 1); //Comment this to hide the median serie
			bottom = pInd->getValue("Bollinger Band Bottom", n + 1);
		}
		jdate = series->data_master[n].jdate;
		this->AppendValue(jdate, median);
		pCtrl->AppendNewValue(szName + " Top", jdate, top);
		pCtrl->AppendNewValue(szName + " Bottom", jdate, bottom);
	}
 
	// Clean up	
	delete pRS;
	delete pInd;
	delete pNav;

	return CIndicator::Calculate();
}
