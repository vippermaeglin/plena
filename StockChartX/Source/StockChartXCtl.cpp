// StockChartXCtl.cpp : Implementation of the CStockChartXCtrl ActiveX Control class.


#include "stdafx.h"
#include "StockChartX.h"
#include "StockChartXCtl.h"
#include "CalendarPanel.h"
#include "DialogReminder.h"
#include "DebugConsole.h"
#include <time.h>
#include <cmath>

//#define _CONSOLE_DEBUG


#pragma comment(lib,"htmlhelp.lib")
#include "htmlhelp.h"

#include "gbitmapprinter.h"
using namespace printer;

#include "StockChartX.h"
extern CStockChartXApp theApp;

#include "julian.h"


#include <d2d1.h>
#include <d2d1helper.h>
#pragma comment(lib, "d2d1")

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// Release build UPX compression post-build step removed 3/29/09:
//.\UPX\Compress.bat $(ProjDir)\UPX, $(TargetPath) 

//#define PERSONAL_LICENSE

//#define DEMO_MODE

IMPLEMENT_DYNCREATE(CStockChartXCtrl, COleControl)


/////////////////////////////////////////////////////////////////////////////
// Message map

BEGIN_MESSAGE_MAP(CStockChartXCtrl, COleControl)
	//{{AFX_MSG_MAP(CStockChartXCtrl)
	ON_WM_PAINT()
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_SETCURSOR()	
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONDOWN()
	ON_WM_CHAR()
	ON_WM_TIMER()
	ON_WM_KEYUP()
	ON_WM_KEYDOWN()
	ON_WM_MOUSEWHEEL()
	ON_WM_KILLFOCUS()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_RBUTTONUP()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_EDIT, OnEdit)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// Dispatch map

BEGIN_DISPATCH_MAP(CStockChartXCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CStockChartXCtrl)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "RealTimeXLabels", m_realTimeXLabels, OnRealTimeXLabelsChanged, VT_BOOL)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ScalePrecision", m_scalePrecision, OnScalePrecisionChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "UseLineSeriesUpDownColors", m_useLineSeriesUpDownColors, OnUseLineSeriesUpDownColorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ID", m_iD, OnIDChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "HorizontalSeparators", m_horizontalSeparators, OnHorizontalSeparatorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "HorizontalSeparatorColor", m_horizontalSeparatorColor, OnHorizontalSeparatorColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ShowRecordsForXLabels", m_showRecordsForXLabels, OnShowRecordsForXLabelsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "DisplayTitles", m_displayTitles, OnDisplayTitlesChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "DisplayTitleBorder", m_displayTitleBorder, OnDisplayTitleBorderChanged, VT_BOOL)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "Version", m_version, OnVersionChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ThreeDStyle", m_threeDStyle, OnThreeDStyleChanged, VT_BOOL)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "YGrid", m_yGrid, OnYGridChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "PriceStyle", m_priceStyle, OnPriceStyleChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "XGrid", m_xGrid, OnXGridChanged, VT_BOOL)		
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "DisplayInfoText", m_displayInfoText, OnDisplayInfoTextChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "MaxDisplayRecords", m_maxDisplayRecords, OnMaxDisplayRecordsChanged, VT_I4)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "IgnoreSeriesLengthErrors", m_ignoreSeriesLengthErrors, OnIgnoreSeriesLengthErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ValueViewGradientTop", m_valueViewGradientTop, OnValueViewGradientTopChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ValueViewGradientBottom", m_valueViewGradientBottom, OnValueViewGradientBottomChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "CrossHairs", m_crossHairs, OnCrossHairsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "BackGradientTop", m_backGradientTop, OnBackGradientTopChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "BackGradientBottom", m_backGradientBottom, OnBackGradientBottomChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "MousePointer", m_mousePointer, OnMousePointerChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "CandleDownOutlineColor", m_candleDownOutlineColor, OnCandleDownOutlineColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "CandleUpOutlineColor", m_candleUpOutlineColor, OnCandleUpOutlineColorChanged, VT_COLOR)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "UseVolumeUpDownColors", m_useVolumeUpDownColors, OnUseVolumeUpDownColorsChanged, VT_BOOL)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "BarStartTime", m_barStartTime, OnBarStartTimeChanged, VT_R8)	
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "YScaleMinTick", m_yScaleMinTick, OnYScaleMinTickChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "TextAreaFontSize", m_textAreaFontSize, OnTextAreaFontSizeChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "TextAreaFontName", m_textAreaFontName, OnTextAreaFontNameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "VolumePostfixLetter", m_volumePostfixLetter, OnVolumePostfixLetterChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CStockChartXCtrl, "ExtraTimePrecision", m_extraTimePrecision, OnExtraTimePrecisionChanged, VT_BOOL)
	DISP_PROPERTY_EX(CStockChartXCtrl, "ValuePanelColor", GetValuePanelColor, SetValuePanelColor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "ChartBackColor", GetChartBackColor, SetChartBackColor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "ChartForeColor", GetChartForeColor, SetChartForeColor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "Gridcolor", GetGridcolor, SetGridcolor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "ScaleType", GetScaleType, SetScaleType, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "Symbol", GetSymbol, SetSymbol, VT_BSTR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "UpColor", GetUpColor, SetUpColor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "DownColor", GetDownColor, SetDownColor, VT_COLOR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "ScaleAlignment", GetScaleAlignment, SetScaleAlignment, VT_I4)		
	DISP_PROPERTY_EX(CStockChartXCtrl, "WorkspaceRight", GetWorkspaceRight, SetWorkspaceRight, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "WorkspaceLeft", GetWorkspaceLeft, SetWorkspaceLeft, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "Changed", GetChanged, SetChanged, VT_BOOL)
	DISP_PROPERTY_EX(CStockChartXCtrl, "VisibleRecordCount", GetVisibleRecordCount, SetVisibleRecordCount, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "RecordCount", GetRecordCount, SetRecordCount, VT_I4)				
	DISP_PROPERTY_EX(CStockChartXCtrl, "FirstVisibleRecord", GetFirstVisibleRecord, SetFirstVisibleRecord, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "LastVisibleRecord", GetLastVisibleRecord, SetLastVisibleRecord, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "SeriesCount", GetSeriesCount, SetNotSupported, VT_I4)	
	DISP_PROPERTY_EX(CStockChartXCtrl, "LineColor", GetLineColor, SetLineColor, VT_COLOR)	
	DISP_PROPERTY_EX(CStockChartXCtrl, "hWnd", GetHWnd, SetNotSupported, VT_HANDLE)
	DISP_PROPERTY_EX(CStockChartXCtrl, "RightDrawingSpacePixels", GetRightDrawingSpacePixels, SetRightDrawingSpacePixels, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "DarvasBoxes", GetDarvasBoxes, SetDarvasBoxes, VT_BOOL)
	DISP_PROPERTY_EX(CStockChartXCtrl, "DarvasStopPercent", GetDarvasStopPercent, SetDarvasStopPercent, VT_R8)
	DISP_PROPERTY_EX(CStockChartXCtrl, "BarWidth", GetBarWidth, SetBarWidth, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "PanelCount", GetPanelCount, SetNotSupported, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "Alignment", GetAlignment, SetAlignment, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "BarInterval", GetBarInterval, SetBarInterval, VT_I4)	
	DISP_PROPERTY_EX(CStockChartXCtrl, "UserEditing", GetUserEditing, SetUserEditing, VT_BOOL)	
	DISP_PROPERTY_EX(CStockChartXCtrl, "SelectedKey", GetSelectedKey, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX(CStockChartXCtrl, "SelectedType", GetSelectedType, SetNotSupported, VT_I4)
	DISP_PROPERTY_EX(CStockChartXCtrl, "CurrentPanel", GetCurrentPanel, SetNotSupported, VT_I2)
	DISP_FUNCTION(CStockChartXCtrl, "EditValue", EditValue, VT_EMPTY, VTS_BSTR VTS_R8 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "ClearValues", ClearValues, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "Update", Update, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ScrollLeft", ScrollLeft, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ScrollRight", ScrollRight, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ZoomIn", ZoomIn, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ZoomOut", ZoomOut, VT_EMPTY, VTS_I4)		
	DISP_FUNCTION(CStockChartXCtrl, "GetValue", GetValue, VT_R8, VTS_BSTR VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "GetValueByJDate", GetValueByJDate, VT_R8, VTS_BSTR VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "ToJulianDate", ToJulianDate, VT_R8, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "FromJulianDate", FromJulianDate, VT_BSTR, VTS_R8)	
	DISP_FUNCTION(CStockChartXCtrl, "AddChartPanel", AddChartPanel, VT_I4, VTS_NONE)		
	DISP_FUNCTION(CStockChartXCtrl, "ResetZoom", ResetZoom, VT_EMPTY, VTS_NONE)			
	DISP_FUNCTION(CStockChartXCtrl, "RemoveAllSeries", RemoveAllSeries, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "ZoomUserDefined", ZoomUserDefined, VT_EMPTY, VTS_NONE)	
	DISP_FUNCTION(CStockChartXCtrl, "SaveFile", SaveFile, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "LoadFile", LoadFile, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "ClearDrawings", ClearDrawings, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "ShowLastTick", ShowLastTick, VT_EMPTY, VTS_BSTR VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "ReDrawYScale", ReDrawYScale, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "GetPanelBySeriesName", GetPanelBySeriesName, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "PrintChart", PrintChart, VT_EMPTY, VTS_NONE)		
	DISP_FUNCTION(CStockChartXCtrl, "RemoveSymbolObject", RemoveSymbolObject, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddSymbolObject", AddSymbolObject, VT_EMPTY, VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_BSTR VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "AddUserSymbolObject", AddUserSymbolObject, VT_EMPTY, VTS_I4 VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddStaticText", AddStaticText, VT_EMPTY, VTS_I4 VTS_BSTR VTS_BSTR VTS_COLOR VTS_BOOL VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "AddUserDefinedText", AddUserDefinedText, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "RemoveObject", RemoveObject, VT_EMPTY, VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddUserTrendLine", AddUserTrendLine, VT_EMPTY, VTS_BSTR)		
	DISP_FUNCTION(CStockChartXCtrl, "SaveChartBitmap", SaveChartBitmap, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetJDate", GetJDate, VT_R8, VTS_BSTR VTS_I4)	
	DISP_FUNCTION(CStockChartXCtrl, "ForcePaint", ForcePaint, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "GetXPixel", GetXPixel, VT_I4, VTS_I4)		
	DISP_FUNCTION(CStockChartXCtrl, "GetYPixel", GetYPixel, VT_I4, VTS_I4 VTS_R8)	
	DISP_FUNCTION(CStockChartXCtrl, "DrawTrendLine", DrawTrendLine, VT_EMPTY, VTS_I4 VTS_R8 VTS_I4 VTS_R8 VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddIndicatorSeries", AddIndicatorSeries, VT_BOOL, VTS_I4 VTS_BSTR VTS_I4 VTS_BOOL)
	//DISP_FUNCTION(CStockChartXCtrl, "AddIndicatorSMA", AddIndicatorSMA, VT_BOOL,VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "AppendValue", AppendValue, VT_EMPTY, VTS_BSTR VTS_R8 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "RemoveSeries", RemoveSeries, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddSeries", AddSeries, VT_I4, VTS_BSTR VTS_I4 VTS_I4)	
	DISP_FUNCTION(CStockChartXCtrl, "ShowHelp", ShowHelp, VT_EMPTY, VTS_BSTR VTS_I4)
	//DISP_FUNCTION(CStockChartXCtrl, "ShowHelp", ShowHelp, VT_BOOL,VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ShowIndicatorDialog", ShowIndicatorDialog, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddHorizontalLine", AddHorizontalLine, VT_EMPTY, VTS_I4 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "RemoveHorizontalLine", RemoveHorizontalLine, VT_EMPTY, VTS_I4 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "EnumIndicators", EnumIndicators, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "SetYScale", SetYScale, VT_EMPTY, VTS_I4 VTS_R8 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "ResetYScale", ResetYScale, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "EnumPriceStyles", EnumPriceStyles, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "GetMaxValue", GetMaxValue, VT_R8, VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "AddSymbolObjectFromFile", AddSymbolObjectFromFile, VT_EMPTY, VTS_I4 VTS_R8 VTS_I4 VTS_BSTR VTS_BSTR VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "GetMinValue", GetMinValue, VT_R8, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetObjectStartRecord", GetObjectStartRecord, VT_I4, VTS_I4 VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "GetObjectEndRecord", GetObjectEndRecord, VT_I4, VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetObjectStartValue", GetObjectStartValue, VT_R8, VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetObjectEndValue", GetObjectEndValue, VT_R8, VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AppendValueAsTick", AppendValueAsTick, VT_EMPTY, VTS_BSTR VTS_R8 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "GetRecordByJDate", GetRecordByJDate, VT_I4, VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "SetSeriesUpDownColors", SetSeriesUpDownColors, VT_EMPTY, VTS_BSTR VTS_COLOR VTS_COLOR)
	DISP_FUNCTION(CStockChartXCtrl, "SetObjectPosition", SetObjectPosition, VT_EMPTY, VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_R8)	
	DISP_FUNCTION(CStockChartXCtrl, "GetYValueByPixel", GetYValueByPixel, VT_R8, VTS_I4)	
	DISP_FUNCTION(CStockChartXCtrl, "GetIndicatorCountByType", GetIndicatorCountByType, VT_I4, VTS_I4)	
	DISP_FUNCTION(CStockChartXCtrl, "ResetBarColors", ResetBarColors, VT_EMPTY, VTS_NONE)	
	//DISP_FUNCTION(CStockChartXCtrl, "ResetBarColors", ResetBarColors, VT_BOOL,VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValueByJDate2", GetPriceStyleValueByJDate2, VT_R8, VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValue1", GetPriceStyleValue1, VT_R8, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValue2", GetPriceStyleValue2, VT_R8, VTS_I4)	
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValueByJDate1", GetPriceStyleValueByJDate1, VT_R8, VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValueByJDate3", GetPriceStyleValueByJDate3, VT_R8, VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "GetPriceStyleValue3", GetPriceStyleValue3, VT_R8, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "EnumSeries", EnumSeries, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "MoveSeries", MoveSeries, VT_EMPTY, VTS_BSTR VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "EditValueByRecord", EditValueByRecord, VT_EMPTY, VTS_BSTR VTS_I4 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "RecalculateIndicators", RecalculateIndicators, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "DrawLineStudy", DrawLineStudy, VT_EMPTY, VTS_I4  VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "CrossOverRecord", CrossOverRecord, VT_I4, VTS_I4 VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "CrossOverValue", CrossOverValue, VT_R8, VTS_I4 VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "IsSelected", IsSelected, VT_BOOL, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "SaveGeneralTemplate", SaveGeneralTemplate, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "LoadGeneralTemplate", LoadGeneralTemplate, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "SaveObjectTemplate", SaveObjectTemplate, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "LoadObjectTemplate", LoadObjectTemplate, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetObjectCount", GetObjectCount, VT_I4, VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "ClearAllSeries", ClearAllSeries, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CStockChartXCtrl, "AddCustomIndDlgIndPropStr", AddCustomIndDlgIndPropStr, VT_I2, VTS_BSTR VTS_I4 VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "AddCustomIndDlgIndPropInt", AddCustomIndDlgIndPropInt, VT_I2, VTS_BSTR VTS_I4 VTS_I4)
	DISP_FUNCTION(CStockChartXCtrl, "AddCustomIndDlgIndPropDbl", AddCustomIndDlgIndPropDbl, VT_I2, VTS_BSTR VTS_I4 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "SetCustomIndicatorData", SetCustomIndicatorData, VT_I4, VTS_BSTR VTS_VARIANT VTS_BOOL)
	DISP_FUNCTION(CStockChartXCtrl, "GetVisibleMaxValue", GetVisibleMaxValue, VT_R8, VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetVisibleMinValue", GetVisibleMinValue, VT_R8, VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "AddTrendLineWatch", AddTrendLineWatch, VT_EMPTY, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "RemoveTrendLineWatch", RemoveTrendLineWatch, VT_EMPTY, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CStockChartXCtrl, "GetIndicatorType", GetIndicatorType, VT_I4, VTS_BSTR)	
	DISP_FUNCTION(CStockChartXCtrl, "EditJDate", EditJDate, VT_EMPTY, VTS_I4 VTS_R8)
	DISP_FUNCTION(CStockChartXCtrl, "ToJulianDateEx", ToJulianDateEx, VT_R8, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesWeight", GetSeriesWeight, SetSeriesWeight, VT_I4, VTS_BSTR)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "PanelY1", GetPanelY1, SetPanelY1, VT_I4, VTS_I4)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "PanelY2", GetPanelY2, SetPanelY2, VT_I4, VTS_I4)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectColor", GetObjectColor, SetObjectColor, VT_COLOR, VTS_I4 VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectSelectable", GetObjectSelectable, SetObjectSelectable, VT_BOOL, VTS_I4 VTS_BSTR)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesType", GetSeriesType, SetSeriesType, VT_I4, VTS_BSTR)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesStyle", GetSeriesStyle, SetSeriesStyle, VT_I4, VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectText", GetObjectText, SetObjectText, VT_BSTR, VTS_I4 VTS_BSTR)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectWeight", GetObjectWeight, SetObjectWeight, VT_I4, VTS_I4 VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectStyle", GetObjectStyle, SetObjectStyle, VT_I4, VTS_I4 VTS_BSTR)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ShareScale", GetShareScale, SetShareScale, VT_BOOL, VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesColor", GetSeriesColor, SetSeriesColor, VT_I4, VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "BarColor", GetBarColor, SetBarColor, VT_COLOR, VTS_I4 VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "IndPropStr", GetIndPropStr, SetIndPropStr, VT_BSTR, VTS_BSTR VTS_I2)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "IndPropDbl", GetIndPropDbl, SetIndPropDbl, VT_R8, VTS_BSTR VTS_I2)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "IndPropInt", GetIndPropInt, SetIndPropInt, VT_I2, VTS_BSTR VTS_I2)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "PriceStyleParam", GetPriceStyleParam, SetPriceStyleParam, VT_R8, VTS_I4)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesVisible", GetSeriesVisible, SetSeriesVisible, VT_BOOL, VTS_BSTR)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "SeriesName", GetSeriesName, SetNotSupported, VT_BSTR, VTS_I4)	
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "LineStudyParam", GetLineStudyParam, SetLineStudyParam, VT_R8, VTS_BSTR VTS_I2)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectZOrder", GetObjectZOrder, SetObjectZOrder, VT_I4, VTS_BSTR VTS_I4)
	DISP_PROPERTY_PARAM(CStockChartXCtrl, "ObjectFillStyle", GetObjectFillStyle, SetObjectFillStyle, VT_I4, VTS_BSTR VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)	
	//}}AFX_DISPATCH_MAP
	DISP_FUNCTION_ID(CStockChartXCtrl, "Freeze", dispidFreeze, Freeze, VT_EMPTY, VTS_BOOL)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorSMA", dispidAddIndicatorSMA, AddIndicatorSMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorASI", dispidAddIndicatorASI, AddIndicatorASI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesPeriods", dispidSeriesPeriods, GetSeriesPeriods, SetSeriesPeriods, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesSource", dispidSeriesSource, GetSeriesSource, SetSeriesSource, VT_BSTR, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorSMA", dispidUpdateIndicatorSMA, UpdateIndicatorSMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorASI", dispidUpdateIndicatorASI, UpdateIndicatorASI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorAroon", dispidAddIndicatorAroon, AddIndicatorAroon, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorAroon", dispidUpdateIndicatorAroon, UpdateIndicatorAroon, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorAroonOsc", dispidAddIndicatorAroonOsc, AddIndicatorAroonOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorAroonOsc", dispidUpdateIndicatorAroonOsc, UpdateIndicatorAroonOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorBollinger", dispidUpdateIndicatorBollinger, UpdateIndicatorBollinger, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorChaikinMoney", dispidAddIndicatorChaikinMoney, AddIndicatorChaikinMoney, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorChaikinMoney", dispidUpdateIndicatorChaikinMoney, UpdateIndicatorChaikinMoney, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorChaikinVolatility", dispidAddIndicatorChaikinVolatility, AddIndicatorChaikinVolatility, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorChaikinVolatility", dispidUpdateIndicatorChaikinVolatility, UpdateIndicatorChaikinVolatility, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorChandeMomentum", dispidAddIndicatorChandeMomentum, AddIndicatorChandeMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorChandeMomentum", dispidUpdateIndicatorChandeMomentum, UpdateIndicatorChandeMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorCommodityChannel", dispidAddIndicatorCommodityChannel, AddIndicatorCommodityChannel, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorCommodityChannel", dispidUpdateIndicatorCommodityChannel, UpdateIndicatorCommodityChannel, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorComparativeRSI", dispidAddIndicatorComparativeRSI, AddIndicatorComparativeRSI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorComparativeRSI", dispidUpdateIndicatorComparativeRSI, UpdateIndicatorComparativeRSI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorDetrendedPrice", dispidAddIndicatorDetrendedPrice, AddIndicatorDetrendedPrice, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorDetrendedPrice", dispidUpdateIndicatorDetrendedPrice, UpdateIndicatorDetrendedPrice, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorDirectMoveSystem", dispidAddIndicatorDirectMoveSystem, AddIndicatorDirectMoveSystem, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorDirectMoveSystem", dispidUpdateIndicatorDirectMoveSystem, UpdateIndicatorDirectMoveSystem, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorEasyMovement", dispidAddIndicatorEasyMovement, AddIndicatorEasyMovement, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorEasyMovement", dispidUpdateIndicatorEasyMovement, UpdateIndicatorEasyMovement, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorEMA", dispidAddIndicatorEMA, AddIndicatorEMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorEMA", dispidUpdateIndicatorEMA, UpdateIndicatorEMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorFractalChaos", dispidAddIndicatorFractalChaos, AddIndicatorFractalChaos, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorFractalChaos", dispidUpdateIndicatorFractalChaos, UpdateIndicatorFractalChaos, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorFractalChaosOsc", dispidAddIndicatorFractalChaosOsc, AddIndicatorFractalChaosOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorFractalChaosOsc", dispidUpdateIndicatorFractalChaosOsc, UpdateIndicatorFractalChaosOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorHighMinusLow", dispidAddIndicatorHighMinusLow, AddIndicatorHighMinusLow, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorHighMinusLow", dispidUpdateIndicatorHighMinusLow, UpdateIndicatorHighMinusLow, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorHighLowBands", dispidAddIndicatorHighLowBands, AddIndicatorHighLowBands, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorHighLowBands", dispidUpdateIndicatorHighLowBands, UpdateIndicatorHighLowBands, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorHistoricalVolatility", dispidAddIndicatorHistoricalVolatility, AddIndicatorHistoricalVolatility, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorHistoricalVolatility", dispidUpdateIndicatorHistoricalVolatility, UpdateIndicatorHistoricalVolatility, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorLinearRegForecast", dispidAddIndicatorLinearRegForecast, AddIndicatorLinearRegForecast, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorLinearRegForecast", dispidUpdateIndicatorLinearRegForecast, UpdateIndicatorLinearRegForecast, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorLinearRegIntercept", dispidUpdateIndicatorLinearRegIntercept, UpdateIndicatorLinearRegIntercept, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorLinearRegRSquare", dispidAddIndicatorLinearRegRSquare, AddIndicatorLinearRegRSquare, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorLinearRegRSquare", dispidUpdateIndicatorLinearRegRSquare, UpdateIndicatorLinearRegRSquare, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorLinearRegSlope", dispidAddIndicatorLinearRegSlope, AddIndicatorLinearRegSlope, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorLinearRegSlope", dispidUpdateIndicatorLinearRegSlope, UpdateIndicatorLinearRegSlope, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMACDHistogram", dispidAddIndicatorMACDHistogram, AddIndicatorMACDHistogram, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMACDHistogram", dispidUpdateIndicatorMACDHistogram, UpdateIndicatorMACDHistogram, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMACD", dispidAddIndicatorMACD, AddIndicatorMACD, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMACD", dispidUpdateIndicatorMACD, UpdateIndicatorMACD, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesShortCycle", dispidSeriesShortCycle, GetSeriesShortCycle, SetSeriesShortCycle, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesLongCycle", dispidSeriesLongCycle, GetSeriesLongCycle, SetSeriesLongCycle, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesSymbol", dispidSeriesSymbol, GetSeriesSymbol, SetSeriesSymbol, VT_BSTR, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesLimitMove", dispidSeriesLimitMove, GetSeriesLimitMove, SetSeriesLimitMove, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMassIndex", dispidAddIndicatorMassIndex, AddIndicatorMassIndex, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMassIndex", dispidUpdateIndicatorMassIndex, UpdateIndicatorMassIndex, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMedian", dispidAddIndicatorMedian, AddIndicatorMedian, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMedian", dispidUpdateIndicatorMedian, UpdateIndicatorMedian, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMomentum", dispidAddIndicatorMomentum, AddIndicatorMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMomentum", dispidUpdateIndicatorMomentum, UpdateIndicatorMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMoneyFlow", dispidAddIndicatorMoneyFlow, AddIndicatorMoneyFlow, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMoneyFlow", dispidUpdateIndicatorMoneyFlow, UpdateIndicatorMoneyFlow, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorMAEnvelope", dispidAddIndicatorMAEnvelope, AddIndicatorMAEnvelope, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorMAEnvelope", dispidUpdateIndicatorMAEnvelope, UpdateIndicatorMAEnvelope, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorNegVolume", dispidAddIndicatorNegVolume, AddIndicatorNegVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorNegVolume", dispidUpdateIndicatorNegVolume, UpdateIndicatorNegVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorBalanceVolume", dispidAddIndicatorBalanceVolume, AddIndicatorBalanceVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorBalanceVolume", dispidUpdateIndicatorBalanceVolume, UpdateIndicatorBalanceVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorParabolicSAR", dispidAddIndicatorParabolicSAR, AddIndicatorParabolicSAR, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_R8 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorParabolicSAR", dispidUpdateIndicatorParabolicSAR, UpdateIndicatorParabolicSAR, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_R8 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPerformance", dispidAddIndicatorPerformance, AddIndicatorPerformance, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPerformance", dispidUpdateIndicatorPerformance, UpdateIndicatorPerformance, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPositiveVolume", dispidAddIndicatorPositiveVolume, AddIndicatorPositiveVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPositiveVolume", dispidUpdateIndicatorPositiveVolume, UpdateIndicatorPositiveVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPriceOscillator", dispidAddIndicatorPriceOscillator, AddIndicatorPriceOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPriceOscillator", dispidUpdateIndicatorPriceOscillator, UpdateIndicatorPriceOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPriceROC", dispidAddIndicatorPriceROC, AddIndicatorPriceROC, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPriceROC", dispidUpdateIndicatorPriceROC, UpdateIndicatorPriceROC, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPriceVolume", dispidAddIndicatorPriceVolume, AddIndicatorPriceVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPriceVolume", dispidUpdateIndicatorPriceVolume, UpdateIndicatorPriceVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPNumberBands", dispidAddIndicatorPNumberBands, AddIndicatorPNumberBands, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPNumberBands", dispidUpdateIndicatorPNumberBands, UpdateIndicatorPNumberBands, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorPNumberOscillator", dispidAddIndicatorPNumberOscillator, AddIndicatorPNumberOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorPNumberOscillator", dispidUpdateIndicatorPNumberOscillator, UpdateIndicatorPNumberOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorRainbowOsc", dispidAddIndicatorRainbowOsc, AddIndicatorRainbowOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorRainbowOsc", dispidUpdateIndicatorRainbowOsc, UpdateIndicatorRainbowOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorRelativeStrenght", dispidAddIndicatorRelativeStrenght, AddIndicatorRelativeStrenght, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorRelativeStrenght", dispidUpdateIndicatorRelativeStrenght, UpdateIndicatorRelativeStrenght, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorStandardDev", dispidAddIndicatorStandardDev, AddIndicatorStandardDev, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorStandardDev", dispidUpdateIndicatorStandardDev, UpdateIndicatorStandardDev, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorStocMomentum", dispidAddIndicatorStocMomentum, AddIndicatorStocMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorStocMomentum", dispidUpdateIndicatorStocMomentum, UpdateIndicatorStocMomentum, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorStocOscillator", dispidAddIndicatorStocOscillator, AddIndicatorStocOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorStocOscillator", dispidUpdateIndicatorStocOscillator, UpdateIndicatorStocOscillator, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorSwingIndex", dispidAddIndicatorSwingIndex, AddIndicatorSwingIndex, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorSwingIndex", dispidUpdateIndicatorSwingIndex, UpdateIndicatorSwingIndex, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTimeSMA", dispidAddIndicatorTimeSMA, AddIndicatorTimeSMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTimeSMA", dispidUpdateIndicatorTimeSMA, UpdateIndicatorTimeSMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTradeVolume", dispidAddIndicatorTradeVolume, AddIndicatorTradeVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTradeVolume", dispidUpdateIndicatorTradeVolume, UpdateIndicatorTradeVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTriangularMA", dispidAddIndicatorTriangularMA, AddIndicatorTriangularMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTriangularMA", dispidUpdateIndicatorTriangularMA, UpdateIndicatorTriangularMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTRIX", dispidAddIndicatorTRIX, AddIndicatorTRIX, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTRIX", dispidUpdateIndicatorTRIX, UpdateIndicatorTRIX, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTrueRange", dispidAddIndicatorTrueRange, AddIndicatorTrueRange, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTrueRange", dispidUpdateIndicatorTrueRange, UpdateIndicatorTrueRange, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorTypicalPrice", dispidAddIndicatorTypicalPrice, AddIndicatorTypicalPrice, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorTypicalPrice", dispidUpdateIndicatorTypicalPrice, UpdateIndicatorTypicalPrice, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorUltimateOsc", dispidAddIndicatorUltimateOsc, AddIndicatorUltimateOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorUltimateOsc", dispidUpdateIndicatorUltimateOsc, UpdateIndicatorUltimateOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVariableMA", dispidAddIndicatorVariableMA, AddIndicatorVariableMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVariableMA", dispidUpdateIndicatorVariableMA, UpdateIndicatorVariableMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVertiHoriFilter", dispidAddIndicatorVertiHoriFilter, AddIndicatorVertiHoriFilter, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVertiHoriFilter", dispidUpdateIndicatorVertiHoriFilter, UpdateIndicatorVertiHoriFilter, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVIDYA", dispidAddIndicatorVIDYA, AddIndicatorVIDYA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVIDYA", dispidUpdateIndicatorVIDYA, UpdateIndicatorVIDYA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVolumeOsc", dispidAddIndicatorVolumeOsc, AddIndicatorVolumeOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVolumeOsc", dispidUpdateIndicatorVolumeOsc, UpdateIndicatorVolumeOsc, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVolumeROC", dispidAddIndicatorVolumeROC, AddIndicatorVolumeROC, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVolumeROC", dispidUpdateIndicatorVolumeROC, UpdateIndicatorVolumeROC, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorWeightedClose", dispidAddIndicatorWeightedClose, AddIndicatorWeightedClose, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorWeightedCLose", dispidUpdateIndicatorWeightedCLose, UpdateIndicatorWeightedCLose, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorWeightedMA", dispidAddIndicatorWeightedMA, AddIndicatorWeightedMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorWeightedMA", dispidUpdateIndicatorWeightedMA, UpdateIndicatorWeightedMA, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorWellesWilder", dispidAddIndicatorWellesWilder, AddIndicatorWellesWilder, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorWellesWilder", dispidUpdateIndicatorWellesWilder, UpdateIndicatorWellesWilder, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorWilliamAD", dispidAddIndicatorWilliamAD, AddIndicatorWilliamAD, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorWilliamAD", dispidUpdateIndicatorWilliamAD, UpdateIndicatorWilliamAD, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorWilliamPCTR", dispidAddIndicatorWilliamPCTR, AddIndicatorWilliamPCTR, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorWilliamPCTR", dispidUpdateIndicatorWilliamPCTR, UpdateIndicatorWilliamPCTR, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesStandarDev", dispidSeriesStandarDev, GetSeriesStandarDev, SetSeriesStandarDev, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesMAType", dispidSeriesMAType, GetSeriesMAType, SetSeriesMAType, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesVolumeSource", dispidSeriesVolumeSource, GetSeriesVolumeSource, SetSeriesVolumeSource, VT_BSTR, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesRateChange", dispidSeriesRateChange, GetSeriesRateChange, SetSeriesRateChange, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesSource2", dispidSeriesSource2, GetSeriesSource2, SetSeriesSource2, VT_BSTR, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorBollinger", dispidAddIndicatorBollinger, AddIndicatorBollinger, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorLinearRegIntercept", dispidAddIndicatorLinearRegIntercept, AddIndicatorLinearRegIntercept, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesBarHistory", dispidSeriesBarHistory, GetSeriesBarHistory, SetSeriesBarHistory, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesShift", dispidSeriesShift, GetSeriesShift, SetSeriesShift, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesMinAF", dispidSeriesMinAF, GetSeriesMinAF, SetSeriesMinAF, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "MaxAF", dispidMaxAF, GetMaxAF, SetMaxAF, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesLevel", dispidSeriesLevel, GetSeriesLevel, SetSeriesLevel, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesMinTick", dispidSeriesMinTick, GetSeriesMinTick, SetSeriesMinTick, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesCycle", dispidSeriesCycle, GetSeriesCycle, SetSeriesCycle, VT_I4, VTS_BSTR VTS_BSTR VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesR2Scale", dispidSeriesR2Scale, GetSeriesR2Scale, SetSeriesR2Scale, VT_R8, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesShortTPeriod", dispidSeriesShortTPeriod, GetSeriesShortTPeriod, SetSeriesShortTPeriod, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesLongTPeriod", dispidSeriesLongTPeriod, GetSeriesLongTPeriod, SetSeriesLongTPeriod, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesPointPercent", dispidSeriesPointPercent, GetSeriesPointPercent, SetSeriesPointPercent, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesKPeriod", dispidSeriesKPeriod, GetSeriesKPeriod, SetSeriesKPeriod, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesKSmooth", dispidSeriesKSmooth, GetSeriesKSmooth, SetSeriesKSmooth, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesPctKDblSmooth", dispidSeriesPctKDblSmooth, GetSeriesPctKDblSmooth, SetSeriesPctKDblSmooth, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesDPeriod", dispidSeriesDPeriod, GetSeriesDPeriod, SetSeriesDPeriod, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesDMAType", dispidSeriesDMAType, GetSeriesDMAType, SetSeriesDMAType, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesKSlowing", dispidSeriesKSlowing, GetSeriesKSlowing, SetSeriesKSlowing, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddUserXLine", dispidAddUserXLine, AddUserXLine, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddUserYLine", dispidAddUserYLine, AddUserYLine, VT_EMPTY, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetXRecordByPixel", dispidGetXRecordByPixel, GetXRecordByPixel, VT_R8, VTS_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "Magnetic", dispidMagnetic, m_Magnetic, OnMagneticChanged, VT_BOOL)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateTrendLine", dispidUpdateTrendLine, UpdateTrendLine, VT_EMPTY, VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL VTS_BOOL VTS_BOOL VTS_R8)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineStyle", dispidGetTrendLineStyle, GetTrendLineStyle, VT_I4, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineThickness", dispidGetTrendLineThickness, GetTrendLineThickness, VT_I4, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineColor", dispidGetTrendLineColor, GetTrendLineColor, VT_I4, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineRightExtension", dispidGetTrendLineRightExtension, GetTrendLineRightExtension, VT_BOOL, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineLeftExtension", dispidGetTrendLineLeftExtension, GetTrendLineLeftExtension, VT_BOOL, VTS_BSTR)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetFibonacciParameter", dispidGetFibonacciParameter, GetFibonacciParameter, VT_R8, VTS_BSTR VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateFibonacciParams", dispidUpdateFibonacciParams, UpdateFibonacciParams, VT_EMPTY, VTS_BSTR VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8)
	DISP_PROPERTY_EX_ID(CStockChartXCtrl, "LineThickness", dispidLineThickness, GetLineThickness, SetLineThickness, VT_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "SetFibonacciRetParams", dispidSetFibonacciRetParams, SetFibonacciRetParams, VT_EMPTY, VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8)
	DISP_FUNCTION_ID(CStockChartXCtrl, "SetFibonacciProParams", dispidSetFibonacciProParams, SetFibonacciProParams, VT_EMPTY, VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8 VTS_R8)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetYScaleMax", dispidGetYScaleMax, GetYScaleMax, VT_R8, VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetYScaleMin", dispidGetYScaleMin, GetYScaleMin, VT_R8, VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "LoadUserStudyLine", dispidLoadUserStudyLine, LoadUserStudyLine, VT_EMPTY, VTS_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "Periodicity", dispidPeriodicity, m_Periodicity, OnPeriodicityChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "StudyDirectory", dispidStudyDirectory, m_StudyDirectory, OnStudyDirectoryChanged, VT_BSTR)
	DISP_PROPERTY_EX_ID(CStockChartXCtrl, "WickUpColor", dispidWickUpColor, GetWickUpColor, SetWickUpColor, VT_COLOR)
	DISP_PROPERTY_EX_ID(CStockChartXCtrl, "WickDownColor", dispidWickDownColor, GetWickDownColor, SetWickDownColor, VT_COLOR)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "Language", dispidLanguage, m_Language, OnLanguageChanged, VT_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorVolume", dispidAddIndicatorVolume, AddIndicatorVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorVolume", dispidUpdateIndicatorVolume, UpdateIndicatorVolume, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorGenericMovingAverage", dispidAddIndicatorGenericMovingAverage, AddIndicatorGenericMovingAverage, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorGenericMovingAverage", dispidUpdateIndicatorGenericMovingAverage, UpdateIndicatorGenericMovingAverage, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesShiftInt", dispidSeriesShiftInt, GetSeriesShiftInt, SetSeriesShiftInt, VT_I4, VTS_BSTR VTS_BSTR)
	DISP_PROPERTY_EX_ID(CStockChartXCtrl, "ApplicationDirectory", dispidApplicationDirectory, GetApplicationDirectory, SetApplicationDirectory, VT_BSTR)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "DeltaCursor", dispidDeltaCursor, m_DeltaCursor, OnDeltaCursorChanged, VT_BOOL)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorHILO", dispidAddIndicatorHILO, AddIndicatorHILO, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_COLOR VTS_COLOR VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorHILO", dispidUpdateIndicatorHILO, UpdateIndicatorHILO, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_COLOR VTS_COLOR VTS_I4 VTS_I4)
	DISP_PROPERTY_EX_ID(CStockChartXCtrl, "BarSize", dispidBarSize, GetBarSize, SetBarSize, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "MousePositionX", dispidMousePositionX, m_MousePositionX, OnMousePositionXChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "MousePositionY", dispidMousePositionY, m_MousePositionY, OnMousePositionYChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "SmoothHeikinType", dispidSmoothHeikinType, m_SmoothHeikinType, OnSmoothHeikinTypeChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "SmoothHeikinPeriods", dispidSmoothHeikinPeriods, m_SmoothHeikinPeriods, OnSmoothHeikinPeriodsChanged, VT_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AppendRangeValues", dispidAppendRangeValues, AppendRangeValues, VT_EMPTY, VTS_BSTR VTS_VARIANT VTS_VARIANT VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorADX", dispidAddIndicatorADX, AddIndicatorADX, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorADX", dispidUpdateIndicatorADX, UpdateIndicatorADX, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorDI", dispidAddIndicatorDI, AddIndicatorDI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorDI", dispidUpdateIndicatorDI, UpdateIndicatorDI, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AbortDrawing", dispidAbortDrawing, AbortDrawing, VT_EMPTY, VTS_NONE)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "PriceLineThickness", dispidPriceLineThickness, m_PriceLineThickness, OnPriceLineThicknessChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "PriceLineMono", dispidPriceLineMono, m_PriceLineMono, OnPriceLineMonoChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "PriceLineThicknessBar", dispidPriceLineThicknessBar, m_PriceLineThicknessBar, OnPriceLineThicknessBarChanged, VT_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "AddIndicatorAccumulationDistribution", dispidAddIndicatorAccumulationDistribution, AddIndicatorAccumulationDistribution, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UpdateIndicatorAccumulationDistribution", dispidUpdateIndicatorAccumulationDistribution, UpdateIndicatorAccumulationDistribution, VT_BOOL, VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_BSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "MovePanelIndex", dispidMovePanelIndex, MovePanelIndex, VT_EMPTY, VTS_I4 VTS_I4 VTS_BOOL)
	DISP_FUNCTION_ID(CStockChartXCtrl, "GetTrendLineValue", dispidGetTrendLineValue, GetTrendLineValue, VT_R8, VTS_BSTR)
	DISP_PROPERTY_PARAM_ID(CStockChartXCtrl, "SeriesThreshold", dispidSeriesThreshold, GetSeriesThreshold, SetSeriesThreshold, VT_R8, VTS_BSTR VTS_I4)
	DISP_FUNCTION_ID(CStockChartXCtrl, "UnSelect", dispidUnSelect, UnSelect, VT_EMPTY, VTS_NONE)
	DISP_PROPERTY_NOTIFY_ID(CStockChartXCtrl, "SaveImageTitle", dispidSaveImageTitle, m_SaveImageTitle, OnSaveImageTitleChanged, VT_BSTR)
	END_DISPATCH_MAP()

	
/////////////////////////////////////////////////////////////////////////////
// Event map

BEGIN_EVENT_MAP(CStockChartXCtrl, COleControl)
	//{{AFX_EVENT_MAP(CStockChartXCtrl)
	EVENT_CUSTOM("SelectSeries", FireSelectSeries, VTS_BSTR)
	EVENT_CUSTOM("DeleteSeries", FireDeleteSeries, VTS_BSTR)
	EVENT_CUSTOM("MouseMove", FireMouseOver, VTS_XPOS_PIXELS  VTS_YPOS_PIXELS  VTS_I4)	
	EVENT_CUSTOM("Paint", FirePaint, VTS_I4)
	EVENT_CUSTOM("Scroll", FireScroll, VTS_NONE)	
	EVENT_CUSTOM("ItemRightClick", FireItemRightClick, VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	EVENT_CUSTOM("ItemLeftClick", FireItemLeftClick, VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	EVENT_CUSTOM("ItemDoubleClick", FireItemDoubleClick, VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	EVENT_CUSTOM("ItemMouseMove", FireItemMouseMove, VTS_I4  VTS_BSTR  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	EVENT_CUSTOM("OnError", FireOnError, VTS_BSTR)
	EVENT_CUSTOM("MouseEnter", FireMouseEnter, VTS_NONE)
	EVENT_CUSTOM("MouseExit", FireMouseExit, VTS_NONE)
	EVENT_CUSTOM("ShowInfoPanel", FireShowInfoPanel, VTS_NONE)
	EVENT_CUSTOM("Zoom", FireZoom, VTS_NONE)
	EVENT_CUSTOM("OnLButtonDown", FireOnLButtonDown, VTS_NONE)
	EVENT_CUSTOM("OnLButtonUp", FireOnLButtonUp, VTS_NONE)		
	EVENT_CUSTOM("ShowDialog", FireShowDialog, VTS_NONE)
	EVENT_CUSTOM("HideDialog", FireHideDialog, VTS_NONE)
	EVENT_CUSTOM("EnumIndicator", FireEnumIndicator, VTS_BSTR  VTS_I4)
	EVENT_CUSTOM("EnumPriceStyle", FireEnumPriceStyle, VTS_BSTR  VTS_I4)
	EVENT_CUSTOM("OnRButtonDown", FireOnRButtonDown, VTS_NONE)
	EVENT_CUSTOM("OnRButtonUp", FireOnRButtonUp, VTS_NONE)
	EVENT_CUSTOM("OnChar", FireOnChar, VTS_I2  VTS_I2  VTS_I2)
	EVENT_CUSTOM("OnKeyUp", FireOnKeyUp, VTS_I2  VTS_I2  VTS_I2)
	EVENT_CUSTOM("OnKeyDown", FireOnKeyDown, VTS_I2  VTS_I2  VTS_I2)
	EVENT_CUSTOM("EnumSeries", FireEnumSeries, VTS_BSTR  VTS_I4  VTS_I4)
	EVENT_CUSTOM("DialogCancel", FireDialogCancel, VTS_NONE)
	EVENT_CUSTOM("SeriesMoved", FireSeriesMoved, VTS_BSTR  VTS_I4  VTS_I4)
	EVENT_CUSTOM("DoubleClick", FireDoubleClick, VTS_NONE)		
	EVENT_CUSTOM("UserDrawingComplete", FireUserDrawingComplete, VTS_I4  VTS_BSTR)
	EVENT_CUSTOM("CustomIndicatorNeedData", FireCustomIndicatorNeedData, VTS_BSTR)
	EVENT_CUSTOM("TrendLinePenetration", FireTrendLinePenetration, VTS_BSTR  VTS_BSTR  VTS_I2)
	EVENT_CUSTOM_ID("Click", DISPID_CLICK, FireClick, VTS_NONE)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()



/////////////////////////////////////////////////////////////////////////////
// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CStockChartXCtrl, "STOCKCHARTX.StockChartXCtrl.1",
	0x89d9aaaf, 0xdc4f, 0x4f72, 0xac, 0xf3, 0xd2, 0xed, 0xb8, 0x64, 0x4a, 0x44)


/////////////////////////////////////////////////////////////////////////////
// Type library ID and version

IMPLEMENT_OLETYPELIB(CStockChartXCtrl, _tlid, _wVerMajor, _wVerMinor)


/////////////////////////////////////////////////////////////////////////////
// Interface IDs

const IID BASED_CODE IID_DStockChartX =
		{ 0x68a22d73, 0x5c59, 0x47a3, { 0x96, 0xa0, 0x97, 0x8e, 0x14, 0x1a, 0xac, 0xa3 } };
const IID BASED_CODE IID_DStockChartXEvents =
		{ 0x6005828e, 0xc667, 0x45e2, { 0xb5, 0x33, 0x71, 0x9, 0xf, 0xea, 0x9b, 0x7a } };


/////////////////////////////////////////////////////////////////////////////
// Control type information

static const DWORD BASED_CODE _dwStockChartXOleMisc =
	OLEMISC_ACTIVATEWHENVISIBLE |
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CStockChartXCtrl, IDS_STOCKCHARTX, _dwStockChartXOleMisc)


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::CStockChartXCtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CStockChartXCtrl

BOOL CStockChartXCtrl::CStockChartXCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: Verify that your control follows apartment-model threading rules.
	// Refer to MFC TechNote 64 for more information.
	// If your control does not conform to the apartment-model rules, then
	// you must modify the code below, changing the 6th parameter from
	// afxRegInsertable | afxRegApartmentThreading to afxRegInsertable.
	if (bRegister)
		return AfxOleRegisterControlClass(
			AfxGetInstanceHandle(),
			m_clsid,
			m_lpszProgID,
			IDS_STOCKCHARTX,
			IDB_STOCKCHARTX,
			afxRegInsertable | afxRegApartmentThreading,
			_dwStockChartXOleMisc,
			_tlid,
			_wVerMajor,
			_wVerMinor);
	else
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}


/////////////////////////////////////////////////////////////////////////////
// Licensing strings

static const TCHAR BASED_CODE _szLicFileName[] = _T("StockChartX.lic");

static const WCHAR BASED_CODE _szLicString[] =
	L"BX9T-BBTN-8972-NYWT-MQRT-98777";


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::CStockChartXCtrlFactory::VerifyUserLicense -
// Checks for existence of a user license

BOOL CStockChartXCtrl::CStockChartXCtrlFactory::VerifyUserLicense()
{
	return AfxVerifyLicFile(AfxGetInstanceHandle(), _szLicFileName,
		_szLicString);
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::CStockChartXCtrlFactory::GetLicenseKey -
// Returns a runtime licensing key

BOOL CStockChartXCtrl::CStockChartXCtrlFactory::GetLicenseKey(DWORD dwReserved,
	BSTR FAR* pbstrKey)
{
	if (pbstrKey == NULL)
		return FALSE;

	*pbstrKey = SysAllocString(_szLicString);
	return (*pbstrKey != NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::CStockChartXCtrl - Constructor

CStockChartXCtrl::CStockChartXCtrl()
	: lineThickness(0)
	, lineWeight(0)
{
	InitializeIIDs(&IID_DStockChartX, &IID_DStockChartXEvents);	
	m_DemoWarned = false;
	m_Demo = 0;
	loaded = false;
	m_pauseCrossHairs = false;
	suspendCrossHair=false;
	m_Language=0;
	m_PriceLineThickness = 1;
	m_PriceLineMono = true;
	savingBitmap = false;
	IgnorePaint = false;
	lastMouse=CPoint(0,0);
	SmoothHeikinType=m_SmoothHeikinType = (int)indExponentialMovingAverage;
	SmoothHeikinPeriods=m_SmoothHeikinPeriods = 5;
	// TODO: Initialize your control's instance data here.
#ifdef _CONSOLE_DEBUG
	InitializeDebugConsole();		//Comment this to hide Debug Console
#endif

	//Set type of PDC: 0->GDI / 1->GDI+ / 2->Direct2D
	PDCType = 1;
	pdcHandler = new PDCHandler();
	pdcHandler->IsClipRegion = false;
	pdcHandler->Connect(this);
	first=true;
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::~CStockChartXCtrl - Destructor

CStockChartXCtrl::~CStockChartXCtrl()
{		
	
	if(m_memDC.m_hDC != NULL)
	{
		if(oldBmp) m_memDC.SelectObject(oldBmp);
		m_memDC.DeleteDC();		
	}
	
	DestroyAll();
	
	try{
		if(m_valueView) delete m_valueView;	//12/27/08
	}
	catch(...){}

}

void CStockChartXCtrl::DestroyAll(bool deletePanels /* = true */){	
	if(!loaded) return; // Added 11/7/08
	int n;	
	if(panels.size() > 0){
		for(n = 0; n != panels.size(); ++n){
			// Destroy everything...
			if(deletePanels)
			{
				try{
					for(int j = 0; j != panels[n]->series.size(); ++j){
						if(panels[n]->series[j]){	
							delete panels[n]->series[j];
							panels[n]->series[j] = NULL;
						}
					}
					panels[n]->series.resize(0);
				}					
				catch(...){}
			}
			try{
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]){							
						RemoveUserStudyLine(panels[n]->textAreas[j]->key,panels[n]->series[0]->szName );	
						delete panels[n]->textAreas[j];
						panels[n]->textAreas[j] = NULL;
					}
				}
				panels[n]->textAreas.resize(0);
			}
			catch(...){}
			try{
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]){										
						RemoveUserStudyLine(panels[n]->lines[j]->key,panels[n]->series[0]->szName );	
						delete panels[n]->lines[j];
						panels[n]->lines[j] = NULL;						
					}
				}
				panels[n]->lines.resize(0);
			}
			catch(...){}
			try{
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]){		
						RemoveUserStudyLine(panels[n]->objects[j]->key,panels[n]->series[0]->szName );	
						delete panels[n]->objects[j];
						panels[n]->objects[j] = NULL;						
					}
				}
				panels[n]->objects.resize(0);
			}
			catch(...){}
			try{
				if(deletePanels){
					if(panels[n]){
						delete panels[n];
						panels[n] = NULL;
					}
				}
				//panels[n]->textAreas.clear();
			}
			catch(...){}
		}
		try{
			if(deletePanels) panels.clear();
		}
		catch(...){}
	}
	try{
		if(deletePanels) panels.clear();
	}
	catch(...){}

}

/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::OnDraw - Drawing function

void CStockChartXCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{	
	if (!IsOptimizedDraw())
	{		
		// The container does not support optimized drawing.

		// TODO: if you selected any GDI objects into the device context *pdc,
		//		restore the previously-selected objects here.
		//		For more information, please see MFC technical note #nnn,
		//		"Optimizing an ActiveX Control".
	}
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::DoPropExchange - Persistence support

void CStockChartXCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);	
	PX_Color(pPX, _T("HorizontalSeparatorColor"), m_horizontalSeparatorColor, RGB(255,255,255));
	PX_Color(pPX, _T("ValuePanelColor"), valuePanelColor, RGB(250,245,215));
	PX_Color(pPX, _T("ChartBackColor"), backColor, RGB(0,0,0));
	PX_Color(pPX, _T("ChartForeColor"), foreColor, RGB(255,255,255));
	PX_Color(pPX, _T("GridColor"), gridColor, RGB(70,70,70));
	PX_Color(pPX, _T("UpColor"), upColor, RGB(0,0,0));
	PX_Color(pPX, _T("DownColor"), downColor, RGB(0,0,0));
	PX_Color(pPX, _T("LineColor"), lineColor, RGB(0,0,255));
	 
	PX_Color(pPX, _T("ValueViewGradientTop"), m_valueViewGradientTop, 0);
	valueViewGradientTop = m_valueViewGradientTop;
	PX_Color(pPX, _T("ValueViewGradientBottom"), m_valueViewGradientBottom, 0);
	valueViewGradientBottom = m_valueViewGradientBottom;

	PX_Color(pPX, _T("BackGradientTop"), m_backGradientTop, 0);
	backGradientTop = m_backGradientTop;
	PX_Color(pPX, _T("BackGradientBottom"), m_backGradientBottom, 0);
	backGradientBottom = m_backGradientBottom;

	PX_Color(pPX, _T("CandleUpOutlineColor"), m_candleUpOutlineColor, 0);	
	PX_Color(pPX, _T("CandleDownOutlineColor"), m_candleDownOutlineColor, 0);
	candleDownOutlineColor = m_candleDownOutlineColor;
	candleUpOutlineColor = m_candleUpOutlineColor;

	PX_Color(pPX, _T("ValuePanelColor"), valuePanelColor, RGB(250,245,215));	
	PX_Long(pPX, _T("ScaleAlignment"), yAlignment, RIGHT);
	PX_Long(pPX, _T("ScaleType"), scalingType, LINEAR);	
	PX_Long(pPX, _T("ScalePrecision"), m_scalePrecision, 2);
	PX_Long(pPX, _T("PriceStyle"), m_priceStyle, psStandard);
	
	PX_Bool(pPX, _T("RealTimeXLabels"), m_realTimeXLabels, FALSE);
	realTime = m_realTimeXLabels == TRUE;
	
	PX_Bool(pPX, _T("ShowRecordsForXLabels"), m_showRecordsForXLabels, FALSE);
	recordLabels = m_showRecordsForXLabels == TRUE;
	
	PX_Double(pPX, _T("DarvasStopPercent"), darvasPct, 0.01f);	
	
	PX_Bool(pPX, _T("HorizontalSeparators"), m_horizontalSeparators, FALSE);	
	m_horzLines = m_horizontalSeparators == TRUE;

	PX_Bool(pPX, _T("DisplayTitleBorder"), m_displayTitleBorder, FALSE);	
	displayTitles =  m_displayTitleBorder == TRUE;

	PX_Bool(pPX, _T("ShowYGrid"), m_yGrid, TRUE);
	showYGrid = m_yGrid == TRUE;

	//PX_Bool(pPX, _T("ShowXGrid"), m_xGrid, FALSE);
	//showXGrid = m_xGrid == FALSE;
		

	// TODO: Call PX_ functions for each persistent custom property.

}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::GetControlFlags -
// Flags to customize MFC's implementation of ActiveX controls.
//
// For information on using these flags, please see MFC technical note
// #nnn, "Optimizing an ActiveX Control".
DWORD CStockChartXCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();


	// The control will not be redrawn when making the transition
	// between the active and inactivate state.
	dwFlags |= noFlickerActivate;

	// The control can optimize its OnDraw method, by not restoring
	// the original GDI objects in the device context.
	dwFlags |= canOptimizeDraw;
	return dwFlags;
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::OnResetState - Reset control to default state

void CStockChartXCtrl::OnResetState()
{
	COleControl::OnResetState();  // Resets defaults found in DoPropExchange

	// TODO: Reset any other control state here.
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::AboutBox - Display an "About" box to the user

void CStockChartXCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_STOCKCHARTX);
	dlgAbout.DoModal();
}

// Random number generator for double values
double CStockChartXCtrl::get_rand(double low_end, double high_end)
{
    /* Assume rand() has been seeded already with srand */
    int num;
    double retval;

    num = rand(); /* Gives a number between 0 and RAND_MAX */

    retval = ((double) num / (double) (RAND_MAX-1)) * (high_end -
              low_end) + low_end;

    return retval;
}


/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl message handlers

/*	SGC	31.05.2004	BEG*/

void CStockChartXCtrl::OnPaint() 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nOnPaint()");
#endif
	
	
	if(m_frozen) return;

	CPaintDC dc(this);	

	if(pdcHandler->IsClipRegion) bResetMemDC = false;
	
	drawToDC( &dc );
	
	if(pdcHandler->IsClipRegion){
		
			pdcHandler->IsClipRegion = false;
#ifdef _CONSOLE_DEBUG
			//printf("\nCLIP-REGION [OFF]\nRETURN");
#endif
			return;
	}

	/*for (int n = 0; n < panels.size(); ++n)
	{
		//force panel repaint, no need here to send message, cause the entire chart gets repainted here
		if (panels[n]->visible)
		{
			panels[n]->InvalidateXOR();
			panels[n]->OnPaintXOR( &m_memDC );
		}
	}*/
	

}
/////////////////////////////////////////////////////////////////////////////

void	CStockChartXCtrl::drawToDC( CDC* pDC )
{
	if( buildingChart || (drawing && focus) )
		return;

	if( ! ::IsWindow( m_hWnd ) )
		return;

	ASSERT(pDC != NULL);

	buildingChart	= true;
	
	if( ! AmbientUserMode() )
	{
		DrawDesign( pDC );
	}
	else
	{
		suspendCrossHair = true;
		DrawComponent( pDC/*, false*/); //Flag removed: Dont suspend CrossHairs just here!
	}
	
	buildingChart = false;

}
/*	SGC	31.05.2004	END*/

/////////////////////////////////////////////////////////////////////////////
/*	SGC	31.05.2004	BEG*/

void CStockChartXCtrl::DrawTransparent(INT x,INT y,INT bitmapID, 
									   COLORREF crColor, CDC* pDC, LPCTSTR fileName)
{		
}
/*	SGC	31.05.2004	END*/


/////////////////////////////////////////////////////////////////////////////
// Draws a 256 color bitmap
bool CStockChartXCtrl::GetBitmapAndPalette(UINT nIDResource, CBitmap &bitmap, CPalette &pal)
{
	LPCTSTR lpszResourceName = (LPCTSTR)nIDResource;

	HBITMAP hBmp = (HBITMAP)::LoadImage( AfxGetInstanceHandle(), 
			lpszResourceName, IMAGE_BITMAP, 0,0, LR_CREATEDIBSECTION );

	if( hBmp == NULL ) 
		return FALSE;

	bitmap.Attach( hBmp );

	// Create a logical palette for the bitmap
	DIBSECTION ds;
	BITMAPINFOHEADER &bmInfo = ds.dsBmih;
	bitmap.GetObject( sizeof(ds), &ds );

	int nColors = bmInfo.biClrUsed ? bmInfo.biClrUsed : 1 << bmInfo.biBitCount;

	//	Create a halftone palette if colors > 256. 
	CClientDC	dc(NULL);	//	Desktop DC
	if( nColors > 256 )
	{
		pal.CreateHalftonePalette( &dc );
	}
	else
	{
		//	Create the palette
		RGBQUAD *pRGB = new RGBQUAD[nColors];
		CDC memDC;
		memDC.CreateCompatibleDC(&dc);

		memDC.SelectObject( &bitmap );
		::GetDIBColorTable( memDC, 0, nColors, pRGB );

		UINT nSize = sizeof(LOGPALETTE) + (sizeof(PALETTEENTRY) * nColors);
		LOGPALETTE *pLP = (LOGPALETTE *) new BYTE[nSize];

		pLP->palVersion = 0x300;
		pLP->palNumEntries = nColors;

		for( int i=0; i < nColors; i++)
		{
			pLP->palPalEntry[i].peRed = pRGB[i].rgbRed;
			pLP->palPalEntry[i].peGreen = pRGB[i].rgbGreen;
			pLP->palPalEntry[i].peBlue = pRGB[i].rgbBlue;
			pLP->palPalEntry[i].peFlags = 0;
		}

		pal.CreatePalette( pLP );

		delete[] pLP;
		delete[] pRGB;
	}

	return TRUE;
}
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::DrawDesign(CDC* pdc) - Draws the StockChartX icon when
// this control is displayed in design mode of a container.
void CStockChartXCtrl::DrawDesign(CDC *pDC)
{		
	// Get client rect to center the bitmap
	RECT rc;
	GetClientRect(&rc);

	// Create a memory DC compatible with the paint DC
	CDC memDC;
	BITMAP BM;
	HGDIOBJ hBrush, hOldBrush, hPen, hOldPen;

	memDC.CreateCompatibleDC(pDC);

	CBitmap bitmap;
	CPalette palette;
	
	GetBitmapAndPalette(IDB_STOCKCHARTX_256, bitmap, palette);
	oldBmp = memDC.SelectObject( &bitmap );

	// Select and realize the palette
	if( pDC->GetDeviceCaps(RASTERCAPS) & RC_PALETTE && palette.m_hObject != NULL)
	{
		pDC->SelectPalette( &palette, FALSE );
		pDC->RealizePalette();
	}

	// delete this block if you don't want to draw the background white
	hPen = CreatePen(PS_SOLID, 1, RGB(255, 255, 255));
	hOldPen = SelectObject(pDC->m_hDC, hPen);
	hBrush = CreateSolidBrush(RGB(255, 255, 255));
	hOldBrush = SelectObject(pDC->m_hDC, hBrush);
	pDC->Rectangle(rc.left, rc.top, rc.right, rc.bottom);
	SelectObject(pDC->m_hDC, hOldBrush);
	DeleteObject(hBrush);
	SelectObject(pDC->m_hDC, hOldPen);
	DeleteObject(hPen);


	bitmap.GetObject(sizeof(BM), &BM);

	// here we actually draw the bitmap
	pDC->BitBlt((rc.right-BM.bmWidth)/2, (rc.bottom-BM.bmHeight)/2,
		 BM.bmWidth, BM.bmHeight, &memDC, 0, 0,SRCCOPY);

	if(m_crossHairs)
		DrawCrossHairs(m_point);
 
	if( oldBmp )
		memDC.SelectObject(oldBmp);
}



/////////////////////////////////////////////////////////////////////////////
// CStockChartXCtrl::DrawControls(CDC* pdc) - Calls drawing functions in child
// chart area objects and also initiates drawing object code based on chart area hWnds.
void CStockChartXCtrl::DrawComponent(CDC* pdc, bool ShowLogo /*=false*/)
{	
	//Just for tests
	//ShowLogo = true;
	
#ifdef _CONSOLE_DEBUG
	//printf("\nDrawComponent()");
#endif
	//	Main drawing method from MW_PAINT 	
	CRect rect;
	pdc->GetWindow()->GetClientRect(&rect);
	
	
	SuspendCrossHairs();
	
	SuspendMeasure();
//Show m-ValueView 
	if(displayInfoText && !resizing && m_mouseState == MOUSE_NORMAL && 
		m_buttonState == MOUSE_DOWN && !movingObject)
	{
		drawing = true;

		if( !m_valueView->visible )
			FireShowInfoPanel();

		m_valueView->Show();

		drawing = false; 
	}
	else
	{
		if(m_valueView->visible)
		{
			drawing = false;
			m_valueView->Hide();
		}
	}
	if( bResetMemDC )
	{
			
#ifdef _CONSOLE_DEBUG
	//printf("\tbResetMemDC");
#endif
		//	Setup the memDC to draw everything on
		try{
			if( oldBmp )
				m_memDC.SelectObject( oldBmp );

			m_memDC.DeleteDC();
			
			//Release Direct2D objects:
			pdcHandler->Release();

			if( ! m_memDC.CreateCompatibleDC( pdc ) )
				return;
			
			m_memDC.SetBkMode(pdc->GetBkMode());
			m_bitmap.DeleteObject();

			if( !m_bitmap.CreateCompatibleBitmap( pdc, rect.right, rect.bottom ) )
				return;
			
			oldBmp = m_memDC.SelectObject( &m_bitmap );
			
			//Create main window:
			m_memDC.FillSolidRect(rect.left,rect.top,rect.right,rect.bottom,RGB(0,0,0));
#ifdef _CONSOLE_DEBUG
	//printf("\tbDrawComponent %f, %f, %f, %f", rect.left, rect.top, rect.right, rect.bottom);
#endif
			//Create another blue windows:
			//m_memDC.FillSolidRect(rect.right,rect.top,rect.right+(rect.right-rect.left),rect.bottom,RGB(0,0,255));
			//Copy blue windows to screen:
			//m_memDC.BitBlt( rect.left, rect.top, rect.Width(), rect.Height(), &m_memDC, rect.right, rect.bottom,SRCCOPY );


			//bResetMemDC = false;
			
			//Initialize Direct2D objects:
			//if(first){
				pdcHandler->Initialize(&m_memDC,&rect);
				//first=false;
			//}


		}
		catch(...)
		{
		}
	}

  //1 Mar 2008 ER-start
  //reset internal memdc
  //m_memDC.FillSolidRect(rect, RGB(255, 255, 255));

	


  //1 Mar 2008 ER-end

	// Calculate the x grid
	// Note that drawing the x grid is very
	// expensive, as the price style series must 
	// be drawn twice since the calendar depends
	// on the output of the price style, and the
	// x grid must be drawn below all series.
	styleSeries = NULL;
  int n;
	if(showXGrid && !IgnorePaint)
	{
		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int s = 0; s != panels[n]->series.size(); ++s){
					if(panels[n]->series[s]->seriesType == OBJECT_SERIES_STOCK ||
						panels[n]->series[s]->seriesType == OBJECT_SERIES_CANDLE ||
						panels[n]->series[s]->seriesType == OBJECT_SERIES_STOCK_HLC){
						CString name = panels[n]->series[s]->szName;
						name.MakeLower();
						if(name.Find(".close") != -1){
							styleSeries = panels[n]->series[s];
							goto buildXGrid;
						}
					}
				}
			}
		}

buildXGrid:		
		if(styleSeries != NULL){ // 10/12/04 RG
			styleSeries->OnPaint(&m_memDC);
		}

		calendar.OnPaint(&m_memDC, this);
	}


	// Send paint message to chart panels
	//CalculateScaleInfo();

 
  //1 Mar 2008 ER-start

// 	bool	inv	= false;
// 	for( n = 0; n != panels.size(); ++n )
// 	{
// 		inv	= false;
// 		if(panels[n]->visible)
// 		{
// 			if( ! m_xor )
// 			{
// 				inv	= panels[n]->invalidated;
// 				
// 				panels[n]->OnPaint( &m_memDC );
// 				
// 				if(inv)
// 				{
// 					FirePaint( n );
// 				}				
// 			}
// 			
// 			panels[n]->OnPaintXOR( &m_memDC );
// 		} 
// 	}
	
	if(IgnorePaint && !bResetMemDC) {
#ifdef _CONSOLE_DEBUG
//printf("\t IGNORED");
#endif
			IgnorePaint = false;
	}
	else{
		 if(IgnorePaint)	IgnorePaint = false;
		  for (n = 0; n < panels.size(); ++n)
		  {
			//force panel repaint, no need here to send message, cause the entire chart gets repainted here
			if (panels[n]->visible)
			{
			  if(bResetMemDC && !pdcHandler->IsClipRegion)
			  {
				  bResetMemDC = false;
				  panels[n]->InvalidateXOR();
			  }
			  panels[n]->invalidated = true;
			  panels[n]->OnPaint( &m_memDC );
			  if(!pdcHandler->IsClipRegion)panels[n]->OnPaintXOR( &m_memDC );
			  FirePaint( n ); // Added back 3/6/2008
			}
		  }
	}
  //1 Mar 2008 ER-end
	

	// Draw the calendar
	calendar.OnPaint( &m_memDC, this );




#pragma region LOGO	
	if (ShowLogo){
#ifdef _CONSOLE_DEBUG
		printf("\t SHOWLOGO()");
#endif
		CRect bannerRect;		
		bannerRect.top = 0;
		bannerRect.bottom = panels[0]->y2;
		if(yAlignment == RIGHT){
			bannerRect.left = 0;
			bannerRect.right = width - yScaleWidth;
		}
		else{
			bannerRect.left = yScaleWidth;
			bannerRect.right = width;
		}


		CFont newFont;	
		newFont.CreatePointFont(200, _T("Arial"), &m_memDC);	
		CFont* pOldFont = m_memDC.SelectObject(&newFont);
		CBrush*	oldBrush	= (CBrush*)m_memDC.SelectStockObject( LTGRAY_BRUSH );
		m_memDC.SetTextColor( RGB( 255,0,0 ) );	
		CString s = "StockChartX TRIAL VERSION";
		m_memDC.SelectObject( oldBrush );
		m_memDC.SelectObject(pOldFont);

		newFont.DeleteObject();
		newFont.CreatePointFont(80, _T("Arial"), &m_memDC);	
		pOldFont = m_memDC.SelectObject(&newFont);
		oldBrush = (CBrush*)m_memDC.SelectStockObject( LTGRAY_BRUSH );
		m_memDC.SetTextColor( RGB( 0,0,0 ) );		
		s = "Register online at http://www.modulusfe.com or by calling (888) 318-3754";
		//pdcHandler->DrawImage("",CRectF(bannerRect.left+15,height-80,bannerRect.right,height),&m_memDC);

		//Draw background logo: Size=518x250
		double scale = 1.0;
		//Adjust scale to window:
		int difX = width-518;
		int difY = height - 250;

#ifdef _CONSOLE_DEBUG
		printf(" W=%d H=%d dW=%d dY=%d",width,height,difX,difY);
#endif

		//Adjust by both
		if (difX < 0 && difY < 0)
		{
#ifdef _CONSOLE_DEBUG
			printf("\n\tBoth");
#endif
			if (difX <= difY)
			{
				scale = (double)(518.0 + difX - 80.0) / 518.0;
			}
			else
			{
				scale = (double)(250.0 + difY - 10.0) / 250.0;
			}
		}
		//Adjust by Y
		else if (difY < 0)
		{
#ifdef _CONSOLE_DEBUG
			printf("\n\tY");
#endif
			scale = (double)(250.0 + difY - 10.0) / 250.0;

		}
		//Adjust by X
		else if (difX < 0)
		{
#ifdef _CONSOLE_DEBUG
			printf("\n\tX");
#endif
			scale = (double)(518.0 + difX - 80.0) / 518.0;
		}

#ifdef _CONSOLE_DEBUG
		printf(" Scale = %f",scale);
#endif
		pdcHandler->DrawImage("", CPointF(bannerRect.left + (bannerRect.right-bannerRect.left)/2-259*scale, bannerRect.top+(bannerRect.bottom-bannerRect.top)/2-125*scale),scale, &m_memDC);
				
		/*
		//That's on ChartPanel::OnPaint()
		s = panels[0]->series[0]->szName;
		if (s.Find(".open")>0)s.Replace(".open", "");
		//External title sufix:
		s += " "+m_SaveImageTitle;
		switch(GetPeriodicity())
		{
			case Minutely:	
				s="\n\n\n\n   "+s+" (MI)";
				break;
			case Hourly:
				s="\n\n\n\n   "+s+" (H)";
				break;
			case Daily:	
				s="\n\n\n\n   "+s+" (D)";
				break;
			case Weekly:	
				if(m_Language==1)s="\n\n\n\n   "+s+" (S)";
				else s="\n\n\n\n   "+s+" (W)";
				break;
			case Month:			
				if(m_Language==1)s="\n\n\n\n   "+s+" (ME)";
				else s="\n\n\n\n  "+s+" (MO)";
				break;
			case Year:		
				if(m_Language==1)s="\n\n\n\n   "+s+" (A)";
				else s="\n\n\n\n   "+s+" (Y)";
				break;
		}
		int opacity=128;
		if(panels[0]->series[0]->lineColor==RGB(255,255,255))opacity = 40;
		pdcHandler->DrawText(s,CRectF(bannerRect.left+10,bannerRect.bottom,bannerRect.right,height),"Arial Rounded MT",40.0F,DT_CENTER,RGB(128,128,128),opacity,&m_memDC);
		*/

		m_memDC.SelectObject( oldBrush );
		m_memDC.SelectObject(pOldFont);
		
	



  }
#pragma endregion


  //3/1/08 ER-start
  if(userZooming && startUserZooming > -1)
  {
    CBrush *pOldBrush = m_memDC.SelectObject(&zoomingBrush);
    m_memDC.PatBlt(oldUserRect.left, oldUserRect.top, oldUserRect.Width(), oldUserRect.Height(), PATINVERT);
    m_memDC.SelectObject(pOldBrush);

  }
  

  //1 Mar 2008 ER-end


	// BitBlt the buffer bitmap to the screen using the memory dc	
	//CBitmap*	oldBmp2 = pdc->SelectObject( &m_bitmap );
	if(!pdcHandler->IsClipRegion)pdc->BitBlt( rect.left, rect.top, rect.right, rect.bottom, &m_memDC, rect.left, rect.top, SRCCOPY );
	else pdc->BitBlt( GetX(endIndex-2)-GetX(startIndex), rect.top, rect.right, rect.bottom, &m_memDC, GetX(endIndex-2)-GetX(startIndex), rect.top, SRCCOPY );
	//if( oldBmp2 )
	//	pdc->SelectObject(&oldBmp2);
	
	ResumeMeasure(m_point);
	ResumeCrossHairs(m_point);


}
/////////////////////////////////////////////////////////////////////////////

int CStockChartXCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nOnCreate()");
#endif
	 if(!IsLicenseValid()) // Added 8/28/07
	 {
		 DestroyAll();
		 return -1;
	 }

	if (COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	if(loaded) return 0;

	panels.resize(MAX_PANELS);
  int n;
	for(n = 0; n != MAX_PANELS; ++n){
		panels[n] = new CChartPanel();
		panels[n]->Connect(this);
	}
	
	dialogErrorShown = false;
	nSpacing = 0;
	themeOffset = 0;
	m_currentPanel = 0;
	wickColor = RGB(0,255,0);
	textAreaFontSize = m_textAreaFontSize = DEFAULT_FONT_SIZE;
	textAreaFontName = m_textAreaFontName = _T("Arial");
	barColorName = "";
	yScaleMinTick = m_yScaleMinTick = 0.25;
	m_yScaleDrawn = false;
	typing = false;
	candleUpOutlineColor = -1;
	candleDownOutlineColor = -1;
	wickDownColor = -1;
	wickUpColor = -1;
	m_mousePointer = 0; //mpNormal
	swapTo = -1;
	m_onRClickFired = false;
	updatingIndicator = false;
	m_maxDisplayRecords = 0;
	m_barInterval = 60;
	barStartTime = 0;
	m_barStartTime = 0;
	oldBmp = NULL;
	xCount = 0;
	styleSeries = NULL;
	xGridDrawn = false;
	realTime = false;
	darvasPct = 0.01;
	priceStyle = psStandard;
	priceStyleParams.resize(10);
	threeDStyle = false;
	locked = false;
	loading = false;
	barWidth = 1; // Candle bar width
	barColors.resize(0);
	yOffset = 0;
	displayTitles = false;
	recordLabels = false;
	horzLineColor = RGB(255,255,255);
	m_version = "StockChartX Professional Version 5.9.6.10 compiled Wednesday, September 8th, 2010";
	version = m_version;
	darvasBoxes = false;
	pause = false;	
	m_crossHairs = false;
	m_Magnetic = false;
	m_horzLines = false;
	swapping = false;
	m_setcap = false;	
	useLineSeriesColors = false;
	useVolumeUpDownColors = false;
	displayInfoText = true;
	lineDrawing = false;
	buildingChart = false;
	reCalc = false;
	m_drawingType = 0;
	textDisplayed = false;
	m_lastStartIndex = 0;
	barSpacing = 0;
	dx = -1;
	activePanel = panels[0];	
	changed = false;
	extendedXPixels = 10;
	decimals = 2;
	m_point = CPoint(0,0);	
	oldUserRect.top = -1;
	endUserZooming = -1;
	startUserZooming = -1;
	yAlignment = RIGHT;
	spaceInterval = 0;
	yScaleWidth = 60;
	scalingType = LINEAR;
	showYGrid = true;
	showXGrid = false;
	m_Cursor = 0;
	flag = false;
	dragFlag = false;
	dragging = false;
	movingObject = false;
	objectSelected = false;
	m_buttonState = MOUSE_UP;
	resizing = false;	
	bResetMemDC = true;
	m_mouseState = 1;
	m_xor = false;
	drawing = false;
	userZooming = false;
	focus = true;
	startX = 0;
	startY = 0;
	m_symbol = "";	
	m_valueView = new CValueView();
	m_valueView->Connect(this);
	m_expired = false;
	m_frozen = false;

	startIndex = 0;
	endIndex = 0;

	
	lineColor = RGB(0,0,255); 
	lineWeight=1;

	m_extraTimePrecision = FALSE;


	CDC* pdc = GetDC();
	ASSERT(pdc != NULL);
	CRect rect;
	pdc->GetWindow()->GetClientRect(&rect);
	height = rect.bottom - CALENDAR_HEIGHT;
	width = rect.right;
	ReleaseDC(pdc);


	EnumIndicatorMap();


	
	loaded = true;


	pdcHandler->Setup();


#ifdef DEMO_MODE

		CDialogReminder* pBuyNow= new CDialogReminder();
		pBuyNow->Create(IDD_REMINDER,this);
		
		WINDOWPLACEMENT wp;

	    wp.length = sizeof(WINDOWPLACEMENT);
		wp.flags = 0;
		wp.showCmd = SW_SHOW;

		pBuyNow->ShowWindow(SW_SHOW);
		pBuyNow->CenterWindow();

		MSG msg;
		while(!pBuyNow->m_ok){
			while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
			Sleep(1);
		};

		m_expired = pBuyNow->m_expired;

		if(pBuyNow != NULL){	
			delete pBuyNow;
		}

#endif





	return 0;
}

bool CStockChartXCtrl::CompareNoCase(CString string1, CString string2)
{
	return string1.CompareNoCase(string2) == 0;
}

void CStockChartXCtrl::OnSize( UINT nType, int cx, int cy ) 
{
//#ifdef _CONSOLE_DEBUG
//	printf("\nOnSize()");
//#endif
	COleControl::OnSize( nType, cx, cy );

	int	nHeight	= cy - CALENDAR_HEIGHT;
	int	nWidth	= cx;


	// Exit if too small
	if(nHeight < 1)
		return;

	
	
	// Exit if only one panel is visible
	if( GetVisiblePanelCount() < 2 )
	{
		panels[0]->y2	= nHeight;
		height	= nHeight;
		width	= nWidth;
		UpdateScreen(true);
		return;
	}
	/*else { 
			double oldHeight	= (double)height;
			double scale		= ((double)panels[1]->y1 / oldHeight);
			panels[1]->y1		= (double)nHeight * scale;
			panels[0]->y2	= panels[1]->y1;
	}*/

	
	// Resize all panels to fit available height
	int	panelsCount	= panels.size();

	for(int n=1;n<panelsCount;n++){
		double oldHeight	= (double)height;
		double scale		= ((double)panels[n]->y1 / oldHeight);
		panels[n]->y1		= (double)nHeight * scale;
		panels[n-1]->y2	= panels[n]->y1;
	}
	panels[GetBottomChartPanel()]->y2 = nHeight;

	/*
	int	n = 0;	
	for( n = 0; n < panelsCount; n++ )
	{
		CChartPanel&	panel	= *(panels[n]);
		if( panel.visible )
		{
			double oldHeight	= (double)height;
			double scale		= ((double)panel.y2 / oldHeight);
			panel.y2			= (double)nHeight * scale;
		}
	}

	if( panels[GetBottomChartPanel()]->y2 < nHeight - 2 )
	{
		panels[GetBottomChartPanel()]->y2 = nHeight;
	}
 
	
	// Update y1 for each panel	
	for( n = 1; n < panelsCount; n++ )
	{
		CChartPanel&	panel	= *(panels[n]);
		if( panel.visible )
		{
			panel.y1 = panels[n-1]->y2;
		}
	}
	*/

	// Must remain at bottom of OnSize function
	height	= nHeight;
	width	= nWidth;
#ifdef _CONSOLE_DEBUG
		//printf("\n\nRefresh Width = %d height = %d", width, height);
#endif
	UpdateScreen(true);	

}

BOOL CStockChartXCtrl::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message) 
{

	if(m_mousePointer == 1){ //mpHourGlass
		SetCursor(AfxGetApp()->LoadStandardCursor(IDC_WAIT));
		return TRUE;
	}
	else if(m_mousePointer == 2){ //mpIndicator
		SetCursor(AfxGetApp()->LoadCursor(IDC_CLOSED_HAND_NEW));		
		return TRUE;
	}


	if(m_Cursor > 0){
		// Dragging something?		
		if(dragging){			
			// Create a new panel?
			if(m_point.y > height){
				m_Cursor = IDC_CLOSED_HAND_NEW;
			}
			// Have we ventured outside of the active panel?
			else if(m_point.y > activePanel->y2 || m_point.y < activePanel->y1){
				m_Cursor = IDC_CLOSED_HAND_ADD;
			}
			// Nothing special going on:
			else{	
				m_Cursor = IDC_CLOSED_HAND;		
			}
		}
		if(m_Cursor > 0){
			SetCursor(AfxGetApp()->LoadCursor(m_Cursor));
		}
		else{
			SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
		}
	}
	else{
		SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
	}
	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// Sends mousemove messages to chart panels and manages dragrect drawing
void CStockChartXCtrl::OnMouseMove(UINT nFlags, CPoint point) 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nOnMouseMove()");
#endif

	bool pointRounded = true;

	//TODO: Always set mouse cursor to snap on round values:
	/*if (pointRounded){
		int panel = 0;
		for (int n = 0; n != panels.size(); ++n){
			if (panels[n]->visible){
				if (point.y > panels[n]->y1 &&
					point.y < panels[n]->y2 ){
					panel = n;
					break;
				}
			}
		}
		int yValue = panels[panel]->GetReverseY((double)point.y);
#ifdef _CONSOLE_DEBUG
		printf("\n\tCurrent Panel = %d Value = %f", panel, yValue);
#endif
	}*/


	if(lastMouse==point) return;
	lastMouse=point;

	IgnorePaint = true;


	if(locked) {



		return; 
	}
	if(panels[0]->series.size() < 1 ||
		panels[0]->series[0]->data_slave.size() < 1){



		return; 
	}

	if(pause) {



		return; 
	}
	
	 SuspendCrossHairs();
	


	// Save locations
	m_prevPoint = m_point;
	m_point = point;
	m_MousePositionX=point.x;
	m_MousePositionY=point.y;



	// This is the appropriate place to check for stuck series pointers
	if( SelectCount() == 0 && !userZooming && !resizing && !dragging && !movingObject && !lineDrawing)
	{
#ifdef _CONSOLE_DEBUG
		//printf("  RESET CURSOR!");
#endif
		m_Cursor = 0;
		m_mouseState = MOUSE_NORMAL;
	}


	if(m_buttonState == MOUSE_DOWN && dx == -1)
	{
#ifdef _CONSOLE_DEBUG
		printf("  DRAGGING!");
#endif
		dragFlag = true;
		dx = m_prevPoint.x;
	}

	if(panels[0]->visible)
	{
		
//	Revision 6/10/2004
//	type cast of int
		long record = 0;
		if(GetRecordCount() > 0)
			record = (long)panels[0]->GetReverseX(point.x);
		else
			record = 0;
		if(record < 0) record = 0;
//	End Of Revision
		int maxRecords = panels[0]->series[0]->data_slave.size();
		if( record < maxRecords ) // Changed from <= 2/6/05 JG
		{
			//TRACE("%d\n", record);
			double jdate = panels[0]->series[0]->data_slave[record].jdate;
			//TRACE("%f\n", jdate);
//	Revision 6/10/2004
//	type cast of int
			record = (long)panels[0]->series[0]->GetMasterRecordIndex(jdate);
//	End Of Revision
			if(record == NULL_VALUE)
			{
				record = -1;
			}
			else
			{
				record += startIndex;
			}
		}
		FireMouseOver(point.x, point.y, record + 1);
	}


	if (userZooming && startUserZooming > -1)
	{
		endUserZooming = point.x;
		CDC* pDC = GetDC();
		ASSERT(pDC != NULL);
		newUserRect.top = 0;
		newUserRect.bottom = height;		
		newUserRect.left = startUserZooming;
		newUserRect.right = endUserZooming;

		if(yAlignment == LEFT){
			if(newUserRect.left < yScaleWidth){
				newUserRect.left = yScaleWidth;
			}
			if(newUserRect.right < yScaleWidth){
				newUserRect.right = yScaleWidth;
			}
		}
		else{
			if(newUserRect.left > width - yScaleWidth){
				newUserRect.left = width - yScaleWidth;
			}
			if(newUserRect.right > width - yScaleWidth){
				newUserRect.right = width - yScaleWidth;
			}
		}

		CBrush* br = new CBrush(RGB(0,0,255));
		pDC->DrawDragRect(newUserRect,CSize(newUserRect.Width(),
						  newUserRect.Height()),oldUserRect,
						  CSize(oldUserRect.Width(),oldUserRect.Height()),
						  br);
		delete br;
		oldUserRect = newUserRect;
		ReleaseDC(pDC);
		ResumeCrossHairs(point);
		return;
	}



	// If the mouse is over a horizontal divide, show the split mouse pointer
  int n;
	if(! resizing && GetVisiblePanelCount() > 1)
	{
		bool bSplit = false;
		int bottom = GetBottomChartPanel();

		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				if(point.y > panels[n]->y2 - 3 && 
					point.y < panels[n]->y2 + 3 &&
					n != bottom){
					bSplit = true;
					break;
				}
			}
		}
		if(bSplit){
			if(m_mouseState == MOUSE_NORMAL){
				m_Cursor = IDC_SPLIT;
			}
		}
		else{
			if(m_mouseState == MOUSE_NORMAL){
				m_Cursor = 0;
			}
		}
	}

	
	// Draw drag line
	if(resizing)
	{
		CClientDC dc(this);
		CRect rect1;
		if(m_point.y<panels[m_index]->y1+15)m_point.y = panels[m_index]->y1+15;
		else if(m_point.y>panels[m_index+1]->y2-15)m_point.y = panels[m_index+1]->y2-15;
		rect1.top = m_point.y - 1;
		rect1.bottom = m_point.y + 1;
		rect1.left = 0;
		rect1.right = width;
		CRect rect2;
		rect2.top = m_prevPoint.y + 1;
		rect2.bottom = m_prevPoint.y - 1;
		rect2.left = 0;
		rect2.right = width;
		dc.DrawDragRect(rect1,CSize(2,2),rect2,CSize(2,2));

		DeleteDC(dc);
	}


	// Send MouseMove event to proper chart panel
	for(n = 0; n != panels.size(); ++n)
	{
		if(panels[n]->visible){
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				m_currentPanel = n; // cache panel mouse was over last
				panels[n]->OnMouseMove(point);
			}
		}
	}



	// Logic changed 10/3/04 BJ
	// Draw value panel, if appropriate

	if(displayInfoText && !resizing && m_mouseState == MOUSE_NORMAL && 
		m_buttonState == MOUSE_DOWN && !movingObject)
	{
		//if( point.x)
		m_valueView->x1 = point.x + 10;
		m_valueView->y1 = point.y + 21;
						
	}

	ResumeCrossHairs(point);
	//COleControl::OnMouseMove(nFlags, point);
}

void CStockChartXCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{	

	if(locked) return;
	//UnSelectAll(); // Unselect everything else

	FireOnLButtonDown();

	/*** FROEDE_MARK ... Keep the crosshair in the current state...
	if(m_crossHairs){
		CDC* pDC = GetScreen();
		pDC->SetROP2(R2_NOT);
		pDC->MoveTo(oldVRect.left,oldVRect.top);
		pDC->LineTo(oldVRect.right,oldVRect.bottom);
		pDC->MoveTo(oldHRect.left,oldHRect.top);
		pDC->LineTo(oldHRect.right,oldHRect.bottom);
		pDC->SetROP2(R2_COPYPEN);		
		ReleaseScreen(pDC);
		m_crossHairs = false;
		oldHRect = CRect(-1,-1,-1,-1);
		oldVRect = CRect(-1,-1,-1,-1);
		Update();
	}

	*/
	SuspendMeasure();
	SuspendCrossHairs();

	if(!focus) GetCtrlFocus();

	if(pause) return;	


	m_buttonState = MOUSE_DOWN;

	if(m_Cursor == IDC_TEXT || m_Cursor == IDC_ZOOMIN) return;


  int n;
	if(m_Cursor == IDC_PENCIL){
		UnSelectAll();
		if(!lineDrawing){
			
			//Just started drawing line object
			int toPanel = 0;
			for(n = 0; n != panels.size(); ++n){
				if(panels[n]->y1 < m_point.y && panels[n]->y2 > m_point.y){
					toPanel = n;
				}
			}
			long record,record_area;

			switch(m_drawingType){
			
			case lsTrendLine:
				panels[toPanel]->lines.push_back(new CLineStandard(lineColor, m_key, panels[toPanel]));
				break;

			case lsEllipse:
				panels[toPanel]->lines.push_back(new CLineStudyEllipse(lineColor, m_key, panels[toPanel]));
				break;

			case lsRectangle:
				panels[toPanel]->lines.push_back(new CLineStudyRectangle(lineColor, m_key, panels[toPanel]));
				break;

			case lsTriangle:
				panels[toPanel]->lines.push_back(new CLineStudyTriangle(lineColor, m_key, panels[toPanel]));
				break;

			case lsPolyline:
				panels[toPanel]->lines.push_back(new CLineStudyPolyline(lineColor, m_key, panels[toPanel]));
				break;

			case lsSpeedLines:
				panels[toPanel]->lines.push_back(new CLineStudySpeedLines(lineColor, m_key, panels[toPanel]));
				break;

			case lsGannFan:
				panels[toPanel]->lines.push_back(new CLineStudyGannFan(lineColor, m_key, panels[toPanel]));
				break;

			case lsFibonacciArcs:
				panels[toPanel]->lines.push_back(new CLineStudyFibonacciArcs(lineColor, m_key, panels[toPanel]));
				break;

			case lsFibonacciFan:
				panels[toPanel]->lines.push_back(new CLineStudyFibonacciFan(lineColor, m_key, panels[toPanel]));
				break;

			case lsFibonacciRetracements:
				panels[toPanel]->lines.push_back(new CLineStudyFibonacciRetracements(lineColor, m_key, panels[toPanel]));
				break;

			case lsFibonacciProgression:
				panels[toPanel]->lines.push_back(new CLineStudyFibonacciProgression(lineColor, m_key, panels[toPanel]));
				break;
				
			case lsFibonacciTimeZones:
				panels[toPanel]->lines.push_back(new CLineStudyFibonacciTimeZones(lineColor, m_key, panels[toPanel]));
				break;

			case lsTironeLevels:
				panels[toPanel]->lines.push_back(new CLineStudyTironeLevels(lineColor, m_key, panels[toPanel]));
				break;

			case lsQuadrantLines:
				panels[toPanel]->lines.push_back(new CLineStudyQuadrantLines(lineColor, m_key, panels[toPanel]));
				break;

			case lsRaffRegression:
				panels[toPanel]->lines.push_back(new CLineStudyRaffRegression(lineColor, m_key, panels[toPanel]));
				break;

			case lsErrorChannel:
				panels[toPanel]->lines.push_back(new CLineStudyErrorChannel(lineColor, m_key, panels[toPanel]));
				break;

			case lsFreehand:
				panels[toPanel]->lines.push_back(new CLineStudyFreehand(lineColor, m_key, panels[toPanel]));
				break;

			case lsChannel:
				panels[toPanel]->lines.push_back(new CLineStudyChannel(lineColor, m_key, panels[toPanel]));
				break;
				
			case lsRay:
				panels[toPanel]->lines.push_back(new CLineStandard(lineColor, m_key, panels[toPanel], true));
				break;
				
			case lsXLine:
					try{		
						panels[toPanel]->lines.push_back(new CLineStandard(lineColor, m_key, panels[toPanel]));
						currentLine = panels[toPanel]->lines[panels[toPanel]->lines.size() - 1];
						currentLine->valuePosition = -1;
						currentLine->Connect(this);
		
						currentLine->x1Value = GetXRecordByPixel(point.x);
						if(m_Magnetic && toPanel==0){
							currentLine->MagneticPointYValue((double)point.y,(double)point.x);
							currentLine->y1Value = currentLine->MagneticPointYValue((double)point.y,(double)point.x);
							currentLine->y2Value = currentLine->MagneticPointYValue((double)point.y,(double)point.x);
						}
						else{
							currentLine->y1Value = GetYValueByPixel(point.y);
							currentLine->y2Value = GetYValueByPixel(point.y);
						}
						currentLine->x2Value = NULL_VALUE;
						currentLine->x1Value_2 = GetXRecordByPixel(point.x);
						
						MMDDYYHHMMSS x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
						MMDDYYHHMMSS x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
						switch(GetPeriodicity())
						{
							case Minutely:	
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								//x2Julian.Second -= 1;
								//x2Julian_2.Second -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Hourly:	
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Minute -= 1;
								x2Julian_2.Minute -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Daily:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Hour -= 1;
								x2Julian_2.Hour -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Weekly:		
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Month:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Year:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
						}
						currentLine->hFixed = true;
						currentLine->SnapLine();
					}
					catch(...){
						ThrowError( CUSTOM_CTL_SCODE(1003), "Failed to add trendline" );
					}
					UpdateScreen(true);
					
					
					lineDrawing = false;
					currentLine->drawing = false;		
					currentLine->drawn = true;
					m_mouseState = MOUSE_NORMAL;
					m_Cursor = 0;						
					OnUserDrawingComplete(lsTrendLine, m_key);
					SaveUserStudies();
					FireClick(); 
					return;

			case lsYLine:
					record_area = panels[toPanel]->panelRect.right-163;
					//long record = panels[toPanel]->series[0]->data_master.size()*point.x/record_area;
					record = (endIndex-startIndex)*point.x/record_area;
					record+=startIndex;
					record+=1;
					record = GetXRecordByPixel(point.x);
					try{
						panels[toPanel]->lines.push_back(new CLineStandard(lineColor, m_key, panels[toPanel]));
						currentLine = panels[toPanel]->lines[panels[toPanel]->lines.size() - 1];
						currentLine->Connect(this);
		
						currentLine->x1Value = (double)record;
						currentLine->y1Value = NULL_VALUE;
						currentLine->x2Value = (double)record;
						currentLine->y2Value = NULL_VALUE;
						
						MMDDYYHHMMSS x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
						MMDDYYHHMMSS x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
						switch(GetPeriodicity())
						{
							case Minutely:	
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								//x2Julian.Second -= 1;
								//x2Julian_2.Second -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Hourly:	
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Minute -= 1;
								x2Julian_2.Minute -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Daily:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Hour -= 1;
								x2Julian_2.Hour -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Weekly:		
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;	
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Month:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
							case Year:			
								x2Julian = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value));
								x2Julian_2 = CJulian::FromJDate(panels[toPanel]->series[0]->GetJDate((int)currentLine->x2Value_2));
								x2Julian.Day -= 1;
								x2Julian_2.Day -= 1;
								currentLine->x1JDate = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value-1);
								currentLine->x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
								currentLine->x1JDate_2 = panels[toPanel]->series[0]->GetJDate((int)currentLine->x1Value_2-1);
								currentLine->x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
								break;
						}
						currentLine->vFixed = true;
						currentLine->SnapLine();

					}
					catch(...){
						ThrowError( CUSTOM_CTL_SCODE(1003), "Failed to add trendline" );
					}
					UpdateScreen(true);
					
					
					lineDrawing = false;
					currentLine->drawing = false;		
					currentLine->drawn = true;
					m_mouseState = MOUSE_NORMAL;
					m_Cursor = 0;						
					OnUserDrawingComplete(lsTrendLine, m_key);
					SaveUserStudies();
					FireClick(); 
					return;

			}

			currentLine = panels[toPanel]->lines[panels[toPanel]->lines.size() - 1];
			currentLine->Connect(this);
			lineDrawing = true;
			currentLine->XORDraw(1, point);	
			

		} else{
			

			//TODO: give this part a better look
			// Finished drawing line object 
			// That is not quite true, there are some studys that demand a third point...	
				
			
			/*if(!(currentLine->nType == lsChannel || currentLine->nType == lsFibonacciProgression || currentLine->nType == lsPolyline)){
				lineDrawing = false;
				m_mouseState = MOUSE_NORMAL;
				m_Cursor = 0;				
				currentLine->XORDraw(3, point);
			} 
			else {
				if(currentLine->state == 2){
					lineDrawing = false;
					m_mouseState = MOUSE_NORMAL;
					m_Cursor = 0;	
				}		
				else if(currentLine->nType != lsChannel && currentLine->nType != lsFibonacciProgression) currentLine->XORDraw(3, point);
			}	
			if(currentLine->nType == lsChannel || currentLine->nType == lsFibonacciProgression)currentLine->XORDraw(3, point);*/
		}
		FireClick(); // 7/20/08
		//ResumeCrossHairs(point);
		return;
	}


	// If resizing, record index of chart panel
	if(m_Cursor == IDC_SPLIT && !resizing){
		resizing = true;
		m_index = GetNextHigherChartPanel(point.y - 5);
		if(m_index == -1){
			m_index = GetTopChartPanel();
		}
	}
	
	
	
	// Send event to all chart panels
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				panels[n]->OnLButtonDown(point);
				activePanel = panels[n];
			}
		}
	}	
 

	// Get start Point for Value View and save on xValue/yValue
	if(/*displayInfoText*/DeltaCursor && !drawing && !objectSelected && activePanel->index==0 && m_mouseState == MOUSE_NORMAL){
		m_valueView->x1Value = activePanel->GetReverseX(point.x) + 1 + startIndex;
		m_valueView->y1Value = activePanel->GetReverseY(point.y);
		m_valueView->x2Value = (double)point.x;
		m_valueView->y2Value = (double)point.y;
		
		m_pauseMeasure = false;
		/*activePanel->lines.push_back(new CLineStandard(lineColor, m_key, activePanel));
		CCOL* currentLine =activePanel->lines[activePanel->lines.size() - 1];
		currentLine->Connect(this);
		lineDrawing = true;
		currentLine->XORDraw(1, point);*/




	}

	objectSelected = false;

	
	FireClick(); // Moved from above on 10/26/04 by RG - DO NOT MOVE

	//ResumeCrossHairs(point);	
	//ResumeMeasure(point);
	COleControl::OnLButtonDown(nFlags, point);
}

void CStockChartXCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{	
	if(locked) return;

	FireOnLButtonUp();

	if(pause) return;
#ifdef _CONSOLE_DEBUG
	printf("\nOnLButtonUp()");
#endif

	if(m_setcap) ::ReleaseCapture();

	SuspendCrossHairs();
	SuspendMeasure();
	m_valueView->x1Value=m_valueView->y1Value=m_valueView->x2Value=m_valueView->y2Value=NULL_VALUE;

	bool  wasDrawing = false;
	
	if(m_Cursor == IDC_PENCIL && lineDrawing){
		if(!(currentLine->nType == lsChannel || currentLine->nType == lsFibonacciProgression || currentLine->nType == lsPolyline)){
				lineDrawing = false;
				m_mouseState = MOUSE_NORMAL;
				m_Cursor = 0;				
				currentLine->XORDraw(3, point);
#ifdef _CONSOLE_DEBUG
				printf(" XORDraw(3a)");
#endif
			} 
			else {
				if(currentLine->state == 2){
					lineDrawing = false;
					m_mouseState = MOUSE_NORMAL;
					m_Cursor = 0;
#ifdef _CONSOLE_DEBUG
					printf(" state = 2");
#endif
				}		
				else if (currentLine->nType != lsChannel && currentLine->nType != lsFibonacciProgression) {
					currentLine->XORDraw(3, point);
#ifdef _CONSOLE_DEBUG
					printf(" XORDraw(3b)");
#endif
				}

			}	
			if (currentLine->nType == lsChannel || currentLine->nType == lsFibonacciProgression){
				currentLine->XORDraw(3, point);
#ifdef _CONSOLE_DEBUG
				printf(" XORDraw(3c)");
#endif
			}
			wasDrawing = true;;
	}

  int n;
	if(m_mouseState == MOUSE_SYMBOL){
		CChartPanel* panel= NULL;
		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				if(point.y > panels[n]->y1 && 
					point.y < panels[n]->y2){					
					panel = panels[n];
					break;
				}
			}
		}

		double value = 0;
		if(panel) 
			value = panel->GetReverseY(point.y);
		// BUG FIX 9/18/04 RG:
		// "value" was not being set here.
		// Must have been Katchei who did this on 6/10/04

//	Revision 6/10/2004
//	type cast of int
		int record = (int)panel->GetReverseX(point.x) + startIndex + 1;
//	End Of Revision
		dragging = false;
		movingObject = false;
		m_mouseState = MOUSE_NORMAL;
		m_Cursor = MOUSE_NORMAL;
		AddSymbolObject(panel->index, value, record, 
						m_symbolType, m_symbolName, m_symbolText);
		m_buttonState = MOUSE_UP;		
		m_valueView->Reset(true);
		COleControl::OnLButtonUp(nFlags, point);
		Update();
		SaveUserStudies();
		ResumeCrossHairs(point);
		return;
	}
	
	if(userZooming && endUserZooming > 0){
		// Got the selection
		m_Cursor = MOUSE_NORMAL;
		int x = endUserZooming;
		if(endUserZooming < startUserZooming){
			x = startUserZooming;
			startUserZooming = endUserZooming;
			endUserZooming = x;
		}
//	Revision 6/10/2004
//	type cast of int		
		int end = endIndex;
		end = (int)panels[0]->GetReverseX(endUserZooming) + startIndex;
		startIndex = (int)panels[0]->GetReverseX(startUserZooming) + startIndex;
		endIndex = end;
//	End Of Revision

		if(endIndex - startIndex < MIN_VISIBLE){			
			endIndex = startIndex + MIN_VISIBLE;
		}
		if(endIndex > RecordCount()){
			endIndex = RecordCount();
		}
		startUserZooming = -1;
		endUserZooming = -1;
		oldUserRect.top = 0;
		oldUserRect.left = 0;
		oldUserRect.right = 0;
		oldUserRect.bottom = 0;
		dragging = false;
		userZooming = false;
		m_mouseState = MOUSE_NORMAL;
		m_Cursor = 0;
		SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
		FireZoom();
		Update();		
		
	}
	else if(userZooming){
		// Start selection;
		startUserZooming = point.x;
		return;
	}

	m_buttonState = MOUSE_UP;
	dragging = false;

	m_valueView->Reset(true);

	SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));

	if(m_Cursor == IDC_TEXT){
		int p = -1;		
		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->y1 < point.y && panels[n]->y2 > point.y){
				p = n;
			}
		}
		if(p != -1){
			panels[p]->textAreas.push_back(new CTextArea(point.x,point.y, panels[p]));
			panels[p]->textAreas[panels[p]->textAreas.size() - 1]->key = m_key;
			m_Cursor = 0;
			m_mouseState = MOUSE_NORMAL;
			movingObject = false;			
		}
		SaveUserStudies();
	}

	// Resize two chart panels if resizing.
	if(resizing){
		resizing = false;
		m_Cursor = 0;
		
		if(m_index != -1){
			if(point.y > height - CALENDAR_HEIGHT){
				point.y = height - CALENDAR_HEIGHT;
			}/*			
			if(panels[m_index + 2]->visible){
				if(point.y > panels[m_index + 2]->y1){
//	Revision 6/10/2004
//	type cast of int
					point.y = (LONG)panels[m_index + 2]->y1 - 2;		
//	End of Revision
				}
			}*/
			
			if(point.y<panels[m_index]->y1+15)point.y = panels[m_index]->y1+15;
			else if(point.y>panels[m_index+1]->y2-15)point.y = panels[m_index+1]->y2-15;
			if(point.y < panels[m_index]->y1){
				panels[m_index]->y2 = panels[m_index]->y1 + 2;				
			}
			else{
				panels[m_index]->y2 = point.y;				
			}			
			if(panels[m_index + 1]->visible){
				panels[m_index + 1]->y1 = panels[m_index]->y2;				
			}

			int bottom = GetBottomChartPanel();
			if(panels[bottom]->y2 < height - 2){
				panels[bottom]->y2 = height;				
			}

			Update();
		}

	}
	


	// See if one or more series are being moved to another 
	// panel, or to a new panel (if y > height - calendar)
	bool swaped = false;	
	int start = startIndex;
	int end = endIndex;
	if(swapSeries.size() > 0){
#ifdef _CONSOLE_DEBUG
		//printf("\nswapSeries!");
#endif
		int found = 0;
		int fromSeries = -1;
		int fromPanel = -1;
		CSeries* series;
		int p = -1;
		std::vector<member> members;
		for(int s = 0; s != swapSeries.size(); ++s){
			
			// Where's it coming from?
			for(n = 0; n != panels.size(); ++n){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(panels[n]->series[j]->szName == swapSeries[s]){
						fromPanel = n; // The panel where this is coming from
						fromSeries = j;
						break;
					}
				}
			}

			int toPanel = -1;
			// Where's it going?
			if(swapTo > -1){ // Manually moving it to another panel
				toPanel = swapTo;
			}
			else{ // The user is dragging it to another panel
				for(n = 0; n != panels.size(); ++n){
					if(panels[n]->y1 < point.y && panels[n]->y2 > point.y){
						toPanel = n;
						break;
					}
				}
			}

			//Swap panel position
			if(fromPanel != toPanel && fromPanel != -1 && toPanel!=0 && fromPanel!=0){
				if(toPanel<fromPanel){
#ifdef _CONSOLE_DEBUG
		//printf("\nMoveUp!");
#endif
					MovePanelIndex(fromPanel,fromPanel-toPanel,true);
				}
				else {
#ifdef _CONSOLE_DEBUG
		//printf("\nMoveDown!");
#endif
					MovePanelIndex(fromPanel,toPanel-fromPanel,false);
				}

			}


		}		
		UnSelectAll();

		startIndex = start;
		endIndex = end;

		Update();
		OnSize(-1,width, height + CALENDAR_HEIGHT);
		swapSeries.clear();
		swaped = true;
		changed = true;

	}




	// Send OnClick event to all chart panels if wasn't drawing:
	if (!wasDrawing){
		for (n = 0; n != panels.size(); ++n){
			if (panels[n]->visible){
				if (point.y > panels[n]->y1 &&
					point.y < panels[n]->y2){
					panels[n]->OnLButtonUp(point);
				}
			}
		}
	}




	// Restore mouse if swaped
	if(swaped){
		m_mouseState = MOUSE_NORMAL;
		m_Cursor = 0;
	}

	//Abort Delta Cursor trendline
	if(/*displayInfoText*/DeltaCursor && !drawing && !objectSelected && activePanel->index==0 && m_mouseState == MOUSE_NORMAL){
		//StopDrawing();
		//Update();
	}
	
	ResumeCrossHairs(point);

	COleControl::OnLButtonUp(nFlags, point);
}

///////////////////////////////////////////////////////////////
// Adds a new CChartPanel and returns code
long CStockChartXCtrl::AddChartPanel()
{
	//	Count how many panels are visible and reset 
	//	all other panels if one is found invisible
	CString		volume = "";
	CSeries*	pVol = NULL;
	int volume_height=70;
	int		nCnt = 0;
	int		n = 0;
	bool	visible = true;	
	int		panelsCount	= panels.size();
	int minsize = (int)(0.15*height); // Minimum size for indicator's panels, use 15 for initialize it minimized
	for( n = 0; n < panelsCount; n++ )
	{
		if( !panels[n]->visible )
		{						
			visible = false;
		}
		else
		{
			nCnt++;
		}

		panels[n]->visible = visible;
		
		if( !visible )
		{
			panels[n]->series.clear();
		}
	}
	
	// Now that we know nCnt is the next unused panel,
	// make it visible and size it to fit with the other panels.
	if(nCnt >= MAX_PANELS)	
		 return -1;	// Too many panels opened
	panels[nCnt]->visible = true;
	if( nCnt > 0 )	//	There are other panels visible
	{
		if(panels[0]->y2-panels[0]->y1<(double)(minsize+15))return -1; //Space isn't enough!
		
		double	y1;
		double	y2;
		double	y;
		
		y1	= panels[nCnt - 1]->y1;
		y2	= panels[nCnt - 1]->y2;
		y=/*volume_height*/minsize;
		if(nCnt>1){
			panels[0]->y2	-= y;
			for(int i=1;i<=nCnt-1;i++)
			{
				panels[i]->y1	-= y;
				panels[i]->y2	-= y;
			}
			panels[nCnt  ]->y1	= panels[nCnt-1]->y2;
			panels[nCnt  ]->y2	= height;
		}
		else{
			panels[0]->y2	= height-y;
			panels[nCnt  ]->y1	= panels[0]->y2;
			panels[nCnt  ]->y2	= height;
		}
		

	}
	else	// There are no other panels visible
	{
		panels[nCnt]->y1 = 0;
		panels[nCnt]->y2 = height;
	}

	//	Order all panel indexes
	for( n = 0; n < panelsCount; n++ )
	{
		panels[n]->index = n;
	}

	
	// Don't repaint because drawing isn't finished since 
	// AddSeries has called this function for a new panel.
	// Just return the index to this new panel.	
		
	
	return nCnt;
}

// Adds a new chart panel using serialized locations (don't calculate panel's position)
long CStockChartXCtrl::AddChartPanelSerialized(void)
{
	//	Count how many panels are visible and reset 
	//	all other panels if one is found invisible
	CString		volume = "";
	CSeries*	pVol = NULL;
	int volume_height=70;
	int		nCnt = 0;
	int		n = 0;
	bool	visible = true;	
	int		panelsCount	= panels.size();
	int minsize = (int)(0.15*height); // Minimum size for indicator's panels, use 15 for initialize it minimized
	for( n = 0; n < panelsCount; n++ )
	{
		if( !panels[n]->visible )
		{						
			visible = false;
		}
		else
		{
			nCnt++;
		}

		panels[n]->visible = visible;
		
		if( !visible )
		{
			panels[n]->series.clear();
		}
	}
	
	// Now that we know nCnt is the next unused panel,
	// make it visible and size it to fit with the other panels.
	if(nCnt >= MAX_PANELS)	
		 return -1;	// Too many panels opened
	panels[nCnt]->visible = true;
	/*if( nCnt > 0 )	//	There are other panels visible
	{
		if(panels[0]->y2-panels[0]->y1<(double)(minsize+15))return -1; //Space isn't enough!

		
		double	y1;
		double	y2;
		double	y;
		
		y1	= panels[nCnt - 1]->y1;
		y2	= panels[nCnt - 1]->y2;
		y=minsize;
		if(nCnt>1){
			panels[0]->y2	-= y;
			for(int i=1;i<=nCnt-1;i++)
			{
				panels[i]->y1	-= y;
				panels[i]->y2	-= y;
			}
			panels[nCnt  ]->y1	= panels[nCnt-1]->y2;
			panels[nCnt  ]->y2	= height;
		}
		else{
			panels[0]->y2	= height-y;
			panels[nCnt  ]->y1	= panels[0]->y2;
			panels[nCnt  ]->y2	= height;
		}
		

	}
	else	// There are no other panels visible
	{
		panels[nCnt]->y1 = 0;
		panels[nCnt]->y2 = height;
	}*/

	//	Order all panel indexes
	for( n = 0; n < panelsCount; n++ )
	{
		panels[n]->index = n;
	}

	
	// Don't repaint because drawing isn't finished since 
	// AddSeries has called this function for a new panel.
	// Just return the index to this new panel.	
		
	
	return nCnt;
}

void CStockChartXCtrl::Refresh()
{
	OnSize(-1,width,height + CALENDAR_HEIGHT);
}

// Removes a chart panel
void CStockChartXCtrl::RecycleChartPanel(int index)
{
	
	// recycle the panel (special thanks to Cagatay Tunali)

	CChartPanel* ppanel = panels[index];
 
	int i;// I delete all the objects from the chart

//	Revision 6/10/2004
//	type cast of int
	for(i = 0; i < (int)ppanel->lines.size(); i++){
		delete ppanel->lines[i];
		ppanel->lines[i] = NULL;
	}
	for(i = 0; i < (int)ppanel->textAreas.size(); i++){
		delete ppanel->textAreas[i];
		ppanel->textAreas[i] = NULL; 
	}
	for(i = 0; i < (int)ppanel->objects.size(); i++){
		delete ppanel->objects[i];
		ppanel->objects[i] = NULL; 
	}
//	End Of Revision
	delete ppanel;	
 
	std::vector<CChartPanel*>::iterator itr = panels.begin() + index;
	panels.erase(itr);
	//(when you erase from a vector it resizes itself.
	// For example; before the erase operation the vector 
	// size is 25, after erase operation it becomes 24
	// So you must resize it before adding a new element)
 
	panels.resize(MAX_PANELS);
	panels[MAX_PANELS-1] = new CChartPanel();
	panels[MAX_PANELS-1]->Connect(this);
 
	OnSize(-1,width,height + CALENDAR_HEIGHT);
 
	// re-order remaining panels
  int n;
	for(n = 0; n != panels.size(); ++n){
		panels[n]->index = n;
	}
	changed = true;

}

///////////////////////////////////////////////////////////////
// Adds a new series to one of the chart panels. If nPanel > panel count
// or panel < 0, then this function creates a new panel and returns it's index.
// nType = bar, line, candle, stock, renko, p&f, etc...
long CStockChartXCtrl::AddSeries(LPCTSTR Name, long Type, long Panel) 
{	



	int index = Panel;

	// Count how many panels are visible
	int nCnt = 0;
	int n = 0;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			nCnt++;
		}
	}
	
	nCnt -= 1;
	if(index > nCnt || index < 0){
		// Use a new panel or return an error code		
		if(nCnt < 0) nCnt = 0;
		if(nCnt >= MAX_PANELS -1){ // Too many panels
			nCnt = AddChartPanel();
			if(nCnt == -1) return -1;
		}
		index = nCnt;
	}

	// Whatever the panel index is now, add the series to it
	panels[index]->Connect(this);

	AddNewSeriesType(index,Type,Name);

	if(((CString)Name).Find(".volume")>0) {
		panels[index]->series[panels[index]->series.size()-1]->seriesVisible = false;
	}

	return index;

}

void CStockChartXCtrl::AddNewSeriesType(long panel, long type, CString name)
{
#ifdef CONSOLE_DEBUG
	InitializeDebugConsole();	
#endif


	switch(type){
		
		case OBJECT_SERIES_LINE: // add a new line series	
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_LINE \n");
#endif

			panels[panel]->series.push_back(new CSeriesStandard(name, OBJECT_SERIES_LINE, 1, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		case OBJECT_SERIES_STOCK: // add a new stock ohlc chart
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_STOCK \n");
#endif
			panels[panel]->series.push_back(new CSeriesStock(name, OBJECT_SERIES_STOCK, 4, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		case OBJECT_SERIES_STOCK_LINE: // add a new stock ohlc line chart
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_STOCK_LINE \n");
#endif
			panels[panel]->series.push_back(new CSeriesStock(name, OBJECT_SERIES_STOCK_LINE, 4, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		case OBJECT_SERIES_STOCK_HLC: // add a new stock hlc chart
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_STOCK_HLC \n");
#endif
			panels[panel]->series.push_back(new CSeriesStock(name, OBJECT_SERIES_STOCK_HLC, 4, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		case OBJECT_SERIES_CANDLE: // add a new stock ohlc chart	
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_CANDLE \n");
#endif
			panels[panel]->series.push_back(new CSeriesStock(name, OBJECT_SERIES_CANDLE, 4, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		case OBJECT_SERIES_BAR: // add a new bar (volume) chart
#ifdef CONSOLE_DEBUG
			InitializeDebugConsole();
			//printf(" OBJECT_SERIES_BAR \n");
#endif
			panels[panel]->series.push_back(new CSeriesStandard(name, OBJECT_SERIES_BAR, 1, panels[panel]));
			panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);			
			panels[panel]->OnUpdate();
			break;

		///////Insert new series types here///////

	}

	changed = true;
}

long CStockChartXCtrl::GetBarInterval() 
{
	return m_barInterval;
}

void CStockChartXCtrl::SetBarInterval(long nNewValue) 
{
	if(nNewValue > 0) m_barInterval = nNewValue;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnBarStartTimeChanged() 
{
	barStartTime = m_barStartTime;
	SetModifiedFlag();
}


// Builds a bar from ticks based on BarInterval (minutes)
void CStockChartXCtrl::AppendValueAsTick(LPCTSTR Name, double JDate, double Value) 
{
	Append(Name, JDate, Value, true);
}

// Appends a range of data to every visible panel that contains a series with this Name.
void CStockChartXCtrl::AppendRangeValues(LPCTSTR Name, VARIANT& JDates, VARIANT& Values, LONG Size)
{
	double* pointerJ = new double[Size];
	double* pointerV = new double[Size]; 
	SafeArrayAccessData((JDates).parray,(void**)&pointerJ);
	SafeArrayAccessData((Values).parray,(void**)&pointerV);
	for(int i=0;i<Size;i++){
		Append(Name, pointerJ[i], pointerV[i], false);	
	}
	delete [] pointerJ;
	delete [] pointerV;
}

// Appends data to every visible panel that contains a series with this Name.
void CStockChartXCtrl::AppendValue(LPCTSTR Name, double JDate, double Value) 
{
	Append(Name, JDate, Value, false);		
}

// Appends data
void CStockChartXCtrl::Append(LPCSTR Name, double JDate, double Value, bool Tick)
{	
	
	// Maximum number of records to display when appending
	if(m_maxDisplayRecords > 0){
		if(endIndex - startIndex > m_maxDisplayRecords){
			startIndex = endIndex - m_maxDisplayRecords;
		}
	}

	double endDate = 0;
  int n;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					if(panels[n]->series[j]->data_master.size() && !swapping){
						endDate = panels[n]->series[j]->data_master[
							panels[n]->series[j]->data_master.size() - 1].jdate;
							if(Tick){
								panels[n]->series[j]->AppendValueAsTick(JDate, Value);
							}
							else{
								panels[n]->series[j]->AppendValue(JDate, Value);
							}

							if(!swapping){
								long start = barColors.size();
								long resize = (start > endIndex ? start : endIndex);
								barColors.resize(resize);
								if(start < 0) start = 0;
								for(int bar = start; bar != barColors.size(); ++bar){
									barColors[bar] = -1;
								}
							}

							if(!swapping) reCalc = true;
							changed = true;
							WatchTrendLines(Name); // Added 6/20/2007
							return;
					}
					else{
						if(Tick){
							panels[n]->series[j]->AppendValueAsTick(JDate, Value);
						}
						else{
							panels[n]->series[j]->AppendValue(JDate, Value);
						}


						if(!swapping){
							long start = barColors.size();
							long resize = (start > endIndex ? start : endIndex);
							barColors.resize(resize);
							if(start < 0) start = 0;
							for(int bar = start; bar != barColors.size(); ++bar){
								barColors[bar] = -1;
							}
						}

						if(!swapping) reCalc = true;
						changed = true;
						WatchTrendLines(Name); // Added 6/20/2007
						return;
					}
				}
			}	
		}
	}
}


// Edits data in every visible panel that contains a series with this Name and JDate
void CStockChartXCtrl::EditValue(LPCTSTR Name, double JDate, double Value) 
{
  int n;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->EditValue(JDate, Value);
					reCalc = true;
					changed = true;
					WatchTrendLines(Name); // Added 7/10/2007
					return;
				}
			}
		}
	}
}


// Edits a value based on the record number instead of JDate
void CStockChartXCtrl::EditValueByRecord(LPCTSTR Name, long Record, double Value) 
{
	Record -= 1; // 1 based - 3/15/05 TW
  int n;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->EditValue(Record, Value);
					reCalc = true;
					changed = true;
					WatchTrendLines(Name); // Added 7/10/2007
					return;
				}
			}
		}
	}

}



// Removes all values for a specified series
void CStockChartXCtrl::ClearValues(LPCTSTR Name) 
{	
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->Clear();
				}
			}
		}
	}
	updatingIndicator = false;
	reCalc = true;
//	startIndex = 0;
	endIndex = 0;
}

// Removes all series with specified name
void CStockChartXCtrl::RemoveSeries(LPCTSTR Name) 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nRemoveSeries(%s)",Name);
#endif
	CString sname = Name;
	if(((CString)Name).Find(".volume")>0)return;
	reCalc = false;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(panels[n]->series.size() > 0){
				for(int j = 0; j != panels[n]->series.size(); ++j){					
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){	
						int cnt = panels[n]->deleteSeries(Name);
						if(cnt == 0){ // No more series in this panel
							int visible = GetVisiblePanelCount();
							if(visible > 1){		
								//Re-order all panels								
								int offset = panels[n]->y2 - panels[n]->y1;
								int a = (int)GetNextHigherChartPanel((int)panels[n]->y1);
								while(a > 0){
									panels[a]->y2 += offset;
									panels[a]->y1 += offset; 
									a = (int)GetNextHigherChartPanel((int)panels[a]->y1);
								}
								panels[0]->y2 += offset;


								RecycleChartPanel(n);
							}
						}
						Update();
						FireDeleteSeries(Name);
						return;
					}
				}
			}
		}
	}
}

// Updates the screen
void CStockChartXCtrl::Update(long operation /* = 0 */) 
{
	if(operation!=NULL && operation == 1){
		if(endIndex < RecordCount()-1){
			return;
		}
		else if(movingObject || m_mouseState == MOUSE_DRAWING){
			//Test if user is drawing on last tick area:
			bool onArea = false;
			for(int m=0; m!=panels.size(); ++m)
			{
				if(panels[m]->y1<m_MousePositionY && panels[m]->y2>m_MousePositionY)
				{
					int n;				
					for(n = 0; n != panels[m]->objects.size(); ++n){
						/*if(panels[m]->objects[n]->drawing){
							if(panels[m]->objects[n]->newRect.left>GetX(endIndex-2)-GetX(startIndex) || panels[m]->objects[n]->newRect.right>GetX(endIndex-2)-GetX(startIndex)) onArea = true;
						}
						else if(panels[m]->objects[n]->selected){
							if(panels[m]->objects[n]->x1>GetX(endIndex-2)-GetX(startIndex) || panels[m]->objects[n]->x2>GetX(endIndex-2)-GetX(startIndex)) onArea = true;
						}*/
					}

					for(n = 0; n != panels[m]->lines.size(); ++n){
						if(panels[m]->lines[n]->drawing){
							if(panels[m]->lines[n]->newRect.left>GetX(endIndex-2)-GetX(startIndex) || panels[m]->lines[n]->newRect.right>GetX(endIndex-2)-GetX(startIndex)) onArea = true;
						}
						else if(panels[m]->lines[n]->selected){
							if(panels[m]->lines[n]->x1>GetX(endIndex-2)-GetX(startIndex) || panels[m]->lines[n]->x2>GetX(endIndex-2)-GetX(startIndex)) onArea = true;
						}
					}
				}
			}

			if(onArea){
				
#ifdef _CONSOLE_DEBUG
			//printf("\nON AREA !!!!");
#endif
			return;
			}
#ifdef _CONSOLE_DEBUG
			//printf("\nCLIP-REGION [ON]");
#endif
			pdcHandler->IsClipRegion = true;
			UpdateScreen(false, operation);
			return;
		}
	}
	UpdateScreen(false, operation);
}

// If suppressErrors = true, the "not all series same length"
// error will not be fired.
void CStockChartXCtrl::UpdateScreen(bool suppressErrors, long operation /* = 0 */){
 int t = clock();
 int delta;
 if(m_frozen) return;
 locked = false;

#ifdef _CONSOLE_DEBUG
 //printf("\nUpdateScreen()");
#endif


 if(!m_pauseCrossHairs)SuspendCrossHairs();

 //StopDrawing();

 long length = 0, max = 0;
 
 // Re-calculate technical indicators if new data is present
 if(reCalc && !loading){
  InvalidateIndicators();
  int count = GetVisiblePanelCount();
  for(int panel = 0; panel != count; ++panel){
   for(int series = 0; series != panels[panel]->series.size(); ++series){
    if(panels[panel]->series[series]->seriesType == OBJECT_SERIES_INDICATOR){
     if(panels[panel]->series[series]->recycleFlag){
      RemoveSeries(panels[panel]->series[series]->szName);
      goto update;
     }
     else{
		 updatingIndicator = true;
		 if(!panels[panel]->series[series]->Calculate()){
		 }
     }
    }
   }
  }
  reCalc = false;
  delta=clock()-t;
  t=clock();



 }
update:
 
 // Remove any panels that have no series left // 4/11/05 RG
 // (this may or may not be done in RemoveSeries) 
  int n;  
 if(!loading && !buildingChart){
  std::vector<int> remove;
  for(n = 0; n != GetVisiblePanelCount(); ++n){
   if(panels[n]->series.size() == 0){
    remove.push_back(n);   
   }
  }
  for(n = 0; n != remove.size(); ++n){
   RecycleChartPanel(remove[n]);
  }
 }
 
  delta=clock()-t;
  t=clock();



 // Continue updating
 if(buildingChart) return;
 int j;
 for(n = 0; n != panels.size(); ++n){
  if(panels[n]->visible){   
   // Update all slaves
   for(j = 0; j != panels[n]->series.size(); ++j){
    if(!suppressErrors){
     length = panels[n]->series[j]->data_master.size();



     if(max == 0) max = length;
     if(max != length && !loading && 
      panels[n]->series[j]->seriesType != OBJECT_SERIES_INDICATOR){
 
      if(m_ignoreSeriesLengthErrors != TRUE){ // RDG 8/5/04
       ThrowError( CUSTOM_CTL_SCODE(1279), "Not all series are same length");
       return;
      }
 
     }
    }
    panels[n]->series[j]->UpdateSlave();    
   }
   // Reset all objects otherwise they will bitblit
   // in the wrong place when they are shown again.
   for(j = 0; j != panels[n]->objects.size(); ++j){
    panels[n]->objects[j]->Reset(false);
   }
   panels[n]->Invalidate();
  }
 }
 
  delta=clock()-t;
  t=clock();



 //UnSelectAll();
 
 //endIndex = this->GetSlaveRecordCount(); 
 barSpacing = 0;
 /*TESTE
 m_mouseState = MOUSE_NORMAL; 
 resizing = false;
 movingObject = false;
 if(m_valueView) m_valueView->Reset(true); 
 TESTE*/
 if(!buildingChart){
  COleControl::InvalidateControl(); 
 } 
 bResetMemDC = true;




 // Has the start index changed?
 t=clock();
 if(startIndex > GetRecordCount() - 1) startIndex = 0;
 delta=clock()-t;



 if(m_lastStartIndex != startIndex && !buildingChart){	
  m_lastStartIndex = startIndex;
  t=clock();
  FireScroll();
  delta=clock()-t;
  



 }
  delta=clock()-t;
  t=clock();




}

// Creates a string GUID
CString CStockChartXCtrl::CreateGUID()
{
	CLSID cClsid;
	HRESULT hr = CoCreateGuid( &cClsid );
	if(FAILED( hr )){
		ASSERT(FALSE);
		return "-987654321";
	} 
	OLECHAR olestrClsid[39];
	int iCharNr = StringFromGUID2(cClsid, olestrClsid, 39);
	char clsid[100];
	wcstombs(clsid, olestrClsid, 100); // For .NET 2003, 6.0 and below
	// wcstombs_s(NULL, clsid, 100, olestrClsid, 100); // For .NET 2005 and above
	return clsid;
}

 
void CStockChartXCtrl::ScrollLeft(long Records) 
{	
	if(buildingChart) return;
	if(startIndex - Records > 0){
		startIndex -= Records;
		endIndex -= Records;
	}
	else{
		long oldLength = endIndex - startIndex;
		startIndex = 0;
		endIndex = startIndex + oldLength;
		if(endIndex > RecordCount()) endIndex = RecordCount();
	}
	Update();
}

void CStockChartXCtrl::ScrollRight(long Records) 
{
	if(buildingChart) return;
	if(endIndex + Records <= RecordCount()){
		startIndex += Records;
		endIndex += Records;
	}
	else{		
		endIndex = RecordCount();
		long oldLength = endIndex - startIndex;		
		startIndex = endIndex - oldLength;
		if(startIndex < 0) startIndex = 0;
	}
	Update();
}

// Returns true if all series will permit scrolling
bool CStockChartXCtrl::CanScroll(int Records)
{
	bool ok = false;
	int max = 0;
	int newStart = startIndex + Records;
	int newEnd = endIndex + Records + newStart;
	// See if newStart will work for all series
	if(newStart < 0) return false;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(panels[n]->GetMinSlaveSize() > newStart){
				//newStart is too big
				return false;
			}
			if(panels[n]->GetMaxSlaveSize() < newEnd - MIN_VISIBLE){
				//newStart is too big
				return false;
			}
			if(panels[n]->series.size() > 0 && 
				panels[n]->GetMinSlaveSize() > 0){
				ok = true;
			}
		}		
	}
	if(ok){
		return true;
	}
	else{
		return false;
	}
}

void CStockChartXCtrl::ZoomIn(long Records) 
{	
	long recCnt = RecordCount();
	if(buildingChart){		
		return;
	}
	/*if(endIndex - startIndex - 2*Records > Records){
		startIndex += 2*Records;
		//endIndex -= Records;
	}	*/
	if(startIndex + 2*Records < endIndex){//
		startIndex += 2*Records;
		//endIndex -= Records;
	}
	else if(endIndex-2*Records>startIndex){//
		endIndex-=2*Records;
	}
	Update();
	FireZoom();
}

void CStockChartXCtrl::ZoomOut(long Records) 
{	
	long recCnt = RecordCount();
	if(buildingChart){
		return;
	}
	if(startIndex - 2*Records > -1){
		startIndex -= 2*Records;
		//endIndex += Records;
	}
	/*else if(priceStyle != psStandard && endIndex < recCnt){
		endIndex += Records;
	}*/
	else if(endIndex + 2*Records < recCnt){//
		endIndex += 2*Records;
	}
	else{
		startIndex = 0;
		endIndex=recCnt;//
	}
	if(endIndex > recCnt){
		endIndex = recCnt;
	}	
	Update();
	FireZoom();
}

// Returns the max recordcount of all series
long CStockChartXCtrl::RecordCount() 
{
	return GetRecordCount();
}


// Returns the visible panel count
long CStockChartXCtrl::GetPanelCount() 
{
	long ret = 0;

	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			ret++;
		}
	}

	return ret;
}


// Returns a double value by index for the series with Name
double CStockChartXCtrl::GetValue(LPCTSTR Name, long Record) 
{
	double ret = NULL_VALUE;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->GetValue(Record - 1);
					return ret;
				}
			}
		}
	}
	return ret;
}

// Returns a double value by index for the series with Name
double CStockChartXCtrl::GetJDate(LPCTSTR Name, long Record) 
{
	double ret = NULL_VALUE;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->GetJDate(Record - 1);
					return ret;
				} 
			}
		}
	}
	return ret;
}

// Returns a double value by jdate for the series with Name
double CStockChartXCtrl::GetValueByJDate(LPCTSTR Name, double JDate) 
{
	double ret = NULL_VALUE;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->GetValue(JDate);
					return ret;
				}
			}
		}
	}
	return ret;
}

// Returns Julian date
double CStockChartXCtrl::ToJulianDate(long nYear, long nMonth, long nDay, long nUHour, long nUMinute, long nUSecond) 
{
  return CJulian::ToJulianDate(nYear, nMonth, nDay, nUHour, nUMinute, nUSecond, 0);
// 	double extra = 100 * nYear + nMonth - 190002.5;
// 	double rjd = 367 * nYear;
// 	rjd -= floor((double)(7 * (nYear + floor((double)((nMonth + 9) / 12))) / 4));
// 	rjd += floor((double)(275 * nMonth / 9));
// 	rjd += nDay;
// 	rjd += ((double)nUHour + ((double)nUMinute + 
// 		   ((double)nUSecond) / 60) / 60) / 24;
// 	rjd += 1721013.5;
// 	rjd -= 0.5 * extra / (double)abs(extra);
// 	rjd += 0.5;
// 	return rjd;	
}

double CStockChartXCtrl::ToJulianDateEx(long nYear, long nMonth, long nDay, long nUHour, long nUMinute, long nUSecond, long nUMilliSecond) 
{
  return CJulian::ToJulianDate(nYear, nMonth, nDay, nUHour, nUMinute, nUSecond, nUMilliSecond);
}


// Converts a Julian date to a string date
BSTR CStockChartXCtrl::FromJulianDate(double JulianDate) 
{
	CString strResult;	
	strResult = FromJDate(JulianDate);
	return strResult.AllocSysString();

}

CString CStockChartXCtrl::FromJDate(double jdate)
{
  return CJulian::FromJulianDate(jdate);

// 	double j1 = 0;
// 	double j2 = 0;
// 	double j3 = 0;
// 	double j4 = 0;
// 	double j5 = 0;
// 
//     //Get the date from the Julian day number
//     
//     double intgr = floor(jdate);
//     double frac = jdate - intgr;
//     double gregjd = 2299161;
//     if(intgr >= gregjd){ //Gregorian calendar correction
//         double tmp = floor(((intgr - 1867216) - 0.25) / 36524.25);
//         j1 = intgr + 1 + tmp - floor(0.25 * tmp);
//     }
// 	else{
//         j1 = intgr;
//     }
// 
//     //Correction for half day offset
//     double dayfrac = frac + 0.5;
//     if(dayfrac >= 1){
//         dayfrac -= 1;
//         j1 += 1;
//     }
// 
//     j2 = j1 + 1524;
//     j3 = floor(6680 + ((j2 - 2439870) - 122.1) / 365.25);
//     j4 = floor(j3 * 365.25);
//     j5 = floor((j2 - j4) / 30.6001);
// 
//     double d = floor(j2 - j4 - floor(j5 * 30.6001));
//     double m = floor(j5 - 1);
// 	if(m > 12){
// 		m -= 12;
// 	}
//         
//     double y = floor(j3 - 4715);
//     if(m > 2){
//         y -= 1;
//     }
//     if(y <= 0){
//         y -= 1;
//     }
//     
//     //Get time of day from day fraction
//     double hr = floor(dayfrac * 24);
//     double mn = floor((dayfrac * 24 - hr) * 60);
//     double f = ((dayfrac * 24 - hr) * 60 - mn) * 60;
//     double sc = floor(f)+3;
//     f -= sc;
//     if(f > 0.5){
//         sc += 1;
//     }
// 	
//     if(y < 0){
//         y = -y;
//     }
// 
// 	// 24 hour math
// 	if(sc > 59){
// 		sc = sc - 60;
// 		mn += 1;
// 		//if(d <31) d += 1; // Added 6/20/08
// 	}
// 
// 	if(mn > 59){
// 		mn = mn - 60;
// 		hr += 1;
// 	}
// 
// 	if(hr == 24){
// 		hr = 0;
// 		mn = 0;
// 		sc = 0;
// 	}
// 
// 	// Update 10/29/04 RG
// 	SYSTEMTIME myTime;
// 	myTime.wMonth = m;
// 	myTime.wDay = d;
// 	myTime.wYear = y;
// 	myTime.wHour = hr;
// 	myTime.wMinute = mn;
// 	myTime.wSecond = sc;
// 	myTime.wMilliseconds = 0; // To be used in the future
// 	char timeBuf[20+1] = "";
// 	char dateBuf[20+1] = "";
// 	GetTimeFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, timeBuf, 20);
// 	GetDateFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, dateBuf, 20);
// 	CString szDT = dateBuf;
// 	szDT += " ";
// 	szDT += timeBuf;
// 	if(strlen(dateBuf) == 0) return "No Date Time";
// 	return szDT;

}


// Returns the date format specific to this machine's locale
// NO LONGER USED
CString CStockChartXCtrl::FormatDate(int month, int day, int year, 
									 int hour, int minute, int second)
{
	COleDateTime vaTimeDate;    
	vaTimeDate.SetDateTime(year,month,day,hour,minute,second);		
    return vaTimeDate.Format(0,LANG_USER_DEFAULT);
}

// DEBUG function only
void CStockChartXCtrl::EmbedData(CString name, int month, int day, int year, double open, double high, double low, double close)
{
	double jdate = this->ToJulianDate(year,month,day,12,30,0);
	AppendValue(name + ".open",jdate,open);
	AppendValue(name + ".high",jdate,high);
	AppendValue(name + ".low",jdate,low);
	AppendValue(name + ".close",jdate,close);
}


void CStockChartXCtrl::RePaint()
{	
#ifdef _CONSOLE_DEBUG
	printf("\nRePaint()");
#endif
	if(IgnorePaint)IgnorePaint = false;
	if(!buildingChart){
		COleControl::InvalidateControl();
	}
	m_valueView->Reset(true);	
}


void CStockChartXCtrl::OnYGridChanged() 
{
	showYGrid = m_yGrid == TRUE;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}


void CStockChartXCtrl::OnXGridChanged() 
{
	showXGrid = m_xGrid == TRUE;
	changed = true;
	UpdateScreen(true);
	ForcePaint(); // Added 2/28/06
	SetModifiedFlag();
}


long CStockChartXCtrl::GetAlignment() 
{	
	return yAlignment;
}

void CStockChartXCtrl::SetAlignment(long nNewValue) 
{
	yAlignment = nNewValue;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

// Calculates the Y scale width based on character width
void CStockChartXCtrl::CalculateScaleInfo()
{

	CDC * pDC = GetDC();
	ASSERT(pDC != NULL);

	double max = 0;	

	// Get max value from all series
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			for(int k = 0; k != panels[n]->series[j]->data_master.size() ; ++k){
				if(panels[n]->series[j]->data_master[k].value > max){
					max = panels[n]->series[j]->data_master[k].value;
				}	
			}
		}
	}

	//Get font info
	CFont newFont;			
	newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial"), pDC);	
	TEXTMETRIC tm;
	CFont* pOldFont = pDC->SelectObject(&newFont);
	pDC->GetTextMetrics(&tm);

//	Revision 6/10/2004
//	type cast of int
	int nAvgWidth = (int)(1 + (tm.tmAveCharWidth * 0.8));
	int nCharHeight = (int)(1 + (tm.tmHeight));
//End Of Revision
	// See how wide the y scale should be
	CString len;
	len.Format("%.*f", 2, max);	
	int nMaxWidth = nAvgWidth * len.GetLength() + 30;
	if(nMaxWidth > yScaleWidth || yScaleWidth > 250){ // 9/5/08
		yScaleWidth = nMaxWidth + 2;
	}
	//decimals = 2;
	if(max < 0.9){
//		decimals = 10;
//		yScaleWidth = nMaxWidth + (10 * nAvgWidth);
	}

	// To support new forex precision 7/7/06
	if(decimals > 5) yScaleWidth = nMaxWidth + (5 * nAvgWidth);


	// Calculate horizontal spacing
	double nWidth = 0;
    double nSpacing = 0;
	long recCnt = GetSlaveRecordCount();
    nWidth = width - yScaleWidth - extendedXPixels;
    if(recCnt > 0) nSpacing = nWidth / (recCnt);
//	Revision 6/10/2004
//	type cast of int
	spaceInterval = (int)nSpacing;
//	End Of Revision


	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();

	ReleaseDC(pDC);

}

void CStockChartXCtrl::UpdateRect(CRect rect)
{
	CDC* pdc = GetDC();
	if(pdc == NULL) return;
	
	//	BitBlt the buffer bitmap to the screen using the memory dc	
	CBitmap* pOldBmp = pdc->SelectObject(&m_bitmap);
	pdc->BitBlt( rect.left, rect.top, rect.Width(), rect.Height(),
		&m_memDC, rect.left, rect.top,SRCCOPY );
	if(pOldBmp) pdc->SelectObject(pOldBmp);
	ReleaseDC(pdc);
}

void CStockChartXCtrl::RePaintXOR()
{
	m_xor = true;
	if(!buildingChart){
		COleControl::InvalidateControl();
	}
}

// Unselects all series and objects
void CStockChartXCtrl::UnSelectAll()
{
#ifdef _CONSOLE_DEBUG
	printf("\nUnSelectAll()");
#endif
  int n, j;
	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->selected){
				panels[n]->series[j]->ownerPanel->Invalidate();
				panels[n]->series[j]->selected = false;
			}
		}
		for(j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->selected){
				panels[n]->objects[j]->ownerPanel->Invalidate();
				panels[n]->objects[j]->selected = false;
			}
		}
		for(j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->selected){
				panels[n]->lines[j]->ownerPanel->Invalidate();
				panels[n]->lines[j]->selected = false;
			}
		}
		for(j = 0; j != panels[n]->textAreas.size(); ++j){
			if(panels[n]->textAreas[j]->selected){
				panels[n]->textAreas[j]->ownerPanel->Invalidate();
				panels[n]->textAreas[j]->selected = false;
			}
		}
	}
	//Update();
	OnSelectSeries("");
}

// Returns total number of currently selected objects
int CStockChartXCtrl::SelectCount()
{
	int selected = 0;
  int n, j;
	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->selected){
				selected++;
			}
		}
		for(j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->selected){
				selected++;
			}
		}
		for(j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->selected){
				selected++;
			}
		}
		for(j = 0; j != panels[n]->textAreas.size(); ++j){
			if(panels[n]->textAreas[j]->selected){
				selected++;
			}
		}
	}
	return selected;
}

OLE_COLOR CStockChartXCtrl::GetLineColor() 
{	
	return lineColor;
}

void CStockChartXCtrl::SetLineColor(OLE_COLOR nNewValue) 
{	
	lineColor = nNewValue;
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::SetFibonacciRetParams(DOUBLE param0, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9)
{
	fibonacciRetParams.resize(10);
	fibonacciRetParams[0]=param0;
	fibonacciRetParams[1]=param1;
	fibonacciRetParams[2]=param2;
	fibonacciRetParams[3]=param3;
	fibonacciRetParams[4]=param4;
	fibonacciRetParams[5]=param5;
	fibonacciRetParams[6]=param6;
	fibonacciRetParams[7]=param7;
	fibonacciRetParams[8]=param8;
	fibonacciRetParams[9]=param9;	
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::SetFibonacciProParams(DOUBLE param0, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9)
{
	fibonacciProParams.resize(10);
	fibonacciProParams[0]=param0;
	fibonacciProParams[1]=param1;
	fibonacciProParams[2]=param2;
	fibonacciProParams[3]=param3;
	fibonacciProParams[4]=param4;
	fibonacciProParams[5]=param5;
	fibonacciProParams[6]=param6;
	fibonacciProParams[7]=param7;
	fibonacciProParams[8]=param8;
	fibonacciProParams[9]=param9;	
	changed = true;
	SetModifiedFlag();
}

// Line Study Parameters - 1/20/05
double CStockChartXCtrl::GetLineStudyParam(LPCTSTR Key, short ParamNum) 
{	

	double ret = NULL_VALUE;
	if(ParamNum < 0) return ret;

	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->lines.size(); ++j){
			if(CompareNoCase(panels[n]->lines[j]->key, Key)){
				if(panels[n]->lines[j]->params.size() >= (unsigned long)ParamNum){
					ret = panels[n]->lines[j]->params[ParamNum - 1];
					break;
				}
			}
		}
	}

	return ret;
}


void CStockChartXCtrl::SetLineStudyParam(LPCTSTR Key, short ParamNum, double newValue) 
{	
	
	if(ParamNum < 0) return;

	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->lines.size(); ++j){
			if(CompareNoCase(panels[n]->lines[j]->key, Key)){
				if(panels[n]->lines[j]->params.size() >= (unsigned long)ParamNum){
					panels[n]->lines[j]->params[ParamNum - 1] = newValue;
					break;
				}
			}
		}
	}

	changed = true;
	SetModifiedFlag();
	Update();
}

LONG CStockChartXCtrl::GetLineThickness()
{
	return lineThickness;
	//return lineWeight;
}

void CStockChartXCtrl::SetLineThickness(LONG newValue)
{
	lineThickness = newValue;
	//lineWeight = newValue;
	changed = true;
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetValuePanelColor() 
{	
	return valuePanelColor;
}

void CStockChartXCtrl::SetValuePanelColor(OLE_COLOR nNewValue) 
{	
	valuePanelColor = nNewValue;
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnRButtonUp(UINT nFlags, CPoint point) 
{	
	
	if(locked) return;

	if(pause) return;

	if(m_setcap) ::ReleaseCapture();

	// Send event to all chart panels
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				panels[n]->OnRButtonUp(point);
			}
		}
	}
	FireOnRButtonUp();

	COleControl::OnRButtonUp(nFlags, point);
}

void CStockChartXCtrl::OnRButtonDown(UINT nFlags, CPoint point) 
{	
	UnSelectAll(); // Unselect everything else

	Update();

	COleControl::OnRButtonDown(nFlags, point); // Moved from bottom 11/5/07
	

	if(locked) return;

	FireClick();

	/*** FROEDE_MARK ... Keep the crosshair in the current state...
	if(m_crossHairs){
		CDC* pDC = GetScreen();
		pDC->SetROP2(R2_NOT);
		pDC->MoveTo(oldVRect.left,oldVRect.top);
		pDC->LineTo(oldVRect.right,oldVRect.bottom);
		pDC->MoveTo(oldHRect.left,oldHRect.top);
		pDC->LineTo(oldHRect.right,oldHRect.bottom);
		pDC->SetROP2(R2_COPYPEN);		
		ReleaseScreen(pDC);
		m_crossHairs = false;
		oldHRect = CRect(-1,-1,-1,-1);
		oldVRect = CRect(-1,-1,-1,-1);
		Update();
	}
	*/

	if(pause) return;

	if(!focus) GetCtrlFocus();

	if(m_mouseState != MOUSE_NORMAL) return;


	// Send event to all chart panels
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){		
				activePanel = panels[n];				
			}
		}
	}

	if(!m_onRClickFired){ // RDG 8/31/04
		FireOnRButtonDown();
	}
	m_onRClickFired = false;

}


CDC* CStockChartXCtrl::GetScreen()
{
	CDC* pDC = GetDC();
	ASSERT(pDC != NULL);
	return pDC;
}

void CStockChartXCtrl::ReleaseScreen(CDC *pDC)
{
	ReleaseDC(pDC);
}



void CStockChartXCtrl::AddUserTrendLine(LPCTSTR Key) 
{	
	StopDrawing();
	UnSelectAll();
	movingObject = true;
	m_Cursor = IDC_PENCIL;	
	m_mouseState = MOUSE_DRAWING;
	m_drawingType = lsTrendLine;
	m_key = Key;
}

//Vertical and Horizontal Lines:
void CStockChartXCtrl::AddUserXLine(LPCTSTR Key)
{
	movingObject = true;
	m_Cursor = IDC_PENCIL;	
	m_mouseState = MOUSE_DRAWING;
	m_drawingType = lsXLine;
	m_key = Key;
}

void CStockChartXCtrl::AddUserYLine(LPCTSTR Key)
{

	movingObject = true;
	m_Cursor = IDC_PENCIL;	
	m_mouseState = MOUSE_DRAWING;
	m_drawingType = lsYLine;
	m_key = Key;
}

void CStockChartXCtrl::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags) 
{	
	
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			panels[n]->textAreas[j]->OnChar(nChar, nRepCnt, nFlags);
		}
	}
	FireOnChar(nChar, nRepCnt, nFlags);
	COleControl::OnChar(nChar, nRepCnt, nFlags);
}

void CStockChartXCtrl::DisplayCaret(int x, int y)
{
	::HideCaret(COleControl::GetSafeHwnd());
	::CreateCaret(COleControl::GetSafeHwnd(), NULL,2,15);
	::SetCaretPos(x,y);
	KillTimer(TIMER_CARET);
	SetTimer(TIMER_CARET, 200, NULL);
}

void CStockChartXCtrl::KillCaret()
{
	KillTimer(TIMER_CARET);
	::SetCaretPos(-1,-1);
	::HideCaret(COleControl::GetSafeHwnd());
}

void CStockChartXCtrl::AddUserDefinedText(LPCTSTR Key) 
{	
	StopDrawing();
	if(m_mouseState == MOUSE_NORMAL){
		drawing = true;
		movingObject = true;
		m_mouseState = MOUSE_DRAWING;
		m_key = Key;
		m_Cursor = IDC_TEXT;
	}
	changed = true;
}

int CStockChartXCtrl::GetSlaveRecordCount()
{
	int ret = (endIndex - startIndex);
	return ret;
	///
	int cnt = 0;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
//	Revision 6/10/2004
//	type cast of unsigned int
				if(panels[n]->series[j]->data_slave.size() > (unsigned int)cnt){
//	End Of Revision
					cnt = panels[n]->series[j]->data_slave.size();
				}
			}
		}
	}	
	return cnt;
}

int CStockChartXCtrl::GetSlaveRecordCount2()
{
	/*int ret = (endIndex - startIndex);
	return ret;*/
	///
	int cnt = 0;
	for(int n = 0; n != panels.size(); ++n){
		//if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
//	Revision 6/10/2004
//	type cast of unsigned int
				if(panels[n]->series[j]->data_slave.size() > (unsigned int)cnt){
//	End Of Revision
					cnt = panels[n]->series[j]->data_slave.size();
				}
			}
		//}
	}	
	return cnt;
}

void CStockChartXCtrl::OnTimer(UINT nIDEvent) 
{	

	if(nIDEvent == TIMER_PAINT){
		KillTimer(nIDEvent);
		reCalc = false;		
		Update();
		return;
	}
	else if(nIDEvent == TIMER_RECALC){
		KillTimer(nIDEvent);
		reCalc = true;
		Update();
		return;
	}
	else if(nIDEvent == TIMER_IND_DLG_CANCEL){ // Sent from CIndPropDlg OnClose
		KillTimer(nIDEvent);
		FireOnDialogCancel();
		return;
	}

	if(nIDEvent > 599 && nIDEvent < 700){ // Child event
		KillTimer(nIDEvent);
		// Send event to all chart panels
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				panels[n]->OnMessage(m_msgGuid, nIDEvent);				
			}
		}
		return;
	}

	switch(nIDEvent){
		case TIMER_CARET:
			::ShowCaret(COleControl::GetSafeHwnd());
			KillTimer(TIMER_CARET);
			break;
 
	}

	COleControl::OnTimer(nIDEvent);
}

void CStockChartXCtrl::OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags) 
{	
	CString serieRemoved;
	// Check for zoom, scroll, delete keys, etc.
	switch(nChar){

	case 189: // zoom out
		
		break;

	case 187: // zoom in
		
		break;
	
	case 46: // delete
		// Delete();
		break;

	case 39: // move right
		
		break;

	case 37: // move left
		
		break;
	}

	FireOnKeyUp(nChar, nRepCnt, nFlags);
  TRACE("Char internal KeyUp");
	
	COleControl::OnKeyUp(nChar, nRepCnt, nFlags);
}

void CStockChartXCtrl::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags) 
{	
	CString serieRemoved;
	// Check for zoom, scroll, delete keys, etc.
	switch(nChar){

	case 189: // zoom out
		
		break;

	case 187: // zoom in
		
		break;
	
	case 46: // delete
		// Delete();
		break;

	case 39: // move right
		
		break;

	case 37: // move left
		
		break;
	}

	FireOnKeyDown(nChar, nRepCnt, nFlags);
  TRACE("Char internal KeyDown");
	
	COleControl::OnKeyDown(nChar, nRepCnt, nFlags);
}

// Recursive function that determines what type of object is selected and deletes it.
void CStockChartXCtrl::Delete(CString groupName)
{
	changed = true;

	m_mouseState = MOUSE_NORMAL;
	m_Cursor = 0;

	// Use recursion to delete remaining members of group.
	if(groupName != ""){
		// Remove all series that belong to this group.
		for(int k = 0; k != panels.size(); ++k){
			int end = panels[k]->series.size();
			for(int l = 0; l != end; ++l){
				CString temp = panels[k]->series[l]->szName;
				int found = temp.Find(".",0);
				if(found > 0){
					temp = temp.Left(found);
					if(temp == groupName){
						RemoveSeries(panels[k]->series[l]->szName);
						Delete(groupName);
						return;
					}
				}
			}
		}		
		UpdateScreen(true);
		return;
	}
	// Are any series selected?
  int n, j;
	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->selected){
				// This series needs to be removed so check
				// to see if it belongs to a group name.
				CString group = panels[n]->series[j]->szName;				
				int found = group.Find(".",0);
				if(found > 0){
					group = group.Left(found);
					// Remove all series that belong to this group.
					for(int k = 0; k != panels.size(); ++k){
						for(int l = 0; l != panels[k]->series.size(); ++l){
							CString temp = panels[k]->series[l]->szName;
							found = temp.Find(".",0);
							if(found > 0){
								temp = temp.Left(found);
								if(temp == group){
									RemoveSeries(panels[k]->series[l]->szName);
									Delete(group);
									return;
								}
							}
						}
					}
				}
				else{
					RemoveSeries(panels[n]->series[j]->szName);
					UpdateScreen(true);
					return;
				}
			}
		}
	}

	// Are there any CCO objects selected?
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->selected){
				delete panels[n]->objects[j];
				panels[n]->objects[j]->selected = false;
				panels[n]->objects[j] = NULL;
				std::vector<CCO*>::iterator itr = panels[n]->objects.begin() + j;
				CCO* elem = *itr;
				panels[n]->objects.erase(itr);
				UpdateScreen(true);
				return;
			}
		}
	}
	
	// Are there any CCOL objects selected?
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->selected){
				delete panels[n]->lines[j];
				panels[n]->lines[j]->selected = false;
				panels[n]->lines[j] = NULL;
				std::vector<CCOL*>::iterator itr = panels[n]->lines.begin() + j;
				CCOL* elem = *itr;
				panels[n]->lines.erase(itr);
				UpdateScreen(true);
				return;
			}
		}
	}

	// Are there any CTextArea objects selected?
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			if(panels[n]->textAreas[j]->selected){
				delete panels[n]->textAreas[j];
				panels[n]->textAreas[j]->selected = false;
				panels[n]->textAreas[j] = NULL;
				std::vector<CTextArea*>::iterator itr = panels[n]->textAreas.begin() + j;
				CTextArea* elem = *itr;
				panels[n]->textAreas.erase(itr);		
				UpdateScreen(true);
				return;
			}
		}
	}

}

long CStockChartXCtrl::GetSeriesType(LPCTSTR Name) 
{
	int style = 0;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					style = panels[n]->series[j]->seriesType;
					return style;
				}
			}	
		}
	}
	return -1;
}

void CStockChartXCtrl::SetSeriesType(LPCTSTR Name, long nNewValue) 
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->seriesType = nNewValue;
					changed = true;
					UpdateScreen(true);
					SetModifiedFlag();
					return;
				}
			}	
		}
	} 
}

long CStockChartXCtrl::GetSeriesStyle(LPCTSTR Name) 
{
	long type = 0;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					type = panels[n]->series[j]->lineStyle;
					return type;
				}
			}			
		}
	}
	return type;	
}

void CStockChartXCtrl::SetSeriesStyle(LPCTSTR Name, long nNewValue) 
{	
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->lineStyle = nNewValue;
					UpdateScreen(true);
					changed = true;
					SetModifiedFlag();
					return;
				}
			}			
		}
	}
}

long CStockChartXCtrl::GetSeriesWeight(LPCTSTR Name) 
{
	long ret = 0;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					return panels[n]->series[j]->lineWeight;
				}
			}	
		}
	}
	return ret;
}

void CStockChartXCtrl::SetSeriesWeight(LPCTSTR Name, long nNewValue) 
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->lineWeight = nNewValue;
					UpdateScreen(true);
					changed = true;
					SetModifiedFlag();
					return;
				}
			}	
		}
	}
}

long CStockChartXCtrl::GetPanelY1(long Panel) 
{
	long ret = -1;
//	Revision 6/10/2004
//	type cast of int
	if(Panel > -1 && Panel < (int)panels.size()){
//	End Of revision
		if(panels[Panel]->visible){
//	Revision 6/10/2004
//	type cast of int
			ret = (long)panels[Panel]->y1;
//	End Of Revision
		}
	}
	return ret;
}

void CStockChartXCtrl::SetPanelY1(long Panel, long nNewValue) 
{
//	Revision 6/10/2004
//	type cast of int
	if(Panel > -1 && Panel < (int)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			panels[Panel]->y1 = nNewValue;
			if(Panel > 0){
				panels[Panel - 1]->y2 = panels[Panel]->y1;
			}
		}
	}
	Refresh();
	changed = true;
	SetModifiedFlag();
}

long CStockChartXCtrl::GetPanelY2(long Panel) 
{
	long ret = -1;
//	Revision 6/10/2004
//	type cast of int
	if(Panel > -1 && Panel < (int)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
//	Revision 6/10/2004
//	type cast of int
			ret = (long)panels[Panel]->y2;
//	End Of Revision
		}
	}
	return ret;
}

void CStockChartXCtrl::SetPanelY2(long Panel, long nNewValue) 
{
//	Revision 6/10/2004
//	type cast of int
	if(Panel > -1 && Panel < (int)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			panels[Panel]->y2 = nNewValue;
//	Revision 6/10/2004
//	type cast of int
			if(Panel < (int)panels.size() - 1){
//	End Of Revision
				panels[Panel + 1]->y1 = panels[Panel]->y2;
			}			
		}
	}
	Refresh();
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::ResetZoom() 
{
	startIndex = 0;
	endIndex = RecordCount();	
	Update();
	FireZoom();
}

OLE_COLOR CStockChartXCtrl::GetChartBackColor() 
{
	return backColor ;
}

void CStockChartXCtrl::SetChartBackColor(OLE_COLOR nNewValue) 
{
	backColor = nNewValue;
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetChartForeColor() 
{
	return foreColor;
}

void CStockChartXCtrl::SetChartForeColor(OLE_COLOR nNewValue) 
{	
	foreColor = nNewValue;
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::AddStaticText(long Panel, LPCTSTR Text, LPCTSTR Key, OLE_COLOR Color, BOOL Selectable, long X, long Y) 
{


//	Revision 6/10/2004
//	type cast of int
	if(Panel > -1 && Panel < (int)panels.size())
	{
//	End Of Revision
		if(panels[Panel]->visible)
		{
			panels[Panel]->textAreas.push_back(new CTextArea(X,Y, panels[Panel]));
			int area = panels[Panel]->textAreas.size() - 1;
			panels[Panel]->textAreas[area]->key = Key;
			panels[Panel]->textAreas[area]->fontColor = Color;
			panels[Panel]->textAreas[area]->Text = Text;
			panels[Panel]->textAreas[area]->Reset();
			panels[Panel]->textAreas[area]->gotUserInput = true;
			panels[Panel]->textAreas[area]->cached = true; // Added 9/27/05
			bool sel = false;
			if(Selectable == TRUE) sel = true;
			panels[Panel]->textAreas[area]->selectable = sel;
			UpdateScreen(true);
		}
	}
}

OLE_COLOR CStockChartXCtrl::GetGridcolor() 
{	
	return gridColor;
}

void CStockChartXCtrl::SetGridcolor(OLE_COLOR nNewValue) 
{
	gridColor = nNewValue;
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetUpColor() 
{
	return upColor;
}

void CStockChartXCtrl::SetUpColor(OLE_COLOR nNewValue) 
{	
	upColor = nNewValue;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetDownColor() 
{	
	return downColor;
}

void CStockChartXCtrl::SetDownColor(OLE_COLOR nNewValue) 
{	
	downColor = nNewValue;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetWickUpColor(void)
{
	return wickUpColor;
}

void CStockChartXCtrl::SetWickUpColor(OLE_COLOR newVal)
{
	wickUpColor = newVal;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

OLE_COLOR CStockChartXCtrl::GetWickDownColor(void)
{
	return wickDownColor;
}

void CStockChartXCtrl::SetWickDownColor(OLE_COLOR newVal)
{
	wickDownColor = newVal;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}
/////////////////////////////////////////////////////////////////////////////
// Returns the number of chart panels that are visible
int CStockChartXCtrl::GetVisiblePanelCount()
{
	int cnt = 0;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			cnt += 1;
		}
	}
	return cnt;
}

/////////////////////////////////////////////////////////////////////////////
// Returns the array index of the chart panel with y1 > y
int CStockChartXCtrl::GetNextLowerChartPanel(double y)
{
	double min = 100000;
	int index = -1;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(panels[n]->y1 > y && panels[n]->y1 < min){
//	Revision 6/10/2004
//	type cast of int
				min = panels[n]->y1;
//	End Of Revision
				index = n;
			}
		}
	}
	return index;
}

/////////////////////////////////////////////////////////////////////////////
// Returns the array index of the chart panel with y1 < y
int CStockChartXCtrl::GetNextHigherChartPanel(double y)
{
	double max = -1;
	int index = -1;
	for(int n = 0; n != MAX_PANELS; ++n){
		if(panels[n]->visible){
			if(panels[n]->y1 < y && panels[n]->y1 > max){
//	Revision 6/10/2004
//	type cast of int
				max = panels[n]->y1;
//	End Of Revision
				index = n;
			}
		}
	}
	return index;
}

/////////////////////////////////////////////////////////////////////////////
// Returns the array index of the chart panel with the top y axe
int CStockChartXCtrl::GetTopChartPanel()
{
	return GetNextLowerChartPanel(-1);
}

/////////////////////////////////////////////////////////////////////////////
// Returns the array index of the chart panel with the bottom y axe
int CStockChartXCtrl::GetBottomChartPanel()
{
	int max = 0;
	int index = -1;
	for(int n = 0; n != MAX_PANELS; ++n){
		if(panels[n]->visible){
			if(panels[n]->y1 > max){
//	Revision 6/10/2004
//	type cast of int
				max = (int)panels[n]->y1;
//	End Of Revision
				index = n;
			}
		}
	}
	return index;
}

/////////////////////////////////////////////////////////////////////////////
// Clears the panels vector and creates new panels without memory leaks
void CStockChartXCtrl::RemoveAllSeries() 
{



	if(userStudyLine.size()>0){
		SaveUserStudies();
	}
	userStudyLine.clear();
	m_Demo = 0;
	m_DemoWarned = false;
	DestroyAll(false);
	barSpacing = 0;	
	if(m_valueView) delete m_valueView;	//5/26/09
	m_valueView = new CValueView();
	m_valueView->Connect(this);
	startIndex = 0;	
	barColors.resize(0);
	panels.resize(MAX_PANELS);
	for(int n = 0; n != MAX_PANELS; ++n){
		panels[n] = new CChartPanel();
		panels[n]->Connect(this);
	}
	UpdateScreen(true);	
}

long CStockChartXCtrl::GetScaleType() 
{
	return scalingType;
}

void CStockChartXCtrl::SetScaleType(long nNewValue) 
{
	scalingType = nNewValue;
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

/////////////////////////////////////////////////////////////////////////////
// Allows user to draw a xor selection to set startIndex and endIndex
void CStockChartXCtrl::ZoomUserDefined() 
{
	userZooming = true;
	m_mouseState = MOUSE_ZOOM;
	m_Cursor = IDC_ZOOMIN;
}

BOOL CStockChartXCtrl::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt) 
{
	int n = endIndex-startIndex;
	int offSet=1;
	if(n<50)offSet=1;
	else if(n<200)offSet=4;
	else if(n<600)offSet=10;
	else offSet=(int)(0.02*n);
	offSet*=2;
	if(locked) return 0;
	
	if(pause) return 0;

	if(zDelta > 0){		
			ScrollRight(offSet);
			Update();
	}
	else if(zDelta < 0){		
			ScrollLeft(offSet);
			Update();
	}	
	return COleControl::OnMouseWheel(nFlags, zDelta, pt);
}

BSTR CStockChartXCtrl::GetSymbol() 
{
	CString strResult;	
	strResult = m_symbol;
	return strResult.AllocSysString();
}

void CStockChartXCtrl::SetSymbol(LPCTSTR lpszNewValue) 
{	
	m_symbol = lpszNewValue;
	changed = true;
	SetModifiedFlag();
}


 


/////////////////////////////////////////////////////////////////////////////

/*	SGC	04.06.2004	BEG	*/
#include <sys/timeb.h>
#define	uint64	unsigned long
uint64		currentTimeMillis()
{
	struct _timeb tb;
	//_ftime_s( &tb ); // For .NET 2005 and up
	_ftime( &tb ); // For .NET 2003, 6.0 and below

	uint64 result = ((uint64)tb.time) * 1000 + tb.millitm;
   	
	return result;
}
/*	SGC	04.06.2004	END	*/
/////////////////////////////////////////////////////////////////////////////

#include "IOStructures.h"


//#include "afx.h"
//#include ".\stockchartxctl.h"



long CStockChartXCtrl::SaveFile(LPCTSTR FileName) 
{
	return Save(FileName, opSaveAll);
}

long CStockChartXCtrl::LoadFile(LPCTSTR FileName) 
{
	return Load(FileName, opLoadAll);
}


//Eugen
//as saver and loader will be used the Strategy Patern.
//see the Gang of Four for its description and reason to use.
//the right saver|loader will be created by Save and Load functions
//accordingly to Operarion value

//TEMPLATES ARE FOR FUTURE USE - NOT CURRENTLY SUPPORTED!!! - supported now, Eugeniu
long CStockChartXCtrl::SaveGeneralTemplate(LPCTSTR FileName) 
{
	return Save(FileName, opSaveGeneral);
}

long CStockChartXCtrl::LoadGeneralTemplate(LPCTSTR FileName) 
{	
	try{
		int result = Load(FileName, opLoadGeneral);	
		RePaint();
		return result;
	}
	catch(...)
	{
		//printf("LoadGeneralTemplate() throw exception...");
	}
	return 0;
}

long CStockChartXCtrl::SaveObjectTemplate(LPCTSTR FileName) 
{
	return Save(FileName, opSaveObjects);
}

long CStockChartXCtrl::LoadObjectTemplate(LPCTSTR FileName) 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nLoadObjectTemplate()");
#endif
	return Load(FileName, opLoadObjects);	
}


#include "SerializerX.h"
// Save workspace and chart data
//everything moved to SerializerX object

long CStockChartXCtrl::Save(LPCTSTR fileName, FileOpType Operation /* = opSaveAll */)
{
  CSerializerX Serializer( *this );
  CStockChartXSerializer *ConcreteSerializer = NULL;

  
 	/*	SGC	04.06.2004	BEG	*/
  uint64	startTime	= currentTimeMillis();
  /*	SGC	04.06.2004	END	*/

 switch( Operation )
  {
    case opSaveAll:     ConcreteSerializer = new CStockChartXSerializer_All    ( *this, Serializer ); break;
    case opSaveGeneral: ConcreteSerializer = new CStockChartXSerializer_General( *this, Serializer ); break;
    case opSaveObjects: ConcreteSerializer = new CStockChartXSerializer_Objects( *this, Serializer ); break;
    default:
	    ThrowErr(1010, "Saving type not suported.");
  }

#ifdef _CONSOLE_DEBUG
  //printf("\nSave() filename=%s",fileName);
#endif
  int iRes = Serializer.Save( fileName, ConcreteSerializer );
  
 /*	SGC	04.06.2004	BEG	*/
	uint64	endTime	= currentTimeMillis();
  uint64	delta	= endTime - startTime;
  /*
  CString	msg;
  msg.Format( "Salvou em %d.%03d segundos", delta/1000, delta%1000 );
  ::AfxMessageBox( msg, MB_OK );
  */
  /*	SGC	04.06.2004	END	*/

  if( iRes == 1 )
    changed = false;

	delete ConcreteSerializer;

  return iRes;
}
/////////////////////////////////////////////////////////////////////////////

long CStockChartXCtrl::Load(LPCTSTR fileName, FileOpType Operation /* = opLoadAll */)
{
	
#ifdef _CONSOLE_DEBUG
  //printf("\nLoad()");
#endif


	/*	SGC	04.06.2004	BEG	*/
	uint64	startTime	= currentTimeMillis();
	/*	SGC	04.06.2004	END	*/
  CSerializerX Serializer( *this );
  CStockChartXSerializer *ConcreteSerializer = NULL;

	loading = true;

  switch( Operation )
  {
    case opLoadAll:     ConcreteSerializer = new CStockChartXSerializer_All    ( *this, Serializer ); break;
    case opLoadGeneral: ConcreteSerializer = new CStockChartXSerializer_General( *this, Serializer ); break;
    case opLoadObjects: ConcreteSerializer = new CStockChartXSerializer_Objects( *this, Serializer ); break;
    default:
      ThrowErr(1010, "Loading type not suported.");
  }
  int iRes = Serializer.Load( fileName, ConcreteSerializer );
	changed = false;
  loading = false;

  delete ConcreteSerializer;

	return iRes;
}
/////////////////////////////////////////////////////////////////////////////









/////////////////////////////////////////////////////////////////////////////
// Save workspace and chart as XML (older functions)
long CStockChartXCtrl::_SaveFile(LPCTSTR FileName) 
{

	/*	SGC	04.06.2004	BEG	*/
	uint64	startTime	= currentTimeMillis();
	/*	SGC	04.06.2004	END	*/

	CXML xml;	

	xml.AddElem("StockChartX-Chart");

	// Save workspace information
	xml.AddChildElem( "Workspace" );
	xml.IntoElem();
	xml.AddChildElem( "Symbol"				, m_symbol );
	xml.AddChildElem( "BackColor"			, ltcstr(backColor) );
	xml.AddChildElem( "ForeColor"			, ltcstr(foreColor) );
	xml.AddChildElem( "GridColor"			, ltcstr(gridColor) );
	xml.AddChildElem( "UpColor"				, ltcstr(upColor) );
	xml.AddChildElem( "DownColor"			, ltcstr(downColor) );
	xml.AddChildElem( "InfoPanelColor"		, ltcstr(valuePanelColor) );
	xml.AddChildElem( "DisplayTitles"		, btcstr(displayTitles) );	
	xml.AddChildElem( "HorizontalSeparators", btcstr(m_horzLines) );
	xml.AddChildElem( "HorizontalSeparatorColor", ltcstr(horzLineColor) );
	xml.AddChildElem( "ThreeDStyle"			, btcstr(threeDStyle) );
	xml.AddChildElem( "StartX"				, ltcstr(startIndex) );
	xml.AddChildElem( "EndX"				, ltcstr(endIndex) );
	xml.AddChildElem( "ExtendedX"			, ltcstr(extendedXPixels) );
	xml.AddChildElem( "ScalingType"			, scaletcstr(scalingType) );
	xml.AddChildElem( "PriceStyle"			, ltcstr(priceStyle) );
	xml.AddChildElem( "ShowYGrid"			, btcstr(showYGrid) );
	xml.AddChildElem( "ShowXGrid"			, btcstr(showXGrid) );
	xml.AddChildElem( "YGridAlignment"		, rltcstr(yAlignment) );
	xml.AddChildElem( "RealTimeXLabels"		, btcstr(realTime) );

	// Chart panels
	double value = 0;
	int count = GetVisiblePanelCount();
	for(int panel = 0; panel != count; ++panel)
	{
		xml.OutOfElem();
		xml.AddChildElem("Panel");
		xml.IntoElem();
		
		// Save x,y
		double y1Scale = (double)panels[panel]->y1 / height;
		double y2Scale = (double)panels[panel]->y2 / height;
		xml.AddChildElem("Y1", dtcstr(y1Scale));
		xml.AddChildElem("Y2", dtcstr(y2Scale));

		// Series
		for(int series = 0; series != panels[panel]->series.size(); ++series)
		{
			xml.AddChildElem("Series");
			xml.IntoElem();
			xml.AddChildElem("Name"			, panels[panel]->series[series]->szName);
			xml.AddChildElem("Title"		, panels[panel]->series[series]->szTitle);
			xml.AddChildElem("SeriesType"	, charttcstr(panels[panel]->series[series]->seriesType));
			xml.AddChildElem("IndicatorType", ltcstr(panels[panel]->series[series]->indicatorType));
			xml.AddChildElem("Link"			, panels[panel]->series[series]->linkTo);
			xml.AddChildElem("UserParams"	, btcstr(panels[panel]->series[series]->userParams));
			xml.AddChildElem("LineStyle"	, linetcstr(panels[panel]->series[series]->lineStyle));
			xml.AddChildElem("LineWeight"	, ltcstr(panels[panel]->series[series]->lineWeight));
			xml.AddChildElem("LineColor"	, ltcstr(panels[panel]->series[series]->lineColor));
			xml.AddChildElem("UpColor"		, ltcstr(panels[panel]->series[series]->upColor));
			xml.AddChildElem("DownColor"	, ltcstr(panels[panel]->series[series]->downColor));
			xml.AddChildElem("Selected"		, btcstr(panels[panel]->series[series]->selected));
			xml.AddChildElem("ShareScale"	, btcstr(panels[panel]->series[series]->shareScale));
			

			// Indicator parameters
			if( panels[panel]->series[series]->seriesType == OBJECT_SERIES_INDICATOR )
			{
				
				xml.AddChildElem("Indicator");
				xml.IntoElem();

				// all data types have equal length
				int size = panels[panel]->series[series]->paramStr.size();
				xml.AddChildElem("ParamCount", ltcstr(size));

				//Strings
				xml.AddChildElem("ParamStr");
				xml.IntoElem();
        int ind;
				for(ind = 0; ind != size; ++ind){
					CString strVal = panels[panel]->series[series]->paramStr[ind];
					xml.AddChildElem("Value", strVal);
				}
				xml.OutOfElem();

				//Doubles
				xml.AddChildElem("ParamDbl");
				xml.IntoElem();
				for(ind = 0; ind != size; ++ind){
					double dblValue = panels[panel]->series[series]->paramDbl[ind];
					xml.AddChildElem("Value", dtcstr(dblValue));
				}
				xml.OutOfElem();

				//Integers
				xml.AddChildElem("ParamInt");
				xml.IntoElem();
				for(ind = 0; ind != size; ++ind){
					long intValue = panels[panel]->series[series]->paramInt[ind];
					xml.AddChildElem("Value", ltcstr(intValue));
				}
				xml.OutOfElem();

				xml.OutOfElem();
			}


			// Data
			xml.AddChildElem("Data");
			xml.IntoElem();
			for(int data = 0; data != panels[panel]->series[series]->data_master.size(); ++data){
				value = panels[panel]->series[series]->data_master[data].value;				
				xml.AddChildElem("Value", dtcstr(value));
				value = panels[panel]->series[series]->data_master[data].jdate;
				xml.AddChildAttrib("JDate", dtcstr(value));
			}


			xml.OutOfElem();
			xml.OutOfElem();
		}

		// Text Areas
		for(int ta = 0; ta != panels[panel]->textAreas.size(); ++ta)
		{
			xml.AddChildElem("Text");
			xml.IntoElem();
			xml.AddChildElem("Key"			, panels[panel]->textAreas[ta]->key);
			xml.AddChildElem("Text"			, panels[panel]->textAreas[ta]->Text);
			xml.AddChildElem("FontColor"	, ltcstr(panels[panel]->textAreas[ta]->fontColor));
			xml.AddChildElem("FontSize"		, ltcstr(panels[panel]->textAreas[ta]->fontSize));
			xml.AddChildElem("StartX"		, ltcstr(panels[panel]->textAreas[ta]->startX));
			xml.AddChildElem("StartY"		, ltcstr(panels[panel]->textAreas[ta]->startY));	
//	Revision 6/10/2004
//	type cast of int		
			xml.AddChildElem("X1"			, ltcstr((int)panels[panel]->textAreas[ta]->x1Value));
//	End Of Revision
			xml.AddChildElem("Y1"			, dtcstr(panels[panel]->textAreas[ta]->y1Value));
//	Revision 6/10/2004
//	type cast of int
			xml.AddChildElem("X2"			, ltcstr((int)panels[panel]->textAreas[ta]->x2Value));
//	End Of Revision
			xml.AddChildElem("Y2"			, dtcstr(panels[panel]->textAreas[ta]->y2Value));					
			xml.AddChildElem("Selectable"	, btcstr(panels[panel]->textAreas[ta]->selectable));
			xml.AddChildElem("Selected"		, btcstr(panels[panel]->textAreas[ta]->selected));
			xml.OutOfElem();
		}


		// Objects
    int object;
		for(object = 0; object != panels[panel]->objects.size(); ++object)
		{
			xml.AddChildElem("Object");
			xml.IntoElem();
			xml.AddChildElem("ObjectType"	, panels[panel]->objects[object]->objectType);
			xml.AddChildElem("Key"			, panels[panel]->objects[object]->key);
			xml.AddChildElem("FileName"		, panels[panel]->objects[object]->fileName);
			xml.AddChildElem("Text"			, panels[panel]->objects[object]->text);
			xml.AddChildElem("BackColor"	, linetcstr(panels[panel]->objects[object]->backColor));			
			xml.AddChildElem("ForeColor"	, ltcstr(panels[panel]->objects[object]->foreColor));
			xml.AddChildElem("LineStyle"	, linetcstr(panels[panel]->objects[object]->lineStyle));
			xml.AddChildElem("LineWeight"	, ltcstr(panels[panel]->objects[object]->lineWeight));
//	Revision 6/10/2004
//	type cast of int
			xml.AddChildElem("X1"			, ltcstr((int)panels[panel]->objects[object]->x1Value));
//	End Of Revision
			xml.AddChildElem("Y1"			, dtcstr(panels[panel]->objects[object]->y1Value));
//	Revision 6/10/2004
//	type cast of int
			xml.AddChildElem("X2"			, ltcstr((int)panels[panel]->objects[object]->x2Value));
//	End Of Revision
			xml.AddChildElem("Y2"			, dtcstr(panels[panel]->objects[object]->y2Value));				
			xml.AddChildElem("Selectable"	, btcstr(panels[panel]->objects[object]->selectable));
			xml.AddChildElem("Selected"		, btcstr(panels[panel]->objects[object]->selected));
			xml.AddChildElem("Visible"		, btcstr(panels[panel]->objects[object]->visible));
			xml.AddChildElem("zOrder"		, ltcstr(panels[panel]->objects[object]->zOrder));
			xml.OutOfElem();
		}


		// Line Objects
		for(object = 0; object != panels[panel]->lines.size(); ++object)
		{
			xml.AddChildElem("Drawing");
			xml.IntoElem();
			xml.AddChildElem("ObjectType"	, panels[panel]->lines[object]->objectType);
			xml.AddChildElem("Key"			, panels[panel]->lines[object]->key);
			xml.AddChildElem("LineStyle"	, linetcstr(panels[panel]->lines[object]->lineStyle));
			xml.AddChildElem("LineWeight"	, ltcstr(panels[panel]->lines[object]->lineWeight));
			xml.AddChildElem("LineColor"	, ltcstr(panels[panel]->lines[object]->lineColor));
//	Revision 6/10/2004
//	type cast of int
			xml.AddChildElem("X1"			, ltcstr((int)panels[panel]->lines[object]->x1Value));
//	End Of Revision
			xml.AddChildElem("Y1"			, dtcstr(panels[panel]->lines[object]->y1Value));
//	Revision 6/10/2004
//	type cast of int
			xml.AddChildElem("X2"			, ltcstr((int)panels[panel]->lines[object]->x2Value));
//	End Of Revision
			xml.AddChildElem("Y2"			, dtcstr(panels[panel]->lines[object]->y2Value));				
			xml.AddChildElem("Selectable"	, btcstr(panels[panel]->lines[object]->selectable));
			xml.AddChildElem("Selected"		, btcstr(panels[panel]->lines[object]->selected));
			xml.AddChildElem("zOrder"		, ltcstr(panels[panel]->lines[object]->zOrder));
			xml.AddChildElem("VFixed"		, btcstr(panels[panel]->lines[object]->vFixed));
			xml.AddChildElem("HFixed"		, btcstr(panels[panel]->lines[object]->hFixed));
			xml.OutOfElem();
		}


		// Horizontal Lines
		xml.AddChildElem("HorizontalLines");
		xml.IntoElem();
		for(object = 0; object != panels[panel]->horizontalLines.size(); ++object)
		{
			xml.AddChildElem("HorzLine", dtcstr(panels[panel]->horizontalLines[object]));
		}
		xml.OutOfElem();

		xml.OutOfElem();
	}

 	HRESULT hr = xml.SaveFile(FileName);

	/*	SGC	04.06.2004	BEG	*/
	uint64	endTime	= currentTimeMillis();
	uint64	delta	= endTime - startTime;

	//CString	msg;
	//msg.Format( "Saved in %d.%d seconds", delta/1000, delta%1000 );
	//::AfxMessageBox( msg, MB_OK );
	/*	SGC	04.06.2004	END	*/

	if(FAILED(hr))
	{
		return -1; // Failed
	}
	else
	{
		changed = false;
		return 1; // Success
	}
}
/////////////////////////////////////////////////////////////////////////////

long CStockChartXCtrl::_LoadFile(LPCTSTR FileName) 
{
	/*	SGC	04.06.2004	BEG	*/
	uint64	startTime	= currentTimeMillis();
	/*	SGC	04.06.2004	END	*/

	loading = true;

	RemoveAllSeries();

	CXML xml;

	// Load XML
	HRESULT hr = xml.LoadFile(FileName);
	if(hr == S_FALSE) return -1;	

	// Load workspace information
	int start = 0;
	int end = 0;
	xml.FindChildElem("Workspace");
	xml.IntoElem();
	xml.FindChildElem("Symbol");
	m_symbol = xml.GetChildData();
	xml.FindChildElem("BackColor");
	backColor = atol(xml.GetChildData());
	xml.FindChildElem("ForeColor");
	foreColor = atol(xml.GetChildData());
	xml.FindChildElem("GridColor");
	gridColor = atol(xml.GetChildData());
	xml.FindChildElem("UpColor");
	upColor = atol(xml.GetChildData());
	xml.FindChildElem("DownColor");
	downColor = atol(xml.GetChildData());
	xml.FindChildElem("InfoPanelColor");
	valuePanelColor = atol(xml.GetChildData());
	xml.FindChildElem("DisplayTitles");
	displayTitles = cstrtb(xml.GetChildData());
	xml.FindChildElem("HorizontalSeparators");
	m_horzLines = cstrtb(xml.GetChildData());	
	m_horizontalSeparators = m_horzLines;
	xml.FindChildElem("HorizontalSeparatorColor");
	horzLineColor = atol(xml.GetChildData());
	m_horizontalSeparatorColor = horzLineColor;
	xml.FindChildElem("ThreeDStyle");
	threeDStyle = cstrtb(xml.GetChildData());
	m_threeDStyle = threeDStyle;
	xml.FindChildElem("StartX");
	start = atol(xml.GetChildData());
	xml.FindChildElem("EndX");
	end = atol(xml.GetChildData());
	xml.FindChildElem("ExtendedX");
	extendedXPixels = atol(xml.GetChildData());
	xml.FindChildElem("ScalingType");
	scalingType = cstrtscale(xml.GetChildData());
	xml.FindChildElem("PriceStyle");
	priceStyle = atol(xml.GetChildData());
	xml.FindChildElem("ShowYGrid");
	showYGrid = cstrtb(xml.GetChildData());
	xml.FindChildElem("ShowXGrid");
	showXGrid = cstrtb(xml.GetChildData());
	xml.FindChildElem("YGridAlignment");
	yAlignment = cstrtrl(xml.GetChildData());
	xml.FindChildElem("RealTimeXLabels");
	realTime = cstrtb(xml.GetChildData());
	xml.OutOfElem();


	// Add chart panels	
	int panel = 0;
	while(xml.FindChildElem("Panel"))
	{
		xml.IntoElem();	
		panel = AddChartPanel();
		xml.OutOfElem();
	}
	xml.ResetPos();
	panel = 0;
	while(xml.FindChildElem("Panel"))
	{
		xml.IntoElem();			
		xml.FindChildElem("Y1");		
		double y1 = (double)height * atof(xml.GetChildData());
		xml.FindChildElem("Y2");		
		double y2 = (double)height * atof(xml.GetChildData());
		panels[panel]->y1 = y1;
		panels[panel]->y2 = y2;
		xml.OutOfElem();
		panel++;
	}	
	xml.ResetPos();
	OnSize(-1,width, height + CALENDAR_HEIGHT);
	xml.ResetPos();
	panel = 0;
	long value = 0;
	double fValue = 0;
	bool bval = false;
	int index = 0;
	while( xml.FindChildElem("Panel") )
	{
		xml.IntoElem();

		// Series
		long indicatorType = 0;
		bool userParams = false;
		int series = 0;
		CString name = "";
		CString title = "";
		CString link = "";
		long type = 0;
		startIndex = 0;
		while(xml.FindChildElem("Series"))
		{
			xml.IntoElem();
			xml.FindChildElem("Name");
			name = xml.GetChildData();
			xml.FindChildElem("Title");
			title = xml.GetChildData();
			xml.FindChildElem("SeriesType");
			type = cstrtchart(xml.GetChildData());			
			xml.FindChildElem("IndicatorType");
			indicatorType = atol(xml.GetChildData());
			xml.FindChildElem("Link");
			link = xml.GetChildData();
			xml.FindChildElem("UserParams");
			userParams = cstrtb(xml.GetChildData());

			if(type == OBJECT_SERIES_INDICATOR)
			{
				AddIndicatorSeries( indicatorType, name, panel, FALSE );
			}
			else
			{
				AddNewSeriesType( panel, type, name );
			}
			panels[panel]->series[series]->szTitle = title;
			panels[panel]->series[series]->linkTo = link;
			panels[panel]->series[series]->userParams = userParams;
			xml.FindChildElem("LineStyle");
			panels[panel]->series[series]->lineStyle = cstrtline(xml.GetChildData());
			xml.FindChildElem("LineWeight");
			panels[panel]->series[series]->lineWeight = atol(xml.GetChildData());
			xml.FindChildElem("LineColor");
			panels[panel]->series[series]->lineColor = atol(xml.GetChildData());
			xml.FindChildElem("UpColor");
			panels[panel]->series[series]->upColor = atol(xml.GetChildData());
			//panels[panel]->series[series]->wickUpColor = atol(xml.GetChildData());
			xml.FindChildElem("DownColor");
			panels[panel]->series[series]->downColor = atol(xml.GetChildData());
			//panels[panel]->series[series]->wickDownColor = atol(xml.GetChildData());
			xml.FindChildElem("Selected");
			panels[panel]->series[series]->selected = cstrtb(xml.GetChildData());
			xml.FindChildElem("ShareScale");
			panels[panel]->series[series]->shareScale = cstrtb(xml.GetChildData());
			

			// Indicator parameters
			if(panels[panel]->series[series]->seriesType == OBJECT_SERIES_INDICATOR){

				long index = 0;
				xml.FindChildElem("Indicator");
				xml.IntoElem();

				// all data types have equal length
				xml.FindChildElem("ParamCount");
				long size = atol(xml.GetChildData());
				panels[panel]->series[series]->paramStr.resize(size);
				panels[panel]->series[series]->paramDbl.resize(size);
				panels[panel]->series[series]->paramInt.resize(size);

				// Strings
				xml.FindChildElem("ParamStr");
				xml.IntoElem();
				index = 0;
				while(xml.FindChildElem("Value")){
					CString strValue = xml.GetChildData();
					panels[panel]->series[series]->paramStr[index] = strValue ;
					index++;
				}
				xml.OutOfElem();

				// Doubles
				xml.FindChildElem("ParamDbl");
				xml.IntoElem();
				index = 0;
				while(xml.FindChildElem("Value"))
				{
					double dblValue = atof(xml.GetChildData());
					panels[panel]->series[series]->paramDbl[index] = dblValue ;
					index++;
				}
				xml.OutOfElem();

				// Integers
				xml.FindChildElem("ParamInt");
				xml.IntoElem();
				index = 0;
				while(xml.FindChildElem("Value")){
					long intValue = atol(xml.GetChildData());
//	Revision 6/10/2004
//	type cast of short
					panels[panel]->series[series]->paramInt[index] = (short)intValue ;
//	End Of Revision
					index++;
				}
				xml.OutOfElem();

				xml.OutOfElem();
			}


			// Data
			int data = 0;
			SeriesPoint sp;
			xml.FindChildElem("Data");
			xml.IntoElem();

			while(xml.FindChildElem("Value")){
				sp.jdate = atof(xml.GetChildAttrib("JDate"));
				sp.value = atof(xml.GetChildData());
				panels[panel]->series[series]->data_master.push_back(sp);
				data++;
			}
			barColors.resize(data, -1); // ToDo: load/save bar colors
			series++;
			xml.OutOfElem();
			xml.OutOfElem();
		
		}
		UpdateScreen(true);


		// Text Areas
		while(xml.FindChildElem("Text")){
			xml.IntoElem();
			panels[panel]->textAreas.push_back(new CTextArea(0,0, panels[panel]));
			index = panels[panel]->textAreas.size() - 1;
			panels[panel]->textAreas[index]->gotUserInput = true;
			xml.FindChildElem("Key");
			panels[panel]->textAreas[index]->key = xml.GetChildData();
			xml.FindChildElem("Text");
			panels[panel]->textAreas[index]->Text = xml.GetChildData();
			xml.FindChildElem("FontColor");
			panels[panel]->textAreas[index]->fontColor = atol(xml.GetChildData());
			xml.FindChildElem("FontSize");
			panels[panel]->textAreas[index]->fontSize = atol(xml.GetChildData());
			xml.FindChildElem("StartX");
			panels[panel]->textAreas[index]->startX = atol(xml.GetChildData());
			xml.FindChildElem("StartY");
			panels[panel]->textAreas[index]->startY = atol(xml.GetChildData());

			xml.FindChildElem("X1");
			value = atol(xml.GetChildData());
			panels[panel]->textAreas[index]->x1Value = value;
//	Revision 6/10/2004
//	type cast of long
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->textAreas[index]->x1 = value;
			xml.FindChildElem("Y1");
			fValue = atof(xml.GetChildData());
			panels[panel]->textAreas[index]->y1Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->textAreas[index]->y1 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("X2");
			value = atol(xml.GetChildData());
			panels[panel]->textAreas[index]->x2Value = value;
//	Revision 6/10/2004
//	type cast of long
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->textAreas[index]->x2 = value;
			xml.FindChildElem("Y2");
			fValue = atof(xml.GetChildData());
			panels[panel]->textAreas[index]->y2Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->textAreas[index]->y2 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("Selectable");
			panels[panel]->textAreas[index]->selectable = cstrtb(xml.GetChildData());
			xml.FindChildElem("Selected");
			panels[panel]->textAreas[index]->selected = cstrtb(xml.GetChildData());
			
			xml.OutOfElem();
		}


		// Objects
		CString objectType = "";

		while(xml.FindChildElem("Object")){
			xml.IntoElem();
			xml.FindChildElem("ObjectType");
			objectType = xml.GetChildData();
			if(objectType == "Buy Symbol"){
				panels[panel]->objects.push_back(new 
					CSymbolObject(0, 0, IDB_BUY, "", "", "Buy Symbol", panels[panel]));
			}
			else if(objectType == "Sell Symbol")
			{
				panels[panel]->objects.push_back(new 
					CSymbolObject(0, 0, IDB_SELL, "", "", "Sell Symbol", panels[panel]));
			}
			else if(objectType == "Exit Symbol")
			{
				panels[panel]->objects.push_back(new 
					CSymbolObject(0, 0, IDB_EXIT, "", "", "Exit Symbol", panels[panel]));
			}
			else if( objectType == "Exit Long Symbol" )
			{
				panels[panel]->objects.push_back( new CSymbolObject( 0, 0, IDB_EXIT_LONG, "", "", "Exit Long Symbol", panels[panel] ) );
			}
			else if( objectType == "Exit Short Symbol" )
			{
				panels[panel]->objects.push_back( new CSymbolObject( 0, 0, IDB_EXIT_SHORT, "", "", "Exit Short Symbol", panels[panel] ) );
			}
			else if( objectType == "Signal Symbol" )
			{
				panels[panel]->objects.push_back( new CSymbolObject( 0, 0, IDB_SIGNAL, "", "", "Signal Symbol", panels[panel] ) );
			}
			else if(objectType == "Custom Symbol"){
				panels[panel]->objects.push_back(new 
					CSymbolObject(0, 0, "", "", "", "Custom Symbol", panels[panel]));
			}
			else{
				panels[panel]->objects.push_back(new CCO());
			}
			//////Add new object types here//////

			index = panels[panel]->objects.size() - 1;
			xml.FindChildElem("FileName");
			panels[panel]->objects[index]->fileName = xml.GetChildData();
			xml.FindChildElem("Key");
			panels[panel]->objects[index]->key = xml.GetChildData();
			xml.FindChildElem("Text");
			panels[panel]->objects[index]->text = xml.GetChildData();
			xml.FindChildElem("BackColor");
			panels[panel]->objects[index]->backColor = atol(xml.GetChildData());
			xml.FindChildElem("ForeColor");
			panels[panel]->objects[index]->foreColor = atol(xml.GetChildData());
			xml.FindChildElem("LineStyle");
			panels[panel]->objects[index]->lineStyle = atol(xml.GetChildData());
			xml.FindChildElem("LineWeight");
			panels[panel]->objects[index]->lineWeight = atol(xml.GetChildData());

			xml.FindChildElem("X1");
			value = atol(xml.GetChildData());
			panels[panel]->objects[index]->x1Value = value;
//	Revision 6/10/2004
//	type cast of long
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->objects[index]->x1 = value;
			xml.FindChildElem("Y1");
			fValue = atof(xml.GetChildData());
			panels[panel]->objects[index]->y1Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->objects[index]->y1 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("X2");
			value = atol(xml.GetChildData());
			panels[panel]->objects[index]->x2Value = value;
//	Revision 6/10/2004
//	type cast of int
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->objects[index]->x2 = value;
			xml.FindChildElem("Y2");
			fValue = atof(xml.GetChildData());
			panels[panel]->objects[index]->y2Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->objects[index]->y2 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("Selectable");
			panels[panel]->objects[index]->selectable = cstrtb(xml.GetChildData());
			xml.FindChildElem("Selected");
			panels[panel]->objects[index]->selected = cstrtb(xml.GetChildData());
			xml.FindChildElem("Visible");
			panels[panel]->objects[index]->visible = cstrtb(xml.GetChildData());
			xml.FindChildElem("zOrder");
			panels[panel]->objects[index]->zOrder = atol(xml.GetChildData());

			panels[panel]->objects[panels[panel]->objects.size() - 1]->Connect(this);
			panels[panel]->objects[panels[panel]->objects.size() - 1]->SetXY();
			panels[panel]->objects[panels[panel]->objects.size() - 1]->UpdatePoints();			

			xml.OutOfElem();
		}


		// Line Objects		
		CString lineType = "";
		CString key = "";
		long style = 0;
		int weight = 0;
		while(xml.FindChildElem("Drawing")){
			xml.IntoElem();
			xml.FindChildElem("ObjectType");
			lineType = xml.GetChildData();
			xml.FindChildElem("Key");
			key = xml.GetChildData();
			xml.FindChildElem("LineStyle");
			style = cstrtline(xml.GetChildData());
			xml.FindChildElem("LineWeight");
			weight = atol(xml.GetChildData());
			xml.FindChildElem("LineColor");
			value = atol(xml.GetChildData());

			if(lineType == "TrendLine"){
				panels[panel]->lines.push_back(new CLineStandard(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "Ellipse"){
				panels[panel]->lines.push_back(new CLineStudyEllipse(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "Rectangle"){
				panels[panel]->lines.push_back(new CLineStudyRectangle(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "Triangle"){
				panels[panel]->lines.push_back(new CLineStudyTriangle(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "Polyline"){
				panels[panel]->lines.push_back(new CLineStudyPolyline(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "SpeedLines"){
				panels[panel]->lines.push_back(new CLineStudySpeedLines(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "GannFan"){
				panels[panel]->lines.push_back(new CLineStudyGannFan(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "FibonacciArcs"){
				panels[panel]->lines.push_back(new CLineStudyFibonacciArcs(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "FibonacciFan"){
				panels[panel]->lines.push_back(new CLineStudyFibonacciFan(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "FibonacciTimeZones"){
				panels[panel]->lines.push_back(new CLineStudyFibonacciTimeZones(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "TironeLevels"){
				panels[panel]->lines.push_back(new CLineStudyTironeLevels(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "QuadrantLines"){
				panels[panel]->lines.push_back(new CLineStudyQuadrantLines(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "ErrorChannel"){
				panels[panel]->lines.push_back(new CLineStudyErrorChannel(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			else if(lineType == "Freehand"){
				panels[panel]->lines.push_back(new CLineStudyFreehand(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			} 
			else if(lineType == "Channel"){
				panels[panel]->lines.push_back(new CLineStudyChannel(value, key, panels[panel]));
				panels[panel]->lines[panels[panel]->lines.size() - 1]->Connect(this);
			}
			

			/////// Add new line object types here ///////

			index = panels[panel]->lines.size() - 1;
			
			panels[panel]->lines[index]->lineWeight = weight;
			xml.FindChildElem("X1");
			value = atol(xml.GetChildData());
			panels[panel]->lines[index]->x1Value = value;
//	Revision 6/10/2004
//	type cast of long
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->lines[index]->x1 = value;
			xml.FindChildElem("Y1");
			fValue = atof(xml.GetChildData());
			panels[panel]->lines[index]->y1Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->lines[index]->y1 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("X2");
			value = atol(xml.GetChildData());
			panels[panel]->lines[index]->x2Value = value;
//	Revision 6/10/2004
//	type cast of long
			value = (long)panels[0]->GetX(value);
//	End Of Revision
			panels[panel]->lines[index]->x2 = value;
			xml.FindChildElem("Y2");
			fValue = atof(xml.GetChildData());
			panels[panel]->lines[index]->y2Value = fValue;
			fValue = panels[0]->GetY(fValue);
//	Revision 6/10/2004
//	type cast of int
			panels[panel]->lines[index]->y2 = (int)fValue;
//	End Of Revision
			xml.FindChildElem("Selectable");
			bval = cstrtb(xml.GetChildData());
			panels[panel]->lines[index]->selectable = bval;
			xml.FindChildElem("Selected");
			bval = cstrtb(xml.GetChildData());
			panels[panel]->lines[index]->selected = bval;			
			xml.FindChildElem("zOrder");
			value = cstrtb(xml.GetChildData());
			panels[panel]->lines[index]->zOrder = value;
			xml.FindChildElem("VFixed");
			bval = cstrtb(xml.GetChildData());
			panels[panel]->lines[index]->vFixed = bval;
			xml.FindChildElem("HFixed");
			bval = cstrtb(xml.GetChildData());
			panels[panel]->lines[index]->hFixed = bval;
			
			xml.OutOfElem();
		}



		// Horizontal Lines	
		double hline = -1;
		xml.FindChildElem("HorizontalLines");
		xml.IntoElem();
		while(xml.FindChildElem("HorzLine"))
		{
			hline = atof(xml.GetChildData());
			panels[panel]->AddHorzLine(hline);			
		}
		xml.OutOfElem();





		panel++;
		xml.OutOfElem();
	}

	// Calculate all indicators
	loading = false;
	int count = GetVisiblePanelCount();
	for(panel = 0; panel != count; ++panel){
		for(int series = 0; series != panels[panel]->series.size(); ++series){
			if(panels[panel]->series[series]->seriesType == OBJECT_SERIES_INDICATOR){
//				panels[panel]->series[series]->Calculate();
			}
		}
	}
	
	startIndex = start;
	endIndex = end;
	//CalculateScaleInfo();
	Update();

	changed = false;


	/*	SGC	04.06.2004	BEG	*/
	uint64	endTime	= currentTimeMillis();
	uint64	delta	= endTime - startTime;

	//CString	msg;
	//msg.Format( "Loaded in %d.%d seconds", delta/1000, delta%1000 );
	//::AfxMessageBox( msg, MB_OK );
	/*	SGC	04.06.2004	END	*/


	return 1;
}
/////////////////////////////////////////////////////////////////////////////

CString CStockChartXCtrl::dtcstr(double value)
{
	CString toStr;
	toStr.Format("%f",value);
	return toStr;
}

CString CStockChartXCtrl::ltcstr(long value)
{
	CString toStr;
	toStr.Format("%d",value);
	return toStr;
}

CString CStockChartXCtrl::btcstr(bool value)
{
	if(value){
		return "True";
	}
	else{
		return "False";
	}
}

bool CStockChartXCtrl::cstrtb(CString value)
{
	if(value == "True"){
		return true;
	}
	else{
		return false;
	}
}

CString CStockChartXCtrl::rltcstr(long value)
{
	if(value == RIGHT){
		return "Right";
	}
	else{
		return "Left";
	}
}

long CStockChartXCtrl::cstrtrl(CString value)
{
	if(value == "Right"){
		return RIGHT;
	}
	else{
		return LEFT;
	}
}

CString CStockChartXCtrl::scaletcstr(long value)
{
	if(value == LINEAR){
		return "Linear";
	}
	else{
		return "Semi-Log";
	}
}

long CStockChartXCtrl::cstrtscale(CString value)
{
	if(value == "Linear"){
		return LINEAR;
	}
	else{
		return SEMILOG;
	}
}

CString CStockChartXCtrl::linetcstr(long value)
{
	if(value == PS_SOLID){
		return "Solid";
	}
	else if(value == PS_DASH){
		return "Dash";
	}
	else if(value == PS_DOT){
		return "Dot";
	}
	else if(value == PS_DASHDOT){
		return "Dash-Dot";
	}
	else{
		return "Solid";
	}
}

long CStockChartXCtrl::cstrtline(CString value)
{
	if(value == "Solid"){
		return PS_SOLID;
	}
	else if(value == "Dash"){
		return PS_DASH;
	}
	else if(value == "Dot"){
		return PS_DOT;
	}
	else if(value == "Dash-Dot"){
		return PS_DASHDOT;
	}
	else{
		return PS_SOLID;
	}
}

CString CStockChartXCtrl::charttcstr(long value)
{
	if(value == OBJECT_SERIES_STOCK_LINE){
		return "StockLine";
	}
	else if(value == OBJECT_SERIES_BAR){
		return "Bar";
	}
	else if(value == OBJECT_SERIES_STOCK){
		return "StockHLC";
	}
	else if(value == OBJECT_SERIES_STOCK_HLC){
		return "Stock";
	}
	else if(value == OBJECT_SERIES_CANDLE){
		return "Candle";
	}
	else if(value == OBJECT_SERIES_PF){
		return "P&F";
	}
	else if(value == OBJECT_SERIES_RENKO){
		return "Renko";
	}
	else if(value == OBJECT_SERIES_INDICATOR){
		return "Indicator";
	}
	else{
		return "Line";
	}
}

long CStockChartXCtrl::cstrtchart(CString value)
{
	if(value == "StockLine"){
		return OBJECT_SERIES_STOCK_LINE;
	}
	else if(value == "Bar"){
		return OBJECT_SERIES_BAR;
	}
	else if(value == "Stock"){
		return OBJECT_SERIES_STOCK;
	}
	else if(value == "StockHLC"){
		return OBJECT_SERIES_STOCK_HLC;
	}
	else if(value == "Candle"){
		return OBJECT_SERIES_CANDLE;
	}
	else if(value == "P&F"){
		return OBJECT_SERIES_PF;
	}
	else if(value == "Renko"){
		return OBJECT_SERIES_RENKO;
	}
	else if(value == "Indicator"){
		return OBJECT_SERIES_INDICATOR;
	}
	else{
		return OBJECT_SERIES_LINE;
	}
}


long CStockChartXCtrl::GetScaleAlignment() 
{	
	return yAlignment;
}

void CStockChartXCtrl::SetScaleAlignment(long nNewValue) 
{	
	yAlignment = nNewValue;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

long CStockChartXCtrl::GetWorkspaceLeft() 
{
	if(yAlignment == LEFT){
		return yScaleWidth + 5;
	}
	else{
		return 0;
	}
}

long CStockChartXCtrl::GetWorkspaceRight() 
{	
	if(yAlignment == LEFT){
		return width;
	}
	else{
		return width - yScaleWidth;
	}
	return 0;
}

void CStockChartXCtrl::SetWorkspaceRight(long nNewValue) 
{
	SetModifiedFlag();
}
void CStockChartXCtrl::SetWorkspaceLeft(long nNewValue) 
{
	SetModifiedFlag();
}

void CStockChartXCtrl::ClearDrawings() 
{



	/*
	for(int n = 0; n != panels.size(); ++n){
		panels[n]->lines.clear();
	}
	for(n = 0; n != panels.size(); ++n){
		panels[n]->objects.clear();
	}
	*/
	  
	DestroyAll(false);
	userStudyLine.clear();
	SaveUserStudies();
	UpdateScreen(true);
}

void CStockChartXCtrl::OnKillFocus(CWnd* pNewWnd) 
{	
#ifdef _CONSOLE_DEBUG
	//printf("\nOnKillFocus()");
#endif
	COleControl::OnKillFocus(pNewWnd);	
	focus = false;
}

void CStockChartXCtrl::GetCtrlFocus()
{		
#ifdef _CONSOLE_DEBUG
	//printf("\nGetCtrlFocus()");
#endif
	UpdateRect(CRect(0,0,width,height + CALENDAR_HEIGHT));
	focus = true;
}

BOOL CStockChartXCtrl::GetChanged() 
{
	if(changed){
		return TRUE;
	}
	else{
		return FALSE;
	}
}

void CStockChartXCtrl::SetChanged(BOOL bNewValue) 
{
	if(bNewValue == TRUE){
		changed = true;
	}
	else{
		changed = false;
	}
	changed = true;
	SetModifiedFlag();
}

long CStockChartXCtrl::GetVisibleRecordCount() 
{
	return endIndex - startIndex;
}

void CStockChartXCtrl::SetVisibleRecordCount(long nNewValue) 
{	
	if(nNewValue<=RecordCount()){
		endIndex = RecordCount();	
		startIndex = endIndex-nNewValue;
	}
	else{
		endIndex = RecordCount();	
		startIndex = 0;
	}
	Update();
	SetModifiedFlag();
}

long CStockChartXCtrl::GetRecordCount() 
{
	int cnt = 0;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
				if(panels[n]->series[j]->GetRecordCount() > cnt){

					// Added indicator check - 9/21/04 RG
					if(panels[n]->series[j]->indicatorType == -1){
						cnt = panels[n]->series[j]->GetRecordCount();
					}

				}
			}
		}
	}
	return cnt;	
}

void CStockChartXCtrl::SetRecordCount(long nNewValue) 
{	
	SetModifiedFlag();
}


long CStockChartXCtrl::GetFirstVisibleRecord() 
{	
	return startIndex + 1;
}

void CStockChartXCtrl::SetFirstVisibleRecord(long nNewValue) 
{	
	if(nNewValue < GetRecordCount() + 1 && 
			nNewValue > 0 && nNewValue < endIndex){
		startIndex = nNewValue - 1;		
		UpdateScreen(true);
		changed = true;



	}
	else{






	}
}

long CStockChartXCtrl::GetLastVisibleRecord() 
{
	return endIndex;
}

void CStockChartXCtrl::SetLastVisibleRecord(long nNewValue) 
{
	if(nNewValue < GetRecordCount() + 1 && 
			nNewValue > 0 && nNewValue > startIndex){
		endIndex = nNewValue;
		UpdateScreen(true);
		changed = true;



	}
	else{






	}
}


// 6/21/89 - adapted from pBrush - LR
OLE_COLOR CStockChartXCtrl::ComputeInverseColor(DWORD rgb, CDC* pDC){
        
	HBITMAP hTempBit1;
	HBITMAP hTempBit2;
	HDC hTempDC1;
	HDC hTempDC2;
	HDC hdc;
	HANDLE hOldObj1;
	HANDLE hOldObj2;
	DWORD rgbInv;
    
	hdc = pDC->m_hDC;
	hTempDC1 = CreateCompatibleDC(hdc);
	hTempDC2 = CreateCompatibleDC(hdc);
    
	/* create two temporary 1x1, 16 color bitmaps */
	hTempBit1 = CreateCompatibleBitmap(hdc, 1, 1);
	hTempBit2 = CreateCompatibleBitmap(hdc, 1, 1);        
    
	hOldObj1 = SelectObject(hTempDC1, hTempBit1);
	hOldObj2 = SelectObject(hTempDC2, hTempBit2);
    
	/* method for getting inverse color : set the given pixel (rgb) on
	* one DC. Now blt it to the other DC using a SRCINVERT rop.
	* This yields a pixel of the inverse color on the destination DC
	*/
	SetPixel(hTempDC1, 0, 0, rgb);
	PatBlt(hTempDC2, 0, 0, 1, 1, WHITENESS);
	BitBlt(hTempDC2, 0, 0, 1, 1, hTempDC1, 0, 0, SRCINVERT);
	rgbInv = GetPixel(hTempDC2, 0, 0);
    
	/* clean up ... */
	SelectObject(hTempDC1, hOldObj1);
	SelectObject(hTempDC2, hOldObj2);
	DeleteObject(hTempBit1);
	DeleteObject(hTempBit2);
	DeleteDC(hTempDC1);
	DeleteDC(hTempDC2);
    
	/* ...and return the inverted RGB value */
	return rgbInv;

 }

void CStockChartXCtrl::ShowLastTick(LPCTSTR Series, double Value) 
{
	double visibleValue;
	double lastTick;
	double tickColor;
	CString stringSeries = Series;
	stringSeries.Delete(stringSeries.GetLength()-6,6);
	// Find all series
	CSeries* series = NULL;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = panels[n]->series.size()-1; j >= 0; --j){
				if((panels[n]->series[j]->data_master.size()>0)&&(panels[n]->series[j]->szName.Find(stringSeries)<0 || (panels[n]->series[j]->szName.Find(".open")<1 && panels[n]->series[j]->szName.Find(".high")<1 && panels[n]->series[j]->szName.Find(".low")<1 && panels[n]->series[j]->szName.Find(".volume")<1))){
					Value = GetValue(panels[n]->series[j]->szName, endIndex);
					series = panels[n]->series[j];
					if(Value==NULL_VALUE||!m_yScaleDrawn || series->dataError)continue;
					CDC* pDC = &m_memDC;

					//Normalize value:


					lastTick = Value;
					tickColor = series->lineColor;

					// Get the value panel border rect
					CRect rect;
					if(yAlignment == LEFT){
						rect.left = 1 + 5;
						rect.right = yScaleWidth - 2;
					}
					else{
						rect.left = width - yScaleWidth + 5;
						rect.right = width - 2; 
					}
				//	Revision 6/10/2004
				//	Addition of type cast of long
					rect.top = (long)series->ownerPanel->y1;
					rect.bottom = (long)series->ownerPanel->y2;	
				//	End Of Revision
					if(yAlignment == LEFT){
						rect.right -= 1;
					}
					else{
						rect.left += 1;
					}

					// Get font info
					CFont newFont;
					newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial Rounded MT Bold"), pDC);	
					TEXTMETRIC tm;
					CFont* pOldFont = pDC->SelectObject(&newFont);
					pDC->GetTextMetrics(&tm);
				//	Revision 6/10/2004
				//	Addition of type cast of long
					int nAvgWidth = (int)(1 + (tm.tmAveCharWidth * 0.8));
				//	End Of Revision
					int nCharHeight = 1 + (tm.tmHeight);

					// Print last-tick box
					double y = 0;
					y = series->ownerPanel->GetY(lastTick);		
					if(y==NULL_VALUE || y>series->ownerPanel->y2 || y<series->ownerPanel->y1)continue;
				//	Revision 6/10/2004
				//	Addition of type cast of long
					rect.top = (long)y - nCharHeight/2;
					if(rect.top<(long)series->ownerPanel->y1+5)rect.top = (long)series->ownerPanel->y1+5;
				//	End Of Revision
					rect.bottom = rect.top + nCharHeight;
					if(rect.bottom>(long)series->ownerPanel->y2-5){
						rect.top -= rect.bottom-((long)series->ownerPanel->y2-5);
						rect.bottom = (long)series->ownerPanel->y2-5;
					}
					if(rect.bottom<0){
						continue;
					}
				//	Revision 6/10/2004
				//	Addition of type cast of DWORD
					CBrush* brh = new CBrush((DWORD)tickColor);
				//	End Of Revision
					CBrush* pOldBrush = pDC->SelectObject(brh);
					pDC->FillRect(rect, brh);	




					// XOR reverse color
				//	Revision 6/10/2004
				//	Addition of type cast of DWORD
					long xorColor = GetContrastColor(series->lineColor); //ComputeInverseColor((DWORD)tickColor, pDC);
				//	End Of Revision
					pDC->SetTextColor(xorColor);
					CString szValue = "";
					int decimalsTemp = (abs(lastTick) > 9999.99) ? 1 : decimals;
					CString volume =series->szName;
					volume.MakeLower();
					//printf("\n volume = %s ",volume);
					if ((((volume.Find(".volume") == -1 && volume.Find("vol") == 0) || volume.Find("a/d") != -1 || volume.Find("obv") != -1) && panels[n]->index != 0) || volume.Find("IBOV") > -1)
					{
						//lastTick/=1000000;
						//decimalsTemp = 0;	
						//szValue.Format("%.*f", decimalsTemp, lastTick);
						//szValue+=m_volumePostfix;
						if(lastTick >= 1000000000 )
						{						

							lastTick = lastTick / 1000000000;
							szValue.Format("%.*f", decimalsTemp, lastTick);
							szValue += " B";
						}
						else if(lastTick >= 1000000 )
						{

							lastTick = lastTick / 1000000;
							szValue.Format("%.*f", decimalsTemp, lastTick);
							szValue += " M";
						}
						else if(lastTick >= 1000 )
						{

							lastTick = lastTick / 1000;
							szValue.Format("%.*f", decimalsTemp, lastTick);
							szValue += " K";
						}
						else
						{
							szValue.Format("%.*f", decimalsTemp, lastTick);						
						}
					}
					else szValue.Format("%.*f", decimalsTemp, lastTick);	
					//pDC->DrawText(szValue, -1, &rect, DT_SINGLELINE | DT_CENTER | DT_VCENTER);

					CRectF rectTextF = CRectF(rect.left,rect.top,rect.right,rect.bottom);
					pdcHandler->DrawText(szValue,rectTextF,"Arial Rounded MT",11,DT_CENTER,xorColor,255,pDC);	

					pDC->SelectObject(pOldBrush);
					pDC->SelectObject(pOldFont);
					newFont.DeleteObject();
					brh->DeleteObject();
					if(brh) delete brh;
					UpdateRect(rect);
				}
			}
		}
	}

	/*DELETE
	// Find series
	CSeries* series = NULL;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Series, panels[n]->series[j]->szName)){
					series = panels[n]->series[j];
					break;
				}
			}
		}
	}
	if(NULL == series) return;

	CDC* pDC = &m_memDC;

	lastTick = Value;
	tickColor = series->lineColor;

	// Get the value panel border rect
	CRect rect;
	if(yAlignment == LEFT){
		rect.left = 1 + 7;
		rect.right = yScaleWidth - 7;
	}
	else{
		rect.left = width - yScaleWidth + 7;
		rect.right = width - 7; 
	}
//	Revision 6/10/2004
//	Addition of type cast of long
	rect.top = (long)series->ownerPanel->y1;
	rect.bottom = (long)series->ownerPanel->y2;	
//	End Of Revision
	if(yAlignment == LEFT){
		rect.right -= 1;
	}
	else{
		rect.left += 1;
	}

	// Get font info
	CFont newFont;
	newFont.CreatePointFont(VALUE_FONT_SIZE-10, _T("Arial"), pDC);	
	TEXTMETRIC tm;
	CFont* pOldFont = pDC->SelectObject(&newFont);
	pDC->GetTextMetrics(&tm);
//	Revision 6/10/2004
//	Addition of type cast of long
	int nAvgWidth = (int)(1 + (tm.tmAveCharWidth * 0.8));
//	End Of Revision
	int nCharHeight = 1 + (tm.tmHeight);

	// Print last-tick box
	double y = 0;
	y = series->ownerPanel->GetY(lastTick);		
//	Revision 6/10/2004
//	Addition of type cast of long
	rect.top = (long)y - nCharHeight/2;
//	End Of Revision
	rect.bottom = rect.top + nCharHeight;
//	Revision 6/10/2004
//	Addition of type cast of DWORD
	CBrush* brh = new CBrush((DWORD)tickColor);
//	End Of Revision
	CBrush* pOldBrush = pDC->SelectObject(brh);
	pDC->FillRect(rect, brh);		
	// XOR reverse color
//	Revision 6/10/2004
//	Addition of type cast of DWORD
	long xorColor = ComputeInverseColor((DWORD)tickColor, pDC);
//	End Of Revision
	pDC->SetTextColor(xorColor);
	CString szValue = "";
	szValue.Format("%.*f", decimals, lastTick);	
	pDC->DrawText(szValue, -1, &rect, DT_SINGLELINE | DT_CENTER | DT_VCENTER);		
	pDC->SelectObject(pOldBrush);
	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();
	brh->DeleteObject();
	if(brh) delete brh;
	UpdateRect(rect);
	*/

}

// Redraws a panel's y scale
void CStockChartXCtrl::ReDrawYScale(long Panel) 
{
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel < (long)panels.size() - 1 && Panel > -1){
//	End Of Revision
		CRect rect = panels[Panel]->DrawYScale(&m_memDC);
		UpdateRect(rect);
	}
}

// Returns a panel index by series name
long CStockChartXCtrl::GetPanelBySeriesName(LPCTSTR Series) 
{	
	CString name;
	long ret = -1;
	int n;
	for(n = 0; n != panels.size(); ++n){		
		for(int j = 0; j != panels[n]->series.size(); ++j){
			name = panels[n]->series[j]->szName;
			if(CompareNoCase(Series, name)){
				return n;
			}
		}
	}

	// No series? Then try objects...
	for(n = 0; n != panels.size(); ++n){		
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			name = panels[n]->objects[j]->key;
			if(CompareNoCase(Series, name)){
				return n;
			}
		}
	}

	// No objects? Then try lines...
	for(n = 0; n != panels.size(); ++n){		
		for(int j = 0; j != panels[n]->lines.size(); ++j){
			name = panels[n]->lines[j]->key;
			if(CompareNoCase(Series, name)){
				return n;
			}
		}
	}

	return ret;
}

long CStockChartXCtrl::GetPanelByName(LPCTSTR Series){

	return GetPanelBySeriesName(Series);
}

void CStockChartXCtrl::OnSelectSeries(CString name)
{
	FireSelectSeries(name);
}


void CStockChartXCtrl::PrintChart() 
{
	AddStaticText(0, "Plena Trading Platform", "Copyright", panels[0]->series[0]->lineColor, true, 20, panels[0]->y2-20);
	// Get mem dc's bitmap size
	BITMAP bitmap;
	m_bitmap.GetObject(sizeof(BITMAP), (LPSTR)&bitmap);
	int width = bitmap.bmWidth;
	int height = bitmap.bmHeight;

	OLE_COLOR oldBackColor = backColor;
	OLE_COLOR oldForeColor = foreColor;
	backColor = RGB(255,255,255);
	foreColor = RGB(0,0,0);
	UpdateScreen(true);





	// Paint on bmp
	CBitmap bmp;
	bmp.CreateCompatibleBitmap(&m_memDC,bitmap.bmWidth,bitmap.bmHeight);	
	CBitmap* pOldBmp = m_memDC.SelectObject(&bmp);

	// Draw the chart with a white background
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){	
			panels[n]->Invalidate();
			panels[n]->OnPaint(&m_memDC);
		}
	}

	CCalendarPanel cp;
	cp.OnPaint(&m_memDC,this);
	
	RemoveObject(OBJECT_TEXT,"Copyright");

	// Restore old bitmap
	if(pOldBmp) m_memDC.SelectObject(pOldBmp);
	
	// Reset the chart with the old background
	foreColor = oldForeColor;
	backColor = oldBackColor;
	UpdateScreen(true);



	// Print the chart
	HPALETTE hPal = NULL;    // handle to the logical palette
	RGBQUAD rgbQ[256];
    int num_got = GetDIBColorTable(m_memDC, 0, 256, rgbQ);
    if (num_got > 0){
        //
        // compute the number of bytes needed for
        // the LOGPALETTE struct plus its colors
        //
        const size_t pal_size =
        sizeof(LOGPALETTE) +
        (num_got - 1) * sizeof(PALETTEENTRY);

        // allocate the memory
        PLOGPALETTE pLogPal = reinterpret_cast<PLOGPALETTE>(
				new unsigned char[pal_size]
                );
		memset(pLogPal, 0, pal_size);

        // initialize the LOGPALETTE structure
        pLogPal->palVersion = 0x300;
        pLogPal->palNumEntries = 256;

        // initialize the colors
        for (int index = 0; index < num_got; ++index){
			pLogPal->palPalEntry[index].peRed =
            rgbQ[index].rgbRed;
            pLogPal->palPalEntry[index].peGreen =
            rgbQ[index].rgbGreen;
            pLogPal->palPalEntry[index].peBlue =
            rgbQ[index].rgbBlue;
		}

        // create the palette
        hPal = CreatePalette(pLogPal);

		// clean up
        delete [] reinterpret_cast<unsigned char*>(pLogPal);
	}

	PRINTDLG pd = {sizeof(PRINTDLG)};
    pd.hwndOwner = this->GetSafeHwnd();
    pd.nCopies = 1;
    pd.nFromPage = 1;
    pd.nToPage = 1;
    pd.nMinPage = 1;
    pd.nMaxPage = 1;
    pd.Flags = PD_RETURNDC;

    if (PrintDlg(&pd)){

		DEVMODE *dm=(DEVMODE *)GlobalLock(pd.hDevMode);
		dm->dmOrientation = DMORIENT_LANDSCAPE;
		GBitmapPrinter BitmapPrinter;
        BitmapPrinter.SetBitmap(bmp, hPal);        
        BitmapPrinter.SetPrintScale(GBP_SCALE_PAGE);
        BitmapPrinter.SetDocName("StockChartX");
        BitmapPrinter.PrintBitmapDoc(pd.hDC);		
        DeleteDC(pd.hDC);
        if (pd.hDevMode) GlobalFree(pd.hDevMode);
        if (pd.hDevNames) GlobalFree(pd.hDevNames);
	}

}

// Adds a new bitmap chart symbol object to the chart
void CStockChartXCtrl::AddNewSymbolObject(long panel, long style, double x, 
											double y, CString key, CString text)
{
	CString label = "";
	DWORD bmp = 0;

	switch(style){
		
		case BUY_SYMBOL: // add a new buy arrow
			bmp = IDB_BUY;
			label = "Buy Symbol";			
			break;
		case SELL_SYMBOL: // add a new sell arrow			
			bmp = IDB_SELL;
			label = "Sell Symbol";
			break;
		case EXIT_SYMBOL: // add a new exit symbol
			bmp = IDB_EXIT;
			label = "Exit Symbol";
			break;
		case EXIT_LONG_SYMBOL: // add a new exit long symbol
			bmp = IDB_EXIT_LONG;
			label = "Exit Long Symbol";
			break;
		case EXIT_SHORT_SYMBOL: // add a new exit short symbol
			bmp = IDB_EXIT_SHORT;
			label = "Exit Short Symbol";
			break;
		case SIGNAL_SYMBOL: // add a new signal symbol
			bmp = IDB_SIGNAL;
			label = "Signal Symbol";
			break;
	}

	panels[panel]->objects.push_back(new CSymbolObject(x, y, bmp, key, text, label, panels[panel]));
	panels[panel]->objects[panels[panel]->objects.size() - 1]->Connect(this);
	panels[panel]->objects[panels[panel]->objects.size() - 1]->SetXY();
	panels[panel]->objects[panels[panel]->objects.size() - 1]->CalculateXJulianDate();
	panels[panel]->objects[panels[panel]->objects.size() - 1]->UpdatePoints();
	panels[panel]->objects[panels[panel]->objects.size() - 1]->Show();
	panels[panel]->OnUpdate();
	changed = true;
}


void CStockChartXCtrl::AddSymbolObjectFromFile(long Panel, double Value, long Record, LPCTSTR FileName, LPCTSTR Key, LPCTSTR Text) 
{
	// add a new symbol from bitmap file
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			panels[Panel]->objects.push_back(new CSymbolObject(Record, Value, FileName, Key, Text, "Custom Symbol", panels[Panel]));
			panels[Panel]->objects[panels[Panel]->objects.size() - 1]->Connect(this);
			panels[Panel]->objects[panels[Panel]->objects.size() - 1]->SetXY();
			panels[Panel]->objects[panels[Panel]->objects.size() - 1]->UpdatePoints();
			panels[Panel]->objects[panels[Panel]->objects.size() - 1]->Show();
			panels[Panel]->OnUpdate();
			changed = true;
		}
	}
}

void CStockChartXCtrl::AddSymbolObject(long Panel, double Value, long Record, 
									   long Type, LPCTSTR Key, LPCTSTR Text) 
{	
	Update();
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			AddNewSymbolObject(Panel, Type, Record, Value, Key, Text);
		}
	}
}
void CStockChartXCtrl::RemoveSymbolObject(LPCTSTR Key) 
{
	int n = 0;	
	for(n = 0; n != panels.size(); ++n){	
		if(panels[n]->visible){		
			for(int j = 0; j != panels[n]->objects.size(); ++j){
				if(panels[n]->objects[j]->key == Key){					
					panels[n]->objects[j]->selected = false;					
					delete panels[n]->objects[j]; // Updated 2/1/08
					panels[n]->objects[j] = NULL;
					std::vector<CCO*>::iterator itr = panels[n]->objects.begin() + j;
					CCO* elem = *itr;
					panels[n]->objects.erase(itr);
					UpdateScreen(true);
					return;
				}
			}
		}
	}
	UpdateScreen(true);
	changed = true;
}

long CStockChartXCtrl::GetSeriesCount() 
{
	int cnt = 0;
	int n = 0;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			cnt += panels[n]->series.size();
		}
	}
	return cnt;
}

void CStockChartXCtrl::AddUserSymbolObject(long Type, LPCTSTR Key, LPCTSTR Text) 
{		
	StopDrawing();
	movingObject = true;
	m_mouseState = MOUSE_SYMBOL;
	switch(Type){
		case BUY_SYMBOL:
			m_Cursor = IDC_SYMBOL_BUY;
			break;
		case SELL_SYMBOL:
			m_Cursor = IDC_SYMBOL_SELL;
			break;
		case EXIT_SYMBOL:
			m_Cursor = IDC_SYMBOL_EXIT;
			break;
		default:
			m_Cursor = IDC_SYMBOL;
			break;
	}
	m_symbolName = Key;
	m_symbolType = Type;
	m_symbolText = Text;
}

void CStockChartXCtrl::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	/*if(lineDrawing=true && currentLine->nType == lsPolyline){
		lineDrawing = false;
		m_mouseState = MOUSE_NORMAL;
		m_Cursor = 0;
	}*/
	if(locked) return;
	m_clickPoint = point;	
	// Send OnDoubleClick event to all chart panels
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				panels[n]->OnDoubleClick(point);				
			}
		}
	}
	FireDoubleClick();
	COleControl::OnLButtonDblClk(nFlags, point);
}

void CStockChartXCtrl::DelayMessage(CString guid, int msgID, long delay)
{
	if (msgID != MSG_POINTOUT && msgID != TIMER_PAINT && msgID != TIMER_RECALC) return;
	if(MSG_POINTOUT && DeltaCursor && m_valueView->visible)return;
	m_msgGuid = guid;
	KillTimer(msgID);
	SetTimer(msgID, delay, NULL);
}

void CStockChartXCtrl::OnDisplayInfoTextChanged() 
{	
	if(m_displayInfoText == TRUE){
		displayInfoText = true;
	}
	else{
		displayInfoText = false;
	}	
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::FireOnItemRightClick(int type, CString name)
{
#ifdef _CONSOLE_DEBUG
	printf("\nOnItemRightClick(%d,%s)",type,name);
#endif
	FireItemRightClick(type, name, m_point.x, m_point.y);
	m_onRClickFired = true;
	UnSelectAll();
	
}

void CStockChartXCtrl::FireOnItemLeftClick(int type, CString name)
{
	FireItemLeftClick(type, name, m_point.x, m_point.y);
}

void CStockChartXCtrl::FireOnItemDoubleClick(int type, CString name)
{




	FireItemDoubleClick(type, name, m_point.x, m_point.y);




}

void CStockChartXCtrl::FireOnItemMouseMove(int type, CString name)
{
	FireItemMouseMove(type, name, m_point.x, m_point.y);
}

void CStockChartXCtrl::NotifyDialogShown()
{
	FireShowDialog();
}

void CStockChartXCtrl::FireOnDialogShown()
{	
	FireShowDialog();
}

void CStockChartXCtrl::FireOnDialogHiden()
{
	FireHideDialog();
}

void CStockChartXCtrl::FireOnDialogCancel()
{
	FireDialogCancel();
}

// Deletes any object specified by ObjectType and Key
void CStockChartXCtrl::RemoveObject(long ObjectType, LPCTSTR Key) 
{

	int n = 0;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						RemoveUserStudyLine(Key, panels[n]->series[0]->szName);						
						panels[n]->textAreas[j] = NULL;
						delete panels[n]->textAreas[j];
						std::vector<CTextArea*>::iterator itr = panels[n]->textAreas.begin() + j;
						CTextArea* elem = *itr;
						panels[n]->textAreas.erase(itr);
						break;
					}
				}
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						RemoveUserStudyLine(Key, panels[n]->series[0]->szName);
						delete panels[n]->objects[j];
						panels[n]->objects[j] = NULL;
						std::vector<CCO*>::iterator itr = panels[n]->objects.begin() + j;
						CCO* elem = *itr;
						panels[n]->objects.erase(itr);
						break;
					}
				}
			}
		}
		break;	
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
						RemoveUserStudyLine(Key, panels[n]->series[0]->szName);
						delete panels[n]->lines[j];
						panels[n]->lines[j] = NULL;
						std::vector<CCOL*>::iterator itr = panels[n]->lines.begin() + j;
						CCOL* elem = *itr;
						panels[n]->lines.erase(itr);
						break;
					}
				}
			}
		}
		break;

	}
	if(m_symbol.GetLength()>2){
		CString path = m_StudyDirectory;
		path.Append(m_symbol);
		path.Append(".sty");
		SaveObjectTemplate(path);
	}
	UpdateScreen(true);

}

OLE_COLOR CStockChartXCtrl::GetObjectColor(long ObjectType, LPCTSTR Key) 
{	
	OLE_COLOR color = RGB(0,0,0);
	int n = 0;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						color = panels[n]->textAreas[j]->fontColor;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						//color = panels[n]->objects[j]->color;
						//Not supported
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
						color = panels[n]->lines[j]->lineColor;
						break;
					}
				}				
			}
		}
		break;

	}
	return color;
}

void CStockChartXCtrl::SetObjectColor(long ObjectType, LPCTSTR Key, OLE_COLOR nNewValue) 
{	
	int n = 0;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						panels[n]->textAreas[j]->fontColor = nNewValue;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						//panels[n]->objects[j]->color = nNewValue;
						//Not supported
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
						panels[n]->lines[j]->lineColor = nNewValue;
						break;
					}
				}				
			}
		}
		break;

	}
	RePaint();
	changed = true;
	SetModifiedFlag();
}

BOOL CStockChartXCtrl::GetObjectSelectable(long ObjectType, LPCTSTR Key) 
{
	int n = 0;
	BOOL selectable = FALSE;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						if(panels[n]->textAreas[j]->selectable) selectable = TRUE;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						if(panels[n]->objects[j]->selectable) selectable = TRUE;						
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
						if(panels[n]->lines[j]->selectable) selectable = TRUE;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SERIES_INDICATOR:
	case OBJECT_SERIES_PF:
	case OBJECT_SERIES_RENKO:
	case OBJECT_SERIES_STOCK:
	case OBJECT_SERIES_STOCK_HLC:
	case OBJECT_SERIES_STOCK_LINE:
	case OBJECT_SERIES_CANDLE:
	case OBJECT_SERIES_LINE:
	case OBJECT_SERIES_BAR:
		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(panels[n]->series[j]->szName, Key)){
						if(panels[n]->series[j]->selectable) selectable = TRUE;						
						break;
					}
				}				
			}
		}
		break;
	}

	return selectable;
}

void CStockChartXCtrl::SetObjectSelectable(long ObjectType, LPCTSTR Key, BOOL bNewValue) 
{
	bool selectable = false;
	if(bNewValue == TRUE) selectable = true;
	int n = 0;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						panels[n]->textAreas[j]->selectable = selectable;
						break;
					}
				}
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case EXIT_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						panels[n]->objects[j]->selectable = selectable;						
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
						panels[n]->lines[j]->selectable = selectable;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SERIES_INDICATOR:
	case OBJECT_SERIES_PF:
	case OBJECT_SERIES_RENKO:
	case OBJECT_SERIES_STOCK:
	case OBJECT_SERIES_STOCK_HLC:	
	case OBJECT_SERIES_STOCK_LINE:
	case OBJECT_SERIES_CANDLE:
	case OBJECT_SERIES_LINE:
	case OBJECT_SERIES_BAR:
		for(n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(panels[n]->series[j]->szName, Key)){
						panels[n]->series[j]->selectable = selectable;
						break;
					}
				}				
			}
		}
		break;
	}

	changed = true;
	SetModifiedFlag();
}

BSTR CStockChartXCtrl::GetObjectText(long ObjectType, LPCTSTR Key) 
{
	int n = 0;
	CString text = "";
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						text = panels[n]->textAreas[j]->Text;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						text = panels[n]->objects[j]->text;
						break;
					}
				}				
			}
		}
		break;
	}
	return text.AllocSysString();
}

void CStockChartXCtrl::SetObjectText(long ObjectType, LPCTSTR Key, LPCTSTR lpszNewValue) 
{
	int n = 0;	
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
						panels[n]->textAreas[j]->Text = lpszNewValue;
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						panels[n]->objects[j]->text = lpszNewValue;
						break;
					}
				}				
			}
		}
		break;
	}
	RePaint();
	SetModifiedFlag();
}

void CStockChartXCtrl::OnRealTimeXLabelsChanged() 
{
	if(m_realTimeXLabels == TRUE){
		realTime = true;
	}
	else{
		realTime = false;
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

OLE_HANDLE CStockChartXCtrl::GetHWnd() 
{
	return (OLE_HANDLE) COleControl::GetSafeHwnd();
}

/*
void CStockChartXCtrl::OnPauseChanged() 
{	
	return;
	if(this->m_pause == TRUE){
		pause = true;
		m_setcap = false;
		m_valueView->Reset(true);
		m_Cursor = 0;
		m_buttonState = MOUSE_NORMAL;
	}
	else{
		pause = false;
	}
	SetModifiedFlag();
}

void CStockChartXCtrl::Release() 
{
	return;
	m_valueView->Reset(true);
	m_setcap = false;
	::ReleaseCapture();
}
*/

long CStockChartXCtrl::GetObjectStyle(long ObjectType, LPCTSTR Name) 
{
OLE_COLOR color = RGB(0,0,0);
	int n = 0;
	int style = 0;
	switch(ObjectType){	
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Name){
						style = panels[n]->lines[j]->lineStyle;
						break;
					}
				}				
			}
		}
		break;
	}
	return style;
}

void CStockChartXCtrl::SetObjectStyle(long ObjectType, LPCTSTR Name, long nNewValue) 
{
	int n = 0;	
	switch(ObjectType){	
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Name){
						panels[n]->lines[j]->lineStyle = nNewValue;
						break;
					}
				}				
			}
		}
		break;
	}	
	RePaint();
	changed = true;
	SetModifiedFlag();
}

long CStockChartXCtrl::GetObjectWeight(long ObjectType, LPCTSTR Name) 
{
	int n = 0;
	int nWidth = 0;
	switch(ObjectType){	
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Name){
						nWidth = panels[n]->lines[j]->lineWeight;
						break;
					}
				}				
			}
		}
		break;
	}
	return nWidth;	
}

void CStockChartXCtrl::SetObjectWeight(long ObjectType, LPCTSTR Name, long nNewValue) 
{
	int n = 0;	
	switch(ObjectType){	
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Name){
						panels[n]->lines[j]->lineWeight = nNewValue;
						break;
					}
				}				
			}
		}
		break;
	}	
	changed = true;
	RePaint();
	SetModifiedFlag();
}



// Updated 6/2/2005
void CStockChartXCtrl::SaveChartBitmap(LPCTSTR FileName) 
{
	double min_aux = GetVisibleMinValue(m_symbol +".low");
    double max = GetVisibleMaxValue(m_symbol +".high");
    double min = min_aux - ((double)10 / 100) *(max -min_aux);
	//SetYScale(0,max,min);
	//Show static text on bottom?
	//AddStaticText(0, "Plena Trading Platform", "Copyright", panels[0]->series[0]->lineColor, true, 20, panels[0]->y2-20);
	savingBitmap = true;

	int oldH, oldW;
	//Maximize:
	oldH = height;
	oldW = width;
	//OnSize(1, 1024, 768);

	Update();
	CDC* pDC = GetDC();
	ASSERT(pDC != NULL);
	
	DrawComponent(pDC,true);
	DeleteDC((HDC)pDC);
	

	HPALETTE    hPalette;
    HDIB        hDIB = NULL;
	hPalette = GetSystemPalette();

	hDIB = BitmapToDIB(m_bitmap,  hPalette);

	SaveDIB(hDIB, FileName);

	DeleteObject(hPalette);
    DeleteObject(hDIB);
	GlobalFree(hDIB);
	//RemoveObject(OBJECT_TEXT,"Copyright");
	savingBitmap = false;

	//OnSize(1, oldW, oldH);
	
	//SetYScale(0,max,min_aux);
	DrawComponent(pDC);
}




/*************************************************************************
 *
 * BitmapToDIB()
 *
 * Parameters:
 *
 * HBITMAP hBitmap  - specifies the bitmap to convert
 *
 * HPALETTE hPal    - specifies the palette to use with the bitmap
 *
 * Return Value:
 *
 * HANDLE             - identifies the device-dependent bitmap
 *
 * Description:
 *
 * This function creates a DIB from a bitmap using the specified palette.
 *
 ************************************************************************/

HANDLE CStockChartXCtrl::BitmapToDIB(HBITMAP hBitmap, HPALETTE hPal)
{
    BITMAP              bm;         // bitmap structure
    BITMAPINFOHEADER    bi;         // bitmap header
    LPBITMAPINFOHEADER  lpbi;       // pointer to BITMAPINFOHEADER
    DWORD               dwLen;      // size of memory block
    HANDLE              hDIB, h;    // handle to DIB, temp handle
    HDC                 hDC;        // handle to DC
    WORD                biBits;     // bits per pixel

    // check if bitmap handle is valid

    if (!hBitmap)
        return NULL;

    // fill in BITMAP structure, return NULL if it didn't work

    if (!GetObject(hBitmap, sizeof(bm), (LPSTR)&bm))
        return NULL;

    // if no palette is specified, use default palette

    if (hPal == NULL)
        hPal = (HPALETTE) GetStockObject(DEFAULT_PALETTE);

    // calculate bits per pixel

    biBits = bm.bmPlanes * bm.bmBitsPixel;

    // make sure bits per pixel is valid

    if (biBits <= 1)
        biBits = 1;
    else if (biBits <= 4)
        biBits = 4;
    else if (biBits <= 8)
        biBits = 8;
    else // if greater than 8-bit, force to 24-bit
        biBits = 24;

    // initialize BITMAPINFOHEADER

    bi.biSize = sizeof(BITMAPINFOHEADER);
    bi.biWidth = bm.bmWidth;
    bi.biHeight = bm.bmHeight;
    bi.biPlanes = 1;
    bi.biBitCount = biBits;
    bi.biCompression = BI_RGB;
    bi.biSizeImage = 0;
    bi.biXPelsPerMeter = 0;
    bi.biYPelsPerMeter = 0;
    bi.biClrUsed = 0;
    bi.biClrImportant = 0;

    // calculate size of memory block required to store BITMAPINFO

    dwLen = bi.biSize + PaletteSize((LPSTR)&bi);

    // get a DC

    hDC = ::GetDC(NULL);

    // select and realize our palette
	
    hPal = SelectPalette( hDC, hPal, FALSE);

    RealizePalette(hDC);

    // alloc memory block to store our bitmap

    hDIB = GlobalAlloc(GHND, dwLen);

    // if we couldn't get memory block

    if (!hDIB)
    {
      // clean up and return NULL

      SelectPalette(hDC, hPal, TRUE);
      RealizePalette(hDC);
      ::ReleaseDC(NULL,hDC);
      return NULL;
    }

    // lock memory and get pointer to it

    lpbi = (LPBITMAPINFOHEADER)GlobalLock(hDIB);

    /// use our bitmap info. to fill BITMAPINFOHEADER

    *lpbi = bi;

    // call GetDIBits with a NULL lpBits param, so it will calculate the
    // biSizeImage field for us    

    GetDIBits(hDC, hBitmap, 0, (UINT)bi.biHeight, NULL, (LPBITMAPINFO)lpbi,
        DIB_RGB_COLORS);

    // get the info. returned by GetDIBits and unlock memory block

    bi = *lpbi;
    GlobalUnlock(hDIB);

    // if the driver did not fill in the biSizeImage field, make one up 
    if (bi.biSizeImage == 0)
        bi.biSizeImage = WIDTHBYTES((DWORD)bm.bmWidth * biBits) * bm.bmHeight;

    // realloc the buffer big enough to hold all the bits

    dwLen = bi.biSize + PaletteSize((LPSTR)&bi) + bi.biSizeImage;

    if (h = GlobalReAlloc(hDIB, dwLen, 0))
        hDIB = h;
    else
    {
        // clean up and return NULL

        GlobalFree(hDIB);
        hDIB = NULL;
        SelectPalette(hDC, hPal, TRUE);
        RealizePalette(hDC);
        ::ReleaseDC(NULL,hDC);
        return NULL;
    }

    // lock memory block and get pointer to it */

    lpbi = (LPBITMAPINFOHEADER)GlobalLock(hDIB);

    // call GetDIBits with a NON-NULL lpBits param, and actualy get the
    // bits this time

    if (GetDIBits(hDC, hBitmap, 0, (UINT)bi.biHeight, (LPSTR)lpbi +
            (WORD)lpbi->biSize + PaletteSize((LPSTR)lpbi), (LPBITMAPINFO)lpbi,
            DIB_RGB_COLORS) == 0)
    {
        // clean up and return NULL

        GlobalUnlock(hDIB);
        hDIB = NULL;
        SelectPalette(hDC, hPal, TRUE);
        RealizePalette(hDC);
        ::ReleaseDC(NULL,hDC);
        return NULL;
    }

    bi = *lpbi;

    // clean up 
    GlobalUnlock(hDIB);
    SelectPalette(hDC, hPal, TRUE);
    RealizePalette(hDC);
    ::ReleaseDC(NULL,hDC);

    // return handle to the DIB
    return hDIB;
}




/*************************************************************************
 *
 * PaletteSize()
 *
 * Parameter:
 *
 * LPSTR lpDIB      - pointer to packed-DIB memory block
 *
 * Return Value:
 *
 * WORD             - size of the color palette of the DIB
 *
 * Description:
 *
 * This function gets the size required to store the DIB's palette by
 * multiplying the number of colors by the size of an RGBQUAD (for a
 * Windows 3.0-style DIB) or by the size of an RGBTRIPLE (for an OS/2-
 * style DIB).
 *
 ************************************************************************/

 WORD CStockChartXCtrl::PaletteSize(LPSTR lpDIB)
{
	if (!lpDIB)
		return (WORD) 0;

    // calculate the size required by the palette
    if (IS_WIN30_DIB (lpDIB))
        return (DIBNumColors(lpDIB) * sizeof(RGBQUAD));
    else
        return (DIBNumColors(lpDIB) * sizeof(RGBTRIPLE));
}




/*************************************************************************
 *
 * PalEntriesOnDevice()
 *
 * Parameter:
 *
 * HDC hDC          - device context
 *
 * Return Value:
 *
 * int              - number of palette entries on device
 *
 * Description:
 *
 * This function gets the number of palette entries on the specified device
 *
 ************************************************************************/
int CStockChartXCtrl::PalEntriesOnDevice(HDC hDC)
{
    int nColors;  // number of colors

    // Find out the number of colors on this device.
    
    nColors = (1 << (GetDeviceCaps(hDC, BITSPIXEL) * GetDeviceCaps(hDC, PLANES)));
    
    return nColors;
}



/*************************************************************************
 *
 * GetSystemPalette()
 *
 * Parameters:
 *
 * None
 *
 * Return Value:
 *
 * HPALETTE         - handle to a copy of the current system palette
 *
 * Description:
 *
 * This function returns a handle to a palette which represents the system
 * palette.  The system RGB values are copied into our logical palette using
 * the GetSystemPaletteEntries function.  
 *
 ************************************************************************/

HPALETTE CStockChartXCtrl::GetSystemPalette(void)
{
    HDC hDC;                // handle to a DC
    static HPALETTE hPal = NULL;   // handle to a palette
    HANDLE hLogPal;         // handle to a logical palette
    LPLOGPALETTE lpLogPal;  // pointer to a logical palette
    int nColors;            // number of colors

    // Find out how many palette entries we want.

    hDC = ::GetDC(NULL);

    if (!hDC)
        return NULL;

    nColors = PalEntriesOnDevice(hDC);   // Number of palette entries

    // Allocate room for the palette and lock it.

    hLogPal = GlobalAlloc(GHND, sizeof(LOGPALETTE) + nColors *
            sizeof(PALETTEENTRY));

    // if we didn't get a logical palette, return NULL

    if (!hLogPal)
        return NULL;

    // get a pointer to the logical palette

    lpLogPal = (LPLOGPALETTE)GlobalLock(hLogPal);

    // set some important fields

    lpLogPal->palVersion = PALVERSION;
    lpLogPal->palNumEntries = (WORD) nColors;

    // Copy the current system palette into our logical palette

    GetSystemPaletteEntries(hDC, 0, nColors,
            (LPPALETTEENTRY)(lpLogPal->palPalEntry));

    // Go ahead and create the palette.  Once it's created,
    // we no longer need the LOGPALETTE, so free it.    

    hPal = CreatePalette(lpLogPal);

    // clean up

    GlobalUnlock(hLogPal);
    GlobalFree(hLogPal);
    ::ReleaseDC(NULL,hDC);

    return hPal;
}





/*************************************************************************
 *
 * DIBNumColors()
 *
 * Parameter:
 *
 * LPSTR lpDIB      - pointer to packed-DIB memory block
 *
 * Return Value:
 *
 * WORD             - number of colors in the color table
 *
 * Description:
 *
 * This function calculates the number of colors in the DIB's color table
 * by finding the bits per pixel for the DIB (whether Win3.0 or OS/2-style
 * DIB). If bits per pixel is 1: colors=2, if 4: colors=16, if 8: colors=256,
 * if 24, no colors in color table.
 *
 ************************************************************************/

 WORD CStockChartXCtrl::DIBNumColors(LPSTR lpDIB)
{
    WORD wBitCount;  // DIB bit count

	if (!lpDIB)
		return (WORD) 0;

    // If this is a Windows-style DIB, the number of colors in the
    // color table can be less than the number of bits per pixel
    // allows for (i.e. lpbi->biClrUsed can be set to some value).
    // If this is the case, return the appropriate value.
    

    if (IS_WIN30_DIB(lpDIB))
    {
        DWORD dwClrUsed;

        dwClrUsed = ((LPBITMAPINFOHEADER)lpDIB)->biClrUsed;
        if (dwClrUsed)

        return (WORD)dwClrUsed;
    }

    // Calculate the number of colors in the color table based on
    // the number of bits per pixel for the DIB.
    
    if (IS_WIN30_DIB(lpDIB))
        wBitCount = ((LPBITMAPINFOHEADER)lpDIB)->biBitCount;
    else
        wBitCount = ((LPBITMAPCOREHEADER)lpDIB)->bcBitCount;

    // return number of colors based on bits per pixel

    switch (wBitCount)
    {
        case 1:
            return 2;

        case 4:
            return 16;

        case 8:
            return 256;

        default:
            return 0;
    }
}



/*************************************************************************
 *
 * SaveDIB()
 *
 * Saves the specified DIB into the specified file name on disk.  No
 * error checking is done, so if the file already exists, it will be
 * written over.
 *
 * Parameters:
 *
 * HDIB hDib - Handle to the dib to save
 *
 * LPSTR lpFileName - pointer to full pathname to save DIB under
 *
 * Return value: 0 if successful, or one of:
 *        ERR_INVALIDHANDLE
 *        ERR_OPEN
 *        ERR_LOCK
 *
 *************************************************************************/

WORD CStockChartXCtrl::SaveDIB(HDIB hDib, LPCSTR lpFileName)
{
    BITMAPFILEHEADER    bmfHdr;     // Header for Bitmap file
    LPBITMAPINFOHEADER  lpBI;       // Pointer to DIB info structure
    HANDLE              fh;         // file handle for opened file
    DWORD               dwDIBSize;
    DWORD               dwWritten;

    if (!hDib)
        return ERR_INVALIDHANDLE;

	if (!lpFileName)
		return ERR_FILENOTFOUND;

    fh = CreateFile(lpFileName, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
            FILE_FLAG_SEQUENTIAL_SCAN | SECURITY_ANONYMOUS | SECURITY_SQOS_PRESENT, NULL);

    if (fh == INVALID_HANDLE_VALUE)
        return ERR_OPEN;

	if ( GetFileType(fh) != FILE_TYPE_DISK )
	{
			CloseHandle(fh);
			return ERR_OPEN;
	}

    // Get a pointer to the DIB memory, the first of which contains
    // a BITMAPINFO structure

    lpBI = (LPBITMAPINFOHEADER)GlobalLock(hDib);
    if (!lpBI)
    {
        CloseHandle(fh);
        return ERR_LOCK;
    }

    // Check to see if we're dealing with an OS/2 DIB.  If so, don't
    // save it because our functions aren't written to deal with these
    // DIBs.

    if (lpBI->biSize != sizeof(BITMAPINFOHEADER))
    {
        GlobalUnlock(hDib);
        CloseHandle(fh);
        return ERR_NOT_DIB;
    }

    // Fill in the fields of the file header

    // Fill in file type (first 2 bytes must be "BM" for a bitmap)

    bmfHdr.bfType = DIB_HEADER_MARKER;  // "BM"

    // Calculating the size of the DIB is a bit tricky (if we want to
    // do it right).  The easiest way to do this is to call GlobalSize()
    // on our global handle, but since the size of our global memory may have
    // been padded a few bytes, we may end up writing out a few too
    // many bytes to the file (which may cause problems with some apps,
    // like HC 3.0).
    //
    // So, instead let's calculate the size manually.
    //
    // To do this, find size of header plus size of color table.  Since the
    // first DWORD in both BITMAPINFOHEADER and BITMAPCOREHEADER conains
    // the size of the structure, let's use this.

    // Partial Calculation

    dwDIBSize = *(LPDWORD)lpBI + PaletteSize((LPSTR)lpBI);  

    // Now calculate the size of the image

    // It's an RLE bitmap, we can't calculate size, so trust the biSizeImage
    // field

    if ((lpBI->biCompression == BI_RLE8) || (lpBI->biCompression == BI_RLE4))
        dwDIBSize += lpBI->biSizeImage;
    else
    {
        DWORD dwBmBitsSize;  // Size of Bitmap Bits only

        // It's not RLE, so size is Width (DWORD aligned) * Height

        dwBmBitsSize = WIDTHBYTES((lpBI->biWidth)*((DWORD)lpBI->biBitCount)) *
                lpBI->biHeight;

        dwDIBSize += dwBmBitsSize;

        // Now, since we have calculated the correct size, why don't we
        // fill in the biSizeImage field (this will fix any .BMP files which 
        // have this field incorrect).

        lpBI->biSizeImage = dwBmBitsSize;
    }


    // Calculate the file size by adding the DIB size to sizeof(BITMAPFILEHEADER)
                   
    bmfHdr.bfSize = dwDIBSize + sizeof(BITMAPFILEHEADER);
    bmfHdr.bfReserved1 = 0;
    bmfHdr.bfReserved2 = 0;

    // Now, calculate the offset the actual bitmap bits will be in
    // the file -- It's the Bitmap file header plus the DIB header,
    // plus the size of the color table.
    
    bmfHdr.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) + lpBI->biSize +
            PaletteSize((LPSTR)lpBI);

    // Write the file header

    WriteFile(fh, (LPSTR)&bmfHdr, sizeof(BITMAPFILEHEADER), &dwWritten, NULL);

    // Write the DIB header and the bits -- use local version of
    // MyWrite, so we can write more than 32767 bytes of data
    
    WriteFile(fh, (LPSTR)lpBI, dwDIBSize, &dwWritten, NULL);

    GlobalUnlock(hDib);
    CloseHandle(fh);

    if (dwWritten == 0)
        return ERR_OPEN; // oops, something happened in the write
    else
        return 0; // Success code
}


long CStockChartXCtrl::GetRightDrawingSpacePixels() 
{
	return extendedXPixels;	
}

void CStockChartXCtrl::SetRightDrawingSpacePixels(long nNewValue) 
{
	extendedXPixels = nNewValue;
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::ForcePaint() 
{

#ifdef _CONSOLE_DEBUG
	printf("\nForcePaint()");
#endif



	CDC* pDC = GetDC();
	ASSERT(pDC != NULL);
	DrawComponent(pDC );
	DeleteDC((HDC)pDC);
	Refresh();

	
	// 12/21/2006
	userZooming = false;
	startUserZooming = -1;
	endUserZooming = -1;
	m_mouseState = MOUSE_NORMAL;
	m_Cursor = 0;			
	m_valueView->Hide();
	resizing = false;
	movingObject = false;
	m_valueView->Reset(true); 
	bResetMemDC = true;
	m_buttonState = MOUSE_UP;


}

void CStockChartXCtrl::ForceRePaint()
{
	ForcePaint();
}

void CStockChartXCtrl::OnVersionChanged() 
{
	version = m_version;
	SetModifiedFlag();
}

void CStockChartXCtrl::LogEvent(CString text)
{
	CString FileName = "C:\\Stx.log";
	try
	{
		CFile file( FileName, CFile::modeWrite | CFile::modeCreate | CFile::modeNoTruncate);
		file.SeekToEnd();
		file.Write("\n", 1);
		file.Write(text.GetBuffer(0), text.GetLength() );
		file.Close();
	}
	catch (CFileException*)
	{
		//
	}
}

BOOL CStockChartXCtrl::GetShareScale(LPCTSTR Series) 
{
	BOOL ret = FALSE;
	for(int n = 0; n != panels.size(); ++n){		
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(CompareNoCase(Series, panels[n]->series[j]->szName)){
				if(panels[n]->series[j]->shareScale == TRUE){
					ret = TRUE;
				}
				break;
			}
		}
	}
	return TRUE;
}

void CStockChartXCtrl::SetShareScale(LPCTSTR Series, BOOL bNewValue) 
{	
	for(int n = 0; n != panels.size(); ++n){		
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(CompareNoCase(Series, panels[n]->series[j]->szName)){
				if(bNewValue == TRUE){
					panels[n]->series[j]->shareScale = true;
				}
				else{
					panels[n]->series[j]->shareScale = false;
				}
				break;
			}
		}
	}	
	SetModifiedFlag();
}

// Changed 12/01/03 from AddTrendLine(Panel, X1, Y1, X2, Y2, Key)
void CStockChartXCtrl::DrawTrendLine(long Panel, double LeftValue, long LeftRecord, double RightValue, long RightRecord, LPCTSTR Key) 
{
	try{
		panels[Panel]->lines.push_back(new CLineStandard(lineColor, Key, panels[Panel]));
		currentLine = panels[Panel]->lines[panels[Panel]->lines.size() - 1];
		currentLine->Connect(this);
		
		currentLine->x1Value = LeftRecord;
		currentLine->y1Value = LeftValue;
		currentLine->x2Value = RightRecord;
		currentLine->y2Value = RightValue;
		
		currentLine->SnapLine();

	}
	catch(...){
		ThrowError( CUSTOM_CTL_SCODE(1003), "Failed to add trendline" );
	}
	UpdateScreen(true);
}


long CStockChartXCtrl::GetSeriesColor(LPCTSTR Name) 
{
	bool twin = false;
	CString sName = Name;
	if(CString(Name).Find("HILO")==0 && CString(Name).Find(" Low")>0){
		twin = true;
		sName.Replace(" Low","");
		Name = sName;



	}
	long ret = RGB(0,0,0);
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					if(!twin)ret = panels[n]->series[j]->lineColor;
					else ret = panels[n]->series[j]->lineColorSignal;
				}
			}	
		}
	}
	return ret;
}

void CStockChartXCtrl::SetSeriesColor(LPCTSTR Name, long nNewValue) 
{
	CString name = Name;
	name.MakeLower();
	if(name.Find(".close") > 0){
		//wickColor = nNewValue;
		return; //Calculate by brightness
	}
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->lineColor = nNewValue;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnScalePrecisionChanged() 
{
	if(m_scalePrecision > 6) m_scalePrecision = 6;
	if(m_scalePrecision < 0) m_scalePrecision = 0;
	decimals = m_scalePrecision;
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnUseLineSeriesUpDownColorsChanged() 
{	
	if(m_useLineSeriesUpDownColors == TRUE){
		useLineSeriesColors = true;
	}
	else{
		useLineSeriesColors = false;
	}
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnUseVolumeUpDownColorsChanged() 
{
	if(m_useVolumeUpDownColors == TRUE){
		useVolumeUpDownColors = true;
	}
	else{
		useVolumeUpDownColors = false;
	}
	UpdateScreen(true);
	SetModifiedFlag();
}


void CStockChartXCtrl::DrawCrossHairs(CPoint point)
{
	if(drawing || m_pauseCrossHairs) return;

	CDC* pDC = GetScreen();
	pDC->SetROP2(R2_NOT);

	// Horizontal line
	if(yAlignment == RIGHT){
		newRect.left = 0;
		newRect.right = width - yScaleWidth;
	}
	else{
		newRect.left = yScaleWidth;
		newRect.right = width;
	}				
	if(point.y < height){ 
			
		newRect.bottom = point.y;
		newRect.top = point.y;

		// Erase horizontal line
		if (oldHRect.left != -1 && oldHRect.right != -1 &&  // FROEDE_MARK
			oldHRect.top != -1 && oldHRect.bottom != -1) {  //
		  pDC->MoveTo(oldHRect.left,oldHRect.top);
		  pDC->LineTo(oldHRect.right,oldHRect.bottom);
		}

		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);
		oldHRect = newRect;

	}

	// Vertical line
	bool show = false;
	if(yAlignment == RIGHT){
		if(point.x < width - yScaleWidth){
			show = true;
		}
	}
	else{
		if(point.x > yScaleWidth){
			show = true;
		}
	}
	if(point.y < height && show){
			
		newRect.left = point.x;
		newRect.right = point.x;
		newRect.bottom = height;
		newRect.top = 0;

		// Erase vertical line
		if (oldVRect.left != -1 && oldVRect.right != -1 &&   // FROEDE_MARK
			oldVRect.top != -1 && oldVRect.bottom != -1) {   //
		pDC->MoveTo(oldVRect.left,oldVRect.top);
		pDC->LineTo(oldVRect.right,oldVRect.bottom);
		}

		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);
		oldVRect = newRect;	

	}

	pDC->SetROP2(R2_COPYPEN);		
	ReleaseScreen(pDC);	

}

void CStockChartXCtrl::SuspendCrossHairs()
{
	if(m_crossHairs)
	{
	  if(drawing) return;

	  CDC* pDC = GetScreen();
	  pDC->SetROP2(R2_NOT);

	  // Erase horizontal line
	  if (oldHRect.left != -1 && oldHRect.right != -1 &&  // FROEDE_MARK
			oldHRect.top != -1 && oldHRect.bottom != -1) {  //
		  pDC->MoveTo(oldHRect.left,oldHRect.top);
		  pDC->LineTo(oldHRect.right,oldHRect.bottom);
	  }

	  // Erase vertical line
	  if (oldVRect.left != -1 && oldVRect.right != -1 &&   // FROEDE_MARK
			oldVRect.top != -1 && oldVRect.bottom != -1) {   //
		pDC->MoveTo(oldVRect.left,oldVRect.top);
		pDC->LineTo(oldVRect.right,oldVRect.bottom);
	  }

	  oldHRect = CRect(-1,-1,-1,-1);
	  oldVRect = CRect(-1,-1,-1,-1);

	  m_pauseCrossHairs = true;

	  pDC->SetROP2(R2_COPYPEN);		
	  ReleaseScreen(pDC);
	}

}

void CStockChartXCtrl::ResumeCrossHairs(CPoint point)
{	
	if(DeltaCursor && m_valueView->visible)return;
	// Just show the cross hairs and exit if m_crossHairs = true
	if(m_crossHairs)
	{
		m_pauseCrossHairs = false;

		DrawCrossHairs(point);
		int n = 0;
		for(n = 0; n != panels.size(); ++n)
		{
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				m_currentPanel = n; // cache panel mouse was over last 1/8/08			
			}
		}
		// return; FROEDE_MARK 
	}
}


void CStockChartXCtrl::DrawMeasure(CPoint point)
{
	if(drawing || m_pauseMeasure) return;

	CDC* pDC = GetScreen();
	pDC->SetROP2(R2_NOT);

	if(point.x>m_valueView->x2Value){
		newRectM.left = m_valueView->x2Value;
		newRectM.top = m_valueView->y2Value;
		newRectM.right = (double)point.x;
		newRectM.bottom = (double)point.y;
	}
	else{
		newRectM.left = (double)point.x;
		newRectM.top = (double)point.y;
		newRectM.right = m_valueView->x2Value;
		newRectM.bottom = m_valueView->y2Value;
	}
	if(newRectM.right > panels[0]->yScaleRect.left)newRectM.right = panels[0]->yScaleRect.left-1;
	if((double)newRectM.bottom >(double)panels[0]->y2-10.0F)newRectM.bottom = panels[0]->y2-10.0F;
	/*
	newRectM.left = 0.0f;
	newRectM.top = (double)point.y;
	newRectM.right = 1000.0f;
	newRectM.bottom = (double)point.y;*/

	// Erase old line
	if (oldRectM.left != NULL_VALUE && oldRectM.right != NULL_VALUE &&  
		oldRectM.top != NULL_VALUE && oldRectM.bottom != NULL_VALUE) { 
		pDC->MoveTo(oldRectM.left,oldRectM.top);
		pDC->LineTo(oldRectM.right,oldRectM.bottom);
	}

	pDC->MoveTo(newRectM.left,newRectM.top);
	pDC->LineTo(newRectM.right,newRectM.bottom);
	oldRectM = newRectM;




	pDC->SetROP2(R2_COPYPEN);		
	ReleaseScreen(pDC);	

}

void CStockChartXCtrl::SuspendMeasure()
{
	if(DeltaCursor)
	{
	  if(drawing) return;

	  CDC* pDC = GetScreen();
	  pDC->SetROP2(R2_NOT);

	  // Erase old line
	  if (oldRectM.left != NULL_VALUE && oldRectM.right != NULL_VALUE &&  
			oldRectM.top != NULL_VALUE && oldRectM.bottom != NULL_VALUE) {  
		  pDC->MoveTo(oldRectM.left,oldRectM.top);
		  pDC->LineTo(oldRectM.right,oldRectM.bottom);
	  }
	  
	  oldRectM = CRectF(NULL_VALUE,NULL_VALUE,NULL_VALUE,NULL_VALUE);

	  m_pauseMeasure = true;

	  pDC->SetROP2(R2_COPYPEN);		
	  ReleaseScreen(pDC);
	}

}

void CStockChartXCtrl::ResumeMeasure(CPoint point)
{	
	// Just show the cross hairs and exit if m_crossHairs = true
	if(DeltaCursor && m_valueView->visible)
	{
		m_pauseMeasure = false;
		if(m_valueView->x2Value == NULL_VALUE || m_valueView->y2Value == NULL_VALUE) return;
		int p1, p2;
		p1 = p2 = -1;
		//	Which panel is selected
		for (int n = 0; n != panels.size(); ++n)
		{

			int	py1 = (int)panels[n]->y1;
			int	py2 = (int)panels[n]->y2;
			int	y1 = point.y;
			int	y2 = m_valueView->y2Value;

			if (y1 > py1 && y1 < py2)
			{
				p1 = n;
			}
			if (y2 > py1 && y2 < py2)
			{
				p2 = n;
			}
		}
		if (p1 != p2)return;
		DrawMeasure(point);
		int n = 0;
		for(n = 0; n != panels.size(); ++n)
		{
			if(point.y > panels[n]->y1 && 
				point.y < panels[n]->y2){
				m_currentPanel = n; // cache panel mouse was over last 1/8/08			
			}
		}
	}
}

void CStockChartXCtrl::OnIDChanged() 
{
	// TODO: Add notification handler code

	SetModifiedFlag();
}

BOOL CStockChartXCtrl::GetDarvasBoxes() 
{
	if(darvasBoxes)
		return TRUE;
	else
		return FALSE;
}

void CStockChartXCtrl::SetDarvasBoxes(BOOL bNewValue) 
{
	if(bNewValue)
		darvasBoxes = true;
	else
		darvasBoxes = false;
	
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

double CStockChartXCtrl::GetDarvasStopPercent() 
{
	return darvasPct;
}

void CStockChartXCtrl::SetDarvasStopPercent(double newValue) 
{
	darvasPct = newValue;
	UpdateScreen(true);
	changed = true;	
	SetModifiedFlag();
}

// 12/27/08 added Panel
long CStockChartXCtrl::GetYPixel(long Panel, double Value) 
{
	long ret = -1;
	if(Panel > -1 && Panel < (long)panels.size())
	{
		if(panels[Panel]->visible)
		{
			ret = (long)panels[Panel]->GetY(Value);
		}	
	}
	return ret;
}

long CStockChartXCtrl::GetX(long record)
{
	return GetXPixel(record);
}

long CStockChartXCtrl::GetXPixel(long Record) 
{
	long ret = -1;
	if(panels[0]->visible){
//	Revision 6/10/2004
//	Addition of type cast of long
		ret = (long)panels[0]->GetX(Record);
//	End Of Revision
	}
	return ret;
}

double CStockChartXCtrl::GetYValueByPixel(long Pixel) 
{	
	int p = -1;
	
	//	Which panel is the mouse currently over?
	for( int n = 0; n != panels.size(); ++n)
	{
//	Revision 6/10/2004
//	Addition type cast int
		int	py1	= (int)panels[n]->y1;
		int	py2	= (int)panels[n]->y2;
//	End Of Revision
		if( Pixel > py1 && Pixel < py2 )
		{
			p = n;
			break;
		}
	}

	int bp = GetBottomChartPanel();
	if(bp != -1){
		if(p == -1 && m_point.y > panels[bp]->y1){
			p = GetBottomChartPanel();
		}
	}

	if(p != -1){
		// n has been replaced with p here
		// 9/24/04 RG - thanks to Mr. Ravichandran
		bool error=true;
		for(int i=0;i<panels[p]->series.size();i++) if(!panels[p]->series[i]->dataError)error=false;
		if(!error)return panels[p]->GetReverseY(Pixel);	
	}


	return NULL_VALUE;
}

double CStockChartXCtrl::GetXRecordByPixel(long Pixel)
{	
	return (panels[0]->GetReverseX(Pixel)+startIndex+1);
	
}
/*
CPoint CStockChartXCtrl::GetPoint(long panel,CPointF pointValue) 
{
	CPoint ret;
	//if(panels[0]->visible){
		ret.x = (long)GetXPixel(pointValue.x);
		ret.y = (long)GetYPixel(panel,pointValue.y);
	//}

	return ret;
}

CPointF CStockChartXCtrl::GetPointValue(CPoint point) 
{
	CPointF ret;
	//if(panels[0]->visible){
		ret.x = (long)GetXRecordByPixel(point.x);
		ret.y = (long)GetYValueByPixel(point.y);
	//}
	return ret;
}*/

OLE_COLOR CStockChartXCtrl::GetBarColor(long Record, LPCTSTR Name) 
{
	OLE_COLOR retVal = 0;
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Record < (long)barColors.size() + 1 && Record > 0){ // 11/18/2007 added + 1
//	End Of Revision
		retVal = barColors[Record - 1];
	}
	return retVal;
}

void CStockChartXCtrl::SetBarColor(long Record, LPCTSTR Name, OLE_COLOR nNewValue) 
{
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Record < (long)barColors.size() + 1 && Record > 0){ // 11/18/2007 added + 1
//	End Of Revision
		barColors[Record - 1] = nNewValue;
	}
	barColorName = Name; // Only one series can "own" the bar colors array
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnHorizontalSeparatorsChanged() 
{
	m_horzLines = m_horizontalSeparators == TRUE;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnHorizontalSeparatorColorChanged() 
{
	horzLineColor = m_horizontalSeparatorColor;
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnShowRecordsForXLabelsChanged() 
{
	if(m_showRecordsForXLabels == TRUE){
		recordLabels = true;
	}
	else{
		recordLabels = false;
	}
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnDisplayTitlesChanged() 
{
	if(m_displayTitles == TRUE){
		displayTitles = true;
	}
	else{
		displayTitles = false;
	}	
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnDisplayTitleBorderChanged() 
{
	if(m_displayTitleBorder == TRUE){
		yOffset = 15;
	}
	else{
		yOffset = 0;
	}
	UpdateScreen(true);
	SetModifiedFlag();
}

long CStockChartXCtrl::GetBarWidth() 
{
	return barWidth;
}

void CStockChartXCtrl::SetBarWidth(long nNewValue) 
{
	if(nNewValue > 0 && nNewValue < 50) barWidth = nNewValue;
	UpdateScreen(true);
	SetModifiedFlag();
}



BOOL CStockChartXCtrl::AddIndicatorSeries(long IndicatorType, LPCTSTR Key, long Panel, BOOL UserParams) 
{
	
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

#ifdef _CONSOLE_DEBUG
	//printf("\nAddIndicator(%s)",Key);
#endif

	bool valid = false;
	bool userParams = false;
	if(UserParams == TRUE) userParams = true;	
	
	switch(IndicatorType){
		
		case indNegativeVolumeIndex:	
			panels[Panel]->series.push_back(new CIndicatorNegativeVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case indBollingerBands:
			panels[Panel]->series.push_back(new CIndicatorBollingerBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;			
			break;

		case indFractalChaosBands:
			panels[Panel]->series.push_back(new CIndicatorFractalChaosBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;			
			break;
			
		case indFractalChaosOscillator:
			panels[Panel]->series.push_back(new CIndicatorFractalChaosOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;			
			break;
	
		case indHighLowBands:
			panels[Panel]->series.push_back(new CIndicatorHighLowBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMovingAverageEnvelope:
			panels[Panel]->series.push_back(new CIndicatorMovingAverageEnvelope(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indHighMinusLow:
			panels[Panel]->series.push_back(new CIndicatorHighMinusLow(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMedian:
			panels[Panel]->series.push_back(new CIndicatorMedian(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPriceROC:
			panels[Panel]->series.push_back(new CIndicatorPriceROC(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indStandardDeviation:
			panels[Panel]->series.push_back(new CIndicatorStandardDeviation(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTypicalPrice:
			panels[Panel]->series.push_back(new CIndicatorTypicalPrice(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVolumeROC:
			panels[Panel]->series.push_back(new CIndicatorVolumeROC(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVolume:
			panels[Panel]->series.push_back(new CIndicatorVolume(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indWeightedClose:
			panels[Panel]->series.push_back(new CIndicatorWeightedClose(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indAccumulativeSwingIndex:
			panels[Panel]->series.push_back(new CIndicatorAccumulativeSwingIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indChaikinMoneyFlow:
			panels[Panel]->series.push_back(new CIndicatorChaikinMoneyFlow(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indAccumulationDistribution:
			panels[Panel]->series.push_back(new CIndicatorAccumulationDistribution(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
		break;

		case  indCommodityChannelIndex:
			panels[Panel]->series.push_back(new CIndicatorCommodityChannelIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indComparativeRelativeStrength:
			panels[Panel]->series.push_back(new CIndicatorComparativeRelativeStrength(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMassIndex:
			panels[Panel]->series.push_back(new CIndicatorMassIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMoneyFlowIndex:
			panels[Panel]->series.push_back(new CIndicatorMoneyFlowIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indOnBalanceVolume:
			panels[Panel]->series.push_back(new CIndicatorOnBalanceVolume(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPerformanceIndex:
			panels[Panel]->series.push_back(new CIndicatorPerformanceIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPositiveVolumeIndex:
			panels[Panel]->series.push_back(new CIndicatorPositiveVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPriceVolumeTrend:
			panels[Panel]->series.push_back(new CIndicatorPriceVolumeTrend(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indRelativeStrengthIndex:
			panels[Panel]->series.push_back(new CIndicatorRelativeStrengthIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indSwingIndex:
			panels[Panel]->series.push_back(new CIndicatorSwingIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTradeVolumeIndex:
			panels[Panel]->series.push_back(new CIndicatorTradeVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indLinearRegressionRSquared:
			panels[Panel]->series.push_back(new CIndicatorLinearRegressionRSquared(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indLinearRegressionForecast:
			panels[Panel]->series.push_back(new CIndicatorLinearRegressionForecast(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indLinearRegressionSlope:
			panels[Panel]->series.push_back(new CIndicatorLinearRegressionSlope(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indLinearRegressionIntercept:
			panels[Panel]->series.push_back(new CIndicatorLinearRegressionIntercept(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indAroon:
			panels[Panel]->series.push_back(new CIndicatorAroon(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indAroonOscillator:
			panels[Panel]->series.push_back(new CIndicatorAroonOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indChaikinVolatility:
			panels[Panel]->series.push_back(new CIndicatorChaikinVolatility(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;
			
		case  indChandeMomentumOscillator:
			panels[Panel]->series.push_back(new CIndicatorChandeMomentumOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indDetrendedPriceOscillator:
			panels[Panel]->series.push_back(new CIndicatorDetrendedPriceOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indDirectionalMovementSystem:
			panels[Panel]->series.push_back(new CIndicatorDirectionalMovementSystem(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indEaseOfMovement:
			panels[Panel]->series.push_back(new CIndicatorEaseOfMovement(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMACD:
			panels[Panel]->series.push_back(new CIndicatorMACD(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMACDHistogram:
			panels[Panel]->series.push_back(new CIndicatorMACDHistogram(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indMomentumOscillator:
			panels[Panel]->series.push_back(new CIndicatorMomentum(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indParabolicSAR:
			panels[Panel]->series.push_back(new CIndicatorParabolicSAR(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPriceOscillator:
			panels[Panel]->series.push_back(new CIndicatorPriceOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indRainbowOscillator:
			panels[Panel]->series.push_back(new CIndicatorRainbowOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indStochasticOscillator:
			panels[Panel]->series.push_back(new CIndicatorStochasticOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indStochasticMomentumIndex:
			panels[Panel]->series.push_back(new CIndicatorStochasticMomentumIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTRIX:
			panels[Panel]->series.push_back(new CIndicatorTRIX(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTrueRange:
			panels[Panel]->series.push_back(new CIndicatorTrueRange(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indUltimateOscillator:
			panels[Panel]->series.push_back(new CIndicatorUltimateOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVerticalHorizontalFilter:
			panels[Panel]->series.push_back(new CIndicatorVerticalHorizontalFilter(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVolumeOscillator:
			panels[Panel]->series.push_back(new CIndicatorVolumeOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indWilliamsAccumulationDistribution:
			panels[Panel]->series.push_back(new CIndicatorWilliamsAccumulationDistribution(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indWilliamsPctR:
			panels[Panel]->series.push_back(new CIndicatorWilliamsPctR(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indExponentialMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorExponentialMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indSimpleMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorSimpleMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indGenericMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorGenericMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTimeSeriesMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorTimeSeriesMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indTriangularMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorTriangularMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVariableMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorVariableMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indWeightedMovingAverage:
			panels[Panel]->series.push_back(new CIndicatorWeightedMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indVIDYA:
			panels[Panel]->series.push_back(new CIndicatorVIDYA(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indWellesWilderSmoothing:
			panels[Panel]->series.push_back(new CIndicatorWellesWilderSmoothing(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPrimeNumberOscillator:
			panels[Panel]->series.push_back(new CIndicatorPrimeNumberOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indPrimeNumberBands:
			panels[Panel]->series.push_back(new CIndicatorPrimeNumberBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;


		case  indHistoricalVolatility:
			panels[Panel]->series.push_back(new CIndicatorHistoricalVolatility(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;
			
		case  indHILOActivator:
			panels[Panel]->series.push_back(new CIndicatorHILOActivator(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indADX:
			panels[Panel]->series.push_back(new CIndicatorADX(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;

		case  indDI:
			panels[Panel]->series.push_back(new CIndicatorDI(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;


		// Custom indicator - Eugen
		case indCustomIndicator:
			panels[Panel]->series.push_back(new CIndicatorCustom(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
			valid = true;
			break;
	

			///////Insert new indicator series types here///////

	}

	if(valid){
		panels[Panel]->series[panels[Panel]->series.size() - 1]->Connect(this);
		panels[Panel]->series[panels[Panel]->series.size() - 1]->userParams = userParams;
		reCalc = true;
		changed = true;
		return TRUE;
	}
	else{
		ThrowErr(1225, "Invalid technical indicator");
		return FALSE;
	}


}


// TA-SDK Indicator Parameters

BSTR CStockChartXCtrl::GetIndPropStr(LPCTSTR Key, short ParamNum) 
{
	CString strResult = "";
	if(ParamNum < 0) return strResult.AllocSysString();

	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	Addition of type cast of unsigned long
					if(panels[n]->series[j]->paramStr.size() >= (unsigned long)ParamNum){
//	End Of Revision
						strResult = panels[n]->series[j]->paramStr[ParamNum - 1];
						break;
					}
				}
			}
		}
	}

	return strResult.AllocSysString();
}

void CStockChartXCtrl::SetIndPropStr(LPCTSTR Key, short ParamNum, LPCTSTR lpszNewValue) 
{

	if(ParamNum < 0) return;
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	Addition of type cast of long
					if(panels[n]->series[j]->paramStr.size() >= (unsigned long)ParamNum){
//	End Of Revision
						panels[n]->series[j]->paramStr[ParamNum - 1] = lpszNewValue;
						reCalc = true;
						break;
					}
				}
			}
		}
	}

	changed = true;
	SetModifiedFlag();
}

double CStockChartXCtrl::GetIndPropDbl(LPCTSTR Key, short ParamNum) 
{
	double ret = 0;

	if(ParamNum < 0) return ret;
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	Addition of type cast of unsigned long
					if(panels[n]->series[j]->paramDbl.size() >= (unsigned long)ParamNum){
//	End Of Revision
						ret = panels[n]->series[j]->paramDbl[ParamNum - 1];
						break;
					}
				}
			}
		}
	}

	return ret;
}

void CStockChartXCtrl::SetIndPropDbl(LPCTSTR Key, short ParamNum, double newValue) 
{

	if(ParamNum < 0) return;
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	Addition of type cast of unsigned long
					if(panels[n]->series[j]->paramDbl.size() >= (unsigned long)ParamNum){
//	End Of Revision
						panels[n]->series[j]->paramDbl[ParamNum - 1] = newValue;
						reCalc = true;
						break;
					}
				}
			}
		}
	}

	changed = true;
	SetModifiedFlag();
}

short CStockChartXCtrl::GetIndPropInt(LPCTSTR Key, short ParamNum) 
{

	short ret = 0;
	if(ParamNum < 0) return ret;

	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	Addition of type cast of unsigned long
					if(panels[n]->series[j]->paramInt.size() >= (unsigned long)ParamNum){
//	End Of Revision
						ret = panels[n]->series[j]->paramInt[ParamNum - 1];
						break;
					}
				}
			}
		}
	}

	return ret;
}

void CStockChartXCtrl::SetIndPropInt(LPCTSTR Key, short ParamNum, short nNewValue) 
{

	if(ParamNum < 0) return;
	for(int n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(panels[n]->series[j]->seriesType == OBJECT_SERIES_INDICATOR){
				if(CompareNoCase(Key, panels[n]->series[j]->szName)){
//	Revision 6/10/2004
//	type cast of int
					if(panels[n]->series[j]->paramInt.size() >= (unsigned long)ParamNum){
//	End Of Revision
						panels[n]->series[j]->paramInt[ParamNum - 1] = nNewValue;
						reCalc = true;
						break;
					}
				}
			}
		}
	}

	changed = true;
	SetModifiedFlag();
}

void CStockChartXCtrl::ThrowErr(long number, LPCTSTR description)
{
	ThrowError(CUSTOM_CTL_SCODE(number), description);
}

void CStockChartXCtrl::ShowHtmlHelp(CString chmFileName, long helpTopic)
{
	// Display a help topic

	CString helpFile = "";

	if(chmFileName.Find(":\\") == -1){

		TCHAR buffer[1024];
		DWORD bytes = GetModuleFileName(NULL,buffer,sizeof(buffer) - 1);
		if(bytes == 0)
			return;

		TCHAR* ptr = buffer + bytes;

		while(ptr >= buffer && *ptr != _TCHAR('\\'))
			ptr--;
			*ptr = _TCHAR('\0');

		helpFile = buffer;	 

		helpFile += "\\" + chmFileName;

	}
	else{
		helpFile = chmFileName;
	}

//	Revision to erase error	6/10/2004 By Katchei
//	This will probably need to be completely rewritten
//	Addition of the :: to denote an SDK function
	::HtmlHelp(AfxGetMainWnd()->m_hWnd,helpFile,HH_HELP_CONTEXT,helpTopic);
//	End Of Revision
}

long CStockChartXCtrl::AddNewSeries(LPCTSTR Name, long Type, long Panel)
{
	return AddSeries(Name, Type, Panel);
}

void CStockChartXCtrl::AppendNewValue(LPCTSTR Name, double JDate, double Value)
{
	AppendValue(Name, JDate, Value);
}

void CStockChartXCtrl::ShowHelp(LPCTSTR chmFileName, long helpTopic) 
{
	ShowHtmlHelp(chmFileName, helpTopic);
}

/*
BOOL CStockChartXCtrl::ShowHelp(LPCTSTR Key, long Panel, LPCTSTR Source, long Periods, int ColorR, int ColorG, int ColorB, int Style, int Thickness) 
{
	
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
//	if(UserParams == TRUE) userParams = true;
	panels[Panel]->series.push_back(new CIndicatorSimpleMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[Panel]));
	panels[Panel]->series[panels[Panel]->series.size() - 1]->Connect(this);
	panels[Panel]->series[panels[Panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[Panel]->series[panels[Panel]->series.size() - 1]->paramStr[0] = Source;
	panels[Panel]->series[panels[Panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[Panel]->series[panels[Panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[Panel]->series[panels[Panel]->series.size() - 1]->lineStyle = Style;
	panels[Panel]->series[panels[Panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;


}
*/
void CStockChartXCtrl::ShowIndicatorDialog(LPCTSTR Key) 
{		
	ShowIndDlg(Key);
}

void CStockChartXCtrl::ShowIndDlg(LPCTSTR key)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(key, panels[n]->series[j]->szName)){
					panels[n]->series[j]->userParams = true;					
					panels[n]->series[j]->showDialog = true;					
					reCalc = true;
					UpdateScreen(false);
					return;
				}
			}
		}
	}	
	
}

void CStockChartXCtrl::SendSeriesMessage(UINT msg, LPCSTR series)
{
	CSeries* pSeries = NULL;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(series, panels[n]->series[j]->szName)){
					pSeries = panels[n]->series[j];
					goto send;
				}
			}
		}
	}
	
send:
	switch(msg){

	case DOUBLE_CLICK:
		if(NULL != pSeries){
			pSeries->selected = true;
			pSeries->OnDoubleClick(m_clickPoint);
		}
		break;

	}
}

void CStockChartXCtrl::InvalidateIndicators()
{
	// Read all indicators for calculation
	int count1 = panels.size();	
	for(int panel = 0; panel != count1; ++panel){
		int count2 = panels[panel]->series.size();
		for(int series = 0; series != count2; ++series){
			panels[panel]->series[series]->calculated = false;
		}
	}

}

void CStockChartXCtrl::AddHorizontalLine( long Panel, double Value )
{
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size())
//	End Of Revision
	{
		if(panels[Panel]->visible)
		{
			panels[Panel]->AddHorzLine(Value);
		}
	}
}

void CStockChartXCtrl::RemoveHorizontalLine(long Panel, double Value) 
{
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size())
//	End Of Revision
	{
		if(panels[Panel]->visible)
		{
			panels[Panel]->RemoveHorzLine(Value);
		}
	}
}

void CStockChartXCtrl::OnThreeDStyleChanged() 
{
	threeDStyle = (m_threeDStyle == TRUE);
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

double CStockChartXCtrl::GetPriceStyleParam(long Index) 
{
	Index--;
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Index > -1 && Index < (long)priceStyleParams.size())
//	End Of Revision
		return priceStyleParams[Index];
	else
		return 0.0;
}

void CStockChartXCtrl::SetPriceStyleParam(long Index, double newValue) 
{
	Index--;

//	Revision 6/10/2004
//	Addition of type cast of long
	if(Index > -1 && Index < (long)priceStyleParams.size())
//	End Of Revision
		priceStyleParams[Index] = newValue;
	
	changed = true;
	UpdateScreen(true);
}

// Enumerate a combo box with indicators
/*void CStockChartXCtrl::EnumIndicators(long hWnd) 
{
	CComboBox* combo = new CComboBox();
	try{
		combo->m_hWnd = (HWND)hWnd;
	}
	catch(...){return;}
	if(NULL == combo) return;

	for(int n = 0; n!= indicatorMap.size(); ++n){
		combo->AddString(indicatorMap[n]);
	}

	combo->m_hWnd = NULL;
	delete combo;

}

// Enumerate a combo box with price styles
void CStockChartXCtrl::EnumPriceStyles(long hWnd) 
{
	CComboBox* combo = new CComboBox();
	try{
		combo->m_hWnd = (HWND)hWnd;
	}
	catch(...){return;}
	if(NULL == combo) return;

	for(int n = 0; n!= priceStyleMap.size(); ++n){
		combo->AddString(priceStyleMap[n]);
	}

	combo->m_hWnd = NULL;
	delete combo;

}
*/

// Enumerate indicators
void CStockChartXCtrl::EnumIndicators() 
{

	for(int n = 0; n!= indicatorMap.size(); ++n){
		FireEnumIndicator(indicatorMap[n], n);
	}

}

// Enumerate price styles
void CStockChartXCtrl::EnumPriceStyles() 
{

	for(int n = 0; n!= priceStyleMap.size(); ++n){		
		FireEnumPriceStyle(priceStyleMap[n], n);
	}

}
void CStockChartXCtrl::EnumIndicatorMap()
{
	indicatorMap.push_back("Simple Moving Average");
	indicatorMap.push_back("Exponential Moving Average");
	indicatorMap.push_back("Time Series Moving Average");
	indicatorMap.push_back("Triangular Moving Average");
	indicatorMap.push_back("Variable Moving Average");
	indicatorMap.push_back("VIDYA Moving Average");
	indicatorMap.push_back("Welles Wilder Smoothing");
	indicatorMap.push_back("Weighted Moving Average");
	indicatorMap.push_back("Williams %R");
	indicatorMap.push_back("Williams Accumulation Distribution");
	indicatorMap.push_back("Volume Oscillator");
	indicatorMap.push_back("Vertical Horizontal Filter");
	indicatorMap.push_back("Ultimate Oscillator");
	indicatorMap.push_back("True Range");
	indicatorMap.push_back("TRIX");
	indicatorMap.push_back("Rainbow Oscillator");
	indicatorMap.push_back("Price Oscillator");
	indicatorMap.push_back("Parabolic SAR");
	indicatorMap.push_back("Momentum Oscillator");
	indicatorMap.push_back("MACD");	
	indicatorMap.push_back("Ease Of Movement");
	indicatorMap.push_back("Directional Movement System");
	indicatorMap.push_back("Detrended Price Oscillator");
	indicatorMap.push_back("Chande Momentum Oscillator");
	indicatorMap.push_back("Chaikin Volatility");
	indicatorMap.push_back("Aroon");
	indicatorMap.push_back("Aroon Oscillator");
	indicatorMap.push_back("Linear Regression R-Squared");
	indicatorMap.push_back("Linear Regression Forecast");
	indicatorMap.push_back("Linear Regression Slope");
	indicatorMap.push_back("Linear Regression Intercept");
	indicatorMap.push_back("Price Volume Trend");
	indicatorMap.push_back("Performance Index");
	indicatorMap.push_back("Commodity Channel Index");
	indicatorMap.push_back("Chaikin Money Flow");
	indicatorMap.push_back("Weighted Close");
	indicatorMap.push_back("Volume ROC");
	indicatorMap.push_back("Typical Price");
	indicatorMap.push_back("Standard Deviation");
	indicatorMap.push_back("Price ROC");
	indicatorMap.push_back("Median Price");
	indicatorMap.push_back("High Minus Low");
	indicatorMap.push_back("Bollinger Bands");
	indicatorMap.push_back("Fractal Chaos Bands");
	indicatorMap.push_back("High/Low Bands");
	indicatorMap.push_back("Moving Average Envelope");
	indicatorMap.push_back("Swing Index");
	indicatorMap.push_back("Accumulative Swing Index");
	indicatorMap.push_back("Comparative RSI");
	indicatorMap.push_back("Mass Index");
	indicatorMap.push_back("Money Flow Index");
	indicatorMap.push_back("Negative Volume Index");
	indicatorMap.push_back("On Balance Volume");
	indicatorMap.push_back("Positive Volume Index");
	indicatorMap.push_back("Relative Strength Index");
	indicatorMap.push_back("Trade Volume Index");
	indicatorMap.push_back("Stochastic Oscillator");
	indicatorMap.push_back("Stochastic Momentum Index");	
	indicatorMap.push_back("Fractal Chaos Oscillator");
	indicatorMap.push_back("Prime Number Oscillator");
	indicatorMap.push_back("Prime Number Bands");
	indicatorMap.push_back("Historical Volatility");
	indicatorMap.push_back("MACD Histogram");

	priceStyleMap.push_back("Standard");
	priceStyleMap.push_back("Point & Figure");
	priceStyleMap.push_back("Renko");
	priceStyleMap.push_back("Kagi");
	priceStyleMap.push_back("Three Line Break");
	priceStyleMap.push_back("Equivolume");
	priceStyleMap.push_back("Equivolume Shadow");
	priceStyleMap.push_back("Candle Volume");
	priceStyleMap.push_back("Heikin Ashi");
	priceStyleMap.push_back("Heikin Ashi Smooth");

}


void CStockChartXCtrl::SetYScale(long Panel, double Max, double Min) 
{
	bool changed = false;
	if (Max <= Min  ) return;
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			//if(Min <= 0) Min = 0.0001; // Removed 5/04/2007
			panels[Panel]->max = Max;
			panels[Panel]->min = Min;
			//panels[Panel]->slmax = Max / 10 * pow((double)10,(double)(Max / panels[Panel]->max));
			//panels[Panel]->slmin = Min / 10 * pow((double)10,(double)(Min / panels[Panel]->min));
			
			panels[Panel]->slmax = log10(Max);
			panels[Panel]->slmin = log10(Min);
			panels[Panel]->staticScale = true;
			UpdateScreen(true);
			return;
		}
	}
}

void CStockChartXCtrl::ChangeYScale(long Panel, double Max, double Min){
	SetYScale(Panel, Max, Min);
}

void CStockChartXCtrl::ResetYScale(long Panel) 
{
//	Revision 6/10/2004
//	Addition of type cast of long
	if(Panel > -1 && Panel < (long)panels.size()){
//	End Of Revision
		if(panels[Panel]->visible){
			panels[Panel]->staticScale = false;
		}
	}
	changed = true;
	UpdateScreen(true);
	SetModifiedFlag();
}

DOUBLE CStockChartXCtrl::GetYScaleMax(LONG panel)
{
	if(panel > -1 && panel < (long)panels.size()){
		if(panels[panel]->visible){
			if(scalingType==LINEAR)	return panels[panel]->max;
			else if(scalingType==SEMILOG) return panels[panel]->slmax;
		}
	}
	return NULL_VALUE;
}


DOUBLE CStockChartXCtrl::GetYScaleMin(LONG panel)
{
	if(panel > -1 && panel < (long)panels.size()){
		if(panels[panel]->visible){
			if(scalingType==LINEAR)	return panels[panel]->min;
			else if(scalingType==SEMILOG) return panels[panel]->slmin;
		}
	}
	return NULL_VALUE;
}

void CStockChartXCtrl::OnPriceStyleChanged() 
{
	
	priceStyle = m_priceStyle;

	if(priceStyle == psStandard){
		dateMap.clear();
		dateMap.resize(0);
		xMap.clear();
		xMap.resize(0);
		xCount = 0;
	}

	changed = true;
	UpdateScreen(true);

	SetModifiedFlag();
}




// Fade (gradient) from top to bottom
void CStockChartXCtrl::FadeVert(CDC* pDC, COLORREF Color1, COLORREF Color2, CRect boxRect)
{
	//pdcHandler->FillRectangleGradient(CRectF(boxRect),Color1,Color2,pDC);
	//return;

 HPEN hPen, oldPen;
 HBRUSH hBrush, oldBrush; 
 double nLCV, nTot;
 double r1, r2, rCur, g1, g2, gCur, b1, b2, bCur, c1, c2, rInc, gInc, bInc;
 
 double nX1, nY1, nX2, nY2;
 nX1 = boxRect.left;
 nX2 = boxRect.right;
 nY1 = boxRect.top;
 nY2 = boxRect.bottom;

 c1 = (double) Color1;
 c2 = (double) Color2;

 b1 = Div(c1, 65536);
 c1 = Mod(c1, 65536);
 g1 = Div(c1, 256);
 c1 = Mod(c1, 256);
 r1 = (int) c1;

 b2 = Div(c2, 65536);
 c2 = Mod(c2, 65536);
 g2 = Div(c2, 256);
 c2 = Mod(c2, 256);
 r2 = (int) c2;

 nTot = nY2 - nY1 + 1;

 rInc = (r2 - r1) / (nTot - 1);
 gInc = (g2 - g1) / (nTot - 1);
 bInc = (b2 - b1) / (nTot - 1);

 rCur = r1;
 gCur = g1;
 bCur = b1;


 for (nLCV = nY1; nLCV <= nY2; nLCV++)
 { 
  if ((nLCV <= nY2) && (nLCV >= 0))
  {
   hPen = CreatePen(PS_SOLID, 0, RGB((int) rCur, (int) gCur, (int) bCur));
   hBrush = CreateSolidBrush(RGB((int) rCur, (int) gCur, (int) bCur));

   oldPen = (HPEN) pDC->SelectObject(hPen);
   oldBrush = (HBRUSH) pDC->SelectObject(hBrush);

   pDC->Rectangle(nX1, nLCV, nX2, nLCV + 1);

	pDC->SelectObject(oldPen);
	pDC->SelectObject(oldBrush);

   DeleteObject(hPen);
   DeleteObject(hBrush);

	  //pdcHandler->FillRectangle(CRectF(nX1, nLCV, nX2, nLCV + 1),RGB((int) rCur, (int) gCur, (int) bCur),pDC);

   rCur += rInc;
   gCur += gInc;
   bCur += bInc;
  }
 }

}


// Fade (gradient) from left to right
void CStockChartXCtrl::FadeHorz(CDC* pDC, COLORREF Color1, COLORREF Color2, CRect boxRect)
{ 
 HPEN hPen, oldPen;
 HBRUSH hBrush, oldBrush; 
 int nLCV, nTot;
 double r1, r2, rCur, g1, g2, gCur, b1, b2, bCur, c1, c2, rInc, gInc, bInc;
 
 int nX1, nY1, nX2, nY2;
 nX1 = boxRect.left;
 nX2 = boxRect.right;
 nY1 = boxRect.top;
 nY2 = boxRect.bottom;

 c1 = (double) Color1;
 c2 = (double) Color2;

 b1 = Div(c1, 65536);
 c1 = Mod(c1, 65536);
 g1 = Div(c1, 256);
 c1 = Mod(c1, 256);
 r1 = (int) c1;

 b2 = Div(c2, 65536);
 c2 = Mod(c2, 65536);
 g2 = Div(c2, 256);
 c2 = Mod(c2, 256);
 r2 = (int) c2;

 nTot = nX2 - nX1 + 1;

 rInc = (r2 - r1) / (nTot - 1);
 gInc = (g2 - g1) / (nTot - 1);
 bInc = (b2 - b1) / (nTot - 1);

 rCur = r1;
 gCur = g1;
 bCur = b1;

 for (nLCV = nX1; nLCV <= nX2; nLCV++)
 { 
  if ((nLCV <= nX2) && (nLCV >= 0))
  {
   hPen = CreatePen(PS_SOLID, 0, RGB((int) rCur, (int) gCur, (int) bCur));
   hBrush = CreateSolidBrush(RGB((int) rCur, (int) gCur, (int) bCur));
   
   oldPen = (HPEN) pDC->SelectObject(hPen);
   oldBrush = (HBRUSH) pDC->SelectObject(hBrush);

   pDC->Rectangle(nLCV, nY1, nLCV + 1, nY2);

	pDC->SelectObject(oldPen);
	pDC->SelectObject(oldBrush);
   
   DeleteObject(hPen);
   DeleteObject(hBrush);

   rCur += rInc;
   gCur += gInc;
   bCur += bInc;
  }
 }

}



// Get the integer division of two numbers
int CStockChartXCtrl::Div(double dNum1, double dNum2)
{
	return (int) (dNum1 / dNum2);
}


// Get the remainder of an integer division of two numbers
double CStockChartXCtrl::Mod(double dNum1, double dNum2)
{
	return (double) (dNum1 - (dNum2 * Div(dNum1, dNum2)));
}

double CStockChartXCtrl::GetMaxValue(LPCTSTR Series) 
{
	return GetMax(Series);
}

double CStockChartXCtrl::GetMinValue(LPCTSTR Series) 
{	
	return GetMin(Series);
}

double CStockChartXCtrl::GetVisibleMaxValue(LPCTSTR Series) 
{
	return GetMax(Series, false, true);
}

double CStockChartXCtrl::GetVisibleMinValue(LPCTSTR Series) 
{
	return GetMin(Series, false, true);
}



double CStockChartXCtrl::GetMax(LPCTSTR Series, bool IgnoreZero /*=false*/, bool OnlyVisible /*=false*/)
{
	double max = 0;
	CString name = "";
	int count1 = panels.size();	
	for(int panel = 0; panel != count1; ++panel){
		int count2 = panels[panel]->series.size();
		for(int series = 0; series != count2; ++series){
			name = panels[panel]->series[series]->szName;
			if(CompareNoCase(Series, name)){
				int count3 = panels[panel]->series[series]->data_slave.size();
					if(count3 > 0) max = panels[panel]->series[series]->data_slave[0].value;
					int start = 0;
					if(OnlyVisible){
						start = startIndex;
						count3 = endIndex;
						if(count3 > 0) max = panels[panel]->series[series]->data_slave[start].value;
					}
					for(int val = start; val != count3; ++val){
						if(IgnoreZero){
							if(panels[panel]->series[series]->data_slave[val].value > max &&
								panels[panel]->series[series]->data_slave[val].value != 0){
								max = panels[panel]->series[series]->data_slave[val].value;
							}
						}							
						else{
							if(panels[panel]->series[series]->data_slave[val].value > max){
								max = panels[panel]->series[series]->data_slave[val].value;
							}
						}						
					}
					return max;
			}
		}
	}
	return NULL_VALUE;
}

double CStockChartXCtrl::GetMin(LPCTSTR Series, bool IgnoreZero /*=false*/, bool OnlyVisible /*=false*/)
{
	double min = 0;
	CString name = "";
	int count1 = panels.size();	
	for(int panel = 0; panel != count1; ++panel){
		int count2 = panels[panel]->series.size();
		for(int series = 0; series != count2; ++series){
			name = panels[panel]->series[series]->szName;
			if(CompareNoCase(Series, name)){
				int count3 = panels[panel]->series[series]->data_slave.size();					
					if(count3 > 0) min = 987654321; // 6/25/08
					int start = 0;
					if(OnlyVisible){
						start = startIndex;
						count3 = endIndex;						
					}
					for(int val = start; val != count3; ++val){
						if(IgnoreZero){
							if(panels[panel]->series[series]->data_slave[val].value < min &&
								panels[panel]->series[series]->data_slave[val].value != 0 && 
								panels[panel]->series[series]->data_slave[val].value != NULL_VALUE){ // 6/25/08
								min = panels[panel]->series[series]->data_slave[val].value;
							}
						}
						else{
							if(panels[panel]->series[series]->data_slave[val].value < min && 
							panels[panel]->series[series]->data_slave[val].value != NULL_VALUE){
								min = panels[panel]->series[series]->data_slave[val].value; // 6/25/08
							}
						}
					}
					return min;
			}
		}
	}
	return NULL_VALUE;
}

// Returns object values
long CStockChartXCtrl::GetObjectStartRecord(long ObjectType, LPCTSTR Key) 
{
	return GetSetChartObjectLocation(ObjectType, Key).x1;
}

long CStockChartXCtrl::GetObjectEndRecord(long ObjectType, LPCTSTR Key) 
{
	return GetSetChartObjectLocation(ObjectType, Key).x2;
}

double CStockChartXCtrl::GetObjectStartValue(long ObjectType, LPCTSTR Key) 
{
	return GetSetChartObjectLocation(ObjectType, Key).y1;
}

double CStockChartXCtrl::GetObjectEndValue(long ObjectType, LPCTSTR Key) 
{
	return GetSetChartObjectLocation(ObjectType, Key).y2;
}


CLocation CStockChartXCtrl::GetSetChartObjectLocation(long ObjectType, LPCTSTR Key)
{
	CLocation ret;	
	ret.y1 = NULL_VALUE, ret.y2 = NULL_VALUE;
	ret.x1 = NULL_VALUE, ret.x2 = NULL_VALUE;
	int n = 0;

	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
//	Revision 6/10/2004 Katchei
//	Addition of type cast of long

					// Update 12/1/04 Katchei please be more careful!
					// y's are doubles not longs - RG

						ret.y1 = (double)panels[n]->textAreas[j]->y1Value;
						ret.y2 = (double)panels[n]->textAreas[j]->y2Value;
						ret.x1 = (long)panels[n]->textAreas[j]->x1Value;
						ret.x2 = (long)panels[n]->textAreas[j]->x2Value;
//	End Of Revision
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
						ret.y1 = (double)panels[n]->objects[j]->y1Value;
						ret.y2 = (double)panels[n]->objects[j]->y2Value;
//	Revision 6/10/2004
//	Addition of type cast of long
						ret.x1 = (long)panels[n]->objects[j]->x1Value;
						ret.x2 = (long)panels[n]->objects[j]->x2Value;
//	End Of Revision
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
//	Revision 6/10/2004
//	Addition of type cast of long
						ret.y1 = (double)panels[n]->lines[j]->y1Value;
						ret.y2 = (double)panels[n]->lines[j]->y2Value;
						ret.x1 = (long)panels[n]->lines[j]->x1Value;
						ret.x2 = (long)panels[n]->lines[j]->x2Value;
//	End Of Revision
						break;					
					}
				}				
			}
		}
	}	
	return ret;
}


// Sets object values
void CStockChartXCtrl::SetObjectPosition(long ObjectType, LPCTSTR Key, long StartRecord, double StartValue, long EndRecord, double EndValue) 
{

	int n = 0;
	switch(ObjectType){
	case OBJECT_TEXT:
	case OBJECT_TEXT_LOCKED:

		// 3/24/05 JG
		StartRecord -= startIndex;
		EndRecord -= startIndex;

		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->textAreas.size(); ++j){
					if(panels[n]->textAreas[j]->key == Key){
//	Revision 6/10/2004
//	Addition of type cast of long
						panels[n]->textAreas[j]->y1 = (int)panels[n]->GetY(StartValue);
						panels[n]->textAreas[j]->y2 = (int)panels[n]->GetY(EndValue);
						panels[n]->textAreas[j]->x1 = (int)panels[n]->GetX(StartRecord);
						panels[n]->textAreas[j]->x2 = (int)panels[n]->GetX(EndRecord);
//	End Of Reservation
						panels[n]->textAreas[j]->Reset();					
						UpdateScreen(true);
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_SYMBOL:
	case BUY_SYMBOL:
	case SELL_SYMBOL:
	case EXIT_SYMBOL:
	case EXIT_LONG_SYMBOL:
	case EXIT_SHORT_SYMBOL:
	case SIGNAL_SYMBOL:
	case CUSTOM_SYMBOL:
		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->objects.size(); ++j){
					if(panels[n]->objects[j]->key == Key){
//	Revision 6/10/2004
//	Addition of type cast of long
						panels[n]->objects[j]->y1 = (int)panels[n]->GetY(StartValue);
						panels[n]->objects[j]->y2 = (int)panels[n]->GetY(EndValue);
						panels[n]->objects[j]->x1 = (int)panels[n]->GetX(StartRecord);
						panels[n]->objects[j]->x2 = (int)panels[n]->GetX(EndRecord);
//	End Of Revision
						panels[n]->objects[j]->y1Value = StartValue;
						panels[n]->objects[j]->y2Value = EndValue;
						panels[n]->objects[j]->x1Value = StartRecord;
						panels[n]->objects[j]->x2Value = EndRecord;						
						panels[n]->objects[j]->UpdatePoints();					
						UpdateScreen(true);
						break;
					}
				}				
			}
		}
		break;
	case OBJECT_LINE_STUDY:
		
		// 3/24/05 JG
		StartRecord -= startIndex;
		EndRecord -= startIndex;

		for(n = 0; n != panels.size(); ++n){		
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->lines.size(); ++j){
					if(panels[n]->lines[j]->key == Key){
//	Revision 6/10/2004
//	Addition of type cast of long
						panels[n]->lines[j]->y1 = (int)panels[n]->GetY(StartValue);
						panels[n]->lines[j]->y2 = (int)panels[n]->GetY(EndValue);
						panels[n]->lines[j]->x1 = (int)panels[n]->GetX(StartRecord);
						panels[n]->lines[j]->x2 = (int)panels[n]->GetX(EndRecord);
//	End Of Revision
						panels[n]->lines[j]->Reset();					
						UpdateScreen(true);
						break;				
					}
				}				
			}
		}
	}	
}




long CStockChartXCtrl::GetRecordByJDate(double JDate) 
{
	double ret = NULL_VALUE;
	double minError = 9.9; //REVISION: Search for closest record
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				for(int k = 0; k != panels[n]->series[j]->data_master.size(); ++k){
					if(panels[n]->series[j]->data_master[k].jdate == JDate){
						return k + 1;
					}
					else if(abs(panels[n]->series[j]->data_master[k].jdate-JDate)<minError){
						ret=k+1;
						minError=abs(panels[n]->series[j]->data_master[k].jdate-JDate);
					}
				}
			}
		}
	}
//	Revision 6/10/2004
//	Addition of type cast to long
	return (long)ret;
//	End Of Revision
}

// Returns current periodicity
long CStockChartXCtrl::GetPeriodicity(void)
{
	return m_Periodicity;
}

// Get a close record in any periodicity
long CStockChartXCtrl::GetRecordByPeriodJDate(double JDate)
{
	long ret = NULL_VALUE;
	/*for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				for(int k = 0; k != panels[n]->series[j]->data_master.size(); ++k){
					if(panels[n]->series[j]->data_master[k].jdate >= JDate){
						return k + 1;
					}
				}
			}
		}
	}*/
	
	// Just look first serie on first panel:
	for(int k = 0; k != panels[0]->series[0]->data_master.size(); ++k){
		if(panels[0]->series[0]->data_master[k].jdate <= JDate){
			ret = k+1;
		}









		else return ret;
	}

	return ret;
}

void CStockChartXCtrl::SetSeriesUpDownColors(LPCTSTR Name, OLE_COLOR UpColor, OLE_COLOR DownColor) 
{	
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->upColor = UpColor;
					panels[n]->series[j]->downColor = DownColor;
					changed = true;
					UpdateScreen(true);
					SetModifiedFlag();
					return;
				}
			}	
		}
	} 
}

void CStockChartXCtrl::OnMaxDisplayRecordsChanged() 
{	
	UpdateScreen(true);
	SetModifiedFlag();
}

BOOL CStockChartXCtrl::GetSeriesVisible(LPCTSTR Name) 
{	
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Name)){
					if(panels[n]->series[j]->seriesVisible){
						return TRUE;
					}
					else{
						return FALSE;
					}
				}	
			}
		}
	}
	return FALSE;
}

void CStockChartXCtrl::SetSeriesVisible(LPCTSTR Name, BOOL bNewValue) 
{
	bool value = (bNewValue == TRUE);
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Name)){
					panels[n]->series[j]->seriesVisible = value;
					UpdateScreen(true);
					return;
				}	
			}
		}
	}
	SetModifiedFlag();
}

// Returns a string name for the series at Index
BSTR CStockChartXCtrl::GetSeriesName(long Index) 
{
	CString strResult;
	int count = 0;
	
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int s = 0; s != panels[n]->series.size(); ++s){
				if(count == Index){
					return CString(panels[n]->series[s]->szName).AllocSysString();
				}
				count++;				
			}
		}
	}

	return strResult.AllocSysString();
}

// Returns the count of a certain type of indicator on the chart
 long CStockChartXCtrl::GetIndicatorCountByType(long IndicatorType) 
{

	long count = 0;

	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int s = 0; s != panels[n]->series.size(); ++s){
				if(panels[n]->series[s]->indicatorType == IndicatorType){				
					count++;
				}
			}
		}
	}
	return count;
}

void CStockChartXCtrl::ResetBarColors() 
{	
	for(int bar = 0; bar != barColors.size(); ++bar){
		barColors[bar] = -1;
	}
	Update();
}

 /*
BOOL CStockChartXCtrl::ResetBarColors(LPCTSTR Key, long Panel, LPCTSTR Source, int Index, long Periods, int ColorR, int ColorG, int ColorB, int Style, int Thickness) 
{
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[Panel]->series[panels[Panel]->series.size() - 1]->paramStr[0] = Source;
	panels[Panel]->series[Index]->paramInt[1] = Periods;
	panels[Panel]->series[Index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[Panel]->series[Index]->lineStyle = Style;
	panels[Panel]->series[Index]->lineWeight = Thickness;

	return TRUE;


}
*/
void CStockChartXCtrl::OnIgnoreSeriesLengthErrorsChanged() 
{	
	SetModifiedFlag();
}

void CStockChartXCtrl::OnValueViewGradientTopChanged() 
{
	valueViewGradientTop = m_valueViewGradientTop;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnValueViewGradientBottomChanged() 
{	
	valueViewGradientBottom = m_valueViewGradientBottom;
	SetModifiedFlag();
}


void CStockChartXCtrl::OnBackGradientTopChanged() 
{
	backGradientTop = m_backGradientTop;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnBackGradientBottomChanged() 
{
	backGradientBottom = m_backGradientBottom;
	SetModifiedFlag();
}

void CStockChartXCtrl::OnCrossHairsChanged() 
{
	UpdateScreen(true);
	SetModifiedFlag();
}

void CStockChartXCtrl::OnMagneticChanged(void)
{
	//UpdateScreen(true);
	SetModifiedFlag();




}


double CStockChartXCtrl::GetPriceStyleValueByJDate1(double JDate) 
{

	double ret = NULL_VALUE;

	for(int n = 0; n != m_psValues1.size(); ++n){

		// There is a very strange bug with the VC++ 6.0
		// compiler that will not allow these two doubles
		// to be compaired! It does not occur in VC++ .NET
		// Please send me an email if you figure it out
		// t.wong@modulusfe.com
		CString date1 = FromJulianDate(JDate);
		CString date2 = FromJulianDate(m_psValues1[n].jdate);

		if(date1 == date2){
			ret = m_psValues1[n].value;
			break;
		}
	}

	return ret;
}

double CStockChartXCtrl::GetPriceStyleValueByJDate2(double JDate) 
{
	double ret = NULL_VALUE;

	for(int n = 0; n != m_psValues2.size(); ++n){

		// There is a very strange bug with the VC++ 6.0
		// compiler that will not allow these two doubles
		// to be compaired! It does not occur in VC++ .NET
		// Please send me an email if you figure it out
		// t.wong@modulusfe.com
		CString date1 = FromJulianDate(JDate);
		CString date2 = FromJulianDate(m_psValues2[n].jdate);

		if(date1 == date2){
			ret = m_psValues2[n].value;
			break;
		}
	}


	return ret;
}

double CStockChartXCtrl::GetPriceStyleValue1(long Record) 
{
	Record--;
	// add (unsigned int) for .NET compilation
	if(m_psValues1.size() == 0 || 
		Record < 0 || Record > (int)m_psValues1.size() -1)
		return NULL_VALUE;
	else
		return m_psValues1[Record].value;
}

double CStockChartXCtrl::GetPriceStyleValue2(long Record) 
{
	Record--;
	// add (unsigned int) for .NET compilation
	if(m_psValues2.size() == 0 ||
		Record < 0 || Record > (int)m_psValues2.size() -1)
		return NULL_VALUE;
	else
		return m_psValues2[Record].value;
}

 

double CStockChartXCtrl::GetPriceStyleValueByJDate3(double JDate) 
{
	double ret = NULL_VALUE;

	for(int n = 0; n != m_psValues3.size(); ++n){

		// There is a very strange bug with the VC++ 6.0
		// compiler that will not allow these two doubles
		// to be compaired! It does not occur in VC++ .NET
		// Please send me an email if you figure it out
		// t.wong@modulusfe.com
		CString date1 = FromJulianDate(JDate);
		CString date2 = FromJulianDate(m_psValues3[n].jdate);

		if(date1 == date2){
			ret = m_psValues3[n].value;
			break;
		}
	}


	return ret;
}

double CStockChartXCtrl::GetPriceStyleValue3(long Record) 
{
	Record--;
	// add (unsigned int) for .NET compilation
	if(m_psValues3.size() == 0 ||
		Record < 0 || Record > (int)m_psValues3.size() -1)
		return NULL_VALUE;
	else
		return m_psValues3[Record].value;
}

// RDG 8/16/04
// Enumerate all series
void CStockChartXCtrl::EnumSeries() 
{
	for(int n = 0; n < panels.size(); ++n){		
		for(int j = 0; j < panels[n]->series.size(); ++j){
			CString test = panels[n]->series[j]->szName;
			FireEnumSeries(panels[n]->series[j]->szName, n, 
				panels[n]->series[j]->seriesType);				
		}			
	}
}

// Move a series to different panel
void CStockChartXCtrl::MoveSeries(LPCTSTR Name, long FromPanel, long ToPanel) 
{		
	swapTo = ToPanel;	
	swapSeries.push_back(Name);
	OnLButtonUp(-1, m_point);
	swapTo = -1;
}

// MousePointer property 9/17/04 TW
void CStockChartXCtrl::OnMousePointerChanged() 
{	
	SetModifiedFlag();
}

// Candle outline colors 9/17/04 TW
void CStockChartXCtrl::OnCandleDownOutlineColorChanged() 
{
	candleDownOutlineColor = m_candleDownOutlineColor;
	Update();
	SetModifiedFlag();
}

void CStockChartXCtrl::OnCandleUpOutlineColorChanged() 
{
	candleUpOutlineColor = m_candleUpOutlineColor;
	Update();
	SetModifiedFlag();
}

// Requested by Martin Larsson 9/17/04
void CStockChartXCtrl::RecalculateIndicators() 
{
	reCalc = true;	
	Update();
	SetModifiedFlag();
}

BOOL CStockChartXCtrl::GetUserEditing() 
{	// 12/20/04 added objectSelected, updated 7/2/08,7/7/08
	if(objectSelected || resizing || dragging || drawing || lineDrawing || userZooming || typing){
		return TRUE;
	}
	else{
		return FALSE;
	}
}

void CStockChartXCtrl::SetUserEditing(BOOL bNewValue) 
{	
	SetModifiedFlag();
}


// Line study tools added 11/8/04
void CStockChartXCtrl::DrawLineStudy(long Type, LPCSTR Key) 
{
#ifdef _CONSOLE_DEBUG
	//printf("\nDrawLineStudy(%s)",Key);
#endif
	StopDrawing();
	UnSelectAll();
	movingObject = true;
	m_Cursor = IDC_PENCIL;
	m_mouseState = MOUSE_DRAWING;
	m_drawingType = Type;
	m_key = Key;

	//FireClick(); // 7/20/08
}

// Finds the most recent record where a cross over occured
long CStockChartXCtrl::CrossOverRecord(long Record, LPCTSTR Series1, LPCTSTR Series2) 
{

	CSeries* pSeries1 = NULL;
  int n, j;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(Series1, panels[n]->series[j]->szName)){
					pSeries1 = panels[n]->series[j];
					break;
				}
			}
		}
	}
	if(pSeries1 == NULL){
		ThrowErr(1920, "Cannot find Series 1");
		return NULL_VALUE;
	}

	CSeries* pSeries2 = NULL;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(Series2, panels[n]->series[j]->szName)){
					pSeries2 = panels[n]->series[j];
					break;
				}
			}
		}
	}
	if(pSeries2 == NULL){
		ThrowErr(1920, "Cannot find Series 2");
		return NULL_VALUE;
	}

    // Look for a cross over
	long recordCount = GetRecordCount();
	if(Record > recordCount) Record = recordCount;
	for(int record = Record - 1; record != 1; --record){

		double value1a = pSeries1->GetValue(record);
		double value1b = pSeries1->GetValue(record - 1);
    
		double value2a = pSeries2->GetValue(record);
		double value2b = pSeries2->GetValue(record - 1);
    
		if(value1a > value2a && value1b < value2b){
			// Series1 just crossed above series2
			return record + 1;
		}
		else if(value1a < value2a && value1b > value2b){
			// Series1 just crossed below series2
			return record + 1;
		}

	}

	return NULL_VALUE;
}

// Returns the cross over value for the EXACT record between the two points
double CStockChartXCtrl::CrossOverValue(long Record, LPCTSTR Series1, LPCTSTR Series2) 
{
	
	CSeries* pSeries1 = NULL;
  int n, j;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(Series1, panels[n]->series[j]->szName)){
					pSeries1 = panels[n]->series[j];
					break;
				}
			}
		}
	}
	if(pSeries1 == NULL){
		ThrowErr(1920, "Cannot find Series 1");
		return NULL_VALUE;
	}

	CSeries* pSeries2 = NULL;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size() ; ++j){
				if(CompareNoCase(Series2, panels[n]->series[j]->szName)){
					pSeries2 = panels[n]->series[j];
					break;
				}
			}
		}
	}
	if(pSeries2 == NULL){
		ThrowErr(1920, "Cannot find Series 2");
		return NULL_VALUE;
	}

    // Look for a cross over
	double value1a = 0, value1b = 0, value2a = 0, value2b = 0;
	long recordCount = GetRecordCount();
	if(Record > recordCount) Record = recordCount;
	for(int record = Record - 1; record != 1; --record){

		value1a = pSeries1->GetValue(record);
		value1b = pSeries1->GetValue(record - 1);
    
		value2a = pSeries2->GetValue(record);
		value2b = pSeries2->GetValue(record - 1);
    
		if(value1a > value2a && value1b < value2b){
			// Series1 just crossed above series2
			break;
		}
		else if(value1a < value2a && value1b > value2b){
			// Series1 just crossed below series2
			break;
		}

	}

    double A1 = 0, A2 = 0;
    double C1 = 0, C2  = 0;
    A1 = value1a - value1b;
    A2 = value2a - value2b;
    C1 = A1 + 1 * value1a;
    C2 = A2 + 1 * value2a;
    if(A2 == A1) return NULL_VALUE;
    return (C1 * A2 - A1 * C2) / (A2 - A1);

	return NULL_VALUE;

}

void CStockChartXCtrl::OnUserDrawingComplete(long StudyType, LPCTSTR Name)
{
	FireUserDrawingComplete(StudyType, Name);
	UnSelectAll();
}

void CStockChartXCtrl::OnYScaleMinTickChanged() 
{
	yScaleMinTick = m_yScaleMinTick;
	Update();
	SetModifiedFlag();
}


// 2/2/05
BOOL CStockChartXCtrl::IsSelected(LPCTSTR Key) 
{
	BOOL ret = FALSE;

  int n, j;
	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->lines.size(); ++j){
			if(CompareNoCase(panels[n]->lines[j]->key, Key)){
				if(panels[n]->lines[j]->selected){
					ret = TRUE;
					break;
				}
			}
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			if(CompareNoCase(panels[n]->objects[j]->key, Key)){
				if(panels[n]->objects[j]->selected){
					ret = TRUE;
					break;
				}
			}
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			if(CompareNoCase(panels[n]->textAreas[j]->key, Key)){
				if(panels[n]->textAreas[j]->selected){
					ret = TRUE;
					break;
				}
			}
		}
	}
	
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){
			if(CompareNoCase(panels[n]->series[j]->szName, Key)){
				if(panels[n]->series[j]->selected){
					ret = TRUE;
					break;
				}
			}
		}
	}

	return ret;
}

// 2/2/05
BSTR CStockChartXCtrl::GetSelectedKey() 
{
	CString strResult;
  int n, j;

	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->selected){
				strResult = panels[n]->lines[j]->key;
				break;
			}	
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->selected){
				strResult = panels[n]->objects[j]->key;
				break;
			}	
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			if(panels[n]->textAreas[j]->selected){
				strResult = panels[n]->textAreas[j]->key;
				break;
			}	
		}
	}
	
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){			
			if(panels[n]->series[j]->selected){
				strResult = panels[n]->series[j]->szName;
				break;
			}
		}
	}

	return strResult.AllocSysString();
}

// 2/2/05
long CStockChartXCtrl::GetSelectedType() 
{

	long ret = 0;
  int n;
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->selected){
				ret = otLineStudyObject;
				break;
			}	
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->selected){
				ret = otSymbolObject;
				break;
			}	
		}
	}

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			if(panels[n]->textAreas[j]->selected){
				ret = otTextObject;
				break;
			}	
		}
	}
	
	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){			
			if(panels[n]->series[j]->selected){
				ret = panels[n]->series[j]->seriesType;
				break;
			}
		}
	}

	return ret;
}

void CStockChartXCtrl::OnTextAreaFontSizeChanged() 
{	
	if(m_textAreaFontSize < 72){
		// 72pt = 720, 8pt = 80, etc.
		m_textAreaFontSize += 100;
	}
	textAreaFontSize = m_textAreaFontSize;
	SetModifiedFlag();
	Update();
}

void CStockChartXCtrl::OnTextAreaFontNameChanged() 
{
	textAreaFontName = m_textAreaFontName;
	SetModifiedFlag();
	Update();
}

// Adding this to the MouseMove event would break compatibility
short CStockChartXCtrl::GetCurrentPanel() 
{
	return m_currentPanel;
}

/*
Old GetObjectCount 6-27-10
long CStockChartXCtrl::GetObjectCount(long Type) 
{
	long count = 0;
     int n, j;

	for(n = 0; n != panels.size(); ++n){
		for(j = 0; j != panels[n]->lines.size(); ++j){
			if(panels[n]->lines[j]->nType == Type || Type == -1){
				count++;
			}
		}
	}
	if(count > 0) return count;

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->objects.size(); ++j){
			if(panels[n]->objects[j]->nType == Type){
				count++;				
			}
		}
	}
	if(count > 0) return count;

	if(Type == otTextObject){
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
				if(panels[n]->textAreas[j]->selected){
					count++;
				}
			}
		}
		return count;
	}

	// Added 9/26/2005
	if(Type == otStaticTextObject){
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			//	if(panels[n]->textAreas[j]->selected){
					count++;
				//}
			}
		}
		return count;
	}	

	for(n = 0; n != panels.size(); ++n){
		for(int j = 0; j != panels[n]->series.size(); ++j){			
			if(panels[n]->series[j]->seriesType == Type){
				count++;				
			}
		}
	}

	return count;
}
*/

//AhmadMusa Fix GetObjectCount Start
long CStockChartXCtrl::GetObjectCount(long Type) {
	long count = 0;
    int n, j;

	if(Type == otLineStudyObject || Type == otTrendLineObject){
		for(n = 0; n != panels.size(); ++n){
			for(j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->nType == Type || Type == -1){
					count++;				
				}
			}
		}
		return count;
	}

	if(Type == otExitShortSymbolObject || Type == otSignalSymbolObject || Type == otExitSymbolObject ||
	   Type == otExitLongSymbolObject || Type == otBuySymbolObject || Type == otSellSymbolObject || Type == otSymbolObject){
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->objects.size(); ++j){
				if(panels[n]->objects[j]->nType == Type){
					count++;				
				}
			}
		}
		return count;
	}

	if(Type == otTextObject){
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
				if(panels[n]->textAreas[j]->selected){
					count++;
				}
			}
		}
		return count;
	}

	// Added 9/26/2005
	if(Type == otStaticTextObject){
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
			//	if(panels[n]->textAreas[j]->selected){
					count++;
				//}
			}
		}
		return count;
	}	

	if(Type == otLineChart || Type == otVolumeChart || Type == otStockBarChart ||
	   Type == otStockBarChartHLC || Type == otCandleChart || Type == otIndicator || Type == otStockLineChart){	
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){			
				if(panels[n]->series[j]->seriesType == Type){
					count++;				
				}
			}
		}
		return count;
	}
	return 0;
}
//AhmadMusa Fix GetObjectCount End



void CStockChartXCtrl::ClearAllSeries() 
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				panels[n]->series[j]->Clear();				
			}
		}
	}
	updatingIndicator = false;
	reCalc = true;
	endIndex = 0;
}




// Eugen Begin 5/5/2005
short CStockChartXCtrl::AddCustomIndDlgIndPropStr(LPCTSTR IndicatorName, long ParamType, LPCTSTR DefValue) 
{
	CSeries *series = GetSeriesByNameType( IndicatorName, OBJECT_SERIES_INDICATOR );

  if( !series )
    return -1;
  if( series->indicatorType != indCustomIndicator )//user requested a wrong series
    return -1;

  return ((CIndicatorCustom*)series)->AddPropStr( ParamType, DefValue );
}

short CStockChartXCtrl::AddCustomIndDlgIndPropInt(LPCTSTR IndicatorName, long ParamType, long DefValue) 
{
  CSeries *series = GetSeriesByNameType( IndicatorName, OBJECT_SERIES_INDICATOR );
  
  if( !series )
    return -1;
  if( series->indicatorType != indCustomIndicator )//user requested a wrong series
    return -1;
  
  return ((CIndicatorCustom*)series)->AddPropInt( ParamType, DefValue );
}

short CStockChartXCtrl::AddCustomIndDlgIndPropDbl(LPCTSTR IndicatorName, long ParamType, double DefValue) 
{
  CSeries *series = GetSeriesByNameType( IndicatorName, OBJECT_SERIES_INDICATOR );
  
  if( !series )
    return -1;
  if( series->indicatorType != indCustomIndicator )//user requested a wrong series
    return -1;
  
  return ((CIndicatorCustom*)series)->AddPropDbl( ParamType, DefValue );
}

CSeries* CStockChartXCtrl::GetSeriesByNameType(LPCTSTR szName, long lType)
{
  for(int n = 0; n != panels.size(); ++n)
    for(int j = 0; j != panels[n]->series.size(); ++j)
      if(panels[n]->series[j]->seriesType == lType)
        if(CompareNoCase(szName, panels[n]->series[j]->szName))
         return panels[n]->series[j];
  return NULL;
}

void CStockChartXCtrl::FireCustomIndicatorEventNeedData(LPCTSTR IndicatorName)
{
  FireCustomIndicatorNeedData( IndicatorName );
}

long CStockChartXCtrl::SetCustomIndicatorData(LPCTSTR IndicatorName, const VARIANT FAR& Data, BOOL Append) 
{
  CSeries *series = GetSeriesByNameType( IndicatorName, OBJECT_SERIES_INDICATOR );
  
  if( !series || !Data.parray )//check pointer
    return -1;
  if( series->indicatorType != indCustomIndicator )//user requested a wrong series
    return -1;

  return ((CIndicatorCustom*)series)->SetData( Data.parray, Append );
}
// Eugen End 5/5/2005


// Line study z-order by RG 5/20/05
long CStockChartXCtrl::GetObjectZOrder(LPCTSTR Key, long ObjectType) 
{

	int n = 0;

	switch(ObjectType){
	
	case otSymbolObject:
	case otBuySymbolObject:
	case otSellSymbolObject:
	case otExitSymbolObject:
	case otExitLongSymbolObject:
	case otExitShortSymbolObject:
	case otSignalSymbolObject:
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->objects.size(); ++j){
				if(CompareNoCase(panels[n]->objects[j]->key, Key)){			
					return panels[n]->objects[j]->zOrder;					
				}
			}
		}
		break;


	case otLineStudyObject:	
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(CompareNoCase(panels[n]->lines[j]->key, Key)){			
					return panels[n]->lines[j]->zOrder;					
				}
			}
		}
		break;


	case otTextObject:
	case otStaticTextObject:
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
				if(CompareNoCase(panels[n]->textAreas[j]->key, Key)){			
					return panels[n]->textAreas[j]->zOrder;					
				}
			}
		}
		break;


	}

	return 0;
}

void CStockChartXCtrl::SetObjectZOrder(LPCTSTR Key, long ObjectType, long nNewValue) 
{

	int n = 0;

	switch(ObjectType){
	
	case otSymbolObject:
	case otBuySymbolObject:
	case otSellSymbolObject:
	case otExitSymbolObject:
	case otExitLongSymbolObject:
	case otExitShortSymbolObject:
	case otSignalSymbolObject:
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->objects.size(); ++j){
				if(CompareNoCase(panels[n]->objects[j]->key, Key)){			
					panels[n]->objects[j]->zOrder = nNewValue;
					break;
				}
			}
		}
		break;


	case otLineStudyObject:	
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(CompareNoCase(panels[n]->lines[j]->key, Key)){			
					panels[n]->lines[j]->zOrder = nNewValue;
					break;
				}
			}
		}
		break;


	case otTextObject:
	case otStaticTextObject:
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->textAreas.size(); ++j){
				if(CompareNoCase(panels[n]->textAreas[j]->key, Key)){			
					panels[n]->textAreas[j]->zOrder = nNewValue;
					break;
				}
			}
		}
		break;


	}

	changed = true;
	SetModifiedFlag();
	Update();
}



// **** FILL STYLES ARE CURRENTLY SUPPORTED FOR RECTANGLE AND ELLIPSE OBJECT ONLY **** //

long CStockChartXCtrl::GetObjectFillStyle(LPCTSTR Key, long ObjectType) 
{

	int n = 0;

	switch(ObjectType){
	
	case otSymbolObject:
	case otBuySymbolObject:
	case otSellSymbolObject:
	case otExitSymbolObject:
	case otExitLongSymbolObject:
	case otExitShortSymbolObject:
	case otSignalSymbolObject:
		ThrowError( CUSTOM_CTL_SCODE(1301), "Invalid type" );
		break;


	case otLineStudyObject:	
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(CompareNoCase(panels[n]->lines[j]->key, Key)){			
					return panels[n]->lines[j]->fillStyle;
					break;
				}
			}
		}
		break;


	case otTextObject:
	case otStaticTextObject:
		ThrowError( CUSTOM_CTL_SCODE(1301), "Invalid type" );
		break;

	}

	changed = true;
	SetModifiedFlag();
	Update();

	return 0;
}

void CStockChartXCtrl::SetObjectFillStyle(LPCTSTR Key, long ObjectType, long nNewValue) 
{
	int n = 0;

	switch(ObjectType){
	
	case otSymbolObject:
	case otBuySymbolObject:
	case otSellSymbolObject:
	case otExitSymbolObject:
	case otExitLongSymbolObject:
	case otExitShortSymbolObject:
	case otSignalSymbolObject:
		ThrowError( CUSTOM_CTL_SCODE(1301), "Invalid type" );
		break;


	case otLineStudyObject:	
		for(n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(CompareNoCase(panels[n]->lines[j]->key, Key)){			
					panels[n]->lines[j]->fillStyle = nNewValue;
					break;
				}
			}
		}
		break;


	case otTextObject:
	case otStaticTextObject:
		ThrowError( CUSTOM_CTL_SCODE(1301), "Invalid type" );
		break;

	}

	changed = true;
	SetModifiedFlag();
	Update();

}

// Added 1/23/2006
void CStockChartXCtrl::OnVolumePostfixLetterChanged() 
{	
	m_volumePostfix = m_volumePostfixLetter;
	SetModifiedFlag();		
}


// Added 6/20/2007
// Removes a trend line from the trend line watch list
void CStockChartXCtrl::RemoveTrendLineWatch(LPCTSTR TrendLineName, LPCTSTR SeriesName) 
{
	for(int n = 0; n < trendWatch.size(); ++n)
	{		
		
		if(CompareNoCase(trendWatch[n].TrendLineName, TrendLineName) && 
			CompareNoCase(trendWatch[n].SeriesName, SeriesName))
		{	
			// Replace the selectable property with what it was originally
			CCOL* pTrendLine = GetTrendLine(trendWatch[n].TrendLineName);
			if(pTrendLine != NULL) pTrendLine->selectable = trendWatch[n].WasSelectable;

			// Remove the trend watch
			std::vector<TrendLineWatch>::iterator itr = trendWatch.begin() + n;
			TrendLineWatch elem = *itr;
			trendWatch.erase(itr);
			n -= 1;
		}
	}
}

// Removes a study line from the UserStudyLine list
void CStockChartXCtrl::RemoveUserStudyLine(LPCTSTR StudyName, LPCTSTR SerieName)
{



	for(int n = 0; n < userStudyLine.size(); n++)
	{		
		
		if(CompareNoCase(userStudyLine[n].key, StudyName) && 
			CompareNoCase(userStudyLine[n].SerieName, SerieName))
		{	
			// Remove the user study line
			std::vector<UserStudyLine>::iterator itr = userStudyLine.begin() + n;
			//UserStudyLine elem = *itr;
			userStudyLine.erase(itr); 
			n -= 1;
		}
	}
	/*if(m_symbol.GetLength()>2){
		CString path = m_StudyDirectory;
		path.Append(m_symbol);
		path.Append(".sty");
		SaveObjectTemplate(path);
	}*/
}

// Find a study line by key on vector userStudyLine
int CStockChartXCtrl::FindUserStudyLine(LPCTSTR Key)
{
	for(int k=0;k<userStudyLine.size();k++)
	{
		if(CompareNoCase(userStudyLine[k].key,Key)) return k;
	}
	return NULL_VALUE;
}

void CStockChartXCtrl::LoadUserStudyLine(LONG panel/* = -1*/) //Load for all panels when "panel = -1 "
{
#ifdef _CONSOLE_DEBUG
	printf("\nLoadUserStudyLine()");
#endif
	if(userStudyLine.size()>0){
		SaveUserStudies();
		ClearUserStudies();
	}
	
	if(m_symbol.GetLength()>2){
		CString path = m_StudyDirectory;
		path.Append(m_symbol);
		path.Append(".sty");
		
		LoadObjectTemplate(path);
	}

	for(int n = 0; n < userStudyLine.size(); ++n)
	{
		//Load studies for a panel:
		if(panel>-1 && panel<panels.size())
		{
		}
		//Load studies for all panels:
		else
		{
			for(int k=0;k<panels.size();++k)
			{
				if(panels[k]->visible){
					if(CompareNoCase(userStudyLine[n].SerieName, panels[k]->series[0]->szName)/*&&(userStudyLine[n].x1JDate>panels[k]->series[0]->data_master[0].jdate||userStudyLine[n].x2JDate>panels[k]->series[0]->data_master[panels[k]->series[0]->data_master.size()-1].jdate)/*&&((double)GetYPixel(k,userStudyLine[n].y1Value)>panels[k]->y1)&&((double)GetYPixel(k,userStudyLine[n].y2Value)<panels[k]->y2)*/)
					{	
						#pragma region Study Lines:
						if( (CString(userStudyLine[n].objectType) == "TrendLine")){
							panels[k]->lines.push_back( new CLineStandard( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );					
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size() - 1]->leftExtension = userStudyLine[n].leftExtension;
							panels[k]->lines[panels[k]->lines.size() - 1]->valuePosition = userStudyLine[n].valuePosition;
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size() - 1]->drawn = true;
#ifdef _CONSOLE_DEBUG
							//printf(" valuePosition = %f", panels[k]->lines[panels[k]->lines.size() - 1]->valuePosition);
#endif




						}
						else if( (CString(userStudyLine[n].objectType) == "Polyline")){
							panels[k]->lines.push_back( new CLineStudyPolyline( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );					
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							//panels[k]->lines[panels[k]->lines.size()-1]->xValues	= userStudyLine[n].xValues;
							panels[k]->lines[panels[k]->lines.size()-1]->xValues.clear();
							for(int i=0;i<userStudyLine[n].xJDates.size();i++)panels[k]->lines[panels[k]->lines.size()-1]->xValues.push_back(GetRecordByPeriodJDate(userStudyLine[n].xJDates[i]));
							panels[k]->lines[panels[k]->lines.size()-1]->yValues	= userStudyLine[n].yValues;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							panels[k]->lines[panels[k]->lines.size()-1]->nType = userStudyLine[n].nType; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "Ellipse" )){
							panels[k]->lines.push_back( new CLineStudyEllipse( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "Rectangle" )){
							panels[k]->lines.push_back( new CLineStudyRectangle( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "Triangle" )){
							panels[k]->lines.push_back( new CLineStudyTriangle( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "SpeedLines" )){
							panels[k]->lines.push_back( new CLineStudySpeedLines( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "GannFan" )){
							panels[k]->lines.push_back( new CLineStudyGannFan( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->xValues.clear();
							for(int i=0;i<userStudyLine[n].xJDates.size();i++)panels[k]->lines[panels[k]->lines.size()-1]->xValues.push_back(GetRecordByPeriodJDate(userStudyLine[n].xJDates[i]));
							panels[k]->lines[panels[k]->lines.size()-1]->yValues	= userStudyLine[n].yValues;
							panels[k]->lines[panels[k]->lines.size()-1]->xJDates	= userStudyLine[n].xJDates;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							panels[k]->lines[panels[k]->lines.size()-1]->upOrDown = userStudyLine[n].upOrDown; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;






						}
						else if( (CString(userStudyLine[n].objectType) == "FibonacciArcs" )){
							panels[k]->lines.push_back( new CLineStudyFibonacciArcs( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "FibonacciFan" )){
							panels[k]->lines.push_back( new CLineStudyFibonacciFan( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->xValues.clear();
							for(int i=0;i<userStudyLine[n].xJDates.size();i++)panels[k]->lines[panels[k]->lines.size()-1]->xValues.push_back(GetRecordByPeriodJDate(userStudyLine[n].xJDates[i]));
							panels[k]->lines[panels[k]->lines.size()-1]->yValues	= userStudyLine[n].yValues;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;







						}
						else if( (CString(userStudyLine[n].objectType) == "FibonacciRetracements" )){
							panels[k]->lines.push_back( new CLineStudyFibonacciRetracements( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[0] = userStudyLine[n].params[0]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[1] = userStudyLine[n].params[1]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[2] = userStudyLine[n].params[2]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[3] = userStudyLine[n].params[3]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[4] = userStudyLine[n].params[4];  
							panels[k]->lines[panels[k]->lines.size()-1]->params[5] = userStudyLine[n].params[5]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[6] = userStudyLine[n].params[6]; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;






						}
						else if( (CString(userStudyLine[n].objectType) == "FibonacciProgression" )){
							panels[k]->lines.push_back( new CLineStudyFibonacciProgression( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[0] = userStudyLine[n].params[0]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[1] = userStudyLine[n].params[1]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[2] = userStudyLine[n].params[2]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[3] = userStudyLine[n].params[3]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[4] = userStudyLine[n].params[4]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[5] = userStudyLine[n].params[5]; 
							panels[k]->lines[panels[k]->lines.size()-1]->params[6] = userStudyLine[n].params[6]; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "FibonacciTimeZones" )){
							panels[k]->lines.push_back( new CLineStudyFibonacciTimeZones( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "TironeLevels" )){
							panels[k]->lines.push_back( new CLineStudyTironeLevels( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "QuadrantLines" )){
							panels[k]->lines.push_back( new CLineStudyQuadrantLines( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "RaffRegression" )){
							panels[k]->lines.push_back( new CLineStudyRaffRegression( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}
						else if( (CString(userStudyLine[n].objectType) == "ErrorChannel" )){
							panels[k]->lines.push_back( new CLineStudyErrorChannel( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						}			
						else if( (CString(userStudyLine[n].objectType) == "Freehand" )){
							panels[k]->lines.push_back( new CLineStudyFreehand( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;
						} 
						else if( (CString(userStudyLine[n].objectType) == "Channel" )){
							panels[k]->lines.push_back( new CLineStudyChannel( userStudyLine[n].lineColor, userStudyLine[n].key, panels[k] ) );			
							panels[k]->lines[panels[k]->lines.size()-1]->Connect(this);
							panels[k]->lines[panels[k]->lines.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->lines[panels[k]->lines.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->lines[panels[k]->lines.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							panels[k]->lines[panels[k]->lines.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->lines[panels[k]->lines.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->lines[panels[k]->lines.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->lines[panels[k]->lines.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->lines[panels[k]->lines.size()-1]->zOrder		= userStudyLine[n].zOrder;
							panels[k]->lines[panels[k]->lines.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							panels[k]->lines[panels[k]->lines.size()-1]->vFixed		= userStudyLine[n].vFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->hFixed		= userStudyLine[n].hFixed;
							panels[k]->lines[panels[k]->lines.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							panels[k]->lines[panels[k]->lines.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							panels[k]->lines[panels[k]->lines.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->lines[panels[k]->lines.size()-1]->SnapLine();
							panels[k]->lines[panels[k]->lines.size()-1]->drawing = false;		
							panels[k]->lines[panels[k]->lines.size()-1]->drawn = true;	
							panels[k]->lines[panels[k]->lines.size()-1]->state = 3;
						}	
						#pragma endregion
						#pragma region Text Areas:
						else if( (CString(userStudyLine[n].objectType) == "Text Area" )){
							panels[k]->textAreas.push_back(new CTextArea(NULL_VALUE,NULL_VALUE, panels[k]));		
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->Connect(this);
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->lineColor	= userStudyLine[n].lineColor;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->y2Value	= userStudyLine[n].y2Value;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->x1Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate_2);
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->y1Value_2	= userStudyLine[n].y1Value_2;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->x2Value_2	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate_2);
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->y2Value_2	= userStudyLine[n].y2Value_2;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->x1JDate_2	= userStudyLine[n].x1JDate_2;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->x2JDate_2	= userStudyLine[n].x2JDate_2;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->zOrder		= userStudyLine[n].zOrder;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->fillStyle	= userStudyLine[n].fillStyle;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->vFixed		= userStudyLine[n].vFixed;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->hFixed		= userStudyLine[n].hFixed;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->isFirstPointArrow = userStudyLine[n].isFirstPointArrow;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->radiusExtension = userStudyLine[n].radiusExtension;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->rightExtension = userStudyLine[n].rightExtension;
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->leftExtension = userStudyLine[n].leftExtension; 
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->SnapLine();
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->drawing = false;		
							//panels[k]->textAreas[panels[k]->textAreas.size()-1]->drawn = true;
									
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->key = userStudyLine[n].key;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->startX = userStudyLine[n].startX;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->startY = userStudyLine[n].startY;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->fontSize = userStudyLine[n].fontSize;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->Text = userStudyLine[n].Text;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->fontColor = userStudyLine[n].fontColor;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->typing = userStudyLine[n].typing;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->gotUserInput = userStudyLine[n].gotUserInput;
							panels[k]->textAreas[panels[k]->textAreas.size()-1]->Update();

							typing = false;
							KillCaret();
							//int x1Int,x2Int,y1Int,y2Int;
							//CRect rect = new CRect(x1Int,y1Int,x2Int,y2Int);
							//UpdateRect(rect);

						}
						#pragma endregion
						#pragma region Symbols:		
						else if((CString(userStudyLine[n].objectType) == "Exit Symbol")||(CString(userStudyLine[n].objectType) == "Sell Symbol")||(CString(userStudyLine[n].objectType) == "Buy Symbol")){								
								
							if((userStudyLine[n].objectType == "Buy Symbol")){
								panels[k]->objects.push_back(new 
									CSymbolObject(0, 0, IDB_BUY, "", "", "Buy Symbol", panels[k]));
							}
							else if((userStudyLine[n].objectType == "Sell Symbol"))
							{
								panels[k]->objects.push_back(new 
									CSymbolObject(0, 0, IDB_SELL, "", "", "Sell Symbol", panels[k]));
							}
							else if((userStudyLine[n].objectType == "Exit Symbol"))
							{
								panels[k]->objects.push_back(new 
									CSymbolObject(0, 0, IDB_EXIT, "", "", "Exit Symbol", panels[k]));
							}
							else return;

							panels[k]->objects[panels[k]->objects.size()-1]->lineStyle	= userStudyLine[n].lineStyle;
							panels[k]->objects[panels[k]->objects.size()-1]->lineWeight	= userStudyLine[n].lineWeight;
							panels[k]->objects[panels[k]->objects.size()-1]->x1Value	= GetRecordByPeriodJDate(userStudyLine[n].x1JDate);
							panels[k]->objects[panels[k]->objects.size()-1]->y1Value	= userStudyLine[n].y1Value;
							panels[k]->objects[panels[k]->objects.size()-1]->x2Value	= GetRecordByPeriodJDate(userStudyLine[n].x2JDate);
							panels[k]->objects[panels[k]->objects.size()-1]->y2Value	= userStudyLine[n].y2Value;
							panels[k]->objects[panels[k]->objects.size()-1]->x1JDate	= userStudyLine[n].x1JDate;
							panels[k]->objects[panels[k]->objects.size()-1]->x2JDate	= userStudyLine[n].x2JDate;
							panels[k]->objects[panels[k]->objects.size()-1]->selectable	= userStudyLine[n].selectable;
							panels[k]->objects[panels[k]->objects.size()-1]->selected	= userStudyLine[n].selected;
							panels[k]->objects[panels[k]->objects.size()-1]->zOrder		= userStudyLine[n].zOrder;

							//panels[k]->objects[panels[k]->objects.size()-1]->bitmapID = userStudyLine[n].bitmapID;
							panels[k]->objects[panels[k]->objects.size()-1]->backColor = userStudyLine[n].backColor;
							panels[k]->objects[panels[k]->objects.size()-1]->foreColor = userStudyLine[n].foreColor;
							panels[k]->objects[panels[k]->objects.size()-1]->text = userStudyLine[n].text;
							//panels[k]->objects[panels[k]->objects.size()-1]->nType = userStudyLine[n].nType;
							panels[k]->objects[panels[k]->objects.size()-1]->fileName = userStudyLine[n].fileName;
							panels[k]->objects[panels[k]->objects.size()-1]->key = userStudyLine[n].key;
							panels[k]->objects[panels[k]->objects.size() - 1]->Connect(this);
							panels[k]->objects[panels[k]->objects.size() - 1]->SetXY();
							//panels[k]->objects[panels[k]->objects.size() - 1]->UpdatePoints();	
									
						}
						#pragma endregion
						UpdateScreen(true);
						Update();
						RePaint();
					}
				
				}
			}	
		}
	}
}

// Save all study lines, text areas and symbols
void CStockChartXCtrl::SaveUserStudies(void)
{		
#ifdef _CONSOLE_DEBUG
	//printf("\nSaveUserStudyLine()");
#endif

	//Ends all drawings
	for(int k=0;k<panels.size();k++)
	{
		if(panels[k]->visible)
		{
			//Save Study Lines:
			for(int n=0;n<panels[k]->lines.size();n++){
				panels[k]->lines[n]->SnapLine();
			}
		}
	}
	//Update Memory
	for(int k=0;k<panels.size();k++)
	{
		if(panels[k]->visible)
		{
			//Save Study Lines:
			for(int n=0;n<panels[k]->lines.size();n++){
				AddUserStudyLine(panels[k]->series[0]->szName,panels[k]->lines[n]);
				
			}
			//Save Text Objects:
			for(int n=0;n<panels[k]->textAreas.size();n++){
				AddUserStudyLine(panels[k]->series[0]->szName,panels[k]->textAreas[n]);
				
			}
			//Save Symbols:
			for(int n=0;n<panels[k]->objects.size();n++){
				AddUserStudyLine(panels[k]->series[0]->szName,panels[k]->objects[n]);				
			}
		}
	}
	//Update Serializer	
	if(m_symbol.GetLength()>2){
		CString path = m_StudyDirectory;
		path.Append(m_symbol);
		path.Append(".sty");
#ifdef _CONSOLE_DEBUG
		//printf("\nSaveUserStudies()\n\tStudyDirectory="+m_StudyDirectory+"\n\tPath="+path);
#endif
		SaveObjectTemplate(path);
	}
}

// Clear all study lines, text areas and symbols
void CStockChartXCtrl::ClearUserStudies(void)
{
#ifdef _CONSOLE_DEBUG
	//printf("\nClearUserStudies()");
#endif
	for(int k=0;k<panels.size();k++)
	{
		if(panels[k]->visible)
		{
			//Clear Study Lines:
			panels[k]->lines.clear();
			//Clear Text Objects:
			panels[k]->textAreas.clear();
			//Clear Symbols:
			panels[k]->objects.clear();
		}
	}
}

// Added 6/20/2007
// Adds a trend line to the trendWatch[] so that AppendValue can 
// call a function to determine if a trend line has been penetrated by
// a specific series. If the line is penetrated, the trend line watch
// function will fire the TrendLinePenetrated() event.
void CStockChartXCtrl::AddTrendLineWatch(LPCTSTR TrendLineName, LPCTSTR SeriesName) 
{
	TrendLineWatch item;
	item.TrendLineName = TrendLineName;
	item.SeriesName = SeriesName;
	CCOL* pTrendLine = GetTrendLine(TrendLineName);
	if(pTrendLine != NULL) item.WasSelectable = pTrendLine->selectable;
	trendWatch.push_back(item);
}

void CStockChartXCtrl::AddUserStudyLine(LPCTSTR SerieName, CCOL* Study)
{
	UserStudyLine newUserStudyLine;
	newUserStudyLine.SerieName = SerieName;
	
	newUserStudyLine.objectType	= Study->objectType;
    newUserStudyLine.key		= Study->key;
    newUserStudyLine.lineStyle	= Study->lineStyle;
    newUserStudyLine.lineWeight	= Study->lineWeight;
    newUserStudyLine.lineColor	= Study->lineColor;
    newUserStudyLine.x1Value	= Study->x1Value;
    newUserStudyLine.y1Value	= Study->y1Value;
    newUserStudyLine.x2Value	= Study->x2Value;
    newUserStudyLine.y2Value	= Study->y2Value;
	newUserStudyLine.x1Value_2	= Study->x1Value_2;
    newUserStudyLine.y1Value_2	= Study->y1Value_2;
    newUserStudyLine.x2Value_2	= Study->x2Value_2;
    newUserStudyLine.y2Value_2	= Study->y2Value_2;
	newUserStudyLine.x1JDate	= Study->x1JDate;
	newUserStudyLine.x2JDate	= Study->x2JDate;
	newUserStudyLine.x1JDate_2	= Study->x1JDate_2;
	newUserStudyLine.x2JDate_2	= Study->x2JDate_2;
	newUserStudyLine.xValues	= Study->xValues;
	newUserStudyLine.xJDates	= Study->xJDates;
	newUserStudyLine.yValues	= Study->yValues;
	newUserStudyLine.upOrDown	= Study->upOrDown;
    newUserStudyLine.selectable	= Study->selectable;
    newUserStudyLine.selected	= Study->selected;
    newUserStudyLine.zOrder		= Study->zOrder;
	newUserStudyLine.fillStyle	= Study->fillStyle;
	newUserStudyLine.vFixed		= Study->vFixed;
	newUserStudyLine.hFixed		= Study->hFixed;
    newUserStudyLine.isFirstPointArrow = Study->isFirstPointArrow;
	newUserStudyLine.radiusExtension = Study->radiusExtension;
	newUserStudyLine.rightExtension = Study->rightExtension;
	newUserStudyLine.leftExtension = Study->leftExtension;
	newUserStudyLine.valuePosition = Study->valuePosition;
	newUserStudyLine.params = Study->params;

#ifdef _CONSOLE_DEBUG
	//printf("\nAddUserStudyLine() valuePosition = %f", newUserStudyLine.valuePosition);
#endif

	if(FindUserStudyLine(Study->key)==NULL_VALUE) {
		userStudyLine.push_back(newUserStudyLine);
	}
	else {
		userStudyLine[FindUserStudyLine(Study->key)] = newUserStudyLine;
	}
	RePaint();
}

void CStockChartXCtrl::AddUserStudyLine(LPCTSTR SerieName, CTextArea* Study)
{	
	UserStudyLine newUserStudyLine;
	newUserStudyLine.SerieName = SerieName;
	
	newUserStudyLine.objectType	= "Text Area";
    newUserStudyLine.key		= Study->key;
    //newUserStudyLine.lineStyle	= Study->lineStyle;
    //newUserStudyLine.lineWeight	= Study->lineWeight;
    //newUserStudyLine.lineColor	= Study->lineColor;
    newUserStudyLine.x1Value	= Study->x1Value;
    newUserStudyLine.y1Value	= Study->y1Value;
    newUserStudyLine.x2Value	= Study->x2Value;
    newUserStudyLine.y2Value	= Study->y2Value;
	newUserStudyLine.x1JDate	= Study->x1JDate;
	newUserStudyLine.x2JDate	= Study->x2JDate;
    newUserStudyLine.selectable	= Study->selectable;
    newUserStudyLine.selected	= Study->selected;
    newUserStudyLine.zOrder		= Study->zOrder;
	//newUserStudyLine.fillStyle	= Study->fillStyle;
	//newUserStudyLine.vFixed		= Study->vFixed;
	//newUserStudyLine.hFixed		= Study->hFixed;
    //newUserStudyLine.isFirstPointArrow = Study->isFirstPointArrow;
	//newUserStudyLine.radiusExtension = Study->radiusExtension;
	//newUserStudyLine.rightExtension = Study->rightExtension;
	//newUserStudyLine.leftExtension = Study->leftExtension;
		
	newUserStudyLine.startX		= Study->startX;
	newUserStudyLine.startY		= Study->startY;
	newUserStudyLine.fontSize	= Study->fontSize;
	newUserStudyLine.Text		= Study->Text;
	newUserStudyLine.fontColor	= Study->fontColor;
	newUserStudyLine.typing = false;
	newUserStudyLine.gotUserInput = true;
	if(FindUserStudyLine(Study->key)==NULL_VALUE) userStudyLine.push_back(newUserStudyLine);
	else userStudyLine[FindUserStudyLine(Study->key)] = newUserStudyLine;
}
 
void CStockChartXCtrl::AddUserStudyLine(LPCTSTR SerieName, CCO* Study)
{	
	if(!(Study->objectType=="Sell Symbol"||Study->objectType=="Buy Symbol"||Study->objectType=="Exit Symbol"))return;
	UserStudyLine newUserStudyLine;
	newUserStudyLine.SerieName = SerieName;
	
	newUserStudyLine.objectType	= Study->objectType;
    newUserStudyLine.key		= Study->key;
    newUserStudyLine.lineStyle	= Study->lineStyle;
    newUserStudyLine.lineWeight	= Study->lineWeight;
    newUserStudyLine.x1Value	= Study->x1Value;
    newUserStudyLine.y1Value	= Study->y1Value;
    newUserStudyLine.x2Value	= Study->x2Value;
    newUserStudyLine.y2Value	= Study->y2Value;
	newUserStudyLine.x1JDate	= Study->x1JDate;
	newUserStudyLine.x2JDate	= Study->x2JDate;
    newUserStudyLine.selectable	= Study->selectable;
    newUserStudyLine.selected	= Study->selected;
    newUserStudyLine.zOrder		= Study->zOrder;
		
	newUserStudyLine.bitmapID		= Study->bitmapID;
	newUserStudyLine.backColor		= Study->backColor;
	newUserStudyLine.text			= Study->text;
	newUserStudyLine.nType			= Study->nType;
	newUserStudyLine.fileName		= Study->fileName;

	
	if(FindUserStudyLine(Study->key)==NULL_VALUE) userStudyLine.push_back(newUserStudyLine);
	else userStudyLine[FindUserStudyLine(Study->key)] = newUserStudyLine;
}

// Added 6/20/2007
// Fires an event if the series has crossed any trend lines
void CStockChartXCtrl::WatchTrendLines(LPCTSTR SeriesName)
{
	// For each trend line that we are watching...
	for(int n = 0; n < (int)trendWatch.size(); ++n)
	{

		// Find the trendline
		CCOL* pTrendLine = GetTrendLine(trendWatch[n].TrendLineName);
		if(pTrendLine == NULL)
		{
			// Remove this watch if the trend line is missing
			RemoveTrendLineWatch(trendWatch[n].TrendLineName, trendWatch[n].SeriesName);
		}
		else
		{

			// This trendline cannot be selectable when it is being watched
			pTrendLine->selectable = false;

			// Find all trendlines associated with this series
			if(CompareNoCase(trendWatch[n].SeriesName, SeriesName))
			{
				CSeries* pSeries = GetSeriesByName(trendWatch[n].SeriesName);			
				if(pSeries == NULL)
				{
					// Remove this watch if the series can't be found
					RemoveTrendLineWatch(trendWatch[n].TrendLineName, trendWatch[n].SeriesName);		
				}
				else
				{

					// Automatically extend the trend line into the future
					double x1 = pTrendLine->x1Value; // First record #
					double y1 = pTrendLine->y1Value; // First value
					double x2 = pTrendLine->x2Value; // Last record #
					double y2 = pTrendLine->y2Value; // Last value
					double incr = 0;
					double diff = (y2 - y1);				 
					incr = (y2 - y1) / (x2 - x1);
					pTrendLine->y2Value = pTrendLine->y1Value + (incr * (GetRecordCount() - x1));
					pTrendLine->x2Value = pTrendLine->x1Value + (GetRecordCount() - x1);			

					// Check to see if the trendline crossed over or under the specified series				
					double pointB = pTrendLine->y2Value;
					double pointA = pTrendLine->y2Value - incr;

					if(pSeries->data_master.size() > 1)
					{
						int size = pSeries->data_master.size();
						if(pSeries->data_master[size - 1].value > pointB && 
							pSeries->data_master[size - 2].value < pointA)
						{
							// Series crossed above trendline
							FireTrendLinePenetration(trendWatch[n].TrendLineName, trendWatch[n].SeriesName, 1);
						}
						else if(pSeries->data_master[size - 1].value < pointB && 
							pSeries->data_master[size - 2].value > pointA)
						{
							// Series crossed below trendline
							FireTrendLinePenetration(trendWatch[n].TrendLineName, trendWatch[n].SeriesName, -1);
						}
					}
				}
			}
		}
		
	}
}

// Added 6/20/2007
CCOL* CStockChartXCtrl::GetTrendLine(LPCSTR Key)
{
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(CompareNoCase(panels[n]->lines[j]->key, Key)){
					return panels[n]->lines[j];
				}
			}
		}
	}
	return NULL;
}

//  Added 6/20/2007
CSeries* CStockChartXCtrl::GetSeriesByName(LPCTSTR szName)
{
  for(int n = 0; n != panels.size(); ++n){
    for(int j = 0; j != panels[n]->series.size(); ++j){		
        if(CompareNoCase(szName, panels[n]->series[j]->szName))
         return panels[n]->series[j];
	}
  }

  return NULL;
}



// Personal license restriction added 8/28/07
bool CStockChartXCtrl::IsLicenseValid()
{
 
	// the personal license must always check for the license 
	// file and is limited to only three installations:
	#ifdef PERSONAL_LICENSE
		// Personal license must check the license file
		// because it can only run on this computer
	#else
		return true;
	#endif

	// Get the hardware id		
	std::string id;
	HKEY hKey;
	char *psz;
	TCHAR szBuff1[MAX_PATH]={0};
	TCHAR szBuff2[MAX_PATH]={0};
	TCHAR szBuff3[MAX_PATH]={0};
	DWORD dwType = 0, dwLen=sizeof(szBuff1);
  
	// Bios date
	psz = "HARDWARE\\DESCRIPTION\\System";
	HRESULT hr = RegOpenKeyEx(HKEY_LOCAL_MACHINE, psz, 0, KEY_READ, &hKey);
		
	psz = "SystemBiosDate";
	hr = RegQueryValueEx(hKey, psz, 0, &dwType, (LPBYTE)szBuff1, &dwLen);
	
	// Central processor 0 Identifier
	dwLen=sizeof(szBuff2);
	psz = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";
	hr = RegOpenKeyEx(HKEY_LOCAL_MACHINE, psz, 0, KEY_READ, &hKey);
	
	psz = "Identifier";
	hr = RegQueryValueEx(hKey, psz, 0, &dwType, (LPBYTE)szBuff2, &dwLen);
	
	// Append
	id.append(szBuff1);
	id.append(szBuff2);
	if(id == "") id = "modulus-bt8-keycode"; //Failsafe

	// Encode or decode and extract letters
	std::string key = "bt8";
	std::string temp, chr;
	int i = 0;
	for(i=0; i < (int)id.length(); ++i)
	{			
		char c = (char)key[ i % key.length() ] ^ id[i];
		if((c >= 48 && c <= 57) ||
			(c >= 65 && c <= 90) ||
			(c >= 97 && c <= 122)) temp += c;		
	}
	id = temp;

	// Now create the encoded hardware id
	char hid[1024];
	strncpy(hid,id.c_str(),1024); 
	std::string encId;
	temp = chr = "";		
	for(i=0; i < (int)id.length(); ++i)
	{
		char c = hid[i];
		if((c >= 48 && c < 57) ||
			(c >= 65 && c < 90) ||
			(c >= 97 && c < 121))
		{
			temp += (c + 1);
		}
		else
		{
			temp += c;
		}
	}
	encId = temp;


	// Get the path of the registered StockChartX.ocx file
	dwType = REG_SZ; dwLen=sizeof(szBuff3);
	psz = "CLSID\\{89D9AAAF-DC4F-4F72-ACF3-D2EDB8644A44}\\InprocServer32";
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT, psz, 0, KEY_READ, &hKey);
	if(hr != ERROR_SUCCESS) return false;	
	hr = RegQueryValueEx(hKey, NULL, 0, &dwType, (LPBYTE)szBuff3, &dwLen);
	if(hr != ERROR_SUCCESS) return false;
	std::string ocxPath = szBuff3;
	TCHAR szShortPath[MAX_PATH];
	_tcscpy( szShortPath, szBuff3 );
	GetLongPathName( szShortPath, szBuff3, MAX_PATH );	
	ocxPath = szBuff3;


	//Read the license from file
	std::string fileName = ocxPath; 	
	for(i = fileName.length(); i != 0; --i)
	{
		if(fileName.substr(i,1) == "\\")			
			break;
	}
	if(i > 0) fileName = fileName.substr(0,i);
	fileName.append("\\StockChartX.plc");
		
	//Read the license from file
	char licenseText[1024] = "";
	std::ifstream in(fileName.c_str(), std::ios::binary);
	in.read( (char*) &licenseText, sizeof(licenseText) );
	std::string license = licenseText;
	if(license != encId)
	{
		AfxMessageBox("Development license missing or invalid.", MB_ICONERROR);
		return false;
	}
	else{
		return true;
	}

 }


 // 9/20/07
long CStockChartXCtrl::GetIndicatorType(LPCTSTR SeriesName) 
{
	int n;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				if(CompareNoCase(SeriesName, panels[n]->series[j]->szName)){					
					return panels[n]->series[j]->indicatorType;
				}
			}
		}
	}
	return -1;
}


// Edits a date by record 1/31/08
void CStockChartXCtrl::EditJDate(long Record, double JDate) 
{
	int n;
	for(n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){				
				panels[n]->series[j]->EditJDate(Record - 1, JDate);
				reCalc = true;
				changed = true;
				return;				
			}
		}
	}
}



void CStockChartXCtrl::OnExtraTimePrecisionChanged() 
{
	// TODO: Add notification handler code

	SetModifiedFlag();
}

BOOL CStockChartXCtrl::ExtraPrecision()
{
  return m_extraTimePrecision;
}

void CStockChartXCtrl::Freeze(BOOL freeze)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_frozen = (freeze == TRUE);
}

//New methods to add Indicators:

VARIANT_BOOL CStockChartXCtrl::AddIndicatorSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorSimpleMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;



}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorHILO(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG PeriodsHigh, LONG PeriodsLow, LONG Shift, DOUBLE Scale, OLE_COLOR LineColorHigh, OLE_COLOR LineColorLow, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorHILOActivator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = PeriodsHigh; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = PeriodsLow; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = Shift; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[4] = Scale; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = LineColorHigh;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = LineColorLow;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "HILO";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+PeriodsHigh+","+PeriodsLow+","+Shift+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorHILO(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG PeriodsHigh, LONG PeriodsLow, LONG Shift, DOUBLE Scale, OLE_COLOR LineColorHigh, OLE_COLOR LineColorLow, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol;
	panels[panel]->series[index]->paramInt[1] = PeriodsHigh;
	panels[panel]->series[index]->paramInt[2] = PeriodsLow;
	panels[panel]->series[index]->paramInt[3] = Shift;
	panels[panel]->series[index]->paramDbl[4] = Scale;
	panels[panel]->series[index]->lineColor = LineColorHigh;
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[index]->lineColorSignal = LineColorLow;
	panels[panel]->series[index]->lineStyleSignal = Style;
	panels[panel]->series[index]->lineWeightSignal = Thickness;
	panels[panel]->series[index]->szTitle = "HILO";
	panels[panel]->series[index]->szTitle += CString("("+PeriodsHigh+","+PeriodsLow+","+Shift+")");
		
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = LineColorLow;
	panels[panel]->series[index+1]->lineStyle = Style;
	panels[panel]->series[index+1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorASI(LPCTSTR Key, LONG panel, LPCTSTR Source, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorAccumulativeSwingIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[1] = LimitMove; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorASI(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramDbl[1] = LimitMove; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorADX(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB,  LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;	
	panels[panel]->series.push_back(new CIndicatorADX(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	
	
	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorADX(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorDI(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorDI(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	//Twin Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style2;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness2;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorDI(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyleSignal = Style2;
	panels[panel]->series[index]->lineWeightSignal = Thickness2;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;

	return TRUE;
}



VARIANT_BOOL CStockChartXCtrl::AddIndicatorAroon(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;	
	panels[panel]->series.push_back(new CIndicatorAroon(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	
	//Parameters for Twin
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style2;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness2;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorAroon(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyleSignal = Style2;
	panels[panel]->series[index]->lineWeightSignal = Thickness2;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorAroonOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;	
	panels[panel]->series.push_back(new CIndicatorAroonOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorAroonOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorBollinger(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorBollingerBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = StandardDev; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "BB";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+","+StandardDev+")");
	//printf("\n\nAdded BB Weight=%d",Thickness);

  
	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorBollinger(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;
	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramDbl[2] = StandardDev; 
	panels[panel]->series[index]->paramInt[3] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[index]->szTitle = "BB";
	panels[panel]->series[index]->szTitle += CString("("+Periods+","+StandardDev+")");
	
	//Parameters for Twin:
	panels[panel]->series[index+1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index+1]->lineStyle = Style;
	panels[panel]->series[index+1]->lineWeight = Thickness;
  
	return TRUE;
	
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorChaikinMoney(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorChaikinMoneyFlow(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorChaikinMoney(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->paramInt[2] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorAccumulationDistribution(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

#ifdef _CONSOLE_DEBUG
	//printf("\nAddIndicatorAccumulationDistribution()");
#endif

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorAccumulationDistribution(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorAccumulationDistribution(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorChaikinVolatility(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG RateChange, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorChaikinVolatility(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = RateChange;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorChaikinVolatility(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG RateChange, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = RateChange;
	panels[panel]->series[index]->paramInt[3] = MAType;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorChandeMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{



	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorChandeMomentumOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorChandeMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{




	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorCommodityChannel(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorCommodityChannelIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorCommodityChannel(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorComparativeRSI(LPCTSTR Key, LONG panel, LPCTSTR Source1, LPCTSTR Source2, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorComparativeRelativeStrength(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source1; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Source2; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorComparativeRSI(LPCTSTR Key, LONG panel, LPCTSTR Source1, LONG Index, LPCTSTR Source2, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source1; 
	panels[panel]->series[index]->paramStr[1] = Source2; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorDetrendedPrice(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorDetrendedPriceOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorDetrendedPrice(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorDirectMoveSystem(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2, LONG ColorR3, LONG ColorG3, LONG ColorB3, LONG Style3, LONG Thickness3)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorDirectionalMovementSystem(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = ColorR3; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = ColorG3;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[4] = ColorB3;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[5] = Style3;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[6] = Thickness3;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	//Twin Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style2;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness2;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorDirectMoveSystem(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2, LONG ColorR3, LONG ColorG3, LONG ColorB3, LONG Style3, LONG Thickness3)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = ColorR3; 
	panels[panel]->series[index]->paramInt[3] = ColorG3; 
	panels[panel]->series[index]->paramInt[4] = ColorB3; 
	panels[panel]->series[index]->paramInt[5] = Style3; 
	panels[panel]->series[index]->paramInt[6] = Thickness3; 	
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyleSignal = Style;
	panels[panel]->series[index]->lineWeightSignal = Thickness;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorEasyMovement(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{

	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;

	panels[panel]->series.push_back(new CIndicatorEaseOfMovement(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorEasyMovement(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LPCTSTR Volume, LONG Periods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;


	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->paramInt[2] = Periods; 
	panels[panel]->series[index]->paramInt[3] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorEMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorExponentialMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorEMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorFractalChaos(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorFractalChaosBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorFractalChaos(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorFractalChaosOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorFractalChaosOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorFractalChaosOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorHighMinusLow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorHighMinusLow(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorHighMinusLow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorHighLowBands(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorHighLowBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorHighLowBands(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorHistoricalVolatility(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG BarHistory, DOUBLE StandardDev, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
#ifdef _CONSOLE_DEBUG
	printf("AddIndicatorHistoricalVolatility() Stand=%f", StandardDev);
#endif
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorHistoricalVolatility(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = BarHistory; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[3] = StandardDev; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorHistoricalVolatility(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG BarHistory, DOUBLE StandardDev, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->paramInt[2] = BarHistory;
	panels[panel]->series[index]->paramDbl[3] = StandardDev;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorLinearRegForecast(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorLinearRegressionForecast(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorLinearRegForecast(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorLinearRegIntercept(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorLinearRegressionIntercept(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorLinearRegIntercept(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorLinearRegRSquare(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorLinearRegressionRSquared(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorLinearRegRSquare(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorLinearRegSlope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorLinearRegressionSlope(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorLinearRegSlope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Index, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMACDHistogram(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG LongCycle, LONG ShortCycle, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMACDHistogram(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = LongCycle;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = ShortCycle;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "MACD-H";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+ShortCycle+","+LongCycle+","+Periods+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMACDHistogram(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG LongCycle, LONG ShortCycle, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = LongCycle;
	panels[panel]->series[index]->paramInt[2] = ShortCycle;
	panels[panel]->series[index]->paramInt[3] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->szTitle = "MACD-H";
	panels[panel]->series[index]->szTitle += CString("("+ShortCycle+","+LongCycle+","+Periods+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMACD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, FLOAT Scale, LONG ShortCycle, LONG LongCycle, LONG ColorR1, LONG ColorG1, LONG ColorB1, LONG Style1, LONG Thickness1, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMACD(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters for Twin
	((CIndicatorMACD *) panels[panel]->series[panels[panel]->series.size() - 1])->lineColorSignal = RGB(ColorR1,ColorG1,ColorB1);
	((CIndicatorMACD *) panels[panel]->series[panels[panel]->series.size() - 1])->lineStyleSignal = Style2;
	((CIndicatorMACD *) panels[panel]->series[panels[panel]->series.size() - 1])->lineWeightSignal = Thickness2;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = LongCycle;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = ShortCycle;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style1;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness1;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "MACD";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+ShortCycle+","+LongCycle+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMACD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Index, LONG Periods, LONG ShortCycle, LONG LongCycle, LONG ColorR1, LONG ColorG1, LONG ColorB1, LONG Style1, LONG Thickness1, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = LongCycle;
	panels[panel]->series[index]->paramInt[2] = ShortCycle;
	panels[panel]->series[index]->paramInt[3] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyle = Style1;
	panels[panel]->series[index]->lineWeight = Thickness1;	
	panels[panel]->series[index]->szTitle = "MACD";
	panels[panel]->series[index]->szTitle += CString("("+ShortCycle+","+LongCycle+")");
	
	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR1,ColorG1,ColorB1);
	panels[panel]->series[index]->lineStyleSignal = Style2;
	panels[panel]->series[index]->lineWeightSignal = Thickness2;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR1,ColorG1,ColorB1);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;


	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMassIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMassIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMassIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMedian(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMedian(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMedian(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMomentum(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMomentum(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMoneyFlow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMoneyFlowIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "MFI";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMoneyFlow(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->paramInt[2] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorMAEnvelope(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG MAType, DOUBLE Shift, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorMovingAverageEnvelope(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[3] = Shift; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "MAE";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+","+Shift+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorMAEnvelope(LPCTSTR Key, LONG panel, LPCSTR Source, LONG Periods, LONG MAType, DOUBLE Shift, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = MAType; 
	panels[panel]->series[index]->paramDbl[3] = Shift; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[index]->szTitle = "MAE";
	panels[panel]->series[index]->szTitle += CString("("+Periods+","+Shift+")");

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorNegVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorNegativeVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorNegVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorBalanceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorOnBalanceVolume(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorBalanceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorParabolicSAR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE MinAF, DOUBLE MaxAF, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorParabolicSAR(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[1] = MinAF; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = MaxAF; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "PSAR";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+MinAF+","+MaxAF+")");
	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorParabolicSAR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE MinAF, DOUBLE MaxAF, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramDbl[1] = MinAF; 
	panels[panel]->series[index]->paramDbl[2] = MaxAF; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "PSAR";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+MinAF+","+MaxAF+")");
	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorPerformance(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPerformanceIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPerformance(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::AddIndicatorPositiveVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPositiveVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPositiveVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramStr[1] = Volume;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorPriceOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG LongCycle, LONG ShortCycle, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPriceOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = LongCycle; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = ShortCycle; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPriceOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG LongCycle, LONG ShortCycle, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramInt[1] = LongCycle;
	panels[panel]->series[index]->paramInt[2] = ShortCycle;
	panels[panel]->series[index]->paramInt[3] = MAType;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorPriceROC(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPriceROC(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPriceROC(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorPriceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPriceVolumeTrend(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPriceVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramStr[1] = Volume;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorPNumberBands(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPrimeNumberBands(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; //Symbol
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPNumberBands(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; //Source ou Symbol??
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorPNumberOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorPrimeNumberOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorPNumberOscillator(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorRainbowOsc(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Levels, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorRainbowOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Levels; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorRainbowOsc(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Levels, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Levels; 
	panels[panel]->series[index]->paramInt[2] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorRelativeStrenght(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, DOUBLE Threshold1, DOUBLE Threshold2, LONG TColorR, LONG TColorG, LONG TColorB, LONG TStyle, LONG TThickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorRelativeStrengthIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = Threshold1; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[3] = Threshold2; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(TColorR,TColorG,TColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = TStyle;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = TThickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "RSI";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+")");





	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorRelativeStrenght(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, DOUBLE Threshold1, DOUBLE Threshold2, LONG TColorR, LONG TColorG, LONG TColorB, LONG TStyle, LONG TThickness)
{
	int index, index30, index70;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
				if(CompareNoCase(panels[n]->series[j]->szName, CString(Key)+" 30")){
					index30 = j;
				}
				if(CompareNoCase(panels[n]->series[j]->szName, CString(Key)+" 70")){
					index70 = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramDbl[2] = Threshold1; 
	panels[panel]->series[index]->paramDbl[3] = Threshold2; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[index]->lineColorSignal = RGB(TColorR,TColorG,TColorB);
	panels[panel]->series[index]->lineStyleSignal = TStyle;
	panels[panel]->series[index]->lineWeightSignal = TThickness;
	panels[panel]->series[index]->szTitle = "RSI";
	panels[panel]->series[index]->szTitle += CString("("+Periods+")");
	
	//Edit Twins	
	panels[panel]->series[index30]->lineColor = RGB(TColorR,TColorG,TColorB);
	panels[panel]->series[index30]->lineStyle = TStyle;
	panels[panel]->series[index30]->lineWeight = TThickness;
	panels[panel]->series[index70]->lineColor = RGB(TColorR,TColorG,TColorB);
	panels[panel]->series[index70]->lineStyle = TStyle;
	panels[panel]->series[index70]->lineWeight = TThickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorStandardDev(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandarDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorStandardDeviation(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = StandarDev; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorStandardDev(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE StandardDev, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramDbl[2] = StandardDev; 
	panels[panel]->series[index]->paramInt[3] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorStocMomentum(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Smooth, LONG DblSmooth, LONG DPeriods, LONG MAType, LONG DMAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorStochasticMomentumIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters for Twin
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style2;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness2;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Smooth; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = DblSmooth; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[4] = DPeriods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[5] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[6] = DMAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "SMI";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+","+DPeriods+")");

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorStocMomentum(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Smooth, LONG DblSmooth, LONG DPeriods, LONG MAType, LONG DMAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = Smooth; 
	panels[panel]->series[index]->paramInt[3] = DblSmooth; 
	panels[panel]->series[index]->paramInt[4] = DPeriods; 
	panels[panel]->series[index]->paramInt[5] = MAType; 
	panels[panel]->series[index]->paramInt[6] = DMAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyleSignal = Style2;
	panels[panel]->series[index]->lineWeightSignal = Thickness2;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorStocOscillator(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Slowing, LONG DPeriods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}







	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorStochasticOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	
	//Parameters for Twin
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyleSignal = Style2;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeightSignal = Thickness2;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Slowing; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = DPeriods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[4] = MAType; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "SO";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("("+Periods+","+DPeriods+")");


	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorStocOscillator(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG Slowing, LONG DPeriods, LONG MAType, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness, LONG ColorR2, LONG ColorG2, LONG ColorB2, LONG Style2, LONG Thickness2)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramInt[2] = Slowing; 
	panels[panel]->series[index]->paramInt[3] = DPeriods; 
	panels[panel]->series[index]->paramInt[4] = MAType; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	//Parameters for Twin
	panels[panel]->series[index]->lineColorSignal = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index]->lineStyleSignal = Style2;
	panels[panel]->series[index]->lineWeightSignal = Thickness2;
	
	//Edit Twin	
	panels[panel]->series[index+1]->lineColor = RGB(ColorR2,ColorG2,ColorB2);
	panels[panel]->series[index+1]->lineStyle = Style2;
	panels[panel]->series[index+1]->lineWeight = Thickness2;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorSwingIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorSwingIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[1] = LimitMove; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorSwingIndex(LPCTSTR Key, LONG panel, LPCTSTR Symbol, DOUBLE LimitMove, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramDbl[1] = LimitMove; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorTimeSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTimeSeriesMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTimeSMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}




VARIANT_BOOL CStockChartXCtrl::AddIndicatorTradeVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, DOUBLE MinTick, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTradeVolumeIndex(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[1] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = MinTick; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTradeVolume(LPCTSTR Key, LONG panel, LPCTSTR Source, LPCTSTR Volume, DOUBLE MinTick, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramStr[1] = Volume; 
	panels[panel]->series[index]->paramDbl[1] = MinTick; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorTriangularMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTriangularMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTriangularMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorTRIX(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTRIX(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTRIX(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorTrueRange(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTrueRange(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTrueRange(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorTypicalPrice(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorTypicalPrice(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "TP";

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorTypicalPrice(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorUltimateOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Cycle1, LONG Cycle2, LONG Cycle3, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorUltimateOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Cycle1; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Cycle2; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = Cycle3; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorUltimateOsc(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Cycle1, LONG Cycle2, LONG Cycle3, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Cycle1; 
	panels[panel]->series[index]->paramInt[2] = Cycle2; 
	panels[panel]->series[index]->paramInt[3] = Cycle3; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVariableMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVariableMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVariableMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVertiHoriFilter(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVerticalHorizontalFilter(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVertiHoriFilter(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVIDYA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVIDYA(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[2] = R2Scale; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVIDYA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->paramDbl[2] = R2Scale; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVolumeOsc(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ShortTerm, LONG LongTerm, LONG PointPercent, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVolumeOscillator(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = ShortTerm; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = LongTerm;  
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = PointPercent; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVolumeOsc(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ShortTerm, LONG LongTerm, LONG PointPercent, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Volume; 
	panels[panel]->series[index]->paramInt[1] = ShortTerm; 
	panels[panel]->series[index]->paramInt[2] = LongTerm; 
	panels[panel]->series[index]->paramInt[3] = PointPercent; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVolumeROC(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVolumeROC(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Volume; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVolumeROC(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Volume; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	/*
	//int nCnt = AddChartPanel();

	// Whatever the panel index is now, add the series to it
	panels[panel]->Connect(this);
	
	//AddNewSeriesType(nCnt,stVolumeChart,Key);
	panels[panel]->series.push_back(new CSeriesStandard(Key, stVolumeChart, 1, panels[panel]));		
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);	
	for(int index=0;index<panels[0]->series[4]->data_master.size();index++)
	{
		panels[panel]->series[panels[panel]->series.size() - 1]->AppendValue(panels[0]->series[4]->GetJDate(index),panels[0]->series[4]->GetValue(index));
	}
	panels[panel]->OnUpdate();
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = 3;
	
	//reCalc = true;
	changed = true;
	
	*/
	
	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorVolume(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;
	/*
	//Copy Volume Data:
	panels[panel]->series[panels[panel]->series.size() - 1]->data_master.clear();
	panels[panel]->series[panels[panel]->series.size() - 1]->data_slave.clear();
	for(int index=0;index<panels[0]->series[4]->data_master.size();index++)
	{
		panels[panel]->series[panels[panel]->series.size() - 1]->AppendValue(panels[0]->series[4]->GetJDate(index),panels[0]->series[4]->GetValue(index));
	}
	*/
	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Volume; 
	//panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = 1; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "VOL";
	//panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	//panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	
	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorVolume(LPCTSTR Key, LONG panel, LPCTSTR Volume, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Volume; 
	//panels[panel]->series[index]->paramInt[1] = 1; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->szTitle = "VOL";
	//panels[panel]->series[index]->lineStyle = Style;
	//panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorWeightedClose(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorWeightedClose(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorWeightedCLose(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorWeightedMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorWeightedMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorWeightedMA(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorWellesWilder(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorWellesWilderSmoothing(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorWellesWilder(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorWilliamAD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorWilliamsAccumulationDistribution(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorWilliamAD(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::AddIndicatorWilliamPCTR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorWilliamsPctR(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Symbol; 
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods; 
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;

	return TRUE;
}

VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorWilliamPCTR(LPCTSTR Key, LONG panel, LPCTSTR Symbol, LONG Periods, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
		}
	reCalc = true;
	changed = true;

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Symbol; 
	panels[panel]->series[index]->paramInt[1] = Periods; 
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;

	return TRUE;
}

// New Indicators Methods

LONG CStockChartXCtrl::GetSeriesPeriods(LPCTSTR Name, LPCTSTR Code)
{
	long ret = 0;
	if((CompareNoCase(Code,"CMF"))||(CompareNoCase(Code,"EOM"))||(CompareNoCase(Code,"HILO Low"))||(CompareNoCase(Code,"MFI")))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[2];
					}
				}	
			}
		}
	}
	else if((CompareNoCase(Code,"MACD"))||(CompareNoCase(Code,"MACD-H")))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[3];
					}
				}	
			}
		}
	}
	else
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[1];
					}
				}	
			}
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesPeriods(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	/*
	if((CompareNoCase(Code,"SMA"))||(CompareNoCase(Code,"Aroon"))||(CompareNoCase(Code,"BB")))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						panels[n]->series[j]->paramInt[1] = newVal;
					}
				}	
			}
		}
	}
	if(CompareNoCase(Code,"MACD"))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						panels[n]->series[j]->paramInt[3] = newVal;
					}
				}	
			}
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();*/
}


BSTR CStockChartXCtrl::GetSeriesSource(LPCTSTR Name, LPCTSTR Code)
{
	CString ret;
	/*if(((CString)Code).Compare("VOL")>-1) {
		ret="symbol.volume";
		printf("\n\nGetSource Volume!");
	}
	else{*/
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramStr[0];
					}
				}	
			}
		}
	//}
	return ret.AllocSysString();
}


void CStockChartXCtrl::SetSeriesSource(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramStr[0] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

LONG CStockChartXCtrl::GetSeriesShortCycle(LPCTSTR Name, LPCTSTR Code)
{
	LONG ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesShortCycle(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramInt[2] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesLongCycle(LPCTSTR Name, LPCTSTR Code)
{
	long ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[1];
				}
			}	
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesLongCycle(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramInt[1] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}


BSTR CStockChartXCtrl::GetSeriesSymbol(LPCTSTR Name, LPCTSTR Code)
{
	CString ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramStr[0];
				}
			}	
		}
	}
	return ret.AllocSysString();
}


void CStockChartXCtrl::SetSeriesSymbol(LPCTSTR Name, LPCTSTR Code, BSTR newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramStr[0] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}

DOUBLE CStockChartXCtrl::GetSeriesLimitMove(LPCTSTR Name, LPCTSTR Code)
{
	double ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[1];
				}
			}	
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesLimitMove(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramDbl[1] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}


DOUBLE CStockChartXCtrl::GetSeriesStandarDev(LPCTSTR Name, LPCTSTR Code)
{
	double ret;
	if(CompareNoCase(Code,"HV"))
	{
		for(int n = 0; n != panels.size(); ++n)
		{
			if(panels[n]->visible)
			{
				for(int j = 0; j != panels[n]->series.size(); ++j)
				{
					if(CompareNoCase(Name, panels[n]->series[j]->szName))
					{
						ret = panels[n]->series[j]->paramDbl[3];
					}
				}	
			}
		}
	}
	else
	{
		for(int n = 0; n != panels.size(); ++n)
		{
			if(panels[n]->visible)
			{
				for(int j = 0; j != panels[n]->series.size(); ++j)
				{
					if(CompareNoCase(Name, panels[n]->series[j]->szName))
					{
						ret = panels[n]->series[j]->paramDbl[2];
					}
				}	
			}
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesStandarDev(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					panels[n]->series[j]->paramDbl[2] = newVal;
				}
			}	
		}
	}
	UpdateScreen(true);
	changed = true;
	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesMAType(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	if ((CompareNoCase(Code, "DPO")) || (CompareNoCase(Code, "MAE")) || (CompareNoCase(Code, "RO")) || (CompareNoCase(Code, "HV")))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[2];
					}
				}	
			}
		}
	}
	else if((CompareNoCase(Code,"BB"))||(CompareNoCase(Code,"CV"))||(CompareNoCase(Code,"EOM"))||(CompareNoCase(Code,"PO"))||(CompareNoCase(Code,"SD"))||(CompareNoCase(Code,"MA")))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[3];
					}
				}	
			}
		}
	}
	else if(CompareNoCase(Code,"SO"))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[4];
					}
				}	
			}
		}
	}
	else if(CompareNoCase(Code,"SMI"))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[5];
					}
				}	
			}
		}
	}
	else
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[1];
					}
				}	
			}
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesMAType(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{

}


BSTR CStockChartXCtrl::GetSeriesVolumeSource(LPCTSTR Name, LPCTSTR Code)
{
	CString ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramStr[1];
				}
			}	
		}
	}
	return ret.AllocSysString();
}


void CStockChartXCtrl::SetSeriesVolumeSource(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal)
{

}


LONG CStockChartXCtrl::GetSeriesRateChange(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	if(CompareNoCase(Code,"CV"))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[2];
					}
				}	
			}
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesRateChange(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


BSTR CStockChartXCtrl::GetSeriesSource2(LPCTSTR Name, LPCTSTR Code)
{
	CString ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramStr[1];
				}
			}	
		}
	}
	return ret.AllocSysString();
}


void CStockChartXCtrl::SetSeriesSource2(LPCTSTR Name, LPCTSTR Code, LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesBarHistory(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesBarHistory(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


DOUBLE CStockChartXCtrl::GetSeriesShift(LPCTSTR Name, LPCTSTR Code)
{
	DOUBLE ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[3];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesShift(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


DOUBLE CStockChartXCtrl::GetSeriesMinAF(LPCTSTR Name, LPCTSTR Code)
{
	DOUBLE ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[1];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesMinAF(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


DOUBLE CStockChartXCtrl::GetMaxAF(LPCTSTR Name, LPCTSTR Code)
{
	DOUBLE ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetMaxAF(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesLevel(LPCTSTR Name, LPCTSTR Code)
{
	LONG ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[1];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesLevel(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}





DOUBLE CStockChartXCtrl::GetSeriesMinTick(LPCTSTR Name, LPCTSTR Code)
{
	double ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesMinTick(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesCycle(LPCTSTR Name, LPCTSTR Code, LONG Index)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					if(Index==1) ret = panels[n]->series[j]->paramInt[1];
					else if(Index==2) ret = panels[n]->series[j]->paramInt[2];
					else if(Index==3) ret = panels[n]->series[j]->paramInt[3];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesCycle(LPCTSTR Name, LPCTSTR Code, LONG Index, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


DOUBLE CStockChartXCtrl::GetSeriesR2Scale(LPCTSTR Name, LPCTSTR Code)
{
	double ret;
	if(CompareNoCase(Code,"MA")||CompareNoCase(Code,"HILO")){
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramDbl[4];




						return ret;
					}
				}	
			}
		}
	}
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramDbl[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesR2Scale(LPCTSTR Name, LPCTSTR Code, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesShortTPeriod(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[1];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesShortTPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesLongTPeriod(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesLongTPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesPointPercent(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[3];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesPointPercent(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesKPeriod(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[1];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesKPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesKSmooth(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesKSmooth(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesPctKDblSmooth(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[3];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesPctKDblSmooth(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesDPeriod(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	if(CompareNoCase(Code,"SO"))
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[3];
					}
				}	
			}
		}
	}
	else
	{
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						ret = panels[n]->series[j]->paramInt[4];
					}
				}	
			}
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesDPeriod(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesDMAType(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[6];
				}
			}	
		}
	}
	
	return ret;
}


void CStockChartXCtrl::SetSeriesDMAType(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


LONG CStockChartXCtrl::GetSeriesKSlowing(LPCTSTR Name, LPCTSTR Code)
{
	int ret;
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					ret = panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	return ret;
}


void CStockChartXCtrl::SetSeriesKSlowing(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}

void CStockChartXCtrl::UpdateTrendLine(LPCTSTR Key, LONG Style, LONG Thickness, LONG ColorR, LONG ColorG, LONG ColorB, VARIANT_BOOL RadExtension, VARIANT_BOOL RightExtension, VARIANT_BOOL LeftExtension, DOUBLE Value)
{
#ifdef _CONSOLE_DEBUG
	printf("\nUpdateTrendLine(value=%f)", Value);
#endif
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					panels[n]->lines[j]->lineStyle = Style;
					panels[n]->lines[j]->lineWeight = Thickness;
					panels[n]->lines[j]->lineColor = RGB(ColorR,ColorG,ColorB);
					panels[n]->lines[j]->radiusExtension = RadExtension;
				    panels[n]->lines[j]->rightExtension = RightExtension;
					panels[n]->lines[j]->leftExtension = LeftExtension;
					panels[n]->lines[j]->valuePosition = Value;

					break;
				}
			}
		}
	}
	UpdateScreen(true);
}

DOUBLE CStockChartXCtrl::GetTrendLineValue(LPCTSTR Key)
{
	double ret = 0;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					ret = panels[n]->lines[j]->valuePosition;
					break;
				}
			}
		}
	}
	return ret;
}

LONG CStockChartXCtrl::GetTrendLineStyle(LPCTSTR Key)
{
	int ret = 0;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					ret = panels[n]->lines[j]->lineStyle;
					break;
				}
			}
		}
	}
	return ret;
}

LONG CStockChartXCtrl::GetTrendLineThickness(LPCTSTR Key)
{
	int ret = 1;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					ret = panels[n]->lines[j]->lineWeight;
					break;
				}
			}
		}
	}
	return ret;
}

LONG CStockChartXCtrl::GetTrendLineColor(LPCTSTR Key)
{
	int ret = RGB(0,0,0);
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					ret = panels[n]->lines[j]->lineColor;
					break;
				}
			}
		}
	}
	return ret;
}

VARIANT_BOOL CStockChartXCtrl::GetTrendLineRightExtension(LPCTSTR Key)
{
	bool ret = false;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
				    ret = panels[n]->lines[j]->rightExtension;
					break;
				}
			}
		}
	}
	return ret;
}

VARIANT_BOOL CStockChartXCtrl::GetTrendLineLeftExtension(LPCTSTR Key)
{
	bool ret = false;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					ret = panels[n]->lines[j]->leftExtension;
					break;
				}
			}
		}
	}
	return ret;
}

// //Reset all state machines used in drawings
void CStockChartXCtrl::StopDrawing(void)
{
	
#ifdef _CONSOLE_DEBUG
			printf("\nStopDraw()");
#endif
	objectSelected = false;
	drawing = false;
	lineDrawing = false;
	userZooming = false;
	startUserZooming = -1;
	endUserZooming = -1;
	//typing = false;
	//resizing = false;
	//dragging = false;
	m_mouseState = MOUSE_NORMAL;
	m_Cursor = 0;	
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible && panels[n]->lines.size()>0){
			for(int m = 0; m != panels[n]->lines.size(); ++m){
				panels[n]->lines[m]->state = 0;
				panels[n]->lines[m]->drawn = true;
				panels[n]->lines[m]->drawing = false;
			}				
		}
	}
}


DOUBLE CStockChartXCtrl::GetFibonacciParameter(LPCTSTR Key, LONG Index)
{
	double param=-1;
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					param = panels[n]->lines[j]->params[Index];
					break;
				}
			}
		}
	}
	return param;
}


void CStockChartXCtrl::UpdateFibonacciParams(LPCTSTR Key, DOUBLE param1, DOUBLE param2, DOUBLE param3, DOUBLE param4, DOUBLE param5, DOUBLE param6, DOUBLE param7, DOUBLE param8, DOUBLE param9, DOUBLE param10)
{
	for(int n = 0; n != panels.size(); ++n){		
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->lines.size(); ++j){
				if(panels[n]->lines[j]->key == Key){
					panels[n]->lines[j]->params[0] = param1;
					panels[n]->lines[j]->params[1] = param2;
					panels[n]->lines[j]->params[2] = param3;
					panels[n]->lines[j]->params[3] = param4;
					panels[n]->lines[j]->params[4] = param5;
					panels[n]->lines[j]->params[5] = param6;
					panels[n]->lines[j]->params[6] = param7;
					break;
				}
			}
		}
	}
}



void CStockChartXCtrl::OnPeriodicityChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}

// Returns all Text Objects for all panels
int CStockChartXCtrl::GetTextAreasCount(void)
{
	int count = 0;
	for(int k=0;k<panels.size();++k){
		if(panels[k]->visible) count+=panels[k]->textAreas.size();
	}
	return count;
}

void CStockChartXCtrl::OnStudyDirectoryChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}

// Calculate contrast color
OLE_COLOR CStockChartXCtrl::GetContrastColor(OLE_COLOR color)

{
	DWORD ole = (DWORD)color;//OleTranslateColor(color,GetSystemPalette(),NULL);
	BYTE r = ((BYTE)(ole));
	BYTE g = ((BYTE)(((WORD)(ole)) >> 8));
	BYTE b = ((BYTE)((ole)>>16));
	double bright = ((double)r*299 + (double)g*587 + (double)b*114)/1000;
	if(bright>128) return RGB(0,0,0);
	else return RGB(255,255,255);
	//return RGB(r,g,b);
}


void CStockChartXCtrl::OnLanguageChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}



VARIANT_BOOL CStockChartXCtrl::AddIndicatorGenericMovingAverage(LPCTSTR Key, LONG panel, LPCTSTR Source, LONG Periods, LONG Shift, LONG MAType, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	
	if(!loading){
		for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					ThrowErr(1355, "Key not unique");
					return FALSE;
				}
			}
		}
	}

	bool valid = false;
	bool userParams = false;
	panels[panel]->series.push_back(new CIndicatorGenericMovingAverage(Key, OBJECT_SERIES_INDICATOR, 1, panels[panel]));
	panels[panel]->series[panels[panel]->series.size() - 1]->Connect(this);
	panels[panel]->series[panels[panel]->series.size() - 1]->userParams = userParams;
	reCalc = true;
	changed = true;

	CString ma_type;
	switch(MAType){
		case (int)indExponentialMovingAverage:
			ma_type="E";
			break;
		case (int)indTimeSeriesMovingAverage:
			ma_type="TS";
			break;
		case (int)indVariableMovingAverage:
			ma_type="V";
			break;
		case (int)indTriangularMovingAverage:
			ma_type="T";
			break;
		case (int)indWeightedMovingAverage:
			ma_type="W";
			break;
		case (int)indVIDYA:
			ma_type="VI";
			break;
		case (int)indWellesWilderSmoothing:
			ma_type="WW";
			break;
		default:
			ma_type="S";
			break;
	}

	//Parameters:
	panels[panel]->series[panels[panel]->series.size() - 1]->paramStr[0] = Source;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[1] = Periods;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[2] = Shift;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3] = MAType;
	panels[panel]->series[panels[panel]->series.size() - 1]->paramDbl[4] = R2Scale;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[panels[panel]->series.size() - 1]->lineStyle = Style;
	panels[panel]->series[panels[panel]->series.size() - 1]->lineWeight = Thickness;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle = "MA";
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString("(");
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += ma_type;
	panels[panel]->series[panels[panel]->series.size() - 1]->szTitle += CString(","+Periods+","+Shift+")");
	
	//printf("\nADDED MAtype = %d param = %d",(int)MAType,panels[panel]->series[panels[panel]->series.size() - 1]->paramInt[3]);
	
	return TRUE;
}


VARIANT_BOOL CStockChartXCtrl::UpdateIndicatorGenericMovingAverage(LPCTSTR Key, LONG panel, LPCSTR Source, LONG Periods, LONG Shift, LONG MAType, DOUBLE R2Scale, LONG ColorR, LONG ColorG, LONG ColorB, LONG Style, LONG Thickness)
{
	int index;
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, Key)){
					index = j;
				}
			}
	}
	reCalc = true;
	changed = true;
	
	CString ma_type;
	switch(MAType){
		case (int)indExponentialMovingAverage:
			ma_type="E";
			break;
		case (int)indTimeSeriesMovingAverage:
			ma_type="TS";
			break;
		case (int)indVariableMovingAverage:
			ma_type="V";
			break;
		case (int)indTriangularMovingAverage:
			ma_type="T";
			break;
		case (int)indWeightedMovingAverage:
			ma_type="W";
			break;
		case (int)indVIDYA:
			ma_type="VI";
			break;
		case (int)indWellesWilderSmoothing:
			ma_type="WW";
			break;
		default:
			ma_type="S";
			break;
	}

	//Parameters:
	panels[panel]->series[index]->paramStr[0] = Source;
	panels[panel]->series[index]->paramInt[1] = Periods;
	panels[panel]->series[index]->paramInt[2] = Shift;
	panels[panel]->series[index]->paramInt[3] = MAType;
	panels[panel]->series[index]->paramDbl[4] = R2Scale;
	panels[panel]->series[index]->lineColor = RGB(ColorR,ColorG,ColorB);
	panels[panel]->series[index]->lineStyle = Style;
	panels[panel]->series[index]->lineWeight = Thickness;
	panels[panel]->series[index]->szTitle = "MA";
	panels[panel]->series[index]->szTitle += CString("(");
	panels[panel]->series[index]->szTitle += ma_type;
	panels[panel]->series[index]->szTitle += CString(","+Periods+","+Shift+")");

	return TRUE;
}


LONG CStockChartXCtrl::GetSeriesShiftInt(LPCTSTR Name, LPCTSTR Code)
{
	if(CompareNoCase(Code,"HILO")){
		for(int n = 0; n != panels.size(); ++n){
			if(panels[n]->visible){
				for(int j = 0; j != panels[n]->series.size(); ++j){
					if(CompareNoCase(Name, panels[n]->series[j]->szName)){
						return panels[n]->series[j]->paramInt[3];
					}
				}	
			}
		}
	}
	for(int n = 0; n != panels.size(); ++n){
		if(panels[n]->visible){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(Name, panels[n]->series[j]->szName)){
					return panels[n]->series[j]->paramInt[2];
				}
			}	
		}
	}
	return NULL_VALUE;
}


void CStockChartXCtrl::SetSeriesShiftInt(LPCTSTR Name, LPCTSTR Code, LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


BSTR CStockChartXCtrl::GetApplicationDirectory(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	CString strResult;

	strResult = m_ApplicationDirectory;

	return strResult.AllocSysString();
}


void CStockChartXCtrl::SetApplicationDirectory(LPCTSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_ApplicationDirectory = newVal;

	SetModifiedFlag();
}


void CStockChartXCtrl::OnDeltaCursorChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	SetModifiedFlag();

	if(m_DeltaCursor) {
		DeltaCursor=true;
	}
	else{
		DeltaCursor=false;
	}
	m_pauseMeasure = true;






}




LONG CStockChartXCtrl::GetBarSize(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	return m_BarSize;
}


void CStockChartXCtrl::SetBarSize(LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	m_BarSize = newVal;

	SetModifiedFlag();
}


void CStockChartXCtrl::OnMousePositionXChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


void CStockChartXCtrl::OnMousePositionYChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}

long CStockChartXCtrl::GetMousePositionY(){
	return m_MousePositionY;
}


void CStockChartXCtrl::OnSmoothHeikinTypeChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	SmoothHeikinType = m_SmoothHeikinType;

	SetModifiedFlag();
}


void CStockChartXCtrl::OnSmoothHeikinPeriodsChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	SmoothHeikinPeriods = m_SmoothHeikinPeriods;

	SetModifiedFlag();
}










void CStockChartXCtrl::AbortDrawing(void)
{
#ifdef _CONSOLE_DEBUG
	printf("\nAbortDrawing()");
#endif
	StopDrawing();
}

long CStockChartXCtrl::GetPriceLineThickness()
{
	return m_PriceLineThickness;
}

long CStockChartXCtrl::GetPriceLineThicknessBar()
{
	return m_PriceLineThicknessBar;
}

bool CStockChartXCtrl::GetPriceLineMono()
{
	return m_PriceLineMono;
}

void CStockChartXCtrl::OnPriceLineThicknessChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


void CStockChartXCtrl::OnPriceLineMonoChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


void CStockChartXCtrl::OnPriceLineThicknessBarChanged(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


void CStockChartXCtrl::MovePanelIndex(LONG panel, LONG offset, VARIANT_BOOL moveUp)
{
#ifdef _CONSOLE_DEBUG
	//printf("\nMovePanelIndex()\n\tPanel=%d\n\tHigh=%d\n\tLow=%d",panel,GetNextHigherChartPanel(panels[panel]->y1),GetNextLowerChartPanel(panels[panel]->y2));
#endif
	//printf("\ny1+=%f y2+=%f\ny1=%f y2=%f\ny1-=%f y2-=%f",panels[panel-1]->y1,panels[panel-1]->y2,panels[panel]->y1,panels[panel]->y2,panels[panel+1]->y1,panels[panel+1]->y2);
	//Protect code:
	if(panel == 0 || (GetNextHigherChartPanel(panels[panel]->y1)==0&&moveUp) || (GetNextLowerChartPanel(panels[panel]->y1-1)==-1 && !moveUp)) {
#ifdef _CONSOLE_DEBUG
	//printf("\n\nRETURN ");
	//if(panel==0)printf(" panel=0");
	//if((GetNextHigherChartPanel(panels[panel]->y1)==0&&moveUp))printf(" NextHigh=0");
	//if((GetNextLowerChartPanel(panels[panel]->y1)==-1 && !moveUp))printf(" NextLow=-1");
#endif
		return;
	}
	//Move Up:
	if(moveUp)
	{
		for(int i=0;i<offset;i++)
		{
			int next = GetNextHigherChartPanel(panels[panel]->y1);
			//switch coordinates:
			panels[next]->y2 = panels[panel]->y2;
			panels[panel]->y2 = panels[next]->y1+(panels[panel]->y2-panels[panel]->y1);
			panels[panel]->y1 = panels[next]->y1;
			panels[next]->y1 = panels[panel]->y2;

			panels[panel]->index--;
			panels[next]->index++;

			std::iter_swap(panels.begin()+panel,panels.begin()+next);

			panel--;
			
		}
	}
	//Move Down:
	else
	{
		for(int i=0;i<offset;i++)
		{
			int next = GetNextLowerChartPanel(panels[panel]->y1);
			//switch coordinates:
			
			panels[next]->y1 = panels[panel]->y1;
			panels[panel]->y1 = panels[next]->y2 - (panels[panel]->y2 - panels[panel]->y1);
			panels[panel]->y2 = panels[next]->y2;
			panels[next]->y2 = panels[panel]->y1;
			
			panels[panel]->index++;
			panels[next]->index--;

			std::iter_swap(panels.begin()+panel,panels.begin()+next);

			//printf("\nNEXT=%d \ny1+=%f y2+=%f\ny1=%f y2=%f\ny1-=%f y2-=%f",next,panels[panel-1]->y1,panels[panel-1]->y2,panels[panel]->y1,panels[panel]->y2,panels[panel+1]->y1,panels[panel+1]->y2);
			
			panel++;
		}
	}
	Update();


	//Re-order all panels								
	/*int offset = panels[n]->y2 - panels[n]->y1;
	int a = (int)GetNextHigherChartPanel((int)panels[n]->y1);
	while(a > 0){
		panels[a]->y2 += offset;
		panels[a]->y1 += offset; 
		a = (int)GetNextHigherChartPanel((int)panels[a]->y1);

		
	std::vector<CChartPanel*>::iterator itr = panels.begin() + index;
	panels.erase(itr);


	}*/
}





DOUBLE CStockChartXCtrl::GetSeriesThreshold(LPCTSTR name, LONG index)
{
	for(int n = 0; n != panels.size(); ++n){
			for(int j = 0; j != panels[n]->series.size(); ++j){
				if(CompareNoCase(panels[n]->series[j]->szName, name)){
#ifdef _CONSOLE_DEBUG
					//printf("\nGetSeriesThreshold(%s,%d)",panels[n]->series[j]->szName,index);
#endif
					return panels[n]->series[j]->paramDbl[index]; 
				}
			}
		}

	return 0;
	
}


void CStockChartXCtrl::SetSeriesThreshold(LPCTSTR name, LONG index, DOUBLE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}


void CStockChartXCtrl::UnSelect()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	UnSelectAll();
	UpdateScreen(false);
	RePaint();
	// TODO: Add your dispatch handler code here
}


void CStockChartXCtrl::OnSaveImageTitleChanged()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your property handler code here

	SetModifiedFlag();
}

CString CStockChartXCtrl::SaveImageTitle()
{
	return m_SaveImageTitle;

}
