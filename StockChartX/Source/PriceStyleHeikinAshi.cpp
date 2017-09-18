// PriceStyleHeikinAshi.cpp: implementation of the CPriceStyleHeikinAshi class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PriceStyleHeikinAshi.h"
#include "StockChartX.h"



#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define _CONSOLE_DEBUG

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*

Heikin Ashi Formula:

	xClose = (Open+High+Low+Close)/4 
	(average price of the current bar)

	xOpen = (prevXOpen + prevClose) / 2 
	(midpoint of the previous bar)

	xHigh = Max(High, xOpen, xClose) 
	(highest value in the set)

	xLow = Min(Low, xOpen, xClose) 
	(lowest value in the set)

*/

CPriceStyleHeikinAshi::CPriceStyleHeikinAshi(bool Smoothed /*=false*/)
{
	Style = 0;	
	connected = false;
	smoothed = Smoothed;
	pSeries = NULL;
}

CPriceStyleHeikinAshi::~CPriceStyleHeikinAshi()
{
	
}

void CPriceStyleHeikinAshi::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

void CPriceStyleHeikinAshi::OnPaint(CDC* pDC)
{
#ifdef _CONSOLE_DEBUG
	printf("\nHeikinAshi::OnPaint() periods=%d type=%d",pCtrl->SmoothHeikinPeriods, pCtrl->SmoothHeikinType);
#endif
	// Find series
	CSeries* open = pSeries->GetSeriesOHLCV("open");	
	if(NULL == open) return;
	CSeries* high = pSeries->GetSeriesOHLCV("high");	
	if(NULL == high) return;
	CSeries* low = pSeries->GetSeriesOHLCV("low");	
	if(NULL == low) return;
	CSeries* close = pSeries->GetSeriesOHLCV("close");	
	if(NULL == close) return;	

	int smoothPeriod = pCtrl->SmoothHeikinPeriods;
	int estimatedNumCandlesHA = ((pCtrl->startIndex - smoothPeriod) <= 0) ? pCtrl->endIndex : 
		                                       pCtrl->endIndex - (pCtrl->startIndex - smoothPeriod) + 1;

	// Creating field objects
	CField *hasOpen = new CField(estimatedNumCandlesHA, "hasOpen");
	CField *hasClose = new CField(estimatedNumCandlesHA, "hasClose");
	CField *hasLow = new CField(estimatedNumCandlesHA, "hasLow");
	CField *hasHigh = new CField(estimatedNumCandlesHA, "hasHigh");

	// Creating TA objects
 	CNavigator* pNavOpen = new CNavigator();
	CRecordset* pRSOpen = new CRecordset();
	CRecordset* pIndOpen = NULL;

	CNavigator* pNavClose = new CNavigator();
	CRecordset* pRSClose = new CRecordset();
	CRecordset* pIndClose = NULL;

	CNavigator* pNavLow = new CNavigator();
	CRecordset* pRSLow = new CRecordset();
	CRecordset* pIndLow = NULL;

	CNavigator* pNavHigh = new CNavigator();
	CRecordset* pRSHigh = new CRecordset();
	CRecordset* pIndHigh = NULL;

	// Checking to see if everything is fine
	if (hasOpen == NULL || hasClose == NULL || hasLow == NULL || hasHigh == NULL ||
		  pNavOpen == NULL || pRSOpen == NULL || pNavClose == NULL || pRSClose == NULL ||
		  pNavLow == NULL || pRSLow == NULL || pNavHigh == NULL || pRSHigh == NULL) {
		//delete hasOpen;	delete hasClose; delete hasLow;	delete hasHigh;
		delete pNavOpen; delete pNavClose; delete pNavLow; delete pNavHigh;
		delete pRSOpen; delete pRSClose; delete pRSLow; delete pRSHigh;
		return;
	}


	int wick = pCtrl->barWidth;
	int halfWick = (wick / 2);
	if(halfWick < 1) halfWick = 0;
	long width = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;

	double nSpace = 0;
	int rcnt = pCtrl->GetSlaveRecordCount();
	
	nSpace = ceil(nSpace * 0.75); // leave 25% space between candles
	if(nSpace > (int)pCtrl->barSpacing)
		pCtrl->barSpacing = nSpace;

    /*if(rcnt > 1){
		double x1 = GetX(rcnt - 1);
		double x2 = GetX(rcnt);		
		nSpace = ((x2 - x1) / 2) - pCtrl->barWidth / 2;		
		if(nSpace > 20){
			nSpace = 20;
		}
		pCtrl->nSpacing = nSpace;
		if(nSpace > pCtrl->barSpacing)
			pCtrl->barSpacing = (int)nSpace;
		nSpace = ceil(nSpace * 0.75); // leave 25% space between candles
	}*/
		 

	int x1 = 0;
	int x2 = 0;
	double y2 = 0;
	double y1 = 0;
	double y3 = 0;

	int direction = 0;
	int bars = pCtrl->endIndex;
	double total = 0;
	long px = 0;
	CRect box;
	CRect frame;


	OLE_COLOR ucolor, dcolor;
	if(pSeries->upColor != -1)
		ucolor = pSeries->upColor;
	else
		ucolor = pCtrl->upColor;
	if(pSeries->downColor != -1)
		dcolor = pSeries->downColor;
	else
		dcolor = pCtrl->downColor;
	
	int found = pCtrl->barColorName.Find(".vol",0);
	bool customOwner = found == -1;
	CBrush* brCustom = new CBrush(pSeries->lineColor);
	CBrush* downBr = new CBrush(dcolor);
	CBrush* upBr = new CBrush(ucolor);
	CPen* pen = new CPen(pSeries->lineStyle, pSeries->lineWeight, pSeries->lineColor);	
	CPen* pOldPen = NULL;
	pOldPen = pDC->SelectObject(pen);
			
	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex);	
	int cnt = 0;
	


	// Darvas boxes map
	int n = 0, x = 0;
	if(pCtrl->darvasBoxes){
		for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){			
			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE)
			{
					cnt++;
					pCtrl->xMap[cnt] = GetX(cnt + 1);
			}
		}
		CSeriesStock* pStock = (CSeriesStock*)pSeries;
		pStock->PaintDarvasBoxes(pDC);
	}
	x = 0;
	cnt = 0;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
	px = (long)x;



	// Paint the Heikin-Ashi bars
	double xOpen = 0, xHigh = 0, xLow = 0, xClose = 0;
	double prevXOpen = NULL_VALUE, prevXHigh = NULL_VALUE, prevXLow = NULL_VALUE, prevXClose = NULL_VALUE;
	double nOpen = 0, nHigh = 0, nLow = 0, nClose = 0;
	double prevOpen = NULL_VALUE, prevHigh = NULL_VALUE, prevLow = NULL_VALUE, prevClose = NULL_VALUE;

	double x1Temp, x2Temp;
	int widthCandle;
	if ((pCtrl->endIndex - pCtrl->startIndex) >= 1) {
	  x1Temp = GetX(0);
	  x2Temp = GetX(1);
	  widthCandle = (x2Temp - x1Temp)*0.75;
	}
	else {
	  widthCandle = (int)nSpace;
	}
	
	// Make sure the wick is centered
	if (pSeries->IsOdd(wick) != pSeries->IsOdd((int)nSpace) || (!pSeries->IsOdd((int)nSpace) && wick == 1)){
		if(nSpace > 1) nSpace -= 1;
	}

	int numCandlesHA = 0;

	// Vamos apenas montar os valores das séries HA
	for(n = pCtrl->startIndex - smoothPeriod; n != pCtrl->endIndex+1; ++n){
		if (n < 0) continue;

		if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
		   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE)
		{
		
			if(n==0) continue;

			if(prevClose == NULL_VALUE) prevClose = (close->data_slave[n-1].value + high->data_slave[n-1].value +
				  low->data_slave[n-1].value + open->data_slave[n-1].value) / 4;

			// Prime the first Heikin-Ashi candle
			if(prevXClose == NULL_VALUE) {
				prevXClose = (close->data_slave[n-1].value + high->data_slave[n-1].value +
				  low->data_slave[n-1].value + open->data_slave[n-1].value) / 4;					
			}

			if(prevXOpen == NULL_VALUE){ 				
				prevXOpen = (open->data_slave[n-1].value + close->data_slave[n-1].value) / 2;				
			}

			if(prevXHigh == NULL_VALUE) {
				prevXHigh = high->data_slave[n-1].value;
			}

			if(prevXLow == NULL_VALUE) {
				prevXLow = low->data_slave[n-1].value;
			}

			// Heikin-Ashi formula
			nOpen = open->data_slave[n].value;
			nHigh = high->data_slave[n].value;
			nLow = low->data_slave[n].value;
			nClose = close->data_slave[n].value;
			
			xOpen = (prevXOpen + prevXClose) / 2;
			xClose = (nOpen + nHigh + nLow + nClose) / 4;
			xHigh = (nHigh > xOpen) ? nHigh : xOpen; //MaxOf(nHigh, xOpen, xOpen);
			xLow = (nLow < xClose) ? nLow : xClose; //MinOf(nLow, xClose, xClose);

			// Save the previous values
			prevXOpen = xOpen;
			prevXHigh = xHigh;
			prevXLow = xLow;
			prevXClose = xClose;
			prevOpen = nOpen;
			prevHigh = nHigh;
			prevLow = nLow;
			prevClose = nClose;

			// Filling field objetcs
		    hasOpen->setValue(numCandlesHA + 1, xOpen);
		    hasClose->setValue(numCandlesHA + 1, xClose);
		    hasLow->setValue(numCandlesHA + 1, xLow);
		    hasHigh->setValue(numCandlesHA + 1, xHigh);

			numCandlesHA++;
		}
	}

	CSeries haOpen, haClose, haLow, haHigh;
	
	// Aplying Heikin Ashi Smoothed
	if(smoothed){
		
#ifdef _CONSOLE_DEBUG
	printf(" SMOOTH!");
#endif
		pRSOpen->addField(hasOpen);
		pNavOpen->setRecordset(pRSOpen);

		pRSClose->addField(hasClose);
		pNavClose->setRecordset(pRSClose);

		pRSLow->addField(hasLow);
		pNavLow->setRecordset(pRSLow);

		pRSHigh->addField(hasHigh);
		pNavHigh->setRecordset(pRSHigh);

		CMovingAverage taOpen;
		CMovingAverage taClose;
		CMovingAverage taLow;
		CMovingAverage taHigh;
		switch(pCtrl->SmoothHeikinType){
		
			case (int)indExponentialMovingAverage:
				pIndLow = taLow.ExponentialMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.ExponentialMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.ExponentialMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.ExponentialMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
			case (int)indTimeSeriesMovingAverage:
				pIndLow = taLow.TimeSeriesMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.TimeSeriesMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.TimeSeriesMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.TimeSeriesMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
			case (int)indVariableMovingAverage:
				pIndLow = taLow.VariableMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.VariableMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.VariableMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.VariableMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
			case (int)indTriangularMovingAverage:
				pIndLow = taLow.TriangularMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.TriangularMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.TriangularMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.TriangularMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
			case (int)indWeightedMovingAverage:
				pIndLow = taLow.WeightedMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.WeightedMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.WeightedMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.WeightedMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
			case (int)indVIDYA:
				pIndLow = taLow.VIDYA(pNavLow, hasLow, smoothPeriod, 0.5f, "hasLow");
				pIndClose = taClose.VIDYA(pNavClose, hasClose, smoothPeriod, 0.5f, "hasClose");
				pIndOpen = taOpen.VIDYA(pNavOpen, hasOpen, smoothPeriod, 0.5f, "hasOpen");	
				pIndHigh = taHigh.VIDYA(pNavHigh, hasHigh, smoothPeriod, 0.5f, "hasHigh");
				break;
			case (int)indWellesWilderSmoothing:
				pIndLow = taLow.WellesWilderSmoothing(hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.WellesWilderSmoothing(hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.WellesWilderSmoothing(hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.WellesWilderSmoothing(hasHigh, smoothPeriod, "hasHigh");
				break;
			default:
				pIndLow = taLow.SimpleMovingAverage(pNavLow, hasLow, smoothPeriod, "hasLow");
				pIndClose = taClose.SimpleMovingAverage(pNavClose, hasClose, smoothPeriod, "hasClose");
				pIndOpen = taOpen.SimpleMovingAverage(pNavOpen, hasOpen, smoothPeriod, "hasOpen");	
				pIndHigh = taHigh.SimpleMovingAverage(pNavHigh, hasHigh, smoothPeriod, "hasHigh");
				break;
		}

		for(int n = 0; n < numCandlesHA  ; ++n){
			if(n < smoothPeriod - 1){
				haOpen.AppendValue(n,NULL_VALUE);
				haClose.AppendValue(n,NULL_VALUE);
				haLow.AppendValue(n,NULL_VALUE);
				haHigh.AppendValue(n,NULL_VALUE);

			}
			else{
				haOpen.AppendValue(n,pIndOpen->getValue("hasOpen", n + 1));
				haClose.AppendValue(n,pIndClose->getValue("hasClose", n + 1));
				haLow.AppendValue(n,pIndLow->getValue("hasLow", n + 1));
				haHigh.AppendValue(n,pIndHigh->getValue("hasHigh", n + 1));
			}
		}
	}
	//Aplying Normal Heikin Ashi
	else{		
#ifdef _CONSOLE_DEBUG
	printf(" NOT SMOOTH!");
#endif
		// Output the indicator values
		for(int n = 0; n < numCandlesHA  ; ++n){
			if(n < smoothPeriod - 1){
				haOpen.AppendValue(n,NULL_VALUE);
				haClose.AppendValue(n,NULL_VALUE);
				haLow.AppendValue(n,NULL_VALUE);
				haHigh.AppendValue(n,NULL_VALUE);

			}
			else{
				haOpen.AppendValue(n,hasOpen->getValue(n + 1));
				haClose.AppendValue(n,hasClose->getValue(n + 1));
				haLow.AppendValue(n,hasLow->getValue(n + 1));
				haHigh.AppendValue(n,hasHigh->getValue(n + 1));
			}
		}
	}
 

	// Clean up
	delete pRSOpen;	delete pIndOpen; delete pNavOpen;
	delete pRSClose; delete pIndClose; delete pNavClose;
	delete pRSLow; delete pIndLow; delete pNavLow;
	delete pRSHigh; delete pIndHigh; delete pNavHigh;
	//delete hasOpen;	delete hasClose; delete hasLow;	delete hasHigh;

	
	OLE_COLOR wickcolor=pCtrl->upColor;
	if(wickcolor == pCtrl->backGradientTop) wickcolor = RGB(128,128,128);
	// Now it´s time to draw
	x1 = GetX(cnt);
	x2 = GetX(cnt+1);
	int j = 0;
	//Just draw line path if overview:
	if(x2-x1<1.25){
		std::vector<CPointF> pointsUp,pointsDown;
		int trend = 0;
		int weight = x2-x1<1?pSeries->lineWeight:pSeries->lineWeight;
		for(n = pCtrl->startIndex - smoothPeriod; n != pCtrl->endIndex; ++n){
			if (n <= 0) continue;
			if (n < pCtrl->startIndex) {
				j++;
				continue;
			}
			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE && haOpen.data_master[j].value != NULL_VALUE)
			{
 				xOpen = haOpen.data_master[j].value;
				xClose = haClose.data_master[j].value;			
				if(xClose-xOpen>=0)trend++;
				else trend--;
			}			
			if (++j >= numCandlesHA) break;
		}
		j=0;
		for(n = pCtrl->startIndex - smoothPeriod; n != pCtrl->endIndex; ++n){
			if (n <= 0) continue;

			if (n < pCtrl->startIndex) {
				j++;
				continue;
			}
			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE)
			{	
				++cnt;
				x += nSpace;
				x1 = (int)GetX(cnt);
				x2 = x1;// + nSpace;
				pCtrl->xMap[cnt - 1] = x2;

				// if the bar is populated...
				if (haOpen.data_master[j].value != NULL_VALUE)
				{
 					xOpen = haOpen.data_master[j].value;
					xClose = haClose.data_master[j].value;	
					xLow = haLow.data_master[j].value;
					xHigh = haHigh.data_master[j].value;
					// Getting the Y coordinates...
					y1 = pSeries->GetY(xHigh);		
					y2 = pSeries->GetY(xLow);
						
					if(trend>=0){
						pointsUp.push_back(CPointF(x1,y1));
						pointsUp.push_back(CPointF(x2,y2));
						if(xClose-xOpen<0){
							pointsDown.push_back(CPointF(x1,y1));
							pointsDown.push_back(CPointF(x2,y2));
						}
					}
					else {
						pointsDown.push_back(CPointF(x1,y1));
						pointsDown.push_back(CPointF(x2,y2));
						if(xClose-xOpen>=0){
							pointsUp.push_back(CPointF(x1,y1));
							pointsUp.push_back(CPointF(x2,y2));
						}
					}

					//Draw now if there's a gap:
					if((n != pCtrl->endIndex-1) && (pSeries->GetY(xHigh)-pSeries->GetY(haLow.data_master[j+1].value)>5 || pSeries->GetY(haHigh.data_master[j+1].value)-pSeries->GetY(xLow)>5)){
						
						if(trend>=0){
							pCtrl->pdcHandler->DrawPath(pointsUp, pointsUp.size(), pSeries->lineWeight, pSeries->lineStyle, wickcolor, wickcolor, pDC, true);
							pCtrl->pdcHandler->DrawSemiPath(pointsDown, pointsDown.size(), pSeries->lineWeight, pSeries->lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
						}
						else{
							pCtrl->pdcHandler->DrawPath(pointsDown, pointsDown.size(), pSeries->lineWeight, pSeries->lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
							pCtrl->pdcHandler->DrawSemiPath(pointsUp, pointsUp.size(), pSeries->lineWeight, pSeries->lineStyle, wickcolor, wickcolor, pDC, true);
						}
						pointsUp.clear();
						pointsDown.clear();
					}
				}
				if (++j >= numCandlesHA) break;
			}
		}	
		if (trend >= 0){
			pCtrl->pdcHandler->DrawPath(pointsUp, pointsUp.size(), pSeries->lineWeight, pSeries->lineStyle, wickcolor, wickcolor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsDown, pointsDown.size(), weight, pSeries->lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
		}
		else{
			pCtrl->pdcHandler->DrawPath(pointsDown, pointsDown.size(), pSeries->lineWeight, pSeries->lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsUp, pointsUp.size(), weight, pSeries->lineStyle, wickcolor, wickcolor, pDC, true);
		}
		
	}
	//Draw complete candles:
	else{
		for(n = pCtrl->startIndex - smoothPeriod; n != pCtrl->endIndex; ++n){

			if (n < 0) continue;

			if (n < pCtrl->startIndex) {
				j++;
				continue;
			}

			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE)
			{	

				if(n==0)continue;

				// Get color
				if(pCtrl->barColors[n] != -1 && customOwner){ // Custom bar color
					brCustom->DeleteObject();
					if(NULL != brCustom) delete brCustom;
					brCustom = new CBrush(pCtrl->barColors[n]);
				}
					
				// Record the x value
				cnt++;
				x += nSpace;
				x1 = (int)GetX(cnt);
				x2 = x1;// + nSpace;
				pCtrl->xMap[cnt - 1] = x2;
		
				if (haOpen.data_master[j].value != NULL_VALUE) {

 				  xOpen = haOpen.data_master[j].value;
				  xClose = haClose.data_master[j].value;
				  xLow = haLow.data_master[j].value;
				  xHigh = haHigh.data_master[j].value;

				  // First, paint wick			
				  OLE_COLOR wickucolor=pSeries->lineColor;
				  OLE_COLOR wickdcolor=pSeries->lineColor;
				  wickcolor=pSeries->lineColor;
				  OLE_COLOR useColor=pSeries->lineColor;
				  if(pCtrl->wickUpColor != -1)
					wickucolor = pCtrl->wickUpColor;
				  if(pCtrl->wickDownColor != -1)
					wickdcolor = pCtrl->wickDownColor;
				  CPen* pen4;
				  CPen* pen5;

				  /*if((x2 + widthCandle/2)>(x1 - widthCandle/2)+4 ){
						wickcolor = lineColor;
					}
					else wickcolor = (close->data_slave[n].value >= open->data_slave[n].value) ? ucolor : dcolor;
					if(wickcolor == pCtrl->backGradientTop) wickcolor = RGB(128,128,128);
					*/
				  wickcolor = (xClose >= xOpen) ? wickucolor : wickdcolor;
			
				 if((x2 + widthCandle/2)>(x1 - widthCandle/2)+4 /*|| (x2 + widthCandle/2)<(x1 - widthCandle/2)+2*/){//widthCandle>4){
					  pen4 = new CPen(pSeries->lineStyle, pSeries->lineWeight, wickcolor);
					  useColor = wickcolor;
				  }
				  else if(xOpen>=xClose){
					  pen4 = new CPen(pSeries->lineStyle, pSeries->lineWeight, pCtrl->downColor);
					  useColor = dcolor;
				  }
				  else if(pCtrl->downColor == pCtrl->candleUpOutlineColor) {
					  pen4 = new CPen(pSeries->lineStyle, pSeries->lineWeight, RGB(128,128,128));
					  useColor = RGB(128,128,128);
				  }
				  else  {
					  pen4 = new CPen(pSeries->lineStyle, pSeries->lineWeight, pCtrl->upColor);
					  useColor = pCtrl->upColor;
				  }

				  pDC->SelectObject(pen4);
				  y1 = pSeries->GetY(xHigh);		
				  y2 = pSeries->GetY(xLow);
				  y3 = pSeries->GetY(xClose);
				  //if(y2 == y1) y1 = y2 - 2;
				  if(xHigh != xOpen){ 
					//pDC->MoveTo((int)x1,(int)y1);
					//pDC->LineTo((int)x2,(int)y3);
					pCtrl->pdcHandler->DrawLine(CPointF(x1,y1), CPointF(x2,y3), pSeries->lineWeight, pSeries->lineStyle, useColor, NULL, true);
				  }
				  //pDC->SelectObject(pen5);
				  //pOldPen = pDC->SelectObject(pen3); //Don't loose pOldPen!
				  if(xOpen != xLow){ 
					//pDC->MoveTo((int)x1,(int)y3);
					//pDC->LineTo((int)x2,(int)y2);
					pCtrl->pdcHandler->DrawLine(CPointF(x1,y3), CPointF(x2,y2), pSeries->lineWeight, pSeries->lineStyle, useColor, NULL, true);
				  }
				  pDC->SelectObject(pen); //Return to pen...
				  if(NULL != pen4) delete pen4;
				  //if(NULL != pen5) delete pen5;

		 
				  // Next, paint candle
				  pDC->MoveTo((int)(x1 - nSpace),(int)y1);
				  if(pCtrl->barColors[n] != -1 && customOwner){ // Custom bar color				
					brCustom->DeleteObject();
					if(NULL != brCustom) delete brCustom;
					brCustom = new CBrush(pCtrl->barColors[n]);
					y1 = pSeries->GetY(xOpen);
					y2 = pSeries->GetY(xClose);
					//if(y1 + 3 > y2) y2 += 2;

					//CRect candle((int)(x1 - nSpace - halfWick), (int)(y1), (int)(x2 + nSpace + halfWick),(int)(y2));	
					CRectF candle((double)((double)x1 - (double)widthCandle/2), (y1), (double)((double)x2 + (double)widthCandle/2),(y2));

					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, pCtrl->barColors[n]);
					}
					else{
						//pDC->Rectangle(candle);
						//pDC->FillRect(candle, brCustom);
						pCtrl->pdcHandler->FillRectangle(candle, pCtrl->barColors[n], NULL);
					}
				
					if(xOpen < xClose){ // Down
						if(pCtrl->candleDownOutlineColor != -1){ // Draw a box (hollow candle)
							if(candle.right>(candle.left+4)|| pCtrl->downColor==pCtrl->backGradientTop || pCtrl->downColor==pCtrl->backGradientBottom){
								//pDC->Draw3dRect(candle, pCtrl->candleDownOutlineColor, pCtrl->candleDownOutlineColor);	
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->candleDownOutlineColor, NULL);
							}
							else {
								//pDC->Draw3dRect(candle, pCtrl->downColor, pCtrl->downColor);	
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->downColor, NULL);
							}
						}
					}
					else if(xOpen > xClose){ // Up
						if(pCtrl->candleUpOutlineColor != -1){ // Draw a box (hollow candle)
							if(candle.right>(candle.left+4)){
								//pDC->Draw3dRect(candle, pCtrl->candleUpOutlineColor, pCtrl->candleUpOutlineColor);	
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->candleUpOutlineColor, NULL);
							}
							else if(pCtrl->downColor == pCtrl->candleUpOutlineColor) {
								//pDC->Draw3dRect(candle, RGB(128,128,128), RGB(128,128,128));	
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, RGB(128,128,128), NULL);
							}

							else {
								//pDC->Draw3dRect(candle, pCtrl->upColor, pCtrl->upColor);		
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->upColor, NULL);
						
							}
						}
					}				
				  }
				  else if(xOpen > xClose){ // Down
					y1 = pSeries->GetY(xOpen);
					y2 = pSeries->GetY(xClose);
					if(y1 == y2) y2 +=  1;
					//if(y1 + 3 > y2) y2 += 2;
					//if(widthCandle<=1)widthCandle=2;
					//CRect candle ((int)(x1 - nSpace - halfWick),(int) y1, (int)(x2 + nSpace + halfWick),(int)y2);
					CRectF candle((double)((double)x1 - (double)widthCandle/2), (y1), (double)((double)x2 + (double)widthCandle/2),(y2));

					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, dcolor);
					}
					else{
						//pDC->Rectangle(candle);
						//pDC->FillRect(candle,downBr);	
						if(n==2){
	#ifdef _CONSOLE_DEBUG
							printf("\nFillRectangle() wc=%d x1=%d x2=%d y1=%f y2=%f",widthCandle,x1,x2,y1,y2);
	#endif
						}
						pCtrl->pdcHandler->FillRectangle(candle, dcolor, NULL);
					}
					if(pCtrl->candleDownOutlineColor != -1){ // Draw a box (hollow candle)
						if(candle.right>(candle.left+4)|| pCtrl->downColor==pCtrl->backGradientTop || pCtrl->downColor==pCtrl->backGradientBottom){
								//pDC->Draw3dRect(candle, pCtrl->candleDownOutlineColor, pCtrl->candleDownOutlineColor);		
								pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->candleDownOutlineColor, NULL);
						}
						else {
							//pDC->Draw3dRect(candle, pCtrl->downColor, pCtrl->downColor);		
							pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->downColor, NULL);
						}
					}
				  }
				  else /*if(xOpen < xClose)*/{ // Up
					y1 = pSeries->GetY(xClose);
					y2 = pSeries->GetY(xOpen);
					if(y1 == y2) y2 +=  1;
					//if(y1 + 3 > y2) y2 += 2;
					//if(widthCandle<=1)widthCandle=2;

					//CRect candle((int)(x1 - nSpace - halfWick), (int)y1, (int)(x2 + nSpace + halfWick),(int)y2);
					CRectF candle((double)((double)x1 - (double)widthCandle/2), (y1), (double)((double)x2 + (double)widthCandle/2),(y2));

					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, ucolor);
					}
					else{
						//pDC->FillRect(candle,upBr);
						if(n==2){
	#ifdef _CONSOLE_DEBUG
							printf("\nFillRectangle() wc=%d x1=%d x2=%d y1=%f y2=%f",widthCandle,x1,x2,y1,y2);
	#endif
						}
						pCtrl->pdcHandler->FillRectangle(candle, ucolor, NULL);
					}
					if(pCtrl->candleUpOutlineColor != -1){ // Draw a box (hollow candle)
						if(candle.right>(candle.left+4)){
							//pDC->Draw3dRect(candle, pCtrl->candleUpOutlineColor, pCtrl->candleUpOutlineColor);
							pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->candleUpOutlineColor, NULL);	
						}
						else if(pCtrl->downColor == pCtrl->candleUpOutlineColor) {
							//pDC->Draw3dRect(candle, RGB(128,128,128), RGB(128,128,128));
							pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, RGB(128,128,128), NULL);
						}

						else {
							//pDC->Draw3dRect(candle, pCtrl->upColor, pCtrl->upColor);
							pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pCtrl->upColor, NULL);	
						}
					}
				  }
				  /*else{ // No change, flat bar
					y1 = pSeries->GetY(xClose);
					y2 = pSeries->GetY(xOpen);
					if(y2 == y1) y1 = y2 - 1;

					//CRect candle((int)(x1 - nSpace - halfWick), (int)y1, (int)(x2 + nSpace + halfWick),(int) y2);
					CRectF candle((int)(x1 - widthCandle/2), (int)(y1), (int)(x2 + widthCandle/2),(int)(y2));

					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, pSeries->lineColor);
					}
					else{
						//pDC->Draw3dRect(candle,pSeries->lineColor, pSeries->lineColor);
						pCtrl->pdcHandler->DrawRectangle(candle, pSeries->lineWeight, pSeries->lineStyle, pSeries->lineColor, NULL);	
					}
				  }*/
				} 

				px = (long)x;
				if (++j >= numCandlesHA) break;
			}
		}
	}

	pDC->SelectObject(pOldPen);

	brCustom->DeleteObject();
	downBr->DeleteObject();
	upBr->DeleteObject();
	pen->DeleteObject();
	
	if(NULL != brCustom) delete brCustom;
	if(NULL != downBr) delete downBr;
	if(NULL != upBr) delete upBr;
	if(NULL != pen) delete pen;

	CPriceStyle::OnPaint(pDC);
}

 

// Gradient candle from left to right
void CPriceStyleHeikinAshi::DrawGradientCandle(CDC *pDC, CRect boxRect, OLE_COLOR color)
{

	if(boxRect.Width() < 2){
		CBrush* br = new CBrush(color);
		pDC->Rectangle(boxRect);
		pDC->FillRect(boxRect, br);
		br->DeleteObject();
		if(NULL != br ) delete br;
		return;
	}

	CPen* pen = new CPen(pSeries->lineStyle, pSeries->lineWeight, pSeries->lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	pCtrl->FadeHorz(pDC, color,RGB(80,80,80), boxRect);
	pDC->MoveTo(boxRect.top, boxRect.left);

	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

	pen = new CPen(pSeries->lineStyle, pSeries->lineWeight, color);
	pOldPen = pDC->SelectObject(pen);
	pDC->MoveTo(boxRect.left, boxRect.top);
	pDC->LineTo(boxRect.right, boxRect.top);
	
	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

}


double CPriceStyleHeikinAshi::GetX(int period)
{
	double nWidth = 0;
    double nSpacing = 0;
	double dRetVal;
	long recCnt = pCtrl->GetSlaveRecordCount(); 
    nWidth = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;
    if(recCnt > 0) nSpacing = nWidth / (recCnt);	
	dRetVal = (period - 1) * nSpacing;
	if(pCtrl->yAlignment == LEFT){
		dRetVal += pCtrl->yScaleWidth + 5;		
	}
    return dRetVal;
}

double CPriceStyleHeikinAshi::MaxOf(double a, double b, double c)
{
	double ret = a;	
	if(b > ret) ret = b;
	if(c > ret) ret = c;
	return ret;
}

double CPriceStyleHeikinAshi::MinOf(double a, double b, double c)
{
	double ret = a;	
	if(b < ret) ret = b;
	if(c < ret) ret = c;
	return ret;
}