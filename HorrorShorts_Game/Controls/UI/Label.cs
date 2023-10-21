﻿using Assimp;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Controls.UI
{
    [DebuggerDisplay("{Text}")]
    public class Label : Control
    {
        private string _text = "Text";
        private FontType _font = FontType.Arial;
        private SpriteFont _spriteFont;
        private Vector2 _position = Vector2.Zero;
        private Color _color = Color.Black;
        private Vector2 _origin = Vector2.Zero;
        private float _scale = 1f;
        private TextAlignament _alignament = TextAlignament.MiddleCenter;

        private Vector2 _measure;
        private bool _needCompute = true;

        public FontType Font
        {
            get => _font;
            set
            {
                if (_font == value) return;
                SetFont(value);
            }
        }
        public SpriteFont SpriteFont { get => _spriteFont; }

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                _needCompute = true;
            }
        }
        public new Point Position
        {
            get => _position.ToPoint();
            set 
            { 
                _position = value.ToVector2();
                UpdateZone();
            }
        }
        public new int X 
        { 
            get => Convert.ToInt32(_position.X);
            set
            {
                _position.X = value;
                UpdateZone();
            }
        }
        public new int Y 
        { 
            get => Convert.ToInt32(_position.Y);
            set
            {
                _position.Y = value;
                UpdateZone();
            }
        }

        public Color Color { get => _color; set => _color = value; }
        public byte Alpha { get => _color.A; set => Color = new(_color, value); }

        public int Scale
        {
            get => Convert.ToInt32(_scale);
            set
            {
                if (_scale == value) return;
                _scale = value;
                _needCompute = true;
            }
        }
        public TextAlignament Alignament
        {
            get => _alignament;
            set
            {
                if (_alignament == value) return;
                _alignament = value;
                _needCompute = true;
            }
        }

        public Vector2 Measure { get => _measure; }
        public Rectangle Zone {  get => _zone; }

        public Label()
        {
            SetFont(FontType.Arial);
            _scale = 1f;
            Compute();
        }
        public Label(string text, int scale = 1, TextAlignament align = TextAlignament.MiddleCenter, FontType font = FontType.Arial)
        {
            SetFont(font);
            this._text = text;
            this._scale = scale;
            this._alignament = align;

            Compute();
        }

        public override void Update()
        {
            if (_needCompute) Compute();
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.DrawString(_spriteFont, _text, _position, _color, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }

        private void Compute()
        {
            _needCompute = false;
            _measure = _spriteFont.MeasureString(_text);
            _origin = _alignament switch
            {
                TextAlignament.TopLeft      => Vector2.Zero,
                TextAlignament.TopCenter    => new Vector2((float)Math.Floor(_measure.X / 2f), 0),
                TextAlignament.TopRight     => new Vector2((float)Math.Floor(_measure.X), 0),
                TextAlignament.MiddleLeft   => new Vector2(0, (float)Math.Floor(_measure.Y / 2f)),
                TextAlignament.MiddleCenter => new Vector2((float)Math.Floor(_measure.X / 2f), (float)Math.Floor(_measure.Y / 2f)),
                TextAlignament.MiddleRight  => new Vector2((float)Math.Floor(_measure.X), (float)Math.Floor(_measure.Y / 2f)),
                TextAlignament.BottomLeft   => new Vector2(0, (float)Math.Floor(_measure.Y / 2f)),
                TextAlignament.BottomCenter => new Vector2(0, (float)Math.Floor(_measure.Y / 2f)),
                TextAlignament.BottomRight  => new Vector2((float)Math.Floor(_measure.X), (float)Math.Floor(_measure.Y)),
                _ => throw new NotImplementedException("Not supported text alginament")
            };

            UpdateZone();
        }
        private void UpdateZone()
        {
            _zone = new(Convert.ToInt32(_position.X - _origin.X * _scale),
                        Convert.ToInt32(_position.Y - _origin.Y * _scale),
                        Convert.ToInt32(_measure.X),
                        Convert.ToInt32(_measure.Y));
        }
        private void SetFont(FontType font)
        {
            this._font = font;
            _spriteFont = Fonts.Get(font);
        }
    }
}
