#if DESKTOP
using Microsoft.Xna.Framework.Input;

namespace HorrorShorts_Game.Inputs
{
    public class KeyboardControl
    {
        private KeyboardState _state;
        public KeyboardState State { get => _state; }

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

        private Keys _upKey = Keys.Up;
        private Keys _downKey = Keys.Down;
        private Keys _leftKey = Keys.Left;
        private Keys _rightKey = Keys.Right;
        private Keys _actionKey = Keys.Space;
        private Keys _pauseKey = Keys.Escape;

        private Keys? _upKey2 = Keys.W;
        private Keys? _downKey2 = Keys.S;
        private Keys? _leftKey2 = Keys.A;
        private Keys? _rightKey2 = Keys.D;
        private Keys? _actionKey2 = Keys.Enter;
        private Keys? _pauseKey2 = null;

        public void Update()
        {
            _state = Keyboard.GetState();

            //Left
            UpdateKeyState(_leftKey, _leftKey2, ref _leftTrigger, ref _leftPressed);

            //Right
            UpdateKeyState(_rightKey, _rightKey2, ref _rightTrigger, ref _rightPressed);

            //Up
            UpdateKeyState(_upKey, _upKey2, ref _upTrigger, ref _upPressed);

            //Down
            UpdateKeyState(_downKey, _downKey2, ref _downTrigger, ref _downPressed);

            //Action
            UpdateKeyState(_actionKey, _actionKey2, ref _actionTrigger, ref _actionPressed);

            //Pause
            UpdateKeyState(_pauseKey, _pauseKey2, ref _pauseTrigger, ref _pausePressed);
        }

        private void UpdateKeyState(Keys key, Keys? key2, ref bool trigger, ref bool pressed)
        {
            if (DetectKeyPressed(key, key2))
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
        private bool DetectKeyPressed(Keys key, Keys? key2) =>
            _state.IsKeyDown(key) || (key2.HasValue && _state.IsKeyDown(key2.Value));

        public void SetPrimaryKeys(Keys upKey, Keys downKey, Keys leftKey, Keys rightKey, Keys actionKey, Keys pauseKey)
        {
            _upKey = upKey;
            _downKey = downKey;
            _leftKey = leftKey;
            _rightKey = rightKey;
            _actionKey = actionKey;
            _pauseKey = pauseKey;

            Logger.Advice($"Setted Keyboard Primary Keys: " +
                $"UP: {upKey} -\t" +
                $"DOWN: {downKey} -\t" +
                $"LEFT: {leftKey} -\t" +
                $"RIGHT: {rightKey} -\t" +
                $"ACTION: {actionKey} -\t" +
                $"PAUSE: {pauseKey}");
        }
        public void SetSecondaryKeys(Keys? upKey2, Keys? downKey2, Keys? leftKey2, Keys? rightKey2, Keys? actionKey2, Keys? pauseKey2)
        {
            _upKey2 = upKey2;
            _downKey2 = downKey2;
            _leftKey2 = leftKey2;
            _rightKey2 = rightKey2;
            _actionKey2 = actionKey2;
            _pauseKey2 = pauseKey2;

            Logger.Advice($"Setted Keyboard Secundary Keys: " +
                $"UP: {(upKey2 != null ? upKey2.ToString() : "NULL")} -\t" +
                $"DOWN: {(downKey2 != null ? downKey2.ToString() : "NULL")} -\t" +
                $"LEFT: {(leftKey2 != null ? leftKey2.ToString() : "NULL")} -\t" +
                $"RIGHT: {(rightKey2 != null ? rightKey2.ToString() : "NULL")} -\t" +
                $"ACTION: {(actionKey2 != null ? actionKey2.ToString() : "NULL")} -\t" +
                $"PAUSE: {(pauseKey2 != null ? pauseKey2.ToString() : "NULL")}");
        }
    }
}
#endif