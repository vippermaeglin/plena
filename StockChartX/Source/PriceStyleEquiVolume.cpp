// PriceStyleEquiVolume.cpp: implementation of the CPriceStyleEquiVolumeEquiVolume class.
//
//////////////////////////////////////////////////////////////////////

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

CPriceStyleEquiVolume::CPriceStyleEquiVolume()
{
	Style = 0;	
	connected = false;
	pSeries = NULL;
}

CPriceStyleEquiVolume::~CPriceStyleEquiVolume()
{
	
}

void CPriceStyleEquiVolume::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	pCtrl = Ctrl;
	pSeries = Series;
	connected = true;
}

void CPriceStyleEquiVolume::OnPaint(CDC* pDC)
{


	/*

	highestVolume = highest volume up to current record (not after)

	x = volume / highestVolume
	if x = 0 then x = 1

	1,0.25,0.5,0.75,0.25,0.5,1
	total=4.25

	/4.25 total
	0.235,0.058,0.117,0.176,0.058,0.117,0.235=1

	300* pixels
	70.5,17.4,35.1,52.8,17.4,35.1,70.5=300

	*/

	// Find series
	CSeries* open = pSeries->GetSeriesOHLCV("open");	
	if(NULL == open) return;
	CSeries* high = pSeries->GetSeriesOHLCV("high");	
	if(NULL == high) return;
	CSeries* low = pSeries->GetSeriesOHLCV("low");	
	if(NULL == low) return;
	CSeries* close = pSeries->GetSeriesOHLCV("close");	
	if(NULL == close) return;
	CSeries* volume = pSeries->GetSeriesOHLCV("volume");	
	if(NULL == volume) return;
	
	long width = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;

	double x1 = 0;
	double x2 = 0;
	double y2 = 0;
	double y1 = 0;

	int direction = 0;
	int up = 1, down = 2;
	double highestVolume = 0;
	double x = 0;
	double equiVol = 0;
	int bars = pCtrl->endIndex;
	double total = 0;
	long px = 0;
	CRect box;
	CRect frame;


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

	int found = pCtrl->barColorName.Find(".vol",0);
	bool customOwner = found == -1;
	CBrush* brCustom = new CBrush(pSeries->lineColor);
	CBrush* brDown = new CBrush(dcolor);
	CBrush* brUp = new CBrush(ucolor);
			
	pCtrl->xMap.resize(pCtrl->endIndex - pCtrl->startIndex);	
	int cnt = 0;
	
	int wick = pCtrl->barWidth;
	int halfWick = (wick / 2);
	if(halfWick < 1) halfWick = 0;

	// Count total Equi-Volume
  int n;
	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){

		if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
		   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE &&
		   volume->data_slave[n].value != NULL_VALUE){
			
			// Calculate Equi-Volume
			highestVolume = HighestVolumeToRecord(n);
			x = volume->data_slave[n].value /  highestVolume;			
			total += x;

			if(pCtrl->darvasBoxes){								
				equiVol = volume->data_slave[n].value /  highestVolume;
				equiVol = equiVol / total;
				equiVol = width * equiVol;
				x += equiVol;				
				// Record the x value
				pCtrl->xMap[cnt] = x;
				cnt++;
			}

		}
	}	
	cnt = 0;


	// Just for darvas boxes
	if(pCtrl->darvasBoxes){
		for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){			
			if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
			   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE &&
			   volume->data_slave[n].value != NULL_VALUE){
				
				if(pCtrl->darvasBoxes){								
					equiVol = volume->data_slave[n].value /  highestVolume;
					equiVol = equiVol / total;
					equiVol = width * equiVol;
					x += equiVol;				
					// Record the x value
					pCtrl->xMap[cnt] = x;
					cnt++;
				}

			}
		}		
		CSeriesStock* pStock = (CSeriesStock*)pSeries;
		pStock->PaintDarvasBoxes(pDC);
	}	
	x = 0;
	cnt = 0;

	if(pCtrl->yAlignment == LEFT) x = pCtrl->yScaleWidth;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (long)
	px = (long)x;
//	End Of Revision

	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){

		if(open->data_slave[n].value != NULL_VALUE && high->data_slave[n].value != NULL_VALUE &&
		   low->data_slave[n].value != NULL_VALUE && close->data_slave[n].value != NULL_VALUE &&
		   volume->data_slave[n].value != NULL_VALUE){
			

			// Get color
			if(pCtrl->barColors[n] != -1 && customOwner){ // Custom bar color
				brCustom->DeleteObject();
				if(NULL != brCustom) delete brCustom;
				brCustom = new CBrush(pCtrl->barColors[n]);

			}
			

			// Calculate Equi-Volume
			highestVolume = HighestVolumeToRecord(n);
			equiVol = volume->data_slave[n].value /  highestVolume;
			equiVol = equiVol / total;
			equiVol = width * equiVol;
			x += equiVol;
			
			
			// Record the x value
			pCtrl->xMap[cnt] = x;
			cnt++;
			

			// Paint the Equi-Volume box
			x1 = px;
			x2 = x;
			y1 = pSeries->GetY(high->data_slave[n].value);
			y2 = pSeries->GetY(low->data_slave[n].value);
			if(y2 == y1) y1 = y2 - 2;

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			pDC->MoveTo((int)x1,(int)y1);
			box = CRect((int)x1,(int)y1,(int)x2,(int)y2);
//	End Of Revision



			frame = box;
			if(Style == psEquiVolumeShadow){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				if(close->data_slave[n].value > open->data_slave[n].value){
					box.top = (int)pSeries->GetY(close->data_slave[n].value);
				}
				else{
					box.bottom = (int)pSeries->GetY(close->data_slave[n].value);
				}
//	End Of Revision
			}



			// Paint candle volume?
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			int wx = (int)(x1 + (x2 - x1) / 2);
//	End Of Revision
			if(Style == psCandleVolume){
				if(close->data_slave[n].value > open->data_slave[n].value){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
					box.top = (int)pSeries->GetY(close->data_slave[n].value);
					box.bottom = (int)pSeries->GetY(open->data_slave[n].value);
//	End Of Revision
				}
				else{
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
					box.top = (long)pSeries->GetY(open->data_slave[n].value);
					box.bottom = (long)pSeries->GetY(close->data_slave[n].value);
//	End Of Revision
				}
				if(box.bottom == box.top) box.top = box.bottom - 2;
				CPen* pen = new CPen(pSeries->lineStyle, pSeries->lineWeight, pSeries->lineColor);
				CPen* pOldPen = pDC->SelectObject(pen);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				pDC->MoveTo((int)wx,(int)y1);
				pDC->LineTo((int)wx,(int)y2);
//	End of Revision
				pDC->SelectObject(pOldPen);
				pen->DeleteObject();
				if(NULL != pen) delete pen;
				frame = box;
			}


			// Is the bar up or down?
			if(n == 0){
				if(close->data_slave[n].value > open->data_slave[n].value){
					direction = up;
				}
				else{
					direction = down;
				}
			}
			else{
				if(close->data_slave[n].value > close->data_slave[n-1].value){
					direction = up;
				}
				else{
					direction = down;
				}
			}


			if(pCtrl->threeDStyle){
				if(pCtrl->barColors[n] != -1){
					if(direction == up){
						pCtrl->FadeVert(pDC, pCtrl->barColors[n], 0, box);
						pDC->Draw3dRect(frame, 0, pCtrl->barColors[n]);
					}
					else{
						pCtrl->FadeVert(pDC, 0, pCtrl->barColors[n], box);
						pDC->Draw3dRect(frame, pCtrl->barColors[n], 0);
					}
				}
				else if(direction == up){
					pCtrl->FadeVert(pDC, ucolor, 0, box);
					pDC->Draw3dRect(frame, ucolor, dcolor);
				}
				else{
					pCtrl->FadeVert(pDC, 0, dcolor, box);
					pDC->Draw3dRect(frame, ucolor, dcolor);
				}				
			}
			else{
				if(pCtrl->barColors[n] != -1 && customOwner){					
					pDC->FillRect(box, brCustom);
					pDC->Draw3dRect(frame, ucolor, pCtrl->barColors[n]);
				}
				else if(direction == up){
					pDC->FillRect(box, brUp);
					pDC->Draw3dRect(frame, ucolor, ucolor);
				}
				else{					
					pDC->FillRect(box, brDown);
					pDC->Draw3dRect(frame, dcolor, dcolor);
				}

			}
			


//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
			px = (long)x;
//	End of Revision

		}
	}



	brCustom->DeleteObject();
	brUp->DeleteObject();
	brDown->DeleteObject();	
	
	if (NULL != brCustom) delete brCustom;
	if (NULL != brUp) delete brUp;
	if (NULL != brDown) delete brDown;


	CPriceStyle::OnPaint(pDC);

}


// Gets the highest volume to date from the MASTER array.
double CPriceStyleEquiVolume::HighestVolumeToRecord(int record)
{
	CSeries* pVolume = pSeries->GetSeriesOHLCV("volume");
	if(NULL == pVolume) return 0;

	double maxVolume = 0;

	for(int n = 0; n != pVolume->data_master.size(); ++n){
		if(n > record) break;
		if(pVolume->data_master[n].value > maxVolume)
			maxVolume = pVolume->data_master[n].value;
	}

	return maxVolume;
}