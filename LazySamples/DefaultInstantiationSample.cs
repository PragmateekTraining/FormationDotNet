using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace LazySamples
{
    public class DefaultInstantiationSample : ISample
    {
        public void Run()
        {
            const int n = 10000000;

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 1; i <= n; ++i)
            {
                Object o = null;

                LazyInitializer.EnsureInitialized(ref o);
            }

            stopwatch.Stop();

            TimeSpan withoutFactoryTime = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 1; i <= n; ++i)
            {
                Object o = null;

                LazyInitializer.EnsureInitialized(ref o, () => new Object());
            }

            stopwatch.Stop();

            TimeSpan withFactoryTime = stopwatch.Elapsed;

            double ratio = 1.0 * withoutFactoryTime.Ticks / withFactoryTime.Ticks;

            Console.WriteLine("With factory time: {0}", withFactoryTime);
            Console.WriteLine("Without factory time: {0} (x{1:N2})", withoutFactoryTime, ratio);
        }
    }
}
