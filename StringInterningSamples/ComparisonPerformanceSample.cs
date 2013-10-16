using System;
using System.Diagnostics;
using System.Runtime;
using SamplesAPI;

namespace StringInterningSamples
{
    public class ComparisonPerformanceSample : ISample
    {
        readonly bool useSameString;

        public ComparisonPerformanceSample(bool useSameString)
        {
            this.useSameString = useSameString;
        }

        public void Run()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            const int n = 1000000;

            string[] strings = new string[n];
            bool[] results = new bool[n - 1];

            for (int i = 0; i < n; ++i)
            {
                strings[i] = useSameString ? new string('a', 100) : Guid.NewGuid().ToString();
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < n - 1; ++i)
            {
                results[i] = strings[i] == strings[i + 1];
            }

            stopwatch.Stop();

            TimeSpan withoutInterningTime = stopwatch.Elapsed;

            for (int i = 0; i < n; ++i)
            {
                strings[i] = string.Intern(strings[i]);
            }

            stopwatch.Restart();

            for (int i = 0; i < n - 1; ++i)
            {
                results[i] = strings[i] == strings[i + 1];
            }

            stopwatch.Stop();

            TimeSpan withInterningTime = stopwatch.Elapsed;

            double ratio = 1.0 * withoutInterningTime.Ticks / withInterningTime.Ticks;

            Console.WriteLine("With interning: {0}", withInterningTime);
            Console.WriteLine("Without interning: {0} (x{1:N2})", withoutInterningTime, ratio);
        }
    }
}
