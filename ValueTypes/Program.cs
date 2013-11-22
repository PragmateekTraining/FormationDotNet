namespace ValueTypesSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new BulkAllocationSample().Run();
            new EqualsOverloadSample(10000000).Run();
            // new DefaultComparisonSample().Run();
            // new BasicSample().Run();
            // new BoxingPerformanceImpactSample().Run();
        }
    }
}
