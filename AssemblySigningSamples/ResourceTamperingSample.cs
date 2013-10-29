using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySigningSamples
{
    public class ResourceTamperingSample
    {
        public void Run()
        {
            // Assembly assembly = Assembly.GetExecutingAssembly();

            /* Stream stream = assembly.GetManifestResourceStream("AssemblySigningSamples.RootSite.config");
            StreamReader reader = new StreamReader(stream);
            Console.WriteLine("Root site: " + reader.ReadToEnd());*/

            Assembly resourcesAssembly = Assembly.Load("Resources");

            resourcesAssembly.GetManifestResourceNames().ToList().ForEach(name => Console.WriteLine("[" + name + "]"));

            Console.WriteLine(new StreamReader(resourcesAssembly.GetManifestResourceStream("EmbeddedRootSite.config")).ReadToEnd());
            Console.WriteLine(new StreamReader(resourcesAssembly.GetManifestResourceStream("LinkedRootSite.config")).ReadToEnd());

            Console.WriteLine(Resources.Resources.GetEmbedded());
            Console.WriteLine(Resources.Resources.GetLinked());
        }
    }
}
