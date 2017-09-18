// TextArea.h: interface for the CTextArea class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_TEXTAREA_H__3BB5E45C_94B6_4160_9DAD_B2DB66CD7700__INCLUDED_)
#define AFX_TEXTAREA_H__3BB5E45C_94B6_4160_9DAD_B2DB66CD7700__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CChartPanel;
class CStockChartXCtrl;

class CTextArea  
{
public:
	int zOrder;
	void OnDoubleClick(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnRButtonUp(CPoint point);
	void OnLButtonDown(CPoint point);
	void Update();
	void Reset();
	void OnMouseMove(CPoint point);
	void OnPaint(CDC* pDC);
	void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	int x1;
	int y1;
	int x2;
	int y2;
	double x1Value; // Chart values
	double x2Value;
	double y1Value;
	double y2Value;
	double x1JDate;
	double x2JDate;
	bool gotUserInput;
	bool typing;
	bool cached;
	bool bReset;
	bool dragging;
	bool selected;
	bool selectable;
	int buttonState;
	int startX;
	int startY;
	int fontSize;
	CRect newRect;
	CRect oldRect;
	CRect m_rect;
	CString Text;
	OLE_COLOR fontColor;	
	CDC cacheDC;
	CChartPanel* ownerPanel;
	CBitmap cache_bitmap;
	CString key;
	CTextArea();
	CTextArea(int X, int Y, CChartPanel* owner);
	virtual ~CTextArea();
private:
	CStockChartXCtrl* pCtrl;
	CBitmap* oldBmp;
};

#endif // !defined(AFX_TEXTAREA_H__3BB5E45C_94B6_4160_9DAD_B2DB66CD7700__INCLUDED_)
