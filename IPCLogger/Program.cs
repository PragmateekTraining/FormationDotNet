using System;
using System.ServiceModel;

namespace IPCLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "net.pipe://localhost/logger";

            using (ServiceHost host = new ServiceHost(new Logger(args[0]), new Uri(address)))
            {
                host.AddServiceEndpoint(typeof(ILogger), new NetNamedPipeBinding(), address);
                host.Open();

                Console.WriteLine("Press enter to stop logger...");
                Console.ReadLine();
            }
        }
    }
}
