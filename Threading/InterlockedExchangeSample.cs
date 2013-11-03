using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class InterlockedExchangeSample : ISample
    {
        int value = 0;
        const int n = 10000000;

        class NaiveLock
        {
            volatile int isLocked = 0;

            public void Enter()
            {
                while (isLocked == 1) ;
                isLocked = 1;
            }

            public void Exit()
            {
                isLocked = 0;
            }
        }

        class AtomicLock
        {
            int isLocked = 0;

            public void Enter()
            {
                while (Interlocked.Exchange(ref isLocked, 1) == 1) ;
                isLocked = 1;
            }

            public void Exit()
            {
                isLocked = 0;
            }
        }

        class AtomicLock2
        {
            int isLocked = 0;

            public void Enter()
            {
                while (Interlocked.CompareExchange(ref isLocked, 1, 0) == 1) ;
            }

            public void Exit()
            {
                isLocked = 0;
            }
        }

        NaiveLock naiveLock = new NaiveLock();

        void NaiveLockIncrement()
        {
            for (int i = 1; i <= n; ++i)
            {
                naiveLock.Enter();
                ++value;
                naiveLock.Exit();
            }
        }

        AtomicLock atomicLock = new AtomicLock();

        void AtomicLockIncrement()
        {
            for (int i = 1; i <= n; ++i)
            {
                atomicLock.Enter();
                ++value;
                atomicLock.Exit();
            }
        }

        AtomicLock2 atomicLock2 = new AtomicLock2();

        void AtomicLock2Increment()
        {
            for (int i = 1; i <= n; ++i)
            {
                atomicLock2.Enter();
                ++value;
                atomicLock2.Exit();
            }
        }

        public void Run()
        {
            Thread naiveIncrement = new Thread(NaiveLockIncrement) { IsBackground = true };
            naiveIncrement.Start();

            for (int i = 1; i <= n; ++i)
            {
                naiveLock.Enter();
                ++value;
                naiveLock.Exit();
            }

            naiveIncrement.Join();

            Console.WriteLine(value);

            value = 0;

            Thread atomicIncrement = new Thread(AtomicLockIncrement) { IsBackground = true };
            atomicIncrement.Start();

            for (int i = 1; i <= n; ++i)
            {
                atomicLock.Enter();
                ++value;
                atomicLock.Exit();
            }

            atomicIncrement.Join();

            Console.WriteLine(value);

            value = 0;

            Thread atomic2Increment = new Thread(AtomicLock2Increment) { IsBackground = true };
            atomic2Increment.Start();

            for (int i = 1; i <= n; ++i)
            {
                atomicLock2.Enter();
                ++value;
                atomicLock2.Exit();
            }

            atomic2Increment.Join();

            Console.WriteLine(value);

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; i <= n; ++i)
            {
                atomicLock.Enter();
                atomicLock.Exit();
            }
            stopwatch.Stop();

            TimeSpan atomicLockTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                atomicLock2.Enter();
                atomicLock2.Exit();
            }
            stopwatch.Stop();

            TimeSpan atomicLock2Time = stopwatch.Elapsed;

            Console.WriteLine(atomicLockTime);
            Console.WriteLine(atomicLock2Time);
        }
    }
}
