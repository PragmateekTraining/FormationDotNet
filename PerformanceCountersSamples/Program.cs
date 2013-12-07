namespace PerformanceCountersSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new CLRMonitoringSample().Run();
            // new LoggingCountersSample(true).Run();
            new MultiInstanceCounterSample(args.Length == 1 ? args[0] : "").Run();
            // new SimpleCounterSample().Run();
        }
    }
}
