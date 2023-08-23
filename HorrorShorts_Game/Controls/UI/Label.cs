using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;

namespace HorrorShorts_Game.Controls.UI
{
    public class Label
    {
        private string _text = "Text";
        private FontType _font = FontType.Arial;
        private SpriteFont _spriteFont;
        private Vector2 _position = Vector2.Zero;
        private Color _textColor = Color.Black;
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
        public Vector2 Position { get => _position; set => _position = value; }
        public float X { get => _position.X; set => _position.X = value; }
        public float Y { get => _position.Y; set => _position.Y = value; }

        public Color TextColor { get => _textColor; set => _textColor = value; }
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

        public void Update()
        {
            if (_needCompute) Compute();
        }
        public void Draw()
        {
            Core.SpriteBatch.DrawString(_spriteFont, _text, _position, _textColor, 0f, _origin, _scale, SpriteEffects.None, 1f);
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
        }
        private void SetFont(FontType font)
        {
            this._font = font;
            _spriteFont = Fonts.Get(font);
        }
    }
}
