// LineStudyFibonacciFan.h: interface for the CLineStudyFibonacciFan class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudyFibonacciFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LineStudyFibonacciFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"

class CStockChartXCtrl;
class CLineStudyFibonacciFan : public CCOL  
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
	CLineStudyFibonacciFan(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);	
	virtual ~CLineStudyFibonacciFan();
private:
	int x1_2;
	int x2_2;
	int y1_2;
	int y2_2;
	int x1_3;
	int x2_3;
	int y1_3;
	int y2_3;
	int x1_4;
	int x2_4;
	int y1_4;
	int y2_4;
	bool IsObjectClicked();
	void DrawLineStudy(CDC *pDC, CRect rect);
	void DisplayInfo();
	CString objectDescription;
	bool displayed;
	bool dragging;
	bool xor;	
	bool moveCtr;	
	int buttonState;
	int startX;
	int startY;
	CORNER m_Move;
	CRect newRect;
	CRect oldRect;
	CStockChartXCtrl* pCtrl;

};

#endif // !defined(AFX_LineStudyFibonacciFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
