using SamplesAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace RxSamples
{
    public class SwitchSample : ISample
    {
        /*IEnumerable<string> GetInput()
        {
            StringBuilder buffer = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    yield break;
                }

                if (keyInfo.Key == ConsoleKey.Backspace && buffer.Length > 0)
                {
                    buffer.Remove(buffer.Length - 1, 1);
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    yield return buffer.ToString();
                }

                buffer.Append(keyInfo.KeyChar);
            }
        }*/

        IEnumerable<string> GetInput()
        {
            while (true)
            {
                Console.Write("Pattern? ");

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    yield break;
                }

                yield return input;
            }
        }

        void Dump(string s)
        {
            string toDump = s;

            if (s.Length > 75)
            {
                toDump = s.Substring(0, 30) + "..." + s.Substring(s.Length - 1 - 30, 30);
            }

            Console.Write("\r{0}", toDump);
        }

        IEnumerable<string> FindMatches(string directory, string pattern, CancellationToken cancel)
        {
            Regex regex = new Regex(pattern, RegexOptions.Compiled);

            IEnumerable<string> currentDirElements = null;
            try
            {
                currentDirElements = Directory.EnumerateFileSystemEntries(directory);
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }

            foreach (string name in currentDirElements)
            {
                if (cancel.IsCancellationRequested) break;

                Dump(name);

                if (regex.IsMatch(name)) yield return name;

                if (Directory.Exists(name))
                {
                    foreach (string match in FindMatches(name, pattern, cancel))
                    {
                        yield return match;
                    }
                }
            }

            yield break;
        }

        public void Run()
        {
            using (GetInput().ToObservable()
                      .Select(pattern =>
                          {
                              Console.WriteLine("\nSearching for '{0}'...", pattern);
                              CancellationTokenSource source = new CancellationTokenSource();
                              return FindMatches("/", pattern, source.Token).ToObservable(Scheduler.ThreadPool).Finally(() =>
                                  {
                                      Console.WriteLine("\nCancelling '{0}' research.", pattern);
                                      source.Cancel();
                                  });
                          })
                      .Switch()
                      .Subscribe(Console.WriteLine)) { }
        }
    }
}
