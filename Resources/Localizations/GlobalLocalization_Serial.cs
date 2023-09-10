using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Resources.Localizations
{
    public class GlobalLocalization_Serial
    {
        //Menu
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string NewGame = "New Game";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Continue = "Continue";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options = "Options";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Exit = "Exit";

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
}
