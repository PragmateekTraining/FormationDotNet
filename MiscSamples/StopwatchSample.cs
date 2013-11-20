using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
