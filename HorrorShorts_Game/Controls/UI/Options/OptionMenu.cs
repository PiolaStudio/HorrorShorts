using HorrorShorts_Game.Controls.UI.Interfaces;
using HorrorShorts_Game.Controls.UI.Options.SubOptions;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.UI.Options
{
    public class OptionMenu : IResolutionDependent, ILocalizable
    {
        private bool _isShowing = false;
        public bool IsShowing { get => _isShowing; }

        private Texture2D _baseTexture;
        private readonly Rectangle[] _sources0;
        private readonly Rectangle[] _sources1;
        private readonly Rectangle[] _sources2;
        private readonly Rectangle[] _scrollSources;

        private Zones _zones;

        //Scroll
        private bool _scrollEnable = false;
        private float _scrollValue = 0f;
        private Rectangle _scrollBarZone;
        private Rectangle _scrollDataSource;
        private Rectangle _scrollSource;
        private bool _scrollDragging = false;
        private int _scrollDragPos = 0;
#if DESKTOP
        private float _scrollWheelValue;
#endif

        private RenderTarget2D _backTexture;
        private RenderTarget2D _dataTexture;

        private bool _needCompute = true;
        private bool _needRenderBack = true;
        private bool _needRenderData = true;

        private SubMenus _selectedSubMenu = SubMenus.Screen;
        private OptionSubMenu _subOptionMenu;
        private ScreenOption _screenSubMenu;
        private SoundOption _soundSubMenu;
#if DESKTOP || CONSOLE
        private ControlOption _controlSubMenu;
#endif
        private GeneralOption _generalSubMenu;

        private Dictionary<SubMenus, Label> _labels = new();
        private readonly Color SelectedLabelColor = Color.White;
        private readonly Color UnselectedLabelColor = new(189, 189, 189);

        private enum SubMenus : byte
        {
            Screen,
            Sound,
#if DESKTOP || CONSOLE
            Controls,
#endif
            General,
            Close
        }

        public OptionMenu()
        {
            _sources0 = new Rectangle[3] { new(208, 0, 16, 16), new(224, 0, 16, 16), new(240, 0, 16, 16) };
            _sources1 = new Rectangle[10];
            _sources2 = new Rectangle[10];
            for (int i = 0; i < _sources1.Length; i++)
            {
                int x = 16 * i;
                _sources1[i] = new(x, 0, 16, 16);
                _sources2[i] = new(x, 16, 16, 16);
            }
            _scrollSources = new Rectangle[2];
            for (int i = 0; i < 2; i++)
                _scrollSources[i] = new(16 * i, 32, 1, 1);

            //Sub menus
            _screenSubMenu = new();
            _soundSubMenu = new();
#if DESKTOP || CONSOLE
            _controlSubMenu = new();
#endif
            _generalSubMenu = new();
        }
        public void LoadContent()
        {
            _baseTexture = Textures.Get(TextureType.OptionMenu);

            _selectedSubMenu = SubMenus.Screen;
            _screenSubMenu.LoadContent();
            _soundSubMenu.LoadContent();
#if DESKTOP || CONSOLE
            _controlSubMenu.LoadContent();
#endif
            _generalSubMenu.LoadContent();

            //Create Labels
            Label screenLBL = new();
            _labels.Add(SubMenus.Screen, screenLBL);

            Label soundLBL = new();
            _labels.Add(SubMenus.Sound, soundLBL);

#if DESKTOP || CONSOLE
            Label controlLBL = new();
            _labels.Add(SubMenus.Controls, controlLBL);
#endif

            Label generalLBL = new();
            _labels.Add(SubMenus.General, generalLBL);

            Label closeLBL = new();
            _labels.Add(SubMenus.Close, closeLBL);

            SetLocalization();
            Compute();
        }

        public void Update()
        {
            if (!_isShowing) return;

            UpdateScroll();
            UpdateTabs();

            if (_needCompute) Compute();

            //Labels
            foreach (Label label in _labels.Values)
                label.Update();

            _subOptionMenu.Update();
            _needRenderData |= _subOptionMenu.NeedRender;
        }
        private void UpdateScroll()
        {
            if (!_scrollEnable) return;
            bool scrollUpdateFlag = false;

#if DESKTOP || PHONE
            if (_scrollDragging)
            {
                if (!Core.Controls.ClickPressed)
                {
                    _scrollDragging = false;
                    _scrollSource = _scrollSources[0];
                }
            }
            else
            {
                if (Core.Controls.ClickPressed)
                {
                    if (_scrollBarZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        _scrollDragging = true;
                        _scrollDragPos = Core.Controls.ClickPositionUI.Y - _scrollBarZone.Y;
                        _scrollSource = _scrollSources[1];
                    }
                }
            }

            if (_scrollDragging)
            {
                float sv = (Core.Controls.ClickPositionUI.Y - _zones.ScrollBarZone.Y - _scrollDragPos) /
                    ((float)_zones.ScrollBarZone.Height - _scrollBarZone.Height);
                float scrollVal = MathHelper.Clamp(sv, 0, 1);
                if (_scrollValue != scrollVal)
                {
                    _scrollValue = scrollVal;
                    scrollUpdateFlag = true;
                }
            }
#if DESKTOP
            else
            {
                if (_scrollWheelValue != Core.Controls.Mouse.State.ScrollWheelValue)
                {
                    float dif = _scrollWheelValue - Core.Controls.Mouse.State.ScrollWheelValue;
                    _scrollWheelValue = Core.Controls.Mouse.State.ScrollWheelValue;
                    _scrollValue = MathHelper.Clamp(_scrollValue + dif / 1000f, 0, 1);
                    scrollUpdateFlag = true;
                }
            }
#endif

            if (scrollUpdateFlag)
            {
                _scrollBarZone.Y = Convert.ToInt32(_zones.ScrollBarZone.Y + ((_zones.ScrollBarZone.Height - _scrollBarZone.Height) * _scrollValue));
                _scrollDataSource.Y = Convert.ToInt32((_zones.DataRealZone.Height - _zones.DataRenderZone.Height) * _scrollValue);
                _subOptionMenu.ScrollTop = _scrollDataSource.Y;
                _subOptionMenu.ComputeVirtualPositions();
            }
#endif
        }
        private void UpdateTabs()
        {
            if (_scrollDragging) return;
            foreach (KeyValuePair<SubMenus, Rectangle> zone in _zones.OptionsAbsolute)
            {
                if (zone.Key == _selectedSubMenu) continue;

                //todo
                if (Core.Controls.ClickPressed)
                {
                    if (zone.Value.Contains(Core.Controls.ClickPositionUI))
                    {
                        Core.SoundManager.Play(SoundType.OptionChange);
                        _selectedSubMenu = zone.Key;
                        _needCompute = _needRenderData = true;
                    }
                }
            }
        }
        private void Compute()
        {
            _needCompute = false;

            switch (_selectedSubMenu)
            {
                case SubMenus.Screen:
                    _subOptionMenu = _screenSubMenu;
                    break;
                case SubMenus.Sound:
                    _subOptionMenu = _soundSubMenu;
                    break;
#if DESKTOP || CONSOLE
                case SubMenus.Controls:
                    _subOptionMenu = _controlSubMenu;
                    break;
#endif
                case SubMenus.General:
                    _subOptionMenu = _generalSubMenu;
                    break;
                case SubMenus.Close:
                    _isShowing = false;
                    //todo
                    break;
                default:
                    throw new NotImplementedException("No implemented option");
            }

            GetZones();

            //Sub menu
            _subOptionMenu.PanelPosition = _zones.DataZoneAbsolute.Location;
            _subOptionMenu.ComputePositions();
            _subOptionMenu.ScrollTop = 0;

            //Labels
            foreach (SubMenus sm in Enum.GetValues<SubMenus>())
            {
                if (_selectedSubMenu == sm)
                {
                    _labels[sm].Color = SelectedLabelColor;
                    _labels[sm].X = 4;
                    _labels[sm].Alignament = TextAlignament.MiddleLeft;
                }
                else
                {
                    _labels[sm].Color = UnselectedLabelColor;
                    _labels[sm].X = 76;
                    _labels[sm].Alignament = TextAlignament.MiddleRight;
                }
                if (sm == SubMenus.Close)
                    _labels[sm].Y = _zones.OptionsRelative[sm].Center.Y - 1;
                else _labels[sm].Y = _zones.OptionsRelative[sm].Center.Y + 1;
            }

            //Scroll Bar
            _scrollValue = 0f;
            _scrollSource = _scrollSources[0];

            _scrollDataSource = new(Point.Zero, _zones.DataRenderZone.Size);
            _scrollEnable = _zones.DataRealZone.Height > _zones.DataZoneAbsolute.Height;
            _scrollBarZone = new(_zones.ScrollBarZone.X, _zones.ScrollBarZone.Y, _zones.ScrollBarSize.X, _zones.ScrollBarSize.Y);
#if DESKTOP
            _scrollWheelValue = Core.Controls.Mouse.State.ScrollWheelValue;
#endif

            //Textures
            if (_backTexture == null || _backTexture.IsDisposed || _backTexture.Width != _zones.Global.Width || _backTexture.Height != _zones.Global.Height)
            {
                if (_backTexture != null && !_backTexture.IsDisposed) _backTexture.Dispose();
                _backTexture = new(Core.GraphicsDevice, _zones.Global.Width, _zones.Global.Height);
            }

            if (_dataTexture == null || _dataTexture.IsDisposed || _dataTexture.Height != _zones.DataRealZone.Height)
            {
                if (_dataTexture != null && !_dataTexture.IsDisposed) _dataTexture.Dispose();
                _dataTexture = new(Core.GraphicsDevice, _zones.DataRealZone.Width, _zones.DataRealZone.Height);
            }
        }


        public void PreDraw()
        {
            if (!_isShowing) return;
            if (_needRenderBack && !_needCompute || true) RenderBack(); //todo: hack
            if (_needRenderData && !_needCompute)
            {
                _subOptionMenu.PreDraw();
                RenderData();
            }
        }
        private void RenderBack()
        {
            _needRenderBack = false;

            Core.GraphicsDevice.SetRenderTarget(_backTexture);

            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp);

            //Top
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(80, 0, 192, 16), _sources1[1], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(272, 0, 16, 16), _sources1[6], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(288, 0, 16, 16), _sources2[2], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            //Bottom
            int bottomY = _zones.Global.Height - 16;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(80, bottomY, 192, 16), _sources1[1], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(272, bottomY, 16, 16), _sources1[6], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(288, bottomY, 16, 16), _sources2[2], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 1f);

            //Right
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(272, 16, 16, bottomY - 16), _sources1[7], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(288, 16, 16, bottomY - 16), _sources2[3], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            //Left
            Tuple<Rectangle, Rectangle, Rectangle> selectedSources = new(_sources1[0], _sources1[1], _sources1[1]);
            Tuple<Rectangle, Rectangle, Rectangle> unselectedSources = new(_sources2[9], _sources2[1], _sources2[2]);
            Tuple<Rectangle, Rectangle, Rectangle> optionSources;


            //Screen
            int optionY = 0;
            optionSources = _selectedSubMenu == SubMenus.Screen ? selectedSources : unselectedSources;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY, 16, 16), optionSources.Item1, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(16, optionY, 48, 16), optionSources.Item2, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, optionY, 16, 16), optionSources.Item3, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            if (_selectedSubMenu == SubMenus.Screen)
                Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            else Core.SpriteBatch.Draw(_baseTexture, new Rectangle(5, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            selectedSources = new(_sources1[2], _sources1[1], _sources1[1]);
            unselectedSources = new(_sources2[5], _sources2[1], _sources2[2]);

            //Sound
            optionY += 16;
            optionSources = _selectedSubMenu == SubMenus.Sound ? selectedSources : unselectedSources;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY, 16, 16), optionSources.Item1, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(16, optionY, 48, 16), optionSources.Item2, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, optionY, 16, 16), optionSources.Item3, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            if (_selectedSubMenu == SubMenus.Sound)
                Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            else Core.SpriteBatch.Draw(_baseTexture, new Rectangle(5, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

#if DESKTOP || CONSOLE
            //Controls
            optionY += 16;
            optionSources = _selectedSubMenu == SubMenus.Controls ? selectedSources : unselectedSources;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY, 16, 16), optionSources.Item1, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(16, optionY, 48, 16), optionSources.Item2, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, optionY, 16, 16), optionSources.Item3, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            if (_selectedSubMenu == SubMenus.Controls)
                Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            else Core.SpriteBatch.Draw(_baseTexture, new Rectangle(5, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
#endif

            //General
            optionY += 16;
            optionSources = _selectedSubMenu == SubMenus.General ? selectedSources : unselectedSources;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY, 16, 16), optionSources.Item1, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(16, optionY, 48, 16), optionSources.Item2, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, optionY, 16, 16), optionSources.Item3, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);

            if (_selectedSubMenu == SubMenus.General)
                Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            else Core.SpriteBatch.Draw(_baseTexture, new Rectangle(5, optionY + 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            //Close
            int option2Y = _zones.Global.Height - 16;
            selectedSources = new(_sources1[0], _sources1[1], _sources1[1]);
            unselectedSources = new(_sources2[9], _sources2[1], _sources2[2]);
            optionSources = _selectedSubMenu == SubMenus.Close ? selectedSources : unselectedSources;
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, option2Y, 16, 16), optionSources.Item1, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(16, option2Y, 48, 16), optionSources.Item2, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, option2Y, 16, 16), optionSources.Item3, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 1f);

            if (_selectedSubMenu == SubMenus.Close)
                Core.SpriteBatch.Draw(_baseTexture, new Rectangle(0, option2Y - 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            else Core.SpriteBatch.Draw(_baseTexture, new Rectangle(5, option2Y - 16, 80, 16), _sources0[0], Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);

            //Body
            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(80, 16, 192, _zones.Global.Height - 32), _sources1[8], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            //for (int i = 64; i < option2Y; i += 16)
            //    Core.SpriteBatch.Draw(_baseTexture, new Rectangle(32, i, 16, 16), sources0[2], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            Core.SpriteBatch.Draw(_baseTexture, new Rectangle(64, 64, 16, option2Y - 64), _sources0[1], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);


            //Labels
            foreach (Label label in _labels.Values)
                label.Draw();

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderData()
        {
            _needRenderData = false;

            Core.GraphicsDevice.SetRenderTarget(_dataTexture);

            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp);

            _subOptionMenu.Draw();

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw()
        {
            if (!_isShowing) return;
            Core.SpriteBatch.Draw(_backTexture, _zones.Global, Color.White);
            Core.SpriteBatch.Draw(_dataTexture, _zones.DataRenderZone, _scrollDataSource, Color.White);
            if (_scrollEnable)
                Core.SpriteBatch.Draw(_baseTexture, _scrollBarZone, _scrollSource, Color.White);

            _subOptionMenu.DrawOver();
        }
        public void Dispose()
        {
            if (_backTexture != null && !_backTexture.IsDisposed) _backTexture.Dispose();
            if (_dataTexture != null && !_dataTexture.IsDisposed) _dataTexture.Dispose();
        }


        private void GetZones()
        {
            Rectangle global = new(8, 8, Core.Resolution.Bounds.Width - 16, Core.Resolution.Bounds.Height - 16);

            Dictionary<SubMenus, Rectangle> optsRelative = new();
            Dictionary<SubMenus, Rectangle> optsAbsolute = new();
            SubMenus[] menus = Enum.GetValues<SubMenus>();

            foreach (SubMenus sub in menus)
            {
                Rectangle relative = sub switch
                {
                    SubMenus.Screen => new(0, 0, 80, 16),
                    SubMenus.Sound => new(0, 16, 80, 16),
#if DESKTOP || CONSOLE
                    SubMenus.Controls => new(0, 32, 80, 16),
                    SubMenus.General => new(0, 48, 80, 16),
#else
                    SubMenus.General => new(0, 32, 80, 16),
#endif
                    SubMenus.Close => new(0, global.Height - 16, 80, 16),
                    _ => throw new NotImplementedException("Not implemented option")
                };

                Rectangle absolute = new(global.X + relative.X, global.Y + relative.Y, relative.Width, relative.Height);

                optsRelative.Add(sub, relative);
                optsAbsolute.Add(sub, absolute);
            }

            Rectangle dataZoneRelative = new(80, 2, 206, global.Height - 4);
            Rectangle dataZoneAbsolute = new(global.X + dataZoneRelative.X, global.Y + dataZoneRelative.Y, dataZoneRelative.Width, dataZoneRelative.Height);
            Rectangle dataRealZone = new(dataZoneAbsolute.X, dataZoneAbsolute.Y, dataZoneRelative.Width, _subOptionMenu.Height);

            Rectangle dataRenderZone;
            Rectangle scrollBarZone;
            Point scrollBarSize;
            if (dataRealZone.Height > dataZoneAbsolute.Height)
            {
                dataRenderZone = new(dataRealZone.X, dataRealZone.Y, dataRealZone.Width, dataZoneAbsolute.Height);
                scrollBarZone = new(global.X + 288, global.Y + dataZoneRelative.Y, 14, dataZoneRelative.Height);
                scrollBarSize = new(14, dataZoneAbsolute.Height - (dataRealZone.Height - dataZoneAbsolute.Height));
            }
            else
            {
                dataRenderZone = dataRealZone;
                scrollBarZone = new(global.X + 288, global.Y + dataZoneRelative.Y, 14, dataZoneAbsolute.Height);
                scrollBarSize = new(14, dataRealZone.Height);
            }

            _zones = new(global, optsRelative, optsAbsolute, dataRealZone, dataZoneAbsolute, dataZoneRelative, dataRenderZone, scrollBarZone, scrollBarSize);
        }
        public void ResetResolution()
        {
            _needCompute = true;
            _needRenderBack = true;
            _needRenderData = true;
        }
        public void SetLocalization()
        {
            _screenSubMenu.SetLocalization();
            _soundSubMenu.SetLocalization();
#if DESKTOP || CONSOLE
            _controlSubMenu.SetLocalization();
#endif
            _generalSubMenu.SetLocalization();

            _labels[SubMenus.Screen].Text = Localizations.Global.Options_Screen.ToUpper();
            _labels[SubMenus.Sound].Text = Localizations.Global.Options_Sound.ToUpper();
#if DESKTOP || CONSOLE
            _labels[SubMenus.Controls].Text = Localizations.Global.Options_Control.ToUpper();
#endif
            _labels[SubMenus.General].Text = Localizations.Global.Options_General.ToUpper();
            _labels[SubMenus.Close].Text = Localizations.Global.Options_Close.ToUpper();
        }
        public void Show()
        {
            _isShowing = true;
            _selectedSubMenu = SubMenus.Screen;
            _needCompute = _needRenderData = true;
        }
        public void Hide()
        {
            _isShowing = false;
        }


        private struct Zones
        {
            public readonly Rectangle Global;
            public readonly Dictionary<SubMenus, Rectangle> OptionsRelative;
            public readonly Dictionary<SubMenus, Rectangle> OptionsAbsolute;
            public readonly Rectangle DataRealZone;
            public readonly Rectangle DataZoneAbsolute;
            public readonly Rectangle DataZoneRelative;
            public readonly Rectangle DataRenderZone;
            public readonly Rectangle ScrollBarZone;
            public readonly Point ScrollBarSize;

            public Zones(Rectangle global, Dictionary<SubMenus, Rectangle> optionsRelative, Dictionary<SubMenus, Rectangle> optionsAbsolute, 
                Rectangle dataRealZone, Rectangle dataZoneAbsolute, Rectangle dataZoneRelative, Rectangle dataRenderZone, 
                Rectangle scrollBarZone, Point scrollBarSize)
            {
                Global = global;
                OptionsRelative = optionsRelative;
                OptionsAbsolute = optionsAbsolute;
                DataRealZone = dataRealZone;
                DataZoneAbsolute = dataZoneAbsolute;
                DataZoneRelative = dataZoneRelative;
                DataRenderZone = dataRenderZone;
                ScrollBarZone = scrollBarZone;
                ScrollBarSize = scrollBarSize;
            }
        }
    }
}
