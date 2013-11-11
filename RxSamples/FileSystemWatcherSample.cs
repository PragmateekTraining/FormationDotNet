using SamplesAPI;
using System;
using System.IO;
using System.Linq;
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

            /*foreach (string subDir in Directory.GetDirectories(path))
            {
                ObserveFolder(subDir);
            }*/

            /* fsw.Created += (s, a) =>
                {
                    Console.WriteLine("in!");
                };*/

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
            //.Merge(Directory.GetDirectories(path).Select(subDir => ObserveFolder(subDir)).Merge());
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

            fsEvents.Subscribe(e =>
                {
                    using (e.ChangeType == WatcherChangeTypes.Created ? Color.Green :
                           e.ChangeType == WatcherChangeTypes.Changed ? Color.Cyan :
                           e.ChangeType == WatcherChangeTypes.Deleted ? Color.Red :
                           Color.Gray)
                    {
                        Console.WriteLine("'{0}' has been '{1}'.", e.FullPath, e.ChangeType);
                    }
                });

            Thread t = new Thread(() =>
                {
                    Directory.CreateDirectory("root/a");
                    Thread.Sleep(1000);
                    Directory.CreateDirectory("root/a/a");
                    Thread.Sleep(1000);
                    Directory.CreateDirectory("root/a/a/a");
                    Thread.Sleep(1000);
                    Directory.CreateDirectory("root/b");
                    Thread.Sleep(1000);
                    Directory.CreateDirectory("root/b/a");
                    Thread.Sleep(1000);
                    using (File.Create("root/b/a/a.txt")) { };
                    Thread.Sleep(1000);
                    File.Delete("root/b/a/a.txt");
                }) { IsBackground = true };
            t.Start();
            t.Join();

            Console.WriteLine("Press enter to stop watching...");

            Console.ReadLine();
        }
    }
}
