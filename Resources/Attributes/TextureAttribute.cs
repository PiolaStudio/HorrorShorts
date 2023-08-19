namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TextureAttribute : ResourceAttribute
    {
        public readonly SpriteSheetType? Sheet;
        public TextureAttribute(string path) : base(path)
        {

        }
        public TextureAttribute(string path, SpriteSheetType sheet) : this(path)
        {
            this.Sheet = sheet;
        }
    }
}
