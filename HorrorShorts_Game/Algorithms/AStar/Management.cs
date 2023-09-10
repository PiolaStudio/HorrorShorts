using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Algorithms.AStar
{
    public class Management
    {
        private static readonly AStar_Rules DefaultRules = new();

        private Node[,] nodes;
        public void LoadMap(Node[,] nodes)
        {
            this.nodes = nodes;

            cancellationToken?.Cancel();
            cancellationToken = new();
        }

        public List<Node> FindPath(Point posA, Point posB, AStar_Rules rules = null)
        {
            try
            {
                rules ??= DefaultRules; //If the rules is null use the defaults rules

                if (posA == posB) return new();

                int width = nodes.GetLength(0);
                int height = nodes.GetLength(1);

                if (posA.X < 0 || posA.Y < 0 || posA.X >= width || posA.Y >= height) return null; //Start Point out of map limits
                if (posB.X < 0 || posB.Y < 0 || posB.X >= width || posB.Y >= height) return null; //End Point out of map limits

                List<NodeBuffer> openSet = new();
                List<NodeBuffer> closedSet = new();

                Node startNode = nodes[posA.X, posA.Y];
                Node endNode = nodes[posB.X, posB.Y];

                openSet.Add(new NodeBuffer(startNode));

                NodeBuffer endNodeBuffer = null;
                while (openSet.Count > 0)
                {
                    NodeBuffer fatherNode = openSet.MinBy(x => x.F);
                    closedSet.Add(fatherNode);
                    openSet.Remove(fatherNode);

                    //Exploration
                    try
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            int x = fatherNode.Node.X + i;
                            if (x < 0 || x >= width) continue; //Out of map limits

                            for (int j = -1; j < 2; j++)
                            {
                                int y = fatherNode.Node.Y + j;
                                if (y < 0 || y >= height) continue; //Out of map limits

                                //Get Node
                                Node node = nodes[x, y];

                                if (node == fatherNode.Node) continue; //Same node as father, ignore
                                if (node.Cost == Node.IMPASSABLE_COST) continue; //Impassable node, ignore
                                if (closedSet.Find(x => x.Node == node) != null) continue; //Processed node, ignore

                                bool isDiagonal = i * j != 0;

                                if (isDiagonal)
                                {
                                    if (!rules.CanDoDiagonal) continue; //Diagonals is ilegal

                                    if (rules.DiagonalCheckBlocks)
                                    {
                                        Node borderNode1 = nodes[fatherNode.Node.X, y];
                                        Node borderNode2 = nodes[x, fatherNode.Node.Y];

                                        if (borderNode1.Cost == Node.IMPASSABLE_COST) continue; //Can't get through a diagonal impassable node
                                        if (borderNode2.Cost == Node.IMPASSABLE_COST) continue; //Can't get through a diagonal impassable node
                                    }
                                }

                                //Find node in the white list
                                NodeBuffer nb = openSet.Find(n => n.Node == node);

                                if (nb == null) //Need add the node to the white list
                                {
                                    //Compute Cost
                                    float G = 0;
                                    if (isDiagonal) G = MathF.Sqrt(MathF.Pow(node.Cost, 2) * 2f) + fatherNode.G;
                                    else G = node.Cost + fatherNode.G;

                                    if (rules.MaxCostAllowed > -1 && G > rules.MaxCostAllowed) continue; //Cost limit exceeded

                                    //Compute Heuristic
                                    float H = 0;
                                    int distance = Math.Abs(node.X - endNode.X) + Math.Abs(node.Y - endNode.Y);
                                    H = distance * 10;

                                    //Compute .
                                    float F = G + H;

                                    if (endNodeBuffer != null && F >= endNodeBuffer.F) continue; //The cost is major to the actual path cost

                                    //Create Buffer Node
                                    nb = new(node, fatherNode, G, H, F);
                                    openSet.Add(nb);

                                    //End node is reached
                                    if (nb.Node == endNode)
                                    {
                                        endNodeBuffer = nb;

                                        if (rules.EndMode == AStar_Rules.EndModes.FirstFound)
                                            goto WAY_BACK; //The first way was found, continue

                                        //Deprecate obsolete nodes from the white list
                                        for (int k = 0; k < openSet.Count; k++)
                                        {
                                            NodeBuffer deprecateNode = openSet[k];
                                            if (deprecateNode.F >= nb.F)
                                            {
                                                openSet.RemoveAt(k);
                                                closedSet.Add(deprecateNode);
                                                k--;
                                            }
                                        }
                                    }
                                }
                                else //The node is in the white list
                                {
                                    //Compute new Cost
                                    float G = 0;
                                    if (isDiagonal) G = MathF.Sqrt(MathF.Pow(node.Cost, 2) * 2f) + fatherNode.G;
                                    else G = node.Cost + fatherNode.G;

                                    if (G < nb.G)
                                    {
                                        //Best way to arrive the node, update it
                                        nb.G = G;
                                        nb.F = nb.G + nb.H;
                                        nb.ParentNode = fatherNode;
                                    }
                                    else continue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new();
                        sb.AppendLine("Error searching a path.");
                        sb.Append($"Parent Node: X: {fatherNode.Node.X} Y: {fatherNode.Node.Y}");
                        sb.AppendLine("Exception: " + ex.Message);
                        Logger.Error(sb.ToString());
                    }
                }

            WAY_BACK:
                if (endNodeBuffer != null) //The path was found
                {
                    //Way back the nodes
                    List<Node> wayBackNodes = new();
                    NodeBuffer prevNode = endNodeBuffer;

                    while (prevNode.Node != startNode)
                    {
                        wayBackNodes.Add(prevNode.Node);
                        prevNode = prevNode.ParentNode;
                    }

                    wayBackNodes.Reverse();

                    //Return the Path
                    return wayBackNodes;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new();
                sb.AppendLine("FATAL error searching a path.");
                sb.AppendLine("Exception: " + ex.Message);
                Logger.Error(sb.ToString());
            }

            //The path was not found
            return null;
        }

        //ASYNC
        private CancellationTokenSource cancellationToken;
        public async Task<List<Node>> FindPath_Async(Point posA, Point posB, AStar_Rules rules = null)
        {
            return await Task.Factory.StartNew(() => FindPath(posA, posB, rules), cancellationToken.Token);
        }
    }
}
