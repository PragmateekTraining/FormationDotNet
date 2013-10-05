using System;
namespace COMInteropSamples
{
    class Program
    {
        // [STAThread]
        static void Main(string[] args)
        {
            // new ExcelAutomationSample().Run();
            // Console.WriteLine(Environment.UserName);
            // new NativeCOMSample("azertyuyiiop!").Run();
            new CSComponentSample().Run();
        }
    }
}
