using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadingSamples
{
    public class RaceAndDeadLockSample : ISample
    {
        class Input
        {
            public string ReadLine()
            {
                return Console.ReadLine();
            }
        }

        Input input = new Input();
        StringBuilder buffer = new StringBuilder();

        void Ping()
        {
            string message;

            while (true)
            {
                lock (input)
                {
                    message = input.ReadLine();
                }

                lock (buffer)
                {
                    buffer.Clear();
                    buffer.Append(message);
                }
            }
        }

        void Pong()
        {
            string message;

            while (true)
            {
                lock (buffer)
                {
                    message = buffer.ToString();
                }

                lock (input)
                {
                    message = input.ReadLine();
                }
            }
        }

        public void Run()
        {
        }
    }
}
