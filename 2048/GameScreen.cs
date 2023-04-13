using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _2048
{
    public partial class GameScreen : UserControl
    {
        SolidBrush black = new SolidBrush(Color.Black);
        SolidBrush yellow = new SolidBrush(Color.Yellow);
        Pen Borderline = new Pen(Color.FromArgb(64, 64, 64), 2);
        Font numberFont = new Font("SansSerif", 30, FontStyle.Bold);

        bool leftDown = false;
        bool rightDown = false;
        bool upDown = false;
        bool downDown = false;
        bool readyMove = true;

        public static int score = 0;
        public static int numMerges = 0;
        int randX, randY, randValue;
        int idCounter = 0;
        int delayTimer = 0;
        int spotsTaken = 0;
        Random random = new Random();

        List<Tile> tiles = new List<Tile>();
        List<Rectangle> tileAnimations = new List<Rectangle>();

        public GameScreen()
        {
            InitializeComponent();
            GeneratePosition();
            AddTile();
            score = 0;
            numMerges = 0;
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(black, 70, 70, 360, 360);

            e.Graphics.DrawRectangle(Borderline, 74, 74, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 163, 74, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 252, 74, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 341, 74, 85, 85);

            e.Graphics.DrawRectangle(Borderline, 74, 163, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 163, 163, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 252, 163, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 341, 163, 85, 85);

            e.Graphics.DrawRectangle(Borderline, 74, 252, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 163, 252, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 252, 252, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 341, 252, 85, 85);

            e.Graphics.DrawRectangle(Borderline, 74, 341, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 163, 341, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 252, 341, 85, 85);
            e.Graphics.DrawRectangle(Borderline, 341, 341, 85, 85);

            e.Graphics.FillRectangle(yellow, randX - 1, randY - 1, 87, 87);

            for (int i = 0; i < tiles.Count; i++)
            {
                //numberFont = new Font("SansSerif", 60 / Convert.ToString(tiles[i].value).Length, FontStyle.Bold);

                //e.Graphics.FillRectangle(tiles[i].tileColour, tileAnimations[i].X, tileAnimations[i].Y, 85, 85);
                //e.Graphics.DrawString(Convert.ToString(tiles[i].value), numberFont, black, tileAnimations[i].X, tileAnimations[i].Y + 15);
                e.Graphics.FillRectangle(tiles[i].tileColour, tiles[i].x, tiles[i].y, 85, 85);
                e.Graphics.DrawString(Convert.ToString(tiles[i].value), numberFont, black, tiles[i].x, tiles[i].y + 15);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (delayTimer == 0)
            {
                if (upDown)
                {
                    DoMove("up");
                }
                else if (downDown)
                {
                    DoMove("down");
                }
                else if (rightDown)
                {
                    DoMove("right");
                }
                else if (leftDown)
                {
                    DoMove("left");
                }
                readyMove = false;
            }

            if (delayTimer > 0 && readyMove == true)
            {
                delayTimer--;
            }

            TileAnimate();

            scoreLabel.Text = $"Score: {score}";

            Refresh();
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
            }
        }

        public void DoMove(string direction)
        {
            if (direction == "up")
            {
                DoMove(0, 163, 1, direction, "down");
                DoMove(0, 252, 2, direction, "down");
                DoMove(0, 341, 3, direction, "down");
            }
            else if (direction == "down")
            {
                DoMove(0, 252, 1, direction, "up");
                DoMove(0, 163, 2, direction, "up");
                DoMove(0, 74, 3, direction, "up");
            }
            else if (direction == "right")
            {
                DoMove(252, 0, 1, direction, "left");
                DoMove(163, 0, 2, direction, "left");
                DoMove(74, 0, 3, direction, "left");
            }
            else if (direction == "left")
            {
                DoMove(163, 0, 1, direction, "right");
                DoMove(252, 0, 2, direction, "right");
                DoMove(341, 0, 3, direction, "right");
            }

            AddTile();
            delayTimer = 10;
        }

        public void DoMove(int locationX, int loctationY, int i, string direction, string reverseDirection)
        {
            for (int t = 0; t < tiles.Count; t++)
            {
                if (tiles[t].x == locationX || tiles[t].y == loctationY)
                {
                    for (int j = 0; j < i; j++)
                    {
                        tiles[t].Move(direction);

                        foreach (Tile tile in tiles)
                        {
                            if (tiles[t].CollidesWith(tile))
                            {
                                if (tile.value != tiles[t].value)
                                {
                                    tiles[t].Move(reverseDirection);
                                }
                                else
                                {
                                    tiles[t].id = -1;
                                    tile.Upgrade();
                                    numMerges++;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            for (int t = 0; t < tiles.Count; t++)
            {
                if (tiles[t].id == -1)
                {
                    tiles.RemoveAt(t);
                    tileAnimations.RemoveAt(t);
                }
            }
        }

        public void AddTile()
        {
            int i = 0;
            idCounter++;

            GeneratePosition();
            spotsTaken = 0;

            foreach (Tile t in tiles)
            {
                if (randX == t.x && randY == t.y)
                {
                    spotsTaken++;
                }
            }

            while (spotsTaken != 0 && i <= 1000)
            {
                GeneratePosition();
                spotsTaken = 0;

                foreach (Tile t in tiles)
                {
                    if (randX == t.x && randY == t.y)
                    {
                        spotsTaken++;
                    }
                }
                i++;
            }

            if (i >= 1000 || tiles.Count > 17)
            {
                gameTimer.Stop();

                Form1.ChangeScreen(this, new GameOverScreen());
            }

            if (random.Next(1, 5) == 4)
            {
                randValue = 4;
            }
            else
            {
                randValue = 2;
            }


            tiles.Add(new Tile(randValue, randX, randY, idCounter));
            tileAnimations.Add(new Rectangle(randX, randY, 1, 1));
        }

        public void GeneratePosition()
        {
            randX = random.Next(1, 5);

            if (randX == 1)
            {
                randX = 74;
            }
            else if (randX == 2)
            {
                randX = 163;
            }
            else if (randX == 3)
            {
                randX = 252;
            }
            else
            {
                randX = 341;
            }

            randY = random.Next(1, 5);

            if (randY == 1)
            {
                randY = 74;
            }
            else if (randY == 2)
            {
                randY = 163;
            }
            else if (randX == 3)
            {
                randY = 252;
            }
            else
            {
                randY = 341;
            }
        }

        public void TileAnimate()
        {
            int movingTiles = 0;
            //int x;
            //int y;
            //for (int i = 0; i < tiles.Count; i++)
            //{
            //    if (tileAnimations[i].X != tiles[i].x && tileAnimations[i].Y != tiles[i].y)
            //    {
            //        movingTiles++;
            //        for (int j = 0; j < 50; j++)
            //        {
            //            if (tiles[i].x > tileAnimations[i].X)
            //            {
            //                x = tileAnimations[i].X + 1;
            //                y = tileAnimations[i].Y;
            //                tileAnimations[i] = new Rectangle(x, y, 1, 1);
            //            }
            //            else if (tiles[i].x < tileAnimations[i].X)
            //            {
            //                x = tileAnimations[i].X - 1;
            //                y = tileAnimations[i].Y;
            //                tileAnimations[i] = new Rectangle(x, y, 1, 1);
            //            }
            //            else if (tiles[i].y > tileAnimations[i].Y)
            //            {
            //                x = tileAnimations[i].X;
            //                y = tileAnimations[i].Y + 1;
            //                tileAnimations[i] = new Rectangle(x, y, 1, 1);
            //            }
            //            else if (tiles[i].y < tileAnimations[i].Y)
            //            {
            //                x = tileAnimations[i].X;
            //                y = tileAnimations[i].Y - 1;
            //                tileAnimations[i] = new Rectangle(x, y, 1, 1);
            //            }
            //        }
            //    }
            //}
            if (movingTiles == 0)
            {
                readyMove = true;
            }
        }
    }
}
