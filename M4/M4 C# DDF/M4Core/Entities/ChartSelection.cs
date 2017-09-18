using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M4Core.Entities
{
    public class ChartSelection
    {
        public string Symbol;
        public Periodicity Periodicity;
        public int Interval;
        public int Bars;
        public string Source = "PLENA";
        public string PriceStyle;
    }
}
