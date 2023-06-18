using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Resources
{
    public static class Sounds
    {
        [Resource("SoundEffects/Speak/Speak1")]
        public static SoundEffect Speak1 { get; private set; }
        [Resource("SoundEffects/Speak/Speak2")]
        public static SoundEffect Speak2 { get; private set; }

        public static string[] AlwaysLoaded = new string[0];

        public static void Init()
        {

        }
        public static void ReLoad(string[] sounds)
        {
            List<string> soundsToLoad = new List<string>();
            List<string> soundsToUnload = new List<string>();

            //Check textures
            PropertyInfo[] props = typeof(Sounds).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(SoundEffect)) continue;
                if (Array.FindIndex(AlwaysLoaded, x => x == props[i].Name) != -1) continue;

                SoundEffect se = (SoundEffect)props[i].GetValue(null);

                if (sounds.Contains(props[i].Name))
                {
                    if (se == null || se.IsDisposed)
                        soundsToLoad.Add(props[i].Name);
                }
                else
                {
                    if (se != null && !se.IsDisposed)
                        soundsToUnload.Add(props[i].Name);
                }
            }

            //todo: add in parallel task
            //Unload Sounds
            for (int i = 0; i < soundsToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(Sounds).GetProperty(soundsToUnload[i]);
                SoundEffect se = (SoundEffect)propInfo.GetValue(null);
                if (!se.IsDisposed) se.Dispose();
                propInfo.SetValue(null, null);
            }

            //Load Sounds
            for (int i = 0; i < soundsToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(Sounds).GetProperty(soundsToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;
                SoundEffect se = Core.Content.Load<SoundEffect>(path);
                propInfo.SetValue(null, se);
            }
        }
    }
}
