using HorrorShorts.Controls.Animations;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HorrorShorts.Resources
{
    public static class Animations
    {
        [Resource("Data/Animations/Megaman")]
        public static Dictionary<string, AnimationData> Megaman { get; private set; }

        public static void Init()
        {

        }
        public static void ReLoad(params string[] animations)
        {
            List<string> animsToLoad = new();
            List<string> animsToUnload = new();

            //Check Anims
            PropertyInfo[] props = typeof(Animations).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(Dictionary<string, AnimationData>)) continue;

                Dictionary<string, AnimationData> a = (Dictionary<string, AnimationData>)props[i].GetValue(null);

                if (animations.Contains(props[i].Name))
                {
                    if (a == null) animsToLoad.Add(props[i].Name);
                }
                else
                {
                    if (a != null) animsToUnload.Add(props[i].Name);
                }
            }

            //todo: add in parallel task
            //Unload anims
            for (int i = 0; i < animsToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(Animations).GetProperty(animsToUnload[i]);
                propInfo.SetValue(null, null);
            }

            //Load anims
            for (int i = 0; i < animsToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(Animations).GetProperty(animsToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;

                Animation_Serial ass = Core.Content.Load<Animation_Serial>(path);
                Dictionary<string, AnimationData> a = new();

                for (int j = 0; j < ass.Animations.Length; j++)
                    a.Add(ass.Animations[j].Name, new AnimationData(ass.Animations[j]));

                propInfo.SetValue(null, a);
            }
        }
    }
}
