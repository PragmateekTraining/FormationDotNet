namespace LazySamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new DeadLockSample().Run();
            // new DefaultInstantiationSample().Run();
            // new LazyInitializerSample().Run();
            // new NotThreadSafeSample().Run();
            new PublicationOnlySample().Run();
            // new ThreadSafetyCostSample().Run();
        }
    }
}
