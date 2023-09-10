using Microsoft.Xna.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game
{
    public class Settings
    {
        public static readonly Rectangle NativeResolution = new(0, 0, 320, 320);
#if DESKTOP
        public int WindowX;
        public int WindowY;
#endif

        public int ResolutionWidth = 640;
        public int ResolutionHeight = 640;
        public bool FullScreen = false;

        //Audio
        public float GeneralVolume = 1f;
        public float MusicVolume = 1f;
        public float EffectsVolume = 1f;
        public float AtmosphereVolume = 1f;

        public float MusicRealVolume = 1f;
        public float EffectsRealVolume = 1f;
        public float AtmosphereRealVolume = 1f;

        //Language
        public LanguageType Language = LanguageType.English;
    }
}
