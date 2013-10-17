using System;
using SamplesAPI;

namespace ExceptionsSamples
{
    public class AppDomainUnhandledExceptionSample : ISample
    {
        public void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, a) =>
            {
                Console.WriteLine("Ooops! Something bad happened: '{0}'!", (a.ExceptionObject as Exception).Message);
            };

            while (true)
            {
                Console.Write("Type some text to throw an exception or enter to quit: ");
                string message = Console.ReadLine();

                if (message == "") break;

                throw new Exception(message);
            }
        }
    }
}
