using SamplesAPI;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace ThreadingSamples
{
    public class DeadLockSample : ISample
    {
        IDbConnection database = new SqlConnection();
        object file = new object();

        void Spin()
        {
            DateTime to = DateTime.Now.AddMilliseconds(500);

            while (DateTime.Now < to) ;
        }

        void ReadDB()
        {
            lock (database)
            {
                Spin();

                lock (file)
                {
                    Spin();
                }
            }

            Console.WriteLine("End read.");
        }

        void WriteDB()
        {
            lock (file)
            {
                Spin();

                lock (database)
                {
                    Spin();
                }
            }

            Console.WriteLine("End write.");
        }

        Thread read;
        Thread write;

        void Monitor()
        {
            while (true)
            {
                Console.WriteLine("{0} / {1}", read.ThreadState, write.ThreadState);

                Thread.Sleep(200);
            }
        }

        public void Run()
        {
            read = new Thread(ReadDB) { IsBackground = true };
            write = new Thread(WriteDB) { IsBackground = true };
            Thread monitor = new Thread(Monitor) { IsBackground = true };

            monitor.Start();
            read.Start();
            write.Start();

            Console.ReadLine();
        }
    }
}
