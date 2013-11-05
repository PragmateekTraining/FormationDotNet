using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class ConcurrentQueueSample : ISample
    {
        const int n = 1000000;
        const int nThread = 10;

        Queue<int> dynamicSizeQueue = new Queue<int>();

        Queue<int> staticSizeQueue = new Queue<int>(n * nThread);

        Queue<int> lockedQueue = new Queue<int>();

        ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();

        void AddToDynamicQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                try
                {
                    dynamicSizeQueue.Enqueue(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caugh exception while adding to dynamic list:\n{0}", e);
                    break;
                }
            }
        }

        void AddToStaticQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                try
                {
                    staticSizeQueue.Enqueue(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caugh exception while adding to static list:\n{0}", e);
                    break;
                }
            }
        }

        void AddToLockedQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                try
                {
                    lock (lockedQueue)
                    {
                        lockedQueue.Enqueue(0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caugh exception while adding to locked list:\n{0}", e);
                    break;
                }
            }
        }

        void AddToConcurrentQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                try
                {
                    concurrentQueue.Enqueue(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caugh exception while adding to concurrent queue:\n{0}", e);
                    break;
                }
            }
        }

        void ReadFromLockedQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                lock (lockedQueue)
                {
                    lockedQueue.Dequeue();
                }
            }
        }

        void ReadFromConcurrentQueue()
        {
            for (int i = 0; i < n; ++i)
            {
                int value;
                concurrentQueue.TryDequeue(out value);
            }
        }

        public void Run()
        {
            IList<Thread> threads = new List<Thread>();

            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(AddToDynamicQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Dynamic queue size: {0}", dynamicSizeQueue.Count);

            threads.Clear();

            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(AddToStaticQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Static queue size: {0}", staticSizeQueue.Count);

            threads.Clear();

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(AddToLockedQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockedListTime = stopwatch.Elapsed;

            Console.WriteLine("Locked queue size: {0}", lockedQueue.Count);
            Console.WriteLine("Locked queue time: {0}", lockedListTime);

            threads.Clear();

            stopwatch.Restart();
            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(AddToConcurrentQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan concurrentQueueTime = stopwatch.Elapsed;

            Console.WriteLine("Concurrent queue size: {0}", concurrentQueue.Count);
            Console.WriteLine("Concurrent queue time: {0}", concurrentQueueTime);

            threads.Clear();

            stopwatch.Restart();
            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(ReadFromLockedQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockedQueueReadTime = stopwatch.Elapsed;

            Console.WriteLine("Locked queue size: {0}", lockedQueue.Count);
            Console.WriteLine("Locked queue read time: {0}", lockedQueueReadTime);

            threads.Clear();

            stopwatch.Restart();
            for (int i = 0; i < nThread; ++i)
            {
                Thread thread = new Thread(ReadFromConcurrentQueue) { IsBackground = true };
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan concurrentQueueReadTime = stopwatch.Elapsed;

            Console.WriteLine("Concurrent queue size: {0}", concurrentQueue.Count);
            Console.WriteLine("Concurrent queue read time: {0}", concurrentQueueReadTime);
        }
    }
}
