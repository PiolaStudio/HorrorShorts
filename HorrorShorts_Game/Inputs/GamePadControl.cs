#if DESKTOP || CONSOLE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HorrorShorts_Game.Inputs
{
    public class GamePadControl
    {
        private GamePadState _state;
        public GamePadState State { get => _state; }

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

        private Buttons _actionButton = Buttons.X;
        private Buttons? _actionButton2 = Buttons.A;

        public void Update()
        {
            _state = GamePad.GetState(PlayerIndex.One);

            if (!_state.IsConnected)
                return;

            //Left
            UpdateButtonState(Buttons.DPadLeft, Buttons.LeftThumbstickLeft, ref _leftTrigger, ref _leftPressed);

            //Right
            UpdateButtonState(Buttons.DPadRight, Buttons.RightThumbstickLeft, ref _rightTrigger, ref _rightPressed);

            //Up
            UpdateButtonState(Buttons.DPadUp, Buttons.LeftThumbstickUp, ref _upTrigger, ref _upPressed);

            //Down
            UpdateButtonState(Buttons.DPadDown, Buttons.LeftThumbstickDown, ref _downTrigger, ref _downPressed);

            //Action
            UpdateButtonState(_actionButton, _actionButton2, ref _actionTrigger, ref _actionPressed);

            //Pause
            UpdateButtonState(Buttons.Start, null, ref _pauseTrigger, ref _pausePressed);
        }

        private void UpdateButtonState(Buttons button, Buttons? button2, ref bool trigger, ref bool pressed)
        {
            if (DetectButtonPressed(button, button2))
            {
                if (!pressed) trigger = true;
                else trigger = false;
                pressed = true;
            }
            else
            {
                trigger = false;
                pressed = false;
            }
        }
        private bool DetectButtonPressed(Buttons button, Buttons? button2) =>
            _state.IsButtonDown(button) || (button2.HasValue && _state.IsButtonDown(button2.Value));

        public void SetButtons(Buttons actionButton, Buttons? actionButton2)
        {
            _actionButton = actionButton;
            _actionButton2 = actionButton2;
        }
    }
}
#endif