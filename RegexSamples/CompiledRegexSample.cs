using SamplesAPI;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexSamples
{
    public class CompiledRegexSample : ISample
    {
        public void Run()
        {
            const int size = 10000000;
            const int n = 100000;

            Regex interpreted = new Regex("abc");
            Regex compiled = new Regex("abc", RegexOptions.Compiled);
            Regex preCompiled = new Tools.PrecompiledRegex.ABCPattern();

            Random rand = new Random();
            StringBuilder textBuilder = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                textBuilder.Append((char)('a' + rand.Next(26)));
            }

            string text = textBuilder.ToString();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < n; ++i)
            {
                interpreted.Match(text);
            }
            stopwatch.Stop();

            TimeSpan interpretedTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                compiled.Match(text);
            }
            stopwatch.Stop();

            TimeSpan compiledTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                preCompiled.Match(text);
            }
            stopwatch.Stop();

            TimeSpan preCompiledTime = stopwatch.Elapsed;

            double interpretedToPreCompiledRatio = 1.0 * interpretedTime.Ticks / compiledTime.Ticks;
            double compiledToPreCompiledRatio = 1.0 * compiledTime.Ticks / preCompiledTime.Ticks;

            Console.WriteLine("Pre-compiled: {0}", preCompiledTime);
            Console.WriteLine("Compiled: {0} (x{1:N2})", compiledTime, compiledToPreCompiledRatio);
            Console.WriteLine("Interpreted: {0} (x{1:N1})", interpretedTime, interpretedToPreCompiledRatio);
        }
    }
}
