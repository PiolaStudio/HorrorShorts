using Microsoft.Xna.Framework.Audio;
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

namespace HorrorShorts_Game.Resources
{
    public static class Songs
    {
        private static readonly Dictionary<SongType, SoundEffect> _loaded = new();
        public static SongType[] AlwaysLoaded = new SongType[0];
        public static SoundEffect Get(SongType song) => _loaded[song];

        public static void Init() { }
        public static void ReLoad(SongType[] songs) 
        {
            List<SongType> songsToLoad = new List<SongType>();
            List<SongType> songsToUnload = new List<SongType>();
            SongType[] allSongs = (SongType[])Enum.GetValues(typeof(SongType));
            Type enumType = typeof(SongType);

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
            {
                if (!_loaded[songsToUnload[i]].IsDisposed)
                    _loaded[songsToUnload[i]].Dispose();
                _loaded.Remove(songsToUnload[i]);
            }

            //Load Sounds
            for (int i = 0; i < songsToLoad.Count; i++)
            {
                MemberInfo[] memberInfos = enumType.GetMember(songsToLoad[i].ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                SongAttribute valueAttributes = (SongAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(SongAttribute), false)[0];

                string path = valueAttributes.ResourcePath;
                SoundEffect se = Core.Content.Load<SoundEffect>(Path.Combine("Music", path));

                _loaded.Add(songsToLoad[i], se);
            }
        }
    }
}
