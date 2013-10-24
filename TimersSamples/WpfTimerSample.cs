using SamplesAPI;
using System;
using System.Windows;
using System.Windows.Threading;

namespace TimersSamples
{
    public class WpfTimerSample : ISample
    {
        public void Run()
        {
            Application app = new Application();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, a) =>
                {
                    Console.WriteLine(DateTime.Now.ToString("o"));
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                };
            timer.Start();

            app.Run();
        }
    }
}
