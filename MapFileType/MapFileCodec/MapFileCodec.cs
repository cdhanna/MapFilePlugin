using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
            //return new MapFile()
            //{
            //    Width = 32,
            //    Height = 32
            //};
            if (bytes.Length == 1)
            {
                return DefaultMapFiles.DefaultPattern(new string[] { "rooms", "walkable" },
                    new CellData[] { new CellData() { ChannelA = 255, ChannelR = 128, ChannelG = 128, ChannelB = 128 },
                    new CellData(){ChannelA=255,ChannelR=255,ChannelB=255,ChannelG=255 } }, 2, 2);
            }

            var formatter = new BinaryFormatter();
            formatter.Binder = new PreMergeToMergedDeserializationBinder();

            return (MapFile)formatter.Deserialize(new MemoryStream(bytes));
        }

    }

    sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String exeAssembly = Assembly.GetExecutingAssembly().FullName;


            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, exeAssembly));

            return typeToDeserialize;
        }
    }
}
