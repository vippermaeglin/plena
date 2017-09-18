// LineStudyRectangle.cpp: implementation of the CLineStudyRectangle class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyRectangle.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


//#define _CONSOLE_DEBUG

CLineStudyRectangle::CLineStudyRectangle(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{

	objectType = "Rectangle";	
	nType = lsRectangle;
	
	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = CRectF(0,0,0,0);
	
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
	startX = 0;
	startY = 0;
	key = Key;
	lineWeight = pCtrl->lineThickness;	
}

CLineStudyRectangle::~CLineStudyRectangle()
{

}

void CLineStudyRectangle::OnPaint(CDC *pDC)
{


	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}

	if (!moveCtr * m_Move==cnNone)Update();
	ExcludeRects(pDC);

	// Create hit test regions for mouse clicks on the object
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a Rectangle
	
	
	// Draw the object
	if(fillStyle != fsOpaque){
		CRect rect = CRect((int)x1,(int)y1,(int)x2,(int)y2);
		CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
		CPen* pOldPen = pDC->SelectObject( &pen );
		if(moveCtr || !drawn){
			DrawRect(pDC, rect);
		}
		else pCtrl->pdcHandler->DrawRectangle(CRectF(x1,y1,x2,y2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		pDC->SelectObject(pOldPen);
		pen.DeleteObject();
	}
	else{
		// Fill in
		CBrush	fill( lineColor );
		if(moveCtr || !drawn){
			pDC->FillRect( CRect(x1, y1, x2, y2), &fill );
		}
		else pCtrl->pdcHandler->FillRectangle(CRectF(x1,y1,x2,y2),lineColor, pDC);
		
		fill.DeleteObject();
	}
	


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle( CRectF(x1-3,y1-3,x1+3,y1+3), lineColor, pDC );		
		pCtrl->pdcHandler->FillRectangle( CRectF(x1+3,y2+3,x1-3,y2-3), lineColor, pDC );
		pCtrl->pdcHandler->FillRectangle( CRectF(x2-3,y1-3,x2+3,y1+3), lineColor, pDC );
		pCtrl->pdcHandler->FillRectangle( CRectF(x2+3,y2+3,x2-3,y2-3), lineColor, pDC );		
		br.DeleteObject();
	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyRectangle::OnLButtonUp(CPoint point)
{
#ifdef _CONSOLE_DEBUG
	printf("\nRectangle::OnLButtonUp()");
#endif

	if(drawing) return; // The object is being resized	

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

	// The left button was released so fire the click event
	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);
		pCtrl->SaveUserStudies();
	}	

	
	// See if the object has been clicked
	bool clicked = IsRegionClicked();


	if(selected && oldRect.right > 0) // the object was just resized
	{

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
		newRect = CRectF(0,0,0,0);
		oldRect = newRect;
  		
		// Snap the object to the nearest bars
		Reset();

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
	pCtrl->movingObject = false;
	moveCtr = false;

}



void CLineStudyRectangle::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	if(IsRegionClicked() && selectable){

		pCtrl->UnSelectAll(); // Unselect everything else
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRectF(0,0,0,0);
			newRect = oldRect;
			pCtrl->movingObject = true;				
			pCtrl->RePaint();
			if(buttonState != MOUSE_DOWN){
				startX = pointF.x; // Save the starting location
				startY = pointF.y;
			}

			// Can we move this line?
			if(pointF.x > x1-15 && pointF.y > y1-15 && pointF.x < x1+15 && pointF.y < y1+15){
				m_Move = cnTopLeft;	
			}
			else if(pointF.x > x2-15 && pointF.y > y1-15 && pointF.x < x2+15 && pointF.y < y1+15){
				m_Move = cnTopRight;				
			}
			else if(pointF.x > x2-15 && pointF.y > y2-15 && pointF.x < x2+15 && pointF.y < y2+15){
				m_Move = cnBottomRight;				
			}
			else if(pointF.x > x1-15 && pointF.y > y2-15 && pointF.x < x1+15 && pointF.y < y2+15){
				m_Move = cnBottomLeft;
			}

			else{
				if(IsRegionClicked()){
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


void CLineStudyRectangle::XORDraw(UINT nFlags, CPoint point)
{	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(nFlags == 1){ // First point
	    pointF.x = RealPointX(pointF.x);
	    pointF.y = RealPointY(pointF.y);
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
	}
	else if(nFlags == 2){ // Drawing the Rectangle
		
		CDC* pDC = pCtrl->GetScreen();
		pDC->SetROP2(R2_NOT);
		
		ExcludeRects(pDC);
		
		
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;
		CRect ioldRect = CRect(oldRect.left,oldRect.top,oldRect.right,oldRect.bottom);
		if(!(ioldRect.left==0 && ioldRect.top==0 && ioldRect.right==0 && ioldRect.bottom==0)) DrawRect(pDC, ioldRect); // Clear the previous drawing
		/*pDC->MoveTo(ioldRect.left, ioldRect.top);
		pDC->LineTo(ioldRect.right, ioldRect.top);
		pDC->MoveTo(ioldRect.left, ioldRect.bottom);
		pDC->LineTo(ioldRect.right, ioldRect.bottom);
		pDC->MoveTo(ioldRect.right, ioldRect.top);
		pDC->LineTo(ioldRect.right, ioldRect.bottom);
		pDC->MoveTo(ioldRect.left, ioldRect.top);
		pDC->LineTo(ioldRect.left, ioldRect.bottom);*/

		
		newRect.left = startX;
		newRect.right = pointF.x;
		newRect.top = startY;
		newRect.bottom = pointF.y;

		if(newRect.right < x1){
			newRect.left = x1 + 5;
			pointF.x = x1 + 5;
		}

	 
		
		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
	
		CRect inewRect = CRect(newRect.left,newRect.top,newRect.right,newRect.bottom);

		// To keep from drawing over things
		ExcludeRects(pDC);

		DrawRect(pDC, inewRect);
		/*pDC->MoveTo(inewRect.left, inewRect.top);
		pDC->LineTo(inewRect.right, inewRect.top);
		pDC->MoveTo(inewRect.left, inewRect.bottom);
		pDC->LineTo(inewRect.right, inewRect.bottom);
		pDC->MoveTo(inewRect.right, inewRect.top);
		pDC->LineTo(inewRect.right, inewRect.bottom);
		pDC->MoveTo(inewRect.left, inewRect.top);
		pDC->LineTo(inewRect.left, inewRect.bottom);*/


		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;
		pCtrl->ReleaseScreen(pDC);

	}
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
#ifdef _CONSOLE_DEBUG
		printf("\nRectangle::XorDraw(3)");
#endif
		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
		double temp = 0;
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
		drawn = true;

		/*	SGC	03.06.2004	BEG	*/		
		this->OnPaint( pCtrl->GetScreen() );
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		pCtrl->OnUserDrawingComplete(lsRectangle, key);
		pCtrl->SaveUserStudies();

	}
}

void CLineStudyRectangle::OnMouseMove(CPoint point)
{
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		return;
	}


	bool clicked = IsRegionClicked();	// Is the mouse within clicking distance?
	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key); // Then fire MouseMove

	
	// Is the mouse pointer over a corner?
	if(selected && m_Move == cnNone){
		if(pointF.x > x1-8 && pointF.y > y1-8 && pointF.x < x1+8 && pointF.y < y1+8){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));
		}
		else if(pointF.x > x2-8 && pointF.y > y1-8 && pointF.x < x2+8 && pointF.y < y1+8){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));				
		}
		else if(pointF.x > x2-8 && pointF.y > y2-8 && pointF.x < x2+8 && pointF.y < y2+8){		
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));
		}
		else if(pointF.x > x1-5 && pointF.y > y2-5 && pointF.x < x1+5 && pointF.y < y2+5){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));
		}
		else{
			if(IsRegionClicked()){
				// Not over a corner, what about the rest of the object?
				SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND_MOVE));
			}
			else{
				// Not over a corner or anything
				SetCursor(AfxGetApp()->LoadStandardCursor(IDC_ARROW));
			}
		}
	}


#ifdef OLD_BEHAVIOR
	// Display the tool top if we are allowed to
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
#endif

	if( !selected && clicked && !pCtrl->movingObject )
	{
		pCtrl->DelayMessage(guid, MSG_POINTOUT, 50); 
	}
	else
	{
		//Restore the line original state
		if (pointOutState) {
		  pointOutState = false;
		  pointOutStep = 0;
		  if (!selected && !pCtrl->movingObject) {
		    pCtrl->RePaint();
		  }
		}
	}

	// See if we are moving or resizing the object, if so, draw a XOR object
	if((m_Move != cnNone || m_Move == cnMoveAll) && buttonState == MOUSE_DOWN && selected){ // If moving

		CDC* pDC = pCtrl->GetScreen();
		
		pDC->SetROP2(R2_NOT); // XOR		


		// Draw the temporary object
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;

		// To keep from drawing over things
		ExcludeRects(pDC);

		//DrawRect(pDC, oldRect);
		pDC->MoveTo(oldRect.left, oldRect.top);
		pDC->LineTo(oldRect.right, oldRect.top);
		pDC->MoveTo(oldRect.left, oldRect.bottom);
		pDC->LineTo(oldRect.right, oldRect.bottom);
		pDC->MoveTo(oldRect.right, oldRect.top);
		pDC->LineTo(oldRect.right, oldRect.bottom);
		pDC->MoveTo(oldRect.left, oldRect.top);
		pDC->LineTo(oldRect.left, oldRect.bottom);
		
 
		// Flip coordinates if needed
		if(m_Move == cnTopLeft){
			if(pointF.x < x2){		// Don't flip
				newRect.left = pointF.x;
				newRect.right = x2;
				x1 = pointF.x;
				y1 = pointF.y;		
			}
			else {					// Flip
				newRect.left = oldRect.left;
				newRect.right = pointF.x;
				x1 = x2;
				x2 = pointF.x;
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnTopRight){
			if(pointF.x > x1){
				newRect.right = pointF.x;
				newRect.left = x1;
				x2 = pointF.x;
				y1 = pointF.y;
			}
			else {
				newRect.right = oldRect.right;
				newRect.left = pointF.x;
				x2 = x1;
				x1 = newRect.left;							
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnBottomRight){
			if(pointF.x >= x1){
				newRect.right = pointF.x;
				newRect.left = x1;
				x2 = pointF.x;
				y2 = pointF.y;
			}
			else {
				newRect.right = x1;
				newRect.left = pointF.x;
				x2 = x1;
				x1 = newRect.left;
				y2 = y1;
				y1 = pointF.y;				
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnBottomLeft){
			if(pointF.x <= x2){
				newRect.left = pointF.x;
				newRect.right = x2;
				x1 = pointF.x;
				y2 = pointF.y;
			}
			else {
				newRect.left = x2;
				newRect.right = pointF.x;
				x1 = x2;
				x2 = newRect.right;
				y2 = y1;
				y1 = pointF.y;				
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnMoveAll){// Just move the entire line (don't resize it)
			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);
		}


		// To keep from drawing over things
		ExcludeRects(pDC);

		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
		//DrawRect(pDC, newRect);
		pDC->MoveTo(newRect.left, newRect.top);
		pDC->LineTo(newRect.right, newRect.top);
		pDC->MoveTo(newRect.left, newRect.bottom);
		pDC->LineTo(newRect.right, newRect.bottom);
		pDC->MoveTo(newRect.right, newRect.top);
		pDC->LineTo(newRect.right, newRect.bottom);
		pDC->MoveTo(newRect.left, newRect.top);
		pDC->LineTo(newRect.left, newRect.bottom);	
		
		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyRectangle::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyRectangle::OnDoubleClick(CPoint point)
{	
	pointOutState = false;
	pointOutStep = 0;
	pCtrl->RePaint();
	if (selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
		selected = false;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
}
/////////////////////////////////////////////////////////////////////////////

// Workaround
// Simply create a CLineStudyRectangle, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyRectangle::SnapLine()
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

void CLineStudyRectangle::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}

	switch(MsgID){
		case MSG_POINTOUT:
		    if( !selected && IsRegionClicked() && !pCtrl->movingObject) {
			  if (!pointOutState) {
				pointOutState = true;
				pointOutStep = 1;
			    pCtrl->RePaint();
			  }
			}
			else {
			  if (pointOutState) {
			    pointOutState = false;
				pointOutStep = 0;
			    pCtrl->RePaint();
			  }
			}

			break;

#ifdef OLD_BEHAVIOR
		case MSG_TOOLTIP:
			DisplayInfo();

			break;
#endif
	
		default:
			break;
	}
}

void CLineStudyRectangle::DisplayInfo()
{
	if(drawing) return; // The object is being resized
	if(!IsRegionClicked()) return;
	CPoint point = pCtrl->m_point;
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	CString temp;
	CString text;
	CString datetime = "";
	if(ownerPanel->series.size() == 0) return;
	double revX = (ownerPanel->GetReverseX(x1) + pCtrl->startIndex);
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		double future =(ownerPanel->GetReverseX(x1) - pCtrl->RecordCount() + pCtrl->startIndex);
		temp.Format("%d", future);
		datetime = temp + " periods into future";
	}
	text += "X1   " + datetime + char(13) + char(10);
	revX = ownerPanel->GetReverseX(x2) + pCtrl->startIndex;
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		double future = ownerPanel->GetReverseX(x2) - pCtrl->RecordCount() + pCtrl->startIndex;
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
	m_valueView.x1 = pointF.x + 10;
	m_valueView.y1 = pointF.y + 21;
	m_valueView.x2 = pointF.x + 155;
	m_valueView.y2 = pointF.y + 88;
	m_valueView.Show();
}
