// PriceStylePointAndFigure.cpp: implementation of the CPriceStylePointAndFigurePointAndFigure class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PriceStylePointAndFigure.h"
#include "StockChartX.h"


#include "stdafx.h"
#include "PriceStyleEquiVolume.h"
#include "StockChartX.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CPriceStylePointAndFigure::CPriceStylePointAndFigure()
{	
	nSavedDC = 0;
	Style = 0;	
	connected = false;
	pSeries = NULL;
}

CPriceStylePointAndFigure::~CPriceStylePointAndFigure()
{
	
}

void CPriceStylePointAndFigure::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

void CPriceStylePointAndFigure::OnPaint(CDC* pDC)
{


	pDC->SetBkMode( TRANSPARENT );
	/*

	pCtrl->priceStyleParams[0] = boxSize
	pCtrl->priceStyleParams[1] = reversalSize

	7 columns

	width = 300

	300 / 7 = 42.85 pixels per column

	*/

	//~ Individual up/down colors on 11/30/04 by Dave Wozniak
	OLE_COLOR ucolor, dcolor;
	if(pSeries->upColor != -1)
		ucolor = pSeries->upColor;
	else
		ucolor = pCtrl->upColor;
	if(pSeries->downColor != -1)
		dcolor = pSeries->downColor;
	else
		dcolor = pCtrl->downColor;
	//~


	// Find series
	CSeries* high = pSeries->GetSeriesOHLCV("high");	
	if(NULL == high) return;
	CSeries* low = pSeries->GetSeriesOHLCV("low");	
	if(NULL == low) return;	

	long width = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;

	double x1 = 0;
	double x2 = 0;
	double y2 = 0;
	double y1 = 0;	
	double x = 0;
	double total = 0;
	double max = pCtrl->GetMax(high->szName);
	double min = pCtrl->GetMin(low->szName, true);
	double boxSize = pCtrl->priceStyleParams[0];
	if(boxSize > 50 || boxSize < 0.00000000000000000000001){		
		boxSize = (max - min) / 25;		

	}
	double reversalSize = pCtrl->priceStyleParams[1];
	if(reversalSize > 50 || reversalSize < 1) reversalSize = 3;	

	pCtrl->priceStyleParams[0] = boxSize;
	pCtrl->priceStyleParams[1] = reversalSize;

	double nHigh = 0, nLow = 0, nLastHigh = 0, nLastLow = 0;
	int column = 0; // X=1 O=2
	int columnHeight = 0;
	int totalColumns = 0;
	int boxes = 0;
	int Xs = 1;
	int Os = 2;
	double colStart = 0;
	CRect frame;

	pCtrl->m_psValues1.clear();
	pCtrl->m_psValues2.clear();
	pCtrl->m_psValues3.clear();

	CPen* penUp = new CPen(pSeries->lineStyle, pSeries->lineWeight, ucolor);
	CPen* penDown = new CPen(pSeries->lineStyle, pSeries->lineWeight, dcolor);
	CPen* pOldPen = NULL;
			
	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex + 1);
	int cnt = 0;

	bool pass = true;

	// Count columns
	int n;
	for(n = pCtrl->startIndex; n != pCtrl->endIndex + 1; ++n){		
		
		if(n < high->data_slave.size() - 1){// break; // Changed from .size() -1 on 7/7/06, changed 2/1/08

		if(high->data_slave[n].value != NULL_VALUE && 
			low->data_slave[n].value != NULL_VALUE){
		
			// Calculate Point and Figure
			nHigh = high->data_slave[n].value;
			nLow = low->data_slave[n].value;
		
	
			if(column == Xs){
				// check high first
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
				boxes = (int)((nHigh - nLastHigh) / boxSize);
//	End Of Revisions
				if(boxes >= boxSize){
					// Add one X box
					columnHeight += 1;
					nLastHigh += boxSize;
					if(nLastHigh > max) max = nLastHigh;
				}
				else{
					// Check for O's reversal
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					boxes = (int)((nLastHigh - nLow) / boxSize);
//	End Of Revisions
					if(boxes >= reversalSize){
						column = Os;
						columnHeight = boxes;
						totalColumns++;
						nLastLow = nLastHigh - (boxes * boxSize);
						if(nLastLow < min && min != 0) min = nLastLow;
					}
				}
			}
			else if(column == Os){
				// check low first
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
				boxes = (int)((nLastLow - nLow) / boxSize);
//	End Of Revision
				if(boxes >= boxSize){
					// Add one O box
					columnHeight += 1;
					nLastLow -= boxSize;
					if(nLastLow < min && min != 0) min = nLastLow;
				}
				else{
					// Check for X's reversal
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					boxes = (int)((nHigh - nLastLow) / boxSize);
//	End Of Revision
					if(boxes >= reversalSize){
						column = Xs;
						columnHeight = boxes;
						totalColumns++;
						nLastHigh = nLastLow + (boxes * boxSize);
						if(nLastHigh > max) max = nLastHigh;
					}
				}
			}


			if(column == 0){ // Prime first column				
				column = Xs;
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
				boxes = (int)floor(((nHigh - (nLow + (boxSize * reversalSize))) / boxSize) + 0.5);
//	End Of Revision
				columnHeight = boxes;
				nLastHigh = nHigh;
				nLastLow = nHigh - (boxes * boxSize);
				totalColumns = 1;				
			}

 
		}
		}
	}	


	pCtrl->xCount = totalColumns;
 
	ExcludeRects(pDC); // added 1/13/08

	// Rescale the y axis
	min = pCtrl->GetMin(low->szName, true);
	pSeries->min = (min - boxSize); // * 0.95; changed 1/13/08

	// Paint columns
	CRect box;
	column = 0;
	int px = 0;
	x = 0;
	if(totalColumns == 0) return;
	int space = ((width - pCtrl->extendedXPixels) / totalColumns);
	totalColumns = 0;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
	
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	px = (int)x;
//	End Of Revision
	
	// Calculate from beginning, but only show between startIndex and endIndex
	int size = high->data_master.size();
	for(n = 0; n != pCtrl->endIndex + 1; ++n){

		if(n < high->data_slave.size()){// break; changed 2/1/08

		if(high->data_master[n].value != NULL_VALUE && 
			low->data_master[n].value != NULL_VALUE){
		
			// Calculate Point and Figure
			nHigh = high->data_master[n].value;
			nLow = low->data_master[n].value;
			

			if(column == Xs){
				// check high first
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
				boxes = (int)((nHigh - nLastHigh) / boxSize);
//	End Of Revision
				if(boxes >= boxSize){
					// Add one X box
					columnHeight += 1;
					nLastHigh += boxSize;
				}
				else{
					// Check for O's reversal
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					boxes = (int)((nLastHigh - nLow) / boxSize);
//	End Of Revision
					if(boxes >= reversalSize){

						// Paint the previous X column
						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
						double y = nLastHigh;						
							if(columnHeight > 0){
								for(int col = 0; col != columnHeight; ++col){
									y1 = pSeries->GetY(y);
									y -= boxSize;
									y2 = pSeries->GetY(y);
									pOldPen = pDC->SelectObject(penUp);
									x1 = x; x2 = x + space;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
									pDC->MoveTo((int)x1, (int)y1);
									pDC->LineTo((int)x2, (int)y2);
									pDC->MoveTo((int)x2, (int)y1);
									pDC->LineTo((int)x1, (int)y2);
//	End Of Revision
									pDC->SelectObject(pOldPen);				
								}
							}
						}

						// Create new O column
						column = Os;
						columnHeight = boxes;

						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
							totalColumns++;
							x += space;
						}

						nLastLow = nLastHigh - (boxes * boxSize);
					}
				}
			}
			else if(column == Os){
				// check low first
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				boxes = (int)((nLastLow - nLow) / boxSize);
//	End Of Revision
				if(boxes >= boxSize){
					// Add one O box
					columnHeight += 1;
					nLastLow -= boxSize;
				}
				else{
					// Check for X's reversal
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
					boxes = (int)((nHigh - nLastLow) / boxSize);
//	End Of Revision
					if(boxes >= reversalSize){

						// Paint the previous O's column
						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
							CBrush* brBack = new CBrush(pCtrl->backColor);
							CBrush* oldBr = pDC->SelectObject(brBack);
							double y = nLastLow - boxSize;
							if(columnHeight > 0){
								for(int col = 0; col != columnHeight; ++ col){
									y2 = pSeries->GetY(y);
									y += boxSize;
									y1 = pSeries->GetY(y);
									pOldPen = pDC->SelectObject(penDown);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
									pDC->Ellipse((int)x, (int)y1, (int)(x + space), (int)y2);
//	End Of Revision
									pDC->SelectObject(pOldPen);					
								}
							}
							pDC->SelectObject(oldBr);
							brBack->DeleteObject();
							if(brBack != NULL) delete brBack;
						}

						// Create new X column
						column = Xs;
						columnHeight = boxes;

						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
							totalColumns++;
							x += space;
						}

						nLastHigh = nLastLow + (boxes * boxSize);
					}
				}
			}			


			if(column == 0){ // Prime first column				
				column = Xs;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				boxes = (int)floor(((nHigh - (nLow + (boxSize * reversalSize))) / boxSize) + 0.5);
//	End Of Revision
				columnHeight = boxes;
				nLastHigh = nHigh;
				nLastLow = nHigh - (boxes * boxSize);

				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
					totalColumns = 1;
				}

				if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
					px = (int)x;
//	End Of Revision
			}

			
			// Record the x value
			if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
				pCtrl->xMap[cnt] = x + (space / 2);
				cnt++;
			}

	 
			if(n >= pCtrl->startIndex && n <= pCtrl->endIndex)
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				px = (int)x;
//	End Of Revision

		}
		}
		

		// 8/13/04 RDG		
		SeriesPoint sp;
		if(n < high->data_master.size())
			sp.jdate = high->data_master[n].jdate; //2/1/08

		int d = -1; if(column == 1) d = 1;
		sp.value = d;
		pCtrl->m_psValues3.push_back(sp);
		sp.value = columnHeight;
		pCtrl->m_psValues1.push_back(sp);
		
		// Once the direction changes, we need to
		// go backwards until the previous change
		// and fill in the values.
		if(pCtrl->m_psValues3.size() > 1){			
			for(int prev = pCtrl->m_psValues3.size() - 1; prev != -1; --prev){
				if(pCtrl->m_psValues3[prev].value != column) break;
				pCtrl->m_psValues1[prev].value = columnHeight;
			}
		}



	}
	
	IncludeRects(pDC); // added 1/13/08

	pDC->SetBkMode( OPAQUE );
	
	penUp->DeleteObject();
	penDown->DeleteObject();

	if(penUp != NULL) delete penUp;	
	if(penDown != NULL) delete penDown;

	CPriceStyle::OnPaint(pDC);

}

