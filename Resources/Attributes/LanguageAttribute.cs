using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LanguageAttribute : Attribute
    {
        public readonly string NativeName;
        public LanguageAttribute(string naitveName)
        {
            NativeName = naitveName;
        }
    }
}
