using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimersSamples
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // new WpfTimerSample().Run();
            // new WinformsTimerSample().Run();
            // new SynchronizingObjectSample().Run();
            // new WpfSynchronizingObjectSample().Run();
            // new SystemThreadingTimerSample().Run();
            new ThreadAffinitySample().Run();
        }
    }
}
