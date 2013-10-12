using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazySamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new DeadLockSample().Run();
            // new DefaultInstantiationSample().Run();
            // new NotThreadSafeSample().Run();
            new ThreadSafetyCostSample().Run();
        }
    }
}
