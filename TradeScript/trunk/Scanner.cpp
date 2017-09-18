// Scanner.cpp : Implementation of CScanner
#include "stdafx.h"
#include "TradeScript.h"
#include "Scanner.h"

/////////////////////////////////////////////////////////////////////////////
// CScanner

STDMETHODIMP CScanner::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* arr[] = 
	{
		&IID_IScanner
	};
	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}


SymbolData* CScanner::GetSymbolData(string Symbol)
{
	SymbolData* ret = NULL;
	itIndex = m_SymbolIndexMap.find(Symbol);
	if ( itIndex != m_SymbolIndexMap.end() )
	{
		int index = m_SymbolIndexMap[(*itIndex).first];
		ret = &m_Symbols[index];
	}
	return ret;
}


STDMETHODIMP CScanner::AppendRecord(BSTR Symbol, double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume)
{
	// PATENT-PENDING! USE OF THIS CLASS AT THIS TIME IS FORBIDDEN!

	string symbol = "";
	USES_CONVERSION;
	try{
		symbol = W2A(Symbol);
	}
	catch(...){}
	if(symbol == "") return S_FALSE;

	record rec;
	rec.jDateTime = JDate;
	rec.open = OpenPrice;
	rec.high = HighPrice;
	rec.low = LowPrice;
	rec.close = ClosePrice;
	rec.volume = Volume;
	m_records.push_back(rec);


	// First, check to see if this symbol has been added to the m_Symbols vector already.
	SymbolData* pData = GetSymbolData(symbol);
	if(pData == NULL) // Add symbol
	{
		// Store symbol data
		SymbolData data;
		data.szSymbol = symbol;
		pData = &data;
		m_Symbols.push_back(data);

		// Save the vector index of this symbol in a map
		pair< map<string, int>::const_iterator, bool> p;				
		p = m_SymbolIndexMap.insert( make_pair( data.szSymbol, (int)m_Symbols.size() - 1 ) );
		if(p.second) itIndex = p.first;
		m_SymbolIndexIters.push_back(itIndex);
	}

	// Append the new record
	pData->data.push_back(rec);



/*

	// PATENT-PENDING! USE OF THIS CLASS AT THIS TIME IS FORBIDDEN!

	// DO NOT USE THIS CLASS, IT IS NOT DOCUMENTED, AND IT IS NOT SUPPORTED YET!

	// USE OF THIS CLASS MAY RESULT IN SEVERE CIVIL PROSECUTION!

	// FOR FUTURE USE ONLY!!!
	// CURRENTLY FOR SCANNING, USE THE ALERT OBJECT AS SHOWN IN THE M4 PROJECT.
	
	// AGAIN, DO NOT USE THIS CLASS!

TODO: This function is finished. Now test to ensure symbol data is saved/returned
	  per symbol and if everything looks good, continue with below.. re-write the EditRecord, GetRecord functions below.
	  Finally, create a RunScript function that is public to the client in the idl file.
	  RunScript will call the script.RunScript function with an extra parameter, to loop through m_Symbols and scan.
	  
	Also, add a function to see if the last price has changed. If no change, then skip over scanning that symbol.
	Actually, there should be a list of symbols sent to this function that the client has identified with changes,
	because changes occur between new bars being added. Only the client will know if the last price has changed.

*/

	return S_OK;
}












STDMETHODIMP CScanner::EditRecord(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet)
{	/*
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
		if(m_records[n].jDateTime >= JDate)
		{			
			m_records[n] = rec;			
			*pRet = VARIANT_TRUE;
			RunScript();
			break;
		}
	}
*/
	return S_OK;
}

STDMETHODIMP CScanner::GetRecordByJDate(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double *ClosePrice, long *Volume, VARIANT_BOOL *pRet)
{
/*
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
*/
	*pRet = VARIANT_FALSE;
	return S_OK;
}


STDMETHODIMP CScanner::GetRecordByIndex(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double *ClosePrice, long *Volume, VARIANT_BOOL *pRet)
{
	/*

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
	*/
	*pRet = VARIANT_TRUE;
	return S_OK;
}


STDMETHODIMP CScanner::ClearRecords()
{
//	m_records.clear();
	return S_OK;
}

STDMETHODIMP CScanner::get_RecordCount(long *pVal)
{
	//*pVal = m_records.size();

	return S_OK;
}






STDMETHODIMP CScanner::get_ScannerScript(BSTR *pVal)
{
	*pVal = CComBSTR(m_scriptText.c_str()).Detach();
	return S_OK;
}

STDMETHODIMP CScanner::put_ScannerScript(BSTR newVal)
{
	USES_CONVERSION;
	try{
		m_script = W2A(newVal);
	}
	catch(...){}
	m_scriptText = m_script;
	if(m_script.length() < 5) return S_OK;	

	RunScript();

	return S_OK;
}

STDMETHODIMP CScanner::ToJulianDate(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet)
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

STDMETHODIMP CScanner::FromJulianDate(double JDate, BSTR *pRet)
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



void CScanner::RunScript()
{


	/*
	m_script = m_scriptText;
	if(m_script.length() < 5) return;

	if(!CheckLicense())
	{
		BSTR Symbol = A2BSTR(m_symbol.c_str());
		string err = "Error: License information not found";
		BSTR Description = A2BSTR(err.c_str());
		Fire_ScriptError(Symbol, Description);
		return;
	}
	
	if(m_exceeded)
	{
		USES_CONVERSION;
		BSTR Symbol = A2BSTR(m_symbol.c_str());		
		string err = "Error: You have exceeded the maximum number of Scanners!";
		BSTR Description = A2BSTR(err.c_str());
		Fire_ScriptError(Symbol, Description);		
		return;
	}

	Script script;
	script.m_records = m_records;
	string ret = script.RunScript(m_script, false);
	m_scriptHelp = script.GetScriptHelp();

	if(ret != "Error: Not enough data available to run script")
	{
		if(ret != "" && ret != "Scanner")
		{
			USES_CONVERSION;
			BSTR Symbol = A2BSTR(m_symbol.c_str());			
			BSTR Description = A2BSTR(ret.c_str());
			Fire_ScriptError(Symbol, Description);
		}
		else if(ret == "Scanner")
		{
			USES_CONVERSION;
			BSTR Symbol = A2BSTR(m_symbol.c_str());			
			Fire_SymbolFound(Symbol);			
		}
	}
	*/

}


// Don't allow this program to run on Windows Server
bool CScanner::CheckLicense()
{
#ifdef DEMO_MODE
	return true;
#endif
	return (m_license == "XST93NQR79ABTW788XR48");	
}

STDMETHODIMP CScanner::get_License(BSTR *pVal)
{	
	*pVal = CComBSTR(m_license.c_str()).Detach();
	return S_OK;
}

STDMETHODIMP CScanner::put_License(BSTR newVal)
{
	USES_CONVERSION;
	try{
		m_license = W2A(newVal);
	}
	catch(...){}

	return S_OK;
}

STDMETHODIMP CScanner::get_ScriptHelp(BSTR *pVal)
{
	*pVal = CComBSTR(m_scriptHelp.c_str()).Detach();
	return S_OK;
}

STDMETHODIMP CScanner::GetJDate(long Index, double *pRet)
{
	Index--;
	if(Index < 0 || Index > m_records.size() - 1)
	{
		*pRet = 0;
		return S_OK;
	}

	*pRet = m_records[Index].jDateTime;
	
	return S_OK;
}

STDMETHODIMP CScanner::Evaluate(BSTR EvaluateScript, VARIANT_BOOL *pRet)
{

	/*
	USES_CONVERSION;
	try{
		m_script = W2A(EvaluateScript);
	}
	catch(...){}
	*pRet = FALSE;

	m_scriptText = m_script;
	if(m_script.length() < 5)
	{
		*pRet = FALSE;
		return S_OK;
	}

	if(!CheckLicense())
	{
		BSTR Symbol = A2BSTR(m_symbol.c_str());		
		string err = "Error: License information not found";
		BSTR Description = A2BSTR(err.c_str());
		Fire_ScriptError(Symbol, Description);
		*pRet = FALSE;
		return S_OK;
	}

	Script oScript;
	oScript.m_records = m_records;
	string ret = oScript.RunScript(m_script, false);
	m_scriptHelp = oScript.GetScriptHelp();

	if(ret != "Error: Not enough data available to run script")
	{
		if(ret != "" && ret != "Scanner")
		{
			USES_CONVERSION;
			BSTR Symbol = A2BSTR(m_symbol.c_str());			
			BSTR Description = A2BSTR(ret.c_str());
			Fire_ScriptError(Symbol, Description);
			*pRet = FALSE;
			return S_OK;
		}
		else if(ret == "Scanner")
		{
			USES_CONVERSION;
			BSTR Symbol = A2BSTR(m_symbol.c_str());			
			Fire_SymbolFound(Symbol);
			*pRet = TRUE;
			return S_OK;
		}
	}

  */

	*pRet = FALSE;
	return S_OK;
}
