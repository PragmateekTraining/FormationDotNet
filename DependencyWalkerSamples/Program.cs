namespace DependencyWalkerSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            new DependenciesTrackingSample(args.Length == 1 ? args[0] : "").Run();
        }
    }
}
