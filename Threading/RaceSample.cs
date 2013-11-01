using SamplesAPI;
using System;
using System.Threading;

namespace ThreadingSamples
{
    public class RaceSample : ISample
    {
        string message;
        AutoResetEvent are = new AutoResetEvent(false);

        Random rand = new Random();

        void WriteRace()
        {
            Thread.Sleep(rand.Next(100));
            message = "Test";
        }

        void ReadRace()
        {
            Thread.Sleep(rand.Next(100));
            Console.WriteLine("Race : [{0}]", message);
        }

        void WriteNoRace()
        {
            Thread.Sleep(rand.Next(100));
            message = "Test";
            are.Set();
        }

        void ReadNoRace()
        {
            Thread.Sleep(rand.Next(100));
            are.WaitOne();
            Console.WriteLine("No race : [{0}]", message);
        }

        public void Run()
        {
            Thread writeRace = new Thread(WriteRace) { IsBackground = true };
            writeRace.Start();
            Thread readRace = new Thread(ReadRace) { IsBackground = true };
            readRace.Start();

            writeRace.Join();
            readRace.Join();

            Thread writeNoRace = new Thread(WriteNoRace) { IsBackground = true };
            writeNoRace.Start();
            Thread readNoRace = new Thread(ReadNoRace) { IsBackground = true };
            readNoRace.Start();

            writeNoRace.Join();
            readNoRace.Join();
        }
    }
}
