using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LINQSamples
{
    public class AnyVsCountSample : ISample
    {
        public void Run()
        {
            const int size = 1000;
            const int n = 10000;

            IList<string> strings = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                strings.Add(i.ToString());
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < n; ++i)
            {
                strings.Count();
            }
            stopwatch.Stop();

            TimeSpan collectionCountTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                strings.Any();
            }
            stopwatch.Stop();

            TimeSpan collectionAnyTime = stopwatch.Elapsed;

            IEnumerable<string> query = strings.Where(s => int.Parse(s) % 2 == 0);

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                query.Count();
            }
            stopwatch.Stop();

            TimeSpan queryCountTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                query.Any();
            }
            stopwatch.Stop();

            TimeSpan queryAnyTime = stopwatch.Elapsed;

            double anyVsCountCollectionRatio = 1.0 * collectionAnyTime.Ticks / collectionCountTime.Ticks;
            double countVsAnyQueryRatio = 1.0 * queryCountTime.Ticks / queryAnyTime.Ticks;

            Console.WriteLine("Count() on collection: {0}", collectionCountTime);
            Console.WriteLine("Any() on collection: {0} (x{1:N1})", collectionAnyTime, anyVsCountCollectionRatio);
            Console.WriteLine("Any() on query: {0}", queryAnyTime);
            Console.WriteLine("Count() on query: {0} (x{1:N1})", queryCountTime, countVsAnyQueryRatio);
        }
    }
}
