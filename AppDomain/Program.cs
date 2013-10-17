namespace AppDomainSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new AddinsSample().Run(args.Length == 1 && args[0] == "new");
            if (args.Length >= 1)
            {
                if (args[0] == "reflection")
                {
                    bool restrict = args.Length == 2 && args[1] == "restrict";

                    new ReflectionSample(restrict).Run();
                }
                else if (args[0] == "isolation")
                {
                    bool sandbox = args.Length == 2 && args[1] == "sandbox";

                    new IsolationSample(sandbox).Run();
                }
                else if (args[0] == "unhandled")
                {
                    new UnhandledExceptionSample().Run();
                }
                // new SecuritySample(args.Length == 1 && args[0] == "restrict").Run();
            }
        }
    }
}
