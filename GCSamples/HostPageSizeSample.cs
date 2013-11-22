using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCSamples
{
    public class HostPageSizeSample : ISample
    {
        public void Run()
        {
            Console.WriteLine("Page's size: {0}", Environment.SystemPageSize);
        }
    }
}
