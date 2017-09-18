// Indicator.cpp: implementation of the CIndicator class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "Indicator.h"
#include "dpi.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CIndicator::CIndicator(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	ownerPanel = NULL;	
}

CIndicator::CIndicator()
{	
}

CIndicator::~CIndicator()
{

}


void CIndicator::SetOwnerChartPanel(CChartPanel *pOwner)
{
	ownerPanel = pOwner;
}

 
void CIndicator::SetName(LPCTSTR pName)
{
	m_Name = pName;
}

LPCTSTR CIndicator::GetName()
{
	return m_Name;
}

BOOL CIndicator::Calculate(){
	
	calculated = true;
	pCtrl->updatingIndicator = false;

	return TRUE;
}

/* 
|
| Drawing Code 
|
*/


void CIndicator::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	printf("Indicator::OnPaint()");
#endif
	if(!seriesVisible) return;
 	if(data_slave.size() < 1) return;	
	
	double x1 = pCtrl->GetSlaveRecordCount() - data_slave.size();
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	double x2 = ownerPanel->GetX((int)x1);
//	End Of Revision
	double y2 = GetY(data_slave[0].value);
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
	
	CPen* penUp = new CPen(lineStyle, lineWeight, ucolor);
	CPen* penDown = new CPen(lineStyle, lineWeight, dcolor);	
	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);	
	CPen* pOldPen = NULL;
	pOldPen = pDC->SelectObject(pen);


	// Geometric pens - 9/18/04 RG
	// Windows 95/98: The PS_ENDCAP_ROUND, PS_ENDCAP_SQUARE, PS_ENDCAP_FLAT, 
	// PS_JOIN_BEVEL, PS_JOIN_MITER, and PS_JOIN_ROUND styles are supported 
	// only for geometric pens when used to draw paths.
	
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

	pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penBar) : pDC->SelectObject(pen);				
//--

	bool isOscillator = false;

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	if(data_slave.size() < (unsigned int)pCtrl->startIndex)
	{
		pen->DeleteObject();
		penUp->DeleteObject();
		penDown->DeleteObject();
		if(pen) delete pen;
		if(penUp) delete penUp;
		if(penDown) delete penDown;
		penBar->DeleteObject();
		penUpBar->DeleteObject();
		penDownBar->DeleteObject();
	    if(penBar) delete penBar;
	    if(penUpBar) delete penUpBar;
	    if(penDownBar) delete penDownBar;

		return;
	}
//	End Of Revision

	int n;
	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		if(data_slave[n].value < 0 && data_slave[n].value != NULL_VALUE){
			isOscillator = true;
			break;
		}
	}

	ExcludeRects(pDC);
	std::vector<CPointF> pointsF;
	for(n = pCtrl->startIndex; n != pCtrl->endIndex; ++n){
		++cnt;
		x1 = ownerPanel->GetX(cnt);

		if(n >= (int)data_slave.size()) // 10/10/07
		{
			// 9/20/07
			IncludeRects(pDC);
			pen->DeleteObject();
			penUp->DeleteObject();
			penDown->DeleteObject();
			if(pen) delete pen;
			if(penUp) delete penUp;
			if(penDown) delete penDown;
			penBar->DeleteObject();
			penUpBar->DeleteObject();
			penDownBar->DeleteObject();
	        if(penBar) delete penBar;
	        if(penUpBar) delete penUpBar;
	        if(penDownBar) delete penDownBar;

			return;			
		}

		y1 = data_slave[n].value;
		if(n == pCtrl->startIndex && n != 0){
			y2 = y1;
		}
		if(y1 != NULL_VALUE){
			y1 = GetY(data_slave[n].value);
		}
		if(n == pCtrl->startIndex && n != 0){
			y2 = y1;
		}


		double value = data_slave[n].value;
		if(data_slave[n].value == NULL_VALUE) value = 0;
		if(indicatorType == indMACDHistogram || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow)
		{
			// MACD Histogram			
			if(value > 0)
			{
				y1 = GetY(value);
				y2 = GetY(0);
			}
			else
			{				
				y2 = GetY(value);
				y1 = GetY(0);
			}
		}


		if((pCtrl->useLineSeriesColors || upColor != -1) && !isOscillator){ // +/- change colors
			if(n > 0){
				if(data_slave[n].value > data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Up
					pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow ) ? 
						         pDC->SelectObject(penUpBar) : pDC->SelectObject(penUp);
				}
				else if(data_slave[n].value < data_slave[n-1].value){
					pDC->SelectObject(pOldPen); // Down
					pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penDownBar) : pDC->SelectObject(penDown);
				}
				else{
					pDC->SelectObject(pOldPen); // Same
					pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penBar) : pDC->SelectObject(pen);
				}
			}
			else{ // First bar
				pDC->SelectObject(pOldPen);
				pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penBar) : pDC->SelectObject(pen);
			}
		}
		else if((pCtrl->useLineSeriesColors || upColor != -1) && isOscillator){ // +/- zero 0 oscillator
			if(data_slave[n].value > 0){
				pDC->SelectObject(pOldPen); // Up
				pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penUpBar) : pDC->SelectObject(penUp);
			}
			else if(data_slave[n].value < 0){
				pDC->SelectObject(pOldPen); // Down
				pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penDownBar) : pDC->SelectObject(penDown);
			}
			else{
				pDC->SelectObject(pOldPen); // Same
				pOldPen = (seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indVolume || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow) ? 
						         pDC->SelectObject(penBar) : pDC->SelectObject(pen);
			}
		}


		if(seriesType == OBJECT_SERIES_BAR || indicatorType == indMACDHistogram || indicatorType == indAroonOscillator || indicatorType == indChaikinMoneyFlow )
		{
			// MACD histogram	
			if(data_slave[n].value != NULL_VALUE && y2 != NULL_VALUE){
				pDC->MoveTo((int)x1,(int)y1);
				pDC->LineTo((int)x1,(int)y2);
			}

		}
		else if(seriesType == OBJECT_SERIES_BAR || indicatorType == indVolume)
		{			
			//Volume
			if(data_slave[n].value != NULL_VALUE && y2 != NULL_VALUE){
				//yMin always bigger then y1
				int yMin = (int)GetY(min);
				if(yMin<(int)y1+1)y1-=1;
				pDC->MoveTo((int)x1,(int)y1);				
				pDC->LineTo((int)x1,yMin); // GetY(0)
			}
		}
		else if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR){

			if(pCtrl->yAlignment == LEFT && x2 < pCtrl->yScaleWidth + 5){
				x2 = x1;
				y2 = y1;
			}
			if(data_slave[n].value != NULL_VALUE && y1 != NULL_VALUE){
				if(x1 > 0) pointsF.push_back(CPointF(x1,y1));
			}
		}
		//y2 = y1;
		//x2 = x1;
	}
	if (szName.Find("PSAR") > -1){
		pCtrl->pdcHandler->DrawRoundDot(pointsF, pointsF.size(), lineWeight, lineStyle, lineColor, pDC);
		//pCtrl->pdcHandler->DrawSemiPath(pointsF, pointsF.size(), lineWeight, 0, lineColor, lineColor, pDC);
		//pCtrl->pdcHandler->DrawPath(pointsF, pointsF.size(), lineWeight, 0, lineColor, lineColor, pDC);
	}
	else if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR)pCtrl->pdcHandler->DrawPath(pointsF,pointsF.size(),lineWeight,lineStyle,lineColor,lineColor, pDC, true);
	IncludeRects(pDC);
	pDC->SelectObject(pOldPen);
	pen->DeleteObject();
	penUp->DeleteObject();
	penDown->DeleteObject();
	if(pen) delete pen;
	if(penUp) delete penUp;
	if(penDown) delete penDown;
	penBar->DeleteObject();
	penUpBar->DeleteObject();
	penDownBar->DeleteObject();
	if(penBar) delete penBar;
	if(penUpBar) delete penUpBar;
	if(penDownBar) delete penDownBar;

	
}


void CIndicator::OnMouseMove(CPoint point)
{

	if(data_slave.size() < 1) return;
	

	if(pCtrl->resizing || pCtrl->drawing || pCtrl->movingObject) return;
	
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x)  + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size() - 1)
//	End Of Revision
		return;	

	double y = GetY(data_slave[index].value);
	double errorPixels = 10;

	if(point.y - errorPixels < y && point.y + errorPixels > y){
		if(pCtrl->m_mouseState == MOUSE_NORMAL){
			pCtrl->FireOnItemMouseMove(OBJECT_SERIES_INDICATOR, szName);
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

void CIndicator::OnLButtonDown(CPoint point)
{
	if(data_slave.size() < 1) return;
			
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size() - 1)
//	End Of Revision
		return;

	double y = GetY(data_slave[index].value);

	double errorPixels = 10;
	
	if(point.y - errorPixels < y && point.y + errorPixels > y ){ //Dont allow moves
#ifdef _CONSOLE_DEBUG
		printf("\nDragging Indicator!");
#endif
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

void CIndicator::OnLButtonUp(CPoint point)
{

	if(!selectable || !seriesVisible) return;
	if(data_slave.size() < 1) return;

	if (pCtrl->movingObject) return;

	if(selected) pCtrl->FireOnItemLeftClick(OBJECT_SERIES_INDICATOR, szName);
			
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex);
	if(index < 0 || index > (int)data_slave.size() - 1) 
//	End Of Revision
		return;

	double y = GetY(data_slave[index].value);

	double errorPixels = 10;
	
#ifdef _CONSOLE_DEBUG
	printf("\nINDICATOR ONLBUTTONUP()");
#endif
	bool repaint = false;
	if(point.y - errorPixels < y && point.y + errorPixels > y){		
#ifdef _CONSOLE_DEBUG
	printf(" SELECT");
#endif
		pCtrl->UnSelectAll();
		// Select this series/object only		
		this->selected = true;
		repaint = true;
		pCtrl->OnSelectSeries(szName);
	}
	else{
#ifdef _CONSOLE_DEBUG
	printf(" UNSELECT");
#endif

	this->selected = false;
	repaint = true;
	}
	if(repaint){
		ownerPanel->Invalidate();
		pCtrl->RePaint();
	}
	
}

// Draws selection boxes around series
void CIndicator::OnPaintXOR(CDC* pDC)
{
	double x1 = pCtrl->GetSlaveRecordCount() - data_slave.size();
	
	if(!this->selected) return;
	CPen* pen = new CPen(lineStyle, lineWeight, lineColor);
	//CPen* penBar = new CPen(PS_GEOMETRIC | PS_SOLID | PS_ENDCAP_FLAT, lineWeightBar, &lb);

	CPen* pOldPen = pDC->SelectObject(pen);	
	if(data_slave.size() == 0) return;
	double x2 = ownerPanel->GetX((int)x1);
	double y2 = GetY(data_slave[0].value);
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
				if(y2 > ownerPanel->y2) y2 = ownerPanel->y2;
				pDC->MoveTo((int)x1,(int)y1);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
				pDC->Rectangle((int)x1,(int)y1,(int)x2,(int)y2);
//	End Of Revision
				space = 0;
			}
		}
	}

	pDC->SetROP2(R2_COPYPEN);
	pDC->SelectObject(pOldPen);
}

void CIndicator::OnRButtonUp(CPoint point)
{	
	if(data_slave.size() < 1) return;
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex + 1);
	if(index < 0 || index > (int)data_slave.size() - 1) return;
	double y = GetY(data_master[index].value);
	double errorPixels = 10;
#ifdef _CONSOLE_DEBUG
	printf("\nINDICATOR ONRBUTTON x=%d y=%d index=%d yInd=%f value=%f",point.x,point.y,index,y,data_master[index].value);
#endif	
	bool repaint = false;
	if(point.y - errorPixels < y && point.y + errorPixels > y){		
		pCtrl->UnSelectAll();
		// Select this series/object only		
		this->selected = true;
		repaint = true;
		pCtrl->OnSelectSeries(szName);
	}
	if(selected) 
	{
#ifdef _CONSOLE_DEBUG
	printf(" selected!!!");
#endif
		pCtrl->FireOnItemRightClick(OBJECT_SERIES_INDICATOR, szName);
	}
	if(repaint){
		ownerPanel->Invalidate();
		pCtrl->RePaint();
	}
}

void CIndicator::OnDoubleClick(CPoint point)
{	
	if (selected) {
		pCtrl->FireOnItemDoubleClick(OBJECT_SERIES_INDICATOR, szName);
		selected = false;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
	/*
	if(selected){
		if(userParams){
			dialogShown = false;
			showDialog = true;
			GetUserInput();
		}
		pCtrl->FireOnItemDoubleClick(OBJECT_SERIES_INDICATOR, szName);
	}*/
}

// Gets a series from the chart and converts it to a TA-SDK CField.
// Name = Input name (Source, Volume, etc.)
// Series = series name from main chart
// Length = record count of series
CField* CIndicator::SeriesToField(LPCTSTR Name, LPCTSTR Series, int Length)
{	
	CString sName = Name;
	int series = -1;
	int panel = pCtrl->GetPanelByName(Series);
	
	if(panel > -1){
		series = pCtrl->panels[panel]->GetSeriesIndex(Series);
	}
	
	if(panel < 0 || series < 0){
		if(!userParams){
			pCtrl->ThrowErr(5207, "Invalid source field " + sName + " for indicator " + szName);
		}
		return NULL;
	}


	CSeries* pSeries = pCtrl->panels[panel]->series[series];
	if(NULL == pSeries) return NULL;
	// Ensure the indicator has been calculated
	if(pSeries->seriesType == OBJECT_SERIES_INDICATOR && !calculating){
		calculating = true; // prevent deep circular references
		if(!pSeries->calculated) {
			if(!pSeries->Calculate()){
#ifdef _CONSOLE_DEBUG
				printf("\nSeriesToField:: ERROR CALCULATING %s",szName);
#endif
			}
		}
		calculating = false;
	}
	double firstVal = 0; // Get first value that isn't a NULL_VALUE
	
  int n;
	for(n = 0; n != Length; ++n){
		if(pSeries->data_master[n].value != NULL_VALUE &&
			pSeries->data_master[n].value != 0){
			firstVal = pSeries->data_master[n].value;
			break;
		}
	}
	
	CField* pRet = new CField(Length, Name);

	for(n = 0; n != Length; ++n){
		if(pSeries->data_master[n].value == NULL_VALUE)
			pRet->setValue(n + 1, firstVal);
		else
		pRet->setValue(n + 1, pSeries->data_master[n].value);
	}

	return pRet;
}

void CIndicator::SetSeriesColor(LPCTSTR Name, OLE_COLOR Color)
{
	return; // 9/29/04 RDG - this is totally useless

	CString Name2;
	for(int n = 0; n != pCtrl->panels.size(); ++n){
		if(pCtrl->panels[n]->visible){
			for(int j = 0; j != pCtrl->panels[n]->series.size(); ++j){
				Name2 = pCtrl->panels[n]->series[j]->szName;				
				if(pCtrl->CompareNoCase(Name, Name2)){

					// Only set the line color if it has not been customized
					// 9/25/04 RG
					
					if(pCtrl->panels[n]->series[j]->lineColor != RGB(255,0,0)){
						pCtrl->panels[n]->series[j]->lineColor = Color;
					}
					
					return;

				}
			}
		}
	}
}

void CIndicator::SetSeriesSignal(LPCTSTR Name, OLE_COLOR Color, int Style, int Weight)
{
	CString Name2;
	for(int n = 0; n != pCtrl->panels.size(); ++n){
		if(pCtrl->panels[n]->visible){
			for(int j = 0; j != pCtrl->panels[n]->series.size(); ++j){
				Name2 = pCtrl->panels[n]->series[j]->szName;				
				if(pCtrl->CompareNoCase(Name, Name2)){
						pCtrl->panels[n]->series[j]->lineColor = Color;
						pCtrl->panels[n]->series[j]->lineStyle = Style;
						pCtrl->panels[n]->series[j]->lineWeight = Weight;
					
					return;

				}
			}
		}
	}
}

// Allow the user to fix the problem or throw an error.
void CIndicator::ProcessError(LPCTSTR Description)
{
	
	UINT answer = 0;

	if(pCtrl->dialogErrorShown) return;
	pCtrl->dialogErrorShown = true;
	pCtrl->FireOnDialogShown();
	
	if(NULL != m_pIndDlg) m_pIndDlg->canUnload = false; // Don't let the user hit Alt+F4	

	CString desc = Description;

	CString error = desc + 
		"\nClick OK to fix the problem or click\nCancel to remove " + szName;

	CString find = desc;
	find.MakeLower();
	if(find.Find("circular reference") != -1){
		if(NULL != m_pIndDlg) m_pIndDlg->m_cmdCancel.EnableWindow(FALSE);
	}	
	
	if(find.Find("remove indicator") != -1){
		answer = -1;
		recycleFlag = true;
		goto remove;
	}	

	if(userParams){

		if(m_pIndDlg == NULL){
			answer = 0;
		}
		else{
			answer = MessageBox(pCtrl->m_hWnd,error, 
				"Error:", MB_ICONWARNING + MB_OKCANCEL);
		}

		if(answer == 1){			
			inputError = true;			
      GetUserInput();
    }
    else{
      if(NULL != m_pIndDlg) m_pIndDlg->ShowWindow(SW_HIDE);
      recycleFlag = true;
remove:
      for(int n = 0; n != pCtrl->panels.size(); ++n){
        if(pCtrl->panels[n]->visible){
          for(int j = 0; j != pCtrl->panels[n]->series.size(); ++j){
            if(pCtrl->panels[n]->series[j]->linkTo == szName){
              pCtrl->panels[n]->deleteSeries(pCtrl->panels[n]->series[j]->szName);
              goto remove;
						}
					}
				}
			}
	
			pCtrl->DelayMessage("", TIMER_RECALC, 500);
			pCtrl->FireOnDialogHiden();
		}				
	}
	else{
		pCtrl->ThrowErr(5208, Description);
	}
	pCtrl->dialogErrorShown = false;
}

void CIndicator::Initialize()
{
	pCtrl->updatingIndicator = true;
	m_pIndDlg = NULL;
	CSeries::Initialize();
	dialogShown = false;
	inputError = false;
	calculating = false;
	calculated = false;
}

void CIndicator::OnDestroy()
{
	if(m_pIndDlg != NULL){	
		delete m_pIndDlg;
	}
}

void CIndicator::SetParamInfo(){
	//
}

void CIndicator::SetParam(int index, int type, LPCTSTR defaultValue)
{
	ASSERT(index < 10 && index > -1);
	paramTypes[index] = type; 
	paramDefs[index] = defaultValue;
}


 
bool CIndicator::GetUserInput()
{

	pCtrl->updatingIndicator = true;

	// If this dialog was originally made programmatically,
	// but the client program is asking to show the dialog,
	// then convert this indicator into a user-param indicator.
	if(paramDefs[0] == "") SetParamInfo();
	if(showDialog) userParams = true;
	if(dialogShown){
		if(HasCircularReference(this) && !inputError){
			ProcessError(szName + " has a circular reference to another indicator");
			return false;
		}	
		return true;
	}

	/*
	First check to see if we have inputs already.
	If we do (paramStr[0] != ""), then return true.
	
	If we don't have inputs or the dialog is requested
	for some reason anyway, then show the dialog and 
	return false. The dialog will call Calculate()
	again when inputs have been collected.

	If the Calculate() function throws an error,
	ProcessError will call this function.
	*/
	
	// Get user input
	bool firstTime = paramStr[0] == "";
	if(paramStr[0] != "" && !inputError && !showDialog){
		return true; // No need to get user input
	}
	pCtrl->locked = true;


	

	int x = 0, y = 0;

	// Get the property dialog
	if(NULL == m_pIndDlg){
		m_pIndDlg = new CIndPropDlg();
		m_pIndDlg->Create(IDD_PROP_DIALOG,pCtrl);
		m_pIndDlg->pCtrl = pCtrl;
		m_pIndDlg->pIndicator = this;
	}

	
	
 	
	int width = 270;
	int height = 300;

	CRect rect;	
	pCtrl->GetWindowRect(&rect);
	rect.left = (rect.Width() / 2) + 
		rect.left - (width / 2);
	rect.top = (rect.Height() / 2) + 
		rect.top  - (height / 2) - CALENDAR_HEIGHT;
	if(rect.left < 0) rect.left = 0;
	if(rect.top < 0) rect.top = 0;
	rect.right = rect.left + width;	
	rect.bottom = rect.top + height;


	// Get the dialog's height
	rect.bottom = rect.top + ((paramStr.size() + 1) * 23) + 160;

	
	// Show the appropriate controls

	CStatic* label = NULL;	
	CComboBox* combo = NULL;
	CEdit* edit = NULL;
	CString value = "";	

	for(int n = 0; n != paramStr.size(); ++n){

		switch(paramTypes[n]){
		
		case ptSymbol:
			combo = GetComboBox(n);			
			EnumSymbols(combo);
			SetComboDefault(combo, paramStr[n]);
			combo->ShowWindow(SW_SHOW);
			break;

		case ptSource:
		case ptSource1:
		case ptSource2:
		case ptSource3:
		case ptVolume:
			combo = GetComboBox(n);			
			EnumSeries(combo);
			SetComboDefault(combo, paramStr[n]);
			combo->ShowWindow(SW_SHOW);
			break;

		case ptPointsOrPercent:
			combo = GetComboBox(n);			
			combo->ResetContent();
			m_pIndDlg->AddString(combo, "Points");
			m_pIndDlg->AddString(combo, "Percent");			
			combo->SetCurSel(0);
			if(paramInt[n] == 1)
				SetComboDefault(combo, "Points");
			else
				SetComboDefault(combo, "Percent");
			combo->ShowWindow(SW_SHOW);
			break;

		case ptMAType:
			combo = GetComboBox(n);
			EnumMATypes(combo);
			SetComboDefault(combo, MATypeToStr(paramInt[n]));
			SetMAComboSel(combo, paramDefs[n]);
			combo->ShowWindow(SW_SHOW);
			break;
		case ptPctDMAType:
			combo = GetComboBox(n);
			EnumMATypes(combo);
			SetComboDefault(combo, MATypeToStr(paramInt[n]));
			SetMAComboSel(combo, paramDefs[n]);
			combo->ShowWindow(SW_SHOW);
			break;

		case ptBarHistory:
		case ptPeriods:
		case ptLevels:
		case ptCycle1:
		case ptCycle2:
		case ptCycle3:
		case ptShortTerm:
		case ptLongTerm:
		case ptPctKPeriods:
		case ptPctDPeriods:
		case ptPctKSlowing:
		case ptPctKSmooth:
		case ptPctDSmooth:
		case ptPctDDblSmooth:
		case ptPctKDblSmooth:
		case ptShortCycle:
		case ptLongCycle:		
		case ptRateOfChange:
		case ptSignalPeriods:
			if(paramInt[n] == 0) value = paramDefs[n]; else value.Format("%d", paramInt[n]);
			edit = GetEdit(n);
			edit->SetWindowText(value);
			edit->ShowWindow(SW_SHOW);
			break;

		case ptStandardDeviations:
		case ptMinTickVal:
		case ptR2Scale:
		case ptMinAF:
		case ptMaxAF:
		case ptShift:
		case ptFactor:
		case ptThreshold1:
		case ptThreshold2:
		case ptLimitMoveValue:
			if(paramDbl[n] == 0) value = paramDefs[n]; else value.Format("%f", paramDbl[n]);
			edit = GetEdit(n);
			edit->SetWindowText(value);
			edit->ShowWindow(SW_SHOW);
			break;


			//// Add new param types here, SetUserInput, and in stdafx.h ////


		}


		// Special handling

		if(firstTime){
			if(paramTypes[n] == ptVolume || paramTypes[n] == ptSource2 || paramTypes[n] == ptSource3){
				CString item = "";
				int length = combo->GetCount();
				for(int n = 0; n != length; ++n){
					combo->GetLBText(n, item);
					if(item.Find("VOL") != -1){
						combo->SetCurSel(n);
						break;
					}
				}
			}


			// 8/23/08 - select .close series as default on first 
			// showing of property dialog if appropriate
			if(paramTypes[n] == ptSource || paramTypes[n] == ptSource1){
				CString item = "";
				int length = combo->GetCount();
				for(int n = 0; n != length; ++n){
					combo->GetLBText(n, item);
					if(item.Find("CLOSE") != -1){
						combo->SetCurSel(n);
						break;
					}
				}
			}

		}


		// Description
		label = GetStatic(n);
		label->ShowWindow(SW_SHOW);
		label->SetWindowText(GetParamName(paramTypes[n]));

	
	}





	// Move the dialog
	dlgRect = rect;
	//m_pIndDlg->MoveWindow(&rect);
	RECT rcNew = ScaleRectangleProportional(rect);
	m_pIndDlg->MoveWindow(&rcNew);


	// Reset controls
	m_pIndDlg->m_cmdCancel.EnableWindow(TRUE);
	m_pIndDlg->m_txtInfo.SetWindowText("");


	// Resize standard controls

	CRect ctrl;
	m_pIndDlg->GetWindowRect(ctrl);

	m_pIndDlg->ScreenToClient(&ctrl); 
	m_pIndDlg->m_fraMain.MoveWindow(SCALEX(5), -1, ctrl.Width() - SCALEX(16), ctrl.Height() - SCALEY(65));
 	
	// See where the buttons should be moved to
	// Major changes - 6/3/2005 RG
	CRect frame;
	m_pIndDlg->m_fraMain.GetWindowRect(&frame);
	CRect button;
	m_pIndDlg->m_cmdApply.GetWindowRect(&button);		
	m_pIndDlg->GetWindowRect(ctrl);

	long frameTop = frame.top - ctrl.top;
	long buttonArea = ctrl.Height() - frame.Height() - frameTop;	
	long frameBottom = ctrl.Height() - (frame.Height() + frameTop);
	long buttonTop = frameBottom + (buttonArea / 2) + (button.Height() / 2);

	m_pIndDlg->m_fraMain.MoveWindow(SCALEX(5), -1, ctrl.Width() - SCALEX(16), 
    ctrl.Height() - (button.Height() * 2) - frameTop);
 
	buttonTop = ctrl.Height() - button.Height() - (button.Height() / 2) - frameTop - SCALEY(4);
	
	m_pIndDlg->m_cmdApply.MoveWindow(ctrl.Width() - SCALEX(158), buttonTop, SCALEX(70), SCALEY(23));
	m_pIndDlg->m_cmdCancel.MoveWindow(ctrl.Width() - SCALEX(83), buttonTop, SCALEX(70), SCALEY(23));
	m_pIndDlg->m_cmdHelp.MoveWindow(SCALEX(6), buttonTop, SCALEX(70), SCALEY(23));

	m_pIndDlg->m_txtInfo.MoveWindow(SCALEX(15), ctrl.Height() - SCALEY(128), ctrl.Width() - SCALEX(36), SCALEY(45));

	// Add color control
	int last = paramStr.size();
	m_pIndDlg->color = lineColor;
	CButton* cmdColor = (CButton*)m_pIndDlg->GetDlgItem(IDC_COLOR);
	cmdColor->MoveWindow(ctrl.Width() - SCALEX(50), ctrl.Height() - SCALEY(156), SCALEX(20), SCALEY(20));
	m_pIndDlg->y = ctrl.Height() - SCALEY(150);
	m_pIndDlg->x = ctrl.Width() - SCALEX(148);
	label = GetStatic(last);
	label->ShowWindow(SW_SHOW);
	label->SetWindowText(_T("Color"));
 	
	showDialog = false;
	inputError = false;

	// Set the dialog caption	
	m_pIndDlg->SetWindowText(szName);

	// Show the dialog
	m_pIndDlg->ShowWindow(SW_SHOW);

	m_pIndDlg->m_cmdApply.SetButtonStyle(BS_DEFPUSHBUTTON);
	m_pIndDlg->m_cmdCancel.SetButtonStyle(BS_PUSHBUTTON);
	m_pIndDlg->m_cmdHelp.SetButtonStyle(BS_PUSHBUTTON);	

	return false;
}


// Sets the combo box selection for a moving average
void CIndicator::SetMAComboSel(CComboBox* combo, LPCSTR paramDef)
{
	CString param = paramDef;

	if(param == "Simple"){
		combo->SetCurSel(0);
	}
	else if(param == "Exponential"){
		combo->SetCurSel(1);
	}
	else if(param == "Time Series"){
		combo->SetCurSel(2);
	}
	else if(param == "Triangular"){
		combo->SetCurSel(3);
	}
	else if(param == "Variable"){
		combo->SetCurSel(4);
	}
	else if(param == "VIDYA"){
		combo->SetCurSel(5);
	}
	else if(param == "Weighted"){
		combo->SetCurSel(6);
	}

}


// Called by CIndPropDlg when Apply is clicked
void CIndicator::SetUserInput()
{
	// Gather inputs from dialog box
		
	CComboBox* combo = NULL;
	CEdit* edit = NULL;
	CString value = "";
	for(int n = 0; n != paramStr.size(); ++n){
	
		switch(paramTypes[n]){

		case ptSymbol:
		case ptSource:
		case ptSource1:
		case ptSource2:
		case ptSource3:
		case ptVolume:
			combo = GetComboBox(n);
			combo->GetWindowText(value);
			paramStr[n] = value;
			break;

		case ptPointsOrPercent:
			combo = GetComboBox(n);			
			combo->GetWindowText(value);
			if(pCtrl->CompareNoCase(value, "Points"))
				paramInt[n] = 1;
			else
				paramInt[n] = 2;
			break;

		case ptMAType:
			combo = GetComboBox(n);			
			paramInt[n] = GetMAType(combo);
			break;

		case ptPctDMAType:
			combo = GetComboBox(n);			
			paramInt[n] = GetMAType(combo);
			break;	

		case ptBarHistory:
		case ptPeriods:
		case ptLevels:
		case ptCycle1:
		case ptCycle2:
		case ptCycle3:
		case ptShortTerm:
		case ptLongTerm:
		case ptPctKPeriods:
		case ptPctDPeriods:
		case ptPctKSlowing:
		case ptPctKSmooth:
		case ptPctDSmooth:
		case ptPctDDblSmooth:
		case ptPctKDblSmooth:
		case ptShortCycle:
		case ptLongCycle:		
		case ptRateOfChange:
		case ptSignalPeriods:
			edit = GetEdit(n);
			edit->GetWindowText(value);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (short)
			paramInt[n] = (short)atol(value);
//	End Of Revision
			break;

		case ptStandardDeviations:
		case ptMinTickVal:
		case ptR2Scale:
		case ptMinAF:
		case ptMaxAF:
		case ptShift:
		case ptFactor:
		case ptThreshold1:
		case ptThreshold2:
		case ptLimitMoveValue:
			edit = GetEdit(n);
			edit->GetWindowText(value);
			paramDbl[n] = atof(value);
			break;

		}
		
	}


	pCtrl->InvalidateIndicators();
	
	

	// Get color selection
	lineColor = m_pIndDlg->color;

	

	// Resume
	dialogShown = true;
	pCtrl->locked = true;
	Calculate();
	if(!inputError){
		pCtrl->locked = false;		
		m_pIndDlg->ShowWindow(SW_HIDE);		
		
		pCtrl->InvalidateIndicators();
		calculated = true;
		UpdateSeries(this);

		pCtrl->DelayMessage("", TIMER_PAINT, 1);

	}


	inputError = false;
	dialogShown = false;


}

CComboBox* CIndicator::GetComboBox(int index)
{
	switch(index){
	case 0:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO1);
	case 1:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO2);
	case 2:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO3);
	case 3:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO4);
	case 4:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO5);
	case 5:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO6);
	case 6:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO7);
	case 7:
		return (CComboBox*)m_pIndDlg->GetDlgItem(IDC_COMBO8);
	}
	
	return NULL;
}

CEdit* CIndicator::GetEdit(int index)
{
	switch(index){
	case 0:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT1);
	case 1:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT2);
	case 2:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT3);
	case 3:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT4);
	case 4:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT5);
	case 5:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT6);
	case 6:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT7);
	case 7:
		return (CEdit*)m_pIndDlg->GetDlgItem(IDC_EDIT8);
	}
	
	return NULL;
}

CStatic* CIndicator::GetStatic(int index)
{
	switch(index){
	case 0:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL1);
	case 1:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL2);
	case 2:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL3);
	case 3:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL4);
	case 4:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL5);
	case 5:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL6);
	case 6:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL7);
	case 7:
		return (CStatic*)m_pIndDlg->GetDlgItem(IDC_LABEL8);
	}
	
	return NULL;
}

void CIndicator::ChangeFocus(int index)
{
	m_pIndDlg->m_txtInfo.SetWindowText(GetParamDescription(index));
}

LPCTSTR CIndicator::GetParamDescription(int index){

	switch(paramTypes[index]){
    case ptSymbol:		
        return "A Symbol is a group of high, low and close series that are displayed as a candle or bar chart";
    case ptSource:		
        return "Calculations are based upon the source field. A source field can be the open, high, low, close, volume or any other available series";
	case ptVolume:		
        return "This indicator requires a volume field for calculation";
	case ptSource1:		
        return "Calculations are based upon the source field. A source field can be the open, high, low, close, volume or any other available series";
	case ptSource2:
        return "The second source field. A source field can be the open, high, low, close, volume or any other available series";
	case ptSource3:
        return "The third source field. A source field can be the open, high, low, close, volume or any other available series";
    case ptCycle1:
        return "The first cycle for the multi-step indicator calculations";
    case ptCycle2:
        return "The second cycle for the multi-step indicator calculations";
    case ptCycle3:
        return "The third cycle for the multi-step indicator calculations";
    case ptLongTerm:
        return "The long term smoothing parameter";
    case ptShortTerm:
        return "The short term smoothing parameter";
    case ptLongCycle:
        return "The long cycle smoothing parameter";
    case ptShortCycle:
        return "The short cycle smoothing parameter";
    case ptLevels:
		return "The level of smoothing periods to use in this calculation";
	case ptPeriods:
        return "The number of bars to use for calculating the indicator";
    case ptRateOfChange:
        return "Rate of change is expressed as momentum / close(t-n) * 100";
    case ptPctKSlowing:
        return "Controls smoothing of %K, where 1 is a fast stochastic and 3 is a slow stochastic";    
    case ptPctKPeriods:
        return "Number of bars used in the stochastic calculation";	
    case ptPctKSmooth:
        return "Number of bars used in the stochastic smoothing";
	case ptPctDSmooth:
		return "Number of bars used in the stochastic double smoothing";
	case ptPctDDblSmooth:
        return "Controls the smoothing of %D";
	case ptPctKDblSmooth:
		return "Controls the smoothing of %K";
    case ptPctDPeriods:
        return "Number of bars used for calculating the average of %D";
    case ptStandardDeviations:
        return "A statistic used as a measure of the dispersion or variation in a distribution";
    case ptMinTickVal:
		return "The dollar value of the move of the smallest tick";
	case ptMinAF:
        return "Minimum acceleration factor";
    case ptMaxAF:
        return "Maximum acceleration factor";
    case ptShift:
        return "The percent of shift to move a series above or below another indicator";
    case ptPointsOrPercent:
        return "Determines the indicator output scale in points or percent";
    case ptMAType:
        return "The moving average type used for smoothing the indicator";
	case ptPctDMAType:
		return "The %D moving average type used for smoothing the indicator";
    case ptR2Scale:
        return "The r-squared (coefficient of determination) scale";    
    case ptSignalPeriods:
        return "The number of bars used for the MACD signal series";
    case ptLimitMoveValue:
        return "The point value of a limit move (futures only)";
	case ptBarHistory:
		return "The number of bars to use in the historical calculation (e.g. 365)";
    case ptFactor:
        return "Keltner Factor";
	default:
		return "";
    }

}

LPCTSTR CIndicator::GetParamName(UINT type)
{
	switch(type){
	case ptMAType:
		return "MA Type";
	case ptPctDMAType:
		return "%D MA Type";
	case ptSymbol:
		return "Symbol";
	case ptVolume:
		return "Volume";
	case ptSource:
		return "Source";
	case ptSource1:
		return "Source 1";
	case ptSource2:
		return "Source 2";
	case ptSource3:
		return "Source 3";
	case ptPointsOrPercent:
		return "Points or Percent";
	case ptLevels:
		return "Levels";
	case ptPeriods:
		return "Periods";
	case ptCycle1:
		return "Cycle 1";
	case ptCycle2:
		return "Cycle 2";
	case ptCycle3:
		return "Cycle 3";
	case ptShortTerm:
		return "Short Term";
	case ptLongTerm:
		return "Long Term";
	case ptRateOfChange:
		return "Rate of Chg";
	case ptPctKPeriods:
		return "%K Periods";
	case ptPctKSlowing:
		return "%K Slowing";
	case ptPctKSmooth:
		return "%K Smooth";
	case ptPctDSmooth:
		return "%D Smooth";
	case ptPctDDblSmooth:
		return "%D Dbl Smooth";
	case ptPctKDblSmooth:
		return "%K Dbl Smooth";
	case ptPctDPeriods:
		return "%D Periods";
	case ptShortCycle:
		return "Short Cycle";
	case ptLongCycle:
		return "Long Cycle";
	case ptStandardDeviations:
		return "Standard Dev";
	case ptR2Scale:
		return "R2 Scale";
	case ptMinTickVal:
		return "Minimum Tick Value";
	case ptMinAF:
		return "Min AF";
	case ptMaxAF:
		return "Max AF";
	case ptShift:
		return "Shift";
	case ptFactor:
		return "Factor";
	case ptSignalPeriods:
		return "Signal Periods";		
	case ptLimitMoveValue:
		return "Limit Move Value";
	case ptBarHistory:
		return "Bar History";
	default:
		return "";
	}

}

void CIndicator::SetComboDefault(CComboBox* combo, LPCTSTR item)
{
	CString text = "";	
	int length = combo->GetCount();
	for(int n = 0; n != length; ++n){
		combo->GetLBText(n, text);
		if(pCtrl->CompareNoCase(text, item)){
			combo->SetCurSel(n);
			return;
		}
	}
}

void CIndicator::EnumSeries(CComboBox* combo)
{
	combo->ResetContent();
	CString series = "";
	CString current = szName;
	current.MakeUpper();
	int count = pCtrl->panels.size();
	for(int panel = 0; panel != count; ++panel){
		for(int s = 0; s != pCtrl->panels[panel]->series.size(); ++s){
			series = pCtrl->panels[panel]->series[s]->szName;
			series.MakeUpper();
			if(current != series && 
				pCtrl->panels[panel]->series[s]->data_master.size() > 1)				
				m_pIndDlg->AddString(combo, series);
		}
	}
	combo->SetCurSel(0);
}

void CIndicator::EnumSymbols(CComboBox* combo)
{
	combo->ResetContent();
	CString symbol = "";
	CString symbols = "";
	int count = pCtrl->panels.size();
	for(int panel = 0; panel != count; ++panel){
		for(int series = 0; series != pCtrl->panels[panel]->series.size(); ++series){
			symbol = pCtrl->panels[panel]->series[series]->szName;
			symbol.MakeUpper();
			int found = symbol.Find(".",0);
			if(found > 0){
				symbol = symbol.Left(found);
				if(symbols.Find("|" + symbol) == -1){
					symbols += "|" + symbol;
					m_pIndDlg->AddString(combo, symbol);
				}
			}
		}
	}
	combo->SetCurSel(0);
}


void CIndicator::EnumPanels(CComboBox* combo){
	combo->ResetContent();
	m_pIndDlg->AddString(combo, "<new panel>");	
	CString num = "";
	int x = 0;
	int size = pCtrl->GetVisiblePanelCount();
	for(int n = 0; n != pCtrl->panels.size(); ++n){
		if(pCtrl->panels[n]->visible){
			x++;
			num.Format("%d", x);			
			m_pIndDlg->AddString(combo, "Panel " + num);
		}
	}	
	if(paramStr[0] == ""){
		combo->SetCurSel(0);
	}
	else{
		combo->SetCurSel(ownerPanel->index + 1);
	}
}

void CIndicator::EnumMATypes(CComboBox* combo){
	combo->ResetContent();
	m_pIndDlg->AddString(combo, "Simple");
	m_pIndDlg->AddString(combo, "Exponential");
	m_pIndDlg->AddString(combo, "Time Series");
	m_pIndDlg->AddString(combo, "Triangular");
	m_pIndDlg->AddString(combo, "Variable");
	m_pIndDlg->AddString(combo, "VIDYA");
	m_pIndDlg->AddString(combo, "Weighted");
	combo->SetCurSel(0);
}

UINT CIndicator::GetMAType(CComboBox* combo){
	UINT ret = indSimpleMovingAverage;
	CString type = "";
	combo->GetWindowText(type);
	if(pCtrl->CompareNoCase(type, "Simple"))
		ret = indSimpleMovingAverage;
	else if(pCtrl->CompareNoCase(type, "Exponential"))
		ret = indExponentialMovingAverage;
	else if(pCtrl->CompareNoCase(type, "Time Series"))
		ret = indTimeSeriesMovingAverage;
	else if(pCtrl->CompareNoCase(type, "Triangular"))
		ret = indTriangularMovingAverage;
	else if(pCtrl->CompareNoCase(type, "Variable"))
		ret = indVariableMovingAverage;
	else if(pCtrl->CompareNoCase(type, "VIDYA"))
		ret = indVIDYA;
	else if(pCtrl->CompareNoCase(type, "Weighted"))
		ret = indWeightedMovingAverage;
	return ret;
}

LPCTSTR CIndicator::MATypeToStr(UINT type){
	LPCTSTR ret = "Simple";
	
	switch(type){
	case indSimpleMovingAverage:
		ret = "Simple";
		break;
	case indExponentialMovingAverage:
		ret = "Exponential";
		break;
	case indTimeSeriesMovingAverage:
		ret = "Time Series";
		break;
	case indTriangularMovingAverage:
		ret = "Triangular";
		break;
	case indVariableMovingAverage:
		ret = "Variable";
		break;
	case indVIDYA:
		ret = "VIDYA";
		break;
	case indWeightedMovingAverage:
		ret = "Weighted";
		break;
	}

	return ret;
}


CSeries* CIndicator::EnsureSeries(LPCTSTR name)
{
	CSeries* series = NULL;

	series = GetSeries(name);
	bool twinCreated = series == NULL;

	if(!series){
  
		pCtrl->AddNewSeries(name, OBJECT_SERIES_LINE, ownerPanel->index);
	}
	
	series = GetSeries(name);
	if(series){
		series->Clear();
		series->linkTo = szName;
		series->isTwin = twinCreated;	
		//series->seriesType = OBJECT_SERIES_INDICATOR;
	}

	return series;

}




bool CIndicator::HasCircularReference(CSeries* series)
{

	int p1 = 0, p2 = 0;
	for(p1 = 0; p1 != series->paramStr.size(); ++p1){
		if(series->paramStr[p1] != ""){
			CSeries* nextSeries = GetSeries(series->paramStr[p1]);
			if(NULL != nextSeries){				
				for(p2 = 0; p2 != nextSeries->paramStr.size(); ++p2){
					if(pCtrl->CompareNoCase(nextSeries->paramStr[p2], szName) ||
						pCtrl->CompareNoCase(nextSeries->linkTo, szName)){
						paramStr[p1] = "";
						return true;
					}
				}
				if(nextSeries != this && nextSeries != NULL){
					if(HasCircularReference(nextSeries)){
						return true;
					}
				}
			}
		}
	}

	return false;
}

bool CIndicator::EnsureField(CField *field, LPCTSTR name)
{
	CString add = name;
	if(field == NULL){
		SeriesPoint sp;
		sp.jdate = 0;
		sp.value = 0;
		data_slave.push_back(sp);
		ProcessError("Missing source field " + add + " for indicator " + szName);
		return false;
	}
	else{
		return true;
	}
}

void CIndicator::UpdateSeries(CSeries *series)
{
	// Update linked indicators		
	CSeries* pLink = NULL;
	int count1 = pCtrl->panels.size();
	for(int p = 0; p != count1; ++p){
		int count2 = pCtrl->panels[p]->series.size();
		for(int s = 0; s != count2; ++s){
			pLink = pCtrl->panels[p]->series[s];			
			if(pLink->seriesType == OBJECT_SERIES_INDICATOR){
				CIndicator* pInd = (CIndicator*)pLink;
				for(int j = 0; j != pLink->paramStr.size(); ++j){
					if(pCtrl->CompareNoCase(pLink->paramStr[j], szName)){							
						pInd->Calculate();
						pInd->UpdateSeries(pLink);
					}
				}
			}
		}
	}
}


// The dialog's cancel button was clicked.
// If the indicator has not yet been calcualted
// then remove the indicator series.
void CIndicator::OnCancleDialog()
{
	if(!calculated && data_master.size() == 0){
		pCtrl->FireOnDialogHiden();
		ProcessError("remove indicator");
	}
}

