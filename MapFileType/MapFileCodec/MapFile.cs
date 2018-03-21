using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapFileCodec
{
    [Serializable]
    public class MapFile
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public CellData[][] LayerData { get; set; }
        public string[] LayerNames { get; set; }
        public int LayerCount { get; set; }
        public string Name { get; set; }

        public MapFile()
        {

        }

        public CellData[] GetData(int x, int y)
        {
            var output = new CellData[LayerCount];
            for (var i = 0; i < LayerCount; i++)
            {
                output[i] = GetData(i, x, y);
            }
            return output;
        }

        public CellData GetData(int layerIndex, int x, int y)
        {
            return LayerData[layerIndex][y * Width + x];
        }

        public CellData GetData(string layerName, int x, int y)
        {
            return GetData(GetLayerIndex(layerName), x, y);
        }

        public int GetLayerIndex(string layerName)
        {
            for (var i = 0; i < LayerNames.Length; i++)
            {
                if (LayerNames[i].ToLower().Equals(layerName))
                {
                    return i;
                }
            }
            throw new Exception($"No layer by that name {layerName}");
        }
    }
}
