using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Effects
{
    public class FadeInOut
    {
        public bool IsOut { get => _state == State.Out; }
        public bool IsIn { get => _state == State.In; }

        private State _state = State.Out;
        private enum State
        {
            Out,
            InterpolatingToOut,
            In,
            InterpolatingToIn
        }


        private float _interpolation = 0f;
        private float _interpolationMax = 1000f;

        private Color _color = Color.Black;

        public void Update()
        {
            if (_state == State.In || _state == State.Out) return;
            _interpolation += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;

            if (_interpolation >= _interpolationMax)
            {
                if (_state == State.InterpolatingToOut)
                {
                    _state = State.Out;
                    _color = new Color(Color.Black, 1f);
                }
                else
                {
                    _state = State.In;
                    _color = new Color(Color.Black, 0f);
                }
                _interpolation = _interpolationMax;
            }
            else
            {
                if (_state == State.InterpolatingToOut) 
                    _color = new Color(Color.Black, _interpolation / _interpolationMax); 
                else _color = new Color(Color.Black, 1f - _interpolation / _interpolationMax);
            }

        }
        public void Draw()
        {
            if (_state == State.In) return;
            Core.SpriteBatch.Draw(Textures.Pixel, Core.Resolution.Bounds, _color);
        }

        public void FadeIn(float duration = 500)
        {
            //if (_isOut) return;
            if (_state == State.In) return;

            //if (_state == State.InterpolatingToOut)
            //    _interpolation = (1 - (_interpolation / _interpolationMax)) * duration;

            _state = State.InterpolatingToIn;
            _interpolationMax = duration;
            _interpolation = 0;
        }
        public void FadeOut(float duration = 500)
        {
            if (_state == State.Out) return;

            //if (_state == State.InterpolatingToIn)
            //    _interpolation = (1 - (_interpolation / _interpolationMax)) * duration;

            _state = State.InterpolatingToOut;
            _interpolationMax = duration;
            _interpolation = 0;
        }
    }
}
