using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Controls.Sprites
{
    [DebuggerDisplay("{Name} | {X} {Y} {Width} {Height}")]
    public class Sprite : ISprite
    {
        protected Texture2D _texture;
        protected Rectangle _rectangle;
        protected Rectangle _source;
        protected Color _color;
        protected Vector2 _origin;
        protected float _rotation;
        protected SpriteEffects _spriteEffect;
        protected float _depth;
        protected string _name;

        public Texture2D Texture { get => _texture; }

        public Point Position
        {
            get => _rectangle.Location;
            set => _rectangle.Location = value;
        }
        public int X
        {
            get => _rectangle.X;
            set => _rectangle.X = value;
        }
        public int Y
        {
            get => _rectangle.Y;
            set => _rectangle.Y = value;
        }

        public int Left
        {
            get => X;
            set => X = value;
        }
        public int Top
        {
            get => Y;
            set => Y = value;
        }
        public int Right
        {
            get => _rectangle.Right;
            set => _rectangle.X = value - _rectangle.Width;
        }
        public int Bottom
        {
            get => _rectangle.Bottom;
            set => _rectangle.Y = value - _source.Height; //todo: test
        }

        public int Width { get => _rectangle.Width; }
        public int Height { get => _rectangle.Height; }

        public Rectangle Rectangle { get => _rectangle; }

        public int VX 
        { 
            get => (int)Math.Floor(X - _origin.X); 
            set => _rectangle.X = (int)Math.Floor(value + _origin.X);
        }
        public int VY 
        { 
            get => (int)Math.Floor(Y - _origin.Y);
            set => _rectangle.Y = (int)Math.Floor(value + _origin.Y);
        }
        public Rectangle VirtualRectangle { get => new(VX, VY, Width, Height); }

        public Rectangle Source
        {
            get => _source;
            set
            {
                if (_source == value) return;
                Vector2 vo = VirtualOrigin;
                _source = value;
                _rectangle.Size = value.Size;
                _origin = _rectangle.Size.ToVector2() * vo;
            }
        }
        public int SourceX { get => _source.X; set => _source.X = value; }
        public int SourceY { get => _source.Y; set => _source.Y = value; }

        public Vector2 Origin { get => _origin; set => _origin = value; }
        public float OriginX { get => _origin.X; set => _origin.X = value; }
        public float OriginY { get => _origin.Y; set => _origin.Y = value; }

        public Vector2 VirtualOrigin 
        {
            get => new(VOriginX, VOriginY);
            set => _origin = new(VOriginX, VOriginY);
        }
        public float VOriginX
        {
            get => _origin.X / _source.Width;
            set => _origin.X = value * _source.Width;
        }
        public float VOriginY
        {
            get => _origin.Y / _source.Height;
            set => _origin.Y = value * _source.Height;
        }


        public Color Color { get => _color; set => _color = value; }
        public byte Alpha { get => _color.A; set => Color = new(_color, value); }


        public float Rotation { get => _rotation; set => _rotation = value; }
        public SpriteEffects SpriteEffect { get => _spriteEffect; set => _spriteEffect = value; }
        public float Depth { get => _depth; set => _depth = value; }

        public string Name { get => _name; }


        #region CONSTRUCTOR
        public Sprite(Texture2D texture, int x = 0, int y = 0, Rectangle? source = null, Color? color = null, Vector2 origin = default, float rotation = 0f, SpriteEffects flip = SpriteEffects.None, float depth = 1) 
            : this(texture.Name, texture, x, y, source, color ?? Color.White, origin, rotation, flip, depth)
        { }
        public Sprite(string name, Texture2D texture, int x = 0, int y= 0, Rectangle? source = null, Color? color = null, Vector2 origin = default, float rotation = 0f, SpriteEffects flip = SpriteEffects.None, float depth = 1) 
            : this(name, texture, x, y, source, color ?? Color.White, origin, rotation, flip, depth)
        { }

        public Sprite(Texture2D texture, Point position, Rectangle? source = null, Color? color = null, Vector2 origin = default, float rotation = 0f, SpriteEffects flip = SpriteEffects.None, float depth = 1) 
            : this(texture.Name, texture, position.X, position.Y, source, color ?? Color.White, origin, rotation, flip, depth)
        { }
        public Sprite(string name, Texture2D texture, Point position, Rectangle? source = null, Color? color = null, Vector2 origin = default, float rotation = 0f, SpriteEffects flip = SpriteEffects.None, float depth = 1) 
            : this(name, texture, position.X, position.Y, source, color ?? Color.White, origin, rotation, flip, depth)
        { }

        private Sprite(string name, Texture2D texture, int x, int y, Rectangle? source, Color color, Vector2 origin, float rotation, SpriteEffects flip, float depth)
        {
            if (source.HasValue) this._source = source.Value;
            else this._source = texture.Bounds;

            this._texture = texture;
            this._rectangle = new Rectangle(x, y, this._source.Width, this._source.Height);
            this._color = color;
            this._origin = origin;
            this._rotation = rotation;
            this.SpriteEffect = flip;
            this._depth = depth;
            this._name = name ?? "Default Name";
        }
        #endregion

        public void Draw()
        {
            Core.SpriteBatch.Draw(_texture, _rectangle, _source, _color, _rotation, _origin, _spriteEffect, _depth);
        }

        public void Draw(int x, int y)
        {
            Core.SpriteBatch.Draw(_texture, new Rectangle(x, y, _source.Width, _source.Height), _source, _color, _rotation, _origin, _spriteEffect, _depth);
        }
        public void Draw(Point position)
        {
            Core.SpriteBatch.Draw(_texture, new Rectangle(position, _source.Size), _source, _color, _rotation, _origin, _spriteEffect, _depth);
        }

#if DEBUG
        public void DrawVirtual()
        {
            Core.SpriteBatch.Draw(Textures.Pixel, VirtualRectangle, new Color(Color.Blue, 0.2f));
        }
#endif

        public Sprite Clone() => (Sprite)MemberwiseClone();
    }
}
