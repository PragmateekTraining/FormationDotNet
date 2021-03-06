﻿using log4net;
using log4net.Config;
using SamplesAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Log4NetSamples
{
    public class LoggerAdditivitySample : ISample
    {
        void NonSensitive()
        {
            ILog logger = LogManager.GetLogger("NotSensitive");

            logger.Error("Not sensitive!");
        }

        void Sensitive()
        {
            ILog logger = LogManager.GetLogger("Sensitive");

            logger.Error("Sensitive!");
        }

        public void Run()
        {
            GlobalContext.Properties["username"] = Environment.UserName;
            GlobalContext.Properties["hostname"] = Dns.GetHostName();
            GlobalContext.Properties["processId"] = Process.GetCurrentProcess().Id;

            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            ILog logger = LogManager.GetLogger("Sample");

            logger.Info("Start");
            NonSensitive();
            Sensitive();
            logger.Info("End");
        }
    }
}
