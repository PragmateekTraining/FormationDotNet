using SamplesAPI;
using System;
using System.Windows.Forms;

namespace TimersSamples
{
    public class WinformsTimerSample : ISample
    {
        public void Run()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, a) =>
                {
                    Console.WriteLine(DateTime.Now.ToString("o"));
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                };
            timer.Start();

            Application.Run(/*new Form { Text = "Hello" }*/);
        }
    }
}
