using System;
using SamplesAPI;

namespace AppDomainSamples
{
    public class UnhandledExceptionSample : ISample
    {
        public class Buggy : MarshalByRefObject
        {
            public void OK()
            {
                Console.WriteLine("Buggy is OK :)");
            }

            public void KO()
            {
                Console.WriteLine("Buggy is KO X(");
                throw new Exception("Buggy is KO X(");
            }
        }

        public void Run()
        {
            AppDomain sandbox = AppDomain.CreateDomain("Addins' domain");

            try
            {
                Buggy buggy = sandbox.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.UnhandledExceptionSample+Buggy") as Buggy;
                buggy.OK();
                buggy.KO();
            }
            catch (Exception e)
            {
                Console.WriteLine("Got unhandled exception:\n{0}", e);
                Console.WriteLine("Unloading sandbox.");
                AppDomain.Unload(sandbox);
            }

            /* sandbox.UnhandledException += (s, a) =>
                {
                    Console.WriteLine(a.ExceptionObject);
                };*/

        }
    }
}
