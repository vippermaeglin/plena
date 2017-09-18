// IndicatorVolume.cpp: implementation of the CIndicatorVolume class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IndicatorVolume.h"
#include "tasdk.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicatorVolume::CIndicatorVolume(LPCTSTR name, int type, int members, CChartPanel* owner)
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

	indicatorType = indVolume;

}

CIndicatorVolume::~CIndicatorVolume()
{
	CIndicator::OnDestroy();
}


void CIndicatorVolume::SetParamInfo(){

	/*  Required inputs for this indicator:
		  
	  1. paramStr[0] = Volume (eg "msft.volume")

	*/

	SetParam(0, ptVolume, "");
	
}


BOOL CIndicatorVolume::Calculate()
{
#ifdef _CONSOLE_DEBUG
		printf("\nVolume::Calculate(%s)",(CString)paramStr[0]);
#endif
	
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
	if(!GetUserInput()) {
#ifdef _CONSOLE_DEBUG
		printf("\nRETURNED !GETUSERINPUT");
#endif
		return FALSE;
	}

	// Validate
	long size = pCtrl->RecordCount();
	if(size == 0) {
#ifdef _CONSOLE_DEBUG
		printf("\nRETURNED SIZE=0");
#endif
		return FALSE;
	}
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	if(paramStr.size() < (unsigned int)paramCount){
//	End Of Revision
#ifdef _CONSOLE_DEBUG
		printf("\nRETURNED paramStr<paramCount");
#endif
		return FALSE;
	}
	
#ifdef _CONSOLE_DEBUG
		printf("\nCopy Volume Data");
#endif
	//Copy Volume Data:
	data_master.clear();
	data_master.resize(0);
	data_slave.clear();
	data_slave.resize(0);
	if(((CString)paramStr[0]).Find(".Open")>0 || ((CString)paramStr[0]).Find(".OPEN")>0){
#ifdef _CONSOLE_DEBUG
		printf(" param=OPEN");
#endif
		for(int index=0;index<pCtrl->panels[0]->series[0]->data_master.size();index++){
			data_master.push_back(pCtrl->panels[0]->series[0]->data_master[index]);
		}
		for(int index=0;index<pCtrl->panels[0]->series[0]->data_slave.size();index++){
			data_slave.push_back(pCtrl->panels[0]->series[0]->data_slave[index]);
		}
	}
	else if(((CString)paramStr[0]).Find(".High")>0){
#ifdef _CONSOLE_DEBUG
		printf(" param=HIGH");
#endif
		for(int index=0;index<pCtrl->panels[0]->series[1]->data_master.size();index++){
			data_master.push_back(pCtrl->panels[0]->series[1]->data_master[index]);
		}
		for(int index=0;index<pCtrl->panels[0]->series[1]->data_slave.size();index++){
			data_slave.push_back(pCtrl->panels[0]->series[1]->data_slave[index]);
		}
	}
	else if(((CString)paramStr[0]).Find(".Low")>0){
#ifdef _CONSOLE_DEBUG
		printf(" param=LOW");
#endif
		for(int index=0;index<pCtrl->panels[0]->series[2]->data_master.size();index++){
			data_master.push_back(pCtrl->panels[0]->series[2]->data_master[index]);
		}
		for(int index=0;index<pCtrl->panels[0]->series[2]->data_slave.size();index++){
			data_slave.push_back(pCtrl->panels[0]->series[2]->data_slave[index]);
		}
	}
	else if(((CString)paramStr[0]).Find(".Close")>0){
#ifdef _CONSOLE_DEBUG
		printf(" param=CLOSE");
#endif
		for(int index=0;index<pCtrl->panels[0]->series[3]->data_master.size();index++){
			data_master.push_back(pCtrl->panels[0]->series[3]->data_master[index]);
		}
		for(int index=0;index<pCtrl->panels[0]->series[3]->data_slave.size();index++){
			data_slave.push_back(pCtrl->panels[0]->series[3]->data_slave[index]);
		}
	}
	else if(((CString)paramStr[0]).Find(".Volume")>0 || ((CString)paramStr[0]).Find(".VOLUME")>0){
#ifdef _CONSOLE_DEBUG
		printf(" param=VOLUME");
#endif
		for(int index=0;index<pCtrl->panels[0]->series[4]->data_master.size();index++){
			data_master.push_back(pCtrl->panels[0]->series[4]->data_master[index]);			
		}
		for(int index=0;index<pCtrl->panels[0]->series[4]->data_slave.size();index++){
			data_slave.push_back(pCtrl->panels[0]->series[4]->data_slave[index]);
		}
	}
	

 /*
	// Get the data	CField* pVolume = SeriesToField("Volume", paramStr[0], size);
	CField* pVolume = SeriesToField("Volume", paramStr[0], size);		
	if(!EnsureField(pVolume, paramStr[0])) return FALSE;
	
 	CNavigator* pNav = new CNavigator();
	CRecordset* pRS = new CRecordset();
	CRecordset* pInd = NULL;

	pRS->addField(pVolume);
	pNav->setRecordset(pRS);


	// Calculate the indicator
	CGeneral ta;
	pInd = ta.Volume(pNav, pVolume, szName);
	

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
	*/

	return CIndicator::Calculate();
}
