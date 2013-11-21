using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PLinqSamples
{
    public class WebClientSample : ISample
    {
        public void Run()
        {
            string[] urls = Enumerable.Repeat("http://google.com", 32).ToArray();
            /*{
                "http://google.com",
                "http://yahoo.com",
                "http://microsoft.com",
                "http://wikipedia.com",
                "http://cnn.com",
                "http://facebook.com",
                "http://youtube.com",
                "http://twitter.com"
            };*/

            /*Task.Run(() =>
                {
                    Console.WriteLine("In");
                    while (true)
                    {
                        Console.WriteLine("In");
                        int wt, cpt;
                        ThreadPool.GetAvailableThreads(out wt, out cpt);
                        Console.WriteLine("{0} / {1}", wt, cpt);
                        Console.WriteLine("In");
                        Thread.Sleep(100);
                    }
                });*/

            WebClient webClient = new WebClient();
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (string url in urls)
            {
                webClient.DownloadString(url);
                Console.WriteLine("Got '{0}'", url);
            }
            stopwatch.Stop();

            TimeSpan sequentialTime = stopwatch.Elapsed;

            stopwatch.Restart();
            CountdownEvent cde = new CountdownEvent(1);
            foreach (string url in urls)
            {
                cde.AddCount();
                webClient = new WebClient();
                webClient.DownloadStringCompleted += (_, __) =>
                {
                    Console.WriteLine("Got '{0}'", __.UserState);
                    cde.Signal();
                };
                webClient.DownloadStringAsync(new Uri(url), url);
            }
            cde.Signal();
            cde.Wait();
            stopwatch.Stop();

            TimeSpan asyncTime = stopwatch.Elapsed;

            stopwatch.Restart();
            ThreadLocal<WebClient> threadWebClient = new ThreadLocal<WebClient>(() => new WebClient());
            urls.AsParallel().WithDegreeOfParallelism(urls.Length).ForAll(url => threadWebClient.Value.DownloadString(url));
            stopwatch.Stop();

            TimeSpan PLinqTime = stopwatch.Elapsed;

            Console.WriteLine("Sequential time: {0}.", sequentialTime);
            Console.WriteLine("PLinq time: {0}.", PLinqTime);
            Console.WriteLine("Async time: {0}.", asyncTime);
        }
    }
}
