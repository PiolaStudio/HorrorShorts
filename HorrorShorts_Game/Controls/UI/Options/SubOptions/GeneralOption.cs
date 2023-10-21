using HorrorShorts_Game.Controls.UI.Language;
using HorrorShorts_Game.Resources;
using Resources;

namespace HorrorShorts_Game.Controls.UI.Options.SubOptions
{
    internal class GeneralOption : OptionSubMenu
    {
        private Label _languageLBL;
        private Button _languageBTN;
        private LanguageMenu _languageMenu;

        public GeneralOption()
        {
            Height = 64;
        }
        public override void LoadContent()
        {
            //Language
            _languageLBL = new();
            _languageLBL.Alignament = TextAlignament.MiddleLeft;
            _languageLBL.LoadContent();
            _controls.Add(_languageLBL);

            _languageBTN = new();
            _languageBTN.Text = Core.Settings.Language.GetNativeName();
            _languageBTN.Size = 4;
            _languageBTN.UserVirtualZone = true;
            _languageBTN.Click += (s, e) => _languageMenu.Show();
            _languageBTN.LoadContent();
            _controls.Add(_languageBTN);

            _languageMenu = new();
            _languageMenu.LoadContent();

            base.LoadContent();
        }
        public override void Update()
        {
            base.Update();
            _languageMenu.Update();
        }
        public override void PreDraw()
        {
            base.PreDraw();
            _languageMenu.PreDraw();
        }
        public override void Draw()
        {
            base.Draw();
        }
        public override void DrawOver()
        {
            _languageMenu.Draw();
        }
        public override void Dispose()
        {
            base.Dispose();
            _languageMenu.Dispose();
        }

        public override void ComputePositions()
        {
            int currentY = InitY;

            //General
            _languageLBL.Position = new(LeftPad, currentY);
            _languageBTN.Position = new(RightPad - 96, currentY - 8);

            ComputeVirtualPositions();
        }
        public override void ComputeVirtualPositions()
        {
            _languageBTN.VirtualPosition = _languageBTN.Position + PanelPosition - _scrollPoint;
        }
        protected override void UpdateStates()
        {
            _languageBTN.IsEnable = !_languageMenu.IsVisible;
        }
        public override void SetLocalization()
        {
            _languageLBL.Text = Localizations.Global.Options_General_Language;
            _languageBTN.Text = Core.Settings.Language.GetNativeName();

            base.SetLocalization();
        }
    }
}
