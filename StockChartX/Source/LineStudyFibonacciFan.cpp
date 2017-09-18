// LineStudyFibonacciFan.cpp: implementation of the CLineStudyFibonacciFan class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFibonacciFan.h"
#include "Coordinates.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyFibonacciFan::CLineStudyFibonacciFan(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{

	// Object type and description
	objectType = "FibonacciFan";
	nType = lsFibonacciFan;
	objectDescription = "Fibonacci Fan";

	// Initialize specific fields for this study
	x1_2 = 0;
	x2_2 = 0;
	y1_2 = 0;
	y2_2 = 0;
	x1_3 = 0;
	x2_3 = 0;
	y1_3 = 0;
	y2_3 = 0;
	x1_4 = 0;
	x2_4 = 0;
	y1_4 = 0;
	y2_4 = 0;
	
	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = CRect(0,0,0,0);


	// Standard initialization
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
	startX = 0;
	startY = 0;
	key = Key;
	
	lineWeight = pCtrl->lineThickness;	
}

CLineStudyFibonacciFan::~CLineStudyFibonacciFan()
{

}

void CLineStudyFibonacciFan::OnPaint(CDC *pDC)
{

	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}
	

	Update();
	// Create hit test regions for mouse clicks on the object
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5);
	
	
	// Draw the object
	CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	DrawLineStudy(pDC, CRect(x1,y1,x2,y2));
	pDC->SelectObject(pOldPen);
	pen.DeleteObject();


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y1+3,x1-3,y1-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y2+3,x1-3,y2-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y1+3,x2-3,y1-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();

	}
	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciFan::OnLButtonUp(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(drawing) return; // The object is being resized	

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

	// The left button was released so fire the click event
	if(selected){
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
		pCtrl->SaveUserStudies();
	}

	
	// See if the object has been clicked
	bool clicked = IsObjectClicked();


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
		//newRect = CRect(0,0,0,0);
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



void CLineStudyFibonacciFan::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	m_valueView.Reset(true); // Hide the tool tip

	bool clicked = IsObjectClicked();

	if(clicked && selectable){
		pCtrl->UnSelectAll(); // Unselect everything else
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRect(0,0,0,0);
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
				if(clicked){
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


void CLineStudyFibonacciFan::XORDraw(UINT nFlags, CPoint point)
{	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);
			
	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);

	if(nFlags == 1){ // First point
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
	}
	else if(nFlags == 2){ // Drawing the FibonacciFan
		
		CDC* pDC = pCtrl->GetScreen();
	 
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;
		pDC->SetROP2(R2_NOT);
		DrawLineStudy(pDC, oldRect); // Clear the previous drawing
		
		if (pointF.x < startX+15){
			pointF.x = RealPointX(startX+15);
		}

		newRect.left = startX;
		newRect.right = pointF.x;
		newRect.top = startY;
		newRect.bottom = pointF.y;

	
		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
	
		DrawLineStudy(pDC, newRect);

		oldRect = newRect;
		pDC->SetROP2(R2_COPYPEN);		
		pCtrl->ReleaseScreen(pDC);

	}
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
		if (pointF.x < startX+15){
			pointF.x = RealPointX(startX+15);
		}

		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
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
		drawn=true;
		/*	SGC	03.06.2004	BEG	*/		
		this->OnPaint( pCtrl->GetScreen() );
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		pCtrl->OnUserDrawingComplete(lsFibonacciFan, key);
		pCtrl->SaveUserStudies();

	}
}

void CLineStudyFibonacciFan::OnMouseMove(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		return;
	}

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	bool clicked = IsObjectClicked();

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
			if(clicked){
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

	if ( !selected && clicked && !pCtrl->movingObject )
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
		

		DrawLineStudy(pDC, oldRect);
		
#ifdef OLD_BEHAVIOR
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
#endif

	   if (m_Move == cnBottomRight || m_Move == cnTopLeft || m_Move == cnTopRight || m_Move == cnBottomLeft) {		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
			if(pointF.x >= x1+15){
				newRect.right = pointF.x;
				newRect.left = x1;
				x2 = pointF.x;
				y2 = pointF.y;
			}
			else {
				pointF.x = RealPointX(x1+15);
				newRect.right = pointF.x;
				newRect.left = x1;
				x2 = pointF.x;
				y2 = pointF.y;

				//newRect.right = x1;
				//newRect.left = pointF.x;
				//x2 = x1;
				//x1 = newRect.left;
				//y2 = y1;
				//y1 = pointF.y;				
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(m_Move == cnMoveAll) { //|| m_Move == cnTopLeft || m_Move == cnTopRight || m_Move == cnBottomLeft){// Just move the entire object (don't resize it)
			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);
		}

		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
		DrawLineStudy(pDC, newRect);	
		
		oldRect = newRect;
		pDC->SetROP2(R2_COPYPEN);
		pCtrl->ReleaseScreen(pDC);		

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciFan::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciFan::OnDoubleClick(CPoint point)
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

// Simply create a CLineStudyFibonacciFan, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFibonacciFan::SnapLine()
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

void CLineStudyFibonacciFan::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}

	switch(MsgID){
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

#ifdef OLD_BEHAVIOR
		case MSG_TOOLTIP:
			DisplayInfo();

			break;
#endif
	
		default:
			break;
	}
}

void CLineStudyFibonacciFan::DisplayInfo()
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
	int revX = (int)(ownerPanel->GetReverseX(x1) + pCtrl->startIndex);
	datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	if(datetime == "Invalid DateTime."){
		int future = (int)(ownerPanel->GetReverseX(x1) - pCtrl->RecordCount() + pCtrl->startIndex);
		temp.Format("%d", future);
		datetime = temp + " periods into future";
	}
	text = objectDescription + char(13) + char(10);
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
	m_valueView.x1 = pointF.x + 10;
	m_valueView.y1 = pointF.y + 21;
	m_valueView.x2 = pointF.x + 155;
	m_valueView.y2 = pointF.y + 100;
	m_valueView.Show();
}

void CLineStudyFibonacciFan::DrawLineStudy(CDC *pDC, CRect rect)
{

	if(rect == CRect(0,0,0,0)) return;

	// Don't draw over scales or other panels
	ExcludeRects(pDC);

	//Test variables
	double xt1, xt2, yt1, yt2;
	double yt3, yt4, yt5 = 0;
	Coordinates coord;


	xt1 = rect.left;
	xt2 = rect.right;
	yt1 = rect.top;
	yt2 = rect.bottom;

#ifdef OLD_BEHAVIOR

	int value = 0;
	double b = 0;
	double c = 0;
	double x = 0; double cx = 0;
	double y = 0; double cy = 0;	

	value = (rect.bottom - rect.top) / 3;

	rect.bottom += 15;
	b = double(rect.top - rect.bottom) / double(rect.left - rect.right);
	c = rect.top - b * rect.left;
	cx = pCtrl->width - pCtrl->yScaleWidth;
	cy = (b * cx + c) + cy * 0.3;	
	y = rect.top;
	x = rect.left;
	pDC->MoveTo(x,y);
	pDC->LineTo(cx,cy);	
	x1_2 = x;
	x2_2 = cx;
	y1_2 = y;
	y2_2 = cy;

	b = double((rect.top + value * 1) - rect.bottom) / double(rect.left - rect.right);
	c = (rect.top + value * 1) - b * rect.left;
	cx = pCtrl->width - pCtrl->yScaleWidth;
	cy = (b * cx + c)+ cy * 0.5;
	y = rect.top;
	x = rect.left;
	x1_3 = x;
	x2_3 = cx;
	y1_3 = y;
	y2_3 = cy;
	pDC->MoveTo(x,y);
	pDC->LineTo(cx,cy);	

	b = double(rect.top + value * 2 - rect.bottom) / double(rect.left - rect.right);
	c = rect.top + value * 2 - b * rect.left;
	cx = pCtrl->width - pCtrl->yScaleWidth;
	cy = (b * cx + c)+ cy * 0.8;
	y = rect.top;
	x = rect.left;
	x1_4 = x;
	x2_4 = cx;
	y1_4 = y;
	y2_4 = cy;
	pDC->MoveTo(x,y);
	pDC->LineTo(cx,cy);
	rect.bottom -= 15;

	// Draw the handle
	pDC->MoveTo(rect.left, rect.top);
	pDC->LineTo(rect.right, rect.bottom);

#endif

	//new calculus for the fibonacciFan
	double dx = abs(yt2 - yt1);
	if(yt1 > yt2){
		yt3 = yt1 - dx/2;
	}else {
		yt3 = yt1 + dx/2;
	}
	yt4 = yt3 + 0.118*dx;
	yt5 = yt3 - 0.118*dx;
	//first line
	double dxt = (xt2 - xt1);
	double dyt = (yt3 - yt1);

	//Make sure that this won't screw up the program, this behavior is similar to the one it was used, so...
	double xp1, xp2, yp1, yp2;
	xp1 = ownerPanel->GetX(0);
	xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
	yp1 = ownerPanel->y1;
	yp2 = ownerPanel->y2;
	double radiusSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));
	CPointF p = coord.MovePolar(xt1,yt1, radiusSize, atan(dyt/dxt)); 
	//first line
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(xt1,yt1);
		pDC->LineTo((int)p.x,(int)p.y);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),p,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	x2_2 = p.x;
	y2_2 = p.y;
	//second line
	dxt = (xt2 - xt1);
	dyt = (yt4 - yt1);
	p = coord.MovePolar(xt1,yt1, radiusSize, atan(dyt/dxt)); 
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(xt1,yt1);
		pDC->LineTo((int)p.x,(int)p.y);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),p,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	x2_3 = p.x;
	y2_3 = p.y;
	//third line
	dxt = (xt2 - xt1);
	dyt = (yt5 - yt1);
	p = coord.MovePolar(xt1,yt1, radiusSize, atan(dyt/dxt)); 
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(xt1,yt1);
		pDC->LineTo((int)p.x,(int)p.y);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),p,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	x2_4 = p.x;
	y2_4 = p.y;
		
	//Line Study
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(xt1,yt1);
		pDC->LineTo(xt2,yt2);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),CPointF(xt2,yt2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);

	IncludeRects(pDC);
}

// Special click function for this object
bool CLineStudyFibonacciFan::IsObjectClicked()
{
	bool clicked = IsRegionClicked();
	if(!clicked) clicked = IsClicked(x1,y1,x2,y2) ||
		IsClicked(x1,y1,x2_2,y2_2) ||
		IsClicked(x1,y1,x2_3,y2_3) ||
		IsClicked(x1,y1,x2_4,y2_4);
	
	return clicked;
}