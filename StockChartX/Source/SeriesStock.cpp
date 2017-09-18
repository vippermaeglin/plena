// SeriesStock.cpp: implementation of the CSeriesStock class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "SeriesStock.h"
#include <time.h>

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSeriesStock::CSeriesStock(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	isTwin = false;
	upColor = -1;
	downColor = -1;
	wickUpColor = -1;
	wickDownColor = -1;
	szName = name;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	seriesType = type;
	memberCount = members;
	Initialize();
	nSpace = 0;


}

CSeriesStock::~CSeriesStock()
{

}

void CSeriesStock::OnPaint(CDC *pDC)
{
	//Get User's Price configuration:
	lineColor = pCtrl->GetContrastColor(pCtrl->backGradientTop);
	lineWeight = 1;
	if(!seriesVisible) return;
	if(data_slave.size() < 1) return;

	//Get length of open and close lines    
    int rcnt = pCtrl->GetSlaveRecordCount();
    if(rcnt > 1){
		double x1 = ownerPanel->GetX(rcnt - 1);
		double x2 = ownerPanel->GetX(rcnt);		
		nSpace = ((x2 - x1) / 2) - pCtrl->barWidth / 2;		
		if(nSpace > 20){
			nSpace = 20;
		}
		pCtrl->nSpacing = nSpace;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		if(nSpace > pCtrl->barSpacing)
			pCtrl->barSpacing = (int)nSpace;
//	End Of Revision
	}

	CString type;
	type = szName;
	type.MakeLower();
	int found = 0;

	found = type.Find(".close", 0);
	if(found != -1){

		

		// Draw a non standard price style
		// such as P&F, Renko, Kagi, etc.
		if(pCtrl->priceStyle != psStandard){
			CPriceStyle* pStyle = NULL;
			switch(pCtrl->priceStyle){
			case psPointAndFigure:
				pStyle = new CPriceStylePointAndFigure();								
				break;
			case psEquiVolume:
				pStyle = new CPriceStyleEquiVolume();
				break;
			case psEquiVolumeShadow:
				pStyle = new CPriceStyleEquiVolume();
				pStyle->Style = psEquiVolumeShadow;
				break;
			case psCandleVolume:
				pStyle = new CPriceStyleEquiVolume();
				pStyle->Style = psCandleVolume;
				break;		
			case psThreeLineBreak:
				pStyle = new CPriceStyleThreeLineBreak();
				break;
			case psRenko:
				pStyle = new CPriceStyleRenko();
				break;
			case psKagi:
				pStyle = new CPriceStyleKagi();
				break;
			case psHeikinAshi:
				pStyle = new CPriceStyleHeikinAshi();
				break;
			case psHeikinAshiSmooth:
				pStyle = new CPriceStyleHeikinAshi(true);
				break;
			}
			if(NULL != pStyle) {
				pCtrl->styleSeries = this;
				pStyle->Connect(pCtrl, this);
				pStyle->OnPaint(pDC);
				delete pStyle;
			}

			return;
		}


		// Paint Darvas boxes?
		if(pCtrl->darvasBoxes)
			PaintDarvasBoxes(pDC);
		


		// Just draw a candle or stock chart
		if(seriesType == OBJECT_SERIES_CANDLE){
			PaintCandles(pDC);
		}
		else if(seriesType == OBJECT_SERIES_STOCK || 
			seriesType == OBJECT_SERIES_STOCK_HLC){
			PaintStock(pDC);
		}
		else if(seriesType == OBJECT_SERIES_STOCK_LINE){
			PaintLines(pDC);
		}

	}
	
}


Candle CSeriesStock::GetCandle(int record){
//
#ifdef _CONSOLE_DEBUG
		//printf("record: %i \n", record);		
#endif

	double high_v;
	double low_v;
	double open_v;
	double close_v;
	
	// Find series
	CSeries* open = GetSeriesOHLCV("open");	
	//if(NULL == open) return; // RDG 8/2/04
	CSeries* high = GetSeriesOHLCV("high");	
//	if(NULL == high || high->data_master.size() == 0) return; // 11/21/05
	CSeries* low = GetSeriesOHLCV("low");	
//	if(NULL == low || low->data_master.size() == 0) return; // 11/21/05
	CSeries* close = GetSeriesOHLCV("close");	
//	if(NULL == close || close->data_master.size() == 0) return; // 11/21/05			

	high_v = high->data_slave[record].value;
	open_v = open->data_slave[record].value;
	close_v = close->data_slave[record].value;
	low_v = low->data_slave[record].value;

	return Candle(open_v,close_v,high_v,low_v);

}

void CSeriesStock::PaintStock(CDC *pDC)
{
	#ifdef _CONSOLE_DEBUG 
				printf("\nSeriesStock::PaintStock()");
#endif
	lineWeight = pCtrl->GetPriceLineThicknessBar();
	// Find series
	CSeries* open = GetSeriesOHLCV("open");	
	//if(NULL == open) return; // RDG 8/2/04
	CSeries* high = GetSeriesOHLCV("high");	
	if(NULL == high || high->data_master.size() == 0) return; // 11/21/05
	CSeries* low = GetSeriesOHLCV("low");	
	if(NULL == low || low->data_master.size() == 0) return; // 11/21/05
	CSeries* close = GetSeriesOHLCV("close");	
	if(NULL == close || close->data_master.size() == 0) return; // 11/21/05
 

	double x1 = 0;
	double x2 = 0;
	double y2 = 0;
	double y1 = 0;
	double y3 = 0;
	int cnt = 0;

	OLE_COLOR ucolor, dcolor, useColor;
	if(upColor != -1)
		ucolor = upColor;
	else
		ucolor = pCtrl->upColor;
	if(downColor != -1)
		dcolor = downColor;
	else
		dcolor = pCtrl->downColor;

	bool customOwner = pCtrl->CompareNoCase(pCtrl->barColorName, szName);
	CPen* penCustom = new CPen(lineStyle, lineWeight, lineColor);
	CPen* penUp = new CPen(lineStyle, lineWeight, ucolor);
	if(ucolor==pCtrl->backGradientTop || ucolor==pCtrl->backGradientBottom)penUp = new CPen(lineStyle, lineWeight, RGB(128,128,128));
	CPen* penDown = new CPen(lineStyle, lineWeight, dcolor);
	if(dcolor==pCtrl->backGradientTop || dcolor==pCtrl->backGradientBottom)penUp = new CPen(lineStyle, lineWeight, RGB(128,128,128));
	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);	
	CPen* pOldPen = NULL;
	pOldPen = pDC->SelectObject(pen);
	
	x1 = ownerPanel->GetX(cnt);
	x2 = ownerPanel->GetX(cnt+1);
	//Just draw line path if overview:
	if(x2-x1<0.8){
		std::vector<CPointF> pointsUp,pointsDown;
		int trend = 0; //indicate if serie is more positive or negative		
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
			if(close->data_slave[n].value-open->data_slave[n].value>=0)trend++;
			else trend--;
		}
		if(useColor == pCtrl->backGradientTop) useColor = RGB(128,128,128);
		else useColor = pCtrl->downColor;
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){

			++cnt;
			x1 = ownerPanel->GetX(cnt);
			x2 = x1;

			// if the bar is populated...
			if ((open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE))
			{
				// Getting the Y coordinates...
				y1 = GetY(high->data_slave[n].value);
				y2 = GetY(low->data_slave[n].value);
						
				if(trend>=0){
					pointsUp.push_back(CPointF(x1,y1));
					pointsUp.push_back(CPointF(x2,y2));
					if(close->data_slave[n].value-open->data_slave[n].value<0){
						pointsDown.push_back(CPointF(x1,y1));
						pointsDown.push_back(CPointF(x2,y2));
					}
				}
				else {
					pointsDown.push_back(CPointF(x1,y1));
					pointsDown.push_back(CPointF(x2,y2));
					if(close->data_slave[n].value-open->data_slave[n].value>=0){
						pointsUp.push_back(CPointF(x1,y1));
						pointsUp.push_back(CPointF(x2,y2));
					}
				}
				
				//Draw now if there's a gap:
				if((n != pCtrl->endIndex-1) && (GetY(high->data_slave[n].value)-GetY(low->data_slave[n+1].value)>5 || GetY(high->data_slave[n+1].value)-GetY(low->data_slave[n].value)>5)){
					if(trend>=0){
						pCtrl->pdcHandler->DrawPath(pointsUp,pointsUp.size(),lineWeight,lineStyle,pCtrl->upColor,pCtrl->upColor, pDC, true);
						pCtrl->pdcHandler->DrawSemiPath(pointsDown,pointsDown.size(),lineWeight,lineStyle,useColor,useColor, pDC, true);
					}
					else{
						pCtrl->pdcHandler->DrawPath(pointsDown,pointsDown.size(),lineWeight,lineStyle,useColor,useColor, pDC, true);
						pCtrl->pdcHandler->DrawSemiPath(pointsUp,pointsUp.size(),lineWeight,lineStyle,pCtrl->upColor,pCtrl->upColor, pDC, true);
					}
					pointsUp.clear();
					pointsDown.clear();
				}
			}
		}	
		if(trend>=0){
			pCtrl->pdcHandler->DrawPath(pointsUp,pointsUp.size(),lineWeight,lineStyle,pCtrl->upColor,pCtrl->upColor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsDown,pointsDown.size(),lineWeight,lineStyle,useColor,useColor, pDC, true);
		}
		else{
			pCtrl->pdcHandler->DrawPath(pointsDown,pointsDown.size(),lineWeight,lineStyle,useColor,useColor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsUp,pointsUp.size(),lineWeight,lineStyle,pCtrl->upColor,pCtrl->upColor, pDC, true);
		}
	}
	//Draw complete candles:
	else{
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
			++cnt;

			if(high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE){
			
				pDC->SelectObject(pOldPen);

				if(pCtrl->barColors[n] != -1 && customOwner){ // Custom bar color				
					penCustom->DeleteObject();
					if(NULL != penCustom) delete penCustom;
					penCustom = new CPen(lineStyle, lineWeight, pCtrl->barColors[n]);
					pDC->SelectObject(pOldPen);
					pOldPen = pDC->SelectObject(penCustom);				
				}
				else if(open != NULL && seriesType != OBJECT_SERIES_STOCK_HLC){
					if(open->data_slave[n].value > close->data_slave[n].value){ // Down
						pDC->SelectObject(pOldPen);
						pOldPen = pDC->SelectObject(penDown);
						useColor=dcolor;
					}
					else if(open->data_slave[n].value <= close->data_slave[n].value){ // Up
						pDC->SelectObject(pOldPen);
						pOldPen = pDC->SelectObject(penUp);
						useColor=ucolor;
					}
				}
				else{
					pDC->SelectObject(pOldPen);
					pOldPen = pDC->SelectObject(pen); // No change
				}
			

				// First, paint bar
				x1 = ownerPanel->GetX(cnt);
				x2 = x1;
				y1 = GetY(high->data_slave[n].value);
				y2 = GetY(low->data_slave[n].value);
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)			
				if(high->data_slave[n].value != low->data_slave[n].value){ // BJ 8/4/04
					//pDC->MoveTo((int)x1,(int)y1);
					//pDC->LineTo((int)x2,(int)y2);
					//Bar body (up or down):
					pCtrl->pdcHandler->DrawLine(CPointF(x1,y1), CPointF(x2,y2), lineWeight, lineStyle, useColor, NULL, false);
				}
	//	End Of Revision
				CString name = szName;
				// Next, paint open and close
				if(open != NULL && seriesType != OBJECT_SERIES_STOCK_HLC){ // by TCW 8/1/04
					x2 = x1 - nSpace;
					y1 = GetY(open->data_slave[n].value);
					y2 = y1;		
		//	Revision 6/10/2004 made by Katchei
		//	Addition of type cast (int)
					//pDC->MoveTo((int)x1,(int)y1);
					//pDC->LineTo((int)x2,(int)y2);
					//Bar open-tab (down or up):
					pCtrl->pdcHandler->DrawLine(CPointF(x1,y1), CPointF(x2,y2), lineWeight, lineStyle, useColor, NULL, false);
		//	End Of Revision
				}
			
				x2 = x1 + nSpace;
				y1 = GetY(close->data_slave[n].value);
				y2 = y1;
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
				//pDC->MoveTo((int)x1,(int)y1);
				//pDC->LineTo((int)x2,(int)y2);
				//Bar close-tab (down or up):
				pCtrl->pdcHandler->DrawLine(CPointF(x1,y1), CPointF(x2,y2), lineWeight, lineStyle, useColor, NULL, false);
	//	End Of Revision
			}
		}
	}

	pDC->SelectObject(pOldPen);

	penCustom->DeleteObject();
	penUp->DeleteObject();
	penDown->DeleteObject();
	pen->DeleteObject();	

	if (NULL != pen) delete pen;
	if (NULL != penUp) delete penUp;
	if (NULL != penDown) delete penDown;
	if (NULL != penCustom) delete penCustom;

}

void CSeriesStock::PaintCandles(CDC *pDC)
{
	int t=clock();
	int delta;
	
#ifdef _CONSOLE_DEBUG
	 //printf("\n\nBEGIN PaintCandles() Time=%d",t);
#endif
	// Find series
	CSeries* open = GetSeriesOHLCV("open");	
	if(NULL == open) return;
	CSeries* high = GetSeriesOHLCV("high");	
	if(NULL == high) return;
	CSeries* low = GetSeriesOHLCV("low");	
	if(NULL == low) return;
	CSeries* close = GetSeriesOHLCV("close");	
	if(NULL == close) return;


	int x1 = 0;
	int x2 = 0;
	double y2 = 0;
	double y1 = 0;
	double y3 = 0;
	int cnt = 0;
	
	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	nSpace = ceil(nSpace * 0.75); // leave 25% space between candles

	ExcludeRects(pDC);

	

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	if(nSpace > (int)pCtrl->barSpacing)
		pCtrl->barSpacing = nSpace;
//	End Of Revisions

	OLE_COLOR ucolor, dcolor, wickucolor, wickdcolor, wickcolor, useColor;
	wickucolor=lineColor;
	wickdcolor=lineColor;
	if(pCtrl->wickUpColor != -1)
		wickucolor = pCtrl->wickUpColor;
	if(pCtrl->wickDownColor != -1)
		wickdcolor = pCtrl->wickDownColor;


	if(upColor != -1)
		ucolor = upColor;
	else
		ucolor = pCtrl->upColor;
	if(downColor != -1)
		dcolor = downColor;
	else
		dcolor = pCtrl->downColor;

	// added GR for drawing up & down wicks separate colors
	CPen* pen2 = new CPen(lineStyle, lineWeight, ucolor);
	CPen* pen3 = new CPen(lineStyle, lineWeight, dcolor);

	long size = open->data_slave.size();
	if(high->data_slave.size() != size ||
		low->data_slave.size() != size ||			
		high->data_slave.size() != size || size == 0){
		return;
	}

	bool customOwner = pCtrl->CompareNoCase(pCtrl->barColorName, szName);
	CBrush* brCustom = new CBrush(lineColor);
	CBrush* downBr = new CBrush(dcolor);
	CBrush* upBr = new CBrush(ucolor);
	int wick = pCtrl->barWidth;	
	double halfWick = (wick / 2);	
	//if(halfWick < 1) halfWick = 0;
	
	double x1Temp, x2Temp;
	int widthCandle;
	if ((pCtrl->endIndex - pCtrl->startIndex) >= 1) {
	  x1Temp = ownerPanel->GetX(0);
	  x2Temp = ownerPanel->GetX(1);
	  widthCandle = (x2Temp - x1Temp)*0.75;
	}
	else {
	  widthCandle = (int)nSpace;
	}
	
	// Make sure the wick is centered
	if (open->IsOdd(wick) != open->IsOdd((int)nSpace) || (!open->IsOdd((int)nSpace) && wick == 1)){
		if(nSpace > 1) nSpace -= 1;
	}
	
	//Use same GDI+ objects:
	/*IntPtr hdc = (IntPtr)pCtrl->m_memHDC;
	Graphics^ gDC = Graphics::FromHdc(hdc );
	gDC->SmoothingMode = System::Drawing::Drawing2D::SmoothingMode::AntiAlias;
	gDC->CompositingQuality = System::Drawing::Drawing2D::CompositingQuality::HighQuality;
	gDC->InterpolationMode = System::Drawing::Drawing2D::InterpolationMode::High;
	Pen^ blackPen = gcnew Pen( ColorTranslator::FromOle(lineColor),lineWeight);*/

	// beginning of loop for all bars
	x1 = ownerPanel->GetX(cnt);
	x2 = ownerPanel->GetX(cnt+1);

	//Just draw line path if overview:
	if(x2-x1<1.5){

#ifdef _CONSOLE_DEBUG
		printf("\nOverview line Candle width=%d x1=%d x2=%d", widthCandle, (x1 - widthCandle / 2), (x1 + widthCandle / 2));
#endif
		std::vector<CPointF> pointsUp,pointsDown;
		int trend = 0; //indicate if serie is more positive or negative		
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
			if (close->data_slave[n].value - open->data_slave[n].value >= 0){
				trend++;
			}
			else {
				trend--;
			}
		}
		wickcolor = pCtrl->upColor;
		if (wickcolor == pCtrl->backGradientTop) wickcolor = RGB(128, 128, 128);
	
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){

			++cnt;
			x1 = ownerPanel->GetX(cnt);
			x2 = x1;

			// if the bar is populated...
			if ((open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE))
			{
				// Getting the Y coordinates...
				y1 = GetY(high->data_slave[n].value);
				y2 = GetY(low->data_slave[n].value);
						
				if(trend>=0){
					pointsUp.push_back(CPointF(x1,y1));
					pointsUp.push_back(CPointF(x2,y2));
					if(close->data_slave[n].value-open->data_slave[n].value<0){
						pointsDown.push_back(CPointF(x1,y1));
						pointsDown.push_back(CPointF(x2,y2));
					}
				}
				else {
					pointsDown.push_back(CPointF(x1,y1));
					pointsDown.push_back(CPointF(x2,y2));
					if(close->data_slave[n].value-open->data_slave[n].value>=0){
						pointsUp.push_back(CPointF(x1,y1));
						pointsUp.push_back(CPointF(x2,y2));
					}
				}
				
				//Draw now if there's a gap:
				if((n != pCtrl->endIndex-1) && (GetY(high->data_slave[n].value)-GetY(low->data_slave[n+1].value)>5 || GetY(high->data_slave[n+1].value)-GetY(low->data_slave[n].value)>5)){
					if(trend>=0){
						pCtrl->pdcHandler->DrawPath(pointsUp, pointsUp.size(), lineWeight, lineStyle, wickcolor, wickcolor, pDC, true);
						pCtrl->pdcHandler->DrawSemiPath(pointsDown, pointsDown.size(), lineWeight, lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
					}
					else{
						pCtrl->pdcHandler->DrawPath(pointsDown, pointsDown.size(), lineWeight, lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
						pCtrl->pdcHandler->DrawSemiPath(pointsUp, pointsUp.size(), lineWeight, lineStyle, wickcolor, wickcolor, pDC, true);
					}
					pointsUp.clear();
					pointsDown.clear();
				}
			}
		}	

		if(trend>=0){
			pCtrl->pdcHandler->DrawPath(pointsUp, pointsUp.size(), lineWeight, lineStyle, wickcolor, wickcolor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsDown, pointsDown.size(), lineWeight, lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
		}
		else{
			pCtrl->pdcHandler->DrawPath(pointsDown, pointsDown.size(), lineWeight, lineStyle, pCtrl->downColor, pCtrl->downColor, pDC, true);
			pCtrl->pdcHandler->DrawSemiPath(pointsUp, pointsUp.size(), lineWeight, lineStyle, wickcolor, wickcolor, pDC, true);
		}
	}
	//Draw complete candles:
	else{
		for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){

			++cnt;
			x1 = (int)ownerPanel->GetX(cnt);
			x2 = x1; // Moved from below the NULL_VALUE check line - 9/1/04 RDG		
		
	#ifdef _CONSOLE_DEBUG
		printf("\nCompleteCandle width=%d x1=%d x2=%d",widthCandle,(x1 - widthCandle/2),(x1 + widthCandle/2));
	#endif
			// if the bar is populated...
			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE){


				// Make sure the wick is centered
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
				if (IsOdd(widthCandle)){
	//	End Of Revision
					if(widthCandle > 1) widthCandle -= 1;
					else widthCandle+=1;
				}

				double d1 = high->data_slave[n].value;
				double d2 = low->data_slave[n].value;
				double d3 = close->data_slave[n].value;	 

				// First, paint wick
				y1 = GetY(high->data_slave[n].value);
				y2 = GetY(low->data_slave[n].value);
				y3 = GetY(close->data_slave[n].value);  // added GR. 

				//if(y2 == y1) y1 = y2 - 2;   // not sure what this is for...
											// if y2 == y1, why draw a wick at all?
				
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)

			//	if(high->data_slave[n].value != low->data_slave[n].value){ //JRG 8/4/04
			//		pDC->MoveTo((int)x1,(int)y1);
			//		pDC->LineTo((int)x2,(int)y2);
			//	}

				if((x2 + widthCandle/2)>(x1 - widthCandle/2)+4 /*|| (x2 + widthCandle/2)<(x1 - widthCandle/2)+2*/){
						//wickcolor = lineColor;
						wickcolor = (close->data_slave[n].value >= open->data_slave[n].value) ? wickucolor : wickdcolor;
				}
				else wickcolor = (close->data_slave[n].value >= open->data_slave[n].value) ? ucolor : dcolor;
				if(wickcolor == pCtrl->backGradientTop) wickcolor = RGB(128,128,128);

				//Just draw wick with candle total height:
				pCtrl->pdcHandler->DrawLine(CPointF((double)x1,y1), CPointF((double)x2,y2), lineWeight, lineStyle, wickcolor, NULL, true);


			

	// End of Revision

				// Next, paint candle
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
				pDC->MoveTo((int)(x1 - nSpace),(int)y1);
	//	End Of Revision
				if(pCtrl->barColors[n] != -1 && customOwner){ // Custom bar color				
					brCustom->DeleteObject();
					if(NULL != brCustom) delete brCustom;
					brCustom = new CBrush(pCtrl->barColors[n]);
					y1 = GetY(open->data_slave[n].value);
					y2 = GetY(close->data_slave[n].value);
					if(y1 + 3 > y2) y2 += 2;
					
		//	Revision 6/10/2004 made by Katchei
		//	Addition of type cast (int)
					CRectF candle((double)(x1 - widthCandle/2), (y1), (double)(x2 + widthCandle/2),(y2));
		//	End Of Revision
					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, pCtrl->barColors[n]);
					}
					else{
						//pDC->Rectangle(candle);
						//pDC->FillRect(candle, brCustom);
						pCtrl->pdcHandler->FillRectangle(candle, pCtrl->barColors[n], NULL);
					}
				
					if(open->data_slave[n].value < close->data_slave[n].value){ // Down
						if(pCtrl->candleDownOutlineColor != -1){ // Draw a box (hollow candle)
							//pDC->Draw3dRect(candle, pCtrl->candleUpOutlineColor, pCtrl->candleUpOutlineColor);	
						}
					}
					else if(open->data_slave[n].value > close->data_slave[n].value){ // Up
						if(pCtrl->candleUpOutlineColor != -1){ // Draw a box (hollow candle)
							//pDC->Draw3dRect(candle, pCtrl->candleDownOutlineColor, pCtrl->candleDownOutlineColor);
						}
					}				
				}
				else if(open->data_slave[n].value > close->data_slave[n].value){ // Down
					y1 = GetY(open->data_slave[n].value);
					y2 = GetY(close->data_slave[n].value);
					if(y1 == y2) y2 += 1;
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)

					//CRect candle ((int)(x1 - nSpace - halfWick),(int) y1, (int)(x2 + nSpace + halfWick),(int)y2);
					CRectF candle((double)(x1 - widthCandle/2), (y1), (double)(x2 + widthCandle/2),(y2));
	//	End Of Revision
					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, dcolor);
					}
					else{
						//pDC->Rectangle(candle);
						//pDC->FillRect(candle,downBr);
						pCtrl->pdcHandler->FillRectangle(candle, dcolor, NULL);
					}
					if(pCtrl->candleDownOutlineColor != -1){ // Draw a box (hollow candle)
						if(candle.right>(candle.left+4) || pCtrl->downColor==pCtrl->backGradientTop || pCtrl->downColor==pCtrl->backGradientBottom){
						//	pDC->Draw3dRect(candle, pCtrl->candleDownOutlineColor, pCtrl->candleDownOutlineColor);
							pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, pCtrl->candleDownOutlineColor, NULL);
						}
						//else pDC->Draw3dRect(candle, pCtrl->downColor, pCtrl->downColor);
						else pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, pCtrl->downColor, NULL);


					
					}
				}
				else /*if(open->data_slave[n].value < close->data_slave[n].value)*/{ // Up
					y1 = GetY(close->data_slave[n].value);
					y2 = GetY(open->data_slave[n].value);
					if(y1 == y2) y2 +=  1;
					//if(y1 + 3 > y2) y2 += 2;
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
				
					//CRect candle((int)(x1 - nSpace - halfWick), (int)y1, (int)(x2 + nSpace + halfWick),(int)y2);
					CRectF candle((double)(x1 - widthCandle/2), (y1), (double)(x2 + widthCandle/2),(y2));
	//	End Of Revision 
					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, ucolor);
					}
					else{
						//pDC->FillRect(candle,upBr);
						pCtrl->pdcHandler->FillRectangle(candle, ucolor, NULL);
					}
					if(pCtrl->candleUpOutlineColor != -1){ // Draw a box (hollow candle)
						if(candle.right>(candle.left+4)){
							pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, pCtrl->candleUpOutlineColor, NULL);
						}
						else if(pCtrl->downColor == pCtrl->candleUpOutlineColor) pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, RGB(128,128,128), NULL);
						else {
							pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, pCtrl->upColor, NULL);
						}
					}
				}
				/*else{ // No change, flat bar
					y1 = GetY(close->data_slave[n].value);
					y2 = GetY(open->data_slave[n].value);
					if(y2 == y1) y1 = y2 - 1;
	//	Revision 6/10/2004 made by Katchei
	//	Addition of type cast (int)
				
					//CRect candle((int)(x1 - nSpace - halfWick), (int)y1, (int)(x2 + nSpace + halfWick),(int) y2);
					CRectF candle((double)(x1 - widthCandle/2), (y1), (double)(x2 + widthCandle/2),(y2));
	//	End Of Revision
					if(pCtrl->threeDStyle){
						//DrawGradientCandle(pDC, candle, lineColor);
					}
					else{
						//pDC->Draw3dRect(candle,lineColor, lineColor);
						pCtrl->pdcHandler->DrawRectangle(candle, lineWeight, lineStyle, lineColor, NULL);
					}
				}*/
			}
		}
	}

	IncludeRects(pDC);
	pDC->SelectObject(pOldPen);
	
	//delete blackPen;
	//delete gDC;

	brCustom->DeleteObject();
	downBr->DeleteObject();
	upBr->DeleteObject();
	pen->DeleteObject();
	pen2->DeleteObject();
	pen3->DeleteObject();

	if(NULL != brCustom) delete brCustom;
	if(NULL != downBr) delete downBr;
	if(NULL != upBr) delete upBr;
	if(NULL != pen) delete pen;
	if(NULL != pen2) delete pen2;
	if(NULL != pen3) delete pen3;

	t=clock();
#ifdef _CONSOLE_DEBUG
	 //printf("\n\END PaintCandles() Time=%d",t);
#endif

}

void CSeriesStock::PaintLines(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG 
				printf("\nSeriesStock::PaintLines()");
#endif
	lineWeight = pCtrl->GetPriceLineThickness();
	// Find series
	CSeries* open = GetSeriesOHLCV("open");	
	if(NULL == open) return;
	CSeries* high = GetSeriesOHLCV("high");	
	if(NULL == high) return;
	CSeries* low = GetSeriesOHLCV("low");	
	if(NULL == low) return;
	CSeries* close = GetSeriesOHLCV("close");	
	if(NULL == close) return;


	double x1 = 0;
	double x2 = 0;
	double y2 = 0;
	double y1 = 0;
	int cnt = 0;
	
	CPen* pen = new CPen(lineStyle, 1, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	ExcludeRects(pDC);

	OLE_COLOR ucolor, dcolor;
	if(upColor != -1)
		ucolor = upColor;
	else
		ucolor = pCtrl->upColor;
	if(downColor != -1)
		dcolor = downColor;
	else
		dcolor = pCtrl->downColor;

	// Added GR for drawing up & down wicks separate colors
	CPen* pen2 = new CPen(lineStyle, lineWeight, ucolor);
	CPen* pen3 = new CPen(lineStyle, lineWeight, dcolor);

	long size = open->data_slave.size();
	if(high->data_slave.size() != size ||
		low->data_slave.size() != size ||			
		high->data_slave.size() != size || size == 0){
		return;
	}
	std::vector<CPointF> pointsF;
	// Beginning of loop for all bars:
	for(int n = pCtrl->startIndex; n != pCtrl->endIndex-1; ++n){

		++cnt;
		x1 = ownerPanel->GetX(cnt);
		x2 = ownerPanel->GetX(cnt+1);

		// if the bar is populated...
		if ((open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
		   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE) 
		   &&
		   (open->data_slave[n+1].value != NULL_VALUE && high->data_slave[n+1].value != NULL_VALUE &&
		   low->data_slave[n+1].value != NULL_VALUE && close->data_slave[n+1].value != NULL_VALUE))
		{
			// Getting the Y coordinates...
			y1 = GetY(close->data_slave[n].value);
			y2 = GetY(close->data_slave[n+1].value);
						
			pointsF.push_back(CPointF(x1,y1));
			if(n==pCtrl->endIndex-2)pointsF.push_back(CPointF(x2,y2));
		}
	}	
	if(pCtrl->GetPriceLineMono())pCtrl->pdcHandler->DrawPath(pointsF,pointsF.size(),lineWeight,lineStyle,lineColor,lineColor, pDC, true);
	else pCtrl->pdcHandler->DrawPath(pointsF,pointsF.size(),lineWeight,lineStyle,lineColor,lineColor, pDC, true);
	IncludeRects(pDC);
	pDC->SelectObject(pOldPen);

	pen->DeleteObject();
	pen2->DeleteObject();
	pen3->DeleteObject();

	if(NULL != pen) delete pen;
	if(NULL != pen2) delete pen2;
	if(NULL != pen3) delete pen3;

}

void CSeriesStock::OnMouseMove(CPoint point)
{
	if(data_slave.size() < 1) return;	

	if(pCtrl->resizing || pCtrl->drawing || pCtrl->movingObject) return;
	
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
//	End Of Revision
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	if(index < 0 || index > (int)data_slave.size() - 1)
//	End Of Revision
		return;	

	double y = GetY(data_slave[index].value);
	double errorPixels = 10;

	if(point.y - errorPixels < y && point.y + errorPixels > y){
		if(pCtrl->m_mouseState == MOUSE_NORMAL){
			pCtrl->FireOnItemMouseMove(seriesType, szName);
		}
	}
	
	// Change the mouse cursor if selected
	if(this->selected){
		if(point.y - errorPixels < y && point.y + errorPixels > y){
			if(pCtrl->m_mouseState == MOUSE_NORMAL){
				pCtrl->m_mouseState = MOUSE_OPEN_HAND;
				pCtrl->m_Cursor = IDC_OPEN_HAND;
			}
		}
		else{
			if(pCtrl->m_mouseState == MOUSE_OPEN_HAND){
				pCtrl->m_mouseState = MOUSE_NORMAL;		
			}
		}
	}
	

}

void CSeriesStock::OnLButtonDown(CPoint point)
{
  return;

#ifdef FROEDE_MARK

	// This behavior was inhibited, because we don´t want to allow the main series
	// to be moved to some place else. 08/09/2011

	if(data_slave.size() < 1) return;
			
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size()-1) return;
//	End Of Revision
	double y = GetY(data_slave[index].value);
	double y1 = y;
	double y2 = y;

	double errorPixels = 10;


	// 7/20/2005 RG
	// If this series belongs to an ohlc group, allow clicking between the h and l	
	CString group = szName;			
	int found = group.Find(".",0);
	if(found > 0) group = group.Left(found);
	group.MakeUpper();
	if(szName.Find(".close") == -1) return;
	if(group != ""){		
		CSeries* pHigh = GetSeries(group + ".high");
		CSeries* pLow = GetSeries(group + ".low");
		if(pHigh != NULL && pLow != NULL)
		{
			y1 = GetY(pHigh->GetValue(index));
			y2 = GetY(pLow->GetValue(index));
		}
	}

	// 7/20/2005 RG
	//if(point.y - errorPixels < y && point.y + errorPixels > y){
	if(y1 - errorPixels < point.y && y2 + errorPixels > point.y){
	
		pCtrl->dragging = true;		

		// If selected, show grab hand
		if(this->selected){			
			pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
			pCtrl->m_Cursor = IDC_CLOSED_HAND;	
			SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
			// This series is being dragged somewhere,
			// so allert the ctrl about this.			
			int found = szName.Find(".",0);
			if(found > 0){
				pCtrl->swapSeries.clear();
				pCtrl->swapSeries.push_back(szName.Left(found) + ".open");
				pCtrl->swapSeries.push_back(szName.Left(found) + ".high");
				pCtrl->swapSeries.push_back(szName.Left(found) + ".low");
				pCtrl->swapSeries.push_back(szName.Left(found) + ".close");
			}

		}

	}

#endif

}

void CSeriesStock::OnLButtonUp(CPoint point)
{

	if(!selectable || !seriesVisible) return;
	if(data_slave.size() < 1) return;

	if(selected) pCtrl->FireOnItemLeftClick(seriesType, szName);	

	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size()-1) return;

	double y = GetY(data_slave[index].value);
	double y1 = y;
	double y2 = y;
	double errorPixels = 10;

	bool repaint = false;
	
	// 7/20/2005 RG
	// If this series belongs to an ohlc group, allow clicking between the h and l	
	CString group = szName;			
	int found = group.Find(".",0);
	if(found > 0) group = group.Left(found);
	group.MakeUpper();
	if(szName.Find(".close") == -1) return;
	if(group != ""){	
		CSeries* pHigh = GetSeries(group + ".high");
		CSeries* pLow = GetSeries(group + ".low");
		if(pHigh != NULL && pLow != NULL)
		{
			y1 = GetY(pHigh->GetValue(index));
			y2 = GetY(pLow->GetValue(index));
		}
	}

	// 7/20/2005 RG
	//if(point.y - errorPixels < y && point.y + errorPixels > y){
	if(y1 - errorPixels < point.y && y2 + errorPixels > point.y){
		/* Dont select stock serie!
		// Unselect all other series.
		pCtrl->UnSelectAll();
		
		// Select this series/object only		
		this->selected = true;
		pCtrl->OnSelectSeries(szName);
		// Since we are now the selected series
		// take control of the panel's y-grid.
		// Only do this if there are other symbols
		// shared in the same panel 9/19/04 TW
		CString right = szName;
		right.MakeLower();
		int found = right.Find(".", 0);
		if(found != -1) right = right.Mid(found);
		for(int n = 0; n != ownerPanel->series.size(); ++n ){
			if(ownerPanel->series[n] != this){
				CString right2 = ownerPanel->series[n]->szName;
				right2.MakeLower();
				int found = right2.Find(".", 0);
				if(found != -1) right2 = right2.Mid(found);
				if(right2 == right){
					CString name = ownerPanel->series[n]->szName;
					name.MakeLower();
					int close = name.Find("." + right, 0);
					if(close != -1){
						ownerPanel->pYScaleOwner = this;
						break;
					}
				}
			}
		}*/
		repaint = true;
	}
	else{
		if(pCtrl->SelectCount() > 0){			
			this->selected = false;
			if(ownerPanel->pYScaleOwner == this) ownerPanel->pYScaleOwner = NULL;
			repaint = true;						
		}
	}
	if(repaint){
		ownerPanel->Invalidate();
		pCtrl->RePaint();
	}
	
}

// Draws selection boxes around series
void CSeriesStock::OnPaintXOR(CDC* pDC)
{

	if(pCtrl->priceStyle == psPointAndFigure ||
		pCtrl->priceStyle == psRenko ||
		pCtrl->priceStyle == psKagi ) return;


	double x1 = pCtrl->GetSlaveRecordCount() - data_slave.size();
	
	if(!this->selected) return;

	
	double y1 = 0;
	int cnt = 0;

	// Find close series
	CSeries* close = GetSeriesOHLCV("close");	
	if(NULL == close) return;

	double x2 = ownerPanel->GetX((int)x1);
	double y2 = close->GetY((int)data_slave[pCtrl->startIndex].value);

	pDC->SetROP2(R2_NOT);
	
	double space = SEL_SPACE;

	for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		++cnt;
		if(data_slave[n].value != NULL_VALUE){
			x1 = ownerPanel->GetX(cnt) - 3;
			space = x1 - x2;
			if(space >= SEL_SPACE){
				x2 = x1 + 8;
				y1 = close->GetY(close->data_slave[n].value) - 3;
				y2 = y1 + 8;
				if(y2 > ownerPanel->y2) y2 = ownerPanel->y2;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				pDC->MoveTo((int)x1,(int)y1);
				pDC->Rectangle((int)x1,(int)y1,(int)x2,(int)y2);
//	End Of Revisions
				space = 0;
			}
		}
	}

	pDC->SetROP2(R2_COPYPEN);

}

void CSeriesStock::OnRButtonUp(CPoint point)
{	
	if(selected) pCtrl->FireOnItemRightClick(seriesType, szName);	
}

void CSeriesStock::OnDoubleClick(CPoint point)
{	
	if(selected) pCtrl->FireOnItemDoubleClick(seriesType, szName);
}


void CSeriesStock::PaintDarvasBoxes(CDC *pDC)
{

	CString type = szName;

	pCtrl->m_psValues1.clear();
	pCtrl->m_psValues2.clear();
	pCtrl->m_psValues3.clear();

	// Find series
	CSeries* close = GetSeriesOHLCV("close");	
	if(NULL == close) return;
	// Only paint once
	if(close != this) return;
	CSeries* low = GetSeriesOHLCV("low");	
	if(NULL == low) return;
	CSeries* high = GetSeriesOHLCV("high");	
	if(NULL == high) return;

	/*
	Initial box top is the high of day 1.

	The first step is to find a new high that is higher than the high of day 1.
	The high can be found anytime - even after 5 days.
	But once the bottom has been found, the box is complete.

	To find the bottom, the low must be after day 2 of the day the last box
	top was found and must be lower than the low of original day 1 low.

	The bottom is always found last and a new high may not be found once 
	the bottom is locked in - the Darvas box is complete then.

	A new box is started when the price breaks out of top or bottom, 

	The bottom stop loss box is drawn as a percentage of the last price.
	*/

	// Draw the Darvas boxes
	ExcludeRects(pDC);
	CBrush* brDarvas = new CBrush(RGB(0,0,255));
	CBrush* brDarvasIncomplete = new CBrush(RGB(0,0,255));//(RGB(100,115,255));
	CBrush* brStop = new CBrush(RGB(255,75,75));	
	double boxTop = 0;
	double boxBottom = 0;
	int topFound = 0;
	int bottomFound = 0;
	int cnt = 0;
	int day = 0;
	int start = 0;
	int startX = 0;
	int state = 0;
	for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		
		++cnt;

		if(close->data_slave[n].value != NULL_VALUE && 
		   high->data_slave[n].value != NULL_VALUE &&
		   low->data_slave[n].value != NULL_VALUE){

			if(n == pCtrl->endIndex - 1){
				
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				int x1 = (int)ownerPanel->GetX(start);
				int x2 = (int)ownerPanel->GetX(cnt);
				int y1 = (int)GetY(boxTop);
				int y2 = (int)GetY(boxBottom);
//	End Of Revisions

				if(state == 5){ // draw the last COMPLETED box
					// Darvas box
					// Solid Darvas box
					// pDC->FillRect(CRect(x1, y1, x2 + 2, y2),brDarvas);
					// Translucent Darvas box
					pDC->SetROP2(R2_NOT);
					pDC->DrawDragRect(CRect(x1, y1, x2 + 2, y2),CSize(x2-x1,y2-y1),
						  CRect(x1, y1, x2, y2),
						  CSize(0,0),
						  brDarvas);
					pDC->SetROP2(R2_COPYPEN);
					pDC->Draw3dRect(CRect(x1, y1, x2 + 2, y2), RGB(240, 240, 240), RGB(150, 150, 150));
				}
				else if(bottomFound > 0){ // draw the last INCOMPLETED box
					// Translucent Darvas box as incomplete color
					pDC->DrawDragRect(CRect(x1, y1, x2 + 2, y2),CSize(x2-x1,y2-y1),
						  CRect(x1, y1, x2, y2),
						  CSize(0,0),
						  brDarvasIncomplete);
					pDC->Draw3dRect(CRect(x1, y1, x2 + 2, y2), RGB(240, 240, 240), RGB(150, 150, 150));
				}

				// Solid stop loss box
				//pDC->FillRect(CRect(x1, y2, x2 + 2, (y2 + (y2 - y1) * pCtrl->darvasPct)),brStop);
				//pDC->Draw3dRect(CRect(x1, y2, x2 + 2, (y2 + (y2 - y1) * pCtrl->darvasPct)), RGB(240, 240, 240), RGB(150, 150, 150));
				// Gradient stop loss box
				pCtrl->FadeVert(pDC, RGB(255,255,255), 255, CRect((int)x1, (int)y2, (int)(x2 + 2),  ((int)GetY(boxBottom - (boxBottom * pCtrl->darvasPct)))));	

				break;
			}




			//==================================================================
			if(state == 0){ // Start of a new box
				// Save new box top and bottom
				day = 1; // reset day
				startX = n; // box start index
				start = cnt;
				topFound = n;
				bottomFound = 0;
				boxTop = high->data_slave[n].value;
				boxBottom = -1;
				state = 1;
			}

			day += 1;

			if(state == 1){
				if(high->data_slave[n].value > boxTop){
					boxTop = high->data_slave[n].value;
					topFound = n;
					state = 1;
				}
				else{
					state = 2;
				}
			}
			
			else if(state == 2){
				if(high->data_slave[n].value > boxTop){
					boxTop = high->data_slave[n].value;
					topFound = n;
					state = 1;
				}
				else{
					bottomFound = n;
					boxBottom = low->data_slave[n].value;
					state = 3;
				}
			}

			else if(state == 3){
				if(high->data_slave[n].value > boxTop){
					boxTop = high->data_slave[n].value;
					topFound = n;
					state = 1;
				}
				else{				
					if(low->data_slave[n].value < boxBottom){
						boxBottom = low->data_slave[n].value;
						bottomFound = n;
						state = 3;
					}
					else{
						state = 4;
					}
				}
			}

			else if(state == 4){
				if(high->data_slave[n].value > boxTop){
					boxTop = high->data_slave[n].value;
					topFound = n;
					state = 1;
				}
				else{				
					if(low->data_slave[n].value < boxBottom){
						boxBottom = low->data_slave[n].value;
						bottomFound = n;
						state = 3;
					}
					else{
						state = 5; // Darvas box is complete
					}
				}
			}			

			//==================================================================



			// Break out?
			if(state == 5){
				if(low->data_slave[n].value < boxBottom || 
					high->data_slave[n].value > boxTop){					
					// Draw the completed Darvas box and start a new one

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
					int x1 = (int)ownerPanel->GetX((int)start);
					int x2 = (int)ownerPanel->GetX((int)cnt);
					int y1 = (int)GetY(boxTop);
					int y2 = (int)GetY(boxBottom);
//	End Of Revision

					// Solid Darvas box
					// pDC->FillRect(CRect(x1, y1, x2, y2),brDarvas);
					// Translucent Darvas box
					pDC->SetROP2(R2_NOT);
					pDC->DrawDragRect(CRect(x1, y1, x2, y2),CSize(x2-x1,y2-y1),
						  CRect(x1, y1, x2, y2),
						  CSize(0,0),
						  brDarvas);
					pDC->SetROP2(R2_COPYPEN);

					pDC->Draw3dRect(CRect(x1, y1, x2, y2), RGB(240, 240, 240), RGB(150, 150, 150));

					// Solid stop loss box
					//pDC->FillRect(CRect(x1, y2, x2, (y2 + (y2 - y1) * pCtrl->darvasPct)),brStop);
					//pDC->Draw3dRect(CRect(x1, y2, x2, (y2 + (y2 - y1) * pCtrl->darvasPct)), RGB(240, 240, 240), RGB(150, 150, 150));
					// Gradient stop loss box
					pCtrl->FadeVert(pDC, RGB(255,255,255), 255, CRect(x1, y2, x2,(int) (GetY(boxBottom - (boxBottom * pCtrl->darvasPct)))));



					state = 0;
					day = 0;
					cnt--;
					n--;
				}
			}

		}



	}



	
	// 8/13/04 RDG
	

	// 2/17/05 Darvas values may not be returned to the program.
	// For *visual* output *only*.
	// Anything else goes against the license agreement as this competes
	// with our Darvas Box Scanning Software (see license agreement under
	// non-compete section).



	IncludeRects(pDC);
	brDarvasIncomplete->DeleteObject();
	brDarvas->DeleteObject();
	brStop->DeleteObject();

	if(NULL != brDarvasIncomplete) delete brDarvasIncomplete;
	if(NULL != brDarvas) delete brDarvas;
	if(NULL != brStop) delete brStop;	

	

}


// Gradient candle from left to right
void CSeriesStock::DrawGradientCandle(CDC *pDC, CRect boxRect, OLE_COLOR color)
{

	if(boxRect.Width() < 2){
		CBrush* br = new CBrush(color);
		pDC->Rectangle(boxRect);
		pDC->FillRect(boxRect, br);
		br->DeleteObject();
		if(NULL != br ) delete br;
		return;
	}

	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);
	CPen* pOldPen = pDC->SelectObject(pen);

	pCtrl->FadeHorz(pDC, color,RGB(80,80,80), boxRect);
	pDC->MoveTo(boxRect.top, boxRect.left);

	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

	pen = new CPen(lineStyle, lineWeight, color);
	pOldPen = pDC->SelectObject(pen);
	pDC->MoveTo(boxRect.left, boxRect.top);
	pDC->LineTo(boxRect.right, boxRect.top);
	
	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	if(pen != NULL) delete pen;

}

