using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Resources
{
    public static class Sounds
    {
        private static readonly Dictionary<SoundType, SoundEffect> _loaded = new();
        public static SoundType[] AlwaysLoaded = new SoundType[0];
        public static SoundEffect Get(SoundType sound) => _loaded[sound];

        public static void Init()
        {

        }
        public static void ReLoad(SoundType[] sounds)
        {
            List<SoundType> soundsToLoad = new List<SoundType>();
            List<SoundType> soundsToUnload = new List<SoundType>();
            SoundType[] allSounds = (SoundType[])Enum.GetValues(typeof(SoundType));
            Type enumType = typeof(SoundType);

            //Check sounds
            for (int i = 0; i < allSounds.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == sounds[i]) != -1) continue;

                if (sounds.Contains(allSounds[i]))
                {
                    if (!_loaded.ContainsKey(allSounds[i]))
                        soundsToLoad.Add(allSounds[i]);
                }
                else
                {
                    if (_loaded.ContainsKey(allSounds[i]))
                        soundsToUnload.Add(allSounds[i]);
                }
            }

            //todo: add in parallel task
            //Unload Sounds
            for (int i = 0; i < soundsToUnload.Count; i++)
            {
                if (!_loaded[soundsToUnload[i]].IsDisposed)
                    _loaded[soundsToUnload[i]].Dispose();
                _loaded.Remove(soundsToUnload[i]);
            }

            //Load Sounds
            for (int i = 0; i < soundsToLoad.Count; i++)
            {
                MemberInfo[] memberInfos = enumType.GetMember(soundsToLoad[i].ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                SoundAttribute valueAttributes = (SoundAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(SoundAttribute), false)[0];

                string path = valueAttributes.ResourcePath;
                SoundEffect se = Core.Content.Load<SoundEffect>(Path.Combine("SoundEffects", path));

                _loaded.Add(soundsToLoad[i], se);
            }
        }
    }
}
