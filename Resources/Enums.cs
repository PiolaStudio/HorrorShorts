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
    public enum TextAlignament : byte
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
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

    public enum SongType : short
    {
#if DEBUG
        [SongAttribute("Test1")]
        Test1,
        [SongAttribute("Test2")]
        Test2
#endif
    }
    public enum SoundType : int
    {
        [SoundAttribute("Speak/Speak1")]
        Speak1,
        [SoundAttribute("Speak/Speak2")]
        Speak2,
#if DEBUG
        [SoundAttribute("Test1")]
        Test1
#endif
    }
}
