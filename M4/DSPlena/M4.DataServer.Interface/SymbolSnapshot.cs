using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M4.DataServer.Interface
{
    public class SymbolSnapshot
    {
        #region Tick

        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Quantity { get; set; }
        public int Buyer { get; set; }
        public int Seller { get; set; }
        #endregion

        #region Status
        public double BidPrice { get; set; }
        public long BidSize { get; set; }
        public double AskPrice { get; set; }
        public long AskSize { get; set; }
        public double VolumeFinancial { get; set; } //Volume Financeiro: Qtde de Ações x Preço (acumulado)
        public long VolumeTrades { get; set; }      //Volume de Negócios: Qtde de Negócios (acumulado)
        public long VolumeStocks { get; set; }      //Volume de Ações: Qtde de Ações (acumulado)
        #endregion
    }
}
