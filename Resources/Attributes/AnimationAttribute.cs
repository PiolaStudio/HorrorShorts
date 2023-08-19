namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AnimationAttribute : ResourceAttribute
    {
        public AnimationAttribute(string path) : base(path)
        {

        }
    }
}
