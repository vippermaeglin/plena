/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */

#include "stdafx.h"
#include "Navigator.h"

CNavigator::CNavigator()
{

}

CNavigator::~CNavigator()
{

}

  void CNavigator::setRecordset(CRecordset* Recordset){
	  m_Recordset = Recordset;
	  LPCTSTR name = Recordset->getName(1);
	  m_RecordCount = Recordset->getField(name)->getRecordCount();
	  m_Index = 1;
  }

  CRecordset* CNavigator::getRecordset(){
	  return m_Recordset;
  }

  int CNavigator::getRecordCount(){
    return m_RecordCount;
  }

  void CNavigator::setPosition(int Index){
    if (Index > 0){
        m_Index = Index;
    }
  }

  int CNavigator::getPosition(){
    return m_Index;
  }

  void CNavigator::MoveNext(){
    setPosition(getPosition() + 1);
  }

  void CNavigator::MovePrevious(){
    setPosition(getPosition() - 1);
  }

  void CNavigator::MoveFirst(){
    setPosition(1);
  }

  void CNavigator::MoveLast(){
	  setPosition(getRecordCount());
  }
