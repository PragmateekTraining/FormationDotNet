namespace PerformanceCountersSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new SimpleCounterSample().Run();
            // new LoggingCountersSample(true).Run();
            // new CLRMonitoringSample().Run();
            new MultiInstanceCounterSample(args.Length == 1 ? args[0] : "").Run();
        }
    }
}
