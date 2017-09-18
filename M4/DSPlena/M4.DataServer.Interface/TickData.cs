using System;

namespace M4.DataServer.Interface
{
    public class TickEventArgs : EventArgs
    {
        public TickData Tick { get; set; }
        public TickEventArgs(TickData tick)
        {
            Tick = tick;
        }
    }
    public partial class TickData
    {
        private int _id;
        private string _symbol;
        private DateTime _tradeDate;
        private float _price;
        private int _quantity;
        private int _buyer;
        private int _seller;


        partial void SymbolChanged();
        partial void TradeDateChanged();
        partial void LastPriceChanged();
        partial void QuantityChanged();
        partial void BuyerChanged();
        partial void SellerChanged();
        partial void IdChanged();


        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;

                _id = value;
                IdChanged();
            }
        }

        public string Symbol
        {
            get { return _symbol; }
            set
            {
                if (_symbol == value) return;

                _symbol = value;
                SymbolChanged();
            }
        }

        public DateTime TradeDate
        {
            get { return _tradeDate; }
            set
            {
                if (_tradeDate == value) return;
                _tradeDate = value;
                TradeDateChanged();
            }
        }

        public float Price
        {
            get { return _price; }
            set
            {
                if (_price == value) return;

                _price = value;
                LastPriceChanged();
            }
        }
        
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value) return;

                _quantity = value;
                QuantityChanged();
            }
        }

        public int Buyer
        {
            get { return _buyer; }
            set
            {
                if (_buyer == value) return;

                _buyer = value;
                BuyerChanged();
            }
        }

        public int Seller
        {
            get { return _seller; }
            set
            {
                if (_seller == value) return;

                _seller = value;
                SellerChanged();
            }
        }
    }
}
