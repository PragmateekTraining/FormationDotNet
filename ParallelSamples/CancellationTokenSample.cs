using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSamples
{
    public class CancellationTokenSample : ISample
    {
        ConcurrentQueue<int> hasRun = new ConcurrentQueue<int>();

        void Trigger(CancellationTokenSource cts)
        {
            Console.WriteLine("Press enter to stop...");
            Console.ReadLine();

            cts.Cancel();
        }

        void F(int i)
        {
            hasRun.Enqueue(i);
            Thread.Sleep(1000);
        }

        public void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task.Run(() => Trigger(cts));

            ParallelLoopResult result = default(ParallelLoopResult);
            try
            {
                result = Parallel.ForEach(Enumerable.Range(1, 100), new ParallelOptions { CancellationToken = cts.Token }, i => F(i));
                Console.WriteLine("Parallel.ForEach has run to completion.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Parallel.ForEach has been cancelled.");
            }

            Console.WriteLine("Parallel.ForEach has{0} been completed.", result.IsCompleted ? "" : " not");

            Console.WriteLine("After Parallel.ForEach.");
            Console.WriteLine(hasRun.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i + ","), sb => sb.Remove(sb.Length - 1, 1).ToString()));
        }
    }
}
