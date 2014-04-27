using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class ReaderWriterLockBenchmark
    {
        private readonly int n;
        private readonly decimal pStart, pEnd, pStep;
        private readonly int threadCount;

        /// <summary>
        /// Current proportion of writes.
        /// </summary>
        private decimal p;

        /// <summary>
        /// The primitive used to ensure that all threads start to work at the same time.
        /// </summary>
        private Barrier barrier;

        object @lock = new object();
        ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Take a full lock for both read and write operations.
        /// </summary>
        void SimpleLocking()
        {
            // Console.WriteLine("Thread waiting to start...");

            // Wait the other threads.
            barrier.SignalAndWait();

           // Console.WriteLine("Thread starting to loop...");

            for (int i = 1; i <= n; ++i)
            {
                lock (@lock)
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// Take a read lock when reading and a write lock when writing.
        /// </summary>
        void RWLocking()
        {
            // Console.WriteLine("Thread waiting to start...");

            // Wait the other threads.
            barrier.SignalAndWait();

            // Console.WriteLine("Thread starting to loop...");

            Random rand = new Random();

            for (int i = 1; i <= n; ++i)
            {
                // If we are in the write range then take a write-lock...
                if (1.0m * rand.Next() / int.MaxValue <= p)
                {
                    try
                    {
                        rwLock.EnterWriteLock();
                        Thread.Sleep(1);
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                // ... otherwise only take a read-lock
                else
                {
                    try
                    {
                        rwLock.EnterReadLock();
                        Thread.Sleep(1);
                    }
                    finally
                    {
                        rwLock.ExitReadLock();
                    }
                }
            }
        }

        /// <summary>
        /// Initialize a new benchmark.
        /// </summary>
        /// <param name="n">The number of operations done by each thread.</param>
        /// <param name="pStart">The initial proportion of writes.</param>
        /// <param name="pEnd">The final proportion of writes.</param>
        /// <param name="pStep">The increase of the proportion of writes at each step.</param>
        /// <param name="threadCount">The number of threads to run.</param>
        public ReaderWriterLockBenchmark(int n, decimal pStart, decimal pEnd, decimal pStep, int threadCount)
        {
            this.n = n;
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.pStep = pStep;
            this.threadCount = threadCount;
        }

        private TimeSpan MeasureReferenceTime()
        {
            // Track the running threads for joining them.
            Thread[] threads = new Thread[threadCount];

            barrier = new Barrier(threadCount + 1);

            for (int i = 0; i < threadCount; ++i)
            {
                Thread thread = new Thread(SimpleLocking) { IsBackground = true };
                threads[i] = thread;
                thread.Start();
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            barrier.SignalAndWait();

            for (int i = 0; i < threadCount; ++i)
            {
                threads[i].Join();
            }
            stopwatch.Stop();

            TimeSpan simpleLockingTime = stopwatch.Elapsed;

            return simpleLockingTime;
        }

        private IDictionary<decimal, double> Benchmark(TimeSpan simpleLockingTime)
        {
            IDictionary<decimal, double> results = new Dictionary<decimal, double>();

            Thread[] threads = new Thread[threadCount];

            Stopwatch stopwatch = new Stopwatch();
            for (p = pStart; p <= pEnd; p += pStep)
            {
                barrier = new Barrier(threadCount + 1);

                for (int i = 1; i <= threadCount; ++i)
                {
                    Thread thread = new Thread(RWLocking) { IsBackground = true };
                    threads[i] = thread;
                    thread.Start();
                }

                stopwatch.Restart();
                barrier.SignalAndWait();

                for (int i = 0; i < threadCount; ++i)
                {
                    threads[i].Join();
                }
                stopwatch.Stop();

                TimeSpan rwLockingTime = stopwatch.Elapsed;

                double ratio = 1.0 * rwLockingTime.TotalMilliseconds / simpleLockingTime.TotalMilliseconds;

                results[p] = ratio;
            }

            return results;
        }

        public void Run()
        {
            TimeSpan simpleLockingTime = MeasureReferenceTime();

            // Console.WriteLine("Reference time: {0}", simpleLockingTime);

            IDictionary<decimal, double> results = Benchmark(simpleLockingTime);

            foreach (KeyValuePair<decimal, double> pair in results)
            {
                Console.WriteLine(pair.Key + "\t" + pair.Value);
            }

            //Console.WriteLine("{0}\t{1}", p, 1.0 * rwLockingTime.TotalMilliseconds / simpleLockingTime.TotalMilliseconds);
        }
    }

    public class ReaderWriterLockSample : ISample
    {
        private readonly ReaderWriterLockBenchmark benchmark = null;

        public ReaderWriterLockSample(int n, decimal pStart, decimal pEnd, decimal pStep, int threadCount)
        {
            benchmark = new ReaderWriterLockBenchmark(n, pStart, pEnd, pStep, threadCount);
        }

        public void Run()
        {
            benchmark.Run();
        }
    }
}
