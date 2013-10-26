namespace SerializationSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new AssemblyResolveSample().Run();
            // new EventsSerializationSample().Run();
            // new ComplexSerializationSample().Run();
            new ISerializablePerformanceSample().Run();
        }
    }
}
