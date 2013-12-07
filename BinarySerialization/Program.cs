namespace SerializationSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new AssemblyResolveSample().Run();            
            // new ComplexSerializationSample().Run();
            // new EventsSerializationSample().Run();
            new ISerializablePerformanceSample().Run();
        }
    }
}
