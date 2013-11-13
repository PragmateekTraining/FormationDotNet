using SamplesAPI;
using System;
using System.Reactive.Disposables;
using System.Threading;

namespace DisposePatternSamples
{
    public class DisposablesSample : ISample
    {
        public void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.WriteLine("Before: cancellation is{0} requested.", cts.IsCancellationRequested ? "" : " not");
            using (CancellationDisposable cancel = new CancellationDisposable(cts)) ;
            Console.WriteLine("After: cancellation is{0} requested.", cts.IsCancellationRequested ? "" : " not");

            Console.WriteLine("===");

            Console.WriteLine("Before using.");
            using (Disposable.Create(() => Console.WriteLine("Disposed!"))) ;
            Console.WriteLine("After using.");

            Console.WriteLine("===");

            Console.WriteLine("Before using.");
            using (new CompositeDisposable
                    (
                        Disposable.Create(() => Console.WriteLine("First disposed!")),
                        Disposable.Create(() => Console.WriteLine("Second disposed!")),
                        Disposable.Create(() => Console.WriteLine("Third disposed!"))
                    )) ;
            Console.WriteLine("After using.");

            Console.WriteLine("===");

            IDisposable resource = Disposable.Create(() => Console.WriteLine("Disposed by ref-count."));
            RefCountDisposable refCountDisposable = new RefCountDisposable(resource);
            using (refCountDisposable.GetDisposable())
            {
                using (refCountDisposable.GetDisposable())
                {
                    using (refCountDisposable.GetDisposable())
                    {
                        Console.WriteLine("Before dispose.");
                        refCountDisposable.Dispose();
                        Console.WriteLine("After dispose.");
                    }
                }

                Console.WriteLine("Going to dispose last reference.");
            }
            Console.WriteLine("After last reference disposed.");
        }
    }
}
