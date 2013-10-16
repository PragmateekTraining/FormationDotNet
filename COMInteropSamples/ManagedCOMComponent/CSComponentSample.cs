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
            Tools.RunAndWait("ManagedCOMComponent/build.bat");
            Tools.RunAndWait("ManagedCOMComponent/Test.exe");
            Tools.RunAndWait("cscript", "ManagedCOMComponent/Test.vbs");
        }
    }
}
