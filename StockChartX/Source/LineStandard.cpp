// LineStandard.cpp: implementation of the CLineStandard class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "LineStandard.h"
#include "Coordinates.h"
//#include "DebugConsole.h"


//#define _CONSOLE_DEBUG 

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//includes for Direct 2D
// Windows Header Files:
//#include <windows.h>

// C RunTime Header Files:
//#include <stdlib.h>
//#include <malloc.h>
//#include <memory.h>
//#include <wchar.h>
//#include <math.h>

#include <d2d1.h>
#include <d2d1helper.h>
#pragma comment(lib, "d2d1")
//#include <dwrite.h>
//#include <wincodec.h>


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CLineStandard::CLineStandard(OLE_COLOR color, LPCTSTR Key, CChartPanel* owner, bool radiusExtensionParameter /*= false*/)
{
	objectType = "TrendLine";
	nType = lsTrendLine;
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
	//Testing left extension
	setLeftExtension(false);

	//Testing right extension
	setRightExtension(false);

	//Testing radius extension
	setRadiusExtension(radiusExtensionParameter);
	
	//valuePosition = -1;

	buttonState = 0;
	startX = 0.0f;
	startY = 0.0f;
	key = Key;
	x1 = y1 = x2 = y2 = 0.0f;
	fixedLine = oldRect = newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
	drawNumber = 0;
	
	lineWeight = pCtrl->lineThickness;	

}

CLineStandard::~CLineStandard()
{

}


//Gets and sets
bool CLineStandard::isLeftExtension(){
	return this->leftExtension;
}
bool CLineStandard::isRightExtension(){
	return this->rightExtension;
}
bool CLineStandard::isRadiusExtension(){
	return this->radiusExtension;
}
void CLineStandard::setLeftExtension(bool left){
	this->leftExtension = left;
}
void CLineStandard::setRightExtension(bool right){
	this->rightExtension = right;
}
void CLineStandard::setRadiusExtension(bool radius){
	this->radiusExtension = radius;
}


void CLineStandard::DrawLineStudy(CDC* pDC, CRectF rect){


	if((rect.top == 0 && !vFixed)|| (rect.left == 0 && !hFixed)) {
#ifdef _CONSOLE_DEBUG
		printf("\nDrawLine() RETURN!");
#endif
		return;
	}
	//if(y1Value != NULL_VALUE) ExcludeRects(pDC);
	double xt1, xt2, yt1, yt2;
	double b = 0;
	double c = 0;
	double x_proj=0;
	double y_proj=0;

	//Getting values
	if(flipped){
		xt1 = rect.right;	
		xt2 = rect.left;
		yt1 = rect.bottom;
		yt2 = rect.top;
	}else {
		xt1 = rect.left;	
		xt2 = rect.right;
		yt1 = rect.top;
		yt2 = rect.bottom;
	}
	
	//if(leftExtension) xt1 = ownerPanel->panelRect.left;
	

	//Drawing Vertical Lines
	if(/*y1Value == NULL_VALUE ||*/ vFixed)
	{
		if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo(xt1,yt2);
		}
		else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),CPointF(xt1,yt2),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
	
	}
	//Drawing Horizontal Lines
	else if (/*x2Value == NULL_VALUE ||*/ hFixed)
	{		
		
		if(moveX1 || moveX2 || moveCtr || !drawn){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo(xt2,yt1);
		}
		else {
			if(valuePosition != -1 && valuePosition!=0 && valuePosition != ownerPanel->GetReverseY(yt1)) {

				yt1 = yt2 = ownerPanel->GetY(valuePosition);
#ifdef _CONSOLE_DEBUG
				printf("\n\tUpdate valueP=%f yt=%f", valuePosition, yt1);
#endif
				rect.top = rect.bottom = y1 = y2 = fixedLine.top = fixedLine.bottom = yt1;
				
				y1Value = y2Value = valuePosition;
				Update();
				//Reset();			
			}
			else if (valuePosition == -1){
				valuePosition = ownerPanel->GetReverseY(yt1);
#ifdef _CONSOLE_DEBUG
				printf("\n\Set valueP=%f yt=%f", valuePosition, yt1);
#endif
			}
			pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),CPointF(xt2,yt1),lineWeight+pointOutStep,lineStyle,lineColor, pDC);
		}
		if(!moveX1 && !moveX2 && !moveCtr){ //Draw the label
#ifdef _CONSOLE_DEBUG
			printf("\tDrawLabel=%f",y1Value);
#endif
			bool textFlipped = false;
			CFont newFont;
			newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial Rounded MT"), pDC);
			CFont* pOldFont = pDC->SelectObject(&newFont);
			OLE_COLOR oldColor = pDC->SetTextColor(lineColor);
			CString strDisplay;
			CRect rectText = CRect((int)rect.left,(int)rect.top,(int)rect.right,(int)rect.bottom);
			CRectF rectTextF = CRectF(rect.left,rect.top,rect.right,rect.bottom);			
			if(leftExtension || rectText.left < ownerPanel->panelRect.left) {
				rectText.left = ownerPanel->panelRect.left;
				rectTextF.left = (double)ownerPanel->panelRect.left;
			}
			
			CSeriesStock *seriesStock = NULL;
			CSeries *seriesSt = NULL;
			seriesSt = ownerPanel->GetSeries(pCtrl->m_symbol + ".low");
			if(seriesSt!=NULL){
				if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
					seriesStock = (CSeriesStock *) seriesSt;
					Candle c;
					if(leftExtension) c = seriesStock->GetCandle(pCtrl->startIndex);
					else c = seriesStock->GetCandle(x1Value-1);
					if((ownerPanel->GetY(y1Value)<ownerPanel->GetY(c.GetMin())+15.0F)&&(ownerPanel->GetY(y1Value)>ownerPanel->GetY((c.GetOpen()+c.GetClose())/2))){
						rectText.bottom += 15;
						rectTextF.bottom += 15.0F;
					}
					else {
						rectText.top -= 15;
						rectTextF.top -= 15.0F;
					}
				}
				else {
					rectText.top -= 15;
					rectTextF.top -= 15.0F;
				}
				strDisplay.Format("%.*f", pCtrl->decimals, y1Value);
				//pDC->DrawText(strDisplay, -1, &rectText, DT_WORDBREAK | DT_LEFT);
				pCtrl->pdcHandler->DrawText(strDisplay,rectTextF,"Arial Rounded MT",12,DT_LEFT,lineColor,255,pDC);
			}
			else{
				rectText.top -= 15.0f;
				strDisplay.Format("%.*f", 0, y1Value);
				//pDC->DrawText(strDisplay, -1, &rectText, DT_WORDBREAK | DT_LEFT);
				pCtrl->pdcHandler->DrawText(strDisplay,rectTextF,"Arial Rounded MT",12,DT_LEFT,lineColor,255, pDC);
			}
		}
		
	} else { //Line Standard
		
		if(rect.top==0.0f && rect.bottom==0.0f && rect.right==0.0f && rect.left==0.0f) {
#ifdef _CONSOLE_DEBUG
		printf("\nDrawLine() RETURN 0!");
#endif
			return;
		}

		if(moveX1 || moveX2 || moveCtr || !drawn ){
			pDC->MoveTo(xt1,yt1);
			pDC->LineTo(xt2,yt2);
		}
		else {
			pCtrl->pdcHandler->DrawLine(CPointF(xt1, yt1), CPointF(xt2, yt2), lineWeight + pointOutStep, lineStyle, lineColor, pDC);

		}
		//New Section
		double PI = 3.14159265;
		Coordinates coord; //Posterior removal of this class in this section -> Make this a global class
	
		CPen* pen = new CPen( PS_DOT, lineWeight+pointOutStep, lineColor );
		CPen* pOldPen = pDC->SelectObject( pen );

		//Radius extension	
		if(isRadiusExtension()){
			double xp1, xp2, yp1, yp2;
			xp1 = ownerPanel->GetX(0);
			xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
			yp1 = ownerPanel->y1;
			yp2 = ownerPanel->y2;
			double radiusSize = sqrt(pow(xp1 - xp2, 2) + pow(yp1 - yp2, 2));
			ExcludeRects(pDC);
					
			if(xt1 > xt2){
				double dx = (double) xt1 - xt2;
				double dy = (double) yt1 - yt2;
				rightPoint = coord.MovePolar(xt1,yt1, radiusSize, atan(dy/dx)- PI);
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt2,yt2);
					pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt2,yt2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);
				
			}else{
				double dx = (double) xt2 - xt1;
				double dy = (double) yt2 - yt1; 
				rightPoint = coord.MovePolar(xt2,yt2, radiusSize, atan(dy/dx)); 	
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt2,yt2);
					pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt2,yt2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);
			}
		
		}
		
		//Include Extension points
		if(isLeftExtension() && !hFixed){
			double xp1, xp2, yp1, yp2;
			xp1 = ownerPanel->GetX(0);
			xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
			yp1 = ownerPanel->y1;
			yp2 = ownerPanel->y2;		
			double extensionSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));
			//ExcludeRects(pDC);
				
			/*if(xt1 > xt2){
				double dx = (double) xt1 - xt2;
				double dy = (double) yt1 - yt2;
				leftPoint = coord.MovePolar(xt1,yt1, extensionSize, atan(dy/dx)- PI);
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt1,yt1);
					pDC->LineTo((int)leftPoint.x,(int)leftPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),leftPoint,lineWeight+pointOutStep,2,lineColor, pDC);
				
			}else{
				double dx = (double) xt1 - xt2;
				double dy = (double) yt1 - yt2; 
				leftPoint = coord.MovePolar(xt1,yt1, extensionSize, atan(dy/dx)); 	
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt1,yt1);
					pDC->LineTo((int)leftPoint.x,(int)leftPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),leftPoint,lineWeight+pointOutStep,2,lineColor, pDC);
			}*/
			
			double dx = (double) xt1 - xt2;
			double dy = (double) yt1 - yt2;
			leftPoint = coord.MovePolar(xt1,yt1, extensionSize, atan(dy/dx)- PI);
			if(moveX1 || moveX2 || moveCtr || !drawn){
				pDC->MoveTo(xt1,yt1);
				pDC->LineTo((int)leftPoint.x,(int)leftPoint.y);		
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(xt1,yt1),leftPoint,lineWeight+pointOutStep,2,lineColor, pDC);
		}
		if(isRightExtension()){
			double xp1, xp2, yp1, yp2;
			xp1 = ownerPanel->GetX(0);
			xp2 = ownerPanel->GetX(pCtrl->GetSlaveRecordCount2());
			yp1 = ownerPanel->y1;
			yp2 = ownerPanel->y2;		
			double extensionSize = sqrt(pow(xp1-xp2,2)+pow(yp1 - yp2,2));
			ExcludeRects(pDC);
			
					
			if(xt1 > xt2){
				double dx = (double) xt1 - xt2;
				double dy = (double) yt1 - yt2;
				rightPoint = coord.MovePolar(xt1,yt1, extensionSize, atan(dy/dx)- PI);
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt2,yt2);
					pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt2,yt2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);
				
			}else{
				double dx = (double) xt2 - xt1;
				double dy = (double) yt2 - yt1; 
				rightPoint = coord.MovePolar(xt2,yt2, extensionSize, atan(dy/dx)); 	
				if(moveX1 || moveX2 || moveCtr || !drawn){
					pDC->MoveTo(xt2,yt2);
					pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);
				}
				else pCtrl->pdcHandler->DrawLine(CPointF(xt2,yt2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);
			}/*
			double dx = (double) x2 - x1;
			double dy = (double) y2 - y1;
			rightPoint = coord.MovePolar(x2,y2, extensionSize, atan(dy/dx));
			if(moveX1 || moveX2 || moveCtr || !drawn){
				pDC->MoveTo(x2,y2);
				pDC->LineTo((int)rightPoint.x,(int)rightPoint.y);		
			}
			else pCtrl->pdcHandler->DrawLine(CPointF(x2,y2),rightPoint,lineWeight+pointOutStep,2,lineColor, pDC);*/
		}
		//Select oldPen
		pDC->SelectObject(pOldPen);
		IncludeRects(pDC);
	}
	//if(y1Value != NULL_VALUE) IncludeRects(pDC);
	

	
}

void CLineStandard::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	//printf("\nLineStand:::::::");
#endif
	//Dont draw with it's not visible!
	if(!hFixed && !vFixed && !isLeftExtension() && !isRightExtension() && !isRadiusExtension())if(((x1Value<pCtrl->startIndex && x2Value<pCtrl->startIndex)||(x1Value>pCtrl->endIndex && x2Value>pCtrl->endIndex))&&!(x2Value == NULL_VALUE))return;

     //Dont draw if it's moving
	if(drawing /*|| (selected && pCtrl->movingObject)*/ )
		return;

	if(!hFixed && !vFixed && !moveCtr && !moveX1 && !moveX2)Update();

#ifdef _CONSOLE_DEBUG
	//printf("::::::::::::::  OnPaint()");
#endif
	// Fix if line is too far off the right
	if (x1 > pCtrl->width - pCtrl->yScaleWidth && !hFixed)
	{
		return;
	}
	if (hFixed && x1_2 > pCtrl->width - pCtrl->yScaleWidth && !leftExtension)
	{
		return;
	}
	if(x2 > pCtrl->width - pCtrl->yScaleWidth)
	{		
		double b = double(y1 - y2) / double(x1 - x2);
		double c = y1 - b * x1;
		x2 = pCtrl->width - pCtrl->yScaleWidth;
		y2 = (b * x2 + c);
	}

	// Does not draw if x1 is not a part of the panel
#ifdef OLD_BEHAVIOR
	if(x1 > pCtrl->width - pCtrl->yScaleWidth) 
	{		
		return;
	}
#endif

	CPen pen( lineStyle, lineWeight+pointOutStep, lineColor );
	CPen* pOldPen = pDC->SelectObject( &pen );
	
	
	//Drawing Horizontal and Vertical Lines
	if(/*y1Value == NULL_VALUE ||*/ vFixed)
	{		
		//y1 = 0;
		//y2 = pCtrl->height;
		y1 = ownerPanel->y1;
		y2 = ownerPanel->y2;
		if (x1 != x2) {
		   x2 = x1;
	    }

		//Reset();
		vFixed = true;
		fixedLine.left = x1;
		fixedLine.right = x2;
		fixedLine.top = y1;
		fixedLine.bottom = y2;
		//pDC->MoveTo(x1,y1);
		//pDC->LineTo(x2,y2);
	}
	else if (/*x2Value == NULL_VALUE ||*/ hFixed)
	{
		//y1 = 0;
		//y2 = pCtrl->height;
		//if(leftExtension)x1 = ownerPanel->panelRect.left;
		//else x1 = ?;
		
		hFixed = true;
		if(leftExtension)x1=ownerPanel->panelRect.left;
		else x1 = x1_2;	
		x2 = ownerPanel->panelRect.right - pCtrl->yScaleWidth;
		y2 = y1;
		fixedLine.left = x1;
		fixedLine.right = x2;
		fixedLine.top = y1;
		fixedLine.bottom = y2;
		//Reset();
		//pDC->MoveTo(x1,y1);
		//pDC->LineTo(x2,y2);
	} 

	DrawLineStudy(pDC, CRectF(x1,y1,x2,y2));
	pDC->SelectObject(pOldPen);

	

	// If selected, paint end point selection boxes while dragging
	if(selected)
	{
		CBrush	br( lineColor );
		if (x1 < pCtrl->width - pCtrl->yScaleWidth){
			pCtrl->pdcHandler->FillRectangle(CRectF(x1 + 3, y1 + 3, x1 - 3, y1 - 3), lineColor, pDC);
		}
		if(x2 < pCtrl->width - pCtrl->yScaleWidth && !hFixed){
			pCtrl->pdcHandler->FillRectangle(CRectF(x2+3,y2+3,x2-3,y2-3),lineColor, pDC);
		}
		pCtrl->m_memDC.SelectObject( pOldPen );
		br.DeleteObject();
	}
	

	pen.DeleteObject();
}

void CLineStandard::InvalidateXOR()
{
	/*if(drawing || (selected && pCtrl->movingObject) )*/oldRect = CRectF(0.0f,0.0f,0.0f,0.0f);
}

void CLineStandard::OnPaintXOR(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	//printf("\nLineStand::OnPaintXOR()");
#endif
	//if((drawing || (selected && pCtrl->movingObject)) && oldRect.left == 0.0f && oldRect.top == 0.0f && newRect.left != 0.0f && newRect.top != 0.0f ) XORDraw(2, CPoint(newRect.right,newRect.bottom));
}
/////////////////////////////////////////////////////////////////////////////

void CLineStandard::OnLButtonUp(CPoint point)
{
#ifdef _CONSOLE_DEBUG
	printf("\nLineStandard::OnLButtonUp()");
#endif
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	if(drawing) return;

	if(selected) {
		pCtrl->FireOnItemLeftClick(OBJECT_LINE_STUDY, key);	
		pCtrl->SaveUserStudies();
	}
	drawn=true;

	if(bReset)
	{
		bReset = false;
	}

	moveX1 = false;
	moveX2 = false;
	moveCtr = false;

	// Get current location based on scale
	bool clicked = IsClicked(x1,y1,x2,y2);
	if(selected && oldRect.bottom != 0 && oldRect.right != 0)
	{
		x1 = newRect.left;
		if(!leftExtension)x1_2 = newRect.left;
		x2 = newRect.right;
		y1 = newRect.top;
		y2 = newRect.bottom;

		//Some adjustments has to be made in case we´re dealing with horizontal and vertical fixed lines
		if (vFixed) {
		  if (x1 == fixedLine.left || x1 == fixedLine.right) {
		    x1 = x2;	
		  }
		  else {
		    x2 = x1;
		  }
		}

		if (hFixed) {
		  y2 = y1;	
		  valuePosition = ownerPanel->GetReverseY(y1);
#ifdef _CONSOLE_DEBUG
		  printf("\nRESETED valuePosition OnLButtonUp() valueP=%f yt=%f", valuePosition, y1);
#endif
		  if (pCtrl->m_Magnetic && ownerPanel->index == 0){
#ifdef _CONSOLE_DEBUG
			  printf("\nLineStand::OnLButtonUp Magnetic");
#endif
			  y1Value = y2Value = MagneticPointYValue(y1, x1);
		  }
		}
		newRect = CRectF(0.0f,0.0f,0.0f,0.0f);
		oldRect = newRect;
  		Reset();



		if (selected){
			pCtrl->m_mouseState = MOUSE_NORMAL;
			pCtrl->movingObject = false;
			pCtrl->SaveUserStudies();
		}
		clicked = IsClicked(x1, y1, x2, y2);
		if (false/*!clicked*/) {
			ownerPanel->Invalidate();
			pCtrl->RePaint();
		}
		pCtrl->changed = true;
		if (clicked) {

			pCtrl->UnSelectAll();
			pCtrl->UpdateScreen(false);
			pCtrl->RePaint();
			selected = true;
		}
		else {
			selected = false;
#ifdef _CONSOLE_DEBUG
			printf("\nSELECTED FALSE 1");
#endif
		}


	}	
	

	buttonState = MOUSE_UP;
	pCtrl->dragging = false;
	pCtrl->movingObject = false;
	

}

void CLineStandard::OnMouseMove(CPoint point)
{

	if(point.x == 0 && point.y == 0)return;
#ifdef _CONSOLE_DEBUG
	//printf("OnMouseMove \n");
#endif
	
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	
	if(drawing && startX > 0 && startY > 0){ // XOR for line drawing
		XORDraw(2, point);
		return;
	}

	if(!pCtrl->m_Cursor == 0) return;	

	
	pointF.x = RealPointX((double)pointF.x);
	pointF.y = RealPointY((double)pointF.y);

	

	double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c=y2;

	if (hFixed){
		if (leftExtension)x1c = ownerPanel->panelRect.left;
		x2c = ownerPanel->panelRect.right - pCtrl->yScaleWidth;
	}
	else{
		if (isLeftExtension() && leftPoint.x != NULL_VALUE){
#ifdef _CONSOLE_DEBUG
			printf("\nMouseMOve(L) mx=%d x1=%f x2=%f", pCtrl->m_point.x, x1c, x2c);
#endif
			x1c = leftPoint.x;
			y1c = leftPoint.y;
		}
		if ((isRightExtension() || isRadiusExtension()) && rightPoint.x != NULL_VALUE){
#ifdef _CONSOLE_DEBUG
			printf("\nMouseMOve(R) mx=%d x1=%f x2=%f", pCtrl->m_point.x,x1c,x2c);
#endif
			x2c = rightPoint.x;
			y2c = rightPoint.y;
		}
	}
	bool clicked = IsClicked(x1c,y1c,x2c,y2c);	

	if (clicked) {
#ifdef _CONSOLE_DEBUG
		printf("\nMouseMOve(CLICK) mx=%d x1=%f x2=%f", pCtrl->m_point.x, x1c, x2c);
#endif
		pCtrl->FireOnItemMouseMove(OBJECT_LINE_STUDY, key);
	}

	if( !selected && clicked && !pCtrl->movingObject )
	{
#ifdef _CONSOLE_DEBUG
		printf("\nMouseMOve(DLAY) mx=%d x1=%f x2=%f", pCtrl->m_point.x, x1c, x2c);
#endif
		pCtrl->DelayMessage(guid, MSG_POINTOUT, 50);
		// MODIFICADO CORRECAO
		if (!selected && !pCtrl->movingObject) {
			pCtrl->RePaint();
		}
	}
	else
	{
		//Restore the line original state
		if (pointOutState) {
		  pointOutState = false;
		  pointOutStep = 0;
		  pCtrl->suspendCrossHair = true;
		  // MODIFICADO CORRECAO
		  if (!selected && !pCtrl->movingObject) {
			  pCtrl->RePaint();
		  }
		}
	}
	if((moveX1 || moveX2 || moveCtr) && buttonState == MOUSE_DOWN && selected){
		
		CDC* pDC = pCtrl->GetScreen();
		if(!pDC) {
#ifdef _CONSOLE_DEBUG
	printf("OnMouseMove() RETURN pDC==null \n");
#endif
			return;
		}
		pDC->SetROP2(R2_NOT);
		//Remove study
		DrawLineStudy(pDC, oldRect);
		/*ExcludeRects(pDC);
		pDC->MoveTo(oldRect.left,oldRect.top);
		pDC->LineTo(oldRect.right,oldRect.bottom);
		IncludeRects(pDC);*/
		

		// Flip coordinates
		if(moveX1){
			
			//Set Magnetic Y
			if (pCtrl->m_Magnetic && !vFixed && ownerPanel->index == 0) {
#ifdef _CONSOLE_DEBUG
				//printf("\nMoveX1 Magnetic y=%f", pointF.y);
#endif
				pointF.y = MagneticPointY(pointF.y, pointF.x);
			}
			if((int)pointF.x>ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex)))pointF.x=ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex))-1;
			if(hFixed){
				x1=pointF.x;
				if(!leftExtension)x1_2 = pointF.x;
				newRect.left = x1;
				newRect.right = x2;
				newRect.top = pointF.y;
				newRect.bottom = pointF.y;
			}
			else if(vFixed)
			{
					newRect.left = pointF.x;
					newRect.right = pointF.x;
					newRect.top = y1 ;
					newRect.bottom = y2;
					x1 = pointF.x;
					x2 = pointF.x;	
			}
			else{
				if(pointF.x <= x2){
					newRect.left = pointF.x;
					newRect.right = x2;
					newRect.top = pointF.y;
					newRect.bottom = y2;
					x1 = pointF.x;
					y1 = pointF.y;		
				}
				else {
					newRect.left = x2;
					newRect.right = pointF.x;
					newRect.top = y2;
					newRect.bottom = pointF.y;
					x1 = x2;
					x2 = pointF.x;
					y1 = y2;
					y2 = pointF.y;
					moveX1 = false;
					moveX2 = true;
					//flipped = !flipped;
				}
			}
		}
		else if(moveX2){
			
#ifdef _CONSOLE_DEBUG
	printf("\nMoveX2 m=%f y1=%f \n\ty2=%f rectTop=%f rectBot=%f",pointF.y,y1,y2,newRect.top,newRect.bottom);
#endif
			//Set Magnetic Y
			if(pCtrl->m_Magnetic && !vFixed && ownerPanel->index==0) {
				pointF.y = MagneticPointY(pointF.y, pointF.x);
			}
			if((int)pointF.x>ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex)))pointF.x=ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex))-1;
			if(vFixed)
			{
					newRect.left = pointF.x;
					newRect.right = pointF.x;
					newRect.top = y1 ;
					newRect.bottom = y2;
					x1 = pointF.x;
					x2 = pointF.x;
			}
			else{
				if(pointF.x >= x1){
					newRect.left = x1;
					newRect.right = pointF.x;
					newRect.top = y1;
					newRect.bottom = pointF.y;
					x2 = pointF.x;
					y2 = pointF.y;
				}
				else {
					newRect.left = pointF.x;
					newRect.right = x1;
					newRect.top = pointF.y;
					newRect.bottom = y1;
					x2 = x1;
					x1 = newRect.left;
					y2 = y1;
					y1 = pointF.y;
					moveX2 = false;
					moveX1 = true;
					//flipped = !flipped;
				}
			}
		}
		else if(moveCtr){// Just move the entire line (don't resize it)
#ifdef _CONSOLE_DEBUG
	printf("\nMoveCtr m=%f y1=%f \n\ty2=%f rectTop=%f rectBot=%f",pointF.y,y1,y2,newRect.top,newRect.bottom);
#endif
		if((int)(x2 - (startX - pointF.x))>ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex)))pointF.x-=(x2 - (startX - pointF.x))-ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex));
			// XLine can't move horizontaly
			if(hFixed){
				newRect.left = x1;
				newRect.right = x2;
				newRect.top = y1 - (startY - pointF.y);
				newRect.bottom = y2 - (startY - pointF.y);
			}
			else if(vFixed)
			{
				newRect.left = x1 - (startX - pointF.x);
				newRect.right = x2 - (startX - pointF.x);
				newRect.top = y1 ;
				newRect.bottom = y2;
				//x1 = pointF.x;
				//x2 = pointF.x;
			}
			else{
				newRect.left = x1 - (startX - pointF.x);
				newRect.right = x2 - (startX - pointF.x);
				newRect.top = y1 - (startY - pointF.y);
				newRect.bottom = y2 - (startY - pointF.y);
			}
		}
		/*
		//Save new coordinates by screen positions:
		x1 = newRect.left;
		if(!leftExtension)x1_2 = newRect.left;
		x2 = newRect.right;
		y1 = newRect.top;
		y2 = newRect.bottom;

		//Some adjustments has to be made in case we´re dealing with horizontal and vertical fixed lines
		if (vFixed) {
		  if (x1 == fixedLine.left || x1 == fixedLine.right) {
		    x1 = x2;	
		  }
		  else {
		    x2 = x1;
		  }
		}

		if (hFixed) {
		  y2 = y1;	
		  if(pCtrl->m_Magnetic && ownerPanel->index==0)y1Value=y2Value=MagneticPointYValue(y1,x1); 
		}
		*/


		//Save values by new coordinates:
		//Reset(); //ERROR 
		

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
		DrawLineStudy(pDC, newRect);
		/*ExcludeRects(pDC);
		pDC->MoveTo(newRect.left,newRect.top);
		pDC->LineTo(newRect.right,newRect.bottom);		

		IncludeRects(pDC);*/

		pDC->SetROP2(R2_COPYPEN);

		pCtrl->ReleaseScreen(pDC);
		oldRect = newRect;

	}
}

void CLineStandard::OnLButtonDown(CPoint point)
{
	if(drawing) return;

	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	pointF.y = RealPointY(pointF.y);


	m_valueView.Reset(true);
	double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c = y2;
	if (hFixed && leftExtension){
		x1c = ownerPanel->panelRect.left;
		x2c = ownerPanel->panelRect.right - pCtrl->yScaleWidth;
	}
	else{
		if (isLeftExtension() && leftPoint.x != NULL_VALUE){
			x1c = leftPoint.x;
			y1c = leftPoint.y;
		}
		if ((isRightExtension() || isRadiusExtension()) && rightPoint.x != NULL_VALUE){
			x2c = rightPoint.x;
			y2c = rightPoint.y;
		}
	}
	if(IsClicked(x1c,y1c,x2c,y2c) && selectable){
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
			// Can we move this line?
			if(pointF.x > x1-10 && pointF.y > y1-10 && pointF.x < x1+10 && pointF.y < y1+10){
				moveX1 = true;		
			}
			else if(pointF.x > x2-10 && pointF.y > y2-10 && pointF.x < x2+10 && pointF.y < y2+10){
				moveX2 = true;		
			}
			else{
				if(IsClicked(x1c,y1c,x2c,y2c)){
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
	OnMouseMove(point);
}

void CLineStandard::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
	if(MsgGuid != guid) {
		return;
	}
	double x1c=x1;
	double x2c=x2;
	double y1c=y1;
	double y2c=y2;

	switch(MsgID){
	case MSG_POINTOUT:
			if (hFixed && leftExtension){
				x1c = ownerPanel->panelRect.left;
				x2c = ownerPanel->panelRect.right - pCtrl->yScaleWidth;
			}
			else{
				if (isLeftExtension() && leftPoint.x != NULL_VALUE){
					x1c = leftPoint.x;
					y1c = leftPoint.y;
				}
				if ((isRightExtension() || isRadiusExtension()) && rightPoint.x != NULL_VALUE){
					x2c = rightPoint.x;
					y2c = rightPoint.y;
				}
			}
		    if( !selected && IsClicked(x1c,y1c,x2c,y2c) && !pCtrl->movingObject) { 
			  if (!pointOutState) {
				pointOutState = true;
				pointOutStep = 1;
				pCtrl->suspendCrossHair=true;
			    pCtrl->RePaint();
			  }
			}
			else {
			  if (pointOutState) {
			    pointOutState = false;
				pointOutStep = 0;
				pCtrl->suspendCrossHair=true;
                     // MODIFICADO CORRECAO
				if (!selected && !pCtrl->movingObject) {
			      pCtrl->RePaint();
				}
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

void CLineStandard::DisplayInfo()
{
	if(drawing) return;
	if(!IsClicked(x1,y1,x2,y2)) return;
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



void CLineStandard::XORDraw(UINT nFlags, CPoint point)
{		
#ifdef _CONSOLE_DEBUG
		//printf("\nCLineStandard::XORDraw() nFlag=%d \n\tnewRect.left=%f newRect.top=%f newRect.right=%f newRect.bottom=%f",nFlags,newRect.left,newRect.top, newRect.right, newRect.bottom);
#endif
	CPointF pointF = CPointF((double)point.x,(double)point.y);
	pointF.x = RealPointX(pointF.x);
	if((int)pointF.x>ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex)))pointF.x=ownerPanel->GetX((int)(pCtrl->endIndex-pCtrl->startIndex))-1;
	pointF.y = RealPointY(pointF.y);
	

	//Set Magnetic Y
	if(pCtrl->m_Magnetic && ownerPanel->index==0) {
		pointF.y = MagneticPointY(pointF.y, pointF.x);
	}

	if (nFlags == 1){ // First pointF


		startX = pointF.x;
		startY = pointF.y;
		if (!hFixed && !vFixed)drawing = true;
		else{
			if(vFixed){
				x1 = pointF.x;
				y1 = pointF.y;
				y2 = y1;
				x2 = pCtrl->width - pCtrl->yScaleWidth;
			}
		}
	
	}
	else if(nFlags == 2){ // Drawing
		CDC* pDC = pCtrl->GetScreen();
		pDC->SetROP2(R2_NOT);
		//ExcludeRects(pDC);
		//pDC->MoveTo(oldRect.left,oldRect.top);
		//pDC->LineTo(oldRect.right,oldRect.bottom);
		DrawLineStudy(pDC, oldRect);
		//IncludeRects(pDC);

		
		/*newRect.left = startX;
		newRect.right = pointF.x;
		newRect.top = startY;
		newRect.bottom = pointF.y;*/

		if(startX <= pointF.x){
			newRect.left = startX;
			newRect.right = pointF.x;
			newRect.top = startY;
			newRect.bottom = pointF.y;
		}
		else {
			newRect.right = startX;
			newRect.left = pointF.x;
			newRect.bottom = startY;
			newRect.top = pointF.y;
		}

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

		DrawLineStudy(pDC, newRect);

		pDC->SetROP2(R2_COPYPEN);
		oldRect = newRect;		
		pCtrl->ReleaseScreen(pDC);
	}
	else if(nFlags == 3)	//	Last point
	{

#ifdef _CONSOLE_DEBUG
		printf("\nLineStandard::XorDraw(3)");
#endif
		y1	= startY;
		y2	= pointF.y;
		x1	= startX;
		x2	= pointF.x;
				
		double temp = 0;
		if(x1 > x2)
		{
			temp = x2;
			x2 = x1;
			x1 = temp;
			temp = y2;
			y2 = y1;
			y1 = temp;
			//flipped = !flipped;
		}
		startX = 0;
		startY = 0;
		pCtrl->movingObject = false;
		pCtrl->RePaint();
		pCtrl->changed = true;
		selected = false;
#ifdef _CONSOLE_DEBUG
		printf("\nSELECTED FALSE 2");
#endif
		Reset();
		drawing = false;		
		drawn = true;



		/*	SGC	03.06.2004	BEG	*/
		CDC* pDC = pCtrl->GetScreen();
		this->OnPaint( pDC );
		pCtrl->ReleaseScreen( pDC ); // Release added 9/29/05
		this->OnPaint( &pCtrl->m_memDC );
		/*	SGC	03.06.2004	END	*/
		
		pCtrl->OnUserDrawingComplete(lsRectangle, key);
		pCtrl->SaveUserStudies();
		
#ifdef _CONSOLE_DEBUG
		printf("\nCLineStandard::XORDraw(3) x2Value=%f y2Value=%f x2JDate=%f",x2Value,y2Value,x2JDate);
#endif
		
	}
}


/////////////////////////////////////////////////////////////////////////////

void CLineStandard::OnRButtonUp(CPoint point)
{		
	if (/*selected*/IsClicked(x1, y1, x2, y2)) { 
		selected = true;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
		pCtrl->FireOnItemRightClick(OBJECT_LINE_STUDY, key);
		
	}
	//setRightExtension(true);
}
/////////////////////////////////////////////////////////////////////////////

void CLineStandard::OnDoubleClick(CPoint point)
{	
	pointOutState = false;
	pointOutStep = 0;
	pCtrl->suspendCrossHair=true;
	pCtrl->RePaint();
	if (selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_LINE_STUDY, key);
		selected = false;
#ifdef _CONSOLE_DEBUG
		printf("\nSELECTED FALSE 3");
#endif
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
}
/////////////////////////////////////////////////////////////////////////////

// Create a CLineStandard,
// set it's x1,y1,x2,y2 and call this.
void CLineStandard::SnapLine()
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