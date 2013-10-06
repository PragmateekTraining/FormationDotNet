using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Threading
{
    class Downloader
    {
        IList<string> GetLinks(string url)
        {
            WebClient wc = new WebClient();
            string page = wc.DownloadString(url);

            Regex regex = new Regex(string.Format(@"href=""({0}.+?)""", url));
            MatchCollection matches = regex.Matches(page);
            return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
        }

        IDictionary<string, string> DownloadAllLinkedPages(string url)
        {
            IDictionary<string, string> pages = new Dictionary<string, string>();

            IList<string> links = GetLinks(url);

            CountdownEvent countDown = new CountdownEvent(links.Count);

            foreach (string link in links)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    string localUrl = o as string;

                    WebClient webClient = new WebClient();

                    string page = webClient.DownloadString(localUrl);

                    lock (pages) pages[localUrl] = page;

                    countDown.Signal();
                }, link);
            }

            countDown.Wait();

            return pages;
        }

        internal void Run()
        {
            IDictionary<string, string> pages = DownloadAllLinkedPages("http://www.lemonde.fr");

            foreach (var pair in pages)
            {
                Console.WriteLine(pair.Key);
            }
        }
    }
}
