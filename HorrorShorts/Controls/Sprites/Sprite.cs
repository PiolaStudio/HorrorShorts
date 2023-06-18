using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Controls.Sprites
{
    //todo: esta clase requiere revisiones
    public class Sprite
    {
        protected Texture2D texture;
        protected Rectangle rectangle;
        protected Rectangle source;
        protected Color color;
        protected Vector2 origin;
        protected float rotation;
        protected SpriteEffects spriteEffect;
        protected float depth;

        public string Name { get; set; }

        public Texture2D Texture { get => texture; set => texture = value; }

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
        public int Right
        {
            get => rectangle.Right;
        }
        public int Bottom
        {
            get => rectangle.Bottom;
        }
        public Rectangle Rectangle { get => rectangle; }

        public int VX { get => (int)Math.Floor(X - origin.X); }
        public int VY { get => (int)Math.Floor(Y - origin.Y); }
        public Rectangle VirtualRectangle { get => new Rectangle(VX, VY, rectangle.Width, rectangle.Height); }

        public Rectangle Source
        {
            get => source;
            set
            {
                if (source == value) return;
                source = value;
                rectangle.Size = value.Size;
            }
        }
        public int SourceX { get => source.X; set => source.X = value; }
        public int SourceY { get => source.Y; set => source.Y = value; }

        public Vector2 Origin { get => origin; set => origin = value; }

        public Color Color { get => color; set => color = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public SpriteEffects SpriteEffect { get => spriteEffect; set => spriteEffect = value; }
        public float Depth { get => depth; set => depth = value; }

        public Sprite(Texture2D texture, int X, int Y, Rectangle? source, Color color)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(X, Y, this.source.Width, this.source.Height);
            this.color = color;
            origin = Vector2.Zero;
            rotation = 0f;
            spriteEffect = SpriteEffects.None;
            depth = 1f;
            Name = texture.Name;
        }
        public Sprite(Texture2D texture, Point position, Rectangle? source, Color color)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(position, this.source.Size);
            this.color = color;
            origin = Vector2.Zero;
            rotation = 0f;
            spriteEffect = SpriteEffects.None;
            depth = 1f;
            Name = texture.Name;
        }
        public Sprite(Texture2D texture, int X, int Y, Rectangle? source, Color color, Vector2 origin)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(X, Y, this.source.Width, this.source.Height);
            this.color = color;
            this.origin = origin;
            rotation = 0f;
            spriteEffect = SpriteEffects.None;
            depth = 1f;
            Name = texture.Name;
        }
        public Sprite(Texture2D texture, Point position, Rectangle? source, Color color, Vector2 origin)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(position, this.source.Size);
            this.color = color;
            this.origin = origin;
            rotation = 0f;
            spriteEffect = SpriteEffects.None;
            depth = 1f;
            Name = texture.Name;
        }

        public Sprite(Texture2D texture, int X, int Y, Rectangle? source, Color color, Vector2 origin, float rotation, SpriteEffects flip, float depth)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(X, Y, this.source.Width, this.source.Height);
            this.color = color;
            this.origin = origin;
            this.rotation = rotation;
            SpriteEffect = flip;
            this.depth = depth;
            Name = texture.Name;
        }
        public Sprite(Texture2D texture, Point position, Rectangle? source, Color color, Vector2 origin, float rotation, SpriteEffects flip, float depth)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(position, this.source.Size);
            this.color = color;
            this.origin = origin;
            this.rotation = rotation;
            SpriteEffect = flip;
            this.depth = depth;
            Name = texture.Name;
        }

        public Sprite(string Name, Texture2D texture, int X, int Y, Rectangle? source, Color color, Vector2 origin, float rotation, SpriteEffects flip, float depth)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(X, Y, this.source.Width, this.source.Height);
            this.color = color;
            this.origin = origin;
            this.rotation = rotation;
            SpriteEffect = flip;
            this.depth = depth;
            this.Name = Name;
        }
        public Sprite(string Name, Texture2D texture, Point position, Rectangle? source, Color color, Vector2 origin, float rotation, SpriteEffects flip, float depth)
        {
            if (source.HasValue) this.source = source.Value;
            else this.source = texture.Bounds;

            this.texture = texture;
            rectangle = new Rectangle(position, this.source.Size);
            this.color = color;
            this.origin = origin;
            this.rotation = rotation;
            SpriteEffect = flip;
            this.depth = depth;
            this.Name = Name;
        }

        public void Draw()
        {
            Core.SpriteBatch.Draw(texture, rectangle, source, color, rotation, origin, spriteEffect, depth);
            //General.SpriteBatch.Draw(Assets.Pixel, VirtualRectangle, new Color(Color.Blue, 0.2f));
        }
        public void Draw(Point position)
        {
            Core.SpriteBatch.Draw(texture, new Rectangle(position, source.Size), source, color, rotation, origin, spriteEffect, depth);
            //General.SpriteBatch.Draw(Assets.Pixel, VirtualRectangle, new Color(Color.Blue, 0.2f));
        }

        public Sprite Clone() => (Sprite)MemberwiseClone();
    }
}
