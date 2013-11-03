using SamplesAPI;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFSamples
{
    public class MetadataExchangeSample : ISample
    {
        [ServiceContract]
        interface IService
        {
            [OperationContract]
            double Add(double a, double b);
        }

        class Service : IService
        {
            public double Add(double a, double b)
            {
                return a + b;
            }
        }

        public void Run()
        {
            ServiceHost host = new ServiceHost(typeof(Service));
            
            host.AddServiceEndpoint(typeof(IService), new WSHttpBinding(), "http://localhost:12345/service/add");

            host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetUrl = new Uri("http://localhost:12345/service/metadata"), HttpGetEnabled = true });

            host.Open();

            Console.ReadLine();

            host.Close();
        }
    }
}
