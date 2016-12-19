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
    /// Class Used for simplifying the use of Lists
    /// </summary>
    public class CustomList
    {
        /// <summary>
        /// List to be drawed
        /// </summary>
        public List<Drawable> lDrawList;


        /// <summary>
        /// Constructor
        /// </summary>
        public CustomList()
        {
            lDrawList = new List<Drawable>();
        }


        /// <summary>
        /// Counts the Elements of lDrawList
        /// </summary>
        public int Count()
        {
            return lDrawList.Count;
        }


        /// <summary>
        /// Adds an Element to the lDrawList
        /// </summary>
        /// <param name="Ddrawable">Drawable to be added to the List</param>
        public void AddElement(Drawable Ddrawable)
        {
            lDrawList.Add(Ddrawable);
        }


        /// <summary>
        /// Adds a List to the lDrawList
        /// </summary>
        /// <param name="lDrawList"></param>
        public void AddList(List<Drawable> lDrawList)
        {
            for (int x = 0; x < lDrawList.Count; x++)
            {
                this.lDrawList.Add(lDrawList[x]);
            }
        }


        /// <summary>
        /// Adds an EnemyProjectile List to a specified List
        /// </summary>
        /// <param name="lDrawList">DrawList</param>
        /// <param name="lEnemyProjectile">EnemyProjectile List that will be added to the DrawList</param>
        /// <returns>Merged List</returns>
        public static List<Drawable> AddProjectiles(List<Drawable> lDrawList, List<EnemyProjectile> lEnemyProjectile)
        {
            List<Drawable> mergedList;
            mergedList = lDrawList;

            for (int x = 0; x < lEnemyProjectile.Count; x++)
                mergedList.Add(lEnemyProjectile[x].Draw());

            return mergedList;
        }


        /// <summary>
        /// Adds a InvisibleProjectile List to a specified List
        /// </summary>
        /// <param name="lDrawList">DrawList</param>
        /// <param name="lInvisibleProjectile">InvisibleProjectile List that will be added to the DrawList</param>
        /// <returns>Merged List</returns>
        public static List<Drawable> AddProjectiles(List<Drawable> lDrawList, List<InvisibleProjectile> lInvisibleProjectile)
        {
            List<Drawable> mergedList;
            mergedList = lDrawList;

            for (int x = 0; x < lInvisibleProjectile.Count; x++)
                mergedList.Add(lInvisibleProjectile[x].Draw());

            return mergedList;
        }


        /// <summary>
        /// Adds a PlayerProjectile List to a specified List
        /// </summary>
        /// <param name="lDrawList">DrawList</param>
        /// <param name="lPlayerProjectile">PlayerProjectile List that will be added to the DrawList</param>
        /// <returns>Merged List</returns>
        public static List<Drawable> AddProjectiles(List<Drawable> lDrawList, List<PlayerProjectile> lPlayerProjectile)
        {
            List<Drawable> mergedList;
            mergedList = lDrawList;

            for (int x = 0; x < lPlayerProjectile.Count; x++)
                mergedList.Add(lPlayerProjectile[x].Draw());

            return mergedList;
        }


        /// <summary>
        /// Draws the lDrawList
        /// </summary>
        /// <returns>lDrawList</returns>
        public List<Drawable> Draw()
        {
            return lDrawList;
        }
    }
}
