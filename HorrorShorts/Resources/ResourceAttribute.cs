using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Resources
{
    public class ResourceAttribute : Attribute
    {
        public string Path { get; private set; }
        public ResourceAttribute(string path)
        {
            Path = path;
        }
    }
}
