using System;
using SamplesAPI;
using System.Threading;

namespace AppDomainSamples
{
    /// <summary>
    /// This sample demonstrates that using an app-domain won't protect you from unhandled exceptions.
    /// </summary>
    public class UnhandledExceptionSample : ISample
    {
        /// <summary>
        /// A component whose instances can throw exceptions.
        /// </summary>
        public class Buggy : MarshalByRefObject
        {
            /// <summary>
            /// Does not throw.
            /// </summary>
            public void OK()
            {
                Console.WriteLine("Buggy is OK :)");
            }

            /// <summary>
            /// Throws.
            /// </summary>
            public void KO()
            {
                Console.WriteLine("Buggy is KO X(");
                throw new Exception("Buggy is KO X(");
            }

            public void SmartKO()
            {
                Thread thread = new Thread(() => { throw new Exception("Buggy is KO X("); });
                thread.Start();
                thread.Join();
            }
        }

        /// <summary>
        /// If true create the buggy instance in a dedicated app-domain.
        /// </summary>
        bool createNewAppDomain = false;

        bool createNewThread = false;

        bool catchExceptions = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createNewAppDomain">True to instantiate the buggy class in a new app-domain.</param>
        public UnhandledExceptionSample(bool createNewAppDomain = false, bool createNewThread = false, bool catchExceptions = false)
        {
            this.createNewAppDomain = createNewAppDomain;
            this.createNewThread = createNewThread;
            this.catchExceptions = catchExceptions;
        }

        public void Run()
        {
            // By default use the current app-domain.
            AppDomain appDomain = AppDomain.CurrentDomain;

            if (createNewAppDomain)
            {
                Console.WriteLine("Creating new domain");
                // Create a dedicated app-domain.
                appDomain = AppDomain.CreateDomain("Addins' domain");
            }

            // Create the buggy instance in the app-domain.
            Buggy buggy = appDomain.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.UnhandledExceptionSample+Buggy") as Buggy;
            buggy.OK();

            Action f = () =>
            {
                if (createNewThread)
                {
                    buggy.SmartKO();
                }
                else
                {
                    // Throws.
                    buggy.KO();
                }
            };

            if (catchExceptions)
            {
                try
                {
                    f();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Got an exception:\n{0}", e);
                    AppDomain.Unload(appDomain);
                }

                Console.WriteLine("I'm alive!");
            }
            else
            {
                f();

                // Don't dream :(
                // Whatever the app-domain this code won't be executed.
                Console.WriteLine("I'm alive!");
            }
        }
    }
}
