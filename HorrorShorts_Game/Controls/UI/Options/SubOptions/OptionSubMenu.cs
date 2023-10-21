using HorrorShorts_Game.Controls.UI.Interfaces;
using Microsoft.Xna.Framework;
using SharpFont.PostScript;
using System;
using System.Collections.Generic;

namespace HorrorShorts_Game.Controls.UI.Options.SubOptions
{
    internal abstract class OptionSubMenu : ILocalizable
    {
        //public static readonly Rectangle Bounds = new(0,0,206)
        protected const int LeftPad = 10;
        protected const int RightPad = 198;
        protected int InitY = 23;
        protected const int RowPad = 24;

        public int ScrollTop 
        { 
            get => _scrollTop;
            set 
            {
                _scrollTop = value;
                _scrollPoint = new(0, _scrollTop);
            }
        }
        protected int _scrollTop = 0;
        protected Point _scrollPoint = Point.Zero;

        protected List<Control> _controls = new();
        protected Control _focus = null;

        public Point PanelPosition;
        public bool NeedRender = true;

        public int Height { get; protected set; }

        public virtual void LoadContent() 
        {
            ComputePositions();
            foreach (Control control in _controls)
                control.FocusEvent += FocusControl_Event;

            SetLocalization();
        }
        public virtual void Update() 
        {
            bool needRender = false;
            foreach (Control control in _controls)
            {
                control.Update();
                needRender |= control.NeedRender;
            }

            UpdateStates();
        }
        public virtual void PreDraw() 
        {
            foreach (Control control in _controls)
                control.PreDraw();
        }
        public virtual void Draw() 
        {
            foreach (Control control in _controls)
                if (control != _focus)
                    control.Draw();

            _focus?.Draw();
        }
        public virtual void DrawOver() { }
        public virtual void Dispose() 
        {
            foreach (Control control in _controls)
                control.Dispose();
        }

        public virtual void ComputePositions() { }
        public virtual void ComputeVirtualPositions() { }
        protected virtual void UpdateStates() { }

        protected void FocusControl_Event(object o, EventArgs e)
        {
            if (_focus != null)
                _focus.RemoveFocus();
            _focus = (Control)o;
        }

        public virtual void SetLocalization() { }
    }
}
