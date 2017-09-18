// ValueView.h: interface for the CValueView class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VALUEVIEW_H__88D132CE_012C_435E_B2B9_F26F7842F32C__INCLUDED_)
#define AFX_VALUEVIEW_H__88D132CE_012C_435E_B2B9_F26F7842F32C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "CO.h"

class CValueView : public CCO
{
public:
	bool	overidePoints;
	bool	okTrendline;
	CString	Text;
	
	int DaysBetweenDates(double x1Value, double x2Value);

	CRect	GetRect();
	void	OnPaint(CDC* pDC);
	
	CValueView();
	virtual ~CValueView();
private:
	CString values;
	int nTotalHeight;
	int nTotalWidth;
	bool okToDraw;

	/*	SGC	32.05.2004	BEG*/
public:
	void	Reset( bool updateRect );
	void	Show();
	void	Hide();
	void	getBackground( CDC* pDC );

	CBitmap	m_cacheBMP;
	CDC		m_cacheDC;
	/*	SGC	32.05.2004	END*/
};

#endif // !defined(AFX_VALUEVIEW_H__88D132CE_012C_435E_B2B9_F26F7842F32C__INCLUDED_)
