using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

namespace Game
{
    class EntityManager
    {
        /// <summary>
        /// Instance of this class to create the initial entity array out a txt file. 
        /// </summary>
        protected EntityArrayCreation entityArrayCreation;

        /// <summary>
        /// Returns the entity array created by the included instance of entityArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public Entity[,] GetEntityArray()
        {
            return entityArrayCreation.GetEntityArray();
        }

        public int GetArrayNumberColumns()
        {
            return entityArrayCreation.GetNumberColumns();
        }

        public int GetArrayNumberRows()
        {
            return entityArrayCreation.GetNumberRows();
        }


        /// <summary>
        /// Requires the Instance of TileManager und the txt file used to create the current level. 
        /// </summary>
        /// <param name="tileManager"></param>
        /// <param name="enemyLayout"></param>
        public EntityManager(TileManager tileManager, string[] enemyLayout)
        {
            entityArrayCreation = new EntityArrayCreation(tileManager, enemyLayout);
        }

        /// <summary>
        /// Method tu opdate all Entities created at the start of the game. 
        /// </summary>
        /// <param name="CharacterPosition"></param>
        /// <param name="TileMapPosition"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        public void UpdateEntitySpawnedAt(Vector2f CharacterPosition, Vector2f TileMapPosition, bool up, bool down, bool right, bool left)
        {
            for(int x = 0, y = 0; y < GetArrayNumberColumns(); x++)
            {
                if(GetEntityArray()[x, y] != null)
                {

                }
            }
        }
    }
}
