using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Algorithms.AStar
{
    [DebuggerDisplay("G: {G.ToString(\"0.00\"),nq}  H: {H.ToString(\"0.00\"),nq}  F: {F.ToString(\"0.00\"),nq}")]
    internal class NodeBuffer
    {
        public Node Node;
        public NodeBuffer ParentNode;

        public float G; //Cost
        public float H; //Heuristic
        public float F; //Estimasted Cost (G + H)

        public NodeBuffer(Node node) //for father node
        {
            Node = node;
            ParentNode = null;
            G = 0;
            H = 0;
            F = 0;
        }
        public NodeBuffer(Node node, NodeBuffer fatherNode, float G, float H, float F)
        {
            Node = node;
            ParentNode = fatherNode;

            this.G = G;
            this.H = H;
            this.F = F;
        }


        public static bool operator ==(NodeBuffer a, NodeBuffer b)
        {
            if (a is null && b is null) return true;
            return a.Equals(b);
        }
        public static bool operator !=(NodeBuffer a, NodeBuffer b)
        {
            if (a is null && b is null) return false;
            return !a.Equals(b);
        }
        public override bool Equals(object obj)
        {
#if DEBUG
            if (obj is null) return false;
            if (obj.GetType() != typeof(NodeBuffer)) return false;
#endif
            return Node == ((NodeBuffer)obj).Node;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
