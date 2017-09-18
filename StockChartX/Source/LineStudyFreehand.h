// LineStudyFreehand.h: interface for the CLineStudyFreehand class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudyFreehand_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LineStudyFreehand_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

 
#include "COL.h"


class CStockChartXCtrl;
class CLineStudyFreehand : public CCOL  
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
	CLineStudyFreehand(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);	
	virtual ~CLineStudyFreehand();
private:
	bool IsDrawingClicked(CPoint point);
	void DisplayInfo();
	bool displayed;
	bool dragging;
	bool xor;	
	bool moveCtr;	
	int buttonState;
	int savedX;
	int savedY;
	int startX;
	int startY;
	int prevX;
	int prevY;
	int maxX;
	int maxY;
	int minX;
	int minY;
	std::vector<CPoint> points;
	CORNER m_Move;
	CRect newRect;
	CRect oldRect;
	CStockChartXCtrl* pCtrl;

};

#endif // !defined(AFX_LineStudyFreehand_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
