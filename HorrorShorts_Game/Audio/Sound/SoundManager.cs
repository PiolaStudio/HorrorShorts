using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Audio;
using Resources;
using System;
using System.Collections.Generic;

namespace HorrorShorts_Game.Audio.Sound
{
    public class SoundManager
    {
        private List<SoundInstance> _soundInstances = new();
        private float _soundVolume;

        public SoundManager()
        {
            _soundVolume = Core.Settings.GeneralVolume * Core.Settings.EffectsVolume;
        }
        public void Update()
        {
            for (int i = 0; i < _soundInstances.Count; i++)
                if (_soundInstances[i].IsDisposed)
                {
                    _soundInstances.RemoveAt(i);
                    i--;
                }

            //Refresh Volume
            float volume = Core.Settings.GeneralVolume * Core.Settings.EffectsVolume;
            if (volume != _soundVolume)
            {
                _soundVolume = volume;
                foreach (SoundInstance sound in _soundInstances)
                    sound.RefreshVolume();
            }
        }
        public void Play(SoundType type, float volume = 1, float pitch = 0, float pan = 0)
        {
            float realVolume = volume * Core.Settings.EffectsVolume;
            Sounds.Get(type).Play(realVolume, pitch, pan);
        }
        public SoundInstance GetInstance(SoundType type, float volume = 1, float pitch = 0, float pan = 0)
        {
            SoundInstance si = new(Sounds.Get(type), volume);
            _soundInstances.Add(si);
            return si;
        }
    }
}

