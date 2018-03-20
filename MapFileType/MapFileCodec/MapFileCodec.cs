using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MapFileCodec
{
    public class Converter
    {
        
        public static byte[] ToBytes(MapFile file)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, file);
                return stream.ToArray();
            }

        }

        public static MapFile FromBytes(byte[] bytes)
        {

        }

    }
}
