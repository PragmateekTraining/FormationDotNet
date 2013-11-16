using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    class BackgroundVsForeground
    {
        void SayHello()
        {
            while (true)
            {
                Console.WriteLine("Greetings from thread!");
                Thread.Sleep(1000);
            }
        }

        public void Run(bool isBackground)
        {
            Thread thread = new Thread(SayHello) { IsBackground = isBackground };

            thread.Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            Console.WriteLine("Exiting.");
        }
    }
}
