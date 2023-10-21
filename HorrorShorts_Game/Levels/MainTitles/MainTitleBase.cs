using HorrorShorts_Game.Algorithms.Tweener;
using HorrorShorts_Game.Controls.Animations;
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Controls.UI;
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Language;
using HorrorShorts_Game.Controls.UI.Options;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HorrorShorts_Game.Screens.MainTitles
{
    public abstract class MainTitleBase : LevelBase
    {
        private Sprite _title;
        private Label _continue;
        private Label _newGame;
        private Label _options;
        private Label _exit;

        private Sprite _world;
        private AnimationSystem _worldAnim;
        private LanguageMenu _languageMenu;

        private static readonly Color UnSelectedOptionColor = Color.White;
        private static readonly Color OverOptionColor = Color.Yellow;
        private static readonly Color DisabledOptionColor = Color.Gray;

        private bool _continueIsEnable = false;
        private int _overOption = -1;
        private int _selectedOption = -1;
        private ScreenStates _screenState = ScreenStates.AppearingWaiting;
        private enum ScreenStates : byte
        {
            AppearingWaiting,
            AppearingRollingDown,
            AppearingOptions,
            WaitingOptionSelect,
            SelectingOptionAnim,
            OptionMenuOpened,
            LanguageMenuOpened
        }

        private float _currentDelay = 0f;
        private const float WAITING_APPEARING_DELAY = 3000f;

        private Tween<byte>[] _appearingOptionsTweens = new Tween<byte>[5];
        protected Tween<float> _rollDownTween;

        private enum Options : byte
        {
            Continue = 0,
            NewGame,
            Options,
            Exit,

            World
        }

        private OptionMenu _optionMenu = new();

        public MainTitleBase() : base()
        {
            _texturesRequired = new TextureType[]
            {
                TextureType.MainTitle,
                TextureType.OptionMenu,
                TextureType.InputButtons,
                TextureType.WorldUI
            };

            _animationRequired = new AnimationType[]
            {
                AnimationType.WorldUI
            };
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Core.Resolution.AjustOriginY = 0f;
            _rollDownTween = new(0f, 1f, 8000, function: TweenFunctions.QuadraticInOut);

            _title = new(Textures.Get(TextureType.MainTitle), 59, 10);
            _continue = new(Localizations.Global.MainTitle_Continue, 1, TextAlignament.MiddleRight); 
            _newGame = new(Localizations.Global.MainTitle_NewGame, 1, TextAlignament.MiddleRight); 
            _options = new(Localizations.Global.MainTitle_Options, 1, TextAlignament.MiddleRight); 
            _exit = new(Localizations.Global.MainTitle_Exit, 1, TextAlignament.MiddleRight);

            _worldAnim = new();
            _worldAnim.SetAnimation(Animations.Get(AnimationType.WorldUI)["World_Unselected"]);
            _worldAnim.Bucle = true;
            _worldAnim.Play();

            _world = new(Textures.Get(TextureType.WorldUI), 0, 0, _worldAnim.Source);

            _languageMenu = new();
            _languageMenu.LoadContent();

            RefreshOptionsColor();
            _title.Alpha = _continue.Alpha = _newGame.Alpha = _options.Alpha = _exit.Alpha = _world.Alpha = 0;
            _appearingOptionsTweens[0] = new Tween<byte>(0, 255, 2000f, 0000f, TweenFunctions.ExpIn);
            _appearingOptionsTweens[1] = new Tween<byte>(0, 255, 2000f, 1000f, TweenFunctions.ExpIn);
            _appearingOptionsTweens[2] = new Tween<byte>(0, 255, 2000f, 1500f, TweenFunctions.ExpIn);
            _appearingOptionsTweens[3] = new Tween<byte>(0, 255, 2000f, 2000f, TweenFunctions.ExpIn);
            _appearingOptionsTweens[4] = new Tween<byte>(0, 255, 2000f, 2500f, TweenFunctions.ExpIn);

            Core.Camera.LimitBounds = new Rectangle(0, 0, 320, 512);

            ComputeZones();

            //Menus Load
            _optionMenu.LoadContent();

            Loaded = true;
        }
        public override void Update()
        {
            base.Update();
            switch (_screenState)
            {
                case ScreenStates.AppearingWaiting:
                    AppearingWaitingUpdate();
                    break;
                case ScreenStates.AppearingRollingDown:
                    RollingDownUpdate();
                    break;
                case ScreenStates.AppearingOptions:
                    AppearingOptionsUpdate();
                    break;
                case ScreenStates.WaitingOptionSelect:
                    WaitingOptionUpdate();
                    break;
                case ScreenStates.SelectingOptionAnim:
                    SelectingOptionUpdate();
                    break;
                case ScreenStates.OptionMenuOpened:
                    OptionMenuSelected();
                    break;
                case ScreenStates.LanguageMenuOpened:
                    LanguageMenuSelected();
                    break;
            }

            if (_screenState == ScreenStates.AppearingWaiting || 
                _screenState == ScreenStates.AppearingRollingDown || 
                _screenState == ScreenStates.AppearingOptions)
            {
                _rollDownTween.Update();

                for (int i = 0; i < _appearingOptionsTweens.Length; i++)
                    _appearingOptionsTweens[i].Update();

                AppearingCancelUpdate();
            }

            _continue.Update();
            _newGame.Update();
            _options.Update();
            _exit.Update();

            if (_worldAnim.FrameChanged)
                _world.Source = _worldAnim.Source;
            _worldAnim.Update();
        }

        protected virtual void AppearingWaitingUpdate()
        {
            _currentDelay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
            if (_currentDelay >= WAITING_APPEARING_DELAY)
                AppearingWaitingChange();
        }
        protected virtual void RollingDownUpdate()
        {
            Core.Camera.Y = _rollDownTween.Value * (512 - Settings.NativeResolution.Height);
            if (_rollDownTween.State != TweenState.Doing)
                RollingDownChange();

            //todo: usar otro tipo de interpolacion
            //Core.Camera.Y += 0.5f;
            //if (Core.Camera.Bounds.Bottom >= 512 - Settings.NativeResolution.Height)
            //    RollingDownChange();
        }
        protected virtual void AppearingOptionsUpdate()
        {
            _title.Alpha = _appearingOptionsTweens[0].Value;
            _continue.Alpha = _appearingOptionsTweens[1].Value;
            _newGame.Alpha = _appearingOptionsTweens[2].Value; 
            _options.Alpha = _appearingOptionsTweens[3].Value; 
            _exit.Alpha = _appearingOptionsTweens[4].Value;

            _world.Alpha = _appearingOptionsTweens[4].Value;

            Tween<byte> tween = Array.Find(_appearingOptionsTweens, x => x.State == TweenState.Doing);
            if (tween == null) AppearingOptionsChange();
        }
        protected virtual void WaitingOptionUpdate()
        {
            //Over options
            int newOverOption = _overOption;
#if DESKTOP
            if (Core.Controls.Mouse.PositionChanged)
            {
                newOverOption = -1;
                if (_continue.Zone.Contains(Core.Controls.Mouse.PositionUI))
                {
                    if (_continueIsEnable) newOverOption = (int)Options.Continue;
                }
                else if (_newGame.Zone.Contains(Core.Controls.Mouse.PositionUI))
                    newOverOption = (int)Options.NewGame;
                else if (_options.Zone.Contains(Core.Controls.Mouse.PositionUI))
                    newOverOption = (int)Options.Options;
                else if (_exit.Zone.Contains(Core.Controls.Mouse.PositionUI))
                    newOverOption = (int)Options.Exit;
                else if (_world.Rectangle.Contains(Core.Controls.Mouse.PositionUI))
                    newOverOption = (int)Options.World;
            }
#endif
#if DESKTOP || CONSOLE
            if (Core.Controls.UpTrigger)
            {
                newOverOption--;
                if (newOverOption == (int)Options.Continue && !_continueIsEnable) newOverOption--;
                if (newOverOption < (int)Options.Continue) newOverOption = (int)Options.World;
                Core.SoundManager.Play(SoundType.OptionChange);
            }
            if (Core.Controls.DownTrigger)
            {
                newOverOption++;
                if (newOverOption == (int)Options.Continue && !_continueIsEnable) newOverOption++;
                if (newOverOption > (int)Options.World)
                {
                    if (_continueIsEnable) newOverOption = (int)Options.Continue;
                    else newOverOption = (int)Options.NewGame;
                }
                Core.SoundManager.Play(SoundType.OptionChange);
            }
#endif

            if (newOverOption != _overOption)
            {
                _overOption = newOverOption;
                if (_overOption >= 0)
                    Core.SoundManager.Play(SoundType.OptionChange);
                RefreshOptionsColor();
            }

            //Trigger option
            Options? optionTriggered = null;
#if DESKTOP || PHONE

            if (Core.Controls.Click)
            {
                if (_continue.Zone.Contains(Core.Controls.ClickPositionUI))
                {
                    if (_continueIsEnable)
                        optionTriggered = Options.Continue;
                }
                else if (_newGame.Zone.Contains(Core.Controls.ClickPositionUI))
                    optionTriggered = Options.NewGame;
                else if (_options.Zone.Contains(Core.Controls.ClickPositionUI))
                    optionTriggered = Options.Options;
                else if (_exit.Zone.Contains(Core.Controls.ClickPositionUI))
                    optionTriggered = Options.Exit;
                else if (_world.Rectangle.Contains(Core.Controls.ClickPositionUI))
                    optionTriggered = Options.World;
            }
#endif
#if DESKTOP || CONSOLE
            if (Core.Controls.ActionTrigger)
            {
                if (_overOption != -1)
                {
                    if (_overOption == (int)Options.Continue)
                    {
                        if (_continueIsEnable)
                            optionTriggered = Options.Continue;
                    }
                    else if (_overOption == (int)Options.NewGame)
                        optionTriggered = Options.NewGame;
                    else if (_overOption == (int)Options.Options)
                        optionTriggered = Options.Options;
                    else if (_overOption == (int)Options.Exit)
                        optionTriggered = Options.Exit;
                    else if (_overOption == (int)Options.World)
                        optionTriggered = Options.World;
                }
            }
#endif

            if (optionTriggered.HasValue)
            {
                _overOption = (int)optionTriggered.Value;
                RefreshOptionsColor();

                switch (optionTriggered)
                {
                    case Options.Continue:
                        ContinueSelected();
                        break;
                    case Options.NewGame:
                        NewGameSelected();
                        break;
                    case Options.Options:
                        OptionsSelected();
                        break;
                    case Options.Exit:
                        ExitSelected();
                        break;
                    case Options.World:
                        WorldSelected();
                        break;
                }
            }
        }
        protected virtual void SelectingOptionUpdate()
        {

        }
        protected virtual void OptionMenuSelected()
        {
            _optionMenu.Update();
            if (!_optionMenu.IsShowing)
                OptionMenuSelectedChange();
        }
        protected virtual void LanguageMenuSelected()
        {
            _languageMenu.Update();
            if (!_languageMenu.IsVisible)
                OptionMenuSelectedChange();
        }

        protected virtual void AppearingWaitingChange()
        {
            _screenState = ScreenStates.AppearingRollingDown;
            _rollDownTween.Start();
        }
        protected virtual void RollingDownChange()
        {
            _rollDownTween.Stop();
            Core.Camera.Y = Core.Camera.LimitBounds.Value.Bottom - 320;
            _screenState = ScreenStates.AppearingOptions;

            for (int i = 0; i < _appearingOptionsTweens.Length; i++)
                _appearingOptionsTweens[i].Start();
        }
        protected virtual void AppearingOptionsChange()
        {
            for (int i = 0; i < _appearingOptionsTweens.Length; i++)
                _appearingOptionsTweens[i].Stop();

            _title.Alpha = _continue.Alpha = _newGame.Alpha = _options.Alpha = _exit.Alpha = _world.Alpha = 255;
            _screenState = ScreenStates.WaitingOptionSelect;

#if DESKTOP || CONSOLE
            if (_continueIsEnable) _overOption = 0;
            else _overOption = 1;
#endif
            RefreshOptionsColor();
        }
        protected virtual void WaitingOptionChange()
        {

        }
        protected virtual void SelectingOptionChange()
        {

        }
        protected virtual void OptionMenuSelectedChange()
        {
#if PHONE
            _overOption = -1;
#endif
            _screenState = ScreenStates.WaitingOptionSelect;
            RefreshOptionsColor();
        }

        private void AppearingCancelUpdate()
        {
            bool finishAnim = false;
#if DESKTOP || PHONE
            finishAnim |= Core.Controls.Click;
#endif
#if DESKTOP || CONSOLE
            finishAnim |= Core.Controls.ActionTrigger || Core.Controls.PauseTrigger;
#endif

            if (finishAnim)
            {
                if (_screenState == ScreenStates.AppearingWaiting)
                    AppearingWaitingChange();
                if (_screenState == ScreenStates.AppearingRollingDown)
                    RollingDownChange();
                if (_screenState == ScreenStates.AppearingOptions)
                    AppearingOptionsChange();
            }
        }
        private void RefreshOptionsColor()
        {
            if (_screenState == ScreenStates.WaitingOptionSelect)
            {
                _continue.Color = _continueIsEnable ? (_overOption == (int)Options.Continue ? OverOptionColor : UnSelectedOptionColor) : DisabledOptionColor;
                _newGame.Color = _overOption == (int)Options.NewGame ? OverOptionColor : UnSelectedOptionColor;
                _options.Color = _overOption == (int)Options.Options ? OverOptionColor : UnSelectedOptionColor;
                _exit.Color = _overOption == (int)Options.Exit ? OverOptionColor : UnSelectedOptionColor;

                if (_overOption == (int)Options.World)
                {
                    if (_worldAnim.Name != "World_Selected")
                        _worldAnim.SwapAnimation(Animations.Get(AnimationType.WorldUI)["World_Selected"]);
                }
                else
                {
                    if (_worldAnim.Name != "World_Unselected")
                        _worldAnim.SwapAnimation(Animations.Get(AnimationType.WorldUI)["World_Unselected"]);
                }
            }
            else
            {
                _continue.Color = DisabledOptionColor;
                _newGame.Color = DisabledOptionColor;
                _options.Color = DisabledOptionColor;
                _exit.Color = DisabledOptionColor;

                //if (_worldAnim.Name != "World_Unselected")
                //    _worldAnim.SwapAnimation(Animations.Get(AnimationType.WorldUI)["World_Unselected"]);
            }
        }


        private void ContinueSelected()
        {
            _screenState = ScreenStates.SelectingOptionAnim;
            _overOption = (int)Options.Continue;
            Core.SoundManager.Play(SoundType.OptionSelect);
            RefreshOptionsColor();
            //todo
        }
        private void NewGameSelected()
        {
            _screenState = ScreenStates.SelectingOptionAnim;
            _overOption = (int)Options.NewGame;
            Core.SoundManager.Play(SoundType.OptionSelect);
            RefreshOptionsColor();
            //todo
        }
        private void OptionsSelected()
        {
            _screenState = ScreenStates.SelectingOptionAnim;
            _overOption = (int)Options.Options;
            Core.SoundManager.Play(SoundType.OptionSelect);

            _screenState = ScreenStates.OptionMenuOpened;
            _optionMenu.Show();
            RefreshOptionsColor();
            //todo
        }
        private void ExitSelected()
        {
            _screenState = ScreenStates.SelectingOptionAnim;
            _overOption = (int)Options.Exit;
            Core.SoundManager.Play(SoundType.OptionSelect);
            Core.Game.Exit(); //todo
        }
        private void WorldSelected()
        {
            _screenState = ScreenStates.SelectingOptionAnim;
            _overOption = (int)Options.World;
            Core.SoundManager.Play(SoundType.OptionSelect);

            _screenState = ScreenStates.LanguageMenuOpened;
            _languageMenu.Show();
            RefreshOptionsColor();
            //todo
        }

        private void ComputeZones()
        {
            _title.Y = Core.Resolution.Bounds.Top - 10;

            int optionHeight = 16;
            int optionRightPadding = 20;
            int optionsX = Core.Resolution.Bounds.Right - optionRightPadding;

            int half = _title.Bottom + (Core.Resolution.Bounds.Bottom - _title.Bottom) / 2;
            if (half + optionHeight * 2 + 20 > Core.Resolution.Bounds.Bottom)
                half = Core.Resolution.Bounds.Bottom - optionHeight * 2 - 10;


            _continue.Position = new(optionsX, half - optionHeight * 1);
            _newGame.Position = new(optionsX, half - optionHeight * 0);
            _options.Position = new(optionsX, half + optionHeight * 1);
            _exit.Position = new(optionsX, half + optionHeight * 2);

            _world.Position = new(Core.Resolution.Bounds.Left + 2, Core.Resolution.Bounds.Bottom - 32 - 2);
        }
        public override void ResetResolution()
        {
            base.ResetResolution();
            ComputeZones();
            _optionMenu?.ResetResolution();
            _languageMenu?.ResetResolution();
        }
        public override void ResetLocalization()
        {
            base.ResetLocalization();
            _optionMenu.SetLocalization();
            _continue.Text = Localizations.Global.MainTitle_Continue;
            _newGame.Text = Localizations.Global.MainTitle_NewGame;
            _options.Text = Localizations.Global.MainTitle_Options;
            _exit.Text = Localizations.Global.MainTitle_Exit;
        }

        public override void PreDraw()
        {
            base.PreDraw();

            switch (_screenState)
            {
                case ScreenStates.OptionMenuOpened:
                    _optionMenu.PreDraw();
                    break;
                case ScreenStates.LanguageMenuOpened:
                    _languageMenu.PreDraw();
                    break;
            }
        }
        public override void DrawUI()
        {
            _title.Draw();
            _continue.Draw();
            _newGame.Draw();
            _options.Draw();
            _exit.Draw();

            _world.Draw();

            switch (_screenState)
            {
                case ScreenStates.OptionMenuOpened:
                    _optionMenu.Draw();
                    break;
                case ScreenStates.LanguageMenuOpened:
                    _languageMenu.Draw();
                    break;
            }

            base.DrawUI();
        }
        public override void Dispose()
        {
            _languageMenu.Dispose();
            //todo
            base.Dispose();
        }
    }
}
