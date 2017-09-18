// IndicatorCustom.cpp: implementation of the CIndicatorCustom class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "..\StockChartX.h"
#include "IndicatorCustom.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
#define MAX_PARAM_COUNT 9//not 10 cause paramStr[0] must be series data

CIndicatorCustom::CIndicatorCustom(LPCTSTR name, int type, int members, CChartPanel* owner)
{
  szName = name;
  ownerPanel = owner;
  pCtrl = owner->pCtrl;
  seriesType = type;
  memberCount = members;
  Initialize();
  nSpace = 0;
  
  // Resize param arrays for this indicator.
  // NOTE! Set all array sizes to max number of parameters.
  // ALL three arrays must be resized.
  paramCount = 0; //no parameters
  paramStr.reserve(MAX_PARAM_COUNT);
  paramDbl.reserve(MAX_PARAM_COUNT);
  paramInt.reserve(MAX_PARAM_COUNT);
  
  indicatorType = indCustomIndicator;

  m_iLastIndex = 0;

  AddPropStr( ptSource, "" );
}

CIndicatorCustom::~CIndicatorCustom()
{
  CIndicator::OnDestroy();
}

void CIndicatorCustom::SetParamInfo()
{
  //no parameters
}

BOOL CIndicatorCustom::Calculate()

{
                

                /*

                                1. Validate the indicator parameters (if any)

                                2. Validate available inputs

                                3. Gather the inputs into a TA-SDK recordset

                                4. Calculate the indicator

                                5. If there is only one output, store the data

                                   in the data_master array of this series. 

                                   If there are two or more outputs, create new 

                                   CSeriesStandard for each additional ouput

                */

 

                // Get input from user

                if(!GetUserInput()) return FALSE;

 

                // Validate

                long size = pCtrl->RecordCount();

                if(size == 0) return FALSE;

//            Revision added 6/10/2004 By Katchei

//            Added type cast to suppress errors

                if(paramStr.size() < (unsigned int)paramCount)

//            End Of Revision

                                return FALSE;

  // Get the data

 
  //fire event need data

  pCtrl->FireCustomIndicatorEventNeedData( szName );//here programmer may fill indicator with parameters

  CString sMsg;

  sMsg.Format( "CustomIndicator Need Data: %s\n", szName );

  OutputDebugString( sMsg );
 

  Clear();


  int index = pCtrl->GetTopChartPanel();

  if (index == -1) return FALSE; //no panels return

  if (pCtrl->panels[index]->series.size() == 0) return FALSE;

  CSeries* series = pCtrl->panels[index]->series[0];

 

  double value = 0, jdate = 0;

  std::vector<double>::size_type lDataLength = m_dValues.size();

  for(int n = 0; n < size; ++n)

  {

    if(n >= (int)lDataLength)
    {

      value = NULL_VALUE;
    }
    else
    {
      value = m_dValues[ n ];
    }

    jdate = series->data_master[ n ].jdate;
    

    AppendValue(jdate, value);

  }

 
  return CIndicator::Calculate();

}

 

long CIndicatorCustom::AddPropStr(long lParamType,LPCTSTR szValue)
{
  if( !ThrowIndexOutOfBounds() )  
    return -1;
  paramTypes[ m_iLastIndex ] = lParamType;
  resize_param_arrays();
  paramStr[ m_iLastIndex++ ] = szValue;
  return m_iLastIndex;
}

long CIndicatorCustom::AddPropInt(long lParamType,int iValue)
{
  if( !ThrowIndexOutOfBounds() )
    return -1;
  paramTypes[ m_iLastIndex ] = lParamType;
  resize_param_arrays();
  paramInt[ m_iLastIndex++ ] = iValue;
  return m_iLastIndex;
}

long CIndicatorCustom::AddPropDbl(long lParamType,double dValue)
{
  if( !ThrowIndexOutOfBounds() )
    return -1;
  paramTypes[ m_iLastIndex ] = lParamType;
  resize_param_arrays();
  paramDbl[ m_iLastIndex++ ] = dValue;
  return m_iLastIndex;
}

BOOL CIndicatorCustom::ThrowIndexOutOfBounds()
{
  if( m_iLastIndex >= MAX_PARAM_COUNT )
  {
    CString sErr;
    sErr.Format("Index out of bounds. Maximum %d custom parameters allowed.", MAX_PARAM_COUNT);
    ProcessError( sErr );
    return FALSE;
  }
  return true;
}

void CIndicatorCustom::resize_param_arrays()
{
  paramInt.resize( m_iLastIndex + 1 );
  paramStr.resize( m_iLastIndex + 1 );
  paramDbl.resize( m_iLastIndex + 1 );
}

long CIndicatorCustom::SetData(SAFEARRAY *array,BOOL bAppend)
{
  double *pArray;
  HRESULT hr;
  int i = 0;

  hr = SafeArrayAccessData( array, (void HUGEP**)&pArray );
  if( FAILED(hr) )
    return -1;
  if( !bAppend )
    m_dValues.clear();
  for( i = 0; i < (int)array->rgsabound[0].cElements; i++ )
  {
    //CString sd;
    //sd.Format("%d-%f\n", i, pArray[ i ]);
    ///OutputDebugString( sd );

    m_dValues.push_back( pArray[ i ] );
  }
  SafeArrayUnaccessData( array );
  return 1;
}

