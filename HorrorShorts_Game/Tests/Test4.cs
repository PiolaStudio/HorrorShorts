using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test4 : TestBase
    {
        SoundBank SoundBank;
        WaveBank WaveBank;
        AudioEngine AudioEngine;

        public override void LoadContent1()
        {
            //AudioEngine = new AudioEngine("Content/Soundtrack/TerrariaMusic.xgs");
            //WaveBank = new WaveBank(AudioEngine, "Content/Soundtrack/Wave Bank.xwb");
            //SoundBank = new SoundBank(AudioEngine, "Content/Soundtrack/Sound Bank.xsb");
            //Cue c = SoundBank.GetCue("Music_10");
            //c.SetVariable("Volume", 1f);
            ////c.Play();


            AudioEngine = new AudioEngine("Content/Soundtrack/2/FarmerSounds.xgs");
            WaveBank = new WaveBank(AudioEngine, "Content/Soundtrack/2/Wave Bank.xwb");
            SoundBank = new SoundBank(AudioEngine, "Content/Soundtrack/2/Sound Bank.xsb");
            Cue c = SoundBank.GetCue("spring_day_ambient");
            c.SetVariable("Volume", 1f);
            c.Play();


            //Song song = Core.Content.Load<Song>(@"Soundtrack/TestSong");
            ////MediaPlayer.IsShuffled
            //MediaPlayer.Play(song);
            //MediaPlayer.IsShuffled = true;
            //MediaPlayer.IsRepeating = true;
        }

        bool flag;
        float delay = 0;
        public override void Update1()
        {
            AudioEngine.Update();

            //delay += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
            //if (delay > 2000)
            //{
            //    delay = 0;
            //    flag = true;

            //    SoundEffectInstance instance = Sounds.Speak2.CreateInstance();
            //    instance.Play();
            //}
        }
    }
}
