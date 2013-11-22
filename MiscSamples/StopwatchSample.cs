using SamplesAPI;
using System;
using System.Diagnostics;

namespace MiscSamples
{
    public class StopwatchSample : ISample
    {
        public void Run()
        {
            // Dump local stopwatch technical specifications.
            Console.WriteLine("Is high resolution? {0}", Stopwatch.IsHighResolution);
            Console.WriteLine("Frequency? {0}", Stopwatch.Frequency);

            // Measure start/stop overhead.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine("Elapsed: {0}", elapsed);
        }
    }
}
