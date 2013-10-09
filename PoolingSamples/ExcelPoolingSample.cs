using System;
using System.Linq;
using SamplesAPI;
using System.Threading.Tasks;
using System.IO;

namespace PoolingSamples
{
    public class ExcelPoolingSample : ISample
    {
        private string folder;

        public ExcelPoolingSample(string folder)
        {
            this.folder = folder;
        }

        async Task ProcessFile(object o)
        {
            string file = o as string;

            DataProcessor processor = new DataProcessor(file);

            await Task.Run((Action)processor.Run);
        }

        public void Run()
        {
            Console.WriteLine("Without pooling");

            string[] files = Directory.GetFiles(folder, "*.xls");

            Task.WaitAll(files.Select(file => ProcessFile(file)).ToArray());

            Console.WriteLine("With pooling");
        }
    }
}
