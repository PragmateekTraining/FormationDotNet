using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegexSamples
{
    public class ExponentialBacktrackingSample : ISample
    {
        public void Run()
        {
            Regex regex = new Regex("^(a+a+)+b$", RegexOptions.Compiled);

            const int maxInputSize = 15;
            const int n = 1000;

            regex.Match("a");

            string input = "";
            Stopwatch stopwatch = new Stopwatch();
            for (int inputSize = 1; inputSize <= maxInputSize; ++inputSize)
            {
                input += "a";

                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    regex.Match(input);
                }
                stopwatch.Stop();

                Console.WriteLine(stopwatch.Elapsed.Ticks);
            }
        }
    }
}
