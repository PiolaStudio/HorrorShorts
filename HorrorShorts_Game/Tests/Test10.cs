#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test10 : TestBase
    {
        private bool _fadeFlag = true;
        public override void Update1()
        {
#if DESKTOP
            if (Core.Controls.Keyboard.ActionTrigger)
            {
                _fadeFlag = !_fadeFlag;
                if (_fadeFlag) Core.FadeEffect.FadeIn();
                else Core.FadeEffect.FadeOut();
            }
#endif
        }
    }
}
#endif