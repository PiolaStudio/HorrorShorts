using HorrorShorts_Game.Controls.Audio;
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Questions;
using HorrorShorts_Game.Inputs;
using HorrorShorts_Game.Resources;
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

        public static SpriteBatch SpriteBatch;
        private static GraphicsDeviceManager GraphicsDeviceManager;
        public static GraphicsDevice GraphicsDevice;
        public static GameWindow Window;
        public static ContentManager Content;
        public static GameTime GameTime;
        public static RenderTarget2D Render;
        public static Settings Settings;

        public static float DeltaTime; //todo
        private static readonly TimeSpan _idealFrameRate = TimeSpan.FromMilliseconds(1000 / 60.0);

        //Audio
        public static SongManager SongManager;
        public static SoundManager SoundManager;

        public static DialogManagement DialogManagement;
        public static QuestionBox QuestionBox;

        public static readonly Color BackColor = new(40, 40, 40);


        public static Matrix ResolutionCamera;
        public static Rectangle ResolutionBounds;
        public static Rectangle ClickZone;

        public static InputControl Controls;

        public static KeyboardState KeyState;
        public static JoystickState JoystickState;

        public static void Init(Game1 game, GraphicsDeviceManager deviceManager)
        {
            Logger.Advice("Initing Core...");
            Game = game;
            GraphicsDeviceManager = deviceManager;
            GraphicsDevice = game.GraphicsDevice;
            Window = game.Window;

            Content = game.Content;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SoundManager = new();
            SongManager = new();
            Controls = new();
            DialogManagement = new();
            QuestionBox = new();

            Render = new(GraphicsDevice, Settings.NativeResolution.Width, Settings.NativeResolution.Height);

            Textures.Init();
            SpriteSheets.Init();
            Fonts.Init();
            Sounds.Init();
            Songs.Init();


            //Try Load Settings
            //todo: load settings from file
#if DEBUG
            Settings = new Settings(); //todo: borrar
            Settings.ResolutionWidth = 640;//todo: borrar
            Settings.ResolutionHeight = 640;//todo: borrar
            Settings.FullScreen = false;//todo: borrar
#endif

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
            game.Window.AllowUserResizing = true;
            game.Window.ClientSizeChanged += (s, e) =>
            {
                SetResolution(Window.ClientBounds.Width, Window.ClientBounds.Height, false);
                GraphicsDeviceManager.ApplyChanges();
            };
#endif

            Render = new RenderTarget2D(GraphicsDevice, Settings.NativeResolution.Width, Settings.NativeResolution.Height);
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

            KeyState = Keyboard.GetState();
            JoystickState = Joystick.GetState(0);

            Controls.Update();
            DialogManagement.Update();
            QuestionBox.Update();
            SongManager.Update();
        }
        public static void Dispose()
        {
            DialogManagement.Dispose();
            QuestionBox.Dispose();
        }

        public static void SetResolution(int width, int height, bool fullScreen)
        {
            Logger.Advice($"Setting resolution. {width} x {height} (FullScreen: {fullScreen})");
            Settings.ResolutionWidth = width;
            Settings.ResolutionHeight = height;
            Settings.FullScreen = fullScreen;

            GraphicsDeviceManager.PreferredBackBufferWidth = Settings.ResolutionWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = Settings.ResolutionHeight;
            GraphicsDeviceManager.IsFullScreen = Settings.FullScreen;

            //Calculate Resoultion Camera
            Rectangle nativeResolution = Settings.NativeResolution;

            float ratioAspect = width / (float)height;
            float nativeRatioAspect = nativeResolution.Width / (float)nativeResolution.Height;

            int baseHeight = (int)Math.Ceiling(nativeResolution.Width / ratioAspect);

            float scale = width / (float)nativeResolution.Width;

            float posY;

            posY = -(nativeResolution.Height - baseHeight) * scale;
            if (ratioAspect < nativeRatioAspect)
            {
                posY /= 2;
                ClickZone = new(0, (int)posY, width, Convert.ToInt32(height - posY * 2));
            }
            else ClickZone = new(0, 0, width, height);

            ResolutionBounds = new(0,
                                   Math.Max(nativeResolution.Height - baseHeight, 0),
                                   nativeResolution.Width,
                                   Math.Min(nativeResolution.Height, baseHeight));


            ResolutionCamera = Matrix.CreateScale(scale) * Matrix.CreateTranslation(0, posY, 0);

            //Reset UI Resolution
            DialogManagement?.ResetResolution();
            QuestionBox?.ResetResolution();
            Logger.Advice($"Resolution Setted!");
        }
    }
}
