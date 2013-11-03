using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class InterlockedIncrementSample : ISample
    {
        const int n = 100000000;

        int value;

        void NaiveIncrement()
        {
            for (int i = 1; i <= n; ++i)
            {
                ++value;
            }
        }

        void AtomicIncrement()
        {
            for (int i = 1; i <= n; ++i)
            {
                Interlocked.Increment(ref value);
            }
        }

        public void Run()
        {
            Thread naiveIncrement = new Thread(NaiveIncrement) { IsBackground = true };
            naiveIncrement.Start();

            for (int i = 1; i <= n; ++i)
            {
                ++value;
            }

            naiveIncrement.Join();

            Console.WriteLine(value);

            value = 0;

            Thread atomicIncrement = new Thread(AtomicIncrement) { IsBackground = true };
            atomicIncrement.Start();

            for (int i = 1; i <= n; ++i)
            {
                Interlocked.Increment(ref value);
            }

            atomicIncrement.Join();

            Console.WriteLine(value);

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; i <= n; ++i)
            {
                ++value;
            }
            stopwatch.Stop();

            TimeSpan naiveIncrementTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                Interlocked.Increment(ref value);
            }
            stopwatch.Stop();

            TimeSpan atomicIncrementTime = stopwatch.Elapsed;

            Console.WriteLine(naiveIncrementTime);
            Console.WriteLine(atomicIncrementTime);
        }
    }
}
