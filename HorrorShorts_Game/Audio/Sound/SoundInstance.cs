using Microsoft.Xna.Framework.Audio;

namespace HorrorShorts_Game.Audio.Sound
{
    public class SoundInstance
    {
        private SoundEffectInstance _soundEffect;

        public SoundState State { get => _soundEffect.State; }
        public float Volume
        {
            get => _soundEffect.Volume;
            set => _soundEffect.Volume = Core.Settings.EffectsVolume * value;
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

        internal SoundInstance(SoundEffect soundEffect, float volume = 1f, float pan = 0f, float pitch = 0f)
        {
            _soundEffect = soundEffect.CreateInstance();
            Volume = volume;
            Pan = 0f;
            Pitch = pitch;
        }
    }
}
