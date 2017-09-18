// LineStandard.h: interface for the CLineStandard class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINESTANDARD_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LINESTANDARD_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"
//#include "StdAfx.h"

class CStockChartXCtrl;

class CLineStandard : public CCOL  
{
public:	
	void SnapLine();
	void OnDoubleClick(CPoint point);
	void XORDraw(UINT nFlags, CPoint point);
	void OnMessage(LPCTSTR MsgGuid, int MsgID);
	CValueView m_valueView;
	void OnRButtonUp(CPoint point);
	void OnLButtonDown(CPoint point);
	void OnMouseMove(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnPaint(CDC* pDC);
	void OnPaintXOR(CDC* pDC);
	void InvalidateXOR();
	void DrawLineStudy(CDC* pDC, CRect rect);
	void DrawLineStudy(CDC* pDC, CRectF rect);
	void DrawLineStudy(CDC* pDC, CPoint p1, CPoint p2);

	// New Atributes Defined
	bool isLeftExtension();
	bool isRightExtension();
	bool isRadiusExtension();
	void setLeftExtension(bool);
	void setRightExtension(bool);
	void setRadiusExtension(bool);
	
	CLineStandard(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner, bool radiusExtensionParameter = false);
	virtual ~CLineStandard();
private:
	bool complete;
	void DisplayInfo();
	bool displayed;
	bool dragging;
	bool xor;
	bool bReset;
	bool moveCtr;
	bool moveX1;
	bool moveX2;
	bool flipped;

	// new Atributes
	/*bool leftExtension;
	bool rightExtension;
	bool radiusExtension;*/

	int buttonState;
	double startX;
	double startY;	
	CRectF fixedLine;
	CStockChartXCtrl* pCtrl;
	long drawNumber;

};

#endif // !defined(AFX_LINESTANDARD_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
