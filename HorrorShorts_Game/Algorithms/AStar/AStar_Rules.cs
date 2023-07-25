using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Algorithms.AStar
{
    public class AStar_Rules
    {
        public bool CanDoDiagonal = true;
        public bool DiagonalCheckBlocks = true; //Only when CanDoDiagonal is true
        public EndModes EndMode = EndModes.FirstFound;
        public int MaxCostAllowed = -1;  //-1 to disable

        public enum EndModes
        {
            FirstFound,
            MinorCost
        }
    }
}
