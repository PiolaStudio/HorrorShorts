using Microsoft.Xna.Framework.Content;

namespace Resources.Sprites
{
    public class Animation_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Animation", FlattenContent = true)]
        public SingleAnimation_Serial[] Animations;
    }
    public class SingleAnimation_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Name;

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string SpriteSheet;

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
