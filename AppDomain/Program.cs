using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                // new SecuritySample(args.Length == 1 && args[0] == "restrict").Run();
            }
        }
    }
}
