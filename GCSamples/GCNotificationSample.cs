using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace GCSamples
{
    class GCNotificationSample : ISample
    {
        IList<object> list = new List<object>();

        readonly GCLatencyMode latencyMode;

        readonly object consoleAccess = new object();

        enum Level
        {
            OK,
            INFO,
            WARNING,
            ERROR
        }

        void Log(Level level, string format, params object[] parameters)
        {
            lock (consoleAccess)
            {
                using (level == Level.OK ? Color.Green :
                    level == Level.INFO ? Color.Gray :
                    level == Level.WARNING ? Color.Yellow :
                    Color.Red)
                {
                    Console.WriteLine(format, parameters);
                }
            }
        }

        void MonitorGC()
        {
            try
            {
                while (true)
                {
                    GCNotificationStatus status = GC.WaitForFullGCApproach();

                    if (status == GCNotificationStatus.Succeeded)
                    {
                        long memory = GC.GetTotalMemory(false) / 1000000;

                        Log(Level.INFO, "[{0}] GC coming ({1}M)...", DateTime.Now.ToString("o"), memory);

                        do
                        {
                            status = GC.WaitForFullGCComplete(100);

                            if (status == GCNotificationStatus.Timeout)
                            {
                                memory = GC.GetTotalMemory(false) / 1000000;

                                Log(Level.WARNING, "[{0}] Still no GC ({1}M).", DateTime.Now.ToString("o"), memory);
                            }
                        }
                        while (status == GCNotificationStatus.Timeout);

                        if (status == GCNotificationStatus.Succeeded)
                        {
                            Log(Level.OK, "[{0}] GC done...", DateTime.Now.ToString("o"));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log(Level.ERROR, "Exception on monitoring thread:\n{0}", e);
            }
        }

        public GCNotificationSample(GCLatencyMode latencyMode)
        {
            this.latencyMode = latencyMode;
        }

        public void Run()
        {
            GCSettings.LatencyMode = latencyMode;
            GC.RegisterForFullGCNotification(5, 5);

            new Thread(MonitorGC) { IsBackground = true }.Start();

            while (true)
            {
                try
                {
                    object o = new object();

                    list.Add(o);
                    //list.Remove(o);
                }
                catch (OutOfMemoryException)
                {
                    long memory = GC.GetTotalMemory(false) / 1000000;

                    Log(Level.ERROR, "Manual GC. {0}", memory);

                    list.Clear();
                    GC.Collect();
                }
            }
        }
    }
}
