using SamplesAPI;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace RxSamples
{
    public class InternalsSample : ISample
    {
        public void Run()
        {
            IObservable<int> stream = Observable.Interval(TimeSpan.FromSeconds(1))
                                                .Select(l => Thread.CurrentThread.ManagedThreadId);

            Func<int, Action<int>> dump = i => id => Console.WriteLine("{0} on {1}", i, id);

            stream.Subscribe(dump(1));
            stream.Subscribe(dump(2));
            stream.Subscribe(dump(3));

            Console.WriteLine("Press enter to stop...");
            Console.ReadLine();
        }
    }
}
