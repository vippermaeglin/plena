// COL.h: interface for the CCOL class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_COL_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_)
#define AFX_COL_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_

#include "ValueView.h"	// Added by ClassView


#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "StdAfx.h"

#include "PointF.h"
#include "RectF.h"

class CStockChartXCtrl;
class CChartPanel;

typedef /* [helpstring][uuid] */ 
enum eCorner{
	cnNone,
	cnMoveAll,
	cnTopLeft,
	cnTopRight,
	cnBottomLeft,
	cnBottomRight
}CORNER;

// Enums for periodicity
enum ePeriodicityType{ // Currently not implimented
		Secondly=1,
		Minutely,
		Hourly,
		Daily,
		Weekly,
		Month,
		Year
}PeriodicityType;

class CCOL
{
public:	
	int fillStyle;
	CRGB GetRGB(int color);
	void FillTranslucent(int color, double opacity, CDC* pDC, CRect rect);	
	std::vector<double> params;
	void SnapLine();
	virtual void OnRButtonUp(CPoint point);
	bool drawn;
	bool drawing;
	virtual void XORDraw(UINT nFlags, CPoint point);
	CString key;
	CString guid;
	virtual void OnMessage(LPCTSTR MsgGuid, int MsgID);
	virtual void OnDoubleClick(CPoint point);
	int GetAngle();
	void Update();
	void Reset();
	double RealPointX(double pointX);
	double RealPointY(double pointY);
	double MagneticPointY(double pointY, double pointX);
	double MagneticPointYValue(double pointY, double pointX);
	void Initialize();
	bool IsClicked(double x, double y, double cx, double cy);
	bool IsPointsClicked(std::vector<double> pxValues,std::vector<double> pyValues);
	bool IsRegionClicked(); // See note at function declaration
	virtual void ExcludeRects(CDC* pDC);
	virtual void IncludeRects(CDC* pDC);
	virtual void Connect(CStockChartXCtrl *Ctrl);
	virtual void OnMouseMove(CPoint point);
	virtual void OnLButtonUp(CPoint point);
	virtual void OnLButtonDown(CPoint point);
	virtual void OnRButtonDown(CPoint point);
	virtual void OnPaint(CDC* pDC);
	virtual void OnPaintXOR(CDC* pDC);
	virtual void InvalidateXOR();
	virtual void DrawRect(CDC* pDC, CRect rect);
	int nSavedDC;
	bool connected;
	bool vFixed;
	bool hFixed;
	bool isFirstPointArrow;
	CRgn m_testRgn1; // For region bounds check
	CRgn m_testRgn2;
	double x1; // Actual pixels
	double x2;
	double y1;
	double y2;
	double x1_2; // Channel second line variables
	double x2_2;
	double y1_2;
	double y2_2;
	double x1Value; // Chart values
	double x2Value;
	double y1Value;
	double y2Value;
	double x1Value_2;
	double x2Value_2;
	double y1Value_2;
	double y2Value_2;
	double valuePosition;
	std::vector<double> xValues;
	std::vector<double> xJDates;
	std::vector<double> yValues;
	double x1JDate;
	double x2JDate;
	double x1JDate_2;
	double x2JDate_2;
	//Extension points
	CPointF leftPoint; //left extension point
	CPointF rightPoint; //right extension point
	CPointF leftPoint_2; //left extension point
	CPointF rightPoint_2; //right extension point
	int upOrDown;
	CString objectType;
	int nType;
	int lineWeight;
	int lineStyle;
	int zOrder;
	CChartPanel* ownerPanel;	
	bool selected;
	bool selectable;
	bool pointOutState;
	int  pointOutStep;
	
	CRectF newRect;
	CRectF oldRect;

	// Atributes for extension flags
	int state; // State Machine to complete draw behavior
	bool leftExtension;
	bool rightExtension;
	bool radiusExtension;
	OLE_COLOR lineColor;
	CCOL();
	CCOL(OLE_COLOR color, CChartPanel* owner);
	virtual ~CCOL();
	CStockChartXCtrl* pCtrl;	
	// Set default values for study
	void SetUserDefault(void);
};

#endif // !defined(AFX_COL_H__1C5B8C95_F2A0_424C_9CAB_CCF5E76B642E__INCLUDED_)
