using System;
using System.Threading;

namespace Threading
{
    class LifeCycle
    {
        static void Task()
        {
            DateTime t = DateTime.Now.AddSeconds(1);

            while (DateTime.Now < t) ;

            Thread.Sleep(1000);
        }

        internal static void Run()
        {
            Thread thread = new Thread(Task);

            Console.WriteLine(thread.ThreadState);

            thread.Start();

            while (true)
            {
                Console.WriteLine(thread.ThreadState);

                Thread.Sleep(100);
            }
        }
    }
}
