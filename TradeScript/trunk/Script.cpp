// Script.cpp : Implementation of Script

//
// Scripting language - Original design 2003 as Trade BASIC
//					  - Redisigned Oct 24th 2005 as Scan Script
// Richard Gardner
//
// Started: 8/25/05
// Finished: 11/3/05
// 
// TradeScript scripting language - Redesigned 2006-2007

#include "stdafx.h"
#include "Script.h"
#include <time.h>

//#include <sstream>
//#include <fstream>
//#include <cstdlib> 
//#include <istream>
//#include <ios> 
//#include <iostream>


#define	MAX_VARIABLES 500
#define SCRIPT_TIME_OUT	300000 // 5 minutes

#define	MAX_RESULTS_FREE	100
#define	MAX_RESULTS_TRIAL	250
#define	MAX_RESULTS_PAID	3000

// Script


double Script::ToJulianDate(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet)
{
double extra = 100 * nYear + nMonth - 190002.5;
	double rjd = 367 * nYear;
	rjd -= (double)(floor((double)(7 * (nYear + floor((double)((nMonth + 9) / 12))) / 4)));
	rjd += (double)floor((double)(275 * nMonth / 9));
	rjd += nDay;	
	rjd += ((double)nHour + ((double)nMinute / 60) + 
		((double)nSecond / 3600) + ((double)nMillisecond / 3600000)) / 24;
	rjd += 1721013.5;
	rjd -= 0.5 * extra / fabs(extra);
	rjd += 0.5;
	*pRet = rjd;

	return S_OK;
}

string Script::FromJulianDate(double JDate)
{

	int iDay = 0, iMonth = 0, iYear = 0;
    int iHour = 0, iMinute = 0, iSecond = 0, iMSec = 0;
    double a = 0, z = 0, f = 0, i = 0, d = 0, b = 0, c = 0, t = 0, rj = 0, rh = 0;

	z = floor(JDate + 0.5);
    f = JDate + 0.5 - z;	
    if(z < 2299161){
      a = z;
    }
	else{
      i = floor((z - 1867216.25) / 36524.25);
      a = z + 1 + i - floor(i / 4);
    }

    b = a + 1524;
    c = floor((b - 122.1) / 365.25);
    d = floor(365.25 * c);
    t = floor((b - d) / 30.6);
    rj = b - d - floor(30.6001 * t) + f;
	if(rj == 0)
	{
		t -= 1;
		rj = b - d - floor(30.6001 * t) + f;
	}
    iDay = floor(rj);
    rh = (rj - floor(rj)) * 24;

    iHour = floor(rh);
    rh -= iHour;
    iMinute = floor(rh * 60);
    rh -= ((double)iMinute / 60);
    iSecond = floor(rh * 3600);
    rh -= ((double)iSecond / 3600);
    iMSec = (rh * 3600000);
    if(t < 14){
      iMonth = t - 1;
	}
    else{
      if(t == 14 || t == 15) iMonth = t - 13;
    }
    if(iMonth > 2){
      iYear = c - 4716;
    }
	else{
      if(iMonth == 1 || iMonth == 2) iYear = c - 4715;
    }
	if(iMSec == 999 && iMinute < 59)
	{
		iMinute++;
		iMSec = 0;
	}
	if(iDay == 0){
		iMonth -= 1;
		iDay = 31;
		if(iMonth == 12) iYear -=1;
	}
 

	
	SYSTEMTIME myTime;
	myTime.wMonth = iMonth;
	myTime.wDay = iDay;
	myTime.wYear = iYear;
	myTime.wHour = iHour;
	myTime.wMinute = iMinute;
	myTime.wSecond = iSecond;
	myTime.wMilliseconds = iMSec;
	char timeBuf[20+1] = "";
	char dateBuf[20+1] = "";
	GetTimeFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, timeBuf, 20);
	GetDateFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, dateBuf, 20);
	string szDT = dateBuf;
	szDT += " ";
	szDT += timeBuf;
	if(strlen(dateBuf) == 0) szDT = "No Date Time";
	return szDT;
	

/*
	// Changed 2/10/10
	 SYSTEMTIME myTime;
     myTime.wMonth = iMonth;
     myTime.wDay = iDay;
     myTime.wYear = iYear;
     myTime.wHour = iHour;
     myTime.wMinute = iMinute;
     myTime.wSecond = iSecond;
     myTime.wMilliseconds = iMSec;
     TCHAR timeBuf[20+1] = _T("");
     TCHAR dateBuf[20+1] = _T("");
     GetTimeFormat(LOCALE_USER_DEFAULT, NULL, &myTime, "HH':'mm':'ss",  
	 timeBuf, 20);  	 //AhmadMusa Formatting the Time
	 GetDateFormat(LOCALE_USER_DEFAULT, NULL, &myTime, "dd'/'MM'/'yyyy",  
	 dateBuf, 20); 	//AhmadMusa Formatting the Date
     string szDT = dateBuf;
     szDT += _T(" ");
     szDT += timeBuf;
     if(strlen(dateBuf) == 0) return _T("No Date Time");
     return szDT;
     //vaTimeDate.SetDateTime(Year,Month,Day,Hour,Minute,Second);
     //return vaTimeDate.Format(0,LANG_USER_DEFAULT);
*/


}



void Script::Test()
{
	
	recordset ohlcv;
	LoadTestData(ohlcv);
	
	CTASDK tasdk;
	tasdk.Test();
	
	tasdk.AddField("Close", 100, ohlcv);
	
	field* close = tasdk.GetField("C", ohlcv);
	field* high = tasdk.GetField("H", ohlcv);
	field* low = tasdk.GetField("L", ohlcv);
	
	//field f = tasdk.LLV( high, 5);
	field* volume = tasdk.GetField("V", ohlcv);
	
	long start = GetTickCount();
	
	for(int test  = 0; test != 1000; test++)
	{

	recordset ret = tasdk.StochasticOscillator(ohlcv, 5,2,1,3);
	field* p = tasdk.GetField("C", ret);
	field f = tasdk.ExponentialMovingAverage(close, 10);
	f = tasdk.DetrendedPriceOscillator(volume, 10, 7);
	f = tasdk.UltimateOscillator (ohlcv, 5,10,20);


	// 1. CONVERT SCRIPT TO UPPER CASE AND REMOVE CHARACTERS LIKE %$#@!
	// 2. ALWAYS CHECK POINTERS FOR NULL (FIELD NOT FOUND IN RECORDSET)
	
	
	ret = tasdk.PrimeNumberBands(ohlcv);
	field* f2 = tasdk.GetField("PN_TOP", ret);
	
	double a = 0;
	for(int n = 1; n != 26; ++n)
	{
		a = f2->data[n];
	}
	
	
	}

	long end = GetTickCount() - start;

	string done;
  
	
//	string test = string(InputValue);
//	test += "!!!";
//	*pVal = test.AllocSysString();

	 
}

// Adds a variable and doesn't check if it exists or not
__inline void Script::AddVar(string name, double value)
{
	variable var;
	var.name = name;
	var.value = value;
	variables.push_back(var);
}

// Adds a primitive variable and doesn't check if it exists or not
__inline void Script::AddPrimVar(string name, double value)
{
	variable var;
	var.name = name;
	var.value = value;
	primitiveVariables.push_back(var);
}

void Script::ResetVariables()
{

	// Clear the garbage collection
	variables.clear();
	primitiveVariables.clear();
	m_recordset.clear();

	// Native constants
	AddPrimVar("PI", 3.1415926535897932384626433832795f);	
	AddPrimVar("TRUE", true);
	AddPrimVar("FALSE", false);

	// Operator constants	
	AddPrimVar("ADD", 1);
	AddPrimVar("SUBTRACT", 2);
	AddPrimVar("MULTIPLY", 3);
	AddPrimVar("DIVIDE", 4);

	// Moving average constants	
	AddPrimVar("SIMPLE", 1);
	AddPrimVar("EXPONENTIAL", 2);
	AddPrimVar("TIME_SERIES", 3);
	AddPrimVar("VARIABLE", 4);
	AddPrimVar("TRIANGULAR", 5);
	AddPrimVar("WEIGHTED", 6);
	AddPrimVar("VOLATILITY", 7);
	AddPrimVar("WILDER", 8);

	/*/ Symbol constants (add later)
	AddPrimVar("52_WEEK_HIGH", m_data->d52WeekHigh);
	AddPrimVar("52_WEEK_LOW", m_data->d52WeekLow);
	AddPrimVar("ALL_TIME_HIGH", m_data->dAllTimeHigh);
	AddPrimVar("ALL_TIME_LOW", m_data->dAllTimeLow);
	AddPrimVar("AVG_VOLUME", m_data->dAvgVolume);
	AddPrimVar("DIVIDEND", m_data->dDividend);
	AddPrimVar("PE_RATIO", m_data->dPERatio);
	AddPrimVar("YIELD", m_data->dYield);
	*/

	// Trends
	AddPrimVar("UP", 1);
	AddPrimVar("DOWN", 2);
	AddPrimVar("SIDEWAYS", 3);

	// Candlestick pattern definitions 
	AddPrimVar("LONG_BODY", 1);
	AddPrimVar("DOJI", 2);
	AddPrimVar("HAMMER", 3);
	AddPrimVar("HARAMI", 4);
	AddPrimVar("STAR", 5);
	AddPrimVar("DOJI_STAR", 6);
	AddPrimVar("MORNING_STAR", 7);
	AddPrimVar("EVENING_STAR", 8);
	AddPrimVar("PIERCING_LINE", 9);
	AddPrimVar("BULLISH_ENGULFING_LINE", 10);
	AddPrimVar("HANGING_MAN", 11);
	AddPrimVar("DARK_CLOUD_COVER", 12);
	AddPrimVar("BEARISH_ENGULFING_LINE", 13);
	AddPrimVar("BEARISH_DOJI_STAR", 14);
	AddPrimVar("BEARISH_SHOOTING_STAR", 15);
	AddPrimVar("SPINNING_TOPS", 16);
	AddPrimVar("HARAMI_CROSS", 17);
	AddPrimVar("BULLISH_TRISTAR", 18);
	AddPrimVar("THREE_WHITE_SOLDIERS", 19);
	AddPrimVar("THREE_BLACK_CROWS", 20);
	AddPrimVar("ABANDONED_BABY", 21);
	AddPrimVar("BULLISH_UPSIDE_GAP", 22);
	AddPrimVar("BULLISH_HAMMER", 23);
	AddPrimVar("BULLISH_KICKING", 24);
	AddPrimVar("BEARISH_KICKING", 25);
	AddPrimVar("BEARISH_BELT_HOLD", 26);
	AddPrimVar("BULLISH_BELT_HOLD", 27);
	AddPrimVar("BEARISH_TWO_CROWS", 28);
	AddPrimVar("BULLISH_MATCHING_LOW", 29);

	// Points or percent (used in indicators like Volume Oscillator)
	AddPrimVar("POINTS", 1);
	AddPrimVar("PERCENT", 2);

}


// Copies price data to the TA-SDK recordset
bool Script::LoadSymbolData(int record)
{

	if(m_watchVars) m_maxBars = m_records.size() - 1; // See GetData

	int test = m_records.size();
	m_temp.clear();
	int n = 0;
	for(n = record - m_maxBars; n < (int)m_records.size(); ++n)
	{
		m_temp.push_back(m_records[n]);
	}


	if(m_temp.size() == 0) return false;
	m_recordset.clear();
	int size = m_temp.size() - 1;
	if(size > m_maxBars) return false;
	
	ResetVariables();


	CTASDK tasdk;
	tasdk.AddField( "D", m_maxBars-1, m_recordset);
	tasdk.AddField( "O", m_maxBars-1, m_recordset);
	tasdk.AddField( "H", m_maxBars-1, m_recordset);
	tasdk.AddField( "L", m_maxBars-1, m_recordset);
	tasdk.AddField( "C", m_maxBars-1, m_recordset);
	tasdk.AddField( "V", m_maxBars-1, m_recordset);

	field* fD = tasdk.GetField("D", m_recordset);
	field* fO = tasdk.GetField("O", m_recordset);
	field* fH = tasdk.GetField("H", m_recordset);
	field* fL = tasdk.GetField("L", m_recordset);
	field* fC = tasdk.GetField("C", m_recordset);
	field* fV = tasdk.GetField("V", m_recordset);

	variable open, high, low, close, volume, null;
	open.field.resize(fD->data.size());
	open.name = "OPEN";
	high.field.resize(fD->data.size());
	high.name = "HIGH";
	low.field.resize(fD->data.size());
	low.name = "LOW";
	close.field.resize(fD->data.size());
	close.name = "CLOSE";
	volume.field.resize(fD->data.size());
	volume.name = "VOLUME";
	null.field.resize(fD->data.size());
	null.name = "NULL";

	int index = fD->data.size() - 1;
	int endpos = m_temp.size() - 1;
	int start = m_maxBars - endpos;	

	for(n = endpos; index > -1; --n)
	{
		if(index < 0) break;
		fD->data[index] = m_temp[n].jDateTime;
		fO->data[index] = m_temp[n].open;
		fH->data[index] = m_temp[n].high;
		fL->data[index] = m_temp[n].low;
		fC->data[index] = m_temp[n].close;
		fV->data[index] = m_temp[n].volume;

		open.field[index] = m_temp[n].open;
		high.field[index] = m_temp[n].high;
		low.field[index] = m_temp[n].low;
		close.field[index] = m_temp[n].close;
		volume.field[index] = m_temp[n].volume;

		index--;
	}

	open.value = open.field[open.field.size()-1];
	high.value = high.field[high.field.size()-1];
	low.value = low.field[low.field.size()-1];
	close.value = close.field[close.field.size()-1];
	volume.value = volume.field[volume.field.size()-1];

	variables.push_back(open);
	variables.push_back(high);	
	variables.push_back(low);
	variables.push_back(close);
	variables.push_back(volume);

	


/*
	CTASDK tasdk;
	tasdk.AddField( "D", m_maxBars-1, m_recordset);
	tasdk.AddField( "O", m_maxBars-1, m_recordset);
	tasdk.AddField( "H", m_maxBars-1, m_recordset);
	tasdk.AddField( "L", m_maxBars-1, m_recordset);
	tasdk.AddField( "C", m_maxBars-1, m_recordset);
	tasdk.AddField( "V", m_maxBars-1, m_recordset);

	field* fD = tasdk.GetField("D", m_recordset);
	field* fO = tasdk.GetField("O", m_recordset);
	field* fH = tasdk.GetField("H", m_recordset);
	field* fL = tasdk.GetField("L", m_recordset);
	field* fC = tasdk.GetField("C", m_recordset);
	field* fV = tasdk.GetField("V", m_recordset);
	
	int end = m_temp.size() - 1;
	int start = end - m_maxBars - 1;
	if(start > end) return false;
	size_t index = fD->data.size() - 1;
	for(n = end; n != start + 1; --n)
	{		
		fD->data[index] = m_temp[n].jDateTime;
		fO->data[index] = m_temp[n].open;
		fH->data[index] = m_temp[n].high;
		fL->data[index] = m_temp[n].low;
		fC->data[index] = m_temp[n].close;
		fV->data[index] = m_temp[n].volume;
		index--;
	}

	// Fill the script variables
	// TA-SDK is 1 based but the scripting language is 0 based.
	// So a loop is required instead of a direct copy.

	variable var;

	var.name = "OPEN";
	var.field.resize(m_maxBars);
	var.field = fO->data;	
	var.value = fO->data[m_maxBars - 1];
	variables.push_back(var);

	var.name = "HIGH";
	var.field.resize(m_maxBars);
	var.field = fH->data;
	var.value = fH->data[m_maxBars - 1];	
	variables.push_back(var);

	var.name = "LOW";
	var.field.resize(m_maxBars);
	var.field = fL->data;
	var.value = fL->data[m_maxBars - 1];
	variables.push_back(var);

	var.name = "CLOSE";
	var.field.resize(m_maxBars);
	var.field = fC->data;
	var.value = fC->data[m_maxBars - 1];
	variables.push_back(var);

	var.name = "VOLUME";
	var.field.resize(m_maxBars);
	var.field = fV->data;
	var.value = fV->data[m_maxBars - 1];
	variables.push_back(var);
*/


/*/ Correct end value:
double t0 = m_temp[m_temp.size() - 1].close;

double tH = m_temp[m_temp.size() - 1].high;
double tL = m_temp[m_temp.size() - 1].low;

double hml = tH - tL;
// Compared to HML function, this proves the values
// match up together (the indicator bar numbers and 
// the data bar numbers). This was difficult because
// there may be fewer bars than m_maxBars and because
// TA-SDK is 1 based and the data is 0 based. This is
// working correctly - verified 11/2/05
// This means that any recordset returned by TA-SDK and
// hard copied (.field = .data) will be 1-based, and the
// first value will be 0. This is why the "REF" function
// does not allow a REF prior to the second bar.

variable c = GetVar("CLOSE");
double t1 = c.field[c.field.size() - 1];
double t2 = c.field[m_maxBars - 1];

double t3 = fC->data[fC->data.size() - 1];
double t4 = fC->data[m_maxBars - 1];

size_t s1 = c.field.size();
size_t s2 = fC->data.size();
*/

	return true;

}


// Copies price data to the TA-SDK recordset
bool Script::LoadSymbolDataForBackTest(int record)
{
#ifdef _CONSOLE_DEBUG
	string msg = "LoadSymbolDataForBackTest()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	m_temp.clear();
	if(m_records.size() == 0) return false;

	if(record > (int)m_records.size()) return false; // 5/14/08

	int n = 0;
	for(n = record - m_maxBars; n < record; ++n)
		m_temp.push_back(m_records[n]);

	if(m_temp.size() == 0) return false;
	m_recordset.clear();
	int size = m_temp.size();
	if(size < m_maxBars) return false;	
	
	ResetVariables();

	
	CTASDK tasdk;
	tasdk.AddField( "D", m_maxBars-1, m_recordset);
	tasdk.AddField( "O", m_maxBars-1, m_recordset);
	tasdk.AddField( "H", m_maxBars-1, m_recordset);
	tasdk.AddField( "L", m_maxBars-1, m_recordset);
	tasdk.AddField( "C", m_maxBars-1, m_recordset);
	tasdk.AddField( "V", m_maxBars-1, m_recordset);

	field* fD = tasdk.GetField("D", m_recordset);
	field* fO = tasdk.GetField("O", m_recordset);
	field* fH = tasdk.GetField("H", m_recordset);
	field* fL = tasdk.GetField("L", m_recordset);
	field* fC = tasdk.GetField("C", m_recordset);
	field* fV = tasdk.GetField("V", m_recordset);

	variable open, high, low, close, volume, null;
	open.field.resize(fD->data.size());
	open.name = "OPEN";
	high.field.resize(fD->data.size());
	high.name = "HIGH";
	low.field.resize(fD->data.size());
	low.name = "LOW";
	close.field.resize(fD->data.size());
	close.name = "CLOSE";
	volume.field.resize(fD->data.size());
	volume.name = "VOLUME";
	null.field.resize(fD->data.size());
	null.name = "NULL";

	int index = fD->data.size() - 1;
	int endpos = m_temp.size() - 1;
	int start = m_maxBars - endpos;	

	for(n = endpos; index > -1; --n)
	{
		if(index < 0) break;
		fD->data[index] = m_temp[n].jDateTime;
		fO->data[index] = m_temp[n].open;
		fH->data[index] = m_temp[n].high;
		fL->data[index] = m_temp[n].low;
		fC->data[index] = m_temp[n].close;
		fV->data[index] = m_temp[n].volume;

		open.field[index] = m_temp[n].open;
		high.field[index] = m_temp[n].high;
		low.field[index] = m_temp[n].low;
		close.field[index] = m_temp[n].close;
		volume.field[index] = m_temp[n].volume;

		index--;
	}

	open.value = open.field[open.field.size()-1];
	high.value = high.field[high.field.size()-1];
	low.value = low.field[low.field.size()-1];
	close.value = close.field[close.field.size()-1];
	volume.value = volume.field[volume.field.size()-1];

	variables.push_back(open);
	variables.push_back(high);	
	variables.push_back(low);
	variables.push_back(close);
	variables.push_back(volume);



	/*
	double x = fC->data[fC->data.size()-1];
	var = GetVar("CLOSE");
	double closeVAR = var.value;
	closeVAR = var.field[var.field.size() - 1];
	double closeTASDK = fC->data[fC->data.size()-1];
	double closeTASDK2 = fC->data[fC->data.size()-2];

	long a = var.field.size();
	long b = fC->data.size();
	*/


/*/ Correct end value:
double t0 = m_temp[m_temp.size() - 1].close;

double tH = m_temp[m_temp.size() - 1].high;
double tL = m_temp[m_temp.size() - 1].low;

double hml = tH - tL;
// Compared to HML function, this proves the values
// match up together (the indicator bar numbers and 
// the data bar numbers). This was difficult because
// there may be fewer bars than m_maxBars and because
// TA-SDK is 1 based and the data is 0 based. This is
// working correctly - verified 11/2/05
// This means that any recordset returned by TA-SDK and
// hard copied (.field = .data) will be 1-based, and the
// first value will be 0. This is why the "REF" function
// does not allow a REF prior to the second bar.

variable c = GetVar("CLOSE");
double t1 = c.field[c.field.size() - 1];
double t2 = c.field[m_maxBars - 1];

double t3 = fC->data[fC->data.size() - 1];
double t4 = fC->data[m_maxBars - 1];

double t5 = fC->data[m_maxBars - 2];
double t6 = c.field[m_maxBars - 2];

size_t s1 = c.field.size();
size_t s2 = fC->data.size();

*/
	return true;

}

void Script::LoadTestData(recordset& r)
{

	CTASDK tasdk;
	tasdk.AddField( "D", 25, r);
	tasdk.AddField( "O", 25, r);
	tasdk.AddField( "H", 25, r);
	tasdk.AddField( "L", 25, r);
	tasdk.AddField( "C", 25, r);
	tasdk.AddField( "V", 25, r);

	// Simulate jdate
	field* f = tasdk.GetField("D", r);
	for(int n= 0; n != 26; ++n)
		f->data[n] = n + 24000;

	f = tasdk.GetField("O",r); 
	f->data[1] = 21.98;
	f->data[2] = 21.97;
	f->data[3] = 21.88;
	f->data[4] = 21.96;
	f->data[5] = 21.99;
	f->data[6] = 21.98;
	f->data[7] = 21.88;
	f->data[8] = 22.05;
	f->data[9] = 22.02;
	f->data[10] = 22.02;
	f->data[11] = 22.09;
	f->data[12] = 22.05;
	f->data[13] = 22.05;
	f->data[14] = 22.06;
	f->data[15] = 22.03;
	f->data[16] = 22.06;
	f->data[17] = 22.04;
	f->data[18] = 22.01;
	f->data[19] = 22.02;
	f->data[20] = 22.05;
	f->data[21] = 22.05;
	f->data[22] = 22.02;
	f->data[23] = 22;
	f->data[24] = 22;
	f->data[25] = 22.02;

	f = tasdk.GetField("H",r);
	f->data[1] = 22.04;
	f->data[2] = 21.97;
	f->data[3] = 21.95;
	f->data[4] = 22;
	f->data[5] = 22;
	f->data[6] = 22.05;
	f->data[7] = 22.05;
	f->data[8] = 22.08;
	f->data[9] = 22.06;
	f->data[10] = 22.12;
	f->data[11] = 22.1;
	f->data[12] = 22.08;
	f->data[13] = 22.08;
	f->data[14] = 22.07;
	f->data[15] = 22.08;
	f->data[16] = 22.07;
	f->data[17] = 22.04;
	f->data[18] = 22.02;
	f->data[19] = 22.06;
	f->data[20] = 22.06;
	f->data[21] = 22.05;
	f->data[22] = 22.02;
	f->data[23] = 22.01;
	f->data[24] = 22.01;
	f->data[25] = 22.09;

	f = tasdk.GetField("L",r);	
	f->data[1] = 21.98;
	f->data[2] = 21.86;
	f->data[3] = 21.88;
	f->data[4] = 21.96;
	f->data[5] = 21.96;
	f->data[6] = 21.88;
	f->data[7] = 21.88;
	f->data[8] = 22.02;
	f->data[9] = 22.01;
	f->data[10] = 22.02;
	f->data[11] = 22.03;
	f->data[12] = 22.03;
	f->data[13] = 22;
	f->data[14] = 22.04;
	f->data[15] = 22.03;
	f->data[16] = 22.04;
	f->data[17] = 22.01;
	f->data[18] = 22.01;
	f->data[19] = 22.01;
	f->data[20] = 22.04;
	f->data[21] = 22.01;
	f->data[22] = 22;
	f->data[23] = 22;
	f->data[24] = 22;
	f->data[25] = 22;

	f = tasdk.GetField("C",r);	
	f->data[1] = 22;
	f->data[2] = 21.88;
	f->data[3] = 21.95;
	f->data[4] = 22;
	f->data[5] = 21.97;
	f->data[6] = 21.88;
	f->data[7] = 22.05;
	f->data[8] = 22.02;
	f->data[9] = 22.01;
	f->data[10] = 22.11;
	f->data[11] = 22.05;
	f->data[12] = 22.06;
	f->data[13] = 22.08;
	f->data[14] = 22.04;
	f->data[15] = 22.08;
	f->data[16] = 22.04;
	f->data[17] = 22.02;
	f->data[18] = 22.02;
	f->data[19] = 22.04;
	f->data[20] = 22.06;
	f->data[21] = 22.01;
	f->data[22] = 22;
	f->data[23] = 22;
	f->data[24] = 22.01;
	f->data[25] = 22.01;

	f = tasdk.GetField("V",r);	
	f->data[1] = 1900;	
	f->data[2] = 6600;
	f->data[3] = 23300;
	f->data[4] = 5400;
	f->data[5] = 2800;
	f->data[6] = 16600;
	f->data[7] = 21700;
	f->data[8] = 9100;
	f->data[9] = 6800;
	f->data[10] = 3200;
	f->data[11] = 2600;
	f->data[12] = 3700;
	f->data[13] = 9300;
	f->data[14] = 2800;
	f->data[15] = 9900;
	f->data[16] = 6800;
	f->data[17] = 3800;
	f->data[18] = 1200;
	f->data[19] = 5000;
	f->data[20] = 7200;
	f->data[21] = 1500;
	f->data[22] = 6000;
	f->data[23] = 2800;
	f->data[24] = 9000;
	f->data[25] = 8500;

}




// Back tests a symbol and returns the output of risk measurement, or an error
// if "error" is found in the returned string.
string Script::BackTest(string& BuyScript, string& SellScript, string& ExitLongScript, string& ExitShortScript, double SlipPct)
{

#ifdef _CONSOLE_DEBUG
	string msg = "BackTest()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
	m_trades.clear();

	if(SlipPct < 0 || SlipPct > 1) SlipPct = 0.00001f;

	string ret = RunBackTest(BuyScript, BUY);
	if (m_error != "") return m_error;
#ifdef _CONSOLE_DEBUG
	msg = "Buy()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	ret = RunBackTest(SellScript, SELL);
	if (m_error != "") return m_error;
#ifdef _CONSOLE_DEBUG
	msg = "Sell()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Check to see if the parameters are valid
	string exitLongScript, exitShortScript;
	if(ExitLongScript != "")
	{		
		ret = RunBackTest(ExitLongScript, EXIT_LONG);
		if(m_error != "") return m_error;
	}
	if(ExitShortScript != "")
	{		
		ret = RunBackTest(ExitShortScript, EXIT_SHORT);
		if(m_error != "") return m_error;
	}
	
	// Add slippage to all trades
	srand((unsigned int)time(NULL));
	double slippage = 0, direction = 0, price = 0;
	int n = 0;
	for(n = 0; n < (int)m_trades.size(); ++n)
	{
		//slippage = ((double)rand() / RAND_MAX * SlipPct);
		direction = ((double)rand() / RAND_MAX * 1);
		if(direction > 0.5)
			m_trades[n].price += (m_trades[n].price * slippage);
		else
			m_trades[n].price -= (m_trades[n].price * slippage);
	}

#ifdef _CONSOLE_DEBUG
	msg = "1()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Get the start and end dates for this back test
	if(m_records.size() == 0) return "";
	string startDate = FromJulianDate(m_records[0].jDateTime);
	string endDate = FromJulianDate(m_records[m_records.size() - 1].jDateTime);


	// Sort the m_trades array by jdate
	sort(m_trades.begin(), m_trades.end());

#ifdef _CONSOLE_DEBUG
	msg = "2()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Delete extra trades
	// Note: this step should be removed if supporting adding 
	// to already existing positions positions (compounding).
	vector<trade> m_tempTrade;

/*
	// Dump raw trades
	string sDate = "";
	string stradeLog = "";
	string dump = "";
	for(n = 0; n < m_trades.size(); n++)
	{
		sDate = FromJulianDate(m_trades[n].jdate);
		stradeLog += sDate + ",";
		if(m_trades[n].signal == BUY)
			stradeLog += "LONG," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == SELL)
			stradeLog += "SHORT," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT_LONG)
			stradeLog += "EXIT LONG," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT_SHORT)
			stradeLog += "EXIT SHORT," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT)
			stradeLog += "EXIT," + toString(m_trades[n].price,4);

		stradeLog += "\r\n";
	}
*/


	
	// Remove excess trades (buy when already long, exit when not in, etc.)
	int lastTrade = NO_TRADE;
	bool tradeOpen = false;

	for(n = 0; n < (int)m_trades.size(); ++n)
	{
		int test = m_trades[n].signal;
		if(m_trades[n].signal != lastTrade) // Filter out duplicate trades
		{

			if((m_trades[n].signal == EXIT_SHORT || m_trades[n].signal == EXIT) && lastTrade == SELL)
			{
				m_tempTrade.push_back(m_trades[n]);
				lastTrade = m_trades[n].signal;
			}
			else if((m_trades[n].signal == EXIT_LONG || m_trades[n].signal == EXIT) && lastTrade == BUY)
			{
				m_tempTrade.push_back(m_trades[n]);
				lastTrade = m_trades[n].signal;
			}
			else if(m_trades[n].signal == BUY && (!tradeOpen || lastTrade == SELL || lastTrade == NO_TRADE))
			{
				m_tempTrade.push_back(m_trades[n]);
				lastTrade = m_trades[n].signal;
			}
			else if(m_trades[n].signal == SELL && (tradeOpen || lastTrade == BUY /*|| lastTrade == NO_TRADE*/))
			{
				m_tempTrade.push_back(m_trades[n]);
				lastTrade = m_trades[n].signal;
			}
			
			// moved from top of inner loop 12/5/09
			if(m_trades[n].signal < 3)
				tradeOpen = true;
			else
				tradeOpen = false;

		}
	}
	m_trades = m_tempTrade;
	m_tempTrade.clear();

#ifdef _CONSOLE_DEBUG
	msg = "3()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Calculate the P&L arrays
	int size = (int)m_trades.size();
	if(size == 0)
	{
		return "Error: Scripts generated no trades.";		
	}
#ifdef _CONSOLE_DEBUG
	msg = "4()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	double value = 0;
	vector<double> pl;
	vector<double> plJDates;
	vector<double> plMonthly;

	for(n = 1; n < size; ++n)
	{
		if(m_trades[n-1].signal == BUY && 
			(m_trades[n].signal == SELL || m_trades[n].signal == EXIT_LONG ||
			m_trades[n].signal == EXIT))
		{
			// Close out of a long position
			value = m_trades[n].price - m_trades[n-1].price;
			pl.push_back(value);
			plJDates.push_back(m_trades[n].jdate); 
		}
		/*else if(m_trades[n-1].signal == SELL && 
			(m_trades[n].signal == BUY || m_trades[n].signal == EXIT_SHORT ||
			 m_trades[n].signal == EXIT))
		{
			// Close out of a short position
			value = m_trades[n-1].price - m_trades[n].price;
			pl.push_back(value);
			plJDates.push_back(m_trades[n].jdate); 
		}*/
	}


#ifdef _CONSOLE_DEBUG
	msg = "5()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
	// Build the trade log string
	string tradeLog, gDate;
	for(n = 0; n < size; ++n)
	{
		gDate = FromJulianDate(m_trades[n].jdate);
		tradeLog += gDate + ",";
		if(m_trades[n].signal == BUY)
			tradeLog += "LONG," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == SELL)
			tradeLog += "SHORT," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT_LONG)
			tradeLog += "EXIT LONG," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT_SHORT)
			tradeLog += "EXIT SHORT," + toString(m_trades[n].price,4);
		else if(m_trades[n].signal == EXIT)
			tradeLog += "EXIT," + toString(m_trades[n].price,4);

		tradeLog += "\r\n";
	}


#ifdef _CONSOLE_DEBUG
	msg = "6()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Get current PL for last position
	int open = 0;
	if(m_trades[size - 1].signal == BUY)
	{
		value = m_records[m_records.size() - 1].close - m_trades[size - 1].price;
		pl.push_back(value);
		plJDates.push_back(m_trades[size - 1].jdate);
		open = 1;
	}
	else if(m_trades[size - 1].signal == EXIT)
	{
		value = m_trades[size - 1].price - m_records[m_records.size() - 1].close;
		pl.push_back(value);
		plJDates.push_back(m_trades[size - 1].jdate);
		open = 1;
	}

#ifdef _CONSOLE_DEBUG
	msg = "7()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

	// Calculate the statistics
	CBackTestStatistics stats;

	// Get sum and total of Profit&Loss
	double totalProfit = 0, totalLoss = 0;
	double maxProfit = 0, maxLoss = 0;
	int numProfits = 0, numLosses = 0;
	for(n = 0; n < (int)pl.size(); ++n)
	{
		if(pl[n] > 0)
		{
			totalProfit += pl[n];
			if(pl[n] > maxProfit) maxProfit = pl[n];
			numProfits++;
		}
		else if(pl[n] < 0)
		{
			totalLoss += pl[n];
			if(pl[n] < maxLoss) maxLoss = pl[n];
			numLosses++;
		}
	}

#ifdef _CONSOLE_DEBUG
	msg = "8()";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
	if(numProfits > 0) numProfits -= open;
	if(numLosses > 0) numLosses -= open;

	if(numProfits + numLosses == 0)
	{
		return "Error: Scripts generated no trades.";		
	}

	string crlf = "\r\n";

	// Sum up the monthly P&L values
	vector<double> monthlyPL = stats.ConvertToMonthly(pl, plJDates);

	string results = "Back-Test de " + startDate + 
			" até " + endDate + crlf;

	// Total number of trades
	results += "Número Total de Operações: " + toString(pl.size()-open,0) + crlf;

	// Average number of trades per month
	results += "Média de Operações por Mês: " + toString(stats.m_avgTradesPerMonth,0) + crlf;
	
	// Num Profits
	results += "Número de Operações com Lucro: " + toString(numProfits,0) + crlf;
	
	// Num Losses
	results += "Número de Operações com Prejuízo: " + toString(numLosses,0) + crlf;

	// Total Profit
	results += "Lucro Total: " + toString(totalProfit,4) + crlf;

	// Total Loss
	results += "Prejuízo Total: " + toString(totalLoss,4) + crlf;

	// PctProfit	
	results += "Porcentagem de Lucro: " + toString(100 * (1 - (fabs(totalLoss) / fabs(totalProfit))),3) + "%" + crlf;
	
	// MaxProfit
	results += "Maior Lucro: " + toString(maxProfit,4) + crlf;

	// MaxLoss
	results += "Maior Prejuízo: " + toString(maxLoss,4) + crlf;

	results += "Drawdown Máximo: " + toString(stats.MaximumDrawdown(monthlyPL),4) + crlf;
	results += "Drawdown Máximo (Monte Carlo): " + toString(stats.MaximumDrawdownMonteCarlo(monthlyPL),4) + crlf;
	
	results += "Capitalização Mensal (Taxa de Retorno): " + toString(stats.CompoundMonthlyROR(monthlyPL),4) + crlf;
	//results += "Compound Quarterly ROR: " + toString(stats.CompoundQuarterlyROR(monthlyPL),3) + crlf;
	//results += "Compound Annualized ROR: " + toString(stats.CompoundAnnualizedROR(monthlyPL),3) + crlf;

	results += "Desvio Padrão: " + toString(stats.StandardDeviation(monthlyPL),8) + crlf;	

	results += "Desvio Padrão Anualizado: " + toString(stats.AnnualizedStandardDeviation(monthlyPL),8) + crlf;	
	
	results += "Semi-Variância (RMA = 10%): " + toString(stats.DownsideDeviation(monthlyPL, 0.1),8) + crlf;	

	results += "Índice de Valor Agregado Mensal (VAMI): " + toString(stats.ValueAddedMonthlyIndex(monthlyPL) / (stats.ValueAddedMonthlyIndex(monthlyPL) - 1), 8) + crlf;

	results += "Índice de Sharpe (RFR = 5%): " + toString(stats.SharpeRatio(monthlyPL, 0.05),8) + crlf;
	results += "Índice de Sharpe Anualizado (RFR = 5%): " + toString(stats.AnnualizedSharpeRatio(monthlyPL, 0.05),8) + crlf;

	results += "Índice de Sortino (MAR = 5%): " + toString(stats.SortinoRatio(monthlyPL, 0.05),8) + crlf;
	results += "Índice de Sortino Anualizado (MAR = 5%): " + toString(stats.AnnualizedSortinoRatio(monthlyPL, 0.05),8) + crlf;

	results += "Índice de Sterling (MAR = 5%): " + toString(stats.SterlingRatio(monthlyPL, 0.05),8) + crlf;	

	results += "Índice de Calmar: " + toString(stats.CalmarRatio (monthlyPL),8) + crlf;	
	
	if(numProfits > 0 && totalProfit != 0)
	{
		double r = ((1 - (numLosses / numProfits)) + (1 - (fabs(totalLoss) / totalProfit))) / 2;
		results += "Índice de Risco x Retorno: " + toString(r,3) + crlf;	
	}
	else{
		results += "Índice de Risco x Retorno: 0%" + crlf;
	}
	
	results += "Log:\n" + tradeLog;


//@#$)@#$&@)(#$(*@#)(@#$)(@)((@)#(#@
	//@#$)@#$&@)(#$(*@#)(@#$)(@)((@)#(#@
// Remember when formatting ASP page, Risk/Reward ratio is the one where
	// the red/yellow/green bar is set by. It goes from -1 to +1
	// anything below 0 is empty bar, anything above 1 is full bar
	// anything between 0 and 1 is % of bar. (width = width * ratio)
	return results;

}


string Script::GetData(string& script)
{

	string crlf = "\r\n";
	string results;

	if(script != "")
	{
		int maxBars = GetMaxBars(script);

		// Force functions to run only this symbol	
		m_watchVars = true;
		m_outputs.clear();

		// Run the script just to get the outputs
		RunScript(script, false);
		if(m_error != "") return m_error;		

		// Now m_outputs contains a list of all output variables
		results = "Date,Open,High,Low,Close,Volume";
		int n = 0;
		for(n = 0; n != m_outputs.size(); ++n)
		{
			variable var = GetVar(m_outputs[n]);			
			if(var.field.size() > 0)
			{
				string name = var.name;

				replace(name, "( ", "(");
				replace(name, " )", ")");			
				replace(name, "=(", "= (");
				replace(name, ",", ",");

				results += ",";
				results += '\"';
				results += name;
				results += '\"';

				//replace(name, ",", "|"); // For VB Split() function in ASP
				m_header += ",";				
				m_header += name;
				m_header += ": " + toString(var.value,4);
			}
		}
		results += crlf;

		// TODO:
		// LoadSymbolData has a problem loading the first bar into TA-SDK
		// so we don't display that here (the first indicator value is missing).
		for(n = 2; n < (int)m_records.size(); ++n)
		{			

			results += FromJulianDate(m_records[n].jDateTime) + ",";
			results += toString(m_records[n].open,4) + ",";
			results += toString(m_records[n].high,4) + ",";
			results += toString(m_records[n].low,4) + ",";
			results += toString(m_records[n].close,4) + ",";
			results += toStringL(m_records[n].volume,0);

			for(int j = 0; j != m_outputs.size(); ++j)
			{
				variable var = GetVar(m_outputs[j]);
				if((int)var.field.size() >= n)
				{
					results += "," + toString(var.field[n-1], 4);
				}
			}
			results += crlf;			
		}

	}
 
	// Must remember to clear this or all subsequent 
	// function calls will use only this symbol:
	m_watchVars = false; // Same with m_watchVars
	m_outputs.clear();
	
	replace(m_header, "( ", "(");

  
	return results;

}


string Script::RunBackTest(string& Script, int buySellExit)
{
	try{
#ifdef _CONSOLE_DEBUG
		string msg = "RunBackTest()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

		// Check to see if the parameters are valid	
		if (Script == "") return "Error: Missing script";
		if (Script.size() < 1) return "Error: Missing script";
		if (Script.size() > 63999) return "Error: Script too large";


		// Clear vars, price, and indicator arrays and other settings.
		makeupper(Script); // Convert script to upper case
		m_scriptHelp = "";
		m_error = "";
		m_exchange = "";
		m_sector = "";
		m_industry = "";
		m_symbol = "";
		m_helpString = "";
		m_lastFunctionName = "";
		m_maxBars = GetMaxBars(Script);
		variables.clear();
		primitiveVariables.clear();

		// First clean the script and find special instructions 
		// before evaluating. Also clear the arrays. This only 
		// needs to be done once for this session.   

#ifdef _CONSOLE_DEBUG
		msg = "a()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
		// Remove comments from the script (pound sign)
		string crlf = "\r\n";
		int found = 0, found2 = 0;
		found = Script.find("#");
		while (found > -1)
		{
			found2 = Script.find(crlf, found);
			if (found2 > -1)
				Script = Script.substr(0, found) + Script.substr(found2 + 2);
			else
				Script = Script.substr(0, found);

			found = Script.find("#");
		}

		// Remove extra white space
		replace(Script, "\t", " ");
		replace(Script, "  ", " ");
		replace(Script, crlf + crlf, crlf);

		// Exit if the script was just a bunch of comments - added 10/10/2006
		// Check for errors and return
		if (Script.size() == 0) return "Error: No script!";

#ifdef _CONSOLE_DEBUG
		msg = "b()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

		// Check to see if the exchange, sector, or industry is being set.
		// These commands are required on the first lines.    
		string block, lastStr;
		int max = -1;
		found = Script.find("SET EXCHANGE");
		if (found > max) max = found;
		found = Script.find("SET SECTOR");
		if (found > max) max = found;
		found = Script.find("SET INDUSTRY");
		if (found > max) max = found;
		found = Script.find("SET SYMBOL");
		if (found > max) max = found;
		if (max > -1)
		{
			found = Script.find("SET ", max);
			found = Script.find(crlf, found);
			if (found > -1)
				block = Script.substr(0, found);

		}
		if (max > -1)
		{
			replace(block, "SET ", "|SET_");
			replace(block, " ", "");
			found = block.find("EXCHANGE=");
			if (found > -1)
			{
				found2 = block.find("|SET_", found);
				if (found2 > -1)
				{
					m_exchange = block.substr(found + string("EXCHANGE=").length(),
						found2 - found - string("EXCHANGE=").length());
				}
				else
				{
					m_exchange = trim(block.substr(found + string("EXCHANGE=").length()));
					lastStr = m_exchange;
				}
			}
			found = block.find("SECTOR=");
			if (found > -1)
			{
				found2 = block.find("|SET_", found);
				if (found2 > -1)
				{
					m_sector = block.substr(found + string("SECTOR=").length(),
						found2 - found - string("SECTOR=").length());
				}
				else
				{
					m_sector = trim(block.substr(found + string("SECTOR=").length()));
					lastStr = m_sector;
				}
			}
			found = block.find("INDUSTRY=");
			if (found > -1)
			{
				found2 = block.find("|SET_", found);
				if (found2 > -1)
				{
					m_industry = block.substr(found + string("INDUSTRY=").length(),
						found2 - found - string("INDUSTRY=").length());
				}
				else
				{
					m_industry = trim(block.substr(found + string("INDUSTRY=").length()));
					lastStr = m_industry;
				}
			}
			found = block.find("SYMBOL=");
			if (found > -1)
			{
				found2 = block.find("|SET_", found);
				if (found2 > -1)
				{
					m_symbol = block.substr(found + string("SYMBOL=").length(),
						found2 - found - string("SYMBOL=").length());
				}
				else
				{
					m_symbol = trim(block.substr(found + string("SYMBOL=").length()));
					lastStr = m_symbol;
				}
			}
		}

		replace(m_exchange, crlf, "");
		replace(m_sector, crlf, "");
		replace(m_industry, crlf, "");
		replace(m_symbol, crlf, "");

#ifdef _CONSOLE_DEBUG
		msg = "c()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

		// Now get the main script
		found = Script.find(lastStr, 0);
		if (found > -1 && lastStr != "")
		{
			Script = trim(Script.substr(found + 2 + lastStr.length()).c_str());
		}



		// Store all variable assignments    
		m_assignments.clear();

		string assignment;
		found = Script.find("SET ");
		while (found > -1)
		{
			found = Script.find(crlf); // There must be another crlf or there will be no FIND STOCKS statement anyway
			if (found > -1)
			{
				assignment = Script.substr(0, found);
				m_assignments.push_back(assignment);
				Script = Script.substr(found + 2);
			}
			else
			{
				break;
			}
			found = Script.find("SET ");
		};



		// Find the market we are searching in, e.g
		// Stocks, Futures, Options, Forex, or Index    
		found = Script.find("FIND ");
		if (found > -1)
		{
			Script = trim(Script.substr(found + 4));
			found2 = Script.find("WHERE");
			if (found2 - found < 20)
			{
				m_market = trim(Script.substr(0, found2));
				Script = trim(Script.substr(found2 + string("WHERE").length()));
			}
		}

		replace(Script, crlf, " ");
		Script = Wrap(trim(Script));


#ifdef _CONSOLE_DEBUG
		msg = "d()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

		// Run the back test

		string results = "";

		//m_data = _AtlModule.data->GetSymbolData(m_symbol.c_str());
		//if(m_data == NULL || m_data->data.size() == 0) return "Error: Symbol not recognized";

		// There's not much reason to limit m_maxBars when backtesting since we're only running on one symbol:
		//if(m_data->data.size() < (size_t)m_maxBars * 2) return "Error: Symbol does not have an adequate amount of historic data on record for back testing. Your script requires " + toStringL(m_maxBars * 2) + " bars.";

		// See if we should not keep a position open
		// longer than x bars:
		size_t size = m_records.size();
		int maxOpen = size;
		if (isNumeric(m_maxPositionOpen) && m_maxPositionOpen != "")
			maxOpen = atol(m_maxPositionOpen.c_str());
		int lastTrade = -1;


		// Prevent script from executing too long
		m_startTimer = GetTickCount();
		size_t size2 = m_records.size();

		for (int n = (m_maxBars)/2; n != m_records.size() + 1; ++n)
		{
#ifdef _CONSOLE_DEBUG
			msg = "e("+to_string(n)+")";
			MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

			// Load data for this symbol
			if (LoadSymbolDataForBackTest(n))
			{
				// Process all user variable assignments
				for (int j = 0; j != m_assignments.size(); ++j)
				{
					m_isAssignment = true;
					Eval(Wrap(m_assignments[j])); // Evaluate the assignment to store the variable
					m_isAssignment = false;
				}


#ifdef _CONSOLE_DEBUG
				msg = "f(" + Script + ")";
				MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

				// Evaulate the script based on this symbol's data
				if (Eval(Script).value)
				{
					// The script evaluated to TRUE, so add the result to the list
					trade t;
					t.record = n;
					t.signal = buySellExit;
					t.jdate = m_records[n - 1].jDateTime;
					t.price = m_records[n - 1].close;

					string dt = FromJulianDate(t.jdate);

					m_trades.push_back(t);
					lastTrade = n;
				}

#ifdef _CONSOLE_DEBUG
				msg = "g()";
				MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
				// The user may request that a position not be kept
				// open longer than MAX_POSITION_OPEN records.
				if (n - lastTrade >= maxOpen && lastTrade != -1)
				{
					trade t;
					t.record = n;
					t.signal = EXIT;
					t.jdate = m_records[n - 1].jDateTime;
					t.price = m_records[n - 1].close;
					m_trades.push_back(t);
					lastTrade = n;
				}
#ifdef _CONSOLE_DEBUG
				msg = "h()";
				MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
				if (m_error != "") break;

				// Free the CPU
				MSG msg;
				while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
				{
					TranslateMessage(&msg);
					DispatchMessage(&msg);
				}
			}
			else{
				return results; // 5/14/08
			}

		} // Next window

		// Trim the last crlf
		if (results.length() > 2) results = results.substr(0, results.length() - 2);


#ifdef _CONSOLE_DEBUG
		msg = "i()";
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif

		// Check for errors and return
		if (m_error != "") results = m_error;
		return results;
	}
	catch (exception ex){

		string msg = ex.what();
		MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
	}
	return "Error: Exception";
}





// This function runs the script and returns a list of matching symbols, or an
// error message if the substring "error" is found within the returned string.
string Script::RunScript(string& script, bool iterate, bool validate /*=false*/)
{

	makeupper(script); // Convert script to upper case
	replace(script, "\t", " ");
	replace(script, "  ", " ");
	script = trim(script);
	if(script.size() < 1)
	{
		return "Error: Missing script";		
	}
	else if(script.size() > 63999)
	{
		return "Error: Script too large";		
	}
	
	// Clear vars, price, and indicator arrays and other settings.	
	m_scriptHelp = "";
	m_error = "";
	m_exchange = "";
	m_sector = "";
	m_industry = "";
	m_symbol = "";
	m_helpString = "";
	m_lastFunctionName = "";
	m_maxBars = GetMaxBars(script);
	variables.clear();

	m_watchVars = true;

    // First clean the script and find special instructions 
	// before evaluating. Also clear the arrays. This only 
	// needs to be done once for this session.   

    // Remove comments from the script (pound sign)
    string crlf = "\r\n";
	int found = 0, found2 = 0;
    found = script.find("#");
    while(found > -1)
	{
        found2 = script.find(crlf, found);
        if(found2 > -1)		
            script = script.substr(0, found) + script.substr(found2 + 2);
        else
			script = script.substr(0, found);

        found = script.find("#");
    }
    
    // Remove extra white space
	replace(script, "\t", " ");
	replace(script, "  ", " ");
	replace(script, crlf + crlf, crlf);	
	
	// Exit if the script was just a bunch of comments
	// Check for errors and return
	if(script.size() == 0) return "Error: No script!";


    // Check to see if the exchange, sector, or industry is being set.
    // These commands are required on the first lines.    
	string block, lastStr;
    int max = -1;    
    found = script.find("SET EXCHANGE");
    if(found > max) max = found;
    found = script.find("SET SECTOR");
    if(found > max) max = found;
    found = script.find("SET INDUSTRY");
    if(found > max) max = found;
	found = script.find("SET SYMBOL");
    if(found > max) max = found;
	if(max > -1)
	{
        found = script.find("SET ", max);
		found = script.find(crlf, found);
		if(found > -1)
			block = script.substr(0, found);

    }	
    if(max > -1)
	{
		replace(block, "SET ", "|SET_");
		replace(block, " ", "");
		found = block.find("EXCHANGE=");
        if(found > -1)
		{
            found2 = block.find("|SET_", found);
            if(found2 > -1)
			{
                m_exchange = block.substr(found + string("EXCHANGE=").length(), 
					found2 - found - string("EXCHANGE=").length());
			}
			else
			{
				m_exchange = trim(block.substr(found + string("EXCHANGE=").length()));
                lastStr = m_exchange;
            }
        }
		found = block.find("SECTOR=");
        if(found > -1)
		{
            found2 = block.find("|SET_", found);
            if(found2 > -1)
			{
                m_sector = block.substr(found + string("SECTOR=").length(), 
					found2 - found - string("SECTOR=").length());
			}
			else
			{
				m_sector = trim(block.substr(found + string("SECTOR=").length()));
                lastStr = m_sector;
            }
        }
		found = block.find("INDUSTRY=");
        if(found > -1)
		{
            found2 = block.find("|SET_", found);
            if(found2 > -1)
			{
                m_industry = block.substr(found + string("INDUSTRY=").length(), 
					found2 - found - string("INDUSTRY=").length());
			}
			else
			{
				m_industry = trim(block.substr(found + string("INDUSTRY=").length()));
                lastStr = m_industry;
            }
        }
		found = block.find("SYMBOL=");
        if(found > -1)
		{
            found2 = block.find("|SET_", found);
            if(found2 > -1)
			{
                m_symbol = block.substr(found + string("SYMBOL=").length(), 
					found2 - found - string("SYMBOL=").length());
			}
			else
			{
				m_symbol = trim(block.substr(found + string("SYMBOL=").length()));
                lastStr = m_symbol;
            }
        }
    }

	replace(m_exchange, crlf, "");
	replace(m_sector, crlf, "");
	replace(m_industry, crlf, "");
	replace(m_symbol, crlf, "");
	

	// Now get the main script
	found = script.find(lastStr,0);
	if(found > -1 && lastStr != "")
	{
		script = trim(script.substr(found + 2 + lastStr.length()).c_str());
	}



	
	// Store all variable assignments
    vector<string> assignments;
	
	string assignment;
    found = script.find("SET ");
    while(found > -1)
	{
        found = script.find(crlf); // There must be another crlf or there will be no FIND STOCKS statement anyway
        if(found > -1)
		{
            assignment = script.substr(0, found);
            assignments.push_back(assignment);
            script = script.substr(found + 2);
        }
		else
		{
            break;
		}        
        found = script.find("SET ");
    };

	replace(script, crlf, " ");
    script = Wrap(trim(script));
   


	// Run the script
	
	string results = "";
	size_t size = m_records.size();	
	m_validating = false;
	if(m_maxBars > (int)size && !validate)
	{
		return "Error: Not enough data available to run script";
	}
	else if(m_maxBars > (int)size && validate)
	{ // Just validating a script, insert fake data			
		record rec;
		rec.jDateTime = 1;
		rec.open = 1;
		rec.high = 1;
		rec.low = 1;
		rec.close = 1;
		rec.volume = 1;
		if(m_maxBars < 250) m_maxBars = 250;
		for(int p = 0; p != m_maxBars * 3; ++p)
			m_records.push_back(rec);

		size = m_records.size();
		m_validating = true;
	}

	m_startTimer = GetTickCount();

	// There are two ways to run a script:
	// 1. On the last bar only
	// 2. On all bars, iterating through in small windows
	if(iterate)
	{
	
		for(int n = m_maxBars; n < (int)size; ++n)
		{
		
			// Load data for this window
			if(LoadSymbolData(n))
			{
				// Process all user variable assignments
				for(int j = 0; j != assignments.size(); ++j)							
					Eval(Wrap(assignments[j])); // Evaluate the assignment to store the variable			

				// Evaulate the script based on this window
				if(Eval(script).value)
				{
					// The script evaluated to TRUE, so add the record number

					
				}

				if(m_error != "") break;

			}

		} // Next window

	}

	else // Just run script from the last record
	{

		if(LoadSymbolData(m_records.size() - 1) || m_validating)
		{
			// Process all user variable assignments
			for(int j = 0; j != assignments.size(); ++j)							
				Eval(Wrap(assignments[j])); // Evaluate the assignment to store the variable			

			// Evaulate the script based on this window
			if(Eval(script).value)
			{
				// The script evaluated to TRUE
				results = "ALERT";
			}
		}

	}


	// Check for errors and return
	m_watchVars = false;
	if(m_error != "") results = m_error;	
	return results;
	
}



// Evalutes a line of script
variable Script::Eval(string expr)
{	

#ifdef _CONSOLE_DEBUG
	string msg = "Eval("+expr+")";
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
   // Prevent script from executing too long
	if(GetTickCount() - m_startTimer > SCRIPT_TIME_OUT)
	{
		m_error = "Error: The script timed out. Please shorten your script and try again.";
		m_scriptHelp = "timeout";
	}

    string assignToVar, operand;
    variable value;
	double tempVal = 0;
    long pos = 0, prevPos = 0, found = 0;
    		
    // See if this expression has already been evaluated
	variable nextToken;
    int n = 0, var = 0;
    string varName;
    bool isField = false;
    varName = CreateVarName(expr);
    for(n = 0; n != variables.size(); ++ n)
	{
		if(variables[n].name == varName) // It has!
		{
            return variables[n];
		}
	}
	for(n = 0; n != primitiveVariables.size(); ++ n)
	{
		if(primitiveVariables[n].name == varName) // Its a primitive!
		{
            return primitiveVariables[n];
		}
	}
    // It hasn't! So save it
    variable v;
	v.name = varName;
	variables.push_back(v);
    var = variables.size() - 1;
	variables[var].field.resize(m_maxBars);
	// Only create vectors if at least one part of the
	// expression is based on the base OHLCV prices:
    if((instr(expr, "OPEN") > -1 || instr(expr, "HIGH") > -1 || 
        instr(expr, "LOW") > -1 || instr(expr, "CLOSE") > -1 || 
        instr(expr, "VOLUME") > -1) && instr(expr, "(") == 0) isField = true;	



    // Check to see if assigning the value to a variable
    expr = trim(expr);
	m_isAssignment = false;
	if(expr.substr(0, 3) == "SET")
	{    
        expr = trim(expr.substr(4));
        found = expr.find("=");
        if(found != -1)
		{
			m_isAssignment = true;
			assignToVar = trim(expr.substr(0,found));
            expr = trim(expr.substr(found + 1));
			varName = CreateVarName(expr);
        }
    }
    
	pos = 1;
	string chr, chr2;
	while((size_t)pos < expr.size() + 1)
	{

		chr = expr.substr(pos - 1, 3);
		if(chr == "NOT" || chr == "OR " || chr == "AND" ||
		   chr == "XOR" || chr == "EQV" || chr == "IMP")
		{
            operand = expr.substr(pos - 1, 3);
            pos += 3;
        }

		chr = expr.substr(pos - 1, 1);    
        if(chr == " ")
		{
			pos++;		
        
		}

		else if(chr == "&" || chr == "+" || chr == "-" ||
				chr == "*" || chr == "/" || chr == "\\" || chr == "^")
		{
			operand = expr.substr(pos - 1, 1);
			pos++;
		}
        
		else if(chr == ">" || chr == "<" || chr == "=" || chr == "!")
		{

			chr2 = expr.substr(pos, 1);
			if(chr2 == ">" || chr2 == "<" || chr2 == "=")
			{
				operand = expr.substr(pos - 1, 2);
				pos++;			
			}
			else
			{
				operand = expr.substr(pos - 1, 1);
			}
			pos++;
		}

		else
		{

			if(operand == "")
			{
				value = Token(expr, pos);
			}
			else if(operand == "&")
			{				
				value.value = Token(expr, pos).value;
			}

			// Begin vector math - RG

			else if(operand == "*" || operand == "/" || 
					operand == "\\" || operand == "^" || 
					operand == "-" || operand == "+" ||
					operand == ">" || operand == "<" ||
					operand == ">=" || operand == "=<" ||
					operand == "=>" || operand == "<=" ||
					operand == "=" || operand == "==" ||
					operand == "!=" || operand == "<>")
			{
				nextToken = Token(expr, pos);
				if(value.field.size() == 0) value.field.resize(m_maxBars);
		
				if(operand == "*")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] *= nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] *= nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] *= nextToken.value;						
					}
				}

				if(operand == "/" || operand == "\\")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] /= nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] /= nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] /= nextToken.value;						
					}
				}

				if(operand == "^")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = pow(variables[var].field[n], nextToken.field[n]);
							if(value.field.size() > 0)
								value.field[n] = pow(variables[var].field[n], nextToken.field[n]);
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = pow(variables[var].field[n], nextToken.value);
					}
				}

				if(operand == "-")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] -= nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] -= nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] -= nextToken.value;						
					}
				}
		
				if(operand == "+")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] += nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] += nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{						
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] += nextToken.value;						
					}
				}
				

				if(operand == ">")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] > nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] > nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] > nextToken.value;						
					}
					
					value.value = (value.value > nextToken.value);

				}


				if(operand == "<")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] < nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] < nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] < nextToken.value;						
					}

					value.value = (value.value < nextToken.value);
				}



				if(operand == "<=" || operand == "=<")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] <= nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] <= nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] <= nextToken.value;						
					}
					
					value.value = (value.value <= nextToken.value);
				}


				if(operand == ">=" || operand == "=>")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] >= nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] >= nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] >= nextToken.value;						
					}

					value.value = (value.value >= nextToken.value);
				}


				if(operand == "!=" || operand == "<>")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] != nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] != nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] != nextToken.value;						
					}

					value.value = (value.value != nextToken.value);
				}


				if(operand == "==" || operand == "=")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] == nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] == nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] == nextToken.value;						
					}

					value.value = (value.value == nextToken.value);
				}


				if(operand == "NOT" || operand == "!")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] = !nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] = !nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] = !nextToken.value;						
					}

					value.value = value.value = !nextToken.value;
				}


				if(operand == "AND" || operand == "&&")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							tempVal = nextToken.field[n];
							if(variables[var].field[n] && tempVal != 0)
								variables[var].field[n] = tempVal;							
							else
								variables[var].field[n] = 0;
							
							if(value.field.size() > 0)
							{
								tempVal = nextToken.field[n];
								if(value.field[n] && tempVal != 0)
									value.field[n] = tempVal;
								else
									value.field[n] = 0;
							}
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							tempVal = nextToken.field[n];
							if(value.field[n] && tempVal != 0)
								value.field[n] = tempVal;
							else
								value.field[n] = 0;
						}
					}

					if(value.value && nextToken.value != 0)
						value.value = nextToken.value;
					else
						value.value = 0;					
				}



				if(operand == "OR " || operand == "||")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = variables[var].field[n] || nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = value.field[n] || nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = value.field[n] || nextToken.value;						
					}

					value.value = value.value || nextToken.value;
				}


				if(operand == "XOR" || operand == "|")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = (long)variables[var].field[n] | (long)nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = (long)value.field[n] | (long)nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = (long)value.field[n] | (long)nextToken.value;						
					}

					value.value = (long)value.value | (long)nextToken.value;
				}


				if(operand == "EQV" || operand == "&")
				{
					if(variables[var].field.size() > 0 && nextToken.field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)
						{
							variables[var].field[n] = (long)variables[var].field[n] & (long)nextToken.field[n];
							if(value.field.size() > 0)
								value.field[n] = (long)value.field[n] & (long)nextToken.field[n];
						}
					}
					else if(variables[var].field.size() > 0)
					{
						for(n = 0; n != variables[var].field.size(); ++n)						
							value.field[n] = (long)value.field[n] & (long)nextToken.value;						
					}

					value.value = (long)value.value & (long)nextToken.value;
				}



                if(operand == "*")
                    value.value *= nextToken.value;
                else if(operand == "/" || operand == "\\")
                    value.value /= nextToken.value;
                else if(operand == "^")
                    value.value = pow(value.value, nextToken.value);
				else if(operand == "+")
                     value.value += nextToken.value;
				else if(operand == "-")
                     value.value -= nextToken.value;
                
                // End vector math


			}

			else if(operand == "NOT")
			{						
				value.value = !Token(expr, pos).value;
			}	
			else if(operand == "AND")
			{		
				tempVal = Token(expr, pos).value;
				if(value.value && tempVal != 0)
					value.value = tempVal;
				else
					value.value = 0;
			}
			else if(operand == "OR ")
			{
				tempVal = Token(expr, pos).value;
				value.value = (value.value || tempVal);
			}
			else if(operand == "XOR")
			{
				tempVal = Token(expr, pos).value;
				value.value = ((long)value.value | (long)tempVal);
			}
			else if(operand == "EQV")
			{
				tempVal = Token(expr, pos).value;
				value.value = ((long)value.value & (long)tempVal);
			}
			else if(operand == "=" || operand == "==")
			{
				tempVal = Token(expr, pos).value;
				value.value = (value.value == tempVal);
			}
			else if(operand == ">")
			{
				variable temp = Token(expr, pos);
				value.value = (value.value > temp.value);
			}
			else if(operand == "<")
			{
				value.value = (value.value < Token(expr, pos).value);
			}
			else if(operand == "<=" || operand == "=<")
			{				
				value.value = (value.value <= Token(expr, pos).value);
			}
			else if(operand == ">=" || operand == "=>")
			{
				value.value = (value.value >= Token(expr, pos).value);
			}
			else if(operand == "<>" || operand == "!=")
			{
				value.value = (value.value != Token(expr, pos).value);
			}

		}

		// Prevent deadlocking
		if(prevPos == pos) break;
		prevPos = pos;

		// Check for errors
		if(m_error != "") break;
    
	};


	// Make sure all "SET" lines resize the field
	// TODO: This only needs to happen if the LOOP
	// function is used. You can optionally check the
	// script to see if "LOOP(" is in it to save some
	// memory instead of resizing the entire field:
	if(value.field.size() == 0 && assignToVar != "")
	{
		value.field.resize(m_maxBars);
		for(int n = 0; n != m_maxBars; ++n)
			value.field[n] = value.value;
	}


	variables[var].field  = value.field;
	variables[var] = value;
	variables[var].name = varName;

    if(assignToVar != "") SetVar(assignToVar, value);
	m_isAssignment = false; // Reset this global flag;
	return variables[var];


	/*
    variables[var].value = ret.value + value.value;

    for(n = 0; n != variables[var].field.size(); ++n)
	{
		if(value.field.size() > 0 && variables[var].field.size() > 0)
			variables[var].field[n] += value.field[n];
		else
			variables[var].field[n] += value.value;
	}

    if(assignToVar != "") SetVar(assignToVar, value);
    
    return variables[var];
	*/


}


// Tokenizer for Eval function. Increments pos.
variable Script::Token(string expr, long& pos)
{	
	string chr, value, fn, v, newValue, item;
    int es = 0, pl = 0;
    long n = 0;
	bool isNum = false;
	variable ret;
	variable var;	
    
	if(pos < 1) return ret;

	while((size_t)pos <= expr.size())
	{
		chr = expr.substr(pos - 1, 1);

		if(chr == "&" || chr == "+" || chr == "-" || chr == "/" || 
		   chr == "\\" || chr == "*" || chr == "^" || chr == " " || chr == ">" || 
		   chr == "<" || chr == "=")
		{
			break;
		}
		else if(chr == "(")
		{
			pl = 1;
			pos++;
			es = pos;

			while(pl != 0 && (size_t)pos <= expr.size() + 1)
			{            				
				chr = expr.substr(pos - 1, 1);
				if(chr == "(") pl++;
				if(chr == ")") pl--;                
				pos++;
			}

			value = expr.substr(es - 1, pos - es - 1);
			fn = item;
			m_lastFunctionName = fn;
			
			if(item == "")
			{
				var = Eval(value);
                item = toString(var.value);
				isNum = true;
			}
			else
			{
				item = trim(value);
				// Evaluate the arguments first
				while(true)
				{
					n++;
					v = GetArg(value, n, 0);
					if(v == "") break;
					var = Eval(v);
					if(var.field.size() > 0) // Was a vector returned?
						newValue += var.name + ", ";
					else // No vector returned
						newValue += toString(var.value) + ", ";
				}
				if(newValue.size() > 2) newValue = newValue.substr(0, newValue.size() - 2);
				ret = EvalFunction(fn, newValue, var, value);					
			}
			 
			break;

		}
        
		else{
			item += chr;
			pos++;
		}

	};


	if(m_error != "") return ret; // Something has gone wrong with EvalFunction
    
	if(ret.field.size() > 0) // Was a vector returned?
	{
		// No action required
	}
	else // No vector returned, check for variable
	{
		ret.field.clear();
        if(isNumeric(item.c_str()) && item != "")
		{
			ret.field = var.field;
			ret.value = atof(item.c_str());
		}
		else if(item != "")
		{
			if(fn != "IF") ret = GetVar(item);
		}
		else
		{
			return ret;
		}
	}

	return ret;

}


// Ensures function with no arguments have parentheses
__inline void Script::noArgs(string& script, string func)
{
	replace(script, func + " ", func);
	replace(script, func, func + "()");
	replace(script, func + "( )", func + "()");
	replace(script, func + "()()", func + "()");	
}


// Adds parentheses where needed. Warning! Only call once per session.
// 1 + 2 = 3 becomes (1 + 2) = 3
// 3 = 1 + 2 becomes 3 = (1 + 2)
// A + B / C = 1 + 2 + 3 becomes (A + B / C) = (1 + 2 + 3)
string Script::Wrap(string script)
{
	string left;
    int found = -1;
    if(script.substr(0,3) == "SET")
	{	
        found = script.find("=");
        if(found > -1)
		{
            left = script.substr(0, found + 1);
            script = trim(script.substr(found + 2));
		}
	}
	if(script == "") return script;

	// Make sure there's at least one space before and after each = sign
	replace(script, "=", " = ");
	replace(script, "> =", ">=");
	replace(script, "< =", "<=");
	replace(script, "= >", "=>");
	replace(script, "= <", "=<");
	replace(script, "! =", "!=");
	replace(script, "=  =", "==");	
	
    string assign[14];

	assign[0] = "==";
	assign[1] = "=<";
	assign[2] = "!=";
	assign[3] = "<>";
	assign[4] = ">=";
	assign[5] = "=>";
	assign[6] = "<=";
	assign[7] = "=<";
	assign[8] = "=<";	
	assign[9] = "=";
	assign[10] = "<";
	assign[11] = ">";
	    
    assign[12] = "AND";
    assign[13] = "OR";
    
	bool hasEqv = false;
    string op;
	int n = 0;
    for(n = 0; n != 12; ++n)
	{
		op = assign[n];		
		replace(script, " " + op + " ", "%" + op + "%");		
		replace(script, "%" + op + "%", ") " + op + " (");		
    }

    for(n = 12; n != 14; ++n)
	{
		op = assign[n];
		if(script.find(op) != string::npos) hasEqv = true;
		replace(script, " " + op + " ", "%" + op + "%");		
		replace(script, "%" + op + "%", ")) " + op + " ((");		
    }

	if(hasEqv)
	{
		script = "((" + script;
		script = script + "))";
	}
	else
	{
		script = "(" + script;
		script = script + ")";
	}

    // Make sure there are no spaces between a parentheses and a function name
    replace(script, "  ", " ");
    replace(script, " (", "(");
	replace(script, " )", ")");


	// Make sure there are parentheses around functions that may have no arguments
	noArgs(script, "HIGHMINUSLOW");
	noArgs(script, " HML");
	noArgs(script, "MEDIANPRICE");	
	noArgs(script, "TYPICALPRICE");	
	noArgs(script, "WEIGHTEDCLOSE");	
	noArgs(script, "TRUERANGE");	
	noArgs(script, "PRIMENUMBERBANDS");
	noArgs(script, "PRIMENUMBEROSCILLATOR");
//	noArgs(script, "PARABOLICSAR"); Commented out 10/10/06
//	noArgs(script, "(PSAR");
//	noArgs(script, " PSAR");
	noArgs(script, " PNO");
	noArgs(script, "(PNO");
	noArgs(script, " PNBT");
	noArgs(script, "(PNBT");
	noArgs(script, " PNBB");
	noArgs(script, "(PNBB");
	noArgs(script, "(WAD");
	noArgs(script, " WAD");
	noArgs(script, "WILLIAMSACCUMULATIONDISTRIBUTION");
	noArgs(script, "CANDLESTICKS");
	noArgs(script, "CANDLESTICKPATTERN");
	noArgs(script, " CSP");
	noArgs(script, "(CSP");

    
    for(n = 12; n != 14; ++n)
	{
		op = assign[n];
        replace(script, assign[n] + ")", assign[n] + " )");
        replace(script, assign[n] + "(", assign[n] + " (");
	}


	if(script == "" || script == "()") return "";

	// Special fix for IF functions
    //replace(script, "IF(", "IF((");
	for(n = 3; n != script.size(); ++n)
	{
		if(script.substr(n - 2, 3) == "IF(")
		{
			script = script.substr(0, n) + "(" + script.substr(n);
			n += 2;
		}
	}
	string::size_type pos = 0;
	string::size_type ppos = 0;	
    while(true)
	{
		pos = script.find( "(IF(", pos );
        if(pos == string::npos) pos = script.find( " IF(", pos );
        if(pos != string::npos)
		{
			pos = script.find( ",", pos );            
            if(pos != string::npos)
			{
                script = script.substr(0, pos) + ")" +
					script.substr(pos);
			}
		}
		else
		{
            break;
		}
	};



	// Same special fix for SUMIF functions
	if(script.size() > 5) // 7/28/08
	{
		for(n = 6; n != script.size(); ++n)
		{
			if(script.substr(n - 2, 6) == "SUMIF(")
			{
				script = script.substr(0, n) + "(" + script.substr(n);
				n += 2;
			}
		}
	}
	pos = 0;
	ppos = 0;	
    while(true)
	{
		pos = script.find( "(SUMIF(", pos );
        if(pos == string::npos) pos = script.find( " SUMIF(", pos );
        if(pos != string::npos)
		{
			pos = script.find( ",", pos );            
            if(pos != string::npos)
			{
                script = script.substr(0, pos) + ")" +
					script.substr(pos);
			}
		}
		else
		{
            break;
		}
	};


 	return left + script;
}


__inline bool Script::GetVectorFromArg(string arg, variable& var)
{
	if(arg == "") return false;
	if(isNumeric(arg)) // Assume single value, no vector
	{
		var.value = atof(arg.c_str());
		var.field.resize(m_maxBars);
		for(int n = 0; n != m_maxBars; ++n)
			var.field[n] = var.value;	
		return true;
	}
    var = GetVar(arg);
	if(var.field.size() == 0) return false;
	return true;
}

__inline bool Script::GetValueFromArg(string func, string arg, double& value, bool isRef /*=true*/, bool isMAType /*=false*/)
{
	if(arg == "") return false;
    if(!isNumeric(arg)) return false;
	value = atof(arg.c_str());
	if((isRef) && value > m_maxBars - 2)
	{
		ThrowError("Function '" + func + "') contains arguments outside of allowed range.");
		m_scriptHelp = func;
		return false;
	}
	else if(isMAType && (value <= MA_START || value >= MA_END))
	{
		ThrowError("Function '" + func + "' contains an invalid moving average type.");
		m_scriptHelp = func;
		return false;
	}
	return true;
}

void Script::GetArg(string argString, int minArgsError, 
		string* arg1 /*= NULL*/, string* arg2 /*=NULL*/, 
		string* arg3 /*=NULL*/, string* arg4 /*=NULL*/, 
		string* arg5 /*=NULL*/, string* arg6 /*=NULL*/, 
		string* arg7 /*=NULL*/, string* arg8 /*=NULL*/)
{
if(arg1 != NULL) *arg1 = GetArg(argString, 1, minArgsError);
	if(arg2 != NULL) *arg2 = GetArg(argString, 2, minArgsError);
	if(arg3 != NULL) *arg3 = GetArg(argString, 3, minArgsError);
	if(arg4 != NULL) *arg4 = GetArg(argString, 4, minArgsError);
	if(arg5 != NULL) *arg5 = GetArg(argString, 5, minArgsError);
	if(arg6 != NULL) *arg6 = GetArg(argString, 6, minArgsError);
	if(arg7 != NULL) *arg7 = GetArg(argString, 7, minArgsError);
	if(arg8 != NULL) *arg8 = GetArg(argString, 8, minArgsError);
}

// Returns an argument from a string list.
// Throws an error if the minimum number of arguments aren't found.
string Script::GetArg(string argString, int argNum, int minArgsError)
{

	long cnt = 0, pcnt = 0, p = 0;
	bool b = false;
	string ret = "";    
	size_t size = argString.size();
	int leftCount = 0, rightCount = 0;
	for(int n = 0; n != size; ++n)
	{
		if(argString.substr(n,1) == "(") rightCount++;
		if(argString.substr(n,1) == ")") leftCount++;		
		if(argString.substr(n,1) == "," && leftCount == rightCount)
		{
			leftCount = 0;
			rightCount = 0;
			cnt++;			
			if(cnt == argNum)
			{
				b = true;
				ret = trim(argString.substr(p, n - p));			
			}
			p = n + 1;
		}
    }
	cnt++;
	if(!b && argNum <= cnt) ret = trim(argString.substr(p));
	if(cnt < minArgsError)
	{
		if(m_lastFunctionName != "")
		{
			m_error = "Error: Argument of function '" + m_lastFunctionName + "' not optional.";
			m_scriptHelp = m_lastFunctionName;
		}
		else
		{
			m_error = "Error: Argument not optional";			
		}
	}

	return ret;
}


// Sets or creates a variable.
__inline int Script::SetVar(string& name, variable value)
{
	if(name == "") return -1;
	for(int n = 0; n != variables.size(); ++n)
	{
		if(variables[n].name == name)
		{
			if(variables[n].field.size() > 0)
			{
				for(int j = 0; j != m_maxBars; ++j)
				{
					if(value.field.size() > 0)
                        variables[n].field[j] = value.field[j];
					else
						variables[n].field[j] = value.value;                    
				}
			}
            variables[n].value = value.value;
			return n;
		}
	}

	if(variables.size() > MAX_VARIABLES)
	{
		// Prevent DOS attacks
		m_error = "Error: Script too complex.";
		m_scriptHelp = "timeout";
		return -1;
	}


	value.name = name;
	variables.push_back(value);

	if(m_watchVars) // Are we compiling a list of all variable names?
	{
		//if(m_isAssignment)
		//{	// Could limit what to display in GetData() here
		//}
		// But for now, just add everything: variables, constants, and functions
		m_outputs.push_back(name);
	}

	return variables.size() - 1;
}

// Finds a variable
__inline variable Script::GetVar(string name)
{

	variable ret;

	//**
	// Alias field names can be set here

	// The last price is the same as the close
	if(name == "LAST") name = "CLOSE";

	//***

	// Lookup a user defined variable
	int n = 0;
	for(n = 0; n != variables.size(); ++n)		
		if(variables[n].name == name) return variables[n];

	// Lookup a primitive variable
	for(n = 0; n != primitiveVariables.size(); ++n)
		if(primitiveVariables[n].name == name) return primitiveVariables[n];

	// If we got here this must be an undefined variable!
	m_error = "Error: Undefined variable '" + name + "'";
   
	return ret;
}

// Returns a variable with a sized field if the calculated vector already exists.
variable Script::GetFunctionVector(string evalString, string functionName)
{
    string varName;
    variable empty;

    if(functionName == "") return empty;

	if(functionName == "OPEN" || functionName == "HIGH" || functionName == "LOW" || 
		functionName == "CLOSE" || functionName == "VOLUME") return empty;
    
    if(evalString != "")
        varName = CreateVarName(evalString, functionName);
	else
        varName = functionName;
    
	for(int n = 0; n != variables.size(); ++n)
	{
		if(variables[n].name == varName) // It has!
			return variables[n];
	}

	// This is the first time this expression has been evaluated.
	if(m_watchVars) // Are we compiling a list of all variable names?
	{
//		if(m_forceSymbol == m_data->szSymbol)
//			m_outputs.push_back(varName);
	}

	empty.name = varName;
	return empty;
}

// Gives variables a unique name based on their expression
string Script::CreateVarName(string& expr, string prefix /* = "" */)
{
	string ret = expr;
    replace(ret, "( ", "(");
    replace(ret, " )", ")");
	replace(ret, ", ", ",");
	replace(ret, ",", ", ");
    if(prefix != "")  ret = prefix + "( " + ret + " )";
    return ret;
}


// Improves performance by setting the maximum number of bars
int Script::GetMaxBars(string& script)
{

	string chr;
	string sNum;
	int left = 0, right = 0;
	bool thisNumeric = false, prevNumeric = false;
	double dNum = 0, dMax = 0;
	for(int n = 0; n != script.size(); ++n)
	{
		chr = script.substr(n,1);
		if(chr == "(") left += 1;
		if(chr == ")") right += 1;

		thisNumeric = isNumeric(chr);
		if(thisNumeric && (left != right))
		{
			sNum += chr;
		}
		else
		{			
			dNum = atof(sNum.c_str());
			if(dNum < 1000)			
				if(dNum > dMax) dMax = dNum;
			sNum = "";
		}
		prevNumeric = thisNumeric;
	}
	
	// NOTE: If you wrap a number in parenthesis, it may be
	// used as dMax. If its not, it won't be. Example:
	// (CLOSE > 50) AND OPEN > 50.... dMax becomes 100
	// CLOSE > 50 AND OPEN > 50.... dMax becomes 0
	// SMA(CLOSE, 50) > 220... dMax becomes 100

	// REMOVED 16/12/2014
	dMax *= 2;

	//if(dMax > m_records.size()) dMax = m_records.size(); // Commented out 11/11/07
	if(dMax < 5) dMax = 5;
	// Changed from dMax < 3 on 11/11/07
	

	// This may be a constant, so don't throw an error yet!
	// We're not sure if its a function parameter at this time and it doesn't matter.
	// The goal of this function is to just get the smallest size return value as
	// possible (multiplied by 2 for exponential indicators).

	/*
	// Some indicators require a minimum number of bars to prime with:
	if(instr(script, "PSAR") > -1 || instr(script, "PARABOLICSAR") > -1 ||
	  instr(script, "PI") > -1 || instr(script, "PERFORMANCEINDEX" ) > -1 ||
	  instr(script, "PVI") > -1 || instr(script, "POSITIVEVOLUMEINDEX") > -1 ||
	  instr(script, "NVI") > -1 || instr(script, "NEGATIVEVOLUMEINDEX" ) > -1)		
	{
		if(dMax < 60) dMax = 60;
	}
	*/
	
	//###
	// 3-3-06
	// Some indicators like EMA were showing different results in the scan
	// than in the actual charts and data. This is because exponential indicators
	// require a lot more data than just 7 or 8 bars, so the following line was added:	
	//dMax += 100;
	// This is slightly inconvenient in terms of performance however the results were
	// inaccurate the other way. 100 is the magic number - don't change it.
	//###

//dMax = 15;
//@#$@*(#$(
//	  Find out why script isn't matching up with Excel sheet
//	  then confirm indicator values and outputs.
//@#($@&(#)

	return (int)dMax + 30; // REMOVED +30 16/12/14 -> Added +20 6/7/09, can go as high as 100, 200, 300 etc.
	// Because alerts, scans and back tests are performed on a sliding window basis
	// where each bar is evaluated in a sliding window, you must set this value high
	// if you intend for the results to match with the output on a chart. The values
	// are used for priming exponential calculations and indexes. Without adding a
	// value here, the results would not match. The higher you set the value, the more
	// bars will be missing in the ScriptOutput for display on a chart, at the beginning
	// of the chart. Those values are used for priming.
}


variable Script::ThrowMissingArgError(string function, string argument)
{
	variable ret;
	if(m_error != "") return ret; // Don't overwrite an existing error
	m_error = "Error: '" + argument + "' argument of '" + function + "' not optional.";	
	m_scriptHelp = function;
	return ret;
}

variable Script::ThrowInvalidArgError(string function, string argument)
{
	variable ret;
	if(m_error != "") return ret; // Don't overwrite an existing error
	m_error = "Error: '" + argument + "' argument of '" + function + "' is invalid.";
	m_scriptHelp = function;
	return ret;
}

variable Script::ThrowError(string error)
{
	variable ret;
	if(m_error != "") return ret; // Don't overwrite an existing error
	m_error = "Error: " + error;	
	return ret;
}









////////////////////////////////////////////////////////////////////////////////////
//**********************************************************************************
//							Wrappers for TA-SDK functions
//**********************************************************************************
////////////////////////////////////////////////////////////////////////////////////


// Evaluates a function and returns the result. name is the name of the function,
// argString contains the arguments (use GetArg) and evalString was the un-evaluated 
// parameters before processing, for reference and error reporting purposes.
variable Script::EvalFunction(string function, string argString, variable var, string evalString /* = "" */)
{
#ifdef _CONSOLE_DEBUG
	string msg = "EvalFunction() "+function;
	MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);
#endif
	variable ret;
	string v1, v2, v3, v4, v5, v6, v7, v8;
	variable var1, var2, var3, var4, var5, var6, var7, var8;
	CTASDK tasdk;

	int periods = 0;
	int shift = 0;
	int maType = 0;
	m_lastFunctionName = function;
 
	ret.field.resize(m_maxBars);

    // See if the function has already been evaluated:
    ret = GetFunctionVector(evalString, function);
    if(ret.field.size() > 0) return ret;

	string varName = CreateVarName(function, argString);


	////////////////////////////////////////////////////////////////////////
	// Math functions
	////////////////////////////////////////////////////////////////////////

	if(function == "ABS" || function == "SIN" || function == "COS" || 
	   function == "TAN" || function == "EXP" || function == "LOG" || 
	   function == "LOG10" || function == "ATN" || function == "RND")
	{
		GetArg(argString, 1, &v1);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");	
		ret.field.resize(m_maxBars);

		if(function == "ABS")
		{
			ret.value = abs(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = fabs(var1.field[n]);
		}
		else if(function == "SIN")
		{
			ret.value = sin(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = sin(var1.field[n]);
		}
		else if(function == "COS")
		{
			ret.value = cos(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = cos(var1.field[n]);
		}
		else if(function == "TAN")
		{
			ret.value = tan(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = tan(var1.field[n]);
		}
		else if(function == "EXP")
		{
			ret.value = exp(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = exp(var1.field[n]);
		}
		else if(function == "LOG")
		{
			ret.value = log(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = log(var1.field[n]);
		}
		else if(function == "LOG10")
		{
			ret.value = log10(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = log10(var1.field[n]);
		}
		else if(function == "ATN")
		{
			ret.value = atan(var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = atan(var1.field[n]);
		}
		else if(function == "RND")
		{
			srand((unsigned int)time(NULL));
			ret.value = ((double)rand() / RAND_MAX * var1.value);
			for(int n = 0; n != var1.field.size(); ++n)
				ret.field[n]  = ((double)rand() / RAND_MAX * var1.value);
		}
    }


	// Conditional IF function
	else if(function == "IF")
	{
		GetArg(argString, 3, &v1, &v2, &v3);        
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "argument 2");
		if(!GetVectorFromArg(v3, var3)) return ThrowMissingArgError(function, "argument 3");

		if(instr(v1, " AND ") > -1 || instr(v1, " OR ") > -1)
		{
			return ThrowInvalidArgError(function, "IF function must contain only one condition, without the 'AND' or 'OR' operators");
		}

		// Return the index of the field that is referenced		
		ret.field.resize(m_maxBars);
		// Get a temp variable with this condition
		v1 = "(" + v1 + ")"; // IMPORTANT!
		
		// The m_runningIfFunction flag tells the vector math processor in Eval and Token
		// that the field being worked on can be modified if it is a conditional operator.
		m_runningIfFunction = true;
		variable cond = Eval(v1.c_str());
		m_runningIfFunction = false;

		bool condition = false;
		if(cond.field.size() == 0)
		{
			if(cond.value != 0)
				ret.value = var2.value;
			else
				ret.value = var3.value;
		}
		else
		{
			for(int n = 0; n != ret.field.size(); ++n)
			{
				
				condition = cond.field[n] != 0;
				if(condition)
					ret.field[n] = var2.field[n];
				else
					ret.field[n] = var3.field[n];
			}
		}
    }



	// Conditional COUNTIF function - same as above but returns a vector of numbers 
	else if(function == "COUNTIF")
	{
		GetArg(argString, 1, &v1);        		
		
		if(instr(v1, " AND ") > -1 || instr(v1, " OR ") > -1)
		{
			return ThrowInvalidArgError(function, "COUNTIF function must contain only one condition, without the 'AND' or 'OR' operators");
		}

		// Return the index of the field that is referenced		
		ret.field.resize(m_maxBars);
		// Get a temp variable with this condition
		v1 = "(" + v1 + ")"; // IMPORTANT!
		
		// The m_runningIfFunction flag tells the vector math processor in Eval and Token
		// that the field being worked on can be modified if it is a conditional operator.
		m_runningIfFunction = true;
		variable cond = Eval(v1.c_str());
		m_runningIfFunction = false;

		int count = 0;
		bool condition = false;
		if(cond.field.size() == 0)
		{
			if(cond.value != 0)
				ret.value = 1;
			else
				ret.value = 0;
		}
		else
		{
			for(int n = 0; n != ret.field.size(); ++n)
			{				
				condition = cond.field[n] != 0;
				if(condition)
					count++;			
			
				ret.field[n] = count;
			}
		}
    }



	// Conditional SUMIF function - same as above but returns a vector with a sum of numbers
	// SUMIF(Condition, Vector)
	else if(function == "SUMIF")
	{
		GetArg(argString, 1, &v1, &v2);        		
		if(!GetVectorFromArg(v2, var1)) return ThrowMissingArgError(function, "vector");

		if(instr(v1, " AND ") > -1 || instr(v1, " OR ") > -1)
		{
			return ThrowInvalidArgError(function, "SUMIF function must contain only one condition, without the 'AND' or 'OR' operators");
		}

		// Return the index of the field that is referenced		
		ret.field.resize(m_maxBars);
		// Get a temp variable with this condition
		v1 = "(" + v1 + ")"; // IMPORTANT!
		
		// The m_runningIfFunction flag tells the vector math processor in Eval and Token
		// that the field being worked on can be modified if it is a conditional operator.
		m_runningIfFunction = true;
		variable cond = Eval(v1.c_str());
		m_runningIfFunction = false;

		double sum = 0;
		bool condition = false;
		if(cond.field.size() == 0)
		{
			if(cond.value != 0)
				ret.value = var1.value;
			else
				ret.value = 0;
		}
		else
		{
			for(int n = 0; n != ret.field.size(); ++n)
			{
				condition = cond.field[n] != 0;
				if(condition)
					sum += var1.field[n];
			
				ret.field[n] = sum;
			}
		}
    }



	// Conditional LASTIF function - same as COUNTIF but returns the records since condition last evaluated to true
	else if(function == "LASTIF")
	{
		GetArg(argString, 1, &v1);
		
		if(instr(v1, " AND ") > -1 || instr(v1, " OR ") > -1)
		{
			return ThrowInvalidArgError(function, "LASTIF function must contain only one condition, without the 'AND' or 'OR' operators");
		}

		// Return the index of the field that is referenced		
		ret.field.resize(m_maxBars);
		// Get a temp variable with this condition
		v1 = "(" + v1 + ")"; // IMPORTANT!
		
		// The m_runningIfFunction flag tells the vector math processor in Eval and Token
		// that the field being worked on can be modified if it is a conditional operator.
		m_runningIfFunction = true;
		variable cond = Eval(v1.c_str());
		m_runningIfFunction = false;

		int count = 0;
		bool condition = false;
		if(cond.field.size() == 0)
		{
			if(cond.value != 0)
				ret.value = 1;
			else
				ret.value = 0;
		}
		else
		{
			for(int n = 0; n != ret.field.size(); ++n)
			{				
				condition = cond.field[n] != 0;
				if(condition)
					count = 0;
				else
					count++;
			
				ret.field[n] = count;
			}
		}
    }



	// LOOP is a very important primitive of the language. LOOP provides a level of control
	// structure in the form of for-loop simulation. Similar to the iota function of the APL language.
	// LOOP(Vector1, Vector2, Offset1, Offset2, Operator)
	else if(function == "LOOP")
	{
		GetArg(argString, 5, &v1, &v2, &v3, &v4, &v5);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "vector1");
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "vector2");
		
		double a = 0, b = 0, c = 0;
		if(!GetValueFromArg(function, v3, a)) return ThrowMissingArgError(function, "offset1");
		if(!GetValueFromArg(function, v4, b)) return ThrowMissingArgError(function, "offset2");		
		if(!GetValueFromArg(function, v5, c)) return ThrowMissingArgError(function, "operator");

		long offset1 = (long)a;
		long offset2 = (long)b;
		long mathop = (long)c;
		
		if(instr(v1, " AND ") > -1 || instr(v1, " OR ") > -1)
		{
			return ThrowInvalidArgError(function, "LOOP function may not contain a condition with 'AND' or 'OR' operators");
		}

		// Do not modify Vector1
		ret.field.resize(m_maxBars);
		for(int n = 0; n != ret.field.size(); ++n)
			ret.field[n] = var1.field[n];

		// Start at the largest offset		
		int start = max(offset1, offset2);
		
		if(mathop == 1) // ADD
		{
			for(int n = start; n != ret.field.size(); ++n)
				ret.field[n] = ret.field[n - offset1] + var2.field[n - offset2];				
		}
		else if(mathop == 2) // SUBTRACT
		{
			for(int n = start; n != ret.field.size(); ++n)
				ret.field[n] = ret.field[n - offset1] - var2.field[n - offset2];
		}
		else if(mathop == 3) // MULTIPLY
		{
			for(int n = start; n != ret.field.size(); ++n)
				ret.field[n] = ret.field[n - offset1] * var2.field[n - offset2];
		}
		else if(mathop == 4) // DIVIDE
		{
			for(int n = start; n != ret.field.size(); ++n)
				if(var1.field[n - offset1] != 0 && var2.field[n - offset2] != 0)
					ret.field[n] = ret.field[n - offset1] / var2.field[n - offset2];
		}		

    }



	// MAXOF - returns the maximum value of one of 8 arguments
	else if(function == "MAXOF")
	{
		GetArg(argString, 2, &v1, &v2, &v3, &v4, &v5, &v6, &v7, &v8);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "argument 1");		
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "argument 2");
		GetVectorFromArg(v3, var3);
		GetVectorFromArg(v4, var4);
		GetVectorFromArg(v5, var5);
		GetVectorFromArg(v6, var6);
		GetVectorFromArg(v7, var7);
		GetVectorFromArg(v8, var8);
		
		ret.field = var2.field;
		for(int n = 0; n != var1.field.size(); ++n)
			if(var1.field[n] > var2.field[n]) ret.field[n] = var1.field[n];
		
		if(v3 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var3.field[n] > ret.field[n]) ret.field[n] = var3.field[n];
		}
		if(v4 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var4.field[n] > ret.field[n]) ret.field[n] = var4.field[n];
		}
		if(v5 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var5.field[n] > ret.field[n]) ret.field[n] = var5.field[n];
		}
		if(v6 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var6.field[n] > ret.field[n]) ret.field[n] = var6.field[n];
		}
		if(v7 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var7.field[n] > ret.field[n]) ret.field[n] = var7.field[n];
		}
		if(v8 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var8.field[n] > ret.field[n]) ret.field[n] = var8.field[n];
		}

    }

	// MINOF - returns the minimum value of one of 8 arguments
	else if(function == "MINOF")
	{
		GetArg(argString, 2, &v1, &v2, &v3, &v4, &v5, &v6, &v7, &v8);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "argument 1");		
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "argument 2");
		GetVectorFromArg(v3, var3);
		GetVectorFromArg(v4, var4);
		GetVectorFromArg(v5, var5);
		GetVectorFromArg(v6, var6);
		GetVectorFromArg(v7, var7);
		GetVectorFromArg(v8, var8);
		
		ret.field = var2.field;
		for(int n = 0; n != var1.field.size(); ++n)
			if(var1.field[n] < var2.field[n]) ret.field[n] = var1.field[n];
		
		if(v3 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var3.field[n] < ret.field[n]) ret.field[n] = var3.field[n];
		}
		if(v4 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var4.field[n] < ret.field[n]) ret.field[n] = var4.field[n];
		}
		if(v5 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var5.field[n] < ret.field[n]) ret.field[n] = var5.field[n];
		}
		if(v6 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var6.field[n] < ret.field[n]) ret.field[n] = var6.field[n];
		}
		if(v7 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var7.field[n] < ret.field[n]) ret.field[n] = var7.field[n];
		}
		if(v8 != "")
		{
			for(int n = 0; n != var1.field.size(); ++n)
				if(var8.field[n] < ret.field[n]) ret.field[n] = var8.field[n];
		}

    }



	// CROSSOVER - returns a vector oscillating from TRUE to FALSE
	// The first vector specified will be checked for the crossover.	
	else if(function == "CROSSOVER")
	{
		GetArg(argString, 2, &v1, &v2);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "vector 1");		
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "vector 2");
		
		ret.field = var2.field;
		for(int n = 1; n != var1.field.size(); ++n)
		{
			if(var1.field[n - 1] <= var2.field[n - 1] && var1.field[n] > var2.field[n])
				ret.field[n] = 1; // Crossed
			else
				ret.field[n] = 0; // No cross
		}
	}


	// Reference function
	else if(function == "REF")
	{
		GetArg(argString, 2, &v1, &v2);
        v1 = GetArg(argString, 1, 2); // Vector
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
        
		if(v2 == "") return ThrowMissingArgError(function, "reference periods");			
		int index = abs(atol(v2.c_str()));
		if(index > m_maxBars - 2) // Reference out of range
			return ThrowError("REF() function reference periods outside of allowed range");
		
		// Return the index of the field that is referenced
		ret.field.resize(m_maxBars);
		for(int n = index; n != ret.field.size(); ++n)
		{
			ret.field[n] = var1.field[n - index];
		}
    }



	////////////////////////////////////////////////////////////////////////
	// General functions	
	////////////////////////////////////////////////////////////////////////


	// Returns constant value UP, DOWN, or SIDEWAYS	
	else if(function == "TREND")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") v2 = "10"; // Default to 10 periods
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

 		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		


		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;

		ret.field = tasdk.Trend( f, (int)periods ).data;

    }

		
	else if(function == "MAX" || function == "MIN")
	{
		GetArg(argString, 2, &v1, &v2);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
	
		double a = 0;
		if(!GetValueFromArg(function, v2, a)) return ThrowMissingArgError(function, "length");		
		int length = (int)a;
		
		bool bMax = function == "MAX";
		double max = 0, min = 0;
		ret.field.resize(m_maxBars);
		for(int n = length - 1; n < m_maxBars; ++n)
		{
			max = min = var1.field[n];
			for(int j = n - length + 1; j < n + 1; ++j)
			{				
				if(var1.field[j] > max) max = var1.field[j];
				if(var1.field[j] < min) min = var1.field[j];
			}
			if(bMax)
				ret.field[n] = max;
			else
				ret.field[n] = min;

			if(n == length - 1)
			{
				for(int j = 0; j < length; ++j)
				{
					if(bMax)
						ret.field[j] = max;
					else
						ret.field[j] = min;
				}
			}

		}
    }



	else if(function == "ALL_TIME_HIGH" || function == "ALL_TIME_LOW")
	{		
		int length = m_maxBars;
		bool bMax = function == "ALL_TIME_HIGH";
		if(bMax)
			var1 = GetVar("HIGH");
		else
			var1 = GetVar("LOW");

		double max = 0, min = 0;
		ret.field.resize(m_maxBars);
		for(int n = length - 1; n < m_maxBars; ++n)
		{
			max = min = var1.field[n];
			for(int j = n - length + 1; j < n + 1; ++j)
			{				
				if(var1.field[j] > max) max = var1.field[j];
				if(var1.field[j] < min) min = var1.field[j];
			}
			if(bMax)
				ret.field[n] = max;
			else
				ret.field[n] = min;

			if(n == length - 1)
			{
				for(int j = 0; j < length; ++j)
				{
					if(bMax)
						ret.field[j] = max;
					else
						ret.field[j] = min;
				}
			}

		}
    }


	

	else if(function == "SUM")
	{
		GetArg(argString, 2, &v1, &v2);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
	
		double a = 0;
		if(!GetValueFromArg(function, v2, a)) return ThrowMissingArgError(function, "length");		
		int length = (int)a;
		
		double sum = 0;
		ret.field.resize(m_maxBars);
		for(int n = length - 1; n < m_maxBars; ++n)
		{
			sum = 0;
			for(int j = n - length + 1; j < n + 1; ++j)			
				sum += var1.field[j];
			
			ret.field[n] = sum;
			
			if(n == length - 1)			
				for(int j = 0; j < length; ++j)				
					ret.field[j] = sum;			

		}
    }


	else if(function == "AVG" || function == "AVERAGE")
	{
		GetArg(argString, 2, &v1, &v2);

		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
	
		double a = 0;
		if(!GetValueFromArg(function, v2, a)) return ThrowMissingArgError(function, "length");		
		int length = (int)a;
		
		double avg = 0;
		ret.field.resize(m_maxBars);
		for(int n = length - 1; n < m_maxBars; ++n)
		{
			avg = 0;
			for(int j = n - length + 1; j < n + 1; ++j)			
				avg += var1.field[j];
			
			ret.field[n] = avg / a;
			
			if(n == length - 1)			
				for(int j = 0; j < length; ++j)				
					ret.field[j] = avg;			

		}
    }


	
	else if(function == "CSP" || function == "CANDLESTICKPATTERN" || function == "CANDLESTICKS")
	{
		ret.value = tasdk.CandleStickPattern(m_recordset);		
    }


	else if(function == "HML" || function == "HIGHMINUSLOW")
	{
		field f = tasdk.HighMinusLow(m_recordset);
		ret.field = f.data;
    }

	else if(function == "MP" || function == "MEDIANPRICE")
	{		
		field f = tasdk.MedianPrice(m_recordset);
		ret.field = f.data;
    }

	else if(function == "TP" || function == "TYPICALPRICE")
	{		
		field f = tasdk.TypicalPrice(m_recordset);
		ret.field = f.data;
    }

	else if(function == "WC" || function == "WEIGHTEDCLOSE")
	{		
		field f = tasdk.WeightedClose(m_recordset);
		ret.field = f.data;
    }

	else if(function == "PROC" || function == "PRICERATEOFCHANGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
	
		double periods = 0;
		if(!GetValueFromArg(function, v2, periods)) return ThrowMissingArgError(function, "periods");
		
		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;

		ret.field = tasdk.PriceROC( f, (int)periods ).data;	
    }

	else if(function == "VROC" || function == "VOLUMERATEOFCHANGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
	
		double periods = 0;
		if(!GetValueFromArg(function, v2, periods)) return ThrowMissingArgError(function, "periods");
		
		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;

		ret.field = tasdk.VolumeROC( f, (int)periods ).data;	
    }


	else if(function == "CA" || function == "CORRELATIONANALYSIS")
	{

		GetArg(argString, 1, &v1, &v2);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source 1");
		if(!GetVectorFromArg(v2, var2)) return ThrowMissingArgError(function, "source 2");
	
		field* f1 = tasdk.AddField("CA1", m_maxBars, m_recordset);
		field* f2 = tasdk.AddField("CA2", m_maxBars, m_recordset);

		f1->data = var1.field;
		f2->data = var2.field;

		ret.value = tasdk.CorrelationAnalysis(f1, f2);	
    }


	else if(function == "HHV" || function == "HIGHESTHIGHVALUE")
	{
		GetArg(argString, 1, &v1);
		double periods = 0;
		if(!GetValueFromArg(function, v1, periods)) return ThrowMissingArgError(function, "periods");
		
		field* f = tasdk.GetField("H", m_recordset);
		ret.field = tasdk.HHV( f, (int)periods ).data;
    }

	else if(function == "LLV" || function == "LOWESTLOWVALUE")
	{
		GetArg(argString, 1, &v1);
		double periods = 0;
		if(!GetValueFromArg(function, v1, periods)) return ThrowMissingArgError(function, "periods");
		
		field* f = tasdk.GetField("H", m_recordset);
		ret.field = tasdk.LLV( f, (int)periods ).data;
    }

	else if(function == "SDV" || function == "STANDARDDEVIATION")
	{
		GetArg(argString, 1, &v1, &v2, &v3, &v4);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
		double periods = 0;
		if(!GetValueFromArg(function, v2, periods)) return ThrowMissingArgError(function, "periods");
		double stdev = 0;
		if(!GetValueFromArg(function, v3, stdev)) return ThrowMissingArgError(function, "standard deviations");
		double matype = 1;
		if(!GetValueFromArg(function, v4, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");
		
		field* f = tasdk.AddField(function, m_maxBars, m_recordset);
		f->data = var1.field;
		ret.field = tasdk.StandardDeviation( f, (int)periods, (int)stdev, (int)matype ).data;
    }




	////////////////////////////////////////////////////////////////////////
	// Linear Regression functions	
	////////////////////////////////////////////////////////////////////////

	else if(function == "R2" || function == "RSQUARED" || function == "SLOPE" || 
			function == "INTERCEPT" || function == "FORECAST")
	{
		GetArg(argString, 1, &v1, &v2);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
		double periods = 0;
		if(!GetValueFromArg(function, v2, periods)) return ThrowMissingArgError(function, "periods");
				
		field* f = tasdk.AddField(function, m_maxBars, m_recordset);
		f->data = var1.field;

		recordset rs = tasdk.Regression( f, (int)periods );
		if(function == "R2" || function == "RSQUARED")
		{
			ret.field = tasdk.GetField("RSQUARED", rs)->data;
		}
		else if(function == "SLOPE")
		{
			ret.field = tasdk.GetField("SLOPE", rs)->data;
		}
		else if(function == "INTERCEPT")
		{
			ret.field = tasdk.GetField("INTERCEPT", rs)->data;
		}
		else if(function == "FORECAST")
		{
			ret.field = tasdk.GetField("FORECAST", rs)->data;
		}
		
    }

	else if(function == "TSF" || function == "TIMESERIESFORECAST")
	{
		GetArg(argString, 1, &v1, &v2);
		if(!GetVectorFromArg(v1, var1)) return ThrowMissingArgError(function, "source");
		double periods = 0;
		if(!GetValueFromArg(function, v2, periods)) return ThrowMissingArgError(function, "periods");
		
		field* f = tasdk.AddField(function, m_maxBars, m_recordset);
		f->data = var1.field;

		ret.field = tasdk.TimeSeriesForecast( f, (int)periods ).data;
		
    }



	////////////////////////////////////////////////////////////////////////
	// Moving Average functions
	////////////////////////////////////////////////////////////////////////
	
	// MEDIAMOVEL(TIPO,PERIODO,FONTE,DESLOCAR)
	else if (function == "MM" || function == "MEDIAMOVEL")
	{
		try{
			GetArg(argString, 4, &v1, &v2, &v3, &v4);
			if (v2 == "") return	ThrowMissingArgError(function, "periods");
			//Translate source:
			if (v3 == "ABR") v3 = "OPEN";
			else if (v3 == "FEC") v3 = "CLOSE";
			else if (v3 == "MAX") v3 = "HIGH";
			else if (v3 == "MIN") v3 = "LOW";
			if (!GetVectorFromArg(v3, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE
			if (v4 == "")v4 = "0";

			periods = atol(v2.c_str());
			shift = atol(v4.c_str());
			maType = atol(v1.c_str());
			if (periods < 2 /*|| periods > m_recordset.size() / 2*/){
				return ThrowInvalidArgError(function, "periods");
			}
			//string msg = "MediMovel("+v1+","+v2+","+v3+","+v4+")";
			//MessageBox(NULL, msg.c_str(), "Error", MB_ICONWARNING);

			field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
			f->data = var1.field;
			field rf = tasdk.GenericMovingAverage(f, periods, shift, maType, 1.0, "");
			ret.field = rf.data;
		}
		catch (exception  ex) {
			return ThrowError(ex.what());}
	}

	else if(function == "SMA" || function == "SIMPLEMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

 		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.SimpleMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "EMA" || function == "EXPONENTIALMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.ExponentialMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "TSMA" || function == "TIMESERIESMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.TimeSeriesMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "VMA" || function == "VARIABLEMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.VariableMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "TMA" || function == "TRIANGULARMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.TriangularMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "WMA" || function == "WEIGHTEDMOVINGAVERAGE")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.WeightedMovingAverage(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "WWS" || function == "WELLESWILDERSMOOTHING")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.WellesWilderSmoothing(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "VIDYA")
	{
		GetArg(argString, 1, &v1, &v2, &v3);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		double r2Scale = 0.65;
		GetValueFromArg(function, v3, r2Scale, false); // Optional

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.VIDYA(f, periods, r2Scale, "");
		ret.field = rf.data;
    }




	////////////////////////////////////////////////////////////////////////
	// Band functions
	////////////////////////////////////////////////////////////////////////

	else if(function == "BBT" || function == "BOLLINGERBANDSTOP" ||
			function == "BBM" || function == "BOLLINGERBANDSMIDDLE" ||
			function == "BBB" || function == "BOLLINGERBANDSBOTTOM")
	{
		GetArg(argString, 1, &v1, &v2, &v3, &v4);
        
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE
        if(var1.field.size() == 0) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");

		double stdev = 0;
		if(!GetValueFromArg(function, v3, stdev, false)) return ThrowMissingArgError(function, "standard deviations");

		double matype = 1;
		if(!GetValueFromArg(function, v4, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		recordset rs = tasdk.BollingerBands(f, periods, (int)stdev, (int)matype);

		if(function == "BBT" || function == "BOLLINGERBANDSTOP")		
			ret.field = tasdk.GetField("BB_TOP", rs)->data;
		else if(function == "BBM" || function == "BOLLINGERBANDSMIDDLE")
			ret.field = tasdk.GetField("BB_MEDIAN", rs)->data;
		else if(function == "BBB" || function == "BOLLINGERBANDSBOTTOM")
			ret.field = tasdk.GetField("BB_BOTTOM", rs)->data;		
		
    }


	else if(function == "KCT" || function == "KELTNERCHANNELTOP" ||
			function == "KCM" || function == "KELTNERCHANNELMEDIAN" ||
			function == "KCB" || function == "KELTNERCHANNELBOTTOM")
	{
		GetArg(argString, 1, &v1, &v2, &v3);
        
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");

		double matype = 1;
		if(!GetValueFromArg(function, v2, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");

		double multiplier = 0;
		if(!GetValueFromArg(function, v3, multiplier, false)) return ThrowMissingArgError(function, "multiplier");
		
		recordset rs = tasdk.KeltnerChannel(m_recordset, periods, (int)matype, multiplier);

		if(function == "KCT" || function == "KELTNERCHANNELTOP")		
			ret.field = tasdk.GetField("KC_TOP", rs)->data;
		else if(function == "KCM" || function == "KELTNERCHANNELMEDIAN")
			ret.field = tasdk.GetField("KC_MEDIAN", rs)->data;
		else if(function == "KCB" || function == "KELTNERCHANNELBOTTOM")
			ret.field = tasdk.GetField("KC_BOTTOM", rs)->data;		
		
    }



	else if(function == "MAET" || function == "MOVINGAVERAGEENVELOPETOP" ||			
			function == "MAEB" || function == "MOVINGAVERAGEENVELOPEBOTTOM")
	{
		GetArg(argString, 1, &v1, &v2, &v3);
				
        var1 = GetVar("CLOSE");

		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");

		double matype = 1;
		if(!GetValueFromArg(function, v2, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");
		
		double shift = 0;
		if(!GetValueFromArg(function, v3, shift, false)) return ThrowMissingArgError(function, "shift");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		recordset rs = tasdk.MovingAverageEnvelope( f, periods, (int)matype, shift );

		if(function == "MAET" || function == "MOVINGAVERAGEENVELOPETOP")		
			ret.field = tasdk.GetField("MAE_TOP", rs)->data;		
		else if(function == "MAEB" || function == "MOVINGAVERAGEENVELOPEBOTTOM")
			ret.field = tasdk.GetField("MAE_BOTTOM", rs)->data;		
		
    }


	else if(function == "PNBT" || function == "PRIMENUMBERBANDSTOP" ||
			function == "PNBB" || function == "PRIMENUMBERBANDSBOTTOM")
	{
		recordset rs = tasdk.PrimeNumberBands( m_recordset );

		if(function == "PNBT" || function == "PRIMENUMBERBANDSTOP")
			ret.field = tasdk.GetField("PNB_TOP", rs)->data;		
		else if(function == "PNBB" || function == "PRIMENUMBERBANDSBOTTOM")
			ret.field = tasdk.GetField("PNB_BOTTOM", rs)->data;		
	
    }





	////////////////////////////////////////////////////////////////////////
	// Oscillator functions
	////////////////////////////////////////////////////////////////////////

	else if(function == "CMO" || function == "CHANDEMOMENTUMOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.ChandeMomentumOscillator(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "MO" || function == "MOMENTUMOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.MomentumOscillator(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "MO" || function == "MOMENTUMOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.MomentumOscillator(f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "TRIX")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.TRIX(f, periods, "");
		ret.field = rf.data;
    }

	// Ultimate oscillator intentionally left out - too processor intensive

	else if(function == "VHF" || function == "VERTICALHORIZONTALFILTER")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.VerticalHorizontalFilter( f, periods, "");
		ret.field = rf.data;
    }

	else if(function == "WPR" || function == "WILLIAMSPCTR")
	{
		GetArg(argString, 1, &v1);
		if(v1 == "") return	ThrowMissingArgError(function, "periods");        
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.WilliamsPctR( m_recordset, periods, "");
		ret.field = rf.data;
    }

	else if(function == "WAD" || function == "WILLIAMSACCUMULATIONDISTRIBUTION")
	{
		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.WilliamsAccumulationDistribution( m_recordset );
		ret.field = rf.data;
    }

	else if(function == "VO" || function == "VOLUMEOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2, &v3, &v4);

		double shortTerm = 0;
		if(!GetValueFromArg(function, v1, shortTerm)) return ThrowMissingArgError(function, "short term periods");
		
		double longTerm = 0;
		if(!GetValueFromArg(function, v2, longTerm)) return ThrowMissingArgError(function, "long term periods");

		double matype = 1;
		if(!GetValueFromArg(function, v3, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");
		
		double pointsOrPercent = 0;
		if(!GetValueFromArg(function, v4, pointsOrPercent)) return ThrowMissingArgError(function, "points or percent constant");

		field* f = tasdk.GetField("V", m_recordset);		
		field rf = tasdk.VolumeOscillator ( f, (int)shortTerm, (int)longTerm, (int)matype, (int)pointsOrPercent );
		ret.field = rf.data;

    }

	else if(function == "CV" || function == "CHAIKINVOLATILITY")
	{
		GetArg(argString, 1, &v1, &v2, &v3);

		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 1 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");	

		double roc = 0;
		if(!GetValueFromArg(function, v2, roc)) return ThrowMissingArgError(function, "rate of change");
		
		double matype = 1;
		if(!GetValueFromArg(function, v3, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");
		
		field rf = tasdk.ChaikinVolatility( m_recordset, (int)periods, (int)roc, (int)matype );
		ret.field = rf.data;

    }

	else if(function == "PO" || function == "PRICEOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2, &v3, & v4);

		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		double shortTerm = 0;
		if(!GetValueFromArg(function, v2, shortTerm)) return ThrowMissingArgError(function, "short term periods");
		
		double longTerm = 0;
		if(!GetValueFromArg(function, v3, longTerm)) return ThrowMissingArgError(function, "long term periods");

		double matype = 1;
		if(!GetValueFromArg(function, v4, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");
		
		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.PriceOscillator ( f, (int)shortTerm, (int)longTerm, (int)matype );
		ret.field = rf.data;

    }

	else if(function == "EOM" || function == "EASEOFMOVEMENT")
	{
		GetArg(argString, 1, &v1, &v2);

		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.EaseOfMovement( m_recordset, (int)periods, 1 );
		ret.field = rf.data;
    }


	else if(function == "DPO" || function == "DETRENDEDPRICEOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2, &v3);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		double matype = 1;
		if(!GetValueFromArg(function, v3, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.DetrendedPriceOscillator( f, (int)periods, (int)matype );
		ret.field = rf.data;
    }

	else if(function == "PNO" || function == "PRIMENUMBEROSCILLATOR")
	{
		GetArg(argString, 1, &v1);
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE
		
		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.PrimeNumberOscillator( f );
		ret.field = rf.data;
    }

	else if(function == "TR" || function == "TRUERANGE")
	{
		field rf = tasdk.TrueRange( m_recordset );
		ret.field = rf.data;
    }

	else if(function == "ATR" || function == "AVERAGETRUERANGE")
	{
		GetArg(argString, 1, &v1, &v2 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());

		double matype = 1;		
		if(!GetValueFromArg(function, v2, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");

		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");	

		field rf = tasdk.AverageTrueRange( m_recordset, periods, (int)matype );
		ret.field = rf.data;
    }

	else if(function == "FCO" || function == "FRACTALCHAOSOSCILLATOR")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		
		field rf = tasdk.FractalChaosOscillator ( m_recordset, (int)periods );
		ret.field = rf.data;
    }

	else if(function == "RBO" || function == "RAINBOWOSCILLATOR")
	{
		GetArg(argString, 1, &v1, &v2, &v3);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

		double levels = 0;
		if(!GetValueFromArg(function, v2, levels)) return ThrowMissingArgError(function, "levels");

		double matype = 1;
		if(!GetValueFromArg(function, v3, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		field rf = tasdk.RainbowOscillator( f, (int)levels, (int)matype );
		ret.field = rf.data;
    }

	else if(function == "PSAR" || function == "PARABOLICSAR")
	{
		GetArg(argString, 1, &v2, &v1);

		double minAf = 0;
		GetValueFromArg(function, v1, minAf); // Default to 0.02
		if(minAf <= 0) minAf = 0.02;

		double maxAf = 0;
		GetValueFromArg(function, v2, maxAf); // Default to 0.2
		if(maxAf <= 0) maxAf = 0.2;
		if(maxAf < minAf) maxAf = minAf + 0.1;

		field* fH = tasdk.GetField("H", m_recordset);
		field* fL = tasdk.GetField("L", m_recordset);
		field rf = tasdk.ParabolicSAR( fH, fL, minAf, maxAf );
		ret.field = rf.data;

    }


	else if(function == "MACD" || function == "MACDSIGNAL" || function == "MACDS")
	{
		GetArg(argString, 1, &v1, &v2, &v3, &v4);

		double shortCycle = 0;
		if(!GetValueFromArg(function, v1, shortCycle)) return ThrowMissingArgError(function, "short cycle periods");
		
		double longCycle = 0;
		if(!GetValueFromArg(function, v2, longCycle)) return ThrowMissingArgError(function, "long cycle periods");

		double signalPeriods = 3;
		double matype = 1;
 	
		if(!GetValueFromArg(function, v3, signalPeriods)) 
			return ThrowMissingArgError(function, "signal periods");
			
		if(!GetValueFromArg(function, v4, matype, false, true)) 
			return ThrowMissingArgError(function, "moving average type");		
	
		recordset rs = tasdk.MACD( m_recordset, (int)shortCycle, 
			(int)longCycle, (int)signalPeriods, (int)matype, "MACD" );
		
		if(function == "MACD")
			ret.field = tasdk.GetField("MACD", rs)->data;
		else
			ret.field = tasdk.GetField("MACD SIGNAL", rs)->data;
		
    }

	// Directional Movement System	
	else if(function == "ADX" || function == "ADXR" || 
			function == "DIP" || function == "DIN" ||
			function == "TRSUM" || function == "DX")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		
		recordset rs = tasdk.DirectionalMovementSystem( m_recordset, (int)periods );
		
		if(function == "ADX")
			ret.field = tasdk.GetField("ADX", rs)->data;
		else if(function == "ADXR")
			ret.field = tasdk.GetField("ADXR", rs)->data;
		else if(function == "DX")
			ret.field = tasdk.GetField("DX", rs)->data;
		else if(function == "TRSUM")
			ret.field = tasdk.GetField("TRSUM", rs)->data;
		else if(function == "DIP")
			ret.field = tasdk.GetField("DI+", rs)->data;
		else if(function == "DIN")
			ret.field = tasdk.GetField("DI-", rs)->data;
	
    }


	else if(function == "AROONUP" || function == "AROONDOWN")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		
		recordset rs = tasdk.Aroon( m_recordset, (int)periods );
		
		if(function == "AROONUP")
			ret.field = tasdk.GetField("AROON UP", rs)->data;
		else if(function == "AROONDOWN")
			ret.field = tasdk.GetField("AROON DOWN", rs)->data;
    }


	else if(function == "SOPD" || function == "STOCHASTICOSCILLATORPCTD" ||
		    function == "SOPK" || function == "STOCHASTICOSCILLATORPCTK")
	{
		GetArg( argString, 1, &v1, &v2, &v3, &v4 );
		if(v1 == "") return	ThrowMissingArgError(function, "%k periods");
		if(v2 == "") return	ThrowMissingArgError(function, "%k slowing periods");
		if(v3 == "") return	ThrowMissingArgError(function, "%d periods");

		double pctKperiods = atol(v1.c_str());
		if(pctKperiods < 2 || pctKperiods > m_maxBars)
			return ThrowInvalidArgError(function, "%k periods");

		double pctKslowingperiods = atol(v2.c_str());
		if(pctKslowingperiods < 2 || pctKslowingperiods > m_maxBars)
			return ThrowInvalidArgError(function, "%k slowing periods");

		double pctDperiods = atol(v3.c_str());
		if(pctDperiods < 2 || pctDperiods > m_maxBars)
			return ThrowInvalidArgError(function, "%d periods");

		double matype = 1;
		if(!GetValueFromArg(function, v4, matype, false, true)) 
				return ThrowMissingArgError(function, "moving average type");	

		recordset rs = tasdk.StochasticOscillator( m_recordset, (int)pctKperiods, 
			(int)pctKslowingperiods, (int)pctDperiods, (int)matype );
		
		if(function == "SOPK" || function == "STOCHASTICOSCILLATORPCTK")
			ret.field = tasdk.GetField("K", rs)->data;
		else
			ret.field = tasdk.GetField("D", rs)->data;

	}



	////////////////////////////////////////////////////////////////////////
	// Index functions
	////////////////////////////////////////////////////////////////////////


	else if(function == "CMF" || function == "CHAIKINMONEYFLOW")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");
		
		ret.field = tasdk.ChaikinMoneyFlow ( m_recordset, (int)periods ).data;
		
    }

	else if(function == "MFI" || function == "MONEYFLOWINDEX")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");
		
		ret.field = tasdk.MoneyFlowIndex ( m_recordset, (int)periods ).data;
		
    }

	else if(function == "MI" || function == "MASSINDEX")
	{
		GetArg(argString, 1, &v1 );
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");
		
		ret.field = tasdk.MassIndex ( m_recordset, (int)periods ).data;
		
    }

	else if(function == "HVI" || function == "HISTORICALVOLATILITYINDEX" ||
			function == "HISTORICALVOLATILITY" || function == "HISTORICVOLATILITY" ||
			function == "HISTORICVOLATILITYINDEX")
	{
		GetArg(argString, 2, &v1, &v2, &v3, &v4);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
		if(v3 == "") return	ThrowMissingArgError(function, "bar history");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE
		
 		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");

 		int barHistory = atol(v3.c_str());
		if(barHistory < 2 || barHistory > m_maxBars)
			return ThrowInvalidArgError(function, "bar history");

		double stdev = 0;
		if(!GetValueFromArg(function, v4, stdev)) return ThrowMissingArgError(function, "standard deviations");

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		ret.field = tasdk.HistoricalVolatility( f, periods, barHistory, (int) stdev ).data ;	
    }

	else if(function == "RSI" || function == "RELATIVESTRENGTHINDEX")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v2 == "") return	ThrowMissingArgError(function, "periods");
        if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE

 		periods = atol(v2.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;
		ret.field = tasdk.RelativeStrengthIndex( f, periods ).data ;		
    }


	else if(function == "CRSI" || function == "COMPARATIVERELATIVESTRENGTHINDEX")
	{
		GetArg(argString, 1, &v1, &v2);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE
		if(!GetVectorFromArg(v2, var2)) var2 = GetVar("OPEN"); // Default to OPEN

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;
		field* f2 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f2->data = var2.field;

		ret.field = tasdk.ComparativeRelativeStrengthIndex( f1, f2 ).data;

    }

	else if(function == "PVT" || function == "PRICEVOLUMETREND")
	{
		GetArg(argString, 1, &v1);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;

		field* f2 = tasdk.GetField("V", m_recordset);

		ret.field = tasdk.PriceVolumeTrend( f1, f2 ).data;

    }


	else if(function == "PVI" || function == "POSITIVEVOLUMEINDEX")
	{
		GetArg(argString, 1, &v1);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;

		field* f2 = tasdk.GetField("V", m_recordset);

		ret.field = tasdk.PositiveVolumeIndex( f1, f2 ).data;

    }

	else if(function == "NVI" || function == "NEGATIVEVOLUMEINDEX")
	{
		GetArg(argString, 1, &v1);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;

		field* f2 = tasdk.GetField("V", m_recordset);

		ret.field = tasdk.NegativeVolumeIndex( f1, f2 ).data;

    }

	else if(function == "PFI" || function == "PERFORMANCEINDEX")
	{
		GetArg(argString, 1, &v1);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		field* f = tasdk.AddField(varName, m_maxBars, m_recordset);
		f->data = var1.field;		

		ret.field = tasdk.PerformanceIndex( f ).data;

    }

	else if(function == "OBV" || function == "ONBALANCEVOLUME")
	{
		GetArg(argString, 1, &v1);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;

		field* f2 = tasdk.GetField("V", m_recordset);

		ret.field = tasdk.OnBalanceVolume( f1, f2 ).data;

    }

	// MinTickVal is the minimum move, not the value per tick
	else if(function == "TVI" || function == "TRADEVOLUMEINDEX")
	{
		GetArg(argString, 1, &v1, &v2);
		
		if(!GetVectorFromArg(v1, var1)) var1 = GetVar("CLOSE"); // Default to CLOSE		

		double minTickVal = 1;
		if(!GetValueFromArg(function, v2, minTickVal, false)) 
				return ThrowMissingArgError(function, "minimum tick value");

		field* f1 = tasdk.AddField(varName, m_maxBars, m_recordset);
		f1->data = var1.field;

		field* f2 = tasdk.GetField("V", m_recordset);

		ret.field = tasdk.TradeVolumeIndex ( f1, f2, minTickVal).data;

    }

	else if(function == "CCI" || function == "COMMODITYCHANNELINDEX")
	{
		GetArg(argString, 1, &v1, &v2);
		if(v1 == "") return	ThrowMissingArgError(function, "periods");
        
 		periods = atol(v1.c_str());
		if(periods < 2 || periods > m_maxBars)
			return ThrowInvalidArgError(function, "periods");
		
		double matype = 1;
		if(!GetValueFromArg(function, v2, matype, false, true)) 
				return ThrowMissingArgError(function, "moving average type");

		ret.field = tasdk.CommodityChannelIndex ( m_recordset, (int)periods, (int)matype ).data ;		
    }

	else if(function == "SI" || function == "SWINGINDEX")
	{
		GetArg(argString, 1, &v1);
		
		double limitMoveValue = 1;
		if(!GetValueFromArg(function, v1, limitMoveValue, false)) 
				return ThrowMissingArgError(function, "limit move value");

		ret.field = tasdk.SwingIndex ( m_recordset, limitMoveValue ).data ;		
    }

	else if(function == "ASI" || function == "ACCUMULATIVESWINGINDEX")
	{
		GetArg(argString, 1, &v1);
		
		double limitMoveValue = 1;
		if(!GetValueFromArg(function, v1, limitMoveValue, false)) 
				return ThrowMissingArgError(function, "limit move value");

		ret.field = tasdk.AccumulativeSwingIndex ( m_recordset, limitMoveValue ).data ;		
    }

	
	else if(function == "SMIK" || function == "SMID")
	{

		GetArg(argString, 1, &v1, &v2, &v3, &v4, &v5, &v6 );
		if(v1 == "") return	ThrowMissingArgError(function, "%k periods");
		if(v2 == "") return	ThrowMissingArgError(function, "%k smooth");
		if(v3 == "") return	ThrowMissingArgError(function, "%k double periods");
		if(v4 == "") return	ThrowMissingArgError(function, "%d periods");

		double pctKperiods = atol(v1.c_str());
		if(pctKperiods < 2 || pctKperiods > m_maxBars)
			return ThrowInvalidArgError(function, "%k periods");

		double pctKsmooth = atol(v2.c_str());
		if(pctKsmooth < 2 || pctKsmooth > m_maxBars)
			return ThrowInvalidArgError(function, "%k smooth");

		double pctKDblsmooth = atol(v2.c_str());
		if(pctKDblsmooth < 2 || pctKDblsmooth > m_maxBars)
			return ThrowInvalidArgError(function, "%k double smooth");

		double pctDeriods = atol(v4.c_str());
		if(pctDeriods < 2 || pctDeriods > m_maxBars)
			return ThrowInvalidArgError(function, "%d periods");

		double matype = 1;
		if(!GetValueFromArg(function, v5, matype, false, true)) 
				return ThrowMissingArgError(function, "moving average type");

		double pctDmatype = 1;
		if(!GetValueFromArg(function, v6, pctDmatype, false, true)) 
				return ThrowMissingArgError(function, "%d moving average type");

		recordset rs = tasdk.StochasticMomentumIndex ( m_recordset, (int)pctKperiods, 
			(int)pctKsmooth, (int)pctKDblsmooth, (int)pctDeriods, (int)matype, (int)pctDmatype );
		

		if(function == "SMIK")
			ret.field = tasdk.GetField("K", rs)->data;
		else
			ret.field = tasdk.GetField("D", rs)->data;

	}



	else
	{
		m_error = "Error: Undefined function '" + function + "'";
	}

	

	// Update last value, set the variable, and exit
	if(ret.field.size() > (size_t)(m_maxBars - 1))
		ret.value = ret.field[m_maxBars - 1];
	else
		ret.field.resize(m_maxBars);

    SetVar(ret.name, ret);
	return ret;

}

string Script::GetScriptHelp()
{

	if(m_scriptHelp == "") return "";

	string szHelp = m_scriptHelp;	
    TCHAR szResHelp[MAX_STRING];

	makeupper(szHelp);

	HMODULE hWnd = GetModuleHandle("TRADESCRIPT.DLL");

	string help;
	if(szHelp == "IF")
		LoadString(hWnd, IDS_IF, szResHelp, MAX_STRING);
	else if(szHelp == "LOOP")
		LoadString(hWnd, IDS_LOOP, szResHelp, MAX_STRING);
	else if(szHelp == "COUNTIF")
		LoadString(hWnd, IDS_COUNTIF, szResHelp, MAX_STRING);
	else if(szHelp == "LASTIF")
		LoadString(hWnd, IDS_LASTIF, szResHelp, MAX_STRING);
	else if(szHelp == "SUMIF")
		LoadString(hWnd, IDS_SUMIF, szResHelp, MAX_STRING);
	else if(szHelp == "SUM")
		LoadString(hWnd, IDS_SUM, szResHelp, MAX_STRING);
	else if(szHelp == "AVG")
		LoadString(hWnd, IDS_AVG, szResHelp, MAX_STRING);
	else if(szHelp == "AVERAGE")
		LoadString(hWnd, IDS_AVG, szResHelp, MAX_STRING);
	else if(szHelp == "MAX")
		LoadString(hWnd, IDS_MAX, szResHelp, MAX_STRING);
	else if(szHelp == "MIN")
		LoadString(hWnd, IDS_MIN, szResHelp, MAX_STRING);
	else if(szHelp == "MAXOF")
		LoadString(hWnd, IDS_MAXOF, szResHelp, MAX_STRING);
	else if(szHelp == "MINOF")
		LoadString(hWnd, IDS_MINOF, szResHelp, MAX_STRING);
	else if(szHelp == "REF")
		LoadString(hWnd, IDS_REF, szResHelp, MAX_STRING);
	else if(szHelp == "TREND")
		LoadString(hWnd, IDS_TREND, szResHelp, MAX_STRING);
	else if(szHelp == "CROSSOVER")
		LoadString(hWnd, IDS_CROSSOVER, szResHelp, MAX_STRING);
	else if(szHelp == "ABS")
		LoadString(hWnd, IDS_ABS, szResHelp, MAX_STRING);
	else if(szHelp == "SIN")
		LoadString(hWnd, IDS_SIN, szResHelp, MAX_STRING);
	else if(szHelp == "COS")
		LoadString(hWnd, IDS_COS, szResHelp, MAX_STRING);
	else if(szHelp == "TAN")
		LoadString(hWnd, IDS_TAN, szResHelp, MAX_STRING);
	else if(szHelp == "ATN")
		LoadString(hWnd, IDS_ATN, szResHelp, MAX_STRING);
	else if(szHelp == "EXP")
		LoadString(hWnd, IDS_EXP, szResHelp, MAX_STRING);
	else if(szHelp == "LOG")
		LoadString(hWnd, IDS_LOG, szResHelp, MAX_STRING);
	else if(szHelp == "LOG10")
		LoadString(hWnd, IDS_LOG10, szResHelp, MAX_STRING);
	else if(szHelp == "RND")
		LoadString(hWnd, IDS_RND, szResHelp, MAX_STRING);
	else if(szHelp == "MEDIAMOVEL")
		LoadString(hWnd, IDS_GENERIC_MOVING_AVERAGE, szResHelp, MAX_STRING);		
	else if(szHelp == "MM")
		LoadString(hWnd, IDS_GENERIC_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if (szHelp == "SIMPLEMOVINGAVERAGE")
		LoadString(hWnd, IDS_SIMPLE_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if (szHelp == "SMA")
		LoadString(hWnd, IDS_SIMPLE_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "EXPONENTIALMOVINGAVERAGE")
		LoadString(hWnd, IDS_EXPONENTIAL_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "EMA")
		LoadString(hWnd, IDS_EXPONENTIAL_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "TIMESERIESMOVINGAVERAGE")
		LoadString(hWnd, IDS_TIME_SERIES_MOVING_AVERAGE, szResHelp, MAX_STRING);		
	else if(szHelp == "TSMA")
		LoadString(hWnd, IDS_TIME_SERIES_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "VARIABLEMOVINGAVERAGE")
		LoadString(hWnd, IDS_VARIABLE_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "VMA")
		LoadString(hWnd, IDS_VARIABLE_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "TRIANGULARMOVINGAVERAGE")		
		LoadString(hWnd, IDS_TRIANGULAR_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "TMA")
		LoadString(hWnd, IDS_TRIANGULAR_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "WEIGHTEDMOVINGAVERAGE")		
		LoadString(hWnd, IDS_WEIGHTED_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "WMA")
		LoadString(hWnd, IDS_WEIGHTED_MOVING_AVERAGE, szResHelp, MAX_STRING);
	else if(szHelp == "WELLESWILDERSMOOTHING")
		LoadString(hWnd, IDS_WELLES_WILDER_SMOOTHING, szResHelp, MAX_STRING);		
	else if(szHelp == "WWS")
		LoadString(hWnd, IDS_WELLES_WILDER_SMOOTHING, szResHelp, MAX_STRING);
	else if(szHelp == "VIDYA")
		LoadString(hWnd, IDS_VIDYA, szResHelp, MAX_STRING);
	else if(szHelp == "RSQUARED")
		LoadString(hWnd, IDS_R2, szResHelp, MAX_STRING);
	else if(szHelp == "R2")
		LoadString(hWnd, IDS_R2, szResHelp, MAX_STRING);
	else if(szHelp == "SLOPE")
		LoadString(hWnd, IDS_SLOPE, szResHelp, MAX_STRING);
	else if(szHelp == "FORECAST")
		LoadString(hWnd, IDS_FORECAST, szResHelp, MAX_STRING);
	else if(szHelp == "INTERCEPT")
		LoadString(hWnd, IDS_INTERCEPT, szResHelp, MAX_STRING);
	else if(szHelp == "BOLLINGERBANDSTOP")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "BBT")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "BOLLINGERBANDSBOTTOM")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "BBB")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "BOLLINGERBANDSMIDDLE")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "BBM")
		LoadString(hWnd, IDS_BB, szResHelp, MAX_STRING);
	else if(szHelp == "MOVINGAVERAGEENVELOPETOP")
		LoadString(hWnd, IDS_MAE, szResHelp, MAX_STRING);
	else if(szHelp == "MOVINGAVERAGEENVELOPEBOTTOM")
		LoadString(hWnd, IDS_MAE, szResHelp, MAX_STRING);
	else if(szHelp == "MAET")
		LoadString(hWnd, IDS_MAE, szResHelp, MAX_STRING);
	else if(szHelp == "MAEB")
		LoadString(hWnd, IDS_MAE, szResHelp, MAX_STRING);
	else if(szHelp == "PRIMENUMBERBANDSTOP")
		LoadString(hWnd, IDS_PRNB, szResHelp, MAX_STRING);	
	else if(szHelp == "PRIMENUMBERBANDSBOTTOM")
		LoadString(hWnd, IDS_PRNB, szResHelp, MAX_STRING);
	else if(szHelp == "PNBT")
		LoadString(hWnd, IDS_PRNB, szResHelp, MAX_STRING);
	else if(szHelp == "PNBB")
		LoadString(hWnd, IDS_PRNB, szResHelp, MAX_STRING);

	if(szHelp == "MOMENTUMOSCILLATOR")
		LoadString(hWnd, IDS_MOMOS, szResHelp, MAX_STRING);
	else if(szHelp == "MO")
		LoadString(hWnd, IDS_MOMOS, szResHelp, MAX_STRING);
	else if(szHelp == "CHANDEMOMENTUMOSCILLATOR")
		LoadString(hWnd, IDS_CMOS, szResHelp, MAX_STRING);
	else if(szHelp == "CMO")
		LoadString(hWnd, IDS_CMOS, szResHelp, MAX_STRING);
	else if(szHelp == "VOLUMEOSCILLATOR")
		LoadString(hWnd, IDS_VOS, szResHelp, MAX_STRING);
	else if(szHelp == "VO")
		LoadString(hWnd, IDS_VOS, szResHelp, MAX_STRING);
	else if(szHelp == "PRICEOSCILLATOR")
		LoadString(hWnd, IDS_POS, szResHelp, MAX_STRING);
	else if(szHelp == "PO")
		LoadString(hWnd, IDS_POS, szResHelp, MAX_STRING);
	else if(szHelp == "DETRENDEDPRICEOSCILLATOR")
		LoadString(hWnd, IDS_DPO, szResHelp, MAX_STRING);
	else if(szHelp == "DPO")
		LoadString(hWnd, IDS_DPO, szResHelp, MAX_STRING);
	else if(szHelp == "PRIMENUMBEROSCILLATOR")
		LoadString(hWnd, IDS_PNO, szResHelp, MAX_STRING);
	else if(szHelp == "PNO")
		LoadString(hWnd, IDS_PNO, szResHelp, MAX_STRING);
	else if(szHelp == "FRACTALCHAOSOSCILLATOR")
		LoadString(hWnd, IDS_FCO, szResHelp, MAX_STRING);
	else if(szHelp == "FCO")
		LoadString(hWnd, IDS_FCO, szResHelp, MAX_STRING);
	else if(szHelp == "RAINBOWOSCILLATOR")
		LoadString(hWnd, IDS_RBO, szResHelp, MAX_STRING);
	else if(szHelp == "RBO")
		LoadString(hWnd, IDS_RBO, szResHelp, MAX_STRING);
	else if(szHelp == "TRIX")
		LoadString(hWnd, IDS_TRIX, szResHelp, MAX_STRING);
	else if(szHelp == "VERTICALHORIZONTALFILTER")
		LoadString(hWnd, IDS_VHF, szResHelp, MAX_STRING);
	else if(szHelp == "VHF")
		LoadString(hWnd, IDS_VHF, szResHelp, MAX_STRING);
	else if(szHelp == "EASEOFMOVEMENT")
		LoadString(hWnd, IDS_EOM, szResHelp, MAX_STRING);
	else if(szHelp == "EOM")
		LoadString(hWnd, IDS_EOM, szResHelp, MAX_STRING);
	else if(szHelp == "ADX")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "ADXR")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "DIP")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "DIN")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "TRSUM")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "DX")
		LoadString(hWnd, IDS_DMS, szResHelp, MAX_STRING);
	else if(szHelp == "TRUERANGE")
		LoadString(hWnd, IDS_TR, szResHelp, MAX_STRING);
	else if(szHelp == "TR")
		LoadString(hWnd, IDS_TR, szResHelp, MAX_STRING);
	else if(szHelp == "WILLIAMSPCTR")
		LoadString(hWnd, IDS_WPR, szResHelp, MAX_STRING);
	else if(szHelp == "WPR")
		LoadString(hWnd, IDS_WPR, szResHelp, MAX_STRING);
	else if(szHelp == "WILLIAMSACCUMULATIONDISTRIBUTION")
		LoadString(hWnd, IDS_WAD, szResHelp, MAX_STRING);
	else if(szHelp == "WAD")
		LoadString(hWnd, IDS_WAD, szResHelp, MAX_STRING);
	else if(szHelp == "CHAIKINVOLATILITY")
		LoadString(hWnd, IDS_CV, szResHelp, MAX_STRING);
	else if(szHelp == "CV")
		LoadString(hWnd, IDS_CV, szResHelp, MAX_STRING);
	else if(szHelp == "AROONUP")
		LoadString(hWnd, IDS_AROON, szResHelp, MAX_STRING);
	else if(szHelp == "AROONDOWN")
		LoadString(hWnd, IDS_AROON, szResHelp, MAX_STRING);
	else if(szHelp == "MACD")
		LoadString(hWnd, IDS_MACD, szResHelp, MAX_STRING);
	else if(szHelp == "MACDSIGNAL")
		LoadString(hWnd, IDS_MACD, szResHelp, MAX_STRING);
	else if(szHelp == "HIGHMINUSLOW")
		LoadString(hWnd, IDS_HML, szResHelp, MAX_STRING);
	else if(szHelp == "HML")
		LoadString(hWnd, IDS_HML, szResHelp, MAX_STRING);
	else if(szHelp == "SOPK")
		LoadString(hWnd, IDS_SO, szResHelp, MAX_STRING);
	else if(szHelp == "SOPD")
		LoadString(hWnd, IDS_SO, szResHelp, MAX_STRING);
	else if(szHelp == "RELATIVESTRENGTHINDEX")
		LoadString(hWnd, IDS_RSI, szResHelp, MAX_STRING);
	else if(szHelp == "RSI")
		LoadString(hWnd, IDS_RSI, szResHelp, MAX_STRING);
	else if(szHelp == "MASSINDEX")
		LoadString(hWnd, IDS_MI, szResHelp, MAX_STRING);
	else if(szHelp == "MI")
		LoadString(hWnd, IDS_MI, szResHelp, MAX_STRING);
	else if(szHelp == "HISTORICALVOLATILITYINDEX")
		LoadString(hWnd, IDS_HVI, szResHelp, MAX_STRING);
	else if(szHelp == "HVI")
		LoadString(hWnd, IDS_HVI, szResHelp, MAX_STRING);
	else if(szHelp == "MONEYFLOWINDEX")
		LoadString(hWnd, IDS_MFI, szResHelp, MAX_STRING);
	else if(szHelp == "MFI")
		LoadString(hWnd, IDS_MFI, szResHelp, MAX_STRING);
	else if(szHelp == "CHAIKINMONEYFLOW")
		LoadString(hWnd, IDS_CMF, szResHelp, MAX_STRING);
	else if(szHelp == "CMF")
		LoadString(hWnd, IDS_CMF, szResHelp, MAX_STRING);
	else if(szHelp == "COMPARATIVERELATIVESTRENGTH")
		LoadString(hWnd, IDS_CRSI, szResHelp, MAX_STRING);
	else if(szHelp == "CRSI")
		LoadString(hWnd, IDS_CRSI, szResHelp, MAX_STRING);
	else if(szHelp == "PRICEVOLUMETREND")
		LoadString(hWnd, IDS_PVT, szResHelp, MAX_STRING);
	else if(szHelp == "PVT")
		LoadString(hWnd, IDS_PVT, szResHelp, MAX_STRING);
	else if(szHelp == "POSITIVEVOLUMEINDEX")
		LoadString(hWnd, IDS_PVI, szResHelp, MAX_STRING);
	else if(szHelp == "PVI")
		LoadString(hWnd, IDS_PVI, szResHelp, MAX_STRING);
	else if(szHelp == "NEGATIVEVOLUMEINDEX")
		LoadString(hWnd, IDS_NVI, szResHelp, MAX_STRING);
	else if(szHelp == "NVI")
		LoadString(hWnd, IDS_NVI, szResHelp, MAX_STRING);
	else if(szHelp == "ONBALANCEVOLUME")
		LoadString(hWnd, IDS_OBV, szResHelp, MAX_STRING);
	else if(szHelp == "OBV")
		LoadString(hWnd, IDS_OBV, szResHelp, MAX_STRING);
	else if(szHelp == "PERFORMANCEINDEX")
		LoadString(hWnd, IDS_PFI, szResHelp, MAX_STRING);
	else if(szHelp == "PFI")
		LoadString(hWnd, IDS_PFI, szResHelp, MAX_STRING);
	else if(szHelp == "TRADEVOLUMEINDEX")
		LoadString(hWnd, IDS_TVI, szResHelp, MAX_STRING);
	else if(szHelp == "TVI")
		LoadString(hWnd, IDS_TVI, szResHelp, MAX_STRING);
	else if(szHelp == "SWINGINDEX")
		LoadString(hWnd, IDS_SI, szResHelp, MAX_STRING);
	else if(szHelp == "SI")
		LoadString(hWnd, IDS_SI, szResHelp, MAX_STRING);
	else if(szHelp == "ACCUMULATIVESWINGINDEX")
		LoadString(hWnd, IDS_ASI, szResHelp, MAX_STRING);
	else if(szHelp == "ASI")
		LoadString(hWnd, IDS_ASI, szResHelp, MAX_STRING);
	else if(szHelp == "COMMODITYCHANNELINDEX")
		LoadString(hWnd, IDS_CCI, szResHelp, MAX_STRING);
	else if(szHelp == "CCI")
		LoadString(hWnd, IDS_CCI, szResHelp, MAX_STRING);
	else if(szHelp == "PARABOLICSAR")
		LoadString(hWnd, IDS_PSAR, szResHelp, MAX_STRING);
	else if(szHelp == "PSAR")
		LoadString(hWnd, IDS_PSAR, szResHelp, MAX_STRING);
	else if(szHelp == "SMIK")
		LoadString(hWnd, IDS_SMI, szResHelp, MAX_STRING);
	else if(szHelp == "SMID")
		LoadString(hWnd, IDS_SMI, szResHelp, MAX_STRING);
	else if(szHelp == "MEDIANPRICE")
		LoadString(hWnd, IDS_MP, szResHelp, MAX_STRING);
	else if(szHelp == "MP")
		LoadString(hWnd, IDS_MP, szResHelp, MAX_STRING);
	else if(szHelp == "TYPICALPRICE")
		LoadString(hWnd, IDS_TP, szResHelp, MAX_STRING);
	else if(szHelp == "TP")
		LoadString(hWnd, IDS_TP, szResHelp, MAX_STRING);
	else if(szHelp == "WEIGHTEDCLOSE")
		LoadString(hWnd, IDS_WC, szResHelp, MAX_STRING);
	else if(szHelp == "WC")
		LoadString(hWnd, IDS_WC, szResHelp, MAX_STRING);
	else if(szHelp == "PRICERATEOFCHANGE")
		LoadString(hWnd, IDS_PROC, szResHelp, MAX_STRING);
	else if(szHelp == "PROC")
		LoadString(hWnd, IDS_PROC, szResHelp, MAX_STRING);
	else if(szHelp == "VOLUMERATEOFCHANGE")
		LoadString(hWnd, IDS_VROC, szResHelp, MAX_STRING);
	else if(szHelp == "VROC")
		LoadString(hWnd, IDS_VROC, szResHelp, MAX_STRING);
	else if(szHelp == "HIGHESTHIGHVALUE")
		LoadString(hWnd, IDS_HHV, szResHelp, MAX_STRING);
	else if(szHelp == "HHV")
		LoadString(hWnd, IDS_HHV, szResHelp, MAX_STRING);
	else if(szHelp == "LOWESTLOWVALUE")
		LoadString(hWnd, IDS_LLV, szResHelp, MAX_STRING);
	else if(szHelp == "LLV")
		LoadString(hWnd, IDS_LLV, szResHelp, MAX_STRING);
	else if(szHelp == "STANDARDDEVIATIONS")
		LoadString(hWnd, IDS_SDV, szResHelp, MAX_STRING);
	else if(szHelp == "SDV")
		LoadString(hWnd, IDS_SDV, szResHelp, MAX_STRING);
	else if(szHelp == "CORRELATIONANALYSIS")
		LoadString(hWnd, IDS_CA, szResHelp, MAX_STRING);
	else if(szHelp == "CA")
		LoadString(hWnd, IDS_CA, szResHelp, MAX_STRING);
	else if(szHelp == "CANDLESTICKPATTERN")
		LoadString(hWnd, IDS_CSP, szResHelp, MAX_STRING);
	else if(szHelp == "CSP")
		LoadString(hWnd, IDS_CSP, szResHelp, MAX_STRING);
	else if(szHelp == "SECTOR")
		LoadString(hWnd, IDS_SECIND, szResHelp, MAX_STRING);
	else if(szHelp == "INDUSTRY")
		LoadString(hWnd, IDS_SECIND, szResHelp, MAX_STRING);
		
	help = szResHelp;

	return help;
}

// Started: 8/25/05
// Finished: 11/3/05
// Converted to TradeScript 6/22/06