using System;
using System.Threading;

namespace Threading
{
    class BarrierSample
    {
        const int N = 4;

        readonly bool useBarrier = false;

        Barrier barrier = null;

        public BarrierSample(bool useBarrier)
        {
            this.useBarrier = useBarrier;
            if (useBarrier) barrier = new Barrier(N);
        }

        void DoWork(object o)
        {
            int n = (int)o;

            Random rand = new Random();

            Console.WriteLine("Thread {0} starts step 1.", n);
            Thread.Sleep(rand.Next(100));
            Console.WriteLine("Thread {0} ends step 1.", n);

            if (useBarrier) barrier.SignalAndWait();

            Console.WriteLine("Thread {0} starts step 2.", n);
            Thread.Sleep(rand.Next(100));
            Console.WriteLine("Thread {0} ends step 2.", n);

            if (useBarrier) barrier.SignalAndWait();

            Console.WriteLine("Thread {0} ends.", n);
        }

        public void Run()
        {
            for (int i = 0; i < N; ++i)
            {
                new Thread(DoWork).Start(i);
            }
        }
    }
}
