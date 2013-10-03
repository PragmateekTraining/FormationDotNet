using SamplesAPI;
using System;

namespace COMInteropSamples
{
    public class ExcelAutomationSample : ISample
    {
        public void Run()
        {
            Type type = Type.GetTypeFromProgID("Excel.Application");
            /*dynamic excel = Activator.CreateInstance(type);
            excel.Visible = true;*/
        }
    }
}
