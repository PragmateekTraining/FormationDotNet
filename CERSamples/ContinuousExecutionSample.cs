using SamplesAPI;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CERSamples
{
    public class ContinuousExecutionSample : ISample
    {
        bool withoutCERRun;
        bool withCERRun;

        /* void WithoutCER()
        {
            Thread.Sleep(Timeout.Infinite);
            withoutCERRun = true;
        }

        void WithCER()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try { }
            finally
            {
                Thread.Sleep(Timeout.Infinite);
                withCERRun = true;
            }
        }*/

        void WithoutCER()
        {
            try { }
            finally
            {
                for (int i = 0; i >= 0; ++i) ;
                withoutCERRun = true;
            }
        }

        void WithCER()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try { }
            finally
            {
                for (int i = 0; i >= 0; ++i) ;
                withCERRun = true;
            }
        }

        public void Run()
        {
            Thread withoutCERThread = new Thread(WithoutCER) { IsBackground = true };
            Thread withCERThread = new Thread(WithCER) { IsBackground = true };

            withoutCERThread.Start();
            withCERThread.Start();

            Thread.Sleep(1000);

            Console.WriteLine("aborting");

            withoutCERThread.Abort();
            withCERThread.Abort();

            Console.WriteLine(withoutCERRun);
            Console.WriteLine(withCERRun);
        }
    }
}
