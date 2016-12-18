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
    /// <summary>
    /// Public enum used to save and determine Entity appareance, except for the players one. 
    /// </summary>
    public enum EntityAppearance
    {
        TriangleCivil       =   0, 
        TriangleBandit1     =   1,
        TriangleBandit2     =   2,
        TriangleBandit3     =   3,
        TriangleBrute       =   4,
        TriangleBomber      =   5,
        TriangleLord        =   6,
        SquareCivil         =  10,
        SquareSoldier1      =  11,
        SquareSoldier2      =  12,
        SquareSoldier3      =  13,
        SquareCommander     =  14,
        SquareGeneral       =  15,
        PentagonCivil       =  20,
        PentagonCenturio    =  21,
        ProjectileVector    = 100,
        ProjectileEdge      = 101
    }

    class EntityManager
    {
        /// <summary>
        /// Instance of this class to create the initial entity array out a txt file. 
        /// </summary>
        protected EntityArrayCreation entityArrayCreation;

        /// <summary>
        /// Fixed size of the entity sprites. 
        /// </summary>
        protected static int entitySize = 50;

        /// <summary>
        /// Sprite Sheet used to draw all entities. 
        /// </summary>
        protected Sprite entitySheet;

        /// <summary>
        /// Returns the entity array created by the included instance of entityArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public Enemy[,] GetEnemyArray()
        {
            return entityArrayCreation.GetEntityArray();
        }

        /// <summary>
        /// Returns the number of Columns of the enemy array created by the underlying Creator. 
        /// </summary>
        /// <returns></returns>
        public int GetArrayNumberColumns()
        {
            return entityArrayCreation.GetNumberColumns();
        }

        /// <summary>
        /// Returns the number of Rows of the enemy array created by the underlying Creator. 
        /// </summary>
        /// <returns></returns>
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
