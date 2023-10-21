using HorrorShorts_Game.Audio.Song;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.UI.Options.SubOptions
{
    internal class SoundOption : OptionSubMenu
    {
        private Label _generalLBL;
        private Slider _generalSLR;

        private Label _musicLBL;
        private Slider _musicSLR;

        private Label _effectsLBL;
        private Slider _effectsSLR;

        private Label _atmosphereLBL;
        private Slider _atmosphereSLR;

        private SoundEffectInstance _generalSound;
        private SoundEffectInstance _musicSound;
        private SoundEffectInstance _effectSound;
        private SoundEffectInstance _atmosphereSound;

        public SoundOption()
        {
            Height = 112;
        }
        public override void LoadContent()
        {
            //General
            _generalLBL = new();
            _generalLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_generalLBL);

            _generalSLR = new();
            _generalSLR.UserVirtualZone = true;
            _generalSLR.Value = Core.Settings.GeneralVolume;
            _generalSLR.DragOnEvent += (s, e) =>
            {
                DisposeSound(_generalSound);

                SoundEffect se = Sounds.Get(SoundType.Test1);
                _generalSound = se.CreateInstance();
                _generalSound.IsLooped = true;
                _generalSound.Play();
                _generalSound.Volume = Core.Settings.GeneralVolume;
            };
            _generalSLR.DragOutEvent += (s, e) => DisposeSound(_generalSound);
            _generalSLR.ChangeValueEvent += (s, e) =>
            {
                Core.Settings.GeneralVolume = MathF.Pow(e, 2);
                Settings.Save(Core.Settings);
            };
            _generalSLR.LoadContent();
            _controls.Add(_generalSLR);

            //Music
            _musicLBL = new();
            _musicLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_musicLBL);

            _musicSLR = new();
            _musicSLR.UserVirtualZone = true;
            _musicSLR.Value = Core.Settings.MusicVolume;
            _musicSLR.DragOnEvent += (s, e) =>
            {
                DisposeSound(_musicSound);

                SoundEffect se = Sounds.Get(SoundType.Test1);
                _musicSound = se.CreateInstance();
                _musicSound.IsLooped = true;
                _musicSound.Volume = Core.Settings.MusicVolume;
                _musicSound.Play();
            };
            _musicSLR.DragOutEvent += (s, e) =>
            {
                DisposeSound(_musicSound);
                Settings.Save(Core.Settings);
            };
            _musicSLR.ChangeValueEvent += (s, e) => Core.Settings.MusicVolume = MathF.Pow(e, 2);
            _musicSLR.LoadContent();
            _controls.Add(_musicSLR);

            //Effects
            _effectsLBL = new();
            _effectsLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_effectsLBL);

            _effectsSLR = new();
            _effectsSLR.UserVirtualZone = true;
            _effectsSLR.Value = Core.Settings.EffectsVolume;
            _effectsSLR.DragOnEvent += (s, e) =>
            {
                DisposeSound(_effectSound);

                SoundEffect se = Sounds.Get(SoundType.Test1);
                _effectSound = se.CreateInstance();
                _effectSound.IsLooped = true;
                _effectSound.Volume = Core.Settings.EffectsVolume;
                _effectSound.Play();
            };
            _effectsSLR.DragOutEvent += (s, e) =>
            {
                DisposeSound(_effectSound);
                Settings.Save(Core.Settings);
            };
            _effectsSLR.ChangeValueEvent += (s, e) => Core.Settings.EffectsVolume = MathF.Pow(e, 2);
            _effectsSLR.LoadContent();
            _controls.Add(_effectsSLR);

            //Atmosphere
            _atmosphereLBL = new();
            _atmosphereLBL.Alignament = TextAlignament.MiddleLeft;
            _controls.Add(_atmosphereLBL);

            _atmosphereSLR = new();
            _atmosphereSLR.UserVirtualZone = true;
            _atmosphereSLR.Value = Core.Settings.AtmosphereVolume;
            _atmosphereSLR.DragOnEvent += (s, e) =>
            {
                DisposeSound(_atmosphereSound);

                SoundEffect se = Sounds.Get(SoundType.Test1);
                _atmosphereSound = se.CreateInstance();
                _atmosphereSound.IsLooped = true;
                _atmosphereSound.Volume = Core.Settings.AtmosphereVolume;
                _atmosphereSound.Play();
            };
            _atmosphereSLR.DragOutEvent += (s, e) =>
            {
                DisposeSound(_atmosphereSound);
                Settings.Save(Core.Settings);
            };
            _atmosphereSLR.ChangeValueEvent += (s, e) => Core.Settings.AtmosphereVolume = MathF.Pow(e, 2);
            _atmosphereSLR.LoadContent();
            _controls.Add(_atmosphereSLR);

            base.LoadContent();
        }
        public override void Update()
        {
            base.Update();

            if (_generalSound != null && _generalSound.State == SoundState.Playing)
                _generalSound.Volume = Core.Settings.GeneralVolume;

            if (_musicSound != null && _musicSound.State == SoundState.Playing)
                _musicSound.Volume = Core.Settings.MusicVolume;

            if (_effectSound != null && _effectSound.State == SoundState.Playing)
                _effectSound.Volume = Core.Settings.EffectsVolume;

            if (_atmosphereSound != null && _atmosphereSound.State == SoundState.Playing)
                _atmosphereSound.Volume = Core.Settings.AtmosphereVolume;
        }
        public override void Draw()
        {
            base.Draw();
        }
        public override void Dispose()
        {
            base.Dispose();

            DisposeSound(_generalSound);
            DisposeSound(_musicSound);
            DisposeSound(_effectSound);
            DisposeSound(_atmosphereSound);
        }

        private void DisposeSound(SoundEffectInstance sei)
        {
            if (sei != null && !sei.IsDisposed)
            {
                if (sei.State == SoundState.Playing) 
                    sei.Stop();
                sei.Dispose();
            }
        }

        public override void ComputePositions()
        {
            int currentY = InitY;

            //General
            _generalLBL.Position = new(LeftPad, currentY);
            _generalSLR.Position = new(RightPad - 80, currentY - 8);
            currentY += RowPad;

            //Music
            _musicLBL.Position = new(LeftPad, currentY);
            _musicSLR.Position = new(RightPad - 80, currentY - 8);
            currentY += RowPad;

            //Efects
            _effectsLBL.Position = new(LeftPad, currentY);
            _effectsSLR.Position = new(RightPad - 80, currentY - 8);
            currentY += RowPad;

            //Atmosphere
            _atmosphereLBL.Position = new(LeftPad, currentY);
            _atmosphereSLR.Position = new(RightPad - 80, currentY - 8);

            ComputeVirtualPositions();
        }
        public override void ComputeVirtualPositions()
        {
            _generalSLR.VirtualPosition = _generalSLR.Position + PanelPosition - _scrollPoint;
            _musicSLR.VirtualPosition = _musicSLR.Position + PanelPosition - _scrollPoint;
            _effectsSLR.VirtualPosition = _effectsSLR.Position + PanelPosition - _scrollPoint;
            _atmosphereSLR.VirtualPosition = _atmosphereSLR.Position + PanelPosition - _scrollPoint;
        }
        protected override void UpdateStates()
        {
            _generalSLR.IsEnable = !_musicSLR.IsDragging && !_effectsSLR.IsDragging && !_atmosphereSLR.IsDragging;
            _musicSLR.IsEnable = !_generalSLR.IsDragging && !_effectsSLR.IsDragging && !_atmosphereSLR.IsDragging;
            _effectsSLR.IsEnable = !_generalSLR.IsDragging && !_musicSLR.IsDragging && !_atmosphereSLR.IsDragging;
            _atmosphereSLR.IsEnable = !_generalSLR.IsDragging && !_musicSLR.IsDragging && !_effectsSLR.IsDragging;
        }

        public override void SetLocalization()
        {
            _generalLBL.Text = Localizations.Global.Options_Sound_General.ToUpper();
            _musicLBL.Text = Localizations.Global.Options_Sound_Music.ToUpper();
            _effectsLBL.Text = Localizations.Global.Options_Sound_Effects.ToUpper();
            _atmosphereLBL.Text = Localizations.Global.Options_Sound_Atmosphere.ToUpper();

            base.SetLocalization();
        }
    }
}
