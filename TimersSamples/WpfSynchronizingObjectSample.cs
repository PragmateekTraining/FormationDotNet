using System;
using System.ComponentModel;
using System.Threading;
using timers = System.Timers;
using System.Windows;
using System.Windows.Threading;
using SamplesAPI;

namespace TimersSamples
{
    public class WpfSynchronizingObjectSample : ISample
    {
        class DispatcherISyncInvoke : ISynchronizeInvoke
        {
            private class DispatcherOperationAsync : IAsyncResult, IDisposable
            {
                private readonly DispatcherOperation dispatcherOperation;
                private ManualResetEvent waitHandle = new ManualResetEvent(false);

                public DispatcherOperationAsync(DispatcherOperation dispatcherOperation)
                {
                    this.dispatcherOperation = dispatcherOperation;
                    dispatcherOperation.Completed += (s, a) => waitHandle.Set();
                }

                public object Result { get { return dispatcherOperation.Result; } }

                public bool IsCompleted { get { return dispatcherOperation.Status == DispatcherOperationStatus.Completed; } }

                public WaitHandle AsyncWaitHandle { get { return waitHandle; } }

                public object AsyncState { get { return null; } }

                public bool CompletedSynchronously { get { return false; } }

                public void Dispose()
                {
                    waitHandle.Dispose();
                    waitHandle = null;
                }
            }

            private readonly Dispatcher dispatcher;

            public DispatcherISyncInvoke(Dispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            public object Invoke(Delegate method, object[] args)
            {
                return InvokeRequired ? EndInvoke(BeginInvoke(method, args)) : method.DynamicInvoke(args);
            }

            public IAsyncResult BeginInvoke(Delegate method, object[] args)
            {
                return new DispatcherOperationAsync(dispatcher.BeginInvoke(method, args));
            }

            public object EndInvoke(IAsyncResult result)
            {
                result.AsyncWaitHandle.WaitOne();

                return ((DispatcherOperationAsync)result).Result;
            }

            public bool InvokeRequired { get { return !dispatcher.CheckAccess(); } }
        }

        public void Run()
        {
            Application app = new Application();

            timers.Timer timer = new timers.Timer(1000);
            timer.Elapsed += (s, a) =>
            {
                Console.WriteLine(DateTime.Now.ToString("o"));
                Console.Write("Press enter to continue...");
                Console.ReadLine();
            };
            timer.SynchronizingObject = new DispatcherISyncInvoke(app.Dispatcher);
            timer.Start();

            app.Run();
        }
    }
}
