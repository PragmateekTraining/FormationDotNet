using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class ReaderWriterLockSample : ISample
    {
        int n;
        decimal p;
        decimal pStart, pEnd, pStep;
        int threadCount;

        Barrier barrier;
        object @lock = new object();
        ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        void SimpleLocking()
        {
            // Console.WriteLine("Thread waiting to start...");

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

        void RWLocking()
        {
            // Console.WriteLine("Thread waiting to start...");

            barrier.SignalAndWait();

            // Console.WriteLine("Thread starting to loop...");

            Random rand = new Random();

            for (int i = 1; i <= n; ++i)
            {
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

        public ReaderWriterLockSample(int n, decimal pStart, decimal pEnd, decimal pStep, int threadCount)
        {
            this.n = n;
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.pStep = pStep;
            this.threadCount = threadCount;
        }

        public void Run()
        {
            IList<Thread> threads = new List<Thread>(threadCount);

            barrier = new Barrier(threadCount + 1);

            for (int i = 1; i <= threadCount; ++i)
            {
                Thread thread = new Thread(SimpleLocking) { IsBackground = true };
                threads.Add(thread);
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

            Console.WriteLine("Reference time: {0}", simpleLockingTime);

            for (p = pStart; p <= pEnd; p += pStep)
            {
                threads.Clear();
                barrier = new Barrier(threadCount + 1);

                for (int i = 1; i <= threadCount; ++i)
                {
                    Thread thread = new Thread(RWLocking) { IsBackground = true };
                    threads.Add(thread);
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

                Console.WriteLine("{0}\t{1}", p, 1.0 * rwLockingTime.TotalMilliseconds / simpleLockingTime.TotalMilliseconds);
            }
            // Console.WriteLine("With RW locking: {0}", rwLockingTime);
        }
    }
}
