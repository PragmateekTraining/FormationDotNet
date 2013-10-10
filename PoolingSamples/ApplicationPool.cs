using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingSamples
{
    public interface IManagedApplication : IDisposable
    {
        Application Application { get; }
    }

    class ManagedApplication : IManagedApplication
    {
        public Application Application { get; private set; }

        private ApplicationPool pool;

        public ManagedApplication(Application application, ApplicationPool pool)
        {
            Application = application;
            this.pool = pool;
        }

        public ManagedApplication(Application application)
            : this(application, null)
        {
        }

        public void Dispose()
        {
            if (pool != null)
            {
                pool.ReleaseApplication(this);
            }
            else
            {
                Application.Quit();
                // Application = null;
            }
        }
    }

    public class ApplicationPool : IDisposable
    {
        ConcurrentBag<Application> applications = new ConcurrentBag<Application>();
        ConcurrentBag<Application> pool = new ConcurrentBag<Application>();

        const int maxApplicationsCount = 3;

        int applicationsCount = 0;

        object countLock = new object();

        public IManagedApplication Get()
        {
            Application application;

            if (!pool.TryTake(out application))
            {
                lock (countLock)
                {
                    if (applicationsCount < maxApplicationsCount)
                    {
                        ++applicationsCount;
                    }
                }

                application = new Application();

                applications.Add(application);
            }

            return new ManagedApplication(application, this);
        }

        public void ReleaseApplication(IManagedApplication managedApplication)
        {
            pool.Add(managedApplication.Application);
        }

        bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                foreach (Application application in applications)
                {
                    application.Quit();
                }

                disposed = true;

                GC.SuppressFinalize(this);
            }
        }

        ~ApplicationPool()
        {
            Dispose();
        }
    }
}
