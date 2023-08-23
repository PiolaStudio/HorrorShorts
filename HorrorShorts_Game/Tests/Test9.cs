#if DEBUG
using HorrorShorts_Game.Controls.UI;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test9 : TestBase
    {
        private Label _label;
        private string _text = "Test de Texto: ";
        public override void LoadContent1()
        {
            Core.Window.TextInput += Window_TextInput;
            _label = new(_text, 1, TextAlignament.TopLeft);
            _label.Y = 2;
        }

        public override void Update1()
        {
            _label.Update();
        }
        public override void Draw1()
        {
            _label.Draw();
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (e.Key == Keys.Back)
            {
                if (_text.Length > 0) _text = _text[..^1];
                _label.Text = _text;
                return;
            }

            char c = e.Character;
            if (!_label.SpriteFont.Characters.Contains(c)) return;
            _text += e.Character;

            int jumpLineIndex = _text.LastIndexOf('\n');
            int l = _text.Length - jumpLineIndex;
            if (l >= 50) _text += '\n';

            _label.Text = _text;
        }
    }
}
#endif