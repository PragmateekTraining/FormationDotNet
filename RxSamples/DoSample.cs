using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RxSamples
{
    public class DoSample : ISample
    {
        IEnumerable<ConsoleKeyInfo> ConsoleKeys
        {
            get
            {
                while (true)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter) yield break;
                    else yield return key;
                }
            }
        }

        public void Run()
        {
            IObservable<ConsoleKeyInfo> keys = ConsoleKeys.ToObservable();

            int count = 0;
            keys.Do(k => ++count)
                .Select(e => e.KeyChar.ToString().ToUpper())
                .Subscribe(c => Console.Write(c), () => Console.WriteLine("\nDone with {0} strokes.", count));
        }
    }
}
