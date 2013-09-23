using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            // LifeCycle.Run();
            // Priorities.Run();
            // UI.Run();
            // new WaitPulse().Run();
            // new MutexOwnership().CanEnsureOwnership();
            // new Scalability().Run();
            // new BackgroundVsForeground().Run(true);
            // new Downloader().Run();
            new BarrierSample(args.Length == 1 && args[0] == "use-barrier").Run();
        }
    }
}
