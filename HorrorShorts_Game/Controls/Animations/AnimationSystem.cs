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
        private AnimationData? _animationData = null;
        private AnimationData Animation { get => _animationData.Value; }

        public string Name
        {
            get
            {
                if (_animationData == null) return null;
                return Animation.Name;
            }
        }

        private AnimationState _state;
        public AnimationState State { get => _state; }

        private float _frameElapsed = 0f;
        public float FrameElapsed { get => _frameElapsed; }
        private AnimationFrame CurrentFrame { get => Animation.Frames[_frameIndex]; }

        public int TotalFrames { get => Animation.Frames.Length; }

        private int _frameIndex = 0;
        public int FrameIndex { get => _frameIndex; }

        private float _speedMod = 1f;
        public float SpeedMod { get => _speedMod; set => _speedMod = value; }

        public bool Bucle
        {
            get => _bucleType != BucleType.None;
            set
            {
                if (value)
                {
                    _bucleType = BucleType.Forward;
                    _updateAction = UpdateForwardBucle;
                }
                else
                {
                    _bucleType = BucleType.None;
                    _updateAction = UpdateNoBucle;
                }
            }
        }
        public BucleType BucleType 
        { 
            get => _bucleType;
            set
            {
                _bucleType = value;
                _updateAction = _bucleType switch
                {
                    BucleType.None => UpdateNoBucle,
                    BucleType.Forward => UpdateForwardBucle,
                    BucleType.Reverse => UpdateReverseBucle,
                    BucleType.PingPong => UpdatePingPongBucle,
                    _ => throw new NotImplementedException("Not implemented bucle type for " + _bucleType)
                };
            }
        }
        private BucleType _bucleType = BucleType.None;
        private bool _pingPongDirection = false;

        public Rectangle Source { get => CurrentFrame.Source; }

        private bool frameChanged = false;
        public bool FrameChanged { get => frameChanged; }

        private Action _updateAction;

        public AnimationSystem()
        {
            _updateAction = UpdateNoBucle;
        }
        public void Update()
        {
            frameChanged = false;
            if (_state != AnimationState.Playing) return;

            if (_frameElapsed >= CurrentFrame.Duration)
                _updateAction.Invoke();

            _frameElapsed += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds * _speedMod;
        }
        private void UpdateNoBucle()
        {
            if (_frameIndex < TotalFrames - 1)
            {
                //Next frame
                _frameElapsed -= CurrentFrame.Duration;
                _frameIndex++;
                frameChanged = true;
            }
            else
            {
                //Finish animation
                _frameElapsed = 0f;
                _frameIndex = 0;
                _state = AnimationState.Stopped;
            }
        }
        private void UpdateForwardBucle()
        {
            _frameElapsed -= CurrentFrame.Duration;
            if (_frameIndex < TotalFrames - 1)
            {
                //Next frame
                _frameIndex++;
                frameChanged = true;
            }
            else
            {
                //Restart animation
                _frameIndex = 0;
                frameChanged = true;
            }
        }
        private void UpdateReverseBucle()
        {
            _frameElapsed -= CurrentFrame.Duration;
            if (_frameIndex > 0)
            {
                //Prev frame
                _frameIndex--;
                frameChanged = true;
            }
            else
            {
                //Restart animation (from back)
                _frameIndex = TotalFrames - 1;
                frameChanged = true;
            }
        }
        private void UpdatePingPongBucle()
        {
            _frameElapsed -= CurrentFrame.Duration;

            if (_pingPongDirection)
            {
                if (_frameIndex < TotalFrames - 1)
                {
                    //Next frame
                    _frameIndex++;
                    frameChanged = true;
                }
                else
                {
                    //Change anim direction
                    _pingPongDirection = !_pingPongDirection;

                    _frameIndex--;
                    frameChanged = true;
                }
            }
            else
            {
                if (_frameIndex > 0)
                {
                    //Prev frame
                    _frameIndex--;
                    frameChanged = true;
                }
                else
                {
                    //Change anim direction
                    _pingPongDirection = !_pingPongDirection;

                    _frameIndex++;
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
            this._animationData = animation;
            frameChanged = true;
        }
        /// <summary>
        /// Change the animation and continue from the same frame
        /// </summary>
        /// <param name="animation"></param>
        public void SwapAnimation(AnimationData animation)
        {
            this._animationData = animation;
            frameChanged = true;
        }

        public void Play()
        {
            _frameIndex = 0;
            _frameElapsed = 0f;
            _state = AnimationState.Playing;
            frameChanged = true;
            _pingPongDirection = false;
        }
        public void Resume()
        {
            if (_state != AnimationState.Paused) return;
            _state = AnimationState.Playing;
        }
        public void Pause()
        {
            if (_state == AnimationState.Stopped) return;
            _state = AnimationState.Paused;
        }
        public void Stop()
        {
            if (_state == AnimationState.Stopped) return;
            _state = AnimationState.Stopped;
        }

        public void SetFrame(int index)
        {
            _frameIndex = index;
            _frameElapsed = 0f;
            frameChanged = true;
        }
        public AnimationFrame GetFrameData(int index) => Animation.Frames[index];
    }
}
