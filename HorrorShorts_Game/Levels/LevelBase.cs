using HorrorShorts_Game.Levels;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Screens
{
    public abstract class LevelBase
    {
        protected TextureType[] _texturesRequired = Array.Empty<TextureType>();
        protected SpriteSheetType[] _spritesheetsRequired = Array.Empty<SpriteSheetType>();
        protected AnimationType[] _animationRequired = Array.Empty<AnimationType>();
        protected SoundType[] _soundRequired = Array.Empty<SoundType>();
        protected AtmosphereType[] _atmosphericRequired = Array.Empty<AtmosphereType>();
        protected SongType[] _songRequired = Array.Empty<SongType>();

        protected ScreenBase _screen;
        public ScreenBase Screen { get => _screen; }

        public bool Loaded { get; protected set; }

        protected readonly Dictionary<LayerType, bool> _usedLayer;
        public bool LayerIsUsed(LayerType layer) => _usedLayer[layer];

        public LevelBase()
        {
            _usedLayer = new();

            LayerType[] layers = Enum.GetValues<LayerType>();
            for (int i = 0; i < layers.Length; i++)
                _usedLayer.Add(layers[i], false);

            _usedLayer.Remove(LayerType.UI);
        }
        protected void LoadResources()
        {
            Textures.ReLoad(_texturesRequired, out List<SpriteSheetType> textureSheets);
            SpriteSheets.ReLoad(_spritesheetsRequired.Concat(textureSheets).ToArray());
            Animations.ReLoad(_animationRequired);
            Sounds.ReLoad(_soundRequired, out List<AtmosphereType> soundAtmospheric);
            Atmospheres.ReLoad(_atmosphericRequired.Concat(soundAtmospheric).ToArray());
            Songs.ReLoad(_songRequired);
        }
        //public enum Levels
        //{
        //    MainMenu,

        //}

        public virtual void LoadContent() 
        {
            LoadResources();
        }

        //private int _loadStage = 0;
        //private Task _loadTask;
        //public void UpdateLoad()
        //{
        //    if (_load == 0)
        //    {
        //        Textures.ReLoad(_texturesRequired, out List<SpriteSheetType> textureSheets);
        //    }
        //    else if (_load == 1)
        //    {
        //        SpriteSheets.ReLoad((SpriteSheetType[])_spritesheetsRequired.Concat(textureSheets));
        //    }
        //}
        public virtual void Update() { }

        public virtual void PreDraw() { }

        public void Draw(LayerType layer)
        {
            switch (layer)
            {
                case LayerType.Background9:
                    DrawBackground9();
                    break;
                case LayerType.Background8:
                    DrawBackground8();
                    break;
                case LayerType.Background7:
                    DrawBackground7();
                    break;
                case LayerType.Background6:
                    DrawBackground6();
                    break;
                case LayerType.Background5:
                    DrawBackground5();
                    break;
                case LayerType.Background4:
                    DrawBackground4();
                    break;
                case LayerType.Background3:
                    DrawBackground3();
                    break;
                case LayerType.Background2:
                    DrawBackground2();
                    break;
                case LayerType.Background1:
                    DrawBackground1();
                    break;
                case LayerType.Entities:
                    DrawEntities();
                    break;
                case LayerType.Frontground1:
                    DrawFrontground1();
                    break;
                case LayerType.Frontground2:
                    DrawFrontground2();
                    break;
                case LayerType.Frontground3:
                    DrawFrontground3();
                    break;
                case LayerType.Frontground4:
                    DrawFrontground4();
                    break;
                case LayerType.Frontground5:
                    DrawFrontground5();
                    break;
                case LayerType.Frontground6:
                    DrawFrontground6();
                    break;
            }
        }
        public virtual void DrawBackground9() { }
        public virtual void DrawBackground8() { }
        public virtual void DrawBackground7() { }
        public virtual void DrawBackground6() { }
        public virtual void DrawBackground5() { }
        public virtual void DrawBackground4() { }
        public virtual void DrawBackground3() { }
        public virtual void DrawBackground2() { }
        public virtual void DrawBackground1() { }
        public virtual void DrawEntities() { }
        public virtual void DrawFrontground1() { }
        public virtual void DrawFrontground2() { }
        public virtual void DrawFrontground3() { }
        public virtual void DrawFrontground4() { }
        public virtual void DrawFrontground5() { }
        public virtual void DrawFrontground6() { }
        public virtual void DrawUI() { }

        public virtual void Dispose() { }


        public virtual void ResetResolution() { }
        public virtual void ResetLocalization() { }
    }
}
