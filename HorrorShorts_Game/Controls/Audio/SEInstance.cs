using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Audio
{
    public class SEInstance
    {
        private SoundEffectInstance _soundEffect;

        public SoundState State { get => _soundEffect.State; }
        public float Volume
        {
            get => _soundEffect.Volume;
            set => _soundEffect.Volume = Core.Settings.MusicVolume * value;
        }
        public float Pitch
        {
            get => _soundEffect.Pitch;
            set => _soundEffect.Pitch = value;
        }
        public float Pan
        {
            get => _soundEffect.Pan;
            set => _soundEffect.Pan = value;
        }
        public bool IsLooped
        {
            get => _soundEffect.IsLooped;
            set => _soundEffect.IsLooped = value;
        }

        public void Play() => _soundEffect.Play();
        public void Pause() => _soundEffect.Pause();
        public void Resume() => _soundEffect.Resume();
        public void Stop() => _soundEffect.Stop();

        public bool IsDisposed { get => _soundEffect.IsDisposed; }
        public void Dispose() => _soundEffect.Dispose();

        internal SEInstance(SoundEffect soundEffect, float volume = 1f, float pan = 0f, float pitch = 0f)
        {
            this._soundEffect = soundEffect.CreateInstance();
            this.Volume = volume;
            this.Pan = 0f;
            this.Pitch = pitch;
        }
    }
}
