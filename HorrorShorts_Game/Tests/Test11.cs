#if DEBUG
using HorrorShorts_Game.Algorithms.Tweener;
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Controls.UI;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    internal class Test11 : TestBase
    {
        private Sprite _sprite;
        private Tween<Point> _tween;
        private Label _label;

        public override void LoadContent1()
        {
            _sprite = new(Textures.Get(TextureType.Pixel), 0, 100, color: Color.Red);
            _tween = new(_sprite.Position, new(319, 100), 5000, function: TweenFunctions.PolynomialInOut);
            _tween.BucleType = TweenBucleType.PingPong;
            _tween.Start();

            _label = new();
            _label.X = 2;
            _label.Y = 2;
            _label.Color = Color.White;
            _label.Alignament = TextAlignament.TopLeft;
        }
        public override void Update1()
        {
            _tween.Update();
            if (_tween.IsUpdated)
            {
                _sprite.Position = _tween.Value;
                _sprite.Color = _tween.FunctionValue > 0.5f ? Color.Red : Color.Green;
                _label.Text = $"{_tween.LinearValue * 100f:0.0000}\n{_tween.FunctionValue * 100f:0.0000}\nX: {_sprite.X} Y: {_sprite.Y}";
            }

            if (Core.Controls.Keyboard.ActionTrigger)
                if (_tween.State == TweenState.Doing) _tween.Pause();
                else _tween.Resume();
            else if (Core.Controls.Keyboard.UpTrigger)
                _tween.Start();
            _label.Update();
        }
        public override void Draw1()
        {
            _sprite.Draw();
            _label.Draw();
        }
    }
}
#endif