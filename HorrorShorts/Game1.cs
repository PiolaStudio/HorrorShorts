using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();
            this.IsFixedTimeStep = true;
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Core.Init(this);
            Core.LoadContent();

            Textures.ReLoad(new string[] { "Mario", nameof(Textures.Girl1) });
            SpriteSheets.ReLoad(new string[] { "Mario", nameof(SpriteSheets.Girl1) });
            Dialogs.Load(new string[] { nameof(Dialogs.Test) });
            Sounds.ReLoad(new string[] { "Speak1", "Speak2" });

#if DEBUG
            Tests.Test.LoadContent();
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            Core.Update(gameTime);

            Window.Title = Core.DeltaTime.ToString();
#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Tests.Test.Update();
#endif


            base.Update(gameTime);
        }

        private void PreDraw(GameTime gameTime)
        {
            Core.DialogManagement.PreDraw();
#if DEBUG
            Tests.Test.PreDraw();
#endif
        }
        protected override void Draw(GameTime gameTime)
        {
            PreDraw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
#if DEBUG
            Tests.Test.Draw();
#endif

            //Core.SpriteBatch.Draw(Textures.Mario, new Rectangle(0, 0, 16 * 4, 16 * 4), SpriteSheets.Mario.Get("Death"), Color.White);
            Core.DialogManagement.Draw();
            Core.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            Core.Dispose();
            base.Dispose(disposing);
        }
    }
}