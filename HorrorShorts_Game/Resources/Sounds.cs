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
                Logger.Warning($"Sound '{sound}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(sound);
                if (!loaded) throw new ContentLoadException($"Error loading sound at runtime: {sound}");
                toReturn = _loaded[sound];
            }
            return toReturn;
        }

        private static readonly Type _enumType = typeof(SoundType);


        public static void Init()
        {
            Logger.Advice("Initing sounds...");
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init sound: {AlwaysLoaded[i]}");
            Logger.Advice("Init sounds loaded!");
        }
        public static void ReLoad(SoundType[] sounds)
        {
            Logger.Advice("Sounds reloading...");
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
                    throw new ContentLoadException($"Error loading sound at loading time: {soundsToUnload[i]}");

            //Load Sounds
            for (int i = 0; i < soundsToLoad.Count; i++)
                if (!Load(soundsToLoad[i]))
                    throw new ContentLoadException($"Error loading sound at loading time: {soundsToLoad[i]}");

            Logger.Advice("Sounds reloaded!");
        }

        private static bool Load(SoundType soundType)
        {
            try
            {
                if (_loaded.ContainsKey(soundType))
                {
                    Logger.Warning($"Sound '{soundType}' not need load because it is already loaded.");
                    return false;
                }

                string path = GetSoundPath(soundType);
                if (path == null)
                {
                    Logger.Warning($"Sound '{soundType}' hasn't a loadeable path.");
                    return false;
                }

                SoundEffect se = Core.Content.Load<SoundEffect>(path);
                _loaded.Add(soundType, se);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static bool UnLoad(SoundType soundType)
        {
            try
            {
                if (!_loaded.ContainsKey(soundType))
                {
                    Logger.Warning($"Sound '{soundType}' not need unload because it is already unloaded.");
                    return false;
                }

                if (!_loaded[soundType].IsDisposed)
                    _loaded[soundType].Dispose();
                _loaded.Remove(soundType);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex);
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
