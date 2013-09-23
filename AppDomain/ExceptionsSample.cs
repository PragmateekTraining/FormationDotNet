using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security;
using System.IO;
using System.Security.Policy;

namespace AppDomainSamples
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

    public class AddinsSample
    {
        internal void Run(bool createNewAppDomain)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;

            if (createNewAppDomain)
            {
                Console.WriteLine("Creating new domain");
                appDomain = AppDomain.CreateDomain("Addins' domain");
            }

            Buggy buggy = appDomain.CreateInstanceAndUnwrap("AppDomain", "AppDomainSamples.Buggy") as Buggy;
            buggy.OK();
            buggy.KO();

            // Don't dream :(
            Console.WriteLine("I'm alive!");
        }
    }
}
