using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CERSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new TerminateThreadSample().Run();
            // new OutOfMemorySample().Run();
            // new StackOverflowSample(args.Length == 1 && args[0] == "CER").Run();
            // new InvalidProgramSample().Run();
            new AccessViolationSample().Run();
        }
    }
}
