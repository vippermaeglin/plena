using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using M4.M4v2.Chart.IndicatorElements;

namespace M4.M4v2.Chart.Mock
{
    public class Mock
    {
        public List<Indicator> Indicators { get; set; }

        public Mock()
        {
            LoadIndicator();
        }

        public void LoadIndicator()
        {
            Indicators = new List<Indicator>
                             {
                                 new Indicator
                                     {
                                         Code = "HILO",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["HILO Activator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "MACD-H",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["MACD Histogram"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "MACD",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["MACD"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "MAE",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Moving Average Envelope"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "VOL",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Volume"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "BB",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Bollinger Bands"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "HV",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Historical Volatility"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "RSI",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Relative Strength Index"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "MA",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Moving Average"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "TP",
                                 //        Description =
                                 //            Program.LanguageDefault.DictionarySelectIndicator["Typical Price"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "PSAR",
                                         Description =
                                             Program.LanguageDefault.DictionarySelectIndicator["Parabolic SAR"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "SMI",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Stochastic Momentum Index"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },                                 
                                 new Indicator
                                     {
                                         Code = "SO",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Stochastic Oscillator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "Aroon",
                                         Description = "Aroon",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "AO",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Aroon Oscillator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "CMF",
                                         Description = "CMF Chaikin Money Flow",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                //new Indicator
                                //     {
                                //         Code = "EOM",
                                //         Description = "EOM Ease Of Movement",
                                //         IndicatorsMocks = new List<IndicatorMock>(),
                                //         Window = Enums.Window.NewWindow,
                                //     },                                 
                                //new Indicator
                                //     {
                                //         Code = "MFI",
                                //         Description = Program.LanguageDefault.DictionarySelectIndicator["Money Flow Index"],
                                //         IndicatorsMocks = new List<IndicatorMock>(),
                                //         Window = Enums.Window.NewWindow,
                                //     },
                                 //new Indicator
                                 //    {
                                 //        Code = "ASI",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Accumulative Swing Index"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "CV",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Chaikin Volatility"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "CMO",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Chande Momentum Oscillator"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 //new Indicator
                                 //    {
                                 //        Code = "WWS",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Welles Wilder Smoothing"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 //new Indicator
                                 //    {
                                 //        Code = "VHF",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Vertical Horizontal Filter"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "TRIX",
                                         Description = "TRIX",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "LRRS",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression R-Squared"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 //new Indicator
                                 //    {
                                 //        Code = "LRF",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Forecast"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },                                 
                                 //new Indicator
                                 //    {
                                 //        Code = "LRS",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Slope"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 //new Indicator
                                 //    {
                                 //        Code = "LRI",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Linear Regression Intercept"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "MO",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Momentum Oscillator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "ADX",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["ADX"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                new Indicator
                                     {
                                         Code = "DI+/DI-",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["DI+/DI-"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                new Indicator
                                     {
                                         Code = "DI+/DI-/ADX",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["DI+/DI-/ADX"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "CCI",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Commodity Channel Index"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "DPO",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Detrended Price Oscillator"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },                                 
                                 //new Indicator
                                 //    {
                                 //        Code = "MI",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Mass Index"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "OBV",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["On Balance Volume"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "PROC",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Price ROC"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "PVT",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Price Volume Trend"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "Williams",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Williams %R"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "WAD",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Williams Accumulation Distribution"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "WC",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Wheighted Close"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "VROC",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Volume ROC"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "FCO",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Fractal Chaos Oscillator"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 //new Indicator
                                 //    {
                                 //        Code = "FCB",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Fractal Chaos Bands"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "UO",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Ultimate Oscillator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "TR",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["True Range"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 //new Indicator
                                 //    {
                                 //        Code = "RO",
                                 //        Description = Program.LanguageDefault.DictionarySelectIndicator["Rainbow Oscillator"],
                                 //        IndicatorsMocks = new List<IndicatorMock>(),
                                 //        Window = Enums.Window.NewWindow,
                                 //    },
                                 new Indicator
                                     {
                                         Code = "A/D",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Accumulation/Distribution"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     }
                                 
/*,
                                 new Indicator
                                     {
                                         Code = "SMA",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Simple Moving Average"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },                                 
                                 new Indicator
                                     {
                                         Code = "EMA",
                                         Description = "Exponential Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "TSMA",
                                         Description = "Time Series Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "VO",
                                         Description = Program.LanguageDefault.DictionarySelectIndicator["Volume Oscillator"],
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "TMA",
                                         Description = "Triangular Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "VMA",
                                         Description = "Variable Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "VIDYAMA",
                                         Description = "VIDYA Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "WMA",
                                         Description = "Weighted Moving Average",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 
                                 new Indicator
                                     {
                                         Code = "CCI",
                                         Description = "Commodity Channel Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "HLB",
                                         Description = "High/Low Bands",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "PI",
                                         Description = "Performance Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "MP",
                                         Description = "Median Price",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "HML",
                                         Description = "High Minus Low",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "SI",
                                         Description = "Swing Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "PNB",
                                         Description = "Price Number Band",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "PVI",
                                         Description = "Positive Volume Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "NVI",
                                         Description = "Negative Volume Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },

                                 new Indicator
                                     {
                                         Code = "TVI",
                                         Description = "Trade Volume Index",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "SD",
                                         Description = "Standard Deviation",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "CRSI",
                                         Description = "Comparative RSI",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "PO",
                                         Description = "Price Oscillator",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },
                                 new Indicator
                                     {
                                         Code = "CV",
                                         Description = "Chaikin Volatility",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     },

                                 new Indicator
                                     {
                                         Code = "HV",
                                         Description = "Historical Volatility",
                                         IndicatorsMocks = new List<IndicatorMock>(),
                                         Window = Enums.Window.NewWindow,
                                     }*/
                             };
        }

        public IndicatorMock GetIndicatorMockByCodeMock(string code, string codeMock)
        {
            return Indicators.Where(r => r.Code.Equals(code)).FirstOrDefault().
                    IndicatorsMocks.Where(r => r.CodeMock.Equals(codeMock)).FirstOrDefault();
        }

        public Indicator GetIndicatorByCode(string code)
        {
            return Indicators.Where(r => r.Code.Equals(code)).FirstOrDefault();
        }

        public void RemoveIndicatorMock(IndicatorMock indicatorMock)
        {
            Indicators.Where(r => r.Code.Equals(indicatorMock.Code)).FirstOrDefault().IndicatorsMocks.Remove(indicatorMock);
        }

        public void ClearIndicatorsMock()
        {
            foreach (var indicator in Indicators)
            {
                indicator.IndicatorsMocks.Clear();
            }
        }

        public void AddIndicatorMock(string code, IndicatorMock indicatorMock)
        {
            Indicators.Where(r => r.Code.Equals(code)).FirstOrDefault().IndicatorsMocks.Add(indicatorMock);
        }

        
    }
}