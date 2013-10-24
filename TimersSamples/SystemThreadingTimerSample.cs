using SamplesAPI;
using System;
using System.Threading;

namespace TimersSamples
{
    public class SystemThreadingTimerSample : ISample
    {
        void TimerElapsed(object state)
        {
            Console.WriteLine(DateTime.Now.ToString("o"));
        }

        public void Run()
        {
            Timer timer = new Timer(TimerElapsed, null, Timeout.Infinite, 0);
            timer.Change(0, 1000);

            while (true)
            {
                Console.Write("> ");

                string command = Console.ReadLine();
                string[] tokens = command.Split();
                string choice = tokens[0];

                if (choice == "s" || choice == "stop") timer.Change(Timeout.Infinite, 0);
                else if (choice == "r" || choice == "restart") timer.Change(0, int.Parse(tokens[1]));
                else if (choice == "q" || choice == "quit") break;
            }
        }
    }
}
