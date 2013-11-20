using SamplesAPI;
using System;
using System.Reactive.Linq;

namespace RxSamples
{
    public class AmbSample : ISample
    {
        public void Run()
        {
            Random rand = new Random();

            Func<int, IObservable<int>> factory = i => Observable.Timer(TimeSpan.FromMilliseconds(rand.Next(1000)), TimeSpan.FromSeconds(1))
                                                                 .Select(l => i);

            while (true)
            {
                using (new[] { factory(1), factory(2), factory(3) }.Amb().Subscribe(Console.WriteLine))
                {
                    Console.WriteLine("Press 'x' to stop or any other key to run again...");
                    if (Console.ReadKey().KeyChar == 'x') break;
                }
            }
        }
    }
}
