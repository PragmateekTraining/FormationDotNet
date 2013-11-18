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
            public byte[] Data = new byte[1 << 20];
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
            IList<BigBunch> bigBunches = new List<BigBunch>();

            bigBunches.Add(new BigBunch());
            bigBunches.Add(new BigBunch());

            Console.WriteLine("Press 'a' to allocate a new instance, 'd' to delete one, 'n' to nullify big bunch, 'r' to reference...");

            BigBunch reference = new BigBunch();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter) break;

                char action = key.KeyChar;

                int target = int.Parse(Console.ReadKey().KeyChar.ToString());

                if (action == 'a')
                {
                    bigBunches[target].bigList.Add(new Big());
                }
                else if (action == 'd' && bigBunches[target].bigList.Count > 0)
                {
                    bigBunches[target].bigList.RemoveAt(bigBunches[target].bigList.Count - 1);
                }
                else if (action == 'n')
                {
                    bigBunches[target] = null;
                }
                else if (action == 'r')
                {
                    reference.bigList.Add(bigBunches[target].bigList[0]);
                    /*reference.bigList.Add(null);
                    reference.bigList.Remove(null);*/
                }
            }
        }
    }
}
