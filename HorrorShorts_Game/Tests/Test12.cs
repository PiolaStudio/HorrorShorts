#if DEBUG
using HorrorShorts_Game.Controls.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;
using System.Text;

namespace HorrorShorts_Game.Tests
{
    internal class Test12 : TestBase
    {
        private Label _label;

        public override void LoadContent1()
        {
            _label = new();
            _label.X = 2;
            _label.Y = 2;
            _label.Color = Color.White;
            _label.Alignament = TextAlignament.TopLeft;
        }
        public override void Update1()
        {
            StringBuilder sb = new();
            sb.Append("Butones: ");
#if DESKTOP || CONSOLE
            foreach (Buttons button in Enum.GetValues<Buttons>())
                if (Core.Controls.GamePad.State.IsButtonDown(button))
                    sb.Append(button.ToString() + "  ");
#endif
            _label.Text = sb.ToString();
            _label.Update();
        }
        public override void Draw1()
        {
            _label.Draw();
        }
    }
}
#endif