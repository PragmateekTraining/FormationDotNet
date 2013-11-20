using SamplesAPI;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace RxSamples
{
    public class ThreadingSample : ISample
    {
        void DumpCurrentThread(string label)
        {
            Console.WriteLine("{0} on thread#{1}.", label, Thread.CurrentThread.ManagedThreadId);
        }

        public void Run()
        {
            DumpCurrentThread("Run");

            Observable.Create<int>(o =>
                {
                    DumpCurrentThread("Subscription");
                    o.OnNext(1);
                    return () => { };
                })
                .Subscribe(i => DumpCurrentThread("Notification"));

            Console.WriteLine("After create.");
            Console.ReadLine();

            Observable.Create<int>(o =>
            {
                DumpCurrentThread("Subscription");
                o.OnNext(1);
                return () => { };
            })
            .SubscribeOn(ThreadPoolScheduler.Instance)
            .Subscribe(i => DumpCurrentThread("Notification"));

            Console.WriteLine("After create.");
            Console.ReadLine();

            Observable.Create<int>(o =>
            {
                DumpCurrentThread("Subscription");
                o.OnNext(1);
                return () => { };
            })
            .SubscribeOn(ThreadPoolScheduler.Instance)
            .ObserveOn(ThreadPoolScheduler.Instance)
            .Subscribe(i => DumpCurrentThread("Notification"));

            Console.WriteLine("After create.");
            Console.ReadLine();
        }
    }
}
