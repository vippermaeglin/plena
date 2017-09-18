// CO.cpp: implementation of the CCO class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "CO.h"
#include "julian.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CCO::CCO()
{
	Initialize();
}
/////////////////////////////////////////////////////////////////////////////

CCO::CCO(CChartPanel* owner)
{
	Initialize();	
	ownerPanel = owner;	
}
/////////////////////////////////////////////////////////////////////////////

CCO::~CCO()
{
	if(oldCacheBmp) cacheDC.SelectObject(oldCacheBmp);
	if(oldBmp) memDC.SelectObject(oldBmp);
	cacheDC.DeleteDC();
	memDC.DeleteDC();
}
/////////////////////////////////////////////////////////////////////////////

void CCO::Initialize()
{
	oldCacheBmp = NULL;
	oldBmp = NULL;
	x1Value = 0;
	x2Value = 0;
	y1Value = 0;
	y2Value = 0;
	x1 = 0;
	x2 = 0;
	y1 = 0;
	y2 = 0;
	background_cached = false;
	visible = false;
	selectable = true;
	selected = false;
	connected = false;
	lineWeight = 1;
	foreColor = RGB(0,0,0);			// Black
	backColor = RGB(255, 253, 220); // Light tan
	zOrder = zOrderFront; // Just on top of chart panel because pCtrl = 0 and CChartPanel = 1

	/*	SGC	03.06.2004	BEG	*/
	pCtrl	= NULL;
	/*	SGC	03.06.2004	END	*/
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnPaint(CDC *pDC)
{
	//	virtual function	
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnClick(int x, int y)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnMouseMove(CPoint point)
{
}
/////////////////////////////////////////////////////////////////////////////

// Hides this object. To prevent flicker, use true 
// for updateRect if the entire chart is being redrawn 
// anyway (scrolling, zooming), otherwise use false 
// to hide the object and update the rect in the main dc
// for immediate results.
void CCO::Reset(bool updateRect)
{	
	visible				= false;
	background_cached	= false;

	/*	SGC 03.06.2004	BEG */
	if( pCtrl == NULL )
		return;
	/*	SGC 03.06.2004	END */

	/*	SGC 31.05.2004	BEG */

		this->Hide();
		if( updateRect )
			pCtrl->UpdateRect( oldRect );

	/*	SGC 31.05.2004	END	*/

	/*	SGC	03.06.2004	BEG	*/
	Show();
	/*	SGC	03.06.2004	END	*/
}
/////////////////////////////////////////////////////////////////////////////

/*	SGC 31.05.2004	BEG */

// Restores background then redraws new panel based on location.
void CCO::Show()
{	
	visible = true;

	/*	SGC 03.06.2004	BEG */
	if( pCtrl == NULL )
		return;
	/*	SGC 03.06.2004	END */

	CDC* pDC = &pCtrl->m_memDC;
	
	newRect.left	= 0;
	newRect.right	= 0;
	newRect.top		= 0;
	newRect.bottom	= 0;	

	if( pCtrl->panels.size() == 0 )
		return;

	try	{
		newRect = GetRect();
	}
	catch(...)
	{		
		return;
	}
	
	if( newRect.bottom == 0 && newRect.right == 0 )
		return;
	if( newRect.bottom == -1 && newRect.right == -1 )
		return;

	if(pDC->m_hDC != NULL){
		OnPaint( pDC );
	}
	else{
		return;
	}

	pCtrl->UpdateRect(newRect);
//	pCtrl->UpdateRect(oldRect);


	// Save old rect
	oldRect = newRect;
}
/*	SGC 31.05.2004	END */
/////////////////////////////////////////////////////////////////////////////

//	Restores background if it has been changed since last Show().
void CCO::Hide()
{
	/*	SGC 03.06.2004	BEG */
	visible = false;

	if( pCtrl == NULL )
		return;

	pCtrl->UpdateRect(oldRect);
	/*	SGC 03.06.2004	END */
}
/////////////////////////////////////////////////////////////////////////////

void CCO::Connect(CStockChartXCtrl *Ctrl)
{
	pCtrl = Ctrl;
	guid = pCtrl->CreateGUID();
	connected = true;
}
/////////////////////////////////////////////////////////////////////////////

int CCO::Width()
{
	return x2 - x1;
}
/////////////////////////////////////////////////////////////////////////////

int CCO::Height()
{
	return y2 - y1;
}
/////////////////////////////////////////////////////////////////////////////

CRect CCO::GetRect()
{
	CRect rect = CRect(0,0,0,0);
	return rect;
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnPaintXOR(CDC *pDC)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnLButtonUp(CPoint point)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnLButtonDown(CPoint point)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnRButtonDown(CPoint point)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::ResetPoints()
{	
}
/////////////////////////////////////////////////////////////////////////////

void CCO::UpdatePoints()
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::SetXY()
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnDoubleClick(CPoint point)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
}
/////////////////////////////////////////////////////////////////////////////

void CCO::OnRButtonUp(CPoint point)
{
}
// Calculate X position as Julian Date reference





void CCO::CalculateXJulianDate(void)
{
	MMDDYYHHMMSS x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
	switch(pCtrl->GetPeriodicity())
	{
		case Minutely:	
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			//x2Julian.Second -= 1;
			//x2Julian_2.Second -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
		case Hourly:	
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian.Minute -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
		case Daily:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian.Hour -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
		case Weekly:		
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
		case Month:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
		case Year:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			break;
	}
}
