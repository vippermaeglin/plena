// Recordset.h: interface for the Recordset class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_RECORDSET_H__FDCB371D_CC2C_4572_B099_F12C6EC6627A__INCLUDED_)
#define AFX_RECORDSET_H__FDCB371D_CC2C_4572_B099_F12C6EC6627A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"
#include <vector>

class CField;
class CNavigator;
class TASDK;

class CRecordset 
{
public:
	CRecordset();
	virtual ~CRecordset();
	void setNavigator(CNavigator* NewNavigator);
	CNavigator* getNavigator();
	void addField(CField* NewField);
	void renameField(LPCTSTR OldFieldName, LPCTSTR NewFieldName);
	void removeField(LPCTSTR FieldName);
	double getValue(LPCTSTR FieldName, int RowIndex);
	void setValue(LPCTSTR FieldName, int RowIndex, double Value);
	CField* getField(LPCTSTR FieldName);
	void copyField(CField* Field, LPCTSTR FieldName);
	int getIndex(LPCTSTR FieldName);
	LPCTSTR getName(int FieldIndex);
	int getFieldCount();
	bool isValidField(LPCTSTR FieldName);

private:	
	std::vector<CField*> FieldNav;
	int FieldCount;
	CNavigator* m_CNavigator;

};

#endif // !defined(AFX_RECORDSET_H__FDCB371D_CC2C_4572_B099_F12C6EC6627A__INCLUDED_)
