using Microsoft.Xna.Framework;

namespace HorrorShorts_Game.Inputs
{
    public class InputControl
    {
#if DESKTOP
        public MouseControl Mouse { get => _mouse; }
        private MouseControl _mouse = new();
        public KeyboardControl Keyboard { get => _keyboard; }
        private KeyboardControl _keyboard = new();
#endif
#if DESKTOP || CONSOLE
        public GamePadControl GamePad { get => _gamePad; }
        private GamePadControl _gamePad = new();
#endif
#if PHONE
        public TouchControl Touch { get => _touch; }
        private TouchControl _touch = new TouchControl();
#endif

        private bool _click = false;
        private Point _clickPos = Point.Zero;

        public bool Click { get => _click; }
        public Point ClickPosition { get => _clickPos; }

        public void Update()
        {
#if DESKTOP
            _mouse.Update();
            if (_mouse.Click)
            {
                _click = true;
                _clickPos = _mouse.Position;
            }
            else _click = false;

            _keyboard.Update();
#endif
#if DESKTOP || CONSOLE
            _gamePad.Update();
#endif
#if PHONE
            _touch.Update();
            if (_touch.Click)
            {
                _click = true;
                _clickPos = _touch.Position;
            }
            else _click = false;
#endif
        }
    }
}
