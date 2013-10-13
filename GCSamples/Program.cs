using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GCSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            new GCNotificationSample((GCLatencyMode)Enum.Parse(typeof(GCLatencyMode), args[0])).Run();
        }
    }
}
