using SamplesAPI;
using System;
using System.Threading;

namespace LazySamples
{
    public class PublicationOnlySample : ISample
    {
        static Random rand = new Random();

        static object ObjectFactory()
        {
            if (rand.Next(10) != 0)
            {
                throw new Exception("ObjectFactory");
            }

            return "Value";
        }

        Lazy<object> lazy = new Lazy<object>(ObjectFactory, LazyThreadSafetyMode.PublicationOnly);

        public void Run()
        {
            object o = null;

            while (!lazy.IsValueCreated)
            {
                Console.WriteLine("==========");

                try
                {
                    o = lazy.Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine("Got value: '{0}'!", lazy.Value);
            Console.WriteLine("Got value: '{0}'!", lazy.Value);
            Console.WriteLine("Got value: '{0}'!", lazy.Value);
        }
    }
}
