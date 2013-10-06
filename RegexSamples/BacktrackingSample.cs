using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegexSamples
{
    public class BacktrackingSample : ISample
    {
        public void Run()
        {
            Regex regex = new Regex("^(a+a+)+b$", RegexOptions.Compiled);

            const int maxInputSize = 10;
            const int n = 1000;

            string input = "";
            Stopwatch stopwatch = new Stopwatch();
            for (int inputSize = 1; inputSize <= maxInputSize; ++inputSize)
            {
                input += 'a';

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
