#include "stdafx.h"

/*
// DIRECT2D
// Windows Header Files:
#include <windows.h>
#include <intsafe.h>

// C RunTime Header Files
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>

#include <d2d1.h>
#include <d2d1helper.h>
#include <dwrite.h>
#include <wincodec.h>
#include <string.h>
#include "PointF.h"
#include "RectF.h"
*/
//GDI+ Example:
/*
	To add those references you need to configurate Project Properties:
	- Set C\C++>General>Common Language Runtime Support: Common Language Runtime Support(/clr)
	- Set C\C++>Code Generation>Multi-threaded DLL(/MD)
	- Define _AFXDLL in StdAfx.h
*/

// GDI+
#using <mscorlib.dll>
#using <System.dll>
#using <System.Drawing.DLL>
#using <System.Windows.Forms.dll>

using namespace System;
using namespace System::Drawing;
using namespace System::Drawing::Drawing2D;


#pragma comment(lib, "d2d1")
#pragma comment(lib, "Dwrite")


#pragma once
class PDCHandler
{	

public:
	bool IsClipRegion;

	PDCHandler();
	~PDCHandler(void);
	void Connect(CStockChartXCtrl* Ctl);

	CStockChartXCtrl* pCtrl;
	void Initialize(CDC* memDC, CRect* rect);
	void DrawLine(CPointF P1, CPointF P2, long Width, long Style, OLE_COLOR Color, CDC* pDC, bool Smooth = true);
	void DrawLine(Graphics^ gDC, Pen^ blackPen, CPointF P1, CPointF P2, long Width, long Style, OLE_COLOR Color, CDC* pDC, bool Smooth = true);
	void FillCircle(CRectF rect, OLE_COLOR Color, CDC* pDC);
	void FillRectangle(CRectF rect, OLE_COLOR Color, CDC* pDC);
	void FillRectangleGradient(CRectF rect, OLE_COLOR Color1, OLE_COLOR Color2, CDC* pDC);
	void DrawRectangle(CRectF rect, long Width, long Style, OLE_COLOR Color,CDC*pDC);
	void DrawEllipse(CRectF rect, long Width, long Style, OLE_COLOR Color, CDC*pDC);
	void DrawArc(CRectF rectBounds, CPointF pSrc, CPointF pStart, CPointF pEnd, bool dirClockwise, long Width, long Style, OLE_COLOR Color,CDC*pDC);
	void DrawText(CString text, CRectF rectBounds, LPCSTR fontType, double fontSize, long alignment, OLE_COLOR Color, int opacity, CDC*pDC);
	void DrawPath(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR uColor, OLE_COLOR dColor, CDC* pDC, bool spline = false, bool smooth = true);
	void DrawSemiPath(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR uColor, OLE_COLOR dColor, CDC* pDC, bool spline = false);
	void DrawLadder(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC);
	void DrawImage(CString name, CPointF point, double scale, CDC*pDC);
	void DrawDot(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC);
	void DrawRoundDot(std::vector<CPointF> points, int size, long Width, long Style, OLE_COLOR Color, CDC* pDC);
	void Release();
	void Setup();
};
