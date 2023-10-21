using Assimp.Unmanaged;
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Controls.UI.Interfaces;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using Resources.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HorrorShorts_Game.Controls.UI.Language
{
    public class LanguageMenu : IResolutionDependent
    {
        private Texture2D _texture;
        private SpriteSheet _sheets;
        private RenderTarget2D _finalTexture;

        private Zones _zones;

        private readonly Dictionary<LanguageType, Label> _labels = new();
        private readonly Color UnselectedTextColor = Color.Black;
        private readonly Color OverTextColor = new(43, 43, 43);
        private readonly Color SelectedTextColor = new(113, 110, 110);

        private LanguageType? _overLanguage = null;
        private bool _overClose = false;

        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        private bool _isVisible = false;

        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
        private bool _isEnable = true;

        private bool _needRender = true;


        public void LoadContent()
        {
            _texture = Textures.Get(TextureType.LanguageMenu);
            _sheets = SpriteSheets.Get(SpriteSheetType.LanguageMenu);
            _finalTexture = new(Core.GraphicsDevice, 128, 64);

            foreach (LanguageType language in Enum.GetValues<LanguageType>())
            {
                Label label = new();
                label.Color = UnselectedTextColor;
                label.Alignament = TextAlignament.MiddleCenter;
                label.Text = language.GetNativeName();

                _labels[language] = label;
            }
        }
        public void Update()
        {
            if (!_isVisible) return;

            UpdateInputs();

            //foreach (LanguageType language in Enum.GetValues<LanguageType>())
            //    _labels[language].Update();
        }
        public void PreDraw()
        {
            if (!_isVisible) return;
            if (!_needRender) return;
            _needRender = false;

            Core.GraphicsDevice.SetRenderTarget(_finalTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin();

            //Options
            foreach (LanguageType language in Enum.GetValues<LanguageType>())
            {
                if (Core.Settings.Language == language)
                    Core.SpriteBatch.Draw(_texture, _zones.OptionsRender[language], _sheets.Get("OptionSelected"), Color.White);
                else if (_overLanguage == language) 
                    Core.SpriteBatch.Draw(_texture, _zones.OptionsRender[language], _sheets.Get("OptionOver"), Color.White);
                else Core.SpriteBatch.Draw(_texture, _zones.OptionsRender[language], _sheets.Get("OptionUnselected"), Color.White);

                _labels[language].Draw();
            }

            //Close
            //if (_closeSelected)
            //    Core.SpriteBatch.Draw(_texture, _zones.CloseRender, _sheets.Get("CrossSelected"), Color.White);
            //else 
            if (_overClose)
                Core.SpriteBatch.Draw(_texture, _zones.CloseRender, _sheets.Get("CrossOver"), Color.White);
            else Core.SpriteBatch.Draw(_texture, _zones.CloseRender, _sheets.Get("CrossUnselected"), Color.White);


            //Borders
            Core.SpriteBatch.Draw(_texture, _zones.GlobalRender, _sheets.Get("Borders"), Color.White);
            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        public void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_finalTexture, _zones.Global, Color.White);
        }
        public void Dispose()
        {
            if (_finalTexture != null && !_finalTexture.IsDisposed) 
                _finalTexture.Dispose();

            foreach (LanguageType language in Enum.GetValues<LanguageType>())
                _labels[language].Dispose();
            _labels.Clear();
        }

        private void UpdateInputs()
        {
            if (!_isEnable) return;

            //Over Options
            bool newOverClose = _overClose;
            LanguageType? newOverlanguage = _overLanguage;
#if DESKTOP
            if (Core.Controls.Mouse.PositionChanged)
            {
                bool findFlag = false;
                foreach (KeyValuePair<LanguageType, Rectangle> option in _zones.Options)
                    if (option.Value.Contains(Core.Controls.Mouse.PositionUI))
                    {
                        newOverlanguage = option.Key;
                        findFlag = true;
                        break;
                    }

                if (!findFlag) newOverlanguage = null;
                newOverClose = _zones.Close.Contains(Core.Controls.Mouse.PositionUI);
            }
#endif
#if DESKTOP || CONSOLE
            LanguageType[] languages = Enum.GetValues<LanguageType>();
            if (Core.Controls.UpTrigger)
            {
                if (_overLanguage.HasValue)
                {
                    int lng = ((int)_overLanguage.Value);
                    lng--;
                    if (lng < 0) newOverlanguage = languages.Last();
                    else newOverlanguage = (LanguageType)lng;
                }
                else newOverlanguage = languages.Last();
            }
            if (Core.Controls.DownTrigger)
            {
                if (_overLanguage.HasValue)
                {
                    int lng = ((int)_overLanguage.Value);
                    lng++;
                    if (lng > (int)languages.Last()) newOverlanguage = languages.First();
                    else newOverlanguage = (LanguageType)lng;
                }
                else newOverlanguage = languages.First();
            }
#endif
            if (_overClose != newOverClose)
            {
                _overClose = newOverClose;
                if (_overClose)
                {
                    newOverlanguage = null;
                    Core.SoundManager.Play(SoundType.OptionChange);
                }
                _needRender = true;
            }
            if (newOverlanguage != _overLanguage)
            {
                _overLanguage = newOverlanguage;
                if (_overLanguage.HasValue)
                    Core.SoundManager.Play(SoundType.OptionChange);
                RefreshLabelColor();
                _needRender = true;
            }


            //Close
            bool closeTriggered = false;
#if DESKTOP || PHONE
            closeTriggered |= _zones.Close.CheckClickedUI();
#endif
#if DESKTOP || CONSOLE
            closeTriggered |= Core.Controls.PauseTrigger;
#endif
            if (closeTriggered)
            {
                Hide();
                _needRender = true;
                return;
            }

            //Options Trigger
            LanguageType? optionTriggered = null;

#if DESKTOP || PHONE
            if (Core.Controls.Click)
                foreach (KeyValuePair<LanguageType, Rectangle> option in _zones.Options)
                    if (option.Value.Contains(Core.Controls.ClickPositionUI))
                    {
                        optionTriggered = option.Key;
                        break;
                    }
#endif
#if DESKTOP || CONSOLE
            if (_overLanguage.HasValue)
                if (Core.Controls.ActionTrigger)
                    optionTriggered = _overLanguage;
#endif

            if (optionTriggered.HasValue && optionTriggered.Value != Core.Settings.Language)
            {
                Core.SoundManager.Play(SoundType.OptionSelect);
                Core.Settings.ChangeLanguage(optionTriggered.Value);
                RefreshLabelColor();
                _needRender = true;
            }
        }
        private void RefreshLabelColor()
        {
            foreach (LanguageType language in Enum.GetValues<LanguageType>())
            {
                if (Core.Settings.Language == language)
                    _labels[language].Color = SelectedTextColor;
                else _labels[language].Color = UnselectedTextColor;

                _labels[language].Update();
            }
            _needRender = true;
        }
        private void GetZones()
        {
            Rectangle global = new(Core.Resolution.Bounds.Width / 2 - 64, Core.Resolution.Bounds.Height / 2 - 32, 128, 64);
            Rectangle globalRender = new(0, 0, 128, 64);
            Rectangle close = new(global.X + 110, global.Y, 16, 16);
            Rectangle closeRender = new(110, 0, 16, 16);

            LanguageType[] languages = Enum.GetValues<LanguageType>();
            Dictionary<LanguageType, Rectangle> options = new();
            Dictionary<LanguageType, Rectangle> optionsRender = new();
            Dictionary<LanguageType, Point> labelRender = new();

            for (int i = 0; i <  languages.Length; i++)
            {
                int padY = 4 * (i + 1) + 16 * i;
                options.Add(languages[i], new(global.X, global.Y + padY, 112, 16));
                optionsRender.Add(languages[i], new(0, padY, 112, 16));
                labelRender.Add(languages[i], optionsRender[languages[i]].Center);
            }

            _zones = new(global, globalRender, options, optionsRender, labelRender, close, closeRender);

            foreach (LanguageType language in Enum.GetValues<LanguageType>())
                _labels[language].Position = _zones.LabelRender[language];
        }

        #region EXTERNAL
        public void ResetResolution()
        {
            GetZones();
        }
        public void Show()
        {
            if (_isVisible) return;
            _isVisible = true;

            _overLanguage = LanguageType.English;

            GetZones();

            _labels[LanguageType.English].Color = OverTextColor;
            RefreshLabelColor();

            _needRender = true;
        }
        public void Hide()
        {
            if (!_isVisible) return;
            _isVisible = false;
        }

        #endregion


        private struct Zones
        {
            public readonly Rectangle Global;
            public readonly Rectangle GlobalRender;
            public readonly Dictionary<LanguageType, Rectangle> Options;
            public readonly Dictionary<LanguageType, Rectangle> OptionsRender;
            public readonly Dictionary<LanguageType, Point> LabelRender;
            public readonly Rectangle Close;
            public readonly Rectangle CloseRender;

            public Zones(Rectangle global,
                         Rectangle globalRender,
                         Dictionary<LanguageType, Rectangle> options,
                         Dictionary<LanguageType, Rectangle> optionsRender,
                         Dictionary<LanguageType, Point> labelRender,
                         Rectangle close,
                         Rectangle closeRender)
            {
                Global = global;
                GlobalRender = globalRender;
                Options = options;
                OptionsRender = optionsRender;
                LabelRender = labelRender;
                Close = close;
                CloseRender = closeRender;
            }
        }
    }
}
