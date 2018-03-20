using PaintDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapFileType
{
    public class FileTypeFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new FileType[]
            {
                new MapType()
            };
        }
    }
}
