

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Wed Dec 03 15:29:12 2014
 */
/* Compiler settings for TradeScript.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0603 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __TradeScript_h__
#define __TradeScript_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IAlert_FWD_DEFINED__
#define __IAlert_FWD_DEFINED__
typedef interface IAlert IAlert;

#endif 	/* __IAlert_FWD_DEFINED__ */


#ifndef __IBacktest_FWD_DEFINED__
#define __IBacktest_FWD_DEFINED__
typedef interface IBacktest IBacktest;

#endif 	/* __IBacktest_FWD_DEFINED__ */


#ifndef __IScriptOutput_FWD_DEFINED__
#define __IScriptOutput_FWD_DEFINED__
typedef interface IScriptOutput IScriptOutput;

#endif 	/* __IScriptOutput_FWD_DEFINED__ */


#ifndef __IValidate_FWD_DEFINED__
#define __IValidate_FWD_DEFINED__
typedef interface IValidate IValidate;

#endif 	/* __IValidate_FWD_DEFINED__ */


#ifndef ___IAlertEvents_FWD_DEFINED__
#define ___IAlertEvents_FWD_DEFINED__
typedef interface _IAlertEvents _IAlertEvents;

#endif 	/* ___IAlertEvents_FWD_DEFINED__ */


#ifndef __Alert_FWD_DEFINED__
#define __Alert_FWD_DEFINED__

#ifdef __cplusplus
typedef class Alert Alert;
#else
typedef struct Alert Alert;
#endif /* __cplusplus */

#endif 	/* __Alert_FWD_DEFINED__ */


#ifndef ___IBacktestEvents_FWD_DEFINED__
#define ___IBacktestEvents_FWD_DEFINED__
typedef interface _IBacktestEvents _IBacktestEvents;

#endif 	/* ___IBacktestEvents_FWD_DEFINED__ */


#ifndef __Backtest_FWD_DEFINED__
#define __Backtest_FWD_DEFINED__

#ifdef __cplusplus
typedef class Backtest Backtest;
#else
typedef struct Backtest Backtest;
#endif /* __cplusplus */

#endif 	/* __Backtest_FWD_DEFINED__ */


#ifndef ___IScriptOutputEvents_FWD_DEFINED__
#define ___IScriptOutputEvents_FWD_DEFINED__
typedef interface _IScriptOutputEvents _IScriptOutputEvents;

#endif 	/* ___IScriptOutputEvents_FWD_DEFINED__ */


#ifndef __IScanner_FWD_DEFINED__
#define __IScanner_FWD_DEFINED__
typedef interface IScanner IScanner;

#endif 	/* __IScanner_FWD_DEFINED__ */


#ifndef __ScriptOutput_FWD_DEFINED__
#define __ScriptOutput_FWD_DEFINED__

#ifdef __cplusplus
typedef class ScriptOutput ScriptOutput;
#else
typedef struct ScriptOutput ScriptOutput;
#endif /* __cplusplus */

#endif 	/* __ScriptOutput_FWD_DEFINED__ */


#ifndef __Validate_FWD_DEFINED__
#define __Validate_FWD_DEFINED__

#ifdef __cplusplus
typedef class Validate Validate;
#else
typedef struct Validate Validate;
#endif /* __cplusplus */

#endif 	/* __Validate_FWD_DEFINED__ */


#ifndef ___IScannerEvents_FWD_DEFINED__
#define ___IScannerEvents_FWD_DEFINED__
typedef interface _IScannerEvents _IScannerEvents;

#endif 	/* ___IScannerEvents_FWD_DEFINED__ */


#ifndef __Scanner_FWD_DEFINED__
#define __Scanner_FWD_DEFINED__

#ifdef __cplusplus
typedef class Scanner Scanner;
#else
typedef struct Scanner Scanner;
#endif /* __cplusplus */

#endif 	/* __Scanner_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IAlert_INTERFACE_DEFINED__
#define __IAlert_INTERFACE_DEFINED__

/* interface IAlert */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IAlert;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("7E2BD722-EBC5-481B-B690-01215E0B0348")
    IAlert : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AppendRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByJDate( 
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByIndex( 
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearRecords( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RecordCount( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Symbol( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Symbol( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AlertScript( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AlertScript( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ToJulianDate( 
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FromJulianDate( 
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AlertName( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AlertName( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_License( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_License( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScriptHelp( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetJDate( 
            /* [in] */ long Index,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Evaluate( 
            /* [in] */ BSTR EvaluateScript,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AppendHistoryRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IAlertVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IAlert * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IAlert * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IAlert * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IAlert * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IAlert * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IAlert * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IAlert * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AppendRecord )( 
            IAlert * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditRecord )( 
            IAlert * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByJDate )( 
            IAlert * This,
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByIndex )( 
            IAlert * This,
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearRecords )( 
            IAlert * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RecordCount )( 
            IAlert * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Symbol )( 
            IAlert * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Symbol )( 
            IAlert * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AlertScript )( 
            IAlert * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AlertScript )( 
            IAlert * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ToJulianDate )( 
            IAlert * This,
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FromJulianDate )( 
            IAlert * This,
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AlertName )( 
            IAlert * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AlertName )( 
            IAlert * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_License )( 
            IAlert * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_License )( 
            IAlert * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScriptHelp )( 
            IAlert * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetJDate )( 
            IAlert * This,
            /* [in] */ long Index,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Evaluate )( 
            IAlert * This,
            /* [in] */ BSTR EvaluateScript,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AppendHistoryRecord )( 
            IAlert * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume);
        
        END_INTERFACE
    } IAlertVtbl;

    interface IAlert
    {
        CONST_VTBL struct IAlertVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IAlert_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IAlert_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IAlert_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IAlert_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IAlert_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IAlert_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IAlert_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IAlert_AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume)	\
    ( (This)->lpVtbl -> AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume) ) 

#define IAlert_EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IAlert_GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IAlert_GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IAlert_ClearRecords(This)	\
    ( (This)->lpVtbl -> ClearRecords(This) ) 

#define IAlert_get_RecordCount(This,pVal)	\
    ( (This)->lpVtbl -> get_RecordCount(This,pVal) ) 

#define IAlert_get_Symbol(This,pVal)	\
    ( (This)->lpVtbl -> get_Symbol(This,pVal) ) 

#define IAlert_put_Symbol(This,newVal)	\
    ( (This)->lpVtbl -> put_Symbol(This,newVal) ) 

#define IAlert_get_AlertScript(This,pVal)	\
    ( (This)->lpVtbl -> get_AlertScript(This,pVal) ) 

#define IAlert_put_AlertScript(This,newVal)	\
    ( (This)->lpVtbl -> put_AlertScript(This,newVal) ) 

#define IAlert_ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet)	\
    ( (This)->lpVtbl -> ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet) ) 

#define IAlert_FromJulianDate(This,JDate,pRet)	\
    ( (This)->lpVtbl -> FromJulianDate(This,JDate,pRet) ) 

#define IAlert_get_AlertName(This,pVal)	\
    ( (This)->lpVtbl -> get_AlertName(This,pVal) ) 

#define IAlert_put_AlertName(This,newVal)	\
    ( (This)->lpVtbl -> put_AlertName(This,newVal) ) 

#define IAlert_get_License(This,pVal)	\
    ( (This)->lpVtbl -> get_License(This,pVal) ) 

#define IAlert_put_License(This,newVal)	\
    ( (This)->lpVtbl -> put_License(This,newVal) ) 

#define IAlert_get_ScriptHelp(This,pVal)	\
    ( (This)->lpVtbl -> get_ScriptHelp(This,pVal) ) 

#define IAlert_GetJDate(This,Index,pRet)	\
    ( (This)->lpVtbl -> GetJDate(This,Index,pRet) ) 

#define IAlert_Evaluate(This,EvaluateScript,pRet)	\
    ( (This)->lpVtbl -> Evaluate(This,EvaluateScript,pRet) ) 

#define IAlert_AppendHistoryRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume)	\
    ( (This)->lpVtbl -> AppendHistoryRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IAlert_INTERFACE_DEFINED__ */


#ifndef __IBacktest_INTERFACE_DEFINED__
#define __IBacktest_INTERFACE_DEFINED__

/* interface IBacktest */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IBacktest;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("6310CAB4-5528-4533-9AA8-37BD763FE109")
    IBacktest : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AppendRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByJDate( 
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByIndex( 
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearRecords( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RecordCount( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ToJulianDate( 
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FromJulianDate( 
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Backtest( 
            /* [in] */ BSTR BuyScript,
            /* [in] */ BSTR SellScript,
            /* [in] */ BSTR ExitLongScript,
            /* [in] */ BSTR ExitShortScript,
            /* [in] */ double SlipPct,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_License( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_License( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScriptHelp( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IBacktestVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IBacktest * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IBacktest * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IBacktest * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IBacktest * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IBacktest * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IBacktest * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IBacktest * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AppendRecord )( 
            IBacktest * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditRecord )( 
            IBacktest * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByJDate )( 
            IBacktest * This,
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByIndex )( 
            IBacktest * This,
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearRecords )( 
            IBacktest * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RecordCount )( 
            IBacktest * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ToJulianDate )( 
            IBacktest * This,
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FromJulianDate )( 
            IBacktest * This,
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Backtest )( 
            IBacktest * This,
            /* [in] */ BSTR BuyScript,
            /* [in] */ BSTR SellScript,
            /* [in] */ BSTR ExitLongScript,
            /* [in] */ BSTR ExitShortScript,
            /* [in] */ double SlipPct,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_License )( 
            IBacktest * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_License )( 
            IBacktest * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScriptHelp )( 
            IBacktest * This,
            /* [retval][out] */ BSTR *pVal);
        
        END_INTERFACE
    } IBacktestVtbl;

    interface IBacktest
    {
        CONST_VTBL struct IBacktestVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IBacktest_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IBacktest_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IBacktest_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IBacktest_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IBacktest_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IBacktest_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IBacktest_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IBacktest_AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume)	\
    ( (This)->lpVtbl -> AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume) ) 

#define IBacktest_EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IBacktest_GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IBacktest_GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IBacktest_ClearRecords(This)	\
    ( (This)->lpVtbl -> ClearRecords(This) ) 

#define IBacktest_get_RecordCount(This,pVal)	\
    ( (This)->lpVtbl -> get_RecordCount(This,pVal) ) 

#define IBacktest_ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet)	\
    ( (This)->lpVtbl -> ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet) ) 

#define IBacktest_FromJulianDate(This,JDate,pRet)	\
    ( (This)->lpVtbl -> FromJulianDate(This,JDate,pRet) ) 

#define IBacktest_Backtest(This,BuyScript,SellScript,ExitLongScript,ExitShortScript,SlipPct,pRet)	\
    ( (This)->lpVtbl -> Backtest(This,BuyScript,SellScript,ExitLongScript,ExitShortScript,SlipPct,pRet) ) 

#define IBacktest_get_License(This,pVal)	\
    ( (This)->lpVtbl -> get_License(This,pVal) ) 

#define IBacktest_put_License(This,newVal)	\
    ( (This)->lpVtbl -> put_License(This,newVal) ) 

#define IBacktest_get_ScriptHelp(This,pVal)	\
    ( (This)->lpVtbl -> get_ScriptHelp(This,pVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IBacktest_INTERFACE_DEFINED__ */


#ifndef __IScriptOutput_INTERFACE_DEFINED__
#define __IScriptOutput_INTERFACE_DEFINED__

/* interface IScriptOutput */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IScriptOutput;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("FD64E200-218C-4920-9CF4-E145AAFA718C")
    IScriptOutput : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AppendRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByJDate( 
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByIndex( 
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearRecords( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RecordCount( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ToJulianDate( 
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FromJulianDate( 
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetScriptOutput( 
            /* [in] */ BSTR DefaultScript,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_License( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_License( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScriptHelp( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IScriptOutputVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IScriptOutput * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IScriptOutput * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IScriptOutput * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IScriptOutput * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IScriptOutput * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IScriptOutput * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IScriptOutput * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AppendRecord )( 
            IScriptOutput * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditRecord )( 
            IScriptOutput * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByJDate )( 
            IScriptOutput * This,
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByIndex )( 
            IScriptOutput * This,
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearRecords )( 
            IScriptOutput * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RecordCount )( 
            IScriptOutput * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ToJulianDate )( 
            IScriptOutput * This,
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FromJulianDate )( 
            IScriptOutput * This,
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetScriptOutput )( 
            IScriptOutput * This,
            /* [in] */ BSTR DefaultScript,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_License )( 
            IScriptOutput * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_License )( 
            IScriptOutput * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScriptHelp )( 
            IScriptOutput * This,
            /* [retval][out] */ BSTR *pVal);
        
        END_INTERFACE
    } IScriptOutputVtbl;

    interface IScriptOutput
    {
        CONST_VTBL struct IScriptOutputVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IScriptOutput_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IScriptOutput_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IScriptOutput_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IScriptOutput_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IScriptOutput_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IScriptOutput_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IScriptOutput_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IScriptOutput_AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume)	\
    ( (This)->lpVtbl -> AppendRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume) ) 

#define IScriptOutput_EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScriptOutput_GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScriptOutput_GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScriptOutput_ClearRecords(This)	\
    ( (This)->lpVtbl -> ClearRecords(This) ) 

#define IScriptOutput_get_RecordCount(This,pVal)	\
    ( (This)->lpVtbl -> get_RecordCount(This,pVal) ) 

#define IScriptOutput_ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet)	\
    ( (This)->lpVtbl -> ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet) ) 

#define IScriptOutput_FromJulianDate(This,JDate,pRet)	\
    ( (This)->lpVtbl -> FromJulianDate(This,JDate,pRet) ) 

#define IScriptOutput_GetScriptOutput(This,DefaultScript,pRet)	\
    ( (This)->lpVtbl -> GetScriptOutput(This,DefaultScript,pRet) ) 

#define IScriptOutput_get_License(This,pVal)	\
    ( (This)->lpVtbl -> get_License(This,pVal) ) 

#define IScriptOutput_put_License(This,newVal)	\
    ( (This)->lpVtbl -> put_License(This,newVal) ) 

#define IScriptOutput_get_ScriptHelp(This,pVal)	\
    ( (This)->lpVtbl -> get_ScriptHelp(This,pVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IScriptOutput_INTERFACE_DEFINED__ */


#ifndef __IValidate_INTERFACE_DEFINED__
#define __IValidate_INTERFACE_DEFINED__

/* interface IValidate */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IValidate;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A8F448D8-D8FB-4991-835E-D8F5BD97741B")
    IValidate : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Validate( 
            /* [in] */ BSTR ValidateScript,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_License( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_License( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Constant( 
            BSTR Name,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScriptHelp( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IValidateVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IValidate * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IValidate * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IValidate * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IValidate * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IValidate * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IValidate * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IValidate * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Validate )( 
            IValidate * This,
            /* [in] */ BSTR ValidateScript,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_License )( 
            IValidate * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_License )( 
            IValidate * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Constant )( 
            IValidate * This,
            BSTR Name,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScriptHelp )( 
            IValidate * This,
            /* [retval][out] */ BSTR *pVal);
        
        END_INTERFACE
    } IValidateVtbl;

    interface IValidate
    {
        CONST_VTBL struct IValidateVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IValidate_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IValidate_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IValidate_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IValidate_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IValidate_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IValidate_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IValidate_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IValidate_Validate(This,ValidateScript,pRet)	\
    ( (This)->lpVtbl -> Validate(This,ValidateScript,pRet) ) 

#define IValidate_get_License(This,pVal)	\
    ( (This)->lpVtbl -> get_License(This,pVal) ) 

#define IValidate_put_License(This,newVal)	\
    ( (This)->lpVtbl -> put_License(This,newVal) ) 

#define IValidate_get_Constant(This,Name,pVal)	\
    ( (This)->lpVtbl -> get_Constant(This,Name,pVal) ) 

#define IValidate_get_ScriptHelp(This,pVal)	\
    ( (This)->lpVtbl -> get_ScriptHelp(This,pVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IValidate_INTERFACE_DEFINED__ */



#ifndef __TradeScriptLib_LIBRARY_DEFINED__
#define __TradeScriptLib_LIBRARY_DEFINED__

/* library TradeScriptLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_TradeScriptLib;

#ifndef ___IAlertEvents_DISPINTERFACE_DEFINED__
#define ___IAlertEvents_DISPINTERFACE_DEFINED__

/* dispinterface _IAlertEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__IAlertEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("8EAE6DA6-5EDF-4B01-92F6-00D4F56E91D0")
    _IAlertEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _IAlertEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IAlertEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IAlertEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IAlertEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _IAlertEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _IAlertEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _IAlertEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _IAlertEvents * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _IAlertEventsVtbl;

    interface _IAlertEvents
    {
        CONST_VTBL struct _IAlertEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IAlertEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IAlertEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IAlertEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IAlertEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _IAlertEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _IAlertEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _IAlertEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___IAlertEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Alert;

#ifdef __cplusplus

class DECLSPEC_UUID("83392496-8720-4F6B-B906-984B587FDF74")
Alert;
#endif

#ifndef ___IBacktestEvents_DISPINTERFACE_DEFINED__
#define ___IBacktestEvents_DISPINTERFACE_DEFINED__

/* dispinterface _IBacktestEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__IBacktestEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("48E1FE57-167A-4BF1-A87F-1CDF759FA8B9")
    _IBacktestEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _IBacktestEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IBacktestEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IBacktestEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IBacktestEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _IBacktestEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _IBacktestEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _IBacktestEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _IBacktestEvents * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _IBacktestEventsVtbl;

    interface _IBacktestEvents
    {
        CONST_VTBL struct _IBacktestEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IBacktestEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IBacktestEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IBacktestEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IBacktestEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _IBacktestEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _IBacktestEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _IBacktestEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___IBacktestEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Backtest;

#ifdef __cplusplus

class DECLSPEC_UUID("DDAF2F33-6E54-4161-AFCD-15058E0AA338")
Backtest;
#endif

#ifndef ___IScriptOutputEvents_DISPINTERFACE_DEFINED__
#define ___IScriptOutputEvents_DISPINTERFACE_DEFINED__

/* dispinterface _IScriptOutputEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__IScriptOutputEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("95F45E4F-0588-45BB-A7BA-38E9CEFFBFDB")
    _IScriptOutputEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _IScriptOutputEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IScriptOutputEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IScriptOutputEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IScriptOutputEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _IScriptOutputEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _IScriptOutputEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _IScriptOutputEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _IScriptOutputEvents * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _IScriptOutputEventsVtbl;

    interface _IScriptOutputEvents
    {
        CONST_VTBL struct _IScriptOutputEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IScriptOutputEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IScriptOutputEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IScriptOutputEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IScriptOutputEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _IScriptOutputEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _IScriptOutputEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _IScriptOutputEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___IScriptOutputEvents_DISPINTERFACE_DEFINED__ */


#ifndef __IScanner_INTERFACE_DEFINED__
#define __IScanner_INTERFACE_DEFINED__

/* interface IScanner */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IScanner;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("B36744FF-29DA-4D1A-8EC5-FA28709150A9")
    IScanner : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AppendRecord( 
            /* [in] */ BSTR Symbol,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditRecord( 
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByJDate( 
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRecordByIndex( 
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearRecords( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RecordCount( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScannerScript( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ScannerScript( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ToJulianDate( 
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FromJulianDate( 
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_License( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_License( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScriptHelp( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetJDate( 
            /* [in] */ long Index,
            /* [retval][out] */ double *pRet) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Evaluate( 
            /* [in] */ BSTR EvaluateScript,
            /* [retval][out] */ VARIANT_BOOL *pRet) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IScannerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IScanner * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IScanner * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IScanner * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IScanner * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IScanner * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IScanner * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IScanner * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AppendRecord )( 
            IScanner * This,
            /* [in] */ BSTR Symbol,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditRecord )( 
            IScanner * This,
            /* [in] */ double JDate,
            /* [in] */ double OpenPrice,
            /* [in] */ double HighPrice,
            /* [in] */ double LowPrice,
            /* [in] */ double ClosePrice,
            /* [in] */ long Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByJDate )( 
            IScanner * This,
            /* [in] */ double JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRecordByIndex )( 
            IScanner * This,
            /* [in] */ long Index,
            /* [in] */ double *JDate,
            /* [in] */ double *OpenPrice,
            /* [in] */ double *HighPrice,
            /* [in] */ double *LowPrice,
            /* [in] */ double *ClosePrice,
            /* [in] */ long *Volume,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearRecords )( 
            IScanner * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RecordCount )( 
            IScanner * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScannerScript )( 
            IScanner * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ScannerScript )( 
            IScanner * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ToJulianDate )( 
            IScanner * This,
            /* [in] */ int nYear,
            /* [in] */ int nMonth,
            /* [in] */ int nDay,
            /* [in] */ int nHour,
            /* [in] */ int nMinute,
            /* [in] */ int nSecond,
            /* [in] */ int nMillisecond,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FromJulianDate )( 
            IScanner * This,
            /* [in] */ double JDate,
            /* [retval][out] */ BSTR *pRet);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_License )( 
            IScanner * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_License )( 
            IScanner * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScriptHelp )( 
            IScanner * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetJDate )( 
            IScanner * This,
            /* [in] */ long Index,
            /* [retval][out] */ double *pRet);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Evaluate )( 
            IScanner * This,
            /* [in] */ BSTR EvaluateScript,
            /* [retval][out] */ VARIANT_BOOL *pRet);
        
        END_INTERFACE
    } IScannerVtbl;

    interface IScanner
    {
        CONST_VTBL struct IScannerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IScanner_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IScanner_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IScanner_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IScanner_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IScanner_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IScanner_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IScanner_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IScanner_AppendRecord(This,Symbol,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume)	\
    ( (This)->lpVtbl -> AppendRecord(This,Symbol,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume) ) 

#define IScanner_EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> EditRecord(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScanner_GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByJDate(This,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScanner_GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet)	\
    ( (This)->lpVtbl -> GetRecordByIndex(This,Index,JDate,OpenPrice,HighPrice,LowPrice,ClosePrice,Volume,pRet) ) 

#define IScanner_ClearRecords(This)	\
    ( (This)->lpVtbl -> ClearRecords(This) ) 

#define IScanner_get_RecordCount(This,pVal)	\
    ( (This)->lpVtbl -> get_RecordCount(This,pVal) ) 

#define IScanner_get_ScannerScript(This,pVal)	\
    ( (This)->lpVtbl -> get_ScannerScript(This,pVal) ) 

#define IScanner_put_ScannerScript(This,newVal)	\
    ( (This)->lpVtbl -> put_ScannerScript(This,newVal) ) 

#define IScanner_ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet)	\
    ( (This)->lpVtbl -> ToJulianDate(This,nYear,nMonth,nDay,nHour,nMinute,nSecond,nMillisecond,pRet) ) 

#define IScanner_FromJulianDate(This,JDate,pRet)	\
    ( (This)->lpVtbl -> FromJulianDate(This,JDate,pRet) ) 

#define IScanner_get_License(This,pVal)	\
    ( (This)->lpVtbl -> get_License(This,pVal) ) 

#define IScanner_put_License(This,newVal)	\
    ( (This)->lpVtbl -> put_License(This,newVal) ) 

#define IScanner_get_ScriptHelp(This,pVal)	\
    ( (This)->lpVtbl -> get_ScriptHelp(This,pVal) ) 

#define IScanner_GetJDate(This,Index,pRet)	\
    ( (This)->lpVtbl -> GetJDate(This,Index,pRet) ) 

#define IScanner_Evaluate(This,EvaluateScript,pRet)	\
    ( (This)->lpVtbl -> Evaluate(This,EvaluateScript,pRet) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IScanner_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_ScriptOutput;

#ifdef __cplusplus

class DECLSPEC_UUID("5CA7CD37-4019-427C-A5BF-4A8C82DD9537")
ScriptOutput;
#endif

EXTERN_C const CLSID CLSID_Validate;

#ifdef __cplusplus

class DECLSPEC_UUID("92347C5B-95EB-4C71-8CB6-7F1AE45A2726")
Validate;
#endif

#ifndef ___IScannerEvents_DISPINTERFACE_DEFINED__
#define ___IScannerEvents_DISPINTERFACE_DEFINED__

/* dispinterface _IScannerEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__IScannerEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("BAF35027-2DD5-4CCF-82CE-5B5388D74341")
    _IScannerEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _IScannerEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IScannerEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IScannerEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IScannerEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _IScannerEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _IScannerEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _IScannerEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _IScannerEvents * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _IScannerEventsVtbl;

    interface _IScannerEvents
    {
        CONST_VTBL struct _IScannerEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IScannerEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IScannerEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IScannerEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IScannerEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _IScannerEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _IScannerEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _IScannerEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___IScannerEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Scanner;

#ifdef __cplusplus

class DECLSPEC_UUID("93FD1A72-C3AC-41B0-AAD7-D34C8083EED6")
Scanner;
#endif
#endif /* __TradeScriptLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


