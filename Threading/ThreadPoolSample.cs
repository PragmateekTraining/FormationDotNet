using SamplesAPI;
using System;
using System.Threading;

namespace ThreadingSamples
{
    public class ThreadPoolSample : ISample
    {
        void F(object o)
        {
            if (Thread.CurrentThread.IsThreadPoolThread)
            {
                Console.WriteLine("F printing '{0}' from thread-pool.", o);
            }
        }

        void F(object o, bool timedOut)
        {
            F(o);
        }

        public void Run()
        {
            Console.WriteLine("Before QueueUserWorkItem.");
            ThreadPool.QueueUserWorkItem(F, "Hello!");
            Console.WriteLine("After QueueUserWorkItem.");

            EventWaitHandle waitHandle = new AutoResetEvent(false);
            ThreadPool.RegisterWaitForSingleObject(waitHandle, F, "Signaled!", Timeout.Infinite, true);

            Console.WriteLine("Press enter to signal...");
            Console.ReadLine();
            waitHandle.Set();
            Console.WriteLine("Set!");

            Console.WriteLine("Press enter to end...");
            Console.ReadLine();
        }
    }
}
