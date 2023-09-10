using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HorrorShorts_Game.Screens;

namespace HorrorShorts_Game.Levels.Empty
{
    public class EmptyLevel : LevelBase
    {
        public EmptyLevel()
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();
            Loaded = true;
        }
    }
}
