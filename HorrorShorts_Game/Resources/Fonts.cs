using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Resources
{
    public static class Fonts
    {
        [Resource("Fonts\\PixelArial")]
        public static SpriteFont Arial { get; private set; }
        //[Resource("Fonts/PixelArial_Italic")]
        //public static SpriteFont Arial_Italic { get; private set; }
        //[Resource("Fonts/PixelArial_Bolds")]
        //public static SpriteFont Arial_Bolds { get; private set; }

        public static void Init()
        {
            Logger.Advice("Initing fonts...");
            Arial = Core.Content.Load<SpriteFont>("Fonts/PixelArial");
            Logger.Advice("Init fonts loaded!");
        }
        public static SpriteFont Get(FontType type)
        {
            return type switch
            {
                FontType.Arial => Arial,
                _ => throw new NotImplementedException("Not implemented Font Type: " + type)
            };
        }
    }
}
