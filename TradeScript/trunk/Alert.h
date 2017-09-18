// Alert.h : Declaration of the CAlert

#ifndef __ALERT_H_
#define __ALERT_H_

#include "resource.h"       // main symbols
#include "Script.h"
#include "TradeScriptCP.h"

class Script;

  #pragma data_seg("Alerts")
   static int m_alertCount = 0; 
  #pragma data_seg() 
  #pragma comment(linker, "/SECTION:Alerts,RWS") 


/////////////////////////////////////////////////////////////////////////////
// CAlert
class ATL_NO_VTABLE CAlert : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CAlert, &CLSID_Alert>,
	public ISupportErrorInfo,
	public IConnectionPointContainerImpl<CAlert>,
	public IDispatchImpl<IAlert, &IID_IAlert, &LIBID_TradeScriptLib, 2008>,
	public CProxy_IAlertEvents< CAlert >
{ // VERSION NUMBER MUST MATCH IDL FILE (2008)
public:
	CAlert()
	{			
		m_license = "";
		if(m_alertCount >= 1000)
		{
			m_exceeded = true;
			m_script = "EXCEEDED";
		}
		else
		{
			m_exceeded = false;
		}

		m_alertCount++;
	}

	~CAlert()
	{
		m_alertCount--;
	}

DECLARE_CLASSFACTORY2(CLicense)

DECLARE_REGISTRY_RESOURCEID(IDR_ALERT)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CAlert)
	COM_INTERFACE_ENTRY(IAlert)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY_IMPL(IConnectionPointContainer)
END_COM_MAP()

BEGIN_CONNECTION_POINT_MAP(CAlert)
	CONNECTION_POINT_ENTRY(DIID__IAlertEvents)
END_CONNECTION_POINT_MAP()


// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

vector<record> m_records;
string m_symbol;
string	m_script;
string m_scriptText; // Never modified
string m_alertName;
bool m_exceeded;
void RunScript();
bool CheckLicense();
string m_license;
string m_scriptHelp;

// IAlert
public:
	STDMETHOD(Evaluate)(/*[in]*/BSTR EvaluateScript, /*[out, retval]*/VARIANT_BOOL *pRet);
	STDMETHOD(get_ScriptHelp)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_License)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_License)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_AlertName)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_AlertName)(/*[in]*/ BSTR newVal);
	STDMETHOD(FromJulianDate)(/*[in]*/ double JDate, /*[out, retval]*/ BSTR *pRet);
	STDMETHOD(ToJulianDate)(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet);
	STDMETHOD(get_AlertScript)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_AlertScript)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_Symbol)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Symbol)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_RecordCount)(/*[out, retval]*/ long *pVal);
	STDMETHOD(ClearRecords)();
	STDMETHOD(GetRecordByJDate)(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(GetRecordByIndex)(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(EditRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet);
	STDMETHOD(AppendRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume);
	STDMETHOD(AppendHistoryRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume);
	STDMETHOD(GetJDate)(long Index, double *pRet);
};

#endif //__ALERT_H_
