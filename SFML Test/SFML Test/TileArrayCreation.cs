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
        protected static int iTileSize = 50;
        /// <summary>
        /// Is generated in the constructor depending on the longest line in the source file (exactly: Biggest number of chars in the longest entry of the received string array). 
        /// </summary>
        protected static int iNumberColumns;
        /// <summary>
        /// Is generated in the consctructor depending on the number of lines in the source file (exactly: Number of entries in the received string array). 
        /// </summary>
        protected static int iNumberRows;
        /// <summary>
        /// Return value 
        /// </summary>
        protected static Tilez[,] e2CurrentLevel;

        /// <summary>
        /// Returns the two dimensional Tilez array. 
        /// </summary>
        /// <returns></returns>
        public Tilez[,] GetTilezArray()
        {
            return e2CurrentLevel;
        }

        /// <summary>
        /// Returns a positive static integer mirroring the standard tile size. 
        /// </summary>
        /// <returns></returns>
        public int GetTileSize()
        {
            return iTileSize;
        }

        /// <summary>
        /// Returns an integer showing the number of columns of the current level Tilez array. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberColumns()
        {
            return iNumberColumns;
        }

        /// <summary>
        /// Returns an integer showing the number of rows of the current level Tilez array. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberRows()
        {
            return iNumberRows;
        }

        /// <summary>
        /// Constructor receives the level data in the form of a string array and converts it into a tile array. After this, the array is not intended to be changed for this running session. 
        /// </summary>
        /// <param name="stringCurrentLevel"></param>
        public TileArrayCreation(string[] stringCurrentLevel)
        {
            iNumberColumns = 0;
            // Determines the number of Columns for the current level depending on the longest line in the source.
            for (int x = 0; x < stringCurrentLevel.Length; x++)
            {
                if (stringCurrentLevel[x].Length > iNumberColumns)
                {
                    iNumberColumns = stringCurrentLevel[x].Length;
                }
            }


            // Determines the number of Rows for the current level depending on the number of Rows in the source.
            iNumberRows = stringCurrentLevel.Length;


            e2CurrentLevel = new Tilez[iNumberColumns, iNumberRows];


            // Creates the Tile Array for the Tile Manager out off the source.
            int xCoord = 0;
            int yCoord = 0;


            while (yCoord < stringCurrentLevel.Length && xCoord < stringCurrentLevel[yCoord].Length)
            {
                e2CurrentLevel[xCoord, yCoord] = TileConversation(stringCurrentLevel[yCoord][xCoord]);

                xCoord++;
                if (xCoord >= stringCurrentLevel[yCoord].Length)
                {
                    xCoord = 0;
                    yCoord++;
                }
            }

            // Ensures that the player does not spawn on a tile with collision. 
            if (iNumberColumns >= 18 && iNumberRows >= 10)
            {
                if (CollisionReturner(e2CurrentLevel[18, 10]))
                {
                    e2CurrentLevel[18, 10] = Tilez.groundStone;
                }
            }
        }

        /// <summary>
        /// A hardcoded List of all available Types of Tiles. This method determines specific tile types in the array and is soly used by the consctructor of this class. 
        /// </summary>
        /// <param name="chTile"></param>
        /// <returns></returns>
        protected Tilez TileConversation(Char chTile)
        {
            switch (chTile)
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
        /// Soly to return a collision bool at specific coordinates if the tile map array. Unused coordinates, like negative values or too big ones, always return true. 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static bool CollisionReturner(int iXCoord, int iYCoord)
        {
            if(iXCoord < 0 || iXCoord >= iNumberColumns || iYCoord < 0 || iYCoord >= iNumberRows)
            {
                return true;
            }
            else 
            switch (e2CurrentLevel[iXCoord, iYCoord])
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

        /// <summary>
        /// Slightly alternated list of tiles with collision to match projectile requirements. Soly to return a collision bool at specific coordinates if the tile map array. Unused coordinates, like negative values or too big ones, always return true. 
        /// </summary>
        /// <param name="iXCoord"></param>
        /// <param name="iYCoord"></param>
        /// <returns></returns>
        public static bool CollisionReturnerProjectiles(int iXCoord, int iYCoord)
        {
            if (iXCoord < 0 || iXCoord >= iNumberColumns || iYCoord < 0 || iYCoord >= iNumberRows)
            {
                return true;
            }
            else
                switch (e2CurrentLevel[iXCoord, iYCoord])
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

        /// <summary>
        /// Returns collision bool in dependency of the given Tilez type 
        /// </summary>
        /// <param name="eTile"></param>
        /// <returns></returns>
        public static bool CollisionReturner(Tilez eTile)
        {
            switch (eTile)
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
