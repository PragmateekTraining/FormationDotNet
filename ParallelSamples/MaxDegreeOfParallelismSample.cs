using SamplesAPI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelSamples
{
    public class MaxDegreeOfParallelismSample : ISample
    {
        volatile int nTasks = 0;

        void Monitor()
        {
            while (true)
            {
                Console.Write("\r{0} tasks running.", nTasks);
                Thread.Sleep(100);
            }
        }

        void F(int i)
        {
            Interlocked.Increment(ref nTasks);
            Thread.Sleep(1000);
            Interlocked.Decrement(ref nTasks);
        }

        public void Run()
        {
            Task.Run((Action)Monitor);

            Parallel.ForEach(Enumerable.Range(1, 100), i => F(i));

            Console.WriteLine("\n" + new string('=', 10));

            Parallel.ForEach(Enumerable.Range(1, 100), new ParallelOptions { MaxDegreeOfParallelism = 3 }, i => F(i));
        }
    }
}
