// SymbolObject.cpp: implementation of the CSymbolObject class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "SymbolObject.h"
#include "julian.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define _CONSOLE_DEBUG


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSymbolObject::CSymbolObject(double X, double Y, long BitmapID, 
			   LPCTSTR Key, LPCTSTR Text, LPCTSTR ObjectType, CChartPanel* Owner)
{
	bitmapID = BitmapID;
	ownerPanel = Owner;


	objectType = ObjectType;
	if(objectType == "Buy Symbol")
	{
		nType = otBuySymbolObject;
	}
	else if(objectType == "Sell Symbol")
	{
		nType = otSellSymbolObject;
	}
	else if(objectType == "Exit Symbol")
	{
		nType = otExitSymbolObject;
	}
	else if( objectType == "Exit Long Symbol" )
	{
		nType = otExitLongSymbolObject;
	}
	else if( objectType == "Exit Short Symbol" )
	{
		nType = otExitShortSymbolObject;
	}
	else if( objectType == "Signal Symbol" )
	{
		nType = otSignalSymbolObject;
	}
	else if( objectType == "Custom Symbol" )
	{
		nType = otSymbolObject;
	}
	else
	{
		nType = otSymbolObject;
	}


//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	x1 = (int)X;
	y1 = (int)Y;
//	End of revision
	x1Value = X;
	y1Value = Y;
	textDisplayed	= false;
	dragging		= false;
	selected		= false;
	selectable		= true;
	startX = 0;
	startY = 0;
	buttonState = MOUSE_UP;
	key = Key;		
	text = Text;
	oldRect = newRect = CRect(0,0,0,0);

	/*	SGC	32.05.2004	BEG*/
	m_faceBMP	= NULL;
	m_maskBMP	= NULL;
	/*	SGC	32.05.2004	END*/
}
/////////////////////////////////////////////////////////////////////////////

CSymbolObject::CSymbolObject(double X, double Y, LPCTSTR FileName, 
			   LPCTSTR Key, LPCTSTR Text, LPCTSTR ObjectType, CChartPanel* Owner)
{

	objectType = ObjectType;
	if(objectType == "Buy Symbol")
	{
		nType = otBuySymbolObject;
	}
	else if(objectType == "Sell Symbol")
	{
		nType = otSellSymbolObject;
	}
	else if(objectType == "Exit Symbol")
	{
		nType = otExitSymbolObject;
	}
	else if( objectType == "Exit Long Symbol" )
	{
		nType = otExitLongSymbolObject;
	}
	else if( objectType == "Exit Short Symbol" )
	{
		nType = otExitShortSymbolObject;
	}
	else if( objectType == "Signal Symbol" )
	{
		nType = otSignalSymbolObject;
	}
	else if( objectType == "Custom Symbol" )
	{
		nType = otSymbolObject;
	}
	else
	{
		nType = otSymbolObject;
	}


	bitmapID = -1;
	fileName = FileName;
	ownerPanel = Owner;
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	x1 = (int)X;
	y1 = (int)Y;
//	End Of Revision
	x1Value = X;
	y1Value = Y;
	textDisplayed = false;
	dragging = false;
	selected = false;
	selectable = true;
	startX = 0;
	startY = 0;
	buttonState = MOUSE_UP;
	key = Key;	
	text = Text;

	/*	SGC	32.05.2004	BEG*/
	m_faceBMP	= NULL;
	m_maskBMP	= NULL;
	/*	SGC	32.05.2004	END*/
}
/////////////////////////////////////////////////////////////////////////////

CSymbolObject::~CSymbolObject()
{
	/*	SGC	31.05.2004	BEG*/

	if( m_faceBMP != NULL )
	{
		m_facedc.SelectObject( (CBitmap*)NULL );
		m_faceBMP->DeleteObject();
		delete	m_faceBMP;
		m_faceBMP	= NULL;
	}

	if( m_maskBMP != NULL )
	{
		m_maskdc.SelectObject( (CBitmap*)NULL );
		m_maskBMP->DeleteObject();
		delete	m_maskBMP;
		m_maskBMP	= NULL;
	}

	/*	SGC	31.05.2004	END*/

}
/////////////////////////////////////////////////////////////////////////////

/*	SGC	31.05.2004	BEG*/

void CSymbolObject::OnPaint(CDC* pDC)
{
	CRect	rect	= GetRect();
	if(rect.top<=ownerPanel->y1 || rect.top>=ownerPanel->y2||rect.bottom<=ownerPanel->y1 || rect.bottom>=ownerPanel->y2){
		return;
	}

	if( m_faceBMP == NULL )
		createBitmaps( pDC );

	if( m_faceBMP == NULL )
		return;

	//	If the object is outside the visible area, the x coords have negative values
	if( rect.left == -1 || rect.right == -1 )
		return;
	UpdatePoints();
	rect.left = x1;
	rect.right = x2;
	rect.top = y1;
	rect.bottom = y2;
	BITMAP	bmp;
	m_faceBMP->GetBitmap( &bmp );

	int	bw	= bmp.bmWidth;
	int	bh	= bmp.bmHeight;

	pDC->BitBlt( rect.left, rect.top, bw,bh, &m_maskdc, 0,0, SRCAND   );	//	Prepare the surface
	pDC->BitBlt( rect.left, rect.top, bw,bh, &m_facedc, 0,0, SRCPAINT );	//	Paint the bitmap transparent

	if(selected && !dragging){	
		CRect tempRect;
		tempRect.bottom = rect.bottom+2;
		tempRect.top = rect.top-2;
		tempRect.right = rect.right+2;
		tempRect.left = rect.left-2;
		pDC->SetROP2( R2_COPYPEN );
		pDC->DrawDragRect( tempRect, CSize(2,2), oldRect, CSize(2,2) );
		//pCtrl->ReleaseScreen( pDC );
		oldRect = tempRect;
	}
}
/////////////////////////////////////////////////////////////////////////////

//	Create this object bitamps for face and mask
//	The mask is necessary to make a transparency
//	This method should be called just once for each object
void	CSymbolObject::createBitmaps( CDC* pDC )
{
	if( pDC == NULL )
	{
		pDC	= pCtrl->GetDC();
		ASSERT(pDC != NULL);
	}

	//	Load the bitmap from file or resources
	CFileBitmap	classBitmap;
	if( fileName.IsEmpty() )
	{
		classBitmap.LoadBitmap( bitmapID );
	}
	else
	{
		if( classBitmap.LoadBMPFile( fileName ) == FALSE )
			return;
	}

	BITMAP	bm;
	classBitmap.GetObject( sizeof(BITMAP),&bm );
	CPoint size( bm.bmWidth,bm.bmHeight );

	CPoint	org( 0,0 );

	//	Create a memory dc (classMemoryDC) and select the bitmap into it
	CDC	classMemoryDC;
	classMemoryDC.CreateCompatibleDC( pDC );
	CBitmap*	pOldClassBitmap	= classMemoryDC.SelectObject( &classBitmap );
	classMemoryDC.SetMapMode( pDC->GetMapMode() );

	//~~~~~~~~~~~~~

	CDC&	facedc	= m_facedc;
	CDC&	maskdc	= m_maskdc;

	facedc.CreateCompatibleDC( pDC );
	maskdc.CreateCompatibleDC( pDC );

	m_faceBMP	= new CBitmap();
	m_maskBMP	= new CBitmap();

	CBitmap&	faceBmp	= *m_faceBMP;
	CBitmap&	maskBmp	= *m_maskBMP;

	int		bw	= bm.bmWidth;
	int		bh	= bm.bmHeight;

	faceBmp.CreateCompatibleBitmap( pDC, bw, bh );
	maskBmp.CreateCompatibleBitmap( pDC, bw, bh );

	CBitmap*	oldFaceBmp	= facedc.SelectObject( &faceBmp );
	CBitmap*	oldMaskBmp	= maskdc.SelectObject( &maskBmp );

	//	Draw faces of the the face bitmap, and the mask bitmap
	COLORREF	black	= RGB( 0,0,0 );
	COLORREF	white	= RGB( 255,255,255 );

	facedc.BitBlt( 0,0, bw, bh, &classMemoryDC, 0,0, SRCCOPY );

	COLORREF	transparentColor	= facedc.GetPixel( 0,0 );
	for( int y = 0; y < bh; y++ )
	{
		for( int x = 0; x < bw; x++ )
		{
			if( facedc.GetPixel( x,y ) == transparentColor )
			{
				maskdc.SetPixel( x,y, white );
				facedc.SetPixel( x,y, black );
			}
			else
			{
				maskdc.SetPixel( x,y, black );
			}
		}
	}
	//	Now we have both face & mask bitmaps

	//~~~~~~~

	classMemoryDC.SelectObject( pOldClassBitmap );
	classBitmap.DeleteObject();
}
/*	SGC	31.05.2004	END*/
/////////////////////////////////////////////////////////////////////////////


CRect CSymbolObject::GetRect()
{
	m_valueView.Connect(pCtrl);

	// Position buy arrow lower or sell arrow higher
	int offset = 0;
	int loffset = 0;
	switch(bitmapID){
		case IDB_BUY:
			offset = -2;
			break;
		case IDB_SELL:
			offset = m_bmHeight + 2;
			break;
		case IDB_EXIT:
			offset = 2 + m_bmHeight / 2;
			break;
		case IDB_EXIT_LONG:
		case IDB_EXIT_SHORT:
			offset = 2;
			break;
		case IDB_SIGNAL:
			offset = 2;
			break;		
		default:
			offset = m_bmHeight / 2;
			break;
	}

	CRect	rect;
	rect.top	= y1 - offset;
	rect.bottom	= y2 - offset;
	rect.left	= x1 - (m_bmWidth / 2) + loffset;
	rect.right	= x2 - (m_bmWidth / 2) + loffset;

	/*	SGC	31.05.2004	BEG	*/
	// Don't display bitmap if off screen	
	if(pCtrl->yAlignment == LEFT){
		if( rect.left > pCtrl->width || rect.left < pCtrl->yScaleWidth + 5)
		{
			rect = CRect( -1, -1, -1, -1 );
		}
	}
	else{
		if( rect.right > (pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels) + (rect.right - rect.left) 
			|| x1 < 0)
		{
			rect = CRect( -1, -1, -1, -1 );
		}
	}



	/* Removed 7/8/07
	//	SGC	31.05.2004

	// Make sure the bitmap is visible on the y scale
	if(rect.bottom > ownerPanel->y2)
	{

//	Revision to rid of build warnings 6/10/2004
//	type cast of LONG
		rect.bottom = (LONG)ownerPanel->y2;
		rect.top = rect.bottom - m_bmHeight;
//	End Of Revision
	}
	else if(rect.top < ownerPanel->y1)
	{
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
		rect.top = (LONG)ownerPanel->y1;
//	End Of Revision
		rect.bottom = rect.top + m_bmHeight;
	}
	*/


	return rect;
}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::SetXY()
{	
	CDC* pDC = &pCtrl->m_memDC;
	CFileBitmap bmp;
	
	if(fileName.GetLength() > 0){
		if(bmp.LoadBMPFile(fileName) == FALSE) return;
	}
	else{
		bmp.LoadBitmap(bitmapID);
	}

	BITMAP bm;
	bmp.GetObject(sizeof(BITMAP),&bm);	
	m_bmHeight = bm.bmHeight;
	m_bmWidth = bm.bmWidth;
	/*
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	x1 = (int)ownerPanel->GetX(x1 - pCtrl->startIndex);
	y1 = (int)ownerPanel->GetY(y1);
//	End Of Revision
	x2 = x1 + m_bmWidth;
	y2 = y1 + m_bmHeight;
	*/
	
	x1 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex), true); // 2/3/06 offscreen
	x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex), true); // 2/3/06 offscreen
	y1 = ownerPanel->GetY(y1Value);
	y2 = ownerPanel->GetY(y2Value);

	/*	
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
	}*/
	bmp.DeleteObject();

}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::UpdatePoints()
{
	if(!connected) return;
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	int period = (int)x1Value - pCtrl->startIndex;
	if(period < 0)
	{
		x1 = -1;
	}
	else{
		x1 = (int)ownerPanel->GetX(period);
	}
	y1 = (int)ownerPanel->GetY(y1Value);
//	End Of Revision
	y2 = y1 + m_bmHeight;
	x2 = x1 + m_bmWidth;

}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::OnMouseMove(CPoint point)
{

	CRect rect = GetRect();
	// The rect is used for positioning rather than
	// x,y because each bitmap has a different size
	// or hot-point.

	if(inArea(rect,point))
	{
		pCtrl->FireOnItemMouseMove(OBJECT_SYMBOL, key);
	}
	
	if( textDisplayed )
	{
		pCtrl->textDisplayed = false;
		textDisplayed = false;
	}
	
	if( dragging )
	{	
		// If we're dragging, then show the updated drag rect position
		newRect.left	= rect.left		- (startX - point.x);
		newRect.right	= rect.right	- (startX - point.x);
		newRect.top		= rect.top		- (startY - point.y);
		newRect.bottom	= rect.bottom	- (startY - point.y);

		CDC* pDC = pCtrl->GetScreen();

		pDC->SetROP2( R2_COPYPEN );
		pDC->DrawDragRect( newRect, CSize(2,2), oldRect, CSize(2,2) );
		pCtrl->ReleaseScreen( pDC );
		oldRect = newRect;
		
		pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
		pCtrl->m_Cursor = IDC_CLOSED_HAND;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		return;
	}
	
	if(selected && buttonState == MOUSE_DOWN && selectable)
	{
		//return
	}

	m_valueView.Text = text;
	if(inArea(rect,point) && !selected)
	{		
#ifdef _CONSOLE_DEBUG
			printf("\nCSymbol::MouseMove() inArea && !selected!");
#endif
		if(m_valueView.Text != "" && !pCtrl->textDisplayed)
		{
			
			pCtrl->textDisplayed = true;
			textDisplayed = true;
			//pCtrl->m_mouseState = MOUSE_DRAWING;
			m_valueView.x1 = rect.left + 10;
			m_valueView.y1 = rect.top + 21;
			m_valueView.Show();
			
		}
		
		if(pCtrl->m_mouseState == MOUSE_NORMAL){
			pCtrl->m_mouseState = MOUSE_OPEN_HAND;
			pCtrl->m_Cursor = IDC_OPEN_HAND;
			SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		}	
		return;
	}
	else
	{
		if(m_valueView.Text != "" && m_valueView.visible )
		{
			m_valueView.Reset(true);
		}
	}
	
	if(inArea(rect,point))

	{

#ifdef _CONSOLE_DEBUG
			printf("\nCSymbol::MouseMove() inArea!");
#endif
		pCtrl->m_mouseState = MOUSE_OPEN_HAND;
		pCtrl->m_Cursor = IDC_OPEN_HAND;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		return;
	}
	//else if(pCtrl->m_mouseState == MOUSE_OPEN_HAND) pCtrl->m_mouseState = MOUSE_NORMAL;	
	else if(!(/*pCtrl->objectSelected || pCtrl->resizing || */pCtrl->dragging || pCtrl->drawing || pCtrl->lineDrawing || pCtrl->typing)&&(pCtrl->m_mouseState==MOUSE_OPEN_HAND)){
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->m_Cursor = 0;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
	}
}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::OnLButtonDown(CPoint point)
{
	buttonState = MOUSE_DOWN;

	CRect rect = GetRect();
	/*
	if(!selected && inArea(rect,point) && selectable)
	{
		newRect = rect;
		CDC* pdc = pCtrl->GetScreen();
		pdc->SetROP2(R2_COPYPEN);
		pdc->DrawDragRect(rect,CSize(2,2),oldRect,CSize(2,2));
		pCtrl->ReleaseScreen(pdc);
		pCtrl->objectSelected = true;
		oldRect = rect;
		
	}

	if(selected)
	{
		dragging = true;
		startX = point.x;
		startY = point.y;		
		pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
		pCtrl->m_Cursor = IDC_CLOSED_HAND;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
	}*/

	
	if(inArea(rect,point)/* && !selected && selectable*/)
	{
		pCtrl->UnSelectAll();
		selected = true;
		pCtrl->objectSelected = true;
		pCtrl->movingObject = true;
	
		dragging = true;
		startX = point.x;
		startY = point.y;		
		pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
		pCtrl->m_Cursor = IDC_CLOSED_HAND;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
	}
	
}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::OnLButtonUp(CPoint point)
{
	buttonState = MOUSE_UP;

	if(inArea(GetRect(),point)){
		pCtrl->FireOnItemLeftClick(OBJECT_SYMBOL, key);
		pCtrl->SaveUserStudies();
	}

	CRect rect = GetRect();
	
	if(inArea(rect,point) && selectable){
		pCtrl->UnSelectAll();
		dragging = false;
		pCtrl->dragging = false;
		pCtrl->movingObject = false;
		
		newRect = rect;
		newRect.left	= rect.left		- 15;
		newRect.right	= rect.right	- 15;
		newRect.top		= rect.top		- 15;
		newRect.bottom	= rect.bottom	- 15;

		CDC* pdc = pCtrl->GetScreen();
		pdc->SetROP2(R2_COPYPEN);
		pdc->DrawDragRect(rect,CSize(4,4),oldRect,CSize(4,4));
		pCtrl->ReleaseScreen(pdc);
		//pCtrl->objectSelected = true;
		selected = true;
		oldRect = rect;

	
		/*pCtrl->m_mouseState = MOUSE_OPEN_HAND;
		pCtrl->m_Cursor = IDC_OPEN_HAND;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));*/
		
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->m_Cursor = 0;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		pCtrl->SaveUserStudies();
		return;
	}
	if(!inArea(rect,point) && selected)selected = false; 
	if(dragging){
		int offset = 0;
		switch(bitmapID){
			case IDB_BUY:
				offset = -2;
				break;
			case IDB_SELL:
				offset = m_bmHeight + 2;
				break;
			case IDB_EXIT:
				offset = 2 + m_bmHeight / 2;
				break;
			case IDB_EXIT_LONG:
			case IDB_EXIT_SHORT:
				offset = -2;
				break;
			case IDB_SIGNAL:			
				offset = +2;
				break;
			default:
				offset = 2 + m_bmHeight / 2;
				break;
		}
		rect.top = newRect.top + offset;
		rect.bottom = newRect.bottom + offset;
		rect.left = newRect.left;
		rect.right = newRect.right;
		/*
		UpdatePoints:
		y1 = rect.top;
		y2 = rect.bottom;
		x1 = rect.left;
		x2 = rect.right;*/
		x1Value = ownerPanel->GetReverseX(rect.left + (m_bmWidth / 2)) + 1 + pCtrl->startIndex;
		x2Value = ownerPanel->GetReverseX(rect.right) + 1 + pCtrl->startIndex;
		y1Value = ownerPanel->GetReverseY(rect.top + 2);
		y2Value = ownerPanel->GetReverseY(rect.bottom);
		CalculateXJulianDate();
		UpdatePoints();
		ownerPanel->Invalidate();
		pCtrl->RePaint();
		dragging = false;
		selected = true;
		pCtrl->movingObject = false;
		pCtrl->dragging = false;
		oldRect = CRect(0,0,0,0);
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->m_Cursor = 0;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		pCtrl->SaveUserStudies();
	}
	//if(!(/*pCtrl->objectSelected || pCtrl->resizing || */pCtrl->dragging || pCtrl->drawing || pCtrl->lineDrawing || pCtrl->typing)) pCtrl->m_mouseState = MOUSE_NORMAL;
}
/////////////////////////////////////////////////////////////////////////////

bool CSymbolObject::inArea(CRect rect, CPoint point)
{
	return point.x > rect.left && point.x < rect.right && 
		   point.y > rect.top && point.y < rect.bottom;
}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::OnRButtonUp(CPoint point)
{
	if(inArea(GetRect(),point)){
		pCtrl->FireOnItemRightClick(OBJECT_SYMBOL, key);
	}
}
/////////////////////////////////////////////////////////////////////////////

void CSymbolObject::OnDoubleClick(CPoint point)
{
	if(inArea(GetRect(),point)){
		if(selected) pCtrl->FireOnItemDoubleClick(OBJECT_SYMBOL, key);	
	}
}
// Get Julian Date references to calculate absolute position


