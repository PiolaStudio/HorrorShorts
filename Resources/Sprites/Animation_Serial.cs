using Microsoft.Xna.Framework.Content;
#if DEBUG
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;
#endif

namespace Resources.Sprites
{
    public class Animation_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Animation", FlattenContent = true)]
        public SingleAnimation_Serial[] Animations;

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
    public class SingleAnimation_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Name;

        [ContentSerializer(AllowNull = false, Optional = false)]
        public SpriteSheetType SpriteSheet;

        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Frame")]
        public AnimationFrame_Serial[] Frames;
    }
    public class AnimationFrame_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Sheet;

        [ContentSerializer(AllowNull = false, Optional = false)]
        public int Duration;
    }
}
