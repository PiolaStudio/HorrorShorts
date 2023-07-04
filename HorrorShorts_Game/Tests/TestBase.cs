#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Tests
{
    public abstract class TestBase
    {
        public virtual void LoadContent1() { }
        public virtual void LoadContent2() { }
        public virtual void LoadContent3() { }

        public virtual void Update1() { }
        public virtual void Update2() { }
        public virtual void Update3() { }

        public virtual void PreDraw1() { }
        public virtual void PreDraw2() { }
        public virtual void PreDraw3() { }

        public virtual void Draw1() { }
        public virtual void Draw2() { }
        public virtual void Draw3() { }
    }
}
#endif