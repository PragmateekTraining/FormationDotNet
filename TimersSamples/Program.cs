using System;

namespace TimersSamples
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // new SynchronizingObjectSample().Run();
            // new SystemThreadingTimerSample().Run();
            new ThreadAffinitySample().Run();
            // new WinformsTimerSample().Run();
            // new WpfSynchronizingObjectSample().Run();
            // new WpfTimerSample().Run();
        }
    }
}
