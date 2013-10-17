using System;
using SamplesAPI;

namespace AppDomainSamples
{
    public class IsolationSample : ISample
    {
        public class User
        {
            public string Name { get; private set; }

            public string Password { get; private set; }

            public static User Current { get; set; }

            public User(string name, string password)
            {
                Name = name;
                Password = password;
            }
        }

        public class Virus : MarshalByRefObject
        {
            public void RunMe()
            {
                Console.WriteLine("Ahahahahahaaahahahahah");
                User currentUser = User.Current;
                if (currentUser != null)
                {
                    Console.WriteLine("'{0}''s password is '{1}'.", currentUser.Name, currentUser.Password);
                }
                else
                {
                    Console.WriteLine("Nooooooo I've failed!");
                }
            }
        }

        bool createNewAppDomain = false;

        public IsolationSample(bool createNewAppDomain)
        {
            this.createNewAppDomain = createNewAppDomain;
        }

        public void Run()
        {
            User.Current = new User("Administrator", "abc123");

            AppDomain appDomain = AppDomain.CurrentDomain;

            if (createNewAppDomain)
            {
                Console.WriteLine("Creating new domain");
                appDomain = AppDomain.CreateDomain("Addins' domain");
            }

            Virus virus = appDomain.CreateInstanceAndUnwrap("AppDomainSamples", "AppDomainSamples.IsolationSample+Virus") as Virus;
            virus.RunMe();
        }
    }
}
