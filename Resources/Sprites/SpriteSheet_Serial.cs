using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;

namespace Resources.Sprites
{
    public class SpriteSheet_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Texture;
        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Sheet")]
        public SingleSheet_Serial[] Sheets;

        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
    }
    public class SingleSheet_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Name;
        [ContentSerializer(AllowNull = false, Optional = false)]
        public Rectangle Source;
    }
}
