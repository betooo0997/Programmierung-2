using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dopsball
{
    public enum Tilez
    {
        white,
        grey,
        darkGrey,
        black
    }

    class TileManager
    {
        protected static uint numberColumns =                       16;
        protected static uint numberRows =                          12;

        protected static int tileSize =                             50;

        protected static Vector2f startingPoint = new Vector2f   (0,0);

        protected Tilez[,] level = new Tilez[numberColumns, numberRows];
        

        public TileManager()
        {
            level[6, 2] = Tilez.black;
            level[9, 2] = Tilez.black;
            level[7, 4] = Tilez.darkGrey;
            level[8, 4] = Tilez.darkGrey;
            level[7, 5] = Tilez.darkGrey;
            level[8, 5] = Tilez.darkGrey;
            level[6, 6] = Tilez.darkGrey;
            level[7, 6] = Tilez.darkGrey;
            level[8, 6] = Tilez.darkGrey;
            level[9, 6] = Tilez.darkGrey;
            level[4, 8] = Tilez.grey;
            level[11, 8] = Tilez.grey;
            level[5, 9] = Tilez.grey;
            level[6, 9] = Tilez.grey;
            level[7, 9] = Tilez.grey;
            level[8, 9] = Tilez.grey;
            level[9, 9] = Tilez.grey;
            level[10, 9] = Tilez.grey;
        }

        
        public void Update()
        {

            Collision();
        }

        public bool Collision()
        {
            return false;
        }

        protected IntRect TileSourceDeterminat0r(Tilez tile)
        {
            switch (tile)
                {
                case Tilez.black:
                    return new IntRect(1 * tileSize, 1 * tileSize, tileSize, tileSize);
                case Tilez.darkGrey:
                    return new IntRect(0 * tileSize, 1 * tileSize, tileSize, tileSize);
                case Tilez.grey:
                    return new IntRect(1 * tileSize, 0 * tileSize, tileSize, tileSize);
                default:
                    return new IntRect(0 * tileSize, 0 * tileSize, tileSize, tileSize);
                }
        }
        
        public void Draw(RenderWindow window, Sprite tileSheet)
        {
            int yCoord = 0;
            int xCoord = 0;

            for (int x = 0; x < (numberColumns * numberRows); x++)
            {
                tileSheet.Position = new Vector2f(startingPoint.X + (xCoord * tileSize), startingPoint.Y + (yCoord * tileSize));
                tileSheet.TextureRect = TileSourceDeterminat0r(level[xCoord, yCoord]);

                window.Draw(tileSheet);

                xCoord++;
                if(xCoord >= numberColumns)
                {
                    xCoord = 0;
                    yCoord++;
                }
            }
        }

    }
}
