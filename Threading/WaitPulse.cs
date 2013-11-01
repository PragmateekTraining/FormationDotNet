using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace ThreadingSamples
{
    class WaitPulse
    {
        readonly object newMessages = new object();

        Queue<string> messages = new Queue<string>();

        bool keepRunning = true;

        void Logger()
        {
            Console.WriteLine("Starting logging");

            while (true)
            {
                string message;

                lock (newMessages)
                {
                    while (messages.Count == 0 && keepRunning)
                        Monitor.Wait(newMessages);

                    if (!keepRunning) break;

                    message = messages.Dequeue();
                }

                Thread.Sleep(3000);

                File.AppendAllLines("logs.log", new[] { message });
            }

            Console.WriteLine("Stopping logging");
        }

        internal void Run()
        {
            new Thread(Logger).Start();

            while (true)
            {
                Console.Write("Enter a log message: ");

                string message = Console.ReadLine();

                lock (newMessages)
                {
                    if (message == "")
                    {
                        keepRunning = false;
                        Monitor.Pulse(newMessages);
                        break;
                    }

                    messages.Enqueue(message);
                    Monitor.Pulse(newMessages);
                }
            }

            Console.WriteLine("Type enter to exit...");
            Console.ReadLine();
        }
    }
}
