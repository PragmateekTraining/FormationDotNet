using System;
using System.Diagnostics;
using SamplesAPI;

namespace ValueTypesSamples
{
    public class BulkAllocationSample : ISample
    {
        class Reference
        {
        }

        struct Value
        {
        }

        public void Run()
        {
            const int N = 10000000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Reference[] references = new Reference[N];
            for (int i = 0; i < N; ++i)
            {
                references[i] = new Reference();
            }
            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            Value[] values = new Value[N];
            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            long ratio = t1.Ticks / t2.Ticks;

            Console.WriteLine("Allocating values: {0}", t2);
            Console.WriteLine("Allocating references: {0} (x{1})", t1, ratio);
        }
    }
}
