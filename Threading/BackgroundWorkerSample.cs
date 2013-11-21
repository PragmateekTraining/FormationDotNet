using SamplesAPI;
using System;
using System.ComponentModel;
using System.Threading;

namespace ThreadingSamples
{
    public class BackgroundWorkerSample : ISample
    {
        public void Run()
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += Frame;

            worker.ProgressChanged += (object sender, ProgressChangedEventArgs args) => Console.Write("\r{0}%", args.ProgressPercentage);

            worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs args) => Console.WriteLine("\r{0}", !args.Cancelled ? args.Result : "---");

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
            Console.WriteLine("Task is{0} running on a pool's thread.", Thread.CurrentThread.IsThreadPoolThread ? "" : " not");

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
