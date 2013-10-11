using SamplesAPI;
using System;
using System.Threading;

namespace LazySamples
{
    public class DeadLockSample : ISample
    {
        static object sqliteLock = new object();

        class SqliteLogger
        {
            public SqliteLogger(string name)
            {
            }

            public void Log(string s)
            {
            }
        }

        Lazy<SqliteLogger> Error;

        SqliteLogger CreateErrorLogger()
        {
            lock (sqliteLock)
            {
                // Do something
            }

            return new SqliteLogger("Error");
        }

        void Spin()
        {
            DateTime to = DateTime.Now.AddMilliseconds(50);
            while (DateTime.Now < to) ;
        }

        void Process1()
        {
            lock (sqliteLock)
            {
                try
                {
                    // Do something
                    Spin();
                    Thread.Sleep(100);
                    throw new Exception();
                }
                catch (Exception e)
                {
                    Error.Value.Log(e.ToString());
                }
            }
        }

        void Process2()
        {
            try
            {
                //Do something
                Spin();
                throw new Exception();
            }
            catch (Exception e)
            {
                Error.Value.Log(e.ToString());
            }
        }

        public void Run()
        {
            Error = new Lazy<SqliteLogger>(CreateErrorLogger);

            Thread process1 = new Thread(Process1) { IsBackground = true };
            Thread process2 = new Thread(Process2) { IsBackground = true };

            process1.Start();

            Thread.Sleep(10);

            process2.Start();

            for (int i = 0; i < 20; ++i)
            {
                Console.WriteLine("{0} - {1}", process1.ThreadState, process2.ThreadState);
                Thread.Sleep(20);
            }

            Console.ReadLine();
        }
    }
}
