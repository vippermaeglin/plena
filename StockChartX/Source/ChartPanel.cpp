// ChartPanel.cpp: implementation of the CChartPanel class.
//
//////////////////////////////////////////////////////////////////////
 
#include "stdafx.h"
#include "ChartPanel.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CChartPanel::CChartPanel()
{
	hasPrice = false;
	staticScale = false;
	max = 0;
	min = 0;
	y1 = 0;
	y2 = 0;
	yOffset = 0;
	visible = false;
	invalidated = true; // Force initial paint
	connected = false;
	pYScaleOwner = NULL;
	yScaleRect = CRect(0,0,0,0);
}

CChartPanel::~CChartPanel()
{

}

void CChartPanel::Connect(CStockChartXCtrl *Ctrl)
{
	pCtrl = Ctrl;
	connected = true;
}
void CChartPanel::InvalidateXOR()
{	
	// Forward messages
  int n;
	for(n = 0; n != series.size(); ++n){
		//series[n]->InvalidateXOR();
	}

	for(n = 0; n != objects.size(); ++n){
		//objects[n]->InvalidateXOR();
	}

	for(n = 0; n != lines.size(); ++n){
		lines[n]->InvalidateXOR();
	}
}
void CChartPanel::OnPaintXOR(CDC *pDC)
{
	// Forward messages
  int n;
	for(n = 0; n != series.size(); ++n){
		series[n]->OnPaintXOR(pDC);
	}

	// Forward XOR for top z-ordered drawings only
	for(n = 0; n != objects.size(); ++n){
		if(objects[n]->zOrder == zOrderFront) objects[n]->OnPaintXOR(pDC);
	}

	for(n = 0; n != lines.size(); ++n){
		if(lines[n]->zOrder == zOrderFront) lines[n]->OnPaintXOR(pDC);
	}

}

void CChartPanel::OnMessage(LPCTSTR MsgGuid, int MsgID)
{
  int n;
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnMessage(MsgGuid, MsgID);
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->OnMessage(MsgGuid, MsgID);
	}
}
/////////////////////////////////////////////////////////////////////////////


// Paints this chart panel
void CChartPanel::OnPaint(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	//printf("\n\tPanel->OnPaint()");
#endif
	if(!pDC) return;

	if(invalidated)
	{
#ifdef _CONSOLE_DEBUG
	//printf("\t INVALIDATED");
#endif
		// Redraw everything
		invalidated = false;
	}
	else
	{
		// Just send the message out		
		return;
	}	

	CRect rect;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	rect.top = y1;
	rect.bottom = y2;
	rect.left = 0;
	rect.right = pCtrl->width;	
	

	CBrush* br = new CBrush(pCtrl->backColor);
	
	panelRect = rect; // Public copy
	
	// Gradient background, MTR 7/29/04
	if(pCtrl->backGradientTop != 0){
		if(!(index  % 2)){
			pCtrl->FadeVert(pDC, 
				pCtrl->backGradientTop,
				pCtrl->backGradientBottom,
				rect);
		}
		else{
			pCtrl->FadeVert(pDC, 
				pCtrl->backGradientBottom,
				pCtrl->backGradientTop,
				rect);
		}
	}
	else{
		pDC->FillRect(&rect,br);
	}



	delete br;

	DrawYScale(pDC);
	
	// Bottom z-ordered drawings
  int n;
	for(n = 0; n != lines.size(); ++n){		
		if(lines[n]->zOrder == zOrderBack) {
			lines[n]->OnPaint(pDC);
		}
	}

	for(n = 0; n != objects.size(); ++n)
	{
		if(objects[n]->zOrder == zOrderBack) objects[n]->Show();
	}

	for(n = 0; n != textAreas.size(); ++n)
	{
		if(textAreas[n]->zOrder == zOrderBack) textAreas[n]->OnPaint(pDC);		
	}

	// Forward series painting message
	for(n = 0; n != series.size(); ++n)
	{
		series[n]->OnPaint(pDC);
	}


	// Top z-ordered drawings
	for(n = 0; n != lines.size(); ++n){	
		if(lines[n]->zOrder == zOrderFront) {
			lines[n]->OnPaint(pDC);
		}
	}

	for(n = 0; n != objects.size(); ++n)
	{
		if(objects[n]->zOrder == zOrderFront) objects[n]->Show();
	}

	for(n = 0; n != textAreas.size(); ++n)
	{
		if(textAreas[n]->zOrder == zOrderFront) textAreas[n]->OnPaint(pDC);		
	}



	// Requested by Malloggi 7/19/04 - RDG
	// Do not draw precision scale if volume is in this panel
	CString volume = "";
	long decimals = pCtrl->decimals;
	/*for(n = 0; n != series.size(); ++n){
		volume = series[n]->szName;
		volume.MakeLower();
		if( volume.Find(".volume") != -1 || volume == "volume" )
		{
			decimals = 0;
			break;
		}
	}*/


	// Print panel titles?
	if(pCtrl->displayTitles)
	{
		CRect rect;
		CPoint point = pCtrl->m_point;
		
		//Get font info
		CFont newFont;			
		newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial Black"), pDC);	
		TEXTMETRIC tm;
		CFont* pOldFont = pDC->SelectObject(&newFont);
		pDC->GetTextMetrics(&tm);
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		int nAvgWidth = (int)((tm.tmAveCharWidth) * 1.5);
//	End Of Revision
		int nCharHeight = 1 + (tm.tmHeight);

		rect.left = 1;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (long)
		rect.top = (long)y1;
//	End Of Revision
		rect.bottom = pCtrl->height;
		rect.right = pCtrl->width - pCtrl->yScaleWidth;
		
		if(pCtrl->yAlignment == LEFT){
			rect.left = pCtrl->yScaleWidth + 1;
			rect.right = pCtrl->width;
		}
		
		// Draw a horizontal line seperator?
		if(pCtrl->m_horzLines && this->y1 != pCtrl->panels[0]->y1){
			//if(pCtrl->yOffset > 0){ // Draw a border
				pCtrl->yOffset = nCharHeight - 2;
				CPen border(0,1,pCtrl->foreColor);
				CPen* pOldPen = pDC->SelectObject(&border);
				//pDC->MoveTo(rect.left, rect.top + pCtrl->yOffset);
				//pDC->LineTo(rect.right, rect.top + pCtrl->yOffset);
				//pDC->MoveTo(rect.left, rect.top);
				//pDC->LineTo(rect.right, rect.top);
				pDC->Draw3dRect(CRect(rect.right,rect.top,rect.left,rect.top+2),RGB(170,170,170),pCtrl->foreColor);
				pDC->SelectObject(pOldPen);
				border.DeleteObject();
			//}
		}
		
		// build up title string
		OLE_COLOR clr = 255;
		int found = 0;
		CString value = "";
		CString title = "";
		for(int n = 0; n != series.size(); ++n){
			if(series[n]->seriesVisible){
				title = series[n]->szName;
				title.MakeLower();
				found = title.Find(".");
				if(found > 0){
					int found2 = title.Find(".vol");
					if(found2 == -1){
						title = title.Mid(0, found);
						clr = series[n]->lineColor;
					}
					else{
						found = -1;
						clr = series[n]->lineColor;
					}
				}
				else{
					clr = series[n]->lineColor;
				}
				CString temp = series[n]->szName;
#ifdef _CONSOLE_DEBUG
				//printf ("\n\n\tNomechartpanel= %s ", temp);
#endif
				temp.MakeLower();
				if((found > 0 && temp.Find(".close") > 0) || found == -1){
					

					// Update 10/26/04 by Eugen Rata
					double val = -1;
					if ( pCtrl->endIndex - 1 < (int)series[n]->data_master.size() ) {
						
						if(series[n]->data_master.size() > (size_t)pCtrl->endIndex - 1) // Added 4/3/06
						{
							val = series[n]->data_master[pCtrl->endIndex - 1].value;

							if(series[n]->data_master[pCtrl->endIndex - 1].value == NULL_VALUE){
								for(int j = pCtrl->endIndex - 1; j != 1; --j){
									if(series[n]->data_master[j].value != NULL_VALUE){
										val = series[n]->data_master[j].value;
										break;
									}
								}
							}
						}

						try{
							value.Format("%.*f", decimals, val);
						}
						catch(...){}
						}
					else{
						OutputDebugString("index error\n");
					}


					/*
					// Get the last visible value
					double val = series[n]->data_master[pCtrl->endIndex - 1].value;
					if(series[n]->data_master[pCtrl->endIndex - 1].value == NULL_VALUE){
						for(int j = pCtrl->endIndex - 1; j != 1; --j){
							if(series[n]->data_master[j].value != NULL_VALUE){
								val = series[n]->data_master[j].value;
								break;
							}
						}
					}
					try{
						value.Format("%.*f", decimals, val);
					}
					catch(...){}
						*/
					if(value == "-987654321.00") value = "";			
					if(series[n]->szTitle != "") title = series[n]->szTitle;
					title.MakeUpper();	
					CString periodError = "Invalid Periods for " + title;
					if(pCtrl->m_Language==1)periodError = "Periodicidade inválida para " + title;
					//Compare Indicator's names by dictionary
					if((series[n]->seriesType==OBJECT_SERIES_INDICATOR||series[n]->seriesType==OBJECT_SERIES_LINE) && pCtrl->m_Language==1){
						if(title.Find("MA")==0 && title.Find("MACD")<0)title.Replace("MA","MM");
						else if(title.Find("RSI")==0 )title.Replace("RSI","IFR");
						else if(title.Find("SO")==0)title.Replace("SO","OE");
						else if(title.Find("SMI")==0)title.Replace("SMI","ME");
					}
					//Don't show candle or twin lines titles (except for MACD)
					if(( (series[n]->seriesType == OBJECT_SERIES_LINE) &&(( (title.Find("BB")!=-1)&&((title.Find("BOTTOM")!=-1)||(title.Find("INFERIOR"))))||								
								( ( (title.Find("MME")!=-1)||(title.Find("MAE")!=-1)||(title.Find("HILO")!=-1) )&&((title.Find("TOP")!=-1)||(title.Find("SUPERIOR"))) )|| (((title.Find("RSI")!=-1)||(title.Find("IFR")!=-1))&&((title.Find("70")!=-1)||(title.Find("30")))) ))/* || (series[n]->seriesType == OBJECT_SERIES_CANDLE)*/) 
					{								
								continue;
					}
					else{
						if(value.GetLength() > 40) value = "0";
						if(pCtrl->m_yScaleDrawn||series[n]->ownerPanel->index==0){
							//title += " = " + value;	
							series[n]->dataError = false;
						}
						else {
#ifdef _CONSOLE_DEBUG
							printf("\nERROR m_yScaleDrawn!!!");
#endif
							series[n]->dataError = true;
							title = periodError;
						}
					}
					//if(n != series.size() - 1 && found == -1) title += ", ";
					if(pCtrl->CompareNoCase(title,pCtrl->m_symbol) && pCtrl->savingBitmap){
						CString titleAux;
						switch(pCtrl->m_Periodicity){
							case 1: //Secondly
								break;
							case 2: //Minutely
								titleAux.Format("(%dm)",pCtrl->m_BarSize);
								title+=titleAux;
								break;
							case 3: //Hourly
								titleAux.Format("(%dh)",pCtrl->m_BarSize);
								title+=titleAux;
								break;
							case 4: //Daily
								title+="(D)";
								break;
							case 5: //Weekly
								pCtrl->m_Language==1?title+="(S)":title+="(W)";
								break;
							case 6: //Month
								title+="(M)";
								break;
							case 7: //Year
								pCtrl->m_Language==1?title+="(A)":title+="(Y)";
								break;

						}
						//External title sufix:
						title += pCtrl->SaveImageTitle();
					}
					OLE_COLOR oldColor = pDC->SetTextColor(clr);
					//pDC->DrawText(title, -1, &rect, DT_WORDBREAK | DT_LEFT);
					CRectF rectTextF = CRectF(rect.left,rect.top+1,rect.right,rect.bottom);
					pCtrl->pdcHandler->DrawText(title,rectTextF,"Arial Rounded MT Bold",12.0F,DT_LEFT,clr,255,pDC);

					pDC->SetTextColor(oldColor);
					rect.left += nAvgWidth * title.GetLength() + 10;
				}
			}
		}
		pDC->SelectObject(pOldFont);
		newFont.DeleteObject();
	}

}
/////////////////////////////////////////////////////////////////////////////

// Returns a pointer to a specified CSeries object
CSeries* CChartPanel::GetSeries(LPCTSTR name)
{
	CSeries* ret = NULL;
	for(int n = 0; n != series.size(); ++n){		
		if(pCtrl->CompareNoCase(name, series[n]->szName)){
			ret = series[n];
			break;
		}
	}
	return ret;
}

// Returns an index to a specified CSeries object
long CChartPanel::GetSeriesIndex(LPCTSTR name)
{
	CString name1, name2;
	name1 = name;
	name1.MakeLower();
	long ret = -1;
	for(int n = 0; n != series.size(); ++n){
		name2 = series[n]->szName;
		name2.MakeLower();
		if(name1 == name2){
			ret = n;
			break;
		}
	}
	return ret;
}

// Deletes a specified CSeries object
int CChartPanel::deleteSeries(LPCTSTR name)
{
	int size = series.size();
	for(int n = 0; n != size; ++n){		
		if(pCtrl->CompareNoCase(name, series[n]->szName)){
			series[n]->szName = "";
			delete series[n];			
			series[n] = NULL;
			std::vector<CSeries*>::iterator itr = series.begin() + n;
			CSeries* elem = *itr;
			series.erase(itr);
			break;
		}
	}
	return series.size();
	OnUpdate();
}

// A CSeries object has been added or removed from the 
// series vector. This is the only function in this 
// entire component that is allowed to initiate a 
// deletion of a chart panel.
void CChartPanel::OnUpdate()
{	
	 
	// get size
	int size = series.size();
	if(size == 0) return;
	
	// order series
	for(int n = 0; n != size; ++n){
		series[n]->index = n;
	}

	// is this panel needed anymore?
	// NOTE: this code is no longer used
	if(size == 0){

		// delete objects		
		size = objects.size();
		for(int j = 0; j != size; ++j){
			try{				
				//delete objects[j];
				objects[j] = NULL;
			}
			catch(...){}
		}

		this->y1 = 0;
		this->y2 = 0;
		this->visible = false;		

		// recycle this object
		pCtrl->RecycleChartPanel(index);
	}

}

void CChartPanel::OnDoubleClick(CPoint point)
{
	// Forward message
  int n;
	for(n = 0; n != series.size(); ++n){
		series[n]->OnDoubleClick(point);
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnDoubleClick(point);
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->OnDoubleClick(point);
	}
	for(n = 0; n != textAreas.size(); ++n){
		textAreas[n]->OnDoubleClick(point);
	}
}

void CChartPanel::OnMouseMove(CPoint point)
{
	// Forward message
  int n;
	for(n = 0; n != series.size(); ++n){
		//series[n]->OnMouseMove(point);
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnMouseMove(point);
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->OnMouseMove(point);
	}
	for(n = 0; n != textAreas.size(); ++n){
		textAreas[n]->OnMouseMove(point);
	}
}

void CChartPanel::OnRButtonDown(CPoint point)
{
	// Forward message
  int n;
	for(n = 0; n != series.size(); ++n){
		series[n]->OnRButtonDown(point);
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnRButtonDown(point);
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->OnRButtonDown(point);
	}
}

void CChartPanel::OnRButtonUp(CPoint point)
{
	// Forward message
  int n;
	try{
		for(n = 0; n != series.size(); ++n){
			if(n > series.size()-1 || series.size() == 0) break;
			series[n]->OnRButtonUp(point);
		}
		for(n = 0; n != objects.size(); ++n){
			if(n > objects.size()-1 || objects.size() == 0) break;
			objects[n]->OnRButtonUp(point);
		}
		for(n = 0; n != lines.size(); ++n){
			if(n > lines.size()-1 || lines.size() == 0) break;
			lines[n]->OnRButtonUp(point);
		}
		for(n = 0; n != textAreas.size(); ++n){
			if(n > textAreas.size()-1 || textAreas.size() == 0) break;
			textAreas[n]->OnRButtonUp(point);
		}
	}
	catch(...){}
}
void CChartPanel::OnLButtonDown(CPoint point)
{
	// Forward message
  int n;
	for(n = 0; n != series.size(); ++n){
		series[n]->OnLButtonDown(point);
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnLButtonDown(point);
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->OnLButtonDown(point);
	}
	for(n = 0; n != textAreas.size(); ++n){
		textAreas[n]->OnLButtonDown(point);
	}
}

void CChartPanel::OnLButtonUp(CPoint point)
{

	/*
	CString temp;
	CRect rect;
	rect.left = point.x;
	rect.top = point.y;
	rect.bottom = point.y + 60;
	rect.right = point.x + 500;	
	pCtrl->m_memDC.SetBkMode(TRANSPARENT);
	temp.Format("%f",GetReverseX(point.x));
	pCtrl->m_memDC.DrawText("X=" + temp,&rect,DT_SINGLELINE);
	rect.top = point.y + 8;
	for(int n = 0; n != series.size(); ++n){
		int x = GetReverseX(point.x);
		double val = series[n]->GetValue(x);
		temp.Format("%f",val);
		pCtrl->m_memDC.DrawText(series[n]->szName + "=" + temp,&rect,DT_SINGLELINE);			
		rect.top += 8;
	}
	pCtrl->RePaint();
	*/

	pYScaleOwner = NULL;

	// Forward message
	//pCtrl->UnSelectAll();
  int n;
	/*for(n = 0; n != lines.size(); ++n){
		lines[n]->OnLButtonUp(point);
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->OnLButtonUp(point);
	}
	for(n = 0; n != textAreas.size(); ++n){
		textAreas[n]->OnLButtonUp(point);
	}
	for(n = 0; n != series.size(); ++n){
		series[n]->OnLButtonUp(point);
	}*/

	for (n = 0; n != series.size(); ++n){
		series[n]->OnLButtonUp(point);
	}
	for (n = 0; n != objects.size(); ++n){
		objects[n]->OnLButtonUp(point);
	}
	for (n = 0; n != lines.size(); ++n){
		lines[n]->OnLButtonUp(point);
	}
	for (n = 0; n != textAreas.size(); ++n){
		textAreas[n]->OnLButtonUp(point);
	}

}

// Flags the panel for complete re-drawing
void CChartPanel::Invalidate()
{
	invalidated = true;
}

// Returns the maximum number of values in data_slave
int CChartPanel::GetMaxSlaveSize()
{
	int nMax = 0;
  int n;
	for(n = 0; n != series.size(); ++n){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		if((int)series[n]->data_slave.size() > nMax){
//	End Of Revision
			nMax = series[n]->data_slave.size();
		}
	}
	return nMax;
}

// Returns the minimum number of values in data_slave
int CChartPanel::GetMinSlaveSize()
{
	int nMin = 0;
  int n;
	for(n = 0; n != series.size(); ++n){
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
		if((int)series[n]->data_slave.size() < nMin || nMin == 0){
//	End Of Revision
			nMin = series[n]->data_slave.size();
		}
	}
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	return (int)min;
//	End  of Revision
}

// Get the max/min for scaling
void CChartPanel::GetMaxMin()
{

#ifdef _CONSOLE_DEBUG
	//printf("\nGETMAXMIN()");
#endif
	//VINICIUS UPDATE: "hasPrice" must be verified always!
	hasPrice = false;
	// Semi-log max/min	
	if(pCtrl->scalingType == SEMILOG && min > 0){				
		for(int n = 0; n != series.size(); ++n){
			CString lName = series[n]->szName;
			lName.MakeLower();
			int found = lName.Find(".close", 0);
			if(found != -1){
				hasPrice = true;	
				break;
			}
		}
	}
	if(staticScale) 
	{	
		return;
	}

	ASSERT(pCtrl->startIndex > -1);

	max = -1000000000;
	min = 1000000000;
	double nHeight = (y2 - y1);
	double value = 0;
	int endS = series.size();
	for(int n = 0; n != endS; ++n){
		if(series[n]->szName.Find(".volume")>0)continue;
		int end = pCtrl->endIndex;
		if (series[n]->data_slave.size() < (unsigned int)end)
			end = series[n]->data_slave.size();
		if(end == 0) return;
		if(pCtrl->startIndex > end) pCtrl->startIndex = end;		
		for(int j = pCtrl->startIndex; j != end; ++j)
		{
			if(series[n]->data_slave[j].value != NULL_VALUE){
				if(series[n]->data_slave[j].value < min){
					min = series[n]->data_slave[j].value;
				}
				if(series[n]->data_slave[j].value > max){
					max = series[n]->data_slave[j].value;
				}
				if (series[n]->data_slave[j].value==0){
#ifdef _CONSOLE_DEBUG
					//printf("\n value=0");
#endif
				}
			}
		}
	}
	
#ifdef _CONSOLE_DEBUG
	//printf("\t max=%f min=%f endS=%d",max,min,endS);
#endif
	

	if(max == -1000000000){
		max = 1;
		min = 0;
	}


	slmax = log10(max);
	slmin = log10(min);
#ifdef _CONSOLE_DEBUG
	//printf("\n\n GetMaxMin() max = %d  min = %d slmax = %d slmin = %d", max, min, slmax, slmin);
#endif

}

// Input: Max of all data, min of all data, and current value
// Output: Normalized value between 1 and 0
double CChartPanel::Normalize(double value)
{
	if(pCtrl->scalingType == SEMILOG && min > 0 && hasPrice){
		return (value - slmin) / (slmax - slmin);
	}
	else{
		return (value - min) / (max - min);
	}
}

// Input: Max of all data, min of all data, and scaled value
// Output: Unscaled value restored between max and min
double CChartPanel::UnNormalize(double value)
{
	if(pCtrl->scalingType == SEMILOG && min > 0 && hasPrice){
#ifdef _CONSOLE_DEBUG
		//printf("\nUnNormalize() value=%f slMax=%f slMin=%f",value,slmax,slmin);
#endif
		return slmin + (value * (slmax - slmin));
	}
	else{
#ifdef _CONSOLE_DEBUG
		//printf("\nUnNormalize() value=%f max=%f min=%f",value,max,min);
#endif
		return min + (value * (max - min));
	}
}

// Input: Actual price value
// Output: Y coordinate for chart area
double CChartPanel::GetY(double value)
{
	int semlog=-1;
	double nHeight = 0;
	double ret = NULL_VALUE;
	double normalize;
//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
    nHeight = (double)(y2 - y1) -(int)yOffset; //9?
//	End of Revision
	if(pCtrl->scalingType == SEMILOG && min > 0 && hasPrice){
		ret = y1 + yOffset+ (nHeight - (nHeight * Normalize(log10(value))));
		normalize=Normalize(log10(value));
		semlog=1;
		//DEL
		/*if(ret > y2 - 5){
			ret = y2;
		}*/
	}
	else{
		semlog=0;
		normalize=Normalize(value);
		ret = y1 + yOffset + (nHeight - (nHeight * Normalize(value)));
	}
	//if(ret == y2) ret = y2 - 1;
	return ret; // yOffset added 2/1/2005
}

// Input: Actual index value
// Output: X coordinate for chart area
double CChartPanel::GetX(int period, bool offscreen /*=false*/)
{

	if(period < 0 && !offscreen) return 0;

	// Just return x-map if a CPriceStyle has already calculated
	// Updated 2/9/2006
	if (pCtrl->priceStyle != psStandard && pCtrl->priceStyle != psHeikinAshi && pCtrl->priceStyle != psHeikinAshiSmooth &&
		(int)pCtrl->xMap.size() > period && pCtrl->xMap.size() > 0 && period > 0)
	{
		if(pCtrl->xMap[period - 1] > 0){
			return pCtrl->xMap[period - 1];
		}
	}

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

// Reverses Y values set by GetY() back to 
// their original values (e.g. returns price
// or indicator value rather than Y pixel location)
double CChartPanel::GetReverseY(double value){

	double nHeight = 0;
	double ret = 0;

//	Revision 6/10/2004 made by Katchei
//	Addition of type cast (int)
	nHeight = ((y2 - y1 - yOffset));
//	End Of Revision
	if(pCtrl->scalingType == SEMILOG && min > 0 && hasPrice){
#ifdef _CONSOLE_DEBUG
		//printf("\nGetReverseY() value = %f",value);
#endif
		value = UnNormalize(1 - (value - y1 - yOffset) / nHeight);
#ifdef _CONSOLE_DEBUG
		//printf(" newValue = %f y1=%f yOS=%d nH=%d unNorm=%f",value,y1,yOffset,nHeight,(1 - (value - y1 - yOffset) / nHeight));
#endif
		if(/*value > 0 &&*/ max > 0){			
			ret =  pow((double)10, (double)value);
		}
	}
	else{
		ret = UnNormalize(1 - (value - y1 - yOffset) / nHeight);
	}
	return ret;
}

// Reverses X values set by GetX() back to 
// the array index.
double CChartPanel::GetReverseX(double value){

	double ret = 0;
	double nWidth = 0;
    double nSpacing = 0;
	long recCnt = pCtrl->GetSlaveRecordCount();	


	// Return x-map if a CPriceStyle has already calculated	
	if(pCtrl->priceStyle != psStandard && 
		pCtrl->xMap.size() > 1){
		double index = -1;
		for(int n = 0; n != pCtrl->xMap.size(); ++n){
			double v1 = 0;
			if(n > 0) v1 = pCtrl->xMap[n - 1];
			double v2 = pCtrl->xMap[n];
			if(n == 0 && value <= v2){
				return (int)floor(n + 0.5);
			}
			if(value >= v1 && value <= v2){
				return (int)floor(n + 0.5);
			}			
		}
		return index;
	}

	if(pCtrl->yAlignment == LEFT){
		value -= pCtrl->yScaleWidth + 5;
	}	
    nWidth = pCtrl->width - pCtrl->yScaleWidth - pCtrl->extendedXPixels;
    if(recCnt > 0) nSpacing = nWidth / (recCnt);
	double index = value / nSpacing; // period
	index = (int)floor(index + 0.5);
	return index;

}

void CChartPanel::Update()
{
	GetMaxMin();
  int n;	
	for(n = 0; n != series.size(); ++n){
		series[n]->Update();
	}
	for(n = 0; n != lines.size(); ++n){
		lines[n]->Update();
	}
	for(n = 0; n != textAreas.size(); ++n){
		textAreas[n]->Update();
	}
	for(n = 0; n != objects.size(); ++n){
		objects[n]->UpdatePoints();
	}
	
}


// Draws the y value panel and returns the memDC rect to update (if needed)
CRect CChartPanel::DrawYScale(CDC *pDC)
{
#ifdef _CONSOLE_DEBUG
	printf("\nDrawYScale");
#endif
	CRect retRect = CRect(0, 0, 0, 0);
	try{
		// Get the paint brushes ready...
		CPen pDark;
		CPen* pOldPen;
		CBrush* br;
		br = new CBrush(pCtrl->backColor);
		CBrush* pOldBrush = pDC->SelectObject(br);
		pDC->SetBkMode(TRANSPARENT);
		pDark.CreatePen(0, 1, pCtrl->foreColor);
		pDC->SelectObject(pOldBrush);
		br->DeleteObject();
		if (br) delete br;
		pOldPen = pDC->SelectObject(&pDark);



		pCtrl->m_yScaleDrawn = false;

		GetMaxMin();

		double smax = max, smin = min;

		if (pYScaleOwner){
#ifdef _CONSOLE_DEBUG
			printf("\thave pYScaleOwner");
#endif
			smax = pYScaleOwner->max;
			smin = pYScaleOwner->min;
		}

#ifdef _CONSOLE_DEBUG
		printf("\tGetMaxMin()smin=%f smax=%f",smax,smin);
#endif

		if (smax == smin || (smax == 0 && smin == 0)) {
			//	Get the y-scale size and position
			CRect rect = CRect(0, 0, 0, 0);
			if (pCtrl->yAlignment == LEFT){
				rect.left = 1;
				rect.right = pCtrl->yScaleWidth;
			}
			else{
				rect.left = pCtrl->width - pCtrl->yScaleWidth;
				rect.right = pCtrl->width + pCtrl->yScaleWidth;
			}
			rect.top = /*(long)*/y1;
			rect.bottom = /*(long)*/y2;

			yScaleRect = rect; // Public copy


			// Paint the y-scale background
			CBrush* brBk = new CBrush(pCtrl->backColor);
			if (pCtrl->backGradientTop != 0){
				if (!(index % 2)){
					pCtrl->FadeVert(pDC,
						pCtrl->backGradientTop,
						pCtrl->backGradientBottom,
						rect);
				}
				else{
					pCtrl->FadeVert(pDC,
						pCtrl->backGradientBottom,
						pCtrl->backGradientTop,
						rect);
				}
			}
			else{
				pDC->FillRect(&rect, brBk);
			}
			brBk->DeleteObject();
			if (brBk) delete brBk;


			//	Draw the border
			if (pCtrl->yAlignment == LEFT){
				pDC->MoveTo(rect.right, rect.top);
				pDC->LineTo(rect.right, rect.bottom);
				rect.right -= 1;
			}
			else{
				pDC->MoveTo(rect.left, rect.top);
				pDC->LineTo(rect.left, rect.bottom);
				rect.left += 1;
			}

			retRect = rect;

#ifdef _CONSOLE_DEBUG
			printf("\tReturning Blank!");
#endif
			return retRect;
		}


		// Get font info
		CFont newFont;

		// Font size changed from 88 to 80 (VALUE_FONT_SIZE) - RG 7/23/04
		newFont.CreatePointFont(VALUE_FONT_SIZE - 6, _T("Arial"), pDC);
		TEXTMETRIC tm;
		CFont* pOldFont = pDC->SelectObject(&newFont);
		pDC->GetTextMetrics(&tm);
		//	Revision 6/10/2004 made by Katchei
		//	Addition of type cast (int)
		int nAvgWidth = (int)(1 + (tm.tmAveCharWidth * 1.2)); // Changed from 0.8 RG 7/23/04
		//	End Of Revision
		int nCharHeight = 1 + (tm.tmHeight);

		// Added by RG 7/23/04
		yOffset = pCtrl->yOffset;
		if (yOffset > 0){ // A title border will be drawn
			yOffset = nCharHeight/* - 5*/;
		}


		// Get max and min pixels from real data values for this panel
		double ymax = 0, ymin = 0;
		if (pYScaleOwner){
			ymax = pYScaleOwner->GetY(smax);
			ymin = pYScaleOwner->GetY(smin);
		}
		else{
			ymax = GetY(smax);
			ymin = GetY(smin);
		}








		//	Get the y-scale size and position
		CRect rect = CRect(0, 0, 0, 0);
		if (pCtrl->yAlignment == LEFT){
			rect.left = 1;
			rect.right = pCtrl->yScaleWidth;
		}
		else{
			rect.left = pCtrl->width - pCtrl->yScaleWidth;
			rect.right = pCtrl->width + pCtrl->yScaleWidth;
		}
		rect.top = /*(long)*/y1;
		rect.bottom = /*(long)*/y2;

		yScaleRect = rect; // Public copy


		// Paint the y-scale background
		CBrush* brBk = new CBrush(pCtrl->backColor);
		if (pCtrl->backGradientTop != 0){
			if (!(index % 2)){
				pCtrl->FadeVert(pDC,
					pCtrl->backGradientTop,
					pCtrl->backGradientBottom,
					rect);
			}
			else{
				pCtrl->FadeVert(pDC,
					pCtrl->backGradientBottom,
					pCtrl->backGradientTop,
					rect);
			}
		}
		else{
			pDC->FillRect(&rect, brBk);
		}
		brBk->DeleteObject();
		if (brBk) delete brBk;


		//	Draw the border
		if (pCtrl->yAlignment == LEFT){
			pDC->MoveTo(rect.right, rect.top);
			pDC->LineTo(rect.right, rect.bottom);
			rect.right -= 1;
		}
		else{
			pDC->MoveTo(rect.left, rect.top);
			pDC->LineTo(rect.left, rect.bottom);
			rect.left += 1;
		}
		// Large numbers set by GetMaxMin() when an error is encountered
		if (smax == -1000000000 && smin == 1000000000) {

#ifdef _CONSOLE_DEBUG
			printf("\tRETURNED 0!");
#endif
			return CRect(0, 0, 0, 0);
		}
		retRect = rect;




		// Get ready for drawing text
		CString szValue = "";
		CRect textRect = rect;
		pDC->SetTextColor(pCtrl->foreColor);
		if (pCtrl->yAlignment == LEFT)
			textRect.left = 1;
		else
			textRect.left += 1;
		textRect.right = textRect.left + pCtrl->yScaleWidth;
		pDC->SelectObject(pOldFont);
		pOldFont = pDC->SelectObject(&newFont);
		double textY = 0, last = 0;

		CString volume = "";
		long decimals = pCtrl->decimals;
		bool isVolume = false;
		for (int n = 0; n != series.size(); ++n){
			volume = series[n]->szName;
			volume.MakeLower();
			if (((volume.Find(".volume") == -1 && volume.Find("vol") == 0) || volume.Find("a/d") != -1 || volume.Find("obv") != -1) && index != 0)
			{
				isVolume = true;
				//decimals = 0;	
				smin = 0;
				break;
			}
		}

		// Get ready for drawing horizontal y scale lines
		pDC->SelectObject(pOldPen);
		CPen pGrid(2, 1, pCtrl->gridColor);
		pOldPen = pDC->SelectObject(&pGrid);

		//bool isVolume = (decimals == 0 && pCtrl->m_volumePostfix != "");
		// Paint y scale from bottom up	
		double minValue = smin, maxValue = smax; // Max/min values
		int maxLabels = (ymin - ymax) / (nCharHeight * 2); // How many lables to display


		// Enusre at least 4 labels
		if (maxLabels < 4) maxLabels = 4;

		double gStart = rect.bottom, gEnd = rect.top;
		double vStart = minValue;
		double vEnd = maxValue;
		int vSteps = maxLabels;

		double vRealStart;
		double vRealStep;

		// Calculate the grid
		GridScale(vStart, vEnd, vSteps, &vRealStart, &vRealStep);
		double k = (gEnd - gStart) / (vRealStep * (vSteps - 1));
		//if (pCtrl->scalingType != LINEAR && vRealStart < 2.0)vRealStart = 2.0;
		last = 9999999999.9;

#ifdef _CONSOLE_DEBUG
		printf("\nGridScale(vRealStart=%f,vRealStep=%f)",vRealStart,vRealStep);
#endif

		for (double i = 0; i < vSteps; i++)
		{
			double y = gStart + (i * vRealStep) * k;
			double labelValue = vRealStart + (vRealStep * i);

			if (pYScaleOwner){
				textY = pYScaleOwner->GetY(labelValue);
#ifdef _CONSOLE_DEBUG
				//printf("\nPYSCALEOWNER!!!");
#endif
			}
			else
				textY = GetY(labelValue);

			if (i == 1){
#ifdef _CONSOLE_DEBUG
				printf("\nDrawYScale() label=%f y=%f", labelValue, textY);
#endif
			}

			// Center the text in the middle of the horizontal line
			textRect.top = (int)(textY - (nCharHeight / 2));
			textRect.bottom = (int)(textY - (nCharHeight / 2)) + nCharHeight;
			if (last - (double)textRect.top > 5.0 || textRect == 0 || pCtrl->scalingType == LINEAR){
				if ((double)(textRect.top + 5) > y1 && textRect.top + nCharHeight < rect.bottom)
				{
					// Draw the text label
					szValue.Format("%.*f", decimals, labelValue);
					CString Symbol = pCtrl->m_symbol;
					// inserir os ativos que se deseja normalizar a ordem de grandeza de todos indicadores
					if (isVolume || Symbol == "IBOV") {
						decimals = 0;
						//labelValue/=1000000;
						//MODIFICADO POR FILIPE NOGUEIRA DIA 24/01/2014
						//{
						if (labelValue >= 1000000000)
						{

							labelValue = labelValue / 1000000000;
							szValue.Format("%.*f", decimals, labelValue);
							szValue += " B";
						}
						else if (labelValue >= 1000000)
						{

							labelValue = labelValue / 1000000;
							szValue.Format("%.*f", decimals, labelValue);
							szValue += " M";
						}
						else if (labelValue >= 1000)
						{

							labelValue = labelValue / 1000;
							szValue.Format("%.*f", decimals, labelValue);
							szValue += " K";
						}
						else
						{
							szValue.Format("%.*f", decimals, labelValue);
						}
						//}
						//szValue.Format("%.*f", decimals, labelValue);
						//szValue += " " + pCtrl->m_volumePostfix;
#ifdef _CONSOLE_DEBUG

						//printf("\nChartPanel::DrawYScale() labelvalue=%f texY=%f szValue=%s title=%s ",labelValue,textY,szValue, pCtrl->m_symbol);
#endif
					}
					//pDC->DrawText(szValue, -1, &textRect, DT_SINGLELINE | DT_CENTER);				
					CRectF rectTextF = CRectF(textRect.left, textRect.top, textRect.right, textRect.bottom);
					pCtrl->pdcHandler->DrawText(szValue, rectTextF, "Arial Rounded MT", 11.5F, DT_CENTER, pCtrl->foreColor, 255, pDC);

					// Show the Y grid?
					if (pCtrl->showYGrid)
					{
						if (pCtrl->yAlignment == LEFT)
						{
							pDC->MoveTo(rect.right + 2, (int)textY);
							pDC->LineTo(pCtrl->width, (int)textY);
						}
						else
						{
							pDC->MoveTo(0, (int)textY);
							pDC->LineTo(rect.left - 2, (int)textY);
						}
					}

					last = textRect.top;

				}
			}

		}


		// Show the X grid? 	
		if (pCtrl->showXGrid){
			//Draw vertical value lines in chart area
			for (int x = 0; x != pCtrl->xGridMap.size(); ++x)
			{
				double yval1 = 0, yval2 = 0;
				if (pYScaleOwner){
					yval1 = y1;
					yval2 = y2;
				}
				else{
					yval1 = y1;
					yval2 = y2;
				}


				if ((pCtrl->yAlignment == RIGHT && ((int)pCtrl->xGridMap[x] < pCtrl->width - pCtrl->yScaleWidth)) ||
					(pCtrl->yAlignment == LEFT && ((int)pCtrl->xGridMap[x] > pCtrl->yScaleWidth)))
				{
					pDC->MoveTo((int)pCtrl->xGridMap[x], (int)yval1);
					pDC->LineTo((int)pCtrl->xGridMap[x], (int)yval2); // Updated 1/18/08
				}
			}
		}
		pDC->SelectObject(pOldPen);

		//9/18/08 paint y scale lines
		pDC->SelectObject(pOldPen);
		CPen pYGrid(1, 1, pCtrl->foreColor);
		pOldPen = pDC->SelectObject(&pYGrid);
		double prevTextY = 0, step = 0;
		for (int j = 0; j < vSteps; j++)
		{
			double y = gStart + (j * vRealStep) * k;
			double labelValue = vRealStart + (vRealStep * j);

			if (pYScaleOwner)
				textY = pYScaleOwner->GetY(labelValue);
			else
				textY = GetY(labelValue);

			if (prevTextY > 0)
				step = (prevTextY - textY) / 2;
			prevTextY = textY;

			textRect.top = (int)(textY - (nCharHeight / 2));
			textRect.bottom = (int)(textY - (nCharHeight / 2)) + nCharHeight;
			if (last - textRect.top > 5 || textRect == 0 || pCtrl->scalingType == LINEAR){
				if (textRect.top + 5 > y1 && textRect.top + nCharHeight < rect.bottom)
				{
					if (pCtrl->yAlignment == LEFT)
					{
						pDC->MoveTo(rect.right - 6, (int)textY);
						pDC->LineTo(rect.right, (int)textY);
						if (step > 0)
						{
							//Show middle values:
							//pDC->MoveTo(rect.right,(int)textY + (step));
							//pDC->LineTo(rect.right - 3,(int)textY + (step));
						}
					}
					else
					{
						pDC->MoveTo(rect.left, (int)textY);
						pDC->LineTo(rect.left + 6, (int)textY);
						if (step > 0)
						{
							//Show middle values:
							//pDC->MoveTo(rect.left,(int)textY + (step));
							//pDC->LineTo(rect.left + 3,(int)textY + (step));
						}
					}
				}
			}
			last = textRect.top;

		}





		// Draw horizontal lines (if any)
		int count = horizontalLines.size();
		if (count > 0)
		{
			CPen pHorz(0, 1, pCtrl->foreColor);
			pOldPen = pDC->SelectObject(&pHorz);
			if (pCtrl->yAlignment == LEFT)
			{
				rect.left = pCtrl->yScaleWidth;
				rect.right = pCtrl->width;
			}
			else
			{
				rect.left = pCtrl->width - pCtrl->yScaleWidth;
				rect.right = pCtrl->width + pCtrl->yScaleWidth;
			}
			for (double y = 0; y != count; ++y)
			{
				//	Revision 6/10/2004 made by Katchei
				//	Addition of type cast (int)
				if (horizontalLines[y] != -1)
				{

					double yval = 0;
					if (pYScaleOwner){
						yval = pYScaleOwner->GetY(horizontalLines[y]);
					}
					else{
						yval = GetY(horizontalLines[y]);
					}

					if (pCtrl->yAlignment == LEFT){
						pDC->MoveTo(rect.left, (int)yval);
						pDC->LineTo(rect.right, (int)yval);
					}
					else{
						pDC->MoveTo(0, (int)yval);
						pDC->LineTo(rect.left, (int)yval);
					}
					//	End Of Revision
				}
			}
			pDC->SelectObject(pOldPen);
			pHorz.DeleteObject();
		}


		// Clean up
		pDC->SelectObject(pOldFont);
		newFont.DeleteObject();
		pDark.DeleteObject();
		pGrid.DeleteObject();
		pYGrid.DeleteObject();


		pCtrl->m_yScaleDrawn = true;
	}
	catch (...){
#ifdef _CONSOLE_DEBUG
		printf("Catch Exception Drawing YScale!");
#endif
	}
	return retRect;	

}
 



int CChartPanel::GridScale (double XMin, double XMax, int N, double* SMin, double* Step)
{
#ifdef _CONSOLE_DEBUG
	//printf("\nGridScale() xMin=%f xMax=%f", XMin, XMax);
#endif
  int    iNegScl;       //  Negative scale flag
  int    iNm1;          //  Number of scale subintervals 
  double lfIniStep;     //  Initial step
  double lfSclStep;     //  Scaled step 
  double lfTmp;         //  Temporary value
  double lfSclFct;      //  Scaling back factor 
  double lfSMax;        //  Scale maximum value 
  int    it;            //  Iteration counter 
  int    i;             //  Neat step counter
  //  Antonio G¦miz

  //  Neat steps
  int Steps [] = {10, 12, 15, 16, 20, 25, 30, 40, 50, 60, 75, 80, 100, 120, 150};
  int iNS = sizeof (Steps) / sizeof (Steps [0]);
     
  //  Some checks
  if (XMin >  XMax)
    {
    lfTmp = XMin;
    XMin  = XMax;
    XMax  = lfTmp;
    }
  if (XMin == XMax)
    XMax = XMin == 0.0 ? 1.0 : XMin + fabs (XMin) / 10.0;
     
  //  Reduce to positive scale case if possible 
  if (XMax <= 0)
    {
    iNegScl = 1;
    lfTmp   = XMin;
    XMin    = -XMax;
    XMax    = -lfTmp;
    }
  else
    iNegScl = 0;
     
  if (N < 2)
    N = 2;
  iNm1 = N - 1;
     
  for (it = 0; it < 3; it++)
    {
    //  Compute initial and scaled steps 
    lfIniStep = (XMax - XMin) / iNm1;
    lfSclStep = lfIniStep;
     
    for (; lfSclStep <  10.0; lfSclStep *= 10.0) ; 
    for (; lfSclStep > 100.0; lfSclStep /= 10.0) ;
     
    //  Find a suitable neat step
    for (i = 0; i < iNS && lfSclStep > Steps [i]; i++) ; 
    lfSclFct = lfIniStep / lfSclStep;
     
    //  Compute step and scale minimum value 
    do
      {
      *Step  = lfSclFct * Steps [i];
      *SMin  = floor (XMin / *Step) * *Step; 
      lfSMax = *SMin + iNm1 * *Step;
      if (XMax <= lfSMax)           //  Function maximum is in the range: the
        {                           // work is done.
        if (iNegScl)
          *SMin = -lfSMax;
		*Step *= iNm1 / (N - 1);
#ifdef _CONSOLE_DEBUG
		//printf("\  sMin=%f Step=%f", *SMin, *Step);
#endif
        return 1;
        }
      i++;
      }
    while (i < iNS);
     
    //  Double number of intervals
    iNm1 *= 2;
    }
  //  Could not solve the problem
  return 0;
}

 
 

// Lambert's W function
double CChartPanel::W(const double x)
{
  int i; double p,e,t,w,eps=4.0e-16; /* eps=desired precision */
  if (x<-0.36787944117144232159552377016146086) {
    fprintf(stderr,"x=%g is < -1/e, exiting.\n",x); exit(1); }
  if (0.0==x) return 0.0;
  /* get initial approximation for iteration... */
  if (x<1.0) { /* series near 0 */
    p=sqrt(2.0*(2.7182818284590452353602874713526625*x+1.0));
    w=-1.0+p-p*p/3.0+11.0/72.0*p*p*p;
  } else w=log(x); /* asymptotic */
  if (x>3.0) w-=log(w);
  for (i=0; i<20; i++) { /* Halley loop */
    e=exp(w); t=w*e-x;
    t/=e*(w+1.0)-0.5*(w+2.0)*t/(w+1.0); w-=t;
    if (fabs(t)<eps*(1.0+fabs(w))) return w; /* rel-abs error */
  } /* never gets here */
  return NULL_VALUE;
}

int CChartPanel::FindByName(LPCTSTR szName)
{	
	int ret = -1;
	if(series.size() > 0){
		for(int n = 0; n != series.size(); ++n){			
			if(pCtrl->CompareNoCase(szName, series[n]->szName)){
				ret = n;
			}
		}
	}
	return ret;
}

void CChartPanel::AddHorzLine(double value)
{	
	int count = horizontalLines.size();
	for(int y = 0; y != count; ++y)
	{
		if(horizontalLines[y] == -1)
		{
			horizontalLines[y] = value;
			return;
		}
	}
	horizontalLines.push_back(value);
}

void CChartPanel::RemoveHorzLine(double value)
{
	int count = horizontalLines.size();
	for(int y = 0; y != count; ++y)
	{
		if(horizontalLines[y] == value)
		{
			horizontalLines[y] = -1;
		}
	}
}


