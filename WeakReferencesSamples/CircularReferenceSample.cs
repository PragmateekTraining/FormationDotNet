using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SamplesAPI;

namespace WeakReferencesSamples
{
    /// <summary>
    /// This sample desmonstrates that weak references are useless for circular references as the GC already manage them.
    /// </summary>
    public class CircularReferenceSample : ISample
    {
        public class A
        {
            public B B { get; set; }

            ~A()
            {
                Console.WriteLine("~A");
            }
        }

        public class B
        {
            public A A { get; set; }

            ~B()
            {
                Console.WriteLine("~B");
            }
        }

        public class WeakA
        {
            public WeakReference B { get; set; }

            ~WeakA()
            {
                Console.WriteLine("~WeakA");
            }
        }

        public class WeakB
        {
            public WeakReference A { get; set; }

            ~WeakB()
            {
                Console.WriteLine("~WeakB");
            }
        }

        public void Run()
        {
            A a = new A();
            B b = new B();
            b.A = a;
            a.B = b;

            WeakA wa = new WeakA();
            WeakB wb = new WeakB();
            wb.A = new WeakReference(wa);
            wa.B = new WeakReference(wb);

            a = null;
            b = null;
            wa = null;
            wb = null;

            Console.WriteLine("Starting GC.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("GC finished.");
        }
    }
}
