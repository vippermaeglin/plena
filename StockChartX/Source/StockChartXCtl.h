#if !defined(AFX_STOCKCHARTXCTL_H__9967D72A_3714_4A67_8185_E0A74FF03C38__INCLUDED_)
#define AFX_STOCKCHARTXCTL_H__9967D72A_3714_4A67_8185_E0A74FF03C38__INCLUDED_

// StockChartX PRO version


#include "StdAfx.h"

#include "Candle.h"
#include "PDCHandler.h"
#include <string>
#include <fstream>
#include <afxinet.h>



#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// StockChartXCtl.h : Declaration of the CStockChartXCtrl ActiveX Control class.

/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl : See StockChartXCtl.cpp for implementation.

//6/2/05
#define PALVERSION 0x300
#define HDIB HANDLE
#define IS_WIN30_DIB(lpbi)  ((*(LPDWORD)(lpbi)) == sizeof(BITMAPINFOHEADER))
#define WIDTHBYTES(bits)    (((bits) + 31) / 32 * 4)
#define DIB_HEADER_MARKER   ((WORD) ('M' << 8) | 'B')

/* Error constants for bitmap saving */
enum {
      ERR_MIN = 0,                     // All error #s >= this value
      ERR_NOT_DIB = 0,                 // Tried to load a file, NOT a DIB!
      ERR_MEMORY,                      // Not enough memory!
      ERR_READ,                        // Error reading file!
      ERR_LOCK,                        // Error on a GlobalLock()!
      ERR_OPEN,                        // Error opening a file!
      ERR_FILENOTFOUND,                // Error opening file in GetDib()
      ERR_INVALIDHANDLE,               // Invalid Handle    
     };


#include "ValueView.h"
#include "CalendarPanel.h"

class CCO;
class CCOL;
class CValueView;
class CCalendarPanel;
class CChartPanel;
class CSeries;
class CLocation;

class CStockChartXSerializer;
class CStockChartXSerializer_General;

//class UserStudyLine;

// SeriesPoint Moved from CSeries.cpp to support 
// access to price style values - 8/13/04 RDG
struct SeriesPoint{
	double jdate; // X
	double value; // Y
};

struct TrendLineWatch{ // Identifier for a trend line watch
	CString TrendLineName;
	CString SeriesName;
	bool WasSelectable; // For reset when removed from watch
};

struct UserStudyLine{
	//Common:
	CString SerieName;
	CString objectType;
    CString key;
	double x1;
	double y1;
	double x2;
	double y2;
	double x1Value;
	double x2Value;
	double y1Value;
	double y2Value;
	double x1JDate;
	double x2JDate;
    bool selectable;
    bool selected;
    bool zOrder;
    long lineStyle;
    long lineWeight;
	//Lines:
    OLE_COLOR lineColor;
	double x1Value_2;
    double y1Value_2;
    double x2Value_2;
    double y2Value_2;
	double x1JDate_2;
	double x2JDate_2;
	std::vector<double> xValues;
	std::vector<double> xJDates;
	std::vector<double> yValues;
	long fillStyle;
	bool vFixed;
	bool hFixed;
    bool isFirstPointArrow;
	bool radiusExtension;
	bool rightExtension;
	bool leftExtension;
	double valuePosition;
	int upOrDown;	
	std::vector<double> params;
	//Text:
	double startX;
	double startY;
	int fontSize;
	CString Text;
	OLE_COLOR fontColor;
	//Object Symbol:	
	CString fileName;
	long	bitmapID;
	OLE_COLOR	backColor;
	OLE_COLOR	foreColor;
	CString text;
	int nType;
	bool typing;
	bool gotUserInput;
};

typedef /* [helpstring][uuid] */ 
enum eFileOpType{ // Currently not implimented
		opSaveAll,
		opLoadAll,
		opSaveGeneral,
		opLoadGeneral,
		opSaveObjects,
		opLoadObjects
}FileOpType;

// Enums for periodicity
/*enum ePeriodicityType{ // Currently not implimented
		Secondly=1,
		Minutely,
		Hourly,
		Daily,
		Weekly,
		Month,
		Year
}PeriodicityType;*/

class CStockChartXCtrl : public COleControl
{
	DECLARE_DYNCREATE(CStockChartXCtrl)

// Constructor
public:
	
	//3/1/08 ER-start
	//ombjects used for zooming drawing
	CDC *zoomingDC;
	CBrush zoomingBrush;
	//1 Mar 2008 ER-end

	CString SaveImageTitle();

	long GetX(long record);

	long GetPriceLineThickness();
	
	long GetPriceLineThicknessBar();

	bool GetPriceLineMono();

	bool m_frozen;

	bool IgnorePaint;
	
	long CStockChartXCtrl::GetMousePositionY();

	bool dialogErrorShown;

	void NotifyDialogShown();

	bool IsLicenseValid();

	CSeries* GetSeriesByName(LPCTSTR szName);
	CCOL* GetTrendLine(LPCSTR Key);
	void WatchTrendLines(LPCTSTR SeriesName);
	std::vector<TrendLineWatch> trendWatch;

	double nSpacing;

	CString version;

	CString m_volumePostfix;

	void FireCustomIndicatorEventNeedData(LPCTSTR IndicatorName);

	CSeries* GetSeriesByNameType(LPCTSTR szName,long lType);
	
	friend class CStockChartXSerializer;
	friend class CStockChartXSerializer_General;

	OLE_COLOR wickColor;

	int themeOffset;

	int m_currentPanel;

	void ForceRePaint();

	CString textAreaFontName;
	long textAreaFontSize;
	double yScaleMinTick;

	long Save(LPCTSTR fileName, FileOpType Operation = opSaveAll);
	long Load(LPCTSTR fileName, FileOpType Operation = opLoadAll);

	void OnUserDrawingComplete(long StudyType, LPCTSTR Name);

	double barStartTime;	

	bool m_yScaleDrawn;

	bool typing;

	OLE_COLOR candleDownOutlineColor;
	OLE_COLOR candleUpOutlineColor;

	long swapTo;

	bool m_onRClickFired;

	OLE_COLOR backGradientTop;
	OLE_COLOR backGradientBottom;

	OLE_COLOR valueViewGradientTop;
	OLE_COLOR valueViewGradientBottom;
	
	long _SaveFile(LPCTSTR FileName);
	long _LoadFile(LPCTSTR FileName);	
	bool updatingIndicator;
	bool displayInfoText;
	long m_barInterval;
	CLocation GetSetChartObjectLocation(long ObjectType, LPCTSTR Key);
	CSeries* styleSeries;
	std::vector<SeriesPoint> m_psValues1;
	std::vector<SeriesPoint> m_psValues2;
	std::vector<SeriesPoint> m_psValues3;
	bool xGridDrawn;
	std::vector<int> xGridMap;
	void ChangeYScale(long Panel, double Max, double Min);
	double GetMin(LPCTSTR Series, bool IgnoreZero = false, bool OnlyVisible = false);
	double GetMax(LPCTSTR Series, bool IgnoreZero = false, bool OnlyVisible = false);
	void FadeVert(CDC* pDC, COLORREF Color1, COLORREF Color2, CRect boxRect);
	void FadeHorz(CDC* pDC, COLORREF Color1, COLORREF Color2, CRect boxRect);
	long xCount;
	std::vector<double> xMap;
	std::vector<double> dateMap;
	long priceStyle;	
	std::vector<double> priceStyleParams;
	bool threeDStyle;	
	void InvalidateIndicators();
	void SendSeriesMessage(UINT msg, LPCSTR series);
	bool locked;
	void ShowIndDlg(LPCTSTR key);
	bool CompareNoCase(CString string1, CString string2);
	void AppendNewValue(LPCTSTR Name, double JDate, double Value);
	long AddNewSeries(LPCTSTR Name, long Type, long Panel);
	void ShowHtmlHelp(CString chmFileName, long helpTopic);	
	long GetPanelByName(LPCTSTR Series);
	void ThrowErr(long number, LPCTSTR description);
	int barWidth;
	int yOffset;
	bool displayTitles;
	bool recordLabels;
	OLE_COLOR horzLineColor;
	bool m_horzLines;
	double darvasPct;
	bool darvasBoxes;
	bool useLineSeriesColors;
	bool useVolumeUpDownColors;
	bool buildingChart;
	bool reCalc;
	
	VARIANT_BOOL m_Magnetic;
	bool suspendCrossHair;

	void LogEvent(CString text);
	HANDLE BitmapToDIB(HBITMAP hBitmap, HPALETTE hPAL);
	WORD SaveDIB(HDIB hDib, LPCSTR lpFileName);
	WORD PaletteSize(LPSTR lpDIB);
	WORD DIBNumColors(LPSTR lpDIB);
	HPALETTE GetSystemPalette(void);
	int PalEntriesOnDevice(HDC hDC);
	bool pause;
	bool realTime;	
	void FireOnDialogShown();
	void FireOnDialogHiden();
	void FireOnDialogCancel();
	void FireOnItemMouseMove(int type, CString name);
	void FireOnItemDoubleClick(int type, CString name);
	void FireOnItemLeftClick(int type, CString name);
	void FireOnItemRightClick(int type, CString name);
	void DelayMessage(CString guid, int msgID, long delay);
	bool textDisplayed;
	CString CreateGUID();
	int barSpacing;
	int dx;		
	void OnSelectSeries(CString name);
	OLE_COLOR ComputeInverseColor(DWORD rgb, CDC* pDC);		
	bool changed;
	void GetCtrlFocus();
	bool focus;
	CValueView* m_valueView;
	CString charttcstr(long value);
	long cstrtchart(CString value);
	CString linetcstr(long value);
	long cstrtline(CString value);
	CString scaletcstr(long value);
	long cstrtscale(CString value);
	CString rltcstr(long value);
	long cstrtrl(CString value);
	bool cstrtb(CString value);
	CString btcstr(bool value);
	CString ltcstr(long value);
	CString dtcstr(double value);
	CString m_symbol;
	bool flag;
	CChartPanel* activePanel;
	bool dragging;
	long extendedXPixels;
	int decimals;
	long yAlignment;
	bool showYGrid;
	bool showXGrid;
	long scalingType;
	OLE_COLOR upColor;
	OLE_COLOR downColor;
	OLE_COLOR foreColor;
	OLE_COLOR backColor;
	OLE_COLOR gridColor;
	OLE_COLOR wickUpColor;
	OLE_COLOR wickDownColor;
	int spaceInterval;
	int endIndex;
	int startIndex;	
	int GetBottomChartPanel();
	int GetTopChartPanel();
	int GetNextHigherChartPanel(double y);
	int GetNextLowerChartPanel(double y);
	int yScaleWidth;
	int height;	
	int width;	
	int GetVisiblePanelCount();
	std::vector<CChartPanel*> panels;

	//User default for Line Study	
	OLE_COLOR lineColor; 
	std::vector<double> fibonacciRetParams;
	std::vector<double> fibonacciProParams;

	//User control for Line Studies on panels
	std::vector<UserStudyLine> userStudyLine;
	
	CPoint lastMouse;

	void Delete(CString groupName = "");
	int GetSlaveRecordCount();
	int GetSlaveRecordCount2();
	long RecordCount();
	void KillCaret();
	void DisplayCaret(int x, int y);
	void ReleaseScreen(CDC* pDC);
	CDC* GetScreen();
	
	CString FromJDate(double jdate);
	OLE_COLOR valuePanelColor;

	int SelectCount();
	int m_buttonState;
	
	void UnSelectAll();
	std::vector<CString> swapSeries;
	std::vector<OLE_COLOR> barColors;
	CString barColorName; // bar colors "owner" (only one series can use barColors at this time)
	void RePaintXOR();

	void UpdateRect(CRect rect);
	void UpdateScreen(bool suppressErrors, long operation = 0);
	
	void RePaint();
	
	void DrawCrossHairs(CPoint point);
	void SuspendCrossHairs();
	void ResumeCrossHairs(CPoint point);
	
	void DrawMeasure(CPoint point);
	void SuspendMeasure();
	void ResumeMeasure(CPoint point);
	

  BOOL ExtraPrecision();

	int m_mouseState;
	
	// DEBUG function only
	void EmbedData(CString name, int month, int day, int year, double open, double high, double low, double close);

	void DrawTransparent(INT x,INT y,INT bitmapID,
			COLORREF crColor, CDC* pDC, LPCTSTR fileName = "");

	CString FormatDate(int month, int day, int year, 
					   int hour, int minute, int second);
	CBitmap* oldBmp;
	bool bResetMemDC;
	
	CDC		m_memDC;
	bool first;
	//Direct2D objects
	PDCHandler*  pdcHandler;
	ID2D1Factory* m_pD2DFactory;
	IDWriteFactory* pDWriteFactory;
	ID2D1DCRenderTarget* m_pDCRT;	
	ID2D1SolidColorBrush* m_Brush;
	ID2D1PathGeometry* pathGeometry;
	//GDI+ objects
	HDC m_memHDC;
	//Type of DC: 0->GDI / 1->GDI+ / 2->Direct2D
	int PDCType;

	CBitmap m_bitmap;
	
	LONG Periodicity;
	void RecycleChartPanel(int index);
	void Refresh();		
	CPoint m_point;
	double get_rand(double low_end, double high_end);
	CStockChartXCtrl();
	UINT m_Cursor;
	bool drawing;
	bool movingObject;
	bool objectSelected;
	bool resizing;
	bool m_expired;
	bool m_pauseCrossHairs;
	bool m_pauseMeasure;
	long m_recordYLine;

	//void InterruptDrawings();

	/*	SGC	32.05.2004	BEG*/
	void	drawToDC( CDC* pDC );
	void	getRect( CRect& rect )	{	GetScreen()->GetWindow()->GetClientRect( &rect );	};
	/*	SGC	32.05.2004	BEG*/


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CStockChartXCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();	
	//}}AFX_VIRTUAL

// Implementation
protected:
	~CStockChartXCtrl();

	BEGIN_OLEFACTORY(CStockChartXCtrl)        // Class factory and guid
		virtual BOOL VerifyUserLicense();
		virtual BOOL GetLicenseKey(DWORD, BSTR FAR*);
	END_OLEFACTORY(CStockChartXCtrl)

	DECLARE_OLETYPELIB(CStockChartXCtrl)      // GetTypeInfo
	//DECLARE_PROPPAGEIDS(CStockChartXCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CStockChartXCtrl)		// Type name and misc status

// Message maps
	//{{AFX_MSG(CStockChartXCtrl)
	afx_msg void OnPaint();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);	
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
	afx_msg void OnKillFocus(CWnd* pNewWnd);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

// Dispatch maps
	//{{AFX_DISPATCH(CStockChartXCtrl)				
	BOOL m_realTimeXLabels;
	afx_msg void OnRealTimeXLabelsChanged();
	long m_scalePrecision;
	afx_msg void OnScalePrecisionChanged();
	BOOL m_useLineSeriesUpDownColors;
	afx_msg void OnUseLineSeriesUpDownColorsChanged();
	CString m_iD;
	afx_msg void OnIDChanged();
	BOOL m_horizontalSeparators;
	afx_msg void OnHorizontalSeparatorsChanged();
	OLE_COLOR m_horizontalSeparatorColor;
	afx_msg void OnHorizontalSeparatorColorChanged();
	BOOL m_showRecordsForXLabels;
	afx_msg void OnShowRecordsForXLabelsChanged();
	BOOL m_displayTitles;
	afx_msg void OnDisplayTitlesChanged();
	BOOL m_displayTitleBorder;
	afx_msg void OnDisplayTitleBorderChanged();
	CString m_version;
	afx_msg void OnVersionChanged();
	BOOL m_threeDStyle;
	afx_msg void OnThreeDStyleChanged();
	BOOL m_yGrid;
	afx_msg void OnYGridChanged();
	long m_priceStyle;
	afx_msg void OnPriceStyleChanged();
	BOOL m_xGrid;
	afx_msg void OnXGridChanged();
	BOOL m_displayInfoText;
	afx_msg void OnDisplayInfoTextChanged();
	long m_maxDisplayRecords;
	afx_msg void OnMaxDisplayRecordsChanged();
	BOOL m_ignoreSeriesLengthErrors;
	afx_msg void OnIgnoreSeriesLengthErrorsChanged();
	OLE_COLOR m_valueViewGradientTop;
	afx_msg void OnValueViewGradientTopChanged();
	OLE_COLOR m_valueViewGradientBottom;
	afx_msg void OnValueViewGradientBottomChanged();
	BOOL m_crossHairs;
	afx_msg void OnCrossHairsChanged();
	OLE_COLOR m_backGradientTop;
	afx_msg void OnBackGradientTopChanged();
	OLE_COLOR m_backGradientBottom;
	afx_msg void OnBackGradientBottomChanged();
	long m_mousePointer;
	afx_msg void OnMousePointerChanged();
	OLE_COLOR m_candleDownOutlineColor;
	afx_msg void OnCandleDownOutlineColorChanged();
	OLE_COLOR m_candleUpOutlineColor;
	afx_msg void OnCandleUpOutlineColorChanged();
	BOOL m_useVolumeUpDownColors;
	afx_msg void OnUseVolumeUpDownColorsChanged();
	double m_barStartTime;
	afx_msg void OnBarStartTimeChanged();
	double m_yScaleMinTick;
	afx_msg void OnYScaleMinTickChanged();
	long m_textAreaFontSize;
	afx_msg void OnTextAreaFontSizeChanged();
	CString m_textAreaFontName;
	afx_msg void OnTextAreaFontNameChanged();
	CString m_volumePostfixLetter;
	afx_msg void OnVolumePostfixLetterChanged();
	BOOL m_extraTimePrecision;
	afx_msg void OnExtraTimePrecisionChanged();
	afx_msg OLE_COLOR GetValuePanelColor();
	afx_msg void SetValuePanelColor(OLE_COLOR nNewValue);
	afx_msg OLE_COLOR GetChartBackColor();
	afx_msg void SetChartBackColor(OLE_COLOR nNewValue);
	afx_msg OLE_COLOR GetChartForeColor();
	afx_msg void SetChartForeColor(OLE_COLOR nNewValue);
	afx_msg OLE_COLOR GetGridcolor();
	afx_msg void SetGridcolor(OLE_COLOR nNewValue);
	afx_msg long GetScaleType();
	afx_msg void SetScaleType(long nNewValue);
	afx_msg BSTR GetSymbol();
	afx_msg void SetSymbol(LPCTSTR lpszNewValue);
	afx_msg OLE_COLOR GetUpColor();
	afx_msg void SetUpColor(OLE_COLOR nNewValue);
	afx_msg OLE_COLOR GetDownColor();
	afx_msg void SetDownColor(OLE_COLOR nNewValue);
	afx_msg long GetScaleAlignment();
	afx_msg void SetScaleAlignment(long nNewValue);
	afx_msg long GetWorkspaceRight();
	afx_msg void SetWorkspaceRight(long nNewValue);
	afx_msg long GetWorkspaceLeft();
	afx_msg void SetWorkspaceLeft(long nNewValue);
	afx_msg BOOL GetChanged();
	afx_msg void SetChanged(BOOL bNewValue);
	afx_msg long GetVisibleRecordCount();
	afx_msg void SetVisibleRecordCount(long nNewValue);
	afx_msg long GetRecordCount();
	afx_msg void SetRecordCount(long nNewValue);
	afx_msg long GetFirstVisibleRecord();
	afx_msg void SetFirstVisibleRecord(long nNewValue);
	afx_msg long GetLastVisibleRecord();
	afx_msg void SetLastVisibleRecord(long nNewValue);
	afx_msg long GetSeriesCount();
	afx_msg OLE_COLOR GetLineColor();
	afx_msg void SetLineColor(OLE_COLOR nNewValue);
	afx_msg OLE_HANDLE GetHWnd();
	afx_msg long GetRightDrawingSpacePixels();
	afx_msg void SetRightDrawingSpacePixels(long nNewValue);
	afx_msg BOOL GetDarvasBoxes();
	afx_msg void SetDarvasBoxes(BOOL bNewValue);
	afx_msg double GetDarvasStopPercent();
	afx_msg void SetDarvasStopPercent(double newValue);
	afx_msg long GetBarWidth();
	afx_msg void SetBarWidth(long nNewValue);
	afx_msg long GetPanelCount();
	afx_msg long GetAlignment();
	afx_msg void SetAlignment(long nNewValue);
	afx_msg long GetBarInterval();
	afx_msg void SetBarInterval(long nNewValue);
	afx_msg BOOL GetUserEditing();
	afx_msg void SetUserEditing(BOOL bNewValue);
	afx_msg BSTR GetSelectedKey();
	afx_msg long GetSelectedType();
	afx_msg short GetCurrentPanel();
	afx_msg void EditValue(LPCTSTR Name, double JDate, double Value);
	afx_msg void ClearValues(LPCTSTR Name);
	afx_msg void Update(LONG operation = 0);
	afx_msg void ScrollLeft(long Records);
	afx_msg void ScrollRight(long Records);
	afx_msg void ZoomIn(long Records);
	afx_msg void ZoomOut(long Records);		
	afx_msg double GetValue(LPCTSTR Name, long Record);
	afx_msg double GetValueByJDate(LPCTSTR Name, double JDate);
	afx_msg double ToJulianDate(long nYear, long nMonth, long nDay, long nUHour, long nUMinute, long nUSecond);
	afx_msg BSTR FromJulianDate(double JulianDate);		
	afx_msg long AddChartPanel();		
	afx_msg void ResetZoom();			
	afx_msg void RemoveAllSeries();
	afx_msg void ZoomUserDefined();	
	afx_msg long SaveFile(LPCTSTR FileName);
	afx_msg long LoadFile(LPCTSTR FileName);
	afx_msg void ClearDrawings();
	afx_msg void ShowLastTick(LPCTSTR Series, double Value);
	afx_msg void ReDrawYScale(long Panel);
	afx_msg long GetPanelBySeriesName(LPCTSTR Series);
	afx_msg void PrintChart();		
	afx_msg void RemoveSymbolObject(LPCTSTR Key);
	afx_msg void AddSymbolObject(long Panel, double Value, long Record, long Type, LPCTSTR Key, LPCTSTR Text);	
	afx_msg void AddUserSymbolObject(long Type, LPCTSTR Key, LPCTSTR Text);
	afx_msg void AddStaticText(long Panel, LPCTSTR Text, LPCTSTR Key, OLE_COLOR Color, BOOL Selectable, long X, long Y);
	afx_msg void AddUserDefinedText(LPCTSTR Key);
	afx_msg void RemoveObject(long ObjectType, LPCTSTR Key);
	afx_msg void AddUserTrendLine(LPCTSTR Key);		
	afx_msg void SaveChartBitmap(LPCTSTR FileName);
	afx_msg double GetJDate(LPCTSTR Name, long Record);	
	afx_msg void ForcePaint();	
	afx_msg void DrawTrendLine(long Panel, double LeftValue, long LeftRecord, double RightValue, long RightRecord, LPCTSTR Key);
	afx_msg BOOL AddIndicatorSeries(long IndicatorType, LPCTSTR Key, long Panel, BOOL UserParams);
	//afx_msg BOOL AddIndicatorSMA(LPCTSTR Key, long Panel, LPCTSTR Source, long Periods, int ColorR, int ColorG, int ColorB, int Style, int Thickness);
	afx_msg void AppendValue(LPCTSTR Name, double JDate, double Value);
	afx_msg void RemoveSeries(LPCTSTR Name);	
	afx_msg long AddSeries(LPCTSTR Name, long Type, long Panel);	
	afx_msg void ShowHelp(LPCTSTR chmFileName, long helpTopic);
	//afx_msg BOOL ShowHelp(LPCTSTR Key, long Panel, LPCTSTR Source, long Periods, int ColorR, int ColorG, int ColorB, int Style, int Thickness);
	afx_msg void ShowIndicatorDialog(LPCTSTR Key);
	afx_msg void AddHorizontalLine(long Panel, double Value);
	afx_msg void RemoveHorizontalLine(long Panel, double Value);
	afx_msg void EnumIndicators();
	afx_msg void SetYScale(long Panel, double Max, double Min);
	afx_msg void ResetYScale(long Panel);
	afx_msg void EnumPriceStyles();
	afx_msg double GetMaxValue(LPCTSTR Series);	
	afx_msg void AddSymbolObjectFromFile(long Panel, double Value, long Record, LPCTSTR FileName, LPCTSTR Key, LPCTSTR Text);	
	afx_msg double GetMinValue(LPCTSTR Series);
	afx_msg long GetObjectStartRecord(long ObjectType, LPCTSTR Key);	
	afx_msg long GetObjectEndRecord(long ObjectType, LPCTSTR Key);
	afx_msg double GetObjectStartValue(long ObjectType, LPCTSTR Key);
	afx_msg double GetObjectEndValue(long ObjectType, LPCTSTR Key);
	afx_msg void AppendValueAsTick(LPCTSTR Name, double JDate, double Value);
	afx_msg long GetRecordByJDate(double JDate);
	afx_msg void SetSeriesUpDownColors(LPCTSTR Name, OLE_COLOR UpColor, OLE_COLOR DownColor);
	afx_msg void SetObjectPosition(long ObjectType, LPCTSTR Key, long StartRecord, double StartValue, long EndRecord, double EndValue);	
	afx_msg double GetYValueByPixel(long Pixel);
	afx_msg long GetIndicatorCountByType(long IndicatorType);	
	afx_msg void ResetBarColors();
	//afx_msg BOOL ResetBarColors(LPCTSTR Key, long Panel, LPCTSTR Source, int Index, long Periods, int ColorR, int ColorG, int ColorB, int Style, int Thickness);			
	afx_msg double GetPriceStyleValueByJDate2(double JDate);
	afx_msg double GetPriceStyleValue1(long Record);
	afx_msg double GetPriceStyleValue2(long Record);	
	afx_msg double GetPriceStyleValueByJDate1(double JDate);
	afx_msg double GetPriceStyleValueByJDate3(double JDate);
	afx_msg double GetPriceStyleValue3(long Record);
	afx_msg void EnumSeries();
	afx_msg void MoveSeries(LPCTSTR Name, long FromPanel, long ToPanel);
	afx_msg void EditValueByRecord(LPCTSTR Name, long Record, double Value);
	afx_msg void RecalculateIndicators();
	afx_msg void DrawLineStudy(long Type, LPCSTR Key);
	afx_msg long CrossOverRecord(long Record, LPCTSTR Series1, LPCTSTR Series2);
	afx_msg double CrossOverValue(long Record, LPCTSTR Series1, LPCTSTR Series2);
	afx_msg BOOL IsSelected(LPCTSTR Key);
	afx_msg long SaveGeneralTemplate(LPCTSTR FileName);
	afx_msg long LoadGeneralTemplate(LPCTSTR FileName);
	afx_msg long SaveObjectTemplate(LPCTSTR FileName);
	afx_msg long LoadObjectTemplate(LPCTSTR FileName);
	afx_msg long GetObjectCount(long Type);
	afx_msg void ClearAllSeries();
	afx_msg short AddCustomIndDlgIndPropStr(LPCTSTR IndicatorName, long ParamType, LPCTSTR DefValue);
	afx_msg short AddCustomIndDlgIndPropInt(LPCTSTR IndicatorName, long ParamType, long DefValue);
	afx_msg short AddCustomIndDlgIndPropDbl(LPCTSTR IndicatorName, long ParamType, double DefValue);
	afx_msg long SetCustomIndicatorData(LPCTSTR IndicatorName, const VARIANT FAR& Data, BOOL Append);
	afx_msg double GetVisibleMaxValue(LPCTSTR Series);
	afx_msg double GetVisibleMinValue(LPCTSTR Series);	
	afx_msg void AddTrendLineWatch(LPCTSTR TrendLineName, LPCTSTR SeriesName);
	afx_msg void RemoveTrendLineWatch(LPCTSTR TrendLineName, LPCTSTR SeriesName);
	afx_msg long GetIndicatorType(LPCTSTR SeriesName);	
	afx_msg void EditJDate(long Record, double JDate);
	afx_msg double ToJulianDateEx(long nYear, long nMonth, long nDay, long nUHour, long nUMinute, long nUSecond, long nUMilliSecond);
	afx_msg long GetSeriesWeight(LPCTSTR Name);
	afx_msg void SetSeriesWeight(LPCTSTR Name, long nNewValue);
	afx_msg long GetPanelY1(long Panel);
	afx_msg void SetPanelY1(long Panel, long nNewValue);
	afx_msg long GetPanelY2(long Panel);
	afx_msg void SetPanelY2(long Panel, long nNewValue);
	afx_msg OLE_COLOR GetObjectColor(long ObjectType, LPCTSTR Key);
	afx_msg void SetObjectColor(long ObjectType, LPCTSTR Key, OLE_COLOR nNewValue);
	afx_msg BOOL GetObjectSelectable(long ObjectType, LPCTSTR Key);
	afx_msg void SetObjectSelectable(long ObjectType, LPCTSTR Key, BOOL bNewValue);
	afx_msg long GetSeriesType(LPCTSTR Name);
	afx_msg void SetSeriesType(LPCTSTR Name, long nNewValue);
	afx_msg long GetSeriesStyle(LPCTSTR Name);
	afx_msg void SetSeriesStyle(LPCTSTR Name, long nNewValue);
	afx_msg BSTR GetObjectText(long ObjectType, LPCTSTR Name);
	afx_msg void SetObjectText(long ObjectType, LPCTSTR Name, LPCTSTR lpszNewValue);
	afx_msg long GetObjectWeight(long ObjectType, LPCTSTR Name);
	afx_msg void SetObjectWeight(long ObjectType, LPCTSTR Name, long nNewValue);
	afx_msg long GetObjectStyle(long ObjectType, LPCTSTR Name);
	afx_msg void SetObjectStyle(long ObjectType, LPCTSTR Name, long nNewValue);
	afx_msg BOOL GetShareScale(LPCTSTR Series);
	afx_msg void SetShareScale(LPCTSTR Series, BOOL bNewValue);
	afx_msg long GetSeriesColor(LPCTSTR Name);
	afx_msg void SetSeriesColor(LPCTSTR Name, long nNewValue);
	afx_msg OLE_COLOR GetBarColor(long Record, LPCTSTR Name);
	afx_msg void SetBarColor(long Record, LPCTSTR Name, OLE_COLOR nNewValue);
	afx_msg BSTR GetIndPropStr(LPCTSTR Key, short ParamNum);
	afx_msg void SetIndPropStr(LPCTSTR Key, short ParamNum, LPCTSTR lpszNewValue);
	afx_msg double GetIndPropDbl(LPCTSTR Key, short ParamNum);
	afx_msg void SetIndPropDbl(LPCTSTR Key, short ParamNum, double newValue);
	afx_msg short GetIndPropInt(LPCTSTR Key, short ParamNum);
	afx_msg void SetIndPropInt(LPCTSTR Key, short ParamNum, short nNewValue);
	afx_msg double GetPriceStyleParam(long Index);
	afx_msg void SetPriceStyleParam(long Index, double newValue);
	afx_msg BOOL GetSeriesVisible(LPCTSTR Name);
	afx_msg void SetSeriesVisible(LPCTSTR Name, BOOL bNewValue);
	afx_msg BSTR GetSeriesName(long Index);
	afx_msg double GetLineStudyParam(LPCTSTR Key, short ParamNum);
	afx_msg void SetLineStudyParam(LPCTSTR Key, short ParamNum, double newValue);
	afx_msg long GetObjectZOrder(LPCTSTR Key, long ObjectType);
	afx_msg void SetObjectZOrder(LPCTSTR Key, long ObjectType, long nNewValue);
	afx_msg long GetObjectFillStyle(LPCTSTR Key, long ObjectType);
	afx_msg void SetObjectFillStyle(LPCTSTR Key, long ObjectType, long nNewValue);
	afx_msg void AboutBox();
	afx_msg long GetXPixel(long Record);		
	afx_msg long GetYPixel(long Panel, double Value);
	//afx_msg CPoint GetPoint(long panel, CPointF pointValue);		
	//afx_msg CPointF GetPointValue(CPoint point);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()
	

// Event maps
	//{{AFX_EVENT(CStockChartXCtrl)
	void FireSelectSeries(LPCTSTR Name)
		{FireEvent(eventidSelectSeries,EVENT_PARAM(VTS_BSTR), Name);}
	void FireDeleteSeries(LPCTSTR Name)
		{FireEvent(eventidDeleteSeries,EVENT_PARAM(VTS_BSTR), Name);}
	void FireMouseOver(OLE_XPOS_PIXELS X, OLE_YPOS_PIXELS Y, long Record)
		{FireEvent(eventidMouseMove,EVENT_PARAM(VTS_XPOS_PIXELS  VTS_YPOS_PIXELS  VTS_I4), X, Y, Record);}
	void FirePaint(long Panel)
		{FireEvent(eventidPaint,EVENT_PARAM(VTS_I4), Panel);}
	void FireScroll()
		{FireEvent(eventidScroll,EVENT_PARAM(VTS_NONE));}
	void FireItemRightClick(long ObjectType, LPCTSTR Name, OLE_XPOS_PIXELS X, OLE_YPOS_PIXELS Y)
		{FireEvent(eventidItemRightClick,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), ObjectType, Name, X, Y);}
	void FireItemLeftClick(long ObjectType, LPCTSTR Name, OLE_XPOS_PIXELS X, OLE_YPOS_PIXELS Y)
		{FireEvent(eventidItemLeftClick,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), ObjectType, Name, X, Y);}
	void FireItemDoubleClick(long ObjectType, LPCTSTR Name, OLE_XPOS_PIXELS X, OLE_YPOS_PIXELS Y)
		{FireEvent(eventidItemDoubleClick,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), ObjectType, Name, X, Y);}
	void FireItemMouseMove(long ObjectType, LPCTSTR Name, OLE_XPOS_PIXELS X, OLE_YPOS_PIXELS Y)
		{FireEvent(eventidItemMouseMove,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), ObjectType, Name, X, Y);}
	void FireOnError(LPCTSTR Description)
		{FireEvent(eventidOnError,EVENT_PARAM(VTS_BSTR), Description);}
	void FireMouseEnter()
		{FireEvent(eventidMouseEnter,EVENT_PARAM(VTS_NONE));}
	void FireMouseExit()
		{FireEvent(eventidMouseExit,EVENT_PARAM(VTS_NONE));}
	void FireShowInfoPanel()
		{FireEvent(eventidShowInfoPanel,EVENT_PARAM(VTS_NONE));}
	void FireZoom()
		{FireEvent(eventidZoom,EVENT_PARAM(VTS_NONE));}
	void FireOnLButtonDown()
		{FireEvent(eventidOnLButtonDown,EVENT_PARAM(VTS_NONE));}
	void FireOnLButtonUp()
		{FireEvent(eventidOnLButtonUp,EVENT_PARAM(VTS_NONE));}
	void FireShowDialog()
		{FireEvent(eventidShowDialog,EVENT_PARAM(VTS_NONE));}
	void FireHideDialog()
		{FireEvent(eventidHideDialog,EVENT_PARAM(VTS_NONE));}
	void FireEnumIndicator(LPCTSTR IndicatorName, long IndicatorID)
		{FireEvent(eventidEnumIndicator,EVENT_PARAM(VTS_BSTR  VTS_I4), IndicatorName, IndicatorID);}
	void FireEnumPriceStyle(LPCTSTR PriceStyleName, long PriceStyleID)
		{FireEvent(eventidEnumPriceStyle,EVENT_PARAM(VTS_BSTR  VTS_I4), PriceStyleName, PriceStyleID);}
	void FireOnRButtonDown()
		{FireEvent(eventidOnRButtonDown,EVENT_PARAM(VTS_NONE));}
	void FireOnRButtonUp()
		{FireEvent(eventidOnRButtonUp,EVENT_PARAM(VTS_NONE));}
	void FireOnChar(short nChar, short nRepCnt, short nFlags)
		{FireEvent(eventidOnChar,EVENT_PARAM(VTS_I2  VTS_I2  VTS_I2), nChar, nRepCnt, nFlags);}
	void FireOnKeyUp(short nChar, short nRepCnt, short nFlags)
		{FireEvent(eventidOnKeyUp,EVENT_PARAM(VTS_I2  VTS_I2  VTS_I2), nChar, nRepCnt, nFlags);}
	void FireOnKeyDown(short nChar, short nRepCnt, short nFlags)
		{FireEvent(eventidOnKeyDown,EVENT_PARAM(VTS_I2  VTS_I2  VTS_I2), nChar, nRepCnt, nFlags);}
	void FireEnumSeries(LPCTSTR Name, long Panel, long TypeOfSeries)
		{FireEvent(eventidEnumSeries,EVENT_PARAM(VTS_BSTR  VTS_I4  VTS_I4), Name, Panel, TypeOfSeries);}
	void FireDialogCancel()
		{FireEvent(eventidDialogCancel,EVENT_PARAM(VTS_NONE));}
	void FireSeriesMoved(LPCTSTR Name, long FromPanel, long ToPanel)
		{FireEvent(eventidSeriesMoved,EVENT_PARAM(VTS_BSTR  VTS_I4  VTS_I4), Name, FromPanel, ToPanel);}
	void FireDoubleClick()
		{FireEvent(eventidDoubleClick,EVENT_PARAM(VTS_NONE));}
	void FireUserDrawingComplete(long StudyType, LPCTSTR Name)
		{FireEvent(eventidUserDrawingComplete,EVENT_PARAM(VTS_I4  VTS_BSTR), StudyType, Name);}
	void FireCustomIndicatorNeedData(LPCTSTR IndicatorName)
		{FireEvent(eventidCustomIndicatorNeedData,EVENT_PARAM(VTS_BSTR), IndicatorName);}
	void FireTrendLinePenetration(LPCTSTR TrendLineName, LPCTSTR SeriesName, short Direction)
		{FireEvent(eventidTrendLinePenetration,EVENT_PARAM(VTS_BSTR  VTS_BSTR  VTS_I2), TrendLineName, SeriesName, Direction);}
	void FireClick()
		{FireEvent(DISPID_CLICK,EVENT_PARAM(VTS_NONE));}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

// Dispatch and event IDs
public:
	bool lineDrawing;
private:

	void Append(LPCSTR Name, double JDate, double Value, bool Tick);

	int Div(double dNum1, double dNum2);
	double Mod(double dNum1, double dNum2);

	void EnumIndicatorMap();

	std::vector<LPCTSTR> indicatorMap;
	std::vector<LPCTSTR> priceStyleMap;

	CPoint m_clickPoint;
	
	bool loaded;
	bool loading;


	long m_Demo;
	bool m_DemoWarned;	

	bool m_setcap;

	CString m_key;

	CCOL* currentLine;

	int m_drawingType;
	CString m_msgGuid;

	CString m_symbolName; // Temp	
	CString m_symbolText;
	long m_symbolType;

	int m_lastStartIndex;
	bool dragFlag;	
	CRect newUserRect;
	CRect oldUserRect;
	int startUserZooming;
	int endUserZooming;
	bool userZooming;
	
	CCalendarPanel calendar;
	struct member{
		CString group;
		int	panel;
	};
	CRect oldRect;
	CRect newRect;
	CRectF oldRectM;
	CRectF newRectM;
	CRect oldHRect;
	CRect oldVRect;
	int startX;
	int startY;
	bool m_xor;
	bool swapping;
	void CalculateScaleInfo();
	bool CanScroll(int Records);
	int m_index;	
	CPoint m_prevPoint;
	CPoint m_prevSelPoint;
	void AddNewSymbolObject(long panel, long style, double x, 
											double y, CString key, CString text);
	void AddNewSeriesType(long panel, long type, CString name);
	bool GetBitmapAndPalette(UINT nIDResource, CBitmap &bitmap, CPalette &pal);
	void DrawDesign(CDC *pDC);
	void DrawComponent(CDC *pDC, bool ShowLogo = false);
	void DestroyAll(bool deletePanels = true);
	enum {
		dispidSaveImageTitle = 388,
		dispidUnSelect = 387L,
		dispidSeriesThreshold = 386,
		dispidGetTrendLineValue = 385L,
		dispidMovePanelIndex = 384L,
		dispidUpdateIndicatorAccumulationDistribution = 383L,
		dispidAddIndicatorAccumulationDistribution = 382L,
		dispidPriceLineThicknessBar = 381,
		dispidPriceLineMono = 380,
		dispidPriceLineThickness = 379,
		dispidAbortDrawing = 378L,
		dispidUpdateIndicatorDI = 377L,

		dispidAddIndicatorDI = 376L,
		dispidUpdateIndicatorADX = 375L,
		dispidAddIndicatorADX = 374L,
		dispidAppendRangeValues = 373L,
		dispidSmoothHeikinPeriods = 372,
		dispidSmoothHeikinType = 371,
		dispidMousePositionY = 370,
		dispidMousePositionX = 369,
		dispidBarSize = 368,
		dispidUpdateIndicatorHILO = 367L,
		dispidAddIndicatorHILO = 366L,
		dispidDeltaCursor = 365,
		dispidApplicationDirectory = 364,
		dispidSeriesShiftInt = 363,
		dispidUpdateIndicatorGenericMovingAverage = 362L,
		dispidAddIndicatorGenericMovingAverage = 361L,
		dispidUpdateIndicatorVolume = 360L,
		dispidAddIndicatorVolume = 359L,
		dispidLanguage = 358,
		dispidWickDownColor = 357,
		dispidWickUpColor = 356,
		dispidStudyDirectory = 355,
		dispidPeriodicity = 354,
		dispidLoadUserStudyLine = 353L,
		dispidGetYScaleMin = 352L,
		dispidGetYScaleMax = 351L,
		dispidSetFibonacciProParams = 350L,
		dispidSetFibonacciRetParams = 349L,
		dispidLineThickness = 348,
		dispidUpdateFibonacciParams = 347L,
		dispidGetFibonacciParameter = 346L,
		dispidGetTrendLineLeftExtension = 345L,
		dispidGetTrendLineRightExtension = 344L,
		dispidGetTrendLineColor = 343L,
		dispidGetTrendLineThickness = 342L,
		dispidGetTrendLineStyle = 341L,
		dispidUpdateTrendLine = 340L,
		dispidMagnetic = 339,
		//dispidMagnetic = 339L,
		dispidGetXRecordByPixel = 338L,
		dispidAddUserYLine = 337L,
		dispidAddUserXLine = 336L,
		dispidSeriesKSlowing = 335,
		dispidSeriesDMAType = 334,
		dispidSeriesDPeriod = 333,
		dispidSeriesPctKDblSmooth = 332,
		dispidSeriesKSmooth = 331,
		dispidSeriesKPeriod = 330,
		dispidSeriesPointPercent = 329,
		dispidSeriesLongTPeriod = 328,
		dispidSeriesShortTPeriod = 327,
		dispidSeriesR2Scale = 326,
		dispidSeriesCycle = 325,
		dispidSeriesMinTick = 324,
		dispidSeriesLevel = 323,
		dispidMaxAF = 322,
		dispidSeriesMinAF = 321,
		dispidSeriesShift = 320,
		dispidSeriesBarHistory = 319,
		dispidAddIndicatorLinearRegIntercept = 318L,
		dispidAddIndicatorBollinger = 317L,
		dispidSeriesSource2 = 316,
		dispidSeriesRateChange = 315,
		dispidSeriesVolumeSource = 314,
		dispidSeriesMAType = 313,
		dispidSeriesStandarDev = 312,
		dispidUpdateIndicatorWilliamPCTR = 311L,
		dispidAddIndicatorWilliamPCTR = 310L,
		dispidUpdateIndicatorWilliamAD = 309L,
		dispidAddIndicatorWilliamAD = 308L,
		dispidUpdateIndicatorWellesWilder = 307L,
		dispidAddIndicatorWellesWilder = 306L,
		dispidUpdateIndicatorWeightedMA = 305L,
		dispidAddIndicatorWeightedMA = 304L,
		dispidUpdateIndicatorWeightedCLose = 303L,
		dispidAddIndicatorWeightedClose = 302L,
		dispidUpdateIndicatorVolumeROC = 301L,
		dispidAddIndicatorVolumeROC = 300L,
		dispidUpdateIndicatorVolumeOsc = 299L,
		dispidAddIndicatorVolumeOsc = 298L,
		dispidUpdateIndicatorVIDYA = 297L,
		dispidAddIndicatorVIDYA = 296L,
		dispidUpdateIndicatorVertiHoriFilter = 295L,
		dispidAddIndicatorVertiHoriFilter = 294L,
		dispidUpdateIndicatorVariableMA = 293L,
		dispidAddIndicatorVariableMA = 292L,
		dispidUpdateIndicatorUltimateOsc = 291L,
		dispidAddIndicatorUltimateOsc = 290L,
		dispidUpdateIndicatorTypicalPrice = 289L,
		dispidAddIndicatorTypicalPrice = 288L,
		dispidUpdateIndicatorTrueRange = 287L,
		dispidAddIndicatorTrueRange = 286L,
		dispidUpdateIndicatorTRIX = 285L,
		dispidAddIndicatorTRIX = 284L,
		dispidUpdateIndicatorTriangularMA = 283L,
		dispidAddIndicatorTriangularMA = 282L,
		dispidUpdateIndicatorTradeVolume = 281L,
		dispidAddIndicatorTradeVolume = 280L,
		dispidUpdateIndicatorTimeSMA = 279L,
		dispidAddIndicatorTimeSMA = 278L,
		dispidUpdateIndicatorSwingIndex = 277L,
		dispidAddIndicatorSwingIndex = 276L,
		dispidUpdateIndicatorStocOscillator = 275L,
		dispidAddIndicatorStocOscillator = 274L,
		dispidUpdateIndicatorStocMomentum = 273L,
		dispidAddIndicatorStocMomentum = 272L,
		dispidUpdateIndicatorStandardDev = 271L,
		dispidAddIndicatorStandardDev = 270L,
		dispidUpdateIndicatorRelativeStrenght = 269L,
		dispidAddIndicatorRelativeStrenght = 268L,
		dispidUpdateIndicatorRainbowOsc = 267L,
		dispidAddIndicatorRainbowOsc = 266L,
		dispidUpdateIndicatorPNumberOscillator = 265L,
		dispidAddIndicatorPNumberOscillator = 264L,
		dispidUpdateIndicatorPNumberBands = 263L,
		dispidAddIndicatorPNumberBands = 262L,
		dispidUpdateIndicatorPriceVolume = 261L,
		dispidAddIndicatorPriceVolume = 260L,
		dispidUpdateIndicatorPriceROC = 259L,
		dispidAddIndicatorPriceROC = 258L,
		dispidUpdateIndicatorPriceOscillator = 257L,
		dispidAddIndicatorPriceOscillator = 256L,
		dispidUpdateIndicatorPositiveVolume = 255L,
		dispidAddIndicatorPositiveVolume = 254L,
		dispidUpdateIndicatorPerformance = 253L,
		dispidAddIndicatorPerformance = 252L,
		dispidUpdateIndicatorParabolicSAR = 251L,
		dispidAddIndicatorParabolicSAR = 250L,
		dispidUpdateIndicatorBalanceVolume = 249L,
		dispidAddIndicatorBalanceVolume = 248L,
		dispidUpdateIndicatorNegVolume = 247L,
		dispidAddIndicatorNegVolume = 246L,
		dispidUpdateIndicatorMAEnvelope = 245L,
		dispidAddIndicatorMAEnvelope = 244L,
		dispidUpdateIndicatorMoneyFlow = 243L,
		dispidAddIndicatorMoneyFlow = 242L,
		dispidUpdateIndicatorMomentum = 241L,
		dispidAddIndicatorMomentum = 240L,
		dispidUpdateIndicatorMedian = 239L,
		dispidAddIndicatorMedian = 238L,
		dispidUpdateIndicatorMassIndex = 237L,
		dispidAddIndicatorMassIndex = 236L,
		dispidSeriesLimitMove = 235,
		dispidSeriesSymbol = 234,
		dispidSeriesLongCycle = 233,
		dispidSeriesShortCycle = 232,
		dispidUpdateIndicatorMACD = 231L,
		dispidAddIndicatorMACD = 230L,
		dispidUpdateIndicatorMACDHistogram = 229L,
		dispidAddIndicatorMACDHistogram = 228L,
		dispidUpdateIndicatorLinearRegSlope = 225L,
		dispidAddIndicatorLinearRegSlope = 224L,
		dispidUpdateIndicatorLinearRegRSquare = 223L,
		dispidAddIndicatorLinearRegRSquare = 222L,
		dispidUpdateIndicatorLinearRegIntercept = 221L,
		dispidUpdateIndicatorLinearRegForecast = 219L,
		dispidAddIndicatorLinearRegForecast = 218L,
		dispidUpdateIndicatorHistoricalVolatility = 217L,
		dispidAddIndicatorHistoricalVolatility = 216L,
		dispidUpdateIndicatorHighLowBands = 215L,
		dispidAddIndicatorHighLowBands = 214L,
		dispidUpdateIndicatorHighMinusLow = 213L,
		dispidAddIndicatorHighMinusLow = 212L,
		dispidUpdateIndicatorFractalChaosOsc = 211L,
		dispidAddIndicatorFractalChaosOsc = 210L,
		dispidUpdateIndicatorFractalChaos = 209L,
		dispidAddIndicatorFractalChaos = 208L,
		dispidUpdateIndicatorEMA = 207L,
		dispidAddIndicatorEMA = 206L,
		dispidUpdateIndicatorEasyMovement = 205L,
		dispidAddIndicatorEasyMovement = 204L,
		dispidUpdateIndicatorDirectMoveSystem = 203L,
		dispidAddIndicatorDirectMoveSystem = 202L,
		dispidUpdateIndicatorDetrendedPrice = 201L,
		dispidAddIndicatorDetrendedPrice = 200L,
		dispidUpdateIndicatorComparativeRSI = 199L,
		dispidAddIndicatorComparativeRSI = 198L,
		dispidUpdateIndicatorCommodityChannel = 197L,
		dispidAddIndicatorCommodityChannel = 196L,
		dispidUpdateIndicatorChandeMomentum = 195L,
		dispidAddIndicatorChandeMomentum = 194L,
		dispidUpdateIndicatorChaikinVolatility = 193L,
		dispidAddIndicatorChaikinVolatility = 192L,
		dispidUpdateIndicatorChaikinMoney = 191L,
		dispidAddIndicatorChaikinMoney = 190L,
		dispidUpdateIndicatorBollinger = 189L,
		dispidUpdateIndicatorAroonOsc = 187L,
		dispidAddIndicatorAroonOsc = 186L,
		dispidUpdateIndicatorAroon = 185L,
		dispidAddIndicatorAroon = 184L,
		dispidUpdateIndicatorASI = 183L,
		dispidUpdateIndicatorSMA = 182L,
		dispidSeriesSource = 181,
		dispidSeriesPeriods = 180,
		dispidAddIndicatorASI = 179L,
		dispidAddIndicatorSMA = 178L,
		dispidFreeze = 177L,
		//{{AFX_DISP_ID(CStockChartXCtrl)	
	dispidRealTimeXLabels = 1L,
	dispidScalePrecision = 2L,
	dispidUseLineSeriesUpDownColors = 3L,
	dispidID = 4L,
	dispidValuePanelColor = 33L,
	dispidChartBackColor = 34L,
	dispidChartForeColor = 35L,
	dispidGridcolor = 36L,
	dispidScaleType = 37L,
	dispidSymbol = 38L,
	dispidUpColor = 39L,
	dispidDownColor = 40L,
	dispidScaleAlignment = 41L,
	dispidWorkspaceRight = 42L,
	dispidWorkspaceLeft = 43L,
	dispidChanged = 44L,
	dispidVisibleRecordCount = 45L,
	dispidRecordCount = 46L,
	dispidFirstVisibleRecord = 47L,
	dispidLastVisibleRecord = 48L,
	dispidSeriesCount = 49L,
	dispidLineColor = 50L,
	dispidHWnd = 51L,
	dispidRightDrawingSpacePixels = 52L,
	dispidDarvasBoxes = 53L,
	dispidDarvasStopPercent = 54L,
	dispidHorizontalSeparators = 5L,
	dispidHorizontalSeparatorColor = 6L,
	dispidShowRecordsForXLabels = 7L,
	dispidDisplayTitles = 8L,
	dispidDisplayTitleBorder = 9L,
	dispidBarWidth = 55L,
	dispidVersion = 10L,
	dispidThreeDStyle = 11L,
	dispidYGrid = 12L,
	dispidPriceStyle = 13L,
	dispidXGrid = 14L,
	dispidPanelCount = 56L,
	dispidAlignment = 57L,
	dispidBarInterval = 58L,
	dispidDisplayInfoText = 15L,
	dispidMaxDisplayRecords = 16L,
	dispidIgnoreSeriesLengthErrors = 17L,
	dispidValueViewGradientTop = 18L,
	dispidValueViewGradientBottom = 19L,
	dispidCrossHairs = 20L,
	dispidBackGradientTop = 21L,
	dispidBackGradientBottom = 22L,
	dispidMousePointer = 23L,
	dispidCandleDownOutlineColor = 24L,
	dispidCandleUpOutlineColor = 25L,
	dispidUserEditing = 59L,
	dispidUseVolumeUpDownColors = 26L,
	dispidBarStartTime = 27L,
	dispidYScaleMinTick = 28L,
	dispidSelectedKey = 60L,
	dispidSelectedType = 61L,
	dispidTextAreaFontSize = 29L,
	dispidTextAreaFontName = 30L,
	dispidCurrentPanel = 62L,
	dispidVolumePostfixLetter = 31L,
	dispidExtraTimePrecision = 32L,
	dispidAppendValue = 99L,
	dispidEditValue = 63L,
	dispidClearValues = 64L,
	dispidUpdate = 65L,
	dispidRemoveSeries = 100L,
	dispidScrollLeft = 66L,
	dispidScrollRight = 67L,
	dispidZoomIn = 68L,
	dispidZoomOut = 69L,
	dispidGetValue = 70L,
	dispidGetValueByJDate = 71L,
	dispidToJulianDate = 72L,
	dispidFromJulianDate = 73L,
	dispidAddSeries = 101L,
	dispidAddChartPanel = 74L,
	dispidResetZoom = 75L,
	dispidRemoveAllSeries = 76L,
	dispidZoomUserDefined = 77L,
	dispidSaveFile = 78L,
	dispidLoadFile = 79L,
	dispidClearDrawings = 80L,
	dispidShowLastTick = 81L,
	dispidReDrawYScale = 82L,
	dispidGetPanelBySeriesName = 83L,
	dispidPrintChart = 84L,
	dispidRemoveSymbolObject = 85L,
	dispidAddSymbolObject = 86L,
	dispidAddUserSymbolObject = 87L,
	dispidAddStaticText = 88L,
	dispidAddUserDefinedText = 89L,
	dispidRemoveObject = 90L,
	dispidAddUserTrendLine = 91L,
	dispidSaveChartBitmap = 92L,
	dispidGetJDate = 93L,
	dispidForcePaint = 94L,
	dispidGetXPixel = 95L,
	dispidSeriesWeight = 155L,
	dispidPanelY1 = 156L,
	dispidPanelY2 = 157L,
	dispidObjectColor = 158L,
	dispidObjectSelectable = 159L,
	dispidSeriesType = 160L,
	dispidSeriesStyle = 161L,
	dispidObjectText = 162L,
	dispidObjectWeight = 163L,
	dispidObjectStyle = 164L,
	dispidShareScale = 165L,
	dispidSeriesColor = 166L,
	dispidGetYPixel = 96L,
	dispidBarColor = 167L,
	dispidDrawTrendLine = 97L,
	dispidAddIndicatorSeries = 98L,
	//dispidAddIndicatorSMA = 178L,
	dispidIndPropStr = 168L,
	dispidIndPropDbl = 169L,
	dispidIndPropInt = 170L,
	dispidShowHelp = 102L,
	dispidShowIndicatorDialog = 103L,
	dispidAddHorizontalLine = 104L,
	dispidRemoveHorizontalLine = 105L,
	dispidPriceStyleParam = 171L,
	dispidEnumIndicators = 106L,
	dispidSetYScale = 107L,
	dispidResetYScale = 108L,
	dispidEnumPriceStyles = 109L,
	dispidGetMaxValue = 110L,
	dispidAddSymbolObjectFromFile = 111L,
	dispidGetMinValue = 112L,
	dispidGetObjectStartRecord = 113L,
	dispidGetObjectEndRecord = 114L,
	dispidGetObjectStartValue = 115L,
	dispidGetObjectEndValue = 116L,
	dispidAppendValueAsTick = 117L,
	dispidGetRecordByJDate = 118L,
	dispidSetSeriesUpDownColors = 119L,
	dispidSetObjectPosition = 120L,
	dispidSeriesVisible = 172L,
	dispidGetYValueByPixel = 121L,
	dispidSeriesName = 173L,
	dispidGetIndicatorCountByType = 122L,
	dispidResetBarColors = 123L,
	dispidGetPriceStyleValueByJDate2 = 124L,
	dispidGetPriceStyleValue1 = 125L,
	dispidGetPriceStyleValue2 = 126L,
	dispidGetPriceStyleValueByJDate1 = 127L,
	dispidGetPriceStyleValueByJDate3 = 128L,
	dispidGetPriceStyleValue3 = 129L,
	dispidEnumSeries = 130L,
	dispidMoveSeries = 131L,
	dispidEditValueByRecord = 132L,
	dispidRecalculateIndicators = 133L,
	dispidDrawLineStudy = 134L,
	dispidCrossOverRecord = 135L,
	dispidCrossOverValue = 136L,
	dispidLineStudyParam = 174L,
	dispidIsSelected = 137L,
	dispidSaveGeneralTemplate = 138L,
	dispidLoadGeneralTemplate = 139L,
	dispidSaveObjectTemplate = 140L,
	dispidLoadObjectTemplate = 141L,
	dispidGetObjectCount = 142L,
	dispidClearAllSeries = 143L,
	dispidAddCustomIndDlgIndPropStr = 144L,
	dispidAddCustomIndDlgIndPropInt = 145L,
	dispidAddCustomIndDlgIndPropDbl = 146L,
	dispidSetCustomIndicatorData = 147L,
	dispidObjectZOrder = 175L,
	dispidObjectFillStyle = 176L,
	dispidGetVisibleMaxValue = 148L,
	dispidGetVisibleMinValue = 149L,
	dispidAddTrendLineWatch = 150L,
	dispidRemoveTrendLineWatch = 151L,
	dispidGetIndicatorType = 152L,
	dispidEditJDate = 153L,
	dispidToJulianDateEx = 154L,
	eventidSelectSeries = 1L,
	eventidDeleteSeries = 2L,
	eventidMouseMove = 3L,
	eventidPaint = 4L,
	eventidScroll = 5L,
	eventidItemRightClick = 6L,
	eventidItemLeftClick = 7L,
	eventidItemDoubleClick = 8L,
	eventidItemMouseMove = 9L,
	eventidOnError = 10L,
	eventidMouseEnter = 11L,
	eventidMouseExit = 12L,
	eventidShowInfoPanel = 13L,
	eventidZoom = 14L,
	eventidOnLButtonDown = 15L,
	eventidOnLButtonUp = 16L,
	eventidShowDialog = 17L,
	eventidHideDialog = 18L,
	eventidEnumIndicator = 19L,
	eventidEnumPriceStyle = 20L,
	eventidOnRButtonDown = 21L,
	eventidOnRButtonUp = 22L,
	eventidOnChar = 23L,
	eventidOnKeyUp = 24L,
	eventidEnumSeries = 25L,
	eventidDialogCancel = 26L,
	eventidSeriesMoved = 27L,
	eventidDoubleClick = 28L,
	eventidUserDrawingComplete = 29L,
	eventidCustomIndicatorNeedData = 30L,
	eventidTrendLinePenetration = 31L,
	eventidOnKeyDown = 32L,
	//}}AFX_DISP_ID
	};

protected:
	void Freeze(BOOL freeze);
	VARIANT_BOOL AddIndicatorSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorASI(LPCTSTR Key, LONG panel, LPCTSTR Source, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	LONG GetSeriesPeriods(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesPeriods(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	BSTR GetSeriesSource(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesSource(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal);
	VARIANT_BOOL UpdateIndicatorSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorASI(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorAroon(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL UpdateIndicatorAroon(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL AddIndicatorAroonOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorAroonOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorBollinger(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorChaikinMoney(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorChaikinMoney(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorChaikinVolatility(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG RateChange, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorChaikinVolatility(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG RateChange, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorChandeMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorChandeMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorCommodityChannel(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorCommodityChannel(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorComparativeRSI(LPCTSTR Key, LONG panel, LPCTSTR Source1, LPCTSTR Source2, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorComparativeRSI(LPCTSTR Key, LONG panel, LPCTSTR Source1, LONG Index, LPCTSTR Source2, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorDetrendedPrice(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorDetrendedPrice(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorDirectMoveSystem(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2, LONG ColorR3, LONG ColorG3, LONG ColorB3, LONG Style3, LONG Thickness3);
	VARIANT_BOOL UpdateIndicatorDirectMoveSystem(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2, LONG ColorR3, LONG ColorG3, LONG ColorB3, LONG Style3, LONG Thickness3);
	VARIANT_BOOL AddIndicatorEasyMovement(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorEasyMovement(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorEMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorEMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorFractalChaos(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorFractalChaos(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorFractalChaosOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorFractalChaosOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorHighMinusLow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorHighMinusLow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorHighLowBands(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorHighLowBands(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorHistoricalVolatility(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG BarHistory, DOUBLE StandardDev, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorHistoricalVolatility(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG BarHistory, DOUBLE StandardDev, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorLinearRegForecast(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorLinearRegForecast(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorLinearRegIntercept(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorLinearRegRSquare(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorLinearRegRSquare(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorLinearRegSlope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorLinearRegSlope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMACDHistogram(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG LongCycle, LONG ShortCycle, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMACDHistogram(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG LongCycle, LONG ShortCycle, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMACD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, FLOAT Scale, LONG ShortCycle, LONG LongCycle, LONG ColorR1, LONG ColorG1, LONG ColorB1, LONG Style1, LONG Thickness1, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL UpdateIndicatorMACD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ShortCycle, LONG LongCycle, LONG ColorR1, LONG ColorG1, LONG ColorB1, LONG Style1, LONG Thickness1, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	LONG GetSeriesShortCycle(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesShortCycle(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesLongCycle(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesLongCycle(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	BSTR GetSeriesSymbol(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesSymbol(LPCTSTR Name, LPCTSTR Code, BSTR newVal);
	DOUBLE GetSeriesLimitMove(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesLimitMove(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	VARIANT_BOOL AddIndicatorMassIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMassIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMedian(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMedian(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMoneyFlow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMoneyFlow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorMAEnvelope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG MAType, DOUBLE Shift, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorMAEnvelope(LPCTSTR Key, LONG panel, LPCSTR Source, LONG Periods, LONG MAType, DOUBLE Shift, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorNegVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorNegVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorBalanceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorBalanceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorParabolicSAR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE MinAF, DOUBLE MaxAF, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorParabolicSAR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE MinAF, DOUBLE MaxAF, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPerformance(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPerformance(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPositiveVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPositiveVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPriceOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG LongCycle, LONG ShortCycle, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPriceOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG LongCycle, LONG ShortCycle, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPriceROC(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPriceROC(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPriceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPriceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPNumberBands(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPNumberBands(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorPNumberOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorPNumberOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorRainbowOsc(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Levels, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorRainbowOsc(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Levels, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorRelativeStrenght(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, DOUBLE Threshold1, DOUBLE Threshold2, LONG TColorR, LONG TColorG, LONG TColorB, LONG TStyle, LONG TThickness);
	VARIANT_BOOL UpdateIndicatorRelativeStrenght(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, DOUBLE Threshold1, DOUBLE Threshold2, LONG TColorR, LONG TColorG, LONG TColorB, LONG TStyle, LONG TThickness);
	VARIANT_BOOL AddIndicatorStandardDev(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandarDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorStandardDev(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorStocMomentum(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Smooth, LONG DblSmooth, LONG DPeriods, LONG MAType, LONG DMAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL UpdateIndicatorStocMomentum(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Smooth, LONG DblSmooth, LONG DPeriods, LONG MAType, LONG DMAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL AddIndicatorStocOscillator(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Slowing, LONG DPeriods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL UpdateIndicatorStocOscillator(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Slowing, LONG DPeriods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	VARIANT_BOOL AddIndicatorSwingIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorSwingIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTimeSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTimeSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTradeVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, DOUBLE MinTick, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTradeVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, DOUBLE MinTick, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTriangularMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTriangularMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTRIX(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTRIX(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTrueRange(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTrueRange(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorTypicalPrice(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorTypicalPrice(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorUltimateOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Cycle1, LONG Cycle2, LONG Cycle3, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorUltimateOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Cycle1, LONG Cycle2, LONG Cycle3, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorVariableMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVariableMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorVertiHoriFilter(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVertiHoriFilter(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorVIDYA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVIDYA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorVolumeOsc(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ShortTerm, LONG LongTerm, LONG PointPercent, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVolumeOsc(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ShortTerm, LONG LongTerm, LONG PointPercent, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorVolumeROC(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVolumeROC(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorWeightedClose(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorWeightedCLose(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorWeightedMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorWeightedMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorWellesWilder(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorWellesWilder(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorWilliamAD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorWilliamAD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorWilliamPCTR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorWilliamPCTR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	DOUBLE GetSeriesStandarDev(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesStandarDev(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	LONG GetSeriesMAType(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesMAType(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	BSTR GetSeriesVolumeSource(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesVolumeSource(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal);
	LONG GetSeriesRateChange(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesRateChange(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	BSTR GetSeriesSource2(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesSource2(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal);
	VARIANT_BOOL AddIndicatorBollinger(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorLinearRegIntercept(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	LONG GetSeriesBarHistory(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesBarHistory(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	DOUBLE GetSeriesShift(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesShift(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	DOUBLE GetSeriesMinAF(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesMinAF(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	DOUBLE GetMaxAF(LPCTSTR Name, LPCTSTR Code);
	void SetMaxAF(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	LONG GetSeriesLevel(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesLevel(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	DOUBLE GetSeriesMinTick(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesMinTick(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	LONG GetSeriesCycle(LPCTSTR Name, LPCTSTR Code, LONG Index);
	void SetSeriesCycle(LPCTSTR Name, LPCTSTR Code, LONG Index, LONG newVal);
	DOUBLE GetSeriesR2Scale(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesR2Scale(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal);
	LONG GetSeriesShortTPeriod(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesShortTPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesLongTPeriod(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesLongTPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesPointPercent(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesPointPercent(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesKPeriod(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesKPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesKSmooth(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesKSmooth(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesPctKDblSmooth(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesPctKDblSmooth(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesDPeriod(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesDPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesDMAType(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesDMAType(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	LONG GetSeriesKSlowing(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesKSlowing(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	void AddUserXLine(LPCTSTR Key);
	void AddUserYLine(LPCTSTR Key);
	double GetXRecordByPixel(LONG Pixel);
	void OnMagneticChanged(void);
	void UpdateTrendLine(LPCTSTR Key, LONG Style, LONG Thickness, LONG ColorR, LONG ColorG, LONG ColorB, VARIANT_BOOL RadExtension, VARIANT_BOOL RightExtension, VARIANT_BOOL LeftExtension, DOUBLE Value);
	LONG GetTrendLineStyle(LPCTSTR Key);
	LONG GetTrendLineThickness(LPCTSTR Key);
	LONG GetTrendLineColor(LPCTSTR Key);
	VARIANT_BOOL GetTrendLineRightExtension(LPCTSTR Key);
	VARIANT_BOOL GetTrendLineLeftExtension(LPCTSTR Key);
public:
	// //Reset all state machines used in drawings
	void StopDrawing(void);
protected:
	DOUBLE GetFibonacciParameter(LPCTSTR Key, LONG Index);
	void UpdateFibonacciParams(LPCTSTR Key, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9, DOUBLE param10);
	LONG GetLineThickness(void);
	void SetLineThickness(LONG newVal);
	void SetFibonacciRetParams(DOUBLE param0, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9);
public:
	int lineThickness;
	int lineWeight;
protected:
	void SetFibonacciProParams(DOUBLE param0, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9);
	DOUBLE GetYScaleMax(LONG panel);
	DOUBLE GetYScaleMin(LONG panel);
public:
	// Removes a study line from the UserStudyLine list
	void RemoveUserStudyLine(LPCTSTR StudyName, LPCTSTR SerieName);
protected:
	void LoadUserStudyLine(LONG panel=-1);
public:
	void AddUserStudyLine(LPCTSTR SerieName, CCOL* Study);
	void AddUserStudyLine(LPCTSTR SerieName, CTextArea* Study);
	void AddUserStudyLine(LPCTSTR SerieName, CCO* Study);
	// // Find 
	int FindUserStudyLine(LPCTSTR Key);
	// Get a close record in any periodicity
	long GetRecordByPeriodJDate(double JDate);
protected:
	void OnPeriodicityChanged(void);
public:
	LONG m_BarSize;
	LONG m_Periodicity;
	bool savingBitmap;
	// Returns current periodicity
	long GetPeriodicity(void);
	// Save all study lines, text areas and symbols
	void SaveUserStudies(void);
	// Returns all Text Objects for all panels
	int GetTextAreasCount(void);
	// Clear all study lines, text areas and symbols
	void ClearUserStudies(void);
protected:
	void OnStudyDirectoryChanged(void);
	CString m_StudyDirectory;
	OLE_COLOR GetWickUpColor(void);
	void SetWickUpColor(OLE_COLOR newVal);
	OLE_COLOR GetWickDownColor(void);
	void SetWickDownColor(OLE_COLOR newVal);
public:
	// Adds a new chart panel using serialized locations (don't calculate panel's position)
	long AddChartPanelSerialized(void);
	// Calculate contrast color
	OLE_COLOR GetContrastColor(OLE_COLOR color);
protected:
	void OnLanguageChanged(void);
public:
	LONG m_Language;
	CString m_ApplicationDirectory;
	VARIANT_BOOL DeltaCursor;
	LONG SmoothHeikinType;
	LONG SmoothHeikinPeriods; 
	LONG GetBarSize(void);
	void UnSelect();

protected:
	VARIANT_BOOL AddIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL AddIndicatorGenericMovingAverage(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG Shift, LONG MAType, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorGenericMovingAverage(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG Shift, LONG MAType, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	LONG GetSeriesShiftInt(LPCTSTR Name, LPCTSTR Code);
	void SetSeriesShiftInt(LPCTSTR Name, LPCTSTR Code, LONG newVal);
	BSTR GetApplicationDirectory(void);
	void SetApplicationDirectory(LPCTSTR newVal);
	void OnDeltaCursorChanged(void);
	VARIANT_BOOL m_DeltaCursor;
	VARIANT_BOOL AddIndicatorHILO(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG PeriodsHigh, LONG PeriodsLow, LONG Shift, DOUBLE Scale, OLE_COLOR LineColorHigh, OLE_COLOR LineColorLow, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorHILO(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG PeriodsHigh, LONG PeriodsLow, LONG Shift, DOUBLE Scale, OLE_COLOR LineColorHigh, OLE_COLOR LineColorLow, LONG Style, LONG Thickness);
	void SetBarSize(LONG newVal);
	void OnMousePositionXChanged(void);
	LONG m_MousePositionX;
	void OnMousePositionYChanged(void);
	LONG m_MousePositionY;
	void OnSmoothHeikinTypeChanged(void);
	LONG m_SmoothHeikinType;
	void OnSmoothHeikinPeriodsChanged(void);
	LONG m_SmoothHeikinPeriods;
	void AppendRangeValues(LPCTSTR Name, VARIANT& JDates, VARIANT& Values, LONG Size);
	VARIANT_BOOL AddIndicatorADX(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Thickness, LONG Style);
	VARIANT_BOOL UpdateIndicatorADX(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Thickness, LONG Style);
	VARIANT_BOOL AddIndicatorDI(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	
	VARIANT_BOOL UpdateIndicatorDI(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2);
	void AbortDrawing(void);
	void OnPriceLineThicknessChanged(void);
	LONG m_PriceLineThickness;
	void OnPriceLineMonoChanged(void);
	VARIANT_BOOL m_PriceLineMono;
	void OnPriceLineThicknessBarChanged(void);
	LONG m_PriceLineThicknessBar;
	VARIANT_BOOL AddIndicatorAccumulationDistribution(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	VARIANT_BOOL UpdateIndicatorAccumulationDistribution(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness);
	void MovePanelIndex(LONG panel, LONG offset, VARIANT_BOOL moveUp);
	DOUBLE GetTrendLineValue(LPCTSTR Key);
	DOUBLE GetSeriesThreshold(LPCTSTR name, LONG index);
	void SetSeriesThreshold(LPCTSTR name, LONG index, DOUBLE newVal);
	void OnSaveImageTitleChanged();
	CString m_SaveImageTitle;
};


/////////////////////////////////////////////////////////////////////////////
// UserStudyLine - Save user's study according with the panel's serie
/*
class UserStudyLine{
public:
// Constructors
	// create an uninitialized class
	UserStudyLine() throw();
	// create from two parameters
	UserStudyLine(
		_In_ CCOL StudyLine,
		_In_ CString SerieName) throw();
};
*/
//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STOCKCHARTXCTL_H__9967D72A_3714_4A67_8185_E0A74FF03C38__INCLUDED)
