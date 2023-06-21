using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Dialogs
{
    //todo: armar esta clase
    public class Characters_Serial
    {
        [ContentSerializer(FlattenContent = true)]
        public CharacterData_Serial[] Characters;
    }
    //todo: armar esta clase
    public class CharacterData_Serial
    {
        public string Name;
        public FaceType DefaultFace;
        public SpeakType Speak;
        public FontType Font;
        public string Face_Texture;
        public string Face_SpriteSheet;
    }
}
