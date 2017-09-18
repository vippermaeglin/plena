// Validate.h : Declaration of the CValidate

#ifndef __VALIDATE_H_
#define __VALIDATE_H_
#include "Script.h"
#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CValidate
class ATL_NO_VTABLE CValidate : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CValidate, &CLSID_Validate>,
	public IDispatchImpl<IValidate, &IID_IValidate, &LIBID_TradeScriptLib, 2008>
{ // VERSION NUMBER MUST MATCH IDL FILE (2008)
public:
	CValidate()
	{
		m_license = "";
	}


	static __inline void makeupper(std::string &str) 
	{ 
		std::string::iterator it = str.begin(); 
		std::string::iterator end = str.end(); 

		for( ; it != end; ++it) 
		{ 
			*it = toupper(*it); 
		}
	}

DECLARE_REGISTRY_RESOURCEID(IDR_VALIDATE)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CValidate)
	COM_INTERFACE_ENTRY(IValidate)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()

bool CheckLicense();
string m_license;
string m_script;
string m_exchange, m_sector, m_industry, m_symbol;
Script m_oScript;

// IValidate
public:
	STDMETHOD(get_ScriptHelp)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_Constant)(BSTR Name, /*[out, retval]*/ BSTR *pVal);
	STDMETHOD(Validate)(/*[in]*/ BSTR ValidateScript, /*[out,reval]*/ BSTR *pRet);	
	STDMETHOD(get_License)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_License)(/*[in]*/ BSTR newVal);
};

#endif //__VALIDATE_H_
