using SamplesAPI;
using System;

namespace DisposePatternSamples
{
    public class UsingStatementSample : ISample
    {
        class Logger : IDisposable
        {
            public void Log(string message)
            {
                Console.WriteLine("Logging '{0}'.", message);
            }

            public void Dispose()
            {
                Console.WriteLine("Disposing logger.");
            }
        }

        public void Run()
        {
            using (Logger logger = new Logger())
            {
                logger.Log("Inside using.");
            }

            /* Logger logger = new Logger();
            try
            {
                logger.Log("Inside using.");
            }
            finally
            {
                if (logger != null)
                {
                    (logger as IDisposable).Dispose();
                }
            }*/
        }
    }
}
