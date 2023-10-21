using HorrorShorts_Game.Audio.Atmosphere;
using HorrorShorts_Game.Audio.Song;
using HorrorShorts_Game.Audio.Sound;
using HorrorShorts_Game.Controls.Camera;
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Questions;
using HorrorShorts_Game.Effects;
using HorrorShorts_Game.Inputs;
using HorrorShorts_Game.Levels.Empty;
using HorrorShorts_Game.Resources;
using HorrorShorts_Game.Screens;
using HorrorShorts_Game.Screens.MainTitles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static HorrorShorts_Game.Settings;

namespace HorrorShorts_Game
{
    public static class Core
    {
        public static Game1 Game;

        public static SpriteBatch SpriteBatch { get; private set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static GameWindow Window { get; private set; }
        public static ContentManager Content { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static RenderTarget2D Render { get; private set; }
        public static Settings Settings { get; private set; }
        public static ResolutionManagment Resolution { get; private set; }

        public static LevelBase Level { get; private set; }

        public static float DeltaTime { get; private set; } //todo
        private static readonly TimeSpan _idealFrameRate = TimeSpan.FromMilliseconds(1000 / 60.0);

        //Audio
        public static SongManager SongManager { get; private set; }
        public static SoundManager SoundManager { get; private set; }
        public static AtmosphereManager AtmosphereManager { get; private set; }

        public static Camera Camera { get; private set; }
        public static DialogManagement DialogManagement { get; private set; }
        public static QuestionBox QuestionBox { get; private set; }
        public static FadeInOut FadeEffect { get; private set; }

        public static readonly Color BackColor = new(40, 40, 40);


        public static InputControl Controls { get; private set; }

        public static void Init(Game1 game, GraphicsDeviceManager deviceManager)
        {
            Logger.Advice("Initing Core...");
            Game = game;
            GraphicsDeviceManager = deviceManager;
            GraphicsDevice = game.GraphicsDevice;
            Window = game.Window;

            Content = game.Content;
            SpriteBatch = new SpriteBatch(GraphicsDevice);


            //Try Load Settings
            Settings = Settings.Load();

            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = Settings.VSync;
            GraphicsDeviceManager.HardwareModeSwitch = Settings.HardwareMode;
#if DEBUG   
            game.IsMouseVisible = true;
#else 
            game.IsMouseVisible = false;
#endif


            Level = new EmptyLevel();

            SoundManager = new();
            SongManager = new();
            AtmosphereManager = new();
            DialogManagement = new();
            QuestionBox = new();
            FadeEffect = new();
            Camera = new();
            Resolution = new();

            //Inputs
            Controls = new();
            

            Render = new(GraphicsDevice, Settings.NativeResolution.Width, Settings.NativeResolution.Height);

            Textures.Init();
            SpriteSheets.Init();
            Fonts.Init();
            Sounds.Init();
            Songs.Init();
            Localizations.Init();

#if DESKTOP
            SetResolution(Settings.ResolutionWidth, Settings.ResolutionHeight, Settings.ResizeMode, true);
#elif PHONE
            SetResolution(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight, ResizeModes.FullScreen);
            //SetResolution(2560, 1440, ResizeModes.FullScreen);
#endif

#if DESKTOP
            game.Window.ClientSizeChanged += (s, e) =>
            {
                SetResolution(Window.ClientBounds.Width, Window.ClientBounds.Height, ResizeModes.Window, true);
                GraphicsDeviceManager.ApplyChanges();
            };
#endif

            Render = new RenderTarget2D(GraphicsDevice, Settings.NativeResolution.Width, Settings.NativeResolution.Height);


            Level = new MainTitle1(); //todo: seleccionar MainTitle correcta
            FadeEffect.FadeIn(1500);

            Logger.Advice("Core initialized!");
        }
        public static void LoadContent()
        {
            Logger.Advice("Loading Core!");
            DialogManagement.LoadContent();
            QuestionBox.LoadContent();
            Logger.Advice("Loaded Core!");
        }
        public static void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / _idealFrameRate.TotalMilliseconds);

            Controls.Update();
            DialogManagement.Update();
            QuestionBox.Update();

            SongManager.Update();
            SoundManager.Update();
            AtmosphereManager.Update();

            FadeEffect.Update();
            Camera.Update();
        }
        public static void Dispose()
        {
            DialogManagement.Dispose();
            QuestionBox.Dispose();
        }

        private static bool _settingResolutionFlag = false;
        public static void SetResolution(int width, int height, ResizeModes resizeMode, bool resizable = false)
        {
            if (_settingResolutionFlag) return;
            _settingResolutionFlag = true;
            Logger.Advice($"Setting resolution. {width} x {height} (ResizeMode: {resizeMode} - Resizable: {resizable})");

#if DESKTOP
            if (resizeMode == ResizeModes.FullScreen || resizeMode == ResizeModes.Bordeless)
#else
            if (resizeMode == ResizeModes.FullScreen)
#endif
            {
                //width = 2560;
                //height = 1440;
#if DESKTOP || CONSOLE
                width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#elif PHONE
                width = GraphicsDeviceManager.PreferredBackBufferWidth;
                height = GraphicsDeviceManager.PreferredBackBufferHeight;
#endif
            }

            Resolution.SetResolution(width, height, resizeMode, resizable);

            DialogManagement?.ResetResolution();
            QuestionBox?.ResetResolution();
            if (Level.Loaded) Level.ResetResolution();

            //Reset UI Resolution
            DialogManagement?.ResetResolution();
            QuestionBox?.ResetResolution();
            if (Level.Loaded) Level.ResetResolution();
            Logger.Advice($"Resolution Setted!");
            _settingResolutionFlag = false;
        }
        public static void ResetLanguage()
        {
            Logger.Advice($"Setting language. {Core.Settings.Language}");
            Localizations.ReLoad();

            //Reset controls Localizations
            DialogManagement?.SetLocalization();
            QuestionBox?.SetLocalization();
            if (Level.Loaded) Level.ResetLocalization();
            Logger.Advice($"Localization Changed!");
        }
    }
}
