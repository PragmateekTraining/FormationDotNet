namespace ProcessSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            new FailFastSample(args.Length == 1 && args[0] == "failFast").Run();
            // new JobObjectSample(args.Length == 1 && args[0] == "create-job-object").Run();            
        }
    }
}
