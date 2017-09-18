using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M4.DataServer.Interface;

namespace M4.M4v2.Chart.Averages
{
    class MovingAverages
    {
        
        public const int NULL_VALUE = -987654321;

        public static MovingAverages Instance;

        public static MovingAverages I()
        {
            return Instance ?? new MovingAverages();
        }


        public List<BarData> SimpleAverage(List<BarData> Source, int Periods)
        {
            List<BarData> Result = new List<BarData>();

            //Fill first n records with NULL_VALUE:
            /*for (int i = 0; i < Periods-1; i++)
            {
                Result.Add(new BarData()
                    {
                        AdjustD = Source[i].AdjustD,
                        AdjustS = Source[i].AdjustS,
                        BaseType = Source[i].BaseType,
                        ClosePrice = NULL_VALUE,
                        HighPrice = NULL_VALUE,
                        LowPrice = NULL_VALUE,
                        OpenPrice = NULL_VALUE,
                        Symbol = Source[i].Symbol,
                        TradeDate = Source[i].TradeDate,
                        TradeDateTicks = Source[i].TradeDateTicks,
                        VolumeF = Source[i].VolumeF,
                        VolumeS = Source[i].VolumeS,
                        VolumeT = Source[i].VolumeT
                    });
            }*/

            //Loop through each record in recordset
            for (int Record = Periods - 1; Record < Source.Count; Record++)
            {

                BarData Avg = new BarData()
                {
                    AdjustD = Source[Record].AdjustD,
                    AdjustS = Source[Record].AdjustS,
                    BaseType = Source[Record].BaseType,
                    ClosePrice = 0,
                    HighPrice = 0,
                    LowPrice = 0,
                    OpenPrice = 0,
                    Symbol = Source[Record].Symbol,
                    TradeDate = Source[Record].TradeDate,
                    TradeDateTicks = Source[Record].TradeDateTicks,
                    VolumeF = Source[Record].VolumeF,
                    VolumeS = Source[Record].VolumeS,
                    VolumeT = Source[Record].VolumeT
                };

                for (int Period = Record; Period > Record - Periods; Period--)
                {
                    Avg.OpenPrice += Source[Period].OpenPrice;
                    Avg.HighPrice += Source[Period].HighPrice;
                    Avg.LowPrice += Source[Period].LowPrice;
                    Avg.ClosePrice += Source[Period].ClosePrice;
                }

                //Calculate average
                Avg.OpenPrice /= (float)Periods;
                Avg.HighPrice /= (float)Periods;
                Avg.LowPrice /= (float)Periods;
                Avg.ClosePrice /= (float)Periods;

                Result.Add(Avg);

            }

            return Result;

        }
        public List<BarData> ExponentialAverage(List<BarData> Source, int Periods)
        {
            List<BarData> Result = new List<BarData>();
            int Record = Periods - 1;
            double Exp = 2 / (double)(Periods + 1);
            
            //Fill first n records with NULL_VALUE:
            /*for (int i = 0; i < Periods-1; i++)
            {
                Result.Add(new BarData()
                {
                    AdjustD = Source[i].AdjustD,
                    AdjustS = Source[i].AdjustS,
                    BaseType = Source[i].BaseType,
                    ClosePrice = NULL_VALUE,
                    HighPrice = NULL_VALUE,
                    LowPrice = NULL_VALUE,
                    OpenPrice = NULL_VALUE,
                    Symbol = Source[i].Symbol,
                    TradeDate = Source[i].TradeDate,
                    TradeDateTicks = Source[i].TradeDateTicks,
                    VolumeF = Source[i].VolumeF,
                    VolumeS = Source[i].VolumeS,
                    VolumeT = Source[i].VolumeT
                });
            }*/

            //First bar is a Simple Average for first n records:
            BarData Avg = new BarData()
            {
                AdjustD = Source[Record].AdjustD,
                AdjustS = Source[Record].AdjustS,
                BaseType = Source[Record].BaseType,
                ClosePrice = 0,
                HighPrice = 0,
                LowPrice = 0,
                OpenPrice = 0,
                Symbol = Source[Record].Symbol,
                TradeDate = Source[Record].TradeDate,
                TradeDateTicks = Source[Record].TradeDateTicks,
                VolumeF = Source[Record].VolumeF,
                VolumeS = Source[Record].VolumeS,
                VolumeT = Source[Record].VolumeT
            };
            for (int Period = Record; Period > Record - Periods; Period--)
            {
                Avg.OpenPrice += Source[Period].OpenPrice;
                Avg.HighPrice += Source[Period].HighPrice;
                Avg.LowPrice += Source[Period].LowPrice;
                Avg.ClosePrice += Source[Period].ClosePrice;
            }
            //Calculate average
            Avg.OpenPrice /= (float)Periods;
            Avg.HighPrice /= (float)Periods;
            Avg.LowPrice /= (float)Periods;
            Avg.ClosePrice /= (float)Periods;
            Result.Add(Avg);


            //Loop through each record in recordset
            for (Record = Periods; Record < Source.Count; Record++)
            {

                Avg = new BarData()
                {
                    AdjustD = Source[Record].AdjustD,
                    AdjustS = Source[Record].AdjustS,
                    BaseType = Source[Record].BaseType,
                    ClosePrice = 0,
                    HighPrice = 0,
                    LowPrice = 0,
                    OpenPrice = 0,
                    Symbol = Source[Record].Symbol,
                    TradeDate = Source[Record].TradeDate,
                    TradeDateTicks = Source[Record].TradeDateTicks,
                    VolumeF = Source[Record].VolumeF,
                    VolumeS = Source[Record].VolumeS,
                    VolumeT = Source[Record].VolumeT
                };

                //Calculate average
                Avg.OpenPrice = (float)(Result.Last().OpenPrice + (Source[Record].OpenPrice - Result.Last().OpenPrice) * Exp);
                Avg.HighPrice = (float)(Result.Last().HighPrice + (Source[Record].HighPrice - Result.Last().HighPrice) * Exp);
                Avg.LowPrice = (float)(Result.Last().LowPrice + (Source[Record].LowPrice - Result.Last().LowPrice) * Exp);
                Avg.ClosePrice = (float)(Result.Last().ClosePrice + (Source[Record].ClosePrice - Result.Last().ClosePrice) * Exp);

                Result.Add(Avg);

            }

            return Result;

        }
        public List<BarData> WellesWilderSmooth(List<BarData> Source, int Periods)
        {
            List<BarData> Result = new List<BarData>();
            int Record = Periods - 1;

            //Fill first n records with NULL_VALUE:
            /*for (int i = 0; i < Periods-1; i++)
            {
                Result.Add(new BarData()
                {
                    AdjustD = Source[i].AdjustD,
                    AdjustS = Source[i].AdjustS,
                    BaseType = Source[i].BaseType,
                    ClosePrice = NULL_VALUE,
                    HighPrice = NULL_VALUE,
                    LowPrice = NULL_VALUE,
                    OpenPrice = NULL_VALUE,
                    Symbol = Source[i].Symbol,
                    TradeDate = Source[i].TradeDate,
                    TradeDateTicks = Source[i].TradeDateTicks,
                    VolumeF = Source[i].VolumeF,
                    VolumeS = Source[i].VolumeS,
                    VolumeT = Source[i].VolumeT
                });
            }*/

            //First bar is a Simple Average for first n records:
            BarData Avg = new BarData()
            {
                AdjustD = Source[Record].AdjustD,
                AdjustS = Source[Record].AdjustS,
                BaseType = Source[Record].BaseType,
                ClosePrice = 0,
                HighPrice = 0,
                LowPrice = 0,
                OpenPrice = 0,
                Symbol = Source[Record].Symbol,
                TradeDate = Source[Record].TradeDate,
                TradeDateTicks = Source[Record].TradeDateTicks,
                VolumeF = Source[Record].VolumeF,
                VolumeS = Source[Record].VolumeS,
                VolumeT = Source[Record].VolumeT
            };
            for (int Period = Record; Period > Record - Periods; Period--)
            {
                Avg.OpenPrice += Source[Period].OpenPrice;
                Avg.HighPrice += Source[Period].HighPrice;
                Avg.LowPrice += Source[Period].LowPrice;
                Avg.ClosePrice += Source[Period].ClosePrice;
            }
            //Calculate average
            Avg.OpenPrice /= (float)Periods;
            Avg.HighPrice /= (float)Periods;
            Avg.LowPrice /= (float)Periods;
            Avg.ClosePrice /= (float)Periods;
            Result.Add(Avg);


            //Loop through each record in recordset
            for (Record = Periods; Record < Source.Count; Record++)
            {

                Avg = new BarData()
                {
                    AdjustD = Source[Record].AdjustD,
                    AdjustS = Source[Record].AdjustS,
                    BaseType = Source[Record].BaseType,
                    ClosePrice = 0,
                    HighPrice = 0,
                    LowPrice = 0,
                    OpenPrice = 0,
                    Symbol = Source[Record].Symbol,
                    TradeDate = Source[Record].TradeDate,
                    TradeDateTicks = Source[Record].TradeDateTicks,
                    VolumeF = Source[Record].VolumeF,
                    VolumeS = Source[Record].VolumeS,
                    VolumeT = Source[Record].VolumeT
                };

                //Calculate average
                Avg.OpenPrice = (float)(Result.Last().OpenPrice + 1 / (float)Periods * (Source[Record].OpenPrice - Result.Last().OpenPrice));
                Avg.HighPrice = (float)(Result.Last().HighPrice + 1/(float)Periods * (Source[Record].HighPrice - Result.Last().HighPrice));
                Avg.LowPrice = (float)(Result.Last().LowPrice + 1 / (float)Periods * (Source[Record].LowPrice - Result.Last().LowPrice));
                Avg.ClosePrice = (float)(Result.Last().ClosePrice + 1 / (float)Periods * (Source[Record].ClosePrice - Result.Last().ClosePrice));

                Result.Add(Avg);
            }

            return Result;

        }
        public List<BarData> WeightedAverage(List<BarData> Source, int Periods)
        {
            List<BarData> Result = new List<BarData>();

            //Fill first n records with NULL_VALUE:
            /*for (int i = 0; i < Periods-1; i++)
            {
                Result.Add(new BarData()
                    {
                        AdjustD = Source[i].AdjustD,
                        AdjustS = Source[i].AdjustS,
                        BaseType = Source[i].BaseType,
                        ClosePrice = NULL_VALUE,
                        HighPrice = NULL_VALUE,
                        LowPrice = NULL_VALUE,
                        OpenPrice = NULL_VALUE,
                        Symbol = Source[i].Symbol,
                        TradeDate = Source[i].TradeDate,
                        TradeDateTicks = Source[i].TradeDateTicks,
                        VolumeF = Source[i].VolumeF,
                        VolumeS = Source[i].VolumeS,
                        VolumeT = Source[i].VolumeT
                    });
            }*/

            float Weight = 0;

            for (int i = 1; i < Periods+1; i++)
            {
                Weight += i;
            }

            //Loop through each record in recordset
            for (int Record = Periods-1; Record < Source.Count; Record++)
            {
                BarData Avg = new BarData()
                {
                    AdjustD = Source[Record].AdjustD,
                    AdjustS = Source[Record].AdjustS,
                    BaseType = Source[Record].BaseType,
                    ClosePrice = 0,
                    HighPrice = 0,
                    LowPrice = 0,
                    OpenPrice = 0,
                    Symbol = Source[Record].Symbol,
                    TradeDate = Source[Record].TradeDate,
                    TradeDateTicks = Source[Record].TradeDateTicks,
                    VolumeF = Source[Record].VolumeF,
                    VolumeS = Source[Record].VolumeS,
                    VolumeT = Source[Record].VolumeT
                };

                int weight = Periods;
                for (int Period = Record; Period > Record - Periods; Period--)
                {
                    Avg.OpenPrice += weight * Source[Period].OpenPrice;
                    Avg.HighPrice += weight * Source[Period].HighPrice;
                    Avg.LowPrice += weight * Source[Period].LowPrice;
                    Avg.ClosePrice += weight * Source[Period].ClosePrice;
                    weight--;
                }

                Avg.OpenPrice /= Weight;
                Avg.HighPrice /= Weight;
                Avg.LowPrice /= Weight;
                Avg.ClosePrice /= Weight;


                Result.Add(Avg);

            }

            return Result;

        }
        


    }
}
