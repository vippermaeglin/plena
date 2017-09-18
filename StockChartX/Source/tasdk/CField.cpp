/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */
  
#include "stdafx.h"
#include "Field.h"

CField::CField(int RecordCount, LPCTSTR Name){  
  TASDK1 = new TASDK();
  m_Name = Name;
  m_RecordCount = RecordCount;
  m_Note = new CNote();
  strpNav.resize(RecordCount + 2);
  dblpNav.resize(RecordCount + 2);
}

CField::~CField(){
	delete m_Note;
	delete TASDK1;
	strpNav.clear();
	dblpNav.clear();	
}

  void CField::setNote(CNote* NewNote){
    m_Note = NewNote;
  }

  CNote* CField::getNote(){
    return m_Note;
  }

  int CField::getRecordCount(){
    return m_RecordCount;
  }

  void CField::setName(LPCTSTR NewFieldName){
    m_Name = NewFieldName;
  }

  LPCTSTR CField::getName(){
    return m_Name;
  }

  void CField::setValue(int RowIndex, double Value){

    try{
	  dblpNav[RowIndex] = Value;
    }
    catch(...){
    }

  }

  double CField::getValue(int RowIndex){

    try{
		double ret = dblpNav[RowIndex];
		if(ret > 1E+100 || ret < -1E+100) ret = NULL_VALUE;
		return ret;
    }
    catch(...){
      return -1;
    }

  }

  void CField::setStrValue(int RowIndex, LPCTSTR Value){

    try{
	  strpNav[RowIndex] = Value;
    }
    catch(...){

    }

  }

  LPCTSTR CField::getStrValue(int RowIndex) {

    try{
	  return strpNav[RowIndex];
    }
    catch(...){
      return "";
    }

  }