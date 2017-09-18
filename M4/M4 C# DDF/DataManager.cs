/* WARNING! This program and source code is owned and licensed by 
   Modulus Financial Engineering, Inc. http://www.modulusfe.com
   Viewing or use this code requires your acceptance of the license
   agreement found at http://www.modulusfe.com/support/license.pdf
   Removal of this comment is a violation of the license agreement.
   Copyright 2007-2010 by Modulus Financial Engineering, Inc. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using M4Core.Entities;

namespace M4
{
    public class HistoryRequestAnswer
    {
        public List<M4.DataServer.Interface.BarData> Data = new List<DataServer.Interface.BarData>();
        public bool HasError;
        public string ErrorMessage;
    }
    public partial class DataManager : UserControl
    {

        public class BarData
        {
            public DateTime TradeDate;
            public double OpenPrice;
            public double HighPrice;
            public double LowPrice;
            public double ClosePrice;
            public double Volume;

            public override string ToString()
            {
                return string.Format("TradeDate: {0}, OpenPrice: {1}, HighPrice: {2}, LowPrice: {3}, ClosePrice: {4}, Volume: {5}",
                  TradeDate, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume);
            }
        }

        //Symbol watch
        private class WatchItem
        {
            public string Symbol;
            public Periodicity BarType;
            public int BarSize;
            public IDataSubscriber Subscriber;
            public List<M4.DataServer.Interface.BarData> Bars;
            public bool IsNewBar;
        }

        //To cache volume
        private class VolumeCache
        {
            public string Symbol;
            public double LastDailyVolume;
            public double LastTickVolume;
        }

        // See PrimeBars mnethod
        public class PrimeBar
        {
            public string Symbol;
            public Periodicity BarType;
            public int BarSize;
            public M4.DataServer.Interface.BarData Bar;
        }

        private readonly List<WatchItem> m_WatchList = new List<WatchItem>();
        private readonly List<VolumeCache> m_VolumeCache = new List<VolumeCache>();

        private readonly List<PrimeBar> m_primeBars = new List<PrimeBar>();

        public DateTime m_LastHistoryTimeStamp;

        //This singleton is managed by frmData. Clients that implement IDataSubscriber may receive events from here.

        //m_WatchList(Symbol, Periodicity, IDataSubscriber) contains all symbols in the watch list of this DataManager.
        //Symbols are automatically placed in m_WatchList when GetHistory is called.

        public DataManager()
        {
            InitializeComponent();
        }

        private void LogFile(string item, string function)
        {
            System.IO.StreamWriter SW = System.IO.File.AppendText(Application.StartupPath + @"\" + function + ".txt");
            SW.WriteLine(item);
            SW.Close();
        }

        //The RealTimeUpdate function is called from within the data feed API event handler
        //to cache the tick data until a new bar is formed. After a new bar has formed this
        //function will call the IDataSubscriber.BarUpdate() function for all controls found
        //in the watch list. You may also implement other fields such as bid, ask, size, etc.
        //Volume must be total DAILY volume (not intraday volume)!
        public void RealTimeUpdate(string Symbol, DateTime TradeDate, float LastPrice, double Volume)
        {
            //#if DEBUG
            //LogFile(TradeDate + "\t" + Symbol + "\t" + LastPrice, "RealTimeUpdate");
            //#endif

            if (TradeDate.Hour >= 16)
            { // TODO: Optional, filter updates after market hours
                return;
            }

            if (LastPrice == 0) return;

            //Set the tick volume based on the daily volume if required by your data provider
            Volume = GetTickVolume(Symbol, (long)Volume);

            //Symbol's last price has just arrived so find the subscribers who are watching it
            for (int n = 0; n < m_WatchList.Count; n++)
            {
                if (n >= m_WatchList.Count) break; //Multithreaded
                if (m_WatchList[n].Symbol != Symbol) continue;

                //The subscriber is watching this symbol so update his bar data
                //First, get the last time that his data was updated
                List<M4.DataServer.Interface.BarData> bars = m_WatchList[n].Bars;
                double newBar = 0;
                if (bars.Count > 0)
                {
                    newBar = 0;
                    DateTime lastTradeDate = bars[bars.Count - 1].TradeDate;

                    switch (m_WatchList[n].BarType)
                    {
                        case Periodicity.Secondly:
                            newBar = (TradeDate - lastTradeDate).TotalSeconds / m_WatchList[n].BarSize;
                            break;
                        case Periodicity.Minutely:
                            newBar = (TradeDate - lastTradeDate).TotalMinutes / m_WatchList[n].BarSize;
                            break;
                        case Periodicity.Hourly:
                            newBar = (TradeDate - lastTradeDate).TotalHours / m_WatchList[n].BarSize;
                            break;
                        case Periodicity.Daily:
                            newBar = (TradeDate - lastTradeDate).TotalDays / m_WatchList[n].BarSize;
                            break;
                        case Periodicity.Weekly:
                            newBar = (TradeDate - lastTradeDate).TotalDays / 5;
                            break;
                    }

                }
                else //First bar in the array is starting for the most recent historic request
                {
                    lock (m_primeBars)
                    {
                        int i;
                        bool primed = false;
                        for (i = 0; i < m_primeBars.Count; ++i)
                        {
                            if (m_primeBars[i].Symbol == Symbol &&
                                m_primeBars[i].BarType == m_WatchList[i].BarType &&
                                m_primeBars[i].BarSize == m_WatchList[i].BarSize)
                            {
                                bars.Add(m_primeBars[i].Bar);
                                primed = true;
                                break;
                            }
                        }

                        if (primed)
                        {
                            m_primeBars.RemoveAt(i);
                        }
                        else
                        {
                            Debug.Assert(bars.Count == 0, "PrimeBars method was not called after GetHistory");
                            bars.Add(new M4.DataServer.Interface.BarData
                            {
                                TradeDate = NextBarTime(m_LastHistoryTimeStamp, 1, m_WatchList[n].BarType),
                                OpenPrice = LastPrice,
                                HighPrice = LastPrice,
                                LowPrice = LastPrice,
                                ClosePrice = LastPrice,
                                VolumeF = 0, //Volume
                            });
                        }
                    }
                }



                // OPTIONAL:
                // From the time when you request historic data then receive the last historic bar
                // and from the time when the first real time udpates start, there may be missing data
                // that will be shown on a chart as several flat bars. The code on the next line will
                // limit the number of flat bars to three because most markets do not have more than 
                // a few flat bars unless there is very little volume. You may remove this code without
                // any negative impacts on the rest of the program.
                if (bars[bars.Count - 1].OpenPrice == bars[bars.Count - 1].HighPrice &&
                  bars[bars.Count - 1].OpenPrice == bars[bars.Count - 1].LowPrice &&
                  bars[bars.Count - 1].OpenPrice == bars[bars.Count - 1].ClosePrice) newBar = 0;




                //If newBar = BarSize then we have a new bar. If newBar > BarSize then we are missing
                //data, but this is the fault of the API if data is missing. Perhaps the data feed API
                //does not send an event when the price has not changed (most likely), or the data
                //feed API connection was down temporarily. At any rate, we cannot recover lost data
                //if it is the fault of the API, so nothing should be done here. Just finish the bar.
                if (newBar >= 1)
                {
                    // Create a new bar                    
                    bars.Add(new M4.DataServer.Interface.BarData());
                    bars[bars.Count - 1].TradeDate = NextBarTime(bars[bars.Count - 2].TradeDate, m_WatchList[n].BarSize, m_WatchList[n].BarType);
                    bars[bars.Count - 1].OpenPrice = LastPrice;
                    bars[bars.Count - 1].HighPrice = LastPrice;
                    bars[bars.Count - 1].LowPrice = LastPrice;
                    bars[bars.Count - 1].ClosePrice = LastPrice;
                    m_WatchList[n].IsNewBar = true;
                    m_WatchList[n].Bars = bars;
                }
                else
                {
                    //Update the current bar
                    bars[bars.Count - 1].ClosePrice = LastPrice;
                    if (LastPrice > bars[bars.Count - 1].HighPrice) bars[bars.Count - 1].HighPrice = LastPrice;
                    if (LastPrice < bars[bars.Count - 1].LowPrice) bars[bars.Count - 1].LowPrice = LastPrice;
                    bars[bars.Count - 1].VolumeF += Volume;
                    m_WatchList[n].IsNewBar = false;
                    m_WatchList[n].Bars = bars;
                }
            }

            //Send the real-time updates to all subscribed clients        
            lock (m_WatchList)
            {
                for (int n = 0; n < m_WatchList.Count; n++)
                {
                    if (m_WatchList[n].Symbol != Symbol) continue;

                    if (m_WatchList[n].Subscriber.GetHandle() == IntPtr.Zero)
                    {
                        //The subscriber is no longer available
                        m_WatchList.RemoveAt(n--);
                    }
                    else
                    {
                        m_WatchList[n].Subscriber.PriceUpdate(Symbol, TradeDate, LastPrice, (long)Volume);
                        m_WatchList[n].Subscriber.BarUpdate(Symbol, (M4.DataServer.Interface.Periodicity)m_WatchList[n].BarType, m_WatchList[n].BarSize,
                            m_WatchList[n].Bars[m_WatchList[n].Bars.Count - 1], m_WatchList[n].IsNewBar);
                    }
                }
            }
        }


        // This function must be called after requesting historic data from a data server.
        // Primes historic bars in m_WatchList.Bars
        // Must be called prior to calling RealTimeUpdate if GetHistory was used.
        public void PrimeBars(PrimeBar item)
        {
            foreach (PrimeBar pb in m_primeBars)
                if (pb.Symbol == item.Symbol && pb.BarType == item.BarType)
                    return;

            m_primeBars.Add(item); // This array is required because the 
            // GetHistoryAsync function is asynchronous and the Watch function is also,
            // which requires the data provider to respond within several ms with a price
            // update. m_primeBars is used in RealTimeUpdate.
        }



        //Subtracts the current daily volume from the previous daily volume to return volume 
        //for the current tick price. Only required for data APIs that do not supply tick volume.
        private double GetTickVolume(string Symbol, long CurrentDailyVolume)
        {
            for (int n = 0; n < m_VolumeCache.Count - 1; n++)
            {
                if (m_VolumeCache[n].Symbol == Symbol)
                {
                    if (m_VolumeCache[n].LastDailyVolume > 0 && CurrentDailyVolume > 0)
                    {
                        m_VolumeCache[n].LastTickVolume = CurrentDailyVolume - m_VolumeCache[n].LastDailyVolume;
                    }
                    m_VolumeCache[n].LastDailyVolume = CurrentDailyVolume;
                    return m_VolumeCache[n].LastTickVolume;
                }
            }
            //Symbol wasn't found meaning no volume has been cached yet for this symbol
            VolumeCache vc = new VolumeCache { Symbol = Symbol };
            m_VolumeCache.Add(vc);
            return 0;
        }


        //Requests the most recent bar history and subscribes the client to real time updates.
        //Forms and controls must implement IDataSubscriber with the BarUpdate() function.    
        //BarSize is the number of minutes/hours/days per bar (based on Periodicity).
        //This is a sample function only - it must be overridden by the client.
        private readonly Random _r = new Random();
        public virtual List<M4.DataServer.Interface.BarData> GetHistory(string symbol, IDataSubscriber subscriber, M4Core.Entities.Periodicity barType, int barSize, int barCount, string source,
          Action<HistoryRequestAnswer> onCompleted)
        {
            List<M4.DataServer.Interface.BarData> data = new List<M4.DataServer.Interface.BarData>();
            //Request historic bar data from your data provider and return it here.        
            for (int bar = 0; bar < barSize; bar++)
            {
                data.Add(new M4.DataServer.Interface.BarData
                {
                    TradeDate = DateTime.Now.AddHours(bar),
                    OpenPrice = (float)_r.NextDouble(),
                    HighPrice = (float)_r.NextDouble(),
                    LowPrice = (float)_r.NextDouble(),
                    ClosePrice = (float)_r.NextDouble()
                });
            }

            AddWatch(symbol, subscriber, barType, barSize);

            return data;
        }

        //Adds a selection to the watch list. It will automatically
        //be removed from the watch list after the client is destroyed.
        public void AddWatch(string Symbol, IDataSubscriber Subscriber, Periodicity BarType, int BarSize)
        {
            if (Subscriber == null) return;

            m_WatchList.Add(new WatchItem
            {
                Symbol = Symbol,
                BarType = BarType,
                BarSize = BarSize,
                Subscriber = Subscriber,
                Bars = new List<M4.DataServer.Interface.BarData>()
            });
        }

        //Removes a symbol from the watch list and returns true if no other subscribers are
        //watching that symbol (allows the overriding class to remove the symbol from the API).
        public bool RemoveWatch(string Symbol, IDataSubscriber Subscriber)
        {
            bool notInUse = false;
            for (int n = 0; n < m_WatchList.Count; n++)
            {
                if (n >= m_WatchList.Count) break;
                if (m_WatchList[n].Symbol == Symbol && m_WatchList[n].Subscriber != null)
                {
                    if (m_WatchList[n].Subscriber.GetHandle() == Subscriber.GetHandle())
                    {
                        m_WatchList.RemoveAt(n);
                        notInUse = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return notInUse;
        }

        //Returns true if the symbol is being watched
        public bool IsWatched(string Symbol)
        {
            return m_WatchList.Any(t => t.Symbol == Symbol);
        }

        //For testing
        public List<M4.DataServer.Interface.BarData> GetAllBars(string Symbol, int BarSize, Periodicity BarType)
        {
            return (from t in m_WatchList
                    where t.BarSize == BarSize && t.BarType == BarType && t.Symbol == Symbol
                    select t.Bars)
              .FirstOrDefault();
        }


        // Rounds time to the nearest periodicity
        public DateTime RoundTime(DateTime d, Periodicity rt)
        {
            DateTime dtRounded = new DateTime();

            switch (rt)
            {
                case Periodicity.Secondly:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
                    break;
                case Periodicity.Minutely:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
                    break;
                case Periodicity.Hourly:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
                    break;
                case Periodicity.Daily:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                    break;
            }

            return dtRounded;
        }

        // Increments the bar time by the number of bars
        public DateTime NextBarTime(DateTime d, int numBars, Periodicity rt)
        {
            switch (rt)
            {
                case Periodicity.Secondly:
                    return d.AddSeconds(numBars);
                case Periodicity.Minutely:
                    return d.AddMinutes(numBars);
                case Periodicity.Hourly:
                    return d.AddHours(numBars);
                case Periodicity.Daily:
                    return d.AddDays(numBars);
            }

            throw new InvalidOperationException();
        }
    }

    public interface IDataSubscriber
    {
        IntPtr GetHandle();
        void BarUpdate(string Symbol, M4.DataServer.Interface.Periodicity BarType, int BarSize, M4.DataServer.Interface.BarData Bar, bool IsNewBar);
        void PriceUpdate(string Symbol, DateTime TradeDate, double LastPrice, long Volume);
    }
}
