// LineStudyFibonacciProgression.h: interface for the CLineStudyFibonacciProgression class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LineStudyFibonacciProgression_H)
#define AFX_LineStudyFibonacciProgression_H 

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"

class CStockChartXCtrl;
class CLineStudyFibonacciProgression : public CCOL  
{
public:
	//void Reset();
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
	CLineStudyFibonacciProgression(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner);	
	virtual ~CLineStudyFibonacciProgression();
private:
	std::vector<CLocation> fibLines;
	std::vector<double> fib;
	bool IsObjectClicked();
	void DrawLineStudy(CDC* pDC, CRect rect);
	void DrawSimpleLine(CDC* pDC, CRect rect);
	CString objectDescription;
	void DisplayInfo();
	bool displayed;
	bool dragging;
	bool xor;	
	bool moveCtr; // move the entire progression element
	bool move1L; //move the first point of the progression line
	bool move2L; //move the second point of the progression line
	bool moveAll; //move entire object

	int buttonState;
	int startX;
	int startY;
	int secondX;
	int secondY;
	CORNER m_Move;
	CRect newRect;
	CRect oldRect;
	CStockChartXCtrl* pCtrl;

	//LineStudyFibonacci Progression add some variables
	//double theta;
	//double distance;
	CRect newRect_2;
	CRect oldRect_2;	
	CRect newRect_3;
	CRect oldRect_3;	
	// State 0 -> Initial State
	// State 1 -> Drawing first line
	// State 2 -> Drawomg second line
	// State 3 -> Drawn 


};

#endif // !defined(AFX_LineStudyFibonacciProgression_H__7A7D922E_6014_4BA3_A6D9_1078DD4E8FDF__INCLUDED_)
