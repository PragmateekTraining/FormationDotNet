using SamplesAPI;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace RxSamples
{
    public class SkipLastSample : ISample
    {
        public void Run()
        {
            Subject<int> integers = new Subject<int>();
            integers.Subscribe(i => Console.WriteLine("Pushed {0}.", i));

            IObservable<int> filteredIntegers = integers.SkipLast(3);
            filteredIntegers.Subscribe(i => Console.WriteLine("Got {0}.", i));

            for (int i = 1; i <= 10; ++i)
            {
                integers.OnNext(i);
            }

            integers.OnCompleted();
        }
    }
}
