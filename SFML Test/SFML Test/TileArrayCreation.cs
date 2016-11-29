using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum Tilez
    {
        black           =  0,
        darkGrey        =  1,
        grey            =  2,
        white           =  3,
        groundGrass     = 10,
        groundStone     = 11,
        groundWood      = 12,
        treeTrunk       = 20,
        treeTop         = 21,
        treeFoilage     = 22,
        structureWood   = 30,
        structureStone  = 31
    }

    public class TileArrayCreation
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

        // A hardcoded List of all available Types of Tiles. 
        protected Tilez TileConversation(Char tile)
        {
            switch (tile)
            {
                case 's':
                    return Tilez.structureStone;
                case 'w':
                    return Tilez.structureWood;
                case '+':
                    return Tilez.treeFoilage;
                case 'x':
                    return Tilez.treeTop;
                case 'u':
                    return Tilez.treeTrunk;
                case 'd':
                    return Tilez.groundWood;
                case 'p':
                    return Tilez.groundStone;
                case 'g':
                    return Tilez.groundGrass;
                case '0':
                    return Tilez.white;
                case '*':
                    return Tilez.grey;
                case '#':
                    return Tilez.darkGrey;
                default:
                    return Tilez.black;
            }
        }

    }
}
