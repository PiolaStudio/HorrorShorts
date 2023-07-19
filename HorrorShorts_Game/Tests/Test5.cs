#if DEBUG
#define TEST_TYPE_2
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;

namespace HorrorShorts_Game.Tests
{
    public class Test5 : TestBase
    {
        private Keys _prevKey = Keys.F1;
        private TimeSpan ts;

        public override void LoadContent1()
        {
            Songs.ReLoad(new SongType[2] { SongType.Test1, SongType.Test2 });

#if TEST_TYPE_1
            Core.SongManager.Play(SongType.Test1, 1000);
#elif TEST_TYPE_2
            Core.SongManager.LoadForParallel(new SongType[2] { SongType.Test1, SongType.Test2 });
            Core.SongManager.PlayFromParallel(SongType.Test1, 1000);
#endif
        }
        public override void Update1()
        {
            ts += Core.GameTime.ElapsedGameTime;
            KeyboardState ks = Core.KeyState;

#if TEST_TYPE_1
            if (ks.IsKeyDown(Keys.F1) && _prevKey != Keys.F1)
            {
                _prevKey = Keys.F1;
                if (!Core.SongManager.IsPlaying || Core.SongManager.Song.Value != SongType.Test1)
                    Core.SongManager.Play(SongType.Test1, 1000, 1000);
            }
            if (ks.IsKeyDown(Keys.F2) && _prevKey != Keys.F2)
            {
                _prevKey = Keys.F2;
                if (!Core.SongManager.IsPlaying || Core.SongManager.Song.Value != SongType.Test2)
                    Core.SongManager.Play(SongType.Test2, 1000, 1000);
            }
            if (ks.IsKeyDown(Keys.F3) && _prevKey != Keys.F3)
            {
                _prevKey = Keys.F3;
                if (Core.SongManager.Song.HasValue)
                    Core.SongManager.Stop(1000);
            }
#elif TEST_TYPE_2

            if (ts.TotalSeconds > 5)
            {
                ts -= TimeSpan.FromSeconds(5);

                if (!Core.SongManager.IsPlaying)
                    Core.SongManager.PlayFromParallel(SongType.Test1, 500, 1000);
                else if (Core.SongManager.Song == SongType.Test1)
                    Core.SongManager.PlayFromParallel(SongType.Test2, 500, 1000);
                else
                    Core.SongManager.Stop(1000);
            }

            //if (ks.IsKeyDown(Keys.F1) && _prevKey != Keys.F1)
            //{
            //    _prevKey = Keys.F1;
            //    if (!Core.SongManager.IsPlaying || Core.SongManager.Song.Value != SongType.Test1)
            //        Core.SongManager.PlayFromParallel(SongType.Test1, 500, 1000);
            //}
            //if (ks.IsKeyDown(Keys.F2) && _prevKey != Keys.F2)
            //{
            //    _prevKey = Keys.F2;
            //    if (!Core.SongManager.IsPlaying || Core.SongManager.Song.Value != SongType.Test2)
            //        Core.SongManager.PlayFromParallel(SongType.Test2, 500, 1000);
            //}
            //if (ks.IsKeyDown(Keys.F3) && _prevKey != Keys.F3)
            //{
            //    _prevKey = Keys.F3;
            //    if (Core.SongManager.IsPlaying)
            //        Core.SongManager.Stop(1000);
            //}
#endif
        }
        public override void Draw1()
        {

        }
    }
}
#endif