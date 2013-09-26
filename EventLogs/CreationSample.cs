using System;
using SamplesAPI;
using System.Diagnostics;
using System.Security;

namespace EventLogs
{
    public class CreationSample : ISample
    {
        private string sourceName;

        public CreationSample(string sourceName)
        {
            this.sourceName = sourceName;
        }

        public void Run()
        {
            if (EventLog.SourceExists(sourceName))
            {
                Console.Error.WriteLine("Source '{0}' already exists!", sourceName);
                return;
            }

            try
            {
                EventLog.CreateEventSource(sourceName, string.Format("Creating new source for '{0}'...", sourceName));
            }
            catch (SecurityException)
            {
                Console.Error.WriteLine("To create the source you must run this program as administrator!");
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Caught an unexpected exception: {0}!", e);
                return;
            }

            Console.WriteLine("Source '{0}' created.", sourceName);

            return;
        }
    }
}
