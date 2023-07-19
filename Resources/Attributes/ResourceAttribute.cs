using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResourceAttribute : Attribute
    {
        public string ResourcePath { get; private set; }
        public ResourceAttribute(string path)
        {
            ResourcePath = path;
        }
    }
}
