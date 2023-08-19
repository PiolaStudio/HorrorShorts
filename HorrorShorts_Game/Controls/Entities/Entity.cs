using HorrorShorts_Game.Controls.Entities.Components;
using HorrorShorts_Game.Controls.Map;
using HorrorShorts_Game.Controls.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Entities
{
    public abstract class Entity : IEntity//, IMapLocation
    {
        public Sprite Sprite { get; internal set; }
        private Sprite _sprite;

        public Vector2 Position { get => _position; set => _position = value; }
        protected Vector2 _position = Vector2.Zero;
        public float X { get => _position.X; set => _position.X = value; }
        public float Y { get => _position.Y; set => _position.Y = value; }

        public bool Direction { get => _direction; set => _direction = value; }
        protected bool _direction = true;

        public float Altitude { get => _altitude; set => _altitude = value; }
        protected float _altitude = 0;

        public bool Visible { get => _visible; set => _visible = value; }
        protected bool _visible = true;

        public bool IsDisposed { get => _isDisposed; }
        protected bool _isDisposed = false;

        private float _moveSpeed = 10;
        private PathTracerComponent _pathTracer;

        private Point _locationInMap;

        public float CostOverMap { get => _costOverMap; }
        private float _costOverMap = 10;

        public virtual void Update() { }
        public virtual void PreDraw() { }
        public virtual void Draw()
        {
            if (!_visible) return;
            _sprite.Draw();
        }
        public virtual void Dispose()
        {
            _isDisposed = true;
        }


        public virtual void UpdateRectanglePosition()
        {
            _sprite.X = (int)Math.Floor(_position.X);
            _sprite.Y = (int)Math.Floor(_position.Y - _altitude);
        }
        public virtual void UpdateDirection()
        {
            if (_direction)
            {
                _sprite.SpriteEffect = SpriteEffects.None;
                //if (shadow != null) shadow.Sprite.SpriteEffect = SpriteEffects.None; //todo: descomentar si se activan las sombras
            }
            else
            {
                _sprite.SpriteEffect = SpriteEffects.FlipHorizontally;
                //if (shadow != null) shadow.Sprite.SpriteEffect = SpriteEffects.FlipHorizontally; //todo: descomentar si se activan las sombras
            }
        }
        public virtual void ApplyDepth()
        {
            float d = (_sprite.Bottom - _sprite.Origin.Y + _altitude) / 320f;
            _sprite.Depth = MathHelper.Clamp(d, 0f, 1f);
        }
        public virtual void UpdateShadow() //todo
        {
            //shadow.UpdatePosition(sprite, position);
            //shadow.UpdateAltitude(Altitude);
        }
        public virtual void UpdatePositionInMap()
        {
            //int x = Math.Floor(Position.X / (float)Core.Map.CellWidth);
            //int y = Math.Floor(Position.Y / (float)Core.Map.CellHeight);
            //Point newLocation = new(x, y);

            //if (_locationInMap != newLocation)
            //{
            //    Point prevLocation = _locationInMap;
            //    _locationInMap = newLocation;

            //    MapData md = new();
            //    md.MoveAt(prevLocation.X, prevLocation.Y, _locationInMap.X, _locationInMap.Y, this);
            //}
        }
    }
}
