using HorrorShorts.Controls.Sprites;
using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Resources.Sprites;

namespace HorrorShorts.Controls.Animations
{
    public struct AnimationFrame
    {
        public Rectangle Source;
        public float Duration;

        public AnimationFrame()
        {
            Source = new Rectangle();
            Duration = 0f;
        }
        public AnimationFrame(Rectangle source, float speed)
        {
            Source = source;
            Duration = speed;
        }
        public AnimationFrame(int x, int y, int width, int height, float speed)
        {
            Source = new Rectangle(x, y, width, height);
            Duration = speed;
        }
    }
    public struct AnimationData
    {
        public string Name;
        public AnimationFrame[] Frames;

        public float GetTotalDuration()
        {
            float totalDuration = 0;
            for (int i = 0; i < Frames.Length; i++)
                totalDuration += Frames[i].Duration;
            return totalDuration;
        }

        public AnimationData()
        {
            Name = "Animation";//"Default" + GetHashCode();
            Frames = new AnimationFrame[0];
        }
        public AnimationData(string name, AnimationFrame[] frames)
        {
            Name = name;
            Frames = frames;
        }
        public AnimationData(SingleAnimation_Serial anim)
        {
            Name = anim.Name;

            Frames = new AnimationFrame[anim.Frames.Length];
            SpriteSheets.GetSheet(anim.SpriteSheet, out SpriteSheet ss);
            for (int i = 0; i < Frames.Length; i++)
                Frames[i] = new AnimationFrame(ss.Get(anim.Frames[i].Sheet), anim.Frames[i].Duration);
        }
    }
}
