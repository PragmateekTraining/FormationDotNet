using SamplesAPI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingSamples
{
    public class BeginEndInvokeSample : ISample
    {
        int Add(int a, int b)
        {
            Console.WriteLine("Add invoked on {0} thread {1}.", Thread.CurrentThread.IsThreadPoolThread ? "thread-pool" : "", Thread.CurrentThread.ManagedThreadId);

            return a + b;
        }

        void Callback(IAsyncResult handle)
        {
            Console.WriteLine("Callback invoked on {0} thread {1}.", Thread.CurrentThread.IsThreadPoolThread ? "thread-pool" : "", Thread.CurrentThread.ManagedThreadId);

            int sum = (handle.AsyncState as Func<int, int, int>).EndInvoke(handle);

            Console.WriteLine("Sum is {0}.", sum);
        }

        public void Run()
        {
            Func<int, int, int> add = Add;
            IAsyncResult handle = add.BeginInvoke(1, 2, null, null);
            int sum = add.EndInvoke(handle);

            Console.WriteLine("===");

            add.BeginInvoke(1, 2, Callback, add);

            Thread.Sleep(100);

            Console.WriteLine("===");

            Task<int> addTask = Task.Factory.StartNew(() => Add(1, 2));
            int sumTask = addTask.Result;
        }
    }
}
