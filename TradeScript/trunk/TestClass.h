// TestClass.h : Declaration of the CTestClass

#ifndef __TESTCLASS_H_
#define __TESTCLASS_H_

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTestClass
class ATL_NO_VTABLE CTestClass : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CTestClass, &CLSID_TestClass>,
	public IDispatchImpl<ITestClass, &IID_ITestClass, &LIBID_TradeScriptLib>
{
public:
	CTestClass()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_TESTCLASS)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CTestClass)
	COM_INTERFACE_ENTRY(ITestClass)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()

// ITestClass
public:
	STDMETHOD(Test)();
};

#endif //__TESTCLASS_H_
