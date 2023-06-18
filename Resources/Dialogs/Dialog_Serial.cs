using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Resources.Dialogs
{
    public class Dialog_Serial
    {
        //[ContentSerializer(AllowNull = true, Optional = true)]
        //public string ID = null;

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Text = "Hello World!";

        [ContentSerializer(AllowNull = true, Optional = true)]
        public Characters? Character = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public FaceType? Face = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public FontType? FontType = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public int? FontSize = null; //2
        [ContentSerializer(AllowNull = true, Optional = true)]
        public Color? FontColor = null; //White

        [ContentSerializer(AllowNull = true, Optional = true)]
        public float? Speed = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public SpeakType? SpeakType = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public int? SpeakSpeed = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public int? SpeakPitch = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public int? SpeakPitchVariation = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public TextBoxLocation? Location = null;

        //public bool WaitInputAtEnd;
        //public bool AjustEndLine;
        //public bool CanAccelerate;
        //public bool StopActions;
        //public bool DoPauses;
    }
}
