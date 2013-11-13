using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class LockFreeSample : ISample
    {
        abstract class BigNumberBase
        {
            protected const int n = 16;

            protected int[] number = new int[n];

            public abstract void Increment();

            public override string ToString()
            {
                return string.Join("", number);
            }
        }

        class NaiveBigNumber : BigNumberBase
        {
            public override void Increment()
            {
                int carry = 1;
                int i = n - 1;
                while (i > 0 && number[i] + carry == 10)
                {
                    number[i] = 0;
                    --i;
                }

                ++number[i];
            }
        }

        class LockedBigNumber : BigNumberBase
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(1);

            public override void Increment()
            {
                try
                {
                    semaphore.Wait();

                    int carry = 1;
                    int i = n - 1;
                    while (i > 0 && number[i] + carry == 10)
                    {
                        number[i] = 0;
                        --i;
                    }

                    ++number[i];
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        class LockFreeBigNumber : BigNumberBase
        {
            public override void Increment()
            {
                int[] original = number;
                int[] local = new int[n];

                do
                {
                    original = number;
                    Array.Copy(original, local, n);

                    int carry = 1;
                    int i = n - 1;
                    while (i > 0 && local[i] + carry == 10)
                    {
                        local[i] = 0;
                        --i;
                    }

                    ++local[i];
                } while (Interlocked.CompareExchange(ref number, local, original) != original);
            }
        }

        void Task(object o)
        {
            const int n = 1000000;

            BigNumberBase bigNumber = o as BigNumberBase;

            for (int i = 1; i <= n; ++i)
            {
                bigNumber.Increment();
            }
        }

        public void Run()
        {
            const int n = 4;

            NaiveBigNumber naiveBigNumber = new NaiveBigNumber();

            Thread[] threads = new Thread[n];

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(naiveBigNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan naiveTime = stopwatch.Elapsed;

            LockedBigNumber lockedBigNumber = new LockedBigNumber();

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(lockedBigNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockedTime = stopwatch.Elapsed;

            LockFreeBigNumber lockFreeBigNumber = new LockFreeBigNumber();

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(lockFreeBigNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockFreeTime = stopwatch.Elapsed;

            Console.WriteLine("Naive: {0} in {1}", naiveBigNumber.ToString(), naiveTime);
            Console.WriteLine("Locked: {0} in {1}", lockedBigNumber.ToString(), lockedTime);
            Console.WriteLine("Lock-free: {0} in {1}", lockFreeBigNumber.ToString(), lockFreeTime);
        }
    }
}
