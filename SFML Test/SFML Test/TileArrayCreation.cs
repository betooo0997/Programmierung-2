using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Everywhere used enum to specify different types of tiles. 
    /// </summary>
    public enum Tilez
    {
        black = 0,
        darkGrey = 1,
        grey = 2,
        white = 3,
        groundGrass = 10,
        groundStone = 11,
        groundWood = 12,
        treeTrunk = 20,
        treeTop = 21,
        treeFoilage = 22,
        structureWood = 30,
        structureStone = 31,
        obstacleStone = 40,
        water = 41,
    }

    public class TileArrayCreation
    {
        /// <summary>
        /// Hardcoded and never changing value to determine ... well, your guess. 
        /// </summary>
        protected static int tileSize = 50;
        /// <summary>
        /// Is generated in the constructor depending on the longest line in the source file (exactly: Biggest number of chars in the longest entry of the received string array). 
        /// </summary>
        protected static int numberColumns;
        /// <summary>
        /// Is generated in the consctructor depending on the number of lines in the source file (exactly: Number of entries in the received string array). 
        /// </summary>
        protected static int numberRows;
        /// <summary>
        /// Return value 
        /// </summary>
        protected static Tilez[,] currentLevel;

   
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

        /// <summary>
        /// Constructor receives the level data in the form of a string array and converts it into a tile array. After this, the array is not intended to be changed for this running session. 
        /// </summary>
        /// <param name="stringCurrentLevel"></param>
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


            while (yCoord < stringCurrentLevel.Length && xCoord < stringCurrentLevel[yCoord].Length)
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

        /// <summary>
        /// A hardcoded List of all available Types of Tiles. This method determines specific tile types in the array and is soly used by the consctructor of this class. 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        protected Tilez TileConversation(Char tile)
        {
            switch (tile)
            {
                case 'e':
                    return Tilez.water;
                case 'o':
                    return Tilez.obstacleStone;
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

        /// <summary>
        /// Soly to return a collision bool. Unused coordinates in the array, like negative values or to big ones, always return false. 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static bool CollisionReturner(int xCoord, int yCoord)
        {
            if(xCoord < 0 || xCoord >= numberColumns || yCoord < 0 || yCoord >= numberRows)
            {
                return true;
            }
            else 
            switch (currentLevel[xCoord, yCoord])
            {
                case Tilez.water:
                    return true;
                case Tilez.obstacleStone:
                    return true;
                case Tilez.structureStone:
                    return true;
                case Tilez.structureWood:
                    return true;
                case Tilez.treeFoilage:
                    return true;
                case Tilez.treeTop:
                    return true;
                case Tilez.treeTrunk:
                    return true;
                default:
                    return false;
            }
        }

        public bool CollisionReturnerProjectiles(int xCoord, int yCoord)
        {
            if (xCoord < 0 || xCoord >= numberColumns || yCoord < 0 || yCoord >= numberRows)
            {
                return true;
            }
            else
                switch (currentLevel[xCoord, yCoord])
                {
                    case Tilez.obstacleStone:
                        return true;
                    case Tilez.structureStone:
                        return true;
                    case Tilez.structureWood:
                        return true;
                    case Tilez.treeFoilage:
                        return true;
                    case Tilez.treeTop:
                        return true;
                    case Tilez.treeTrunk:
                        return true;
                    default:
                        return false;
                }
        }

        public static bool CollisionReturner(Tilez Tile)
        {
            switch (Tile)
            {
                case Tilez.groundGrass:
                    return false;
                case Tilez.groundStone:
                    return false;
                case Tilez.groundWood:
                    return false;
                default:
                    return true;
            }
        }
    }
}
