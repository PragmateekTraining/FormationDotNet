using System;
namespace COMInteropSamples
{
    class Program
    {
        // [STAThread]
        static void Main(string[] args)
        {
            new CSComponentSample().Run();
            // new ExcelAutomationSample().Run();
            // new NativeCOMSample("azertyuyiiop!").Run();            
            // new TlbImpSample().Run();
        }
    }
}
