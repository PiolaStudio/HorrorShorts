using HorrorShorts.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Resources
{
    public static class Textures
    {
        public static Texture2D Pixel { get; private set; }
        [Resource("Textures/Characters/Mario")]
        public static Texture2D Mario { get; private set; }


        public static string[] AlwaysLoadedTextures = new string[]
        {
            nameof(Pixel)
        };

        public static void Init()
        {
            Pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[1] { Color.White });
        }
        public static void ReLoad(string[] textures)
        {
            List<string> texturesToLoad = new List<string>();
            List<string> texturesToUnload = new List<string>();
            List<string> sheetsToLoad = new List<string>();
            List<string> sheetsToUnload = new List<string>();

            //Check textures
            PropertyInfo[] props = typeof(Textures).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(Texture2D)) continue;
                if (Array.FindIndex(AlwaysLoadedTextures, x => x == props[i].Name) != -1) continue;

                //int tIndex = textures.FindIndex(x => x == fields[i].Name);
                Texture2D t = (Texture2D)props[i].GetValue(null);

                PropertyInfo sheetPropInfo = typeof(Textures).GetProperty(props[i].Name + "_Sheet");
                if (textures.Contains(props[i].Name))
                {
                    if (t == null || t.IsDisposed)
                    {
                        texturesToLoad.Add(props[i].Name);
                        if (sheetPropInfo != null) sheetsToLoad.Add(sheetPropInfo.Name);
                    }
                }
                else
                {
                    if (t != null && !t.IsDisposed)
                    {
                        texturesToUnload.Add(props[i].Name);
                        if (sheetPropInfo != null) sheetsToUnload.Add(sheetPropInfo.Name);
                    }
                }
            }

            //todo: add in parallel task
            //Unload textures
            for (int i = 0; i < texturesToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(Textures).GetProperty(texturesToUnload[i]);
                Texture2D t = (Texture2D)propInfo.GetValue(null);
                if (!t.IsDisposed) t.Dispose();
                propInfo.SetValue(null, null);
            }
            ////Unload sheets
            //for (int i = 0; i < sheetsToUnload.Count; i++)
            //{
            //    PropertyInfo fi = typeof(Textures).GetProperty(sheetsToUnload[i]);
            //    fi.SetValue(null, null);
            //}

            //Load textures
            for (int i = 0; i < texturesToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(Textures).GetProperty(texturesToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;
                Texture2D t = Core.Content.Load<Texture2D>(path);
                propInfo.SetValue(null, t);
            }
            ////Load sheets
            //for (int i = 0; i < sheetsToLoad.Count; i++)
            //{
            //    FieldInfo fi = typeof(Textures).GetField(sheetsToLoad[i]);
            //    string path = ((DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute), true)).Description;
            //    SpriteSheet s = SpriteSheetSerial.Load(path).ToSpriteSheet();
            //    fi.SetValue(null, s);
            //}
        }
    }
}
