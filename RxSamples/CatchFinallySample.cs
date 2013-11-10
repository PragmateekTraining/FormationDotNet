using SamplesAPI;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace RxSamples
{
    public class CatchFinallySample : ISample
    {
        public void Run()
        {
            Subject<int> subject = new Subject<int>();

            IObservable<int> numbers = subject.Select(i => 6 / i).Catch(Observable.Return(-1)).Finally(() => Console.WriteLine("Done."));

            using (numbers.Subscribe(i => Console.WriteLine("[1] {0}", i)))
            {
                using (numbers.Subscribe(i => Console.WriteLine("[2] {0}", i)))
                {
                    subject.OnNext(3);
                    subject.OnNext(2);
                    subject.OnNext(1);
                    subject.OnNext(0);
                }
            }
        }
    }
}
