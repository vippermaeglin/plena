// LineStudyFibonacciRetracements.cpp: implementation of the CLineStudyFibonacciRetracements class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyFibonacciRetracements.h"
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


CLineStudyFibonacciRetracements::CLineStudyFibonacciRetracements(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "FibonacciRetracements";
	nType = lsFibonacciRetracements;
	objectDescription = "Fibonacci Retracements";

	fibLines.resize(10);
	fib.resize(10);
	fib[0] = 0.0557;
	fib[1] = 0.0902;
	fib[2] = 0.118;
	fib[3] = 0.1459;
	fib[4] = 0.2361;
	fib[5] = 0.381;
	fib[6] = 0.618;	

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

    // default values for fibonacci retracements
	lineWeight = pCtrl->lineThickness;	
	/*params[0] = 0.0;
	params[1] = 0.382;
	params[2] = 0.5;
	params[3] = 0.618;
	params[4] = 1.0;*/
	params[0]=pCtrl->fibonacciRetParams[0];
	params[1]=pCtrl->fibonacciRetParams[1];
	params[2]=pCtrl->fibonacciRetParams[2];
	params[3]=pCtrl->fibonacciRetParams[3];
	params[4]=pCtrl->fibonacciRetParams[4];
	params[5]=pCtrl->fibonacciRetParams[5];
	params[6]=pCtrl->fibonacciRetParams[6];
	params[7]=NULL_VALUE;
	params[8]=NULL_VALUE;
	params[9]=NULL_VALUE;





}

CLineStudyFibonacciRetracements::~CLineStudyFibonacciRetracements()
{

}

void CLineStudyFibonacciRetracements::OnPaint(CDC *pDC)
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
	{ 
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y1+3,x1-3,y1-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y2+3,x1-3,y2-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y1+3,x2-3,y1-3),lineColor, pDC);
		pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();
		
	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciRetracements::OnLButtonUp(CPoint point)
{

	if(drawing) return; // The object is being resized	

	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

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

		// We're not drawing anymore so reset the state
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->movingObject = false;
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
		pCtrl->SaveUserStudies();

	}


	buttonState = MOUSE_UP; // The mouse is up
	pCtrl->dragging = false; // The mouse is up so we're not dragging anything
	pCtrl->movingObject = false;

}



void CLineStudyFibonacciRetracements::OnLButtonDown(CPoint point)
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


void CLineStudyFibonacciRetracements::XORDraw(UINT nFlags, CPoint point)
{	
	CPointF pointF = CPointF((double)point.x,(double)point.y);

#ifdef _CONSOLE_DEBUG
	printf("\nXORDraw(%d)",nFlags);
#endif
			
	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
	if(nFlags == 1){ // First pointF
	    pointF.x = RealPointX(pointF.x);
	    pointF.y = RealPointY(pointF.y);
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
		drawn = false;
		oldRect = CRectF(0,0,0,0);
	}
	else if(nFlags == 2){ // Drawing the Rectangle
		
		CDC* pDC = pCtrl->GetScreen();
		pDC->SetROP2(R2_NOT);
		
		ExcludeRects(pDC);
		
		
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;
		DrawLineStudy(pDC, oldRect); // Clear the previous drawing
		

		if(startX<pointF.x){
			newRect.left = startX;
			newRect.right = pointF.x;
			newRect.top = startY;
			newRect.bottom = pointF.y;
		}
		else{
			newRect.right = startX;
			newRect.left = pointF.x;
			newRect.bottom = startY;
			newRect.top = pointF.y;
		}

		/*if(startX<pointF.x){
			newRect.left = startX;
			newRect.right = pointF.x;
		}
		else{
			newRect.right = startX;
			newRect.left = pointF.x;
		}
		if(startY<pointF.y){
			newRect.top = startY;
			newRect.bottom = pointF.y;
		}
		else{
			newRect.bottom = startY;
			newRect.top = pointF.y;
		}*/

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
		if(startX<pointF.x){
			x1	= startX;
			y1	= startY;
			x2	= pointF.x;
			y2	= pointF.y;
		}
		else{
			x2	= startX;
			y2	= startY;
			x1	= pointF.x;
			y1	= pointF.y;
		}
		/*
		int temp = 0;if(x1 > x2)
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

		/*	SGC	03.06.2004	BEG	*/		
		this->OnPaint( pCtrl->GetScreen() );
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		drawn = true;

		pCtrl->OnUserDrawingComplete(lsFibonacciRetracements, key);
		pCtrl->SaveUserStudies();

	}
}

void CLineStudyFibonacciRetracements::OnMouseMove(CPoint point)
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
			if(pCtrl->m_Magnetic && ownerPanel->index==0)pointF.y = MagneticPointY(pointF.y, pointF.x);
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
			if(pCtrl->m_Magnetic)pointF.y = MagneticPointY(pointF.y, pointF.x);
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

void CLineStudyFibonacciRetracements::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyFibonacciRetracements::OnDoubleClick(CPoint point)
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
// Simply create a CLineStudyFibonacciRetracements, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyFibonacciRetracements::SnapLine()
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

void CLineStudyFibonacciRetracements::OnMessage(LPCTSTR MsgGuid, int MsgID)
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

void CLineStudyFibonacciRetracements::DisplayInfo()
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

void CLineStudyFibonacciRetracements::DrawLineStudy(CDC* pDC, CRectF rect)
{ 
	if(rect.left==0&&rect.right==0&&rect.top==0&&rect.bottom==0) return;
	if (pCtrl->yAlignment == LEFT) {
		if (x1 >= pCtrl->width - 35) return;
	}
	else{
		if (x1 >= (pCtrl->width - pCtrl->yScaleWidth - 35)) return;
	}

	if (x1 >= ownerPanel->panelRect.right - 35) return;
	
	
	
	if(m_Move != cnNone || !drawn){
		pDC->MoveTo(rect.left, rect.top);
		pDC->LineTo(rect.right, rect.bottom);		
	}
	//else pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top ),CPointF(rect.right,rect.bottom),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			

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
	x2 = rect.right;


	CString strNum1, strNum2;
	CRect rectText(0,0,0,0);
	double y = 0;
	double fibNum = 1.618;
	/*
	if (params[0] == NULL_VALUE) {
      // default values for fibonacci retracements
	  params[0] = 1.0;
	  params[1] = 0.618;
	  params[2] = 0.5;
	  params[3] = 0.382;
	  params[4] = 0.0;
	  //params[5] = -0.618;
	}*/

	for(int n = 0; n < 10; n++){

		if(params[n] != NULL_VALUE){ // Custom levels
			if(params[n]>1.0) fibNum=-(params[n]-1.0);
			else fibNum = params[n];

		if(rect.top>rect.bottom){
			if(m_Move != cnNone || !drawn){
#ifdef _CONSOLE_DEBUG
				printf("\n\n\tA x1=%f x2=%f y1=%f\n",rect.left,rect.right, rect.top);
#endif
				pDC->MoveTo(rect.left, rect.top - rect.Height() * (1-fibNum));
				pDC->LineTo(rect.right, rect.top - rect.Height() * (1-fibNum));
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top - rect.Height() * (1-fibNum)),CPointF(rect.right,rect.top - rect.Height() * (1-fibNum)),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			fibLines[n].x1 = rect.left;
			fibLines[n].y1 = rect.top - rect.Height() * (1-fibNum);
			fibLines[n].x2 = rect.right;
			fibLines[n].y2 = rect.top - rect.Height() * (1-fibNum);
		}
		else{
			if(m_Move != cnNone || !drawn){
#ifdef _CONSOLE_DEBUG
				printf("\n\n\tB x1=%f x2=%f\n",rect.left,rect.right);
#endif
				pDC->MoveTo(rect.left, rect.top + rect.Height() * (1-fibNum));
				pDC->LineTo(rect.right, rect.top + rect.Height() * (1-fibNum));
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(rect.left,rect.top + rect.Height() * (1-fibNum)),CPointF(rect.right,rect.top + rect.Height() * (1-fibNum)),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			fibLines[n].x1 = rect.left;
			fibLines[n].y1 = rect.top + rect.Height() * (1-fibNum);
			fibLines[n].x2 = rect.right;
			fibLines[n].y2 = rect.top + rect.Height() * (1-fibNum);
		}


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

				if(rect.top>rect.bottom)rectText.top = (rect.top - ((double)rect.Height() * (1-fibNum))) - 15;
				else rectText.top = (rect.top + ((double)rect.Height() * (1-fibNum))) - 15;
				rectText.bottom = rectText.top + 20;
				//rectText.left = (rect.left < ownerPanel->panelRect.left) ? ownerPanel->panelRect.left : rect.left;
				//rectText.right = rectText.left + 120;
				rectText.right = rect.right;
				rectText.left = rectText.right - 120;
				y = ownerPanel->GetReverseY((double)rectText.top + 15);

				strNum2.Format("%.*f", pCtrl->decimals, y);

				strNum2 = strNum2 + " (" + strNum1 + "%)";
				//pDC->DrawText(strNum2, -1, &rectText, DT_WORDBREAK | DT_RIGHT);
				
				CRectF rectTextF = CRectF(rectText.left,rectText.top,rectText.right,rectText.bottom);				
				//Remove text for params?
				/*if(n>1 && n<6){
					int offset = 1;
					if(params[n]>params[n]+1) offset = -1;
					if((double)abs(fibLines[n].y1-fibLines[n+offset].y1)<13.0F) {
						continue;
					}
				}*/
				pCtrl->pdcHandler->DrawText(strNum2,rectTextF,"Arial Rounded MT",11,DT_RIGHT,lineColor, 255,pDC);

			}
		}
		else{
			//break;
			//fibNum *= 0.618;
		}
		
	}

	//pDC->SetTextColor(oldColor);
	//pDC->SelectObject(pOldFont);
	//newFont.DeleteObject();	
	//pDC->SelectObject(pOldPen);
	//pen->DeleteObject();
	//if(pen != NULL) delete pen;

}



// Special click function for this object
bool CLineStudyFibonacciRetracements::IsObjectClicked()
{
	bool clicked = IsRegionClicked();
	if(!clicked){
		for(int n = 0; n < 10; n++){
			clicked = IsClicked(fibLines[n].x1,fibLines[n].y1,fibLines[n].x2,fibLines[n].y2);
			if(clicked) break;
		}
	}
	return clicked;
}


// Sets chart value lookup based on actual pixel position
void CLineStudyFibonacciRetracements::Reset()
{
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	x1 = (int)ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));	
	x2 = (int)ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));	
	y1 = (int)ownerPanel->GetY(y1Value);
	y2 = (int)ownerPanel->GetY(y2Value);


	if(pCtrl->yAlignment == LEFT){
		x2 = pCtrl->width;
	}
	else{
		x2 = pCtrl->width - pCtrl->yScaleWidth - 35;
	}
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