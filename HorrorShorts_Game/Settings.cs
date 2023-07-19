using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game
{
    public class Settings
    {
        public static readonly Rectangle NativeResolution = new(0, 0, 640, 640);
#if DESKTOP
        public int WindowX;
        public int WindowY;
#endif
        public int ResolutionWidth;
        public int ResolutionHeight;
        public bool FullScreen;

        //Audio
        public float GeneralVolume = 1f;
        public float MusicVolume = 1f;
        public float EffectsVolume = 1f;

        public float MusicRealVolume = 1f;
        public float EffectsRealVolume = 1f;
    }
}
