using HorrorShorts_Game.Audio.Atmosphere;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Resources;
using Resources.Attributes;
using Resources.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Resources
{
    public class Atmospheres
    {
        private static readonly Dictionary<AtmosphereType, AtmosphereSound> _loaded = new();
        private static readonly AtmosphereType[] AlwaysLoaded = new AtmosphereType[] { };
        public static AtmosphereSound Get(AtmosphereType sound)
        {
            if (!_loaded.TryGetValue(sound, out AtmosphereSound toReturn))
            {
                Logger.Warning($"atmosphere-sound '{sound}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(sound);
                if (!loaded) throw new ContentLoadException($"Error loading atmosphere-sound at runtime: {sound}");
                toReturn = _loaded[sound];
            }
            return toReturn;
        }
        private static readonly Type _enumType = typeof(AtmosphereType);

        public static void Init() 
        {
            Logger.Advice("Initing atmosphere...");
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init atmosphere-sound: {AlwaysLoaded[i]}");
            Logger.Advice("Init atmosphere loaded!");

        }
        public static void ReLoad(AtmosphereType[] sounds)
        {
            Logger.Advice("Atmosphere-sound reloading...");
            List<AtmosphereType> atmospheresToLoad = new();
            List<AtmosphereType> atmospheresToUnload = new();
            AtmosphereType[] allAtmospheres = (AtmosphereType[])Enum.GetValues(_enumType);

            //Check Atmosphere-sounds
            for (int i = 0; i < allAtmospheres.Length; i++)
            {   
                if (Array.FindIndex(AlwaysLoaded, x => x == allAtmospheres[i]) != -1) continue;

                if (sounds.Contains(allAtmospheres[i]))
                {
                    if (!_loaded.ContainsKey(allAtmospheres[i]))
                        atmospheresToLoad.Add(allAtmospheres[i]);
                }
                else
                {
                    if (_loaded.ContainsKey(allAtmospheres[i]))
                        atmospheresToUnload.Add(allAtmospheres[i]);
                }
            }

            //Unload Sounds
            for (int i = 0; i < atmospheresToUnload.Count; i++)
                if (!UnLoad(atmospheresToUnload[i]))
                    throw new ContentLoadException($"Error loading atmosphere-sound at loading time: {atmospheresToUnload[i]}");

            //Load Sounds
            for (int i = 0; i < atmospheresToLoad.Count; i++)
                if (!Load(atmospheresToLoad[i]))
                    throw new ContentLoadException($"Error loading atmosphere-sound at loading time: {atmospheresToLoad[i]}");

            Logger.Advice("Atmosphere-sound reloaded!");
        }

        private static bool Load(AtmosphereType atmosphereType)
        {
            try
            {
                if (_loaded.ContainsKey(atmosphereType))
                {
                    Logger.Warning($"Atmosphere-sound '{atmosphereType}' not need load because it is already loaded.");
                    return false;
                }

                string path = GetAtmospherePath(atmosphereType);
                if (path == null)
                {
                    Logger.Warning($"Atmosphere-sound '{atmosphereType}' hasn't a loadeable path.");
                    return false;
                }

                AtmosphereSound_Serial a_se_serial = Core.Content.Load<AtmosphereSound_Serial>(path);
                AtmosphereSound a_se = new(a_se_serial);
                _loaded.Add(atmosphereType, a_se);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static bool UnLoad(AtmosphereType atmosphereType)
        {
            try
            {
                if (!_loaded.ContainsKey(atmosphereType))
                {
                    Logger.Warning($"Atmosphere-sound '{atmosphereType}' not need unload because it is already unloaded.");
                    return false;
                }

                string path = GetAtmospherePath(atmosphereType);
                Core.Content.UnloadAsset(path); //todo: check
                _loaded.Remove(atmosphereType);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex);
                return false;
            }
        }

        private static string GetAtmospherePath(AtmosphereType atmosphereType)
        {
            AtmosphereAttribute valueAttributes = GetAtmosphereAttribute(atmosphereType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("Data", "Atmosphere", valueAttributes.ResourcePath);
            return path;
        }
        private static AtmosphereAttribute GetAtmosphereAttribute(AtmosphereType atmosphereType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(atmosphereType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            AtmosphereAttribute valueAttributes = (AtmosphereAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(AtmosphereAttribute), false)[0];
            return valueAttributes;
        }
    }
}
