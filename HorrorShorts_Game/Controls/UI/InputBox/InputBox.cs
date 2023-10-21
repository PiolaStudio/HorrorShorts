#if DESKTOP
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;

namespace HorrorShorts_Game.Controls.UI.InputBox
{
    public class InputBox : Control
    {
        protected Texture2D _texture;
        protected SpriteSheet _sheet;
        protected Rectangle _source;
        protected Rectangle _selectedSource;

        protected bool _waitingInput = false;

        public bool HasValue { get => _hasValue; }
        protected bool _hasValue = false;

        protected bool _needRefresh = false;

        public InputBox()
        {
            _zone = new(0, 0, 16, 16);
            _virtualZone = new(0, 0, 16, 16);
        }
        public override void LoadContent()
        {
            _texture = Textures.Get(TextureType.InputButtons);
            _sheet = SpriteSheets.Get(SpriteSheetType.InputButtons);
            _selectedSource = _sheet.Get("Selected");

            _needRefresh = false;
            UpdateSource();
        }
        public override void Update()
        {
            //todo
            if (IsEnable && IsVisible)
            {
                if (Core.Controls.Mouse.Click)
                {
                    Rectangle clickZone = _useVirtualZone ? _virtualZone : _zone;
                    if (clickZone.Contains(Core.Controls.Mouse.PositionUI))
                    {
                        Core.SoundManager.Play(SoundType.OptionChange);
                        _waitingInput = true;
                    }
                    else _waitingInput = false;
                }
            }

            if (_waitingInput)
                WaitingInputUpdate();

            if (_needRefresh)
            {
                _needRefresh = false;
                UpdateSource();
            }
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_texture, _zone, _source, Color.White);

            if (_waitingInput)
                Core.SpriteBatch.Draw(_texture, _zone, _selectedSource, Color.White);
        }

        protected virtual void WaitingInputUpdate() { }
        protected virtual void UpdateSource() { }
    }
}
#endif