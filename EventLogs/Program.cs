using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security;

namespace EventLogs
{
    class Program
    {
        const string SourceName = "NightlyBatch";

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0] == "--create")
                {
                    if (EventLog.SourceExists(SourceName))
                    {
                        Console.Error.WriteLine("Source '{0}' already exists!", SourceName);
                        return;
                    }

                    try
                    {
                        EventLog.CreateEventSource(SourceName, string.Format("Creating new source for '{0}'...", SourceName));
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

                    Console.WriteLine("Source '{0}' created.", SourceName);

                    return;
                }
                else if (args[0] == "--delete")
                {
                    if (!EventLog.SourceExists(SourceName))
                    {
                        Console.Error.WriteLine("Source '{0}' does not exist!", SourceName);
                        return;
                    }

                    try
                    {
                        EventLog.DeleteEventSource(SourceName);
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

                    Console.WriteLine("Source '{0}' deleted.", SourceName);

                    return;
                }
                else if (args[0] == "--monitor")
                {
                    if (!EventLog.SourceExists(SourceName))
                    {
                        Console.Error.WriteLine("Source '{0}' does not exist!", SourceName);
                        return;
                    }

                    EventLog log = new EventLog("Application", ".", SourceName);
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

            while (true)
            {
                Console.Write("Enter some message: ");

                string message = Console.ReadLine();

                if (message == "") break;

                EventLog.WriteEntry(SourceName, message, EventLogEntryType.Error, 1);
            }
        }
    }
}
