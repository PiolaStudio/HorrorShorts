using Resources.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Resources
{
    public enum Characters : short
    {
        [Character(null, FaceType.None, SpeakType.Speak1, 3, -10, 10, FontType.Arial, null, null)]
        Narrator,
        [Character("Girl 1", FaceType.Happy, SpeakType.Speak1, 3, -10, 10, FontType.Arial, "Girl1", "Girl1")]
        Girl1
    }
    public enum FaceType : byte
    {
        None = 0,
        Normal,
        Happy,
        Sad,
        Angry,
        Default = 255,
    }

    public enum TextBoxLocation : byte
    {
        TopLeft,
        TopRight,
        MiddleLeft,
        MiddleRight,
        BottomLeft,
        BottomRight
    }
    public enum SpeakType : byte
    {
        None = 0,
        Speak1,
        Speak2,
        Default = 255
    }

    public enum FontType : byte
    {
        Arial = 0,
        Default = 255
    }
}
