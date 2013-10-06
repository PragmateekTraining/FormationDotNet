using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegexSamples
{
    public class AnchorsSample : ISample
    {
        public void Run()
        {
            Regex noAnchorsRegex = new Regex("a+b", RegexOptions.Compiled);
            Regex withAnchorsRegex = new Regex("^a+b$", RegexOptions.Compiled);

            const int maxInputSize = 100;
            const int n = 1000;

            noAnchorsRegex.Match("a");
            withAnchorsRegex.Match("a");

            string input = "";
            Stopwatch stopwatch = new Stopwatch();
            for (int inputSize = 1; inputSize <= maxInputSize; ++inputSize)
            {
                input += 'a';

                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    noAnchorsRegex.Match(input);
                }
                stopwatch.Stop();

                TimeSpan noAnchorsTime = stopwatch.Elapsed;

                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    withAnchorsRegex.Match(input);
                }
                stopwatch.Stop();

                TimeSpan withAnchorsTime = stopwatch.Elapsed;

                Console.WriteLine("{0}\t{1}", noAnchorsTime.Ticks, withAnchorsTime.Ticks);
            }
        }
    }
}
