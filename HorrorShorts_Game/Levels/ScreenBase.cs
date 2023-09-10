using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Levels
{
    public abstract class ScreenBase
    {
        public virtual void LoadContent() { }
        public virtual void Update() { }
        public virtual void PreDraw() { }
        public void Draw(LayerType layer)
        {
            switch (layer)
            {
                case LayerType.Background9:
                    DrawBackground9();
                    break;
                case LayerType.Background8:
                    DrawBackground8();
                    break;
                case LayerType.Background7:
                    DrawBackground7();
                    break;
                case LayerType.Background6:
                    DrawBackground6();
                    break;
                case LayerType.Background5:
                    DrawBackground5();
                    break;
                case LayerType.Background4:
                    DrawBackground4();
                    break;
                case LayerType.Background3:
                    DrawBackground3();
                    break;
                case LayerType.Background2:
                    DrawBackground2();
                    break;
                case LayerType.Background1:
                    DrawBackground1();
                    break;
                case LayerType.Entities:
                    DrawEntities();
                    break;
                case LayerType.Frontground1:
                    DrawFrontground1();
                    break;
                case LayerType.Frontground2:
                    DrawFrontground2();
                    break;
                case LayerType.Frontground3:
                    DrawFrontground3();
                    break;
                case LayerType.Frontground4:
                    DrawFrontground4();
                    break;
                case LayerType.Frontground5:
                    DrawFrontground5();
                    break;
                case LayerType.Frontground6:
                    DrawFrontground6();
                    break;
            }
        }
        public virtual void DrawBackground9() { }
        public virtual void DrawBackground8() { }
        public virtual void DrawBackground7() { }
        public virtual void DrawBackground6() { }
        public virtual void DrawBackground5() { }
        public virtual void DrawBackground4() { }
        public virtual void DrawBackground3() { }
        public virtual void DrawBackground2() { }
        public virtual void DrawBackground1() { }
        public virtual void DrawEntities() { }
        public virtual void DrawFrontground1() { }
        public virtual void DrawFrontground2() { }
        public virtual void DrawFrontground3() { }
        public virtual void DrawFrontground4() { }
        public virtual void DrawFrontground5() { }
        public virtual void DrawFrontground6() { }
        public virtual void DrawUI() { }

        public virtual void Dispose() { }
    }
}
