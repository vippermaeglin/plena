// PriceStyle.cpp: implementation of the CPriceStyle class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "PriceStyle.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CPriceStyle::CPriceStyle()
{
	nSavedDC = 0;
}

CPriceStyle::~CPriceStyle()
{

}

void CPriceStyle::OnPaint(CDC *pDC)
{
	// 3/31/05 JG
	// Estimate extra points in x map
	double last = 0;
	if(pCtrl->xMap.size() > 0) last = pCtrl->xMap[0];
	double avgDiff = 0;
	double cntDiff = 0;
	double sumDiff = 0;
	double sum = 0;
	long cnt = 0;
  int n;
	for(n = 0; n != pCtrl->xMap.size(); ++n){
		if(pCtrl->xMap[n] != last){
			avgDiff += cntDiff;
			sumDiff ++;
			cntDiff = 0;

			sum += pCtrl->xMap[n] - last;
			cnt++;

			last = pCtrl->xMap[n];
		}
		cntDiff++;		
	}
	
	// Double the xMap with estimated values
	avgDiff = avgDiff / sumDiff;
	double avg = sum / cnt;	
	int j = pCtrl->xMap.size() - 1;
	cnt = 0;
	int size = pCtrl->xMap.size();
	double x = pCtrl->xMap[pCtrl->xMap.size() - 1] + avg;
	for(n = 0; n != size; ++n){
		cnt++;
		if(cnt >= avgDiff){
			cnt = 0;
			x += avg;						
		}
		pCtrl->xMap.push_back(x);
	}

}

void CPriceStyle::Connect(CStockChartXCtrl *Ctrl, CSeries* Series)
{
	// virtual function
}

// Prevents drawing over certain areas
void CPriceStyle::ExcludeRects(CDC* pDC)
{	
	// Save the old DC
	nSavedDC = pDC->SaveDC();
	// Don't draw over the y scale
	pDC->ExcludeClipRect(pSeries->ownerPanel->yScaleRect);
	CRect panel = pSeries->ownerPanel->panelRect;
	panel.bottom = panel.top;
	panel.top = 0; // Don't draw above this panel	
	pDC->ExcludeClipRect(panel);
	// Don't draw below the panel
	panel = pSeries->ownerPanel->panelRect;
	panel.top = panel.bottom;
	panel.bottom = pCtrl->height + CALENDAR_HEIGHT;
	pDC->ExcludeClipRect(panel);	
}

void CPriceStyle::IncludeRects(CDC* pDC)
{
	pDC->RestoreDC(nSavedDC);
}