#if DEBUG
using HorrorShorts_Game.Algorithms.AStar;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Resources;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Tests
{
    public class Test06 : TestBase
    {
        Management aStar = new();
        Node[,] map;
        List<Node> path;
        Point startPos;
        Point endPos;

        Task<List<Node>> task;
        Stopwatch sw;

        int nodeSize = 3;
        int padding = 10;
        bool space = false;
        bool R = false;

        float elapsed = 0;
        bool showPath = false;

        public override void LoadContent1()
        {
            aStar = new();
            map = new Node[40, 40];
            Regenerate();
        }
        public override void Update1()
        {
            if (Core.Controls.Keyboard.State.IsKeyDown(Keys.Space))
            {
                if (!space)
                {
                    space = true;
                    Regenerate();
                }
            }
            else space = false;

            if (Core.Controls.Keyboard.State.IsKeyDown(Keys.R))
            {
                if (!R)
                {
                    R = true;
                    path = null;

                    sw = Stopwatch.StartNew();
                    task = aStar.FindPath_Async(startPos, endPos);
                }
            }
            else R = false;

            if (task != null && task.IsCompletedSuccessfully)
            {
                path = task.Result;
                task = null;
                Core.Window.Title = sw.ElapsedMilliseconds.ToString();
            }

            elapsed += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed > 1000)
            {
                elapsed = 0;
                showPath = !showPath;
            }
        }
        public override void Draw1()
        {
            int l0 = map.GetLength(0);
            int l1 = map.GetLength(1);
            for (int i = 0; i < l0; i++)
            {
                for (int j = 0; j < l1; j++)
                {
                    Core.SpriteBatch.Draw(
                        Textures.Pixel, 
                        new Rectangle(
                            i * nodeSize + padding, 
                            Core.Resolution.Bounds.Y + j * nodeSize + padding, 
                            nodeSize, 
                            nodeSize), 
                        null,
                        map[i,j].Cost == Node.IMPASSABLE_COST ? Color.Blue : Color.White,
                        0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                }
            }

            if (showPath)
            {
                if (path != null)
                    for (int i = 0; i < path.Count; i++)
                    {
                        Core.SpriteBatch.Draw(
                            Textures.Pixel,
                            new Rectangle(
                                Core.Resolution.Bounds.X + padding + path[i].X * nodeSize, 
                                Core.Resolution.Bounds.Y + padding + path[i].Y * nodeSize, 
                                nodeSize, 
                                nodeSize),
                            null,
                            Color.Red,
                            0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }

                Core.SpriteBatch.Draw(
                            Textures.Pixel,
                            new Rectangle(
                                startPos.X * nodeSize + padding,
                                Core.Resolution.Bounds.Y + startPos.Y * nodeSize + padding,
                                nodeSize,
                                nodeSize),
                            null,
                            Color.Green,
                            0f, Vector2.Zero, SpriteEffects.None, 1f);


                Core.SpriteBatch.Draw(
                            Textures.Pixel,
                            new Rectangle(
                                endPos.X * nodeSize + padding,
                                Core.Resolution.Bounds.Y + endPos.Y * nodeSize + padding,
                                nodeSize,
                                nodeSize),
                            null,
                            Color.DarkGreen,
                            0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }

        private void Regenerate()
        {
            int l0 = map.GetLength(0);
            int l1 = map.GetLength(1);
            for (int i = 0; i < l0; i++)
            {
                for (int j = 0; j < l1; j++)
                {
                    if (i + j == 0 || i * j == (l0 * l1) - 2)
                        map[i, j] = new Node(i, j, 10);
                    else
                        map[i, j] = new Node(i, j, Random.Shared.Next(3) == 0 ? Node.IMPASSABLE_COST : 10);
                }
            }

            while (true)
            {
                int x = Random.Shared.Next(l0);
                int y = Random.Shared.Next(l1);

                if (map[x, y].Cost != Node.IMPASSABLE_COST)
                {
                    startPos = new(x, y);
                    break;
                }
            }

            while (true)
            {
                int x = Random.Shared.Next(l0);
                int y = Random.Shared.Next(l1);

                if (map[x, y].Cost != Node.IMPASSABLE_COST && startPos.X != x && startPos.Y != y)
                {
                    endPos = new(x, y);
                    break;
                }
            }

            aStar.LoadMap(map);
            path = null;

            sw = Stopwatch.StartNew();
            task = aStar.FindPath_Async(startPos, endPos);
        }
    }
}
#endif