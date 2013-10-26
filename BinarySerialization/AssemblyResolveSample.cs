using System;
using SamplesAPI;
using System.IO;
using System.Reflection;

namespace SerializationSamples
{
    public class RemoteSerializer : MarshalByRefObject
    {
        public void Serialize()
        {
            global::A.A a = new global::A.A("42");
            File.WriteAllBytes("a.dat", a.ToNetBinary());
        }

        public void Deserialize()
        {
            Console.WriteLine("Deserializing: " + File.ReadAllBytes("a.dat").FromNetBinary<global::A.A>());
        }
    }

    public class AssemblyResolveSample : ISample
    {
        private static Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            if (assemblyName.Name == "B")
            {
                return Assembly.Load(File.ReadAllBytes("C.dll"));
            }

            return null;
        }

        public void Run()
        {
            // We isolate all the operation in dedicated temporaries app-domains to avoid locking assemblies
            AppDomain tmp = AppDomain.CreateDomain("tmp");
            RemoteSerializer serializer = tmp.CreateInstanceAndUnwrap("SerializationSamples", "SerializationSamples.RemoteSerializer") as RemoteSerializer;
            // Serialize in the context of the temporary app-domain
            serializer.Serialize();
            AppDomain.Unload(tmp);

            // Check that deserialization is OK
            tmp = AppDomain.CreateDomain("tmp");
            serializer = tmp.CreateInstanceAndUnwrap("SerializationSamples", "SerializationSamples.RemoteSerializer") as RemoteSerializer;
            serializer.Deserialize();
            Console.WriteLine("\n==========\n");
            AppDomain.Unload(tmp);

            // Simulate a renaming of the assembly, the old B.dll assemly does not exist anymore
            File.Delete("B.dll");

            tmp = AppDomain.CreateDomain("tmp");
            serializer = tmp.CreateInstanceAndUnwrap("SerializationSamples", "SerializationSamples.RemoteSerializer") as RemoteSerializer;
            try
            {
                // Should throw
                serializer.Deserialize();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot deserialize:\n{0}", e);
            }
            Console.WriteLine("\n==========\n");
            // Help the CLR find the correct assembly
            tmp.AssemblyResolve += AssemblyResolveHandler;
            try
            {
                // Should not throw
                serializer.Deserialize();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot deserialize:\n{0}", e);
            }
            Console.WriteLine("\n==========\n");
            AppDomain.Unload(tmp);
        }
    }
}
