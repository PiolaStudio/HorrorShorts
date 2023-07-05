using System;

namespace HorrorShorts_Game.Controls.UI.Dialogs
{
    public class DialogManagement
    {
        private readonly DialogBox _dialogBox;

        public EventHandler<int> DialogEvent;
        public EventHandler<int> ResponseEvent;

        private Dialog[] _dialogs;
        private int currentDialog = 0;
        private bool _finished = true;

        public DialogManagement()
        {
            _dialogBox = new DialogBox();
            _dialogBox.DialogEvent += DialogEvent;
        }
        public void LoadContent()
        {
            _dialogBox.LoadContent();
        }
        public void Update()
        {
            if (_finished) return;

            _dialogBox.Update();
            if (_dialogBox.Closed)
            {
                currentDialog++;
                if (currentDialog < _dialogs.Length)
                    _dialogBox.Show(_dialogs[currentDialog]);
                else _finished = true;
            }
        }
        public void PreDraw()
        {
            if (_finished) return;
            _dialogBox.PreDraw();
        }
        public void Draw()
        {
            if (_finished) return;
            _dialogBox.Draw();
        }
        public void Dispose()
        {
            _dialogBox.Dispose();
        }

        public void Start(Dialog[] dialogs)
        {
            currentDialog = 0;
            _dialogs = dialogs;
            _dialogBox.Show(_dialogs[currentDialog]);
            //if (!_dialogBox.Closed) 
            //dialogBox.Close(); //todo
            _finished = false;
        }
        public void ResetResolution()
        {
            _dialogBox.ResetResolution();
        }
    }
}
