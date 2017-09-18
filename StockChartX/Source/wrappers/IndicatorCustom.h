// IndicatorCustom.h: interface for the CIndicatorCustom class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INDICATORCUSTOM_H__D6A8B152_9631_4138_9A48_B1A87C54FE65__INCLUDED_)
#define AFX_INDICATORCUSTOM_H__D6A8B152_9631_4138_9A48_B1A87C54FE65__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Indicator.h"

class CIndicatorCustom : public CIndicator  
{
public:
  void SetParamInfo();
  CIndicatorCustom(LPCTSTR name, int type, int members, CChartPanel* owner);
  virtual ~CIndicatorCustom();
  BOOL Calculate();

  long AddPropStr(long lParamType,LPCTSTR szValue);
  long AddPropInt(long lParamType,int iValue);
  long AddPropDbl(long lParamType,double dValue);

  long SetData(SAFEARRAY *array,BOOL bAppend);
private:
  int m_iLastIndex;
  std::vector<double> m_dValues;

  BOOL ThrowIndexOutOfBounds();
  void resize_param_arrays();
};



#endif // !defined(AFX_INDICATORCUSTOM_H__D6A8B152_9631_4138_9A48_B1A87C54FE65__INCLUDED_)
