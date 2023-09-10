using Microsoft.Xna.Framework;
using Resources;
using Resources.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Audio.Atmosphere
{
    public struct AtmosphereSound
    {
        public SoundType Sound;
        public float LoopDelay;
        public float BaseVolume;

        public float PitchMin;
        public float PitchMax;

        public bool PanAllowed;

        public bool IsGlobalSound;
        public int PlayRange;
        public Point Origin;
        public bool StopWhenNotUse;

        public AtmosphereSound(SoundType soundType = SoundType.Field1_Atmosphere)
        {
            Sound = soundType;
            LoopDelay = 0f;
            BaseVolume = 1f;
            PitchMin = 0f;
            PitchMax = 0f;
            PanAllowed = false;
            IsGlobalSound = true;
            PlayRange = 0;
            Origin = Point.Zero;
            StopWhenNotUse = false;
        }
        public AtmosphereSound(AtmosphereSound_Serial serial)
        {
            Sound = serial.Sound;
            LoopDelay = serial.LoopDelay ?? 0f;
            BaseVolume = serial.BaseVolume ?? 1f;
            PitchMin = serial.PitchMin ?? 0f;
            PitchMax = serial.PitchMax ?? 0f;
            PanAllowed = serial.PanAllowed ?? false;
            IsGlobalSound = serial.IsGlobalSound ?? true;
            PlayRange = serial.PlayRange ?? 0;
            Origin = serial.Origin ?? Point.Zero;
            StopWhenNotUse = serial.StopWhenNotUse ?? false;
        }
        public AtmosphereSound(AtmosphereSound original, Point origin)
        {
            Sound = original.Sound;
            LoopDelay = original.LoopDelay;
            BaseVolume = original.BaseVolume;
            PitchMin = original.PitchMin;
            PitchMax = original.PitchMax;
            PanAllowed = original.PanAllowed;
            IsGlobalSound = original.IsGlobalSound;
            PlayRange = original.PlayRange;
            Origin = origin;
            StopWhenNotUse = original.StopWhenNotUse;
        }
    }
}
