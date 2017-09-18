// Series.cpp: implementation of the CSeries class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartXCtl.h"
#include "Series.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define _CONSOLE_DEBUG 1


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CSeries::CSeries(LPCTSTR name, int type, int members, CChartPanel* owner)
{
	isTwin = false;
	showDialog = false;
	szTitle = "";
	userParams = false;
	szName = name;
	ownerPanel = owner;	
	seriesType = type; // Candle, stock, renko, etc.
	memberCount = members;
	Initialize();
}

void CSeries::Connect(CStockChartXCtrl *Ctrl)
{
	pCtrl = Ctrl;
	connected = true;
}

CSeries::CSeries()
{
	memberCount = 1;
	nSavedDC = 0;
	Initialize();
	connected = false;
}

CSeries::~CSeries()
{

}

void CSeries::Initialize()
{
	seriesVisible = true;
	downColor = -1;
	upColor = -1;
	wickDownColor = -1;
	wickUpColor = -1;
	pOpen = NULL;
	pHigh = NULL;
	pLow = NULL;
	pClose = NULL;
	pVolume = NULL;
	calculated	= false;
	selected	= false;
	selectable	= true;
	max = 0;
	min = 0;
	shareScale = true;
	lineColor = RGB(0,0,0);
	lineWeight = 1;
	lineStyle = 0;
	showDialog = false;
	szTitle = "";
	linkTo = "";
	userParams = false;  
	recycleFlag = false;	
	for(int n = 0; n != 10; ++n){
		paramTypes[n] = -1;
		paramDefs[n] = "";
	}
	indicatorType = -1;
	dataError = false;
	
	//TWIN	
	lineColorSignal = RGB(255,0,0);
	lineStyleSignal = 0;
	lineWeightSignal = 1;

}

// Returns record count of data
int CSeries::GetRecordCount()
{
	return data_master.size();
}

// Appends data and Julian date to the data vector
void CSeries::AppendValue(double jdate, double value)
{
	SeriesPoint sp;
	sp.jdate = jdate;
	sp.value = value;
	data_master.push_back(sp);
	if(connected){
		if(!pCtrl->updatingIndicator){
			pCtrl->endIndex = data_master.size();
		}
	}
}

// Builds a candle and appends data
// This function is called only if the series is .close or .volume
void CSeries::AppendValueAsTick(double jdate, double value)
{
			
	// Update 12/16/04 RG - BarStartTime integration,
	// automatically picks up at correct time
	// unless BarStartTime is null.

	// Make sure this is a .close or a .volume series
	CString c = szName;
	c.MakeLower();	// szName changed to c 11/21/05
	bool isClose = (c.Find(".close") != -1);
	bool isVolume = (c.Find(".volume") != -1);
	if(!isClose && !isVolume) return;	
    
	// Connect to the group
	if(NULL == pOpen) pOpen = GetSeriesOHLCV("open");
	if(NULL == pHigh) pHigh = GetSeriesOHLCV("high");
	if(NULL == pLow) pLow = GetSeriesOHLCV("low");
	if(NULL == pClose) pClose = GetSeriesOHLCV("close");
	if(NULL == pVolume) pVolume = GetSeriesOHLCV("volume");

	// Determine the exact record this data belongs to
	// and append a new record if it does not exist.
	// First check to see if the record exists already
	double jRecord = -1;
	long nRecord = -1;
	double diff;

	//NEW UPDATE: real-time should works for all periodicities!
	switch(pCtrl->m_Periodicity){
		case 1: //Secondly
			diff = JSECOND * pCtrl->m_barInterval;
			break;
		case 2: //Minutely
			diff = JMINUTE * pCtrl->m_barInterval;
			break;
		case 3: //Hourly
			diff = JHOUR * pCtrl->m_barInterval;
			break;
		case 4: //Daily
			diff = JDAY * pCtrl->m_barInterval;
			break;
		case 5: //Weekly
			diff = JWEEK * pCtrl->m_barInterval;
			break;
		case 6: //Month
			diff = JMONTH * pCtrl->m_barInterval;
			break;
		case 7: //Year
			diff = JYEAR * pCtrl->m_barInterval;
			break;
	}

	//Insert data into existent record:
	if(data_master.size() > 0){
		for(int n = 0; n != data_master.size(); ++n){
			if(data_master[n].jdate + diff > jdate && data_master[n].jdate <= jdate){
				// Found the record
				jRecord = data_master[n].jdate;
				nRecord = n;
			}
		}
	}

	// Append a new record after a certain time has elapsed
	if(isClose && (data_master.size() == 0 || 
		(nRecord == -1 && jdate > data_master[data_master.size() - 1].jdate + 
		diff))){
		
		
		if(data_master.size() > 0){
				
			double jdateLast = jdate;
			jdate = data_master[data_master.size() - 1].jdate;
			while(jdateLast - jdate > diff){				
				jdate += diff;
			};

		}
		else if(data_master.size() == 0){
			jdate = pCtrl->barStartTime;
		}
		//The tick is the new candle
		if(NULL != pOpen) pOpen->AppendValue(jdate, value);
		if(NULL != pHigh) pHigh->AppendValue(jdate, value);
		if(NULL != pLow) pLow->AppendValue(jdate, value);
		if(NULL != pClose) pClose->AppendValue(jdate, value);
		if(NULL != pVolume) pVolume->AppendValue(jdate, 0);		
		nRecord = pClose->data_master.size() - 1;
		jRecord = jdate;

	}

	// At this point whatever nRecord and jRecord are, that is the 
	// current bar we are updating

	// Bad tick filter
    if(NULL != pOpen && !isVolume){
      //  if(value > pOpen->data_master[nRecord].value * 1.5 ||
	//		value < pOpen->data_master[nRecord].value * 0.5) return;
    }

	// Sum the volume and exit if this is a volume series
	if(isVolume){
		if(nRecord != -1 && nRecord < (int)data_master.size()){ // 2/1/05 RG
			data_master[nRecord].value += value;
		}
		return;
	}
	
	// Ensure that all series are the same length
	long size = data_master.size();
	if(NULL != pOpen){
		if(pOpen->data_master.size() != size) return;
	}
	if(NULL != pHigh){
		if(pHigh->data_master.size() != size) return;
	}
	if(NULL != pLow){
		if(pLow->data_master.size() != size) return;
	}

	// Update the record
	if(NULL != pHigh){
		if(value > pHigh->data_master[nRecord].value || value == NULL_VALUE){
			pHigh->data_master[nRecord].value = value;
		}
	}
	if(NULL != pLow){
		if(value < pLow->data_master[nRecord].value || value == NULL_VALUE){
			pLow->data_master[nRecord].value = value;
		}
	}

	data_master[nRecord].value = value;
	
	
}

// Edits data specified by Julian date;
void CSeries::EditValue(double jdate, double value)
{
	for(int n = 0; n != data_master.size(); ++n){
		if(data_master[n].jdate == jdate){
			data_master[n].value = value;
			break;
		}
	}
}

// Edits data specified by array index;
void CSeries::EditValue(long index, double value)
{
	if(index > -1 && index < (long)data_master.size()){
		data_master[index].value = value;
	}
}

// Edits Julian date specified by array index;
void CSeries::EditJDate(long index, double jdate)
{
	if(index > -1 && index < (long) data_master.size()){
		data_master[index].jdate = jdate;
	}
}

// Clear's the vector
void CSeries::Clear()
{	
	data_master.clear();
	data_master.resize(0);
	data_slave.clear();
	data_slave.resize(0);
}

// Returns a value specified by Julian date
double CSeries::GetValue(double jdate)
{
	double ret = NULL_VALUE;
	for(int n = 0; n != data_master.size(); ++n){
		if(data_master[n].jdate == jdate){
			ret = data_master[n].value;
			break;
		}
	}
	return ret;
}

// Returns a record lookup by Julian date
double CSeries::GetMasterRecordIndex(double jdate)
{
	double ret = NULL_VALUE;
	for(int n = 0; n != data_master.size(); ++n){
		if(data_master[n].jdate == jdate){
			ret = n;
			break;
		}
	}
	return ret;
}

// Returns a value specified by array index
double CSeries::GetValue(int index)
{
	double ret = NULL_VALUE;
	if(index > -1 && index <  (int)data_master.size()){
		ret = data_master[index].value;
	}
	return ret;
}

// Returns a Julian date specified by array index
double CSeries::GetJDate(int index)
{
	double ret = NULL_VALUE;
	if(index > -1 && index < (int)data_master.size()){
		ret = data_master[index].jdate;
	}
	return ret;
}

// Updates data_slave
void CSeries::UpdateSlave()
{	
	SeriesPoint sp;

	if(data_master.size() < 1) return;

	data_slave.clear();

	for(int n = 0; n != data_master.size(); ++n){
		sp.jdate = data_master[n].jdate;
		sp.value = data_master[n].value;
		data_slave.push_back(sp);			
	}
	ownerPanel->Update();
}


// Get the max/min for scaling
void CSeries::GetMaxMin()
{

	// If the scale is set by the client then return
	if(ownerPanel->staticScale){
		max = ownerPanel->max;
		min = ownerPanel->min;
		slmax = ownerPanel->slmax;
		slmin = ownerPanel->slmin;
		return;
	}

	// Share the scale with another series?
	if(this->shareScale){
		max = ownerPanel->max;
		min = ownerPanel->min;
		slmax = ownerPanel->slmax;
		slmin = ownerPanel->slmin;
		return;
	}
	else{

		// No, don't share the scale with another series
		// (overlay this series with the other(s))

		GetAbsMaxMin();

	}

}


// Searches for the absolute maximum and minimum values of a series or group.
// If this series belongs to a OHLC group, this function finds the min of the
// low and max of the high series.
void CSeries::GetAbsMaxMin(CSeries* pSeries /* = NULL */)
{
	if(pSeries == NULL) pSeries = this;

	double value = 0;
	int end = pCtrl->endIndex;
	pSeries->max = 0;
	pSeries->min = 1000000000000;
	pSeries->slmax = pSeries->max;
	pSeries->slmin = pSeries->min;
	double y = 0;
	
	// If it is a .close series, get the max of the 
	// high and the min of the low in recursive calls.

	CString lName = pSeries->szName;
	lName.MakeLower();
	int close = lName.Find(".close", 0);
	
	if(close != -1){ // If this is a .close series
		
		double tmin = 0, tmax = 0;
		double tslmin = 0, tslmax = 0;
		
		// Find the max
		CSeries* high = GetSeriesOHLCV("high");	
		if(high){
			GetAbsMaxMin(high);
			tmax = high->max;
			tslmax = high->slmax;
		}
		
		// Find the min
		CSeries* low = GetSeriesOHLCV("low");
		if(low){
			GetAbsMaxMin(low);
			tmin = low->min;
			tslmin = low->slmin;
		}
		
		high->min = tmin;
		high->slmin = tslmin;
		low->max = tmax;
		low->slmax = tslmax;

		max = tmax;
		min = tmin;
		slmax = tslmax;
		slmin = tslmin;

		pSeries->ownerPanel->hasPrice = true;
		
		return;

	}

	
	if (pSeries->data_slave.size() < (unsigned int)end) end = pSeries->data_slave.size();

	// Linear max/min
	for(int j = pCtrl->startIndex; j != end; ++j){
		if(pSeries->data_slave[j].value != NULL_VALUE){
			if(pSeries->data_slave[j].value < pSeries->min){
				pSeries->min = pSeries->data_slave[j].value;
			}
			else if(pSeries->data_slave[j].value > pSeries->max){
				pSeries->max = pSeries->data_slave[j].value;
			}
		}
	}

	
	// Semi-log max/min	
	pSeries->slmax = log10(max);
	pSeries->slmin = log10(min);

}

// Input: Max of all data, min of all data, and current value
// Output: Normalized value between 1 and 0
double CSeries::Normalize(double value)
{
	if(pCtrl->scalingType == SEMILOG && min > 0 && ownerPanel->hasPrice){
		return (value - slmin) / (slmax - slmin);
	}
	else
	{
	//	if(max < value && !ownerPanel->staticScale) max = value; // 7/7/2007
		return (value - min) / (max - min);
	}
}

// Input: Max of all data, min of all data, and scaled value
// Output: Unscaled value restored between max and min
double CSeries::UnNormalize(double value)
{    
	if(pCtrl->scalingType == SEMILOG && min > 0 && ownerPanel->hasPrice){
		return slmin + (value * (slmax - slmin));
	}
	else{
		return min + (value * (max - min));
	}
}


// Input: Actual price value
// Output: Y coordinate for chart area
double CSeries::GetY(double value)
{
	double nHeight = 0;
	double ret = 0;
	double bottom = 0;
	if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR) bottom = 2;
    nHeight = (double)((ownerPanel->y2 - ownerPanel->y1) /*- 2*/ - ownerPanel->yOffset - bottom);
	if(pCtrl->scalingType == SEMILOG && min > 0 && ownerPanel->hasPrice){
		ret = ownerPanel->y1 + ownerPanel->yOffset + (nHeight - (nHeight * Normalize(log10(value))));
		if(ret > ownerPanel->y2 - 5){
			ret = ownerPanel->y2;
		}
	}
	else{
		ret = ownerPanel->y1  + ownerPanel->yOffset +  (nHeight - (nHeight * Normalize(value)));
	}
	return ret /*+ 2*/;
}

// Reverses Y values set by GetY() back to 
// their original values (e.g. returns price
// or indicator value rather than Y pixel location)
double CSeries::GetReverseY(double value){
	double nHeight = 0;
	double ret = 0;
	if(max == 0) GetMaxMin();
	int bottom = 0;
	if(seriesType == OBJECT_SERIES_LINE || seriesType == OBJECT_SERIES_INDICATOR) bottom = 2;
	nHeight = (double)((ownerPanel->y2 - ownerPanel->y1) - 2 - pCtrl->yOffset - bottom);
	if(pCtrl->scalingType == SEMILOG && min > 0 && ownerPanel->hasPrice){
		value = UnNormalize(1 - (value - ownerPanel->y1) / nHeight);
		if(value > 0 && max > 0){			
			ret =  pow((double)10, (double)value);
		}
	}
	else{
		ret = UnNormalize(1 - (value - ownerPanel->y1) / nHeight) + 2 + pCtrl->yOffset;
	}
	return ret;
}

void CSeries::OnPaint(CDC *pDC)
{
	// virtual functions below
}

void CSeries::Update()
{
	GetMaxMin();	
}

void CSeries::OnLButtonDown(CPoint point)
{

}

void CSeries::OnLButtonUp(CPoint point)
{

}

void CSeries::OnMouseMove(CPoint point)
{

}

void CSeries::OnPaintXOR(CDC *pDC)
{

}

void CSeries::OnRButtonDown(CPoint point)
{

}

void CSeries::OnDoubleClick(CPoint point)
{
	if (selected && seriesType == OBJECT_SERIES_INDICATOR) {
		pCtrl->FireOnItemDoubleClick(OBJECT_SERIES_INDICATOR, szName);
		selected = false;
		pCtrl->UpdateScreen(false);
		pCtrl->RePaint();
	}
}

void CSeries::OnRButtonUp(CPoint point)
{
	if (data_slave.size() < 1) return;
	int index = (int)(ownerPanel->GetReverseX(point.x) + pCtrl->startIndex + 1);
	if (index < 0 || index >(int)data_slave.size() - 1) return;
	double y = GetY(data_master[index].value);
	double errorPixels = 10;
#ifdef _CONSOLE_DEBUG
	printf("\SERIE ONRBUTTON x=%d y=%d index=%d yInd=%f value=%f", point.x, point.y, index, y, data_master[index].value);
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

BOOL CSeries::Calculate() // For CIndicator
{
	return FALSE;
}

// Finds a series and returns the pointer
CSeries* CSeries::GetSeries(LPCTSTR Series)
{
	int series = -1;
	int panel = pCtrl->GetPanelByName(Series);
	if(panel > -1){
		series = pCtrl->panels[panel]->GetSeriesIndex(Series);
	}
	if(panel < 0 || series < 0){
		return NULL;
	}
	return pCtrl->panels[panel]->series[series];
}

// Finds a series OHLCV group and returns the pointer
CSeries* CSeries::GetSeriesOHLCV(LPCTSTR OHLCV)
{
	CSeries* pRet = NULL;
	CString group = szName;
	CString type = OHLCV;
	int found = group.Find(".", 0);
	if(found != -1){
		group = group.Left(found);
		group += "." + type;
		pRet = GetSeries(group);
	}
	return pRet;
}




// Returns true if the integer is an odd number
bool CSeries::IsOdd(int value){
    if (value == NULL)
    return false;
	return ((value % 2) > 0);
}



void CSeries::ExcludeRects(CDC* pDC)
{		
	nSavedDC = pDC->SaveDC();	
	pDC->ExcludeClipRect(ownerPanel->yScaleRect);
	CRect panel = ownerPanel->panelRect;
	panel.bottom = panel.top;
	panel.top = 0;
	pDC->ExcludeClipRect(panel);	
	panel = ownerPanel->panelRect;
	panel.top = panel.bottom;
	panel.bottom = pCtrl->height + CALENDAR_HEIGHT;
	pDC->ExcludeClipRect(panel);	
}

void CSeries::IncludeRects(CDC* pDC)
{
	pDC->RestoreDC(nSavedDC);
}