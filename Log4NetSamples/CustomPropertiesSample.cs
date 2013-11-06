using log4net;
using log4net.Config;
using SamplesAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Log4NetSamples
{
    public class CustomPropertiesSample : ISample
    {
        public void Run()
        {
            GlobalContext.Properties["username"] = Environment.UserName;
            GlobalContext.Properties["hostname"] = Dns.GetHostName();
            GlobalContext.Properties["processId"] = Process.GetCurrentProcess().Id;

            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            ILog logger = LogManager.GetLogger("Sample");

            logger.Info("Blah blah.");
            logger.Warn("Ooops!");
            logger.Fatal("Aaarrrrggghhhhh!");
        }
    }
}
