using HorrorShorts.Controls.UI;
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

        public static DialogManagement Dialog;

        public static KeyboardState KeyState;
        public static JoystickState JoystickState;

        public static void Init(Game1 game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = game.Content;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.Init();
            SpriteSheets.Init();
        }
        public static void Update()
        {
            KeyState = Keyboard.GetState();
            JoystickState = Joystick.GetState(0);
        }
    }
}
