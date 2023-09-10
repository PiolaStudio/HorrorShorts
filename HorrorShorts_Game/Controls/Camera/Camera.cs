using Microsoft.Xna.Framework;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Camera
{
    public class Camera
    {
        private Matrix _matrix = Matrix.Identity;
        public Matrix Matrix { get => _matrix; }

        public float X 
        { 
            get => _position.X;
            set
            {
                if (_position.X == value) return;
                _position.X = value;
                AlignXWithLimits();
                _center.X = _position.X + Settings.NativeResolution.Width / 2f;
            }
        }
        public float Y 
        { 
            get => _position.Y;
            set
            {
                if (_position.Y == value) return;
                _position.Y = value;
                AlignYWithLimits();
                _center.Y = _position.Y + Settings.NativeResolution.Height / 2f;
            }
        }
        public Vector2 Position 
        { 
            get => _position;
            set
            {
                if (_position == value) return;
                _position = value;

                AlignXWithLimits();
                AlignYWithLimits();
                _center = new(_position.X + Settings.NativeResolution.Width / 2f,
                    _position.Y + Settings.NativeResolution.Height / 2f);
            }
        }

        private Vector2 _position = Vector2.Zero;
        private Vector2 _center = new(320, 320);

        public float Scale 
        { 
            get => _scale;
            set
            {
                if (_scale == value) return;
                _scale = value;
            }
        }
        private float _scale = 1f;

        public Rectangle? LimitBounds
        {
            get => _limitBounds;
            set
            {
                if (_limitBounds == value) return;
                _limitBounds = value;
                AlignXWithLimits();
                AlignYWithLimits();
                _center = new(_position.X + Settings.NativeResolution.Width / 2f,
                    _position.Y + Settings.NativeResolution.Height / 2f);
            }
        }
        private Rectangle? _limitBounds = new(0, 0, Settings.NativeResolution.Width, Settings.NativeResolution.Height);


        private void AlignXWithLimits()
        {
            if (_limitBounds == null) return;
            if (_position.X < _limitBounds.Value.Left)
                _position.X = _limitBounds.Value.Left;
            if (_position.X > _limitBounds.Value.Right - Settings.NativeResolution.Width)
                _position.X = _limitBounds.Value.Right - Settings.NativeResolution.Width;
        }
        private void AlignYWithLimits()
        {
            if (_limitBounds == null) return;
            if (_position.Y < _limitBounds.Value.Top)
                _position.Y = _limitBounds.Value.Top;
            if (_position.Y > _limitBounds.Value.Bottom - Settings.NativeResolution.Height)
                _position.Y = _limitBounds.Value.Bottom - Settings.NativeResolution.Height;
        }

        public Rectangle Bounds { get => _bounds; }
        private Rectangle _bounds;


        public void Update()
        {
            //todo: quitar
            //if (Core.Controls.Keyboard.UpPressed)
            //    Core.Camera.Y -= 0.5f;
            //else if (Core.Controls.Keyboard.DownPressed)
            //    Core.Camera.Y += 0.5f;
            //if (Core.Controls.Keyboard.LeftPressed)
            //    Core.Camera.X -= 0.5f;
            //else if (Core.Controls.Keyboard.RightPressed)
            //    Core.Camera.X += 0.5f;

            //if (Core.Controls.Keyboard.UpTrigger)
            //    _scale *= 2;
            //if (Core.Controls.Keyboard.DownTrigger)
            //    _scale /= 2;

            //if (Core.Controls.Keyboard.LeftTrigger)
            //    rot += 0.1f;
            //if (Core.Controls.Keyboard.RightTrigger)
            //    rot -= 0.1f;


            float scl = 160 - (160 / _scale);
            Vector2 finalPos = (_position * -1 - new Vector2(scl)) * _scale;
            Point pos = finalPos.ToPoint();

            _matrix = Matrix.CreateScale(_scale, _scale, 1f) * Matrix.CreateTranslation(pos.X, pos.Y, 0f);
            _bounds = new(pos, Core.Resolution.Bounds.Size);
        }
    }
}
