// LineStudyFibonacciProgression.cpp: implementation of the CLineStudyFibonacciProgression class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFibonacciProgression.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyFibonacciProgression::CLineStudyFibonacciProgression(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "FibonacciProgression";
	nType = lsFibonacciProgression;
	objectDescription = "Fibonacci Progression";

	fibLines.resize(10);
	fib.resize(10);
	fib[0] = 0.0557;
	fib[1] = 0.0902;
	fib[2] = 0.118;
	fib[3] = 0.1459;
	fib[4] = 0.2361;
	fib[5] = 0.381;
	fib[6] = 0.618;	
		

	x1 = y1 = x2 = y2 = x1_2 = y1_2 = x2_2 = y2_2 = 0;
	oldRect = newRect = CRect(0,0,0,0);
	oldRect_2 = newRect_2 = CRect(0,0,0,0);
	oldRect_3 = newRect_3 = CRect(0,0,0,0);
	state = 0;

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

	//new variables
	move1L = false;
	move2L = false;
	moveCtr = false;
	moveAll = false;

	//default for params	
	lineWeight = pCtrl->lineThickness;	
	/*params[0] = 2.618;
	params[1] = 1.618;
	params[2] = 1.0;
	params[3] = 0.618;
	params[4] = 0.5;
	params[5] = 0.382;
	params[6] = 0.0;*/
	
	params[0]=pCtrl->fibonacciProParams[0];
	params[1]=pCtrl->fibonacciProParams[1];
	params[2]=pCtrl->fibonacciProParams[2];
	params[3]=pCtrl->fibonacciProParams[3];
	params[4]=pCtrl->fibonacciProParams[4];
	params[5]=pCtrl->fibonacciProParams[5];
	params[6]=pCtrl->fibonacciProParams[6];
	params[7]=NULL_VALUE;
	params[8]=NULL_VALUE;
	params[9]=NULL_VALUE;
	
}

CLineStudyFibonacciProgression::~CLineStudyFibonacciProgression()
{

}

void CLineStudyFibonacciProgression::OnPaint(CDC *pDC)
{

	if(drawing || (selected && pCtrl->movingObject) ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}

	Update();
	ExcludeRects(pDC);

	//// Create hit test regions for mouse clicks on the object
	//m_testRgn1.DeleteObject();
	//m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	//m_testRgn2.DeleteObject();
	//m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a Rectangle
	//
	
	// Draw the object
	CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	DrawSimpleLine(pDC, CRect(x1,y1,x2,y2));
	DrawSimpleLine(pDC, CRect(x2,y2,x1_2,y1_2));
	DrawLineStudy(pDC, CRect(x1_2,y1_2,x2_2,y2_2));
	pDC->SelectObject(pOldPen);
	pen.DeleteObject();


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1_2-3,y1_2-3,x1_2+3,y1_2+3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x1_2+3,y2_2+3,x1_2-3,y2_2-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2_2-3,y1_2-3,x2_2+3,y1_2+3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2_2+3,y2_2+3,x2_2-3,y2_2-3),lineColor, pDC);
		
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();

	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciProgression::OnLButtonUp(CPoint point)
{

	if(drawing) return; // The object is being resized	

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving
	move1L = false;
	move2L = false;
	moveCtr = false;
	moveAll = false;

	// The left button was released so fire the click event
	if(selected) {
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
		x1_2 = newRect_3.left;
		x2_2 = newRect_3.right;		
		y1_2 = newRect_3.top;
		y2_2 = newRect_3.bottom;


		// We're not drawing anymore so reset the state
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->movingObject = false;
		m_Move = cnNone;		
		newRect = CRect(0,0,0,0);
		oldRect = newRect;
		newRect_2 = CRect(0,0,0,0);
		oldRect_2 = newRect_2;
		newRect_3 = CRect(0,0,0,0);
		oldRect_3 = newRect_3;
  		
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



void CLineStudyFibonacciProgression::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	if(IsObjectClicked() && selectable){
		pCtrl->UnSelectAll(); // Unselect everything else
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRect(0,0,0,0);
			newRect = oldRect;
			oldRect_2 = CRect(0,0,0,0);
			newRect_2 = oldRect_2;
			oldRect_3 = CRect(0,0,0,0);
			newRect_3 = oldRect_3;
			pCtrl->movingObject = true;				
			pCtrl->RePaint();
			if(buttonState != MOUSE_DOWN){
				startX = pointF.x; // Save the starting location
				startY = pointF.y;
			}


			if(pointF.x > x1-10 && pointF.y > y1-10 && pointF.x < x1+10 && pointF.y < y1+10){
				move1L = true;
			} 
			else if(pointF.x > x2-10 && pointF.y > y2-10 && pointF.x < x2+10 && pointF.y < y2+10){
				move2L = true;
			}
			else if((pointF.x>=x1+10 && pointF.x<=x2-10 && ((pointF.y>=y1+10 &&  pointF.y<=y2-10)||(pointF.y<=y1+10 &&  pointF.y>=y2-10)))
						|| (pointF.x>=x2+10 && pointF.x<=x1_2-10 && ((pointF.y>=y2+10 &&  pointF.y<=y1_2-10)||(pointF.y<=y2+10 &&  pointF.y>=y1_2-10))))
			{
				m_Move = cnMoveAll;
				moveAll = true;
			}

			if(IsObjectClicked()){
				m_Move = cnMoveAll;
				moveCtr = true;
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


void CLineStudyFibonacciProgression::XORDraw(UINT nFlags, CPoint point)
{				
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	if(nFlags == 1){ // First point
				
		oldRect = CRect(0,0,0,0);
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
		drawn = false;
		state = 1;
	}
	else if(nFlags == 2){ // Drawing the Rectangle
		
		CDC* pDC = pCtrl->GetScreen();
		
		int next = 0;

		switch (state){

		case 1:
			
			pDC->SetROP2(R2_NOT);
			DrawSimpleLine(pDC, oldRect);

			if (pointF.x < startX){
				pointF.x = RealPointX(startX);
			}

			newRect.left = startX;
			newRect.right = pointF.x;
			newRect.top = startY;
			newRect.bottom = pointF.y;									

			//Checking bounds
			if(newRect.bottom < ownerPanel->y1){
				next = pCtrl->GetNextHigherChartPanel((int)ownerPanel->y1);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect.bottom > ownerPanel->y2){
				next = pCtrl->GetNextLowerChartPanel((int)ownerPanel->y2);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect.top < ownerPanel->y1){
				next = pCtrl->GetNextHigherChartPanel((int)ownerPanel->y1);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect.top > ownerPanel->y2){
				next = pCtrl->GetNextLowerChartPanel((int)ownerPanel->y2);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
	
			DrawSimpleLine(pDC, newRect);

			pDC->SetROP2(R2_COPYPEN);
			oldRect = newRect;
			pCtrl->ReleaseScreen(pDC);
			break;
			
		case 2:
			
			pDC->SetROP2(R2_NOT);
			DrawSimpleLine(pDC, oldRect_2);
			DrawLineStudy(pDC, oldRect_3);

			newRect_2.left = oldRect.right;
			newRect_2.right = pointF.x;
			newRect_2.top = oldRect.bottom;
			newRect_2.bottom = pointF.y;
			
			int dx, dy;
			dx = newRect.left - newRect.right;
			dy = newRect.top - newRect.bottom;
			newRect_3 = CRect(newRect_2.right, newRect_2.bottom, newRect_2.right + dx, newRect_2.bottom - dy );
			
			//Checking bounds
			if(newRect_2.bottom < ownerPanel->y1){
				next = pCtrl->GetNextHigherChartPanel((int)ownerPanel->y1);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect_2.bottom > ownerPanel->y2){
				next = pCtrl->GetNextLowerChartPanel((int)ownerPanel->y2);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect_2.top < ownerPanel->y1){
				next = pCtrl->GetNextHigherChartPanel((int)ownerPanel->y1);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
			if(newRect_2.top > ownerPanel->y2){
				next = pCtrl->GetNextLowerChartPanel((int)ownerPanel->y2);
				if(next != -1) pCtrl->panels[next]->Invalidate();
			}
	
			DrawSimpleLine(pDC, newRect_2);
			DrawLineStudy(pDC, newRect_3);

			pDC->SetROP2(R2_COPYPEN);
			oldRect_2 = newRect_2;
			oldRect_3 = newRect_3;
			pCtrl->ReleaseScreen(pDC);
			break;
		}
	}
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
		

		if(state == 1){

			if (pointF.x < startX){
				pointF.x = RealPointX(startX);
			}

			y1	= startY;
			y2	= pointF.y;
			x1	= startX;
			x2	= pointF.x;
					
			// Return and wait for second line
			state = 2;
			return;
		}

		if(state == 2){
			int dx, dy;
			dx = x2 - x1;
			dy = y2 - y1;
			x1_2 = pointF.x;
			y1_2 = pointF.y;
			x2_2 = pointF.x + dx;
			y2_2 = pointF.y + dy;

			state = 3;
		}

		//flip coordinates
		if ( x1 > x2){
			int temp;
			temp = x1;
			x1 = x2;
			x2 = temp;
			
			temp = y1;
			y1 = y2;
			y2 = temp;

			temp = x1_2;
			x1_2 = x2_2;
			x2_2 = temp;
			
			temp = y1_2;
			y1_2 = y2_2;
			y2_2 = temp;
					
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
		
		pCtrl->OnUserDrawingComplete(lsFibonacciProgression, key);
		pCtrl->SaveUserStudies();

	}
}

void CLineStudyFibonacciProgression::OnMouseMove(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

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



	// Display the tool top if we are allowed to
	if(pCtrl == NULL) return;

#ifdef OLD_BEHAVIOR
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

		// To keep from drawing over things
		ExcludeRects(pDC);

		DrawSimpleLine(pDC, oldRect);
		DrawSimpleLine(pDC, oldRect_2);
		DrawLineStudy(pDC, oldRect_3);

		
		
		if(move1L){		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);

			newRect.left = pointF.x;
			newRect.right = x2;
			newRect.top = pointF.y;
			newRect.bottom = y2;
			x1 = pointF.x;
			y1 = pointF.y;

			//Restrain movements for channel
			if(x1 > x2){
				x1 = x2;
				newRect.left = x1;
			}

			newRect_2.left = x2;
			newRect_2.right = x1_2;
			newRect_2.top = y2;
			newRect_2.bottom = y1_2;

			int dx, dy;
			dx = newRect.left - newRect.right;
			dy = newRect.top - newRect.bottom;
			newRect_3 = CRect(newRect_2.right, newRect_2.bottom, newRect_2.right + dx, newRect_2.bottom - dy );
			
 		} 
		else if(move2L){		
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
			newRect.left = x1;
			newRect.right = pointF.x;
			newRect.top = y1;
			newRect.bottom = pointF.y;
			x2 = pointF.x;
			y2 = pointF.y;

			if(x2 < x1){
				x2 = x1;
				newRect.right = x2;
			}

			newRect_2.left = x2;
			newRect_2.right = x1_2;
			newRect_2.top = y2;
			newRect_2.bottom = y1_2;

			int dx, dy;
			dx = newRect.left - newRect.right;
			dy = newRect.top - newRect.bottom;
			newRect_3 = CRect(newRect_2.right, newRect_2.bottom, newRect_2.right + dx, newRect_2.bottom - dy );

		}
		else if(moveAll){

			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);

			newRect_2.left = x2 - (startX - pointF.x);
			newRect_2.right = x1_2 - (startX - pointF.x);
			newRect_2.top = y2 - (startY - pointF.y);
			newRect_2.bottom = y1_2 - (startY - pointF.y);

			newRect_3.left = x1_2 - (startX - pointF.x);
			newRect_3.right = x2_2 - (startX - pointF.x);
			newRect_3.top = y1_2 - (startY - pointF.y);
			newRect_3.bottom = y2_2 - (startY - pointF.y);	

		}
		else if(moveCtr){
			newRect.left = x1;
			newRect.right = x2;
			newRect.top = y1;
			newRect.bottom = y2;

			newRect_2.left = x2;
			newRect_2.right = x1_2 - (startX - pointF.x);
			newRect_2.top = y2;
			newRect_2.bottom = y1_2 - (startY - pointF.y);

			newRect_3.left = x1_2 - (startX - pointF.x);
			newRect_3.right = x2_2 - (startX - pointF.x);
			newRect_3.top = y1_2 - (startY - pointF.y);
			newRect_3.bottom = y2_2 - (startY - pointF.y);			

		} 

		
		DrawSimpleLine(pDC, newRect);
		DrawSimpleLine(pDC, newRect_2);
		DrawLineStudy(pDC, newRect_3);	
		
		pDC->SetROP2(R2_COPYPEN);

		
		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;
		oldRect_2 = newRect_2;
		oldRect_3 = newRect_3;
		IncludeRects(pDC);
	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciProgression::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciProgression::OnDoubleClick(CPoint point)
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
// Simply create a CLineStudyFibonacciProgression, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFibonacciProgression::SnapLine()
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

void CLineStudyFibonacciProgression::OnMessage(LPCTSTR MsgGuid, int MsgID)
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

void CLineStudyFibonacciProgression::DisplayInfo()
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

void CLineStudyFibonacciProgression::DrawLineStudy(CDC* pDC, CRect rect)
{

	if (pCtrl->yAlignment == LEFT) {
		if (x1 >= pCtrl->width - 35) return;
	}
	else{
		if (x1 >= (pCtrl->width - pCtrl->yScaleWidth - 35)) return;
	}

	if (x1 >= ownerPanel->panelRect.right - 35) return;

	CPen* pen = new CPen(PS_SOLID, lineWeight+pointOutStep, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	CFont newFont;
	newFont.CreatePointFont(VALUE_FONT_SIZE - 5, _T("Arial Rounded MT"), pDC);
	CFont* pOldFont = pDC->SelectObject(&newFont);
	OLE_COLOR oldColor = pDC->SetTextColor(lineColor);



	if(pCtrl->yAlignment == LEFT){
		rect.right = pCtrl->width - 35; // 7/16/08
	}
	else{
		rect.right = pCtrl->width - pCtrl->yScaleWidth - 35;
	}


	// Get width of Fibonacci values and percentages
	TEXTMETRIC tm;
	pDC->GetTextMetrics(&tm);
	int nAvgWidth = (int)(1 + (tm.tmAveCharWidth * 1.2));
	int nCharHeight = (int)(1 + (tm.tmHeight));
	CString len;		
	double max = ownerPanel->GetReverseY((double)rect.top);
	len.Format("%.*f", 2, max);	
	int nMaxWidth = ((nAvgWidth * (len.GetLength() + 5)));	
	//rect.right -= nMaxWidth;	
	//x2 = rect.right;



	CString strNum1, strNum2;
	CRect rectText(0,0,0,0);
	double y = 0;
	double fibNum = 1.618;
	
	for(int n = 0; n < 10; n++){
		if(params[n]>1.0) fibNum=-(params[n]-1.0);
			else fibNum = params[n];
		fibLines[n].x1 = rect.left;
		fibLines[n].y1 = rect.top + rect.Height() * (1-fibNum);
		fibLines[n].x2 = rect.right;
		fibLines[n].y2 = rect.top + rect.Height() * (1-fibNum);
	}

	for(int n = 0; n < 10; n++){

		if(params[n] != NULL_VALUE){ // Custom levels
			if(params[n]>1.0) fibNum=-(params[n]-1.0);
			else fibNum = params[n];
			
			if(m_Move != cnNone || move1L || move2L || moveCtr || moveAll || !drawn){
				pDC->MoveTo(rect.left, rect.top + rect.Height() * (1-fibNum));
				pDC->LineTo(rect.right, rect.top + rect.Height() * (1-fibNum));
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top + rect.Height() * (1-fibNum)),CPointF(rect.right,rect.top + rect.Height() * (1-fibNum)),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			


			if(m_Move == cnNone && !drawing){ 
				//rectText.top = (rect.top + ((double)rect.Height() * (1-fibNum))) - 6;
				//rectText.bottom = rectText.top + 20;
				//rectText.left = rect.left - 50;
				//rectText.right = rect.left - 5;

				// 9/20/07
				double display = fibNum;
				//if(display < 0) display = 0;
				//if(display > 1) display = 1;
			
				strNum1.Format("%.*f", 1,  (1 - display) * 100);

				rectText.top = (rect.top + ((double)rect.Height() * (1-fibNum))) - 15;
				rectText.bottom = rectText.top + 20;
				//rectText.left = (rect.left < ownerPanel->panelRect.left) ? ownerPanel->panelRect.left : rect.left;
				//rectText.right = rectText.left + 120;
				rectText.right = rect.right;
				rectText.left =  rect.right - 120;
				y = ownerPanel->GetReverseY((double)rectText.top + 15);

				strNum2.Format("%.*f", pCtrl->decimals, y);

				strNum2 = strNum2 + " (" + strNum1 + "%)";
				CRectF rectTextF = CRectF(rectText.left,rectText.top,rectText.right,rectText.bottom);
				//pDC->DrawText(strNum2, -1, &rectText, DT_WORDBREAK | DT_RIGHT);
				double sizeText=11.0F;
				//Remove text for params?
				/*if(n>1 && n<6){
					int offset = 1;
					if(params[n]>params[n]+1) offset = -1;
					if((double)abs(fibLines[n].y1-fibLines[n+offset].y1)<13.0F) {
						continue;
					}
				}*/
				pCtrl->pdcHandler->DrawText(strNum2,rectTextF,"Arial Rounded MT",sizeText,DT_RIGHT,lineColor, 255,pDC);

			}
		}
		
	}
#ifdef _CONSOLE_DEBUG
	printf("\n\nFIBONACCI PARAMS:");
	for(int n = 0; n < 10 ; n++){
		if(params[n] != NULL_VALUE)	printf("\n\tparam[%d].y=%f",n,fibLines[n].y1);
	}

#endif
	pDC->SetTextColor(oldColor);
	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();	
	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

}

// Simple Line
void CLineStudyFibonacciProgression::DrawSimpleLine(CDC* pDC, CRect rect)
{
	if(rect == CRect(0,0,0,0)) return;

	CPen pen( PS_DOT, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	if(m_Move != cnNone || move1L || move2L || moveCtr || moveAll || !drawn || drawing){
		pDC->MoveTo(rect.left,rect.top);
		pDC->LineTo(rect.right,rect.bottom);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top),CPointF(rect.right,rect.bottom),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	
	pDC->SelectObject(pOldPen);
	pen.DeleteObject();
	
}

// Special click function for this object
bool CLineStudyFibonacciProgression::IsObjectClicked()
{
	bool clicked =  IsClicked(x1, y1, x2, y2) || IsClicked(x2, y2, x1_2, y1_2) || IsClicked(x1_2, y1_2, x2_2, y2_2);
	if(!clicked){
		for(int n = 0; n < 10; n++){
			clicked = IsClicked(fibLines[n].x1,fibLines[n].y1,fibLines[n].x2,fibLines[n].y2);
			if(clicked) break;
		}
	}
	return clicked;
}


//// Sets chart value lookup based on actual pixel position
//void CLineStudyFibonacciProgression::Reset()
//{
//	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
//	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
//	y1Value = ownerPanel->GetReverseY(y1);
//	y2Value = ownerPanel->GetReverseY(y2);
//	x1 = (int)ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));	
//	y1 = (int)ownerPanel->GetY(y1Value);
//	y2 = (int)ownerPanel->GetY(y2Value);
//
//	if (x2 < ownerPanel->panelRect.right) {
//	  if(pCtrl->yAlignment == LEFT){
//		  x2 = pCtrl->width;
//	  }
//	  else{
//		  x2 = pCtrl->width - pCtrl->yScaleWidth - 35;
//	  }
//	}
//}