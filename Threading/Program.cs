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
            new MutexOwnership().CanEnsureOwnership();
        }
    }
}
