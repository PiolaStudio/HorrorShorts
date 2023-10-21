using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;

namespace HorrorShorts_Game.Controls.UI
{
    public class IconBox : Control
    {
        private Texture2D _texture;
        private Rectangle? _source;
        public Rectangle? Source
        {
            get => _source;
            set
            {
                _source = value;

                if (_source.HasValue)
                    _zone.Size = _virtualZone.Size = _source.Value.Size;
                else if (_texture != null)
                    _zone.Size = _virtualZone.Size = _texture.Bounds.Size;
            }
        }

        public event EventHandler ClickEvent;

        public void SetTexture(Texture2D texture, Rectangle? source)
        {
            _texture = texture;
            _source = source;

            Point size = source.HasValue ? source.Value.Size : texture.Bounds.Size;
            _zone = new(_zone.X, _zone.Y, size.X, size.Y);
            _virtualZone.Size = size;
        }

        public override void Update()
        {
            //Input
            if (_isEnable && _isVisible)
            {
#if DESKTOP || PHONE
                if (Core.Controls.Click)
                {
                    Rectangle clickZone = _useVirtualZone ? _virtualZone : _zone;
                    if (clickZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        Core.SoundManager.Play(SoundType.OptionChange);
                        FireFocus();
                        ClickEvent?.Invoke(this, EventArgs.Empty);
                    }
                }
#endif
                //todo: otros controles
            }
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_texture, _zone, _source, Color.White);
        }
    }
}
