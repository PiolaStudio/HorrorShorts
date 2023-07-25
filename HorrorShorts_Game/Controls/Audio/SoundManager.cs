using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Audio;
using Resources;
using System;
using System.Collections.Generic;

namespace HorrorShorts_Game.Controls.Audio
{
    public class SoundManager
    {
        public void Play(SoundType type, float volume = 1, float pitch = 0, float pan = 0) 
        {
            float realVolume = volume * Core.Settings.EffectsRealVolume;
            Sounds.Get(type).Play(realVolume, pitch, pan);
        }
        public SEInstance GetInstance(SoundType type, float volume = 1, float pitch = 0, float pan = 0)
        {
            return new(Sounds.Get(type), volume);
        }
    }
}

