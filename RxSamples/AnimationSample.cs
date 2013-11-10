using SamplesAPI;
using System;
using System.Reactive.Linq;

namespace RxSamples
{
    public class AnimationSample : ISample
    {
        public void Run()
        {
            const string sprite = @"|/-\";

            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(200))
                      .Select(i => new string('=', (int)i) + "> " + sprite[(int)i % sprite.Length])
                      .Subscribe(c => Console.Write("\r{0}", c));

            Console.Write("Press enter to stop...");
            Console.ReadLine();
        }
    }
}
