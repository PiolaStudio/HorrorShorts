namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SoundAttribute : ResourceAttribute
    {
        public SoundAttribute(string path) : base(path) 
        { 

        }
    }
}
