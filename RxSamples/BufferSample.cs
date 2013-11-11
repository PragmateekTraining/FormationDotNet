using SamplesAPI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace RxSamples
{
    public class BufferSample : ISample
    {
        public void Run()
        {
            const int n = 70;

            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
                      .Select(l => l % n == 0 ? '*' : '=')
                      .StartWith(new String('=', n-1))
                      .Buffer(n, 1)
                      .Subscribe(l => Console.Write("\r{0}", String.Join("", l.ToArray())));

            Console.ReadLine();
        }
    }
}
