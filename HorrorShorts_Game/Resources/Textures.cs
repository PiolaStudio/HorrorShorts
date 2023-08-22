using HorrorShorts_Game.Controls.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using SharpFont.PostScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HorrorShorts_Game.Resources
{
    public static class Textures
    {
        private static readonly Dictionary<TextureType, Texture2D> _loaded = new();
        private static readonly TextureType[] AlwaysLoaded = new TextureType[]
        {
            TextureType.Pixel,
            TextureType.DialogMenu
        };
        public static Texture2D Get(TextureType texture)
        {
            if (!_loaded.TryGetValue(texture, out Texture2D toReturn))
            {
                Logger.Warning($"Texture '{texture}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(texture);
                if (!loaded) throw new ContentLoadException($"Error loading texture at game time: {texture}");
                toReturn = _loaded[texture];
            }

            return toReturn;
        }

        public static Texture2D Pixel { get; private set; }

        private static readonly Type _enumType = typeof(TextureType);


        public static void Init()
        {
            Logger.Advice("Initing textures...");
            Pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[1] { Color.White });
            _loaded.Add(TextureType.Pixel, Pixel);

            for (int i = 1; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init texture: {AlwaysLoaded[i]}");
            Logger.Advice("Init textures loaded!");
        }
        public static void ReLoad(TextureType[] textures, out List<SpriteSheetType> sheetsToLoad)
        {
            Logger.Advice("Textures reloading...");
            List<TextureType> texturesToLoad = new();
            List<TextureType> texturesToUnload = new();
            sheetsToLoad = new();

            TextureType[] allTextures = (TextureType[])Enum.GetValues(typeof(TextureType));

            //Check textures
            for (int i = 0; i < allTextures.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == allTextures[i]) != -1) continue;
                TextureAttribute attribute = GetTextureAttribute(allTextures[i]);
                bool isSheet = attribute.Sheet.HasValue;

                if (textures.Contains(allTextures[i]))
                {
                    if (!_loaded.ContainsKey(allTextures[i]))
                    {
                        texturesToLoad.Add(allTextures[i]);
                        if (isSheet) sheetsToLoad.Add(attribute.Sheet.Value);
                    }
                }
                else
                {
                    if (_loaded.ContainsKey(allTextures[i]))
                    {
                        texturesToUnload.Add(allTextures[i]);
                        //if (isSheet) sheetsToUnload.Add(attribute.Sheet.Value);
                    }
                }
            }

            //todo: add in parallel task
            //Unload textures
            for (int i = 0; i < texturesToUnload.Count; i++)
                if (!UnLoad(texturesToUnload[i]))
                    throw new ContentLoadException($"Error unloading texture at loading time: {texturesToUnload[i]}");

            //Load textures
            for (int i = 0; i < texturesToLoad.Count; i++)
                if (!Load(texturesToLoad[i]))
                    throw new ContentLoadException($"Error loading texture at loading time: {texturesToLoad[i]}");

            Logger.Advice("Textures reloaded!");
        }

        private static bool Load(TextureType textureType)
        {
            try
            {
                if (_loaded.ContainsKey(textureType))
                {
                    Logger.Warning($"Texture '{textureType}' not need load because it is already loaded.");
                    return false;
                }

                string path = GetTexturePath(textureType);
                if (path == null)
                {
                    Logger.Warning($"Texture '{textureType}' hasn't a loadeable path.");
                    return false;
                }

                Texture2D t = Core.Content.Load<Texture2D>(path);
                _loaded.Add(textureType, t);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static bool UnLoad(TextureType textureType)
        {
            try
            {
                if (!_loaded.ContainsKey(textureType))
                {
                    Logger.Warning($"Texture '{textureType}' not need unload because it is already unloaded.");
                    return false; 
                }

                if (!_loaded[textureType].IsDisposed)
                    _loaded[textureType].Dispose();
                _loaded.Remove(textureType);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex);
                return false;
            }
        }
        public static string GetTexturePath(TextureType textureType)
        {
            TextureAttribute valueAttributes = GetTextureAttribute(textureType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("Textures", valueAttributes.ResourcePath);
            return path;
        }
        public static TextureAttribute GetTextureAttribute(TextureType textureType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(textureType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            TextureAttribute valueAttributes = (TextureAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(TextureAttribute), false)[0];
            return valueAttributes;
        }
    }
}
