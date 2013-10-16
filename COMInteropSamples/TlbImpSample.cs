using ManagedCOMLogger;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMInteropSamples
{
    public class TlbImpSample : ISample
    {
        public void Run()
        {
            IComLogger logger = new ComLogger();
            logger.Log("Ooops core meltdown!");
        }
    }
}
