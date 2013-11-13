using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class LockFreeSample2 : ISample
    {
        abstract class StrangeNumberBase
        {
            protected int value;

            public abstract void Increment();

            public override string ToString()
            {
                return value.ToString();
            }
        }

        class NaiveNumber : StrangeNumberBase
        {
            public override void Increment()
            {
                value += 2;
            }
        }

        class LockedNumber : StrangeNumberBase
        {
            object @lock = new object();

            public override void Increment()
            {
                lock (@lock)
                {
                    value += 2;
                }
            }
        }

        class LockFreeNumber : StrangeNumberBase
        {
            public override void Increment()
            {
                int current;

                do
                {
                    current = value;
                }
                while (Interlocked.CompareExchange(ref value, current + 2, current) != current);
            }
        }

        void Task(object o)
        {
            const int n = 1000000;

            StrangeNumberBase number = o as StrangeNumberBase;

            for (int i = 1; i <= n; ++i)
            {
                number.Increment();
            }
        }

        public void Run()
        {
            const int n = 4;

            NaiveNumber naiveNumber = new NaiveNumber();

            Thread[] threads = new Thread[n];

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(naiveNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan naiveTime = stopwatch.Elapsed;

            LockedNumber lockedNumber = new LockedNumber();

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(lockedNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockedTime = stopwatch.Elapsed;

            LockFreeNumber lockFreeNumber = new LockFreeNumber();

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                (threads[i - 1] = new Thread(Task) { IsBackground = true }).Start(lockFreeNumber);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();

            TimeSpan lockFreeTime = stopwatch.Elapsed;

            Console.WriteLine("Naive: {0} in {1}", naiveNumber.ToString(), naiveTime);
            Console.WriteLine("Locked: {0} in {1}", lockedNumber.ToString(), lockedTime);
            Console.WriteLine("Lock-free: {0} in {1}", lockFreeNumber.ToString(), lockFreeTime);
        }
    }
}
