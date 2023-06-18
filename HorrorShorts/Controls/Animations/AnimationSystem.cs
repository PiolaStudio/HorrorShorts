using HorrorShorts.Controls.Sprites;
using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Controls.Animations
{
    public enum AnimationState : byte
    {
        Playing,
        Stopped,
        Paused
    }
    public enum BucleType : byte
    {
        None,
        Forward,
        Reverse,
        PingPong
    }
    
    [DebuggerDisplay("Name: {Name} | Frame: {FrameIndex} | State: {State}")]
    public class AnimationSystem
    {
        private AnimationData? animationData = null;
        private AnimationData Animation { get => animationData.Value; }

        public string Name
        {
            get
            {
                if (animationData == null) return null;
                return Animation.Name;
            }
        }

        private AnimationState state;
        public AnimationState State { get => state; }

        private float frameElapsed = 0f;
        public float FrameElapsed { get => frameElapsed; }
        private AnimationFrame CurrentFrame { get => Animation.Frames[frameIndex]; }

        public int TotalFrames { get => Animation.Frames.Length; }

        private int frameIndex = 0;
        public int FrameIndex { get => frameIndex; }

        private float speedMod = 1f;
        public float SpeedMod { get => speedMod; set => speedMod = value; }

        public bool Bucle
        {
            get => bucleType != BucleType.None;
            set
            {
                if (value) bucleType = BucleType.Forward;
                else bucleType = BucleType.None;
            }
        }
        public BucleType BucleType { get => bucleType; set => bucleType = value; }
        private BucleType bucleType = BucleType.None;
        private bool pingPongDirection = false;

        public Rectangle Source { get => CurrentFrame.Source; }

        private bool frameChanged = false;
        public bool FrameChanged { get => frameChanged; }

        public void Update()
        {
            frameChanged = false;
            if (state == AnimationState.Stopped) return;
            if (state == AnimationState.Paused) return;

            if (frameElapsed >= CurrentFrame.Duration)
            {
                switch (bucleType)
                {
                    case BucleType.None:
                        UpdateNoBucle();
                        break;
                    case BucleType.Forward:
                        UpdateForwardBucle();
                        break;
                    case BucleType.Reverse:
                        UpdateReverseBucle();
                        break;
                    case BucleType.PingPong:
                        UpdatePingPongBucle();
                        break;
                }
            }
            frameElapsed += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds * speedMod;
        }
        private void UpdateNoBucle()
        {
            if (frameIndex < TotalFrames - 1)
            {
                //Next frame
                frameElapsed -= CurrentFrame.Duration;
                frameIndex++;
                frameChanged = true;
            }
            else
            {
                //Finish animation
                frameElapsed = 0f;
                frameIndex = 0;
                state = AnimationState.Stopped;
            }
        }
        private void UpdateForwardBucle()
        {
            frameElapsed -= CurrentFrame.Duration;
            if (frameIndex < TotalFrames - 1)
            {
                //Next frame
                frameIndex++;
                frameChanged = true;
            }
            else
            {
                //Restart animation
                frameIndex = 0;
                frameChanged = true;
            }
        }
        private void UpdateReverseBucle()
        {
            frameElapsed -= CurrentFrame.Duration;
            if (frameIndex > 0)
            {
                //Prev frame
                frameIndex--;
                frameChanged = true;
            }
            else
            {
                //Restart animation (from back)
                frameIndex = TotalFrames - 1;
                frameChanged = true;
            }
        }
        private void UpdatePingPongBucle()
        {
            frameElapsed -= CurrentFrame.Duration;

            if (pingPongDirection)
            {
                if (frameIndex < TotalFrames - 1)
                {
                    //Next frame
                    frameIndex++;
                    frameChanged = true;
                }
                else
                {
                    //Change anim direction
                    pingPongDirection = !pingPongDirection;

                    frameIndex--;
                    frameChanged = true;
                }
            }
            else
            {
                if (frameIndex > 0)
                {
                    //Prev frame
                    frameIndex--;
                    frameChanged = true;
                }
                else
                {
                    //Change anim direction
                    pingPongDirection = !pingPongDirection;

                    frameIndex++;
                    frameChanged = true;
                }
            }
        }

        /// <summary>
        /// Set a new Animation
        /// </summary>
        /// <param name="animation"></param>
        public void SetAnimation(AnimationData animation)
        {
            Stop();
            this.animationData = animation;
            frameChanged = true;
        }
        /// <summary>
        /// Change the animation and continue from the same frame
        /// </summary>
        /// <param name="animation"></param>
        public void SwapAnimation(AnimationData animation)
        {
            this.animationData = animation;
            frameChanged = true;
        }

        public void Play()
        {
            frameIndex = 0;
            frameElapsed = 0f;
            state = AnimationState.Playing;
            frameChanged = true;
            pingPongDirection = false;
        }
        public void Resume()
        {
            if (state != AnimationState.Paused) return;
            state = AnimationState.Playing;
        }
        public void Pause()
        {
            if (state == AnimationState.Stopped) return;
            state = AnimationState.Paused;
        }
        public void Stop()
        {
            if (state == AnimationState.Stopped) return;
            state = AnimationState.Stopped;
        }

        public void SetFrame(int index)
        {
            frameIndex = index;
            frameElapsed = 0f;
            frameChanged = true;
        }
        public AnimationFrame GetFrameData(int index) => Animation.Frames[index];
    }
}
