using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelSamples
{
    public class PartitionerSample : ISample
    {
        public void Run()
        {
            const int n = 100000000;

            int[] array = new int[n];

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = i;
            }
            stopwatch.Stop();

            TimeSpan serialTime = stopwatch.Elapsed;

            stopwatch.Restart();
            Parallel.For(0, n, i => array[i] = i);
            stopwatch.Stop();

            TimeSpan defaultParallelTime = stopwatch.Elapsed;

            stopwatch.Restart();
            Parallel.ForEach(Partitioner.Create(0, n, 1000), r =>
                {
                    for (int i = r.Item1; i < r.Item2; ++i)
                    {
                        array[i] = i;
                    }
                });
            stopwatch.Stop();

            TimeSpan withPartitionerTime = stopwatch.Elapsed;

            Console.WriteLine("Serial time: {0}.", serialTime);
            Console.WriteLine("Default parallel time: {0}.", defaultParallelTime);
            Console.WriteLine("With partitioner time: {0}.", withPartitionerTime);
        }
    }
}
