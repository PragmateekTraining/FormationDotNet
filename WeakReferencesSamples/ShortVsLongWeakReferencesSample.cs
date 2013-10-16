using System;
using SamplesAPI;

namespace WeakReferencesSamples
{
    public class ShortVsLongWeakReferencesSample : ISample
    {
        class A
        {
            ~A()
            {
                Console.WriteLine("~A");
            }
        }

        public void Run()
        {
            A target = new A();

            WeakReference shortWeakReference = new WeakReference(target);
            WeakReference longWeakReference = new WeakReference(target, trackResurrection: true);

            target = null;

            Console.WriteLine("{0} / {1}", shortWeakReference.Target ?? "null", longWeakReference.Target ?? "null");

            Console.WriteLine("===First collection===");

            GC.Collect();

            Console.WriteLine("{0} / {1}", shortWeakReference.Target ?? "null", longWeakReference.Target ?? "null");

            Console.WriteLine("===Finalization===");

            GC.WaitForPendingFinalizers();

            Console.WriteLine("{0} / {1}", shortWeakReference.Target ?? "null", longWeakReference.Target ?? "null");

            Console.WriteLine("===Second collection===");

            GC.Collect();

            Console.WriteLine("{0} / {1}", shortWeakReference.Target ?? "null", longWeakReference.Target ?? "null");
        }
    }
}
