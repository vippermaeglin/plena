// SymbolObject.h: interface for the CSymbolObject class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SymbolObject_H__31C7E4DD_9B65_4556_966E_B8450CA38202__INCLUDED_)
#define AFX_SymbolObject_H__31C7E4DD_9B65_4556_966E_B8450CA38202__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "CO.h"

class CSymbolObject : public CCO  
{
public:
	void OnDoubleClick(CPoint point);
	void OnRButtonUp(CPoint point);
	void OnLButtonDown(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnMouseMove(CPoint point);
	void UpdatePoints()	;
	void SetXY();
	CRect GetRect();
	CSymbolObject(double X, double Y, long BitmapID, 
			   LPCTSTR Key, LPCTSTR Text, LPCTSTR ObjectType, CChartPanel* Owner);
	CSymbolObject(double X, double Y, LPCTSTR FileName, 
			   LPCTSTR Key, LPCTSTR Text, LPCTSTR ObjectType, CChartPanel* Owner);
	virtual ~CSymbolObject();
	void OnPaint(CDC* pDC);

	bool	dragging;
	int		buttonState;


private:
	bool	inArea(CRect rect, CPoint point);

	CValueView	m_valueView;
	bool		textDisplayed;
	int			m_bmHeight;
	int			m_bmWidth;
	int			startX;
	int			startY;
	CRect		newRect;
	CRect		oldRect;
	CRect		m_rect;

	/*	SGC	31.05.2004	BEG*/
	void	createBitmaps( CDC* pDC );

	//	I use the mask bitmap to realize the transparency of the bitmap
	//	The transparent is the color of top-left pixel of the bitmap
	//	These two bitmap are created just once
	CBitmap*	m_faceBMP;	//	Face bitmap of the object
	CBitmap*	m_maskBMP;	//	Mask bitmap of the object
	CDC			m_facedc;
	CDC			m_maskdc;
	/*	SGC	31.05.2004	END*/
public:
	void GetXYJulianDate(void);
};

#endif // !defined(AFX_SymbolObject_H__31C7E4DD_9B65_4556_966E_B8450CA38202__INCLUDED_)
