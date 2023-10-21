using Microsoft.Xna.Framework;
using Resources;
using Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game
{
    public static class Extensions
    {
        //public static T AddFlag<T>(this Enum value, T flag) where T : System.Enum
        //{
        //    return (T)(object)((int)(object)value | (int)(object)flag);
        //}
        //public static T RemoveFlag<T>(this Enum value, T flag) where T : System.Enum
        //{
        //    return (T)(object) ((int)(object)value & ~(int)(object)flag);
        //}

        public static Point PointDistance(this Point point1, Point point2)
        {
            return new(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y));
        }
        public static bool CheckClicked(this Rectangle rectangle)
        {
#if DESKTOP || PHONE
            return Core.Controls.Click && rectangle.Contains(Core.Controls.ClickPosition);
#else
            return false;
#endif
        }
        public static bool CheckClickedUI(this Rectangle rectangle)
        {
#if DESKTOP || PHONE
            return Core.Controls.Click && rectangle.Contains(Core.Controls.ClickPositionUI);
#else
            return false;
#endif
        }


        public static bool NextBool(this Random random)
        {
            return random.Next(2) == 0;
        }
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public static Color HexToColor(this string hex)
        {
            byte R = Convert.ToByte(hex.Substring(0, 2), 16);
            byte G = Convert.ToByte(hex.Substring(2, 2), 16);
            byte B = Convert.ToByte(hex.Substring(4, 2), 16);

            byte A = 255;
            if (hex.Length > 6) 
                A = Convert.ToByte(hex.Substring(6, 2), 16);

            return new Color(R, G, B, A);
        }
        public static string ToHexShortCommand(this string value)
        {
            short shor = BitConverter.ToInt16(UTF8Encoding.UTF8.GetBytes(value));
            return "0x" + shor.ToString("X");
        }


        public static string GetNativeName(this LanguageType language)
        {
            Type languageType = typeof(LanguageType);
            var memberInfos = languageType.GetMember(language.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == languageType);
            var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(LanguageAttribute), false);
            LanguageAttribute la = ((LanguageAttribute)valueAttributes[0]);
            return la.NativeName;
        }
    }
}
