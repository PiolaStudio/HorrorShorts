using HorrorShorts.Controls.Audio;
using HorrorShorts.Controls.UI.Dialogs;
using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts
{
    public static class Core
    {
        public static Game1 Game;

        public static SpriteBatch SpriteBatch;
        public static GraphicsDevice GraphicsDevice;
        public static ContentManager Content;
        public static GameTime GameTime;

        public static float DeltaTime; //todo
        private static readonly TimeSpan _idealFrameRate = TimeSpan.FromMilliseconds(1000 / 60.0);

        public static AudioManager AudioManager;
        public static DialogManagement DialogManagement;

        public static KeyboardState KeyState;
        public static JoystickState JoystickState;

        public static void Init(Game1 game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = game.Content;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            AudioManager = new AudioManager();
            DialogManagement = new DialogManagement();

            Textures.Init();
            SpriteSheets.Init();
            Fonts.Init();
            Sounds.Init();
        }
        public static void LoadContent()
        {
            DialogManagement.LoadContent();
        }
        public static void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / _idealFrameRate.TotalMilliseconds);

            KeyState = Keyboard.GetState();
            JoystickState = Joystick.GetState(0);

            DialogManagement.Update();
        }
        public static void Dispose()
        {
            DialogManagement.Dispose();
        }
    }
}
