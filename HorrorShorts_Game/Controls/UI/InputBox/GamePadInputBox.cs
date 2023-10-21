#if DESKTOP || CONSOLE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace HorrorShorts_Game.Controls.UI.InputBox
{
    [DebuggerDisplay("button: {Button}")]
    public class GamePadInputBox : InputBox
    {
        public enum GamepadType : byte
        {
            Unknown = 0,
            Xbox,
            Playstation,
            Switch
        }

        private static readonly Dictionary<Buttons, string> PlaystationSources;
        private static readonly Dictionary<Buttons, string> XboxSources;
        private static readonly Dictionary<Buttons, string> SwitchSources;
        private static readonly Dictionary<Buttons, string> GenericSources;
        static GamePadInputBox()
        {
            //Playstation
            PlaystationSources = new()
            {
                { Buttons.A, "Cross" },
                { Buttons.B, "Circle" },
                { Buttons.X, "Triangle" },
                { Buttons.Y, "Square" },

                { Buttons.Start, "Start_Type1" },
                { Buttons.Back, "Select" },
                { Buttons.BigButton, "Playstation_BigButton" },

                { Buttons.LeftShoulder, "L1" },
                { Buttons.RightShoulder, "R1" },
                { Buttons.RightTrigger, "R2" },
                { Buttons.LeftTrigger, "R1" },

                { Buttons.DPadUp, "DPadUp" },
                { Buttons.DPadDown, "DPadDown" },
                { Buttons.DPadLeft, "DPadLeft" },
                { Buttons.DPadRight, "DPadRight" },

                { Buttons.LeftStick, "LeftStick_Button" },
                { Buttons.RightStick, "RightStick_Button" },

                { Buttons.LeftThumbstickUp, "LeftStick_Up" },
                { Buttons.LeftThumbstickDown, "LeftStick_Down" },
                { Buttons.LeftThumbstickLeft, "LeftStick_Left" },
                { Buttons.LeftThumbstickRight, "LeftStick_Right" },

                { Buttons.RightThumbstickUp, "RightStick_Up" },
                { Buttons.RightThumbstickDown, "RightStick_Down" },
                { Buttons.RightThumbstickLeft, "RightStick_Left" },
                { Buttons.RightThumbstickRight, "RightStick_Right" }
            };

            //Xbox
            XboxSources = new()
            {
                { Buttons.A, "A_Type1" },
                { Buttons.B, "B_Type1" },
                { Buttons.X, "X_Type1" },
                { Buttons.Y, "Y_Type1" },

                { Buttons.Start, "Start_Type2" },
                { Buttons.Back, "Back" },
                { Buttons.BigButton, "XBOX_BigButton" },

                { Buttons.LeftShoulder, "LB" },
                { Buttons.RightShoulder, "RB" },
                { Buttons.LeftTrigger, "LT" },
                { Buttons.RightTrigger, "RT" },

                { Buttons.DPadUp, "DPadUp" },
                { Buttons.DPadDown, "DPadDown" },
                { Buttons.DPadLeft, "DPadLeft" },
                { Buttons.DPadRight, "DPadRight" },

                { Buttons.LeftStick, "LeftStick_Button" },
                { Buttons.RightStick, "RightStick_Button" },

                { Buttons.LeftThumbstickUp, "LeftStick_Up" },
                { Buttons.LeftThumbstickDown, "LeftStick_Down" },
                { Buttons.LeftThumbstickLeft, "LeftStick_Left" },
                { Buttons.LeftThumbstickRight, "LeftStick_Right" },

                { Buttons.RightThumbstickUp, "RightStick_Up" },
                { Buttons.RightThumbstickDown, "RightStick_Down" },
                { Buttons.RightThumbstickLeft, "RightStick_Left" },
                { Buttons.RightThumbstickRight, "RightStick_Right" }
            };

            //Switch
            SwitchSources = new()
            {
                { Buttons.A, "A_Type2" },
                { Buttons.B, "B_Type2" },
                { Buttons.X, "X_Type2" },
                { Buttons.Y, "Y_Type2" },

                { Buttons.Start, "More" },
                { Buttons.Back, "Minus" },
                { Buttons.BigButton, "Home" },

                { Buttons.LeftShoulder, "L" },
                { Buttons.RightShoulder, "R" },
                //{ Buttons.LeftTrigger, "LT" },
                //{ Buttons.RightTrigger, "RT" },

                { Buttons.DPadUp, "DPadUp" },
                { Buttons.DPadDown, "DPadDown" },
                { Buttons.DPadLeft, "DPadLeft" },
                { Buttons.DPadRight, "DPadRight" },

                { Buttons.LeftStick, "LeftStick_Button" },
                { Buttons.RightStick, "RightStick_Button" },

                { Buttons.LeftThumbstickUp, "LeftStick_Up" },
                { Buttons.LeftThumbstickDown, "LeftStick_Down" },
                { Buttons.LeftThumbstickLeft, "LeftStick_Left" },
                { Buttons.LeftThumbstickRight, "LeftStick_Right" },

                { Buttons.RightThumbstickUp, "RightStick_Up" },
                { Buttons.RightThumbstickDown, "RightStick_Down" },
                { Buttons.RightThumbstickLeft, "RightStick_Left" },
                { Buttons.RightThumbstickRight, "RightStick_Right" }
            };

            //Generic
            GenericSources = new()
            {
                { Buttons.A, "3" },
                { Buttons.B, "2" },
                { Buttons.X, "4" },
                { Buttons.Y, "1" },

                { Buttons.Start, "Start_Type2" },
                { Buttons.Back, "Back" },
                { Buttons.BigButton, "Home" },

                { Buttons.LeftShoulder, "L1" },
                { Buttons.RightShoulder, "R1" },
                { Buttons.LeftTrigger, "L2" },
                { Buttons.RightTrigger, "R2" },

                { Buttons.DPadUp, "DPadUp" },
                { Buttons.DPadDown, "DPadDown" },
                { Buttons.DPadLeft, "DPadLeft" },
                { Buttons.DPadRight, "DPadRight" },

                { Buttons.LeftStick, "LeftStick_Button" },
                { Buttons.RightStick, "RightStick_Button" },

                { Buttons.LeftThumbstickUp, "LeftStick_Up" },
                { Buttons.LeftThumbstickDown, "LeftStick_Down" },
                { Buttons.LeftThumbstickLeft, "LeftStick_Left" },
                { Buttons.LeftThumbstickRight, "LeftStick_Right" },

                { Buttons.RightThumbstickUp, "RightStick_Up" },
                { Buttons.RightThumbstickDown, "RightStick_Down" },
                { Buttons.RightThumbstickLeft, "RightStick_Left" },
                { Buttons.RightThumbstickRight, "RightStick_Right" }
            };
        }


        public Buttons Button
        {
            get => _button;
            set
            {
                if (_button == value) return;
                _button = value;
                _hasValue = _button != Buttons.None;
                _needRefresh = true;
            }
        }
        private Buttons _button;

        private GamepadType _gamepadType = GamepadType.Unknown;

        public event EventHandler<Buttons> ButtonInputDetected;

        protected override void WaitingInputUpdate()
        {
            foreach (Buttons button in Enum.GetValues<Buttons>())
            {
                if (Core.Controls.GamePad.State.IsButtonDown(button))
                {
                    Button = button;
                    _waitingInput = false;
                    ButtonInputDetected?.Invoke(this, Button);
                }
            }
        }
        protected override void UpdateSource()
        {
            if (!_hasValue) _source = _sheet.Get("Empty");
            else
            {
                Dictionary<Buttons, string> sources = _gamepadType switch
                {
                    GamepadType.Playstation => PlaystationSources,
                    GamepadType.Xbox => XboxSources,
                    GamepadType.Switch => SwitchSources,
                    GamepadType.Unknown => GenericSources,
                    _ => XboxSources
                };

                string sheetName = string.Empty;
                if (sources.ContainsKey(_button))
                    sheetName = "Gamepad_" + sources[_button];

                if (!string.IsNullOrEmpty(sheetName) && _sheet.TryGet(sheetName, out Rectangle sheet))
                {
                    _source = sheet;
                    _waitingInput = false;
                }
                else _source = _sheet.Get("Gamepad_ButtonEmpty");
            }
        }
    }
}
#endif