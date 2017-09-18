// LineStudyChannel.cpp: implementation of the CLineStandard class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStudyChannel.h"
#include "Coordinates.h"
#include <math.h>



//#define _CONSOLE_DEBUG 

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CLineStudyChannel::CLineStudyChannel(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner, bool radiusExtensionParameter /*= false*/)
{
	objectType = "Channel";
	nType = lsChannel;
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
	flipped = false;
	moveDistX1 = false;
	moveDistX2 = false;

	//Testing left extension
	setLeftExtension(false);

	//Testing right extension
	setRightExtension(true);

	//Testing radius extension
	setRadiusExtension(radiusExtensionParameter);
	
	buttonState = 0;
	startX = 0.0f;
	startY = 0.0f;
	key = Key;
	x1 = y1 = x2 = y2 = x1_2 = y1_2 = x2_2 = y2_2 = 0.0f;

	fixedLine = oldRect = newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
	drawNumber = 0;
	
	//New Variables for the LineStudyChannel
	theta = 0;
	distance = 0;
	newRect_2 = CRectF(0.0f,0.0f,0.0f,0.0f);
	oldRect_2 = CRectF(0.0f,0.0f,0.0f,0.0f);
	state = 0;
	
	lineWeight = pCtrl->lineThickness;	

}

CLineStudyChannel::~CLineStudyChannel()
{

}


//Gets and sets
bool CLineStudyChannel::isLeftExtension(){
	return this->leftExtension;
}
bool CLineStudyChannel::isRightExtension(){
	return this->rightExtension;
}
bool CLineStudyChannel::isRadiusExtension(){
	return this->radiusExtension;
}
void CLineStudyChannel::setLeftExtension(bool left){
	this->leftExtension = left;
}
void CLineStudyChannel::setRightExtension(bool right){
	this->rightExtension = right;
}
void CLineStudyChannel::setRadiusExtension(bool radius){
	this->radiusExtension = radius;
}


void CLineStudyChannel::DrawLineStudy(CDC* pDC, CRectF rect){

#ifdef _CONSOLE_DEBUG
	printf("\nChannel::DrawLineStudy()");
#endif

	/*if(y1Value != NULL_VALUE)*/ ExcludeRects(pDC);
	double xt1, xt2, yt1, yt2;

	xt1 = rect.left;	
	xt2 = rect.right;
	yt1 = rect.top;
	yt2 = rect.bottom;

	
			
	if(rect.left==0 && rect.top==0 && rect.right==0 && rect.bottom==0) {
		
#ifdef _CONSOLE_DEBUG
	printf("  RETURN!");
#endif
		return;
	}

		
	if(moveX1 || moveX2 || moveDistX1 || moveDistX2 || moveCtr || !drawn){
		pDC->MoveTo(xt1,yt1);
		pDC->LineTo(xt2,yt2);
	}
	else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),CPointF(xt2,yt2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	//New Section
	double PI =   3.14159265358979;
	Coordinates coord; 
	

	//OBSOLETO:
	//Radius extension	
	/*if(isRadiusExtension()){
		double xp1, xp2, yp1, yp2;
		xp1 = ownerPanel->GetX(0);
		xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
		yp1 = ownerPanel->y1;
		yp2 = ownerPanel->y2;
		double radiusSize = sqrt( pow(xp1 - xp2,2) + pow(yp1 - yp2,2));
			
		CPen* pen = new CPen( PS_DOT, lineWeight+pointOutStep, lineColor );
		CPen* pOldPen = pDC->SelectObject( pen );	
		
		
		
		if(xt1 > xt2){
			double dx = (double) xt1 - xt2;
			double dy = (double) yt1 - yt2;
			//double L = sqrt(pow(dx,2)+pow(dy,2));
			//distance = pointF.y-y2;
			CPointF p = coord.MovePolar((double)xt1,(double)yt1, radiusSize, atan(dy/dx)- PI);
			//CPointF p = CPointF(xt2+distance*(-dy)/radiusSize,yt2+distance*dx/radiusSize);
			if(moveX1 || moveX2 || moveDistX1 || moveDistX2 || moveCtr || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)p.x,(int)p.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),p,lineWeight+pointOutStep,2,lineColor, pDC);
				
		}else{
			double dx = (double) xt2 - xt1;
			double dy = (double) yt2 - yt1; 
			//double L = sqrt(pow(dx,2)+pow(dy,2));
			//distance = pointF.y-y2;
			CPointF p = coord.MovePolar((double)xt2,(double)yt2, radiusSize, atan(dy/dx)); 
			//CPointF p = CPointF(xt1+distance*(-dy)/radiusSize,yt1+distance*dx/radiusSize);
			if(moveX1 || moveX2 || moveDistX1 || moveDistX2 || moveCtr || !drawn){
				pDC->MoveTo(xt2,yt2);
				pDC->LineTo((int)p.x,(int)p.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt2,yt2),p,lineWeight+pointOutStep,2,lineColor, pDC);
		}		
	}*/


	/*if(y1Value != NULL_VALUE)*/ IncludeRects(pDC);
		
}


void CLineStudyChannel::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	printf("\nChannel::OnPaint()");
#endif

	if(drawing || moveX1 || moveX2 || moveCtr /*|| moveDistX1 || moveDistX2 */) {
#ifdef _CONSOLE_DEBUG
	printf(" RETURN!!!");
#endif

	// Draw first line	
	//DrawLineStudy(pDC, CRectF(newRect.left,newRect.top,newRect.right,newRect.bottom));	
	// Draw second line
	//DrawLineStudy(pDC, CRectF(newRect_2.left,newRect_2.top,newRect_2.right,newRect_2.bottom));	
		return;
	}


	if (!moveCtr && !moveX1 && !moveX2)Update();

	CPen pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	
	// Sometimes catch unknown exception
	//try{
		// Draw first line	
		DrawLineStudy(pDC, CRectF(x1,y1,x2,y2));	


		// Update second line by distance from x1:		
		//Temporaries
		Coordinates coord;
		CPointF p_line1;
		CPointF p_line2;
		double dx, dy, theta;
		dy = y2 - y1; 
		dx = x2 - x1;
		theta = atan(dy/dx);
		double L = sqrt(pow(dx,2)+pow(dy,2));
		distance = sqrt(pow(y1_2-y1,2)+pow(x1_2-x1,2));
		if(abs(distance)<20.0F)
		{
			if(distance<0)distance = -20.0F;
			else distance = 20.0F;
		}
		
		/*x1_2 = x1+distance*(-dy)/L;
		x2_2 = x2+distance*(-dy)/L;
		y1_2 = y1+distance*dx/L;
		y2_2 = y2+distance*dx/L;*/

		// Draw second line
		if(!(moveDistX1 || moveDistX2))DrawLineStudy(pDC, CRectF(x1_2,y1_2,x2_2,y2_2));
	//}
	//catch(...)
	//{
		
	//}
	pDC->SelectObject(pOldPen);

	double PI = 3.14159265;
	if(isRightExtension()){
		double xp1, xp2, yp1, yp2;
		xp1 = ownerPanel->GetX(0);
		xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
		yp1 = ownerPanel->y1;
		yp2 = ownerPanel->y2;
		double extensionSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));
		ExcludeRects(pDC);
		CPen* pen = new CPen( PS_DOT, lineWeight+pointOutStep, lineColor );
	    pOldPen = pDC->SelectObject( pen );

		//Draw first extension
		double dx =  x2 - x1;
		double dy =  y2 - y1;
		//double L = sqrt(pow(dx,2)+pow(dy,2));
		//distance = pointF.y-y2;
		//CPointF p = CPointF(x2+(yp2-yp1)*(-dy)/extensionSize,y2+(xp1-xp2)*(dx)/extensionSize);
		rightPoint = coord.MovePolar(x2, y2, extensionSize, atan(dy/dx));
		if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(x2,y2);
			pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(x2,y2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);
		//Draw second extension
		if(!(moveDistX1 || moveDistX2)){
			dx =  x2_2 - x1_2;
			dy =  y2_2 - y1_2;
			//L = sqrt(pow(dx,2)+pow(dy,2));
			//distance = pointF.y-y2;
			//p = CPointF(x1_2+distance*(-dy)/L,y1_2+distance*dx/L);
			rightPoint_2 = coord.MovePolar(x2_2, y2_2, extensionSize, atan(dy/dx));
			if(moveX1 || moveX2 || moveCtr || !drawn){
				pDC->MoveTo(x2_2,y2_2);
				pDC->LineTo((int)rightPoint_2.x,(int)rightPoint_2.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(x2_2,y2_2),rightPoint_2,lineWeight+pointOutStep,2,lineColor, pDC);
		}
		//Select oldPen
		pDC->SelectObject(pOldPen);
		IncludeRects(pDC);
	}
	
	if(isLeftExtension()){
		double xp1, xp2, yp1, yp2;
		xp1 = ownerPanel->GetX(0);
		xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
		yp1 = ownerPanel->y1;
		yp2 = ownerPanel->y2;		
		double extensionSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));
		ExcludeRects(pDC);
		CPen* pen = new CPen( PS_DOT, lineWeight+pointOutStep, lineColor );
	    pOldPen = pDC->SelectObject( pen );

		//Draw first extension
		double dx =  x1 - x2;
		double dy =  y1 - y2;
		//double L = sqrt(pow(dx,2)+pow(dy,2));
		//distance = pointF.y-y2;
		//CPointF p = CPointF(x2+(yp2-yp1)*(-dy)/extensionSize,y2+(xp1-xp2)*(dx)/extensionSize);
		leftPoint = coord.MovePolar(x1, y1, extensionSize, atan(dy/dx)- PI);
		if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(x1,y1);
			pDC->LineTo((int)leftPoint.x,(int)leftPoint.y);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(x1,y1),leftPoint,lineWeight+pointOutStep,2,lineColor, pDC);
		//Draw second extension
		if(!(moveDistX1 || moveDistX2)){
			dx =  x1_2 - x2_2;
			dy =  y1_2 - y2_2;
			//L = sqrt(pow(dx,2)+pow(dy,2));
			//distance = pointF.y-y2;
			//p = CPointF(x1_2+distance*(-dy)/L,y1_2+distance*dx/L);
			leftPoint_2 = coord.MovePolar(x1_2, y1_2, extensionSize, atan(dy/dx)-PI);
			if(moveX1 || moveX2 || moveCtr || !drawn){
				pDC->MoveTo(x1_2,y1_2);
				pDC->LineTo((int)leftPoint_2.x,(int)leftPoint_2.y);
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(x1_2,y1_2),leftPoint_2,lineWeight+pointOutStep,2,lineColor, pDC);
		}
		//Select oldPen
		pDC->SelectObject(pOldPen);
		IncludeRects(pDC);
	}

	// If selected, paint end point selection boxes while dragging
	if(selected && !(moveDistX1 || moveDistX2))
	{
		CBrush	br( lineColor );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1+3,y1+3,x1-3,y1-3),lineColor, pDC);
		if(x2 < pCtrl->width - pCtrl->yScaleWidth)
				pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		CBrush br1( 0x00ff );
		pCtrl->pdcHandler->FillRectangle(CRectF(x1_2+3,y1_2+3,x1_2-3,y1_2-3),lineColor, pDC);
		if(x2 < pCtrl->width - pCtrl->yScaleWidth)
				pCtrl->pdcHandler->FillRectangle(CRectF(x2_2+3,y2_2+3,x2_2-3,y2_2-3),lineColor, pDC);

		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();
		br1.DeleteObject();
	}

	pen.DeleteObject();
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyChannel::OnLButtonUp(CPoint point)
{
#ifdef _CONSOLE_DEBUG
	printf("\n\nChannel::OnLButUP");
#endif
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	
	if(drawing) return;
	
	// Get current location based on scale
	bool clicked;
	if(/*!clicked && */!selected) return;

	 /*if((newRect.top == 0 && newRect.left == 0) || (newRect_2.top == 0 && newRect_2.left == 0)) {
		 
  		Reset();
		if(!clicked) selected = false;		
		ownerPanel->Invalidate();
		pCtrl->UnSelectAll();
		pCtrl->changed = true;
		if(clicked) selected = true;
		pCtrl->RePaint();		
		pCtrl->SaveUserStudies();
		 return;
	 }*/
	//Temporaries
	Coordinates coord;
	CPointF p_line1;
	CPointF p_line2;
	double dx, dy;
	dy = newRect.bottom - newRect.top; //y2 - y1;
	dx = newRect.right-newRect.left; //x2 - x1;
	theta = atan(dy/dx);

	double L = sqrt(pow(dx,2)+pow(dy,2));
	distance = sqrt(pow(newRect_2.top-newRect.top,2)+pow(newRect_2.left-newRect.left,2));
	/*if(abs(distance)<20.0F)
	{
		if(distance<0)distance = -20.0F;
		else distance = 20.0F;
	}
	
	newRect_2.left = newRect.left+distance*(-dy)/L;
	newRect_2.right = newRect.right+distance*(-dy)/L;
	newRect_2.top = newRect.top+distance*dx/L;
	newRect_2.bottom = newRect.bottom+distance*dx/L;*/

		
#ifdef _CONSOLE_DEBUG
	printf("\n\nChannel::OnLButUP:\n\tx1=%f x2=%f y1=%f y2=%f x1_2=%f x2_2=%f y1_2=%f y2_2=%f",newRect.left,newRect.right,newRect.top,newRect.bottom,newRect_2.left,newRect_2.right,newRect_2.top,newRect_2.bottom);
#endif

	if(newRect.top>newRect_2.top /*y1>y1_2 && state==3*/) { // Make R1 always on top
#ifdef _CONSOLE_DEBUG
	printf("\n\tSwitch R1 on top!");
#endif

		double temp;
		temp = newRect.left;
		newRect.left = newRect_2.left;
		newRect_2.left = temp;

		temp = newRect.right;
		newRect.right = newRect_2.right;
		newRect_2.right = temp;

		temp = newRect.top;
		newRect.top = newRect_2.top;
		newRect_2.top = temp;

		temp = newRect.bottom;
		newRect.bottom = newRect_2.bottom;
		newRect_2.bottom = temp;
	}
	
	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
		//pCtrl->SaveUserStudies();
	}
	clicked = IsClicked(newRect.left,newRect.top,newRect.right,newRect.bottom) || IsClicked(newRect_2.left,newRect_2.top,newRect_2.right,newRect_2.bottom);
	if(selected )
	{
#ifdef _CONSOLE_DEBUG
	printf("\n\tUpdate x, x_2, y e y_2!");
#endif
		//if(moveCtr){
			x1 = newRect.left;
			x2 = newRect.right;
			y1 = newRect.top;
			y2 = newRect.bottom;

			x1_2 = newRect_2.left;
			x2_2 = newRect_2.right;
			y1_2 = newRect_2.top;
			y2_2 = newRect_2.bottom;
			//Re-calc in moveX1/2
			if(moveX1||moveX2){
				Reset();
				Update();
				dy = y2 - y1;
				dx = x2 - x1;
				L = sqrt(pow(dx,2)+pow(dy,2));
				//distance = sqrt(pow(y2_2-y2,2)+pow(x2_2-x2,2));
	
				x1_2 = x1+distance*(-dy)/L;
				x2_2 = x2+distance*(-dy)/L;
				y1_2 = y1+distance*dx/L;
				y2_2 = y2+distance*dx/L;
			}
		//}

		
		pCtrl->SaveUserStudies();

		pCtrl->m_mouseState = MOUSE_NORMAL;	
		pCtrl->movingObject = false;
		moveX1 = false;
		moveX2 = false;
		moveCtr = false;
		moveDistX1 = false;
		moveDistX2 = false;
		/*newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
		oldRect = newRect;
		newRect_2 = CRectF(0.0f,0.0f,0.0f,0.0f);
		oldRect_2 = newRect_2;*/
  		Reset();
		if(!clicked) selected = false;		
		ownerPanel->Invalidate();
		pCtrl->UnSelectAll();
		pCtrl->changed = true;
		if(clicked) selected = true;
		pCtrl->UpdateScreen(false);		
		pCtrl->SaveUserStudies();
				
	}

	
	//oldRect = CRectF(x1,y1,x2,y2);
	//newRect = CRectF(0,0,0,0);
	//oldRect_2 = CRectF(x1_2,y1_2,x2_2,y2_2);
	//newRect_2 = CRectF(0,0,0,0);				
	moveX1 = false;
	moveX2 = false;
	moveCtr = false;
	moveDistX1 = false;
	moveDistX2 = false;
	buttonState = MOUSE_UP;
	pCtrl->dragging = false;
	pCtrl->movingObject = false;
#ifdef _CONSOLE_DEBUG
	printf("\n\tSaveUserStudies() y1=%f y1_2=%f",y1,y1_2);
#endif
}

void CLineStudyChannel::OnMouseMove(CPoint point)
{
	CPointF pointF = CPointF((double)point.x,(double)point.y);

	if(!drawn && drawing && startX > 0 && startY > 0){ // XOR for line drawing
		XORDraw(2, point);
		return;
	}

	//if(!pCtrl->m_Cursor == 0) return;	

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);
	double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c=y2;
	double x1c_2=x1_2;
	double x2c_2=x2_2;
	double y1c_2=y1_2;
	double y2c_2=y2_2;
	if(isRightExtension()&&rightPoint.x!=NULL_VALUE){
		x2c=rightPoint.x;
		y2c=rightPoint.y;
		x2c_2=rightPoint_2.x;
		y2c_2=rightPoint_2.y;
	}
	bool clicked = IsClicked(x1c,y1c,x2c,y2c) || IsClicked(x1c_2,y1c_2,x2c_2,y2c_2);	

	if(clicked) pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key);

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
	
	if((moveX1 || moveX2 || moveCtr || moveDistX1 || moveDistX2) && buttonState == MOUSE_DOWN && selected){

		CDC* pDC = pCtrl->GetScreen();
		
		pDC->SetROP2(R2_NOT);

     	//Remove study
		if((newRect.bottom != 0 && newRect.right != 0) || (newRect_2.bottom != 0 && newRect_2.right != 0))	
		{
			//pDC->SetROP2(R2_COPYPEN);
			if(!(moveDistX1 || moveDistX2))DrawLineStudy(pDC, oldRect);
			//pDC->SetROP2(R2_NOT);
			DrawLineStudy(pDC, oldRect_2);
		}
#ifdef _CONSOLE_DEBUG
			printf("\nChannelMove()");
#endif
		//else if(moveDistX1 || moveDistX2)DrawLineStudy(pDC, oldRect);
		if(moveX1){
#ifdef _CONSOLE_DEBUG
			printf("\tmoveX1");
#endif
			newRect.left = pointF.x;
			newRect.right = x2;
			newRect.top = pointF.y;
			newRect.bottom = y2;

			
			//x1 = pointF.x;
			//y1 = pointF.y;

			//Restrain movements for channel
			if(newRect.left > x2-20){
				newRect.left = x2-20;
			}


			//New Theta
			double dx, dy;
			dy = newRect.bottom - newRect.top; //y2 - y1;
			dx = newRect.right-newRect.left; //x2 - x1;
			theta = atan(dy/dx);
			

			//Temporaries
			Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;
						
							
			p_line1 = coord.MovePolar(newRect.left, newRect.top, distance, coord.PI/2 + theta);
			p_line2 = coord.MovePolar(newRect.right, newRect.bottom, distance, coord.PI/2 + theta);
		

			// Change New Rect to make line move only in parallel with first one
			/*newRect_2.left = p_line1.x;
			newRect_2.right = p_line2.x;
			newRect_2.top = p_line1.y;
			newRect_2.bottom = p_line2.y;*/

			double L = sqrt(pow(dx,2)+pow(dy,2));
			distance = sqrt(pow(y1_2-y1,2)+pow(x1_2-x1,2));
			//distance = y1_2-y1;

			newRect_2.left = newRect.left+distance*(-dy)/L;
			newRect_2.right = newRect.right+distance*(-dy)/L;
			newRect_2.top = newRect.top+distance*dx/L;
			newRect_2.bottom = newRect.bottom+distance*dx/L;

		}
		else if(moveX2){
			
#ifdef _CONSOLE_DEBUG
			printf("\tmoveX2");
#endif
			newRect.left = x1;
			newRect.right = pointF.x;
			newRect.top = y1;
			newRect.bottom = pointF.y;
			//x2 = pointF.x;
			//y2 = pointF.y;

			if(newRect.right < x1+15){
				newRect.right = x1 + 20;
			}

			//New Theta
			double dy, dx;
			dy = newRect.bottom-newRect.top; //y2 - y1;
			dx =  newRect.right-newRect.left; //x2 - x1;
			theta = atan(dy/dx);

			//Temporaries
			Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;
						
			p_line1 = coord.MovePolar(newRect.left, newRect.top, distance, coord.PI/2 + theta);
			p_line2 = coord.MovePolar(newRect.right, newRect.bottom, distance, coord.PI/2 + theta);
			

			// Change New Rect to make line move only in parallel with first one
			/*newRect_2.left = p_line1.x;
			newRect_2.right = p_line2.x;
			newRect_2.top = p_line1.y;
			newRect_2.bottom = p_line2.y;*/

			double L = sqrt(pow(dx,2)+pow(dy,2));
			//distance = y2_2-y2;
			distance = sqrt(pow(y2_2-y2,2)+pow(x2_2-x2,2));

			newRect_2.left = newRect.left+distance*(-dy)/L;
			newRect_2.right = newRect.right+distance*(-dy)/L;
			newRect_2.top = newRect.top+distance*dx/L;
			newRect_2.bottom = newRect.bottom+distance*dx/L;

		} else if(moveCtr){// Just move the entire line (don't resize it)
#ifdef _CONSOLE_DEBUG
			printf("\tmoveCtr");
#endif
			newRect.left = x1 - (startX - pointF.x);
			newRect.right = x2 - (startX - pointF.x);
			newRect.top = y1 - (startY - pointF.y);
			newRect.bottom = y2 - (startY - pointF.y);
			newRect_2.left = x1_2 - (startX - pointF.x);
			newRect_2.right = x2_2 - (startX - pointF.x);
			newRect_2.top = y1_2 - (startY - pointF.y);
			newRect_2.bottom = y2_2 - (startY - pointF.y);

		}
		else // moveDistX1 || moveDistX2
		{
			
#ifdef _CONSOLE_DEBUG
			printf("\tmoveDistX");
#endif
			newRect.left = x1;
			newRect.right = x2;
			newRect.top = y1;
			newRect.bottom = y2;

			/* 

			Y = mX+q
			q = (y2-y1*x2/x1)/(1-x2/x1);
			m = (y1-q)/x1;

			Temos duas retas com as equações: 
			y = a1 * x + b1  
			y = a2 * x + b2 
 
			Como queremos verificar a intersecção, fazemos y(reta1) = y(reta2) onde teremos: 
			m1 * x + q1 = m2 * x + q2  
			Isolando o valor de x temos: 
 
			x = ( q2 - q1 ) / (m1 - m2) 
 
			Se a1 == a2 teremos um erro (divisão por zero), o que indica que as retas são paralelas (não têm intersecção). 
 
			Utilizaremos o valor de x em alguma das equações acima: 
			y = a1 * x + b1 
*/ 

			//Temporaries
			Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;	
			CPointF p_line1i;
			CPointF p_line2i;		
			CPointF p_intersect;	
			double dx = x2-x1;
			double dy = y2-y1;
			double L = sqrt(pow((x1-x2),2)+pow((y1-y2),2));
			double m,q,qi,mi; 
			
			distance = sqrt(pow((x1_2-x2_2),2)+pow((y1_2-y2_2),2));
			L = sqrt(pow((dx),2)+pow((dy),2));
				
			//Calculate parallel line on cursor:
			p_line1 = coord.MovePolar(pointF.x,pointF.y,L,theta-coord.PI);
			p_line2 = coord.MovePolar(pointF.x,pointF.y,distance-L,theta);

				
			//Calculate perpendicular line to line_1 (x1):
			distance = sqrt(pow((pointF.x-x2_2),2)+pow((pointF.y-y2_2),2));
			p_line1i = coord.MovePolar(x1,y1,2*distance,theta+coord.PI/2);
			p_line2i = coord.MovePolar(x1,y1,2*distance,theta-coord.PI/2);
				
			//Calculate perpendicular function:
			q = (p_line2i.y-p_line1i.y*p_line2i.x/p_line1i.x)/(1-p_line2i.x/p_line1i.x);
			m = (p_line1i.y-q)/p_line1i.x;
				
			//Calculate new line_2 function:
			qi = (p_line2.y-p_line1.y*p_line2.x/p_line1.x)/(1-p_line2.x/p_line1.x);
			mi = (p_line1.y-qi)/p_line1.x;
				
			//Calculate intersection between new line_2 and perpendicular line:
			double xi,yi;
			xi = (qi-q)/(m-mi);
			yi = m*xi+q;
				
			distance = sqrt(pow((xi-x1),2)+pow((yi-y1),2));

			int pos = -1;
			if(yi>y1) pos = 1;
			//Finally plot line_2 parallel to line_1:
			p_line1.x = newRect.left+distance*(-pos)*(dy)/L;
			p_line2.x = newRect.right+distance*(-pos)*(dy)/L;
			p_line1.y = newRect.top+distance*(pos)*(dx)/L;
			p_line2.y = newRect.bottom+distance*(pos)*(dx)/L;
			
						
			// Change New Rect to make line move only in parallel with first one
			newRect_2.left = p_line1.x;
			newRect_2.right = p_line2.x;
			newRect_2.top = p_line1.y;
			newRect_2.bottom = p_line2.y;
		}
		
		/*else if(moveDistX1){
			
#ifdef _CONSOLE_DEBUG
			printf("\tmoveDistX1");
#endif
			//Temporaries
			Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;
			
			//p_line1 = coord.MovePolar(x1, y1, distance, coord.PI/2 + theta);
			//p_line2 = coord.MovePolar(x2, y2, distance, coord.PI/2 + theta);

			//newRect_2.left = p_line1.x;
			//newRect_2.right = p_line2.x;
			//newRect_2.top = p_line1.y;
			//newRect_2.bottom = p_line2.y;

			
			// Change New Rect to make line move only in parallel with first one

			newRect.left = x1;
			newRect.right = x2;
			newRect.top = y1;
			newRect.bottom = y2;

			double L = sqrt(pow((x1-x2),2)+pow((y1-y2),2));
			distance = pointF.y-y1;

			newRect_2.left = newRect.left+distance*(y1-y2)/L;
			newRect_2.right = newRect.right+distance*(y1-y2)/L;
			newRect_2.top = newRect.top+distance*(x2-x1)/L;
			newRect_2.bottom = newRect.bottom+distance*(x2-x1)/L;

									
		} else if(moveDistX2){
							
			
#ifdef _CONSOLE_DEBUG
			printf("\tmoveDistX2");
#endif
			//Temporaries
			Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;
										
			distance = abs(pointF.y - y2)/cos(theta);			
			if(pointF.y > y2){
				p_line1 = coord.MovePolar(x1, y1, distance, coord.PI/2 + theta);
				p_line2 = coord.MovePolar(x2, y2, distance, coord.PI/2 + theta);
			} else{
				p_line1 = coord.MovePolar(x1, y1, distance, theta - coord.PI/2);
				p_line2 = coord.MovePolar(x2, y2, distance, theta - coord.PI/2);
			}
			newRect.left = x1;
			newRect.right = x2;
			newRect.top = y1;
			newRect.bottom = y2;
			// Change New Rect to make line move only in parallel with first one
			newRect_2.left = p_line1.x;
			newRect_2.right = p_line2.x;
			newRect_2.top = p_line1.y;
			newRect_2.bottom = p_line2.y;
									
		}*/



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
		
		
		//Draw Study
		//if(!moveDistX1&&!moveDistX2)DrawLineStudy(pDC, newRect);
		if(!(moveDistX1 || moveDistX2))DrawLineStudy(pDC, newRect);
		DrawLineStudy(pDC, newRect_2);
		
		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;
		oldRect_2 = newRect_2;

	}

}

void CLineStudyChannel::OnLButtonDown(CPoint point)
{
	if(drawing || moveX1 || moveX2 || moveCtr || moveDistX1 || moveDistX2  ) {
		return;
	}

	CPointF pointF = CPointF((double)point.x,(double)point.y);
	
	//Temporaries
	Coordinates coord;
	CPointF p_line1;
	CPointF p_line2;
	double dx, dy;
	dy = y2 - y1;
	dx = x2 - x1;
	theta = atan(dy/dx);
	//double L = sqrt(pow(dx,2)+pow(dy,2));
	//distance = y1_2-y1;

				
	//distance = abs(y1_2 - y1)/cos(theta);

	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);

	m_valueView.Reset(true);double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c=y2;
	double x1c_2=x1_2;
	double x2c_2=x2_2;
	double y1c_2=y1_2;
	double y2c_2=y2_2;
	if(isRightExtension()&&rightPoint.x!=NULL_VALUE){
		x2c=rightPoint.x;
		y2c=rightPoint.y;
		x2c_2=rightPoint_2.x;
		y2c_2=rightPoint_2.y;
	}
	if((IsClicked(x1c,y1c,x2c,y2c) || IsClicked(x1c_2,y1c_2,x2c_2,y2c_2)) && selectable){
		pCtrl->UnSelectAll();
		if(pCtrl->SelectCount() == 0 || selected){	
			oldRect = CRectF(x1,y1,x2,y2);
			newRect = oldRect;
			oldRect_2 = CRectF(x1_2,y1_2,x2_2,y2_2);
			newRect_2 = oldRect_2;					
			pCtrl->RePaint();		
			pCtrl->movingObject = true;	
			if(buttonState != MOUSE_DOWN){
				startX = pointF.x;
				startY = pointF.y;
			}
			// Can we move this line?
			if(pointF.x > x1-10 && pointF.x < x1+10 && pointF.y > y1-10 && pointF.y < y1+10){
				moveX1 = true;	
				// Don't move, just change the angle, point X1 is base
			}
			else if(pointF.x > x2-10 && pointF.y > y2-10 && pointF.x < x2+10 && pointF.y < y2+10){
				moveX2 = true;		
				// Don't move, just change the angle, Point X2 is base
			} 
			else if(pointF.x > x1_2-10 && pointF.y > y1_2-10 && pointF.x < x1_2+10 && pointF.y < y1_2+10){
				moveDistX1 = true;
				// Change the distance between lines
			}
			else if(pointF.x > x2_2-10 && pointF.y > y2_2-10 && pointF.x < x2_2+10 && pointF.y < y2_2+10){
				moveDistX2 = true;		
				// Change the distance between lines
			}
			else{
				if(IsClicked(x1c,y1c,x2c,y2c) || IsClicked(x1c_2,y1c_2,x2c_2,y2c_2)){
					moveCtr = true; 		
					// Move the entire object
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

void CLineStudyChannel::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}
	double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c=y2;
	double x1c_2=x1_2;
	double x2c_2=x2_2;
	double y1c_2=y1_2;
	double y2c_2=y2_2;
	if(isRightExtension()&&rightPoint.x!=NULL_VALUE){
		x2c=rightPoint.x;
		y2c=rightPoint.y;
		x2c_2=rightPoint_2.x;
		y2c_2=rightPoint_2.y;
	}

	switch(MsgID){
		case MSG_POINTOUT:
		    if( !selected && (IsClicked(x1c,y1c,x2c,y2c)||IsClicked(x1c_2,y1c_2,x2c_2,y2c_2)) && !pCtrl->movingObject) {
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

void CLineStudyChannel::DisplayInfo()
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

void CLineStudyChannel::XORDraw(UINT nFlags, CPoint point)
{		
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);


	if(nFlags == 1){ // First point
		startX = pointF.x;
		startY = pointF.y;
		drawing = true;		
		state = 1;

		
	}
	else if(nFlags == 2){ // Drawing
		
		CDC* pDC = pCtrl->GetScreen();
		int next = 0;

		switch(state){
			case 1: //Drawing First Line
				
				pDC->SetROP2(R2_NOT);
				DrawLineStudy(pDC, oldRect);		
				newRect.left = startX;
				newRect.right = pointF.x;
				newRect.top = startY;
				newRect.bottom = pointF.y;
				
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
	
				DrawLineStudy(pDC, newRect);

				pDC->SetROP2(R2_COPYPEN);
				oldRect = newRect;
				pCtrl->ReleaseScreen(pDC);
			break;	

			case 2: //Drawing Second Line
				//DrawLineStudy(pDC, newRect);
				pDC->SetROP2(R2_NOT);
				DrawLineStudy(pDC, oldRect_2);		
				
				//Temporaries
				Coordinates coord;
				double dy, dx;
				dy = y2 - y1;
				dx = x2 - x1;
				theta = atan(dy/dx);
				CPointF p_line1;
				CPointF p_line2;
				CPointF p_line1i; 
				CPointF p_line2i;
				double q,m,qi,mi;
				
				distance = sqrt(pow((x1-x2),2)+pow((y1-y2),2));
				double L = sqrt(pow((dx),2)+pow((dy),2));
				
				//Calculate parallel line on cursor:
				p_line1 = coord.MovePolar(pointF.x,pointF.y,L,theta-coord.PI);
				p_line2 = coord.MovePolar(pointF.x,pointF.y,distance-L,theta);

				/*p_line1.x = x1+distance*(-dy)/L;
				p_line2.x = x2+distance*(-dy)/L;
				p_line1.y = y1+distance*dx/L;
				p_line2.y = y2+distance*dx/L;*/
				
				//Calculate perpendicular line to line_1 (x1):
				distance = sqrt(pow((pointF.x-x2_2),2)+pow((pointF.y-y2_2),2));
				p_line1i = coord.MovePolar(x1,y1,2*distance,theta+coord.PI/2);
				p_line2i = coord.MovePolar(x1,y1,2*distance,theta-coord.PI/2);
				
				//Calculate perpendicular function:
				q = (p_line2i.y-p_line1i.y*p_line2i.x/p_line1i.x)/(1-p_line2i.x/p_line1i.x);
				m = (p_line1i.y-q)/p_line1i.x;
				
				//Calculate new line_2 function:
				qi = (p_line2.y-p_line1.y*p_line2.x/p_line1.x)/(1-p_line2.x/p_line1.x);
				mi = (p_line1.y-qi)/p_line1.x;
				
				//Calculate intersection between new line_2 and perpendicular line:
				double xi,yi;
				xi = (qi-q)/(m-mi);
				yi = m*xi+q;
				
				distance = sqrt(pow((xi-x1),2)+pow((yi-y1),2));

				int pos = -1;
				if(yi>y1) pos = 1;
				//Finally plot line_2 parallel to line_1:
				p_line1.x = newRect.left+distance*(-pos)*(dy)/L;
				p_line2.x = newRect.right+distance*(-pos)*(dy)/L;
				p_line1.y = newRect.top+distance*(pos)*(dx)/L;
				p_line2.y = newRect.bottom+distance*(pos)*(dx)/L;
				
				//p_line1 = p_line1i; //SHOWING PERPENDICULAR LINE FOR TESTS: OK
				//p_line2 = p_line2i; //SHOWING PERPENDICULAR LINE FOR TESTS: OK
				
				//p_line1 = CPointF(xi,yi); //SHOWING INTERSECTION POINT FOR TESTS
				//p_line2 = CPointF(xi+3,yi); //SHOWING INTERSECTION POINT FOR TESTS

				// Change New Rect to make line move only in parallel with first one
				newRect_2.left = p_line1.x;
				newRect_2.right = p_line2.x;
				newRect_2.top = p_line1.y;
				newRect_2.bottom = p_line2.y;

				//Save inclination angle
				
							
				/*
				if(pointF.y > y2){
					p_line1 = coord.MovePolar(x1, y1, distance, coord.PI/2 + theta);
					p_line2 = coord.MovePolar(x2, y2, distance, coord.PI/2 + theta);
				} else{
					p_line1 = coord.MovePolar(x1, y1, distance, theta - coord.PI/2);
					p_line2 = coord.MovePolar(x2, y2, distance, theta - coord.PI/2);
				}

				// Change New Rect to make line move only in parallel with first one
				
				double L = sqrt(pow(dx,2)+pow(dy,2));
				distance = pointF.y-y2;

				newRect_2.left = x1+distance*(-dy)/L;
				newRect_2.right = x2+distance*(-dy)/L;
				newRect_2.top = y1+distance*dx/L;
				newRect_2.bottom = y2+distance*dx/L;
				*/


				x1_2 = newRect_2.left;
				x2_2 = newRect_2.right;
				y1_2 = newRect_2.top;
				y2_2 = newRect_2.bottom;
				
				// Invalidate bad movements			
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
	
				/*if(y1_2>=y1-10 && y1_2 <= y1+10)
				{
					if(y1_2>y1)
					{
						y1_2 += 10;
						y2_2 += 10;
						newRect_2.top += 10;
						newRect_2.bottom += 10;
					}
					else
					{
						y1_2 -= 10;
						y2_2 -= 10;
						newRect_2.top -= 10;
						newRect_2.bottom -= 10;
					}
				}*/

				DrawLineStudy(pDC, newRect_2);

				pDC->SetROP2(R2_COPYPEN);
				oldRect_2 = newRect_2;
				pCtrl->ReleaseScreen(pDC);
			break;
					
		}
		
	}
	else if(nFlags == 3)	//	Last point
	{
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

		if(state == 1){
			y1	= startY;
			y2	= pointF.y;
			x1	= startX;
			x2	= pointF.x;

			if(x1>x2) //Flipped!
			{
				double tempX = x1;
				double tempY = y1;
				x1 = x2;
				y1 = y2;
				x2 = tempX;
				y2 = tempY;
			}
					
			// Return and wait for second line
			state = 2;
			return;
		}

		if(state == 2){
			
			//Save point map

			//Temporaries
			/*Coordinates coord;
			CPointF p_line1;
			CPointF p_line2;
									
			//distance = abs(pointF.y - y2)/cos(theta);			
			if(pointF.y > y2){
				p_line1 = coord.MovePolar(x1, y1, distance, coord.PI/2 + theta);
				p_line2 = coord.MovePolar(x2, y2, distance, coord.PI/2 + theta);
			}else{
				p_line1 = coord.MovePolar(x1, y1, distance, theta - coord.PI/2);
				p_line2 = coord.MovePolar(x2, y2, distance, theta - coord.PI/2);
			}

			double L = sqrt(pow((x2-x1),2)+pow((y2-y1),2));
			distance = pointF.y-y2;

			x1_2 = x1+distance*(y1-y2)/L;
			x2_2 = x2+distance*(y1-y2)/L;
			y1_2 = y1+distance*(x2-x1)/L;
			y2_2 = y2+distance*(x2-x1)/L;*/
				

			state = 3;
			
			if(y1>y1_2) { // Make R1 always on top
#ifdef _CONSOLE_DEBUG
			printf("\n\tSwitch R1 on top!");
#endif

				double temp;
				temp = x1;
				x1 = x1_2;
				x1_2 = temp;

				temp = y1;
				y1 = y1_2;
				y1_2 = temp;

				temp = x2;
				x2 = x2_2;
				x2_2 = temp;

				temp = y2;
				y2 = y2_2;
				y2_2 = temp;

				newRect.left = x1;
				newRect.top = y1;
				newRect.right = x2;
				newRect.bottom = y2;
				
				newRect_2.left = x1_2;
				newRect_2.top = y1_2;
				newRect_2.right = x2_2;
				newRect_2.bottom = y2_2;

			}
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
		//drawn	= false;
		CDC* pDC = pCtrl->GetScreen();
		this->OnPaint( pDC );
		pCtrl->ReleaseScreen( pDC ); // Release added 9/29/05
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/

		pCtrl->OnUserDrawingComplete(lsChannel, key);
		pCtrl->SaveUserStudies();
		
	}
}


/////////////////////////////////////////////////////////////////////////////

void CLineStudyChannel::OnRButtonUp(CPoint point)
{		
	if(selected) pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);
	
}
/////////////////////////////////////////////////////////////////////////////

void CLineStudyChannel::OnDoubleClick(CPoint point)
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

// Create a CLineStudyChannel,
// set it's x1,y1,x2,y2 and call this.
void CLineStudyChannel::SnapLine()
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