using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Resources;
using System;

namespace HorrorShorts_Game.Audio.Atmosphere
{
    public class AtmosphereInstance
    {
        //Sound
        public SoundType SoundType { get => _soundType; }
        private readonly SoundType _soundType;
        private SoundEffectInstance _sound;

        //Loop
        public float LoopDelay
        {
            get => _loopDelay;
            set
            {
                if (_loopDelay == value) return;
                _loopDelay = value;
                if (SoundIsPlaying())
                    _sound.IsLooped = value == 0;
            }
        }
        private float _loopDelay = 0f;
        private float _currentDelay = 999999f;

        //Volume
        public float BaseVolume
        {
            get => _baseVolume;
            set
            {
                if (_baseVolume == value) return;
                _baseVolume = value;
                if (SoundIsPlaying())
                    _sound.Volume = GetVolume();
            }
        }
        private float _baseVolume = 1f;

        //Pitch
        private bool _usePitchRange = false;
        private float _pitchMin = -1f;
        private float _pitchMax = 1f;
        public void SetPitchRange(float min, float max)
        {
            _pitchMin = min;
            _pitchMax = max;

            _usePitchRange = min != max;
        }

        //Pan
        public bool PanAllowed
        {
            get => _panAllowed;
            set
            {
                if (_panAllowed == value) return;
                _panAllowed = value;
                if (SoundIsPlaying())
                    _sound.Pan = GetPan();
            }
        }
        private bool _panAllowed = true;

        //Emisor - Receptor
        public bool IsGlobalSound
        {
            get => _isGlobalSound;
            set
            {
                if (_isGlobalSound != value) return;
                _isGlobalSound = value;
            }
        }
        private bool _isGlobalSound = true;
        public int PlayRange
        {
            get => _playRange;
            set
            {
                if (_playRange == value) return;
                _playRange = value;
            }
        }
        private int _playRange = 20;

        public Point Origin
        {
            get => _origin;
            set => _origin = value;
        }
        private Point _origin;
        public Point Listener
        {
            get => _listener;
            set => _listener = value;
        }
        private Point _listener;

        public bool StopWhenNotUse
        {
            get => _stopWhenNotUse;
            set
            {
                if (_stopWhenNotUse == value) return;
                _stopWhenNotUse = value;
            }
        }
        private bool _stopWhenNotUse = false;

        public AtmosphereInstance(SoundType sound)
        {
            _soundType = sound;
        }
        public AtmosphereInstance(AtmosphereSound values)
        {
            _soundType = values.Sound;

            _loopDelay = values.LoopDelay;
            _baseVolume = values.BaseVolume;
            SetPitchRange(values.PitchMin, values.PitchMax);
            _panAllowed = values.PanAllowed;

            _isGlobalSound = values.IsGlobalSound;
            _playRange = values.PlayRange;
            _origin = values.Origin;
            _stopWhenNotUse = values.StopWhenNotUse;
        }

        public void Update()
        {
            if (!SoundIsPlaying())
            {
                _currentDelay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
                if (_currentDelay >= _loopDelay)
                {
                    _currentDelay = 0f;
                    CreateInstance();
                }
            }
            else if (!_isGlobalSound)
            {
                _sound.Volume = GetVolume();
                if (_sound.Volume == 0) _sound.Stop();
                _sound.Pitch = GetPitch();
                _sound.Pan = GetPan();
            }
        }

        private bool SoundIsPlaying()
        {
            return _sound != null && !_sound.IsDisposed && _sound.State == SoundState.Playing;
        }

        private void CreateInstance()
        {
            float volume = GetVolume();
            if (volume == 0 && _stopWhenNotUse) return;

            _sound = Sounds.Get(_soundType).CreateInstance();
            _sound.Volume = volume;
            _sound.Pitch = GetPitch();
            _sound.Pan = GetPan();
            _sound.IsLooped = _loopDelay == 0;
            _sound.Play();
        }
        private float GetVolume()
        {
            float realBaseVolume = (float)Math.Pow(_baseVolume, 2);

            if (_isGlobalSound) return realBaseVolume * Core.Settings.AtmosphereRealVolume;
            else
            {
                float distance = MathHelper.Distance(_origin.X, _listener.X) + MathHelper.Distance(_origin.Y, _listener.Y);
                float bruteDistanceVolume = MathHelper.Clamp(distance / _playRange, 0, 1);
                float distanceVolume = (float)Math.Pow(bruteDistanceVolume, 2);
                return realBaseVolume * distanceVolume * Core.Settings.AtmosphereRealVolume;
            }
        }
        private float GetPitch()
        {
            if (!_usePitchRange) return _pitchMin;
            else return Random.Shared.NextFloat(_pitchMin, _pitchMax);
        }
        private float GetPan()
        {
            if (!_panAllowed) return 0f;
            else
            {
                float distanceX = _origin.X - _listener.X;
                float bruteRange = distanceX / _playRange;
                return Math.Clamp(bruteRange, -1f, 1f);
            }
        }
    }
}
