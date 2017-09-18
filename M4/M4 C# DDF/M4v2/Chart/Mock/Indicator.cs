using System.Collections.Generic;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.Mock
{
    public class Indicator
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public Enums.Window Window { get; set; }

        public List<IndicatorMock> IndicatorsMocks { get; set; }
    }
}
