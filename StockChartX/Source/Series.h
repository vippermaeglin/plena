// Series.h: interface for the CSeries class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SERIES_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
#define AFX_SERIES_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "StockChartXCtl.h"
#include "Candle.h"

class CChartPanel;

class CSeries
{
public:
	//TWIN
	OLE_COLOR lineColorSignal;
	int lineStyleSignal;
	int lineWeightSignal;
	bool dataError; //When isn't possible to calculate serie
	bool isTwin;
	OLE_COLOR upColor; // May overide CStockChartXCtrl.UpColor/DownColor
	OLE_COLOR downColor;
	OLE_COLOR wickUpColor; // May overide CStockChartXCtrl.wickUpColor/wickDownColor
	OLE_COLOR wickDownColor;		
	bool IsOdd(int value);
	CSeries* GetSeriesOHLCV(LPCTSTR OHLCV);
	CSeries* GetSeries(LPCTSTR Series);
	bool calculated;
	UINT paramTypes[10];
	CString paramDefs[10];
	std::vector<double> paramDbl;
	std::vector<short> paramInt;
	std::vector<CString> paramStr;
	virtual BOOL Calculate();
	CString linkTo;
	bool recycleFlag;
	bool showDialog;
	virtual void OnRButtonUp(CPoint point);
	virtual void OnDoubleClick(CPoint point);
	bool connected;
	virtual void Connect(CStockChartXCtrl *Ctrl);
	virtual void OnRButtonDown(CPoint point);
	int memberCount;
	virtual void OnPaintXOR(CDC* pDC);
	virtual void OnMouseMove(CPoint point);
	virtual void OnLButtonUp(CPoint point);
	virtual void OnLButtonDown(CPoint point);
	void Update();
	virtual void OnPaint(CDC* pDC);
	double GetY(double value);
	double GetReverseY(double value);
	bool shareScale;
	void UpdateSlave();
	void Clear();
	void AppendValue(double jdate, double value);
	void AppendValueAsTick(double jdate, double value);
	double slmax;
	double slmin;
	double max;
	double min;
	int index;
	void EditJDate(long index, double jdate);
	void EditValue(long index, double value);
	void EditValue(double jdate, double value);
	int GetRecordCount();
	double GetMasterRecordIndex(double jdate);
	double GetJDate(int index);
	double GetValue(int index);
	double GetValue(double jdate);
	bool userParams;
	bool selected;
	bool selectable;
	bool seriesVisible;
	bool isDummy;
	int seriesType;
	int indicatorType;
	CString szName;
	CString szTitle;
	CChartPanel* ownerPanel;
	CSeries(LPCTSTR name, int type, int members, CChartPanel* owner);
	CSeries::CSeries();
	std::vector<SeriesPoint> data_master;
	std::vector<SeriesPoint> data_slave;
	OLE_COLOR lineColor;
	int lineWeight;
	int lineStyle;
	virtual ~CSeries();
	void Initialize();
	CSeries* pHigh;
	CSeries* pLow;
	CSeries* pOpen;
	CSeries* pVolume;
	CSeries* pClose;
	void GetMaxMin();

	
private:	
	CStockChartXCtrl* pCtrl;
	void GetAbsMaxMin(CSeries* pSeries = NULL);
	double Normalize(double value);
	double UnNormalize(double value);


protected:
	int nSavedDC;
	void ExcludeRects(CDC* pDC);
	void IncludeRects(CDC* pDC);

};



#endif // !defined(AFX_SERIES_H__E5248285_966F_4CEC_BE4D_78699034A3B4__INCLUDED_)
