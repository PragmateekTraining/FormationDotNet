using System;
using SamplesAPI;
using System.Diagnostics;

namespace EventLogs
{
    public class LoggingSample : ISample
    {
        private string sourceName;

        public LoggingSample(string sourceName)
        {
            this.sourceName = sourceName;
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("Enter some message: ");

                string message = Console.ReadLine();

                if (message == "") break;

                EventLog.WriteEntry(sourceName, message, EventLogEntryType.Error, 1);
            }
        }
    }
}
