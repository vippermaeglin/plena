// Backtest.h : Declaration of the CBacktest

#ifndef __BACKTEST_H_
#define __BACKTEST_H_

#include "resource.h"       // main symbols
#include "Script.h"
#include "TradeScriptCP.h"

/////////////////////////////////////////////////////////////////////////////
// CBacktest
class ATL_NO_VTABLE CBacktest : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CBacktest, &CLSID_Backtest>,
	public ISupportErrorInfo,
	public IConnectionPointContainerImpl<CBacktest>,
	public IDispatchImpl<IBacktest, &IID_IBacktest, &LIBID_TradeScriptLib, 2008>,
	public CProxy_IBacktestEvents< CBacktest >
	{ // VERSION NUMBER MUST MATCH IDL FILE (2008)
public:
	CBacktest()
	{
		m_exceeded = false;
		m_license = "";
	}
DECLARE_CLASSFACTORY2(CLicense)

DECLARE_REGISTRY_RESOURCEID(IDR_BACKTEST)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CBacktest)
	COM_INTERFACE_ENTRY(IBacktest)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY_IMPL(IConnectionPointContainer)
END_COM_MAP()
BEGIN_CONNECTION_POINT_MAP(CBacktest)
CONNECTION_POINT_ENTRY(DIID__IBacktestEvents)
END_CONNECTION_POINT_MAP()


// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

// IBacktest

vector<record> m_records;
string m_symbol;
string	m_script;
string m_BacktestName;
bool m_exceeded;
void RunScript();
bool CheckLicense();
string m_license;
Script m_oScript;
public:
	STDMETHOD(get_ScriptHelp)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_License)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_License)(/*[in]*/ BSTR newVal);
	STDMETHOD(Backtest)(BSTR BuyScript, BSTR SellScript, BSTR ExitLongScript, BSTR ExitShortScript, double SlipPct, BSTR *pRet);	
	STDMETHOD(FromJulianDate)(/*[in]*/ double JDate, /*[out, retval]*/ BSTR *pRet);
	STDMETHOD(ToJulianDate)(int nYear, int nMonth, int nDay, int nHour, int nMinute, int nSecond, int nMillisecond, double *pRet);
	STDMETHOD(get_RecordCount)(/*[out, retval]*/ long *pVal);
	STDMETHOD(ClearRecords)();
	STDMETHOD(GetRecordByJDate)(double JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(GetRecordByIndex)(long Index, double *JDate, double *OpenPrice, double *HighPrice, double *LowPrice, double* ClosePrice, long* Volume, VARIANT_BOOL *pRet);
	STDMETHOD(EditRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume, VARIANT_BOOL *pRet);
	STDMETHOD(AppendRecord)(double JDate, double OpenPrice, double HighPrice, double LowPrice, double ClosePrice, long Volume);
};

#endif //__BACKTEST_H_
