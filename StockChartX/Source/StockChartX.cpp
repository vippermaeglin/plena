// StockChartX.cpp : Implementation of CStockChartXApp and DLL registration.

#include "stdafx.h"
#include "StockChartX.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CStockChartXApp NEAR theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0x6556e667, 0xf828, 0x4d0e, { 0xaf, 0x3e, 0xc0, 0xfc, 0x76, 0x87, 0x96, 0xcc } };
const WORD _wVerMajor = 5;
const WORD _wVerMinor = 0;


////////////////////////////////////////////////////////////////////////////
// CStockChartXApp::InitInstance - DLL initialization

BOOL CStockChartXApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{

		CoInitialize(NULL);
		
 


		//HRESULT hr = CLSIDFromProgID(L"STOCKCHARTX.StockChartXCtrl.1",&clsid);
		//if(FAILED(hr)){
		//	MessageBox(NULL, "Failed to register STOCKCHARTX.StockChartXCtrl.1","REGSVR32",0);
		//	return FALSE;
		//}

		
		//HRESULT hr = CLSIDFromString(L"{89D9AAAF-DC4F-4F72-ACF3-D2EDB8644A44}", &m_clsid);
		//if(FAILED(hr)){
		//	MessageBox(NULL, "Failed to register {89D9AAAF-DC4F-4F72-ACF3-D2EDB8644A44}","REGSVR32",0);
		//	return FALSE;
		//}

		//IIDFromString(L"{40FC6ED4-2438-11CF-A3DB-080036F12502}", &iid);


		//hr = CoCreateInstance(m_clsid, NULL, CLSCTX_INPROC_SERVER ,
		//_tlid, (void **)&theApp.pCtrl);


	}

	return bInit;
}


////////////////////////////////////////////////////////////////////////////
// CStockChartXApp::ExitInstance - DLL termination

int CStockChartXApp::ExitInstance()
{
	// TODO: Add your own module termination code here.
	CoUninitialize();
	return COleControlModule::ExitInstance();
}


/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}


/////////////////////////////////////////////////////////////////////////////
// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
 