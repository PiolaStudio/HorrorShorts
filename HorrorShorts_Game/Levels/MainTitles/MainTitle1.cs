using CppNet;
using HorrorShorts_Game.Algorithms.Tweener;
using HorrorShorts_Game.Audio.Atmosphere;
using HorrorShorts_Game.Controls.Animations;
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Screens.MainTitles
{
    public class MainTitle1 : MainTitleBase
    {
        private Sprite _sky;
        private Sprite _moon;
        private Sprite _tree;
        private Sprite _ground1;
        private Sprite _wall;

        private AnimationSystem _moonAnim;
        private AnimationSystem[] _fireFlyAnims;

        private const float FIELD_VOLUME_MIN = 0.15f;
        private const float FIELD_VOLUME_MAX = 0.6f;

        private List<FireFly>[] _fireFlies;
        private Tween<float> _fireFlyMovementY;

        public MainTitle1() 
        {
            _texturesRequired = _texturesRequired.Concat(
                new TextureType[] {
                    TextureType.MainTitle,
                    TextureType.MainTitle1,
                }).ToArray();

            _spritesheetsRequired = _spritesheetsRequired.Concat(
                new SpriteSheetType[] {
                    SpriteSheetType.MainTitle1,
                }).ToArray();

            _animationRequired = _animationRequired.Concat(
                new AnimationType[] {
                    AnimationType.MainTitle1,
                }).ToArray();

            _soundRequired = _soundRequired.Concat(
                new SoundType[] {
                    SoundType.Field1_Atmosphere
                }).ToArray();

            _atmosphericRequired = _atmosphericRequired.Concat(
                new AtmosphereType[] {
                    AtmosphereType.Field1,
                }).ToArray();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _usedLayer[LayerType.Background9] = _usedLayer[LayerType.Background4] = 
                _usedLayer[LayerType.Background3] = _usedLayer[LayerType.Background2] = true;

            Texture2D sheetTexture = Textures.Get(TextureType.MainTitle1);
            SpriteSheet sheetData = SpriteSheets.Get(SpriteSheetType.MainTitle1);
            Dictionary<string, AnimationData> animsData = Animations.Get(AnimationType.MainTitle1);

            _sky = new(sheetTexture, Point.Zero, sheetData.Get("Sky"));

            _moonAnim = new();
            _moonAnim.SetAnimation(animsData["MoonLight"]);
            _moonAnim.Bucle = true;
            _moonAnim.Play();
            _moon = new(sheetTexture, 208, 46, _moonAnim.Source);

            _tree = new(sheetTexture, 0, 35, sheetData.Get("Tree"));
            _ground1 = new(sheetTexture, 0, 240, sheetData.Get("Ground1"));
            _wall = new(sheetTexture, 0, 272, sheetData.Get("Wall"));

            _fireFlyAnims = new AnimationSystem[3];
            _fireFlies = new List<FireFly>[3];
            for (int i = 0; i < 3; i++)
            {
                AnimationSystem _fireFlyAnim = new();
                _fireFlyAnim.SetAnimation(animsData[$"FireFly{i + 1}_Light"]);
                _fireFlyAnim.BucleType = BucleType.PingPong;
                _fireFlyAnim.Play();
                _fireFlyAnims[i] = _fireFlyAnim;

                _fireFlies[i] = new();
            }

            _fireFlyMovementY = new(0f, 1f, 1200, 0f, TweenFunctions.SinInOut);
            _fireFlyMovementY.BucleType = TweenBucleType.PingPong;
            _fireFlyMovementY.Start();

            AtmosphereSound fieldSound = Atmospheres.Get(AtmosphereType.Field1);
            fieldSound.BaseVolume = FIELD_VOLUME_MIN;
            Core.AtmosphereManager.Add("Field", fieldSound);

            Loaded = true;
        }
        public override void Update()
        {
            base.Update();

            _moonAnim.Update();

            if (_moonAnim.FrameChanged)
                _moon.Source = _moonAnim.Source;

            UpdateFireFlies();
        }
        private void UpdateFireFlies()
        {
            _fireFlyMovementY.Update();
            for (int i = 0; i < 3; i++)
            {
                _fireFlyAnims[i].Update();

                if (_fireFlies[i].Count < 2)
                {
                    FireFly ff = new(i);
                    ff.Sprite.Source = _fireFlyAnims[i].Source;
                    _fireFlies[i].Add(ff);
                }

                for (int j = 0; j < _fireFlies[i].Count; j++)
                {
                    _fireFlies[i][j].Update();

                    if (_fireFlies[i][j].Disposed)
                    {
                        _fireFlies[i].RemoveAt(j);
                        j--;
                        continue;
                    }

                    _fireFlies[i][j].SetY(_fireFlyMovementY.Value);
                    if (_fireFlyAnims[i].FrameChanged)
                        _fireFlies[i][j].Sprite.Source = _fireFlyAnims[i].Source;
                }
            }
        }

        public override void DrawBackground9()
        {
            base.DrawBackground9();
            _sky.Draw();
            _moon.Draw();
        }
        public override void DrawBackground4()
        {
            base.DrawBackground4();
            _ground1.Draw();

            for (int i = 0; i < _fireFlies[2].Count; i++)
                _fireFlies[2][i].Draw();
        }
        public override void DrawBackground3()
        {
            base.DrawBackground3();
            _tree.Draw();

            for (int i = 0; i < _fireFlies[1].Count; i++)
                _fireFlies[1][i].Draw();
        }
        public override void DrawBackground2()
        {
            base.DrawFrontground2();
            _wall.Draw();

            for (int i = 0; i < _fireFlies[0].Count; i++)
                _fireFlies[0][i].Draw();
        }
        public override void DrawBackground1()
        {
            //todo
        }


        protected override void RollingDownUpdate()
        {
            base.RollingDownUpdate();
            Core.AtmosphereManager["Field"].BaseVolume = FIELD_VOLUME_MIN + _rollDownTween.Value * FIELD_VOLUME_MAX;
        }
        protected override void RollingDownChange()
        {
            base.RollingDownChange();
            Core.AtmosphereManager["Field"].BaseVolume = FIELD_VOLUME_MAX;
        }
    }

    public class FireFly
    {
        public Sprite Sprite { get => _sprite; set => _sprite = value; }
        private Sprite _sprite;

        private bool _directionX;
        private bool _directionY;
        private float _posX;
        private float _posY;
        private float _speed;

        private float _yMod;

        public bool Disposed { get => _disposed; }
        private bool _disposed;

        private const int MIN_Y_LAYER_1 = 208;
        private const int MAX_Y_LAYER_1 = 480;
        private const int MIN_Y_LAYER_2 = 208;
        private const int MAX_Y_LAYER_2 = 352;
        private const int MIN_Y_LAYER_3 = 208;
        private const int MAX_Y_LAYER_3 = 352;

        public FireFly(int type)
        {
            _directionX = Random.Shared.NextBool();
            _directionY = Random.Shared.NextBool();
            SpriteEffects flip = _directionX ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            int x = _directionX ? Core.Resolution.Bounds.Right + Random.Shared.Next(50, 100) : -Random.Shared.Next(50, 100);
            Texture2D texture = Textures.Get(TextureType.MainTitle1);
            SpriteSheet sheet = SpriteSheets.Get(SpriteSheetType.MainTitle1);
            Rectangle source;
            int y;

            if (type == 0)
            {
                y = Random.Shared.Next(MIN_Y_LAYER_1, MAX_Y_LAYER_1);
                source = sheet.Get("FireFly1_1");
                _speed = Random.Shared.NextFloat(0.5f, 1.2f);
                _yMod = 6;
            }
            else if (type == 1)
            {
                y = Random.Shared.Next(MIN_Y_LAYER_2, MAX_Y_LAYER_2);
                source = sheet.Get("FireFly2_1");
                _speed = Random.Shared.NextFloat(0.2f, 0.8f);
                _yMod = 4;
            }
            else if (type == 2)
            {
                y = Random.Shared.Next(MIN_Y_LAYER_3, MAX_Y_LAYER_3);
                source = sheet.Get("FireFly3_1");
                _speed = Random.Shared.NextFloat(0.15f, 0.4f);
                _yMod = 2;
            }
            else throw new NotImplementedException($"Not implemented Firefly type {type}");

            _posX = x;
            _posY = y;
            _sprite = new(texture, x, y, source, Color.White, new(source.Width / 2f), 0f, flip, 1f);
        }
        public void Update()
        {
            if (_disposed) return;
            float mod = _speed * Core.DeltaTime;

            if (_directionX)
            {
                _posX -= mod;
                if (_posX < -50)
                {
                    _disposed = true;
                    return;
                }
            }
            else
            {
                _posX += mod;
                if (_posX > Core.Resolution.Bounds.Right + 50)
                {
                    _disposed = true;
                    return;
                }
            }

            _sprite.X = (int)Math.Floor(_posX);
        }
        public void Draw()
        {
            if (_disposed) return;
            _sprite.Draw();
        }

        public void SetY(float value)
        {
            if (_directionY) Sprite.Y = (int)Math.Floor(_posY + value * _yMod);
            else Sprite.Y = (int)Math.Floor(_posY + (1f - value) * _yMod);
        }
    }
}
