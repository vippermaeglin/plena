// LineStudyRaffRegression.cpp: implementation of the CLineStudyRaffRegression class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyRaffRegression.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyRaffRegression::CLineStudyRaffRegression(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "RaffRegression";
	nType = lsRaffRegression;
	objectDescription = "Raff Regression";

	raffLines.resize(3);

	raffLines.resize(3);
	for(int n = 0; n != 3; n++){
		raffLines[n].x1 = 0;
		raffLines[n].y1 = 0;
		raffLines[n].x2 = 0;
		raffLines[n].y2 = 0;
	}
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
	startX = 0;
	startY = 0;
	key = Key;
}

CLineStudyRaffRegression::~CLineStudyRaffRegression()
{

}

void CLineStudyRaffRegression::OnPaint(CDC *pDC)
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
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a Rectangle
	
	
	// Draw the object
	CPen	pen( lineStyle, lineWeight, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	DrawLineStudy(pDC, CRect(x1,y1,x2,y2));
	pDC->SelectObject(pOldPen);
	pen.DeleteObject();


	// If the object is selected, paint end point selection boxes
	if(selected)
	{ // Paint 4 selector boxes around the object's base
		CBrush	br( lineColor );

		pDC->FillRect( CRect(x1-3,raffLines[2].y1-3,x1+3,raffLines[2].y1+3), &br );
		pDC->FillRect( CRect(x2+3,raffLines[2].y2+3,x2-3,raffLines[2].y2-3), &br );
		pDC->FillRect( CRect(x1-3,raffLines[0].y1-3,x1+3,raffLines[0].y1+3), &br );
		pDC->FillRect( CRect(x2+3,raffLines[0].y2+3,x2-3,raffLines[0].y2-3), &br );
	
		br.DeleteObject();
	}


	IncludeRects(pDC);

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyRaffRegression::OnLButtonUp(CPoint point)
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
		newRect = CRect(0,0,0,0);
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

}



void CLineStudyRaffRegression::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	if(IsObjectClicked() && selectable){
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
			if(pointF.x > x1-15 && pointF.y > raffLines[2].y1-15 && pointF.x < x1+15 && pointF.y < raffLines[2].y1+15){
				m_Move = cnTopLeft;	
			}
			else if(pointF.x > x2-15 && pointF.y > raffLines[2].y2-15 && pointF.x < x2+15 && pointF.y < raffLines[2].y2+15){
				m_Move = cnTopRight;				
			}
			else if(pointF.x > x2-15 && pointF.y > raffLines[0].y2-15 && pointF.x < x2+15 && pointF.y < raffLines[0].y2+15){
				m_Move = cnBottomRight;				
			}
			else if(pointF.x > x1-15 && pointF.y > raffLines[0].y2-15 && pointF.x < x1+15 && pointF.y < raffLines[0].y2+15){
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


void CLineStudyRaffRegression::XORDraw(UINT nFlags, CPoint point)
{	
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(nFlags == 1){ // First point
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

		/*	SGC	03.06.2004	BEG	*/		
		this->OnPaint( pCtrl->GetScreen() );
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		pCtrl->OnUserDrawingComplete(lsRaffRegression, key);
		pCtrl->SaveUserStudies();

	}
}

















void CLineStudyRaffRegression::OnMouseMove(CPoint point)
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
		if(pointF.x > x1-15 && pointF.y > raffLines[2].y1-15 && pointF.x < x1+15 && pointF.y < raffLines[2].y1+15){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));
		}
		else if(pointF.x > x2-15 && pointF.y > raffLines[2].y2-15 && pointF.x < x2+15 && pointF.y < raffLines[2].y2+15){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));				
		}
		else if(pointF.x > x2-15 && pointF.y > raffLines[0].y2-15 && pointF.x < x2+15 && pointF.y < raffLines[0].y2+15){
			SetCursor(AfxGetApp()->LoadCursor(IDC_OPEN_HAND));
		}
		else if(pointF.x > x1-15 && pointF.y > raffLines[0].y2-15 && pointF.x < x1+15 && pointF.y < raffLines[0].y2+15){
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


		// Draw the temporary object
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;

		// To keep from drawing over things
		ExcludeRects(pDC);

		DrawLineStudy(pDC, oldRect);
		
 
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
		DrawLineStudy(pDC, newRect);	
		
		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyRaffRegression::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyRaffRegression::OnDoubleClick(CPoint point)
{	
	if(selected) pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
}
/////////////////////////////////////////////////////////////////////////////

// Workaround
// Simply create a CLineStudyRaffRegression, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyRaffRegression::SnapLine()
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

void CLineStudyRaffRegression::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) return;
	switch(MsgID){
		case MSG_TOOLTIP:
			DisplayInfo();
			break;
	}
}

void CLineStudyRaffRegression::DisplayInfo()
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


void CLineStudyRaffRegression::DrawLineStudy(CDC* pDC, CRect rect)
{

	// ****************************************************************
	// *Note: This line study requires OHLC series in the owner panel!*
	// ****************************************************************

	// Get the starting and ending record numbers
	if(rect.left > rect.right){
		int left = rect.left;
		rect.left = rect.right;
		rect.right = left;
	}

	int revX1 = (int)(ownerPanel->GetReverseX(rect.left) + pCtrl->startIndex);
	int revX2 = (int)(ownerPanel->GetReverseX(rect.right) + pCtrl->startIndex);	
	if(revX1 < 0) revX1 = 0;
	if(revX2 < 0) revX2 = 0;
	
	// Get the highest high of the high series.
	// Note: this code makes the assumption
	// that only one symbol exists on this panel.
	CSeries* pHigh = GetOHLCSeries("high");
	if(NULL == pHigh) return;
	double highestHigh = 0;	
	if(revX1 > (int)pHigh->data_master.size() - 1){
		revX1 = (int)pHigh->data_master.size() - 1;
	}
	if(revX2 > (int)pHigh->data_master.size() - 1){
		revX2 = (int)pHigh->data_master.size() - 1;
	}


	// Get the lowest low of the low series.
	// Note: this code makes the assumption
	// that only one symbol exists on this panel.
	CSeries* pLow = GetOHLCSeries("low");
	if(NULL == pLow) return;
	double lowestLow = highestHigh;
	if(revX1 > (int)pLow->data_master.size() - 1){
		revX1 = (int)pLow->data_master.size() - 1;
	}
	if(revX2 > (int)pLow->data_master.size() - 1){
		revX2 = (int)pLow->data_master.size() - 1;
	}


	// Get the close series
	CSeries* pClose = GetOHLCSeries("close");
	if(NULL == pClose) return;


	//**********
	// Perform linear regression on the data
	double xSum = 0, ySum = 0, xSquaredSum = 0, ySquaredSum = 0, xYSum = 0;
  int n;
	if(revX1 > revX2) revX2 = revX1;
	int x = revX2 - revX1;
    for (n = 1; n != x + 1; ++n){
		int j = revX1 + n - 1;
		xSum += n;
		ySum += pClose->data_master[j].value;
		xSquaredSum += (n * n);
		ySquaredSum += (pClose->data_master[j].value * pClose->data_master[j].value);
		xYSum += (pClose->data_master[j].value * n);
	}
    n = x;
    double q1 = (xYSum - ((xSum * ySum) / n));
    double q2 = (xSquaredSum - ((xSum * xSum) / n));
    double q3 = (ySquaredSum - ((ySum * ySum) / n));
    double slope = (q1 / q2);
    double leftValue = (((1 / (double)n) * ySum) - (((int)((double)n / 2)) * slope));
    double rightValue = ((n * slope) + leftValue);
    double inc = (rightValue - leftValue) / (x - 1);    
    double prevVal = 0;
	int j = 0;
	
	// Find max distance from linear regression line
	double maxDistance = 0;
	lowestLow = pHigh->data_master[0].value;
	for(n = revX1; n != revX2 + 1; ++n){
        j++;
		double val = leftValue + inc * (j - 1);
		if(prevVal != 0){
			if(pHigh->data_master[n].value - val > highestHigh){
				highestHigh = pHigh->data_master[n].value - val;
			}
			if(val - pLow->data_master[n].value < lowestLow &&
				val - pLow->data_master[n].value > 0){
				lowestLow = val - pLow->data_master[n].value;
			}
		}
		prevVal = val;
    }
	if(highestHigh > lowestLow) lowestLow = highestHigh;
	if(lowestLow > highestHigh) highestHigh = lowestLow;

	// Draw the study
	CPen* pen = new CPen(PS_DOT, lineWeight, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);
	j = 0;
	prevVal = 0;
	
	for(n = revX1; n != revX2 + 1; ++n){
        j++;
		double val = leftValue + inc * (j - 1);
		int lX1 = ownerPanel->GetX(n - pCtrl->startIndex);
		int lX2 = ownerPanel->GetX(n - pCtrl->startIndex + 1);
		double lY1 = ownerPanel->GetY(prevVal + highestHigh);
		double lY2 = ownerPanel->GetY(val + highestHigh);
		if(n == revX1) raffLines[2].y1 = ownerPanel->GetY(val + highestHigh);
		if(n == revX2) raffLines[2].y2 = lY2;
		if(prevVal != 0){
			pDC->MoveTo(lX1, lY1);
			pDC->LineTo(lX2, lY2);
		}
		lY1 = ownerPanel->GetY(prevVal - lowestLow);
		lY2 = ownerPanel->GetY(val - lowestLow);
		if(n == revX1) raffLines[0].y1 = ownerPanel->GetY(val - lowestLow);
		if(n == revX2) raffLines[0].y2 = lY2;
		if(prevVal != 0){
			pDC->MoveTo(lX1, lY1);
			pDC->LineTo(lX2, lY2);
			pDC->SelectObject(pOldPen);
		}
		lY1 = ownerPanel->GetY(prevVal);
		lY2 = ownerPanel->GetY(val);
		if(n == revX1) raffLines[1].y1 = ownerPanel->GetY(val);
		if(n == revX2) raffLines[1].y2 = lY2;
		if(prevVal != 0){
			pDC->MoveTo(lX1, lY1);
			pDC->LineTo(lX2, lY2);
			pOldPen = pDC->SelectObject(pen);
		}
		prevVal = val;
    }

	raffLines[0].x1 = rect.left;
	raffLines[0].x2 = rect.right;
	raffLines[1].x1 = rect.left;
	raffLines[1].x2 = rect.right;
	raffLines[2].x1 = rect.left;
	raffLines[2].x2 = rect.right;

	//**********

 
	if(drawing || m_Move == cnMoveAll){
		pDC->MoveTo(rect.left, ownerPanel->y1);
		pDC->LineTo(rect.left, ownerPanel->y2);
		pDC->MoveTo(rect.right, ownerPanel->y1);
		pDC->LineTo(rect.right, ownerPanel->y2);
	}

	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

}



// Special click function for this object
bool CLineStudyRaffRegression::IsObjectClicked()
{
	bool clicked = IsRegionClicked();
	if(!clicked){
		for(int n = 0; n != 3; ++n){
			clicked = IsClicked(raffLines[n].x1,raffLines[n].y1,
				raffLines[n].x2,raffLines[n].y2);
			if(clicked) break;
		}
	}
	return clicked;
}

CSeries* CLineStudyRaffRegression::GetOHLCSeries(LPCSTR ohlc)
{
	CSeries* pSeries = NULL;
	for(int n = 0; n != ownerPanel->series.size(); ++n){
		CString group = ownerPanel->series[n]->szName;
		int found = group.Find(".", 0);
		if(found != -1){
			group = group.Left(found);
			group += "." + CString(ohlc);
			pSeries = ownerPanel->GetSeries(group);
			break;
		}
	}
	return pSeries;
}


// Specialized version of Reset for this line study
void CLineStudyRaffRegression::Reset()
{
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	x1 = (int)ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2 = (int)ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));
	
	if(raffLines[0].y1 < raffLines[2].y1){	
		y1 = raffLines[0].y1;
		y2 = raffLines[2].y2;
	}
	else{
		y1 = raffLines[2].y1;
		y2 = raffLines[1].y2;	
	}
}
