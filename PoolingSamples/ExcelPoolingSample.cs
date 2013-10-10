using System;
using System.Linq;
using SamplesAPI;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace PoolingSamples
{
    public class ExcelPoolingSample : ISample
    {
        private string folder;
        private int filesCount;

        public ExcelPoolingSample(string folder, int filesCount)
        {
            this.folder = folder;
            this.filesCount = filesCount;
        }

        /*async Task ProcessFile(object o)
        {
            string file = o as string;

            DataProcessor processor = new DataProcessor(file);

            await Task.Run((Action)processor.Run);
        }*/

        public void Run()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            for (int i = 1; i <= filesCount; ++i)
            {
                File.Copy("model.xlsx", folder + "/" + i + ".xlsx", true);
            }

            IEnumerable<string> files = Directory.GetFiles(folder, "*.xlsx").Select(file => Path.Combine(currentDirectory, file));

            Stopwatch stopwatch = Stopwatch.StartNew();

            foreach (string file in files)
            {
                DataProcessor processor = new DataProcessor(file);

                processor.Run();
            }
            // Task.WaitAll(files.Select(file => ProcessFile(file)).ToArray());

            stopwatch.Stop();

            TimeSpan withoutPoolingTime = stopwatch.Elapsed;

            ApplicationFactory.EnablePooling = true;

            stopwatch.Restart();

            foreach (string file in files)
            {
                DataProcessor processor = new DataProcessor(file);

                processor.Run();
            }
            // Task.WaitAll(files.Select(file => ProcessFile(file)).ToArray());

            stopwatch.Stop();

            TimeSpan withPoolingTime = stopwatch.Elapsed;

            Console.WriteLine("Without pooling: " + withoutPoolingTime);
            Console.WriteLine("With pooling: " + withPoolingTime);
        }
    }
}
