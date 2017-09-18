using System;

namespace M4.DataServer.Interface
{
  public partial class HistoryRequest
  {
    public string RequestId { get; set; }    public string Symbol { get; set; }

    public Periodicity Periodicity { get; set; }

    public DateTime LastRecordTime { get; set; }

    public float LastRecordValue { get; set; }

    public int BarSize { get; set; }

    public int BarCount { get; set; }
  }
}
