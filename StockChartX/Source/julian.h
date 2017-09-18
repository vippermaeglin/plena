
#ifndef __JULIAN_H__
#define __JULIAN_H__

// All code in this project copyright 1998-2008 Modulus Financial Engineering, Inc.
// Code may not be redistributed.

// we use this year as the origin in the code below
static const int ORIGIN_YEAR = 1583;

// days from julian origin to 31 dec. 1582
// ie. julian date of noon on 31 dec. 1582
static const int ORIGIN_JULIAN = 2299238;

// days in each month in a non-leap year.
static const int monthDays[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


struct MMDDYYHHMMSS{
  short Year;
  short Month;
  short Day;
  short Hour;
  short Minute;
  short Second;
  short Millisecond;
};
// Julian date converter.
// The following conventions are used:
// 1 <= month <= 12
// 1 <= day <= 31
// 0 <= hour <= 23
// 0 <= minute <= 59
// 0 <= second <= 59
// 0 <= millisecond <= 59
// This code works for any date after 1/1/1583 down to the microsecond
class CJulian
{
public:
  // returns true if year is leap.
  static BOOL inline IsLeap(const int year)
  {
    return ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0);
  }

  // returns the number of days from the julian origin to a specific date
  // or in other words, the julian date for noon that day
  static int DaysFromDate(const int year, const int month, const int day)
  {
    int i, d = 0;
    // not very efficient, but shouldn't be a problem for any realistic date
    for (i = ORIGIN_YEAR; i < year; i++)
    {
      d += 365;
      if (IsLeap(i))
        d++;
    }
    if (IsLeap(year) && month > 2)
      d++;
    for (i = 1; i < month; i++)
      d += monthDays[i - 1];
    d += day;
    return d + ORIGIN_JULIAN;
  }

  // computes the year month and day, given the julian date for noon that day
  static void DaysToDate(const int julian_days, short &year, short& month, short &day)
  {
    int d, y, m, tmp;
    d = julian_days - ORIGIN_JULIAN;              // days from 11/31/1582
    y = ORIGIN_YEAR;
    while (d > (tmp = 365 + (IsLeap(y) ? 1 : 0)))
    {
      d -= tmp;
      y++;
    }
    m = 1;
    while (d > (tmp = monthDays[m - 1] + ((IsLeap(y) && m == 2) ? 1 : 0)))
    {
      d -= tmp;
      m++;
    }
    year = y;
    month = m;
    day = d;
  }


  // converts a date into a julian date
  static double ToJulianDate(const int year, const int month, const int day, const int hour, const int minute, const int second, const int millisecond)
  {
    double julian;
    int ms = millisecond;
    if(millisecond > 995) ms = 0;
    // date component
    julian = (double)DaysFromDate(year, month, day);
    // time component
    julian += (((((double)ms + 0.5)/1000.0 + (double)second)/60.0 + (double)minute)/60.0 + (double)hour)/24.0;
    // origin is at noon
    return julian - 0.5;
  }

  // converts a julian date into a date.
  static MMDDYYHHMMSS FromJDate(const double julian)
  {
    
    int jdays;
    double remainder, _hour, _minute, _second, _millisec;
    
    // separate the integer and decimal part of the julian date
    jdays = (int)julian;
    remainder = julian - (double)jdays;
    
    // make adjustment to account for the origin being at noon
    if (remainder < 0.5)
    {
      remainder += 0.5;
    }
    else
    {
      jdays++;
      remainder -= 0.5;
    }
    
    MMDDYYHHMMSS ret;
    
    // compute the date
    DaysToDate(jdays, ret.Year, ret.Month, ret.Day);
    
    // compute the time
    _hour = remainder * 24.0;
    ret.Hour = (int)_hour;
    remainder = _hour - (double)ret.Hour;
    
    _minute = remainder * 60.0;
    ret.Minute = (int)_minute;
    remainder = _minute - (double)ret.Minute;
    
    _second = remainder * 60.0;
    ret.Second = (int)_second;
    remainder = _second - (double)ret.Second;
    
    _millisec = remainder * 1000.0;
    if(_millisec  > 995) _millisec = 0;//
    ret.Millisecond = (int)_millisec;
    
    return ret;
  }

  // Returns a Gregorian date
  static CString FromJulianDate(double JDate)
  {
    MMDDYYHHMMSS date = FromJDate(JDate);
    
    CString dt = "error";   
    if(date.Month > 0 && date.Month < 13 && date.Day > 0 && date.Day < 32 && 
      date.Year > 0)
    {
      dt = FormatDate(date.Month,date.Day,date.Year, date.Hour,date.Minute,date.Second, date.Millisecond);
    }
    
    return dt;
  }

  // Returns the date format specific to this machine's locale
  static CString FormatDate(int Month, int Day, int Year, 
    int Hour, int Minute, int Second, int Millisecond)
  {
    COleDateTime vaTimeDate;
    
    if(Hour == 24)
    {
      Hour = 0; 
      Minute = 0; 
      Second = 0; 
      Millisecond = 0;
      Day += 1;
    }

    if(Minute == 60) Minute = 0;
    if(Second == 60) Second = 0;
    
    SYSTEMTIME myTime;
    myTime.wMonth = Month;
    myTime.wDay = Day;
    myTime.wYear = Year;
    myTime.wHour = Hour;
    myTime.wMinute = Minute;
    myTime.wSecond = Second;
    myTime.wMilliseconds = Millisecond;
    TCHAR timeBuf[20+1] = _T("");
    TCHAR dateBuf[20+1] = _T("");
    GetTimeFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, timeBuf, 20);
    GetDateFormat(LOCALE_USER_DEFAULT, 0, &myTime, NULL, dateBuf, 20);
    CString szDT = dateBuf;
    szDT += _T(" ");
    szDT += timeBuf;
    if(strlen(dateBuf) == 0) return _T("No Date Time");
    return szDT;


    //vaTimeDate.SetDateTime(Year,Month,Day,Hour,Minute,Second);
    //return vaTimeDate.Format(0,LANG_USER_DEFAULT);
  }

  // Returns the date format specific to this machine's locale
  static CString FormatTime(double jDate)
  {
    MMDDYYHHMMSS datetime = FromJDate(jDate);

    if(datetime.Hour == 24)
    {
      datetime.Hour = 0; 
      datetime.Minute = 0; 
      datetime.Second = 0; 
      datetime.Millisecond = 0;
      datetime.Day += 1;
    }
    
    if(datetime.Minute == 60) datetime.Minute = 0;
    if(datetime.Second == 60) datetime.Second = 0;

    TCHAR timeBuf[13] = {0};

    _stprintf(timeBuf, _T("%02d:%02d:%02d:%03d"), datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);

    return timeBuf;
  }

};

#endif