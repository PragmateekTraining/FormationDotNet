using SamplesAPI;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace RxSamples
{
    public class OnErrorResumeNextSample : ISample
    {
        public void Run()
        {
            Subject<int> subject = new Subject<int>();

            Func<IObservable<int>> factory = () => subject.Select(i => 6 / i);

            IObservable<int> numbers = factory().OnErrorResumeNext(factory());

            numbers.Subscribe(Console.WriteLine);

            for (int i = 3; i >= -3; --i)
            {
                subject.OnNext(i);
            }
        }
    }
}
