// PriceStyleKagi.cpp: implementation of the CPriceStyleKagiKagi class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PriceStyleKagi.h"
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

CPriceStyleKagi::CPriceStyleKagi()
{
	Style = 0;	
	connected = false;
	pSeries = NULL;
}

CPriceStyleKagi::~CPriceStyleKagi()
{
	
}

void CPriceStyleKagi::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

int thin = 1, thick = 3;

void CPriceStyleKagi::OnPaint(CDC* pDC)
{


	/*

	pCtrl->priceStyleParams[0] = lines

	7 columns

	width = 300

	300 / 7 = 42.85 pixels per column

	*/


	// Find series
	CSeries* high = pSeries->GetSeriesOHLCV("high");
	if(NULL == high) return;
	CSeries* low = pSeries->GetSeriesOHLCV("low");
	if(NULL == low) return;	
	CSeries* close = pSeries->GetSeriesOHLCV("close");
	if(NULL == close) return;	

	long width = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;

	double x = 0;

	double reversalSize = pCtrl->priceStyleParams[0];
	if(reversalSize > 50 || reversalSize < 0.0001){ // Changed from 0.001 12/9/05
		reversalSize = 1;		
	}

	double ptsOrPct = pCtrl->priceStyleParams[1];
	if(ptsOrPct != 2 && ptsOrPct != 1){
		ptsOrPct = 1; // Points = 1, Percent = 2
	}

	pCtrl->priceStyleParams[1] = ptsOrPct;
	pCtrl->priceStyleParams[0] = reversalSize;

	double reverse = 0;	

	pCtrl->m_psValues1.clear();
	pCtrl->m_psValues2.clear();
	pCtrl->m_psValues3.clear();
	
	double nClose = 0, nClose2 = 0;
	double start = 0;
	int points = 1, percent = 2;
	int up = 1, down = 2;
	int direction = 0; // up or down	
	int weight = 0; // thick or thin
	int totalBars = 0;
	double max = 0, min = 0;
	double oldMax = 0, oldMin = 0;

	if(close->data_slave.size() < 3) return;
	
	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex + 1);
	int cnt = 0;

	// Count columns that will fit on screen
  int n;
	for(n = 0; n != pCtrl->endIndex + 1; ++n){

		if(n < close->data_slave.size() - 1){ // Changed on 7/7/06 and 2/1/08
		
			if(close->data_master[n].value != NULL_VALUE){
		

				nClose = close->data_master[n].value;			
			

				if(ptsOrPct == percent){
					reverse = nClose * reversalSize; // Percent
				}
				else{
					reverse = reversalSize; // Points
				}


		
				if(direction == 0){ // First bar
					nClose2 = close->data_master[n + 1].value;
					if(nClose2 > nClose){
						direction = up;
						weight = thick;
						start = nClose;
						max = nClose;
					}
					else{
						direction = down;
						weight = thin;
						start = nClose2;
						min = nClose2;
					}
				}

 

				if(direction == up){
					if(nClose > max){
						max = nClose;
						if(max > start) weight = thick;
					}
					else if(nClose < max - reverse){
						direction = down;
						start = max;
						min = nClose;
						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex)
							totalBars++;
					}
				}
				else if(direction == down){
					if(nClose < min){
						min = nClose;
						if(min < start) weight = thin;
					}
					else if(nClose > min + reverse){
						direction = up;
						start = min;
						max = nClose;
						if(n >= pCtrl->startIndex && n <= pCtrl->endIndex)
							totalBars++;
					}
				}

			}

		}

	}	


	pCtrl->xCount = totalBars;



	// Paint columns
	CRect box;	
	int px = 0;
	if(totalBars == 0) return;
	int space = ((width - pCtrl->extendedXPixels) / totalBars);	
	totalBars = 0;
	direction = 0;
	int pWeight = 0;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		px = (int) x;
//	End Of Revision

	// Calculate from beginning, but only show between startIndex and endIndex
	int size = close->data_master.size();
	
	for(n = 0; n != pCtrl->endIndex; ++n){

		if(close->data_slave[n].value != NULL_VALUE){
		

			nClose = close->data_master[n].value;
		

			if(ptsOrPct == percent){
				reverse = nClose * reversalSize; // Percent
			}
			else{
				reverse = reversalSize; // Points
			}


	
			if(direction == 0){ // First bar
				nClose2 = close->data_master[n + 1].value;
				if(nClose2 > nClose){
					direction = up;
					weight = thick;
					start = nClose;
					max = nClose;
					min = nClose;
					oldMin = nClose2;
					oldMax = nClose;
					pWeight = thick;
				}
				else{
					direction = down;
					weight = thin;
					start = nClose2;
					min = nClose2;
					max = nClose2;
					oldMin = nClose;
					oldMax = nClose2;
					pWeight = thin;
				}
			}

 

			if(direction == up){
				if(nClose > max){
					max = nClose;
					if(max > oldMax){					
						weight = thick;
					}
				}
				else if(nClose < max - reverse){

					// Paint previous up bar
					if(weight == pWeight) oldMax = 0;
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
						PaintBar(pDC, (int)x, space, max, min, up, n, weight, oldMax);
//	End Of Revision
						x += space;
					}
					
					pWeight = weight;
					direction = down;
					start = max;
					oldMin = min;
					min = nClose;					
					totalBars++;
				}
			}
			else if(direction == down){
				if(nClose < min){
					min = nClose;
					if(min < oldMin){						
						weight = thin;
					}
				}
				else if(nClose > min + reverse){

					// Paint previous down bar
					if(weight == pWeight) oldMin = 0;
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
						PaintBar(pDC, (int)x, space, max, min, down, n, weight, oldMin);
//	End Of Revision
						x += space;
					}

					pWeight = weight;
					direction = up;
					start = min;
					oldMax = max;
					max = nClose;
					totalBars++;
				}
			}





			// Record the x value
			if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
				pCtrl->xMap[cnt] = x + (space / 2);
				cnt++;
			}

		}



		// 8/13/04 RDG
		SeriesPoint sp;
		sp.jdate = close->data_master[n].jdate;

		sp.value = max;
		pCtrl->m_psValues1.push_back(sp);
		sp.value = min;
		pCtrl->m_psValues2.push_back(sp);
		int d = -1; if(direction == up) d = 1;
		sp.value = d;
		pCtrl->m_psValues3.push_back(sp);
		



	}
	
	
	// Paint last bar
	if(direction == up){
		if(nClose > max){
			max = nClose;
			if(max >= oldMax){					
				weight = thick;
			}
		}
		if(weight == pWeight) oldMax = 0;
	}
	else if(direction == down){		
		if(nClose <= min){
			min = nClose;
			if(min < oldMin){						
				weight = thin;
			}
		}
		if(weight == pWeight) oldMin = 0;
	}
	if(direction == down){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		PaintBar(pDC, (int)x, 0, max, min, down, n, weight, oldMin);
//	End Of Revision
	}
	else{
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		PaintBar(pDC, (int)x, 0, max, min, up, n, weight, oldMax);
//	End Of Revision
	}


	// 8/13/04 RDG
	SeriesPoint sp;	
	sp.jdate = close->data_master[n-1].jdate;

	sp.value = max;
	pCtrl->m_psValues1.push_back(sp);
	sp.value = min;
	pCtrl->m_psValues2.push_back(sp);
	int d = -1; if(direction == up) d = 1;
	sp.value = d;
	pCtrl->m_psValues3.push_back(sp);

	
	CPriceStyle::OnPaint(pDC);

}



void CPriceStyleKagi::PaintBar(CDC* pDC, int x, double space,
										 double top, double bottom, 
										 int direction, int index,
										 int weight,
										 double changePrice){

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


	CRect box;

	double y = 0;	
	top = pSeries->GetY(top);
	bottom = pSeries->GetY(bottom);
	double change = pSeries->GetY(changePrice);
	
	y = top;
	if(y > bottom){
		top = bottom;
		bottom = y;
	}
		
	if(changePrice > 0) changePrice = pSeries->GetY(changePrice);


	CPen* penUpThin = new CPen(pSeries->lineStyle, 1, ucolor);
	CPen* penDownThin = new CPen(pSeries->lineStyle, 1, dcolor);
	CPen* penUpThick = new CPen(pSeries->lineStyle, 3, ucolor);
	CPen* penDownThick = new CPen(pSeries->lineStyle, 3, dcolor);
	CPen* pOldPen = NULL;


	if(direction == 1){
		if(changePrice > 0){
			pOldPen = pDC->SelectObject(penUpThick);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)x, (int)changePrice);
			pDC->SelectObject(penUpThin);
			pDC->MoveTo((int)x, (int)changePrice);
			pDC->LineTo((int)x, (int)bottom);
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)(x + space), (int)top);
//	End Of Revision
			pDC->SelectObject(pOldPen);
		}
		else{
			if(weight == thin){
				pOldPen = pDC->SelectObject(penUpThin);
			}
			else{
				pOldPen = pDC->SelectObject(penUpThick);
			}
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)x, (int)bottom);
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)(x + space), (int)top);
//	End of REvision
			pDC->SelectObject(pOldPen);
		}
	}
	else{
		if(changePrice > 0){
			pOldPen = pDC->SelectObject(penDownThick);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)x, (int)changePrice);
			pDC->SelectObject(penDownThin);
			pDC->MoveTo((int)x, (int)changePrice);
			pDC->LineTo((int)x, (int)bottom);
			pDC->MoveTo((int)x, (int)bottom);
			pDC->LineTo((int)(x + space), (int)bottom);
//	End Of Revision
			pDC->SelectObject(pOldPen);
		}
		else{
			if(weight == thin){
				pOldPen = pDC->SelectObject(penDownThin);
			}
			else{
				pOldPen = pDC->SelectObject(penDownThick);
			}
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			pDC->MoveTo((int)x, (int)top);
			pDC->LineTo((int)x, (int)bottom);
			pDC->LineTo((int)x, (int)bottom);
			pDC->LineTo((int)(x + space), (int)bottom);
//	End Of Revision
			pDC->SelectObject(pOldPen);
		}
	}
	
	
	penUpThin->DeleteObject();
	penUpThick->DeleteObject();
	penDownThin->DeleteObject();
	penDownThick->DeleteObject();
	if(penUpThin != NULL) delete penUpThin;
	if(penUpThick != NULL) delete penUpThick;
	if(penDownThin != NULL) delete penDownThin;
	if(penDownThick != NULL) delete penDownThick;

}