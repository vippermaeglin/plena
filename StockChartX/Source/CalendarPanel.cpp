// CalendarPanel.cpp: implementation of the CCalendarPanel class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "CalendarPanel.h"
#include "Date.h"
#include <exception>
using namespace std;

#include "StockChartX.h"

#include "julian.h"

//#define _CONSOLE_DEBUG

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CCalendarPanel::CCalendarPanel()
{

}

CCalendarPanel::~CCalendarPanel()
{

}


/*void CCalendarPanel::OnPaint(CDC* pDC, CStockChartXCtrl* pCtrl)
{
  
  if(pCtrl->panels[0]->series.size() < 1 ||
    pCtrl->panels[0]->series[0]->data_slave.size() < 1){    
      return;
  } 

  CRectF panelRect;
  panelRect.left = 0;
  panelRect.right = pCtrl->width;
  panelRect.top = pCtrl->height;
  panelRect.bottom = pCtrl->height + CALENDAR_HEIGHT;
  
  //Get font info
  CFont newFont;
  CFont* pOldFont;  
  newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial"), pDC);
  TEXTMETRIC tm;
  pOldFont = pDC->SelectObject(&newFont); 
  pDC->GetTextMetrics(&tm);
  int nAvgCharWidth = 2 + (tm.tmAveCharWidth);
  int nCharHeight = 1 + (tm.tmHeight);  


  CBrush* pOldBrush = NULL;
  CBrush* br = new CBrush(pCtrl->backColor);
  pOldBrush = pDC->SelectObject(br);

  //Draw the date panel border
  pCtrl->pdcHandler->FillRectangle(panelRect, pCtrl->backColor, pDC);
  pCtrl->pdcHandler->DrawLine(CPointF(panelRect.left,panelRect.top), CPointF(panelRect.right,panelRect.top), 1, 0, pCtrl->foreColor, pDC, true);
  CPen pDark (0,1,pCtrl->foreColor);
  CPen* pOldPen = pDC->SelectObject(&pDark);
  //pDC->FillRect(panelRect, br);
  //pDC->MoveTo(panelRect.left,panelRect.top);
  //pDC->LineTo(panelRect.right,panelRect.top);
  
  pDC->SelectObject(pOldBrush);
  br->DeleteObject();
  if(br) delete br;
    
  pDC->SetTextColor(pCtrl->foreColor);

  int rcnt = pCtrl->GetSlaveRecordCount();
  if(rcnt < 1){   
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }


  // Find the series with the greatest number of records
  int max = 0;
  int nPanel = 0;
  int nSeries = 0;
  for(int n = 0; n != pCtrl->panels.size(); ++n){
    for(int j = 0; j != pCtrl->panels[n]->series.size(); ++j){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->panels[n]->series[j]->data_slave.size() > max){
//  End Of Revision
        max = pCtrl->panels[n]->series[j]->data_slave.size();
        nPanel = n; nSeries = j;
      }
    }
  }


  //Get size of periods in pixels
  if(pCtrl->panels.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  if(pCtrl->panels[0]->series.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  if(pCtrl->panels[0]->series[0]->data_master.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  
  double periodPixels = GetX(rcnt, pCtrl) / rcnt;
  if(periodPixels < 1){
    periodPixels = 1;
  }
  
  pCtrl->xGridMap.clear();
  pCtrl->xGridMap.resize(rcnt);

  double dTradeWeek  = periodPixels * 5;   // 5 trading days in a week (avg)
  double dTradeMonth = periodPixels * 20;  // 20 trading days in a month (avg)
  double dTradeYear  = periodPixels * 253; // 253 trading days in a year (avg)  


  // Level 1:
  // YYYY
  double Level1 = nAvgCharWidth * 4;

  // Level 2:
  // YY F M A M J J A S O N D
  double Level2 = nAvgCharWidth * 2;

  // Level 3:
  // YY Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec
  double Level3 = nAvgCharWidth * 3;

  // Level 4:
  // YYYY February March April May June July August September October November December
  double Level4 = nAvgCharWidth * 10;

  // Level 5:
  // From -5 periods on right end, begin:
  // Jan DD  Feb DD  Mar DD  Apr DD  May DD  Jun DD  Jul DD  Aug DD  Sep DD  Oct DD  Nov DD  Dec DD
  double Level5 = nAvgCharWidth * 6;

  // Level 6
  // Jan DD HH:MM:SS:MMM
  double Level6 = nAvgCharWidth * 10;

  // Level 7:
  // 12345 (record numbers to 99,999)
  double Level7 = nAvgCharWidth * 5;



  int xGrid = 0;
  int x = 0;
  int lx = 0;
  int incr = 0;
  double dDate = 0;
  CDate pDate;  
  CString szDate = "";
  CString szDay = "";
  CString szCache = "";
  CString szTemp = "";
  CRectF rect;

  double hr = 0.0417f;
  double startTime = 0;


  // Use time for x labels
  if(pCtrl->realTime){
    CString szHour = "";
    CString szMin = "";
    CString szTime = "";
    CString szMonth = "";
    CString szYear = "";
    CString szPrevDay = "";
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)(Level6); // removed + 1 - RDG 8/3/04
//  End Of Revision
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)     
      x = (int)GetX(period + 1, pCtrl);
//  End Of Revision
      if(x != lx){
        
        dDate = GetDate(period, x, pCtrl);
        pDate = FromJulianDateToCDate(dDate);
        if(incr > Level6){
          incr = 0;
          startTime = GetDate(period, x, pCtrl); //Reset
          //Draw vertical line
		  pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
          //pDC->MoveTo(x, panelRect.bottom);
          //pDC->LineTo(x, panelRect.top);
          //Get Julian date
          pDate = FromJulianDateToCDate(dDate);
          if(pDate.IsValid()){
            //Format date
            //COleDateTime vaTimeDate;
            //vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
            //      pDate.Hour, pDate.Minute, pDate.Second);          
            //vaTimeDate.Format(0, LANG_USER_DEFAULT);
          
            //szHour.Format("// %02d", vaTimeDate.GetHour());
//             if(szHour.GetLength() == 1){
//               szHour = "0" + szHour;
//             }
//             szMin.Format("%02d", vaTimeDate.GetMinute());
//             if(szMin.GetLength() == 1){
//               szMin = "0" + szMin;
//             }
            // szTime = szHour + ":" + szMin;
            if (pCtrl->ExtraPrecision())
            {
              szTime = CJulian::FormatTime(dDate);
            }
            else
            {
              //szHour.Format("%02d", pDate.Hour);
              //szMin.Format("%02d", pDate.Minute);
              //szTime = szHour + ":" + szMin;
              szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
            }

            szDay.Format("%02d", pDate.Day);
//             if(szDay.GetLength() == 1){
//               szDay = "0" + szDay;
//             }
            szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
//             if(szMonth.GetLength() == 1){
//               szMonth = "0" + szMonth;
//             }
            szYear.Format("%02d", pDate.Year);
//             if(szDay.GetLength() == 1){
//               szDay = "0" + szDay;
//             }

          }
          if(szPrevDay != szDay){
            szPrevDay = szDay;
            szDate = szYear + " " + szMonth + " " + szDay + " " + szTime;
            Level6 = nAvgCharWidth * (pCtrl->ExtraPrecision() ? 17 : 15);
            lx += (int)(Level6 / 2);
          }
          else{
            szDate = szTime;          
          }

          //Print text in calendar box
          rect.top = panelRect.top + 3;
          rect.bottom = panelRect.bottom - 3;
          rect.left = x + 3;
          rect.right = (int)(x + Level6 + 3);
		  pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);

          //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
          xGrid++;

        }
        if(dDate - startTime > hr) startTime = 0;
        incr += x - lx;
        lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
          pCtrl->xGridMap[xGrid] = rect.left - 3;
      }

    }
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();

    if(pCtrl->showXGrid){
      pCtrl->xGridMap.resize(xGrid + 1);
    }

    return;
  }

  
  // Use records for x labels

  int cnt = 0;
  if(pCtrl->recordLabels){        
    for(long period = pCtrl->startIndex; period != pCtrl->endIndex; ++period){
      cnt++;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)GetX(cnt, pCtrl);
//  End Of Revision
      incr += x - lx;
      if(incr > Level7){
        incr = 0;
        //Draw vertical line
		  pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Display the label
        szTemp.Format("%d", period + 1);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level7 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szTemp, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szTemp, -1, &rect, DT_SINGLELINE | DT_LEFT);        
        xGrid++;
      }   
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();

    if(pCtrl->showXGrid){
      pCtrl->xGridMap.resize(xGrid + 1);
    }

    return;
  } 


  // What level will fit on the screen?
  if(Level5 <= dTradeWeek){
    incr = (int)Level5;
    for(long period = 0; period != rcnt; ++period){
      x = (int)GetX(period + 1, pCtrl);
      if(incr > Level5)
      {
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Get Julian date
        dDate = GetDate(period, x, pCtrl);
        pDate = FromJulianDateToCDate(dDate);       
        if(pDate.IsValid()){
          //Format date
          COleDateTime vaTimeDate;
          vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
                pDate.Hour, pDate.Minute, pDate.Second);
          //Display the 3-character month and 2-digit weekday
          vaTimeDate.Format(0, LANG_USER_DEFAULT);
        
          szDay.Format("%d", vaTimeDate.GetDay());
          if(szDay.GetLength() == 1){
            szDay = "0" + szDay;
          }
          CString szMonth = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
          szDate = szMonth + " " + szDay;
        }

        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level5 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
      }
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level4 <= dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level4;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)(GetX(period + 1, pCtrl));
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 10, pCtrl);
      }
      if(szDate == "January"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate + " Jan";
      }
      if(incr > Level4 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level4 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level3 + 2 < dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level3;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)(GetX(period + 1, pCtrl));
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      if(szDate == "Jan"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate.Mid(2);
      }
      if(incr > Level3 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level3 + 4);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
        pCtrl->xGridMap[xGrid] = rect.left - 3;
//  End Of Revision
    }
  }
  else if(Level2 <= dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level2;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)GetX(period + 1, pCtrl);
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      //Format date
      COleDateTime vaTimeDate;
      if(pDate.IsValid()){
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      if(szDate == "Jan"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate.Mid(2);
        szTemp = szDate;
      }
      else{
        szTemp = szDate.Mid(0,1);
      }
      if(incr > Level2 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.left = (int)(x + 3);
        rect.right = (int)(x + Level3 + 3);
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)   
		pCtrl->pdcHandler->DrawText(szTemp, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szTemp, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level1 <= dTradeYear){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level1;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
      x = (int)(GetX(period + 1, pCtrl));
      if(x == -1) break;
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      szDate.Format("%d", pDate.Year);
      if(incr > Level1 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		  pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level3 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((long)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else{
    //Can't display a calendar
  }


  if(pCtrl->showXGrid){
    pCtrl->xGridMap.resize(xGrid + 1);
  }
  
  pDC->SelectObject(pOldBrush);
  pDC->SelectObject(pOldPen);
  pDC->SelectObject(pOldFont);
  newFont.DeleteObject();

}*/


void CCalendarPanel::OnPaint(CDC* pDC, CStockChartXCtrl* pCtrl)
{
  
  if(pCtrl->panels[0]->series.size() < 1 ||
    pCtrl->panels[0]->series[0]->data_slave.size() < 1){    
      return;
  } 

  CRectF panelRect;
  panelRect.left = 0;
  panelRect.right = pCtrl->width;
  panelRect.top = pCtrl->height;
  panelRect.bottom = pCtrl->height + CALENDAR_HEIGHT;
  
  //Get font info
  CFont newFont;
  CFont* pOldFont;  
  newFont.CreatePointFont(VALUE_FONT_SIZE, _T("Arial"), pDC);
  TEXTMETRIC tm;
  pOldFont = pDC->SelectObject(&newFont); 
  pDC->GetTextMetrics(&tm);
  int nAvgCharWidth = /*2 +*/ (tm.tmAveCharWidth);
  int nCharHeight = 1 + (tm.tmHeight);  


  CBrush* pOldBrush = NULL;
  CBrush* br = new CBrush(pCtrl->backColor);
  pOldBrush = pDC->SelectObject(br);

  //Draw the date panel border
  pCtrl->pdcHandler->FillRectangle(panelRect, pCtrl->backColor, pDC);
  pCtrl->pdcHandler->DrawLine(CPointF(panelRect.left,panelRect.top), CPointF(panelRect.right,panelRect.top), 1, 0, pCtrl->foreColor, pDC, true);
  CPen pDark (0,1,pCtrl->foreColor);
  CPen* pOldPen = pDC->SelectObject(&pDark);
  //pDC->FillRect(panelRect, br);
  //pDC->MoveTo(panelRect.left,panelRect.top);
  //pDC->LineTo(panelRect.right,panelRect.top);
  
  pDC->SelectObject(pOldBrush);
  br->DeleteObject();
  if(br) delete br;
    
  pDC->SetTextColor(pCtrl->foreColor);

  int rcnt = pCtrl->GetSlaveRecordCount();
  if(rcnt < 1){   
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }


  // Find the series with the greatest number of records
  int max = 0;
  int nPanel = 0;
  int nSeries = 0;
  for(int n = 0; n != pCtrl->panels.size(); ++n){
    for(int j = 0; j != pCtrl->panels[n]->series.size(); ++j){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->panels[n]->series[j]->data_slave.size() > max){
//  End Of Revision
        max = pCtrl->panels[n]->series[j]->data_slave.size();
        nPanel = n; nSeries = j;
      }
    }
  }


  //Get size of periods in pixels
  if(pCtrl->panels.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  if(pCtrl->panels[0]->series.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  if(pCtrl->panels[0]->series[0]->data_master.size() < 1){
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();
    return;
  }
  
  double periodPixels = GetX(rcnt, pCtrl) / rcnt;
  if(periodPixels < 1){
    periodPixels = 1;
  }
  
  pCtrl->xGridMap.clear();
  pCtrl->xGridMap.resize(rcnt);

  double dTradeWeek  = periodPixels * 5;   // 5 trading days in a week (avg)
  double dTradeMonth = periodPixels * 20;  // 20 trading days in a month (avg)
  double dTradeYear  = periodPixels * 253; // 253 trading days in a year (avg)  


  // Level 1:
  // DD/MMM
  double Level1 = nAvgCharWidth * 6;

  // Level 2:
  // DD/MMM/YY
  double Level2 = nAvgCharWidth * 9;

  // Level 3:
  // DD/MMM/YY
  double Level3 = nAvgCharWidth * 9;

  // Level 4:
  // MMM/YYYY 
  double Level4 = nAvgCharWidth * 8;

  // Level 5:
  // HH:MM 
  double Level5 = nAvgCharWidth * 6;
  
 
  //Current Levl to show:
  int levelCurrent = 0;

  int xGrid = 0;
  int x = 0;
  int lx = -50;
  int incr = 0;
  double dDate = 0;
  CDate pDate;  
  CString szDate = "";
  CString szDay = "";
  CString szCache = "";
  CString szTemp = "";
  CRectF rect;

  double hr = 0.0417f;
  double startTime = 0;
  
//REAL TIME (USE THIS!)
  if(true/*pCtrl->realTime*/){
    CString szHour = "";
    CString szMin = "";
    CString szTime = "";
    CString szMonth = "";
    CString szYear = "";
    CString szPrevDay = "";
	bool newMonth = false;

	//Calculate Level to show:
	int rlevel = (pCtrl->endIndex-pCtrl->startIndex);
	//Periodicity = Day
	if(pCtrl->m_Periodicity == 4){
		if(rlevel<=10) levelCurrent = 1;
		else if(rlevel<=50) levelCurrent = 2;
		else if(rlevel<=100) levelCurrent = 3;
		else if (rlevel <= 300) levelCurrent = 4;
		else if (rlevel <= 850) levelCurrent = 6;
		else if (rlevel <= 1900) levelCurrent = 7;
		else levelCurrent = 8;
	}	
	//Periodicity = Week
	else if(pCtrl->m_Periodicity == 5){
		/*if(rlevel<=10) levelCurrent = 1;
		else if(rlevel<=50) levelCurrent = 2;
		else*/ if (rlevel <= 30) levelCurrent = 3;
		else if (rlevel <= 60) levelCurrent = 4;
		else if (rlevel <= 180) levelCurrent = 6;
		else if (rlevel <= 360) levelCurrent = 7;
		else levelCurrent = 8;
	}
	//Periodicity = Month
	else if(pCtrl->m_Periodicity == 6){
		/*if(rlevel<=10) levelCurrent = 1;
		else if(rlevel<=50) levelCurrent = 2;
		else if(rlevel<=100) levelCurrent = 3;
		else*/ if (rlevel <= 15) levelCurrent = 4;
		else if (rlevel <= 45) levelCurrent = 6;
		else if (rlevel <= 80) levelCurrent = 7;
		else levelCurrent = 8;
	}
	//Periodicity = Year
	else if(pCtrl->m_Periodicity == 7){
		/*if(rlevel<=10) levelCurrent = 1;
		else if(rlevel<=50) levelCurrent = 2;
		else if(rlevel<=100) levelCurrent = 3;
		else*/ levelCurrent = 8;
	}
	//Periodicity < 30 Minute
	else if(pCtrl->m_Periodicity == 2 && pCtrl->m_BarSize<30){
		if(rlevel<=250) levelCurrent = 5;
		else levelCurrent = 1;
	}
	//Periodicity > 30 Minutes
	else if(pCtrl->m_Periodicity == 2 && pCtrl->m_BarSize!=60){
		if(rlevel<=40) levelCurrent = 5;
		else if(rlevel<=300) levelCurrent = 1;
		else if(rlevel<=1000) levelCurrent = 2;
		else levelCurrent = 4;
	}
	//Periodicity = Hour
	else {
		if(rlevel<=40) levelCurrent = 5;
		else if(rlevel<=300) levelCurrent = 1;
		else if(rlevel<=1000) levelCurrent = 2;
		else levelCurrent = 4;
	}

	// Aux members:	
	int lastReference = 0;
	int lastReference2 = 0;
	int lastMark = 0;
	int subx = 0;
	bool firstCheck = true;
#ifdef _CONSOLE_DEBUG
	printf("\nCalendar Level=%d",levelCurrent);
#endif

	switch(levelCurrent)
	{
#pragma region 1- Reference by Days:
		case 1:			
			for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				dDate = GetDate(period, x, pCtrl); //day reference
				pDate = FromJulianDateToCDate(dDate); //month reference
				if(firstCheck)
				{
					int lastPeriod = (int)GetX(period, pCtrl);
					double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
					CDate pDate2 = FromJulianDateToCDate(dDate2);
					lastReference = pDate2.Day;
					lastReference2 = pDate2.Month;
					firstCheck = false;
				}
				if(x != lx  )
				{     

					if (pDate.Day != lastReference && x > lx+(Level1)){
					    //Draw vertical line
						pDC->MoveTo(x, panelRect.bottom);
						pDC->LineTo(x, panelRect.top);
						lastReference = pDate.Day;
						if(pDate.Month != lastReference2) newMonth = true;
						else newMonth = false;
						lastReference2 = pDate.Month;

								
						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);          
						if (pDate.IsValid()) {
							
							if (pCtrl->ExtraPrecision())
							{
							  szTime = CJulian::FormatTime(dDate);
							}
							else
							{
							  //szHour.Format("%02d", pDate.Hour);
							  //szMin.Format("%02d", pDate.Minute);
							  //szTime = szHour + ":" + szMin;
							  szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year >= 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}
						
						if(newMonth)szDate = /*szDay + "/" + */szMonth + "/" + szYear;
						else szDate = szDay;
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level1 + 3);


						pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
						//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
						xGrid++;
						if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
						//if(pCtrl->showXGrid)pCtrl->xGridMap.resize(xGrid + 1);					   
						lx = x;
					}
				}
			}
			break;
#pragma endregion
#pragma region 2- Reference by Weeks:
		case 2:	
			for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				dDate = GetDate(period, x, pCtrl);
				pDate = FromJulianDateToCDate(dDate);	
				if(x != lx )
				{        				
					if (firstCheck) {
						int lastPeriod = (int)GetX(period, pCtrl);
						double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
						CDate pDate2 = FromJulianDateToCDate(dDate2);
						lastReference = pDate2.Month;
						lastReference2 = pDate2.Day+2;
						firstCheck = false;
					}

					if ((pDate.Month != lastReference || pDate.Day > lastReference2)&&period!=0){
						if(x>lx+Level2){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						if(pDate.Month != lastReference) newMonth = true;
						else newMonth = false;
						lastReference = pDate.Month;
								
						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);          
						if (pDate.IsValid()) {
							
							if (pCtrl->ExtraPrecision())
							{
							  szTime = CJulian::FormatTime(dDate);
							}
							else
							{
							  //szHour.Format("%02d", pDate.Hour);
							  //szMin.Format("%02d", pDate.Minute);
							  //szTime = szHour + ":" + szMin;
							  szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}
						
						if(newMonth)szDate = /*szDay + "/" + */szMonth + "/" + szYear;
						else szDate = szDay;
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level2 + 3);


						if(x>lx+Level2){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) {
								pCtrl->xGridMap[xGrid] = rect.left - 3;
								lx = x;
							}
						}
					}
				}
				lastReference2 = pDate.Day+2;
			}

			break;
#pragma endregion
#pragma region 3- Reference by Half-Months:
		case 3:
			for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				dDate = GetDate(period, x, pCtrl);
				pDate = FromJulianDateToCDate(dDate);	
				if(x != lx )
				{        				
					if (firstCheck) {
						int lastPeriod = (int)GetX(period, pCtrl);
						double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
						CDate pDate2 = FromJulianDateToCDate(dDate2);
						lastReference = pDate2.Month;
						lastReference2 = pDate.Day+2;
						firstCheck = false;
					}

					if ((pDate.Month != lastReference || (pDate.Day > lastReference2 && pDate.Day>12 && pDate.Day<20))&&period!=0){
						
						// Adjust to get a record on the midle:
						if(pDate.Day > lastReference2 && pDate.Day<16 && pDate.Day>12){
							int offset = 16-pDate.Day;
							if(period+offset<rcnt-1){
								period += offset;
								x = (int)GetX(period + 1, pCtrl);
								dDate = GetDate(period, x, pCtrl);
								pDate = FromJulianDateToCDate(dDate);
							}
						}
						
						if(x>lx+Level2){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						if(pDate.Month != lastReference) newMonth = true;
						else newMonth = false;
						lastReference = pDate.Month;
								
						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);          
						if (pDate.IsValid()) {
							
							if (pCtrl->ExtraPrecision())
							{
							  szTime = CJulian::FormatTime(dDate);
							}
							else
							{
							  //szHour.Format("%02d", pDate.Hour);
							  //szMin.Format("%02d", pDate.Minute);
							  //szTime = szHour + ":" + szMin;
							  szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}
						
						if(newMonth)szDate = /*szDay + "/" + */szMonth + "/" + szYear;
						else szDate = szDay;
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level2 + 3);


						if(x>lx+Level2){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) {
								pCtrl->xGridMap[xGrid] = rect.left - 3;
								lx = x;
							}
						}
					}
				}
				lastReference2 = pDate.Day+2;
			}
			/*for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				dDate = GetDate(period, x, pCtrl);
				pDate = FromJulianDateToCDate(dDate);
				if(x != lx)
				{        

					if (firstCheck) {
						lastReference = pDate.Month;
						firstCheck = false;
					}
					if (pDate.Month != lastReference){
						if(x>lx+Level3){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						lastReference = pDate.Month;
												
						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);          
						if (pDate.IsValid()) {
							
							if (pCtrl->ExtraPrecision())
							{
							  szTime = CJulian::FormatTime(dDate);
							}
							else
							{
							  //szHour.Format("%02d", pDate.Hour);
							  //szMin.Format("%02d", pDate.Minute);
							  //szTime = szHour + ":" + szMin;
							  szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}
						
						szDate = szDay + "/" + szMonth+ "/" + szYear;
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level3 + 3);
						
						if(x>lx+Level2){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
							lx = x;
						}

						subx = (int)GetX((lastMark+(period-lastMark)/2) + 1, pCtrl);							
						dDate = GetDate(lastMark+(period-lastMark)/2, subx, pCtrl);
						pDate = FromJulianDateToCDate(dDate);
						if(lastMark!=0){
							if(subx>(int)GetX(lastMark+1, pCtrl)+Level3 && subx+Level3<x){
								//Draw vertical line
								pDC->MoveTo(subx, panelRect.bottom);
								pDC->LineTo(subx, panelRect.top);
							}
							//lastReference = pDate.Month;
												
							//Get Julian date
							pDate = FromJulianDateToCDate(dDate);          
							if (pDate.IsValid()) {
							
								if (pCtrl->ExtraPrecision())
								{
									szTime = CJulian::FormatTime(dDate);
								}
								else
								{
									//szHour.Format("%02d", pDate.Hour);
									//szMin.Format("%02d", pDate.Minute);
									//szTime = szHour + ":" + szMin;
									szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
								}

								szDay.Format("%02d", pDate.Day);
								szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
								szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
							}
						
							szDate = szDay + "/" + szMonth+ "/" + szYear;
							//Print text in calendar box
							rect.top = panelRect.top + 3;
							rect.bottom = panelRect.bottom - 3;
							rect.left = subx + 3;
							rect.right = (int)(subx + Level3 + 3);
						
							if(subx>(int)GetX(lastMark+1, pCtrl)+Level3 && subx+Level3<x){
									pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
									//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
									xGrid++;
									if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
							}
					
						
						}
						
						lastMark = period;

					}		
					
					
				}
			}*/
			break;
#pragma endregion
#pragma region 4- Reference by Months:
		case 4:
			for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				if(x != lx )
				{        
					dDate = GetDate(period, x, pCtrl);
					pDate = FromJulianDateToCDate(dDate);

					if (firstCheck) {
						int lastPeriod = (int)GetX(period, pCtrl);
						double dDate2 = GetDate(period-1, lastPeriod, pCtrl);
						CDate pDate2 = FromJulianDateToCDate(dDate2);
						lastReference = pDate2.Month;
						firstCheck = false;
					}
					if (pDate.Month != lastReference || pCtrl->m_Periodicity == 7){
						if(x > lx+(Level4) ){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						lastReference = pDate.Month;
												
						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);          
						if (pDate.IsValid()) {
							
							if (pCtrl->ExtraPrecision())
							{
							  szTime = CJulian::FormatTime(dDate);
							}
							else
							{
							  //szHour.Format("%02d", pDate.Hour);
							  //szMin.Format("%02d", pDate.Minute);
							  //szTime = szHour + ":" + szMin;
							  szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}
						
						szDate = szMonth + "/" + szYear;
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level4 + 3);


						if(x > lx+(Level4) ){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
							lx = x;
						}
						
					}
					
				}
			}
			break;
#pragma endregion
#pragma region 5- Reference by Hours:
		case 5:			
			for(long period = 0; period != rcnt; ++period)
			{				
				x = (int)GetX(period + 1, pCtrl);
				dDate = GetDate(period, x, pCtrl);
				pDate = FromJulianDateToCDate(dDate);
				if (firstCheck){
					int lastPeriod = (int)GetX(period, pCtrl);
					double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
					CDate pDate2 = FromJulianDateToCDate(dDate2);
					lastReference = pDate2.Day;
					firstCheck = false;
				}
				if(x > lx+(Level5) )
				{        
					lx = x;

					//Draw vertical line
					pDC->MoveTo(x, panelRect.bottom);
					pDC->LineTo(x, panelRect.top);
								
					//Get Julian date
					pDate = FromJulianDateToCDate(dDate);          
					if (pDate.IsValid()) {
							
						if (pCtrl->ExtraPrecision())
						{
							szTime = CJulian::FormatTime(dDate);
						}
						else
						{
							//szHour.Format("%02d", pDate.Hour);
							//szMin.Format("%02d", pDate.Minute);
							//szTime = szHour + ":" + szMin;
							szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
						}

						szDay.Format("%02d", pDate.Day);
						szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
						szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
					}
						
					if(pDate.Day != lastReference)szDate = szDay + "/" + szMonth;
					else szDate = szTime;
					//Print text in calendar box
					rect.top = panelRect.top + 3;
					rect.bottom = panelRect.bottom - 3;
					rect.left = x + 3;
					rect.right = (int)(x + Level1 + 3);


					pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
					//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
					xGrid++;
					if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
					//if(pCtrl->showXGrid)pCtrl->xGridMap.resize(xGrid + 1);
					lastReference = pDate.Day;
					
				}
			}
			break;
#pragma endregion
#pragma region 6- Reference by Trimester:
		case 6:
			for (long period = 0; period != rcnt; ++period)
			{
				x = (int)GetX(period + 1, pCtrl);
				if (x != lx)
				{
					dDate = GetDate(period, x, pCtrl);
					pDate = FromJulianDateToCDate(dDate);

					bool isTrimester = pDate.Month == 1 || pDate.Month == 4 || pDate.Month == 7 || pDate.Month == 10;

					if (firstCheck) {
						int lastPeriod = (int)GetX(period, pCtrl);
						double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
						CDate pDate2 = FromJulianDateToCDate(dDate2);
						lastReference = pDate2.Month;
						firstCheck = false;
					}
					if ((pDate.Month != lastReference || pCtrl->m_Periodicity == 7) && (isTrimester)){
						if (x > lx + (Level4) && isTrimester){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						lastReference = pDate.Month;

						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);
						if (pDate.IsValid()) {

							if (pCtrl->ExtraPrecision())
							{
								szTime = CJulian::FormatTime(dDate);
							}
							else
							{
								//szHour.Format("%02d", pDate.Hour);
								//szMin.Format("%02d", pDate.Minute);
								//szTime = szHour + ":" + szMin;
								szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}

						if (isTrimester)szDate = szMonth + "/" + szYear;
						else
						{
							continue;
						}
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level4 + 3);


						if (x > lx + (Level4)){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
							lx = x;
						}

					}

				}
			}
			break;
#pragma endregion
#pragma region 7- Reference by Semester:
		case 7:
			for (long period = 0; period != rcnt; ++period)
			{
				x = (int)GetX(period + 1, pCtrl);
				if (x != lx)
				{
					dDate = GetDate(period, x, pCtrl);
					pDate = FromJulianDateToCDate(dDate);

					bool isSemester = pDate.Month == 1 || pDate.Month == 7;

					if (firstCheck) {
						int lastPeriod = (int)GetX(period, pCtrl);
						double dDate2 = GetDate(period - 1, lastPeriod, pCtrl);
						CDate pDate2 = FromJulianDateToCDate(dDate2);
						lastReference = pDate2.Month;
						firstCheck = false;
					}
					if ((pDate.Month != lastReference || pCtrl->m_Periodicity == 7) && (isSemester)){
						if (x > lx + (Level4) && isSemester){
							//Draw vertical line
							pDC->MoveTo(x, panelRect.bottom);
							pDC->LineTo(x, panelRect.top);
						}
						lastReference = pDate.Month;

						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);
						if (pDate.IsValid()) {

							if (pCtrl->ExtraPrecision())
							{
								szTime = CJulian::FormatTime(dDate);
							}
							else
							{
								//szHour.Format("%02d", pDate.Hour);
								//szMin.Format("%02d", pDate.Minute);
								//szTime = szHour + ":" + szMin;
								szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
						}

						if (isSemester)szDate = szMonth + "/" + szYear;
						else
						{
							continue;
						}
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level4 + 3);


						if (x > lx + (Level4)){
							pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
							//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
							xGrid++;
							if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
							lx = x;
						}

					}

				}
			}
			break;
#pragma endregion
#pragma region 8- Reference by Year:
		case 8:
			lastReference2 = 0;
			for (long period = 0; period != rcnt; ++period)
			{
				x = (int)GetX(period + 1, pCtrl);
				if (x != lx)
				{
					dDate = GetDate(period, x, pCtrl);
					pDate = FromJulianDateToCDate(dDate);

					bool isYear = pDate.Month == 1 && pDate.Year!=lastReference2;

					if (isYear){

						//Draw vertical line
						pDC->MoveTo(x, panelRect.bottom);
						pDC->LineTo(x, panelRect.top);
						lastReference2 = pDate.Year;

						//Get Julian date
						pDate = FromJulianDateToCDate(dDate);
						if (pDate.IsValid()) {

							if (pCtrl->ExtraPrecision())
							{
								szTime = CJulian::FormatTime(dDate);
							}
							else
							{
								//szHour.Format("%02d", pDate.Hour);
								//szMin.Format("%02d", pDate.Minute);
								//szTime = szHour + ":" + szMin;
								szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
							}

							szDay.Format("%02d", pDate.Day);
							szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
							szYear.Format("%04d", pDate.Year);
						}

						if (isYear)szDate = szYear;
						else
						{
							continue;
						}
						//Print text in calendar box
						rect.top = panelRect.top + 3;
						rect.bottom = panelRect.bottom - 3;
						rect.left = x + 3;
						rect.right = (int)(x + Level4 + 3);



						pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
						//pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
						xGrid++;
						if ((int)pCtrl->xGridMap.size() > xGrid) pCtrl->xGridMap[xGrid] = rect.left - 3;
						lx = x;

					}

				}
			}
			break;
#pragma endregion
	}



	/*
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)(Level6); // removed + 1 - RDG 8/3/04
//  End Of Revision

	int lastDay = 0;
	int lastMonth = 0;
	int lastYear = 0;
	bool newMonth = false, firstCheck = true;
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)     
      x = (int)GetX(period + 1, pCtrl);
//  End Of Revision
      if(x != lx){
        
        dDate = GetDate(period, x, pCtrl);
        pDate = FromJulianDateToCDate(dDate);

		if (firstCheck) {
			lastDay = pDate.Day;
			lastMonth = pDate.Month;
			lastYear = pDate.Year;
			firstCheck = false;
		}

		if (pDate.Day < lastDay || pDate.Month != lastMonth || pDate.Year != lastYear) {
			newMonth = true;
		}

		if (incr > Level6 && newMonth) {
			newMonth = false;
		//if (pDate.Day < lastDay) {
          incr = 0;
          startTime = GetDate(period, x, pCtrl); //Reset
          //Draw vertical line

          pDC->MoveTo(x, panelRect.bottom);
          pDC->LineTo(x, panelRect.top);
          //Get Julian date
          pDate = FromJulianDateToCDate(dDate);
          
		  if (pDate.IsValid()) {
            //Format date
            //COleDateTime vaTimeDate;
            //vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
            //      pDate.Hour, pDate.Minute, pDate.Second);          
            //vaTimeDate.Format(0, LANG_USER_DEFAULT);
          
            //szHour.Format("// %02d", vaTimeDate.GetHour());
//             if(szHour.GetLength() == 1){
//               szHour = "0" + szHour;
//             }
//             szMin.Format("%02d", vaTimeDate.GetMinute());
//             if(szMin.GetLength() == 1){
//               szMin = "0" + szMin;
//             }
            // szTime = szHour + ":" + szMin;
            if (pCtrl->ExtraPrecision())
            {
              szTime = CJulian::FormatTime(dDate);
            }
            else
            {
              //szHour.Format("%02d", pDate.Hour);
              //szMin.Format("%02d", pDate.Minute);
              //szTime = szHour + ":" + szMin;
              szTime.Format(_T("%02d:%02d"), pDate.Hour, pDate.Minute);
            }

            szDay.Format("%02d", pDate.Day);
//             if(szDay.GetLength() == 1){
//               szDay = "0" + szDay;
//             }
            szMonth = pDate.GetMonth(pDate.Month, 3, pCtrl);
//             if(szMonth.GetLength() == 1){
//               szMonth = "0" + szMonth;
//             }
            szYear.Format("%02d", (pDate.Year > 2000) ? pDate.Year - 2000 : pDate.Year - 1900);
//             if(szDay.GetLength() == 1){
//               szDay = "0" + szDay;
//             }

          }

          //if (szPrevDay != szDay) {
            szPrevDay = szDay;
            //szDate = szYear + " " + szMonth + " " + szDay; // + " " + szTime;
			szDate = szMonth + "/" + szYear; // + " " + szTime;
            //Level6 = nAvgCharWidth * (pCtrl->ExtraPrecision() ? 17 : 15);			
			Level6 = nAvgCharWidth * 6;
            lx += (int)(Level6 / 2);
          //}
          //else {


          //  szDate = szTime;          
          //}


          //Print text in calendar box
          rect.top = panelRect.top + 3;
          rect.bottom = panelRect.bottom - 3;
          rect.left = x + 3;
          rect.right = (int)(x + Level6 + 3);


          pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
		  //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
          xGrid++;

        }

		if (newMonth && lastDay > 8) {
			newMonth = false;
		}

		lastDay = pDate.Day;
		lastMonth = pDate.Month;
		lastYear = pDate.Year;

        if (dDate - startTime > hr) startTime = 0;
        incr += x - lx;
        lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        if ((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
          pCtrl->xGridMap[xGrid] = rect.left - 3;
      }

    }
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();

    if(pCtrl->showXGrid){
      pCtrl->xGridMap.resize(xGrid + 1);
    }

	*/

    return;
  }
  /*
#pragma region RECORD
  // Use records for x labels
  int cnt = 0;
  if(pCtrl->recordLabels){        
    for(long period = pCtrl->startIndex; period != pCtrl->endIndex; ++period){
      cnt++;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)GetX(cnt, pCtrl);
//  End Of Revision
      incr += x - lx;
      if(incr > Level7){
        incr = 0;
        //Draw vertical line
		  pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Display the label
        szTemp.Format("%d", period + 1);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level7 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szTemp, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szTemp, -1, &rect, DT_SINGLELINE | DT_LEFT);        
        xGrid++;
      }   
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
    pDC->SelectObject(pOldBrush);
    pDC->SelectObject(pOldPen);
    pDC->SelectObject(pOldFont);
    newFont.DeleteObject();

    if(pCtrl->showXGrid){
      pCtrl->xGridMap.resize(xGrid + 1);
    }

    return;
  } 
#pragma endregion
  */
  /*
  // What level will fit on the screen?
  if(Level5 <= dTradeWeek){
    incr = (int)Level5;
    for(long period = 0; period != rcnt; ++period){
      x = (int)GetX(period + 1, pCtrl);
      if(incr > Level5)
      {
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Get Julian date
        dDate = GetDate(period, x, pCtrl);
        pDate = FromJulianDateToCDate(dDate);       
        if(pDate.IsValid()){
          //Format date
          COleDateTime vaTimeDate;
          vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
                pDate.Hour, pDate.Minute, pDate.Second);
          //Display the 3-character month and 2-digit weekday
          vaTimeDate.Format(0, LANG_USER_DEFAULT);
        
          szDay.Format("%d", vaTimeDate.GetDay());
          if(szDay.GetLength() == 1){
            szDay = "0" + szDay;
          }
          CString szMonth = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
          szDate = szMonth + " " + szDay;
        }

        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level5 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
      }
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level4 <= dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level4;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)(GetX(period + 1, pCtrl));
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 10, pCtrl);
      }
      if(szDate == "January"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate + " Jan";
      }
      if(incr > Level4 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level4 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level3 + 2 < dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level3;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)(GetX(period + 1, pCtrl));
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      if(szDate == "Jan"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate.Mid(2);
      }
      if(incr > Level3 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level3 + 4);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
        pCtrl->xGridMap[xGrid] = rect.left - 3;
//  End Of Revision
    }
  }
  else if(Level2 <= dTradeMonth){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level2;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      x = (int)GetX(period + 1, pCtrl);
//  End Of Revision
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      //Format date
      COleDateTime vaTimeDate;
      if(pDate.IsValid()){
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      if(szDate == "Jan"){
        szDate.Format("%d", pDate.Year);
        szDate = szDate.Mid(2);
        szTemp = szDate;
      }
      else{
        szTemp = szDate.Mid(0,1);
      }
      if(incr > Level2 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.left = (int)(x + 3);
        rect.right = (int)(x + Level3 + 3);
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)   
		pCtrl->pdcHandler->DrawText(szTemp, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szTemp, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((int)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else if(Level1 <= dTradeYear){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
    incr = (int)Level1;
//  End Of Revision
    szCache = "#";
    for(long period = 0; period != rcnt; ++period){
      x = (int)(GetX(period + 1, pCtrl));
      if(x == -1) break;
      //Get Julian date
      dDate = GetDate(period, x, pCtrl);
      pDate = FromJulianDateToCDate(dDate);
      if(pDate.IsValid()){
        //Format date
        COleDateTime vaTimeDate;
        vaTimeDate.SetDateTime(pDate.Year,pDate.Month,pDate.Day,
              pDate.Hour, pDate.Minute, pDate.Second);
        //Display month
        vaTimeDate.Format(0, LANG_USER_DEFAULT);
        szDate = pDate.GetMonth(vaTimeDate.GetMonth(), 3, pCtrl);
      }
      szDate.Format("%d", pDate.Year);
      if(incr > Level1 && szCache != szDate){
        incr = 0; //Reset
        //Draw vertical line
		  pCtrl->pdcHandler->DrawLine(CPointF(x, panelRect.bottom), CPointF(x, panelRect.top), 1, 0, pCtrl->foreColor,  pDC, true);
        //pDC->MoveTo(x, panelRect.bottom);
        //pDC->LineTo(x, panelRect.top);
        //Print text in calendar box
        rect.top = panelRect.top + 3;
        rect.bottom = panelRect.bottom - 3;
        rect.left = x + 3;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
        rect.right = (int)(x + Level3 + 3);
//  End Of Revision
		pCtrl->pdcHandler->DrawText(szDate, rect, "Arial", 12, DT_LEFT, pCtrl->foreColor, 255, pDC);
        //pDC->DrawText(szDate, -1, &rect, DT_SINGLELINE | DT_LEFT);
        xGrid++;
        
      }
      szCache = szDate;
      incr += x - lx;
      lx = x;
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      if((long)pCtrl->xGridMap.size() > xGrid)
//  End Of Revision
        pCtrl->xGridMap[xGrid] = rect.left - 3;
    }
  }
  else{
    //Can't display a calendar
  }


  if(pCtrl->showXGrid){
    pCtrl->xGridMap.resize(xGrid + 1);
  }
  
  pDC->SelectObject(pOldBrush);
  pDC->SelectObject(pOldPen);
  pDC->SelectObject(pOldFont);
  newFont.DeleteObject();
  */
}


// Converts a Julian date to a CDate object
// (calls FormatDate to return a date format 
// specific to this machine's locale)
CDate CCalendarPanel::FromJulianDateToCDate(double jdate)
{ 

//   double j1 = 0;
//   double j2 = 0;
//   double j3 = 0;
//   double j4 = 0;
//   double j5 = 0;
// 
//     //Get the date from the Julian day number
//     
//     double intgr = floor(jdate);
//     double frac = jdate - intgr;
//     double gregjd = 2299161;
//     if(intgr >= gregjd){ //Gregorian calendar correction
//         double tmp = floor(((intgr - 1867216) - 0.25) / 36524.25);
//         j1 = intgr + 1 + tmp - floor(0.25 * tmp);
//     }
//   else{
//         j1 = intgr;
//     }
// 
//     //Correction for half day offset
//     double dayfrac = frac + 0.5;
//     if(dayfrac >= 1){
//         dayfrac -= 1;
//         j1 += 1;
//     }
// 
//     j2 = j1 + 1524;
//     j3 = floor(6680 + ((j2 - 2439870) - 122.1) / 365.25);
//     j4 = floor(j3 * 365.25);
//     j5 = floor((j2 - j4) / 30.6001);
// 
//     double d = floor(j2 - j4 - floor(j5 * 30.6001));
//     double m = floor(j5 - 1);
//   if(m > 12){
//     m -= 12;
//   }
//         
//     double y = floor(j3 - 4715);
//     if(m > 2){
//         y -= 1;
//     }
//     if(y <= 0){
//         y -= 1;
//     }
//     
//     //Get time of day from day fraction
//     double hr = floor(dayfrac * 24);
//     double mn = floor((dayfrac * 24 - hr) * 60);
//     double f = ((dayfrac * 24 - hr) * 60 - mn) * 60;
//     double sc = floor(f)+3;
//     f -= sc;
//     if(f > 0.5){
//         sc += 1;
//     }
// 
//     if(y < 0){
//         y = -y;
//     }
// 
//   if(sc > 59){
//     sc = sc - 60;
//     mn += 1;
//   }
// 
//   if(mn > 59){
//     mn = mn - 60;
//     hr += 1;
//   }
// 
//   // Update 9/9/05
//   if(hr == 24){
//     hr = 0;
//     mn = 0;
//     sc = 59;
//   }

//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
//return CDate((int)m,(int)d,(int)y,(int)hr,(int)mn,(int)sc);
//  End Of Revision
  
  MMDDYYHHMMSS date = CJulian::FromJDate(jdate);
  return CDate(date.Month, date.Day, date.Year, date.Hour, date.Minute, date.Second);
}

// Returns the date format specific to this machine's locale
// LPCTSTR CCalendarPanel::FormatDate(int month, int day, int year, 
//                    int hour, int minute, int second)
// {
//   COleDateTime vaTimeDate;    
//   vaTimeDate.SetDateTime(year,month,day,hour,minute,second);    
//     return vaTimeDate.Format(0,LANG_USER_DEFAULT);
// }


double CCalendarPanel::GetX(int period, CStockChartXCtrl* pCtrl)
{

//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
  if(pCtrl->priceStyle != psStandard && (int)pCtrl->xMap.size() > period){
//  End If
    return pCtrl->xMap[period-1];
  }
  else{
    return pCtrl->panels[0]->GetX(period);
  }
}


double CCalendarPanel::GetDate(int period, int x, CStockChartXCtrl* pCtrl){
  
  // Special handling for different price styles
  if(pCtrl->priceStyle != psStandard && pCtrl->xMap.size() > 0){
    if(pCtrl->priceStyle == psKagi){
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      period = (int)pCtrl->panels[0]->GetReverseX(pCtrl->xMap[period] + 1);
//  End Of Revision
    }
    else{
//  Revision 6/10/2004 made by Katchei
//  Addition of type cast (int)
      period = (int)pCtrl->panels[0]->GetReverseX(pCtrl->xMap[period]);
//  End Of Revision
    }
  }
  
  long record = period + pCtrl->startIndex;
  if(record > pCtrl->RecordCount()) record = 0;
  if(record < 0) record = 0;

  return pCtrl->panels[0]->series[0]->data_slave[record].jdate;
  

}
