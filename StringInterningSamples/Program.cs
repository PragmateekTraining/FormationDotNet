using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringInterningSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new MemorySavingSample().Run();
            // new InterningDrawbacksSample().Run();
            new ComparisonPerformanceSample(args[0] == "useSameString").Run();
        }
    }
}
