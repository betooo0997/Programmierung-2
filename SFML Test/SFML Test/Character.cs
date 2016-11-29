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
    abstract class Character : Entity
    {
        protected int iHealth;
        public static float iSpeed = 1.5f;

        protected Projectile pProjectile;
        protected List<Projectile> lProjectile;
        protected List<Drawable> drawList;

        protected float iAngle;


        protected void DisposeProjectile()
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct())
                {
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);

                    Console.WriteLine("Projectile Destroyed");
                }
            }
        }
    }
}