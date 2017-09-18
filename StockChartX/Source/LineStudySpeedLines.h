// LineStudySpeedLines.h: interface for the CLineStudySpeedLines class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudySpeedLines_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LineStudySpeedLines_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"

class CStockChartXCtrl;
class CLineStudySpeedLines : public CCOL  
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
	CLineStudySpeedLines(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);	
	virtual ~CLineStudySpeedLines();
private:
	double x1_2;
	double x2_2;
	double y1_2;
	double y2_2;
	double x1_3;
	double x2_3;
	double y1_3;
	double y2_3;
	double x1_4;
	double x2_4;
	double y1_4;
	double y2_4;
	bool IsObjectClicked();
	void DrawLineStudy(CDC *pDC, CRectF rect);
	void DisplayInfo();
	CString objectDescription;
	bool displayed;
	bool dragging;
	bool xor;	
	bool moveCtr;	
	int buttonState;
	double startX;
	double startY;
	CORNER m_Move;
	CRectF newRect;
	CRectF oldRect;
	CStockChartXCtrl* pCtrl;

};

#endif // !defined(AFX_LineStudySpeedLines_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
