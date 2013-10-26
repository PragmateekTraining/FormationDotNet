using SamplesAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationSamples
{
    public class ISerializablePerformanceSample : ISample
    {
        [Serializable]
        class DefaultSerialization
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
        }

        [Serializable]
        class CustomSerialization : ISerializable
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }

            public CustomSerialization()
            {
            }

            public CustomSerialization(SerializationInfo info, StreamingContext context)
            {
                A = info.GetString("A");
                B = info.GetString("B");
                C = info.GetString("C");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("A", A);
                info.AddValue("B", B);
                info.AddValue("C", C);
            }
        }

        public void Run()
        {
            const int n = 1000000;

            BinaryFormatter formatter = new BinaryFormatter();

            DefaultSerialization ds = new DefaultSerialization();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < n; ++i)
            {
                formatter.Serialize(Stream.Null, ds);
            }

            stopwatch.Stop();

            TimeSpan defaultSerializationTime = stopwatch.Elapsed;

            CustomSerialization cs = new CustomSerialization();

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                formatter.Serialize(Stream.Null, cs);
            }

            stopwatch.Stop();

            TimeSpan customSerializationTime = stopwatch.Elapsed;

            Console.WriteLine("Default serialization: {0}", defaultSerializationTime);
            Console.WriteLine("Custom serialization: {0}", customSerializationTime);

            stopwatch.Restart();

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, ds);
                stream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < n; ++i)
                {
                    formatter.Deserialize(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }

            stopwatch.Stop();

            TimeSpan defaultDeserializationTime = stopwatch.Elapsed;

            stopwatch.Restart();

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, cs);
                stream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < n; ++i)
                {
                    formatter.Deserialize(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }

            stopwatch.Stop();

            TimeSpan customDeserializationTime = stopwatch.Elapsed;

            Console.WriteLine("Default serialization: {0}", defaultDeserializationTime);
            Console.WriteLine("Custom serialization: {0}", customDeserializationTime);
        }
    }
}
