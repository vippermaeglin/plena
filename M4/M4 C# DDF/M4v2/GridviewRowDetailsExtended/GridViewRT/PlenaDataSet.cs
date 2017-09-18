using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace M4.M4v2.GridviewRowDetailsExtended.GridViewRT
{
    public class OrderDataSet
    {
        public List<Order> Orders { get; set; }
        public OrderDataSet()
        {
            Orders = new List<Order>();
        }
        public void Add(string symbol, int value)
        {
            Orders.Add(new Order() { Symbol = symbol, Value = value });
        }
    }

    public class Order
    {
        public string Symbol { get; set; }
        public int Value;

    }

}
