using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace LazySamples
{
    public class LazyInitializerSample : ISample
    {
        public void Run()
        {
            const int n = 10000000;

            Lazy<object>[] lazy = new Lazy<object>[n];
            object[] lazyInitializer = new object[n];

            for (int i = 0; i < n; ++i)
            {
                lazy[i] = new Lazy<object>(() => new object());
                lazyInitializer[i] = null;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            object o;

            for (int i = 0; i < n; ++i)
            {
                o = lazy[i].Value;
            }

            stopwatch.Stop();

            TimeSpan lazyInitialTime = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                LazyInitializer.EnsureInitialized(ref lazyInitializer[i], () => new object());
            }

            stopwatch.Stop();

            TimeSpan lazyInitializerInitialTime = stopwatch.Elapsed;

            double ratioInitial = 1.0 * lazyInitialTime.Ticks / lazyInitializerInitialTime.Ticks;

            Console.WriteLine("LazyInitializer<T> first access: {0}", lazyInitializerInitialTime);
            Console.WriteLine("Lazy<T> first access: {0} (x{1:N2})", lazyInitialTime, ratioInitial);

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                o = lazy[i].Value;
            }

            stopwatch.Stop();

            TimeSpan lazySecondTime = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                LazyInitializer.EnsureInitialized(ref lazyInitializer[i], () => new object());
            }

            stopwatch.Stop();

            TimeSpan lazyInitializerSecondTime = stopwatch.Elapsed;

            double ratioSecond = 1.0 * lazySecondTime.Ticks / lazyInitializerSecondTime.Ticks;

            Console.WriteLine("LazyInitializer<T> second access: {0}", lazyInitializerSecondTime);
            Console.WriteLine("Lazy<T> second access: {0} (x{1:N2})", lazySecondTime, ratioSecond);
        }
    }
}
