using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.UI
{
    public class CheckBox : Control
    {
        private Texture2D _texture;
        private SpriteSheet _sheets;
        private Rectangle _source;

        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                _needCompute = true;
            }
        }
        private bool _checked = false;

        public new bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (_isEnable == value) return;
                _isEnable = value;
                _needCompute = true;
            }
        }


        public event EventHandler<bool> ClickEvent;

        public bool NeedCompute { get => _needCompute; }
        private bool _needCompute = true;
        public new bool NeedRender { get => _needCompute; }


        public CheckBox()
        {
            _zone = new(0, 0, 16, 16);
            _virtualZone = new(0, 0, 16, 16);
        }
        public override void LoadContent()
        {
            _texture = Textures.Get(TextureType.UIControls);
            _sheets = SpriteSheets.Get(SpriteSheetType.UIControls);
            _needCompute = true;
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
                        _checked = !_checked;
                        _needCompute = true;
                        FireFocus();
                        ClickEvent?.Invoke(this, _checked);
                    }
                }
#endif
                //todo: otros controles
            }

            //Refresh
            if (_needCompute)
            {
                _needCompute = false;

                string sheet = "CheckBox";
                sheet += _isEnable ? "_Enable" : "_Disable";
                sheet += _checked ? "_Tick" : "_Cross";

                _source = _sheets.Get(sheet);
            }
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_texture, _zone, _source, Color.White);
        }
    }
}
