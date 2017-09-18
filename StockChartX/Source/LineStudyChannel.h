// LineStandard.h: interface for the CLineStandard class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINESTUDYCHANNEL_H)
#define AFX_LINESTUDYCHANNEL_H

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"
#include "StdAfx.h";


class CStockChartXCtrl;

class CLineStudyChannel : public CCOL  
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
	void DrawLineStudy(CDC* pDC, CRectF rect);

	// New Atributes Defined
	bool isLeftExtension();
	bool isRightExtension();
	bool isRadiusExtension();
	void setLeftExtension(bool);
	void setRightExtension(bool);
	void setRadiusExtension(bool);
	
	CLineStudyChannel(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner, bool radiusExtensionParameter = false);
	virtual ~CLineStudyChannel();
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

	int buttonState;
	double startX;
	double startY;	
	CRectF newRect;
	CRectF oldRect;
	CRectF fixedLine;
	CStockChartXCtrl* pCtrl;
	//Extension points
	int lx, ly; //left extension point
	int rx, ry; //right extension point
	long drawNumber;


	//LineStudyChannel add some variables
	double theta;
	double distance;
	CRectF newRect_2;
	CRectF oldRect_2;	
	// State 0 -> Initial State
	// State 1 -> Drawing first line
	// State 2 -> Drawomg second line
	// State 3 -> Drawn 
	bool moveDistX1;
	bool moveDistX2;


	

		

};

#endif // !defined(AFX_LINESTUDYCHANNEL)