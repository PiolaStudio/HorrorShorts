using HorrorShorts_Game.Controls.Camera;
using HorrorShorts_Game.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HorrorShorts_Game
{
    public class Settings
    {
        public static readonly Rectangle NativeResolution = new(0, 0, 320, 320);
#if DESKTOP
        public int WindowX;
        public int WindowY;
#endif

        public enum ResizeModes : byte
        {
            FullScreen = 0,
#if DESKTOP
            Bordeless,
            Window
#endif
        }

        //Resolution
        public int ResolutionWidth { get; set; } = 640;
        public int ResolutionHeight { get; set; } = 640;

#if DESKTOP
        public ResizeModes ResizeMode { get; set; } = ResizeModes.Window;
        public bool Resizable { get; set; } = true;
#endif

        //Audio
        public float GeneralVolume { get; set; } = 1f;
        public float MusicVolume { get; set; } = 1f;
        public float EffectsVolume { get; set; } = 1f;
        public float AtmosphereVolume { get; set; } = 1f;

        //Controls
        public ControlSettings Controls { get; set; } = new();
        public class ControlSettings
        {
#if DESKTOP
            //Keyboard
            public Keys UpKey { get; set; } = Keys.Up;
            public Keys DownKey { get; set; } = Keys.Down;
            public Keys RightKey { get; set; } = Keys.Right;
            public Keys LeftKey { get; set; } = Keys.Left;
            public Keys ActionKey { get; set; } = Keys.Space;
            public Keys PauseKey { get; set; } = Keys.Escape;

            public Keys? UpKey2 { get; set; } = Keys.W;
            public Keys? DownKey2 { get; set; } = Keys.S;
            public Keys? RightKey2 { get; set; } = Keys.D;
            public Keys? LeftKey2 { get; set; } = Keys.A;
            public Keys? ActionKey2 { get; set; } = Keys.Enter;
            public Keys? PauseKey2 { get; set; } = Keys.Back;

            private bool UseGamePad { get; set; } = true;
#endif

#if DESKTOP || CONSOLE
            //Gamepad
            public Buttons UpButton { get; set; } = Buttons.DPadUp;
            public Buttons DownButton { get; set; } = Buttons.DPadDown;
            public Buttons RightButton { get; set; } = Buttons.DPadRight;
            public Buttons LeftButton { get; set; } = Buttons.DPadLeft;
            public Buttons ActionButton { get; set; } = Buttons.X;
            public Buttons PauseButton { get; set; } = Buttons.B;

            public Buttons? UpButton2 { get; set; } = Buttons.LeftThumbstickUp;
            public Buttons? DownButton2 { get; set; } = Buttons.LeftThumbstickDown;
            public Buttons? RightButton2 { get; set; } = Buttons.LeftThumbstickRight;
            public Buttons? LeftButton2 { get; set; } = Buttons.LeftThumbstickLeft;
            public Buttons? ActionButton2 { get; set; } = Buttons.A;
            public Buttons? PauseButton2 { get; set; } = Buttons.Y;
            public bool VibrationEnable { get; set; } = true;
#endif
        }

        //Graphics
        public bool VSync { get; set; } = true;
        public bool HardwareMode { get; set; } = true;

        //Language
        public LanguageType Language { get; set; } = LanguageType.English;


        private void LoadDefaults()
        {
            //todo
        }
        public override string ToString()
        {
            StringBuilder sb = new();
#if DESKTOP
            sb.AppendLine($"Resolution: {ResolutionWidth} x {ResolutionHeight} px - Resize Mode: {ResizeMode}");
#else
            sb.AppendLine($"Resolution: {ResolutionWidth} x {ResolutionHeight} px");
#endif
            sb.AppendLine($"Volume:  " +
                $"General: {Convert.ToInt32(GeneralVolume) * 100}  " +
                $"Music: {Convert.ToInt32(MusicVolume) * 100}  " +
                $"Effects: {Convert.ToInt32(EffectsVolume) * 100}  " +
                $"Atmosphere: {Convert.ToInt32(AtmosphereVolume) * 100}");

#if DESKTOP
            sb.AppendLine($"Primary Keyboard:  " +
                $"Up: {Controls.UpKey}  " +
                $"Down: {Controls.DownKey}  " +
                $"Left: {Controls.LeftKey}  " +
                $"Right: {Controls.RightKey}  " +
                $"Action: {Controls.ActionKey}  " +
                $"Pause: {Controls.PauseKey}");

            sb.AppendLine($"Secundary Keyboard:  " +
                $"Up: {(Controls.UpKey2 != null ? Controls.UpKey2.ToString() : "null")}  " +
                $"Down: {(Controls.DownKey2 != null ? Controls.DownKey2.ToString() : "null")}  " +
                $"Left: {(Controls.LeftKey2 != null ? Controls.LeftKey2.ToString() : "null")}  " +
                $"Right: {(Controls.RightKey2 != null ? Controls.RightKey2.ToString() : "null")}  " +
                $"Action: {(Controls.ActionKey2 != null ? Controls.ActionKey2.ToString() : "null")}  " +
                $"Pause: {(Controls.PauseKey2 != null ? Controls.PauseKey2.ToString() : "null")}");

            sb.AppendLine($"Gamepad vibration: {Controls.VibrationEnable}");
#endif

#if DESKTOP || CONSOLE
            sb.AppendLine($"Primary Gamepad buttons:  " +
                $"Up: {Controls.UpButton}" +
                $"Down: {Controls.DownButton}" +
                $"Left: {Controls.LeftButton}" +
                $"Right: {Controls.RightButton}" +
                $"Action: {Controls.ActionButton}" +
                $"Pause: {Controls.PauseButton}");

            sb.AppendLine($"Secundary Gamepad buttons:  " +
                $"Up: {(Controls.UpButton2 != null ? Controls.UpButton2.ToString() : "null")}  " +
                $"Down: {(Controls.DownButton2 != null ? Controls.DownButton2.ToString() : "null")}  " +
                $"Left: {(Controls.LeftButton2 != null ? Controls.LeftButton2.ToString() : "null")}  " +
                $"Right: {(Controls.RightButton2 != null ? Controls.RightButton2.ToString() : "null")}  " +
                $"Action: {(Controls.ActionButton2 != null ? Controls.ActionButton2.ToString() : "null")}  " +
                $"Pause: {(Controls.PauseButton2 != null ? Controls.PauseButton2.ToString() : "null")}");
#endif

            sb.AppendLine($"Vsync: {VSync} - Hardware Mode: {HardwareMode}");
            sb.Append($"Language: {Language}");

            return sb.ToString();
        }


        public static void Save(Settings settings)
        {
            Logger.Advice("Saving settings...");
            try
            {
                if (Directory.Exists(Directories.BaseDataDirectory))
                    Directory.CreateDirectory(Directories.BaseDataDirectory);

                XmlSerializer serial = new(typeof(Settings));
                using FileStream fs = new(Directories.SettingsFile, FileMode.Create);
                serial.Serialize(fs, settings);

                Logger.Advice("Settings Saved!", settings.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public static Settings Load()
        {
            Logger.Advice("Loading settings...");
            Settings settings = null;
            try
            {
                //try load settings
                if (File.Exists(Directories.SettingsFile))
                {
                    XmlSerializer serial = new(typeof(Settings));
                    using FileStream fs = new(Directories.SettingsFile, FileMode.Open);
                    settings = (Settings)serial.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


            if (settings == null)
            {
                //Create settings file if needs it
                Logger.Advice("Regeneration Settings file...");
                settings = new();
                Save(settings);
            }

            Logger.Advice("Settings Loaded!", settings.ToString());
            return settings;
        }


#if DESKTOP
        public void SetResolution(Resolutions resolution)
        {
            ResolutionInfo ri = new(resolution);
            ResolutionWidth = ri.Width;
            ResolutionHeight = ri.Height;
            Core.SetResolution(ResolutionWidth, ResolutionHeight, ResizeMode, Resizable);
            Core.GraphicsDeviceManager.ApplyChanges();
        }
        public void SetResizableMode(ResizeModes resizeMode)
        {
            ResizeMode = resizeMode;
            Core.SetResolution(ResolutionWidth, ResolutionHeight, ResizeMode, Resizable);
            Core.GraphicsDeviceManager.ApplyChanges();
        }
        public void SetResizableScreen(bool resizable)
        {
            Resizable = resizable;
            Core.Window.AllowUserResizing = resizable;
        }
#endif
        public void SetVsync(bool vsync)
        {
            VSync = vsync;
            Core.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = VSync;
        }
        public void SetHardwareMode(bool mode)
        {
            HardwareMode = mode;
            Core.GraphicsDeviceManager.HardwareModeSwitch = mode;
        }

#if DESKTOP
        public void SetPrimaryKeys(ControlSettings controls)
        {
            Core.Controls.Keyboard.SetPrimaryKeys(controls.UpKey, controls.DownKey, controls.LeftKey, controls.RightKey, controls.ActionKey, controls.PauseKey);
        }
        public void SetSecundaryKeys(ControlSettings controls)
        {
            Core.Controls.Keyboard.SetSecondaryKeys(controls.UpKey2, controls.DownKey2, controls.LeftKey2, controls.RightKey2, controls.ActionKey2, controls.PauseKey2);
        }
#endif
#if DESKTOP || CONSOLE
        public void SetPrimaryButtons(ControlSettings controls)
        {
            Core.Controls.GamePad.SetSecondaryButtons(controls.UpButton, controls.DownButton, controls.LeftButton, controls.RightButton, controls.ActionButton, controls.PauseButton);
        }
        public void SetSecundaryButtons(ControlSettings controls)
        {
            Core.Controls.GamePad.SetSecondaryButtons(controls.UpButton2, controls.DownButton2, controls.LeftButton2, controls.RightButton2, controls.ActionButton2, controls.PauseButton2);
        }
#endif

        public void ChangeLanguage(LanguageType language)
        {
            if (Language == language) return;
            Language = language;
            Core.ResetLanguage();
            Save(this);
        }
    }
}
