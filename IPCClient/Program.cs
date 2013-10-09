using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPCLogger;
using System.ServiceModel;

namespace IPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "net.pipe://localhost/logger";

            ILogger logger = ChannelFactory<ILogger>.CreateChannel(new NetNamedPipeBinding(), new EndpointAddress(address));

            try
            {
                string message = null;
                while ((message = Console.ReadLine()) != "")
                {
                    logger.Log(message);
                }
            }
            finally
            {
                IDisposable disposable = logger as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
