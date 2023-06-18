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
        public readonly string TextureName;
        public readonly string SheetName;

        public CharacterAttribute(string defaultName, FaceType defaultFace, SpeakType defaultSpeak, int defaultSpeakSpeed, int defaultSpeakPitchMin, int defaultSpeakPitchMax, FontType defaultFont, string textureName, string textureSheet)
        {
            DefaultName = defaultName;
            DefaultFace = defaultFace;
            DefaultSpeak = defaultSpeak;
            DefaultSpeakSpeed = defaultSpeakSpeed;
            DefaultFont = defaultFont;
            TextureName = textureName;
            SheetName = textureSheet;
            DefaultSpeakPitchMin = defaultSpeakPitchMin;
            DefaultSpeakPitchMax = defaultSpeakPitchMax;
        }
    }
}
