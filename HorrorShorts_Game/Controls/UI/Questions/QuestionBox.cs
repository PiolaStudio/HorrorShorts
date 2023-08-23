using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;

namespace HorrorShorts_Game.Controls.UI.Questions
{
    public class QuestionBox
    {
        #region VARIABLES & PROPERTIES
        //Textures
        private Texture2D _backgroundsTextures;
        private RenderTarget2D _borderTexture;
        private RenderTarget2D _optionsTexture;

        //Zones
        private Zones _zones;
        public int WidthScale
        {
            get => _scaleX;
            set
            {
                if (_scaleX == value) return;
                _scaleX = value;
                GetZone();
                _isDirty |= Dirtys.NeedRenderBorders;
                _isDirty |= Dirtys.NeedRenderOptions;
            }
        }
        private int _scaleX = 8;

        private bool _topIsSharp = false;
        private bool _bottomIsSharp = false;

        private readonly Point _textBorderPadding = new(6, 1);
        private readonly Point _globalPaddingForDialog = new(3, 0);

        private QuestionBoxLocation _location = QuestionBoxLocation.MiddleCenter;

        //Sprite-Sheets Sources
        private readonly Rectangle _roundedLeftWindowSource = new(0, 64, 16, 16);
        private readonly Rectangle _roundedCenterWindowSource = new(16, 64, 16, 16);
        private readonly Rectangle _roundedRightWindowSource = new(32, 64, 16, 16);

        private readonly Rectangle _sharpLeftWindowSource = new(0, 80, 16, 16);
        private readonly Rectangle _sharpCenterWindowSource = new(16, 80, 16, 16);
        private readonly Rectangle _sharpRightWindowSource = new(32, 80, 16, 16);

        private readonly Rectangle _middleLeftWindowSource = new(0, 96, 16, 16);
        private readonly Rectangle _middleRightWindowSource = new(16, 96, 16, 16);

        private readonly Rectangle _leftSplitterSource = new(0, 112, 16, 4);
        private readonly Rectangle _centerSplitterSource = new(16, 112, 16, 4);
        private readonly Rectangle _rightSplitterSource = new(32, 112, 16, 4);

        private readonly Rectangle _unSelectedBackgroundSource = new(32, 96, 1, 12);
        private readonly Rectangle _overBackgroundSource = new(33, 96, 1, 12);
        private readonly Rectangle _selectedBackgroundSource = new(34, 96, 1, 12);

        private SpriteFont _textFont;
        private HorizontalAlignament _textAlign = HorizontalAlignament.Left;

        private Color _selectedOptionTextColor;
        private Color _overOptionTextColor;
        private Color _unselectedOptionTextColor;

        //Flags
        [Flags()]
        private enum Dirtys : byte
        {
            None = 0,
            NeedRenderBorders = 1,
            NeedRenderOptions = 2,
        }
        private Dirtys _isDirty = Dirtys.None;

        //Options
        private string[] _options;
        private sbyte _overOptionIndex = -1;
        private sbyte _selectedOptionIndex = -1;

        //public sbyte OverOptionIndex { get => _overOptionIndex; }
        public sbyte SelectedOptionIndex { get => _overOptionIndex; }
        public string SelectedOptionValue { get => _overOptionIndex != -1 ? _options[_overOptionIndex] : null; }


        private bool _isVisible = true;
        private bool _closed = true;
        private bool _finished = false;
        private bool _optionSelectedTrigger = false;

        public bool OptionSelectedTrigger { get => _optionSelectedTrigger; }
        public bool Closed { get => _closed; }

        private bool _closing = false;
        private float _closingDelay = 0;
        private const float CLOSING_TOTAL_DELAY = 400f;
        private const float SELECTED_INTERMITTENT_DELAY = 50f;
        private bool _intermittentFlag = false;
        #endregion

        #region XNA
        public QuestionBox()
        {
            _options = new string[3] { "Option 1", "Option 2", "Option 3" };
            _textAlign = HorizontalAlignament.Center;
        }
        public void LoadContent()
        {
            _textFont = Fonts.Get(FontType.Arial);
            _backgroundsTextures = Textures.Get(TextureType.DialogMenu);

            GetZone();
            _borderTexture = new(Core.GraphicsDevice, _zones.Global.Width, _zones.Global.Height);
            _optionsTexture = new(Core.GraphicsDevice, _zones.Global.Width, _zones.Global.Height);
        }
        public void Update()
        {
            _optionSelectedTrigger = false;
            if (!_isVisible) return;
            if (_closed) return;

            //todo...
            GetZone();

            UpdateInputs();

            if (_closing)
            {
                _closingDelay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;

                if (_closingDelay >= CLOSING_TOTAL_DELAY)
                {
                    _optionSelectedTrigger = true;
                    _closed = true;
                }
                else
                {
                    bool intermittent = Math.Floor(_closingDelay / SELECTED_INTERMITTENT_DELAY) % 2 == 0;
                    if (_intermittentFlag != intermittent)
                    {
                        _intermittentFlag = intermittent;
                        _isDirty |= Dirtys.NeedRenderOptions;
                    }
                }
            }
        }
        public void PreDraw()
        {
            if (!_isVisible) return;
            if (_closed) return;

            if (_isDirty.HasFlag(Dirtys.NeedRenderBorders))
            {
                RenderBorders();
                _isDirty &= ~Dirtys.NeedRenderBorders;
            }

            if (_isDirty.HasFlag(Dirtys.NeedRenderOptions))
            {
                RenderOptions();
                _isDirty &= ~Dirtys.NeedRenderOptions;
            }
        }
        public void Draw()
        {
            if (!_isVisible) return;
            if (_closed) return;

            Core.SpriteBatch.Draw(_optionsTexture, _zones.Global, Color.White);
            Core.SpriteBatch.Draw(_borderTexture, _zones.Global, Color.White);
        }
        public void Dispose()
        {
            _borderTexture?.Dispose();
            _optionsTexture?.Dispose();
        }
        #endregion

        #region INPUTS
        private void UpdateInputs()
        {
            if (_closing) return;

#if DESKTOP
            UpdateKeyboard();
            UpdateMouse();
#endif
#if DESKTOP || CONSOLE
            UpdateGamePad();
#endif
#if PHONE
            //todo
#endif
        }

#if DESKTOP
        private void UpdateKeyboard()
        {
            if (_closing) return;

            //Up
            if (Core.Controls.Keyboard.UpTrigger)
            {
                _overOptionIndex--;
                if (_overOptionIndex < 0)
                    _overOptionIndex = (sbyte)(_options.Length - 1);

                Core.SoundManager.Play(SoundType.OptionChange);
                _isDirty |= Dirtys.NeedRenderOptions;
            }

            //Down
            if (Core.Controls.Keyboard.DownTrigger)
            {
                _overOptionIndex++;
                if (_overOptionIndex > _options.Length - 1)
                    _overOptionIndex = 0;

                Core.SoundManager.Play(SoundType.OptionChange);
                _isDirty |= Dirtys.NeedRenderOptions;
            }

            //Action
            if (Core.Controls.Keyboard.ActionTrigger)
            {
                if (_overOptionIndex != -1)
                {
                    _selectedOptionIndex = _overOptionIndex;
                    _closing = true;
                    Core.SoundManager.Play(SoundType.OptionSelect);
                    _isDirty |= Dirtys.NeedRenderOptions;
                    return;
                }
            }
        }
        private void UpdateMouse()
        {
            if (_closing) return;

            //Move
            if (Core.Controls.Mouse.PositionChanged)
            {
                sbyte over = -1;
                for (int i = 0; i < _zones.OptionsClickZones.Length; i++)
                    if (_zones.OptionsClickZones[i].Contains(Core.Controls.Mouse.PositionUI))
                    {
                        over = (sbyte)i;
                        break;
                    }

                if (_overOptionIndex != over)
                {
                    _overOptionIndex = over;
                    Core.SoundManager.Play(SoundType.OptionChange);
                    _isDirty |= Dirtys.NeedRenderOptions;
                }
            }

            //Click
            if (Core.Controls.Mouse.Click)
            {
                for (int i = 0; i < _zones.OptionsClickZones.Length; i++)
                    if (_zones.OptionsClickZones[i].Contains(Core.Controls.Mouse.PositionUI))
                    {
                        _selectedOptionIndex = _overOptionIndex = (sbyte)i;
                        _closing = true;
                        Core.SoundManager.Play(SoundType.OptionSelect);
                        _isDirty |= Dirtys.NeedRenderOptions;
                        return;
                    }
            }
        }
#endif
#if DESKTOP || CONSOLE
        private void UpdateGamePad()
        {
            if (_closing) return;

            //Up
            if (Core.Controls.GamePad.UpTrigger)
            {
                _overOptionIndex--;
                if (_overOptionIndex < 0)
                    _overOptionIndex = (sbyte)(_options.Length - 1);

                Core.SoundManager.Play(SoundType.OptionChange);
                _isDirty |= Dirtys.NeedRenderOptions;
            }

            //Down
            if (Core.Controls.GamePad.DownTrigger)
            {
                _overOptionIndex++;
                if (_overOptionIndex > _options.Length - 1)
                    _overOptionIndex = 0;

                Core.SoundManager.Play(SoundType.OptionChange);
                _isDirty |= Dirtys.NeedRenderOptions;
            }

            //Action
            if (Core.Controls.GamePad.ActionTrigger)
            {
                if (_overOptionIndex != -1)
                {
                    _selectedOptionIndex = _overOptionIndex;
                    _closing = true;
                    Core.SoundManager.Play(SoundType.OptionSelect);
                    _isDirty |= Dirtys.NeedRenderOptions;
                    return;
                }
            }
        }
#endif

        #endregion

        #region COMPUTE
        private void GetZone()
        {
            int globalWidth = _scaleX * 16 + 16;
            int globalHeight = 32 + (_options.Length - 2) * 14;
            int halfGlobalWidth = globalWidth / 2;
            int halfGlobalHeight = globalHeight / 2;

            Rectangle globalZone = _location switch
            {
                QuestionBoxLocation.TopLeft             => new(Core.ResolutionBounds.Left,
                                                             Core.ResolutionBounds.Top,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.TopCenter           => new(Core.ResolutionBounds.Center.X - halfGlobalWidth, 
                                                             Core.ResolutionBounds.Top, 
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.TopRight            => new(Core.ResolutionBounds.Right - globalWidth, 
                                                             Core.ResolutionBounds.Top, 
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.MiddleLeft          => new(Core.ResolutionBounds.Left, 
                                                             Core.ResolutionBounds.Center.Y - halfGlobalHeight, 
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.MiddleCenter        => new(Core.ResolutionBounds.Center.X - halfGlobalWidth, 
                                                             Core.ResolutionBounds.Center.Y - halfGlobalHeight, 
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.MiddleRight         => new(Core.ResolutionBounds.Right - globalWidth,
                                                             Core.ResolutionBounds.Center.Y - halfGlobalHeight,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.BottomLeft          => new(Core.ResolutionBounds.Left,
                                                             Core.ResolutionBounds.Bottom - globalHeight,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.BottomCenter        => new(Core.ResolutionBounds.Center.X - halfGlobalWidth,
                                                             Core.ResolutionBounds.Bottom - globalHeight,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.BottomRight         => new(Core.ResolutionBounds.Right - globalWidth,
                                                             Core.ResolutionBounds.Bottom - globalHeight,
                                                             globalWidth, globalHeight),

                QuestionBoxLocation.DialogTopLeft       => new(Core.ResolutionBounds.Right - globalWidth - _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Top + 58 + _globalPaddingForDialog.Y,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.DialogTopRight      => new(Core.ResolutionBounds.Left + _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Top + 58 + _globalPaddingForDialog.Y,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.DialogMiddleLeft    => new(Core.ResolutionBounds.Right - globalWidth - _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Center.Y + 36 + _globalPaddingForDialog.Y,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.DialogMiddleRight   => new(Core.ResolutionBounds.Left + _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Center.Y + 36 + _globalPaddingForDialog.Y,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.DialogBottomLeft    => new(Core.ResolutionBounds.Right - globalWidth - _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Bottom - globalHeight - 58 + _globalPaddingForDialog.Y,
                                                             globalWidth, globalHeight),
                QuestionBoxLocation.DialogBottomRight   => new(Core.ResolutionBounds.Left + _globalPaddingForDialog.X,
                                                             Core.ResolutionBounds.Bottom - globalHeight - 58 + _globalPaddingForDialog.Y, 
                                                             globalWidth, globalHeight),

                _ => throw new NotImplementedException("Not implemented location for " + _location)
            }; 

            Vector2[] textPositions = new Vector2[_options.Length];
            Rectangle[] optionsLeftZones = new Rectangle[_options.Length];
            Rectangle[] optionsRightZones = new Rectangle[_options.Length];
            Rectangle[] optionsBackground = new Rectangle[_options.Length];
            Rectangle[] optionsClickZones = new Rectangle[_options.Length];
            Rectangle[] splittersLeftZones= new Rectangle[_options.Length - 1];
            Rectangle[] splittersCenterZones = new Rectangle[_options.Length - 1];
            Rectangle[] splittersRightZones = new Rectangle[_options.Length - 1];

            for (int i = 0; i < _options.Length; i++)
            {
                int cy = 16 * i - 2 * Math.Max(0, i - 1);

                optionsClickZones[i] = new(globalZone.X,
                    globalZone.Y + cy, 
                    globalZone.Width, 16);

                optionsLeftZones[i] = new(0, cy, 16, 16);
                optionsRightZones[i] = new(globalZone.Width - 16, cy, 16, 16);
                optionsBackground[i] = new(0, 4 + 14 * i, globalZone.Width, 12);

                if (_textFont != null)
                {
                    Vector2 textMeasure = _textFont.MeasureString(_options[i]);
                    int textY = _textBorderPadding.Y + optionsBackground[i].Y + (i == _options.Length - 1 ? 0 : 1);
                    textPositions[i] = _textAlign switch
                    {
                        HorizontalAlignament.Left => new(_textBorderPadding.X, textY),
                        HorizontalAlignament.Center => new((float)Math.Floor((globalZone.Width - textMeasure.X) / 2f), textY),
                        HorizontalAlignament.Right => new((float)Math.Floor(globalZone.Width - _textBorderPadding.X - textMeasure.Y), textY),
                        _ => throw new NotImplementedException("Not supported Text Alignament for " + _textAlign)
                    };
                }
                else textPositions[i] = Vector2.Zero;

                if (i > 0)
                {
                    splittersLeftZones[i - 1] = new(0, cy - 1, 16, 4);
                    splittersCenterZones[i - 1] = new(16, cy - 1, globalZone.Width - 32, 4);
                    splittersRightZones[i - 1] = new(globalZone.Width - 16, cy - 1, 16, 4);
                }
            }


            _topIsSharp = _location == QuestionBoxLocation.DialogTopLeft || _location == QuestionBoxLocation.DialogTopRight ||
                _location == QuestionBoxLocation.DialogMiddleLeft || _location == QuestionBoxLocation.DialogMiddleRight;
            _bottomIsSharp = _location == QuestionBoxLocation.DialogBottomLeft || _location == QuestionBoxLocation.DialogBottomRight;

            _zones = new(globalZone, 
                optionsLeftZones, optionsRightZones,
                splittersLeftZones, splittersCenterZones, splittersRightZones,
                optionsBackground, optionsClickZones, textPositions,
                new(0, 0, 16, 16), //Top Left
                new(16, 0, globalZone.Width - 32, 16), //Top Center
                new(globalZone.Width - 16, 0, 16, 16), //Top Right
                new(0, globalZone.Height - 16, 16, 16), //Bottom Left
                new(16, globalZone.Height - 16, globalZone.Width - 32, 16),  //Bottom Center
                new(globalZone.Width - 16, globalZone.Height - 16, 16, 16)); //Bottom Right
        }
        #endregion

        #region RENDER
        private void RenderBorders()
        {
            if (_borderTexture == null || _borderTexture.Bounds.Size != _zones.Global.Size) //|| _baseTexture.IsDisposed
            {
                _borderTexture?.Dispose();
                _borderTexture = new(Core.GraphicsDevice, _zones.Global.Width, _zones.Global.Height);
            }

            Core.GraphicsDevice.SetRenderTarget(_borderTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Top
            if (_topIsSharp)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopLeftEdge, _sharpLeftWindowSource, Color.White); //Left
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopCenterSide, _sharpCenterWindowSource, Color.White); //Center
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopRightEdge, _sharpRightWindowSource, Color.White); //Right
            }
            else
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopLeftEdge, _roundedLeftWindowSource, Color.White); //Left
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopCenterSide, _roundedCenterWindowSource, Color.White); //Center
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.TopRightEdge, _roundedRightWindowSource, Color.White); //Right
            }

            //Bottom
            if (_bottomIsSharp)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomLeftEdge, _sharpLeftWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomCenterSide, _sharpCenterWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomRightEdge, _sharpRightWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            }
            else
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomLeftEdge, _roundedLeftWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomCenterSide, _roundedCenterWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BottomRightEdge, _roundedRightWindowSource, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 1f);
            }

            //Middle
            for (int i = 1; i < _options.Length - 1; i++)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.OptionsLeft[i], _middleLeftWindowSource, Color.White);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.OptionsRight[i], _middleRightWindowSource, Color.White);
            }

            //Splitters
            for (int i = 0; i < _options.Length - 1; i++)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.SplitterLeft[i], _leftSplitterSource, Color.White);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.SplitterCenter[i], _centerSplitterSource, Color.White);
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.SplitterRight[i], _rightSplitterSource, Color.White);
            }

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderOptions()
        {
            if (_optionsTexture == null || _optionsTexture.Bounds.Size != _zones.Global.Size) //|| _baseTexture.IsDisposed
            {
                _optionsTexture?.Dispose();
                _optionsTexture = new(Core.GraphicsDevice, _zones.Global.Width, _zones.Global.Height);
            }

            Core.GraphicsDevice.SetRenderTarget(_optionsTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            for (int i = 0; i < _options.Length; i++)
            {
                if (_selectedOptionIndex == i && !_intermittentFlag)
                {
                    Core.SpriteBatch.Draw(_backgroundsTextures, _zones.OptionsBackground[i], _selectedBackgroundSource, Color.White);
                    Core.SpriteBatch.DrawString(_textFont, _options[i], _zones.OptionsTextPosition[i], _selectedOptionTextColor);
                }
                else if (_overOptionIndex == i)
                {
                    Core.SpriteBatch.Draw(_backgroundsTextures, _zones.OptionsBackground[i], _overBackgroundSource, Color.White);
                    Core.SpriteBatch.DrawString(_textFont, _options[i], _zones.OptionsTextPosition[i], _overOptionTextColor);
                }
                else
                {
                    Core.SpriteBatch.Draw(_backgroundsTextures, _zones.OptionsBackground[i], _unSelectedBackgroundSource, Color.White);
                    Core.SpriteBatch.DrawString(_textFont, _options[i], _zones.OptionsTextPosition[i], _unselectedOptionTextColor);
                }
            }

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        #endregion

        #region EXTERNAL
        public void Show(Question question)
        {
            _options = (string[])question.Options.Clone();
            _textAlign = question.TextAlign;
            _textFont = Fonts.Get(question.Font);
            _location = question.Location;
            _overOptionIndex = question.DefaultOption;

            _selectedOptionTextColor = question.SelectedTextColor;
            _overOptionTextColor = question.OverTextColor;
            _unselectedOptionTextColor = question.UnSelectedTextColor;

            GetZone();

            _selectedOptionIndex = -1;
            _finished = false;
            _closed = false;
            _isVisible = true;
            _closing = false;
            _closingDelay = 0;
            _intermittentFlag = false;
            _optionSelectedTrigger = false;

            _isDirty |= Dirtys.NeedRenderBorders;
            _isDirty |= Dirtys.NeedRenderOptions;
        }
        public void Show(string[] options)
        {
            //todo
        }
        internal void ResetResolution()
        {
            GetZone();
        }
        #endregion

        #region SUB-CLASSES
        private readonly struct Zones
        {
            public readonly Rectangle Global;

            public readonly Rectangle[] OptionsLeft;
            public readonly Rectangle[] OptionsRight;

            public readonly Rectangle[] SplitterLeft;
            public readonly Rectangle[] SplitterCenter;
            public readonly Rectangle[] SplitterRight;

            public readonly Rectangle[] OptionsBackground;
            public readonly Rectangle[] OptionsClickZones;
            public readonly Vector2[] OptionsTextPosition;

            public readonly Rectangle TopLeftEdge;
            public readonly Rectangle TopCenterSide;
            public readonly Rectangle TopRightEdge;

            public readonly Rectangle BottomLeftEdge;
            public readonly Rectangle BottomCenterSide;
            public readonly Rectangle BottomRightEdge;

            public Zones(Rectangle global, 
                Rectangle[] optionsLeft, Rectangle[] optionsRight,
                Rectangle[] splitterLeft, Rectangle[] splitterCenter, Rectangle[] splitterRight,
                Rectangle[] optionsBackground, Rectangle[] optionsClickZones, Vector2[] optionsTextPosition, 
                Rectangle topLeftEdge, Rectangle topCenterSide, Rectangle topRightEdge, 
                Rectangle bottomLeftEdge, Rectangle bottomCenterSide, Rectangle bottomRightEdge)
            {
                Global = global;
                OptionsLeft = optionsLeft;
                OptionsRight = optionsRight;
                SplitterLeft = splitterLeft;
                SplitterCenter = splitterCenter;
                SplitterRight = splitterRight;
                OptionsBackground = optionsBackground;
                OptionsClickZones = optionsClickZones;
                OptionsTextPosition = optionsTextPosition;
                TopLeftEdge = topLeftEdge;
                TopCenterSide = topCenterSide;
                TopRightEdge = topRightEdge;
                BottomLeftEdge = bottomLeftEdge;
                BottomCenterSide = bottomCenterSide;
                BottomRightEdge = bottomRightEdge;
            }
        }
        #endregion
    }
}
