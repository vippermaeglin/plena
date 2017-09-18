// Validate.cpp : Implementation of CValidate
#include "stdafx.h"
#include "TradeScript.h"
#include "Validate.h"

/////////////////////////////////////////////////////////////////////////////
// CValidate

// This class validates a script and returns any errors.
// It also sets the Exchange, Sector, Industry, and any 
// other constants that are set in the script.

STDMETHODIMP CValidate::Validate(BSTR ValidateScript, BSTR *pRet)
{

	USES_CONVERSION;
	try{
		m_script = W2A(ValidateScript);
	}
	catch(...){}

	if(m_script.length() < 5) return S_OK;

	if(!CheckLicense())
	{
		string err = "Error: License information not found";
		BSTR Description = A2BSTR(err.c_str());
		return S_OK;
	}
	
	string ret = m_oScript.RunScript(m_script, false, true);
	if(ret == "ALERT") ret = ""; // Not interested here

	*pRet = CComBSTR(ret.c_str()).Detach();

	return S_OK;
}



// Don't allow this program to run on Windows Server
bool CValidate::CheckLicense()
{
#ifdef DEMO_MODE
	return true;
#endif
	return (m_license == "XRT93NQR79ABTW788XR48");	
}

STDMETHODIMP CValidate::get_License(BSTR *pVal)
{	
	*pVal = CComBSTR(m_license.c_str()).Detach();
	return S_OK;
}

STDMETHODIMP CValidate::put_License(BSTR newVal)
{
	USES_CONVERSION;
	try{
		m_license = W2A(newVal);
	}
	catch(...){}

	return S_OK;
}

STDMETHODIMP CValidate::get_Constant(BSTR Name, BSTR *pVal)
{
	string name;
	USES_CONVERSION;
	try{
		name = W2A(Name);
	}
	catch(...){}
	
	makeupper(name);

	string ret;
	if(name == "SYMBOL")
		ret = m_oScript.m_symbol;
	else if(name == "EXCHANGE")
		ret = m_oScript.m_exchange;
	else if(name == "SECTOR")
		ret = m_oScript.m_sector;
	else if(name == "INDUSTRY")
		ret = m_oScript.m_industry;

	*pVal = CComBSTR(ret.c_str()).Detach();

	return S_OK;
}

STDMETHODIMP CValidate::get_ScriptHelp(BSTR *pVal)
{
	string help = m_oScript.GetScriptHelp();
	*pVal = CComBSTR(help.c_str()).Detach();
	return S_OK;
}
