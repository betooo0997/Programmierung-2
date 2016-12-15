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
        public Enemy[,] GetEnemyArray()
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
        /// Requires the Instance of TileManager und the .txt file used to create the current level. 
        /// </summary>
        /// <param name="tileManager"></param>
        /// <param name="enemyLayout"></param>
        public EntityManager(TileManager tileManager, string[] enemyLayout)
        {
            entityArrayCreation = new EntityArrayCreation(tileManager, enemyLayout);
        }

        /// <summary>
        /// Returns a list of Enemies created in dependency of the initial Entity array, so Enemies get their type and spawn location. Ignores all non-hostile Entities. 
        /// </summary>
        /// <returns></returns>
        public List<Enemy> ReturnListCreatedOutOfArray()
        {
            List<Enemy> list = new List<Enemy>();

            for (int x = 0, y = 0; y < GetArrayNumberRows(); x++)
            {
                if(GetEnemyArray()[x, y] != null)
                {
                    list.Add(GetEnemyArray()[x, y]);
                }

                if(x >= GetArrayNumberColumns())
                {
                    x = -1;
                    y++;
                }
            }

            return list;
        }
    }
}
