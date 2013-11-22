namespace GCSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new GCNotificationSample((GCLatencyMode)Enum.Parse(typeof(GCLatencyMode), args[0])).Run();
            // new GCGenerationsSample().Run();
            new GenerationsSizeSample().Run();
            // new HostPageSizeSample().Run();
            // new MemoryFailPointSample().Run();
        }
    }
}
