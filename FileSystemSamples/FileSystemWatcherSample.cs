using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace FileSystemSamples
{
    public class FileSystemWatcherSample : ISample
    {
        /*readonly string directory;
        readonly string filePattern;

        public FileSystemWatcherSample(string directory, string filePattern)
        {
            this.directory = directory;
            this.filePattern = filePattern;
        }*/

        IDictionary<string, DateTime> lastOperationTimes = new ConcurrentDictionary<string, DateTime>();

        public void Run()
        {
            Console.Write("Directory: ");
            string directory = Console.ReadLine();
            Console.Write("Files: ");
            string filePattern = Console.ReadLine();
            Console.Write("Filter? ");
            bool filter = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("Watching files matching '{0}' pattern in directory '{1}'...", filePattern, directory);

            FileSystemWatcher watcher = new FileSystemWatcher(directory, filePattern) { EnableRaisingEvents = true };

            watcher.Created += (s, a) =>
                {
                    if (filter)
                    {
                        lastOperationTimes[a.FullPath] = File.GetLastWriteTime(a.FullPath);
                    }

                    Console.WriteLine("'{0}' has been created.", a.Name);
                };

            watcher.Changed += (s, a) =>
                {
                    bool notify = !filter;

                    if (filter)
                    {
                        if (!lastOperationTimes.ContainsKey(a.FullPath))
                        {
                            lastOperationTimes[a.FullPath] = DateTime.MinValue;
                        }

                        DateTime lastWriteTime = File.GetLastWriteTime(a.FullPath);

                        // Console.WriteLine("{0} vs {1}", lastOperationTimes[a.FullPath].ToString("o"), lastWriteTime.ToString("o"));

                        if ((lastWriteTime - lastOperationTimes[a.FullPath]).TotalMilliseconds > 10)
                        {
                            lastOperationTimes[a.FullPath] = lastWriteTime;
                            notify = true;
                        }
                    }

                    if (notify)
                    {
                        Console.WriteLine("'{0}' has been changed.", a.Name);
                    }
                };

            watcher.Renamed += (s, a) => Console.WriteLine("'{0}' has been renamed to '{1}'.", a.OldName, a.Name);

            watcher.Deleted += (s, a) => Console.WriteLine("'{0}' has been deleted.", a.Name);

            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
        }
    }
}
