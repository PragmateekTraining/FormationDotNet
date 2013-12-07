namespace ParallelSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new CancellationTokenSample().Run();
            // new ExceptionsSample().Run();
            // new MaxDegreeOfParallelismSample().Run();            
            new PartitionerSample().Run();
        }
    }
}
