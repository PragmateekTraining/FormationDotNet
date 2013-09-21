using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Deamon
{
    class Program
    {
        static void Main(string[] args)
        {
            string guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

            using (Mutex mutex = new Mutex(false, "deamon-" + guid))
            {
                if (!mutex.WaitOne(0))
                {
                    Console.WriteLine("Another instance is running!");

                    return;
                }

                Console.WriteLine("Running batch...");
                Console.ReadLine();
            }
        }
    }
}
