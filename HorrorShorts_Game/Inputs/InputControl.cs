using Microsoft.Xna.Framework;
using System.Diagnostics;

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

#if DESKTOP || PHONE
        private bool _click = false;
        private bool _clickPressed = false;
        private Point _clickPos = Point.Zero;
        private Point _clickPosUI = Point.Zero;

        public bool Click { get => _click; }
        public bool ClickPressed { get => _clickPressed; }
        public Point ClickPosition { get => _clickPos; }
        public Point ClickPositionUI { get => _clickPosUI; }
#endif

#if DESKTOP || CONSOLE
        private bool _upPressed = false;
        private bool _downPressed = false;
        private bool _leftPressed = false;
        private bool _rightPressed = false;
        private bool _actionPressed = false;
        private bool _pausePressed = false;

        private bool _upTrigger = false;
        private bool _downTrigger = false;
        private bool _leftTrigger = false;
        private bool _rightTrigger = false;
        private bool _actionTrigger = false;
        private bool _pauseTrigger = false;

        public bool UpPressed { get => _upPressed; }
        public bool DownPressed { get => _downPressed; }
        public bool LeftPressed { get => _leftPressed; }
        public bool RightPressed { get => _rightPressed; }
        public bool ActionPressed { get => _actionPressed; }
        public bool PausePressed { get => _pausePressed; }

        public bool UpTrigger { get => _upTrigger; }
        public bool DownTrigger { get => _downTrigger; }
        public bool LeftTrigger { get => _leftTrigger; }
        public bool RightTrigger { get => _rightTrigger; }
        public bool ActionTrigger { get => _actionTrigger; }
        public bool PauseTrigger { get => _pauseTrigger; }
#endif


        public InputControl()
        {
#if DESKTOP
            Keyboard.SetPrimaryKeys(Core.Settings.Controls.UpKey,
                                    Core.Settings.Controls.DownKey,
                                    Core.Settings.Controls.LeftKey,
                                    Core.Settings.Controls.RightKey,
                                    Core.Settings.Controls.ActionKey,
                                    Core.Settings.Controls.PauseKey);

            Keyboard.SetSecondaryKeys(Core.Settings.Controls.UpKey2,
                                      Core.Settings.Controls.DownKey2,
                                      Core.Settings.Controls.LeftKey2,
                                      Core.Settings.Controls.RightKey2,
                                      Core.Settings.Controls.ActionKey2,
                                      Core.Settings.Controls.PauseKey2);
#endif
#if DESKTOP || CONSOLE
            GamePad.SetPrimaryButtons(Core.Settings.Controls.UpButton,
                                    Core.Settings.Controls.DownButton,
                                    Core.Settings.Controls.LeftButton,
                                    Core.Settings.Controls.RightButton,
                                    Core.Settings.Controls.ActionButton,
                                    Core.Settings.Controls.PauseButton);

            GamePad.SetSecondaryButtons(Core.Settings.Controls.UpButton2,
                                      Core.Settings.Controls.DownButton2,
                                      Core.Settings.Controls.LeftButton2,
                                      Core.Settings.Controls.RightButton2,
                                      Core.Settings.Controls.ActionButton2,
                                      Core.Settings.Controls.PauseButton2);
#endif
        }
        public void Update()
        {
            //Clicks
#if DESKTOP
            _mouse.Update();
            _click = _mouse.Click;
            if (_mouse.LeftPressed)
            {
                _clickPressed = true;
                _clickPos = _mouse.Position;
                _clickPosUI = _mouse.PositionUI;
            }
            else _clickPressed = false;

#elif PHONE
            _touch.Update();

            _click = _touch.Click;
            if (_touch.Pressed)
            {
                _clickPressed = true;
                _clickPos = _touch.Position;
                _clickPosUI = _touch.PositionUI;
            }
            else _clickPressed = false;
#endif


                //Buttons
#if DESKTOP || CONSOLE
            _upPressed = false;
            _downPressed = false;
            _leftPressed = false;
            _rightPressed = false;
            _actionPressed = false;
            _pausePressed = false;

            _upTrigger = false;
            _downTrigger = false;
            _leftTrigger = false;
            _rightTrigger = false;
            _actionTrigger = false;
            _pauseTrigger = false;
#endif
#if DESKTOP
            _keyboard.Update();

            _upPressed |= _keyboard.UpPressed;
            _downPressed |= _keyboard.DownPressed;
            _leftPressed |= _keyboard.LeftPressed;
            _rightPressed |= _keyboard.RightPressed;
            _actionPressed |= _keyboard.ActionPressed;
            _pausePressed |= _keyboard.PausePressed;

            _upTrigger |= _keyboard.UpTrigger;
            _downTrigger |= _keyboard.DownTrigger;
            _leftTrigger |= _keyboard.LeftTrigger;
            _rightTrigger |= _keyboard.RightTrigger;
            _actionTrigger |= _keyboard.ActionTrigger;
            _pauseTrigger |= _keyboard.PauseTrigger;
#endif
#if DESKTOP || CONSOLE
            _gamePad.Update();

            _upPressed |= _gamePad.UpPressed;
            _downPressed |= _gamePad.DownPressed;
            _leftPressed |= _gamePad.LeftPressed;
            _rightPressed |= _gamePad.RightPressed;
            _actionPressed |= _gamePad.ActionPressed;
            _pausePressed |= _gamePad.PausePressed;

            _upTrigger |= _gamePad.UpTrigger;
            _downTrigger |= _gamePad.DownTrigger;
            _leftTrigger |= _gamePad.LeftTrigger;
            _rightTrigger |= _gamePad.RightTrigger;
            _actionTrigger |= _gamePad.ActionTrigger;
            _pauseTrigger |= _gamePad.PauseTrigger;
#endif
        }
    }
}
