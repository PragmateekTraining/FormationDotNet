using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClrSamples
{
    public class FinalizersSamples : ISample
    {
        class WithoutFinalizer
        {
        }

        class WithEmptyFinalizer
        {
            ~WithEmptyFinalizer()
            {
            }
        }

        public void Run()
        {
            const int N = 10000000;

            IList<WithoutFinalizer> withoutFinalizers = new List<WithoutFinalizer>();

            for (int i = 0; i < N; ++i)
            {
                withoutFinalizers.Add(new WithoutFinalizer());
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            withoutFinalizers = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            IList<WithEmptyFinalizer> withFinalizers = new List<WithEmptyFinalizer>();

            for (int i = 0; i < N; ++i)
            {
                withFinalizers.Add(new WithEmptyFinalizer());
            }

            stopwatch.Restart();

            withFinalizers = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            double ratio = 1.0 * t2.Ticks / t1.Ticks;

            Console.WriteLine("Without finalizers: {0}", t1);
            Console.WriteLine("With finalizers: {0} (x{1:N1})", t2, ratio);
        }
    }
}
