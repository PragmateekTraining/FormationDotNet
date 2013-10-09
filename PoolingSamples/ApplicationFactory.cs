using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingSamples
{
    class ApplicationFactory
    {
        private static ApplicationPool pool = new ApplicationPool();

        public static bool EnablePooling { get; set; }

        public static IManagedApplication Get()
        {
            IManagedApplication application = null;

            if (EnablePooling)
            {
                application = pool.Get();
            }
            else
            {
                application = new ManagedApplication(new Application());
            }

            return application;
        }
    }
}
