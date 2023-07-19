using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Audio;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Audio
{
    public class SongManager
    {
        private SongBuffer _currentSong = null;
        private readonly List<SongBuffer> _prevSong = new();
        private readonly Dictionary<SongType, SongBuffer> _parallelSongs = new();

        public float Volume
        {
            get => _baseVolume;
            set
            {
                _baseVolume = value;
                _realBaseVolume = (float)Math.Pow(value, 2);

                if (_currentSong != null)
                    _currentSong.Sound.Volume = _realBaseVolume * Core.Settings.MusicRealVolume;
            }
        }
        private float _baseVolume = 1f;
        private float _realBaseVolume = 1f;

        public bool IsPlaying 
        {
            get
            {
                if (_currentSong == null) return false;
                if (_currentSong.Sound.State != SoundState.Playing) return false;
                return true;
            }
        }
        public SongType? Song { get => _currentSong?.SoundType; }

        public void LoadForParallel(SongType[] types)
        {
            //Stop previous (parallels)
            foreach (SongBuffer sb in _parallelSongs.Values)
            {
                sb.Sound.Stop();
                sb.Sound.Dispose();
            }
            _parallelSongs.Clear();

            //Create sounds
            for (int i = 0; i < types.Length; i++)
            {
                SoundEffect se = Songs.Get(types[i]);
                SongBuffer s = new(se, types[i], true);
                s.Sound.Volume = 0;

                _parallelSongs.Add(types[i], s);
            }

            //Play in parallel
            Parallel.ForEach(_parallelSongs.Keys, index => 
                _parallelSongs[index].Sound.Play());
        }
        public void PlayFromParallel(SongType type, float inDelay = 0, float outDelay = 0)
        {
            Stop(outDelay);

            _currentSong = _parallelSongs[type];
            _currentSong.CurrentDelay = 0;

            if (inDelay > 0)
            {
                _currentSong.TotalDelay = inDelay;
                _currentSong.Sound.Volume = 0f;
            }
            else
            {
                _currentSong.TotalDelay = 0;
                _currentSong.Sound.Volume = _baseVolume * Core.Settings.GeneralVolume;
            }
        }

        public void Play(SongType type, float inDelay = 0, float outDelay = 0)
        {
            Stop(outDelay);

            SoundEffect se = Songs.Get(type);
            SongBuffer s = new(se, type);

            if (inDelay > 0)
            {
                //Start with intro
                s.TotalDelay = inDelay;
                s.Sound.Volume = 0f;
            }
            else s.Sound.Volume = _realBaseVolume * Core.Settings.MusicRealVolume;

            s.Sound.Play();
            _currentSong = s;
        }
        public void Stop(float outDelay = 0)
        {
            if (_currentSong == null) return;

            if (outDelay == 0)
            {
                //Stop Inmediate
                if (!_currentSong.IsInParallel)
                {
                    if (_currentSong.Sound.State == SoundState.Playing) _currentSong.Sound.Stop();
                    _currentSong.Sound.Dispose();
                }
                else _currentSong.Sound.Volume = 0f;

                _currentSong = null;
            }
            else
            {
                //Prev sound outro
                _currentSong.TotalDelay = outDelay;
                _currentSong.CurrentDelay = 0f;
                _prevSong.Add(_currentSong);

                _currentSong = null;
            }
        }

        internal void Update()
        {
            if (_currentSong != null)
                if (_currentSong.CurrentDelay != _currentSong.TotalDelay)
                {
                    _currentSong.CurrentDelay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_currentSong.CurrentDelay > _currentSong.TotalDelay)
                        _currentSong.CurrentDelay = _currentSong.TotalDelay;

                    _currentSong.Sound.Volume = ((float)Math.Pow(_currentSong.CurrentDelay / _currentSong.TotalDelay, 2))
                        * _realBaseVolume
                        * Core.Settings.MusicRealVolume;
                }

            for (int i = 0; i < _prevSong.Count; i++)
            {
                SongBuffer sb = _prevSong[i];

                if (sb.CurrentDelay != sb.TotalDelay)
                {
                    sb.CurrentDelay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
                    if (sb.CurrentDelay > sb.TotalDelay)
                    {
                        sb.CurrentDelay = sb.TotalDelay;
                        if (!sb.IsInParallel)
                        {
                            sb.Sound.Stop();
                            sb.Sound.Dispose();
                        }
                        else sb.Sound.Volume = 0f;

                        _prevSong.Remove(sb);
                        continue;
                    }
                    else sb.Sound.Volume = ((float)Math.Pow(1f - (sb.CurrentDelay / sb.TotalDelay), 2))
                            * _realBaseVolume
                            * Core.Settings.MusicRealVolume;
                }
            }
        }


        private class SongBuffer
        {
            public float TotalDelay = 0;
            public float CurrentDelay = 0;
            public readonly bool IsInParallel = false;
            public readonly SoundEffectInstance Sound;
            public readonly SongType SoundType;

            public SongBuffer(SoundEffect song, SongType soundType, bool isInParallel = false)
            {
                Sound = song.CreateInstance();
                Sound.IsLooped = true;

                SoundType = soundType;
                IsInParallel = isInParallel;
            }
        }
    }
}
