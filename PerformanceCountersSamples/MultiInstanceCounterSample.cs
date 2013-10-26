using SamplesAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace PerformanceCountersSamples
{
    public class MultiInstanceCounterSample : ISample
    {
        const string categoryName = "Multi-instance logger";
        const string totalLogsCounterName = "# total logs";

        class Logger
        {
            PerformanceCounter totalLogsCounter;

            public Logger(string appName)
            {
                totalLogsCounter = new PerformanceCounter(categoryName, totalLogsCounterName, string.Format("{0} ({1})", appName, Process.GetCurrentProcess().Id), false);
                totalLogsCounter.RawValue = 0;
            }

            public void Log(string message)
            {
                totalLogsCounter.Increment();
            }
        }

       string appName = null;

        public MultiInstanceCounterSample(string appName)
        {
            this.appName = appName;
        }

        public void Run()
        {
            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                Console.WriteLine("Creating category '{0}'...", categoryName);
                CounterCreationDataCollection counters = new CounterCreationDataCollection();
                counters.Add(new CounterCreationData(totalLogsCounterName, "The total number of logs from the dawn of time.", PerformanceCounterType.NumberOfItems64));

                PerformanceCounterCategory.Create(categoryName, "Performance counters for the logging system.", PerformanceCounterCategoryType.MultiInstance, counters);
                Console.WriteLine("Category '{0}' created.", categoryName);
            }

            Logger logger = new Logger(appName);

            Random rand = new Random();

            Console.WriteLine("Starting to log for app '{0}'...", appName);
            while (true)
            {
                logger.Log("");
                Thread.Sleep(rand.Next(200));
            }
        }
    }
}
