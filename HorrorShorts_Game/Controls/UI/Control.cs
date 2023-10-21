using Microsoft.Xna.Framework;
using System;

namespace HorrorShorts_Game.Controls.UI
{
    public abstract class Control
    {
        protected Rectangle _zone;
        protected Rectangle _virtualZone;


        public Point Position
        {
            get => _zone.Location;
            set => _zone.Location = value;
        }
        public int X
        {
            get => _zone.X;
            set => _zone.X = value;
        }
        public int Y
        {
            get => _zone.Y;
            set => _zone.Y = value;
        }


        public Point VirtualPosition
        {
            get => _virtualZone.Location;
            set => _virtualZone.Location = value;
        }
        public int VirtualX
        {
            get => _virtualZone.X;
            set => _virtualZone.X = value;
        }
        public int VirtualY
        {
            get => _virtualZone.Y;
            set => _virtualZone.Y = value;
        }


        public bool UserVirtualZone { get => _useVirtualZone; set => _useVirtualZone = value; }
        protected bool _useVirtualZone = false;

        public bool NeedRender { get => _needRender; }
        private bool _needRender;

        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
        protected bool _isEnable = true;
        public bool IsFocus { get => _isFocus; } 
        protected bool _isFocus = false;
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        protected bool _isVisible = true;

        public object Tag = null;

        public event EventHandler FocusEvent;
        public event EventHandler UnfocusEvent;
        protected void FireFocus() => FocusEvent?.Invoke(this, EventArgs.Empty);
        protected void FireUnfocus() => UnfocusEvent?.Invoke(this, EventArgs.Empty);

        public virtual void LoadContent() { }
        public virtual void Update() { }
        public virtual void PreDraw() { }
        public virtual void Draw() { }
        public virtual void Dispose() { }

        public virtual void GetFocus()
        {
            if (_isFocus) return;
            FireFocus();
        }
        public virtual void RemoveFocus()
        {
            if (!_isFocus) return;
            FireUnfocus();
        }
    }
}
