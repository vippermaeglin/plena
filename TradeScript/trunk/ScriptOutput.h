// ScriptOutput.h : Declaration of the CScriptOutput

#ifndef __SCRIPTOUTPUT_H_
#define __SCRIPTOUTPUT_H_

#include "resource.h"       // main symbols
#include "Script.h"
#include "TradeScriptCP.h"

/////////////////////////////////////////////////////////////////////////////
// CScriptOutput
class ATL_NO_VTABLE CScriptOutput : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CScriptOutput, &CLSID_ScriptOutput>,
	public ISupportErrorInfo,
	public IConnectionPointContainerImpl<CScriptOutput>,
	public IDispatchImpl<IScriptOutput, &IID_IScriptOutput, &LIBID_TradeScriptLib, 2008>,
	public CProxy_IScriptOutputEvents< CScriptOutput >
{ // VERSION NUMBER MUST MATCH IDL FILE (2008)
public:
	CScriptOutput()
	{
		m_exceeded = false;
		m_license = "";
	}

DECLARE_CLASSFACTORY2(CLicense)

DECLARE_REGISTRY_RESOURCEID(IDR_SCRIPTOUTPUT)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CScriptOutput)
	COM_INTERFACE_ENTRY(IScriptOutput)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY_IMPL(IConnectionPointContainer)
END_COM_MAP()
BEGIN_CONNECTION_POINT_MAP(CScriptOutput)
CONNECTION_POINT_ENTRY(DIID__IScriptOutputEvents)
END_CONNECTION_POINT_MAP()


// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

vector<record> m_records;
string m_symbol;
string	m_script;
string m_BacktestName;
bool m_exceeded;
void RunScript();
bool CheckLicense();
Script m_oScript;
string m_license;
// IScriptOutput
public:
	STDMETHOD(get_ScriptHelp)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_License)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_License)(/*[in]*/ BSTR newVal);
	STDMETHOD(GetLicense)(BSTR *pRet);
	STDMETHOD(GetScriptOutput)(BSTR DefaultScript, /*[out, retval]*/ BSTR *pRet);
	STDMETHOD(FromJulianDate)(/*[in]*/ double JDate, /*[out, retval]*/ BSTR *pRet);
	STDMETHOD(ToJulianDate)(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet);
	STDMETHOD(get_RecordCount)(/*[out, retval]*/ long *pVal);
	STDMETHOD(ClearRecords)();
	STDMETHOD(GetRecordByJDate)(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(GetRecordByIndex)(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(EditRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet);
	STDMETHOD(AppendRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume);

};

#endif //__SCRIPTOUTPUT_H_
