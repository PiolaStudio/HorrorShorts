using HorrorShorts_Game.Resources;
#if DEBUG
using HorrorShorts_Game.Tests;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;

namespace HorrorShorts_Game
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

#if DEBUG
        private readonly TestBase test = null;
        private static readonly Color ClearColor = new Color(84, 50, 98);
#else
        private static readonly Color clearColor = Color.Black;
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
            //Sounds.ReLoad(new SoundType[] { SoundType.Speak1, SoundType.Speak2, SoundType.Test1 }, out _);

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

            if (!Core.Level.Loaded) Core.Level.LoadContent();
            Core.Level.Update();

            base.Update(gameTime);
        }

        private void PreDraw(GameTime gameTime)
        {
            Core.DialogManagement.PreDraw();
            Core.QuestionBox.PreDraw();
            Core.Level.PreDraw();
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

            GraphicsDevice.Clear(ClearColor);

            if (Core.Level.Loaded)
            {
                //Layers
                LayerType[] layers = Enum.GetValues<LayerType>();
                for (int i = 0; i < layers.Length; i++)
                {
                    if (layers[i] == LayerType.UI) continue;
                    if (!Core.Level.LayerIsUsed(layers[i])) continue;

                    Core.SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied, transformMatrix: Core.Camera.Matrix);
                    Core.Level.Draw(layers[i]);
                    Core.SpriteBatch.End();
                }

                //UI
                Core.SpriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);
                Core.Level.DrawUI();
                Core.DialogManagement.Draw();
                Core.QuestionBox.Draw();
                Core.SpriteBatch.End();
            }

#if DEBUG
            Core.SpriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);
            test?.Draw1();
            Core.SpriteBatch.End();
#endif

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);
            Core.FadeEffect.Draw();
            Core.SpriteBatch.End();

            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderFinal()
        {
            GraphicsDevice.Clear(Core.BackColor);
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Core.Resolution.Matrix);
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