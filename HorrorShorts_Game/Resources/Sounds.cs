using HorrorShorts_Game.Controls.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
        private static readonly SoundType[] AlwaysLoaded = new SoundType[] 
        { 
            SoundType.OptionChange, 
            SoundType.OptionSelect 
        };
        public static SoundEffect Get(SoundType sound)
        {
            if (!_loaded.TryGetValue(sound, out SoundEffect toReturn))
            {
                //todo: log advice
                bool loaded = Load(sound);
                if (!loaded) throw new ContentLoadException($"Error loading resource at runtime: {sound}");
                toReturn = _loaded[sound];
            }
            return toReturn;
        }

        private static readonly Type _enumType = typeof(SoundType);


        public static void Init()
        {
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init resource: {AlwaysLoaded[i]}");
        }
        public static void ReLoad(SoundType[] sounds)
        {
            List<SoundType> soundsToLoad = new();
            List<SoundType> soundsToUnload = new();
            SoundType[] allSounds = (SoundType[])Enum.GetValues(typeof(SoundType));

            //Check sounds
            for (int i = 0; i < allSounds.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == allSounds[i]) != -1) continue;

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
                if (!UnLoad(soundsToUnload[i]))
                {
                    /*todo: log advice or throw exception*/
                    throw new ContentLoadException($"Error loading resource at loading time: {soundsToUnload[i]}");
                }

            //Load Sounds
            for (int i = 0; i < soundsToLoad.Count; i++)
                if (!Load(soundsToLoad[i]))
                {
                    /*todo: log advice or throw exception*/
                    throw new ContentLoadException($"Error loading resource at loading time: {soundsToLoad[i]}");
                }
        }

        private static bool Load(SoundType soundType)
        {
            try
            {
                if (_loaded.ContainsKey(soundType)) return false; //todo: log advice

                string path = GetSoundPath(soundType);
                if (path == null) return false;  //todo: log advice

                SoundEffect se = Core.Content.Load<SoundEffect>(path);
                _loaded.Add(soundType, se);
                return true;
            }
            catch (Exception)
            {
                //todo: log ex
                return false;
            }
        }
        private static bool UnLoad(SoundType soundType)
        {
            try
            {
                if (!_loaded.ContainsKey(soundType)) return false; //todo: log advice

                if (!_loaded[soundType].IsDisposed)
                    _loaded[soundType].Dispose();
                _loaded.Remove(soundType);
                return true;
            }
            catch (Exception)
            {
                //todo: log ex
                return false;
            }
        }

        private static string GetSoundPath(SoundType soundType)
        {
            SoundAttribute valueAttributes = GetSoundAttribute(soundType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("SoundEffects", valueAttributes.ResourcePath);
            return path;
        }
        private static SoundAttribute GetSoundAttribute(SoundType soundType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(soundType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            SoundAttribute valueAttributes = (SoundAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(SoundAttribute), false)[0];
            return valueAttributes;
        }
    }
}
