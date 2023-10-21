using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Resources.Localizations
{
    public class GlobalLocalization_Serial
    {
        //Menu
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string MainTitle_NewGame = "New Game";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string MainTitle_Continue = "Continue";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string MainTitle_Options = "Options";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string MainTitle_Exit = "Exit";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen = "Screen";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Sound = "Sound";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control = "Control";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_General = "General";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Save = "Save";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Close = "Close";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Screen = "Screen";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Resolution = "Resolution";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Resizable = "Resizable";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Vsync = "Vsync";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_HardwareMode = "Hadrware Mode";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_FullScreen = "Full Screen";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Window = "Window";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Screen_Borderless = "Borderless";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Sound_General = "Sound";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Sound_Music = "Music";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Sound_Effects = "Effects";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Sound_Atmosphere = "Atmosphere";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Control = "Control";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Reset = "Reset";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Primary = "Primary";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Secundary = "Secundary";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Up = "Up";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Down = "Down";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Left = "Left";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Right = "Right";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Action = "Action";
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_Control_Back = "Back";

        [ContentSerializer(AllowNull = false, Optional = false)]
        public string Options_General_Language = "Language";

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
}
