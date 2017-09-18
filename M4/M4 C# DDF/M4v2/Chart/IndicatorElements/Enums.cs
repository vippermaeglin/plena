
namespace M4.M4v2.Chart.IndicatorElements
{
    public class Enums
    {
        public enum LineStyle { Normal, Tracejados, Pontilhados }

        public enum Window { NewWindow, PanelMain, Panel1 }

        public enum Source { Abertura, Fechamento, Máximo, Mínimo, Volume }
        public enum Source2 { Open, Close, High, Low, Volume }

        public enum SourceOHLC { Abertura, Fechamento, Máximo, Mínimo }
        public enum SourceOHLC2 { Open, Close, High, Low }

        public enum SourceVolume { Volume } //Add other Volume types someday

        public enum Type { Simples, Exponencial, WellesWilder, TimeSeries, Triangular, Variável, VIDYA, Ponderada }

        public enum TypeHeikin { Simples, Exponencial, WellesWilder /*, TimeSeries, Triangular, Variável, VIDYA*/,Ponderada=7 }

        public enum PointsPercent { Point=1, Percent }

    }
}
