using System.Linq;

namespace AppDomainSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {

                if (args[0] == "isolation")
                {
                    bool sandbox = args.Length == 2 && args[1] == "sandbox";

                    new IsolationSample(sandbox).Run();
                }
                else if (args[0] == "reflection")
                {
                    bool restrict = args.Length == 2 && args[1] == "restrict";

                    new ReflectionSample(restrict).Run();
                }
                else if (args[0] == "security")
                {
                    new SecuritySample(args.Length == 1 && args[0] == "restrict").Run();
                }
                else if (args[0] == "unhandled")
                {
                    new UnhandledExceptionSample(args.Any(s => s == "new-app-domain"), args.Any(s => s == "new-thread"), args.Any(s => s == "catch")).Run();
                }
            }
        }
    }
}
