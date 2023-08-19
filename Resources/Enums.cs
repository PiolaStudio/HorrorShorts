﻿using Resources.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Resources
{
    public enum Characters : short
    {
        [Character(null, FaceType.None, SpeakType.Speak1, 3, -10, 10, FontType.Arial, null)]
        Narrator,
        [Character("Girl 1", FaceType.Happy, SpeakType.Speak1, 3, -10, 10, FontType.Arial, TextureType.Girl1)]
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

    public enum DialogBoxLocation : byte
    {
        TopLeft,
        TopRight,
        MiddleLeft,
        MiddleRight,
        BottomLeft,
        BottomRight
    }
    public enum QuestionBoxLocation : byte
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,

        DialogTopLeft,
        DialogTopRight,
        DialogMiddleLeft,
        DialogMiddleRight,
        DialogBottomLeft,
        DialogBottomRight
    }

    public enum HorizontalAlignament : byte
    {
        Left,
        Center,
        Right
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


    //RESOURCES
    public enum TextureType : uint
    {
        [TextureAttribute(null)]
        Pixel,
        [TextureAttribute("UI/DialogMenu")]
        DialogMenu,
        [TextureAttribute("Characters/Girl1", SpriteSheetType.Girl1)]
        Girl1,
#if DEBUG
        [TextureAttribute("Characters/Mario", SpriteSheetType.Mario)]
        Mario,
        [TextureAttribute("Characters/Megaman", SpriteSheetType.Megaman)]
        Megaman,
#endif
    }
    public enum SpriteSheetType : uint
    {
        [SpriteSheetAttribute("Characters/Girl1")]
        Girl1,
#if DEBUG
        [SpriteSheetAttribute("Characters/Mario")]
        Mario,
        [SpriteSheetAttribute("Characters/Megaman")]
        Megaman
#endif
    }
    public enum AnimationType : uint
    {
#if DEBUG
        [AnimationAttribute("Characters/Megaman")]
        Megaman
#endif
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
        [SoundAttribute("UI/OptionChange")]
        OptionChange,
        [SoundAttribute("UI/OptionSelect")]
        OptionSelect,
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
