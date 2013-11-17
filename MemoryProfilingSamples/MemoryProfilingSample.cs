using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace MemoryProfilingSamples
{
    public class MemoryProfilingSample : ISample
    {
        class Big
        {
            byte[] data = new byte[1 << 20];
        }

        class BigBunch
        {
            public IList<Big> bigList = new List<Big>();

            ~BigBunch()
            {
                Console.WriteLine("Collected");
            }
        }

        public void Run()
        {
            BigBunch bigBunch = new BigBunch();

            Console.WriteLine("Press 'a' to allocate a new instance, 'd' to delete one, 'n' to nullify big bunch...");

            while (true)
            {
                char input = Console.ReadKey().KeyChar;

                if (input == 'a')
                {
                    bigBunch.bigList.Add(new Big());
                }
                else if (input == 'd' && bigBunch.bigList.Count > 0)
                {
                    bigBunch.bigList.RemoveAt(bigBunch.bigList.Count - 1);
                }
                else if (input == 'n')
                {
                    bigBunch = null;
                }
            }
        }
    }
}
