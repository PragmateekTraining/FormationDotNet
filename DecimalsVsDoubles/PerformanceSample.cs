using System;
using SamplesAPI;
using System.Diagnostics;

namespace DecimalsVsDoubles
{
    public class PerformanceSample : ISample
    {
        public void Run()
        {
            const int N = 100000000;

            decimal sumDecimal = 0;
            double sumDouble = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < N; ++i)
            {
                ++sumDecimal;
            }
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < N; ++i)
            {
                ++sumDouble;
            }
            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            Console.WriteLine("With double: {0:N2}ms", t2.TotalMilliseconds);
            Console.WriteLine("With decimal: {0:N2}ms (x{1})", t1.TotalMilliseconds, t1.Ticks / t2.Ticks);
        }
    }
}
