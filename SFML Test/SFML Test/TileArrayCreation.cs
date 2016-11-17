using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum Tilez
    {
        white,
        grey,
        darkGrey,
        black
    }

    class TileArrayCreation
    {
        protected static int tileSize = 50;

        protected int numberColumns;
        protected int numberRows;
        
        protected Tilez[,] currentLevel;

   
        public Tilez[,] GetTilezArray()
        {
            return currentLevel;
        }

        public int GetTileSize()
        {
            return tileSize;
        }

        public int GetNumberColumns()
        {
            return numberColumns;
        }

        public int GetNumberRows()
        {
            return numberRows;
        }

        public TileArrayCreation(string[] stringCurrentLevel)
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

    }
}
