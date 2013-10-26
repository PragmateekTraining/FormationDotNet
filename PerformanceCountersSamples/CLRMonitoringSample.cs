using SamplesAPI;
using System;
using System.Threading;

namespace PerformanceCountersSamples
{
    public class CLRMonitoringSample : ISample
    {
        object @lock = new object();

        int n = 0;

        void Processing()
        {
            while (true)
            {
                lock (@lock)
                {
                    ++n;
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("> ");
                string choice = Console.ReadLine();

                if (choice == "new" || choice == "n") new Thread(Processing) { IsBackground = true }.Start();
                else if (choice == "quit" || choice == "q") break;
            }
        }
    }
}
