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
        /// <summary>
        /// TriangleCivil Appearance
        /// </summary>
        TriangleCivil =   0,

        /// <summary>
        /// TriangleBandit1 Appearance
        /// </summary>
        TriangleBandit1 =   1,

        /// <summary>
        /// TriangleBandit2 Appearance
        /// </summary>
        TriangleBandit2 =   2,

        /// <summary>
        /// TriangleBandit3 Appearance
        /// </summary>
        TriangleBandit3 =   3,

        /// <summary>
        /// TriangleBrute Appearance
        /// </summary>
        TriangleBrute =   4,

        /// <summary>
        /// TriangleBomber Appearance
        /// </summary>
        TriangleBomber =   5,

        /// <summary>
        /// TriangleLord Appearance
        /// </summary>
        TriangleLord =   6,

        /// <summary>
        /// SquareCivil Appearance
        /// </summary>
        SquareCivil =  10,

        /// <summary>
        /// SquareSoldier1 Appearance
        /// </summary>
        SquareSoldier1 =  11,

        /// <summary>
        /// SquareSoldier2 Appearance
        /// </summary>
        SquareSoldier2 =  12,

        /// <summary>
        /// SquareSoldier3 Appearance
        /// </summary>
        SquareSoldier3 =  13,

        /// <summary>
        /// SquareCommander Appearance
        /// </summary>
        SquareCommander =  14,

        /// <summary>
        /// SquareGeneral Appearance
        /// </summary>
        SquareGeneral =  15,

        /// <summary>
        /// PentagonCivil Appearance
        /// </summary>
        PentagonCivil =  20,

        /// <summary>
        /// PentagonCenturio Appearance
        /// </summary>
        PentagonCenturio =  21,

        /// <summary>
        /// ProjectileVector Appearance
        /// </summary>
        ProjectileVector = 100,

        /// <summary>
        /// ProjectileEdge Appearance
        /// </summary>
        ProjectileEdge = 101
    }

    /// <summary>
    /// Uses EntityArrayCreation to make the enemy array. Is able to return it in form of a list. 
    /// </summary>
    class EntityManager
    {
        /// <summary>
        /// Instance of this class to create the initial entity array out a txt file. 
        /// </summary>
        protected EntityArrayCreation entityArrayCreation;

        /// <summary>
        /// Returns the two dimensional entity array created by the included instance of entityArrayCreation. 
        /// </summary>
        /// <returns></returns>
        public Enemy[,] GetEnemyArray()
        {
            return entityArrayCreation.GetEntityArray();
        }

        /// <summary>
        /// Returns an integer reflecting the number of Columns of the enemy array created by the underlying Creator. 
        /// </summary>
        /// <returns></returns>
        public int GetArrayNumberColumns()
        {
            return entityArrayCreation.GetNumberColumns();
        }

        /// <summary>
        /// Returns an integer reflecting the number of Rows of the enemy array created by the underlying Creator. 
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
        /// <param name="sEnemyLayout"></param>
        public EntityManager(TileManager tileManager, string[] sEnemyLayout)
        {
            entityArrayCreation = new EntityArrayCreation(tileManager, sEnemyLayout);
        }

        /// <summary>
        /// Returns a list of Enemies created in dependency of the initial Entity array, so Enemies get their type and spawn location. 
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
