using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpriteSheetAttribute : ResourceAttribute
    {
        public SpriteSheetAttribute(string path) : base(path)
        {

        }
    }
}
