using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace RxSamples
{
    public class PullPushSample : ISample
    {
        IEnumerable<int> Numbers
        {
            get
            {
                Thread.Sleep(1000);
                yield return 1;
                Thread.Sleep(2000);
                yield return 2;
                Thread.Sleep(3000);
                yield return 3;
                yield break;
            }
        }

        public void Run()
        {
            Console.WriteLine("Before enumeration.");

            foreach (int n in Numbers)
            {
                Console.WriteLine(n);
            }

            Console.WriteLine("After enumeration.");


            Console.WriteLine("Before subscription.");

            Numbers.ToObservable(Scheduler.ThreadPool)
                   .Subscribe(Console.WriteLine);

            Console.WriteLine("After subscription.");

            Console.WriteLine("Press enter to end.");
            Console.ReadLine();
        }
    }
}
