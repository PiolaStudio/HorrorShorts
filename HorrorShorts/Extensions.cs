using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts
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
    }
}
