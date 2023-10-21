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

        private Buttons _leftButton = Buttons.DPadLeft;
        private Buttons _rightButton = Buttons.DPadRight;
        private Buttons _upButton = Buttons.DPadUp;
        private Buttons _downButton = Buttons.DPadDown;
        private Buttons _actionButton = Buttons.X;
        private Buttons _pauseButton = Buttons.Start;

        private Buttons? _leftButton2 = Buttons.LeftThumbstickLeft;
        private Buttons? _rightButton2 = Buttons.LeftThumbstickRight;
        private Buttons? _upButton2 = Buttons.LeftThumbstickUp;
        private Buttons? _downButton2 = Buttons.LeftThumbstickDown;
        private Buttons? _actionButton2 = Buttons.A;
        private Buttons? _pauseButton2 = null;

        public void Update()
        {
            _state = GamePad.GetState(PlayerIndex.One);

            if (!_state.IsConnected)
                return;

            //Left
            UpdateButtonState(_leftButton, _leftButton2, ref _leftTrigger, ref _leftPressed);

            //Right
            UpdateButtonState(_rightButton, _rightButton2, ref _rightTrigger, ref _rightPressed);

            //Up
            UpdateButtonState(_upButton, _upButton2, ref _upTrigger, ref _upPressed);

            //Down
            UpdateButtonState(_downButton, _downButton2, ref _downTrigger, ref _downPressed);

            //Action
            UpdateButtonState(_actionButton, _actionButton2, ref _actionTrigger, ref _actionPressed);

            //Pause
            UpdateButtonState(_pauseButton, _pauseButton2, ref _pauseTrigger, ref _pausePressed);
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

        public void SetPrimaryButtons(Buttons upButton, Buttons downButton, Buttons leftButton, Buttons rightButton, Buttons actionButton, Buttons pauseButton)
        {
            _upButton = upButton;
            _downButton = downButton;
            _leftButton = leftButton;
            _rightButton = rightButton;
            _actionButton = actionButton;
            _pauseButton = pauseButton;

            Logger.Advice($"Setted Gamepad Primary Buttons: " +
                $"UP: {upButton} -\t" +
                $"DOWN: {downButton} -\t" +
                $"LEFT: {leftButton} -\t" +
                $"RIGHT: {rightButton} -\t" +
                $"ACTION: {actionButton} -\t" +
                $"PAUSE: {pauseButton}");
        }
        public void SetSecondaryButtons(Buttons? upButton2, Buttons? downButton2, Buttons? leftButton2, Buttons? rightButton2, Buttons? actionButton2, Buttons? pauseButton2)
        {
            _upButton2 = upButton2;
            _downButton2 = downButton2;
            _leftButton2 = leftButton2;
            _rightButton2 = rightButton2;
            _actionButton2 = actionButton2;
            _pauseButton2 = pauseButton2;

            Logger.Advice($"Setted Gamepad Secundary Buttons: " +
                $"UP: {(upButton2 != null ? upButton2.ToString() : "NULL")} -\t" +
                $"DOWN: {(downButton2 != null ? downButton2.ToString() : "NULL")} -\t" +
                $"LEFT: {(leftButton2 != null ? leftButton2.ToString() : "NULL")} -\t" +
                $"RIGHT: {(rightButton2 != null ? rightButton2.ToString() : "NULL")} -\t" +
                $"ACTION: {(actionButton2 != null ? actionButton2.ToString() : "NULL")} -\t" +
                $"PAUSE: {(pauseButton2 != null ? pauseButton2.ToString() : "NULL")}");
        }
    }
}
#endif