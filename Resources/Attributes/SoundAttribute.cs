namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SoundAttribute : ResourceAttribute
    {
        public AtmosphereType? AtmosphereParent = null;
        public SoundAttribute(string path) : base(path) { }
        public SoundAttribute(string path, AtmosphereType? atmosphereParent = null) : base(path) 
        {
            AtmosphereParent = atmosphereParent;
        }
    }
}
