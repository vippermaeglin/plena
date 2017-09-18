// LineStudyFibonacciTimeZones.cpp: implementation of the CLineStudyFibonacciTimeZones class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFibonacciTimeZones.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyFibonacciTimeZones::CLineStudyFibonacciTimeZones(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "FibonacciTimeZones";
	nType = lsFibonacciTimeZones;
	objectDescription = "Fibonacci Time Zones";
	
	fibLines.resize(7);
	fib.resize(7);
	fib[0] = 0.618;
	fib[1] = 0.5;
	fib[2] = 0.382;
	fib[3] = -0.382;
	fib[4] = -0.5;
	fib[5] = -0.618;
	fib[6] = -1.618;
	
	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = CRectF(0,0,0,0);

	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	lineColor = color;
	Initialize();	
	drawing = false;
	drawn = false;
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

CLineStudyFibonacciTimeZones::~CLineStudyFibonacciTimeZones()
{

}

void CLineStudyFibonacciTimeZones::OnPaint(CDC *pDC)
{

	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}

	Update();
	ExcludeRects(pDC);

	// Create hit test regions for mouse clicks on the object
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a Rectangle
	
	
	// Draw the object
	CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	DrawLineStudy(pDC, CRectF(x1,y1,x2,y2));
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
		br.DeleteObject();
	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciTimeZones::OnLButtonUp(CPoint point)
{

	if(drawing) return; // The object is being resized	

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
		pCtrl->SaveUserStudies();
		
		pCtrl->UpdateScreen(false);	
	}

	buttonState = MOUSE_UP; // The mouse is up
	pCtrl->dragging = false; // The mouse is up so we're not dragging anything
	pCtrl->movingObject = false;

}



void CLineStudyFibonacciTimeZones::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	if(IsObjectClicked() && selectable){
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
				if(IsObjectClicked()){
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


void CLineStudyFibonacciTimeZones::XORDraw(UINT nFlags, CPoint point)
{				
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	if(nFlags == 1){ // First pointF
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
		DrawLineStudy(pDC, oldRect); // Clear the previous drawing
		
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
	
		// To keep from drawing over things
		ExcludeRects(pDC);

		DrawLineStudy(pDC, newRect);


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
		
		pCtrl->OnUserDrawingComplete(lsFibonacciTimeZones, key);
		pCtrl->SaveUserStudies();

	}
}

void CLineStudyFibonacciTimeZones::OnMouseMove(CPoint point)
{
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		return;
	}


	bool clicked = IsObjectClicked();	// Is the mouse within clicking distance?
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
			if(IsObjectClicked()){
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

		DrawLineStudy(pDC, oldRect);
		
 
		// Flip coordinates if needed
		if(m_Move == cnTopLeft){		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
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
			//Set Magnetic Y
			if(pCtrl->m_Magnetic)pointF.y = MagneticPointY(pointF.y, pointF.x);
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
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
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
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
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
		DrawLineStudy(pDC, newRect);	
		
		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciTimeZones::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciTimeZones::OnDoubleClick(CPoint point)
{	
	//Restore the line original state
	if (pointOutState) {
		pointOutState = false;
		pointOutStep = 0;
		pCtrl->RePaint();
	}
	if(selected) pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
}
/////////////////////////////////////////////////////////////////////////////

// Workaround
// Simply create a CLineStudyFibonacciTimeZones, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFibonacciTimeZones::SnapLine()
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

void CLineStudyFibonacciTimeZones::OnMessage(LPCTSTR MsgGuid, int MsgID)
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

void CLineStudyFibonacciTimeZones::DisplayInfo()
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


void CLineStudyFibonacciTimeZones::DrawLineStudy(CDC* pDC, CRectF rect)
{
	
	if(m_Move!=cnNone || !drawn){
		pDC->MoveTo(rect.left, rect.top);
		pDC->LineTo(rect.left, rect.bottom);
		pDC->MoveTo(rect.right, rect.top);
		pDC->LineTo(rect.right, rect.bottom);
	}
	else {
		pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top),CPointF(rect.left,rect.bottom),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		pCtrl->pdcHandler->DrawLine(CPointF(rect.right,rect.top),CPointF(rect.right,rect.bottom),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	}

	CPen* pen = new CPen(PS_DOT, lineWeight+pointOutStep, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	for(int n = 0; n != 7; ++n){
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(rect.left + rect.Width() * (1-fib[n]), rect.top);
			pDC->LineTo(rect.left + rect.Width() * (1-fib[n]), rect.bottom);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(rect.left + rect.Width() * (1-fib[n]), rect.top),CPointF(rect.left + rect.Width() * (1-fib[n]), rect.bottom),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		
		fibLines[n].x1 = rect.left + rect.Width() * (1-fib[n]);
		fibLines[n].y1 = rect.top;
		fibLines[n].x2 = rect.left + rect.Width() * (1-fib[n]);
		fibLines[n].y2 = rect.bottom;
	}

	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

}


// Special click function for this object
bool CLineStudyFibonacciTimeZones::IsObjectClicked()
{
	bool clicked = IsRegionClicked();
	if(!clicked){
		for(int n = 0; n != 7; ++n){
			clicked = IsClicked(fibLines[n].x1,fibLines[n].y1,fibLines[n].x2,fibLines[n].y2);
			if(clicked) break;
		}
	}
	return clicked;
}