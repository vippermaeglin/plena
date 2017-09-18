using System;

namespace M4.DataServer.Interface
{
  public class Ping
  {
    public DateTime RequestTimeStamp { get; set; }

    public DateTime AnswerTimeStamp { get; set; }

    public int IntervalKeepAliveMillis { get; set; } 

    public Ping()
    {
      RequestTimeStamp = DateTime.MinValue;
      AnswerTimeStamp = DateTime.MinValue;
    }

    public Ping(DateTime requestTimeStamp, DateTime answerTimeStamp)
    {
      RequestTimeStamp = requestTimeStamp;
      AnswerTimeStamp = answerTimeStamp;
    }

    /// <summary>
    /// Returns the durarion of ping command in milliseconds
    /// </summary>
    public double Duration
    {
      get
      {
        return AnswerTimeStamp == DateTime.MinValue ? 0 : (AnswerTimeStamp - RequestTimeStamp).TotalMilliseconds;
      }
    }

    public static DateTime GetUtcTime()
    {
      return DateTime.Now.ToUniversalTime();
    }
  }
}
