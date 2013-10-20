using SamplesAPI;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CERSamples
{
    public class ThreadRudeAbortSample : ISample
    {
        public class B : MarshalByRefObject
        {
            public void Call(C c)
            {
                c.Call();
            }
        }

        public class C : MarshalByRefObject
        {
            public void Call()
            {
                for (int i = 0; i >= 0; ++i) ;
            }
        }

        AppDomain appDomainB;
        AppDomain appDomainC;

        bool withoutCERFinally;
        bool withCERFinally;

        void WithoutCER()
        {
            try
            {
                appDomainB = AppDomain.CreateDomain("b");
                appDomainC = AppDomain.CreateDomain("c");

                B b = appDomainB.CreateInstanceAndUnwrap("CERSamples", "CERSamples.ThreadRudeAbortSample+B") as B;
                C c = appDomainC.CreateInstanceAndUnwrap("CERSamples", "CERSamples.ThreadRudeAbortSample+C") as C;

                b.Call(c);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                withoutCERFinally = true;
            }
        }

        void WithCER()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                appDomainB = AppDomain.CreateDomain("b");
                appDomainC = AppDomain.CreateDomain("c");

                B b = appDomainB.CreateInstanceAndUnwrap("CERSamples", "CERSamples.ThreadRudeAbortSample+B") as B;
                C c = appDomainC.CreateInstanceAndUnwrap("CERSamples", "CERSamples.ThreadRudeAbortSample+C") as C;

                b.Call(c);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                withCERFinally = true;
            }
        }

        public void Run()
        {
            Thread withoutCERThread = new Thread(WithoutCER) { IsBackground = true };
            withoutCERThread.Start();

            Thread.Sleep(1000);
            AppDomain.Unload(appDomainB);

            withoutCERThread.Join();

            Thread withCERThread = new Thread(WithCER) { IsBackground = true };
            withCERThread.Start();

            Thread.Sleep(1000);
            AppDomain.Unload(appDomainB);
            withCERThread.Join();

            Console.WriteLine(withoutCERFinally);
            Console.WriteLine(withCERFinally);
        }
    }
}
