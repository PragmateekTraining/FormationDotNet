using SamplesAPI;
using System;
using System.Threading;

namespace ThreadingSamples
{
    public class MemoryVisibilitySample : ISample
    {
        volatile bool ok = false;

        void F()
        {
            Console.WriteLine();
            // int n = 0;
            // while (!ok) ++n;
            while (!ok) /*Thread.MemoryBarrier()*/;
        }

        public void Run()
        {
            Thread thread = new Thread(F);
            thread.Start();

            Console.Write("Press enter to notify thread...");
            Console.ReadLine();

            ok = true;

            Console.WriteLine("Thread notified.");
        }
    }
}
