using HorrorShorts.Controls.Sprites;
using HorrorShorts.Resources;
#if DEBUG
using HorrorShorts.Tests;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Principal;

namespace HorrorShorts
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

#if DEBUG
        private Test3 test = new();
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

            Core.Init(this, _graphics);
            base.Initialize();
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            Core.LoadContent();

#if DEBUG
            Dialogs.ReLoad(new string[] { nameof(Dialogs.Test) });
#endif
            Sounds.ReLoad(new string[] { "Speak1", "Speak2" });

#if DEBUG
            test?.LoadContent1();
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            Core.Update(gameTime);

#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            test?.Update1();
#endif

            base.Update(gameTime);
        }

        private void PreDraw(GameTime gameTime)
        {
            Core.DialogManagement.PreDraw();
#if DEBUG
            test?.PreDraw1();
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
            test?.Draw1();
#endif
            Core.DialogManagement.Draw();

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderFinal()
        {
            GraphicsDevice.Clear(Core.BackColor);
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