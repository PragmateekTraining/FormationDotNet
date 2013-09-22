using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System
{
    public static class BinarySerializationExtensions
    {
        public static byte[] ToNetBinary(this object thisObject)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, thisObject);

                return stream.ToArray();
            }
        }

        public static T FromNetBinary<T>(this byte[] thisByteArray)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                stream.Write(thisByteArray, 0, thisByteArray.Length);

                stream.Seek(0, 0);

                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
