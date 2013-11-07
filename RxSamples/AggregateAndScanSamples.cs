using SamplesAPI;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxSamples
{
    public class AggregateAndScanSamples : ISample
    {
        public void Run()
        {
            ReplaySubject<int> numbers = new ReplaySubject<int>();

            numbers.Subscribe(i => Console.WriteLine("New number: {0}", i), () => Console.WriteLine("Numbers sequence completed."));

            IObservable<double> liveMean = numbers.Scan(new { N = 0, Sum = 0 }, (o, i) => new { N = o.N + 1, Sum = o.Sum + i })
                                      .Select(o => 1.0 * o.Sum / o.N);

            liveMean.Subscribe(m => Console.WriteLine("Live mean is {0}", m), () => Console.WriteLine("First scan completed."));

            while (true)
            {
                Console.Write("n? ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    numbers.OnCompleted();
                    break;
                }

                numbers.OnNext(int.Parse(input));
            }

            IObservable<double> finalMean = numbers.Aggregate(new { N = 0, Sum = 0 }, (o, i) => new { N = o.N + 1, Sum = o.Sum + i })
                                                   .Select(o => 1.0 * o.Sum / o.N);

            finalMean.Subscribe(m => Console.WriteLine("Final mean is {0}", m), () => Console.WriteLine("Final mean completed."));

            IObservable<double>  secondScanMean = numbers.Scan(new { N = 0, Sum = 0 }, (o, i) => new { N = o.N + 1, Sum = o.Sum + i })
                                                         .Select(o => 1.0 * o.Sum / o.N);

            secondScanMean.Subscribe(m => Console.WriteLine("Second scan mean is {0}", m), () => Console.WriteLine("Second scan completed."));
        }
    }
}
