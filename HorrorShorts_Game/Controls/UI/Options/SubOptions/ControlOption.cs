#if DESKTOP || CONSOLE
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Controls.UI.InputBox;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HorrorShorts_Game.Settings;

namespace HorrorShorts_Game.Controls.UI.Options.SubOptions
{
    internal class ControlOption : OptionSubMenu
    {
        //Top Controls
        private Button _resetBTN;
#if DESKTOP
        private IconBox _keyboardIBX;
        private IconBox _gamePadIBX;
#endif

        private Label _vibrationLBL;
        private CheckBox _vibrationCBX;

        private Texture2D _inputButtonTexture;
        private SpriteSheet _inputButtonSheet;

        //Titles
        private Label _primaryLBL;
        private Label _secundaryLBL;

        //Control Labels
        private Label _upLBL;
        private Label _downLBL;
        private Label _rightLBL;
        private Label _leftLBL;
        private Label _actionLBL;
        private Label _backLBL;


#if DESKTOP
        private InputType _inputType = InputType.Keyboard;
#else
        private InputType _inputType = InputType.Gamepad;
#endif
        private enum InputType : byte
        {
#if DESKTOP
            Keyboard,
            Mouse,
#endif
#if DESKTOP || CONSOLE
            Gamepad,
#endif
#if PHONE
            Touch
#endif
        }

        //Keyboard
#if DESKTOP
        private KeyInputBox _up1KIB;
        private KeyInputBox _up2KIB;
        private KeyInputBox _down1KIB;
        private KeyInputBox _down2KIB;
        private KeyInputBox _right1KIB;
        private KeyInputBox _right2KIB;
        private KeyInputBox _left1KIB;
        private KeyInputBox _left2KIB;
        private KeyInputBox _action1KIB;
        private KeyInputBox _action2KIB;
        private KeyInputBox _back1KIB;
        private KeyInputBox _back2KIB;
#endif

        //Console
        private GamePadInputBox _up1GIB;
        private GamePadInputBox _up2GIB;
        private GamePadInputBox _down1GIB;
        private GamePadInputBox _down2GIB;
        private GamePadInputBox _right1GIB;
        private GamePadInputBox _right2GIB;
        private GamePadInputBox _left1GIB;
        private GamePadInputBox _left2GIB;
        private GamePadInputBox _action1GIB;
        private GamePadInputBox _action2GIB;
        private GamePadInputBox _back1GIB;
        private GamePadInputBox _back2GIB;

        private Rectangle[] _rows = new Rectangle[3];

        public ControlOption()
        {
            Height = 200;
            InitY = 16;
        }
        public override void LoadContent()
        {
            _inputButtonTexture = Textures.Get(TextureType.InputButtons);
            _inputButtonSheet = SpriteSheets.Get(SpriteSheetType.InputButtons);

            //Top
#if DESKTOP
            _keyboardIBX = new();
            _keyboardIBX.UserVirtualZone = true;
            _keyboardIBX.ClickEvent += (s, e) =>
            {
                if (_inputType == InputType.Keyboard) return;
                _inputType = InputType.Keyboard;
                RefreshInputType();
            };
            _keyboardIBX.LoadContent();
            _keyboardIBX.SetTexture(_inputButtonTexture, _inputButtonSheet.Get("Keyboard_Icon_Selected"));
            _controls.Add(_keyboardIBX);

            _gamePadIBX = new();
            _gamePadIBX.UserVirtualZone = true;
            _gamePadIBX.ClickEvent += (s, e) =>
            {
                if (_inputType == InputType.Gamepad) return;
                _inputType = InputType.Gamepad;
                RefreshInputType();
            };
            _gamePadIBX.LoadContent();
            _gamePadIBX.SetTexture(_inputButtonTexture, _inputButtonSheet.Get("Gamepad_Icon"));
            _controls.Add(_gamePadIBX);
#endif

            _resetBTN = new();
            _resetBTN.Size = 2;
            _resetBTN.UserVirtualZone = true;
            _resetBTN.Click += (s, e) =>
            {
                Core.SoundManager.Play(SoundType.OptionChange);
                ResetControls();
            };
            _resetBTN.LoadContent();
            _controls.Add(_resetBTN);

            //Titles
            _primaryLBL = new();
            _primaryLBL.Alignament = TextAlignament.MiddleCenter;
            _controls.Add(_primaryLBL);

            _secundaryLBL = new();
            _secundaryLBL.Alignament = TextAlignament.MiddleRight;
            _controls.Add(_secundaryLBL);


            LoadInputLabels();
#if DESKTOP
            LoadKeyInputs();
#endif
            LoadGamepadInputs();

            RefreshInputType();
            base.LoadContent();
        }

        private void LoadInputLabels()
        {
            _upLBL = new();
            _upLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_upLBL);

            _downLBL = new();
            _downLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_downLBL);

            _leftLBL = new();
            _leftLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_leftLBL);

            _rightLBL = new();
            _rightLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_rightLBL);

            _actionLBL = new();
            _actionLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_actionLBL);

            _backLBL = new();
            _backLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_backLBL);
        }
#if DESKTOP
        private void LoadKeyInputs()
        {
            CreateKeyInputBox(Core.Settings.Controls.UpKey, Core.Settings.Controls.UpKey2, out _up1KIB, out _up2KIB);
            CreateKeyInputBox(Core.Settings.Controls.DownKey, Core.Settings.Controls.DownKey2, out _down1KIB, out _down2KIB);
            CreateKeyInputBox(Core.Settings.Controls.LeftKey, Core.Settings.Controls.LeftKey2, out _left1KIB, out _left2KIB);
            CreateKeyInputBox(Core.Settings.Controls.RightKey, Core.Settings.Controls.RightKey2, out _right1KIB, out _right2KIB);
            CreateKeyInputBox(Core.Settings.Controls.ActionKey, Core.Settings.Controls.ActionKey2, out _action1KIB, out _action2KIB);
            CreateKeyInputBox(Core.Settings.Controls.PauseKey, Core.Settings.Controls.PauseKey2, out _back1KIB, out _back2KIB);

            //Up
            _up1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.UpKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _up2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.UpKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };

            //Down
            _down1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.DownKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _down2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.DownKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };

            //Right
            _right1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.RightKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _right2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.RightKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };

            //Left
            _left1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.LeftKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _left2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.LeftKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };

            //Action
            _action1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.ActionKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _action2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.ActionKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };

            //Back
            _back1KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.PauseKey = e;
                Core.Settings.SetPrimaryKeys(Core.Settings.Controls);
            };
            _back2KIB.KeyInputDetected += (s, e) =>
            {
                Core.Settings.Controls.PauseKey2 = e;
                Core.Settings.SetSecundaryKeys(Core.Settings.Controls);
            };
        }
        private void CreateKeyInputBox(Keys k1, Keys? k2, out KeyInputBox inputBox1, out KeyInputBox inputBox2)
        {
            inputBox1 = new();
            inputBox1.Key = k1;
            inputBox1.UserVirtualZone = true;
            inputBox1.LoadContent();
            _controls.Add(inputBox1);

            inputBox2 = new();
            inputBox2.Key = k2 ?? Keys.None;
            inputBox2.UserVirtualZone = true;
            inputBox2.LoadContent();
            _controls.Add(inputBox2);
        }
#endif
        private void LoadGamepadInputs()
        {
            CreateGamepadInputBox(Core.Settings.Controls.UpButton, Core.Settings.Controls.UpButton2, out _up1GIB, out _up2GIB);
            CreateGamepadInputBox(Core.Settings.Controls.DownButton, Core.Settings.Controls.DownButton2, out _down1GIB, out _down2GIB);
            CreateGamepadInputBox(Core.Settings.Controls.LeftButton, Core.Settings.Controls.LeftButton2, out _left1GIB, out _left2GIB);
            CreateGamepadInputBox(Core.Settings.Controls.RightButton, Core.Settings.Controls.RightButton2, out _right1GIB, out _right2GIB);
            CreateGamepadInputBox(Core.Settings.Controls.ActionButton, Core.Settings.Controls.ActionButton2, out _action1GIB, out _action2GIB);
            CreateGamepadInputBox(Core.Settings.Controls.PauseButton, Core.Settings.Controls.PauseButton2, out _back1GIB, out _back2GIB);

            //Up
            _up1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.UpButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _up2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.UpButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };

            //Down
            _down1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.DownButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _down2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.DownButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };

            //Right
            _right1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.RightButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _right2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.RightButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };

            //Left
            _left1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.LeftButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _left2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.LeftButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };

            //Action
            _action1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.ActionButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _action2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.ActionButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };

            //Back
            _back1GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.PauseButton = e;
                Core.Settings.SetPrimaryButtons(Core.Settings.Controls);
            };
            _back2GIB.ButtonInputDetected += (s, e) =>
            {
                Core.Settings.Controls.PauseButton2 = e;
                Core.Settings.SetSecundaryButtons(Core.Settings.Controls);
            };
        }
        private void CreateGamepadInputBox(Buttons b1, Buttons? b2, out GamePadInputBox inputBox1, out GamePadInputBox inputBox2)
        {
            inputBox1 = new();
            inputBox1.Button = b1;
            inputBox1.UserVirtualZone = true;
            inputBox1.LoadContent();
            _controls.Add(inputBox1);

            inputBox2 = new();
            inputBox2.Button = b2 ?? Buttons.None;
            inputBox2.UserVirtualZone = true;
            inputBox2.LoadContent();
            _controls.Add(inputBox2);
        }

        public override void Update()
        {
            base.Update();
        }
        public override void PreDraw()
        {
            base.PreDraw();
        }
        public override void Draw()
        {
            base.Draw();
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        public override void ComputePositions()
        {
            int currentY = InitY;
            //todo
#if DESKTOP
            _keyboardIBX.Position = new(4, currentY - 8);
            _gamePadIBX.Position = new(36, currentY - 8);
#endif
            _resetBTN.Position = new(74, currentY - 8);
            currentY += RowPad;
            //todo
            //currentY += RowPad;

            //Titles
            _primaryLBL.Position = new(104, currentY);
            _secundaryLBL.Position = new(RightPad, currentY);
            currentY += RowPad;

            int key1X = 104 - 8;
            int key2X = RightPad - 40;

            //Up
            _upLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _up1KIB.Position = new(key1X, currentY - 8);
            _up2KIB.Position = new(key2X, currentY - 8);
#endif
            _up1GIB.Position = new(key1X, currentY - 8);
            _up2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            //Down
            _downLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _down1KIB.Position = new(key1X, currentY - 8);
            _down2KIB.Position = new(key2X, currentY - 8);
#endif
            _down1GIB.Position = new(key1X, currentY - 8);
            _down2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            //Right
            _rightLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _right1KIB.Position = new(key1X, currentY - 8);
            _right2KIB.Position = new(key2X, currentY - 8);
#endif
            _right1GIB.Position = new(key1X, currentY - 8);
            _right2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            //Left
            _leftLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _left1KIB.Position = new(key1X, currentY - 8);
            _left2KIB.Position = new(key2X, currentY - 8);
#endif
            _left1GIB.Position = new(key1X, currentY - 8);
            _left2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            //Action
            _actionLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _action1KIB.Position = new(key1X, currentY - 8);
            _action2KIB.Position = new(key2X, currentY - 8);
#endif
            _action1GIB.Position = new(key1X, currentY - 8);
            _action2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            //Back
            _backLBL.Position = new(LeftPad, currentY);
#if DESKTOP
            _back1KIB.Position = new(key1X, currentY - 8);
            _back2KIB.Position = new(key2X, currentY - 8);
#endif
            _back1GIB.Position = new(key1X, currentY - 8);
            _back2GIB.Position = new(key2X, currentY - 8);
            currentY += RowPad;

            ComputeVirtualPositions();
        }
        public override void ComputeVirtualPositions()
        {
#if DESKTOP
            _keyboardIBX.VirtualPosition = _keyboardIBX.Position + PanelPosition - _scrollPoint;
            _gamePadIBX.VirtualPosition = _gamePadIBX.Position + PanelPosition - _scrollPoint;
#endif
            _resetBTN.VirtualPosition = _resetBTN.Position + PanelPosition - _scrollPoint;

            Point pos1;
            Point pos2;

            //Up
            pos1 = _up1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _up2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _up1KIB.VirtualPosition = pos1;
            _up2KIB.VirtualPosition = pos2;
#endif
            _up1GIB.VirtualPosition = pos1;
            _up2GIB.VirtualPosition = pos2;

            //Down
            pos1 = _down1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _down2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _down1KIB.VirtualPosition = pos1;
            _down2KIB.VirtualPosition = pos2;
#endif
            _down1GIB.VirtualPosition = pos1;
            _down2GIB.VirtualPosition = pos2;

            //Right
            pos1 = _right1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _right2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _right1KIB.VirtualPosition = pos1;
            _right2KIB.VirtualPosition = pos2;
#endif
            _right1GIB.VirtualPosition = pos1;
            _right2GIB.VirtualPosition = pos2;

            //Left
            pos1 = _left1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _left2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _left1KIB.VirtualPosition = pos1;
            _left2KIB.VirtualPosition = pos2;
#endif
            _left1GIB.VirtualPosition = pos1;
            _left2GIB.VirtualPosition = pos2;

            //Action
            pos1 = _action1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _action2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _action1KIB.VirtualPosition = pos1;
            _action2KIB.VirtualPosition = pos2;
#endif
            _action1GIB.VirtualPosition = pos1;
            _action2GIB.VirtualPosition = pos2;

            //Back
            pos1 = _back1KIB.Position + PanelPosition - _scrollPoint;
            pos2 = _back2KIB.Position + PanelPosition - _scrollPoint;
#if DESKTOP
            _back1KIB.VirtualPosition = pos1;
            _back2KIB.VirtualPosition = pos2;
#endif
            _back1GIB.VirtualPosition = pos1;
            _back2GIB.VirtualPosition = pos2;
        }
        protected override void UpdateStates()
        {
            if (_inputType == InputType.Gamepad)
            {
                _upLBL.Color = CheckDupped(_up1GIB) || CheckDupped(_up2GIB) ? Color.Red : Color.Black;
                _downLBL.Color = CheckDupped(_down1GIB) || CheckDupped(_down2GIB) ? Color.Red : Color.Black;
                _leftLBL.Color = CheckDupped(_left1GIB) || CheckDupped(_left2GIB) ? Color.Red : Color.Black;
                _rightLBL.Color = CheckDupped(_right1GIB) || CheckDupped(_right2GIB) ? Color.Red : Color.Black;
                _actionLBL.Color = CheckDupped(_action1GIB) || CheckDupped(_action2GIB) ? Color.Red : Color.Black;
                _backLBL.Color = CheckDupped(_back1GIB) || CheckDupped(_back2GIB) ? Color.Red : Color.Black;
            }
#if DESKTOP
            else if (_inputType == InputType.Keyboard)
            {
                _upLBL.Color = CheckDupped(_up1KIB) || CheckDupped(_up2KIB) ? Color.Red : Color.Black;
                _downLBL.Color = CheckDupped(_down1KIB) || CheckDupped(_down2KIB) ? Color.Red : Color.Black;
                _leftLBL.Color = CheckDupped(_left1KIB) || CheckDupped(_left2KIB) ? Color.Red : Color.Black;
                _rightLBL.Color = CheckDupped(_right1KIB) || CheckDupped(_right2KIB) ? Color.Red : Color.Black;
                _actionLBL.Color = CheckDupped(_action1KIB) || CheckDupped(_action2KIB) ? Color.Red : Color.Black;
                _backLBL.Color = CheckDupped(_back1KIB) || CheckDupped(_back2KIB) ? Color.Red : Color.Black;
            }
#endif

            base.UpdateStates();
        }
#if DESKTOP
        private bool CheckDupped(KeyInputBox control)
        {
            if (control.Key == Keys.None) return false;

            for (int i = 0; i < _controls.Count; i++)
            {
                if (_controls[i].GetType() != typeof(KeyInputBox)) continue;
                if (_controls[i] == control) continue;

                if (((KeyInputBox)_controls[i]).Key == control.Key)
                    return true;
            }

            return false;
        }
#endif
        private bool CheckDupped(GamePadInputBox control)
        {
            if (control.Button == Buttons.None) return false;

            for (int i = 0; i < _controls.Count; i++)
            {
                if (_controls[i].GetType() != typeof(GamePadInputBox)) continue;
                if (_controls[i] == control) continue;

                if (((GamePadInputBox)_controls[i]).Button == control.Button)
                    return true;
            }

            return false;
        }

        private void RefreshInputType()
        {
#if DESKTOP
            bool keyboardVisible = _inputType == InputType.Keyboard;
            _up1KIB.IsVisible = keyboardVisible;
            _up2KIB.IsVisible = keyboardVisible;
            _down1KIB.IsVisible = keyboardVisible;
            _down2KIB.IsVisible = keyboardVisible;
            _right1KIB.IsVisible = keyboardVisible;
            _right2KIB.IsVisible = keyboardVisible;
            _left1KIB.IsVisible = keyboardVisible;
            _left2KIB.IsVisible = keyboardVisible;
            _action1KIB.IsVisible = keyboardVisible;
            _action2KIB.IsVisible = keyboardVisible;
            _back1KIB.IsVisible = keyboardVisible;
            _back2KIB.IsVisible = keyboardVisible;
#endif

            bool gamepadVisible =  _inputType == InputType.Gamepad;
            _up1GIB.IsVisible = gamepadVisible;
            _up2GIB.IsVisible = gamepadVisible;
            _down1GIB.IsVisible = gamepadVisible;
            _down2GIB.IsVisible = gamepadVisible;
            _right1GIB.IsVisible = gamepadVisible;
            _right2GIB.IsVisible = gamepadVisible;
            _left1GIB.IsVisible = gamepadVisible;
            _left2GIB.IsVisible = gamepadVisible;
            _action1GIB.IsVisible = gamepadVisible;
            _action2GIB.IsVisible = gamepadVisible;
            _back1GIB.IsVisible = gamepadVisible;
            _back2GIB.IsVisible = gamepadVisible;

            if (gamepadVisible)
            {
                _keyboardIBX.Source = _inputButtonSheet.Get("Keyboard_Icon");
                _gamePadIBX.Source = _inputButtonSheet.Get("Gamepad_Icon_Selected");
            }
#if DESKTOP
            else if (keyboardVisible)
            {
                _keyboardIBX.Source = _inputButtonSheet.Get("Keyboard_Icon_Selected");
                _gamePadIBX.Source = _inputButtonSheet.Get("Gamepad_Icon");
            }
#endif
        }
        private void ResetControls()
        {
            Settings.ControlSettings defaults = new();

#if DESKTOP
            _up1KIB.Key = defaults.UpKey;
            _up2KIB.Key = defaults.UpKey2 ?? Keys.None;
            _down1KIB.Key = defaults.DownKey;
            _down2KIB.Key = defaults.DownKey2 ?? Keys.None;
            _right1KIB.Key = defaults.RightKey;
            _right2KIB.Key = defaults.RightKey2 ?? Keys.None;
            _left1KIB.Key = defaults.LeftKey;
            _left2KIB.Key = defaults.LeftKey2 ?? Keys.None;
            _action1KIB.Key = defaults.ActionKey;
            _action2KIB.Key = defaults.ActionKey2 ?? Keys.None;
            _back1KIB.Key = defaults.PauseKey;
            _back2KIB.Key = defaults.PauseKey2 ?? Keys.None;
            Core.Settings.SetPrimaryKeys(defaults);
            Core.Settings.SetSecundaryKeys(defaults);
#endif

            _up1GIB.Button = defaults.UpButton;
            _up2GIB.Button = defaults.UpButton2 ?? Buttons.None;
            _down1GIB.Button = defaults.DownButton;
            _down2GIB.Button = defaults.DownButton2 ?? Buttons.None;
            _right1GIB.Button = defaults.RightButton;
            _right2GIB.Button = defaults.RightButton2 ?? Buttons.None;
            _left1GIB.Button = defaults.LeftButton;
            _left2GIB.Button = defaults.LeftButton2 ?? Buttons.None;
            _action1GIB.Button = defaults.ActionButton;
            _action2GIB.Button = defaults.ActionButton2 ?? Buttons.None;
            _back1GIB.Button = defaults.PauseButton;
            _back2GIB.Button = defaults.PauseButton2 ?? Buttons.None;
            Core.Settings.SetPrimaryButtons(defaults);
            Core.Settings.SetSecundaryButtons(defaults);
        }


        public override void SetLocalization()
        {
            _resetBTN.Text = Localizations.Global.Options_Control_Reset.ToUpper();
            _primaryLBL.Text = Localizations.Global.Options_Control_Primary.ToUpper();
            _secundaryLBL.Text = Localizations.Global.Options_Control_Secundary.ToUpper();

            _upLBL.Text = Localizations.Global.Options_Control_Up;
            _downLBL.Text = Localizations.Global.Options_Control_Down;
            _rightLBL.Text = Localizations.Global.Options_Control_Left;
            _leftLBL.Text = Localizations.Global.Options_Control_Right;
            _actionLBL.Text = Localizations.Global.Options_Control_Action;
            _backLBL.Text = Localizations.Global.Options_Control_Back;

            base.SetLocalization();
        }
    }
}
#endif