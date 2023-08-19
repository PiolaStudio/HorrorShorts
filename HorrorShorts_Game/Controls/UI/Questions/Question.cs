using Microsoft.Xna.Framework;
using Resources;
using Resources.Localizations;
using System;
using System.Diagnostics;

namespace HorrorShorts_Game.Controls.UI.Questions
{
    [DebuggerDisplay("{DebugDisplay}")]
    public struct Question
    {
        public string[] Options;
        public sbyte DefaultOption;
        public HorizontalAlignament TextAlign;
        public QuestionBoxLocation Location;
        public FontType Font;
        public Color SelectedTextColor;
        public Color OverTextColor;
        public Color UnSelectedTextColor;

        private string DebugDisplay
        {
            get
            {
                string display = string.Empty;
                for (int i = 0; i < Options.Length; i++)
                    display += $"{i + 1}. {Options[i]}";

                return display;
            }
        }

        public Question()
        {
            Options = new string[2]
            {
                "Option 1",
                "Option 2"
            };

            DefaultOption = -1;
            TextAlign = HorizontalAlignament.Center;
            Font = FontType.Arial;
            SelectedTextColor = Color.White;
            OverTextColor = Color.DarkGray;
            UnSelectedTextColor = Color.Gray;
            Location = QuestionBoxLocation.MiddleCenter;
        }
        public Question(Question_Serial serial)
        {
            Options = serial.Options;
            DefaultOption = serial.DefaultOption ?? -1;
            TextAlign = serial.TextAlign ?? HorizontalAlignament.Center;
            Font = serial.Font ?? FontType.Arial;
            SelectedTextColor = serial.SelectedTextColor ?? Color.White;
            OverTextColor = serial.OverTextColor ?? Color.White;
            UnSelectedTextColor = serial.UnselectedTextColor ?? Color.Gray;
            Location = serial.Location ?? QuestionBoxLocation.MiddleCenter;
        }
        public Question(Question_Serial serial, DialogBoxLocation dialogLocation) : this(serial)
        {
            Location = dialogLocation switch
            {
                DialogBoxLocation.TopLeft => QuestionBoxLocation.DialogTopLeft,
                DialogBoxLocation.TopRight => QuestionBoxLocation.DialogTopRight,
                DialogBoxLocation.MiddleLeft => QuestionBoxLocation.DialogMiddleLeft,
                DialogBoxLocation.MiddleRight => QuestionBoxLocation.DialogMiddleRight,
                DialogBoxLocation.BottomLeft => QuestionBoxLocation.DialogBottomLeft,
                DialogBoxLocation.BottomRight => QuestionBoxLocation.DialogBottomRight,
                _ => throw new NotImplementedException("Not implemented position for " + dialogLocation)
            };
        }
    }
}
