/*#include "stdafx.h"
#include "LineStudyPolyline.h"


CLineStudyPolyline::CLineStudyPolyline(void)
{
}


CLineStudyPolyline::~CLineStudyPolyline(void)
{
}*/



// LineStudyPolyline.cpp: implementation of the CLineStudyPolyline class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "math.h"
#include "StockChartX.h"
#include "LineStudyPolyline.h"
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

CLineStudyPolyline::CLineStudyPolyline(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner)
{
	objectType = "Polyline";
	nType = lsPolyline;
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
	startX = 0.0f;
	startY = 0.0f;
	key = Key;
	x1 = y1 = x2 = y2 = 0.0f;
	oldRect = newRect = fixedLine = CRectF(NULL_VALUE,NULL_VALUE,NULL_VALUE,NULL_VALUE);
	lineWeight = pCtrl->lineThickness;	

}


CLineStudyPolyline::~CLineStudyPolyline()
{

}

CPoint CLineStudyPolyline::MovePolar(double x, double y, double radius, double theta){
	CPoint pointReturn;
	pointReturn.x = x + radius*cos(theta);
	pointReturn.y = y + radius*sin(theta);
	return pointReturn;
}


void CLineStudyPolyline::OnPaint(CDC *pDC)
{
	if(false/*drawing || (selected && pCtrl->movingObject)*/ )return;
	CPen	pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
		
	if(xValues.size()>1) ExcludeRects(pDC);

	if(xValues.size()>1){
		for(int i=0;i<xValues.size()-1;i++){
			double tempX1 = pCtrl->panels[0]->GetX((xValues[i]-pCtrl->startIndex),true);
			double tempY1 = ownerPanel->GetY(yValues[i]);
			double tempX2 = pCtrl->panels[0]->GetX((xValues[i+1]-pCtrl->startIndex),true);
			double tempY2 = ownerPanel->GetY(yValues[i+1]);
			if((xValues[i]>xValues[i+1]?xValues[i]>pCtrl->startIndex:xValues[i+1]>pCtrl->startIndex)){
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(tempX1,tempY1);
					pDC->LineTo(tempX2,tempY2);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(tempX1,tempY1),CPointF(tempX2,tempY2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
			}
			
		}
	}

	if(y1Value != NULL_VALUE) IncludeRects(pDC);

	pDC->SelectObject(pOldPen);

	// If selected, paint end point selection boxes while dragging
	if(selected)
	{
		
		CBrush	br( lineColor );
		for(int i=0;i<xValues.size();i++){	
			pCtrl->pdcHandler->FillRectangle(CRectF(pCtrl->panels[0]->GetX(xValues[i]-pCtrl->startIndex)-3,ownerPanel->GetY(yValues[i])-3,pCtrl->panels[0]->GetX(xValues[i]-pCtrl->startIndex)+3,ownerPanel->GetY(yValues[i])+3),lineColor, pDC);
			//pDC->FillRect( CRect(), &br );
		}
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();
	}

	pen.DeleteObject();
	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyPolyline::OnLButtonUp(CPoint point)
{

	if(drawing) return;
	
	if(selected){
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
	
	}


	if(bReset)
	{
		//Reset();
		bReset = false;
	}

	// Get current location based on scale
	//bool clicked = IsClicked(x1,y1,x2,y2);
	bool clicked = IsPointsClicked(xValues,yValues);

	if(selected/* && oldRect.bottom != 0 && oldRect.right != 0*/)
	{
		/*
		x1 = newRect.left;
		x2 = newRect.right;
		y1 = newRect.top;
		y2 = newRect.bottom;*/

		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->movingObject = false;
		//newRect = CRect(0,0,0,0);
		//oldRect = newRect;
  		Reset();
		if(!clicked) selected = false;		
		ownerPanel->Invalidate();
		pCtrl->UnSelectAll();
		pCtrl->changed = true;
		if(clicked) selected = true;
		pCtrl->RePaint();	
		pCtrl->SaveUserStudies();
		pCtrl->UpdateScreen(false);	
	}
	
	moveX1 = false;
	moveX2 = false;
	moveCtr = false;
	buttonState = MOUSE_UP;
	pCtrl->dragging = false;
	pCtrl->movingObject = false;
}

void CLineStudyPolyline::OnMouseMove(CPoint point)
{
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing && startX > 0 && startY > 0){ // XOR for line drawing
		XORDraw(2, point);
		return;
	}

	if(!pCtrl->m_Cursor == 0) return;	

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	//bool clicked = IsClicked(x1,y1,x2,y2);	
	bool clicked = IsPointsClicked(xValues,yValues);

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
		for(int i=0;i<xValues.size()-1;i++){
			int tempX1 = (long)pCtrl->panels[0]->GetX(xValues[i]-pCtrl->startIndex);
			int tempY1 = (long)ownerPanel->GetY(yValues[i]);
			int tempX2 = (long)pCtrl->panels[0]->GetX(xValues[i+1]-pCtrl->startIndex);
			int tempY2 = (long)ownerPanel->GetY(yValues[i+1]);
			//FLIPPED?
			if(tempX1>tempX2)
			{			
				pDC->MoveTo(tempX2,tempY2);
				pDC->LineTo(tempX1,tempY1);	
			}
			else{
				pDC->MoveTo(tempX1,tempY1);
				pDC->LineTo(tempX2,tempY2);		
			}
		}
		/*
		pDC->MoveTo(oldRect.left,oldRect.top);
		pDC->LineTo(oldRect.right,oldRect.bottom);
		*/

		if(moveX1){
			xValues[pointMove]=(long)pCtrl->panels[0]->GetReverseX(pointF.x)+pCtrl->startIndex+1; //Get real X Record
			yValues[pointMove]=ownerPanel->GetReverseY(pointF.y);  //Get real Y JDate
		
		}
		else if(moveCtr){// Just move the entire line (don't resize it)
			for(int i=0;i<xValues.size();i++){
				int xtemp = xValues[i];
				xValues[i]= pCtrl->panels[0]->GetReverseX(pCtrl->panels[0]->GetX(xValues[i])+(pointF.x-startX))+1;
				yValues[i]=ownerPanel->GetReverseY(ownerPanel->GetY(yValues[i])+(pointF.y-startY));
			}
			
		}
		
		startX=pointF.x;
		startY=pointF.y;

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
		for(int i=0;i<xValues.size()-1;i++){
			int tempX1 = pCtrl->panels[0]->GetX(xValues[i]-pCtrl->startIndex);
			int tempY1 = ownerPanel->GetY(yValues[i]);
			int tempX2 = pCtrl->panels[0]->GetX(xValues[i+1]-pCtrl->startIndex);
			int tempY2 = ownerPanel->GetY(yValues[i+1]);			
			//FLIPPED?
			if(tempX1>tempX2)
			{			
				pDC->MoveTo(tempX2,tempY2);
				pDC->LineTo(tempX1,tempY1);	
			}
			else{
				pDC->MoveTo(tempX1,tempY1);
				pDC->LineTo(tempX2,tempY2);		
			}	
		}
		/*
		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);		*/

		IncludeRects(pDC);

		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}

}

void CLineStudyPolyline::OnLButtonDown(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return;

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	m_valueView.Reset(true);
	//if(IsClicked(x1,y1,x2,y2) && selectable){
	if(IsPointsClicked(xValues,yValues) && selectable){
		pCtrl->UnSelectAll();
		if(pCtrl->SelectCount() == 0 || selected){			
			oldRect = CRectF(0.0f,0.0f,0.0f,0.0f);
			newRect = oldRect;
			pCtrl->movingObject = true;						
			pCtrl->RePaint();
			if(buttonState != MOUSE_DOWN){
				startX = pointF.x;
				startY = pointF.y;
			}
			for(int i=0;i<xValues.size()-1;i++){
				double tempX1 = pCtrl->panels[0]->GetX((int)(xValues[i]-pCtrl->startIndex));
				double tempY1 = ownerPanel->GetY(yValues[i]);
				double tempX2 = pCtrl->panels[0]->GetX(xValues[i+1]-pCtrl->startIndex);
				double tempY2 = ownerPanel->GetY(yValues[i+1]);
				// Can we move this line?
				if(pointF.x > tempX1-10 && pointF.y > tempY1-10 && pointF.x < tempX1+10 && pointF.y < tempY1+10){
					moveX1 = true;		
					moveCtr = false;
					pointMove = i;
					break;
				}
				else if(pointF.x > tempX2-10 && pointF.y > tempY2-10 && pointF.x < tempX2+10 && pointF.y < tempY2+10){
					moveX1 = true;	
					moveCtr = false;
					pointMove = i+1;
					break;
				}
				else{
					if(IsClicked(tempX1,tempY1,tempX2,tempY2)){
						moveX1 = false;	
						moveCtr = true;
					}
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

void CLineStudyPolyline::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}

	switch(MsgID){
		case MSG_POINTOUT:
		    //if( !selected && IsClicked(x1,y1,x2,y2) && !pCtrl->movingObject) {
			if( !selected && IsPointsClicked(xValues,yValues) && !pCtrl->movingObject) {
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

void CLineStudyPolyline::DisplayInfo()
{
	if(drawing) return;
	//if(!IsClicked(x1,y1,x2,y2)) return;
	if(!IsPointsClicked(xValues,yValues)) return;
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
	text += "X1   " + datetime + char(13) + char(10);
	revX = (int)ownerPanel->GetReverseX(x2) + pCtrl->startIndex;
	if(revX < (int)ownerPanel->series[0]->data_slave.size())  // Added 1/22/06
		datetime = pCtrl->FromJDate(ownerPanel->series[0]->data_slave[revX].jdate);
	else
		datetime = "Invalid DateTime."; // Added 1/22/06
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

void CLineStudyPolyline::XORDraw(UINT nFlags, CPoint point)
{		
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	if(nFlags == 1){ // First point
		state = 0;
		startX = pointF.x;
		startY = pointF.y;
		startPoint.x = pointF.x;
		startPoint.y = pointF.y;
		//points.push_back(startPoint);
		xValues.push_back((double)(pCtrl->panels[0]->GetReverseX(startPoint.x)+pCtrl->startIndex+1));
		yValues.push_back(ownerPanel->GetReverseY(startPoint.y));
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
		//pCtrl->UpdateRect(newRect);
		ExcludeRects(pDC);
		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);		
		IncludeRects(pDC);
		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;
		pCtrl->ReleaseScreen(pDC);


	}
	else if(nFlags == 3)	//	Middle point
	{
		
		if (!(abs(pCtrl->panels[0]->GetX(xValues[xValues.size() - 1]) - pointF.x)>5.0 && abs(ownerPanel->GetY(yValues[yValues.size() - 1]) - pointF.y) > 5.0)){
		
			return;
		}

		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
		int temp = 0;

		startX = (double)pointF.x;
		startY = (double)pointF.y;
		startPoint.x = (double)pointF.x;
		startPoint.y =(double)pointF.y;
		//points.push_back(startPoint);
		xValues.push_back((double)(pCtrl->panels[0]->GetReverseX(startPoint.x)+pCtrl->startIndex+1));
		yValues.push_back(ownerPanel->GetReverseY(startPoint.y));
		drawing = true;		
		pCtrl->movingObject = false;
		pCtrl->RePaint();
		pCtrl->changed = true;
		selected = false;
		//Reset();   VINICIUS
		




	
	}
	else if(nFlags == 4)	//	Last point
	{	
		if(pCtrl->panels[0]->GetReverseX(pointF.x)+pCtrl->startIndex+1==xValues[xValues.size()-1] && ownerPanel->GetReverseY(pointF.y)==yValues[yValues.size()-1]) {
			
			if(xValues.size()<2) return;
		}
		else{			
			//Dont add last point twice:
			if (abs(pCtrl->panels[0]->GetX(xValues[xValues.size() - 1]) - pointF.x)>5.0 && abs(ownerPanel->GetY(yValues[yValues.size() - 1]) - pointF.y) > 5.0){
				xValues.push_back(pCtrl->panels[0]->GetReverseX(pointF.x) + pCtrl->startIndex + 1);
				yValues.push_back(ownerPanel->GetReverseY(pointF.y));
				endPoint = CPoint((int)pointF.x, (int)pointF.y);
			}
		}
		startX = 0;
		startY = 0;
		pCtrl->movingObject = false;
		pCtrl->RePaint();
		pCtrl->changed = true;
		pCtrl->objectSelected = pCtrl->resizing = pCtrl->dragging = pCtrl->drawing = pCtrl->lineDrawing = pCtrl->typing = false;
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

		pCtrl->OnUserDrawingComplete(lsPolyline, key);
		pCtrl->SaveUserStudies();
		
	}
}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyPolyline::OnRButtonUp(CPoint point)
{		
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyPolyline::OnDoubleClick(CPoint point)
{	
#ifdef _CONSOLE_DEBUG
	printf("\nDoubleClick() %s",key);
#endif
	if(drawing){	
		state = 2;
		XORDraw(4, point);
		//pCtrl->lineDrawing = false;
		pCtrl->m_mouseState = MOUSE_NORMAL;
		pCtrl->m_Cursor = 0;
		drawing=false;
		drawn=true;
	}
	else{
		Reset();
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
}
/////////////////////////////////////////////////////////////////////////////

// Create a CLineStandard,
// set it's x1,y1,x2,y2 and call this.
void CLineStudyPolyline::SnapLine()
{
	startX = 0;
	startY = 0;
	pCtrl->movingObject = false;
	pCtrl->RePaint();
	pCtrl->changed = true;
	selected = false;
	Reset();
	drawing = false;	
	drawn = true;
}
/////////////////////////////////////////////////////////////////////////////
// Sets chart value lookup based on actual pixel position
void CLineStudyPolyline::Reset()
{
	//return;
	xJDates.clear();
	if(xValues.size()>1){
		for(int i=0;i<xValues.size();i++){
			switch(pCtrl->GetPeriodicity())
			{
				case Minutely:	
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
				case Hourly:	
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
				case Daily:		
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
				case Weekly:	
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
				case Month:		
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
				case Year:		
					xJDates.push_back(ownerPanel->series[0]->GetJDate((int)xValues[i]-1));
					break;
			}
		}
	}
	
#ifdef _CONSOLE_DEBUG
	printf("\nPolyline::Reset() xValues=%d xJDates=%d",xValues.size(),xJDates.size());
	//for(int i=0;i<)
#endif
//	End Of Revision
}