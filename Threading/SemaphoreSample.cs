using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class SemaphoreSample : ISample
    {
        SemaphoreSlim semaphore;

        void F(object o)
        {
            int i = (int)o;

            Console.WriteLine("Thread#{0} going to wait...", i);
            semaphore.Wait();
            Console.WriteLine("Thread#{0} in!", i);
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("Number of threads? ");
                int n = int.Parse(Console.ReadLine());

                Console.Write("Capacity? ");
                int capacity = int.Parse(Console.ReadLine());
                Console.Write("Initial count? ");
                int initialCount = int.Parse(Console.ReadLine());

                semaphore = new SemaphoreSlim(initialCount, capacity);

                Thread[] threads = new Thread[n];
                for (int i = 0; i < n; ++i)
                {
                    (threads[i] = new Thread(F) { IsBackground = true }).Start(i);
                }

                Console.Write("Continue? ");
                string input = Console.ReadLine();

                foreach (Thread thread in threads)
                {
                    thread.Abort();
                }

                if (!input.Equals("y", StringComparison.InvariantCultureIgnoreCase)) break;
            }
        }
    }
}
