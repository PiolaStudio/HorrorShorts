#if DESKTOP
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Controls.UI.InputBox
{
    [DebuggerDisplay("Key: {Key}")]
    public class KeyInputBox : InputBox
    {
        public Keys Key 
        { 
            get => _key;
            set
            {
                if (_key == value) return;
                _key = value;
                _hasValue = _key != Keys.None;
                _needRefresh = true;
            }
        }
        public Keys _key = Keys.None;

        public event EventHandler<Keys> KeyInputDetected;

        protected override void WaitingInputUpdate()
        {
            Keys[] keys = Core.Controls.Keyboard.State.GetPressedKeys();
            if (keys.Length > 0)
            {
                Keys key = keys[0];
                Key = key;
                _waitingInput = false;
                KeyInputDetected?.Invoke(this, Key);
            }
        }
        protected override void UpdateSource()
        {
            if (!_hasValue) _source = _sheet.Get("Empty");
            else
            {
                if (_sheet.TryGet($"Keyboard_{_key}", out Rectangle sheet))
                {
                    _source = sheet;
                    _waitingInput = false;
                }
                else _source = _sheet.Get("Keyboard_Unknown");
            }
        }
    }
}
#endif