using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Algorithms.AStar
{
    [DebuggerDisplay("X: {X} Y: {Y} | Cost:{Cost > 0 ? Cost.ToString() : \"IMPASSABLE\",nq}")]
    public struct Node
    {
        public const int IMPASSABLE_COST = -1;

        public int Cost = 10;
        public int X = 0;
        public int Y = 0;
        private string DebuggerDisplay
        {
            get
            {
                return $"{X} {Y} Cost: ";
            }
        }

        public Node()
        {
            Cost = 10;
            X = 0;
            Y = 0;
        }
        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
        }

        public static bool operator ==(Node a, Node b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Node a, Node b)
        {
            return !a.Equals(b);
        }
        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != typeof(Node)) return false;

            return X == ((Node)obj).X && Y == ((Node)obj).Y;
        }

        public override int GetHashCode()
        {
            //todo: no implementado correctamente
            return (17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode() + Cost;
        }
    }
}
