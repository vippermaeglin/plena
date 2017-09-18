// Scanner.h : Declaration of the CScanner

#ifndef __SCANNER_H_
#define __SCANNER_H_

#include "resource.h"       // main symbols
#include "Script.h"
#include "TradeScriptCP.h"

class Script;

  #pragma data_seg("Scanner")
   static int m_scannerCount = 0; 
  #pragma data_seg() 
  #pragma comment(linker, "/SECTION:Scanner,RWS") 


/////////////////////////////////////////////////////////////////////////////
// CScanner
class ATL_NO_VTABLE CScanner : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CScanner, &CLSID_Scanner>,
	public ISupportErrorInfo,
	public IConnectionPointContainerImpl<CScanner>,
	public IDispatchImpl<IScanner, &IID_IScanner, &LIBID_TradeScriptLib, 2008>,
	public CProxy_IScannerEvents< CScanner >
{ // VERSION NUMBER MUST MATCH IDL FILE (2008)
public:
	CScanner()
	{			
		m_license = "";
		if(m_scannerCount >= 1000)
		{
			m_exceeded = true;
			m_script = "EXCEEDED";
		}
		else
		{
			m_exceeded = false;
		}

		m_scannerCount++;
	}

	~CScanner()
	{
		m_scannerCount--;
	}



vector<record> m_records;
string m_scriptText;
string m_symbol;
string	m_script;
bool m_exceeded;
void RunScript();
bool CheckLicense();
string m_license;
string m_scriptHelp;



vector<SymbolData> m_Symbols;


vector< map<string, int>::const_iterator > m_SymbolIndexIters;
map< string, int >::const_iterator itIndex;
map< string, int > m_SymbolIndexMap;



SymbolData* GetSymbolData(string Symbol);



DECLARE_REGISTRY_RESOURCEID(IDR_SCANNER)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CScanner)
	COM_INTERFACE_ENTRY(IScanner)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
END_COM_MAP()
BEGIN_CONNECTION_POINT_MAP(CScanner)
END_CONNECTION_POINT_MAP()


// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

// IScanner
public:
	STDMETHOD(Evaluate)(/*[in]*/BSTR EvaluateScript, /*[out, retval]*/VARIANT_BOOL *pRet);
	STDMETHOD(get_ScriptHelp)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_License)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_License)(/*[in]*/ BSTR newVal);	
	STDMETHOD(FromJulianDate)(/*[in]*/ double JDate, /*[out, retval]*/ BSTR *pRet);
	STDMETHOD(ToJulianDate)(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet);
	STDMETHOD(get_ScannerScript)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_ScannerScript)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_RecordCount)(/*[out, retval]*/ long *pVal);
	STDMETHOD(ClearRecords)();
	STDMETHOD(GetRecordByJDate)(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(GetRecordByIndex)(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(EditRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet);
	STDMETHOD(AppendRecord)(BSTR Symbol, double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume);
	STDMETHOD(GetJDate)(long Index, double *pRet);
};

#endif //__SCANNER_H_
