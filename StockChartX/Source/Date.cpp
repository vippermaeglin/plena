// Date.cpp: implementation of the CDate class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "StockChartX.h"
#include "Date.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CDate::CDate()
{
	Month = 0;
	Day = 0;
	Year = 0;
	Hour = 0;
	Minute = 0;
	Second = 0;
}

CDate::CDate(int nMonth, int nDay, int nYear, int nHour, int nMinute, int nSecond)
{
	Month = nMonth;
	Day = nDay;
	Year = nYear;
	Hour = nHour;
	Minute = nMinute;
	Second = nSecond;
}

CDate::~CDate()
{

}

//Returns a CString month specified by length
//(1 = "J", 3 = "Jan", >3 = "January".
CString CDate::GetMonth(int nMonth, int nLength, CStockChartXCtrl* pCtrl)
{
	CString szRet = "";
	if(nLength <= 1){ //J
		switch(nMonth){
			case 1:
				szRet = "J";
				break;
			case 2:
				szRet = "F";
				break;
			case 3:
				szRet = "M";
				break;
			case 4:
				szRet = "A";
				break;
			case 5:
				szRet = "M";
				break;
			case 6:
				szRet = "J";
				break;
			case 7:
				szRet = "J";
				break;
			case 8:
				szRet = "A";
				break;
			case 9:
				szRet = "S";
				break;
			case 10:
				szRet = "O";
				break;
			case 11:
				szRet = "N";
				break;
			case 12:
				szRet = "D";
				break;
		}
	}
	else if(nLength == 3){ //Jan
		if(pCtrl->m_Language==0){ //English
			switch(nMonth){
				case 1:
					szRet = "Jan";
					break;
				case 2:
					szRet = "Feb";
					break;
				case 3:
					szRet = "Mar";
					break;
				case 4:
					szRet = "Apr";
					break;
				case 5:
					szRet = "May";
					break;
				case 6:
					szRet = "Jun";
					break;
				case 7:
					szRet = "Jul";
					break;
				case 8:
					szRet = "Aug";
					break;
				case 9:
					szRet = "Sep";
					break;
				case 10:
					szRet = "Oct";
					break;
				case 11:
					szRet = "Nov";
					break;
				case 12:
					szRet = "Dec";
					break;
			}
		}
		else if(pCtrl->m_Language==1){ //Portuguese
			switch(nMonth){
				case 1:
					szRet = "Jan";
					break;
				case 2:
					szRet = "Fev";
					break;
				case 3:
					szRet = "Mar";
					break;
				case 4:
					szRet = "Abr";
					break;
				case 5:
					szRet = "Mai";
					break;
				case 6:
					szRet = "Jun";
					break;
				case 7:
					szRet = "Jul";
					break;
				case 8:
					szRet = "Ago";
					break;
				case 9:
					szRet = "Set";
					break;
				case 10:
					szRet = "Out";
					break;
				case 11:
					szRet = "Nov";
					break;
				case 12:
					szRet = "Dez";
					break;
			}
		}
	}
	else if(nLength > 3){ //January
		if(pCtrl->m_Language==0){ //English
			switch(nMonth){
				case 1:
					szRet = "January";
					break;
				case 2:
					szRet = "February";
					break;
				case 3:
					szRet = "March";
					break;
				case 4:
					szRet = "April";
					break;
				case 5:
					szRet = "May";
					break;
				case 6:
					szRet = "June";
					break;
				case 7:
					szRet = "July";
					break;
				case 8:
					szRet = "August";
					break;
				case 9:
					szRet = "September";
					break;
				case 10:
					szRet = "October";
					break;
				case 11:
					szRet = "November";
					break;
				case 12:
					szRet = "December";
					break;
			}
		}
		else if(pCtrl->m_Language==1){ //Portuguese
			switch(nMonth){
				case 1:
					szRet = "Janeiro";
					break;
				case 2:
					szRet = "Fevereiro";
					break;
				case 3:
					szRet = "Março";
					break;
				case 4:
					szRet = "Abril";
					break;
				case 5:
					szRet = "Maio";
					break;
				case 6:
					szRet = "Junho";
					break;
				case 7:
					szRet = "Julho";
					break;
				case 8:
					szRet = "Agosto";
					break;
				case 9:
					szRet = "Setembro";
					break;
				case 10:
					szRet = "Outubro";
					break;
				case 11:
					szRet = "Novembro";
					break;
				case 12:
					szRet = "Dezembro";
					break;
			}
		}
	}
	return szRet;
}



bool CDate::IsValid()
{
	return (Month > 0 && Day > 0 && Year > 0 && 
		Hour > -1 && Minute > -1 && Second > -1);	
}