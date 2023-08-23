using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
#if DEBUG
using HorrorShorts_Game.Tests;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;
using System.Collections;
using System.Security.Principal;
using System.Text;

namespace HorrorShorts_Game
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

#if DEBUG
        private readonly TestBase test = new Test9();
#endif

#if DEBUG
        public Game1(string testID) : this()
        {
            if (string.IsNullOrEmpty(testID))
                test = null;
            else if (testID != "default")
            {
                Type testType = Type.GetType("HorrorShorts_Game.Tests.Test" + testID);
                if (testType != null)
                {
                    Logger.Advice($"Loading Test '{testID}'");
                    test = (TestBase)Activator.CreateInstance(testType);
                }
            }
        }
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Advice("Game Initialize start...");
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

            Core.Init(this, _graphics);
            base.Initialize();
            _graphics.ApplyChanges();
            Logger.Advice("Game Initialize finish!");
        }

        protected override void LoadContent()
        {
            Core.LoadContent();

#if DEBUG
            Localizations.ReLoad(new string[] { nameof(Localizations.Test) });
#endif
            Sounds.ReLoad(new SoundType[] { SoundType.Speak1, SoundType.Speak2, SoundType.Test1 });

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
            Core.QuestionBox.PreDraw();
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

#if DEBUG
            GraphicsDevice.Clear(new Color(84, 50, 98));
#else
            GraphicsDevice.Clear(Color.Black);
#endif

            Core.SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);
#if DEBUG
            test?.Draw1();
#endif
            Core.DialogManagement.Draw();
            Core.QuestionBox.Draw();

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