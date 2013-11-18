using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryProfilingSamples
{
    public class MemoryLeakSample : ISample
    {
        public event EventHandler OnSomething = delegate { };

        class Handler
        {
            byte[] data = new byte[1 << 24];

            public Handler()
            {
                Array.Clear(data, 0, data.Length);
            }

            public void HandleSomething(object sender, EventArgs args)
            {
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Press enter to leak...");
                Console.ReadLine();
                Handler handler = new Handler();
                OnSomething += handler.HandleSomething;
                handler = null;
                GC.Collect();
            }
        }
    }
}
