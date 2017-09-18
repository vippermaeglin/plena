using System.Diagnostics;

namespace M4Utils
{
    public class TimerLog
    {
        private static TimerLog _timerLog;

        private readonly Stopwatch _stopwatch;

        public TimerLog()
        {
            _stopwatch = new Stopwatch();
        }

        public static TimerLog Instance()
        {
            return _timerLog ?? (_timerLog = new TimerLog());
        }

        public void IniciaTempo()
        {
            _stopwatch.Start();
        }

        public void FinalizaTempo()
        {
            if (_stopwatch.IsRunning)
                _stopwatch.Stop();
        }

        public long TempoExecucao()
        {
            long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Reset();
            return elapsedMilliseconds;
        }

        public long GetTempoExecucao()
        {
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}