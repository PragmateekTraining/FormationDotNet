using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwigSamples
{
    public class SwigSample : ISample
    {
        public void Run()
        {
            NativeLogger logger = new NativeLogger("logs.log");
            logger.log("Hello from SWIG!");
        }
    }
}
