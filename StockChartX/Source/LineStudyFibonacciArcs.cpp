// LineStudyFibonacciArcs.cpp: implementation of the CLineStudyFibonacciArcs class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFibonacciArcs.h"
#include "Coordinates.h"
#include "julian.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyFibonacciArcs::CLineStudyFibonacciArcs(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{

	objectType = "FibonacciArcs";
	nType = lsFibonacciArcs;
	objectDescription = "Fibonacci Arcs";

	// Initialize specific fields for this study

	arcs.resize(3);
	for(int n = 0; n < 3; n++){
		arcs[n].x1 = 0;
		arcs[n].y1 = 0;
		arcs[n].x2 = 0;
		arcs[n].y2 = 0;
	}	
	x1 = y1 = x2 = y2 = 0.0f;
	oldRect = newRect = CRectF(0.0f,0.f,0.0f,0.0f);

	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	lineColor = color;
	Initialize();	
	drawing = false;
	drawn=false;
	displayed = false;
	xor = false;
	dragging = false;	
	m_Move = cnNone;	
	buttonState = 0;
	startX = 0.0f;
	startY = 0.0f;
	key = Key;
	lineWeight = pCtrl->lineThickness;	
}

CLineStudyFibonacciArcs::~CLineStudyFibonacciArcs()
{

}

void CLineStudyFibonacciArcs::OnPaint(CDC *pDC)
{

	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}

	Update();
	// Create hit test regions for mouse clicks on the object or bounding
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn((int)x1, (int)y1, (int)x2, (int)y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1+5, y1+5, x2-5, y2-5);
	
	
	// Draw the object
	CPen	pen( lineStyle, lineWeight + pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	
	DrawLineStudy(pDC, &CRectF(x1,y1,x2,y2), &CRectF(x2,y2,x2,y2));	


	pDC->SelectObject(pOldPen);
	pen.DeleteObject();


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y1+3,x1-3,y1-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();

	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciArcs::OnLButtonUp(CPoint point)
{

	if(drawing) return; // The object is being resized	

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

	// The left button was released so fire the click event
	if(selected){
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
		newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
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
		pCtrl->SaveUserStudies();	

	}

	buttonState = MOUSE_UP; // The mouse is up
	pCtrl->dragging = false; // The mouse is up so we're not dragging anything
	pCtrl->movingObject = false;

}



void CLineStudyFibonacciArcs::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	if(IsObjectClicked() && selectable){
		pCtrl->UnSelectAll(); // Unselect everything else
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRectF(0.0f,0.0f,0.0f,0.0f);
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
				//m_Move = cnTopRight;	
				m_Move = cnMoveAll;
			}
			else if(pointF.x > x2-15 && pointF.y > y2-15 && pointF.x < x2+15 && pointF.y < y2+15){
				m_Move = cnBottomRight;				
			}
			else if(pointF.x > x1-15 && pointF.y > y2-15 && pointF.x < x1+15 && pointF.y < y2+15){
				//m_Move = cnBottomLeft;
				m_Move = cnMoveAll;
			}
			else m_Move = cnMoveAll;

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


void CLineStudyFibonacciArcs::XORDraw(UINT nFlags, CPoint point)
{	
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
	if(nFlags == 1){ // First point
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
	}
	else if(nFlags == 2){ // Drawing the Fibonacci Arcs
		
		CDC* pDC = pCtrl->GetScreen();
		pDC->SetROP2(R2_NOT);
		
		DrawLineStudy(pDC, &oldRect);

		newRect.left = startX;
		newRect.right = pointF.x;
		newRect.top = startY;
		newRect.bottom = pointF.y;
	 
		DrawLineStudy(pDC, &newRect);

		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;
		pCtrl->ReleaseScreen(pDC);

	}	
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
	/*	int temp = 0;
		if(x1 > x2)
		{
			temp = x2;
			x2 = x1;
			x1 = temp;
			temp = y2;
			y2 = y1;
			y1 = temp;
		}*/
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
		
		pCtrl->OnUserDrawingComplete(lsFibonacciArcs, key);
		pCtrl->SaveUserStudies();

	}
}


void CLineStudyFibonacciArcs::OnMouseMove(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		return;
	}
	

	bool clicked = IsObjectClicked();	// Is the mouse within clicking distance?
	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key); // Then fire MouseMove

	// Add pointoutmsg
	if( !selected && clicked && !pCtrl->movingObject )
	{

#ifdef OLD_BEHAVIOR
		displayed = true;
		pCtrl->textDisplayed = true;
		pCtrl->DelayMessage(guid, MSG_TOOLTIP, 1000); 
#endif

		pCtrl->DelayMessage(guid, MSG_POINTOUT, 50); 
	}
	else
	{

#ifdef OLD_BEHAVIOR

		if( displayed )
			pCtrl->textDisplayed = false;
		
		displayed	= false;
		
		/*	SGC	03.06.2004	BEG*/
		m_valueView.Connect( pCtrl );
		/*	SGC	03.06.2004	END*/
		
		m_valueView.Reset(true);	
#endif

		//Restore the line original state
		if (pointOutState) {
		  pointOutState = false;
		  pointOutStep = 0;
		  if (!selected && !pCtrl->movingObject) {
		    pCtrl->RePaint();
		  }
		}
	}
	
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



	// See if we are moving or resizing the object, if so, draw a XOR object
	if((m_Move != cnNone || m_Move == cnMoveAll) && buttonState == MOUSE_DOWN && selected){ // If moving

		CDC* pDC = pCtrl->GetScreen();
		
		pDC->SetROP2(R2_NOT); // XOR	
		DrawLineStudy(pDC, &oldRect);	

		
		
 
		// Flip coordinates if needed
		if(m_Move == cnTopLeft){//		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
			//if(point.x < x2){		// Don't flip
				newRect.left = pointF.x;
				newRect.right = x2;
				x1 = pointF.x;
				y1 = pointF.y;		
			//}
			/*else {					// Flip
				newRect.left = oldRect.left;
				newRect.right = pointF.x;
				x1 = x2;
				x2 = pointF.x;
			}*/
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnTopRight){		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, point.x);
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
		else if(m_Move == cnBottomRight){//	
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)point.y = MagneticPointY(point.y, point.x);
			//if(point.x >= x1){
				newRect.right = point.x;
				newRect.left = x1;
				x2 = point.x;
				y2 = point.y;
			//}
			/*else {
				newRect.right = x1;
				newRect.left = point.x;
				x2 = x1;
				x1 = newRect.left;
				y2 = y1;
				y1 = point.y;				
			}*/
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnBottomLeft){			
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)point.y = MagneticPointY(point.y, point.x);
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

		DrawLineStudy(pDC, &newRect);			
		
		pDC->SetROP2(R2_COPYPEN);
		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;		

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciArcs::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciArcs::OnDoubleClick(CPoint point)
{	
	//Restore the line original state
	if (pointOutState) {
		pointOutState = false;
		pointOutStep = 0;
		pCtrl->RePaint();
	}
	if (selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
		selected = false;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
}
/////////////////////////////////////////////////////////////////////////////

// Workaround
// Simply create a CLineStudyFibonacciArcs, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFibonacciArcs::SnapLine()
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

void CLineStudyFibonacciArcs::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) return;
	switch(MsgID){
		case MSG_TOOLTIP:
			DisplayInfo();
			break;
		 //Added MSGPOINTOUT treatment
		case MSG_POINTOUT:
			if( !selected && IsObjectClicked() && !pCtrl->movingObject) {
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
	}
}

void CLineStudyFibonacciArcs::DisplayInfo()
{
	if(drawing) return; // The object is being resized

	bool clicked = IsObjectClicked();

	if(!clicked) return;
	CPoint point = pCtrl->m_point;
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	CString temp;
	CString text;
	CString datetime = "";
	if(ownerPanel->series.size() == 0) return;
	double revX = (ownerPanel->GetReverseX(x1) + pCtrl->startIndex);
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		double future = (ownerPanel->GetReverseX(x1) - pCtrl->RecordCount() + pCtrl->startIndex);
		temp.Format("%d", future);
		datetime = temp + " periods into future";
	}
	text = objectDescription + char(13) + char(10);
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
	m_valueView.Text = text;
	m_valueView.Connect(pCtrl);
	m_valueView.overidePoints = true;
	m_valueView.x1 = pointF.x + 10;
	m_valueView.y1 = pointF.y + 21;
	m_valueView.x2 = pointF.x + 155;
	m_valueView.y2 = pointF.y + 87;
	m_valueView.Show();
}


void CLineStudyFibonacciArcs::DrawLineStudy(CDC *pDC, CRectF* pRect, CRectF* pArc /*=NULL*/)
{

	if(pRect->bottom == 0 || pRect->right == 0) return;
	if(pDC == NULL || pRect == NULL) return;
	
	// Don't draw over scales or other panels
	ExcludeRects(pDC);
	
	double radius;
	double distance;
	Coordinates coord;
	CRect boundRect;

	//Calculate radius based on fibonnaci distance
	double xp1 = pRect->left;
	double yp1 = pRect->top;
	double xp2 = pRect->right;
	double yp2 = pRect->bottom;


	distance = sqrt(pow((double) xp2 - xp1, (double) 2)+pow( (double) yp2 - yp1, (double) 2));
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(xp1,yp1);
		pDC->LineTo(xp2,yp2);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xp1,yp1),CPointF(xp2,yp2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);

	//If superior quadrants
	if(yp1 > yp2){
		//Calculate first arc
		radius = distance*0.5;
		CRectF boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4),coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		boundRectF = CRectF(boundRectF.left,boundRectF.top,boundRectF.right,boundRectF.bottom);

		//pDC->Draw3dRect(boundRect,lineColor,lineColor);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2-radius,yp2), CPoint(xp2+radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2-radius,yp2),CPointF(xp2+radius, yp2),false,lineWeight+pointOutStep,lineStyle,lineColor,pDC);

		arcs[0].x1 = boundRect.left;
		arcs[0].y1 = boundRect.top;
		arcs[0].x2 = boundRect.right;
		arcs[0].y2 = boundRect.bottom;
		//Calculate second arc
		radius = distance*0.382;
		boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2-radius,yp2), CPoint(xp2+radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2-radius,yp2),CPointF(xp2+radius, yp2),false,lineWeight+pointOutStep,lineStyle,lineColor,pDC);
		arcs[1].x1 = boundRect.left;
		arcs[1].y1 = boundRect.top;
		arcs[1].x2 = boundRect.right;
		arcs[1].y2 = boundRect.bottom;
		//Calculate third arc
		radius = distance*0.618;
		boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2-radius,yp2), CPoint(xp2+radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2-radius,yp2),CPointF(xp2+radius, yp2),false,lineWeight+pointOutStep,lineStyle,lineColor,pDC);
		arcs[2].x1 = boundRect.left;
		arcs[2].y1 = boundRect.top;
		arcs[2].x2 = boundRect.right;
		arcs[2].y2 = boundRect.bottom;
		//Calculate third arc
		/*radius = distance*0.618;
		boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2-radius,yp2), CPoint(xp2+radius, yp2) );
		}
		//else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2-radius,yp2),CPointF(xp2+radius, yp2),false,lineWeight+pointOutStep,lineStyle,lineColor);
		arcs[2].x1 = boundRect.left;
		arcs[2].y1 = boundRect.top;
		arcs[2].x2 = boundRect.right;
		arcs[2].y2 = boundRect.bottom;*/
	} else {
		//Calculate first arc
		radius = distance*0.5;
		CRectF boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2+radius,yp2), CPoint(xp2-radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2+radius,yp2),CPointF(xp2-radius, yp2),true,lineWeight+pointOutStep,lineStyle,lineColor,pDC);
		arcs[0].x1 = boundRect.left;
		arcs[0].y1 = boundRect.top;
		arcs[0].x2 = boundRect.right;
		arcs[0].y2 = boundRect.bottom;
		//Calculate second arc
		radius = distance*0.382;
		boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2+radius,yp2), CPoint(xp2-radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2+radius,yp2),CPointF(xp2-radius, yp2),true,lineWeight+pointOutStep,lineStyle,lineColor,pDC);
		arcs[1].x1 = boundRect.left;
		arcs[1].y1 = boundRect.top;
		arcs[1].x2 = boundRect.right;
		arcs[1].y2 = boundRect.bottom;
		//Calculate third arc
		radius = distance*0.618;
		boundRectF = CRectF(coord.MovePolar(xp2,yp2,sqrt(2.)*radius,5*coord.PI/4), 
			coord.MovePolar(xp2,yp2,sqrt(2.)*radius,coord.PI/4)) ;
		boundRect = CRect((int)boundRectF.left,(int)boundRectF.top,(int)boundRectF.right,(int)boundRectF.bottom);
		if( m_Move!=cnNone || !drawn){
			pDC->MoveTo(xp2,yp2);
			pDC->Arc( boundRect, CPoint(xp2+radius,yp2), CPoint(xp2-radius, yp2) );
		}
		else pCtrl->pdcHandler->DrawArc(boundRectF,CPointF(xp2,yp2),CPointF(xp2+radius,yp2),CPointF(xp2-radius, yp2),true,lineWeight+pointOutStep,lineStyle,lineColor,pDC);
		arcs[2].x1 = boundRect.left;
		arcs[2].y1 = (double) boundRect.top;
		arcs[2].x2 = boundRect.right;
		arcs[2].y2 = (double) boundRect.bottom;
	}



#ifdef OLD_BEHAVIOR
	CPoint pl, pr;

	// Lock radius
	if(pRect->Width() < pRect->Height()){
		int change = (pRect->Height() - pRect->Width()) / 2;
		pRect->right += change;
		pRect->left -= change;
	}
	else if(pRect->Height() < pRect->Width()){			
		int change = (pRect->Width() - pRect->Height()) / 2;
		pRect->bottom += change;
		pRect->top -= change;
	}

	pl.x = pr.x = pRect->right;
	pl.y = pr.y = pRect->bottom;

	if(pArc != NULL){
		pDC->Arc(pRect->left, pRect->top, pRect->right, pRect->bottom, 
			pArc->left, pArc->top, pArc->right, pArc->bottom);
	}
	else{
		pDC->Arc(pRect, pl, pr);
	}

	
	CRect rect;
	rect.top = pRect->top;
	rect.left = pRect->left;
	rect.bottom = pRect->bottom;
	rect.right = pRect->right;

	for(int n = 0; n != 2; ++n){
		pRect->left += (pRect->Width() * 0.1);
		pRect->right -= (pRect->Width() * 0.1);
		pRect->top += (pRect->Height() * 0.1);
		pRect->bottom -= (pRect->Height() * 0.1);		
		pl.x = pr.x = pRect->right;
		pl.y = pr.y = pRect->bottom;
		pDC->Arc(pRect, pl, pr);
		arcs[n].x1 = pRect->left;
		arcs[n].x2 = pRect->right;
		arcs[n].y1 = pRect->top;
		arcs[n].y2  = pRect->bottom;
	}

	pRect->top = rect.top; 
	pRect->left = rect.left; 
	pRect->bottom = rect.bottom; 
	pRect->right = rect.right;

#endif



 	IncludeRects(pDC);

}



// Special click function for this object
bool CLineStudyFibonacciArcs::IsObjectClicked()
{
	bool clicked;
	if(x1<x2)clicked  = IsClicked(x1,y1,x2,y2);	 
	else clicked = IsClicked(x2,y2,x1,y1);
	

	if(!clicked){
		for(int n = 0; n < arcs.size(); ++n){ // 3/1/08
			if(arcs[n].x1<arcs[n].x2){
				if(IsClicked(arcs[n].x1,arcs[n].y1,arcs[n].x2,arcs[n].y2)){
					return true;
				}
			}
			else{
				if(IsClicked(arcs[n].x2,arcs[n].y2,arcs[n].x1,arcs[n].y1)){
					return true;
				}
			}
		}
		
	}
	
	


	return clicked;
}



// Specialized version of Reset for this line study
void CLineStudyFibonacciArcs::Reset()
{
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	x1 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));
	y1 = ownerPanel->GetY(y1Value);
	y2 = ownerPanel->GetY(y2Value);
	MMDDYYHHMMSS x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
	MMDDYYHHMMSS x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
	switch(pCtrl->GetPeriodicity())
	{
		case Minutely:	
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			//x2Julian.Second -= 1;
			//x2Julian_2.Second -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
		case Hourly:	
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Minute -= 1;
			x2Julian_2.Minute -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
		case Daily:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Hour -= 1;
			x2Julian_2.Hour -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
		case Weekly:		
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
		case Month:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
		case Year:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = CJulian::ToJulianDate(x2Julian_2.Year,x2Julian_2.Month,x2Julian_2.Day,x2Julian_2.Hour,x2Julian_2.Minute,x2Julian_2.Second,x2Julian_2.Millisecond);
			break;
	}
}
