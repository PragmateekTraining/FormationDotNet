using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelSamples
{
    public class ExceptionsSample : ISample
    {
        ConcurrentQueue<int> hasRun = new ConcurrentQueue<int>();

        void WithoutHandling(int i)
        {
            hasRun.Enqueue(i);
            throw new Exception(i.ToString());
        }

        void ProcessWithoutHandling()
        {
            Parallel.ForEach(Enumerable.Range(1, 100), i => WithoutHandling(i));
        }

        ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();

        void WithHandling(int i)
        {
            hasRun.Enqueue(i);
            try
            {
                throw new Exception(i.ToString());
            }
            catch (Exception e)
            {
                exceptions.Enqueue(e);
            }
        }

        void ProcessWithHandling()
        {
            Parallel.ForEach(Enumerable.Range(1, 100), i => WithHandling(i));

            if (exceptions.Count != 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        string Stringify<T>(IEnumerable<T> data)
        {
            return data.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i + ","), sb => sb.Remove(sb.Length - 1, 1).ToString());
        }

        public void Run()
        {
            try
            {
                ProcessWithoutHandling();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(Stringify(hasRun));

            Console.WriteLine(new string('=', 10));

            hasRun = new ConcurrentQueue<int>();

            try
            {
                ProcessWithHandling();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(Stringify(hasRun));
        }
    }
}
