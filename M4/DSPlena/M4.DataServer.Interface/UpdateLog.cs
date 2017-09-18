using System;
using ExtremeDB;

namespace M4.DataServer.Interface
{
    [Persistent(Disk = true)]
    public class UpdateLog
    {
        [Indexable(Unique = true)] // create unique (tree) eXtremeDB index by "DateTimeTick" field
        public long TradeDateTicks;
        public DateTime TradeDate;
        public bool Succeeded = false;

        public UpdateLog()
        {
            TradeDate = DateTime.MinValue;
        }

        public UpdateLog(DateTime tradeDate, bool succeeded)
        {
            TradeDate = tradeDate;
            TradeDateTicks = TradeDate.Ticks;
            Succeeded = succeeded;
        }

    }

}
