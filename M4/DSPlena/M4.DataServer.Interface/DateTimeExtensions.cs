using System;

namespace M4.DataServer.Interface
{
  public static class DateTimeExtensions
  {
    public static readonly int[] DaysToMonth365 = new[] {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
    public static readonly int[] DaysToMonth366 = new[] {0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};
 

    public static long DateToTicks(int year, int month, int day)
    {
      if (((year >= 1) && (year <= 9999)) && ((month >= 1) && (month <= 12)))
      {
        int[] numArray = DateTime.IsLeapYear(year) ? DaysToMonth366 : DaysToMonth365;
        if ((day >= 1) && (day <= (numArray[month] - numArray[month - 1])))
        {
          int num = year - 1;
          int num2 = ((((((num * 365) + (num / 4)) - (num / 100)) + (num / 400)) + numArray[month - 1]) + day) - 1;
          return (num2 * 864000000000L);
        }
      }
      throw new ArgumentOutOfRangeException();
    }

    public static long TimeToTicks(int hour, int minute, int second)
    {
      long num = ((hour * 3600L) + (minute * 60L)) + second;
      if ((num > 922337203685L) || (num < -922337203685L))
      {
        throw new ArgumentOutOfRangeException();
      }
      return (num * 10000000L);
    }

    public static long ToBinary(this DateTime self)
    {
      long num = DateToTicks(self.Year, self.Month, self.Day) + TimeToTicks(self.Hour, self.Minute, self.Second);
      if ((self.Millisecond < 0) || (self.Millisecond >= 1000))
      {
        throw new ArgumentOutOfRangeException();
      }
      num += self.Millisecond * 10000L;
      if ((num < 0L) || (num > 3155378975999999999L))
      {
        throw new ArgumentException();
      }
      num = num | (((long)self.Kind) << 62);

      return num;
    }

    public static DateTime FromBinary(long dateData)
    {
      return new DateTime(dateData);
    }

    public static bool IsMidnight(this DateTime self)
    {
      return (self.Hour == 0)
             && (self.Minute == 0)
             && (self.Second == 0);
    }

    public static string Serialize(this DateTime self)
    {
      return string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:0000}",
                           self.Year, self.Month, self.Day, self.Hour, self.Minute,
                           self.Second, self.Millisecond);
    }

    public static DateTime DeSerialize(string dt)
    {
      return new DateTime(
        Convert.ToInt32(dt.Substring(0, 4)), //year
        Convert.ToInt32(dt.Substring(4, 2)), //month
        Convert.ToInt32(dt.Substring(6, 2)), //day
        Convert.ToInt32(dt.Substring(8, 2)), //hour
        Convert.ToInt32(dt.Substring(10, 2)), //minute
        Convert.ToInt32(dt.Substring(12, 2)), //second
        Convert.ToInt32(dt.Substring(14, 2)) //ms
        );
    }
  }
}
