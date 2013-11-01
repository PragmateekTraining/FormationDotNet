using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class ThreadStaticSample : ISample
    {
        [ThreadStatic]
        static int threadStatic = 0;

        static ThreadLocal<int> threadLocal = new ThreadLocal<int>(() => 0);

        public void Run()
        {
            const int n = 10000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; i <= n; ++i)
            {
                ++threadStatic;
            }
            stopwatch.Stop();

            TimeSpan threadStaticTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                ++threadLocal.Value;
            }
            stopwatch.Stop();

            TimeSpan threadLocalTime = stopwatch.Elapsed;

            Console.WriteLine("ThreadStatic: {0}", threadStaticTime);
            Console.WriteLine("ThreadLocal: {0}", threadLocalTime);
        }
    }
}
