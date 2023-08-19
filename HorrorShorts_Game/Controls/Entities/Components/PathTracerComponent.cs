using HorrorShorts_Game.Algorithms.AStar;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HorrorShorts_Game.Controls.Entities.Components
{
    public class PathTracerComponent : IComponent
    {
        private List<Node> _path = new List<Node>();

        public bool FindingPathAsyn { get; private set; }

        public void Update()
        {

            _path.RemoveAt(_path.Count - 1);

        }
        public void GoTo(Point posB)
        {
        }
        public void GoTo_Async()
        {

        }
    }
}
