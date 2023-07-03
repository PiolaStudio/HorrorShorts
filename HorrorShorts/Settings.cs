using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts
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
    }
}
