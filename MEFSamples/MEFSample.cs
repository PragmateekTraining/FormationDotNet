using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFSamples
{
    public class MEFSample : ISample
    {
        public void Run()
        {
            new ExtensibleShell().Run();
        }
    }
}
