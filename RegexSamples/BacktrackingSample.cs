﻿using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegexSamples
{
    public class BacktrackingSample : ISample
    {
        public void Run()
        {
            Regex noBacktrackingRegex = new Regex("ab", RegexOptions.Compiled);
            Regex backtrackingRegex = new Regex("a+b", RegexOptions.Compiled);

            const int maxInputSize = 100;
            const int n = 1000;

            backtrackingRegex.Match("a");
            noBacktrackingRegex.Match("a");

            string input = "";
            Stopwatch stopwatch = new Stopwatch();
            for (int inputSize = 1; inputSize <= maxInputSize; ++inputSize)
            {
                input += 'a';

                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    noBacktrackingRegex.Match(input);
                }
                stopwatch.Stop();

                TimeSpan noBacktrackingTime = stopwatch.Elapsed;

                stopwatch.Restart();
                for (int i = 0; i < n; ++i)
                {
                    backtrackingRegex.Match(input);
                }
                stopwatch.Stop();

                TimeSpan backtrackingTime = stopwatch.Elapsed;

                Console.WriteLine("{0}\t{1}", noBacktrackingTime.Ticks, backtrackingTime.Ticks);
            }
        }
    }
}
