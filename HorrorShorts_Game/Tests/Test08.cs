#if DEBUG
using HorrorShorts_Game.Algorithms.AStar;
using HorrorShorts_Game.Controls.Animations;
using HorrorShorts_Game.Controls.UI.Questions;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test08 : TestBase
    {
        public override void LoadContent1()
        {
            Core.QuestionBox.Show(Localizations.Test.Questions["Pregunta 1"]);
        }

        public override void Update1()
        {
            
        }
        public override void Draw1()
        {
#if DESKTOP
            Core.SpriteBatch.Draw(Textures.Pixel, Core.Controls.Mouse.Position.ToVector2(), null, Color.Red,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
#endif
        }
    }
}
#endif