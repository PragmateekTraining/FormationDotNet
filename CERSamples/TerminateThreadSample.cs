using SamplesAPI;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace CERSamples
{
    public class TerminateThreadSample : ISample
    {
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        static extern bool TerminateThread(uint hThread, uint dwExitCode);

        [DllImport("native.dll")]
        static extern void spin();

        // CountdownEvent countDown = new CountdownEvent(2);

        uint withoutCERThreadID;
        uint withCERThreadID;

        bool withoutCERThreadHasRunFinally;
        bool withCERThreadHasRunFinally;

        void WithoutCERThread()
        {
            withoutCERThreadID = GetCurrentThreadId();
            
            try
            {
                // while (true) ;
                for (int i = 0; i >= 0; ++i) ;
                // Console.WriteLine("WithoutCERThread try");
                // countDown.Signal();

                //spin();
                // Thread.Sleep(1000);
                // Console.WriteLine("WithoutCERThread after sleep");
            }
            finally
            {
                // Console.WriteLine("WithoutCERThread finally");
                withoutCERThreadHasRunFinally = true;
            }
        }

        void WithCERThread()
        {
            withCERThreadID = GetCurrentThreadId();
            
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                // while (true) ;
                for (int i = 0; i >= 0; ++i) ;
                // Console.WriteLine("WithCERThread try");
                // countDown.Signal();
                // Thread.Sleep(1000);
                //spin();
                // Console.WriteLine("WithCERThread try");
            }
            finally
            {
                // Console.WriteLine("WithCERThread finally");
                withCERThreadHasRunFinally = true;
            }
        }

        public void Run()
        {
            Thread withoutCERThread = new Thread(WithoutCERThread) { IsBackground = true };
            Thread withCERThread = new Thread(WithCERThread) { IsBackground = true };

            withoutCERThread.Start();
            withCERThread.Start();

            Console.WriteLine("Before wait");
            // countDown.Wait();
            Console.WriteLine("After wait");

            Thread.Sleep(1000);

            Console.WriteLine(withoutCERThreadID);
            Console.WriteLine(withCERThreadID);

            // withoutCERThread.Abort();
            // withCERThread.Abort();

            TerminateThread(withoutCERThreadID, 1);
            TerminateThread(withCERThreadID, 1);

            Console.WriteLine(withoutCERThreadHasRunFinally);
            Console.WriteLine(withCERThreadHasRunFinally);
        }
    }
}
