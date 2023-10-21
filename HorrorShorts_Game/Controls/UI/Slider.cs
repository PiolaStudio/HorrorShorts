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
    public class Slider : Control
    {
        private Texture2D _texture;
        private SpriteSheet _sheets;

        private Rectangle _arrowZone;
        private Rectangle _sliderSource;
        private Rectangle _arrowSource;

        private bool _needCompute = true;

        public event EventHandler<float> ChangeValueEvent;
        public event EventHandler DragOnEvent;
        public event EventHandler DragOutEvent;

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

        public float Value 
        { 
            get => _value; 
            set
            {
                if (_value == value) return;
                _value = value;
                _needCompute = true;
            }
        }
        private float _value = 0f;

        public bool IsDragging { get => _isDragging; }
        private bool _isDragging = false;

        public Slider()
        {
            _zone = new(0, 0, 80, 16);
            _virtualZone = new(0, 0, 80, 16);
        }
        public override void LoadContent()
        {
            _texture = Textures.Get(TextureType.UIControls);
            _sheets = SpriteSheets.Get(SpriteSheetType.UIControls);
            _arrowSource = _sheets.Get("Slider_Arrow");
            _arrowZone = new(0, 0, 8, 8);
            _needCompute = true;
        }
        public override void Update()
        {
            //Input
            if (_isEnable && _isVisible)
            {
                if (!_isDragging && Core.Controls.Click)
                {
                    Rectangle clickZone = _useVirtualZone ? _virtualZone : _zone;
                    if (clickZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        DragOnEvent?.Invoke(this, EventArgs.Empty);
                        _isDragging = true;
                        FireFocus();
                    }
                }
                if (_isDragging)
                {
#if DESKTOP || PHONE
                    if (Core.Controls.ClickPressed)
                    {
                        Rectangle clickZone = _useVirtualZone ? _virtualZone : _zone;

                        int mouseX = Core.Controls.ClickPositionUI.X;
                        float value;

                        if (mouseX < clickZone.Left) value = 0f;
                        else if (mouseX > clickZone.Right) value = 1f;
                        else value = (mouseX - clickZone.X) / (float)clickZone.Width;

                        if (value != _value)
                        {
                            _value = value;
                            _needCompute = true;
                            //Core.SoundManager.Play(SoundType.OptionChange);
                            ChangeValueEvent?.Invoke(this, _value);
                        }
                    }
                    else
                    { 
                        _isDragging = false;
                        DragOutEvent?.Invoke(this, EventArgs.Empty);

                    }
#endif
                }

                //todo: otros controles
            }


            //Refresh
            if (_needCompute)
            {
                _needCompute = false;

                string sheet = "Slider_Bar";
                if (_isEnable) sheet += "_Enable";
                else sheet += "_Disable";

                _sliderSource = _sheets.Get(sheet);

                _arrowZone.Location = new(
                    X + 2 + Convert.ToInt32(78 * _value) - 4, 
                    Convert.ToInt32(Y + 12));
            }
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_texture, _zone, _sliderSource, Color.White);
            Core.SpriteBatch.Draw(_texture, _arrowZone, _arrowSource, Color.White);
        }
    }
}
