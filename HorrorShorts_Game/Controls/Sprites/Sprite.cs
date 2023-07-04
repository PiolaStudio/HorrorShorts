using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HorrorShorts.Controls.Sprites
{
    [DebuggerDisplay("{Name}")]
    public class Sprite : ISprite
    {
        protected Texture2D texture;
        protected Rectangle rectangle;
        protected Rectangle source;
        protected Color color;
        protected Vector2 origin;
        protected float rotation;
        protected SpriteEffects spriteEffect;
        protected float depth;
        protected string name;

        public Texture2D Texture { get => texture; }

        public Point Position
        {
            get => rectangle.Location;
            set => rectangle.Location = value;
        }
        public int X
        {
            get => rectangle.X;
            set => rectangle.X = value;
        }
        public int Y
        {
            get => rectangle.Y;
            set => rectangle.Y = value;
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
            get => rectangle.Right;
            set => rectangle.X = value - rectangle.Width;
        }
        public int Bottom
        {
            get => rectangle.Bottom;
            set => rectangle.Y = value - source.Height; //todo: test
        }

        public int Width { get => rectangle.Width; }
        public int Height { get => rectangle.Height; }

        public Rectangle Rectangle { get => rectangle; }

        public int VX 
        { 
            get => (int)Math.Floor(X - origin.X); 
            set => rectangle.X = (int)Math.Floor(value + origin.X);
        }
        public int VY 
        { 
            get => (int)Math.Floor(Y - origin.Y);
            set => rectangle.Y = (int)Math.Floor(value + origin.Y);
        }
        public Rectangle VirtualRectangle { get => new(VX, VY, Width, Height); }

        public Rectangle Source
        {
            get => source;
            set
            {
                if (source == value) return;
                source = value;
                rectangle.Size = value.Size;
                origin = VirtualOrigin;
            }
        }
        public int SourceX { get => source.X; set => source.X = value; }
        public int SourceY { get => source.Y; set => source.Y = value; }

        public Vector2 Origin { get => origin; set => origin = value; }
        public float OriginX { get => origin.X; set => origin.X = value; }
        public float OriginY { get => origin.Y; set => origin.Y = value; }

        public Vector2 VirtualOrigin 
        {
            get => new(VOriginX, VOriginY);
            set => origin = new(VOriginX, VOriginY);
        }
        public float VOriginX
        {
            get => origin.X / source.Width;
            set => origin.X = value * source.Width;
        }
        public float VOriginY
        {
            get => origin.Y / source.Height;
            set => origin.Y = value * source.Height;
        }


        public Color Color { get => color; set => color = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public SpriteEffects SpriteEffect { get => spriteEffect; set => spriteEffect = value; }
        public float Depth { get => depth; set => depth = value; }

        public string Name { get => name; }


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
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            this.rectangle = new Rectangle(x, y, this.source.Width, this.source.Height);
            this.color = color;
            this.origin = origin;
            this.rotation = rotation;
            this.SpriteEffect = flip;
            this.depth = depth;
            this.name = name;
        }
        #endregion

        public void Draw()
        {
            Core.SpriteBatch.Draw(texture, rectangle, source, color, rotation, origin, spriteEffect, depth);
        }

        public void Draw(int x, int y)
        {
            Core.SpriteBatch.Draw(texture, new Rectangle(x, y, source.Width, source.Height), source, color, rotation, origin, spriteEffect, depth);
        }
        public void Draw(Point position)
        {
            Core.SpriteBatch.Draw(texture, new Rectangle(position, source.Size), source, color, rotation, origin, spriteEffect, depth);
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
