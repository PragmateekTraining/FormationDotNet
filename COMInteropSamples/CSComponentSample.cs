using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMInteropSamples
{
    public class CSComponentSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait("cscript", "Test.vbs");
        }
    }
}
