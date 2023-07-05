using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Resources
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResourceAttribute : Attribute
    {
        public string Path { get; private set; }
        public ResourceAttribute(string path)
        {
            Path = path;
        }
    }
}
