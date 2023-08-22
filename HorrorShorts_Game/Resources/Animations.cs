using HorrorShorts_Game.Controls.Animations;
using HorrorShorts_Game.Controls.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HorrorShorts_Game.Resources
{
    public static class Animations
    {
        private static readonly Dictionary<AnimationType, Dictionary<string, AnimationData>> _loaded = new();
        public static Dictionary<string, AnimationData> Get(AnimationType anim)
        {
            if (!_loaded.TryGetValue(anim, out Dictionary<string, AnimationData> toReturn))
            {
                //todo: log advice
                Logger.Warning($"Animation '{anim}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(anim);
                if (!loaded) throw new ContentLoadException($"Error loading animation at runtime: {anim}");
                toReturn = _loaded[anim];
            }
            return toReturn;
        }

        private static readonly AnimationType[] AlwaysLoaded = new AnimationType[] { };
        private static readonly Type _enumType = typeof(AnimationType);


        public static void Init()
        {
            Logger.Advice("Initing animations...");
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init animation: {AlwaysLoaded[i]}");
            Logger.Advice("Init animations loaded!");
        }

        //public static
        public static void ReLoad(AnimationType[] animations)
        {
            Logger.Advice("Animations reloading...");
            List<AnimationType> animsToLoad = new();
            List<AnimationType> animsToUnload = new();

            AnimationType[] allAnims = (AnimationType[])Enum.GetValues(typeof(AnimationType));

            //Check Anims
            for (int i = 0; i < allAnims.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == allAnims[i]) != -1) continue;

                if (animations.Contains(allAnims[i]))
                {
                    if (animsToLoad.Contains(allAnims[i])) continue;
                    if (!_loaded.ContainsKey(allAnims[i]))
                        animsToLoad.Add(allAnims[i]);
                }
                else
                {
                    if (animsToUnload.Contains(allAnims[i])) continue;
                    if (_loaded.ContainsKey(allAnims[i]))
                        animsToUnload.Add(allAnims[i]);
                }
            }

            //todo: add in parallel task
            //Unload anims
            for (int i = 0; i < animsToUnload.Count; i++)
                if (!UnLoad(animsToUnload[i]))
                    throw new ContentLoadException($"Error unloading animations at loading time: {animsToUnload[i]}");

            //Load anims
            for (int i = 0; i < animsToLoad.Count; i++)
                if (!Load(animsToLoad[i]))
                    throw new ContentLoadException($"Error loading animations at loading time: {animsToLoad[i]}");

            Logger.Advice("Animations reloaded!");
        }

        private static bool Load(AnimationType animType)
        {
            try
            {
                if (_loaded.ContainsKey(animType))
                {
                    Logger.Warning($"Animation '{animType}' not need load because it is already loaded.");
                    return false;
                }

                string path = GetAnimationPath(animType);
                if (path == null)
                {
                    Logger.Warning($"Animation '{animType}' hasn't a loadeable path.");
                    return false; 
                }

                Animation_Serial ass = Core.Content.Load<Animation_Serial>(path);
                Dictionary<string, AnimationData> a = new();

                for (int j = 0; j < ass.Animations.Length; j++)
                    a.Add(ass.Animations[j].Name, new AnimationData(ass.Animations[j]));

                _loaded.Add(animType, a);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static bool UnLoad(AnimationType animType)
        {
            try
            {
                if (!_loaded.ContainsKey(animType))
                {
                    Logger.Warning($"Animation '{animType}' not need unload because it is already unloaded.");
                    return false;
                }

                string path = GetAnimationPath(animType);
                Core.Content.UnloadAsset(path); //todo: check
                _loaded.Remove(animType);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static string GetAnimationPath(AnimationType animType)
        {
            AnimationAttribute valueAttributes = GetAnimationAttribute(animType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("Data", "Animations", valueAttributes.ResourcePath);
            return path;
        }
        private static AnimationAttribute GetAnimationAttribute(AnimationType animType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(animType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            AnimationAttribute valueAttributes = (AnimationAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(AnimationAttribute), false)[0];
            return valueAttributes;
        }
    }
}
