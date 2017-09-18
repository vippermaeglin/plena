// LineStudyFreehand.cpp: implementation of the CLineStudyFreehand class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFreehand.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyFreehand::CLineStudyFreehand(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{

	objectType = "Freehand";	
	nType = lsFreehand;
	
	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = CRect(0,0,0,0);
	
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	lineColor = color;
	Initialize();	
	drawing = false;
	displayed = false;
	xor = false;
	dragging = false;	
	m_Move = cnNone;	
	buttonState = 0;
	savedX = 0;
	savedY = 0;
	startX = 0;
	startY = 0;
	prevX = 0;
	prevY = 0;
	maxX = 0;
	maxY = 0;
	minX = 1000000000;
	minY = 1000000000;
	key = Key;
	lineWeight = pCtrl->lineThickness;	
}

CLineStudyFreehand::~CLineStudyFreehand()
{

}

void CLineStudyFreehand::OnPaint(CDC *pDC)
{


	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}
	

	ExcludeRects(pDC);

	// Create hit test regions for mouse clicks on the object
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a Freehand Drawing
	
	
	// Draw the object
	if(fillStyle != fsOpaque){
		CRect rect = CRect(minX, minY, maxX, maxY);
		CPen	pen1( 2, lineWeight, lineColor );
		CPen* pOldPen1 = pDC->SelectObject( &pen1 );
		if(selected) DrawRect(pDC, rect);
		pDC->SelectObject(pOldPen1);
		pen1.DeleteObject();

		CPen	pen( lineStyle, lineWeight, lineColor );
		CPen* pOldPen = pDC->SelectObject( &pen );

		// Object was moved so update the points
		if(savedY != y1 || savedX != x1)
		{
			maxY = 0, maxX = 0;
			minY = 10000000, minX = 10000000;
			for(int n = 0; n != points.size(); ++n)
			{
				// Update all the pre-drawn freehand points
				points[n].x = x1 + points[n].x - savedX;
				points[n].y = y1 + points[n].y - savedY;

				// Recalculate max/min values
				if(points[n].x > maxX) maxX = points[n].x;
				if(points[n].y > maxY) maxY = points[n].y;
				if(points[n].x < minX) minX = points[n].x;
				if(points[n].y < minY) minY = points[n].y;
			}
			savedY = y1;
			savedX = x1;
		}

		


		// Paint user-drawn freehand points
		CPoint prev(0,0);
		for(int n = 0; n != points.size(); ++n)
		{			
			CPoint point = points[n];
			if(prev.x == 0)
			{
				pDC->MoveTo(point.x, point.y);
				pDC->LineTo(point.x, point.y);
			}
			else
			{
				pDC->MoveTo(prev.x, prev.y);
				pDC->LineTo(point.x, point.y);
			}
			prev = point;
		}





		pDC->SelectObject(pOldPen);
		pen.DeleteObject();
	}
	else{
		// Fill in
		CBrush	fill( lineColor );
		pDC->FillRect( CRect(x1, y1, x2, y2), &fill );
		fill.DeleteObject();
	}
	


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( RGB(0,0,255) );
		int oldLineWeight = lineWeight;
		lineWeight=1;
		pDC->FillRect( CRect(minX-3,minY-3,minX+3,minY+3), &br );		
		pDC->FillRect( CRect(minX+3,maxY+3,minX-3,maxY-3), &br );
		pDC->FillRect( CRect(maxX-3,minY-3,maxX+3,minY+3), &br );
		pDC->FillRect( CRect(maxX+3,maxY+3,maxX-3,maxY-3), &br );		
		br.DeleteObject();
		lineWeight=oldLineWeight;
	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFreehand::OnLButtonUp(CPoint point)
{

	if(drawing) return; // The object is being resized	

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

	// The left button was released so fire the click event
	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
		pCtrl->SaveUserStudies();
	}

	
	// See if the object has been clicked
	bool clicked = IsDrawingClicked(point);
	

	//	point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;


	if(selected && oldRect.right > 0) // the object was just resized
	{

		int oldX1 = x1;
		int oldY1 = y1;

		// Update the x's and y's to the 
		// drag rect that was just drawn
		x1 = newRect.left;
		x2 = newRect.right;		
		y1 = newRect.top;
		y2 = newRect.bottom;

		// We're not drawing anymore so reset the state
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->movingObject = false;
		m_Move = cnNone;		
		newRect = CRect(0,0,0,0);
		oldRect = newRect;
  		
		// Snap the object to the nearest bars		
		Reset();
		
		maxY = 0, maxX = 0;
		minY = 10000000, minX = 10000000;
		for(int n = 0; n != points.size(); ++n)
		{
			// Update all the pre-drawn freehand points
			points[n].x = x1 + points[n].x - oldX1;
			points[n].y = y1 + points[n].y - oldY1;

			// Recalculate max/min values
			if(points[n].x > maxX) maxX = points[n].x;
			if(points[n].y > maxY) maxY = points[n].y;
			if(points[n].x < minX) minX = points[n].x;
			if(points[n].y < minY) minY = points[n].y;
		}

		savedX = x1;
		savedY = y1;

		if(!clicked){
			selected = false;
		}
		else{
			selected = true;
		}

		ownerPanel->Invalidate(); // The panel needs to repaint
		pCtrl->UnSelectAll(); // Unselect ALL objects and series
		pCtrl->changed = true; // The file should be saved
		
		pCtrl->RePaint(); // Cause the chart to repaint	
		pCtrl->UpdateScreen(false);		

	}

	buttonState = MOUSE_UP; // The mouse is up
	pCtrl->dragging = false; // The mouse is up so we're not dragging anything

}



void CLineStudyFreehand::OnLButtonDown(CPoint point)
{
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip


	bool clicked = IsDrawingClicked(point); //point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;

	if(clicked && selectable){
		pCtrl->UnSelectAll(); // Unselect everything else
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRect(0,0,0,0);
			newRect = oldRect;
			pCtrl->movingObject = true;				
			pCtrl->RePaint();
			if(buttonState != MOUSE_DOWN){
				startX = point.x; // Save the starting location
				startY = point.y;
			}

			// Can we move this line?
			if(point.x > x1-15 && point.y > y1-15 && point.x < x1+15 && point.y < y1+15){
				m_Move = cnTopLeft;	
			}
			else if(point.x > x2-15 && point.y > y1-15 && point.x < x2+15 && point.y < y1+15){
				m_Move = cnTopRight;
			}
			else if(point.x > x2-15 && point.y > y2-15 && point.x < x2+15 && point.y < y2+15){
				m_Move = cnBottomRight;
			}
			else if(point.x > x1-15 && point.y > y2-15 && point.x < x1+15 && point.y < y2+15){
				m_Move = cnBottomLeft;
			}

			else{
				if(IsDrawingClicked(point)){
					m_Move = cnMoveAll;
				}
			}

			SetCursor(AfxGetApp()->LoadCursor(IDC_CLOSED_HAND));

			pCtrl->UnSelectAll();
			selected = true;
			buttonState = MOUSE_DOWN;
			pCtrl->dragging = true;
			return;
		}
	}

	bool wasSelected = selected;
	selected = false;
	ownerPanel->Invalidate();
	if(wasSelected) pCtrl->RePaint();
	
	buttonState = MOUSE_DOWN;
	pCtrl->dragging = true;
}


void CLineStudyFreehand::XORDraw(UINT nFlags, CPoint point)
{	

	if(nFlags == 1){ // First point
		startX = point.x;
		startY = point.y;
		drawing = true;	// The object is being resized
	}
	else if(nFlags == 2){ // Drawing by freehand
		
		CDC* pDC = pCtrl->GetScreen();
		pDC->SetROP2(R2_NOT);
		
		ExcludeRects(pDC);
		
		
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;
	//	DrawRect(pDC, oldRect); // Clear the previous drawing

		
		newRect.left = startX;
		newRect.right = point.x;
		newRect.top = startY;
		newRect.bottom = point.y;

		if(newRect.right < x1){
			newRect.left = x1 + 5;
			point.x = x1 + 5;
		}

	 
		
		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
	
		// To keep from drawing over things
		ExcludeRects(pDC);


		if(prevX == 0)
		{
			pDC->MoveTo(point.x, point.y);
			pDC->LineTo(point.x, point.y);
		}
		else
		{
			pDC->MoveTo(prevX, prevY);
			pDC->LineTo(point.x, point.y);
		}

		// Save maximum and minimum start/end positions
		// to draw a rectangle around the drawing
		if(point.x > maxX) maxX = point.x;
		if(point.y > maxY) maxY = point.y;
		if(point.x < minX) minX = point.x;
		if(point.y < minY) minY = point.y;

		// Save previous points
		prevX = point.x;
		prevY = point.y;

		// Save current points in array
		points.push_back(point);


		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;
		pCtrl->ReleaseScreen(pDC);

	}
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
		y1	= minY;
		y2	= maxY;
		x1	= minX;
		x2	= maxX;
		int temp = 0;
		if(x1 > x2)
		{
			temp = x2;
			x2 = x1;
			x1 = temp;
			temp = y2;
			y2 = y1;
			y1 = temp;
		}
		startX = 0;
		startY = 0;
		pCtrl->movingObject = false;
		pCtrl->RePaint();
		pCtrl->changed = true;
		selected = false;
		Reset();
		drawing = false;
		savedX = x1;
		savedY = y1;

		/*	SGC	03.06.2004	BEG	*/		
		this->OnPaint( pCtrl->GetScreen() );
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		pCtrl->OnUserDrawingComplete(lsFreehand, key);
		pCtrl->SaveUserStudies();

	}
}

















void CLineStudyFreehand::OnMouseMove(CPoint point)
{

	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		return;
	}


	//bool clicked = IsDrawingClicked();	// Is the mouse within clicking distance?
	bool clicked = point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;
	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key); // Then fire MouseMove

	
	// Is the mouse pointer over the freehand drawing border?
	if(selected && m_Move == cnNone){
		if(point.x > x1-8 && point.y > y1-8 && point.x < x1+8 && point.y < y1+8){
			SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
		}
		else if(point.x > x2-8 && point.y > y1-8 && point.x < x2+8 && point.y < y1+8){
			SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));		
		}
		else if(point.x > x2-8 && point.y > y2-8 && point.x < x2+8 && point.y < y2+8){		
			SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
		}
		else if(point.x > x1-5 && point.y > y2-5 && point.x < x1+5 && point.y < y2+5){
			SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
		}
		else{
			if(IsDrawingClicked(point)){
				// Not over a corner, what about the rest of the object?
				SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND_MOVE));
			}
			else{
				// Not over a corner or anything
				SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
			}
		}
	}



	// Display the tool tip if we are allowed to
	if(!pCtrl->textDisplayed )
	{
		displayed = true;
		pCtrl->textDisplayed = true;
		pCtrl->DelayMessage(guid, MSG_TOOLTIP, 1000);
	}
	else // Hide the tool tip
	{
		if( displayed ) pCtrl->textDisplayed = false;
		displayed = false;
		m_valueView.Connect( pCtrl );		
		m_valueView.Reset(true);
	}



	// See if we are moving or resizing the object, if so, draw a XOR object
	if((m_Move == cnMoveAll) && buttonState == MOUSE_DOWN && selected){ // If moving

		CDC* pDC = pCtrl->GetScreen();
		
		pDC->SetROP2(R2_NOT); // XOR		


		// Draw the temporary object
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;

		// To keep from drawing over things
		ExcludeRects(pDC);

		DrawRect(pDC, oldRect);
		
 
		// Flip coordinates if needed
		if(m_Move == cnTopLeft){
			if(point.x < x2){		// Don't flip
				newRect.left = point.x;
				newRect.right = x2;
				x1 = point.x;
				y1 = point.y;		
			}
			else {					// Flip
				newRect.left = oldRect.left;
				newRect.right = point.x;
				x1 = x2;
				x2 = point.x;
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnTopRight){
			if(point.x > x1){
				newRect.right = point.x;
				newRect.left = x1;
				x2 = point.x;
				y1 = point.y;
			}
			else {
				newRect.right = oldRect.right;
				newRect.left = point.x;
				x2 = x1;
				x1 = newRect.left;							
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnBottomRight){
			if(point.x >= x1){
				newRect.right = point.x;
				newRect.left = x1;
				x2 = point.x;
				y2 = point.y;
			}
			else {
				newRect.right = x1;
				newRect.left = point.x;
				x2 = x1;
				x1 = newRect.left;
				y2 = y1;
				y1 = point.y;				
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnBottomLeft){
			if(point.x <= x2){
				newRect.left = point.x;
				newRect.right = x2;
				x1 = point.x;
				y2 = point.y;
			}
			else {
				newRect.left = x2;
				newRect.right = point.x;
				x1 = x2;
				x2 = newRect.right;
				y2 = y1;
				y1 = point.y;				
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnMoveAll){// Just move the entire line (don't resize it)
			newRect.left = x1 - (startX - point.x);
			newRect.right = x2 - (startX - point.x);
			newRect.top = y1 - (startY - point.y);
			newRect.bottom = y2 - (startY - point.y);
		}


		// To keep from drawing over things
		ExcludeRects(pDC);

		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
		DrawRect(pDC, newRect);	
		
		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyFreehand::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFreehand::OnDoubleClick(CPoint point)
{	
	if(selected) pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
}
/////////////////////////////////////////////////////////////////////////////

// Workaround
// Simply create a CLineStudyFreehand, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFreehand::SnapLine()
{
	startX = 0;
	startY = 0;
	pCtrl->movingObject = false;
	pCtrl->RePaint();
	pCtrl->changed = true;
	selected = false;
	Reset();
	drawing = false;		
}
/////////////////////////////////////////////////////////////////////////////


// Info text

void CLineStudyFreehand::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) return;
	switch(MsgID){
		case MSG_TOOLTIP:
			DisplayInfo();
			break;
	}
}

void CLineStudyFreehand::DisplayInfo()
{
	if(drawing) return; // The object is being resized	
	CPoint point = pCtrl->m_point;
	if(!IsDrawingClicked(point)) return;
	CString temp;
	CString text;
	CString datetime = "";
	if(ownerPanel->series.size() == 0) return;
	int revX = (int)(ownerPanel->GetReverseX(x1) + pCtrl->startIndex);
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		int future = (int)(ownerPanel->GetReverseX(x1) - pCtrl->RecordCount() + pCtrl->startIndex);
		temp.Format("%d", future);
		datetime = temp + " periods into future";
	}
	text += "X1   " + datetime + char(13) + char(10);
	revX = (int)ownerPanel->GetReverseX(x2) + pCtrl->startIndex;
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		int future = (int)ownerPanel->GetReverseX(x2) - pCtrl->RecordCount() + pCtrl->startIndex;
		temp.Format("%d", future);
		datetime = temp + " periods into future";
	}
	text += "X2   " + datetime + char(13) + char(10);
	temp.Format("%.*f", 2, ownerPanel->GetReverseY(y1));
	text += "Y1   " + temp + char(13) + char(10);
	temp.Format("%.*f", 2, ownerPanel->GetReverseY(y2));
	text += "Y2   " + temp + char(13) + char(10);		
	temp.Format("%d", GetAngle());
	text += "Base Angle   " + temp + "°";
	m_valueView.Text = text;
	m_valueView.Connect(pCtrl);
	m_valueView.overidePoints = true;
	m_valueView.x1 = point.x + 10;
	m_valueView.y1 = point.y + 21;
	m_valueView.x2 = point.x + 155;
	m_valueView.y2 = point.y + 88;
	m_valueView.Show();
}

bool CLineStudyFreehand::IsDrawingClicked(CPoint point)
{
	return point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;
}