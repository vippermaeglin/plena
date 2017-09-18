// LineStudyGannFan.cpp: implementation of the CLineStudyGannFan class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyGannFan.h"
#include "Coordinates.h"
#include "julian.h"
//#include "DebugConsole.h"


//#define _CONSOLE_DEBUG 1

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CLineStudyGannFan::CLineStudyGannFan(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{

	// Object type and description
	objectType = "GannFan";
	nType = lsGannFan;
	objectDescription = "Gann Fan";

	// Initialize specific fields for this study

	gannFan.resize(10);
	for(int n = 0; n < 10; n++){
		gannFan[n].x1 = 0;
		gannFan[n].y1 = 0;
		gannFan[n].x2 = 0;
		gannFan[n].y2 = 0;
	}	
	prevUp = true;

	//Start as Up
	upOrDown = 1;

	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = CRectF(0,0,0,0);

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

	flagPaint=FALSE;

}

CLineStudyGannFan::~CLineStudyGannFan()
{

}

void CLineStudyGannFan::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	/*printf("\nGanFan::OnPaint()");
	if(drawing)printf(" drawing");
	if(selected && pCtrl->movingObject)printf(" selected && pCtrl->movingObject");*/
#endif
	if(drawing || m_Move!=cnNone ){
		// The object is being resized, MouseMove 
		// will call XORDraw instead
		return; 
	}

	Update();
	// Create hit test regions for mouse clicks on the object
	m_testRgn1.DeleteObject();
	m_testRgn1.CreateRectRgn(x1, y1, x2, y2);
	m_testRgn2.DeleteObject();
	m_testRgn2.CreateRectRgn(x1 + 5, y1 + 5, x2 - 5, y2 - 5); // This is a GannFan
	
	
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
	

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyGannFan::OnLButtonUp(CPoint point)
{

	
	m_Move = cnNone; // If the mouse is up we can't be drawing or moving

	if(drawing) return; // The object is being resized	


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
		m_Move = cnNone;		
		newRect = CRectF(0,0,0,0);
		oldRect = newRect;
  		
		// Snap the object to the nearest bars
		
		//CalculateAngles();
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
	pCtrl->RePaint(); // Cause the chart to repaint

}



void CLineStudyGannFan::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(drawing) return; // The object is being resized

	m_valueView.Reset(true); // Hide the tool tip

	bool clicked = IsObjectClicked();

	if(clicked && selectable){
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
				m_Move = cnMoveAll;	
			}
			else if(pointF.x > x2-15 && pointF.y > y1-15 && pointF.x < x2+15 && pointF.y < y1+15){
				m_Move = cnMoveAll;				
			}
			else if(pointF.x > x2-15 && pointF.y > y2-15 && pointF.x < x2+15 && pointF.y < y2+15){
				m_Move = cnMoveAll;				
			}
			else if(pointF.x > x1-15 && pointF.y > y2-15 && pointF.x < x1+15 && pointF.y < y2+15){
				m_Move = cnMoveAll;
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


void CLineStudyGannFan::XORDraw(UINT nFlags, CPoint point)
{	
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(nFlags == 1){ // First pointF
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;	// The object is being resized
	}
	else if(nFlags == 2){ // Drawing the Gann Fan
		
		CDC* pDC = pCtrl->GetScreen();
	 
		CPoint pl, pr;
		pl.x = pr.x = oldRect.right;
		pl.y = pr.y = oldRect.bottom;
		pDC->SetROP2(R2_NOT);
		DrawLineStudy(pDC, oldRect); // Clear the previous drawing
		
		// Static sized box
		newRect.left = pointF.x;
		newRect.right = pointF.x + 10;
		newRect.top = pointF.y - 10;
		newRect.bottom = pointF.y;

		
		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
	
		//Find if it is UpOrDown		
		CString sName;
		CSeriesStock *seriesStock = NULL;
		sName = pCtrl->m_symbol + ".low";
		CSeries *seriesSt = ownerPanel->GetSeries(sName);
		if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
			seriesStock = (CSeriesStock *) seriesSt;
		}
		if(seriesStock != NULL){
			Candle c = seriesStock->GetCandle(pCtrl->startIndex +  ownerPanel->GetReverseX((double) point.x));
			if( ownerPanel->GetReverseY( (double) point.y) > (c.GetMax() + c.GetMin())/2 )  {
				upOrDown = -1;
			} else {
				upOrDown = 1;
			}
		}
			

		DrawLineStudy(pDC, newRect);

		oldRect = newRect;
		pDC->SetROP2(R2_COPYPEN);		
		pCtrl->ReleaseScreen(pDC);

	}
	else if(nFlags == 3)	//	Finished drawing with the mouse
	{
		
		double xp1, xp2, yp1, yp2;
		xp1 = ownerPanel->GetX(0);
		xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
		yp1 = ownerPanel->y1;
		yp2 = ownerPanel->y2;	
		double radiusSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));//50; 
		Coordinates coord;
		CPointF temp;

		// Static sized box
		x1 = pointF.x;
		y1 = pointF.y;
		//Calculate max radius:
		temp=coord.MovePolarDouble(x1,y1,10.0F, coord.DegreeToRad(45.0F));
		while(x1+temp.x<MAX_VISIBLE && y1-temp.y>ownerPanel->y1) {
#ifdef _CONSOLE_DEBUG
			printf("\n\ttempX=%f tempY=%f",temp.x,temp.y);
#endif
			temp=coord.MovePolarDouble(temp.x,temp.y,10.0F, coord.DegreeToRad(45.0F));
		}
		temp=coord.MovePolarDouble(temp.x,temp.y,-10.0F, coord.DegreeToRad(45.0F));
		//x2=temp.x;
		//y2=temp.y;
		x2 = x1+10;
		y2 = y1+10;

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
		
		pCtrl->OnUserDrawingComplete(lsGannFan, key);
		pCtrl->SaveUserStudies();

	}
}



void CLineStudyGannFan::OnMouseMove(CPoint point)
{
	flagPaint=true;
	//Sleep(100);
	
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing && startX > 0){ // Sizing the object with the mouse so allow
		XORDraw(2, point); // XOR painting to draw a temporary XOR object
		
		flagPaint=false;
		return;
	}

	
	
	bool clicked = IsObjectClicked();

	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key); // Then fire MouseMove

	// Add pointoutmsg
	if( !selected && clicked && !pCtrl->movingObject )
	{

#ifdef OLD_BEHAVIOR
		displayed = true;
		pCtrl->textDisplayed = true;
		pCtrl->DelayMessage(guid, MSG_TOOLTIP, 1000); 
#endif

		//pCtrl->DelayMessage(guid, MSG_POINTOUT, 10); 
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



	// Display the tool top if we are allowed to
	if(!pCtrl->textDisplayed )
	{
		displayed = true;
		pCtrl->textDisplayed = true;
		//pCtrl->DelayMessage(guid, MSG_TOOLTIP, 1000);
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
		
		//Erase old rect
		DrawLineStudy(pDC, oldRect);
		

		//Find if it is UpOrDown	
		CString sName;
		CSeriesStock *seriesStock = NULL;
		sName = pCtrl->m_symbol + ".low";
		CSeries *seriesSt = ownerPanel->GetSeries(sName);
		if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
			seriesStock = (CSeriesStock *) seriesSt;
		}
		if(seriesStock != NULL){
			Candle c = seriesStock->GetCandle(pCtrl->startIndex +  ownerPanel->GetReverseX((double) point.x));
			if (ownerPanel->GetReverseY((double)point.y) > (c.GetMax() + c.GetMin()) / 2)  {
				upOrDown = -1;
			} else {
				upOrDown = 1;
			}
		}
			

		if(!(newRect.left==0 && newRect.top==0 && newRect.right==0 && newRect.bottom==0)) {
			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);
		}
		else newRect = CRectF(x1,y1,x2,y2);

		pl.x = pr.x = newRect.right;
		pl.y = pr.y = newRect.bottom;
		//Draw new rect
		DrawLineStudy(pDC, newRect);		
		
		oldRect = newRect;
		pDC->SetROP2(R2_COPYPEN);
		pCtrl->ReleaseScreen(pDC);		

	}
	
	flagPaint=false;
}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyGannFan::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyGannFan::OnDoubleClick(CPoint point)
{	
	if (selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
		selected = false;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
}
/////////////////////////////////////////////////////////////////////////////

// Simply create a CLineStudyGannFan, 
// set it's x1,y1,x2,y2 and call this.
void CLineStudyGannFan::SnapLine()
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

void CLineStudyGannFan::OnMessage(LPCTSTR MsgGuid, int MsgID)
{

//#ifdef _CONSOLE_DEBUG
//	printf("CLineStudyGannFan.OnMessage\n\n");
//
//	if(MsgID == MSG_POINTOUT){
//	
//		printf("MessagePointOut\n");
//	
//	}
//#endif
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

void CLineStudyGannFan::DisplayInfo()
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
	temp.Format("%d", 45);
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



void CLineStudyGannFan::DrawLineStudy(CDC *pDC, CRectF rect)
{	
	
	if(rect.left==0 && rect.top==0 && rect.right==0 && rect.bottom==0) {
		return;
	}

	// Don't draw over scales or other panels
	ExcludeRects(pDC);
	
	
	double xt1, xt2, yt1, yt2;
	xt1 = (double) rect.left;
	xt2 = (double) rect.right;
	yt1 = (double) rect.top;
	yt2 = (double) rect.bottom;



	double xp1, xp2, yp1, yp2;
	xp1 = ownerPanel->GetX(0);
	xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
	yp1 = ownerPanel->y1;
	yp2 = ownerPanel->y2;	
	double radiusSize = 10.0F;

	Coordinates coord;
	CPointF temp;
	double dxt,dyt;
	bool increase=false;
	// Calibrate rectangle for the minimum size
	while(abs(xt1-xt2)<1 && m_Move==cnNone && drawn){
		
#ifdef _CONSOLE_DEBUG
	printf("\nGannFan::Draw() \n\tx1=%f x2=%f y1=%f y2=%f\n\tx1JDate=%f x2JDate=%f y1Value=%f y2Value=%f",xt1, xt2, yt1, yt2,x1JDate,x2JDate,y1Value,y2Value);
#endif
		if(increase){
			increase=false;
			radiusSize+=10.0F;
			if(radiusSize > sqrt( pow(xp1 - xp2,2) + pow(yp1 - yp2,2)))return;
		}
		dxt = x2JDate - x1JDate;
		dyt = y2Value - y1Value;

		//Calculate max radius:
		temp=coord.MovePolarDouble(x1JDate,y1Value,radiusSize, atan(dyt/dxt));

		x2JDate = temp.x;
		y2Value = temp.y;		
		x2Value	= pCtrl->GetRecordByPeriodJDate(x2JDate);

		xt2 = x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));
		yt2 = y2 = ownerPanel->GetY(y2Value);
		if(abs(xt2-xt1)<1)increase=true;
	}
	radiusSize = sqrt( pow(xp1 - xp2,2) + pow(yp1 - yp2,2));
	if (upOrDown <= 0){ //down
		// 1/1
		dxt = xt2 - xt1;
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/2
		dxt = 2*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/3
		dxt = 3*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/4
		dxt = 4*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/8
		dxt = 8*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 2/1
		dxt = xt2 - xt1;
		dyt = 2*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 3/1
		dxt = xt2 - xt1;
		dyt = 3*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 4/1
		dxt = xt2 - xt1;
		dyt = 4*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 8/1
		dxt = xt2 - xt1;
		dyt = 8*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	}
	else{ // up
		// 1/1
		dxt = xt2 - xt1;
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/2
		dxt = 2*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/3
		dxt = 3*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/4
		dxt = 4*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 1/8
		dxt = 8*(xt2 - xt1);
		dyt = yt2 - yt1;
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 2/1
		dxt = xt2 - xt1;
		dyt = 2*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 3/1
		dxt = xt2 - xt1;
		dyt = 3*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 4/1
		dxt = xt2 - xt1;
		dyt = 4*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		// 8/1
		dxt = xt2 - xt1;
		dyt = 8*(yt2 - yt1);
		temp = coord.MovePolarDouble(xt1,yt1,radiusSize, -atan(dyt/dxt));
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo((int)temp.x,(int)temp.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	}
	
	
/*
	if(m_Move == cnNone && !drawing){	
		for(int i=0;i<xValues.size();i++)
		{
			
			double dxt = ownerPanel->GetX((int)(xValues[i] - pCtrl->startIndex)) - xt1;
			double dyt = ownerPanel->GetY(yValues[i]) - yt1;
			//temp = coord.MovePolar((double)xt1,(double)yt1,radiusSize, atan(dyt/dxt));
			temp = coord.MovePolarDouble(xt1,yt1,radiusSize, atan(dyt/dxt));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			temp = CPointF(ownerPanel->GetX((int)(xValues[i] - pCtrl->startIndex)),ownerPanel->GetY(yValues[i]));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,RGB(0,0,255), pDC);

		}
	}
	else{
		
		if (upOrDown <= 0){ //down
	#ifdef _CONSOLE_DEBUG
			printf("\nGanFan::Draw()");
			if(!drawn)printf(" !drawn");
			if(m_Move!=cnNone)printf(" m_move!=cnnone");
	#endif
			////First Line
			temp = coord.MovePolar((double)xt1,(double)yt1,radiusSize, coord.DegreeToRad(82.5));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[0].x1 = rect.left;
			gannFan[0].y1 = rect.top;
			gannFan[0].x2 = temp.x;
			gannFan[0].y2 = temp.y;
			//Second Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[1].x1 = rect.left;
			gannFan[1].y1 = rect.top;
			gannFan[1].x2 = temp.x;
			gannFan[1].y2 = temp.y;
			//Third Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(71.25));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[2].x1 = rect.left;
			gannFan[2].y1 = rect.top;
			gannFan[2].x2 = temp.x;
			gannFan[2].y2 = temp.y;
			//Fourth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(63.75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[3].x1 = rect.left;
			gannFan[3].y1 = rect.top;
			gannFan[3].x2 = temp.x;
			gannFan[3].y2 = temp.y;
			//Fifth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(45));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[4].x1 = rect.left;
			gannFan[4].y1 = rect.top;
			gannFan[4].x2 = temp.x;
			gannFan[4].y2 = temp.y;
			//Sixth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(26.25));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[5].x1 = rect.left;
			gannFan[5].y1 = rect.top;
			gannFan[5].x2 = temp.x;
			gannFan[5].y2 = temp.y;
			//Seventh Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(18.75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[6].x1 = rect.left;
			gannFan[6].y1 = rect.top;
			gannFan[6].x2 = temp.x;
			gannFan[6].y2 = temp.y;
			//Eigth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(15));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[7].x1 = rect.left;
			gannFan[7].y1 = rect.top;
			gannFan[7].x2 = temp.x;
			gannFan[7].y2 = temp.y;
			//Nineth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(7.5));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[8].x1 = rect.left;
			gannFan[8].y1 = rect.top;
			gannFan[8].x2 = temp.x;
			gannFan[8].y2 = temp.y;

		} else {

			////First Line
			temp = coord.MovePolar((double)xt1,(double)yt1,radiusSize, coord.DegreeToRad(-82.5));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[0].x1 = rect.left;
			gannFan[0].y1 = rect.top;
			gannFan[0].x2 = temp.x;
			gannFan[0].y2 = temp.y;
			//Second Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[1].x1 = rect.left;
			gannFan[1].y1 = rect.top;
			gannFan[1].x2 = temp.x;
			gannFan[1].y2 = temp.y;
			//Third Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-71.25));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[2].x1 = rect.left;
			gannFan[2].y1 = rect.top;
			gannFan[2].x2 = temp.x;
			gannFan[2].y2 = temp.y;
			//Fourth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-63.75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[3].x1 = rect.left;
			gannFan[3].y1 = rect.top;
			gannFan[3].x2 = temp.x;
			gannFan[3].y2 = temp.y;
			//Fifth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-45));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[4].x1 = rect.left;
			gannFan[4].y1 = rect.top;
			gannFan[4].x2 = temp.x;
			gannFan[4].y2 = temp.y;
			//Sixth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-26.25));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[5].x1 = rect.left;
			gannFan[5].y1 = rect.top;
			gannFan[5].x2 = temp.x;
			gannFan[5].y2 = temp.y;
			//Seventh Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-18.75));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[6].x1 = rect.left;
			gannFan[6].y1 = rect.top;
			gannFan[6].x2 = temp.x;
			gannFan[6].y2 = temp.y;
			//Eigth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-15));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[7].x1 = rect.left;
			gannFan[7].y1 = rect.top;
			gannFan[7].x2 = temp.x;
			gannFan[7].y2 = temp.y;
			//Nineth Line
			temp = coord.MovePolar(xt1,yt1,radiusSize, coord.DegreeToRad(-7.5));
			if(m_Move!=cnNone || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)temp.x,(int)temp.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),temp,lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			gannFan[8].x1 = rect.left;
			gannFan[8].y1 = rect.top;
			gannFan[8].x2 = temp.x;
			gannFan[8].y2 = temp.y;

		}
	}
	*/

/*	
	CRect intRect = CRect((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom);

	double x = 0; double cx = 0;
	double y = 0; double cy = 0;	

	// Draw  three trend lines that go out from	
	// the three horizontal box sections to form the speed lines.
 
	
	//static CRect prevRect;

	for(int n = 0; n != 10; ++n){
		
		bool flip = !(upOrDown<=0);


		// Continue calculation
		int right = intRect.left + ((intRect.right  + MAX_VISIBLE) * ((double)(n + 3) * 0.125));
		cx = right;

		if(flip){
			
			cy =  intRect.top - MAX_VISIBLE;
			if(n == 4) n = 5;
			if(n == 5){
				cx = intRect.right + MAX_VISIBLE;				
			}
			y = intRect.bottom;
			x = intRect.left;
		}
		else{
			
			cy =  intRect.bottom + MAX_VISIBLE;
			y = intRect.top;
			x = intRect.left;			
		}
		
		// Store the individual line
		if(m_Move!=cnNone || !drawn){
			pDC->MoveTo(x,y);
			pDC->LineTo(cx,cy);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(x,y),CPointF(cx,cy),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		gannFan[n].x1 = x;
		gannFan[n].x2 = cx;
		gannFan[n].y1 = y;
		gannFan[n].y2 = cy;

	}*/





	IncludeRects(pDC);
	
	

}

// Special click function for this object
bool CLineStudyGannFan::IsObjectClicked()
{
	bool clicked = IsRegionClicked();	 
	
	if(!clicked){
		for(int n = 0; n != 10; ++n){			
			if(IsClicked(gannFan[n].x1,gannFan[n].y1,gannFan[n].x2,gannFan[n].y2)){
				return true;
			}
		}
	}

	return clicked;
}



// Specialized version of Reset for this line study
void CLineStudyGannFan::Reset()
{

	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	
	x1Value_2 = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value_2 = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value_2 = ownerPanel->GetReverseY(y1);
	y2Value_2 = ownerPanel->GetReverseY(y2);

	x1 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));	
	y1 = ownerPanel->GetY(y1Value);
	y2 = ownerPanel->GetY(y2Value);
	
	x1_2 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2_2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));	
	y1_2 = ownerPanel->GetY(y1Value);
	y2_2 = ownerPanel->GetY(y2Value);
	
	#ifdef _CONSOLE_DEBUG
	printf("\n\nGannFan::Reset(): \n\tx1=%f y1=%f x2=%f y2=%f\n\tx1Value=%f y1Value=%f x2Value=%f y2Value=%f",x1,y1,x2,y2,x1Value,y1Value,x2Value,y2Value);
#endif

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
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
		case Hourly:	
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Minute -= 1;
			x2Julian_2.Minute -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
		case Daily:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Hour -= 1;
			x2Julian_2.Hour -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
		case Weekly:		
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
		case Month:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
		case Year:			
			x2Julian = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value));
			x2Julian_2 = CJulian::FromJDate(ownerPanel->series[0]->GetJDate((int)x2Value_2));
			x2Julian.Day -= 1;
			x2Julian_2.Day -= 1;
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  CJulian::ToJulianDate(x2Julian.Year,x2Julian.Month,x2Julian.Day,x2Julian.Hour,x2Julian.Minute,x2Julian.Second,x2Julian.Millisecond);
			x1JDate_2 = x1JDate;
			x2JDate_2 = x2JDate;
			break;
	}


}

void CLineStudyGannFan::CalculateAngles(){
	// Save the base box:
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	x1 = (int)ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2 = x1 + 10;	
	y1 = (int)ownerPanel->GetY(y1Value);
	y2 = y1 + 10;
	
	//Save the reference points:

	double xp1, xp2, yp1, yp2;
	xp1 = ownerPanel->GetX(0);
	xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
	yp1 = ownerPanel->y1;
	yp2 = ownerPanel->y2;	
	/*xp1 = x1;
	xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
	yp1 = y1;
	if(upOrDown<=0)yp2 = ownerPanel->y2;
	else yp2 = ownerPanel->y2;*/
	double radiusSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));//50; 
	Coordinates coord;
	CPointF temp;
	double angle,signal;
	if(upOrDown<=0)signal=1; //positive angles
	else signal=-1; //negative angles
	xValues.clear();
	yValues.clear();
	////First Line
	angle=signal*82.5F;
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue1=%f",yValues[0]);
#endif
	////Second Line
	angle=signal*75.0F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue2=%f",yValues[1]);
#endif
	////Third Line
	angle=signal*71.25F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue3=%f",yValues[2]);
#endif
	////Fourth Line
	angle=signal*63.75F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue4=%f",yValues[3]);
#endif
	////Fifth Line
	angle=signal*45.0F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue5=%f",yValues[4]);
#endif
	////Sixth Line
	angle=signal*26.25F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue6=%f",yValues[5]);
#endif
	////Seventh Line
	angle=signal*18.75F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue7=%f",yValues[6]);
#endif
	////Eighth Line
	angle=signal*15.0F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue8=%f",yValues[7]);
#endif
	////Ninth Line
	angle=signal*7.5F;
	temp = coord.MovePolarDouble(x1,y1,radiusSize, coord.DegreeToRad(angle));
	//temp = coord.MovePolar((double)x1,(double)y1,radiusSize, coord.DegreeToRad(angle));
	xValues.push_back(ownerPanel->GetReverseX(temp.x) + 1 + pCtrl->startIndex);
	yValues.push_back(ownerPanel->GetReverseY(temp.y));
#ifdef _CONSOLE_DEBUG
	printf("\n\tyValue9=%f",yValues[8]);
#endif
#ifdef _CONSOLE_DEBUG
	printf("\nGanFan::CalculateAngles()  xValues=%d yValues=%d",xValues.size(),yValues.size());
#endif
}