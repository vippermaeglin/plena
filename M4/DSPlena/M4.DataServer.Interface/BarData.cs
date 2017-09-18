using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;

namespace M4.DataServer.Interface
{
    public enum BaseType
    {
        Days,
        Minutes,
        Minutes15
    }

    public class RTAssetsInfo
    {
        #region Propriedades

        public string Symbol { get; set; }
        
        public double Last { get; set; }

        public string[] Time { get; set; }

        public double Variation { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Open { get; set; }

        public double Trades { get; set; }

        public double[] Volume { get; set; }

        public int Position { get; set; }

        public RTAssetsInfo()
        {
        }

        #endregion
    }

    public class PortfolioDataSet
    {
        public List<RTAssetsInfo> Assets { get; set; }
        public PortfolioDataSet()
        {
            Assets = new List<RTAssetsInfo>();
        }
        public void Add(RTAssetsInfo asset)
        {
            Assets.Add(asset);
        }
        public void Update(RTAssetsInfo asset)
        {
            int index = -1;
            index = Assets.FindIndex(a => a.Symbol == asset.Symbol);
            if (index != null && index != -1)
            {
                Assets[index].Close = asset.Close;
                Assets[index].High = asset.High;
                Assets[index].Last = asset.Last;
                Assets[index].Low = asset.Low;
                Assets[index].Open = asset.Open;
                Assets[index].Position = asset.Position;
                Assets[index].Symbol = asset.Symbol;
                Assets[index].Time = asset.Time;
                Assets[index].Trades = asset.Trades;
                Assets[index].Variation = asset.Variation;
                Assets[index].Volume = asset.Volume;
            }
        }
    }

    public class BarDataEventArgs : EventArgs
    {
        public BarData Bar;
        public BarDataEventArgs()
        {
        }
        public BarDataEventArgs(BarData bar)
        {
            Bar = bar;
        }
    }

    //[Index("byDate", Keys = new string[] { "TradeDateTicks" })]
    //[Index("bySymbol", Keys = new string[] { "Symbol" })]
    //[Index("byBaseSymbol", Keys = new string[] { "BaseType", "Symbol" })]
    //[Index("byDateSymbol", Keys = new string[] { "TradeDateTicks", "Symbol" }, Type = Database.IndexType.BTree)]
    ////To enable another base:
    //[Index("byBaseSymbolDate", Keys = new string[] { "BaseType", "Symbol", "TradeDateTicks" }, Unique = true, Type = Database.IndexType.BTree)]
    
    
    //[Persistent(InMemory = true)]
    public class BarData
    {
        //[Indexable(Unique = true, Descending = true)] // create unique (tree) eXtremeDB index by "DateTimeTick" field
        /// <summary>
        /// Date-time of trade represented by Ticks
        /// </summary>
        public long TradeDateTicks;
        /// <summary>
        /// Date-time of trade
        /// </summary>
        public DateTime TradeDate;
        /// <summary>
        /// Type of base 0 = Daily , 1 = Minute , 2 = 15Minutes
        /// </summary>
        public int BaseType; 
        /// <summary>
        /// Stock symbol
        /// </summary>
        public string Symbol;
        public float OpenPrice;
        public float HighPrice;
        public float LowPrice;
        public float ClosePrice;
        /// <summary>
        /// Financial Volume (Volume Financeiro)
        /// </summary>
        public double VolumeF;
        /// <summary>
        /// Stock Volume (Volume Quantidade)
        /// </summary>
        public long VolumeS;
        /// <summary>
        /// Trade Volume (Volume de Negócios)
        /// </summary>
        public long VolumeT;
        /// <summary>
        /// Adjustments for splits or reverse-splits
        /// </summary>
        public float AdjustS;
        /// <summary>
        /// Adjustments for dividends, distributions and interests rates
        /// </summary>
        public float AdjustD;

        public BarData()
        {
            Symbol = string.Empty;
            TradeDate = DateTime.MinValue;
        }

        public BarData(BarDataDisk bar)
        {
            Symbol = bar.Symbol;
            BaseType = bar.BaseType;
            TradeDate = bar.TradeDate;
            TradeDateTicks = TradeDate.Ticks;
            OpenPrice = bar.OpenPrice;
            HighPrice = bar.HighPrice;
            LowPrice = bar.LowPrice;
            ClosePrice = bar.ClosePrice;
            VolumeF = bar.VolumeF;
            VolumeS = bar.VolumeS;
            VolumeT = bar.VolumeT;
            AdjustD = bar.AdjustD;
            AdjustS = bar.AdjustS;
        }

        public BarData(string symbol, int baseType, DateTime tradeDate, float openPrice, float highPrice, float lowPrice, float closePrice, double volumeF, long volumeS, long volumeT, float adjustD, float adjustS)
        {
            Symbol = symbol;
            BaseType = baseType;
            TradeDate = tradeDate;
            TradeDateTicks = TradeDate.Ticks;
            OpenPrice = openPrice;
            HighPrice = highPrice;
            LowPrice = lowPrice;
            ClosePrice = closePrice;
            VolumeF = volumeF;
            VolumeT = volumeT;
            VolumeS = volumeS;
            AdjustS = adjustS;
            AdjustD = adjustD;
        }

        public bool Equals(BarData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
            Symbol == other.Symbol &&
            BaseType == other.BaseType &&
            TradeDate == other.TradeDate &&
            TradeDateTicks == other.TradeDateTicks &&
            OpenPrice == other.OpenPrice &&
            HighPrice == other.HighPrice &&
            LowPrice == other.LowPrice &&
            ClosePrice == other.ClosePrice &&
            VolumeF == other.VolumeF &&
            VolumeT == other.VolumeT &&
            VolumeS == other.VolumeS &&
            AdjustS == other.AdjustS &&
            AdjustD == other.AdjustD;
        }

        public bool Equals(BarDataDisk other)
        {
            if (ReferenceEquals(null, other)) return false;
            return
            Symbol == other.Symbol &&
            BaseType == other.BaseType &&
            TradeDate == other.TradeDate &&
            TradeDateTicks == other.TradeDateTicks &&
            OpenPrice == other.OpenPrice &&
            HighPrice == other.HighPrice &&
            LowPrice == other.LowPrice &&
            ClosePrice == other.ClosePrice &&
            VolumeF == other.VolumeF &&
            VolumeT == other.VolumeT &&
            VolumeS == other.VolumeS &&
            AdjustS == other.AdjustS &&
            AdjustD == other.AdjustD;
        }

    }


    //[Index("byDate", Keys = new string[] { "TradeDateTicks" })]
    //[Index("bySymbol", Keys = new string[] { "Symbol" })]
    //[Index("byBaseSymbol", Keys = new string[] { "BaseType", "Symbol" })]
    //[Index("byDateSymbol", Keys = new string[] { "TradeDateTicks", "Symbol" }, Type = Database.IndexType.BTree)]
    //[Index("byBaseSymbolDate", Keys = new string[] { "BaseType", "Symbol", "TradeDateTicks" }, Unique = true, Type = Database.IndexType.BTree)]
    ////[Index("byDateSymbolAdjustment", Keys = new string[] { "TradeDateTicks", "Symbol", "Adjustment" }, Unique = true, Type = Database.IndexType.BTree)]
    //[Persistent(Disk = true)]
    public class BarDataDisk
    {
        //[Indexable(Unique = true)] // create unique (tree) eXtremeDB index by "DateTimeTick" field
        /// <summary>
        /// Date-time of trade represented by Ticks
        /// </summary>
        public long TradeDateTicks;
        /// <summary>
        /// Date-time of trade
        /// </summary>
        public DateTime TradeDate;
        /// <summary>
        /// Type of base 0 = Daily , 1 = Minute , 2 = 15Minutes
        /// </summary>
        public int BaseType;
        /// <summary>
        /// Stock symbol
        /// </summary>
        public string Symbol;
        public float OpenPrice;
        public float HighPrice;
        public float LowPrice;
        public float ClosePrice;
        /// <summary>
        /// Financial Volume (Volume Financeiro)
        /// </summary>
        public double VolumeF;
        /// <summary>
        /// Stock Volume (Volume Quantidade)
        /// </summary>
        public long VolumeS;
        /// <summary>
        /// Trade Volume (Volume de Negócios)
        /// </summary>
        public long VolumeT;
        /// <summary>
        /// Adjustments for splits or reverse-splits
        /// </summary>
        public float AdjustS;
        /// <summary>
        /// Adjustments for dividends, distributions and interests rates
        /// </summary>
        public float AdjustD;

        public BarDataDisk()
        {
            Symbol = string.Empty;
            TradeDate = DateTime.MinValue;
        }

        public BarDataDisk(string symbol, int baseType, DateTime tradeDate, float openPrice, float highPrice, float lowPrice, float closePrice, double volumeF, long volumeS, long volumeT, float adjustS, float adjustD)
        {
            Symbol = symbol;
            BaseType = baseType;
            TradeDate = tradeDate;
            TradeDateTicks = TradeDate.Ticks;
            OpenPrice = openPrice;
            HighPrice = highPrice;
            LowPrice = lowPrice;
            ClosePrice = closePrice;
            VolumeF = volumeF;
            VolumeS = volumeS;
            VolumeT = volumeT;
            AdjustD = adjustD;
            AdjustS = adjustS;
        }

        public bool Equals(BarData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
            Symbol == other.Symbol &&
            BaseType == other.BaseType &&
            TradeDate == other.TradeDate &&
            TradeDateTicks == other.TradeDateTicks &&
            OpenPrice == other.OpenPrice &&
            HighPrice == other.HighPrice &&
            LowPrice == other.LowPrice &&
            ClosePrice == other.ClosePrice &&
            VolumeF == other.VolumeF &&
            VolumeT == other.VolumeT &&
            VolumeS == other.VolumeS &&
            AdjustS == other.AdjustS &&
            AdjustD == other.AdjustD;
        }

        public bool Equals(BarDataDisk other)
        {
            if (ReferenceEquals(null, other)) return false;
            return
            Symbol == other.Symbol &&
            BaseType == other.BaseType &&
            TradeDate == other.TradeDate &&
            TradeDateTicks == other.TradeDateTicks &&
            OpenPrice == other.OpenPrice &&
            HighPrice == other.HighPrice &&
            LowPrice == other.LowPrice &&
            ClosePrice == other.ClosePrice &&
            VolumeF == other.VolumeF &&
            VolumeT == other.VolumeT &&
            VolumeS == other.VolumeS &&
            AdjustS == other.AdjustS &&
            AdjustD == other.AdjustD;
        }
        
    }

}
