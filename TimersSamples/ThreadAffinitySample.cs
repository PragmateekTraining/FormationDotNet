using SamplesAPI;
using System;
using System.Windows;
using System.Windows.Forms;
using timers = System.Timers;

namespace TimersSamples
{
    public class ThreadAffinitySample : ISample
    {
        Window window = new Window();
        Form form = new Form();

        void ChangeWindow()
        {
            try
            {
                Console.WriteLine("CheckAccess()? {0}", window.Dispatcher.CheckAccess());
                window.Title = "Test";
                Console.WriteLine("Done with window.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception with window:\n{0}", e);
            }
        }

        void ChangeForm()
        {
            try
            {
                Console.WriteLine("InvokeRequired? {0}", form.InvokeRequired);
                form.Text = "Test";
                Console.WriteLine("Done with form.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception with form:\n{0}", e);
            }
        }

        public void Run()
        {
            /*new Thread(ChangeWindow) { IsBackground = true }.Start();
            new Thread(ChangeForm) { IsBackground = true }.Start();*/

            timers.Timer timer = new timers.Timer(2000);
            timer.Elapsed += (s, a) =>
                {
                    timer.Stop();

                    ChangeWindow();
                    ChangeForm();
                };
            timer.Start();

            System.Windows.Forms.Application.Run(form);
        }
    }
}
