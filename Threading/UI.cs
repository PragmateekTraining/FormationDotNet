using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadingSamples
{
    class UI
    {
        static DateTime? T = null;

        static void DoWork()
        {
            while (DateTime.Now < T) ;
        }

        static IEnumerable<Thread> threads = Enumerable.Range(1, 10).Select(i => new Thread(DoWork) { Priority = ThreadPriority.AboveNormal }).ToList();

        static void RunThreads()
        {
            T = DateTime.Now.AddSeconds(10);

            foreach (Thread thread in threads) thread.Start();
        }

        internal static void Run()
        {
            new Thread(RunThreads).Start();

            Console.Write("Type some text: ");
            Console.ReadLine();
        }
    }
}
