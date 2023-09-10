using Microsoft.Xna.Framework;
using System;

namespace HorrorShorts_Game.Algorithms.Tweener
{
    public static class TweenConverters
    {
        public static T FloatConverter<T>(float value, T start, T end)
        {
            float startP = (float)Convert.ChangeType(start, typeof(float));
            float endP = (float)Convert.ChangeType(end, typeof(float));
            float result = startP + (endP - startP) * value;

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T IntConverter<T>(float value, T start, T end)
        {
            int startP = (int)Convert.ChangeType(start, typeof(int));
            int endP = (int)Convert.ChangeType(end, typeof(int));
            int result = Convert.ToInt32(startP + (endP - startP) * value);

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T ShortConverter<T>(float value, T start, T end)
        {
            int startP = (int)Convert.ChangeType(start, typeof(int));
            int endP = (int)Convert.ChangeType(end, typeof(int));
            short result = Convert.ToInt16(startP + (endP - startP) * value);

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T ByteConverter<T>(float value, T start, T end)
        {
            int startP = (int)Convert.ChangeType(start, typeof(int));
            int endP = (int)Convert.ChangeType(end, typeof(int));
            byte result = Convert.ToByte(startP + (endP - startP) * value);
                
            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T PointConverter<T>(float value, T start, T end)
        {
            Point startP = (Point)Convert.ChangeType(start, typeof(Point));
            Point endP = (Point)Convert.ChangeType(end, typeof(Point));
            Point result = new(startP.X + Convert.ToInt32((endP.X - startP.X) * value),
                               startP.Y + Convert.ToInt32((endP.Y - startP.Y) * value));

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T Vector2Converter<T>(float value, T start, T end)
        {
            Vector2 startP = (Vector2)Convert.ChangeType(start, typeof(Vector2));
            Vector2 endP = (Vector2)Convert.ChangeType(end, typeof(Vector2));
            Vector2 result = new(startP.X + (endP.X - startP.X) * value,
                                 startP.Y + (endP.Y - startP.Y) * value);

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static T ColorConverter<T>(float value, T start, T end)
        {
            Color startP = (Color)Convert.ChangeType(start, typeof(Color));
            Color endP = (Color)Convert.ChangeType(end, typeof(Color));
            Color result = Color.Lerp(startP, endP, value);

            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
