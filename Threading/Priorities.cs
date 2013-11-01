using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadingSamples
{
    class Priorities
    {
        static readonly long[] counters = new long[3];

        static DateTime? T = null;

        static void Count(object o)
        {
            int i = (int)o;

            while (DateTime.Now < T)
            {
                ++counters[i];
            }
        }

        internal static void Run()
        {
            IEnumerable<Thread> lows = Enumerable.Range(1, 10).Select(i => new Thread(Count) { Priority = ThreadPriority.Lowest }).ToList();
            IEnumerable<Thread> mediums = Enumerable.Range(1, 10).Select(i => new Thread(Count) { Priority = ThreadPriority.Normal }).ToList();
            IEnumerable<Thread> highs = Enumerable.Range(1, 10).Select(i => new Thread(Count) { Priority = ThreadPriority.Highest }).ToList();

            IEnumerable<Thread> all = lows.Concat(mediums).Concat(highs);

            T = DateTime.Now.AddSeconds(10);

            int c = 0;
            foreach (Thread thread in all)
            {
                thread.Start(thread.Priority == ThreadPriority.Lowest ? 0 :
                             thread.Priority == ThreadPriority.Normal ? 1 :
                             2);
                Console.WriteLine("Started {0} threads.", ++c);
            }

            foreach (Thread thread in all)
            {
                thread.Join();
            }

            long sum = counters[0] + counters[1] + counters[2];

            Console.WriteLine("low: {0} ({3:N2}%)\nmedium: {1} ({4:N2}%)\nhigh: {2} ({5:N2}%)", counters[0], counters[1], counters[2], 1.0 * counters[0] / sum * 100, 1.0 * counters[1] / sum * 100, 1.0 * counters[2] / sum * 100);
        }
    }
}
