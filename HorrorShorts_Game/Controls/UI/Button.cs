using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using System;

namespace HorrorShorts_Game.Controls.UI
{
    public class Button : Control
    {
        private Texture2D _texture;
        private RenderTarget2D _finalTexture;
        private SpriteSheet _sheet;
        private Zones _zones;
        private Label _label;

        private bool _needCompute = true;
        private bool _needSync = true;
        private bool _needRender = true;

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                _needSync = true;
                _needRender = true;
            }
        }
        private string _text = "BUTTON";

        public int Size
        {
            get => _size;
            set
            {
                if (_size == value) return;
                _size = value;
                _needCompute = true;
            }
        }
        private int _size = 1;

        public bool IsPushable
        {
            get => _isPushable;
            set => _isPushable = value;
        }
        private bool _isPushable = false;
        public bool IsPressed
        {
            get => _isPressed;
            set
            {
                if (_isPressed == value) return;
                _isPressed = value;
                _needRender = true;
            }
        }
        private bool _isPressed = false;

        public new bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (_isEnable == value) return;
                _isEnable = value;
                _needRender = true;
            }
        }

        public event EventHandler<bool> Click;

        public Button()
        {
            _zone = new(0, 0, 32, 16);
            _virtualZone = new(0, 0, 32, 16);
        }
        public override void LoadContent()
        {
            _texture = Textures.Get(TextureType.UIControls);
            _sheet = SpriteSheets.Get(SpriteSheetType.UIControls);

            _label = new();
            _label.Alignament = TextAlignament.MiddleCenter;
            _label.LoadContent();

            _needCompute = true;
            _needSync = true;
            _needRender = true;

            Compute();
            Sync();
        }
        public override void Update()
        {
            if (_needCompute) Compute();

            if (_isEnable && _isVisible)
            {
#if DESKTOP || PHONE
                if (Core.Controls.Click)
                {
                    Rectangle clickZone = _useVirtualZone ? _virtualZone : _zone;
                    if (clickZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        if (_isPushable) _isPressed = !_isPressed;
                        else _isPressed = true;
                        Click?.Invoke(this, _isPressed);
                        _needSync = true;
                    }
                }
#endif
                //todo
            }
            if (_isPressed && !Core.Controls.ClickPressed && !_needSync)
            {
                _isPressed = false;
                _needSync = true;
            }

            if (_needSync) Sync();

            base.Update();
        }
        public override void PreDraw()
        {
            if (!_isVisible) return;
            if (_needCompute) return;
            if (_needRender) Render();
        }
        public override void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_finalTexture, _zone, Color.White);
        }
        public override void Dispose()
        {
            if (_finalTexture != null && !_finalTexture.IsDisposed)
                _finalTexture.Dispose();
            if (_label != null) _label.Dispose();
        }

        private void Compute()
        {
            _needCompute = false;
            _zones = GetZones();
            _label.Position = new(_zone.Width / 2, _zone.Height / 2);

            if (_finalTexture == null || _finalTexture.IsDisposed || _finalTexture.Width != _zone.Width || _finalTexture.Height != _zone.Height)
            {
                if (_finalTexture != null && _finalTexture.IsDisposed)
                    _finalTexture.Dispose();

                _finalTexture = new(Core.GraphicsDevice, _zone.Width, _zone.Height);
            }

            _needSync = true;
            _needRender = true;
        }
        private void Sync()
        {
            _needSync = false;
            _label.Text = _text;
            _label.Update();
            _needRender = true;
        }
        private void Render()
        {
            _needRender = false;

            string sheet = "Button";
            if (!_isEnable || _isPressed) sheet += "_Disable";
            else sheet += "_Enable";

            Core.GraphicsDevice.SetRenderTarget(_finalTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin();
            Core.SpriteBatch.Draw(_texture, _zones.RenderZone[0], _sheet.Get($"{sheet}_Left"), Color.White);
            Core.SpriteBatch.Draw(_texture, _zones.RenderZone[1], _sheet.Get($"{sheet}_Middle"), Color.White);
            Core.SpriteBatch.Draw(_texture, _zones.RenderZone[2], _sheet.Get($"{sheet}_Right"), Color.White);
            _label.Draw();
            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }

        private Zones GetZones()
        {
            _zone.Width = _virtualZone.Width = 32 + _size * 16;

            Rectangle[] renderZones = new Rectangle[3];
            renderZones[0] = new(0, 0, 16, 16);
            renderZones[1] = new(16, 0, _zone.Width - 32, 16);
            renderZones[2] = new(_zone.Width - 16, 0, 16, 16);
            Point text = new(_zone.Width / 2, 8);
            return new(renderZones, text);
        }
        private readonly struct Zones
        {
            public readonly Rectangle[] RenderZone;
            public readonly Point Text;
            public Zones(Rectangle[] renderZone, Point text)
            {
                RenderZone = renderZone;
                Text = text;
            }
        }
    }
}
