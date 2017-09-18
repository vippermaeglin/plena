// PriceStyleThreeLineBreak.cpp: implementation of the CPriceStyleThreeLineBreakThreeLineBreak class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PriceStyleThreeLineBreak.h"
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

CPriceStyleThreeLineBreak::CPriceStyleThreeLineBreak()
{
	prevIndex = 0;
	Style = 0;	
	connected = false;
	pSeries = NULL;
}

CPriceStyleThreeLineBreak::~CPriceStyleThreeLineBreak()
{
	
}

void CPriceStyleThreeLineBreak::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

void CPriceStyleThreeLineBreak::OnPaint(CDC* pDC)
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
	double lines = pCtrl->priceStyleParams[0];
	if(lines > 50 || lines < 1){		
		lines = 3;
		pCtrl->priceStyleParams[0] = lines;
	}

	highs.resize(lines);
	lows.resize(lines);
	
	double nHigh = 0, nLow = 0, nClose = 0, nHH = 0, nLL = 0;
	int white = 1, black = 2;
	double nStart = 0;
	int block = 0; // black or white
	int totalBlocks = 0;

	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex + 1);
	int cnt = 0;

	pCtrl->m_psValues1.clear();
	pCtrl->m_psValues2.clear();
	pCtrl->m_psValues3.clear();

	// Count columns that will fit on screen
  int n;
	for(n = 0; n != pCtrl->endIndex + 1; ++n){
	
		if(n > close->data_slave.size() -1) break;
		if(high->data_master[n].value != NULL_VALUE && 
			close->data_master[n].value != NULL_VALUE && 
			low->data_master[n].value != NULL_VALUE){
		
			// Calculate N Line Break
			nHigh = high->data_master[n].value;
			nLow = low->data_master[n].value;
			nClose = close->data_master[n].value;
		

			if(block == white){
				 if(IsNewBlock(-1, nClose)){
					nHH = nStart; // New black block
					nLL = nClose;
					nStart = nClose;
					block = black;
					AddBlock(nHH, nLL);
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
						totalBlocks++;
					}
				}
				if(IsNewBlock(1, nClose)){
					nHH = nClose; // New white block
					nLL = nStart;
					nStart = nClose;
					block = white;
					AddBlock(nHH, nLL);
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
						totalBlocks++;
					}
				}
			}

			else if(block == black){
				 if(IsNewBlock(1, nClose)){
					nHH = nClose; // New white block
					nLL = nStart;
					nStart = nClose;
					block = white;
					AddBlock(nHH, nLL);
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
						totalBlocks++;
					}
				}
				if(IsNewBlock(-1, nClose)){
					nHH = nStart; // New black block
					nLL = nClose;
					nStart = nClose;
					block = black;
					AddBlock(nHH, nLL);
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
						totalBlocks++;
					}
				}
			}



			if(block == 0){ // Prime first block				
				double nClose2 = close->data_slave[n + 1].value;
				if(nClose2 > nClose){
					block = white; nHH = nClose2; nLL = nClose;	nStart = nClose;
				}
				else{
					block = black; nHH = nClose; nLL = nClose2;	nStart = nClose2;
				}
				AddBlock(nHH, nLL);
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
					totalBlocks++;
				}
			}

 
		}
	}	


	pCtrl->xCount = totalBlocks;

	highs.clear();
	lows.clear();

	
	highs.resize(lines);
	lows.resize(lines);


	// Paint columns
	CRect box;
	block = 0;
	int px = 0;
	x = 0;
	if(totalBlocks == 0) return;
	int space = ((width - pCtrl->extendedXPixels) / totalBlocks);
	totalBlocks = 0;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
	px = (int)x;

	// Calculate from beginning, but only show between startIndex and endIndex
	int size = close->data_master.size();
	for(n = 0; n != pCtrl->endIndex + 1; ++n){

		if(n > close->data_slave.size() - 1) break; // Changed from .size() -1 on 7/7/06


		if(n == close->data_slave.size())
		{
			int test = 1;
		}

		if(high->data_master[n].value != NULL_VALUE && 
			close->data_master[n].value != NULL_VALUE && 
			low->data_master[n].value != NULL_VALUE){
		
			// Calculate N Line Break
			nHigh = high->data_master[n].value;
			nLow = low->data_master[n].value;
			nClose = close->data_master[n].value;
			


			if(block == white){
				if(IsNewBlock(-1, nClose)){

					// Paint last white block
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revisions added 6/10/2004 By Katchei
//	Added type cast to suppress errors
						PaintBox(pDC, (int)x, space, nHH, nLL, 1, n, close);
//	End Of Revisions
						x += space;
					}

					nHH = nStart; // New black block
					nLL = nClose;
					nStart = nClose;
					block = black;
					AddBlock(nHH, nLL);					
					totalBlocks++;						
				}
				if(IsNewBlock(1, nClose)){

					// Paint last black block
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
						PaintBox(pDC, (int)x, space, nHH, nLL, 1, n, close);
//	End Of Revision
						x += space;
					}

					nHH = nClose; // New white block
					nLL = nStart;
					nStart = nClose;
					block = white;
					AddBlock(nHH, nLL);
					totalBlocks++;										
				}
			}

			else if(block == black){
				if(IsNewBlock(1, nClose)){

					// Paint last white block
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
						PaintBox(pDC,(int) x, space, nHH, nLL, -1, n, close);
//	End Of Revision
						x += space;
					}

					nHH = nClose; // New white block
					nLL = nStart;
					nStart = nClose;
					block = white;
					AddBlock(nHH, nLL);
					totalBlocks++;
				}
				if(IsNewBlock(-1, nClose)){

					// Paint last black block
					if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
//	Revisions added 6/10/2004 By Katchei
//	Added type cast to suppress errors
						PaintBox(pDC, (int)x, space, nHH, nLL, -1, n, close);
//	End Of Revisions
						x += space;
					}

					nHH = nStart; // New black block
					nLL = nClose;
					nStart = nClose;
					block = black;
					AddBlock(nHH, nLL);
					totalBlocks++;
				}
			}



			if(block == 0){ // Prime first block				
				double nClose2 = close->data_slave[n + 1].value;
				if(nClose2 > nClose){
					block = white; nHH = nClose2; nLL = nClose;	nStart = nClose;
				}
				else{
					block = black; nHH = nClose; nLL = nClose2;	nStart = nClose2;
				}
				AddBlock(nHH, nLL);
				if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
					totalBlocks = 1;
				}				
				x = 0;
				if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revisions added 6/10/2004 By Katchei
//	Added type cast to suppress errors
				px = (int)x;
//	End Of Revisions
			}

			
			// Record the x value
			if(n >= pCtrl->startIndex && n <= pCtrl->endIndex){
				pCtrl->xMap[cnt] = x + (space / 2);
				cnt++;
			}



		}	
		


	}


	// Finish last block
	if(block == black)
		PaintBox(pDC, (int)x, space, nHH, nLL, -1, n, close);
	else
		PaintBox(pDC, (int)x, space, nHH, nLL, 1, n, close);

	CPriceStyle::OnPaint(pDC);

}


void CPriceStyleThreeLineBreak::AddBlock(double high, double low){

//	Revisions added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	int lines = (int)pCtrl->priceStyleParams[0] - 1;
//	End Of Revisions
	for(int n = 0; n < lines; ++n){
		highs[n] = highs[n + 1];
		lows[n] = lows[n + 1];
	}

	highs[lines] = high; 
	lows[lines] = low;

}


bool CPriceStyleThreeLineBreak::IsNewBlock(int direction, double close){
 	
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	int lines = (int)pCtrl->priceStyleParams[0];
//	End Of Revisions
	int exceed = 0;

	for(int n = 0; n < lines; ++n){
		if(direction == 1){
			if(close > highs[n] || highs[n] == 0) exceed++;
		}
		else if(direction == -1){
			if(close < lows[n] || lows[n] == 0) exceed++;
		}
	}

	return exceed == lines;

}



void CPriceStyleThreeLineBreak::PaintBox(CDC* pDC, int x, double space,
										 double top, double bottom, 
										 int direction, int index,
										 CSeries* pClose){
	


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


	// 8/13/04 RDG
	SeriesPoint sp;
	for(int n = prevIndex + 1; n != index + 1; ++n){
		double jdate = 0;
		if(n < pClose->data_master.size())
			jdate = pClose->data_master[n].jdate;		
		sp.jdate = jdate;
		sp.value = top;
		pCtrl->m_psValues1.push_back(sp);
		sp.value = bottom;
		pCtrl->m_psValues2.push_back(sp);
		sp.value = direction;
		pCtrl->m_psValues3.push_back(sp);
	}
	prevIndex = index;


	
	CRect box;
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	box.top = (int)pSeries->GetY(top);
	box.bottom = (int)pSeries->GetY(bottom);
	box.left = x;
	box.right = x + (int)space;
//	End Of Revisions
	if(pCtrl->threeDStyle){
		if(direction == 1){
			pCtrl->FadeVert(pDC, ucolor, 0, box);
			pDC->Draw3dRect(box, ucolor, pCtrl->gridColor);
		}
		else if(direction == -1){
			pCtrl->FadeVert(pDC, dcolor, 0, box);
			pDC->Draw3dRect(box, dcolor, pCtrl->gridColor);
		}
	}
	
	else{
				
		int found = pCtrl->barColorName.Find(".vol",0);
		bool customOwner = found == -1;

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