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
using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            //todo: load settings from file
#if DEBUG
            Settings = new Settings(); //todo: borrar
#endif



            Level = new EmptyLevel();

            SoundManager = new();
            SongManager = new();
            AtmosphereManager = new();
            Controls = new();
            DialogManagement = new();
            QuestionBox = new();
            FadeEffect = new();
            Camera = new();
            Resolution = new();

            Render = new(GraphicsDevice, Settings.NativeResolution.Width, Settings.NativeResolution.Height);

            Textures.Init();
            SpriteSheets.Init();
            Fonts.Init();
            Sounds.Init();
            Songs.Init();
            Localizations.Init();

#if DESKTOP
            if (Settings.FullScreen)
                SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Settings.FullScreen);
            else SetResolution(Settings.ResolutionWidth, Settings.ResolutionHeight, Settings.FullScreen);
#elif PHONE
            //SetResolution(2560, 1440, true);
            SetResolution(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight, true);
#endif


#if DESKTOP
            game.IsMouseVisible = true;
            game.Window.IsBorderless = false;
            game.Window.AllowUserResizing = true;
            game.Window.ClientSizeChanged += (s, e) =>
            {
                SetResolution(Window.ClientBounds.Width, Window.ClientBounds.Height, false);
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
            AtmosphereManager.Update();

            FadeEffect.Update();
            Camera.Update();
        }
        public static void Dispose()
        {
            DialogManagement.Dispose();
            QuestionBox.Dispose();
        }

        public static void SetResolution(int width, int height, bool fullScreen)
        {
            Logger.Advice($"Setting resolution. {width} x {height} (FullScreen: {fullScreen})");

            Resolution.SetResolution(width, height, fullScreen);

            DialogManagement?.ResetResolution();
            QuestionBox?.ResetResolution();
            if (Level.Loaded) Level.ResetResolution();

            //Reset UI Resolution
            DialogManagement?.ResetResolution();
            QuestionBox?.ResetResolution();
            if (Level.Loaded) Level.ResetResolution();
            Logger.Advice($"Resolution Setted!");
        }
    }
}
