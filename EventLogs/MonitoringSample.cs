using System;
using SamplesAPI;
using System.Diagnostics;

namespace EventLogs
{
    public class MonitoringSample : ISample
    {
        private string sourceName;

        public MonitoringSample(string sourceName)
        {
            this.sourceName = sourceName;
        }

        public void Run()
        {
            if (!EventLog.SourceExists(sourceName))
            {
                Console.Error.WriteLine("Source '{0}' does not exist!", sourceName);
                return;
            }

            EventLog log = new EventLog("Application", ".", sourceName);
            log.EnableRaisingEvents = true;
            log.EntryWritten += (s, a) =>
            {
                Console.WriteLine("New entry: '{0}'.", a.Entry.ReplacementStrings[0]);
            };

            Console.WriteLine("Press enter to stop monitoring.");
            Console.ReadLine();
            Console.Write("Monitoring stopped.");

            return;
        }
    }
}
