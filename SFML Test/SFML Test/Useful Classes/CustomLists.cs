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
    public class CustomList
    {
        public List<Drawable> drawList;

        public CustomList()
        {
            drawList = new List<Drawable>();
        }


        public int Count()
        {
            int count = drawList.Count;

            return count;
        }

        public void AddElement(Drawable drawable)
        {
            drawList.Add(drawable);
        }


        public void AddList(List<Drawable> drawable)
        {
            for (int x = 0; x < drawable.Count; x++)
            {
                drawList.Add(drawable[x]);
            }
        }

        public static List<Drawable> AddProjectiles(List<Drawable> a, List<EnemyProjectile> b)
        {
            List<Drawable> mergedList;
            mergedList = a;

            for (int x = 0; x < b.Count; x++)
                mergedList.Add(b[x].Draw());

            return mergedList;
        }

        public static List<Drawable> AddProjectiles(List<Drawable> a, List<InvisibleProjectile> b)
        {
            List<Drawable> mergedList;
            mergedList = a;

            for (int x = 0; x < b.Count; x++)
                mergedList.Add(b[x].Draw());

            return mergedList;
        }

        public static List<Drawable> AddProjectiles(List<Drawable> a, List<PlayerProjectile> b)
        {
            List<Drawable> mergedList;
            mergedList = a;

            for (int x = 0; x < b.Count; x++)
                mergedList.Add(b[x].Draw());

            return mergedList;
        }


        public void RemoveAt(int index)
        {
            for (int x = index; x < drawList.Count; x++)
            {
                drawList[x] = drawList[x + 1];
            }

            drawList.RemoveAt(drawList.Count);
        }



        public List<Drawable> MergeLists(List<Drawable> a, List<Drawable> b, List<Drawable> c)
        {
            drawList = a;

            for (int x = 0; x <= b.Count; x++)
                drawList.Add(b[x]);

            for (int x = 0; x <= c.Count; x++)
                drawList.Add(c[x]);

            return drawList;
        }


        public List<Drawable> Draw()
        {
            return drawList;
        }
    }
}
