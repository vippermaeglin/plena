namespace M4Core.Entities
{
    public class Assets
    {
        #region Propriedades

        public string Symbol { get; set; }

        public decimal Last { get; set; }

        public string Time { get; set; }

        public decimal Variation { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Open { get; set; }

        public decimal Trades { get; set; }

        public double Volume { get; set; }

        public int Position { get; set; }

        #endregion
    }
}