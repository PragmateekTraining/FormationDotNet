using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemSamples
{
    public class FileCreateHandleSample : ISample
    {
        public void Run()
        {
            const string fileName = "test.txt";

            File.Delete(fileName);

            Tools.RunAndWait(@"..\..\..\Tools\Handle\handle.exe", "-p " + Process.GetCurrentProcess().Id);
            File.Create(fileName);
            Tools.RunAndWait(@"..\..\..\Tools\Handle\handle.exe", "-p " + Process.GetCurrentProcess().Id);
        }
    }
}
