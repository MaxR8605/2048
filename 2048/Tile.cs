using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2048
{
    internal class Tile
    {
        public int value, x, y, id;

        public SolidBrush tileColour = new SolidBrush(Color.Black);

        public Tile(int _value, int _x, int _y, int _id)
        {
            value = _value;
            x = _x;
            y = _y;
            id = _id;

            if (value == 2)
            {
                tileColour.Color = Color.FromArgb(180, 180, 180);
            }
            else
            {
                tileColour.Color = Color.FromArgb(200, 180, 160);
            }
        }
        
        public void Move(string direction)
        {
            if (direction == "up")
            {
                y -= 89;
            }
            else if (direction == "down")
            {
                y += 89;
            }
            else if (direction == "right")
            {
                x += 89;
            }
            else if (direction == "left")
            {
                x -= 89;
            }
        }

        public bool CollidesWith(Tile otherTile)
        {
            Rectangle tile1 = new Rectangle(x, y, 85, 85);
            Rectangle tile2 = new Rectangle(otherTile.x, otherTile.y, 85, 85);

            if (tile1.IntersectsWith(tile2) && id != otherTile.id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Upgrade()
        {
            value *= 2;
            GameScreen.score += value;

            if (value == 2)
            {
                tileColour.Color = Color.FromArgb(180, 180, 180);
            }
            else if (value == 4)
            {
                tileColour.Color = Color.FromArgb(200, 180, 160);
            }
            else if (value == 8)
            {
                tileColour.Color = Color.FromArgb(250, 120, 100);
            }
            else if (value == 16)
            {
                tileColour.Color = Color.FromArgb(230, 70, 0);
            }
            else if (value == 32)
            {
                tileColour.Color = Color.FromArgb(235, 60, 50);
            }
            else if (value == 64)
            {
                tileColour.Color = Color.FromArgb(255, 0, 0);
            }
            else if (value == 128)
            {
                tileColour.Color = Color.FromArgb(250, 230, 150);
            }
            else if (value == 256)
            {
                tileColour.Color = Color.FromArgb(250, 200, 100);
            }
            else if (value == 512)
            {
                tileColour.Color = Color.FromArgb(255, 180, 50);
            }
            else if (value == 1024)
            {
                tileColour.Color = Color.FromArgb(255, 160, 0);
            }
            else if (value == 2048)
            {
                tileColour.Color = Color.FromArgb(255, 100, 0);
            }
        }
    }
}
