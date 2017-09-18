// SeriesStandard.cpp: implementation of the CSeriesStandard class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "SeriesStandard.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSeriesStandard::CSeriesStandard(LPCTSTR name, int type, int members, CChartPanel* owner)
{	
	isTwin = false;
	upColor = -1;
	downColor = -1;
	szName = name;
	ownerPanel = owner;
	pCtrl = owner->pCtrl;
	seriesType = type;
	memberCount = members;
	Initialize();
	nSpace = 0;
}

CSeriesStandard::~CSeriesStandard()
{

}

void CSeriesStandard::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
			//printf("\nCSeriesStandard::OnPaint() "+szName);
#endif
	if(!seriesVisible) return;
	if(data_slave.size() < 1) return;

	// Calculating the the best lineWeightBar for this viewport
	int lineWeightBar;
	double x1Temp, x2Temp;	

	if (data_slave.size() >= 2) {
	  x1Temp = ownerPanel->GetX(0);
	  x2Temp = ownerPanel->GetX(1);
	  lineWeightBar = (((x2Temp - x1Temp)*0.75) > 1) ? (x2Temp - x1Temp)*0.75 : 1;
	}
	else {
      lineWeightBar = lineWeight;
	}
	
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (double)	
	double x1 = pCtrl->GetSlaveRecordCount() - data_slave.size();
	double x2 = ownerPanel->GetX((int)x1);
	double y2 = GetY((double)data_slave[0].value);
//	End Of Revision
	double y1 = 0;
	int cnt = 0;
	
	
	OLE_COLOR ucolor, dcolor;
	if(upColor != -1)
		ucolor = upColor;
	else
		ucolor = pCtrl->upColor;
	if(downColor != -1)
		dcolor = downColor;
	else
		dcolor = pCtrl->downColor;



	// Geometric pens - 9/18/04 RG
	// Windows 95/98: The PS_ENDCAP_ROUND, PS_ENDCAP_SQUARE, PS_ENDCAP_FLAT, 
	// PS_JOIN_BEVEL, PS_JOIN_MITER, and PS_JOIN_ROUND styles are supported 
	// only for geometric pens when used to draw paths.

	CPen* penCustom = new CPen(lineStyle, lineWeight, lineColor);

	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);
	CPen* penUp = new CPen(lineStyle, lineWeight, ucolor);
	CPen* penDown = new CPen(lineStyle, lineWeight, dcolor);

	LOGBRUSH lbUp;
	lbUp.lbStyle = BS_SOLID;
	lbUp.lbColor = ucolor;
	lbUp.lbHatch = 0;	
	CPen* penUpBar = new CPen(PS_GEOMETRIC | PS_SOLID | PS_ENDCAP_FLAT, lineWeightBar, &lbUp);

	LOGBRUSH lbDown;
	lbDown.lbStyle = BS_SOLID;
	lbDown.lbColor = dcolor;
	lbDown.lbHatch = 0;	
	CPen* penDownBar = new CPen(PS_GEOMETRIC | PS_SOLID | PS_ENDCAP_FLAT, lineWeightBar, &lbDown);
	
	LOGBRUSH lb;
	lb.lbStyle = BS_SOLID;
	lb.lbColor = lineColor;
	lb.lbHatch = 0;
	CPen* penBar = new CPen(PS_GEOMETRIC | PS_SOLID | PS_ENDCAP_FLAT, lineWeightBar, &lb);
	

	// Volume up/down colors 10/1/04 RG requested by Nat Ravichandran
	bool isVolume = false;
	if(szName.Find("VOL")!=-1) {
		isVolume = true;
	}
	bool volumeUpDown = pCtrl->useVolumeUpDownColors && isVolume;

	std::vector<CPointF> pointsF;

	CPen* pOldPen = NULL;

	if(isVolume){
		pOldPen = pDC->SelectObject(penBar);
	}
	else{
		pOldPen = pDC->SelectObject(pen);
	}

	bool isOscillator = false;

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	if(data_slave.size() < (unsigned int)pCtrl->startIndex)
//	End Of Revisions
		return;
  int n;
	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		if(data_slave[n].value < 0 && data_slave[n].value != NULL_VALUE){
			isOscillator = true;
			break;
		}
	}
	ExcludeRects(pDC);
	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		++cnt;
		x1 = ownerPanel->GetX(cnt);
		y1 = data_slave[n].value;
		if(y1 != NULL_VALUE){
			y1 = GetY(data_slave[n].value);
		}
		if(n == pCtrl->startIndex && n != 0){
			y2 = y1;
		}



		// Volume up/down colors 10/1/04 RG requested by Nat Ravichandran
		CSeries* close = NULL;
		if(volumeUpDown){		
			close = GetSeriesOHLCV("close");	
			if(NULL == close) volumeUpDown = false;
		}
		if(volumeUpDown){
			if(n > 0){
				if(close->data_slave[n].value > close->data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Up
					if(isVolume){
						pOldPen = pDC->SelectObject(penUpBar);
					}
					else{
						pOldPen = pDC->SelectObject(penUp);
					}
				}
				else if(close->data_slave[n].value < close->data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Down
					if(isVolume){
						pOldPen = pDC->SelectObject(penDownBar);
					}
					else{
						pOldPen = pDC->SelectObject(penDown);
					}
				}
				else{
					pDC->SelectObject(pOldPen); // Same
					if(isVolume){
						pOldPen = pDC->SelectObject(penBar);
					}
					else{
						pOldPen = pDC->SelectObject(pen);
					}
				}
			}
			else{ // First bar
				pDC->SelectObject(pOldPen);
				if(isVolume){
					pOldPen = pDC->SelectObject(penBar);
				}
				else{
					pOldPen = pDC->SelectObject(pen);
				}
			}
		}

		else if((pCtrl->useLineSeriesColors || upColor != -1) && !isOscillator){ // +/- change colors
			if(n > 0){
				if(data_slave[n].value > data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Up
					if(isVolume){
						pOldPen = pDC->SelectObject(penUpBar);
					}
					else{
						pOldPen = pDC->SelectObject(penUp);
					}
				}
				else if(data_slave[n].value < data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Down
					if(isVolume){
						pOldPen = pDC->SelectObject(penDownBar);
					}
					else{
						pOldPen = pDC->SelectObject(penDown);
					}
				}
				else{
					pDC->SelectObject(pOldPen); // Same
					if(isVolume){
						pOldPen = pDC->SelectObject(penBar);
					}
					else{
						pOldPen = pDC->SelectObject(pen);
					}
				}
			}
			else{ // First bar
				pDC->SelectObject(pOldPen);
				if(isVolume){
					pOldPen = pDC->SelectObject(penBar);
				}
				else{
					pOldPen = pDC->SelectObject(pen);
				}
			}
		}
		else if((pCtrl->useLineSeriesColors || upColor != -1) && isOscillator){ // +/- zero 0 oscillator
			if(data_slave[n].value > 0){
				pDC->SelectObject(pOldPen); // Up
				if(isVolume){
					pOldPen = pDC->SelectObject(penUpBar);
				}
				else{
					pOldPen = pDC->SelectObject(penUp);
				}
			}
			else if(data_slave[n].value < 0){
				pDC->SelectObject(pOldPen); // Down
				if(isVolume){
					pOldPen = pDC->SelectObject(penDownBar);
				}
				else{
					pOldPen = pDC->SelectObject(penDown);
				}
			}
			else{
				pDC->SelectObject(pOldPen); // Same
				if(isVolume){
					pOldPen = pDC->SelectObject(penBar);
				}
				else{
					pOldPen = pDC->SelectObject(pen);
				}
			}
		}


		if(seriesType == OBJECT_SERIES_BAR){
#ifdef _CONSOLE_DEBUG
		//	printf("\n\tOBJECT_SERIES_BAR!!! "+szName);
#endif
			bool custom = false;
			// if(data_slave[n].value != NULL_VALUE && y2 != NULL_VALUE){ changed 3/3/08
			if(data_slave[n].value != NULL_VALUE){

				if(pCtrl->barColors[n] != -1){ // Custom bar color
					if(pCtrl->CompareNoCase(pCtrl->barColorName, szName)){
						penCustom->DeleteObject();
						if(NULL != penCustom) delete penCustom;
						penCustom = new CPen(lineStyle, lineWeightBar, pCtrl->barColors[n]);						
						pOldPen = pDC->SelectObject(penCustom);
						custom = true;
					}
				}

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)

				
				// 6/2/2005 by JG
				// Make sure at least 2 or 3 pixels show
				// if the value is the same as the min Y.
				int minY = (int)GetY(min);
				int nY1 = (int)y1;
				if(minY - 3 < nY1) nY1 -=3;
				pDC->MoveTo((int)x1,nY1);
				if(isOscillator){
					pDC->LineTo((int)x1,(int)GetY(0));
				}
				else{
					pDC->LineTo((int)x1, minY);
//	End Of Revisions
				}

				if(custom){
					pDC->SelectObject(pOldPen);					
					custom = false;
				}


			}

		}
		else if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR){
			
#ifdef _CONSOLE_DEBUG
		//	printf("\n\tOBJECT_SERIES_INDICATOR\LINE!!! "+szName);
#endif
			if(pCtrl->yAlignment == LEFT && x2 < pCtrl->yScaleWidth + 5){
				x2 = x1;
				y2 = y1;
			}
			if(data_slave[n].value != NULL_VALUE && y2 != NULL_VALUE || (szName.Find("HILO")>-1)){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				//pDC->MoveTo((int)x1,(int)y1);
				//pDC->LineTo((int)x2,(int)y2);
				//pCtrl->pdcHandler->DrawLine(CPointF(x1,y1),CPointF(x2,y2),lineWeight,lineStyle,lineColor, pDC);
				pointsF.push_back(CPointF(x1-2,y1));
//	End Of Revisions
			}
		}
		y2 = y1;
		x2 = x1;
	}	
#ifdef _CONSOLE_DEBUG
	//printf (" \n\n\n\n Nome Serie impressa = %s", szName);
#endif
	if(szName.Find("HILO")>-1)pCtrl->pdcHandler->DrawLadder(pointsF,pointsF.size(),lineWeight,lineStyle,lineColor, pDC);
	else if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR)pCtrl->pdcHandler->DrawPath(pointsF,pointsF.size(),lineWeight,lineStyle,lineColor,lineColor, pDC, true);
	IncludeRects(pDC);
	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	penUp->DeleteObject();
	penDown->DeleteObject();
	penBar->DeleteObject();
	penUpBar->DeleteObject();
	penDownBar->DeleteObject();
	penCustom->DeleteObject();

	if(NULL != pen) delete pen;
	if(NULL != penUp) delete penUp;
	if(NULL != penDown) delete penDown;
	if(NULL != penBar) delete penBar;
	if(NULL != penUpBar) delete penUpBar;
	if(NULL != penDownBar) delete penDownBar;
	if(NULL != penCustom) delete penCustom;

	
}


void CSeriesStandard::OnMouseMove(CPoint point)
{

	if(data_slave.size() < 1) return;
	

	if(pCtrl->resizing || pCtrl->drawing || pCtrl->movingObject) return;
	
//	Revision added 6/10/2004 By Katchei
//	Added type cast to suppress errors
	int index = (int)(ownerPanel->GetReverseX(point.x)  + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size()-1)
//	End Of Revisions
		return;	

	double y = GetY(data_slave[index].value);
	double errorPixels = 10;

	if(point.y - errorPixels < y && point.y + errorPixels > y){
		if(pCtrl->m_mouseState == MOUSE_NORMAL){
			pCtrl->FireOnItemMouseMove(OBJECT_SERIES_LINE, szName);
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

void CSeriesStandard::OnLButtonDown(CPoint point)
{
	if(data_slave.size() < 1) return;
			
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size()-1)
//	End Of Revisions
		return;

	double y = GetY(data_slave[index].value);

	double errorPixels = 10;

	if(point.y - errorPixels < y && point.y + errorPixels > y){

		pCtrl->dragging = true;
		
		// If selected, show grab hand
		if(this->selected){						
			pCtrl->m_mouseState = MOUSE_CLOSED_HAND;
			pCtrl->m_Cursor = IDC_CLOSED_HAND;
			SetCursor(AfxGetApp()->LoadCursor(pCtrl->m_Cursor));
			// This series is being dragged somewhere,
			// so allert the app about this.
			pCtrl->swapSeries.push_back(szName);
		}

	}
}

void CSeriesStandard::OnLButtonUp(CPoint point)
{

	if(!selectable || !seriesVisible) return;
	if(data_slave.size() < 1) return;
	if (pCtrl->movingObject)return;
	if(selected) pCtrl->FireOnItemLeftClick(OBJECT_SERIES_LINE, szName);
		
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)	
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size()-1) return;
//	End Of Revisions

	double y = GetY(data_slave[index].value);

	double errorPixels = 10;

	bool repaint = false;
	if(point.y - errorPixels < y && point.y + errorPixels > y){		
		pCtrl->UnSelectAll();
		// Select this series/object only		
		this->selected = true;
		repaint = true;
		pCtrl->OnSelectSeries(szName);
	}
	else{
		if(pCtrl->SelectCount() > 0){
			this->selected = false;
			repaint = true;
		}
	}
	if(repaint){
		ownerPanel->Invalidate();
		pCtrl->RePaint();
	}
	
}

// Draws selection boxes around series
void CSeriesStandard::OnPaintXOR(CDC* pDC)
{
double x1 = pCtrl->GetSlaveRecordCount() - data_slave.size();
	
	if(!this->selected) return;

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	double x2 = ownerPanel->GetX((int)x1);	
	double y2 = GetY(data_slave[0].value);
//	End Of Revisions
	double y1 = 0;
	int cnt = 0;

	pDC->SetROP2(R2_NOT);

	//cnt = pCtrl->GetSlaveRecordCount() - data_slave.size();
	double space = SEL_SPACE;

	for(int n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		++cnt;
		if(data_slave[n].value != NULL_VALUE){	
			x1 = ownerPanel->GetX(cnt) - 3;
			space = x1 - x2;
			if(space >= SEL_SPACE){
				x2 = x1 + 8;
				y1 = GetY(data_slave[n].value) - 3;
				y2 = y1 + 8;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				if(y2 > (int)ownerPanel->y2)
					y2 = (int)ownerPanel->y2;
				pDC->MoveTo((int)x1,(int)y1);
				pDC->Rectangle((int)x1,(int)y1,(int)x2,(int)y2);
//	End Of Revision
				space = 0;
			}
		}
	}

	pDC->SetROP2(R2_COPYPEN);

}

void CSeriesStandard::OnRButtonUp(CPoint point)
{
	if (data_slave.size() < 1) return;
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex + 1);
	if (index < 0 || index >(int)data_slave.size() - 1) return;
	double y = GetY(data_master[index].value);
	double errorPixels = 10;
#ifdef _CONSOLE_DEBUG
	printf("\SERIESTAND ONRBUTTON x=%d y=%d index=%d yInd=%f value=%f", point.x, point.y, index, y, data_master[index].value);
#endif	
	bool repaint = false;
	if (point.y - errorPixels < y && point.y + errorPixels > y){
		pCtrl->UnSelectAll();
		// Select this series/object only		
		this->selected = true;
		repaint = true;
		pCtrl->OnSelectSeries(szName);
	}
	if (selected)
	{
#ifdef _CONSOLE_DEBUG
		printf(" selected!!!");
#endif
		pCtrl->FireOnItemRightClick(OBJECT_SERIES_INDICATOR, szName);
	}
	if (repaint){
		ownerPanel->Invalidate();
		pCtrl->RePaint();
	}
	

}

void CSeriesStandard::OnDoubleClick(CPoint point)
{	
	if(selected){
		if(linkTo != ""){ // This series belongs to an indicator
			pCtrl->SendSeriesMessage(DOUBLE_CLICK, linkTo);
		}
		else{
			pCtrl->FireOnItemDoubleClick(seriesType, szName);
		}
	}
}
