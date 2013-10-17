using System;
using SamplesAPI;

namespace AppDomainSamples
{
    public class AddinsSample : ISample
    {
        //[Serializable]
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

        bool createNewAppDomain = false;

        public AddinsSample(bool createNewAppDomain)
        {
            this.createNewAppDomain = createNewAppDomain;
        }

        public void Run()
        {
            AppDomain appDomain = AppDomain.CurrentDomain;

            if (createNewAppDomain)
            {
                Console.WriteLine("Creating new domain");
                appDomain = AppDomain.CreateDomain("Addins' domain");
            }

            Buggy buggy = appDomain.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.AddinsSample+Buggy") as Buggy;
            buggy.OK();
            buggy.KO();

            // Don't dream :(
            Console.WriteLine("I'm alive!");
        }
    }
}
