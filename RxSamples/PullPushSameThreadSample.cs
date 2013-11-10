using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxSamples
{
    public class PullPushSameThreadSample : ISample
    {
        public void Run()
        {
            IEnumerable<int> enumeration = Enumerable.Range(1, 3);

            Console.WriteLine("Before enumeration.");

            foreach (int n in enumeration)
            {
                Console.WriteLine(n);
            }

            Console.WriteLine("After enumeration.");


            IObservable<int> observable = Observable.Range(1, 3);

            Console.WriteLine("Before subscription.");

            observable.Subscribe(Console.WriteLine);

            Console.WriteLine("After subscription.");


            Console.WriteLine("Press enter to end.");
        }
    }
}
