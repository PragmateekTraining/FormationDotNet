using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using timers = System.Timers;

namespace PerformanceCountersSamples
{
    public class LoggingCountersSample : ISample
    {
        enum Level
        {
            INFO,
            WARNING,
            ERROR
        }

        const string categoryName = "Logging counters";

        class Logger
        {
            PerformanceCounter totalLogsCounter;
            PerformanceCounter logsPerSecondCounter;
            PerformanceCounter totalErrorLogsCounter;
            PerformanceCounter errorLogsPer10sCounter;

            long oneSecondAgoTotal = 0;
            long oneSecondsAgoErrorTotal = 0;

            Queue<long> errorLogsQueue = new Queue<long>();

            timers.Timer timer = new timers.Timer(1000);

            void SetupPerformanceCounters()
            {
                const string totalLogsCounterName = "Total logs";
                const string logsPerSecondCounterName = "Logs per second";
                const string totalErrorLogsCounterName = "Total error logs";
                const string errorLogsPer10sCounterName = "Errors per 10s";

                if (!PerformanceCounterCategory.Exists(categoryName))
                {
                    CounterCreationDataCollection counters = new CounterCreationDataCollection();
                    counters.Add(new CounterCreationData(totalLogsCounterName, "The total number of logs from the dawn of time.", PerformanceCounterType.NumberOfItems64));
                    counters.Add(new CounterCreationData(logsPerSecondCounterName, "The number of logs currently processed per second.", PerformanceCounterType.NumberOfItems32));
                    counters.Add(new CounterCreationData(totalErrorLogsCounterName, "The total number of error logs from the dawn of time.", PerformanceCounterType.NumberOfItems64));
                    counters.Add(new CounterCreationData(errorLogsPer10sCounterName, "The number of error logs currently processed per 10s.", PerformanceCounterType.NumberOfItems32));

                    PerformanceCounterCategory.Create(categoryName, "Performance counters for the logging system.", PerformanceCounterCategoryType.SingleInstance, counters);
                }

                totalLogsCounter = new PerformanceCounter(categoryName, totalLogsCounterName, readOnly: false);
                logsPerSecondCounter = new PerformanceCounter(categoryName, logsPerSecondCounterName, readOnly: false);
                totalErrorLogsCounter = new PerformanceCounter(categoryName, totalErrorLogsCounterName, readOnly: false);
                errorLogsPer10sCounter = new PerformanceCounter(categoryName, errorLogsPer10sCounterName, readOnly: false);
            }

            void UpdateCounters(object sender, timers.ElapsedEventArgs args)
            {
                long currentTotal = totalLogsCounter.RawValue;
                logsPerSecondCounter.RawValue = currentTotal - oneSecondAgoTotal;
                oneSecondAgoTotal = currentTotal;

                long currentErrorTotal = totalErrorLogsCounter.RawValue;
                long errorLogsLastSecond = currentErrorTotal - oneSecondsAgoErrorTotal;

                errorLogsQueue.Enqueue(errorLogsLastSecond);
                if (errorLogsQueue.Count == 11) errorLogsQueue.Dequeue();
                errorLogsPer10sCounter.RawValue = errorLogsQueue.Sum();

                oneSecondsAgoErrorTotal = currentErrorTotal;
            }

            public Logger()
            {
                SetupPerformanceCounters();

                timer.Elapsed += UpdateCounters;
                timer.Start();
            }

            public void Log(string message, Level level)
            {
                totalLogsCounter.Increment();

                if (level == Level.ERROR)
                {
                    totalErrorLogsCounter.Increment();
                }
            }
        }

        private bool delete;

        public LoggingCountersSample(bool delete)
        {
            this.delete = delete;
        }

        public void Run()
        {
            if (delete && PerformanceCounterCategory.Exists(categoryName))
            {
                Console.WriteLine("Deleting category '{0}'...", categoryName);
                PerformanceCounterCategory.Delete(categoryName);
                Console.WriteLine("Category '{0}' deleted.", categoryName);
            }

            Logger logger = new Logger();

            Random rand = new Random();

            Console.WriteLine("Starting to log...");
            while (true)
            {
                logger.Log(string.Empty, rand.Next() % 5 == 0 ? Level.ERROR : Level.INFO);

                Thread.Sleep(rand.Next(200));
            }
        }
    }
}
