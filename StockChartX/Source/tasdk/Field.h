#if !defined(AFX_FIELD_H__0B3051AF_7A97_4BB8_A73D_2D424351B38D__INCLUDED_)
#define AFX_FIELD_H__0B3051AF_7A97_4BB8_A73D_2D424351B38D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"
#include <vector>

class CNote;
class TASDK;

class CField  
{
public:
	CField(int RecordCount, LPCTSTR Name);
	virtual ~CField();
	void setNote(CNote* NewNote);
	CNote* getNote();
	int getRecordCount();
	void setName(LPCTSTR NewFieldName);
	LPCTSTR getName();
	void setValue(int RowIndex, double Value);
	double getValue(int RowIndex);
	void setStrValue(int RowIndex, LPCTSTR Value);
	LPCTSTR getStrValue(int RowIndex);
private:
	TASDK* TASDK1;
	CString m_Name;
    int m_RecordCount;
    CNote* m_Note;    
	std::vector<LPCTSTR> strpNav;
	std::vector<double> dblpNav;

};
#endif

 // !defined(AFX_FIELD_H__0B3051AF_7A97_4BB8_A73D_2D424351B38D__INCLUDED_)
