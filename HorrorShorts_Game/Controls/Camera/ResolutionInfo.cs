using Resources;
using System;

namespace HorrorShorts_Game.Controls.Camera
{
    public struct ResolutionInfo
    {
        public Resolutions? Type { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float AspectRatio { get; private set; }

        public ResolutionInfo(Resolutions type)
        {
            Type = type;

            GetSize(type, out int width, out int height);
            Width = width;
            Height = height;
            AspectRatio = (float)Width  / Height;
        }
        public ResolutionInfo(int width, int height)
        {
            Type = GetType(width, height);
            Width = width;
            Height = height;
            AspectRatio = (float)Width / Height;
        }

        private static void GetSize(Resolutions type, out int width, out int height)
        {
            string typeStr = type.ToString();
            width = Convert.ToInt32(typeStr.Substring(1, typeStr.IndexOf('x') - 1));
            height = Convert.ToInt32(typeStr.Substring(typeStr.IndexOf('x') + 1));
        }
        private static Resolutions? GetType(int width, int height)
        {
            Resolutions[] resolutions = Enum.GetValues<Resolutions>();
            for (int i = 0; i <  resolutions.Length; i++)
            {
                GetSize(resolutions[i], out int w, out int h);
                if (w == width && h == height) 
                    return resolutions[i];
            }

            return null;
        }
    }
}
