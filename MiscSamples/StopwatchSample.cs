using SamplesAPI;
using System;
using System.Diagnostics;

namespace MiscSamples
{
    public class StopwatchSample : ISample
    {
        public void Run()
        {
            Console.WriteLine("Is high resolution? {0}", Stopwatch.IsHighResolution);
            Console.WriteLine("Frequency? {0}", Stopwatch.Frequency);
        }
    }
}
