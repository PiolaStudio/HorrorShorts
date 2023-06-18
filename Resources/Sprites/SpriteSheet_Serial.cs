using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#if DEBUG
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif
using System.Diagnostics;
using System.Xml;

namespace Resources.Sprites
{
    //[DebuggerDisplay("{Name}")]
    public class SpriteSheet_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Texture;

        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Sheet")]
        public SingleSheet_Serial[] Sheets;

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
    public class SingleSheet_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Name;
        [ContentSerializer(AllowNull = false, Optional = false)]
        public Rectangle Source;
    }
}
