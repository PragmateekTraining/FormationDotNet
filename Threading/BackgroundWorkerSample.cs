using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading;

namespace ThreadingSamples
{
    public class BackgroundWorkerSample : ISample
    {
        private HashSet<int> usedThreads = new HashSet<int>();

        public void Run()
        {
            Console.WriteLine("Primary thread is n°{0}", Thread.CurrentThread.ManagedThreadId);

            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += Frame;

            worker.ProgressChanged += (object sender, ProgressChangedEventArgs args) =>
                {
                    Console.Write("\r{0}% [{1}]", args.ProgressPercentage, Thread.CurrentThread.ManagedThreadId);
                    usedThreads.Add(Thread.CurrentThread.ManagedThreadId);
                };

            worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs args) =>
                {
                    Console.WriteLine("\r{0} [{1}]", !args.Cancelled ? args.Result : "---", Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("Threads used for notification: {0}", usedThreads.Aggregate("", (a, id) => (a != "" ? ( a + ",") : "") + id));
                };

            Console.WriteLine("Press enter to stop...");

            worker.RunWorkerAsync("test");

            Console.ReadLine();

            if (worker.IsBusy)
            {
                worker.CancelAsync();

                Console.WriteLine("Worker cancelled.");
                Console.Write("Press enter to quit...");
                Console.ReadLine();
            }
        }

        private static void Frame(object sender, DoWorkEventArgs e)
        {
            if (Thread.CurrentThread.IsThreadPoolThread)
            {
                Console.WriteLine("Task is running on pool's thread n°{0}.", Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                Console.WriteLine("Task is not running on a pool's thread.");
            }

            for (int p = 0; p <= 100 && !e.Cancel; ++p)
            {
                Thread.Sleep(50);

                (sender as BackgroundWorker).ReportProgress(p);

                if ((sender as BackgroundWorker).CancellationPending) e.Cancel = true;
            }

            if (!e.Cancel) e.Result = string.Format("|{0}|", e.Argument as string);
        }
    }
}
