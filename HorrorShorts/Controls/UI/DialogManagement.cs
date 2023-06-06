using System;

namespace HorrorShorts.Controls.UI
{
    public class DialogManagement
    {
        private DialogBox _dialog;
        public EventHandler<int> DialogEvent;

        private DialogBox[] _dialogs;
        private int currentDialog = 0;

        public DialogManagement()
        {
            _dialog = new DialogBox();
            _dialog.DialogEvent += DialogEvent;
        }
        public void LoadContent()
        {
            _dialog.LoadContent();
        }
        public void Update()
        {
            _dialog.Update();
        }
        public void PreDraw()
        {
            _dialog.PreDraw();
        }
        public void Draw()
        {
            _dialog.Draw();
        }

        public void Start(DialogBox[] dialogs)
        {
            currentDialog = 0;
            _dialogs = dialogs;
        }
    }
}
