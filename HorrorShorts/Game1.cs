using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources.Sprites;

namespace HorrorShorts
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Core.Init(this);
            Textures.ReLoad(new string[1] { "Mario" });
            SpriteSheets.ReLoad(new string[1] { "Mario" });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        private void PreDraw(GameTime gameTime)
        {

        }
        protected override void Draw(GameTime gameTime)
        {
            PreDraw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Core.SpriteBatch.Draw(Textures.Mario, new Rectangle(0, 0, 16 * 4, 16 * 4), SpriteSheets.Mario.Get("Death"), Color.White);
            Core.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}