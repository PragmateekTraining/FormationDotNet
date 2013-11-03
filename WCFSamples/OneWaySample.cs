using SamplesAPI;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;

namespace WCFSamples
{
    public class OneWaySample : ISample
    {
        [ServiceContract(/*SessionMode = SessionMode.NotAllowed*/)]
        interface ILogService
        {
            [OperationContract]
            void Log(string message);

            [OperationContract(IsOneWay = true)]
            void LogOneWay(string message);
        }

        class LogService : ILogService
        {
            public void Log(string message)
            {
                Thread.Sleep(100);
            }

            public void LogOneWay(string message)
            {
                Thread.Sleep(100);
            }
        }

        public void Run()
        {
            const string url = "http://localhost:12345/log";
            WSHttpBinding binding = new WSHttpBinding(SecurityMode.None);

            ServiceHost host = new ServiceHost(typeof(LogService));
            host.AddServiceEndpoint(typeof(ILogService), binding, url);
            host.Open();

            ILogService proxy = ChannelFactory<ILogService>.CreateChannel(binding, new EndpointAddress(url));
            proxy.Log("");

            const int n = 100;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < n; ++i)
            {
                proxy.LogOneWay("");
                Console.WriteLine(i);
            }
            stopwatch.Stop();

            TimeSpan datagramTime = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                proxy.Log("");
                Console.WriteLine(i);
            }
            stopwatch.Stop();

            TimeSpan halfDuplexTime = stopwatch.Elapsed;

            host.Close();

            /*ServiceHost oneWayHost = new ServiceHost(typeof(OneWayLogService));
            oneWayHost.AddServiceEndpoint(typeof(IOneWayLogService), new WSHttpBinding(), "http://localhost:12345/log");
            oneWayHost.Open();

            IOneWayLogService oneWayProxy = ChannelFactory<IOneWayLogService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12345/log"));

            stopwatch.Restart();
            for (int i = 0; i < n; ++i)
            {
                oneWayProxy.Log("");
            }
            stopwatch.Stop();

            TimeSpan datagramTime = stopwatch.Elapsed;

            oneWayHost.Close();*/

            Console.WriteLine("Half-duplex: {0}", halfDuplexTime);
            Console.WriteLine("Datagram: {0}", datagramTime);
        }
    }
}
