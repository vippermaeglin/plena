// PriceStyleRenko.cpp: implementation of the CPriceStyleRenkoRenko class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PriceStyleRenko.h"
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

CPriceStyleRenko::CPriceStyleRenko()
{
	Style = 0;	
	connected = false;
	pSeries = NULL;
}

CPriceStyleRenko::~CPriceStyleRenko()
{
	
}

void CPriceStyleRenko::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

void CPriceStyleRenko::OnPaint(CDC* pDC)
{


	/*

	pCtrl->priceStyleParams[0] = lines

	7 columns

	width = 300

	300 / 7 = 42.85 pixels per column

	*/


	// Find series
	CSeries* close = pSeries->GetSeriesOHLCV("close");
	if(NULL == close) return;	

	double width = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;

	double x = 0;
	double boxSize = pCtrl->priceStyleParams[0];
	if(boxSize > 50 || boxSize < 0.0001){   // Changed from 0.001 12/9/05
		boxSize = 1;
	}

	pCtrl->priceStyleParams[0] = boxSize;
	
	double nHigh = 0, nLow = 0, nClose = 0, nHH = 0, nLL = 0;
	int white = 1, black = 2;	
	int brick = 0; // black or white
	int totalBricks = 0;

			
	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex + 1);
	int cnt = 0;


	pCtrl->m_psValues1.clear();
	pCtrl->m_psValues2.clear();
	pCtrl->m_psValues3.clear();


	// Calculate from beginning, but only show between startIndex and endIndex
	int size = close->data_master.size();
	int n;
	for(n = 0; n != pCtrl->endIndex + 1; ++n){
		
		if(n < close->data_slave.size() - 1){// break; // Changed from .size() -1 on 7/7/06, 2/1/08
			if(close->data_slave[n].value != NULL_VALUE){
		
			// Calculate Renko
			nClose = close->data_slave[n].value;


			if(brick == 0){ // Prime first brick				
				double nClose2 = close->data_slave[n + 1].value;
				if(nClose2 > nClose){
					brick = white; nHH = nClose + boxSize; nLL = nClose;
				}
				else{
					brick = black; nHH = nClose2; nLL = nClose2 - boxSize;
				}
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
					totalBricks = 1;
				}
			}


			if(nClose < nLL - boxSize){
				brick = black;
				nHH = nLL;
				nLL = nHH - boxSize;
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex)
				totalBricks++;
			}
			else if(nClose > nHH + boxSize){
				brick = white;
				nLL = nHH;
				nHH = nLL + boxSize;
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex)
				totalBricks++;
			}


		}
		}
	}	


	pCtrl->xCount = totalBricks;

	// Rescale the y axis	

	// Paint columns
	CRect box;
	brick = 0;
	int px = 0;
	x = 0;
	if(totalBricks == 0) return;
	double space = ((width - pCtrl->extendedXPixels) / totalBricks);	
	double o = space * totalBricks;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
		px = (int)x;
//	End Of Revision
   	
	totalBricks = 0;

	// Calculate from beginning, but only show between startIndex and endIndex
	size = close->data_master.size();
	for(n = 0; n != pCtrl->endIndex + 1; ++n){

		if(n < close->data_slave.size()) { // Changed 2/1/08
			if(close->data_master[n].value != NULL_VALUE){
		
			// Calculate Renko
			nClose = close->data_master[n].value;
			

			if(brick == 0){ // Prime first brick				
				double nClose2 = close->data_slave[n + 1].value;
				if(nClose2 > nClose){
					brick = white; nHH = nClose + boxSize; nLL = nClose;
				}
				else{
					brick = black; nHH = nClose2; nLL = nClose2 - boxSize;					
				}
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
					totalBricks = 1;
				}
				x = 0;
				if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					px = (int)x;
//	End Of Revision
			}




			if(nClose < nLL - boxSize){

				// Paint last white brick
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					PaintBox(pDC, (int)x, space, nHH, nLL, brick, n);
//	End Of Revision
					totalBricks++;
					x += space;
				}

				brick = black;
				nHH = nLL;
				nLL = nHH - boxSize;
				
			}
			else if(nClose > nHH + boxSize){

				// Paint last black brick				
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					PaintBox(pDC, (int)x, space, nHH, nLL, brick, n);
//	End Of Revisions
					totalBricks++;
					x += space;
				}

				brick = white;
				nLL = nHH;
				nHH = nLL + boxSize;				
			}


			
			// Record the x value
			if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
				pCtrl->xMap[cnt] = x + (space / 2);
				cnt++;
			}


			}
		}	
		


		// 8/13/04 RDG		
		SeriesPoint sp;
		if(n < close->data_master.size())
			sp.jdate = close->data_master[n].jdate; // 2/1/08

		sp.value = nHH;
		pCtrl->m_psValues1.push_back(sp);
		sp.value = nLL;
		pCtrl->m_psValues2.push_back(sp);
		int d = -1; if(brick == white) d = 1;
		sp.value = d;
		pCtrl->m_psValues3.push_back(sp);




	}


	CPriceStyle::OnPaint(pDC);

}




void CPriceStyleRenko::PaintBox(CDC* pDC, int x, double space,
										 double top, double bottom, 
										 int direction, int index){



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



//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	CRect box;
	box.top = (long)pSeries->GetY(top);
	box.bottom = (long)pSeries->GetY(bottom);
	box.left = x;
	box.right = x + (int)space;
//	End Of Revision

	if(pCtrl->threeDStyle){
		if(direction == 1){
			pCtrl->FadeVert(pDC, ucolor, 0, box);
			pDC->Draw3dRect(box, ucolor, pCtrl->gridColor);
		}
		else if(direction == 2){
			pCtrl->FadeVert(pDC, dcolor, 0, box);
			pDC->Draw3dRect(box, dcolor, pCtrl->gridColor);
		}
	}
	
	else{
		bool customOwner = pCtrl->CompareNoCase(pCtrl->barColorName, pSeries->szName);
		CBrush* brCustom = new CBrush(pSeries->lineColor);
		CBrush* brDown = new CBrush(dcolor);
		CBrush* brUp = new CBrush(ucolor);
		if(pCtrl->barColors[index] != -1 && customOwner){ // Custom bar color
			brCustom = new CBrush(pCtrl->barColors[index]);
		}

		if(pCtrl->barColors[index] != -1 && customOwner){					
			pDC->FillRect(box, brCustom);
			pDC->Draw3dRect(box, ucolor, pCtrl->barColors[index]);
		}
		else if(direction == 1){
			pDC->FillRect(box, brUp);
			pDC->Draw3dRect(box, ucolor, ucolor);
		}
		else{					
			pDC->FillRect(box, brDown);
			pDC->Draw3dRect(box, dcolor, dcolor);
		}

		if(NULL != brCustom) brCustom->DeleteObject();
		if(NULL != brUp) brUp->DeleteObject();
		if(NULL != brDown) brDown->DeleteObject();
		if (NULL != brCustom) delete brCustom;
		if (NULL != brUp) delete brUp;
		if (NULL != brDown) delete brDown;

	}


}