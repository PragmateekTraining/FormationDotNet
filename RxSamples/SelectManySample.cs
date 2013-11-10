using DrWPF.Windows.Data;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace RxSamples
{
    public class SelectManySample : ISample
    {
        public void Run()
        {
            /* Observable.Interval(TimeSpan.FromSeconds(1))
                      .SelectMany(i => Observable.Interval(TimeSpan.FromSeconds(1))
                                                 .Select(j => (i + 1) * (j + 1)))
                      .Distinct()
                      .Subscribe(e => Console.WriteLine(e));

            Console.ReadLine();*/

            ObservableDictionary<char, ObservableCollection<string>> words = new ObservableDictionary<char, ObservableCollection<string>>();

            IDisposable wordsSubscription = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(words, "CollectionChanged")
                                                      .SelectMany(e =>
                                                          {
                                                              KeyValuePair<char, ObservableCollection<string>> pair = (KeyValuePair<char, ObservableCollection<string>>)e.EventArgs.NewItems[0];

                                                              Console.WriteLine("New letter '{0}'.", pair.Key);

                                                              return Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(pair.Value, "CollectionChanged");
                                                          })
                                                      .Select(e => e.EventArgs.NewItems[0] as string)
                                                      .Subscribe(w => Console.WriteLine("New word '{0}'.", w), () => Console.WriteLine("Done."));

            using (wordsSubscription)
            {
                while (true)
                {
                    Console.Write("Word? ");

                    string word = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(word)) break;

                    char letter = word[0];

                    if (!words.ContainsKey(letter)) words[letter] = new ObservableCollection<string>();

                    if (!words[letter].Contains(word)) words[letter].Add(word);
                }
            }
        }
    }
}
