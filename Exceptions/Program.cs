using System;
using System.Windows;
using System.Windows.Controls;

namespace ExceptionsSamples
{
    class Program
    {
        static event EventHandler h = delegate { };

        [STAThread]
        static void Main(string[] args)
        {
            // new RethrowSample(args.Length == 1 && args[0] == "rethrow").Run();
            // new ExceptionDataSample().Run();
            h(null, null);
            h -= delegate { };
            h(null, null);
        }
    }
}
