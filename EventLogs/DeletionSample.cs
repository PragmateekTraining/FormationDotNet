using System;
using SamplesAPI;
using System.Diagnostics;
using System.Security;

namespace EventLogs
{
    public class DeletionSample : ISample
    {
        private string sourceName;

        public DeletionSample(string sourceName)
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

            try
            {
                EventLog.DeleteEventSource(sourceName);
            }
            catch (SecurityException)
            {
                Console.Error.WriteLine("To delete the source you must run this program as administrator!");
                return;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Caught an unexpected exception: {0}!", e);
                return;
            }

            Console.WriteLine("Source '{0}' deleted.", sourceName);
        }
    }
}
