using HorrorShorts_Game.Controls.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Resources
{
    public static class SpriteSheets
    {
        private static readonly Dictionary<SpriteSheetType, SpriteSheet> _loaded = new();
        public static SpriteSheet Get(SpriteSheetType sheet)
        {
            if (!_loaded.TryGetValue(sheet, out SpriteSheet toReturn))
            {
                Logger.Warning($"SpriteSheet '{sheet}' is not loaded in 'RELOAD' time. Loading in game time...");
                bool loaded = Load(sheet);
                if (!loaded) throw new ContentLoadException($"Error loading spritesheet at game time: {sheet}");
                toReturn = _loaded[sheet];
            }
            return toReturn;
        }
        public static SpriteSheet Get(TextureType texture)
        {
            TextureAttribute valueAttributes = Textures.GetTextureAttribute(texture);
            if (valueAttributes.Sheet == null) return null;
            SpriteSheetType sheet = valueAttributes.Sheet.Value;
            return Get(sheet);
        }

        private static readonly SpriteSheetType[] AlwaysLoaded = new SpriteSheetType[] 
        {
            SpriteSheetType.UIControls,
        };

        private static readonly Type _enumType = typeof(SpriteSheetType);

        public static void Init()
        {
            Logger.Advice("Initing spritesheets...");
            for (int i = 0; i < AlwaysLoaded.Length; i++)
                if (!Load(AlwaysLoaded[i]))
                    throw new ContentLoadException($"Error loading a Init spritesheet: {AlwaysLoaded[i]}");
            Logger.Advice("Init spritesheets loaded!");
        }
        public static void ReLoad(SpriteSheetType[] sheets)
        {
            Logger.Advice("Spritesheets reloading...");
            List<SpriteSheetType> sheetsToLoad = new();
            List<SpriteSheetType> sheetsToUnload = new();

            SpriteSheetType[] allSheets = (SpriteSheetType[])Enum.GetValues(typeof(SpriteSheetType));

            //Check sheets
            for (int i = 0; i < allSheets.Length; i++)
            {
                if (Array.FindIndex(AlwaysLoaded, x => x == allSheets[i]) != -1) continue;

                if (sheets.Contains(allSheets[i]))
                {
                    if (sheetsToLoad.Contains(allSheets[i])) continue;
                    if (!_loaded.ContainsKey(allSheets[i]))
                        sheetsToLoad.Add(allSheets[i]);
                }
                else
                {
                    if (sheetsToUnload.Contains(allSheets[i])) continue;
                    if (_loaded.ContainsKey(allSheets[i]))
                        sheetsToUnload.Add(allSheets[i]);
                }
            }

            //todo: add in parallel task
            //Unload sheets
            for (int i = 0; i < sheetsToUnload.Count; i++)
                if (!UnLoad(sheetsToUnload[i]))
                    throw new ContentLoadException($"Error unloading spritesheet at loading time: {sheetsToUnload[i]}");


            //Load sheets
            for (int i = 0; i < sheetsToLoad.Count; i++)
                if (!Load(sheetsToLoad[i]))
                    throw new ContentLoadException($"Error loading spritesheet at loading time: {sheetsToLoad[i]}");

            Logger.Advice("Spritesheets reloaded!");
        }

        private static bool Load(SpriteSheetType sheetType)
        {
            try
            {
                if (_loaded.ContainsKey(sheetType))
                {
                    Logger.Warning($"SpriteSheet '{sheetType}' not need load because it is already loaded.");
                    return false; 
                }

                string path = GetSheetPath(sheetType);
                if (path == null)
                {
                    Logger.Warning($"SpriteSheet '{sheetType}' hasn't a loadeable path.");
                    return false; 
                }

                SpriteSheet_Serial ss_serial = Core.Content.Load<SpriteSheet_Serial>(path);
                SpriteSheet ss = new(ss_serial);
                _loaded.Add(sheetType, ss);
                return true;
            }
            catch (Exception ex) 
            { 
                Logger.Error(ex);
                return false; 
            }
        }
        private static bool UnLoad(SpriteSheetType sheetType)
        {
            try
            {
                if (!_loaded.ContainsKey(sheetType))
                {
                    Logger.Warning($"SpriteSheet '{sheetType}' not need unload because it is already unloaded.");
                    return false;
                }

                string path = GetSheetPath(sheetType);
                Core.Content.UnloadAsset(path); //todo: check
                _loaded.Remove(sheetType);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex);
                return false; 
            }
        }
        private static string GetSheetPath(SpriteSheetType sheetType)
        {
            SpriteSheetAttribute valueAttributes = GetSheetAttribute(sheetType);
            if (valueAttributes.ResourcePath == null) return null;
            string path = Path.Combine("Data", "SpriteSheets", valueAttributes.ResourcePath);
            return path;
        }
        private static SpriteSheetAttribute GetSheetAttribute(SpriteSheetType sheetType)
        {
            MemberInfo[] memberInfos = _enumType.GetMember(sheetType.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == _enumType);
            SpriteSheetAttribute valueAttributes = (SpriteSheetAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(SpriteSheetAttribute), false)[0];
            return valueAttributes; 
        }
    }
}
