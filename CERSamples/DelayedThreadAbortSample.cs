using SamplesAPI;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace CERSamples
{
    public class DelayedThreadAbortSample : ISample
    {
        bool withoutCERThreadHasRunCatch;
        bool withCERThreadHasRunCatch;

        bool withoutCERThreadAborted;
        bool withCERThreadAborted;

        void WithoutCERThread()
        {
            try
            {
                throw new Exception();
            }
            catch
            {
                while (!withoutCERThreadAborted) Thread.Sleep(1);
                withoutCERThreadHasRunCatch = true;
            }
        }

        void WithCERThread()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                throw new Exception();
            }
            catch
            {
                while (!withCERThreadAborted) Thread.Sleep(1) ;
                withCERThreadHasRunCatch = true;
            }
        }

        public void Run()
        {
            Thread withoutCERThread = new Thread(WithoutCERThread) { IsBackground = true };
            Thread withCERThread = new Thread(WithCERThread) { IsBackground = true };

            withoutCERThread.Start();
            withCERThread.Start();

            Thread.Sleep(1000);

            withoutCERThread.Abort();
            withCERThread.Abort();

            Console.WriteLine("after abort");

            withoutCERThreadAborted = true;
            withCERThreadAborted = true;

            withoutCERThread.Join();
            withCERThread.Join();

            Console.WriteLine(withoutCERThreadHasRunCatch);
            Console.WriteLine(withCERThreadHasRunCatch);
        }
    }
}
