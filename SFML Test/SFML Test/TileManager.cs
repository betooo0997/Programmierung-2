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

namespace Game
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
        protected static int tileSize = 50;

        protected int numberColumns;
        protected int numberRows;

        protected string[] stringCurrentLevel;
        protected Tilez[,] currentLevel;

        protected Texture tTileSheet;
        protected Sprite tileSheet;


        public TileManager(string[] levelText)
        {
            tTileSheet = ContentLoader.textureTileSheet;
            tileSheet = new Sprite(tTileSheet);

            stringCurrentLevel = levelText;

            TileMapCreation();
        }

        protected void TileMapCreation()
        {


            // Determines the number of Columns for the current level depending on the longest line in the source.
            for (int x = 0; x < stringCurrentLevel.Length; x++)
            {
                if (stringCurrentLevel[x].Length > numberColumns)
                {
                    numberColumns = stringCurrentLevel[x].Length;
                }
            }


            // Determines the number of Rows for the current level depending on the number of Rows in the source.
            numberRows = stringCurrentLevel.Length;


            currentLevel = new Tilez[numberColumns, numberRows];


            // Creates the Tile Array for the Tile Manager out off the source.
            int xCoord = 0;
            int yCoord = 0;


            while (yCoord < stringCurrentLevel.Length && xCoord <= stringCurrentLevel[yCoord].Length)
            {
                currentLevel[xCoord, yCoord] = TileConversation(stringCurrentLevel[yCoord][xCoord]);
                Console.WriteLine(stringCurrentLevel[yCoord][xCoord]);

                xCoord++;
                if (xCoord >= stringCurrentLevel[yCoord].Length)
                {
                    xCoord = 0;
                    yCoord++;
                }
            }
        }

        protected Tilez TileConversation(Char tile)
        {
            switch (tile)
            {
                case '3':
                    return Tilez.black;
                case '2':
                    return Tilez.darkGrey;
                case '1':
                    return Tilez.grey;
                default:
                    return Tilez.white;
            }
        }


        public void Update()
        {
        }


        // Determines the source rectangle on the tile sheet. 
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

        // Draw the Tiles denpending on the upper parameter. 
        public void Draw(RenderWindow window, Vector2f TileMapPosition)
        {
            int yCoord = 0;
            int xCoord = 0;

            for (int x = 0; x < (numberColumns * numberRows); x++)
            {
                tileSheet.Position = new Vector2f(((int)(xCoord * tileSize + TileMapPosition.X)), (int)((yCoord * tileSize + TileMapPosition.Y)));
                tileSheet.TextureRect = TileSourceDeterminat0r(currentLevel[xCoord, yCoord]);

                window.Draw(tileSheet);

                xCoord++;
                if (xCoord >= numberColumns)
                {
                    xCoord = 0;
                    yCoord++;
                }
            }
        }
    }
}