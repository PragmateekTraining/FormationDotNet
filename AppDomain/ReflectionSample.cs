using System;
using SamplesAPI;
using System.Security;
using System.Security.Permissions;
using System.Reflection;

namespace AppDomainSamples
{
    public class ReflectionSample : ISample
    {
        public class User
        {
            public string Name { get; private set; }
            private const string PASSWORD = "secret";
            private string password = "secret";
            private string Password
            {
                get
                {
                    return "secret";
                }
            }

            public static User Current
            {
                get
                {
                    return new User { Name = "Administrator" };
                }
            }
        }

        public class Virus : MarshalByRefObject
        {
            public void RunMe()
            {
                Console.WriteLine("Ahahahahahaaahahahahah");
                User currentUser = User.Current;
                string password = typeof(User).GetField("PASSWORD", BindingFlags.NonPublic | BindingFlags.Static).GetValue(currentUser) as string;  // OK no problem take it, it's free!
                password = typeof(User).GetField("password", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(currentUser) as string;
                password = typeof(User).GetProperty("Password", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(currentUser, null) as string;
                Console.WriteLine("'{0}''s password is '{1}'.", currentUser.Name, password);
            }
        }

        bool restrict = false;

        public ReflectionSample(bool restrict)
        {
            this.restrict = restrict;
        }

        public void Run()
        {
            AppDomainSetup sandboxSetup = new AppDomainSetup
            {
                ApplicationBase = "."
            };
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            if (!restrict)
            {
                permissions.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
            }
            /*else
            {
                permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag..None));
            }*/
            AppDomain sandbox = AppDomain.CreateDomain("sandbox", null, sandboxSetup, permissions);

            Virus virus = sandbox.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.ReflectionSample+Virus") as Virus;

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
