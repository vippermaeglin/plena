using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AxSTOCKCHARTXLib;
using STOCKCHARTXLib;

namespace M4.M4v2.Settings
{
    public class Scheme
    {
        private static Scheme _scheme;

        public List<KeyValuePair<string, string>> Schemes { get; set; }

        public static Scheme Instance()
        {
            return _scheme ?? new Scheme();
        }

        public Scheme()
        {
            LoadScheme();
        }

        private void LoadScheme()
        {
            Schemes = new List<KeyValuePair<string, string>>
                          {
                              new KeyValuePair<string, string>("SchemeSky",
                                                               Program.LanguageDefault.DictionarySettings["SchemeSky"]),
                              new KeyValuePair<string, string>("SchemeWhite",
                                                               Program.LanguageDefault.DictionarySettings["SchemeWhite"]),
                              new KeyValuePair<string, string>("SchemeBlue",
                                                               Program.LanguageDefault.DictionarySettings["SchemeBlue"]),
                              new KeyValuePair<string, string>("SchemeBeige",
                                                               Program.LanguageDefault.DictionarySettings["SchemeBeige"]),
                              new KeyValuePair<string, string>("SchemeDark",
                                                               Program.LanguageDefault.DictionarySettings["SchemeDark"]),
                              new KeyValuePair<string, string>("SchemeGreen",
                                                               Program.LanguageDefault.DictionarySettings["SchemeGreen"]),
                                new KeyValuePair<string, string>("SchemeMono",
                                                               Program.LanguageDefault.DictionarySettings["SchemeMono"]),
                          };

            Schemes = Schemes.OrderBy(r => r.Value).ToList();
        }

        public void UpdateChartColors(AxStockChartX stockChartX1, string style)
        {
            stockChartX1.set_SeriesColor(stockChartX1.Symbol + ".close", ColorTranslator.ToOle(Color.Black));
            stockChartX1.ValuePanelColor = Color.FromArgb(204, 227, 242);
            switch (style)
            {

                case "SchemeSky":
                    stockChartX1.BackGradientTop = Color.White;
                    stockChartX1.BackGradientBottom = Color.FromArgb(0xd5, 0xe7, 0xff);
                    stockChartX1.ChartBackColor = Color.FromArgb(0xd5, 0xe7, 0xff);
                    stockChartX1.Gridcolor = Color.SkyBlue;
                    stockChartX1.ChartForeColor = Color.Black;
                    stockChartX1.UpColor = Color.FromArgb(0, 122, 204);
                    stockChartX1.DownColor = Color.FromArgb(255, 0, 0);
                    stockChartX1.HorizontalSeparatorColor = Color.SkyBlue;

                    stockChartX1.CandleUpOutlineColor = Color.Black;
                    stockChartX1.CandleDownOutlineColor = Color.Black;
                    stockChartX1.WickUpColor = Color.Black;
                    stockChartX1.WickDownColor = Color.Black;


                    stockChartX1.CandleUpOutlineColor = Color.FromArgb(0, 0, 255);
                    stockChartX1.CandleDownOutlineColor = Color.FromArgb(180, 0, 0);
                    stockChartX1.WickUpColor = Color.FromArgb(0, 0, 255);
                    stockChartX1.WickDownColor = Color.FromArgb(180, 0, 0);

                    break;
                case "SchemeWhite":
                    stockChartX1.BackGradientTop = Color.White;
                    stockChartX1.BackGradientBottom = Color.White;
                    stockChartX1.ChartBackColor = Color.White;
                    stockChartX1.Gridcolor = Color.FromArgb(210, 210, 210);
                    stockChartX1.ChartForeColor = Color.FromArgb(0, 0, 0);
                    stockChartX1.UpColor = Color.FromArgb(33, 77, 207);
                    stockChartX1.DownColor = Color.FromArgb(255, 0, 0);
                    stockChartX1.HorizontalSeparatorColor = Color.FromArgb(128, 128, 128);

                    stockChartX1.CandleUpOutlineColor = Color.FromArgb(0, 0, 255);
                    stockChartX1.CandleDownOutlineColor = Color.FromArgb(180, 0, 0);
                    stockChartX1.WickUpColor = Color.FromArgb(0, 0, 255);
                    stockChartX1.WickDownColor = Color.FromArgb(180, 0, 0);


                    
                    break;
                case "SchemeBlue":
                    stockChartX1.BackGradientTop = Color.FromArgb(0, 33, 66);
                    stockChartX1.BackGradientBottom = Color.FromArgb(0, 66, 176);
                    stockChartX1.ChartBackColor = Color.Black;
                    stockChartX1.Gridcolor = Color.FromArgb(102, 102, 102);
                    stockChartX1.ChartForeColor = Color.White;
                    stockChartX1.UpColor = Color.FromArgb(0, 255, 0);
                    stockChartX1.DownColor = Color.FromArgb(255, 0, 0);
                    stockChartX1.HorizontalSeparatorColor = Color.FromArgb(160, 160, 160);

                    stockChartX1.CandleUpOutlineColor = Color.White;
                    stockChartX1.CandleDownOutlineColor = Color.White;
                    stockChartX1.WickUpColor = Color.White;
                    stockChartX1.WickDownColor = Color.White;
                    break;
                case "SchemeBeige":
                    stockChartX1.BackGradientTop = Color.FromArgb(238, 239, 222);
                    stockChartX1.BackGradientBottom = Color.FromArgb(250, 251, 239);
                    stockChartX1.ChartBackColor = Color.FromArgb(245, 245, 245);
                    stockChartX1.Gridcolor = Color.FromArgb(210, 210, 210);
                    stockChartX1.ChartForeColor = Color.FromArgb(0, 0, 0);
                    stockChartX1.UpColor = Color.FromArgb(0, 192, 0);
                    stockChartX1.DownColor = Color.FromArgb(192, 0, 0);
                    stockChartX1.HorizontalSeparatorColor = Color.FromArgb(64, 64, 64);
                    stockChartX1.set_SeriesColor(stockChartX1.Symbol + ".close", Color.FromArgb(0xFF, 0xff, 0xff, 0xff).ToArgb());

                    stockChartX1.CandleUpOutlineColor = Color.FromArgb(0, 0, 0);
                    stockChartX1.CandleDownOutlineColor = Color.FromArgb(0, 0, 0);
                    stockChartX1.WickUpColor = Color.FromArgb(0, 0, 0);
                    stockChartX1.WickDownColor = Color.FromArgb(0, 0, 0);
                    break;
                case "SchemeDark":
                    stockChartX1.BackGradientTop = Color.Black;
                    stockChartX1.BackGradientBottom = Color.Black;
                    stockChartX1.ChartBackColor = Color.Black;
                    stockChartX1.Gridcolor = Color.FromArgb(92, 92, 92);
                    stockChartX1.ChartForeColor = Color.FromArgb(160, 160, 160);
                    stockChartX1.UpColor = Color.FromArgb(0, 255, 0);
                    stockChartX1.DownColor = Color.FromArgb(255, 0, 0);
                    stockChartX1.HorizontalSeparatorColor = Color.FromArgb(128, 128, 128);

                    stockChartX1.CandleUpOutlineColor = Color.FromArgb(0, 255, 0);
                    stockChartX1.CandleDownOutlineColor = Color.FromArgb(255, 0, 0);
                    stockChartX1.WickUpColor = Color.FromArgb(0, 255, 0);
                    stockChartX1.WickDownColor = Color.FromArgb(255, 0, 0);
                    break;
                case "SchemeGreen":
                    stockChartX1.BackGradientTop = Color.FromArgb(0, 66, 33);


                    stockChartX1.BackGradientBottom = Color.FromArgb(0, 176, 66);
                    stockChartX1.ChartBackColor = Color.Black;
                    stockChartX1.Gridcolor = Color.FromArgb(92, 92, 92);
                    stockChartX1.ChartForeColor = Color.White;
                    stockChartX1.UpColor = Color.Lime;
                    stockChartX1.DownColor = Color.Tomato;
                    stockChartX1.HorizontalSeparatorColor = Color.White;

                    stockChartX1.CandleUpOutlineColor = Color.White;
                    stockChartX1.CandleDownOutlineColor = Color.White;
                    stockChartX1.WickUpColor = Color.White;
                    stockChartX1.WickDownColor = Color.White;
                    break;
                case "SchemeMono":
                    stockChartX1.BackGradientTop = Color.White;

                    stockChartX1.BackGradientBottom = Color.White;
                    stockChartX1.ChartBackColor = Color.White;
                    stockChartX1.Gridcolor = Color.FromArgb(210, 210, 210);
                    stockChartX1.ChartForeColor = Color.FromArgb(0, 0, 0);
                    if (stockChartX1.get_SeriesType(stockChartX1.Symbol + ".close") == SeriesType.stStockBarChart) stockChartX1.UpColor = Color.FromArgb(160, 160, 160);
                    else stockChartX1.UpColor = Color.White;
                    stockChartX1.DownColor = Color.Black;
                    stockChartX1.HorizontalSeparatorColor = Color.FromArgb(128, 128, 128);

                    stockChartX1.CandleUpOutlineColor = Color.Black;
                    stockChartX1.CandleDownOutlineColor = Color.Black;
                    stockChartX1.WickUpColor = Color.Black;
                    stockChartX1.WickDownColor = Color.Black;
                    break;
            }

            stockChartX1.Update();
        }
    }
}
