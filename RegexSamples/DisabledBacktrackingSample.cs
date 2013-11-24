using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace RegexSamples
{
    public class DisabledBacktrackingSample : ISample
    {
        bool checkRatherThanBenchmarking = false;

        public DisabledBacktrackingSample(bool checkRatherThanBenchmarking)
        {
            this.checkRatherThanBenchmarking = checkRatherThanBenchmarking;
        }

        public void Run()
        {
            Regex backtrackingRegex = new Regex("a+b", RegexOptions.Compiled);
            Regex disabledBacktrackingRegex = new Regex("(?>a+)b", RegexOptions.Compiled);

            const int maxInputSize = 100;
            const int n = 1000;

            backtrackingRegex.Match("a");
            disabledBacktrackingRegex.Match("a");

            Random rand = new Random();

            string input = "";
            Stopwatch stopwatch = new Stopwatch();
            double meanRatio = 0.0;
            for (int inputSize = 1; inputSize <= maxInputSize; ++inputSize)
            {
                if (checkRatherThanBenchmarking)
                {
                    input += rand.Next(5) != 0 ? 'a' : 'b';
                }
                else
                {
                    input += 'a';
                }

                Match[] noBacktrackingMatches = null;
                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    noBacktrackingMatches = disabledBacktrackingRegex.Matches(input).Cast<Match>().ToArray();
                }
                stopwatch.Stop();

                TimeSpan noBacktrackingTime = stopwatch.Elapsed;

                Match[] backtrackingMatches = null;
                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    backtrackingMatches = backtrackingRegex.Matches(input).Cast<Match>().ToArray();
                }
                stopwatch.Stop();

                TimeSpan backtrackingTime = stopwatch.Elapsed;

                double ratio = 1.0 * backtrackingTime.Ticks / noBacktrackingTime.Ticks;

                if (noBacktrackingMatches.Length != backtrackingMatches.Length)
                {
                    throw new Exception("Matches counts differ!");
                }

                if (checkRatherThanBenchmarking)
                {
                    for (int i = 0; i < noBacktrackingMatches.Length; ++i)
                    {
                        if (noBacktrackingMatches[i].Value != backtrackingMatches[i].Value)
                        {
                            throw new Exception(string.Format("Matches differ at index {0}: '{1}' vs '{2}'!", i, noBacktrackingMatches[i].Value, backtrackingMatches[i].Value));
                        }
                    }
                }

                meanRatio += ratio / maxInputSize;

                Console.WriteLine("{0}\t{1:N2}", inputSize, ratio);
            }

            Console.WriteLine("Mean ratio: {0:N2}", meanRatio);
        }
    }
}
