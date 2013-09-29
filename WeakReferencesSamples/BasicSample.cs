using System;
using SamplesAPI;

namespace WeakReferencesSamples
{
    /// <summary>
    /// To avoid any side-effects you should run it only in release mode without using the VS debugger, using the console
    /// </summary>
    public class BasicSample : ISample
    {
        public void Run()
        {
            A a = new A("strong");
            WeakReference wa = new WeakReference(new A("weak"));

            Console.WriteLine("Weak reference " + (wa.IsAlive ? "is" : "is not") + " alive.");

            Console.WriteLine("Starting GC.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("GC finished.");

            // Ensure compiler does not optimize, cleaning up prematurely
            // Console.WriteLine(a.S);
            a.ToString();
            Console.WriteLine("Weak reference " + (wa.IsAlive ? "is" : "is not") + " alive.");
        }
    }
}
