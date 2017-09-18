// ChartPanel.h: interface for the CChartPanel class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CHARTPANEL_H__DC9CAD73_B89A_477B_AF71_5796509151C0__INCLUDED_)
#define AFX_CHARTPANEL_H__DC9CAD73_B89A_477B_AF71_5796509151C0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "COL.h"
#include "Candle.h"

class CTextArea;
class CSeries;
class CCO;
class CCOL;
class CStockChartXCtrl;

class CChartPanel  
{
public:
	long yOffset;
	CRect panelRect;
	CRect yScaleRect;
	CSeries* pYScaleOwner;
	bool staticScale;
	void RemoveHorzLine(double value);
	void AddHorzLine(double value);
	bool buildingChart;
	void OnMessage(LPCTSTR MsgGuid, int MsgID);
	void OnDoubleClick(CPoint point);
	CStockChartXCtrl* pCtrl;
	bool connected;
	void Connect(CStockChartXCtrl* pCtrl);
	void OnRButtonDown(CPoint point);
	int FindByName(LPCTSTR szName);
	void OnPaintXOR(CDC* pDC);
	int GetMaxSlaveSize();
	int GetMinSlaveSize();	
	double W(const double x);
	void Update();
	void OnMouseMove(CPoint point);
	double GetY(double value);
	double GetX(int period, bool offscreen = false);
	double GetReverseY(double value);
	double GetReverseX(double value);
	void GetMaxMin();
	void Invalidate();
	bool invalidated;
	int index;
	void OnLButtonDown(CPoint point);
	void OnRButtonUp(CPoint point);
	void OnLButtonUp(CPoint point);
	void OnUpdate();
	void InvalidateXOR();
	int deleteSeries(LPCTSTR name);
	CSeries* GetSeries(LPCTSTR name);
	long GetSeriesIndex(LPCTSTR name);
	double slmax;
	double slmin;
	double max;
	double min;
	double y2;
	double y1;
	bool visible;
	bool hasPrice;
	CRect DrawYScale(CDC* pDC);
	void OnPaint(CDC *pDC);
	CChartPanel();
	virtual ~CChartPanel();
	std::vector <CSeries*> series;
	std::vector <CCO*> objects;
	std::vector <CCOL*> lines;
	std::vector <CTextArea*> textAreas;
	std::vector <double> horizontalLines;

	

private:
	int GridScale (double XMin, double XMax, int N, double* SMin, double* Step);
	double Normalize(double value);
	double UnNormalize(double value);	
};

#endif // !defined(AFX_CHARTPANEL_H__DC9CAD73_B89A_477B_AF71_5796509151C0__INCLUDED_)
