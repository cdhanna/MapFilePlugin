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
            : base("Map File", FileTypeFlags.SupportsSaving | FileTypeFlags.SupportsLoading | FileTypeFlags.SupportsLayers | FileTypeFlags.SupportsCustomHeaders, new string[] { ".mft"})
        {
        }

        protected override SaveConfigToken OnCreateDefaultSaveConfigToken()
        {
            return base.OnCreateDefaultSaveConfigToken();
        }

        protected override SaveConfigToken GetSaveConfigTokenFromSerializablePortion(object portion)
        {
            return base.GetSaveConfigTokenFromSerializablePortion(portion);
        }

        protected override object GetSerializablePortionOfSaveConfigToken(SaveConfigToken token)
        {
            return base.GetSerializablePortionOfSaveConfigToken(token);
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
                            byte alpha = (byte)(255 * Math.Floor(((float)pixel.A) / layer.Opacity));
                            layerDatas[i][y * input.Size.Width + x] = new CellData()
                            {
                                ChannelR = pixel.R,
                                ChannelG = pixel.G,
                                ChannelB = pixel.B,
                                ChannelA = alpha
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

            var doc = new Document(file.Width, file.Height);

            for (var i = 0; i < file.LayerCount; i++)
            {
                var layer = new BitmapLayer(file.Width, file.Height);
                layer.Name = file.LayerNames[i];
                for (var y = 0; y < file.Height; y++)
                {
                    for (var x = 0; x < file.Width; x++)
                    {
                        var p = file.LayerData[i][y * file.Width + x];
                        layer.Surface[x, y] = new ColorBgra()
                        {
                            R = p.ChannelR,
                            G = p.ChannelG,
                            B = p.ChannelB,
                            A = p.ChannelA,
                        };

                    }
                }
                doc.Layers.Add(layer);

            }


            return doc;
        }
    }
}
