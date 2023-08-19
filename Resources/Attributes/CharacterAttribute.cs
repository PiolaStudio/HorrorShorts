using System.ComponentModel;

namespace Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CharacterAttribute : Attribute
    {
        public readonly string DefaultName;
        public readonly FaceType DefaultFace;
        public readonly SpeakType DefaultSpeak;
        public readonly int DefaultSpeakPitchMin;
        public readonly int DefaultSpeakPitchMax;

        public readonly int DefaultSpeakSpeed;
        public readonly FontType DefaultFont;
        public readonly TextureType? TextureType;

        public CharacterAttribute(string defaultName, FaceType defaultFace, SpeakType defaultSpeak, int defaultSpeakSpeed, int defaultSpeakPitchMin, int defaultSpeakPitchMax, FontType defaultFont, object textureType)
        {
            DefaultName = defaultName;
            DefaultFace = defaultFace;
            DefaultSpeak = defaultSpeak;
            DefaultSpeakSpeed = defaultSpeakSpeed;
            DefaultFont = defaultFont;
            TextureType = (TextureType?)textureType;
            DefaultSpeakPitchMin = defaultSpeakPitchMin;
            DefaultSpeakPitchMax = defaultSpeakPitchMax;
        }
    }
}
