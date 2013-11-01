using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class ThreadLocalSample : ISample
    {
        int n = 100000000;
        int nThreads = 4;
        const int max = 10;

        int[] data;
        int[] sharedFrequencies = new int[max + 1];
        int[] sharedAggregatedFrequencies = new int[max + 1];
        ThreadLocal<int[]> localFrequencies = new ThreadLocal<int[]>(() => new int[11]);

        Barrier barrier;

        void ComputeFrequenciesShared(object o)
        {
            int threadId = (int)o;

            barrier.SignalAndWait();

            for (int i = threadId * (n / nThreads); i < threadId * (n / nThreads) + n / nThreads; ++i)
            {
                lock (sharedFrequencies)
                {
                    ++sharedFrequencies[data[i]];
                }
            }
        }

        void ComputeFrequenciesLocal(object o)
        {
            int threadId = (int)o;

            barrier.SignalAndWait();

            for (int i = threadId * (n / nThreads); i < threadId * (n / nThreads) + n / nThreads; ++i)
            {
                ++localFrequencies.Value[data[i]];
            }

            lock (sharedAggregatedFrequencies)
            {
                for (int i = 0; i < sharedAggregatedFrequencies.Length; ++i)
                {
                    sharedAggregatedFrequencies[i] += localFrequencies.Value[i];
                }
            }
        }

        public void Run()
        {
            data = new int[n];

            Random rand = new Random();

            for (int i = 0; i < n; ++i)
            {
                data[i] = rand.Next(max + 1);
            }

            IList<Thread> threads = new List<Thread>(nThreads);

            barrier = new Barrier(nThreads + 1);
            for (int i = 0; i < nThreads; ++i)
            {
                Thread thread = new Thread(ComputeFrequenciesShared) { IsBackground = true };
                threads.Add(thread);
                thread.Start(i);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            barrier.SignalAndWait();

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            stopwatch.Stop();

            TimeSpan sharedTime = stopwatch.Elapsed;

            threads.Clear();
            barrier = new Barrier(nThreads + 1);
            for (int i = 0; i < nThreads; ++i)
            {
                Thread thread = new Thread(ComputeFrequenciesLocal) { IsBackground = true };
                threads.Add(thread);
                thread.Start(i);
            }

            stopwatch.Restart();
            barrier.SignalAndWait();

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            stopwatch.Stop();

            TimeSpan localTime = stopwatch.Elapsed;

            for (int i = 0; i < sharedFrequencies.Length; ++i)
            {
                if (sharedFrequencies[i] != sharedAggregatedFrequencies[i])
                {
                    throw new Exception(string.Format("Frequencies don't match for {0} : {1} vs {2}", i, sharedFrequencies[i], sharedAggregatedFrequencies[i]));
                }
            }

            Console.WriteLine("Shared data time: {0}", sharedTime);
            Console.WriteLine("Local data time: {0}", localTime);
        }
    }
}
