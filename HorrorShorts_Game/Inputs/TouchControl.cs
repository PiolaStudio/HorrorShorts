#if PHONE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Inputs
{
    public class TouchControl
    {
        private bool _click = false;
        private bool _clickFlag = false;
        private bool _leftPressed = false;

        private Point _realPosition = Point.Zero;
        private Point _virtualPositionScene = Point.Zero;
        private Point _virtualPositionUI = Point.Zero;

        private TouchLocation? _state = null;
        private TouchCollection _states;

        public Point Position { get => _virtualPositionScene; }
        public Point PositionUI { get => _virtualPositionUI; }
        public bool Click { get => _click; }


        public void Update()
        {
            _states = TouchPanel.GetState();
            if (_states.Count > 0) _state = _states[0];
            else _state = null;

            UpdatePosition();
            UpdateClick();
        }

        private void UpdatePosition()
        {
            if (!_state.HasValue) return;
            _realPosition = _state.Value.Position.ToPoint();

            //if (_realPosition.X < 0 || _realPosition.X > Core.Window.ClientBounds.Width)
            //    return;
            //if (_realPosition.Y < 0 || _realPosition.Y > Core.Window.ClientBounds.Height)
            //    return;

            //Out of Click Zone
            if (_realPosition.Y < Core.ClickZone.Top ||
                _realPosition.Y > Core.ClickZone.Bottom) return;

            //todo: remplazar por el verdadero Render Bounds (rectangulo que representa la camara)
            Rectangle renderBounds = new(0, Core.ResolutionBounds.Y, Core.ResolutionBounds.Width, Core.ResolutionBounds.Height);

            Vector2 inScenePerc = (_realPosition - Core.ClickZone.Location).ToVector2() / Core.ClickZone.Size.ToVector2();
            _virtualPositionScene = renderBounds.Location + (inScenePerc * renderBounds.Size.ToVector2()).ToPoint();

            _virtualPositionUI = new(
                Convert.ToInt32(_realPosition.X / (float)Core.ClickZone.Width * Settings.NativeResolution.Width),
                Convert.ToInt32(_realPosition.Y / (float)Core.ClickZone.Height * Settings.NativeResolution.Height));
        }
        private void UpdateClick()
        {
            if (_state.HasValue && _state.Value.State == TouchLocationState.Pressed && Core.Game.IsActive)
            {
                _leftPressed = true;
                if (!_clickFlag)
                {
                    _clickFlag = true;
                    _click = true;
                }
                else _click = false;
            }
            else
            {
                _clickFlag = false;
                _click = false;
                _leftPressed = false;
            }
        }
    }
}
#endif