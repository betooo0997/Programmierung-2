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
    /// <summary>
    /// Use to create an array of enemies out of the underlying tile map and the chosen enemy layout. 
    /// </summary>
    class EntityArrayCreation
    {
        /// <summary>
        /// Generated out off the values the tile array in the constructor. 
        /// </summary>
        protected int iNumberColumns;

        /// <summary>
        /// Generated out off the values the tile array in the constructor. 
        /// </summary>
        protected int iNumberRows;
        
        /// <summary>
        /// Created dependant on the underlying tile map. Ensured to not spawn entities on tiles with collision or out of the map boarders. 
        /// </summary>
        protected Enemy[,] enemy2Array;

        /// <summary>
        /// Number used to mark created Enemies. 
        /// </summary>
        protected uint uiId = 1;

        /// <summary>
        /// Random function soly used to randomize appearance of certain entities. 
        /// </summary>
        Random rRandom;

        /// <summary>
        /// Returns the two dimensional entity array created out of the chosen .txt file. 
        /// </summary>
        /// <returns></returns>
        public Enemy[,] GetEntityArray()
        {
            return enemy2Array;
        }
        
        /// <summary>
        /// Returns an integer reflecting the number of columns of the created entity array. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberColumns()
        {
            return iNumberColumns;
        }

        /// <summary>
        /// Returns an integer reflecting the number of rows of the created entity array. 
        /// </summary>
        /// <returns></returns>
        public int GetNumberRows()
        {
            return iNumberRows;
        }
        

        /// <summary>
        /// Creation of the entity array happens in the constructor and is used to spawn enemies at the start of a game. Depends on the underlying tile array and a specific string array. Entities placed out off the maps boarders or on tiles with collision are ignored in the creation process. 
        /// </summary>
        /// <param name="tileManager"></param>
        /// <param name="stringEnemyLayout"></param>
        public EntityArrayCreation(TileManager tileManager, string[] stringEnemyLayout)
        {
            rRandom = new Random();

            iNumberColumns = tileManager.GetNumberColumns();
            iNumberRows = tileManager.GetNumberRows();

            enemy2Array = new Enemy[iNumberColumns + 1, iNumberRows];

            uiId = 1;

            for(int x = 0, y = 0; y < iNumberRows; x++)
            {
                if (y < stringEnemyLayout.Length && x < stringEnemyLayout[y].Length)
                {
                    enemy2Array[x, y] = EnemyConversation(stringEnemyLayout[y][x], tileManager, x, y);
                }

                if(x >= iNumberColumns)
                {
                    x = -1;
                    y++;
                }
            }
        }

        /// <summary>
        /// Returns an entity depending on the used char and gives them correspondening values. Default is null. 
        /// </summary>
        /// <param name="chType"></param>
        /// <param name="tileManager"></param>
        /// <param name="iXCoord"></param>
        /// <param name="iYCoord"></param>
        /// <returns></returns>
        protected Enemy EnemyConversation(char chType, TileManager tileManager, int iXCoord, int iYCoord)
        {
            if (!tileManager.GetCollisionAt(iXCoord, iYCoord))
            {
                switch (chType)
                {
                    case ('+'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.PentagonCenturio, 20, 400, false, 140);
                    case ('-'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.PentagonCivil, 5, 200, false, 40);
                    case ('3'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.SquareGeneral, 25, 800, true, 200);
                    case ('2'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.SquareCommander, 20, 900, false, 100);
                    case ('1'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, AppearanceRandomizer(4), 18, 600, false, 80);
                    case ('0'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.SquareCivil, 5, 200, false, 40);
                    case ('d'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.TriangleLord, 25, 600, true, 120);
                    case ('c'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.TriangleBomber, 25, 800, false, 60);
                    case ('b'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.TriangleBrute, 20, 450, false, 100);
                    case ('a'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, AppearanceRandomizer(3), 15, 600, false, 60);
                    case ('z'):
                        uiId++;
                        return new Archer(new Vector2f((float)(iXCoord * tileManager.GetTileSize()), (float)(iYCoord * tileManager.GetTileSize())), uiId - 1, EntityAppearance.TriangleCivil, 5, 200, false, 40);
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Function used to give entities with optional appearances one. Input case 4 stands for Square Soldiers. Every other case will be Triangle Bandits. 
        /// </summary>
        /// <returns></returns>
        protected EntityAppearance AppearanceRandomizer(int iCase)
        {
            int iRNGesus = rRandom.Next(1, 4);

            if(iCase == 4)
            {
                switch (iRNGesus)
                {
                    case 1:
                        return EntityAppearance.SquareSoldier1;
                    case 2:
                        return EntityAppearance.SquareSoldier2;
                    default:
                        return EntityAppearance.SquareSoldier3;
                }
            }
            else
            switch (iRNGesus)
            {
                case 1:
                    return EntityAppearance.TriangleBandit1;
                case 2:
                    return EntityAppearance.TriangleBandit2;
                default:
                    return EntityAppearance.TriangleBandit3;
            }

        }
    }
}
