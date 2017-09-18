// COL.cpp: implementation of the CCOL class.
//
//////////////////////////////////////////////////////////////////////

//#include "stdafx.h"
#include "StockChartX.h"
#include "COL.h"
#include "julian.h"
#include "StdAfx.h"

//#define _CONSOLE_DEBUG


#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CCOL::CCOL()
{
	lineColor = RGB(255,0,0);
	Initialize();
}

CCOL::CCOL(OLE_COLOR color, CChartPanel* owner)
{
	ownerPanel = owner;	
	lineColor = color;
	Initialize();
}

void CCOL::Connect(CStockChartXCtrl *Ctrl)
{
	pCtrl = Ctrl;
	guid = pCtrl->CreateGUID();
	connected = true;
}

void CCOL::Initialize()
{
	fillStyle = fsTransparent; // 5/20/05 RG
	zOrder = zOrderFront;
	x1 = 0.0f; // Pixels
	x2 = 0.0f;
	y1 = 0.0f;
	y2 = 0.0f;
	x1_2 = 0; // Pixels
	x2_2 = 0;
	y1_2 = 0;
	y2_2 = 0;
	x1Value = 0; // Chart values
	x2Value = 0;
	y1Value = 0;
	y2Value = 0;
	x1Value_2 = 0; // Chart values
	x2Value_2 = 0;
	y1Value_2 = 0;
	y2Value_2 = 0;	
	x1JDate = 0;
	x2JDate = 0;
	x1JDate_2 = 0;
	x2JDate_2 = 0;

	lineWeight = 1;
	lineStyle = 0;	
	selectable = true;
	selected = false;
	connected = false;
	nSavedDC = 0;
	hFixed = false;
	vFixed = false;
	isFirstPointArrow = false;
	pointOutState = false;
	pointOutStep = 0;
	
	// Params set by CStockChartXCtrl::SetLineStudyParam
	// Exclusively for Line Studies.
	params.resize(MAX_PARAMS);
	for(int n = 0; n != params.size(); ++n){
		params[n] = NULL_VALUE;//pCtrl->fibonacciParams[n];
	}
	//New Atributes -> Extensions on TrendLine
	leftExtension = false;
	rightExtension = false;
	radiusExtension = false;

	leftPoint  = CPointF(NULL_VALUE,NULL_VALUE);
	rightPoint  = CPointF(NULL_VALUE,NULL_VALUE);
	leftPoint_2  = CPointF(NULL_VALUE,NULL_VALUE);
	rightPoint_2  = CPointF(NULL_VALUE,NULL_VALUE);

}

// Set default values for study
void CCOL::SetUserDefault(void)
{	
	//lineWeight = pCtrl->lineThickness;
	//lineColor = pCtrl->lineColor;
	//for(int i=0;i<10;i++) params[i]=pCtrl->fibonacciParams[i];
}

CCOL::~CCOL()
{

}

void CCOL::OnPaint(CDC *pDC)
{

}

void CCOL::OnPaintXOR(CDC *pDC)
{

}

void CCOL::InvalidateXOR()
{

}

void CCOL::OnRButtonDown(CPoint point)
{

}

void CCOL::OnLButtonDown(CPoint point)
{

}

void CCOL::OnLButtonUp(CPoint point)
{

}

void CCOL::OnMouseMove(CPoint point)
{

}

bool CCOL::IsClicked(double x, double y, double cx, double cy)
{
	double nError = 10;
	bool lineHit = false;
	double b = 0;
	double c = 0;
	double currentY = pCtrl->m_point.y;
	double currentX = pCtrl->m_point.x;

	try{

		if(x == cx){ // Vertical line
			// Is the mouse within nError of the vertical line?
			if(currentX > x-nError && currentX < x + nError && 
				((currentY>y-nError && currentY < cy+nError)||(currentY>cy-nError && currentY < y+nError))){
			
				lineHit = true;
			}
			return lineHit;
		}
		else if(y == cy){ // Horizontal line
			if((y > currentY - nError && cy < currentY + nError) &&	(x < currentX && cx > currentX || abs(x-currentX)<(nError) || abs(cx-currentX)<(nError))){
				lineHit = true;
			}
			return lineHit;
		}
	
		//Is over Point1 or Point2?
		if((currentX > x-10 && currentY > y-10 && currentX < x+10 && currentY < y+10)||(currentX > cx-10 && currentY > cy-10 && currentX < cx+10 && currentY < cy+10)){
					lineHit = true;	
		}

		if((y - cy) != 0 && (x - cx) != 0){
			b = (y - cy) / (x - cx);
		}
		c = y - b * x;

	  
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
		if(abs((currentY - (b * currentX + c))) < nError){    
	//	End Of Revision
			//if(currentY >= y && currentY <= cy || currentX >= x && currentX <= cx /*|| (abs(currentX-x)<nError) || (abs(currentX-cx)<nError) || (abs(currentY-y)<nError) || (abs(currentY-cy)<nError)*/){
			if (currentX >= x && currentX <= cx) lineHit = true;
			//}
		}
	}
	catch(...){}


	return lineHit;
}

//Used in Polyline or FreeHand
bool CCOL::IsPointsClicked(std::vector<double> xValues,std::vector<double> yValues)
{
	int nError = 10;
	bool lineHit = false;
	double b = 0;
	double c = 0;
	double currentY = pCtrl->m_point.y;
	double currentX = pCtrl->m_point.x;
				
	for(int i=0;i<xValues.size()-1;i++){
		double tempX1 = pCtrl->panels[0]->GetX(xValues[i]-pCtrl->startIndex);
		double tempY1 = ownerPanel->GetY(yValues[i]);
		double tempX2 = pCtrl->panels[0]->GetX(xValues[i+1]-pCtrl->startIndex);
		double tempY2 = ownerPanel->GetY(yValues[i+1]);

		//Flipped?
		if(tempX1>tempX2)
		{
			double tempX = tempX1;
			double tempY = tempY1;
			tempX1 = tempX2;
			tempX2=tempX;
			tempY1=tempY2;
			tempY2=tempY;
		}

		// Line is Y=BX+C so find B and C
		if((tempY1 - tempY2) != 0 && (tempX1 - tempX2) != 0){
			b = (tempY1 - tempY2) / (tempX1 - tempX2);
		}
		c = tempY1 - b * tempX1;
		if(abs((currentY - (b * currentX + c))) < nError){   
			//if(currentY >= y && currentY <= cy || currentX >= x && currentX <= cx /*|| (abs(currentX-x)<nError) || (abs(currentX-cx)<nError) || (abs(currentY-y)<nError) || (abs(currentY-cy)<nError)*/){
			if(currentY >= tempY1 && currentY <= tempY2 || currentX >= tempX1 && currentX <= tempX2 /*|| (abs(currentX-x)<nError) || (abs(currentX-cx)<nError) || (abs(currentY-y)<nError) || (abs(currentY-cy)<nError)*/){
				lineHit = true;
			}
		}
		//Test region between 2 points on diagonal:
		if((currentX > tempX1-10 && currentY > tempY1-10 && currentX < tempX1+10 && currentY < tempY1+10)||(currentX > tempX2-10 && currentY > tempY2-10 && currentX < tempX2+10 && currentY < tempY2+10)){
					lineHit = true;	
		}
		//Test if it's almost horizontal line
		if(abs(tempX1-tempX2)<nError){
			if(tempY1<tempY2){
				if(currentX > tempX1-10 && currentX < tempX1+10 && currentY > tempY1-10  && currentY < tempY2+10){
					lineHit = true;	
				}
			}
			else{
				if(currentX > tempX1-10 && currentX < tempX1+10 && currentY > tempY2-10  && currentY < tempY1+10){
					lineHit = true;	
				}
			}
		}
		//Test if it's almost vertical line
		if(abs(tempY1-tempY2)<nError){
			if(tempX1<tempX2){
				if(currentY > tempY1-10 && currentY < tempY1+10 && currentX > tempX1-10  && currentX < tempX2+10){
					lineHit = true;	
				}
			}
			else{
				if(currentY > tempY1-10 && currentY < tempY1+10 && currentX > tempX2-10  && currentX < tempX1+10){
					lineHit = true;	
				}
			}
		}
	}

	
	return lineHit;
}

// Sets chart value lookup based on actual pixel position
void CCOL::Reset()
{
#ifdef _CONSOLE_DEBUG
	printf("\nCCOL::Reset()");
#endif
	//if(x1 < 1 || x2 < 1) return; // 2/3/06 - Don't reset if 0, removed 3/22/09
	double y1ValueOld = y1Value;
	x1Value = ownerPanel->GetReverseX(x1) + 1 + pCtrl->startIndex;
	x2Value = ownerPanel->GetReverseX(x2) + 1 + pCtrl->startIndex;
	y1Value = ownerPanel->GetReverseY(y1);
	y2Value = ownerPanel->GetReverseY(y2);
	// For channel second variables
	x1Value_2 = ownerPanel->GetReverseX(x1_2) + 1 + pCtrl->startIndex;
	x2Value_2 = ownerPanel->GetReverseX(x2_2) + 1 + pCtrl->startIndex;
	y1Value_2 = ownerPanel->GetReverseY(y1_2);
	y2Value_2 = ownerPanel->GetReverseY(y2_2);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	x1 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex));
	x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex));
	x1_2 = ownerPanel->GetX((int)(x1Value_2 - pCtrl->startIndex));
	x2_2 = ownerPanel->GetX((int)(x2Value_2 - pCtrl->startIndex));

	// 1/14/04 WB
	// int offset = ownerPanel->yOffset;
	// if(offset > 0) offset = ownerPanel->yOffset + 2;

	y1 = ownerPanel->GetY(y1Value);
	y2 = ownerPanel->GetY(y2Value);
	y1_2 = ownerPanel->GetY(y1Value_2);
	y2_2 = ownerPanel->GetY(y2Value_2);
	//valuePosition = y1;
	if(x1 < 1 || x2 < 1) Update(); // added 3/22/09
		
	switch(pCtrl->GetPeriodicity())
	{
		case Minutely:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
		case Hourly:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
		case Daily:		
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
		case Weekly:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
		case Month:		
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
		case Year:	
			x1JDate = ownerPanel->series[0]->GetJDate((int)x1Value-1);
			x2JDate =  ownerPanel->series[0]->GetJDate((int)x2Value-1);
			x1JDate_2 = ownerPanel->series[0]->GetJDate((int)x1Value_2-1);
			x2JDate_2 = ownerPanel->series[0]->GetJDate((int)x2Value_2-1);
			break;
	}

//	End Of Revision
}

// Sets chart value lookup based on actual pixel position
double CCOL::RealPointX(double pointX)
{
	double xValue;
	double x;

	xValue = pCtrl->panels[0]->GetReverseX(pointX) + 1 + pCtrl->startIndex;
	x = (int)pCtrl->panels[0]->GetX((int)(xValue - pCtrl->startIndex));

	return x;
}

// Sets chart value lookup based on actual pixel position
double CCOL::RealPointY(double pointY)
{
	double yValue;
	double y;

	yValue = ownerPanel->GetReverseY(pointY);
	y = ownerPanel->GetY(yValue);

	return y;
}

// Sets chart value lookup based on candle's max/min position
double CCOL::MagneticPointY(double pointY, double pointX)
{
	double yValue, xValue;
	double y;
	
	yValue = ownerPanel->GetReverseY(pointY);
	xValue = ownerPanel->GetReverseX(pointX);
		
	CString sName;
	CSeriesStock *seriesStock = NULL;
	sName = pCtrl->m_symbol + ".low";
	CSeries *seriesSt = ownerPanel->GetSeries(sName);
	if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
		seriesStock = (CSeriesStock *) seriesSt;
	}
	if(seriesStock != NULL){
		Candle c = seriesStock->GetCandle((int)(pCtrl->startIndex +  xValue));
		if( yValue > (c.GetMax() + c.GetMin())/2 )  {
			y = ownerPanel->GetY(c.GetMax()); 
		} else {
			y = ownerPanel->GetY(c.GetMin()); 
		}
	}
	return y;
}

// Sets chart Y Value lookup based on candle's max/min position
double CCOL::MagneticPointYValue(double pointY, double pointX)
{
	double yValue, xValue;
	double y;
	
	yValue = ownerPanel->GetReverseY(pointY);
	xValue = ownerPanel->GetReverseX(pointX);
		
	CString sName;
	CSeriesStock *seriesStock = NULL;
	sName = pCtrl->m_symbol + ".low";
	CSeries *seriesSt = ownerPanel->GetSeries(sName);
	if(seriesSt->seriesType==OBJECT_SERIES_CANDLE || seriesSt->seriesType == OBJECT_SERIES_STOCK || seriesSt->seriesType == OBJECT_SERIES_BAR || seriesSt->seriesType == OBJECT_SERIES_STOCK_HLC || seriesSt->seriesType == OBJECT_SERIES_STOCK_LINE){
		seriesStock = (CSeriesStock *) seriesSt;
	}
	if(seriesStock != NULL){
		Candle c = seriesStock->GetCandle((int)(pCtrl->startIndex +  xValue));
		/*
#ifdef _CONSOLE_DEBUG
		c.PrintCandle();
		printf("StartIndex %i\n", pCtrl->startIndex);
		printf("ActualY %f\n",ownerPanel->GetReverseY( (double) pr.y));
		printf("Media %f \n", (c.GetMax() + c.GetMin())/2);
#endif
		*/
		if( yValue > (c.GetMax() + c.GetMin())/2 )  {
			y = c.GetMax(); 
		} else {
			y = c.GetMin(); 
		}
	}
	return y;
}

void CCOL::Update()
{
	if (!connected) return;
#ifdef _CONSOLE_DEBUG
	printf("\nCCOL::Update()");
#endif
	//Revision for SemiLog Scale:
	/*if(pCtrl->scalingType == SEMILOG){
		x1 = (int)ownerPanel->GetX((int)(x1Value - pCtrl->startIndex), true); // 2/3/06 offscreen
		x2 = (int)ownerPanel->GetX((int)(x2Value - pCtrl->startIndex), true); // 2/3/06 offscreen
		if((int)x1Value<pCtrl->startIndex){
			printf("  x1Value(%f)<start(%d)",x1Value,pCtrl->startIndex);
			y1 = (int)ownerPanel->GetY(y1Value);
		}
		else y1 = (int)ownerPanel->GetY(y1Value);
		if((int)x2Value>pCtrl->endIndex){
			printf("  x2Value(%f)>end(%d)",x2Value,pCtrl->endIndex);
			y2 = (int)ownerPanel->GetY(y2Value);
		}
		else y2 = (int)ownerPanel->GetY(y2Value);
		x1_2 = (int)ownerPanel->GetX((int)(x1Value_2 - pCtrl->startIndex), true); // 2/3/06 offscreen
		x2_2 = (int)ownerPanel->GetX((int)(x2Value_2 - pCtrl->startIndex), true); // 2/3/06 offscreen
		if((int)x1Value_2<pCtrl->startIndex)y1_2 = (int)ownerPanel->GetY(y1Value_2);
		else y1_2 = (int)ownerPanel->GetY(y1Value_2);
		if((int)x2Value_2>pCtrl->endIndex)y2_2 = (int)ownerPanel->GetY(y2Value_2);
		else y2_2 = (int)ownerPanel->GetY(y2Value_2);
		return;
	}*/
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	x1 = ownerPanel->GetX((int)(x1Value - pCtrl->startIndex), true); // 2/3/06 offscreen
	x2 = ownerPanel->GetX((int)(x2Value - pCtrl->startIndex), true); // 2/3/06 offscreen
	y1 = ownerPanel->GetY(y1Value);
	y2 = ownerPanel->GetY(y2Value);
	x1_2 = ownerPanel->GetX((int)(x1Value_2 - pCtrl->startIndex), true); // 2/3/06 offscreen
	x2_2 = ownerPanel->GetX((int)(x2Value_2 - pCtrl->startIndex), true); // 2/3/06 offscreen
	y1_2 = ownerPanel->GetY(y1Value_2);
	y2_2 = ownerPanel->GetY(y2Value_2);
	//valuePosition = y1;

//	End Of Revision
}


// Returns the angle of this line
int CCOL::GetAngle(){
	double length = 0;
	double ang = 0;
	double pi = 3.14159265358979f;
	int temp = 0;
	int ay1 = 0;
	int ay2 = 0;
	int ax1 = 0;
	int ax2 = 0;    
	ay1 = y1;
	ay2 = y2;
	ax1 = x1;
	ax2 = x2;
	if(ax1 < ax2){
		temp = ax1;
		ax1 = ax2;
		ax2 = temp;
	}
	if(ay1 < ay2){
		temp = ay1;
		ay1 = ay2;
		ay2 = temp;
	}
	length = sqrt((double)(ax1 - ax2) * (ax1 - ax2) + (ay1 - ay2) * (ay1 - ay2));
	ang = atan((ay1 - ay2) / ((ax1 - ax2) + 0.01f)) * 180 / pi;
	return (int)ang;
}

void CCOL::OnDoubleClick(CPoint point)
{

}

void CCOL::OnMessage(LPCTSTR MsgGuid, int MsgID)
{

}

void CCOL::XORDraw(UINT nFlags, CPoint point)
{

}

void CCOL::OnRButtonUp(CPoint point)
{

}

void CCOL::SnapLine()
{

}

// Prevents drawing over certain areas
void CCOL::ExcludeRects(CDC* pDC)
{	
	// Save the old DC	
	nSavedDC = pDC->SaveDC();
	// Don't draw over the y scale
	pDC->ExcludeClipRect(ownerPanel->yScaleRect);
	CRect panel = ownerPanel->panelRect;
	panel.bottom = panel.top;
	panel.top = 0; // Don't draw above this panel	
	pDC->ExcludeClipRect(panel);
	// Don't draw below the panel
	panel = ownerPanel->panelRect;
	panel.top = panel.bottom;
	panel.bottom = pCtrl->height + CALENDAR_HEIGHT;
	pDC->ExcludeClipRect(panel);	
}

void CCOL::IncludeRects(CDC* pDC)
{
	pDC->RestoreDC(nSavedDC);
}

// m_testRgn1 and m_testRgn2 must be set in OnPaint
bool CCOL::IsRegionClicked()
{
	if(!connected) return false;
	bool a = PtInRegion(m_testRgn1, pCtrl->m_point.x, pCtrl->m_point.y) == TRUE;
	bool b = PtInRegion(m_testRgn2, pCtrl->m_point.x, pCtrl->m_point.y) == TRUE;

	// Also check grabber areas
	bool c = false;
	if(pCtrl->m_point.x > x1-15 && pCtrl->m_point.y > 
		y1-15 && pCtrl->m_point.x < x1+15 && pCtrl->m_point.y < y1+15){
		c = true;
	}
	else if(pCtrl->m_point.x > x2-15 && pCtrl->m_point.y > 
		y1-15 && pCtrl->m_point.x < x2+15 && pCtrl->m_point.y < y1+15){
		c = true;
	}
	else if(pCtrl->m_point.x > x2-15 && pCtrl->m_point.y > 
		y2-15 && pCtrl->m_point.x < x2+15 && pCtrl->m_point.y < y2+15){
		c = true;
	}
	else if(pCtrl->m_point.x > x1-15 && pCtrl->m_point.y > 
		y2-15 && pCtrl->m_point.x < x1+15 && pCtrl->m_point.y < y2+15){
		c = true;
	}
	//midle intersection between x1 e x2 (around y1):
	else if(pCtrl->m_point.x > x1-15 && pCtrl->m_point.x < x2+15 && pCtrl->m_point.y > y1-15 && pCtrl->m_point.y < y1+15){
		c = true;
	}
	//midle intersection between x1 e x2 (around y2):
	else if(pCtrl->m_point.x > x1-15 && pCtrl->m_point.x < x2+15 && pCtrl->m_point.y > y2-15 && pCtrl->m_point.y < y2+15){
		c = true;
	}
	//midle intersection between y1 e y2 (around x1):
	else if(pCtrl->m_point.y > y1-15 && pCtrl->m_point.y < y2+15 && pCtrl->m_point.x > x1-15 && pCtrl->m_point.x < x1+15){
		c = true;
	}
	//midle intersection between y1 e y2 (around x2):
	else if(pCtrl->m_point.y > y1-15 && pCtrl->m_point.y < y2+15 && pCtrl->m_point.x > x2-15 && pCtrl->m_point.x < x2+15){
		c = true;
	}

	return (a == true && b == false) || c;
}

void CCOL::DrawRect(CDC* pDC, CRect rect)
{

	pDC->MoveTo(rect.left, rect.top);
	pDC->LineTo(rect.right, rect.top);
	pDC->MoveTo(rect.left, rect.bottom);
	pDC->LineTo(rect.right, rect.bottom);
	pDC->MoveTo(rect.right, rect.top);
	pDC->LineTo(rect.right, rect.bottom);
	pDC->MoveTo(rect.left, rect.top);
	pDC->LineTo(rect.left, rect.bottom);
}




// 2/4/05
CRGB CCOL::GetRGB(int color) {
    int tmp = color; CRGB out;

    out.red = tmp % 256;
    tmp = tmp / 256;
    out.green = tmp % 256;
    tmp = tmp / 256;
    out.blue = tmp % 256;
    tmp = tmp / 256;
    
    return out;
}

// DO NOT USE THIS FUNCTION - IT NEEDS SOME SPEEDING UP!
void CCOL::FillTranslucent(int color, double opacity, CDC* pDC, CRect rect)
{
	CRGB tmp, tmp1 = GetRGB(color); 
	int r=0,g=0,b=0;
	for(int x = rect.left; x != rect.right + 1; x++){
		for(int y = rect.top; y != rect.bottom + 1; y++){
			COLORREF color = pDC->GetPixel(x,y);
			tmp = GetRGB(color);
			r = static_cast<int>(tmp1.red * opacity + tmp.red * (1.0f - opacity));
			g = static_cast<int>(tmp1.green * opacity + tmp.green * (1.0f - opacity));
			b = static_cast<int>(tmp1.blue * opacity + tmp.blue * (1.0f - opacity));
			pDC->SetPixelV(x,y,RGB(r,g,b));
		}
	}
}

