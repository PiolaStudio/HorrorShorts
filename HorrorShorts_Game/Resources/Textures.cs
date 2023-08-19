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
                //todo: log advice
                bool loaded = Load(texture);
                if (!loaded) throw new Microsoft.Xna.Framework.Content.ContentLoadException($"Error loading resource at runtime: {texture}");
                toReturn = _loaded[texture];
            }

            return toReturn;
        }

        public static Texture2D Pixel { get; private set; }

        private static readonly Type _enumType = typeof(TextureType);


        public static void Init()
        {
            Pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[1] { Color.White });
            _loaded.Add(TextureType.Pixel, Pixel);

            for (int i = 1; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init resource: {AlwaysLoaded[i]}");
        }
        public static void ReLoad(TextureType[] textures, out List<SpriteSheetType> sheetsToLoad)
        {
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
            {
                if (!UnLoad(texturesToUnload[i]))
                { 
                    /*todo: log advice or throw exception*/
                        throw new ContentLoadException($"Error loading resource at loading time: {texturesToUnload[i]}");
                }
            }

            //Load textures
            for (int i = 0; i < texturesToLoad.Count; i++)
                if (!Load(texturesToLoad[i]))
                {
                    /*todo: log advice or throw exception*/
                    throw new ContentLoadException($"Error loading resource at loading time: {texturesToLoad[i]}");
                }

        }

        private static bool Load(TextureType textureType)
        {
            try
            {
                if (_loaded.ContainsKey(textureType)) return false;  //todo: log advice

                string path = GetTexturePath(textureType);
                if (path == null) return false;  //todo: log advice

                Texture2D t = Core.Content.Load<Texture2D>(path);
                _loaded.Add(textureType, t);
                return true;
            }
            catch (Exception ex)
            {
                //todo: log ex
                return false;
            }
        }
        private static bool UnLoad(TextureType textureType)
        {
            try
            {
                if (!_loaded.ContainsKey(textureType)) return false; //todo: log advice

                if (!_loaded[textureType].IsDisposed)
                    _loaded[textureType].Dispose();
                _loaded.Remove(textureType);

                return true;
            }
            catch (Exception ex)
            {
                //todo: log ex
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
