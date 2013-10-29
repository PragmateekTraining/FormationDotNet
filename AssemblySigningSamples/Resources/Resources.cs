using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySigningSamples.Resources
{
    public static class Resources
    {
        static string embedded = null;
        static string linked = null;

        static Resources()
        {
            Assembly resourcesAssembly = Assembly.GetExecutingAssembly();

            // resourcesAssembly.GetManifestResourceNames().ToList().ForEach(name => Console.WriteLine("[" + name + "]"));

            embedded = new StreamReader(resourcesAssembly.GetManifestResourceStream("EmbeddedRootSite.config")).ReadToEnd();
            linked = new StreamReader(resourcesAssembly.GetManifestResourceStream("LinkedRootSite.config")).ReadToEnd();
        }

        public static string GetEmbedded()
        {
            return embedded;
        }

        public static string GetLinked()
        {
            return linked;
        }
    }
}
