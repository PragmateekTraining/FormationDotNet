using System.IO;
using System.Runtime.Serialization;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;

namespace System
{
    public static class Extensions
    {
        static private MemoryStream SerializeZipStream(object o, object context = null)
        {
            var serializer = new BinaryFormatter();
            if (context != null)
            {
                serializer.Context = new StreamingContext(StreamingContextStates.All, context);
            }
            var fs = new MemoryStream();
            using (var compressedzipStream = new GZipStream(fs, CompressionMode.Compress))
            {
                serializer.Serialize(compressedzipStream, o);
            }
            return fs;
        }

        static private void SerializeZipStream(object o, Stream fs, object context = null)
        {
            var serializer = new BinaryFormatter();
            if (context != null)
            {
                serializer.Context = new StreamingContext(StreamingContextStates.All, context);
            }
            using (var compressedzipStream = new GZipStream(fs, CompressionMode.Compress))
            {
                serializer.Serialize(compressedzipStream, o);
            }
        }

        static public T DeserializeZip<T>(Byte[] compressedArray, SerializationBinder binder, object context = null)
        {
            var serializer = new BinaryFormatter();
            if (context != null)
            {
                serializer.Context = new StreamingContext(StreamingContextStates.All, context);
            }
            if (binder != null)
            {
                serializer.Binder = binder;
                serializer.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }
            using (var fs = new MemoryStream(compressedArray))
            {
                using (var compressedzipStream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    T obj = (T)serializer.Deserialize(compressedzipStream);
                    return obj;
                }
            }
        }

        static public T DeserializeZip<T>(Stream compressedArray, SerializationBinder binder, bool allowUnsafe, object context = null)
        {
            var serializer = new BinaryFormatter();
            if (context != null)
            {
                serializer.Context = new StreamingContext(StreamingContextStates.All, context);
            }
            if (binder != null)
            {
                serializer.Binder = binder;
                serializer.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }
            using (var compressedzipStream = new GZipStream(compressedArray, CompressionMode.Decompress))
            {
                T obj;
                if (allowUnsafe)
                {
                    HeaderHandler hh = headers => headers;
                    obj = (T)serializer.UnsafeDeserialize(compressedzipStream, hh);
                }
                else
                {
                    obj = (T)serializer.Deserialize(compressedzipStream);
                }

                return obj;
            }
        }

        static public byte[] Zip(this object thisObject)
        {
            return SerializeZipStream(thisObject).ToArray();
        }

        static public T UnZip<T>(this byte[] thisByteArray)
        {
            return DeserializeZip<T>(thisByteArray, null);
        }

        static public void SaveAsZipArchive(this byte[] data, string zipArchivePath = "./data.zip", string entryName = "data.bin")
        {
            using (FileStream file = new FileStream(zipArchivePath, FileMode.Create, FileAccess.Write))
            {
                using (ZipOutputStream zipArchive = new ZipOutputStream(file))
                {
                    ZipEntry dataEntry = new ZipEntry(entryName);

                    zipArchive.PutNextEntry(dataEntry);

                    zipArchive.Write(data, 0, data.Length);

                    zipArchive.CloseEntry();
                }
            }
        }

        static public IDictionary<string, byte[]> GetContentOfZipArchive(this byte[] zipArchive)
        {
            IDictionary<string, byte[]> content = new Dictionary<string, byte[]>();

            using (ZipFile zipFile = new ZipFile(new MemoryStream(zipArchive)))
            {
                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsDirectory)
                    {
                        content[entry.Name] = null;
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            zipFile.GetInputStream(entry).CopyTo(ms);

                            content[entry.Name] = ms.ToArray();
                        }
                    }
                }
            }

            return content;
        }

        static public IDictionary<string, byte[]> GetContentOfZipArchive(this string zipArchivePath)
        {
            return File.ReadAllBytes(zipArchivePath).GetContentOfZipArchive();
        }
    }
}