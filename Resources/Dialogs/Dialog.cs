using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Dialogs
{
    public struct Dialog
    {
        public string Text;

        public Characters Character;
        public string Face;
        public Locations Location;
        public SpeakType Speak;
        public float Speed;

        public int Color; //256 256 256
        public float Scale;
        public bool WaitInputAtEnd;
        public bool AjustEndLine;
        public bool CanAccelerate;
        public bool StopActions;

        public Dialog()
        {
            Text = "Default Text";
            Character = Characters.None;
            Face = string.Empty;
            Location = Locations.Bottom;
            Speak = SpeakType.None;
            Speed = 100f;

            Color = 0;
            Scale = 2f;
            WaitInputAtEnd = true;
            AjustEndLine = true;
            CanAccelerate = true;
            StopActions = true;
        }
    }
}
