using System;
using System.Diagnostics;

namespace LazySamples
{
    class ThreadSafetyCostSample
    {
        public void Run()
        {
            const int n = 10000000;

            Lazy<object>[] notThreadSafe = new Lazy<object>[n];
            Lazy<object>[] threadSafe = new Lazy<object>[n];

            for (int i = 0; i < n; ++i)
            {
                notThreadSafe[i] = new Lazy<object>(() => new object(), false);
                threadSafe[i] = new Lazy<object>(() => new object(), true);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            object o;

            for (int i = 0; i < n; ++i)
            {
                o = notThreadSafe[i].Value;
            }

            stopwatch.Stop();

            TimeSpan notThreadSafeTime = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                o = threadSafe[i].Value;
            }

            stopwatch.Stop();

            TimeSpan threadSafeTime = stopwatch.Elapsed;

            double ratio = 1.0 * threadSafeTime.Ticks / notThreadSafeTime.Ticks;

            Console.WriteLine("Not thread-safe: {0}", notThreadSafeTime);
            Console.WriteLine("Thread-safe: {0} (x{1:N2})", threadSafeTime, ratio);
        }
    }
}
