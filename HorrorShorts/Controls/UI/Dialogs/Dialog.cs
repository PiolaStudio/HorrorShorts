using Resources.Dialogs;
using Resources;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System;

namespace HorrorShorts.Controls.UI.Dialogs
{
    [DebuggerDisplay("{Text}")]
    public struct Dialog
    {
        public string Text;

        public Characters Character;
        public FaceType Face;

        public TextBoxLocation Location;
        public TextAlignament TextAlign;
        public float Speed;

        //Speak
        public SpeakType Speak;
        public int SpeakSpeed;
        public int SpeakPitch;
        public int SpeakPitchVariation;


        public Color Color; //256 256 256
        public int FontSize;
        public FontType Font;

        //public bool AjustTop;
        public bool WaitInputAtEnd; //todo
        public bool AjustEndLine; //todo
        public bool CanAccelerate; //todo
        public bool StopActions; //todo
        public bool DoPauses; //todo

        public Dialog()
        {
            Text = "Default Text";
            Character = Characters.Narrator;
            Face = FaceType.None;
            TextAlign = TextAlignament.TopLeft;
            Location = TextBoxLocation.BottomLeft;
            Speak = SpeakType.None;
            Speed = 20;
            Font = FontType.Default;
            FontSize = 2;
            Color = Color.White;
            SpeakSpeed = 3;
            SpeakPitch = 0;
            SpeakPitchVariation = 0;

            WaitInputAtEnd = true;
            AjustEndLine = true;
            CanAccelerate = true;
            StopActions = true;
            DoPauses = true;
        }
        public Dialog(Dialog_Serial serial)
        {
            Text = serial.Text;
            Character = serial.Character ?? Characters.Narrator; //todo: change to -1 (default)
            Face = serial.Face ?? FaceType.None; //todo: change to -1 (default)
            Location = serial.Location ?? TextBoxLocation.BottomLeft;
            if (serial.TextAlign == null)
            {
                switch (Location)
                {
                    case TextBoxLocation.TopLeft:
                    case TextBoxLocation.MiddleLeft:
                    case TextBoxLocation.BottomLeft:
                        TextAlign = TextAlignament.MiddleLeft;
                        break;
                    case TextBoxLocation.TopRight:
                    case TextBoxLocation.MiddleRight:
                    case TextBoxLocation.BottomRight:
                        TextAlign = TextAlignament.MiddleRight;
                        break;
                    default:
                        throw new NotImplementedException("Not implemented Text Align for Location: " + Location);
                }
            }
            else TextAlign = serial.TextAlign.Value;

            Speed = serial.Speed ?? 20; //todo: change to -1 (default)
            Color = serial.FontColor != null ? new Color(serial.FontColor.Value, 1f) : Color.White;
            FontSize = serial.FontSize ?? 2; //todo: change to -1 (default)
            Font = serial.FontType ?? FontType.Default;

            Speak = serial.SpeakType ?? SpeakType.Default;
            SpeakSpeed = serial.SpeakSpeed ?? 3; //todo: change to -1 (default)
            SpeakPitch = serial.SpeakPitch ?? -1; //default
            SpeakPitchVariation = serial.SpeakPitchVariation ?? 0;

            //todo
            WaitInputAtEnd = true;
            AjustEndLine = true;
            CanAccelerate = true;
            StopActions = true;
            DoPauses = true;
        }
    }
}
