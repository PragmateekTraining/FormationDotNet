namespace CERSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new AccessViolationSample().Run();
            new CERWithHostingSample("ThreadAbortTimeoutSample").Run();
            // new ContinuousExecutionSample().Run();
            // new DelayedThreadAbortSample().Run();
            // new InvalidProgramSample().Run();
            // new NativeAccessViolationSample(args.Length == 1 && args[0] == "CER").Run();
            // new OutOfMemorySample().Run();
            // new StackOverflowSample(args.Length == 1 && args[0] == "CER").Run();
            // new TerminateThreadSample().Run();
            // new ThreadRudeAbortSample().Run();
        }
    }
}
