﻿#if DEBUG
using HorrorShorts_Game.Controls.Animations;
using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Input;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test2 : TestBase
    {
        private Dictionary<string, AnimationData> Anims { get => Animations.Megaman; }
        private Sprite _sprite;
        private Keys _prevKey = Keys.F1;
        private AnimationSystem animationSystem;

        public override void LoadContent1()
        {
            Textures.ReLoad(
                nameof(Textures.Mario),
                nameof(Textures.Megaman),
                nameof(Textures.Girl1));

            SpriteSheets.ReLoad(
                nameof(SpriteSheets.Mario),
                nameof(SpriteSheets.Megaman),
                nameof(SpriteSheets.Girl1));

            Animations.ReLoad(
                nameof(Animations.Megaman));

            //Animation_Serial anim = new();
            //anim.Animations = new SingleAnimation_Serial[2];

            //anim.Animations[0] = new();
            //anim.Animations[0].SpriteSheet = "Sheet Name";
            //anim.Animations[0].Name = "Animation 1";
            //anim.Animations[0].Frames = new AnimationFrame_Serial[2];
            //anim.Animations[0].Frames[0] = new();
            //anim.Animations[0].Frames[0].Sheet = "Run_1";
            //anim.Animations[0].Frames[0].Duration = 100;
            //anim.Animations[0].Frames[1] = new();
            //anim.Animations[0].Frames[1].Sheet = "Run_2";
            //anim.Animations[0].Frames[1].Duration = 100;

            //anim.Animations[1] = new();
            //anim.Animations[1].SpriteSheet = "Sheet Name";
            //anim.Animations[1].Name = "Animation 2";
            //anim.Animations[1].Frames = new AnimationFrame_Serial[2];
            //anim.Animations[1].Frames[0] = new();
            //anim.Animations[1].Frames[0].Sheet = "Run_1";
            //anim.Animations[1].Frames[0].Duration = 100;
            //anim.Animations[1].Frames[1] = new();
            //anim.Animations[1].Frames[1].Sheet = "Run_2";
            //anim.Animations[1].Frames[1].Duration = 100;

            //anim.Save("Anim.xml");

            animationSystem = new();
            animationSystem.SetAnimation(Anims["Idle"]);
            animationSystem.BucleType = BucleType.Forward;
            animationSystem.Play();

            _sprite = new Sprite(Textures.Megaman, y: Core.ResolutionBounds.Bottom - 32, source: animationSystem.Source);
        }
        public override void Update1()
        {
            KeyboardState ks = Core.KeyState;

            if (ks.IsKeyDown(Keys.F1) && _prevKey != Keys.F1)
            {
                _prevKey = Keys.F1;
                animationSystem.SetAnimation(Anims["Idle"]);
                animationSystem.BucleType = BucleType.Forward;
                animationSystem.Play();
                _sprite.Source = animationSystem.Source;
            }
            if (ks.IsKeyDown(Keys.F2) && _prevKey != Keys.F2)
            {
                _prevKey = Keys.F2;
                animationSystem.SetAnimation(Anims["Run"]);
                animationSystem.BucleType = BucleType.PingPong;
                animationSystem.Play();
                _sprite.Source = animationSystem.Source;
            }
            if (ks.IsKeyDown(Keys.F3) && _prevKey != Keys.F3)
            {
                _prevKey = Keys.F3;
                animationSystem.SetAnimation(Anims["Tornado"]);
                animationSystem.BucleType = BucleType.Forward;
                animationSystem.Play();
                _sprite.Source = animationSystem.Source;
            }
            if (ks.IsKeyDown(Keys.F4) && _prevKey != Keys.F4)
            {
                _prevKey = Keys.F4;
                animationSystem.SetAnimation(Anims["Climb"]);
                animationSystem.BucleType = BucleType.Forward;
                animationSystem.Play();
                _sprite.Source = animationSystem.Source;
            }

            animationSystem.Update();

            if (animationSystem.FrameChanged)
                _sprite.Source = animationSystem.Source;
        }
        public override void Draw1()
        {
            _sprite.Draw();
            base.Draw1();
        }
    }
}
#endif