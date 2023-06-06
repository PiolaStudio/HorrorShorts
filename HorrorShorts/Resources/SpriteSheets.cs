using HorrorShorts.Data;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Resources
{
    public static class SpriteSheets
    {
        [ResourceAttribute("Data/SpriteSheets/Mario")]
        public static SpriteSheet Mario { get; private set; }

        public static void Init()
        {
        }
        public static void ReLoad(string[] sheets)
        {
            List<string> sheetsToLoad = new List<string>();
            List<string> sheetsToUnload = new List<string>();

            //Check sheets
            PropertyInfo[] props = typeof(SpriteSheets).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(SpriteSheet)) continue;

                SpriteSheet s = (SpriteSheet)props[i].GetValue(null);

                if (sheets.Contains(props[i].Name))
                {
                    if (s == null)
                        sheetsToLoad.Add(props[i].Name);
                }
                else
                {
                    if (s != null)
                        sheetsToUnload.Add(props[i].Name);
                }
            }

            //todo: add in parallel task
            //Unload sheets
            for (int i = 0; i < sheetsToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(SpriteSheets).GetProperty(sheetsToUnload[i]);
                propInfo.SetValue(null, null);
            }

            //Load sheets
            for (int i = 0; i < sheetsToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(SpriteSheets).GetProperty(sheetsToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;

                SpriteSheet_Serial ss = Core.Content.Load<SpriteSheet_Serial>(path);
                SpriteSheet s = new(ss);
                propInfo.SetValue(null, s);
            }
        }
    }
}
