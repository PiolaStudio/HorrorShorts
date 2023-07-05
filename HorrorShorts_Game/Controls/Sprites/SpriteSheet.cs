using Microsoft.Xna.Framework;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Sprites
{
    public class SpriteSheet
    {
        private Dictionary<string, Rectangle> Sources = new Dictionary<string, Rectangle>();

        public SpriteSheet()
        {
            Sources = new Dictionary<string, Rectangle>();
        }
        public SpriteSheet(SpriteSheet_Serial serial)
        {
            Sources = new Dictionary<string, Rectangle>();
            for (int i = 0; i < serial.Sheets.Length; i++)
                Sources.Add(serial.Sheets[i].Name, serial.Sheets[i].Source);
        }

        public void Add(string name, Rectangle source) => Sources.Add(name, source);
        public void Add(string name, int x, int y, int w, int h) => Sources.Add(name, new Rectangle(x, y, w, h));
        public Rectangle Get(string name) => Sources[name];
        public Rectangle Get(object name) => Sources[name.ToString()];
        public Dictionary<string, Rectangle> GetAll() => Sources;
    }
}
