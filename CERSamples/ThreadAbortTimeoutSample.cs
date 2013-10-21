using SamplesAPI;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace CERSamples
{
    public class ThreadAbortTimeoutSample : ISample
    {
        bool quickTerminationHasRun;
        bool aBitSlowTerminationHasRun;
        bool slowTerminationHasRun;
        bool slowWithCERTerminationHasRun;

        /// <summary>
        /// The number of increments that can be done in 1ms.
        /// </summary>
        double factor;

        /// <summary>
        /// Calibrate the spin wait to compute the above factor.
        /// </summary>
        void Calibrate()
        {
            const long n = 1000000000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (long i = 0; i < n; ++i) ;
            stopwatch.Stop();

            Console.WriteLine("Count done in {0}.", stopwatch.Elapsed);

            factor = n / stopwatch.Elapsed.TotalMilliseconds;

            Console.WriteLine("Factor is {0}.", factor);
        }

        /// <summary>
        /// Test the accuracy of the factor calibration.
        /// </summary>
        void Test()
        {
            // Wait during 1s.
            Stopwatch stopwatch = Stopwatch.StartNew();
            Spin(1000);
            stopwatch.Stop();

            Console.WriteLine("Spin 1s in {0}.", stopwatch.Elapsed);
        }

        /// <summary>
        /// Wait.
        /// </summary>
        /// <param name="ms">The number of ms to wait.</param>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        void Spin(int ms)
        {
            long to = (long)(ms * factor);
            for (long i = 0; i < to; ++i) ;
        }

        void QuickTermination()
        {
            try
            {
                while (true) Thread.Sleep(100);
            }
            finally
            {
                quickTerminationHasRun = true;
            }
        }

        void ABitSlowTermination()
        {
            try
            {
                while (true) Thread.Sleep(100);
            }
            finally
            {
                Spin(200);
                aBitSlowTerminationHasRun = true;
            }
        }

        void SlowTermination()
        {
            try
            {
                while (true) Thread.Sleep(100);
            }
            finally
            {
                Spin(2000);
                slowTerminationHasRun = true;
            }
        }

        void SlowWithCERTermination()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                while (true) Thread.Sleep(100);
            }
            finally
            {
                Spin(2000);
                slowWithCERTerminationHasRun = true;
            }
        }

        public void Run()
        {
            Calibrate();
            /*Calibrate();
            Calibrate();

            Test();
            Test();
            Test();*/

            Thread quick = new Thread(QuickTermination) { IsBackground = true };
            quick.Start();
            Thread.Sleep(100);
            quick.Abort();
            quick.Join();

            Thread aBitSlow = new Thread(ABitSlowTermination) { IsBackground = true };
            aBitSlow.Start();
            Thread.Sleep(100);
            aBitSlow.Abort();
            aBitSlow.Join();

            Thread slow = new Thread(SlowTermination) { IsBackground = true };
            slow.Start();
            Thread.Sleep(100);
            slow.Abort();
            slow.Join();

            Thread slowWithCER = new Thread(SlowWithCERTermination) { IsBackground = true };
            slowWithCER.Start();
            Thread.Sleep(100);
            slowWithCER.Abort();
            slowWithCER.Join();

            string format = "|{0,10}|{1,10}|{2,10}|{3,10}|{4,10}|";

            Func<bool, string> getStatus = b => b ? "OK" : "X";

            Console.WriteLine(format, "Thread", "Quick", "A bit slow", "Slow", "Slow CER");
            Console.WriteLine(format, "Finally?", getStatus(quickTerminationHasRun), getStatus(aBitSlowTerminationHasRun), getStatus(slowTerminationHasRun), getStatus(slowWithCERTerminationHasRun));
        }
    }
}
