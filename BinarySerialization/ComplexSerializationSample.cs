using SamplesAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationSamples
{
    [Serializable]
    public class AV1
    {
        private string n;
        public string N
        {
            get { return n; }
            set { n = value; }
        }
    }

    [Serializable]
    public class AV2
    {
        public int N { get; set; }
    }

    public static class SerializationExtension
    {
        public static IDictionary<string, SerializationEntry> AsDictionary(this SerializationInfo info)
        {
            IDictionary<string, SerializationEntry> dictionary = new Dictionary<string, SerializationEntry>();

            foreach (SerializationEntry entry in info)
            {
                dictionary[entry.Name] = entry;
            }

            return dictionary;
        }
    }

    public class TypesMapSerializationBinder : SerializationBinder
    {
        struct TypeName
        {
            public string Assembly { get; private set; }
            public string Type { get; private set; }

            public TypeName(string assembly, string type)
                : this()
            {
                Assembly = assembly;
                Type = type;
            }
        }

        IDictionary<TypeName, Type> mapping = new Dictionary<TypeName, Type>();

        public TypesMapSerializationBinder Map(string assemblyName, string typeName, Type type)
        {
            mapping[new TypeName(assemblyName, typeName)] = type;

            return this;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            TypeName fullTypeName = new TypeName(assemblyName, typeName);

            Type type = null;
            if (!mapping.TryGetValue(fullTypeName, out type))
            {
                AssemblyName fullAssemblyName = new AssemblyName(assemblyName);

                fullTypeName = new TypeName(fullAssemblyName.Name, typeName);

                mapping.TryGetValue(fullTypeName, out type);
            }

            return type;
        }
    }

    [Serializable]
    public class A : ISerializable
    {
        public int N { get; set; }

        public A()
        {
        }

        public A(SerializationInfo info, StreamingContext context)
        {
            IDictionary<string, SerializationEntry> entries = info.AsDictionary();

            SerializationEntry nEntry;

            if (entries.TryGetValue("n", out nEntry) || entries.TryGetValue("<N>k__BackingField", out nEntry) || entries.TryGetValue("N", out nEntry))
            {
                N = Convert.ToInt32(nEntry.Value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("N", N);
        }
    }

    public class ComplexSerializationSample : ISample
    {
        public void Run()
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new TypesMapSerializationBinder().Map("SerializationSamples", "SerializationSamples.AV1", typeof(A))
                                                                .Map("SerializationSamples", "SerializationSamples.AV2", typeof(A));

            AV1 av1 = new AV1 { N = "1" };
            AV2 av2 = new AV2 { N = 2 };
            A a = new A { N = 3 };

            byte[] av1Bytes;
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, av1);
                av1Bytes = memory.ToArray();
            }

            byte[] av2Bytes;
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, av2);
                av2Bytes = memory.ToArray();
            }

            byte[] aBytes;
            using (MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, a);
                aBytes = memory.ToArray();
            }

            A av1Out;
            using (MemoryStream memory = new MemoryStream(av1Bytes))
            {
                av1Out = formatter.Deserialize(memory) as A;
            }

            A av2Out;
            using (MemoryStream memory = new MemoryStream(av2Bytes))
            {
                av2Out = formatter.Deserialize(memory) as A;
            }

            A aOut;
            using (MemoryStream memory = new MemoryStream(aBytes))
            {
                aOut = formatter.Deserialize(memory) as A;
            }

            Console.WriteLine("av1Out is A: {0}", av1Out is A);
            Console.WriteLine("av2Out is A: {0}", av2Out is A);

            Console.WriteLine("av1Out.N: {0}", av1Out.N);
            Console.WriteLine("av2Out.N: {0}", av2Out.N);
            Console.WriteLine("aOut.N: {0}", aOut.N);
        }
    }
}
