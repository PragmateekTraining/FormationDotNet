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
            new SecuritySample().Run(args.Length == 1 && args[0] == "restrict");
        }
    }
}
