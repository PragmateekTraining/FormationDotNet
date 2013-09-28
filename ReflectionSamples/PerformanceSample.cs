using System;
using SamplesAPI;
using System.Reflection;
using System.Diagnostics;

namespace ReflectionSamples
{
    public class PerformanceSample : ISample
    {
        public static void F()
        {
        }

        private long n;

        public PerformanceSample(long n)
        {
            this.n = n;
        }

        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 1; i <= n; ++i)
            {
                F();
            }
            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            MethodInfo method = typeof(PerformanceSample).GetMethod("F", BindingFlags.Public | BindingFlags.Static);

            stopwatch.Restart();
            for (int i = 1; i <= n; ++i)
            {
                method.Invoke(null, null);
            }
            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            long ratio = t2.Ticks / t1.Ticks;

            Console.WriteLine("Without reflection: {0}", t1);
            Console.WriteLine("With reflection: {0} (x{1})", t2, ratio);
        }
    }
}
