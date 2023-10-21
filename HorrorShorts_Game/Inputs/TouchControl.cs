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
        private bool _pressed = false;

        private Point _realPosition = Point.Zero;
        private Point _prevRealPosition = Point.Zero;
        private Point _virtualPositionScene = Point.Zero;
        private Point _virtualPositionUI = Point.Zero;
        private bool _positionChanged;

        private TouchLocation? _state = null;
        private TouchCollection _states;

        public Point Position { get => _virtualPositionScene; }
        public Point PositionUI { get => _virtualPositionUI; }
        public bool Click { get => _click; }
        public bool Pressed { get => _pressed; }


        public void Update()
        {
            _states = TouchPanel.GetState();
            if (_states.Count > 0) _state = _states.FirstOrDefault();
            else _state = null;

            UpdatePosition();
            UpdateClick();
        }
        private void UpdatePosition()
        {
            if (!_state.HasValue) return;
            if (!Core.Game.IsActive) return;

            //Out of Windows Zone
            _prevRealPosition = _realPosition;
            _realPosition = _state.Value.Position.ToPoint();
            _positionChanged = _realPosition != _prevRealPosition;

            //if (Core.Settings.ResizeMode != Settings.ResizeModes.FullScreen)
            //{
            //    if (_realPosition.X < 0 || _realPosition.X > Core.Window.ClientBounds.Width)
            //        return;
            //    if (_realPosition.Y < 0 || _realPosition.Y > Core.Window.ClientBounds.Height)
            //        return;
            //}

            //Out of Click Zone
            if (_realPosition.Y < Core.Resolution.ClickZone.Top ||
                _realPosition.Y > Core.Resolution.ClickZone.Bottom) return;

            //todo: remplazar por el verdadero Render Bounds (rectangulo que representa la camara)
            Rectangle renderBounds = new(0, Core.Resolution.Bounds.Y, Core.Resolution.Bounds.Width, Core.Resolution.Bounds.Height);

            Vector2 inClickZonePerc = (_realPosition - Core.Resolution.ClickZone.Location).ToVector2() / Core.Resolution.ClickZone.Size.ToVector2();
            _virtualPositionScene = renderBounds.Location + (inClickZonePerc * renderBounds.Size.ToVector2()).ToPoint();

            _virtualPositionUI = Core.Resolution.Bounds.Location + (inClickZonePerc * Core.Resolution.Bounds.Size.ToVector2()).ToPoint();
        }
        private void UpdateClick()
        {
            if (_state.HasValue 
                && (_state.Value.State == TouchLocationState.Pressed || _state.Value.State == TouchLocationState.Moved)
                && Core.Game.IsActive)
            {
                _pressed = true;
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
                _pressed = false;
            }
        }
    }
}
#endif