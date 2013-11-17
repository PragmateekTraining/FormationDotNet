using SamplesAPI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLinqSamples
{
    public class BrokenPipelineSample : ISample
    {
        public void Run()
        {
            ParallelQuery<int> query = ParallelEnumerable.Range(1, 10).Select(i => { Thread.Sleep(100); return i; });

            Console.WriteLine("==========");

            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.ForEach(query, Console.WriteLine);
            stopwatch.Stop();
            TimeSpan forEachTime = stopwatch.Elapsed;

            Console.WriteLine("==========");

            stopwatch.Restart();
            query.ForAll(Console.WriteLine);
            stopwatch.Stop();
            TimeSpan forAllTime = stopwatch.Elapsed;

            Console.WriteLine("ForEach: {0}.", forEachTime);
            Console.WriteLine("ForAll: {0}.", forAllTime);
        }
    }
}
