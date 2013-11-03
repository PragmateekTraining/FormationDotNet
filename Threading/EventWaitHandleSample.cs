using SamplesAPI;
using System;
using System.Threading;

namespace ThreadingSamples
{
    public class EventWaitHandleSample : ISample
    {
        string input;

        AutoResetEvent newInputAvailable = new AutoResetEvent(false);
        AutoResetEvent inputHasBeenProcessed = new AutoResetEvent(false);

        void ToUpper()
        {
            while (true)
            {
                newInputAvailable.WaitOne();

                Console.WriteLine(input.ToUpper());

                inputHasBeenProcessed.Set();
            }
        }

        public void Run()
        {
            new Thread(ToUpper) { IsBackground = true }.Start();

            while (true)
            {
                Console.Write("input? ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                newInputAvailable.Set();
                inputHasBeenProcessed.WaitOne();
            }
        }
    }
}
