/**
 * Title:        TASDK
 * Description:  Technical Analysis Library
 * Copyright:    Copyright (c) 1999 to 2004
 * Company:      Modulus Financial Engineering
 * @author		 Modulus FE
 * @version		 5.01
 */
  
#include "stdafx.h"
#include "Recordset.h"
// for .net use:
#include <iostream>
// for 6.0 use:
//#include <iostream.h>


CRecordset::CRecordset()
{
	FieldCount = 0;
}

CRecordset::~CRecordset()
{	
	for(int n = 0; n != FieldNav.size(); ++n){
		try{
			if(NULL != FieldNav[n]){
				delete FieldNav[n];
			}
		}
		catch(...){}
	}
	FieldNav.clear();
}

  void CRecordset::setNavigator(CNavigator* NewNavigator){
	  m_CNavigator = NewNavigator;
  }

  CNavigator* CRecordset::getNavigator(){
	  return m_CNavigator;
  }

  void CRecordset::addField(CField* NewField){
      FieldCount += 1;
	  FieldNav.resize(FieldCount + 1);
      FieldNav[FieldCount] = NewField;
  }

  void CRecordset::renameField(LPCTSTR OldFieldName, LPCTSTR NewFieldName){

    int FieldIndex;

    try{
      FieldIndex = getIndex(OldFieldName);
      FieldNav[FieldIndex]->setName(NewFieldName);
    }
    catch(...){

    }

  }

  void CRecordset::removeField(LPCTSTR FieldName){

    int n = 0;
    std::vector<CField*> NewField(FieldCount);
    for (int i = 1; i < FieldCount + 1; i ++){
      if (lstrcmp(FieldNav[i]->getName(), FieldName) != 0){
        n = n + 1;
        NewField[n] = FieldNav[i];
      }	  
    }

    FieldNav = NewField;
    FieldCount -= 1;

  }

  double CRecordset::getValue(LPCTSTR FieldName, int RowIndex){

    try{
      return FieldNav[getIndex(FieldName)]->getValue(RowIndex);
    }

    catch(...){
      return -1;
    }

  }

  void CRecordset::setValue(LPCTSTR FieldName, int RowIndex, double Value){

    try{
      FieldNav[getIndex(FieldName)]->setValue(RowIndex, Value);
    }

    catch(...){

    }

  }

  CField* CRecordset::getField(LPCTSTR FieldName){

	int index = getIndex(FieldName);

	if(index > -1){
		try{
		  return FieldNav[getIndex(FieldName)];
		}

		catch(...){
		   CField *e = new CField(0,"");
		   return e;
		}
	}
	else{
		return new CField(0,"");
	}

  }



  void CRecordset::copyField(CField* Field, LPCTSTR FieldName){

	int index = getIndex(FieldName);
	CField* src = NULL;

	if(index > -1){
		try{
		  src = FieldNav[getIndex(FieldName)];
		}
		catch(...){
		   return;
		}
		int RecordCount = src->getRecordCount();
		for(int Record = 1; Record != RecordCount + 1; ++Record){
			Field->setValue(Record, src->getValue(Record));			
		}
	}

  }


  int CRecordset::getIndex(LPCTSTR FieldName){

    int FieldIndex;
    bool found = false;

    for (FieldIndex = 1; FieldIndex < FieldCount + 1; FieldIndex++){		
        if (lstrcmp(FieldNav[FieldIndex]->getName(), FieldName) == 0){
            found = true;
            break;
        }
    }

    if (!found){
      return -1;
    }

    else{
        return FieldIndex;
    }

  }

  LPCTSTR CRecordset::getName(int FieldIndex){

    try{
      return FieldNav[FieldIndex]->getName();
    }

   catch(...){
    return "";
   }

  }

  int CRecordset::getFieldCount(){
    return FieldCount;
  }

  bool CRecordset::isValidField(LPCTSTR FieldName){
    bool retval = false;
    for (int FieldIndex = 1; FieldIndex < FieldCount + 1; FieldIndex ++){
        if (lstrcmp(FieldNav[FieldIndex]->getName(), FieldName) == 0){
            retval = true;
            break;
        }
    }
    return retval;
  }
