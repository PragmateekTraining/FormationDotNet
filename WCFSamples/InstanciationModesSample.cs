using SamplesAPI;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace WCFSamples
{
    public class InstanciationModesSample : ISample
    {
        [ServiceContract(Namespace = "http://pragmateek.com/")]
        public interface ITestService : IDisposable
        {
            [OperationContract()]
            void Test();
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
        public class TestServicePerCall : ITestService
        {
            public TestServicePerCall()
            {
                Console.WriteLine("* TestServicePerCall");
            }

            public void Test()
            {
            }

            public void Dispose()
            {
                Console.WriteLine("- TestServicePerCall");
            }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
        public class TestServicePerSession : ITestService
        {
            public TestServicePerSession()
            {
                Console.WriteLine("* TestServicePerSession");
            }

            public void Test()
            {
            }

            public void Dispose()
            {
                Console.WriteLine("- TestServicePerSession");
            }
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class TestServiceSingle : ITestService
        {
            public TestServiceSingle()
            {
                Console.WriteLine("* TestServiceSingle");
            }

            public void Test()
            {
            }

            public void Dispose()
            {
                Console.WriteLine("- TestServiceSingle");
            }
        }

        public void Run()
		{
			Task.Run(() => 
				{
					using(ServiceHost servicesHost = new ServiceHost(typeof(TestServicePerCall), new Uri("http://localhost:12341/services/")))
					{
						Console.WriteLine("Launching per call...");
					
						servicesHost.AddServiceEndpoint(typeof(ITestService), new WSHttpBinding(), "TestServicePerCall");
						servicesHost.Open();

						Console.WriteLine("Per call launched...");
						
						Console.ReadLine();
					}
				}
			);
			
			Task.Run(() => 
				{
					using(ServiceHost servicesHost = new ServiceHost(typeof(TestServicePerSession), new Uri("http://localhost:12342/services/")))
					{
						Console.WriteLine("Launching per session...");
					
						servicesHost.AddServiceEndpoint(typeof(ITestService), new WSHttpBinding(), "TestServicePerSession");
						servicesHost.Open();

						Console.WriteLine("Per session launched...");
						
						Console.ReadLine();
					}
				}
			);
			
			Task.Run(() => 
				{
					using(ServiceHost servicesHost = new ServiceHost(typeof(TestServiceSingle), new Uri("http://localhost:12343/services/")))
					{
						Console.WriteLine("Launching single...");
					
						servicesHost.AddServiceEndpoint(typeof(ITestService), new WSHttpBinding(), "TestServiceSingle");
						servicesHost.Open();

						Console.WriteLine("Single launched...");
						
						Console.ReadLine();
					}
				}
			);
			
			Thread.Sleep(2000); // Technically useless but make ouput clearer for the purpose of this demo
			
			Console.WriteLine();
			
			Console.WriteLine("Testing per call 1...");

            using (ITestService servicePerCall1 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12341/services/TestServicePerCall")))
            {
                servicePerCall1.Test();
                servicePerCall1.Test();
                servicePerCall1.Test();
            }
			
			Console.WriteLine("Testing per call 2...");

            using (ITestService servicePerCall2 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12341/services/TestServicePerCall")))
            {
                servicePerCall2.Test();
                servicePerCall2.Test();
                servicePerCall2.Test();
            }
			
			Console.WriteLine();
			
			Console.WriteLine("Testing per session 1...");

            using (ITestService servicePerSession1 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12342/services/TestServicePerSession")))
            {
                servicePerSession1.Test();
                servicePerSession1.Test();
                servicePerSession1.Test();
            }
			
			Console.WriteLine("Testing per session 2...");

            using (ITestService servicePerSession2 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12342/services/TestServicePerSession")))
            {
                servicePerSession2.Test();
                servicePerSession2.Test();
                servicePerSession2.Test();
            }
			
			Console.WriteLine();
			
			Console.WriteLine("Testing single 1...");

            using (ITestService serviceSingle1 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12343/services/TestServiceSingle")))
            {
                serviceSingle1.Test();
                serviceSingle1.Test();
                serviceSingle1.Test();
            }
			
			Console.WriteLine("Testing single 2...");

            using (ITestService serviceSingle2 = ChannelFactory<ITestService>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:12343/services/TestServiceSingle")))
            {
                serviceSingle2.Test();
                serviceSingle2.Test();
                serviceSingle2.Test();
            }
		}
    }
}
