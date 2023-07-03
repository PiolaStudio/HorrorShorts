using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HorrorShorts
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        Texture2D t; //todo: borrar

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

            Core.Init(this, _graphics);
            base.Initialize();
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            Core.LoadContent();

            Textures.ReLoad(new string[] { "Mario", nameof(Textures.Girl1) });
            SpriteSheets.ReLoad(new string[] { "Mario", nameof(SpriteSheets.Girl1) });
#if DEBUG
            Dialogs.Load(new string[] { nameof(Dialogs.Test) });
#endif
            Sounds.ReLoad(new string[] { "Speak1", "Speak2" });

#if DEBUG
            Tests.Test.LoadContent();
#endif
            t = Content.Load<Texture2D>("Textures/Background1");
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
            //Pre Draw
            PreDraw(gameTime);

            //Render Native
            RenderNative();

            //Render Final
            RenderFinal();

            base.Draw(gameTime);
        }
        private void RenderNative()
        {
            Core.GraphicsDevice.SetRenderTarget(Core.Render);

            GraphicsDevice.Clear(Color.Black);
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
#if DEBUG
            Tests.Test.Draw();
#endif
            Core.SpriteBatch.Draw(t, Vector2.Zero, Color.White);
            Core.DialogManagement.Draw();

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderFinal()
        {
            GraphicsDevice.Clear(new(40, 40, 40));
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Core.ResolutionCamera);
            Core.SpriteBatch.Draw(Core.Render, Vector2.Zero, Color.White);
            Core.SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            Core.Dispose();
            base.Dispose(disposing);
        }
    }
}