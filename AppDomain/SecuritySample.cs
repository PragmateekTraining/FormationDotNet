using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using SamplesAPI;

namespace AppDomainSamples
{
    //[Serializable]
    public class Virus : MarshalByRefObject
    {
        public void RunMe()
        {
            Console.WriteLine("Ahahahahahaaahahahahah");
            string password = File.ReadAllText("my_password.txt");
            Console.WriteLine("Password is '{0}'.", password);
        }
    }

    public class SecuritySample : ISample
    {
        bool restrict = false;

        public SecuritySample(bool restrict)
        {
            this.restrict = restrict;
        }

        public void Run()
        {
            AppDomainSetup sandboxSetup = new AppDomainSetup
            {
                ApplicationBase = "." //Assembly.GetExecutingAssembly().CodeBase
            };
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            if (!restrict)
            {
                permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
            }
            AppDomain sandbox = AppDomain.CreateDomain("sandbox", null, sandboxSetup, permissions);
            // AppDomain sandbox = AppDomain.CreateDomain("sandbox");
            // PolicyLevel pl = PolicyLevel.CreateAppDomainLevel();
            // new PolicyStatement(permissions)
            // sandbox.SetAppDomainPolicy();
            Virus virus = sandbox.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.Virus") as Virus;

            try
            {
                virus.RunMe();
            }
            catch (Exception e)
            {
                Console.WriteLine("Virus has done something bad: {0}", e);
            }
        }
    }
}
