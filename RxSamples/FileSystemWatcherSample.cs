using SamplesAPI;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace RxSamples
{
    public class FileSystemWatcherSample : ISample
    {
        class HREvent
        {
            public string Department { get; set; }
            public string Employee { get; set; }
            public bool IsHired { get; set; }
        }

        IObservable<FileSystemEventArgs> ObserveFolder(string path)
        {
            FileSystemWatcher fsw = new FileSystemWatcher(path) { EnableRaisingEvents = true };

            fsw.Created += (s, a) =>
                {
                    Console.WriteLine("in!");
                };

            var sources = new[] 
                { 
                    Observable.FromEventPattern<FileSystemEventArgs>(fsw, "Created"), 
                    Observable.FromEventPattern<FileSystemEventArgs>(fsw, "Changed"), 
                    Observable.FromEventPattern<FileSystemEventArgs>(fsw, "Renamed"), 
                    Observable.FromEventPattern<FileSystemEventArgs>(fsw, "Deleted"),
                };

            return sources.Merge()
                          .SelectMany(e =>
                          {
                              if (e.EventArgs.ChangeType == WatcherChangeTypes.Created && Directory.Exists(e.EventArgs.FullPath))
                              {
                                  Console.WriteLine("Watching new directory '{0}'.", e.EventArgs.FullPath);
                                  return ObserveFolder(e.EventArgs.FullPath);
                              }
                              else
                              {
                                  return Observable.Return(e).Select(o => o.EventArgs);
                              }
                          });
        }

        public void Run()
        {
            const string rootDir = "root";

            if (Directory.Exists(rootDir))
            {
                Directory.Delete(rootDir, true);
            }

            Directory.CreateDirectory(rootDir);

            IObservable<FileSystemEventArgs> fsEvents = ObserveFolder(rootDir);

            fsEvents.Subscribe(e => Console.WriteLine("Something happen with '{0}'.", e.FullPath));

            Thread t = new Thread(() =>
                {

                    //Thread.Sleep(1000);

                    Directory.CreateDirectory("root/a");
                    Directory.CreateDirectory("root/a/a");
                    Directory.CreateDirectory("root/a/a/a");
                    Directory.CreateDirectory("root/b");
                    Directory.CreateDirectory("root/b/a");
                }) { IsBackground = true };
            t.Start();
            t.Join();

            Console.WriteLine("Press enter to stop watching...");

            Console.ReadLine();
        }
    }
}
