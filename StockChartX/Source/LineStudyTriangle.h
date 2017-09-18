// LineStudyTriangle.h: interface for the CLineStudyTriangle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudyTriangle_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LineStudyTriangle_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

 
#include "COL.h"


class CStockChartXCtrl;
class CLineStudyTriangle : public CCOL  
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
	CLineStudyTriangle(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);
	virtual ~CLineStudyTriangle();
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
	int buttonState;
	double startX;
	double startY;
	CPoint MovePolar(double x, double y, double radius, double theta);
	CRectF newRect;
	CRectF oldRect;
	CRectF fixedLine;	
	CStockChartXCtrl* pCtrl;

};

#endif // !defined(AFX_LineStudyTriangle_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)

