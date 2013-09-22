using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Threading
{
    class Scalability
    {
        const int NRand = 200000000;
        const int NThread = 20;

        void GenerateNumbers(object o)
        {
            int n = (int)o;
            Random rand = new Random();
            for (int i = 0; i < NRand/n; ++i)
            {
                rand.NextDouble();
            }
        }

        public void Run()
        {
            TimeSpan[] times = new TimeSpan[NThread];

            for (int n = 1; n <= NThread; ++n)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                IList<Thread> threads = new List<Thread>();
                for (int i = 1; i <= n; ++i)
                {
                    Thread thread = new Thread(GenerateNumbers);
                    threads.Add(thread);
                    thread.Start(n);
                }
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
                stopwatch.Stop();
                times[n - 1] = stopwatch.Elapsed;
            }

            for (int i = 0; i < times.Length; ++i)
            {
                Console.WriteLine("{0}\t{1}", i + 1, times[i].TotalSeconds);
            }
        }
    }
}
