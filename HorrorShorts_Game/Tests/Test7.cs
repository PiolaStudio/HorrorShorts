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
    public class Test7 : TestBase
    {
        Management aStar = new();
        Node[,] map;
        List<Node> path;

        int nodeSize = 16;
        bool space = false;
        bool R = false;

        float elapsed = 0;
        bool showPath = false;

        Texture2D furnanceSheet;
        Texture2D background;

        private readonly Rectangle doorSheet = new(0, 0, 48, 128);
        private readonly Rectangle closetSheet = new(48, 0, 80, 96);
        private readonly Rectangle chestSheet = new(48, 96, 64, 32);
        private readonly Rectangle windowSheet = new(128, 0, 48, 48);
        private readonly Rectangle plantSheet = new(128, 48, 16, 48);
        private readonly Rectangle carpetSheet = new(144, 48, 48, 48);
        private readonly Rectangle cribSheet = new(0, 128, 96, 64);

        private Point currentPos = new(7, 16);
        private int pathIndex = 0;

        public override void LoadContent1()
        {
            aStar = new();
            map = new Node[20, 20];

            //Impassable
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 13; j++)
                    map[i, j] = new(i, j, Node.IMPASSABLE_COST);

            for (int j = 13; j < 15; j++)
                for (int i = 0; i < 3; i++)
                {
                    map[i, j] = new(i, j, Node.IMPASSABLE_COST);
                    map[19 - i, j] = new(19 - i, j, Node.IMPASSABLE_COST);
                }

            for (int j = 15; j < 17; j++)
                for (int i = 0; i < 2; i++)
                {
                    map[i, j] = new(i, j, Node.IMPASSABLE_COST);
                    map[19 - i, j] = new(19 - i, j, Node.IMPASSABLE_COST);
                }

            for (int i = 0; i < 6; i++)
                map[12 + i, 16] = new(12 + i, 16, Node.IMPASSABLE_COST);


            for (int j = 17; j < 19; j++)
            {
                map[0, j] = new(0, j, Node.IMPASSABLE_COST);
                map[19, j] = new(19, j, Node.IMPASSABLE_COST);
            }

            for (int i = 0; i < 20; i++)
                map[i, 19] = new(i, 19, Node.IMPASSABLE_COST);

            //Passable
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if (map[i, j].Cost != -1)
                        map[i, j] = new(i, j, 10);

            aStar.LoadMap(map);


            background = Core.Content.Load<Texture2D>("Textures/Backgrounds/RoomBackground");
            furnanceSheet = Core.Content.Load<Texture2D>("Textures/Backgrounds/FurnacesSheet");
        }
        public override void Update1()
        {
        }
        public override void Draw1()
        {
            Core.SpriteBatch.Draw(background, Vector2.Zero, Color.White);

            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(17, 140), doorSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(53, 116), closetSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(128, 180), chestSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(198, 121), windowSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(0, 270), plantSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(46, 228), carpetSheet, Color.White);
            Core.SpriteBatch.Draw(furnanceSheet, new Vector2(194, 205), cribSheet, Color.White);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
                                i * nodeSize,
                                j * nodeSize,
                                nodeSize, nodeSize),
                            map[i, j].Cost == Node.IMPASSABLE_COST ? new Color(Color.Blue, 0.25f) : new Color(Color.White, 0.25f));
                    }
                }
            }
        }
        public override void Draw2()
        {
            
        }
    }
}
#endif