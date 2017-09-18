// Backtest.cpp : Implementation of CBacktest
#include "stdafx.h"
#include "TradeScript.h"
#include "Backtest.h"

/////////////////////////////////////////////////////////////////////////////
// CBacktest

STDMETHODIMP CBacktest::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* arr[] = 
	{
		&IID_IBacktest
	};
	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}


static __inline int instr(std::string &s, const std::string &to_find)
{	
	std::string::size_type pos;
	if((pos = s.find(to_find)) != string::npos)
	{
		return pos;
	}
	return -1;
}

STDMETHODIMP CBacktest::AppendRecord(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume)
{
	
	record rec;
	rec.jDateTime = JDate;
	rec.open = OpenPrice;
	rec.high = HighPrice;
	rec.low = LowPrice;
	rec.close = ClosePrice;
	rec.volume = Volume;
	m_records.push_back(rec);

	return S_OK;
}

STDMETHODIMP CBacktest::EditRecord(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet)
{	
	record rec;
	rec.jDateTime = JDate;
	rec.open = OpenPrice;
	rec.high = HighPrice;
	rec.low = LowPrice;
	rec.close = ClosePrice;
	rec.volume = Volume;

	*pRet = VARIANT_FALSE;
	for(int n = 0; n != m_records.size(); ++n)
	{
		if(m_records[n].jDateTime == JDate)
		{
			m_records[n] = rec;			
			*pRet = VARIANT_TRUE;
			break;
		}
	}	

	return S_OK;
}

STDMETHODIMP CBacktest::GetRecordByJDate(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double *ClosePrice, long *Volume, VARIANT_BOOL *pRet)
{

	for(int n = 0; n != m_records.size(); ++n)
	{
		if(m_records[n].jDateTime == JDate)
		{
			*OpenPrice = m_records[n].open;
			*HighPrice = m_records[n].high;
			*LowPrice = m_records[n].low;
			*ClosePrice = m_records[n].close;
			*Volume = m_records[n].volume;
			*pRet = VARIANT_TRUE;
			return S_OK;
		}			
	}

	*pRet = VARIANT_FALSE;
	return S_OK;
}


STDMETHODIMP CBacktest::GetRecordByIndex(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double *ClosePrice, long *Volume, VARIANT_BOOL *pRet)
{

	Index--;
	if(Index < 0 || Index > m_records.size() - 1)
	{
		*pRet = VARIANT_FALSE;
		return S_OK;
	}

	*OpenPrice = m_records[Index].open;
	*HighPrice = m_records[Index].high;
	*LowPrice = m_records[Index].low;
	*ClosePrice = m_records[Index].close;
	*Volume = m_records[Index].volume;	
	*JDate = m_records[Index].jDateTime;
	
	*pRet = VARIANT_TRUE;
	return S_OK;
}


STDMETHODIMP CBacktest::ClearRecords()
{
	m_records.clear();
	return S_OK;
}

STDMETHODIMP CBacktest::get_RecordCount(long *pVal)
{
	*pVal = m_records.size();

	return S_OK;
}

 
STDMETHODIMP CBacktest::ToJulianDate(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet)
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

STDMETHODIMP CBacktest::FromJulianDate(double JDate, BSTR *pRet)
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

	*pRet = CComBSTR(szDT.c_str()).Detach();

	return S_OK;
}

 
// Don't allow this program to run on Windows Server
bool CBacktest::CheckLicense()
{
#ifdef DEMO_MODE
	return true;
#endif
	return (m_license == "XRT93NQR79ABTW788XR48");
}


STDMETHODIMP CBacktest::Backtest(BSTR BuyScript, BSTR SellScript, BSTR ExitLongScript, BSTR ExitShortScript, double SlipPct, BSTR *pRet)
{
	USES_CONVERSION;
	string buyScript, sellScript, exitLongScript, exitShortScript;
	try{
		buyScript = W2A(BuyScript);
		sellScript = W2A(SellScript);
		exitLongScript = W2A(ExitLongScript);
		exitShortScript = W2A(ExitShortScript);
	}
	catch(...){}

	if(SlipPct < 0 || SlipPct > 0.5) SlipPct = 0.001;

	if(buyScript.length() < 5) return S_OK;

	if(!CheckLicense())
	{
		string err = "Error: License information not found";
		BSTR Description = A2BSTR(err.c_str());
		Fire_ScriptError(Description);		
		return S_OK;
	}

	m_oScript.m_records = m_records;
	string ret = m_oScript.BackTest(buyScript, sellScript, exitLongScript, exitShortScript, SlipPct);

	if(instr(ret, "Error:") > -1)
	{
		USES_CONVERSION;
		BSTR Description = A2BSTR(ret.c_str());
		Fire_ScriptError(Description);
		return S_OK;
	}

	*pRet = CComBSTR(ret.c_str()).Detach();

	return S_OK;
}

STDMETHODIMP CBacktest::get_License(BSTR *pVal)
{	
	*pVal = CComBSTR(m_license.c_str()).Detach();
	return S_OK;
}

STDMETHODIMP CBacktest::put_License(BSTR newVal)
{
	USES_CONVERSION;
	try{
		m_license = W2A(newVal);
	}
	catch(...){}

	return S_OK;
}

STDMETHODIMP CBacktest::get_ScriptHelp(BSTR *pVal)
{
	string help = m_oScript.GetScriptHelp();
	*pVal = CComBSTR(help.c_str()).Detach();
	return S_OK;
}
