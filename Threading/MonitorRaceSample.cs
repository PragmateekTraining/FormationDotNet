using SamplesAPI;
using System;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class MonitorRaceSample : ISample
    {
        StringBuilder input = new StringBuilder();

        void ToUpper()
        {
            lock (input)
            {
                while (true)
                {
                    Monitor.Wait(input);

                    Console.WriteLine(input.ToString().ToUpper());

                    // DEADLOCK Monitor.Pulse(input);
                }
            }
        }

        public void Run()
        {
            new Thread(ToUpper) { IsBackground = true }.Start();

            // DIY
            // Thread.Sleep(100);

            while (true) 
            {
                lock (input)
                {
                    input.Clear();
                    input.Append(Console.ReadLine());
                    Monitor.Pulse(input);
                    // DEADLOCK Monitor.Wait(input);
                }
                // Thread.Sleep(1);
            }
        }
    }
}
