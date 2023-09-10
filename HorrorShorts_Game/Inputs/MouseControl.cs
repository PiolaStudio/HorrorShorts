#if DESKTOP
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Inputs
{
    public class MouseControl
    {
        private MouseState _state;
        public MouseState State { get => _state; }

        private bool _click = false;
        private bool _clickFlag = false;
        private bool _leftPressed = false;

        private Point _realPosition = Point.Zero;
        private Point _prevRealPosition = Point.Zero;
        private Point _virtualPositionScene = Point.Zero;
        private Point _virtualPositionUI = Point.Zero;
        private bool _positionChanged;

        public Point Position { get => _virtualPositionScene; }
        public Point PositionUI { get => _virtualPositionUI; }
        public bool Click { get => _click; }
        public bool LeftPressed { get => _leftPressed; }
        public bool PositionChanged { get => _positionChanged; }

        public void Update()
        {
            _state = Mouse.GetState();

            UpdatePosition();
            UpdateClick();
        }

        private void UpdatePosition()
        {
            if (!Core.Game.IsActive) return;

            //Out of Windows Zone
            _prevRealPosition = _realPosition;
            _realPosition = _state.Position;
            _positionChanged = _realPosition != _prevRealPosition;

            if (!Core.Settings.FullScreen)
            {
                if (_realPosition.X < 0 || _realPosition.X > Core.Window.ClientBounds.Width)
                    return;
                if (_realPosition.Y < 0 || _realPosition.Y > Core.Window.ClientBounds.Height)
                    return;
            }

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
            if (_state.LeftButton == ButtonState.Pressed && Core.Game.IsActive)
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