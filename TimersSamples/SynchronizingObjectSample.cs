using SamplesAPI;
using System;
using timers = System.Timers;
using System.Windows.Forms;

namespace TimersSamples
{
    public class SynchronizingObjectSample : ISample
    {
        public void Run()
        {
            Form form = new Form { Text = "Test" };

            timers.Timer timer = new timers.Timer(1000);
            timer.Elapsed += (s, a) =>
            {
                Console.WriteLine(DateTime.Now.ToString("o"));
                Console.Write("Press enter to continue...");
                Console.ReadLine();
            };
            timer.SynchronizingObject = form;
            timer.Start();

            Application.Run(form);
        }
    }
}
