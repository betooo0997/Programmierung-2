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
        public static float iSpeed = 1.5f;
        protected List<Drawable> drawList;

        /// <summary>
        /// Vector to correct the Characters Position in case of Collision
        /// </summary>
        protected Vector2f vChracterPositionSpace;



        protected float fAngle;

        /// <summary>
        /// Disposes a Projectile from the Projectile List if its Destruct bool is true
        /// </summary>
        protected void DisposeProjectile(List<PlayerProjectile> lProjectile, uint Damage)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct() || EnemyProjectileCollision(lProjectile[x], Damage))
                {
                    lProjectile[x].DisposeTexture();
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);
                }
            }
        }

        protected void DisposeProjectile(List<EnemyProjectile> lProjectile, uint Damage)
        {
            for (int x = 0; x < lProjectile.Count; x++)
            {
                if (lProjectile[x].Destruct() || PlayerProjectileCollision(lProjectile[x], Damage))
                {
                    lProjectile[x].DisposeTexture();
                    for (int y = x; y + 1 < lProjectile.Count; y++)
                        lProjectile[y] = lProjectile[y + 1];

                    if (lProjectile.Count == 1)
                        lProjectile.RemoveAt(0);
                    else
                        lProjectile.RemoveAt(lProjectile.Count - 1);
                }
            }
        }

        protected void DisposeProjectile(List<InvisibleProjectile> lProjectile)
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
                }
            }
        }

        protected bool PlayerProjectileCollision(EnemyProjectile iProjectile, uint Damage)
        {
            Vector2f b = MainMap.GetStartCharacterPosition() + new Vector2f(25, 25);

            if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 50)
            {
                Player.ReduceHealth(Damage);
                return true;
            }

            return false;
        }

        protected bool EnemyProjectileCollision(PlayerProjectile iProjectile, uint Damage)
        {
            List<Enemy> lEnemy;
            lEnemy = MainMap.GetEnemies();

            for (int x = 0; x < lEnemy.Count; x++)
            {
                Vector2f b = lEnemy[x].sEntity.Position - new Vector2f(25, 25);

                if (Utilities.DistanceBetweenVectors(iProjectile.vEntityPosition, b) <= 50)
                {
                    lEnemy[x].ReduceHealth(Damage);
                    return true;
                }
            }
            return false;
        }
    }
}