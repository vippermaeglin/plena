// LineStudyTriangle.cpp: implementation of the CLineStudyTriangle class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "math.h"
#include "StockChartX.h"
#include "LineStudyTriangle.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CLineStudyTriangle::CLineStudyTriangle(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "Triangle";
	nType = lsTriangle;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	lineColor = color;
	Initialize();
	drawn = false;
	drawing = false;
	displayed = false;
	bReset = true;
	xor = false;
	dragging = false;
	moveCtr = false;
	moveX1 = false;
	moveX2 = false;
	buttonState = 0;
	startX = 0;
	startY = 0;
	key = Key;
	x1 = y1 = x2 = y2 = 0;
	oldRect = newRect = fixedLine = CRectF(NULL_VALUE,NULL_VALUE,NULL_VALUE,NULL_VALUE);
	lineWeight = pCtrl->lineThickness;	

}


CLineStudyTriangle::~CLineStudyTriangle()
{

}

CPoint CLineStudyTriangle::MovePolar(double x, double y, double radius, double theta){
	CPoint pointReturn;
	pointReturn.x = x + radius*cos(theta);
	pointReturn.y = y + radius*sin(theta);
	return pointReturn;
}


void CLineStudyTriangle::OnPaint(CDC *pDC)
{
	if(drawing || (selected && pCtrl->movingObject) )
		return;


	if(x1 > pCtrl->width - pCtrl->yScaleWidth) 
	{		
		return;
	}


	if (!moveCtr && !moveX1 && !moveX2)Update();

	CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
		
	if(y1Value != NULL_VALUE) ExcludeRects(pDC);
	

//#ifdef TENT01
	if(moveX1 || moveX2 || moveCtr || !drawn){
		pDC->MoveTo(x1,y1);
		pDC->LineTo(x2,y2);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(x1,y1),CPointF(x2,y2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	double pi = 3.14159265;
	if (!isFirstPointArrow) {
		double dx = (double)(x2-x1);
		double dy = (double)(y2-y1);	    
		CPoint c1 = MovePolar(x2, y2, 5, atan((double)dy/dx) - pi/2);
		CPoint c2 = MovePolar(x2, y2, 5, atan((double)dy/dx) + pi/2);
		CPoint c3 = MovePolar(x2, y2, 10, atan((double)dy/dx));
		if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(x2,y2);
			pDC->LineTo(c1);
			pDC->LineTo(c3);
			pDC->LineTo(c2);
			pDC->LineTo(x2,y2);
		}
		else {
			pCtrl->pdcHandler->DrawLine(CPointF(x2,y2),CPointF((double)c1.x,(double)c1.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c1.x,(double)c1.y),CPointF((double)c3.x,(double)c3.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c3.x,(double)c3.y),CPointF((double)c2.x,(double)c2.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c2.x,(double)c2.y),CPointF(x2,y2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		}
	}
	else {
		double dx = (double)(x1-x2);
		double dy = (double)(y1-y2);
	    CPoint c1 = MovePolar(x1, y1, 5, atan((double)dy/dx) - pi/2);
		CPoint c2 = MovePolar(x1, y1, 5, atan((double)dy/dx) + pi/2);
		CPoint c3 = MovePolar(x1, y1, 10, atan((double)dy/dx) - pi);if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(x1,y1);
			pDC->LineTo(c1);
			pDC->LineTo(c3);
			pDC->LineTo(c2);
			pDC->LineTo(x1,y1);
		}
		else {
			pCtrl->pdcHandler->DrawLine(CPointF(x1,y1),CPointF((double)c1.x,(double)c1.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c1.x,(double)c1.y),CPointF((double)c3.x,(double)c3.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c3.x,(double)c3.y),CPointF((double)c2.x,(double)c2.y),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			pCtrl->pdcHandler->DrawLine(CPointF((double)c2.x,(double)c2.y),CPointF(x1,y1),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		}

	}
//#endif

	if(y1Value != NULL_VALUE) IncludeRects(pDC);

	pDC->SelectObject(pOldPen);

	// If selected, paint end point selection boxes while dragging
	if(selected)
	{
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y1+3,x1-3,y1-3),lineColor, pDC);
		if(x2 < pCtrl->width - pCtrl->yScaleWidth){
			pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		}
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();

	}

	pen.DeleteObject();


 		

	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyTriangle::OnLButtonUp(CPoint point)
{

	if(drawing) return;

	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
		pCtrl->SaveUserStudies();
	}

	

	if(bReset)
	{
		Reset();
		bReset = false;
	}

	// Get current location based on scale
	bool clicked = IsClicked(x1,y1,x2,y2);

	if(selected && oldRect.bottom != 0 && oldRect.right != 0)
	{
		x1 = newRect.left;
		x2 = newRect.right;
		y1 = newRect.top;
		y2 = newRect.bottom;

		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->movingObject = false;
		moveX1 = false;
		moveX2 = false;
		moveCtr = false;
		newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
		oldRect = newRect;
  		Reset();
		if(!clicked) selected = false;		
		ownerPanel->Invalidate();
		pCtrl->UnSelectAll();
		pCtrl->changed = true;
		if(clicked) selected = true;
		pCtrl->RePaint();
		pCtrl->UpdateScreen(false);				
		pCtrl->SaveUserStudies();
	}
	
	moveX1 = false;
	moveX2 = false;
	moveCtr = false;
	buttonState = MOUSE_UP;
	pCtrl->dragging = false;
	pCtrl->movingObject = false;
}

void CLineStudyTriangle::OnMouseMove(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(drawing && startX > 0 && startY > 0){ // XOR for line drawing
		XORDraw(2, point);
		return;
	}

	if(!pCtrl->m_Cursor == 0) return;	

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	bool clicked = IsClicked(x1,y1,x2,y2);	

	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key);

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
	
	if((moveX1 || moveX2 || moveCtr) && buttonState == MOUSE_DOWN && selected){

		CDC* pDC = pCtrl->GetScreen();
		
		pDC->SetROP2(R2_NOT);

		ExcludeRects(pDC);
		pDC->MoveTo(oldRect.left,oldRect.top);
		pDC->LineTo(oldRect.right,oldRect.bottom);
	//	IncludeRects(pDC);
		

		// Flip coordinates
		if(moveX1){
			if(pointF.x <= x2){
				newRect.left = pointF.x;
				newRect.right = x2;
				x1 = pointF.x;
				y1 = pointF.y;		
			}
			else {
				newRect.left = x2;
				newRect.right = pointF.x;
				x1 = x2;
				x2 = pointF.x;
				y1 = y2;
				y2 = pointF.y;
				moveX1 = false;
				moveX2 = true;
				isFirstPointArrow = !isFirstPointArrow;
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(moveX2){
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
				moveX2 = false;
				moveX1 = true;
				isFirstPointArrow = !isFirstPointArrow;
			}
			newRect.top = y1;
			newRect.bottom = y2;
		}
		else if(moveCtr){// Just move the entire line (don't resize it)
			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);
		}

		// Check bounds
		int next = 0;
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
		

		ExcludeRects(pDC);
		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);		

		IncludeRects(pDC);

		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}

void CLineStudyTriangle::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return;

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	m_valueView.Reset(true);
	if(IsClicked(x1,y1,x2,y2) && selectable){
		pCtrl->UnSelectAll();
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRectF(0,0,0,0);
			newRect = oldRect;
			pCtrl->movingObject = true;						
			pCtrl->RePaint();
			if(buttonState != MOUSE_DOWN){
				startX = pointF.x;
				startY = pointF.y;
			}
			// Can we move this line?
			if(pointF.x > x1-10 && pointF.y > y1-10 && pointF.x < x1+10 && pointF.y < y1+10){
				moveX1 = true;		
			}
			else if(pointF.x > x2-10 && pointF.y > y2-10 && pointF.x < x2+10 && pointF.y < y2+10){
				moveX2 = true;		
			}
			else{
				if(IsClicked(x1,y1,x2,y2)){
					moveCtr = true; 		
				}
			}
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

void CLineStudyTriangle::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}

	switch(MsgID){
		case MSG_POINTOUT:
		    if( !selected && IsClicked(x1,y1,x2,y2) && !pCtrl->movingObject) {
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

void CLineStudyTriangle::DisplayInfo()
{
	if(drawing) return;
	if(!IsClicked(x1,y1,x2,y2)) return;
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
	text += "X1   " + datetime + char(13) + char(10);
	revX = ownerPanel->GetReverseX(x2) + pCtrl->startIndex;
	if(revX < ownerPanel->series[0]->data_slave.size())  // Added 1/22/06
		datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	else
		datetime = "Invalid DateTime."; // Added 1/22/06
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
	text += "Angle   " + temp + "°";
	m_valueView.Text = text;
	m_valueView.Connect(pCtrl);
	m_valueView.overidePoints = true;
	m_valueView.x1 = pointF.x + 10;
	m_valueView.y1 = pointF.y + 21;
	m_valueView.x2 = pointF.x + 155;
	m_valueView.y2 = pointF.y + 88;
	m_valueView.Show();
}

void CLineStudyTriangle::XORDraw(UINT nFlags, CPoint point)
{		
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	if(nFlags == 1){ // First point
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;		
	}
	else if(nFlags == 2){ // Drawing
		CDC* pDC = pCtrl->GetScreen();				//set draw to screen
		pDC->SetROP2(R2_NOT);						//select NOT draw mode
		ExcludeRects(pDC);							//exclude labels areas
		pDC->MoveTo(oldRect.left,oldRect.top);		
		pDC->LineTo(oldRect.right,oldRect.bottom);
		IncludeRects(pDC);
		newRect.left = startX;
		newRect.right = pointF.x;
		newRect.top = startY;
		newRect.bottom = pointF.y;
		int next = 0;
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
		pCtrl->UpdateRect(CRect(newRect.left,newRect.top,newRect.right, newRect.bottom));
		ExcludeRects(pDC);
		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);		
		IncludeRects(pDC);
		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;
		pCtrl->ReleaseScreen(pDC);
	}
	else if(nFlags == 3)	//	Last pointF
	{
		isFirstPointArrow = false;
		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
		int temp = 0;
		if(x1 > x2)
		{
			isFirstPointArrow = true;
			temp = x2;
			x2 = x1;
			x1 = temp;
			temp = y2;
			y2 = y1;
			y1 = temp;
		}
		startX = 0.0f;
		startY = 0.0f;
		pCtrl->movingObject = false;
		pCtrl->RePaint();
		pCtrl->changed = true;
		selected = false;
		Reset();
		drawing = false;		
		drawn = true;



		/*	SGC	03.06.2004	BEG	*/
		CDC* pDC = pCtrl->GetScreen();
		this->OnPaint( pDC );
		pCtrl->ReleaseScreen( pDC ); // Release added 9/29/05
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/

		pCtrl->OnUserDrawingComplete(lsTriangle, key);
		pCtrl->SaveUserStudies();
		
	}
}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyTriangle::OnRButtonUp(CPoint point)
{		
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyTriangle::OnDoubleClick(CPoint point)
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

// Create a CLineStandard,
// set it's x1,y1,x2,y2 and call this.
void CLineStudyTriangle::SnapLine()
{
	startX = 0.0f;
	startY = 0.0f;
	pCtrl->movingObject = false;
	pCtrl->RePaint();
	pCtrl->changed = true;
	selected = false;
	Reset();
	drawing = false;	
	drawn = true;
}
/////////////////////////////////////////////////////////////////////////////