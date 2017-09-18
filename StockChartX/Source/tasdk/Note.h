#if !defined(AFX_NOTE_H__C6ED2BA3_80EF_46A2_8FB4_29D18B9FD487__INCLUDED_)
#define AFX_NOTE_H__C6ED2BA3_80EF_46A2_8FB4_29D18B9FD487__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "TASDK.h"
#include <windows.h>

class CNote  
{
public:
	CNote();
	virtual ~CNote();
	void setPeriod(int Period);
	void setValue(double Value);
	void setNote(LPCTSTR Note);
	int getPeriod();
	double getValue();
	LPCTSTR getNote();

private:
	int m_Period;
	double m_Value;
	LPCTSTR m_Note;

};

#endif // !defined(AFX_NOTE_H__C6ED2BA3_80EF_46A2_8FB4_29D18B9FD487__INCLUDED_)
