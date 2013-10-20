using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
namespace CERSamples
{
    class Program
    {
        static bool cerWorked;

        unsafe static void Main(string[] args)
        {
            Value value;
            value.Data[0] = 0;

            /*try
            {
                cerWorked = true;
                MyFn();
            }
            catch (Exception e)
            {
                Console.WriteLine(cerWorked);
                Console.WriteLine(e);
            }
            Console.ReadLine();*/
        }

    unsafe struct Value
    {
        // public fixed byte Data[1073741800]; // StackOverflow (2^30 - 24)
        // public fixed byte Data[1073741801]; // InvalidProgram
        // public fixed byte Data[2147483631]; // InvalidProgram
        // public fixed byte Data[2147483632]; //OutOfMemory (2^31 - 16)
        public fixed byte Data[2147483647]; // OutOfMemory
    }

        //results depends on the existance of this attribute
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        unsafe static void StackOverflow()
        {
            /*Big big;
            big.Bytes[int.MaxValue - 1] = 1;*/

            int n = int.Parse(Console.ReadLine());

            int* p = stackalloc int[n];
        }

        static void MyFn()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                cerWorked = false;
            }
            finally
            {
                Console.WriteLine("IN");
                StackOverflow();
            }
        }

        /*static void Main(string[] args)
        {
            // new TerminateThreadSample().Run();
            // new OutOfMemorySample().Run();
            new StackOverflowSample(args.Length == 1 && args[0] == "CER").Run();
            // new InvalidProgramSample().Run();
            // new AccessViolationSample().Run();
            // new ContinuousExecutionSample().Run();
            // new ThreadRudeAbortSample().Run();
            // new NativeAccessViolationSample(args.Length == 1 && args[0] == "CER").Run();
        }*/

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
