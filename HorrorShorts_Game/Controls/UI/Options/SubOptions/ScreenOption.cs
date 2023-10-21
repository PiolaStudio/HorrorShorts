using HorrorShorts_Game.Controls.Camera;
using HorrorShorts_Game.Resources;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.UI.Options.SubOptions
{
    internal class ScreenOption : OptionSubMenu
    {
#if DESKTOP
        private Label _screenLBL;
        private ComboBox _screenCMB;

        private Label _resolutionLBL;
        private ComboBox _resolutionCMB;

        private Label _resizeLBL;
        private CheckBox _resizeCBX;
#endif

        private Label _vsyncLBL;
        private CheckBox _vsyncCBX;

        private Label _hardwareModeLBL;
        private CheckBox _hardwareModeCBX;

        public ScreenOption()
        {
            Height = 128;
        }
        public override void LoadContent()
        {
#if DESKTOP
            //Screen
            _screenLBL = new();
            _screenLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_screenLBL);

            _screenCMB = new();
            _screenCMB.Options = new string[3]
            {
                Localizations.Global.Options_Screen_FullScreen,
                Localizations.Global.Options_Screen_Borderless,
                Localizations.Global.Options_Screen_Window
            };
            _screenCMB.ClickEvent += (s, e) => NeedRender = true;
            _screenCMB.OptionClickEvent += (s, e) =>
            {
                Core.Settings.SetResizableMode((Settings.ResizeModes)e);
                NeedRender = true;
            };
            _screenCMB.Size = 4;
            _screenCMB.OptionSize = 4;
            _screenCMB.OptionAlignament = HorizontalAlignament.Right;
            _screenCMB.UserVirtualZone = true;
            _screenCMB.LoadContent();
            _screenCMB.SelectOption((int)Core.Settings.ResizeMode);
            _controls.Add(_screenCMB);

            //Resolution
            _resolutionLBL = new();
            _resolutionLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_resolutionLBL);

            string[] resolutionNames = Enum.GetNames<Resolutions>();
            for (int i = 0; i < resolutionNames.Length; i++)
                resolutionNames[i] = resolutionNames[i][1..];

            _resolutionCMB = new();
            _resolutionCMB.Options = resolutionNames;
            _resolutionCMB.ClickEvent += (s, e) => NeedRender = true;
            _resolutionCMB.OptionClickEvent += (s, e) =>
            {
                if (e >= 0)
                {
                    Resolutions r = Enum.GetValues<Resolutions>()[e];
                    Core.Settings.SetResolution(r);
                    NeedRender = true;
                }
            };
            _resolutionCMB.Size = 4;
            _resolutionCMB.OptionSize = 4;
            _resolutionCMB.MaxItemsWithoutScroll = 4;
            _resolutionCMB.OptionAlignament = HorizontalAlignament.Right;
            _resolutionCMB.UserVirtualZone = true;
            _resolutionCMB.LoadContent();
            _resolutionCMB.SelectOption(0); //todo
            _controls.Add(_resolutionCMB);

            //Resize
            _resizeLBL = new();
            _resizeLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_resizeLBL);

            _resizeCBX = new();
            _resizeCBX.UserVirtualZone = true;
            _resizeCBX.Checked = Core.Settings.Resizable;
            _resizeCBX.ClickEvent += (s, e) =>
            {
                Core.Settings.SetResizableScreen(e);
                NeedRender = true;
            };
            _resizeCBX.LoadContent();
            _controls.Add(_resizeCBX);
#endif

            //Vsync
            _vsyncLBL = new();
            _vsyncLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_vsyncLBL);

            _vsyncCBX = new();
            _vsyncCBX.UserVirtualZone = true;
            _vsyncCBX.Checked = Core.Settings.VSync;
            _vsyncCBX.ClickEvent += (s, e) =>
            {
                Core.Settings.SetVsync(e);
                NeedRender = true;
            };
            _vsyncCBX.LoadContent();
            _controls.Add(_vsyncCBX);

            //Hardware Mode
            _hardwareModeLBL = new();
            _hardwareModeLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_hardwareModeLBL);

            _hardwareModeCBX = new();
            _hardwareModeCBX.UserVirtualZone = true;
            _hardwareModeCBX.Checked = Core.Settings.HardwareMode;
            _hardwareModeCBX.ClickEvent += (s, e) =>
            {
                Core.Settings.HardwareMode = e;
                NeedRender = true;
            };
            _hardwareModeCBX.LoadContent();
            _controls.Add(_hardwareModeCBX);

            base.LoadContent();
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

#if DESKTOP
            //Screen
            _screenLBL.Position         = new(LeftPad, currentY);
            _screenCMB.Position         = new(RightPad - _screenCMB.Width, currentY - 8);
            currentY += RowPad;

            //Resolution
            _resolutionLBL.Position     = new(LeftPad, currentY);
            _resolutionCMB.Position         = new(RightPad - _resolutionCMB.Width, currentY - 8);
            currentY += RowPad;

            //Resize
            _resizeLBL.Position         = new(LeftPad, currentY);
            _resizeCBX.Position         = new(RightPad - 16, currentY - 8);
            currentY += RowPad;
#endif

            //Vsync
            _vsyncLBL.Position          = new(LeftPad, currentY);
            _vsyncCBX.Position          = new(RightPad - 16, currentY - 8);
            currentY += RowPad;

            //Hardware Mode
            _hardwareModeLBL.Position   = new(LeftPad, currentY);
            _hardwareModeCBX.Position   = new(RightPad - 16, currentY - 8);

            ComputeVirtualPositions();
        }
        public override void ComputeVirtualPositions()
        {
#if DESKTOP
            _screenCMB.VirtualPosition = _screenCMB.Position + PanelPosition - _scrollPoint;
            _resolutionCMB.VirtualPosition = _resolutionCMB.Position + PanelPosition - _scrollPoint;
            _resizeCBX.VirtualPosition = _resizeCBX.Position + PanelPosition - _scrollPoint;
#endif
            _vsyncCBX.VirtualPosition = _vsyncCBX.Position + PanelPosition - _scrollPoint;
            _hardwareModeCBX.VirtualPosition = _hardwareModeCBX.Position + PanelPosition - _scrollPoint;
        }
        protected override void UpdateStates()
        {
#if DESKTOP
            _screenCMB.IsEnable = !_resolutionCMB.IsDroped;
            _resolutionCMB.IsEnable = !_screenCMB.IsDroped && Core.Settings.ResizeMode == Settings.ResizeModes.Window;
            _resizeCBX.IsEnable = !_screenCMB.IsDroped && !_resolutionCMB.IsDroped && Core.Settings.ResizeMode == Settings.ResizeModes.Window;
            _vsyncCBX.IsEnable = !_screenCMB.IsDroped && !_resolutionCMB.IsDroped;
            _hardwareModeCBX.IsEnable = !_screenCMB.IsDroped && !_resolutionCMB.IsDroped;
#else
            _vsyncCBX.IsEnable = true;
            _hardwareModeCBX.IsEnable = true;
#endif
        }

        public override void SetLocalization()
        {
#if DESKTOP
            _screenLBL.Text = Localizations.Global.Options_Screen_Screen.ToUpper();
            _resolutionLBL.Text = Localizations.Global.Options_Screen_Resolution.ToUpper();
            _resizeLBL.Text = Localizations.Global.Options_Screen_Resizable.ToUpper();
#endif
            _vsyncLBL.Text = Localizations.Global.Options_Screen_Vsync.ToUpper();
            _hardwareModeLBL.Text = Localizations.Global.Options_Screen_HardwareMode.ToUpper();

            base.SetLocalization();
        }
    }
}
