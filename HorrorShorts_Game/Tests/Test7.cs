#if DEBUG
using HorrorShorts_Game.Algorithms.AStar;
using HorrorShorts_Game.Controls.Animations;
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
        List<Node> path = new();
        private float elapsedTimePath = 0f;

        int nodeSize = 16;
        int nodePadding = 2;
        bool space = false;
        bool R = false;

        float elapsed = 0;
        bool showPath = false;

        private Texture2D furnanceSheet;
        private Texture2D background;

        private readonly Rectangle doorSheet = new(0, 0, 48, 128);
        private readonly Rectangle closetSheet = new(48, 0, 80, 96);
        private readonly Rectangle chestSheet = new(48, 96, 64, 32);
        private readonly Rectangle windowSheet = new(128, 0, 48, 48);
        private readonly Rectangle plantSheet = new(128, 48, 16, 48);
        private readonly Rectangle carpetSheet = new(144, 48, 48, 48);
        private readonly Rectangle cribSheet = new(0, 128, 96, 64);

        private readonly Rectangle doorRectangle = new(41, 268, 48, 128);
        private readonly Rectangle closetRectangle = new(93, 212, 80, 96);
        private readonly Rectangle chestRectangle = new(160, 212, 64, 32);
        private readonly Rectangle windowRectangle = new(222, 169, 48, 48);
        private readonly Rectangle plantRectangle = new(8, 318, 16, 48);
        private readonly Rectangle carpetRectangle = new(70, 276, 48, 48);
        private readonly Rectangle cribRectangle = new(242, 269, 96, 64);

        private Vector2 doorOrigin;
        private Vector2 closetOrigin;
        private Vector2 chestOrigin;
        private Vector2 windowOrigin;
        private Vector2 plantOrigin;
        private Vector2 carpetOrigin;
        private Vector2 cribOrigin;


        private Texture2D characterTexture;
        private Rectangle characterRectangle;
        private readonly Rectangle characterSheet = new(0, 0, 48, 80);
        private SpriteEffects characterFlip = SpriteEffects.None;

        private Point currentPos = new(7, 16);
        private int pathIndex = 0;
        private Vector2 characterOrigin;
        private AnimationSystem animationSystem;


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
            characterTexture = Core.Content.Load<Texture2D>("Textures/Characters/Mummy");

            doorOrigin = new(doorRectangle.Width / 2, doorRectangle.Height);
            closetOrigin = new(closetRectangle.Width / 2, closetRectangle.Height);
            chestOrigin = new(chestRectangle.Width / 2, chestRectangle.Height);
            windowOrigin = new(windowRectangle.Width / 2, windowRectangle.Height);
            plantOrigin = new(plantRectangle.Width / 2, plantRectangle.Height);
            carpetOrigin = new(carpetRectangle.Width / 2, carpetRectangle.Height);
            cribOrigin = new(cribRectangle.Width / 2, cribRectangle.Height);

            characterOrigin = new(characterSheet.Width / 2f, characterSheet.Height);
        }

        public override void Update1()
        {
            MouseUpdate();

            //Core.Window.Title = Core.Controls.Mouse.Position.ToString() + "  " + Core.Controls.Mouse.PositionUI.ToString();

            if (path.Count > 0)
            {
                elapsedTimePath += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTimePath > 250)
                {
                    elapsedTimePath = 0;
                    if (path[0].X < currentPos.X) characterFlip = SpriteEffects.None;
                    else if (path[0].X > currentPos.X) characterFlip = SpriteEffects.FlipHorizontally;
                    currentPos = new(path[0].X, path[0].Y);
                    path.RemoveAt(0);
                }
            }

            characterRectangle = new(
                currentPos.X * nodeSize + nodeSize / 2,
                (currentPos.Y + 1) * nodeSize, 
                characterSheet.Width, characterSheet.Height);
        }
        private void MouseUpdate()
        {
            if (Core.Controls.Click)
            {
                Point nodeClicked = new(Core.Controls.ClickPosition.X / nodeSize, Core.Controls.ClickPosition.Y / nodeSize);
                if (map[nodeClicked.X, nodeClicked.Y].Cost != Node.IMPASSABLE_COST)
                {
                    path = aStar.FindPath(currentPos, nodeClicked);
                    //currentPos.X = nodeClicked.X;
                    //currentPos.Y = nodeClicked.Y;
                }
            }
        }
        public override void Draw1()
        {
            Core.SpriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Core.SpriteBatch.Draw(furnanceSheet, doorRectangle, doorSheet, Color.White, 0f, doorOrigin, SpriteEffects.None, (doorRectangle.Bottom - doorOrigin.Y) / 320f);
            Core.SpriteBatch.Draw(furnanceSheet, closetRectangle, closetSheet, Color.White, 0f, closetOrigin, SpriteEffects.None, (closetRectangle.Bottom - closetOrigin.Y) / 320f);
            Core.SpriteBatch.Draw(furnanceSheet, chestRectangle, chestSheet, Color.White, 0f, chestOrigin, SpriteEffects.None, (chestRectangle.Bottom - chestOrigin.Y) / 320f);
            Core.SpriteBatch.Draw(furnanceSheet, windowRectangle, windowSheet, Color.White, 0f, windowOrigin, SpriteEffects.None, (windowRectangle.Bottom - windowOrigin.Y) / 320f);
            Core.SpriteBatch.Draw(furnanceSheet, plantRectangle, plantSheet, Color.White, 0f, plantOrigin, SpriteEffects.None, (plantRectangle.Bottom - plantSheet.Y) / 320f);
            Core.SpriteBatch.Draw(furnanceSheet, carpetRectangle, carpetSheet, Color.White, 0f, carpetOrigin, SpriteEffects.None, 0.001f);
            Core.SpriteBatch.Draw(furnanceSheet, cribRectangle, cribSheet, Color.White, 0f, cribOrigin, SpriteEffects.None, (cribRectangle.Bottom - cribOrigin.Y) / 320f);

            Core.SpriteBatch.Draw(characterTexture, characterRectangle, characterSheet, Color.White, 0f,
                characterOrigin, characterFlip,
                (characterRectangle.Bottom - characterOrigin.Y) / 320f);

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
                                nodeSize - nodePadding / 2, 
                                nodeSize - nodePadding / 2),
                            null,
                            currentPos == new Point(i, j) ? new Color(Color.Green, 0.25f) :
                            path.Contains(map[i, j]) ? new Color(Color.Red, 0.25f) :
                            map[i, j].Cost == Node.IMPASSABLE_COST ? new Color(Color.Blue, 0.25f) :
                            new Color(Color.White, 0.25f), 
                            0f, Vector2.Zero, SpriteEffects.None, 1f);
                    }   
                }
            }

#if DESKTOP
            Core.SpriteBatch.Draw(Textures.Pixel, Core.Controls.Mouse.Position.ToVector2(), null, Color.Red, 
                0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
#endif
        }
        public override void Draw2()
        {
            
        }
    }
}
#endif