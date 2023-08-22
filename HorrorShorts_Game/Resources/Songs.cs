using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HorrorShorts_Game.Resources
{
    public static class Songs
    {
        private static readonly Dictionary<SongType, SoundEffect> _loaded = new();
        public static readonly SongType[] AlwaysLoaded = new SongType[0];
        public static SoundEffect Get(SongType song)
        {
            if (!_loaded.TryGetValue(song, out SoundEffect toReturn))
            {
                Logger.Warning($"Song '{song}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(song);
                if (!loaded) throw new ContentLoadException($"Error loading song at runtime: {song}");
                toReturn = _loaded[song];
            }
            return toReturn;
        }
        private static readonly Type _enumType = typeof(SongType);

        public static void Init() 
        {
            Logger.Advice("Initing songs...");
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init song: {AlwaysLoaded[i]}");
            Logger.Advice("Init songs loaded!");
        }
        public static void ReLoad(SongType[] songs) 
        {
            Logger.Advice("Songs reloading...");
            List<SongType> songsToLoad = new List<SongType>();
            List<SongType> songsToUnload = new List<SongType>();
            SongType[] allSongs = (SongType[])Enum.GetValues(typeof(SongType));

            //Check songs
            for (int i = 0; i < allSongs.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == songs[i]) != -1) continue;

                if (songs.Contains(allSongs[i]))
                {
                    if (!_loaded.ContainsKey(allSongs[i]))
                        songsToLoad.Add(allSongs[i]);
                }
                else
                {
                    if (!_loaded.ContainsKey(allSongs[i]))
                        songsToUnload.Add(allSongs[i]);
                }
            }

            //todo: add in parallel task
            //Unload Songs
            for (int i = 0; i < songsToUnload.Count; i++)
                if (!UnLoad(songsToUnload[i]))
                    throw new ContentLoadException($"Error unloading songs at loading time: {songsToUnload[i]}");

            //Load Sounds
            for (int i = 0; i < songsToLoad.Count; i++)
                if (!Load(songsToLoad[i]))
                    throw new ContentLoadException($"Error loading songs at loading time: {songsToLoad[i]}");

            Logger.Advice("Songs reloaded!");
        }

        private static bool Load(SongType songType)
        {
            try
            {
                if (_loaded.ContainsKey(songType))
                {
                    Logger.Warning($"Song '{songType}' not need load because it is already loaded.");
                    return false;
                }

                string path = GetSongPath(songType);
                if (path == null)
                {
                    Logger.Warning($"Song '{songType}' hasn't a loadeable path.");
                    return false;
                }

                SoundEffect se = Core.Content.Load<SoundEffect>(path);
                _loaded.Add(songType, se);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static bool UnLoad(SongType songType)
        {
            try
            {
                if (!_loaded.ContainsKey(songType))
                {
                    Logger.Warning($"Song '{songType}' not need unload because it is already unloaded.");
                    return false;
                }

                if (!_loaded[songType].IsDisposed)
                    _loaded[songType].Dispose();
                _loaded.Remove(songType);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static string GetSongPath(SongType songType)
        {
            SongAttribute valueAttributes = GetSongAttribute(songType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("Music", valueAttributes.ResourcePath);
            return path;
        }
        private static SongAttribute GetSongAttribute(SongType songType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(songType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            SongAttribute valueAttributes = (SongAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(SongAttribute), false)[0];
            return valueAttributes;
        }
    }
}
