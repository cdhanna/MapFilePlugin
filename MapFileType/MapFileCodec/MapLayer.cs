using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapFileCodec
{
    public class MapLayer
    {
        public string Name { get; set; }

        public CellData[] Data { get; set; }

        public MapLayer()
        {

        }
    }
}
