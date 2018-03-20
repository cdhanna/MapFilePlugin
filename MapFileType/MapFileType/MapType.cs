using PaintDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MapFileType
{
    public class MapType : FileType
    {
        public MapType()
            : base("Map File", FileTypeFlags.SupportsSaving, new string[] { ".mft"})
        {
        }

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {

            var layerCount = input.Layers.Count;
            for (var i = 0; i < layerCount; i++)
            {
                var layer = input.Layers.GetAt(i);
                using (RenderArgs ra = new RenderArgs(new PaintDotNet.Surface(input.Size)))
                {
                    //You must call this to prepare the bitmap
                    layer.Render(ra, new System.Drawing.Rectangle(0, 0, input.Size.Width, input.Size.Height));
                    var bitmap = ra.Bitmap;

                    //output.Write()
                    //Now you can access the bitmap and perform some logic on it
                    //In this case I'm converting the bitmap to something else (byte[])
                    //var sampleData = ConvertBitmapToSampleFormat(ra.Bitmap);

                    //output.Write(sampleData, 0, sampleData.Length);
                }
            }
        }

        protected override Document OnLoad(Stream input)
        {
            throw new NotImplementedException();
        }
    }
}
