/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;

namespace M4
{
  ///<summary>
  ///</summary>
  public static class DateTimeEx
  {
    ///<summary>
    /// Converts a Datetime object to its julian representation
    ///</summary>
    ///<param name="self"></param>
    ///<returns></returns>
    public static double ToJDate(this DateTime self)
    {
      int ms = self.Millisecond;
      if (ms > 995) ms = 0;
      // date component
      double julian = self.DaysFromDate();
      // time component
      julian += ((((ms + 0.5) / 1000.0 + self.Second) / 60.0 + self.Minute) / 60.0 + self.Hour) / 24.0;
      // origin is at noon
      return julian - 0.5;
    }

    ///<summary>
    /// Converts a julian date into a DateTime
    ///</summary>
    ///<param name="julian"></param>
    ///<returns></returns>
    public static DateTime FromJDate(double julian)
    {
      // separate the integer and decimal part of the julian date
      int jdays = (int)julian;
      double remainder = julian - jdays;

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

      short year = 0, month = 0, day = 0;
      // compute the date
      DaysToDate(jdays, ref year, ref month, ref day);

      // compute the time
      double dHhour = remainder * 24.0;
      int hour = (int)dHhour;
      remainder = dHhour - hour;

      double dMinute = remainder * 60.0;
      int minute = (int)dMinute;
      remainder = dMinute - minute;

      double dSecond = remainder * 60.0;
      int second = (int)dSecond;
      remainder = dSecond - second;

      double millisec = remainder * 1000.0;
      if (millisec > 995) millisec = 0;//

      return new DateTime(year, month, day, hour, minute, second, (int)millisec);
    }

    // we use this year as the origin in the code below
    const int ORIGIN_YEAR = 1583;
    // days from julian origin to 31 dec. 1582
    // ie. julian date of noon on 31 dec. 1582
    const int ORIGIN_JULIAN = 2299238;

    // days in each month in a non-leap year.
    private static readonly int[] monthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


    /// <summary>
    /// returns the number of days from the julian origin to a specific date
    /// or in other words, the julian date for noon that day
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static int DaysFromDate(this DateTime self)
    {
      int i, d = 0;
      // not very efficient, but shouldn't be a problem for any realistic date
      for (i = ORIGIN_YEAR; i < self.Year; i++)
      {
        d += 365;
        if (IsLeap(i))
          d++;
      }
      if (IsLeap(self.Year) && self.Month > 2)
        d++;
      for (i = 1; i < self.Month; i++)
        d += monthDays[i - 1];
      d += self.Day;
      return d + ORIGIN_JULIAN;
    }

    ///<summary>
    /// computes the year month and day, given the julian date for noon that day
    ///</summary>
    ///<param name="julian_days"></param>
    ///<param name="year"></param>
    ///<param name="month"></param>
    ///<param name="day"></param>
    public static void DaysToDate(int julian_days, ref short year, ref short month, ref short day)
    {
      int tmp;
      int d = julian_days - ORIGIN_JULIAN;
      int y = ORIGIN_YEAR;
      while (d > (tmp = 365 + (IsLeap(y) ? 1 : 0)))
      {
        d -= tmp;
        y++;
      }
      int m = 1;
      while (d > (tmp = monthDays[m - 1] + ((IsLeap(y) && m == 2) ? 1 : 0)))
      {
        d -= tmp;
        m++;
      }
      year = (short)y;
      month = (short)m;
      day = (short)d;
    }


    /// <summary>
    /// returns true if year is leap
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static bool IsLeap(int year)
    {
      return ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0);
    }

    ///<summary>
    /// Checks whether a DateTime object has a Leap year
    ///</summary>
    ///<param name="self"></param>
    ///<returns></returns>
    public static bool IsLeap(this DateTime self)
    {
      return IsLeap(self.Year);
    }
  }
}
