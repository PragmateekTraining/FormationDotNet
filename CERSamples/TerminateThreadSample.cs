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

        uint withoutCERThreadID;
        uint withCERThreadID;

        bool withoutCERThreadHasRunFinally;
        bool withCERThreadHasRunFinally;

        void WithoutCERThread()
        {
            withoutCERThreadID = GetCurrentThreadId();
            
            try
            {
                for (int i = 0; i >= 0; ++i) ;
            }
            finally
            {
                withoutCERThreadHasRunFinally = true;
            }
        }

        void WithCERThread()
        {
            withCERThreadID = GetCurrentThreadId();
            
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                for (int i = 0; i >= 0; ++i) ;
            }
            finally
            {
                withCERThreadHasRunFinally = true;
            }
        }

        public void Run()
        {
            Thread withoutCERThread = new Thread(WithoutCERThread) { IsBackground = true };
            Thread withCERThread = new Thread(WithCERThread) { IsBackground = true };

            withoutCERThread.Start();
            withCERThread.Start();

            Thread.Sleep(1000);

            Console.WriteLine(withoutCERThreadID);
            Console.WriteLine(withCERThreadID);

            withoutCERThread.Abort();
            withCERThread.Abort();

            /* TerminateThread(withoutCERThreadID, 1);
            TerminateThread(withCERThreadID, 1);*/

            Console.WriteLine(withoutCERThreadHasRunFinally);
            Console.WriteLine(withCERThreadHasRunFinally);
        }
    }
}
