using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DisposePatternSamples
{
    public class ConsoleColorSample : ISample
    {
        void Task()
        {
            const int n = 1000;

            Random rand = new Random();

            for (int i = 1; i <= n; ++i)
            {
                using (rand.Next(2) == 0 ? Color.Cyan : Color.Green) ;
            }
        }

        void TaskSafe()
        {
            const int n = 1000;

            Random rand = new Random();

            for (int i = 1; i <= n; ++i)
            {
                using (rand.Next(2) == 0 ? new Color(ConsoleColor.Cyan, true) : new Color(ConsoleColor.Green, true)) ;
            }
        }

        public void Run()
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            const int n = 4;

            Thread[] threads = new Thread[n];

            for (int i = 0; i < n; ++i)
            {
                (threads[i] = new Thread(Task) { IsBackground = true }).Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("===");

            Console.ForegroundColor = originalColor;

            for (int i = 0; i < n; ++i)
            {
                (threads[i] = new Thread(TaskSafe) { IsBackground = true }).Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("===");
        }
    }
}
