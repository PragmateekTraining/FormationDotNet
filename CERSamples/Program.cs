using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
namespace CERSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new TerminateThreadSample().Run();
            // new OutOfMemorySample().Run();
            // new StackOverflowSample(args.Length == 1 && args[0] == "CER").Run();
            // new InvalidProgramSample().Run();
            // new AccessViolationSample().Run();
            // new ContinuousExecutionSample().Run();
            // new ThreadRudeAbortSample().Run();
            // new NativeAccessViolationSample(args.Length == 1 && args[0] == "CER").Run();
            // new DelayedThreadAbortSample().Run();
            new CERWithHostingSample("ThreadAbortTimeoutSample").Run();
        }

        /*static void Main(string[] args)
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                DisposableType d = new DisposableType();
                d.Dispose();
                d = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
                Console.WriteLine("catch");
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }

        public class DisposableType : IDisposable
        {
            public void Dispose()
            {
            }

            ~DisposableType()
            {
                throw new NotImplementedException();
            }
        }*/
    }
}
