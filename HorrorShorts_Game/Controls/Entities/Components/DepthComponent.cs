using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Entities.Components
{
    public class DepthComponent : IComponent
    {
        private Entity _parentEntity;
        public DepthComponent(Entity parentEntity)
        {
            _parentEntity = parentEntity;
        }
        public void ApplyDepth()
        {
            float d = (_parentEntity.Sprite.Bottom - _parentEntity.Sprite.OriginY + _parentEntity.Altitude) / 320f;
            _parentEntity.Sprite.Depth = MathHelper.Clamp(d, 0f, 1f);
        }
    }
}
