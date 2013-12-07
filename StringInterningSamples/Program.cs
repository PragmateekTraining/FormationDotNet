namespace StringInterningSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            new ComparisonPerformanceSample(args[0] == "useSameString").Run();
            // new InterningDrawbacksSample().Run();
            // new MemorySavingSample().Run();
        }
    }
}
