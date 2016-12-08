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
    class EntityArrayCreation
    {
        /// <summary>
        /// Generated out off the values the tile array in the constructor. 
        /// </summary>
        protected int numberColumns;
        /// <summary>
        /// Generated out off the values the tile array in the constructor. 
        /// </summary>
        protected int numberRows;

        /// <summary>
        /// Created dependant on the underlying tile map. Ensured to not spawn entities on tiles with collision or out of the map boarders. 
        /// </summary>
        protected Entity[,] entityArray;


        public Entity[,] GetEntityArray()
        {
            return entityArray;
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
        /// Creation of the entity array happens in the constructor and is used to spawn enemies at the start of a game. Depends on the underlying tile array and a specific string array. Entities placed out off the maps boarders or on tiles with collision are ignored in the creation process. 
        /// </summary>
        /// <param name="tileManager"></param>
        /// <param name="stringEnemyLayout"></param>
        public EntityArrayCreation(TileManager tileManager, string[] stringEnemyLayout)
        {
            numberColumns = tileManager.GetNumberColumns();
            numberRows = tileManager.GetNumberRows();

            entityArray = new Entity[numberColumns, numberRows];


            for(int x = 0, y = 0; y < numberRows; x++)
            {
                if (y < stringEnemyLayout.Length && x < stringEnemyLayout[y].Length)
                {
                    entityArray[x, y] = EntityConversation(stringEnemyLayout[y][x], tileManager, x, y);
                }

                if(x >= numberColumns)
                {
                    x = 0;
                    y++;
                }
            }
        }

        /// <summary>
        /// Returns an entity depending on the used char. Default is null. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tileManager"></param>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <returns></returns>
        protected Entity EntityConversation(char type, TileManager tileManager, int xCoord, int yCoord)
        {
            if (!tileManager.GetCollisionAt(xCoord, yCoord))
            {
                switch (type)
                {
                    case ('a'):
                        return new Archer(new Vector2f((float)(xCoord * tileManager.GetTileSize()), (float)(yCoord * tileManager.GetTileSize())));
                    default:
                        return null;
                }
            }
            else
                return null;
        }

    }
}
