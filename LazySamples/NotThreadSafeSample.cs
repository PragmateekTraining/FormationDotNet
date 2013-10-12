using System;
using System.Threading;
using SamplesAPI;

namespace LazySamples
{
    public class NotThreadSafeSample : ISample
    {
        static object ObjectFactory()
        {
            Thread.Sleep(100);
            return new object();
        }

        Lazy<object> lazy = new Lazy<object>(ObjectFactory, false);

        public void Run()
        {
            Thread process = new Thread(() => { object o = lazy.Value; }) { IsBackground = true };

            process.Start();

            Thread.Sleep(10);

            // Will throw an exception because value factory is running on another thread
            object value = lazy.Value;
        }
    }
}
