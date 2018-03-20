using PaintDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MapFileCodec;

namespace MapFileType
{
    public class MapType : FileType
    {
        public MapType()
            : base("Map File", FileTypeFlags.SupportsSaving | FileTypeFlags.SupportsLoading | FileTypeFlags.SupportsLayers, new string[] { ".mft"})
        {
        }

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {

            var layerCount = input.Layers.Count;

            var layerNames = new string[layerCount];
            var layerDatas = new CellData[layerCount][];

            for (var i = 0; i < layerCount; i++)
            {
                var layer = input.Layers.GetAt(i);
                layerNames[i] = layer.Name;
                using (RenderArgs ra = new RenderArgs(new PaintDotNet.Surface(input.Size)))
                {
                    //You must call this to prepare the bitmap
                    layer.Render(ra, new System.Drawing.Rectangle(0, 0, input.Size.Width, input.Size.Height));
                    var bitmap = ra.Bitmap;
                    layerDatas[i] = new CellData[input.Size.Height * input.Size.Width];
                    for (var y = 0; y < input.Size.Height; y++)
                    {
                        for (var x = 0; x < input.Size.Width; x++)
                        {
                            var pixel = bitmap.GetPixel(x, y);
                            layerDatas[i][y * input.Size.Width + x] = new CellData()
                            {
                                ChannelR = pixel.R,
                                ChannelG = pixel.G,
                                ChannelB = pixel.B,
                                ChannelA = pixel.A
                            };

                        }
                    }
                    //output.Write()
                    //Now you can access the bitmap and perform some logic on it
                    //In this case I'm converting the bitmap to something else (byte[])
                    //var sampleData = ConvertBitmapToSampleFormat(ra.Bitmap);

                    //output.Write(sampleData, 0, sampleData.Length);
                }
            }

            var file = new MapFile()
            {
                Name = "default",
                Height = input.Size.Height,
                Width = input.Size.Width,
                LayerCount = layerCount,
                LayerData = layerDatas,
                LayerNames = layerNames
            };
            var buffer = MapFileCodec.Converter.ToBytes(file);
            output.Write(buffer, 0, buffer.Length);

        }

        protected override Document OnLoad(Stream input)
        {
            var bytes = StreamHelper.ReadToEnd(input);
            var file = MapFileCodec.Converter.FromBytes(bytes);

            
        }
    }
}
