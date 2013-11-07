using SamplesAPI;
using System.Threading;

namespace ThreadingSamples
{
    public class CountdownEventRaceSample : ISample
    {
        const int n = 10;

        CountdownEvent countdown;

        void DoSomething()
        {
            // Work
            countdown.Signal();
        }

        void BadLaunch()
        {
            for (int i = 1; i <= n; ++i)
            {
                countdown.AddCount();
                new Thread(DoSomething) { IsBackground = true }.Start();
            }
        }

        void Bad()
        {
            countdown = new CountdownEvent(0);
            new Thread(BadLaunch) { IsBackground = true }.Start();
            Thread.Sleep(1000);
            countdown.Wait();
        }

        void GoodLaunch()
        {
            for (int i = 1; i <= n; ++i)
            {
                countdown.AddCount();
                new Thread(DoSomething) { IsBackground = true }.Start();
            }
            countdown.Signal();
        }

        void Good()
        {
            countdown = new CountdownEvent(1);
            new Thread(GoodLaunch) { IsBackground = true }.Start();
            countdown.Wait();
        }

        public void Run()
        {
            // Bad();
            Good();
        }
    }
}
