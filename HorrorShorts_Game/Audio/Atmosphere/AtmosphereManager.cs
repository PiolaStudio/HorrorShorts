using Microsoft.Xna.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Audio.Atmosphere
{
    public class AtmosphereManager
    {
        private readonly Dictionary<string, AtmosphereInstance> _sounds = new();
        public AtmosphereInstance this[string id] { get => _sounds[id]; }

        private Point Listener = Point.Zero;
        private float _atmosphereVolume;

        public AtmosphereManager()
        {
            _atmosphereVolume = Core.Settings.GeneralVolume * Core.Settings.AtmosphereVolume;
        }
        public void Load()
        {
            //todo
        }
        public void Add(string name, AtmosphereSound sound)
        {
            AtmosphereInstance instance = new(sound);
            _sounds.Add(name, instance);
        }

        public void Clear()
        {
            //todo
            _sounds.Clear();
        }

        public void Update()
        {
            foreach (AtmosphereInstance sound in _sounds.Values)
            {
                sound.Listener = Listener;
                sound.Update();
            }

            //Volume changed
            float volume = Core.Settings.GeneralVolume * Core.Settings.AtmosphereVolume;
            if (volume != _atmosphereVolume)
            {
                _atmosphereVolume = volume;
                foreach (AtmosphereInstance sound in _sounds.Values)
                    sound.RefreshVolume();
            }
        }
    }
}
