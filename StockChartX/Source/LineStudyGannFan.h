// LineStudyGannFan.h: interface for the CLineStudyGannFan class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudyGannFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
#define AFX_LineStudyGannFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"

class CStockChartXCtrl;
class CLineStudyGannFan : public CCOL  
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
	void CalculateAngles();	
	bool flagPaint;
	CLineStudyGannFan(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);	
	virtual ~CLineStudyGannFan();
private:

	bool prevUp;
	void Reset();
	std::vector<CLocation> gannFan;
	
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

#endif // !defined(AFX_LineStudyGannFan_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
