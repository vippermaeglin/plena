
using M4Core.Entities;
namespace M4.M4v2.GridviewRowDetailsExtended
{
    public class DataAssetsInfo
    {
        public string Symbol { get; set; }

        public string Last { get; set; }

        public string Variation { get; set; }

        public string Volume { get; set; }

        public string Time { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Close { get; set; }

        public string Open { get; set; }

        public string Trades { get; set; }
        public DataAssetsInfo()
        {
            
        }
        public DataAssetsInfo(Assets asset)
        {
            Symbol = asset.Symbol;
            Last = asset.Last.ToString();
            Variation = asset.Variation.ToString();
            Volume = asset.Volume.ToString();
            Time = asset.Time.ToString();
            High = asset.High.ToString();
            Low = asset.Low.ToString();
            Close = asset.Close.ToString();
            Open = asset.Open.ToString();
            Trades = asset.Trades.ToString();
        }
    }
}