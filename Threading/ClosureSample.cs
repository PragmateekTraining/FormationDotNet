using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class ClosureSample : ISample
    {
        public void Run()
        {
            const int n = 10;

            IList<Thread> threads = new List<Thread>();

            for (int i = 1; i <= n; ++i)
            {
                Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(new Random().Next(1000));
                        Console.WriteLine(i);
                    })
                    {
                        IsBackground = true
                    };

                thread.Start();

                threads.Add(thread);

                Thread.Sleep(new Random().Next(1000));
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine(new String('=', 70));

            threads.Clear();

            for (int i = 1; i <= n; ++i)
            {
                int local = i;

                Thread thread = new Thread(() =>
                {
                    Thread.Sleep(new Random().Next(1000));
                    Console.WriteLine(local);
                })
                {
                    IsBackground = true
                };

                thread.Start();

                threads.Add(thread);

                Thread.Sleep(new Random().Next(1000));
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine(new String('=', 70));

            threads.Clear();

            for (int i = 1; i <= n; ++i)
            {
                Thread thread = new Thread(o =>
                {
                    Thread.Sleep(new Random().Next(1000));
                    Console.WriteLine((int)o);
                })
                {
                    IsBackground = true
                };

                thread.Start(i);

                threads.Add(thread);

                Thread.Sleep(new Random().Next(1000));
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
