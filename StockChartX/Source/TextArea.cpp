// TextArea.cpp: implementation of the CTextArea class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "TextArea.h"
#include "julian.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CTextArea::CTextArea()
{
	selected = false;
}

CTextArea::CTextArea(int X, int Y, CChartPanel* owner)
{
	zOrder = zOrderFront;
	fontSize = 0; // Uses global font size if left at zero
	ownerPanel = owner;
	pCtrl = owner->pCtrl;	
	x1 = X;
	y1 = Y;
	x2 = x1;
	y2 = y1;
	Text = "";
	fontColor = pCtrl->lineColor;
	gotUserInput = false;
	typing = false;
	cached = false;
	bReset = true;
	dragging = false;
	selectable = true;
	startX = 0;
	startY = 0;
	buttonState = 0;
	x1Value = 0; // Chart values
	x2Value = 0;
	y1Value = 0;
	y2Value = 0;
	oldRect = CRect(0,0,0,0);
	m_rect = oldRect;
}

CTextArea::~CTextArea()
{
	/*	SGC	32.05.2004	BEG*/
	if(cacheDC) // Added 9/27/05
	{
		if( oldBmp )
			cacheDC.SelectObject( oldBmp );
		cacheDC.DeleteDC();
		cache_bitmap.DeleteObject();
	}
	/*	SGC	32.05.2004	END*/
}
/////////////////////////////////////////////////////////////////////////////

void CTextArea::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	/*	SGC	31.05.2004	BEG*/
	
	CPoint point = pCtrl->m_point;

	if( typing )
	{

		//	Capture the text
		char	ch	= (char)nChar;

		//~~~~
		//	Restore the cached the background before typing
		CBitmap* oldBmp2 = pCtrl->m_memDC.SelectObject( &cache_bitmap );
		pCtrl->m_memDC.BitBlt( m_rect.left, m_rect.top, m_rect.Width(), m_rect.Height(), &cacheDC, 0, 0, SRCCOPY);
		pCtrl->m_memDC.SelectObject( &oldBmp2 );
		//~~~~

		//	If the character is alphabetical, number or enter
		if( ch >=32 ) 
		{
			//	Append the character to the text
			Text += ch;
		}
		else	//	The character is control code
		{
			//	if the backspace is pressed
			if( ch == 8 )
			{
				int	size	= Text.GetLength();
				if( size > 0 )
					Text = Text.Left( size - 1 );
			}
			else if( ch == 13 )
			{
				//	if Enter is pressed, do nothing
				Text	+= ch;
			}
		}


		if( Text.GetLength() > 0 )
		{
			OnPaint( &pCtrl->m_memDC );
		}
		

		//newRect	= m_rect;
		newRect.top		= y1;
		newRect.left	= x1;
		newRect.right	= x2;
		newRect.bottom	= y2;
		pCtrl->UpdateRect( newRect );
	}
	
	/*	SGC	31.05.2004	END*/
}
/////////////////////////////////////////////////////////////////////////////

void CTextArea::OnPaint(CDC *pDC)

{	
#ifdef _CONSOLE_DEBUG
	printf("TextArea::OnPaint()");
#endif
	CPoint point = pCtrl->m_point;

	if(x2Value < pCtrl->startIndex) return; // 2/16/2007 FG
	
	Update();

	//Get font info
	CFont newFont;
	int nFontSize = fontSize;
	if(nFontSize == 0) nFontSize = pCtrl->textAreaFontSize;
	newFont.CreatePointFont(nFontSize, pCtrl->textAreaFontName, pDC);	
	TEXTMETRIC tm;
	CFont *pOldFont = pDC->SelectObject(&newFont);
	pDC->GetTextMetrics(&tm);
	int nAvgWidth = (tm.tmAveCharWidth);
	int nCharHeight = 1 + (tm.tmHeight);	

	m_rect.left = x1;
	m_rect.top = y1;
	m_rect.bottom = pCtrl->height;
    m_rect.right = pCtrl->width - pCtrl->yScaleWidth;
	//m_rect.bottom = y2;
	//m_rect.right = x2;
	if(m_rect.top<=ownerPanel->y1 || m_rect.top>=ownerPanel->y2){
		return;
	}
	if(pCtrl->yAlignment == LEFT){
		if(x1 < pCtrl->yScaleWidth + 5){
			m_rect.right = m_rect.left;
		}
	}
	
	/*	SGC	31.05.2004	BEG*/
		
	//	Cache the background before typing
	if( !cached )
	{
		if(oldBmp) cacheDC.SelectObject(&oldBmp);
		cacheDC.DeleteDC();
		cacheDC.CreateCompatibleDC(&pCtrl->m_memDC);
		try{
			cacheDC.SetBkMode(pCtrl->m_memDC.GetBkMode());
		}
		catch(...){}
		cache_bitmap.DeleteObject();
		//if(!cache_bitmap.CreateCompatibleBitmap(&pCtrl->m_memDC, 
		//	m_rect.right, m_rect.bottom)) return;
		if(!cache_bitmap.CreateCompatibleBitmap(&pCtrl->m_memDC, 
			100, 100)) return; // 8/8/08
		oldBmp = cacheDC.SelectObject(&cache_bitmap);		
		cacheDC.BitBlt(0,0,m_rect.right,m_rect.bottom, 
			&pCtrl->m_memDC,m_rect.left,m_rect.top,SRCCOPY);
		cached = true;
	}

	/*	SGC	31.05.2004	END*/

	OLE_COLOR oldColor = pDC->SetTextColor(fontColor);
	pDC->DrawText(Text, -1, &m_rect, DT_WORDBREAK | DT_LEFT);
	pDC->SetTextColor(oldColor);
	
	// Do some fancy stuff with the Text string
	// to find the caret position and change x2 & y2.
	int found = -1;
	CString temp;
	CString block;
	temp = Text;
	std::vector<CString> blocks;
  int n;
	for(n = 0; n != Text.GetLength(); ++n){
		// Loop through this string to parse crlf blocks
		found = temp.Find(char(13),0);
		if(found == -1){
			blocks.push_back(temp);
			break; // No more line breaks
		}
		block = temp.Left(found);
		blocks.push_back(block);
		temp = temp.Mid(found + 1);
	}
	int max = 0;
	int maxPixels = 0;
	int lastPixels = 0;
	if(blocks.size() > 0){
		for(n = 0; n != blocks.size(); ++n){
			if(blocks[n].GetLength() > max){
				max = blocks[n].GetLength();
				maxPixels = pDC->GetOutputTextExtent(blocks[n]).cx;
			}
		}		
		lastPixels = pDC->GetOutputTextExtent(blocks[blocks.size() - 1]).cx;
	}
	
	int height = pDC->GetOutputTextExtent(Text).cy; // Height of characters	
	y2 = height * (blocks.size() - 1);
	x2 = maxPixels;
	if(typing){		
		pCtrl->DisplayCaret(x1 + lastPixels + 2, y1 + y2 + (nCharHeight / 2) - 7);
	}
	y2 = height * (blocks.size());
	x2 = x1 + x2;
	if(y2 == 0) y2 = height;
	y2 = y1 + y2;
	if(!dragging){
		newRect.left = x1 - 2;
		newRect.top = y1 - 2;
		newRect.right = x2 + 7;
		newRect.bottom = y2 + 2;
	}

	if(typing){
		//pDC->DrawDragRect(newRect,CSize(2,2),oldRect,CSize(2,2));
		oldRect = newRect;
	}

	if(selected){
		pDC->DrawDragRect(newRect,CSize(2,2),oldRect,CSize(2,2));
	}

	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();	

}

void CTextArea::OnLButtonUp(CPoint point)
{

	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_TEXT, key);
		pCtrl->SaveUserStudies();
	}
	
	if(!gotUserInput && !typing){ // Setup the caret		
		pCtrl->typing = true;
		typing = true;
		Reset();
		pCtrl->DisplayCaret(x1, y1);
	}
	else if(typing){ // Done typing
		pCtrl->typing = false;
		typing = false;
		gotUserInput = true;
		pCtrl->KillCaret();
		pCtrl->UpdateRect(m_rect);
		pCtrl->StopDrawing();
	}
	if(dragging){ // Re-position this text area
		/*m_rect.left = newRect.left;	
		m_rect.right = newRect.right;
		m_rect.top = newRect.top;
		m_rect.bottom = newRect.bottom;*/

		
		x1 = newRect.left;	
		x2 = newRect.right;
		y1 = newRect.top;
		y2 = newRect.bottom;

		ownerPanel->Invalidate();
		pCtrl->RePaint();
		Reset();
				
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->m_Cursor = 0;
		SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
		dragging = false;
		oldRect = CRect(0,0,0,0);
		// Fix clipping in upper/lower panels
		int nFontSize = fontSize;
		if(nFontSize == 0) nFontSize = pCtrl->textAreaFontSize;
		if(nFontSize > DEFAULT_FONT_SIZE){
			pCtrl->ForceRePaint();
		}
	}
	if(point.x > x1 && point.x < x2 && point.y > y1 && point.y < y2){
		if(selectable) selected = true;
	}
	else{
		selected = false;
		//OnPaint(&pCtrl->m_memDC);
	}

	buttonState = MOUSE_UP;
}

void CTextArea::OnMouseMove(CPoint point)
{
	if(point.x > x1 && point.x < x2 && point.y > y1 && point.y < y2){
		pCtrl->FireOnItemMouseMove(OBJECT_TEXT, key);
	}

	if(!selectable) return;
	if(dragging){
		newRect.left = x1 - (startX - point.x);
		newRect.right = x2 - (startX - point.x) /*+ 7*/;
		newRect.top = y1 - (startY - point.y);
		newRect.bottom = y2 - (startY - point.y) /*+ 4*/;
		CDC* pDC = pCtrl->GetScreen();
		if(!pDC) return;
		pDC->SetROP2(R2_COPYPEN);
		pDC->DrawDragRect(newRect,CSize(2,2),oldRect,CSize(2,2));
		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;
	}

	if(point.x > x1 && point.x < x2 && point.y > y1 && point.y < y2){
		if(buttonState == MOUSE_UP){
			if(pCtrl->m_mouseState == MOUSE_NORMAL){
				pCtrl->m_mouseState = MOUSE_OPEN_HAND;
				pCtrl->m_Cursor = IDC_OPEN_HAND;
				SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
			}			
		}
	}
	else{
		if(!dragging){
			if(pCtrl->m_mouseState == MOUSE_OPEN_HAND){
				pCtrl->m_mouseState = MOUSE_NORMAL;		
				pCtrl->m_Cursor = 0;
				SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));		
			}
		}
	}


}


// Sets chart value lookup based on actual pixel position
void CTextArea::Reset()
{
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2)  + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	if(x2Value <= x1Value) x2Value = x1Value;
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	x1 = (int)(ownerPanel->GetX((double)(x1Value - pCtrl->startIndex)));
	x2 = (int)ownerPanel->GetX((double)(x2Value - pCtrl->startIndex));
	y1 = (int)ownerPanel->GetY((double)y1Value);
	y2 = (int)ownerPanel->GetY((double)y2Value);
//	End Of Revision
	switch(pCtrl->GetPeriodicity())
	{
		case Minutely:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
		case Hourly:
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
		case Daily:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
		case Weekly:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
		case Month:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
		case Year:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			break;
	}
}

void CTextArea::Update()
{	
//	Revision to rid of build warnings 6/10/2004
//	type cast of int
	x1 = (int)ownerPanel->GetX((double)(x1Value - pCtrl->startIndex));
	x2 = (int)ownerPanel->GetX((double)(x2Value - pCtrl->startIndex));
	y1 = (int)ownerPanel->GetY((double)y1Value);
	y2 = (int)ownerPanel->GetY((double)y2Value);
//	End Of Revision
}

void CTextArea::OnLButtonDown(CPoint point)
{	
	if(point.x > x1 && point.x < x2 && point.y > y1 && point.y < y2){
		if(selectable){
			pCtrl->UnSelectAll();
			selected = true;
			dragging = true;
			startX = point.x;
			startY = point.y;
			pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
			pCtrl->m_Cursor = IDC_CLOSED_HAND;
			SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
			//newRect = m_rect;
			//oldRect = newRect;
		}
	}
	buttonState = MOUSE_DOWN;
}

void CTextArea::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_TEXT, key);	
}

void CTextArea::OnDoubleClick(CPoint point)
{	
	if(selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_TEXT, key);
	}
}
