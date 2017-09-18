// Navigator.h: interface for the CNavigator class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CNavigator_H__E1A755A3_FAA2_4CE6_B5B4_5BE980071DE5__INCLUDED_)
#define AFX_CNavigator_H__E1A755A3_FAA2_4CE6_B5B4_5BE980071DE5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"

class CField;
class CRecordset;

class CNavigator  
{
public:
	CNavigator();
	virtual ~CNavigator();
	void setRecordset(CRecordset* Recordset);
	CRecordset* getRecordset();
	int getRecordCount();
	void setPosition(int Index);
	int getPosition();
	void MoveNext();
	void MovePrevious();
	void MoveFirst();
	void MoveLast();
private:
	CField* m_Date;
	CField* m_Open;
	CField* m_High;
	CField* m_Low;
	CField* m_Close;
	CField* m_Volume;
	CRecordset* m_Recordset;
	int m_Index;
	int m_RecordCount;

};

#endif // !defined(AFX_CNavigator_H__E1A755A3_FAA2_4CE6_B5B4_5BE980071DE5__INCLUDED_)
